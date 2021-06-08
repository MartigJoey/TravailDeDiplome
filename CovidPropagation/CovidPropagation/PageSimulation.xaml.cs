using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using LiveCharts.Wpf;
using LiveCharts;
using System.ComponentModel;
using System.Reflection;
using LiveCharts.Defaults;
using System.Linq;
using LiveCharts.Geared;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO.Pipes;
using System.IO;
using System.Text.Json;

namespace CovidPropagation
{
    /// <summary>
    /// Logique d'interaction pour PageSimulation.xaml
    /// </summary>
    public partial class PageSimulation : Page
    {
        // Unity Process
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        WindowRawDatas rawDatasWindow;
        MainWindow mw;
        public ChartData[,] chartDatas; // Contient les données des graphiques avec une position x et y
        Simulation sim;
        Dictionary<UIType, object> charts;
        Grid grdStruct;

        System.Windows.Forms.Integration.WindowsFormsHost wfhUnityHost; // Contient le panel Unity
        System.Windows.Forms.Panel panelUnity; // Affiche la fenêtre unity

        private Process process; // Process contenant le programme Unity
        private IntPtr unityHWND = IntPtr.Zero; // Handler d'unity
        NamedPipeServerStream pipeServer; // Permet la création du pipeline connectant WPF à Unity
        StreamString ss; // Stream permettant l'envoie de données à Unity
        Thread server;

        public PageSimulation()
        {
            InitializeComponent();
            rawDatasWindow = new WindowRawDatas();
            mw = (MainWindow)Application.Current.MainWindow;
            charts = new Dictionary<UIType, object>();
            sim = new Simulation();
        }

        private void OpenRawDatasWindow_Click(object sender, RoutedEventArgs e)
        {
            rawDatasWindow.Show();
            rawDatasWindow.Focus();
        }

        /// <summary>
        /// Modifie l'interval et donc la vitesse de la simulation en fonction de la valeur du slider.
        /// </summary>
        private void IntervalSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            sim.Interval = Convert.ToInt32(intervalSlider.Maximum - intervalSlider.Value);
        }

        /// <summary>
        /// Créé la simulation si celle-ci n'est pas initialisée ou relance le timer.
        /// </summary>
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (!sim.IsInitialized)
            {
                sim.Initialize();
                sim.Interval = GlobalVariables.DEFAULT_INTERVAL;

                if(panelUnity != null)
                {
                    sim.OnGUIUpdate += new GUIDataEventHandler(OnGUIUpdate);
                    sim.OnGUIInitialize += new InitializeGUIEventHandler(OnGUIInitialize);
                }

                rawDatasWindow.CreateLabels(sim.GetAllDatas(0,0,0,0,0));
                sim.OnDataUpdate += new DataUpdateEventHandler(rawDatasWindow.UpdateLabels);

                if (panelUnity == null)
                {
                    StartSimulationIteration();
                }
                else
                {
                    LoadUnityExe();
                    ConnectToUnity(); // Connexion à unity puis démarrage de l'itération.
                }

                intervalSlider.Value = Convert.ToInt32(intervalSlider.Maximum - sim.Interval);
                mw.btnGraphicSettings.IsEnabled = false;
                btnOpenRawDatas.IsEnabled = true;
            }
            sim.Start();
        }

        private void StartSimulationIteration()
        {
            Task.Factory.StartNew(() => sim.Iterate());
        }

        /// <summary>
        /// Met en pause la simulation
        /// </summary>
        private void Break_Click(object sender, RoutedEventArgs e)
        {
            sim.Stop();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            sim.Stop();
            sim = new Simulation();
            if (chartDatas == null)
                chartDatas = new ChartData[0,0];

            for (int x = 0; x < chartDatas.GetLength(0); x++)
            {
                for (int y = 0; y < chartDatas.GetLength(1); y++)
                {
                    chartDatas[x, y].DisplayWindow = 0;
                }
            }
            
            grdContent.Children.Clear();

            if (panelUnity != null)
            {
                CloseUnity();
            }

            SetGrid(grdStruct, chartDatas);
            mw.btnGraphicSettings.IsEnabled = true;
        }

        #region View

        /// <summary>
        /// Modifie la grille de cette page pour qu'elle correspondent à celle modifiée dans les paramètres graphiques.
        /// </summary>
        /// <param name="grd">Grille contenant les colonnes et lignes à afficher.</param>
        /// <param name="chartDatas">Données des graphiques à afficher dans la grille.</param>
        public void SetGrid(Grid grd, ChartData[,] chartDatas)
        {
            this.chartDatas = chartDatas;
            grdStruct = grd;
            grdContent = grd;
            slvScroller.Content = grdContent;
            charts.Clear();
            DisplayUI();
        }

        /// <summary>
        /// Permet de déplacer l'affichage de données dans le temps en fonction de l'interval actuel du graphique.
        /// (Passer au jours suivant par exemple.)
        /// </summary>
        private void MoveChartDataForward_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Grid chartGrid = VisualTreeHelper.GetParent(btn) as Grid;
            CartesianChart chart = (CartesianChart)chartGrid.Children[4];
            ChartData chartData = (ChartData)chart.Tag;

            chartData.DisplayWindow++;
            chartData.AutoDisplay = false;

            chart.Tag = chartData;
            sim.TriggerDisplayChanges();
        }

        /// <summary>
        /// Permet de déplacer l'affichage de données dans le temps en fonction de l'interval actuel du graphique.
        /// (Passer au jours précédent par exemple.)
        /// </summary>
        private void MoveChartDataBackward_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Grid chartGrid = VisualTreeHelper.GetParent(btn) as Grid;
            CartesianChart chart = (CartesianChart)chartGrid.Children[4];
            ChartData chartData = (ChartData)chart.Tag;

            if (chartData.DisplayWindow > 0)
                chartData.DisplayWindow--;

            chartData.AutoDisplay = false;

            chart.Tag = chartData;
            sim.TriggerDisplayChanges();
        }

        /// <summary>
        /// Lorsque la sélection de l'interval de temps des graphiques change.
        /// Réaffiche les graphiques pour appliquer la modification.
        /// </summary>
        private void TimeInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            Grid chartGrid = VisualTreeHelper.GetParent(cbx) as Grid;
            CartesianChart chart = (CartesianChart)chartGrid.Children[4];
            ChartData chartData = (ChartData)chart.Tag;
            chartData.DisplayInterval = cbx.SelectedIndex;
            chart.Tag = chartData;

            sim.TriggerDisplayChanges();
        }

        /// <summary>
        /// Active le mode autoDisplay sur le graphique.
        /// Une fois activé, l'affichage suis les dernières données du graphique automatiquement.
        /// </summary>
        private void MoveChartAuto_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Grid chartGrid = VisualTreeHelper.GetParent(btn) as Grid;
            CartesianChart chart = (CartesianChart)chartGrid.Children[4];
            ChartData chartData = (ChartData)chart.Tag;
            chartData.AutoDisplay = true;
            chart.Tag = chartData;

            sim.TriggerDisplayChanges();

        }

        /// <summary>
        /// Créé un bouton qui permet de modifier le contenu des graphiques en se déplacant dans le jours/semaine/mois suivant ou précédent ou en activant l'affichage automatique.
        /// </summary>
        /// <param name="content">Contenu textuel du bouton.</param>
        /// <returns>Bonton lié au graphique.</returns>
        private Button CreateChartButton(string content)
        {
            Button btn = new Button();
            btn.Style = this.FindResource("GraphButtonStyle") as Style;
            btn.Content = content;
            btn.Foreground = Brushes.White;
            btn.FontSize = 15;
            btn.VerticalAlignment = VerticalAlignment.Stretch;
            btn.HorizontalAlignment = HorizontalAlignment.Stretch;
            return btn;
        }

        /// <summary>
        /// Créé un combobo permettant la sélection de l'interval à afficher (Jours, semaine, mois et total)
        /// </summary>
        /// <returns>Combobox contenant les valeurs temporels.</returns>
        private ComboBox CreateChartCombobox()
        {
            ComboBox cbx = new ComboBox();
            cbx.VerticalAlignment = VerticalAlignment.Stretch;
            cbx.HorizontalAlignment = HorizontalAlignment.Stretch;
            cbx.ItemsSource = from ChartsDisplayInterval n
                              in Enum.GetValues(typeof(ChartsDisplayInterval))
                              select GetEnumDescription(n);

            cbx.SelectedIndex = 0;
            return cbx;
        }

        /// <summary>
        /// Affiche le bon type de graphique au bon endroit dans la grille.
        /// Change le contenu du graphique en fonction de son type.
        /// </summary>
        private void DisplayUI()
        {
            foreach (ChartData chartData in chartDatas)
            {
                if (chartData.SpanX > 0)
                {
                    object uiElement;
                    // Créé les graphiques, leur ajoute leur contenu et s'abonne à un évènement qui mettra à jour les données.
                    switch (chartData.UIType)
                    {
                        default:
                        case (int)UIType.Linear:
                            uiElement = CreateCartesianChart((ChartsAxisData)chartData.AxisX, (ChartsAxisData)chartData.AxisY);
                            ((CartesianChart)uiElement).Tag = chartData;
                            AddCurvesToCartesianChart((CartesianChart)uiElement, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            SubscribeToChartEvents((CartesianChart)uiElement, chartData);
                            break;
                        case (int)UIType.Vertical:
                            uiElement = CreateCartesianChart((ChartsAxisData)chartData.AxisX, (ChartsAxisData)chartData.AxisY);
                            ((CartesianChart)uiElement).Tag = chartData;
                            AddColumnsToCartesianChart((CartesianChart)uiElement, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            SubscribeToChartEvents((CartesianChart)uiElement, chartData);
                            break;
                        case (int)UIType.Horizontal:
                            uiElement = CreateCartesianChart((ChartsAxisData)chartData.AxisX, (ChartsAxisData)chartData.AxisY);
                            ((CartesianChart)uiElement).Tag = chartData;
                            AddRowsToCartesianChart((CartesianChart)uiElement, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            SubscribeToChartEvents((CartesianChart)uiElement, chartData);
                            ((CartesianChart)uiElement).AxisX[0].MaxValue = double.NaN;
                            break;
                        case (int)UIType.PieChart:
                            uiElement = CreatePieChart();
                            AddSectionToPieChart((PieChart)uiElement, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            sim.OnDataUpdate += new DataUpdateEventHandler(((PieChart)uiElement).OnDataUpdatePieChart);
                            break;
                        case (int)UIType.HeatMap:
                            uiElement = CreateCartesianChart((ChartsAxisData)chartData.AxisX, (ChartsAxisData)chartData.AxisY);
                            ((CartesianChart)uiElement).Tag = chartData;
                            AddHeatMapToCartesianChart((CartesianChart)uiElement, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            SubscribeToChartEvents((CartesianChart)uiElement, chartData);
                            ((CartesianChart)uiElement).AxisX[0].MaxValue = double.NaN;
                            break;
                        case (int)UIType.GUI:
                            uiElement = null;
                            wfhUnityHost = new System.Windows.Forms.Integration.WindowsFormsHost();
                            panelUnity = new System.Windows.Forms.Panel();
                            panelUnity.Resize += panel1_Resize;

                            wfhUnityHost.VerticalAlignment = VerticalAlignment.Stretch;
                            wfhUnityHost.HorizontalAlignment = HorizontalAlignment.Stretch;

                            Grid.SetColumn(wfhUnityHost, chartData.X);
                            Grid.SetRow(wfhUnityHost, chartData.Y);
                            Grid.SetColumnSpan(wfhUnityHost, chartData.SpanX);
                            Grid.SetRowSpan(wfhUnityHost, chartData.SpanY);

                            wfhUnityHost.Child = panelUnity;
                            grdContent.Children.Add((UIElement)wfhUnityHost);
                            break;
                    }

                    if (chartData.UIType != (int)UIType.PieChart && chartData.UIType != (int)UIType.GUI)
                    {
                        Grid chartGrid = new Grid();
                        ColumnDefinition firstColumn = new ColumnDefinition();
                        ColumnDefinition secondColumn = new ColumnDefinition();
                        ColumnDefinition thirdColumn = new ColumnDefinition();
                        ColumnDefinition thourthColumn = new ColumnDefinition();
                        ColumnDefinition fifthColumn = new ColumnDefinition();

                        firstColumn.MinWidth = 20;
                        firstColumn.MaxWidth = 20;
                        secondColumn.MinWidth = 20;
                        secondColumn.MaxWidth = 20;
                        thirdColumn.MinWidth = 70;
                        thirdColumn.MaxWidth = 70;
                        thourthColumn.MinWidth = 50;
                        thourthColumn.MaxWidth = 50;

                        chartGrid.ColumnDefinitions.Add(firstColumn);
                        chartGrid.ColumnDefinitions.Add(secondColumn);
                        chartGrid.ColumnDefinitions.Add(thirdColumn);
                        chartGrid.ColumnDefinitions.Add(thourthColumn);
                        chartGrid.ColumnDefinitions.Add(fifthColumn);

                        RowDefinition firstRow = new RowDefinition();
                        RowDefinition chartRow = new RowDefinition();

                        firstRow.MinHeight = 20;
                        firstRow.MaxHeight = 20;

                        chartGrid.RowDefinitions.Add(firstRow);
                        chartGrid.RowDefinitions.Add(chartRow);

                        Button btnLeft = CreateChartButton("<");
                        Button btnRight = CreateChartButton(">");
                        ComboBox cbxTimeIncrement = CreateChartCombobox();
                        Button btnAuto = CreateChartButton("auto.");

                        btnLeft.Click += MoveChartDataBackward_Click;
                        btnRight.Click += MoveChartDataForward_Click;
                        cbxTimeIncrement.SelectionChanged += TimeInterval_SelectionChanged;
                        btnAuto.Click += MoveChartAuto_Click;

                        Grid.SetColumn(btnLeft, 0);
                        Grid.SetRow(btnLeft, 0);

                        Grid.SetColumn(btnRight, 1);
                        Grid.SetRow(btnRight, 0);

                        Grid.SetColumn(cbxTimeIncrement, 2);
                        Grid.SetRow(cbxTimeIncrement, 0);

                        Grid.SetColumn(btnAuto, 3);
                        Grid.SetRow(btnAuto, 0);

                        Grid.SetColumn((UIElement)uiElement, 1);
                        Grid.SetRow((UIElement)uiElement, 1);
                        Grid.SetColumnSpan((UIElement)uiElement, 5);
                        Grid.SetRowSpan((UIElement)uiElement, 1);

                        chartGrid.Children.Add(btnLeft);
                        chartGrid.Children.Add(btnRight);
                        chartGrid.Children.Add(cbxTimeIncrement);
                        chartGrid.Children.Add(btnAuto);
                        chartGrid.Children.Add((UIElement)uiElement);

                        Grid.SetColumn(chartGrid, chartData.X);
                        Grid.SetRow(chartGrid, chartData.Y);
                        Grid.SetColumnSpan(chartGrid, chartData.SpanX);
                        Grid.SetRowSpan(chartGrid, chartData.SpanY);

                        if ((UIType)chartData.UIType == UIType.HeatMap)
                        {
                            cbxTimeIncrement.SelectedIndex = 1;
                            cbxTimeIncrement.IsEnabled = false;
                        }

                        grdContent.Children.Add(chartGrid);
                    }
                    else if(chartData.UIType == (int)UIType.PieChart)
                    {
                        Grid.SetColumn((UIElement)uiElement, chartData.X);
                        Grid.SetRow((UIElement)uiElement, chartData.Y);
                        Grid.SetColumnSpan((UIElement)uiElement, chartData.SpanX);
                        Grid.SetRowSpan((UIElement)uiElement, chartData.SpanY);

                        grdContent.Children.Add((UIElement)uiElement);
                    }
                }
            }
        }

        /// <summary>
        /// S'abonne aux évènements permettant l'ajout de donéées ainsi que la modification de l'affichage.
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="chartData"></param>
        private void SubscribeToChartEvents(CartesianChart chart, ChartData chartData)
        {
            sim.OnDisplay += new DispalyChangeEventHandler(chart.Display);
        }

        /// <summary>
        /// Créé un graphique cartésien permettant l'ajout de courbe, colonne, ligne, heatmap.
        /// </summary>
        /// <param name="axeXDatas">Donnée sur l'axe X</param>
        /// <param name="axeYDatas">Donnée sur l'axe Y</param>
        /// <returns>Le graphique créé.</returns>
        private CartesianChart CreateCartesianChart(ChartsAxisData axeXDatas, ChartsAxisData axeYDatas)
        {
            CartesianChart cartesianChart = new CartesianChart();
            cartesianChart.Series = new SeriesCollection();
            cartesianChart.LegendLocation = LegendLocation.Top;
            cartesianChart.Foreground = Brushes.Gray;
            cartesianChart.DisableAnimations = true;
            cartesianChart.Hoverable = false;

            Axis axisX = CreateAxis(axeXDatas);
            axisX.MinValue = 0;
            axisX.MaxValue = 1; // Nombre de période sur une semaine.
            cartesianChart.AxisX.Add(axisX);

            Axis axisY = CreateAxis(axeYDatas);
            axisY.MinValue = 0;
            cartesianChart.AxisY.Add(axisY);

            DataContext = this;
            cartesianChart.VerticalAlignment = VerticalAlignment.Stretch;
            cartesianChart.HorizontalAlignment = HorizontalAlignment.Stretch;

            return cartesianChart;
        }

        /// <summary>
        /// Créé un graphique cylindrique.
        /// </summary>
        /// <returns>Le graphique cylindrique créé.</returns>
        private PieChart CreatePieChart()
        {
            PieChart pieChart = new PieChart();
            pieChart.Series = new SeriesCollection();
            pieChart.LegendLocation = LegendLocation.Right;
            pieChart.Foreground = Brushes.Gray;
            pieChart.DisableAnimations = true;
            pieChart.Hoverable = false;

            DataContext = this;
            pieChart.VerticalAlignment = VerticalAlignment.Stretch;
            pieChart.HorizontalAlignment = HorizontalAlignment.Stretch;

            return pieChart;
        }

        /// <summary>
        /// Créé un axe pour un graphique cartésien.
        /// </summary>
        /// <param name="axeDatas">Nom de l'axe.</param>
        /// <returns>L'axe créé.</returns>
        private Axis CreateAxis(ChartsAxisData axeDatas)
        {
            Axis axis = new Axis();
            axis.Title = GetEnumDescription(axeDatas);
            axis.MaxValue = 10;

            return axis;
        }

        /// <summary>
        /// Ajoute une ou plusieurs courbes à un graphique cartésien.
        /// </summary>
        /// <param name="chart">graphique où la courbe sera ajoutée.</param>
        /// <param name="curvesData">Type de données à afficher par courbe.</param>
        private void AddCurvesToCartesianChart(CartesianChart chart, ChartsDisplayData[] curvesData)
        {
            for (int i = 0; i < curvesData.Length; i++)
            {
                ChartValues<double> values = new ChartValues<double>();
                chart.Series.Add(new LineSeries
                {
                    Title = GetEnumDescription(curvesData[i]).ToString(),
                    Foreground = Brushes.Gray,
                    Tag = curvesData[i],
                    PointGeometry = null,
                    Values = values,
                    DataLabels = false,
                });;
                chart.Series[0].Values = values.AsGearedValues().WithQuality(Quality.Low);
            }
        }            

        /// <summary>
        /// Ajoute une ou plusieurs colonnes à un graphique cartésien.
        /// </summary>
        /// <param name="chart">graphique où la colonne sera ajoutée.</param>
        /// <param name="curvesData">Type de données à afficher par colonne.</param>
        private void AddColumnsToCartesianChart(CartesianChart chart, ChartsDisplayData[] curvesData)
        {
            for (int i = 0; i < curvesData.Length; i++)
            {
                ChartValues<double> values = new ChartValues<double>();
                chart.Series.Add(new ColumnSeries
                {
                    Title = GetEnumDescription(curvesData[i]).ToString(),
                    Foreground = Brushes.Gray,
                    Tag = curvesData[i],
                    Values = values,
                    DataLabels = false
                });
            }
        }

        /// <summary>
        /// Ajoute une ou plusieurs lignes à un graphique cartésien.
        /// </summary>
        /// <param name="chart">graphique où la ligne sera ajoutée.</param>
        /// <param name="curvesData">Type de données à afficher par ligne.</param>
        private void AddRowsToCartesianChart(CartesianChart chart, ChartsDisplayData[] curvesData)
        {
            for (int i = 0; i < curvesData.Length; i++)
            {
                chart.Series.Add(new RowSeries
                {
                    Title = GetEnumDescription(curvesData[i]).ToString(),
                    Foreground = Brushes.Gray,
                    Tag = curvesData[i],
                    Values = new ChartValues<double> ()
                });
            }
        }

        /// <summary>
        /// Ajoute une ou plusieurs section à un graphique cylindrique.
        /// </summary>
        /// <param name="chart">Graphique où la section sera ajoutée.</param>
        /// <param name="curvesData">Type de données à afficher par section.</param>
        private void AddSectionToPieChart(PieChart chart, ChartsDisplayData[] curvesData)
        {
            for (int i = 0; i < curvesData.Length; i++)
            {
                chart.Series.Add(new PieSeries
                {
                    Title = GetEnumDescription(curvesData[i]).ToString(),
                    Foreground = Brushes.Gray,
                    Tag = curvesData[i],
                    Values = new ChartValues<double>()
                });
            }
        }

        /// <summary>
        /// Ajoute une heatMap à un graphique cartésien.
        /// </summary>
        /// <param name="chart">graphique où la heatMap sera ajoutée.</param>
        /// <param name="heatMapData">Type de données à afficher dans la heatMap.</param>
        private void AddHeatMapToCartesianChart(CartesianChart chart, ChartsDisplayData[] heatMapData)
        {
            ChartValues<HeatPoint> values = new ChartValues<HeatPoint>();

            HeatSeries heatSeries = new HeatSeries();
            heatSeries.Values = values;
            heatSeries.Title = GetEnumDescription(heatMapData[0]).ToString();
            heatSeries.Tag = heatMapData[0];
            heatSeries.DataLabels = false;
            heatSeries.GradientStopCollection = new GradientStopCollection()
            {
                new GradientStop(Colors.Green, 0),
                new GradientStop(Colors.GreenYellow, 0.25),
                new GradientStop(Colors.Yellow, 0.5),
                new GradientStop(Colors.Orange, 0.75),
                new GradientStop(Colors.Red, 1)
            };
            chart.Series.Add(heatSeries);
        }

        /// <summary>
        /// Récupère la description des valeurs d'un enum pour un affichage plus logique.
        /// </summary>
        /// <param name="value">Valeur du enum à convertir en description.</param>
        /// <returns>Description du enum</returns>
        public static string GetEnumDescription(Enum value)
        {
            string result;
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
                result = attributes.First().Description;
            else
                result = value.ToString();

            return result;
        }

        #endregion

        #region Unity

        /// <summary>
        /// Lance le programme unity situé dans les fichier de ce programme.
        /// Lui donne un handle qui s'occupera de le positionner et le controler.
        /// </summary>
        public void LoadUnityExe()
        {
            IntPtr unityHandle = panelUnity.Handle;

            //Start embedded Unity Application
            Task.Run(() =>
            {
                process = new Process();
                //process.StartInfo.FileName = @".\GUIBuild\CovidPropagationGUI.exe";
                process.StartInfo.FileName = @".\GUIBuild2D\CovidPropagationGUI2D.exe";
                process.StartInfo.Arguments = "-parentHWND " + unityHandle.ToInt32() + " " + Environment.CommandLine;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                if (process.WaitForInputIdle())
                {
                    EnumChildWindows(unityHandle, WindowEnum, IntPtr.Zero);
                }
            });
        }

        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            unityHWND = hwnd;
            return 0;
        }


        /// <summary>
        /// Se connecte à unity via un pipeline.
        /// </summary>
        public void ConnectToUnity()
        {
            server = new Thread(ServerThread);
            server.Start();
        }

        /// <summary>
        /// Initialise le pipeline et attend que la connexion soit établis pour lancer la simulation.
        /// </summary>
        private void ServerThread(object data)
        {
            int numThreads = 1;
            pipeServer = new NamedPipeServerStream("SimulationToUnity", PipeDirection.Out, numThreads);

            pipeServer.WaitForConnection();

            ss = new StreamString(pipeServer);
            StartSimulationIteration();
        }

        /// <summary>
        /// Stop la connexion entre WPF et unity et éteint les processus d'unity.
        /// </summary>
        private void CloseUnity()
        {
            if (ss != null)
            {
                ss.CloseLink();
            }

            process.CloseMainWindow();
            process.Kill();
            process.Close();
        }

        /// <summary>
        /// Lorsque la page est fermée, ferme le programme unity ainsi que le pipeline.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            CloseUnity();
        }

        /// <summary>
        /// Resize la fenêtre unity lorsque le panel le contenant change de taille.
        /// </summary>
        private void panel1_Resize(object sender, EventArgs e)
        {
            MoveWindow(unityHWND, 0, 0, panelUnity.Width, panelUnity.Height, true);
        }

        /// <summary>
        /// Convertit les objets en json et les envoies au GUI pour créer l'interface graphique en fonction des données de la simulation.
        /// </summary>
        /// <param name="populationDatas">Données de la population à envoyer.</param>
        /// <param name="siteDatas">Données des sites à envoyer.</param>
        private void OnGUIInitialize(DataPopulation populationDatas, DataSites siteDatas)
        {
            if (ss != null)
            {
                string objectToSend = "Initialize ";
                objectToSend += JsonSerializer.Serialize(populationDatas);
                objectToSend += " " + JsonSerializer.Serialize(siteDatas);
                ss.WriteString(objectToSend);
            }
        }

        /// <summary>
        /// Lorsque la simulation fait une itération, récupère les données de celle-ci et les envoies au GUI au format JSON.
        /// </summary>
        private void OnGUIUpdate(int[] personsNewSite, int[] personsNewState)
        {
            if (ss != null)
            {
                DataIteration jsonIteration = new DataIteration(personsNewSite, personsNewState);
                string objectToSend = "Iterate ";
                objectToSend += JsonSerializer.Serialize(jsonIteration);
                ss.WriteString(objectToSend);
            }
        }
        #endregion
    }
}
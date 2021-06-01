using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
using System.Windows.Interop;
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
        WindowRawDatas rawDatasWindow;
        MainWindow mw;
        public ChartData[,] chartDatas;
        Simulation sim;
        Dictionary<UIType, object> charts;
        Grid grdStruct;

        System.Windows.Forms.Integration.WindowsFormsHost wfhUnityHost;
        System.Windows.Forms.Panel panelUnity;


        public PageSimulation()
        {
            InitializeComponent();
            rawDatasWindow = new WindowRawDatas();
            mw = (MainWindow)Application.Current.MainWindow;
            charts = new Dictionary<UIType, object>();
            sim = new Simulation();
            Virus.Init();
        }

        /// <summary>
        /// Lorsque la simulation fait une itération, récupère les données de celle-ci
        /// </summary>
        static void OnTimerTick(Simulation e)
        {
            //Debug.WriteLine(e.GetData());
            // Sera utilisé pour envoyer des informations au GUI
            // Pas pour récupérer des données.
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
                sim.Initialize(30, 0.1, 50000);
                sim.Interval = GlobalVariables.DEFAULT_INTERVAL;
                sim.OnTickSP += new GetDataEventHandler(OnTimerTick);
                rawDatasWindow.CreateLabels(sim.GetAllDatas());
                sim.OnDataUpdate += new DataUpdateEventHandler(rawDatasWindow.UpdateLabels);
                btnOpenRawDatas.IsEnabled = true;

                Task.Factory.StartNew(() => sim.Iterate());
                intervalSlider.Value = Convert.ToInt32(intervalSlider.Maximum - sim.Interval);

                mw.btnGraphicSettings.IsEnabled = false;
            }
            sim.Start();
            if (panelUnity != null)
            {
                LoadUnityExe();
                ConnectToUnity();
            }
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
                CloseUnity();

            SetGrid(grdStruct, chartDatas);

            mw.btnGraphicSettings.IsEnabled = true;
        }

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
        /// Permet de récupérer la valeur de l'interval en timeFrames.
        /// Les valeurs sont différentes suivant le type de graphiques car leur affichage le demande.
        /// </summary>
        /// <param name="enumInterval">Interval du graphique.</param>
        /// <param name="type">Type de graphique.</param>
        /// <returns>Interval actuel pour ce graphique.</returns>
        private int GetInterval(ChartsDisplayInterval enumInterval, UIType type)
        {
            int interval;
            switch (enumInterval)
            {
                default:
                case ChartsDisplayInterval.Day:
                    interval = 48;
                    if (type == UIType.Horizontal || type == UIType.Vertical)
                        interval = 12;
                    break;
                case ChartsDisplayInterval.Week:
                    interval = 336;
                    if (type == UIType.Horizontal || type == UIType.Vertical || type == UIType.HeatMap)
                        interval = 7;
                    break;
                case ChartsDisplayInterval.Month:
                    interval = 1440; 
                    if (type == UIType.Horizontal || type == UIType.Vertical)
                        interval = 4;
                    break;
                case ChartsDisplayInterval.Total:
                    interval = 0;
                    break;
            }
            return interval;
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
            axis.MaxValue = double.NaN;

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
                    Title = curvesData[i].ToString(),
                    Foreground = Brushes.Gray,
                    Tag = curvesData[i],
                    PointGeometry = null,
                    Values = values,
                    DataLabels = false,
                });
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
                    Title = curvesData[i].ToString(),
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
                Random rdm = GlobalVariables.rdm;
                chart.Series.Add(new RowSeries
                {
                    Title = curvesData[i].ToString(),
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
                Random rdm = GlobalVariables.rdm;
                chart.Series.Add(new PieSeries
                {
                    Title = curvesData[i].ToString(),
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
            Random rdm = GlobalVariables.rdm;

            ChartValues<HeatPoint> values = new ChartValues<HeatPoint>();

            HeatSeries heatSeries = new HeatSeries();
            heatSeries.Values = values;
            heatSeries.Title = heatMapData[0].ToString();
            heatSeries.Tag = heatMapData[0];
            heatSeries.GradientStopCollection = new GradientStopCollection()
            {
                new GradientStop(Colors.Green, 0),
                new GradientStop(Colors.GreenYellow, 0.25),
                new GradientStop(Colors.Yellow, 0.5),
                new GradientStop(Colors.Orange, 0.75),
                new GradientStop(Colors.Red, 1)
            };
            heatSeries.DataLabels = true;
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

        // UNITY_______________________________________________________________________________________________________________________________

        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private Process process;
        private IntPtr unityHWND = IntPtr.Zero;

        private const int WM_ACTIVATE = 0x0006;
        private readonly IntPtr WA_ACTIVE = new IntPtr(1);
        private readonly IntPtr WA_INACTIVE = new IntPtr(0);
        StreamString ss;

        public void LoadUnityExe()
        {
            IntPtr unityHandle = panelUnity.Handle;

            //Start embedded Unity Application
            process = new Process();
            process.StartInfo.FileName = @".\GUIBuild\CovidPropagationGUI.exe";
            process.StartInfo.Arguments = "-parentHWND " + unityHandle.ToInt32() + " " + Environment.CommandLine;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = true;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();

            if (process.WaitForInputIdle())
            {
                EnumChildWindows(unityHandle, WindowEnum, IntPtr.Zero);
            }
        }

        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            unityHWND = hwnd;
            return 0;
        }
        private void ActivateUnityWindow()
        {
            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveWindow(unityHWND, 0, 0, (int)this.ActualWidth / 3 * 2, (int)this.ActualHeight, true);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CloseUnity();
        }

        private void CloseUnity()
        {
            if (ss != null)
            {
                ss.CloseLink();
            }

            process.CloseMainWindow();

            while (!process.HasExited)
                process.Kill();
        }

        public void ConnectToUnity()
        {
            Thread server;

            Debug.WriteLine("Waiting for client connect...\n");
            server = new Thread(ServerThread);
            server.Start();
            #region ouais
            //int i;
            //Thread.Sleep(250);
            //while (i > 0)
            //{
            //    for (int j = 0; j < numThreads; j++)
            //    {
            //        if (servers[j] != null)
            //        {
            //            if (servers[j].Join(250))
            //            {
            //                Debug.WriteLine("Server thread[{0}] finished.", servers[j].ManagedThreadId);
            //                servers[j] = null;
            //                i--;    // decrement the thread watch count
            //            }
            //        }
            //    }
            //}
            //Debug.WriteLine("\nServer threads exhausted, exiting.");
            #endregion
        }

        private void ServerThread(object data)
        {
            int numThreads = 1;
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.Out, numThreads);
            //int threadId = Thread.CurrentThread.ManagedThreadId;

            pipeServer.WaitForConnection();

            //Debug.WriteLine("Client connected on thread[{0}].", threadId);
            Debug.WriteLine("Client connected.");
            try
            {
                Debug.WriteLine("Creating streamString...");
                ss = new StreamString(pipeServer);

                Debug.WriteLine("You can now write.");
            }
            catch (IOException e)
            {
                Debug.WriteLine("ERROR: {0}", e.Message);
            }
            //pipeServer.Close();
        }
        private async void TestUnity_Click(object sender, RoutedEventArgs e)
        {
            if (ss != null)
            {
                Debug.WriteLine("In");
                WeatherForecastWithPOCOs testJson = new WeatherForecastWithPOCOs();
                string objectToSend = JsonSerializer.Serialize(testJson);
                Debug.WriteLine(objectToSend);
                await Task.Run(() =>
                {
                    // Invoke uniquement utile en cas d'utilisation du tbxValue.Text
                    Dispatcher.Invoke((Action)(() =>
                    {
                        ss.WriteString(objectToSend); // tbxValue.Text
                    }));
                });
            }
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            MoveWindow(unityHWND, 0, 0, panelUnity.Width, panelUnity.Height, true);
        }
    }
    public class StreamString
    {
        private BinaryWriter stream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream stream)
        {
            this.stream = new BinaryWriter(stream);
            streamEncoding = new UnicodeEncoding();
        }

        public async void WriteString(string outString)
        {
            await Task.Run(() => {
                byte[] outBuffer = streamEncoding.GetBytes(outString);
                int len = outBuffer.Length;

                List<byte> dataToSend = new List<byte>();
                dataToSend.Add((byte)(len >> 8));
                dataToSend.Add((byte)(len >> 0));
                dataToSend.AddRange(outBuffer.ToList());
                stream.Write(dataToSend.ToArray(), 0, dataToSend.Count);
                stream.Flush();
            });

        }

        public void CloseLink()
        {
            if (stream != null)
            {
                stream.Close();
            }

        }
    }

    #region UnityTestClasses

    public class WeatherForecastWithPOCOs
    {
        public DateTimeOffset Date { get; set; }
        public int TemperatureCelsius { get; set; }
        public string Summary { get; set; }
        public string SummaryField;
        public IList<DateTimeOffset> DatesAvailable { get; set; }
        public Dictionary<string, HighLowTemps> TemperatureRanges { get; set; }
        public string[] SummaryWords { get; set; }

        public WeatherForecastWithPOCOs()
        {
            SummaryWords = new string[] { "1", "3", "3" };
            TemperatureRanges = new Dictionary<string, HighLowTemps>();
            TemperatureRanges.Add("range1", new HighLowTemps());
        }
    }
    public class HighLowTemps
    {
        public int High { get; set; }
        public int Low { get; set; }
    }

    #endregion
}
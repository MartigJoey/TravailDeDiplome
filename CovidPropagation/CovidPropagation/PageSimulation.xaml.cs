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
        Dictionary<ChartsType, object> charts;
        
        public PageSimulation()
        {
            InitializeComponent();
            rawDatasWindow = new WindowRawDatas();
            mw = (MainWindow)Application.Current.MainWindow;
            charts = new Dictionary<ChartsType, object>();
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
                sim.Iterate();
                intervalSlider.Value = Convert.ToInt32(intervalSlider.Maximum - sim.Interval);
            }
            sim.Start();
        }

        /// <summary>
        /// Met en pause la simulation
        /// </summary>
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            sim.Stop();
        }

        /// <summary>
        /// Modifie la grille de cette page pour qu'elle correspondent à celle modifiée dans les paramètres graphiques.
        /// </summary>
        /// <param name="grd">Grille contenant les colonnes et lignes à afficher.</param>
        /// <param name="chartDatas">Données des graphiques à afficher dans la grille.</param>
        public void SetGrid(Grid grd, ChartData[,] chartDatas)
        {
            this.chartDatas = chartDatas;
            grdContent = grd;
            slvScroller.Content = grdContent;
            charts.Clear();
            DisplayCharts();
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
            ComboBox cbx = (ComboBox)chartGrid.Children[2];
            ChartData chartData = (ChartData)chart.Tag;
            Axis axis;
            int interval;

            if (chartData.ChartType == (int)ChartsType.HeatMap)
                interval = GetInterval(ChartsDisplayInterval.Week, (ChartsType)chartData.ChartType);
            else
                interval = GetInterval((ChartsDisplayInterval)cbx.SelectedIndex, (ChartsType)chartData.ChartType);


            if ((ChartsType)chartData.ChartType == ChartsType.Horizontal)
                axis = chart.AxisY[0];
            else
                axis = chart.AxisX[0];

            axis.MinValue += interval;
            axis.MaxValue += interval;

            if (interval != 0)
                chartData.AutoDisplay = false;

            chart.Tag = chartData;
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
            ComboBox cbx = (ComboBox)chartGrid.Children[2];
            ChartData chartData = (ChartData)chart.Tag;
            Axis axis;
            int interval;

            if (chartData.ChartType == (int)ChartsType.HeatMap)
                interval = GetInterval(ChartsDisplayInterval.Week, (ChartsType)chartData.ChartType);
            else
                interval = GetInterval((ChartsDisplayInterval)cbx.SelectedIndex, (ChartsType)chartData.ChartType);

            if ((ChartsType)chartData.ChartType == ChartsType.Horizontal)
                axis = chart.AxisY[0];
            else
                axis = chart.AxisX[0];

            // Modifier la ligne en dessous pour qu'elle permette de se déplacer uniquement de jours en jours/semaines/mois et de ne pas être décalé.
            //Math.Ceiling((double)maxValue / interval) * interval;

            if (axis.MinValue - interval >= 0 && interval != 0)
            {
                axis.MinValue -= interval;
                axis.MaxValue -= interval;
            }
            else
            {
                axis.MinValue = 0;
                axis.MaxValue = interval;
            }

            if(interval != 0)
                chartData.AutoDisplay = false;

            chart.Tag = chartData;
        }

        /// <summary>
        /// Permet de récupérer la valeur de l'interval en timeFrames.
        /// Les valeurs sont différentes suivant le type de graphiques car leur affichage le demande.
        /// </summary>
        /// <param name="enumInterval">Interval du graphique.</param>
        /// <param name="type">Type de graphique.</param>
        /// <returns>Interval actuel pour ce graphique.</returns>
        private int GetInterval(ChartsDisplayInterval enumInterval, ChartsType type)
        {
            int interval;
            switch (enumInterval)
            {
                default:
                case ChartsDisplayInterval.Day:
                    interval = 48;
                    if (type == ChartsType.Horizontal || type == ChartsType.Vertical)
                        interval = 12;
                    break;
                case ChartsDisplayInterval.Week:
                    interval = 336;
                    if (type == ChartsType.Horizontal || type == ChartsType.Vertical || type == ChartsType.HeatMap)
                        interval = 7;
                    break;
                case ChartsDisplayInterval.Month:
                    interval = 1440; 
                    if (type == ChartsType.Horizontal || type == ChartsType.Vertical)
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
        private void DisplayCharts()
        {
            foreach (ChartData chartData in chartDatas)
            {
                if (chartData.SpanX > 0)
                {
                    object chart;
                    // Créé les graphiques, leur ajoute leur contenu et s'abonne à un évènement qui mettra à jour les données.
                    switch (chartData.ChartType)
                    {
                        default:
                        case (int)ChartsType.Linear:
                            chart = CreateCartesianChart((ChartsAxisData)chartData.AxisX, (ChartsAxisData)chartData.AxisY);
                            ((CartesianChart)chart).Tag = chartData;
                            AddCurvesToCartesianChart((CartesianChart)chart, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            SubscribeToChartEvents((CartesianChart)chart, chartData);
                            break;
                        case (int)ChartsType.Vertical:
                            chart = CreateCartesianChart((ChartsAxisData)chartData.AxisX, (ChartsAxisData)chartData.AxisY);
                            ((CartesianChart)chart).Tag = chartData;
                            AddColumnsToCartesianChart((CartesianChart)chart, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            SubscribeToChartEvents((CartesianChart)chart, chartData);
                            break;
                        case (int)ChartsType.Horizontal:
                            chart = CreateCartesianChart((ChartsAxisData)chartData.AxisX, (ChartsAxisData)chartData.AxisY);
                            ((CartesianChart)chart).Tag = chartData;
                            AddRowsToCartesianChart((CartesianChart)chart, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            SubscribeToChartEvents((CartesianChart)chart, chartData);
                            ((CartesianChart)chart).AxisX[0].MaxValue = double.NaN;
                            break;
                        case (int)ChartsType.PieChart:
                            chart = CreatePieChart();
                            AddSectionToPieChart((PieChart)chart, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            sim.OnDataUpdate += new DataUpdateEventHandler(((PieChart)chart).OnDataUpdatePieChart);
                            break;
                        case (int)ChartsType.HeatMap:
                            chart = CreateCartesianChart((ChartsAxisData)chartData.AxisX, (ChartsAxisData)chartData.AxisY);
                            ((CartesianChart)chart).Tag = chartData;
                            AddHeatMapToCartesianChart((CartesianChart)chart, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            SubscribeToChartEvents((CartesianChart)chart, chartData);
                            ((CartesianChart)chart).AxisX[0].MaxValue = double.NaN;
                            break;
                    }

                    if (chartData.ChartType != (int)ChartsType.PieChart)
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

                        Grid.SetColumn((UIElement)chart, 1);
                        Grid.SetRow((UIElement)chart, 1);
                        Grid.SetColumnSpan((UIElement)chart, 5);
                        Grid.SetRowSpan((UIElement)chart, 1);

                        chartGrid.Children.Add(btnLeft);
                        chartGrid.Children.Add(btnRight);
                        chartGrid.Children.Add(cbxTimeIncrement);
                        chartGrid.Children.Add(btnAuto);
                        chartGrid.Children.Add((UIElement)chart);

                        Grid.SetColumn(chartGrid, chartData.X);
                        Grid.SetRow(chartGrid, chartData.Y);
                        Grid.SetColumnSpan(chartGrid, chartData.SpanX);
                        Grid.SetRowSpan(chartGrid, chartData.SpanY);

                        if ((ChartsType)chartData.ChartType == ChartsType.HeatMap)
                        {
                            cbxTimeIncrement.SelectedIndex = 1;
                            cbxTimeIncrement.IsEnabled = false;
                        }

                        grdContent.Children.Add(chartGrid);
                    }
                    else
                    {
                        Grid.SetColumn((UIElement)chart, chartData.X);
                        Grid.SetRow((UIElement)chart, chartData.Y);
                        Grid.SetColumnSpan((UIElement)chart, chartData.SpanX);
                        Grid.SetRowSpan((UIElement)chart, chartData.SpanY);

                        grdContent.Children.Add((UIElement)chart);
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
            sim.OnDataUpdate += new DataUpdateEventHandler(chart.OnDataUpdate);
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
                    Values = new ChartValues<double> {
                        rdm.Next(1, 10),
                        rdm.Next(1, 10),
                        rdm.Next(1, 10),
                        rdm.Next(1, 10),
                        rdm.Next(1, 10)
                    }
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
                    Values = new ChartValues<double> {
                        rdm.Next(1, 10)
                    }
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

            ChartValues<HeatPoint> values = new ChartValues<HeatPoint>
            {
                new HeatPoint(0, 0, rdm.Next(0, 10)),
                new HeatPoint(0, 1, rdm.Next(0, 10)),
                new HeatPoint(0, 2, rdm.Next(0, 10)),
                new HeatPoint(0, 3, rdm.Next(0, 10)),
                new HeatPoint(0, 4, rdm.Next(0, 10)),
                new HeatPoint(0, 5, rdm.Next(0, 10)),
                new HeatPoint(0, 6, rdm.Next(0, 10)),

                new HeatPoint(1, 0, rdm.Next(0, 10)),
                new HeatPoint(1, 1, rdm.Next(0, 10)),
                new HeatPoint(1, 2, rdm.Next(0, 10)),
                new HeatPoint(1, 3, rdm.Next(0, 10)),
                new HeatPoint(1, 4, rdm.Next(0, 10)),
                new HeatPoint(1, 5, rdm.Next(0, 10)),
                new HeatPoint(1, 6, rdm.Next(0, 10)),

                new HeatPoint(2, 0, rdm.Next(0, 10)),
                new HeatPoint(2, 1, rdm.Next(0, 10)),
                new HeatPoint(2, 2, rdm.Next(0, 10)),
                new HeatPoint(2, 3, rdm.Next(0, 10)),
                new HeatPoint(2, 4, rdm.Next(0, 10)),
                new HeatPoint(2, 5, rdm.Next(0, 10)),
                new HeatPoint(2, 6, rdm.Next(0, 10)),

                new HeatPoint(3, 0, rdm.Next(0, 10)),
                new HeatPoint(3, 1, rdm.Next(0, 10)),
                new HeatPoint(3, 2, rdm.Next(0, 10)),
                new HeatPoint(3, 3, rdm.Next(0, 10)),
                new HeatPoint(3, 4, rdm.Next(0, 10)),
                new HeatPoint(3, 5, rdm.Next(0, 10)),
                new HeatPoint(3, 6, rdm.Next(0, 10)),

                new HeatPoint(3, 0, rdm.Next(0, 10)),
                new HeatPoint(3, 1, rdm.Next(0, 10)),
                new HeatPoint(3, 2, rdm.Next(0, 10)),
                new HeatPoint(3, 3, rdm.Next(0, 10)),
                new HeatPoint(3, 4, rdm.Next(0, 10)),
                new HeatPoint(3, 5, rdm.Next(0, 10)),
                new HeatPoint(3, 6, rdm.Next(0, 10)),

                new HeatPoint(3, 0, rdm.Next(0, 10)),
                new HeatPoint(3, 1, rdm.Next(0, 10)),
                new HeatPoint(3, 2, rdm.Next(0, 10)),
                new HeatPoint(3, 3, rdm.Next(0, 10)),
                new HeatPoint(3, 4, rdm.Next(0, 10)),
                new HeatPoint(3, 5, rdm.Next(0, 10)),
                new HeatPoint(3, 6, rdm.Next(0, 10)),

                new HeatPoint(3, 0, rdm.Next(0, 10)),
                new HeatPoint(3, 1, rdm.Next(0, 10)),
                new HeatPoint(3, 2, rdm.Next(0, 10)),
                new HeatPoint(3, 3, rdm.Next(0, 10)),
                new HeatPoint(3, 4, rdm.Next(0, 10)),
                new HeatPoint(3, 5, rdm.Next(0, 10)),
                new HeatPoint(3, 6, rdm.Next(0, 10)),
            };

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
    }
}
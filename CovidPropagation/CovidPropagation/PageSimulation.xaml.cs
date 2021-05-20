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
        Legend legendPage;
        MainWindow mw;
        public ChartData[,] chartDatas;
        Simulation sim;
        Dictionary<ChartsType, object> charts;
        
        public PageSimulation()
        {
            InitializeComponent();
            legendPage = new Legend();
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

        private void OpenLegendWindow_Click(object sender, RoutedEventArgs e)
        {
            legendPage.Show();
            legendPage.Focus();
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
                            SubscribeToTimeChange((CartesianChart)chart, chartData);
                            break;
                        case (int)ChartsType.Vertical:
                            chart = CreateCartesianChart((ChartsAxisData)chartData.AxisX, (ChartsAxisData)chartData.AxisY);
                            ((CartesianChart)chart).Tag = chartData;
                            AddColumnsToCartesianChart((CartesianChart)chart, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            SubscribeToTimeChange((CartesianChart)chart, chartData);
                            break;
                        case (int)ChartsType.Horizontal:
                            chart = CreateCartesianChart((ChartsAxisData)chartData.AxisX, (ChartsAxisData)chartData.AxisY);
                            ((CartesianChart)chart).Tag = chartData;
                            AddRowsToCartesianChart((CartesianChart)chart, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            SubscribeToTimeChange((CartesianChart)chart, chartData);
                            break;
                        case (int)ChartsType.PieChart:
                            chart = CreatePieChart();
                            AddSectionToPieChart((PieChart)chart, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            sim.OnTimeFramChange += new TimeFrameChangeEventHandler(((PieChart)chart).OnTimeFrameChange);
                            break;
                        case (int)ChartsType.HeatMap:
                            chart = CreateCartesianChart((ChartsAxisData)chartData.AxisX, (ChartsAxisData)chartData.AxisY);
                            ((CartesianChart)chart).Tag = chartData;
                            AddHeatMapToCartesianChart((CartesianChart)chart, Array.ConvertAll(chartData.Datas, d => (ChartsDisplayData)d));
                            SubscribeToTimeChange((CartesianChart)chart, chartData);
                            break;
                    }
                    Grid.SetColumn((UIElement)chart, chartData.X);
                    Grid.SetRow((UIElement)chart, chartData.Y);
                    Grid.SetColumnSpan((UIElement)chart, chartData.SpanX);
                    Grid.SetRowSpan((UIElement)chart, chartData.SpanY);

                    grdContent.Children.Add((UIElement)chart);
                }
            }
        }

        private void SubscribeToTimeChange(CartesianChart chart, ChartData chartData)
        {
            ChartsAxisData temporalAxis = ChartsAxisData.QuantityOfCase;

            if (chartData.AxisX <= (int)ChartsAxisData.Week)
                temporalAxis = (ChartsAxisData)chartData.AxisX;
            else if (chartData.AxisY <= (int)ChartsAxisData.Week)
                temporalAxis = (ChartsAxisData)chartData.AxisY;

            switch (temporalAxis)
            {
                default:
                case ChartsAxisData.TimeFrame:
                    sim.OnTimeFramChange += new TimeFrameChangeEventHandler(chart.OnTimeFrameChange);
                    break;
                case ChartsAxisData.Day:
                    sim.OnDayChange += new DayChangeEventHandler(chart.OnDayChange);
                    break;
                case ChartsAxisData.Week:
                    sim.OnWeekChange += new WeekChangeEventHandler(chart.OnWeekChange);
                    break;
            }
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
            cartesianChart.AxisX.Add(axisX);

            Axis axisY = CreateAxis(axeYDatas);
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
                Random rdm = GlobalVariables.rdm;
                SeriesCollection newSerie = new SeriesCollection();
                ChartValues<double> values = new ChartValues<double>();
                chart.Series.Add(new LineSeries
                {
                    Title = curvesData[i].ToString(),
                    Foreground = Brushes.Gray,
                    Tag = curvesData[i],
                    PointGeometry = null,
                    Values = values,
                    DataLabels = false
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
                Random rdm = GlobalVariables.rdm;
                chart.Series.Add(new ColumnSeries
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
        /// <param name="curvesData">Type de données à afficher dans la heatMap.</param>
        private void AddHeatMapToCartesianChart(CartesianChart chart, ChartsDisplayData[] curvesData)
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

                new HeatPoint(4, 0, rdm.Next(0, 10)),
                new HeatPoint(4, 1, rdm.Next(0, 10)),
                new HeatPoint(4, 2, rdm.Next(0, 10)),
                new HeatPoint(4, 3, rdm.Next(0, 10)),
                new HeatPoint(4, 4, rdm.Next(0, 10)),
                new HeatPoint(4, 5, rdm.Next(0, 10)),
                new HeatPoint(4, 6, rdm.Next(0, 10))
            };

            HeatSeries heatSeries = new HeatSeries();
            heatSeries.Values = values;
            heatSeries.Title = curvesData[0].ToString();
            heatSeries.Tag = curvesData[0];
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

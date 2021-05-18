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

namespace CovidPropagation
{
    /// <summary>
    /// Logique d'interaction pour PageSimulation.xaml
    /// </summary>
    public partial class PageSimulation : Page
    {
        Legend legendPage;
        MainWindow mw;
        public GraphicData[,] graphicDatas;
        string data;
        //public int SliderValue { get => (int)intervalSlider.Value; set => intervalSlider.Value = value; }
        public PageSimulation()
        {
            InitializeComponent();
            legendPage = new Legend();
            //intervalSlider.Value = GlobalVariables.DEFAULT_INTERVAL;
            mw = (MainWindow)Application.Current.MainWindow;
            Loaded += MyPage_Loaded;
        }

        private void MyPage_Loaded(object sender, RoutedEventArgs e)
        {
            //intervalSlider.Value = Convert.ToInt32(intervalSlider.Maximum - mw.Sim.Interval);
        }

        private void OpenLegendWindow_Click(object sender, RoutedEventArgs e)
        {
            legendPage.Show();
            legendPage.Focus();
        }

        private void IntervalSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            //mw.Sim.Interval = Convert.ToInt32(intervalSlider.Maximum - intervalSlider.Value);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            mw.Sim.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            mw.Sim.Stop();
        }

        public void SetGrid(Grid grd, GraphicData[,] graphicDatas)
        {
            this.graphicDatas = graphicDatas;
            grdContent = grd;
            slvScroller.Content = grdContent;
            DisplayGraphics();
        }

        private void DisplayGraphics()
        {
            foreach (GraphicData graph in graphicDatas)
            {
                if (graph.SpanX > 0)
                {
                    switch (graph.GraphicType)
                    {
                        case (int)GraphicsType.Linear:
                            // CreateCartesian
                            CartesianChart linearGraph = CreateCartesianGraph((GraphicsAxisData)graph.AxisX, (GraphicsAxisData)graph.AxisY);
                            AddCurvesToCartesianGraphic(linearGraph, Array.ConvertAll(graph.Datas, d => (GraphicsDisplayData)d));
                            Grid.SetColumn(linearGraph, graph.X);
                            Grid.SetRow(linearGraph, graph.Y);
                            Grid.SetColumnSpan(linearGraph, graph.SpanX);
                            Grid.SetRowSpan(linearGraph, graph.SpanY);
                            grdContent.Children.Add(linearGraph);
                            break;
                        case (int)GraphicsType.Vertical:
                            // CreateCartesian
                            CartesianChart verticalGraph = CreateCartesianGraph((GraphicsAxisData)graph.AxisX, (GraphicsAxisData)graph.AxisY);
                            AddColumnsToCartesianGraphic(verticalGraph, Array.ConvertAll(graph.Datas, d => (GraphicsDisplayData)d));
                            Grid.SetColumn(verticalGraph, graph.X);
                            Grid.SetRow(verticalGraph, graph.Y);
                            Grid.SetColumnSpan(verticalGraph, graph.SpanX);
                            Grid.SetRowSpan(verticalGraph, graph.SpanY);
                            grdContent.Children.Add(verticalGraph);
                            break;
                        case (int)GraphicsType.Horizontal:
                            // CreateCartesian
                            CartesianChart horizontalGraph = CreateCartesianGraph((GraphicsAxisData)graph.AxisX, (GraphicsAxisData)graph.AxisY);
                            AddRowsToCartesianGraphic(horizontalGraph, Array.ConvertAll(graph.Datas, d => (GraphicsDisplayData)d));
                            Grid.SetColumn(horizontalGraph, graph.X);
                            Grid.SetRow(horizontalGraph, graph.Y);
                            Grid.SetColumnSpan(horizontalGraph, graph.SpanX);
                            Grid.SetRowSpan(horizontalGraph, graph.SpanY);
                            grdContent.Children.Add(horizontalGraph);
                            break;
                        case (int)GraphicsType.PieChart:
                            // CreateCartesian
                            PieChart pieGraph = CreatePieGraph();
                            AddSectionToPieGraphic(pieGraph, Array.ConvertAll(graph.Datas, d => (GraphicsDisplayData)d));
                            Grid.SetColumn(pieGraph, graph.X);
                            Grid.SetRow(pieGraph, graph.Y);
                            Grid.SetColumnSpan(pieGraph, graph.SpanX);
                            Grid.SetRowSpan(pieGraph, graph.SpanY);
                            grdContent.Children.Add(pieGraph);
                            break;
                        case (int)GraphicsType.HeatMap:
                            // CreateCartesian
                            CartesianChart heatMapGraph = CreateCartesianGraph((GraphicsAxisData)graph.AxisX, (GraphicsAxisData)graph.AxisY);
                            AddHeatMapToCartesianGraphic(heatMapGraph, Array.ConvertAll(graph.Datas, d => (GraphicsDisplayData)d));
                            Grid.SetColumn(heatMapGraph, graph.X);
                            Grid.SetRow(heatMapGraph, graph.Y);
                            Grid.SetColumnSpan(heatMapGraph, graph.SpanX);
                            Grid.SetRowSpan(heatMapGraph, graph.SpanY);
                            grdContent.Children.Add(heatMapGraph);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private CartesianChart CreateCartesianGraph(GraphicsAxisData axeXDatas, GraphicsAxisData axeYDatas)
        {
            CartesianChart cartesianChart = new CartesianChart();
            cartesianChart.Series = new SeriesCollection();
            cartesianChart.LegendLocation = LegendLocation.Top;
            cartesianChart.Foreground = Brushes.Gray;

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
        private PieChart CreatePieGraph()
        {
            PieChart pieChart = new PieChart();
            pieChart.Series = new SeriesCollection();
            pieChart.LegendLocation = LegendLocation.Right;
            pieChart.Foreground = Brushes.Gray;

            DataContext = this;
            pieChart.VerticalAlignment = VerticalAlignment.Stretch;
            pieChart.HorizontalAlignment = HorizontalAlignment.Stretch;

            return pieChart;
        }

        private Axis CreateAxis(GraphicsAxisData axeDatas)
        {
            Axis axis = new Axis();
            axis.Title = GetEnumDescription(axeDatas);
            axis.MaxValue = double.NaN;

            return axis;
        }

        private void AddCurvesToCartesianGraphic(CartesianChart chart, GraphicsDisplayData[] curvesData)
        {
            for (int i = 0; i < curvesData.Length; i++)
            {
                Random rdm = GlobalVariables.rdm;
                chart.Series.Add(new LineSeries
                {
                    Title = curvesData[i].ToString(),
                    Foreground = Brushes.Gray,
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

        private void AddColumnsToCartesianGraphic(CartesianChart chart, GraphicsDisplayData[] curvesData)
        {
            for (int i = 0; i < curvesData.Length; i++)
            {
                Random rdm = GlobalVariables.rdm;
                chart.Series.Add(new ColumnSeries
                {
                    Title = curvesData[i].ToString(),
                    Foreground = Brushes.Gray,
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

        private void AddRowsToCartesianGraphic(CartesianChart chart, GraphicsDisplayData[] curvesData)
        {
            for (int i = 0; i < curvesData.Length; i++)
            {
                Random rdm = GlobalVariables.rdm;
                chart.Series.Add(new RowSeries
                {
                    Title = curvesData[i].ToString(),
                    Foreground = Brushes.Gray,
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

        private void AddSectionToPieGraphic(PieChart chart, GraphicsDisplayData[] curvesData)
        {
            for (int i = 0; i < curvesData.Length; i++)
            {
                Random rdm = GlobalVariables.rdm;
                chart.Series.Add(new PieSeries
                {
                    Title = curvesData[i].ToString(),
                    Foreground = Brushes.Gray,
                    Values = new ChartValues<double> {
                    rdm.Next(1, 10)
                }
                });
            }
        }


        private void AddHeatMapToCartesianGraphic(CartesianChart chart, GraphicsDisplayData[] curvesData)
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

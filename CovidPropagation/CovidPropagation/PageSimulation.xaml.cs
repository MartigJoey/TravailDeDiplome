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
                            AddCurvesToCartesianGraphic(linearGraph, Array.ConvertAll(graph.Datas, d => (GraphicsCurvesData)d));
                            Grid.SetColumn(linearGraph, graph.X);
                            Grid.SetRow(linearGraph, graph.Y);
                            Grid.SetColumnSpan(linearGraph, graph.SpanX);
                            Grid.SetRowSpan(linearGraph, graph.SpanY);
                            grdContent.Children.Add(linearGraph);
                            break;
                        case (int)GraphicsType.Vertical:
                            break;
                        case (int)GraphicsType.Horizontal:
                            break;
                        case (int)GraphicsType.PieChart:
                            break;
                        case (int)GraphicsType.HeatMap:
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

            Axis axisX = CreateAxis(axeXDatas);
            cartesianChart.AxisX.Add(axisX);

            Axis axisY = CreateAxis(axeYDatas);
            cartesianChart.AxisY.Add(axisY);

            DataContext = this;
            cartesianChart.VerticalAlignment = VerticalAlignment.Stretch;
            cartesianChart.HorizontalAlignment = HorizontalAlignment.Stretch;

            return cartesianChart;
        }

        private Axis CreateAxis(GraphicsAxisData axeDatas)
        {
            Axis axis = new Axis();
            switch (axeDatas)
            {
                case GraphicsAxisData.Time:
                    axis.Title = GraphicsAxisData.Time.ToString();
                    axis.Labels = new[] { "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam", "Dim" };
                    break;
                default:
                case GraphicsAxisData.NumberOfPeople:
                    axis.Title = GraphicsAxisData.NumberOfPeople.ToString();
                    break;
            }
            axis.MaxValue = double.NaN;

            return axis;
        }

        private void AddCurvesToCartesianGraphic(CartesianChart chart, GraphicsCurvesData[] curvesData)
        {
            for (int i = 0; i < curvesData.Length; i++)
            {
                Random rdm = GlobalVariables.rdm;
                chart.Series.Add(new LineSeries
                {
                    Title = curvesData[i].ToString(),
                    Foreground = Brushes.White,
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
    }
}

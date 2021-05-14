using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CovidPropagation
{
    public delegate void SaveEventHandler(object source, int[] e);

    /// <summary>
    /// Logique d'interaction pour WindowGraph.xaml
    /// </summary>
    public partial class WindowGraph : Window
    {
        public event SaveEventHandler OnSave;
        private const int MAX_NUMBER_OF_CURVES = 5;
        int cellX;
        int cellY;
        ComboBox[] cbxDatas;
        int currentCurvesIndex;
        object chart;

        public WindowGraph(int cellX, int cellY)
        {
            InitializeComponent();

            this.cellX = cellX;
            this.cellY = cellY;
            cbxDatas = new ComboBox[MAX_NUMBER_OF_CURVES];

            int curvesRow = Grid.GetRow(cbxQuantityOfCurves);
            int curvesColumn = Grid.GetColumn(cbxQuantityOfCurves);
            for (int i = 1; i <= MAX_NUMBER_OF_CURVES; i++)
            {
                cbxQuantityOfCurves.Items.Add(i);
                cbxDatas[i - 1] = new ComboBox();

                curvesRow++;
                Grid.SetRow(cbxDatas[i - 1], curvesRow);
                Grid.SetColumn(cbxDatas[i - 1], curvesColumn);

                cbxDatas[i - 1].ItemsSource = from GraphicsCurvesData n 
                                               in Enum.GetValues(typeof(GraphicsCurvesData))
                                               select GetEnumDescription(n);

                cbxDatas[i - 1].SelectedIndex = 0;
                grdContent.Children.Add(cbxDatas[i - 1]);
            }

            cbxValueX.ItemsSource = from GraphicsAxisData n
                                    in Enum.GetValues(typeof(GraphicsAxisData))
                                    select GetEnumDescription(n);

            cbxValueY.ItemsSource = from GraphicsAxisData n
                                    in Enum.GetValues(typeof(GraphicsAxisData))
                                    select GetEnumDescription(n);

            cbxGraphType.ItemsSource = from GraphicsType n
                                       in Enum.GetValues(typeof(GraphicsType))
                                       select GetEnumDescription(n);
            cbxValueX.SelectedIndex = 0;
            cbxValueY.SelectedIndex = 1;
            cbxGraphType.SelectedIndex = 0;


            currentCurvesIndex = 0;
            cbxQuantityOfCurves.SelectedIndex = currentCurvesIndex;
        }

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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (OnSave != null)
                OnSave(10, new int[] { cellX, cellY });
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GraphType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxValueX != null)
            {
                cbxValueX.IsEnabled = true;
                cbxQuantityOfCurves.IsEnabled = true;
            }
            switch ((GraphicsType)cbxGraphType.SelectedIndex)
            {
                case GraphicsType.Linear:
                    chart = CreateCartesianGraph();
                    SetData(DisplayCurvesOnGraph);
                    break;
                case GraphicsType.Vertical:
                    chart = CreateCartesianGraph(); 
                    SetData(DisplayColumnsOnGraph);
                    break;
                case GraphicsType.Horizontal:
                    chart = CreateCartesianGraph();
                    SetData(DisplayRowsOnGraph);
                    break;
                case GraphicsType.PieChart:
                    chart = CreatePieGraph();
                    SetData(DisplayPieSectionOnGraph);
                    break;
                case GraphicsType.HeatMap:
                    chart = CreateCartesianGraph();
                    SetData(DisplayHeatMapOnGraph);
                    if (cbxValueX != null)
                    {
                        cbxValueX.IsEnabled = false;
                        cbxQuantityOfCurves.IsEnabled = false;
                        cbxQuantityOfCurves.SelectedIndex = 0;
                    }
                    break;
                default:
                    break;
            }
        }

        private void DataQuantity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentCurvesIndex = cbxQuantityOfCurves.Items.IndexOf(cbxQuantityOfCurves.SelectedItem);
            
            switch ((GraphicsType)cbxGraphType.SelectedIndex)
            {
                case GraphicsType.Linear:
                    SetData(DisplayCurvesOnGraph);
                    break;
                case GraphicsType.Vertical:
                    SetData(DisplayColumnsOnGraph);
                    break;
                case GraphicsType.Horizontal:
                    SetData(DisplayRowsOnGraph);
                    break;
                case GraphicsType.PieChart:
                    SetData(DisplayPieSectionOnGraph);
                    break;
                case GraphicsType.HeatMap:
                    SetData(DisplayHeatMapOnGraph);
                    break;

                default:
                    break;
            }
        }

        private void SetData(Func<int, bool, bool> callback)
        {
            if (cbxDatas != null)
            {
                for (int i = 0; i < MAX_NUMBER_OF_CURVES; i++)
                {
                    if (i <= currentCurvesIndex)
                    {
                        cbxDatas[i].Visibility = Visibility.Visible;
                        callback(i, true);
                        cbxDatas[i].Tag = i;
                        cbxDatas[i].SelectionChanged += CbxDatas_SelectionChanged;
                    }
                    else
                    {
                        cbxDatas[i].Visibility = Visibility.Hidden;
                        callback(i, false);
                    }
                }
            }
        }

        private bool DisplayCurvesOnGraph(int index, bool isDisplayed)
        {
            if (chart == null)
                chart = CreateCartesianGraph();

            CartesianChart cartesianChart = (CartesianChart)chart;

            if (isDisplayed && index > cartesianChart.Series.GetLastIndex())
            {
                Random rdm = GlobalVariables.rdm;
                cartesianChart.Series.Add(new LineSeries
                {
                    Title = cbxDatas[index].SelectedItem.ToString(),
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
            else
            {
                int lastIndex = cartesianChart.Series.GetLastIndex();
                if (lastIndex > 0 && lastIndex > currentCurvesIndex)
                    cartesianChart.Series.RemoveAt(lastIndex);
            }
            return true;
        }

        private bool DisplayColumnsOnGraph(int index, bool isDisplayed)
        {
            if (chart == null)
                chart = CreateCartesianGraph();

            CartesianChart cartesianChart = (CartesianChart)chart;

            if (isDisplayed && index > cartesianChart.Series.GetLastIndex())
            {
                Random rdm = GlobalVariables.rdm;
                cartesianChart.Series.Add(new ColumnSeries
                {
                    Title = cbxDatas[index].SelectedItem.ToString(),
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
            else
            {
                int lastIndex = cartesianChart.Series.GetLastIndex();
                if (lastIndex > 0 && lastIndex > currentCurvesIndex)
                    cartesianChart.Series.RemoveAt(lastIndex);
            }
            return true;
        }

        private bool DisplayRowsOnGraph(int index, bool isDisplayed)
        {
            if (chart == null)
                chart = CreateCartesianGraph();

            CartesianChart cartesianChart = (CartesianChart)chart;

            if (isDisplayed && index > cartesianChart.Series.GetLastIndex())
            {
                Random rdm = GlobalVariables.rdm;
                cartesianChart.Series.Add(new RowSeries
                {
                    Title = cbxDatas[index].SelectedItem.ToString(),
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
            else
            {
                int lastIndex = cartesianChart.Series.GetLastIndex();
                if (lastIndex > 0 && lastIndex > currentCurvesIndex)
                    cartesianChart.Series.RemoveAt(lastIndex);
            }
            return true;
        }

        private bool DisplayPieSectionOnGraph(int index, bool isDisplayed)
        {
            if (chart == null)
                chart = CreatePieGraph();

            PieChart pieChart = (PieChart)chart;

            if (isDisplayed && index > pieChart.Series.GetLastIndex())
            {
                Random rdm = GlobalVariables.rdm;
                pieChart.Series.Add(new PieSeries
                {
                    Title = cbxDatas[index].SelectedItem.ToString(),
                    Foreground = Brushes.White,
                    Values = new ChartValues<double> {
                        rdm.Next(1, 10)
                    }
                });
            }
            else
            {
                int lastIndex = pieChart.Series.GetLastIndex();
                if (lastIndex > 0 && lastIndex > currentCurvesIndex)
                    pieChart.Series.RemoveAt(lastIndex);
            }
            return true;
        }

        private bool DisplayHeatMapOnGraph(int index, bool isDisplayed)
        {
            // Index & isDisplayed useless
            if (chart == null)
                chart = CreateCartesianGraph();

            CartesianChart cartesianChart = (CartesianChart)chart;
            Random rdm = GlobalVariables.rdm;

            if (cartesianChart.Series.Count == 0)
            {
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
                heatSeries.Title = cbxDatas[0].SelectedItem.ToString();
                heatSeries.GradientStopCollection = new GradientStopCollection()
                {
                    new GradientStop(Colors.Green, 0),
                    new GradientStop(Colors.GreenYellow, 0.25),
                    new GradientStop(Colors.Yellow, 0.5),
                    new GradientStop(Colors.Orange, 0.75),
                    new GradientStop(Colors.Red, 1)
                };
                heatSeries.DataLabels = true;
                cartesianChart.Series.Add(heatSeries);
            }
            DataContext = this;
            return true;
        }

        private void CbxDatas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbxData = (ComboBox)sender;
            switch ((GraphicsType)cbxGraphType.SelectedIndex)
            {
                case GraphicsType.Linear:
                    CartesianChart linearChart = (CartesianChart)chart;
                    linearChart.Series[Convert.ToInt32(cbxData.Tag)] = new LineSeries
                    {
                        Title = cbxData.SelectedItem.ToString(),
                        Values = linearChart.Series[Convert.ToInt32(cbxData.Tag)].Values
                    };
                    break;
                case GraphicsType.Vertical:
                    CartesianChart verticalChart = (CartesianChart)chart;
                    verticalChart.Series[Convert.ToInt32(cbxData.Tag)] = new ColumnSeries
                    {
                        Title = cbxData.SelectedItem.ToString(),
                        Values = verticalChart.Series[Convert.ToInt32(cbxData.Tag)].Values
                    };
                    break;
                case GraphicsType.Horizontal:
                    CartesianChart horizontalChart = (CartesianChart)chart;
                    horizontalChart.Series[Convert.ToInt32(cbxData.Tag)] = new RowSeries
                    {
                        Title = cbxData.SelectedItem.ToString(),
                        Values = horizontalChart.Series[Convert.ToInt32(cbxData.Tag)].Values
                    };
                    break;
                case GraphicsType.PieChart:
                    PieChart pieChart = (PieChart)chart;
                    pieChart.Series[Convert.ToInt32(cbxData.Tag)] = new PieSeries
                    {
                        Title = cbxData.SelectedItem.ToString(),
                        Values = pieChart.Series[Convert.ToInt32(cbxData.Tag)].Values
                    };
                    break;
                case GraphicsType.HeatMap:
                    chart = CreateCartesianGraph();
                    SetData(DisplayHeatMapOnGraph);
                    break;
                default:
                    break;
            }
        }

        private CartesianChart CreateCartesianGraph()
        {
            CartesianChart cartesianChart = new CartesianChart();
            cartesianChart.Series = new SeriesCollection();
            cartesianChart.LegendLocation = LegendLocation.Right;
            
            Axis axisX = new Axis();
            axisX.Title = GraphicsAxisData.Time.ToString();
            axisX.Labels = new[] { "Lun", "Mar", "Mer", "Jeu", "Ven" };
            axisX.MaxValue = double.NaN;
            cartesianChart.AxisX.Add(axisX);

            Axis axisY = new Axis();
            axisY.Title = GraphicsAxisData.NumberOfPeople.ToString();
            axisY.MaxValue = double.NaN;
            cartesianChart.AxisY.Add(axisY);

            DataContext = this;
            cartesianChart.VerticalAlignment = VerticalAlignment.Stretch;
            cartesianChart.HorizontalAlignment = HorizontalAlignment.Stretch;
            ugrGraph.Children.Clear();
            ugrGraph.Children.Add(cartesianChart);

            return cartesianChart;
        }

        private PieChart CreatePieGraph()
        {
            PieChart pieChart = new PieChart();
            pieChart.Series = new SeriesCollection();
            pieChart.LegendLocation = LegendLocation.Right;

            DataContext = this;
            pieChart.VerticalAlignment = VerticalAlignment.Stretch;
            pieChart.HorizontalAlignment = HorizontalAlignment.Stretch;

            ugrGraph.Children.Clear();
            ugrGraph.Children.Add(pieChart);

            return pieChart;
        }

        private void AxisValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbxAxis = (ComboBox)sender;
            Axis axis;
            switch (chart)
            {
                case CartesianChart cartesianChart:
                    switch ((GraphicsAxisData)cbxAxis.SelectedIndex)
                    {
                        case GraphicsAxisData.NumberOfPeople:
                            if ((string)cbxAxis.Tag == "X")
                                axis = cartesianChart.AxisX[0];
                            else
                                axis = cartesianChart.AxisY[0];

                            axis.Title = "Nombre de personnes";
                            axis.Labels = null;
                            break;
                        case GraphicsAxisData.Time:
                            if ((string)cbxAxis.Tag == "X")
                                axis = cartesianChart.AxisX[0];
                            else
                                axis = cartesianChart.AxisY[0];

                            axis.Title = "Temps";
                            axis.Labels = new[] { "Lun", "Mar", "Mer", "Jeu", "Ven" };
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

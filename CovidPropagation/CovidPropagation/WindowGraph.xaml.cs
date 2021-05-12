using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
        int cellX;
        int cellY;
        public WindowGraph(int cellX, int cellY)
        {
            InitializeComponent();
            this.cellX = cellX;
            this.cellY = cellY;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (OnSave != null)
                OnSave(10, new int[] { cellX, cellY });
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbxGraphType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            switch ((GraphicsType)cbxGraphType.SelectedValue)
            {
                default:
                case GraphicsType.Linear:
                    CartesianChart cartesianChart = new CartesianChart();

                    string[] labels;
                    Func<double, string> yFormatter;
                    cartesianChart.Series = new SeriesCollection
                    {
                        new LineSeries
                        {
                            Title = "Series 1",
                            Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                        },
                        new LineSeries
                        {
                            Title = "Series 2",
                            Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                            PointGeometry = null
                        },
                        new LineSeries
                        {
                            Title = "Series 3",
                            Values = new ChartValues<double> { 4,2,7,2,7 },
                            PointGeometry = DefaultGeometries.Square,
                            PointGeometrySize = 15
                        }
                    };
                    cartesianChart.LegendLocation = LegendLocation.Right;
                    Axis axisX = new Axis();
                    axisX.Title = "Sales";
                    yFormatter = value => value.ToString("C");
                    axisX.LabelFormatter = yFormatter;
                    cartesianChart.AxisX.Add(axisX);

                    Axis axisY = new Axis();
                    axisY.Title = "Sales";
                    labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
                    axisY.Labels = labels;
                    cartesianChart.AxisY.Add(axisY);

                    DataContext = this;
                    cartesianChart.VerticalAlignment = VerticalAlignment.Stretch;
                    cartesianChart.HorizontalAlignment = HorizontalAlignment.Stretch;
                    ugrGraph.Children.Clear();
                    ugrGraph.Children.Add(cartesianChart);
                    break;
                case GraphicsType.Vertical:
                    break;
                case GraphicsType.Horizontal:
                    MessageBox.Show("daw");
                    break;
                case GraphicsType.Cylinder:
                    break;
                case GraphicsType.HeatMap:
                    break;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

            if (Convert.ToInt32(e.Text) > 5)
            {
                e.Handled = false;
                tbxQuantityOfCurves.Text = "5";
            }
            else if (Convert.ToInt32(e.Text) < 1)
            {
                e.Handled = false;
                tbxQuantityOfCurves.Text = "1";
            }

            if (tbxQuantityOfCurves.Text.Length > 0)
            {
                e.Handled = true;
                tbxQuantityOfCurves.Text = tbxQuantityOfCurves.Text.ToChar().ToString();
            }
        }
    }
}

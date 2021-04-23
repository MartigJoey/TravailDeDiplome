using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace CovidPropagation
{
    /// <summary>
    /// Logique d'interaction pour LC.xaml
    /// </summary>
    public partial class LC : Window
    {
        public LC()
        {
            InitializeComponent();
            CreateGraphLinear();
            CreateGraphColumn();
            CreateGraphHeatMap();
            CreateGraphRow();
            CreateGraphPieChart();
            CreateGraphBubbles();
            CreateGraphStackedColumn();
            CreateGraphStackedArea();
        }

        public SeriesCollection SeriesCollection1 { get; set; }
        public string[] Labels1 { get; set; }
        public Func<double, string> YFormatter1 { get; set; }
        private void CreateGraphLinear()
        {
            SeriesCollection1 = new SeriesCollection
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

            Labels1 = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter1 = value => value.ToString("C");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection2 { get; set; }
        public string[] Labels2 { get; set; }
        public Func<double, string> Formatter2 { get; set; }
        private void CreateGraphColumn()
        {
            SeriesCollection2 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            SeriesCollection2.Add(new ColumnSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            //also adding values updates and animates the chart automatically
            SeriesCollection2[1].Values.Add(48d);

            Labels2 = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter2 = value => value.ToString("N");

            DataContext = this;
        }

        public ChartValues<HeatPoint> Values3 { get; set; }
        public string[] Days3 { get; set; }
        public string[] SalesMan3 { get; set; }
        private void CreateGraphHeatMap()
        {

            var r = new Random();

            Values3 = new ChartValues<HeatPoint>
            {
                //X means sales man
                //Y is the day
 
                //"Jeremy Swanson"
                new HeatPoint(0, 0, r.Next(0, 10)),
                new HeatPoint(0, 1, r.Next(0, 10)),
                new HeatPoint(0, 2, r.Next(0, 10)),
                new HeatPoint(0, 3, r.Next(0, 10)),
                new HeatPoint(0, 4, r.Next(0, 10)),
                new HeatPoint(0, 5, r.Next(0, 10)),
                new HeatPoint(0, 6, r.Next(0, 10)),
 
                //"Lorena Hoffman"
                new HeatPoint(1, 0, r.Next(0, 10)),
                new HeatPoint(1, 1, r.Next(0, 10)),
                new HeatPoint(1, 2, r.Next(0, 10)),
                new HeatPoint(1, 3, r.Next(0, 10)),
                new HeatPoint(1, 4, r.Next(0, 10)),
                new HeatPoint(1, 5, r.Next(0, 10)),
                new HeatPoint(1, 6, r.Next(0, 10)),
 
                //"Robyn Williamson"
                new HeatPoint(2, 0, r.Next(0, 10)),
                new HeatPoint(2, 1, r.Next(0, 10)),
                new HeatPoint(2, 2, r.Next(0, 10)),
                new HeatPoint(2, 3, r.Next(0, 10)),
                new HeatPoint(2, 4, r.Next(0, 10)),
                new HeatPoint(2, 5, r.Next(0, 10)),
                new HeatPoint(2, 6, r.Next(0, 10)),
 
                //"Carole Haynes"
                new HeatPoint(3, 0, r.Next(0, 10)),
                new HeatPoint(3, 1, r.Next(0, 10)),
                new HeatPoint(3, 2, r.Next(0, 10)),
                new HeatPoint(3, 3, r.Next(0, 10)),
                new HeatPoint(3, 4, r.Next(0, 10)),
                new HeatPoint(3, 5, r.Next(0, 10)),
                new HeatPoint(3, 6, r.Next(0, 10)),
 
                //"Essie Nelson"
                new HeatPoint(4, 0, r.Next(0, 10)),
                new HeatPoint(4, 1, r.Next(0, 10)),
                new HeatPoint(4, 2, r.Next(0, 10)),
                new HeatPoint(4, 3, r.Next(0, 10)),
                new HeatPoint(4, 4, r.Next(0, 10)),
                new HeatPoint(4, 5, r.Next(0, 10)),
                new HeatPoint(4, 6, r.Next(0, 10))
            };

            Days3 = new[]
            {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday"
            };

            SalesMan3 = new[]
            {
                "Jeremy Swanson",
                "Lorena Hoffman",
                "Robyn Williamson",
                "Carole Haynes",
                "Essie Nelson"
            };

            DataContext = this;

            r = new Random();
            foreach (var chartValue in Values3)
            {
                chartValue.Weight = r.Next(0, 10);
            }
        }

        public SeriesCollection SeriesCollection4 { get; set; }
        public string[] Labels4 { get; set; }
        public Func<double, string> Formatter4 { get; set; }
        private void CreateGraphRow()
        {
            SeriesCollection4 = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            SeriesCollection4.Add(new RowSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            //also adding values updates and animates the chart automatically
            SeriesCollection4[1].Values.Add(48d);

            Labels4 = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter4 = value => value.ToString("N");

            DataContext = this;
        }

        public Func<ChartPoint, string> PointLabel5 { get; set; }
        private void CreateGraphPieChart()
        {
            PointLabel5 = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            DataContext = this;
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        private void CreateGraphBubbles()
        {
            SeriesCollection6 = new SeriesCollection
            {
                new ScatterSeries
                {
                    Values = new ChartValues<ScatterPoint>
                    {
                        new ScatterPoint(5, 5, 20),
                        new ScatterPoint(3, 4, 80),
                        new ScatterPoint(7, 2, 20),
                        new ScatterPoint(2, 6, 60),
                        new ScatterPoint(8, 2, 70)
                    },
                    MinPointShapeDiameter = 15,
                    MaxPointShapeDiameter = 45
                },
                new ScatterSeries
                {
                    Values = new ChartValues<ScatterPoint>
                    {
                        new ScatterPoint(7, 5, 1),
                        new ScatterPoint(2, 2, 1),
                        new ScatterPoint(1, 1, 1),
                        new ScatterPoint(6, 3, 1),
                        new ScatterPoint(8, 8, 1)
                    },
                    PointGeometry = DefaultGeometries.Triangle,
                    MinPointShapeDiameter = 15,
                    MaxPointShapeDiameter = 45
                }
            };

            DataContext = this;
        }
        public SeriesCollection SeriesCollection6 { get; set; }

        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var series in SeriesCollection6)
            {
                foreach (var bubble in series.Values.Cast<ScatterPoint>())
                {
                    bubble.X = r.NextDouble() * 10;
                    bubble.Y = r.NextDouble() * 10;
                    bubble.Weight = r.NextDouble() * 10;
                }
            }
        }

        public SeriesCollection SeriesCollection7 { get; set; }
        public string[] Labels7 { get; set; }
        public Func<double, string> Formatter7 { get; set; }
        private void CreateGraphStackedColumn()
        {
            SeriesCollection7 = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Values = new ChartValues<double> {4, 5, 6, 8},
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                    DataLabels = true
                },
                new StackedColumnSeries
                {
                    Values = new ChartValues<double> {2, 5, 6, 7},
                    StackMode = StackMode.Values,
                    DataLabels = true
                }
            };

            //adding series updates and animates the chart
            SeriesCollection7.Add(new StackedColumnSeries
            {
                Values = new ChartValues<double> { 6, 2, 7 },
                StackMode = StackMode.Values
            });

            //adding values also updates and animates
            SeriesCollection7[2].Values.Add(4d);

            Labels7 = new[] { "Chrome", "Mozilla", "Opera", "IE" };
            Formatter7 = value => value + " Mill";

            DataContext = this;
        }


        private Func<double, string> _yFormatter8;
        public SeriesCollection SeriesCollection8 { get; set; }
        public Func<double, string> XFormatter8 { get; set; }

        public Func<double, string> YFormatter8
        {
            get { return _yFormatter8; }
            set
            {
                _yFormatter8 = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void CreateGraphStackedArea()
        {
            SeriesCollection8 = new SeriesCollection
            {
                new StackedAreaSeries
                {
                    Title = "Africa",
                    Values = new ChartValues<DateTimePoint>
                    {
                        new DateTimePoint(new DateTime(1950, 1, 1), .228),
                        new DateTimePoint(new DateTime(1960, 1, 1), .285),
                        new DateTimePoint(new DateTime(1970, 1, 1), .366),
                        new DateTimePoint(new DateTime(1980, 1, 1), .478),
                        new DateTimePoint(new DateTime(1990, 1, 1), .629),
                        new DateTimePoint(new DateTime(2000, 1, 1), .808),
                        new DateTimePoint(new DateTime(2010, 1, 1), 1.031),
                        new DateTimePoint(new DateTime(2013, 1, 1), 1.110)
                    },
                    LineSmoothness = 0
                },
                new StackedAreaSeries
                {
                    Title = "N & S America",
                    Values = new ChartValues<DateTimePoint>
                    {
                        new DateTimePoint(new DateTime(1950, 1, 1), .339),
                        new DateTimePoint(new DateTime(1960, 1, 1), .424),
                        new DateTimePoint(new DateTime(1970, 1, 1), .519),
                        new DateTimePoint(new DateTime(1980, 1, 1), .618),
                        new DateTimePoint(new DateTime(1990, 1, 1), .727),
                        new DateTimePoint(new DateTime(2000, 1, 1), .841),
                        new DateTimePoint(new DateTime(2010, 1, 1), .942),
                        new DateTimePoint(new DateTime(2013, 1, 1), .972)
                    },
                    LineSmoothness = 0
                },
                new StackedAreaSeries
                {
                    Title = "Asia",
                    Values = new ChartValues<DateTimePoint>
                    {
                        new DateTimePoint(new DateTime(1950, 1, 1), 1.395),
                        new DateTimePoint(new DateTime(1960, 1, 1), 1.694),
                        new DateTimePoint(new DateTime(1970, 1, 1), 2.128),
                        new DateTimePoint(new DateTime(1980, 1, 1), 2.634),
                        new DateTimePoint(new DateTime(1990, 1, 1), 3.213),
                        new DateTimePoint(new DateTime(2000, 1, 1), 3.717),
                        new DateTimePoint(new DateTime(2010, 1, 1), 4.165),
                        new DateTimePoint(new DateTime(2013, 1, 1), 4.298)
                    },
                    LineSmoothness = 0
                },
                new StackedAreaSeries
                {
                    Title = "Europe",
                    Values = new ChartValues<DateTimePoint>
                    {
                        new DateTimePoint(new DateTime(1950, 1, 1), .549),
                        new DateTimePoint(new DateTime(1960, 1, 1), .605),
                        new DateTimePoint(new DateTime(1970, 1, 1), .657),
                        new DateTimePoint(new DateTime(1980, 1, 1), .694),
                        new DateTimePoint(new DateTime(1990, 1, 1), .723),
                        new DateTimePoint(new DateTime(2000, 1, 1), .729),
                        new DateTimePoint(new DateTime(2010, 1, 1), .740),
                        new DateTimePoint(new DateTime(2013, 1, 1), .742)
                    },
                    LineSmoothness = 0
                }
            };

            XFormatter8 = val => new DateTime((long)val).ToString("yyyy");
            YFormatter8 = val => val.ToString("N") + " M";

            DataContext = this;
        }
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

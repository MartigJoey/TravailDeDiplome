using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CovidPropagation
{
    public delegate void SaveEventHandler(object source, ChartData e);

    /// <summary>
    /// Logique d'interaction pour WindowGraph.xaml
    /// </summary>
    public partial class WindowChart : Window
    {
        public event SaveEventHandler OnSave;
        private const int MAX_NUMBER_OF_CURVES = 5;
        int cellX;
        int cellY;
        int sizeX;
        int sizeY;
        ComboBox[] cbxDatas;
        ChartData graphicDatas;
        int currentCurvesIndex;
        object chart;

        public WindowChart(int cellX, int cellY, int sizeX, int sizeY, ChartData graphicDatas)
        {
            InitializeComponent();
            this.cellX = cellX;
            this.cellY = cellY;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.graphicDatas = graphicDatas;
            cbxDatas = new ComboBox[MAX_NUMBER_OF_CURVES];

            // Créé et positionne les combobox de valeurs pour leur futur utilisation.
            int curvesRow = Grid.GetRow(cbxQuantityOfCurves);
            int curvesColumn = Grid.GetColumn(cbxQuantityOfCurves);
            for (int i = 1; i <= MAX_NUMBER_OF_CURVES; i++)
            {
                cbxQuantityOfCurves.Items.Add(i);
                cbxDatas[i - 1] = new ComboBox();

                curvesRow++;
                Grid.SetRow(cbxDatas[i - 1], curvesRow);
                Grid.SetColumn(cbxDatas[i - 1], curvesColumn);

                cbxDatas[i - 1].ItemsSource = from ChartsDisplayData n 
                                               in Enum.GetValues(typeof(ChartsDisplayData))
                                               select GetEnumDescription(n);

                cbxDatas[i - 1].SelectedIndex = 0;
                cbxDatas[i - 1].Margin = new Thickness(0, 2, 0, 2);
                grdContent.Children.Add(cbxDatas[i - 1]);
            }

            // Sélectionne les description des enums et les insères dans les combobox.
            cbxValueX.ItemsSource = from ChartsAxisData n
                                    in Enum.GetValues(typeof(ChartsAxisData))
                                    select GetEnumDescription(n);

            cbxValueY.ItemsSource = from ChartsAxisData n
                                    in Enum.GetValues(typeof(ChartsAxisData))
                                    select GetEnumDescription(n);

            cbxGraphType.ItemsSource = from ChartsType n
                                       in Enum.GetValues(typeof(ChartsType))
                                       select GetEnumDescription(n);

            cbxDisplayInterval.ItemsSource = from ChartsDisplayInterval n
                                             in Enum.GetValues(typeof(ChartsDisplayInterval))
                                             select GetEnumDescription(n);

            cbxValueX.SelectedIndex = this.graphicDatas.AxisX;
            cbxValueY.SelectedIndex = this.graphicDatas.AxisY;
            cbxGraphType.SelectedIndex = this.graphicDatas.ChartType;
            cbxDisplayInterval.SelectedIndex = this.graphicDatas.DisplayInterval;


            currentCurvesIndex = this.graphicDatas.Datas.Length - 1;
            cbxQuantityOfCurves.SelectedIndex = currentCurvesIndex;

            for (int i = 0; i < this.graphicDatas.Datas.Length; i++)
            {
                cbxDatas[i].Visibility = Visibility.Visible;
                cbxDatas[i].SelectedIndex = this.graphicDatas.Datas[i];
            }
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

        /// <summary>
        /// Lorsque l'utilisateur sauvegarde les valeurs modifiées.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (OnSave != null)
            {
                List<int> datas = new List<int>();
                for (int i = 0; i <= cbxQuantityOfCurves.SelectedIndex; i++)
                {
                    datas.Add(cbxDatas[i].SelectedIndex);
                }
                ChartData graphicDatas = new ChartData(cellX, cellY, sizeX, sizeY, datas.ToArray(), cbxGraphType.SelectedIndex, cbxValueX.SelectedIndex, cbxValueY.SelectedIndex);
                OnSave(10, graphicDatas);
                this.Close();
            }
        }

        /// <summary>
        /// Lorsque l'utilisateur décide d'annuler ses modification du graphique.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Lorsqu'un type de graphique est selectionné, affiche le bon graphique ainsi que les paramètres disponibles.
        /// </summary>
        private void GraphType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxValueX != null)
            {
                cbxValueX.IsEnabled = true;
                cbxValueY.IsEnabled = true;
                cbxQuantityOfCurves.IsEnabled = true;
            }
            switch ((ChartsType)cbxGraphType.SelectedIndex)
            {
                case ChartsType.Linear:
                    chart = CreateCartesianGraph();
                    SetData(DisplayCurvesOnGraph);
                    break;
                case ChartsType.Vertical:
                    chart = CreateCartesianGraph(); 
                    SetData(DisplayColumnsOnGraph);
                    break;
                case ChartsType.Horizontal:
                    chart = CreateCartesianGraph();
                    SetData(DisplayRowsOnGraph);
                    break;
                case ChartsType.PieChart:
                    chart = CreatePieGraph();
                    SetData(DisplayPieSectionOnGraph);
                    if (cbxValueX != null)
                    {
                        cbxValueX.IsEnabled = false;
                        cbxValueY.IsEnabled = false;
                        cbxQuantityOfCurves.SelectedIndex = 1;
                    }
                    break;
                case ChartsType.HeatMap:
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

        /// <summary>
        /// Lorsque le nombre de données à afficher est modifié par l'utilisateur.
        /// Modifie les données qui sont affichées dans le graphique pour en ajouter ou en retirer.
        /// </summary>
        private void DataQuantity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentCurvesIndex = cbxQuantityOfCurves.Items.IndexOf(cbxQuantityOfCurves.SelectedItem);
            
            switch ((ChartsType)cbxGraphType.SelectedIndex)
            {
                case ChartsType.Linear:
                    SetData(DisplayCurvesOnGraph);
                    break;
                case ChartsType.Vertical:
                    SetData(DisplayColumnsOnGraph);
                    break;
                case ChartsType.Horizontal:
                    SetData(DisplayRowsOnGraph);
                    break;
                case ChartsType.PieChart:
                    SetData(DisplayPieSectionOnGraph);
                    break;
                case ChartsType.HeatMap:
                    SetData(DisplayHeatMapOnGraph);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Affiche le nombre de combobox nécessaire en fonction du nombre de données à afficher choisis par l'utilisateur
        /// </summary>
        /// <param name="callback">Méthode qui sera appelée pour l'affichage des données du graphique (Courbe, colonne, etc.)</param>
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

        /// <summary>
        /// SetData utilisé pour les graphiques heatmap qui ne contient qu'une "courbe" de valeur et qui n'a donc besoin que d'un unique appelle.
        /// </summary>
        /// <param name="callback">Méthode qui sera appelée pour l'affichage des données du graphique (Courbe, colonne, etc.)</param>
        private void SetData(Func<bool> callback)
        {
            if (cbxDatas != null)
            {
               callback();
            }
        }

        /// <summary>
        /// Affiche les courbes sur un graphique cartesien.
        /// Définit si une courbe doit rester affichée, être ajoutée, ou supprimée.
        /// </summary>
        /// <param name="index">Index de la courbe à modifier.</param>
        /// <param name="isDisplayed">Si la courbe doit être affichée ou non.</param>
        /// <returns>Valeur obligatoir à l'utilisation de callback</returns>
        private bool DisplayCurvesOnGraph(int index, bool isDisplayed)
        {
            chart ??= CreateCartesianGraph();

            CartesianChart cartesianChart = (CartesianChart)chart;

            // Si l'index doit être affiché et n'existe pas déjà
            if (isDisplayed && index > cartesianChart.Series.GetLastIndex())
            {
                Random rdm = GlobalVariables.rdm;
                cartesianChart.Series.Add(new LineSeries
                {
                    Title = cbxDatas[index].SelectedItem.ToString(),
                    Values = new ChartValues<double> { 
                        rdm.Next(1, 10), 
                        rdm.Next(1, 10), 
                        rdm.Next(1, 10), 
                        rdm.Next(1, 10), 
                        rdm.Next(1, 10) 
                    },
                    PointGeometry = null,
                    DataLabels = false
                });
            }
            else if(!isDisplayed) // Sinon retire le dernier index
            {
                int lastIndex = cartesianChart.Series.GetLastIndex();
                if (lastIndex > 0 && lastIndex > currentCurvesIndex)
                    cartesianChart.Series.RemoveAt(lastIndex);
            }
            return true;
        }

        /// <summary>
        /// Affiche les colonnes sur un graphique cartesien.
        /// Définit si une colonne doit rester affichée, être ajoutée, ou supprimée.
        /// </summary>
        /// <param name="index">Index de la colonne à modifier.</param>
        /// <param name="isDisplayed">Si la colonne doit être affichée ou non.</param>
        /// <returns>Valeur obligatoir à l'utilisation de callback</returns>
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
                    Values = new ChartValues<double> {
                        rdm.Next(1, 10),
                        rdm.Next(1, 10),
                        rdm.Next(1, 10),
                        rdm.Next(1, 10),
                        rdm.Next(1, 10)
                    },
                    PointGeometry = null,
                    DataLabels = false
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

        /// <summary>
        /// Affiche les lignes sur un graphique cartesien.
        /// Définit si une ligne doit rester affichée, être ajoutée, ou supprimée.
        /// </summary>
        /// <param name="index">Index de la ligne à modifier.</param>
        /// <param name="isDisplayed">Si la ligne doit être affichée ou non.</param>
        /// <returns>Valeur obligatoir à l'utilisation de callback</returns>
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
                    Values = new ChartValues<double> {
                        rdm.Next(1, 10),
                        rdm.Next(1, 10),
                        rdm.Next(1, 10),
                        rdm.Next(1, 10),
                        rdm.Next(1, 10)
                    },
                    PointGeometry = null,
                    DataLabels = false
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

        /// <summary>
        /// Affiche les sections sur un graphique cylindrique.
        /// Définit si une section doit rester affichée, être ajoutée, ou supprimée.
        /// </summary>
        /// <param name="index">Index de la section à modifier.</param>
        /// <param name="isDisplayed">Si la section doit être affichée ou non.</param>
        /// <returns>Valeur obligatoir à l'utilisation de callback</returns>
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
                    Values = new ChartValues<double> {
                        rdm.Next(1, 10)
                    },
                    PointGeometry = null,
                    DataLabels = false
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

        /// <summary>
        /// Remplit le graphique heatmap de valeur s'il n'y en a pas déjà.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isDisplayed"></param>
        /// <returns>Valeur obligatoir à l'utilisation de callback</returns>
        private bool DisplayHeatMapOnGraph()
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

        /// <summary>
        /// Lorsque la valeur choisie par l'utilisateur pour une courbe est modifié.
        /// Change la valeur le nom de la courbe ainsi que ses valeurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxDatas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbxData = (ComboBox)sender;
            switch ((ChartsType)cbxGraphType.SelectedIndex)
            {
                case ChartsType.Linear:
                    CartesianChart linearChart = (CartesianChart)chart;
                    linearChart.Series[Convert.ToInt32(cbxData.Tag)] = new LineSeries
                    {
                        Title = cbxData.SelectedItem.ToString(),
                        Values = linearChart.Series[Convert.ToInt32(cbxData.Tag)].Values
                    };
                    break;
                case ChartsType.Vertical:
                    CartesianChart verticalChart = (CartesianChart)chart;
                    verticalChart.Series[Convert.ToInt32(cbxData.Tag)] = new ColumnSeries
                    {
                        Title = cbxData.SelectedItem.ToString(),
                        Values = verticalChart.Series[Convert.ToInt32(cbxData.Tag)].Values
                    };
                    break;
                case ChartsType.Horizontal:
                    CartesianChart horizontalChart = (CartesianChart)chart;
                    horizontalChart.Series[Convert.ToInt32(cbxData.Tag)] = new RowSeries
                    {
                        Title = cbxData.SelectedItem.ToString(),
                        Values = horizontalChart.Series[Convert.ToInt32(cbxData.Tag)].Values
                    };
                    break;
                case ChartsType.PieChart:
                    PieChart pieChart = (PieChart)chart;
                    pieChart.Series[Convert.ToInt32(cbxData.Tag)] = new PieSeries
                    {
                        Title = cbxData.SelectedItem.ToString(),
                        Values = pieChart.Series[Convert.ToInt32(cbxData.Tag)].Values
                    };
                    break;
                case ChartsType.HeatMap:
                    chart = CreateCartesianGraph();
                    SetData(DisplayHeatMapOnGraph);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Créé un graphique cartesien avec des valeurs par défaut.
        /// </summary>
        /// <returns>Graphique créé.</returns>
        private CartesianChart CreateCartesianGraph()
        {
            CartesianChart cartesianChart = new CartesianChart();
            cartesianChart.Series = new SeriesCollection();
            cartesianChart.LegendLocation = LegendLocation.Right;
            cartesianChart.Foreground = Brushes.Gray;
            cartesianChart.DisableAnimations = true;
            cartesianChart.Hoverable = false;

            Axis axisX = new Axis();
            axisX.Foreground = Brushes.Gray;
            axisX.MaxValue = double.NaN;
            axisX.Title = GetEnumDescription((ChartsAxisData)graphicDatas.AxisX);
            cartesianChart.AxisX.Add(axisX);

            Axis axisY = new Axis();
            axisY.Foreground = Brushes.Gray;
            axisY.MaxValue = double.NaN;
            axisY.Title = GetEnumDescription((ChartsAxisData)graphicDatas.AxisY);
            cartesianChart.AxisY.Add(axisY);

            DataContext = this;
            cartesianChart.VerticalAlignment = VerticalAlignment.Stretch;
            cartesianChart.HorizontalAlignment = HorizontalAlignment.Stretch;
            ugrGraph.Children.Clear();
            ugrGraph.Children.Add(cartesianChart);

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
            pieChart.DisableAnimations = true;
            pieChart.Hoverable = false;

            DataContext = this;
            pieChart.VerticalAlignment = VerticalAlignment.Stretch;
            pieChart.HorizontalAlignment = HorizontalAlignment.Stretch;

            ugrGraph.Children.Clear();
            ugrGraph.Children.Add(pieChart);

            return pieChart;
        }

        /// <summary>
        /// Lorsque la valeur d0un des axes est modifée.
        /// Change le nom de l'axe ainsi que ses valeurs.
        /// </summary>
        private void AxisValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbxAxis = (ComboBox)sender;
            ChartsAxisData axisData = (ChartsAxisData)cbxAxis.SelectedIndex;
            CartesianChart cartesianChart = (CartesianChart)chart;
            Axis axis;

            if ((string)cbxAxis.Tag == "X")
                axis = cartesianChart.AxisX[0];
            else
                axis = cartesianChart.AxisY[0];

            if (axisData >= 0)
            {
                if ((string)cbxAxis.Tag == "X")
                    axis.Title = GetEnumDescription(axisData);
                else
                    axis.Title = GetEnumDescription(axisData);
            }
        }
    }
}

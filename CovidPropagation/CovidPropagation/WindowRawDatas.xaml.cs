using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// <summary>
    /// Logique d'interaction pour Legend.xaml
    /// </summary>
    public partial class WindowRawDatas : Window
    {
        List<Label> labelsName;
        List<Label> labelsValue;
        public WindowRawDatas()
        {
            InitializeComponent();
            labelsName = new List<Label>();
            labelsValue = new List<Label>();
        }

        public void CreateLabels(SimulationDatas datas)
        {
            CreateRows((datas.CentralizedDatas.Count + 1) * 2);

            // Last
            int columnLast = 0;
            int rowLast = 1;
            CreateTitleLabel("Denières valeurs enregistrée:", columnLast, rowLast - 1);
            rowLast = CreateLabelsGroup(columnLast, rowLast, datas);

            // Average
            int columnAverage = 2;
            int rowAverage = 1;
            CreateTitleLabel("Moyenne des valeurs enregistrées:", columnAverage, rowAverage - 1);
            CreateLabelsGroup(columnAverage, rowAverage, datas);

            // Max
            int columnMax = 0;
            int rowMax = rowLast + 1;
            CreateTitleLabel("Valeurs maximums enregistrées:", columnMax, rowMax - 1);
            CreateLabelsGroup(columnMax, rowMax, datas);

            // Min
            int columnMin = 2;
            int rowMin = rowLast + 1;
            CreateTitleLabel("Valeurs minimums enregistrées:", columnMin, rowMin - 1);
            CreateLabelsGroup(columnMin, rowMin, datas);
        }

        private int CreateLabelsGroup(int column, int row, SimulationDatas datas)
        {
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                CreateLabel(item.Key, item.Value.Last(), column, row);
                row++;
            }
            return row;
        }

        private void CreateLabel(string key, double value, int x, int y)
        {
            Label labelName = new Label();
            Label labelValue = new Label();

            labelName.Content = $"{key}: ";
            labelName.Foreground = Brushes.LightGray;
            labelName.FontSize = 15;

            labelValue.Content = $"{value}";
            labelValue.Foreground = Brushes.LightGray;
            labelValue.FontSize = 15;
            labelValue.BorderThickness = new Thickness(0.5d,0,3,0);
            labelValue.BorderBrush = Brushes.LightGray;

            Grid.SetColumn(labelName, x);
            Grid.SetRow(labelName, y);

            Grid.SetColumn(labelValue, x + 1);
            Grid.SetRow(labelValue, y);

            grdContent.Children.Add(labelName);
            grdContent.Children.Add(labelValue);

            labelsName.Add(labelName);
            labelsValue.Add(labelValue);
        }

        private void CreateTitleLabel(string content, int x, int y)
        {
            Label labelTitle = new Label();

            labelTitle.Content = $"{content}";
            labelTitle.Foreground = this.FindResource("normalDark") as Brush; 
            labelTitle.FontSize = 18;
            labelTitle.Background = this.FindResource("normalGreen") as Brush;

            Grid.SetColumn(labelTitle, x);
            Grid.SetRow(labelTitle, y);
            Grid.SetColumnSpan(labelTitle, 2);

            grdContent.Children.Add(labelTitle);
        }

        private void CreateRows(int nbRows)
        {
            for (int i = 0; i < nbRows; i++)
            {
                RowDefinition newRow = new RowDefinition();
                newRow.MaxHeight = 30;
                newRow.MinHeight = 30;
                grdContent.RowDefinitions.Add(newRow);
            }
        }

        public void UpdateLabels(SimulationDatas datas)
        {
            // Affiche les dernières données de la simulation.
            int labelsIndex = 0;
            int index = 0;
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                labelsName[labelsIndex].Content = $"{item.Key}: ";
                labelsValue[labelsIndex].Content = $"{item.Value.Last().ToString("F2")}";
                labelsIndex++;
            }

            // Affiche la moyenne des données de la simulation.
            index = 0;
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                labelsName[labelsIndex].Content = $"{item.Key}: ";
                labelsValue[labelsIndex].Content = $"{item.Value.Average().ToString("F2")}";
                labelsIndex++;
                index++;
            }

            // Affiche la valeur maximum enregistrée des données de la simulation
            index = 0;
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                labelsName[labelsIndex].Content = $"{item.Key}: ";
                labelsValue[labelsIndex].Content = $"{item.Value.Max().ToString("F2")}";
                labelsIndex++;
                index++;
            }

            // Affiche la valeur minimum enregistrée des données de la simulation
            index = 0;
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                labelsName[labelsIndex].Content = $"{item.Key}: ";
                labelsValue[labelsIndex].Content = $"{item.Value.Min().ToString("F2")}";
                labelsIndex++;
                index++;
            }
        }
    }
}

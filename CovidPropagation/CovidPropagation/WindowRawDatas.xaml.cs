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
        List<Label> labels;
        public WindowRawDatas()
        {
            InitializeComponent();
            labels = new List<Label>();
        }

        public void CreateLabels(SimulationDatas datas)
        {
            // Last
            int columnLast = 0;
            int rowLast = 2;
            CreateLabel("Denières valeurs enregistrée:", columnLast, rowLast - 1);
            CreateRow();
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                CreateLabel(item.Key, item.Value.Last(), columnLast, rowLast);
                CreateRow();
                rowLast++;
            }

            // Average
            int columnAverage = 1;
            int rowAverage = 2;
            CreateLabel("Moyenne des valeurs enregistrées:", columnAverage, rowAverage - 1);
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                CreateLabel(item.Key, item.Value.Last(), columnAverage, rowAverage);
                rowAverage++;
            }

            // Max
            int columnMax = 0;
            int rowMax = rowLast + 2;
            CreateLabel("Valeurs maximums enregistrées:", columnMax, rowMax - 1);
            CreateRow();
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                CreateLabel(item.Key, item.Value.Last(), columnMax, rowMax);
                CreateRow();
                rowMax++;
            }

            // Min
            int columnMin = 1;
            int rowMin = rowLast + 2;
            CreateLabel("Valeurs minimums enregistrées:", columnMin, rowMin - 1);
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                CreateLabel(item.Key, item.Value.Last(), columnMin, rowMin);
                rowMin++;
            }
        }

        private void CreateLabel(string key, double value, int x, int y)
        {
            string content = $"{key}: {value}";
            labels.Add(CreateLabel(content, x, y));
        }

        private Label CreateLabel(string content, int x, int y)
        {
            Label labelDatas = new Label();

            labelDatas.Content = $"{content}";
            labelDatas.Foreground = Brushes.LightGray;
            labelDatas.FontSize = 15;

            Grid.SetColumn(labelDatas, x);
            Grid.SetRow(labelDatas, y);

            grdContent.Children.Add(labelDatas);

            return labelDatas;
        }

        private void CreateRow()
        {
            RowDefinition newRow = new RowDefinition();
            newRow.MaxHeight = 30;
            newRow.MinHeight = 30;
            grdContent.RowDefinitions.Add(newRow);
        }

        public void UpdateLabels(SimulationDatas datas)
        {
            // Affiche les dernières données de la simulation.
            int labelsIndex = 0;
            int index = 0;
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                labels[labelsIndex].Content = $"{item.Key}: {item.Value.Last()}";
                labelsIndex++;
            }

            // Affiche la moyenne des données de la simulation.
            index = 0;
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                labels[labelsIndex].Content = $"{item.Key}: {item.Value.Average().ToString("F1")}";
                labelsIndex++;
                index++;
            }

            // Affiche la valeur maximum enregistrée des données de la simulation
            index = 0;
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                labels[labelsIndex].Content = $"{item.Key}: {item.Value.Max().ToString("F1")}";
                labelsIndex++;
                index++;
            }

            // Affiche la valeur minimum enregistrée des données de la simulation
            index = 0;
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                labels[labelsIndex].Content = $"{item.Key}: {item.Value.Min().ToString("F1")}";
                labelsIndex++;
                index++;
            }
        }
    }
}

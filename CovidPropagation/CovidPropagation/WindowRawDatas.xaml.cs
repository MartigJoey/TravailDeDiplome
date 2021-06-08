/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CovidPropagation
{
    /// <summary>
    /// Classe permettant l'affichage des données brutes de la simulation dans une page indépendante.
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

        /// <summary>
        /// Créé les différents groupes de labels et les positionnes.
        /// </summary>
        /// <param name="datas">Données de la simulation.</param>
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

        /// <summary>
        /// Créé plusieurs labels formant un groupe.
        /// </summary>
        /// <param name="column">Colonne où le groupe doit être affiché.</param>
        /// <param name="row">La ligne de départ de ce groupe de labels.</param>
        /// <param name="datas">Les données que les labels afficheront.</param>
        /// <returns></returns>
        private int CreateLabelsGroup(int column, int row, SimulationDatas datas)
        {
            foreach (KeyValuePair<string, List<double>> item in datas.CentralizedDatas)
            {
                CreateLabel(item.Key, item.Value.Last(), column, row);
                row++;
            }
            return row;
        }

        /// <summary>
        /// Créé les labels à la position demandée avec le contenu demandé.
        /// La clé étant le nom des données et la valeurs, la valeur des données.
        /// </summary>
        /// <param name="key">Nom des données.</param>
        /// <param name="value">Valeur des données.</param>
        /// <param name="x">Position X.</param>
        /// <param name="y">Position Y.</param>
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

        /// <summary>
        /// Créé les labels de titres à la position demandée avec le contenu demandé.
        /// Change la police ainsi que le background.
        /// </summary>
        /// <param name="content">Contenu du label.</param>
        /// <param name="x">Position X.</param>
        /// <param name="y">Position Y.</param>
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

        /// <summary>
        /// Créé une ou plusieurs nouvelles lignes dans la grid.
        /// </summary>
        /// <param name="nbRows">Le nombre de ligne à ajouter.</param>
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

        /// <summary>
        /// Met à jour les labels avec les derières données.
        /// </summary>
        /// <param name="datas">Données à afficher</param>
        public void UpdateLabels(SimulationDatas datas)
        {
            try
            {
                Dispatcher.Invoke(() =>
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
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Update labels error : " + ex);
            }
        }
    }
}
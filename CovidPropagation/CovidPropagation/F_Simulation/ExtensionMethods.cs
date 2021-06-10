/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Geared;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CovidPropagation
{
    static class ExtensionMethods
    {
        /// <summary>
        /// Convertis un booléen en int.
        /// </summary>
        /// <returns>1 ou 0 représentat la valeur du bool.</returns>
        public static int ConvertToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        /// <summary>
        /// Choisis aléatoirement (50/50) si la valeur retournée est true ou false.
        /// </summary>
        /// <returns>Résultat du flipCoing</returns>
        public static bool NextBoolean(this Random value)
        {
            return Convert.ToBoolean(value.Next(0, 2));
        }

        /// <summary>
        /// Choisis aléatoirement une valeur qui résultera en true/false. Le true à un poid définit.
        /// </summary>
        /// <param name="trueWeight">Poid du true (format double 0 à 1)</param>
        /// <returns></returns>
        public static bool NextBoolean(this Random value, double trueWeight)
        {
            bool result = false;
            if (value.NextDouble() < trueWeight)
                result = true;

            return result;
        }

        /// <summary>
        /// Choisi en fonction des probabilité données un objet de du tableau et retourne celui sélectionné. Chaque objet à un poid qui défini les chances qu'il a d'être sélectionné.
        /// La somme des poids ne peut être plus grande ou plus petite que 1
        /// </summary>
        /// <param name="weight">Poid de l'objet</param>
        /// <returns>L'objet qui a été choisis</returns>
        public static object NextProbability(this Random value, KeyValuePair<object, double>[] weight)
        {
            double weightSum = weight.Sum(w => w.Value);
            if (weightSum > 1 && weightSum < 1)
                throw new ArgumentException("Weight array sum must be equal to 1.");

            object result = 0;
            double rdm = value.NextDouble();
            double sumPreviousWeight = 0;
            weight = weight.OrderBy(w => w.Value).ToArray();
            for (int i = 0; i < weight.Length; i++)
            {
                sumPreviousWeight += weight[i].Value;
                if (rdm < sumPreviousWeight)
                {
                    result = weight[i].Key;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Si le résultat obtenu est trop proche du maximum, la valeur resultante est modifiée pour obtenir un résultat suffisant.
        /// ⚠ Inclusif ⚠
        /// </summary>
        /// <param name="minValue">Valeurs minimum requise entre le résultat du random et le maximum possible</param>
        /// <returns></returns>
        public static int NextWithMinimum(this Random value, int minRdm, int maxRdm, int minValue)
        {
            int result = value.Next(minRdm, maxRdm + 1);

            if (result > (maxRdm - minValue) && result != maxRdm)
                result = maxRdm - minValue;

            return result;
        }

        /// <summary>
        /// Identique à Random.Next() mais la valeur la plus grande est incluse.
        /// </summary>
        /// <param name="minRdm">Valeur minimum à tirer au sort.</param>
        /// <param name="maxRdm">Valeur maximum à tirer au sort.</param>
        /// <returns></returns>
        public static int NextInclusive(this Random value, int minRdm, int maxRdm)
        {
            return value.Next(minRdm, maxRdm + 1);
        }

        /// <summary>
        /// Identique à Random.NextDouble() mais avec un minimum et un maximum.
        /// </summary>
        /// <param name="minRdm">Valeur minimum à tirer au sort.</param>
        /// <param name="maxRdm">Valeur maximum à tirer au sort.</param>
        /// <returns></returns>
        public static double NextDoubleInclusive(this Random value, double minRdm, double maxRdm)
        {
            return value.NextDouble() * (maxRdm - minRdm) + minRdm;
        }


        /// <summary>
        /// Permet de mélanger une liste de tout type.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = GlobalVariables.rdm.Next(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }

        /// <summary>
        /// Convertit un caractère string en char. Si la string est plus grande qu'un caractère, seul le premier est récupéré.
        /// </summary>
        /// <returns>Premier caractère de la string en char</returns>
        public static char ToChar(this string value)
        {
            return value.ToCharArray()[0];
        }

        /// <summary>
        /// Récupère le dernier index d'une liste.
        /// Permet d'éviter d'écrire List.Count - 1 lors d'un for ou de récupérer le dernier élément d'une liste.
        /// </summary>
        /// <returns>Dernier index de la liste.</returns>
        public static int GetLastIndex<T>(this IEnumerable<T> value)
        {
            int result = 0;
            if (value != null)
                result = value.Count() - 1;

            return result;
        }

        /// <summary>
        /// Si la valeur d'un double vaut NaN, alors le transform en 0.
        /// </summary>
        /// <returns>0 ou la valeur du double si autre que NaN</returns>
        public static double SetValueIfNaN(this double value)
        {
            return double.IsNaN(value) ? 0 : value;
        }

        #region Charts

        /// <summary>
        /// Lorsque les données d'un graphique cylindrique sont mise à jours.
        /// Récupère la dernière données de chaque section de la simulation et l'affiche.
        /// </summary>
        public static void OnDataUpdatePieChart(this PieChart chart, SimulationDatas e)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                ChartValues<double> cv;
                foreach (PieSeries serie in chart.Series)
                {
                    cv = new ChartValues<double>();
                    cv.Add(e.GetDataFromEnum((ChartsDisplayData)serie.Tag).Last());
                    serie.Values = cv;
                }
            }));
        }

        private static void SetMaxAxisValue(Axis axis, double axisMaxValue)
        {
            if (axisMaxValue <= 5)
                axis.MaxValue = 5;
            else
                axis.MaxValue = double.NaN;
        }

        /// <summary>
        /// Affiche les données des graphiques dans le format des différents graphiques.
        /// Affiche les données suivant l'interval de temps donné.
        /// Pour les graphiques à colonne et en ligne, les données sont réduites. 
        /// Par exemple, pour afficher une semaine, à la palce d'afficher 48*7 colonnes, 7 colonnes sont affichées correspondant aux jours.
        /// </summary>
        /// <param name="chart">Graphique à modifier.</param>
        /// <param name="e">Valeurs de la simulation.</param>
        /// <param name="isDisplayChange">Si l'affichage change et qu'il est nécessaire de recharger les données du graphiques.</param>
        public static void Display(this CartesianChart chart, SimulationDatas e, bool isDisplayChange)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                int interval = 48;
                Axis axisX = chart.AxisX[0];
                Axis axisY = chart.AxisY[0];
                Axis currentAxis = axisX;
                double maxValue = chart.Series[0].Values.Count;
                ChartData datas = (ChartData)chart.Tag;
                int displayWindow = datas.DisplayWindow;

                switch ((UIType)datas.UIType)
                {
                    case UIType.Linear:
                        axisX.MaxValue = maxValue;

                        // Récupère l'interval
                        switch ((ChartsDisplayInterval)datas.DisplayInterval)
                        {
                            default:
                            case ChartsDisplayInterval.Day:
                                interval = 48;
                                break;
                            case ChartsDisplayInterval.Week:
                                interval = 336;
                                break;
                            case ChartsDisplayInterval.Month:
                                interval = 1440;
                                break;
                            case ChartsDisplayInterval.Total:
                                interval = 0;
                                break;
                        }

                        double axisYMaxValue = 0;
                        // ID Documentation : Curve_Chart
                        foreach (LineSeries serie in chart.Series)
                        {
                            List<double> lineSerieDatas = new List<double>(e.GetDataFromEnum((ChartsDisplayData)serie.Tag));

                            // Si l'affichage n'est pas "total" et que lineSerieDatas a suffisament de données
                            if (interval != 0 && lineSerieDatas.GetLastIndex() > interval)
                            {
                                // Permet de garder le focus sur la dernière fenêtre d'informations
                                if (datas.AutoDisplay)
                                    displayWindow = lineSerieDatas.GetLastIndex() / interval;

                                // Permet d'éviter d'incrémenter displayWindow au delà de la quantité d'interval actuellement disponible.
                                if (displayWindow > lineSerieDatas.GetLastIndex() / interval)
                                    displayWindow = lineSerieDatas.GetLastIndex() / interval;

                                // Différencie le premier jour du reste.
                                if ((interval * displayWindow + interval) < lineSerieDatas.GetLastIndex())
                                {
                                    int lastItemIndex = interval * displayWindow + interval;
                                    lineSerieDatas.RemoveRange(lastItemIndex, lineSerieDatas.GetLastIndex() - lastItemIndex + 1);
                                    lineSerieDatas.RemoveRange(0, interval * displayWindow);
                                }
                                else
                                {
                                    lineSerieDatas.RemoveRange(0, lineSerieDatas.GetLastIndex() - interval);
                                }
                            }
                            double serieMaxValue = lineSerieDatas.Max();
                            if (serieMaxValue > axisYMaxValue)
                            {
                                axisYMaxValue = serieMaxValue;
                            }
                            serie.Values = lineSerieDatas.AsGearedValues().WithQuality(Quality.Low);
                        }
                        SetMaxAxisValue(axisY, axisYMaxValue);

                        // ID Documentation : Curve_Chart_AxeX
                        if (maxValue - interval > 0 && interval != 0)
                        {
                            axisX.MinValue = maxValue - interval;
                            axisX.MaxValue = maxValue;
                        }
                        else if (interval == 0)
                        {
                            axisX.MinValue = 0;
                            axisX.MaxValue = maxValue;
                        }
                        else
                        {
                            axisX.MinValue = 0;
                            axisX.MaxValue = interval;
                        }

                        datas.DisplayWindow = displayWindow;
                        chart.Tag = datas;
                        break;
                    case UIType.Vertical:
                        //SetMaxAxisValue(axisY);
                        DisplayColumnRowChart(chart, e, axisX, axisY, isDisplayChange);
                        break;
                    case UIType.Horizontal:
                        //SetMaxAxisValue(axisX);
                        DisplayColumnRowChart(chart, e, axisY, axisX, isDisplayChange);
                        break;
                    case UIType.HeatMap:
                        HeatSeries serieHeatMap = (HeatSeries)chart.Series[0];
                        List<double> simulationData = new List<double>(e.GetDataFromEnum((ChartsDisplayData)serieHeatMap.Tag));

                        if (simulationData != null)
                        {
                            // Trouver premier jour semaine
                            int x = 0; 
                            int y = 0;
                            int week = 0;
                            double value = 0;
                            if (simulationData.Count > 0)
                            {
                                // Parcours les données reçues
                                for (int i = 1; i <= simulationData.Count; i++)
                                {
                                    // Ajotue les données à value
                                    value += simulationData[i - 1];
                                    // Si il y a de nouvelles valeurs
                                    if (serieHeatMap.Values.Count < (i/4))
                                    {
                                        // Si value a pu récupérer 4 valeurs
                                        if (i % 4 == 0)
                                        {
                                            // Si c'est la première semaine, créé des heatpoint
                                            if (week == 0)
                                            {
                                                double avgValue = value / 4;
                                                serieHeatMap.Values.Add(new HeatPoint(x, y, avgValue));
                                            }
                                            else // Sinon, les ajoutent au heatpoint déjà existant et en fait la moyenne
                                            {
                                                HeatPoint hm = ((HeatPoint)serieHeatMap.Values[(x * 12) + y]);
                                                hm.Weight = ((week * 4) * hm.Weight + value) / ((week * 4) + 4);
                                            }
                                        }
                                    }
                                    // Lors d'un changement de semaine.
                                    if (i % 336 == 0)
                                    {
                                        week++;
                                        x = 0;
                                        y = 0;
                                        value = 0;
                                    }
                                    else if (i % 48 == 0) // Lors d'un changement de jour.
                                    {
                                        x++;
                                        y = 0;
                                        value = 0;
                                    }
                                    else if (i % 4 == 0) // Lors d'un changement de case.
                                    {
                                        y++;
                                        value = 0;
                                    }
                                }
                            }
                        }
                        axisX.MaxValue = 7;
                        axisY.MaxValue = 12;
                        break;
                }
            }));
        }

        private static void DisplayColumnRowChart(CartesianChart chart, SimulationDatas e, Axis axis, Axis axisValueSize, bool isDisplayChange)
        {
            int interval;
            ChartValues<double> cv;
            ChartData datas = (ChartData)chart.Tag;
            double maxValue = chart.Series[0].Values.Count;
            switch ((ChartsDisplayInterval)datas.DisplayInterval)
            {
                default:
                case ChartsDisplayInterval.Day:
                    interval = 12;
                    DisplayColumnRow(chart, e, 4, isDisplayChange, 48);
                    maxValue = 12;
                    break;
                case ChartsDisplayInterval.Week:
                    interval = 7;
                    DisplayColumnRow(chart, e, 48, isDisplayChange, 336);
                    maxValue = interval;
                    break;
                case ChartsDisplayInterval.Month:
                    interval = 4;
                    DisplayColumnRow(chart, e, 336, isDisplayChange, 1440);
                    maxValue = interval;
                    break;
                case ChartsDisplayInterval.Total:
                    foreach (Series serie in chart.Series)
                    {
                        cv = new ChartValues<double>();
                        List<double> day = new List<double>();
                        List<double> avg = new List<double>();
                        List<double> columnDatas = e.GetDataFromEnum((ChartsDisplayData)serie.Tag);
                        avg.Add(columnDatas.Average());
                        cv.AddRange(avg);
                        serie.Values = cv;
                    }
                    interval = 1;
                    break;
            }

            double axisMaxValue = 0;
            foreach (Series serie in chart.Series)
            {
                double serieMaxValue = e.GetDataFromEnum((ChartsDisplayData)serie.Tag).Max();
                if (serieMaxValue > axisMaxValue)
                {
                    axisMaxValue = serieMaxValue;
                }
            }

            SetMaxAxisValue(axisValueSize, axisMaxValue);

            if (maxValue - interval > 0 && interval != 0)
            {
                axis.MinValue = maxValue - interval;
                axis.MaxValue = maxValue;
            }
            else
            {
                axis.MinValue = 0;
                axis.MaxValue = interval;
            }
        }

        /// <summary>
        /// ID Documentation : ColumnRow_Chart
        /// </summary>
        /// <param name="chart">Graphique à modifier.</param>
        /// <param name="e">Valeurs de la simulation.</param>
        /// <param name="modulo">Nombre par lequel les données seront regroupée. (Ex: Modulo = 4 pour 12 données alors il y aura 3 colonne/ligne d'ajoutées.)</param>
        /// <param name="isDisplayChange">Si l'affichage change et qu'il est nécessaire de recharger les données du graphiques.</param>
        /// <param name="interval">Interval à afficher.</param>
        private static void DisplayColumnRow(CartesianChart chart, SimulationDatas e, int modulo, bool isDisplayChange, int interval)
        {
            ChartValues<double> cv;
            ChartData datas = (ChartData)chart.Tag;
            foreach (Series serie in chart.Series)
            {
                cv = new ChartValues<double>();
                List<double> columnDatas = new List<double>();
                List<double> columnAvg = new List<double>();
                List<double> colRowSerieDatas = new List<double>(e.GetDataFromEnum((ChartsDisplayData)serie.Tag));

                if (colRowSerieDatas.GetLastIndex() > interval && datas.AutoDisplay)
                {
                    colRowSerieDatas.RemoveRange(0, colRowSerieDatas.GetLastIndex() - interval);
                }
                else if ((interval * datas.DisplayWindow + interval) < colRowSerieDatas.GetLastIndex())
                {
                    int lastItemIndex = interval * datas.DisplayWindow + interval;
                    colRowSerieDatas.RemoveRange(lastItemIndex, colRowSerieDatas.GetLastIndex() - lastItemIndex + 1);
                    colRowSerieDatas.RemoveRange(0, interval * datas.DisplayWindow);
                }

                // parcours les données et créé des moyennes pour correspondre au nombre de ligne/colonne de l'interval.
                //     Par exemple: 48 données pour 1 jour = 12 colonnes/lignes, 48 données pour 1 semaine = 1 colonne/ligne.
                for (int i = 0; i < colRowSerieDatas.Count; i++)
                {
                    columnDatas.Add(colRowSerieDatas[i]);
                    if (i % modulo == 0)
                    {
                        columnAvg.Add(columnDatas.Average());
                        columnDatas.Clear();
                    }
                }
                serie.Values = columnAvg.AsChartValues();
            }
        }

        #endregion
    }
}

/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 06.05.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Geared;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
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
            double result;
            if (double.IsNaN(value))
                result = 0;
            else
                result = value;

            return result;
        }

        #region Charts

        /// <summary>
        /// Lorsque les données d'un graphique cylindrique sont mise à jours.
        /// Récupère la dernière données de chaque section de la simulation et l'affiche.
        /// </summary>
        public static void OnDataUpdatePieChart(this PieChart chart, SimulationDatas e)
        {
            ChartValues<double> cv;
            foreach (PieSeries serie in chart.Series)
            {
                cv = new ChartValues<double>();
                cv.Add(e.GetDataFromEnum((ChartsDisplayData)serie.Tag).Last());
                serie.Values = cv;
            }
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
                double[] maxValueAndIntervalOfColRowChart;
                int interval = 48;
                Axis axisX = chart.AxisX[0];
                Axis axisY = chart.AxisY[0];
                Axis currentAxis = axisX;
                double maxValue = chart.Series[0].Values.Count;
                ChartData datas = (ChartData)chart.Tag;
                int displayWindow = datas.DisplayWindow;

                switch ((ChartsType)datas.ChartType)
                {
                    case ChartsType.Linear:
                        axisX.MaxValue = maxValue;
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

                            serie.Values = lineSerieDatas.AsGearedValues().WithQuality(Quality.Low);
                        }
                        datas.DisplayWindow = displayWindow;
                        chart.Tag = datas;
                        break;
                    case ChartsType.Vertical:
                        // Affichage différent
                        maxValueAndIntervalOfColRowChart = DisplayColumnRowChart(chart, e, isDisplayChange);
                        maxValue = maxValueAndIntervalOfColRowChart[0];
                        interval = Convert.ToInt32(maxValueAndIntervalOfColRowChart[1]);
                        break;
                    case ChartsType.Horizontal:
                        // Affichage différent
                        currentAxis = axisY;
                        maxValueAndIntervalOfColRowChart = DisplayColumnRowChart(chart, e, isDisplayChange);
                        maxValue = maxValueAndIntervalOfColRowChart[0];
                        interval = Convert.ToInt32(maxValueAndIntervalOfColRowChart[1]);
                        break;
                    case ChartsType.HeatMap:
                        HeatSeries serieHM = (HeatSeries)chart.Series[0];
                        List<double> heatSeriesData = new List<double>(e.GetDataFromEnum((ChartsDisplayData)serieHM.Tag));
                        interval = 7;
                        int nbTimeFramesPerDay = GlobalVariables.NUMBER_OF_TIMEFRAME;
                        int nbOfDatasUnified = 4;
                        int dayDivision = 4;

                        if (heatSeriesData != null)
                        {
                            // Trouver premier jour semaine
                            maxValue = Math.Ceiling((double)maxValue / interval) * interval;
                            for (int i = 1; i < heatSeriesData.Count; i += nbTimeFramesPerDay)
                            {
                                for (int j = 0; j < nbTimeFramesPerDay; j += nbOfDatasUnified)
                                {
                                    if (heatSeriesData.Count > i + j + nbOfDatasUnified && serieHM.Values.Count < (i + j + nbOfDatasUnified) / dayDivision)
                                    {
                                        double avgData = 0;
                                        for (int k = 0; k < nbOfDatasUnified; k++)
                                        {
                                            avgData += heatSeriesData[i + j + k];
                                        }
                                        serieHM.Values.Add(new HeatPoint(i / nbTimeFramesPerDay, j / dayDivision, avgData / nbOfDatasUnified));
                                    }
                                }
                            }
                        }

                        maxValue = Math.Ceiling((double)serieHM.Values.Count / 48 / (double)interval) * interval;
                        axisY.MaxValue = 12;
                        break;
                }
                /*
                if (maxValue - interval > 0 && interval != 0)
                {
                    currentAxis.MinValue = maxValue - interval;
                    currentAxis.MaxValue = maxValue;
                }
                else if((ChartsType)datas.ChartType == ChartsType.Linear && interval == 0)
                {
                    axisX.MinValue = 0;
                    axisX.MaxValue = maxValue;
                }
                else
                {
                    currentAxis.MinValue = 0;
                    currentAxis.MaxValue = interval;
                }*/

            }));
        }

        private static double[] DisplayColumnRowChart(CartesianChart chart, SimulationDatas e, bool isDisplayChange)
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
                    DisplayColumnRow(chart, e, 4, isDisplayChange);
                    maxValue = Math.Ceiling((double)maxValue / interval) * interval;
                    break;
                case ChartsDisplayInterval.Week:
                    interval = 7;
                    DisplayColumnRow(chart, e, 48, isDisplayChange);
                    maxValue = Math.Ceiling((double)maxValue / interval) * interval;
                    break;
                case ChartsDisplayInterval.Month:
                    interval = 4;
                    DisplayColumnRow(chart, e, 336, isDisplayChange);
                    maxValue = Math.Ceiling((double)maxValue / interval) * interval;
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
            return new double[] { maxValue, interval };
        }

        private static void DisplayColumnRow(CartesianChart chart, SimulationDatas e, int modulo, bool isDisplayChange)
        {
            ChartValues<double> cv;
            foreach (Series serie in chart.Series)
            {
                cv = new ChartValues<double>();
                List<double> columnDatas = new List<double>();
                List<double> columnAvg = new List<double>();
                List<double> simDatas = e.GetDataFromEnum((ChartsDisplayData)serie.Tag);
                if (serie.Values.Count * modulo < simDatas.Count || isDisplayChange)
                {
                    for (int i = serie.Values.Count * modulo; i < simDatas.Count; i++)
                    {
                        columnDatas.Add(simDatas[i]);
                        if (i % modulo == 0)
                        {
                            columnAvg.Add(columnDatas.Average());
                            columnDatas.Clear();
                        }
                    }

                    if (isDisplayChange)
                    {
                        cv.AddRange(columnAvg);
                        serie.Values = cv.AsGearedValues();
                    }
                    else
                    {
                        for (int i = 0; i < columnAvg.Count; i++)
                        {
                            serie.Values.Add(columnAvg[i]);
                        }
                    }
                }
            }
        }

        #endregion
    }
}

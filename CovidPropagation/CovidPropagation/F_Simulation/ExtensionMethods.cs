/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 06.05.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using LiveCharts;
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
            return value.Count() - 1;
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

        public static void OnDataUpdate(this CartesianChart chart, SimulationDatas e)
        {
            chart.AddNewValueToChart((ChartData)chart.Tag, e);
        }

        private static void AddNewValueToChart(this CartesianChart chart, ChartData datas, SimulationDatas e)
        {
            ChartValues<double> cv;

            switch ((ChartsType)datas.ChartType)
            {
                case ChartsType.Linear:
                    foreach (LineSeries serie in chart.Series)
                    {
                        serie.Values = e.GetDataFromEnum((ChartsDisplayData)serie.Tag).AsGearedValues().WithQuality(Quality.Low);
                    }
                    break;
                case ChartsType.Vertical:
                    switch ((ChartsDisplayInterval)datas.DisplayInterval)
                    {
                        case ChartsDisplayInterval.Day:
                            foreach (ColumnSeries serie in chart.Series)
                            {
                                cv = new ChartValues<double>();
                                cv.AddRange(e.GetDataFromEnum((ChartsDisplayData)serie.Tag));
                                serie.Values = cv;
                            }
                            break;
                        case ChartsDisplayInterval.Week:
                            foreach (ColumnSeries serie in chart.Series)
                            {
                                cv = new ChartValues<double>();
                                List<double> day = new List<double>();
                                List<double> daysAvg = new List<double>();
                                List<double> columnDatas = e.GetDataFromEnum((ChartsDisplayData)serie.Tag);
                                for (int i = 0; i < columnDatas.Count; i++)
                                {
                                    day.Add(columnDatas[i]);
                                    if (i % 48 == 0)
                                    {
                                        daysAvg.Add(day.Average());
                                        day.Clear();
                                    }
                                }
                                cv.AddRange(daysAvg);
                                serie.Values = cv;
                            }
                            break;
                        case ChartsDisplayInterval.Month:
                            foreach (ColumnSeries serie in chart.Series)
                            {
                                cv = new ChartValues<double>();
                                List<double> day = new List<double>();
                                List<double> daysAvg = new List<double>();
                                List<double> columnDatas = e.GetDataFromEnum((ChartsDisplayData)serie.Tag);
                                for (int i = 0; i < columnDatas.Count; i++)
                                {
                                    day.Add(columnDatas[i]);
                                    if (i % 336 == 0)
                                    {
                                        daysAvg.Add(day.Average());
                                        day.Clear();
                                    }
                                }
                                cv.AddRange(daysAvg);
                                serie.Values = cv;
                            }
                            break;
                        case ChartsDisplayInterval.Total:
                            break;
                        default:
                            break;
                    }
                    break;
                case ChartsType.Horizontal:
                    foreach (RowSeries serie in chart.Series)
                    {
                        cv = new ChartValues<double>();
                        cv.AddRange(e.GetDataFromEnum((ChartsDisplayData)serie.Tag));
                        serie.Values = cv;
                    }
                    break;
                case ChartsType.HeatMap:
                    foreach (ColumnSeries serie in chart.Series)
                    {
                        cv = new ChartValues<double>();
                        List<double> day = new List<double>();
                        List<double> avg = new List<double>();
                        List<double> columnDatas = e.GetDataFromEnum((ChartsDisplayData)serie.Tag);
                        avg.Add(columnDatas.Average());
                        cv.AddRange(avg);
                        serie.Values = cv;
                    }
                    break;
                default:
                    break;
            }
        }

        public static void Display(this CartesianChart chart, SimulationDatas e, bool isDisplayChange)
        {
            ChartValues<double> cv;
            int interval;
            Axis axisX = chart.AxisX[0];
            double maxValue = chart.Series[0].Values.Count;
            ChartData datas = (ChartData)chart.Tag;

            if (datas.AutoDisplay || isDisplayChange)
            {
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

                        if (maxValue - interval > 0 && interval != 0)
                        {
                            axisX.MinValue = maxValue - interval;
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
                        break;
                    case ChartsType.Vertical:
                        // Affichage différent
                        axisX.MaxValue = maxValue;
                        switch ((ChartsDisplayInterval)datas.DisplayInterval)
                        {
                            default:
                            case ChartsDisplayInterval.Day:
                                Debug.WriteLine("Day");
                                foreach (ColumnSeries serie in chart.Series)
                                {
                                    cv = new ChartValues<double>();
                                    cv.AddRange(e.GetDataFromEnum((ChartsDisplayData)serie.Tag));
                                    serie.Values = cv;
                                }
                                interval = 48;
                                if (maxValue - interval > 0 && interval != 0)
                                {
                                    axisX.MinValue = maxValue - interval;
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
                                break;
                            case ChartsDisplayInterval.Week:
                                interval = 7;
                                Debug.WriteLine("Week");
                                foreach (ColumnSeries serie in chart.Series)
                                {
                                    cv = new ChartValues<double>();
                                    List<double> day = new List<double>();
                                    List<double> daysAvg = new List<double>();
                                    List<double> columnDatas = e.GetDataFromEnum((ChartsDisplayData)serie.Tag);
                                    for (int i = 0; i < columnDatas.Count; i++)
                                    {
                                        day.Add(columnDatas[i]);
                                        if (i % 48 == 0)
                                        {
                                            daysAvg.Add(day.Average());
                                            day.Clear();
                                        }
                                    }
                                    cv.AddRange(daysAvg);
                                    serie.Values = cv;
                                }
                                maxValue = chart.Series[0].Values.Count;
                                Debug.WriteLine(maxValue);

                                if (maxValue - interval > 0 && interval != 0)
                                {
                                    axisX.MinValue = maxValue - interval;
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
                                break;
                            case ChartsDisplayInterval.Month:
                                interval = 4;
                                foreach (ColumnSeries serie in chart.Series)
                                {
                                    cv = new ChartValues<double>();
                                    List<double> day = new List<double>();
                                    List<double> daysAvg = new List<double>();
                                    List<double> columnDatas = e.GetDataFromEnum((ChartsDisplayData)serie.Tag);
                                    for (int i = 0; i < columnDatas.Count; i++)
                                    {
                                        day.Add(columnDatas[i]);
                                        if (i % 336 == 0)
                                        {
                                            daysAvg.Add(day.Average());
                                            day.Clear();
                                        }
                                    }
                                    cv.AddRange(daysAvg);
                                    serie.Values = cv;
                                }
                                maxValue = chart.Series[0].Values.Count;

                                if (maxValue - interval > 0 && interval != 0)
                                {
                                    axisX.MinValue = maxValue - interval;
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
                                break;
                            case ChartsDisplayInterval.Total:
                                foreach (ColumnSeries serie in chart.Series)
                                {
                                    cv = new ChartValues<double>();
                                    List<double> day = new List<double>();
                                    List<double> avg = new List<double>();
                                    List<double> columnDatas = e.GetDataFromEnum((ChartsDisplayData)serie.Tag);
                                    avg.Add(columnDatas.Average());
                                    cv.AddRange(avg);
                                    serie.Values = cv;
                                }
                                break;
                        }
                        break;
                    case ChartsType.Horizontal:

                        break;
                    case ChartsType.HeatMap:
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}

/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 29.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CovidPropagation
{
    /// <summary>
    /// Jour composé de plusieurs périodes.
    /// </summary>
    public class Day
    {
        #region SiteIds
        private const char HOME_ID = 'H';
        private const char TRANSPORT_ID = 'T';
        private const char COMPANY_ID = 'C';
        private const char RESTAURANT_ID = 'R';
        private const char SUPERMARKET_ID = 'S';
        private const char STORE_ID = 'M'; // MAGASIN
        private const char SCHOOL_ID = 'E'; // ECOLE
        private const char OUTSIDE_ID = 'O';

        #endregion

        #region WorkingClass
        private static string[] adultWeekDaysSeeds = new string[] {
            "H10 T1 C12 T1 R2 T1 C10 T1 H10",
            "H10 T1 C12 T1 R2 T1 C10 T1 S1 T1 H8",
            "H10 T1 C12 T1 R2 T1 C10 T1 S2 T1 H7",
            "H10 T1 C12 T1 R2 T1 C10 T1 S3 T1 H6",

            "H10 T1 C12 T1 R2 T1 C10 T1 R2 T1 H7",
            "H10 T1 C12 T1 R2 T1 C10 T1 R3 T1 H6",
            "H10 T1 C12 T1 R2 T1 C10 T1 R4 T1 H5",
            "H10 T1 C12 T1 R2 T1 C10 T1 R5 T1 H4",
            "H10 T1 C12 T1 R2 T1 C10 T1 R6 T1 H3",

            "H10 T1 C12 T1 R2 T1 C10 T1 O1 T1 H8",
            "H10 T1 C12 T1 R2 T1 C10 T1 O5 T1 H4",
            "H10 T1 C12 T1 R2 T1 C10 T1 O8 T1 H1",
            "H10 T1 C12 T1 R2 T1 C10 T1 H10"
        };

        private static string[] adultWeekEndDaysSeeds = new string[] {
            "H22 T1 R2 T1 H10 T1 S1 T1 H9",
            "H26 H10 T1 S1 T1 H9",

            "H22 T1 R2 T1 H10 T1 R4 T1 H6",
            "H36 T1 R4 T1 H6",

            "H22 T1 R2 T1 H5 O5 T1 S1 T1 H9",
            "H26 H10 T1 S1 T1 H9",

            "H22 T1 R2 T1 H10 T1 R4 T1 H6",
            "H30 O6 T1 R4 T1 H6"
        };
        #endregion

        #region Students
        private static string[] studentsWeekDaysSeeds = new string[] {
            "H10 T1 E12 T1 R2 T1 E10 T1 H10",
            "H10 T1 E12 T1 R2 T1 E10 T1 S1 T1 H8",
            "H10 T1 E12 T1 R2 T1 E10 T1 S2 T1 H7",
            "H10 T1 E12 T1 R2 T1 E10 T1 S3 T1 H6",
        };

        private static string[] studentWeekEndDaysSeeds = new string[] {
            "H22 T1 R2 T1 H10 H12",
            "H26 O11 H11",
            "H30 T1 R6 T1 H10",
            "H26 O22",
            "H48"
        };
        #endregion

        #region Elders
        private static string[] eldersDaysSeeds = new string[] {
            "H10 T1 S3 T1 R2 T1 O5 H5 T1 H19",
            "H10 T1 S3 T1 R2 T1 M4 T1 H5 T1 H19",
            "H22 T1 R2 T1 H10 H12",
            "H26 O11 H11",
            "H30 T1 R6 T1 H10",
            "H26 O22",
            "H48"
        };

        #endregion

        private const int NB_WORKING_DAYS = 5;

        private Period[] _periods;
        public Period[] Periods { get => _periods; }

        public Day()
        {
            _periods = new Period[GlobalVariables.NUMBER_OF_PERIODS];
        }

        /// <summary>
        /// Récupère l'activité à l'index spécifié.
        /// </summary>
        /// <param name="period">Index de la période à récupérer</param>
        /// <returns>Activité dans la période.</returns>
        public Site GetActivity(int period)
        {
            return _periods[period].Activity;
        }

        /// <summary>
        /// Récupère l'activité actuelle
        /// </summary>
        /// <returns>L'activité en cours.</returns>
        public Site GetCurrentActivity()
        {
            return _periods[TimeManager.CurrentPeriod].Activity;
        }

        public void CreateStudentDay(int dayOfWeek)
        {
            Random rdm = GlobalVariables.rdm;
            if (dayOfWeek > NB_WORKING_DAYS - 1)
            {
                int seedIndex = rdm.Next(studentsWeekDaysSeeds.Length - 1);
                CreateDay(studentsWeekDaysSeeds[seedIndex]);
            }
            else
            {
                int seedIndex = rdm.Next(studentWeekEndDaysSeeds.Length - 1);
                CreateDay(studentWeekEndDaysSeeds[seedIndex]);
            }
        }

        public void CreateAdultDay(int dayOfWeek)
        {
            Random rdm = GlobalVariables.rdm;
            if (dayOfWeek > NB_WORKING_DAYS-1)
            {
                int seedIndex = rdm.Next(adultWeekDaysSeeds.Length - 1);
                CreateDay(adultWeekDaysSeeds[seedIndex]);
            }
            else
            {
                int seedIndex = rdm.Next(adultWeekEndDaysSeeds.Length - 1);
                CreateDay(adultWeekEndDaysSeeds[seedIndex]);
            }
        }

        public void CreateElderDay(int dayOfWeek, Home home, Site hobby, Site transport)
        {
            Random rdm = GlobalVariables.rdm;
            int seedIndex = rdm.Next(eldersDaysSeeds.Length - 1);
            CreateDay(eldersDaysSeeds[seedIndex], home, hobby, transport);
        }

        private void CreateDay(string seed, Home home, Site hobby, Site transport)
        {
            string[] seedSplitted = seed.Split(" ");
            List<Period> periods = new List<Period>();
            foreach (string item in seedSplitted)
            {
                char siteType = item.Substring(0, 1).ToChar();
                int count = Convert.ToInt32(item.Substring(1));
                for (int i = 0; i < count; i++)
                {
                    Period period;
                    switch (siteType)
                    {
                        case TRANSPORT_ID:
                            period = new Period(transport);
                            periods.Add(period);
                            break;
                        case COMPANY_ID:
                            period = new Period(new Company(5));
                            periods.Add(period);
                            break;
                        case RESTAURANT_ID:
                            period = new Period(new Restaurant(5));
                            periods.Add(period);
                            break;
                        case SUPERMARKET_ID:
                            period = new Period(new Supermarket(5));
                            periods.Add(period);
                            break;
                        case OUTSIDE_ID:
                            period = new Period(new Outside(5));
                            periods.Add(period);
                            break;
                        case SCHOOL_ID:
                            period = new Period(new School(5));
                            periods.Add(period);
                            break;
                        case STORE_ID:
                            period = new Period(new Store(5));
                            periods.Add(period);
                            break;
                        case HOME_ID:
                        default:
                            period = new Period(new Home(5));
                            periods.Add(period);
                            break;
                    }
                }
            }
            _periods = periods.ToArray();
        }
    }
}

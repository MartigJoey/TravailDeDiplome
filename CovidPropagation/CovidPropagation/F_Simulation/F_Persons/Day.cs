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

        private TimeFrame[] _timeFrames;
        public TimeFrame[] TimeFrames { get => _timeFrames; }

        public Day()
        {
            _timeFrames = new TimeFrame[GlobalVariables.NUMBER_OF_TIMEFRAME];
        }

        /// <summary>
        /// Récupère l'activité à l'index spécifié.
        /// </summary>
        /// <param name="timeFrame">Index de la période à récupérer</param>
        /// <returns>Activité dans la période.</returns>
        public Site GetActivity(int timeFrame)
        {
            return _timeFrames[timeFrame].Activity;
        }

        /// <summary>
        /// Récupère l'activité actuelle
        /// </summary>
        /// <returns>L'activité en cours.</returns>
        public Site GetCurrentActivity()
        {
            return _timeFrames[TimeManager.CurrentTimeFrame].Activity;
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

        public void CreateElderDay(int dayOfWeek)
        {
            Random rdm = GlobalVariables.rdm;
            int seedIndex = rdm.Next(eldersDaysSeeds.Length - 1);
            CreateDay(eldersDaysSeeds[seedIndex]);
        }

        private void CreateDay(string seed)
        {
            string[] seedSplitted = seed.Split(" ");
            List<TimeFrame> timeFrames = new List<TimeFrame>();
            foreach (string item in seedSplitted)
            {
                char siteType = item.Substring(0, 1).ToChar();
                int count = Convert.ToInt32(item.Substring(1));
                for (int i = 0; i < count; i++)
                {
                    TimeFrame timeFrame;
                    switch (siteType)
                    {
                        case GlobalVariables.TRANSPORT_ID:
                            timeFrame = new TimeFrame(new Car(), SitePersonStatus.Other);
                            timeFrames.Add(timeFrame);
                            break;
                        case GlobalVariables.COMPANY_ID:
                            timeFrame = new TimeFrame(new Company(), SitePersonStatus.Other);
                            timeFrames.Add(timeFrame);
                            break;
                        case GlobalVariables.RESTAURANT_ID:
                            timeFrame = new TimeFrame(new Restaurant(), SitePersonStatus.Other);
                            timeFrames.Add(timeFrame);
                            break;
                        case GlobalVariables.SUPERMARKET_ID:
                            timeFrame = new TimeFrame(new Supermarket(), SitePersonStatus.Other);
                            timeFrames.Add(timeFrame);
                            break;
                        case GlobalVariables.OUTSIDE_ID:
                            timeFrame = new TimeFrame(new Outside(), SitePersonStatus.Other);
                            timeFrames.Add(timeFrame);
                            break;
                        case GlobalVariables.SCHOOL_ID:
                            timeFrame = new TimeFrame(new School(), SitePersonStatus.Other);
                            timeFrames.Add(timeFrame);
                            break;
                        case GlobalVariables.STORE_ID:
                            timeFrame = new TimeFrame(new Store(), SitePersonStatus.Other);
                            timeFrames.Add(timeFrame);
                            break;
                        case GlobalVariables.HOME_ID:
                        default:
                            timeFrame = new TimeFrame(new Home(), SitePersonStatus.Other);
                            timeFrames.Add(timeFrame);
                            break;
                    }
                }
            }
            _timeFrames = timeFrames.ToArray();
        }
    }
}

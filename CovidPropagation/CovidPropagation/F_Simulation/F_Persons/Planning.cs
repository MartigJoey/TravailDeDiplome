/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 06.05.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CovidPropagation
{
    /// <summary>
    /// Gère le planning des individus. Composé de Jours, qui sont composés de périodes, qui sont composé d'activités.
    /// </summary>
    public class Planning
    {
        private const int SUNDAY_INDEX = 6;
        private const int SATURDAY_INDEX = 5;
        private const int FRIDAY_INDEX = 4;

        private const double WORK_ON_SUNDAY_PROBABILITY = 0.1;
        private const double WORK_ON_SATURDAY_PROBABILITY = 0.3;
        private Day[] _days = new Day[GlobalVariables.NUMBER_OF_DAY];
        public Day[] Days { get => _days; }

        public Planning(List<KeyValuePair<Site, SitePersonStatus>> personSites, int nbWorkDays)
        {
            bool[] workDays = new bool[GlobalVariables.NUMBER_OF_DAY];
            Random rdm = GlobalVariables.rdm;
            // Calcul les jours de la semaine ou l'individu travail
            if (nbWorkDays > 0 && rdm.NextBoolean(WORK_ON_SUNDAY_PROBABILITY))
            {
                workDays[SUNDAY_INDEX] = true;
                nbWorkDays--;
            }
            if (nbWorkDays > 0 && rdm.NextBoolean(WORK_ON_SATURDAY_PROBABILITY))
            {
                workDays[SATURDAY_INDEX] = true;
                nbWorkDays--;
            }
            while (nbWorkDays > 0)
            {
                workDays[rdm.NextInclusive(0, FRIDAY_INDEX)] = true;
                nbWorkDays--;
            }

            for (int i = 0; i < _days.Length; i++)
            {
                if (workDays[i])
                    _days[i] = new Day(personSites, true);
                else
                    _days[i] = new Day(personSites, false);
            }
        }

        /// <summary>
        /// Récupère une activité au jour et à la période demandée.
        /// </summary>
        /// <param name="dayOfWeek">int du jour de la semaine</param>
        /// <param name="timeFrameOfDay">int de la période de la journée</param>
        /// <returns>L'activité au jour et à la période demandé</returns>
        public Site GetActivity(int dayOfWeek, int timeFrameOfDay)
        {
            return _days[dayOfWeek].GetActivity(timeFrameOfDay);
        }

        /// <summary>
        /// Récupère une activité au jour et à la période actuelle.
        /// </summary>
        /// <returns>L'activité actuelle</returns>
        public Site GetActivity()
        {
            return _days[TimeManager.CurrentDay].GetCurrentActivity();
        }

        public SitePersonStatus PersonTypeInActivity()
        {
            return _days[TimeManager.CurrentDay].GetCurrentPersonTypeInActivity();
        }

        /// <summary>
        /// Récupère la prochaine activité.
        /// </summary>
        /// <returns>L'activité qui sera réalisé après un tick</returns>
        public Site GetNextActivity()
        {
            int[] nextTimeFrame = TimeManager.GetNextTimeFrame();
            return _days[nextTimeFrame[0]].GetActivity(nextTimeFrame[1]);
        }
    }
}

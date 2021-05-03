/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 29.04.2021
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
        private Day[] _days = new Day[GlobalVariables.NUMBER_OF_DAY];
        public Day[] Days { get => _days; }

        private Site _dayActivity;

        public Planning()
        {
            // Does nothing
        }

        /// <summary>
        /// Récupère une activité au jour et à la période demandée.
        /// </summary>
        /// <param name="dayOfWeek">int du jour de la semaine</param>
        /// <param name="periodOfDay">int de la période de la journée</param>
        /// <returns>L'activité au jour et à la période demandé</returns>
        public Type GetActivity(int dayOfWeek, int periodOfDay)
        {
            return _days[dayOfWeek].GetActivity(periodOfDay);
        }

        /// <summary>
        /// Récupère une activité au jour et à la période actuelle.
        /// </summary>
        /// <returns>L'activité actuelle</returns>
        public Type GetActivity()
        {
            return _days[TimeManager.CurrentDay].GetCurrentActivity();
        }

        /// <summary>
        /// Récupère la prochaine activité.
        /// </summary>
        /// <returns>L'activité qui sera réalisé après un tick</returns>
        public Type GetNextActivity()
        {
            int[] nextPeiod = TimeManager.GetNextPeriod();
            return _days[nextPeiod[0]].GetActivity(nextPeiod[1]);
        }

        public void CreateAdultPlanning()
        {
            List<Day> days = new List<Day>();
            for (int i = 0; i < GlobalVariables.NUMBER_OF_DAY; i++)
            {
                Day day = new Day();
                day.CreateAdultDay(i);
                days.Add(day);
            }
            _days = days.ToArray();
        }

        public void CreateStudentPlanning()
        {
            List<Day> days = new List<Day>();
            for (int i = 0; i < GlobalVariables.NUMBER_OF_DAY; i++)
            {
                Day day = new Day();
                day.CreateStudentDay(i);
                days.Add(day);
            }
            _days = days.ToArray();
        }

        public void CreateElderPlanning()
        {
            List<Day> days = new List<Day>();
            for (int i = 0; i < GlobalVariables.NUMBER_OF_DAY; i++)
            {
                Day day = new Day();
                day.CreateElderDay(i);
                days.Add(day);
            }
            _days = days.ToArray();
        }
    }
}

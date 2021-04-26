using CovidPropagation.Simulation;
using System;
using System.Drawing;

namespace CovidPropagation.Classes.Person
{
    /// <summary>
    /// Gère le planning des individus. Composé de Jours, qui sont composés de périodes, qui sont composé d'activités.
    /// </summary>
    class Planning
    {
        private Day[] _days = new Day[GlobalVariables.NUMBER_OF_DAY];
        public Day[] Days { get => _days; }

        private int _personAge;
        private Site _dayActivity;

        public Planning(Day[] days, int age)
        {
            _days = days;
            _personAge = age;
        }

        /// <summary>
        /// Récupère une activité au jour et à la période demandée.
        /// </summary>
        /// <param name="dayOfWeek">int du jour de la semaine</param>
        /// <param name="periodOfDay">int de la période de la journée</param>
        /// <returns>L'activité au jour et à la période demandé</returns>
        public Site GetActivity(int dayOfWeek, int periodOfDay)
        {
            return _days[dayOfWeek].GetActivity(periodOfDay);
        }

        /// <summary>
        /// Récupère une activité au jour et à la période actuelle.
        /// </summary>
        /// <returns>L'activité actuelle</returns>
        public Site GetActivity()
        {
            return _days[TimeManager.CurrentDay].GetCurrentActivity();
        }

        /// <summary>
        /// Récupère la prochaine activité.
        /// </summary>
        /// <returns>L'activité qui sera réalisé après un tick</returns>
        public Site GetNextActivity()
        {
            int[] nextPeiod = TimeManager.GetNextPeriod();
            return _days[nextPeiod[0]].GetActivity(nextPeiod[1]);
        }





        /// <summary>
        /// Génère le planning en fonction des paramètres de la personne.
        /// </summary>
        public void Generate()
        {
        }
    }
}

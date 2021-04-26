using CovidPropagation.Simulation;

namespace CovidPropagation.Classes.Person
{
    /// <summary>
    /// Jour composé de plusieurs périodes.
    /// </summary>
    class Day
    {
        private Period[] _periods = new Period[GlobalVariables.NUMBER_OF_PERIODS];

        public Period[] Periods { get => _periods; }

        public Day(Period[] periods)
        {
            _periods = periods;
        }

        /// <summary>
        /// Récupère l'activité à l'index spécifié.
        /// </summary>
        /// <param name="period">Index de la période à récupérer</param>
        /// <returns>Activité dans la période.</returns>
        public Site GetActivity(int period)
        {
            return Periods[period].Activity;
        }

        /// <summary>
        /// Récupère l'activité actuelle
        /// </summary>
        /// <returns>L'activité en cours.</returns>
        public Site GetCurrentActivity()
        {
            return Periods[TimeManager.CurrentPeriod].Activity;
        }
    }
}

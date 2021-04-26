using CovidPropagation.Simulation;

namespace CovidPropagation.Classes.Person
{
    /// <summary>
    /// Période composé d'une activité.
    /// </summary>
    class Period
    {
        private Site _activity;

        public Site Activity { get => _activity; }

        public Period(Site activity)
        {
            _activity = activity;
        }
    }
}

/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 27.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
namespace CovidPropagation
{
    /// <summary>
    /// Période composé d'une activité.
    /// </summary>
    public class Period
    {
        private Site _activity;

        public Site Activity { get => _activity; }

        public Period(Site activity)
        {
            _activity = activity;
        }
    }
}

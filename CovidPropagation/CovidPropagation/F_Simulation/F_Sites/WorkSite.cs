/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */

namespace CovidPropagation
{
    /// <summary>
    /// Lieu dans lequel les individus travaillent.
    /// </summary>
    public class WorkSite : Site
    {
        private int _nbPlaces;
        private int _nbWorker;
        public WorkSite(SiteType[] workPlaceTypes,
                       double length,
                       double width,
                       double height,
                       double ventilationWithOutside,
                       double additionalControlMeasures,
                       int nbPlaces) :
                 base(workPlaceTypes, length, width, height, ventilationWithOutside, additionalControlMeasures)
        {
            this._nbPlaces = nbPlaces;
            _nbWorker = 0;
        }

        /// <summary>
        /// Définit s'il y a encore de la place dans le lieu.
        /// </summary>
        /// <returns>S'il a de la place ou non.</returns>
        public bool IsHiring()
        {
            bool result = false;
            if (_nbWorker < _nbPlaces)
            {
                _nbWorker++;
                result = true;
            }
            return result;
        }
    }
}

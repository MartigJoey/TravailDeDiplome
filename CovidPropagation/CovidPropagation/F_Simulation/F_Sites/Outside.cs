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
    /// Lieu spécial représentant le monde à l'extérieur des bâtiments.
    /// </summary>
    class Outside : Site
    {
        private const int LENGTH = 1000000;
        private const int WIDTH = 1000000;
        private const int HEIGHT = 1000000;
        private const double VENTILATION_WITH_OUTSIDE = 20;

        private static SiteType[] outsideTypes = new SiteType[] { SiteType.Transport, SiteType.Eat };
        public Outside(double length = LENGTH,
                           double width = WIDTH,
                           double height = HEIGHT,
                           double ventilationWithOutside = VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(outsideTypes, length, width, height, ventilationWithOutside, additionalControlMeasures)
        {

        }
    }
}

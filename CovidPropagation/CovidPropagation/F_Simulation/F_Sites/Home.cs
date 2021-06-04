/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */

namespace CovidPropagation
{
    public class Home : Site
    {
        private const int LENGTH = 10;
        private const int WIDTH = 10;
        private const int HEIGHT = 2;
        private static SiteType[] homeTypes = new SiteType[] { SiteType.Home, SiteType.Eat };
        public Home(double length = LENGTH,
                           double width = WIDTH,
                           double height = HEIGHT,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(homeTypes, length, width, height, ventilationWithOutside, additionalControlMeasures)
        {

        }
    }
}

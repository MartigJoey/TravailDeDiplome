/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */

namespace CovidPropagation
{
    class Restaurant : WorkSite
    {
        private const int LENGTH = 13;
        private const int WIDTH = 30;
        private const int HEIGHT = 5;
        private const int NUMBER_OF_WORK_PLACE = 5;

        private static SiteType[] restaurantTypes = new SiteType[] { SiteType.Eat, SiteType.WorkPlace };
        public Restaurant(double length = LENGTH,
                           double width = WIDTH,
                           double height = HEIGHT,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(restaurantTypes, length, width, height, ventilationWithOutside, additionalControlMeasures, NUMBER_OF_WORK_PLACE)
        {

        }

        public Restaurant() : this(GlobalVariables.BUILDING_LENGTH,
                         GlobalVariables.BUILDING_WIDTH,
                         GlobalVariables.BUILDING_HEIGHT,
                         GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                         GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES)
        {

        }
    }
}

/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 29.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */

namespace CovidPropagation
{
    class Store : WorkSite
    {
        private const int LENGTH = 10;
        private const int WIDTH = 10;
        private const int HEIGHT = 3;
        private const int NUMBER_OF_WORK_PLACE = 5;

        private static SiteType[] storeTypes = new SiteType[] { SiteType.Store, SiteType.WorkPlace };
        public Store(double length = LENGTH,
                           double width = WIDTH,
                           double height = HEIGHT,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(storeTypes, length, width, height, ventilationWithOutside, additionalControlMeasures, NUMBER_OF_WORK_PLACE)
        {

        }

        public Store() : this(GlobalVariables.BUILDING_LENGTH,
                 GlobalVariables.BUILDING_WIDTH,
                 GlobalVariables.BUILDING_HEIGHT,
                 GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                 GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES)
        {

        }
    }
}

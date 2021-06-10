/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 29.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */

namespace CovidPropagation
{
    public class Supermarket : WorkSite
    {
        private const int LENGTH = 50;
        private const int WIDTH = 40;
        private const int HEIGHT = 5;
        private const int NUMBER_OF_WORK_PLACE = 15;

        private static SiteType[] supermarketTypes = new SiteType[] { SiteType.Store, SiteType.WorkPlace };
        public Supermarket(double length = LENGTH, 
                           double width = WIDTH, 
                           double height = HEIGHT,  
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE, 
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(supermarketTypes, length, width, height, ventilationWithOutside, additionalControlMeasures, NUMBER_OF_WORK_PLACE)
        {
            
        }

        public Supermarket() : this(GlobalVariables.BUILDING_LENGTH,
                 GlobalVariables.BUILDING_WIDTH,
                 GlobalVariables.BUILDING_HEIGHT,
                 GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                 GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES)
        {

        }
    }
}

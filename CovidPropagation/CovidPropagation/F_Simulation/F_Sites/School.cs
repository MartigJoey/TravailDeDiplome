/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
namespace CovidPropagation
{
    public class School : WorkSite
    {
        private const int LENGTH = 50;
        private const int WIDTH = 70;
        private const int HEIGHT = 5;
        private const int nbWorkPlaces = 20;
        private static SiteType[] schoolTypes = new SiteType[] { SiteType.Eat, SiteType.WorkPlace };
        public School(double length = LENGTH,
                           double width = WIDTH,
                           double height = HEIGHT,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(schoolTypes, length, width, height, ventilationWithOutside, additionalControlMeasures, nbWorkPlaces)
        {

        }

        public School() : this(GlobalVariables.BUILDING_LENGTH,
                         GlobalVariables.BUILDING_WIDTH,
                         GlobalVariables.BUILDING_HEIGHT,
                         GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                         GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES)
        {

        }
    }
}

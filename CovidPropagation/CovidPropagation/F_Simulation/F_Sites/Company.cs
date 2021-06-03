/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */

namespace CovidPropagation
{
    public class Company : WorkSite
    {
        private const int nbWorkPlaces = 150;
        private static SiteType[] companyTypes = new SiteType[] { SiteType.Eat, SiteType.WorkPlace };
        public Company(double length = GlobalVariables.BUILDING_LENGTH,
                           double width = GlobalVariables.BUILDING_WIDTH,
                           double height = GlobalVariables.BUILDING_HEIGHT,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(companyTypes, length, width, height, ventilationWithOutside, additionalControlMeasures, nbWorkPlaces)
        {

        }

        public Company() : this(GlobalVariables.BUILDING_LENGTH,
                 GlobalVariables.BUILDING_WIDTH,
                 GlobalVariables.BUILDING_HEIGHT,
                 GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                 GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES)
        {

        }
    }
}

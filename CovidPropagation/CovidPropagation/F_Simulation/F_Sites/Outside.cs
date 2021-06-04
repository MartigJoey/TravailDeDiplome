/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */

namespace CovidPropagation
{
    class Outside : Site
    {
        private const double VENTILATION_WITH_OUTSIDE = 20;
        private static SiteType[] outsideTypes = new SiteType[] { SiteType.Transport, SiteType.Eat };
        public Outside(double length = 1000000,
                           double width = 1000000,
                           double height = 1000000,
                           double ventilationWithOutside = VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(outsideTypes, length, width, height, ventilationWithOutside, additionalControlMeasures)
        {

        }
    }
}

/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 06.05.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */

namespace CovidPropagation
{
    /// <summary>
    /// Lieu similaire au voiture mais comprenant un chauffeur et plusieurs passagers.
    /// </summary>
    class Bus : WorkSite
    {
        private const int NUMBER_OF_WORK_PLACE = 1;
        private static SiteType[] busTypes = new SiteType[] { SiteType.Transport, SiteType.WorkPlace };

        public Bus(double length = GlobalVariables.BUS_LENGTH,
                   double width = GlobalVariables.BUS_WIDTH,
                   double height = GlobalVariables.BUS_HEIGHT,
                   double ventilationWithOutside = GlobalVariables.BUS_VENTILATION_WITH_OUTSIDE,
                   double additionalControlMeasures = GlobalVariables.BUS_ADDITIONAL_CONTROL_MEASURES) :
                 base(busTypes, length, width, height, ventilationWithOutside, additionalControlMeasures, NUMBER_OF_WORK_PLACE)
        {

        }
    }
}

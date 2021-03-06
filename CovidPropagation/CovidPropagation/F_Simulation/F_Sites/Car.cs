/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */

namespace CovidPropagation
{
    class Car : Site
    {
        private static SiteType[] carTypes = new SiteType[] { SiteType.Transport };

        public Car(double length = GlobalVariables.CAR_LENGTH,
                   double width = GlobalVariables.CAR_WIDTH,
                   double height = GlobalVariables.CAR_HEIGHT,
                   double ventilationWithOutside = GlobalVariables.CAR_VENTILATION_WITH_OUTSIDE,
                   double additionalControlMeasures = GlobalVariables.CAR_ADDITIONAL_CONTROL_MEASURES) :
                 base(carTypes, length, width, height, ventilationWithOutside, additionalControlMeasures)
        {
            
        }
    }
}

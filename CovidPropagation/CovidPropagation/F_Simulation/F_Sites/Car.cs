/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 27.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System.Drawing;

namespace CovidPropagation
{
    class Car : Site
    {

        public Car(int maxPerson,
                           double length = GlobalVariables.CAR_LENGTH,
                           double width = GlobalVariables.CAR_WIDTH,
                           double height = GlobalVariables.CAR_HEIGHT,
                           double pressure = GlobalVariables.CAR_PRESSURE,
                           double temperature = GlobalVariables.CAR_TEMPERATURE,
                           double co2 = GlobalVariables.CAR_ADDITIONAL_CONTROL_MEASURES,
                           double ventilationWithOutside = GlobalVariables.CAR_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.CAR_ADDITIONAL_CONTROL_MEASURES) :
                 base(maxPerson, length, width, height, pressure, temperature, co2, ventilationWithOutside, additionalControlMeasures)
        {
            
        }
    }
}

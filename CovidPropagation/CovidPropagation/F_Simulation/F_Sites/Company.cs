/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 27.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public class Company : Site
    {
        public Company(int maxPerson,
                           double length = GlobalVariables.BUILDING_LENGTH,
                           double width = GlobalVariables.BUILDING_WIDTH,
                           double height = GlobalVariables.BUILDING_HEIGHT,
                           double pressure = GlobalVariables.BUILDING_PRESSURE,
                           double temperature = GlobalVariables.BUILDING_TEMPERATURE,
                           double co2 = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(maxPerson, length, width, height, pressure, temperature, co2, ventilationWithOutside, additionalControlMeasures)
        {

        }
    }
}

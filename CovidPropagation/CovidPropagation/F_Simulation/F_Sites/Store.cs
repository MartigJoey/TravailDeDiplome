/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 29.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    class Store : Site
    {

        private static SiteType[] storeTypes = new SiteType[] { SiteType.Hobby, SiteType.WorkPlace };
        public Store(double length = GlobalVariables.BUILDING_LENGTH,
                           double width = GlobalVariables.BUILDING_WIDTH,
                           double height = GlobalVariables.BUILDING_HEIGHT,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(storeTypes, length, width, height, ventilationWithOutside, additionalControlMeasures)
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

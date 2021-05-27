﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    class Restaurant : WorkSite
    {
        private const int nbWorkPlaces = 5;
        private static SiteType[] restaurantTypes = new SiteType[] { SiteType.Eat, SiteType.WorkPlace };
        public Restaurant(double length = GlobalVariables.BUILDING_LENGTH,
                           double width = GlobalVariables.BUILDING_WIDTH,
                           double height = GlobalVariables.BUILDING_HEIGHT,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(restaurantTypes, length, width, height, ventilationWithOutside, additionalControlMeasures, nbWorkPlaces)
        {

        }

        public Restaurant() : this(GlobalVariables.BUILDING_LENGTH,
                         GlobalVariables.BUILDING_WIDTH,
                         GlobalVariables.BUILDING_HEIGHT,
                         GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                         GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES)
        {

        }
    }
}

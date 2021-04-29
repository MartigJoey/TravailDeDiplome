using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    class Outside : Site
    {
        public Outside(int maxPerson,
                           double length = GlobalVariables.BUILDING_LENGTH,
                           double width = GlobalVariables.BUILDING_WIDTH,
                           double height = GlobalVariables.BUILDING_HEIGHT,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(maxPerson, length, width, height, ventilationWithOutside, additionalControlMeasures)
        {

        }
    }
}

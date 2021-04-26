using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public class Company : Building
    {
        public Company(int maxPerson, double length = 4, double width = 2, double height = 1.7d, double pressure = 0.95d, double temperature = 20, double co2 = 415, double ventilationWithOutside = 5.7d, double additionalControlMeasures = 0) :
                 base(maxPerson, length, width, height, pressure, temperature, co2, ventilationWithOutside, additionalControlMeasures)
        {

        }
    }
}

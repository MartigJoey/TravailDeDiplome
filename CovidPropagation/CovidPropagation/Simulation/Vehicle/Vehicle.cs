using CovidPropagation.Classes;
using CovidPropagation.Simulation;
using System;
using System.Drawing;

namespace CovidPropagation
{
    abstract class Vehicle : Site
    {
        protected Random rdm = new Random();
        protected int _maxPersons;


        public Vehicle(int maxPerson, double length = 4, double width = 2, double height = 1.7d, double pressure = 0.95d, double temperature = 20, double co2 = 415, double ventilationWithOutside = 5.7d, double additionalControlMeasures = 0) : 
                 base (length, width, height, pressure, temperature, co2, ventilationWithOutside, additionalControlMeasures)
        {
            _maxPersons = maxPerson;
        }
    }
}

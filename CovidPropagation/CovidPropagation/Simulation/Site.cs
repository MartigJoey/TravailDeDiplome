using CovidPropagation.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation.Simulation
{
    public class Site
    {

        protected double eventDuration = GlobalVariables.DURATION_OF_PERIOD;
        double hospitalisationRate = 20;
        double deathRate = 4;
        // Size
        protected double length;
        protected double width;
        protected double height;
        protected double air;
        protected double volume;

        // Air
        protected double pressure;
        protected double temperature; // En Celsius
        protected double humidity; // %
        protected double co2;

        // Ventilation & deposition
        protected double ventilationWithOutside;
        protected double decayRateOfVirus;
        protected double depositionOnSurfaceRate;
        protected double additionalControlMeasures;
        protected double sumFirstOrderLossRate;

        protected double ventilationPerPersonRate;

        public Site(double length, double width, double height, double pressure = 0.95d, double temperature = 20, double humidity = 50, double co2 = 415,
                    double ventilationWithOutside = 0.7d, double additionalControlMeasures = 0)
        {
            this.length = length;  
            this.width = width;
            this.height = height;
            this.pressure = pressure;
            this.temperature = temperature;
            this.humidity = humidity;
            this.co2 = co2;

            this.ventilationWithOutside = ventilationWithOutside;
            this.decayRateOfVirus = 0.62d;// récupérer du virus
            this.depositionOnSurfaceRate = 0.3d; // récupérer du virus
            this.additionalControlMeasures = additionalControlMeasures;

            this.sumFirstOrderLossRate = this.ventilationWithOutside + decayRateOfVirus + depositionOnSurfaceRate + this.additionalControlMeasures;

            air = this.length * this.width;
            volume = air * this.height;
        }

        public void CalculateRisk()
        {
            int nbPersons = 61;
            int infectivePersons = 1;
            float fractionOfImmune = 0;
            int nbSusceptiblePersons = ((nbPersons - infectivePersons) * (1-(int)fractionOfImmune) * -1) / 10; // Bof

            //Density
            //double densityAreaPersons = air / nbPersons;
            //double densityPersonsArea = nbPersons / air;
            //double densityVolumePersons = volume / nbPersons;

            // Breathing
            double breathingRate = 0.026d * 60;
            //double co2EmissionsPerPerson = 0.0091d;
            //double co2Emissions = co2EmissionsPerPerson * nbPersons * (1 / pressure) * (273.15d + temperature) / 273.15d;
            double quantaExhalationRateOfInfected = 970; // A modifier en fonction des individus infecté et de leurs symptoms.

            //ventilationPerPersonRate = volume * (ventilationWithOutside + additionalControlMeasures) * 1000 / 3600 / nbPersons;

            // Masks %
            double exhalationMaskEfficiency = 0;
            double percentagePersonWithMask = 0;
            double inhalationMaskEfficiency = 0;


            // Virus
            double probabilityOfBeingInfective = 0.011d;

            // RESULT
            double sumQuantaExhalationRateOfInfected = quantaExhalationRateOfInfected * (1 - exhalationMaskEfficiency * percentagePersonWithMask) * infectivePersons; // Personnalisé par personne en fonction des symptoms

            // sumQuantaExhalationRateOfInfected / sumFirstOrderLossRate / volume * (1-(1 / sumFirstOrderLossRate / eventDuration) * (1-exp(-sumFirstOrderLossRate * eventDuration)))
            double avgQuantaConcentration = (Math.Abs(sumQuantaExhalationRateOfInfected) / Math.Abs(sumFirstOrderLossRate) / Math.Abs(volume) * (1 - (1 / Math.Abs(sumFirstOrderLossRate) / eventDuration) * (1 - Math.Exp(-Math.Abs(sumFirstOrderLossRate) * Math.Abs(eventDuration)))));
            double quantaInhaledPerPerson = avgQuantaConcentration * breathingRate * eventDuration * (1 - inhalationMaskEfficiency * percentagePersonWithMask);

            // Infection
            double probabilityOfOneInfection = 1 - Math.Exp(-quantaInhaledPerPerson);
            double probabilityOfInfection = 1 - Math.Pow((1 - sumQuantaExhalationRateOfInfected * probabilityOfBeingInfective), nbSusceptiblePersons);

            double nOfInfectivePersons = (infectivePersons + nbSusceptiblePersons) * probabilityOfBeingInfective;
            double re = (nbSusceptiblePersons - nOfInfectivePersons) * probabilityOfInfection;
            double probabilityOfHospitalisation = re * hospitalisationRate;
            double probabilityOfDeath = re * deathRate;
        }
    }
}

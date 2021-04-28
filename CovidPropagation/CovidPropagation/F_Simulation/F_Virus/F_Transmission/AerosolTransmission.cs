using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    class AerosolTransmission : Transmission
    {
        private double _avgQuantaConcentration;
        private double _quantaInhaledPerPerson;
        
        private double _probabilityOfOneInfection;
        private double _probabilityOfInfection;
        
        private double _nbOfInfectivePersons;
        private double _probabilityOfHospitalisation;
        private double _probabilityOfDeath;

        public AerosolTransmission()
        {

        }

        /// <summary>
        /// Calcule les risques de propagation d'aérosols.
        /// </summary>
        public TransmissionData CalculateRisk(int nbPersons, int infectivePersons, double fractionOfImmune, int nbPersonsWithMask, double inhalationMaskEfficiency, double sumFirstOrderLossRate, double volume, double quantaExhalationRateOfInfected, double probabilityOfBeingInfective)
        {
            // /!\ Utiliser l'objet AirTransmission pour certains paramètres
            int nbSusceptiblePersons = ((nbPersons - infectivePersons) * (1 - (int)fractionOfImmune) * -1) / 10; // Bof

            #region NotImplementedYet
            //Density
            //double densityAreaPersons = air / nbPersons;
            //double densityPersonsArea = nbPersons / air;
            //double densityVolumePersons = volume / nbPersons;

            // Breathing
            //double co2EmissionsPerPerson = 0.0091d;
            //double co2Emissions = co2EmissionsPerPerson * nbPersons * (1 / pressure) * (273.15d + temperature) / 273.15d;

            //ventilationPerPersonRate = volume * (ventilationWithOutside + additionalControlMeasures) * 1000 / 3600 / nbPersons;
            #endregion

            // Masks 0 à 1
            // double exhalationMaskEfficiency = 0;
            double percentagePersonWithMask = nbPersonsWithMask / nbPersons;

            // sumQuantaExhalationRateOfInfected / sumFirstOrderLossRate / volume * (1-(1 / sumFirstOrderLossRate / eventDuration) * (1-exp(-sumFirstOrderLossRate * eventDuration)))
            _avgQuantaConcentration = (Math.Abs(quantaExhalationRateOfInfected) / Math.Abs(sumFirstOrderLossRate) / Math.Abs(volume) * (1 - (1 / Math.Abs(sumFirstOrderLossRate) / GlobalVariables.DURATION_OF_PERIOD) * (1 - Math.Exp(-Math.Abs(sumFirstOrderLossRate) * Math.Abs(GlobalVariables.DURATION_OF_PERIOD)))));
            _quantaInhaledPerPerson = _avgQuantaConcentration * GlobalVariables.BREATHING_RATE * GlobalVariables.DURATION_OF_PERIOD * (1 - inhalationMaskEfficiency * percentagePersonWithMask);

            // Infection probability
            _probabilityOfOneInfection = 1 - Math.Exp(-_quantaInhaledPerPerson);
            _probabilityOfInfection = 1 - Math.Pow((1 - quantaExhalationRateOfInfected * probabilityOfBeingInfective), nbSusceptiblePersons);
            
            _nbOfInfectivePersons = (infectivePersons + nbSusceptiblePersons) * probabilityOfBeingInfective;
            _probabilityOfInfection = (nbSusceptiblePersons - _nbOfInfectivePersons) * _probabilityOfInfection;
            _probabilityOfHospitalisation = _probabilityOfInfection * Virus.HospitalRate;
            _probabilityOfDeath = _probabilityOfInfection * Virus.DeathRate;

            return new TransmissionData(
                _avgQuantaConcentration,
                _quantaInhaledPerPerson,
                _probabilityOfOneInfection,
                _probabilityOfInfection,
                _nbOfInfectivePersons,
                _probabilityOfHospitalisation,
                _probabilityOfDeath
            );
        }
    }
}

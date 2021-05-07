using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CovidPropagation
{
    class AerosolTransmission : Transmission
    {        
        private double _probabilityOfOneInfection;
        private double _probabilityOfInfection;
        
        private double _nOfInfectivePersons;
        private double _probabilityOfHospitalisation;
        private double _probabilityOfDeath;
        private double _virusAraisingCases;

        public AerosolTransmission()
        {

        }

        /// <summary>
        /// Calcule les risques de propagation d'aérosols.
        /// </summary>
        public TransmissionData CalculateRisk(int nbPersons, int infectivePersons, double fractionOfImmune, int nbPersonsWithMask, double inhalationMaskEfficiency, double sumFirstOrderLossRate, double volume, double quantaExhalationRateOfInfected, double probabilityOfBeingInfective, double quantaInhaledPerPerson)
        {
            // /!\ Utiliser l'objet AirTransmission pour certains paramètres
            int nbSusceptiblePersons = ((nbPersons - infectivePersons) * (1 - (int)fractionOfImmune)); // Bof

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

            // Infection probability
            _probabilityOfOneInfection = 1 - Math.Exp(-quantaInhaledPerPerson);
            _probabilityOfInfection = (1 - Math.Pow((1 - _probabilityOfOneInfection * probabilityOfBeingInfective), nbSusceptiblePersons)) / 10;

            _nOfInfectivePersons = (infectivePersons + nbSusceptiblePersons) * probabilityOfBeingInfective;
            _nOfInfectivePersons = _nOfInfectivePersons.SetValueIfNaN();
            _virusAraisingCases = (nbSusceptiblePersons - _nOfInfectivePersons) * _probabilityOfInfection;
            _virusAraisingCases = _virusAraisingCases.SetValueIfNaN();
            _probabilityOfHospitalisation = _probabilityOfInfection * Virus.HospitalRate;
            _probabilityOfDeath = _probabilityOfInfection * Virus.DeathRate;

            return new TransmissionData(
                _probabilityOfOneInfection,
                _probabilityOfInfection,
                _nOfInfectivePersons,
                _probabilityOfHospitalisation,
                _probabilityOfDeath,
                _virusAraisingCases
            );
        }
    }
}

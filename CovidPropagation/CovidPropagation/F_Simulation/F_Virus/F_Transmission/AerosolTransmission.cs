/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System;

namespace CovidPropagation
{
    class AerosolTransmission : Transmission
    {        
        private double _probabilityOfOneInfection;
        private double _probabilityOfInfection;
        
        private double _nOfInfectivePersons;
        private double _virusAraisingCases;

        public AerosolTransmission()
        {

        }

        /// <summary>
        /// Calcule les risques de propagation d'aérosols.
        /// </summary>
        public AerosolTransmissionData CalculateRisk(int nbPersons, int infectivePersons, double fractionOfImmune, int nbPersonsWithMask, double inhalationMaskEfficiency, double sumFirstOrderLossRate, double volume, double quantaExhalationRateOfInfected, double probabilityOfBeingInfective, double quantaInhaledPerPerson)
        {
            int nbSusceptiblePersons = ((nbPersons - infectivePersons) * (1 - (int)fractionOfImmune));

            // Infection probability
            _probabilityOfOneInfection = 1 - Math.Exp(-quantaInhaledPerPerson);
            _probabilityOfInfection = (1 - Math.Pow((1 - _probabilityOfOneInfection * probabilityOfBeingInfective), nbSusceptiblePersons)) / 10;

            _nOfInfectivePersons = (infectivePersons + nbSusceptiblePersons) * probabilityOfBeingInfective;
            _nOfInfectivePersons = _nOfInfectivePersons.SetValueIfNaN();
            _virusAraisingCases = (nbSusceptiblePersons - _nOfInfectivePersons) * _probabilityOfInfection;
            _virusAraisingCases = _virusAraisingCases.SetValueIfNaN();

            return new AerosolTransmissionData(
                _probabilityOfOneInfection,
                _probabilityOfInfection,
                _nOfInfectivePersons,
                _virusAraisingCases
            );
        }
    }
}

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
using System.Linq;

namespace CovidPropagation
{
    /// <summary>
    /// Lieu dans lequel des personnes entrent et sortent et propageant le virus.
    /// </summary>
    public class Site
    {
        List<Person> persons;
        bool hasEnvironnementChanged;
        double averageQuantaExhalationRate;

        #region probability
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

        protected double probabilityOfInfection;
        #endregion

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

            persons = new List<Person>();
            hasEnvironnementChanged = true;
            averageQuantaExhalationRate = GlobalVariables.AVERAGE_QUANTA_EXHALATION;
        }

        /// <summary>
        /// Récupère les probabilité d'infection d'une personne dans ce lieu.
        /// Recalcul le taux de probabilité si nécessaire.
        /// </summary>
        /// <returns>Taux de probabilité d'infection actuellement dans ce lieu</returns>
        public double GetProbabilityOfInfection()
        {
            if (hasEnvironnementChanged)
            {
                if (Virus.IsTransmissibleBy(typeof(AirTransmission)))
                    CalculateAerosolRisk();

                hasEnvironnementChanged = false;
            }
            return probabilityOfInfection;
        }

        /// <summary>
        /// Calcule les risques de propagation d'aérosols ainsi que d'autres nombreux paramètres du site.
        /// </summary>
        private void CalculateAerosolRisk()
        {
            // /!\ Utiliser l'objet AirTransmission pour certains paramètres
            int nbPersons = persons.Count;
            int infectivePersons = persons.Where(p => (int)p.CurrentState > 1).Count();
            double fractionOfImmune = persons.Where(p => p.CurrentState == PersonState.Immune).Count() / nbPersons * 100; // %
            int nbSusceptiblePersons = ((nbPersons - infectivePersons) * (1-(int)fractionOfImmune) * -1) / 10; // Bof

            //Density
            //double densityAreaPersons = air / nbPersons;
            //double densityPersonsArea = nbPersons / air;
            //double densityVolumePersons = volume / nbPersons;

            // Breathing
            double breathingRate = 0.026d * 60;
            //double co2EmissionsPerPerson = 0.0091d;
            //double co2Emissions = co2EmissionsPerPerson * nbPersons * (1 / pressure) * (273.15d + temperature) / 273.15d;
            double quantaExhalationRateOfInfected; // A modifier en fonction des individus infecté et de leurs symptoms.

            //ventilationPerPersonRate = volume * (ventilationWithOutside + additionalControlMeasures) * 1000 / 3600 / nbPersons;

            // Masks 0 à 1
            //double exhalationMaskEfficiency = 0;
            int nbPersonsWithMask = persons.Where(p => p.HasMask).Count();
            double percentagePersonWithMask = nbPersonsWithMask / persons.Count;
            double inhalationMaskEfficiency = persons.Where(p => p.HasMask).Sum(p => p.InhalationMaskEfficiency) / nbPersonsWithMask;


            // Virus
            double probabilityOfBeingInfective = 0.011d;

            // RESULT
            //Ancienne version: quantaExhalationRateOfInfected = quantaExhalationRateOfInfected * (1 - exhalationMaskEfficiency * percentagePersonWithMask) * infectivePersons; // Personnalisé par personne en fonction des symptoms
            quantaExhalationRateOfInfected = persons.Where(p => (int)p.CurrentState > 2).Sum(p =>  p.QuantaExhalationRate * (1 - p.ExhalationMaskEfficiency * p.HasMask.ConvertToInt())) / infectivePersons;

            // sumQuantaExhalationRateOfInfected / sumFirstOrderLossRate / volume * (1-(1 / sumFirstOrderLossRate / eventDuration) * (1-exp(-sumFirstOrderLossRate * eventDuration)))
            double avgQuantaConcentration = (Math.Abs(quantaExhalationRateOfInfected) / Math.Abs(sumFirstOrderLossRate) / Math.Abs(volume) * (1 - (1 / Math.Abs(sumFirstOrderLossRate) / eventDuration) * (1 - Math.Exp(-Math.Abs(sumFirstOrderLossRate) * Math.Abs(eventDuration)))));
            double quantaInhaledPerPerson = avgQuantaConcentration * breathingRate * eventDuration * (1 - inhalationMaskEfficiency * percentagePersonWithMask);

            // Infection
            double probabilityOfOneInfection = 1 - Math.Exp(-quantaInhaledPerPerson);
            double probabilityOfInfection = 1 - Math.Pow((1 - quantaExhalationRateOfInfected * probabilityOfBeingInfective), nbSusceptiblePersons);

            double nOfInfectivePersons = (infectivePersons + nbSusceptiblePersons) * probabilityOfBeingInfective;
            probabilityOfInfection = (nbSusceptiblePersons - nOfInfectivePersons) * probabilityOfInfection;
            double probabilityOfHospitalisation = probabilityOfInfection * hospitalisationRate;
            double probabilityOfDeath = probabilityOfInfection * deathRate;
        }

        /// <summary>
        /// Fait entrer une personne sur le site.
        /// L'ajoutant à la liste de personne et informant le site qu'il doit recalculer le taux de propagation dans le lieu.
        /// </summary>
        /// <param name="personEntering">La personne qui entre</param>
        public void Enter(Person personEntering)
        {
            persons.Add(personEntering);
            hasEnvironnementChanged = true;
        }

        /// <summary>
        /// Fait sortir une personne du site.
        /// La retirant de la liste de personne et informant le site qu'il doit recalculer le taux de propagation dans le lieu.
        /// </summary>
        /// <param name="personLeaving">La personne quittant le site</param>
        public void Leave(Person personLeaving)
        {
            persons.Remove(personLeaving);
            hasEnvironnementChanged = true;
        }

        /// <summary>
        /// Récupère le taux de quanta exhalé moyen dans se bâtiment.
        /// Utile pour différencier une salle de sport d'une classe par exemple.
        /// </summary>
        /// <returns>Taux de quanta exhalé moyen.</returns>
        public double GetAverageQuantaExhalationRate()
        {
            return averageQuantaExhalationRate;
        }
    }
}

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
        double hospitalisationRate = 20;
        double deathRate = 4;
        // Size
        protected double length;
        protected double width;
        protected double height;
        protected double air;
        protected double volume;

        // Air
        protected double temperature; // En Celsius
        protected double humidity; // %

        // Ventilation & deposition
        protected double ventilationWithOutside;
        protected double decayRateOfVirus;
        protected double depositionOnSurfaceRate;
        protected double additionalControlMeasures;
        protected double sumFirstOrderLossRate;

        protected double ventilationPerPersonRate;

        protected double probabilityOfInfection;

        #endregion

        public Site(double length, double width, double height,
                    double ventilationWithOutside = 0.7d, double additionalControlMeasures = 0)
        {
            this.length = length;  
            this.width = width;
            this.height = height;

            this.ventilationWithOutside = ventilationWithOutside;
            this.decayRateOfVirus = Virus.DecayRateOfVirus;// récupérer du virus
            this.depositionOnSurfaceRate = Virus.DepositionOnSurfaceRate; // récupérer du virus
            this.additionalControlMeasures = additionalControlMeasures;

            this.sumFirstOrderLossRate = this.ventilationWithOutside + decayRateOfVirus + depositionOnSurfaceRate + this.additionalControlMeasures;

            air = this.length * this.width;
            volume = air * this.height;

            persons = new List<Person>();
            hasEnvironnementChanged = true;
            averageQuantaExhalationRate = GlobalVariables.AVERAGE_QUANTA_EXHALATION;
        }

        /// <summary>
        /// Récupère les taux de probabilité d'infection d'une personne dans ce lieu.
        /// </summary>
        /// <returns>Taux de probabilité d'infection actuellement dans ce lieu</returns>
        public double GetProbabilityOfInfection()
        {
            return probabilityOfInfection;
        }

        /// <summary>
        /// Calcul le taux de probabilité d'infection d'une personne dans ce lieu.
        /// </summary>
        public void CalculateprobabilityOfInfection()
        {
            if (hasEnvironnementChanged && persons.Count > 0)
            {
                if (Virus.IsTransmissibleBy(typeof(AerosolTransmission)))
                {
                    AerosolTransmission aerosolTransmission = Virus.GetTransmission(typeof(AerosolTransmission)) as AerosolTransmission;
                    int nbPersons = persons.Count;
                    int infectivePersons = persons.Where(p => (int)p.CurrentState > 1).Count();
                    double fractionOfImmune = persons.Where(p => p.CurrentState == PersonState.Immune).Count() / nbPersons * 100; // %
                    int nbPersonsWithMask = persons.Where(p => p.HasMask).Count();
                    double inhalationMaskEfficiency = persons.Where(p => p.HasMask).Sum(p => p.InhalationMaskEfficiency) / nbPersonsWithMask;
                    double probabilityOfBeingInfective = persons.Where(p => (int)p.CurrentState >= 3).Count() / nbPersons; // A modifier pour entrer en accord avec la simulation

                    // Personnalisé par personne en fonction des symptoms
                    // Ancienne version: quantaExhalationRateOfInfected = quantaExhalationRateOfInfected * (1 - exhalationMaskEfficiency * percentagePersonWithMask) * infectivePersons; 
                    double quantaExhalationRateOfInfected = persons.Where(p => (int)p.CurrentState > 2).Sum(p => p.QuantaExhalationRate * (1 - p.ExhalationMaskEfficiency * p.HasMask.ConvertToInt())) / infectivePersons;

                    TransmissionData aerosolDatas = aerosolTransmission.CalculateRisk(nbPersons, infectivePersons, fractionOfImmune, nbPersonsWithMask, inhalationMaskEfficiency, sumFirstOrderLossRate, volume, quantaExhalationRateOfInfected, probabilityOfBeingInfective);
                }
            }
            hasEnvironnementChanged = false;
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

        public int CountNbPeople()
        {
            return persons.Count;
        }
    }
}

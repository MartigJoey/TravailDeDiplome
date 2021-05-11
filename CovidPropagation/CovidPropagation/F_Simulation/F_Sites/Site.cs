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
using System.Diagnostics;

namespace CovidPropagation
{
    /// <summary>
    /// Lieu dans lequel des personnes entrent et sortent et propageant le virus.
    /// </summary>
    public class Site
    {
        private const double DURATION_OF_HOUR = 60;

        List<Person> persons;
        bool hasEnvironnementChanged;
        double averageQuantaExhalationRate;
        SiteType[] types;
        bool clientsMustWearMasks;
        bool workersMustWearMasks;

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
        private double sumFirstOrderLossRate;

        protected double ventilationPerPersonRate;

        private double probabilityOfInfection;
        double fractionPersonsWithMask;
        double exhalationMaskEfficiency;
        double netEmissionRate;
        double avgQuantaConcentration;
        double quantaInhaledPerPerson;

        protected int nbPersons;
        protected int nbInfectivePersons;
        protected double fractionOfImmune;
        protected int nbPersonsWithMask;
        protected double inhalationMaskEfficiency;
        protected double probabilityOfBeingInfective;
        protected double quantaExhalationRateOfInfected;

        protected double probabilityOfOneInfection;
        protected double nbOfInfectivePersons;
        protected double virusAraisingCases;



        public SiteType[] Type { get => types; }
        public int NbPersons { get => nbPersons; set => nbPersons = value; }
        public int NbInfectivePersons { get => nbInfectivePersons; set => nbInfectivePersons = value; }
        public double FractionOfImmune { get => fractionOfImmune; set => fractionOfImmune = value; }
        public int NbPersonsWithMask { get => nbPersonsWithMask; set => nbPersonsWithMask = value; }
        public double InhalationMaskEfficiency { get => inhalationMaskEfficiency; set => inhalationMaskEfficiency = value; }
        public double ProbabilityOfBeingInfective { get => probabilityOfBeingInfective; set => probabilityOfBeingInfective = value; }
        public double QuantaExhalationRateOfInfected { get => quantaExhalationRateOfInfected; set => quantaExhalationRateOfInfected = value; }
        public double ProbabilityOfOneInfection { get => probabilityOfOneInfection; set => probabilityOfOneInfection = value; }
        public double NbOfInfectivePersons { get => nbOfInfectivePersons; set => nbOfInfectivePersons = value; }
        public double VirusAraisingCases { get => virusAraisingCases; set => virusAraisingCases = value; }
        public double FractionPersonsWithMask { get => fractionPersonsWithMask; set => fractionPersonsWithMask = value; }
        public double ExhalationMaskEfficiency { get => exhalationMaskEfficiency; set => exhalationMaskEfficiency = value; }
        public double NetEmissionRate { get => netEmissionRate; set => netEmissionRate = value; }
        public double AvgQuantaConcentration { get => avgQuantaConcentration; set => avgQuantaConcentration = value; }
        public double QuantaInhaledPerPerson { get => quantaInhaledPerPerson; set => quantaInhaledPerPerson = value; }
        public double SumFirstOrderLossRate { get => sumFirstOrderLossRate; set => sumFirstOrderLossRate = value; }
        public double ProbabilityOfInfection { get => probabilityOfInfection; set => probabilityOfInfection = value; }
        public double AverageQuantaExhalationRate { get => averageQuantaExhalationRate; set => averageQuantaExhalationRate = value; }
        public bool HasEnvironnementChanged { get => hasEnvironnementChanged; set => hasEnvironnementChanged = value; }

        #endregion

        public Site(SiteType[] types, double length, double width, double height,
                    double ventilationWithOutside = 0.7d, double additionalControlMeasures = 0)
        {
            this.types = types;
            this.length = length;  
            this.width = width;
            this.height = height;

            this.ventilationWithOutside = ventilationWithOutside;
            this.decayRateOfVirus = Virus.DecayRateOfVirus;// récupérer du virus
            this.depositionOnSurfaceRate = Virus.DepositionOnSurfaceRate; // récupérer du virus
            this.additionalControlMeasures = additionalControlMeasures;

            this.SumFirstOrderLossRate = this.ventilationWithOutside + decayRateOfVirus + depositionOnSurfaceRate + this.additionalControlMeasures;
            air = this.length * this.width;
            volume = air * this.height;

            persons = new List<Person>();
            HasEnvironnementChanged = true;
            AverageQuantaExhalationRate = GlobalVariables.AVERAGE_QUANTA_EXHALATION;
        }

        /// <summary>
        /// Récupère les taux de probabilité d'infection d'une personne dans ce lieu.
        /// </summary>
        /// <returns>Taux de probabilité d'infection actuellement dans ce lieu</returns>
        public double GetProbabilityOfInfection()
        {
            return ProbabilityOfInfection;
        }

        /// <summary>
        /// Calcul le taux de probabilité d'infection d'une personne dans ce lieu.
        /// </summary>
        public void CalculateprobabilityOfInfection()
        {
            if (HasEnvironnementChanged && persons.Count > 0)
            {
                if (Virus.IsTransmissibleBy(typeof(AerosolTransmission)))
                {
                    AerosolTransmission aerosolTransmission = Virus.GetTransmission(typeof(AerosolTransmission)) as AerosolTransmission;
                    NbPersons = persons.Count;
                    NbInfectivePersons = CountNumberInfectivePersons();
                    FractionOfImmune = GetFractionOfImmune(NbPersons);

                    NbPersonsWithMask = CountPersonsWithMask(); 
                    FractionPersonsWithMask = GetFractionpersonsWithMask(NbPersonsWithMask, NbPersons);

                    InhalationMaskEfficiency = GetInhalationMaskEfficiency(NbPersonsWithMask);
                    ExhalationMaskEfficiency = GetExhalationMaskEfficiency(NbPersonsWithMask);

                    ProbabilityOfBeingInfective = GetprobabilityOfBeingInfective(); // actuellement valeur fixe

                    QuantaExhalationRateOfInfected = GetQuantaExhalationRateofInfected(NbInfectivePersons);
                    NetEmissionRate = GetNetEmissionRate(QuantaExhalationRateOfInfected, ExhalationMaskEfficiency, 
                                                                FractionPersonsWithMask, NbInfectivePersons);

                    double eventDuration = GlobalVariables.DURATION_OF_TIMEFRAME / DURATION_OF_HOUR;
                    AvgQuantaConcentration = GetAverageQuantaConcentration(NetEmissionRate, eventDuration);

                    QuantaInhaledPerPerson = GetQuantaInhaledPerPerson(AvgQuantaConcentration, eventDuration, InhalationMaskEfficiency, FractionPersonsWithMask);

                    TransmissionData aerosolDatas = aerosolTransmission.CalculateRisk(NbPersons, NbInfectivePersons, FractionOfImmune,
                                                                                      NbPersonsWithMask, InhalationMaskEfficiency, SumFirstOrderLossRate,
                                                                                      volume, QuantaExhalationRateOfInfected, ProbabilityOfBeingInfective,
                                                                                      QuantaInhaledPerPerson);

                    ProbabilityOfInfection = aerosolDatas.ProbabilityOfInfection.SetValueIfNaN();
                    ProbabilityOfOneInfection = aerosolDatas.ProbabilityOfOneInfection.SetValueIfNaN();
                    NbOfInfectivePersons = aerosolDatas.NOfInfectivePersons.SetValueIfNaN();
                    VirusAraisingCases = aerosolDatas.VirusAraisingCases.SetValueIfNaN();
                }
            }
            HasEnvironnementChanged = false;
        }

        /// <summary>
        /// Fait entrer une personne sur le site.
        /// L'ajoutant à la liste de personne et informant le site qu'il doit recalculer le taux de propagation dans le lieu.
        /// Lui demande de porter le masque si nécessaire.
        /// </summary>
        /// <param name="personEntering">La personne qui entre</param>
        public void Enter(Person personEntering, SitePersonStatus sitePersonStatus)
        {
            persons.Add(personEntering);
            HasEnvironnementChanged = true;

            if (sitePersonStatus == SitePersonStatus.Client && clientsMustWearMasks)
                personEntering.PutMaskOn();
            else if(sitePersonStatus == SitePersonStatus.Worker && workersMustWearMasks)
                personEntering.PutMaskOn();


        }

        /// <summary>
        /// Fait sortir une personne du site.
        /// La retirant de la liste de personne et informant le site qu'il doit recalculer le taux de propagation dans le lieu.
        /// </summary>
        /// <param name="personLeaving">La personne quittant le site</param>
        public void Leave(Person personLeaving)
        {
            persons.Remove(personLeaving);
            HasEnvironnementChanged = true;
            personLeaving.RemoveMask();
        }

        public void SetMaskMeasure(bool clientsMustWearMasks, bool workersMustWearMasks)
        {
            this.clientsMustWearMasks = clientsMustWearMasks;
            this.workersMustWearMasks = workersMustWearMasks;
        }

        public void SetDistanciations(bool isDistanciationSet)
        {
            additionalControlMeasures = isDistanciationSet.ConvertToInt();
            SumFirstOrderLossRate = ventilationWithOutside + decayRateOfVirus + depositionOnSurfaceRate + additionalControlMeasures;
        }

        #region Calculs
        private int CountNumberInfectivePersons()
        {
            return persons.Where(p => (int)p.CurrentState >= (int)PersonState.Infected).Count();
        }
        private double GetFractionOfImmune(double nbPersons)
        {
            return (double)persons.Where(p => p.CurrentState == PersonState.Immune).Count() / nbPersons * 100d;
        }
        private int CountPersonsWithMask()
        {
            return persons.Where(p => p.HasMask).Count();
        }

        private double GetFractionpersonsWithMask(double nbPersonsWithMask, double nbPersons)
        {
            return nbPersonsWithMask / (nbPersons / 100d) / 100d; //fractionPersonsWithMask = fractionPersonsWithMask.SetValueIfNaN();
        }

        private double GetInhalationMaskEfficiency(double nbPersonsWithMask)
        {
            double inhalationMaskEfficiency = (double)persons.Where(p => p.HasMask)
                                                             .Sum(p => p.InhalationMaskEfficiency) 
                                                             / nbPersonsWithMask; 
            return inhalationMaskEfficiency.SetValueIfNaN();
        }

        private double GetExhalationMaskEfficiency(double nbPersonsWithMask)
        {
            double exhalationMaskEfficiency = (double)persons.Where(p => p.HasMask)
                                                             .Sum(p => p.ExhalationMaskEfficiency) 
                                                             / nbPersonsWithMask; 
            return exhalationMaskEfficiency.SetValueIfNaN();
        }
        
        private double GetprobabilityOfBeingInfective()
        {
            double probabilityOfBeingInfective = 0.0011;//(double)persons.Where(p => (int)p.CurrentState > 2).Count() / (double)nbPersons; // A modifier pour entrer en accord avec la simulation
            return probabilityOfBeingInfective.SetValueIfNaN();
        }

        private double GetQuantaExhalationRateofInfected(double nbInfectivePersons)
        {
            double quantaExhalationRateOfInfected = persons.Where(p => (int)p.CurrentState > (int)PersonState.Infectious)
                                                            .Sum(p => p.QuantaExhalationRate * (1 - p.ExhalationMaskEfficiency * p.HasMask.ConvertToInt())) 
                                                            / nbInfectivePersons; 

            return quantaExhalationRateOfInfected.SetValueIfNaN();
        }

        private double GetNetEmissionRate(double quantaExhalationRateOfInfected, double exhalationMaskEfficiency, double fractionPersonsWithMask, double nbInfectivePersons)
        {
            return quantaExhalationRateOfInfected * (1 - exhalationMaskEfficiency * fractionPersonsWithMask) * nbInfectivePersons;
        }

        private double GetAverageQuantaConcentration(double netEmissionRate, double eventDuration)
        {
            double avgQuantaConcentration = Math.Abs(netEmissionRate) / 
                                            Math.Abs(SumFirstOrderLossRate) / 
                                            volume * 
                                            ( 1 - (1 / Math.Abs(SumFirstOrderLossRate) / eventDuration) * (1 - Math.Exp(-SumFirstOrderLossRate * eventDuration)));

            return avgQuantaConcentration.SetValueIfNaN();
        }

        private double GetQuantaInhaledPerPerson(double avgQuantaConcentration, double eventDuration, double inhalationMaskEfficiency, double fractionPersonsWithMask)
        {
            double quantaInhaledPerPerson = avgQuantaConcentration * Math.Abs(SumFirstOrderLossRate) * eventDuration * (1 - inhalationMaskEfficiency * fractionPersonsWithMask);
            return quantaInhaledPerPerson.SetValueIfNaN();
        }
        
        #endregion
    }
}

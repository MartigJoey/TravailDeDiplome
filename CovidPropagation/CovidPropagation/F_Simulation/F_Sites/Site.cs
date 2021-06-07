/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CovidPropagation
{
    /// <summary>
    /// ID Documentation : Site_Class
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
        protected int nbInfectiousPersons;
        protected double fractionOfImmune;
        protected int nbPersonsWithMask;
        protected double inhalationMaskEfficiency;
        protected double probabilityOfBeingInfectious;
        protected double quantaExhalationRateOfInfected;

        protected double probabilityOfOneInfection;
        protected double nbOfInfectiousPersons;
        protected double virusAraisingCases;

        public SiteType[] Type { get => types; }
        public int NbPersons { get => nbPersons; set => nbPersons = value; }
        public int NbInfectiousPersons { get => nbInfectiousPersons; set => nbInfectiousPersons = value; }
        public double FractionOfImmune { get => fractionOfImmune; set => fractionOfImmune = value; }
        public int NbPersonsWithMask { get => nbPersonsWithMask; set => nbPersonsWithMask = value; }
        public double InhalationMaskEfficiency { get => inhalationMaskEfficiency; set => inhalationMaskEfficiency = value; }
        public double ProbabilityOfBeingInfectious { get => probabilityOfBeingInfectious; set => probabilityOfBeingInfectious = value; }
        public double QuantaExhalationRateOfInfected { get => quantaExhalationRateOfInfected; set => quantaExhalationRateOfInfected = value; }
        public double ProbabilityOfOneInfection { get => probabilityOfOneInfection; set => probabilityOfOneInfection = value; }
        public double NbOfInfectiousPersons { get => nbOfInfectiousPersons; set => nbOfInfectiousPersons = value; }
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
        /// ID Documentation : Calculate_Probability
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
                    NbInfectiousPersons = CountNumberInfectiousPersons();
                    FractionOfImmune = GetFractionOfImmune();

                    NbPersonsWithMask = CountPersonsWithMask(); 
                    FractionPersonsWithMask = GetFractionPersonsWithMask();

                    InhalationMaskEfficiency = GetInhalationMaskEfficiency();
                    ExhalationMaskEfficiency = GetExhalationMaskEfficiency();

                    ProbabilityOfBeingInfectious = GetprobabilityOfBeingInfectious();

                    QuantaExhalationRateOfInfected = GetQuantaExhalationRateofInfected();
                    NetEmissionRate = GetNetEmissionRate(QuantaExhalationRateOfInfected, ExhalationMaskEfficiency, 
                                                                FractionPersonsWithMask, NbInfectiousPersons);

                    double eventDuration = GlobalVariables.DURATION_OF_TIMEFRAME / DURATION_OF_HOUR;
                    AvgQuantaConcentration = GetAverageQuantaConcentration(NetEmissionRate, eventDuration);

                    QuantaInhaledPerPerson = GetQuantaInhaledPerPerson(AvgQuantaConcentration, eventDuration, InhalationMaskEfficiency, FractionPersonsWithMask);

                    TransmissionData aerosolDatas = aerosolTransmission.CalculateRisk(NbPersons, NbInfectiousPersons, FractionOfImmune,
                                                                                      NbPersonsWithMask, InhalationMaskEfficiency, SumFirstOrderLossRate,
                                                                                      volume, QuantaExhalationRateOfInfected, ProbabilityOfBeingInfectious,
                                                                                      QuantaInhaledPerPerson);

                    ProbabilityOfInfection = aerosolDatas.ProbabilityOfInfection.SetValueIfNaN();
                    ProbabilityOfOneInfection = aerosolDatas.ProbabilityOfOneInfection.SetValueIfNaN();
                    NbOfInfectiousPersons = aerosolDatas.NOfInfectiousPersons.SetValueIfNaN();
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

        /// <summary>
        /// Applique oui ou non la mesure du port des masques aux client et/ou aux employés.
        /// </summary>
        /// <param name="clientsMustWearMasks">True si les clients doivent porter le masque.</param>
        /// <param name="workersMustWearMasks">True si les employés doivent porter le masque</param>
        public void SetMaskMeasure(bool clientsMustWearMasks, bool workersMustWearMasks)
        {
            this.clientsMustWearMasks = clientsMustWearMasks;
            this.workersMustWearMasks = workersMustWearMasks;
        }

        /// <summary>
        /// Définit si les distanciation social sont en place ou non.
        /// Change les mesures de controles additionnels en fonction du paramètre.
        /// </summary>
        public void SetDistanciations(bool isDistanciationSet)
        {
            additionalControlMeasures = isDistanciationSet.ConvertToInt();
            SumFirstOrderLossRate = ventilationWithOutside + decayRateOfVirus + depositionOnSurfaceRate + additionalControlMeasures;
        }

        /// <summary>
        /// Permet de convertir le type de bâtiment en int pour simplifier sont transfère vers le GUI.
        /// </summary>
        /// <returns>Type de cet objet convertis en int.</returns>
        public int ConvertTypeToInt()
        {
            Type thisType = this.GetType();
            int result = 0;
            if (thisType == typeof(Home))
                result = 0;
            else if (thisType == typeof(Hospital))
                result = 1;
            else if (thisType == typeof(Restaurant))
                result = 2;
            else if (thisType == typeof(School))
                result = 3;
            else if (thisType == typeof(Store))
                result = 4;
            else if (thisType == typeof(Supermarket))
                result = 5;
            else if (thisType == typeof(Company))
                result = 6;
            else if (thisType == typeof(Bus))
                result = 7;

            return result;
        }

        #region Calculs

        /// <summary>
        /// Compte le nombre d'infecté dans le bâtiments.
        /// </summary>
        /// <returns>Nombre d'infectés.</returns>
        private int CountNumberInfectiousPersons()
        {
            return persons.Where(p => (int)p.CurrentState >= (int)PersonState.Infectious).Count();
        }

        /// <summary>
        /// Compte le pourcentage d'individus infectés dans le bâtiment.
        /// </summary>
        /// <returns>Pourcentage d'infecté dans le bâtiment.</returns>
        private double GetFractionOfImmune()
        {
            return (double)persons.Where(p => p.CurrentState == PersonState.Immune).Count() / NbPersons * 100d;
        }

        /// <summary>
        /// Compte le nombre de personnes qui portent le masque dans le bâtiment.
        /// </summary>
        /// <returns>Nombre de personnes qui portent le masque.</returns>
        private int CountPersonsWithMask()
        {
            return persons.Where(p => p.HasMask).Count();
        }

        /// <summary>
        /// Récupère le pourcentage de personnes qui porte le masque dans le bâtiment.
        /// </summary>
        /// <returns>Pourcentage de personnes portant le masque.</returns>
        private double GetFractionPersonsWithMask()
        {
            return NbPersonsWithMask / (NbPersons / 100d) / 100d; //fractionPersonsWithMask = fractionPersonsWithMask.SetValueIfNaN();
        }

        /// <summary>
        /// Récupère la moyenne d'éfficacité de filtrage, lors d'une inhalation, des masque présents dans le bâtiment.
        /// </summary>
        /// <returns>Moyenne d'efficacité des masques</returns>
        private double GetInhalationMaskEfficiency()
        {
            double inhalationMaskEfficiency = (double)persons.Where(p => p.HasMask)
                                                             .Sum(p => p.InhalationMaskEfficiency) 
                                                             / NbPersonsWithMask; 
            return inhalationMaskEfficiency.SetValueIfNaN();
        }

        /// <summary>
        /// Récupère la moyenne d'éfficacité de filtrage, lors d'une exhalation, des masque présents dans le bâtiment.
        /// </summary>
        /// <returns>Moyenne d'efficacité des masques</returns>
        private double GetExhalationMaskEfficiency()
        {
            double exhalationMaskEfficiency = (double)persons.Where(p => p.HasMask)
                                                             .Sum(p => p.ExhalationMaskEfficiency) 
                                                             / NbPersonsWithMask; 
            return exhalationMaskEfficiency.SetValueIfNaN();
        }
        
        /// <summary>
        /// Récupère la probabilité qu'une personne soit infectieuse.
        /// </summary>
        /// <returns>Probabilité qu'une personne soit infectieuse.</returns>
        private double GetprobabilityOfBeingInfectious()
        {
            double probabilityOfBeingInfectious = (double)persons.Where(p => (int)p.CurrentState >= (int)PersonState.Infectious).Count() / (double)nbPersons; // A modifier pour entrer en accord avec la simulation
            return probabilityOfBeingInfectious.SetValueIfNaN();
        }

        /// <summary>
        /// Récupère la moyenne de quanta exhalé par les personnes infectées et contagieuses.
        /// </summary>
        /// <returns>Moyenne des quantas exhalés</returns>
        private double GetQuantaExhalationRateofInfected()
        {
            double quantaExhalationRateOfInfected = persons.Where(p => (int)p.CurrentState >= (int)PersonState.Infectious)
                                                            .Sum(p => p.QuantaExhalationRate * (1 - p.ExhalationMaskEfficiency * p.HasMask.ConvertToInt())) / NbInfectiousPersons;

            return quantaExhalationRateOfInfected.SetValueIfNaN();
        }

        /// <summary>
        /// Récupère les emissions de quantas en prenom en compte le nombre d'infecté ainsi que le nombre de personne portant le masque et leur efficacités.
        /// </summary>
        /// <returns>Emissions de quantas</returns>
        private double GetNetEmissionRate(double quantaExhalationRateOfInfected, double exhalationMaskEfficiency, double fractionPersonsWithMask, double nbInfectiousPersons)
        {
            return quantaExhalationRateOfInfected * (1 - exhalationMaskEfficiency * fractionPersonsWithMask) * nbInfectiousPersons;
        }

        /// <summary>
        /// Récupère la moyenne de concentration de quanta durant la durée d'un évènement.
        /// </summary>
        /// <returns>Moyenne de concentration de quanta durant une période T.</returns>
        private double GetAverageQuantaConcentration(double netEmissionRate, double eventDuration)
        {
            double avgQuantaConcentration = Math.Abs(netEmissionRate) / 
                                            Math.Abs(SumFirstOrderLossRate) / 
                                            volume * 
                                            ( 1 - (1 / Math.Abs(SumFirstOrderLossRate) / eventDuration) * (1 - Math.Exp(-SumFirstOrderLossRate * eventDuration)));

            return avgQuantaConcentration.SetValueIfNaN();
        }

        /// <summary>
        /// Calcul la moyenne de quantas inhalé par personne durant une période en prenant en compte les masques et leur efficacité.
        /// </summary>
        /// <returns>Moyenne de quantas inhalé par personnes.</returns>
        private double GetQuantaInhaledPerPerson(double avgQuantaConcentration, double eventDuration, double inhalationMaskEfficiency, double fractionPersonsWithMask)
        {
            double quantaInhaledPerPerson = avgQuantaConcentration * Math.Abs(SumFirstOrderLossRate) * eventDuration * (1 - inhalationMaskEfficiency * fractionPersonsWithMask);
            return quantaInhaledPerPerson.SetValueIfNaN();
        }
        
        #endregion
    }
}

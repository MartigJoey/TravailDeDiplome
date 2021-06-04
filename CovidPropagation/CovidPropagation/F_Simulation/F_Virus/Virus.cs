/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Diagnostics;

namespace CovidPropagation
{
    /// <summary>
    /// ID Documentation : Virus_Class
    /// Class contenant les données du virus qui est utilisé pour simuler le/la covid.
    /// </summary>
    public static class Virus
    {
        private static List<Symptom> commonSymptoms;
        private static List<Symptom> rareSymptoms;
        private static List<Transmission> transmissions;

        private static int _incubationDurationMin;
        private static int _incubationDurationMax;
        private static int _durationMin; // En jours      
        private static int _durationMax; // En jours                 
        private static double _hospitalRate;
        private static double _deathRate;

        private static int _immunityDurationMin; // En jours
        private static int _immunityDurationMax; // En jours
        private static double _immunityEfficiency;

        private static double _decayRateOfVirus;
        private static double _depositionOnSurfaceRate;

        public static int IncubationDurationMin { get => _incubationDurationMin; }
        public static int IncubationDurationMax { get => _incubationDurationMax; }
        public static int IncubationDuration { get => GlobalVariables.rdm.NextInclusive(_incubationDurationMin, _incubationDurationMax); }
        public static int DurationMin { get => _durationMin; }
        public static int DurationMax { get => _durationMax; }
        public static int Duration { get => GlobalVariables.rdm.NextInclusive(_durationMin, _durationMax); }
        public static double HospitalRate { get => _hospitalRate; }
        public static double DeathRate { get => _deathRate; }
        public static double DecayRateOfVirus { get => _decayRateOfVirus; }
        public static double DepositionOnSurfaceRate { get => _depositionOnSurfaceRate; }
        public static int ImmunityDurationMin { get => _immunityDurationMin; set => _immunityDurationMin = value; }
        public static int ImmunityDurationMax { get => _immunityDurationMax; set => _immunityDurationMax = value; }
        public static double ImmunityEfficiency { get => _immunityEfficiency; set => _immunityEfficiency = value; }

        /// <summary>
        /// Récupère les données se trouvant dans le fichier XML contenant les données du covid et intialise le virus.
        /// </summary>
        public static void Init()
        {
            XmlDocument xmlDatas = new XmlDocument();
            xmlDatas.Load("./CovidData.xml");

            XmlNodeList transmissionsNode = xmlDatas.GetElementsByTagName("Transmissions");
            XmlNodeList symptomsNode = xmlDatas.GetElementsByTagName("Symptoms");

            transmissions = new List<Transmission>();
            commonSymptoms = new List<Symptom>();
            rareSymptoms = new List<Symptom>();

            _incubationDurationMin = VirusParameters.IncubationDurationMin;
            _incubationDurationMax = VirusParameters.IncubationDurationMax;
            _durationMin = VirusParameters.DurationMin;
            _durationMax = VirusParameters.DurationMax;
            _hospitalRate = VirusParameters.HospitalRate;
            _deathRate = VirusParameters.DeathRate;
            _decayRateOfVirus = VirusParameters.DecayRateOfVirus;
            _depositionOnSurfaceRate = VirusParameters.DepositionOnSurfaceRate;
            _immunityDurationMin = VirusParameters.ImmunityDurationMin;
            _immunityDurationMax = VirusParameters.ImmunityDurationMax;
            _immunityEfficiency = VirusParameters.ImmunityEfficiency;

            // Parcours les moyens de transmissions et les ajoutent au virus.
            for (int i = 0; i < transmissionsNode.Count; i++)
            {
                for (int j = 0; j < transmissionsNode[i].ChildNodes.Count; j++)
                {
                    if (transmissionsNode[i].ChildNodes[j].Name == "Aerosol" && VirusParameters.IsAerosolTransmissionActive)
                    {
                        transmissions.Add(new AerosolTransmission());
                    }
                }
            }

            // Parcours les symptômes et les ajoutent au virus.
            for (int i = 0; i < symptomsNode.Count; i++)
            {
                for (int j = 0; j < symptomsNode[i].ChildNodes.Count; j++)
                {
                    if (symptomsNode[i].ChildNodes[j].Name == "Cough" && VirusParameters.IsCoughSymptomActive)
                    {
                        int minQanta = 100;
                        int maxQanta = 200;

                        if (symptomsNode[i].ChildNodes[j].ChildNodes.Count >= 1 && VirusParameters.CoughMinQuanta < 0)
                            minQanta = Convert.ToInt32(symptomsNode[i].ChildNodes[j].ChildNodes[0].InnerText);
                        else if(VirusParameters.CoughMinQuanta > 0)
                            minQanta = VirusParameters.CoughMinQuanta;

                        if (symptomsNode[i].ChildNodes[j].ChildNodes.Count >= 2 && VirusParameters.CoughMaxQuanta < 0)
                            maxQanta = Convert.ToInt32(symptomsNode[i].ChildNodes[j].ChildNodes[1].InnerText);
                        else if(VirusParameters.CoughMaxQuanta > 0)
                            maxQanta = VirusParameters.CoughMaxQuanta;

                        commonSymptoms.Add(new CoughSymptom(minQanta, maxQanta));
                    }
                }
            }
        }

        /// <summary>
        /// Définit si le virus est transmissible par le type de transmission demandée.
        /// </summary>
        /// <param name="typeOfTransmission">Type de transmission à vérifier.</param>
        /// <returns>Si le virus est transmissible par le type demandé ou non.</returns>
        public static bool IsTransmissibleBy(Type typeOfTransmission)
        {
            bool result = false;
            if (transmissions.Where(t => t.GetType() == typeOfTransmission).Any())
                result = true;

            return result;
        }

        /// <summary>
        /// Récupère un certain type de transmission du virus.
        /// </summary>
        /// <param name="typeOfTransmission">Le type de transmission à récupérer.</param>
        /// <returns>Objet du type demandé.</returns>
        public static Transmission GetTransmission(Type typeOfTransmission)
        {
            return transmissions.Where(t => t.GetType() == typeOfTransmission).First();
        }

        /// <summary>
        ///  Récupère la liste de symptômes du virus
        /// </summary>
        /// <returns>Liste de symptômes</returns>
        public static List<Symptom> GetCommonSymptoms(){
            return commonSymptoms;
        }
    }
}
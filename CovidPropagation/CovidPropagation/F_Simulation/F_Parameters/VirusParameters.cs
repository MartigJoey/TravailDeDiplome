/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */

using System;
using System.Xml;

namespace CovidPropagation 
{ 
    /// <summary>
    /// Classe contenant les paramètres du virus.
    /// </summary>
    public static class VirusParameters
    {
        private const int DEFAULT_MIN_QUANTA = 100;
        private const int DEFAULT_MAX_QUANTA = 200;
        private const string INCUBATION_DURATION_MIN_NAME = "IncubationDurationMin";
        private const string INCUBATION_DURATION_MAX_NAME = "IncubationDurationMax";

        private const string DURATION_MIN_NAME = "DurationMin";
        private const string DURATION_MAX_NAME = "DurationMax";

        private const string DECAY_RATE_OF_VIRUS_NAME = "DecayRateOfVirus";
        private const string DEPOSITION_ON_SURFACE_RATE_NAME = "DepositionOnSurfaceRate";


        private const string IMMUNITY_DURATION_MIN_NAME = "ImmunityDurationMin";
        private const string IMMUNITY_DURATION_MAX_NAME = "ImmunityDurationMax";
        private const string IMMUNITY_EFFICIENCY_NAME = "ImmunityEfficiency";

        /// <summary>
        /// Initialise les variables de la class en allant chercher les valeurs dans un fichier XML.
        /// </summary>
        public static void Init()
        {
            XmlDocument xmlDatas = new XmlDocument();
            xmlDatas.Load("./CovidData.xml");

            IncubationDurationMin = Convert.ToInt32(xmlDatas.GetElementsByTagName(INCUBATION_DURATION_MIN_NAME)[0].InnerText);
            IncubationDurationMax = Convert.ToInt32(xmlDatas.GetElementsByTagName(INCUBATION_DURATION_MAX_NAME)[0].InnerText);
            DurationMin = Convert.ToInt32(xmlDatas.GetElementsByTagName(DURATION_MIN_NAME)[0].InnerText);
            DurationMax = Convert.ToInt32(xmlDatas.GetElementsByTagName(DURATION_MAX_NAME)[0].InnerText);
            DecayRateOfVirus = Convert.ToDouble(xmlDatas.GetElementsByTagName(DECAY_RATE_OF_VIRUS_NAME)[0].InnerText);
            DepositionOnSurfaceRate = Convert.ToDouble(xmlDatas.GetElementsByTagName(DEPOSITION_ON_SURFACE_RATE_NAME)[0].InnerText);
            ImmunityDurationMin = Convert.ToInt32(xmlDatas.GetElementsByTagName(IMMUNITY_DURATION_MIN_NAME)[0].InnerText);
            ImmunityDurationMax = Convert.ToInt32(xmlDatas.GetElementsByTagName(IMMUNITY_DURATION_MAX_NAME)[0].InnerText);
            ImmunityEfficiency = Convert.ToDouble(xmlDatas.GetElementsByTagName(IMMUNITY_EFFICIENCY_NAME)[0].InnerText);
            CoughMinQuanta = DEFAULT_MIN_QUANTA;
            CoughMaxQuanta = DEFAULT_MAX_QUANTA;
            IsCoughSymptomActive = true;
            IsAerosolTransmissionActive = true;
        }
        // Virus
        public static int IncubationDurationMin { get; set; }
        public static int IncubationDurationMax { get; set; }
        public static int DurationMin { get; set; }
        public static int DurationMax { get; set; }
        public static double DecayRateOfVirus { get; set; }
        public static double DepositionOnSurfaceRate { get; set; }
        public static int ImmunityDurationMin { get; set; }
        public static int ImmunityDurationMax { get; set; }
        public static double ImmunityEfficiency { get; set; }

        // Symptoms
        public static int CoughMaxQuanta { get; set; }
        public static int CoughMinQuanta { get; set; }
        public static bool IsCoughSymptomActive { get; set; }

        // Transmission
        public static bool IsAerosolTransmissionActive { get; set; }
    }
}

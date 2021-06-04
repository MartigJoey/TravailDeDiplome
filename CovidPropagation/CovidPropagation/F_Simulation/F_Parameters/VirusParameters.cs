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
    public static class VirusParameters
    {
        private const int DEFAULT_MIN_QUANTA = 100;
        private const int DEFAULT_MAX_QUANTA = 200;
        public static void Init()
        {
            XmlDocument xmlDatas = new XmlDocument();
            xmlDatas.Load("./CovidData.xml");

            IncubationDurationMin = Convert.ToInt32(xmlDatas.GetElementsByTagName("IncubationDurationMin")[0].InnerText);
            IncubationDurationMax = Convert.ToInt32(xmlDatas.GetElementsByTagName("IncubationDurationMax")[0].InnerText);
            DurationMin = Convert.ToInt32(xmlDatas.GetElementsByTagName("DurationMin")[0].InnerText);
            DurationMax = Convert.ToInt32(xmlDatas.GetElementsByTagName("DurationMax")[0].InnerText);
            HospitalRate = Convert.ToDouble(xmlDatas.GetElementsByTagName("HospitalRate")[0].InnerText);
            DeathRate = Convert.ToDouble(xmlDatas.GetElementsByTagName("DeathRate")[0].InnerText);
            DecayRateOfVirus = Convert.ToDouble(xmlDatas.GetElementsByTagName("DecayRateOfVirus")[0].InnerText);
            DepositionOnSurfaceRate = Convert.ToDouble(xmlDatas.GetElementsByTagName("DepositionOnSurfaceRate")[0].InnerText);
            ImmunityDurationMin = Convert.ToInt32(xmlDatas.GetElementsByTagName("ImmunityDurationMin")[0].InnerText);
            ImmunityDurationMax = Convert.ToInt32(xmlDatas.GetElementsByTagName("ImmunityDurationMax")[0].InnerText);
            ImmunityEfficiency = Convert.ToDouble(xmlDatas.GetElementsByTagName("ImmunityEfficiency")[0].InnerText);
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
        public static double HospitalRate { get; set; }
        public static double DeathRate { get; set; }
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

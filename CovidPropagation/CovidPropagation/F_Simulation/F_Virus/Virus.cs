using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Linq;
using System.Xml.Linq;

namespace CovidPropagation
{
    public static class Virus
    {
        private static string name;
        private static List<Symptom> commonSymptoms;
        private static List<Symptom> rareSymptoms;
        private static List<Transmission> transmissions;

        private static int _incubationDurationMin;
        private static int _incubationDurationMax;
        private static double _incubationDurationMedian;
        private static int _duration; // En jours               
        private static double _hospitalRate;
        private static double _deathRate;

        private static double _decayRateOfVirus;
        private static double _depositionOnSurfaceRate;

        public static int IncubationDurationMin { get => _incubationDurationMin; }
        public static int IncubationDurationMax { get => _incubationDurationMax; }
        public static double IncubationDurationMedian { get => _incubationDurationMedian; }
        public static int Duration { get => _duration; }
        public static double HospitalRate { get => _hospitalRate; }
        public static double DeathRate { get => _deathRate; }
        public static double DecayRateOfVirus { get => _decayRateOfVirus; }
        public static double DepositionOnSurfaceRate { get => _depositionOnSurfaceRate; }

        public static void Init()
        {
            XmlReader reader = XmlReader.Create("./CovidData.xml");
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name.ToString())
                    {
                        case "Name":
                            name = reader.ReadString();
                            break;
                        case "IncubationDurationMin":
                            _incubationDurationMin = Convert.ToInt32(reader.ReadString());
                            break;
                        case "IncubationDurationMax":
                            _incubationDurationMax = Convert.ToInt32(reader.ReadString());
                            break;
                        case "IncubationDurationMedian":
                            _incubationDurationMedian = Convert.ToDouble(reader.ReadString());
                            break;
                        case "Duration":
                            _duration = Convert.ToInt32(reader.ReadString());
                            break;
                        case "HospitalRate":
                            _hospitalRate = Convert.ToDouble(reader.ReadString());
                            break;
                        case "DeathRate":
                            _deathRate = Convert.ToDouble(reader.ReadString());
                            break;
                        case "DecayRateOfVirus":
                            _decayRateOfVirus = Convert.ToDouble(reader.ReadString());
                            break;
                        case "DepositionOnSurfaceRate":
                            _depositionOnSurfaceRate = Convert.ToDouble(reader.ReadString());
                            break;
                        case "Transmissions":
                            Debug.WriteLine(reader.Name);
                            break;
                        case "Symptom":
                        case "Transmission":
                            Debug.WriteLine(reader.Name + " " + reader.ReadString());
                            break;
                    }
                }
            }
            transmissions = new List<Transmission>();
            commonSymptoms = new List<Symptom>();
            rareSymptoms = new List<Symptom>();

            commonSymptoms.Add(new CoughSymptom());
            transmissions.Add(new AerosolTransmission());
        }

        public static bool IsTransmissibleBy(Type typeOfTransmission)
        {
            bool result = false;
            if (transmissions.Where(t => t.GetType() == typeOfTransmission).Any())
                result = true;

            return result;
        }

        public static Transmission GetTransmission(Type typeOfTransmission)
        {
            return transmissions.Where(t => t.GetType() == typeOfTransmission).First();
        }

        public static List<Symptom> GetCommonSymptoms(){
            return commonSymptoms;
        }
    }
}
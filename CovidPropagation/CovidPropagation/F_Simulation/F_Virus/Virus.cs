using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Linq;

namespace CovidPropagation
{
    public static class Virus
    {
        private static string name;
        private static List<Symptom> commonSymptoms;
        private static List<Symptom> rareSymptoms;
        private static List<Transmission> transmissions;
        public static void Init()
        {
            using (XmlReader reader = XmlReader.Create(@"./CovidData.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name.ToString())
                        {
                            case "Name":
                                name = reader.ReadString();
                                break;
                            case "Symptoms":
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
            }
            Debug.WriteLine(name);
            transmissions = new List<Transmission>();
            transmissions.Add(new Transmission());
        }

        public static bool IsTransmissibleBy(Type typeOfTransmission)
        {
            bool result = false;
            if (transmissions.Where(t => t.GetType() == typeOfTransmission).Any())
                result = true;

            return result;
        }
    }
}
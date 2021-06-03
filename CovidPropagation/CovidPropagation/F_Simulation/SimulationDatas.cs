/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public struct SimulationDatas
    {
        public void Initialize()
        {
            CentralizedDatas = new List<KeyValuePair<string, List<double>>>();
            NumberOfPeople = new List<double>();
            NumberOfInfected = new List<double>();
            NumberOfImmune = new List<double>();
            NumberOfReproduction = new List<double>();
            NumberOfHealthy = new List<double>();
            NumberOfHospitalisation = new List<double>();
            NumberOfDeath = new List<double>();
            NumberOfContamination = new List<double>();

            CentralizedDatas.Add(new KeyValuePair<string, List<double>>("Nombre de personnes", NumberOfPeople));
            CentralizedDatas.Add(new KeyValuePair<string, List<double>>("Nombre d'infecté", NumberOfInfected));
            CentralizedDatas.Add(new KeyValuePair<string, List<double>>("Nombre d'immunisé", NumberOfImmune));
            CentralizedDatas.Add(new KeyValuePair<string, List<double>>("Nombre de reproductions", NumberOfReproduction));
            CentralizedDatas.Add(new KeyValuePair<string, List<double>>("Nombre de sains", NumberOfHealthy));
            CentralizedDatas.Add(new KeyValuePair<string, List<double>>("Nombre d'hospitalisation", NumberOfHospitalisation));
            CentralizedDatas.Add(new KeyValuePair<string, List<double>>("Nombre de décès", NumberOfDeath));
            CentralizedDatas.Add(new KeyValuePair<string, List<double>>("Nombre de contamination", NumberOfContamination));
        }
        public List<KeyValuePair<string, List<double>>> CentralizedDatas { get; set; }

        public List<double> NumberOfPeople { get; set; }

        public List<double> NumberOfInfected { get; set; }

        public List<double> NumberOfImmune { get; set; }

        public List<double> NumberOfReproduction { get; set; }

        public List<double> NumberOfHealthy { get; set; }

        public List<double> NumberOfHospitalisation { get; set; }

        public List<double> NumberOfDeath { get; set; }

        public List<double> NumberOfContamination { get; set; }

        /// <summary>
        /// Ajoute des données à la structure.
        /// </summary>
        /// <param name="newDatas">Données à ajouter à la structure provenant d'une autre structure.</param>
        public void AddDatas(SimulationDatas newDatas)
        {
            NumberOfPeople.AddRange(newDatas.NumberOfPeople);
            NumberOfInfected.AddRange(newDatas.NumberOfInfected);
            NumberOfImmune.AddRange(newDatas.NumberOfImmune);
            NumberOfReproduction.AddRange(newDatas.NumberOfReproduction);
            NumberOfHealthy.AddRange(newDatas.NumberOfHealthy);
            NumberOfHospitalisation.AddRange(newDatas.NumberOfHospitalisation);
            NumberOfDeath.AddRange(newDatas.NumberOfDeath);
            NumberOfContamination.AddRange(newDatas.NumberOfContamination);
        }

        /// <summary>
        /// Récupère la liste correspondant a la valeur du enum.
        /// </summary>
        /// <param name="enumData">Valeurs à récupérer.</param>
        /// <returns>Valeurs correspondants aux valeurs à récupérer.</returns>
        public List<double> GetDataFromEnum(ChartsDisplayData enumData)
        {
            List<double> result;
            switch (enumData)
            {
                default:
                case ChartsDisplayData.Persons:
                    result = NumberOfPeople;
                    break;
                case ChartsDisplayData.Cases:
                    result = NumberOfInfected;
                    break;
                case ChartsDisplayData.Immune:
                    result = NumberOfImmune;
                    break;
                case ChartsDisplayData.Re:
                    result = NumberOfReproduction;
                    break;
                case ChartsDisplayData.Healthy:
                    result = NumberOfHealthy;
                    break;
                case ChartsDisplayData.Hospitalisation:
                    result = NumberOfHospitalisation;
                    break;
                case ChartsDisplayData.Death:
                    result = NumberOfDeath;
                    break;
                case ChartsDisplayData.Contamination:
                    result = NumberOfContamination;
                    break;
            }
            if (result == null)
                result = new List<double>();
            return result;
        }
    }
}

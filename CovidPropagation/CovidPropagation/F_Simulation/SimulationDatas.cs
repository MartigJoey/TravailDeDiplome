using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public struct SimulationDatas
    {
        public int NumberOfPeople { get; set; }

        public int NumberOfInfected { get; set; }

        public int NumberOfImmune { get; set; }

        public double NumberOfReproduction { get; set; }

        public int NumberOfHealthy { get; set; }

        public int NumberOfHospitalisation { get; set; }

        public int NumberOfDeath { get; set; }

        public int NumberOfContamination { get; set; }

        public void AddDatas(SimulationDatas newDatas)
        {
            NumberOfPeople = newDatas.NumberOfPeople;
            NumberOfInfected = newDatas.NumberOfInfected;
            NumberOfImmune = newDatas.NumberOfImmune;
            NumberOfReproduction = newDatas.NumberOfReproduction;
            NumberOfHealthy = newDatas.NumberOfHealthy;
            NumberOfHospitalisation = newDatas.NumberOfHospitalisation;
            NumberOfDeath = newDatas.NumberOfDeath;
            NumberOfContamination = newDatas.NumberOfContamination;
        }

        public double GetDataFromEnum(ChartsDisplayData enumData)
        {
            double result;
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
            return result;
        }
    }
}

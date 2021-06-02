using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public class DataPopulation
    {
        private int nbPersons;
        private int[] indexOfInfected;

        public int NbPersons { get => nbPersons; set => nbPersons = value; }
        public int[] IndexOfInfected { get => indexOfInfected; set => indexOfInfected = value; }

        public DataPopulation(int nbPersons, int[] indexInfected)
        {
            NbPersons = nbPersons;
            IndexOfInfected = indexInfected;
        }
    }
}

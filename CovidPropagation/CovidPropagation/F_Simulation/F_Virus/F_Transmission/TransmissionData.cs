using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public class TransmissionData
    {
        double probabilityOfOneInfection;
        double probabilityOfInfection;
        double nbOfInfectivePersons;
        double probabilityOfHospitalisation;
        double probabilityOfDeath;
        double virusAraisingCases;

        public double ProbabilityOfOneInfection { get => probabilityOfOneInfection; set => probabilityOfOneInfection = value; }
        public double ProbabilityOfInfection { get => probabilityOfInfection; set => probabilityOfInfection = value; }
        public double NOfInfectivePersons { get => nbOfInfectivePersons; set => nbOfInfectivePersons = value; }
        public double ProbabilityOfHospitalisation { get => probabilityOfHospitalisation; set => probabilityOfHospitalisation = value; }
        public double ProbabilityOfDeath { get => probabilityOfDeath; set => probabilityOfDeath = value; }
        public double VirusAraisingCases { get => virusAraisingCases; set => virusAraisingCases = value; }

        public TransmissionData(
                double probabilityOfOneInfection,
                double probabilityOfInfection,
                double nbOfInfectivePersons,
                double probabilityOfHospitalisation,
                double probabilityOfDeath,
                double virusAraisingCases)
        {

            ProbabilityOfOneInfection = probabilityOfOneInfection;
            ProbabilityOfInfection = probabilityOfInfection;
            NOfInfectivePersons = nbOfInfectivePersons;
            ProbabilityOfHospitalisation = probabilityOfHospitalisation;
            ProbabilityOfDeath = probabilityOfDeath;
            VirusAraisingCases = virusAraisingCases;
        }

    }
}

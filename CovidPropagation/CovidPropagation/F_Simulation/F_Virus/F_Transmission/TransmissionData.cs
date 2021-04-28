using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public class TransmissionData
    {

        double avgQuantaConcentration;
        double quantaInhaledPerPerson;
        double probabilityOfOneInfection;
        double probabilityOfInfection;
        double nbOfInfectivePersons;
        double probabilityOfHospitalisation;
        double probabilityOfDeath;


        public double AvgQuantaConcentration { get => avgQuantaConcentration; set => avgQuantaConcentration = value; }
        public double QuantaInhaledPerPerson { get => quantaInhaledPerPerson; set => quantaInhaledPerPerson = value; }
        public double ProbabilityOfOneInfection { get => probabilityOfOneInfection; set => probabilityOfOneInfection = value; }
        public double ProbabilityOfInfection { get => probabilityOfInfection; set => probabilityOfInfection = value; }
        public double NbOfInfectivePersons { get => nbOfInfectivePersons; set => nbOfInfectivePersons = value; }
        public double ProbabilityOfHospitalisation { get => probabilityOfHospitalisation; set => probabilityOfHospitalisation = value; }
        public double ProbabilityOfDeath { get => probabilityOfDeath; set => probabilityOfDeath = value; }
        public TransmissionData(
                double avgQuantaConcentration,
                double quantaInhaledPerPerson,
                double probabilityOfOneInfection,
                double probabilityOfInfection,
                double nbOfInfectivePersons,
                double probabilityOfHospitalisation,
                double probabilityOfDeath)
        {

            AvgQuantaConcentration = avgQuantaConcentration;
            QuantaInhaledPerPerson = quantaInhaledPerPerson;
            ProbabilityOfOneInfection = probabilityOfOneInfection;
            ProbabilityOfInfection = probabilityOfInfection;
            NbOfInfectivePersons = nbOfInfectivePersons;
            ProbabilityOfHospitalisation = probabilityOfHospitalisation;
            ProbabilityOfDeath = probabilityOfDeath;
        }

    }
}

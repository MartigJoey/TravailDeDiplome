/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
namespace CovidPropagation
{
    /// <summary>
    /// Classe utilisée dans le transfère de données de la transmission du virus.
    /// </summary>
    public struct AerosolTransmissionData
    {
        public double ProbabilityOfOneInfection { get; set; }
        public double ProbabilityOfInfection { get; set; }
        public double NOfInfectiousPersons { get; set; }
        public double VirusAraisingCases { get; set; }

        public AerosolTransmissionData(
                double probabilityOfOneInfection,
                double probabilityOfInfection,
                double nbOfInfectivePersons,
                double virusAraisingCases)
        {

            ProbabilityOfOneInfection = probabilityOfOneInfection;
            ProbabilityOfInfection = probabilityOfInfection;
            NOfInfectiousPersons = nbOfInfectivePersons;
            VirusAraisingCases = virusAraisingCases;
        }
    }
}

/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */

namespace CovidPropagation
{
    static class QuarantineParameters
    {
        private const double DEFAULT_PROBABILITY_OF_BEING_QUARANTINED = 0.95;
        private const int DEFAULT_DURATION_OF_BEING_QUARANTINED = 14;
        public static void Init()
        {
            IshealthyQuarantined = false;
            IsInfectedQuarantined = false;
            IsInfectiousQuarantined = false;
            IsImmuneQuarantined = false;

            ProbabilityOfHealthyQuarantined = DEFAULT_PROBABILITY_OF_BEING_QUARANTINED;
            ProbabilityOfInfectedQuarantined = DEFAULT_PROBABILITY_OF_BEING_QUARANTINED;
            ProbabilityOfInfectiousQuarantined = DEFAULT_PROBABILITY_OF_BEING_QUARANTINED;
            ProbabilityOfImmuneQuarantined = DEFAULT_PROBABILITY_OF_BEING_QUARANTINED;

            DurationHealthyQuarantined = DEFAULT_DURATION_OF_BEING_QUARANTINED;
            DurationInfectedQuarantined = DEFAULT_DURATION_OF_BEING_QUARANTINED;
            DurationInfectiousQuarantined = DEFAULT_DURATION_OF_BEING_QUARANTINED;
            DurationImmuneQuarantined = DEFAULT_DURATION_OF_BEING_QUARANTINED;
        }
        // Paramètres généraux
        public static bool IshealthyQuarantined { get; set; }
        public static bool IsInfectedQuarantined { get; set; }
        public static bool IsInfectiousQuarantined { get; set; }
        public static bool IsImmuneQuarantined { get; set; }

        public static double ProbabilityOfHealthyQuarantined { get; set; }
        public static double ProbabilityOfInfectedQuarantined { get; set; }
        public static double ProbabilityOfInfectiousQuarantined { get; set; }
        public static double ProbabilityOfImmuneQuarantined { get; set; }

        public static int DurationHealthyQuarantined { get; set; }
        public static int DurationInfectedQuarantined { get; set; }
        public static int DurationInfectiousQuarantined { get; set; }
        public static int DurationImmuneQuarantined { get; set; }
    }
}

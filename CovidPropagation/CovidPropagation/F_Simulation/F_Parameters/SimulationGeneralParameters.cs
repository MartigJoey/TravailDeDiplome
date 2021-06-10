/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */

namespace CovidPropagation
{
    public static class SimulationGeneralParameters
    {
        private const int DEFAULT_NUMBER_OF_PEOPLE = 100000;
        private const float DEFAULT_PROBABILITY_OF_BEING_INFECTED = 0.01f;

        private const int DEFAULT_MIN_TRIGGER = 1000;
        public static void Init()
        {
            NbPeople = DEFAULT_NUMBER_OF_PEOPLE;
            ProbabilityOfInfected = DEFAULT_PROBABILITY_OF_BEING_INFECTED;

            IsMaskMeasuresEnabled = false;
            IsDistanciationMeasuresEnabled = false;
            IsQuarantineMeasuresEnabled = false;
            IsVaccinationMeasuresEnabled = false;

            NbInfecetdForMaskActivation = DEFAULT_MIN_TRIGGER;
            NbInfecetdForMaskDeactivation = DEFAULT_MIN_TRIGGER;
            NbInfecetdForDistanciationActivation = DEFAULT_MIN_TRIGGER;
            NbInfecetdForDistanciationDeactivation = DEFAULT_MIN_TRIGGER;
            NbInfecetdForQuarantineActivation = DEFAULT_MIN_TRIGGER;
            NbInfecetdForQuarantineDeactivation = DEFAULT_MIN_TRIGGER;
            NbInfecetdForVaccinationActivation = DEFAULT_MIN_TRIGGER;
            NbInfecetdForVaccinationDeactivation = DEFAULT_MIN_TRIGGER;
        }
        // Paramètres généraux
        public static int NbPeople { get; set; }
        public static float ProbabilityOfInfected { get; set; }

        // Si les mesures son activées
        public static bool IsMaskMeasuresEnabled { get; set; }
        public static bool IsDistanciationMeasuresEnabled { get; set; }
        public static bool IsQuarantineMeasuresEnabled { get; set; }
        public static bool IsVaccinationMeasuresEnabled { get; set; }

        // Trigger des mesures
        public static int NbInfecetdForMaskActivation { get; set; }
        public static int NbInfecetdForMaskDeactivation { get; set; }
        public static int NbInfecetdForDistanciationActivation { get; set; }
        public static int NbInfecetdForDistanciationDeactivation { get; set; }
        public static int NbInfecetdForQuarantineActivation { get; set; }
        public static int NbInfecetdForQuarantineDeactivation { get; set; }
        public static int NbInfecetdForVaccinationActivation { get; set; }
        public static int NbInfecetdForVaccinationDeactivation { get; set; }
    }
}

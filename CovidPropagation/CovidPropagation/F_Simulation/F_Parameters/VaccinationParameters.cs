/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */

namespace CovidPropagation
{
    /// <summary>
    /// Classe contenant les paramètres de la vaccination.
    /// </summary>
    class VaccinationParameters
    {
        private const int VACCIN_DURATION = 30 * 6;
        private const double VACCIN_EFFICIENCY = 0.95;
        public static void Init()
        {
            Duration = VACCIN_DURATION;
            Efficiency = VACCIN_EFFICIENCY;
        }

        public static int Duration { get; set; }
        public static double Efficiency { get; set; }
    }
}

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
    class VaccinationParameters
    {
        public static void Init()
        {
            Duration = 30 * 6;
            Efficiency = 95;
        }
        // Paramètres généraux
        public static int Duration { get; set; }
        public static int Efficiency { get; set; }
    }
}

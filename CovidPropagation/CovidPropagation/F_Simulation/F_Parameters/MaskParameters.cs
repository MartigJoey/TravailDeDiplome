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
    class MaskParameters
    {
        public static void Init()
        {
            IsClientMaskOn = false;
            IsPersonnelMaskOn = false;
        }
        // Paramètres généraux
        public static bool IsClientMaskOn { get; set; }
        public static bool IsPersonnelMaskOn { get; set; }
    }
}

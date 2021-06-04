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
    class QuarantineParameters
    {
        public static void Init()
        {
            IshealthyQuarantined = false;
            IsInfectedQuarantined = false;
            IsInfectiousQuarantined = false;
            IsImmuneQuarantined = false;
        }
        // Paramètres généraux
        public static bool IshealthyQuarantined { get; set; }
        public static bool IsInfectedQuarantined { get; set; }
        public static bool IsInfectiousQuarantined { get; set; }
        public static bool IsImmuneQuarantined { get; set; }
    }
}

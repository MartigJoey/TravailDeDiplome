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
            Efficacity = 95;
        }
        // Paramètres généraux
        public static int Duration { get; set; }
        public static int Efficacity { get; set; }
    }
}

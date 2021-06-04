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

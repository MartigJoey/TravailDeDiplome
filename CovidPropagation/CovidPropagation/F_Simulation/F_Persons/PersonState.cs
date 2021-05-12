using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    /// <summary>
    /// Personne propageant le virus.
    /// </summary>
    public enum PersonState
    {
        Dead = -1,
        Healthy = 0,
        Immune = 1,
        Infected = 2,
        Infectious = 3,
        Asymptomatic = 4
    }
}

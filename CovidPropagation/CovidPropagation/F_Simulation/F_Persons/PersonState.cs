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
    /// État des individus.
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

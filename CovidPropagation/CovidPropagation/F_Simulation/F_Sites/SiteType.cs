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
    /// Permet de catégoriser un lieu.
    /// </summary>
    public enum SiteType
    {
        Home,
        Store,
        Eat,
        Transport,
        Hospital,
        WorkPlace,
        School
    }
}

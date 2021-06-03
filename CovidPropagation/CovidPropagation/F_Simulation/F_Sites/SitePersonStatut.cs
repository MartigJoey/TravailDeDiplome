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
    /// Raison pour un individu de se déplacer dans un lieu.
    /// Permet de lui appliquer différentes mesures.
    /// </summary>
    public enum SitePersonStatus
    {
        Client,
        Worker,
        Other
    }
}

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
    /// Données des sites qui seront transmisent par pipeline au GUI.
    /// </summary>
    public class DataSites
    {
        private int[] sitesType; // Le type de chaque site.
        private int[] sitesId; // L'id unique de chaque site.

        public int[] SitesType { get => sitesType; set => sitesType = value; }
        public int[] SitesId { get => sitesId; set => sitesId = value; }

        public DataSites(int[] types, int[] ids)
        {
            SitesType = types;
            SitesId = ids;
        }
    }
}

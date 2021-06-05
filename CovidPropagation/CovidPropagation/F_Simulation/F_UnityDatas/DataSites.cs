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
        private int _nbHouse;
        private int _nbCompany;
        private int _nbHospital;
        private int _nbRestaurant;
        private int _nbSchool;
        private int _nbStore;
        private int _nbSupermarket;

        public int NbHouse { get => _nbHouse; set => _nbHouse = value; }
        public int NbCompany { get => _nbCompany; set => _nbCompany = value; }
        public int NbHospital { get => _nbHospital; set => _nbHospital = value; }
        public int NbRestaurant { get => _nbRestaurant; set => _nbRestaurant = value; }
        public int NbSchool { get => _nbSchool; set => _nbSchool = value; }
        public int NbStore { get => _nbStore; set => _nbStore = value; }
        public int NbSupermarket { get => _nbSupermarket; set => _nbSupermarket = value; }

        public DataSites(int nbHouse, int nbCompany, int nbHospital, int nbRestaurant, int nbSchool, int nbStore, int nbSupermarket)
        {
            _nbHouse = nbHouse;
            _nbCompany = nbCompany;
            _nbHospital = nbHospital;
            _nbRestaurant = nbRestaurant;
            _nbSchool = nbSchool;
            _nbStore = nbStore;
            _nbSupermarket = nbSupermarket;
        }
    }
}

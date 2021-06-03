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
    /// Permet de transférer les données de la population au GUI.
    /// </summary>
    public class DataPopulation
    {
        private int nbPersons; // Le nombre d'individus
        private int[] indexOfInfected; // L'index des individus infectés.

        public int NbPersons { get => nbPersons; set => nbPersons = value; }
        public int[] IndexOfInfected { get => indexOfInfected; set => indexOfInfected = value; }

        public DataPopulation(int nbPersons, int[] indexInfected)
        {
            NbPersons = nbPersons;
            IndexOfInfected = indexInfected;
        }
    }
}

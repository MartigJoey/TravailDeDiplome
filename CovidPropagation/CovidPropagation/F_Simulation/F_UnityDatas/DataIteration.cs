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
    /// Données qui seront envoyé au GUI à chaque itération.
    /// </summary>
    public class DataIteration
    {
        private int[] personsNewSite;
        private int[] personsNewState;

        public int[] PersonsNewSite { get => personsNewSite; set => personsNewSite = value; }
        public int[] PersonsNewState { get => personsNewState; set => personsNewState = value; }

        public DataIteration(int[] personsNewSite, int[] personsNewState)
        {
            this.PersonsNewSite = personsNewSite;
            this.PersonsNewState = personsNewState;
        }
    }
}

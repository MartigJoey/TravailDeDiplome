/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System;

namespace CovidPropagation
{
    /// <summary>
    /// Maladie impactant la résistance au virus d'une personne.
    /// </summary>
    class Ilness
    {
        int timeBeforeDesapearing; // En période
        int attack;
        public int Attack { get => attack; set => attack = value; }
        public Ilness(int age, Random rdm)
        {
            timeBeforeDesapearing = rdm.Next(1, 7) * 48; // entre 7 jours et 30 (Temporaire)
            Attack = CalculateAttack(age, rdm); // Entre 5 et 20, en fonction de l'âge
        }

        /// <summary>
        /// ID Documentation : Ilness_Impact
        /// Calcul à quel point la maladie baisse les défenses d'une personne en fonction de son âge.
        /// </summary>
        /// <param name="age">Age de l'individu.</param>
        /// <param name="rdm">Chances que la maladie soit grave.</param>
        /// <returns>"Puissance" de la maladie.</returns>
        private int CalculateAttack(int age, Random rdm)
        {
            // Entre 5 et 20
            // Plus age est élevé plus c'est haut
            int result;
            if (age < 20)
            {
                result = rdm.Next(5, 5 + 1);
            }
            else if (age < 30)
            {
                result = rdm.Next(5, 8 + 1);
            }
            else if (age < 40)
            {
                result = rdm.Next(5, 12 + 1);
            }
            else if (age < 60)
            {
                result = rdm.Next(5, 15 + 1);
            }
            else if(age < 70)
            {
                result = rdm.Next(5, 18 + 1);
            }
            else
            {
                result = rdm.Next(5, 20 + 1);
            }
            return result;
        }

        /// <summary>
        /// Décrémente le temps avant que la maladie ne disparaisse.
        /// </summary>
        public void DecrementTimeBeforeDesapearance(int decrement)
        {
            timeBeforeDesapearing -= decrement;
        }

        /// <summary>
        /// Informe si la maladie doit disparaitre ou non.
        /// </summary>
        public bool Desapear()
        {
            return timeBeforeDesapearing <= 0;
        }
    }
}

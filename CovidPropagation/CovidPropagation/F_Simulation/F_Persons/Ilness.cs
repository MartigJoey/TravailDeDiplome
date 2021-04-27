/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 27.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    /// <summary>
    /// Maladie impactant la résistance au virus d'une personne.
    /// </summary>
    class Ilness
    {
        int timeBeforeDesapear; // En période
        int attack;
        public int Attack { get => attack; set => attack = value; }
        public Ilness(int age, Random rdm)
        {
            timeBeforeDesapear = rdm.Next(7, 30) * 48; // entre 7 jours et 30 (Temporaire)
            Attack = CalculateAttack(age, rdm); // Entre 5 et 20, en fonction de l'âge
        }

        /// <summary>
        /// TEMPORAIRE. Recherche d'une équation permettant de résoudre ce problème mathématiquement.
        /// Calcul à quel point la maladie baisse les défenses d'une personne en fonction de son âge.
        /// </summary>
        /// <param name="age"></param>
        /// <param name="rdm"></param>
        /// <returns></returns>
        private int CalculateAttack(int age, Random rdm)
        {
            // Entre 5 et 20
            // Plus age est élevé plus c'est haut
            int result;
            if (age < 20)
            {
                if (rdm.Next(0, 100) < 80)
                    result = rdm.Next(5, 10 + 1);
                else
                    result = rdm.Next(5, 20 + 1);
            }
            else if (age < 30)
            {
                if (rdm.Next(0, 100) < 70)
                    result = rdm.Next(5, 10 + 1);
                else
                    result = rdm.Next(5, 20 + 1);
            }
            else if (age < 40)
            {
                if (rdm.Next(0, 100) < 60)
                    result = rdm.Next(5, 10 + 1);
                else
                    result = rdm.Next(5, 20 + 1);
            }
            else if (age < 60)
            {
                if (rdm.Next(0, 100) < 50)
                    result = rdm.Next(5, 10 + 1);
                else
                    result = rdm.Next(5, 20 + 1);
            }
            else if(age < 70)
            {
                if (rdm.Next(0, 100) < 40)
                    result = rdm.Next(5, 10 + 1);
                else
                    result = rdm.Next(5, 20 + 1);
            }
            else
            {
                if (rdm.Next(0, 100) < 30)
                    result = rdm.Next(5, 10 + 1);
                else
                    result = rdm.Next(5, 20 + 1);
            }
            return result;
        }

        /// <summary>
        /// Décrémente le temps avant que la maladie ne disparaisse.
        /// </summary>
        public void DecrementTimeBeforeDesapearance()
        {
            timeBeforeDesapear--;
        }

        /// <summary>
        /// Informe si la maladie doit disparaitre ou non.
        /// </summary>
        public bool Desapear()
        {
            return timeBeforeDesapear <= 0;
        }
    }
}

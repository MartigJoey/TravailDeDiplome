
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
        private const int MIN_TIME_BEFORE_DESAPEARING = 1;
        private const int MAX_TIME_BEFORE_DESAPEARING = 7;

        private const int FIRST_STAGE_AGE = 20;
        private const int FIRST_STAGE_MIN_ATTACK = 3;
        private const int FIRST_STAGE_MAX_ATTACK = 6;

        private const int SECOND_STAGE_AGE = 30;
        private const int SECOND_STAGE_MIN_ATTACK = 5;
        private const int SECOND_STAGE_MAX_ATTACK = 9;

        private const int THIRD_STAGE_AGE = 40;
        private const int THIRD_STAGE_MIN_ATTACK = 5;
        private const int THIRD_STAGE_MAX_ATTACK = 13;

        private const int FOURTH_STAGE_AGE = 60;
        private const int FOURTH_STAGE_MIN_ATTACK = 5;
        private const int FOURTH_STAGE_MAX_ATTACK = 16;

        private const int FIFTH_STAGE_AGE = 70;
        private const int FIFTH_STAGE_MIN_ATTACK = 5;
        private const int FIFTH_STAGE_MAX_ATTACK = 19;

        private const int SIXTH_STAGE_MIN_ATTACK = 5;
        private const int SIXTH_STAGE_MAX_ATTACK = 21;

        int timeBeforeDesapearing; // En période
        int attack;
        public int Attack { get => attack; set => attack = value; }
        public Ilness(int age, Random rdm)
        {
            timeBeforeDesapearing = rdm.Next(MIN_TIME_BEFORE_DESAPEARING, MAX_TIME_BEFORE_DESAPEARING) * GlobalVariables.NUMBER_OF_TIMEFRAME; // entre 7 jours et 30 (Temporaire)
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
            if (age < FIRST_STAGE_AGE)
            {
                result = rdm.Next(FIRST_STAGE_MIN_ATTACK, FIRST_STAGE_MAX_ATTACK);
            }
            else if (age < SECOND_STAGE_AGE)
            {
                result = rdm.Next(SECOND_STAGE_MIN_ATTACK, SECOND_STAGE_MAX_ATTACK);
            }
            else if (age < THIRD_STAGE_AGE)
            {
                result = rdm.Next(THIRD_STAGE_MIN_ATTACK, THIRD_STAGE_MAX_ATTACK);
            }
            else if (age < FOURTH_STAGE_AGE)
            {
                result = rdm.Next(FOURTH_STAGE_MIN_ATTACK, FOURTH_STAGE_MAX_ATTACK);
            }
            else if(age < FIFTH_STAGE_AGE)
            {
                result = rdm.Next(FIFTH_STAGE_MIN_ATTACK, FIFTH_STAGE_MAX_ATTACK);
            }
            else
            {
                result = rdm.Next(SIXTH_STAGE_MIN_ATTACK, SIXTH_STAGE_MAX_ATTACK);
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

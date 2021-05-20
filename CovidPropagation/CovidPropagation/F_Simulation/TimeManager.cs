/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 27.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;

namespace CovidPropagation
{
    static class TimeManager
    {
        private static int currentDay;
        private static int currentTimeFrame;
        private static string[] daysOfWeek = new string[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"};

        public static int CurrentDay { get => currentDay; }
        public static string CurrentDayString { get => daysOfWeek[currentDay]; }
        public static int CurrentTimeFrame { get => currentTimeFrame; }
        public static string CurrentHour { get => GetTime(); }

        public static void Init()
        {
            currentDay = 0;
            currentTimeFrame = 0;
        }

        /// <summary>
        /// Change la période actuelle pour passer à la suivante.
        /// </summary>
        public static void NextTimeFrame()
        {
            // Incrémente les périodes temps qu'elles ne dépassent pas la limite.
            if (currentTimeFrame < GlobalVariables.NUMBER_OF_TIMEFRAME-1)
            {
                currentTimeFrame++;
            }
            else
            {
                // Si elles dépassent la limite, on change de jour et on vérifie la même chose pour eux.
                currentTimeFrame = 0; // Réinitialisation de la période car changement de jour
                if (currentDay < GlobalVariables.NUMBER_OF_DAY-1)
                {
                    currentDay++;
                }
                else
                {
                    currentDay = 0;
                }
            }
        }

        /// <summary>
        /// Récupère la prochaine période sans altérer le temps.
        /// </summary>
        /// <returns>Tableau int contenant le jour et la période du prochain Tick.</returns>
        public static int[] GetNextTimeFrame()
        {
            // Identique à NextTimeFrame sauf qu'on utilise des variables encapsulées.
            int tempCurrentTimeFrame = currentTimeFrame;
            int tempCurrentDay = currentDay;
            if (tempCurrentTimeFrame < GlobalVariables.NUMBER_OF_TIMEFRAME - 1)
            {
                tempCurrentTimeFrame++;
            }
            else
            {
                tempCurrentTimeFrame = 0;
                if (tempCurrentDay < GlobalVariables.NUMBER_OF_DAY - 1)
                {
                    tempCurrentDay++;
                }
                else
                {
                    tempCurrentDay = 0;
                }
            }

            return new int[] { tempCurrentDay, tempCurrentTimeFrame };
        }

        public static bool DoesDayChangeOnThisTimeFrame()
        {
            bool result = false;
            if (currentTimeFrame == 0)
                result = true;

            return result;
        }

        public static bool DoesWeekChangeOnThisTimeFrame()
        {
            bool result = false;
            if (currentTimeFrame == 0 && currentDay == 0)
                result = true;

            return result;
        }

        /// <summary>
        /// Convertit les périodes actuelles en heures. Retourne le résultat en string.
        /// </summary>
        /// <returns>Résultat en string.</returns>
        private static string GetTime()
        {
            float time = currentTimeFrame * GlobalVariables.DURATION_OF_TIMEFRAME / 60f;
            int hours = (int)Math.Truncate(time);
            int minutes = (int)((time - hours) * 60);
            return $"{hours.ToString("00")}:{minutes.ToString("00")}";
        }
    }
}

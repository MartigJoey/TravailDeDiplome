using System;

namespace CovidPropagation.Classes
{
    static class TimeManager
    {
        static private int currentDay;
        static private int currentPeriod;
        static private string[] daysOfWeek = new string[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"};

        public static int CurrentDay { get => currentDay; }
        public static string CurrentDayString { get => daysOfWeek[currentDay]; }
        public static int CurrentPeriod { get => currentPeriod; }
        public static string CurrentHour { get => GetTime(); }

        public static void Init()
        {
            currentDay = 0;
            currentPeriod = 0;
        }

        /// <summary>
        /// Change la période actuelle pour passer à la suivante.
        /// </summary>
        public static void NextPeriod()
        {
            // Incrémente les périodes temps qu'elles ne dépassent pas la limite.
            if (currentPeriod < GlobalVariables.NUMBER_OF_PERIODS-1)
            {
                currentPeriod++;
            }
            else
            {
                // Si elles dépassent la limite, on change de jour et on vérifie la même chose pour eux.
                currentPeriod = 0; // Réinitialisation de la période car changement de jour
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
        public static int[] GetNextPeriod()
        {
            // Identique à NextPeriod sauf qu'on utilise des variables encapsulées.
            int tempCurrentPeriod = currentPeriod;
            int tempCurrentDay = currentDay;
            if (tempCurrentPeriod < GlobalVariables.NUMBER_OF_PERIODS - 1)
            {
                tempCurrentPeriod++;
            }
            else
            {
                tempCurrentPeriod = 0;
                if (tempCurrentDay < GlobalVariables.NUMBER_OF_DAY - 1)
                {
                    tempCurrentDay++;
                }
                else
                {
                    tempCurrentDay = 0;
                }
            }

            return new int[] { tempCurrentDay, tempCurrentPeriod };
        }

        /// <summary>
        /// Convertit les périodes actuelles en heures. Retourne le résultat en string.
        /// </summary>
        /// <returns>Résultat en string.</returns>
        private static string GetTime()
        {
            float time = currentPeriod * GlobalVariables.DURATION_OF_PERIOD / 60f;
            int hours = (int)Math.Truncate(time);
            int minutes = (int)((time - hours) * 60);
            return $"{hours.ToString("00")}:{minutes.ToString("00")}";
        }
    }
}

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
    static class GlobalVariables
    {
        public static readonly Random rdm = new Random();

        public const int DURATION_OF_TIMEFRAME = 30; // En minutes
        public const int MINUTES_PER_DAY = 1440;
        public const int NUMBER_OF_TIMEFRAME = MINUTES_PER_DAY / DURATION_OF_TIMEFRAME;
        public const int NUMBER_OF_DAY = 7;

        public const double ILNESS_INFECTION_PROBABILITY = 0.01d;

        public const int DEFAULT_INTERVAL = 500;

        // Symptomatic / Asymptomatic
        public const int PERCENTAGE_OF_ASYMPTOMATIC = 5;
        public const int ASYMPTOMATIC_MIN_RESISTANCE = 90;
        public const int ASYMPTOMATIC_MAX_RESISTANCE = 101; // 101 exclus

        public const int SYMPTOMATIC_MIN_RESISTANCE = 80;
        public const int SYMPTOMATIC_MAX_RESISTANCE = 90; // 90 exclus

        // Person
        public const int DEFAULT_PERSON_AGE = 30;

        public const double AVERAGE_QUANTA_EXHALATION = 5.6d;

        #region SitesDefaultParameter
        public const double OUTSIDE_TEMPERATURE = 30;

        // Bâtiments
        public const double BUILDING_WIDTH = 15;
        public const double BUILDING_LENGTH = 15;
        public const double BUILDING_HEIGHT = 2;

        public const double BUILDING_PRESSURE = 0.95d;
        public const double BUILDING_TEMPERATURE = 23;

        public const double BUILDING_CO2 = 415;
        public const double BUILDING_VENTILATION_WITH_OUTSIDE = 0.7d;
        public const double BUILDING_ADDITIONAL_CONTROL_MEASURES = 0;

        // Véhicules
            // Voiture
        public const double CAR_WIDTH = 30;
        public const double CAR_LENGTH = 30;
        public const double CAR_HEIGHT = 3;
                            
        public const double CAR_VENTILATION_WITH_OUTSIDE = 0.7d;
        public const double CAR_ADDITIONAL_CONTROL_MEASURES = 0;
            // Bus
        public const double BUS_WIDTH = 2.5d;
        public const double BUS_LENGTH = 19;
        public const double BUS_HEIGHT = 3.3d;
                            
        public const double BUS_VENTILATION_WITH_OUTSIDE = 3d;
        public const double BUS_ADDITIONAL_CONTROL_MEASURES = 0;
        #endregion
    }
}

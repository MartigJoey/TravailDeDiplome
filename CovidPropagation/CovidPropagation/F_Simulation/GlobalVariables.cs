/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 27.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;
using System.Drawing;

namespace CovidPropagation
{
    static class GlobalVariables
    {
        // Tableau contenant les proba pour chaque âge
        // 0-19  20-30  30-40  40-60  60-70  80+
        //public static readonly double[] PROBABILITY_TO_HAVE_ILNESS = new double[] { 0.0001, 0.0002, 0.0003, 0.0004, 0.0006 };
        //public static readonly double[] PROBABILITY_TO_HAVE_SMALL_ILNESS = new double[] {     0.96, 0.90, 0.85, 0.75, 0.50 };
        //public static readonly double[] PROBABILITY_TO_HAVE_NORMAL_ILNESS = new double[] {    0.03, 0.07, 0.10, 0.15, 0.30  };
        //public static readonly double[] PROBABILITY_TO_HAVE_DANGEROUS_ILNESS = new double[] { 0.01, 0.03, 0.05, 0.10, 0.20  };

        public static readonly Random rdm = new Random();

        public const int DURATION_OF_PERIOD = 30; // En minutes
        public const int MINUTES_PER_DAY = 1440;
        public const int NUMBER_OF_PERIODS = MINUTES_PER_DAY / DURATION_OF_PERIOD;
        public const int NUMBER_OF_DAY = 7;

        public const int TIMER_INTERVAL = 1000; // 50 minimum

        public const int BUS_MAX_PERSON = 18;
        public const int CAR_MAX_PERSON = 4;

        public const double ILNESS_INFECTION_PROBABILITY = 0.01d;
        public const double ILNESS_MIN_ATTACK = 0.05d;
        public const double ILNESS_MAX_ATTACK = 0.2d;

        public const int DEFAULT_INTERVAL = 500;

        // Symptomatic / Asymptomatic
        public const int PERCENTAGE_OF_ASYMPTOMATIC = 5;
        public const int ASYMPTOMATIC_MIN_RESISTANCE = 90;
        public const int ASYMPTOMATIC_MAX_RESISTANCE = 101; // 101 exclus

        public const int SYMPTOMATIC_MIN_RESISTANCE = 80;
        public const int SYMPTOMATIC_MAX_RESISTANCE = 90; // 90 exclus

        // Person
        public const int DEFAULT_PERSON_AGE = 30;
        public const int MAX_SCHOOL_AGE = 18;

        public const double BREATHING_RATE = 0.026d * 60;

        public const double AVERAGE_QUANTA_EXHALATION = 5.6d;

        public const double AVERAGE_EXHALATION_MASK_EFFICIENCY = 0.5;
        public const double AVERAGE_INHALATION_MASK_EFFICIENCY = 0.3;

        #region SitesDefaultParameter
        public const double OUTSIDE_TEMPERATURE = 30;

        // Bâtiments
        public const double BUILDING_WIDTH = 30;
        public const double BUILDING_LENGTH = 30;
        public const double BUILDING_HEIGHT = 3;

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
                            
        public const double CAR_PRESSURE = 0.95d;
        public const double CAR_TEMPERATURE = 23;
                            
        public const double CAR_CO2 = 415;
        public const double CAR_VENTILATION_WITH_OUTSIDE = 0.7d;
        public const double CAR_ADDITIONAL_CONTROL_MEASURES = 0;
            // Bus
        public const double BUS_WIDTH = 2.5d;
        public const double BUS_LENGTH = 19;
        public const double BUS_HEIGHT = 3.3d;
                            
        public const double BUS_PRESSURE = 0.95d;
        public const double BUS_TEMPERATURE = 23;
                            
        public const double BUS_CO2 = 415;
        public const double BUS_VENTILATION_WITH_OUTSIDE = 3d;
        public const double BUS_ADDITIONAL_CONTROL_MEASURES = 0;
        #endregion
    }
}

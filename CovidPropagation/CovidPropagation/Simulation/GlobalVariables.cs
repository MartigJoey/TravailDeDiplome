using System.Drawing;

namespace CovidPropagation.Classes
{
    static class GlobalVariables
    {
        public const int DURATION_OF_PERIOD = 30; // En minutes
        public const int MINUTES_PER_DAY = 1440;
        public const int NUMBER_OF_PERIODS = MINUTES_PER_DAY / DURATION_OF_PERIOD;
        public const int NUMBER_OF_DAY = 7;

        public const int TIMER_INTERVAL = 1000; // 50 minimum

        public const int BUS_MAX_PERSON = 18;
        public const int CAR_MAX_PERSON = 4;
    }
}

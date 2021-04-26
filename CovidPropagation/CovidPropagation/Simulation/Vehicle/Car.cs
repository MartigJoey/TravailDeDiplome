using CovidPropagation.Classes;
using System.Drawing;

namespace CovidPropagation
{
    class Car : Vehicle
    {
        private bool _isDisplayed;

        public bool IsDisplayed { get => _isDisplayed; set => _isDisplayed = value; }

        public Car() : base(GlobalVariables.CAR_MAX_PERSON)
        {
            IsDisplayed = false;
        }
    }
}

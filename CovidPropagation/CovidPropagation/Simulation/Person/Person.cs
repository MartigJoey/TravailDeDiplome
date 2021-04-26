using System;
using System.Drawing;
using CovidPropagation.Classes.Person;
using CovidPropagation.Classes;
using CovidPropagation.Simulation;

namespace CovidPropagation
{
    enum PersonState
    {
        Healthy,
        Infected,
        Asymptomatic
    }

    class Person
    {
        private Planning _planning;
        private Site _current; // Int temporaire --> batiment / véhicules
        private PersonState _state;
        public PersonState CurrentState { get => _state; set => _state = value; }

        public Person(Planning planning, PersonState state)
        {
            _planning = planning;
            _state = state;
        }

        public Person(Planning planning) : this (planning, PersonState.Healthy)
        {
            // Does nothing
        }

        public void ChangeActivity()
        {
            _current = _planning.GetActivity();
            

        }
    }
}

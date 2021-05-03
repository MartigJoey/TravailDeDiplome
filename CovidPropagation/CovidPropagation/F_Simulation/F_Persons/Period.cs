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
    /// <summary>
    /// Période composé d'une activité.
    /// </summary>
    public class Period
    {
        private Type _activity;

        public Type Activity { get => _activity; }

        public Period(Type activity)
        {
            _activity = activity;
        }
    }
}

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
    public class TimeFrame
    {
        private Site _activity;
        private SitePersonStatus _reason;

        public Site Activity { get => _activity; }
        public SitePersonStatus Reason { get => _reason; set => _reason = value; }

        public TimeFrame(Site activity, SitePersonStatus reason)
        {
            _activity = activity;
            _reason = reason;
        }
    }
}

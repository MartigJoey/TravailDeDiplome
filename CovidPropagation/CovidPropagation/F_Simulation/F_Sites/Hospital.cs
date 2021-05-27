/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 27.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CovidPropagation
{
    public class Hospital : WorkSite
    {
        int nbCovidPatientMax;
        int nbExtremeCovidPatientMax;
        int nbSumCovidPatientMax;
        List<Person> covidPatient;
        private const int nbWorkPlaces = 50;
        private static SiteType[] hospitalTypes = new SiteType[] { SiteType.Hospital, SiteType.Eat, SiteType.WorkPlace };
        
        public Hospital(double length = GlobalVariables.BUILDING_LENGTH,
                           double width = GlobalVariables.BUILDING_WIDTH,
                           double height = GlobalVariables.BUILDING_HEIGHT,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(hospitalTypes, length, width, height, ventilationWithOutside, additionalControlMeasures, nbWorkPlaces)
        {
            nbCovidPatientMax = 50;
            nbExtremeCovidPatientMax = 10;
            nbSumCovidPatientMax = nbCovidPatientMax + nbExtremeCovidPatientMax;
            covidPatient = new List<Person>();
        }

        public Hospital() : this(GlobalVariables.BUILDING_LENGTH,
                                 GlobalVariables.BUILDING_WIDTH,
                                 GlobalVariables.BUILDING_HEIGHT,
                                 GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                                 GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES)
        {

        }

        /// <summary>
        /// Fait entrer un individu pour cause de covid dans le bâtiment s'il y a de la place
        /// </summary>
        /// <param name="patient">Patient qui essai d'entrer.</param>
        /// <returns>Si le patient a pu entrer ou non.</returns>
        public bool EnterForCovid(Person patient)
        {
            bool result = false;
            if (covidPatient.Count < nbSumCovidPatientMax)
            {
                covidPatient.Add(patient);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Fait quitter un individu étant admis pour cause de covid.
        /// </summary>
        /// <param name="patient">Patient qui quitte l'hôpital.</param>
        public void LeaveForCovid(Person patient)
        {
            covidPatient.Remove(patient);
        }

        /// <summary>
        /// Traite le patient. Réduit le nombre de maladies plus rapidement que la normale.
        /// Recalcile la résistance au virus du patient.
        /// Vérifie s'il doit quitter l'hôpital car il est remit.
        /// </summary>
        public void TreatPatients()
        {
            covidPatient.ForEach(p => { 
                p.GetHospitalTreatment();
                p.RecalculateVirusResistance(2);
                if (p.CurrentState < PersonState.Infected)
                    p.MustLeaveHospital = true;
            });
        }

        /// <summary>
        /// Compte le nombre de patients covid.
        /// </summary>
        /// <returns>Nombre de patient covid.</returns>
        public int CountPatients()
        {
            return covidPatient.Count;
        }
    }
}

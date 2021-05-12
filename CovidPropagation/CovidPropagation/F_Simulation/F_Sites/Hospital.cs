/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 27.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public class Hospital : Site
    {
        int nbCovidPatientMax;
        int nbExtremeCovidPatientMax;
        int nbSumCovidPatientMax;
        List<Person> covidPatient;
        private static SiteType[] hospitalTypes = new SiteType[] { SiteType.Hospital, SiteType.Eat, SiteType.WorkPlace };
        
        public Hospital(double length = GlobalVariables.BUILDING_LENGTH,
                           double width = GlobalVariables.BUILDING_WIDTH,
                           double height = GlobalVariables.BUILDING_HEIGHT,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(hospitalTypes, length, width, height, ventilationWithOutside, additionalControlMeasures)
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

        public void LeaveForCovid(Person patient)
        {
            covidPatient.Remove(patient);
        }

        public void TreatPatients()
        {
            covidPatient.ForEach(p => { 
                p.GetCovidTreatment();
                p.RecalculateVirusResistance(2);
            });
        }
    }
}

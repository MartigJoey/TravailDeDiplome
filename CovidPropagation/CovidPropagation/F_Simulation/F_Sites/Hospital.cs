/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 27.04.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste tel qu'une ville.
 */
using System.Collections.Generic;

namespace CovidPropagation
{
    /// <summary>
    /// Lieu pouvant accueillir des cas de covid pour les traiter.
    /// </summary>
    public class Hospital : WorkSite
    {
        private const int LENGTH = 100;
        private const int WIDTH = 50;
        private const int HEIGHT = 10;
        private const int NUMBER_OF_WORK_PLACE = 50;
        private const int NUMBER_MAX_OF_COVID_PATIENT = 50;
        private const int NUMBER_MAX_OF_COVID_PATIENT_EXTREME = 10;

        int nbCovidPatientMax;
        int nbExtremeCovidPatientMax;
        int nbSumCovidPatientMax;
        List<Person> covidPatient;
        private static SiteType[] hospitalTypes = new SiteType[] { SiteType.Hospital, SiteType.Eat, SiteType.WorkPlace };
        
        public Hospital(double length = LENGTH,
                           double width = WIDTH,
                           double height = HEIGHT,
                           double ventilationWithOutside = GlobalVariables.BUILDING_VENTILATION_WITH_OUTSIDE,
                           double additionalControlMeasures = GlobalVariables.BUILDING_ADDITIONAL_CONTROL_MEASURES) :
                 base(hospitalTypes, length, width, height, ventilationWithOutside, additionalControlMeasures, NUMBER_OF_WORK_PLACE)
        {
            nbCovidPatientMax = NUMBER_MAX_OF_COVID_PATIENT;
            nbExtremeCovidPatientMax = NUMBER_MAX_OF_COVID_PATIENT_EXTREME;
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
        /// ID Documentation : Enter_Hospital
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

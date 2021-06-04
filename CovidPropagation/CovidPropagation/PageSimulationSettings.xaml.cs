/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CovidPropagation
{
    /// <summary>
    /// Logique d'interaction pour PageSimulationSettings.xaml
    /// </summary>
    public partial class PageSimulationSettings : Page
    {
        enum tbxType
        {
            Probability = 1,
            Size = 2,
            DayDuration = 3,
            MonthDuration = 4,
            NbPersons = 5,
            Quanta = 7
        }
        public PageSimulationSettings()
        {
            InitializeComponent();
            SetUIParameters();
            SetDefaultData();
        }

        private void SetUIParameters()
        {
            // Ajoute un évènement qui est trigger lorsque ctrl + v est pressé dans le textbox
            DataObject.AddPastingHandler(tbxCoughMaxQuanta, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxCoughMinQuanta, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxNbPeople, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxProbabilityOfInfected, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVaccinationDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVaccinationEfficiency, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusDeathRate, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusHospitalisationRate, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMaxDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMaxImmunityDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMaxIncubationDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMinDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMinImmunityDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMinIncubationDuration, this.OnCancelCommand);

            tbxMaskNbPeopleToStart.IsEnabled = false;
            tbxMaskNbPeopleToStop.IsEnabled = false;
            tbxDistanciationNbPeopleToStart.IsEnabled = false;
            tbxDistanciationNbPeopleToStop.IsEnabled = false;
            pnlCbxQuarantine.IsEnabled = false;
            tbxQuarantineNbPeopleToStart.IsEnabled = false;
            tbxQuarantineNbPeopleToStop.IsEnabled = false;
            tbxVaccinationDuration.IsEnabled = false;
            tbxVaccinationEfficiency.IsEnabled = false;
            tbxVaccinationNbPeopleToStart.IsEnabled = false;
            tbxVaccinationNbPeopleToStop.IsEnabled = false;
        }

        private void SetDefaultData()
        {
            // Général
            tbxNbPeople.Text = SimulationGeneralParameters.NbPeople.ToString();
            tbxProbabilityOfInfected.Text = SimulationGeneralParameters.ProbabilityOfInfected.ToString();

            // Triggers mesures
            tbxQuarantineNbPeopleToStart.Text = SimulationGeneralParameters.NbInfecetdForQuarantineActivation.ToString();
            tbxQuarantineNbPeopleToStop.Text = SimulationGeneralParameters.NbInfecetdForQuarantineDeactivation.ToString();

            tbxVaccinationNbPeopleToStart.Text = SimulationGeneralParameters.NbInfecetdForVaccinationActivation.ToString();
            tbxVaccinationNbPeopleToStop.Text = SimulationGeneralParameters.NbInfecetdForVaccinationDeactivation.ToString();

            tbxMaskNbPeopleToStart.Text = SimulationGeneralParameters.NbInfecetdForMaskActivation.ToString();
            tbxMaskNbPeopleToStop.Text = SimulationGeneralParameters.NbInfecetdForMaskDeactivation.ToString();

            tbxDistanciationNbPeopleToStart.Text = SimulationGeneralParameters.NbInfecetdForDistanciationActivation.ToString();
            tbxDistanciationNbPeopleToStop.Text = SimulationGeneralParameters.NbInfecetdForDistanciationDeactivation.ToString();

            // Vaccin
            tbxVaccinationDuration.Text = VaccinationParameters.Duration.ToString();
            tbxVaccinationEfficiency.Text = VaccinationParameters.Efficacity.ToString();

            // Virus
            tbxVirusMinDuration.Text = VirusParameters.DurationMin.ToString();
            tbxVirusMaxDuration.Text = VirusParameters.DurationMax.ToString();

            tbxVirusMinIncubationDuration.Text = VirusParameters.IncubationDurationMin.ToString();
            tbxVirusMaxIncubationDuration.Text = VirusParameters.IncubationDurationMax.ToString();

            tbxVirusMinImmunityDuration.Text = VirusParameters.ImmunityDurationMin.ToString();
            tbxVirusMaxImmunityDuration.Text = VirusParameters.ImmunityDurationMax.ToString();

            tbxVirusHospitalisationRate.Text = VirusParameters.HospitalRate.ToString();
            tbxVirusDeathRate.Text = VirusParameters.DeathRate.ToString();

            // Quanta
            tbxCoughMinQuanta.Text = VirusParameters.CoughMinQuanta.ToString();
            tbxCoughMaxQuanta.Text = VirusParameters.CoughMaxQuanta.ToString();
        }
        
        /// <summary>
        /// Regex qui filtres les caractères non numériques.
        /// </summary>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Si la valeur du textBox est plus grande que le maximum autorisé, alors il est assigné au maximum.
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            int min, max;
            switch ((tbxType)Convert.ToInt32(tbx.Tag))
            {
                case tbxType.Probability:
                    min = 0;
                    max = 100;
                    break;
                case tbxType.Size:
                    min = 10;
                    max = 1000;
                    break;
                case tbxType.DayDuration:
                    min = 0;
                    max = 100000;
                    break;
                case tbxType.MonthDuration:
                    min = 0;
                    max = 100000;
                    break;
                default:
                case tbxType.NbPersons:
                    min = 1000;
                    max = 1000000;
                    break;
                case tbxType.Quanta:
                    min = 0;
                    max = 800;
                    break;
            }

            if (tbx.Text.Length > 0 && Convert.ToDouble(tbx.Text) > max)
                tbx.Text = max.ToString();
            else if(tbx.Text.Length > 0 && Convert.ToDouble(tbx.Text) < min)
                tbx.Text = min.ToString();
        }

        /// <summary>
        /// S'active lors d'un ctrl+v. Permet d'éviter de coller des caratères qui ne sont pas du texte.
        /// </summary>
        private void OnCancelCommand(object sender, DataObjectEventArgs e)
        {
            e.CancelCommand();
        }

        private void Measure_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chx = sender as CheckBox;

            switch (chx.Tag)
            {
                default:
                case "Mask":
                    tbxMaskNbPeopleToStart.IsEnabled = !tbxMaskNbPeopleToStart.IsEnabled;
                    tbxMaskNbPeopleToStop.IsEnabled = !tbxMaskNbPeopleToStop.IsEnabled;
                    break;
                case "Distancing":
                    tbxDistanciationNbPeopleToStart.IsEnabled = !tbxDistanciationNbPeopleToStart.IsEnabled;
                    tbxDistanciationNbPeopleToStop.IsEnabled = !tbxDistanciationNbPeopleToStop.IsEnabled;
                    break;
                case "Quarantine":
                    pnlCbxQuarantine.IsEnabled = !pnlCbxQuarantine.IsEnabled;
                    tbxQuarantineNbPeopleToStart.IsEnabled = !tbxQuarantineNbPeopleToStart.IsEnabled;
                    tbxQuarantineNbPeopleToStop.IsEnabled = !tbxQuarantineNbPeopleToStop.IsEnabled;
                    break;
                case "Vaccination":
                    tbxVaccinationDuration.IsEnabled = !tbxVaccinationDuration.IsEnabled;
                    tbxVaccinationEfficiency.IsEnabled = !tbxVaccinationEfficiency.IsEnabled;
                    tbxVaccinationNbPeopleToStart.IsEnabled = !tbxVaccinationNbPeopleToStart.IsEnabled;
                    tbxVaccinationNbPeopleToStop.IsEnabled = !tbxVaccinationNbPeopleToStop.IsEnabled;
                    break;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SetVirusParameters();
            SetGeneralParameters();
            SetMaskMeasureParameters();
            SetQuarantineMeasureParameters();
            SetVaccinationMeasureParameters();
        }

        private void SetVirusParameters()
        {
            VirusParameters.Init();

            if (tbxVirusMinIncubationDuration.Text.Length > 0)
                VirusParameters.IncubationDurationMin = Convert.ToInt32(tbxVirusMinIncubationDuration.Text);

            if (tbxVirusMaxIncubationDuration.Text.Length > 0)
                VirusParameters.IncubationDurationMax = Convert.ToInt32(tbxVirusMaxIncubationDuration.Text);

            if (tbxVirusMaxDuration.Text.Length > 0)
                VirusParameters.DurationMax = Convert.ToInt32(tbxVirusMaxDuration.Text);

            if (tbxVirusMinDuration.Text.Length > 0)
                VirusParameters.DurationMin = Convert.ToInt32(tbxVirusMinDuration.Text);

            if (tbxVirusMinImmunityDuration.Text.Length > 0)
                VirusParameters.ImmunityDurationMin = Convert.ToInt32(tbxVirusMinImmunityDuration.Text);

            if (tbxVirusMaxImmunityDuration.Text.Length > 0)
                VirusParameters.ImmunityDurationMax = Convert.ToInt32(tbxVirusMaxImmunityDuration.Text);

            if (tbxVirusHospitalisationRate.Text.Length > 0)
                VirusParameters.HospitalRate = Convert.ToDouble(tbxVirusHospitalisationRate.Text);

            if (tbxVirusDeathRate.Text.Length > 0)
                VirusParameters.DeathRate = Convert.ToDouble(tbxVirusDeathRate.Text);

            // Symptoms
            if (tbxCoughMinQuanta.Text.Length > 0)
                VirusParameters.CoughMinQuanta = Convert.ToInt32(tbxCoughMinQuanta.Text);

            if (tbxCoughMaxQuanta.Text.Length > 0)
                VirusParameters.CoughMaxQuanta = Convert.ToInt32(tbxCoughMaxQuanta.Text);

            if (rdbCoughSymptomEnabled.IsChecked == true)
                VirusParameters.IsCoughSymptomActive = true;
            else if (rdbCoughSymptomDisabled.IsChecked == true)
                VirusParameters.IsCoughSymptomActive = false;

            // Transmission
            if (rdbAerosolTransmissionIsEnabled.IsChecked == true)
                VirusParameters.IsAerosolTransmissionActive = true;
            else if (rdbAerosolTransmissionIsDisabled.IsChecked == true)
                VirusParameters.IsCoughSymptomActive = false;
        }

        private void SetGeneralParameters()
        {
            // Nombre d'individu, et pourcentage d'infectés dès le départ.
            if (tbxNbPeople.Text.Length > 0)
                SimulationGeneralParameters.NbPeople = Convert.ToInt32(tbxNbPeople.Text);

            if (tbxProbabilityOfInfected.Text.Length > 0)
                SimulationGeneralParameters.ProbabilityOfInfected = (float)Convert.ToDouble(tbxProbabilityOfInfected.Text);

            // Si les mesures sont activées où non.
            if (chxMask.IsChecked == true)
                SimulationGeneralParameters.IsMaskMeasuresEnabled = true;

            if (chxDistanciation.IsChecked == true)
                SimulationGeneralParameters.IsDistanciationMeasuresEnabled = true;

            if (chxQuarantaine.IsChecked == true)
                SimulationGeneralParameters.IsQuarantineMeasuresEnabled = true;

            if (chxVaccination.IsChecked == true)
                SimulationGeneralParameters.IsVaccinationMeasuresEnabled = true;

            // Trigger des mesures
            // Masques
            if (tbxMaskNbPeopleToStart.Text.Length > 0)
                SimulationGeneralParameters.NbInfecetdForMaskActivation = Convert.ToInt32(tbxMaskNbPeopleToStart.Text);

            if (tbxMaskNbPeopleToStop.Text.Length > 0)
                SimulationGeneralParameters.NbInfecetdForMaskDeactivation = Convert.ToInt32(tbxMaskNbPeopleToStop.Text);

            // Distanciation
            if (tbxDistanciationNbPeopleToStart.Text.Length > 0)
                SimulationGeneralParameters.NbInfecetdForDistanciationActivation = Convert.ToInt32(tbxDistanciationNbPeopleToStart.Text);

            if (tbxDistanciationNbPeopleToStop.Text.Length > 0)
                SimulationGeneralParameters.NbInfecetdForDistanciationDeactivation = Convert.ToInt32(tbxDistanciationNbPeopleToStop.Text);

            // Quarantaine
            if (tbxQuarantineNbPeopleToStart.Text.Length > 0)
                SimulationGeneralParameters.NbInfecetdForQuarantineActivation = Convert.ToInt32(tbxQuarantineNbPeopleToStart.Text);

            if (tbxQuarantineNbPeopleToStop.Text.Length > 0)
                SimulationGeneralParameters.NbInfecetdForQuarantineDeactivation = Convert.ToInt32(tbxQuarantineNbPeopleToStop.Text);

            // Vaccination
            if (tbxVaccinationNbPeopleToStart.Text.Length > 0)
                SimulationGeneralParameters.NbInfecetdForVaccinationActivation = Convert.ToInt32(tbxVaccinationNbPeopleToStart.Text);

            if (tbxVaccinationNbPeopleToStop.Text.Length > 0)
                SimulationGeneralParameters.NbInfecetdForVaccinationDeactivation = Convert.ToInt32(tbxVaccinationNbPeopleToStop.Text);
        }

        private void SetMaskMeasureParameters()
        {
            MaskParameters.Init();
            
            // Nombre d'individu, et pourcentage d'infectés dès le départ.
            if (chxClientMaskIsOn.IsChecked == true)
                MaskParameters.IsClientMaskOn = true;

            if (chxClientMaskIsOn.IsChecked == true)
                MaskParameters.IsPersonnelMaskOn = true;
        }

        private void SetQuarantineMeasureParameters()
        {
            QuarantineParameters.Init();

            // Saint
            if (chxQuarantineHealthy.IsChecked == true)
                QuarantineParameters.IshealthyQuarantined = true;
            else
                QuarantineParameters.IshealthyQuarantined = false;

            // Infectés
            if (chxQuarantineInfected.IsChecked == true)
                QuarantineParameters.IsInfectedQuarantined = true;
            else
                QuarantineParameters.IsInfectedQuarantined = false;

            // Contagieux
            if (chxQuarantineInfectious.IsChecked == true)
                QuarantineParameters.IsInfectiousQuarantined = true;
            else
                QuarantineParameters.IsInfectiousQuarantined = false;

            // Immunisés
            if (chxQuarantineImmune.IsChecked == true)
                QuarantineParameters.IsImmuneQuarantined = true;
            else
                QuarantineParameters.IsImmuneQuarantined = false;
        }

        private void SetVaccinationMeasureParameters()
        {
            VaccinationParameters.Init();

            // Durée du vaccin
            if (tbxVaccinationDuration.Text.Length > 0)
                VaccinationParameters.Duration = Convert.ToInt32(tbxVaccinationDuration.Text);

            // Efficacité du vaccin
            if (tbxVaccinationEfficiency.Text.Length > 0)
                VaccinationParameters.Efficacity = Convert.ToInt32(tbxVaccinationEfficiency.Text);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            VirusParameters.Init();
            SimulationGeneralParameters.Init();
            MaskParameters.Init();
            VaccinationParameters.Init();
            QuarantineParameters.Init();
        }
    }
    /*
    public class RangeValidationRule : ValidationRule
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public override ValidationResult Validate(
          object value, System.Globalization.CultureInfo cultureInfo)
        {
            int intValue;

            string text = String.Format("Must be between {0} and {1}", MinValue, MaxValue);
            if (!Int32.TryParse(value.ToString(), out intValue))
                return new ValidationResult(false, "Not an integer");
            if (intValue < MinValue)
                return new ValidationResult(false, "To small. " + text);
            if (intValue > MaxValue)
                return new ValidationResult(false, "To large. " + text);
            return ValidationResult.ValidResult;
        }
    }*/
}

/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CovidPropagation
{
    /// <summary>
    /// Logique d'interaction pour PageSimulationSettings.xaml
    /// </summary>
    public partial class PageSimulationSettings : Page
    {
        public PageSimulationSettings()
        {
            InitializeComponent();
            SetUIParameters();
            SetParametersData();
        }

        /// <summary>
        /// Prend les valeurs des paramètres et active/désactive les champs nécessaires.
        /// </summary>
        private void SetUIParameters()
        {
            // Ajoute un évènement qui est trigger lorsque ctrl + v est pressé dans le textbox
            DataObject.AddPastingHandler(tbxCoughMaxQuanta, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxCoughMinQuanta, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxNbPeople, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxProbabilityOfInfected, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVaccinationDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVaccinationEfficiency, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusImmunityEfficiency, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMaxDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMaxImmunityDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMaxIncubationDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMinDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMinImmunityDuration, this.OnCancelCommand);
            DataObject.AddPastingHandler(tbxVirusMinIncubationDuration, this.OnCancelCommand);

            // Distanciation
            chxDistanciation.IsChecked = SimulationGeneralParameters.IsDistanciationMeasuresEnabled;
            tbxDistanciationNbPeopleToStart.IsEnabled = SimulationGeneralParameters.IsDistanciationMeasuresEnabled;
            tbxDistanciationNbPeopleToStop.IsEnabled = SimulationGeneralParameters.IsDistanciationMeasuresEnabled;

            tbxProbabilityForHealthyToBeQuarantined.IsEnabled = QuarantineParameters.IshealthyQuarantined;
            tbxQuarantinedDurationForHealthy.IsEnabled = QuarantineParameters.IshealthyQuarantined;

            tbxProbabilityForInfectedToBeQuarantined.IsEnabled = QuarantineParameters.IsInfectedQuarantined;
            tbxQuarantinedDurationForInfected.IsEnabled = QuarantineParameters.IsInfectedQuarantined;

            tbxProbabilityForInfectiousToBeQuarantined.IsEnabled = QuarantineParameters.IsInfectiousQuarantined;
            tbxQuarantinedDurationForInfectious.IsEnabled = QuarantineParameters.IsInfectiousQuarantined;

            tbxProbabilityForImmuneToBeQuarantined.IsEnabled = QuarantineParameters.IsImmuneQuarantined;
            tbxQuarantinedDurationForImmune.IsEnabled = QuarantineParameters.IsImmuneQuarantined;

            // Masques
            chxMask.IsChecked = SimulationGeneralParameters.IsMaskMeasuresEnabled;
            tbxMaskNbPeopleToStart.IsEnabled = SimulationGeneralParameters.IsMaskMeasuresEnabled;
            tbxMaskNbPeopleToStop.IsEnabled = SimulationGeneralParameters.IsMaskMeasuresEnabled;
            if (!SimulationGeneralParameters.IsMaskMeasuresEnabled)
            {
                splClientMask.IsEnabled = false;
                splPersonnelMask.IsEnabled = false;

                rdbClientMaskIsOn.IsChecked = MaskParameters.IsClientMaskOn;
                rdbClientMaskIsOff.IsChecked = !MaskParameters.IsClientMaskOn;
                rdbPersonnelMaskIsOn.IsChecked = MaskParameters.IsPersonnelMaskOn;
                rdbPersonnelMaskIsOff.IsChecked = !MaskParameters.IsPersonnelMaskOn;
            }

            // Quarantaine
            chxQuarantaine.IsChecked = SimulationGeneralParameters.IsQuarantineMeasuresEnabled;
            tbxQuarantineNbPeopleToStart.IsEnabled = SimulationGeneralParameters.IsQuarantineMeasuresEnabled;
            tbxQuarantineNbPeopleToStop.IsEnabled = SimulationGeneralParameters.IsQuarantineMeasuresEnabled;
            pnlCbxQuarantine.IsEnabled = SimulationGeneralParameters.IsQuarantineMeasuresEnabled;
            if (!SimulationGeneralParameters.IsQuarantineMeasuresEnabled)
            {
                chxQuarantineHealthy.IsChecked = false;
                chxQuarantineImmune.IsChecked = false;
                chxQuarantineInfected.IsChecked = false;
                chxQuarantineInfectious.IsChecked = false;
            }

            // Vaccination
            chxVaccination.IsChecked = SimulationGeneralParameters.IsVaccinationMeasuresEnabled;
            tbxVaccinationDuration.IsEnabled = SimulationGeneralParameters.IsVaccinationMeasuresEnabled;
            tbxVaccinationEfficiency.IsEnabled = SimulationGeneralParameters.IsVaccinationMeasuresEnabled;
            tbxVaccinationNbPeopleToStart.IsEnabled = SimulationGeneralParameters.IsVaccinationMeasuresEnabled;
            tbxVaccinationNbPeopleToStop.IsEnabled = SimulationGeneralParameters.IsVaccinationMeasuresEnabled;
        }

        /// <summary>
        /// Prend les valeurs des paramètres et les affiches.
        /// </summary>
        private void SetParametersData()
        {
            // Général
            tbxNbPeople.Text = SimulationGeneralParameters.NbPeople.ToString();
            tbxProbabilityOfInfected.Text = (SimulationGeneralParameters.ProbabilityOfInfected * 100).ToString();

            // Triggers mesures
            tbxQuarantineNbPeopleToStart.Text = SimulationGeneralParameters.NbInfecetdForQuarantineActivation.ToString();
            tbxQuarantineNbPeopleToStop.Text = SimulationGeneralParameters.NbInfecetdForQuarantineDeactivation.ToString();

            tbxVaccinationNbPeopleToStart.Text = SimulationGeneralParameters.NbInfecetdForVaccinationActivation.ToString();
            tbxVaccinationNbPeopleToStop.Text = SimulationGeneralParameters.NbInfecetdForVaccinationDeactivation.ToString();

            tbxMaskNbPeopleToStart.Text = SimulationGeneralParameters.NbInfecetdForMaskActivation.ToString();
            tbxMaskNbPeopleToStop.Text = SimulationGeneralParameters.NbInfecetdForMaskDeactivation.ToString();

            tbxDistanciationNbPeopleToStart.Text = SimulationGeneralParameters.NbInfecetdForDistanciationActivation.ToString();
            tbxDistanciationNbPeopleToStop.Text = SimulationGeneralParameters.NbInfecetdForDistanciationDeactivation.ToString();

            // Quarantaine
            tbxProbabilityForHealthyToBeQuarantined.Text = (QuarantineParameters.ProbabilityOfHealthyQuarantined * 100).ToString();
            tbxQuarantinedDurationForHealthy.Text = QuarantineParameters.DurationHealthyQuarantined.ToString();

            tbxProbabilityForInfectedToBeQuarantined.Text = (QuarantineParameters.ProbabilityOfInfectedQuarantined * 100).ToString();
            tbxQuarantinedDurationForInfected.Text = QuarantineParameters.DurationInfectedQuarantined.ToString();

            tbxProbabilityForInfectiousToBeQuarantined.Text = (QuarantineParameters.ProbabilityOfHealthyQuarantined * 100).ToString();
            tbxQuarantinedDurationForInfectious.Text = QuarantineParameters.DurationInfectiousQuarantined.ToString();

            tbxProbabilityForImmuneToBeQuarantined.Text = (QuarantineParameters.ProbabilityOfImmuneQuarantined * 100).ToString();
            tbxQuarantinedDurationForImmune.Text = QuarantineParameters.DurationImmuneQuarantined.ToString();

            // Vaccin
            tbxVaccinationDuration.Text = VaccinationParameters.Duration.ToString();
            tbxVaccinationEfficiency.Text = (VaccinationParameters.Efficiency * 100).ToString();

            // Virus
            tbxVirusMinDuration.Text = VirusParameters.DurationMin.ToString();
            tbxVirusMaxDuration.Text = VirusParameters.DurationMax.ToString();

            tbxVirusMinIncubationDuration.Text = VirusParameters.IncubationDurationMin.ToString();
            tbxVirusMaxIncubationDuration.Text = VirusParameters.IncubationDurationMax.ToString();

            tbxVirusMinImmunityDuration.Text = VirusParameters.ImmunityDurationMin.ToString();
            tbxVirusMaxImmunityDuration.Text = VirusParameters.ImmunityDurationMax.ToString();

            tbxVirusImmunityEfficiency.Text = (VirusParameters.ImmunityEfficiency * 100).ToString(); // Transformation de probabilités en pourcentage

            // Quanta
            tbxCoughMinQuanta.Text = VirusParameters.CoughMinQuanta.ToString();
            tbxCoughMaxQuanta.Text = VirusParameters.CoughMaxQuanta.ToString();
        }

        /// <summary>
        /// Regex qui filtres les caractères non numériques allant de 1'000 à 99'999'999.
        /// </summary>
        private void NbPersons_LeaveFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            Regex regex = new Regex("^([1-9][0-9]{3,7})$");
            string defaultValue = "10000";
            CheckTextBoxFormat(tbx, regex, defaultValue);
        }

        /// <summary>
        /// Regex qui filtres les caractères non numériques allant de 0 à 99'999'999.
        /// </summary>
        private void NbPersonsMeasures_LeaveFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            Regex regex = new Regex("^([0-9]{0,8})$");
            string defaultValue = "1000";
            CheckTextBoxFormat(tbx, regex, defaultValue);
        }

        /// <summary>
        /// Regex qui filtres les caractères non numériques allant de 0 à 1'000'000.
        /// </summary>
        private void DayDuration_LeaveFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            Regex regex = new Regex("^([0-9][0-9]{0,5})$");
            string defaultValue = "7";
            CheckTextBoxFormat(tbx, regex, defaultValue);
        }

        /// <summary>
        /// Regex qui filtres les caractères non numériques allant de 0 à 1'000'000.
        /// </summary>
        private void MonthDuration_LeaveFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            Regex regex = new Regex("^([0-9][0-9]{0,5})$");
            string defaultValue = "6";
            CheckTextBoxFormat(tbx, regex, defaultValue);
        }

        /// <summary>
        /// Regex qui filtres les caractères non numériques allant de 0 à 800.
        /// </summary>
        private void Quantas_LeaveFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            Regex regex = new Regex("^([1-7][0-9]{0,2}|800|[0-9]{1,2})$");
            string defaultValue = "200";
            CheckTextBoxFormat(tbx, regex, defaultValue);
        }

        /// <summary>
        /// Regex qui filtres les caractères non numériques allant de 0% à 100% en prenant en compte les décimals.
        /// </summary>
        private void Probability_LeaveFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            Regex regex = new Regex(@"\b(?<!\.)(?!0+(?:\.0+)?%)(?:\d|[1-9]\d|100)(?:(?<!100)\.\d+)?$");
            string defaultValue = "0.00";
            CheckTextBoxFormat(tbx, regex, defaultValue);
        }

        /// <summary>
        /// Vérifie que le format du textBox entre en accord avec le regex attribué.
        /// </summary>
        /// <param name="tbx">Textbox modifié.</param>
        /// <param name="regex">Règle des valeurs du textbox.</param>
        /// <param name="defaultValue">Valeur placée dans le textbox si l'utilisateur rentre un format incorrect.</param>
        private void CheckTextBoxFormat(TextBox tbx, Regex regex, string defaultValue)
        {
            if (!regex.IsMatch(tbx.Text))
            {
                tbx.Background = this.FindResource("highlightWrongFormat") as Brush;
                tbx.Text = defaultValue;
            }
            else
            {
                tbx.Background = this.FindResource("highlightCorrectFormat") as Brush;
            }
        }

        /// <summary>
        /// S'active lors d'un ctrl+v. Permet d'éviter de coller des caratères qui ne sont pas du texte.
        /// </summary>
        private void OnCancelCommand(object sender, DataObjectEventArgs e)
        {
            e.CancelCommand();
        }

        /// <summary>
        /// Active ou désactive les champs lié à un checkbox.
        /// </summary>
        /// <param name="sender">Checkbox qui a trigger l'évènement.</param>
        private void Measure_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chx = sender as CheckBox;

            switch (chx.Tag)
            {
                default:
                case "Mask":
                    tbxMaskNbPeopleToStart.IsEnabled = (chx.IsChecked == true);
                    tbxMaskNbPeopleToStop.IsEnabled = (chx.IsChecked == true);
                    splClientMask.IsEnabled = (chx.IsChecked == true);
                    splPersonnelMask.IsEnabled = (chx.IsChecked == true);
                    break;
                case "Distancing":
                    tbxDistanciationNbPeopleToStart.IsEnabled = (chx.IsChecked == true);
                    tbxDistanciationNbPeopleToStop.IsEnabled = (chx.IsChecked == true);
                    break;
                case "Quarantine":
                    pnlCbxQuarantine.IsEnabled = (chx.IsChecked == true);
                    tbxQuarantineNbPeopleToStart.IsEnabled = (chx.IsChecked == true);
                    tbxQuarantineNbPeopleToStop.IsEnabled = (chx.IsChecked == true);
                    if (chx.IsChecked == false)
                    {
                        chxQuarantineHealthy.IsChecked = false;
                        chxQuarantineImmune.IsChecked = false;
                        chxQuarantineInfected.IsChecked = false;
                        chxQuarantineInfectious.IsChecked = false;
                    }
                    break;
                case "Vaccination":
                    tbxVaccinationDuration.IsEnabled = (chx.IsChecked == true);
                    tbxVaccinationEfficiency.IsEnabled = (chx.IsChecked == true);
                    tbxVaccinationNbPeopleToStart.IsEnabled = (chx.IsChecked == true);
                    tbxVaccinationNbPeopleToStop.IsEnabled = (chx.IsChecked == true);
                    break;
            }
        }

        /// <summary>
        /// Active ou désactive les champs lié au checkbox de la quarantaine.
        /// </summary>
        /// <param name="sender">CheckBox qui a été coché.</param>
        private void QuarantineTypeChecked_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chx = sender as CheckBox;
            switch (chx.Tag.ToString())
            {
                default:
                case "Healthy":
                    tbxProbabilityForHealthyToBeQuarantined.IsEnabled = chxQuarantineHealthy.IsChecked == true;
                    tbxQuarantinedDurationForHealthy.IsEnabled = chxQuarantineHealthy.IsChecked == true;
                    break;
                case "Infected":
                    tbxProbabilityForInfectedToBeQuarantined.IsEnabled = chxQuarantineInfected.IsChecked == true;
                    tbxQuarantinedDurationForInfected.IsEnabled = chxQuarantineInfected.IsChecked == true;
                    break;
                case "Infectious":
                    tbxProbabilityForInfectiousToBeQuarantined.IsEnabled = chxQuarantineInfectious.IsChecked == true;
                    tbxQuarantinedDurationForInfectious.IsEnabled = chxQuarantineInfectious.IsChecked == true;
                    break;
                case "Immune":
                    tbxProbabilityForImmuneToBeQuarantined.IsEnabled = chxQuarantineImmune.IsChecked == true;
                    tbxQuarantinedDurationForImmune.IsEnabled = chxQuarantineImmune.IsChecked == true;
                    break;
            }
        }

        /// <summary>
        /// Sauvegarde les paramètres que l'utilisateur a entré.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SetVirusParameters();
            SetGeneralParameters();
            SetMaskMeasureParameters();
            SetQuarantineMeasureParameters();
            SetVaccinationMeasureParameters();
        }

        /// <summary>
        /// Récupère les paramètres du virus et les affiches.
        /// </summary>
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

            if (tbxVirusImmunityEfficiency.Text.Length > 0)
                VirusParameters.ImmunityEfficiency = Convert.ToDouble(tbxVirusImmunityEfficiency.Text) / 100; // Transformation de pourcentage en probabilités

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

        /// <summary>
        /// Récupère les paramètres généraux et les affiches.
        /// </summary>
        private void SetGeneralParameters()
        {
            // Nombre d'individu, et pourcentage d'infectés dès le départ.
            if (tbxNbPeople.Text.Length > 0)
                SimulationGeneralParameters.NbPeople = Convert.ToInt32(tbxNbPeople.Text);

            if (tbxProbabilityOfInfected.Text.Length > 0)
                SimulationGeneralParameters.ProbabilityOfInfected = (float)Convert.ToDouble(tbxProbabilityOfInfected.Text) / 100;

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

        /// <summary>
        /// Sauvegarde les paramètres des masques s'il sont actifs.
        /// </summary>
        private void SetMaskMeasureParameters()
        {
            MaskParameters.Init();
            
            // Nombre d'individu, et pourcentage d'infectés dès le départ.
            if (rdbClientMaskIsOn.IsChecked == true)
                MaskParameters.IsClientMaskOn = (rdbClientMaskIsOn.IsChecked == true);

            if (rdbPersonnelMaskIsOn.IsChecked == true)
                MaskParameters.IsPersonnelMaskOn = (rdbPersonnelMaskIsOn.IsChecked == true);
        }

        /// <summary>
        /// Sauvegarde les paramètres des mesures de quarantaines.
        /// </summary>
        private void SetQuarantineMeasureParameters()
        {
            QuarantineParameters.Init();

            // Saint
            if (chxQuarantineHealthy.IsChecked == true)
            {
                QuarantineParameters.IshealthyQuarantined = true;
                QuarantineParameters.ProbabilityOfHealthyQuarantined = Convert.ToDouble(tbxProbabilityForHealthyToBeQuarantined.Text) / 100;
                QuarantineParameters.DurationHealthyQuarantined = Convert.ToInt32(tbxQuarantinedDurationForHealthy.Text);
            }
            else
            {
                QuarantineParameters.IshealthyQuarantined = false;
            }

            // Infectés
            if (chxQuarantineInfected.IsChecked == true)
            {
                QuarantineParameters.IsInfectedQuarantined = true;
                QuarantineParameters.ProbabilityOfInfectedQuarantined = Convert.ToDouble(tbxProbabilityForInfectedToBeQuarantined.Text) / 100;
                QuarantineParameters.DurationInfectedQuarantined = Convert.ToInt32(tbxQuarantinedDurationForInfected.Text);
            }
            else
            {
                QuarantineParameters.IsInfectedQuarantined = false;
            }

            // Contagieux
            if (chxQuarantineInfectious.IsChecked == true)
            {
                QuarantineParameters.IsInfectiousQuarantined = true;
                QuarantineParameters.ProbabilityOfHealthyQuarantined = Convert.ToDouble(tbxProbabilityForInfectiousToBeQuarantined.Text) / 100;
                QuarantineParameters.DurationInfectiousQuarantined = Convert.ToInt32(tbxQuarantinedDurationForInfectious.Text);
            }
            else
            {
                QuarantineParameters.IsInfectiousQuarantined = false;
            }

            // Immunisés
            if (chxQuarantineImmune.IsChecked == true)
            {
                QuarantineParameters.IsImmuneQuarantined = true;
                QuarantineParameters.ProbabilityOfImmuneQuarantined = Convert.ToDouble(tbxProbabilityForImmuneToBeQuarantined.Text) / 100;
                QuarantineParameters.DurationImmuneQuarantined = Convert.ToInt32(tbxQuarantinedDurationForImmune.Text);
            }
            else
            {
                QuarantineParameters.IsImmuneQuarantined = false;
            }
        }

        /// <summary>
        /// Sauvegarde les paramètres de vaccination.
        /// </summary>
        private void SetVaccinationMeasureParameters()
        {
            VaccinationParameters.Init();

            // Durée du vaccin
            if (tbxVaccinationDuration.Text.Length > 0)
                VaccinationParameters.Duration = Convert.ToInt32(tbxVaccinationDuration.Text);

            // Efficacité du vaccin
            if (tbxVaccinationEfficiency.Text.Length > 0)
                VaccinationParameters.Efficiency = Convert.ToInt32(tbxVaccinationEfficiency.Text);
        }

        /// <summary>
        /// Annule les modifications et remet les paramètres précédents.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SetUIParameters();
            SetParametersData();
        }

        /// <summary>
        /// Remet les paramètres par défaut.
        /// </summary>
        private void Default_Click(object sender, RoutedEventArgs e)
        {
            VirusParameters.Init();
            SimulationGeneralParameters.Init();
            MaskParameters.Init();
            VaccinationParameters.Init();
            QuarantineParameters.Init();

            SetUIParameters();
            SetParametersData();
        }
    }
}

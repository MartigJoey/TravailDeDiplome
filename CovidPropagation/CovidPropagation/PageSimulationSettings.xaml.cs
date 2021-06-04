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
            SetParametersData();
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

        private void SetParametersData()
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
            tbxVaccinationEfficiency.Text = VaccinationParameters.Efficiency.ToString();

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
        /// Regex qui filtres les caractères non numériques.
        /// </summary>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9].+");
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
                    if ((chx.IsChecked == false))
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
            if (rdbClientMaskIsOn.IsChecked == true)
                MaskParameters.IsClientMaskOn = true;

            if (rdbPersonnelMaskIsOn.IsChecked == true)
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
                VaccinationParameters.Efficiency = Convert.ToInt32(tbxVaccinationEfficiency.Text);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SetUIParameters();
            SetParametersData();
        }

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

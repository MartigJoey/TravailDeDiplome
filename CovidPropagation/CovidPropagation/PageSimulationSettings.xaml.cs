using System;
using System.Collections.Generic;
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

            if (tbx.Text.Length > 0 && Convert.ToInt32(tbx.Text) > max)
                tbx.Text = max.ToString();
            else if(tbx.Text.Length > 0 && Convert.ToInt32(tbx.Text) < min)
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
                    // Does something not implemeted yet
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
    }
}

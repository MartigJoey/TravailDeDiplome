/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System.Windows;
using System.Windows.Navigation;

namespace CovidPropagation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PageSimulation pageSimulation;
        PageSimulationSettings pageSimulationSettings;
        PageGraphicSettings pageGraphicSettings;

        public MainWindow()
        {
            InitializeComponent();

            SimulationGeneralParameters.Init();
            VirusParameters.Init();
            MaskParameters.Init();
            VaccinationParameters.Init();
            QuarantineParameters.Init();

            pageSimulation = new PageSimulation();
            pageSimulationSettings = new PageSimulationSettings();
            pageGraphicSettings = new PageGraphicSettings();
            MainContent.NavigationUIVisibility = NavigationUIVisibility.Hidden; // Cache la barre de navigation du contenu
        }

        private void SimulationPage_Click(object sender, RoutedEventArgs e)
        {
            pageSimulation.SetGrid(pageGraphicSettings.GetGrid(), pageGraphicSettings.GetChartsData());
            MainContent.Navigate(pageSimulation);
        }

        private void GraphicSettingsPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(pageGraphicSettings);
        }

        private void SettingsPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(pageSimulationSettings);
        }
    }
}

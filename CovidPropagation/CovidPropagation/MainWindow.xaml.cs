using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            pageSimulation = new PageSimulation();
            pageSimulationSettings = new PageSimulationSettings();
            pageGraphicSettings = new PageGraphicSettings();
            MainContent.NavigationUIVisibility = NavigationUIVisibility.Hidden; // Cache la barre de navigation du contenu
        }

        private void SimulationPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(pageSimulation);
        }

        private void GraphicSettingsPage_Click(object sender, RoutedEventArgs e)
        {
            LC lcPage = new LC();
            lcPage.Show();
            //MainContent.Navigate(pageSimulationSettings);
        }

        private void SettingsPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(pageGraphicSettings);
        }
    }
}

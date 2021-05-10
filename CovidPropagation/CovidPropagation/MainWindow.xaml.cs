﻿using System;
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
        Simulation sim;

        public Simulation Sim { get => sim; set => sim = value; }

        public MainWindow()
        {
            InitializeComponent();
            pageSimulation = new PageSimulation();
            pageSimulationSettings = new PageSimulationSettings();
            pageGraphicSettings = new PageGraphicSettings();
            MainContent.NavigationUIVisibility = NavigationUIVisibility.Hidden; // Cache la barre de navigation du contenu
            Virus.Init();
            Sim = new Simulation(30, 0.01, 100000);
            Sim.Interval = GlobalVariables.DEFAULT_INTERVAL;
            Sim.Iterate();
            Sim.Start();

            Sim.OnTickSP += new MyEventHandler(OnTimerTick);
        }

        //This is the actual method that will be assigned to the event handler
        //within the above class. This is where we perform an action once the
        //event has been triggered.
        static void OnTimerTick(object source, Simulation e)
        {
            Debug.WriteLine(e.GetData());
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
            MainContent.Navigate(pageGraphicSettings); // Switch avec SimulationSettings
        }
    }
}

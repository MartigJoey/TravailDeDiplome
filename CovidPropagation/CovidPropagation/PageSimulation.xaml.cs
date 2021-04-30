using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace CovidPropagation
{
    /// <summary>
    /// Logique d'interaction pour PageSimulation.xaml
    /// </summary>
    public partial class PageSimulation : Page
    {
        Legend legendPage;
        MainWindow mw;
        string data;
        public int SliderValue { get => (int)intervalSlider.Value; set => intervalSlider.Value = value; }
        public PageSimulation()
        {
            InitializeComponent();
            legendPage = new Legend();
            intervalSlider.Value = GlobalVariables.DEFAULT_INTERVAL;
            mw = ((MainWindow)Application.Current.MainWindow);
        }

        private void OpenLegendWindow_Click(object sender, RoutedEventArgs e)
        {
            legendPage.Show();
            legendPage.Focus();
        }

        private void IntervalSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            mw.Sim.Interval = Convert.ToInt32(intervalSlider.Maximum - intervalSlider.Value);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            mw.Sim.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            mw.Sim.Stop();
        }
    }
}

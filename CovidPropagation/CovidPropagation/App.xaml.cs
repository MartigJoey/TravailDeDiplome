using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;

namespace CovidPropagation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        string windowName;

        public string WindowName { get => windowName; set => windowName = value; }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = GetCurrentWindow();
            if (currentWindow.GetType() == typeof(MainWindow))
                Current.Shutdown();
            else
                GetCurrentWindow().Hide();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = GetCurrentWindow();
            if (currentWindow.WindowState == WindowState.Normal)
                currentWindow.WindowState = WindowState.Maximized;
            else
                currentWindow.WindowState = WindowState.Normal;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentWindow().WindowState = WindowState.Minimized;
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                GetCurrentWindow().DragMove();
        }

        private Window GetCurrentWindow()
        {
            return Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        }
    }
}

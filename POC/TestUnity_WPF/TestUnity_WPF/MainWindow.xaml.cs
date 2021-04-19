using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace TestUnity_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("User32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private Process process;
        private IntPtr unityHWND = IntPtr.Zero;

        private const int WM_ACTIVATE = 0x0006;
        private readonly IntPtr WA_ACTIVE = new IntPtr(1);
        private readonly IntPtr WA_INACTIVE = new IntPtr(0);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource source = PresentationSource.FromVisual(unityPanel) as HwndSource;
            IntPtr unityHandle = source.Handle;

            //Start embedded Unity Application
            process = new Process();
            process.StartInfo.FileName = @"C:\Users\schad\OneDrive\Bureau\TravailDeDiplome\POC\testWPF_Unity\testWPF_Unity.exe";
            process.StartInfo.Arguments = "-parentHWND " + unityHandle.ToInt32() + " " + Environment.CommandLine;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();

            if (process.WaitForInputIdle())
            {
                EnumChildWindows(unityHandle, WindowEnum, IntPtr.Zero);
                MoveWindow(unityHWND, 0, 0, (int)unityPanel.ActualWidth, (int)unityPanel.ActualHeight, true);
                ActivateUnityWindow();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveWindow(unityHWND, 100, 100, (int)this.ActualWidth/3, (int)this.ActualHeight/3, true);
            ActivateUnityWindow();
        }

        private void ActivateUnityWindow()
        {
            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
        }

        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            unityHWND = hwnd;
            ActivateUnityWindow();
            return 0;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            process.CloseMainWindow();

            while (!process.HasExited)
                process.Kill();
        }
    }
}

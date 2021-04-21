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
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Text.Json;

namespace TestUnity_WPF
{
    public class WeatherForecastWithPOCOs
    {
        public DateTimeOffset Date { get; set; }
        public int TemperatureCelsius { get; set; }
        public string Summary { get; set; }
        public string SummaryField;
        public IList<DateTimeOffset> DatesAvailable { get; set; }
        public Dictionary<string, HighLowTemps> TemperatureRanges { get; set; }
        public string[] SummaryWords { get; set; }

        public WeatherForecastWithPOCOs()
        {
            SummaryWords = new string[] { "1", "3", "3" };
            TemperatureRanges = new Dictionary<string, HighLowTemps>();
            TemperatureRanges.Add("range1", new HighLowTemps());
        }
    }
    public class HighLowTemps
    {
        public int High { get; set; }
        public int Low { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
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
        StreamString ss;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ss != null)
            {
                Debug.WriteLine("In");
                WeatherForecastWithPOCOs testJson = new WeatherForecastWithPOCOs();
                string objectToSend = JsonSerializer.Serialize(testJson);
                Debug.WriteLine(objectToSend);
                await Task.Run(() =>
                {
                    // Invoke uniquement utile en cas d'utilisation du tbxValue.Text
                    Dispatcher.Invoke((Action)(() =>
                    {
                        ss.WriteString(objectToSend); // tbxValue.Text
                    }));
                });
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUnityExe();
            ConnectToUnity();
        }

        private void LoadUnityExe()
        {
            HwndSource source = PresentationSource.FromVisual(unityGrid) as HwndSource;
            IntPtr unityHandle = source.Handle;

            //Start embedded Unity Application
            process = new Process();
            process.StartInfo.FileName = @".\UnityBuild\testWPF_Unity.exe";
            process.StartInfo.Arguments = "-parentHWND " + unityHandle.ToInt32() + " " + Environment.CommandLine;
            //process.StartInfo.UseShellExecute = false;
            //process.StartInfo.CreateNoWindow = false;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();

            if (process.WaitForInputIdle())
            {
                EnumChildWindows(unityHandle, WindowEnum, IntPtr.Zero);
            }
        }

        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            unityHWND = hwnd;
            //ActivateUnityWindow();
            return 0;
        }
        private void ActivateUnityWindow()
        {
            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveWindow(unityHWND, 0, 0, (int)this.ActualWidth/3 * 2, (int)this.ActualHeight, true);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (ss != null)
            {
                ss.CloseLink();
            }
            
            //process.CloseMainWindow();
            //
            //while (!process.HasExited)
            //    process.Kill();
        }

        private static int numThreads = 1;
        private void ConnectToUnity()
        {
            Thread server;

            Debug.WriteLine("Waiting for client connect...\n");
            server = new Thread(ServerThread);
            server.Start();
            #region ouais
            //int i;
            //Thread.Sleep(250);
            //while (i > 0)
            //{
            //    for (int j = 0; j < numThreads; j++)
            //    {
            //        if (servers[j] != null)
            //        {
            //            if (servers[j].Join(250))
            //            {
            //                Debug.WriteLine("Server thread[{0}] finished.", servers[j].ManagedThreadId);
            //                servers[j] = null;
            //                i--;    // decrement the thread watch count
            //            }
            //        }
            //    }
            //}
            //Debug.WriteLine("\nServer threads exhausted, exiting.");
            #endregion
        }

        private void ServerThread(object data)
        {
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.Out, numThreads);
            //int threadId = Thread.CurrentThread.ManagedThreadId;

            pipeServer.WaitForConnection();

            //Debug.WriteLine("Client connected on thread[{0}].", threadId);
            Debug.WriteLine("Client connected.");
            try
            {
                Debug.WriteLine("Creating streamString...");
                ss = new StreamString(pipeServer);

                Debug.WriteLine("You can now write.");
            }
            catch (IOException e)
            {
                Debug.WriteLine("ERROR: {0}", e.Message);
            }
            //pipeServer.Close();
        }
    }

    public class StreamString
    {
        private BinaryWriter stream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream stream)
        {
            this.stream = new BinaryWriter(stream);
            streamEncoding = new UnicodeEncoding();
        }

        public async void WriteString(string outString)
        {
            await Task.Run(() => {
                byte[] outBuffer = streamEncoding.GetBytes(outString);
                int len = outBuffer.Length;

                List<byte> dataToSend = new List<byte>();
                dataToSend.Add((byte)(len >> 8));
                dataToSend.Add((byte)(len >> 0));
                dataToSend.AddRange(outBuffer.ToList());
                stream.Write(dataToSend.ToArray(), 0, dataToSend.Count);
                stream.Flush();
            });
            
        }

        public void CloseLink()
        {
            if (stream != null)
            {
                stream.Close();
            }
           
        }
    }
}

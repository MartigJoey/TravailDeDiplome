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

namespace TestUnity_WPF
{
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
                await Task.Run(() => ss.WriteString(tbxValue.Text));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //HwndSource source = PresentationSource.FromVisual(unityPanel) as HwndSource;
            //IntPtr unityHandle = source.Handle;
            //
            ////Start embedded Unity Application
            //process = new Process();
            //process.StartInfo.FileName = @"C:\Users\schad\OneDrive\Bureau\TravailDeDiplome\POC\testWPF_Unity\testWPF_Unity.exe";
            //process.StartInfo.Arguments = "-parentHWND " + unityHandle.ToInt32() + " " + Environment.CommandLine;
            //process.StartInfo.UseShellExecute = false;
            //process.StartInfo.CreateNoWindow = true;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            //process.Start();
            //
            //if (process.WaitForInputIdle())
            //{
            //    EnumChildWindows(unityHandle, WindowEnum, IntPtr.Zero);
            //}
            ConnectToUnity();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveWindow(unityHWND, 0, 0, (int)this.ActualWidth/3 * 2, (int)this.ActualHeight, true);
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
            ss.CloseLink();
            //process.CloseMainWindow();
            //
            //while (!process.HasExited)
            //    process.Kill();
        }

        private static int numThreads = 1;
        private void ConnectToUnity()
        {
            Thread server;

            Debug.WriteLine("\n*** Named pipe server stream with impersonation example ***\n");
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
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut, numThreads);

            int threadId = Thread.CurrentThread.ManagedThreadId;

            pipeServer.WaitForConnection();

            Debug.WriteLine("Client connected on thread[{0}].", threadId);
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
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            int len = 0;

            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        public void WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();
        }

        public void CloseLink()
        {
            ioStream.Close();
        }
    }

    public class ReadFileToStream
    {
        private string fn;
        private StreamString ss;

        public ReadFileToStream(StreamString str, string filename)
        {
            fn = filename;
            ss = str;
        }

        public void Start()
        {
            string contents = File.ReadAllText(fn);
            ss.WriteString(contents);
            Debug.WriteLine(contents);
        }
    }
}

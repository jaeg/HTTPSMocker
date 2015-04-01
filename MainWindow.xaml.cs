using System;
using System.Collections.Generic;
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
using System.Threading;
using System.IO;

namespace HTTPSMocker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread oThread;
        HttpMockServer server;
        static public MainWindow that;
        List<string> prefixes;
        public MainWindow()
        {
            InitializeComponent();
            MainWindow.that = this;
            Loaded += Window_Loaded;
 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] prefixFile = File.ReadAllLines("prefixes.txt");
                prefixes = new List<string>(prefixFile);
            }
            catch (Exception)
            {
                AddLog("No prefix.txt found so adding localhost by default");
                prefixes = new List<string>();
                prefixes.Add("http://localhost"); 
                prefixes.Add("http://127.0.0.1");  
            }

        }

        private void StartServiceButton_Click(object sender, RoutedEventArgs e)
        {
            server = new HttpMockServer(PortTextBox.Text, prefixes);
            oThread = new Thread(new ThreadStart(server.Start));

            oThread.Start();

            StartServiceButton.IsEnabled = false;
            StopServiceButton.IsEnabled = true;
        }

        private void StopServiceButton_Click(object sender, RoutedEventArgs e)
        {
            server.listener.Close();
            oThread.Abort();

            StartServiceButton.IsEnabled = true;
            StopServiceButton.IsEnabled = false;
        }

        public string GetResponse()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => GetResponse());
                //return "";
            }
            return ResponseTextBox.Text;
        }

        public void AddLog(string text)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => AddLog(text));
                return;
            }
            LogTextBox.Text += text + "\n";
            LogTextBox.ScrollToEnd();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (oThread != null)
            {
                if (oThread.IsAlive)
                {
                    server.listener.Close();
                    oThread.Abort();
                }
            }
        }


    }
}

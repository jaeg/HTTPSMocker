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
        public MainWindow()
        {
            InitializeComponent();
            MainWindow.that = this;
        }

        private void StartServiceButton_Click(object sender, RoutedEventArgs e)
        {
            server = new HttpMockServer(PortTextBox.Text);
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
            if (oThread.IsAlive)
            {
                server.listener.Close();
                oThread.Abort();
            }
        }


    }
}

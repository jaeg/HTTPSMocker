﻿using System;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartServiceButton_Click(object sender, RoutedEventArgs e)
        {
            server = new HttpMockServer(PortTextBox.Text, ResponseTextBox.Text);
            oThread = new Thread(new ThreadStart(server.Start));

            oThread.Start();
        }

        private void StopServiceButton_Click(object sender, RoutedEventArgs e)
        {
            server.listener.Close();
            oThread.Abort();
        }

  


    }
}
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        const string MOCK_CNTRL_NAME = "Mock Controller";

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if (value != errorMessage)
                {
                    errorMessage = value;
                }
            }
        }

        private int baudrate = 9600;
        public string Baudrate
        {
            get
            {
                return baudrate.ToString();
            }
            set
            {
                int ival;
                try
                {
                    ival = Int32.Parse(value);
                }
                catch (System.FormatException e)
                {
                    Trace.WriteLine(e.Message);
                    return;
                }
               
                if (ival <= 0)
                {
                    return;
                }
                baudrate = ival;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            ObservableCollection<string> list = new ObservableCollection<string>();

            list.Add(MOCK_CNTRL_NAME);
            foreach (string portName in System.IO.Ports.SerialPort.GetPortNames())
            {
                list.Add(portName);
            }
            UsbPortSelect.ItemsSource = list;
        }

        public void OnConnectClick(object sender, RoutedEventArgs e)
        {
            string selectedPort = UsbPortSelect.SelectedItem.ToString();

            if (selectedPort == MOCK_CNTRL_NAME)
            {
                MockController cntrl = new MockController();
                ControlWindow cntrlWindow = new ControlWindow(cntrl);
                cntrlWindow.Show();
                this.Close();
            }
            else
            {
                SerialController cntrl = new SerialController(selectedPort, baudrate);
                ControlWindow cntrlWindow = new ControlWindow(cntrl);
                cntrlWindow.Show();
                this.Close();
            }
        }
    }
}

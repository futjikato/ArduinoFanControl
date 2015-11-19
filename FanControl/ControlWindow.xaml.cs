using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for ControlWindow.xaml
    /// </summary>
    public partial class ControlWindow : Window
    {
        private SerialController serialCntrl;

        private FanControlDO fanDataObject;

        private FanUdpServer udpServer;

        public ControlWindow(SerialController controller)
        {
            serialCntrl = controller;
            InitializeComponent();

            fanDataObject = new FanControlDO(controller);
            this.DataContext = fanDataObject;

            SerialController.ResponseCallback fanCallback = ListFans;
            serialCntrl.Request(1, fanCallback);

            // start Udp server
            udpServer = new FanUdpServer();
            udpServer.Run(fanDataObject);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            udpServer.Stop();
        }

        public void ListFans(SerialController.Response resp)
        {
            int counter = 1;
            foreach (byte rByte in resp.answerBytes)
            {
                if (rByte == 0)
                {
                    return;
                }

                fanDataObject.setPinByte(counter, rByte);
                fanDataObject.setPinSpeed(counter, 0, false);
                counter++;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i <= 8; i++)
            {
                if (fanDataObject.GetPinOutput(i) != null)
                {
                    fanDataObject.setPinSpeed(i, 0, true);
                }
            }
        }
    }
}

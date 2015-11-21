using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp
{
    class FanUdpServer
    {
        private const int UDP_PORT = 11099;

        private Thread serverThread;

        private FanControlDO fanControlDataObject;

        private UdpClient listener;

        public void Run(FanControlDO fanData)
        {
            fanControlDataObject = fanData;
            serverThread = new Thread(serverLoop);
            serverThread.Start();
        }

        public void Stop()
        {
            Trace.WriteLine("Request server stop.");
            listener.Close();
        }

        protected void serverLoop()
        {
            Trace.WriteLine("Waiting for UDP messages.");
            listener = new UdpClient(UDP_PORT);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, UDP_PORT);
            byte[] receive_byte_array;

            bool running = true;
            while (running)
            {
                try
                {
                    receive_byte_array = listener.Receive(ref groupEP);
                    if (receive_byte_array.Length != 2)
                    {
                        Trace.WriteLine("Invalid UDP message received. Ignored message!");
                        continue;
                    }

                    Trace.WriteLine("Upp fan speed message received.");
                    int fan = receive_byte_array[0];
                    byte speed = receive_byte_array[1];
                    fanControlDataObject.setPinSpeed(fan, speed, true);
                }
                catch
                {
                    running = false;
                }
            }
        }
    }
}

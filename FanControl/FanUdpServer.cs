using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class FanUdpServer
    {
        private const int UDP_PORT = 11099;

        private System.Threading.Thread serverThread;

        private FanControlDO fanControlDataObject;

        public void Run(FanControlDO fanData)
        {
            fanControlDataObject = fanData;
            serverThread = new System.Threading.Thread(serverLoop);
            serverThread.Start();
        }

        public void Stop()
        {
            serverThread.Abort();
        }

        protected void serverLoop()
        {
            Trace.WriteLine("Waiting for UDP messages.");
            UdpClient listener = new UdpClient(UDP_PORT);
            listener.Client.ReceiveTimeout = 1000;
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, UDP_PORT);
            byte[] receive_byte_array;

            while (serverThread.IsAlive)
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
                catch (SocketException e)
                {
                    if(e.ErrorCode == 10060)
                    {
                        // timeout is ok
                        continue;
                    }
                    else
                    {
                        throw e;
                    }
                }
            }

            Trace.WriteLine("UDP server shut down!");
        }
    }
}

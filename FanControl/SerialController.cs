using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class SerialController
    {
        private SerialPort sPort;

        private readonly BackgroundWorker worker = new BackgroundWorker();

        public delegate void ResponseCallback(Response resp);

        public const byte REQUEST_FANLIST = 1;

        public const byte REQUEST_FANPOWER = 2;

        private Queue<Job> waitingQueue;

        private class Job
        {
            public byte MessageType = 0;

            public byte FanPort = 0;

            public byte FanSpeed = 0;

            public ResponseCallback FinishCallback;

            public Response response;
        }

        public class Response
        {
            public byte[] answerBytes;
        }

        public SerialController(string PortName, int Baudrate)
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = false;

            sPort = new SerialPort(PortName, Baudrate);
            waitingQueue = new Queue<Job>();
        }

        public void Request(byte requestByte, ResponseCallback responseCallback)
        {
            Request(requestByte, responseCallback, 0, 0);
        }

        public void Request(byte requestByte, ResponseCallback responseCallback, byte fan, byte speed)
        {
            Job j = new Job();
            j.MessageType = requestByte;
            j.FinishCallback = responseCallback;
            j.FanPort = fan;
            j.FanSpeed = speed;

            waitingQueue.Enqueue(j);
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync(j);
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Job j = (Job)e.Argument;

            sPort.Open();
            sPort.Write(new byte[] { j.MessageType, j.FanPort, j.FanSpeed }, 0, 3);

            LinkedList<byte> answerBytes = new LinkedList<byte>();
            byte rByte;
            while ((rByte = (byte)sPort.ReadByte()) != 0)
            {
                answerBytes.AddLast(rByte);
            }
            sPort.Close();

            Response resp = new Response();
            resp.answerBytes = answerBytes.ToArray<byte>();

            j.response = resp;

            e.Result = j;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Job j = (Job)e.Result;
            j.FinishCallback(j.response);

            waitingQueue.Dequeue();
            if (waitingQueue.Count > 0)
            {
                Job nextJob = waitingQueue.Peek();
                worker.RunWorkerAsync(nextJob);
            }
        }
    }
}

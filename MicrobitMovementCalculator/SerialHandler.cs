using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobitMovementCalculator
{
    public class SerialHandler
    {
        private static readonly Lazy<SerialHandler> lazy = new (() => new SerialHandler());
        public static SerialHandler Instance => lazy.Value;

        bool isOpened;
        SerialPort port;
        Thread readThread;

        List<string> recvMsgs;

        public bool IsOpened => isOpened;
        public List<string> ReceivedMessages => recvMsgs.ToList();

        public SerialHandler()
        {
            //readThread = new Thread(Read);
            port = new();
            recvMsgs = new();
            isOpened = false;
        }

        public void Init(string portname)
        {
            port.PortName = portname;
            port.BaudRate = 115200;
            port.Parity = Parity.None;
            port.DataBits = 8;
            port.StopBits = StopBits.One;
            port.Handshake = Handshake.XOnXOff;

            port.ReadTimeout = 500;
            port.WriteTimeout = 500;
        }

        public void Start()
        {
            if (!isOpened)
            {
                port.Open();
                isOpened = true;

                //if (readThread.ThreadState != System.Threading.ThreadState.Running)
                readThread = new Thread(Read);
                readThread.Start();
            }
        }

        private void Read()
        {
            while (isOpened)
            {
                try
                {
                    recvMsgs.Add(port.ReadLine());
                }
                catch (TimeoutException) { }
            }
        }

        public void ClearBuffer() =>
            recvMsgs.Clear();

        public void Close()
        {
            if (isOpened)
            {
                //readThread.Join();
                //readThread.Suspend();
                isOpened = false;
                Thread.Sleep(500);
                port.Close();
            }
        }
    }
}
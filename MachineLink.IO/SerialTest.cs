using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Timers;

namespace MachineLink.IO
{
    public class SerialTest
    {
        private SerialPort _port;
        private Timer _timeoutTimer;
        private StringBuilder _receivedBuilder;

        public bool IsDownloading { get; private set; }

        public string ReceivedProgram => _receivedBuilder.ToString();

        public SerialTest()
        {
            _port = new SerialPort
            {
                PortName="COM1",
                BaudRate=9400,
                DataBits=7,
                StopBits=StopBits.Two,
                Handshake=Handshake.None,
                RtsEnable=true,
                DtrEnable=true,
                Parity = Parity.Even
            };

            _port.DataReceived += _port_DataReceived;
        }

        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (IsDownloading)
            {
                HandleDownloading(e);
            }

            _timeoutTimer?.Dispose();

            _timeoutTimer = new Timer();
            _timeoutTimer.Interval = 5000;
            _timeoutTimer.Elapsed += _timeoutTimer_Elapsed;
            _timeoutTimer.Enabled = true;
            _timeoutTimer.Start();
        }

        private void HandleDownloading(SerialDataReceivedEventArgs e)
        {
            _receivedBuilder.Append(_port.ReadExisting());
        }

        private void _timeoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timeoutTimer.Dispose();

            if (IsDownloading)
            {
                IsDownloading = false;
            }
        }

        public void DownloadProgram()
        {
            _receivedBuilder = new StringBuilder();
            _port.Open();
            IsDownloading = true;
        }

    }
}

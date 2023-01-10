using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace MonitorMPS.Objects
{
    class ProgramSettings
    {
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public Parity Parity { get; set; }
        public string PortName2 { get; set; }
        public int BaudRate2 { get; set; }
        public int DataBits2 { get; set; }
        public StopBits StopBits2 { get; set; }
        public Parity Parity2 { get; set; }
        public int Frequency { get; set; }
        public int Channels { get; set; }
        public int Points { get; set; }
        public string Direcory { get; set; }
        public int EveryTooth { get; set; }

        public ProgramSettings()
        {
            Read();
        }

        public void Read()
        {
            PortName = Properties.Settings.Default.PortName;
            BaudRate = Properties.Settings.Default.BaudRate;
            DataBits = Properties.Settings.Default.DataBits;
            StopBits = Properties.Settings.Default.StopBits;
            Parity = Properties.Settings.Default.Parity;
            PortName2 = Properties.Settings.Default.PortName2;
            BaudRate2 = Properties.Settings.Default.BaudRate2;
            DataBits2 = Properties.Settings.Default.DataBits2;
            StopBits2 = Properties.Settings.Default.StopBits2;
            Parity2 = Properties.Settings.Default.Parity2;
            Frequency = Properties.Settings.Default.Frequency;
            Channels = Properties.Settings.Default.Channels;
            Points = Properties.Settings.Default.Points;
            Direcory = Properties.Settings.Default.Directory;
            EveryTooth = Properties.Settings.Default.EveryTooth;
        }

        public void Write()
        {
            Properties.Settings.Default.PortName = PortName;
            Properties.Settings.Default.BaudRate = BaudRate;
            Properties.Settings.Default.DataBits = DataBits;
            Properties.Settings.Default.StopBits = StopBits;
            Properties.Settings.Default.Parity = Parity;
            Properties.Settings.Default.PortName2 = PortName2;
            Properties.Settings.Default.BaudRate2 = BaudRate2;
            Properties.Settings.Default.DataBits2 = DataBits2;
            Properties.Settings.Default.StopBits2 = StopBits2;
            Properties.Settings.Default.Parity2 = Parity2;
            Properties.Settings.Default.Frequency = Frequency;
            Properties.Settings.Default.Points = Points;
            Properties.Settings.Default.Channels = Channels;
            Properties.Settings.Default.Directory = Direcory;
            Properties.Settings.Default.EveryTooth = EveryTooth;
            Properties.Settings.Default.Save();
        }
    }
}

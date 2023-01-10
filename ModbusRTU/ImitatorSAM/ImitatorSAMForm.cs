using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModbusSP;

namespace ImitatorSAM
{
    public partial class MainForm : Form
    {
        ModbusSerialPort _RS, _PAS, _UIT, _SYMAP;
        ushort _UIT_start_address, _SYMAP_start_address;
        public MainForm()
        {
            InitializeComponent();
            _PAS = new ModbusSerialPort();
            _PAS.PortName = "COM47";
            _PAS.BaudRate = 38400;
            _PAS.DataBits = 8;
            _PAS.StopBits = System.IO.Ports.StopBits.One;
            _PAS.Parity = System.IO.Ports.Parity.None;
            _PAS.Device = 2;
            _PAS.ReceivedPacket += PAS_ReceivedPacket;
            _RS = new ModbusSerialPort();
            _RS.PortName = "COM47";
            _RS.BaudRate = 38400;
            _RS.DataBits = 8;
            _RS.StopBits = System.IO.Ports.StopBits.One;
            _RS.Parity = System.IO.Ports.Parity.None;
            _RS.Device = 1;
            _RS.ReceivedPacket += RS_ReceivedPacket;
            _UIT = new ModbusSerialPort();
            _UIT.PortName = "COM47";
            _UIT.BaudRate = 38400;
            _UIT.DataBits = 8;
            _UIT.StopBits = System.IO.Ports.StopBits.One;
            _UIT.Parity = System.IO.Ports.Parity.None;
            _UIT.Device = 3;
            _UIT.ReceivedPacket += UIT_ReceivedPacket;
            _UIT_start_address = 0;
            _SYMAP = new ModbusSerialPort();
            _SYMAP.PortName = "COM47";
            _SYMAP.BaudRate = 38400;
            _SYMAP.DataBits = 8;
            _SYMAP.StopBits = System.IO.Ports.StopBits.One;
            _SYMAP.Parity = System.IO.Ports.Parity.None;
            _SYMAP.Device = 4;
            _SYMAP.ReceivedPacket += SYMAP_ReceivedPacket;
            _SYMAP_start_address = 43;
        }
        //--------------------------------------------------------------------------------
        public void PAS_ReceivedPacket(ModbusSerialPort port, byte command, UInt16[] values, int length)
        {

        }
        //--------------------------------------------------------------------------------
        public void RS_ReceivedPacket(ModbusSerialPort port, byte command, UInt16[] values, int length)
        {

        }
        //--------------------------------------------------------------------------------
        public void UIT_ReceivedPacket(ModbusSerialPort port, byte command, UInt16[] values, int length)
        {
            if (_UIT_start_address == 50)
                _UIT_start_address = 0;
            else
                _UIT_start_address += 10;
        }
        //--------------------------------------------------------------------------------
        public void SYMAP_ReceivedPacket(ModbusSerialPort port, byte command, UInt16[] values, int length)
        {
            if (_SYMAP_start_address == 43)
                _SYMAP_start_address = 112;
            else
                if (_SYMAP_start_address == 112)
                    _SYMAP_start_address = 200;
                else
                    if (_SYMAP_start_address == 200)
                        _SYMAP_start_address = 43;
        }
        //--------------------------------------------------------------------------------
        private void PAS_TSBtn_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                _PAS.Close();
            else
            {
                _PAS.Open();
                if (SendTimer.Enabled == false)
                    SendTimer.Enabled = true;
            }
        }
        //--------------------------------------------------------------------------------
        private void RS_TSBtn_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                _RS.Close();
            else
            {
                _RS.Open();
                if (SendTimer.Enabled == false)
                    SendTimer.Enabled = true;
            }
        }
        //--------------------------------------------------------------------------------
        private void SendTimer_Tick(object sender, EventArgs e)
        {
            if (_RS.IsOpen)
            {
                _RS.Query_03(0x0D, 4);
            }
            if (_PAS.IsOpen)
            {
                _PAS.Query_04(0, 8);
            }
            if (_UIT.IsOpen)
            {
                if (_UIT_start_address == 50)
                    _PAS.Query_03(_UIT_start_address, 4);
                else
                    _PAS.Query_03(_UIT_start_address, 10);
            }
            if (_SYMAP.IsOpen)
            {
                if (_SYMAP_start_address == 43)
                    _SYMAP.Query_03(_SYMAP_start_address, 1);
                else
                    if (_SYMAP_start_address == 112)
                        _SYMAP.Query_03(_SYMAP_start_address, 31);
                    else
                        _SYMAP.Query_03(_SYMAP_start_address, 4);
            }
        }
        //--------------------------------------------------------------------------------
        private void UIT_TSBtn_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                _UIT.Close();
            else
            {
                _UIT.Open();
                if (SendTimer.Enabled == false)
                    SendTimer.Enabled = true;
            }
        }
        //--------------------------------------------------------------------------------
        private void SYMAP_TSBtn_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                _SYMAP.Close();
            else
            {
                _SYMAP.Open();
                if (SendTimer.Enabled == false)
                    SendTimer.Enabled = true;
            }
        }
        //--------------------------------------------------------------------------------
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace MonitorMPS.Objects
{
    //------------------------------------------------------------------------------
    public delegate void ModBusReceivePacketEventHandler(ModBusComPort sender, byte command, UInt16[] values, int length);
    //public delegate void PrintSerialDataCallback(string data);
    //------------------------------------------------------------------------------
    public class ModBusComPort : SerialPort
    {
        public event ModBusReceivePacketEventHandler RaiseModBusReceivePacketEvent;

        byte MB_DEVICE;
        int _index;
        int _length;
        UInt16 _CRC;
        byte[] _read_buffer;
        UInt16 _portion;
        //------------------------------------------------------------------------------
        public ModBusComPort()
        {
            MB_DEVICE = 1;
            _index = _length = 0;
            _CRC = 0;
            _read_buffer = new byte[20480];
        }
        //--------------------------------------------------------------------------------
        protected virtual void OnRaiseModBusReceivePacketEvent(byte command, UInt16[] values, int length)
        {
            ModBusReceivePacketEventHandler handler = RaiseModBusReceivePacketEvent;

            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, command, values, length);
            }
        }
        //------------------------------------------------------------------------------
        public void MB_SendQuery(byte device, byte function, UInt16 address, UInt16 parameter, UInt16[] values)
        {
            int length = 0;
            byte[] query = new byte[16];
            UInt16 _CRC = 0xffff;
            query[0] = device;
            query[1] = function;
            query[2] = (byte)((address & 0xff00) >> 8);
            query[3] = (byte)(address & 0xff);
            query[4] = (byte)((parameter & 0xff00) >> 8);
            query[5] = (byte)(parameter & 0xff);
            if ((function == 0x16) || (function == 0x71)) // function = 0x16
            {
                length = 2; // parameter;
                for (int i = 0; i < length; i++)
                {
                    query[6 + i * 2] = (byte)((values[i] & 0xff00) >> 8);
                    query[7 + i * 2] = (byte)(values[i] & 0xff);
                }
            }
            
            // _CRC
            for (int i = 0; i < 6 + length * 2; i++)
                _CRC = _CRC16(query[i], _CRC);
            query[6 + length * 2] = (byte)(_CRC & 0xff);
            query[7 + length * 2] = (byte)((_CRC & 0xff00) >> 8);

            _index = 0;
            // send
            Write(query, 0, 8 + length * 2);
            _length = 7; // Minimum length
        }
        //------------------------------------------------------------------------------
        public void MB_ParseInput(byte[] buffer, int length)
        {
            byte ch;
            for (int i = 0; i < length; i++)
            {
                ch = buffer[i];
                if (ch == MB_DEVICE)
                {
                    if ((_index == 0) || (_index >= _length))
                    {
                        _read_buffer[0] = MB_DEVICE;
                        _index = 1;
                        _CRC = _CRC16(ch, 0xffff);
                        continue;
                    }
                }
                // Not MB_DEVICE
                if (_index == 1)
                {
                    if ((ch == 0x03) || (ch == 0x04) || (ch == 0x06) || (ch == 0x07) || (ch == 0x0F) || (ch == 0x16) || (ch == 0x70) || (ch == 0x71))
                    {  // command
                        _read_buffer[1] = ch;
                        _index = 2;
                        _CRC = _CRC16(ch, _CRC);
                    }
                    else // not command
                        _index = 0;
                    continue;
                }
                if (_index == 2) // length
                {
                    if (ch == 0xF0) // long packet
                        _length = _portion * 2 + 5;
                    else
                        _length = ch + 5;
                }
                if (_index == _length - 2)
                {
                    _read_buffer[_index] = ch;
                    _index++;
                    continue;
                }
                if (_index == _length - 1)
                {
                    if (_CRC == _read_buffer[_index - 1] + (ch << 8))
                    {
                        _index = 0;
                        _CRC = 0;
                        DecodePacket(_read_buffer[1]);
                        continue;
                    }
                    else
                    {
                        _index = 0;
                        _CRC = 0;
                        continue;
                    }
                }
                _read_buffer[_index] = ch;
                _CRC = _CRC16(ch, _CRC);
                _index++;
                //break;
            }
        }
        //------------------------------------------------------------------------------
        UInt16 _CRC16(byte ch, UInt16 _CRC)
        {
            _CRC ^= ch;
            for (int i = 0; i < 8; i++)
                _CRC = (UInt16)(((_CRC & 0x0001) == 1) ? ((_CRC >> 1) ^ 0xA001) : (_CRC >> 1));
            return _CRC;
        }
        //------------------------------------------------------------------------------
        public void GetParameters()
        {
            MB_SendQuery(0x01, 0x03, 0x00, 6, null);
        }
        //------------------------------------------------------------------------------
        public void GetDigital()
        {
            MB_SendQuery(0x01, 0x07, 0x00, 0x00, null);
        }
        //------------------------------------------------------------------------------
        public void GetADC()
        {
            MB_SendQuery(0x01, 0x04, 0x00, 0x10, null);
        }
        //------------------------------------------------------------------------------
        public void SetDigital(UInt16 value)
        {
            MB_SendQuery(0x01, 0x0F, value, 0x00, null);
        }
        //------------------------------------------------------------------------------
        public void SetNumChannels(UInt16 value)
        {
            MB_SendQuery(0x01, 0x06, 0x02, value, null);
        }
        //------------------------------------------------------------------------------
        public void SetEveryTooth(UInt16 value)
        {
            MB_SendQuery(0x01, 0x06, 0x05, value, null);
        }
        //------------------------------------------------------------------------------
        public void SetFrequency(UInt32 value)
        {
            UInt16[] values = new UInt16[2];
            values[1] = (UInt16)((value & 0xffff0000) >> 16);
            values[0] = (UInt16)(value & 0xffff);
            MB_SendQuery(0x01, 0x16, 0x00, 0x02, values);
        }
        //------------------------------------------------------------------------------
        public void SetNumPoints(UInt32 value)
        {
            UInt16[] values = new UInt16[2];
            values[1] = (UInt16)((value & 0xffff0000) >> 16);
            values[0] = (UInt16)(value & 0xffff);
            MB_SendQuery(0x01, 0x16, 0x03, 0x02, values);
        }
        //------------------------------------------------------------------------------
        public void StartMeasurement()
        {
            MB_SendQuery(0x01, 0x70, 0, 0, null);
        }
        //------------------------------------------------------------------------------
        public void GetData(UInt16 channel, UInt32 point, UInt16 length)
        {
            _portion = length;
            UInt16[] values = new UInt16[2];
            values[0] = (UInt16)(point & 0xffff);
            values[1] = (UInt16)((point & 0xffff0000) >> 16);
            MB_SendQuery(0x01, 0x71, channel, length, values);
        }
        //------------------------------------------------------------------------------
        public void DecodePacket(byte command)
        {
            UInt16[] values = new UInt16[10000];
            int length = 0;
            switch (command)
            {
                case 0x03:
                    length = 6;
                    for (int i = 0; i < 6; i++)
                        values[i] = (UInt16)((_read_buffer[3 + i * 2] << 8) + _read_buffer[4 + i * 2]);
                    break;
                case 0x04:
                    length = 16;
                    for (int i = 0; i < 16; i++)
                        values[i] = (UInt16)((_read_buffer[3 + i * 2] << 8) + _read_buffer[4 + i * 2]);
                    break;
                case 0x06:
                    length = 1;
                    values[0] = (UInt16)((_read_buffer[3] << 8) + _read_buffer[4]);
                    break;
                case 0x07:
                    length = 1;
                    values[0] = (UInt16)((_read_buffer[3] << 8) + _read_buffer[4]);
                    values[1] = (UInt16)((_read_buffer[5] << 8) + _read_buffer[6]);
                    values[2] = (UInt16)((_read_buffer[7] << 8) + _read_buffer[8]);
                    break;
                case 0x0F:
                    length = 1;
                    values[0] = (UInt16)((_read_buffer[3] << 8) + _read_buffer[4]);
                    values[1] = (UInt16)((_read_buffer[5] << 8) + _read_buffer[6]);
                    values[2] = (UInt16)((_read_buffer[7] << 8) + _read_buffer[8]);
                    break;
                case 0x16:
                    length = 2;
                    values[0] = (UInt16)((_read_buffer[3] << 8) + _read_buffer[4]);
                    values[1] = (UInt16)((_read_buffer[5] << 8) + _read_buffer[6]);
                    break;
                case 0x70:
                    length = 7;
                    for (int i = 0; i < 7; i++)
                        values[i] = (UInt16)((_read_buffer[3 + i * 2] << 8) + _read_buffer[4 + i * 2]);
                    length = 2;
                    values[0] = (UInt16)((_read_buffer[3] << 8) + _read_buffer[4]);
                    values[1] = (UInt16)((_read_buffer[5] << 8) + _read_buffer[6]);
                    break;
                case 0x71:
                    length = _portion;
                    for (int i = 0; i < _portion; i++)
                        values[i] = (UInt16)((_read_buffer[3 + i * 2] << 8) + _read_buffer[4 + i * 2]);
                    break;
            }
            OnRaiseModBusReceivePacketEvent(command, values, length);
        }
    }
}

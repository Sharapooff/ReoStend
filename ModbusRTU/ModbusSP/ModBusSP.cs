using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ModbusSP
{
    //------------------------------------------------------------------------------
    public delegate void ModbusReceivePacketEventHandler(ModbusSerialPort sender, byte command, UInt16[] values, int length);
    //public delegate void PrintSerialDataCallback(string data);
    //------------------------------------------------------------------------------
    public class ModbusSerialPort : SerialPort
    {
        public event ModbusReceivePacketEventHandler ReceivedPacket;

        private byte _device;   // номер ведомого устройства
        public byte Device
        {
            set { _device = value; }
            get { return _device; }
        }
        private int _index;             // индекс принимаемого байта
        private int _length;            // длина принимаемого пакета
        private UInt16 _CRC;            // контрольная сумма
        private byte[] _read_buffer;    // буфер чтения
        private byte _function;         // код команды
        private UInt16 _portion;        // длина данных для большого массива
        //------------------------------------------------------------------------------
        public ModbusSerialPort()
        {
            _device = _function = 1;
            _index = _length = 0;
            _CRC = 0;
            _read_buffer = new byte[20480];
            DataReceived += new SerialDataReceivedEventHandler(DataReceviedHandler);            
        }
        //--------------------------------------------------------------------------------
        public void DataReceviedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int recvBytes = sp.BytesToRead;
            byte[] inputBuffer = new byte[20480];
            sp.Read(inputBuffer, 0, recvBytes);
            ParseInput(inputBuffer, recvBytes);
        }
        //--------------------------------------------------------------------------------
        protected virtual void OnRaiseModbusReceivePacketEvent(byte command, UInt16[] values, int length)
        {
            ModbusReceivePacketEventHandler handler = ReceivedPacket;

            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, command, values, length);
            }
        }
        //------------------------------------------------------------------------------
        public void SendQuery(byte device, byte function, UInt16 address, UInt16 parameter, UInt16[] values)
        {
            if (device == 3) //для УСТА
            {
                int length = 0;
                byte[] query = new byte[16];
                UInt16 _CRC = 0xffff;
                query[0] = device;
                query[1] = (byte)function;
                query[2] = (byte)2;
                query[3] = (byte)0;
                query[4] = (byte)parameter;  //0 или 1 или 2 ножи на месте вверх вниз
               
                // _CRC
                for (int i = 0; i < 5 + length * 2; i++)
                    _CRC = _CRC16(query[i], _CRC);
                query[5 + length * 2] = (byte)(_CRC & 0xff);
                query[6 + length * 2] = (byte)((_CRC & 0xff00) >> 8);

                _index = 0;
                // send
                Write(query, 0, 7 + length * 2);
            }
            else
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
                if ((function == 0x10) || (function == 0x71))
                {
                    if (function == 0x71)
                        length = 2;
                    else
                        length = parameter;
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
                //_length = 7; // Minimum length
            }
        }
        //------------------------------------------------------------------------------
        public void ParseInput(byte[] buffer, int length)
        {
            byte ch;
            for (int i = 0; i < length; i++)
            {
                ch = buffer[i];
               
                if (ch == _device)
                {
                    if ((_index == 0) || (_index >= _length))
                    {
                        _read_buffer[0] = _device;
                        _index = 1;
                        _CRC = _CRC16(ch, 0xffff);
                        continue;
                    }
                }
                // Not _device
                if (_index == 1)
                {
                    if ((ch == 0x03) || (ch == 0x04) || (ch == 0x06) || (ch == 0x10) || (ch == 0x70) || (ch == 0x71) || (ch == 0x05))
                    {  // command
                        _read_buffer[1] = _function = ch;
                        _index = 2;
                        _CRC = _CRC16(ch, _CRC);
                    }
                    else // not command
                        _index = 0;
                    continue;
                }
                if (_index == 2) // length
                {
                    //if ((_function == 0x03) || (_function == 0x04))
                    //    _length = ch + 5;
                    if (ch == 0xF0) // long packet
                        _length = _portion * 2 + 5;
                    else
                        if ((_function == 0x03) || (_function == 0x04) || (_function == 0x70))
                            _length = ch + 5;
                        else
                        {
                            if (_function == 0x05)
                            {
                                _length = 9;
                            }
                            else
                            {
                                _length = 8;
                            }
                        }
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
        public void Query_03(ushort address, ushort length)
        {
            SendQuery(_device, 0x03, address, length, null);
        }
        //------------------------------------------------------------------------------
        public void Query_04(ushort address, ushort length)
        {
            SendQuery(_device, 0x04, address, length, null);
        }
        //------------------------------------------------------------------------------
        public void Query_06(ushort address, ushort value)
        {
            SendQuery(_device, 0x06, address, value, null);
        }
        //------------------------------------------------------------------------------
        public void Query_05(ushort address, ushort value)
        {
            SendQuery(_device, 0x05, address, value, null);
        }
        //------------------------------------------------------------------------------
        public void Query_10(ushort address, ushort length, ushort[] values)
        {
            SendQuery(_device, 0x10, address, length, values); 
        }
        //-------------------------------------------------------------------------------
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
                case 0x05:
                    length = 4; //4 байта данных + 2 байта CRC
                    for (int i = 0; i < 3; i++)//3 числа по 2 байта каждое
                        values[i] = (UInt16)((_read_buffer[3 + i * 2] << 8) + _read_buffer[4 + i * 2]);
                    break;
                case 0x06:
                    length = 1;
                    values[0] = (UInt16)((_read_buffer[3] << 8) + _read_buffer[4]);
                    break;
                //case 0x07:
                //    length = 1;
                //    values[0] = (UInt16)((_read_buffer[3] << 8) + _read_buffer[4]);
                //    values[1] = (UInt16)((_read_buffer[5] << 8) + _read_buffer[6]);
                //    values[2] = (UInt16)((_read_buffer[7] << 8) + _read_buffer[8]);
                //    break;
                //case 0x0F:
                //    length = 1;
                //    values[0] = (UInt16)((_read_buffer[3] << 8) + _read_buffer[4]);
                //    values[1] = (UInt16)((_read_buffer[5] << 8) + _read_buffer[6]);
                //    values[2] = (UInt16)((_read_buffer[7] << 8) + _read_buffer[8]);
                //    break;
                case 0x10:
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
            OnRaiseModbusReceivePacketEvent(command, values, length);
        }
        // функции для МПС
        //------------------------------------------------------------------------------
        //public void GetParameters()
        //{
        //    SendQuery(0x01, 0x03, 0x00, 6, null);
        //}
        ////------------------------------------------------------------------------------
        //public void GetDigital()
        //{
        //    SendQuery(0x01, 0x07, 0x00, 0x00, null);
        //}
        //------------------------------------------------------------------------------
        //public void GetADC()
        //{
        //    SendQuery(0x01, 0x04, 0x00, 0x10, null);
        //}
        ////------------------------------------------------------------------------------
        //public void SetDigital(UInt16 value)
        //{
        //    SendQuery(0x01, 0x0F, value, 0x00, null);
        //}
        //------------------------------------------------------------------------------
        //public void SetNumChannels(UInt16 value)
        //{
        //    SendQuery(0x01, 0x06, 0x02, value, null);
        //}
        //------------------------------------------------------------------------------
        //public void SetEveryTooth(UInt16 value)
        //{
        //    SendQuery(0x01, 0x06, 0x05, value, null);
        //}
        //------------------------------------------------------------------------------
        //public void SetFrequency(UInt32 value)
        //{
        //    UInt16[] values = new UInt16[2];
        //    values[1] = (UInt16)((value & 0xffff0000) >> 16);
        //    values[0] = (UInt16)(value & 0xffff);
        //    SendQuery(0x01, 0x16, 0x00, 0x02, values);
        //}
        //------------------------------------------------------------------------------
        //public void SetNumPoints(UInt32 value)
        //{
        //    UInt16[] values = new UInt16[2];
        //    values[1] = (UInt16)((value & 0xffff0000) >> 16);
        //    values[0] = (UInt16)(value & 0xffff);
        //    SendQuery(0x01, 0x16, 0x03, 0x02, values);
        //}
        //------------------------------------------------------------------------------
        //public void StartMeasurement()
        //{
        //    SendQuery(_device, 0x70, 0, 0, null);
        //}
        //------------------------------------------------------------------------------
        public void GetData(UInt16 channel, UInt32 point, UInt16 length)
        {
            _portion = length;
            UInt16[] values = new UInt16[2];
            values[0] = (UInt16)(point & 0xffff);
            values[1] = (UInt16)((point & 0xffff0000) >> 16);
            SendQuery(_device, 0x71, channel, length, values);
        }
    }
}

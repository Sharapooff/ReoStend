using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MonitorMPS.Objects;
using System.IO.Ports;
using System.IO;
using ModbusSP;
//using System.Runtime.Serialization.Formatters.Binary;

namespace MonitorMPS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void PrintReceivedDataCallback(string data);
        delegate void PrintRegisterValuesCallback(int command, UInt16[] values);
        delegate void PutDigitalValuesCallback(UInt16[] values);

        ProgramSettings _settings;
        //ModBusComPort _com_port;
        ModbusSerialPort _com_port;
        SerialPort _debug_port;
        UInt16 _channel;
        UInt32 _point;
        UInt32 _points;
        UInt32 _frequency;
        UInt16 _portion;
        UInt16[][] _data;
        int _set_parameter;
        bool _read_results;
        DispatcherTimer _query_timer;
        int _repeat_query;
        //--------------------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            _com_port = new ModbusSerialPort();
            _com_port.ReceivedPacket += ModbusReceivedPacket;
            _debug_port = new SerialPort();
            _data = new UInt16[17][];
            _query_timer = new DispatcherTimer();
            _query_timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            _query_timer.Tick += new EventHandler(TimerTick);
            _repeat_query = 0;
            for (int i = 0; i < 17; i++)
                _data[i] = new UInt16[100000];
            _set_parameter = 0;
            _read_results = true;
            PrintSerialData("\n");
        }
        //--------------------------------------------------------------------------------
        private void TimerTick(object sender, EventArgs e)
        {
            _repeat_query++;
            if (_repeat_query == 5)
            {
                ((DispatcherTimer)sender).Stop();
                MessageBox.Show("Нет ответа на запросы данных", "Ошибка передачи результатов", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _com_port.GetData(_channel, _point, _portion);
        }
        //--------------------------------------------------------------------------------
        public void ModbusReceivedPacket(ModbusSerialPort sender, byte command, UInt16[] values, int length)
        {
            UInt32 errors;
            switch (command)
            {
                case 0x03:
                    for (int i = 0; i < length; i++)
                        PrintSerialData(String.Format("{0}: {1}\n", i, values[i]));
                    PrintRegisterValues(0x03, values);
                    break;
                case 0x04:
                    PrintSerialData("\n Результаты преобразования:\n");
                    for (int i = 0; i < length; i++)
                        PrintSerialData(String.Format("АЦП №{0} Канал №{1} = {2}\n", (i % 2) + 1, (i / 2) + 1 ,values[i]));
                    break;
                case 0x06:
                    PrintSerialData("Setted data\n");
                    //PrintRegisterValues(0x06, values);
                    _set_parameter = 0;
                    if (values[0] < 6)
                        _com_port.Query_03(0, 6);
                    else
                        _com_port.Query_04(0, 16);
                    break;
                //case 0x07:
                //case 0x0F:
                //    for (int i = 0; i < 3; i++)
                //        PrintSerialData(String.Format("{0}: {1}\n", i, values[i]));
                //    PutDigitalPins(values);
                //    break;
                case 0x10:
                    PrintSerialData("Setted data\n");
                    //PrintRegisterValues(0x16, values);
                    _set_parameter = 0;
                    _com_port.Query_03(0, 6);
                    break;
                case 0x70:
                    _frequency = (UInt32)((values[1] << 16) + values[0]);
                    _points = (UInt32)((values[4] << 16) + values[3]);
                    errors = (UInt32)((values[6] << 16) + values[5]);
                    PrintSerialData(String.Format("{0} - измерения закончены. Ошибок: {1}\n", DateTime.Now.ToLongTimeString(), errors));
                    _channel = 0;
                    _point = 0;
                    _portion = 10000;
                    if (_read_results)
                    {
                        _com_port.GetData(_channel, _point, _portion);
                        _query_timer.Start();
                    }
                    break;
                case 0x71:
                    _query_timer.Stop();
                    for (int i = 0; i < _portion; i++)
                        _data[_channel][_point + i] = values[i];
                        _channel++;
                        if (_channel == 17)
                        {
                            _channel = 0;
                            _point += _portion;
                            PrintSerialData(String.Format("{0} - принято: {1} срезов\n", DateTime.Now.ToLongTimeString(), _point));
                        }
                        if (_point < _points)
                        {
                            _com_port.GetData(_channel, _point, _portion);
                            _query_timer.Start();
                            _repeat_query = 0;
                        }
                        else
                        {
                            PrintSerialData(String.Format("{0} - передача данных закончена\n", DateTime.Now.ToLongTimeString()));
                            DateTime dt = DateTime.Now;
                            string filepath = String.Format("{0}\\Results\\{1:d} {2:00}_{3:00}", _settings.Direcory, dt, dt.Hour, dt.Minute);
                            if (!Directory.Exists(filepath))
                                Directory.CreateDirectory(filepath);
                            for (int i = 0; i < 17; i++)
                            {
                                int num;
                                if (i < 16)
                                    if (i % 2 == 0)
                                        num = (int)(i / 2) + 1;
                                    else
                                        num = (int)(i / 2) + 11;
                                else
                                    num = 19;
                                using (FileStream writer = new FileStream(String.Format("{0}\\channel_{1}.dat", filepath, num), FileMode.Create))
                                {
                                    writer.Write(BitConverter.GetBytes((UInt32)(i + 1)), 0, 4);
                                    writer.Write(BitConverter.GetBytes((UInt32)_points), 0, 4);
                                    writer.Write(BitConverter.GetBytes((UInt16)(_frequency / 1000)), 0, 2);
                                    for (int j = 0; j < _points; j++)
                                        writer.Write(BitConverter.GetBytes(_data[i][j]), 0, 2);
                                    byte[] end = { 0x18, 0 };
                                    writer.Write(end, 0, 2);
                                }
                            }
                            PrintSerialData(String.Format("{0} - данные записаны в каталог {1}\n", DateTime.Now.ToLongTimeString(), filepath));
                        }
                    break; 
            }
        }
        //--------------------------------------------------------------------------------
        private void LoadSettings()
        {
            _settings = new ProgramSettings();
            string[] ports = SerialPort.GetPortNames();
            foreach (string s in ports)
                PortName_CB.Items.Add(s);
            PortName_CB.SelectedIndex = PortName_CB.Items.IndexOf(_settings.PortName);
            foreach (Parity p in Enum.GetValues(typeof(Parity)))
                Parity_CB.Items.Add(p);
            Parity_CB.SelectedIndex = Parity_CB.Items.IndexOf(_settings.Parity);
            foreach (StopBits sb in Enum.GetValues(typeof(StopBits)))
                StopBits_CB.Items.Add(sb);
            StopBits_CB.SelectedIndex = StopBits_CB.Items.IndexOf(_settings.StopBits);
            DataBits_CB.Items.Add(5);
            DataBits_CB.Items.Add(6);
            DataBits_CB.Items.Add(7);
            DataBits_CB.Items.Add(8);
            DataBits_CB.SelectedIndex = DataBits_CB.Items.IndexOf(_settings.DataBits);
            Baudrate_CB.Items.Add(9600);
            Baudrate_CB.Items.Add(19200);
            Baudrate_CB.Items.Add(38400);
            Baudrate_CB.Items.Add(57600);
            Baudrate_CB.Items.Add(115200);
            Baudrate_CB.Items.Add(230400);
            Baudrate_CB.Items.Add(460800);
            Baudrate_CB.Items.Add(921600);
            Baudrate_CB.SelectedIndex = Baudrate_CB.Items.IndexOf(_settings.BaudRate);
            ports = SerialPort.GetPortNames();
            foreach (string s in ports)
                PortName_2_CB.Items.Add(s);
            PortName_2_CB.SelectedIndex = PortName_2_CB.Items.IndexOf(_settings.PortName2);
            foreach (Parity p in Enum.GetValues(typeof(Parity)))
                Parity_2_CB.Items.Add(p);
            Parity_2_CB.SelectedIndex = Parity_2_CB.Items.IndexOf(_settings.Parity2);
            foreach (StopBits sb in Enum.GetValues(typeof(StopBits)))
                StopBits_2_CB.Items.Add(sb);
            StopBits_2_CB.SelectedIndex = StopBits_2_CB.Items.IndexOf(_settings.StopBits2);
            DataBits_2_CB.Items.Add(5);
            DataBits_2_CB.Items.Add(6);
            DataBits_2_CB.Items.Add(7);
            DataBits_2_CB.Items.Add(8);
            DataBits_2_CB.SelectedIndex = DataBits_2_CB.Items.IndexOf(_settings.DataBits2);
            Baudrate_2_CB.Items.Add(9600);
            Baudrate_2_CB.Items.Add(19200);
            Baudrate_2_CB.Items.Add(38400);
            Baudrate_2_CB.Items.Add(57600);
            Baudrate_2_CB.Items.Add(115200);
            Baudrate_2_CB.SelectedIndex = Baudrate_2_CB.Items.IndexOf(_settings.BaudRate2);
            MeasFrequency_CB.Items.Add(1000);   // 1000 мкс
            MeasFrequency_CB.Items.Add(5000);   //  200 мкс
            MeasFrequency_CB.Items.Add(10000);  //  100 мкс
            MeasFrequency_CB.Items.Add(20000);  //   50 мкс
            MeasFrequency_CB.Items.Add(40000);  //   25 мкс
            MeasFrequency_CB.Items.Add(50000);  //   20 мкс
            MeasFrequency_CB.Items.Add(100000); //   10 мкс
            MeasFrequency_CB.SelectedIndex = MeasFrequency_CB.Items.IndexOf(_settings.Frequency);
            Points_CB.Items.Add(10000);
            Points_CB.Items.Add(20000);
            Points_CB.Items.Add(40000);
            Points_CB.Items.Add(50000);
            Points_CB.Items.Add(100000);
            Points_CB.Items.Add(200000);
            Points_CB.SelectedIndex = Points_CB.Items.IndexOf(_settings.Points);
            Channels_CB.Items.Add(6);
            Channels_CB.Items.Add(8);
            Channels_CB.Items.Add(12);
            Channels_CB.Items.Add(16);
            Channels_CB.Items.Add(20);
            Channels_CB.SelectedIndex = Channels_CB.Items.IndexOf(_settings.Channels);
            EveryTooth_CB.Items.Add(1);
            EveryTooth_CB.Items.Add(124);
            EveryTooth_CB.Items.Add(248);
            EveryTooth_CB.SelectedIndex = EveryTooth_CB.Items.IndexOf(_settings.EveryTooth);
            Directory_TB.Text = _settings.Direcory;
        }
        //--------------------------------------------------------------------------------
        private void Open_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (_com_port.IsOpen)
            {
                _com_port.Close();
                ((Button)sender).ToolTip = "Открыть порт";
                OpenBtn_Image.Source = new BitmapImage(new Uri(@"/Images/Connect.png", UriKind.Relative));
                OutDebug_Btn.IsEnabled = Start_Btn.IsEnabled = GetPins_Btn.IsEnabled = FrequencySet_Btn.IsEnabled = ChannelsSet_Btn.IsEnabled = PointsSet_Btn.IsEnabled = EveryTooth_Btn.IsEnabled = false;
                Pin_Convst_Ck.IsEnabled = Pin_Rd_Ck.IsEnabled = Pin_CS1_Ck.IsEnabled = Pin_CS2_Ck.IsEnabled = Pin_Reset_Ck.IsEnabled = false;
            }
            else
            {
                //_com_port = new ComPort(_settings.PortName, _settings.BaudRate, _settings.Parity, _settings.DataBits, _settings.StopBits);
                _com_port.PortName = _settings.PortName;
                _com_port.BaudRate = _settings.BaudRate;
                _com_port.Parity = _settings.Parity;
                _com_port.DataBits = _settings.DataBits;
                _com_port.StopBits = _settings.StopBits;
                _com_port.DataReceived += new SerialDataReceivedEventHandler(DataReceviedHandler);
                _com_port.Open();
                ((Button)sender).ToolTip = "Закрыть порт";
                OpenBtn_Image.Source = new BitmapImage(new Uri(@"/Images/Disconnect.png", UriKind.Relative));
                OutDebug_Btn.IsEnabled = Start_Btn.IsEnabled = GetPins_Btn.IsEnabled = FrequencySet_Btn.IsEnabled = ChannelsSet_Btn.IsEnabled = PointsSet_Btn.IsEnabled = EveryTooth_Btn.IsEnabled = true;
                Pin_Convst_Ck.IsEnabled = Pin_Rd_Ck.IsEnabled = Pin_CS1_Ck.IsEnabled = Pin_CS2_Ck.IsEnabled = Pin_Reset_Ck.IsEnabled = true;
                _com_port.Query_03(0, 6); // читаем параметры (регистры 0 - 5)
                //_com_port.GetDigital();
            }

            if (_debug_port.IsOpen)
            {
                _debug_port.Close();
               // OutDebug_Image.Source = new BitmapImage(new Uri(@"/Images/button_ok.png", UriKind.Relative));
            }
            else
            {
                try
                {
                    _debug_port.PortName = _settings.PortName2;
                    _debug_port.BaudRate = _settings.BaudRate2;
                    _debug_port.Parity = _settings.Parity2;
                    _debug_port.DataBits = _settings.DataBits2;
                    _debug_port.StopBits = _settings.StopBits2;
                    _debug_port.DataReceived += new SerialDataReceivedEventHandler(DebugReceviedHandler);
                    _debug_port.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Communications error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //OutDebug_Image.Source = new BitmapImage(new Uri(@"/Images/button_cancel.png", UriKind.Relative));
            }
        }
        //--------------------------------------------------------------------------------
        public void DataReceviedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int recvBytes = sp.BytesToRead;
            byte[] inputBuffer = new byte[20480];
            //if (recvBytes > 1024)
            //    sp.ReadExisting();
            //else
            //{
                sp.Read(inputBuffer, 0, recvBytes);
                //for (int i = 0; i < recvBytes; i++)
                _com_port.ParseInput(inputBuffer, recvBytes);
            //}
        }
        //--------------------------------------------------------------------------------
        public void DebugReceviedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int recvBytes = sp.BytesToRead;
            byte[] inputBuffer = new byte[1024];
            if (recvBytes > 1024)
                sp.ReadExisting();
            else
            {
                sp.Read(inputBuffer, 0, recvBytes);
                PrintSerialData(System.Text.Encoding.Default.GetString(inputBuffer, 0, recvBytes));
            }
        }
        //--------------------------------------------------------------------------------
        public void PrintSerialData(string data)
        {
            if (Report_RTB.Dispatcher.CheckAccess())
            {
                Report_RTB.AppendText(data);
                Report_RTB.ScrollToEnd();
            }
            else
            {
                PrintReceivedDataCallback d = new PrintReceivedDataCallback(PrintSerialData);
                Report_RTB.Dispatcher.Invoke(d, new object[] { data });
            }
        }
        //--------------------------------------------------------------------------------
        public void PutDigitalPins(UInt16[] values)
        {
            if (Report_RTB.Dispatcher.CheckAccess())
            {
                Pin_Convst_Ck.IsChecked = ((values[0] & 0x01) > 0) ? true : false;
                Pin_Rd_Ck.IsChecked = ((values[0] & 0x02) > 0) ? true : false;
                Pin_CS1_Ck.IsChecked = ((values[0] & 0x04) > 0) ? true : false;
                Pin_CS2_Ck.IsChecked = ((values[0] & 0x08) > 0) ? true : false;
                Pin_Reset_Ck.IsChecked = ((values[0] & 0x10) > 0) ? true : false;
                Pin_Busy1_Ck.IsChecked = ((values[0] & 0x100) > 0) ? true : false;
                Pin_Busy2_Ck.IsChecked = ((values[0] & 0x200) > 0) ? true : false;
                Pin_DB0_1_Ck.IsChecked = ((values[1] & 0x01) > 0) ? true : false;
                Pin_DB1_1_Ck.IsChecked = ((values[1] & 0x02) > 0) ? true : false;
                Pin_DB2_1_Ck.IsChecked = ((values[1] & 0x04) > 0) ? true : false;
                Pin_DB3_1_Ck.IsChecked = ((values[1] & 0x08) > 0) ? true : false;
                Pin_DB4_1_Ck.IsChecked = ((values[1] & 0x10) > 0) ? true : false;
                Pin_DB5_1_Ck.IsChecked = ((values[1] & 0x20) > 0) ? true : false;
                Pin_DB6_1_Ck.IsChecked = ((values[1] & 0x40) > 0) ? true : false;
                Pin_DB7_1_Ck.IsChecked = ((values[1] & 0x80) > 0) ? true : false;
                Pin_DB8_1_Ck.IsChecked = ((values[1] & 0x100) > 0) ? true : false;
                Pin_DB9_1_Ck.IsChecked = ((values[1] & 0x200) > 0) ? true : false;
                Pin_DB10_1_Ck.IsChecked = ((values[1] & 0x400) > 0) ? true : false;
                Pin_DB11_1_Ck.IsChecked = ((values[1] & 0x800) > 0) ? true : false;
                Pin_DB12_1_Ck.IsChecked = ((values[1] & 0x1000) > 0) ? true : false;
                Pin_DB13_1_Ck.IsChecked = ((values[1] & 0x2000) > 0) ? true : false;
                Pin_DB0_2_Ck.IsChecked = ((values[2] & 0x01) > 0) ? true : false;
                Pin_DB1_2_Ck.IsChecked = ((values[2] & 0x02) > 0) ? true : false;
                Pin_DB2_2_Ck.IsChecked = ((values[2] & 0x04) > 0) ? true : false;
                Pin_DB3_2_Ck.IsChecked = ((values[2] & 0x08) > 0) ? true : false;
                Pin_DB4_2_Ck.IsChecked = ((values[2] & 0x10) > 0) ? true : false;
                Pin_DB5_2_Ck.IsChecked = ((values[2] & 0x20) > 0) ? true : false;
                Pin_DB6_2_Ck.IsChecked = ((values[2] & 0x40) > 0) ? true : false;
                Pin_DB7_2_Ck.IsChecked = ((values[2] & 0x80) > 0) ? true : false;
                Pin_DB8_2_Ck.IsChecked = ((values[2] & 0x100) > 0) ? true : false;
                Pin_DB9_2_Ck.IsChecked = ((values[2] & 0x200) > 0) ? true : false;
                Pin_DB10_2_Ck.IsChecked = ((values[2] & 0x400) > 0) ? true : false;
                Pin_DB11_2_Ck.IsChecked = ((values[2] & 0x800) > 0) ? true : false;
                Pin_DB12_2_Ck.IsChecked = ((values[2] & 0x1000) > 0) ? true : false;
                Pin_DB13_2_Ck.IsChecked = ((values[2] & 0x2000) > 0) ? true : false;
            }
            else
            {
                PutDigitalValuesCallback d = new PutDigitalValuesCallback(PutDigitalPins);
                Report_RTB.Dispatcher.Invoke(d, new object[] { values });
            }
        }
        //--------------------------------------------------------------------------------
        public void PrintRegisterValues(int command, UInt16[] values)
        {
            if (Report_RTB.Dispatcher.CheckAccess())
            {
                switch (command)
                {
                    case 0x03:
                        Frequency_L.Content = ((values[1] << 16) + values[0]).ToString();
                        Channels_L.Content = values[2].ToString();
                        Points_L.Content = ((values[4] << 16) + values[3]).ToString();
                        EveryTooth_L.Content = values[5].ToString();
                        break;
                    //case 0x06:
                    //    if (_set_parameter == 2)
                    //        Channels_L.Content = values[0].ToString();
                    //    else
                    //        EveryTooth_L.Content = values[0].ToString();
                    //    _set_parameter = 0;
                    //    break;
                    //case 0x10:
                    //    if (_set_parameter == 1)
                    //        Frequency_L.Content = ;
                    //    else
                    //        if (_set_parameter == 3)
                    //            Points_L.Content = Points_CB.Text;
                    //    _set_parameter = 0;
                    //    break;
                }
            }
            else
            {
                PrintRegisterValuesCallback d = new PrintRegisterValuesCallback(PrintRegisterValues);
                Report_RTB.Dispatcher.Invoke(d, new object[] { command, values });
            }
        }
        //--------------------------------------------------------------------------------
        private void SaveSettings_Btn_Click(object sender, RoutedEventArgs e)
        {
            _settings.BaudRate = (int)Baudrate_CB.SelectedItem;
            _settings.DataBits = (int)DataBits_CB.SelectedItem;
            _settings.StopBits = (StopBits)StopBits_CB.SelectedItem;
            _settings.PortName = (string)PortName_CB.SelectedItem;
            _settings.Parity = (Parity)Parity_CB.SelectedItem;
            _settings.BaudRate2 = (int)Baudrate_2_CB.SelectedItem;
            _settings.DataBits2 = (int)DataBits_2_CB.SelectedItem;
            _settings.StopBits2 = (StopBits)StopBits_2_CB.SelectedItem;
            _settings.PortName2 = (string)PortName_2_CB.SelectedItem;
            _settings.Parity2 = (Parity)Parity_2_CB.SelectedItem;
            _settings.Frequency = (int)MeasFrequency_CB.SelectedItem;
            _settings.Points = (int)Points_CB.SelectedItem;
            _settings.Direcory = Directory_TB.Text;
            _settings.Write();
        }
        //--------------------------------------------------------------------------------
        private void OutDebug_Btn_Click(object sender, RoutedEventArgs e)
        {
            _com_port.Query_03(0, 6); // читаем параметры (регистры 0 - 5)
        }
        //------------------------------------------------------------------------------
        private void FrequencySet_Btn_Click(object sender, RoutedEventArgs e)
        {
            UInt32 value = Convert.ToUInt32(MeasFrequency_CB.Text);
            UInt16[] values = new UInt16[2];
            values[1] = (UInt16)((value & 0xffff0000) >> 16);
            values[0] = (UInt16)(value & 0xffff);
            _com_port.Query_10(0, 2, values);
            //_com_port.SetFrequency(Convert.ToUInt32(MeasFrequency_CB.Text));
            _set_parameter = 1;
        }
        //------------------------------------------------------------------------------
        private void PointsSet_Btn_Click(object sender, RoutedEventArgs e)
        {
            UInt32 value = Convert.ToUInt32(Points_CB.Text);
            UInt16[] values = new UInt16[2];
            values[1] = (UInt16)((value & 0xffff0000) >> 16);
            values[0] = (UInt16)(value & 0xffff);
            _com_port.Query_10(3, 2, values);
            //_com_port.SetNumPoints(Convert.ToUInt32(Points_CB.Text));
            _set_parameter = 3;
        }
        //------------------------------------------------------------------------------
        private void ChannelsSet_Btn_Click(object sender, RoutedEventArgs e)
        {
            _com_port.Query_06(2, Convert.ToUInt16(Channels_CB.Text));
            _set_parameter = 2;
        }
        //----------------------------------------------------------------------------
        private void Start_Btn_Click(object sender, RoutedEventArgs e)
        {
            _com_port.SendQuery(_com_port.Device, 0x70, 0, 0, null);
        }
        //----------------------------------------------------------------------------
        private void GetDirectory_Btn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowNewFolderButton = true;
            dialog.Description = "Выберите каталог, в котором будут храниться файлы, необходимые для работы программы, и записываться результаты измерений";
            dialog.SelectedPath = _settings.Direcory;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Directory_TB.Text = dialog.SelectedPath;
            }
        }
        //----------------------------------------------------------------------------
        private void GetPins_Btn_Click(object sender, RoutedEventArgs e)
        {
            _com_port.Query_04(0, 16); // читаем все 16 каналов АЦП
        }
        //--------------------------------------------------------------------------
        private void SetDigitOutputs(object sender, RoutedEventArgs e)
        {
            UInt16 value = 0;
            value += (UInt16)(Pin_Convst_Ck.IsChecked.Value ? 1 : 0);
            value += (UInt16)(Pin_Rd_Ck.IsChecked.Value ? 2 : 0);
            value += (UInt16)(Pin_CS1_Ck.IsChecked.Value ? 4 : 0);
            value += (UInt16)(Pin_CS2_Ck.IsChecked.Value ? 8 : 0);
            value += (UInt16)(Pin_Reset_Ck.IsChecked.Value ? 16 : 0);
            _com_port.Query_06(6, value);
        }
        //-------------------------------------------------------------------------
        private void ReadResults_Ck_Checked(object sender, RoutedEventArgs e)
        {
            _read_results = ReadResults_Ck.IsChecked.Value;
        }
        //-------------------------------------------------------------------------
        private void ReadResults_Ck_Unchecked(object sender, RoutedEventArgs e)
        {
            _read_results = ReadResults_Ck.IsChecked.Value;
        }
        //-------------------------------------------------------------------------
        private void EveryTooth_Btn_Click(object sender, RoutedEventArgs e)
        {
            _com_port.Query_06(5, Convert.ToUInt16(EveryTooth_CB.Text));
            _set_parameter = 4;
        }
        //--------------------------------------------------------------------------
                      
    }
}

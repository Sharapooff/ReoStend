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

using ZedGraph;
using System.Collections;

namespace MonitorMeasDev
{
    public partial class MainForm : Form
    {
        delegate void PrintReceivedDataCallback(int device, string value);
        
        ModbusSerialPort _modbus_port;

        int _capacity = 151;/// Maximum queue size
        List<double> _data_A; /// Here store data
        List<double> _data_V; /// 
        List<double> _data_kVt; /// 
                                /// 
        //Строки файла CSV которые будем писать в файл
        string[] csvLines = new string[100000 + 1];//100001 элемент
        //счетчик записей
        int paket = 0;

        static bool _zapis { get; set; }//sign of recording in the parameters file from devices
        
        byte _poloj_noj = 0x00;       
        
        static bool rejim_automatic { get; set; }//control flag (automatic or manual)
      
        public MainForm()
        {
            InitializeComponent();
            _data_A = new List<double>();
            _data_V = new List<double>();
            _data_kVt = new List<double>();
            
            
            _modbus_port = new ModbusSerialPort();
            _modbus_port.BaudRate = 9600;
            _modbus_port.PortName = "COM" + Properties.Settings.Default.COM; ; //№com
            _modbus_port.Parity = System.IO.Ports.Parity.None;//no parity check
            _modbus_port.StopBits = System.IO.Ports.StopBits.One;
            _modbus_port.DataBits = 8;
            _modbus_port.ReceivedPacket += OnReceivedPacket;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            DrawGraph_AV_Clear();
            DrawGraph_A_Clear();
            DrawGraph_V_Clear();
            DrawGraph_kVt_Clear();                      
            
        }

        private void StartBtn_Click(object sender, EventArgs e)//connection to box
        {            
            if (!_modbus_port.IsOpen)
            {
                try
                {
                    _modbus_port.PortName = "COM" + Properties.Settings.Default.COM; //номер com порта     
                    MeasurementT.Interval = Properties.Settings.Default.Interval;
                    _modbus_port.Open();
                    MeasurementT.Enabled = true;
                    StartBtn.Text = "Отключиться от пульта";
                    StartBtn.BackColor = Color.GreenYellow;
                    button2.BackColor = Color.GreenYellow;
                    button3.BackColor = Color.GreenYellow;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("Отсутствует соединение с приборами пульта! Ошибка подключения к COM порту.");                    
                    StartBtn.Text = "Соединиться с пультом"; 
                    StartBtn.BackColor = Color.Red;
                    button2.BackColor = Color.Red;
                    button3.BackColor = Color.Red;
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                }
            }
            else
            {
                _modbus_port.Device = 3;
                _poloj_noj = 0x00;
                _modbus_port.Query_05(0, _poloj_noj);

                if (MeasurementT.Enabled == true)
                {
                    MeasurementT.Enabled = false; StartBtn.Text = "Соединиться с пультом";
                    StartBtn.BackColor = Color.Red;
                    button2.BackColor = Color.Red;
                    button3.BackColor = Color.Red;
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    //к кнопке запуска фиксации параметров
                    {
                        _zapis = false; button6.Text = "Начать запись параметров";
                       //??ОСТАНОВИТЬ ЗАПИСЬ ПАРАМЕТРОВ
                    }
                }
                else
                {
                    MeasurementT.Enabled = true; StartBtn.Text = "Отключиться от пульта";
                    StartBtn.BackColor = Color.GreenYellow;
                    button2.BackColor = Color.GreenYellow;
                    button3.BackColor = Color.GreenYellow;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
            }
        }

        //--------------------------------------------------------------------------------
        public void PrintModbusData(int device, string value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // if these threads are different, it returns true.
            if (this.ValueL.InvokeRequired)
            {
                try
                {
                    PrintReceivedDataCallback d = new PrintReceivedDataCallback(PrintModbusData);
                    this.Invoke(d, new object[] { device, value });
                }
                catch { }
            }
            else
            {
                if (device == 3)//teperature
                    label10.Text = String.Format("{0:####.#}", Convert.ToDouble(value));
                else
                {
                    if (device == 2)//voltmeter
                        ValueL.Text = String.Format("{0:####.#}", Convert.ToSingle(value));
                    else
                    {
                        if (device == 1)//ammeter
                        {
                            Value2L.Text = String.Format("{0:####.#}", Convert.ToSingle(value));
                        }
                        else
                        {
                            if (device == 4)
                            {//discrete outputs
                                label7.Text = value;
                            }
                            else
                            {
                                if (device == 5)
                                {//discrete inputs
                                    label14.Text = value;
                                    //___lower water level
                                    if (value.Remove(0,value.Length - 1) == "1")
                                    {
                                        ovalShape3.BackColor = Color.Red;
                                    }
                                    else
                                    {
                                        ovalShape3.BackColor = Color.Silver;
                                    }
                                    //___average water level
                                    if ((value.Remove(0, value.Length-2).Remove(1, 1)) == "1")
                                    {
                                        ovalShape2.BackColor = Color.Yellow;
                                    }
                                    else
                                    {
                                        ovalShape2.BackColor = Color.Silver;
                                    }
                                    //___upper water level
                                    if ((value.Remove(0, value.Length-3).Remove(1, 2)) == "1")
                                    {
                                        ovalShape1.BackColor = Color.LimeGreen;
                                    }
                                    else
                                    {
                                        ovalShape1.BackColor = Color.Silver;
                                    }
                                    //___pressed the button on the remote knives up
                                    if ((value.Remove(0, value.Length - 4).Remove(1, 3)) == "1")
                                    {
                                        button2.BackColor = Color.Red;
                                    }
                                    else
                                    {
                                        button2.BackColor = Color.GreenYellow;
                                    }
                                    //___pressed the button on the remote knives down
                                    if ((value.Remove(0, value.Length - 5).Remove(1, 4)) == "1")
                                    {
                                        button3.BackColor = Color.Red;
                                    }
                                    else
                                    {                                        
                                        button3.BackColor = Color.GreenYellow;
                                    }
                                }
                            }
                        }
                    }
                }
                if ((ValueL.Text != "0.0") && (Value2L.Text != "0.0")) 
                {
                    label3.Text = String.Format("{0:####.#}",((Convert.ToDouble(ValueL.Text) * Convert.ToDouble(Value2L.Text))/1000d));

                    // Add it to the end of the list
                    _data_V.Add(Convert.ToDouble(ValueL.Text));
                    _data_A.Add(Convert.ToDouble(Value2L.Text));
                    _data_kVt.Add(Convert.ToDouble(label3.Text));

                    // Delete the first item in the data list,
                    // if you have filled the maximum capacity
                    if (_data_A.Count > _capacity)
                    {
                        _data_A.RemoveAt(0);
                    }
                    if (_data_V.Count > _capacity)
                    {
                        _data_V.RemoveAt(0);
                    }
                    if (_data_kVt.Count > _capacity)
                    {
                        _data_kVt.RemoveAt(0);
                    }
                    //filtering of three arrays for smoothing
                  //  for (int i = 0; i < _data_A.Count-3; i++)
                  //  {
                  //      _data_A[i + 1] = 1 / 3 * (_data_A[i] + _data_A[i + 1] + _data_A[i + 2]);
                        //_data_A[i]=1/3*(_data_A[i+1]+_data_A[i]+_data_A[i-1]);
                        //_data_V[i]=1/3*(_data_V[i+1]+_data_V[i]+_data_V[i-1]);
                        //_data_kVt[i]=1/3*(_data_kVt[i+1]+_data_kVt[i]+_data_kVt[i-1]);
                 //   }
                    DrawGraph_A();
                    DrawGraph_V();
                    DrawGraph_kVt();
                    
                    
                }
            }
        }
        //--------------------------------------------------------------------------------
        
        public void OnReceivedPacket(ModbusSerialPort port, byte command, UInt16[]  values, int length)
        {
            byte[] buf = new byte[4];
            Single res;
            if (command == 0x04)
            {
                buf[1] = (byte)((values[0] & 0xff00) >> 8);
                buf[0] = (byte)(values[0] & 0xff);
                buf[3] = (byte)((values[1] & 0xff00) >> 8);
                buf[2] = (byte)(values[1] & 0xff);
                res = BitConverter.ToSingle(buf, 0);
                PrintModbusData(_modbus_port.Device, res.ToString());
            }
            else
            {
                if (command == 0x05)
                {
                    //temperature output
                    //values[1] = Convert.ToUInt16(values[1]*0.0625);//translation according to the passport of the temperature sensor

                    Int16 temper = Get_Int16_in_UInt16(values[1]);//because the sensor according to the protocol gives the value in UInt16 translate in Int16 checking the sign
                    Double temper_Double = Convert.ToDouble(temper * 0.0625);
                    PrintModbusData(_modbus_port.Device, (temper_Double.ToString()));
                    //discrete output
                    string result = string.Empty;
                    byte num = (byte)(values[0] & 0xff);

                    for (int i = sizeof(byte) * 8 - 1; i >= 0; i--)
                    {
                        if ((num & (1 << i)) != 0)
                            result += '1';
                        else
                            result += '0';
                    }
                    PrintModbusData(4, result);
                    //discrete inputs
                    result = "";
                    num = (byte)((values[0] & 0xff00) >> 8);
                    for (int i = sizeof(byte) * 8 - 1; i >= 0; i--)
                    {
                        if ((num & (1 << i)) != 0)
                            result += '1';
                        else
                            result += '0';
                    }
                    PrintModbusData(5, result);
                }
            }
        }
        //--------------------------------------------------------------------------------
        private void MeasurementT_Tick(object sender, EventArgs e)
        {
            //iterating through devices
            switch (_modbus_port.Device)
            {
                case 1:
                    _modbus_port.Device = 2;//amperage
                    break;
                case 2:
                    _modbus_port.Device = 3;//control box
                    break;
                case 3:
                    _modbus_port.Device = 1;//voltage
                    break;
            }            
            try
            {
                if (_modbus_port.Device != 3)
                {
                    _modbus_port.Query_04(0, 2);//SendQuery to control box
                }
                else
                {
                    //frst version
                    //_modbus_port.Query_05(0, _poloj_noj);//parametr 0x00 or 0x01 or 0x02

                    //automatic power leveling
                    if (rejim_automatic == true) //or (checkBox1.Checked == true)
                    {
                        int a = Convert.ToInt32(textBox1.Text), b = Convert.ToInt32(textBox2.Text);//diapason
                        int gr = a + b / 2;//mid diapason
                        int tok = Convert.ToInt32(Value2L.Text);//present amperage
                        if ((tok < a) || (tok > b))//when out of range
                        {                            
                            if (tok > gr)//
                            {
                                _poloj_noj = 0x01; //up plates - lower current
                            }
                            else
                            {
                                _poloj_noj = 0X02; //down plates - increase current
                            }
                        }
                        else
                        {
                            _poloj_noj = 0X00; //in place
                        }
                        _modbus_port.Query_05(0, _poloj_noj);//parametr 0x00 or 0x01 or 0x02 for SendQuery to control box
                    }
                    else //not automatic
                    {
                        _modbus_port.Query_05(0, _poloj_noj);//parametr 0x00 or 0x01 or 0x02 for SendQuery to control box
                    }
                }
            }
            catch
            {
                MeasurementT.Enabled = false;
                _modbus_port.Dispose();
                _modbus_port.Close();
                MessageBox.Show("Ошибка опроса приборов! Проверьте подключение!");                
            }
        }
        //--------------------------------------------------------------------------------
        private double f(double x, double h)
        {
            double _sin_x;
            _sin_x = Math.Sin(x);

            return Math.Sin(x) * Math.Cos(2.0 * x) + h;
        }
        //--------------------------------------------------------------------------------
        private void DrawGraph_A()
        {
            // Получим панель для рисования
            GraphPane pane = zedGraph_A.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list_A = new PointPairList();

            // !!! Изменим угол наклона меток по осям. Углы задаются в градусах
            //pane.XAxis.Scale.FontSpec.Angle = 90;
            // !!! Для оси Y 0 градусов означают, что надписи будут располагаться параллельно оси
            //pane.YAxis.Scale.FontSpec.Angle = 120;
            // Установим размеры шрифтов для подписей по осям
            //pane.XAxis.Title.FontSpec.Size = 8;
            //pane.YAxis.Title.FontSpec.Size = 8;
            // Установим размеры шрифтов для меток вдоль осей
            pane.XAxis.Scale.FontSpec.Size = 12;
            pane.YAxis.Scale.FontSpec.Size = 12;
            // Установим размеры шрифта для легенды
            pane.Legend.FontSpec.Size = 12;
            // Установим размеры шрифта для общего заголовка
            //pane.Title.FontSpec.Size = 30;
            //pane.Title.FontSpec.IsUnderline = true;
            // Горизонтальная линия на уровне y = 0 рисоваться не будет
            pane.YAxis.MajorGrid.IsZeroLine = false;
            // Изменим тест надписи по оси X
            pane.XAxis.Title.Text = "Точки измерений";
            pane.XAxis.Title.FontSpec.FontColor = Color.Black;//
            // Изменим текст по оси Y
            pane.YAxis.Title.Text = "Значения A";
            // Изменим текст заголовка графика
            pane.Title.Text = "Сигнал с амперметра";
            // Сделаем шрифт не полужирным
            //pane.Title.FontSpec.IsBold = false;
            // Включаем отображение сетки напротив крупных рисок по оси Y
            pane.YAxis.MajorGrid.IsVisible = true;            
            // Задаем вид пунктирной линии для крупных рисок по оси X:
            // Длина штрихов равна 10 пикселям, ...
            //pane.XAxis.MajorGrid.DashOn = 10;
            // затем 5 пикселей - пропуск
            //pane.XAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив крупных рисок по оси X
            pane.XAxis.MajorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.YAxis.MajorGrid.DashOn = 10;
            //pane.YAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.XAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси Y:
            // Длина штрихов равна одному пикселю, ...
            //pane.YAxis.MinorGrid.DashOn = 1;
            // затем 2 пикселя - пропуск
            //pane.YAxis.MinorGrid.DashOff = 2;
            // Включаем отображение сетки напротив мелких рисок по оси Y
         //   pane.XAxis.MinorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.XAxis.MinorGrid.DashOn = 1;
            //***
            // Установим цвет рамки для всего компонента
            pane.Border.Color = Color.Black;

            // Установим цвет рамки вокруг графика
            pane.Chart.Border.Color = Color.Green;

            // Закрасим фон всего компонента ZedGraph
            // Заливка будет сплошная
            pane.Fill.Type = FillType.Solid;
            pane.Fill.Color = Color.Black;

            // Закрасим область графика (его фон) в черный цвет
            pane.Chart.Fill.Type = FillType.Solid;
            pane.Chart.Fill.Color = Color.Black;

            // Включим показ оси на уровне X = 0 и Y = 0, чтобы видеть цвет осей
            pane.XAxis.MajorGrid.IsZeroLine = true;
            pane.YAxis.MajorGrid.IsZeroLine = true;
            // Установим цвет осей
            pane.XAxis.Color = Color.Gray;
            pane.YAxis.Color = Color.Gray;

            // Включим сетку
            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;
            // Установим цвет для сетки
            pane.XAxis.MajorGrid.Color = Color.Green;
            pane.YAxis.MajorGrid.Color = Color.Green;

            // Установим цвет для подписей рядом с осями
            pane.XAxis.Title.FontSpec.FontColor = Color.White;
            pane.YAxis.Title.FontSpec.FontColor = Color.White;

            // Установим цвет подписей под метками
            pane.XAxis.Scale.FontSpec.FontColor = Color.GreenYellow;
            pane.YAxis.Scale.FontSpec.FontColor = Color.GreenYellow;

            // Установим цвет заголовка над графиком
            pane.Title.FontSpec.FontColor = Color.LimeGreen;
            //***

            // Интервал, где есть данные
            double xmin = 0;
            double xmax = _capacity;

            // Расстояние между соседними точками по горизонтали
            double dx = 1.0;            
            double curr_x = 0;            
            DateTime timeStamp = DateTime.Now;
            // Заполняем список точек
            foreach (double val in _data_A)
            {
                list_A.Add(curr_x, val);
                curr_x += dx;
            }

            // Очистим список кривых от прошлых рисунков (кадров)
            pane.CurveList.Clear();
            LineItem Curve_V = pane.AddCurve("V", list_A, Color.Lime, SymbolType.None);//Yellow Tomato
            // Меняем толщину линии
            Curve_V.Line.Width = 2.0f;
            //скрыть легенду
            pane.Legend.IsVisible = false;

            //LineItem Curve_A = pane.AddCurve("A", list2, Color.LawnGreen, SymbolType.None);//LawnGreen YellowGreen
            // Меняем толщину линии
            //Curve_A.Line.Width = 2.0f;

            // Установим масштаб по умолчанию для оси X
            pane.XAxis.Scale.MinAuto = true;
            pane.XAxis.Scale.MaxAuto = true;

            // Установим масштаб по умолчанию для оси Y
            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;

            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = xmin;
            pane.XAxis.Scale.Max = xmax;

            // Устанавливаем интересующий нас интервал по оси Y
            //pane.YAxis.Scale.Min = _ymin;
            //pane.YAxis.Scale.Max = _ymax;

            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph_A.AxisChange();

            // Обновляем график
            zedGraph_A.Invalidate();
        }
        //--------------------------------------------------------------------------------
        private void DrawGraph_A_Clear()
        {
            // Получим панель для рисования
            GraphPane pane = zedGraph_A.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list_A = new PointPairList();

            // !!! Изменим угол наклона меток по осям. Углы задаются в градусах
            //pane.XAxis.Scale.FontSpec.Angle = 90;
            // !!! Для оси Y 0 градусов означают, что надписи будут располагаться параллельно оси
            //pane.YAxis.Scale.FontSpec.Angle = 120;
            // Установим размеры шрифтов для подписей по осям
            //pane.XAxis.Title.FontSpec.Size = 8;
            //pane.YAxis.Title.FontSpec.Size = 8;
            // Установим размеры шрифтов для меток вдоль осей
            pane.XAxis.Scale.FontSpec.Size = 12;
            pane.YAxis.Scale.FontSpec.Size = 12;
            // Установим размеры шрифта для легенды
            pane.Legend.FontSpec.Size = 12;
            // Установим размеры шрифта для общего заголовка
            //pane.Title.FontSpec.Size = 30;
            //pane.Title.FontSpec.IsUnderline = true;
            // Горизонтальная линия на уровне y = 0 рисоваться не будет
            pane.YAxis.MajorGrid.IsZeroLine = false;
            // Изменим тест надписи по оси X
            pane.XAxis.Title.Text = "Точки измерений";
            pane.XAxis.Title.FontSpec.FontColor = Color.Black;//
            // Изменим текст по оси Y
            pane.YAxis.Title.Text = "Значения I, A";
            // Изменим текст заголовка графика
            pane.Title.Text = "Сигнал с амперметра";
            // Сделаем шрифт не полужирным
            //pane.Title.FontSpec.IsBold = false;
            // Включаем отображение сетки напротив крупных рисок по оси Y
            pane.YAxis.MajorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси X:
            // Длина штрихов равна 10 пикселям, ...
            //pane.XAxis.MajorGrid.DashOn = 10;
            // затем 5 пикселей - пропуск
            //pane.XAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив крупных рисок по оси X
            pane.XAxis.MajorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.YAxis.MajorGrid.DashOn = 10;
            //pane.YAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.XAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси Y:
            // Длина штрихов равна одному пикселю, ...
            //pane.YAxis.MinorGrid.DashOn = 1;
            // затем 2 пикселя - пропуск
            //pane.YAxis.MinorGrid.DashOff = 2;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            //   pane.XAxis.MinorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.XAxis.MinorGrid.DashOn = 1;
            //***
            // Установим цвет рамки для всего компонента
            pane.Border.Color = Color.Black;

            // Установим цвет рамки вокруг графика
            pane.Chart.Border.Color = Color.Green;

            // Закрасим фон всего компонента ZedGraph
            // Заливка будет сплошная
            pane.Fill.Type = FillType.Solid;
            pane.Fill.Color = Color.Black;

            // Закрасим область графика (его фон) в черный цвет
            pane.Chart.Fill.Type = FillType.Solid;
            pane.Chart.Fill.Color = Color.Black;

            // Включим показ оси на уровне X = 0 и Y = 0, чтобы видеть цвет осей
            pane.XAxis.MajorGrid.IsZeroLine = true;
            pane.YAxis.MajorGrid.IsZeroLine = true;
            // Установим цвет осей
            pane.XAxis.Color = Color.Gray;
            pane.YAxis.Color = Color.Gray;

            // Включим сетку
            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;
            // Установим цвет для сетки
            pane.XAxis.MajorGrid.Color = Color.Green;
            pane.YAxis.MajorGrid.Color = Color.Green;

            // Установим цвет для подписей рядом с осями
            pane.XAxis.Title.FontSpec.FontColor = Color.White;
            pane.YAxis.Title.FontSpec.FontColor = Color.White;

            // Установим цвет подписей под метками
            pane.XAxis.Scale.FontSpec.FontColor = Color.GreenYellow;
            pane.YAxis.Scale.FontSpec.FontColor = Color.GreenYellow;

            // Установим цвет заголовка над графиком
            pane.Title.FontSpec.FontColor = Color.LimeGreen;
            //***

            // Очистим список кривых от прошлых рисунков (кадров)
            pane.CurveList.Clear();
            LineItem Curve_V = pane.AddCurve("V", list_A, Color.Lime, SymbolType.None);//Yellow Tomato
            // Меняем толщину линии
            Curve_V.Line.Width = 0.0f;
            //скрыть легенду
            pane.Legend.IsVisible = false;

            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = 0;
            pane.XAxis.Scale.Max = _capacity;

            // Устанавливаем интересующий нас интервал по оси Y
            pane.YAxis.Scale.Min = 0;
            pane.YAxis.Scale.Max = 50;//220

            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph_A.AxisChange();

            // Обновляем график
            zedGraph_A.Invalidate();
        }
        //--------------------------------------------------------------------------------
        private void DrawGraph_V()
        {
            // Получим панель для рисования
            GraphPane pane = zedGraph_V.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list_V = new PointPairList();


            // !!! Изменим угол наклона меток по осям. Углы задаются в градусах
            //pane.XAxis.Scale.FontSpec.Angle = 90;
            // !!! Для оси Y 0 градусов означают, что надписи будут располагаться параллельно оси
            //pane.YAxis.Scale.FontSpec.Angle = 120;
            // Установим размеры шрифтов для подписей по осям
            //pane.XAxis.Title.FontSpec.Size = 8;
            //pane.YAxis.Title.FontSpec.Size = 8;
            // Установим размеры шрифтов для меток вдоль осей
            pane.XAxis.Scale.FontSpec.Size = 12;
            pane.YAxis.Scale.FontSpec.Size = 12;
            // Установим размеры шрифта для легенды
            pane.Legend.FontSpec.Size = 12;
            // Установим размеры шрифта для общего заголовка
            //pane.Title.FontSpec.Size = 30;
            //pane.Title.FontSpec.IsUnderline = true;
            // Горизонтальная линия на уровне y = 0 рисоваться не будет
            pane.YAxis.MajorGrid.IsZeroLine = false;
            // Изменим тест надписи по оси X
            pane.XAxis.Title.Text = "Точки измерений";
            pane.XAxis.Title.FontSpec.FontColor = Color.Black;//
            // Изменим текст по оси Y
            pane.YAxis.Title.Text = "Значения V";
            // Изменим текст заголовка графика
            pane.Title.Text = "Сигнал с вольтметра";
            // Сделаем шрифт не полужирным
            //pane.Title.FontSpec.IsBold = false;
            // Включаем отображение сетки напротив крупных рисок по оси Y
            pane.YAxis.MajorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси X:
            // Длина штрихов равна 10 пикселям, ...
            //pane.XAxis.MajorGrid.DashOn = 10;
            // затем 5 пикселей - пропуск
            //pane.XAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив крупных рисок по оси X
            pane.XAxis.MajorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.YAxis.MajorGrid.DashOn = 10;
            //pane.YAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.XAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси Y:
            // Длина штрихов равна одному пикселю, ...
            //pane.YAxis.MinorGrid.DashOn = 1;
            // затем 2 пикселя - пропуск
            //pane.YAxis.MinorGrid.DashOff = 2;
            // Включаем отображение сетки напротив мелких рисок по оси Y
           // pane.XAxis.MinorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.XAxis.MinorGrid.DashOn = 1;
            //***
            // Установим цвет рамки для всего компонента
            pane.Border.Color = Color.Black;

            // Установим цвет рамки вокруг графика
            pane.Chart.Border.Color = Color.Green;

            // Закрасим фон всего компонента ZedGraph
            // Заливка будет сплошная
            pane.Fill.Type = FillType.Solid;
            pane.Fill.Color = Color.Black;

            // Закрасим область графика (его фон) в черный цвет
            pane.Chart.Fill.Type = FillType.Solid;
            pane.Chart.Fill.Color = Color.Black;

            // Включим показ оси на уровне X = 0 и Y = 0, чтобы видеть цвет осей
            pane.XAxis.MajorGrid.IsZeroLine = true;
            pane.YAxis.MajorGrid.IsZeroLine = true;
            // Установим цвет осей
            pane.XAxis.Color = Color.Gray;
            pane.YAxis.Color = Color.Gray;

            // Включим сетку
            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;
            // Установим цвет для сетки
            pane.XAxis.MajorGrid.Color = Color.Green;
            pane.YAxis.MajorGrid.Color = Color.Green;

            // Установим цвет для подписей рядом с осями
            pane.XAxis.Title.FontSpec.FontColor = Color.White;
            pane.YAxis.Title.FontSpec.FontColor = Color.White;

            // Установим цвет подписей под метками
            pane.XAxis.Scale.FontSpec.FontColor = Color.GreenYellow;
            pane.YAxis.Scale.FontSpec.FontColor = Color.GreenYellow;

            // Установим цвет заголовка над графиком
            pane.Title.FontSpec.FontColor = Color.LimeGreen;
            //***

            // Интервал, где есть данные
            double xmin = 0;
            double xmax = _capacity;

            // Расстояние между соседними точками по горизонтали
            double dx = 1.0;
            double curr_x = 0;
            DateTime timeStamp = DateTime.Now;
            // Заполняем список точек
            foreach (double val in _data_V)
            {
                list_V.Add(curr_x, val);
                curr_x += dx;
            }          

            // Очистим список кривых от прошлых рисунков (кадров)
            pane.CurveList.Clear();
            LineItem Curve_V = pane.AddCurve("V", list_V, Color.Lime, SymbolType.None);//Yellow Tomato
            // Меняем толщину линии
            Curve_V.Line.Width = 2.0f;
            //скрыть легенду
            pane.Legend.IsVisible = false;

            //LineItem Curve_A = pane.AddCurve("A", list2, Color.LawnGreen, SymbolType.None);//LawnGreen YellowGreen
            // Меняем толщину линии
            //Curve_A.Line.Width = 2.0f;
            // Установим масштаб по умолчанию для оси X
            pane.XAxis.Scale.MinAuto = true;
            pane.XAxis.Scale.MaxAuto = true;

            // Установим масштаб по умолчанию для оси Y
            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;

            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = xmin;
            pane.XAxis.Scale.Max = xmax;

            // Устанавливаем интересующий нас интервал по оси Y
            //pane.YAxis.Scale.Min = _ymin;
            //pane.YAxis.Scale.Max = _ymax;

            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph_V.AxisChange();

            // Обновляем график
            zedGraph_V.Invalidate();
        }
        //--------------------------------------------------------------------------------
        private void DrawGraph_V_Clear()
        {
            // Получим панель для рисования
            GraphPane pane = zedGraph_V.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list_V = new PointPairList();


            // !!! Изменим угол наклона меток по осям. Углы задаются в градусах
            //pane.XAxis.Scale.FontSpec.Angle = 90;
            // !!! Для оси Y 0 градусов означают, что надписи будут располагаться параллельно оси
            //pane.YAxis.Scale.FontSpec.Angle = 120;
            // Установим размеры шрифтов для подписей по осям
            //pane.XAxis.Title.FontSpec.Size = 8;
            //pane.YAxis.Title.FontSpec.Size = 8;
            // Установим размеры шрифтов для меток вдоль осей
            pane.XAxis.Scale.FontSpec.Size = 12;
            pane.YAxis.Scale.FontSpec.Size = 12;
            // Установим размеры шрифта для легенды
            pane.Legend.FontSpec.Size = 12;
            // Установим размеры шрифта для общего заголовка
            //pane.Title.FontSpec.Size = 30;
            //pane.Title.FontSpec.IsUnderline = true;
            // Горизонтальная линия на уровне y = 0 рисоваться не будет
            pane.YAxis.MajorGrid.IsZeroLine = false;
            // Изменим тест надписи по оси X
            pane.XAxis.Title.Text = "Точки измерений";
            pane.XAxis.Title.FontSpec.FontColor = Color.Black;//
            // Изменим текст по оси Y
            pane.YAxis.Title.Text = "Значения V,B";
            // Изменим текст заголовка графика
            pane.Title.Text = "Сигнал с вольтметра";
            // Сделаем шрифт не полужирным
            //pane.Title.FontSpec.IsBold = false;
            // Включаем отображение сетки напротив крупных рисок по оси Y
            pane.YAxis.MajorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси X:
            // Длина штрихов равна 10 пикселям, ...
            //pane.XAxis.MajorGrid.DashOn = 10;
            // затем 5 пикселей - пропуск
            //pane.XAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив крупных рисок по оси X
            pane.XAxis.MajorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.YAxis.MajorGrid.DashOn = 10;
            //pane.YAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.XAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси Y:
            // Длина штрихов равна одному пикселю, ...
            //pane.YAxis.MinorGrid.DashOn = 1;
            // затем 2 пикселя - пропуск
            //pane.YAxis.MinorGrid.DashOff = 2;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            // pane.XAxis.MinorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.XAxis.MinorGrid.DashOn = 1;
            //***
            // Установим цвет рамки для всего компонента
            pane.Border.Color = Color.Black;

            // Установим цвет рамки вокруг графика
            pane.Chart.Border.Color = Color.Green;

            // Закрасим фон всего компонента ZedGraph
            // Заливка будет сплошная
            pane.Fill.Type = FillType.Solid;
            pane.Fill.Color = Color.Black;

            // Закрасим область графика (его фон) в черный цвет
            pane.Chart.Fill.Type = FillType.Solid;
            pane.Chart.Fill.Color = Color.Black;

            // Включим показ оси на уровне X = 0 и Y = 0, чтобы видеть цвет осей
            pane.XAxis.MajorGrid.IsZeroLine = true;
            pane.YAxis.MajorGrid.IsZeroLine = true;
            // Установим цвет осей
            pane.XAxis.Color = Color.Gray;
            pane.YAxis.Color = Color.Gray;

            // Включим сетку
            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;
            // Установим цвет для сетки
            pane.XAxis.MajorGrid.Color = Color.Green;
            pane.YAxis.MajorGrid.Color = Color.Green;

            // Установим цвет для подписей рядом с осями
            pane.XAxis.Title.FontSpec.FontColor = Color.White;
            pane.YAxis.Title.FontSpec.FontColor = Color.White;

            // Установим цвет подписей под метками
            pane.XAxis.Scale.FontSpec.FontColor = Color.GreenYellow;
            pane.YAxis.Scale.FontSpec.FontColor = Color.GreenYellow;

            // Установим цвет заголовка над графиком
            pane.Title.FontSpec.FontColor = Color.LimeGreen;
            //***
                        
            // Очистим список кривых от прошлых рисунков (кадров)
            pane.CurveList.Clear();
            LineItem Curve_V = pane.AddCurve("V", list_V, Color.Lime, SymbolType.None);//Yellow Tomato
            // Меняем толщину линии
            Curve_V.Line.Width = 0.0f;
            //скрыть легенду
            pane.Legend.IsVisible = false;
                        
            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = 0;
            pane.XAxis.Scale.Max = _capacity;

            // Устанавливаем интересующий нас интервал по оси Y
            pane.YAxis.Scale.Min = 0;
            pane.YAxis.Scale.Max = 50;

            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph_V.AxisChange();

            // Обновляем график
            zedGraph_V.Invalidate();
        }
        //------------------------------------------------------------------------------
        private void DrawGraph_kVt()
        {
            // Получим панель для рисования
            GraphPane pane = zedGraph_kVt.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list_kVt = new PointPairList();


            // !!! Изменим угол наклона меток по осям. Углы задаются в градусах
            //pane.XAxis.Scale.FontSpec.Angle = 90;
            // !!! Для оси Y 0 градусов означают, что надписи будут располагаться параллельно оси
            //pane.YAxis.Scale.FontSpec.Angle = 120;
            // Установим размеры шрифтов для подписей по осям
            //pane.XAxis.Title.FontSpec.Size = 8;
            //pane.YAxis.Title.FontSpec.Size = 8;
            // Установим размеры шрифтов для меток вдоль осей
            pane.XAxis.Scale.FontSpec.Size = 12;
            pane.YAxis.Scale.FontSpec.Size = 12;
            // Установим размеры шрифта для легенды
            pane.Legend.FontSpec.Size = 12;
            // Установим размеры шрифта для общего заголовка
            //pane.Title.FontSpec.Size = 30;
            //pane.Title.FontSpec.IsUnderline = true;
            // Горизонтальная линия на уровне y = 0 рисоваться не будет
            pane.YAxis.MajorGrid.IsZeroLine = false;
            // Изменим тест надписи по оси X
            pane.XAxis.Title.Text = "Точки измерений";
            pane.XAxis.Title.FontSpec.FontColor = Color.Black;//
            // Изменим текст по оси Y
            pane.YAxis.Title.Text = "Значения Nву";
            // Изменим текст заголовка графика
            pane.Title.Text = "Сигнал Nву";
            // Сделаем шрифт не полужирным
            //pane.Title.FontSpec.IsBold = false;
            // Включаем отображение сетки напротив крупных рисок по оси Y
            pane.YAxis.MajorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси X:
            // Длина штрихов равна 10 пикселям, ...
            //pane.XAxis.MajorGrid.DashOn = 10;
            // затем 5 пикселей - пропуск
            //pane.XAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив крупных рисок по оси X
            pane.XAxis.MajorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.YAxis.MajorGrid.DashOn = 10;
            //pane.YAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.XAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси Y:
            // Длина штрихов равна одному пикселю, ...
            //pane.YAxis.MinorGrid.DashOn = 1;
            // затем 2 пикселя - пропуск
            //pane.YAxis.MinorGrid.DashOff = 2;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            // pane.XAxis.MinorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.XAxis.MinorGrid.DashOn = 1;
            //***
            // Установим цвет рамки для всего компонента
            pane.Border.Color = Color.Black;

            // Установим цвет рамки вокруг графика
            pane.Chart.Border.Color = Color.Green;

            // Закрасим фон всего компонента ZedGraph
            // Заливка будет сплошная
            pane.Fill.Type = FillType.Solid;
            pane.Fill.Color = Color.Black;

            // Закрасим область графика (его фон) в черный цвет
            pane.Chart.Fill.Type = FillType.Solid;
            pane.Chart.Fill.Color = Color.Black;

            // Включим показ оси на уровне X = 0 и Y = 0, чтобы видеть цвет осей
            pane.XAxis.MajorGrid.IsZeroLine = true;
            pane.YAxis.MajorGrid.IsZeroLine = true;
            // Установим цвет осей
            pane.XAxis.Color = Color.Gray;
            pane.YAxis.Color = Color.Gray;

            // Включим сетку
            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;
            // Установим цвет для сетки
            pane.XAxis.MajorGrid.Color = Color.Green;
            pane.YAxis.MajorGrid.Color = Color.Green;

            // Установим цвет для подписей рядом с осями
            pane.XAxis.Title.FontSpec.FontColor = Color.White;
            pane.YAxis.Title.FontSpec.FontColor = Color.White;

            // Установим цвет подписей под метками
            pane.XAxis.Scale.FontSpec.FontColor = Color.GreenYellow;
            pane.YAxis.Scale.FontSpec.FontColor = Color.GreenYellow;

            // Установим цвет заголовка над графиком
            pane.Title.FontSpec.FontColor = Color.LimeGreen;
            //***

            // Интервал, где есть данные
            double xmin = 0;
            double xmax = _capacity;

            // Расстояние между соседними точками по горизонтали
            double dx = 1.0;
            double curr_x = 0;
            DateTime timeStamp = DateTime.Now;
            // Заполняем список точек
            foreach (double val in _data_kVt)
            {
                list_kVt.Add(curr_x, val);
                curr_x += dx;
            }

            // Очистим список кривых от прошлых рисунков (кадров)
            pane.CurveList.Clear();
            LineItem Curve_V = pane.AddCurve("kVt", list_kVt, Color.Lime, SymbolType.None);//Yellow Tomato
            // Меняем толщину линии
            Curve_V.Line.Width = 2.0f;
            //скрыть легенду
            pane.Legend.IsVisible = false;

            //LineItem Curve_A = pane.AddCurve("A", list2, Color.LawnGreen, SymbolType.None);//LawnGreen YellowGreen
            // Меняем толщину линии
            //Curve_A.Line.Width = 2.0f;
            // Установим масштаб по умолчанию для оси X
            pane.XAxis.Scale.MinAuto = true;
            pane.XAxis.Scale.MaxAuto = true;

            // Установим масштаб по умолчанию для оси Y
            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;
            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = xmin;
            pane.XAxis.Scale.Max = xmax;

            // Устанавливаем интересующий нас интервал по оси Y
            //pane.YAxis.Scale.Min = _ymin;
            //pane.YAxis.Scale.Max = _ymax;

            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph_kVt.AxisChange();

            // Обновляем график
            zedGraph_kVt.Invalidate();
        }
        //--------------------------------------------------------------------------------
        private void DrawGraph_kVt_Clear()
        {
            // Получим панель для рисования
            GraphPane pane = zedGraph_kVt.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list_kVt = new PointPairList();
            
            // !!! Изменим угол наклона меток по осям. Углы задаются в градусах
            //pane.XAxis.Scale.FontSpec.Angle = 90;
            // !!! Для оси Y 0 градусов означают, что надписи будут располагаться параллельно оси
            //pane.YAxis.Scale.FontSpec.Angle = 120;
            // Установим размеры шрифтов для подписей по осям
            //pane.XAxis.Title.FontSpec.Size = 8;
            //pane.YAxis.Title.FontSpec.Size = 8;
            // Установим размеры шрифтов для меток вдоль осей
            pane.XAxis.Scale.FontSpec.Size = 12;
            pane.YAxis.Scale.FontSpec.Size = 12;
            // Установим размеры шрифта для легенды
            pane.Legend.FontSpec.Size = 12;
            // Установим размеры шрифта для общего заголовка
            //pane.Title.FontSpec.Size = 30;
            //pane.Title.FontSpec.IsUnderline = true;
            // Горизонтальная линия на уровне y = 0 рисоваться не будет
            pane.YAxis.MajorGrid.IsZeroLine = false;
            // Изменим тест надписи по оси X
            pane.XAxis.Title.Text = "Точки измерений";
            pane.XAxis.Title.FontSpec.FontColor = Color.Black;//
            // Изменим текст по оси Y
            pane.YAxis.Title.Text = "Значения Nву";
            // Изменим текст заголовка графика
            pane.Title.Text = "Сигнал Nву";
            // Сделаем шрифт не полужирным
            //pane.Title.FontSpec.IsBold = false;
            // Включаем отображение сетки напротив крупных рисок по оси Y
            pane.YAxis.MajorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси X:
            // Длина штрихов равна 10 пикселям, ...
            //pane.XAxis.MajorGrid.DashOn = 10;
            // затем 5 пикселей - пропуск
            //pane.XAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив крупных рисок по оси X
            pane.XAxis.MajorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.YAxis.MajorGrid.DashOn = 10;
            //pane.YAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.XAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси Y:
            // Длина штрихов равна одному пикселю, ...
            //pane.YAxis.MinorGrid.DashOn = 1;
            // затем 2 пикселя - пропуск
            //pane.YAxis.MinorGrid.DashOff = 2;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            // pane.XAxis.MinorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.XAxis.MinorGrid.DashOn = 1;
            //***
            // Установим цвет рамки для всего компонента
            pane.Border.Color = Color.Black;

            // Установим цвет рамки вокруг графика
            pane.Chart.Border.Color = Color.Green;

            // Закрасим фон всего компонента ZedGraph
            // Заливка будет сплошная
            pane.Fill.Type = FillType.Solid;
            pane.Fill.Color = Color.Black;

            // Закрасим область графика (его фон) в черный цвет
            pane.Chart.Fill.Type = FillType.Solid;
            pane.Chart.Fill.Color = Color.Black;

            // Включим показ оси на уровне X = 0 и Y = 0, чтобы видеть цвет осей
            pane.XAxis.MajorGrid.IsZeroLine = true;
            pane.YAxis.MajorGrid.IsZeroLine = true;
            // Установим цвет осей
            pane.XAxis.Color = Color.Gray;
            pane.YAxis.Color = Color.Gray;

            // Включим сетку
            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;
            // Установим цвет для сетки
            pane.XAxis.MajorGrid.Color = Color.Green;
            pane.YAxis.MajorGrid.Color = Color.Green;

            // Установим цвет для подписей рядом с осями
            pane.XAxis.Title.FontSpec.FontColor = Color.White;
            pane.YAxis.Title.FontSpec.FontColor = Color.White;

            // Установим цвет подписей под метками
            pane.XAxis.Scale.FontSpec.FontColor = Color.GreenYellow;
            pane.YAxis.Scale.FontSpec.FontColor = Color.GreenYellow;

            // Установим цвет заголовка над графиком
            pane.Title.FontSpec.FontColor = Color.LimeGreen;
            //***
          
            // Очистим список кривых от прошлых рисунков (кадров)
            pane.CurveList.Clear();
            LineItem Curve_V = pane.AddCurve("kVt", list_kVt, Color.Lime, SymbolType.None);//Yellow Tomato
            // Меняем толщину линии
            Curve_V.Line.Width = 2.0f;
            //скрыть легенду
            pane.Legend.IsVisible = false;

            //LineItem Curve_A = pane.AddCurve("A", list2, Color.LawnGreen, SymbolType.None);//LawnGreen YellowGreen
            // Меняем толщину линии
            //Curve_A.Line.Width = 2.0f;

            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = 0;
            pane.XAxis.Scale.Max = _capacity;

            // Устанавливаем интересующий нас интервал по оси Y
            pane.YAxis.Scale.Min = 0;
            pane.YAxis.Scale.Max = 50;

            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph_kVt.AxisChange();

            // Обновляем график
            zedGraph_kVt.Invalidate();
        }
        //------------------------------------------------------------------------------


        //--------------------------------------------------------------------------------

        // Создадим список точек
        PointPairList list_AV = new PointPairList();
        // Создадим список точек
        ///PointPairList list_point = new PointPairList();
        PointPairList list_green_point = new PointPairList();
        PointPairList list_red_point = new PointPairList();


        private void DrawGraph_AV(string SER, string PKM, PointPairList list_AV, PointPairList list_green_point, PointPairList list_red_point)
        {
            // Получим панель для рисования
            GraphPane pane_AV = zedGraph_AV.GraphPane;
            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane_AV.CurveList.Clear();
                       
            // Установим размеры шрифтов для меток вдоль осей
            pane_AV.XAxis.Scale.FontSpec.Size = 12;
            pane_AV.YAxis.Scale.FontSpec.Size = 12;
            // Установим размеры шрифта для легенды
            pane_AV.Legend.FontSpec.Size = 12;
            // Установим размеры шрифта для общего заголовка
            //pane.Title.FontSpec.Size = 30;
            //pane.Title.FontSpec.IsUnderline = true;
            // Горизонтальная линия на уровне y = 0 рисоваться не будет
            pane_AV.YAxis.MajorGrid.IsZeroLine = false;
            // Изменим тест надписи по оси X
            pane_AV.XAxis.Title.Text = "Значения A";
            pane_AV.XAxis.Title.FontSpec.FontColor = Color.Black;//
            // Изменим текст по оси Y
            pane_AV.YAxis.Title.Text = "Значения V";
            // Изменим текст заголовка графика
            pane_AV.Title.Text = "Поле допустимых режимов для внешних характеристик тепловоза " + SER + " на " + PKM;
            // Сделаем шрифт не полужирным
            //pane.Title.FontSpec.IsBold = false;
            // Включаем отображение сетки напротив крупных рисок по оси Y
            pane_AV.YAxis.MajorGrid.IsVisible = true;           
            // Включаем отображение сетки напротив крупных рисок по оси X
            pane_AV.XAxis.MajorGrid.IsVisible = true;        
            //***
            // Установим цвет рамки для всего компонента
            pane_AV.Border.Color = Color.Black;
            // Установим цвет рамки вокруг графика
            pane_AV.Chart.Border.Color = Color.Green;
            // Закрасим фон всего компонента ZedGraph
            // Заливка будет сплошная
            pane_AV.Fill.Type = FillType.Solid;
            pane_AV.Fill.Color = Color.Black;
            // Закрасим область графика (его фон) в черный цвет
            pane_AV.Chart.Fill.Type = FillType.Solid;
            pane_AV.Chart.Fill.Color = Color.Black;
            // Включим показ оси на уровне X = 0 и Y = 0, чтобы видеть цвет осей
            pane_AV.XAxis.MajorGrid.IsZeroLine = true;
            pane_AV.YAxis.MajorGrid.IsZeroLine = true;
            // Установим цвет осей
            pane_AV.XAxis.Color = Color.Gray;
            pane_AV.YAxis.Color = Color.Gray;
            // Включим сетку
            pane_AV.XAxis.MajorGrid.IsVisible = true;
            pane_AV.YAxis.MajorGrid.IsVisible = true;
            // Установим цвет для сетки
            pane_AV.XAxis.MajorGrid.Color = Color.Green;
            pane_AV.YAxis.MajorGrid.Color = Color.Green;
            // Установим цвет для подписей рядом с осями
            pane_AV.XAxis.Title.FontSpec.FontColor = Color.White;
            pane_AV.YAxis.Title.FontSpec.FontColor = Color.White;
            // Установим цвет подписей под метками
            pane_AV.XAxis.Scale.FontSpec.FontColor = Color.GreenYellow;
            pane_AV.YAxis.Scale.FontSpec.FontColor = Color.GreenYellow;
            // Установим цвет заголовка над графиком
            pane_AV.Title.FontSpec.FontColor = Color.LimeGreen;

            //списки точек передаются уже заполнеными

            // Очистим список кривых от прошлых рисунков (кадров)
            pane_AV.CurveList.Clear();
            LineItem Curve_V = pane_AV.AddCurve("AV", list_AV, Color.Lime, SymbolType.None);//Yellow Tomato
            // Меняем толщину линии
            Curve_V.Line.Width = 2.0f;
            //скрыть легенду
            pane_AV.Legend.IsVisible = false;
            // Создадим кривую с названием "Scatter".
            // Обводка ромбиков будут рисоваться голубым цветом (Color.Blue),
            // Опорные точки - ромбики (SymbolType.Diamond)
            LineItem greenCurve = pane_AV.AddCurve("GREEN_POINT", list_green_point, Color.Green, SymbolType.Diamond);
            // У кривой линия будет невидимой
            greenCurve.Line.IsVisible = false;
            // Цвет заполнения отметок (ромбиков)
            greenCurve.Symbol.Fill.Color = Color.GreenYellow;
            // Тип заполнения - сплошная заливка
            greenCurve.Symbol.Fill.Type = FillType.Solid;
            // Размер ромбиков
            greenCurve.Symbol.Size = 6;
            LineItem redCurve = pane_AV.AddCurve("RED_POINT", list_red_point, Color.OrangeRed, SymbolType.Diamond);
            // У кривой линия будет невидимой
            redCurve.Line.IsVisible = false;
            // Цвет заполнения отметок (ромбиков)
            redCurve.Symbol.Fill.Color = Color.OrangeRed;
            // Тип заполнения - сплошная заливка
            redCurve.Symbol.Fill.Type = FillType.Solid;
            // Размер ромбиков
            redCurve.Symbol.Size = 6;
            // Меняем толщину линии
            //Curve_A.Line.Width = 2.0f;
            // Устанавливаем интересующий нас интервал по оси X
            //pane.XAxis.Scale.Min = xmin;
            //pane.XAxis.Scale.Max = xmax;
            // Устанавливаем интересующий нас интервал по оси Y
            //pane.YAxis.Scale.Min = _ymin;
            //pane.YAxis.Scale.Max = _ymax;
            // Установим масштаб по умолчанию для оси X
            pane_AV.XAxis.Scale.MinAuto = true;
            pane_AV.XAxis.Scale.MaxAuto = true;
            // Установим масштаб по умолчанию для оси Y
            pane_AV.YAxis.Scale.MinAuto = true;
            pane_AV.YAxis.Scale.MaxAuto = true;
            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph_AV.AxisChange();
            // Обновляем график
            zedGraph_AV.Invalidate();
        }

        private void DrawGraph_AV_Clear()
        {
            // Получим панель для рисования
            GraphPane pane = zedGraph_AV.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list_AV = new PointPairList();
            
            // !!! Изменим угол наклона меток по осям. Углы задаются в градусах
            //pane.XAxis.Scale.FontSpec.Angle = 90;
            // !!! Для оси Y 0 градусов означают, что надписи будут располагаться параллельно оси
            //pane.YAxis.Scale.FontSpec.Angle = 120;
            // Установим размеры шрифтов для подписей по осям
            //pane.XAxis.Title.FontSpec.Size = 8;
            //pane.YAxis.Title.FontSpec.Size = 8;
            // Установим размеры шрифтов для меток вдоль осей
            pane.XAxis.Scale.FontSpec.Size = 12;
            pane.YAxis.Scale.FontSpec.Size = 12;
            // Установим размеры шрифта для легенды
            pane.Legend.FontSpec.Size = 12;
            // Установим размеры шрифта для общего заголовка
            //pane.Title.FontSpec.Size = 30;
            //pane.Title.FontSpec.IsUnderline = true;
            // Горизонтальная линия на уровне y = 0 рисоваться не будет
            pane.YAxis.MajorGrid.IsZeroLine = false;
            // Изменим тест надписи по оси X
            pane.XAxis.Title.Text = "I, A";
            pane.XAxis.Title.FontSpec.FontColor = Color.Black;//
            // Изменим текст по оси Y
            pane.YAxis.Title.Text = "V, B";
            // Изменим текст заголовка графика
            pane.Title.Text = "Поле допустимых режимов для внешних характеристик тепловоза";
            // Сделаем шрифт не полужирным
            //pane.Title.FontSpec.IsBold = false;
            // Включаем отображение сетки напротив крупных рисок по оси Y
            pane.YAxis.MajorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси X:
            // Длина штрихов равна 10 пикселям, ...
            //pane.XAxis.MajorGrid.DashOn = 10;
            // затем 5 пикселей - пропуск
            //pane.XAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив крупных рисок по оси X
            pane.XAxis.MajorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.YAxis.MajorGrid.DashOn = 10;
            //pane.YAxis.MajorGrid.DashOff = 5;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.XAxis.MinorGrid.IsVisible = true;
            // Включаем отображение сетки напротив мелких рисок по оси X
            //pane.YAxis.MinorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси Y:
            // Длина штрихов равна одному пикселю, ...
            //pane.YAxis.MinorGrid.DashOn = 1;
            // затем 2 пикселя - пропуск
            //pane.YAxis.MinorGrid.DashOff = 2;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            // pane.XAxis.MinorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            //pane.XAxis.MinorGrid.DashOn = 1;

            //***
            // Установим цвет рамки для всего компонента
            pane.Border.Color = Color.Black;

            // Установим цвет рамки вокруг графика
            pane.Chart.Border.Color = Color.Green;

            // Закрасим фон всего компонента ZedGraph
            // Заливка будет сплошная
            pane.Fill.Type = FillType.Solid;
            pane.Fill.Color = Color.Black;

            // Закрасим область графика (его фон) в черный цвет
            pane.Chart.Fill.Type = FillType.Solid;
            pane.Chart.Fill.Color = Color.Black;

            // Включим показ оси на уровне X = 0 и Y = 0, чтобы видеть цвет осей
            pane.XAxis.MajorGrid.IsZeroLine = true;
            pane.YAxis.MajorGrid.IsZeroLine = true;
            // Установим цвет осей
            pane.XAxis.Color = Color.Gray;
            pane.YAxis.Color = Color.Gray;

            // Включим сетку
            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;
            // Установим цвет для сетки
            pane.XAxis.MajorGrid.Color = Color.Green;
            pane.YAxis.MajorGrid.Color = Color.Green;

            // Установим цвет для подписей рядом с осями
            pane.XAxis.Title.FontSpec.FontColor = Color.White;
            pane.YAxis.Title.FontSpec.FontColor = Color.White;

            // Установим цвет подписей под метками
            pane.XAxis.Scale.FontSpec.FontColor = Color.GreenYellow;
            pane.YAxis.Scale.FontSpec.FontColor = Color.GreenYellow;

            // Установим цвет заголовка над графиком
            pane.Title.FontSpec.FontColor = Color.LimeGreen;

            // Для оси X установим календарный тип
            //      pane.XAxis.Type = AxisType.
            //***

            // Интервал, где есть данные
            //double xmin = 0;
            //double xmax = _capacity;


            // Заполняем список точек
            // for (int i = 1; i <= 15; i++)
            {

                //list_AV.Add(0.2+Math.Sin(i/2), 2.4+i);
              /*  list_AV.Add(0, 0);
                list_AV.Add(500, 27);
                list_AV.Add(1200, 27);
                list_AV.Add(1200, 0);
                list_AV.Add(1000, 0);
                list_AV.Add(1000, 24);
                list_AV.Add(500, 24);
                list_AV.Add(0, 24);
                list_AV.Add(0, 27);
*/

            }

            // Очистим список кривых от прошлых рисунков (кадров)
            pane.CurveList.Clear();
            LineItem Curve_V = pane.AddCurve("AV", list_AV, Color.Lime, SymbolType.None);//Yellow Tomato
            // Меняем толщину линии
            Curve_V.Line.Width = 0.0f;
            //скрыть легенду
            pane.Legend.IsVisible = false;

            

            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = 0;
            pane.XAxis.Scale.Max = 7500;

            // Устанавливаем интересующий нас интервал по оси Y
            //pane.YAxis.Scale.Min = _ymin;
            //pane.YAxis.Scale.Max = _ymax;

            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph_AV.AxisChange();

            // Обновляем график
            zedGraph_AV.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            if (comboBox1.Text != "Выберете серию")
            {
                comboBox2.Text = "ПКМ";
                if (comboBox1.Text == "2ТЭ116")
                {
                    comboBox2.Text = "1ПКМ";
                    comboBox2.Items.Add("1ПКМ"); comboBox2.Items.Add("2ПКМ"); comboBox2.Items.Add("3ПКМ"); comboBox2.Items.Add("4ПКМ"); comboBox2.Items.Add("5ПКМ");
                    comboBox2.Items.Add("7ПКМ"); comboBox2.Items.Add("11ПКМ"); comboBox2.Items.Add("15ПКМ");
                }
                if (comboBox1.Text == "2ТЭ116У")
                {
                    comboBox2.Text = "1ПКМ";
                    comboBox2.Items.Add("1ПКМ"); comboBox2.Items.Add("2ПКМ"); comboBox2.Items.Add("3ПКМ"); comboBox2.Items.Add("4ПКМ"); comboBox2.Items.Add("5ПКМ");
                    comboBox2.Items.Add("6ПКМ"); comboBox2.Items.Add("7ПКМ"); comboBox2.Items.Add("8ПКМ"); comboBox2.Items.Add("9ПКМ"); comboBox2.Items.Add("11ПКМ"); comboBox2.Items.Add("15ПКМ");
                }
            }
            
            list_red_point.Clear();//чистим список не входящих точек
            list_green_point.Clear();//чистим список входящих точек
            list_AV.Clear();

            list_AV = GET_LIST_AV(comboBox1.Text, comboBox2.Text);
            DrawGraph_AV(comboBox1.Text, comboBox2.Text, list_AV, list_green_point, list_red_point);
        }

        private PointPairList GET_LIST_AV(string ser, string pkm)
        {
            //list of points describing the diesel characteristic
            PointPairList list = new PointPairList();
            //choice of characteristics depending on the series and PKM
            // 2ТЭ116
            if (ser == "2ТЭ116")
            {
                switch(pkm)
                {
                    case "1ПКМ":
                        //list.Add(0, 35); list.Add(1225, 35);//list.Add(1225, 0); list.Add(1000, 0);//list.Add(1000, 24); list.Add(0, 24);//list.Add(0, 35);
                        for (int i = 0; i <= 1225; i++) { list.Add(Convert.ToDouble(i),35); }
                        for (int i = 35; i <= 0; i--) { list.Add(1225, Convert.ToDouble(i)); }                       
                        for (int i = 1225; i >= 1000; i--) { list.Add(Convert.ToDouble(i), 0); }
                        for (int i = 0; i <= 24; i++) { list.Add(1000, Convert.ToDouble(i)); }                       
                        for (int i = 1000; i >= 0; i--) { list.Add(Convert.ToDouble(i), 24); }
                        for (int i = 24; i <= 35; i++) { list.Add(0, Convert.ToDouble(i)); }                   
                        break;
                    case "2ПКМ":
                        //list.Add(0, 60); list.Add(2225, 60); list.Add(2225, 0); //list.Add(2000, 0);//list.Add(2000, 45); list.Add(0, 45);//list.Add(0, 60);
                        for (int i = 0; i <= 2225; i++) { list.Add(Convert.ToDouble(i), 60); }
                        for (int i = 60; i >= 0; i--) { list.Add(2225, Convert.ToDouble(i)); }
                        for (int i = 2225; i >= 2000; i--) { list.Add(Convert.ToDouble(i), 0); }     
                        for (int i = 0; i <= 45; i++) { list.Add(2000, Convert.ToDouble(i)); }
                        for (int i = 2000; i >= 0; i--) { list.Add(Convert.ToDouble(i), 45); }
                        for (int i = 45; i <= 60; i++) { list.Add(0, Convert.ToDouble(i)); }
                        break;
                    case "3ПКМ":
                        //list.Add(0, 100); list.Add(3400, 100);
                        //list.Add(3400, 0); list.Add(3000, 0);
                        //list.Add(3000, 80); list.Add(0, 80);
                        //list.Add(0, 100);
                        for (int i = 0; i <= 3400; i++) { list.Add(Convert.ToDouble(i), 100); }
                        for (int i = 100; i >= 0; i--) { list.Add(3400, Convert.ToDouble(i)); }
                        for (int i = 3400; i >= 3000; i--) { list.Add(Convert.ToDouble(i), 0); }
                        for (int i = 0; i<= 80; i++ ) { list.Add(3000, Convert.ToDouble(i)); }
                        for (int i = 3000; i >= 0; i--) { list.Add(Convert.ToDouble(i), 80); }
                        for (int i = 80; i <= 100; i++) { list.Add(0, Convert.ToDouble(i)); }
                        break;
                    case "4ПКМ":
                        list.Add(0, 408); list.Add(500, 398); list.Add(1000, 388); list.Add(1320, 382);
                        list.Add(1500, 344); list.Add(1750, 304);
                        list.Add(2000, 266); list.Add(2500, 212);
                        list.Add(3000, 172); list.Add(3500, 146);
                        list.Add(4000, 130); 
                        list.Add(4250, 124); 
                        list.Add(4380, 122); 
                        list.Add(4380, 0); list.Add(4000, 0); list.Add(4000, 52); list.Add(3500, 56); list.Add(3000, 68); list.Add(2500, 88); list.Add(2000, 110); list.Add(1500, 140);
                        list.Add(1250, 168); list.Add(1000, 208); list.Add(750, 284); list.Add(640, 336); list.Add(0, 336); list.Add(0, 408);
                        break;
                    case "5ПКМ":
                        list.Add(0, 376); list.Add(0, 456);
                        list.Add(500, 446); list.Add(1000, 436); list.Add(1470, 424); list.Add(1500, 414); list.Add(1750, 362); list.Add(2000, 314); list.Add(2250, 280); list.Add(2500, 254);
                        list.Add(2750, 232); list.Add(3000, 212); list.Add(3500, 180); list.Add(4000, 156); list.Add(4500, 142); list.Add(5000, 132); list.Add(5600, 120); list.Add(5600, 0);
                        list.Add(5000, 0); list.Add(5000, 68); list.Add(4500, 74); list.Add(4000, 82); list.Add(3500, 96); list.Add(3000, 110); list.Add(2750, 120); list.Add(2500, 132);
                        list.Add(2250, 148); list.Add(2000, 162); list.Add(1750, 184); list.Add(1500, 214); list.Add(1250, 266); list.Add(1000, 326); list.Add(860, 376);
                        list.Add(500, 376); list.Add(0, 376);
                        break;
                    case "7ПКМ":
                        list.Add(0, 436); list.Add(0, 504); list.Add(500, 496); list.Add(1000, 488); list.Add(1500, 479); list.Add(1760, 474); list.Add(1910, 436); list.Add(2000, 416);
                        list.Add(2250, 376); list.Add(2500, 340); list.Add(2750, 308); list.Add(3000, 282); list.Add(3250, 260); list.Add(3500, 238); list.Add(3750, 220); list.Add(4000, 204);
                        list.Add(4250, 192); list.Add(4500, 180); list.Add(4750, 170); list.Add(5000, 160); list.Add(5250, 152); list.Add(5500, 146); list.Add(5750, 142); list.Add(6000, 138);
                        list.Add(6500, 132); list.Add(6500, 0); list.Add(6000, 0); list.Add(6000, 90); list.Add(5750, 94); list.Add(5500, 98); list.Add(5250, 102); list.Add(5000, 106);
                        list.Add(4750, 112); list.Add(4500, 118); list.Add(4250, 127); list.Add(4000, 136); list.Add(3750, 148); list.Add(3500, 160); list.Add(3250, 172); list.Add(3000, 184);
                        list.Add(2750, 198); list.Add(2500, 215); list.Add(2250, 246); list.Add(2000, 276); list.Add(1750, 314); list.Add(1500, 364); list.Add(1280, 436); list.Add(1000, 436);
                        list.Add(0, 436);
                        break;
                    case "11ПКМ":
                        list.Add(0, 564); list.Add(0, 644); list.Add(500, 638); list.Add(1000, 632); list.Add(1500, 625); list.Add(2210, 614); list.Add(2250, 600); list.Add(2380, 564);
                        list.Add(2500, 540); list.Add(2750, 490); list.Add(3000, 452); list.Add(3250, 416); list.Add(3500, 390); list.Add(3750, 368); list.Add(4000, 344);

                        break;
                    case "15ПКМ":
                        list.Add(0, 564); list.Add(0, 644); list.Add(500, 638); list.Add(100, 632); list.Add(150, 625); list.Add(2210, 614); list.Add(225, 600); list.Add(238, 564);
                        list.Add(2500, 540); list.Add(2750, 490); list.Add(3000, 452); list.Add(3250, 416); list.Add(3500, 390); list.Add(3750, 368); list.Add(4000, 344);
                        
                        break;
                }
            }
            return (list);
        }
        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            list_red_point.Clear();//чистим список не входящих точек
            list_green_point.Clear();//чистим список входящих точек
            list_AV.Clear();
            list_AV = GET_LIST_AV(comboBox1.Text, comboBox2.Text);
            DrawGraph_AV(comboBox1.Text, comboBox2.Text, list_AV, list_green_point, list_red_point);
        }

        int pp = 40;
        
        private void button2_Click(object sender, EventArgs e)
        {
            /*
            if ((_poloj_noj == 0x00) || (_poloj_noj == 0x02))
            {
                _poloj_noj = 0x01;
                button2.BackColor = Color.Red;
                button3.BackColor = Color.GreenYellow;
            }
            else
            {
                _poloj_noj = 0x00;
                button2.BackColor = Color.GreenYellow;
            }         
             */

            //pp++; if (pp > 100) { pp = 100; }
            //DrawGraph_NOJI();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            /*
            if ((_poloj_noj == 0x00) || (_poloj_noj == 0x01))
            {
                _poloj_noj = 0x02;
                button3.BackColor = Color.Red;
                button2.BackColor = Color.GreenYellow;
            }
            else
            {
                _poloj_noj = 0x00;
                button3.BackColor = Color.GreenYellow;
            }
            */

            //pp--; if (pp < 0) { pp = 0; }
            //DrawGraph_NOJI();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Очистить график?", "Очистка графика",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                list_red_point.Clear();//clean the list of not entering points
                list_green_point.Clear();//clean the list of incoming points
                DrawGraph_AV(comboBox1.Text, comboBox2.Text, list_AV, list_green_point, list_red_point);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(Value2L.Text);
            double y = Convert.ToDouble(ValueL.Text);
            //list_green_point.Add(Convert.ToDouble(Value2L.Text), Convert.ToDouble(ValueL.Text)); list_red_point.Add(Convert.ToDouble(Value2L.Text), 50);
            //check if there are any points in the sheet (presence of a graph)
            if (list_AV.Count() > 0)
            {
                //for the minimum and maximum we take the first element of the list (x)
                double maxX = list_AV[0].X;
                double minX = list_AV[0].X;
                PointPairList list_y = new PointPairList(); //sheet for storing points of the graph for which X coincides with X points
                list_y.Clear();
                //look through all X elements of the list, starting from the 2nd (not from 0 but from 1 index) and find the maximum and minimum value of X
                for (int i = 0; i < list_AV.Count(); i++)
                {
                    if (maxX < list_AV[i].X)
                    {
                        maxX = list_AV[i].X;
                    }
                    if (minX > list_AV[i].X)
                    {
                        minX = list_AV[i].X;
                    }
                    //fill in the sheet of all points of the line Y
                    if (list_AV[i].X == x)
                    {
                        list_y.Add(list_AV[i]);
                    }
                }
                //if our point goes beyond the bounds of the values along the x axis, then draw it with a red
                if ((minX > x) || (maxX < x))
                {
                    list_red_point.Add(x, y);
                }
                else//if the point is in the range of the X axis, we look at the corresponding values of the point in Y
                {
                    if (list_y.Count > 0)
                    {
                        //for the minimum and maximum we take the first element of the list (y) (the point must be equal to or greater than the minimum or less than or equal to the maximum in Y)
                        double maxY = list_y[0].Y;
                        double minY = list_y[0].Y;
                        for (int i = 0; i < list_y.Count(); i++)
                        {
                            if (maxY < list_y[i].Y)
                            {
                                maxY = list_y[i].Y;
                            }
                            if (minY > list_y[i].Y)
                            {
                                minY = list_y[i].Y;
                            }
                        }
                        if ((y <= maxY) && (y >= minY)) { list_green_point.Add(x, y); }
                        else { list_red_point.Add(x, y); }
                    }
                    { list_red_point.Add(x, y); }
                }
            }
            else //if there is no graphics and you need to show a point, then draw it green
            {
                list_green_point.Add(x, y);
            }
            DrawGraph_AV(comboBox1.Text, comboBox2.Text, list_AV, list_green_point, list_red_point);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)//plate up
        {
            if ((_poloj_noj == 0x00) || (_poloj_noj == 0x02)) 
            {
                _poloj_noj = 0x01;
                button2.BackColor = Color.Red;
                button3.BackColor = Color.GreenYellow;
            }
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)//plate up
        {
            if ((_poloj_noj != 0x00) || (_poloj_noj != 0x02))           
            {
                _poloj_noj = 0x00;
                button2.BackColor = Color.GreenYellow;
            }
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)//plate down
        {
            if ((_poloj_noj == 0x00) || (_poloj_noj == 0x01))
            {
                _poloj_noj = 0x02;
                button3.BackColor = Color.Red;
                button2.BackColor = Color.GreenYellow;
            }
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)//plate down
        {
            if ((_poloj_noj != 0x00) || (_poloj_noj != 0x01))            
            {
                _poloj_noj = 0x00;
                button3.BackColor = Color.GreenYellow;
            }
        }

        private static byte BitArrayToByte(System.Collections.BitArray ba)//процедура заполнения 1 бита битами из BitArray
        {
            byte result = 0;
            for (byte index = 0, m = 1; index < 8; index++, m *= 2)
            {
                result += ba.Get(index) ? m : (byte)0;
            }
            return result;
        }

        private static Int16 BitArrayToInt16(System.Collections.BitArray ba)
        {
            Int16 result = 0;
            for (byte index = 0, m = 1; index < 16; index++, m *= 2)
            {
                result += ba.Get(index) ? m : (byte)0;

            }
            return result;
        }

        private Int16 Get_Int16_in_UInt16(UInt16 x)
        {
            Int16 _rez = 0;
            //выделение из 2 байтного числа байтов по отдельности
            Byte minor = Convert.ToByte(x & 0xFF); // младший
            Byte major = Convert.ToByte((x >> 8) & 0xFF);   // старший
            //если первый (старший) бит из старшего байта == 1, то число отрицательное (для знаковых чисел)
            if ((major & 0X80) != 0)
            {
                //каждый из байтов переводим в массив битов
                BitArray bt_minor = new BitArray(8);
                BitArray bt_major = new BitArray(8);
                bt_minor.SetAll(false); //или так bt1[0]=false;//первые 8 бит в 0
                bt_major.SetAll(false);
                //переводим сразу инвертируя все биты
                if ((minor & 0X01) == 0) { bt_minor.Set(0, true); }
                if ((minor & 0X02) == 0) { bt_minor.Set(1, true); }
                if ((minor & 0X04) == 0) { bt_minor.Set(2, true); }
                if ((minor & 0X08) == 0) { bt_minor.Set(3, true); }
                if ((minor & 0X10) == 0) { bt_minor.Set(4, true); }
                if ((minor & 0X20) == 0) { bt_minor.Set(5, true); }
                if ((minor & 0X40) == 0) { bt_minor.Set(6, true); }
                if ((minor & 0X80) == 0) { bt_minor.Set(7, true); }
                //0X01//0X02//0X04//0X08//0X10//020//0X40//0X80
                if ((major & 0X01) == 0) { bt_major.Set(0, true); }
                if ((major & 0X02) == 0) { bt_major.Set(1, true); }
                if ((major & 0X04) == 0) { bt_major.Set(2, true); }
                if ((major & 0X08) == 0) { bt_major.Set(3, true); }
                if ((major & 0X10) == 0) { bt_major.Set(4, true); }
                if ((major & 0X20) == 0) { bt_major.Set(5, true); }
                if ((major & 0X40) == 0) { bt_major.Set(6, true); }
                if ((major & 0X80) == 0) { bt_major.Set(7, true); }
                //0X01//0X02//0X04//0X08//0X10//020//0X40//0X80
                byte a = BitArrayToByte(bt_minor);
                byte b = BitArrayToByte(bt_major);
                //в массив байт пишем старший и младший байты
                byte[] buf = new byte[2];
                //полученные байты записываем в массив батов
                buf[0] = (byte)a;
                buf[1] = (byte)b;
                //из массива байт в двубайтовое число
                Int16 res2 = BitConverter.ToInt16(buf, 0);
                //добавляем один бит сконца и меняем знак           
                res2 = ++res2; res2 = Convert.ToInt16(-1 * res2);
                _rez = res2;
            }
            else//если старший бит != 1 (число не отрицательное), то просто его переписываем
            {
                _rez = Convert.ToInt16(x);
            }
            return _rez;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            
                //запись параметров вфайл
                if (_zapis == false)
                {
                   
                    _zapis = true;
                    button6.Text = "Остановить запись параметров";
                    //заголовок файла параметров
                    csvLines[paket] = "Дата/Время;Сила тока (А);Напряжение(В);Мощность (кВат);Температура (С)";
                    paket++;
                    timer1.Enabled = true;
                }
                else
                {
                    timer1.Enabled = false;
                    //файл в который пишем параметры после остановки записи
                    System.IO.File.WriteAllLines(@"D:\Показания приборов с пульта реостата_" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".csv", csvLines, System.Text.Encoding.GetEncoding(1251));
                    
                    //clear array CSV              
                    Array.Clear(csvLines, 0, csvLines.Length);
                    _zapis = false; button6.Text = "Начать запись параметров";
                }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //сохранение графика
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (this.Height == 399)
            {
                this.Width = 1408;
                this.Height = 778;
                button8.Text = "Свернуть окно";
            }
            else
            {
                this.Width = 1408;
                this.Height = 399;
                button8.Text = "Развернуть окно";
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            // Пропускаем цифровые кнопки
            if ((e.KeyCode >= Keys.D0) && (e.KeyCode <= Keys.D9)) e.SuppressKeyPress = false;
            // Пропускаем цифровые кнопки с NumPad'а
            if ((e.KeyCode >= Keys.NumPad0) && (e.KeyCode <= Keys.NumPad9)) e.SuppressKeyPress = false;
            // Пропускаем Delete, Back, Left и Right
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back) ||
                (e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right)) e.SuppressKeyPress = false;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            // Пропускаем цифровые кнопки
            if ((e.KeyCode >= Keys.D0) && (e.KeyCode <= Keys.D9)) e.SuppressKeyPress = false;
            // Пропускаем цифровые кнопки с NumPad'а
            if ((e.KeyCode >= Keys.NumPad0) && (e.KeyCode <= Keys.NumPad9)) e.SuppressKeyPress = false;
            // Пропускаем Delete, Back, Left и Right
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back) ||
                (e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right)) e.SuppressKeyPress = false;
        }

        int min_d = 0, d = 0; // диапазоны для поля допуска

        private void checkBox1_MouseDown(object sender, MouseEventArgs e)//range check
        {
            if (checkBox1.Checked == false)
            {
                //MessageBox.Show("false -> true");
                int a = 0, b = 0;
                try
                {
                    a = Convert.ToInt32(textBox1.Text);
                    b = Convert.ToInt32(textBox2.Text);
                    if ((a < b) && (a >= 0) && (b > 0))
                    {
                        checkBox1.Checked = true;
                        textBox1.Enabled = false;
                        textBox2.Enabled = false;
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка в формате или величине диапазона для тока!");
                }
            }
            else
            {
                //MessageBox.Show("true -> false");
                checkBox1.Checked = false;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) //change mode
        {
            if (_modbus_port.IsOpen)//if connection
            {
                if (checkBox1.Checked) //(automatically)
                {
                    button2.Enabled = false;
                    button3.Enabled = false;
                    rejim_automatic = true;
                }
                else //(buttons)
                {
                    button2.Enabled = true;
                    button3.Enabled = true;
                    rejim_automatic = false;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //can write parameters to a file                    
            csvLines[paket] = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ";" + Value2L.Text + ";" + ValueL.Text + ";" + label3.Text + ";" + label10.Text + ";";
            paket++;
        }

      



 
    }
}

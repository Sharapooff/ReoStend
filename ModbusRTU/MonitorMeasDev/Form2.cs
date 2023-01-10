using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonitorMeasDev
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.COM;
            textBox2.Text = Properties.Settings.Default.Interval.ToString();            
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.COM;
            textBox2.Text = Properties.Settings.Default.Interval.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.COM = textBox1.Text; //сохраняем в настройках порт
            Properties.Settings.Default.Interval = Convert.ToInt32(textBox2.Text);
            Properties.Settings.Default.Save();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            // Пропускаем цифровые кнопки
            if ((e.KeyCode >= Keys.D0) && (e.KeyCode <= Keys.D9)) e.SuppressKeyPress = false;
            // Пропускаем цифровые кнопки с NumPad'а
            if ((e.KeyCode >= Keys.NumPad0) && (e.KeyCode <= Keys.NumPad9)) e.SuppressKeyPress = false;
            // Пропускаем Delete, Back, Left и Righ
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

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
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

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }



    }
}

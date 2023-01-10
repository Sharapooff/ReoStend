namespace MonitorMeasDev
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.StartBtn = new System.Windows.Forms.Button();
            this.MeasurementT = new System.Windows.Forms.Timer(this.components);
            this.zedGraph_A = new ZedGraph.ZedGraphControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ValueL = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Value2L = new System.Windows.Forms.Label();
            this.zedGraph_kVt = new ZedGraph.ZedGraphControl();
            this.zedGraph_V = new ZedGraph.ZedGraphControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.ovalShape3 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.ovalShape2 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.ovalShape1 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.zedGraph_AV = new ZedGraph.ZedGraphControl();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartBtn
            // 
            this.StartBtn.BackColor = System.Drawing.Color.Red;
            this.StartBtn.ForeColor = System.Drawing.Color.Black;
            this.StartBtn.Image = ((System.Drawing.Image)(resources.GetObject("StartBtn.Image")));
            this.StartBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.StartBtn.Location = new System.Drawing.Point(1238, 290);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(119, 41);
            this.StartBtn.TabIndex = 0;
            this.StartBtn.Text = "Соединиться с пультом";
            this.StartBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.StartBtn.UseVisualStyleBackColor = false;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // MeasurementT
            // 
            this.MeasurementT.Interval = 75;
            this.MeasurementT.Tick += new System.EventHandler(this.MeasurementT_Tick);
            // 
            // zedGraph_A
            // 
            this.zedGraph_A.BackColor = System.Drawing.Color.Silver;
            this.zedGraph_A.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.zedGraph_A.Location = new System.Drawing.Point(5, 78);
            this.zedGraph_A.Name = "zedGraph_A";
            this.zedGraph_A.ScrollGrace = 0D;
            this.zedGraph_A.ScrollMaxX = 0D;
            this.zedGraph_A.ScrollMaxY = 0D;
            this.zedGraph_A.ScrollMaxY2 = 0D;
            this.zedGraph_A.ScrollMinX = 0D;
            this.zedGraph_A.ScrollMinY = 0D;
            this.zedGraph_A.ScrollMinY2 = 0D;
            this.zedGraph_A.Size = new System.Drawing.Size(338, 240);
            this.zedGraph_A.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.zedGraph_A);
            this.groupBox1.Controls.Add(this.zedGraph_kVt);
            this.groupBox1.Controls.Add(this.zedGraph_V);
            this.groupBox1.Controls.Add(this.StartBtn);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.shapeContainer1);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(8, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1380, 356);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.MenuText;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox2.ForeColor = System.Drawing.Color.Chartreuse;
            this.textBox2.Location = new System.Drawing.Point(242, 323);
            this.textBox2.MaxLength = 4;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(59, 26);
            this.textBox2.TabIndex = 110;
            this.textBox2.Text = "800";
            this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox1.Location = new System.Drawing.Point(15, 330);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 109;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            this.checkBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBox1_MouseDown);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(216, 328);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(24, 16);
            this.label17.TabIndex = 108;
            this.label17.Text = "до";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(31, 329);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(124, 16);
            this.label16.TabIndex = 107;
            this.label16.Text = "Диапазон тока от";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.MenuText;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.ForeColor = System.Drawing.Color.Chartreuse;
            this.textBox1.Location = new System.Drawing.Point(156, 323);
            this.textBox1.MaxLength = 4;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(59, 26);
            this.textBox1.TabIndex = 105;
            this.textBox1.Text = "400";
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label15.Location = new System.Drawing.Point(1214, 336);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(105, 13);
            this.label15.TabIndex = 104;
            this.label15.Text = "Дискретные входы";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Black;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.ForeColor = System.Drawing.Color.LawnGreen;
            this.label14.Location = new System.Drawing.Point(1319, 336);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(55, 13);
            this.label14.TabIndex = 103;
            this.label14.Text = "00000000";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label13.Location = new System.Drawing.Point(1037, 336);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(113, 13);
            this.label13.TabIndex = 101;
            this.label13.Text = "Дискретные выходы";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Black;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.Color.LawnGreen;
            this.label7.Location = new System.Drawing.Point(1150, 336);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 102;
            this.label7.Text = "00000000";
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Silver;
            this.button8.Location = new System.Drawing.Point(1045, 290);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(89, 40);
            this.button8.TabIndex = 100;
            this.button8.Text = "Развернуть окно";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Silver;
            this.button7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button7.Image = ((System.Drawing.Image)(resources.GetObject("button7.Image")));
            this.button7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button7.Location = new System.Drawing.Point(1045, 243);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(89, 41);
            this.button7.TabIndex = 99;
            this.button7.Text = "Настройки";
            this.button7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(1144, 243);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 41);
            this.button2.TabIndex = 19;
            this.button2.Text = "Поднятие пластин";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button2_MouseDown);
            this.button2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button2_MouseUp);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Location = new System.Drawing.Point(1029, 14);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(338, 61);
            this.panel4.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.Location = new System.Drawing.Point(262, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 31);
            this.label6.TabIndex = 13;
            this.label6.Text = "о";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label11.Location = new System.Drawing.Point(21, 13);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 31);
            this.label11.TabIndex = 12;
            this.label11.Text = "t =";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label12.Location = new System.Drawing.Point(285, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 31);
            this.label12.TabIndex = 11;
            this.label12.Text = "C";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Black;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.ForeColor = System.Drawing.Color.LawnGreen;
            this.label10.Location = new System.Drawing.Point(78, 2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 55);
            this.label10.TabIndex = 10;
            this.label10.Text = "0.0";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Red;
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(1144, 290);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(87, 41);
            this.button3.TabIndex = 20;
            this.button3.Text = "Опускание пластин";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            this.button3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button3_MouseDown);
            this.button3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button3_MouseUp);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ValueL);
            this.panel1.Location = new System.Drawing.Point(347, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(338, 62);
            this.panel1.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label8.Location = new System.Drawing.Point(20, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 31);
            this.label8.TabIndex = 13;
            this.label8.Text = "V =";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(280, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 31);
            this.label1.TabIndex = 5;
            this.label1.Text = "В";
            // 
            // ValueL
            // 
            this.ValueL.AutoSize = true;
            this.ValueL.BackColor = System.Drawing.Color.Black;
            this.ValueL.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ValueL.ForeColor = System.Drawing.Color.LawnGreen;
            this.ValueL.Location = new System.Drawing.Point(78, 2);
            this.ValueL.Name = "ValueL";
            this.ValueL.Size = new System.Drawing.Size(91, 55);
            this.ValueL.TabIndex = 4;
            this.ValueL.Text = "0.0";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.Value2L);
            this.panel2.Location = new System.Drawing.Point(5, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(338, 61);
            this.panel2.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label9.Location = new System.Drawing.Point(25, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 31);
            this.label9.TabIndex = 13;
            this.label9.Text = "I =";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(281, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 31);
            this.label2.TabIndex = 6;
            this.label2.Text = "A";
            // 
            // Value2L
            // 
            this.Value2L.AutoSize = true;
            this.Value2L.BackColor = System.Drawing.Color.Black;
            this.Value2L.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Value2L.ForeColor = System.Drawing.Color.LawnGreen;
            this.Value2L.Location = new System.Drawing.Point(78, 2);
            this.Value2L.Name = "Value2L";
            this.Value2L.Size = new System.Drawing.Size(91, 55);
            this.Value2L.TabIndex = 5;
            this.Value2L.Text = "0.0";
            // 
            // zedGraph_kVt
            // 
            this.zedGraph_kVt.BackColor = System.Drawing.Color.Silver;
            this.zedGraph_kVt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.zedGraph_kVt.Location = new System.Drawing.Point(687, 77);
            this.zedGraph_kVt.Name = "zedGraph_kVt";
            this.zedGraph_kVt.ScrollGrace = 0D;
            this.zedGraph_kVt.ScrollMaxX = 0D;
            this.zedGraph_kVt.ScrollMaxY = 0D;
            this.zedGraph_kVt.ScrollMaxY2 = 0D;
            this.zedGraph_kVt.ScrollMinX = 0D;
            this.zedGraph_kVt.ScrollMinY = 0D;
            this.zedGraph_kVt.ScrollMinY2 = 0D;
            this.zedGraph_kVt.Size = new System.Drawing.Size(338, 240);
            this.zedGraph_kVt.TabIndex = 17;
            // 
            // zedGraph_V
            // 
            this.zedGraph_V.BackColor = System.Drawing.Color.Silver;
            this.zedGraph_V.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.zedGraph_V.Location = new System.Drawing.Point(346, 78);
            this.zedGraph_V.Name = "zedGraph_V";
            this.zedGraph_V.ScrollGrace = 0D;
            this.zedGraph_V.ScrollMaxX = 0D;
            this.zedGraph_V.ScrollMaxY = 0D;
            this.zedGraph_V.ScrollMaxY2 = 0D;
            this.zedGraph_V.ScrollMinX = 0D;
            this.zedGraph_V.ScrollMinY = 0D;
            this.zedGraph_V.ScrollMinY2 = 0D;
            this.zedGraph_V.Size = new System.Drawing.Size(338, 240);
            this.zedGraph_V.TabIndex = 16;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(688, 14);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(338, 61);
            this.panel3.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(-1, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 31);
            this.label5.TabIndex = 12;
            this.label5.Text = "Nву =";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(269, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 31);
            this.label4.TabIndex = 11;
            this.label4.Text = "кВт";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.LawnGreen;
            this.label3.Location = new System.Drawing.Point(78, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 55);
            this.label3.TabIndex = 10;
            this.label3.Text = "0.0";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 16);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.ovalShape3,
            this.ovalShape2,
            this.ovalShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(1374, 337);
            this.shapeContainer1.TabIndex = 28;
            this.shapeContainer1.TabStop = false;
            // 
            // ovalShape3
            // 
            this.ovalShape3.BackColor = System.Drawing.Color.Silver;
            this.ovalShape3.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque;
            this.ovalShape3.FillGradientColor = System.Drawing.SystemColors.ActiveBorder;
            this.ovalShape3.Location = new System.Drawing.Point(1031, 181);
            this.ovalShape3.Name = "ovalShape3";
            this.ovalShape3.Size = new System.Drawing.Size(23, 23);
            // 
            // ovalShape2
            // 
            this.ovalShape2.BackColor = System.Drawing.Color.Silver;
            this.ovalShape2.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque;
            this.ovalShape2.FillGradientColor = System.Drawing.SystemColors.ActiveBorder;
            this.ovalShape2.Location = new System.Drawing.Point(1031, 154);
            this.ovalShape2.Name = "ovalShape2";
            this.ovalShape2.Size = new System.Drawing.Size(23, 23);
            // 
            // ovalShape1
            // 
            this.ovalShape1.BackColor = System.Drawing.Color.Silver;
            this.ovalShape1.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque;
            this.ovalShape1.FillGradientColor = System.Drawing.SystemColors.ActiveBorder;
            this.ovalShape1.Location = new System.Drawing.Point(1031, 127);
            this.ovalShape1.Name = "ovalShape1";
            this.ovalShape1.Size = new System.Drawing.Size(23, 23);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1045, 76);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(332, 179);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "2ТЭ116",
            "2ТЭ116У",
            "ТЭП70",
            "ТЭП70БС",
            "2ТЭ25А",
            "2ТЭ25КМ"});
            this.comboBox1.Location = new System.Drawing.Point(1097, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(135, 24);
            this.comboBox1.TabIndex = 17;
            this.comboBox1.Text = "Выбирите серию";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // zedGraph_AV
            // 
            this.zedGraph_AV.BackColor = System.Drawing.Color.Silver;
            this.zedGraph_AV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.zedGraph_AV.Location = new System.Drawing.Point(6, 12);
            this.zedGraph_AV.Name = "zedGraph_AV";
            this.zedGraph_AV.ScrollGrace = 0D;
            this.zedGraph_AV.ScrollMaxX = 0D;
            this.zedGraph_AV.ScrollMaxY = 0D;
            this.zedGraph_AV.ScrollMaxY2 = 0D;
            this.zedGraph_AV.ScrollMinX = 0D;
            this.zedGraph_AV.ScrollMinY = 0D;
            this.zedGraph_AV.ScrollMinY2 = 0D;
            this.zedGraph_AV.Size = new System.Drawing.Size(1022, 354);
            this.zedGraph_AV.TabIndex = 16;
            // 
            // comboBox2
            // 
            this.comboBox2.Enabled = false;
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(1238, 17);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(76, 24);
            this.comboBox2.TabIndex = 18;
            this.comboBox2.Text = "ПКМ";
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightGray;
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(1130, 56);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 41);
            this.button1.TabIndex = 18;
            this.button1.Text = "Отобразить точку";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.LightGray;
            this.button4.Location = new System.Drawing.Point(1130, 103);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(155, 41);
            this.button4.TabIndex = 23;
            this.button4.Text = "Очистить список точек";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.LightGray;
            this.button6.Location = new System.Drawing.Point(1130, 150);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(155, 41);
            this.button6.TabIndex = 25;
            this.button6.Text = "Начать запись параметров";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.zedGraph_AV);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Location = new System.Drawing.Point(7, 361);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1381, 371);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.LightGray;
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(1130, 197);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(155, 41);
            this.button5.TabIndex = 28;
            this.button5.Text = "Сохранить график в виде картинки";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1392, 607);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Реостат";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Timer MeasurementT;
        private ZedGraph.ZedGraphControl zedGraph_A;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ValueL;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Value2L;
        private System.Windows.Forms.Label label5;
        private ZedGraph.ZedGraphControl zedGraph_V;
        private ZedGraph.ZedGraphControl zedGraph_kVt;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape1;
        private Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape3;
        private Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape2;
        private System.Windows.Forms.ComboBox comboBox1;
        private ZedGraph.ZedGraphControl zedGraph_AV;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Timer timer1;
    }
}


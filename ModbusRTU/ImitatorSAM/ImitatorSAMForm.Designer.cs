namespace ImitatorSAM
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.PAS_TSBtn = new System.Windows.Forms.ToolStripButton();
            this.SendTimer = new System.Windows.Forms.Timer(this.components);
            this.RS_TSBtn = new System.Windows.Forms.ToolStripButton();
            this.UIT_TSBtn = new System.Windows.Forms.ToolStripButton();
            this.SYMAP_TSBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RS_TSBtn,
            this.PAS_TSBtn,
            this.UIT_TSBtn,
            this.SYMAP_TSBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(552, 28);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // PAS_TSBtn
            // 
            this.PAS_TSBtn.CheckOnClick = true;
            this.PAS_TSBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.PAS_TSBtn.Image = ((System.Drawing.Image)(resources.GetObject("PAS_TSBtn.Image")));
            this.PAS_TSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PAS_TSBtn.Name = "PAS_TSBtn";
            this.PAS_TSBtn.Size = new System.Drawing.Size(42, 25);
            this.PAS_TSBtn.Text = "PAS";
            this.PAS_TSBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.PAS_TSBtn.Click += new System.EventHandler(this.PAS_TSBtn_Click);
            // 
            // SendTimer
            // 
            this.SendTimer.Interval = 50;
            this.SendTimer.Tick += new System.EventHandler(this.SendTimer_Tick);
            // 
            // RS_TSBtn
            // 
            this.RS_TSBtn.CheckOnClick = true;
            this.RS_TSBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RS_TSBtn.Image = ((System.Drawing.Image)(resources.GetObject("RS_TSBtn.Image")));
            this.RS_TSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RS_TSBtn.Name = "RS_TSBtn";
            this.RS_TSBtn.Size = new System.Drawing.Size(33, 25);
            this.RS_TSBtn.Text = "RS";
            this.RS_TSBtn.Click += new System.EventHandler(this.RS_TSBtn_Click);
            // 
            // UIT_TSBtn
            // 
            this.UIT_TSBtn.CheckOnClick = true;
            this.UIT_TSBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.UIT_TSBtn.Image = ((System.Drawing.Image)(resources.GetObject("UIT_TSBtn.Image")));
            this.UIT_TSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UIT_TSBtn.Name = "UIT_TSBtn";
            this.UIT_TSBtn.Size = new System.Drawing.Size(37, 25);
            this.UIT_TSBtn.Text = "UIT";
            this.UIT_TSBtn.Click += new System.EventHandler(this.UIT_TSBtn_Click);
            // 
            // SYMAP_TSBtn
            // 
            this.SYMAP_TSBtn.CheckOnClick = true;
            this.SYMAP_TSBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SYMAP_TSBtn.Image = ((System.Drawing.Image)(resources.GetObject("SYMAP_TSBtn.Image")));
            this.SYMAP_TSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SYMAP_TSBtn.Name = "SYMAP_TSBtn";
            this.SYMAP_TSBtn.Size = new System.Drawing.Size(65, 25);
            this.SYMAP_TSBtn.Text = "SYMAP";
            this.SYMAP_TSBtn.Click += new System.EventHandler(this.SYMAP_TSBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 273);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "Imitator";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton PAS_TSBtn;
        private System.Windows.Forms.Timer SendTimer;
        private System.Windows.Forms.ToolStripButton RS_TSBtn;
        private System.Windows.Forms.ToolStripButton UIT_TSBtn;
        private System.Windows.Forms.ToolStripButton SYMAP_TSBtn;

    }
}


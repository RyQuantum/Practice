namespace V4HubTester
{
    partial class Form2
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.comboBoxCom = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOpenCloseCom = new System.Windows.Forms.Button();
            this.groupBoxReceiveData = new System.Windows.Forms.GroupBox();
            this.textBoxReceive = new System.Windows.Forms.TextBox();
            this.Button_Refresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBoxReceiveData.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxCom
            // 
            this.comboBoxCom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxCom.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCom.FormattingEnabled = true;
            this.comboBoxCom.Location = new System.Drawing.Point(65, 12);
            this.comboBoxCom.Name = "comboBoxCom";
            this.comboBoxCom.Size = new System.Drawing.Size(121, 32);
            this.comboBoxCom.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonOpenCloseCom
            // 
            this.buttonOpenCloseCom.Location = new System.Drawing.Point(376, 12);
            this.buttonOpenCloseCom.Name = "buttonOpenCloseCom";
            this.buttonOpenCloseCom.Size = new System.Drawing.Size(111, 33);
            this.buttonOpenCloseCom.TabIndex = 3;
            this.buttonOpenCloseCom.Text = "Open serial";
            this.buttonOpenCloseCom.UseVisualStyleBackColor = true;
            this.buttonOpenCloseCom.Click += new System.EventHandler(this.buttonOpenCloseCom_Click);
            // 
            // groupBoxReceiveData
            // 
            this.groupBoxReceiveData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxReceiveData.Controls.Add(this.textBoxReceive);
            this.groupBoxReceiveData.Location = new System.Drawing.Point(19, 50);
            this.groupBoxReceiveData.Name = "groupBoxReceiveData";
            this.groupBoxReceiveData.Size = new System.Drawing.Size(786, 416);
            this.groupBoxReceiveData.TabIndex = 7;
            this.groupBoxReceiveData.TabStop = false;
            this.groupBoxReceiveData.Text = "Receive data";
            // 
            // textBoxReceive
            // 
            this.textBoxReceive.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBoxReceive.Location = new System.Drawing.Point(6, 30);
            this.textBoxReceive.Multiline = true;
            this.textBoxReceive.Name = "textBoxReceive";
            this.textBoxReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxReceive.Size = new System.Drawing.Size(774, 382);
            this.textBoxReceive.TabIndex = 0;
            // 
            // Button_Refresh
            // 
            this.Button_Refresh.Location = new System.Drawing.Point(227, 12);
            this.Button_Refresh.Name = "Button_Refresh";
            this.Button_Refresh.Size = new System.Drawing.Size(110, 33);
            this.Button_Refresh.TabIndex = 10;
            this.Button_Refresh.Text = "Refresh port";
            this.Button_Refresh.UseVisualStyleBackColor = true;
            this.Button_Refresh.Click += new System.EventHandler(this.Button_Refresh_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "波特率";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "数据位";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "校验位";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "停止位";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 478);
            this.Controls.Add(this.Button_Refresh);
            this.Controls.Add(this.groupBoxReceiveData);
            this.Controls.Add(this.buttonOpenCloseCom);
            this.Controls.Add(this.comboBoxCom);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form2";
            this.Text = "V4Hub Tester";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBoxReceiveData.ResumeLayout(false);
            this.groupBoxReceiveData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxCom;
        private System.Windows.Forms.Button buttonOpenCloseCom;
        //private System.Windows.Forms.GroupBox groupBoxReceiveSetting;
        //private System.Windows.Forms.RadioButton radioButtonReceiveDataASCII;
        //private System.Windows.Forms.RadioButton radioButtonReceiveDataHEX;
        private System.Windows.Forms.GroupBox groupBoxReceiveData;
        private System.Windows.Forms.TextBox textBoxReceive;
        //private System.Windows.Forms.Button buttonClearRecData;
        private System.Windows.Forms.Button Button_Refresh;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        //private ComboBox comboBoxBaudRate;
        //private ComboBox comboBoxDataBit;
        //private ComboBox comboBoxCheckBit;
        //private ComboBox comboBoxStopBit;
        //private GroupBox groupBoxSerialPortSetting;
    }
}


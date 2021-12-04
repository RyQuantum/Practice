namespace V4HubTester
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PCBA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PCBACPU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PCBAETH0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PCBAWiFi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PCBABT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PCBAIMEI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PCBACCID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TFCardCap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ADCDC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ADCBAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ADCLTE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ETH0PING = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LTEPWR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LTEWDIS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LTECOMM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZWAVEPWR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZWAVECOMM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZWAVENVR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WiFiPING = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BTSCAN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RESULTTIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.PCBA,
            this.PCBACPU,
            this.PCBAETH0,
            this.PCBAWiFi,
            this.PCBABT,
            this.PCBAIMEI,
            this.PCBACCID,
            this.TFCardCap,
            this.ADCDC,
            this.ADCBAT,
            this.ADCLTE,
            this.ETH0PING,
            this.LTEPWR,
            this.LTEWDIS,
            this.LTECOMM,
            this.ZWAVEPWR,
            this.ZWAVECOMM,
            this.ZWAVENVR,
            this.WiFiPING,
            this.BTSCAN,
            this.RESULTTIME});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(776, 426);
            this.dataGridView1.TabIndex = 0;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.MinimumWidth = 8;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 50;
            // 
            // PCBA
            // 
            this.PCBA.DataPropertyName = "PCBA";
            this.PCBA.HeaderText = "PCBA";
            this.PCBA.MinimumWidth = 8;
            this.PCBA.Name = "PCBA";
            this.PCBA.ReadOnly = true;
            this.PCBA.Width = 150;
            // 
            // PCBACPU
            // 
            this.PCBACPU.DataPropertyName = "PCBACPU";
            this.PCBACPU.HeaderText = "PCBACPU";
            this.PCBACPU.MinimumWidth = 8;
            this.PCBACPU.Name = "PCBACPU";
            this.PCBACPU.ReadOnly = true;
            this.PCBACPU.Width = 180;
            // 
            // PCBAETH0
            // 
            this.PCBAETH0.DataPropertyName = "PCBAETH0";
            this.PCBAETH0.HeaderText = "PCBAETH0";
            this.PCBAETH0.MinimumWidth = 8;
            this.PCBAETH0.Name = "PCBAETH0";
            this.PCBAETH0.ReadOnly = true;
            this.PCBAETH0.Width = 70;
            // 
            // PCBAWiFi
            // 
            this.PCBAWiFi.DataPropertyName = "PCBAWiFi";
            this.PCBAWiFi.HeaderText = "PCBAWiFi";
            this.PCBAWiFi.MinimumWidth = 8;
            this.PCBAWiFi.Name = "PCBAWiFi";
            this.PCBAWiFi.ReadOnly = true;
            this.PCBAWiFi.Width = 160;
            // 
            // PCBABT
            // 
            this.PCBABT.DataPropertyName = "PCBABT";
            this.PCBABT.HeaderText = "PCBABT";
            this.PCBABT.MinimumWidth = 8;
            this.PCBABT.Name = "PCBABT";
            this.PCBABT.ReadOnly = true;
            this.PCBABT.Width = 160;
            // 
            // PCBAIMEI
            // 
            this.PCBAIMEI.DataPropertyName = "PCBAIMEI";
            this.PCBAIMEI.HeaderText = "PCBAIMEI";
            this.PCBAIMEI.MinimumWidth = 8;
            this.PCBAIMEI.Name = "PCBAIMEI";
            this.PCBAIMEI.ReadOnly = true;
            this.PCBAIMEI.Width = 170;
            // 
            // PCBACCID
            // 
            this.PCBACCID.DataPropertyName = "PCBACCID";
            this.PCBACCID.HeaderText = "PCBACCID";
            this.PCBACCID.MinimumWidth = 8;
            this.PCBACCID.Name = "PCBACCID";
            this.PCBACCID.ReadOnly = true;
            this.PCBACCID.Width = 220;
            // 
            // TFCardCap
            // 
            this.TFCardCap.DataPropertyName = "TFCardCap";
            this.TFCardCap.HeaderText = "TFCardCap";
            this.TFCardCap.MinimumWidth = 8;
            this.TFCardCap.Name = "TFCardCap";
            this.TFCardCap.ReadOnly = true;
            this.TFCardCap.Width = 70;
            // 
            // ADCDC
            // 
            this.ADCDC.DataPropertyName = "ADCDC";
            this.ADCDC.HeaderText = "ADCDC";
            this.ADCDC.MinimumWidth = 8;
            this.ADCDC.Name = "ADCDC";
            this.ADCDC.ReadOnly = true;
            this.ADCDC.Width = 70;
            // 
            // ADCBAT
            // 
            this.ADCBAT.DataPropertyName = "ADCBAT";
            this.ADCBAT.HeaderText = "ADCBAT";
            this.ADCBAT.MinimumWidth = 8;
            this.ADCBAT.Name = "ADCBAT";
            this.ADCBAT.ReadOnly = true;
            this.ADCBAT.Width = 70;
            // 
            // ADCLTE
            // 
            this.ADCLTE.DataPropertyName = "ADCLTE";
            this.ADCLTE.HeaderText = "ADCLTE";
            this.ADCLTE.MinimumWidth = 8;
            this.ADCLTE.Name = "ADCLTE";
            this.ADCLTE.ReadOnly = true;
            this.ADCLTE.Width = 70;
            // 
            // ETH0PING
            // 
            this.ETH0PING.DataPropertyName = "ETH0PING";
            this.ETH0PING.HeaderText = "ETH0PING";
            this.ETH0PING.MinimumWidth = 8;
            this.ETH0PING.Name = "ETH0PING";
            this.ETH0PING.ReadOnly = true;
            this.ETH0PING.Width = 70;
            // 
            // LTEPWR
            // 
            this.LTEPWR.DataPropertyName = "LTEPWR";
            this.LTEPWR.HeaderText = "LTEPWR";
            this.LTEPWR.MinimumWidth = 8;
            this.LTEPWR.Name = "LTEPWR";
            this.LTEPWR.ReadOnly = true;
            this.LTEPWR.Width = 70;
            // 
            // LTEWDIS
            // 
            this.LTEWDIS.DataPropertyName = "LTEWDIS";
            this.LTEWDIS.HeaderText = "LTEWDIS";
            this.LTEWDIS.MinimumWidth = 8;
            this.LTEWDIS.Name = "LTEWDIS";
            this.LTEWDIS.ReadOnly = true;
            this.LTEWDIS.Width = 70;
            // 
            // LTECOMM
            // 
            this.LTECOMM.DataPropertyName = "LTECOMM";
            this.LTECOMM.HeaderText = "LTECOMM";
            this.LTECOMM.MinimumWidth = 8;
            this.LTECOMM.Name = "LTECOMM";
            this.LTECOMM.ReadOnly = true;
            this.LTECOMM.Width = 70;
            // 
            // ZWAVEPWR
            // 
            this.ZWAVEPWR.DataPropertyName = "ZWAVEPWR";
            this.ZWAVEPWR.HeaderText = "ZWAVEPWR";
            this.ZWAVEPWR.MinimumWidth = 8;
            this.ZWAVEPWR.Name = "ZWAVEPWR";
            this.ZWAVEPWR.ReadOnly = true;
            this.ZWAVEPWR.Width = 70;
            // 
            // ZWAVECOMM
            // 
            this.ZWAVECOMM.DataPropertyName = "ZWAVECOMM";
            this.ZWAVECOMM.HeaderText = "ZWAVECOMM";
            this.ZWAVECOMM.MinimumWidth = 8;
            this.ZWAVECOMM.Name = "ZWAVECOMM";
            this.ZWAVECOMM.ReadOnly = true;
            this.ZWAVECOMM.Width = 70;
            // 
            // ZWAVENVR
            // 
            this.ZWAVENVR.DataPropertyName = "ZWAVENVR";
            this.ZWAVENVR.HeaderText = "ZWAVENVR";
            this.ZWAVENVR.MinimumWidth = 8;
            this.ZWAVENVR.Name = "ZWAVENVR";
            this.ZWAVENVR.ReadOnly = true;
            this.ZWAVENVR.Width = 70;
            // 
            // WiFiPING
            // 
            this.WiFiPING.DataPropertyName = "WiFiPING";
            this.WiFiPING.HeaderText = "WiFiPING";
            this.WiFiPING.MinimumWidth = 8;
            this.WiFiPING.Name = "WiFiPING";
            this.WiFiPING.ReadOnly = true;
            this.WiFiPING.Width = 70;
            // 
            // BTSCAN
            // 
            this.BTSCAN.DataPropertyName = "BTSCAN";
            this.BTSCAN.HeaderText = "BTSCAN";
            this.BTSCAN.MinimumWidth = 8;
            this.BTSCAN.Name = "BTSCAN";
            this.BTSCAN.ReadOnly = true;
            this.BTSCAN.Width = 70;
            // 
            // RESULTTIME
            // 
            this.RESULTTIME.DataPropertyName = "RESULTTIME";
            this.RESULTTIME.HeaderText = "RESULTTIME";
            this.RESULTTIME.MinimumWidth = 8;
            this.RESULTTIME.Name = "RESULTTIME";
            this.RESULTTIME.ReadOnly = true;
            this.RESULTTIME.Width = 160;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn id;
        private DataGridViewTextBoxColumn PCBA;
        private DataGridViewTextBoxColumn PCBACPU;
        private DataGridViewTextBoxColumn PCBAETH0;
        private DataGridViewTextBoxColumn PCBAWiFi;
        private DataGridViewTextBoxColumn PCBABT;
        private DataGridViewTextBoxColumn PCBAIMEI;
        private DataGridViewTextBoxColumn PCBACCID;
        private DataGridViewTextBoxColumn TFCardCap;
        private DataGridViewTextBoxColumn ADCDC;
        private DataGridViewTextBoxColumn ADCBAT;
        private DataGridViewTextBoxColumn ADCLTE;
        private DataGridViewTextBoxColumn ETH0PING;
        private DataGridViewTextBoxColumn LTEPWR;
        private DataGridViewTextBoxColumn LTEWDIS;
        private DataGridViewTextBoxColumn LTECOMM;
        private DataGridViewTextBoxColumn ZWAVEPWR;
        private DataGridViewTextBoxColumn ZWAVECOMM;
        private DataGridViewTextBoxColumn ZWAVENVR;
        private DataGridViewTextBoxColumn WiFiPING;
        private DataGridViewTextBoxColumn BTSCAN;
        private DataGridViewTextBoxColumn RESULTTIME;
    }
}
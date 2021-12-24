namespace FobRegister
{
    partial class Main
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.tsbtnConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsFM1208 = new System.Windows.Forms.ToolStrip();
            this.textUID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textKey = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DecID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MixedKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DecryptedKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.retry = new System.Windows.Forms.DataGridViewButtonColumn();
            this.remove = new System.Windows.Forms.DataGridViewLinkColumn();
            this.fobNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isManually = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.tsFM1208.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // tsbtnConnect
            // 
            this.tsbtnConnect.BackColor = System.Drawing.SystemColors.Control;
            this.tsbtnConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnConnect.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnConnect.Image")));
            this.tsbtnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnConnect.Name = "tsbtnConnect";
            this.tsbtnConnect.Size = new System.Drawing.Size(75, 29);
            this.tsbtnConnect.Text = "连接(C)";
            this.tsbtnConnect.Click += new System.EventHandler(this.tsbtnConnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 34);
            // 
            // tsFM1208
            // 
            this.tsFM1208.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsFM1208.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnConnect,
            this.toolStripSeparator1});
            this.tsFM1208.Location = new System.Drawing.Point(0, 0);
            this.tsFM1208.Name = "tsFM1208";
            this.tsFM1208.Size = new System.Drawing.Size(667, 34);
            this.tsFM1208.TabIndex = 33;
            this.tsFM1208.Text = "FM1208";
            // 
            // textUID
            // 
            this.textUID.Location = new System.Drawing.Point(69, 300);
            this.textUID.Name = "textUID";
            this.textUID.ReadOnly = true;
            this.textUID.Size = new System.Drawing.Size(445, 28);
            this.textUID.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 303);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 21);
            this.label1.TabIndex = 35;
            this.label1.Text = "卡号:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 343);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 21);
            this.label5.TabIndex = 44;
            this.label5.Text = "密钥:";
            // 
            // textKey
            // 
            this.textKey.Location = new System.Drawing.Point(69, 340);
            this.textKey.Name = "textKey";
            this.textKey.Size = new System.Drawing.Size(445, 28);
            this.textKey.TabIndex = 46;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(520, 376);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 34);
            this.button1.TabIndex = 49;
            this.button1.Text = "初始化(空格)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.DecID,
            this.MixedKey,
            this.Column4,
            this.Column2,
            this.Column3,
            this.DecryptedKey});
            this.dataGridView1.Location = new System.Drawing.Point(12, 37);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(642, 251);
            this.dataGridView1.TabIndex = 50;
            this.dataGridView1.Visible = false;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Id";
            this.Column1.HeaderText = "Id";
            this.Column1.MinimumWidth = 8;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 50;
            // 
            // DecID
            // 
            this.DecID.DataPropertyName = "DecID";
            this.DecID.HeaderText = "卡号";
            this.DecID.MinimumWidth = 8;
            this.DecID.Name = "DecID";
            this.DecID.ReadOnly = true;
            this.DecID.Width = 120;
            // 
            // MixedKey
            // 
            this.MixedKey.DataPropertyName = "MixedKey";
            this.MixedKey.HeaderText = "密钥";
            this.MixedKey.MinimumWidth = 8;
            this.MixedKey.Name = "MixedKey";
            this.MixedKey.ReadOnly = true;
            this.MixedKey.Width = 280;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "Uploaded";
            this.Column4.HeaderText = "已上传";
            this.Column4.MinimumWidth = 8;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column4.Width = 100;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "UID";
            this.Column2.HeaderText = "HexID";
            this.Column2.MinimumWidth = 8;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Visible = false;
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "Key";
            this.Column3.HeaderText = "Key";
            this.Column3.MinimumWidth = 8;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Visible = false;
            this.Column3.Width = 290;
            // 
            // DecryptedKey
            // 
            this.DecryptedKey.DataPropertyName = "DecryptedKey";
            this.DecryptedKey.HeaderText = "DecryptedKey";
            this.DecryptedKey.MinimumWidth = 8;
            this.DecryptedKey.Name = "DecryptedKey";
            this.DecryptedKey.ReadOnly = true;
            this.DecryptedKey.Visible = false;
            this.DecryptedKey.Width = 150;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(109, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(281, 21);
            this.label2.TabIndex = 51;
            this.label2.Text = "<=    1. 点击连接读卡器（重置）";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(91, 383);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(423, 21);
            this.label3.TabIndex = 52;
            this.label3.Text = "2. 把卡放到读卡器上，点击这里（重复操作）  =>";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(542, 296);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 34);
            this.button2.TabIndex = 53;
            this.button2.Text = "查看卡";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.UID,
            this.Status,
            this.Count,
            this.retry,
            this.remove,
            this.fobNumber,
            this.Key,
            this.isManually});
            this.dataGridView2.Location = new System.Drawing.Point(12, 34);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersWidth = 62;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(642, 254);
            this.dataGridView2.TabIndex = 54;
            this.dataGridView2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellClick);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.MinimumWidth = 8;
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Width = 50;
            // 
            // UID
            // 
            this.UID.DataPropertyName = "UID";
            this.UID.HeaderText = "卡号";
            this.UID.MinimumWidth = 8;
            this.UID.Name = "UID";
            this.UID.ReadOnly = true;
            this.UID.Width = 120;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "状态";
            this.Status.MinimumWidth = 8;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 250;
            // 
            // Count
            // 
            this.Count.DataPropertyName = "Count";
            this.Count.HeaderText = "No.";
            this.Count.MinimumWidth = 8;
            this.Count.Name = "Count";
            this.Count.ReadOnly = true;
            this.Count.Width = 30;
            // 
            // retry
            // 
            this.retry.DataPropertyName = "retry";
            this.retry.HeaderText = "";
            this.retry.MinimumWidth = 8;
            this.retry.Name = "retry";
            this.retry.ReadOnly = true;
            this.retry.Text = "重试";
            this.retry.UseColumnTextForButtonValue = true;
            this.retry.Width = 50;
            this.retry.DefaultCellStyle.BackColor = Color.LightGray;
            // 
            // remove
            // 
            this.remove.DataPropertyName = "remove";
            this.remove.HeaderText = "";
            this.remove.LinkColor = System.Drawing.Color.Red;
            this.remove.MinimumWidth = 8;
            this.remove.Name = "remove";
            this.remove.ReadOnly = true;
            this.remove.Text = "删除";
            this.remove.UseColumnTextForLinkValue = true;
            this.remove.VisitedLinkColor = System.Drawing.Color.Red;
            this.remove.Width = 50;
            // 
            // fobNumber
            // 
            this.fobNumber.DataPropertyName = "fobNumber";
            this.fobNumber.HeaderText = "fobNumber";
            this.fobNumber.MinimumWidth = 8;
            this.fobNumber.Name = "fobNumber";
            this.fobNumber.ReadOnly = true;
            this.fobNumber.Visible = false;
            this.fobNumber.Width = 150;
            // 
            // Key
            // 
            this.Key.DataPropertyName = "Key";
            this.Key.HeaderText = "Key";
            this.Key.MinimumWidth = 8;
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            this.Key.Visible = false;
            this.Key.Width = 150;
            // 
            // isManually
            // 
            this.isManually.DataPropertyName = "isManually";
            this.isManually.HeaderText = "isManually";
            this.isManually.MinimumWidth = 8;
            this.isManually.Name = "isManually";
            this.isManually.ReadOnly = true;
            this.isManually.Visible = false;
            this.isManually.Width = 150;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(542, 336);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 34);
            this.button3.TabIndex = 55;
            this.button3.Text = "验证卡";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(667, 428);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textKey);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textUID);
            this.Controls.Add(this.tsFM1208);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发卡器程序";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.FM1208_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.tsFM1208.ResumeLayout(false);
            this.tsFM1208.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton tsbtnConnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip tsFM1208;
        private System.Windows.Forms.TextBox textUID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textKey;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button button3;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn UID;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn Count;
        private DataGridViewButtonColumn retry;
        private DataGridViewLinkColumn remove;
        private DataGridViewTextBoxColumn fobNumber;
        private DataGridViewTextBoxColumn Key;
        private DataGridViewTextBoxColumn isManually;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn DecID;
        private DataGridViewTextBoxColumn MixedKey;
        private DataGridViewCheckBoxColumn Column4;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn DecryptedKey;
    }
}


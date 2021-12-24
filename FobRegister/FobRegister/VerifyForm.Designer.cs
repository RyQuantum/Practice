namespace FobRegister
{
    partial class VerifyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerifyForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tsFM1208 = new System.Windows.Forms.ToolStrip();
            this.button1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.textKey = new System.Windows.Forms.TextBox();
            this.textUID = new System.Windows.Forms.TextBox();
            this.tsFM1208.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "卡号:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "密钥:";
            // 
            // tsFM1208
            // 
            this.tsFM1208.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsFM1208.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button1,
            this.toolStripSeparator1});
            this.tsFM1208.Location = new System.Drawing.Point(0, 0);
            this.tsFM1208.Name = "tsFM1208";
            this.tsFM1208.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.tsFM1208.Size = new System.Drawing.Size(467, 34);
            this.tsFM1208.TabIndex = 34;
            this.tsFM1208.Text = "FM1208";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 29);
            this.button1.Text = "连接 (C)";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 34);
            // 
            // textKey
            // 
            this.textKey.Location = new System.Drawing.Point(71, 77);
            this.textKey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textKey.Name = "textKey";
            this.textKey.ReadOnly = true;
            this.textKey.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textKey.Size = new System.Drawing.Size(368, 31);
            this.textKey.TabIndex = 3;
            // 
            // textUID
            // 
            this.textUID.Location = new System.Drawing.Point(71, 38);
            this.textUID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textUID.Name = "textUID";
            this.textUID.ReadOnly = true;
            this.textUID.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textUID.Size = new System.Drawing.Size(368, 31);
            this.textUID.TabIndex = 2;
            // 
            // VerifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 131);
            this.Controls.Add(this.tsFM1208);
            this.Controls.Add(this.textKey);
            this.Controls.Add(this.textUID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "VerifyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "验证卡";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VerifyForm_FormClosing);
            this.Load += new System.EventHandler(this.VerifyForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VerifyForm_KeyDown);
            this.tsFM1208.ResumeLayout(false);
            this.tsFM1208.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStrip tsFM1208;
        private System.Windows.Forms.ToolStripButton button1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private TextBox textKey;
        private TextBox textUID;
    }
}
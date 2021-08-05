using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace PanelLabelPrinter
{
    public partial class ConfigurationForm : Form
    {
        private Process process;

        public ConfigurationForm(Process process)
        {
            this.process = process;
            InitializeComponent();
            ThreadStart entry = new ThreadStart(loadDb);
            Thread workThread = new Thread(entry);
            workThread.Start();
        }

        private void loadDb()
        {
            using (var db = new MyLockDB())
            {
                db.Locks.FirstOrDefault(f => true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请填写型号。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("请填写一箱几个。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("请填写箱子重量。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("请填写批次。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox5.Text == "")
            {
                MessageBox.Show("请填写初始箱号。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int num = Int32.Parse(textBox2.Text);
            if (num < 0)
            {
                MessageBox.Show("每箱最少装1个。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int weight = Int32.Parse(textBox3.Text);
            if (weight < 0)
            {
                MessageBox.Show("箱子重量需大于0KG。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string batchNo = textBox4.Text;
            string initialCaseNo = textBox5.Text;
            Regex regex = new Regex(@"\d{8}F\d{6}");
            if (!regex.IsMatch(initialCaseNo))
            {
                MessageBox.Show("初始箱号需按照“xxxxxxxxFxxxxxx”格式填写（x为数字）。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string model = textBox1.Text;
            MainForm form = new MainForm(num, model, weight, batchNo, initialCaseNo, this.process);
            Hide();
            form.ShowDialog();
        }

        private void ConfigurationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            process.Kill();
        }
    }
}

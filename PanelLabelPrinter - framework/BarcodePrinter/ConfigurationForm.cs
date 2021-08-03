using Common.Logging;
using Common.Logging.Simple;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace BarcodePrinter
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


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                MessageBox.Show("请填写型号。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("请填写一箱几个。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("请填写箱子重量。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("请填写批次。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("请填写初始箱号。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int num = Int32.Parse(textBox1.Text);
            if (num < 0)
            {
                MessageBox.Show("每箱最少装1个。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int weight = Int32.Parse(textBox2.Text);
            if (weight < 0)
            {
                MessageBox.Show("箱子重量需大于0KG。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string batchNo = textBox3.Text;
            string initialCaseNo = textBox4.Text;
            Regex regex = new Regex(@"\d{8}F\d{6}");
            if (!regex.IsMatch(initialCaseNo))
            {
                MessageBox.Show("初始箱号需按照“xxxxxxxxFxxxxxx”格式填写（x为数字）。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string model = textBox5.Text;
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

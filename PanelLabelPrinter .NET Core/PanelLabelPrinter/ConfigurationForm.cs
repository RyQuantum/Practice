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
        private Config config;
        public ConfigurationForm(Process process)
        {
            this.process = process;
            InitializeComponent();
            comboBox1.Text = "Oaks Access Panel 3";
        }

        public ConfigurationForm(Process process, Config config)
        {
            this.process = process;
            InitializeComponent();
            comboBox1.Text = config.model;
            textBox1.Text = config.qty;
            textBox2.Text = config.weight;
            textBox3.Text = config.po;
            this.config = config;
            button1.Visible = true;
            button2.Visible = false;
            button3.Visible = true;
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
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
                MessageBox.Show("请填写PO。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            string model = comboBox1.Text;
            using (var db = new MyDatabase())
            {
                db.Configs.Add(new Config()
                {
                    model = model,
                    qty = num.ToString(),
                    weight = weight.ToString(),
                    po = textBox3.Text,
                });
                db.SaveChangesAsync();
            }
            MainForm form = new MainForm(num, model, weight, textBox3.Text, this.process);
            Hide();
            form.Show();
        }

        private void ConfigurationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (config == null)
            {
                process.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new MyDatabase())
            {
                var config = db.Configs.FirstOrDefault(f => true);
                config.model = comboBox1.Text;
                config.qty = textBox1.Text;
                config.weight = textBox2.Text;
                config.po = textBox3.Text;
                db.SaveChangesAsync();
                MessageBox.Show($"请重启软件使设置生效", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                process.Kill();
                System.Environment.Exit(1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}

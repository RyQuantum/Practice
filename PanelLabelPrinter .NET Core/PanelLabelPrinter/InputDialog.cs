using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodePrinter
{
    public partial class InputDialog : Form
    {
        public bool isValid = false;
        public InputDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "321")
            {
                MessageBox.Show("密码错误！请重试。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                isValid = true;
                this.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}

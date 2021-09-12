using RJCP.IO.Ports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Host
{
    //SerialPortStream demo
    public partial class Form3 : Form
    {
        public Thread thread;
        public Form3()
        {
            InitializeComponent();
            thread = new Thread(StartSerialPort);
            thread.Start();
        }

        public void StartSerialPort()
        {
            foreach (string c in SerialPortStream.GetPortNames())
            {
                if (c != "COM4") continue;
                Debug.WriteLine("GetPortNames: " + c);
                SerialPortStream s = new SerialPortStream(c, 115200, 8, Parity.None, StopBits.One);
                s.Open();
                byte[] buf = new byte[1000000];
                while (true)
                {
                    int count = s.Read(buf);
                    string str = System.Text.Encoding.Default.GetString(buf.Take(count).ToArray());
                    Debug.WriteLine("res: \n" + str);
                    listBox1.Invoke(new MethodInvoker(() => listBox1.Items.Insert(listBox1.Items.Count, "res: \n" + str)));
                }
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}

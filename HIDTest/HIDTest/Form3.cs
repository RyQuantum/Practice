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

namespace HIDTest
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            Thread thread = new Thread(StartSerialPort);
            thread.Start();
        }

        public void StartSerialPort()
        {
            foreach (string c in SerialPortStream.GetPortNames())
            {
                Debug.WriteLine("GetPortNames: " + c);
                SerialPortStream s = new SerialPortStream(c, 115200, 8, Parity.None, StopBits.One);
                s.Open();
                byte[] buf = new byte[1000000];
                while (true)
                {
                    int count = s.Read(buf);
                    string str = System.Text.Encoding.Default.GetString(buf.Take(count).ToArray());
                    Debug.WriteLine("res: \n" + str);
                }
            }
        }
    }
}

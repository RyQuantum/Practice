using HidLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Host
{
    public partial class Form1 : Form
    {
        //HidLibrary demo
        public Form1()
        {
            InitializeComponent();
            Func();
        }

        public void Func()
        {
            var HidDevice = HidDevices.Enumerate(0x1995, 0x0806).FirstOrDefault();

            Debug.WriteLine("Connected: " + HidDevice.IsConnected.ToString());
            byte[] OutData = new byte[HidDevice.Capabilities.OutputReportByteLength];

            // Send a report to initiate an error sound
            OutData[0] = 0x01;
            OutData[1] = 0x02;
            OutData[2] = 0x03;
            OutData[3] = 0x04;
            OutData[62] = 0x63;
            OutData[63] = 0x64;
            OutData[64] = 0x65;

            HidDevice.Write(OutData);

            // Blocking read of report
            HidDeviceData InData = HidDevice.Read();
            Debug.WriteLine(BitConverter.ToString(InData.Data).Replace("-", ""));
        }
    }

}

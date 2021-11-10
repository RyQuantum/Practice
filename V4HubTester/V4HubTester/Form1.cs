using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace V4HubTester
{
    public partial class Form1 : Form
    {
        static SerialPort serialPort1;
        public Form1()
        {
            serialPort1 = new SerialPort();
            serialPort1.DataReceived += serialPort1_DataReceived_1;
            InitializeComponent();
            InitControls();
        }

        public void InitControls()
        {
            #region 串口缓冲器数据设置，以及时间显示初始化并打开定时器进行计时
            serialPort1.ReceivedBytesThreshold = 1;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.Text = System.DateTime.Now.ToString();
            timer2.Start();
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (button2.Text)
            {
                case MyCommonStr.OpenString: EventBtn2打开串口(); break;
                default:
                    EventBtn2Else();
                    break;
            }
        }

        private void EventBtn2打开串口()
        {
            try
            {
                #region 1.配置串口
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = 115200;
                serialPort1.Open();
                serialPort1.DiscardInBuffer();
                #endregion

                #region 2.打开串口后，下拉框不可用
                comboBox1.Enabled = false;
                #endregion

                #region 3.更改按键状态
                UI_Change(button2, MyCommonStr.CloseString, MyCommonStr.BtnCancelColor);
                #endregion

            }
            catch
            {
                MessageBox.Show(MyCommonStr.SerialCloseErrorMsg, "Wrong");   //弹出错误对话框
            }
        }

        private void UI_Change(Button btn, string str, Color color)
        {
            btn.Text = str;
            btn.ForeColor = color;
        }

        private void EventBtn2Else()
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }

            timer1.Stop();
            comboBox1.Enabled = true;
            UI_Change(button2, MyCommonStr.OpenString, Color.White);
        }

        private void ProbeSendData()
        {
            Thread.Sleep(10);
            if (serialPort1.IsOpen)
            {
                serialPort1.DiscardInBuffer();
            }

            byte[] tmp = System.Text.Encoding.UTF8.GetBytes(richTextBox2.Text); ;
            serialPort1.Write(tmp, 0, tmp.Length);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            ProbeSendData();

            if (!checkBox1.Checked)
            {
                timer1.Stop();
            }
            else
            {
                timer1.Start();
            }
        }

        string[] ArryPort = { "" };
        private void timer2_Tick(object sender, EventArgs e)
        {
            #region 1.更新时间并显示
            richTextBox1.Text = System.DateTime.Now.ToString();
            #endregion

            #region 2.实时检测并更新串口
            if (!Enumerable.SequenceEqual(ArryPort, SerialPort.GetPortNames()))
            {
                Updata_Serialport_Name(comboBox1);  //调用更新可用串口函数，comboBox1为 端口 组合框名字
            }

            #endregion
        }

        /// <summary>
        /// 检测是否插入串口
        /// </summary>
        /// <param name="MycomboBox"></param>
        private void Updata_Serialport_Name(ComboBox MycomboBox)
        {
            ArryPort = SerialPort.GetPortNames();

            MycomboBox.Items.Clear();

            if (ArryPort.Length <= 0)
            {
                comboBox1.Text = "";
            }
            else
            {
                for (int i = 0; i < ArryPort.Length; i++)
                {
                    MycomboBox.Items.Add(ArryPort[i]);
                }

                if (ArryPort.Length > 0)
                {
                    comboBox1.Text = ArryPort[0];
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button2.Text == MyCommonStr.OpenString)
            {
                MessageBox.Show(MyCommonStr.OpenSerialFirst, "Wrong");
                return;
            }

            switch (button1.Text)
            {
                case MyCommonStr.StopSendString: EventBtn1停止发送(); break;
                default:
                    EventBtn1Else();
                    break;
            }
        }

        private void EventBtn1停止发送()
        {
            UI_Change(button1, MyCommonStr.SendString, Color.White);
            timer1.Stop();
        }

        private void EventBtn1Else()
        {
            UI_Change(button1, MyCommonStr.StopSendString, MyCommonStr.BtnCancelColor);
            if (checkBox1.Checked)
            {
                timer1.Start();
            }
            else
            {
                Action act = ProbeSendData;
                this.Invoke(act);
                timer1.Stop();
            }
        }

        private void serialPort1_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(10);

                int length = serialPort1.BytesToRead;

                byte[] buf = new byte[length];

                serialPort1.Read(buf, 0, length);//13

                new Thread(() => receivedDataControl(buf)).Start();

            }
            catch
            {
                MessageBox.Show("串口读取数据错误。", "Wrong");   //弹出错误对话框 
            }
        }


        /// <summary>
        /// 对接收的数据进行处理并在界面显示
        /// </summary>
        /// <param name="tmp"></param>
        private void receivedDataControl(byte[] tmp)
        {
            this.BeginInvoke((EventHandler)delegate
            {
                richTextBox2.AppendText(Encoding.ASCII.GetString(tmp) + "\r\n");
                serialPort1.DiscardInBuffer();
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
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
            #region ���ڻ������������ã��Լ�ʱ����ʾ��ʼ�����򿪶�ʱ�����м�ʱ
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
                case MyCommonStr.OpenString: EventBtn2�򿪴���(); break;
                default:
                    EventBtn2Else();
                    break;
            }
        }

        private void EventBtn2�򿪴���()
        {
            try
            {
                #region 1.���ô���
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = 115200;
                serialPort1.Open();
                serialPort1.DiscardInBuffer();
                #endregion

                #region 2.�򿪴��ں������򲻿���
                comboBox1.Enabled = false;
                #endregion

                #region 3.���İ���״̬
                UI_Change(button2, MyCommonStr.CloseString, MyCommonStr.BtnCancelColor);
                #endregion

            }
            catch
            {
                MessageBox.Show(MyCommonStr.SerialCloseErrorMsg, "Wrong");   //��������Ի���
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
            #region 1.����ʱ�䲢��ʾ
            richTextBox1.Text = System.DateTime.Now.ToString();
            #endregion

            #region 2.ʵʱ��Ⲣ���´���
            if (!Enumerable.SequenceEqual(ArryPort, SerialPort.GetPortNames()))
            {
                Updata_Serialport_Name(comboBox1);  //���ø��¿��ô��ں�����comboBox1Ϊ �˿� ��Ͽ�����
            }

            #endregion
        }

        /// <summary>
        /// ����Ƿ���봮��
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
                case MyCommonStr.StopSendString: EventBtn1ֹͣ����(); break;
                default:
                    EventBtn1Else();
                    break;
            }
        }

        private void EventBtn1ֹͣ����()
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
                MessageBox.Show("���ڶ�ȡ���ݴ���", "Wrong");   //��������Ի��� 
            }
        }


        /// <summary>
        /// �Խ��յ����ݽ��д����ڽ�����ʾ
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
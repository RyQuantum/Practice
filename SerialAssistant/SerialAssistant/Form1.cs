using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace SerialAssistant
{
    public partial class Form1 : Form
    {
        HubDBContext db;
        Hub hubObj;
        Form2 form2;
        bool hasResult = false;
        private StringBuilder sb = new StringBuilder();    //为了避免在接收处理函数中反复调用，依然声明为一个全局变量

        public Form1(HubDBContext db, Form2 form2)
        {
            InitializeComponent();
            this.db = db;
            this.form2 = form2;
        }

        private bool search_port_is_exist(String item, String[] port_list)
        {
            for (int i = 0; i < port_list.Length; i++)
            {
                if (port_list[i].Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        /* 扫描串口列表并添加到选择框 */
        private void Update_Serial_List()
        {
            try
            {
                /* 搜索串口 */
                String[] cur_port_list = System.IO.Ports.SerialPort.GetPortNames();

                /* 刷新串口列表comboBox */
                int count = comboBox1.Items.Count;
                if (count == 0)
                {
                    //combox中无内容，将当前串口列表全部加入
                    comboBox1.Items.AddRange(cur_port_list);
                    return;
                }
                else
                {
                    //combox中有内容

                    //判断有无新插入的串口
                    for (int i = 0; i < cur_port_list.Length; i++)
                    {
                        if (!comboBox1.Items.Contains(cur_port_list[i]))
                        {
                            //找到新插入串口，添加到combox中
                            comboBox1.Items.Add(cur_port_list[i]);
                        }
                    }

                    //判断有无拔掉的串口
                    for (int i = 0; i < count; i++)
                    {
                        if (!search_port_is_exist(comboBox1.Items[i].ToString(), cur_port_list))
                        {
                            //找到已被拔掉的串口，从combox中移除
                            comboBox1.Items.RemoveAt(i);
                        }
                    }
                }

                /* 如果当前选中项为空，则默认选择第一项 */
                if (comboBox1.Items.Count > 0)
                {
                    if (comboBox1.Text.Equals(""))
                    {
                        //软件刚启动时，列表项的文本值为空
                        comboBox1.Text = comboBox1.Items[0].ToString();
                    }
                }
                else
                {
                    //无可用列表，清空文本值
                    comboBox1.Text = "";
                }


            }
            catch (Exception)
            {
                //当下拉框被打开时，修改下拉框会发生异常
                return;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            /* 添加串口选择列表 */
            Update_Serial_List();


            /* 在串口未打开的情况下每隔1s刷新一次串口列表框 */
            timer1.Interval = 1000;
            timer1.Start();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Update_Serial_List();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //将可能产生异常的代码放置在try块中
                //根据当前串口属性来判断是否打开
                if (serialPort1.IsOpen)
                {
                    //串口已经处于打开状态

                    serialPort1.Close();    //关闭串口
                    button1.BackgroundImage = global::SerialAssistant.Properties.Resources.connect;
                    comboBox1.Enabled = true;

                    //开启端口扫描
                    timer1.Interval = 1000;
                    timer1.Start();
                }
                else
                {
                    /* 串口已经处于关闭状态，则设置好串口属性后打开 */
                    //停止串口扫描
                    timer1.Stop();

                    comboBox1.Enabled = false;
                    //checkBox4.Enabled = true;
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = 115200;
                    serialPort1.DataBits = 8;

                    serialPort1.Parity = System.IO.Ports.Parity.None;

                    serialPort1.StopBits = System.IO.Ports.StopBits.One;

                    //打开串口，设置状态
                    serialPort1.Open();
                    button1.BackgroundImage = global::SerialAssistant.Properties.Resources.disconnect;

                }
            }
            catch (Exception ex)
            {
                //捕获可能发生的异常并进行处理

                //捕获到异常，创建一个新的对象，之前的不可以再用  
                serialPort1 = new System.IO.Ports.SerialPort(components);
                serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPort1_DataReceived);

                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button1.BackgroundImage = global::SerialAssistant.Properties.Resources.connect;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;

                //开启串口扫描
                timer1.Interval = 1000;
                timer1.Start();
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            /* 串口接收事件处理 */
            //hasResult = true;
            int num = serialPort1.BytesToRead;      //获取接收缓冲区中的字节数
            byte[] received_buf = new byte[num];    //声明一个大小为num的字节数据用于存放读出的byte型数据


            serialPort1.Read(received_buf, 0, num);   //读取接收缓冲区中num个字节到byte数组中

            sb.Clear();     //防止出错,首先清空字符串构造器

            sb.Append(Encoding.ASCII.GetString(received_buf));  //将整个数组解码为ASCII数组
            try
            {
                //因为要访问UI资源，所以需要使用invoke方式同步ui
                Invoke((EventHandler)(delegate
                {
                    string log = sb.ToString();
                    if (log == "\0")
                    {
                        textBox1.Text = "";
                        hasResult = false;
                        return;
                    }
                    /* 不需要时间戳 */
                    textBox1.AppendText(log);
                    if (hasResult) return;
                    var index = textBox1.Text.IndexOf("result_end");
                    if (index != -1)
                    {
                        hubObj = new Hub();

                        var pcbaCpuIndex = textBox1.Text.IndexOf("PCBA:  CPU");
                        if (pcbaCpuIndex != -1) hubObj.PCBACPU = textBox1.Text.Substring(pcbaCpuIndex + 13, 16);

                        var pcbaEth0Index = textBox1.Text.IndexOf("PCBA: ETH0");
                        if (pcbaEth0Index != -1) hubObj.PCBAETH0 = textBox1.Text.Substring(pcbaEth0Index + 13, 4);

                        var pcbaWifiIndex = textBox1.Text.IndexOf("PCBA: WiFi");
                        if (pcbaWifiIndex != -1) hubObj.PCBAWiFi = textBox1.Text.Substring(pcbaWifiIndex + 13, 17);

                        var pcbaBtIndex = textBox1.Text.IndexOf("PCBA:   BT");
                        if (pcbaBtIndex != -1) hubObj.PCBABT = textBox1.Text.Substring(pcbaBtIndex + 13, 17);

                        var pcbaImeiIndex = textBox1.Text.IndexOf("PCBA: IMEI");
                        if (pcbaImeiIndex != -1) hubObj.PCBAIMEI = textBox1.Text.Substring(pcbaImeiIndex + 13, 15);

                        var pcbaCcidIndex = textBox1.Text.IndexOf("PCBA: CCID");
                        if (pcbaCcidIndex != -1) hubObj.PCBACCID = textBox1.Text.Substring(pcbaCcidIndex + 13, 20);

                        var tfCardCapIndex = textBox1.Text.IndexOf("TFCard:  Cap");
                        if (tfCardCapIndex != -1) hubObj.TFCardCap = textBox1.Text.Substring(tfCardCapIndex + 15, 4);

                        var adcDcIndex = textBox1.Text.IndexOf("ADC:   DC");
                        if (adcDcIndex != -1) hubObj.ADCDC = textBox1.Text.Substring(adcDcIndex + 12, 5);

                        var adcBatIndex = textBox1.Text.IndexOf("ADC:  BAT");
                        if (adcBatIndex != -1) hubObj.ADCBAT = textBox1.Text.Substring(adcBatIndex + 12, 5);

                        var adcLteIndex = textBox1.Text.IndexOf("ADC:  LTE");
                        if (adcLteIndex != -1) hubObj.ADCLTE = textBox1.Text.Substring(adcLteIndex + 12, 6);

                        var eth0PingIndex = textBox1.Text.IndexOf("ETH0: PING");
                        if (eth0PingIndex != -1) hubObj.ETH0PING = textBox1.Text.Substring(eth0PingIndex + 13, 4);

                        var ltePwrIndex = textBox1.Text.IndexOf("LTE:  PWR");
                        if (ltePwrIndex != -1) hubObj.LTEPWR = textBox1.Text.Substring(ltePwrIndex + 12, 4);

                        var lteWdisIndex = textBox1.Text.IndexOf("LTE: WDIS");
                        if (lteWdisIndex != -1) hubObj.LTEWDIS = textBox1.Text.Substring(lteWdisIndex + 12, 4);

                        var lteCommIndex = textBox1.Text.IndexOf("LTE: COMM");
                        if (lteCommIndex != -1) hubObj.LTECOMM = textBox1.Text.Substring(lteCommIndex + 12, 4);

                        var zwavePwrIndex = textBox1.Text.IndexOf("ZWAVE:  PWR");
                        if (zwavePwrIndex != -1) hubObj.ZWAVEPWR = textBox1.Text.Substring(zwavePwrIndex + 14, 4);

                        var zwaveCommIndex = textBox1.Text.IndexOf("ZWAVE: COMM");
                        if (zwaveCommIndex != -1) hubObj.ZWAVECOMM = textBox1.Text.Substring(zwaveCommIndex + 14, 4);

                        var zwaveNvrIndex = textBox1.Text.IndexOf("ZWAVE:  NVR");
                        if (zwaveNvrIndex != -1) hubObj.ZWAVENVR = textBox1.Text.Substring(zwaveNvrIndex + 14, 4);

                        var wifiPingIndex = textBox1.Text.IndexOf("Wi-Fi: PING");
                        if (wifiPingIndex != -1) hubObj.WiFiPING = textBox1.Text.Substring(wifiPingIndex + 14, 4);

                        var btScanIndex = textBox1.Text.IndexOf("BT: SCAN");
                        if (btScanIndex != -1) hubObj.BTSCAN = textBox1.Text.Substring(btScanIndex + 11, 4);

                        var resultTimeIndex = textBox1.Text.IndexOf("RESULT: TIME");
                        if (resultTimeIndex != -1) hubObj.RESULTTIME = textBox1.Text.Substring(resultTimeIndex + 15, 14);

                        db.Hubs.Add(hubObj);
                        db.SaveChanges();
                        hasResult = true;
                        form2.presentNewestTable();
                    }
                }));
            }
            catch (Exception ex)
            {
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(ex.Message);

            }
        }

    }
}

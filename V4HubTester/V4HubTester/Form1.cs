using System.IO.Ports;
using System.Text;

namespace V4HubTester
{
    public partial class Form1 : Form
    {
        MainForm mainForm;
        // use integer (position) to represent which part of the UI and serial port the user is setting
        int position = 1; // 1: top left; 2: top right; 3: bottom left; 4: bottom right
        String[] ports = new String[5]; // 4 COM are saved to index 1 - 4
        private StringBuilder sb = new StringBuilder();
        Dictionary<String, Dictionary<String, int>> states = new Dictionary<String, Dictionary<String, int>>()
        {
            { "hub1", new Dictionary<String, int>() { { "phase", 0 }, { "index", 0 } } },
            { "hub2", new Dictionary<String, int>() { { "phase", 0 }, { "index", 0 } } },
            { "hub3", new Dictionary<String, int>() { { "phase", 0 }, { "index", 0 } } },
            { "hub4", new Dictionary<String, int>() { { "phase", 0 }, { "index", 0 } } },
        }; // record the current phase for each hub; index is used to optimize the performance, no need to search the keyword in textBox before index
        Dictionary<string, string> resultTimes = new Dictionary<string, string>()
        {
            { "hub1", "" }, { "hub2", "" }, { "hub3", "" }, { "hub4", "" },
        }; // set resultTime as the ID

        SerialPort serialPort1 = new SerialPort();
        SerialPort serialPort2 = new SerialPort();
        SerialPort serialPort3 = new SerialPort();
        SerialPort serialPort4 = new SerialPort();

        // save mainForm variable for updating the newest data on dataGridView after testing
        public Form1(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            serialPort2.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            serialPort3.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            serialPort4.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
        }

        // init serial port
        public void initSerial(SerialPort serialPort)
        {
            serialPort.BaudRate = 115200;
        }

        // enable/disable the user input for hub in the position 1
        private void setHub1Report(bool isEnable)
        {
            radioButton8.Checked = false;
            radioButton7.Checked = false;
            radioButton6.Checked = false;
            radioButton5.Checked = false;
            radioButton4.Checked = false;
            radioButton3.Checked = false;
            radioButton2.Checked = false;
            radioButton1.Checked = false;
            radioButton8.Enabled = isEnable;
            radioButton7.Enabled = isEnable;
            radioButton6.Enabled = isEnable;
            radioButton5.Enabled = isEnable;
            radioButton4.Enabled = isEnable;
            radioButton3.Enabled = isEnable;
            radioButton2.Enabled = isEnable;
            radioButton1.Enabled = isEnable;
        }

        // enable/disable the user input for hub in the position 2
        private void setHub2Report(bool isEnable)
        {
            radioButton22.Checked = false;
            radioButton21.Checked = false;
            radioButton20.Checked = false;
            radioButton19.Checked = false;
            radioButton18.Checked = false;
            radioButton17.Checked = false;
            radioButton16.Checked = false;
            radioButton15.Checked = false;
            radioButton22.Enabled = isEnable;
            radioButton21.Enabled = isEnable;
            radioButton20.Enabled = isEnable;
            radioButton19.Enabled = isEnable;
            radioButton18.Enabled = isEnable;
            radioButton17.Enabled = isEnable;
            radioButton16.Enabled = isEnable;
            radioButton15.Enabled = isEnable;
        }

        // enable/disable the user input for hub in the position 3
        private void setHub3Report(bool isEnable)
        {
            radioButton36.Checked = false;
            radioButton35.Checked = false;
            radioButton34.Checked = false;
            radioButton33.Checked = false;
            radioButton32.Checked = false;
            radioButton31.Checked = false;
            radioButton30.Checked = false;
            radioButton29.Checked = false;
            radioButton36.Enabled = isEnable;
            radioButton35.Enabled = isEnable;
            radioButton34.Enabled = isEnable;
            radioButton33.Enabled = isEnable;
            radioButton32.Enabled = isEnable;
            radioButton31.Enabled = isEnable;
            radioButton30.Enabled = isEnable;
            radioButton29.Enabled = isEnable;
        }

        // enable/disable the user input for hub in the position 4
        private void setHub4Report(bool isEnable)
        {
            radioButton50.Checked = false;
            radioButton49.Checked = false;
            radioButton48.Checked = false;
            radioButton47.Checked = false;
            radioButton46.Checked = false;
            radioButton45.Checked = false;
            radioButton44.Checked = false;
            radioButton43.Checked = false;
            radioButton50.Enabled = isEnable;
            radioButton49.Enabled = isEnable;
            radioButton48.Enabled = isEnable;
            radioButton47.Enabled = isEnable;
            radioButton46.Enabled = isEnable;
            radioButton45.Enabled = isEnable;
            radioButton44.Enabled = isEnable;
            radioButton43.Enabled = isEnable;
        }

        // update the specific port list
        private void updatePortList(String[] portList, ComboBox comboBox, SerialPort serialPort)
        {
            // if list is empty, clear the content
            if (portList.Length == 0)
            {
                comboBox.Items.Clear();
                comboBox.Text = "";
                comboBox.Enabled = true;
                serialPort.Close();
                return;
            }
            String newAdded = "", newRemoved = "";
            // compare old and new list, get the newly added item
            for (int i = 0; i < portList.Length; i++)
            {
                if (!comboBox.Items.Contains(portList[i]))
                {
                    newAdded = portList[i];
                    comboBox.Items.Add(newAdded);
                }
            }
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (!portList.Contains(comboBox.Items[i].ToString()))
                {
                    newRemoved = comboBox.Items[i].ToString();
                    comboBox.Items.RemoveAt(i);
                }
            }
            // always display the newly added item to the comboBox
            if (newAdded != "" && !ports.Contains(newAdded))
            {
                comboBox.Text = newAdded;
            }
        }

        //update the serial list and add it to the comboBox 
        private void Update_Serial_List()
        {
            try
            {
                String[] cur_port_list = SerialPort.GetPortNames();

                // update serial list based on current position
                switch (position)
                {
                    case 1:
                        updatePortList(cur_port_list, comboBox1, serialPort1);
                        return;
                    case 2:
                        updatePortList(cur_port_list, comboBox2, serialPort2);
                        return;
                    case 3:
                        updatePortList(cur_port_list, comboBox3, serialPort3);
                        return;
                    case 4:
                        updatePortList(cur_port_list, comboBox4, serialPort4);
                        return;
                }
            }
            catch (Exception ex)
            {
                //exceptions occur when modifying a dropdown box when the dropdown box is opened
                Console.WriteLine("err" + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        // init serial ports and UI
        private void Form1_Load(object sender, EventArgs e)
        {
            initSerial(serialPort1);
            initSerial(serialPort2);
            initSerial(serialPort3);
            initSerial(serialPort4);
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            setHub1Report(false);
            setHub2Report(false);
            setHub3Report(false);
            setHub4Report(false);
            // scan serial list every 0.5 sec
            timer1.Start();
            Update_Serial_List();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Update_Serial_List();
            // display and blink the arrow to indicate which serial the user is setting, based on the position
            switch (position)
            {
                case 1:
                    label5.Visible = !label5.Visible;
                    break;
                case 2:
                    label6.Visible = !label6.Visible;
                    break;
                case 3:
                    label7.Visible = !label7.Visible;
                    break;
                case 4:
                    label8.Visible = !label8.Visible;
                    break;
            }
        }

        // connect button callback
        private void button_Click(object sender, EventArgs e)
        {
            // init variables
            Button button = new Button();
            Button scanButton = new Button();
            SerialPort serialPort = new SerialPort();
            ComboBox comboBox = new ComboBox();
            Label arrow = new Label();
            Label tutorial = new Label();
            Label nextTutorial = new Label();
            String info = "";
            try
            {
                // get sender name and select correct items based on button name
                String buttonName = (sender as Button).Name;
                switch (buttonName)
                {
                    case "button1":
                        button = button1;
                        scanButton = button6;
                        serialPort = serialPort1;
                        comboBox = comboBox1;
                        arrow = label5;
                        tutorial = label11;
                        nextTutorial = label21;
                        info = "Plug in the top right hub and click the connect button";
                        break;
                    case "button2":
                        button = button2;
                        scanButton = button8;
                        serialPort = serialPort2;
                        comboBox = comboBox2;
                        arrow = label6;
                        tutorial = label21;
                        nextTutorial = label31;
                        info = "Plug in the bottom left hub and click the connect button";
                        break;
                    case "button3":
                        button = button3;
                        scanButton = button10;
                        serialPort = serialPort3;
                        comboBox = comboBox3;
                        arrow = label7;
                        tutorial = label31;
                        nextTutorial = label41;
                        info = "Plug in the bottom right hub and click the connect button";
                        break;
                    case "button4":
                        button = button4;
                        scanButton = button12;
                        serialPort = serialPort4;
                        comboBox = comboBox4;
                        arrow = label8;
                        tutorial = label41;
                        nextTutorial = label41;
                        info = "Scan the barcode on the bottom of each board and start testing";
                        break;
                }
                // forbid user to manually click disconnect button
                if (ports[Int32.Parse(buttonName[6].ToString())] != null) return;

                // check whether the serial port is enabled
                if (!serialPort.IsOpen)
                {
                    // enable the serial port and change UI
                    serialPort.PortName = comboBox.Text;
                    serialPort.Open();
                    button.BackgroundImage = global::V4HubTester.Properties.Resources.disconnect;
                    comboBox.Enabled = false;
                    arrow.Visible = false;
                    nextTutorial.Enabled = true;
                    tutorial.Text = "请扫描hub底部QR码";
                    // save the serial port name into array
                    ports[position] = serialPort.PortName;
                    label9.Text = info;
                    // change to next position
                    position++;
                    // enable scan button
                    scanButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("err" + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        // serial port data receive callback
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // get serial port name
            String portName = (sender as SerialPort).PortName;
            // get index (equivalent to position), to pass the right parameters
            int index = Array.IndexOf(ports, portName);
            switch (index)
            {
                case 1:
                    processData(serialPort1, richTextBox1, label11, label10);
                    break;
                case 2:
                    processData(serialPort2, richTextBox2, label21, label20);
                    break;
                case 3:
                    processData(serialPort3, richTextBox3, label31, label30);
                    break;
                case 4:
                    processData(serialPort4, richTextBox4, label41, label40);
                    break;
                default:
                    return;
            }
        }

        // process the data from serial port
        private void processData(SerialPort serialPort, RichTextBox richTextBox, Label tutorial, Label info)
        {
            // get pos (equivalent to position)
            int pos = Array.IndexOf(ports, serialPort.PortName);

            //read from serial port and fix unix format
            String text = serialPort.ReadExisting().Replace("\r\r\n", "\r\n");
            sb.Clear();
            sb.Append(text);
            try
            {
                //call invoke method due to needs to update the UI
                Invoke((EventHandler)(delegate
                {
                    if (states["hub" + pos]["phase"] < 1 && tutorial.Text != "请扫描hub底部QR码") tutorial.Text = "开机中，请稍后...";
                    string log = sb.ToString();
                    // if unplug the serial port, this function can be called and receive "\0". So clean the UI
                    if (log == "\0")
                    {
                        richTextBox.Text = "";
                        // mark corresponding serial port can test new hub and parse the result
                        states["hub" + pos] = new Dictionary<string, int> { { "phase", 0 }, { "index", 0 } };
                        // update UI based on the position
                        switch (pos)
                        {
                            case 1:
                                label19.Text = "PCBA: ";
                                button6.Visible = true;
                                setHub1Report(false);
                                button5.Enabled = false;
                                label10.Text = "";
                                label10.ForeColor = Color.Black;
                                label11.Text = "请扫描hub底部QR码";
                                break;
                            case 2:
                                label29.Text = "PCBA: ";
                                button8.Visible = true;
                                setHub2Report(false);
                                button7.Enabled = false;
                                label20.Text = "";
                                label20.ForeColor = Color.Black;
                                label21.Text = "请扫描hub底部QR码";
                                break;
                            case 3:
                                label39.Text = "PCBA: ";
                                button10.Visible = true;
                                setHub3Report(false);
                                button9.Enabled = false;
                                label30.Text = "";
                                label30.ForeColor = Color.Black;
                                label31.Text = "请扫描hub底部QR码";
                                break;
                            case 4:
                                label49.Text = "PCBA: ";
                                button12.Visible = true;
                                setHub4Report(false);
                                button11.Enabled = false;
                                label40.Text = "";
                                label40.ForeColor = Color.Black;
                                label41.Text = "请扫描hub底部QR码";
                                break;
                        }
                        return;
                    }

                    // append data on UI
                    richTextBox.AppendText(log);
                    richTextBox.HideSelection = false;
                    var text = richTextBox.Text.Substring(states["hub" + pos]["index"]);
                    switch (states["hub" + pos]["phase"])
                    {
                        case 0: // hub is launching, monitoring when it's done.
                            if (text.IndexOf("new high-speed USB device number 3 using ci_hdrc") == -1) return;
                            states["hub" + pos]["phase"] = 1;
                            states["hub" + pos]["index"] = richTextBox.Text.Length;
                            tutorial.Text = "请按黑黑红红按键开始测试";
                            break;
                        case 1: // hub has launched, monitoring when it starts testing 
                            if (text.IndexOf("Begin TF-Card testing...") == -1) return;
                            states["hub" + pos]["phase"] = 2;
                            states["hub" + pos]["index"] = richTextBox.Text.Length;
                            tutorial.Text = "正在测试，请稍后...";
                            break;
                        case 2: // hub is testing, monitoring when it's done and need user to see the LED
                            if (text.IndexOf("Press any key to continue...") == -1) return;
                            states["hub" + pos]["phase"] = 3;
                            states["hub" + pos]["index"] = richTextBox.Text.Length;
                            tutorial.Text = "请按任意按键，观察LED";
                            break;
                        case 3: // hub is waiting for user input, monitoring when need user to input more keys
                            if (text.IndexOf("Begin KEYs testing...") == -1) return;
                            states["hub" + pos]["phase"] = 4;
                            states["hub" + pos]["index"] = richTextBox.Text.Length;
                            tutorial.Text = "请按所有按键(红,黑,reset)";
                            break;
                        case 4:// hub is waiting for user input, monitoring where is the end of the test report
                            if (text.IndexOf("result_end") == -1) return;
                            states["hub" + pos]["phase"] = 5;
                            tutorial.Text = "请根据实际情况输入结果";
                            info.Text = "完成后点击保存";
                            using (var db = new HubDBContext())
                            {

                                Hub hubObj = new Hub();

                                // extract the data from serial log
                                var pcbaCpuIndex = text.IndexOf("PCBA:  CPU");
                                if (pcbaCpuIndex != -1) hubObj.PCBACPU = text.Substring(pcbaCpuIndex + 13, 16);

                                var pcbaEth0Index = text.IndexOf("PCBA: ETH0");
                                if (pcbaEth0Index != -1) hubObj.PCBAETH0 = text.Substring(pcbaEth0Index + 13, 4);

                                var pcbaWifiIndex = text.IndexOf("PCBA: WiFi");
                                if (pcbaWifiIndex != -1) hubObj.PCBAWiFi = text.Substring(pcbaWifiIndex + 13, 17);

                                var pcbaBtIndex = text.IndexOf("PCBA:   BT");
                                if (pcbaBtIndex != -1) hubObj.PCBABT = text.Substring(pcbaBtIndex + 13, 17);

                                var pcbaImeiIndex = text.IndexOf("PCBA: IMEI");
                                if (pcbaImeiIndex != -1) hubObj.PCBAIMEI = text.Substring(pcbaImeiIndex + 13, 15);

                                var pcbaCcidIndex = text.IndexOf("PCBA: CCID");
                                if (pcbaCcidIndex != -1) hubObj.PCBACCID = text.Substring(pcbaCcidIndex + 13, 20);

                                var tfCardCapIndex = text.IndexOf("TFCard:  Cap");
                                if (tfCardCapIndex != -1) hubObj.TFCardCap = text.Substring(tfCardCapIndex + 15, 4);

                                var adcDcIndex = text.IndexOf("ADC:   DC");
                                if (adcDcIndex != -1) hubObj.ADCDC = text.Substring(adcDcIndex + 12, 5);

                                var adcBatIndex = text.IndexOf("ADC:  BAT");
                                if (adcBatIndex != -1) hubObj.ADCBAT = text.Substring(adcBatIndex + 12, 5);

                                var adcLteIndex = text.IndexOf("ADC:  LTE");
                                if (adcLteIndex != -1) hubObj.ADCLTE = text.Substring(adcLteIndex + 12, 6);

                                var eth0PingIndex = text.IndexOf("ETH0: PING");
                                if (eth0PingIndex != -1) hubObj.ETH0PING = text.Substring(eth0PingIndex + 13, 4);

                                var ltePwrIndex = text.IndexOf("LTE:  PWR");
                                if (ltePwrIndex != -1) hubObj.LTEPWR = text.Substring(ltePwrIndex + 12, 4);

                                var lteWdisIndex = text.IndexOf("LTE: WDIS");
                                if (lteWdisIndex != -1) hubObj.LTEWDIS = text.Substring(lteWdisIndex + 12, 4);

                                var lteCommIndex = text.IndexOf("LTE: COMM");
                                if (lteCommIndex != -1) hubObj.LTECOMM = text.Substring(lteCommIndex + 12, 4);

                                var zwavePwrIndex = text.IndexOf("ZWAVE:  PWR");
                                if (zwavePwrIndex != -1) hubObj.ZWAVEPWR = text.Substring(zwavePwrIndex + 14, 4);

                                var zwaveCommIndex = text.IndexOf("ZWAVE: COMM");
                                if (zwaveCommIndex != -1) hubObj.ZWAVECOMM = text.Substring(zwaveCommIndex + 14, 4);

                                var zwaveNvrIndex = text.IndexOf("ZWAVE:  NVR");
                                if (zwaveNvrIndex != -1) hubObj.ZWAVENVR = text.Substring(zwaveNvrIndex + 14, 4);

                                var wifiPingIndex = text.IndexOf("Wi-Fi: PING");
                                if (wifiPingIndex != -1) hubObj.WiFiPING = text.Substring(wifiPingIndex + 14, 4);

                                var btScanIndex = text.IndexOf("BT: SCAN");
                                if (btScanIndex != -1) hubObj.BTSCAN = text.Substring(btScanIndex + 11, 4);

                                // replace RESULTTIME by current time
                                hubObj.RESULTTIME = DateTime.Now.ToString("yyyyMMddHHmmss");
                                resultTimes["hub" + pos] = hubObj.RESULTTIME;

                                hubObj.KeyUp = richTextBox.Text.IndexOf("[ KeyUp   ] has been detected.") != -1;
                                hubObj.KeyDown = richTextBox.Text.IndexOf("[ KeyDown ] has been detected.") != -1;
                                hubObj.KeyPwr = richTextBox.Text.IndexOf("[ KeyPwr ] has been detected.") != -1;

                                // get QRcode and save into db
                                switch (pos)
                                {
                                    case 1:
                                        button5.Enabled = true;
                                        hubObj.PCBA = label19.Text.Substring(6);
                                        break;
                                    case 2:
                                        button7.Enabled = true;
                                        hubObj.PCBA = label29.Text.Substring(6);
                                        break;
                                    case 3:
                                        button9.Enabled = true;
                                        hubObj.PCBA = label39.Text.Substring(6);
                                        break;
                                    case 4:
                                        button11.Enabled = true;
                                        hubObj.PCBA = label49.Text.Substring(6);
                                        break;
                                }

                                // add to db
                                db.Hubs.Add(hubObj);
                                db.SaveChanges();
                            }
                            mainForm.presentNewestTable();
                            break;
                        default:
                            return;
                    }
                }));
            }
            catch (Exception ex)
            {
                //beep and present the error to the user
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(ex.Message);
            }
        }

        // close all serial ports and clear UI
        private void closeSerialPorts()
        {
            try
            {
                serialPort1.Close();
                serialPort2.Close();
                serialPort3.Close();
                serialPort4.Close();
                // update richTextBox on another thread
                richTextBox1.Invoke(new MethodInvoker(() =>
                {
                    richTextBox1.Text = "";
                }));
                richTextBox2.Invoke(new MethodInvoker(() =>
                {
                    richTextBox2.Text = "";
                }));
                richTextBox3.Invoke(new MethodInvoker(() =>
                {
                    richTextBox3.Text = "";
                }));
                richTextBox4.Invoke(new MethodInvoker(() =>
                {
                    richTextBox4.Text = "";
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); //catch any serial port closing error messages
            }
        }

        //reset UI and serial ports
        private void button13_Click(object sender, EventArgs e)
        {
            position = 1;
            //close port in new thread to avoid hang
            Thread closeDown = new Thread(new ThreadStart(closeSerialPorts));
            closeDown.Start();
            // reset UI
            button1.BackgroundImage = global::V4HubTester.Properties.Resources.connect;
            button2.BackgroundImage = global::V4HubTester.Properties.Resources.connect;
            button3.BackgroundImage = global::V4HubTester.Properties.Resources.connect;
            button4.BackgroundImage = global::V4HubTester.Properties.Resources.connect;
            button6.Visible = true;
            button8.Visible = true;
            button10.Visible = true;
            button12.Visible = true;
            button6.Enabled = false;
            button8.Enabled = false;
            button10.Enabled = false;
            button12.Enabled = false;
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label11.Text = "连接串口";
            label21.Text = "连接串口";
            label31.Text = "连接串口";
            label41.Text = "连接串口";
            label21.Enabled = false;
            label31.Enabled = false;
            label41.Enabled = false;
            label19.Text = "PCBA: ";
            label29.Text = "PCBA: ";
            label39.Text = "PCBA: ";
            label49.Text = "PCBA: ";
            setHub1Report(false);
            setHub2Report(false);
            setHub3Report(false);
            setHub4Report(false);
            states = new Dictionary<String, Dictionary<String, int>>()
            {
                { "hub1", new Dictionary<String, int>() { { "phase", 0 }, { "index", 0 } } },
                { "hub2", new Dictionary<String, int>() { { "phase", 0 }, { "index", 0 } } },
                { "hub3", new Dictionary<String, int>() { { "phase", 0 }, { "index", 0 } } },
                { "hub4", new Dictionary<String, int>() { { "phase", 0 }, { "index", 0 } } },
            };
            ports = new String[5];
            label9.Text = "Plug in the top left hub and click the connect button";
        }

        // callback function for all scan buttons
        private void button6_Click(object sender, EventArgs e)
        {
            // pass db to verify duplication; pass button name to identify the sender
            Form inputDialog = new InputDialog(this, (sender as Button).Name);
            inputDialog.Show();
        }

        // callback function for inputDialog
        public void passQRCode(String qrCode, String buttonName)
        {
            // update QRcode based on the sender button name
            switch (buttonName)
            {
                case "button6":
                    label19.Text += qrCode;
                    button6.Visible = false;
                    label11.Text = label11.Text == "请扫描hub底部QR码" ? "通电启动hub" : label11.Text;
                    setHub1Report(true);
                    break;
                case "button8":
                    label29.Text += qrCode;
                    button8.Visible = false;
                    label21.Text = label21.Text == "请扫描hub底部QR码" ? "通电启动hub" : label21.Text;
                    setHub2Report(true);
                    break;
                case "button10":
                    label39.Text += qrCode;
                    button10.Visible = false;
                    label31.Text = label31.Text == "请扫描hub底部QR码" ? "通电启动hub" : label31.Text;
                    setHub3Report(true);
                    break;
                case "button12":
                    label49.Text += qrCode;
                    button12.Visible = false;
                    label41.Text = label41.Text == "请扫描hub底部QR码" ? "通电启动hub" : label41.Text;
                    setHub4Report(true);
                    break;
            }
        }

        // callback function to verify radio buttons are not empty, and save the results to db
        private void button5_Click(object sender, EventArgs e)
        {
            if (!((radioButton8.Checked || radioButton7.Checked) && (radioButton6.Checked || radioButton5.Checked) && (radioButton4.Checked || radioButton3.Checked) && (radioButton2.Checked || radioButton1.Checked)))
            {
                MessageBox.Show("Please input all results, then save them.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var db = new HubDBContext())
            {
                // resultTime is the identifier
                String resultTime = resultTimes["hub1"];
                var hub = db.Hubs.OrderByDescending(h => h.id).FirstOrDefault(h => h.RESULTTIME == resultTime);
                hub.GreenLights = radioButton8.Checked;
                hub.RGBLight = radioButton6.Checked;
                hub.USBScreen = radioButton4.Checked;
                hub.Ammeter = radioButton2.Checked;
                db.SaveChanges();
            }
            label10.ForeColor = Color.Green;
            label10.Text = "Saved";
        }

        // callback function to verify radio buttons are not empty, and save the results to db
        private void button7_Click(object sender, EventArgs e)
        {
            if (!((radioButton15.Checked || radioButton16.Checked) && (radioButton17.Checked || radioButton18.Checked) && (radioButton19.Checked || radioButton20.Checked) && (radioButton21.Checked || radioButton22.Checked)))
            {
                MessageBox.Show("Please verify the all items and input the results, then save them.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var db = new HubDBContext())
            {
                // resultTime is the identifier
                String resultTime = resultTimes["hub2"];
                var hub = db.Hubs.OrderByDescending(h => h.id).FirstOrDefault(h => h.RESULTTIME == resultTime);
                hub.GreenLights = radioButton22.Checked;
                hub.RGBLight = radioButton20.Checked;
                hub.USBScreen = radioButton18.Checked;
                hub.Ammeter = radioButton16.Checked;
                db.SaveChanges();
            }
            label20.ForeColor = Color.Green;
            label20.Text = "Saved";
        }

        // callback function to verify radio buttons are not empty, and save the results to db
        private void button9_Click(object sender, EventArgs e)
        {
            if (!((radioButton29.Checked || radioButton30.Checked) && (radioButton31.Checked || radioButton32.Checked) && (radioButton33.Checked || radioButton34.Checked) && (radioButton35.Checked || radioButton36.Checked)))
            {
                MessageBox.Show("Please verify the all items and input the results, then save them.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var db = new HubDBContext())
            {
                // resultTime is the identifier
                String resultTime = resultTimes["hub3"];
                var hub = db.Hubs.OrderByDescending(h => h.id).FirstOrDefault(h => h.RESULTTIME == resultTime);
                hub.GreenLights = radioButton36.Checked;
                hub.RGBLight = radioButton34.Checked;
                hub.USBScreen = radioButton32.Checked;
                hub.Ammeter = radioButton30.Checked;
                db.SaveChanges();
            }
            label30.ForeColor = Color.Green;
            label30.Text = "Saved";
        }

        // callback function to verify radio buttons are not empty, and save the results to db
        private void button11_Click(object sender, EventArgs e)
        {
            if (!((radioButton43.Checked || radioButton44.Checked) && (radioButton45.Checked || radioButton46.Checked) && (radioButton47.Checked || radioButton48.Checked) && (radioButton49.Checked || radioButton50.Checked)))
            {
                MessageBox.Show("Please verify the all items and input the results, then save them.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var db = new HubDBContext())
            {
                // resultTime is the identifier
                String resultTime = resultTimes["hub4"];
                var hub = db.Hubs.OrderByDescending(h => h.id).FirstOrDefault(h => h.RESULTTIME == resultTime);
                hub.GreenLights = radioButton50.Checked;
                hub.RGBLight = radioButton48.Checked;
                hub.USBScreen = radioButton46.Checked;
                hub.Ammeter = radioButton44.Checked;
                db.SaveChanges();
            }
            label40.ForeColor = Color.Green;
            label40.Text = "Saved";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.MainForm_FormClosing(sender, e);
        }
    }
}
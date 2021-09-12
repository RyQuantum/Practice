using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Net.Http;
using System.Threading.Tasks;

namespace Host
{
    public class SerialPortUtils
    {
        static Form0 form1;
        public static string[] GetPortNames(Form0 form)
        {
            form1 = form;
            return SerialPort.GetPortNames();
        }

        public static SerialPort SerialPort = null;
        public static SerialPort OpenClosePort(string comName, int baud)
        {
            //串口未打开
            if (SerialPort == null || !SerialPort.IsOpen)
            {
                SerialPort = new SerialPort();
                //串口名称
                SerialPort.PortName = comName;
                //波特率
                SerialPort.BaudRate = baud;
                //数据位
                SerialPort.DataBits = 8;
                //停止位
                SerialPort.StopBits = StopBits.One;
                //校验位
                SerialPort.Parity = Parity.None;
                //打开串口
                SerialPort.Open();
                //串口数据接收事件实现
                SerialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);

                return SerialPort;
            }
            //串口已经打开
            else
            {
                SerialPort.Close();
                return SerialPort;
            }
        }

        public static async void ReceiveData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort _SerialPort = (SerialPort)sender;

            int _bytesToRead = _SerialPort.BytesToRead;
            byte[] recvData = new byte[_bytesToRead];

            _SerialPort.Read(recvData, 0, _bytesToRead);
            string str = System.Text.Encoding.ASCII.GetString(recvData);
            //向控制台打印数据
            Debug.WriteLine("Received：" + str);
            form1.ReceiveDataCallback(str);
        }

        public static bool SendData(byte[] data)
        {
            if (SerialPort != null && SerialPort.IsOpen)
            {
                SerialPort.Write(data, 0, data.Length);
                Debug.WriteLine("Sent：" + BitConverter.ToString(data).Replace("-", ""));
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

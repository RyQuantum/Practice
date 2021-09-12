using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Host
{
    public partial class Form0 : Form
    {
        private readonly HttpClient HttpClient = new HttpClient();

        //Native serial library demo
        public Form0()
        {
            InitializeComponent();
        }

        public byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public async Task<bool> DownloadFile(string url, string directoryName, string fileName)
        {
            bool sign = true;
            try
            {
                HttpResponseMessage response = await HttpClient.GetAsync(url);
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    //获取文件后缀
                    string extension = Path.GetExtension(response.RequestMessage.RequestUri.ToString());
                    string file = $"{directoryName}/{fileName}{extension}";
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                    using (FileStream fileStream = new FileStream(file, FileMode.CreateNew))
                    {
                        byte[] buffer = new byte[10000];
                        int readLength = 0;
                        int length;
                        while ((length = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                        {
                            readLength += length;
                            // 写入到文件  
                            fileStream.Write(buffer, 0, length);
                        }
                    }
                }
            }
            catch (IOException)
            {
                //这里的异常捕获并不完善，请结合实际操作而定  
                sign = false;
                Console.WriteLine("Downloader.DownloadFile：请检查文件名是否重复！");
            }
            return sign;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await DownloadFile("http://127.0.0.1:8888/server.key", "tmp", "server");
            listBox1.Items.Insert(0, "Server key downloaded");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] portNames = SerialPortUtils.GetPortNames(this);

            if (portNames != null)
            {
                foreach (string name in portNames)
                {
                    Debug.WriteLine(name);
                }

                SerialPortUtils.OpenClosePort("COM7", 115200);
                listBox1.Items.Insert(listBox1.Items.Count, "Open serial port 7");
                SerialPortUtils.SendData(StringToByteArray("BF112233445566FB"));
                listBox1.Items.Insert(listBox1.Items.Count, "Sent request: BF112233445566FB");

            }
        }

        public void print(string str)
        {
            listBox1.Invoke(new MethodInvoker(() => listBox1.Items.Insert(listBox1.Items.Count, str)));
        }

        private string received = "";
        public void ReceiveDataCallback(string str)
        {
            print("Received: " + str);

            received += str;
            if (received.Length >= 34 && received.Substring(received.Length - 34) != "-----END CERTIFICATE REQUEST-----\0") return;
            _ = SaveFile(received);
        }

        public async Task SaveFile(string text)
        {
            await File.WriteAllTextAsync("tmp/client.csr", text);
            print("Save csr file");
            LaunchCommandLineApp();
        }

        public void LaunchCommandLineApp()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;   //是否使用操作系统shell启动 
            process.StartInfo.CreateNoWindow = false;   //是否在新窗口中启动该进程的值 (不显示程序窗口)
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            string str = ".\\openssl\\openssl.exe x509 -req -in .\\tmp\\client.csr -out .\\tmp\\client.crt -signkey .\\tmp\\server.key -days 365";
            Console.WriteLine("Generate crt file");
            print("Generate crt file");
            print("Send back to device");
            process.StandardInput.WriteLine(str + "&exit");
            process.WaitForExit();  //等待程序执行完退出进程
            process.Close();
            string text = System.IO.File.ReadAllText(@".\\tmp\\client.cst");
            byte[] b = System.Text.Encoding.UTF8.GetBytes(text);
            //SerialPortUtils.SendData(b);
            SerialPortUtils.SendData(StringToByteArray("BF112233445566FB"));
        }
    }
}

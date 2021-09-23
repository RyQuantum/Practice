using Device.Net;
using Hid.Net.Windows;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Host
{
    public partial class Form2 : Form
    {
        //Device.Net demo
        Thread thread;
        IDeviceFactory hidFactory;
        IDevice device;
        byte[] result = new byte[] { };
        public Form2()
        {
            var loggerFactory = LoggerFactory.Create((builder) =>
            {
                _ = builder.AddDebug().SetMinimumLevel(LogLevel.Trace);
            });
            hidFactory =
                new FilterDeviceDefinition(vendorId: 0x2FE3, productId: 0x0100)
                .CreateWindowsHidDeviceFactory(loggerFactory);
            InitializeComponent();
            //Main();
        }

        private void Scan()
        {
            _ = ScanAsync();
        }

        private async Task ScanAsync()
        {
            while (true)
            {
                var deviceDefinitions = (await hidFactory.GetConnectedDeviceDefinitionsAsync().ConfigureAwait(false)).ToList();
                if (deviceDefinitions.Count == 0) continue;
                await InitializeAsync(deviceDefinitions);
            }
        }

        private async Task InitializeAsync(List<ConnectedDeviceDefinition> deviceDefinitions)
        {
            device = await hidFactory.GetDeviceAsync(deviceDefinitions.First()).ConfigureAwait(false);

            //Initialize the device
            await device.InitializeAsync().ConfigureAwait(false);

            //Create the request buffer
            var buffer = new byte[65];
            buffer[0] = 0x01;
            buffer[1] = 0x3f;
            buffer[2] = 0x23;
            buffer[3] = 0x23;

            //Write and read the data to the device
            var readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
            result = readBuffer.Data;
            await ReceiveAsync();
        }

        private async Task ReceiveAsync()
        {
            while (System.Text.Encoding.ASCII.GetString(result.Skip(result.Length - 33).ToArray()) != "-----END CERTIFICATE REQUEST-----")
            {
                var readBuffer = await device.ReadAsync().ConfigureAwait(false);
                result = result.Skip(0).Concat(readBuffer.Data.Skip(0)).ToArray();
                //byte[] rv = new byte[result.Length + readBuffer.Data.Length];
                //System.Buffer.BlockCopy(result, 0, rv, 0, result.Length);
                //System.Buffer.BlockCopy(readBuffer.Data, 0, rv, result.Length, readBuffer.Data.Length);
                //result = rv;
            }
            await SaveFile();
        }

        private async Task SaveFile()
        {
            if (!Directory.Exists("tmp"))
            {
                Directory.CreateDirectory("tmp");
            }
            await File.WriteAllBytesAsync("tmp/client.csr", result);
            Debug.WriteLine("Save csr file");
            GenerateCertificate();
        }

        private void GenerateCertificate()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;   //是否使用操作系统shell启动 
            process.StartInfo.CreateNoWindow = false;   //是否在新窗口中启动该进程的值 (不显示程序窗口)
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            string str1 = ".\\openssl\\openssl.exe x509 -req -in .\\tmp\\client.csr -out .\\tmp\\client.crt -signkey server.key -days 365";
            Debug.WriteLine("Generate crt file");
            //print("Generate crt file");
            //print("Send back to device");
            process.StandardInput.WriteLine(str1);
            string str2 = ".\\openssl\\openssl.exe req -in .\\tmp\\client.csr -noout -pubkey -config .\\openssl\\openssl.cnf -out .\\tmp\\client_pub.key";
            process.StandardInput.WriteLine(str2 + "&exit");
            process.WaitForExit();  //等待程序执行完退出进程
            process.Close();
            //byte[] b = System.Text.Encoding.UTF8.GetBytes(text);
            //SerialPortUtils.SendData(b);
            //SerialPortUtils.SendData(StringToByteArray("BF112233445566FB"));
            _ = ReadAndSendAsync();
        }

        private async Task ReadAndSendAsync()
        {
            Byte[] bytes = System.IO.File.ReadAllBytes(".\\tmp\\client.crt");
            var num = bytes.Length / 64;
            if (num * 64 < bytes.Length) num++;
            for (int i = 0; i < num; i++)
            {
                var buffer = bytes.Skip(i * 64).Take(64).Prepend((byte) 0).ToArray();
                await device.WriteAsync(buffer).ConfigureAwait(false);
            }
        }

        //private static async Task Main()
        //{
        //    //Create logger factory that will pick up all logs and output them in the debug output window
        //    var loggerFactory = LoggerFactory.Create((builder) =>
        //    {
        //        _ = builder.AddDebug().SetMinimumLevel(LogLevel.Trace);
        //    });

        //    //----------------------

        //    // This is Windows specific code. You can replace this with your platform of choice or put this part in the composition root of your app

        //    //Register the factory for creating Hid devices. 
        //    var hidFactory =
        //        new FilterDeviceDefinition(vendorId: 0x2FE3, productId: 0x0100)
        //        .CreateWindowsHidDeviceFactory(loggerFactory);

        //    //----------------------

        //    //Get connected device definitions
        //    var deviceDefinitions = (await hidFactory.GetConnectedDeviceDefinitionsAsync().ConfigureAwait(false)).ToList();

        //    if (deviceDefinitions.Count == 0)
        //    {
        //        //No devices were found
        //        return;
        //    }

        //    //Get the device from its definition
        //    var device = await hidFactory.GetDeviceAsync(deviceDefinitions.First()).ConfigureAwait(false);

        //    //Initialize the device
        //    await device.InitializeAsync().ConfigureAwait(false);

        //    //Create the request buffer
        //    var buffer = new byte[12800];
        //    buffer[0] = 0x01;
        //    buffer[1] = 0x3f;
        //    buffer[2] = 0x23;
        //    buffer[3] = 0x23;

        //    DateTime dd = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //    long timeStamp = (Int64)(DateTime.Now - dd).TotalMilliseconds;

        //    //Write and read the data to the device
        //    var readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
        //    readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
        //    readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
        //    readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
        //    readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
        //    readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
        //    readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
        //    readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
        //    readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
        //    readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);

        //    var diff = (Int64)(DateTime.Now - dd).TotalMilliseconds - timeStamp;

        //    System.Diagnostics.Debug.WriteLine("diff: " + diff);
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Start");
            if (thread != null) return;
            Debug.WriteLine("New thread");
            thread = new Thread(Scan);
        }
    }
}

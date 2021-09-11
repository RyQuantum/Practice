using Device.Net;
using Usb.Net.Windows;
using Hid.Net.Windows;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;

namespace HIDTest
{
    public partial class Form2 : Form
    {
        //Device.Net demo
        public Form2()
        {
            InitializeComponent();
            Main();
        }

        private static async Task Main()
        {
            //Create logger factory that will pick up all logs and output them in the debug output window
            var loggerFactory = LoggerFactory.Create((builder) =>
            {
                _ = builder.AddDebug().SetMinimumLevel(LogLevel.Trace);
            });

            //----------------------

            // This is Windows specific code. You can replace this with your platform of choice or put this part in the composition root of your app

            //Register the factory for creating Hid devices. 
            var hidFactory =
                new FilterDeviceDefinition(vendorId: 0x2FE3, productId: 0x0100)
                .CreateWindowsHidDeviceFactory(loggerFactory);

            //----------------------

            //Get connected device definitions
            var deviceDefinitions = (await hidFactory.GetConnectedDeviceDefinitionsAsync().ConfigureAwait(false)).ToList();

            if (deviceDefinitions.Count == 0)
            {
                //No devices were found
                return;
            }

            //Get the device from its definition
            var device = await hidFactory.GetDeviceAsync(deviceDefinitions.First()).ConfigureAwait(false);

            //Initialize the device
            await device.InitializeAsync().ConfigureAwait(false);

            //Create the request buffer
            var buffer = new byte[12800];
            buffer[0] = 0x01;
            buffer[1] = 0x3f;
            buffer[2] = 0x23;
            buffer[3] = 0x23;

            DateTime dd = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long timeStamp = (Int64)(DateTime.Now - dd).TotalMilliseconds;

            //Write and read the data to the device
            var readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
            readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
            readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
            readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
            readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
            readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
            readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
            readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
            readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
            readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);

            var diff = (Int64)(DateTime.Now - dd).TotalMilliseconds - timeStamp;

            System.Diagnostics.Debug.WriteLine("diff: " + diff);
        }
    }
}

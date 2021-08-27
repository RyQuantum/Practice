using Device.Net;
using Usb.Net.Windows;
using Hid.Net.Windows;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIDTest
{
    public partial class Form2 : Form
    {
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
                new FilterDeviceDefinition(vendorId: 0x1995, productId: 0x0806)
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
            var buffer = new byte[65];
            buffer[0] = 0x00;
            buffer[1] = 0x3f;
            buffer[2] = 0x23;
            buffer[3] = 0x23;

            //Write and read the data to the device
            var readBuffer = await device.WriteAndReadAsync(buffer).ConfigureAwait(false);
            System.Diagnostics.Debug.WriteLine(readBuffer);
        }
    }
}

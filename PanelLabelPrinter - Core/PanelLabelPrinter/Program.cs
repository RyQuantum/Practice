using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace PanelLabelPrinter
{
    static class Program
    {
        /// <summary>
        /////  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo("local-server\\Python38-32\\python.exe", "local-server\\main.py");
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConfigurationForm(process));
        }
    }
}

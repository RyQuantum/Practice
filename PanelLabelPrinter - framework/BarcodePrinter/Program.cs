using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodePrinter
{
    static class Program
    {
        private static System.Threading.Mutex mutex;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mutex = new System.Threading.Mutex(true, "OnlyRun");
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("程序已经在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                return;
            }

            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Rently\\Label Printer (panel)");
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo("local-server\\Python38-32\\python.exe", "local-server\\main.py");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo = startInfo;
            process.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConfigurationForm(process));
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("I'm out of here");
        }
    }
}

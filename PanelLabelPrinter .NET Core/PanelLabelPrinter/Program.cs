using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace BarcodePrinter
{
    static class Program
    {
        private static System.Threading.Mutex mutex;
        /// <summary>
        /////  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            mutex = new System.Threading.Mutex(true, "Panel Label Printer");
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("程序已经在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                return;
            }
            Process process = new Process();
            process.StartInfo.FileName = "server.exe";
            //process.StartInfo.CreateNoWindow = true;
            process.Start();

            Assembly.GetExecutingAssembly().GetManifestResourceStream()

            ApplicationConfiguration.Initialize();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Rently\\Label Printer (panel)\\";
            string templateFile = path + "template.lsdx";
            if (!File.Exists(templateFile))
            {
                byte[] template = global::BarcodePrinter.Properties.Resources.template;
                using (FileStream fs = new FileStream(templateFile, FileMode.OpenOrCreate))
                {
                    fs.Write(template, 0, template.Length);
                }
            }
            using (var db = new MyDatabase(@"data source=" + path + "sqlite.db"))
            {
                var config = db.Configs.FirstOrDefault(f => true);
                if (config != null)
                {
                    MainForm mainForm = new MainForm(Int32.Parse(config.qty), config.model, Int32.Parse(config.weight), config.po, process);
                    Application.Run(mainForm);
                }
                else
                {
                    ConfigurationForm configForm = new ConfigurationForm(process);
                    Application.Run(configForm);
                }
            }
        }
    }
}

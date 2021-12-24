namespace FobRegister
{
    internal static class Program
    {
        private static System.Threading.Mutex mutex;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // single instance check
            mutex = new System.Threading.Mutex(true, "Fob Register");
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("程序已经在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                return;
            }

            // Set working directory
            string applicationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(applicationPath + @"\Rently"))
            {
                Directory.CreateDirectory(applicationPath + @"\Rently");
            }
            if (!Directory.Exists(applicationPath + @"\Rently\Fob Register"))
            {
                Directory.CreateDirectory(applicationPath + @"\Rently\Fob Register");
            }
            Environment.CurrentDirectory = applicationPath + @"\Rently\Fob Register";

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Login());
        }
    }
}
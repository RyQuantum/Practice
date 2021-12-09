namespace V4HubTester
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
            mutex = new System.Threading.Mutex(true, "OnlyRun");
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("程序已经在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                return;
            }
            // set the sqlite file path
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Rently\\V4Hub Tester\\sqlite.db";
            using (var db = new HubDBContext(@"data source=" + filePath))
            {
                var count = db.Hubs.Count();
                Console.WriteLine("count: " + count);
            }
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(filePath));
        }
    }
}
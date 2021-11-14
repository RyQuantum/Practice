namespace V4HubTester
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Form2());

            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Rently\\V4Hub Tester\\sqlite.db";
            using (var db = new HubDBContext(@"data source=" + filePath))
            {
                var hubs = db.Hubs.Where(hub => true).ToArray();
                //if (hubs.Length > 0 && hubs[hubs.Length - 1] != null && hubs[hubs.Length - 1].HubMac is not String)
                //{
                //    db.Hubs.Remove(hubs[hubs.Length - 1]);
                //    db.SaveChanges();
                //    hubs = hubs.Take(hubs.Length - 1).ToArray();
                //}
                //Application.SetHighDpiMode(HighDpiMode.SystemAware);
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1(db, hubs));
            }
        }
    }
}
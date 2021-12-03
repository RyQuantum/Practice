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
            // set the sqlite file path
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Rently\\V4Hub Tester\\sqlite.db";
            using (var db = new HubDBContext(@"data source=" + filePath))
            {
                var count = db.Hubs.Count();
                // only get the latest 20 records
                var hubs = db.Hubs.Where(hub => count - hub.id < 10).ToArray();
                //Application.Run(new MainForm(db, filePath, hubs));
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1(db));
            }

            //Application.Run(new Form1());
        }
    }
}
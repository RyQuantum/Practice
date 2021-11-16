using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialAssistant
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Rently\\V4Hub Tester\\sqlite.db";
            using (var db = new HubDBContext(@"data source=" + filePath))
            {
                var hubs = db.Hubs.Where(hub => true).ToArray();
                Application.Run(new Form2(db, hubs));
            }
        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FobRegister
{
    public partial class Login : Form
    {
        private HttpClient httpClient = new HttpClient { Timeout = TimeSpan.FromMilliseconds(30000) };
        public Login()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            textBox2.KeyDown += new KeyEventHandler(textBox_KeyDown);
            textBox1.Text = "admin@gmail.com";
            textBox2.Text = "admin";
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            using (var db = new LocalDBContext())
            {
                var fob = db.Fobs.FirstOrDefault(f => true);
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) login();
        }

        private async void login()
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button1.Enabled = false;

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("username", textBox1.Text));
            values.Add(new KeyValuePair<string, string>("password", textBox2.Text));
            values.Add(new KeyValuePair<string, string>("grant_type", "password"));
            values.Add(new KeyValuePair<string, string>("factory", "true"));
            var content = new FormUrlEncodedContent(values);
            try
            {
                var response = await httpClient.PostAsync("https://keyless.rentlyopensesame.com/api/agents", content);
                var responseString = await response.Content.ReadAsStringAsync();
                JObject res = JObject.Parse(responseString);
                if ((string)res["access_token"] != null)
                {

                    Form form1 = new Main((string)res["access_token"]);
                    form1.ShowDialog();
                    Application.ExitThread();
                }
                else
                {
                    Console.WriteLine(res);
                    MessageBox.Show((string)res["error_description"], (string)res["error"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (TaskCanceledException ex)
            {
                MessageBox.Show("Login API timeout!", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            login();
        }
    }
}

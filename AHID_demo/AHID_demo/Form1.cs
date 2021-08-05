using System;
using System.Text;
using System.Windows.Forms;

namespace AHID_demo
{
    public partial class Form1 : Form
    {
        void timerCallback(object sender, EventArgs e)
        {
            read();

            if (0 == (findInterval++ % 10))
            {
                find();
            }
        }

        public Form1()
        {
            InitializeComponent();

            Timer timer = new Timer();

            timer.Interval = TIMER_INTERVAL;
            timer.Tick += new EventHandler(timerCallback);
            timer.Start();
            var rsa = new RSACryption();
            string pubKey = "";
            string privKey = "";
            rsa.RSAKey(out privKey, out pubKey);
            string encryptedString = rsa.RSAEncrypt(pubKey, "Ryan test");
            System.Diagnostics.Debug.WriteLine("encrypted: " + encryptedString);
            string decryptedString = rsa.RSADecrypt(privKey, encryptedString);
            System.Diagnostics.Debug.WriteLine("res: " + decryptedString);

            string output = "";
            string testSrt = "Ryan2 test";
            string md5 = "";
            rsa.GetHash(testSrt, ref md5);
            rsa.SignatureFormatter(privKey, md5, ref output);
            System.Diagnostics.Debug.WriteLine("output: " + output);
            var res = rsa.SignatureDeformatter(pubKey, md5, output);
            System.Diagnostics.Debug.WriteLine("res2: " + res.ToString());
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            connect();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            write();
        }
    }
}

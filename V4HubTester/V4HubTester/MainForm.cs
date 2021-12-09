using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace V4HubTester
{
    public partial class MainForm : Form
    {
        private DataTable hubTable = new DataTable();
        String filePath;
        private Form form1;
        public MainForm(String filePath)
        {
            this.filePath = filePath;
            InitializeComponent();
            dataGridView1.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);

            using (var db = new HubDBContext())
            {
                var count = db.Hubs.Count();
                var hubs = db.Hubs.Where(hub => count - hub.id < 10).ToArray();
                bindDataSource(hubs);
            }
            form1 = new Form1(this);
            form1.Show();
        }

        // called from out of the class
        public void presentNewestTable()
        {
            using (var db = new HubDBContext())
            {
                var count = db.Hubs.Count();
                // only get the latest 10 records
                var hubs = db.Hubs.Where(hub => count - hub.id < 10).ToArray();
                bindDataSource(hubs);
                DisableDefaultSelection();
            }
        }

        //present the data on the dataGridView
        private void bindDataSource(Hub[] hubs)
        {
            hubTable = new DataTable();
            DataColumn col0 = new DataColumn("id", typeof(int));
            DataColumn col1 = new DataColumn("QRCode", typeof(string));
            DataColumn col2 = new DataColumn("PCBACPU", typeof(string));
            DataColumn col3 = new DataColumn("PCBAETH0", typeof(string));
            DataColumn col4 = new DataColumn("PCBAWiFi", typeof(string));
            DataColumn col5 = new DataColumn("PCBABT", typeof(string));
            DataColumn col6 = new DataColumn("PCBAIMEI", typeof(string));
            DataColumn col7 = new DataColumn("PCBACCID", typeof(string));
            //DataColumn col8 = new DataColumn("TFCardCap", typeof(string));
            DataColumn col9 = new DataColumn("ADCDC", typeof(string));
            DataColumn col10 = new DataColumn("ADCBAT", typeof(string));
            DataColumn col11 = new DataColumn("ADCLTE", typeof(string));
            DataColumn col12 = new DataColumn("ETH0PING", typeof(string));
            DataColumn col13 = new DataColumn("LTEPWR", typeof(string));
            DataColumn col14 = new DataColumn("LTEWDIS", typeof(string));
            DataColumn col15 = new DataColumn("LTECOMM", typeof(string));
            DataColumn col16 = new DataColumn("ZWAVEPWR", typeof(string));
            DataColumn col17 = new DataColumn("ZWAVECOMM", typeof(string));
            DataColumn col18 = new DataColumn("ZWAVENVR", typeof(string));
            DataColumn col19 = new DataColumn("WiFiPING", typeof(string));
            DataColumn col20 = new DataColumn("BTSCAN", typeof(string));
            DataColumn col21 = new DataColumn("RESULTTIME", typeof(string));
            hubTable.Columns.Add(col0);
            hubTable.Columns.Add(col1);
            hubTable.Columns.Add(col2);
            hubTable.Columns.Add(col3);
            hubTable.Columns.Add(col4);
            hubTable.Columns.Add(col5);
            hubTable.Columns.Add(col6);
            hubTable.Columns.Add(col7);
            //hubTable.Columns.Add(col8);
            hubTable.Columns.Add(col9);
            hubTable.Columns.Add(col10);
            hubTable.Columns.Add(col11);
            hubTable.Columns.Add(col12);
            hubTable.Columns.Add(col13);
            hubTable.Columns.Add(col14);
            hubTable.Columns.Add(col15);
            hubTable.Columns.Add(col16);
            hubTable.Columns.Add(col17);
            hubTable.Columns.Add(col18);
            hubTable.Columns.Add(col19);
            hubTable.Columns.Add(col20);
            hubTable.Columns.Add(col21);
            foreach (var hub in hubs)
            {
                DataRow dr = hubTable.NewRow();
                dr["id"] = hub.id;
                dr["QRCode"] = hub.QRCode;
                dr["PCBACPU"] = hub.PCBACPU;
                dr["PCBAETH0"] = hub.PCBAETH0;
                dr["PCBAWiFi"] = hub.PCBAWiFi;
                dr["PCBABT"] = hub.PCBABT;
                dr["PCBAIMEI"] = hub.PCBAIMEI;
                dr["PCBACCID"] = hub.PCBACCID;
                //dr["TFCardCap"] = hub.TFCardCap;
                dr["ADCDC"] = hub.ADCDC;
                dr["ADCBAT"] = hub.ADCBAT;
                dr["ADCLTE"] = hub.ADCLTE;
                dr["ETH0PING"] = hub.ETH0PING;
                dr["LTEPWR"] = hub.LTEPWR;
                dr["LTEWDIS"] = hub.LTEWDIS;
                dr["LTECOMM"] = hub.LTECOMM;
                dr["ZWAVEPWR"] = hub.ZWAVEPWR;
                dr["ZWAVECOMM"] = hub.ZWAVECOMM;
                dr["ZWAVENVR"] = hub.ZWAVENVR;
                dr["WiFiPING"] = hub.WiFiPING;
                dr["BTSCAN"] = hub.BTSCAN;
                dr["RESULTTIME"] = hub.RESULTTIME;
                hubTable.Rows.Add(dr);
            }
            BindingSource source = new BindingSource();
            source.DataSource = hubTable;
            dataGridView1.DataSource = source;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DisableDefaultSelection();
        }

        private void DisableDefaultSelection()
        {
            dataGridView1.ClearSelection();
            dataGridView1.RowHeadersDefaultCellStyle.Padding = new Padding(dataGridView1.RowHeadersWidth);
            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
            }
        }

        public static void SendEmail(string server, int port, string sender, string recipient, string subject,
    string body, bool isBodyHtml, Encoding encoding, string authentication, string userName, params string[] files)
        {
            //set sender and recipient
            MailMessage message = new MailMessage(sender, recipient);
            message.IsBodyHtml = isBodyHtml;

            message.SubjectEncoding = encoding;
            message.BodyEncoding = encoding;

            message.Subject = subject;
            message.Body = body;

            message.Attachments.Clear();
            // add attachment
            if (files != null && files.Length != 0)
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    Attachment attach = new Attachment(files[i]);
                    message.Attachments.Add(attach);
                }
            }

            // create client
            SmtpClient smtpClient = new SmtpClient(server, port);
            smtpClient.Timeout = 50000;
            smtpClient.EnableSsl = true;

            if (string.IsNullOrEmpty(authentication))
            {
                smtpClient.UseDefaultCredentials = true;
            }
            else
            {
                // smtpClient.UseDefaultCredentials = true;// optional
                smtpClient.Credentials = new NetworkCredential(userName, authentication);
            }
            // smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;// send through network
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                foreach (Attachment attachment in message.Attachments)
                {
                    attachment.Dispose();
                }
                message.Attachments.Dispose();
                smtpClient.Dispose();
            }

        }

        // confirm before close all threads
        public void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("确定要退出吗？", "退出确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    e.Cancel = true;
                    return;
                }
                Form f = new UploadingForm();
                f.Show();         //Make sure we're the owner
                f.Refresh();
                this.Enabled = false; //Disable ourselves
                Cursor.Current = Cursors.WaitCursor;
                var arr = filePath.Split('\\');
                var newFile = string.Join("\\", arr.Take(arr.Length - 1)) + "\\v4hub.db";
                // copy the file to solve the conflicts of file operation
                File.Copy(filePath, newFile, true);
                SendEmail("smtp.126.com", 25, "mayue3434@126.com", "ryan@rently.com", "V4 hub Tester Results", "DB file:", false, Encoding.UTF8, "ROOQBZWWUAFDXDXG", "mayue3434", new String[] { newFile });
                File.Delete(newFile); this.Enabled = true;  //We're done, enable ourselves
                f.Close(); 
                Application.Exit();
            }
        }
    }
}

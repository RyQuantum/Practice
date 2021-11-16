using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialAssistant
{
    public partial class Form2 : Form
    {
        private DataTable hubTable = new DataTable();
        private Form form1;
        public Form2(HubDBContext db, Hub[] hubs)
        {
            InitializeComponent();

            bindDataSource(hubs);
            form1 = new Form1(db, this);
            form1.Show();
        }

        public void presentNewestTable()
        {
            using (var db = new HubDBContext())
            {
                var hubs = db.Hubs.Where(hub => true).ToArray();
                bindDataSource(hubs);
            }
        }

        private void bindDataSource(Hub[] hubs)
        {
            hubTable = new DataTable();
            DataColumn col1 = new DataColumn("id", typeof(int));
            DataColumn col2 = new DataColumn("PCBACPU", typeof(string));
            DataColumn col3 = new DataColumn("PCBAETH0", typeof(string));
            DataColumn col4 = new DataColumn("PCBAWiFi", typeof(string));
            DataColumn col5 = new DataColumn("PCBABT", typeof(string));
            DataColumn col6 = new DataColumn("PCBAIMEI", typeof(string));
            DataColumn col7 = new DataColumn("PCBACCID", typeof(string));
            DataColumn col8 = new DataColumn("TFCardCap", typeof(string));
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
            hubTable.Columns.Add(col1);
            hubTable.Columns.Add(col2);
            hubTable.Columns.Add(col3);
            hubTable.Columns.Add(col4);
            hubTable.Columns.Add(col5);
            hubTable.Columns.Add(col6);
            hubTable.Columns.Add(col7);
            hubTable.Columns.Add(col8);
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
                dr["PCBACPU"] = hub.PCBACPU;
                dr["PCBAETH0"] = hub.PCBAETH0;
                dr["PCBAWiFi"] = hub.PCBAWiFi;
                dr["PCBABT"] = hub.PCBABT;
                dr["PCBAIMEI"] = hub.PCBAIMEI;
                dr["PCBACCID"] = hub.PCBACCID;
                dr["TFCardCap"] = hub.TFCardCap;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}

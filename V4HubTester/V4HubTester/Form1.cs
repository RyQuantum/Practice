using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace V4HubTester
{
    public partial class Form1 : Form
    {
        private DataTable hubTable = new DataTable();

        public Form1(HubDBContext db, Hub[] hubs)
        {
            InitializeComponent();

            DataColumn col1 = new DataColumn("id", typeof(int));
            DataColumn col2 = new DataColumn("HubMac", typeof(string));
            hubTable.Columns.Add(col1);
            hubTable.Columns.Add(col2);

            foreach (var hub in hubs)
            {
                DataRow dr = hubTable.NewRow();
                dr["id"] = hub.id;
                dr["HubMac"] = hub.HubMac;
                hubTable.Rows.Add(dr);
            }
            bindDataSource();

            var form2 = new Form2(db, hubs);
            form2.Show();
        }

        private void bindDataSource()
        {
            BindingSource source = new BindingSource();
            source.DataSource = hubTable;
            dataGridView1.DataSource = source;
        }
    }
}

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodePrinter
{
    public partial class MainForm : Form
    {
        private DataTable snTable = new DataTable();
        private bool isAuthorized = false;
        private int handle = 0;     //  用于保存标签模板的句柄
        private int boxSize = 0;
        private string model = "";
        private int weight = 0;
        private string po = "";
        private int historyIndex = -1;
        private Process process;
        public MainForm(int num, string model, int weight, string po, Process process)
        {
            this.boxSize = num;
            this.model = model;
            this.weight = weight;
            this.po = po;
            this.process = process;

            for (int i = 1; i <= boxSize; i++)
            {
                snTable.Columns.Add(i.ToString());
            }
            snTable.Columns.Add("model");
            snTable.Columns.Add("qty");
            snTable.Columns.Add("weight");
            snTable.Columns.Add("date");
            snTable.Columns.Add("po");
            snTable.Columns.Add("case_no");
            snTable.Columns.Add("qr");
            snTable.Columns.Add("is_end");

            InitializeComponent();
            TLXLabelPaintCLS.RET ret;
            TLXLabelPaintCLS.LICENSETYPE license = 0;

            //  以下是关于授权的操作
            //  通过授权密码对接口进行授权，如果您有企业版授权码，请在此输入授权码
            //
            //  00000-00000-00000-00000-00000
            ret = TLXLabelPaintCLS.SetLicense("00000-00000-00000-00000-00000");

            //  获取授权状态
            ret = TLXLabelPaintCLS.GetLicense(ref license);

            switch (license)
            {
                case TLXLabelPaintCLS.LICENSETYPE.TLXLP_LICENSE_BASIC:
                    label1.Text = "授权: 标准版";
                    break;
                case TLXLabelPaintCLS.LICENSETYPE.TLXLP_LICENSE_PROFESSIONAL:
                    label1.Text = "授权: 专业版";
                    break;
                case TLXLabelPaintCLS.LICENSETYPE.TLXLP_LICENSE_ENTERPRISE:
                    label1.Text = "授权: 企业版";
                    break;
                case TLXLabelPaintCLS.LICENSETYPE.TLXLP_LICENSE_NONE:
                    label1.Text = "授权: 未授权";
                    break;
            }

            //  系统中的所有打印机
            //foreach (string sPrint in PrinterSettings.InstalledPrinters)
            //    cbPrinters.Items.Add(sPrint);

            //  获取 LabelShop 支持的打印机列表
            //  LabelShop 从 5.30 版本起，支持内置打印机驱动模式，使用 GetSysPrinterNames 接口，可以获取系统中已经安装的此类打印机
            //  需要注意的是，此类打印机在WINDOWS中是不可见的
            //  另外，此类打印机一般是需要在 LabelShop 中手工安装的
            //  但是如果打开的一个标签模板中使用了一台未安装的此类打印机，则系统会自动安装此打印机
            //  因此在打开了一个模板后，可能需要刷新打印机列表

            string strPrinters = "";
            ret = TLXLabelPaintCLS.GetSysPrinterNames(ref strPrinters);

            //  打印机名称是用 \n 分隔开的
            string[] sPrinterArray = strPrinters.Split(new char[] { '\n' });
            foreach (string sPrint in sPrinterArray)
                comboBox1.Items.Add(sPrint);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Size = new Size(900, 300);
            addHistoryRows();
            addNewRow();
            this.dataGridView1.Columns.Add(this.Print);
            for (int i = 0; i < boxSize; i++)
            {
                dataGridView1.Columns[i].Width = 100;
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.AutoGenerateColumns = false;

            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Rently\\Label Printer (panel)\\template.lsdx";
            TLXLabelPaintCLS.RET ret; 
            ret = TLXLabelPaintCLS.OpenDocument(filePath, ref handle);
            if (ret != TLXLabelPaintCLS.RET.TLXLP_OK)
            {
                MessageBox.Show(ErrorMessage(ret), "出错", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //  模板文件是否被授权。我们将使用付费版本的 LabelShop 编辑保存的标签模板提前为被授权的标签模板文件
            //  在使用企业版授权模式打印时，需要使用授权的标签模板
            TLXLabelPaintCLS.DOCLEVEL level = TLXLabelPaintCLS.DOCLEVEL.TLXLP_DOCUMENT_BASIC;
            ret = TLXLabelPaintCLS.GetDocumentLevel(handle, ref level);
            label2.Text = "模板: ";
            if (ret != TLXLabelPaintCLS.RET.TLXLP_OK)
            {
                MessageBox.Show(ErrorMessage(ret), "出错", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                switch (level)
                {
                    case TLXLabelPaintCLS.DOCLEVEL.TLXLP_DOCUMENT_BASIC:
                        label2.Text = "模板: 未授权";
                        break;
                    case TLXLabelPaintCLS.DOCLEVEL.TLXLP_DOCUMENT_PROFESSIONAL:
                        label2.Text = "模板: 已授权";
                        break;
                }
            }
            string strNames = "";
            ret = TLXLabelPaintCLS.GetNamedVarNames(handle, ref strNames);

            //  命令变量是用 \n 分隔开的
            string[] sNameArray = strNames.Split(new char[] { '\n' });
            //TODO
        }

        private void addHistoryRows()
        {
            using (var db = new MyDatabase())
            {
                var list = db.Records.Where(r => true).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    var serialNumbers = list[i].serial_numbers.Split(',');
                    DataRow dr = snTable.NewRow();
                    for (int j = 0; j < boxSize; j++)
                    {
                        if (j < serialNumbers.Length) dr[j] = serialNumbers[j];
                        else dr[j] = "";
                    }
                    dr["model"] = list[i].model;
                    dr["qty"] = list[i].qty;
                    dr["weight"] = list[i].weight;
                    dr["date"] = list[i].date;
                    dr["po"] = list[i].po;
                    dr["case_no"] = list[i].case_no;
                    dr["qr"] = list[i].qr;
                    dr["is_end"] = list[i].is_end;
                    snTable.Rows.Add(dr);
                    historyIndex++;
                }
            }
            dataGridView1.DataSource = snTable;
            dataGridView1.Columns["model"].Visible = false;
            dataGridView1.Columns["qty"].Visible = false;
            dataGridView1.Columns["weight"].Visible = false;
            dataGridView1.Columns["date"].Visible = false;
            dataGridView1.Columns["po"].Visible = false;
            dataGridView1.Columns["case_no"].Visible = false;
            dataGridView1.Columns["qr"].Visible = false;
            dataGridView1.Columns["is_end"].Visible = false;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = string.Format("{0}", i + 1);
                dataGridView1.Rows[i].ReadOnly = true;
                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
            }
        }

        private void addNewRow()
        {
            DataRow dr = snTable.NewRow();
            snTable.Rows.Add(dr);
            dataGridView1.DataSource = snTable;
            dataGridView1.CurrentCell = null;
            var row = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
            row.Cells["1"].Selected = true;
            row.HeaderCell.Value = string.Format("{0}", row.Index + 1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("row: " + e.RowIndex + " column: " + e.ColumnIndex);
            if (e.ColumnIndex < 0 || dataGridView1.Columns[e.ColumnIndex].Name != "Print") return;
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请选择打印机", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBox1.Focus();
                return;
            }
            var emptyOnce = false;
            for (var i = 0; i < boxSize; i++)
            {
                if (emptyOnce)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString() != "")
                    {
                        MessageBox.Show($"请勿留空。", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString() == "")
                {
                    emptyOnce = true;
                }
            }
            if (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() == "")
            {
                MessageBox.Show($"请输入SN号", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!isAuthorized && emptyOnce && e.RowIndex > historyIndex)
            {
                //InputDialog inputDialog = new InputDialog();
                //inputDialog.ShowDialog();
                //if (!inputDialog.isValid) return;
                var dialogResult = MessageBox.Show($"确定准备打印尾箱吗？", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Cancel) return;
                isAuthorized = true;
            }

            using (var db = new MyDatabase())
            {
                var row = snTable.Rows[e.RowIndex];
                if (e.RowIndex < dataGridView1.RowCount - 1)
                {
                    showPreivew(row, e.RowIndex, emptyOnce);
                    return;
                }
                var list = new List<int>();
                var set = new HashSet<Tuple<int, int>>();
                for (var i = 0; i < boxSize; i++)
                {
                    string sn = row[i].ToString();
                    var lockObj = db.Locks.FirstOrDefault(l => l.serial_number == sn);
                    if (lockObj == null && (isAuthorized ? sn != "" : true))
                    {
                        list.Add(i);
                    }
                    for (int x = 0; x <= e.RowIndex; x++)
                    {
                        for (int y = 0; y < boxSize; y++)
                        {
                            dataGridView1.Rows[x].Cells[y].Style.BackColor = x > historyIndex ? Color.White : Color.LightGray;
                            if (dataGridView1.Rows[x].Cells[y].Value.ToString() == sn && !(x == e.RowIndex && y == i) && (isAuthorized ? sn != "" : true))
                            {
                                set.Add(new Tuple<int, int>(x, y));
                                set.Add(new Tuple<int, int>(e.RowIndex, i));
                            }
                        }
                    }
                }
                foreach (var i in EnumerableUtilities.RangePython(0, boxSize))
                {
                    dataGridView1.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.White;
                }

                var duplicated = "";
                if (set.Count != 0)
                {
                    var tmp = new List<string>();
                    foreach (Tuple<int, int> t in set)
                    {
                        dataGridView1.Rows[t.Item1].Cells[t.Item2].Style.BackColor = Color.Yellow;
                    }
                    for (int i = 0; i < dataGridView1.ColumnCount - 1; i++)
                    {
                        if (set.Contains(new Tuple<int, int>(dataGridView1.RowCount - 1, i)))
                        {
                            tmp.Add(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[i].Value.ToString());
                        }
                    }
                    string s = String.Join(", ", tmp);
                    duplicated = "SN号: [" + s + "] 重复！";
                }

                var nonexistent = "";
                if (list.Count != 0)
                {
                    list.ForEach(i =>
                    {
                        if (set.Contains(new Tuple<int, int>(dataGridView1.Rows.Count - 1, i)))
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.Orange;
                        }
                        else
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.Red;
                        }
                    });
                    List<string> tmp = new List<string>();
                    list.ForEach(i => tmp.Add(row[i].ToString()));
                    string s = String.Join(", ", tmp);
                    nonexistent = "SN号: [" + s + "] 不存在！";
                }

                if (duplicated != "" || nonexistent != "")
                {
                    var output = String.Join("\n", new List<string>(new string[] { duplicated, nonexistent }));
                    MessageBox.Show(output, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var (res, strArr) = showPreivew(row, e.RowIndex, emptyOnce);
                if (res == DialogResult.OK)
                {
                    var serialNumbers = "";
                    foreach (var i in EnumerableUtilities.RangePython(0, boxSize))
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[i].ReadOnly = true;
                        var serialNumber = dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString();
                        if (serialNumber != "" && i == 0) serialNumbers = serialNumber;
                        if (serialNumber != "" && i != 0) serialNumbers = serialNumbers + "," + serialNumber;
                    }
                    addNewRow();
                    //var record = db.Records.FirstOrDefault(r => r.rowIndex == e.RowIndex);
                    //if (record != null) return;
                    snTable.Rows[e.RowIndex]["model"] = strArr[0];
                    snTable.Rows[e.RowIndex]["qty"] = strArr[1];
                    snTable.Rows[e.RowIndex]["weight"] = strArr[2];
                    snTable.Rows[e.RowIndex]["date"] = strArr[3];
                    snTable.Rows[e.RowIndex]["po"] = strArr[4];
                    snTable.Rows[e.RowIndex]["case_no"] = strArr[5];
                    snTable.Rows[e.RowIndex]["qr"] = strArr[6];
                    snTable.Rows[e.RowIndex]["is_end"] = strArr[7];
                    db.Records.Add(new Record()
                    {
                        rowIndex = e.RowIndex,
                        serial_numbers = serialNumbers,
                        model = strArr[0],
                        qty = strArr[1],
                        weight = strArr[2],
                        date = strArr[3],
                        po = strArr[4],
                        case_no = strArr[5],
                        qr = strArr[6],
                        is_end = strArr[7],
                    });
                    db.SaveChangesAsync();
                };
            }
        }

        private (DialogResult, string[]) showPreivew(DataRow row, int index, bool emptyOnce)
        {
            TLXLabelPaintCLS.RET ret;
            string modelStr, qtyStr, weightStr, dateStr, poStr, caseNoStr, qrStr, isEndStr;
            if (index > historyIndex)
            {
                modelStr = model;
                var caseIndex = "000001";
                for (int i = index - 1; i >= 0; i--)
                {
                    var previousRow = snTable.Rows[i];
                    var previousPo = previousRow["po"].ToString();
                    if (previousPo == "PO" + po)
                    {
                        var preCaseIndex = previousRow["case_no"].ToString().Split('F')[1];
                        caseIndex = (Int32.Parse(preCaseIndex) + 1).ToString("D6");
                        break;
                    }
                }
                dateStr = DateTime.Now.ToString("d");
                var dateArr = DateTime.Now.ToString("s").Split('T')[0].Split('-');
                caseNoStr = String.Join("", dateArr) + "F" + caseIndex;
                qrStr = "";
                foreach (var i in EnumerableUtilities.RangePython(0, boxSize))
                {
                    ret = TLXLabelPaintCLS.SetNamedVariable(handle, (i + 1) + "", "");
                }
                int qty = 0;
                foreach (var i in EnumerableUtilities.RangePython(0, boxSize))
                {
                    if (row[i].ToString() == "") break;
                    qty++;
                    ret = TLXLabelPaintCLS.SetNamedVariable(handle, (i + 1) + "", row[i].ToString());
                    qrStr = qrStr + (i == 0 ? "" : ",") + row[i].ToString();
                    Console.WriteLine((i + 1) + " " + row[i].ToString() + " ret: " + ret);
                }
                int tmp = qty == boxSize ? weight : (int)Math.Ceiling((double)weight / boxSize * qty);
                weightStr = tmp.ToString() + "KG";
                poStr = "PO" + po;
                qtyStr = qty.ToString();
                isEndStr = emptyOnce ? "It's the ENDING BOX." : "";
            }
            else
            {
                using (var db = new MyDatabase())
                {
                    var record = db.Records.FirstOrDefault(r => r.rowIndex == index);
                    modelStr = record.model;
                    qtyStr = record.qty;
                    weightStr = record.weight;
                    dateStr = record.date;
                    poStr = record.po;
                    caseNoStr = record.case_no;
                    qrStr = record.qr;
                    isEndStr = record.is_end;
                    var serialNumbers = record.serial_numbers.Split(',');
                    foreach (var i in EnumerableUtilities.RangePython(0, serialNumbers.Length))
                    {
                        ret = TLXLabelPaintCLS.SetNamedVariable(handle, (i + 1) + "", serialNumbers[i]);
                    }
                }
            }

            ret = TLXLabelPaintCLS.SetNamedVariable(handle, "model", modelStr);
            ret = TLXLabelPaintCLS.SetNamedVariable(handle, "qty", qtyStr);
            ret = TLXLabelPaintCLS.SetNamedVariable(handle, "weight", weightStr);
            ret = TLXLabelPaintCLS.SetNamedVariable(handle, "date", dateStr);
            ret = TLXLabelPaintCLS.SetNamedVariable(handle, "po", poStr);
            ret = TLXLabelPaintCLS.SetNamedVariable(handle, "caseNo", caseNoStr);
            ret = TLXLabelPaintCLS.SetNamedVariable(handle, "qr", qrStr);
            ret = TLXLabelPaintCLS.SetNamedVariable(handle, "isEnd", isEndStr);

            PreviewForm preview = new PreviewForm(handle);
            var res = preview.ShowDialog();
            return (res, new string[] { modelStr, qtyStr, weightStr, dateStr, poStr, caseNoStr, qrStr, isEndStr });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TLXLabelPaintCLS.SetPrinterName(handle, comboBox1.Text);
            TLXLabelPaintCLS.PrinterProperties(handle, IntPtr.Zero);
        }

        private string ErrorMessage(TLXLabelPaintCLS.RET id)
        {
            string strMessage = "未知错误";
            switch (id)
            {
                case TLXLabelPaintCLS.RET.TLXLP_OK:
                    strMessage = "成功返回";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_ERROR:
                    strMessage = "出错";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_INVALIDDOCUMENT:
                    strMessage = "无效的文档";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_OPENDOCUMENT:
                    strMessage = "打开文档失败";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_SAVEDOCUMENT:
                    strMessage = "保存文档失败";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_NOTFOUNDRECORD:
                    strMessage = "未查找到数据库记录";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_PRINTLABELS:
                    strMessage = "打印标签失败";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_INVALIDVARIABLE:
                    strMessage = "无效的变量";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_BUFFERTOOSMALL:
                    strMessage = "缓冲区内存空间太小";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_INVALIDLICENSEKEY:
                    strMessage = "无效的授权码";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_UNAUTHORIZED:
                    strMessage = "未授权";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_NOTFOUNDPRINTER:
                    strMessage = "未找到目标打印机";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_NOTFOUNDLABELSHOP:
                    strMessage = "没有安装 LabelShop";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_VERSIONTOOLOW:
                    strMessage = "LabelShop 版本太低，需要 LabelShop 5.21 或者更高的版本";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_NOTCHANGE:
                    strMessage = "未发生改变";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_INVALIDROWNUMBER:
                    strMessage = "无效的记录行号";
                    break;
                case TLXLabelPaintCLS.RET.TLXLP_NONSUPPORT:
                    strMessage = "不支持的功能";
                    break;
            }

            return strMessage;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var dialogResult = MessageBox.Show($"确定退出吗？", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            e.Cancel = (dialogResult == DialogResult.Cancel);
            if (e.Cancel == false)
            {
                process.Close();
                System.Environment.Exit(0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new MyDatabase())
            {
                var config = db.Configs.FirstOrDefault(f => true);
                ConfigurationForm configForm = new ConfigurationForm(process, config);
                configForm.ShowDialog();
            }
        }
    }
}

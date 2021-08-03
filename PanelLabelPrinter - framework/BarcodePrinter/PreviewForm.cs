using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodePrinter
{
    public partial class PreviewForm : Form
    {
        int handle = 0;
        public TLXLabelPaintCLS.RET ret;
        //public TLXLabelPaintCLS.RET res
        //{
        //    get
        //    {
        //        return ret;
        //    }
        //}
        public PreviewForm(int handle)
        {
            this.handle = handle;
            InitializeComponent();
        }

        private void Preview_Load(object sender, EventArgs e)
        {
            Bitmap bmp = null;
            ret = TLXLabelPaintCLS.GetThumbnailEx(handle, ref bmp, this.pictureBox1.Size.Width - 2, this.pictureBox1.Size.Height - 2,
                TLXLabelPaintCLS.TLXLP_THUMB_WORKSPACECOLOR, 0);
            pictureBox1.Image = bmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
                    err 取值
                    0   授权不符时，输出标签，但是标签上会包含额外的标记
                    1   授权不符时，不输出标签
                    2   授权不符时，给出提示，用户选择是否输出标签，如果希望尽量避免输出带额外标记的标签，可以用这个方法提供用户选择的机会
            */
            ret = TLXLabelPaintCLS.OutputDocument(handle, 1, 1, 0);
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

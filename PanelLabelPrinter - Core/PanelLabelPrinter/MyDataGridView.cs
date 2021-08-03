using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PanelLabelPrinter
{
    class MyDataGridView : DataGridView
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case System.Windows.Forms.Keys.Enter:
                    System.Windows.Forms.SendKeys.Send("{TAB}");
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}

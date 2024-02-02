using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KsIndexerNET
{
    public partial class DlgInput2 : Form
    {
        public DlgInput2(string label1, string label2, string texto1 = "", string texto2 = "")
        {
            InitializeComponent();
            lbl1.Text = label1;
            lbl2.Text = label2;
            if (texto1 != "")
            {
                txt1.Text = texto1;
            }
            if (texto2 != "")
            {
                txt2.Text = texto2;
            }
        }

        public string GetText1()
        {
            return txt1.Text;
        }

        public string GetText2()
        {
            return txt2.Text;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            this.Dispose();
        }

        private void DlgInput2_Load(object sender, EventArgs e)
        {
            LangUtils.TranslateForm(this);
        }
    }
}

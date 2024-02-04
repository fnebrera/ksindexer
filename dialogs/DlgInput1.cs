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
    public partial class DlgInput1 : Form
    {
        public DlgInput1(string label, string texto = "")
        {
            InitializeComponent();
            lbl1.Text = label;
            if (texto != "")
            {
                txt1.Text = texto;
            }
        }

        public string GetText1()
        {
            return txt1.Text;
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

        private void DlgInput1_Load(object sender, EventArgs e)
        {
            // Traducir el formulario
            LangUtils.TranslateForm(this);
        }
    }
}

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
    public partial class DlgImport : Form
    {
        public DlgImport()
        {
            InitializeComponent();
        }

        public string GetText()
        {
            return DocText.Text;
        }

        public void SetText(string text)
        {
            DocText.Text = text;
        }

        private void DlgImportar_Load(object sender, EventArgs e)
        {
            // Quitar posible seleccion
            DocText.Select(0, 0);
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
    }
}

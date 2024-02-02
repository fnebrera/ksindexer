using ksindexer;
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
    public partial class DlgSelectDoc : Form
    {
        public DlgSelectDoc(List<string[]> lista)
        {
            InitializeComponent();
            DocList.View = View.Details;
            if (lista == null)
                return;
            foreach (string[] item in lista)
            {
                ListViewItem lItem = new ListViewItem(item);
                DocList.Items.Add(lItem);
            }
        }

        public int GetSelectedId()
        {
            if (DocList.SelectedItems.Count == 0)
                return 0;
            return int.Parse(DocList.SelectedItems[0].SubItems[0].Text);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (DocList.SelectedItems.Count == 0)
            {
                Messages.ShowError(LangUtils.TranslateTxt(this, "MUST_SELECT_DOC"));
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            this.Dispose();
        }

        private void DocList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSelect.Enabled = DocList.SelectedItems.Count > 0;
        }

        private void DlgSelectDoc_Load(object sender, EventArgs e)
        {
            LangUtils.TranslateForm(this);
        }
    }
}

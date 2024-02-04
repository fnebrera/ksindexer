using KsIndexerNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KsIndexerNET
{
    public partial class DlgSearchComplex : Form
    {
        public DlgSearchComplex()
        {
            InitializeComponent();
            comboAndOr.SelectedIndex = 0;
        }

        public string GetTitulo()
        {
            return txtTitle.Text;
        }

        public string GetFechaDesde()
        {
            return txtDateFrom.Text;
        }

        public string GetFechaHasta()
        {
            return txtDateTo.Text;
        }

        public string GetClaves()
        {
            return txtKeys.Text;
        }

        public bool GetClavesTodas()
        {
            return comboAndOr.SelectedIndex == 1;
        }

        public string GetAsistente()
        {
            return txtAssistant.Text;
        }

        public string GetEmpresa()
        {
            return txtCompany.Text;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim().Length == 0 &&
                txtDateFrom.Text.Trim().Length == 0 &&
                txtDateTo.Text.Trim().Length == 0 &&
                txtKeys.Text.Trim().Length == 0 &&
                txtAssistant.Text.Trim().Length == 0 &&
                txtCompany.Text.Trim().Length == 0)
            {
                //Messages.ShowWarning("Debe introducir al menos un criterio de búsqueda");
                Messages.ShowWarning(Texts.NO_SEARCH_CRITERIA);
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

        private void txtDateFrom_Leave(object sender, EventArgs e)
        {
            DateTime dt = LangUtils.ParseDateTime(txtDateFrom.Text);
            if (txtDateFrom.Text.Length > 0 && dt == DateTime.MinValue)
            {
                Messages.ShowError(Texts.WRONG_DATE_FORMAT);
                txtDateFrom.SelectAll();
                txtDateFrom.Focus();
                return;
            }
        }

        private void txtDateTo_Leave(object sender, EventArgs e)
        {
            DateTime dt = LangUtils.ParseDateTime(txtDateTo.Text);
            if (txtDateTo.Text.Length > 0 && dt == DateTime.MinValue)
            {
                Messages.ShowError(Texts.WRONG_DATE_FORMAT);
                txtDateTo.SelectAll();
                txtDateTo.Focus();
                return;
            }
        }

        private void DlgSearchComplex_Load(object sender, EventArgs e)
        {
            LangUtils.TranslateForm(this);
        }
    }
}

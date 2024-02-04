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
    public partial class DlgSettings : Form
    {
        private string[] cultures = { "es-ES", "en-US" };
        private string[] culturesNames = { "Español (España)", "English (USA)" };
        public DlgSettings()
        {
            InitializeComponent();
            cbLanguage.Items.AddRange(culturesNames);
        }

        public string GetCulture()
        {
            return cultures[cbLanguage.SelectedIndex];
        }

        public void SetCulture(string culture)
        {
            for (int i = 0; i < cultures.Length; i++)
            {
                if (cultures[i] == culture)
                {
                    cbLanguage.SelectedIndex = i;
                    break;
                }
            }
        }

        public string GetDateFormat()
        {
            return cbDateFormat.Text;
        }

        public void SetDateFormat(string format)
        {
            cbDateFormat.Text = format;
        }

        public int GetMaxSize()
        {
            int.TryParse(txtMaxSize.Text, out int maxSize);
            return maxSize;
        }

        public void SetMaxSize(int maxSize)
        {
            txtMaxSize.Text = maxSize.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void DlgSettings_Load(object sender, EventArgs e)
        {
            LangUtils.TranslateForm(this);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}

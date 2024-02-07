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

        public string GetStartDate()
        {
            return txtStartDate.Text;
        }

        public void SetStartDate(string val)
        {
            txtStartDate.Text = val;
        }

        public string GetStartAttendant()
        {
            return txtStartAttendant.Text;
        }

        public void SetStartAttendant(string val)
        {
            txtStartAttendant.Text = val;
        }

        public string GetStartTag()
        {
            return txtStartTag.Text;
        }

        public void SetStartTag(string val)
        {
            txtStartTag.Text = val;
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

    }
}

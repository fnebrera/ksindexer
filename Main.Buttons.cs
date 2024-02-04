using KsIndexerNET.Db;
using KsIndexerNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace KsIndexerNET
{
    partial class Main
    {
        private void BtnKeyAdd()
        {
            DlgInput1 dlg = new DlgInput1(Texts.ENTER_KEYWORDS);
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string clave = dlg.GetText1();
            dlg.Dispose();
            if (clave.Trim().Length == 0)
                return;
            string[] claves = clave.Split(' ');
            foreach (string clv in claves)
            {
                if (clv.Trim().Length == 0)
                    continue;
                string clvnorm = FileUtils.NormalizeString(clv);
                if (Keywords.Items.Contains(clvnorm))
                    continue;
                Keywords.Items.Add(clvnorm);
                CurrentDoc.Keywords.Add(new Keyword(CurrentDoc.Id, clvnorm));
                SetTextChanged();
            }
        }

        private void BtnKeyEdit()
        {
            int pos = Keywords.SelectedIndex;
            if (pos < 0)
                return;
            DlgInput1 dlg = new DlgInput1(Texts.ENTER_NEW_VALUE, Keywords.Items[pos].ToString());
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string clave = FileUtils.NormalizeString(dlg.GetText1());
            dlg.Dispose();
            if (clave.Trim().Length == 0)
                return;
            Keywords.Items[pos] = clave;
            CurrentDoc.Keywords[pos] = new Keyword(CurrentDoc.Id, clave);
            SetTextChanged();
        }

        private void BtnKeyDelete()
        {
            if (Keywords.SelectedIndex < 0)
                return;
            string keyword = Keywords.SelectedItem.ToString();
            Keywords.Items.RemoveAt(Keywords.SelectedIndex);
            CurrentDoc.DeleteKeyword(keyword);
            SetTextChanged();
        }

        private void BtnAttAdd()
        {
            DlgInput2 dlg = new DlgInput2(Texts.ENTER_ATT_NAME, Texts.ENTER_ATT_COMPANY);
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string nombre = dlg.GetText1();
            string empresa = dlg.GetText2();
            dlg.Dispose();
            if (nombre.Trim().Length == 0)
                return;
            nombre = FileUtils.NormalizeString(nombre);
            empresa = FileUtils.NormalizeString(empresa);
            foreach (ListViewItem item in Attendants.Items)
            {
                if (item.SubItems[0].Text == nombre)
                    return;
            }
            Attendants.Items.Add(new ListViewItem(new string[] { nombre, empresa }));
            CurrentDoc.Attendants.Add(new Attendant(CurrentDoc.Id, nombre, empresa));
            SetTextChanged();
        }

        private void BtnAttEdit()
        {
            if (Attendants.SelectedItems.Count == 0)
                return;
            int pos = Attendants.SelectedIndices[0];
            string nombre = Attendants.SelectedItems[0].SubItems[0].Text;
            string empresa = Attendants.SelectedItems[0].SubItems[1].Text;
            DlgInput2 dlg = new DlgInput2(Texts.ENTER_ATT_NAME, Texts.ENTER_ATT_COMPANY, nombre, empresa);
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            nombre = dlg.GetText1();
            empresa = dlg.GetText2();
            dlg.Dispose();
            if (nombre.Trim().Length == 0)
                return;
            nombre = FileUtils.NormalizeString(nombre);
            empresa = FileUtils.NormalizeString(empresa);
            Attendants.Items[pos] = new ListViewItem(new string[] { nombre, empresa });
            CurrentDoc.Attendants[pos] = new Attendant(CurrentDoc.Id, nombre, empresa);
            SetTextChanged();
        }

        private void BtnAttDelete()
        {
            if (Attendants.SelectedItems.Count == 0)
                return;
            string nombre = Attendants.SelectedItems[0].SubItems[0].Text;
            Attendants.Items.RemoveAt(Attendants.SelectedIndices[0]);
            CurrentDoc.DeleteAttendant(nombre);
            SetTextChanged();
        }

        private void BtnAnxAdd()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = Texts.FILTER_ALL_FILES;
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            // Nombre del archivo
            string filepath = dlg.FileName;
            AddAnnexToDoc(filepath);
        }

        private void BtnAnxDelete()
        {
            if (Annexes.SelectedItems.Count == 0)
                return;
            string name = Annexes.SelectedItems[0].SubItems[0].Text;
            Annexes.Items.RemoveAt(Annexes.SelectedIndices[0]);
            CurrentDoc.DeleteAnnex(name);
            SetTextChanged();
        }

        private void BtnAnxView()
        {
            if (Annexes.SelectedItems.Count == 0)
                return;
            string name = Annexes.SelectedItems[0].SubItems[0].Text;
            byte[] contenido = CurrentDoc.GetAnnexContent(name);
            if (contenido == null)
            {
                Messages.ShowError(Texts.COULD_NOT_LOAD_FILE + " " + name);
                return;
            }
            // Salvar a un temporal y mostrar
            string filename = Path.GetTempPath() + "kstmpfile" + Path.GetExtension(name);
            File.WriteAllBytes(filename, contenido);
            Process.Start(filename);
        }
    }
}

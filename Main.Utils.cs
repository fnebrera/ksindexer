using ksindexer.Db;
using ksindexer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KsIndexerNET
{
    partial class Main
    {
        // Habilitar o deshabilitar los controles segun el estado del documento
        private void EnableControls()
        {
            if (DocEmpty)
            {
                MenuSave.Enabled = toolBtnSave.Enabled = false;
                MenuRegenMetadata.Enabled = toolBtnRegenMetadata.Enabled = false;
                MenuExport.Enabled = toolBtnExportHtml.Enabled = false;
                MenuPrint.Enabled = toolBtnPrint.Enabled = false;
                MenuDelete.Enabled = toolBtnDelete.Enabled = false;
                MenuUpdatePdf.Enabled = toolBtnUpdatePdf.Enabled = false;
                btnAttAdd.Enabled = btnKeyAdd.Enabled = btnAnxAdd.Enabled = false;
            }
            else
            {
                MenuSave.Enabled = toolBtnSave.Enabled = DocChanged;
                MenuRegenMetadata.Enabled = toolBtnRegenMetadata.Enabled = CurrentDoc.ImportedText.Length > 0;
                MenuExport.Enabled = toolBtnExportHtml.Enabled = true;
                MenuPrint.Enabled = toolBtnPrint.Enabled = true;
                MenuDelete.Enabled = toolBtnDelete.Enabled = CurrentDoc.Id > 0;
                MenuUpdatePdf.Enabled = toolBtnUpdatePdf.Enabled = true;
                btnAttAdd.Enabled = btnKeyAdd.Enabled = btnAnxAdd.Enabled = true;
            }
            statusId.BackColor = statusLabelId.BackColor = DocChanged ? Color.LightCoral : Color.White;
        }

        // Establecer si el documento ha cambiado y habilitar/deshabilitar los controles
        private void SetTextChanged(bool changed = true)
        {
            DocChanged = changed;
            if (changed)
                DocEmpty = false;
            EnableControls();
        }

        // Limpiar todos los controles y el documento asociado
        private void ClearAll()
        {
            CurrentDoc.Clear();
            TextInDb.Text = "";
            Title.Text = "";
            DocDate.Text = "";
            Keywords.Items.Clear();
            Attendants.Items.Clear();
            Annexes.Items.Clear();
            pdfView.Navigate(new Uri("about:blank"));
            this.statusId.Text = "vacío";
            DocChanged = false;
            DocEmpty = true;
            EnableControls();
        }

        // Regenerar los metadatos
        private void RegenMetadata()
        {
            if (CurrentDoc.ImportedText.Length == 0)
            {
                Messages.ShowError(Texts.NO_TEXT_TO_PROCESS);
                return;
            }
            // Obtener el titulo del documento
            Title.Text = FileUtils.GetDocTitle(CurrentDoc.ImportedText);
            CurrentDoc.Title = Title.Text;
            // Obtener la fecha del documento
            DocDate.Text = FileUtils.GetDocDate(CurrentDoc.ImportedText);
            if (DocDate.Text.Length > 0)
            {
                CurrentDoc.DocDate = DateTime.Parse(DocDate.Text);
            }
            // Obtener las palabras clave del documento 
            string[] keywords = FileUtils.GetDocKeywords(CurrentDoc.ImportedText);
            // Mostrar las palabras clave en el ListBox
            Keywords.Items.Clear();
            CurrentDoc.Keywords.Clear();
            foreach (string keyword in keywords)
            {
                Keywords.Items.Add(keyword);
                CurrentDoc.Keywords.Add(new Keyword(0, keyword));
            }
            // Obtener los asistentes del documento
            string[][] attendants = FileUtils.GetDocAttendants(CurrentDoc.ImportedText);
            // Mostrar los asistentes en el ListView
            Attendants.Items.Clear();
            CurrentDoc.Attendants.Clear();
            Attendants.View = View.Details;
            foreach (string[] attendant in attendants)
            {
                Attendants.Items.Add(new ListViewItem(attendant));
                CurrentDoc.Attendants.Add(new Attendant(0, attendant[0], attendant[1]));
            }
            // Mostrar el texto limpio en el control
            TextInDb.Text = FileUtils.GetTextCleared(CurrentDoc.ImportedText);
            CurrentDoc.DocText = TextInDb.Text;
            // Indicar que el documento ha cambiado
            DocChanged = true;
            DocEmpty = false;
            EnableControls();
        }

        private void FillControlsFromDoc()
        {
            // Mostrar el contenido en el TextBox
            TextInDb.Text = CurrentDoc.DocText;
            // Mostrar el titulo
            Title.Text = CurrentDoc.Title;
            // Mostrar la fecha
            DocDate.Text = LangUtils.FormatDateTime(CurrentDoc.DocDate);
            // Numero de documento en satusBar
            statusId.Text = CurrentDoc.Id.ToString();
            // Mostrar las palabras clave en el ListBox
            Keywords.Items.Clear();
            foreach (Keyword palabra in CurrentDoc.Keywords)
            {
                Keywords.Items.Add(palabra.Key);
            }
            // Mostrar los asistentes en el ListView
            Attendants.Items.Clear();
            Attendants.View = View.Details;
            foreach (Attendant asistente in CurrentDoc.Attendants)
            {
                Attendants.Items.Add(new ListViewItem(new string[] { asistente.Name, asistente.Company ?? "" }));
            }
            // Mostrar el PDF en el WebBrowser
            if (CurrentDoc.Pdf.Length > 0)
            {
                string filename = Path.GetTempFileName() + "kstmpfile.pdf";
                File.WriteAllBytes(filename, CurrentDoc.Pdf);
                pdfView.Navigate(filename);
            }
            else
            {
                pdfView.Navigate(new Uri("about:blank"));
            }
            // Mostrar los anexos en el ListView
            Annexes.Items.Clear();
            Annexes.View = View.Details;
            foreach (Annex anexo in CurrentDoc.Annexes)
            {
                Annexes.Items.Add(new ListViewItem(new string[] { anexo.FileName, anexo.Size.ToString("N0") }));
            }
        }

        private void FillDocFromControls()
        {
            // Obtener el texto del TextBox
            CurrentDoc.DocText = TextInDb.Text;
            // Obtener el titulo
            CurrentDoc.Title = Title.Text;
            // Obtener la fecha
            CurrentDoc.DocDate = DateTime.Parse(DocDate.Text);
            // Obtener las palabras clave del ListBox
            CurrentDoc.Keywords.Clear();
            foreach (string keyword in Keywords.Items)
            {
                CurrentDoc.Keywords.Add(new Keyword(CurrentDoc.Id, keyword));
            }
            // Obtener los asistentes del ListView
            CurrentDoc.Attendants.Clear();
            foreach (ListViewItem item in Attendants.Items)
            {
                CurrentDoc.Attendants.Add(new Attendant(CurrentDoc.Id, item.SubItems[0].Text, item.SubItems[1].Text));
            }
            /*
             * IMPORTANTE: La lista de anexos tiene un tratamiento especial, porque no cargamos todos los anexos
             * al abrir un documento, por lo que el ListView es sólo una vista parcial de lo que tenemos en el documento
             * en cada momento, y su actualización se va haciendo en los eventos de los botones de anexos.
             */
        } 
        
        private void AddAnnexToDoc(string filename)
        {
            // Verificar que no sea un directorio
            FileInfo fi = new FileInfo(filename);
            if (fi.Attributes.HasFlag(FileAttributes.Directory))
            {
                Messages.ShowError(Texts.FOLDER_NOT_ALLOWED);
                return;
            }
            // Verificar que no existe ya
            string name = Path.GetFileName(filename);
            foreach (Annex anexo in CurrentDoc.Annexes)
            {
                if (anexo.FileName == name)
                {
                    Messages.ShowError(Texts.ANNEX_ALREADY_EXISTS);
                    return;
                }
            }
            // Verificar que no exceda el tamaño maximo
            if (fi.Length > maxAnnexSize)
            {
                int mb = (int)(maxAnnexSize / 1024 / 1024);
                Messages.ShowError(Texts.WRONG_ANNEX_SIZE + " " + mb.ToString() + " MB");
                return;
            }
            // Cargar el archivo
            byte[] contenido = FileUtils.GetBinaryFromDisc(filename);
            int size = contenido.Length;
            if (size == 0)
            {
                Messages.ShowError(Texts.ERROR_LOADING_ANNEX);
                return;
            }
            // Agregar a la lista
            Annexes.Items.Add(new ListViewItem(new string[] { name, size.ToString("N0") }));
            //
            // Importante: el nuevo anexo se guarda con DocId = 0, para que al salvar el documento se guarde
            // en la BD. Si se guarda con el DocId actual, se perdería al salir sin salvar.
            //
            CurrentDoc.Annexes.Add(new Annex(0, name, size, contenido));
            SetTextChanged();
        }
    }
}

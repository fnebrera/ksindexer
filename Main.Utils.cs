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
                stripMenuExport.Enabled = toolBtnExportHtml.Enabled = false;
                MenuPrint.Enabled = false;
                MenuDelete.Enabled = toolBtnDelete.Enabled = false;
                MenuUpdatePdf.Enabled = false;
                btnAttAdd.Enabled = btnKeyAdd.Enabled = btnAnxAdd.Enabled = false;
            }
            else
            {
                MenuSave.Enabled = toolBtnSave.Enabled = DocChanged;
                MenuRegenMetadata.Enabled = toolBtnRegenMetadata.Enabled = CurrentDoc.TextoImportado.Length > 0;
                stripMenuExport.Enabled = toolBtnExportHtml.Enabled = true;
                MenuPrint.Enabled = true;
                MenuDelete.Enabled = toolBtnDelete.Enabled = CurrentDoc.Id > 0;
                MenuUpdatePdf.Enabled = true;
                btnAttAdd.Enabled = btnKeyAdd.Enabled = btnAnxAdd.Enabled = true;
            }
            statusId.BackColor = DocChanged ? Color.LightCoral : Color.White;
        }

        // Establecer si el documento ha cambiado y habilitar/deshabilitar los controles
        private void SetTextChanged(bool changed = true)
        {
            DocChanged = changed;
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
            pdfView.Hide();
            this.statusId.Text = "vacío";
            DocChanged = false;
            DocEmpty = true;
            EnableControls();
        }

        // Regenerar los metadatos
        private void RegenMetadata()
        {
            if (CurrentDoc.TextoImportado.Length == 0)
            {
                Messages.ShowError("No hay texto importado para procesar");
                return;
            }
            // Obtener el titulo del documento
            Title.Text = FileUtils.GetDocTitle(CurrentDoc.TextoImportado);
            CurrentDoc.Titulo = Title.Text;
            // Obtener la fecha del documento
            DocDate.Text = FileUtils.GetDocDate(CurrentDoc.TextoImportado);
            if (DocDate.Text.Length > 0)
            {
                CurrentDoc.Fecha = DateTime.Parse(DocDate.Text);
            }
            // Obtener las palabras clave del documento 
            string[] keywords = FileUtils.GetDocKeywords(CurrentDoc.TextoImportado);
            // Mostrar las palabras clave en el ListBox
            Keywords.Items.Clear();
            CurrentDoc.Claves.Clear();
            foreach (string keyword in keywords)
            {
                Keywords.Items.Add(keyword);
                CurrentDoc.Claves.Add(new Keyword(0, keyword));
            }
            // Obtener los asistentes del documento
            string[][] attendants = FileUtils.GetDocAttendants(CurrentDoc.TextoImportado);
            // Mostrar los asistentes en el ListView
            Attendants.Items.Clear();
            CurrentDoc.Asistentes.Clear();
            Attendants.View = View.Details;
            foreach (string[] attendant in attendants)
            {
                Attendants.Items.Add(new ListViewItem(attendant));
                CurrentDoc.Asistentes.Add(new Attendant(0, attendant[0], attendant[1]));
            }
            // Mostrar el texto limpio en el control
            TextInDb.Text = FileUtils.GetTextCleared(CurrentDoc.TextoImportado);
            CurrentDoc.Texto = TextInDb.Text;
            // Indicar que el documento ha cambiado
            DocChanged = true;
            DocEmpty = false;
            EnableControls();
        }

        private void FillControlsFromDoc()
        {
            // Mostrar el contenido en el TextBox
            TextInDb.Text = CurrentDoc.Texto;
            // Mostrar el titulo
            Title.Text = CurrentDoc.Titulo;
            // Mostrar la fecha
            DocDate.Text = CurrentDoc.Fecha.ToString("dd/MM/yyyy HH:mm:ss");
            // Numero de documento en satusBar
            statusId.Text = CurrentDoc.Id.ToString();
            // Mostrar las palabras clave en el ListBox
            Keywords.Items.Clear();
            foreach (Keyword palabra in CurrentDoc.Claves)
            {
                Keywords.Items.Add(palabra.Key);
            }
            // Mostrar los asistentes en el ListView
            Attendants.Items.Clear();
            Attendants.View = View.Details;
            foreach (Attendant asistente in CurrentDoc.Asistentes)
            {
                Attendants.Items.Add(new ListViewItem(new string[] { asistente.Name, asistente.Company ?? "" }));
            }
            // Mostrar el PDF en el WebBrowser
            if (CurrentDoc.Pdf.Length > 0)
            {
                string filename = Path.GetTempFileName() + "kstmpfile.pdf";
                File.WriteAllBytes(filename, CurrentDoc.Pdf);
                pdfView.Navigate(filename);
                pdfView.Show();
            }
            else
            {
                pdfView.Hide();
            }
            // Mostrar los anexos en el ListView
            Annexes.Items.Clear();
            Annexes.View = View.Details;
            foreach (Annex anexo in CurrentDoc.Anexos)
            {
                Annexes.Items.Add(new ListViewItem(new string[] { anexo.FileName, anexo.Size.ToString("N0") }));
            }
        }

        private void FillDocFromControls()
        {
            // Obtener el texto del TextBox
            CurrentDoc.Texto = TextInDb.Text;
            // Obtener el titulo
            CurrentDoc.Titulo = Title.Text;
            // Obtener la fecha
            CurrentDoc.Fecha = DateTime.Parse(DocDate.Text);
            // Obtener las palabras clave del ListBox
            CurrentDoc.Claves.Clear();
            foreach (string keyword in Keywords.Items)
            {
                CurrentDoc.Claves.Add(new Keyword(CurrentDoc.Id, keyword));
            }
            // Obtener los asistentes del ListView
            CurrentDoc.Asistentes.Clear();
            foreach (ListViewItem item in Attendants.Items)
            {
                CurrentDoc.Asistentes.Add(new Attendant(CurrentDoc.Id, item.SubItems[0].Text, item.SubItems[1].Text));
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
                Messages.ShowError("No se puede agregar un directorio");
                return;
            }
            // Verificar que no existe ya
            string name = Path.GetFileName(filename);
            foreach (Annex anexo in CurrentDoc.Anexos)
            {
                if (anexo.FileName == name)
                {
                    Messages.ShowError("Ya existe un anexo con ese nombre");
                    return;
                }
            }
            // Verificar que no exceda el tamaño maximo
            if (fi.Length > maxAnnexSize)
            {
                int mb = (int)(maxAnnexSize / 1024 / 1024);
                Messages.ShowError("No se permite añadir archivos con un tamaño mayor de " + mb.ToString() + "MB");
                return;
            }
            // Cargar el archivo
            byte[] contenido = FileUtils.GetBinaryFromDisc(filename);
            int size = contenido.Length;
            if (size == 0)
            {
                Messages.ShowError("No se ha podido cargar el archivo");
                return;
            }
            // Agregar a la lista
            Annexes.Items.Add(new ListViewItem(new string[] { name, size.ToString("N0") }));
            //
            // Importante: el nuevo anexo se guarda con DocId = 0, para que al salvar el documento se guarde
            // en la BD. Si se guarda con el DocId actual, se perdería al salir sin salvar.
            //
            CurrentDoc.Anexos.Add(new Annex(0, name, size, contenido));
            SetTextChanged();
        }
    }
}

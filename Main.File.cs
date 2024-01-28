using ksindexer;
using ksindexer.Db;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KsIndexerNET
{
    partial class Main
    {
        private void DoMenuImport()
        {
            // Verificar si hay cambios sin guardar
            if (DocChanged && !Messages.AskDocChanged())
                return;
            // Limpiar todos los controles y el documento asociado
            ClearAll();
            // Mostrar dialogo para abrir archivo
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            // Nombre del archivo, nos va a hacer falta luego
            string filename = openFileDialog.FileName;
            // Leer el archivo
            string content = FileUtils.GetTextFromDisc(filename);
            // Mostrar el contenido en el dialogo de importacion
            DlgImport dlg = new DlgImport();
            dlg.SetText(content);
            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            dlg.Dispose();
            // Guardamos texto importado en el documento actual
            CurrentDoc.TextoImportado = dlg.GetText();
            // Intentar leer un PDF con el mismo nombre
            string pdfname = Path.ChangeExtension(filename, "pdf");
            CurrentDoc.Pdf = FileUtils.GetBinaryFromDisc(pdfname);
            // Mostrar el contenido en el WebBrowser
            if (CurrentDoc.Pdf.Length > 0)
            {
                pdfView.Navigate(pdfname);
                pdfView.Show();
            }
            else
            {
                pdfView.Hide();
            }
            //pdfView.Refresh(WebBrowserRefreshOption.Completely);
            statusId.Text = "importado";
            // Regenerar los metadatos
            RegenMetadata();
        }

        private void DoMenuSave()
        {
            // Si no ha cambiado, o esta vacio, no hacer nada
            if (DocEmpty || !DocChanged)
                return;
            // Al menos necesitamos titulo y fecha
            if (Title.Text.Length == 0 || DocDate.Text.Length == 0)
            {
                Messages.ShowError("El documento debe tener al menos un titulo y una fecha/hora");
                return;
            }
            DateTime fecha;
            if (!DateTime.TryParse(DocDate.Text, out fecha))
            {
                Messages.ShowError("La fecha/hora no tiene un formato válido");
                return;
            }
            // Si es nuevo, verificar que no exista ya un documento con la misma fecha/hora
            if (CurrentDoc.Id == 0 && Document.ExistsSimilar(Title.Text, fecha))
            {
                if (!Messages.Confirm("Ya existe un documento con el mismo titulo y fecha/hora\n¿Está seguro de guardar el actual?"))
                    return;
            }
            // Actualizar el documento con los valores de los controles
            FillDocFromControls();
            // Guardar documento actual en la BD
            if (!CurrentDoc.Save())
            {
                Messages.ShowError("Se ha producido un error al guardar el documento");
                return;
            }
            // Mostrar id en el status bar
            statusId.Text = CurrentDoc.Id.ToString();
            Messages.ShowInfo("Documento guardado con id: " + CurrentDoc.Id);
            DocChanged = false;
            EnableControls();
        }

        private void DoMenuDelete()
        {
            if (CurrentDoc.Id == 0)
            {
                Messages.ShowError("No hay documento seleccionado");
                return;
            }
            if (!Messages.Confirm("¿Está seguro de eliminar el documento actual?\nTodos los datos se perderán"))
                return;
            if (!Document.Delete(CurrentDoc.Id))
            {
                Messages.ShowError("Se ha prodcido un error al eliminar el documento");
                return;
            }
            ClearAll();
        }

        private void DoMenuUpdatePdf()
        {
            // Mostrar dialogo para abrir archivo
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos Pdf (*.pdf)|*.pdf|Todos los archivos (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            // Nombre del archivo
            string filename = openFileDialog.FileName;
            // Cargar el archivo en el documento actual
            CurrentDoc.Pdf = FileUtils.GetBinaryFromDisc(filename);
            // Mostrar el contenido en el WebBrowser
            if (CurrentDoc.Pdf.Length > 0)
            {
                pdfView.Navigate(filename);
                pdfView.Show();
            }
            else
            {
                pdfView.Hide();
            }
            //pdfView.Refresh(WebBrowserRefreshOption.Completely);
            SetTextChanged();
        }

        private void DoMenuRegenMetadata()
        {
            // Verificar si es posible hacerlo
            if (CurrentDoc.TextoImportado.Length == 0)
            {
                Messages.ShowError("No hay texto importado para procesar");
                return;
            }
            // Mostrar el contenido en el dialogo de importacion
            DlgImport dlg = new DlgImport();
            dlg.SetText(CurrentDoc.TextoImportado);
            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            // Guardamos texto importado en el documento actual
            CurrentDoc.TextoImportado = dlg.GetText();
            dlg.Dispose();
            // Regenerar los metadatos
            DocChanged = true;
            RegenMetadata();
        }
    }
}

using KsIndexerNET;
using KsIndexerNET.Db;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            // Mostrar dialogo para abrir archivo
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            // Nombre del archivo
            string filename = openFileDialog.FileName;
            ImportFromFile(filename);
        }

        private void ImportFromFile(string filename)
        {
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
            ClearAll();
            // Guardamos texto importado en el documento actual
            CurrentDoc.ImportedText = dlg.GetText();
            // Intentar leer un PDF con el mismo nombre
            string pdfname = Path.ChangeExtension(filename, "pdf");
            CurrentDoc.Pdf = FileUtils.GetBinaryFromDisc(pdfname);
            // Mostrar el contenido en el WebBrowser
            if (CurrentDoc.Pdf.Length > 0)
            {
                pdfView.Navigate(pdfname);
            }
            else
            {
                pdfView.Navigate(new Uri("about:blank"));
            }
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
                Messages.ShowError(Texts.TITLE_AND_DATE_REQUIRED);
                return;
            }
            DateTime fecha = LangUtils.ParseDateTime(DocDate.Text);
            if (fecha == DateTime.MinValue)
            {
                Messages.ShowError(Texts.WRONG_DATE_FORMAT);
                return;
            }
            // Es nuevo ?
            bool isNew = CurrentDoc.Id == 0;
            // Si es nuevo, verificar que no exista ya un documento con la misma fecha/hora
            if (isNew && Document.ExistsSimilar(Title.Text, fecha))
            {
                if (!Messages.Confirm(Texts.SIMILAR_DOC_EXISTS + "\n" + Texts.OK_TO_PROCEED))
                    return;
            }
            // Actualizar el documento con los valores de los controles
            FillDocFromControls();
            // Guardar documento actual en la BD
            if (!CurrentDoc.Save())
            {
                Messages.ShowError(Texts.ERROR_SAVE_DOC);
                return;
            }
            // Mostrar id en el status bar
            statusId.Text = CurrentDoc.Id.ToString();
            if(isNew)
                Messages.ShowInfo(Texts.DOC_SAVED_WITH_ID + ": " + CurrentDoc.Id);
            DocChanged = false;
            EnableControls();
        }

        private void DoMenuDelete()
        {
            if (CurrentDoc.Id == 0)
            {
                Messages.ShowError(Texts.NO_DOC_SELECTED);
                return;
            }
            if (!Messages.Confirm(Texts.CONFIRM_DELETE_DOC + "\n" + Texts.CONFIRM_DELETE_DOC1))
                return;
            if (!Document.Delete(CurrentDoc.Id))
            {
                Messages.ShowError(Texts.ERROR_DELETE_DOC);
                return;
            }
            ClearAll();
        }

        private void DoMenuUpdatePdf()
        {
            // Si ya tenemos Pdf, pedir confirmación
            if (CurrentDoc.Pdf.Length > 0)
            {
                if (!Messages.Confirm(Texts.CONFIRM_PDF_ALREADY_EXISTS + "\n" + Texts.CONFIRM_PDF_ALREADY_EXISTS1))
                    return;
            }
            // Mostrar dialogo para abrir archivo
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
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
            }
            else
            {
                pdfView.Navigate(new Uri("about:blank"));
            }
            //pdfView.Refresh(WebBrowserRefreshOption.Completely);
            SetTextChanged();
        }

        private void DoMenuRegenMetadata()
        {
            // Verificar si es posible hacerlo
            if (CurrentDoc.ImportedText.Length == 0)
            {
                Messages.ShowError(Texts.NO_TEXT_TO_PROCESS);
                return;
            }
            // Mostrar el contenido en el dialogo de importacion
            DlgImport dlg = new DlgImport();
            dlg.SetText(CurrentDoc.ImportedText);
            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            // Guardamos texto importado en el documento actual
            CurrentDoc.ImportedText = dlg.GetText();
            dlg.Dispose();
            // Regenerar los metadatos
            DocChanged = true;
            RegenMetadata();
        }

        private void DoMenuSettings()
        {
            DlgSettings dlg = new DlgSettings();
            dlg.SetCulture(Thread.CurrentThread.CurrentUICulture.Name);
            dlg.SetDateFormat(CurrentDateFormat);
            dlg.SetMaxSize(MaxAnnexSize);
            dlg.SetStartDate(DateStartChar);
            dlg.SetStartAttendant(AttendantStartChar);
            dlg.SetStartTag(TagStartChar);
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            bool save = false;
            // Cambiar el idioma
            string culture = dlg.GetCulture();
            if (culture != Thread.CurrentThread.CurrentUICulture.Name)
            {
                save = true;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
                LangUtils.TranslateForm(this);
            }
            string dateformat = dlg.GetDateFormat();
            string datetimeformat = dateformat + " HH:mm";
            int maxSize = dlg.GetMaxSize();
            string dateStartChar = dlg.GetStartDate().Trim();
            string attendantStartChar = dlg.GetStartAttendant().Trim();
            string tagStartChar = dlg.GetStartTag().Trim();
            if (dateformat != CurrentDateFormat)
            {
                save = true;
                DocDate.Text = LangUtils.ConvertDateFormat(DocDate.Text, CurrentDateTimeFormat , datetimeformat);
                CurrentDateFormat = dateformat;
                CurrentDateTimeFormat = datetimeformat;
            }
            if (maxSize != MaxAnnexSize)
            {
                // Cuidado, establecemos un limite glbal maximo de 5 GB
                if (maxSize > 5120)
                    maxSize = 5120;
                save = true;
                MaxAnnexSize = maxSize;
            }
            // V 1.1.6
            if (dateStartChar.Length != 0 && dateStartChar != DateStartChar)
            {
                save = true;
                DateStartChar = dateStartChar;
            }
            if (attendantStartChar.Length != 0 && attendantStartChar != AttendantStartChar)
            {
                save = true;
                AttendantStartChar = attendantStartChar;
            }
            if (tagStartChar.Length != 0 && tagStartChar != TagStartChar)
            {
                save = true;
                TagStartChar = tagStartChar;
            }
            if (save)
            {
                Properties.Settings.Default.Culture = culture;
                Properties.Settings.Default.DateFormat = dateformat;
                Properties.Settings.Default.MaxAnnexSize = maxSize;
                Properties.Settings.Default.DateStartChar = dateStartChar;
                Properties.Settings.Default.AttStartChar = attendantStartChar;
                Properties.Settings.Default.TagStartChar = tagStartChar;
                Properties.Settings.Default.Save();
            }
            dlg.Dispose();
        }
    }
}

/**
 * KsIndexerNET
 * Aplicacion de gestion de documentos para Kindle Scribe
 * @autor Faustino Nebrera
 * @date 2024-01-26
 * @version 1.1.2
 */

using ksindexer;
using ksindexer.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KsIndexerNET
{
    public partial class Main : Form
    {
        // Constantes
        public const string appVersion = "1.1.3";
        public const int maxAnnexSize = 50 * 1024 * 1024; // 50 Mb

        private static Document CurrentDoc = new Document();
        private static bool DocChanged = false;
        private static bool DocEmpty = true;

        public Main()
        {
            InitializeComponent();
            // Open and test the database connection. If it fails, it will show an error message and exit
            _ = Database.GetInstance();
        }

        //
        // Eventos de los menús
        //

        private void MenuNew_Click(object sender, EventArgs e)
        {
            // Verificar si hay cambios sin guardar
            if (DocChanged && !Messages.AskDocChanged())
                return;
            // Limpiar todos los controles y el documento asociado
            ClearAll();
        }

        private void MenuImport_Click(object sender, EventArgs e)
        {
            DoMenuImport();
        }

        private void MenuSave_Click(object sender, EventArgs e)
        {
            DoMenuSave();
        }

        private void MenuRegenMetadata_Click(object sender, EventArgs e)
        {
            DoMenuRegenMetadata();
        }

        private void MenuPrint_Click(object sender, EventArgs e)
        {
            DoMenuPrint();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            // Solicitar confirmacion
            if (!Messages.CanExit(DocChanged))
                return;
            // Terminar la Aplicacon
            System.Environment.Exit(0);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Solicitar confirmacion
            if (!Messages.CanExit(DocChanged))
            {
                e.Cancel = true;
                return;
            }
        }

        private void MenuSearchById_Click(object sender, EventArgs e)
        {
            SearchById();
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            // Mostrar un dialogo con informacion de la aplicacion
            Messages.ShowInfo("KsIndexer v" + appVersion + "\nIndexador de documentos para Kindle Scribe\n(C) 2024 Faustino Nebrera");
        }

        private void btnKeyAdd_Click(object sender, EventArgs e)
        {
            BtnKeyAdd();
        }

        private void btnKeyEdit_Click(object sender, EventArgs e)
        {
            BtnKeyEdit();
        }

        private void btnKeyDelete_Click(object sender, EventArgs e)
        {
            BtnKeyDelete();
        }

        private void btnAsisAdd_Click(object sender, EventArgs e)
        {
            BtnAttAdd();
        }

        private void btnAsisEdit_Click(object sender, EventArgs e)
        {
            BtnAttEdit();
        }

        private void btnAsisDelete_Click(object sender, EventArgs e)
        {
            BtnAttDelete();
        }

        private void Title_TextChanged(object sender, EventArgs e)
        {
            SetTextChanged();
        }

        private void DocDate_TextChanged(object sender, EventArgs e)
        {
            SetTextChanged();
        }

        private void Main_Layout(object sender, LayoutEventArgs e)
        {
            // Ajustar posicion de los controles al cambiar el tamaño de la ventana
            int freeHeight = this.ClientSize.Height - menuStrip1.Height - statusStrip1.Height - toolStripMain.Height;
            int freeWidth = this.ClientSize.Width;
            // Si es muy pequeña, ni caso
            if (freeWidth < 500 || freeHeight < 300 )
                return;
            // Controles de la derecha ajustados a la derecha
            int titleLeft = freeWidth - Title.Width - 10;
            int btn3Left = freeWidth - btnKeyDelete.Width - 10;
            int btn2Left = btn3Left - btnKeyEdit.Width - 10;
            int btn1Left = btn2Left - btnKeyAdd.Width - 10;
            Title.Left = lblTitle.Left = lblDate.Left = DocDate.Left = 
                lblAttendats.Left = Attendants.Left = 
                lblKeywords.Left = Keywords.Left = 
                lblAnnexes.Left = Annexes.Left = titleLeft;
            // Boton Ahora
            btnDateNow.Left = DocDate.Right + 10;
            btnDateNow.Height = DocDate.Height;
            // Botones de asistentes, keywords y anexos
            btnAttAdd.Left = btnKeyAdd.Left = btnAnxAdd.Left = btn1Left;
            btnAttDelete.Left = btnKeyDelete.Left = btnAnxDelete.Left = btn2Left;
            btnAttEdit.Left = btnKeyEdit.Left = btnAnxView.Left = btn3Left;
            // splitPane de de la izquierda ajustado al espacio que nos queda
            splitLeftPane.Left = 5;
            splitLeftPane.Width = titleLeft - 10;
            splitLeftPane.Top = menuStrip1.Height + toolStripMain.Height + 5;
            splitLeftPane.Height = freeHeight - 10;
            // Ajustamos verticalmente las tres listas y sus botones a 1/3 de lo disponible cada una
            freeHeight = (this.ClientSize.Height - DocDate.Bottom - statusStrip1.Height) / 3;
            // Attendants
            lblAttendats.Top = DocDate.Bottom + 5;
            Attendants.Top = lblAttendats.Bottom + 5;
            Attendants.Height = freeHeight - lblAttendats.Height - btnAttAdd.Size.Height - 15;
            btnAttAdd.Top = btnAttEdit.Top = btnAttDelete.Top = Attendants.Bottom + 5;
            // Keywords
            lblKeywords.Top = btnAttAdd.Bottom + 5;
            Keywords.Top = lblKeywords.Bottom + 5;
            Keywords.Height = freeHeight - lblKeywords.Height - btnKeyAdd.Size.Height - 15;
            btnKeyAdd.Top = btnKeyEdit.Top = btnKeyDelete.Top = Keywords.Bottom + 5;
            // Annexes
            lblAnnexes.Top = btnKeyAdd.Bottom + 5;
            Annexes.Top = lblAnnexes.Bottom + 5;
            Annexes.Height = freeHeight - lblAnnexes.Height - btnAnxAdd.Size.Height - 15;
            btnAnxAdd.Top = btnAnxView.Top = btnAnxDelete.Top = Annexes.Bottom + 5;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            DocChanged = false;
            DocEmpty = true;
            EnableControls();
        }

        private void TextInDb_TextChanged(object sender, EventArgs e)
        {
            SetTextChanged();
        }

        private void MenuSearchByDate_Click(object sender, EventArgs e)
        {
            SearchByDate();
        }

        private void MenuSearchComplex_Click(object sender, EventArgs e)
        {
            SearchComplex();
        }

        private void Attendants_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAttDelete.Enabled = btnAttEdit.Enabled = Attendants.SelectedItems.Count > 0;
        }

        private void Keywords_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnKeyDelete.Enabled = btnKeyEdit.Enabled = Keywords.SelectedItems.Count > 0;
        }

        private void DocDate_Leave(object sender, EventArgs e)
        {
            if (DocDate.Text.Length == 0)
                return;
            if (DateTime.TryParse(DocDate.Text, out DateTime dt))
            {
                DocDate.Text = dt.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                Messages.ShowError("La fecha/hora no tiene un formato válido");
                DocDate.SelectAll();
                DocDate.Focus();
                return;
            }
        }

        private void menuExportHtml_Click(object sender, EventArgs e)
        {
            DoMenuExportHtml();
        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            DoMenuDelete();
        }

        private void MenuReadme_Click(object sender, EventArgs e)
        {
            // TODO: Escribir un help completo
            Process notepad = new Process();
            notepad.StartInfo.FileName = "notepad.exe";
            notepad.StartInfo.Arguments = ".\\Leeme.txt";
            notepad.Start();
            notepad.WaitForExit();
        }

        private void MenuExportTxt_Click(object sender, EventArgs e)
        {
            DoMenuExportTxt();
        }

        private void MenuUpdatePdf_Click(object sender, EventArgs e)
        {
            DoMenuUpdatePdf();
        }

        private void btnAnxAdd_Click(object sender, EventArgs e)
        {
            BtnAnxAdd();
        }

        private void btnAnxDelete_Click(object sender, EventArgs e)
        {
            BtnAnxDelete();
        }

        private void btnAnxView_Click(object sender, EventArgs e)
        {
            BtnAnxView();
        }

        private void Annexes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAnxDelete.Enabled = btnAnxView.Enabled = Annexes.SelectedItems.Count > 0;
        }

        private void Annexes_DragDrop(object sender, DragEventArgs e)
        {
            // Agregar el archivo en el clipboard
            if (DocEmpty || !e.Data.GetDataPresent(DataFormats.FileDrop))
                return;
            // Obtener el nombre del archivo
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string filename = files[0];
            AddAnnexToDoc(filename);
        }

        private void Annexes_DragEnter(object sender, DragEventArgs e)
        {
            // Solo si el documento no está vacío, y si en el drop viene un archivo
            if (!DocEmpty && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All; // OK
            else
                e.Effect = DragDropEffects.None; // None
        }

        private void TextInDb_DragEnter(object sender, DragEventArgs e)
        {
            // Solo si el documento no se ha modificado, y si en el drop viene un archivo
            if (!DocChanged && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All; // OK
            else
                e.Effect = DragDropEffects.None; // None
        }

        private void TextInDb_DragDrop(object sender, DragEventArgs e)
        {
            // Agregar el archivo en el clipboard
            if (DocChanged || !e.Data.GetDataPresent(DataFormats.FileDrop))
                return;
            // Obtener el nombre del archivo
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string filename = files[0];
            if (Path.GetExtension(filename) != ".txt")
                return;
            ImportFromFile(filename);
        }

        private void acercaDeLaBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fi = new FileInfo(Database.GetDbPath());
            Messages.ShowInfo("Versión de la base de datos: " + Database.GetDbVersion() +
                "\nTamaño de la base de datos: " + (fi.Length / 1024).ToString("N0") + " KBytes");
        }

        private void compactarLaBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Messages.Confirm("El proceso de compactación de la base de datos puede tardar varios minutos.\n¿Desea continuar?"))
                return;
            Database.VacuumDb();
        }

        private void btnUpdatePdf_Click(object sender, EventArgs e)
        {
            DoMenuUpdatePdf();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DoMenuPrint();
        }

        private void pdfView_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ((WebBrowser)sender).Show();
            ((WebBrowser)sender).Invalidate();
        }

        private void btnDateNow_Click(object sender, EventArgs e)
        {
            DocDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }
    }
}

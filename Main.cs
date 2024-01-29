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
            // TODO: Imprimir el documento actual
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
            // Usamos 1/3 de la altura disponible para el TextBox
            TextInDb.Height = freeHeight / 3;
            TextInDb.Top = menuStrip1.Height + toolStripMain.Height + 5;
            // El pdf 5 pixeles por debajo
            pdfView.Top = TextInDb.Bottom + 5;
            pdfView.Height = freeHeight - TextInDb.Height - 20;
            // Ambos ocupan todo el ancho disponible, salvo que nos hayan cerrado demasiado
            if (freeWidth < 700)
                return;
            // Controles de la derecha
            int titleLeft = freeWidth - Title.Width - 10;
            int btn3Left = freeWidth - btnKeyDelete.Width - 10;
            int btn2Left = btn3Left - btnKeyEdit.Width - 10;
            int btn1Left = btn2Left - btnKeyAdd.Width - 10;
            TextInDb.Width = titleLeft - 10;
            pdfView.Width = TextInDb.Width;
            Title.Left = lblTitle.Left = lblDate.Left = DocDate.Left = 
                lblAttendats.Left = Attendants.Left = 
                lblKeywords.Left = Keywords.Left = 
                lblAnnexes.Left = Annexes.Left = titleLeft;
            btnAttAdd.Left = btnKeyAdd.Left = btnAnxAdd.Left = btn1Left;
            btnAttDelete.Left = btnKeyDelete.Left = btnAnxDelete.Left = btn2Left;
            btnAttEdit.Left = btnKeyEdit.Left = btnAnxView.Left = btn3Left;
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
            DateTime dt;
            if (DocDate.Text.Length > 0 && !DateTime.TryParse(DocDate.Text, out dt))
            {
                Messages.ShowError("La fecha/hora no tiene un formato válido");
                DocDate.SelectAll();
                DocDate.Focus();
                return;
            }
        }

        private void menuExportHtml_Click(object sender, EventArgs e)
        {
            MenuExportHtml();
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
            MenuExportTxt();
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
    }
}

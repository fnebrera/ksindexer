/**
 * ---------
 * KsIndexer
 * ---------
 * 
 * Aplicacion de gestion de documentos para Kindle Scribe y Remarkable
 * (C) 2024 Faustino Nebrera
 * 
 * Version Date       By  Notes
 * ------- ---------- --- ----------------------------------------------------------------------------- 
 * 1.0.0   2024-01-12 FNG Version preliminar
 * 1.1.0   2024-01-26 FNG Se agrega la tabla DbVersion.
 *                        Se agrega el campo TextNorm a la tabla Documents.
 * 1.1.1   2024-01-27 FNG Pequeños cambios
 * 1.1.2   2024-01-27 FNG Mas correcciones y pequeños cambios.
 *                        Se ajusta la posicion de los controles al cambiar el tamaño de la ventana.
 * 1.1.3   2024-01-29 FNG Se agrega la tabla Doc_Annexes para almacenar los anexos de los documentos.
 *                        Se corrige un problema en la busqueda por palabras clave unidas por AND.
 *                        Se incluye un SplitContainer para poder ajustar los tamaños de Texto y Pdf.
 * 1.1.4   2024-01-30 FNG Se implementa la opcion Imprimir.
 *                        Se agregan shortcuts a las entradas del menu.
 *                        Se formatea la fecha en el evento Leave del campo DocDate.
 *                        Se agrega un boton'Ahora' para poner la fecha actual en el campo DocDate.
 * 1.1.5   2024-02-01 FNG Se convierte en multidioma, empleando archivos .resx. En esta version se soporta
 *                        español e inglés.
 *                        Se implementan settings para el idioma y el formato de fechas.
 * 1.1.6   2024-02-06 FNG Se admiten varios asistentes en una sóla línea.
 *                        Se eliminan las posibles comas al final de las palabras clave.
 *                        Se permite al usuario establecer los símbolos que marcan fecha, asistentes y palabras clave.
 * 1.2.0   2024-02-11 FNG Se agrega la estructura de carpetas para almacenar los documentos.
 *                        Al salvar un documento nuevo, se muestra el dialogo de seleccion de carpeta.
 *                        Nuevo menu "Abrir" que muestra el diálogo de seleccion de carpeta y documento.
 *                        Se incluyen funciones para eliminar, agregar, cambiar nombre, mover, etc. carpetas y documentos.
 */

using KsIndexerNET;
using KsIndexerNET.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KsIndexerNET
{
    public partial class Main : Form
    {
        // Constantes
        public const string appVersion = "1.2.0";

        //
        // Preferencias de usuario, que leemos de settings
        //

        // Formato para parsear y formatear fechas, p.e. dd/MM/yyyy
        public static string CurrentDateFormat { get; set; }
        // Formato para formatear fecha y hora, p.e. dd/MM/yyyy HH:mm
        public static string CurrentDateTimeFormat { get; set; }
        // Tamaño maximo de los anexos, en bytes
        public static int MaxAnnexSize { get; set; }
        // V 1.1.6 - Símbolos para marcar fecha, asistentes y palabras clave
        public static string DateStartChar { get; set; }
        public static string AttendantStartChar { get; set; }
        public static string TagStartChar { get; set; }

        //
        // Variables estáticas de uso común
        //

        private static Document CurrentDoc = new Document();
        private static bool DocChanged = false;
        private static bool DocEmpty = true;
        // V 1.2.0 - Inode donde se ha guardado el ultimo documento
        private static string LastInode = "1";

        public Main()
        {
            InitializeComponent();
            //
            // Database es una clase singleton, por lo que la instanciamos al principio, con lo que verificamos
            // que la base de datos esté disponible y actualizada.
            //
            _ = Database.GetInstance();
            //
            // Cargar setting actuales. Se actualizan mediante el evento MenuSettings_Click
            //
            // CultureInfo nos sirve para traducir los textos de los controles y los mensajes.
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Culture);
            // TODO: Ver si es necesario cambiar el CurrentCulture (por separador de decimales, etc.). De momento no.
            // De manera separada, cargamos los formatos de fecha, que no necesariamente son iguales que el CultureInfo.
            // Por ejemplo, el usuario puede querer la interfaz en español pero las fechas en ANSI (yyyy-MM-dd).
            CurrentDateFormat = Properties.Settings.Default.DateFormat;
            CurrentDateTimeFormat = CurrentDateFormat + " HH:mm";
            // Tamaño maximo de los anexos
            MaxAnnexSize = Properties.Settings.Default.MaxAnnexSize;
            if (MaxAnnexSize == 0)
                MaxAnnexSize = 50;
            // Símbolos para marcar fecha, asistentes y palabras clave
            DateStartChar = Properties.Settings.Default.DateStartChar;
            if (DateStartChar.Length == 0)
                DateStartChar = "@";
            AttendantStartChar = Properties.Settings.Default.AttStartChar;
            if (AttendantStartChar.Length == 0)
                AttendantStartChar = ">";
            TagStartChar = Properties.Settings.Default.TagStartChar;
            if (TagStartChar.Length == 0)
                TagStartChar = "#";

            //
            // NOTA: Cómo se maneja el multi-idioma:
            // 1. Existe un archivo Texts.resx y su paralelo Texts.es-US.resx con textos generales de la aplicación,
            //    como puedan ser mensajes de error, confirmaciones, etc. Visual Studio genera una clase Texts (en Texts.designer.cs)
            //    que nos permite acceder a los textos como propiedades de esta clase, que son extraidos automáticamente
            //    del archivo con la cultura actual. Si no existe la cultura, se hace fallback a la de defecto, en este
            //    caso a Español.
            // 2. Los textos de los controles (botones, labels, etc.), así como mensajes u otros textos específicos de un
            //    Form se manejan mediante los archivo de recursos .resx asociados a cada Form. Hay un archivo xxx.resx
            //    para la cultura por defecto (Español) y otro para cada cultura alternativa (p.e. xxx.en-Us.resx). En la
            //    clase LangUtils hay métodos estáticos para traducir de una vez todos los controles de un Form y para
            //    traducir un texto específico.
            // 3. Tambien en la clase LangUtils hay métodos estáticos para parsear y formatear fechas y horas, en base, en este caso,
            //    a los formatos de fecha y hora que se leen de settings (NO a la cultura).
            // 
            // VisualStudio compila los archivos .resx en forma de DLLs, que se deben incluir a la hora de generar el setup,
            // agregando al ApplicationFolder lo que VisualStudio llama 'Recursos adaptados'.
            //

            // PRUEBAS para ver que leñes hace el .NET con los recursos. Es un tema peliaguo.
            /*
            string[] resxnames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            StringBuilder sb = new StringBuilder();
            foreach (string resxname in resxnames)
            {
                sb.Append(resxname + "\t" + "\n");
            }
            MessageBox.Show(sb.ToString());
            */
        }

        /// <summary>
        /// Establecer el path del documento, si es el mismo que estamos visualizando.
        /// Se llama desde otras ventanas, si cambia el path o es un documento nuevo.
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="path"></param>
        public void SetDocumentPath(int docId, string path)
        {
            if (CurrentDoc.Id == docId)
                this.statusDocPath.Text = path;
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
            //Messages.ShowInfo("KsIndexer v" + appVersion + "\nIndexador de documentos para Kindle Scribe\n(C) 2024 Faustino Nebrera");
            Messages.ShowInfo(Texts.ABOUT1 + appVersion + "\n" + Texts.ABOUT2 + "\n(C) 2024 Faustino Nebrera");
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
            // Ancho de los controles (.NET hace lo que le da la gana)
            Title.Width = Attendants.Width = Keywords.Width = Annexes.Width = 450;
            DocDate.Width = 150;
            btnDateNow.Width = btnAttAdd.Width = btnAttDelete.Width = btnAttEdit.Width = 
                btnKeyAdd.Width = btnKeyDelete.Width = btnKeyEdit.Width = 
                btnAnxAdd.Width = btnAnxDelete.Width = btnAnxView.Width = 80;
            // Controles de la derecha ajustados a la derecha
            int titleLeft = freeWidth - Title.Width - 10;
            int btn3Left = freeWidth - btnKeyDelete.Width - 10;
            int btn2Left = btn3Left - btnKeyEdit.Width - 10;
            int btn1Left = btn2Left - btnKeyAdd.Width - 10;
            Title.Left = lblTitle.Left = lblDate.Left = DocDate.Left = 
                lblAttendants.Left = Attendants.Left = 
                lblKeywords.Left = Keywords.Left = 
                lblAnnexes.Left = Annexes.Left = titleLeft;
            // Controles de la derecha excepto listas
            lblTitle.Top = menuStrip1.Height + toolStripMain.Height + 5;
            Title.Top = lblTitle.Bottom + 5;
            lblDate.Top = Title.Bottom + 5;
            DocDate.Top = btnDateNow.Top = lblDate.Bottom + 5;
            // Boton Ahora
            btnDateNow.Left = DocDate.Right + 10;
            btnDateNow.Height = DocDate.Height;
            // Todos los botones, de 30 pixeles de alto
            btnAttAdd.Height = btnAttEdit.Height = btnAttDelete.Height = 
                btnKeyAdd.Height = btnKeyEdit.Height = btnKeyDelete.Height = 
                btnAnxAdd.Height = btnAnxView.Height = btnAnxDelete.Height = 30;
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
            lblAttendants.Top = DocDate.Bottom + 5;
            Attendants.Top = lblAttendants.Bottom + 5;
            Attendants.Height = freeHeight - lblAttendants.Height - btnAttAdd.Size.Height - 15;
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
            LangUtils.TranslateForm(this);
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
            DateTime dt = LangUtils.ParseDateTime(DocDate.Text);
            if (dt != DateTime.MinValue)
            {
                DocDate.Text = LangUtils.FormatDateTime(dt);
            }
            else
            {
                Messages.ShowError(Texts.WRONG_DATE_FORMAT);
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
            // Mostrar el help
            string hlpfile = @".\help\KsIndexer.chm";
            if (!File.Exists(hlpfile))
            {
                Process notepad = new Process();
                notepad.StartInfo.FileName = "notepad.exe";
                notepad.StartInfo.Arguments = @".\Leeme.txt";
                notepad.Start();
                notepad.WaitForExit();
                return;
            }
            Process.Start(hlpfile);
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
            Messages.ShowInfo(Texts.DB_INFO1 + Database.GetDbVersion() +
                "\n" + Texts.DB_INFO2 + (fi.Length / 1024).ToString("N0") + " KBytes");
        }

        private void compactarLaBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Messages.Confirm(Texts.COMPACT_DATABASE + "\n" + Texts.OK_TO_PROCEED))
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
            DocDate.Text = LangUtils.FormatDateTime(DateTime.Now);
        }

        private void menuSettings_Click(object sender, EventArgs e)
        {
            DoMenuSettings();
        }

        private void MenuOpen_Click(object sender, EventArgs e)
        {
            DoMenuOpen();
        }

        private void MenuSearchByFolder_Click(object sender, EventArgs e)
        {
            SearchByFolder();
        }
    }
}

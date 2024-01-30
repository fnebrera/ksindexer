using System.Windows.Forms;

namespace KsIndexerNET
{
    partial class Main
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuImport = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRegenMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuUpdatePdf = new System.Windows.Forms.ToolStripMenuItem();
            this.stripMenuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExportHtml = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExportTxt = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSalir = new System.Windows.Forms.ToolStripMenuItem();
            this.buscarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSearchById = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSearchByDate = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSearchComplex = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuLeeme = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeLaBaseDeDatosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compactarLaBaseDeDatosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabelId = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusId = new System.Windows.Forms.ToolStripStatusLabel();
            this.TextInDb = new System.Windows.Forms.TextBox();
            this.pdfView = new System.Windows.Forms.WebBrowser();
            this.lblTitle = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.TextBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.DocDate = new System.Windows.Forms.TextBox();
            this.lblAttendats = new System.Windows.Forms.Label();
            this.Attendants = new System.Windows.Forms.ListView();
            this.AttName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AttCompany = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblKeywords = new System.Windows.Forms.Label();
            this.Keywords = new System.Windows.Forms.ListBox();
            this.btnAttAdd = new System.Windows.Forms.Button();
            this.btnAttDelete = new System.Windows.Forms.Button();
            this.btnKeyDelete = new System.Windows.Forms.Button();
            this.btnKeyAdd = new System.Windows.Forms.Button();
            this.btnKeyEdit = new System.Windows.Forms.Button();
            this.btnAttEdit = new System.Windows.Forms.Button();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolBtnNew = new System.Windows.Forms.ToolStripButton();
            this.toolBtnImport = new System.Windows.Forms.ToolStripButton();
            this.toolBtnRegenMetadata = new System.Windows.Forms.ToolStripButton();
            this.btnUpdatePdf = new System.Windows.Forms.ToolStripButton();
            this.toolBtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolBtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolBtnSearchId = new System.Windows.Forms.ToolStripButton();
            this.toolBtnSearchDate = new System.Windows.Forms.ToolStripButton();
            this.toolBtnSearchComplex = new System.Windows.Forms.ToolStripButton();
            this.toolBtnExportHtml = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnAnxView = new System.Windows.Forms.Button();
            this.btnAnxDelete = new System.Windows.Forms.Button();
            this.btnAnxAdd = new System.Windows.Forms.Button();
            this.Annexes = new System.Windows.Forms.ListView();
            this.AnxFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AnxSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblAnnexes = new System.Windows.Forms.Label();
            this.splitLeftPane = new System.Windows.Forms.SplitContainer();
            this.btnDateNow = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLeftPane)).BeginInit();
            this.splitLeftPane.Panel1.SuspendLayout();
            this.splitLeftPane.Panel2.SuspendLayout();
            this.splitLeftPane.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.buscarToolStripMenuItem,
            this.ayudaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1860, 36);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuNew,
            this.MenuImport,
            this.MenuSave,
            this.MenuRegenMetadata,
            this.MenuUpdatePdf,
            this.stripMenuExport,
            this.MenuPrint,
            this.MenuDelete,
            this.MenuSalir});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(98, 32);
            this.archivoToolStripMenuItem.Text = "&Archivo";
            // 
            // MenuNew
            // 
            this.MenuNew.Name = "MenuNew";
            this.MenuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.MenuNew.Size = new System.Drawing.Size(314, 36);
            this.MenuNew.Text = "&Nuevo";
            this.MenuNew.Click += new System.EventHandler(this.MenuNew_Click);
            // 
            // MenuImport
            // 
            this.MenuImport.Name = "MenuImport";
            this.MenuImport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.MenuImport.Size = new System.Drawing.Size(314, 36);
            this.MenuImport.Text = "&Importar";
            this.MenuImport.Click += new System.EventHandler(this.MenuImport_Click);
            // 
            // MenuSave
            // 
            this.MenuSave.Name = "MenuSave";
            this.MenuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.MenuSave.Size = new System.Drawing.Size(314, 36);
            this.MenuSave.Text = "&Guardar";
            this.MenuSave.Click += new System.EventHandler(this.MenuSave_Click);
            // 
            // MenuRegenMetadata
            // 
            this.MenuRegenMetadata.Name = "MenuRegenMetadata";
            this.MenuRegenMetadata.Size = new System.Drawing.Size(314, 36);
            this.MenuRegenMetadata.Text = "&Regenerar metadatos";
            this.MenuRegenMetadata.Click += new System.EventHandler(this.MenuRegenMetadata_Click);
            // 
            // MenuUpdatePdf
            // 
            this.MenuUpdatePdf.Name = "MenuUpdatePdf";
            this.MenuUpdatePdf.Size = new System.Drawing.Size(314, 36);
            this.MenuUpdatePdf.Text = "&Actualizar Pdf";
            this.MenuUpdatePdf.Click += new System.EventHandler(this.MenuUpdatePdf_Click);
            // 
            // stripMenuExport
            // 
            this.stripMenuExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuExportHtml,
            this.menuExportTxt});
            this.stripMenuExport.Name = "stripMenuExport";
            this.stripMenuExport.Size = new System.Drawing.Size(314, 36);
            this.stripMenuExport.Text = "E&xportar";
            // 
            // menuExportHtml
            // 
            this.menuExportHtml.Name = "menuExportHtml";
            this.menuExportHtml.Size = new System.Drawing.Size(270, 36);
            this.menuExportHtml.Text = "Como &HTML";
            this.menuExportHtml.Click += new System.EventHandler(this.menuExportHtml_Click);
            // 
            // menuExportTxt
            // 
            this.menuExportTxt.Name = "menuExportTxt";
            this.menuExportTxt.Size = new System.Drawing.Size(270, 36);
            this.menuExportTxt.Text = "Como &TXT";
            this.menuExportTxt.Click += new System.EventHandler(this.MenuExportTxt_Click);
            // 
            // MenuPrint
            // 
            this.MenuPrint.Name = "MenuPrint";
            this.MenuPrint.Size = new System.Drawing.Size(314, 36);
            this.MenuPrint.Text = "I&mprimir";
            this.MenuPrint.Click += new System.EventHandler(this.MenuPrint_Click);
            // 
            // MenuDelete
            // 
            this.MenuDelete.Name = "MenuDelete";
            this.MenuDelete.Size = new System.Drawing.Size(314, 36);
            this.MenuDelete.Text = "&Eliminar";
            this.MenuDelete.Click += new System.EventHandler(this.MenuDelete_Click);
            // 
            // MenuSalir
            // 
            this.MenuSalir.Name = "MenuSalir";
            this.MenuSalir.Size = new System.Drawing.Size(314, 36);
            this.MenuSalir.Text = "&Salir";
            this.MenuSalir.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // buscarToolStripMenuItem
            // 
            this.buscarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuSearchById,
            this.MenuSearchByDate,
            this.MenuSearchComplex});
            this.buscarToolStripMenuItem.Name = "buscarToolStripMenuItem";
            this.buscarToolStripMenuItem.Size = new System.Drawing.Size(91, 32);
            this.buscarToolStripMenuItem.Text = "&Buscar";
            // 
            // MenuSearchById
            // 
            this.MenuSearchById.Name = "MenuSearchById";
            this.MenuSearchById.Size = new System.Drawing.Size(365, 36);
            this.MenuSearchById.Text = "Por &ID";
            this.MenuSearchById.Click += new System.EventHandler(this.MenuSearchById_Click);
            // 
            // MenuSearchByDate
            // 
            this.MenuSearchByDate.Name = "MenuSearchByDate";
            this.MenuSearchByDate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.MenuSearchByDate.Size = new System.Drawing.Size(365, 36);
            this.MenuSearchByDate.Text = "Por &fecha";
            this.MenuSearchByDate.Click += new System.EventHandler(this.MenuSearchByDate_Click);
            // 
            // MenuSearchComplex
            // 
            this.MenuSearchComplex.Name = "MenuSearchComplex";
            this.MenuSearchComplex.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.MenuSearchComplex.Size = new System.Drawing.Size(365, 36);
            this.MenuSearchComplex.Text = "&Busqueda compleja";
            this.MenuSearchComplex.Click += new System.EventHandler(this.MenuSearchComplex_Click);
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuLeeme,
            this.acercaDeLaBaseDeDatosToolStripMenuItem,
            this.compactarLaBaseDeDatosToolStripMenuItem,
            this.MenuAbout});
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(86, 32);
            this.ayudaToolStripMenuItem.Text = "A&yuda";
            // 
            // MenuLeeme
            // 
            this.MenuLeeme.Name = "MenuLeeme";
            this.MenuLeeme.Size = new System.Drawing.Size(412, 36);
            this.MenuLeeme.Text = "&Uso de KsIndexer";
            this.MenuLeeme.Click += new System.EventHandler(this.MenuReadme_Click);
            // 
            // acercaDeLaBaseDeDatosToolStripMenuItem
            // 
            this.acercaDeLaBaseDeDatosToolStripMenuItem.Name = "acercaDeLaBaseDeDatosToolStripMenuItem";
            this.acercaDeLaBaseDeDatosToolStripMenuItem.Size = new System.Drawing.Size(412, 36);
            this.acercaDeLaBaseDeDatosToolStripMenuItem.Text = "Información de la &Base de Datos";
            this.acercaDeLaBaseDeDatosToolStripMenuItem.Click += new System.EventHandler(this.acercaDeLaBaseDeDatosToolStripMenuItem_Click);
            // 
            // compactarLaBaseDeDatosToolStripMenuItem
            // 
            this.compactarLaBaseDeDatosToolStripMenuItem.Name = "compactarLaBaseDeDatosToolStripMenuItem";
            this.compactarLaBaseDeDatosToolStripMenuItem.Size = new System.Drawing.Size(412, 36);
            this.compactarLaBaseDeDatosToolStripMenuItem.Text = "&Compactar la Base de Datos";
            this.compactarLaBaseDeDatosToolStripMenuItem.Click += new System.EventHandler(this.compactarLaBaseDeDatosToolStripMenuItem_Click);
            // 
            // MenuAbout
            // 
            this.MenuAbout.Name = "MenuAbout";
            this.MenuAbout.Size = new System.Drawing.Size(412, 36);
            this.MenuAbout.Text = "&Acerca de";
            this.MenuAbout.Click += new System.EventHandler(this.MenuAbout_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelId,
            this.statusId});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1089);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 14, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1860, 32);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabelId
            // 
            this.statusLabelId.Name = "statusLabelId";
            this.statusLabelId.Size = new System.Drawing.Size(71, 25);
            this.statusLabelId.Text = "Doc ID:";
            // 
            // statusId
            // 
            this.statusId.BackColor = System.Drawing.Color.White;
            this.statusId.Name = "statusId";
            this.statusId.Size = new System.Drawing.Size(53, 25);
            this.statusId.Text = "vacío";
            // 
            // TextInDb
            // 
            this.TextInDb.AllowDrop = true;
            this.TextInDb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextInDb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextInDb.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextInDb.Location = new System.Drawing.Point(0, 0);
            this.TextInDb.Multiline = true;
            this.TextInDb.Name = "TextInDb";
            this.TextInDb.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextInDb.Size = new System.Drawing.Size(599, 320);
            this.TextInDb.TabIndex = 1;
            this.TextInDb.TextChanged += new System.EventHandler(this.TextInDb_TextChanged);
            this.TextInDb.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextInDb_DragDrop);
            this.TextInDb.DragEnter += new System.Windows.Forms.DragEventHandler(this.TextInDb_DragEnter);
            // 
            // pdfView
            // 
            this.pdfView.AllowWebBrowserDrop = false;
            this.pdfView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfView.IsWebBrowserContextMenuEnabled = false;
            this.pdfView.Location = new System.Drawing.Point(0, 0);
            this.pdfView.MinimumSize = new System.Drawing.Size(20, 20);
            this.pdfView.Name = "pdfView";
            this.pdfView.Size = new System.Drawing.Size(599, 691);
            this.pdfView.TabIndex = 2;
            this.pdfView.TabStop = false;
            this.pdfView.WebBrowserShortcutsEnabled = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(1140, 87);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(65, 28);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Título";
            // 
            // Title
            // 
            this.Title.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.Location = new System.Drawing.Point(1144, 114);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(694, 35);
            this.Title.TabIndex = 3;
            this.Title.TextChanged += new System.EventHandler(this.Title_TextChanged);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(1144, 155);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(130, 28);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "Fecha y hora";
            // 
            // DocDate
            // 
            this.DocDate.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocDate.Location = new System.Drawing.Point(1144, 181);
            this.DocDate.Name = "DocDate";
            this.DocDate.Size = new System.Drawing.Size(233, 35);
            this.DocDate.TabIndex = 4;
            this.DocDate.TextChanged += new System.EventHandler(this.DocDate_TextChanged);
            this.DocDate.Leave += new System.EventHandler(this.DocDate_Leave);
            // 
            // lblAttendats
            // 
            this.lblAttendats.AutoSize = true;
            this.lblAttendats.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttendats.Location = new System.Drawing.Point(1144, 235);
            this.lblAttendats.Name = "lblAttendats";
            this.lblAttendats.Size = new System.Drawing.Size(107, 28);
            this.lblAttendats.TabIndex = 7;
            this.lblAttendats.Text = "Asistentes";
            // 
            // Attendants
            // 
            this.Attendants.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.Attendants.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.AttName,
            this.AttCompany});
            this.Attendants.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Attendants.FullRowSelect = true;
            this.Attendants.GridLines = true;
            this.Attendants.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Attendants.HideSelection = false;
            this.Attendants.HoverSelection = true;
            this.Attendants.Location = new System.Drawing.Point(1144, 262);
            this.Attendants.MultiSelect = false;
            this.Attendants.Name = "Attendants";
            this.Attendants.Size = new System.Drawing.Size(694, 178);
            this.Attendants.TabIndex = 6;
            this.Attendants.UseCompatibleStateImageBehavior = false;
            this.Attendants.View = System.Windows.Forms.View.Details;
            this.Attendants.SelectedIndexChanged += new System.EventHandler(this.Attendants_SelectedIndexChanged);
            // 
            // AttName
            // 
            this.AttName.Text = "Nombre";
            this.AttName.Width = 250;
            // 
            // AttCompany
            // 
            this.AttCompany.Text = "Empresa";
            this.AttCompany.Width = 200;
            // 
            // lblKeywords
            // 
            this.lblKeywords.AutoSize = true;
            this.lblKeywords.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeywords.Location = new System.Drawing.Point(1144, 499);
            this.lblKeywords.Name = "lblKeywords";
            this.lblKeywords.Size = new System.Drawing.Size(143, 28);
            this.lblKeywords.TabIndex = 9;
            this.lblKeywords.Text = "Palabras clave";
            // 
            // Keywords
            // 
            this.Keywords.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Keywords.FormattingEnabled = true;
            this.Keywords.ItemHeight = 28;
            this.Keywords.Location = new System.Drawing.Point(1148, 526);
            this.Keywords.Name = "Keywords";
            this.Keywords.Size = new System.Drawing.Size(690, 172);
            this.Keywords.TabIndex = 10;
            this.Keywords.SelectedIndexChanged += new System.EventHandler(this.Keywords_SelectedIndexChanged);
            // 
            // btnAttAdd
            // 
            this.btnAttAdd.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAttAdd.Location = new System.Drawing.Point(1480, 446);
            this.btnAttAdd.Name = "btnAttAdd";
            this.btnAttAdd.Size = new System.Drawing.Size(114, 45);
            this.btnAttAdd.TabIndex = 7;
            this.btnAttAdd.Text = "Agregar";
            this.btnAttAdd.UseVisualStyleBackColor = true;
            this.btnAttAdd.Click += new System.EventHandler(this.btnAsisAdd_Click);
            // 
            // btnAttDelete
            // 
            this.btnAttDelete.Enabled = false;
            this.btnAttDelete.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAttDelete.Location = new System.Drawing.Point(1600, 446);
            this.btnAttDelete.Name = "btnAttDelete";
            this.btnAttDelete.Size = new System.Drawing.Size(114, 45);
            this.btnAttDelete.TabIndex = 8;
            this.btnAttDelete.Text = "Eliminar";
            this.btnAttDelete.UseVisualStyleBackColor = true;
            this.btnAttDelete.Click += new System.EventHandler(this.btnAsisDelete_Click);
            // 
            // btnKeyDelete
            // 
            this.btnKeyDelete.Enabled = false;
            this.btnKeyDelete.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKeyDelete.Location = new System.Drawing.Point(1604, 704);
            this.btnKeyDelete.Name = "btnKeyDelete";
            this.btnKeyDelete.Size = new System.Drawing.Size(114, 45);
            this.btnKeyDelete.TabIndex = 12;
            this.btnKeyDelete.Text = "Eliminar";
            this.btnKeyDelete.UseVisualStyleBackColor = true;
            this.btnKeyDelete.Click += new System.EventHandler(this.btnKeyDelete_Click);
            // 
            // btnKeyAdd
            // 
            this.btnKeyAdd.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKeyAdd.Location = new System.Drawing.Point(1480, 704);
            this.btnKeyAdd.Name = "btnKeyAdd";
            this.btnKeyAdd.Size = new System.Drawing.Size(114, 45);
            this.btnKeyAdd.TabIndex = 11;
            this.btnKeyAdd.Text = "Agregar";
            this.btnKeyAdd.UseVisualStyleBackColor = true;
            this.btnKeyAdd.Click += new System.EventHandler(this.btnKeyAdd_Click);
            // 
            // btnKeyEdit
            // 
            this.btnKeyEdit.Enabled = false;
            this.btnKeyEdit.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKeyEdit.Location = new System.Drawing.Point(1724, 704);
            this.btnKeyEdit.Name = "btnKeyEdit";
            this.btnKeyEdit.Size = new System.Drawing.Size(114, 45);
            this.btnKeyEdit.TabIndex = 13;
            this.btnKeyEdit.Text = "Modificar";
            this.btnKeyEdit.UseVisualStyleBackColor = true;
            this.btnKeyEdit.Click += new System.EventHandler(this.btnKeyEdit_Click);
            // 
            // btnAttEdit
            // 
            this.btnAttEdit.Enabled = false;
            this.btnAttEdit.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAttEdit.Location = new System.Drawing.Point(1724, 446);
            this.btnAttEdit.Name = "btnAttEdit";
            this.btnAttEdit.Size = new System.Drawing.Size(114, 45);
            this.btnAttEdit.TabIndex = 9;
            this.btnAttEdit.Text = "Modificar";
            this.btnAttEdit.UseVisualStyleBackColor = true;
            this.btnAttEdit.Click += new System.EventHandler(this.btnAsisEdit_Click);
            // 
            // toolStripMain
            // 
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnNew,
            this.toolBtnImport,
            this.toolBtnRegenMetadata,
            this.btnUpdatePdf,
            this.toolBtnSave,
            this.toolBtnDelete,
            this.toolBtnSearchId,
            this.toolBtnSearchDate,
            this.toolBtnSearchComplex,
            this.toolBtnExportHtml,
            this.btnPrint});
            this.toolStripMain.Location = new System.Drawing.Point(0, 36);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1860, 33);
            this.toolStripMain.TabIndex = 14;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolBtnNew
            // 
            this.toolBtnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnNew.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnNew.Image")));
            this.toolBtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnNew.Name = "toolBtnNew";
            this.toolBtnNew.Size = new System.Drawing.Size(34, 28);
            this.toolBtnNew.Text = "Nuevo";
            this.toolBtnNew.Click += new System.EventHandler(this.MenuNew_Click);
            // 
            // toolBtnImport
            // 
            this.toolBtnImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnImport.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnImport.Image")));
            this.toolBtnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnImport.Name = "toolBtnImport";
            this.toolBtnImport.Size = new System.Drawing.Size(34, 28);
            this.toolBtnImport.Text = "Importar";
            this.toolBtnImport.Click += new System.EventHandler(this.MenuImport_Click);
            // 
            // toolBtnRegenMetadata
            // 
            this.toolBtnRegenMetadata.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnRegenMetadata.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnRegenMetadata.Image")));
            this.toolBtnRegenMetadata.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnRegenMetadata.Name = "toolBtnRegenMetadata";
            this.toolBtnRegenMetadata.Size = new System.Drawing.Size(34, 28);
            this.toolBtnRegenMetadata.Text = "Regenerar metadatos";
            this.toolBtnRegenMetadata.Click += new System.EventHandler(this.MenuRegenMetadata_Click);
            // 
            // btnUpdatePdf
            // 
            this.btnUpdatePdf.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUpdatePdf.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdatePdf.Image")));
            this.btnUpdatePdf.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdatePdf.Name = "btnUpdatePdf";
            this.btnUpdatePdf.Size = new System.Drawing.Size(34, 28);
            this.btnUpdatePdf.Text = "Actualizar Pdf";
            this.btnUpdatePdf.Click += new System.EventHandler(this.btnUpdatePdf_Click);
            // 
            // toolBtnSave
            // 
            this.toolBtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnSave.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnSave.Image")));
            this.toolBtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnSave.Name = "toolBtnSave";
            this.toolBtnSave.Size = new System.Drawing.Size(34, 28);
            this.toolBtnSave.Text = "Guardar";
            this.toolBtnSave.Click += new System.EventHandler(this.MenuSave_Click);
            // 
            // toolBtnDelete
            // 
            this.toolBtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnDelete.Image")));
            this.toolBtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnDelete.Name = "toolBtnDelete";
            this.toolBtnDelete.Size = new System.Drawing.Size(34, 28);
            this.toolBtnDelete.Text = "Eliminar";
            this.toolBtnDelete.Click += new System.EventHandler(this.MenuDelete_Click);
            // 
            // toolBtnSearchId
            // 
            this.toolBtnSearchId.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnSearchId.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnSearchId.Image")));
            this.toolBtnSearchId.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnSearchId.Name = "toolBtnSearchId";
            this.toolBtnSearchId.Size = new System.Drawing.Size(34, 28);
            this.toolBtnSearchId.Text = "Buscar por Id";
            this.toolBtnSearchId.Click += new System.EventHandler(this.MenuSearchById_Click);
            // 
            // toolBtnSearchDate
            // 
            this.toolBtnSearchDate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnSearchDate.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnSearchDate.Image")));
            this.toolBtnSearchDate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnSearchDate.Name = "toolBtnSearchDate";
            this.toolBtnSearchDate.Size = new System.Drawing.Size(34, 28);
            this.toolBtnSearchDate.Text = "Buscar por fecha";
            this.toolBtnSearchDate.Click += new System.EventHandler(this.MenuSearchByDate_Click);
            // 
            // toolBtnSearchComplex
            // 
            this.toolBtnSearchComplex.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnSearchComplex.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnSearchComplex.Image")));
            this.toolBtnSearchComplex.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnSearchComplex.Name = "toolBtnSearchComplex";
            this.toolBtnSearchComplex.Size = new System.Drawing.Size(34, 28);
            this.toolBtnSearchComplex.Text = "Búsqueda compleja";
            this.toolBtnSearchComplex.Click += new System.EventHandler(this.MenuSearchComplex_Click);
            // 
            // toolBtnExportHtml
            // 
            this.toolBtnExportHtml.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnExportHtml.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnExportHtml.Image")));
            this.toolBtnExportHtml.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnExportHtml.Name = "toolBtnExportHtml";
            this.toolBtnExportHtml.Size = new System.Drawing.Size(34, 28);
            this.toolBtnExportHtml.Text = "Exportar como HTML";
            this.toolBtnExportHtml.Click += new System.EventHandler(this.menuExportHtml_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(34, 28);
            this.btnPrint.Text = "Imprimir";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnAnxView
            // 
            this.btnAnxView.Enabled = false;
            this.btnAnxView.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnxView.Location = new System.Drawing.Point(1737, 965);
            this.btnAnxView.Name = "btnAnxView";
            this.btnAnxView.Size = new System.Drawing.Size(114, 45);
            this.btnAnxView.TabIndex = 17;
            this.btnAnxView.Text = "Abrir..";
            this.btnAnxView.UseVisualStyleBackColor = true;
            this.btnAnxView.Click += new System.EventHandler(this.btnAnxView_Click);
            // 
            // btnAnxDelete
            // 
            this.btnAnxDelete.Enabled = false;
            this.btnAnxDelete.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnxDelete.Location = new System.Drawing.Point(1613, 965);
            this.btnAnxDelete.Name = "btnAnxDelete";
            this.btnAnxDelete.Size = new System.Drawing.Size(114, 45);
            this.btnAnxDelete.TabIndex = 16;
            this.btnAnxDelete.Text = "Eliminar";
            this.btnAnxDelete.UseVisualStyleBackColor = true;
            this.btnAnxDelete.Click += new System.EventHandler(this.btnAnxDelete_Click);
            // 
            // btnAnxAdd
            // 
            this.btnAnxAdd.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnxAdd.Location = new System.Drawing.Point(1493, 965);
            this.btnAnxAdd.Name = "btnAnxAdd";
            this.btnAnxAdd.Size = new System.Drawing.Size(114, 45);
            this.btnAnxAdd.TabIndex = 15;
            this.btnAnxAdd.Text = "Agregar";
            this.btnAnxAdd.UseVisualStyleBackColor = true;
            this.btnAnxAdd.Click += new System.EventHandler(this.btnAnxAdd_Click);
            // 
            // Annexes
            // 
            this.Annexes.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.Annexes.AllowDrop = true;
            this.Annexes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.AnxFileName,
            this.AnxSize});
            this.Annexes.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Annexes.FullRowSelect = true;
            this.Annexes.GridLines = true;
            this.Annexes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Annexes.HideSelection = false;
            this.Annexes.HoverSelection = true;
            this.Annexes.Location = new System.Drawing.Point(1144, 785);
            this.Annexes.MultiSelect = false;
            this.Annexes.Name = "Annexes";
            this.Annexes.Size = new System.Drawing.Size(694, 174);
            this.Annexes.TabIndex = 14;
            this.Annexes.UseCompatibleStateImageBehavior = false;
            this.Annexes.View = System.Windows.Forms.View.Details;
            this.Annexes.SelectedIndexChanged += new System.EventHandler(this.Annexes_SelectedIndexChanged);
            this.Annexes.DragDrop += new System.Windows.Forms.DragEventHandler(this.Annexes_DragDrop);
            this.Annexes.DragEnter += new System.Windows.Forms.DragEventHandler(this.Annexes_DragEnter);
            // 
            // AnxFileName
            // 
            this.AnxFileName.Text = "Nombre";
            this.AnxFileName.Width = 300;
            // 
            // AnxSize
            // 
            this.AnxSize.Text = "Tamaño";
            this.AnxSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AnxSize.Width = 100;
            // 
            // lblAnnexes
            // 
            this.lblAnnexes.AutoSize = true;
            this.lblAnnexes.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnnexes.Location = new System.Drawing.Point(1144, 758);
            this.lblAnnexes.Name = "lblAnnexes";
            this.lblAnnexes.Size = new System.Drawing.Size(177, 28);
            this.lblAnnexes.TabIndex = 18;
            this.lblAnnexes.Text = "Ficheros adjuntos";
            // 
            // splitLeftPane
            // 
            this.splitLeftPane.BackColor = System.Drawing.Color.LightGray;
            this.splitLeftPane.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitLeftPane.Location = new System.Drawing.Point(0, 69);
            this.splitLeftPane.Name = "splitLeftPane";
            this.splitLeftPane.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitLeftPane.Panel1
            // 
            this.splitLeftPane.Panel1.Controls.Add(this.TextInDb);
            // 
            // splitLeftPane.Panel2
            // 
            this.splitLeftPane.Panel2.Controls.Add(this.pdfView);
            this.splitLeftPane.Size = new System.Drawing.Size(601, 1020);
            this.splitLeftPane.SplitterDistance = 322;
            this.splitLeftPane.SplitterWidth = 5;
            this.splitLeftPane.TabIndex = 0;
            this.splitLeftPane.TabStop = false;
            // 
            // btnDateNow
            // 
            this.btnDateNow.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDateNow.Location = new System.Drawing.Point(1397, 181);
            this.btnDateNow.Name = "btnDateNow";
            this.btnDateNow.Size = new System.Drawing.Size(105, 35);
            this.btnDateNow.TabIndex = 5;
            this.btnDateNow.Text = "Ahora";
            this.btnDateNow.UseVisualStyleBackColor = true;
            this.btnDateNow.Click += new System.EventHandler(this.btnDateNow_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1860, 1121);
            this.Controls.Add(this.btnDateNow);
            this.Controls.Add(this.splitLeftPane);
            this.Controls.Add(this.btnAnxView);
            this.Controls.Add(this.btnAnxDelete);
            this.Controls.Add(this.btnAnxAdd);
            this.Controls.Add(this.Annexes);
            this.Controls.Add(this.lblAnnexes);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.btnKeyEdit);
            this.Controls.Add(this.btnAttEdit);
            this.Controls.Add(this.btnKeyDelete);
            this.Controls.Add(this.btnKeyAdd);
            this.Controls.Add(this.btnAttDelete);
            this.Controls.Add(this.btnAttAdd);
            this.Controls.Add(this.Keywords);
            this.Controls.Add(this.lblKeywords);
            this.Controls.Add(this.Attendants);
            this.Controls.Add(this.lblAttendats);
            this.Controls.Add(this.DocDate);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KsIndexer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.Main_Layout);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.splitLeftPane.Panel1.ResumeLayout(false);
            this.splitLeftPane.Panel1.PerformLayout();
            this.splitLeftPane.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitLeftPane)).EndInit();
            this.splitLeftPane.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuNew;
        private System.Windows.Forms.ToolStripMenuItem MenuImport;
        private System.Windows.Forms.ToolStripMenuItem MenuSave;
        private System.Windows.Forms.ToolStripMenuItem MenuRegenMetadata;
        private System.Windows.Forms.ToolStripMenuItem stripMenuExport;
        private System.Windows.Forms.ToolStripMenuItem MenuPrint;
        private System.Windows.Forms.ToolStripMenuItem MenuSalir;
        private System.Windows.Forms.ToolStripMenuItem buscarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuSearchById;
        private System.Windows.Forms.ToolStripMenuItem MenuSearchByDate;
        private System.Windows.Forms.ToolStripMenuItem MenuSearchComplex;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuAbout;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelId;
        private System.Windows.Forms.ToolStripStatusLabel statusId;
        private System.Windows.Forms.TextBox TextInDb;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox Title;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox DocDate;
        private System.Windows.Forms.Label lblAttendats;
        private System.Windows.Forms.ListView Attendants;
        private System.Windows.Forms.ColumnHeader AttName;
        private System.Windows.Forms.ColumnHeader AttCompany;
        private System.Windows.Forms.Label lblKeywords;
        private System.Windows.Forms.ListBox Keywords;
        private System.Windows.Forms.WebBrowser pdfView;
        private System.Windows.Forms.Button btnAttAdd;
        private System.Windows.Forms.Button btnAttDelete;
        private System.Windows.Forms.Button btnKeyDelete;
        private System.Windows.Forms.Button btnKeyAdd;
        private System.Windows.Forms.Button btnKeyEdit;
        private System.Windows.Forms.Button btnAttEdit;
        private System.Windows.Forms.ToolStripMenuItem menuExportHtml;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolBtnNew;
        private System.Windows.Forms.ToolStripButton toolBtnImport;
        private System.Windows.Forms.ToolStripButton toolBtnSave;
        private System.Windows.Forms.ToolStripButton toolBtnRegenMetadata;
        private System.Windows.Forms.ToolStripButton toolBtnSearchId;
        private System.Windows.Forms.ToolStripButton toolBtnSearchDate;
        private System.Windows.Forms.ToolStripButton toolBtnSearchComplex;
        private System.Windows.Forms.ToolStripButton toolBtnExportHtml;
        private System.Windows.Forms.ToolStripMenuItem MenuDelete;
        private System.Windows.Forms.ToolStripButton toolBtnDelete;
        private System.Windows.Forms.ToolStripMenuItem MenuLeeme;
        private System.Windows.Forms.ToolStripMenuItem menuExportTxt;
        private System.Windows.Forms.Button btnAnxView;
        private System.Windows.Forms.Button btnAnxDelete;
        private System.Windows.Forms.Button btnAnxAdd;
        private System.Windows.Forms.ListView Annexes;
        private System.Windows.Forms.ColumnHeader AnxFileName;
        private System.Windows.Forms.ColumnHeader AnxSize;
        private System.Windows.Forms.Label lblAnnexes;
        private System.Windows.Forms.ToolStripMenuItem MenuUpdatePdf;
        private System.Windows.Forms.ToolStripMenuItem acercaDeLaBaseDeDatosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compactarLaBaseDeDatosToolStripMenuItem;
        private SplitContainer splitLeftPane;
        private ToolStripButton btnUpdatePdf;
        private ToolStripButton btnPrint;
        private Button btnDateNow;
    }
}


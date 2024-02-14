namespace KsIndexerNET.dialogs
{
    partial class DlgOpen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgOpen));
            this.ctxMenuNodes = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxMenuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tvIcons = new System.Windows.Forms.ImageList(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvNodes = new System.Windows.Forms.TreeView();
            this.lvDocs = new System.Windows.Forms.ListView();
            this.colId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblPath = new System.Windows.Forms.Label();
            this.ctxMenuNodes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctxMenuNodes
            // 
            this.ctxMenuNodes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxMenuNew,
            this.ctxMenuDelete});
            this.ctxMenuNodes.Name = "ctxMenuNodes";
            this.ctxMenuNodes.Size = new System.Drawing.Size(118, 48);
            // 
            // ctxMenuNew
            // 
            this.ctxMenuNew.Name = "ctxMenuNew";
            this.ctxMenuNew.Size = new System.Drawing.Size(117, 22);
            this.ctxMenuNew.Text = "Nuevo";
            this.ctxMenuNew.Click += new System.EventHandler(this.ctxMenuNew_Click);
            // 
            // ctxMenuDelete
            // 
            this.ctxMenuDelete.Name = "ctxMenuDelete";
            this.ctxMenuDelete.Size = new System.Drawing.Size(117, 22);
            this.ctxMenuDelete.Text = "Eliminar";
            this.ctxMenuDelete.Click += new System.EventHandler(this.ctxMenuDelete_Click);
            // 
            // tvIcons
            // 
            this.tvIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("tvIcons.ImageStream")));
            this.tvIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.tvIcons.Images.SetKeyName(0, "FolderSelected.png");
            this.tvIcons.Images.SetKeyName(1, "Folder.png");
            this.tvIcons.Images.SetKeyName(2, "ElementSelected.png");
            this.tvIcons.Images.SetKeyName(3, "Element.png");
            // 
            // btnOk
            // 
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(628, 479);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 35);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Seleccionar";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(748, 479);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvNodes);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvDocs);
            this.splitContainer1.Size = new System.Drawing.Size(861, 472);
            this.splitContainer1.SplitterDistance = 287;
            this.splitContainer1.TabIndex = 5;
            // 
            // tvNodes
            // 
            this.tvNodes.AllowDrop = true;
            this.tvNodes.ContextMenuStrip = this.ctxMenuNodes;
            this.tvNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvNodes.ImageIndex = 0;
            this.tvNodes.ImageList = this.tvIcons;
            this.tvNodes.LabelEdit = true;
            this.tvNodes.Location = new System.Drawing.Point(0, 0);
            this.tvNodes.Margin = new System.Windows.Forms.Padding(4);
            this.tvNodes.Name = "tvNodes";
            this.tvNodes.SelectedImageIndex = 0;
            this.tvNodes.ShowPlusMinus = false;
            this.tvNodes.ShowRootLines = false;
            this.tvNodes.Size = new System.Drawing.Size(287, 472);
            this.tvNodes.TabIndex = 1;
            this.tvNodes.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvNodes_AfterLabelEdit);
            this.tvNodes.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvNodes_ItemDrag);
            this.tvNodes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvNodes_AfterSelect);
            this.tvNodes.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvNodes_DragDrop);
            this.tvNodes.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvNodes_DragEnter);
            this.tvNodes.DragOver += new System.Windows.Forms.DragEventHandler(this.tvNodes_DragOver);
            this.tvNodes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvNodes_MouseDown);
            // 
            // lvDocs
            // 
            this.lvDocs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colDate,
            this.colTitle});
            this.lvDocs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDocs.FullRowSelect = true;
            this.lvDocs.HideSelection = false;
            this.lvDocs.Location = new System.Drawing.Point(0, 0);
            this.lvDocs.Name = "lvDocs";
            this.lvDocs.Size = new System.Drawing.Size(570, 472);
            this.lvDocs.TabIndex = 0;
            this.lvDocs.UseCompatibleStateImageBehavior = false;
            this.lvDocs.View = System.Windows.Forms.View.Details;
            this.lvDocs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvDocs_ColumnClick);
            this.lvDocs.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvDocs_ItemDrag);
            this.lvDocs.SelectedIndexChanged += new System.EventHandler(this.lvDocs_SelectedIndexChanged);
            // 
            // colId
            // 
            this.colId.Text = "Id";
            this.colId.Width = 0;
            // 
            // colDate
            // 
            this.colDate.Text = "Fecha";
            this.colDate.Width = 150;
            // 
            // colTitle
            // 
            this.colTitle.Text = "Titulo";
            this.colTitle.Width = 400;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.BackColor = System.Drawing.SystemColors.Control;
            this.lblPath.Location = new System.Drawing.Point(12, 487);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(14, 18);
            this.lblPath.TabIndex = 6;
            this.lblPath.Text = "\\";
            // 
            // DlgOpen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 524);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DlgOpen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Carpeta";
            this.Load += new System.EventHandler(this.DlgSearchFolder_Load);
            this.ctxMenuNodes.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ImageList tvIcons;
        private System.Windows.Forms.ContextMenuStrip ctxMenuNodes;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuNew;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuDelete;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvNodes;
        private System.Windows.Forms.ListView lvDocs;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colTitle;
        private System.Windows.Forms.Label lblPath;
    }
}
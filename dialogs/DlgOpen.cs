using KsIndexerNET.Db;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Xml.Linq;

namespace KsIndexerNET.dialogs
{
    public partial class DlgOpen : Form
    {
        // Enum de Modo de seleccion: carpeta o documento
        public enum SelectMode { Folder, Document };

        // Auxiliar para indicar que hay que ordenar la lista de nodos cuando se pueda
        private bool sortPending = false;
        // Formulario principal del que dependemos
        private Main mainForm;
        // INode que seleccionamos inicialmente
        private string selectedInodeId = "1";
        // Mode de seleccion
        private SelectMode selectionMode = SelectMode.Folder;

        public DlgOpen(Main main, string startInodeId, SelectMode selMode)
        {
            InitializeComponent();
            this.mainForm = main;
            this.selectedInodeId = startInodeId;
            this.selectionMode = selMode;
            this.lvDocs.Enabled = (selMode == SelectMode.Document);
            if (selMode == SelectMode.Document)
            {
                this.Text = Texts.SELECT_DOCUMENT;
            }
            else
            {
                this.Text = Texts.SELECT_FOLDER;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string GetSelectedInodeId()
        {
            TreeNode node = tvNodes.SelectedNode;
            // Si no hay ninguno seleccionado, tomamos el raiz
            if (node == null)
                return "1";
            return node.Name;
        }

        public string GetSelectedInodePath()
        {
            TreeNode node = tvNodes.SelectedNode;
            // Si no hay ninguno seleccionado, cadena vacia
            if (node == null)
                return "";
            return node.FullPath;
        }

        public int GetSelectedDocId()
        {
            if (lvDocs.SelectedItems.Count == 0)
                return 0;
            return int.Parse(lvDocs.SelectedItems[0].SubItems[0].Text);
        }

        private void DlgSearchFolder_Load(object sender, EventArgs e)
        {
            // Traducir el formulario
            LangUtils.TranslateForm(this);
            // Metodo comun para cargar los nodos
            InodeUtils.LoadInodes(tvNodes);
            // Seleccionar el nodo inicial
            TreeNode node = tvNodes.Nodes.Find(selectedInodeId, true).First();
            if (node != null)
            {
                tvNodes.SelectedNode = node;
            }
        }

        /**
         * Evento AfterLabelEdit del TreeView. Se dispra cuando el usuario ha terminado de editar
         * el nombre de un nodo de manera interactiva. Se encarga de comprobar que el nombre
         * no esté vacío y que no haya otro nodo con el mismo nombre. Si todo es correcto,
         * cambia el nombre del nodo y lo actualiza en la Base de Datos.
         * Por último, actualiza el flag sortPending para que los hijos del nodo padre se ordenen
         * tan pronto como sea posible (ver MouseDown). No hacemos la ordenación de manera inmediata
         * porque se producen efectos visuales indeseados.
         * El flag se pasa por referencia, y debe ser un atributo del Form.
         */
        private void tvNodes_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // Compruebo que la etiqueta no este vacia
            if (e.Label == null || e.Label.Length == 0)
            {
                e.CancelEdit = true;
                return;
            }
            TreeNode parent = e.Node.Parent;
            // Si es el root, no permitimos cambiar el nombre
            if (parent == null)
            {
                e.CancelEdit = true;
                return;
            }
            // Evitar que el nombre contenga la barra inversa
            if (e.Label.Contains(@"\"))
            {
                e.CancelEdit = true;
                return;
            }
            // Compruebo que no haya otro nodo con el mismo nombre
            if (InodeUtils.ChildExists(parent, e.Label))
            {
                e.CancelEdit = true;
                return;
            }
            // Actualizar nodo en la Base de Datos (Id, Parent, Name)
            if (!Inode.Update(e.Node.Name, parent.Name, e.Label))
            {
                e.CancelEdit = true;
                return;
            }
            // Si llego aqui, termino la edicion aceptando el nuevo nombre
            e.Node.EndEdit(false);
            // Esto hay que hacerlo, aunque se supone que lo deberia hacer solo
            e.Node.Text = e.Label;
            // Actualizar el flag para ordenar los hijos del nodo padre
            sortPending = true;
            // Actualizar el path del documento en el label de abajo
            lblPath.Text = e.Node.FullPath;
        }

        private void tvNodes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Debe implementarse en cada formulario que use el arbol
            TreeNode node = tvNodes.SelectedNode;
            if (node == null)
            {
                ctxMenuDelete.Enabled = ctxMenuNew.Enabled = false;
                lblPath.Text = "";
            }
            else
            {
                ctxMenuNew.Enabled = true;
                TreeNode parent = node.Parent;
                // Cargar los documentos en el nodo seleccionado
                InodeUtils.LoadDocuments(lvDocs, Int32.Parse(node.Name));
                lvDocs.SelectedItems.Clear();
                // El root no se puede borrar, ni tampoco si tiene hijos
                ctxMenuDelete.Enabled = parent != null && node.Nodes.Count == 0 && lvDocs.Items.Count == 0;
                lblPath.Text = node.FullPath;
            }
            EnableControls();
        }

        /**
         * Evento MouseDown del TreeView. Se encarga de seleccionar el nodo sobre el que se hace mouse down.
         * Si hay un sort pendiente, previamente ordena los hermanos del nodo seleccionado y actualiza el flag.
         * El flag se pasa por referencia, y debe ser un atributo del Form.
         */
        private void tvNodes_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = tvNodes.GetNodeAt(e.X, e.Y);
            if (node == null)
                return;
            if (sortPending)
            {
                if (tvNodes.SelectedNode != null && tvNodes.SelectedNode.Parent != null)
                    InodeUtils.SortChildren(tvNodes.SelectedNode.Parent);
                sortPending = false;
            }
            tvNodes.SelectedNode = node;
        }

        /**
         * Evento ItemDrag del TreeView. Se encarga de iniciar el drag del nodo seleccionado, guardando
         * el nodo (Item) y el efecto (Move) en el objeto DragDrop.
         * Por el momento solo permitimos mover nodos, manteniendo pulsado el boton izquierdo.
         * No permitimos mover el root.
         */
        private void tvNodes_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TreeNode node = (TreeNode)e.Item;
                // No permitir mover el root
                if (node.Parent == null)
                    return;
                // Drag con boton izquierdo es mover el nodo.
                // Por el momento no soportamos copiar.
                tvNodes.DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        /**
         * Evento DragEnter del TreeView. Se dispara cuando arrastramos el cursor por primera vez
         * sobre el destino del dragdrop, para que visualmente el cambio de cursor muestre que se puede soltar aquí.
         * Se limita a aceptar el efecto que se nos ofrece, en nuestro caso, mover.
         */
        private void tvNodes_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        /**
         * Evento DragOver del TreeView. Se dispara después del DragEnter, cada vez que movemos el cursor
         * sobre el destino del dragdrop.
         * Lo que hacemos es seleccionar el nodo sobre el que se arrastra, para que visualmente
         * tengamos feedback de lo que va a ocurrir.
         * NOTA: El magnífico diseño de Windows Forms hace que en los eventos de drag and drop, las
         * coordenadas se tienen que convertir de pantalla a cliente, cosa que no ocurre, por ejemplo,
         * en el Mouse Down.
         */
        private void tvNodes_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = tvNodes.PointToClient(new Point(e.X, e.Y));
            // Seleccionar el nodo sobre el que se arrastra
            TreeNode targetNode = tvNodes.GetNodeAt(targetPoint);
            if (targetNode == null)
                return;
            tvNodes.SelectedNode = targetNode;
        }

        /**
         * Evento DragDrop del TreeView. Se dispara cuando soltamos lo que venimos arrastrando sobre el destino.
         * Nos pueden arrastrar otro nood del TreeView, o un documento del ListView.
         * Lo que hacemos es mover el objeto arrastrado al destino, y actualizar la Base de Datos.
         */
        private void tvNodes_DragDrop(object sender, DragEventArgs e)
        {
            // Coordenadas donde nos han soltado
            Point targetPoint = tvNodes.PointToClient(new Point(e.X, e.Y));
            // Nodo destino
            TreeNode targetNode = tvNodes.GetNodeAt(targetPoint);
            if (targetNode == null)
                return;
            // Diferenciar si nos estan ararstrando un nodo o un documento
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                // Mover un documento
                ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                int.TryParse(draggedItem.SubItems[0].Text, out int docid);
                if (docid == 0)
                    return;
                // Actualizar en la Base de Datos (Id, InodeId)
                if (!Document.UpdateInode(docid, int.Parse(targetNode.Name)))
                    return;
                mainForm.SetDocumentPath(docid, targetNode.FullPath);
                // Forzar recarga del nodo destino
                tvNodes.SelectedNode = null;
                tvNodes.SelectedNode = targetNode;
                return;
            }
            else if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                // Obtener nodo origen desde los datos del DragDrop
                TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                // Si es el mismo o el padre del mismo, no hago nada
                if (draggedNode.Equals(targetNode) || draggedNode.Parent.Equals(targetNode))
                    return;
                // Actualizar en la Base de Datos (Id, Parent, Name)
                if (!Inode.Update(draggedNode.Name, targetNode.Name, draggedNode.Text))
                    return;
                draggedNode.Remove();
                targetNode.Nodes.Add(draggedNode);
                InodeUtils.SortChildren(targetNode);
                targetNode.Expand();
            }
        }

        /** 
         * Crear un nuevo nodo como hijo del actualmente seleccionado. Simulando el comportamiento
         * del Explorador de Windoes, le pronemos el nombre "new folder" y lo ponemos en edicion para que el
         * usuario cambie el nombre a su gusto.
         */
        private void ctxMenuNew_Click(object sender, EventArgs e)
        {
            TreeNode parent = tvNodes.SelectedNode;
            if (parent == null)
                return;
            // Si ya tenia un hijo nuevo, no dejo seguir
            if (InodeUtils.ChildExists(parent, InodeUtils.NEW_FOLDER_NAME))
                return;
            // Insertar en la Base de Datos y obtener el Id
            int id = Inode.Insert(parent.Name, InodeUtils.NEW_FOLDER_NAME);
            if (id == 0)
                return;
            // Agregar el nodo al TreeView
            TreeNode node = parent.Nodes.Add(id.ToString(), InodeUtils.NEW_FOLDER_NAME, InodeUtils.IMG_FOLDER, InodeUtils.IMG_FOLDER_SEL);
            InodeUtils.SortChildren(parent);
            tvNodes.SelectedNode = node;
            node.BeginEdit();
        }

        /**
         * Eliminar el nodo seleccionado. Si no hay ninguno seleccionado, o es el root, no hago nada.
         * Si tiene hijos, no se puede eliminar.
         */
        private void ctxMenuDelete_Click(object sender, EventArgs e)
        {
            TreeNode node = tvNodes.SelectedNode;
            if (node == null || node.Parent == null || node.Nodes.Count > 0 || lvDocs.Items.Count > 0)
                return;
            // Eliminar de la Base de Datos
            if (!Inode.Delete(node.Name))
                return;
            node.Remove();
        }

        private void lvDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void lvDocs_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.lvDocs.ListViewItemSorter = new ListViewItemComparer(e.Column);
        }

        private void EnableControls()
        {
            if (selectionMode == SelectMode.Folder)
            {
                btnOk.Enabled = tvNodes.SelectedNode != null;
            }
            else
            {
                btnOk.Enabled = lvDocs.SelectedItems.Count > 0;
            }
        }

        private void lvDocs_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && lvDocs.SelectedItems.Count > 0)
            {
                lvDocs.DoDragDrop(lvDocs.SelectedItems[0], DragDropEffects.Move);
            }
        }
    }

    // Ordenacion manual por culumna del ListView.
    class ListViewItemComparer : IComparer
    {
        private int col;
        public ListViewItemComparer()
        {
            col = 1;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            // Como en el ListView no se puede diferenciar el tipo de dato del item
            // tenemos que hacerlo hardcoded. Es una chapuza para salir del paso.
            string txt1 = ((ListViewItem)x).SubItems[col].Text;
            string txt2 = ((ListViewItem)y).SubItems[col].Text;
            if (col == 1)
            {
                // Otra chapucilla, si el texto contiene ":" tiene hora, si no es fecha solo
                DateTime dt1, dt2;
                if (txt1.Contains(":"))
                {
                    dt1 = LangUtils.ParseDateTime(txt1);
                    dt2 = LangUtils.ParseDateTime(txt2);
                }
                else
                {
                    dt1 = LangUtils.ParseDate(txt1);
                    dt2 = LangUtils.ParseDate(txt2);
                }
                return DateTime.Compare(dt1, dt2);
            }
            else
            {
                return String.Compare(txt1, txt2);
            }
        }
    }
}

using KsIndexerNET.Db;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KsIndexerNET.dialogs
{
    // Clase para eventos comunes en Forms en que aparezca el arbol de Inodes
    internal class InodeUtils
    {
        public const int IMG_FOLDER = 0;
        public const int IMG_FOLDER_SEL = 1;
        public const int IMG_FILE = 2;
        public const int IMG_FILE_SEL = 3;
        public const string NEW_FOLDER_NAME = "new folder";

        /**
         * Carga los nodos de la tabla Inodes en el TreeView
         * que se le pasa como argumento. Se asume que el icono
         * de carpeta es el 0 y el de carpeta selecionada el 1. En el Form padre
         * se debe incluir un ImageList con los iconos correspondientes.
         * 
         * En cada nodo guardamos los siguientes datos:
         * - Name: Id del nodo, recibido de la tabla Inodes (Id).
         * - Text: Nombre del nodo, recibido de la tabla Inodes (Name).
         * 
         * No nos hace falta guardar de manera explícita el Id del nodo padre, ya que
         * el propio TreeView se encarga de ello. 
         */
        public static void LoadInodes(TreeView tvNodes)
        {
            List<string[]> inodes = Inode.SelectAll();
            bool gotRoot = false;
            // Agregamos los nodos
            foreach (string[] inode in inodes)
            {
                TreeNode child = new TreeNode();
                child.ImageIndex = IMG_FOLDER;
                child.SelectedImageIndex = IMG_FOLDER_SEL;
                child.Name = inode[1];          // Id
                child.Text = inode[0];          // Nombre
                int id = int.Parse(inode[1]);
                if (inode[2] == "0") // Es el root
                {
                    if (!gotRoot)   // Solo aceptamos un root, por ahora
                    {
                        child.Text = @"\";
                        tvNodes.Nodes.Add(child);
                        gotRoot = true;
                    }
                }
                else
                {
                    TreeNode[] parents = tvNodes.Nodes.Find(inode[2], true);
                    if (parents.Length > 0)
                    {
                        parents[0].Nodes.Add(child);
                    }
                }
            }
        }

        /**
         * Cargar en el ListView que se le pasa como argumento los documentos que hay en una carpeta,
         * identificada por su INodeId.
         */
        public static void LoadDocuments(ListView lvDocs, int nodeId)
        {
            lvDocs.Items.Clear();
            List<string[]> docs = Document.GetByInode(nodeId);
            foreach (string[] item in docs)
            {
                ListViewItem lItem = new ListViewItem(item);
                lvDocs.Items.Add(lItem);
            }
        }

        //
        // Utilidades
        //
        public static bool ChildExists(TreeNode parent, string name)
        {
            foreach (TreeNode child in parent.Nodes)
            {
                if (child.Text == name)
                    return true;
            }
            return false;
        }

        public static void SortChildren(TreeNode parent)
        {
            // Para ordenar debe haber al menos dos nodos
            if (parent.Nodes.Count < 2)
                return;
            TreeNode[] aux = new TreeNode[parent.Nodes.Count];
            parent.Nodes.CopyTo(aux, 0);
            Array.Sort(aux, (x, y) => string.Compare(x.Text, y.Text));
            parent.Nodes.Clear();
            parent.Nodes.AddRange(aux);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsIndexerNET.Db
{
    /**
     * Encapsula los metodos de acceso a la tabla INodes, en la que estan almacenadas las entradas de
     * las carpetas y subcarpetas en las que archivamos los documentos.
     * 
     * La tabla contiene las siguientes columnas:
     * - id: Identificador unico del INode.
     * - parent: Identificador del INode padre. Será 0 si es el nodo raiz (sólo hay uno).
     * - name: Nombre del INode para visualizacion.
     *     
     * El la tabla Documentos, la columna InodeId indica en qué carpeta está el documento.
     */
    internal class Inode
    {
        public const string SQL_IN_SELECT_ALL = "SELECT name, id, parent FROM inodes order by parent, name";
        public const string SQL_IN_INSERT = "INSERT INTO inodes (parent, name) VALUES (@parent, @name)";
        public const string SQL_IN_DELETE = "DELETE FROM inodes WHERE id = @id";
        public const string SQL_IN_UPDATE = "UPDATE inodes SET parent = @parent, name = @name WHERE id = @id";

        public static List<string[]> SelectAll()
        {
            List<string[]> result = new List<string[]>();
            Database db = Database.GetInstance();
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_IN_SELECT_ALL);
            while (rdr.Read())
            {
                // Devuelvo [name, id, parent]
                result.Add(new string[] { rdr.GetString(0), rdr.GetInt32(1).ToString(), rdr.GetInt32(2).ToString() });
            }
            rdr.Close();
            return result;
        }

        public static int Insert(string sparent, string name)
        {
            Database db = Database.GetInstance();
            if (!Int32.TryParse(sparent, out int parent))
                return 0;
            Dictionary<string, object> prms = new Dictionary<string, object>
            {
                { "@parent", parent },
                { "@name", name }
            };
            if (db.ExecuteNonQuery(SQL_IN_INSERT, prms) != 1)
                return 0;
            SQLiteDataReader rdr = db.ExecuteQuery(Database.SQL_GET_LAST_ID);
            rdr.Read();
            return rdr.GetInt32(0);
        }

        public static bool Delete(string id)
        {
            Database db = Database.GetInstance();
            if (!Int32.TryParse(id, out int iid))
                return false;
            Dictionary<string, object> prms = new Dictionary<string, object>
            {
                { "@id", iid }
            };
            return db.ExecuteNonQuery(SQL_IN_DELETE, prms) == 1;
        }

        public static bool Update(string id, string sparent, string name)
        {
            Database db = Database.GetInstance();
            if (!Int32.TryParse(id, out int iid) || !Int32.TryParse(sparent, out int parent))
                return false;
            Dictionary<string, object> prms = new Dictionary<string, object>
            {
                { "@id", iid },
                { "@parent", parent },
                { "@name", name }
            };
            return db.ExecuteNonQuery(SQL_IN_UPDATE, prms) == 1;
        }

        public static string GetPathFromId(int inodeId)
        {
            Database db = Database.GetInstance();
            List<string> nodes = new List<string>();
            while (inodeId != 0)
            {
                SQLiteDataReader rdr = db.ExecuteQuery("SELECT name, parent FROM inodes WHERE id = " + inodeId);
                if(!rdr.Read())
                {
                    rdr.Close();
                    return "";
                }
                inodeId = rdr.GetInt32(1);
                // Solo si el padre no es el root
                if (inodeId > 0)
                    nodes.Add(rdr.GetString(0));
                rdr.Close();
            }
            string path = @"\";
            // Leemos al reves
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                path += @"\" + nodes[i];
            }
            return path;
        }

        public static int GetIdFromPath(string path)
        {
            // Quitamos posibles barras al principio y al final
            path = path.Trim('\\');
            // Si solo contiene barras, es el root
            if (path.Length == 0)
                return 1;
            string[] nodes = path.Split('\\');
            Database db = Database.GetInstance();
            // Empezamos con el root
            int inodeId = 1;
            for (int i = 0; i < nodes.Length; i++)
            {
                // Saltar las dos barras del root, si nos las pasan
                //if (nodes[i].Length == 0)
                //    continue;
                //SQLiteDataReader rdr = db.ExecuteQuery("SELECT id FROM inodes WHERE name = '" + nodes[i] + "' AND parent = " + inodeId);
                SQLiteDataReader rdr = db.ExecuteQuery("SELECT id FROM inodes WHERE name LIKE '%" + nodes[i] + "%' AND parent = " + inodeId);
                if (!rdr.Read())
                {
                    rdr.Close();
                    return 0;
                }
                inodeId = rdr.GetInt32(0);
                rdr.Close();
            }
            return inodeId;
        }
    }
}

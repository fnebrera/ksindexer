using KsIndexerNET;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KsIndexerNET.Db
{
    partial class Database
    {
        //
        // Funciones de actualizacion de la base de datos
        //

        private static void UpgradeDatabase_10_11()
        {
            string sql;
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            try
            {
                // 
                // Tabla Documents
                //
                // Agregar campo TitleNorm en la tabla de documentos, si no existe ya
                if (!ColumnExists("Documentos", "TitleNorm"))
                {
                    sql = "ALTER TABLE Documents ADD \"TitleNorm\" TEXT";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                    // Actualizar el campo TitleNorm con los valores actuales de Title normalizados.
                    sql = "SELECT Id, Title FROM Documents";
                    cmd = new SQLiteCommand(sql, dbConn);
                    List<string[]> docs = new List<string[]>();
                    string[] doc;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        doc = new string[2];
                        doc[0] = reader.GetInt32(0).ToString();
                        doc[1] = reader.GetString(1);
                        docs.Add(doc);
                    }
                    reader.Close();
                    foreach (string[] d in docs)
                    {
                        string titleNorm = FileUtils.NormalizeString(d[1]);
                        sql = "UPDATE Documents SET TitleNorm = @titlenorm WHERE Id = @id";
                        cmd = new SQLiteCommand(sql, dbConn);
                        cmd.Parameters.AddWithValue("@id", Int32.Parse(d[0]));
                        cmd.Parameters.AddWithValue("@titlenorm", titleNorm);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Messages.ShowError(Texts.ERROR_UPDATE_DB + ": " + e.Message);
                Environment.Exit(1);
            }
        }

        private static void UpgradeDatabase_11_12()
        {
            string sql;
            SQLiteCommand cmd;
            try
            {
                //
                // En version 1.2 se agrega la tabla de anexos
                //
                if (!TableExists("Doc_annexes"))
                {
                    // Crear la tabla Doc_annexes
                    sql = "CREATE TABLE Doc_annexes (DocId INTEGER, FileName TEXT, Content BLOB, Size INTEGER)";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                    // Crear el indice
                    sql = "CREATE UNIQUE INDEX Doc_annexes_PK ON Doc_annexes(DocId, FileName)";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Messages.ShowError(Texts.ERROR_UPDATE_DB + ":\n" + ex.Message);
                Environment.Exit(1);
            }
        }

        private static void UpgradeDatabase_12_13()
        {
            string sql;
            SQLiteCommand cmd;
            try
            {
                //
                // En version 1.3 se agrega el indice Doc_keyword_ByKey
                //
                if (!IndexExists("Doc_keywords", "Doc_keywords_ByKey"))
                {
                    // Crear indice
                    sql = "CREATE INDEX Doc_keywords_ByKey on Doc_keywords (Keyword)";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Messages.ShowError(Texts.ERROR_UPDATE_DB + ":\n" + ex.Message);
                Environment.Exit(1);
            }
        }

        private static void UpgradeDatabase_13_14()
        {
            string sql;
            SQLiteCommand cmd;
            try
            {
                //
                // Se agrega la tabla INodes
                //
                if (!TableExists("INodes"))
                {
                    // Crear la tabla
                    sql = "CREATE TABLE INodes (Id INTEGER PRIMARY KEY AUTOINCREMENT, Parent INTEGER, Name TEXT)";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                    // Crear el nodo raiz
                    sql = "INSERT INTO INodes (Parent, Name) VALUES (0, 'root')";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                }
                //
                // Se agrega el campo InodeId en la tabla de documentos
                //
                if (!ColumnExists("Documents", "INodeId"))
                {
                    // Crear columna
                    sql = "ALTER TABLE Documents ADD \"INodeId\" INTEGER";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                    // Poner como padre el nodo 1 a todos los documentos
                    sql = "UPDATE Documents SET INodeId = 1";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Messages.ShowError(Texts.ERROR_UPDATE_DB + ":\n" + ex.Message);
                Environment.Exit(1);
            }
        }

        private static bool TableExists(string tableName)
        {
            bool exists = false;
            SQLiteCommand cmd = new SQLiteCommand("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='" + tableName + "'", dbConn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader.GetInt32(0) > 0)
                {
                    exists = true;
                }
            }
            reader.Close();
            return exists;
        }

        private static bool IndexExists(string tableName, string indexName)
        {
            bool exists = false;
            SQLiteCommand cmd = new SQLiteCommand("SELECT count(*) FROM sqlite_master WHERE type='index' AND tbl_name='" + 
                tableName + "' AND name='" + indexName + "'", dbConn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader.GetInt32(0) > 0)
                {
                    exists = true;
                }
            }
            reader.Close();
            return exists;
        }

        private static bool ColumnExists(string tableName, string columnName)
        {
            bool exists = false;
            SQLiteCommand cmd = new SQLiteCommand("SELECT count(*) FROM pragma_table_info('" + tableName + "') WHERE name='" + columnName + "'", dbConn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader.GetInt32(0) > 0)
                {
                    exists = true;
                }
            }
            reader.Close();
            return exists;
        }

        private static void UpdateDbVersion(string version)
        {
            string sql = "UPDATE DbVersion set version='" + version + "'";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConn);
            cmd.ExecuteNonQuery();
        }

        public static void VacuumDb()
        {
            string sql = "VACUUM";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConn);
            cmd.ExecuteNonQuery();
        }
    }
}

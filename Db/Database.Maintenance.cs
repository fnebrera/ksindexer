using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ksindexer.Db
{
    partial class Database
    {
        //
        // Funciones de actualizacion de la base de datos
        //

        private void UpgradeDatabase_11_12()
        {
            Messages.ShowInfo("Se va proceder a actualizar la base de datos a la versión " + dbVersion + "\nPulse OK para continuar");
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
                Messages.ShowError("Error al actualizar la base de datos:\n" + ex.Message);
                Environment.Exit(1);
            }
        }

        // Obsoleta. La mantengo por referencia
        private static void UpgradeDatabaseFrom_10(bool tableExists, bool recordExists)
        {
            Messages.ShowInfo("Se va a actualizar la base de datos a la versión " + dbVersion + "\nEste proceso puede requerir algún tiempo");
            string sql;
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            try
            {
                //
                // Tabla DbVersion
                //
                if (!tableExists)
                {
                    // Crear la tabla DbVersion
                    sql = "CREATE TABLE DbVersion (version TEXT)";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                }
                // Insertra o actualizar el registro de version
                if (recordExists)
                {
                    // Actualizar el registro con la version actual
                    sql = "UPDATE DbVersion set version= '" + dbVersion + "'";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    // Insertar el registro con la version actual
                    sql = "INSERT INTO DbVersion (version) VALUES ('" + dbVersion + "')";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                }
                // 
                // Tabla Documents
                //
                // Agregar campo TitleNorm en la tabla de documentos, si no existe ya
                if (!ColumnExists("Documentos", "TitleNorm"))
                {
                    sql = "ALTER TABLE Documents ADD \"TitleNorm\" TEXT";
                    cmd = new SQLiteCommand(sql, dbConn);
                    cmd.ExecuteNonQuery();
                }
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
                    cmd.Parameters.AddWithValue("@titlenorm", d[1]);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al actualizar la base de datos: " + e.Message);
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Net;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Runtime.CompilerServices;

namespace ksindexer.Db
{
    public partial class Database
    {
        // V1.1 FNG 2024/01/26 Se agrega la constante dbVersion para facilitar upgrades automaticos, si son necesarios.
        // Se ha creado una tabla DbVersion con un solo campo, version, que se actualiza en cada upgrade. Al iniciar la
        // aplicación se lee la versión de la BD y se compara con la versión actual. Si es menor, se ejecutan los scripts
        // de upgrade necesarios.
        private const string dbVersion = "1.2";
        // Formato del connString en SQLite: "Data Source=.\\KsIndexer.db3;Version=3;";
        // En desarrollo usamos la BD local
        private const string dbDevelop = ".\\KsIndexer.db3";
        // Una vez instalado, está en un directorio fijo, para evitar los problemas de
        // acceso en actualización en la carpeta Program Files
        private const string dbRelease = "C:\\KsIndexer\\KsIndexer.db3";
        private static SQLiteConnection dbConn;
        private static Database _instance = null;

        private Database()
        {
            try
            {
                string connString = "";
                string oldVersion;
                if (File.Exists(dbDevelop))
                {
                    connString = "Data Source=" + dbDevelop + ";Version=3;";
                }
                else if (File.Exists(dbRelease))
                {
                    connString = "Data Source=" + dbRelease + ";Version=3;";
                }
                else
                {
                    MessageBox.Show("No se encuentra la base de datos. Verifique instalación");
                    Environment.Exit(1);
                }
                dbConn = new SQLiteConnection(connString);
                dbConn.Open();
                SQLiteCommand cmd;
                SQLiteDataReader reader;
                // Verificamos que exista la tabla DbVersion. Si no existe, es un error de instalación
                if (!TableExists("DbVersion"))
                {
                    MessageBox.Show("Error de instalación. No se encuentra la tabla DbVersion\nElimine la BD anterior y reinstale");
                    Environment.Exit(1);
                }
                // Si existe, leer la version
                cmd = new SQLiteCommand("SELECT version FROM DbVersion", dbConn);
                reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    MessageBox.Show("Error de instalación. La tabla DbVersion no tiene un contenido válido\nElimine la BD anterior y reinstale");
                    Environment.Exit(1);
                }
                oldVersion = reader.GetString(0);
                reader.Close();
                // Si la version es distinta, ejecutar el script de upgrade
                if (oldVersion != dbVersion)
                {
                    // Ejecutar el script que corresponda.
                    // Agregar elseif para sucesivos cambios de version
                    if (oldVersion == "1.1" && dbVersion == "1.2")
                    {                
                        UpgradeDatabase_11_12();
                    }
                    else
                    {
                        MessageBox.Show("Error de instalación. No se encuentra el script de actualización de la versión " + oldVersion + " a la versión " + dbVersion + "\nElimine la BD anterior y reinstale");
                        Environment.Exit(1);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error accessing Database: " + e.Message);
                Environment.Exit(1);
            }
        }

        public static Database GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Database();
            }
            return _instance;
        }

        public static string GetDbVersion()
        {
            return dbVersion;
        }

        public SQLiteDataReader ExecuteQuery(string query, Dictionary<string, object> prms = null)
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(query, dbConn);
                if (prms != null)
                {
                    foreach (KeyValuePair<string, Object> param in prms)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error accediendo a la Base de Datos: " + e.Message);
                Environment.Exit(1);
            }
            return null;
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object> prms = null)
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(query, dbConn);
                if (prms != null)
                {
                    foreach (KeyValuePair<string, Object> param in prms)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error accediendo a la Base de Datos: " + e.Message);
                Environment.Exit(1);
            }
            return -1;
        }

        public void Dispose()
        {
            if (dbConn != null)
            {
                dbConn.Close();
            }
        }
    }
}

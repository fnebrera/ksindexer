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
using KsIndexerNET;

namespace KsIndexerNET.Db
{
    /**
     * Clase para acceder a datos en una BD SQLite.
     * Incluye los métodos básicos para ejecutar consultas y comandos.
     * 
     * Adicionalmente incuye métodos para actualizar la base de datos de manera automática
     * en basea la versión anterior y la actual.
     * 
     * IMPORTANTE: La version de la base de datos no tiene nada que ver con la version de la aplicación.
     * 
     * Version DB Cambios
     * ---------- ---------------------------------------------------------------------------
     * 1.0        Version inicial
     * 1.1        Se agrega la tabla DbVersion con un solo campo, version, que se actualiza en cada upgrade.
     *            Se agrega el campo 'TextNorm' a la tabla 'Documentos' para almacenar el texto normalizado.
     * 1.2        Se agrega la tabla 'Doc_Annexes' para almacenar los anexos de los documentos, asi como el
     *            indice 'Doc_Annexes_PK' para la clave primaria.
     * 1.3        Se crea el indice 'Doc_Keywords_ByKey' para el campo 'Keyword' de la tabla 'Doc_Keywords',
     *            para agilizar las búsquedas por clave.
     * 1.4        Se crea la tabla INodes para almacenar las carpetas y subcarpetas en las que archivamos los documentos.
     *            Se agrega el campo 'INodeId' a la tabla 'Documentos' para indicar en qué carpeta está el documento.
     */
    public partial class Database
    {
        public const string SQL_GET_LAST_ID = "SELECT last_insert_rowid()";
        // Se ha creado una tabla DbVersion con un solo campo, version, que se actualiza en cada upgrade. Al iniciar la
        // aplicación se lee la versión de la BD y se compara con la versión actual. Si es menor, se ejecutan los scripts
        // de upgrade necesarios.
        private const string dbVersion = "1.4";
        // Formato del connString en SQLite: "Data Source=.\\KsIndexer.db3;Version=3;";
        // En desarrollo usamos la BD local
        private const string dbDevelop = ".\\KsIndexer.db3";
        // Una vez instalado, está en un directorio fijo, para evitar los problemas de
        // acceso en actualización en la carpeta Program Files
        private const string dbRelease = "C:\\KsIndexer\\KsIndexer.db3";
        private static string dbPath = "";
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
                    dbPath = dbDevelop;
                }
                else if (File.Exists(dbRelease))
                {
                    connString = "Data Source=" + dbRelease + ";Version=3;";
                    dbPath = dbRelease;
                }
                else
                {
                    Messages.ShowError(Texts.ERROR_DB_NOT_FOUND);
                    Environment.Exit(1);
                }
                dbConn = new SQLiteConnection(connString);
                dbConn.Open();
                SQLiteCommand cmd;
                SQLiteDataReader reader;
                // Verificamos que exista la tabla DbVersion. Si no existe, es un error de instalación
                if (!TableExists("DbVersion"))
                {
                    Messages.ShowError(Texts.ERROR_WRONG_DB + "\n" + Texts.ERROR_WRONG_DB1);
                    Environment.Exit(1);
                }
                // Si existe, leer la version
                cmd = new SQLiteCommand("SELECT version FROM DbVersion", dbConn);
                reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    Messages.ShowError(Texts.ERROR_WRONG_DB + "\n" + Texts.ERROR_WRONG_DB1);
                    Environment.Exit(1);
                }
                oldVersion = reader.GetString(0);
                reader.Close();
                // Si la version es antigua, ejecutar el script de upgrade
                if (oldVersion.CompareTo(dbVersion) < 0)
                {
                    Messages.ShowInfo(Texts.UPDATE_DATABASE + " " + dbVersion + "\n" + Texts.UPDATE_DATABASE1);
                    // Ejecutar el o los scripts que corresponda.
                    switch (oldVersion)
                    {
                        case "1.0":
                            UpgradeDatabase_10_11();
                            UpgradeDatabase_11_12();
                            UpgradeDatabase_12_13();
                            break;
                        case "1.1":
                            UpgradeDatabase_11_12();
                            UpgradeDatabase_12_13();
                            break;
                        case "1.2":
                            UpgradeDatabase_12_13();
                            break;
                        case "1.3":
                            UpgradeDatabase_13_14();
                            break;
                        default:
                            Messages.ShowError("Internal error. No sccript to update from DB version " +
                                oldVersion + " to version " + dbVersion + "\nRemove previous database and reinstall");
                            Environment.Exit(1);
                            break;
                    }
                    // Actualizar la version en la tabla DbVersion
                    UpdateDbVersion(dbVersion);
                }
            }
            catch (Exception e)
            {
                Messages.ShowError("SQLite error accessing database: " + e.Message +
                    "\nPlease contact support team");
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

        public static string GetDbPath()
        {
            return dbPath;
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
                Messages.ShowError(Texts.ERROR_ACCES_DATABASE + ": " + e.Message);
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
                Messages.ShowError(Texts.ERROR_ACCES_DATABASE + ": " + e.Message);
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

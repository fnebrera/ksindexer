﻿using KsIndexerNET;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsIndexerNET.Db
{
    public class Document
    {
        // Queries standard
        public const string SQL_DOC_EXISTS = "SELECT count(*) FROM Documents WHERE id = @id";
        // V 1.2.0 FNG 2024-02-10 : Por titulo normalizado
        public const string SQL_DOC_EXISTS_SIMILAR = "SELECT count(*) FROM Documents WHERE titlenorm = @titlenorm AND date = @date";
        // V 1.2.0 FNG 2024-02-09 : Se agrega el campo INodeId a la tabla Documents
        //public const string SQL_DOC_INSERT = "INSERT INTO Documents (title, date, text, pdf, titlenorm) VALUES (@title, @date, @text, @pdf, @titlenorm)";
        public const string SQL_DOC_INSERT = "INSERT INTO Documents (title, date, text, pdf, titlenorm, inodeid) " +
            "VALUES (@title, @date, @text, @pdf, @titlenorm, @inodeid)";
        //public const string SQL_DOC_UPDATE = "UPDATE Documents SET title = @title, date = @date, text = @text, pdf = @pdf, titlenorm = @titlenorm WHERE id = @id";
        public const string SQL_DOC_UPDATE = "UPDATE Documents SET title = @title, date = @date, " + 
            "text = @text, pdf = @pdf, titlenorm = @titlenorm, inodeid = @inodeid WHERE id = @id";
        public const string SQL_DOC_UPDATE_INODE = "UPDATE Documents SET inodeid = @inodeid WHERE id = @id";
        public const string SQL_DOC_DELETE = "DELETE FROM Documents WHERE id = @id";
        //public const string SQL_DOC_SELECT = "SELECT title, date, text, pdf, titlenorm FROM Documents WHERE id = @id";
        public const string SQL_DOC_SELECT = "SELECT title, date, text, pdf, titlenorm, inodeid FROM Documents WHERE id = @id";
        public const string SQL_DOC_SELECT_BY_DATE = "SELECT id, date, title FROM Documents WHERE date(date) = @sdate ORDER BY date";
        public const string SQL_DOC_SELECT_BY_INODE = "SELECT id, date, title FROM Documents WHERE inodeid = @inodeid ORDER BY date";

        public int Id { get; set; } = 0;
        public string Title { get; set; } = "";
        public DateTime DocDate { get; set; } = DateTime.MinValue;
        public string DocText { get; set; } = "";
        public string ImportedText { get; set; } = "";
        public byte[] Pdf { get; set; } = new byte[0];
        public List<Keyword> Keywords { get; set; } = new List<Keyword>();
        public List<Attendant> Attendants { get; set; } = new List<Attendant>();
        public List<Annex> Annexes { get; set; } = new List<Annex>();
        // V 1.2.0 FNG 2024-02-11 : Se agrega el campo INodeId. Por defecto es 1 (carpeta raiz)
        public int INodeId { get; set; } = 1;

        // Constructor vacío
        public Document()
        {
        }

        // Limpiar el documento actual
        public void Clear()
        {
            Id = 0;
            Title = "";
            DocDate = DateTime.MinValue;
            DocText = "";
            ImportedText = "";
            Pdf = new byte[0];
            Keywords = new List<Keyword>();
            Attendants = new List<Attendant>();
            Annexes = new List<Annex>();
            INodeId = 1;
        }

        // Eliminar una palabra clave
        public void DeleteKeyword(string clave)
        {
            for (int i = 0; i < Keywords.Count; i++)
            {
                if (Keywords[i].Key == clave)
                {
                    Keywords.RemoveAt(i);
                    break;
                }
            }
        }

        // Eliminar un asistente
        public void DeleteAttendant(string attendant)
        {
            for (int i = 0; i < Attendants.Count; i++)
            {
                if (Attendants[i].Name == attendant)
                {
                    Attendants.RemoveAt(i);
                    break;
                }
            }
        }

        // Eliminar un anexo
        public void DeleteAnnex(string filename)
        {
            for (int i = 0; i < Annexes.Count; i++)
            {
                if (Annexes[i].FileName == filename)
                {
                    Annex anexo = Annexes[i];
                    // Si tiene id de documento, borrarlo de la base de datos
                    if (anexo.DocId > 0 && anexo.FileName.Length > 0)
                    {
                        anexo.Delete();
                    }
                    Annexes.RemoveAt(i);
                    break;
                }
            }
        }

        // Comprueba si existe un documento con el id indicado.
        // Por ahora no se usa.
        public static bool ExistsById(int id)
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>();
            prms.Add("@id", id);
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_DOC_EXISTS, prms);
            if (!rdr.Read())
                return false;
            return (rdr.GetInt32(0) > 0);
        }

        // Comprueba si existe un documento con el mismo título y fecha.
        // V 1.2.0 FNG 2024-02-10 : Se comprueba por titulo normalizado
        public static bool ExistsSimilar(string title, DateTime dat)
        {
            Database db = Database.GetInstance();
            string titlenorm = FileUtils.NormalizeString(title);
            Dictionary<string, object> prms = new Dictionary<string, object>
            {
                { "@titlenorm", titlenorm },
                { "@date", dat }
            };
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_DOC_EXISTS_SIMILAR, prms);
            if (!rdr.Read())
                return false;
            return (rdr.GetInt32(0) > 0);
        }   

        // Guardar el documento actual, diferenciando insert o update según que el Id sea o no cero
        public bool Save()
        {
            Database db = Database.GetInstance();
            // V 1.1 FNG 2024-01-26 : Normalizar el título
            string titleNorm = FileUtils.NormalizeString(Title);
            if (Id > 0)
            {
                // Actualizar en la tabla Documents
                Dictionary<string, object> prms = new Dictionary<string, object>
                {
                    { "@id", Id },
                    { "@title", Title },
                    { "@date", DocDate },
                    { "@text", DocText },
                    { "@pdf", Pdf },
                    { "@titlenorm", titleNorm },
                    { "@inodeid", INodeId }
                };
                if (db.ExecuteNonQuery(SQL_DOC_UPDATE, prms) != 1)
                {
                    return false;
                }
            }
            else
            {
                // Insertar en la tabla Documents
                Dictionary<string, object> prms = new Dictionary<string, object>() 
                {
                    { "@title", Title },
                    { "@date", DocDate },
                    { "@text", DocText },
                    { "@pdf", Pdf },
                    { "@titlenorm", titleNorm },
                    { "@inodeid", INodeId }
                };
                if (db.ExecuteNonQuery(SQL_DOC_INSERT, prms) != 1)
                {
                    return false;
                }
                // Obtener el id del documento insertado
                SQLiteDataReader rdr = db.ExecuteQuery(Database.SQL_GET_LAST_ID);
                rdr.Read();
                Id = rdr.GetInt32(0);
                if (Id < 1)
                    return false;
            }
            // Insertar en la tabla Doc_keywords, eliminando previamante las claves existentes
            if (!Keyword.Delete(Id))
                return false;
            foreach (Keyword palabra in Keywords)
            {
                palabra.DocId = Id;
                if (!palabra.Insert())
                    return false;
            }
            // Insertar en la tabla Doc_attendants, eliminando previamante los asistentes existentes
            if (!Attendant.Delete(Id))
                return false;
            foreach (Attendant asistente in Attendants)
            {
                asistente.DocId = Id;
                if (!asistente.Insert())
                    return false;
            }
            // Insertar en la tabla Doc_attachments los que no tengan id de documento
            foreach (Annex anexo in Annexes)
            {
                if (anexo.DocId < 1)
                {
                    anexo.DocId = Id;
                    if (!anexo.Insert())
                        return false;
                }
            }
            return true;
        }

        // Actualizar el campo INodeId, cuando movemos un documento a otra carpeta
        public static bool UpdateInode(int docId, int nodeId)
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>
            {
                { "@id", docId },
                { "@inodeid", nodeId }
            };
            return db.ExecuteNonQuery(SQL_DOC_UPDATE_INODE, prms) == 1;
        }

        // Cargar un documento por id
        public static Document Load(int id)
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>()
            {
                { "@id", id }
            };
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_DOC_SELECT, prms);
            if (!rdr.Read())
                return null;
            Document doc = new Document()
            {
                Id = id,
                Title = rdr.GetString(0),
                DocDate = rdr.GetDateTime(1),
                DocText = rdr.GetString(2),
                Pdf = (byte[])rdr.GetValue(3),
                Keywords = Keyword.GetByDocId(id),
                Attendants = Attendant.GetByDocId(id),
                Annexes = Annex.GetByDocId(id),
                // Saltamos el campo 4 (titlenorm) que no forma parte del objeto (no es necesario)
                INodeId = rdr.GetInt32(5)
            };
            rdr.Close();
            return doc;
        }

        // Devuelve una lista con fecha/hora y titulo de documentos con la misma fecha (sin la hora)
        public static List<string[]> GetByDate(DateTime date)
        {
            List<string[]> docs = new List<string[]>();
            string sdate = date.ToString("yyyy-MM-dd");
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>()
            {
                { "@sdate", sdate }
            };
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_DOC_SELECT_BY_DATE, prms);
            while (rdr.Read())
            {
                docs.Add(new string[] { rdr.GetInt32(0).ToString(), LangUtils.FormatDateTime(rdr.GetDateTime(1)), rdr.GetString(2) });
            }
            rdr.Close();
            return docs;
        }

        // Devuelve una lista de documentos en base a un SQL predefinido.
        // Se usa para busquedas complejas.
        public static List<string[]> GetBySql(string sql)
        {
            List<string[]> docs = new List<string[]>();
            Database db = Database.GetInstance();
            SQLiteDataReader rdr = db.ExecuteQuery(sql);
            while (rdr.Read())
            {
                //docs.Add(new string[] { rdr.GetInt32(0).ToString(), rdr.GetDateTime(1).ToString("dd/MM/yyy HH:mm"), rdr.GetString(2) });
                docs.Add(new string[] { rdr.GetInt32(0).ToString(), LangUtils.FormatDateTime(rdr.GetDateTime(1)), rdr.GetString(2) });
            }
            rdr.Close();
            return docs;
        }

        // Obtener documentos por carpeta.
        // Se usa en la vista de carpetas, para mostrar los documentos que contiene la
        // carpeta seleccionada.
        // TODO: Establecer a priori el orden de los documentos.
        public static List<string[]> GetByInode(int inodeid)
        {
            List<string[]> docs = new List<string[]>();
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>()
            {
                { "@inodeid", inodeid }
            };
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_DOC_SELECT_BY_INODE, prms);
            while (rdr.Read())
            {
                docs.Add(new string[] { rdr.GetInt32(0).ToString(), LangUtils.FormatDateTime(rdr.GetDateTime(1)), rdr.GetString(2) });
            }
            rdr.Close();
            return docs;
        }

        // Elimina un documento y sus datos asociados
        public static bool Delete(int id)
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>()
            {
                { "@id", id }
            };
            if (db.ExecuteNonQuery(SQL_DOC_DELETE, prms) != 1)
                return false;
            if (!Keyword.Delete(id))
                return false;
            if (!Attendant.Delete(id))
                return false;
            if (!Annex.DeleteByDocId(id))
                return false;
            return true;
        }

        // Obtener el contenido del anexo solicitado como un byte array
        public byte[] GetAnnexContent(string filename)
        {
            Annex anexo = null;
            // Buscar el anexo por nombre
            foreach (Annex an in Annexes)
            {
                if (an.FileName == filename)
                {
                    anexo = an;
                    break;
                }
            }
            if (anexo == null)
            {
                Messages.ShowError(Texts.ERROR_READ_FILE + " " + filename);
                return null;
            }
            // Si ya tiene contenido, lo devolvemos, si no,
            // lo leemos de la Base de Datos.
            if (anexo.Content.Length == 0)
            {
                if (!anexo.FillContent())
                    return null;
            }
            return anexo.Content;
        }
    }
}

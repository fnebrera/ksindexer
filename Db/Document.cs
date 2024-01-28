using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ksindexer.Db
{
    public class Document
    {
        // Queries standard
        public const string SQL_DOC_EXISTS = "SELECT count(*) FROM Documents WHERE id = @id";
        public const string SQL_DOC_EXISTS_SIMILAR = "SELECT count(*) FROM Documents WHERE title = @title AND date = @date";
        public const string SQL_DOC_INSERT = "INSERT INTO Documents (title, date, text, pdf, titlenorm) VALUES (@title, @date, @text, @pdf, @titlenorm)";
        public const string SQL_DOC_UPDATE = "UPDATE Documents SET title = @title, date = @date, text = @text, pdf = @pdf, titlenorm = @titlenorm WHERE id = @id";
        public const string SQL_DOC_DELETE = "DELETE FROM Documents WHERE id = @id";
        public const string SQL_DOC_SELECT = "SELECT title, date, text, pdf FROM Documents WHERE id = @id";
        public const string SQL_GET_LAST_ID = "SELECT last_insert_rowid()";
        public const string SQL_DOC_SELECT_BY_DATE = "SELECT id, date, title FROM Documents WHERE date(date) = @sdate ORDER BY date";

        public int Id { get; set; } = 0;
        public string Titulo { get; set; } = "";
        public DateTime Fecha { get; set; } = DateTime.MinValue;
        public string Texto { get; set; } = "";
        public string TextoImportado { get; set; } = "";
        public byte[] Pdf { get; set; } = new byte[0];
        public List<Keyword> Claves { get; set; } = new List<Keyword>();
        public List<Attendant> Asistentes { get; set; } = new List<Attendant>();
        public List<Annex> Anexos { get; set; } = new List<Annex>();

        // Constructor vacío
        public Document()
        {
        }

        // Constructor en base a datos existentes
        public Document(int id,
            string titulo,
            DateTime fecha,
            string texto,
            byte[] pdf, 
            List<Keyword> claves,
            List<Attendant> asistentes,
            List<Annex> anexos)
        {
            Id = id;
            Titulo = titulo;
            Fecha = fecha;
            Texto = texto;
            Pdf = pdf;
            Claves = claves;
            Asistentes = asistentes;
            Anexos = anexos;
        }

        public void Clear()
        {
            Id = 0;
            Titulo = "";
            Fecha = DateTime.MinValue;
            Texto = "";
            TextoImportado = "";
            Pdf = new byte[0];
            Claves = new List<Keyword>();
            Asistentes = new List<Attendant>();
            Anexos = new List<Annex>();
        }

        public void BorrarClave(string clave)
        {
            for (int i = 0; i < Claves.Count; i++)
            {
                if (Claves[i].Key == clave)
                {
                    Claves.RemoveAt(i);
                    break;
                }
            }
        }

        public void BorrarAsistente(string asistente)
        {
            for (int i = 0; i < Asistentes.Count; i++)
            {
                if (Asistentes[i].Name == asistente)
                {
                    Asistentes.RemoveAt(i);
                    break;
                }
            }
        }

        public void BorrarAnexo(string filename)
        {
            for (int i = 0; i < Anexos.Count; i++)
            {
                if (Anexos[i].FileName == filename)
                {
                    Annex anexo = Anexos[i];
                    // Si tiene id de documento, borrarlo de la base de datos
                    if (anexo.DocId > 0 && anexo.FileName.Length > 0)
                    {
                        anexo.Delete();
                    }
                    Anexos.RemoveAt(i);
                    break;
                }
            }
        }

        public static bool Exists(int id)
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>();
            prms.Add("@id", id);
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_DOC_EXISTS, prms);
            if (!rdr.Read())
                return false;
            return (rdr.GetInt32(0) > 0);
        }

        public static bool ExistsSimilar(string title, DateTime dat)
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>();
            prms.Add("@title", title);
            prms.Add("@date", dat);
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_DOC_EXISTS_SIMILAR, prms);
            if (!rdr.Read())
                return false;
            return (rdr.GetInt32(0) > 0);
        }   

        public bool Save()
        {
            Database db = Database.GetInstance();
            // V 1.1 FNG 2024-01-26 : Normalizar el título
            string titleNorm = FileUtils.NormalizeString(Titulo);
            if (Id > 0)
            {
                // Actualizar en la tabla Documents
                Dictionary<string, object> prms = new Dictionary<string, object>
                {
                    { "@id", Id },
                    { "@title", Titulo },
                    { "@date", Fecha },
                    { "@text", Texto },
                    { "@pdf", Pdf },
                    { "@titlenorm", titleNorm }
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
                    { "@title", Titulo },
                    { "@date", Fecha },
                    { "@text", Texto },
                    { "@pdf", Pdf },
                    { "@titlenorm", titleNorm }
                };
                if (db.ExecuteNonQuery(SQL_DOC_INSERT, prms) != 1)
                {
                    return false;
                }
                // Obtener el id del documento insertado
                SQLiteDataReader rdr = db.ExecuteQuery(SQL_GET_LAST_ID);
                rdr.Read();
                Id = rdr.GetInt32(0);
                if (Id < 1)
                    return false;
            }
            // Insertar en la tabla Doc_keywords, eliminando previamante las claves existentes
            if (!Keyword.Delete(Id))
                return false;
            foreach (Keyword palabra in Claves)
            {
                palabra.DocId = Id;
                if (!palabra.Insert())
                    return false;
            }
            // Insertar en la tabla Doc_attendants, eliminando previamante los asistentes existentes
            if (!Attendant.Delete(Id))
                return false;
            foreach (Attendant asistente in Asistentes)
            {
                asistente.DocId = Id;
                if (!asistente.Insert())
                    return false;
            }
            // Insertar en la tabla Doc_attachments los que no tengan id de documento
            foreach (Annex anexo in Anexos)
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
                Titulo = rdr.GetString(0),
                Fecha = rdr.GetDateTime(1),
                Texto = rdr.GetString(2),
                Pdf = (byte[])rdr.GetValue(3),
                Claves = Keyword.GetByDocId(id),
                Asistentes = Attendant.GetByDocId(id),
                Anexos = Annex.GetByDocId(id)
            };
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
                docs.Add(new string[] { rdr.GetInt32(0).ToString(), rdr.GetDateTime(1).ToString("dd/MM/yyy HH:mm"), rdr.GetString(2) });
            }
            return docs;
        }

        // Devuelve una lista de documento en base a un SQL predefinido
        public static List<string[]> GetBySql(string sql)
        {
            List<string[]> docs = new List<string[]>();
            Database db = Database.GetInstance();
            SQLiteDataReader rdr = db.ExecuteQuery(sql);
            while (rdr.Read())
            {
                docs.Add(new string[] { rdr.GetInt32(0).ToString(), rdr.GetDateTime(1).ToString("dd/MM/yyy HH:mm"), rdr.GetString(2) });
            }
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

        // Obtener el anexo solicitado por nombre
        public byte[] GetAnnexContent(string filename)
        {
            Annex anexo = null;
            // Buscar el anexo por nombre
            foreach (Annex an in Anexos)
            {
                if (an.FileName == filename)
                {
                    anexo = an;
                    break;
                }
            }
            if (anexo == null)
            {
                Messages.ShowError("Se ha producido un error interno al acceder al archivo " + filename);
                return null;
            }
            // Si ya tiene contenido, lo devolvemos
            if (anexo.Content.Length == 0)
            {
                if (!anexo.FillContent())
                    return null;
            }
            // Devolvemos el contenido
            return anexo.Content;
        }
    }
}

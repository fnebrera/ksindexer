using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ksindexer.Db
{
    public class Annex
    {
        public const string SQL_ANX_DELETE = "DELETE FROM Doc_annexes WHERE DocId = @id AND FileName = @filename";
        public const string SQL_ANX_DELETE_BY_DOCID = "DELETE FROM Doc_annexes WHERE DocId = @id";
        public const string SQL_ANX_INSERT = "INSERT INTO Doc_annexes (DocId, FileName, Content, Size) VALUES (@id, @filename, @content, @size)";
        public const string SQL_ANX_SELECT_CONTENT = "SELECT Content FROM Doc_annexes WHERE DocId = @id AND FileName = @filename";
        public const string SQL_ANX_SELECT = "SELECT FileName, Size FROM Doc_annexes WHERE DocId = @id";

        public int DocId { get; set; }
        public string FileName { get; set; }

        public int Size { get; set; }

        public byte[] Content { get; set; }

        public Annex(int docId, string nombre, int tamano)
        {
            DocId = docId;
            FileName = nombre;
            Size = tamano;
            Content = new byte[0];
        }

        public Annex(int docId, string nombre, int tamano, byte[] contenido)
        {
            DocId = docId;
            FileName = nombre;
            Size = tamano;
            Content = contenido;
        }

        public bool Insert()
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object> {
                { "@id", DocId },
                { "@filename", FileName },
                { "@content", Content},
                { "@size", Size}
            };
            return (db.ExecuteNonQuery(SQL_ANX_INSERT, prms) == 1);
        }

        public bool Delete()
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>
            {
                { "@id", DocId },
                { "@filename", FileName }
            };
            return (db.ExecuteNonQuery(SQL_ANX_DELETE, prms) >= 0);
        }

        public static bool DeleteByDocId(int docId)
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>
            {
                { "@id", docId }
            };
            return (db.ExecuteNonQuery(SQL_ANX_DELETE_BY_DOCID, prms) >= 0);
        }

        public static List<Annex> GetByDocId(int docId)
        {
            List<Annex> anexos = new List<Annex>();
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>
            {
                { "@id", docId }
            };
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_ANX_SELECT, prms);
            while (rdr.Read())
            {
                anexos.Add(new Annex(docId, rdr.GetString(0), rdr.GetInt32(1)));
            }
            return anexos;
        }

        public bool FillContent()
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>
            {
                { "@id", DocId },
                { "@filename", FileName }
            };
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_ANX_SELECT_CONTENT, prms);
            if (!rdr.Read())
                return false;
            Content = (byte[])rdr.GetValue(0);
            Size = Content.Length;
            return true;
        }
    }
}

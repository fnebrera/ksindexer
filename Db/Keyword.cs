using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsIndexerNET.Db
{
    public class Keyword
    {
        public const string SQL_PAL_INSERT = "INSERT INTO Doc_keywords (DocId, Keyword) VALUES (@id, @keyword)";
        public const string SQL_PAL_DELETE = "DELETE FROM Doc_keywords WHERE DocId = @id";
        public const string SQL_PAL_SELECT = "SELECT Keyword FROM Doc_keywords WHERE DocId = @id";

        public int DocId { get; set; }
        public string Key { get; set; }

        public Keyword(int docId, string keyword)
        {
            DocId = docId;
            Key = keyword;
        }

        public bool Insert()
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>()
            {
                { "@id", DocId },
                { "@keyword", Key }
            };
            return (db.ExecuteNonQuery(SQL_PAL_INSERT, prms) == 1);
        }

        public static bool Delete(int docId)
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>()
            {
                { "@id", docId }
            };
            return (db.ExecuteNonQuery(SQL_PAL_DELETE, prms) >= 0);
        }

        public static List<Keyword> GetByDocId(int docId)
        {
            List<Keyword> palabras = new List<Keyword>();
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>()
            {
                { "@id", docId }
            };
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_PAL_SELECT, prms);
            while (rdr.Read())
            {
                palabras.Add(new Keyword(docId, rdr.GetString(0)));
            }
            return palabras;
        }
    }
}

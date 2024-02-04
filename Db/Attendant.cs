using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsIndexerNET.Db
{
    public class Attendant
    {
        public const string SQL_ASI_DELETE = "DELETE FROM Doc_attendants WHERE DocId = @id";
        public const string SQL_ASI_INSERT = "INSERT INTO Doc_attendants (DocId, Name, Company) VALUES (@id, @name, @company)";
        public const string SQL_ASI_SELECT = "SELECT Name, Company FROM Doc_attendants WHERE DocId = @id";

        public Attendant(int docId, string name, string company)
        {
            DocId = docId;
            Name = name;
            Company = company;
        }

        public int DocId { get; set; }
        public string Name { get; set; } 
        public string Company { get; set; }

        public bool Insert()
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>();
            prms.Add("@id", DocId);
            prms.Add("@name", Name);
            prms.Add("@company", Company);
            return (db.ExecuteNonQuery(SQL_ASI_INSERT, prms) == 1);
        }

        public static bool Delete(int docId)
        {
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>
            {
                { "@id", docId }
            };
            return (db.ExecuteNonQuery(SQL_ASI_DELETE, prms) >= 0);
        }

        public static List<Attendant> GetByDocId(int docId)
        {
            List<Attendant> asistentes = new List<Attendant>();
            Database db = Database.GetInstance();
            Dictionary<string, object> prms = new Dictionary<string, object>
            {
                { "@id", docId }
            };
            SQLiteDataReader rdr = db.ExecuteQuery(SQL_ASI_SELECT, prms);
            while (rdr.Read())
            {
                asistentes.Add(new Attendant(docId, rdr.GetString(0), rdr.GetString(1)));
            }
            return asistentes;
        }
    }
}

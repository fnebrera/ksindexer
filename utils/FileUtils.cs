using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KsIndexerNET
{
    internal class FileUtils
    {
        // Leer un archivo de texto desde disco y devolver el contenido como string
        public static string GetTextFromDisc(string path)
        {
            string content = "";
            try
            {
                // Recibidos desde Amazon. Llegan en UTF-8, pero con saltos de linea solo LF
                content = System.IO.File.ReadAllText(path, Encoding.UTF8);
                content = content.Replace("\n", "\r\n");
                // Encoding estandar Windows Latin 1
                // content = System.IO.File.ReadAllText(path, Encoding.Latin1);
            }
            catch (Exception e)
            {
                Messages.ShowError(Texts.ERROR_READ_FILE + ": " + e.Message);
            }
            return content;
        }

        // Leer un archivo binario desde disco y devolver el contenido como byte[]
        public static byte[] GetBinaryFromDisc(string path)
        {
            byte[] content = new byte[0];
            try
            {
                content = System.IO.File.ReadAllBytes(path);
            }
            catch (Exception)
            {
                // No hacemos nada, si no existe el archivo devolvemos null
            }
            return content;
        }

        public static void WritePdfToDisc(string path, byte[] pdf)
        {
            try
            {
                System.IO.File.WriteAllBytes(path, pdf);
            }
            catch (Exception e)
            {
                Messages.ShowError(Texts.ERROR_WRITE_TEMP + ": " + e.Message);
            }
        }

        public static string GetDocTitle(string content)
        {
            string[] lines = content.Split('\n');
            // El titulo debe ser la primera linea no vacia, exceptuando las que empiecen por "Página..." y los metadatos
            for (int i = 0; i < lines.Length; i++)
            {
                string linea = lines[i].Trim();
                // V1.1.6 Los caracteres especiales estan en configuracion
                if (linea.Length > 0 &&
                    !linea.StartsWith(Main.DateStartChar) &&
                    !linea.StartsWith(Main.TagStartChar) &&
                    !linea.StartsWith(Main.AttendantStartChar) &&
                    !linea.StartsWith("Pág") && 
                    !linea.StartsWith("Pag"))
                {
                    return linea;
                }
            }
            return "";
        }

        public static string GetDocDate(string content)
        {
            string[] lines = content.Split('\n');
            // La fecha debe ser la primera linea que empiece por @
            for (int i = 0; i < lines.Length; i++)
            {
                string linea = lines[i].Trim();
                // V 1.1.6 Se toma de configuracion
                //if (linea.Length > 0 && linea[0] == '@')
                if (linea.Length > 0 && linea.StartsWith(Main.DateStartChar))
                {
                    // A veces se escapan puntos o comas al escribir. Los quitamos, y tambien los blancos antes y despues
                    string sfecha = linea.Substring(Main.DateStartChar.Length)
                        .Replace('.', ' ')
                        .Replace(',', ' ')
                        .Trim();
                    DateTime fecha = LangUtils.ParseDateTime(sfecha);
                    if (fecha == DateTime.MinValue)
                        return "";
                    return LangUtils.FormatDateTime(fecha);
                }
            }
            return "";
        }

        public static string[] GetDocKeywords(string content)
        {
            string[] lines = content.Split('\n');
            // Las palabras clave deben ser las lineas que empiecen por #
            List<string> keywords = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                string linea = lines[i].Trim();
                // No tratar lineas de significado especial, salvo las keywords
                // V 1.1.6 Los caracteres especiales estan en configuracion
                if (linea.Length == 0 ||
                    linea.StartsWith(Main.DateStartChar) ||
                    linea.StartsWith(Main.AttendantStartChar) ||
                    linea.StartsWith("Pág"))
                    continue;
                // Lineas que empiezan por # consisten sólamente en palabras clave
                if (linea.StartsWith(Main.TagStartChar))
                {
                    // A veces se escapan puntos al escribir. Tampoco quiero las comas
                    string kws = linea.Substring(Main.TagStartChar.Length)
                        .Replace('.', ' ')
                        .Replace(',', ' ')
                        .Trim();
                    string[] akeyword = kws.Split(' ');
                    for (int j = 0; j < akeyword.Length; j++)
                    {
                        string keyword = NormalizeString(akeyword[j]);
                        if (keyword.Length > 0 && ! keywords.Contains(keyword))
                        {
                            keywords.Add(keyword);
                        }
                    }
                }
                else
                {
                    // Lineas normales, que pueden contener keywords
                    while (true)
                    {
                        // V 1.1.6 Los caracteres especiales estan en configuracion
                        int pos = linea.IndexOf(Main.TagStartChar);
                        if (pos < 0)
                            break;
                        // Saltar caracteres de inicio del tag menos el primero
                        pos += Main.TagStartChar.Length;
                        // Saltamos posibles blancos para encontrar la primera palabra
                        while (pos < linea.Length && linea[pos] == ' ')
                            pos++;
                        linea = linea.Substring(pos);
                        // Sólo nos interesa la primera palabra después del #
                        int pos2 = linea.IndexOf(' ');
                        if (pos2 < 0)
                            pos2 = linea.Length;
                        string keyword = NormalizeString(
                            linea.Substring(0, pos2)
                            .Replace('.', ' ')
                            .Replace(',', ' '));
                        if (keyword.Length > 0 && !keywords.Contains(keyword))
                        {
                            keywords.Add(keyword);
                        }
                    }
                }
            }
            return keywords.ToArray();
        }

        public static string[][] GetDocAttendants(string content)
        {
            string[] lines = content.Split('\n');
            // V 1.1.6 Se establece en configuracion
            // Los asistentes deben ser las lineas que empiecen por >
            List<string[]> attendants = new List<string[]>();
            for (int i = 0; i < lines.Length; i++)
            {
                string linea = lines[i].Trim();
                if (linea.Length > 0 && linea.StartsWith(Main.AttendantStartChar))
                {
                    // A veces se escapan puntos al escribir. Lo quitamos en NormalizeString()
                    //string asistente = linea.Substring(1).Replace('.', ' ').Trim();
                    // V 1.1.6 Permitir mas de un asistente, separados por comas
                    string[] aasistentes = linea.Substring(Main.AttendantStartChar.Length).Split(',');
                    foreach (string ast in aasistentes)
                    {
                        string asistente = ast.Trim();
                        if (asistente.Length == 0)
                            continue;
                        int pos1 = asistente.IndexOf('(');
                        int pos2 = asistente.Substring(pos1 + 1).IndexOf(')');
                        string[] newAtt;
                        if (pos1 > 0 && pos2 > 0)
                        {
                            string nombre = NormalizeString(asistente.Substring(0, pos1));
                            if (nombre.Length == 0)
                                continue;
                            string emp = NormalizeString(asistente.Substring(pos1 + 1, pos2));
                            newAtt = new string[] { nombre, emp };
                        }
                        else
                        {
                            newAtt = new string[] { NormalizeString(asistente), "" };
                        }
                        // Agregar solo sin no existe ya
                        if (!ContainsAttendant(attendants, newAtt))
                            attendants.Add(newAtt);
                    }
                }
            }
            return attendants.ToArray();
        }

        // Comporbar si existe un asistente, incluyendo nombre y empresa
        private static bool ContainsAttendant(List<string[]> attendants, string[] newAtt)
        {
            foreach (string[] attendant in attendants)
            {
                if (attendant[0] == newAtt[0] && attendant[1] == newAtt[1])
                    return true;
            }
            return false;
        }   

        public static string GetTextCleared(string content)
        {
            string[] lines = content.Split('\n');
            // Eliminar las lineas que empiecen por @, #, >
            StringBuilder sb = new StringBuilder();
            bool primera = true;
            for (int i = 0; i < lines.Length; i++)
            {
                string linea = lines[i].Trim();
                // V 1.1.6 Los caracteres especiales estan en configuracion
                if (linea.StartsWith(Main.DateStartChar) ||
                    linea.StartsWith(Main.TagStartChar) || 
                    linea.StartsWith(Main.AttendantStartChar) || 
                    linea.StartsWith("Pág") ||
                    linea.StartsWith("Pag"))
                    continue;
                // La agregamos, a menos que sea la primera linea (titulo)
                if (primera && linea.Length > 0 && !linea.StartsWith("Pág") && !linea.StartsWith("Pag"))
                {
                    primera = false;
                    continue;
                }
                sb.AppendLine(linea);
            }
            return sb.ToString();
        }

        // Poner una cadena en minusculas sustituyendo letras acentuadas por no acentuadas
        public static string NormalizeString(string orig)
        {
            return orig.Trim().ToLower()
                .Replace('á', 'a')
                .Replace('é', 'e')
                .Replace('í', 'i')
                .Replace('ó', 'o')
                .Replace('ú', 'u');
        }
    }
}

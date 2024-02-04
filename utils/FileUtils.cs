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
            // El titulo debe ser la primera linea no vacia, exceptuando las que empiecen por "Página..." y la fecha
            for (int i = 0; i < lines.Length; i++)
            {
                string linea = lines[i].Trim();
                if (linea.Length > 0 && linea[0] != '@' && !linea.StartsWith("Pág"))
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
                if (linea.Length > 0 && linea[0] == '@')
                {
                    // A veces se escapan puntos al escribir. Los quitamos, y tambien los blancos antes y despues
                    string sfecha = linea.Substring(1).Replace('.', ' ').Trim();
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
                if (linea.Length == 0 || linea.StartsWith("@") || linea.StartsWith(">") || linea.StartsWith("Pág"))
                    continue;
                // Lineas que empiezan por # consisten sólamente en palabras clave
                if (linea[0] == '#')
                {
                    // A veces se escapan puntos al escribir.
                    string kws = linea.Substring(1).Replace('.', ' ').Trim();
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
                        int pos = linea.IndexOf('#');
                        if (pos < 0)
                            break;
                        // Saltamos posibles blancos para encontrar la primera palabra
                        while (pos + 1 < linea.Length && linea[pos + 1] == ' ')
                            pos++;
                        linea = linea.Substring(pos + 1);
                        // Sólo nos interesa la primera palabra después del #
                        int pos2 = linea.IndexOf(' ');
                        if (pos2 < 0)
                            pos2 = linea.Length;
                        string keyword = NormalizeString(linea.Substring(0, pos2));
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
            // Los asistentes deben ser las lineas que empiecen por >
            List<string[]> attendants = new List<string[]>();
            for (int i = 0; i < lines.Length; i++)
            {
                string linea = lines[i].Trim();
                if (linea.Length > 0 && linea[0] == '>')
                {
                    // A veces se escapan puntos al escribir.
                    string asistente = linea.Substring(1).Replace('.', ' ').Trim();
                    int pos1 = asistente.IndexOf('(');
                    int pos2 = asistente.Substring(pos1+1).IndexOf(')');
                    if (pos1 > 0 && pos2 > 0)
                    {
                        string nombre = asistente.Substring(0,pos1).Trim();
                        string emp = asistente.Substring(pos1+1, pos2).Trim();
                        attendants.Add(new string[] { NormalizeString(nombre), NormalizeString(emp) });
                    }
                    else
                    {
                        attendants.Add(new string[] { NormalizeString(asistente), "" });
                    }
                }
            }
            return attendants.ToArray();
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
                if (linea.StartsWith("@") || linea.StartsWith("#") || linea.StartsWith(">") || linea.StartsWith("Pág"))
                    continue;
                // La agregamos, a menos que sea la primera linea (titulo)
                if (primera && linea.Length > 0 && !linea.StartsWith("Pág"))
                {
                    primera = false;
                    continue;
                }
                sb.AppendLine(linea);
            }
            return sb.ToString();
        }

        public static string NormalizeString(string orig)
        {
            return orig.Trim().ToLower().Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Replace(".","");
        }
    }
}

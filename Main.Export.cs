using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using ksindexer;
using ksindexer.Db;
using System.Reflection;

namespace KsIndexerNET
{
    partial class Main
    {
        private void MenuExportHtml()
        {
            if (DocEmpty)
                return;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "HTML files (*.html)|*.html|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            dlg.FileName = CurrentDoc.Titulo + ".html";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = dlg.FileName;
            dlg.Dispose();
            if (filename.Trim().Length == 0)
                return;
            // Preparamos el documento de salida
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            html.AppendLine("<title>" + CurrentDoc.Titulo + "</title>");
            // Estilos
            html.AppendLine("<style type=\"text/css\">");
            html.AppendLine("body {font-family: Calibri;}");
            html.AppendLine("h1 {font-size: 24px;}");
            html.AppendLine("h3 {font-size: 16px;}");
            html.AppendLine("table.tb { font-size: 14px;border-collapse: collapse; }");
            html.AppendLine(".tb th, .tb td { padding: 2px; border: 1px solid; }");
            html.AppendLine("pre {font-size: 14px;}");
            html.AppendLine("</style>");
            // FIN estilos
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            // Titulo
            html.AppendLine("<h1>" + CurrentDoc.Titulo + "</h1>");
            // Fecha y (opcionalmente) hora
            string fecha = CurrentDoc.Fecha.ToString("dd/MM/yyyy");
            string hora = CurrentDoc.Fecha.ToString("HH:mm");
            if (hora == "00:00")
                html.AppendLine("<h3>" + fecha + "</h3>");
            else
                html.AppendLine("<h3>" + fecha + " " + hora + "</h3>");
            // Palabras clave
            if (CurrentDoc.Claves.Count > 0)
            {
                html.AppendLine("<h3>");
                foreach (Keyword p in CurrentDoc.Claves)
                    html.Append("#" + p.Key + " ");
                html.AppendLine("</h3>");
            }
            if (CurrentDoc.Asistentes.Count > 0)
            {
                html.AppendLine("<table class='tb'>");
                html.AppendLine("<caption style='font-size: 16;font-weight: bold;'>Asistentes</caption>");
                html.AppendLine("<thead>");
                html.AppendLine("<tr><th>Nombre</th><th>Empresa</th></tr>");
                html.AppendLine("</thead>");
                html.AppendLine("<tbody>");
                foreach (Attendant a in CurrentDoc.Asistentes)
                    html.Append("<tr><td>" + a.Name + "</td><td>" + a.Company + "</td></tr>");
                html.AppendLine("</tbody>");
                html.AppendLine("</table>");
            }
            // Texto
            html.AppendLine("<pre>" + CurrentDoc.Texto + "</pre>");
            // Id
            html.AppendLine("<h3>KsIndexer Id: " + CurrentDoc.Id + "</h3>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            // Guardamos el documento
            try
            {
                System.IO.File.WriteAllText(filename, html.ToString());
            }
            catch (Exception ex)
            {
                Messages.ShowError("Error guardando el documento: " + ex.Message);
            }
        }

        private void MenuExportTxt()
        {
            if (DocEmpty)
                return;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            dlg.FileName = CurrentDoc.Titulo + ".txt";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = dlg.FileName;
            dlg.Dispose();
            if (filename.Trim().Length == 0)
                return;
            // Preparamos el documento de salida
            StringBuilder html = new StringBuilder();
            html.AppendLine(CurrentDoc.Titulo);
            // Fecha
            string fecha = CurrentDoc.Fecha.ToString("dd/MM/yyyy HH:mm");
            html.AppendLine("@ " + fecha);
            // Palabras clave
            if (CurrentDoc.Claves.Count > 0)
            {
                html.Append("#");
                foreach (Keyword p in CurrentDoc.Claves)
                    html.Append(" " + p.Key);
                html.Append(Environment.NewLine);
            }
            if (CurrentDoc.Asistentes.Count > 0)
            {
                foreach (Attendant a in CurrentDoc.Asistentes)
                {
                    html.Append("> " + a.Name);
                    if (a.Company.Length > 0)
                        html.Append(" (" + a.Company + ")");
                    html.Append(Environment.NewLine);
                }
            }
            // Texto
            if (CurrentDoc.Texto.Length > 0) 
            {
                foreach (string linea in CurrentDoc.Texto.Split('\n'))
                {
                    html.AppendLine(linea.Replace("\r",""));
                }
            }
            // Guardamos el documento
            try
            {
                System.IO.File.WriteAllText(filename, html.ToString());
            }
            catch (Exception ex)
            {
                Messages.ShowError("Error guardando el documento: " + ex.Message);
            }
        }
    }
}

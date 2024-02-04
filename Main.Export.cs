using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using KsIndexerNET;
using KsIndexerNET.Db;
using System.Reflection;
using System.Drawing.Printing;

namespace KsIndexerNET
{
    partial class Main
    {
        private void DoMenuExportHtml()
        {
            if (DocEmpty)
                return;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "HTML files (*.html)|*.html|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            dlg.FileName = CurrentDoc.Title + ".html";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = dlg.FileName;
            dlg.Dispose();
            if (filename.Trim().Length == 0)
                return;
            // Preparamos el documento de salida
            string html = GetDocumentAsHtml();
            // Guardamos el documento
            try
            {
                System.IO.File.WriteAllText(filename, html);
            }
            catch (Exception ex)
            {
                Messages.ShowError(Texts.ERROR_SAVE_DOC + ": " + ex.Message);
            }
        }

        private void DoMenuExportTxt()
        {
            if (DocEmpty)
                return;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            dlg.FileName = CurrentDoc.Title + ".txt";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = dlg.FileName;
            dlg.Dispose();
            if (filename.Trim().Length == 0)
                return;
            // Preparamos el documento de salida
            StringBuilder html = new StringBuilder();
            html.AppendLine(CurrentDoc.Title);
            // Fecha
            string fecha = LangUtils.FormatDateTime(CurrentDoc.DocDate);
            html.AppendLine("@ " + fecha);
            // Palabras clave
            if (CurrentDoc.Keywords.Count > 0)
            {
                html.Append("#");
                foreach (Keyword p in CurrentDoc.Keywords)
                    html.Append(" " + p.Key);
                html.Append(Environment.NewLine);
            }
            if (CurrentDoc.Attendants.Count > 0)
            {
                foreach (Attendant a in CurrentDoc.Attendants)
                {
                    html.Append("> " + a.Name);
                    if (a.Company.Length > 0)
                        html.Append(" (" + a.Company + ")");
                    html.Append(Environment.NewLine);
                }
            }
            // Texto
            if (CurrentDoc.DocText.Length > 0) 
            {
                foreach (string linea in CurrentDoc.DocText.Split('\n'))
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
                Messages.ShowError(Texts.ERROR_SAVE_DOC + ": " + ex.Message);
            }
        }

        private void DoMenuPrint()
        {
            // Sólo si no está vacío
            if (DocEmpty)
                return;
            // Preparamos el documento de salida
            string html = GetDocumentAsHtml();
            // Lo salvamos en un archivo temporal
            string filename = System.IO.Path.GetTempPath() + "kstmpfile.html";
            try
            {
                System.IO.File.WriteAllText(filename, html);
            }
            catch (Exception ex)
            {
                Messages.ShowError(Texts.ERROR_SAVE_TEMP_FILE + ": " + ex.Message);
                return;
            }
            // Imprimimos el archivo
            WebBrowser wb = new WebBrowser();
            // Truco para que se vea de un tamaño razonable en la vista previa
            wb.Parent = this;
            // Add an event handler that prints the document after it loads.
            wb.DocumentCompleted +=
                new WebBrowserDocumentCompletedEventHandler(OnPrintDocument);
            // Set the Url property to load the document.
            wb.Url = new Uri(filename);
        }

        // Evento que se dispara cuando el control WebBrowser interno termina de cargar el documento
        private void OnPrintDocument(object sender,
            WebBrowserDocumentCompletedEventArgs e)
        {
            // Mostrar dialogo de opciones de impresión
            ((WebBrowser)sender).ShowPrintPreviewDialog();
            // Dispose the WebBrowser now that the task is complete.
            ((WebBrowser)sender).Dispose();
        }

        private string GetDocumentAsHtml()
        {
            // Preparamos el texto de salida
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            html.AppendLine("<title>" + CurrentDoc.Title + "</title>");
            // Estilos
            html.AppendLine("<style type=\"text/css\">");
            html.AppendLine("body {font-family: Calibri;}");
            html.AppendLine("h1 {font-size: 24px;}");
            html.AppendLine("h3 {font-size: 16px;}");
            html.AppendLine("pre {font-family: Calibri;font-size: 12px;}");
            html.AppendLine("table.tb { font-size: 14px;border-collapse: collapse; }");
            html.AppendLine(".tb th, .tb td { padding: 2px; border: 1px solid; }");
            html.AppendLine("pre {font-size: 14px;}");
            html.AppendLine("</style>");
            // FIN estilos
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            // Titulo
            html.AppendLine("<h1>" + CurrentDoc.Title + "</h1>");
            // Fecha y (opcionalmente) hora
            string fecha = LangUtils.FormatDate(CurrentDoc.DocDate);
            string hora = CurrentDoc.DocDate.ToString("HH:mm");
            if (hora == "00:00")
                html.AppendLine("<h3>" + fecha + "</h3>");
            else
                html.AppendLine("<h3>" + fecha + " " + hora + "</h3>");
            // Palabras clave
            if (CurrentDoc.Keywords.Count > 0)
            {
                html.AppendLine("<h3>");
                foreach (Keyword p in CurrentDoc.Keywords)
                    html.Append("#" + p.Key + " ");
                html.AppendLine("</h3>");
            }
            if (CurrentDoc.Attendants.Count > 0)
            {
                html.AppendLine("<table class='tb'>");
                html.AppendLine("<caption style='font-size: 16;font-weight: bold;'>" + Texts.ATTENDANTS + "</caption>");
                html.AppendLine("<thead>");
                html.AppendLine("<tr><th>" + Texts.NAME + "</th><th>" + Texts.COMPANY + " </th></tr>");
                html.AppendLine("</thead>");
                html.AppendLine("<tbody>");
                foreach (Attendant a in CurrentDoc.Attendants)
                    html.Append("<tr><td>" + a.Name + "</td><td>" + a.Company + "</td></tr>");
                html.AppendLine("</tbody>");
                html.AppendLine("</table>");
            }
            // Texto
            html.AppendLine("<pre>" + CurrentDoc.DocText + "</pre>");
            // Listar anexos
            if (CurrentDoc.Annexes.Count > 0)
            {
                html.AppendLine("<table class='tb'>");
                html.AppendLine("<caption style='font-size: 16;font-weight: bold;'>" + Texts.ANNEXES + " </caption>");
                html.AppendLine("<thead>");
                html.AppendLine("<tr><th>" + Texts.NAME + " </th><th>" + Texts.SIZE + " KB</th></tr>");
                html.AppendLine("</thead>");
                html.AppendLine("<tbody>");
                foreach (Annex a in CurrentDoc.Annexes)
                    html.Append("<tr><td>" + a.FileName + "</td><td align=right>" + (a.Size / 1024).ToString("N0") + "</td></tr>");
                html.AppendLine("</tbody>");
                html.AppendLine("</table>");
            }   
            // Id
            html.AppendLine("<h3>KsIndexer Id: " + CurrentDoc.Id + "</h3>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }
    }
}

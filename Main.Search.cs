using ksindexer.Db;
using ksindexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KsIndexerNET
{
    partial class Main
    {
        // Metodo generico para mostrar lista de documentos encontrados y devolver el seleccionado, si lo hay
        private int ShowDocList(List<string[]> docs)
        {
            if (docs.Count == 0)
                return 0;
            // Mostrar la lista de los encontrados
            DlgSelectDoc sdlg = new DlgSelectDoc(docs);
            if (sdlg.ShowDialog() == DialogResult.Cancel)
                return 0;
            return sdlg.GetSelectedId();
        }

        private void SearchByDate()
        {
            DlgInput1 dlg = new DlgInput1("Introduzca la fecha como DD/MM/YYYY");
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string fecha = dlg.GetText1();
            dlg.Dispose();
            DateTime dfecha;
            if (fecha.Trim().Length == 0 || !DateTime.TryParse(fecha, out dfecha))
            {
                Messages.ShowError("Debe introducir una fecha válida");
                return;
            }
            // Buscar en BD
            List<string[]> docs = Document.GetByDate(dfecha);
            if (docs.Count == 0)
            {
                Messages.ShowWarning("No se han encontrado documentos esa fecha");
                return;
            }
            // Mostrar la lista de los encontrados y seleccionar uno
            int selectedId = ShowDocList(docs);
            if (selectedId == 0)
                return;
            // Cargar documento seleccionado
            ClearAll();
            Document doc = Document.Load(selectedId);
            if (doc == null)
            {
                Messages.ShowError("Error accediendo al documento con id: " + selectedId);
                return;
            }
            CurrentDoc = doc;
            FillControlsFromDoc();
            DocChanged = false;
            DocEmpty = false;
            EnableControls();
        }

        private void SearchById()
        {
            // Verificar si hay cambios sin guardar
            if (DocChanged && !Messages.AskDocChanged())
                return;
            // Pedir el id del documento a buscar
            DlgInput1 dlg = new DlgInput1("Introduzca el id del documento a buscar");
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string id = dlg.GetText1();
            dlg.Dispose();
            int nid;
            if (id.Length == 0 || !Int32.TryParse(id, out nid) || nid < 1)
            {
                Messages.ShowError("Debe introducir un id numérico mayor que cero");
                return;
            }
            ClearAll();
            Document doc = Document.Load(nid);
            if (doc == null)
            {
                Messages.ShowError("No se ha encontrado el documento con id: " + nid);
                return;
            }
            CurrentDoc = doc;
            FillControlsFromDoc();
            DocChanged = false;
            DocEmpty = false;
            EnableControls();
        }

        private void SearchComplex()
        {
            DlgSearchComplex dlg = new DlgSearchComplex();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string titulo = FileUtils.NormalizeString(dlg.GetTitulo());
            string fechaDesde = dlg.GetFechaDesde().Trim();
            string fechaHasta = dlg.GetFechaHasta().Trim();
            DateTime dFechaDesde = DateTime.TryParse(fechaDesde, out dFechaDesde) ? dFechaDesde : DateTime.MinValue;
            DateTime dFechaHasta = DateTime.TryParse(fechaHasta, out dFechaHasta) ? dFechaHasta : DateTime.MinValue;
            string claveraw = dlg.GetClaves().Trim();
            bool clavesTodas = dlg.GetClavesTodas();
            string asistente = FileUtils.NormalizeString(dlg.GetAsistente());
            string empresa = FileUtils.NormalizeString(dlg.GetEmpresa());
            dlg.Dispose();
            // Preparar SQL
            StringBuilder sql = new StringBuilder("SELECT DISTINCT id, date, title FROM Documents d, Doc_Keywords k, Doc_Attendants a WHERE ");
            sql.Append("d.Id = k.DocId AND d.id = a.DocId ");
            if (titulo.Length > 0)
            {
                // V 1.1 FNG 2024-01-26 : Buscar en el titulo normalizado
                sql.Append("AND d.titlenorm LIKE '%" + titulo + "%' ");
            }
            if (dFechaDesde != DateTime.MinValue)
            {
                sql.Append("AND d.date >= '" + dFechaDesde.ToString("yyyy-MM-dd") + "' ");
            }
            if (dFechaHasta != DateTime.MinValue)
            {
                sql.Append("AND d.date <= '" + dFechaHasta.ToString("yyyy-MM-dd") + "' ");
            }
            if (asistente.Length > 0)
            {
                sql.Append("AND a.name LIKE '%" + asistente + "%' ");
            }
            if (empresa.Length > 0)
            {
                sql.Append("AND a.company LIKE '%" + empresa + "%' ");
            }
            if (claveraw.Length > 0)
            {
                string[] claves = claveraw.Split(' ');
                if (clavesTodas)
                {
                    foreach (string clave in claves)
                    {
                        sql.Append("AND k.keyword = '" + FileUtils.NormalizeString(clave) + "' ");
                    }
                }
                else
                {
                    sql.Append("AND (");
                    foreach (string clave in claves)
                    {
                        sql.Append("k.keyword = '" + FileUtils.NormalizeString(clave) + "' OR ");
                    }
                    // Quitar el ultimo OR
                    sql.Remove(sql.Length - 3, 3);
                    sql.Append(") ");
                }
            }
            sql.Append("ORDER BY d.date");
            // Ejecutar SQL
            List<string[]> docs = Document.GetBySql(sql.ToString());
            if (docs.Count == 0)
            {
                Messages.ShowWarning("No se han encontrado documentos con esas condiciones");
                return;
            }
            // Mostrar la lista de los encontrados y seleccionar uno
            int selectedId = ShowDocList(docs);
            if (selectedId == 0)
                return;
            // Cargar documento seleccionado
            ClearAll();
            Document doc = Document.Load(selectedId);
            if (doc == null)
            {
                Messages.ShowError("Error accediendo al documento con id: " + selectedId);
                return;
            }
            CurrentDoc = doc;
            FillControlsFromDoc();
            DocChanged = false;
            DocEmpty = false;
            EnableControls();
        }
    }
}

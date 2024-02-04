using KsIndexerNET.Db;
using KsIndexerNET;
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
            DlgInput1 dlg = new DlgInput1(Texts.ENTER_DATE);
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string fecha = dlg.GetText1();
            dlg.Dispose();
            DateTime dfecha = LangUtils.ParseDateOnly(fecha);
            if (fecha.Trim().Length == 0 || dfecha == DateTime.MinValue)
            {
                Messages.ShowError(Texts.WRONG_DATE_FORMAT);
                return;
            }
            // Buscar en BD
            List<string[]> docs = Document.GetByDate(dfecha);
            if (docs.Count == 0)
            {
                Messages.ShowWarning(Texts.NO_DOCUMENT_FOUND);
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
                Messages.ShowError(Texts.ERROR_RETRIEVING_DOC_WITH_ID + ": " + selectedId);
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
            DlgInput1 dlg = new DlgInput1(Texts.ENTER_ID_TO_SEARCH);
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string id = dlg.GetText1();
            dlg.Dispose();
            int nid;
            if (id.Length == 0 || !Int32.TryParse(id, out nid) || nid < 1)
            {
                Messages.ShowError(Texts.WRONG_ID_ENTERED);
                return;
            }
            ClearAll();
            Document doc = Document.Load(nid);
            if (doc == null)
            {
                Messages.ShowError(Texts.DOC_ID_NOT_FOUND + ": " + nid);
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
            // Pedir los datos de busqueda
            DlgSearchComplex dlg = new DlgSearchComplex();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string titulo = FileUtils.NormalizeString(dlg.GetTitulo());
            string fechaDesde = dlg.GetFechaDesde().Trim();
            string fechaHasta = dlg.GetFechaHasta().Trim();
            DateTime dFechaDesde = LangUtils.ParseDateTime(fechaDesde);
            DateTime dFechaHasta = LangUtils.ParseDateTime(fechaHasta);
            string claveraw = dlg.GetClaves().Trim();
            bool clavesTodas = dlg.GetClavesTodas();
            string asistente = FileUtils.NormalizeString(dlg.GetAsistente());
            string empresa = FileUtils.NormalizeString(dlg.GetEmpresa());
            dlg.Dispose();
            // Si no han rellenado nada, salir
            if (titulo.Length == 0 && fechaDesde.Length == 0 && fechaHasta.Length == 0 && claveraw.Length == 0 && asistente.Length == 0 && empresa.Length == 0)
            {
                Messages.ShowWarning(Texts.SOME_CRITERIA_REQUIRED);
                return;
            }
            // Preparar SQL
            StringBuilder sql = new StringBuilder("SELECT DISTINCT d.id, d.date, d.title FROM Documents d, Doc_Keywords k, Doc_Attendants a ");
            //
            // 1) Si existe condicion por clave, es la mas rápida, porque va por indice
            //
            if (claveraw.Length > 0)
            {
                string[] claves = claveraw.Split(' ');
                if (claves.Length < 2)
                {
                    // Solo una clave, no hace falta el OR ni el AND adicionales
                    sql.Append("WHERE d.Id = k.DocId AND k.keyword = '" + FileUtils.NormalizeString(claves[0]) + "' AND ");
                }
                else
                {
                    // Palabras unidas por AND o por OR?
                    if (clavesTodas)
                    {
                        // Clausulas JOIN adicionales
                        int i = 0;
                        foreach (string clave in claves)
                        {
                            i++;
                            sql.Append("JOIN Doc_Keywords k" + i + " ON d.Id = k" + i + ".DocId ");
                        }
                        // Clausulas where contra los joins
                        sql.Append("WHERE ");
                        i = 0;
                        foreach (string clave in claves)
                        {
                            i++;
                            sql.Append("k" + i + ".keyword = '" + FileUtils.NormalizeString(clave) + "' AND ");
                        }
                    }
                    else
                    {
                        sql.Append("WHERE ");
                        // Como tenenmos un join, el OR funciona perfectamente
                        sql.Append("d.Id = k.DocId AND (");
                        foreach (string clave in claves)
                        {
                            sql.Append("k.keyword = '" + FileUtils.NormalizeString(clave) + "' OR ");
                        }
                        // Quitar el ultimo OR
                        sql.Remove(sql.Length - 3, 3);
                        sql.Append(") AND ");
                    }
                }
            }
            else
            {
                // No hay claves, necesitamos el where, para el resto de cláusulas
                sql.Append("WHERE ");
            }
            //
            // 2) El siguiente mas rapido es por fecha
            //
            if (dFechaDesde != DateTime.MinValue)
            {
                sql.Append("d.date >= '" + dFechaDesde.ToString("yyyy-MM-dd") + "' AND ");
            }
            if (dFechaHasta != DateTime.MinValue)
            {
                sql.Append("d.date <= '" + dFechaHasta.ToString("yyyy-MM-dd") + " 23:59' AND ");
            }
            //
            // 3) El siguiente mas rapido es por titulo
            //
            if (titulo.Length > 0)
            {
                sql.Append("d.titlenorm LIKE '%" + titulo + "%' AND ");
            }
            //
            // 4) Si hay asistente o empresa, hay que hacer un join con la tabla de asistentes
            //
            if (asistente.Length > 0 || empresa.Length > 0)
            {
                sql.Append("d.Id = a.DocId AND ");
            }
            //
            // 4.1) Si hay asistente, agregar filtro
            //
            if (asistente.Length > 0)
            {
                sql.Append("a.name LIKE '%" + asistente + "%' AND ");
            }
            //
            // 4.2) Si hay empresa, agregar filtro
            //
            if (empresa.Length > 0)
            {
                sql.Append("a.company LIKE '%" + empresa + "%' AND ");
            }
            // Quitar el ultimo AND 
            sql.Remove(sql.Length - 4, 4);
            //
            // 5) Ordenar por fecha
            //
            sql.Append("ORDER BY d.date");
            // Ejecutar SQL
            List<string[]> docs = Document.GetBySql(sql.ToString());
            if (docs.Count == 0)
            {
                Messages.ShowWarning(Texts.NO_DOCUMENT_FOUND);
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
                Messages.ShowError(Texts.ERROR_RETRIEVING_DOC_WITH_ID + ": " + selectedId);
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

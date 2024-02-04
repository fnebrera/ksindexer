using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Reflection;


namespace KsIndexerNET
{
    internal class LangUtils
    {
        /*
        public static void TranslateFormOld(Form form, string subdir = null)
        {
            // Buscamos el archivo de recursos del formulario, si existe
            string fullpath = form.Name;
            if (subdir != null)
                fullpath = subdir + @"\" + fullpath;
            //string resxFile = @".\" + fullpath + "." + Thread.CurrentThread.CurrentUICulture + ".resx";
            string resxFile = fullpath + "." + Thread.CurrentThread.CurrentUICulture + ".resx";
            try
            {
                ResXResourceSet resxSet = new ResXResourceSet(resxFile);
                if (resxSet != null)
                {
                    //
                    // Texto en el formulario
                    //
                    string maintext = resxSet.GetString(form.Name);
                    if (maintext != null)
                    {
                        // Si existe, lo asignamos al control
                        form.Text = maintext;
                    }
                    //
                    // Recorremos todos los controles del formulario
                    //
                    foreach (Control control in form.Controls)
                    {
                        switch (control.GetType().Name)
                        {
                            case "ToolStrip":
                                // Barra de herramientas
                                foreach (ToolStripButton item in ((ToolStrip)control).Items)
                                {
                                    string ctext = resxSet.GetString(item.Name);
                                    if (ctext != null)
                                        item.ToolTipText = ctext;
                                }
                                break;
                            case "MenuStrip":
                                // Menu principal
                                foreach (ToolStripItem item in ((MenuStrip)control).Items)
                                    TranslateMenuItem(item, resxSet);
                                break;
                            case "ListView":
                                // Traducimos las cabeceras de las columnas.
                                // El dato Name no se conserva en la coleccion, por lo que usamos
                                // el nombre del ListView mas el indice de la columna
                                foreach (ColumnHeader col in ((ListView)control).Columns)
                                {
                                    string ctext = resxSet.GetString(control.Name + col.Index);
                                    if (ctext != null)
                                        col.Text = ctext;
                                }
                                break;
                            case "ComboBox":
                                // Las entradas del combobox son simplemente strings
                                ComboBox cb = (ComboBox)control;
                                for (int i = 0; i < cb.Items.Count; i++)
                                {
                                    string ctext = resxSet.GetString(control.Name + i);
                                    if (ctext != null)
                                        cb.Items[i] = ctext;
                                }
                                break;
                            default:
                                // El resto de controles tienen un texto asociado
                                string text = resxSet.GetString(control.Name);
                                if (text != null)
                                    control.Text = text;
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Messages.ShowError("Error al cargar los recursos del formulario " + resxFile);
                // No existe el archivo de recursos, no hacemos nada
                // y el formulario queda con los textos de diseño
            }
        }
        */

        public static void TranslateForm(Form form)
        {
            // Buscamos el archivo incrustado de recursos del formulario, si existe. Debe llamarse igual que el formulario,
            // incluyendo el namespace.
            string resxFile = form.GetType().FullName;
            try
            {
                ResourceManager resxSet = new ResourceManager(resxFile, Assembly.GetExecutingAssembly());
                if (resxSet != null)
                {
                    //
                    // Texto en el formulario. GetString tiene ya en cuenta el CultureInfo actual
                    //
                    string maintext = resxSet.GetString(form.Name);
                    if (maintext != null)
                    {
                        // Si existe, lo asignamos al control
                        form.Text = maintext;
                    }
                    //
                    // Recorremos todos los controles del formulario
                    //
                    foreach (Control control in form.Controls)
                    {
                        switch (control.GetType().Name)
                        {
                            case "ToolStrip":
                                // Barra de herramientas
                                foreach (ToolStripButton item in ((ToolStrip)control).Items)
                                {
                                    string ctext = resxSet.GetString(item.Name);
                                    if (ctext != null)
                                        item.ToolTipText = ctext;
                                }
                                break;
                            case "MenuStrip":
                                // Menu principal
                                foreach (ToolStripItem item in ((MenuStrip)control).Items)
                                    TranslateMenuItem(item, resxSet);
                                break;
                            case "ListView":
                                // Traducimos las cabeceras de las columnas.
                                // El dato Name no se conserva en la coleccion, por lo que usamos
                                // el nombre del ListView mas el indice de la columna
                                foreach (ColumnHeader col in ((ListView)control).Columns)
                                {
                                    string ctext = resxSet.GetString(control.Name + col.Index);
                                    if (ctext != null)
                                        col.Text = ctext;
                                }
                                break;
                            case "ComboBox":
                                // Las entradas del combobox son simplemente strings
                                ComboBox cb = (ComboBox)control;
                                for (int i = 0; i < cb.Items.Count; i++)
                                {
                                    string ctext = resxSet.GetString(control.Name + i);
                                    if (ctext != null)
                                        cb.Items[i] = ctext;
                                }
                                break;
                            default:
                                // El resto de controles tienen un texto asociado
                                string text = resxSet.GetString(control.Name);
                                if (text != null)
                                    control.Text = text;
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Messages.ShowError("Error al cargar los recursos del formulario " + resxFile);
                // No existe el archivo de recursos, no hacemos nada
                // y el formulario queda con los textos de diseño
            }
        }

        // private static void TranslateMenuItem(ToolStripItem item, ResXResourceSet resxSet)
        private static void TranslateMenuItem(ToolStripItem item, ResourceManager resxSet)
        {
            // Buscamos el texto de cada control en el archivo de recursos
            string text = resxSet.GetString(item.Name);
            if (text != null)
            {
                // Si existe, lo asignamos al control
                item.Text = text;
            }
            if (item is ToolStripMenuItem)
            {
                // Si es un menu, recorremos todos los subitems
                foreach (ToolStripItem subitem in ((ToolStripMenuItem)item).DropDownItems)
                {
                    TranslateMenuItem(subitem, resxSet);
                }
            }
        }

        /*
        public static string TranslateTxt(Form form, string key)
        {
            return TranslateTxt(form.GetType().FullName, key);
        }

        public static string TranslateTxt(string resfile, string key)
        {
            // Texto por defecto si no se encuentra
            string ret = "!!resource not found!!";
            try
            {
                ResourceManager resxSet = new ResourceManager(resfile, Assembly.GetExecutingAssembly());
                if (resxSet == null)
                    return ret;
                // Texto solicitado
                string text = resxSet.GetString(key);
                if (text != null)
                    return text;
            }
            catch (Exception)
            {
                // No existe el archivo de recursos, hay que devolver una cadena de error
            }
            return ret;
        }
        */

        //
        // Formato de fechas en base a los settings del usuario
        // No empleamos el formateo por defecto de DateTime, porque no nos permite
        // formatear a gusto del usuario, no exactamente con el Locale. P.e. el usuario
        // quiere un locale en español, pero con el formato de fecha en ANSI.
        //

        public static DateTime ParseDateTime(string indate)
        {
            // Parsear la fecha en el formato actual, con numeros parciales, si es necesario
            if (indate.Trim().Length == 0)
                return DateTime.MinValue;
            if (DateTime.TryParseExact(NormalizeDateTime(indate), Main.CurrentDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                return dt;
            return DateTime.MinValue;
        }

        // Usar cuando solo queremos parsear fecha, es decir NO queremos en entren hora
        public static DateTime ParseDateOnly(string indate)
        {
            // Parsear la fecha en el formato actual, con numeros parciales, si es necesario,
            // pero sin horas ni minutos.
            if (indate.Trim().Length == 0)
                return DateTime.MinValue;
            // Si tiene hora, no parsear
            if (indate.Split(' ').Length > 1)
                return DateTime.MinValue;
            if (DateTime.TryParseExact(NormalizeDateTime(indate), Main.CurrentDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                return dt;
            return DateTime.MinValue;
        }

        public static string FormatDate(DateTime date)
        {
            // Formatear la fecha en el formato actual
            return date.ToString(Main.CurrentDateFormat);
        }
        public static string FormatDateTime(DateTime date)
        {
            // Formatear la fecha y hora en el formato actual
            return date.ToString(Main.CurrentDateTimeFormat);
        }

        public static string NormalizeDateTime(string datetime)
        {
            // Retornar minvalue si no se puede parsear
            string ret = FormatDateTime(DateTime.MinValue);
            if (datetime.Trim().Length == 0)
                return ret;
            char sep = Main.CurrentDateFormat.Contains('/') ? '/' : '-';
            string sdat;
            string stime = "";
            int mins = 0, hours = 0;
            string[] adatetime = datetime.Trim().Split(' ');
            sdat = adatetime[0];
            if (adatetime.Length > 1)
                stime = adatetime[1];
            string[] adat = sdat.Split(sep);
            if (adat.Length != 3)
                return ret;
            // Parsear componentes de la fecha
            int.TryParse(adat[0], out int f1);
            int.TryParse(adat[1], out int f2);
            int.TryParse(adat[2], out int f3);
            if (stime.Length > 0)
            {
                // Parsear la hora
                string[] atime = stime.Split(':');
                int.TryParse(atime[0], out hours);
                if (atime.Length > 1)
                    int.TryParse(atime[1], out mins);
            }
            // Formatear en base a los componentes parseados, y en funcion del separador
            string outdate;
            if (sep == '/')
            {
                // El f3 es el año. Intentar ajustar valores tipo 24 o 56
                if (f3 < 100)
                {
                    if (f3 < 50)
                        f3 += 2000;
                    else
                        f3 += 1900;
                }
                outdate = string.Format("{0:00}/{1:00}/{2:0000} {3:00}:{4:00}", f1, f2, f3, hours, mins);
            }
            else
            {
                // El f1 es el año
                if (f1 < 100)
                {
                    if (f1 < 50)
                        f1 += 2000;
                    else
                        f1 += 1900;
                }
                outdate = string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}", f1, f2, f3, hours, mins);
            }
            return outdate;
        }

        public static string ConvertDateFormat(string date, string from, string to)
        {
            if (date.Trim().Length == 0)
                return date;
            // Convertir una fecha de un formato a otro
            DateTime.TryParseExact(date, from, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt);
            if (dt == DateTime.MinValue)
                return date;
            return dt.ToString(to);
        }
    }
}

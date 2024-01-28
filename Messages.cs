using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ksindexer
{
    internal class Messages
    {
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowInfo(string message)
        {
            MessageBox.Show(message, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool AskDocChanged(string addtext = "")
        {
            string message = "El documento actual ha cambiado";
            if (addtext != "")
            {
                message += "\n" + addtext;
            }
            else
            {
                message += "\n¿Desea continuar sin guardar?";
            }
            DialogResult result = MessageBox.Show(message, "Pregunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        public static bool Confirm(string message)
        {
            DialogResult result = MessageBox.Show(message, "Pregunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        public static bool CanExit(bool changed)
        {
            bool salir;
            if (changed)
                salir = Messages.AskDocChanged();
            else
                salir = Messages.Confirm("¿Está seguro de salir de la Aplicación?");
            return salir;
        }
    }
}

using KsIndexerNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KsIndexerNET
{
    internal class Messages
    {
        public static void ShowError(string message)
        {
            MessageBox.Show(message, Texts.DLG_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, Texts.DLG_WARN, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowInfo(string message)
        {
            MessageBox.Show(message, Texts.DLG_INFO, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool AskDocChanged(string addtext = "")
        {
            string message = Texts.DOC_HAS_CHANGED;
            if (addtext != "")
            {
                message += "\n" + addtext;
            }
            else
            {
                message += "\n" + Texts.PROCEED_WITHOUT_SAVE;
            }
            DialogResult result = MessageBox.Show(message, Texts.DLG_CONFIRM, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        public static bool Confirm(string message)
        {
            DialogResult result = MessageBox.Show(message, Texts.DLG_CONFIRM, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        public static bool CanExit(bool changed)
        {
            bool salir;
            if (changed)
                salir = Messages.AskDocChanged();
            else
                salir = Messages.Confirm(Texts.CONFIRM_EXIT_APP);
            return salir;
        }
    }
}

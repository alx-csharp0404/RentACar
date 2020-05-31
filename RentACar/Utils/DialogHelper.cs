using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentACar
{
    static class DialogHelper
    {
        public static void E(String message)
        {
            MessageBox.Show(message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graffin
{
    static class Program
    {
        static Ventana ventana;
        public static Ventana getVentana()
        {
            return ventana;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ventana = new Ventana();
            Application.Run(ventana);
        }
    }
}

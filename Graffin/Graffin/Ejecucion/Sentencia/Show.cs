using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graffin.Ejecucion.Sentencia
{
    class Show
    {
        public Show(Expresion titulo, Expresion contenido)
        {
            try
            {
                
                MessageBox.Show(contenido.respuesta.ToString(), titulo.respuesta.ToString());
            }
            catch(Exception e)
            {
                Program.getVentana().agregarError("Show no irve","Semantico",-1,-1,"TOkenazo");
            }
        }
    }
}

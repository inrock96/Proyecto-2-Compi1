using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graffin.Ejecucion.Sentencia
{
    class Print
    {
        public Print(Expresion exp)
        {
            try
            {

                Program.getVentana().appendSalida(exp.respuesta.ToString()+"\n");
                Console.Write(exp.respuesta.ToString());
            }catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}

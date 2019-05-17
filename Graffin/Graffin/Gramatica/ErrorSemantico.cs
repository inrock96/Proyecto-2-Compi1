using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graffin.Gramatica
{
    class ErrorSemantico
    {
        public string mensaje, tipo;
        public int fila, columna;
        public string token;
        public ErrorSemantico(string mensaje,string tipo,int fila,int columna,string token)
        {
            this.mensaje = mensaje;
            this.tipo = tipo;
            this.fila = fila;
            this.columna = columna;
        }
    }
}

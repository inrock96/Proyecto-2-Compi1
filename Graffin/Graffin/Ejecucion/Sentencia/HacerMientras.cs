using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion.Sentencia
{
    class HacerMientras
    {
        ParseTreeNode nodo;
        TablaSimbolos actual;
        TablaFunciones funciones;
        public object respuesta;
        public HacerMientras(ParseTreeNode nodo, TablaSimbolos actual, TablaFunciones funciones)
        {
            this.actual = new TablaSimbolos(actual);
            this.funciones = funciones;
            this.nodo = nodo;
        }
        public void ejecutar()
        {
            Expresion condicion = new Expresion(nodo.ChildNodes[3], funciones);
            condicion.ejecutar(actual, funciones);
            if (condicion.respuesta != null)
            {
                if (condicion.respuesta is bool)
                {
                    REGRESAR:
                    Bloque b = new Bloque(nodo.ChildNodes[1], actual, funciones);
                    b.ejecutar(actual);
                    condicion.ejecutar(this.actual, funciones);
                
                    if (b.retorno == true)
                    {
                        respuesta = b.respuesta;
                        goto NOREGRESAR;
                    }
                    else if (b.romper)
                    {
                        goto NOREGRESAR;
                    }
                    else if (b.continuar)
                    {
                        goto REGRESAR;
                    }
                    condicion.ejecutar(actual, funciones);
                    if((bool)condicion.respuesta)
                        goto REGRESAR;
                
                    NOREGRESAR:;
                }
                else
                {
                    Program.getVentana().agregarError("Error, no es bool", "Semantico", -1, -1, "");
                }
            }
            {
                Program.getVentana().agregarError("Error, es null ", "Semantico", -1, -1, "");

            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion.Sentencia
{
    class Repetir
    {
        ParseTreeNode nodo;
        TablaSimbolos actual;
        TablaFunciones funciones;
        public object respuesta;
        public Repetir(ParseTreeNode nodo, TablaSimbolos actual, TablaFunciones funciones)
        {
            this.actual = new TablaSimbolos(actual);
            this.funciones = funciones;
            this.nodo = nodo;
        }
        public void ejecutar()
        {
            Expresion condicion = new Expresion(nodo.ChildNodes[1], funciones);
            condicion.ejecutar(actual, funciones);

            if (condicion.respuesta != null)
            {
                if (condicion.respuesta is int)
                {
                    int i = 0;
                    REGRESAR:

                    if (i < (int)condicion.respuesta)
                    {
                        Bloque b = new Bloque(nodo.ChildNodes[2], actual, funciones);
                        b.ejecutar(actual);
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
                        i++;
                        goto REGRESAR;
                    }
                    NOREGRESAR:;
                }
                else
                {
                    Program.getVentana().agregarError("Error,no es int ", "Semantico", -1, -1, "");
                }
            }
            else
            {
                Program.getVentana().agregarError("Error, es null", "Semantico", -1, -1, "");
            }


        }
    }
}

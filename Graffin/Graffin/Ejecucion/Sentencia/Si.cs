using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion.Sentencia
{
    class Si
    {
        TablaSimbolos actual;
        ParseTreeNode nodo;
        public object respuesta;
        TablaFunciones funciones;
        public Si(ParseTreeNode nodo,TablaFunciones funciones)
        {
            this.nodo = nodo;
            this.funciones = funciones;
        }
        public void ejecutar(TablaSimbolos local)
        {
            actual = new TablaSimbolos(local);
            ejecutar(actual, nodo);
        }
        private void ejecutar(TablaSimbolos actual,ParseTreeNode raiz)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                //sin else 
                Expresion condicion = new Expresion(nodo.ChildNodes[1], funciones);
                condicion.ejecutar(actual, funciones);
                if (condicion.respuesta != null)
                {
                    if (condicion.respuesta is bool)
                    {
                        if ((bool)condicion.respuesta)
                        {
                            Bloque b = new Bloque(nodo.ChildNodes[2], actual, funciones);
                            b.ejecutar(actual);
                            respuesta = b.respuesta;
                        }

                    }
                    else
                    {
                        Program.getVentana().agregarError("Error, no es bool ", "Semantico", -1, -1, "");
                    }
                }
                else
                {
                    Program.getVentana().agregarError("Error, es null ", "Semantico", -1, -1, "");
                }

            }
            else
            {
                //con else :V
                Expresion condicion = new Expresion(nodo.ChildNodes[1], funciones);
                condicion.ejecutar(actual, funciones);
                if(condicion.respuesta is bool)
                {
                    if ((bool)condicion.respuesta)
                    {
                        Bloque b = new Bloque(nodo.ChildNodes[2], actual, funciones);
                        b.ejecutar(actual);
                        respuesta = b.respuesta;
                    }
                    else
                    {
                        Bloque b = new Bloque(nodo.ChildNodes[4], actual, funciones);
                        b.ejecutar(actual);
                        respuesta = b.respuesta;
                    }
                }
                else
                {
                    Program.getVentana().agregarError("Error, no es bool", "Semantico", -1, -1, "");
                }
            }
        }
    }
}

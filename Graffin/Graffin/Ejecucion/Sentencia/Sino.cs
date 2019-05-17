using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion.Sentencia
{
    class Sino
    {
        TablaSimbolos actual;
        ParseTreeNode nodo;
        TablaFunciones funciones;
        public object respuesta;
        public Sino(ParseTreeNode nodo, TablaFunciones funciones)
        {
            this.nodo = nodo;
            this.funciones = funciones;
        }
        public void ejecutar(TablaSimbolos local)
        {
            actual = new TablaSimbolos(local);
            ejecutar(actual, nodo);
        }
        private void ejecutar(TablaSimbolos actual, ParseTreeNode raiz)
        {
            if (nodo.ChildNodes.Count == 4)
            {
                //sin else 
                Expresion condicion = new Expresion(nodo.ChildNodes[1], funciones);
                condicion.ejecutar(actual, funciones);
                if (condicion.respuesta is bool)
                {
                    if ((bool)condicion.respuesta)
                    {
                        Bloque b = new Bloque(nodo.ChildNodes[2], actual, funciones);
                        b.ejecutar(actual);
                        respuesta = b.respuesta;
                    }
                    else
                    {
                        //Recorrer else ifs
                        foreach (ParseTreeNode elseif in nodo.ChildNodes[3].ChildNodes)
                        {
                            Expresion cond = new Expresion(elseif.ChildNodes[2],funciones);
                            cond.ejecutar(actual, funciones);
                            if(cond.respuesta is bool)
                            {
                                if ((bool)cond.respuesta)
                                {
                                    Bloque b = new Bloque(elseif.ChildNodes[3], actual, funciones);
                                    b.ejecutar(actual);
                                    respuesta = b.respuesta;
                                }
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error, no es bool ", "Semantico", -1, -1, "");
                            }
                        }
                    }

                }
                else
                {
                    Program.getVentana().agregarError("Error, if else no es bool", "Semantico", -1, -1, "");
                }
            }
            else
            {
                //con else :V
                Expresion condicion = new Expresion(nodo.ChildNodes[1],funciones);
                bool hayElse = true;
                condicion.ejecutar(actual, funciones);
                if (condicion.respuesta is bool)
                {
                    if ((bool)condicion.respuesta)
                    {
                        Bloque b = new Bloque(nodo.ChildNodes[2], actual, funciones);
                        b.ejecutar(actual);
                        respuesta = b.respuesta;
                        hayElse = false;
                    }
                    else
                    {
                        //Recorrer else ifs
                        foreach (ParseTreeNode elseif in nodo.ChildNodes[3].ChildNodes)
                        {
                            Expresion cond = new Expresion(elseif.ChildNodes[2],funciones);
                            cond.ejecutar(actual, funciones);
                            if (cond.respuesta is bool)
                            {
                                if ((bool)cond.respuesta)
                                {
                                    Bloque b = new Bloque(elseif.ChildNodes[3], actual, funciones);
                                    b.ejecutar(actual);
                                    respuesta = b.respuesta;
                                    hayElse = false;
                                    break;
                                }
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error, no es bool", "Semantico", -1, -1, "");
                            }
                        }
                        if (hayElse)
                        {
                            Bloque b = new Bloque(nodo.ChildNodes[5], actual, funciones);
                            b.ejecutar(actual);
                            respuesta = b.respuesta;
                        }
                    }

                }
                else
                {
                    Program.getVentana().agregarError("Error, no es bool ", "Semantico", -1, -1, "");
                }
            }
        }
    }
}

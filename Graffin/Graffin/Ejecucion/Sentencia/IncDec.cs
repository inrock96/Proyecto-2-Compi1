using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion.Sentencia
{
    class IncDec
    {
        ParseTreeNode nodo;
       public  object respuesta;
        public IncDec(ParseTreeNode nodo)
        {
            this.nodo = nodo;
            respuesta = null;
        }
        public void ejecutar(TablaSimbolos actual,TablaFunciones funciones)
        {
            if (nodo.ChildNodes[0].ChildNodes.Count == 2)
            {
                if (nodo.ChildNodes[0].Term.ToString().Equals("INCREMENTO"))
                {
                    if (nodo.ChildNodes[0].ChildNodes[0].Term.ToString().Equals("identificador"))
                    {
                        string id = nodo.ChildNodes[0].ChildNodes[0].Token.Text.ToLower();
                        if (actual.existe(id))
                        {
                            if (actual.sacar(id).dimension == 0)
                            {
                                Simbolo s = actual.sacar(id);
                                if (s.valor is int)
                                {
                                    s.valor = (int)s.valor + 1;
                                    actual.reemplazar(s.identificador, s);
                                    respuesta = s.valor;

                                }
                                else if (s.valor is double)
                                {
                                    s.valor = (double)s.valor + 1;
                                    actual.reemplazar(s.identificador, s);
                                    respuesta = s.valor;
                                }
                                else if (s.valor is char)
                                {
                                    s.valor = (char)((int)s.valor + 1);
                                    actual.reemplazar(s.identificador, s);
                                    respuesta = s.valor;
                                }
                                else
                                {
                                    Program.getVentana().agregarError("Error, tipo no compatible", "Semantico", -1, -1, "");
                                    respuesta = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        double result = Convert.ToDouble(nodo.ChildNodes[0].Token.Text);
                        try
                        {
                            int result2 = Convert.ToInt32(nodo.ChildNodes[0].Token.Text);
                            respuesta = result2 + 1;
                        }
                        catch (Exception)
                        {
                            respuesta = result + 1.0;
                        }
                    }
                }
                else
                {
                    if (nodo.ChildNodes[0].ChildNodes[0].Term.ToString().Equals("identificador"))
                    {
                        string id = nodo.ChildNodes[0].ChildNodes[0].Token.Text.ToLower();
                        if (actual.existe(id))
                        {
                            if (actual.sacar(id).dimension == 0)
                            {
                                Simbolo s = actual.sacar(id);
                                if (s.valor is int)
                                {
                                    s.valor = (int)s.valor - 1;
                                    actual.reemplazar(s.identificador, s);
                                    respuesta = s.valor;

                                }
                                else if (s.valor is double)
                                {
                                    s.valor = (double)s.valor - 1;
                                    actual.reemplazar(s.identificador, s);
                                    respuesta = s.valor;
                                }
                                else if (s.valor is char)
                                {
                                    s.valor = (char)((int)s.valor - 1);
                                    actual.reemplazar(s.identificador, s);
                                    respuesta = s.valor;
                                }
                                else
                                {
                                    Program.getVentana().agregarError("Error, tipo no compatible", "Semantico", -1, -1, "");
                                    respuesta = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        double result = Convert.ToDouble(nodo.ChildNodes[0].Token.Text);
                        try
                        {
                            int result2 = Convert.ToInt32(nodo.ChildNodes[0].Token.Text);
                            respuesta = result2 - 1;
                        }
                        catch (Exception)
                        {
                            respuesta = result - 1.0;
                        }
                    }
                }

            }
            else
            {

            }
        }
    }
}

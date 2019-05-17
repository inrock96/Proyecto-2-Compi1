using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Graffin.Ejecucion.Sentencia;

namespace Graffin.Ejecucion
{
    class Funcion : Simbolo
    {
        public ParseTreeNode nodo;
        public TablaSimbolos actual;
        public TablaFunciones tFunc;
        public List<Simbolo> listaParametros;
        public ParseTreeNode parametros;
        public List<object> atributos;
        public bool overRide;
        public string key;
        string idParams;
        
        public Funcion(string identificador, object valor, string tipo, int linea, int columna, int dimension,ParseTreeNode bloque,ParseTreeNode parametros) : base(identificador, valor, tipo, linea, columna, dimension)
        {
            idParams = "";
            this.identificador = identificador;
            this.valor = valor;
            this.tipo = tipo;
            this.linea = linea;
            atributos = new List<object>();
            this.nodo = bloque;
            overRide = false;
            listaParametros = new List<Simbolo>();
            this.parametros = parametros;
            setParametros(this.parametros);
            key = identificador + idParams;
            
        }
        public void setParametros(ParseTreeNode nodo)
        {
            this.listaParametros = new List<Simbolo>();
            if (nodo != null)
            {
                foreach (ParseTreeNode hijo in nodo.ChildNodes)
                {
                    if (hijo.ChildNodes.Count > 2)
                    {
                        //Es array
                        string id = hijo.ChildNodes[1].Token.Text.ToLower();
                        object valor = new object();
                        asignarTipo(hijo.ChildNodes[0]);
                        Arreglo s = new Arreglo(id, null, asignarTipo(hijo.ChildNodes[0]), -1, -1, hijo.ChildNodes[3].ChildNodes.Count);
                        s.setDimension(hijo.ChildNodes[2], this.actual, this.tFunc);
                        listaParametros.Add(s);
                        idParams += "array" + s.tipo;
                    }
                    else if(hijo.ChildNodes.Count==2)
                    {
                        string id = hijo.ChildNodes[1].Token.Text.ToLower();
                        asignarTipo(hijo.ChildNodes[0]);
                        if (asignarTipo(hijo.ChildNodes[0]) != "int" && asignarTipo(hijo.ChildNodes[0]) != "char" && asignarTipo(hijo.ChildNodes[0]) != "string" && asignarTipo(hijo.ChildNodes[0]) != "double" && asignarTipo(hijo.ChildNodes[0]) != "bool")
                        {
                            Objeto s = new Objeto(id, asignarTipo(hijo.ChildNodes[0]), -1, -1, 0,null,null);
                            listaParametros.Add(s);
                            idParams += s.tipo;
                        }
                        else
                        {
                            Simbolo s = new Simbolo(id, null, asignarTipo(hijo.ChildNodes[0]), -1, -1, 0);
                            listaParametros.Add(s);
                            idParams += s.tipo;
                        }

                    }
                    else
                    {
                        Program.getVentana().agregarError("Error desconocido" + hijo.ChildNodes[1].Term.ToString(), "Semantico", -1, -1, "");
                        break;
                    }

                }
            }
            

        }
        
        public void setAtributos(ParseTreeNode nodo,TablaSimbolos actual, TablaFunciones tFunc)
        {   
            if (nodo.ChildNodes.Count == listaParametros.Count)
            {
                int i = 0;
                foreach (ParseTreeNode hijo in nodo.ChildNodes)
                {
                    Expresion e = new Expresion(hijo, tFunc);
                    e.ejecutar(actual, tFunc);
                    if (e.tipo == listaParametros[i].tipo)
                    {
                        if (e.esObjeto)
                        {
                            Objeto o = (Objeto)e.respuesta;
                            Objeto swap = (Objeto)listaParametros[i];
                            swap.local = o.local;
                            swap.funciones = o.funciones;
                            listaParametros[i] = swap;
                            
                        }
                        else if (e.esArreglo)
                        {
                            if (listaParametros[i].dimension > 0)
                            {
                                Arreglo a = (Arreglo)e.respuesta;
                                Arreglo sap = (Arreglo)listaParametros[i];
                                sap.valores = a.valores;
                                sap.pagina = a.pagina;
                                sap.fila = a.fila;
                                sap.col = a.col;
                                if(sap.fila == a.fila && sap.col == a.col && sap.pagina == a.pagina)
                                {
                                    listaParametros[i] = sap;
                                }
                                else
                                {
                                    Program.getVentana().agregarError("Error, no tienen los mismos tamaños" , "Semantico", -1, -1, "");
                                    break;
                                }
                                
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error, no es arreglo" , "Semantico", -1, -1, "");
                                break;
                            }
                           
                        }
                        else
                        {
                            atributos.Add(e.respuesta);
                            listaParametros[i].valor = e.respuesta;                            
                            
                        }
                    }
                    else
                    {
                        Program.getVentana().agregarError("Error, tipos diferentes" , "Semantico", -1, -1, "");
                        break;
                    }                       
                    i++;
                }
            }
            else
            {
                Program.getVentana().agregarError("Error, no tienen la misma dimension", "Semantico", -1, -1, "");
            }

        }
        public void ejecutar(TablaSimbolos local, TablaFunciones tf)
        {
            tFunc = tf;
            //this.actual = new TablaSimbolos(local);
            this.actual = new TablaSimbolos(local);
            int i = 0;
            foreach(Simbolo s in listaParametros)
            {
                actual.agregar(s.identificador, s);
                i++;
            }
            Bloque b = new Bloque(nodo, actual, tFunc);
            b.ejecutar(actual);
            valor = b.respuesta;
        }
        
        public string asignarTipo(ParseTreeNode nodo)
        {
            if (nodo.Term.ToString().Equals("TIPO"))
            {

                ParseTreeNode hijo = nodo.ChildNodes[0];
                string op = hijo.Term.ToString().ToLower();

                if (op.Equals("int"))
                {
                    
                    valor = (int)0;
                    return tipo = "int";
                }
                else if (op.Equals("double"))
                {
                    
                    valor = (double)0.0;
                    return tipo = "double";
                    
                }
                else if (op.Equals("string"))
                {
                    
                    valor = (string)"";
                    return tipo = "string";
                }
                else if (op.Equals("char"))
                {
                    
                    valor = (char)'\u0000';
                   return tipo = "char";
                }
                else if (op.Equals("bool"))
                {
                    valor = false;
                   return tipo = "bool";
                    valor = (bool)false;
                }
                else if (op.Equals("void"))
                {
                    valor = null;
                    return tipo = "void";
                    valor = null;

                }
                else
                {
                    if (Ejecutor.tc.existe(op))
                    {
                        return tipo = op;
                        valor = null;
                    }
                    else
                    {
                        Program.getVentana().agregarError("Error, ese tipo no existe", "Semantico", -1, -1, "");
                    }
                }
            }
            else
            {
                string op = nodo.Token.Text.ToLower();
                if (Ejecutor.tc.existe(op))
                {
                   return tipo = op;
                }
                else
                {
                    Program.getVentana().agregarError("Error ese tipo no existe", "Semantico", -1, -1, "");
                }
            }
            return "";
        }
        public string getID()
        {
            
            return key;
        }
    }
}

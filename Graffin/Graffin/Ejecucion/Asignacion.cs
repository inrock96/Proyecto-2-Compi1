using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Graffin.Ejecucion.Sentencia;

namespace Graffin.Ejecucion
{
    class Asignacion
    {
        ParseTreeNode nodo;
        //TablaSimbolos actual;
        TablaFunciones tFunciones;
        public bool arreglo;
        List<Simbolo> listaVar;
        string tipo;
        object valor;
        public Asignacion(ParseTreeNode nodo,TablaFunciones tFunciones)
        {
            this.nodo = nodo;
            this.tFunciones = tFunciones;
            arreglo = false;
            tipo = "";
            valor = null;
            listaVar = new List<Simbolo>();
        }
        public void ejecutar(TablaSimbolos actual)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                if (nodo.ChildNodes[0].Term.ToString().Equals("IDARREGLO"))
                {
                    //ASIGNAR IDARREGLO
                    ParseTreeNode dimensiones = nodo.ChildNodes[0].ChildNodes[1];
                    string id = nodo.ChildNodes[0].ChildNodes[0].Token.Text.ToLower();
                    Expresion exp = new Expresion(nodo.ChildNodes[1],tFunciones);
                    exp.ejecutar(actual, tFunciones);
                    if (actual.existe(id))
                    {
                        Arreglo arreglo;
                        if (actual.sacar(id).dimension>0)
                        {
                            arreglo =(Arreglo) actual.sacar(id);
                            if (arreglo.tipo.Equals(exp.tipo))
                            {
                                arreglo.setValorU(exp,dimensiones, actual, tFunciones);
                                actual.reemplazar(arreglo.identificador, arreglo);
                            } 
                        }
                    }
                }
                else if (nodo.ChildNodes[0].Term.ToString().Equals("ACCESO"))
                {
                    //asignación a atributo objeto
                    Acceso acceso = new Acceso(nodo.ChildNodes[0].ChildNodes[0].Token.Text.ToLower(), nodo.ChildNodes[0].ChildNodes[1].Token.Text.ToLower());
                    Expresion exp = new Expresion(nodo.ChildNodes[1],tFunciones);
                    exp.ejecutar(actual, tFunciones);
                    if (acceso.getValor(actual) != null)
                    {
                        if (acceso.getValor(actual).tipo.Equals(exp.tipo))
                        {
                            Simbolo s = new Simbolo(acceso.getValor(actual).identificador, acceso.getValor(actual).tipo, acceso.getValor(actual).linea, acceso.getValor(actual).columna, 0);

                            s.visible = acceso.getValor(actual).visible;
                            if (s.visible)
                            {
                                s.valor = exp.respuesta;

                                acceso.setValor(actual, s);
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error, es privada", "Semantico", -1, -1, "");
                            }

                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, diferente tipo", "Semantico", -1, -1, "");
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    //ASIGNACION NORMAL
                    string id = nodo.ChildNodes[0].Token.Text.ToLower();
                    if (validarNodo(id,actual))
                    {
                        //Si existe entonces lo asigna
                        
                        if (setValor(nodo.ChildNodes[1],actual))
                        {
                            if (!arreglo)
                            {
                                //Si el valor es igual a la asignación entonces lo asigna
                                Simbolo s = actual.sacar(id);
                                if (s.esObjeto())
                                {
                                    Objeto o = (Objeto)actual.sacar(id);
                                    Objeto tmp = (Objeto)valor;
                                    Objeto nuevo = new Objeto(o.identificador, o.tipo, o.linea, o.columna, 0, tmp.local, tmp.funciones);
                                    actual.reemplazar(nuevo.identificador, nuevo);
                                }
                                else
                                {
                                    s.valor = this.valor;
                                    actual.reemplazar(s.identificador, s);
                                }
                                
                            }
                            else
                            {
                                Arreglo a = (Arreglo)actual.sacar(id);
                                if(valor is Arreglo)
                                {
                                    Arreglo tmp = (Arreglo)valor;
                                    a.valores = tmp.valores;
                                    actual.reemplazar(a.identificador, a);
                                }
                                else
                                {
                                    Program.getVentana().agregarError("Error, no es arreglo", "Semantico", -1, -1, "");
                                }

                                //a.getValor((Arreglo)valor); 
                            }
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, no se pudo asignar el valor", "Semantico", -1, -1, "");
                        }
                    }
                    else
                    {
                        Program.getVentana().agregarError("Error, no se pudo validar la variable", "Semantico", -1, -1, "");
                    }

                }
            }
            else
            {
                //iNICIALIZAR objeto
                string id = nodo.ChildNodes[0].Token.Text.ToLower();
                validarNodo(id,actual);
                Objeto nuevo;
                if (actual.sacar(id).esObjeto())
                {
                    nuevo = (Objeto)actual.sacar(id);
                    if (Ejecutor.tc.existe(nuevo.tipo))
                    {
                        
                        Clase c = Ejecutor.tc.sacar(id);
                        c.primerRecorrido();
                        TablaSimbolos nueva = new TablaSimbolos(null);
                        foreach(object key in c.global.listaSimbolos.Keys)
                        {
                            nueva.agregar(key.ToString(), c.global.sacar(key.ToString()));
                        }
                        
                        nuevo.funciones = c.funciones;
                        nuevo.local = nueva;
                        actual.reemplazar(nuevo.identificador, nuevo);
                    }
                    else
                    {
                        // error semantico
                    }
                }
            }
        }
        public bool validarNodo(string identificador,TablaSimbolos actual)
        {
            if (actual.existe(identificador))
            {
                tipo = actual.sacar(identificador).tipo;
                if (actual.sacar(identificador).dimension == 0)
                    arreglo = false;
                else
                    arreglo = true;
                
                return true;
            }
            else
            {
                Program.getVentana().agregarError("Error, no existe", "Semantico", -1, -1, "");
                return false;
            }
        }
        public bool setValor(ParseTreeNode EXP,TablaSimbolos actual)
        {
            Expresion e = new Expresion(EXP,tFunciones);
            e.ejecutar(actual, tFunciones);
            if(e.tipo == tipo)
            {
                valor = e.respuesta;
                
                return true;
            }
            else
            {
                if (retornarTerminal(EXP) != null)
                    Program.getVentana().agregarError("No se pudo asignar valor", "Semantico", retornarTerminal(EXP).Token.Location.Line, retornarTerminal(EXP).Token.Location.Column, retornarTerminal(EXP).Token.Text);
                else
                    Program.getVentana().agregarError("Error, diferente tipo", "Semantico", -1, -1, "");
                return false;
            }
        }
        ParseTreeNode retornarTerminal(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 0)
            {
                return nodo;
            }
            else
            {
                foreach (ParseTreeNode hijo in nodo.ChildNodes)
                {
                    return retornarTerminal(hijo);
                }
            }
            return null;
        }
    }
}

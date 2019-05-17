using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Graffin.Gramatica;

namespace Graffin.Ejecucion
{
    class Declaracion 
    {
        //Para asignar un arreglo, no puede salir del rango?
      
        public int dimension;
        List<Simbolo> listaVar;
        TablaFunciones tFunc;
        string  tipo;
        object defecto;
        ParseTreeNode nodo;
        bool visibilidad;

        public Declaracion(ParseTreeNode nodo,TablaFunciones tFunc)
        {
            this.nodo = nodo;
            dimension = 0;
            listaVar = new List<Simbolo>();
            this.tFunc = tFunc;

        }
        public void asignarTipo(ParseTreeNode nodo)
        {
            
            if (nodo.Term.ToString().Equals("TIPO"))
            {

                ParseTreeNode hijo = nodo.ChildNodes[0];
                string op = hijo.Term.ToString().ToLower();

                if (op.Equals("int"))
                {
                    tipo = "int";
                    defecto = (int)0;
                }
                else if (op.Equals("double"))
                {
                    tipo = "double";
                    defecto = (double)0.0;
                }
                else if (op.Equals("string"))
                {
                    tipo = "string";
                    defecto = (string)"";
                }
                else if (op.Equals("char"))
                {
                    tipo = "char";
                    defecto = (char)'\u0000';
                }
                else if (op.Equals("bool"))
                {
                    tipo = "bool";
                    defecto = (bool)false;
                }
                else if (op.Equals("void"))
                {
                    //Error, una variable no puede ser void
                    tipo = "void";
                    defecto = null;
                    Program.getVentana().agregarError("Error, una variable no puede ser void ", "Semantico", -1, -1, "");


                }
                else
                {
                    if (Ejecutor.tc.existe(op))
                    {
                        tipo = op;
                        defecto = null;
                    }
                    else
                    {
                        tipo = "null";
                        Program.getVentana().agregarError("Error que no existe la clase", "Semantico", -1, -1, "");
                    }
                }
            }
            else
            {
                string op = nodo.Token.Text.ToLower();
                if (Ejecutor.tc.existe(op))
                {
                    tipo = op;
                }
                else
                {
                    tipo = "null";
                    Program.getVentana().agregarError("Error, no existe esa onda ", "Semantico", -1, -1, "");
                }
            }
        }

        public void ejecutar(TablaSimbolos actual)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                //2
                asignarTipo(nodo.ChildNodes[0]);
                foreach(ParseTreeNode hijo in nodo.ChildNodes[1].ChildNodes)
                {
                    if(tipo.Equals("int")|| tipo.Equals("string")|| tipo.Equals("char")|| tipo.Equals("double")|| tipo.Equals("bool"))
                    {
                        Simbolo nuevo = new Simbolo(hijo.Token.Text.ToLower(), defecto, tipo, hijo.Token.Location.Line, hijo.Token.Location.Column, 0);
                        if (nuevo != null)
                        {
                            if (actual.existeActual(nuevo.identificador))
                            {
                                actual.agregar(nuevo.identificador, nuevo);
                            }
                            

                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, no existe  ", "Semantico", -1, -1, "");
                            break;
                        }
                    }
                    else
                    {
                        if (Ejecutor.tc.existe(tipo))
                        {
                            Objeto nuevo = new Objeto(hijo.Token.Text.ToLower(), tipo, hijo.Token.Location.Line, hijo.Token.Location.Column, 0, null, null);
                            actual.agregar(nuevo.identificador, nuevo);
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, no existe ese tipo  ", "Semantico", -1, -1, "");
                            break;
                        }
                        
                    }
                }        
            }
            else if (nodo.ChildNodes.Count == 3)
            {
                //3
                //2 de arriba pero con visibilidad
                if (nodo.ChildNodes[0].Term.ToString().Equals("VISIBILIDAD"))
                {
                    
                    asignarTipo(nodo.ChildNodes[1]);
                    asignarVisibilidad(nodo.ChildNodes[0]);
                    foreach(ParseTreeNode hijo in nodo.ChildNodes[2].ChildNodes)
                    {
                        if (tipo.Equals("int") || tipo.Equals("char") || tipo.Equals("double") || tipo.Equals("string") || tipo.Equals("bool"))
                        {
                            Simbolo nuevo = new Simbolo(hijo.Token.Text.ToLower(), defecto, tipo, hijo.Token.Location.Line, hijo.Token.Location.Column, 0);
                            nuevo.visible = visibilidad;
                            if (nuevo != null)
                            {
                                if (actual.existeActual(nuevo.identificador))
                                {
                                    actual.agregar(nuevo.identificador, nuevo);
                                }
                                else
                                {
                                    Program.getVentana().agregarError("Error, ya existe en este entorno", "Semantico", -1, -1, "");
                                    break;
                                }


                            }
                            else
                            {
                                Program.getVentana().agregarError("Error no se pudo crear simbolo ", "Semantico", -1, -1, "");
                            }
                        }
                        else
                        {
                            
                            
                            Objeto nuevo = new Objeto(hijo.Token.Text.ToLower(), tipo, hijo.Token.Location.Line, hijo.Token.Location.Column, 0, null, null);
                            nuevo.visible = visibilidad;
                            if (actual.existeActual(nuevo.identificador))
                            {
                                actual.agregar(nuevo.identificador, nuevo);
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error, ya existe en el entorno actual", "Semantico", -1, -1, "");
                            }

                        }
                    }
                    
                }
                else
                {
                    //Nodo asignado
                    Simbolo nuevo;
                    asignarTipo(nodo.ChildNodes[0]);
                    if (setValor(nodo.ChildNodes[2],actual))
                    {
                        foreach (ParseTreeNode hijo in nodo.ChildNodes[1].ChildNodes)
                        {
                            nuevo = new Simbolo(hijo.Token.Text.ToLower(), defecto, tipo, hijo.Token.Location.Line, hijo.Token.Location.Column, 0);
                           
                            if (actual.existeActual(nuevo.identificador))
                            {
                                actual.agregar(nuevo.identificador, nuevo);
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error ya existe en el entorno actual ", "Semantico", -1, -1, "");
                            }
                        }
                    }
                    else
                    {
                        Program.getVentana().agregarError("Error, no se pudo agregar el valor", "Semantico", -1, -1, "");
                    }

                }
            }
            else if (nodo.ChildNodes.Count == 4)
            {
                //4
                if (nodo.ChildNodes[1].Term.ToString().Equals("array"))
                {
                    //Arreglo primitivos son 2
                    asignarTipo(nodo.ChildNodes[0]);
                    foreach(ParseTreeNode var in nodo.ChildNodes[2].ChildNodes)
                    {
                        Arreglo a = new Arreglo(var.Token.Text.ToLower(), null, tipo, var.Token.Location.Line, var.Token.Location.Column, nodo.ChildNodes[3].ChildNodes.Count);
                        a.setDimension(nodo.ChildNodes[3], actual, tFunc);
                        if (actual.existeActual(a.identificador))
                        {
                            actual.agregar(a.identificador, a);
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, ay existe en el entorno actual", "Semantico", -1, -1, "");
                        }
                    }
                    


                }
                else if (nodo.ChildNodes[0].Term.ToString().Equals("VISIBILIDAD"))
                    {
                        asignarVisibilidad(nodo.ChildNodes[0]);
                        asignarTipo(nodo.ChildNodes[1]);
                        if (setValor(nodo.ChildNodes[3],actual))
                        {
                            Simbolo nuevo;
                            foreach (ParseTreeNode hijo in nodo.ChildNodes[2].ChildNodes)
                            {
                                nuevo = new Simbolo(hijo.Token.Text.ToLower(), defecto, tipo, hijo.Token.Location.Line, hijo.Token.Location.Column, 0);
                                if (actual.existeActual(nuevo.identificador))
                                {
                                    actual.agregar(nuevo.identificador, nuevo);
                                }
                                else
                                {
                                Program.getVentana().agregarError("Error, ya existe perro", "Semantico", -1, -1, "");
                            }
                        }
                        }
                    }
                else
                {
                    //DECLARA OBJETO E INICIALIZA
                    asignarTipo(nodo.ChildNodes[0]);
                    string id = nodo.ChildNodes[1].Token.Text.ToLower();
                    if (Ejecutor.tc.existe(tipo))
                    {
                        TablaSimbolos nueva1 = new TablaSimbolos(null);
                        TablaFunciones nueva2 = new TablaFunciones();
                        
                        TablaSimbolos n1 = Ejecutor.tc.sacar(tipo).global;
                        TablaFunciones n2 = Ejecutor.tc.sacar(tipo).funciones;
                        foreach(object key in n1.listaSimbolos.Keys)
                        {
                            if (n1.sacar(key.ToString()).visible)
                            {
                                nueva1.agregar(key.ToString(), n1.sacar(key.ToString()));
                            }
                        }
                        foreach (object key in n2.tabla.Keys)
                        {
                            if (n2.sacar(key.ToString()).visible)
                            {
                                nueva2.agregar(nueva2.sacar(key.ToString()));
                            }
                        }
                        if (actual.existeActual(id))
                        {
                            Objeto o = new Objeto(id, tipo, nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column, 0, nueva1, nueva2);
                            actual.agregar(id, o);
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, ya existe ", "Semantico", -1, -1, "");
                        }
                    }
                    else
                    {
                        Program.getVentana().agregarError("Error , no existe clase", "Semantico", -1, -1, "");
                    }
                }
                
            }
            else if (nodo.ChildNodes.Count == 5)
            {
                //5
                //ARREGLO CON VISIBILIDAD o OBJETO CON VISIBILIDAD 
                if (nodo.ChildNodes[0].Term.ToString().Equals("VISIBILIDAD"))
                {
                    if (nodo.ChildNodes[2].Term.ToString().Equals("array"))
                    {
                        //arreglo con visibilidad
                        asignarVisibilidad(nodo.ChildNodes[0]);
                        asignarTipo(nodo.ChildNodes[1]);
                        foreach(ParseTreeNode var in nodo.ChildNodes[3].ChildNodes)
                        {
                            Arreglo a = new Arreglo(var.Token.Text.ToLower(), null, tipo, var.Token.Location.Line, var.Token.Location.Column, nodo.ChildNodes[4].ChildNodes.Count);
                            a.setDimension(nodo.ChildNodes[4], actual, tFunc);
                            a.visible = visibilidad;
                            if (actual.existeActual(a.identificador))
                            {
                                actual.agregar(a.identificador, a);
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error, ya existe en el entorno actual ", "Semantico", -1, -1, "");
                            }
                        }
                        
                    }
                    else
                    {
                        // objeto con visibilidad
                        //DECLARA OBJETO E INICIALIZA
                        asignarTipo(nodo.ChildNodes[1]);
                        asignarVisibilidad(nodo.ChildNodes[0]);
                        string id = nodo.ChildNodes[1].Token.Text.ToLower();
                        if (Ejecutor.tc.existe(tipo))
                        {
                            TablaSimbolos n1 = Ejecutor.tc.sacar(tipo).global;
                            TablaFunciones n2 = Ejecutor.tc.sacar(tipo).funciones;
                            if (actual.existeActual(id))
                            {
                                Objeto o = new Objeto(id, tipo, nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column, 0, n1, n2);
                                o.visible = visibilidad;
                                actual.agregar(id, o);
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error, que ya existe ", "Semantico", -1, -1, "");
                            }
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, no existe clase", "Semantico", -1, -1, "");
                        }
                    }
                }
                else
                {
                    // ARREGLO INICIALIZADO PRIMITIVO
                    asignarTipo(nodo.ChildNodes[0]);
                    foreach(ParseTreeNode var in nodo.ChildNodes[2].ChildNodes)
                    {
                        Arreglo a = new Arreglo(var.Token.Text.ToLower(), null, tipo, var.Token.Location.Line, var.Token.Location.Column, nodo.ChildNodes[3].ChildNodes.Count);
                        a.setDimension(nodo.ChildNodes[3], actual, tFunc);
                        a.setAsignacion(nodo.ChildNodes[4], actual, tFunc);
                        if (actual.existeActual(a.identificador))
                        {
                            actual.agregar(a.identificador, a);
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, ya existeee ", "Semantico", -1, -1, "");
                        }
                    }
                    
                }

            }
            else if (nodo.ChildNodes.Count == 6)
            {
                //Arreglo inicializado con visibilidad
                asignarVisibilidad(nodo.ChildNodes[0]);
                asignarTipo(nodo.ChildNodes[1]);
                Arreglo a = new Arreglo(nodo.ChildNodes[3].Token.Text.ToLower(), null, tipo, nodo.ChildNodes[3].Token.Location.Line, nodo.ChildNodes[3].Token.Location.Column, nodo.ChildNodes[4].ChildNodes.Count);
                a.setDimension(nodo.ChildNodes[4], actual, tFunc);
                a.setAsignacion(nodo.ChildNodes[5], actual, tFunc);
                if (actual.existeActual(a.identificador)) 
                {
                    actual.agregar(a.identificador, a);
                }else{
                    Program.getVentana().agregarError("Error, ya existe", "Semantico", -1, -1, "");
                }
            }
            else
            {
                Program.getVentana().agregarError("Error en teoría", "Semantico", -1, -1, "");
            }

        }
        public void asignarDimension(ParseTreeNode nodo)
        {
            //RECIBO DIMENSIONES
            dimension = nodo.ChildNodes.Count;
        }
        public void asignarVisibilidad(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes[0].Token.ToString().ToLower().Equals("publico"))
            {
                visibilidad = true;
            }
            else
            {
                visibilidad = false;
            }
        }
        public bool setValor(ParseTreeNode EXP,TablaSimbolos actual)
        {
            Expresion e = new Expresion(EXP, tFunc);
            e.ejecutar(actual,tFunc);
            if (this.tipo == e.tipo)
            {
                this.defecto = e.respuesta;
                return true;
            }
            else
            {
                // ErrorSemantico errorS = new ErrorSemantico(nodo.Token.Text, "Error semántico, tipos incompatibles", nodo.Token.Location.Line, nodo.Token.Location.Column);
                if (retornarTerminal(EXP) != null)
                    Program.getVentana().agregarError("No son del mismo tipo", "Semantico", retornarTerminal(EXP).Token.Location.Line, retornarTerminal(EXP).Token.Location.Column, retornarTerminal(EXP).Token.Value.ToString());
                else
                Program.getVentana().agregarError("no son del mismo tipo, declaracion on generada", "Semantico", -1, -1, "");
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

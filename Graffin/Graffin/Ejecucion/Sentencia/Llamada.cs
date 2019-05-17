using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion
{
    class Llamada
    {
        ParseTreeNode raiz;
        string tipo;
        public object respuesta;
        string id;
        TablaFunciones funciones;
        public Llamada(ParseTreeNode raiz,TablaFunciones funciones)
        {
            this.raiz = raiz;
            this.funciones = funciones;
        }
        public void ejecutar(TablaSimbolos actual)
        {
            if (raiz.ChildNodes.Count ==1)
            {
                id = raiz.ChildNodes[0].Token.Text.ToLower();
                if (funciones.existe(id))
                {
                    Funcion aux = funciones.sacar(id);
                    Funcion f = new Funcion(aux.identificador,null,aux.tipo,aux.linea,aux.columna,aux.dimension,aux.nodo,aux.parametros);
                    f.ejecutar(actual, funciones);
                    respuesta = f.valor;
                    tipo = f.tipo;
                }
            }
            else if (raiz.ChildNodes.Count == 2)
            {
                if (raiz.ChildNodes[1].Term.ToString().Equals("LISTAEXP"))
                {
                    id = raiz.ChildNodes[0].Token.Text.ToLower();
                    getId(raiz.ChildNodes[1], actual, funciones);
                    if (funciones.existe(id))
                    {
                        Funcion f = new Funcion(funciones.sacar(id).identificador, null, funciones.sacar(id).tipo, funciones.sacar(id).linea, funciones.sacar(id).columna, funciones.sacar(id).dimension, funciones.sacar(id).nodo, funciones.sacar(id).parametros); 
                        
                        f.setAtributos(raiz.ChildNodes[1],actual,funciones);
                        f.ejecutar(actual, funciones);
                        respuesta = f.valor;
                        tipo = f.tipo;
                    }
                    else
                    {
                        Program.getVentana().agregarError("Error, no existe ", "Semantico", -1, -1, "");
                    }
                }
                else
                {
                    //acceso sin params
                    string id = raiz.ChildNodes[0].Token.Text.ToLower();
                    string atr = raiz.ChildNodes[1].Token.Text.ToLower();
                    if (actual.existe(id))
                    {
                        if (actual.sacar(id).esObjeto())
                        {
                            Objeto o = (Objeto)actual.sacar(id);
                            if (o.funciones.existe(atr))
                            {
                               
                                Funcion f = o.funciones.sacar(atr);
                                if (f != null)
                                {
                                    if (f.visible)
                                    {
                                        f.ejecutar(o.local, o.funciones);
                                        respuesta = f.valor;
                                        tipo = f.tipo;
                                    }
                                    else
                                    {
                                        Program.getVentana().agregarError("Error, es privado", "Semantico", -1, -1, "");
                                    }
                                }
                                else
                                {
                                    Program.getVentana().agregarError("Error, es nulo", "Semantico", -1, -1, "");
                                }

                            }
                        }
                    }
                }
            }
            else if (raiz.ChildNodes.Count == 3)
            {
                //acceso con param
                string id = raiz.ChildNodes[0].Token.Text.ToLower();
                string atr = raiz.ChildNodes[1].Token.Text.ToLower();
                getId(raiz.ChildNodes[2], actual, funciones);
                if (actual.existe(id))
                {
                    if (actual.sacar(id).esObjeto())
                    {
                        Objeto o = (Objeto)actual.sacar(id);
                        if (o.funciones.existe(atr))
                        {
                            Funcion f = o.funciones.sacar(atr);
                            if (f.visible)
                            {
                                f.setAtributos(raiz.ChildNodes[2], actual, funciones);
                                f.ejecutar(o.local, o.funciones);
                                respuesta = f.valor;
                                tipo = f.tipo;
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error ", "Semantico", -1, -1, "");
                            }
                        }
                    }
                }
            }
            else
            {
                Program.getVentana().agregarError("Error desconocido ", "Semantico", -1, -1, "");

            }
        }
        void getId(ParseTreeNode raiz,TablaSimbolos actual, TablaFunciones funciones)
        {
            
            foreach(ParseTreeNode exp in raiz.ChildNodes)
            {
                Expresion e = new Expresion(exp,funciones);
                
                e.ejecutar(actual, funciones);
                if (e.esArreglo)
                    id += "array";
                id += e.tipo;
            }
        }

    }
}

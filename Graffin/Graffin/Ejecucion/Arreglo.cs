using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion
{
    class Arreglo:Simbolo
    {
        List<int> tamanos;
        public int pagina,col,fila;
        public object[] valores;
        int dimensiones;
        public Arreglo(string identificador, object valor, string tipo, int linea, int columna, int dimension) : base(identificador, valor, tipo, linea, columna, dimension)
        {
            this.identificador = identificador;
            this.valor = valor;
            this.tipo = tipo;
            this.linea = linea;
            this.dimensiones = dimension;
            tamanos = new List<int>();
            pagina = 0;
            this.col = 0;
            fila = 0;
            
        }
        public void setDimension(ParseTreeNode nodo,TablaSimbolos actual,TablaFunciones funciones)
        {
            foreach(ParseTreeNode hijo in nodo.ChildNodes)
            {
                try
                {
                    Expresion exp = new Expresion(hijo.ChildNodes[0],funciones);
                    exp.ejecutar(actual,funciones);
                    if(exp.respuesta is int)
                        tamanos.Add((int)exp.respuesta);
                    else
                        Program.getVentana().agregarError("Error en la dimension, no es int", "Semantico", nodo.Token.Location.Line, nodo.Token.Location.Column, nodo.Token.Text);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Program.getVentana().agregarError("Error,"+e.Message, "Semantico", -1, -1, "");
                }
            }
            valorDefecto();
            if (tamanos.Count == 1)
            {
                col = tamanos[0];
                valores = new object[col];
                for (int i = 0; i < col; i++)
                {
                    valores[i] = valor;
                }
            }else if (tamanos.Count == 2)
            {
                fila = tamanos[0];
                col = tamanos[1];
                this.valores = new object[col*fila];
                for (int i = 0; i < fila * col; i++)
                {
                    valores[i] = valor;
                }
            }else if (tamanos.Count == 3)
            {
                pagina = tamanos[0];
                fila = tamanos[1];
                col = tamanos[2];
                valores = new object[col*fila*pagina];
                for (int i = 0; i < fila * col*pagina; i++)
                {
                    valores[i] = valor;
                }
            }
            else
            {
                Program.getVentana().agregarError("Error,desconocido ", "Semantico", -1, -1, "");
            }


        }
        public void setAsignacion(ParseTreeNode nodo,TablaSimbolos actual, TablaFunciones funciones)
        {
            if (nodo.ChildNodes[0].Term.ToString().Equals("ARREGLO"))
            {
                if(dimension == 1)
                {
                    ParseTreeNode expresiones = nodo.ChildNodes[0].ChildNodes[0];//Tomamos lista exp
                    if (expresiones.ChildNodes.Count == col)
                    {
                        Expresion e;
                        int i=0;
                        foreach (ParseTreeNode exp in expresiones.ChildNodes)
                        {

                            e = new Expresion(exp,funciones);
                            e.ejecutar(actual, funciones);
                            if (e.respuesta != null)
                            {
                                if (e.tipo.Equals(tipo))
                                {
                                    valores[i] = e.respuesta;
                                }
                                else
                                {
                                    Program.getVentana().agregarError("Error, tipos diferentes", "Semantico", -1, -1, "");
                                    break;
                                }
                            }
                            
                            i++;
                        }
                    }
                    else
                    {
                        Program.getVentana().agregarError("Error,", "Semantico", -1, -1, "");
                    }

                }
                else
                {
                    Program.getVentana().agregarError("Error,dimension erronea ", "Semantico", -1, -1, "");
                }
            }
            else if (nodo.ChildNodes[0].Term.ToString().Equals("LISTACURLY2"))
            {
                if (dimension == 2)
                {
                    
                    ParseTreeNode filas = nodo.ChildNodes[0].ChildNodes[0];//Lista curly
                    
                    if(filas.ChildNodes.Count == fila)
                    {
                        int j = 0;
                        foreach (ParseTreeNode expresiones in filas.ChildNodes)
                        {
                            if (expresiones.ChildNodes[0].ChildNodes.Count == col)
                            {
                                Expresion e;
                                int i=0;
                                foreach (ParseTreeNode exp in expresiones.ChildNodes[0].ChildNodes)
                                {
                                    e = new Expresion(exp,funciones);
                                    e.ejecutar(actual, funciones);
                                    if (e.respuesta!=null){
                                        if (e.tipo.Equals(tipo))
                                        {
                                            valores[j + i] = e.respuesta;
                                        }
                                        else
                                        {
                                            Program.getVentana().agregarError("Error,es null", "Semantico", -1, -1, "");
                                            break;
                                        }
                                    }
                                    i++;
                                }
                            }
                            else
                            {

                                Program.getVentana().agregarError("Error,columna mala", "Semantico", -1, -1, "");
                                break;
                            }
                            j += col;
                        }
                    }
                    
                    
                }
                else
                {
                    Program.getVentana().agregarError("Error,la dimension no es 2", "Semantico", -1, -1, "");
                }
            }
            else if (nodo.ChildNodes[0].Term.ToString().Equals("LISTADECURLYS"))
            {
                if (dimension == 3)
                {
                    ParseTreeNode paginas = nodo.ChildNodes[0];
                    
                    if (paginas.ChildNodes.Count == pagina)
                    {
                        int k = 0;
                        foreach (ParseTreeNode filas in paginas.ChildNodes)
                        {   
                            if (filas.ChildNodes[0].ChildNodes.Count == fila)
                            {
                                int j = 0;
                                foreach (ParseTreeNode columnas in filas.ChildNodes[0].ChildNodes)//columna es arreglo
                                {
                                    
                                    if(columnas.ChildNodes[0].ChildNodes.Count == col)
                                    {
                                        int i = 0;
                                        foreach (ParseTreeNode exp in columnas.ChildNodes[0].ChildNodes)
                                        {
                                            Expresion e = new Expresion(exp,funciones);
                                            e.ejecutar(actual, funciones);
                                            if (e.respuesta != null)
                                            {
                                                if (e.tipo.Equals(tipo))
                                                {
                                                    valores[i + j + k] = e.respuesta;
                                                }
                                                else
                                                {
                                                    Program.getVentana().agregarError("Error, tipos diferentes", "Semantico", -1, -1, "");
                                                }
                                            }
                                            else
                                            {
                                                Program.getVentana().agregarError("Error, es null", "Semantico", -1, -1, "");
                                            }
                                            i++;
                                        }
                                    }
                                    else
                                    {
                                        Program.getVentana().agregarError("Error, columnas erronesa", "Semantico", -1, -1, "");
                                    }
                                    j += col;
                                }
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error, filas distintas", "Semantico", -1, -1, "");
                            }
                            k += fila * col;
                        }
                    }
                    
                }
                else
                {
                    Program.getVentana().agregarError("Error,", "Semantico", -1, -1, "");
                }
            }
            else
            {
                Program.getVentana().agregarError("Error, desconocido", "Semantico", -1, -1, "");
            }
        }
        public void setValorU(Expresion valor,ParseTreeNode indice,TablaSimbolos actual, TablaFunciones funciones)
        {
            valor.ejecutar(actual, funciones);
            if (valor.tipo.Equals(tipo))
            {
                int posicion;
                if (indice.ChildNodes.Count != dimension)
                {
                    Program.getVentana().agregarError("Error, dimension diferente", "Semantico", -1, -1, "");
                }
                else
                {

                    if (dimension == 1)
                    {
                        Expresion ef = new Expresion(indice.ChildNodes[0].ChildNodes[0],funciones);
                        ef.ejecutar(actual, funciones);
                        if (ef.respuesta != null)
                        {
                            if (ef.respuesta is int)
                            {
                                posicion = (int)ef.respuesta;
                                if (posicion < col)
                                {
                                    valores[posicion] = valor.respuesta;
                                }
                                else
                                {
                                    Program.getVentana().agregarError("Error,fuera del indice", "Semantico", -1, -1, "");
                                }
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error, no es int el indice", "Semantico", -1, -1, "");
                            }
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, es null la respuesta", "Semantico", -1, -1, "");
                        }

                    }
                    else if (dimension == 2)
                    {
                        Expresion ef = new Expresion(indice.ChildNodes[0].ChildNodes[0],funciones);
                        ef.ejecutar(actual, funciones);
                        Expresion ec = new Expresion(indice.ChildNodes[1].ChildNodes[0],funciones);
                        ec.ejecutar(actual, funciones);
                        if (ef.respuesta != null && ec.respuesta != null)
                        {
                            if (ef.respuesta is int && ec.respuesta is int)
                            {
                                if ((int)ef.respuesta < fila && (int)ec.respuesta < col)
                                {
                                    posicion = (int)ef.respuesta * col + (int)ec.respuesta;
                                    valores[posicion] = valor.respuesta;
                                }
                                else
                                {
                                    Program.getVentana().agregarError("Error, fuera del índice", "Semantico", -1, -1, "");
                                }
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error, indice no es int", "Semantico", -1, -1, "");
                            }
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, indice es null ", "Semantico", -1, -1, "");
                        }


                    }
                    else if (dimension == 3)
                    {
                        Expresion ep = new Expresion(indice.ChildNodes[0].ChildNodes[0],funciones);
                        ep.ejecutar(actual, funciones);
                        Expresion ef = new Expresion(indice.ChildNodes[1].ChildNodes[0], funciones);
                        ef.ejecutar(actual, funciones);
                        Expresion ec = new Expresion(indice.ChildNodes[2].ChildNodes[0], funciones);
                        ec.ejecutar(actual, funciones);
                        if (ef.respuesta != null && ec.respuesta != null && ep.respuesta != null)
                        {
                            if (ef.respuesta is int && ec.respuesta is int && ep.respuesta is int)
                            {
                                if ((int)ef.respuesta < fila && (int)ec.respuesta < col && (int)ep.respuesta < pagina)
                                {
                                    posicion = (int)ep.respuesta * fila * col + (int)ef.respuesta * col + (int)ec.respuesta;
                                    valores[posicion] = valor.respuesta;

                                }
                                else
                                {
                                    Program.getVentana().agregarError("Error, fuera del índice", "Semantico", -1, -1, "");
                                }
                            }
                            else
                            {
                                Program.getVentana().agregarError("Error, no es int el índice", "Semantico", -1, -1, "");
                            }
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, es null el índice ", "Semantico", -1, -1, "");
                        }

                    }
                }
            }
            
        }
        public object getValor(ParseTreeNode indice,TablaSimbolos actual, TablaFunciones funciones)
        {
            if (dimension == indice.ChildNodes.Count)//Dimensiones
            {
                if (dimension == 1)
                {
                    Expresion exp = new Expresion(indice.ChildNodes[0].ChildNodes[0], funciones);
                    exp.ejecutar(actual,funciones);
                    if(exp.respuesta is int)
                    {
                        int posiciones = (int)exp.respuesta;
                        if (posiciones < col)
                        {
                            return valores[posiciones];
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, fuera del indice", "Semantico", -1, -1, "");
                        }
                    }
                }
                else if (dimension == 2)
                {
                    Expresion expF= new Expresion(indice.ChildNodes[0].ChildNodes[0], funciones);
                    expF.ejecutar(actual, funciones);
                    Expresion expC = new Expresion(indice.ChildNodes[1].ChildNodes[0], funciones);
                    expC.ejecutar(actual, funciones);
                    if (expF.respuesta is int && expC.respuesta is int)
                    {
                        int posiciones = (int)expF.respuesta*col+(int)expC.respuesta;
                        if ((int)expC.respuesta < col&& (int)expF.respuesta<fila)
                        {
                            return valores[posiciones];
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, fuera del índice ", "Semantico", -1, -1, "");
                        }
                    }
                    else
                    {
                        Program.getVentana().agregarError("Error, no es int", "Semantico", -1, -1, "");
                    }
                }
                else if (dimension == 3)
                {
                    Expresion expF = new Expresion(indice.ChildNodes[1].ChildNodes[0], funciones);
                    expF.ejecutar(actual, funciones);
                    Expresion expC = new Expresion(indice.ChildNodes[2].ChildNodes[0], funciones);
                    expC.ejecutar(actual, funciones);
                    Expresion expP = new Expresion(indice.ChildNodes[0].ChildNodes[0], funciones);
                    expP.ejecutar(actual, funciones);
                    if (expF.respuesta is int && expC.respuesta is int&&expP.respuesta is int)
                    {
                        int posiciones = (int)expF.respuesta * col + (int)expC.respuesta+(int)expP.respuesta*fila*col;
                        if ((int)expC.respuesta < col && (int)expF.respuesta < fila&& (int)expP.respuesta<pagina)
                        {
                            return valores[posiciones];
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error,", "Semantico", -1, -1, "");
                        }
                    }
                    else
                    {
                        Program.getVentana().agregarError("Error,", "Semantico", -1, -1, "");
                    }
                }
                else
                {
                    Program.getVentana().agregarError("Error, desconocido ", "Semantico", -1, -1, "");

                }
            }
            else
            {

                Program.getVentana().agregarError("Error, diferentes tipos", "Semantico", -1, -1, "");
            }
            return null;
        }
        void valorDefecto()
        {
            if (tipo.Equals("int"))
            {
                valor = (int)0;
            }else if (tipo.Equals("double"))
            {
                valor = (double)0.0;
            }
            else if(tipo.Equals("char"))
            {
                valor = (char)'\u0000';
            }else if (tipo.Equals("string"))
            {
                valor = (string)"";
            }
            else if(tipo.Equals("bool"))
            {
                valor = (bool)false;
            }
            else
            {
                Program.getVentana().agregarError("Error, no hay arreglo de objetos", "Semantico", -1, -1, "");
            }
        }
    }
}

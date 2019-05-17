using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Graffin.Ejecucion.Sentencia;

namespace Graffin.Ejecucion
{
    class Bloque
    {
        public bool romper;
        public bool continuar;
        public bool retorno;
        ParseTreeNode nodo;
        TablaSimbolos actual;
        TablaFunciones tFunc;
        public object respuesta;
        public Bloque(ParseTreeNode nodo,TablaSimbolos local,TablaFunciones tFunc)
        {
            this.nodo = nodo;
            this.tFunc = tFunc;
            continuar = false;
            romper = false;
            retorno = false;
        }
        public void ejecutar(TablaSimbolos actual)
        {
            ejecutar(actual,this.nodo);
        }
        private void ejecutar(TablaSimbolos actual, ParseTreeNode raiz)
        {
            switch (raiz.Term.ToString())
            
            {

                case "BLOQUE":
                    ejecutar(actual, raiz.ChildNodes[0]);
                    break;
                case "LISTASENTENCIA":
                    foreach (ParseTreeNode nodo in raiz.ChildNodes)
                    {
                        if (nodo.ChildNodes[0].Term.ToString().Equals("RETURN"))
                        {
                            Expresion e = new Expresion(nodo.ChildNodes[0].ChildNodes[1],tFunc);
                            e.ejecutar(actual, tFunc);
                            respuesta = e.respuesta;
                            retorno = true;
                            break;
                        } else if (nodo.ChildNodes[0].Term.ToString().Equals("CONTINUE"))
                        {
                            continuar = true;
                            break;
                        } else if (nodo.ChildNodes[0].Term.ToString().Equals("BREAK"))
                        {
                            romper = true;
                            break;
                        }
                        ejecutar(actual, nodo);
                    }
                    break;
                case "SENTENCIA":
                    ejecutar(actual, raiz.ChildNodes[0]);
                    break;
                case "DECLARACIONES":
                    Declaracion d = new Declaracion(raiz, tFunc);
                    d.ejecutar(actual);
                    break;
                case "DECLARAASIG":
                    Declaracion da = new Declaracion(raiz, tFunc);
                    da.ejecutar(actual);
                    break;
                case "ASIGNACION":
                    Asignacion a = new Asignacion(raiz, tFunc);
                    a.ejecutar(actual);
                    break;
                case "IF":
                    Si nuevoIf = new Si(raiz, tFunc);
                    nuevoIf.ejecutar(actual);
                    respuesta = nuevoIf.respuesta;
                    break;
                case "ELSEIF":
                    Sino nuevoELseIf = new Sino(raiz, tFunc);
                    nuevoELseIf.ejecutar(actual);
                    respuesta = nuevoELseIf.respuesta;
                    break;
                case "INCDEC":
                    IncDec incDec = new IncDec(raiz);
                    incDec.ejecutar(actual, tFunc);
                    respuesta = incDec.respuesta;
                    break;
                case "WHILE":
                    Mientras mientras = new Mientras(raiz, actual,tFunc);
                    mientras.ejecutar();
                    respuesta = mientras.respuesta;
                    break;
                case "DOWHILE":
                    HacerMientras hacerMientras = new HacerMientras(raiz, actual,tFunc);
                    hacerMientras.ejecutar();
                    respuesta = hacerMientras.respuesta;
                    break;
                case "FOR":
                    Para para = new Para(raiz, actual,tFunc);
                    para.ejecutar();
                    respuesta = para.respuesta;
                    break;
                case "SWITCH":
                    Comprobar comprobar = new Comprobar(raiz, tFunc);
                    comprobar.ejecutar(actual);
                    respuesta = comprobar.respuesta;
                    break;
                case "REPEAT":
                    Repetir repetir = new Repetir(raiz, actual,tFunc);
                    repetir.ejecutar();
                    respuesta = repetir.respuesta;
                    break;
                case "RETURN":
                    Expresion returno = new Expresion(raiz.ChildNodes[1],tFunc);
                    returno.ejecutar(actual, tFunc);
                    respuesta = returno.respuesta;
                    break;
                case "BREAK":
                    break;
                case "CONTINUE":
                    break;
                case "ACCESO":
                    Acceso acceso = new Acceso(raiz.ChildNodes[0].Token.ToString().ToLower(), raiz.ChildNodes[1].Token.ToString().ToLower());
                    
                    break;
                case "LLAMADA":
                    Llamada llamada = new Llamada(raiz,tFunc);
                    llamada.ejecutar(actual);
                    respuesta = llamada.respuesta;
                    break;
                case "IMPRIMIR":
                    Expresion exp = new Expresion(raiz.ChildNodes[1],tFunc);
                    exp.ejecutar(actual, tFunc);
                    Print print = new Print(exp);
                    break;
                case "SHOW":
                    Expresion titulo = new Expresion(raiz.ChildNodes[1],tFunc);
                    titulo.ejecutar(actual,tFunc);
                    Expresion texto = new Expresion(raiz.ChildNodes[2], tFunc);
                    texto.ejecutar(actual, tFunc);
                    Show show = new Show(titulo,texto);
                    break;
                case "FIGURE":
                    Expresion figura = new Expresion(raiz.ChildNodes[1],tFunc);
                    figura.ejecutar(actual, tFunc);
                    //Figure.mostrarFigura(nodo.Childnodes[1]);
                    Program.getVentana().mostrar(figura.respuesta.ToString());
                    break;
                case "ADDFIGURE":
                    //Figure.agregarFigura(nodo);
                    Nativa nativa = new Nativa(raiz,tFunc);
                    nativa.ejecutar(actual);
                    break;
                default:
                    Program.getVentana().agregarError("Faltó un nodo " + nodo.Term.ToString(), "Semantico", 0, 0, "");
                    break;

            }
        
        }
    }
}

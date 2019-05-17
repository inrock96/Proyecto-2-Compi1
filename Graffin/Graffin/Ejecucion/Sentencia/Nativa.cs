using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion.Sentencia
{
    class Nativa
    {
        public ParseTreeNode nodo;
        public TablaFunciones funciones;
        string tipo;
        public Nativa(ParseTreeNode nodo, TablaFunciones funciones)
        {
            this.nodo = nodo;
            this.funciones = funciones;
        }
        public void ejecutar(TablaSimbolos actual)
        {
            tipo = getTipo(nodo.ChildNodes[1]);
            int op = nodo.ChildNodes[2].ChildNodes.Count;
            ParseTreeNode lExp = nodo.ChildNodes[2];
            if (!tipo.Equals("null"))
            {
                switch (op)
                {
                    case 5:
                        Expresion c0 = new Expresion(lExp.ChildNodes[0], funciones);
                        c0.ejecutar(actual, funciones);
                        Expresion c1 = new Expresion(lExp.ChildNodes[1], funciones);
                        c1.ejecutar(actual, funciones);
                        Expresion c2 = new Expresion(lExp.ChildNodes[2], funciones);
                        c2.ejecutar(actual, funciones);
                        Expresion c3 = new Expresion(lExp.ChildNodes[3], funciones);
                        c3.ejecutar(actual, funciones);
                        Expresion c4 = new Expresion(lExp.ChildNodes[4], funciones);
                        c4.ejecutar(actual, funciones);
                        if(c0.respuesta is string&&(c1.respuesta is double || c1.respuesta is int)&&c2.respuesta is bool&&(c3.respuesta is int|| c3.respuesta is double)&&(c4.respuesta is int || c4.respuesta is double))
                        {
                            if (Graff.convert_color(c0.respuesta))
                            {
                                Figura figura = new Figura(tipo,c0.respuesta,Convert.ToDouble(c1.respuesta),(bool)c2.respuesta,Convert.ToDouble(c3.respuesta),Convert.ToDouble(c4.respuesta));
                                Ejecutor.figuras.Add(figura);
                            }
                        }
                        else
                        {

                        }

                        break;
                    case 6:
                        if (tipo.Equals("square"))
                        {
                            Expresion s0 = new Expresion(lExp.ChildNodes[0], funciones);
                            s0.ejecutar(actual, funciones);
                            Expresion s1 = new Expresion(lExp.ChildNodes[1], funciones);
                            s1.ejecutar(actual, funciones);
                            Expresion s2 = new Expresion(lExp.ChildNodes[2], funciones);
                            s2.ejecutar(actual, funciones);
                            Expresion s3 = new Expresion(lExp.ChildNodes[3], funciones);
                            s3.ejecutar(actual, funciones);
                            Expresion s4 = new Expresion(lExp.ChildNodes[4], funciones);
                            s4.ejecutar(actual, funciones);
                            Expresion s5 = new Expresion(lExp.ChildNodes[5], funciones);
                            s5.ejecutar(actual, funciones);
                            if (s0.respuesta is string && (s2.respuesta is double || s2.respuesta is int) && s1.respuesta is bool && (s3.respuesta is int || s3.respuesta is double) && (s4.respuesta is int || s4.respuesta is double) && (s5.respuesta is int || s5.respuesta is double))
                            {
                                if (Graff.convert_color(s0.respuesta))
                                {
                                    Ejecutor.figuras.Add(new Figura(tipo, s0.respuesta,
                                        (bool)s1.respuesta,
                                        Convert.ToDouble(s2.respuesta),
                                        Convert.ToDouble(s3.respuesta),
                                        Convert.ToDouble(s4.respuesta),
                                        Convert.ToDouble(s5.respuesta)));
                                  
                                }
                            }
                            else
                            {

                            }
                        }
                        else if (tipo.Equals("line"))
                        {
                            Expresion l0 = new Expresion(lExp.ChildNodes[0], funciones);
                            l0.ejecutar(actual, funciones);
                            Expresion l1 = new Expresion(lExp.ChildNodes[1], funciones);
                            l1.ejecutar(actual, funciones);
                            Expresion l2 = new Expresion(lExp.ChildNodes[2], funciones);
                            l2.ejecutar(actual, funciones);
                            Expresion l3 = new Expresion(lExp.ChildNodes[3], funciones);
                            l3.ejecutar(actual, funciones);
                            Expresion l4 = new Expresion(lExp.ChildNodes[4], funciones);
                            l4.ejecutar(actual, funciones);
                            Expresion l5 = new Expresion(lExp.ChildNodes[5], funciones);
                            l5.ejecutar(actual, funciones);
                            if (l0.respuesta is string && (l2.respuesta is double || l2.respuesta is int) && (l1.respuesta is double || l1.respuesta is int) && (l3.respuesta is int || l3.respuesta is double) && (l4.respuesta is int || l4.respuesta is double) && (l5.respuesta is int || l5.respuesta is double))
                            {
                                if (Graff.convert_color(l0.respuesta))
                                {
                                    Ejecutor.figuras.Add(new Figura(tipo, l0.respuesta,
                                       Convert.ToDouble(l1.respuesta),
                                       Convert.ToDouble(l2.respuesta),
                                       Convert.ToDouble(l3.respuesta),
                                       Convert.ToDouble(l4.respuesta),
                                        (int)l5.respuesta));
                                }
                            }
                        }
                        else
                        {
                            //error inesperado
                        }
                        break;
                    case 8:
                        //triangulo
                        Expresion e0 = new Expresion(lExp.ChildNodes[0], funciones);
                        e0.ejecutar(actual, funciones);
                        Expresion e1 = new Expresion(lExp.ChildNodes[1], funciones);
                        e1.ejecutar(actual, funciones);
                        Expresion e2 = new Expresion(lExp.ChildNodes[2], funciones);
                        e2.ejecutar(actual, funciones);
                        Expresion e3 = new Expresion(lExp.ChildNodes[3], funciones);
                        e3.ejecutar(actual, funciones);
                        Expresion e4 = new Expresion(lExp.ChildNodes[4], funciones);
                        e4.ejecutar(actual, funciones);
                        Expresion e5 = new Expresion(lExp.ChildNodes[5], funciones);
                        e5.ejecutar(actual, funciones);
                        Expresion e6 = new Expresion(lExp.ChildNodes[6], funciones);
                        e6.ejecutar(actual, funciones);
                        Expresion e7 = new Expresion(lExp.ChildNodes[7], funciones);
                        e7.ejecutar(actual, funciones);
                        if (e0.respuesta is string && (e2.respuesta is double || e2.respuesta is int) && e1.respuesta is bool && (e3.respuesta is int || e3.respuesta is double) && (e4.respuesta is int || e4.respuesta is double) && (e5.respuesta is int || e5.respuesta is double) && (e6.respuesta is int || e6.respuesta is double) && (e7.respuesta is int || e7.respuesta is double))
                        {
                            if (Graff.convert_color(e0.respuesta))
                            {
                                Ejecutor.figuras.Add(new Figura(tipo, e0.respuesta,
                                    (bool)e1.respuesta,
                                    Convert.ToDouble(e2.respuesta),
                                    Convert.ToDouble(e3.respuesta),
                                    Convert.ToDouble(e4.respuesta),
                                    Convert.ToDouble(e5.respuesta),
                                    Convert.ToDouble(e6.respuesta),
                                    Convert.ToDouble(e7.respuesta)));
                            }
                        }
                        break;
                    default:
                        //error semantico
                        break;
                }
            }
            else
            {
                //error sintactico
            }
            
        }
        string getTipo(ParseTreeNode nodo)
        {
            switch (nodo.ChildNodes[0].Term.ToString().ToLower())
            {
                case "circle":
                    return "circle";
                case "line":
                    return "line";
                case "square":
                    return "square";
                case "triangle":
                    return "triangle";
                default:
                    return "null";
            }
        }
    }
}

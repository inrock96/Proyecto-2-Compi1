using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Graffin.Ejecucion.Sentencia;

namespace Graffin.Ejecucion
{
    class Expresion
    {
        public string tipo;
        public bool esArreglo;
        public bool esObjeto;
        public object respuesta;
        TablaSimbolos actual;
        ParseTreeNode nodo;
        TablaFunciones tFunc;

        public Expresion(ParseTreeNode nodo,TablaFunciones tFunc)
        {
            this.nodo = nodo;
            esArreglo = false;
            esObjeto = false;
            this.tFunc = tFunc;
        }
        
        public void ejecutar(TablaSimbolos actual,TablaFunciones tFunc)
        {
            ejecutar(nodo, actual, tFunc);
        }
        public void asignarTipo()
        {
            if (respuesta is int)
            {
                tipo = "int";
            }
            else if (respuesta is double)
            {
                tipo = "double";
            }
            else if (respuesta is string)
            {
                tipo = "string";
            }
            else if (respuesta is char)
            {
                tipo = "char";
            }
            else if (respuesta is bool)
            {
                tipo = "bool";
            }
            else if(respuesta is Objeto)
            {
                Objeto objeto = (Objeto)respuesta;
                tipo = objeto.tipo;
                esObjeto = true;
            }else if(respuesta is Arreglo)
            {
                Arreglo arreglo = (Arreglo)respuesta;
                tipo = arreglo.tipo;
                esArreglo = true;
            }
        }
        void ejecutar(ParseTreeNode raiz, TablaSimbolos actual,TablaFunciones tFunc)
        {
            this.actual = actual;
            switch (raiz.Term.ToString())
            {
                case "EXP":
                    ejecutar(raiz.ChildNodes[0], actual,tFunc);
                    break;
                case "EXP_AR":
                    ejecutarAR(raiz);
                    break;
                case "EXP_LO":
                    ejecutarLO(raiz);
                    break;
                case "EXP_RE":
                    ejecutarRE(raiz);
                    break;
                case "ACCESO":
                    Acceso acceso = new Acceso(raiz.ChildNodes[0].Token.Text.ToLower(), raiz.ChildNodes[1].Token.Text.ToLower());
                    Simbolo s = acceso.getValor(actual);
                    if (s != null)
                    {
                        if (s.esObjeto())
                        {
                            Objeto o = (Objeto)acceso.getValor(actual);
                            respuesta = 0;
                        }else if (s.dimension > 0)
                        {
                            Arreglo a = (Arreglo)acceso.getValor(actual);
                            respuesta = a;
                        }
                        else
                        {
                            respuesta = s.valor;
                        }
                    }
                    
                    break;
                case "LLAMADA":
                    Llamada llamada = new Llamada(raiz,tFunc);
                    llamada.ejecutar(actual);
                    respuesta = llamada.respuesta;
                  
                    break;
                case "PRIMITIVO":
                    if (raiz.ChildNodes[0].Term.ToString() == "IDARREGLO")
                    {
                        string ident = raiz.ChildNodes[0].ChildNodes[0].Token.Text.ToLower();
                        if (actual.existe(ident))
                        {
                            if (actual.sacar(ident).dimension > 0)
                            {
                                Arreglo a = (Arreglo)actual.sacar(ident);
                                respuesta = a.getValor(raiz.ChildNodes[0].ChildNodes[1], actual, tFunc);

                            }
                            else
                            {
                                if(retornarTerminal(raiz)!=null)
                                Program.getVentana().agregarError("Error, no es arreglo", "Semantico", raiz.Token.Location.Line, raiz.Token.Location.Column, raiz.Token.Text);
                                else
                                    Program.getVentana().agregarError("Error, no es arreglo", "Semantico", -1, -1, "no encontrado");
                            }
                        }
                        else
                        {
                            if(retornarTerminal(raiz)!=null)
                            Program.getVentana().agregarError("Error, no existe en la", "Semantico", raiz.Token.Location.Line, raiz.Token.Location.Column, raiz.Token.Text);
                            else
                                    Program.getVentana().agregarError("Error, no existe en la", "Semantico", -1, -1, "no encontrado");
                        }

                    }
                    else
                    {
                        switch (raiz.ChildNodes[0].Term.ToString())
                        {
                            case "cadena":
                                respuesta = (string)raiz.ChildNodes[0].Token.Value;
                                break;
                            case "caracter":
                                respuesta = Convert.ToChar(raiz.ChildNodes[0].Token.Value);
                                break;
                            case "numero":
                                double result = Convert.ToDouble(raiz.ChildNodes[0].Token.Text);
                                try
                                {
                                    int result2 = Convert.ToInt32(raiz.ChildNodes[0].Token.Text);
                                    respuesta = result2;
                                }
                                catch (Exception)
                                {
                                    respuesta = result;
                                }
                                break;
                            case "grfFalse":
                                respuesta = (bool)false;
                                break;
                            case "grfTrue":
                                respuesta = (bool)true;
                                break;
                            case "grfVerdadero":
                                respuesta = (bool)true;
                                break;
                            case "grfFalso":
                                respuesta =(bool) false;
                                break;
                            case "identificador":
                                string identi = raiz.ChildNodes[0].Token.Text.ToLower() ;

                                if (actual.existe(identi))
                                {
                                    if (actual.sacar(identi).dimension==0)
                                    {
                                        if(actual.sacar(identi) is Objeto)
                                        {
                                            Objeto o = (Objeto)actual.sacar(identi);
                                            respuesta = o;
                                        }
                                        else
                                        {
                                            Simbolo sim = actual.sacar(identi);
                                            respuesta = sim.valor;
                                        }
                                        
                                    }
                                    else
                                    {
                                        Arreglo sim =(Arreglo) actual.sacar(identi);
                                        respuesta = sim;
                                    }
              
                                }
                                else
                                {
                                    if (retornarTerminal(raiz) != null)
                                    Program.getVentana().agregarError("Error, no existe", "Semantico", retornarTerminal(raiz).Token.Location.Line, retornarTerminal(raiz).Token.Location.Column, retornarTerminal(raiz).Token.Text);
                                    else
                                    Program.getVentana().agregarError("Error, no existe", "Semantico", -1, -1, "no encontrado");

                                }


                                break;
                        }
                    }
                    break;
                case "INCDEC":
                    IncDec inde = new IncDec(nodo);
                    inde.ejecutar(actual, tFunc);
                    respuesta = inde.respuesta;
                    
                    break;
            }
            asignarTipo();
        }

        object ejecutarAR(ParseTreeNode raiz)
        {
            if (raiz.ChildNodes.Count == 3)
            {
                switch (raiz.ChildNodes[1].Term.ToString())
                {
                    case "+":
                        return respuesta =  suma(raiz.ChildNodes[0], raiz.ChildNodes[2]);
                    case "-":
                        return respuesta =  resta(raiz.ChildNodes[0], raiz.ChildNodes[2]);
                    case "*":
                        return respuesta =  multiplicacion(raiz.ChildNodes[0], raiz.ChildNodes[2]);
                    case "/":
                        return respuesta= division(raiz.ChildNodes[0], raiz.ChildNodes[2]);
                    case "^":
                        return respuesta = potencia(raiz.ChildNodes[0], raiz.ChildNodes[2]);
                    default:
                        if (retornarTerminal(raiz) != null) 
                        Program.getVentana().agregarError("Error desconocido con"+raiz.ChildNodes[1].Term.ToString(), "Semantico", nodo.Token.Location.Line, nodo.Token.Location.Column, nodo.Token.Text);
                        else
                                    Program.getVentana().agregarError("Error, no existe", "Semantico", -1, -1, "no encontrado");
                        break;

                }
                return null;
            }
            else if(raiz.ChildNodes.Count==2)
            {
                menosU(raiz.ChildNodes[1]);
                return null;
            }
            else
            {
                return null;
            }
        }
        object ejecutarLO(ParseTreeNode raiz)
        {
            if (raiz.ChildNodes.Count == 3)
            {
                switch (raiz.ChildNodes[1].Term.ToString())
                {
                    case "||":
                        return respuesta = andLogico(raiz.ChildNodes[0], raiz.ChildNodes[2]);
                    case "&&":
                        return respuesta = orLogico(raiz.ChildNodes[0], raiz.ChildNodes[2]);
                    default:
                        if (retornarTerminal(raiz) != null) 
                        Program.getVentana().agregarError("Error desconocido" + raiz.ChildNodes[1].Term.ToString(), "Semantico", -1, -1, "");
                        else
                                    Program.getVentana().agregarError("Error, no existe", "Semantico", -1, -1, "no encontrado");
                        break;

                }
                return null;
            }
            else if (raiz.ChildNodes.Count == 2){
                Expresion a = new Expresion(nodo.ChildNodes[1], tFunc);
                a.ejecutar(actual,tFunc);
                if (a.respuesta is bool)
                {
                    return respuesta = !(bool)a.respuesta;
                }
            }
            else
            {
                if(retornarTerminal(raiz)!=null)
                Program.getVentana().agregarError("Error en la operacion logica, datos incompatibles", "Semantico", raiz.Token.Location.Line, raiz.Token.Location.Column, raiz.Token.Text);
                else
                    Program.getVentana().agregarError("Error, no existe", "Semantico", -1, -1, "no encontrado");
            }
            return null;
        }
        object ejecutarRE(ParseTreeNode raiz)
        {
            if (raiz.ChildNodes.Count == 3)
            {
                try
                {
                    switch (raiz.ChildNodes[1].Term.ToString())
                    {
                        case ">":
                            return respuesta = comparar(raiz.ChildNodes[0], raiz.ChildNodes[2]);
                        case "<":
                            return respuesta = comparar(raiz.ChildNodes[2], raiz.ChildNodes[0]);
                        case "==":
                            return respuesta = igualacion(raiz.ChildNodes[0], raiz.ChildNodes[2]);
                        case ">=":
                            return respuesta = compararIgual(raiz.ChildNodes[0], raiz.ChildNodes[2]);
                        case "<=":
                            return respuesta = compararIgual(raiz.ChildNodes[2], raiz.ChildNodes[0]);
                        case "!=":
                            return respuesta = diferente(raiz.ChildNodes[0], raiz.ChildNodes[2]);
                        default:
                            if(retornarTerminal(raiz)!=null)
                            Program.getVentana().agregarError("Error desconocido", "Semantico", raiz.Token.Location.Line, raiz.Token.Location.Column, raiz.Token.Text);
                            else
                                Program.getVentana().agregarError("Error, no existe", "Semantico", -1, -1, "no encontrado");
                            break;

                    }
                }
                catch(NullReferenceException e)
                {
                    if(retornarTerminal(raiz)!=null)
                    Program.getVentana().agregarError("No se puede hacer esa comparacion", "Semantico", retornarTerminal(raiz).Token.Location.Line, retornarTerminal(raiz).Token.Location.Column, retornarTerminal(raiz).Token.Text);
                    else
                        Program.getVentana().agregarError("Error, no existe", "Semantico", -1, -1, "no encontrado");
                    return null;
                }
                
            }
            return null;
        }
        object suma(ParseTreeNode op1, ParseTreeNode op2)
        {
            Expresion a = new Expresion(op1, tFunc); Expresion b = new Expresion(op2, tFunc);
            a.ejecutar(actual,tFunc);
            b.ejecutar(actual,tFunc);
            object respuesta;
            //Int
            if(a.respuesta is int && b.respuesta is int)
            {
                return respuesta = (int)a.respuesta + (int)b.respuesta;
            }
            else if (a.respuesta is char && b.respuesta is char)
            {
                return respuesta = (int)((char)a.respuesta) + (int)((char)b.respuesta);
            }
            else if (a.respuesta is int && b.respuesta is char)
            {
                return respuesta = (int)a.respuesta + (int)((char)b.respuesta);
            }
            else if (a.respuesta is char && b.respuesta is int)
            {
                return respuesta = (int)((char)a.respuesta) + (int)b.respuesta;
            }
            
            else if (a.respuesta is int && b.respuesta is bool)
            {
                int exp1 = (bool)b.respuesta ? 1 : 0;
                return respuesta = (int)a.respuesta + exp1;
            }
            else if (a.respuesta is bool && b.respuesta is int)
            {
                int exp1 = (bool)a.respuesta ? 1 : 0;
                return respuesta = exp1 + (int)b.respuesta;

            }
            else if (a.respuesta is bool && b.respuesta is char)
            {
                int exp1 = (bool)a.respuesta ? 1 : 0;
                return respuesta = exp1 + (char)b.respuesta;

            }
            else if (a.respuesta is char && b.respuesta is bool)
            {
                int exp1 = (bool)b.respuesta ? 1 : 0;
                return respuesta = exp1 + (char)a.respuesta;

            }
            //BOOLEANO
            else if (a.respuesta is bool && b.respuesta is bool)
            {
                return respuesta = ((bool)a.respuesta) || ((bool)b.respuesta);
            }
            //DOUBLE
            else if(a.respuesta is int && b.respuesta is double)
            {
                return respuesta = (int)a.respuesta + (double)b.respuesta;
            }
            else if (a.respuesta is double && b.respuesta is int)
            {
                return respuesta = (double)a.respuesta + (int)b.respuesta;
            }
            else if (a.respuesta is double && b.respuesta is char)
            {
                return respuesta = (double)a.respuesta + (char)b.respuesta;
            }
            else if (a.respuesta is double && b.respuesta is bool)
            {
                int exp = (bool)b.respuesta ? 0 : 1;
                return respuesta = (double)a.respuesta + exp;
            }
            else if (a.respuesta is char && b.respuesta is double)
            {
                return respuesta = (char)a.respuesta + (double)b.respuesta;
            }
            else if (a.respuesta is bool && b.respuesta is double)
            {
                int exp = (bool)a.respuesta ? 0 : 1;
                return respuesta = exp + (double)b.respuesta;
            }
            else if (a.respuesta is double && b.respuesta is double)
            {
                return respuesta = (double)a.respuesta + (double)b.respuesta;
            }
            //STRING
            else if ((a.respuesta is string && b.respuesta is string)
               || (a.respuesta is string && b.respuesta is int)
               || (a.respuesta is int && b.respuesta is string)
               || (a.respuesta is string && b.respuesta is double)
               || (a.respuesta is double && b.respuesta is string)
               || (a.respuesta is string && b.respuesta is char)
               || (a.respuesta is char && b.respuesta is string)
               || (a.respuesta is string && b.respuesta is bool)
               || (a.respuesta is bool && b.respuesta is string))
            {
                return respuesta = a.respuesta.ToString() + b.respuesta.ToString();
            }
            else
            {
                Program.getVentana().agregarError("Error en la suma, datos incompatibles", "Semantico", retornarTerminal(op1).Token.Location.Line, retornarTerminal(op1).Token.Location.Column,(retornarTerminal(op1).Token.Text));
                return null;
            }

        }
        object resta(ParseTreeNode op1, ParseTreeNode op2)
        {
            Expresion a = new Expresion(op1, tFunc); Expresion b = new Expresion(op2, tFunc);
            a.ejecutar(actual,tFunc);
            b.ejecutar(actual,tFunc);
            object respuesta;
            //Int
            if(a.respuesta!=null && b.respuesta != null)
            {
                if (a.respuesta is int && b.respuesta is int)
                {
                    return respuesta = (int)a.respuesta - (int)b.respuesta;
                }
                else if (a.respuesta is char && b.respuesta is char)
                {
                    return respuesta = (int)((char)a.respuesta) - (int)((char)b.respuesta);
                }
                else if (a.respuesta is int && b.respuesta is char)
                {
                    return respuesta = (int)a.respuesta - (int)((char)b.respuesta);
                }
                else if (a.respuesta is char && b.respuesta is int)
                {
                    return respuesta = (int)((char)a.respuesta) - (int)b.respuesta;
                }

                else if (a.respuesta is int && b.respuesta is bool)
                {
                    int exp1 = (bool)b.respuesta ? 1 : 0;
                    return respuesta = (int)a.respuesta - exp1;
                }
                else if (a.respuesta is bool && b.respuesta is int)
                {
                    int exp1 = (bool)a.respuesta ? 1 : 0;
                    return respuesta = exp1 - (int)b.respuesta;

                }
                //DOUBLE
                else if (a.respuesta is int && b.respuesta is double)
                {
                    return respuesta = (int)a.respuesta - (double)b.respuesta;
                }
                else if (a.respuesta is double && b.respuesta is int)
                {
                    return respuesta = (double)a.respuesta - (int)b.respuesta;
                }
                else if (a.respuesta is double && b.respuesta is char)
                {
                    return respuesta = (double)a.respuesta - (char)b.respuesta;
                }
                else if (a.respuesta is double && b.respuesta is bool)
                {
                    int exp = (bool)b.respuesta ? 0 : 1;
                    return respuesta = (double)a.respuesta - exp;
                }
                else if (a.respuesta is char && b.respuesta is double)
                {
                    return respuesta = (char)a.respuesta - (double)b.respuesta;
                }
                else if (a.respuesta is bool && b.respuesta is double)
                {
                    int exp = (bool)a.respuesta ? 0 : 1;
                    return respuesta = exp - (double)b.respuesta;
                }
                else if (a.respuesta is double && b.respuesta is double)
                {
                    return respuesta = (double)a.respuesta - (double)b.respuesta;
                }
                else
                {
                    if(retornarTerminal(op1)!=null)
                    Program.getVentana().agregarError("Error en la resta, datos incompatibles", "Semantico", retornarTerminal(op1).Token.Location.Line, retornarTerminal(op1).Token.Location.Column, retornarTerminal(op1).Token.Text);
                    else
                        Program.getVentana().agregarError("Error, no existe", "Semantico", -1, -1, "no encontrado");
                    return null;
                }
            }
            else
            {
                return null;
            }
            
        }
        object multiplicacion(ParseTreeNode op1, ParseTreeNode op2)
        {
            Expresion a = new Expresion(op1, tFunc); Expresion b = new Expresion(op2, tFunc);
            a.ejecutar(actual,tFunc); b.ejecutar(actual,tFunc);
            object respuesta;
            //Int
            if (a.respuesta is int && b.respuesta is int)
            {
                return respuesta = (int)a.respuesta * (int)b.respuesta;
            }
            else if (a.respuesta is char && b.respuesta is char)
            {
                return respuesta = (int)((char)a.respuesta) * (int)((char)b.respuesta);
            }
            else if (a.respuesta is int && b.respuesta is char)
            {
                return respuesta = (int)a.respuesta * (int)((char)b.respuesta);
            }
            else if (a.respuesta is char && b.respuesta is int)
            {
                return respuesta = (int)((char)a.respuesta) * (int)b.respuesta;
            }

            else if (a.respuesta is int && b.respuesta is bool)
            {
                int exp1 = (bool)b.respuesta ? 1 : 0;
                return respuesta = (int)a.respuesta * exp1;
            }
            else if (a.respuesta is bool && b.respuesta is int)
            {
                int exp1 = (bool)a.respuesta ? 1 : 0;
                return respuesta = exp1 * (int)b.respuesta;

            }
            else if (a.respuesta is bool && b.respuesta is char)
            {
                int exp1 = (bool)a.respuesta ? 1 : 0;
                return respuesta = exp1 * (char)b.respuesta;

            }
            else if (a.respuesta is char && b.respuesta is bool)
            {
                int exp1 = (bool)b.respuesta ? 1 : 0;
                return respuesta = exp1 * (char)a.respuesta;

            }
            //BOOLEANO
            else if (a.respuesta is bool && b.respuesta is bool)
            {
                return respuesta = ((bool)a.respuesta) && ((bool)b.respuesta);
            }
            //DOUBLE
            else if (a.respuesta is int && b.respuesta is double)
            {
                return respuesta = (int)a.respuesta * (double)b.respuesta;
            }
            else if (a.respuesta is double && b.respuesta is int)
            {
                return respuesta = (double)a.respuesta * (int)b.respuesta;
            }
            else if (a.respuesta is double && b.respuesta is char)
            {
                return respuesta = (double)a.respuesta * (char)b.respuesta;
            }
            else if (a.respuesta is double && b.respuesta is bool)
            {
                int exp = (bool)b.respuesta ? 0 : 1;
                return respuesta = (double)a.respuesta * exp;
            }
            else if (a.respuesta is char && b.respuesta is double)
            {
                return respuesta = (char)a.respuesta * (double)b.respuesta;
            }
            else if (a.respuesta is bool && b.respuesta is double)
            {
                int exp = (bool)a.respuesta ? 0 : 1;
                return respuesta = exp * (double)b.respuesta;
            }
            else if (a.respuesta is double && b.respuesta is double)
            {
                return respuesta = (double)a.respuesta * (double)b.respuesta;
            }
            else
            {
                if(retornarTerminal(op1)!=null)
                Program.getVentana().agregarError("Error en la multiplicacion, datos incompatibles", "Semantico", retornarTerminal(op1).Token.Location.Line, retornarTerminal(op1).Token.Location.Column, retornarTerminal(op1).Token.Text);
                else
                    Program.getVentana().agregarError("Error, no existe", "Semantico", -1, -1, "no encontrado");
                return null;
            }
        }
        object division(ParseTreeNode op1, ParseTreeNode op2)
        {
            Expresion a = new Expresion(op1, tFunc); Expresion b = new Expresion(op2, tFunc);
            a.ejecutar(actual,tFunc); b.ejecutar(actual,tFunc);
            object respuesta;
            if (a.respuesta is int && b.respuesta is int)
            {
                if ((int)b.respuesta != 0)
                {
                    return respuesta = Convert.ToDouble(a.respuesta) / Convert.ToDouble(b.respuesta);
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, op1.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is char && b.respuesta is char)
            {
                if ((int)b.respuesta != 0)
                {
                    return respuesta = (double)a.respuesta / (double)b.respuesta;
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, nodo.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is int && b.respuesta is char)
            {
                if (Convert.ToInt32(b.respuesta) != 0)
                {
                    return respuesta = Convert.ToDouble(Convert.ToInt32(a.respuesta)) / Convert.ToDouble(Convert.ToInt32(b.respuesta));
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, nodo.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is char && b.respuesta is int)
            {
                if ((int)b.respuesta != 0)
                {
                    return respuesta = (double)a.respuesta / (double)b.respuesta;
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, nodo.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is double && b.respuesta is double)
            {
                if ((double)b.respuesta != 0.0)
                {
                    return respuesta = (double)a.respuesta / (double)b.respuesta;
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, nodo.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is int && b.respuesta is double)
            {
                if ((double)b.respuesta != 0.0)
                {
                    return respuesta = (int)a.respuesta / (double)b.respuesta;
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, nodo.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is double && b.respuesta is int)
            {
                if ((int)b.respuesta != 0)
                {
                    return respuesta = (double)a.respuesta / (int)b.respuesta;
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, op1.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is double && b.respuesta is char)
            {
                if ((int)b.respuesta != 0)
                {
                    return respuesta = (double)a.respuesta / (int)b.respuesta;
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, op1.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is char && b.respuesta is double)
            {
                if ((double)b.respuesta != 0.0)
                {
                    return respuesta = (int)a.respuesta / (double)b.respuesta;
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, op1.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is double && b.respuesta is bool)
            {
                int exp = (bool)b.respuesta ? 1 : 0;
                if (exp != 0)
                {
                    return respuesta = (double)a.respuesta / (double)exp;
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, op1.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is int && b.respuesta is bool)
            {
                int exp = (bool)b.respuesta ? 1 : 0;
                if (exp != 0)
                {
                    return respuesta = (int)a.respuesta / exp;
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, nodo.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is bool && b.respuesta is double)
            {
                int exp = (bool)a.respuesta ? 1 : 0;
                if ((double)b.respuesta != 0.0)
                {
                    return respuesta = exp / (double)b.respuesta;
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, nodo.Token.Text);
                    return null;
                }
            }
            else if (a.respuesta is bool && b.respuesta is int)
            {

                if ((int)b.respuesta != 0)
                {
                    int exp = (bool)a.respuesta ? 0 : 1;
                    return respuesta = exp / (int)b.respuesta;
                }
                else
                {
                    Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", nodo.Token.Location.Line, nodo.Token.Location.Column, nodo.Token.Text);
                    return null;
                }
            }
            else
            {
                //if (getTerminal(expresionA) != null)
                //  errorSemantico(getTerminal(expresionA).Token.Text, ",Error de división valores incompatibles", getTerminal(expresionA).Token.Location.Line, getTerminal(expresionA).Token.Location.Column);
                //else
                //   errorSemantico("Token", ",Error de división valores incompatibles no encontrado", -1, -1);
                if(retornarTerminal(op1) != null)
                Program.getVentana().agregarError("Error en la division, valores incompatibles", "Semantico", retornarTerminal(op1).Token.Location.Line, retornarTerminal(op1).Token.Location.Column, retornarTerminal(op1).Token.Text);
                else
                    Program.getVentana().agregarError("Error en la division, valores incompatibles", "Semantico", -1,-1,"no encontrado");

                return respuesta = null;
            }
        }
        object potencia(ParseTreeNode op1, ParseTreeNode op2)
        {
            Expresion a = new Expresion(op1, tFunc); Expresion b = new Expresion(op2, tFunc);
            a.ejecutar(actual,tFunc); b.ejecutar(actual,tFunc);
            object respuesta;
            //Int
            if (a.respuesta is int && b.respuesta is int)
            {
                return respuesta = (int)Math.Pow((int)a.respuesta, (int)b.respuesta);
            }
            else if (a.respuesta is char && b.respuesta is char)
            {
                return respuesta = (int)Math.Pow((double)((char)a.respuesta), (int)((char)b.respuesta));
            }
            else if ((a.respuesta is int && b.respuesta is char))
            {
                return respuesta = (int)Math.Pow((double)(a.respuesta), (int)((char)b.respuesta));
            }
            else if ((b.respuesta is int && a.respuesta is char))
            {
                return respuesta = (int)Math.Pow((char)(a.respuesta), ((double)b.respuesta));
            }
            else if (a.respuesta is int && b.respuesta is bool)
            {
                int exp = (bool)b.respuesta ? 1 : 0;
                return respuesta = (int)Math.Pow((int)a.respuesta, exp);
            }
            else if (a.respuesta is bool && b.respuesta is int)
            {
                int exp = (bool)a.respuesta ? 1 : 0;
                return respuesta =(int) Math.Pow(exp, (int)b.respuesta);
            }
            else if (a.respuesta is double && b.respuesta is double)
            {
                return respuesta = Math.Pow((double)a.respuesta, (double)b.respuesta);
            }
            else if (a.respuesta is int && b.respuesta is double)
            {
                return respuesta = Math.Pow((int)a.respuesta, (double)b.respuesta);

            }
            else if (a.respuesta is double && b.respuesta is int)
            {
                return respuesta = Math.Pow((double)a.respuesta, (int)b.respuesta);

            }
            else if (a.respuesta is double && b.respuesta is bool)
            {

                int exp = (bool)b.respuesta ? 1 : 0;
                return respuesta = Math.Pow((double)a.respuesta, (double)exp);

            }
            else if (a.respuesta is bool && b.respuesta is double)
            {
                int exp = (bool)a.respuesta ? 1 : 0;
                return respuesta = Math.Pow((double)exp, (double)b.respuesta);

            }
            else if (a.respuesta is double && b.respuesta is char)
            {
                return respuesta = Math.Pow((double)a.respuesta, (int)((char)b.respuesta));

            }
            else if (a.respuesta is char && b.respuesta is double)
            {
                return respuesta = Math.Pow((double)a.respuesta, (int)((char)b.respuesta));

            }
            else if (a.respuesta is bool && b.respuesta is char)
            {
                int exp = (bool)a.respuesta ? 0 : 1;
                return respuesta = (int)Math.Pow(exp, (double)((char)b.respuesta));

            }
            else if (a.respuesta is char && b.respuesta is bool)
            {
                int exp = (bool)b.respuesta ? 0 : 1;
                return respuesta = (int)Math.Pow(exp, (double)((char)b.respuesta));

            }
            else
            {
                if(retornarTerminal(op1)!=null)
                Program.getVentana().agregarError("Error en la potencia, datos incompatibles", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, op1.Token.Text);
                //if (getTerminal(expresionA) != null)
                //  errorSemantico(getTerminal(expresionA).Token.Text, ",Error de potencia valores incompatibles", getTerminal(expresionA).Token.Location.Line, getTerminal(expresionA).Token.Location.Column);
                //else
                //  errorSemantico("Token", ",Error de potencia valores incompatibles no encontrado", -1, -1);
                else
                    Program.getVentana().agregarError("Error en la division, valores incompatibles", "Semantico", -1, -1, "no encontrado");
                return respuesta = null;
            }
        }
        object menosU(ParseTreeNode op1)
        {
            Expresion a = new Expresion(op1, tFunc);
            a.ejecutar(actual, tFunc);
            if(a.respuesta is int)
            {
                return respuesta = 0 - (int)a.respuesta;
            }
            else if (a.respuesta is double)
            {
                return respuesta = 0.0 - (double)a.respuesta;
            }
            else if (a.respuesta is char)
            {
                return respuesta = 0 -(int)(char)a.respuesta;
            }
            else
            {
                if(retornarTerminal(op1)!=null)
                Program.getVentana().agregarError("Error en la negacion, datos incompatibles", "Semantico", retornarTerminal(op1).Token.Location.Line, retornarTerminal(op1).Token.Location.Column, retornarTerminal(op1).Token.Text);
                else
                    Program.getVentana().agregarError("Error en la division, valores incompatibles", "Semantico", -1, -1, "no encontrado");
                return null;
            }
        }
        object orLogico(ParseTreeNode op1, ParseTreeNode op2)
        {
            Expresion a = new Expresion(op1, tFunc); Expresion b = new Expresion(op2, tFunc);
            a.ejecutar(actual,tFunc); b.ejecutar(actual,tFunc);
            object respuesta;
            if (a.respuesta is bool && a.respuesta is bool)
            {
                return respuesta = (bool)a.respuesta || (bool)b.respuesta;
            }
            else
            {
                if(retornarTerminal(op1)!=null)
                Program.getVentana().agregarError("Error en el or, datos incompatibles", "Semantico", retornarTerminal(op1).Token.Location.Line, retornarTerminal(op1).Token.Location.Column, retornarTerminal(op1).Token.Text);
                else
                    Program.getVentana().agregarError("Error en la division, valores incompatibles", "Semantico", -1, -1, "no encontrado");
                return respuesta = null;
            }
        }
        object andLogico(ParseTreeNode op1, ParseTreeNode op2)
        {
            Expresion a = new Expresion(op1, tFunc); Expresion b = new Expresion(op2, tFunc);
            a.ejecutar(actual,tFunc); b.ejecutar(actual,tFunc);
            object respuesta;
            if (a.respuesta is bool && a.respuesta is bool)
            {
                return respuesta = (bool)a.respuesta && (bool)b.respuesta;
            }
            else
            {
                if(retornarTerminal(op1)!=null)
                Program.getVentana().agregarError("Error en el and, datos incompatibles", "Semantico", op1.Token.Location.Line, op1.Token.Location.Column, op1.Token.Text);
                return respuesta = null;
            }
        }
        object igualacion(ParseTreeNode op1, ParseTreeNode op2)
        {
            Expresion a = new Expresion(op1, tFunc); Expresion b = new Expresion(op2, tFunc);
            a.ejecutar(actual,tFunc);
            b.ejecutar(actual,tFunc);
            object respuesta;
            //Int
            if (a.respuesta is int && b.respuesta is int)
            {
                return respuesta = (int)a.respuesta == (int)b.respuesta;
            }
            else if (a.respuesta is bool && b.respuesta is bool)
            {
                return respuesta = (bool)a.respuesta == (bool)b.respuesta;
            }
            else if (a.respuesta is char && b.respuesta is char)
            {
                return respuesta = (int)((char)a.respuesta) == (int)((char)b.respuesta);
            }
            else if ((a.respuesta is int && b.respuesta is char))
            {
                return respuesta = (int)(a.respuesta) == (int)((char)b.respuesta);
            }
            else if ((b.respuesta is int && a.respuesta is char))
            {
                return respuesta = (int)(b.respuesta) == (int)((char)a.respuesta);
            }
            else if (a.respuesta is int && b.respuesta is bool)
            {
                int exp = (bool)b.respuesta ? 1 : 0;
                return respuesta = (int)a.respuesta == exp;
            }
            else if (a.respuesta is bool && b.respuesta is int)
            {
                int exp = (bool)a.respuesta ? 1 : 0;
                return respuesta = exp == (int)b.respuesta;
            }
            //Double
            else if (a.respuesta is double && b.respuesta is double)
            {
                return respuesta = (double)a.respuesta == (double)b.respuesta;
            }
            else if (a.respuesta is int && b.respuesta is double)
            {
                return respuesta = (int)a.respuesta == (double)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is int)
            {
                return respuesta = (double)a.respuesta == (int)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is bool)
            {

                int exp = (bool)b.respuesta ? 1 : 0;
                return respuesta = (double)a.respuesta == (double)exp;

            }
            else if (a.respuesta is bool && b.respuesta is double)
            {
                int exp = (bool)a.respuesta ? 1 : 0;
                return respuesta = (double)exp == (double)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is char)
            {
                return respuesta = (double)a.respuesta == (int)((char)b.respuesta);

            }
            else if (a.respuesta is char && b.respuesta is double)
            {
                return respuesta = (double)b.respuesta == (int)((char)a.respuesta);

            }//String
            else if (a.respuesta is string && b.respuesta is string)
            {
                return respuesta = a.respuesta.ToString() == a.respuesta.ToString();
            }
            else
            {
                Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", -1, nodo.Token.Location.Column, nodo.Token.Text);

                return respuesta = null;
            }
        }
        object diferente(ParseTreeNode op1, ParseTreeNode op2)
        {
            Expresion a = new Expresion(op1, tFunc); Expresion b = new Expresion(op2, tFunc);
            a.ejecutar(actual,tFunc); b.ejecutar(actual,tFunc);
            object respuesta;
            //Int
            if (a.respuesta is int && b.respuesta is int)
            {
                return respuesta = (int)a.respuesta != (int)b.respuesta;
            }
            else if (a.respuesta is bool && b.respuesta is bool)
            {
                return respuesta = (bool)a.respuesta != (bool)b.respuesta;
            }
            else if (a.respuesta is char && b.respuesta is char)
            {
                return respuesta = (int)((char)a.respuesta) != (int)((char)b.respuesta);
            }
            else if ((a.respuesta is int && b.respuesta is char))
            {
                return respuesta = (int)(a.respuesta) != (int)((char)b.respuesta);
            }
            else if ((b.respuesta is int && a.respuesta is char))
            {
                return respuesta = (int)(b.respuesta) != (int)((char)a.respuesta);
            }
            else if (a.respuesta is int && b.respuesta is bool)
            {
                int exp = (bool)b.respuesta ? 1 : 0;
                return respuesta = (int)a.respuesta != exp;
            }
            else if (a.respuesta is bool && b.respuesta is int)
            {
                int exp = (bool)a.respuesta ? 1 : 0;
                return respuesta = exp != (int)b.respuesta;
            }
            //Double
            else if (a.respuesta is double && b.respuesta is double)
            {
                return respuesta = (double)a.respuesta != (double)b.respuesta;
            }
            else if (a.respuesta is int && b.respuesta is double)
            {
                return respuesta = (int)a.respuesta != (double)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is int)
            {
                return respuesta = (double)a.respuesta != (int)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is bool)
            {

                int exp = (bool)b.respuesta ? 1 : 0;
                return respuesta = (double)a.respuesta != (double)exp;

            }
            else if (a.respuesta is bool && b.respuesta is double)
            {
                int exp = (bool)a.respuesta ? 1 : 0;
                return respuesta = (double)exp != (double)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is char)
            {
                return respuesta = (double)a.respuesta != (int)((char)b.respuesta);

            }
            else if (a.respuesta is char && b.respuesta is double)
            {
                return respuesta = (double)b.respuesta != (int)((char)a.respuesta);

            }//String
            else if (a.respuesta is string && b.respuesta is string)
            {
                return respuesta = a.respuesta.ToString() != a.respuesta.ToString();
            }
            else
            {
                Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", nodo.Token.Location.Line, nodo.Token.Location.Column, nodo.Token.Text);

                return respuesta = null;
            }
        }
        object comparar(ParseTreeNode op1, ParseTreeNode op2)
        {
            Expresion a = new Expresion(op1, tFunc); Expresion b = new Expresion(op2, tFunc);
            a.ejecutar(actual,tFunc);
            b.ejecutar(actual,tFunc);
            object respuesta;
            //Int
            if (a.respuesta is int && b.respuesta is int)
            {
                return respuesta = (int)a.respuesta > (int)b.respuesta;
            }
            else if (a.respuesta is char && b.respuesta is char)
            {
                return respuesta = (int)((char)a.respuesta) > (int)((char)b.respuesta);
            }
            else if ((a.respuesta is int && b.respuesta is char))
            {
                return respuesta = (int)(a.respuesta) > (int)((char)b.respuesta);
            }
            else if ((b.respuesta is int && a.respuesta is char))
            {
                return respuesta = (int)(b.respuesta) > (int)((char)a.respuesta);
            }
            else if (a.respuesta is int && b.respuesta is bool)
            {
                int exp = (bool)b.respuesta ? 1 : 0;
                return respuesta = (int)a.respuesta > exp;
            }
            else if (a.respuesta is bool && b.respuesta is int)
            {
                int exp = (bool)a.respuesta ? 1 : 0;
                return respuesta = exp > (int)b.respuesta;
            }
            //Double
            else if (a.respuesta is double && b.respuesta is double)
            {
                return respuesta = (double)a.respuesta > (double)b.respuesta;
            }
            else if (a.respuesta is int && b.respuesta is double)
            {
                return respuesta = (int)a.respuesta > (double)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is int)
            {
                return respuesta = (double)a.respuesta > (int)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is bool)
            {

                int exp = (bool)b.respuesta ? 1 : 0;
                return respuesta = (double)a.respuesta > (double)exp;

            }
            else if (a.respuesta is bool && b.respuesta is double)
            {
                int exp = (bool)a.respuesta ? 1 : 0;
                return respuesta = exp > (double)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is char)
            {
                return respuesta = (double)a.respuesta > (int)((char)b.respuesta);

            }
            else if (a.respuesta is char && b.respuesta is double)
            {
                return respuesta = (double)b.respuesta > (int)((char)a.respuesta);

            }//String
            else if(a.respuesta is string && b.respuesta is string)
            {
                if (string.Compare(a.respuesta.ToString(), b.respuesta.ToString()) > 1)
                {
                    return respuesta = true;
                }
                return respuesta = false;
            }
            else
            {
                Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", nodo.Token.Location.Line, nodo.Token.Location.Column, nodo.Token.Text);
                return respuesta = null;
            }
        }
        object compararIgual(ParseTreeNode op1, ParseTreeNode op2)
        {
            Expresion a = new Expresion(op1, tFunc); Expresion b = new Expresion(op2, tFunc);
            a.ejecutar(actual,tFunc); b.ejecutar(actual,tFunc);
            object respuesta;
            //Int
            if (a.respuesta is int && b.respuesta is int)
            {
                return respuesta = (int)a.respuesta >= (int)b.respuesta;
            }
            else if (a.respuesta is char && b.respuesta is char)
            {
                return respuesta = (int)((char)a.respuesta) >= (int)((char)b.respuesta);
            }
            else if ((a.respuesta is int && b.respuesta is char))
            {
                return respuesta = (int)(a.respuesta) >= (int)((char)b.respuesta);
            }
            else if ((b.respuesta is int && a.respuesta is char))
            {
                return respuesta = (int)(b.respuesta) >= (int)((char)a.respuesta);
            }
            else if (a.respuesta is int && b.respuesta is bool)
            {
                int exp = (bool)b.respuesta ? 1 : 0;
                return respuesta = (int)a.respuesta >= exp;
            }
            else if (a.respuesta is bool && b.respuesta is int)
            {
                int exp = (bool)a.respuesta ? 1 : 0;
                return respuesta = exp >= (int)b.respuesta;
            }
            //Double
            else if (a.respuesta is double && b.respuesta is double)
            {
                return respuesta = (double)a.respuesta >= (double)b.respuesta;
            }
            else if (a.respuesta is int && b.respuesta is double)
            {
                return respuesta = (int)a.respuesta >= (double)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is int)
            {
                return respuesta = (double)a.respuesta >= (int)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is bool)
            {

                int exp = (bool)b.respuesta ? 1 : 0;
                return respuesta = (double)a.respuesta >= (double)exp;

            }
            else if (a.respuesta is bool && b.respuesta is double)
            {
                int exp = (bool)a.respuesta ? 1 : 0;
                return respuesta = (double)exp >= (double)b.respuesta;

            }
            else if (a.respuesta is double && b.respuesta is char)
            {
                return respuesta = (double)a.respuesta >= (int)((char)b.respuesta);

            }
            else if (a.respuesta is char && b.respuesta is double)
            {
                return respuesta = (double)b.respuesta >= (int)((char)a.respuesta);

            }
            else if (a.respuesta is string && b.respuesta is string)
            {
                if (string.Compare(a.respuesta.ToString(), b.respuesta.ToString()) > 1|| string.Compare(a.respuesta.ToString(), b.respuesta.ToString()) ==0)
                {
                    return respuesta = true;
                }
                return respuesta = false;
            }//String
            else
            {

                Program.getVentana().agregarError("Error en la division, no se puede dividir entre 0", "Semantico", nodo.Token.Location.Line, nodo.Token.Location.Column, nodo.Token.Text);

                return respuesta = null;
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
                foreach(ParseTreeNode hijo in nodo.ChildNodes)
                {
                    return retornarTerminal(hijo);
                }
            }
            return null;
        }

    }
}

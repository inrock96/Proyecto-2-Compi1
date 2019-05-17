using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Gramatica
{
    class Sintactico
    {
        public static ParseTree padre;
        public List<ErrorSemantico> erroresS;
        public List<ErrorSemantico> erroresL;
        public Sintactico()
        {
            erroresS = new List<ErrorSemantico>();
            erroresL = new List<ErrorSemantico>();
        }
        public bool esCadenaValida(string cadenaEntrada, Grammar grammar)
        {
            LanguageData language = new LanguageData(grammar);
            Parser p = new Parser(language);
            ParseTree arbol = p.Parse(cadenaEntrada);
            padre = arbol;
            return arbol.Root != null;
        }
        public ParseTreeNode analizar(string cadenaEntrada)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData language = new LanguageData(gramatica);
            Parser parser = new Parser(language);
            ParseTree arbol = parser.Parse(cadenaEntrada);
            ParseTreeNode raiz = arbol.Root;
            if (raiz != null)
            {
                return raiz;
            }
            else
            {

                int i;
                ErrorSemantico errorNuevo;
                for (i = 0; i < arbol.ParserMessages.Count(); i++)
                {
                    if (arbol.ParserMessages.ElementAt(i).Message.Contains("Syntax"))
                    {
                        errorNuevo = new ErrorSemantico(
                                            arbol.ParserMessages.ElementAt(i).Message,
                                            "SINTACTICO",
                                            arbol.ParserMessages.ElementAt(i).Location.Line+1,
                                            arbol.ParserMessages.ElementAt(i).Location.Column,
                                            arbol.ParserMessages.ElementAt(i).Level.ToString());
                    
                        //errorNuevo = new ErrorSemantico(arbol.ParserMessages.ElementAt(i).Message,)
                        erroresS.Add(errorNuevo);
                    }
                    else
                    {
                        errorNuevo = new ErrorSemantico(
                                            arbol.ParserMessages.ElementAt(i).Message,
                                            "LEXICO",
                                            arbol.ParserMessages.ElementAt(i).Location.Line+1,
                                            arbol.ParserMessages.ElementAt(i).Location.Column,
                                            arbol.ParserMessages.ElementAt(i).Level.ToString());
                        erroresL.Add(errorNuevo);
                    }


                }
                return null; //NO SE PUDO GENERAR EL ARBOLITO
            }
        }
    }
}

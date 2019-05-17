using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Gramatica
{
    class Gramatica:Grammar
    {
        public Gramatica():base(caseSensitive: false)
        {
            #region Terminales

            #region ER
            NumberLiteral numero = new NumberLiteral("numero");
            IdentifierTerminal identificador = new IdentifierTerminal("identificador");
            StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.AllowsAllEscapes);
            StringLiteral caracter = new StringLiteral("caracter", "'", StringOptions.IsChar);
            #endregion
            #region Comentarios 
            CommentTerminal comM = new CommentTerminal("comM", "<-", "->");
            CommentTerminal comU = new CommentTerminal("comU", ">>", "\n", "\r\n");
            base.NonGrammarTerminals.Add(comM);
            base.NonGrammarTerminals.Add(comU);
            #endregion
            #region Simbolos
            Terminal mas = ToTerm("+"),
                menos = ToTerm("-"),
                slash = ToTerm("/"),
                multi = ToTerm("*"),
                potencia = ToTerm("^"),
                igual = ToTerm("="),
                parC = ToTerm(")"),
                parA = ToTerm("("),
                corchC = ToTerm("]"),
                corchA = ToTerm("["),
                curlyA = ToTerm("{"),
                curlyC = ToTerm("}"),
                menorQue = ToTerm("<"),
                mayorQue = ToTerm(">"),
                menorIgual = ToTerm("<="),
                mayorIgual = ToTerm(">="),
                igualacion = ToTerm("=="),
                diferente = ToTerm("!="),
                orLogico = ToTerm("||"),
                andLogico = ToTerm("&&"),
                notLogico = ToTerm("!"),
                menosMenos = ToTerm("--"),
                masMas = ToTerm("++"),
                coma = ToTerm(","),
                puntoYComa = ToTerm(";"),
                dosPuntos = ToTerm(":"),
                punto = ToTerm("."),
                grfTrue = ToTerm("true", "grfTrue"),
                grfFalse = ToTerm("false", "grfFalse"),
                grfVerdadero = ToTerm("verdadero", "grfVerdadero"),
                grfFalso = ToTerm("falso", "grfFalso");
            #endregion
            #region Palabras Reservadas
            KeyTerm grfInt = ToTerm("int"),
               grfDouble = ToTerm("double"),
               grfChar = ToTerm("char"),
               grfString = ToTerm("string"),
               grfBool = ToTerm("bool"),
               grfArray = ToTerm("array"),
               grfVoid = ToTerm("void"),
               grfClase = ToTerm("clase"),
               grfImport = ToTerm("importar"),
               grfOverride = ToTerm("override"),
               grfPublic = ToTerm("publico"),
               grfPrivate = ToTerm("privado"),
               grfMain = ToTerm("main"),
               grfIf = ToTerm("if"),
               grfElse = ToTerm("else"),
               grfNew = ToTerm("new"),
               grfRepeat = ToTerm("repeat"),
               grfWhile = ToTerm("while"),
               grfDo = ToTerm("hacer"),
               grfMientras = ToTerm("mientras"),
               grfPrint = ToTerm("print"),
               grfBreak = ToTerm("salir"),
               grfContinue = ToTerm("continuar"),
               grfReturn = ToTerm("return"),
               grfShow = ToTerm("show"),
               grfCase = ToTerm("caso"),
               grfDefault = ToTerm("defecto"),
               grfSwitch = ToTerm("comprobar"),
               grfFor = ToTerm("for"),
               grfAddF = ToTerm("addfigure"),
               grfCircle = ToTerm("circle"),
               grfTriangle = ToTerm("triangle"),
               grfSquare = ToTerm("square"),
               grfLine = ToTerm("line"),
               grfFigure = ToTerm("figure")
               ;
            MarkReservedWords("int","double","char","string","bool","array","void","importar","override","publico",
                "privado","salir","main","if","else","new","repeat","while","hacer","mientras","print","continuar","show",
                "caso","defecto","comprobar","for","addfigure","circle","triangle","square","line","figure");
            #endregion

            #endregion
            #region No terminales
            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal CUERPO = new NonTerminal("CUERPO");
            NonTerminal CUERPO2 = new NonTerminal("CUERPO2");
            NonTerminal LISTASENTENCIAGLOBAL = new NonTerminal("LISTAGLOBAL");
            NonTerminal LISTASENTENCIA = new NonTerminal("LISTASENTENCIA");
            NonTerminal SENTENCIA = new NonTerminal("SENTENCIA");
            NonTerminal LISTAVAR = new NonTerminal("LISTAVAR");
            NonTerminal BLOQUE = new NonTerminal("BLOQUE");
            NonTerminal METODOS = new NonTerminal("METODOS");
            NonTerminal ASIGNACION = new NonTerminal("ASIGNACION");
            NonTerminal DECLARACIONES = new NonTerminal("DECLARACIONES");
            NonTerminal PRINCIPAL = new NonTerminal("PRINCIPAL");
            NonTerminal VISIBILIDAD = new NonTerminal("VISIBILIDAD");
            NonTerminal TIPO = new NonTerminal("TIPO");
            NonTerminal LISTAPARAM = new NonTerminal("LISTAPARAM");
            NonTerminal PARAM = new NonTerminal("PARAM");
            NonTerminal DECLARACIONGLOBAL = new NonTerminal("DECGLOBAL");
            NonTerminal EXP = new NonTerminal("EXP");
            NonTerminal DIMENSIONES = new NonTerminal("DIMENSIONES");
            NonTerminal DIMENSION = new NonTerminal("DIMENSION");
            NonTerminal LISTAEXP = new NonTerminal("LISTAEXP");
            NonTerminal LISTACURLY = new NonTerminal("LISTACURLY");
            NonTerminal ASIGARREGLO = new NonTerminal("ASIGARREGLO");
            NonTerminal ARREGLO = new NonTerminal("ARREGLO");
            NonTerminal EXP_AR = new NonTerminal("EXP_AR");
            NonTerminal EXP_LO = new NonTerminal("EXP_LO");
            NonTerminal EXP_RE = new NonTerminal("EXP_RE");
            NonTerminal PRIMITIVO = new NonTerminal("PRIMITIVO");
            NonTerminal BLOQUEGLOBAL = new NonTerminal("BLOQUEGLOBAL");
            NonTerminal SENTENCIAIF = new NonTerminal("IF");
            NonTerminal SENTENCIADOWHILE = new NonTerminal("DOWHILE");
            NonTerminal SENTENCIAWHILE = new NonTerminal("WHILE");
            NonTerminal SENTENCIABREAK = new NonTerminal("BREAK");
            NonTerminal SENTENCIAFOR = new NonTerminal("FOR");
            NonTerminal SENTENCIASHOW = new NonTerminal("SHOW");
            NonTerminal SENTENCIAACCESO = new NonTerminal("ACCESO");
            NonTerminal SENTENCIAIMPRIMIR = new NonTerminal("IMPRIMIR");
            NonTerminal SENTENCIALLAMADA = new NonTerminal("LLAMADA");
            NonTerminal SENTENCIACONTINUE = new NonTerminal("CONTINUE");
            NonTerminal SENTENCIARETURN = new NonTerminal("RETURN");
            NonTerminal CONSTRUCTOR = new NonTerminal("CONSTRUCTOR");
            NonTerminal INCREMENTO = new NonTerminal("INCREMENTO");
            NonTerminal DECREMENTO = new NonTerminal("DECREMENTO");
            NonTerminal INCDEC = new NonTerminal("INCDEC");
            NonTerminal IDARREGLO = new NonTerminal("IDARREGLO");
            NonTerminal INSTRFOR = new NonTerminal("INSTRFOR");
            NonTerminal SENTENCIAGLOBAL = new NonTerminal("SENTENCIAGLOBAL");
            NonTerminal LISTACURLY2 = new NonTerminal("LISTACURLY2");
            NonTerminal LISTADECURLYS = new NonTerminal("LISTADECURLYS");
            NonTerminal SENTENCIAELSEIF = new NonTerminal("ELSEIF");
            NonTerminal SENTENCIAREPEAT = new NonTerminal("REPEAT");
            NonTerminal SENTENCIASWITCH = new NonTerminal("SWITCH");
            NonTerminal CASOS = new NonTerminal("CASOS");
            NonTerminal CASO = new NonTerminal("CASO");
            NonTerminal LISTACASO = new NonTerminal("LISTACASO");
            NonTerminal DEFECTO = new NonTerminal("DEFECTO");
            NonTerminal ADDFIGURE = new NonTerminal("ADDFIGURE");
            NonTerminal FIGURE = new NonTerminal("FIGURE");
            NonTerminal LISTAELSEIF = new NonTerminal("LISTAELSEIF");
            NonTerminal IFELSE = new NonTerminal("IFELSE");
            NonTerminal TIPOFIGURA = new NonTerminal("TIPOFIGURA");
            NonTerminal DECLARAASIG = new NonTerminal("DECLARAASIG");
            NonTerminal DECLARAASIGGLOBAL = new NonTerminal("DECLARAASIGGLOBAL");
            NonTerminal METODOSIMPLE = new NonTerminal("METODOSIMPLE");
            NonTerminal METODOARRAY = new NonTerminal("METODOARRAY");
            NonTerminal METODOOBJETO = new NonTerminal("METODOOBJETO");
            
            #endregion
            #region Gramatica

            INICIO.Rule = CUERPO;

            CUERPO.Rule = MakePlusRule(CUERPO, CUERPO2);

            CUERPO2.Rule = grfClase + identificador + grfImport + LISTAVAR + BLOQUEGLOBAL
                        | grfClase + identificador + BLOQUEGLOBAL
                        ;

            BLOQUEGLOBAL.Rule = curlyA + LISTASENTENCIAGLOBAL + curlyC;
            BLOQUEGLOBAL.ErrorRule = SyntaxError + curlyC;
            
            LISTASENTENCIAGLOBAL.Rule = MakePlusRule(LISTASENTENCIAGLOBAL, SENTENCIAGLOBAL);
            LISTASENTENCIAGLOBAL.ErrorRule = SyntaxError + puntoYComa
                                            |SyntaxError + curlyC;

            SENTENCIAGLOBAL.Rule = METODOS
                          | ASIGNACION + puntoYComa
                          | DECLARACIONGLOBAL + puntoYComa
                          | DECLARAASIGGLOBAL + puntoYComa
                          ;


            METODOS.Rule = METODOSIMPLE
                            | METODOARRAY
                            | METODOOBJETO
                            |PRINCIPAL
                            ;
            METODOARRAY.Rule = VISIBILIDAD + identificador + grfArray + TIPO + DIMENSIONES + parA + LISTAPARAM + parC + BLOQUE
                            | VISIBILIDAD + identificador + grfArray + TIPO + DIMENSIONES + parA + parC + BLOQUE
                            | VISIBILIDAD + identificador + grfArray + TIPO + DIMENSIONES + grfOverride + parA + parC + BLOQUE
                            | VISIBILIDAD + identificador + grfArray + TIPO + DIMENSIONES + grfOverride + parA + LISTAPARAM + parC + BLOQUE

                            | identificador + grfArray + TIPO + DIMENSIONES + parA + LISTAPARAM + parC + BLOQUE
                            | identificador + grfArray + TIPO + DIMENSIONES + parA + parC + BLOQUE
                            | identificador + grfArray + TIPO + DIMENSIONES + grfOverride + parA + parC + BLOQUE
                            | identificador + grfArray + TIPO + DIMENSIONES + grfOverride + parA + LISTAPARAM + parC + BLOQUE;
            METODOOBJETO.Rule = VISIBILIDAD + identificador + identificador + parA + LISTAPARAM + parC + BLOQUE
                            | VISIBILIDAD + identificador + identificador + parA + parC + BLOQUE
                            | VISIBILIDAD + identificador + identificador + grfOverride + parA + parC + BLOQUE
                            | VISIBILIDAD + identificador + identificador + grfOverride + parA + LISTAPARAM + parC + BLOQUE

                            | identificador + identificador + parA + LISTAPARAM + parC + BLOQUE
                            | identificador + identificador + parA + parC + BLOQUE
                            | identificador + identificador + grfOverride + parA + parC + BLOQUE
                            | identificador + identificador + grfOverride + parA + LISTAPARAM + parC + BLOQUE;

            METODOSIMPLE.Rule = VISIBILIDAD + identificador + TIPO + parA + LISTAPARAM + parC + BLOQUE
                            | VISIBILIDAD + identificador + TIPO + parA + parC + BLOQUE
                            | VISIBILIDAD + identificador + TIPO + grfOverride + parA + parC + BLOQUE
                            | VISIBILIDAD + identificador + TIPO + grfOverride + parA + LISTAPARAM + parC + BLOQUE

                            | identificador + TIPO + parA + LISTAPARAM + parC + BLOQUE
                            | identificador + TIPO + parA + parC + BLOQUE
                            | identificador + TIPO + grfOverride + parA + parC + BLOQUE
                            | identificador + TIPO + grfOverride + parA + LISTAPARAM + parC + BLOQUE;

            PRINCIPAL.Rule = grfMain + parA + parC + BLOQUE;

            BLOQUE.Rule = curlyA + LISTASENTENCIA + curlyC;
            BLOQUE.ErrorRule = SyntaxError + curlyC;

            LISTASENTENCIA.Rule = MakePlusRule(LISTASENTENCIA, SENTENCIA);
            LISTASENTENCIA.ErrorRule = SyntaxError + puntoYComa
                                     | SyntaxError + curlyC
                                     ;
            LISTAPARAM.Rule = MakeStarRule(LISTAPARAM, coma, PARAM);

            PARAM.Rule = TIPO + identificador
                         |identificador + identificador
                         |TIPO + grfArray + identificador+DIMENSIONES
                         ;

            SENTENCIA.Rule = DECLARACIONES + puntoYComa
                            | ASIGNACION + puntoYComa
                            | DECLARAASIG + puntoYComa
                            | SENTENCIAIF
                            | SENTENCIAELSEIF
                            | INCDEC + puntoYComa
                            | SENTENCIAWHILE
                            | SENTENCIADOWHILE + puntoYComa
                            | SENTENCIABREAK + puntoYComa
                            | SENTENCIAFOR
                            | SENTENCIAACCESO + puntoYComa
                            | SENTENCIAIMPRIMIR + puntoYComa
                            | SENTENCIASHOW + puntoYComa
                            | SENTENCIALLAMADA + puntoYComa
                            | SENTENCIACONTINUE + puntoYComa
                            | SENTENCIARETURN + puntoYComa
                            | SENTENCIAREPEAT
                            | SENTENCIASWITCH
                            | ADDFIGURE + puntoYComa
                            | FIGURE + puntoYComa
                            ;
            

            LISTAVAR.Rule = MakePlusRule(LISTAVAR, coma, identificador);

            DECLARACIONGLOBAL.Rule = VISIBILIDAD + TIPO + LISTAVAR //Ya 3
                                | TIPO + LISTAVAR//Ya 2
                                | TIPO + grfArray + LISTAVAR + DIMENSIONES//4
                                | VISIBILIDAD + TIPO + grfArray + LISTAVAR + DIMENSIONES//5
                                | VISIBILIDAD + identificador + LISTAVAR//Ya 3
                                | identificador + LISTAVAR//Ya 2
                                ;
            DECLARAASIGGLOBAL.Rule = VISIBILIDAD + TIPO + LISTAVAR + igual + EXP//4 ya 
                                    |TIPO + LISTAVAR + igual + EXP//3 ya
                                    |VISIBILIDAD + TIPO + grfArray + LISTAVAR + DIMENSIONES + igual + ASIGARREGLO//6
                                    |TIPO + grfArray + LISTAVAR + DIMENSIONES + igual + ASIGARREGLO//5
                                    | VISIBILIDAD + identificador + identificador + igual + grfNew + CONSTRUCTOR//5 ya
                                    | identificador + identificador + igual + grfNew + CONSTRUCTOR//4 ya
                                    ;
            DECLARACIONES.Rule = TIPO + LISTAVAR
                                | TIPO + grfArray + LISTAVAR + DIMENSIONES
                                | identificador + LISTAVAR
                                ;
                                ;
            DECLARAASIG.Rule = identificador + identificador + igual + grfNew + CONSTRUCTOR
                              | TIPO + LISTAVAR + igual + EXP
                              | TIPO + grfArray + LISTAVAR + DIMENSIONES + igual + ASIGARREGLO
                              ;
            INCDEC.Rule = INCREMENTO
                        | DECREMENTO
                        ;
            INCREMENTO.Rule =   identificador + masMas
                                |identificador + punto + identificador + masMas
                                |numero+masMas
                                ;

            DECREMENTO.Rule = identificador + menosMenos
                            | identificador + punto + identificador + menosMenos
                            | numero + menosMenos
                            ;

            CONSTRUCTOR.Rule = identificador + parA + parC;
            
            ASIGNACION.Rule = identificador + igual + EXP//2
                            | IDARREGLO + igual + EXP//3
                            | identificador + igual + grfNew + CONSTRUCTOR//3
                            | SENTENCIAACCESO + igual + EXP//2
                            ;
            ASIGARREGLO.Rule = ARREGLO
                                | LISTACURLY2
                                | curlyA + LISTADECURLYS + curlyC
                                ;
            
            IDARREGLO.Rule = identificador + DIMENSIONES;

            LISTACURLY.Rule = MakePlusRule(LISTACURLY, coma, ARREGLO);

            LISTACURLY2.Rule = curlyA + LISTACURLY + curlyC;

            LISTADECURLYS.Rule = MakePlusRule(LISTADECURLYS, coma, LISTACURLY2);

            ARREGLO.Rule = curlyA + LISTAEXP + curlyC;

            DIMENSION.Rule = MakePlusRule(DIMENSIONES, DIMENSION);

            DIMENSION.Rule = corchA + EXP + corchC;

            SENTENCIAIF.Rule = grfIf + parA + EXP + parC + BLOQUE
                              |grfIf + parA + EXP + parC + BLOQUE + grfElse + BLOQUE
                              ;
            SENTENCIAWHILE.Rule = grfWhile + parA + EXP + parC + BLOQUE;

            SENTENCIADOWHILE.Rule = grfDo + BLOQUE + grfMientras + parA + EXP + parC;

            SENTENCIAFOR.Rule = grfFor + parA + INSTRFOR + parC + BLOQUE;

            INSTRFOR.Rule = DECLARAASIG + puntoYComa + EXP + puntoYComa + INCDEC
                            |ASIGNACION + puntoYComa + EXP + puntoYComa + INCDEC;

            SENTENCIABREAK.Rule = grfBreak;

            SENTENCIAACCESO.Rule = identificador + punto + identificador;
                                    
            SENTENCIALLAMADA.Rule = identificador + parA + LISTAEXP + parC
                                    |identificador + parA + parC
                                    |identificador + punto + identificador + parA + parC
                                    |identificador + punto + identificador + parA + LISTAEXP + parC
                                    ;
            SENTENCIAIMPRIMIR.Rule = grfPrint + parA + EXP + parC;

            SENTENCIASHOW.Rule = grfShow + parA + EXP + coma + EXP + parC;

            SENTENCIACONTINUE.Rule = grfContinue;

            SENTENCIARETURN.Rule = grfReturn + EXP;

            SENTENCIAREPEAT.Rule = grfRepeat + parA + EXP + parC + BLOQUE;

            SENTENCIAELSEIF.Rule = grfIf + parA + EXP + parC + BLOQUE + LISTAELSEIF
                                  | grfIf + parA + EXP + parC + BLOQUE + LISTAELSEIF + grfElse + BLOQUE
                                  ;
            SENTENCIASWITCH.Rule = grfSwitch + parA + EXP + parC + curlyA + CASOS + curlyC
                                   ;
            CASOS.Rule = MakePlusRule(CASOS, CASO);

            CASO.Rule = grfCase + EXP + dosPuntos + LISTASENTENCIA
                        |DEFECTO;

            DEFECTO.Rule = grfDefault + dosPuntos + LISTASENTENCIA;

            LISTAELSEIF.Rule = MakePlusRule(LISTAELSEIF, IFELSE);

            IFELSE.Rule = grfElse + grfIf + parA + EXP + parC + BLOQUE;

            ADDFIGURE.Rule = grfAddF + parA + TIPOFIGURA + parA + LISTAEXP + parC + parC;

            FIGURE.Rule = grfFigure + parA + EXP + parC;

            LISTAEXP.Rule = MakeStarRule(LISTAEXP, coma, EXP);

            EXP.Rule = EXP_AR
                        | EXP_LO
                        | EXP_RE
                        | SENTENCIALLAMADA
                        | SENTENCIAACCESO
                        | parA + EXP + parC
                        | PRIMITIVO
                        ;
            EXP_AR.Rule = EXP + mas + EXP
                        | EXP + menos + EXP
                        | EXP + multi + EXP
                        | EXP + slash + EXP
                        | EXP + potencia + EXP
                        | menos + EXP
                        ;
            EXP_LO.Rule = EXP + orLogico + EXP
                        | EXP + andLogico + EXP
                        | notLogico + EXP
                        ;
            EXP_RE.Rule = EXP + mayorQue + EXP
                          | EXP + menorQue + EXP
                          | EXP + igualacion + EXP
                          | EXP + menorIgual + EXP
                          | EXP + mayorIgual + EXP
                          | EXP + diferente + EXP
                          ;
            TIPO.Rule = grfInt
                        | grfBool
                        | grfString
                        | grfChar
                        | grfVoid
                        |grfDouble
                        ;
            TIPOFIGURA.Rule = grfCircle
                             | grfSquare
                             | grfTriangle
                             | grfLine
                             ;
            VISIBILIDAD.Rule = grfPublic
                        | grfPrivate
                        ;
            PRIMITIVO.Rule = identificador
                            | cadena
                            |grfVerdadero
                            |grfFalse
                            |grfTrue
                            |grfFalso
                            |caracter
                            |numero
                            |IDARREGLO
                            ;
            #endregion
            #region Precedencia
            RegisterOperators(1, Associativity.Right, igual);
            RegisterOperators(2, Associativity.Left, orLogico);
            RegisterOperators(3, Associativity.Left, andLogico);
            RegisterOperators(4, Associativity.Right, notLogico);
            RegisterOperators(5, Associativity.Left, igualacion, diferente);
            RegisterOperators(6, Associativity.Left, menorQue, menorIgual);
            RegisterOperators(7, Associativity.Left, mayorQue, mayorIgual);
            RegisterOperators(8, Associativity.Left, mas, menos);
            RegisterOperators(9, Associativity.Left, slash, multi);
            RegisterOperators(10, Associativity.Right, potencia);
            
            RegisterOperators(11, Associativity.Left, punto);
            RegisterOperators(12, Associativity.Neutral, parA, parC);
            #endregion
            MarkPunctuation(parA, parC, curlyA, curlyC,corchA,corchC, puntoYComa, dosPuntos, coma, igual, punto);
            this.Root = INICIO;
        }
    }
}

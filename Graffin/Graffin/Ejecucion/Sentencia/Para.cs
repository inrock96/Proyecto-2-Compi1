using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion.Sentencia
{
    class Para
    {
        ParseTreeNode nodo;
        TablaSimbolos actual;
        TablaFunciones funciones;
        Expresion comparacion;
        IncDec incDec;
        public object respuesta;
        public Para(ParseTreeNode nodo, TablaSimbolos actual, TablaFunciones funciones)
        {
            this.actual = new TablaSimbolos(actual);
            this.funciones = funciones;
            this.nodo = nodo;
        }
        public void ejecutar()
        {
            hacerInstr();

            if (comparacion.respuesta is bool)
            {
                REGRESAR:
                comparacion.ejecutar(this.actual, funciones);
                if ((bool)comparacion.respuesta)
                {
                    Bloque b = new Bloque(nodo.ChildNodes[2], actual, funciones);
                    b.ejecutar(actual);
                    if (b.retorno == true)
                    {
                        respuesta = b.respuesta;
                        goto NOREGRESAR;
                    }
                    else if (b.romper)
                    {
                        goto NOREGRESAR;
                    }
                    else if (b.continuar)
                    {
                        goto REGRESAR;
                    }
                    incDec.ejecutar(actual, funciones);
                    goto REGRESAR;
                }
                NOREGRESAR:;
            }
            else
            {
                Program.getVentana().agregarError("Error, no es bool el for", "Semantico", -1, -1, "");
            }

        }
        void hacerInstr()
        {
            ParseTreeNode instr = nodo.ChildNodes[1];
            if (instr.ChildNodes[0].Term.ToString().Equals("DECLARAASIG"))
            {
                Declaracion d = new Declaracion(instr.ChildNodes[0], funciones);
                d.ejecutar(actual);

            }
            else
            {
                Asignacion a = new Asignacion(instr.ChildNodes[0], funciones);
                a.ejecutar(actual);
            }
            comparacion = new Expresion(instr.ChildNodes[1],funciones);
            comparacion.ejecutar(actual, funciones);
            incDec = new IncDec(instr.ChildNodes[2]);
            
        }
    }
}

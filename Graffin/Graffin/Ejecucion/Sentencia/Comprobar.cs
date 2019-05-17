using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion.Sentencia
{
    class Comprobar
    {
  
        ParseTreeNode nodo;
        TablaFunciones funciones;
        public object respuesta;
        public Comprobar(ParseTreeNode nodo, TablaFunciones funciones)
        {
            this.nodo = nodo;
            this.funciones = funciones;
        }
        public void ejecutar(TablaSimbolos local)
        {
            ejecutar(local, nodo);
        }
        private void ejecutar(TablaSimbolos actual, ParseTreeNode raiz)
        {
            Expresion comparar = new Expresion(raiz.ChildNodes[1], funciones);
            comparar.ejecutar(actual, funciones);
            foreach(ParseTreeNode caso in raiz.ChildNodes[2].ChildNodes)
            {
                if (caso.ChildNodes.Count > 1)
                {
                    Expresion e = new Expresion(caso.ChildNodes[1],funciones);
                    e.ejecutar(actual, funciones);
                    if (e.respuesta.Equals(comparar.respuesta))
                    {
                        TablaSimbolos local = new TablaSimbolos(actual);
                        Bloque b = new Bloque(caso.ChildNodes[2], local, funciones);
                        b.ejecutar(local);
                        if (b.retorno)
                        {
                            respuesta = b.respuesta;
                            break;
                        }else if (b.romper)
                        {

                            break;
                        }
                    }
                }
                else
                {
                    //defecto
                    TablaSimbolos local = new TablaSimbolos(actual);
                    Bloque b = new Bloque(caso.ChildNodes[0].ChildNodes[1],local,funciones);
                    b.ejecutar(local);
                    if (b.retorno)
                    {
                        respuesta = b.respuesta;
                        break;
                    }else if (b.romper)
                    {
                        break;
                    }
                }
            }
        }
    }
}

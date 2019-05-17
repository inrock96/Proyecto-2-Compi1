using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion
{
    class Main:Funcion
    {
        TablaSimbolos actual;
        TablaFunciones funciones;
        public Main(string identificador, object valor, string tipo, int linea, int columna, int dimension, ParseTreeNode nodo,ParseTreeNode parametros) : base(identificador, valor, tipo, linea, columna, dimension, nodo,parametros)
        {
            this.identificador = identificador;
            this.valor = valor;
            this.tipo = tipo;
            this.linea = linea;
            this.nodo = nodo;
        }
        public void setEntorno(TablaSimbolos entorno, TablaFunciones funciones)
        {
            actual = new TablaSimbolos(entorno);
            this.funciones = funciones;

        }
        public void ejecutar()
        {
            Bloque b = new Bloque(nodo, actual, funciones);
            b.ejecutar(actual);
        }
    }
}

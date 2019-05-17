using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graffin.Ejecucion
{
    class Objeto : Simbolo
    {
        public TablaFunciones funciones;
        public TablaSimbolos local;
        
        public Objeto(string identificador, string tipo, int linea, int columna, int dimension,TablaSimbolos local, TablaFunciones funciones):base(identificador,tipo,linea,columna,dimension)
        {
            this.identificador = identificador;
            this.tipo = tipo;
            this.linea = linea;
            this.columna = columna;
            this.dimension = dimension;
            visible = true;
            this.funciones = funciones;
            this.local = local;
        }
        public Objeto(string identificador, string tipo, int linea, int columna, int dimension) : base(identificador, tipo, linea, columna, dimension)
        {
            this.identificador = identificador;
            this.tipo = tipo;
            this.linea = linea;
            this.columna = columna;
            this.dimension = dimension;
            visible = true;
        }
        public void asignar(TablaSimbolos local,TablaFunciones funciones)
        {
            this.local = local;
            this.funciones = funciones;
        }
    }
}

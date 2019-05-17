using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graffin.Ejecucion
{
    class Simbolo
    {
        public string identificador;
        public object valor;
        public string tipo;
        public int linea;
        public int columna;
        public int dimension;
        public bool visible;

        public Simbolo(string identificador, object valor, string tipo, int linea, int columna, int dimension)
        {
            this.identificador = identificador;
            this.valor = valor;
            this.tipo = tipo;
            this.linea = linea;
            this.columna = columna;
            this.dimension = dimension;
            visible = true;
        }
        public Simbolo(string identificador,string tipo,int linea, int columna, int dimension)
        {
            this.identificador = identificador;
            this.tipo = tipo;
            this.linea = linea+1;
            this.columna = columna;
            this.dimension = dimension;
            visible = true;
        }
        public Simbolo(string tipo)
        {
            this.tipo = tipo;
        }

        
        public bool esObjeto()
        {
            if (!tipo.Equals("int") && !tipo.Equals("double") && !tipo.Equals("string") && !tipo.Equals("char") && !tipo.Equals("bool"))
            {
                return true;
            }
            return false;
        }
    }
}

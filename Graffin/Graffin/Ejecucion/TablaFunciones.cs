using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Graffin.Ejecucion
{
    class TablaFunciones
    {
        public Hashtable tabla;
        public TablaFunciones()
        {
            tabla = new Hashtable();
        }
        public void agregar(Funcion funcion)
        {   
            tabla.Add(funcion.key, funcion);
        }
        public bool existe(string id)
        {
            return tabla.Contains(id);
        }
        public Funcion sacar(string id)
        {
            Funcion nueva = (Funcion)tabla[id];
            if (nueva!=null)
            {
                return nueva;
            }
            return null;

        }


    }
}

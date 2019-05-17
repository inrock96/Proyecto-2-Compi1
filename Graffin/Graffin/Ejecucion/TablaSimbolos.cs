using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graffin.Ejecucion
{
    class TablaSimbolos
    {
        TablaSimbolos anterior;
        public Hashtable listaSimbolos;
        public TablaSimbolos(TablaSimbolos anterior)
        {
            this.anterior = anterior;
            listaSimbolos = new Hashtable();
        }
        public void agregar(string id, Simbolo simbolo)
        {
            listaSimbolos.Add(id, simbolo);
        }
        public bool existe(string id)
        {
            for(TablaSimbolos ts = this; ts != null; ts = ts.anterior)
            {
                if (ts.listaSimbolos.Contains(id))
                    return true;
            }
            return false;
        }
        public Simbolo sacar (string id)
        {
            TablaSimbolos ts;
            Simbolo sacado;
            for(ts = this;ts!=null;ts = ts.anterior)
            {
                sacado = (Simbolo)ts.listaSimbolos[id];
                if (ts.listaSimbolos.Contains(id))
                    return sacado;
            }
            return null;
        }
        public bool existeActual(string id)
        {
            Simbolo encontrado = (Simbolo)listaSimbolos[id];
            return encontrado == null;
        }
        public bool reemplazar(string id, Simbolo simbolo)
        {
            Simbolo encontrado;
            for(TablaSimbolos ts = this; ts != null; ts = ts.anterior)
            {
                encontrado = (Simbolo)ts.listaSimbolos[id];
                if (encontrado != null)
                {
                    ts.listaSimbolos[id] = simbolo;
                    return true;
                }
            }
            Console.WriteLine("El simbolo \"" + id + "\" no ha sido declarado en el entorno actual ni en alguno externo");
            return false;
            
        }
    }
}

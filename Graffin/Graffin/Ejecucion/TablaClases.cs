using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Windows.Forms;

namespace Graffin.Ejecucion
{
    class TablaClases
    {
        TablaSimbolos global;
        TablaFunciones funciones;
        public static Hashtable tabla;
        List<string> nombres;
        public TablaClases()
        {
            global = new TablaSimbolos(null);
            funciones = new TablaFunciones();
            tabla = new Hashtable();
            nombres = new List<string>();
        }
        public void agregar(Clase nueva)
        {
            tabla.Add(nueva.nombre, nueva);
            nombres.Add(nueva.nombre);
        }
        public void iniciar()
        {
            int i;
            Clase nueva;
            for (i = 0; i < nombres.Count; i++)
            {
                if (tabla.Contains(nombres[i]))
                {
                    nueva = sacar(nombres[i]);
                    
                        nueva.primerRecorrido();
                        this.reemplazar(nueva.nombre, nueva);
                    
                    
                }
            }
            
            startMain();
        }
        public void startMain()
        {
            
            
            int i;
            Clase encontrada;
            for (i = 0; i < nombres.Count; i++)
            {
                encontrada = sacar(nombres[i]);
                if (encontrada != null)
                {
                    Funcion main =  encontrada.funciones.sacar("main");
                    if (main != null)
                    {
                        main.ejecutar(encontrada.global, encontrada.funciones);
                        goto LABEL;
                    }
                }
            }
            MessageBox.Show("No se encontró el main");
            LABEL:;
            
        }
        public bool existe(string nombre)
        {
            return (tabla.Contains(nombre));
        }
        public Clase sacar(string nombre)
        {
            return (Clase)tabla[nombre];
        }
        public bool reemplazar(string id, Clase clase)
        {
            Clase encontrado;
            
                encontrado = (Clase)tabla[id];
                if (encontrado != null)
                {
                    tabla[id] = clase;
                    return true;
                }
            
            Console.WriteLine("El simbolo \"" + id + "\" no ha sido declarado en el entorno actual ni en alguno externo");
            return false;

        }
        public Hashtable getTabla()
        {
            return tabla;
        }
    }
}

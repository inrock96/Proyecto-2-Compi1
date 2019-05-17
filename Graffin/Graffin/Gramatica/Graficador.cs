using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Graffin.Gramatica
{
    class Graficador
    {
        private int indice;
        private readonly string rutaExeDot = "C:\\Program Files (x86)\\Graphviz2.38\\bin\\dot.exe";
        public void graficar(ParseTreeNode raiz)
        {
            StreamWriter archivo = new StreamWriter("ArbolSintactico.dot");
            string texto = "graph G{";
            texto += "node[shape=egg]";
            indice = 0;
            definirNodos(raiz, ref texto);
            indice = 0;
            enlazarNodos(raiz, 0, ref texto);
            texto += "}";
            archivo.Write(texto);
            archivo.Close();
            ProcessStartInfo startInfo = new ProcessStartInfo(rutaExeDot);
            startInfo.Arguments = "-Tpng ArbolSintactico.dot -o ArbolSintactico.png";
            Process.Start(startInfo);
            Thread.Sleep(1000);
            startInfo.FileName = "ArbolSintactico.png";
            Process.Start(startInfo);
        }

        public void definirNodos(ParseTreeNode nodo, ref string texto)
        {
            texto += "node" + indice.ToString() + "[label = \"" + nodo.ToString() + "\", style = filled];";
            indice++;
            foreach (ParseTreeNode hijo in nodo.ChildNodes)
            {
                definirNodos(hijo, ref texto);
            }
        }

        public void enlazarNodos(ParseTreeNode nodo, int actual, ref string texto)
        {
            if (nodo != null)
            {
                foreach (ParseTreeNode hijo in nodo.ChildNodes)
                {
                    indice++;
                    texto += "\"node" + actual.ToString() + "\"--" + "\"node" + indice.ToString() + "\"";
                    enlazarNodos(hijo, indice, ref texto);
                }
            }
        }
    }
}

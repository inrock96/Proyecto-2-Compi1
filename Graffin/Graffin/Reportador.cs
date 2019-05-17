using Graffin.Gramatica;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graffin
{
    class Reportador
    {
        TextWriter archivo;
        List<ErrorSemantico> errores;

        string titulo;
        public Reportador(List<ErrorSemantico> errores,string titulo)
        {
            this.errores = errores;
            this.titulo = titulo;
            archivo = new StreamWriter(titulo);
        }
        public void ejecutar()
        {
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(titulo))
                {
                    File.Delete(titulo);
                }

                // Create a new file     
                File.Create(titulo);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
            archivo.WriteLine("<html lang \"es\">");
            archivo.WriteLine("<head><meta charset=\"utf-8\"><title>"+titulo+"</title></head>");
            archivo.WriteLine("<center><h1>"+titulo+"</h1></center>");
            archivo.WriteLine("<center><table border=\"1\">");
            archivo.WriteLine("<tr>");
            archivo.WriteLine("<th>No.</th><th>Lexema</th><th>Fila</th><th>Columna</th><th>Error</th><th>Tipo</th>");
            archivo.WriteLine("</tr>");
            int i = 0;
            foreach(ErrorSemantico error in errores)
            {
                i++;
                archivo.WriteLine("<tr>");
                archivo.WriteLine("<td>" + i + "</td><td>" + error.token+"</td><td>"+error.fila+ "</td><td>"+error.columna+ "</td><td>"+error.mensaje+ "</td><td>"+error.tipo+ "</td>");
                archivo.WriteLine("</tr>");
            }
            archivo.WriteLine("</table></center>");
            archivo.WriteLine("</html>");
            archivo.Close();
            Process.Start(titulo);

        }

    }
}

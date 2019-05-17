using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Graffin.Ejecucion.Sentencia;

namespace Graffin.Ejecucion
{
    class Ejecutor
    {
       public static TablaClases tc;
        List<ParseTreeNode> raices;
        public static List<Figura> figuras;
        public Ejecutor(List<ParseTreeNode> raices)
        {
            tc = new TablaClases();
            this.raices = raices;
            figuras = new List<Figura>();
        }
        
        public void primerRecorrido(List<ParseTreeNode> raices)
        {
            foreach (ParseTreeNode raiz in raices)
                primerRecorrido(raiz);
        }
        private void primerRecorrido(ParseTreeNode raiz)
        {

            switch (raiz.Term.Name.ToString())
            {
                case "INICIO":
                    primerRecorrido(raiz.ChildNodes[0]);
                    break;
                case "CUERPO":
                    foreach (ParseTreeNode hijo in raiz.ChildNodes)
                    {
                        primerRecorrido(hijo);
                    }
                    break;
                case "CUERPO2":
                    if (raiz.ChildNodes.Count == 3)
                    //LA CLASE NO EXTIENDE DE NADA
                    {
                            
                        Clase nueva = new Clase(raiz.ChildNodes[1].Token.Text.ToLower(), raiz.ChildNodes[2].ChildNodes[0]);
                        if(!tc.existe(raiz.ChildNodes[1].Token.Text.ToLower()))
                        {
                            tc.agregar(nueva);
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, esa clase ya existe", "Semantico", -1, -1, "");
                        }
                    }
                    else if(raiz.ChildNodes.Count ==4)
                    //SI LA CLASE TIENE EXTENSIONES SE LE PASAN LOS NOMBRES NADA MAS
                    {
                        LinkedList<string> imports = new LinkedList<string>();
                        foreach(ParseTreeNode hijo in raiz.ChildNodes[3].ChildNodes)
                        {
                            imports.AddLast( hijo.Token.ToString().ToLower());
                        }
                        if (!tc.existe(raiz.ChildNodes[1].Token.Text))
                        {
                            Clase nueva = new Clase(raiz.ChildNodes[1].Token.Text.ToLower(),raiz.ChildNodes[3], imports);
                            tc.agregar(nueva);
                        }
                        else
                        {
                            //error semantico
                        }
                    }
                    break;
                default:
                    Program.getVentana().agregarError("Error, error desconocido", "Semantico", -1, -1, "");
                    break;
            }
        }

        public void ejecutar()
        {
            primerRecorrido(this.raices);
            //Con la tabla de clases se verifica que solo haya un main, y que las exportaciones existan
            tc.iniciar();
         
        }

        public int asignarTipo(ParseTreeNode hijo)
        {
            if (hijo.Token.Text.Equals("int"))
            {
                return 0;
            }
            else if (hijo.Token.Text.Equals("double"))
            {
                return 1;
            }
            else if (hijo.Token.Text.Equals("string"))
            {
                return 2;
            }
            else if (hijo.Token.Text.Equals("char"))
            {
                return 3;
            }
            else if (hijo.Token.Text.Equals("bool"))
            {
                return 4;

            }
            else if (hijo.Token.Text.Equals("void"))
            {
                //Error, una variable no puede ser void
                return 5;
            }
            else
            {
                //Tipo clase :v11111111111
            
                return 6;
            }
        }

    }
}

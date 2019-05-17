using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace Graffin.Ejecucion
{
    class Clase
    {
        public TablaSimbolos global;
        public TablaFunciones funciones;
        public string nombre;
        public ParseTreeNode raiz;
        public bool principal;
        public LinkedList<String> importaciones;
        public Clase (string nombre,ParseTreeNode raiz)
        {
            this.nombre = nombre;
            this.raiz = raiz;
            importaciones = new LinkedList<string>();
            global = new TablaSimbolos(null);
            funciones = new TablaFunciones();
            principal = false;
        }
        public Clase(string nombre, ParseTreeNode raiz, LinkedList<String> importaciones)
        {
            this.nombre = nombre;
            this.raiz = raiz;
            this.importaciones = importaciones;
            funciones = new TablaFunciones();
            global = new TablaSimbolos(null);
        }
        public void primerRecorrido()
        {
            primerRecorrido(raiz);
        }
        public void ejecutar()
        {
            
        }
        private void primerRecorrido(ParseTreeNode raiz)
        {
            if (importaciones.Count >0)
            {
                Clase nueva;
                foreach (string nombre in importaciones)
                {
                    nueva = new Clase(nombre,(Ejecutor.tc.sacar(nombre).raiz));
                    if (nueva.global != null)
                    {
                        nueva.primerRecorrido();
                        Simbolo s;
                        Funcion f;
                        foreach (object key in nueva.global.listaSimbolos.Keys)
                        {
                            s = nueva.global.sacar(key.ToString());
                            
                            if (s != null)
                            {
                                if (s.visible)
                                {
                                    this.global.agregar(s.identificador, s);
                                }
                            }
                        }
                        foreach(object key in nueva.funciones.tabla.Keys)
                        {
                            f = nueva.funciones.sacar(key.ToString());
                            if (f != null)
                            {
                                if (f.visible)
                                {
                                    funciones.agregar(f);
                                }
                            }
                        }
                        
                    }
                }
            }
            switch (raiz.Term.Name.ToString())
            {
                //Inicio el programa
                case "BLOQUEGLOBAL"://Listo
                    primerRecorrido(raiz.ChildNodes[0]);
                    break;
                case "LISTAGLOBAL"://Listo
                    foreach(ParseTreeNode hijo in raiz.ChildNodes)
                    {
                        primerRecorrido(hijo);
                    }
                    break;
                case "METODOS"://Listo
                    primerRecorrido(raiz.ChildNodes[0]);
                    break;
                case "METODOSIMPLE"://Probar
                    if (raiz.ChildNodes.Count == 3)
                    {
                        //simple
                        Funcion f = new Funcion(raiz.ChildNodes[0].Token.Text.ToLower(), null, asignarTipo(raiz.ChildNodes[1].ChildNodes[0]), raiz.ChildNodes[1].ChildNodes[0].Token.Location.Line, raiz.ChildNodes[1].ChildNodes[0].Token.Location.Column, 0, raiz.ChildNodes[2],null);
                        f.visible = true;
                        f.overRide = false;
                        funciones.agregar(f);
                    }else if (raiz.ChildNodes.Count == 4)
                    {
                        if (raiz.ChildNodes[0].Term.ToString().Equals("VISIBILIDAD"))
                        {
                            

                            ParseTreeNode metodo = raiz;
                            Funcion f = new Funcion(metodo.ChildNodes[1].Token.Text.ToLower(), null, asignarTipo(metodo.ChildNodes[2]), metodo.ChildNodes[1].Token.Location.Line, metodo.ChildNodes[1].Token.Location.Column, 0, metodo.ChildNodes[3], null);
                            f.visible = asignarVisibilidad(metodo.ChildNodes[0]);
                            f.overRide = false;
                            //f.setParametros(raiz.ChildNodes[2]);
                            //global.agregar(f.identificador, f);
                            funciones.agregar(f);


                        }
                        else
                        {
                            if (raiz.ChildNodes[2].Term.ToString().Equals("override"))
                            {
                                //Sin params
                                

                                //Con params
                                ParseTreeNode metodo = raiz;
                                Funcion f = new Funcion(metodo.ChildNodes[0].Token.Text.ToLower(), null, asignarTipo(metodo.ChildNodes[1]), metodo.ChildNodes[0].Token.Location.Line, metodo.ChildNodes[0].Token.Location.Column, 0, metodo.ChildNodes[3], null);
                                f.visible = true;
                                f.overRide = true;
                                //f.setParametros(raiz.ChildNodes[2]);
                                //global.agregar(f.identificador, f);
                                funciones.agregar(f);
                            }
                            else
                            {
                                //Con params
                                ParseTreeNode metodo = raiz;
                                Funcion f = new Funcion(metodo.ChildNodes[0].Token.Text.ToLower(), null, asignarTipo(metodo.ChildNodes[1]), metodo.ChildNodes[0].Token.Location.Line, metodo.ChildNodes[0].Token.Location.Column, 0, metodo.ChildNodes[3], metodo.ChildNodes[2]);
                                f.visible = true;
                                f.overRide = false;
                                //f.setParametros(raiz.ChildNodes[2]);
                                //global.agregar(f.identificador, f);
                                funciones.agregar(f);
                            }
                        }
                    }
                    else if (raiz.ChildNodes.Count == 5)
                    {
                        if (raiz.ChildNodes[0].Term.ToString().Equals("VISIBILIDAD"))
                        {
                            if (raiz.ChildNodes[3].Term.ToString().Equals("override"))
                            {
                                

                                ParseTreeNode metodo = raiz;
                                string id = metodo.ChildNodes[1].Token.Text.ToLower();
                                int fil = metodo.ChildNodes[1].Token.Location.Line;
                                int col = metodo.ChildNodes[1].Token.Location.Column;
                                Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[2]), fil, col, 0, metodo.ChildNodes[4], metodo.ChildNodes[3]);
                                f.visible = asignarVisibilidad(raiz.ChildNodes[0]);
                                f.overRide = true;

                                //global.agregar(f.identificador, f);
                                if (!funciones.existe(id))
                                {
                                    funciones.agregar(f);

                                }
                            }
                            else
                            {
                                ParseTreeNode metodo = raiz;
                                string id = metodo.ChildNodes[1].Token.Text.ToLower();
                                int fil = metodo.ChildNodes[1].Token.Location.Line;
                                int col = metodo.ChildNodes[1].Token.Location.Column;
                                Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[2]), fil, col, 0, metodo.ChildNodes[4], metodo.ChildNodes[3]);
                                f.visible = asignarVisibilidad(raiz.ChildNodes[0]);
                                f.overRide = false;

                                //global.agregar(f.identificador, f);
                                if (!funciones.existe(id))
                                {
                                    funciones.agregar(f);

                                }
                            }
                        }
                        else
                        {   //Override y params
                            

                            ParseTreeNode metodo = raiz;
                            string id = metodo.ChildNodes[0].Token.Text.ToLower();
                            int fil = metodo.ChildNodes[0].Token.Location.Line;
                            int col = metodo.ChildNodes[0].Token.Location.Column;
                            Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[1]), fil, col, 0, metodo.ChildNodes[4], metodo.ChildNodes[3]);
                            f.visible = true;
                            f.overRide = true;
                            //f.setParametros(raiz.ChildNodes[4]);
                            //global.agregar(f.identificador, f);
                            funciones.agregar(f);
                        }
                    }
                    else if (raiz.ChildNodes.Count == 6)
                    {
                        ParseTreeNode metodo = raiz;
                        string id = metodo.ChildNodes[1].Token.Text.ToLower();
                        int fil = metodo.ChildNodes[1].Token.Location.Line;
                        int col = metodo.ChildNodes[1].Token.Location.Column;
                        Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[2]), fil, col, 0, metodo.ChildNodes[5], metodo.ChildNodes[4]);
                        f.visible = asignarVisibilidad(raiz.ChildNodes[0]);
                        f.overRide = true;
                        f.setParametros(raiz.ChildNodes[4]);
                        //global.agregar(f.identificador, f);
                        funciones.agregar(f);
                    }
                    break;
                case "METODOARRAY"://Terminar
                    //if (raiz.ChildNodes.Count == 5)
                    //{
                    //    //simple
                    //    Funcion f = new Funcion(raiz.ChildNodes[0].Token.Text.ToLower(), null, asignarTipo(raiz.ChildNodes[1].ChildNodes[0]), raiz.Token.Location.Line, raiz.Token.Location.Column, 0, raiz.ChildNodes[4],null);
                    //    f.visible = true;
                    //    f.overRide = false;
                    //    global.agregar(f.identificador, f);
                    //    funciones.agregar(f);
                    //}
                    //else if (raiz.ChildNodes.Count == 6)
                    //{
                    //    if (raiz.ChildNodes[0].Term.ToString().Equals("VISIBILIDAD"))
                    //    {
                    //        Funcion f = new Funcion(raiz.ChildNodes[1].Token.Text.ToLower(), null, asignarTipo(raiz.ChildNodes[2].ChildNodes[0]), raiz.Token.Location.Line, raiz.Token.Location.Column, 0, raiz.ChildNodes[3]);
                    //        f.visible = asignarVisibilidad(raiz.ChildNodes[0]);
                    //        f.overRide = false;
                    //        global.agregar(f.identificador, f);
                    //        funciones.agregar(f);
                    //    }
                    //    else
                    //    {
                    //        if (raiz.ChildNodes[4].Term.ToString().Equals("grfOverride"))
                    //        {
                    //            //Sin params
                    //            Funcion f = new Funcion(raiz.ChildNodes[0].Token.Text.ToLower(), null, asignarTipo(raiz.ChildNodes[1].ChildNodes[0]), raiz.Token.Location.Line, raiz.Token.Location.Column, 0, raiz.ChildNodes[3]);
                    //            f.visible = true;
                    //            f.overRide = true;
                    //            global.agregar(f.identificador, f);
                    //            funciones.agregar(f);
                    //        }
                    //        else
                    //        {
                    //            //Con params
                    //            Funcion f = new Funcion(raiz.ChildNodes[0].Token.Text.ToLower(), null, asignarTipo(raiz.ChildNodes[1].ChildNodes[0]), raiz.Token.Location.Line, raiz.Token.Location.Column, 0, raiz.ChildNodes[3]);

                    //            f.visible = true;
                    //            f.overRide = false;
                    //            f.setParametros(raiz.ChildNodes[2]);
                    //            //global.agregar(f.identificador, f);
                    //            funciones.agregar(f);
                    //        }
                    //    }
                    //}
                    //else if (raiz.ChildNodes.Count == 7)
                    //{
                    //    if (raiz.ChildNodes[0].Term.ToString().Equals("VISIBILIDAD"))
                    //    {
                    //        if (raiz.ChildNodes[5].Term.ToString().Equals("grfOverride"))
                    //        {
                    //            Funcion f = new Funcion(raiz.ChildNodes[1].Token.Text.ToLower(), null, asignarTipo(raiz.ChildNodes[2].ChildNodes[0]), raiz.Token.Location.Line, raiz.Token.Location.Column, 0, raiz.ChildNodes[3]);
                    //            f.visible = asignarVisibilidad(raiz.ChildNodes[0]);
                    //            f.overRide = true;
                    //            global.agregar(f.identificador, f);
                    //            funciones.agregar(f);
                    //        }
                    //        else
                    //        {
                    //            Funcion f = new Funcion(raiz.ChildNodes[1].Token.Text.ToLower(), null, asignarTipo(raiz.ChildNodes[2].ChildNodes[0]), raiz.Token.Location.Line, raiz.Token.Location.Column, 0, raiz.ChildNodes[4]);
                    //            f.visible = asignarVisibilidad(raiz.ChildNodes[0]);
                    //            f.overRide = false;
                    //            f.setParametros(raiz.ChildNodes[3]);
                    //            //global.agregar(f.identificador, f);
                    //            funciones.agregar(f);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        Funcion f = new Funcion(raiz.ChildNodes[0].Token.Text.ToLower(), null, asignarTipo(raiz.ChildNodes[1].ChildNodes[0]), raiz.Token.Location.Line, raiz.Token.Location.Column, 0, raiz.ChildNodes[3]);

                    //        f.visible = true;
                    //        f.overRide = true;
                    //        f.setParametros(raiz.ChildNodes[3]);
                    //        //global.agregar(f.identificador, f);
                    //        funciones.agregar(f);
                    //    }
                    //}
                    //else if (raiz.ChildNodes.Count == 8)
                    //{
                    //    Funcion f = new Funcion(raiz.ChildNodes[1].Token.Text.ToLower(), null, asignarTipo(raiz.ChildNodes[2].ChildNodes[0]), raiz.Token.Location.Line, raiz.Token.Location.Column, 0, raiz.ChildNodes[5]);
                    //    f.visible = asignarVisibilidad(raiz.ChildNodes[0]);
                    //    f.overRide = true;
                    //    f.setParametros(raiz.ChildNodes[4]);
                    //    //global.agregar(f.identificador, f);
                    //    funciones.agregar(f);
                    //}
                    break;
                case "METODOOBJETO"://Probar
                    if (raiz.ChildNodes.Count == 3)
                    {
                        //simple
                        

                        ParseTreeNode metodo = raiz;
                        string id = metodo.ChildNodes[0].Token.Text.ToLower();
                        int fil = metodo.ChildNodes[0].Token.Location.Line;
                        int col = metodo.ChildNodes[0].Token.Location.Column;
                        Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[1]), fil, col, 0, metodo.ChildNodes[2],null);
                        f.visible = true;
                        f.overRide = false;
                        //f.setParametros(raiz.ChildNodes[3]);
                        //global.agregar(f.identificador, f);
                        funciones.agregar(f);
                    }
                    else if (raiz.ChildNodes.Count == 4)
                    {
                        if (raiz.ChildNodes[0].Term.ToString().Equals("VISIBILIDAD"))
                        {
                            

                            ParseTreeNode metodo = raiz;
                            string id = metodo.ChildNodes[1].Token.Text.ToLower();
                            int fil = metodo.ChildNodes[1].Token.Location.Line;
                            int col = metodo.ChildNodes[1].Token.Location.Column;
                            Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[2]), fil, col, 0, metodo.ChildNodes[3], null);
                            f.visible = true;
                            f.overRide = false;
                            //f.setParametros(raiz.ChildNodes[2]);
                            //global.agregar(f.identificador, f);
                            funciones.agregar(f);
                        }
                        else
                        {
                            if (raiz.ChildNodes[2].Term.ToString().Equals("override"))
                            {
                                //Sin params
                                

                                ParseTreeNode metodo = raiz;
                                string id = metodo.ChildNodes[0].Token.Text.ToLower();
                                int fil = metodo.ChildNodes[0].Token.Location.Line;
                                int col = metodo.ChildNodes[0].Token.Location.Column;
                                Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[1]), fil, col, 0, metodo.ChildNodes[3], null);
                                f.visible = true;
                                f.overRide = true;
                                //f.setParametros(raiz.ChildNodes[2]);
                                //global.agregar(f.identificador, f);
                                funciones.agregar(f);
                            }
                            else
                            {
                                //Con params
                                ParseTreeNode metodo = raiz;
                                string id = metodo.ChildNodes[0].Token.Text.ToLower();
                                int fil = metodo.ChildNodes[0].Token.Location.Line;
                                int col = metodo.ChildNodes[0].Token.Location.Column;
                                Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[1]), fil, col, 0, metodo.ChildNodes[3], metodo.ChildNodes[2]);
                                f.visible = true;
                                f.overRide = false;
                                //f.setParametros(raiz.ChildNodes[2]);
                                //global.agregar(f.identificador, f);
                                funciones.agregar(f);
                            }
                        }
                    }
                    else if (raiz.ChildNodes.Count == 5)
                    {
                        if (raiz.ChildNodes[0].Term.ToString().Equals("VISIBILIDAD"))
                        {
                            if (raiz.ChildNodes[3].Term.ToString().Equals("override"))
                            {
                                ParseTreeNode metodo = raiz;
                                string id = metodo.ChildNodes[1].Token.Text.ToLower();
                                int fil = metodo.ChildNodes[1].Token.Location.Line;
                                int col = metodo.ChildNodes[1].Token.Location.Column;
                                Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[2]), fil, col, 0, metodo.ChildNodes[4], null);
                                f.visible = asignarVisibilidad(raiz.ChildNodes[0]);
                                f.overRide = true;
                                f.setParametros(raiz.ChildNodes[3]);
                                //global.agregar(f.identificador, f);
                                funciones.agregar(f);
                            }
                            else
                            {
                                ParseTreeNode metodo = raiz;
                                string id = metodo.ChildNodes[1].Token.Text.ToLower();
                                int fil = metodo.ChildNodes[1].Token.Location.Line;
                                int col = metodo.ChildNodes[1].Token.Location.Column;
                                Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[2]), fil, col, 0, metodo.ChildNodes[4], metodo.ChildNodes[3]);
                                f.visible = asignarVisibilidad(raiz.ChildNodes[0]);
                                f.overRide = false;
                                f.setParametros(raiz.ChildNodes[3]);
                                //global.agregar(f.identificador, f);
                                funciones.agregar(f);
                            }
                        }
                        else
                        {
                            ParseTreeNode metodo = raiz;
                            string id = metodo.ChildNodes[0].Token.Text.ToLower();
                            int fil = metodo.ChildNodes[0].Token.Location.Line;
                            int col = metodo.ChildNodes[0].Token.Location.Column;
                            Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[1]), fil, col, 0, metodo.ChildNodes[4], metodo.ChildNodes[3]);
                            f.overRide = true;
                            f.setParametros(raiz.ChildNodes[3]);
                            //global.agregar(f.identificador, f);
                            funciones.agregar(f);
                        }
                    }
                    else if (raiz.ChildNodes.Count == 6)
                    {
                        

                        ParseTreeNode metodo = raiz;
                        string id = metodo.ChildNodes[1].Token.Text.ToLower();
                        int fil = metodo.ChildNodes[1].Token.Location.Line;
                        int col = metodo.ChildNodes[1].Token.Location.Column;
                        Funcion f = new Funcion(id, null, asignarTipo(metodo.ChildNodes[2]), fil, col, 0, metodo.ChildNodes[5], metodo.ChildNodes[4]);
                        f.visible = asignarVisibilidad(metodo.ChildNodes[0]);
                        f.overRide = true;
                        f.setParametros(raiz.ChildNodes[3]);
                        //global.agregar(f.identificador, f);
                        funciones.agregar(f);
                    }
                    break;
                case "DECGLOBAL"://Probar
                    Declaracion d = new Declaracion(raiz,funciones);
                    d.ejecutar(global);
                    break;
                case "ASIGNACION"://Probar
                    Asignacion a = new Asignacion(raiz,funciones);
                    a.ejecutar(global);
                    break;
                case "PRINCIPAL"://Listo
                    Funcion main = new Funcion("main", null, "void", raiz.ChildNodes[0].Token.Location.Line, raiz.ChildNodes[0].Token.Location.Column, 0, raiz.ChildNodes[1],null);
                    main.visible = false;
                    funciones.agregar(main);
                    break;
                case "SENTENCIAGLOBAL"://Listo
                    primerRecorrido(raiz.ChildNodes[0]);
                    break;
                case "DECLARAASIGGLOBAL":
                    Declaracion dag = new Declaracion(raiz,funciones);
                    dag.ejecutar(global);
                    break;
                default:
                    Program.getVentana().agregarError("Error critico en el arbol", "Semantico", raiz.Token.Location.Line, raiz.Token.Location.Column, raiz.Token.Text);
                    break;
            }
            

        }
        string asignarTipo(ParseTreeNode nodo)
        {
            if (nodo.Term.ToString().Equals("TIPO"))
            {
                nodo = nodo.ChildNodes[0];
                switch (nodo.Token.Text.ToLower())
                {
                    case "int":
                        return "int";
                    case "double":
                        return "double";
                    case "string":
                        return "string";
                    case "char":
                        return "char";
                    case "bool":
                        return "bool";
                    default:
                        if (Ejecutor.tc.existe(nodo.Token.Value.ToString().ToLower()))
                        {
                            return nodo.Token.Value.ToString().ToLower();
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error,no existe ese tipo ", "Semantico", -1, -1, "");
                            return "";
                        }
                        break;

                }
            }
            else
            {
                if (Ejecutor.tc.existe(nodo.Token.Text.ToLower()))
                {
                    return nodo.Token.Text.ToLower();
                }
                else
                {
                    return "";
                    Program.getVentana().agregarError("Error, no existe ese tipo ", "Semantico", -1, -1, "");
                }

            }
 
        }
        bool asignarVisibilidad(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes[0].Token.Text.ToLower().Equals("publico"))
            {
                return true;
            } else{
                return false;
            }
            
        }
        int asignarDimension(ParseTreeNode nodo)
        {
            return nodo.ChildNodes.Count;
        }
        
    }
    
}

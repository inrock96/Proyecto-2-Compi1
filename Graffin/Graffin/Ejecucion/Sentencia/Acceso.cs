using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graffin.Ejecucion.Sentencia
{
    class Acceso
    {
        string idObjeto;
        string idAtributo;
        public Acceso(string idObjeto,string idAtributo)
        {
            this.idAtributo = idAtributo;
            this.idObjeto = idObjeto;
        }
        public string getTipo(TablaSimbolos actual)
        {
            if (actual.existe(idObjeto))
            {
                Objeto o;   
                if(actual.sacar(idObjeto).esObjeto())
                {
                    o = (Objeto)actual.sacar(idObjeto);
                    if (o.local.existe(idAtributo))
                    {
                        if (o.local.sacar(idAtributo).visible)
                        {
                            return o.local.sacar(idAtributo).tipo;
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, es privado", "Semantico", -1, -1, "");
                            return null;
                        }
                    }
                    else
                    {
                        return "null";
                        Program.getVentana().agregarError("Error no existe ese atributo", "Semantico", -1, -1, "");
                    }


                }
                else
                {
                    Program.getVentana().agregarError("Error, no es objeto ", "Semantico", -1, -1, "");
                    return "null";
                }
            }
            else
            {
                Program.getVentana().agregarError("Error no existe el objeto", "Semantico", -1, -1, "");
                return "null";
            }
        }
        public Simbolo getValor(TablaSimbolos actual)
        {
            if (actual.existe(idObjeto))
            {
                Objeto o;
               
                if (actual.sacar(idObjeto).esObjeto())
                {
                    o = (Objeto)actual.sacar(idObjeto);
                    if (o.local.existe(idAtributo))
                    {
                        return o.local.sacar(idAtributo);
                    }
                    else
                    {
                        Program.getVentana().agregarError("Error, no existe atributo ", "Semantico", -1, -1, "");
                        return null;
                    }

                }
                else
                {
                    Program.getVentana().agregarError("Error no es objeto ", "Semantico", -1, -1, "");
                    return null;
                }
            }
            else
            {
                Program.getVentana().agregarError("Error, no existe el objeto", "Semantico", -1, -1, "");
                return null;
            }
            return null;
        }
        public void setValor(TablaSimbolos actual,Simbolo nuevo)
        {
            if (actual.existe(idObjeto))
            {
                Objeto o;
                if (actual.sacar(idObjeto).esObjeto())
                {
                    Objeto tmp = (Objeto)actual.sacar(idObjeto);

                    if (tmp.local.existe(idAtributo))
                    {
                        if (tmp.local.sacar(idAtributo).visible)
                        {
                            TablaSimbolos sinModificar = tmp.local;
                            TablaSimbolos nuevos = new TablaSimbolos(null);
                            foreach (object key in sinModificar.listaSimbolos.Keys)
                            {
                                if (!key.ToString().Equals(idAtributo))
                                {
                                    nuevos.agregar(sinModificar.sacar(key.ToString()).identificador, sinModificar.sacar(key.ToString()));
                                }
                            }
                            nuevos.agregar(idAtributo, nuevo);
                            actual.reemplazar(idObjeto, new Objeto(idObjeto, tmp.tipo, tmp.linea,tmp.columna, 0, nuevos, tmp.funciones));
                        }
                        else
                        {
                            Program.getVentana().agregarError("Error, es privado", "Semantico", -1, -1, "");
                        }
                    }
                    else
                    {
                        Program.getVentana().agregarError("Error no existe" , "Semantico", -1, -1, "");
                    }

                }
                else
                {
                    Program.getVentana().agregarError("Error no es objeto", "Semantico", -1, -1, "");
                }
            }
            else
            {
                Program.getVentana().agregarError("Error, no existe", "Semantico", -1, -1, "");
            }
        }
    }
}

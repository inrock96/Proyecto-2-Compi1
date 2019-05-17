using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graffin.Ejecucion.Sentencia
{
    class Figura
    {
        public string tipo;
        public object color;
        public bool solido;
        public double radio;
        public double pos1X, pos1Y, pos2X, pos2Y, pos3X, pos3Y;
        public double thickness;
        public Figura(string tipo, object color, double radio, bool solido, double posX, double posY)
        {//criculo
            this.tipo = tipo;
            this.color = color;
            this.radio = radio;
            this.solido = solido;
            this.pos1X = posX;
            this.pos1Y = posY;
        }
        public Figura()
        {

        }
        public void addCuadrado(string tipo,object color,bool solido,double posX,double posY,double width, double height)
        {
            this.color = color;
            this.solido = solido;
            this.pos1X = posX;
            this.pos1Y = posY;
            this.pos2X = width;
            this.pos2Y = height;
        }
        public Figura(string tipo, object color, bool solido, double pos1X, double pos1Y, double pos2X, double pos2Y, double pos3X, double pos3Y)
        {//triangulo
            this.tipo = tipo;
            this.color = color;
            this.solido = solido;
            this.pos1X = pos1X;
            this.pos1Y = pos1Y;
            this.pos2X = pos2X;
            this.pos2Y = pos2Y;
            this.pos3X = pos3X;
            this.pos3Y = pos3Y;
        }
        public Figura(string tipo, object color, bool solido, double pos1X, double pos1Y, double height, double width)
        {//cuadrado
            this.tipo = tipo;
            this.color = color;
            this.solido = solido;
            this.pos1X = pos1X;
            this.pos1Y = pos1Y;
            this.pos2X = width;
            this.pos2Y = height;
        }
        public Figura(string tipo, object color, double pos1X, double pos1Y, double pos2X, double pos2Y, int thickness)
        {//linea
            this.tipo = tipo;
            this.color = color;
            this.pos1X = pos1X;
            this.pos1Y = pos1Y;
            this.pos2X = pos2X;
            this.pos2Y = pos2Y;
            this.thickness = thickness;
        }
    }
}

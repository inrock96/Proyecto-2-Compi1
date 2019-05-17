using Graffin.Ejecucion;
using Graffin.Ejecucion.Sentencia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graffin
{
    public partial class Graff : Form
    {
        /*palabra reservada de la libreria.. sirve para mover la ventana con el mouse*/
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);
        List<Figura> figuras;
        public Graff(string titulo)
        {
            figuras = Ejecutor.figuras;
            InitializeComponent();
            this.Text = titulo;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brushito;
            Pen penito;
            Color colorsito;
            for(int i=0; i< figuras.Count; i++)
            {
                switch (figuras[i].tipo)
                {
                    case "circle":
                        colorsito =ColorTranslator.FromHtml( figuras[i].color.ToString());
                        if (figuras[i].solido)
                        {
                            brushito = new SolidBrush(colorsito);
                            g.FillEllipse(brushito, Convert.ToInt32(figuras[i].pos1X), Convert.ToInt32(figuras[i].pos1Y), Convert.ToInt32(figuras[i].radio), Convert.ToInt32(figuras[i].radio));
                        }
                        else
                        {
                            penito = new Pen(colorsito);
                            g.DrawEllipse(penito, Convert.ToInt32(figuras[i].pos1X), Convert.ToInt32(figuras[i].pos1Y),Convert.ToInt32(figuras[i].radio), Convert.ToInt32(figuras[i].radio));
                        }
                        break;
                    case "triangle":
                        colorsito = ColorTranslator.FromHtml(figuras[i].color.ToString());
                        if (figuras[i].solido)
                        {
                            brushito = new SolidBrush(colorsito);
                            Point p1 = new Point(Convert.ToInt32(figuras[i].pos1X), Convert.ToInt32( figuras[i].pos1Y));
                            Point p2 = new Point(Convert.ToInt32(figuras[i].pos2X), Convert.ToInt32(figuras[i].pos2Y));
                            Point p3 = new Point(Convert.ToInt32(figuras[i].pos3X), Convert.ToInt32(figuras[i].pos3Y)); 
                            Point[] triangulo = { p1, p2, p3 };
                            g.FillPolygon(brushito, triangulo);
                            
                        }
                        else
                        {
                            penito = new Pen(colorsito);
                            Point p1 = new Point(Convert.ToInt32(figuras[i].pos1X), Convert.ToInt32(figuras[i].pos1Y));
                            Point p2 = new Point(Convert.ToInt32(figuras[i].pos2X), Convert.ToInt32(figuras[i].pos2Y));
                            Point p3 = new Point(Convert.ToInt32(figuras[i].pos3X), Convert.ToInt32(figuras[i].pos3Y));
                            Point[] triangulo = { p1, p2, p3 };
                            g.DrawPolygon(penito, triangulo);
                        }
                        break;
                    case "square":
                        colorsito = ColorTranslator.FromHtml(figuras[i].color.ToString());
                        if (figuras[i].solido)
                        {
                            brushito = new SolidBrush(colorsito);

                            g.FillRectangle(brushito, Convert.ToInt32(figuras[i].pos1X), Convert.ToInt32(figuras[i].pos1Y), Convert.ToInt32(figuras[i].pos2X), Convert.ToInt32(figuras[i].pos2Y));
                        }
                        else
                        {
                            penito = new Pen(colorsito);
                            g.DrawRectangle(penito, Convert.ToInt32(figuras[i].pos1X), Convert.ToInt32(figuras[i].pos1Y), Convert.ToInt32(figuras[i].pos2X), Convert.ToInt32(figuras[i].pos2Y));
                        }
                        break;
                    case "line":
                        colorsito = ColorTranslator.FromHtml(figuras[i].color.ToString());
                        penito = new Pen(colorsito, Convert.ToInt32(figuras[i].thickness));
                        g.DrawLine(penito, Convert.ToInt32(figuras[i].pos1X), Convert.ToInt32(figuras[i].pos1Y), Convert.ToInt32(figuras[i].pos2X), Convert.ToInt32(figuras[i].pos2Y));

                        break;
                    default:
                        break;
                }
            }

        }

        public static bool convert_color(object colorsito)
        {
                try
                {
                    Color miColor = ColorTranslator.FromHtml(colorsito.ToString());
                    return true;
                }catch
                {
                    return false;
                }
            
            
        }
    }
}

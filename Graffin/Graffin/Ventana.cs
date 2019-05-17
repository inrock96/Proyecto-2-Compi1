using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;
using System.IO;
using Graffin.Gramatica;
using Graffin.Ejecucion;
using System.Collections;

namespace Graffin
{
    public partial class Ventana : Form
    {
        List<string> nombrePestanas;
        List<ParseTreeNode> raices;
        List<ErrorSemantico> erroresSintacticos;
        List<ErrorSemantico> erroresLexicos;
        List<ErrorSemantico> erroresSemanticos;
        public static Graff testo;
        bool hayError;
        public Ventana()
        {
            InitializeComponent();
            crearPestana("blank", "");
            nombrePestanas = new List<string>();
            erroresSemanticos = new List<ErrorSemantico>();
            hayError = false;//
            raices = new List<ParseTreeNode>();
            erroresLexicos = new List<ErrorSemantico>();
            erroresSintacticos = new List<ErrorSemantico>();
            dataGridView1.Columns.Add("numero", "No.");
            dataGridView1.Columns.Add("identificador", "ID");
            dataGridView1.Columns.Add("tipo", "Tipo");
            dataGridView1.Columns.Add("valor", "Valor");
            dataGridView1.Columns.Add("linea", "Linea");
            dataGridView1.Columns.Add("columna", "Columna");
     
        }

        private void nuevaPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            crearPestana("Nueva", "");

            //testo.Graff_Paint(sender,new PaintEventArgs(testo.CreateGraphics(),new Rectangle()));
            //testo.agregarLinea(Color.Blue, 0, 0, 60, 90, 2);
            //mostrar();
            //testo = new Graff("lel");
        }
        void cerrarPestana()
        {
            if (tcAnalisis.SelectedIndex > -1)
            {
                TabPage tp = tcAnalisis.TabPages[tcAnalisis.SelectedIndex];
                nombrePestanas.Remove(tp.Name);
                tcAnalisis.TabPages.RemoveAt(tcAnalisis.SelectedIndex);
            }
            else
                MessageBox.Show("Ya no hay mas pestañas");
        }
        public  void mostrar(string titulo)
        {
            Graff testo = new Graff(titulo);
            testo.Show();
        }
        private void abrir()
        {
            StreamReader stream = null;
            string path, texto;
            OpenFileDialog ofd;
            ofd = new OpenFileDialog();
            //FILTRO
            ofd.Title = "Seleccione archivo";
            ofd.Filter = "OLC Files|*.olc";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    path = ofd.FileName.ToString();
                    using (stream = new StreamReader(path))
                        texto = stream.ReadToEnd();
                    if (getRichTextBoxAnalisis() == null)
                    {
                        nombrePestanas.Add(ofd.FileName);
                        crearPestana(ofd.SafeFileName.ToString(), texto);
                    }
                    else
                    {
                        nombrePestanas.Add(ofd.FileName);
                        string nombre = ofd.SafeFileName.ToString();
                        tcAnalisis.TabPages[tcAnalisis.SelectedIndex].Text = nombre;
                        getRichTextBoxAnalisis().Text = texto;
                    }
                }
                catch (IOException e)
                {
                    MessageBox.Show("Error: Hubo un problema al abrir archivo");

                }
            }
        }
        private void guardar()
        {
            //Se crea el flujo de escritura           
            StreamWriter sw = null;
            //Se toma ell nombre de la pestaña actual
            string nombreArchivo = tcAnalisis.TabPages[tcAnalisis.SelectedIndex].Text;
            bool banderita = false;
            try
            {
                //Se recorre la lista de paths 
                foreach (string item in nombrePestanas)
                {
                    //Si contiene el nombre se guarda
                    if (item.Contains(nombreArchivo))
                    {
                        using (sw = new StreamWriter(item))
                        {
                            //Se escribe en esa madre
                            sw.Write(getRichTextBoxAnalisis().Text);
                           
                            MessageBox.Show("SE GUARDO, CREO");
                            banderita = true;
                            break;
                        }
                    }
                }
                if (!banderita)
                {
                    guardarComo();
                }
            }
            catch (IOException e){}

        }
        private void guardarComo()
        {
            StreamWriter sw = null;
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (sw = new StreamWriter(sfd.FileName))
                        sw.Write(getRichTextBoxAnalisis().Text);
                    sw.Close();
                    string nombre = sfd.FileName.ToString();
                    tcAnalisis.TabPages[tcAnalisis.SelectedIndex].Text = nombre;

                }
                catch (Exception)
                {

                    MessageBox.Show("Hubo un error perrón al guardar");
                }
            }
        }
        public RichTextBox getRichTextBoxAnalisis()
        {
            RichTextBox rtb = null;
            TabPage tp = tcAnalisis.SelectedTab;
            if (tp != null)
            {
                rtb = tp.Controls[0] as RichTextBox;
            }
            return rtb;
        }
        void crearPestana(string titulo,string texto)
        {
            RichTextBox rtb1 = new RichTextBox();
            rtb1.Dock = DockStyle.Fill;
            rtb1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TabPage tp1 = new TabPage("" + titulo);
            rtb1.Text = texto;

            tp1.Controls.Add(rtb1);
            tcAnalisis.TabPages.Add(tp1);

        }
        int selectionStart = 0;
        private void posicionPuntero(object sender, EventArgs e)
        {
            if (selectionStart != getRichTextBoxAnalisis().SelectionStart)
            {
                selectionStart = getRichTextBoxAnalisis().SelectionStart;
                lblCambioLinea.Text = selectionStart.ToString();
            }
            selectionStart = getRichTextBoxAnalisis().SelectionStart;
            int pos = selectionStart;
            int fila = 1, columna = 0;
            int ultimalinea = -1;
            String text = getRichTextBoxAnalisis().Text.Replace("\r", "");

            for (int i = 0; i < pos; i++)
            {
                if (text.ToCharArray()[i] == 10)
                {
                    fila++;
                    ultimalinea = i;
                }
            }

            columna = pos - ultimalinea;
            lblCambioLinea.Text = fila + "";
            lblCambioColumna.Text = columna + "";
        }
        
        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cerrarPestana();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrir();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guardar();
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guardarComo();
        }

        private void tcAnalisis_SelectedIndexChanged(object sender, EventArgs e)
        {
            getRichTextBoxAnalisis().SelectionChanged += new System.EventHandler(this.posicionPuntero);
        }

        private void Ventana_Load(object sender, EventArgs e)
        {
            getRichTextBoxAnalisis().SelectionChanged += new System.EventHandler(this.posicionPuntero);
        }

        private void dGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lexicosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (erroresLexicos.Count > 0)
            {
                Reportador r = new Reportador(erroresLexicos, "Lexico.html");
                r.ejecutar();
            }
            else
            {
                MessageBox.Show("No", "Hay");
            }
        }

        private void sintacticosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (erroresSintacticos.Count > 0)
            {
                Reportador r = new Reportador(erroresSintacticos, "Sintactico.html");
                r.ejecutar();
            }
            else
            {
                MessageBox.Show("Hay ", "No");
            }
        }

        private void semanticosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (erroresSemanticos.Count > 0)
            {
                Reportador r = new Reportador(erroresSemanticos, "Semantico.html");
                r.ejecutar();
            }else
            {
                MessageBox.Show("Titulo", "Mensaje");
            }
            
        }

        public void agregarError(string mensaje, string tipo, int fila, int columna, string token)
        {
            erroresSemanticos.Add(new ErrorSemantico(mensaje, tipo, fila, columna, token));
        }

        public void appendSalida(string mensaje)
        {
            richTextBox1.AppendText(mensaje);
        }

        private void ejecutarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            erroresLexicos = new List<ErrorSemantico>();
            erroresSintacticos = new List<ErrorSemantico>();
            erroresSemanticos = new List<ErrorSemantico>();
            RichTextBox rtb = null;
            string cadenaEntrada = "";
            raices = new List<ParseTreeNode>();
            foreach (TabPage pestana in tcAnalisis.TabPages)
            {
                rtb = pestana.Controls[0] as RichTextBox;
                
                Sintactico analizador = new Sintactico();
                cadenaEntrada = rtb.Text;
                ParseTreeNode raiz = analizador.analizar(cadenaEntrada);
                if (raiz != null)
                {
                    raices.Add(raiz);
                }
                else
                {
                    this.erroresLexicos.AddRange(analizador.erroresL);
                    this.erroresSintacticos.AddRange(analizador.erroresS);
                }
                
            }
            Ejecutor ejecutor = new Ejecutor(raices);
            if (raices != null&&erroresLexicos.Count==0&&erroresSintacticos.Count==0) {
                richTextBox1.Text = "";
                ejecutor.ejecutar();
                llenarTabla();
            }
            else MessageBox.Show("Error al leer archivos");
        }
        private void llenarTabla()
        {
            int i=0;
            //dataGridView1.Rows.Add("no", "id","tipo", "valor", "col", "row");
            Hashtable tabla = Ejecutor.tc.getTabla();
            if (tabla != null)
            {
                Clase c;
                foreach(object key in tabla.Keys)
                {
                    c = (Clase)tabla[key.ToString()];
                    if (c != null)
                    {
                        Simbolo s;
                        Funcion f;
                        foreach(object llave in c.global.listaSimbolos.Keys)
                        {
                            s = c.global.sacar(llave.ToString());
                            if (s != null)
                            {
                                i++;
                                dataGridView1.Rows.Add(i, s.identificador, s.tipo, s.valor, s.linea, s.columna);
                            }
                        }
                        foreach (object llave in c.funciones.tabla.Keys)
                        {

                        }
                    }
                }
            }
            
        }
        private void graficarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cadenaEntrada = "";
            cadenaEntrada = getRichTextBoxAnalisis().Text;
            Sintactico analizador = new Sintactico();
            ParseTreeNode raiz = analizador.analizar(cadenaEntrada);

            if (raiz != null)
            {
                Graficador g = new Graficador();
                g.graficar(raiz);


            }
            else
            {
                //mostrarErrores(analizador);
                MessageBox.Show("No se puede graficar, tiene errores");
            }
        }
    }
}

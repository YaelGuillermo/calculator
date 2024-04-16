using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCalculadora;

namespace WFCalculadora
{
    public partial class Form1 : Form
    {
        bool bandera = false, cerrarParentesis = false, fe = false, hyp = false;
        string deg = "deg";
        Panel panHis, panMem;
        Label lbl1His, lbl2His, lbl1Mem;
        int nHis = 1, nMem = 1;
        Label[] lb1His = new Label[0], lb2His = new Label[0], lb1Mem = new Label[0];

        public Form1()
        {
            InitializeComponent();
        }

        private void Historico(string num, string mem)
        {
            panHis = new Panel();
            lbl1His = new Label();
            lbl2His = new Label();

            panHis.Height = 70;
            panHis.Dock = DockStyle.Top;
            panHis.MouseEnter += new EventHandler(PanFoco);
            panHis.MouseLeave += new EventHandler(PanFormaFoco);
            panHis.Click += new EventHandler(PanClickHis);
            panHis.Name = "pan" + nHis;

            lbl1His.Text = mem + " =";
            lbl1His.Font = new Font("Segoe UI", 10);
            lbl1His.Name = "lbl1_" + nHis;
            lbl1His.Left = panelHistorico.Width - lbl1His.Width;

            lbl2His.Text = num;
            lbl2His.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lbl2His.Top = 20;
            lbl2His.Name = "lbl2_" + nHis;
            lbl2His.Height = 32;
            lbl2His.Left = panelHistorico.Width - lbl1His.Width;

            panelHistorico.Controls.Add(panHis);
            panHis.Controls.Add(lbl2His);
            panHis.Controls.Add(lbl1His);

            Array.Resize(ref lb1His, nHis);
            Array.Resize(ref lb2His, nHis);
            lb1His[nHis - 1] = lbl1His;
            lb2His[nHis - 1] = lbl2His;
            nHis++;
        }

        private void PanFoco(object sender, EventArgs e) 
        {
            Panel pan = sender as Panel;
            pan.BackColor = Color.Silver;
        }

        private void PanFormaFoco(object sender, EventArgs e)
        {
            Panel pan = sender as Panel;
            pan.BackColor = Color.Transparent;
        }

        private void PanClickHis(object sender, EventArgs e) 
        {
            Panel panel = sender as Panel;
            lbl1His = lb1His[int.Parse(panel.Name.Remove(0, 3)) - 1]; 
            lbl2His = lb2His[int.Parse(panel.Name.Remove(0, 3)) - 1];

            lblResultado.Text = lbl2His.Text;
            lblOperaciones.Text = lbl1His.Text.Remove(lbl1His.Text.Length - 2, 2); 

            bandera = true;
            cerrarParentesis = false;
        }

        private void Memoria(string num)
        {
            panMem = new Panel();
            lbl1Mem = new Label();

            panMem.Height = 70;
            panMem.Dock = DockStyle.Top;
            panMem.MouseEnter += new EventHandler(PanFoco);
            panMem.MouseLeave += new EventHandler(PanFormaFoco);
            panMem.Click += new EventHandler(PanClickMem);
            panMem.Name = "pan" + nMem;

            lbl1Mem.Text = num;
            lbl1Mem.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lbl1Mem.Top = 20;
            lbl1Mem.Name = "lbl2_" + nMem;
            lbl1Mem.Height = 32;
            lbl1Mem.Left = panelMemoria.Width - lbl1Mem.Width;

            panelMemoria.Controls.Add(panMem);
            panMem.Controls.Add(lbl1Mem);

            Array.Resize(ref lb1Mem, nMem);
            lb1Mem[nMem - 1] = lbl1Mem;
            nMem++;
        }

        private void PanClickMem(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            string a = panel.Name.Remove(0, 2);
            lbl1Mem = lb1Mem[int.Parse(a) - 1];

            lblResultado.Text = lbl1Mem.Text;

            bandera = true;
            cerrarParentesis = false;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.Width <= 600)
            {
                splitContainer1.Panel2Collapsed = true;
            }
            else
            {
                splitContainer1.Panel2Collapsed = false;
            }
        }

        private void btnNum_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (lblResultado.Text == "0" || bandera)
            {
                lblResultado.Text = "";
                bandera = false;
            }
            lblResultado.Text += btn.Text;
        }

        private void btnOperaciones_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string aux;
            if(fe)
                aux = double.Parse(lblResultado.Text).ToString("0.###E+0");
            else
                aux = lblResultado.Text;
            if(fe)
                lblResultado.Text = Evaluar(lblOperaciones.Text + lblResultado.Text).ToString("0.###E+0");
            else
                lblResultado.Text = Evaluar(lblOperaciones.Text + lblResultado.Text).ToString();
            if (cerrarParentesis)
                lblOperaciones.Text += " " + btn.Text + " ";
            else
                lblOperaciones.Text += aux + " " + btn.Text + " ";
            bandera = true;
        }

        private double Evaluar(string expresion) 
        {
            if (expresion.Contains("%"))
            {
                expresion = expresion.Replace("%", "*") + "/ 100";
            }
            expresion = expresion.Replace(",", ".");
            expresion = expresion.Replace("÷", "/");
            expresion = expresion.Replace("×", "*");
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("expresion", string.Empty.GetType(), expresion);
                System.Data.DataRow row = table.NewRow();
                table.Rows.Add(row);
                return double.Parse((string)row["expresion"]);
            }
            catch (Exception)
            {
                return double.Parse("0");
            }
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            lblResultado.Text = "0";
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            lblOperaciones.Text = "";
            lblResultado.Text = "0";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            lblResultado.Text = lblResultado.Text.Substring(0, lblResultado.Text.Length - 1);
        }

        private void btnComa_Click(object sender, EventArgs e)
        {
            if (!lblResultado.Text.Contains(','))
            {
                lblResultado.Text += ",";
            }
        }

        private void btnMasMenos_Click(object sender, EventArgs e)
        {
            lblResultado.Text = (double.Parse(lblResultado.Text) * -1).ToString();
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            string aux = lblOperaciones.Text + lblResultado.Text;
            double resultado = Evaluar(aux);
            if (fe)
                lblResultado.Text = resultado.ToString("0.###E+0");
            else
                lblResultado.Text = resultado.ToString();
            lblOperaciones.Text = "";
            bandera = true;
            Historico(resultado.ToString(), aux);
        }

        private void btnCuadrado_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Pow(double.Parse(lblResultado.Text), 2).ToString();
            bandera = true;
        }

        private void btnPotencia_Click(object sender, EventArgs e)
        {
            string aux = lblResultado.Text;
            lblResultado.Text = Math.Pow(double.Parse(lblResultado.Text), double.Parse(lblResultado.Text)).ToString();
            lblOperaciones.Text += aux + " ^ ";
            bandera = true;
        }

        private void btnSeno_Click(object sender, EventArgs e)
        {
            if (hyp)
                lblResultado.Text = Math.Sinh(double.Parse(lblResultado.Text)).ToString();
            else
                lblResultado.Text = Math.Sin(double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void btnCoseno_Click(object sender, EventArgs e)
        {
            if (hyp)
                lblResultado.Text = Math.Cosh(double.Parse(lblResultado.Text)).ToString();
            else
                lblResultado.Text = Math.Cos(double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void btnTangente_Click(object sender, EventArgs e)
        {
            if (hyp)
                lblResultado.Text = Math.Tanh(double.Parse(lblResultado.Text)).ToString();
            else
                lblResultado.Text = Math.Tan(double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void btnCubo_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Pow(double.Parse(lblResultado.Text), 3).ToString();
            bandera = true;
        }

        private void btnRaizn_Click(object sender, EventArgs e)
        {
            string aux = lblResultado.Text;
            lblResultado.Text = Evaluar(lblOperaciones.Text + lblResultado.Text).ToString();
            lblOperaciones.Text += aux + " nroot ";
            bandera = true;
        }

        private void btnASeno_Click(object sender, EventArgs e)
        {
            if (hyp)
                lblResultado.Text = Math.Pow(Math.Sinh(double.Parse(lblResultado.Text)), -1).ToString();
            else
                lblResultado.Text = Math.Asin(double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void btnACoseno_Click(object sender, EventArgs e)
        {
            if (hyp)
                lblResultado.Text = Math.Pow(Math.Cosh(double.Parse(lblResultado.Text)), -1).ToString();
            else
                lblResultado.Text = Math.Acos(double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void btnATangente_Click(object sender, EventArgs e)
        {
            if (hyp)
                lblResultado.Text = Math.Pow(Math.Tanh(double.Parse(lblResultado.Text)), -1).ToString();
            else
                lblResultado.Text = Math.Atan(double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void btnRaiz_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Sqrt(double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void btn10n_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Pow(10, double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void btnLogaritmo_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Log(double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void btnExponencial_Click(object sender, EventArgs e)
        {
            string aux = lblResultado.Text;
            if (aux.Contains(','))
            {
                lblResultado.Text = aux + "E+";
            }
            else
            {
                lblResultado.Text = aux + ",E+";
            }
        }

        private void btnModular_Click(object sender, EventArgs e)
        {
            string aux = lblResultado.Text;
            lblResultado.Text = Evaluar(lblOperaciones.Text + lblResultado.Text).ToString();
            lblOperaciones.Text += aux + " Mod ";
            bandera = true;
        }

        private void btnUnoEntreX_Click(object sender, EventArgs e)
        {
            lblResultado.Text = (1 / double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void btnEuler_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.Pow(Math.E, double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void btnLogaritmoNatural_Click(object sender, EventArgs e)
        {
            lblResultado.Text = (Math.Log(double.Parse(lblResultado.Text)) / Math.Log(Math.E)).ToString();
            bandera = true;
        }

        private string DecimalToMinutes(string dec) 
        {
            double decimal_degrees = double.Parse(dec);
            int cont = int.Parse(((decimal_degrees - Math.Floor(decimal_degrees)) * 0.6).ToString().Split(',')[0]);
            string degree = (cont + Math.Floor(decimal_degrees)).ToString() + "," + 
                ((decimal_degrees - Math.Floor(decimal_degrees)) / 0.6).ToString().Split(',')[1];
            return degree;
        }

        private void btnDms_Click(object sender, EventArgs e)
        {
            lblResultado.Text = DecimalToMinutes(lblResultado.Text);
            bandera = true;
        }

        private string DecimalToDegree(string dec)
        {
            double decimal_degrees = double.Parse(dec);
            int cont = int.Parse(((decimal_degrees - Math.Floor(decimal_degrees)) / 0.6).ToString().Split(',')[0]);
            string degree = (cont + Math.Floor(decimal_degrees)).ToString() + "," + 
                ((decimal_degrees - Math.Floor(decimal_degrees)) / 0.6).ToString().Split(',')[1];
            return degree;
        }

        private void btnMS_Click(object sender, EventArgs e)
        {
            Memoria(lblResultado.Text);
        }

        private void btnMC_Click(object sender, EventArgs e)
        {
            try
            {
                lb1Mem = new Label[0];
                panelMemoria.Controls.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void btnMR_Click(object sender, EventArgs e)
        {
            try
            {
                string a = panMem.Name.Remove(0, 2);
                lbl1Mem = lb1Mem[int.Parse(a) - 1];

                lblResultado.Text = lbl1Mem.Text;

                bandera = true;
                cerrarParentesis = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnMmas_Click(object sender, EventArgs e)
        {
            double suma = double.Parse(lblResultado.Text);
            double a = double.Parse(lb1Mem[lb1Mem.Length - 1].Text);
            double resultado = suma + a;
            panelMemoria.Controls.Remove(panMem);
            Memoria(resultado.ToString());

            bandera = true;
            cerrarParentesis = false;
        }

        private void btnMmenos_Click(object sender, EventArgs e)
        {
            double resta = double.Parse(lblResultado.Text);
            double a = double.Parse(lb1Mem[lb1Mem.Length - 1].Text);
            double resultado = a - resta;
            panelMemoria.Controls.Remove(panMem);
            Memoria(resultado.ToString());

            bandera = true;
            cerrarParentesis = false;
        }

        private void btnGrado_Click(object sender, EventArgs e)
        {
            lblResultado.Text = DecimalToDegree(lblResultado.Text);
            bandera = true;
        }

        private void btnPi_Click(object sender, EventArgs e)
        {
            lblResultado.Text = Math.PI.ToString();
            bandera = true;
        }

        private void btnFactorial_Click(object sender, EventArgs e)
        {
            double num = double.Parse(lblResultado.Text);
            double factorial = 1;
            for (double i = num; i > 0; i--)
            {
                factorial *= i;
            }
            lblResultado.Text = factorial.ToString();
            bandera = true;
        }

        private void btnAbrirParentesis_Click(object sender, EventArgs e)
        {
            lblOperaciones.Text += " ( ";
            bandera = true;
        }

        private void btnDEG_Click(object sender, EventArgs e)
        {
            if (deg == "deg")
            {
                btnDEG.Text = "RAD";
                deg = "rad";
            }
            else if (deg == "rad")
            {
                btnDEG.Text = "GRAD";
                deg = "grad";
            }
            else
            {
                btnDEG.Text = "DEG";
                deg = "deg";
            }
        }

        private void btnCerrarParentesis_Click(object sender, EventArgs e)
        {
            string aux = lblResultado.Text;
            lblOperaciones.Text += aux + " ) ";
            bandera = true;
            cerrarParentesis = true;
        }

        private void btnHYP_Click(object sender, EventArgs e)
        {
            if (hyp)
            {
                hyp = false;
                btnHYP.FlatAppearance.BorderSize = 0;
            }
            else
            {
                hyp = true;
                btnHYP.FlatAppearance.BorderColor = Color.Red;
                btnHYP.FlatAppearance.BorderSize = 3;
            }
        }

        private void btnFE_Click(object sender, EventArgs e)
        {
            if (fe)
            {
                fe = false;
                btnFE.FlatAppearance.BorderSize = 0;
                lblResultado.Text = double.Parse(lblResultado.Text).ToString();
            }
            else
            {
                fe = true;
                btnFE.FlatAppearance.BorderColor = Color.Red;
                btnFE.FlatAppearance.BorderSize = 3;
                lblResultado.Text = double.Parse(lblResultado.Text).ToString("0.###E+0");
            }
        }
    }
}

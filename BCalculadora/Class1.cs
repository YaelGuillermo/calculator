using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace BCalculadora
{
    public class Calculadora
    {
        private bool bandera = false;
        private bool cerrarParentesis = false;
        private bool fe = false;
        private Label lblResultado;
        private Label lblOperaciones;

        public Calculadora(bool _bandera, bool _cerrarParentesis, bool _fe,
            Label _lblResultado, Label _lblOperaciones) 
        {
            bandera = _bandera;
            cerrarParentesis = _cerrarParentesis;
            fe = _fe;
            lblResultado = _lblResultado;
            lblOperaciones = _lblOperaciones;
        }

        public Label Resultado { get { return lblResultado; } }
        public Label Operaciones { get { return lblOperaciones; } }

        public void cadenaNumero(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (lblResultado.Text == "0" || bandera)
            {
                lblResultado.Text = "";
                bandera = false;
            }
            lblResultado.Text += btn.Text;
        }
        
        public void cadenaOperaciones(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string aux;
            if (fe)
                aux = double.Parse(lblResultado.Text).ToString("0.###E+0");
            else
                aux = lblResultado.Text;
            if (fe)
                lblResultado.Text = Evaluar(lblOperaciones.Text + lblResultado.Text).ToString("0.###E+0");
            else
                lblResultado.Text = Evaluar(lblOperaciones.Text + lblResultado.Text).ToString();
            if (cerrarParentesis)
                lblOperaciones.Text += " " + btn.Text + " ";
            else
                lblOperaciones.Text += aux + " " + btn.Text + " ";
            bandera = true;
        }
        
        public double Evaluar(string expresion)
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

        private void numeroPi()
        {
            lblResultado.Text = Math.PI.ToString();
            bandera = true;
        }

        private void numeroEuler()
        {
            lblResultado.Text = Math.Pow(Math.E, double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void Raiz()
        {
            lblResultado.Text = Math.Sqrt(double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void n()
        {
            lblResultado.Text = Math.Pow(10, double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void Logaritmo()
        {
            lblResultado.Text = Math.Log(double.Parse(lblResultado.Text)).ToString();
            bandera = true;
        }

        private void Exponencial()
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

        private void Cuadrado()
        {
            lblResultado.Text = Math.Pow(double.Parse(lblResultado.Text), 2).ToString();
            bandera = true;
        }

        private void Potencia()
        {
            string aux = lblResultado.Text;
            lblResultado.Text = Math.Pow(double.Parse(lblResultado.Text), double.Parse(lblResultado.Text)).ToString();
            lblOperaciones.Text += aux + " ^ ";
            bandera = true;
        }
    }
}

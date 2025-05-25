using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuenosAires.BodegaBA
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        //nuevo agregado (eithan)
        private void btnConsultarProductos_click(object sender, EventArgs e)
        {
            ProductosDisponibles ventana = new ProductosDisponibles();
            ventana.Show();
            this.Hide();
        }
        
         
        private void btnAdministrarDespacho_click(object sender, EventArgs e)
        {
            GuiaDespacho ventana = new GuiaDespacho();
            ventana.Show();
            this.Hide();

        }
        
             private void btnANWO_click(object sender, EventArgs e)
        {
            ReservasANWO ventana = new ReservasANWO();
            ventana.Show();
            this.Hide();

        }

             private void btnSalir_click(object sender, EventArgs e)
        {
            Application.Exit();

        }
        //fin agregado
         
         
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}

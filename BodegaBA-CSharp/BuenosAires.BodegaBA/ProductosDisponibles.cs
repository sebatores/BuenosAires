using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BuenosAires.ServiceProxy;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;


namespace BuenosAires.BodegaBA
{
    public partial class ProductosDisponibles : Form
    {
        public ProductosDisponibles()
        {
            InitializeComponent();
            this.Load += ProductosDisponibles_Load;

        }

        private void ProductosDisponibles_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        public void CargarProductos()
        {
            var bc = new ScProductoBodega();
            bc.LeerTodos();

            lvBodega.Clear();

            lvBodega.Columns.Add("ID", 150);
            lvBodega.Columns.Add("Nombre", 200);
            lvBodega.Columns.Add("Stock", 200);
            lvBodega.Columns.Add("Disponible", 190);


            lvBodega.View = View.Details;

            foreach (var prod in bc.Lista)
            {
                var item = new ListViewItem(prod.idprod.ToString());
                item.SubItems.Add(prod.nomprod);
                item.SubItems.Add(prod.Cantidad.ToString());
                item.SubItems.Add(prod.Disponibilidad);

                lvBodega.Items.Add(item);
            }

            if (bc.HayErrores)
                this.MensajeInfo(bc.Mensaje);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal();
            menu.Show();
            this.Close();
        }
    }
}

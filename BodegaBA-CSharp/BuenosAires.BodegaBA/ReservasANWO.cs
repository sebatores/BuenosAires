using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BuenosAires.DataLayer;
using BuenosAires.Model.Utiles;
using BuenosAires.ServiceProxy;


namespace BuenosAires.BodegaBA
{
    //nuevo agregado (eithan)
    public partial class ReservasANWO : Form
    {
        public ReservasANWO()
        {
            InitializeComponent();
            this.Text = "ReservasANWO";

            this.Load += (s, e) => CargarProductos();

            grid.CellContentClick += grid_CellContentClick;

            ConfigurarGrid();
         }

        private void ConfigurarGrid()
        {
            grid.Columns.Clear();

            grid.Columns.Add("NroSerie", "NroSerie");
            grid.Columns.Add("NombreProducto", "Nombre Producto");
            grid.Columns.Add("Precio", "Precio");
            grid.Columns.Add("Reservado", "Reservado");

            // Columna Opciones como botón para reservar
            var btnColumn = new DataGridViewButtonColumn();
            btnColumn.Name = "Opciones";
            btnColumn.HeaderText = "Opciones";
            btnColumn.Text = "Reservar";
            btnColumn.UseColumnTextForButtonValue = true;
            grid.Columns.Add(btnColumn);

            // Ajusta propiedades del grid
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
        }

        private void CargarProductos()
        {
            var dc = new DcProductoAnwo();
            dc.LeerTodos();

            if (dc.HayErrores)
            {
                MessageBox.Show(dc.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            grid.Rows.Clear();

            foreach (var prod in dc.Lista)
            {
            
                grid.Rows.Add(
                    prod.NroSerieAnwo,
                    prod.NomProdAnwo,
                    prod.PrecioAnwo.ToString("C"), 
                    prod.Reservado == "1" ? "si" : "no"
                );
            }
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return; // Header

            // Verificamos que la columna clickeada sea la de "Opciones"
            if (grid.Columns[e.ColumnIndex].Name == "Opciones")
            {
                string nroSerie = grid.Rows[e.RowIndex].Cells["NroSerie"].Value?.ToString();

                if (string.IsNullOrEmpty(nroSerie))
                {
                    MessageBox.Show("Número de serie inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirmar la reserva
                var confirmar = MessageBox.Show(
                    $"¿Desea reservar el producto con NroSerie {nroSerie}?",
                    "Confirmar Reserva",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmar == DialogResult.Yes)
                {
                    ReservarProducto(nroSerie);
                }
            }
        }

        private void ReservarProducto(string nroSerie)
        {
            var dc = new DcProductoAnwo();
            string usuario = Environment.UserName; // O bien obtener de sesión o parámetro

            dc.Reservar(nroSerie, usuario);

            if (dc.HayErrores)
            {
                MessageBox.Show(dc.Mensaje, "Error al reservar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(dc.Mensaje, "Reserva Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProductos(); // Refrescar lista para mostrar cambio
            }
        }


        private void btnVolverAlMenuPrincipal_click(object sender, EventArgs e)
        {
            MenuPrincipal ventana = new MenuPrincipal();
            ventana.Show();
            this.Close();

        }
        //fin agregado

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

using BuenosAires.BodegaBA.WsGuiaDespachoReference;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;
using BuenosAires.ServiceProxy;
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
    public partial class VentanaGuiaDespacho : Form
    {
        public VentanaGuiaDespacho()
        {
            InitializeComponent();

            grid.ConfigurarDataGridView(
                  "nrogd:Nro GD, "
                + "nomprod:Producto, "
                + "estadogd:Estado GD, "
                + "nrofac:Nro Fac, "
                + "nomcli:Cliente"
            );
            CargarProductos();

        }




        public void CargarProductos()
        {
            var bc = new ScGuiaDespacho();
            bc.LeerTodos();
            grid.DataSource = bc.Lista;
            grid.RefrescarYajustar();
            if (bc.HayErrores == true) this.MensajeInfo(bc.Mensaje);
        }



        private void btnMenuPrincipal_Click(object sender, EventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal();
            menu.Show();
            this.Close();
        }
    }
}

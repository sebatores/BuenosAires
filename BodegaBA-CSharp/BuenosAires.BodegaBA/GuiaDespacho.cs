using BuenosAires.BodegaBA.WsGuiaDespachoReference;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;
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
    public partial class GuiaDespacho : Form
    {
        public GuiaDespacho()
        {
            InitializeComponent();

        }


        private void GuiaDespacho_Load(object sender, EventArgs e)
        {
            RefrescarDataGridView();
        }


        public void RefrescarDataGridView()
        {
         
        }

        private void btnMenuPrincipal_Click(object sender, EventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal();
            menu.Show();
            this.Close();
        }
    }
}

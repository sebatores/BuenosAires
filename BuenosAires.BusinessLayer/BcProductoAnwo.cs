using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuenosAires.Model;
using BuenosAires.DataLayer;
using BuenosAires.Model.Utiles;

namespace BuenosAires.BusinessLayer
{
    public class BcProductoAnwo
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;

        public ProductoAnwo Producto = null;
        public List<ProductoAnwo> Lista = null;

        private DcProductoAnwo dc = new DcProductoAnwo();

        public BcProductoAnwo()
        {
            Inicializar("");
        }

        public void ObtenerProductos()
        {
            dc.LeerTodos();
            this.Accion = dc.Accion;
            this.Mensaje = dc.Mensaje;
            this.HayErrores = dc.HayErrores;
            this.Lista = dc.Lista;
        }

        public void Inicializar(string accion)
        {
            this.Accion = accion;
            this.Mensaje = "";
            this.HayErrores = false;
            this.Producto = null;
            this.Lista = null;
        }

        public void CopiarPropiedades(DcProductoAnwo dc)
        {
            this.Accion = dc.Accion;
            this.Mensaje = dc.Mensaje;
            this.HayErrores = dc.HayErrores;
            this.Producto = dc.ProductoAnwo;
            this.Lista = dc.Lista;

        }


        public void Reservar(string nroserieanwo)
        {
            var dc = new DcProductoAnwo();
            dc.Reservar(nroserieanwo);
            this.CopiarPropiedades(dc);
        }

    }
  }


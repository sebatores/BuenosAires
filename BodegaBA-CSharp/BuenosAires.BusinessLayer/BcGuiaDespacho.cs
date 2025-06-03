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
    public class BcGuiaDespacho
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public ListaGuiaDespacho GuiaDespacho = null;
        public List<ListaGuiaDespacho> Lista = null;

        public BcGuiaDespacho()
        {
            Inicializar("");
        }

        public void Inicializar(string accion)
        {
            this.Accion = accion;
            this.Mensaje = "";
            this.HayErrores = false;
            this.GuiaDespacho = new ListaGuiaDespacho();
            this.Lista = new List<ListaGuiaDespacho>();
        }

        public void CopiarPropiedades(DcGuiaDespacho dc)
        {
            this.Accion = dc.Accion;
            this.Mensaje = dc.Mensaje;
            this.HayErrores = dc.HayErrores;
            this.GuiaDespacho = dc.GuiaDespacho;
            this.Lista = dc.Lista;
        }

        public bool RetornarError(string mensaje)
        {
            this.HayErrores = true;
            this.Mensaje = mensaje;
            return false;
        }

        public bool ValidarGuia(GuiaDespacho guiaDespacho)
        {
            if (guiaDespacho.nrogd < 0) return RetornarError($"Cuando la guia es nueva, el campo ID debe valer 0");
            if (guiaDespacho.estadogd.Trim() == "") return RetornarError($"El estado de la guia no puede estar vacio");
            if (guiaDespacho.idprod < 0) return RetornarError($"El producto no puede estar vacio");
            if (guiaDespacho.nrofac < 0) return RetornarError($"La factura no puede estar facia");
            return true;
        }

        public bool RetornarMensaje(string mensaje)
        {
            this.Mensaje = mensaje;
            return false;
        }


        //public void Crear(GuiaDespacho guiaDespacho)
        //{
        //    if (ValidarGuia(guiaDespacho) == false) return;
        //    var dcProd = new DcProducto();
        //    dcProd.Leer(guiaDespacho.idprod);
        //    if (dcProd.Producto == null)
        //    {
        //        RetornarError("La ID del producto debe existir con anterioridad");
        //        return;
        //    }

        //    var dc = new DcGuiaDespacho();
        //    dc.Crear(guiaDespacho);
        //    this.CopiarPropiedades(dc);
        //}

        public void LeerTodos()
        {
            var dc = new DcGuiaDespacho();
            dc.LeerTodos();
            this.CopiarPropiedades(dc);
        }

        //public void Leer(int id)
        //{
        //    var dc = new DcGuiaDespacho();
        //    dc.Leer(id);
        //    this.CopiarPropiedades(dc);
        //}

        public void Actualizar(GuiaDespacho guiaDespacho)
        {
            if (ValidarGuia(guiaDespacho) == false) return;
            var dc = new DcGuiaDespacho();
            dc.Actualizar(guiaDespacho);
            this.CopiarPropiedades(dc);
        }

        

        public bool Eliminar(int id)
        {
            this.Inicializar($"Eliminar la guía de despacho con el id {id}");
            var dc = new DcGuiaDespacho();
            dc.Eliminar(id);
            if (dc.HayErrores) return RetornarError(dc.Mensaje);
            this.CopiarPropiedades(dc);
            return true;
        }

        public void CambiarEstado(int nrogd, string estado)
        {
            var dc = new DcGuiaDespacho();
            dc.CambiarEstado(nrogd, estado);
            this.CopiarPropiedades(dc);
        }
    }
}

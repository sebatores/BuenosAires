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
        public GuiaDespacho GuiaDespacho = null;
        public List<GuiaDespacho> Lista = null;

        public BcGuiaDespacho()
        {
            Inicializar("");
        }

        public void Inicializar(string accion)
        {
            this.Accion = accion;
            this.Mensaje = "";
            this.HayErrores = false;
            this.GuiaDespacho = null;
            this.Lista = null;
        }

        private bool ErrCampoRequerido(string nombreCampo)
        {
            this.Mensaje = $"{nombreCampo} es un campo requerido, por lo que debe tener un valor.";
            return false;
        }

        private bool ErrID()
        {
            this.Mensaje = $"Cuando la guía de despacho es nueva el campo ID debe valer 0.";
            return false;
        }

        public bool RetornarError(string mensaje)
        {
            this.HayErrores = true;
            this.Mensaje = mensaje;
            return false;
        }

        public bool RetornarMensaje(string mensaje)
        {
            this.Mensaje = mensaje;
            return false;
        }

        public bool ValidarGuia(GuiaDespacho guia)
        {
            this.HayErrores = true;
            if (guia.idguia < 0) return ErrID();
            if (string.IsNullOrWhiteSpace(guia.descripcion)) return ErrCampoRequerido("Descripción de la guía de despacho");
            // Agrega aquí más validaciones si tu clase `GuiaDespacho` tiene más campos importantes
            this.HayErrores = false;
            return true;
        }

        public void Crear(GuiaDespacho guia)
        {
            if (ValidarGuia(guia) == false) return;
            var dc = new DcGuiaDespacho();
            dc.Crear(guia);
            Util.CopiarPropiedades(dc, this);
        }

        public void LeerTodos()
        {
            var dc = new DcGuiaDespacho();
            dc.LeerTodos();
            Util.CopiarPropiedades(dc, this);
        }

        public void Leer(int id)
        {
            var dc = new DcGuiaDespacho();
            dc.Leer(id);
            Util.CopiarPropiedades(dc, this);
        }

        public void Actualizar(GuiaDespacho guia)
        {
            if (ValidarGuia(guia) == false) return;
            var dc = new DcGuiaDespacho();
            dc.Actualizar(guia);
            Util.CopiarPropiedades(dc, this);
        }

        

        public bool Eliminar(int id)
        {
            this.Inicializar($"Eliminar la guía de despacho con el id {id}");
            var dc = new DcGuiaDespacho();
            dc.Eliminar(id);
            if (dc.HayErrores) return RetornarError(dc.Mensaje);
            Util.CopiarPropiedades(dc, this);
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BuenosAires.BusinessLayer;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;
using BuenosAires.ServiceLayer;

namespace BuenosAires.ServiceLayer
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "WsGuiaDespacho" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione WsGuiaDespacho.svc o WsGuiaDespacho.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class WsGuiaDespacho : IWsGuiaDespacho
    {
        public Respuesta ObtenerRespuesta(BcGuiaDespacho bc)
        {
            var respuesta = new Respuesta();
            respuesta.Accion = bc.Accion;
            respuesta.Mensaje = bc.Mensaje;
            respuesta.HayErrores = bc.HayErrores;
            respuesta.XmlGuiaDespacho = Util.SerializarXML(bc.GuiaDespacho);
            respuesta.XmlListaGuiaDespacho = Util.SerializarXML(bc.Lista);
            return respuesta;

        }

        public Respuesta ValidarProducto(GuiaDespacho guiaDespacho)
        {
            var bc = new BcGuiaDespacho();
            bc.ValidarGuia(guiaDespacho);
            return ObtenerRespuesta(bc);
        }

        public Respuesta Crear(GuiaDespacho guiaDespacho)
        {
            var bc = new BcGuiaDespacho();
            bc.Crear(guiaDespacho);
            return ObtenerRespuesta(bc);
        }

        public Respuesta LeerTodos()
        {
            var bc = new BcGuiaDespacho();
            bc.LeerTodos();
            return ObtenerRespuesta(bc);
        }


        public Respuesta Leer(int id)
        {
            var bc = new BcGuiaDespacho();
            bc.Leer(id);
            return ObtenerRespuesta(bc);
        }

        public Respuesta Actualizar(GuiaDespacho guiaDespacho)
        {
            var bc = new BcGuiaDespacho();
            bc.Actualizar(guiaDespacho);
            return ObtenerRespuesta(bc);
        }

        public Respuesta Eliminar(int id)
        {
            var bc = new BcGuiaDespacho();
            bc.Eliminar(id);
            return ObtenerRespuesta(bc);
        }

        public Respuesta CambiarEstado(int nrogd, string estado)
        {
            var bc = new BcGuiaDespacho();
            bc.CambiarEstado(nrogd, estado);
            return ObtenerRespuesta(bc);
        }


    }
}
    





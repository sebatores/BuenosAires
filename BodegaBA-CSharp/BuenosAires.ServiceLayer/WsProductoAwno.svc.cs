using BuenosAires.BusinessLayer;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BuenosAires.ServiceLayer
{
    public class WsProductoAwno : IWsProductoAwno
    {
        private Respuesta ObtenerRespuesta(BcProductoAnwo bc)
        {
            var respuesta = new Respuesta();
            respuesta.Accion = bc.Accion;
            respuesta.Mensaje = bc.Mensaje;
            respuesta.HayErrores = bc.HayErrores;
            respuesta.XmlProducto = Util.SerializarXML(bc.Producto);
            respuesta.XmlListaProducto = Util.SerializarXML(bc.Lista);
            return respuesta;
        }

        public Respuesta LeerTodos()
        {
            var bc = new BcProductoAnwo();
            bc.ObtenerProductos();
            return ObtenerRespuesta(bc);
        }

        public Respuesta Reservar(string nroSerie, string usuario)
        {
            var bc = new BcProductoAnwo();
            bc.ReservarProducto(nroSerie, usuario);
            return ObtenerRespuesta(bc);
        }


    }
}

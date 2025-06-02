using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;
using BuenosAires.ServiceLayer;

namespace BuenosAires.ServiceLayer
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "WsGuiaDespacho" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione WsGuiaDespacho.svc o WsGuiaDespacho.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class WsGuiaDespacho : IWsGuiaDespacho
    {
        public Respuesta ObtenerRespuesta(WsGuiaDespacho wg)
        {
            var respuesta = new Respuesta();
            respuesta.Accion = wg.Accion;
            respuesta.Mensaje = wg.Mensaje;
            respuesta.HayErrores = wg.HayErrores;
            respuesta.XmlGuiaDespacho = Util.SerializarXML(wg.GuiaDespacho);
            respuesta.XmlListaGuiaDespacho = Util.SerializarXML(wg.Lista);
            return respuesta;

        }



        public Respuesta Leer(int id)
        {
            var wg = new WsGuiaDespacho();
            wg.Leer(id);
            return ObtenerRespuesta(wg);
        }

        public Respuesta Actualizar(GuiaDespacho guiaDespacho)
        {
            var wg = new WsGuiaDespacho();
            wg.Actualizar(guiaDespacho);
            return ObtenerRespuesta(wg);
        }

        public Respuesta Eliminar(int id)
        {
            var wg = new WsGuiaDespacho();
            wg.Eliminar(id);
            return ObtenerRespuesta(wg);
        }

        public Respuesta CambiarEstado(int nrogd, string estado)
        {
            var wg = new WsGuiaDespacho();
            wg.CambiarEstado(nrogd, estado);
            return ObtenerRespuesta(wg);
        }


    }
}
    





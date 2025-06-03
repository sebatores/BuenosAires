using BuenosAires.BodegaBA.WsGuiaDespachoReference;
using BuenosAires.BodegaBA.WsProductoReference;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAires.ServiceProxy
{
    public class ScGuiaDespacho
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public ListaGuiaDespacho GuiaDespacho = null;
        public List<ListaGuiaDespacho> Lista = null;

        public void CopiarPropiedades(Respuesta resp)
        {
            this.Accion = resp.Accion;
            this.Mensaje = resp.Mensaje;
            this.HayErrores = resp.HayErrores;
            this.GuiaDespacho = Util.DeserializarXML<ListaGuiaDespacho>(resp.XmlGuiaDespacho);
            this.Lista = Util.DeserializarXML<List<ListaGuiaDespacho>>(resp.XmlListaGuiaDespacho);
        }

        public WsGuiaDespachoClient getWs()
        {
            var ws = new WsGuiaDespachoClient();
            ws.InnerChannel.OperationTimeout = new TimeSpan(1, 0, 0);
            return ws;
        }

        //public void Crear(GuiaDespacho guiaDespacho)
        //{
        //    CopiarPropiedades(getWs().Crear(guiaDespacho));
        //}

        public void LeerTodos()
        {
            CopiarPropiedades(getWs().LeerTodos());
        }

        public void Leer(int id)
        {
            CopiarPropiedades(getWs().Leer(id));
        }

        //public void Actualizar(Producto producto)
        //{
        //    CopiarPropiedades(getWs().Actualizar(producto));
        //}

        public void Eliminar(int id)
        {
            CopiarPropiedades(getWs().Eliminar(id));
        }

        public void CambiarEstado(int nrogd, string estado)
        {
            CopiarPropiedades(getWs().CambiarEstado(nrogd, estado));
        }

    }
}

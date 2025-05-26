using BuenosAires.Model.Utiles;
using BuenosAires.Model;
using System;
using System.Collections.Generic;
using BuenosAires.BodegaBA.WsProductoAnwoReference;


namespace BuenosAires.BodegaBA
{
    public class ScProductoAnwo
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public ProductoAnwo Producto = null;
        public List<ProductoAnwo> Lista = null;

        private void CopiarPropiedades(Respuesta resp)
        {
            this.Accion = resp.Accion;
            this.Mensaje = resp.Mensaje;
            this.HayErrores = resp.HayErrores;
            this.Producto = Util.DeserializarXML<ProductoAnwo>(resp.XmlProducto);
            this.Lista = Util.DeserializarXML<List<ProductoAnwo>>(resp.XmlListaProducto);
        }

        private WsProductoAwnoClient getWs()
        {
            var ws = new WsProductoAwnoClient();
            ws.InnerChannel.OperationTimeout = new TimeSpan(1, 0, 0);
            return ws;
        }

        public void LeerTodos()
        {
            var respuesta = getWs().LeerTodos();
            CopiarPropiedades(respuesta);
        }

        public void Reservar(string nroSerie, string usuario)
        {
            var respuesta = getWs().Reservar(nroSerie, usuario);
            CopiarPropiedades(respuesta);
        }
    }
}

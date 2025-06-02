using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BuenosAires.Model;
using BuenosAires.BusinessLayer;

namespace BuenosAires.ServiceLayer
{

    [ServiceContract]
    public interface IWsGuiaDespacho
    {
        [OperationContract]
        Respuesta ObtenerRespuesta(BcGuiaDespacho bc);

        [OperationContract]
        Respuesta ValidarProducto(GuiaDespacho guiaDespacho);

        [OperationContract]
        Respuesta Crear(GuiaDespacho guiaDespacho);

        [OperationContract]
        Respuesta LeerTodos();

        [OperationContract]
        Respuesta Leer(int id);

        [OperationContract]
        Respuesta Actualizar(GuiaDespacho guiaDespacho);

        [OperationContract]
        Respuesta Eliminar(int id);

        [OperationContract]
        Respuesta CambiarEstado(int nrogd, string estado);
    }

}
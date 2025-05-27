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
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IWsGuiaDespacho" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IWsGuiaDespacho
    {
        [OperationContract]
        void DoWork();
    }
}

[ServiceContract]
public interface IWsGuiaDespacho
{
    [OperationContract]
    Respuesta ObtenerRespuesta();

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
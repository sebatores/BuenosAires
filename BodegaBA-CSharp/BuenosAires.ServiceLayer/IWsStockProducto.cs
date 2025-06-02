using System.ServiceModel;
using System.Xml;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using BuenosAires.Model;
using System.Text;



namespace BuenosAires.ServiceLayer
{
    [ServiceContract]
    public interface IWsStockProducto
    {
        [OperationContract]
        Respuesta ObtenerEquiposEnBodega();
    }
}
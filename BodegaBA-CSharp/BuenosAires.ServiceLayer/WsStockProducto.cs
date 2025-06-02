using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;
using BuenosAires.BusinessLayer;
using System.Net.Http;
using BuenosAires.ServiceLayer;
using System.Xml;

public class WsStockProducto : IWsStockProducto
{
    public Respuesta ObtenerEquiposEnBodega()
    {
        var resp = new Respuesta();
        resp.Accion = "obtener equipos en bodega";
        resp.Mensaje = "";
        resp.HayErrores = false;
        resp.JsonStockProducto = "";

        string apiUrl = "http://127.0.0.1:8000/BuenosAiresApiRest/obtener_equipos_en_bodega";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Espera la respuesta de forma síncrona
                var response = client.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    resp.JsonStockProducto = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    resp.HayErrores = true;
                    resp.Mensaje = "No se pudo obtener el stock desde la API. Código: " + response.StatusCode;
                }
            }
        }
        catch (Exception ex)
        {
            resp.HayErrores = true;
            resp.Mensaje = Util.MensajeError(resp.Accion, "WsStockProducto.ObtenerEquiposEnBodega", ex);
        }

        return resp;
    }
}


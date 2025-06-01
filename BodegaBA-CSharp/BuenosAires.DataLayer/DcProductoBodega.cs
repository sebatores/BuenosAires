using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;

namespace BuenosAires.DataLayer
{
    public class DcProductoBodega
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public List<ProductoBodega> Lista = null;

        public DcProductoBodega()
        {
            Inicializar("Obtener productos en bodega");
        }

        public void Inicializar(string accion)
        {
            this.Accion = accion;
            this.Mensaje = "";
            this.HayErrores = false;
            this.Lista = null;
        }

        public void LeerTodos()
        {
            Inicializar("obtener productos en bodega");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    Lista = bd.Database
                        .SqlQuery<ProductoBodega>("EXEC SP_OBTENER_EQUIPOS_EN_BODEGA")
                        .ToList();

                    if (Lista.Count == 0)
                        Mensaje = "No hay productos disponibles en bodega";
                    else
                        Mensaje = $"Se encontraron {Lista.Count} productos en bodega";
                }
            }
            catch (Exception ex)
            {
                HayErrores = true;
                Mensaje = Util.MensajeError(Accion, "DcProductoBodega.LeerTodos", ex);
            }
        }
    }
}

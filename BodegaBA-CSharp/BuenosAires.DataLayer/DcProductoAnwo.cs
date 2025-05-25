using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;

namespace BuenosAires.DataLayer
{
    public class DcProductoAnwo
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public ProductoAnwo Producto = null;
        public List<ProductoAnwo> Lista = null;

        public DcProductoAnwo()
        {
            Inicializar("");
        }

        public void Inicializar(string accion)
        {
            this.Accion = accion;
            this.Mensaje = "";
            this.HayErrores = false;
            this.Producto = null;
            this.Lista = null;
        }

        public void LeerTodos()
        {
            Inicializar("obtener la lista de productos ANWO");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    Lista = bd.Database
                        .SqlQuery<ProductoAnwo>("EXEC SP_OBTENER_EQUIPOS_ANWO")
                        .ToList();

                    if (Lista.Count == 0) Mensaje = "La lista de productos ANWO está vacía";
                    else Mensaje = $"Se cargaron {Lista.Count} productos ANWO";
                }
            }
            catch (Exception ex)
            {
                HayErrores = true;
                Mensaje = Util.MensajeError(Accion, "DcProductoAnwo.LeerTodos", ex);
            }
        }

        public void Leer(string nroserieanwo)
        {
            Inicializar($"obtener el producto ANWO con nroserie '{nroserieanwo}'");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    var param = new SqlParameter("@nroserieanwo", nroserieanwo);

                    var resultado = bd.Database
                        .SqlQuery<ProductoAnwo>("EXEC SP_OBTENER_EQUIPO_ANWO @nroserieanwo", param)
                        .FirstOrDefault();

                    if (resultado == null)
                    {
                        Mensaje = $"No fue posible {Accion} pues no existe en la BD";
                    }
                    else
                    {
                        Producto = resultado;
                        Mensaje = $"Producto ANWO con nroserie {nroserieanwo} cargado correctamente";
                    }
                }
            }
            catch (Exception ex)
            {
                HayErrores = true;
                Mensaje = Util.MensajeError(Accion, "DcProductoAnwo.Leer", ex);
            }
        }

        public void Reservar(string nroserieanwo, string usuario)
        {
            Inicializar($"reservar el producto ANWO con nroserie '{nroserieanwo}' para usuario '{usuario}'");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    var paramNroSerie = new SqlParameter("@nroserieanwo", nroserieanwo);
                    var paramUser = new SqlParameter("@usuario", usuario);
                    var paramMensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.VarChar, 500)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };

                    bd.Database.ExecuteSqlCommand(
                        "EXEC SP_RESERVAR_EQUIPO_ANWO @nroserieanwo, @usuario, @mensaje OUTPUT",
                        paramNroSerie, paramUser, paramMensaje);

                    Mensaje = paramMensaje.Value?.ToString() ?? "Reserva completada";
                }
            }
            catch (Exception ex)
            {
                HayErrores = true;
                Mensaje = Util.MensajeError(Accion, "DcProductoAnwo.Reservar", ex);
            }
        }
    }
}

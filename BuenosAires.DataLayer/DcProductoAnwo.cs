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
        public ProductoAnwo ProductoAnwo = null;
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
            this.ProductoAnwo = null;
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
                        ProductoAnwo = resultado;
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

        public void Reservar(string nroserieanwo)
        {
            Inicializar($"reservar el producto ANWO con nroserie '{nroserieanwo}'");
            try
            {
                var bd = new base_datosEntities();
                var encontrado = bd.AnwoStockProducto.FirstOrDefault(p => p.nroserieanwo == nroserieanwo);
                if (encontrado == null)
                {
                    this.Mensaje = $"No fue posible {Accion} pues no existe en la BD";
                }
                else
                {
                    encontrado.reservado = "S";
                    bd.SaveChanges();
                    this.ProductoAnwo = new ProductoAnwo();
                    Util.CopiarPropiedades(encontrado, this.ProductoAnwo);
                    this.Mensaje = $"El producto '{ProductoAnwo.NroSerieAnwo}' fue reservado";
                }
                bd.Dispose();
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, $"No fue posible ", ex);
            }
        }
    }
}

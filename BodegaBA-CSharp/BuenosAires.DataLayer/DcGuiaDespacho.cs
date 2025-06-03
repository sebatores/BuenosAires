using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;

namespace BuenosAires.DataLayer
{
    public class DcGuiaDespacho
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public ListaGuiaDespacho GuiaDespacho = null;
        public List<ListaGuiaDespacho> Lista = null;

        public DcGuiaDespacho()
        {
            Inicializar("");
        }

        public void Inicializar(string accion)
        {
            this.Accion = accion;
            this.Mensaje = "";
            this.HayErrores = false;
            this.GuiaDespacho = null;
            this.Lista = null;
        }

        public int ContarGuiasDespachoPorProducto(int idprod)
        {
            this.Inicializar($"contar las guias de despacho asociadas al producto con el ID '{idprod}'");
            try
            {
                var bd = new base_datosEntities();
                int cantidad = bd.GuiaDespacho.Count(g => g.idprod == idprod);
                bd.Dispose();
                return cantidad;
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, $"DcGuiaDespacho.ContarGuiasDespachoPorProducto", ex);
                return -1;
            }
        }

        public int ContarGuiaDespachoPorFactura(int nrofac)
        {
            this.Inicializar($"Contar las guias de despacho asociadas a la factura con el numero {nrofac}");

            try
            {
                var bd = new base_datosEntities();
                int cantidad = bd.GuiaDespacho.Count(g => g.nrofac == nrofac);
                bd.Dispose();
                return cantidad;
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, $"DcGuiaDespacho.ContarGuiaDespachoPorFactura", ex);
                return -1;
            }

        }

        public int obtenerSiguienteId()
        {
            this.Inicializar("obtener un nuevo ID");
            int siguienteId = -1;
            try
            {
                var bd = new base_datosEntities();
                siguienteId = -1;
                if (bd.GuiaDespacho.Count() > 0) siguienteId = bd.GuiaDespacho.Max(s => s.nrogd);
                bd.Dispose();
                return siguienteId;
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, "DcGuiaDespacho.obtenerSiguienteId", ex);
                return -1;
            }
        }

        //public void Crear(GuiaDespacho guiaDespacho)
        //{
        //    this.Inicializar($"crear la guía de despacho '{this.obtenerSiguienteId()}'");
        //    try
        //    {

        //        var bd = new base_datosEntities();
        //        int siguienteId = this.obtenerSiguienteId();
        //        if (this.HayErrores) return;

        //        guiaDespacho.nrogd = siguienteId;
        //        bd.GuiaDespacho.Add(guiaDespacho);
        //        bd.SaveChanges();
        //        this.GuiaDespacho = guiaDespacho;
        //        this.Mensaje = $"La guía de despacho '{guiaDespacho.nrogd}' fue creada correctamente";

        //    }
        //    catch (Exception ex)
        //    {
        //        this.HayErrores = true;
        //        this.Mensaje = Util.MensajeError(this.Accion, "DcGuiaDespacho.Crear", ex);
        //    }
        //}


        public void LeerTodos()
        {
            Inicializar("obtener la lista de guías de despacho");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    Lista = bd.Database
                        .SqlQuery<ListaGuiaDespacho>(
                            "EXEC SP_OBTENER_GUIAS_DE_DESPACHO @tipousu, @rut",
                            new SqlParameter("@tipousu", "Bodeguero"),
                            new SqlParameter("@rut", "0000-0")
                        )
                        .ToList();

                    if (Lista.Count == 0)
                        this.Mensaje = "La lista de Guías de despacho se encuentra vacía";
                    else
                        this.Mensaje = $"Se encontraron {Lista.Count} guías de despacho";
                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, "DcGuiaDespacho.LeerTodos", ex);
            }
        }


        //public void Leer(int idguia)
        //{
        //    this.Inicializar($"obtener la guía de despacho con el ID '{idguia}'");
        //    try
        //    {
        //        using (var bd = new base_datosEntities())
        //        {
        //            this.GuiaDespacho = bd..FirstOrDefault(p => p.nrogd == idguia);
        //            bd.Dispose();
        //            if (this.GuiaDespacho == null)
        //                this.Mensaje = $"No fue posible {this.Accion} pues no existe en la BD";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.HayErrores = true;
        //        this.Mensaje = Util.MensajeError(this.Accion, "DcGuiaDespacho.Leer", ex);
        //    }
        //}

        public void Actualizar(GuiaDespacho guiaDespacho)
        {
            this.Inicializar($"actualizar la guía de despacho '{guiaDespacho.nrogd}'");
            try
            {
                var bd = new base_datosEntities();

                var encontrado = bd.GuiaDespacho.FirstOrDefault(gd => gd.nrogd == guiaDespacho.nrogd);
                if (encontrado == null)
                {
                    this.Mensaje = $"No fue posible {this.Accion} pues no existe en la BD";
                }
                else
                {
                    Util.CopiarPropiedades(guiaDespacho, encontrado);
                    bd.SaveChanges();
                    this.GuiaDespacho = new ListaGuiaDespacho();
                    Util.CopiarPropiedades(guiaDespacho, this.GuiaDespacho);
                    this.Mensaje = $"La guía de despacho '{guiaDespacho.nrogd}' fue actualizada correctamente";
                }
                bd.Dispose();
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, "DcGuiaDespacho.Actualizar", ex);
            }
        }

        public void Eliminar(int idguia)
        {
            this.Inicializar($"eliminar la guía de despacho con el ID '{idguia}'");
            try
            {
                var bd = new base_datosEntities();

                var encontrado = bd.GuiaDespacho.FirstOrDefault(gd => gd.nrogd == idguia);
                if (encontrado == null)
                {
                    this.Mensaje = $"No fue posible {this.Accion} pues no existe en la BD";
                }
                else
                {
                    bd.GuiaDespacho.Remove(encontrado);
                    bd.SaveChanges();
                    this.Mensaje = $"La guía de despacho '{encontrado.nrogd}' fue eliminada correctamente";
                }
                bd.Dispose();
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, "DcGuiaDespacho.Eliminar", ex);
            }
        }


        public void CambiarEstado(int nrogd, string estado)
        {
            this.Inicializar($"Cambiar el estado de la guia de despacho '{nrogd}' a '{estado}'");
            try
            {
                var bd = new base_datosEntities();
                var encontrado = bd.GuiaDespacho.FirstOrDefault(gd => gd.nrogd == nrogd);
                if (encontrado == null)
                {
                    this.Mensaje = $"No fue posible {this.Accion} pues no existe en la BD";
                }
                else
                {
                    encontrado.estadogd = estado;
                    bd.SaveChanges();
                    this.Mensaje = $"La Guia de Despacho numero '{nrogd}' ahora esta en el estado {estado}";
                }
                bd.Dispose();
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, $"DcGuiaDespacho.CambiarEstado", ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
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
        public GuiaDespacho GuiaDespacho = null;
        public List<GuiaDespacho> Lista = null;

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

        public int ObtenerSiguienteId()
        {
            Inicializar("obtener un nuevo ID");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    int siguienteID = 1;
                    if (bd.GuiaDespacho.Count() > 0)
                        siguienteID = bd.GuiaDespacho.Max(g => g.idguia) + 1;

                    this.Mensaje = "Se ha logrado crear el nuevo ID";
                    return siguienteID;
                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, "DcGuiaDespacho.ObtenerSiguienteId", ex);
                return -1;
            }
        }

        public void Crear(GuiaDespacho guia)
        {
            this.Inicializar($"crear la guía de despacho '{guia.descripcion}'");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    int siguienteId = this.ObtenerSiguienteId();
                    if (this.HayErrores) return;

                    guia.idguia = siguienteId;
                    bd.GuiaDespacho.Add(guia);
                    bd.SaveChanges();
                    this.GuiaDespacho = guia;
                    this.Mensaje = $"La guía de despacho '{guia.descripcion}' fue creada correctamente";
                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, "DcGuiaDespacho.Crear", ex);
            }
        }

        public void LeerTodos()
        {
            this.Inicializar("obtener la lista de guías de despacho");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    this.Lista = bd.GuiaDespacho.ToList();
                    if (this.Lista.Count == 0) this.Mensaje = "La lista de guías de despacho está vacía";
                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, "DcGuiaDespacho.LeerTodos", ex);
            }
        }

        public void Leer(int idguia)
        {
            this.Inicializar($"obtener la guía de despacho con el ID '{idguia}'");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    this.GuiaDespacho = bd.GuiaDespacho.FirstOrDefault(g => g.idguia == idguia);
                    if (this.GuiaDespacho == null)
                        this.Mensaje = $"No fue posible {this.Accion} pues no existe en la BD";
                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, "DcGuiaDespacho.Leer", ex);
            }
        }

        public void Actualizar(GuiaDespacho guia)
        {
            this.Inicializar($"actualizar la guía de despacho '{guia.descripcion}'");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    var encontrado = bd.GuiaDespacho.FirstOrDefault(g => g.idguia == guia.idguia);
                    if (encontrado == null)
                    {
                        this.Mensaje = $"No fue posible {this.Accion} pues no existe en la BD";
                    }
                    else
                    {
                        Util.CopiarPropiedades(guia, encontrado);
                        bd.SaveChanges();
                        this.GuiaDespacho = new GuiaDespacho();
                        Util.CopiarPropiedades(guia, this.GuiaDespacho);
                        this.Mensaje = $"La guía de despacho '{guia.descripcion}' fue actualizada correctamente";
                    }
                }
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
                using (var bd = new base_datosEntities())
                {
                    var encontrado = bd.GuiaDespacho.FirstOrDefault(g => g.idguia == idguia);
                    if (encontrado == null)
                    {
                        this.Mensaje = $"No fue posible {this.Accion} pues no existe en la BD";
                    }
                    else
                    {
                        bd.GuiaDespacho.Remove(encontrado);
                        bd.SaveChanges();
                        this.Mensaje = $"La guía de despacho '{encontrado.descripcion}' fue eliminada correctamente";
                    }
                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError(this.Accion, "DcGuiaDespacho.Eliminar", ex);
            }
        }
    }
}

using BuenosAires.BusinessLayer;
using BuenosAires.DataLayer;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;
using System;
using System.Collections.Generic;

namespace BuenosAires.BodegaBA
{
    public class ScProductoBodega
    {
        public string Mensaje = "";
        public bool HayErrores = false;
        public List<ProductoBodega> Lista = null;

        public void LeerTodos()
        {
            var bc = new BcProductoBodega();
            bc.LeerTodos();

            this.Mensaje = bc.Mensaje;
            this.HayErrores = bc.HayErrores;
            this.Lista = bc.Lista;
        }
    }
}

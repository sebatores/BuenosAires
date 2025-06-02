using System.Collections.Generic;
using BuenosAires.DataLayer;
using BuenosAires.Model;

namespace BuenosAires.BusinessLayer
{
    public class BcProductoBodega
    {
        public string Mensaje = "";
        public bool HayErrores = false;
        public List<ProductoBodega> Lista = null;

        public void LeerTodos()
        {
            var dc = new DcProductoBodega();
            dc.LeerTodos();

            this.Mensaje = dc.Mensaje;
            this.HayErrores = dc.HayErrores;
            this.Lista = dc.Lista;
        }
    }
}
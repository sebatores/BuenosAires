using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAires.Model
{
    public class ProductoBodega
    {
        public int idprod { get; set; }
        public string nomprod { get; set; }
        public int Cantidad { get; set; }
        public string Disponibilidad { get; set; }
    }
}

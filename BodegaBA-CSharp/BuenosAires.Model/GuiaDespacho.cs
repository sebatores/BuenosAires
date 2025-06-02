using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAires.Model
{
    using System;
    using System.Collections.Generic;

    public partial class GuiaDespacho
    {
        public int nrogd { get; set; }
        public string estadogd { get; set; }
        public int nrofac { get; set; }
        public int idprod { get; set; }

        public virtual Factura Factura { get; set; }
        public virtual Producto Producto { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Procesos.TD
{
    public class DetalleFactura
    {
        public int id_comprobante { get; set; }
        public int orden_detalle { get; set; }
        public string cuenta { get; set; }
        public string descripcion { get; set; }
        public double importe { get; set; }
        public string afectoImpuesto { get; set; }
    }
}

/*
 * Fecha de Modificación : 01/06/2016
 * Modificado por        : Juan Castro
 * Nro. Orden de Trabajo : 8844
 * Descripción del cambio: Creación.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaProcesosDA;
using System.Data;

namespace SistemaProcesosBL
{
    public class ConsultaInversionesBL
    {
        ConsultaInversionesDA da = new ConsultaInversionesDA();
        public DataTable ObtenerValorCuotaInversiones(DateTime fecha, string portafolio, string serie)
        {
            return da.ObtenerValorCuota(fecha, portafolio, serie);
        }

        public DataTable ListarPortafolios()
        {
            return da.ListarPortafolios();
        }
    }
}

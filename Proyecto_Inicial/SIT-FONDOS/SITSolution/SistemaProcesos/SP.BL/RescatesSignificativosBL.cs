/*
 * Fecha Modificación	: 15/06/2015
 * Modificado por		: Robert Castillo
 * Nro de OT			: 7370
 * Descripción del cambio: Se crea la clase que se encarga de la lógica de negocio para los rescates significativos.
 * ********************************************************************************************************************
 * Fecha Modificación	: 02/07/2015
 * Modificado por		: Robert Castillo
 * Nro de OT			: 7370 - PSC001
 * Descripción del cambio: Se crea el método CalcularRescatesSignificativos
 * */
using System;
using System.Data;
//using Procesos.TD;
using SistemaProcesosDA;
using System.Data.SqlClient;


namespace SistemaProcesosBL
{
    public class RescatesSignificativosBO
    {
        public RescatesSignificativosBO() { }

        public DataTable ObtenerRescatesSignificativos(DateTime fechaProceso, string codigoPadre)
        {
            RescatesSignificativosDA da = new RescatesSignificativosDA();
            return da.ObtenerRescatesSignificativos(fechaProceso, codigoPadre);
        }

        public DataTable ObtenerTablaGeneral(string codigoTabla, string llaveTabla)
        {
            RescatesSignificativosDA da = new RescatesSignificativosDA();
            return da.ObtenerTablaGeneral(codigoTabla, llaveTabla);
        }

		// 7370 - PSC001
        public void CalcularRescatesSignificativos(DateTime fechaProceso, string codigoPadre)
        {
            RescatesSignificativosDA da = new RescatesSignificativosDA();
            da.CalcularRescatesSignificativos(fechaProceso, codigoPadre);
        }
		// 7370 - PSC001

    }
}

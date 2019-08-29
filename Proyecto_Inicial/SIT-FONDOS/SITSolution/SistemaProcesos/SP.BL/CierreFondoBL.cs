/*
* Fecha de Modificación		: 04/10/2017
* Modificado por			: Rosmery Contreras
* Numero de OT				: 10808
* Descripción del cambio	: Creacion.
* */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Procesos.DA;
using System.Data;

namespace Procesos.BL
{
    public class CierreFondoBL
    {
        CierreFondoDA cierreFondoDA = new CierreFondoDA();

        public DataSet ObtenerFondos_TipoFondos()
        {
            DataSet result = new DataSet();

            //Se obtiene todos los fondos
            CierreFondoDA cierreFondoDA = new CierreFondoDA();
            DataTable dtFondos = cierreFondoDA.ObtenerFondos();

            result.Tables.Add(dtFondos);

            //Se obtiene los tipos fondos
            cierreFondoDA.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            cierreFondoDA.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;
            DataTable dtTipoFondo = cierreFondoDA.ObtenerTipoFondo("TIPFON");

            result.Tables.Add(dtTipoFondo);

            return result;
        }

		public DataSet ObtenerFondoCierre()
		{
			DataSet result = new DataSet();
			//Se obtiene los tipos fondos
			cierreFondoDA.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
			cierreFondoDA.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;
			DataTable dtTipoFondo = cierreFondoDA.ObtenerTipoFondo("TIPOPECIERRE");

			result.Tables.Add(dtTipoFondo);
			return result;
		}

        public DataTable MontoSolicitudRescatePreliminar(DateTime fecha, int idFondo)
        {
            CierreFondoDA da = new CierreFondoDA();
			return da.MontoSolicitudRescatePreliminar(fecha, idFondo);
        }
	}
}

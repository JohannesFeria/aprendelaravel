/*
 * Fecha de Modificaci�n: 31/07/2012
 * Modificado por: Cesar Arasaki
 * Numero de OT: 4792
 * Descripci�n del cambio: Creaci�n
 * */
/*
 * Fecha de Modificaci�n: 28/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5117
 * Descripci�n del cambio: Se cambia namespace
 * */
using System;
using System.Data;
using System.Security.Principal;

using SistemaProcesosDA;
using SistemaProcesosTD;

namespace SistemaProcesosBL
{
	public class CierreBO
	{
		private string codigoUsuario;
		public CierreBO(string codigoUsuario) 
		{
			this.codigoUsuario = codigoUsuario;
		}

		public bool EjecutarCierre(int idFondo, DateTime fecha)
		{
			CierreDA cierreDA = new CierreDA();
			DateTime serverDate = cierreDA.GetServerDate();
			
			int idPrecierre = cierreDA.ObtenerIdPrecierreNoCerrado(idFondo, fecha);
			if (idPrecierre == -1) return false;

			CierreTD.CierreRow drCierre = new CierreTD().Cierre.NewCierreRow();
			drCierre.ID_PRECIERRE = idPrecierre;
			drCierre.USUARIO = codigoUsuario;
			drCierre.FECHA_PROCESO = serverDate;

			cierreDA.InsertarCierre(drCierre);

			return true;
		}

		public bool VerificarFechaCerrada(decimal idFondo, DateTime fechaOperacion)
		{
			return new CierreDA().VerificarFechaCerrada(idFondo, fechaOperacion);
		}
	}
}

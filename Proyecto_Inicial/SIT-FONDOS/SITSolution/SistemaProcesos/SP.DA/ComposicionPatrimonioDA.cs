/*
 * Fecha Modificaci�n:		19/09/2012
 * Modificado por:			Robert Castillo
 * Nro de OT:				4959
 * Descripci�n del cambio:	Se crea la clase DA para generar el archivo de Composici�n Patrimonial.
* */
/*
 * Fecha de Modificaci�n: 28/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5117
 * Descripci�n del cambio: Se cambia namespace
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
	/// <summary>
	/// Descripci�n breve de ValorCuotaDA.
	/// </summary>
	public class ComposicionPatrimonioDA: INGFondos.Data.DA
	{
		public ComposicionPatrimonioDA(): base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) {}
	
		public DataTable ObtenerProporciones( DateTime fecha, string codigoPadre)
		{
			try
			{
				SqlConnection cn = GetConnection();
				SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_PROPORCION_FONDOS_PATRIMONIO", cn);
				cmd.CommandType = CommandType.StoredProcedure;

				SqlParameter prmFecha = cmd.Parameters.Add("@fecha", DbType.DateTime);
				prmFecha.Value = fecha;

				SqlParameter prmCodigoPadre = cmd.Parameters.Add("@codigoFondoPadre", DbType.String);
				prmCodigoPadre.Value = codigoPadre;

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable("PROPORCIONES");
				da.Fill(dt);
				return dt;
			}
			catch
			{ 
				return null;
			}
		}
	}
}
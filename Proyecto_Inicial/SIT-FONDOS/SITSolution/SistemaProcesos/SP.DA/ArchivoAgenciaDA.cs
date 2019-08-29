/*
 * Fecha Modificación:		19/11/2013
 * Modificado por:			Giovana Veliz
 * Nro de OT:				5908
 * Descripción del cambio:	Se crea la clase DA para generar el archivo de posiciones.
* */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
	/// <summary>
    /// Descripción breve de ArchivoAgenciaDA
	/// </summary>
	public class ArchivoAgenciaDA: INGFondos.Data.DA
	{
        public ArchivoAgenciaDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }
	
        /// <summary>
        /// Método para obtener los registros de saldos de promotores, enviando como
        /// parámetro el codigo de agencia y la fecha de corte.
        /// </summary>
        /// <param name="codAgencia">Código de Agencia</param>
        /// <param name="fecha">Fecha de corte del reporte</param>
        /// <returns>Listado de registro de saldos de promotores</returns>
		public DataTable ObtenerPosicionesAgencia(string codAgencia, DateTime fecha)
		{
			try
			{
				SqlConnection cn = GetConnection();
                SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_OBTENER_POSICIONES_AGENCIA", cn);
				cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmCodAgencia = cmd.Parameters.Add("@Cod_Agencia", DbType.String);
                prmCodAgencia.Value = codAgencia;

                SqlParameter prmFecha = cmd.Parameters.Add("@Fecha", DbType.DateTime);
				prmFecha.Value = fecha;
				
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable("Posiciones");
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
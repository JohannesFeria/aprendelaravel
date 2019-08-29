/*
----------------------------------------------------------
* Fecha de Creación	: 14/01/2013
* Modificado por	: Michell Cornejo
* Numero de OT		: 5187
* Descripción		: Creación.
----------------------------------------------------------
*/

using System;
using System.Data;
using System.Data.SqlClient;
using INGFondos.Data;

namespace Procesos.DA
{
	/// <summary>
	/// Descripción breve de MigracionCarteraDA.
	/// </summary>
	public class MigracionCarteraDA : INGFondos.Data.DA
	{
		public MigracionCarteraDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones)
		{
		}

		public SqlConnection getConexion()
		{
			return this.GetConnection();
		}

		public void ActualizarAsignacionPromotor(object[] migracion,string codigoUsuario, SqlConnection cn)
		{ 
			SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_ASIGNACION_PROMOTOR", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmID_PARTICIPE = cmd.Parameters.Add("@ID_PARTICIPE", SqlDbType.Decimal);
			prmID_PARTICIPE.Value = Convert.ToDecimal(Convert.ToDecimal(migracion[0].ToString())-44670000);

			SqlParameter prmCODIGO_PROMOTOR = cmd.Parameters.Add("@CODIGO_PROMOTOR", SqlDbType.VarChar);
			prmCODIGO_PROMOTOR.Value =  migracion[3].ToString();

			SqlParameter prmFECHA_INICIO_ASIGNACION = cmd.Parameters.Add("@FECHA_INICIO_ASIGNACION", SqlDbType.DateTime);
			prmFECHA_INICIO_ASIGNACION.Value = Convert.ToDateTime(migracion[6]);
					
			SqlParameter prmFECHA_FIN_ASIGNACION = cmd.Parameters.Add("@FECHA_FIN_ASIGNACION", SqlDbType.DateTime);
			prmFECHA_FIN_ASIGNACION.Value = Convert.ToDateTime(migracion[6]).AddDays(-1);;
					
			SqlParameter prmPROPOSITO = cmd.Parameters.Add("@PROPOSITO", SqlDbType.VarChar);
			prmPROPOSITO.Value = migracion[4]==null?"":migracion[4].ToString();

			SqlParameter prmUSUARIO = cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar);
			prmUSUARIO.Value = codigoUsuario;

			SqlParameter prmTIPO_COMISION = cmd.Parameters.Add("@TIPO_COMISION", SqlDbType.VarChar);
			prmTIPO_COMISION.Value = migracion[5]==null?"":migracion[5].ToString();

			try
			{
				cmd.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				throw e;
			}
			finally
			{
				cmd.Dispose();
			}
		}
		public void ActualizarProspecto (object[] migracion,string codigoUsuario, SqlConnection cn)
		{
			SqlCommand cmd = new SqlCommand("WMProsp.WMPR_ACT_PROSPECTO_DESDE_ADMCUENTAS", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmID_PARTICIPE = cmd.Parameters.Add("@ID_PARTICIPE", SqlDbType.Decimal);
			prmID_PARTICIPE.Value = Convert.ToDecimal(Convert.ToDecimal(migracion[0].ToString())-44670000);

			SqlParameter prmCODIGO_PROMOTOR = cmd.Parameters.Add("@CODIGO_PROMOTOR", SqlDbType.VarChar);
			prmCODIGO_PROMOTOR.Value =  migracion[3].ToString();

			SqlParameter prmID = cmd.Parameters.Add("@ID", SqlDbType.Decimal);
			prmID.Value = 0;

			try
			{
				cmd.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				throw e;
			}
			finally
			{
				cmd.Dispose();
			}
		}
	}
}

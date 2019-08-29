/*
 * Fecha de Modificación: 31/07/2012
 * Modificado por: Cesar Arasaki
 * Numero de OT: 4792
 * Descripción del cambio: Creación
 * */
/*
 * Fecha de Modificación: 28/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5117
 * Descripción del cambio: Se cambia namespace
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using SistemaProcesosTD;
using SistemaProcesosTD.Constantes;

namespace SistemaProcesosDA
{
    public class CierreDA : INGFondos.Data.DA // : ConSQL
	{
		public CierreDA() : base(INGFondos.Constants.Conexiones.ServidorBancos, INGFondos.Constants.Conexiones.BaseDeDatosBancos) {}


		public void InsertarCierre(CierreTD.CierreRow drCierre)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_INS_CIERRE", cn);
			cmd.CommandType = CommandType.StoredProcedure;		
			
			SqlParameter prmIdPrecierre = cmd.Parameters.Add("@idPrecierre", SqlDbType.Int);
			prmIdPrecierre.Value = drCierre.ID_PRECIERRE;

			SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.Int);
			prmUsuario.Value = drCierre.USUARIO;

			SqlParameter prmIdCierre = cmd.Parameters.Add("@idCierre", SqlDbType.Int);
			prmIdCierre.Direction = ParameterDirection.Output;

			cn.Open();
			SqlTransaction trans = cn.BeginTransaction();
			cmd.Transaction = trans;
			try
			{
				cmd.ExecuteNonQuery();
				drCierre.ID = Convert.ToInt32(prmIdCierre.Value);
                //// RecordLog(cn, trans, new CierreTD().Cierre, drCierre, INGFondos.Constants.Log.INSERT_CODE, Tablas.ID_TABLA_CIERRE, drCierre.ID);
				trans.Commit();
			}
			catch(Exception e)
			{
				trans.Rollback();
				throw e;
			}
			finally
			{
				cn.Close();
			}
		}

		public int ObtenerIdPrecierreNoCerrado(decimal idFondo, DateTime fecha)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_ID_PRECIERRE_NO_CERRADO", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmFondo.Value = idFondo;

			SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
			prmFecha.Value = fecha;

			object idPrecierre;
			try
			{
				cn.Open();
				idPrecierre = cmd.ExecuteScalar();	
			}
			finally
			{
				cn.Close();
			}
			return idPrecierre == null ? -1 : Convert.ToInt32(idPrecierre);
		}

		public bool VerificarFechaCerrada(decimal idFondo, DateTime fechaOperacion)
		{
			SqlConnection cn = GetConnection();
			string sql = string.Format("SELECT dbo.INGF_VER_EXISTE_CIERRE({0},'{1}')", idFondo, fechaOperacion.ToString("yyyy-MM-dd HH:mm:ss"));
			SqlCommand cmd = new SqlCommand(sql, cn);
			cmd.CommandType = CommandType.Text;

			int cerrada;
			try
			{
				cn.Open();
				cerrada = Convert.ToInt32(cmd.ExecuteScalar());
			}
			finally
			{
				cn.Close();
			}
			return cerrada == 1;
		}
	}
}

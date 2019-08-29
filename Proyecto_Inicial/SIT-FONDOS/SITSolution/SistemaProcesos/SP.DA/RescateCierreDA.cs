/*
* Fecha de Modificación		: 04/10/2017
* Modificado por			: Rosmery Contreras
* Numero de OT				: 10808
* Descripción del cambio	: Creacion.
* */

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using Procesos.Constants;
using System.Text;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
    public class RescateCierreDA : INGFondos.Data.DA
    {
        public RescateCierreDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        public SqlConnection GetConnection2()
        {
            return base.GetConnection();
        }

        public void GrabarCierrexFondo(RescateCierre rescateCierre)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_CIERRE", cn);
			cn.Open();
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@TIPO_PROCESO", SqlDbType.VarChar).Value = rescateCierre.TipoProceso;
                cmd.Parameters.Add("@FECHA_PROCESO", SqlDbType.DateTime).Value = rescateCierre.FechaProceso;
                cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int).Value = rescateCierre.IdFondo;
                cmd.Parameters.Add("@VALOR1", SqlDbType.Decimal).Value = rescateCierre.Var1;
                cmd.Parameters.Add("@VALOR2", SqlDbType.Decimal).Value = rescateCierre.Var2;
                cmd.Parameters.Add("@USUARIO_CREACION", SqlDbType.VarChar).Value = rescateCierre.UsuarioCreacion;
                cmd.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = rescateCierre.UsuarioModificacion;
				cmd.Parameters.Add("@AREA_MODIFICACION", SqlDbType.VarChar).Value = rescateCierre.AreaModificacion;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
				cn.Close();
            } 
        }

		public DataTable ObtenerPrecierre_Fecha(int idFondo, DateTime fecha)
		{
			SqlConnection cn = GetConnection();
			DataTable dt = new DataTable();
			cn.Open();
			try
			{
				SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_PRECIERRE_FECHA", cn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@idFondo", SqlDbType.Int).Value = idFondo;
				cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				cn.Close();
			}

			return dt;
		}

		public DataTable ListarCierre_Fecha(string fechaRegistro,int idFondo)
		{
			SqlConnection cn = GetConnection();
			DataTable dt = new DataTable();
			cn.Open();
			try
			{	SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_CIERRE", cn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@FECHA_REGISTRO", SqlDbType.VarChar).Value = fechaRegistro;
				cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int).Value = idFondo;

				SqlDataAdapter da = new SqlDataAdapter(cmd);				
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				cn.Close();
			}
			
			return dt;
		}		
	}
}

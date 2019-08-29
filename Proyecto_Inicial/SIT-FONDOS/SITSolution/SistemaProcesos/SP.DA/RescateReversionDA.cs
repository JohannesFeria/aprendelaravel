/*
* Fecha de Modificación		: 04/10/2017
* Modificado por			: Rosmery Contreras
* Numero de OT				: 10808
* Descripción del cambio	: Creacion.
* */

using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
    public class RescateReversionDA : INGFondos.Data.DA
    {
        public RescateReversionDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

		public void reversionRescatePreliminar(int idFondo, string usuario)
		{
			SqlConnection cn = GetConnection();
			cn.Open();
			SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_CIERRE", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmIdFondo = cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int);
			prmIdFondo.Value = idFondo;

			SqlParameter prmUsuario = cmd.Parameters.Add("@CODIGO_USUARIO", SqlDbType.VarChar, 20);
			prmUsuario.Value = usuario;

			SqlParameter prmArea = cmd.Parameters.Add("@AREA_MODIFICACION", SqlDbType.VarChar, 20);
			prmArea.Value = Constants.ConstantesING.AREA_MODIFICACION_CIERRE_RESCATE;

			cmd.ExecuteNonQuery();
			cn.Close();
		}

		public DataTable validarPrecierreEjecutadoUltimaFechaCierre(int idFondo)
		{
			SqlConnection cn = GetConnection();
			cn.Open();
			SqlCommand cmd = new SqlCommand("dbo.FMPR_VAL_PRECIERRE_EJECUTADO_ULTIMA_FECHA_CIERRE", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmIdFondo = cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int);
			prmIdFondo.Value = idFondo;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("VAL_PRECIERRE");
			da.Fill(dt);
			cn.Close();
			return dt;
		}

		public DataTable listarFondosReversionCierre()
		{
			SqlConnection cn = GetConnection();
			cn.Open();
			SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_FONDO_REVERSION_CIERRE", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("FONDO_REVERSION");
			da.Fill(dt);
			cn.Close();
			return dt;
		}

	}
}

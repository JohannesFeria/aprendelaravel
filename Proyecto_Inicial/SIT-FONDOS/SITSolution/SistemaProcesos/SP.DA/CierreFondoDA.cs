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

namespace Procesos.DA
{
    public class CierreFondoDA : INGFondos.Data.DA
    {
        public CierreFondoDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        public SqlConnection GetConnection2()
        {
            return base.GetConnection();
        }
        
        //Se obtiene todos los fondos
        public DataTable ObtenerFondos()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_FONDO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 4000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("FONDOS");
            da.Fill(dt);
            return dt;
        }

        //Se obtiene los tipos fondos
        public DataTable ObtenerTipoFondo(string tipoFondo)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_TABLA_GENERAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 4000;

            SqlParameter prmCod = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
            prmCod.Value = tipoFondo;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("TablaGeneral");
            da.Fill(dt);
            return dt;

        }

		public DataTable MontoSolicitudRescatePreliminar(DateTime fecha, int idFondo)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_MONTOS_SOLICITUD_RESCATE_PRELIMINAR", cn);
			cmd.CommandType = CommandType.StoredProcedure;
			SqlParameter prmCod = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmCod.Value = idFondo;
			SqlParameter prmFec = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
			prmFec.Value = fecha;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("CIERRE");
			da.Fill(dt);
			return dt;
		}
	}
}

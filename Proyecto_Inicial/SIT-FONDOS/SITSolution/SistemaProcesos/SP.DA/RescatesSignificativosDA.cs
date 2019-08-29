/*
 * Fecha Modificación	: 15/06/2015
 * Modificado por		: Robert Castillo
 * Nro de OT			: 7370
 * Descripción del cambio: Se crea la clase que se encarga del acceso a datos para los rescates significativos.
 * ********************************************************************************************************************
 * Fecha Modificación	: 02/07/2015
 * Modificado por		: Robert Castillo
 * Nro de OT			: 7370 - PSC001
 * Descripción del cambio: Se crea el método CalcularRescatesSignificativos
 *                         Se elimina el método ObtenerRescatesSignificativos
 * */
 
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace SistemaProcesosDA
{
    public class RescatesSignificativosDA : INGFondos.Data.DA // : ConSQL
    {
        public RescatesSignificativosDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        //public DataSet ObtenerRescatesSignificativos(DateTime fechaProceso, decimal idFondo)
        /* 7370 - PSC001
        public DataSet ObtenerRescatesSignificativos(DateTime fechaProceso, string codigoPadre)
        {
            DataSet dt = new DataSet();	

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_RESCATES_SIGNIFICATIVOS", cn);
            cmd.CommandTimeout = 10000;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmFechaProceso = cmd.Parameters.Add("@FECHA_PROCESO", SqlDbType.DateTime);
            prmFechaProceso.Value = fechaProceso;

            //SqlParameter prmIdFondo = cmd.Parameters.Add("@ID_FONDO", SqlDbType.Decimal);
            //prmIdFondo.Value = idFondo;

            SqlParameter prmCodigoPadre = cmd.Parameters.Add("@CODIGO_PADRE", SqlDbType.VarChar);
            prmCodigoPadre.Value = codigoPadre;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }
        */

        public DataTable ObtenerTablaGeneral(string codigoTabla, string llaveTabla)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_TABLA_GENERAL", cn);
            cmd.CommandTimeout = 10000;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmCodigoTabla = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
            prmCodigoTabla.Value = codigoTabla;

            SqlParameter prmLlaveTabla = cmd.Parameters.Add("@llaveTabla", SqlDbType.VarChar);
            prmLlaveTabla.Value = llaveTabla;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("TABLA_GENERAL");
            da.Fill(dt);
            return dt;
        }
		
		// 7370 - PSC001
        public void CalcularRescatesSignificativos(DateTime fechaProceso, string codigoPadre)
        {
            SqlConnection cn = GetConnection();
            SqlTransaction tr = null;

            try
            {
                cn.Open();
                tr = cn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("dbo.FMPR_CAL_RESCATE_SIGNIFICATIVO", cn, tr);
                cmd.CommandTimeout = 10000;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmFechaProceso = cmd.Parameters.Add("@FECHA_PROCESO", SqlDbType.DateTime);
                prmFechaProceso.Value = fechaProceso;

                SqlParameter prmCodigoPadre = cmd.Parameters.Add("@CODIGO_PADRE", SqlDbType.VarChar);
                prmCodigoPadre.Value = codigoPadre;

                cmd.ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                throw ex;
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable ObtenerRescatesSignificativos(DateTime fechaProceso, string codigoPadre)
        {
            //DataSet dt = new DataSet();

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_RESCATE_SIGNIFICATIVO", cn);
            cmd.CommandTimeout = 10000;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmFechaProceso = cmd.Parameters.Add("@FECHA_PROCESO", SqlDbType.DateTime);
            prmFechaProceso.Value = fechaProceso;

            SqlParameter prmCodigoPadre = cmd.Parameters.Add("@CODIGO_PADRE", SqlDbType.VarChar);
            prmCodigoPadre.Value = codigoPadre;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("RESCATE_SIGNIFICATIVO");
            da.Fill(dt);

            return dt;
        }
		// 7370 - PSC001
    }
}

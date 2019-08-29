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
/*
 * Fecha de Modificación : 20/11/2017
 * Modificado por        : Anthony Joaquin
 * Nro. Orden de Trabajo : 10928
 * Descripción del cambio: Se agrega el método verificarTributacion.
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
// using SistemaProcesosTD;

namespace SistemaProcesosDA
{
	/// <summary>
	/// Descripción breve de Class1.
	/// </summary>
    public class FondoDA : INGFondos.Data.DA // : ConSQL
	{
		public FondoDA(): base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) {}

        /// <summary>
        /// Lista elementos de la tabla TABLA_GENERAL
        /// </summary>
        /// <param name="codigoTabla">Indica el código de tabla lógica</param>
        /// <returns>DataTable: Listado de los registros de la tabla TABLA_GENERAL con CODIGO_TABLA determinado</returns>
        public DataTable ObtenerLista(string codigoTabla)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_TABLA_GENERAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmCodigo = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar, 20);
            prmCodigo.Value = codigoTabla;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("LISTA");
            da.Fill(dt);
            return dt;
        }

		public DataTable ObtenerFondosPrecierre()
		{
			try
			{
				SqlConnection cn = GetConnection();
				SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_PRECIERRE_FONDOS", cn);
				cmd.CommandType = CommandType.StoredProcedure;

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable("FONDO");
				da.Fill(dt);
				return dt;
			}
			catch
			{ 
				return null;
			}
		}

        public String obtenerParametroXCodigo(String codigo)
        {
            String parametro = null;

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_PARAMETRO_SISTEMA", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmNombre = cmd.Parameters.Add("@nombre", SqlDbType.VarChar);
            prmNombre.Value = codigo;

            try
            {
                cn.Open();
                DataTable dtParametro = new DataTable("PARAMETRO");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtParametro);

                if (dtParametro.Rows.Count > 0)
                    parametro = dtParametro.Rows[0]["VALOR"].ToString();
            }
            finally
            {
                cn.Close();
            }

            return parametro;
        }

        public bool verificarTributacionEjecutada(int idFondo, DateTime fecha)
        {
            bool result = false;

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMTR_OBT_LOG_DIARIO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Int);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@fechaProceso", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            SqlParameter prmTipoProceso = cmd.Parameters.Add("@tipoProceso", SqlDbType.VarChar);
            prmTipoProceso.Value = "D";

            try
            {
                cn.Open();
                DataTable dtParametro = new DataTable("LOG_DIARIO");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtParametro);

                result = (dtParametro.Rows.Count > 0);
            }
            finally
            {
                cn.Close();
            }

            return result;
        }


        public int ObtenerCantidadOperacionesTipoFlu(int idFondo, DateTime fecha)
        {
            int pagaCupon = 0;
            SqlConnection cn = GetConnection();
            cn.Open();
            //SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_PAGA_CUPON_X_FONDO", cn); INGF_OBT_CANT_OPERACIONES_FLU_X_FONDO
            SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_CANT_OPERACIONES_FLU_X_FONDO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Int);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            try
            {
                /// cn.Open();
                pagaCupon = Convert.ToInt32(cmd.ExecuteScalar());


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
            return pagaCupon;
        }


        //OT10928 INI
        public bool verificarTributacion(int idFondo, DateTime fecha)
        {
            bool result = false;

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMTR_OBT_LOG_DIARIO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Int);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@fechaProceso", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            SqlParameter prmTipoProceso = cmd.Parameters.Add("@tipoProceso", SqlDbType.VarChar);
            prmTipoProceso.Value = "F";

            try
            {
                cn.Open();
                DataTable dtParametros = new DataTable("LOG_DIARIOS");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtParametros);

                result = (dtParametros.Rows.Count > 0);
            }
            finally
            {
                cn.Close();
            }

            return result;
        }
        //OT10928 INI

	}
}

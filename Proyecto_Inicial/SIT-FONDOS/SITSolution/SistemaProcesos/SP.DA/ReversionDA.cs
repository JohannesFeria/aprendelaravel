/*
 * Fecha de Modificación: 27/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5117
 * Descripción del cambio: Creación
 * */
/*
 * Fecha Modificación	: 02/05/2013
 * Modificado por		: Davis Rixi
 * Nro de OT			: 5007
 * Descripción del cambio: Se crean métodos EjecutarReversionExcesos
 * ********************************************************************************************************************
 * Fecha Modificación	: 02/07/2015
 * Modificado por		: Robert Castillo
 * Nro de OT			: 7370 - PSC001
 * Descripción del cambio: Se crea el método EjecutarReversionRescatesSignificativos
 * */
/*
 * Fecha de Modificación    : 14/01/2016
 * Modificado por           : Richard Valdez
 * Numero de OT             : 7968
 * Descripción del cambio   : Agregar el método EjecutarReversionDepositos que llama al procedure dbo.FMPR_EJE_REVERSION_PRECIERRE
 * */
/*
 * Fecha de Modificación    : 10/02/2016
 * Modificado por           : Richard Valdez
 * Numero de OT             : 8292
 * Descripción del cambio   : - Método RevertirPrecierre: agregar un parámetro de salida que almacena el identificador del precierre 
 *                              que se está revertiendo.
 *                            - Método RevertirPrecierre: retornar el valor del parámetro de salida que se ha creado en el paso anterior.  
 *                            - Método EjecutarReversionDepositos: agregar un parámetro de entrada para enviar el identificador del 
 *                              precierre que se desea revertir. El parámetro será enviado al llamar al procedure 
 *                              dbo.FMPR_EJE_REVERSION_PRECIERRE.PRC (Depósitos).
 * */
/*
 * Fecha Modificación	 : 21/03/2017
 * Modificado por		 : Héctor Mendoza Rosales
 * Nro. Orden de Trabajo : 10110
 * Descripción del cambio: Se modifica el método RevertirPrecierre() donde se agrega y se usa el parámetro de motivo de la Reversión.
 * */
/*
 * Fecha de Modificación : 12/05/2017
 * Modificado por        : Anthony Joaquin
 * Nro. Orden de Trabajo : 10217 -PSC001
 * Descripción del cambio: Homologación.
 * */
/*
 * Fecha de Modificación : 24/05/2018
 * Modificado por        : Walter Albites
 * Nro. Orden de Trabajo : OT11264 PSC002
 * Descripción del cambio: Se agrega el método ReversionAlertaActividadCliente para el registro de Alertas.
 * */
/*
 * Fecha de Modificación : 26/09/2018
 * Modificado por        : Robert Castillo
 * Nro. Orden de Trabajo : OT11527
 * Descripción del cambio: Se modifica método RevertirPrecierre.
 * */
/*
 * Fecha de Modificación : 25/07/2019
 * Modificado por        : Robert Castillo
 * Nro. Orden de Trabajo : OT11777
 * Descripción del cambio: Se agrega el método ObtenerSolicitudReversionAprobada.
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using SistemaProcesosTD;

namespace SistemaProcesosDA
{
	/// <summary>
	/// Clase de acceso a datos para el proceso de reversión de precierre.
	/// </summary>
	public class ReversionDA : INGFondos.Data.DA
	{
		public ReversionDA(): base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) {}

		public SqlConnection GetConnection2()
		{	
			return base.GetConnection();
		}

		/*INI 10110*/
		//public decimal RevertirPrecierre(SqlConnection cn, SqlTransaction trans, int idFondo, DateTime fecha, string usuario)
		public decimal RevertirPrecierre(SqlConnection cn, SqlTransaction trans, int idFondo, DateTime fecha, string usuario, string motivo)
		/*FIN 10110*/
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_EJE_REVERSION_PRECIERRE", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;
			//OT11527 INI
			cmd.CommandTimeout = 10000;
			//OT11527 FIN

			SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
			prmFecha.Value = fecha;

			SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmIdFondo.Value = idFondo;

			SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
			prmUsuario.Value = usuario;

			SqlParameter prmArea = cmd.Parameters.Add("@area", SqlDbType.VarChar);
			prmArea.Value = String.Empty;

			/*INI 10110*/
			SqlParameter prmMotivo = cmd.Parameters.Add("@motivo", SqlDbType.VarChar);
			prmMotivo.Value = motivo;
			/*FIN 10110*/

            //OT 8292 INI

            SqlParameter prmIdPrecierreRevertido = cmd.Parameters.Add("@idPrecierreRevertido", SqlDbType.Decimal);
            prmIdPrecierreRevertido.Direction = ParameterDirection.Output;

            //OT 8292 FIN

			cmd.ExecuteNonQuery();

            //OT 8292 INI

            return Convert.ToDecimal(prmIdPrecierreRevertido.Value);

            //OT 8292 FIN
		}

		public DataTable ObtenerFondos()
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_FONDO", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmArea = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
			prmArea.Value = "ACT";

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("FONDOS");
			da.Fill(dt);
			return dt;
		}

		public string ObtenerFechaReversion(int idFondo)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_PRECIERRE_ULTIMA_FECHA_FONDO", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.VarChar);
			prmIdFondo.Value = idFondo;

			try
			{
				cn.Open();
				object valor = cmd.ExecuteScalar();
				if(valor != null && valor != DBNull.Value)
				{
					return Convert.ToDateTime(valor).ToString("yyyy-MM-dd");
				}
				else
				{
					return "";
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmd.Dispose();
				cn.Close();
			}
		}



		//OT5007 - Ejecución de reversión de excesos
		public void EjecutarReversionExcesos(decimal idFondo,DateTime fecha, String usuario, SqlConnection cn, SqlTransaction trans)
		{	
			DataSet dt = new DataSet();
			SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_EXCESOS_FONDO", cn, trans);
			cmd.CommandTimeout = 10000;
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmIdFondo.Value = idFondo;

			SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
			prmFecha.Value = fecha;

			SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
			prmUsuario.Value = usuario;


			//cn.Open();
			cmd.ExecuteNonQuery();

		}

		// 7370 - PSC001
        public void EjecutarReversionRescatesSignificativos(decimal idFondo, DateTime fecha, String usuario, SqlConnection cn, SqlTransaction trans)
        {
            DataSet dt = new DataSet();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_RESCATE_SIGNIFICATIVO", cn, trans);
            cmd.CommandTimeout = 10000;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
            prmUsuario.Value = usuario;


            //cn.Open();
            cmd.ExecuteNonQuery();

        }
		// 7370 - PSC001

        //OT 7968 INI

        //OT 8292 INI
        //public void EjecutarReversionDepositos(SqlConnection cn, SqlTransaction trans, int idFondo, DateTime fecha, string usuario)        
        public void EjecutarReversionDepositos(SqlConnection cn, SqlTransaction trans, int idFondo, DateTime fecha, 
            decimal idPrecierre, string usuario)
        //OT 8292 FIN
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_EJE_REVERSION_PRECIERRE", cn, trans);
            //cmd.CommandTimeout = 40000;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            //OT 8292 INI

            SqlParameter prmIdPrecierre = cmd.Parameters.Add("@idPrecierre", SqlDbType.Decimal);
            prmIdPrecierre.Value = idPrecierre;

            //OT 8292 FIN

            SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
            prmUsuario.Value = usuario;

            cmd.ExecuteNonQuery();

        }

        //OT 7968 FIN

		//INI OT11264 PSC002
		public void ReversionAlertaActividadCliente(SqlConnection cn, SqlTransaction trans, int idFondo, DateTime fecha, string usuario)
		{
			SqlCommand cmd = new SqlCommand("dbo.FMPR_REVERSION_ALERTA_ACTIVIDAD_CLIENTE", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
			cmd.Parameters.Add("@idFondo", SqlDbType.Decimal).Value = idFondo;
            cmd.Parameters.Add("@titulo", SqlDbType.VarChar).Value = SistemaProcesosTD.Constantes.ConstantesING.TITULO_ALERTA;
			cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;

			cmd.ExecuteNonQuery();

		}
		//FIN OT11264 PSC002

        //OT11777 INI	
        public DataTable ObtenerSolicitudReversionAprobada(int idFondo, int fecha)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_SOLICITUD_REVERSION_APROBADA", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@FECHA", SqlDbType.Int);
            prmFecha.Value = fecha;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("SOLICITUD_REVERSION_APROBADA");
            da.Fill(dt);
            return dt;
        }
        //OT11777 FIN

	}
}

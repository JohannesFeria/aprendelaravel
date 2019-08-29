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
 * Fecha Modificación	: 02/05/2013
 * Modificado por		: Davis Rixi
 * Nro de OT			: 5007
 * Descripción del cambio: Se crean métodos EjecutarRegistroExcesos y ListarAlertasExcesos
 * */
/*
 * Fecha Modificación	: 14/05/2013
 * Modificado por		: Davis Rixi
 * Nro de OT			: 5007
 * Descripción del cambio: Se cambia la llamada al SP FMPR_LIS_ALERTA_EXCESO en el método ListarAlertasExcesos
 * */
/*
 * Fecha Modificación	: 18/06/2013
 * Modificado por		: Robert Castillo
 * Nro de OT			: 5526
 * Descripción del cambio: Se modifica el dato que retorna el método PreviewPrecierre a DataSet.
 * */
/*
 * Fecha Modificación	: 11/11/2013
 * Modificado por		: Robert Castillo
 * Nro de OT			: 5948
 * Descripción del cambio: Se agrega el método VerificarFondoPadrePrecierre.
 * */
/*
 * Fecha Modificación	: 15/06/2015
 * Modificado por		: Robert Castillo
 * Nro de OT			: 7370
 * Descripción del cambio: Se agrega el método ObtenerUltimaFechaPrecierreXFondo.
 * */
/*
 * Fecha Modificación	    : 07/01/2016
 * Modificado por		    : Richard Valdez
 * Nro de OT			    : 7968
 * Descripción del cambio   : Agregar los métodos InsertarDepositoAsociadoTraspasoFondos, ObtenerTraspasoFondosPendienteTipoCambio, 
 *                            ObtenerTablaGeneral, ListarTablaGeneral
 * */

using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Constants;
using INGFondos.Data;
using SistemaProcesosTD;
using SistemaProcesosTD.Constantes;

namespace SistemaProcesosDA
{
	/// <summary>
	/// Descripción breve de PrecierreDA.
	/// </summary>
    public class PrecierreDA : INGFondos.Data.DA // : ConSQL
	{
		public PrecierreDA(): base(INGFondos.Constants.Conexiones.ServidorBancos, INGFondos.Constants.Conexiones.BaseDeDatosBancos) {}

        /// <summary>
        /// Obtiene el detalle del Valor Cuota Cerrado para una fecha específica
        /// </summary>
        /// <param name="idFondo"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public DataRow ObtenerDetalleValorCuotaCerrado(decimal idFondo, DateTime fecha)
        {
            /*CRumiche: Los datos de servidor deben de definirse a este nivel no a nivel de BL*/            
            this.Server = INGFondos.Constants.Conexiones.ServidorOperaciones; 
            this.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;

            using (SqlConnection cn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_PRECIERRE", cn))
                {                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
                    prmFecha.Value = fecha;
                    SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
                    prmIdFondo.Value = idFondo;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0) return dt.Rows[0];

                        return null;                        
                    }
                }           
            }



        }



        private void EliminarPrecierre(SqlConnection cn, SqlTransaction trans, int idPrecierre)
        {
            SqlCommand cmd = new SqlCommand("dbo.INGF_ELI_PRECIERRE", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdPrecierre = cmd.Parameters.Add("@idPrecierre", SqlDbType.Int);
            prmIdPrecierre.Value = idPrecierre;

            cmd.ExecuteNonQuery();
        }

        private int InsertarPrecierre(SqlConnection cn, SqlTransaction trans, PrecierreTD.PrecierreRow drPrecierre)
        {
            SqlCommand cmd = new SqlCommand("dbo.INGF_INS_PRECIERRE", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmIdFondo.Value = drPrecierre.ID_FONDO;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = drPrecierre.FECHA;

            SqlParameter prmHorarioOperacion = cmd.Parameters.Add("@horarioOperacion", SqlDbType.DateTime);
            prmHorarioOperacion.Value = drPrecierre.HORARIO_OPERACION;

            SqlParameter prmValorCuota = cmd.Parameters.Add("@valorCuota", SqlDbType.Decimal);
            prmValorCuota.Value = drPrecierre.VALOR_CUOTA;

            SqlParameter prmFlagEliminado = cmd.Parameters.Add("@flagEliminado", SqlDbType.Char, 1);
            prmFlagEliminado.Value = drPrecierre.FLAG_ELIMINADO;

            SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 20);
            prmUsuario.Value = drPrecierre.USUARIO;

            SqlParameter prmFechaProceso = cmd.Parameters.Add("@fechaProceso", SqlDbType.DateTime);
            prmFechaProceso.Value = drPrecierre.FECHA_PROCESO;

            SqlParameter prmId = cmd.Parameters.Add("@id", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(prmId.Value);
        }

        private SqlCommand ObtenerSqlCommandActualizarDepositoPrecierre(SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.INGF_ACT_DEPOSITO_PRECIERRE", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters.Add("@numeroCuotas", SqlDbType.Decimal);
            cmd.Parameters.Add("@idPrecierre", SqlDbType.Int);
            cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
            return cmd;
        }

        public SqlConnection GetConnection2()
        {
            return base.GetConnection();
        }

        public void EjecutarPrecierre(PrecierreTD.PrecierreRow drPrecierreAnterior, PrecierreTD.PrecierreRow drPrecierre, DepositoTD.DepositoPrecierreDataTable dtDepositosPrecierre, SqlConnection cn, SqlTransaction trans)
        {
            PrecierreTD.PrecierreDataTable precierreMetadata = new PrecierreTD().Precierre;
            if (drPrecierreAnterior != null)
            {
                EliminarPrecierre(cn, trans, drPrecierreAnterior.ID);
                ////RecordLog(cn, trans, precierreMetadata, drPrecierreAnterior, INGFondos.Constants.Log.DELETE_CODE, Tablas.ID_TABLA_PRECIERRE, drPrecierreAnterior.ID);
            }

            drPrecierre.ID = InsertarPrecierre(cn, trans, drPrecierre);
            string usuario = drPrecierre.USUARIO;
            ////RecordLog(cn, trans, precierreMetadata, drPrecierre, INGFondos.Constants.Log.INSERT_CODE, Tablas.ID_TABLA_PRECIERRE, drPrecierre.ID);

            if (dtDepositosPrecierre != null)
            {
                DepositoTD.DepositoPrecierreDataTable depositoMetadata = new DepositoTD().DepositoPrecierre;
                SqlCommand cmdActualizarDepositoPrecierre = ObtenerSqlCommandActualizarDepositoPrecierre(cn, trans);
                foreach (DepositoTD.DepositoPrecierreRow drDeposito in dtDepositosPrecierre.Rows)
                {
                    drDeposito.ID_PRECIERRE = drPrecierre.ID;
                    cmdActualizarDepositoPrecierre.Parameters["@id"].Value = drDeposito.ID;
                    cmdActualizarDepositoPrecierre.Parameters["@numeroCuotas"].Value = drDeposito.NUMERO_CUOTAS;
                    cmdActualizarDepositoPrecierre.Parameters["@idPrecierre"].Value = drDeposito.ID_PRECIERRE;
                    cmdActualizarDepositoPrecierre.Parameters["@usuario"].Value = usuario;
                    cmdActualizarDepositoPrecierre.ExecuteNonQuery();
                    ////RecordLog(cn, trans, depositoMetadata, drDeposito, INGFondos.Constants.Log.UPDATE_CODE, Tablas.ID_TABLA_DEPOSITO, drDeposito.ID);
                }
            }
        }

        //public void ConciliarOperacion(int idOperacion, string nroOperacionBancaria)
        //{
        //    SqlConnection cn = GetConnection();
        //    SqlCommand cmd = new SqlCommand("dbo.INGF_ACT_OPERACION_CONCILIACION", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;
			
        //    SqlParameter prmIdOperacion = cmd.Parameters.Add("@idOperacion", SqlDbType.Int);
        //    prmIdOperacion.Value = idOperacion;

        //    SqlParameter prmNroOperacion = cmd.Parameters.Add("@numeroOperacion", SqlDbType.VarChar, 50);
        //    prmNroOperacion.Value = (nroOperacionBancaria.Trim().Equals(string.Empty)) ? DBNull.Value : (object)nroOperacionBancaria;

        //    try
        //    {
        //        cn.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    finally
        //    {
        //        cn.Close();
        //    }
        //}

        //public DataTable ConsultarPrecierre(int idFondo, DateTime fechaDesde, DateTime fechaHasta)
        //{
        //    SqlConnection cn = GetConnection();
        //    SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_PRECIERRE", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Int);
        //    prmIdFondo.Value = idFondo;

        //    SqlParameter prmFechaDesde = cmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
        //    prmFechaDesde.Value = fechaDesde;

        //    SqlParameter prmFechaHasta = cmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
        //    prmFechaHasta.Value = fechaHasta;
            
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable("PRECIERRES");
        //    da.Fill(dt);
        //    return dt;
        //}

        //public DataTable ObtenerBancos()
        //{
        //    SqlConnection cn = GetConnection();
        //    SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_BANCO", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;
            
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable("BANCOS");
        //    da.Fill(dt);
        //    return dt;
        //}

        //public DataTable ObtenerDepositosxConciliar(int idBanco)
        //{
        //    SqlConnection cn = GetConnection();
        //    SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_DEPOSITOS_POR_CONCILIAR", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    SqlParameter prmIdBanco = cmd.Parameters.Add("@idBanco", SqlDbType.Int);
        //    prmIdBanco.Value = idBanco;

        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable("DEPOSITOS");
        //    da.Fill(dt);
        //    return dt;		
        //}

        public DepositoTD.DepositoPrecierreDataTable ObtenerDepositosPrecierre(decimal idFondo, DateTime fecha, DateTime horarioOperacion)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_DEPOSITOS_PRECIERRE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            SqlParameter prmHorarioOperacion = cmd.Parameters.Add("@horarioOperacion", SqlDbType.DateTime);
            prmHorarioOperacion.Value = horarioOperacion;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DepositoTD ds = new DepositoTD();
            da.Fill(ds.DepositoPrecierre);
            return ds.DepositoPrecierre;
        }

        //public DataTable ObtenerFondos()
        //{
        //    SqlConnection cn = GetConnection();
        //    SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_FONDO", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable("FONDOS");
        //    da.Fill(dt);
        //    return dt;
        //}

        //public DataRow ObtenerOperacionPorConciliar(int idFondo, int participe, double monto, DateTime fecha, string codigoViaPago)
        //{
        //    SqlConnection cn = GetConnection();
        //    SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_OPERACION_POR_CONCILIAR_AUTOMATICO", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Int);
        //    prmIdFondo.Value = idFondo;

        //    SqlParameter prmParticipe = cmd.Parameters.Add("@participe", SqlDbType.Int);
        //    prmParticipe.Value = participe;

        //    SqlParameter prmMonto = cmd.Parameters.Add("@monto", SqlDbType.Float);
        //    prmMonto.Value = monto;

        //    SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
        //    prmFecha.Value = fecha;

        //    SqlParameter prmCodigoViaPago = cmd.Parameters.Add("@codigoViaPago", SqlDbType.VarChar, 20);
        //    prmCodigoViaPago.Value = codigoViaPago;

        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    return (dt.Rows.Count == 0) ? null : dt.Rows[0];
        //}

        //public DataRow ObtenerPrecierre(int idPrecierre)
        //{
        //    SqlConnection cn = GetConnection();
        //    SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_PRECIERRE", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    SqlParameter prmIdPrecierre = cmd.Parameters.Add("@idPrecierre", SqlDbType.Int);
        //    prmIdPrecierre.Value = idPrecierre;

        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    return (dt.Rows.Count == 0) ? null : dt.Rows[0];
        //}

        public PrecierreTD.PrecierreRow ObtenerPrecierre(decimal idFondo, DateTime fecha)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_PRECIERRE_x_FONDO_x_FECHA", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            PrecierreTD ds = new PrecierreTD();
            da.Fill(ds.Precierre);
            if (ds.Precierre.Rows.Count == 0) return null;
            return (PrecierreTD.PrecierreRow)ds.Precierre.Rows[0];
        }

        //public DataTable ObtenerViasPago()
        //{
        //    SqlConnection cn = GetConnection();
        //    SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_VIA_PAGO_SUSCRIPCION", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable("VIAS_PAGO");
        //    da.Fill(dt);
        //    return dt;
        //}
		
        public bool EsFeriado(DateTime fecha)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_VER_FERIADO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            bool feriado;
            try
            {
                cn.Open();
                feriado = cmd.ExecuteScalar().ToString().Equals("S");
            }
            finally
            {
                cn.Close();
            }

            return feriado;
        }

		public DataSet PreviewPrecierre(decimal idFondo,String flagVerificacion , String usuario)
		{	
			DataSet dt = new DataSet();			

			SqlConnection cn = GetConnection();
			try
			{
				SqlCommand cmd = new SqlCommand("dbo.INGF_CAL_PREVIEW_PRECIERRE", cn);
				cmd.CommandType = CommandType.StoredProcedure;

				SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
				prmIdFondo.Value = idFondo;

				SqlParameter prmFlag = cmd.Parameters.Add("@flagVerificacion", SqlDbType.Char);
				prmFlag.Value = flagVerificacion;

				SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
				prmUsuario.Value = usuario;

				SqlDataAdapter da = new SqlDataAdapter(cmd);			
				da.Fill(dt);
			}
			catch(Exception e)
			{
				throw e;
			}
			finally
			{
				cn.Close();
			}
			return dt;
		}
        // Inicio | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018
        public DataSet EjecutarPrecierreOperaciones(decimal comisionSAFM, decimal idFondo, String flagVerificacion, String usuario, SqlConnection cn, SqlTransaction trans)
        {
            DataSet dt = new DataSet();
            SqlCommand cmd = new SqlCommand("dbo.INGF_CAL_PRECIERRE", cn, trans);
            cmd.CommandTimeout = 10000;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFlag = cmd.Parameters.Add("@flagVerificacion", SqlDbType.Char);
            prmFlag.Value = flagVerificacion;

            SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
            prmUsuario.Value = usuario;
          
            SqlParameter prmComision = cmd.Parameters.Add("@comision", SqlDbType.Decimal);
            prmComision.Value = comisionSAFM;
            //--
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
        // Fin | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018

        //OT5007 - Ejecución de registro de excesos
        public void EjecutarRegistroExcesos(string codigoPadre, DateTime fecha, String usuario, SqlConnection cn, SqlTransaction trans)
        {
            DataSet dt = new DataSet();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_EXCESOS_FONDO", cn, trans);
            cmd.CommandTimeout = 10000;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@codigoPadre", SqlDbType.VarChar);
            prmIdFondo.Value = codigoPadre;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
            prmUsuario.Value = usuario;


            //cn.Open();
            cmd.ExecuteNonQuery();

        }

        public DataSet ListarAlertasExcesos(decimal idFondo)
        {
            DataSet ds = new DataSet();
            SqlConnection cn = GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_ALERTA_EXCESO", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
                prmFecha.Value = DateTime.Today;

                SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
                prmIdFondo.Value = idFondo;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataTable VerificarFondoPadrePrecierre(decimal idFondo, DateTime fecha)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_VER_FONDO_PADRE_PRECIERRE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@ID_FONDO", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@FECHA", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("FONDO_PADRE");
            da.Fill(dt);
            return dt;
        }

        //OT 7370 INI
        public DataTable ObtenerUltimaFechaPrecierreXFondo(decimal idFondo)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_ULTIMA_FECHA_PRECIERRE_X_FONDO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@ID_FONDO", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("FECHA_PRECIERRE");
            da.Fill(dt);
            return dt;
        }
        //OT 7370 INI


        //OT 7968 INI
        /// <summary>
        /// Se encarga de llamar al procedure dbo.FMPR_INS_DEPOSITO_ASOCIADO_TRASPASO_FONDOS.PRC
        /// </summary>
        /// <returns></returns>
        public void InsertarDepositoAsociadoTraspasoFondos(decimal idFondo, DateTime fecha, string usuario,
            SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_DEPOSITO_ASOCIADO_TRASPASO_FONDOS", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trans;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 20);
            prmUsuario.Value = usuario;

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Se encarga de llamar al procedure dbo.FMPR_OBT_TRASPASO_FONDOS_PENDIENTE_TIPO_CAMBIO.PRC
        /// </summary>
        /// <param name="idFondo"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public DataTable ObtenerTraspasoFondosPendienteTipoCambio(decimal idFondo, DateTime fecha)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_TRASPASO_FONDOS_PENDIENTE_TIPO_CAMBIO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("TRASPASO_PENDIENTE_TC");
            da.Fill(dt);

            return dt;
        }

        ///// <summary>
        ///// Obtener registros de la tabla general
        ///// </summary>
        ///// <param name="codigoTabla"></param>
        ///// <param name="llaveTabla"></param>
        ///// <returns></returns>
        //public DataTable ObtenerTablaGeneral(string codigoTabla, string llaveTabla)
        //{

        //    SqlConnection cn = GetConnection();
        //    SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_TABLA_GENERAL", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    try
        //    {
        //        SqlParameter prmCodigoTabla = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
        //        prmCodigoTabla.Value = codigoTabla;

        //        SqlParameter prmLlaveTabla = cmd.Parameters.Add("@llaveTabla", SqlDbType.VarChar);
        //        prmLlaveTabla.Value = llaveTabla;

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable("TABLA_GENERAL");
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        cmd.Dispose();
        //    }

        //}

        public DataTable ListarTablaGeneral(string codigoTabla)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_TABLA_GENERAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoTabla = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
                prmCodigoTabla.Value = codigoTabla;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("TABLA");
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        ////OT 7968 FIN

        ////OT8829

       
        ////OT 8829


        public void insertarLogDiario(decimal idFondo, DateTime fecha,
            SqlConnection cn, SqlTransaction trans)
        {
            DataSet dt = new DataSet();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_LOG_DIARIO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trans;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@ID_FONDO", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            /*SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 20);
            prmUsuario.Value = usuario;*/

            cmd.ExecuteNonQuery();
        }

        //OT11264 PSC002
        public void InsertarAlertaActividadCliente(int idParticipe, int idFondo, int idOperacion, string codigoUsuario, DateTime fechaProceso, SqlConnection conn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_INS_ALERTA", conn, trans);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@clase", SqlDbType.VarChar).Value = ConstantesING.CLASE_ALERTA_EVENTO;
            cmd.Parameters.Add("@tipo", SqlDbType.VarChar).Value = ConstantesING.TIPO_ALERTA_OBSERVACION;
            cmd.Parameters.Add("@subtipo", SqlDbType.VarChar).Value = ConstantesING.SUBTIPO_ALERTA_OTROS;
            cmd.Parameters.Add("@titulo", SqlDbType.VarChar).Value = ConstantesING.TITULO_ALERTA;
            cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = ConstantesING.DESCRIPCION_ALERTA;


            cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = DBNull.Value;
            cmd.Parameters.Add("@fechaLimite", SqlDbType.DateTime).Value = fechaProceso;
            cmd.Parameters.Add("@idUsuarioOrigen", SqlDbType.Int).Value = ConstantesING.ID_USUARIO_ORIGEN;
            cmd.Parameters.Add("@idUsuarioDestino", SqlDbType.Int).Value = DBNull.Value;
            cmd.Parameters.Add("@idParticipe", SqlDbType.Int).Value = idParticipe;
            cmd.Parameters.Add("@idFondo", SqlDbType.Int).Value = idFondo;
            cmd.Parameters.Add("@idOperacion", SqlDbType.Int).Value = idOperacion;

            cmd.Parameters.Add("@idPrecierre", SqlDbType.Int).Value = DBNull.Value;
            cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar).Value = codigoUsuario;
            cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar).Value = ConstantesING.AREA_MODIFICACION_ALERTA;
            cmd.Parameters.Add("@subtipoOtro", SqlDbType.VarChar).Value = DBNull.Value;
            cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;


            cmd.ExecuteNonQuery();

        }
        //OT11264 PSC002 FIN

        //OT11264 PSC003 INI
        public DataTable ValidarFondoPrecerradoXFondo(int idFondo, DateTime fecha)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_VAL_FONDO_PRECERRADO_X_FECHA", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmIdFondo = cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int);
                prmIdFondo.Value = idFondo;

                SqlParameter prmFecha = cmd.Parameters.Add("@FECHA", SqlDbType.DateTime);
                prmFecha.Value = fecha;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("FONDO_PRECERRADO_X_FECHA");
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        //OT11264 PSC003 FIN
	}
}
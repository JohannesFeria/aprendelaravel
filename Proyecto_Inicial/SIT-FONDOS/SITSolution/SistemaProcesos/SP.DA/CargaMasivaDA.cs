/*
 * Fecha de Creación: 07/10/2016
 * Creado por: Irene Reyes
 * Numero de OT: 8954
 * Descripción del cambio: Creación.
 * 
 */
/*
 * Fecha de Modificación: 20/10/2016
 * Modificado por: Irene Reyes
 * Numero de OT: 8954 PSC-001
 * Descripción del cambio: Agregar método InsertarTraspasoMasivo().
 * 
 */
/*
 * Fecha de Modificación: 21/03/2017
 * Modificado por: Anthony Joaquin
 * Numero de OT: 10082 PSC001
 * Descripción del cambio: Se agrega el método InsertarFlujo() para que inserte una operación de rescate.
 * 
 */
/*
 * Fecha de Modificación: 28/04/2017
 * Modificado por: Robert Castillo
 * Numero de OT: 10082 PSC002
 * Descripción del cambio: Se modifican los valores de algunos campos en el método InsertarFlujo.
 * 
 */
/*
 * Fecha de Modificación : 26/04/2017
 * Modificado por        : Anthony Joaquin
 * Nro. Orden de Trabajo : 10217
 * Descripción del cambio: Creación de método InsertarConversionCuotasMasivo.
 * */
/*
 * Fecha de Modificación : 07/06/2017
 * Modificado por        : Robert Castillo
 * Nro. Orden de Trabajo : 10433
 * Descripción del cambio: Realizar los siguientes cambios:
			Agregar métodos CorrelativoCuentaParticipacion, InsertarCuentasParticipacionMasivo 
 *		y ActualizarCuentaParticipacion.
 * */
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using INGFondos.Data;
using System.Data.SqlClient;
using System.Data;
using Procesos.TD;
using System.Configuration;
using INGFondos.Constants;


namespace Procesos.DA
{
    public class CargaMasivaDA : INGFondos.Data.DA
    {

        private SqlConnection conn;
        private SqlTransaction trans;

        private  SqlConnection cnDepositos;
        private SqlTransaction transDeposiTos; 

        string servidorDepositos = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorBancos];
        string baseDeDatosDepositos = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosBancos];

        public SqlTransaction Trans
        {
            get { return trans; }
            set { trans = value; }
        }

        public SqlConnection Conn
        {
            get { return conn; }
            set { conn = value; }
        }

        public SqlTransaction TransDepositos
        {
            get { return transDeposiTos; }
            set { transDeposiTos = value; }
        }

        public SqlConnection CnDepositos
        {
            get { return cnDepositos; }
            set { cnDepositos = value; }
        }
        
        #region
        //AdmCuentas
        public void CrearConexion()
        {
            conn = GetConnection();

        }

        public void AbrirConexion()
        {
            conn.Open();
        }

        public void AbrirTransaccion()
        {
            trans = conn.BeginTransaction();
        }

        public void CommitTransaccion()
        {
            trans.Commit();
        }

        public void RollbackTransaccion()
        {
            trans.Rollback();
        }

        public void LiberarConexion()
        {
            trans.Dispose();
            conn.Close();
            conn.Dispose();
        }
        #endregion

        #region
        //Depositos
        public void CrearConexionDeposito()
        {
            
             cnDepositos = new SqlConnection();
             cnDepositos.ConnectionString = @"data source=" + servidorDepositos + ";initial catalog="
               + baseDeDatosDepositos + ";integrated security=SSPI";
             
        }

        public void AbrirConexionDeposito()
        {
            cnDepositos.Open();
        }

        public void AbrirTransaccionDeposito()
        {
            transDeposiTos = cnDepositos.BeginTransaction();
        }

        public void CommitTransaccionDeposito()
        {
            transDeposiTos.Commit();
        }

        public void RollbackTransaccionDeposito()
        {
            transDeposiTos.Rollback();
        }

        public void LiberarConexionDeposito()
        {
            transDeposiTos.Dispose();
            cnDepositos.Close();
            cnDepositos.Dispose();
        }
        #endregion


        public CargaMasivaDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }


        #region
        /// <summary>
        /// Listar tipos de Operación
        /// </summary>
        public DataTable ListarTablaGeneral(string codigoTabla)
        {
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_TABLA_GENERAL", con);//trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmCodigoTabla = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
            prmCodigoTabla.Value = codigoTabla;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
           
        }
        #endregion

        #region
        /// <summary>
        /// Listar tipos de Operación
        /// </summary>
        public DataTable ListarLotes()
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_LOTES_MASIVOS", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        #endregion

        #region
        /// <summary>
        /// Listar fondos
        /// </summary>
        public DataTable ListarFondos()
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_BUSCAR_FONDO", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmCodigo = cmd.Parameters.Add("@codigo", SqlDbType.VarChar);
            prmCodigo.Value = DBNull.Value;
            SqlParameter prmNombreComercial = cmd.Parameters.Add("@nombreComercial", SqlDbType.VarChar);
            prmNombreComercial.Value = DBNull.Value;
            SqlParameter prmtipoFondo = cmd.Parameters.Add("@tipoFondo", SqlDbType.VarChar);
            prmtipoFondo.Value = DBNull.Value;
            SqlParameter prmTipoParticipe = cmd.Parameters.Add("@tipoParticipe", SqlDbType.VarChar);
            prmTipoParticipe.Value = DBNull.Value;
            SqlParameter prmMoneda = cmd.Parameters.Add("@moneda", SqlDbType.VarChar);
            prmMoneda.Value = DBNull.Value;
            SqlParameter prmEstado = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
            prmEstado.Value = DBNull.Value;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        #endregion

        #region
        /// <summary>
        /// Listar cuenta participación
        /// </summary>
        public DataTable ListarCuentasParticipacion(int idParticipe)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_CUENTA_PARTICIPACION", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdParticipe = cmd.Parameters.Add("@IdParticipe", SqlDbType.Decimal);
            prmIdParticipe.Value = idParticipe;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        #endregion

        #region
        /// <summary>
        /// Obtener fondos
        /// </summary>
        public DataTable ObtenerFondo(int id)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_FONDO_XID", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmId = cmd.Parameters.Add("@id", SqlDbType.Decimal);
            prmId.Value = id;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        #endregion

        #region
        /// <summary>
        /// Listar fechas feriado
        /// </summary>
        public DataTable ListarFechasFeriados(DateTime fecha)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_FERIADO", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.VarChar);
            prmFecha.Value = fecha.ToShortDateString();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        #endregion

        #region
        /// <summary>
        /// Insertar Traspaso Fondos
        /// </summary>
        public int InsertarTraspasoFondo(OperacionMasivo traspaso)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_INS_TRASPASO_FONDOS", conn, trans);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ID_PARTICIPE_ORIGEN", SqlDbType.Decimal).Value = traspaso.IdParticipeOrigen;
            cmd.Parameters.Add("@ID_PARTICIPE_DESTINO", SqlDbType.Decimal).Value = traspaso.IdParticipeDestino;
            cmd.Parameters.Add("@ID_FONDO_ORIGEN", SqlDbType.Decimal).Value = traspaso.IdFondoOrigen;
            cmd.Parameters.Add("@ID_FONDO_DESTINO", SqlDbType.Decimal).Value = traspaso.IdFondoDestino;
            cmd.Parameters.Add("@ID_CUENTA_PARTICIPACION_ORIGEN", SqlDbType.Decimal).Value = traspaso.IdCuentaOrigen;
            cmd.Parameters.Add("@ID_CUENTA_PARTICIPACION_DESTINO", SqlDbType.Decimal).Value = DBNull.Value;
            cmd.Parameters.Add("@flagMonedaDistinto", SqlDbType.VarChar).Value = traspaso.FlagMonedaDistinto;
            cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = traspaso.Usuario;
            cmd.Parameters.Add("@fecha", SqlDbType.Date).Value = DateTime.Now;
            cmd.Parameters.Add("@area", SqlDbType.VarChar).Value = traspaso.Area;
            cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            int result = (int)cmd.Parameters["@RETURN_VALUE"].Value;
            return result;

        }
        #endregion

        #region
        /// <summary>
        /// Obtener promotor
        /// </summary>
        public string ObtenerPromotor(int idParticipe)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_PARTICIPE_PROMOTOR", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdParticipe = cmd.Parameters.Add("@IdParticipe", SqlDbType.Decimal);
            prmIdParticipe.Value = idParticipe;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string promotor = string.Empty;
            DataRow[] drPromotores = dt.Select("FLAG_ACTUAL = 'S'", "ID DESC");
            if (drPromotores.Length > 0)
            {
                promotor = drPromotores[0]["CODIGO"].ToString();
            }

            return promotor;
        }
        #endregion

        #region
        /// <summary>
        /// Obtener último Valor Cuota
        /// </summary>
        public decimal ObtenerUltimoValorCuotaFondo(int idFondo)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_VALOR_CUOTA_ULTIMO", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idfondo", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            decimal vc = 0;

            if (dt.Rows.Count > 0)
            {
                vc = Convert.ToDecimal(dt.Rows[0]["VALOR_CUOTA"]);
            }

            return vc;
        }
        #endregion

        #region
        /// <summary>
        /// Obtener saldo cuotas por cuenta participación
        /// </summary>
        public decimal ObtenerSaldoCuotasxCuentaParticipacion(int idFondo, int idCuentaParticipacion)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_CUENTA_PARTICIPACION_X_FONDO_MONTO_CUOTAS", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlParameter prmIdCuenta = cmd.Parameters.Add("@idCuenta", SqlDbType.Decimal);
            prmIdCuenta.Value = idCuentaParticipacion;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            decimal saldoCuotas = 0;

            if (dt.Rows.Count > 0)
            {
                saldoCuotas = Convert.ToDecimal(dt.Rows[0]["TOTAL_CUOTAS"]);
            }

            return saldoCuotas;
        }
        #endregion

        #region
        /// <summary>
        /// Obtener saldo cuotas por cuenta participación
        /// </summary>
        public string InsertarRescateTraspaso(int participeOrigen, int fondoOrigen, int cuentaParticipacionOrigen, int idTraspaso, string tipoRescate, decimal montoRescate, decimal numeroCuotas, decimal vc, string promotor, string codigo, string cuentaRecaudo, DateTime fechaSolicitudRescate, DateTime fechaPrecierre, DateTime fechaPagoRescate )
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_INS_OPERACION_RESCATE", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            #region parametros
            SqlParameter tipoOperacion = cmd.Parameters.Add("@tipoOperacion", SqlDbType.VarChar);
            SqlParameter estado = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
            SqlParameter nroCuotas = cmd.Parameters.Add("@nroCuotas", SqlDbType.Decimal);
            SqlParameter valorCuota = cmd.Parameters.Add("@valorCuota", SqlDbType.Decimal);
            SqlParameter fechaOperacion = cmd.Parameters.Add("@fechaOperacion", SqlDbType.DateTime);
            SqlParameter idParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
            SqlParameter idCtaOrigen = cmd.Parameters.Add("@idCtaOrigen", SqlDbType.Decimal);
            SqlParameter idCtaDestino = cmd.Parameters.Add("@idCtaDestino", SqlDbType.Decimal);
            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            SqlParameter monto = cmd.Parameters.Add("@monto", SqlDbType.Decimal);
            SqlParameter montoNeto = cmd.Parameters.Add("@montoNeto", SqlDbType.Decimal);
            SqlParameter codigoPromotor = cmd.Parameters.Add("@codigoPromotor", SqlDbType.VarChar);
            SqlParameter codigoViaPago = cmd.Parameters.Add("@codigoViaPago", SqlDbType.VarChar);
            SqlParameter codigoFormaPago = cmd.Parameters.Add("@codigoFormaPago", SqlDbType.VarChar);
            SqlParameter fechaAbonoContable = cmd.Parameters.Add("@fechaAbonoContable", SqlDbType.DateTime);
            SqlParameter fechaAbonoDisponible = cmd.Parameters.Add("@fechaAbonoDisponible", SqlDbType.DateTime);
            SqlParameter importePago = cmd.Parameters.Add("@importePago", SqlDbType.Decimal);
            SqlParameter nroOperacionDeposito = cmd.Parameters.Add("@nroOperacionDeposito", SqlDbType.VarChar);
            SqlParameter nroCheque = cmd.Parameters.Add("@nroCheque", SqlDbType.VarChar);
            SqlParameter nroCuentaCheque = cmd.Parameters.Add("@nroCuentaCheque", SqlDbType.VarChar);
            SqlParameter fechaCheque = cmd.Parameters.Add("@fechaCheque", SqlDbType.DateTime);
            SqlParameter codigoBancoCheque = cmd.Parameters.Add("@codigoBancoCheque", SqlDbType.VarChar);
            SqlParameter flagRescateTotal = cmd.Parameters.Add("@flagRescateTotal", SqlDbType.VarChar);
            SqlParameter flagRescateEnCuotas = cmd.Parameters.Add("@flagRescateEnCuotas", SqlDbType.VarChar);
            SqlParameter flagRescateSignificativo = cmd.Parameters.Add("@flagRescateSignificativo", SqlDbType.VarChar);
            SqlParameter fechaProceso = cmd.Parameters.Add("@fechaProceso", SqlDbType.DateTime);
            SqlParameter idCuentaBancaria = cmd.Parameters.Add("@idCuentaBancaria", SqlDbType.Decimal);
            SqlParameter fechaCargo = cmd.Parameters.Add("@fechaCargo", SqlDbType.DateTime);
            SqlParameter flagNivel1 = cmd.Parameters.Add("@flagNivel1", SqlDbType.VarChar);
            SqlParameter flagNivel2 = cmd.Parameters.Add("@flagNivel2", SqlDbType.VarChar);
            SqlParameter flagEliminado = cmd.Parameters.Add("@flagEliminado", SqlDbType.VarChar);
            SqlParameter adicional1 = cmd.Parameters.Add("@adicional1", SqlDbType.VarChar);
            SqlParameter adicional2 = cmd.Parameters.Add("@adicional2", SqlDbType.VarChar);
            SqlParameter adicional3 = cmd.Parameters.Add("@adicional3", SqlDbType.VarChar);
            SqlParameter idOperacionReferencia = cmd.Parameters.Add("@idOperacionReferencia", SqlDbType.Decimal);
            SqlParameter idPreCierre = cmd.Parameters.Add("@idPreCierre", SqlDbType.Decimal);
            SqlParameter codigoViaSolicitud = cmd.Parameters.Add("@codigoViaSolicitud", SqlDbType.VarChar);
            SqlParameter idFondoDestino = cmd.Parameters.Add("@idFondoDestino", SqlDbType.Decimal);
            SqlParameter numeroSolicitud = cmd.Parameters.Add("@numeroSolicitud", SqlDbType.VarChar);
            SqlParameter idParticipeDestino = cmd.Parameters.Add("@idParticipeDestino", SqlDbType.Decimal);
            SqlParameter porcentajeComision = cmd.Parameters.Add("@porcentajeComision", SqlDbType.Decimal);
            SqlParameter comision = cmd.Parameters.Add("@comision", SqlDbType.Decimal);
            SqlParameter porcentajeComisionEspecial = cmd.Parameters.Add("@porcentajeComisionEspecial", SqlDbType.Decimal);
            SqlParameter comisionEspecial = cmd.Parameters.Add("@comisionEspecial", SqlDbType.Decimal);
            SqlParameter igv = cmd.Parameters.Add("@igv", SqlDbType.Decimal);
            SqlParameter flagProcesoComisiones = cmd.Parameters.Add("@flagProcesoComisiones", SqlDbType.VarChar);
            SqlParameter montoAjustadoBruto = cmd.Parameters.Add("@montoAjustadoBruto", SqlDbType.Decimal);
            SqlParameter montoAjustadoNeto = cmd.Parameters.Add("@montoAjustadoNeto", SqlDbType.Decimal);
            SqlParameter concepto = cmd.Parameters.Add("@concepto", SqlDbType.VarChar);
            SqlParameter usuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
            SqlParameter usuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            SqlParameter areaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            SqlParameter estadoAnterior = cmd.Parameters.Add("@estadoAnterior", SqlDbType.VarChar);
            SqlParameter idFondoOrigen = cmd.Parameters.Add("@idFondoOrigen", SqlDbType.Decimal);
            SqlParameter idParticipeOrigen = cmd.Parameters.Add("@idParticipeOrigen", SqlDbType.Decimal);
            SqlParameter numeroCertificado = cmd.Parameters.Add("@numeroCertificado", SqlDbType.VarChar);
            SqlParameter fechaSolicitud = cmd.Parameters.Add("@fechaSolicitud", SqlDbType.VarChar);
            SqlParameter fechaPago = cmd.Parameters.Add("@fechaPago", SqlDbType.VarChar);
            SqlParameter flagRescateProgramado = cmd.Parameters.Add("@flagRescateProgramado", SqlDbType.VarChar);
            SqlParameter nroOperacion = cmd.Parameters.Add("@nroOperacion", SqlDbType.Decimal);
            nroOperacion.Direction = ParameterDirection.Output;

            SqlParameter idOperacion = cmd.Parameters.Add("@idOperacion", SqlDbType.VarChar);
            idOperacion.Direction = ParameterDirection.ReturnValue;
            #endregion

            #region Valores en Común de los rescates
            tipoOperacion.Value = Constants.ConstantesING.TIPO_OPERACION_RESCATE;
            estado.Value = Constants.ConstantesING.ESTADO_OPERACION_PENDIENTE;
            fechaOperacion.Value = DateTime.Now.ToString(Constants.ConstantesING.FORMATO);
            idCtaDestino.Value = DBNull.Value;

            flagRescateSignificativo.Value = Constants.ConstantesING.NO;
            idCuentaBancaria.Value = DBNull.Value;
            fechaCargo.Value = DBNull.Value;
            flagNivel1.Value = Constants.ConstantesING.NO;
            flagNivel2.Value = Constants.ConstantesING.NO;
            flagEliminado.Value = Constants.ConstantesING.NO;
            adicional1.Value = DBNull.Value;
            adicional2.Value = Constants.ConstantesING.ADICIONAL2;
            adicional3.Value = cuentaRecaudo;
            idOperacionReferencia.Value = DBNull.Value;
            idPreCierre.Value = DBNull.Value;
            codigoViaSolicitud.Value = Constants.ConstantesING.CODIGO_VIA_SOLICITUD_AGENCIA;
            idFondoDestino.Value = DBNull.Value;
            numeroSolicitud.Value = DBNull.Value;
            idParticipeDestino.Value = DBNull.Value;
            porcentajeComision.Value = DBNull.Value;
            comision.Value = DBNull.Value;
            porcentajeComisionEspecial.Value = DBNull.Value;
            comisionEspecial.Value = DBNull.Value;
            igv.Value = 18;
            flagProcesoComisiones.Value = Constants.ConstantesING.NO;
            montoAjustadoBruto.Value = DBNull.Value;
            montoAjustadoNeto.Value = DBNull.Value;
            concepto.Value = idTraspaso.ToString();
            usuarioCreacion.Value = codigo;
            usuarioModificacion.Value = codigo;
            areaModificacion.Value = Constants.ConstantesING.AREA_TRASPASO_FONDO;
            estadoAnterior.Value = Constants.ConstantesING.ESTADO_OPERACION_PENDIENTE;
            flagRescateProgramado.Value = Constants.ConstantesING.NO;

            fechaSolicitud.Value = fechaSolicitudRescate.ToString(Constants.ConstantesING.FORMATO);//DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//PONER FECHA Y HORA QUE CORRESPONDE PARA EL SIGUIENTE CIERRE EN TESTING
            fechaPago.Value = fechaPagoRescate.ToString(Constants.ConstantesING.FORMATO);
            #endregion

            #region variables
            if (numeroCuotas > 0)
            {
                nroCuotas.Value = numeroCuotas;
            }
            else
            {
                nroCuotas.Value = DBNull.Value;
            }
            valorCuota.Value = DBNull.Value;
            idParticipe.Value = participeOrigen;
            idCtaOrigen.Value = cuentaParticipacionOrigen;
            prmIdFondo.Value = fondoOrigen;
            if (tipoRescate == Constants.ConstantesING.TIPO_RESCATE_TOTAL)
            {
                flagRescateTotal.Value = Constants.ConstantesING.SI;
                flagRescateEnCuotas.Value = Constants.ConstantesING.SI;
            }
            else
            {
                flagRescateTotal.Value = Constants.ConstantesING.NO;
                if (numeroCuotas > 0)
                {
                    flagRescateEnCuotas.Value = Constants.ConstantesING.SI;
                }
                else
                {
                    flagRescateEnCuotas.Value = Constants.ConstantesING.NO;
                }
            }

            if (montoRescate > 0)
            {
                monto.Value = montoRescate;
                montoNeto.Value = montoRescate;
            }
            else
            {
                monto.Value = numeroCuotas * vc;
                montoNeto.Value = numeroCuotas * vc;
            }

            codigoPromotor.Value = promotor;
            if (codigoPromotor.Value.Equals(""))
                codigoPromotor.Value = DBNull.Value;

            codigoViaPago.Value = Constants.ConstantesING.CODIGO_VIA_PAGO_TRASPASO_FONDOS;
            codigoFormaPago.Value = Constants.ConstantesING.CODIGO_FORMA_PAGO_TRASPASO_FONDOS;
            fechaAbonoContable.Value = DBNull.Value;
            fechaAbonoDisponible.Value = DBNull.Value;
            importePago.Value = DBNull.Value;
            nroOperacionDeposito.Value = DBNull.Value;
            nroCheque.Value = DBNull.Value;
            nroCuentaCheque.Value = DBNull.Value;
            fechaCheque.Value = DBNull.Value;
            codigoBancoCheque.Value = DBNull.Value;
            fechaProceso.Value = fechaPrecierre.ToString(Constants.ConstantesING.FORMATO);
            idFondoOrigen.Value = fondoOrigen;
            idParticipeOrigen.Value = participeOrigen;
            numeroCertificado.Value = DBNull.Value;

            #endregion
            cmd.ExecuteNonQuery();
            string result = idOperacion.Value.ToString();
            return result;

        }

        #endregion

        #region
        /// <summary>
        /// Insertar alerta
        /// </summary>
				/// 
				//OT10433 INI 
        public void InsertarAlerta(string tipoOperacion, string descripcion,int idOperacion, string codigoUsuario, int numeroLote, string tipo)
				//OT10433 FIN	
				{
            SqlCommand cmd = new SqlCommand("dbo.FOND_INS_ALERTA", conn, trans);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@clase", SqlDbType.VarChar).Value = Constants.ConstantesING.CLASE_ALERTA_EVENTO;
            cmd.Parameters.Add("@tipo", SqlDbType.VarChar).Value = Constants.ConstantesING.TIPO_ALERTA_OBSERVACION;
            cmd.Parameters.Add("@subtipo", SqlDbType.VarChar).Value = Constants.ConstantesING.SUBTIPO_ALERTA_OTROS;
            cmd.Parameters.Add("@titulo", SqlDbType.VarChar).Value = tipoOperacion;
						//OT10433 INI 
						cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = descripcion;
						//OT10433 FIN	
						cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = Constants.ConstantesING.ESTADO_ALERTA;
            cmd.Parameters.Add("@fechaLimite", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@idUsuarioOrigen", SqlDbType.Int).Value = Constants.ConstantesING.USUARIO_ALERTA;
            cmd.Parameters.Add("@idUsuarioDestino", SqlDbType.Int).Value = DBNull.Value;
            cmd.Parameters.Add("@idParticipe", SqlDbType.Int).Value = DBNull.Value;
            cmd.Parameters.Add("@idFondo", SqlDbType.Int).Value = DBNull.Value;
						//OT10433 INI 
						if (tipo.Equals("REV") || idOperacion == 0)
            {
                cmd.Parameters.Add("@idOperacion", SqlDbType.Int).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@idOperacion", SqlDbType.Int).Value = idOperacion;
            }
						//OT10433 FIN
            cmd.Parameters.Add("@idPrecierre", SqlDbType.Int).Value = DBNull.Value;
            cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar).Value = codigoUsuario;
            cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar).Value = Constants.ConstantesING.AREA_MODIFICACION_ALERTA;
            if (tipo.Equals("REV"))
            {
                cmd.Parameters.Add("@subtipoOtro", SqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@subtipoOtro", SqlDbType.VarChar).Value = numeroLote.ToString();
            }
            cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;


            cmd.ExecuteNonQuery();

        }
        #endregion

        #region
        /// <summary>
        /// Obtener número de lote
        /// </summary>
        public int ObtenerNumeroLote()
        {
          // int idFondo, DateTime fecha
        
            int numeroLote = 0;

            SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_NUM_LOTES", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
           

                numeroLote = Convert.ToInt32(cmd.ExecuteScalar()) + 1;


            
            return numeroLote;

        }
        #endregion

       
        #region
        /// <summary>
        /// Insertar rescate
        /// </summary>
        public string InsertarRescate(int participeOrigen, int fondoOrigen, int cuentaParticipacionOrigen, int idTraspaso, string tipoRescate, decimal montoRescate, decimal numeroCuotas, decimal vc, string promotor, string codigo, string cuentaRecaudo, DateTime fechaSolicitudRescate, DateTime fechaPrecierre, DateTime fechaPagoRescate, string viaPago, string formaPago, decimal idCuentaBan)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_INS_OPERACION_RESCATE", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            #region parametros
            SqlParameter tipoOperacion = cmd.Parameters.Add("@tipoOperacion", SqlDbType.VarChar);
            SqlParameter estado = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
            SqlParameter nroCuotas = cmd.Parameters.Add("@nroCuotas", SqlDbType.Decimal);
            SqlParameter valorCuota = cmd.Parameters.Add("@valorCuota", SqlDbType.Decimal);
            SqlParameter fechaOperacion = cmd.Parameters.Add("@fechaOperacion", SqlDbType.DateTime);
            SqlParameter idParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
            SqlParameter idCtaOrigen = cmd.Parameters.Add("@idCtaOrigen", SqlDbType.Decimal);
            SqlParameter idCtaDestino = cmd.Parameters.Add("@idCtaDestino", SqlDbType.Decimal);
            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            SqlParameter monto = cmd.Parameters.Add("@monto", SqlDbType.Decimal);
            SqlParameter montoNeto = cmd.Parameters.Add("@montoNeto", SqlDbType.Decimal);
            SqlParameter codigoPromotor = cmd.Parameters.Add("@codigoPromotor", SqlDbType.VarChar);
            SqlParameter codigoViaPago = cmd.Parameters.Add("@codigoViaPago", SqlDbType.VarChar);
            SqlParameter codigoFormaPago = cmd.Parameters.Add("@codigoFormaPago", SqlDbType.VarChar);
            SqlParameter fechaAbonoContable = cmd.Parameters.Add("@fechaAbonoContable", SqlDbType.DateTime);
            SqlParameter fechaAbonoDisponible = cmd.Parameters.Add("@fechaAbonoDisponible", SqlDbType.DateTime);
            SqlParameter importePago = cmd.Parameters.Add("@importePago", SqlDbType.Decimal);
            SqlParameter nroOperacionDeposito = cmd.Parameters.Add("@nroOperacionDeposito", SqlDbType.VarChar);
            SqlParameter nroCheque = cmd.Parameters.Add("@nroCheque", SqlDbType.VarChar);
            SqlParameter nroCuentaCheque = cmd.Parameters.Add("@nroCuentaCheque", SqlDbType.VarChar);
            SqlParameter fechaCheque = cmd.Parameters.Add("@fechaCheque", SqlDbType.DateTime);
            SqlParameter codigoBancoCheque = cmd.Parameters.Add("@codigoBancoCheque", SqlDbType.VarChar);
            SqlParameter flagRescateTotal = cmd.Parameters.Add("@flagRescateTotal", SqlDbType.VarChar);
            SqlParameter flagRescateEnCuotas = cmd.Parameters.Add("@flagRescateEnCuotas", SqlDbType.VarChar);
            SqlParameter flagRescateSignificativo = cmd.Parameters.Add("@flagRescateSignificativo", SqlDbType.VarChar);
            SqlParameter fechaProceso = cmd.Parameters.Add("@fechaProceso", SqlDbType.DateTime);
            SqlParameter idCuentaBancaria = cmd.Parameters.Add("@idCuentaBancaria", SqlDbType.Decimal);
            SqlParameter fechaCargo = cmd.Parameters.Add("@fechaCargo", SqlDbType.DateTime);
            SqlParameter flagNivel1 = cmd.Parameters.Add("@flagNivel1", SqlDbType.VarChar);
            SqlParameter flagNivel2 = cmd.Parameters.Add("@flagNivel2", SqlDbType.VarChar);
            SqlParameter flagEliminado = cmd.Parameters.Add("@flagEliminado", SqlDbType.VarChar);
            SqlParameter adicional1 = cmd.Parameters.Add("@adicional1", SqlDbType.VarChar);
            SqlParameter adicional2 = cmd.Parameters.Add("@adicional2", SqlDbType.VarChar);
            SqlParameter adicional3 = cmd.Parameters.Add("@adicional3", SqlDbType.VarChar);
            SqlParameter idOperacionReferencia = cmd.Parameters.Add("@idOperacionReferencia", SqlDbType.Decimal);
            SqlParameter idPreCierre = cmd.Parameters.Add("@idPreCierre", SqlDbType.Decimal);
            SqlParameter codigoViaSolicitud = cmd.Parameters.Add("@codigoViaSolicitud", SqlDbType.VarChar);
            SqlParameter idFondoDestino = cmd.Parameters.Add("@idFondoDestino", SqlDbType.Decimal);
            SqlParameter numeroSolicitud = cmd.Parameters.Add("@numeroSolicitud", SqlDbType.VarChar);
            SqlParameter idParticipeDestino = cmd.Parameters.Add("@idParticipeDestino", SqlDbType.Decimal);
            SqlParameter porcentajeComision = cmd.Parameters.Add("@porcentajeComision", SqlDbType.Decimal);
            SqlParameter comision = cmd.Parameters.Add("@comision", SqlDbType.Decimal);
            SqlParameter porcentajeComisionEspecial = cmd.Parameters.Add("@porcentajeComisionEspecial", SqlDbType.Decimal);
            SqlParameter comisionEspecial = cmd.Parameters.Add("@comisionEspecial", SqlDbType.Decimal);
            SqlParameter igv = cmd.Parameters.Add("@igv", SqlDbType.Decimal);
            SqlParameter flagProcesoComisiones = cmd.Parameters.Add("@flagProcesoComisiones", SqlDbType.VarChar);
            SqlParameter montoAjustadoBruto = cmd.Parameters.Add("@montoAjustadoBruto", SqlDbType.Decimal);
            SqlParameter montoAjustadoNeto = cmd.Parameters.Add("@montoAjustadoNeto", SqlDbType.Decimal);
            SqlParameter concepto = cmd.Parameters.Add("@concepto", SqlDbType.VarChar);
            SqlParameter usuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
            SqlParameter usuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            SqlParameter areaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            SqlParameter estadoAnterior = cmd.Parameters.Add("@estadoAnterior", SqlDbType.VarChar);
            SqlParameter idFondoOrigen = cmd.Parameters.Add("@idFondoOrigen", SqlDbType.Decimal);
            SqlParameter idParticipeOrigen = cmd.Parameters.Add("@idParticipeOrigen", SqlDbType.Decimal);
            SqlParameter numeroCertificado = cmd.Parameters.Add("@numeroCertificado", SqlDbType.VarChar);
            SqlParameter fechaSolicitud = cmd.Parameters.Add("@fechaSolicitud", SqlDbType.VarChar);
            SqlParameter fechaPago = cmd.Parameters.Add("@fechaPago", SqlDbType.VarChar);
            SqlParameter flagRescateProgramado = cmd.Parameters.Add("@flagRescateProgramado", SqlDbType.VarChar);

            SqlParameter nroOperacion = cmd.Parameters.Add("@nroOperacion", SqlDbType.Decimal);
            nroOperacion.Direction = ParameterDirection.Output;

            SqlParameter idOperacion = cmd.Parameters.Add("@idOperacion", SqlDbType.VarChar);
            idOperacion.Direction = ParameterDirection.ReturnValue;
            #endregion

            #region Valores en Común de los rescates
            tipoOperacion.Value = Constants.ConstantesING.TIPO_OPERACION_RESCATE;
            estado.Value = Constants.ConstantesING.ESTADO_OPERACION_PENDIENTE;
            fechaOperacion.Value = DateTime.Now.ToString(Constants.ConstantesING.FORMATO);
            idCtaDestino.Value = DBNull.Value;

            flagRescateSignificativo.Value = Constants.ConstantesING.NO;

            fechaCargo.Value = DBNull.Value;
            flagNivel1.Value = Constants.ConstantesING.NO;
            flagNivel2.Value = Constants.ConstantesING.NO;
            flagEliminado.Value = Constants.ConstantesING.NO;
            adicional1.Value = DBNull.Value;
            if (!cuentaRecaudo.Equals(string.Empty))
            {
                adicional2.Value = Constants.ConstantesING.ADICIONAL2;
                adicional3.Value = cuentaRecaudo;
            }
            else
            {
                adicional2.Value = DBNull.Value;
                adicional3.Value = DBNull.Value;
            }
            idOperacionReferencia.Value = DBNull.Value;
            idPreCierre.Value = DBNull.Value;
            codigoViaSolicitud.Value = Constants.ConstantesING.CODIGO_VIA_SOLICITUD_AGENCIA;
            idFondoDestino.Value = DBNull.Value;
            numeroSolicitud.Value = DBNull.Value;
            idParticipeDestino.Value = DBNull.Value;
            porcentajeComision.Value = DBNull.Value;
            comision.Value = DBNull.Value;
            porcentajeComisionEspecial.Value = DBNull.Value;
            comisionEspecial.Value = DBNull.Value;
            igv.Value = Constants.ConstantesING.IGV_VALOR;
            flagProcesoComisiones.Value = Constants.ConstantesING.NO;
            montoAjustadoBruto.Value = DBNull.Value;
            montoAjustadoNeto.Value = DBNull.Value;
            if (idTraspaso == 0)
            {
                concepto.Value = DBNull.Value;
            }
            else
            {
                concepto.Value = idTraspaso.ToString();
            }
            usuarioCreacion.Value = codigo;
            usuarioModificacion.Value = codigo;
            areaModificacion.Value = Constants.ConstantesING.AREA_TRASPASO_FONDO;
            estadoAnterior.Value = Constants.ConstantesING.ESTADO_OPERACION_PENDIENTE;
            flagRescateProgramado.Value = Constants.ConstantesING.NO;
            fechaSolicitud.Value = fechaSolicitudRescate.ToString(Constants.ConstantesING.FORMATO);//DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//PONER FECHA Y HORA QUE CORRESPONDE PARA EL SIGUIENTE CIERRE EN TESTING
            fechaPago.Value = fechaPagoRescate.ToString(Constants.ConstantesING.FORMATO);
            #endregion

            #region variables
            if (numeroCuotas > 0)
            {
                nroCuotas.Value = numeroCuotas;
            }
            else
            {
                nroCuotas.Value = DBNull.Value;
            }
            valorCuota.Value = DBNull.Value;
            idParticipe.Value = participeOrigen;
            idCtaOrigen.Value = cuentaParticipacionOrigen;
            prmIdFondo.Value = fondoOrigen;
            if (tipoRescate == Constants.ConstantesING.TIPO_RESCATE_TOTAL)
            {
                flagRescateTotal.Value = Constants.ConstantesING.SI;
                flagRescateEnCuotas.Value = Constants.ConstantesING.SI;
            }
            else
            {
                flagRescateTotal.Value = Constants.ConstantesING.NO;
                if (numeroCuotas > 0)
                {
                    flagRescateEnCuotas.Value = Constants.ConstantesING.SI;
                }
                else
                {
                    flagRescateEnCuotas.Value = Constants.ConstantesING.NO;
                }
            }

            if (montoRescate > 0)
            {
                monto.Value = montoRescate;
                montoNeto.Value = montoRescate;
            }
            else
            {
                monto.Value = numeroCuotas * vc;
                montoNeto.Value = numeroCuotas * vc;
            }

            codigoPromotor.Value = promotor;
            if (codigoPromotor.Value.Equals(""))
                codigoPromotor.Value = DBNull.Value;

            codigoViaPago.Value = viaPago;
            codigoFormaPago.Value = formaPago;
            if (formaPago.Equals(Constants.ConstantesING.CODIGO_FORMA_PAGO_TRASPASO_FONDOS) && cuentaRecaudo.Equals(string.Empty)) //preguntar si es traspasos a fondo
            {
                idCuentaBancaria.Value = idCuentaBan;
            }
            else
            {
                idCuentaBancaria.Value = DBNull.Value;
            }
            fechaAbonoContable.Value = DBNull.Value;
            fechaAbonoDisponible.Value = DBNull.Value;
            importePago.Value = DBNull.Value;
            nroOperacionDeposito.Value = DBNull.Value;
            nroCheque.Value = DBNull.Value;
            nroCuentaCheque.Value = DBNull.Value;
            fechaCheque.Value = DBNull.Value;
            codigoBancoCheque.Value = DBNull.Value;
            fechaProceso.Value = fechaPrecierre.ToString(Constants.ConstantesING.FORMATO);
            idFondoOrigen.Value = fondoOrigen;
            idParticipeOrigen.Value = participeOrigen;
            numeroCertificado.Value = DBNull.Value;

            #endregion
            cmd.ExecuteNonQuery();
            string result = idOperacion.Value.ToString();
            return result;//result;

        }
        #endregion

				//OT10082 PSC001 INI
				public string InsertarFlujo(int participeOrigen, int fondoOrigen, decimal montoFlujo, decimal vc, string promotor, string codigo, DateTime fechaSolicitudRescate, DateTime fechaPrecierre, DateTime fechaPagoRescate, string viaPago, string formaPago, decimal idCuentaBan)
				{
					SqlCommand cmd = new SqlCommand("dbo.FOND_INS_OPERACION_RESCATE", conn, trans);
					cmd.CommandType = CommandType.StoredProcedure;
					#region parametros
					SqlParameter tipoOperacion = cmd.Parameters.Add("@tipoOperacion", SqlDbType.VarChar);
					SqlParameter estado = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
					SqlParameter nroCuotas = cmd.Parameters.Add("@nroCuotas", SqlDbType.Decimal);
					SqlParameter valorCuota = cmd.Parameters.Add("@valorCuota", SqlDbType.Decimal);
					SqlParameter fechaOperacion = cmd.Parameters.Add("@fechaOperacion", SqlDbType.DateTime);
					SqlParameter idParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
					SqlParameter idCtaOrigen = cmd.Parameters.Add("@idCtaOrigen", SqlDbType.Decimal);
					SqlParameter idCtaDestino = cmd.Parameters.Add("@idCtaDestino", SqlDbType.Decimal);
					SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
					SqlParameter monto = cmd.Parameters.Add("@monto", SqlDbType.Decimal);
					SqlParameter montoNeto = cmd.Parameters.Add("@montoNeto", SqlDbType.Decimal);
					SqlParameter codigoPromotor = cmd.Parameters.Add("@codigoPromotor", SqlDbType.VarChar);
					SqlParameter codigoViaPago = cmd.Parameters.Add("@codigoViaPago", SqlDbType.VarChar);
					SqlParameter codigoFormaPago = cmd.Parameters.Add("@codigoFormaPago", SqlDbType.VarChar);
					SqlParameter fechaAbonoContable = cmd.Parameters.Add("@fechaAbonoContable", SqlDbType.DateTime);
					SqlParameter fechaAbonoDisponible = cmd.Parameters.Add("@fechaAbonoDisponible", SqlDbType.DateTime);
					SqlParameter importePago = cmd.Parameters.Add("@importePago", SqlDbType.Decimal);
					SqlParameter nroOperacionDeposito = cmd.Parameters.Add("@nroOperacionDeposito", SqlDbType.VarChar);
					SqlParameter nroCheque = cmd.Parameters.Add("@nroCheque", SqlDbType.VarChar);
					SqlParameter nroCuentaCheque = cmd.Parameters.Add("@nroCuentaCheque", SqlDbType.VarChar);
					SqlParameter fechaCheque = cmd.Parameters.Add("@fechaCheque", SqlDbType.DateTime);
					SqlParameter codigoBancoCheque = cmd.Parameters.Add("@codigoBancoCheque", SqlDbType.VarChar);
					SqlParameter flagRescateTotal = cmd.Parameters.Add("@flagRescateTotal", SqlDbType.VarChar);
					SqlParameter flagRescateEnCuotas = cmd.Parameters.Add("@flagRescateEnCuotas", SqlDbType.VarChar);
					SqlParameter flagRescateSignificativo = cmd.Parameters.Add("@flagRescateSignificativo", SqlDbType.VarChar);
					SqlParameter fechaProceso = cmd.Parameters.Add("@fechaProceso", SqlDbType.DateTime);
					SqlParameter idCuentaBancaria = cmd.Parameters.Add("@idCuentaBancaria", SqlDbType.Decimal);
					SqlParameter fechaCargo = cmd.Parameters.Add("@fechaCargo", SqlDbType.DateTime);
					SqlParameter flagNivel1 = cmd.Parameters.Add("@flagNivel1", SqlDbType.VarChar);
					SqlParameter flagNivel2 = cmd.Parameters.Add("@flagNivel2", SqlDbType.VarChar);
					SqlParameter flagEliminado = cmd.Parameters.Add("@flagEliminado", SqlDbType.VarChar);
					SqlParameter adicional1 = cmd.Parameters.Add("@adicional1", SqlDbType.VarChar);
					SqlParameter adicional2 = cmd.Parameters.Add("@adicional2", SqlDbType.VarChar);
					SqlParameter adicional3 = cmd.Parameters.Add("@adicional3", SqlDbType.VarChar);
					SqlParameter idOperacionReferencia = cmd.Parameters.Add("@idOperacionReferencia", SqlDbType.Decimal);
					SqlParameter idPreCierre = cmd.Parameters.Add("@idPreCierre", SqlDbType.Decimal);
					SqlParameter codigoViaSolicitud = cmd.Parameters.Add("@codigoViaSolicitud", SqlDbType.VarChar);
					SqlParameter idFondoDestino = cmd.Parameters.Add("@idFondoDestino", SqlDbType.Decimal);
					SqlParameter numeroSolicitud = cmd.Parameters.Add("@numeroSolicitud", SqlDbType.VarChar);
					SqlParameter idParticipeDestino = cmd.Parameters.Add("@idParticipeDestino", SqlDbType.Decimal);
					SqlParameter porcentajeComision = cmd.Parameters.Add("@porcentajeComision", SqlDbType.Decimal);
					SqlParameter comision = cmd.Parameters.Add("@comision", SqlDbType.Decimal);
					SqlParameter porcentajeComisionEspecial = cmd.Parameters.Add("@porcentajeComisionEspecial", SqlDbType.Decimal);
					SqlParameter comisionEspecial = cmd.Parameters.Add("@comisionEspecial", SqlDbType.Decimal);
					SqlParameter igv = cmd.Parameters.Add("@igv", SqlDbType.Decimal);
					SqlParameter flagProcesoComisiones = cmd.Parameters.Add("@flagProcesoComisiones", SqlDbType.VarChar);
					SqlParameter montoAjustadoBruto = cmd.Parameters.Add("@montoAjustadoBruto", SqlDbType.Decimal);
					SqlParameter montoAjustadoNeto = cmd.Parameters.Add("@montoAjustadoNeto", SqlDbType.Decimal);
					SqlParameter concepto = cmd.Parameters.Add("@concepto", SqlDbType.VarChar);
					SqlParameter usuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
					SqlParameter usuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
					SqlParameter areaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
					SqlParameter estadoAnterior = cmd.Parameters.Add("@estadoAnterior", SqlDbType.VarChar);
					SqlParameter idFondoOrigen = cmd.Parameters.Add("@idFondoOrigen", SqlDbType.Decimal);
					SqlParameter idParticipeOrigen = cmd.Parameters.Add("@idParticipeOrigen", SqlDbType.Decimal);
					SqlParameter numeroCertificado = cmd.Parameters.Add("@numeroCertificado", SqlDbType.VarChar);
					SqlParameter fechaSolicitud = cmd.Parameters.Add("@fechaSolicitud", SqlDbType.VarChar);
					SqlParameter fechaPago = cmd.Parameters.Add("@fechaPago", SqlDbType.VarChar);
					SqlParameter flagRescateProgramado = cmd.Parameters.Add("@flagRescateProgramado", SqlDbType.VarChar);

					SqlParameter nroOperacion = cmd.Parameters.Add("@nroOperacion", SqlDbType.Decimal);
					nroOperacion.Direction = ParameterDirection.Output;

					SqlParameter idOperacion = cmd.Parameters.Add("@idOperacion", SqlDbType.VarChar);
					idOperacion.Direction = ParameterDirection.ReturnValue;
					#endregion

					#region Valores en Común de los rescates
					tipoOperacion.Value = "FLU";
					estado.Value = "CON";
					fechaOperacion.Value = DateTime.Now.ToString(Constants.ConstantesING.FORMATO);
					idCtaDestino.Value = DBNull.Value;

					flagRescateSignificativo.Value = Constants.ConstantesING.NO;

					fechaCargo.Value = DBNull.Value;
					flagNivel1.Value = Constants.ConstantesING.NO;
					flagNivel2.Value = Constants.ConstantesING.NO;
					flagEliminado.Value = Constants.ConstantesING.NO;
					adicional1.Value = DBNull.Value;
					adicional2.Value = DBNull.Value;
					adicional3.Value = DBNull.Value;

					idOperacionReferencia.Value = DBNull.Value;
					idPreCierre.Value = DBNull.Value;
					//OT10082 PSC002 INI
					codigoViaSolicitud.Value = DBNull.Value;//Constants.ConstantesING.CODIGO_VIA_SOLICITUD_AGENCIA;
					//OT10082 PSC002 FIN
					idFondoDestino.Value = DBNull.Value;
					numeroSolicitud.Value = DBNull.Value;
					idParticipeDestino.Value = DBNull.Value;
					porcentajeComision.Value = DBNull.Value;
					//OT10082 PSC002 INI
					comision.Value = 0;//DBNull.Value;
					//OT10082 PSC002 FIN
					porcentajeComisionEspecial.Value = DBNull.Value;
					comisionEspecial.Value = DBNull.Value;
					igv.Value = 0;//Constants.ConstantesING.IGV_VALOR;
					//OT10082 PSC002 FIN
					flagProcesoComisiones.Value = Constants.ConstantesING.NO;
					montoAjustadoBruto.Value = DBNull.Value;
					montoAjustadoNeto.Value = DBNull.Value;

					concepto.Value = DBNull.Value;

					usuarioCreacion.Value = codigo;
					usuarioModificacion.Value = codigo;
					areaModificacion.Value = Constants.ConstantesING.AREA_TRASPASO_FONDO;
					estadoAnterior.Value = Constants.ConstantesING.ESTADO_OPERACION_PENDIENTE;
					flagRescateProgramado.Value = Constants.ConstantesING.NO;
					fechaSolicitud.Value = fechaSolicitudRescate.ToString(Constants.ConstantesING.FORMATO);//DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//PONER FECHA Y HORA QUE CORRESPONDE PARA EL SIGUIENTE CIERRE EN TESTING
					fechaPago.Value = fechaPagoRescate.ToString(Constants.ConstantesING.FORMATO);
					#endregion

					#region variables

					nroCuotas.Value = DBNull.Value;

					valorCuota.Value = DBNull.Value;
					idParticipe.Value = participeOrigen;
					idCtaOrigen.Value = DBNull.Value;
					prmIdFondo.Value = fondoOrigen;

					flagRescateTotal.Value = Constants.ConstantesING.NO;

					flagRescateEnCuotas.Value = Constants.ConstantesING.NO;


					monto.Value = montoFlujo;
					montoNeto.Value = montoFlujo;


					codigoPromotor.Value = promotor;
					if (codigoPromotor.Value.Equals(""))
						codigoPromotor.Value = DBNull.Value;

					codigoViaPago.Value = viaPago;
					codigoFormaPago.Value = formaPago;

					idCuentaBancaria.Value = idCuentaBan;

					fechaAbonoContable.Value = DBNull.Value;
					fechaAbonoDisponible.Value = DBNull.Value;
					importePago.Value = DBNull.Value;
					nroOperacionDeposito.Value = DBNull.Value;
					nroCheque.Value = DBNull.Value;
					nroCuentaCheque.Value = DBNull.Value;
					fechaCheque.Value = DBNull.Value;
					codigoBancoCheque.Value = DBNull.Value;
					fechaProceso.Value = fechaPrecierre.ToString(Constants.ConstantesING.FORMATO);
					//OT10082 PSC002 INI
					idFondoOrigen.Value = DBNull.Value;//fondoOrigen;
					idParticipeOrigen.Value = DBNull.Value;//participeOrigen;
					//OT10082 PSC002 FIN
					numeroCertificado.Value = DBNull.Value;

					#endregion
					cmd.ExecuteNonQuery();
					string result = idOperacion.Value.ToString();
					return result;//result;

				}
				//OT10082 PSC001 FIN

        
         /// <summary>
        /// Cargar abonos masivos
        /// </summary>
        public string  CargarAbonosMasivo( AbonoMasivo abo, string usuario)
        {
           
                
                #region Obtener las conexiones

                //Depositos
       

                SqlCommand cmd = new SqlCommand("dbo.INGF_INS_DEPOSITO_BWS", cnDepositos, transDeposiTos);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmIdBanco = cmd.Parameters.Add("@idBanco", SqlDbType.Int);
                prmIdBanco.Value = Constants.ConstantesING.ID_FONDO_ABONO_MASIVO;

                SqlParameter prmIdRecepcion = cmd.Parameters.Add("@idRecepcion", SqlDbType.Int);
                prmIdRecepcion.Value = DBNull.Value;

                SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Int);
                prmIdFondo.Value = abo.IdFondo;

                SqlParameter prmNumeroOperacion = cmd.Parameters.Add("@numeroOperacion", SqlDbType.VarChar, 20);
                prmNumeroOperacion.Value = abo.NumOperacionBancaria;

                SqlParameter prmParticipe = cmd.Parameters.Add("@participe", SqlDbType.Int);
                prmParticipe.Value = abo.Cuc;

                
                SqlParameter prmFechaContable = cmd.Parameters.Add("@fechaContable", SqlDbType.DateTime);
                prmFechaContable.Value = abo.FechaContable.ToShortDateString();

                SqlParameter prmFechaDisponible = cmd.Parameters.Add("@fechaDisponible", SqlDbType.DateTime);
                prmFechaDisponible.Value = abo.FechaDisponible.ToShortDateString(); 

                SqlParameter prmMonto = cmd.Parameters.Add("@monto", SqlDbType.Money);
                prmMonto.Value = abo.Monto;
                
                SqlParameter prmIdTipoPago = cmd.Parameters.Add("@idTipoPago", SqlDbType.Char, 3);
                prmIdTipoPago.Value = Constants.ConstantesING.TIPO_PAGO_ABONO_MASIVO;

                SqlParameter prmReferencia = cmd.Parameters.Add("@referencia", SqlDbType.VarChar, 50);
                prmReferencia.Value = abo.Referencia;

                SqlParameter prmAcreditado = cmd.Parameters.Add("@acreditado", SqlDbType.Char, 1);
                prmAcreditado.Value = Constants.ConstantesING.SI;

                SqlParameter prmDisponible = cmd.Parameters.Add("@disponible", SqlDbType.Char, 1);
                prmDisponible.Value = Constants.ConstantesING.SI;

                SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 20);
                prmUsuario.Value = usuario;

          
                SqlParameter prmCodigoAgencia = cmd.Parameters.Add("@CODIGO_AGENCIA", SqlDbType.VarChar, 20);
                prmCodigoAgencia.Value = DBNull.Value;

                SqlParameter prmIdDeposito = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmIdDeposito.Direction = ParameterDirection.ReturnValue;


                cmd.ExecuteNonQuery();
              //  transDeposiTos.Commit();
                string result = prmIdDeposito.Value.ToString();
                return result;

               
           

        }
                #endregion


        #region
        /// <summary>
        /// Cargar abonos masivos obtener fondo
        /// </summary>
        public DataTable ObtenerFondosOperaciones()
        {
            
                SqlCommand cmdC = null;
                cmdC = new SqlCommand("dbo.FOND_LIS_FONDO", conn, trans);

                cmdC.CommandType = CommandType.StoredProcedure;
                SqlParameter prmEstado = cmdC.Parameters.Add("@estado", SqlDbType.VarChar, 3);
                prmEstado.Value = "ACT";


                SqlDataAdapter da = new SqlDataAdapter(cmdC);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

           
        }
        #endregion

        #region
        /// <summary>
        /// Listar cuentas bancarias
        /// </summary>
        public DataTable ListarCuentasBancarias(int idParticipe)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_CUENTA_BANCARIA_XPARTICIPE", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
            prmParticipe.Value = idParticipe;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        #endregion

        #region
        ///<sumary>
        ///Reversión - Actualizar Traspaso Fondos
        ///</sumary>
        public void ActualizarTraspasoFondo(string codigoUsuario, int idOperacion)
        {
            SqlCommand cmd = new SqlCommand("dbo.INGF_ACT_TRASPASO_FONDOS_CARGA_MASIVA", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

           
                cmd.Parameters.Add("@idOperacion", SqlDbType.Int).Value = idOperacion;
                cmd.Parameters.Add("@codigoUsuario", SqlDbType.VarChar).Value = codigoUsuario;


                cmd.ExecuteNonQuery();
           
 
        }
        
        #endregion



        #region
        ///<sumary>
        ///Reversión - Actualizar Operacion
        ///</sumary>
        public void ActualizarOperacion(string codigoUsuario, int idOperacion)
        {
            SqlCommand cmd = new SqlCommand("dbo.INGF_ACT_OPERACION_CARGA_MASIVA", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure; 

           
                cmd.Parameters.Add("@idOperacion", SqlDbType.Int).Value = idOperacion;
                cmd.Parameters.Add("@codigoUsuario", SqlDbType.VarChar).Value = codigoUsuario;


                cmd.ExecuteNonQuery();
           


        }

        #endregion


        #region
        ///<sumary>
        ///Reversión - Actualizar Depósito
        ///</sumary>
        public void ActualizarDeposito(string codigoUsuario, int idOperacion)
        {
            #region Obtener las conexiones
     

            #endregion

            SqlCommand cmd = new SqlCommand("dbo.INGF_ACT_DEPOSITO_CARGA_MASIVA", cnDepositos, transDeposiTos);
            cmd.CommandType = CommandType.StoredProcedure;

           
            cmd.Parameters.Add("@idOperacion", SqlDbType.Int).Value = idOperacion;
            cmd.Parameters.Add("@codigoUsuario", SqlDbType.VarChar).Value = codigoUsuario;


            cmd.ExecuteNonQuery();
            //transDeposiTos.Commit();

           
           

        }

        #endregion


        //PSC-001 OT8954 INI 
        #region
        /// <summary>
        /// Cargar Traspaso Masivo
        /// </summary>
        public string InsertarTraspasoMasivo(OperacionMasivo tpf, string codigoUsuario)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_INS_OPERACION_RESCATE", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            #region parametros
            SqlParameter tipoOperacion = cmd.Parameters.Add("@tipoOperacion", SqlDbType.VarChar);
            SqlParameter estado = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
            SqlParameter nroCuotas = cmd.Parameters.Add("@nroCuotas", SqlDbType.Decimal);
            SqlParameter valorCuota = cmd.Parameters.Add("@valorCuota", SqlDbType.Decimal);
            SqlParameter fechaOperacion = cmd.Parameters.Add("@fechaOperacion", SqlDbType.DateTime);
            SqlParameter idParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
            SqlParameter idCtaOrigen = cmd.Parameters.Add("@idCtaOrigen", SqlDbType.Decimal);
            SqlParameter idCtaDestino = cmd.Parameters.Add("@idCtaDestino", SqlDbType.Decimal);
            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            SqlParameter monto = cmd.Parameters.Add("@monto", SqlDbType.Decimal);
            SqlParameter montoNeto = cmd.Parameters.Add("@montoNeto", SqlDbType.Decimal);
            SqlParameter codigoPromotor = cmd.Parameters.Add("@codigoPromotor", SqlDbType.VarChar);
            SqlParameter codigoViaPago = cmd.Parameters.Add("@codigoViaPago", SqlDbType.VarChar);
            SqlParameter codigoFormaPago = cmd.Parameters.Add("@codigoFormaPago", SqlDbType.VarChar);
            SqlParameter fechaAbonoContable = cmd.Parameters.Add("@fechaAbonoContable", SqlDbType.DateTime);
            SqlParameter fechaAbonoDisponible = cmd.Parameters.Add("@fechaAbonoDisponible", SqlDbType.DateTime);
            SqlParameter importePago = cmd.Parameters.Add("@importePago", SqlDbType.Decimal);
            SqlParameter nroOperacionDeposito = cmd.Parameters.Add("@nroOperacionDeposito", SqlDbType.VarChar);
            SqlParameter nroCheque = cmd.Parameters.Add("@nroCheque", SqlDbType.VarChar);
            SqlParameter nroCuentaCheque = cmd.Parameters.Add("@nroCuentaCheque", SqlDbType.VarChar);
            SqlParameter fechaCheque = cmd.Parameters.Add("@fechaCheque", SqlDbType.DateTime);
            SqlParameter codigoBancoCheque = cmd.Parameters.Add("@codigoBancoCheque", SqlDbType.VarChar);
            SqlParameter flagRescateTotal = cmd.Parameters.Add("@flagRescateTotal", SqlDbType.VarChar);
            SqlParameter flagRescateEnCuotas = cmd.Parameters.Add("@flagRescateEnCuotas", SqlDbType.VarChar);
            SqlParameter flagRescateSignificativo = cmd.Parameters.Add("@flagRescateSignificativo", SqlDbType.VarChar);
            SqlParameter fechaProceso = cmd.Parameters.Add("@fechaProceso", SqlDbType.DateTime);
            SqlParameter idCuentaBancaria = cmd.Parameters.Add("@idCuentaBancaria", SqlDbType.Decimal);
            SqlParameter fechaCargo = cmd.Parameters.Add("@fechaCargo", SqlDbType.DateTime);
            SqlParameter flagNivel1 = cmd.Parameters.Add("@flagNivel1", SqlDbType.VarChar);
            SqlParameter flagNivel2 = cmd.Parameters.Add("@flagNivel2", SqlDbType.VarChar);
            SqlParameter flagEliminado = cmd.Parameters.Add("@flagEliminado", SqlDbType.VarChar);
            SqlParameter adicional1 = cmd.Parameters.Add("@adicional1", SqlDbType.VarChar);
            SqlParameter adicional2 = cmd.Parameters.Add("@adicional2", SqlDbType.VarChar);
            SqlParameter adicional3 = cmd.Parameters.Add("@adicional3", SqlDbType.VarChar);
            SqlParameter idOperacionReferencia = cmd.Parameters.Add("@idOperacionReferencia", SqlDbType.Decimal);
            SqlParameter idPreCierre = cmd.Parameters.Add("@idPreCierre", SqlDbType.Decimal);
            SqlParameter codigoViaSolicitud = cmd.Parameters.Add("@codigoViaSolicitud", SqlDbType.VarChar);
            SqlParameter idFondoDestino = cmd.Parameters.Add("@idFondoDestino", SqlDbType.Decimal);
            SqlParameter numeroSolicitud = cmd.Parameters.Add("@numeroSolicitud", SqlDbType.VarChar);
            SqlParameter idParticipeDestino = cmd.Parameters.Add("@idParticipeDestino", SqlDbType.Decimal);
            SqlParameter porcentajeComision = cmd.Parameters.Add("@porcentajeComision", SqlDbType.Decimal);
            SqlParameter comision = cmd.Parameters.Add("@comision", SqlDbType.Decimal);
            SqlParameter porcentajeComisionEspecial = cmd.Parameters.Add("@porcentajeComisionEspecial", SqlDbType.Decimal);
            SqlParameter comisionEspecial = cmd.Parameters.Add("@comisionEspecial", SqlDbType.Decimal);
            SqlParameter igv = cmd.Parameters.Add("@igv", SqlDbType.Decimal);
            SqlParameter flagProcesoComisiones = cmd.Parameters.Add("@flagProcesoComisiones", SqlDbType.VarChar);
            SqlParameter montoAjustadoBruto = cmd.Parameters.Add("@montoAjustadoBruto", SqlDbType.Decimal);
            SqlParameter montoAjustadoNeto = cmd.Parameters.Add("@montoAjustadoNeto", SqlDbType.Decimal);
            SqlParameter concepto = cmd.Parameters.Add("@concepto", SqlDbType.VarChar);
            SqlParameter usuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
            SqlParameter usuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            SqlParameter areaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            SqlParameter estadoAnterior = cmd.Parameters.Add("@estadoAnterior", SqlDbType.VarChar);
            SqlParameter idFondoOrigen = cmd.Parameters.Add("@idFondoOrigen", SqlDbType.Decimal);
            SqlParameter idParticipeOrigen = cmd.Parameters.Add("@idParticipeOrigen", SqlDbType.Decimal);
            SqlParameter numeroCertificado = cmd.Parameters.Add("@numeroCertificado", SqlDbType.VarChar);
            SqlParameter fechaSolicitud = cmd.Parameters.Add("@fechaSolicitud", SqlDbType.VarChar);
            SqlParameter fechaPago = cmd.Parameters.Add("@fechaPago", SqlDbType.VarChar);
            SqlParameter flagRescateProgramado = cmd.Parameters.Add("@flagRescateProgramado", SqlDbType.VarChar);
            SqlParameter nroOperacion = cmd.Parameters.Add("@nroOperacion", SqlDbType.Decimal);
            nroOperacion.Direction = ParameterDirection.Output;

            SqlParameter idOperacion = cmd.Parameters.Add("@idOperacion", SqlDbType.VarChar);
            idOperacion.Direction = ParameterDirection.ReturnValue;
            #endregion

            #region Valores en Común de los rescates
            tipoOperacion.Value = Constants.ConstantesING.TIPO_OPERACION_TRASPASO;
            estado.Value = Constants.ConstantesING.ESTADO_OPERACION_PENDIENTE;
            fechaOperacion.Value = DateTime.Now.ToString(Constants.ConstantesING.FORMATO);
            idCtaDestino.Value = tpf.IdCuentaParticipacionDestinoTraspaso;
            flagRescateSignificativo.Value = DBNull.Value;
            idCuentaBancaria.Value = DBNull.Value;
            fechaCargo.Value = DBNull.Value;
            flagNivel1.Value = Constants.ConstantesING.SI;
            flagNivel2.Value = Constants.ConstantesING.NO;
            flagEliminado.Value = Constants.ConstantesING.NO;
            adicional1.Value = Constants.ConstantesING.NO;
            adicional2.Value = DBNull.Value;
            adicional3.Value = DBNull.Value;
            idOperacionReferencia.Value = DBNull.Value;
            idPreCierre.Value = DBNull.Value;
            codigoViaSolicitud.Value = DBNull.Value;
            idFondoDestino.Value = DBNull.Value;
            numeroSolicitud.Value = DBNull.Value;
            idParticipeDestino.Value = tpf.IdParticipeDestinoTraspaso;
            porcentajeComision.Value = DBNull.Value;
            comision.Value = DBNull.Value;
            porcentajeComisionEspecial.Value = DBNull.Value;
            comisionEspecial.Value = DBNull.Value;
            igv.Value = DBNull.Value;
            flagProcesoComisiones.Value = DBNull.Value;
            montoAjustadoBruto.Value = DBNull.Value;
            montoAjustadoNeto.Value = DBNull.Value;
            concepto.Value = DBNull.Value;
            usuarioCreacion.Value = codigoUsuario;
            usuarioModificacion.Value = codigoUsuario;
            areaModificacion.Value = Constants.ConstantesING.AREA_OPERACION_TRASPASO;
            estadoAnterior.Value = DBNull.Value;
            flagRescateProgramado.Value = Constants.ConstantesING.NO;
            fechaSolicitud.Value = DBNull.Value;
            fechaPago.Value = DBNull.Value;
            #endregion

            #region variables
            if (tpf.Cuotas > 0)
            {
                nroCuotas.Value = tpf.Cuotas;
            }
            else
            {
                nroCuotas.Value = DBNull.Value;
            }
            valorCuota.Value = tpf.ValorCuota;
            idParticipe.Value = tpf.IdParticipeOrigenTraspaso;
            idCtaOrigen.Value = tpf.IdCuentaParticipacionOrigenTraspaso;
            prmIdFondo.Value = tpf.IdNombreFondoOrigenTraspaso;
            flagRescateTotal.Value = DBNull.Value;
            flagRescateEnCuotas.Value = DBNull.Value;
            monto.Value = tpf.Cuotas*tpf.ValorCuota;
            montoNeto.Value = tpf.Cuotas*tpf.ValorCuota;
            codigoPromotor.Value = tpf.CodigoPromotorOrigen;
            if (codigoPromotor.Value.Equals(""))
                codigoPromotor.Value = DBNull.Value;
            codigoViaPago.Value = DBNull.Value;
            codigoFormaPago.Value = DBNull.Value;
            fechaAbonoContable.Value = DBNull.Value;
            fechaAbonoDisponible.Value = DBNull.Value;
            importePago.Value = DBNull.Value;
            nroOperacionDeposito.Value = DBNull.Value;
            nroCheque.Value = DBNull.Value;
            nroCuentaCheque.Value = DBNull.Value;
            fechaCheque.Value = DBNull.Value;
            codigoBancoCheque.Value = DBNull.Value;
            fechaProceso.Value = tpf.FechaProceso.ToString(Constants.ConstantesING.FORMATO);
            idFondoOrigen.Value = DBNull.Value;
            idParticipeOrigen.Value = DBNull.Value;
            numeroCertificado.Value = DBNull.Value;

            #endregion
            cmd.ExecuteNonQuery();
            string result = idOperacion.Value.ToString();
            return result;

        }
        #endregion

        //PSC-001 OT8954 INI 

        //OT10217 INI
        public string InsertarConversionCuotasMasivo(OperacionMasivo cov, string codigoUsuario, decimal valorIgv, DateTime FechaSolicitudRescate, int idOpeReferencia)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_INS_OPERACION_RESCATE", conn, trans);
			cmd.CommandType = CommandType.StoredProcedure;

			#region parametros
			SqlParameter tipoOperacion = cmd.Parameters.Add("@tipoOperacion", SqlDbType.VarChar);
			SqlParameter estado = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
			SqlParameter nroCuotas = cmd.Parameters.Add("@nroCuotas", SqlDbType.Decimal);
			SqlParameter valorCuota = cmd.Parameters.Add("@valorCuota", SqlDbType.Decimal);
			SqlParameter fechaOperacion = cmd.Parameters.Add("@fechaOperacion", SqlDbType.DateTime);
			SqlParameter idParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
			SqlParameter idCtaOrigen = cmd.Parameters.Add("@idCtaOrigen", SqlDbType.Decimal);
			SqlParameter idCtaDestino = cmd.Parameters.Add("@idCtaDestino", SqlDbType.Decimal);
			SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			SqlParameter monto = cmd.Parameters.Add("@monto", SqlDbType.Decimal);
			SqlParameter montoNeto = cmd.Parameters.Add("@montoNeto", SqlDbType.Decimal);
			SqlParameter codigoPromotor = cmd.Parameters.Add("@codigoPromotor", SqlDbType.VarChar);
			SqlParameter codigoViaPago = cmd.Parameters.Add("@codigoViaPago", SqlDbType.VarChar);
			SqlParameter codigoFormaPago = cmd.Parameters.Add("@codigoFormaPago", SqlDbType.VarChar);
			SqlParameter fechaAbonoContable = cmd.Parameters.Add("@fechaAbonoContable", SqlDbType.DateTime);
			SqlParameter fechaAbonoDisponible = cmd.Parameters.Add("@fechaAbonoDisponible", SqlDbType.DateTime);
			SqlParameter importePago = cmd.Parameters.Add("@importePago", SqlDbType.Decimal);
			SqlParameter nroOperacionDeposito = cmd.Parameters.Add("@nroOperacionDeposito", SqlDbType.VarChar);
			SqlParameter nroCheque = cmd.Parameters.Add("@nroCheque", SqlDbType.VarChar);
			SqlParameter nroCuentaCheque = cmd.Parameters.Add("@nroCuentaCheque", SqlDbType.VarChar);
			SqlParameter fechaCheque = cmd.Parameters.Add("@fechaCheque", SqlDbType.DateTime);
			SqlParameter codigoBancoCheque = cmd.Parameters.Add("@codigoBancoCheque", SqlDbType.VarChar);
			SqlParameter flagRescateTotal = cmd.Parameters.Add("@flagRescateTotal", SqlDbType.VarChar);
			SqlParameter flagRescateEnCuotas = cmd.Parameters.Add("@flagRescateEnCuotas", SqlDbType.VarChar);
			SqlParameter flagRescateSignificativo = cmd.Parameters.Add("@flagRescateSignificativo", SqlDbType.VarChar);
			SqlParameter fechaProceso = cmd.Parameters.Add("@fechaProceso", SqlDbType.DateTime);
			SqlParameter idCuentaBancaria = cmd.Parameters.Add("@idCuentaBancaria", SqlDbType.Decimal);
			SqlParameter fechaCargo = cmd.Parameters.Add("@fechaCargo", SqlDbType.DateTime);
			SqlParameter flagNivel1 = cmd.Parameters.Add("@flagNivel1", SqlDbType.VarChar);
			SqlParameter flagNivel2 = cmd.Parameters.Add("@flagNivel2", SqlDbType.VarChar);
			SqlParameter flagEliminado = cmd.Parameters.Add("@flagEliminado", SqlDbType.VarChar);
			SqlParameter adicional1 = cmd.Parameters.Add("@adicional1", SqlDbType.VarChar);
			SqlParameter adicional2 = cmd.Parameters.Add("@adicional2", SqlDbType.VarChar);
			SqlParameter adicional3 = cmd.Parameters.Add("@adicional3", SqlDbType.VarChar);
			SqlParameter idOperacionReferencia = cmd.Parameters.Add("@idOperacionReferencia", SqlDbType.Decimal);
			SqlParameter idPreCierre = cmd.Parameters.Add("@idPreCierre", SqlDbType.Decimal);
			SqlParameter codigoViaSolicitud = cmd.Parameters.Add("@codigoViaSolicitud", SqlDbType.VarChar);
			SqlParameter idFondoDestino = cmd.Parameters.Add("@idFondoDestino", SqlDbType.Decimal);
			SqlParameter numeroSolicitud = cmd.Parameters.Add("@numeroSolicitud", SqlDbType.VarChar);
			SqlParameter idParticipeDestino = cmd.Parameters.Add("@idParticipeDestino", SqlDbType.Decimal);
			SqlParameter porcentajeComision = cmd.Parameters.Add("@porcentajeComision", SqlDbType.Decimal);
			SqlParameter comision = cmd.Parameters.Add("@comision", SqlDbType.Decimal);
			SqlParameter porcentajeComisionEspecial = cmd.Parameters.Add("@porcentajeComisionEspecial", SqlDbType.Decimal);
			SqlParameter comisionEspecial = cmd.Parameters.Add("@comisionEspecial", SqlDbType.Decimal);
			SqlParameter igv = cmd.Parameters.Add("@igv", SqlDbType.Decimal);
			SqlParameter flagProcesoComisiones = cmd.Parameters.Add("@flagProcesoComisiones", SqlDbType.VarChar);
			SqlParameter montoAjustadoBruto = cmd.Parameters.Add("@montoAjustadoBruto", SqlDbType.Decimal);
			SqlParameter montoAjustadoNeto = cmd.Parameters.Add("@montoAjustadoNeto", SqlDbType.Decimal);
			SqlParameter concepto = cmd.Parameters.Add("@concepto", SqlDbType.VarChar);
			SqlParameter usuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
			SqlParameter usuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
			SqlParameter areaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
			SqlParameter estadoAnterior = cmd.Parameters.Add("@estadoAnterior", SqlDbType.VarChar);
			SqlParameter idFondoOrigen = cmd.Parameters.Add("@idFondoOrigen", SqlDbType.Decimal);
			SqlParameter idParticipeOrigen = cmd.Parameters.Add("@idParticipeOrigen", SqlDbType.Decimal);
			SqlParameter numeroCertificado = cmd.Parameters.Add("@numeroCertificado", SqlDbType.VarChar);
			SqlParameter fechaSolicitud = cmd.Parameters.Add("@fechaSolicitud", SqlDbType.VarChar);
			SqlParameter fechaPago = cmd.Parameters.Add("@fechaPago", SqlDbType.VarChar);
			SqlParameter flagRescateProgramado = cmd.Parameters.Add("@flagRescateProgramado", SqlDbType.VarChar);

            SqlParameter nroOperacion = cmd.Parameters.Add("@nroOperacion", SqlDbType.Decimal);
            nroOperacion.Direction = ParameterDirection.Output;

            SqlParameter idOperacion = cmd.Parameters.Add("@idOperacion", SqlDbType.VarChar);
            idOperacion.Direction = ParameterDirection.ReturnValue;
			#endregion

		
            #region Valor parametros
            tipoOperacion.Value = Constants.ConstantesING.TIPO_OPERACION_CONVERSION_CUOTAS;
            estado.Value = Constants.ConstantesING.ESTADO_OPERACION_CONFIRMADO;
            nroCuotas.Value = DBNull.Value;
            valorCuota.Value = DBNull.Value;
            fechaOperacion.Value = DateTime.Now.ToString(Constants.ConstantesING.FORMATO);
            idParticipe.Value = cov.IdParticipeOrigen;
            idCtaOrigen.Value = cov.IdCuentaParticipacionOrigenTraspaso;
            idCtaDestino.Value = cov.IdCuentaParticipacionDestinoTraspaso;
            prmIdFondo.Value = cov.IdFondo;
            monto.Value = cov.Monto;
            montoNeto.Value = cov.Monto;
            codigoPromotor.Value = DBNull.Value;
            codigoViaPago.Value = DBNull.Value;
            codigoFormaPago.Value = DBNull.Value;
            fechaAbonoContable.Value = DBNull.Value;
            fechaAbonoDisponible.Value = DBNull.Value;
            importePago.Value = DBNull.Value;
            nroOperacionDeposito.Value = DBNull.Value;
            nroCheque.Value = DBNull.Value;
            nroCuentaCheque.Value = DBNull.Value;
            fechaCheque.Value = DBNull.Value;
            codigoBancoCheque.Value = DBNull.Value;
            flagRescateTotal.Value = Constants.ConstantesING.NO;
            flagRescateEnCuotas.Value = Constants.ConstantesING.NO;
            flagRescateSignificativo.Value = Constants.ConstantesING.NO;
            fechaProceso.Value = cov.FechaProceso.ToString(Constants.ConstantesING.FORMATO);
            idCuentaBancaria.Value = DBNull.Value;
            fechaCargo.Value = DBNull.Value;
            flagNivel1.Value = Constants.ConstantesING.NO;
            flagNivel2.Value = Constants.ConstantesING.NO;
            flagEliminado.Value = Constants.ConstantesING.NO;
            adicional1.Value = DBNull.Value;
            adicional2.Value = DBNull.Value;
            adicional3.Value = DBNull.Value;
            if (idOpeReferencia == 0)
            {
                idOperacionReferencia.Value = DBNull.Value;
            }
            else
            {
                idOperacionReferencia.Value = idOpeReferencia;
            }
            idPreCierre.Value = DBNull.Value;
            codigoViaSolicitud.Value = DBNull.Value;
            idFondoDestino.Value = cov.IdFondoDestino;
            numeroSolicitud.Value = DBNull.Value;
            idParticipeDestino.Value = cov.IdParticipeDestino;
            porcentajeComision.Value = 0;
            comision.Value = 0;
            porcentajeComisionEspecial.Value = DBNull.Value;
            comisionEspecial.Value = DBNull.Value;
            igv.Value = valorIgv;
            flagProcesoComisiones.Value = Constants.ConstantesING.NO;
            montoAjustadoBruto.Value = DBNull.Value;
            montoAjustadoNeto.Value = DBNull.Value;
            concepto.Value = DBNull.Value;
            usuarioCreacion.Value = codigoUsuario;
            usuarioModificacion.Value = codigoUsuario;
            areaModificacion.Value = Constants.ConstantesING.AREA_OPERACION_CONVERSION_CUOTAS;
            estadoAnterior.Value = Constants.ConstantesING.ESTADO_OPERACION_CONFIRMADO;
            idFondoOrigen.Value = cov.IdFondoOrigen;
            idParticipeOrigen.Value = cov.IdParticipeOrigen;
            numeroCertificado.Value = DBNull.Value;
            fechaSolicitud.Value = FechaSolicitudRescate.ToString(Constants.ConstantesING.FORMATO);//modifacion día menos
            fechaPago.Value = DBNull.Value;
            flagRescateProgramado.Value = Constants.ConstantesING.NO;

            #endregion 

            cmd.ExecuteNonQuery();
            string result = idOperacion.Value.ToString();
			return result;
		}
        //OT10217 FIN

				//OT10433 INI
        public string CorrelativoCuentaParticipacion(string idParticipe)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_CORRELATIVO_CUENTA_PARTICIPACION", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdParticipe = cmd.Parameters.Add("@IdParticipe", SqlDbType.Int);
            prmIdParticipe.Value = idParticipe;

            cmd.ExecuteNonQuery();
            String correlativo = cmd.ExecuteScalar().ToString();
            return correlativo;
        }

        public int InsertarCuentasParticipacionMasivo(CuentaParticipacionMasivo cpm, string correlativoCodigoIdParticipe, string codigoUsuario)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_INS_CUENTA_PARTICIPACION_XPARTICIPE", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            #region parametros
            SqlParameter codigo = cmd.Parameters.Add("@codigo", SqlDbType.VarChar);
            SqlParameter etiqueta = cmd.Parameters.Add("@etiqueta", SqlDbType.VarChar);
            SqlParameter flagPredeterminado = cmd.Parameters.Add("@flagPredeterminado", SqlDbType.VarChar);
            SqlParameter idParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Int);
            SqlParameter usuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
            SqlParameter usuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            SqlParameter areaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            SqlParameter porcentajeParticipacion = cmd.Parameters.Add("@porcentajeParticipacion", SqlDbType.Decimal);
            SqlParameter idParticipeAsociado = cmd.Parameters.Add("@idParticipeAsociado", SqlDbType.Int);
            SqlParameter flagCuotasGarantia = cmd.Parameters.Add("@flagCuotasGarantia", SqlDbType.VarChar);
            SqlParameter codigoPlan = cmd.Parameters.Add("@codigoPlan", SqlDbType.VarChar);

            SqlParameter idCntParticipacion = cmd.Parameters.Add("@idCntParticipacion", SqlDbType.VarChar);
            idCntParticipacion.Direction = ParameterDirection.ReturnValue;
            #endregion

            #region Prueba parametros
            codigo.Value = correlativoCodigoIdParticipe;
            etiqueta.Value = cpm.Etiqueta;
            flagPredeterminado.Value = Constants.ConstantesING.NO;
            idParticipe.Value = cpm.IdParticipe;
            usuarioCreacion.Value = codigoUsuario;
            usuarioModificacion.Value = codigoUsuario;
            areaModificacion.Value = Constants.ConstantesING.AREA_CUENTA_PARTICIPACION;
            porcentajeParticipacion.Value = DBNull.Value;
            idParticipeAsociado.Value = DBNull.Value;
						flagCuotasGarantia.Value = cpm.FlagBloqueada;

						string codigoPlanCadena = cpm.CodigoPlan.Trim();
						if (codigoPlanCadena == string.Empty)
						{
								codigoPlan.Value = DBNull.Value;
						}
						else {
								codigoPlan.Value = codigoPlanCadena;
						}
            #endregion

            cmd.ExecuteNonQuery();
            int result = Convert.ToInt32(idCntParticipacion.Value.ToString());
            return result;
        }

        public void ActualizarCuentaParticipacion(string codigoUsuario, int idCuentaParticipacion)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_ELI_CUENTA_PARTICIPACION_XPARTICIPE", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@idCuentaParticipacion", SqlDbType.Int).Value = idCuentaParticipacion;
            cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar).Value = codigoUsuario;
            cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar).Value = Constants.ConstantesING.AREA_CUENTA_PARTICIPACION;
            cmd.ExecuteNonQuery();
        }
				//OT10433 FIN
    }
}

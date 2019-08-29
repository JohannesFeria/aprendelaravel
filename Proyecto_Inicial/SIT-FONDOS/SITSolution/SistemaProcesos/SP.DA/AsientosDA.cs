/*  -------------------------------------------------------------
 *  Fecha de modificación: 21/01/2015
 *  Modificado por: Juan Castro
 *  Nro. de Orden de Trabajo: 6986 
 *  Descripción del cambio: Creación
 *  -------------------------------------------------------------
 * */
/*  -------------------------------------------------------------
 *  Fecha de modificación: 24/11/2015
 *  Modificado por: Robert Castillo
 *  Nro. de Orden de Trabajo: 7999 
 *  Descripción del cambio: Se agrega el método ObtenerTipoCambio.
 *  -------------------------------------------------------------
 * */
/*  -------------------------------------------------------------
 *  Fecha de modificación: 04/12/2015
 *  Modificado por: Richard Valdez
 *  Nro. de Orden de Trabajo: 7940-PSC002
 *  Descripción del cambio: Se invoca al procedure FMPR_OBT_TIPO_CAMBIO
 *                          en el método ObtenerTipoCambio.
 *  -------------------------------------------------------------
 * */
using System;
using System.Collections.Generic;
using System.Text;
using INGFondos.Data;
using System.Data.SqlClient;
using System.Data;
using Procesos.TD;
//OT 7999 INI
using System.Configuration;
//OT 7999 FIN

namespace Procesos.DA
{
    public class AsientosDA : INGFondos.Data.DA
    {
        public AsientosDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        /// <summary>
        /// Obtiene una conexión de base de datos para trabajar con transacciones desde la capa de negocio.
        /// </summary>
        /// <returns>SqlConnection: Conexión a la base de datos.</returns>
        public SqlConnection GetConnection2()
        {
            return GetConnection();
        }

        /// <summary>
        /// Obtiene los registros de asientos tipo PD de retenciones generadas.
        /// </summary>
        /// <param name="dtAsientos">Tabla en la que se volcará la información de los asientos a generar.</param>
        /// <param name="fechaInicio">Fecha de inicio de la consulta.</param>
        /// <param name="fechaFin">Fecha de fin de la consulta.</param>
        public void ObtenerAsientosPDRetenciones(Procesos.TD.Asiento.LISTA_ASIENTODataTable dtAsientos, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                SqlConnection cn = GetConnection();
                SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_RETENCIONES_X_MONEDA", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000;

                SqlParameter prmFechaInicio = cmd.Parameters.Add("@fechaInicio", SqlDbType.Date);
                prmFechaInicio.Value = fechaInicio;

                SqlParameter prmFechaFin = cmd.Parameters.Add("@fechaFin", SqlDbType.Date);
                prmFechaFin.Value = fechaFin;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    dtAsientos.ImportRow(dr);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene los asientos generados de un tipo de asiento determinado y completa las información de los UUID y estados registrados.
        /// </summary>
        /// <param name="dtAsientos">Tabla que contiene la data de los asientos a generar.</param>
        /// <param name="tipoAsiento">Indica el tipo de asiento que se desea verificar su generación previa.</param>
        /// <param name="fecha">Indica la fecha de consulta de los asientos.</param>
        public void ListarControlAsiento(Procesos.TD.Asiento.LISTA_ASIENTODataTable dtAsientos, string tipoAsiento, DateTime fecha)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_CONTROL_ASIENTO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;

            SqlParameter prmTipoAsiento = cmd.Parameters.Add("@tipoAsiento", SqlDbType.VarChar);
            prmTipoAsiento.Value = tipoAsiento;

            SqlParameter prmFechaInicio = cmd.Parameters.Add("@fechaInicio", SqlDbType.Date);
            prmFechaInicio.Value = fecha;

            SqlParameter prmFechaFin = cmd.Parameters.Add("@fechaFin", SqlDbType.Date);
            prmFechaFin.Value = fecha;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                DataRow[] drAsientos = dtAsientos.Select("MONEDA = '" + dr["MONEDA"] + "' AND FECHA_PROCESO = '#" + fecha.ToString("dd/MM/yyyy") + "#' AND FONDO = '" + dr["FONDO"] + "'");
                drAsientos[0]["UUID"] = dr["UUID"];
                drAsientos[0]["ESTADO"] = dr["ESTADO"];
                drAsientos[0]["DESCRIPCION_ERROR"] = dr["DESCRIPCION_ERROR"];
            }
        }

        /// <summary>
        /// Obtiene los registros de asientos tipo VT y PD de comisiones anticipadas.
        /// </summary>
        /// <param name="dtAsientos">Tabla en la que se volcará la información de los asientos a generar.</param>
        /// <param name="fechaInicio">Fecha de inicio de la consulta.</param>
        /// <param name="fechaFin">Fecha de fin de la consulta.</param>
        public void ObtenerAsientosVTComisiones(Procesos.TD.Asiento.LISTA_ASIENTODataTable dtAsientos, DateTime fechaInicio, DateTime fechaFin, bool sapActivo)
        {
            try
            {
                SqlConnection cn = GetConnection();
                SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_COMISIONES_ANTICIPADAS", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000;

                SqlParameter prmFechaInicio = cmd.Parameters.Add("@fechaInicio", SqlDbType.Date);
                prmFechaInicio.Value = fechaInicio;

                SqlParameter prmFechaFin = cmd.Parameters.Add("@fechaFin", SqlDbType.Date);
                prmFechaFin.Value = fechaFin;

                SqlParameter prmSapActivo = cmd.Parameters.Add("@SAP_ACTIVO", SqlDbType.VarChar);
                prmSapActivo.Value = sapActivo ? "S" : "N";
                
                //SqlParameter prmBoletaTexto = cmd.Parameters.Add("@boletaTexto", SqlDbType.VarChar);
                //prmBoletaTexto.Value = boletaTexto;

                //SqlParameter prmBoletaNumero = cmd.Parameters.Add("@boletaNumero", SqlDbType.VarChar);
                //prmBoletaNumero.Value = boletaNumero;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    dtAsientos.ImportRow(dr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene las cabeceras de los asiento tipo registrados en la BD.
        /// </summary>
        /// <param name="cn">Objeto que representa la conexión a la BD.</param>
        /// <param name="trans">Objeto que representa la transacción con la que se trabaja el proceso de registro de los asientos.</param>
        /// <returns>DataTable: Contiene los registros de las cabeceras de los asientos tipo</returns>
        public DataTable ObtenerAsientoTipoCabecera(SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_ASIENTO_TIPO", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// Obtiene los movimientos de los asiento tipo registrados en la BD.
        /// </summary>
        /// <param name="cn">Objeto que representa la conexión a la BD.</param>
        /// <param name="trans">Objeto que representa la transacción con la que se trabaja el proceso de registro de los asientos.</param>
        /// <returns>DataTable: Contiene los registros de los movimientos de los asientos tipo</returns>
        public DataTable ObtenerAsientoTipoDetalle(SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_ASIENTO_TIPO_DETALLE", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// Registra la información de control de transacción de un asiento que se genera.
        /// </summary>
        /// <param name="usuario">Usuario responsable de la generación del asiento.</param>
        /// <param name="ip">Dirección IP desde donde se genera el asiento.</param>
        /// <param name="referencia">Indica un valor que puede servir para dar trazabilidad al asiento</param>
        /// <param name="cn">Objeto que representa la conexión a la BD.</param>
        /// <param name="trans">Objeto que representa la transacción con la cual se realiza el proceso de registro.</param>
        /// <returns></returns>
        //public string RegistrarControl(string usuario, string ip, string referencia, SqlConnection cn, SqlTransaction trans)
        // string uuid = da.RegistrarControl(usuario, direccionIP, referencia,cantidadSubDetalle, datosAsiento.TIPO_ASIENTO, cn, trans);
        public string RegistrarControl(string usuario, string ip, string referencia, decimal cantidadSubDetalle, string asientoOrigen, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_CONTROL", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
                prmUsuario.Value = usuario;

                SqlParameter prmIP = cmd.Parameters.Add("@ip", SqlDbType.VarChar);
                prmIP.Value = ip;

                SqlParameter prmReferencia = cmd.Parameters.Add("@referencia", SqlDbType.VarChar);
                prmReferencia.Value = referencia;

                SqlParameter prmCodigo = cmd.Parameters.Add("@codigoUnico", SqlDbType.VarChar,36);
                prmCodigo.Direction = ParameterDirection.Output;

                SqlParameter prmCantidadSubDetalle = cmd.Parameters.Add("@cantidadSubDetalle", SqlDbType.Decimal);
                prmCantidadSubDetalle.Value = cantidadSubDetalle;

                SqlParameter prmAsientoOrigen = cmd.Parameters.Add("@asientoOrigen", SqlDbType.VarChar,11);
                prmAsientoOrigen.Value = asientoOrigen;

                cmd.ExecuteNonQuery();

                return cmd.Parameters["@codigoUnico"].Value.ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Dispose(); }

        }

        /// <summary>
        /// Registra la información de cabecera del asiento contable.
        /// </summary>
        /// <param name="sapar1Row">Objeto que contiene la información de la cabecera del asiento que se desea registrar.</param>
        /// <param name="cn">Objeto que representa la conexión a la BD.</param>
        /// <param name="trans">Objeto que representa la transacción con la cual se realiza el proceso de registro.</param>
        public void RegistrarCabeceraAsiento(Procesos.TD.Asiento.SAPAR1Row sapar1Row, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_CABECERA_ASIENTO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trans;
            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@UUID", SqlDbType.VarChar);
                prmCodigo.Value = sapar1Row.SAPUUI;

                SqlParameter prmNumeroDocumentoOrigen = cmd.Parameters.Add("@numeroDocumentoOrigen", SqlDbType.VarChar);
                prmNumeroDocumentoOrigen.Value = sapar1Row.SAPNU5;

                SqlParameter prmClaseDocumento = cmd.Parameters.Add("@claseDocumento", SqlDbType.VarChar);
                prmClaseDocumento.Value = sapar1Row.SAPCLA;

                SqlParameter prmSociedadFI = cmd.Parameters.Add("@sociedadFI", SqlDbType.VarChar);
                prmSociedadFI.Value = sapar1Row.SAPSOC;

                SqlParameter prmMonedaDocumento = cmd.Parameters.Add("@monedaDocumento", SqlDbType.VarChar);
                prmMonedaDocumento.Value = sapar1Row.SAPMON;

                SqlParameter prmFechaContabilizacion = cmd.Parameters.Add("@fechaContabilizacion", SqlDbType.VarChar);
                prmFechaContabilizacion.Value = sapar1Row.SAPFEC;

                SqlParameter prmFechaDocumento = cmd.Parameters.Add("@fechaDocumento", SqlDbType.VarChar);
                prmFechaDocumento.Value = sapar1Row.SAPFE1;

                SqlParameter prmFechaConversion = cmd.Parameters.Add("@fechaConversion", SqlDbType.VarChar);
                if (sapar1Row.SAPFE2 == "")
                {
                    prmFechaConversion.Value = DBNull.Value;
                }
                else
                {
                    prmFechaConversion.Value = sapar1Row.SAPFE2;
                }

                SqlParameter prmReferencia = cmd.Parameters.Add("@referencia", SqlDbType.VarChar);
                prmReferencia.Value = sapar1Row.SAPREF;

                SqlParameter prmTexto = cmd.Parameters.Add("@textoCabecera", SqlDbType.VarChar);
                prmTexto.Value = sapar1Row.SAPTEX;

                SqlParameter prmTipoCambio = cmd.Parameters.Add("@tipoCambio", SqlDbType.Decimal);
                if (sapar1Row.SAPTAS == 0)
                {
                    prmTipoCambio.Value = DBNull.Value;
                }
                else
                {
                    prmTipoCambio.Value = sapar1Row.SAPTAS;
                }

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Dispose(); }
        }

        /// <summary>
        /// Registra la información de un movimiento del asiento que se registra.
        /// </summary>
        /// <param name="sapar2Row">Objeto que contiene la información del movimiento del asiento que se desea registrar.</param>
        /// <param name="cn">Objeto que representa la conexión a la BD.</param>
        /// <param name="trans">Objeto que representa la transacción con la cual se realiza el proceso de registro.</param>
        public void RegistrarDetalleAsiento(Procesos.TD.Asiento.SAPAR2Row sapar2Row, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_DETALLE_ASIENTO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trans;
            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@UUID", SqlDbType.VarChar);
                prmCodigo.Value = sapar2Row.SAPU06;

                SqlParameter prmSociedadFI = cmd.Parameters.Add("@sociedadFI", SqlDbType.VarChar);
                prmSociedadFI.Value = sapar2Row.SAPSO5;

                SqlParameter prmNumeroDocumentoOrigen = cmd.Parameters.Add("@numeroDocumentoOrigen", SqlDbType.VarChar);
                prmNumeroDocumentoOrigen.Value = sapar2Row.SAPN15;

                SqlParameter prmNumeroPosicion = cmd.Parameters.Add("@numeroPosicion", SqlDbType.Int);
                prmNumeroPosicion.Value = sapar2Row.SAPNU7;

                SqlParameter prmTipoMovimiento = cmd.Parameters.Add("@tipoMovimiento", SqlDbType.VarChar);
                prmTipoMovimiento.Value = sapar2Row.SAPTI4;

                SqlParameter prmImporte = cmd.Parameters.Add("@importeMonedaDoc", SqlDbType.Decimal);
                prmImporte.Value = sapar2Row.SAPIMP;

                SqlParameter prmTexto = cmd.Parameters.Add("@textoPosicion", SqlDbType.VarChar);
                prmTexto.Value = sapar2Row.SAPTE1;

                SqlParameter prmAsignacion = cmd.Parameters.Add("@asignacion", SqlDbType.VarChar);
                prmAsignacion.Value = sapar2Row.SAPASI;

                SqlParameter prmReferencia1 = cmd.Parameters.Add("@referencia1", SqlDbType.VarChar);
                prmReferencia1.Value = sapar2Row.SAPRE3;

                SqlParameter prmReferencia3 = cmd.Parameters.Add("@referencia3", SqlDbType.VarChar);
                prmReferencia3.Value = sapar2Row.SAPR04;

                SqlParameter prmCuenta = cmd.Parameters.Add("@cuenta", SqlDbType.VarChar);
                prmCuenta.Value = sapar2Row.SAPCUE;

                SqlParameter prmTipoObjetoImputacion = cmd.Parameters.Add("@tipoObjetoImputacion", SqlDbType.VarChar);
                prmTipoObjetoImputacion.Value = sapar2Row.SAPTI5;

                SqlParameter prmObjetoImputacion = cmd.Parameters.Add("@objetoImputacion", SqlDbType.VarChar);
                prmObjetoImputacion.Value = sapar2Row.SAPOBJ;

                SqlParameter prmIndicadorIGV = cmd.Parameters.Add("@indicadorIGV", SqlDbType.VarChar);
                prmIndicadorIGV.Value = sapar2Row.SAPIND;

                SqlParameter prmFechaValor = cmd.Parameters.Add("@fechaValor", SqlDbType.VarChar);
                prmFechaValor.Value = sapar2Row.SAPFE3;

                SqlParameter prmCondicionPago = cmd.Parameters.Add("@condicionPago", SqlDbType.VarChar);
                prmCondicionPago.Value = sapar2Row.SAPCON;

                SqlParameter prmViaPago = cmd.Parameters.Add("@viaPago", SqlDbType.VarChar);
                prmViaPago.Value = sapar2Row.SAPVIA;

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Dispose(); }
        }

        /// <summary>
        /// Registra la información de control interno en el legado para la generación de asientos
        /// </summary>
        /// <param name="UUID">Código único de registro del asiento.</param>
        /// <param name="datosAsiento">Objeto que contiene la información del asiento que se desea registrar.</param>
        /// <param name="usuario">Usuario responsable de la creación del asiento y de su respectivo control.</param>
        /// <param name="cn">Objeto que representa la conexión a la BD.</param>
        /// <param name="trans">Objeto que representa la transacción con la cual se realiza el proceso de registro.</param>
        public void RegistrarControlAsiento(string UUID, Asiento.LISTA_ASIENTORow datosAsiento, string usuario,SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_CONTROL_ASIENTO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trans;
            try
            {
                cmd.Parameters.Add("@UUID", SqlDbType.VarChar).Value = UUID;
                cmd.Parameters.Add("@tipoAsiento", SqlDbType.VarChar).Value = datosAsiento.TIPO_ASIENTO;
                cmd.Parameters.Add("@moneda", SqlDbType.VarChar).Value = datosAsiento.MONEDA;
                cmd.Parameters.Add("@fecha", SqlDbType.Date).Value = datosAsiento.FECHA_PROCESO;
                cmd.Parameters.Add("@fondo", SqlDbType.VarChar).Value = datosAsiento.IsFONDONull()? null: datosAsiento.FONDO;
                object valor = DBNull.Value;
                if (!datosAsiento.IsID_OPERACIONNull() && datosAsiento.ID_OPERACION != "")
                {
                    valor = datosAsiento.ID_OPERACION;
                }

                cmd.Parameters.Add("@idOperacion", SqlDbType.VarChar).Value = valor;
                cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Dispose(); }
        }

        /// <summary>
        /// Se encarga de eliminar la información de un asiento previamente creado.
        /// </summary>
        /// <param name="UUID">Código único del asiento que se desea eliminar.</param>
        /// <param name="cn">Objeto que representa la conexión a la BD.</param>
        /// <param name="trans">Objeto que representa la transacción con la cual se realiza el proceso de registro.</param>
        public void EliminarAsientoAntiguo(string UUID, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_ASIENTO_CONTABLE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trans;
            try
            {
                cmd.Parameters.Add("@UUID", SqlDbType.VarChar).Value = UUID;

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Dispose(); }
        }

        public void ActualizarControl(string uuid, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_CONTROL", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlParameter prmUUID = cmd.Parameters.Add("@uuid", SqlDbType.VarChar);
                prmUUID.Value = uuid;

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Dispose(); }

        }

        //OT 7999 INI
        public DataTable ObtenerTipoCambio(string strFecha)
        {
            //Conexion a base de datos Prem_Main
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Main"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Main"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            try
            {
                //SqlConnection cn = GetConnection();
                //OT7940-PSC002 INI
                //SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_TIPO_CAMBIO", cn);
                SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_TIPO_CAMBIO", cn);
                //OT7940-PSC002 FIN  
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmFecha = cmd.Parameters.Add("@cFechaInicio", SqlDbType.VarChar);
                prmFecha.Value = strFecha;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("TiCambio");
                da.Fill(dt);
                return dt;

            }
            catch
            {
                return null;
            }
            finally
            {
                cn.Close();
            }
        }
        //OT 7999 FIN
    }
}

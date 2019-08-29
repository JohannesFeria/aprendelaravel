/*
 * Fecha de Modificación: 17/10/2013
 * Modificado por:        Carlos Galán
 * Numero de OT:          5924
 * Descripción del cambio: Se crea la clase AsientoPDRetencionDA, con los métodos ConsultarAlerta, InsertarAlerta, ObtenerTipoCambio, ObtenerRetenciones y InsertarAsientos
 */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

using System.Configuration;

namespace Procesos.DA
{
    /// <summary>
    /// Clase de Acceso a Datos que contiene los métodos para la generación de asientos de Papeleta de Depósito(PD) de las Retenciones
    /// </summary>
    public class AsientoPDRetencionDA: INGFondos.Data.DA
    {
        public AsientoPDRetencionDA(): base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) {}

        /// <summary>
        /// Obtiene la cantidad de alertas de generación de asientos de una fecha de proceso, para verificar si ya fueron generados.
        /// </summary>
        /// <param name="datFecha">Fecha de Proceso</param>
        /// <returns>Cantidad de alertas</returns>
        public DataTable ConsultarAlerta(DateTime datFecha)
        {
            try
            {
                SqlConnection cn = GetConnection();
                SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_ALERTA_GENERA_ASIENTO", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
                prmFecha.Value = datFecha;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ALERTA");
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Inserta registro de alerta de la generación de asientos.
        /// </summary>
        /// <param name="datFecha">Fecha de proceso</param>
        /// <param name="strUsuario">Usuario que generó los asientos</param>
        public void InsertarAlerta(DateTime datFecha, string strUsuario)
        {
            SqlConnection cn = GetConnection();
            SqlTransaction tr = null;
            try
            {
                cn.Open();
                tr = cn.BeginTransaction();

                string strFechaCorta;
                strFechaCorta = datFecha.ToString("dd/MM/yyyy");

                SqlCommand cmd = new SqlCommand("dbo.FOND_INS_ALERTA", cn, tr);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmClase = cmd.Parameters.Add("@clase", SqlDbType.VarChar);
                prmClase.Value = "EVE";

                SqlParameter prmTipo = cmd.Parameters.Add("@tipo", SqlDbType.VarChar);
                prmTipo.Value = "OBS";

                SqlParameter prmSubtipo = cmd.Parameters.Add("@subtipo", SqlDbType.VarChar);
                prmSubtipo.Value = DBNull.Value;

                SqlParameter prmTitulo = cmd.Parameters.Add("@titulo", SqlDbType.VarChar);
                prmTitulo.Value = "Genera Asientos PD de las Retenciones";

                SqlParameter prmDescripcion = cmd.Parameters.Add("@descripcion", SqlDbType.VarChar);
                prmDescripcion.Value = "Genera Asientos PD de las Retenciones - " + strFechaCorta;

                SqlParameter prmEstado = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
                prmEstado.Value = DBNull.Value;

                SqlParameter prmFechaLimite = cmd.Parameters.Add("@fechaLimite", SqlDbType.DateTime);
                prmFechaLimite.Value = datFecha;

                SqlParameter prmIdUsuarioOrigen = cmd.Parameters.Add("@idUsuarioOrigen", SqlDbType.Decimal);
                prmIdUsuarioOrigen.Value = DBNull.Value;

                SqlParameter prmIdUsuarioDestino = cmd.Parameters.Add("@idUsuarioDestino", SqlDbType.Decimal);
                prmIdUsuarioDestino.Value = DBNull.Value;

                SqlParameter prmIdParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
                prmIdParticipe.Value = DBNull.Value;

                SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
                prmIdFondo.Value = DBNull.Value;

                SqlParameter prmIdOperacion = cmd.Parameters.Add("@idOperacion", SqlDbType.Decimal);
                prmIdOperacion.Value = DBNull.Value;

                SqlParameter prmIdPreCierre = cmd.Parameters.Add("@idPreCierre", SqlDbType.Decimal);
                prmIdPreCierre.Value = DBNull.Value;

                SqlParameter prmUsuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
                prmUsuarioModificacion.Value = strUsuario;

                SqlParameter prmAreaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
                prmAreaModificacion.Value = "";

                SqlParameter prmSubtipoOtro = cmd.Parameters.Add("@subtipoOtro", SqlDbType.VarChar);
                prmSubtipoOtro.Value = DBNull.Value;

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

        /// <summary>
        /// Obtiene el tipo de cambio de la fecha de proceso
        /// </summary>
        /// <param name="strFecha">Fecha de proceso</param>
        /// <returns>Registro de tipo de cambio</returns>
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
                SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_TIPO_CAMBIO", cn);
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

        /// <summary>
        /// Obtiene las monedas con el total de retenciones de una fecha de proceso
        /// </summary>
        /// <param name="datFecha">Fecha de proceso</param>
        /// <returns>Retenciones por moneda</returns>
        public DataTable ObtenerRetenciones(DateTime datFecha)
        {
            try
            {
                SqlConnection cn = GetConnection();
                SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_RETENCION_MONEDA_X_FECHA", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
                prmFecha.Value = datFecha;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("RETENCION");
                da.Fill(dt);
                return dt;

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Inserta los asientos DP de las retenciones
        /// </summary>
        /// <param name="strAnio">Año de asiento</param>
        /// <param name="strMes">Mes de asiento</param>
        /// <param name="strFecha">Fecha de proceso</param>
        /// <param name="strNumDoc">Número de Documento</param>
        /// <param name="strCheque">Número de Cheque</param>
        /// <param name="strMoneda">Moneda</param>
        /// <param name="strTipoCambio">Tipo de cambio de la fecha de proceso</param>
        /// <param name="decMontoSol">Monto en soles</param>
        /// <param name="decMontoDol">Monto en dolares</param>
        public bool InsertarAsientos(string strAnio, string strMes, string strFecha, string strNumDoc, string strCheque, string strMoneda, decimal strTipoCambio, decimal decMontoSol, decimal decMontoDol) 
        {
            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();
            SqlTransaction tr = null;

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            try
            {
                //Inserta Cabecera
                string strLibro = "PD";
                string strTipo = "I";
                string strFlujoCaja = "40";
                string strTipoDoc = "00";

                string strDescripcion;
                string strCuentaContable;
                string strCuentaCaja;

                if (strMoneda == "S")
                {
                    strDescripcion = "TRANSFERENCIA DE IR - FFMM MN";
                    strCuentaContable = "401751";
                    strCuentaCaja = "1041011";
                }
                else
                {
                    strDescripcion = "TRANSFERENCIA DE IR - FFMM ME";
                    strCuentaContable = "401752";
                    strCuentaCaja = "1041021";
                }


                tr = cn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("dbo.sp_Ins_Cabecera_Comprobante", cn, tr);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmAno = cmd.Parameters.Add("@cAno", SqlDbType.VarChar);
                prmAno.Value = strAnio;

                SqlParameter prmMes = cmd.Parameters.Add("@cMes", SqlDbType.VarChar);
                prmMes.Value = strMes;

                SqlParameter prmLibro = cmd.Parameters.Add("@cLibro", SqlDbType.VarChar);
                prmLibro.Value = strLibro;

                SqlParameter prmFecha = cmd.Parameters.Add("@dFecha", SqlDbType.VarChar);
                prmFecha.Value = strFecha;

                SqlParameter prmDetalle = cmd.Parameters.Add("@cDetalle", SqlDbType.VarChar);
                prmDetalle.Value = strDescripcion;

                SqlParameter prmTipo = cmd.Parameters.Add("@cTipo", SqlDbType.VarChar);
                prmTipo.Value = strTipo;

                SqlParameter prmValDefa = cmd.Parameters.Add("@cValDefa", SqlDbType.VarChar);
                prmValDefa.Value = "";

                SqlParameter prmEstado = cmd.Parameters.Add("@cEstado", SqlDbType.VarChar);
                prmEstado.Value = "N";

                SqlParameter prmTrans = cmd.Parameters.Add("@cTrans", SqlDbType.VarChar);
                prmTrans.Value = "N";

                SqlParameter prmModif = cmd.Parameters.Add("@cModif", SqlDbType.VarChar);
                prmModif.Value = "S";

                SqlParameter prmComprobante = cmd.Parameters.Add("@cComprobante", SqlDbType.VarChar, 5);
                prmComprobante.Direction = ParameterDirection.Output;

                SqlParameter prmMsgRetorno = cmd.Parameters.Add("@cMsgRetorno", SqlDbType.VarChar, 100);
                prmMsgRetorno.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                string strComprobante = prmComprobante.Value.ToString();
                string strMsgRetorno = prmMsgRetorno.Value.ToString();

                if (strMsgRetorno == "No se pudo crear el Comprobante")
                {
                    tr.Rollback();
                    return false;
                }

                //Inserta Detalle
                SqlCommand cmdDet = new SqlCommand("dbo.sp_Ins_Detalle_Comprobante", cn, tr);
                cmdDet.CommandType = CommandType.StoredProcedure;

                SqlParameter prmAno1 = cmdDet.Parameters.Add("@cAno", SqlDbType.VarChar);
                prmAno1.Value = strAnio;

                SqlParameter prmMes1 = cmdDet.Parameters.Add("@cMes", SqlDbType.VarChar);
                prmMes1.Value = strMes;

                SqlParameter prmLibro1 = cmdDet.Parameters.Add("@cLibro", SqlDbType.VarChar);
                prmLibro1.Value = strLibro;

                SqlParameter prmComprobante1 = cmdDet.Parameters.Add("@cComprobante", SqlDbType.VarChar);
                prmComprobante1.Value = strComprobante;

                SqlParameter prmCenCos = cmdDet.Parameters.Add("@cCenCos", SqlDbType.VarChar);
                prmCenCos.Value = "";

                SqlParameter prmCenGes = cmdDet.Parameters.Add("@cCenGes", SqlDbType.VarChar);
                prmCenGes.Value = "";

                SqlParameter prmCuentaCon = cmdDet.Parameters.Add("@cCuentaCon", SqlDbType.VarChar);
                prmCuentaCon.Value = strCuentaContable;

                SqlParameter prmCtaCte = cmdDet.Parameters.Add("@cCtaCte", SqlDbType.VarChar);
                prmCtaCte.Value = "";

                SqlParameter prmNombre = cmdDet.Parameters.Add("@cNombre", SqlDbType.VarChar);
                prmNombre.Value = "";

                SqlParameter prmTipDoc = cmdDet.Parameters.Add("@cTipDoc", SqlDbType.VarChar);
                prmTipDoc.Value = strTipoDoc;  

                SqlParameter prmNumDoc = cmdDet.Parameters.Add("@cNumDoc", SqlDbType.VarChar);
                prmNumDoc.Value = strNumDoc;

                SqlParameter prmTipo1 = cmdDet.Parameters.Add("@cTipo", SqlDbType.VarChar);
                prmTipo1.Value = strTipo;

                SqlParameter prmFluCja = cmdDet.Parameters.Add("@cFluCja", SqlDbType.VarChar);
                prmFluCja.Value = strFlujoCaja;

                SqlParameter prmMoneda = cmdDet.Parameters.Add("@cMoneda", SqlDbType.VarChar);
                prmMoneda.Value = strMoneda;

                SqlParameter prmCuenta = cmdDet.Parameters.Add("@cCuenta", SqlDbType.VarChar);
                prmCuenta.Value = strCuentaCaja;

                SqlParameter prmGraba = cmdDet.Parameters.Add("@cGraba", SqlDbType.VarChar);
                prmGraba.Value = "D";

                SqlParameter prmChequera = cmdDet.Parameters.Add("@cChequera", SqlDbType.VarChar);
                prmChequera.Value = "";

                SqlParameter prmCheque = cmdDet.Parameters.Add("@cCheque", SqlDbType.VarChar);
                prmCheque.Value = strCheque;

                SqlParameter prmEstadoPrn = cmdDet.Parameters.Add("@cEstadoPrn", SqlDbType.VarChar);
                prmEstadoPrn.Value = "N";

                SqlParameter prmAutoriza = cmdDet.Parameters.Add("@cAutoriza", SqlDbType.VarChar);
                prmAutoriza.Value = "";

                SqlParameter prmConcepto01 = cmdDet.Parameters.Add("@cConcepto01", SqlDbType.VarChar);
                prmConcepto01.Value = strDescripcion;

                SqlParameter prmConcepto02 = cmdDet.Parameters.Add("@cConcepto02", SqlDbType.VarChar);
                prmConcepto02.Value = "";

                SqlParameter prmFecDoc = cmdDet.Parameters.Add("@dFecDoc", SqlDbType.VarChar);
                prmFecDoc.Value = strFecha;
                
                SqlParameter prmTipCam = cmdDet.Parameters.Add("@nTipCam", SqlDbType.Decimal);
                prmTipCam.Value = strTipoCambio;

                SqlParameter prmDebSol = cmdDet.Parameters.Add("@nDebSol", SqlDbType.Decimal);
                prmDebSol.Value = decMontoSol;

                SqlParameter prmHabSol = cmdDet.Parameters.Add("@nHabSol", SqlDbType.Decimal);
                prmHabSol.Value = 0;

                SqlParameter prmDebDol = cmdDet.Parameters.Add("@nDebDol", SqlDbType.Decimal);
                prmDebDol.Value = decMontoDol;

                SqlParameter prmHabDol = cmdDet.Parameters.Add("@nHabDol", SqlDbType.Decimal);
                prmHabDol.Value = 0;

                SqlParameter prmTipOpe = cmdDet.Parameters.Add("@cTipOpe", SqlDbType.VarChar);
                prmTipOpe.Value = "N";

                SqlParameter prmTcOpe = cmdDet.Parameters.Add("@nTcOpe", SqlDbType.Float);
                prmTcOpe.Value = 0;

                SqlParameter prmEstado1 = cmdDet.Parameters.Add("@cEstado", SqlDbType.VarChar);
                prmEstado1.Value = "T";

                SqlParameter prmImpGan = cmdDet.Parameters.Add("@nImpGan", SqlDbType.Float);
                prmImpGan.Value = 0;

                SqlParameter prmImpPer = cmdDet.Parameters.Add("@nImpPer", SqlDbType.Float);
                prmImpPer.Value = 0;

                SqlParameter prmValDefa1 = cmdDet.Parameters.Add("@cValDefa", SqlDbType.VarChar);
                prmValDefa1.Value = "";

                SqlParameter prmValida = cmdDet.Parameters.Add("@cValida", SqlDbType.VarChar);
                prmValida.Value = "";

                SqlParameter prmTrans1 = cmdDet.Parameters.Add("@cTrans", SqlDbType.VarChar);
                prmTrans1.Value = "N";

                SqlParameter prmComMod = cmdDet.Parameters.Add("@cComMod", SqlDbType.VarChar);
                prmComMod.Value = "S";

                SqlParameter prmAmarre = cmdDet.Parameters.Add("@cAmarre", SqlDbType.VarChar);
                prmAmarre.Value = "";

                SqlParameter prmBiMoneda = cmdDet.Parameters.Add("@cBiMoneda", SqlDbType.VarChar);
                prmBiMoneda.Value = "N";

                SqlParameter prmMsgRetorno1 = cmdDet.Parameters.Add("@cMsgRetorno", SqlDbType.VarChar, 100);
                prmMsgRetorno1.Direction = ParameterDirection.Output;

                cmdDet.ExecuteNonQuery();
                string strMsgRetorno1 = prmMsgRetorno1.Value.ToString();

                if (strMsgRetorno1 != "Detalle de Comprobante Grabado con exito")
                {
                    tr.Rollback();
                    return false;
                }

                tr.Commit();

                return true;
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

    }
}

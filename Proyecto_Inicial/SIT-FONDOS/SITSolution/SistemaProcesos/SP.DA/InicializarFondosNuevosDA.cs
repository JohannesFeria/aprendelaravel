/*
 * Fecha de Modificación    : 19/11/2015                    
 * Modificado por           : Richard Valdez
 * Numero de OT             : 7961
 * Descripción del cambio   : Creación
 * */
/*
 * Fecha de Modificación    : 29/02/2016                    
 * Modificado por           : Irene Reyes
 * Numero de OT             : 8365
 * Descripción del cambio   : Modificar el método InicializarFondosNuevos para llamar al procedure FMPR_INS_FONDO_PADRE_X_FONDO_HIJO_INICIALIZACION
 *                            desde la base de datos CONASEV. 
 * */
/*
 * Fecha de Modificación    : 30/03/2016                    
 * Modificado por           : Irene Reyes
 * Numero de OT             : 8540
 * Descripción del cambio   : Agregar validación para que registre en la base de datos conasev para los fondos públicos. 
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

using System.Data.SqlClient;

namespace Procesos.DA
{
    public class InicializarFondosNuevosDA : INGFondos.Data.DA
    {
        public InicializarFondosNuevosDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        /// <summary>
        /// Lista los Fondos Nuevos.
        /// Un fondo es considerado nuevo si tiene menos de 2 Valores Cuotas.
        /// </summary>
        public DataTable ObtenerFondosNuevos()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_FONDO_NUEVO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        /// <summary>
        /// Inicializar aquellos Fondos Nuevos que NO tienen Valor Cuota.
        /// </summary>
        /// <param name="FondoID"></param>
        /// <param name="FechaPrimerPrecierre"></param>
        /// <param name="ValorCuota"></param>
        /// <param name="NumeroContabilidadNueva"></param>
        /// <param name="NumeroContabilidadModelo"></param>
        /// <param name="FondoRUC"></param>
        /// <param name="FondoDescripcion"></param>
        /// <returns>En caso de éxito retorna una cadena vacía, en caso de error retorna un mensaje de error</returns>
       //agregamos los parametros del nuevo procedure
        public string InicializarFondosNuevos(int FondoID, DateTime FechaPrimerPrecierre, decimal ValorCuota,
            string NumeroContabilidadNueva, string NumeroContabilidadModelo, string FondoRUC, string FondoDescripcion,string codigoConasev)
        {
            string strMensajeRetorno = string.Empty;

            SqlConnection cnOperaciones;
            SqlConnection cnPreMain;
            SqlConnection cnTributacion;
            //OT INI 8365
            SqlConnection cnConasev;
            //OT FIN 8365

            SqlTransaction tranOperaciones;
            SqlTransaction tranPreMain;
            SqlTransaction tranTributacion;
            //OT INI 8365
            SqlTransaction tranConasev;
            //OT FIN 8365


            #region Obtener las conexiones

            //Operaciones

            cnOperaciones = GetConnection();
            cnOperaciones.Open();

            //PreMain

            string servidorPremMain = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorPremMain];
            string baseDeDatosPremMain = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosPremMain];

            cnPreMain = new SqlConnection();
            cnPreMain.ConnectionString = @"data source=" + servidorPremMain + ";initial catalog=" 
                    + baseDeDatosPremMain + ";integrated security=SSPI";
            cnPreMain.Open();

            //Tributacion

            string servidorTributacion = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorTributacion];
            string baseDeDatosTributacion = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosTributacion];

            cnTributacion = new SqlConnection();
            cnTributacion.ConnectionString = @"data source=" + servidorTributacion + ";initial catalog="
                    + baseDeDatosTributacion + ";integrated security=SSPI";
            cnTributacion.Open();

            //OT INI  8365
            //Conasev

            string servidorConasev = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorCONASEV];
            string baseDeDatosConasev = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosCONASEV];

            cnConasev = new SqlConnection();
            cnConasev.ConnectionString = @"data source=" + servidorConasev + ";initial catalog=" + baseDeDatosConasev + ";integrated security=SSPI";
            cnConasev.Open();
            //OT FIN 8365
            #endregion

            #region Obtener las transacciones

            tranOperaciones = cnOperaciones.BeginTransaction(IsolationLevel.ReadUncommitted);
            tranPreMain = cnPreMain.BeginTransaction(IsolationLevel.ReadUncommitted);
            tranTributacion = cnTributacion.BeginTransaction(IsolationLevel.ReadUncommitted);
            //OT INI 8365
            tranConasev = cnConasev.BeginTransaction(IsolationLevel.ReadUncommitted);
            //OT FIN 8365

            #endregion
            
            try
            {
                //La fecha a grabar es un dia antes al que selecciono el usuario
                FechaPrimerPrecierre = FechaPrimerPrecierre.AddDays(-1);
                
                
                #region Insert Valor Cuota - Base de datos AdmCuentas

                SqlCommand cmdValorCuota = new SqlCommand("dbo.FMPR_INS_VALOR_CUOTA_X_INICIALIZACION", cnOperaciones, tranOperaciones);
                cmdValorCuota.CommandType = CommandType.StoredProcedure;

                SqlParameter prmIdFondo = cmdValorCuota.Parameters.Add("@ID_FONDO", SqlDbType.Int);
                prmIdFondo.Value = FondoID;

                SqlParameter prmFecha = cmdValorCuota.Parameters.Add("@FECHA", SqlDbType.DateTime);
                prmFecha.Value = FechaPrimerPrecierre;

                SqlParameter prmValorCuota = cmdValorCuota.Parameters.Add("@VALOR_CUOTA", SqlDbType.Decimal);
                prmValorCuota.Value = ValorCuota;

                cmdValorCuota.ExecuteNonQuery();

                #endregion

                #region Insert Pre Cierre - Base de datos AdmCuentas

                SqlCommand cmdPrecierre = new SqlCommand("dbo.FMPR_INS_PRECIERRE_X_INICIALIZACION", cnOperaciones, tranOperaciones);
                cmdPrecierre.CommandType = CommandType.StoredProcedure;

                SqlParameter prmIdFondoPreCierre = cmdPrecierre.Parameters.Add("@ID_FONDO", SqlDbType.Int);
                prmIdFondoPreCierre.Value = FondoID;

                SqlParameter prmFechaPreCierre = cmdPrecierre.Parameters.Add("@FECHA", SqlDbType.DateTime);
                prmFechaPreCierre.Value = FechaPrimerPrecierre;

                SqlParameter prmValorCuotaPreCierre = cmdPrecierre.Parameters.Add("@VALOR_CUOTA", SqlDbType.Decimal);
                prmValorCuotaPreCierre.Value = ValorCuota;

                cmdPrecierre.ExecuteNonQuery();

                #endregion

                #region Insert Log Diario - Base de datos tributacion

                SqlCommand cmdLogDiario = new SqlCommand("dbo.FMPR_INS_LOG_DIARIO", cnTributacion, tranTributacion);
                cmdLogDiario.CommandType = CommandType.StoredProcedure;

                SqlParameter prmIdFondoLogDiario = cmdLogDiario.Parameters.Add("@ID_FONDO", SqlDbType.Int);
                prmIdFondoLogDiario.Value = FondoID;

                SqlParameter prmFechaLogDiario = cmdLogDiario.Parameters.Add("@FECHA", SqlDbType.DateTime);
                prmFechaLogDiario.Value = FechaPrimerPrecierre;

                cmdLogDiario.ExecuteNonQuery();

                #endregion

                #region prem_banco

                SqlCommand cmdPremBanco = new SqlCommand("dbo.FMPR_ELI_MOVIMIENTOS_BD_PREM_BANCO", cnPreMain, tranPreMain);
                cmdPremBanco.CommandType = CommandType.StoredProcedure;

                SqlParameter prmContaNueva = cmdPremBanco.Parameters.Add("@CONTA_NUEVA", SqlDbType.Int);
                prmContaNueva.Value = NumeroContabilidadNueva;

                string strMensajePremBanco = cmdPremBanco.ExecuteScalar().ToString();

                if (strMensajePremBanco != "0")
                {
                    strMensajeRetorno = strMensajePremBanco;

                    throw new Exception();
                }

                #endregion

                #region prem_conta

                SqlCommand cmdPremConta = new SqlCommand("dbo.FMPR_ELI_MOVIMIENTOS_BD_PREM_CONTA", cnPreMain, tranPreMain);
                cmdPremConta.CommandType = CommandType.StoredProcedure;

                SqlParameter prmContaNuevaPreConta = cmdPremConta.Parameters.Add("@CONTA_NUEVA", SqlDbType.Int);
                prmContaNuevaPreConta.Value = NumeroContabilidadNueva;

                String strMensajePremConta = cmdPremConta.ExecuteScalar().ToString();

                if (strMensajePremConta != "0")
                {
                    strMensajeRetorno = strMensajePremConta;

                    throw new Exception();
                }

                #endregion

                #region Insert empresa - Base de datos prem_main

                SqlCommand cmdEmpresa = new SqlCommand("dbo.FMPR_INS_EMPRESA", cnPreMain, tranPreMain);
                cmdEmpresa.CommandType = CommandType.StoredProcedure;

                SqlParameter prmCuentaNueva = cmdEmpresa.Parameters.Add("@CUENTA_NUEVA", SqlDbType.VarChar, 3);
                prmCuentaNueva.Value = NumeroContabilidadNueva;

                SqlParameter prmDescripcion = cmdEmpresa.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar, 50);
                prmDescripcion.Value = FondoDescripcion;

                SqlParameter prmCuentaModelo = cmdEmpresa.Parameters.Add("@CUENTA_MODELO", SqlDbType.VarChar, 3);
                prmCuentaModelo.Value = NumeroContabilidadModelo;
                
                SqlParameter prmRUC = cmdEmpresa.Parameters.Add("@RUC", SqlDbType.VarChar, 11);
                prmRUC.Value = FondoRUC;

                string strMensajeEmpresa = cmdEmpresa.ExecuteScalar().ToString();

                if (strMensajeEmpresa != "0")
                {
                    strMensajeRetorno = strMensajeEmpresa;

                    throw new Exception();
                }

                 
                #endregion
                 
                //OT 8365 INI
                //OT 8540 INI
                if (codigoConasev != "")
                {

                    #region Insert Fondo - FondoPadre - Base de datos Conasev

                    SqlCommand cmdConasev = new SqlCommand("dbo.FMPR_INS_FONDO_PADRE_X_FONDO_HIJO_INICIALIZACION", cnConasev, tranConasev);
                    cmdConasev.CommandType = CommandType.StoredProcedure;

                    SqlParameter prmCodigoSMV = cmdConasev.Parameters.Add("@CODIGOSMV", SqlDbType.Char, 4);
                    prmCodigoSMV.Value = codigoConasev;

                    SqlParameter prmIdFondoConasev = cmdConasev.Parameters.Add("@ID", SqlDbType.Int);
                    prmIdFondoConasev.Value = FondoID;

                    SqlParameter prmContabilidadNuevaIngresada = cmdConasev.Parameters.Add("@CONTABILIDAD_NUEVA_INGRESADA", SqlDbType.Int);
                    prmContabilidadNuevaIngresada.Value = NumeroContabilidadNueva;

                    //cmdConasev.ExecuteNonQuery();
                    string strMensajeConasev = cmdConasev.ExecuteScalar().ToString();

                    if (strMensajeConasev != "0")
                    {
                        strMensajeRetorno = strMensajeConasev;

                        throw new Exception();
                    }


                    #endregion
                }
                //OT 8540 FIN
                tranOperaciones.Commit();
                tranPreMain.Commit();
                tranTributacion.Commit();
                //OT INI 8365
                tranConasev.Commit();
                //OT FIN 8365
                
                
            }
            catch (Exception e)
            {
                tranOperaciones.Rollback();
                tranPreMain.Rollback();
                tranTributacion.Rollback();
                ///OT INI 8365
                tranConasev.Rollback();
                //OT FIN 8365
                if(strMensajeRetorno == string.Empty)
                {
                    //throw new Exception();
                    strMensajeRetorno = "Error. No se pudo realizar la inicialización.";
                }else
                {
                    strMensajeRetorno = "Error. No se pudo realizar la inicialización.\r\nDetalle: " + strMensajeRetorno;
                } 
            }
            finally {
                cnOperaciones.Close();
                cnPreMain.Close();
                cnTributacion.Close();
                cnConasev.Close();
            }

            return strMensajeRetorno;
        }
        //OT 8365 FIN
                
        /// <summary>
        /// Modificar la Inicialización de aquellos Fondos Nuevos que tienen 1 Valor Cuota.
        /// </summary>
        /// <param name="FondoID"></param>
        /// <param name="FechaPrimerPrecierre"></param>
        /// <returns>En caso de éxito retorna una cadena vacía, en caso de error retorna un mensaje de error</returns>
        public string ModificarInicializacionFondosNuevos(int FondoID, DateTime FechaPrimerPrecierre, decimal ValorCuota)
        {
            string strMensajeRetorno = string.Empty;

            //variables de conexión

            SqlConnection cnOperaciones;
            SqlConnection cnTributacion;

            //variables de Transacción

            SqlTransaction tranOperaciones;
            SqlTransaction tranTributacion;

            #region Obtener las conexiones

            //Operaciones
            cnOperaciones = GetConnection();
            cnOperaciones.Open();

            //Tributacion
            string servidorTributacion = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorTributacion];
            string baseDeDatosTributacion = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosTributacion];

            cnTributacion = new SqlConnection();
            cnTributacion.ConnectionString = @"data source=" + servidorTributacion + ";initial catalog="
                    + baseDeDatosTributacion + ";integrated security=SSPI";
            cnTributacion.Open();

            #endregion

            #region Obtener las Transaciones

            tranOperaciones = cnOperaciones.BeginTransaction(IsolationLevel.ReadUncommitted);
            tranTributacion = cnTributacion.BeginTransaction(IsolationLevel.ReadUncommitted);

            #endregion

            try
            {
                //La fecha a grabar es un dia antes al que selecciono el usuario
                FechaPrimerPrecierre = FechaPrimerPrecierre.AddDays(-1);

                #region Actualizar la tabla valor Cuota su campo FECHA y su campo Valor Cuota - Base de datos AdmCuentas

                SqlCommand cmdValorCuota = new SqlCommand("dbo.FMPR_ACT_VALOR_CUOTA_X_INICIALIZACION", cnOperaciones, tranOperaciones);
                cmdValorCuota.CommandType = CommandType.StoredProcedure;

                SqlParameter prmIdFondo = cmdValorCuota.Parameters.Add("@ID_FONDO", SqlDbType.Int);
                prmIdFondo.Value = FondoID;

                SqlParameter prmFecha = cmdValorCuota.Parameters.Add("@FECHA", SqlDbType.DateTime);
                prmFecha.Value = FechaPrimerPrecierre;

                SqlParameter prmValorCuota = cmdValorCuota.Parameters.Add("@VALOR_CUOTA", SqlDbType.Decimal);
                prmValorCuota.Value = ValorCuota;

                cmdValorCuota.ExecuteNonQuery();

                #endregion

                #region Actualizar Pre Cierre - Base de datos AdmCuentas

                SqlCommand cmdPrecierre = new SqlCommand("dbo.FMPR_ACT_PRECIERRE_X_INICIALIZACION", cnOperaciones, tranOperaciones);
                cmdPrecierre.CommandType = CommandType.StoredProcedure;

                SqlParameter prmIdFondoPreCierre = cmdPrecierre.Parameters.Add("@ID_FONDO", SqlDbType.Int);
                prmIdFondoPreCierre.Value = FondoID;

                SqlParameter prmFechaPreCierre = cmdPrecierre.Parameters.Add("@FECHA", SqlDbType.DateTime);
                prmFechaPreCierre.Value = FechaPrimerPrecierre;

                SqlParameter prmValorCuotaPreCierre = cmdPrecierre.Parameters.Add("@VALOR_CUOTA", SqlDbType.Decimal);
                prmValorCuotaPreCierre.Value = ValorCuota;

                cmdPrecierre.ExecuteNonQuery();

                #endregion

                #region Actualizar a Log_Diario de la Base de datos Prem_Main

                SqlCommand cmdLogDiario = new SqlCommand("dbo.FMPR_ACT_LOG_DIARIO_X_INICIALIZACION", cnTributacion, tranTributacion);
                cmdLogDiario.CommandType = CommandType.StoredProcedure;

                SqlParameter prmIdFondoLogDiario = cmdLogDiario.Parameters.Add("@ID_FONDO", SqlDbType.Int);
                prmIdFondoLogDiario.Value = FondoID;

                SqlParameter prmFechaLogDiario = cmdLogDiario.Parameters.Add("@FECHA_PROCESO", SqlDbType.DateTime);
                prmFechaLogDiario.Value = FechaPrimerPrecierre;

                cmdLogDiario.ExecuteNonQuery();

                #endregion

                tranOperaciones.Commit();
                tranTributacion.Commit();

            }
            catch (Exception ex)
            { 
                tranOperaciones.Rollback();
                tranTributacion.Rollback();

                strMensajeRetorno = "Error. No se pudo realizar la actualización.";
            }
            finally
            {
                cnOperaciones.Close();
                cnTributacion.Close();
            }

            return strMensajeRetorno;
        }
    }
}

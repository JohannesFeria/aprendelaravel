/*
 * Fecha de Creación: 04/07/2017
 * Creado por: Anthony Joaquin
 * Numero de OT: 10563
 * Descripción del cambio: Creación
 * */

using System;
using System.Data;
using Procesos.TD;
using Procesos.DA;
using System.Data.SqlClient;
using INGFondos.Constants;
using INGFondos.Data;
using System.Configuration;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using INGFondos.Support.Interop;
using Procesos.Constants;

namespace Procesos.BL
{
	public class SimulacionRetencionMasivaBL
	{
		public SimulacionRetencionMasivaBL()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}

        //public DataTable ObtenerFondos()
        //{
        //    SimulacionRetencionMasivaDA da = new SimulacionRetencionMasivaDA();
        //    DataTable fondo = da.ObtenerFondos();
        //    return fondo;
        //}

		
        //public void EliminarTablasSimulacion(string usuario){
        //    SimulacionRetencionMasivaDA simulacionRetencionMasivaDA = new SimulacionRetencionMasivaDA();
        //    string server = string.Empty;
        //    string database = string.Empty;

        //    ConnectionManager cmOperaciones = new ConnectionManager();

        //    server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorOperaciones];
        //    database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosOperaciones];

        //    SqlConnection cnOperaciones = cmOperaciones.GetTrustedConnection(server, database);
        //    cnOperaciones.Open();

        //    ConnectionManager cmTributacion = new ConnectionManager();

        //    server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorTributacion];
        //    database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosTributacion];

        //    SqlConnection cnTributacion = cmTributacion.GetTrustedConnection(server, database);
        //    cnTributacion.Open();

        //    SqlTransaction transOperaciones = cnOperaciones.BeginTransaction();
        //    SqlTransaction transTributacion = cnTributacion.BeginTransaction();

        //    try
        //    {
        //        simulacionRetencionMasivaDA.eliminarOperacionSimulacion(cnOperaciones, transOperaciones);
        //        simulacionRetencionMasivaDA.eliminarRetencionSimulacion(cnTributacion, transTributacion);
        //        transOperaciones.Commit();
        //        transTributacion.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        transOperaciones.Rollback();
        //        transTributacion.Rollback();
        //        throw ex;
        //    }
        //    finally {
        //        transOperaciones.Dispose();
        //        transTributacion.Dispose();
        //        cnOperaciones.Close();
        //        cnTributacion.Close();
        //    }
        //}
		

        //public void CargarRescateSimulacionMasivo(string archivo, string idFondoCadena, DateTime fechaActual, string usuario)
        //{
        //    Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-PE");
        //    CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

        //    #region Leer archivo Excel
        //    Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        //    ExcelApplication app = new ExcelApplication();
        //    ExcelWorkBook wb = app.OpenWorkBook(archivo, ExcelMode.Full);
        //    ExcelWorkSheet sheet = wb.GetSheet(1);

        //    DataTable dt = sheet.LeerTabla("B", "L", 2, sheet.ObtenerUltimaFilaTabla("B", 2), true);
        //    wb.Close();
        //    app.Close();
        //    #endregion

        //    DateTime fechaSolicitud = fechaActual;
        //    DateTime fechaSolicitudDiaAnterior = fechaActual.AddDays(-1);

        //    string fechaCadena = fechaSolicitudDiaAnterior.ToString("dd/MM/yyyy").Substring(0, 10);
        //    string fechaCadenaOriginal = fechaCadena;
        //    fechaCadena = fechaCadena.Substring(fechaCadena.Length - 4, 4) + "-" + fechaCadena.Substring(fechaCadena.Length - 7, 2) + "-" + fechaCadena.Substring(0, 2);

        //    SimulacionRetencionMasivaDA da = new SimulacionRetencionMasivaDA();

        //    string server = string.Empty;
        //    string database = string.Empty;

        //    ConnectionManager cmOperaciones = new ConnectionManager();

        //    server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorOperaciones];
        //    database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosOperaciones];

        //    SqlConnection cnOperaciones = cmOperaciones.GetTrustedConnection(server, database);
        //    cnOperaciones.Open();

        //    SqlTransaction transOperaciones = cnOperaciones.BeginTransaction();

        //    DataTable dtFondos = da.ListarFondos(cnOperaciones, transOperaciones);
        //    DataTable dtCuentaRecaudo = da.ListarTablaGeneral(ConstantesING.CODIGO_TABLA_CUENTA_RECAUDO);
        //    int numeroFila = 0;
        //    try
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            int cuc = Convert.ToInt32(dr["CUC"]);
        //            int idParticipe = cuc - 44670000;
        //            string cuentaParticipacion = dr["CUENTA_PARTICIPACION"].ToString().Trim();
        //            DataTable dtCuentasParticipacion = da.ListarCuentasParticipacion(idParticipe, cnOperaciones, transOperaciones);
        //            int idCuentaParticipacion = Convert.ToInt32(dtCuentasParticipacion.Select("CODIGO = '" + cuentaParticipacion + "'")[0]["ID"]);
        //            int fondoOrigen = Convert.ToInt32(idFondoCadena);//dr["FONDO_ORIGEN"].ToString().Trim();

        //            int idFondo = Convert.ToInt32(idFondoCadena);
        //            DataTable dtFondo = da.ObtenerFondo(idFondo, cnOperaciones, transOperaciones);
        //            int plazo = Convert.ToInt32(dtFondo.Rows[0]["PLAZO_LIQUIDACION_DIAS_RESCATE"]);
        //            DateTime horaHasta = Convert.ToDateTime(dtFondo.Rows[0]["HORARIO_OPERACION_HASTA"]);
        //            DateTime fechaProceso = fechaSolicitud;
        //            DateTime fechaPago = fechaSolicitud;

        //            string viaPago = dr["CODIGO_BANCO"].ToString().Trim();
        //            string formaPago = dr["FORMA_PAGO"].ToString().Trim();
        //            string cuentaBancaria = dr["CUENTA_BANCARIA"].ToString().Trim();
        //            string comentario = dr["COMENTARIO"].ToString().Trim();

        //            int idCuentaBancaria = 0;

        //            if (formaPago == ConstantesING.CODIGO_FORMA_PAGO_TRASPASO_FONDOS)
        //            {
        //                DataTable dtCuentaBancaria = da.ListarCuentasBancarias(idParticipe, cnOperaciones, transOperaciones);
        //                if (cuentaBancaria.Equals(string.Empty))
        //                {
        //                    if (dtCuentaBancaria.Select("COMENTARIO = '" + comentario + "'").Length == 0)
        //                    {
        //                        throw new Exception("El CUC " + cuc.ToString() + "no tiene una cuenta bancaria que cumpla con el comentario.");
        //                    }
        //                    viaPago = dtCuentaBancaria.Select("COMENTARIO = '" + comentario + "'")[0]["CODIGO_BANCO"].ToString();
        //                    idCuentaBancaria = Convert.ToInt32(dtCuentaBancaria.Select("COMENTARIO = '" + comentario + "'")[0]["ID"]);
        //                }
        //                else
        //                {
        //                    if (dtCuentaBancaria.Select("NUMERO_CUENTA = '" + cuentaBancaria + "'").Length == 0)
        //                    {
        //                        throw new Exception("El número de la cuenta bancaria ingresada no está registrada para el CUC " + cuc.ToString());
        //                    }
        //                    idCuentaBancaria = Convert.ToInt32(dtCuentaBancaria.Select("NUMERO_CUENTA = '" + cuentaBancaria + "'")[0]["ID"]);
        //                }
        //            }

        //            fechaProceso = fechaProceso.AddYears(1900 - fechaProceso.Year);
        //            fechaProceso = fechaProceso.AddMonths(1 - fechaProceso.Month);
        //            fechaProceso = fechaProceso.AddDays(1 - fechaProceso.Day);

        //            if (fechaProceso > horaHasta)
        //            {
        //                fechaProceso = fechaSolicitud.AddDays(1);
        //                while (fechaProceso.DayOfWeek == DayOfWeek.Saturday || fechaProceso.DayOfWeek == DayOfWeek.Sunday || isFeriado(fechaProceso,cnOperaciones,transOperaciones) == true)
        //                {
        //                    fechaProceso.AddDays(1);
        //                }
        //                fechaProceso = fechaProceso.Date.AddHours(8);
        //            }
        //            else
        //            {
        //                fechaProceso = fechaSolicitud;
        //            }

        //            while (plazo > 0)
        //            {
        //                fechaPago = fechaPago.AddDays(1);
        //                if (fechaPago.DayOfWeek == DayOfWeek.Saturday || fechaPago.DayOfWeek == DayOfWeek.Sunday || isFeriado(fechaPago,cnOperaciones,transOperaciones) == true)
        //                {
        //                    continue;
        //                }
        //                plazo--;
        //            }
        //            fechaPago = fechaPago.Date;

        //            string monedaOrigen = dtFondos.Select("ID = '" + fondoOrigen + "'")[0]["CODIGO_MONEDA"].ToString();
        //            string tipoRescate = dr["TIPO_RESCATE"].ToString().Trim();
        //            decimal monto = Convert.ToDecimal(dr["MONTO"]);
        //            decimal cuotas = Convert.ToDecimal(dr["CUOTAS"]);

        //            string promotor = da.ObtenerPromotor(idParticipe,cnOperaciones, transOperaciones);

        //            decimal vc = da.ObtenerValorCuotaFecha(fechaCadena,idFondo, cnOperaciones, transOperaciones);

        //            if (tipoRescate == "TOTAL")
        //            {
        //                cuotas = da.ObtenerSaldoCuotasxCuentaParticipacion(idFondo, idCuentaParticipacion, cnOperaciones, transOperaciones);
        //            }

        //            //INSERTANDO EN OPERACION_SIMULACION
        //            da.InsertarOperacionSimulacion(idParticipe, idFondo, idCuentaParticipacion, 0, tipoRescate, monto, cuotas, vc, promotor, usuario, string.Empty, fechaSolicitud, fechaProceso, fechaPago, viaPago, formaPago, idCuentaBancaria,cnOperaciones, transOperaciones);

        //            numeroFila = numeroFila + 1;
        //        }

        //        transOperaciones.Commit();

        //        //MessageBox.Show("Se cargaron " + numeroFila + " registros de " + "rescates simulados.", "Mensaje");

        //    }
        //    catch (Exception ex)
        //    {
        //        transOperaciones.Rollback();
        //        MessageBox.Show("Error en la carga del archivo  en el número de fila " + (numeroFila + 2).ToString() + " " + ex.ToString(), "Atención");
        //    }
        //    finally
        //    {
        //        transOperaciones.Dispose();
        //        cnOperaciones.Close();
        //        Thread.CurrentThread.CurrentCulture = originalCulture;
        //    }
        //}

        //#region
        //private bool isFeriado(DateTime fecha,SqlConnection cn, SqlTransaction trans)
        //{
        //    SimulacionRetencionMasivaDA simulacionRetencionMasivaDA = new SimulacionRetencionMasivaDA();

        //    int cantidad = simulacionRetencionMasivaDA.ListarFechasFeriados(fecha,cn,trans).Rows.Count;
        //    if (cantidad == 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
        //#endregion

        ///*
        // Además actualizará un registro de la TABLA_GENERAL de la BD Tributación que 
        // * indica que se va a realizar el proceso de simulación de cálculo de retenciones. Se manejarán 
        // * 2 transacciones para cada base de datos mencionada.
        //*/
        //public void actualizarFlagSimulacion(string usuario, string descripcionCorta) {
        //    SimulacionRetencionMasivaDA simulacionRetencionMasivaDA = new SimulacionRetencionMasivaDA();
        //    string server = string.Empty;
        //    string database = string.Empty;

        //    ConnectionManager cmTributacion = new ConnectionManager();

        //    server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorTributacion];
        //    database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosTributacion];

        //    SqlConnection cnTributacion = cmTributacion.GetTrustedConnection(server, database);
        //    cnTributacion.Open();

        //    SqlTransaction transTributacion = cnTributacion.BeginTransaction();

        //    try
        //    {
        //        string codigoTabla = "FLAG_CALC_RET_SIMUL";
        //        DataTable dtTablaGeneral = simulacionRetencionMasivaDA.listarTablaGeneralXCodigo(codigoTabla, cnTributacion, transTributacion);

        //        TablaGeneralTD.TABLA_GENERALRow drTablaGeneral = new TablaGeneralTD().TABLA_GENERAL.NewTABLA_GENERALRow();
        //        if (dtTablaGeneral.Rows.Count > 0)
        //        {
        //            drTablaGeneral.ID = Convert.ToDecimal(dtTablaGeneral.Rows[0]["ID"]);
        //            drTablaGeneral.LLAVE_TABLA = Convert.ToString(dtTablaGeneral.Rows[0]["LLAVE_TABLA"]);
        //            drTablaGeneral.NUMERO_ORDEN = Convert.ToDecimal(dtTablaGeneral.Rows[0]["NUMERO_ORDEN"]);
        //            drTablaGeneral.DESCRIPCION_CORTA = descripcionCorta;
        //            drTablaGeneral.DESCRIPCION_LARGA = Convert.ToString(dtTablaGeneral.Rows[0]["DESCRIPCION_LARGA"]);
        //            drTablaGeneral.FLAG_ELIMINADO = Convert.ToString(dtTablaGeneral.Rows[0]["FLAG_ELIMINADO"]);
        //            drTablaGeneral.USUARIO = usuario;

        //            simulacionRetencionMasivaDA.actualizarTablaGeneralXId(drTablaGeneral, cnTributacion, transTributacion);
        //        }

        //        transTributacion.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        transTributacion.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        transTributacion.Dispose();
        //        cnTributacion.Close();
        //    }
        //}

        //public void actualizarFlagSimulacionAdmCuentas(string usuario, string descripcionCorta)
        //{
        //    SimulacionRetencionMasivaDA simulacionRetencionMasivaDA = new SimulacionRetencionMasivaDA();
        //    string server = string.Empty;
        //    string database = string.Empty;

        //    ConnectionManager cmOperaciones = new ConnectionManager();

        //    server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorOperaciones];
        //    database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosOperaciones];

        //    SqlConnection cnOperaciones = cmOperaciones.GetTrustedConnection(server, database);
        //    cnOperaciones.Open();

        //    SqlTransaction transOperaciones = cnOperaciones.BeginTransaction();

        //    try
        //    {
        //        string codigoTablaAdmCuentas = "FLAG_CALC_RET_SIMUL";
        //        DataTable dtTablaGeneralAdmCuentas = simulacionRetencionMasivaDA.listarTablaGeneralAdmCuentas(codigoTablaAdmCuentas, cnOperaciones, transOperaciones);
        //        TablaGeneralAdmCuenta tga = new TablaGeneralAdmCuenta();              
        //        if (dtTablaGeneralAdmCuentas.Rows.Count > 0)
        //        {
        //            tga.Id = Convert.ToInt32(dtTablaGeneralAdmCuentas.Rows[0]["ID"]);
        //            tga.LlaveTabla = Convert.ToString(dtTablaGeneralAdmCuentas.Rows[0]["LLAVE_TABLA"]);
        //            tga.NumeroOrden = Convert.ToInt32(dtTablaGeneralAdmCuentas.Rows[0]["NUMERO_ORDEN"]);
        //            tga.DescripcionCorta = descripcionCorta;
        //            tga.DescripcionLarga = Convert.ToString(dtTablaGeneralAdmCuentas.Rows[0]["DESCRIPCION_LARGA"]);
        //            tga.Abreviacion = Convert.ToString(dtTablaGeneralAdmCuentas.Rows[0]["ABREVIACION"]);
        //            tga.Estado = Convert.ToString(dtTablaGeneralAdmCuentas.Rows[0]["ESTADO"]);

        //            simulacionRetencionMasivaDA.actualizarTablaGeneralAdmCuentas(tga, cnOperaciones, transOperaciones);
        //        }

        //        transOperaciones.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        transOperaciones.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        transOperaciones.Dispose();
        //        cnOperaciones.Close();
        //    }
        //}

        //public void GenerarAtribucionSimulacion(int idFondo,DateTime fecha,string usuario) {
        //    SimulacionRetencionMasivaDA simulacionRetencionMasivaDA = new SimulacionRetencionMasivaDA();
        //    try
        //    {
        //        DataTable dtFondos = simulacionRetencionMasivaDA.ObtenerFondosPrecierre();
        //        string tipoAcceso = string.Empty;
        //        if (dtFondos.Rows.Count > 0)
        //        {
        //            tipoAcceso = dtFondos.Rows[0]["TIPO_ACCESO"].ToString();
        //            AtribucionBL atribucionBL = new AtribucionBL(usuario);
        //            atribucionBL.GenerarAtribucionPrecierre(idFondo, fecha, usuario, false, "", tipoAcceso);
        //            atribucionBL.RevertirTributacion(idFondo, fecha, usuario, "D");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //finally
        //    //{

        //    //}

        //}

        //public void ProcesarSimulacionRetencionMasiva(string archivo, string idFondoCadena, string usuario)
        //{
        //    string descripcionCorta = ConstantesING.SI;
        //    DateTime fechaActual = DateTime.Now;
        //    DateTime fechaDiaAnterior = fechaActual.AddDays(-1);
            
        //    try
        //    {
        //        EliminarTablasSimulacion(usuario);
        //        CargarRescateSimulacionMasivo(archivo, idFondoCadena, fechaActual, usuario);
        //        actualizarFlagSimulacion(usuario, descripcionCorta);
        //        actualizarFlagSimulacionAdmCuentas(usuario, descripcionCorta);
        //        GenerarAtribucionSimulacion(Convert.ToInt32(idFondoCadena), fechaDiaAnterior, usuario);

        //        //MessageBox.Show("Se cargaron " + numeroFila + " registros de " + "rescates simulados.", "Mensaje");
        //        MessageBox.Show("Se calcularon las retenciones de los rescates simulados.", "Mensaje");
			
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error en la simulación del cálculo de retenciones." + " " + ex.Message, "Atención");
        //    }
        //    finally {
        //        descripcionCorta = ConstantesING.NO;
        //        actualizarFlagSimulacion(usuario, descripcionCorta);
        //        actualizarFlagSimulacionAdmCuentas(usuario, descripcionCorta);
        //    }
        //}


	}
}

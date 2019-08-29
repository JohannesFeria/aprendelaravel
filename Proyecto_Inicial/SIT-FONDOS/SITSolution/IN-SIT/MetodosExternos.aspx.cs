using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using System.Data;
using SistemaProcesosBL;
using SistemaProcesosTD;
using SistemaProcesosTD.Constantes;
using System.Collections;

using System.Text;
using FondosSuraWS_Email;


/// <summary>
/// CRumiche: Servicios necesarios para la Lectura y Ejecucion de datos del Cierre del Valor Cuota en el Sistema Operaciones
/// </summary>
public partial class MetodosExternos : System.Web.UI.Page
{

    #region "CRumiche: Servicios necesarios para la Lectura y Ejecucion"

    [WebMethod]
    public static string getTipoCambioOperacion(string fecha)
    {
        try
        {
            AtribucionBL atribucionBL = new AtribucionBL("");
            DataTable dtTipoCambio = atribucionBL.ObtenerTipoCambioContable(Convert.ToDateTime(fecha));
            decimal tipoCambioSafp = dtTipoCambio.Rows.Count > 0 ? Convert.ToDecimal(dtTipoCambio.Rows[0]["Safp"]) : 1;
            return tipoCambioSafp.ToString();
        }
        catch (Exception ex)
        {
            return "1";
        }
    }

    [WebMethod]
    public static ProcesoPreCierreResult Get_VerificarPreviewPrecierre(decimal idFondoOpe, string codUsuario)
    {
        DataSet dsInfoPreview;
        StringBuilder rescatesRechazados;

        try
        {
            VerificarPreviewPrecierre(idFondoOpe, codUsuario, true, true, out dsInfoPreview, out rescatesRechazados);
            return ObtenerResultadoVerificacion(dsInfoPreview, "V", rescatesRechazados.ToString().Trim());
        }
        catch (Exception ex){
            return new ProcesoPreCierreResult { Notificaciones = ex.Message, ProcesoErrado = true };
        }
    }

    [WebMethod]
    // Inicio | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018
    public static ProcesoPreCierreResult Get_EjecutarPrecierre(decimal comisionSAFM, decimal idFondoOpe, int enviarCorreos, string codUsuario)
    {
        DataSet dsInfoEjecucion;
        StringBuilder erroresCorreo, erroresVarios;
        try
        {
            // Inicio | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018
            EjecutarPrecierre(comisionSAFM  , idFondoOpe, (enviarCorreos == 1), codUsuario, out dsInfoEjecucion, out erroresCorreo, out erroresVarios);
            // Fin | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018
            erroresCorreo.AppendLine(erroresVarios.ToString()); // Unificamos todos los errores para mostrar

            return ObtenerResultadoVerificacion(dsInfoEjecucion, "E", erroresCorreo.ToString().Trim());
        }
        catch (Exception ex)
        {
            return new ProcesoPreCierreResult { Notificaciones = ex.Message, ProcesoErrado = true };
        }
    }
    // Fin | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018
    [WebMethod]
    public static ProcesoPreCierreResult Get_InfoPrecierreHistorico(decimal idFondoOpe, string fecha)
    {
        try
        {
            PrecierreBO bo = new PrecierreBO();
            DataRow row = bo.ObtenerDetalleValorCuotaCerrado(idFondoOpe, UIUtility.ConvertirStringaFecha(fecha));

            DataSet dsSoloLectura = new DataSet();
            dsSoloLectura.Tables.Add(row.Table);
            dsSoloLectura.Tables.Add("Eventos");

            return ObtenerResultadoVerificacion(dsSoloLectura, "L", "");
        }
        catch (Exception ex)
        {
            return new ProcesoPreCierreResult { Notificaciones = ex.Message, ProcesoErrado = true };
        }
    }

    #endregion


    #region "CRumiche: Lógica y Utilitarios para trabajar el Precierre y sus datos"

    public class ProcesoPreCierreResult
    {
        public string Fondo = "";
        public string Fecha = "";
        public string ValorCuota = "";

        public string SaldoAyer = "";
        public string SaldoDia = "";

        public string SaldoCuotasAyer = "";
        public string SaldoCuotasDia = "";

        public string TotalIngresoBruto = "";
        public string TotalEgresoBruto = "";

        public string TotalIngresoNeto = "";
        public string TotalEgresoNeto = "";

        public string TotalIngresoCuotas = "";
        public string TotalEgresoCuotas = "";

        public string ConversionCuotas = "";
        public string EgresoCuotasRT = "";

        public string EgresoMontoRT = "";
        public string PagoFlujoMonto = "";
        public string RetencionFlujo = "";


        public string Notificaciones = "";
        public string TipoProceso = "";
        public bool ProcesoErrado = false; 
    }

    public static void wsEmail_enviarEmail(string p1, string p2, string p3, out bool p4, out bool p5)
    {
        EmailManager email = new EmailManager();
        email.enviarEmail(p1, p2, p3, out p4, out p5);
    }

    /*CRumiche: tipoProceso=> V: VERIFICACION, E: EJECUCION, L: Solo Lectura (u Otros) */
    public static ProcesoPreCierreResult ObtenerResultadoVerificacion(DataSet dsInfoPreview, string tipoProceso , string mensajesOErrores)
    {
        ProcesoPreCierreResult resul = new ProcesoPreCierreResult();
        resul.TipoProceso = tipoProceso;
        resul.Notificaciones = mensajesOErrores;

        DataTable dtPrecierre = dsInfoPreview.Tables[0];
        DataTable dtEventos = dsInfoPreview.Tables[1];

        DataRow rowPrecierre = dtPrecierre.Rows[0];

        resul.Fondo = rowPrecierre["NOMBRE"].ToString();
        resul.Fecha = String.Format("{0:dd/MM/yyyy}", rowPrecierre["FECHA"]);
        resul.ValorCuota = String.Format("{0:#,##0.00000000}", rowPrecierre["VALOR_CUOTA"] is DBNull ? 0 : rowPrecierre["VALOR_CUOTA"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();

        resul.SaldoAyer = String.Format("{0:n}", rowPrecierre["TOTAL_MONTO_ANTERIOR"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();
        resul.SaldoDia = String.Format("{0:n}", rowPrecierre["TOTAL_MONTO_ACUMULADO"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();

        resul.SaldoCuotasAyer = String.Format("{0:#,##0.00000000}", rowPrecierre["TOTAL_CUOTAS_ANTERIOR"]);
        resul.SaldoCuotasDia = String.Format("{0:#,##0.00000000}", rowPrecierre["TOTAL_CUOTAS_ACUMULADO"]);

        resul.TotalIngresoBruto = String.Format("{0:n}", rowPrecierre["INGRESO_BRUTO"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();
        resul.TotalEgresoBruto = String.Format("{0:n}", rowPrecierre["EGRESO_BRUTO"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();

        resul.TotalIngresoNeto = String.Format("{0:n}", rowPrecierre["INGRESO_NETO"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();
        resul.TotalEgresoNeto = String.Format("{0:n}", rowPrecierre["EGRESO_NETO"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();

        resul.TotalIngresoCuotas = String.Format("{0:#,##0.00000000}", rowPrecierre["INGRESO_CUOTAS"]);
        resul.TotalEgresoCuotas = String.Format("{0:#,##0.00000000}", rowPrecierre["EGRESO_CUOTAS"]);

        if (rowPrecierre.Table.Columns.Contains("RETENCION_TRASPASO_CUOTAS"))// Puede que el metodo de solo lectura no lo traiga
        {
            resul.EgresoCuotasRT = String.Format("{0:#,##0.00000000}", rowPrecierre["RETENCION_TRASPASO_CUOTAS"]);
            resul.EgresoMontoRT = String.Format("{0:n}", rowPrecierre["RETENCION_TRASPASO_MONTO"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();   
        }

        if (rowPrecierre["PAGA_CUPON"] != null && rowPrecierre["PAGA_CUPON"].ToString().Trim() != "S")
        {
            resul.ConversionCuotas = "-"; // El Valor de "-" nos indicará que en HTML no será mostrado
            resul.PagoFlujoMonto = "-"; // El Valor de "-" nos indicará que en HTML no será mostrado
            resul.RetencionFlujo = "-"; // El Valor de "-" nos indicará que en HTML no será mostrado
        }
        else
        {
            if (rowPrecierre.Table.Columns.Contains("CONVERSION_CUOTAS")) // Puede que el metodo de solo lectura no lo traiga
            {
                resul.ConversionCuotas = String.Format("{0:n}", rowPrecierre["CONVERSION_CUOTAS"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();
                resul.PagoFlujoMonto = String.Format("{0:n}", rowPrecierre["MONTO_PAGO_FLUJO_NETO"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();
                resul.RetencionFlujo = String.Format("{0:n}", rowPrecierre["MONTO_RETENCION_FLUJO_NETO"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();
            }
            else if(tipoProceso == "E")
            {
                 resul.PagoFlujoMonto = String.Format("{0:n}", rowPrecierre["MONTO_PAGO_FLUJO_NETO"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();
                 resul.RetencionFlujo = String.Format("{0:n}", rowPrecierre["MONTO_RETENCION_FLUJO_NETO"]) + " " + rowPrecierre["CODIGO_MONEDA"].ToString();
            }
        }        

        return resul;
    }

    public static void VerificarPreviewPrecierre(decimal idFondoOpe, string usuario, bool procesarAtribucionRenta, bool mantener_VCuotaSit_Si_VCuotaOpe_Es_Diferente, out DataSet dsInfoPreview, out StringBuilder rescatesRechazados)
    {
        string flagVerificacion = "2";        
        
        // Evaluamos los requisitos del Fondo antes de mostrar la ventana del Preview
        EvaluarRequisitosFondo(Convert.ToInt32(idFondoOpe), usuario, procesarAtribucionRenta, mantener_VCuotaSit_Si_VCuotaOpe_Es_Diferente);

        rescatesRechazados = new StringBuilder();

        PrecierreBO bo = new PrecierreBO(usuario);
        dsInfoPreview = bo.PreviewPrecierre(idFondoOpe, flagVerificacion);

        DataTable dtPreview = dsInfoPreview.Tables[0]; // 
        DataTable dtRescates = dsInfoPreview.Tables[1]; // RESCATES RECHAZADOS

        foreach (DataRow row in dtRescates.Rows)
        {
            rescatesRechazados.AppendLine(row["FONDO"].ToString() + " - " + row["DATOS_RESCATE"].ToString());            
        }        
    }
    // Inicio | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018
    public static void EjecutarPrecierre(decimal comisionSAFM, decimal idFondoOpe, bool enviarCorreos, string codUsuario, out DataSet dsInfoEjecucion, out StringBuilder erroresCorreo, out StringBuilder erroresVarios)
    {
        string flagVerificacion = "2"; // Constante segú  la programación encontrada en Operaciones
        //ArrayList result = new ArrayList();
        erroresCorreo = new StringBuilder();
        erroresVarios = new StringBuilder();

        PrecierreBO bo = new PrecierreBO(codUsuario);
        DataSet dsPreview = bo.PreviewPrecierre(idFondoOpe, flagVerificacion);

        DataTable dtPreview = dsPreview.Tables[0]; // 
        DataTable dtRescatesRechazados = dsPreview.Tables[1]; // RESCATES RECHAZADOS


        ArrayList resultAlertasExcesos = new ArrayList();
        DataTable correosPrecierre = bo.ListarTablaGeneral(ConstantesING.CODIGO_TABLA_INFO_PRECIERRE);

        // string idFondo;
        string nombreFondo;
        DateTime fechaFondo;

        // Auxiliares
        bool returnValue, returnValueSpecified;

        DataRow drPre = dtPreview.Rows[0];
        try
        {
            DateTime horario = Convert.ToDateTime(drPre["HORARIO"]);
            DateTime horarioParametro = new DateTime(1900, 1, 1, horario.Hour, horario.Minute, 0);
            // Inicio | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018
            dsInfoEjecucion = bo.EjecutarPrecierre(
                                            comisionSAFM,
                                            idFondoOpe,
                                            flagVerificacion,
                                            Convert.ToDateTime(drPre["FECHA"]),
                                            horarioParametro,
                                            Decimal.Parse(drPre["VALOR_CUOTA"].ToString()));
            // Fin | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018
            resultAlertasExcesos.Add(bo.ListarAlertasExcesos(idFondoOpe));

            //Se almacena en una lista los fondos - su ID y fecha de precierre que se han precerrado exitosamente.
            // idFondo = idFondoOpe.ToString();
            nombreFondo = drPre["DESCRIPCION"].ToString();  // message += "Se ha precerrado fondo " + dr["DESCRIPCION"].ToString() + " exitósamente.\n";
            fechaFondo = Convert.ToDateTime(drPre["FECHA"]);

            /*INICIO 8844*/
            foreach (DataRow infoCorreo in correosPrecierre.Rows)
            {
                try
                {
                    wsEmail_enviarEmail(infoCorreo["DESCRIPCION_LARGA"].ToString(),
                        "Precierre de fondo " + drPre["NOMBRE"].ToString() + " " + Convert.ToDateTime(drPre["FECHA"]).ToString("dd/MM/yyyy"),
                        NotificarPrecierreEjecutado(drPre["NOMBRE"].ToString(), Convert.ToDateTime(drPre["FECHA"]), Convert.ToDecimal(drPre["VALOR_CUOTA"])),
                        out returnValue,
                        out returnValueSpecified);
                }
                catch (Exception ex)
                {
                    erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + infoCorreo["DESCRIPCION_LARGA"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("El fondo " + drPre["DESCRIPCION"].ToString() + " no se pudo precerrar por el siguiente error: " + ex.Message);
        }
        // Fin | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018

        //OT11264 INI
        DataTable dtDestinatariosAlerta = new DataTable();
        dtDestinatariosAlerta = bo.ListarTablaGeneral(ConstantesING.CODIGO_CORREOS_DESTINATARIO_ALERTA);
        //OT11264 FIN


        //OT 7968 INI
        #region Mail de operaciones que están pendientes de modificar el tipo de cambio por tener cruce de monedas

        //Usuarios que recibirán el correo de Alerta de Registro de TC para Solicitudes de Traspasos
        DataTable dtCorreosUsuariosSURA = bo.ListarTablaGeneral(ConstantesING.MAIL_TC_TRASPASO);
        DataTable dtTraspasosSinTC = bo.ObtenerTraspasoFondosPendienteTipoCambio(idFondoOpe, fechaFondo);
        bool returnValueTC, returnValueSpecifiedTC;

        if (dtTraspasosSinTC.Rows.Count > 0)
        {
            foreach (DataRow drCorreoUsuarioSURA in dtCorreosUsuariosSURA.Rows)
            {
                try
                {
                    wsEmail_enviarEmail(drCorreoUsuarioSURA["DESCRIPCION_LARGA"].ToString(),
                        "Alerta de Registro de TC para Solicitudes de Traspasos",
                        crearEmailOperacionesPendienteTipoCambio(dtTraspasosSinTC),
                        out returnValueTC,
                        out returnValueSpecifiedTC);
                }
                /*INICIO 8844*/
                catch (Exception ex)
                {
                    erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + drCorreoUsuarioSURA["DESCRIPCION_LARGA"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                }
                /*FIN 8844*/
            }
        }
        #endregion
        //OT 7968 FIN

        //OT11247 INI
        string codigoTablaFondoFirbi = ConstantesING.CODIGO_TABLA_FONDOS_FIRBI;
        DataTable dtFondosFirbi = bo.ListarTablaGeneral(codigoTablaFondoFirbi);
        //OT11247 FIN

        if (enviarCorreos && dsInfoEjecucion != null)
        {
            //if (result.Count > 0)
            //{
                //foreach (DataSet dsInfoEjecucion in result)
                //{
            DataTable dtSuscripciones = dsInfoEjecucion.Tables[2];
            DataTable dtRescates = dsInfoEjecucion.Tables[3];
            DataTable dtRetenciones = dsInfoEjecucion.Tables[4];

            //OT 7968 INI

            DataTable dtSuscripcionesAsociadasTraspasoFondos = dsInfoEjecucion.Tables[5];

            //OT 7968 FIN

            //OT 8911 INI
            //DataTable dtConversionCuotasOrigen = ds.Tables[6];
            //DataTable dtConversionCuotasDestino = ds.Tables[7];
            //DataTable dtParticipeDestinoTraspasoAsociadoConversionCuotas = ds.Tables[8];

            DataTable dtConversionCuotasDestino = dsInfoEjecucion.Tables[6];
            DataTable dtParticipeDestinoTraspasoAsociadoConversionCuotas = dsInfoEjecucion.Tables[7];
            //OT 8911 FIN

            //OT8895 INI
            DataTable dtPagoFlujo = dsInfoEjecucion.Tables[8];
            //OT8895 FIN

         
            DataTable dtPagoFlujoCapitalInteres = dsInfoEjecucion.Tables[9]; // OT11436
            //DataTable dtLiberacionCuotas = dsInfoEjecucion.Tables[9]; //Se agrega en Tables(10) 29/10/2018
            DataTable dtLiberacionCuotas = dsInfoEjecucion.Tables[10]; //OT10839 PSC001

            foreach (DataRow dr in dtSuscripciones.Rows)
            {
                try
                {
                    wsEmail_enviarEmail(dr["CORREO"].ToString(), "Confirmación de Operación de Suscripción", crearEmailSuscripciones(dr), out returnValue, out returnValueSpecified);
                    //OT11264 INI
                    if (dr["TIPO_PARTICIPE"].ToString() != String.Empty)
                    {
                        String subject = "";
                        foreach (DataRow drDestinatariosAlerta in dtDestinatariosAlerta.Rows)
                        {
                            subject = "Actividad de cliente " + dr["TIPO_PARTICIPE"].ToString() + " (" + dr["CUC"].ToString() + ") - (" + dr["NOMBRE_PARTICIPE"].ToString() + ") -" + "Fondos SURA";
                            wsEmail_enviarEmail(drDestinatariosAlerta["DESCRIPCION_LARGA"].ToString(), subject, crearAlertaEmailSuscripciones(dr), out returnValue, out returnValueSpecified);
                        }

                        //OT11264 PSC002 INI
                        try
                        {
                            int idParticipe = Convert.ToInt32(dr["ID_PARTICIPE"].ToString());
                            int idFondo = Convert.ToInt32(dr["ID_FONDO"].ToString());
                            int idOperacion = Convert.ToInt32(dr["ID_OPERACION"].ToString());
                            DateTime fechaProceso = Convert.ToDateTime(dr["FECHA_PROCESO"].ToString());
                            bo.InsertarAlertaActividadCliente(idParticipe, idFondo, idOperacion, fechaProceso);
                        }
                        catch (Exception ex)
                        {                                   
                            erroresVarios.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo insertar en Alerta Actividad de Cliente. Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                        }
                        //OT11264 PSC002 FIN
                    }
                }
                /*INICIO 8844*/
                catch (Exception ex)
                {
                    erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + dr["CORREO"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                }
                /*FIN 8844*/
            }

            foreach (DataRow dr in dtRescates.Rows)
            {
                try
                {
                    wsEmail_enviarEmail(dr["CORREO"].ToString(), "Confirmación de Operación de Rescate", crearEmailRescates(dr), out returnValue, out returnValueSpecified);
                    //OT11264 INI
                    if (dr["TIPO_PARTICIPE"].ToString() != String.Empty)
                    {
                        String subject = "";

                        foreach (DataRow drDestinatariosAlerta in dtDestinatariosAlerta.Rows)
                        {
                            subject = "Actividad de cliente " + dr["TIPO_PARTICIPE"].ToString() + " (" + dr["CUC"].ToString() + ") - (" + dr["NOMBRE_PARTICIPE"].ToString() + ") -" + "Fondos SURA";
                            wsEmail_enviarEmail(drDestinatariosAlerta["DESCRIPCION_LARGA"].ToString(), subject, crearAlertaEmailRescates(dr), out returnValue, out returnValueSpecified);
                        }

                        //OT11264 PSC002 INI
                        try
                        {
                            int idParticipe = Convert.ToInt32(dr["ID_PARTICIPE"].ToString());
                            int idFondo = Convert.ToInt32(dr["ID_FONDO"].ToString());
                            int idOperacion = Convert.ToInt32(dr["ID_OPERACION"].ToString());
                            DateTime fechaProceso = Convert.ToDateTime(dr["FECHA_PROCESO"].ToString());
                            bo.InsertarAlertaActividadCliente(idParticipe, idFondo, idOperacion, fechaProceso);
                        }
                        catch (Exception ex)
                        {
                            erroresVarios.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo insertar en Alerta Actividad de Cliente . Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                        }
                        //OT11264 PSC002 FIN
                    }
                    //OT11264 FIN
                }
                /*INICIO 8844*/
                catch (Exception ex)
                {
                    erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + dr["CORREO"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                }
                /*FIN 8844*/
            }

            foreach (DataRow dr in dtRetenciones.Rows)
            {
                try
                {
                    wsEmail_enviarEmail(dr["CORREO"].ToString(), "Confirmación de Retención Anual - Fondos SURA", crearEmailRetenciones(dr), out returnValue, out returnValueSpecified);
                }
                /*INICIO 8844*/
                catch (Exception ex)
                {
                    erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + dr["CORREO"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                }
                /*FIN 8844*/
            }

            //OT 7968 INI

            //OT 7968 PSC 001 INI
          
            foreach (DataRow dr in dtSuscripcionesAsociadasTraspasoFondos.Rows)
            {
                try
                {                                
                    wsEmail_enviarEmail(dr["CORREO_PARTICIPE_ORIGEN"].ToString(), "Confirmación de Operación de Traspaso",
                        crearEmailSuscripcionesPorTraspaso(dr), out returnValue, out returnValueSpecified);
                }
                catch (Exception ex)
                {
                    erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + dr["CORREO"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                }
            }
                
            //OT 7968 PSC 001 FIN

            //OT 7968 FIN

            //OT 8911 INI

            /*
            foreach (DataRow dr in dtConversionCuotasOrigen.Rows)
            {
                try
                {
                    wsEmail_enviarEmail(dr["CORREO"].ToString(), "Confirmación de Operación de Conversión de Cuotas - Origen", crearEmailConversionCuotasOrigen(dr), out returnValue, out returnValueSpecified);
                }
                catch (Exception ex)
                {
                    log.Write(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + dr["CORREO"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                    errorCorreo = true;
                }
            }
            */

            foreach (DataRow dr in dtConversionCuotasDestino.Rows)
            {
                try
                {
                    wsEmail_enviarEmail(dr["CORREO"].ToString(), "Confirmación de Operación de Conversión de Cuotas  - Destino", crearEmailConversionCuotasDestino(dr), out returnValue, out returnValueSpecified);
                }
                catch (Exception ex)
                {
                    erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + dr["CORREO"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                }
            }

            foreach (DataRow dr in dtParticipeDestinoTraspasoAsociadoConversionCuotas.Rows)
            {
                try
                {
                    wsEmail_enviarEmail(dr["CORREO"].ToString(), "Confirmación de Operación Traspaso asociado a Conversión de Cuotas - Destino", crearEmailParticipesDestinoTraspasoAsociadoConversionCuotas(dr), out returnValue, out returnValueSpecified);
                    //OT11264 INI
                    if (dr["TIPO_PARTICIPE"].ToString() != String.Empty)
                    {
                        String subject = "";
                        foreach (DataRow drDestinatariosAlerta in dtDestinatariosAlerta.Rows)
                        {
                            subject = "Actividad de cliente " + dr["TIPO_PARTICIPE"].ToString() + " (" + dr["CUC"].ToString() + ") - (" + dr["NOMBRE_PARTICIPE"].ToString() + ") -" + "Fondos SURA";
                            wsEmail_enviarEmail(drDestinatariosAlerta["DESCRIPCION_LARGA"].ToString(), subject, crearAlertaEmailTPS(dr), out returnValue, out returnValueSpecified);
                        }

                        //OT11264 PSC002 INI
                        try
                        {
                            int idParticipe = Convert.ToInt32(dr["ID_PARTICIPE"].ToString());
                            int idFondo = Convert.ToInt32(dr["ID_FONDO"].ToString());
                            int idOperacion = Convert.ToInt32(dr["ID_OPERACION"].ToString());
                            DateTime fechaProceso = Convert.ToDateTime(dr["FECHA_PROCESO"].ToString());
                            bo.InsertarAlertaActividadCliente(idParticipe, idFondo, idOperacion, fechaProceso);
                        }
                        catch (Exception ex)
                        {
                            erroresVarios.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo insertar en Alerta Actividad de Cliente . Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                        }
                        //OT11264 PSC002 FIN
                    }
                    //OT11264 FIN

                }
                catch (Exception ex)
                {
                    erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + dr["CORREO"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                }
            }
            //OT 8911 FIN

            //OT11622 INI
        #region Código comentado por la OT11622
            //foreach (DataRow dr in dtPagoFlujo.Rows)
            //{
            //    try
            //    {
            //        //OT11247 INI
            //        //wsEmail_enviarEmail(dr["CORREO"].ToString(), "Confirmación de Operación de Pago Programado", crearEmailPagoFlujo(dr), out returnValue, out returnValueSpecified);
            //        if (dr["ID_FONDO"] != null && dr["ID_FONDO"] != DBNull.Value)
            //        {
            //            int idFon = Convert.ToInt32(dr["ID_FONDO"]);
            //            string idFondoCadena = Convert.ToString(idFon);

            //            //Verificando si el fondo es FIRBI
            //            DataRow[] drFonFirbi = dtFondosFirbi.Select("LLAVE_TABLA = '" + idFondoCadena + "'");
            //            if (drFonFirbi.Length > 0)//es fondo Firbi
            //            {
            //                wsEmail_enviarEmail(dr["CORREO"].ToString(), "Confirmación de Distribución de Resultados", crearEmailPagoFlujo(dr), out returnValue, out returnValueSpecified);
            //            }
            //            else
            //            {
            //                wsEmail_enviarEmail(dr["CORREO"].ToString(), "Confirmación de Operación de Pago Programado", crearEmailPagoFlujoNoFIR(dr), out returnValue, out returnValueSpecified);
            //            }
            //        }
            //        //OT11247 FIN
            //    }
            //    catch (Exception ex)
            //    {
            //        erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + dr["CORREO"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace + "\r\n");
            //    }
            //}
            ////OT11436 INI
            //foreach (DataRow dr in dtPagoFlujoCapitalInteres.Rows)
            //{
            //    try
            //    {
            //        wsEmail_enviarEmail(dr["CORREO"].ToString(), "Confirmación de Distribución de Resultados", crearEmailPagoFlujoFondoPrivadoCapitalInteres(dr), out returnValue, out returnValueSpecified);
            //    }
            //    catch (Exception ex)
            //    {
            //        erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + dr["CORREO"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
            //    }
            //}
            //OT11436 FIN
          #endregion
            //OT11622 FIN



            //OT10839 PSC001 INI
            foreach (DataRow dr in dtLiberacionCuotas.Rows)
            {
                try
                {
                    wsEmail_enviarEmail(dr["CORREO"].ToString(), "Confirmación de Operación de Liberación de cuotas", crearEmailLiberacionCuotas(dr), out returnValue, out returnValueSpecified);
                }

                catch (Exception ex)
                {
                    erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + dr["CORREO"].ToString() + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                }
            }
            //OT10839 PSC001 FIN
            //}

            if (resultAlertasExcesos != null)
            {
                foreach (DataSet dsAlertas in resultAlertasExcesos)
                {

                    DataTable dtTerceroCartaGrupo = dsAlertas.Tables[0];
                    DataTable dtTerceroCartaPersona = dsAlertas.Tables[1];
                    DataTable dtTerceroAlertaGrupo = dsAlertas.Tables[2];
                    DataTable dtTerceroAlertaPersona = dsAlertas.Tables[3];
                    DataTable dtPropioAlertaGrupo = dsAlertas.Tables[4];
                    DataTable dtPropioAlertaPersona = dsAlertas.Tables[5];

                    string cuerpo = string.Empty;
                    string correo = string.Empty;
                    string subject = string.Empty;


                    cuerpo = "Se ha generado los siguientes clientes para envío de carta:<br><br>";

                    //Alerta de cartas

                    foreach (DataRow dr in dtTerceroCartaGrupo.Rows)
                    {

                        subject = dr["NOMBRE"] + ": Alertas de generación de carta de exceso.";
                        correo = dr["CORREO"].ToString();
                        cuerpo += "Nombre: " + dr["CLIENTE"].ToString() + ".<br>";
                        cuerpo += "Fecha inicio de exceso: " + Convert.ToDateTime(dr["FECHA_INICIO_CARTA"]).ToString("dd/MM/yyyy") + ".<br>";
                        cuerpo += "Fecha fin de plazo: " + Convert.ToDateTime(dr["FECHA_FIN_RESCATE"]).ToString("dd/MM/yyyy") + ".<br><br>";
                    }

                    foreach (DataRow dr in dtTerceroCartaPersona.Rows)
                    {
                        subject = dr["NOMBRE"] + ": Alertas de generación de carta de exceso.";
                        correo = dr["CORREO"].ToString();
                        cuerpo += "Nombre: " + dr["CLIENTE"].ToString() + ".<br>";
                        cuerpo += "Fecha inicio de exceso: " + Convert.ToDateTime(dr["FECHA_INICIO_CARTA"]).ToString("dd/MM/yyyy") + ".<br>";
                        cuerpo += "Fecha fin de plazo: " + Convert.ToDateTime(dr["FECHA_FIN_RESCATE"]).ToString("dd/MM/yyyy") + ".<br><br>";
                    }

                    cuerpo += "Ingrese a la herramienta de excesos para generar la carta.";

                    try
                    {
                        if (!cuerpo.Equals(""))
                        {
                            wsEmail_enviarEmail(correo, subject, cuerpo, out returnValue, out returnValueSpecified);
                        }
                    }
                    /*INICIO 8844*/
                    catch (Exception ex)
                    {
                        erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + correo + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                    }
                    /*FIN 8844*/

                    //Alertas de fin de plazo de exceso
                    subject = string.Empty;
                    cuerpo = string.Empty;
                    correo = string.Empty;

                    cuerpo = "Se ha generado los siguientes clientes para programar el rescate:<br><br>";

                    //cuerpo = "Se informa que se debe enviar la alerta de fin de plazo de exceso a los siguientes grupos:<br>";
                    foreach (DataRow dr in dtTerceroAlertaGrupo.Rows)
                    {
                        subject = dr["NOMBRE"] + ": Alertas de fin de plazo de exceso.";
                        correo = dr["CORREO"].ToString();
                        cuerpo += "Nombre: " + dr["CLIENTE"].ToString() + ".<br>";
                        cuerpo += "Fecha inicio de exceso: " + Convert.ToDateTime(dr["FECHA_INICIO_CARTA"]).ToString("dd/MM/yyyy") + ".<br>";
                        cuerpo += "Fecha fin de plazo: " + Convert.ToDateTime(dr["FECHA_FIN_RESCATE"]).ToString("dd/MM/yyyy") + ".<br><br>";
                    }

                    foreach (DataRow dr in dtTerceroAlertaPersona.Rows)
                    {
                        subject = dr["NOMBRE"] + ": Alertas de fin de plazo de exceso.";
                        correo = dr["CORREO"].ToString();
                        cuerpo += "Nombre: " + dr["CLIENTE"].ToString() + ".<br>";
                        cuerpo += "Fecha inicio de exceso: " + Convert.ToDateTime(dr["FECHA_INICIO_CARTA"]).ToString("dd/MM/yyyy") + ".<br>";
                        cuerpo += "Fecha fin de plazo: " + Convert.ToDateTime(dr["FECHA_FIN_RESCATE"]).ToString("dd/MM/yyyy") + ".<br><br>";
                    }

                    foreach (DataRow dr in dtPropioAlertaGrupo.Rows)
                    {
                        subject = dr["NOMBRE"] + ": Alertas de fin de plazo de exceso.";
                        correo = dr["CORREO"].ToString();
                        cuerpo += "Nombre: " + dr["CLIENTE"].ToString() + ".<br>";
                        cuerpo += "Fecha inicio de exceso: " + Convert.ToDateTime(dr["FECHA_INICIO_CARTA"]).ToString("dd/MM/yyyy") + ".<br>";
                        cuerpo += "Fecha fin de plazo: " + Convert.ToDateTime(dr["FECHA_FIN_RESCATE"]).ToString("dd/MM/yyyy") + ".<br><br>";
                    }

                    foreach (DataRow dr in dtPropioAlertaPersona.Rows)
                    {
                        subject = dr["NOMBRE"] + ": Alertas de fin de plazo de exceso.";
                        correo = dr["CORREO"].ToString();
                        cuerpo += "Nombre: " + dr["CLIENTE"].ToString() + ".<br>";
                        cuerpo += "Fecha inicio de exceso: " + Convert.ToDateTime(dr["FECHA_INICIO_CARTA"]).ToString("dd/MM/yyyy") + ".<br>";
                        cuerpo += "Fecha fin de plazo: " + Convert.ToDateTime(dr["FECHA_FIN_RESCATE"]).ToString("dd/MM/yyyy") + ".<br><br>";
                    }

                    try
                    {
                        if (!cuerpo.Equals(""))
                        {
                            wsEmail_enviarEmail(correo, subject, cuerpo, out returnValue, out returnValueSpecified);
                        }
                    }
                    /*INICIO 8844*/
                    catch (Exception ex)
                    {
                        erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + correo + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                    }
                    /*FIN 8844*/
                }
            }

            //OT 7370 INI
            //Ver si hay rescates significativos
            RescatesSignificativosBO rescatesSignificativosBO = new RescatesSignificativosBO();
            //bool rescatesSignificativos = false;

            int numeroRescateSignificativoGrupal = 0;
            int numeroRescateSignificativoIndividual = 0;

            // 7370 - PSC001
            //int banderaRescateSignificativoGrupal = 0;

            Hashtable htCodigoPadreProcesado = new Hashtable();
            string codigoPadre = string.Empty;

            // 7370 - PSC001
            string tipoRescate = string.Empty;
            string flagComposicionGrupal = string.Empty;
            // 7370 - PSC001

            //decimal idFondo = Convert.ToDecimal(drFondo["ID_FONDO"]);
            DataTable dtFechaPrecierre = bo.ObtenerUltimaFechaPrecierreXFondo(idFondoOpe);
            DateTime fechaPrecierre = Convert.ToDateTime(dtFechaPrecierre.Rows[0]["FECHA"]);

            //Verificando que se han precerrado a la fecha de proceso todos los fondos hijos del fondo padre
            DataTable dtFondoPadre = bo.VerificarFondoPadrePrecierre(idFondoOpe, fechaPrecierre);
            codigoPadre = string.Empty;
            if (dtFondoPadre.Rows.Count > 0)
            {
                codigoPadre = dtFondoPadre.Rows[0]["CODIGO_PADRE"].ToString();
                // 7370 - PSC001
                numeroRescateSignificativoGrupal = 0;
                numeroRescateSignificativoIndividual = 0;
                // 7370 - PSC001

                if (htCodigoPadreProcesado.ContainsKey(codigoPadre))
                {
                    //codigo padre ya fue procesado
                    //continue;
                }
                else
                {
                    //Se han precerrado a la fecha de proceso todos los fondos hijos del fondo padre 
                    // 7370 - PSC001
                    rescatesSignificativosBO.CalcularRescatesSignificativos(fechaPrecierre, codigoPadre);
                    DataTable dtRescatesSignificativos = rescatesSignificativosBO.ObtenerRescatesSignificativos(fechaPrecierre, codigoPadre);

                    //tipoRescate = "GRU"; //tipo grupal
                    flagComposicionGrupal = "S";
                    DataRow[] drRescatesSignificativoGrupal = dtRescatesSignificativos.Select(string.Format("FLAG_COMPOSICION_GRUPAL = '{0}'", flagComposicionGrupal));
                    if (drRescatesSignificativoGrupal.Length > 0)
                    {
                        numeroRescateSignificativoGrupal = 1;
                    }

                    tipoRescate = "IND"; //tipo individual
                    DataRow[] drRescatesSignificativoIndividual = dtRescatesSignificativos.Select(string.Format("TIPO = '{0}'", tipoRescate));
                    if (drRescatesSignificativoGrupal.Length > 0)
                    {
                        numeroRescateSignificativoIndividual = drRescatesSignificativoIndividual.Length;
                    }

                    if (dtRescatesSignificativos.Rows.Count > 0)
                    {
                        //mandar correo
                        string cuerpoCorreo = string.Empty;

                        if (numeroRescateSignificativoGrupal > 0 && numeroRescateSignificativoIndividual > 0)
                        {
                            cuerpoCorreo = "Se ha producido " + numeroRescateSignificativoGrupal.ToString() + " rescate significativo del tipo Grupal y ";
                            cuerpoCorreo = cuerpoCorreo + numeroRescateSignificativoIndividual.ToString() + " rescate(s) significativo(s) del tipo Individual en el fondo ";
                            cuerpoCorreo = cuerpoCorreo + drRescatesSignificativoGrupal[0]["NOMBRE_FONDO_PADRE"].ToString();
                            cuerpoCorreo = cuerpoCorreo + ". Por favor revisar el reporte a la fecha.";
                        }
                        else
                        {
                            if (numeroRescateSignificativoGrupal > 0)
                            {
                                cuerpoCorreo = "Se ha producido " + numeroRescateSignificativoGrupal.ToString() + " rescate significativo del tipo Grupal en el fondo ";
                                cuerpoCorreo = cuerpoCorreo + drRescatesSignificativoGrupal[0]["NOMBRE_FONDO_PADRE"].ToString() + ".";
                                cuerpoCorreo = cuerpoCorreo + " Por favor revisar el reporte a la fecha.";
                            }
                            else
                            {
                                cuerpoCorreo = "Se ha(n) producido " + numeroRescateSignificativoIndividual.ToString() + " rescate(s) significativo(s) del tipo Individual en el fondo ";
                                cuerpoCorreo = cuerpoCorreo + drRescatesSignificativoIndividual[0]["NOMBRE_FONDO_PADRE"].ToString() + ".";
                                cuerpoCorreo = cuerpoCorreo + " Por favor revisar el reporte a la fecha.";
                            }
                        }
                        // 7370 - PSC001
                        //obtener direccion de email destino
                        string codigoTabla = "CORREO_RESCATE_SIG";
                        string llaveTabla = "RES_SIG";

                        DataTable dtEmailDestinoRescateSignificativo = rescatesSignificativosBO.ObtenerTablaGeneral(codigoTabla, llaveTabla);
                        string emailDestinoRescateSignificativo = string.Empty;
                        if (dtEmailDestinoRescateSignificativo.Rows.Count > 0)
                        {
                            emailDestinoRescateSignificativo = Convert.ToString(dtEmailDestinoRescateSignificativo.Rows[0]["DESCRIPCION_LARGA"]);
                        }

                        string asuntoCorreoRescateSignificativo = null;
                        asuntoCorreoRescateSignificativo = "Rescate Significativo";
                        try
                        {
                            wsEmail_enviarEmail(emailDestinoRescateSignificativo, asuntoCorreoRescateSignificativo, cuerpoCorreo, out returnValue, out returnValueSpecified);
                        }
                        /*INICIO 8844*/
                        catch (Exception ex)
                        {
                            erroresCorreo.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - No se pudo enviar el correo a " + emailDestinoRescateSignificativo + ". Detalle: " + ex.Message + "\r\n" + ex.StackTrace);
                        }
                        /*FIN 8844*/
                    }
                }
            }

            //agregar en Hash
            htCodigoPadreProcesado.Add(codigoPadre, codigoPadre);                
            //}
            //OT 7370 FIN

            //string mensajeAdicional = errorCorreo ? "\r\nSe ha encontrado error en el envío de correos.Verificar el log de errores." : "";
            //MessageBox.Show(message + mensajeAdicional, "Procesos");
            /*FIN 8844*/
            //FrmPrecierreResumen frm = new FrmPrecierreResumen(result);
            //frm.ShowDialog();
            //this.Close();
        }

    }

    public static void EvaluarRequisitosFondo(int idFondoOpe, string codUsuario, bool procesarAtribucionRenta, bool mantener_VCuotaSit_Si_VCuotaOpe_Es_Diferente)
    {
        string msgValidacion = "Problemas al evaluar los REQUISITOS para el PRECIERRE: ";
        String nombreFondo = ""; // Aquí podriamos indicar el nombre del FONDO sin embargo no sería muy reelevante dado que cerramos uno por uno los fondos

        FondoBO fondoBO = new FondoBO();
        DataSet dsInfoFondos = fondoBO.obtenerFondos();

        DataRow[] listaFondosSelec = dsInfoFondos.Tables["FONDO"].Select("ID_FONDO = '" + idFondoOpe.ToString() + "'");
        if (listaFondosSelec.Length == 0)
            throw new Exception(msgValidacion + "No se ha encontrado la información del Fondo en el Sistema de Operaciones");

        /*INICIO OT8844*/
        ConsultaInversionesBL consultaInversionesBL = new ConsultaInversionesBL();
        ValorCuotaBO valorCuotaBO = new ValorCuotaBO();

        DataTable dtPortafolios = consultaInversionesBL.ListarPortafolios();
        Hashtable ht = new Hashtable();
        //El key del HT será el código del portafolio y su value será el codigo SBS
        foreach (DataRow drPortafolio in dtPortafolios.Rows)
        {
            ht.Add(drPortafolio["Descripcion"].ToString(), drPortafolio["CodigoPortafolioSBS"].ToString());
        }
        /*FIN OT8844*/

        DataRow drFondo = listaFondosSelec[0];
        /*INICIO OT8844*/        
        string tipoAccesoFondo = drFondo["TIPO_ACCESO"].ToString();

        //OT10944 INI	
        if (tipoAccesoFondo.Equals(ConstantesING.TIPO_ACCESO_FONDO_INVERSION))
        {
            tipoAccesoFondo = ConstantesING.TIPO_ACCESO_FONDO_PUBLICO;
        }
        //OT10944 FIN

        DateTime fecha = Convert.ToDateTime(drFondo["FECHA_PRECIERRE"]);
        /*FIN OT8844*/
        if (drFondo["FECHA_PRECIERRE"] == DBNull.Value)
        {
            throw new Exception(msgValidacion + "El Fondo " + nombreFondo + " no tiene fecha a precerrar.");
        }

        //OT11264 PSC003 INI
        //validar si se ha precerrado el fondo
        PrecierreBO precierreBO = new PrecierreBO();
        DataTable dtValidarFondoPrecerradoXFondo = precierreBO.ValidarFondoPrecerradoXFondo(idFondoOpe, fecha);
        int precierreEjecutado = 0;
        if (dtValidarFondoPrecerradoXFondo.Rows.Count > 0)
        {
            DataRow drValidarFondoPrecerradoXFondo = dtValidarFondoPrecerradoXFondo.Rows[0];
            precierreEjecutado = Convert.ToInt32(drValidarFondoPrecerradoXFondo["FLAG_EXISTE_PRECIERRE"]);
        }

        if (precierreEjecutado > 0)
        //se ha ejecutado el precierre para la fecha y fondo
        {
            throw new Exception(msgValidacion + "El Fondo " + nombreFondo + " ya se ha precerrado a la fecha.");
        }
        //OT11264 PSC003 FIN

        /*INICIO OT 8844*/
        double valorCuotaPortafolioSerie = 0;
        object valorCuotaOperaciones = null;
        if (ConstantesING.TIPO_ACCESO_FONDO_PRIVADO.Equals(tipoAccesoFondo))
        {
            if (drFondo["VALOR_CUOTA"] == DBNull.Value)
            {
                throw new Exception(msgValidacion + "El Fondo " + nombreFondo + " no tiene VC a la fecha.");
            }
        }
        else
        {
            string portafolio = drFondo["PORTAFOLIO"].ToString();
            string serie = drFondo["SERIE"].ToString();
            string codigoSBS = ht[portafolio].ToString();
                
            DataTable dtValorCuotaOpe = valorCuotaBO.obtenerValorCuota(fecha.ToString("yyyy-MM-dd"), idFondoOpe);
            if (dtValorCuotaOpe.Rows.Count > 0)
            {
                drFondo["VALOR_CUOTA"] = Convert.ToDouble(dtValorCuotaOpe.Rows[0]["VALOR_CUOTA"]);
            }

            DataTable dtValorCuota = consultaInversionesBL.ObtenerValorCuotaInversiones(fecha, codigoSBS, serie);

            if (dtValorCuota == null || dtValorCuota.Rows.Count == 0)
            {
                //if (drFondo["VALOR_CUOTA"] != DBNull.Value)
                //{
                //    // DialogResult result = MessageBox.Show("Fondo " + fondo + " no tiene VC a la fecha en SIT.\r\n¿Desea mantener el VC registrado en el Sistema de Procesos?", "Procesos", MessageBoxButtons.YesNoCancel);
                //    // if (result == DialogResult.Yes)
                //    // throw new Exception(msgValidacion + "No se tiene el Valor Cuota, asegúrese de haberlo GUARDADO antes de iniciar este proceso");
                //} else
                 
                throw new Exception(msgValidacion + "No se encontró el Valor Cuota, asegúrese de haberlo GUARDADO antes de iniciar este proceso");                   
            }
                
            valorCuotaPortafolioSerie = Convert.ToDouble(dtValorCuota.Rows[0]["ValCuotaPreCierreVal"]);

            if (drFondo["VALOR_CUOTA"] != DBNull.Value)
            {
                valorCuotaOperaciones = drFondo["VALOR_CUOTA"];
                if (Convert.ToDouble(valorCuotaOperaciones) != valorCuotaPortafolioSerie)
                {
                    //DialogResult result = MessageBox.Show("Fondo " + fondo + " tiene VC diferente entre el SIT y Sistema de Procesos.\r\n¿Desea mantener el VC registrado en el Sistema de Procesos?", "Procesos", MessageBoxButtons.YesNo);

                    if (mantener_VCuotaSit_Si_VCuotaOpe_Es_Diferente)
                    {
                        valorCuotaOperaciones = valorCuotaPortafolioSerie;
                        if (valorCuotaOperaciones == null || valorCuotaOperaciones == DBNull.Value)
                            
                            valorCuotaBO.insertarValorCuota(fecha.ToString("yyyy-MM-dd"), valorCuotaPortafolioSerie, codUsuario, idFondoOpe, true);
                        else
                            valorCuotaBO.actualizarValorCuota(idFondoOpe.ToString(), valorCuotaPortafolioSerie, fecha.ToString("yyyy-MM-dd"), codUsuario, true);
                            
                        drFondo["FLAG_DOBLE_CHECKEO"] = "S";
                    }
                    else                        
                        valorCuotaPortafolioSerie = Convert.ToDouble(valorCuotaOperaciones);                        
                }
            }
            else
            {
                if (valorCuotaOperaciones == null || valorCuotaOperaciones == DBNull.Value)                    
                    valorCuotaBO.insertarValorCuota(fecha.ToString("yyyy-MM-dd"), valorCuotaPortafolioSerie, codUsuario, idFondoOpe, true);                    
                else                    
                    valorCuotaBO.actualizarValorCuota(idFondoOpe.ToString(), valorCuotaPortafolioSerie, fecha.ToString("yyyy-MM-dd"), codUsuario, true);
                    
                drFondo["FLAG_DOBLE_CHECKEO"] = "S";
            }

            //lvFondo.Items[e.Index].SubItems[4].Text = valorCuotaPortafolioSerie.ToString("####0.00000000");
        }
        /*FIN OT 8844*/
        if (drFondo["FLAG_DOBLE_CHECKEO"] != DBNull.Value && drFondo["FLAG_DOBLE_CHECKEO"].ToString().Equals("N"))
        {
            throw new Exception(msgValidacion + "El doble Chequeo del Valor Cuota no está aprobado.");
        }
        if (drFondo["FLAG_OPERACIONES_PENDIENTES"] != DBNull.Value && drFondo["FLAG_OPERACIONES_PENDIENTES"].ToString().Equals("S"))
        {
            throw new Exception(msgValidacion + "Existen operaciones pendientes de aprobación en el fondo " + nombreFondo + ".");
        }
        /*INICIO OT 8844*/
        //OT10928 INI

        bool verificarTributacion;

        FondoBO Fondobo = new FondoBO();
        verificarTributacion = Fondobo.verificarTributacion(idFondoOpe, fecha);

        //OT10928 FIN

        if (drFondo["FLAG_EJECUTO_TRIBUTACION"] != DBNull.Value && drFondo["FLAG_EJECUTO_TRIBUTACION"].ToString().Equals("N"))
        {
            if (ConstantesING.TIPO_ACCESO_FONDO_PRIVADO.Equals(tipoAccesoFondo))
            {
                throw new Exception(msgValidacion + "No se ejecuto proceso de tributación para el fondo " + nombreFondo + ".");
            }
            else
            {
                if (!verificarTributacion)//OT10928 INI
                {                        
                    if (!procesarAtribucionRenta)
                        throw new Exception(msgValidacion + "No se ejecuto proceso de tributación para el fondo " + nombreFondo + ".");

                    //PASAMOS A CALCULAR LA ATRIBUCION
                    AtribucionBL atribucionBL = new AtribucionBL(codUsuario);
                    try
                    {
                        DataTable dtTipoCambio = atribucionBL.ObtenerTipoCambioContable(fecha);
                        if (dtTipoCambio.Rows.Count == 0)
                        {
                            throw new Exception(msgValidacion + "No se ha registrado el tipo de cambio contable para la fecha " + fecha.ToString("dd/MM/yyyy"));
                        }
                        atribucionBL.RevertirTributacion(idFondoOpe, fecha, codUsuario, "D");
                        atribucionBL.GenerarAtribucionPrecierre(idFondoOpe, fecha, codUsuario, false, "", tipoAccesoFondo); //por ahora desFondo no importa mas que para privados
                        drFondo["FLAG_EJECUTO_TRIBUTACION"] = "S";
                        //lvFondo.Items[e.Index].SubItems[5].Text = "S";
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(msgValidacion + "No se pudo ejecutar la atribución de rentas del fondo.\r\nDetalle: " + ex.Message);
                    }                        
                }
            }
        }
        else
        {
            if (!ConstantesING.TIPO_ACCESO_FONDO_PRIVADO.Equals(tipoAccesoFondo))
            {
                if (!verificarTributacion)//OT10928 INI
                {
                    if (!procesarAtribucionRenta)
                        throw new Exception(msgValidacion + "No se ejecuto proceso de tributación para el fondo " + nombreFondo + ".");

                    //CALCULAR LA ATRIBUCION
                    AtribucionBL atribucionBL = new AtribucionBL(codUsuario);
                    try
                    {
                        DataTable dtTipoCambio = atribucionBL.ObtenerTipoCambioContable(fecha);
                        if (dtTipoCambio.Rows.Count == 0)
                            throw new Exception(msgValidacion + "No se ha registrado el tipo de cambio contable para la fecha " + fecha.ToString("dd/MM/yyyy"));

                        atribucionBL.RevertirTributacion(idFondoOpe, fecha, codUsuario, "D");
                        atribucionBL.GenerarAtribucionPrecierre(idFondoOpe, fecha, codUsuario, false, "", tipoAccesoFondo); //por ahora desFondo no importa mas que para privados
                        drFondo["FLAG_EJECUTO_TRIBUTACION"] = "S";
                        //lvFondo.Items[e.Index].SubItems[5].Text = "S";
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(msgValidacion + "No se pudo ejecutar la atribución de rentas del fondo.\r\nDetalle: " + ex.Message);
                    }
                }
            }
        }

        /*FIN OT 8844*/
        //OT 5070 INI
        if (drFondo["FLAG_ALERTA_PRECIERRE"] != DBNull.Value && drFondo["FLAG_ALERTA_PRECIERRE"].ToString().Equals("S"))
        {
            throw new Exception(msgValidacion + "No se ha revertido el proceso de tributación para el fondo " + nombreFondo + " a la fecha.");
        }
        //OT 5070 FIN        
    }

    #endregion

    #region "Metodos para elaboración de correos"

    private static string NotificarPrecierreEjecutado(string fondo, DateTime fecha, decimal valorCuota)
    {
        StringBuilder sb = new StringBuilder("Se notifica el precierre:<br>Fondo: ");
        sb.Append(fondo);
        sb.Append("<br>Fecha: ");
        sb.Append(fecha.ToString("dd/MM/yyyy"));
        sb.Append("<br>Valor Cuota: ");
        sb.Append(valorCuota.ToString("####0.00000000"));
        sb.Append("<br>Fecha y hora de ejecución: ");
        sb.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        sb.Append("<br><br>Atte.<br><br>");
        return sb.ToString();
    }

    private static String crearEmailSuscripciones(DataRow dr)
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        String saludo = null;
        String sexo = dr["sexo"].ToString();
        String estadoCivil = dr["CODIGO_ESTADO_CIVIL"].ToString();
        String nombreCompleto = dr["NOMBRE_PARTICIPE"].ToString();
        String codigoAgenciaOrigen = dr["CODIGO_AGENCIA_ORIGEN"].ToString();

        if (sexo != null && sexo == ConstantesING.PERSONA_SEXO_FEMENINO)
        {
            if (estadoCivil == ConstantesING.ESTADO_CIVIL_SOL)
            {
                saludo = "Estimada Srta. ";
            }
            else
            {
                saludo = "Estimada Sra. ";
            }
        }
        else
        {
            saludo = "Estimado Sr. ";
        }
        String disclosure = "La diversificación de cartera es importante para la toma de decisiones de inversión, mantener una concentración alta en determinada posición representa un mayor riesgo para su cartera. Los factores que intervienen en la evaluación de si su cartera general de inversión está lo suficientemente diversificada, pueden no ser tan evidentes si sólo toma en consideración su cuenta en Citibank del Peru S.A.<BR>" +
            "Por lo tanto es importante que revise con el debido cuidado su cartera total con Citibank** y las diferentes instituciones con las que mantiene relaciones comerciales, entre las cuales se encuentra Fondos SURA SAF SAC., todo esto en relación a su patrimonio total y se asegure que ésta se encuentra acorde con sus objetivos de inversión, tolerancia al riesgo y objetivos de diversificación.<BR>" +
            "Contacte a su Asesor Financiero para  revisar su portafolio y estrategias potenciales para reducir el riesgo y/o volatilidad de la cartera. <BR>";
        disclosure += "- Los productos de inversión no están asegurados por ninguna entidad Gubernamental ni por Citibank del Peru S.A.<BR>";
        disclosure += "- Las inversiones no son depósitos bancarios por lo que no poseen ningún seguro de depósito.<BR>";
        disclosure += "- Los productos de inversión están sujetos a riesgos de inversión incluyendo posibles pérdidas.<BR>";
        disclosure += "- Los resultados pasados no representan resultados futuros.<BR>";
        disclosure += "(**): Cualquier referencia a Citibank debe entenderse como efectuada a Citibank del Peru S.A.";

        String nota = "Nota: La fecha de proceso que aparece en esta confirmación no hace referencia necesariamente al día en que realizó el abono en el banco, hace referencia al día en el que su abono adquiere las cuotas de participación del fondo.";

        email =
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + nombreCompleto + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Lo saludamos y le informamos que la operación realizada con nosotros ha sido procesada con éxito. A continuación le indicamos el detalle de la misma: </SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P><BR>" +

            "			<table>" +
            "				<tr><td colspan='2'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">OPERACIÓN DE SUSCRIPCIÓN" + (codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK ? ": UNSOLICITED" : "") + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Nro. Operación:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["NUMERO_OPERACION"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondo: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["DESCRIPCION"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de proceso:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_PROCESO"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Monto:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cuotas:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + Convert.ToDecimal(dr["NUMERO_CUOTAS"]).ToString("############0.00000000") + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Valor cuota del día:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + Convert.ToDecimal(dr["VALOR_CUOTA"]).ToString("###########0.00000000") + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "			</table>" +

            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>" +


            "			<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + nota + "</SPAN></P><BR>"


            + ((codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK) ? "<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + disclosure + "</SPAN></P>" : "") +

            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";
        return email;
    }

    //OT11264 INI
    private static String crearAlertaEmailSuscripciones(DataRow dr)
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        String saludo = null;
        String nombreCompleto = dr["NOMBRE_PARTICIPE"].ToString();
        String cuc = dr["CUC"].ToString();
        String tipoCliente = dr["TIPO_PARTICIPE"].ToString();
        String riesgo = dr["NIVEL_RIESGO"].ToString();
        String tipoOperacion = dr["DESC_OPERACION"].ToString();
        String numeroDocumento = dr["NUMERO_DOCUMENTO"].ToString();
        String moneda = dr["DESC_MONEDA"].ToString();

        saludo = "Se informa que el cliente " + tipoCliente + "(" + nombreCompleto + ")" + " ha realizado la siguiente actividad:";

        String nota = "Nota: La fecha de proceso que aparece en esta confirmación no hace referencia necesariamente al día en que realizó el abono en el banco, hace referencia al día en el que su abono adquiere las cuotas de participación del fondo.";

        email =
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <table  align=center border=1 width=600 >" +
            "            <tr>" +
            "             <th>  FECHA  </th>" +
            "             <th>CUC</th>" +
            "             <th>DNI</th>" +
            "             <th>APELLIDOS NOMBRES</th>" +
            "             <th>TIPO DE CLIENTE</th>" +
            "             <th>N.RIESGO</th>" +
            "             <th>TIPO OPERACION</th>" +
            "             <th>MONEDA</th>" +
            "             <th>MONTO</th>" +
            "            </tr>" +
            "            <tr>" +
            "             <td>" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_PROCESO"]) + "</td>" +
            "             <td>" + cuc + "</td>" +
            "             <td>" + numeroDocumento + "</td>" +
            "             <td>" + nombreCompleto + "</td>" +
            "             <td>" + tipoCliente + " </td>" +
            "             <td>" + riesgo + "</td>" +
            "             <td>" + tipoOperacion + "</td>" +
            "             <td>" + moneda + "</td>" +
            "             <td>" + String.Format("{0:###,##0.00}", dr["MONTO"]) + "</td>" +
            "            </tr>" +
            "            </table>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>" +
            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";
        return email;

    }
    //OT11264 FIN

    private static String crearEmailRescates(DataRow dr)
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        /*************************************************************/

        String disclosure = "La diversificación de cartera es importante para la toma de decisiones de inversión, mantener una concentración alta en determinada posición representa un mayor riesgo para su cartera. Los factores que intervienen en la evaluación de si su cartera general de inversión está lo suficientemente diversificada, pueden no ser tan evidentes si sólo toma en consideración su cuenta en Citibank del Peru S.A.<BR>" +
            "Por lo tanto es importante que revise con el debido cuidado su cartera total con Citibank** y las diferentes instituciones con las que mantiene relaciones comerciales, entre las cuales se encuentra Fondos SURA SAF SAC., todo esto en relación a su patrimonio total y se asegure que ésta se encuentra acorde con sus objetivos de inversión, tolerancia al riesgo y objetivos de diversificación.<BR>" +
            "Contacte a su Asesor Financiero para  revisar su portafolio y estrategias potenciales para reducir el riesgo y/o volatilidad de la cartera. <BR>";
        disclosure += "- Los productos de inversión no están asegurados por ninguna entidad Gubernamental ni por Citibank del Peru S.A.<BR>";
        disclosure += "- Las inversiones no son depósitos bancarios por lo que no poseen ningún seguro de depósito.<BR>";
        disclosure += "- Los productos de inversión están sujetos a riesgos de inversión incluyendo posibles pérdidas.<BR>";
        disclosure += "- Los resultados pasados no representan resultados futuros.<BR>";
        disclosure += "(**): Cualquier referencia a Citibank debe entenderse como efectuada a Citibank del Peru S.A.";


        String saludo = null;
        String sexo = dr["sexo"].ToString();
        String estadoCivil = dr["CODIGO_ESTADO_CIVIL"].ToString();
        String nombreCompleto = dr["NOMBRE_PARTICIPE"].ToString();
        String codigoAgenciaOrigen = dr["CODIGO_AGENCIA_ORIGEN"].ToString();

        if (sexo != null && sexo == ConstantesING.PERSONA_SEXO_FEMENINO)
        {
            if (estadoCivil == ConstantesING.ESTADO_CIVIL_SOL)
            {
                saludo = "Estimada Srta. ";
            }
            else
            {
                saludo = "Estimada Sra. ";
            }
        }
        else
        {
            saludo = "Estimado Sr. ";
        }
        email =

            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + nombreCompleto + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Lo saludamos y le informamos que la operación realizada con nosotros ha sido procesada con éxito. A continuación le indicamos el detalle de la misma: </SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P><BR>" +

            "			<table>" +
            "				<tr><td colspan='2'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">OPERACIÓN DE RESCATE" + (codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK ? ": UNSOLICITED" : "") + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Nro. Operación:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["NUMERO_OPERACION"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "			</table>" +


            "			<table>" +
            "				<tr><td colspan='2'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">DETALLE DE LA OPERACIÓN DE RESCATE</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondo: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["NOMBRE_COMERCIAL_FONDO"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Tipo de rescate: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + (dr["FLAG_RESCATE_TOTAL"].ToString() == "N" ? "Rescate parcial" : "Rescate total") + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de solicitud: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + ((dr["FLAG_RESCATE_TOTAL"] != null) ? String.Format("{0:dd-MM-yyyy}", dr["FECHA_SOLICITUD_RESCATE"]) : " NO ESPECIFICADA") + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de venta de cuotas: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_PROCESO"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de pago: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + ((dr["FECHA_PAGO_RESCATE"] != null) ? String.Format("{0:dd-MM-yyyy}", dr["FECHA_PAGO_RESCATE"]) : " NO ESPECIOFICADA") + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "			</table>" +

            "			<table>" +
            "				<tr><td colspan='2'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">DETALLE PAGO</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Vía pago: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["VIA_PAGO"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Forma pago: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["FORMA_PAGO"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Importe: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO"]) + "</SPAN></td></tr>" +

            "			    <tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Comisión por rescate anticipado: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            ((dr["COMISION_RESCATE_ANTICIPADO"] != null && Decimal.Parse(dr["COMISION_RESCATE_ANTICIPADO"].ToString()) > 0) ? dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["COMISION_ESPECIAL"]) : (dr["MONEDA"].ToString() + " 0.00"))
            + "</SPAN></td></tr>" +

            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">IGV de la comisión por rescate anticipado: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + ((Decimal.Parse(dr["IMPUESTO_TOTAL"].ToString()) > 0) ? (dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["IMPUESTO_TOTAL"])) : (dr["MONEDA"].ToString() + " 0.00")) + "</SPAN></td></tr>"
            + (dr["GENERA_RETENCIONES"].ToString() == ConstantesING.SI ? ("<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Retención de impuestos ley(*): </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + ((dr["MONTO_RETENCION"] != null && Decimal.Parse(dr["MONTO_RETENCION"].ToString()) > 0) ? (dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_RETENCION"])) : (dr["MONEDA"].ToString() + " 0.00")) + "</SPAN></td></tr>") : "") +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Importe neto a recibir: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_NETO"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Nro. cuotas vendidas: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + Convert.ToDecimal(dr["NRO_CUOTAS_VENDIDAS"]).ToString("##########0.00000000") + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Valor cuota: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + Convert.ToDecimal(dr["VALOR_CUOTA"]).ToString("###########0.00000000") + "</SPAN></td></tr>" +
            "			</table>" +

            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>"
            + (dr["GENERA_RETENCIONES"].ToString() == ConstantesING.SI ? ("<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">(*) D.S. 179-2004-EF.</SPAN></P>") : "") +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>"
            + ((codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK) ? "<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + disclosure + "</SPAN></P>" : "") +


            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";
        return email;
    }

    //OT11264 INI
    private static String crearAlertaEmailRescates(DataRow dr)
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        String saludo = null;
        String sexo = dr["sexo"].ToString();
        String estadoCivil = dr["CODIGO_ESTADO_CIVIL"].ToString();
        String nombreCompleto = dr["NOMBRE_PARTICIPE"].ToString();
        String codigoAgenciaOrigen = dr["CODIGO_AGENCIA_ORIGEN"].ToString();
        String cuc = dr["CUC"].ToString();
        String tipoCliente = dr["TIPO_PARTICIPE"].ToString();
        String riesgo = dr["NIVEL_RIESGO"].ToString();
        String tipoOperacion = dr["DESC_OPERACION"].ToString();
        String numeroDocumento = dr["NUMERO_DOCUMENTO"].ToString();
        String moneda = dr["DESC_MONEDA"].ToString();

        saludo = "Se informa que el cliente " + tipoCliente + "(" + nombreCompleto + ")" + " ha realizado la siguiente actividad:";

        String nota = "Nota: La fecha de proceso que aparece en esta confirmación no hace referencia necesariamente al día en que realizó el abono en el banco, hace referencia al día en el que su abono adquiere las cuotas de participación del fondo.";

        email =
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <table  align=center border=1 width=600 >" +
            "            <tr>" +
            "             <th>  FECHA  </th>" +
            "             <th>CUC</th>" +
            "             <th>DNI</th>" +
            "             <th>APELLIDOS NOMBRES</th>" +
            "             <th>TIPO DE CLIENTE</th>" +
            "             <th>N.RIESGO</th>" +
            "             <th>TIPO OPERACION</th>" +
            "             <th>MONEDA</th>" +
            "             <th>MONTO</th>" +
            "            </tr>" +
            "            <tr>" +
            "             <td>" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_PROCESO"]) + "</td>" +
            "             <td>" + cuc + "</td>" +
            "             <td>" + numeroDocumento + "</td>" +
            "             <td>" + nombreCompleto + "</td>" +
            "             <td>" + tipoCliente + " </td>" +
            "             <td>" + riesgo + "</td>" +
            "             <td>" + tipoOperacion + "</td>" +
            "             <td>" + moneda + "</td>" +
            "             <td>" + String.Format("{0:###,##0.00}", dr["MONTO"]) + "</td>" +
            "            </tr>" +
            "            </table>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>" +

            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";
        return email;

    }
    //OT11264 FIN

    private static String crearEmailRetenciones(DataRow dr)
    {
        String email = "";

        String body = null;
        String prom = "Estimado cliente ";
        String saludo = "Este correo es para informarle que se le han retenido cuotas por concepto de la \"Retención Anual de Impuestos de Ley por Saldos \", según D.S. 179-2004-EF.";

        String seccion1 = "\n\n\nRetención Anual\n\n";
        String fondo = "Fondo                        :" + dr["NOMBRE_COMERCIAL_FONDO"].ToString() + "\n\n";
        seccion1 += fondo;

        String seccion2 = "Fecha de Venta de Cuotas     :" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_PROCESO"]) + "\n\n";

        String retencion2 = "";

        if (Decimal.Parse(dr["MONTO"].ToString()) > 0)
            retencion2 = "Retención de Impuestos ley   :" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO"]) + "\n\n";
        else
            retencion2 = "Retención de Impuestos ley   :" + "\n\n";



        String seccion3 = "Nro.Cuotas Retenidas         :" + Convert.ToDecimal(dr["NUMERO_CUOTAS"]).ToString("###########0.00000000") + "\n\n";

        seccion3 += "Valor Cuota                  :" + Convert.ToDecimal(dr["VALOR_CUOTA"]).ToString("###########0.00000000") + "\n\n";

        String despedida = "\nAtentamente. \n\nFondos SURA SAF SAC.";
        String nombrecompleto = (dr["RAZON_SOCIAL"] != null) ? dr["RAZON_SOCIAL"].ToString() : "";

        body = prom + " " + nombrecompleto + ":\n\n";
        email = body + saludo + seccion1 + seccion2 + retencion2 + seccion3 + despedida;

        return email;
    }

    //OT 7968 INI

    /// <summary>
    /// Crea el Email en caso la suscripción este relacionada a un traspaso entre Fondos
    /// </summary>
    /// <param name="dr"></param>
    /// <returns></returns>
    private static String crearEmailSuscripcionesPorTraspaso(DataRow dr)
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        String saludo = null;
        String sexo = dr["SEXO_PARTICIPE_ORIGEN"].ToString();
        String estadoCivil = dr["CODIGO_ESTADO_CIVIL_PARTICIPE_ORIGEN"].ToString();
        String nombreCompleto = dr["NOMBRE_PARTICIPE_ORIGEN"].ToString();
        String codigoAgenciaOrigen = dr["CODIGO_AGENCIA_ORIGEN_PARTICIPE_ORIGEN"].ToString();

        #region Obtener saludo de acuerdo al sexo del participe

        if (sexo != null && sexo == ConstantesING.PERSONA_SEXO_FEMENINO)
        {
            if (estadoCivil == ConstantesING.ESTADO_CIVIL_SOL)
            {
                saludo = "Estimada Srta. ";
            }
            else
            {
                saludo = "Estimada Sra. ";
            }
        }
        else
        {
            saludo = "Estimado Sr. ";
        }

        #endregion

        #region Footer en caso la agencia sea CITIBANK

        String disclosure = "La diversificación de cartera es importante para la toma de decisiones de inversión, mantener una concentración alta en determinada posición representa un mayor riesgo para su cartera. Los factores que intervienen en la evaluación de si su cartera general de inversión está lo suficientemente diversificada, pueden no ser tan evidentes si sólo toma en consideración su cuenta en Citibank del Peru S.A.<BR>" +
            "Por lo tanto es importante que revise con el debido cuidado su cartera total con Citibank** y las diferentes instituciones con las que mantiene relaciones comerciales, entre las cuales se encuentra Fondos SURA SAF SAC., todo esto en relación a su patrimonio total y se asegure que ésta se encuentra acorde con sus objetivos de inversión, tolerancia al riesgo y objetivos de diversificación.<BR>" +
            "Contacte a su Asesor Financiero para  revisar su portafolio y estrategias potenciales para reducir el riesgo y/o volatilidad de la cartera. <BR>";
        disclosure += "- Los productos de inversión no están asegurados por ninguna entidad Gubernamental ni por Citibank del Peru S.A.<BR>";
        disclosure += "- Las inversiones no son depósitos bancarios por lo que no poseen ningún seguro de depósito.<BR>";
        disclosure += "- Los productos de inversión están sujetos a riesgos de inversión incluyendo posibles pérdidas.<BR>";
        disclosure += "- Los resultados pasados no representan resultados futuros.<BR>";
        disclosure += "(**): Cualquier referencia a Citibank debe entenderse como efectuada a Citibank del Peru S.A.";

        #endregion

        String nota = "Nota: La fecha de proceso que aparece en esta confirmación hace referencia al día en que su abono adquiere las cuotas de participación en el fondo de destino.";

        email =
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + nombreCompleto + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Lo saludamos y le informamos que la solicitud de traspaso realizada con nosotros ha sido procesada con éxito. A continuación le indicamos el detalle de la misma: </SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P><BR>" +

            "			<table>" +

            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Solicitud Nro. :</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["TRASPASO_FONDO_ID"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondo Origen: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["FONDO_ORIGEN"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondo Destino: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["FONDO_DESTINO"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de proceso:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_PROCESO"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Monto:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Tc:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + String.Format("{0:###,##0.00}", dr["TIPO_CAMBIO"]) + "</SPAN></td></tr>" +

            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cuotas:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + Convert.ToDecimal(dr["NUMERO_CUOTAS"]).ToString("############0.00000000") + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Valor cuota del día:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + Convert.ToDecimal(dr["VALOR_CUOTA"]).ToString("###########0.00000000") + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "			</table>" +

            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>" +


            "			<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + nota + "</SPAN></P><BR>"


            + ((codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK) ? "<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + disclosure + "</SPAN></P>" : "") +

            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";

        return email;
    }

    /// <summary>
    /// Crea correo para las operaciones de traspaso entre fondos que requieren de un Tipo de Cambio
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static String crearEmailOperacionesPendienteTipoCambio(DataTable dt)
    {
        StringBuilder sbEmail = new StringBuilder();

        sbEmail.Append(
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid; BORDER-BOTTOM: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Las siguientes operaciones requieren el registro de TC para la acreditación al Sistemas de Operaciones: </SPAN></P>" +
            "			 <table style='border-collapse: collapse;'>" +

            //CABECERA TABLA DE TRASPASOS
            "               <tr>" +
            "				    <td style='BORDER: rgb(187,189,191) 1px solid;'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Nro de Solicitud</SPAN></td>" +
            "				    <td style='BORDER: rgb(187,189,191) 1px solid;'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">CUC</SPAN></td>" +
            "				    <td style='BORDER: rgb(187,189,191) 1px solid;'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondo Origen</SPAN></td>" +
            "				    <td style='BORDER: rgb(187,189,191) 1px solid;'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondo Destino</SPAN></td>" +
            "				    <td style='BORDER: rgb(187,189,191) 1px solid;'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de Abono</SPAN></td>" +
            "               </tr>"
        );

        foreach (DataRow dr in dt.Rows)
        {
            sbEmail.Append(
                "           <tr>" +
                "				<td style='BORDER: rgb(187,189,191) 1px solid;'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["TRASPASO_FONDO_ID"].ToString() + "</SPAN></td>" +
                "				<td style='BORDER: rgb(187,189,191) 1px solid;'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["CODIGO"].ToString() + "</SPAN></td>" +
                "				<td style='BORDER: rgb(187,189,191) 1px solid;'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["FONDO_ORIGEN"].ToString() + "</SPAN></td>" +
                "				<td style='BORDER: rgb(187,189,191) 1px solid;'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["FONDO_DESTINO"].ToString() + "</SPAN></td>" +
                "				<td style='BORDER: rgb(187,189,191) 1px solid;'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_ABONO_CONTABLE"]) + "</SPAN></td>" +
                "           </tr>"
            );
        }

        sbEmail.Append(
            "               <tr><td colspan='5' height='17'></td></tr>" +
            "			</table>" +
            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "</HTML>"
        );

        return sbEmail.ToString();
    }

    private static String crearEmailConversionCuotasDestino(DataRow dr)
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        String saludo = null;
        String sexo = dr["SEXO"].ToString();
        String estadoCivil = dr["CODIGO_ESTADO_CIVIL"].ToString();
        String nombreCompleto = dr["NOMBRE_PARTICIPE"].ToString();
        String codigoAgenciaOrigen = dr["CODIGO_AGENCIA_ORIGEN"].ToString();

        if (sexo != null && sexo == ConstantesING.PERSONA_SEXO_FEMENINO)
        {
            if (estadoCivil == ConstantesING.ESTADO_CIVIL_SOL)
            {
                saludo = "Estimada Srta. ";
            }
            else
            {
                saludo = "Estimada Sra. ";
            }
        }
        else
        {
            saludo = "Estimado Sr. ";
        }
        String disclosure = "La diversificación de cartera es importante para la toma de decisiones de inversión, mantener una concentración alta en determinada posición representa un mayor riesgo para su cartera. Los factores que intervienen en la evaluación de si su cartera general de inversión está lo suficientemente diversificada, pueden no ser tan evidentes si sólo toma en consideración su cuenta en Citibank del Peru S.A.<BR>" +
            "Por lo tanto es importante que revise con el debido cuidado su cartera total con Citibank** y las diferentes instituciones con las que mantiene relaciones comerciales, entre las cuales se encuentra Fondos SURA SAF SAC., todo esto en relación a su patrimonio total y se asegure que ésta se encuentra acorde con sus objetivos de inversión, tolerancia al riesgo y objetivos de diversificación.<BR>" +
            "Contacte a su Asesor Financiero para  revisar su portafolio y estrategias potenciales para reducir el riesgo y/o volatilidad de la cartera. <BR>";
        disclosure += "- Los productos de inversión no están asegurados por ninguna entidad Gubernamental ni por Citibank del Peru S.A.<BR>";
        disclosure += "- Las inversiones no son depósitos bancarios por lo que no poseen ningún seguro de depósito.<BR>";
        disclosure += "- Los productos de inversión están sujetos a riesgos de inversión incluyendo posibles pérdidas.<BR>";
        disclosure += "- Los resultados pasados no representan resultados futuros.<BR>";
        disclosure += "(**): Cualquier referencia a Citibank debe entenderse como efectuada a Citibank del Peru S.A.";

        String nota = "";//"Nota: La fecha de proceso que aparece en esta confirmación no hace referencia necesariamente al día en que realizó el abono en el banco, hace referencia al día en el que su abono adquiere las cuotas de participación del fondo.";

        email =
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + nombreCompleto + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Lo saludamos y le informamos que la conversión de cuotas realizada con nosotros ha sido procesada con éxito. A continuación le indicamos el detalle de la misma: </SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P><BR>" +

            "			<table>" +
            //"				<tr><td colspan='2'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">OPERACIÓN DE CONVERSIÓN DE CUOTAS" + (codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK ? ": UNSOLICITED" : "") + "</SPAN></td></tr>" +
            //"				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Nro. Operación:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["NUMERO_OPERACION"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "				<tr><td colspan='2'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">DETALLE DE LA CONVERSIÓN" + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Serie Origen: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["DESCRIPCION_FONDO_ORIGEN"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Serie Destino: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["DESCRIPCION"].ToString() + "</SPAN></td></tr>" +
            //"				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">CUC Origen: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["CODIGO"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Tipo de conversión: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["TIPO_CONVERSION"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de solicitud:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_SOLICITUD_RESCATE"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de conversión de Cuotas:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_CONVERSION_CUOTAS"]) + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            //"				<tr><td colspan='2'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">DETALLE DEL ABONO" + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Importe:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Nro. de cuotas final serie destino:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + Convert.ToDecimal(dr["NUMERO_CUOTAS"]).ToString("############0.00000000") + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Valor cuota serie destino:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + Convert.ToDecimal(dr["VALOR_CUOTA"]).ToString("###########0.00000000") + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "			</table>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>" +


            "			<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + nota + "</SPAN></P><BR>"


            + ((codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK) ? "<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + disclosure + "</SPAN></P>" : "") +

            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";
        return email;
    }

    private static String crearEmailParticipesDestinoTraspasoAsociadoConversionCuotas(DataRow dr)
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        String saludo = null;
        String sexo = dr["SEXO"].ToString();
        String estadoCivil = dr["CODIGO_ESTADO_CIVIL"].ToString();
        String nombreCompleto = dr["NOMBRE_PARTICIPE"].ToString();
        String codigoAgenciaOrigen = dr["CODIGO_AGENCIA_ORIGEN"].ToString();

        if (sexo != null && sexo == ConstantesING.PERSONA_SEXO_FEMENINO)
        {
            if (estadoCivil == ConstantesING.ESTADO_CIVIL_SOL)
            {
                saludo = "Estimada Srta. ";
            }
            else
            {
                saludo = "Estimada Sra. ";
            }
        }
        else
        {
            saludo = "Estimado Sr. ";
        }
        String disclosure = "La diversificación de cartera es importante para la toma de decisiones de inversión, mantener una concentración alta en determinada posición representa un mayor riesgo para su cartera. Los factores que intervienen en la evaluación de si su cartera general de inversión está lo suficientemente diversificada, pueden no ser tan evidentes si sólo toma en consideración su cuenta en Citibank del Peru S.A.<BR>" +
            "Por lo tanto es importante que revise con el debido cuidado su cartera total con Citibank** y las diferentes instituciones con las que mantiene relaciones comerciales, entre las cuales se encuentra Fondos SURA SAF SAC., todo esto en relación a su patrimonio total y se asegure que ésta se encuentra acorde con sus objetivos de inversión, tolerancia al riesgo y objetivos de diversificación.<BR>" +
            "Contacte a su Asesor Financiero para  revisar su portafolio y estrategias potenciales para reducir el riesgo y/o volatilidad de la cartera. <BR>";
        disclosure += "- Los productos de inversión no están asegurados por ninguna entidad Gubernamental ni por Citibank del Peru S.A.<BR>";
        disclosure += "- Las inversiones no son depósitos bancarios por lo que no poseen ningún seguro de depósito.<BR>";
        disclosure += "- Los productos de inversión están sujetos a riesgos de inversión incluyendo posibles pérdidas.<BR>";
        disclosure += "- Los resultados pasados no representan resultados futuros.<BR>";
        disclosure += "(**): Cualquier referencia a Citibank debe entenderse como efectuada a Citibank del Peru S.A.";

        String nota = "Nota: La fecha de proceso que aparece en esta confirmación no hace referencia necesariamente al día en que realizó el abono en el banco, hace referencia al día en el que su abono adquiere las cuotas de participación del fondo.";

        email =
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + nombreCompleto + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            //                "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Lo saludamos y le informamos que la operación realizada con nosotros ha sido procesada con éxito. A continuación le indicamos el detalle de la misma: </SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Lo saludamos y le informamos que se le ha transferido exitosamente el importe de " + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO"]) +
            " a un valor cuota de " + Convert.ToDecimal(dr["VALOR_CUOTA"]).ToString("###########0.00000000") + " por lo cual tiene un total de " + Convert.ToDecimal(dr["NUMERO_CUOTAS"]).ToString("############0.00000000") + " cuotas en el fondo " + dr["DESCRIPCION"].ToString() + "</SPAN></P>" +
            "            <BR><BR>" +

            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>" +
            "			<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + nota + "</SPAN></P><BR>"


            + ((codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK) ? "<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + disclosure + "</SPAN></P>" : "") +

            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";
        return email;
    }

    //OT11264 INI
    private static String crearAlertaEmailTPS(DataRow dr)
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        String saludo = null;
        String sexo = dr["sexo"].ToString();
        String estadoCivil = dr["CODIGO_ESTADO_CIVIL"].ToString();
        String nombreCompleto = dr["NOMBRE_PARTICIPE"].ToString();
        String codigoAgenciaOrigen = dr["CODIGO_AGENCIA_ORIGEN"].ToString();
        String cuc = dr["CUC"].ToString();
        String tipoCliente = dr["TIPO_PARTICIPE"].ToString();
        String riesgo = dr["NIVEL_RIESGO"].ToString();
        String tipoOperacion = dr["DESC_OPERACION"].ToString();
        String numeroDocumento = dr["NUMERO_DOCUMENTO"].ToString();
        String moneda = dr["DESC_MONEDA"].ToString();

        saludo = "Se informa que el cliente " + tipoCliente + "(" + nombreCompleto + ")" + " ha realizado la siguiente actividad:";

        String nota = "Nota: La fecha de proceso que aparece en esta confirmación no hace referencia necesariamente al día en que realizó el abono en el banco, hace referencia al día en el que su abono adquiere las cuotas de participación del fondo.";

        email =
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <table  align=center border=1 width=600 >" +
            "            <tr>" +
            "             <th>  FECHA  </th>" +
            "             <th>CUC</th>" +
            "             <th>DNI</th>" +
            "             <th>APELLIDOS NOMBRES</th>" +
            "             <th>TIPO DE CLIENTE</th>" +
            "             <th>N.RIESGO</th>" +
            "             <th>TIPO OPERACION</th>" +
            "             <th>MONEDA</th>" +
            "             <th>MONTO</th>" +
            "            </tr>" +
            "            <tr>" +
            "             <td>" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_CONVERSION_CUOTAS"]) + "</td>" +
            "             <td>" + cuc + "</td>" +
            "             <td>" + numeroDocumento + "</td>" +
            "             <td>" + nombreCompleto + "</td>" +
            "             <td>" + tipoCliente + " </td>" +
            "             <td>" + riesgo + "</td>" +
            "             <td>" + tipoOperacion + "</td>" +
            "             <td>" + moneda + "</td>" +
            "             <td>" + String.Format("{0:###,##0.00}", dr["MONTO"]) + "</td>" +
            "            </tr>" +
            "            </table>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>" +
            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";
        return email;
    }
    //OT11264 FIN

    //OT11247 INI
    private static String crearEmailPagoFlujoNoFIR(DataRow dr)
    //OT11247 FIN
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        String saludo = null;
        String sexo = dr["SEXO"].ToString();
        String estadoCivil = dr["CODIGO_ESTADO_CIVIL"].ToString();
        String nombreCompleto = dr["NOMBRE_PARTICIPE"].ToString();
        String codigoAgenciaOrigen = dr["CODIGO_AGENCIA_ORIGEN"].ToString();

        if (sexo != null && sexo == ConstantesING.PERSONA_SEXO_FEMENINO)
        {
            if (estadoCivil == ConstantesING.ESTADO_CIVIL_SOL)
            {
                saludo = "Estimada Srta. ";
            }
            else
            {
                saludo = "Estimada Sra. ";
            }
        }
        else
        {
            saludo = "Estimado Sr. ";
        }
        String disclosure = "La diversificación de cartera es importante para la toma de decisiones de inversión, mantener una concentración alta en determinada posición representa un mayor riesgo para su cartera. Los factores que intervienen en la evaluación de si su cartera general de inversión está lo suficientemente diversificada, pueden no ser tan evidentes si sólo toma en consideración su cuenta en Citibank del Peru S.A.<BR>" +
            "Por lo tanto es importante que revise con el debido cuidado su cartera total con Citibank** y las diferentes instituciones con las que mantiene relaciones comerciales, entre las cuales se encuentra Fondos SURA SAF SAC., todo esto en relación a su patrimonio total y se asegure que ésta se encuentra acorde con sus objetivos de inversión, tolerancia al riesgo y objetivos de diversificación.<BR>" +
            "Contacte a su Asesor Financiero para  revisar su portafolio y estrategias potenciales para reducir el riesgo y/o volatilidad de la cartera. <BR>";
        disclosure += "- Los productos de inversión no están asegurados por ninguna entidad Gubernamental ni por Citibank del Peru S.A.<BR>";
        disclosure += "- Las inversiones no son depósitos bancarios por lo que no poseen ningún seguro de depósito.<BR>";
        disclosure += "- Los productos de inversión están sujetos a riesgos de inversión incluyendo posibles pérdidas.<BR>";
        disclosure += "- Los resultados pasados no representan resultados futuros.<BR>";
        disclosure += "(**): Cualquier referencia a Citibank debe entenderse como efectuada a Citibank del Peru S.A.";

        String nota = "";//"Nota: La fecha de proceso que aparece en esta confirmación no hace referencia necesariamente al día en que realizó el abono en el banco, hace referencia al día en el que su abono adquiere las cuotas de participación del fondo.";

        email =
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + nombreCompleto + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Lo saludamos y le informamos que su pago programado realizado ha sido procesado con éxito. A continuación le indicamos el detalle de la misma: </SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P><BR>" +
            "			<table>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "				<tr><td colspan='2'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">DETALLE DEL PAGO" + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondo: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["DESCRIPCION_FONDO"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Tipo: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + "Pago programado" + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de Proceso: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_PROCESO"]) + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Monto Bruto: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Retención: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_RETENCION"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">ITF: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_ITF"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Monto Neto a Pagar: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_NETO"]) + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "			</table>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>" +


            "			<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + nota + "</SPAN></P><BR>"


            + ((codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK) ? "<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + disclosure + "</SPAN></P>" : "") +

            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";
        return email;
    }

    //OT11247 INI
    private static String crearEmailPagoFlujo(DataRow dr)
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        String saludo = null;
        String sexo = dr["SEXO"].ToString();
        String estadoCivil = dr["CODIGO_ESTADO_CIVIL"].ToString();
        String nombreCompleto = dr["NOMBRE_PARTICIPE"].ToString();
        String codigoAgenciaOrigen = dr["CODIGO_AGENCIA_ORIGEN"].ToString();

        if (sexo != null && sexo == ConstantesING.PERSONA_SEXO_FEMENINO)
        {
            if (estadoCivil == ConstantesING.ESTADO_CIVIL_SOL)
            {
                saludo = "Estimada Srta. ";
            }
            else
            {
                saludo = "Estimada Sra. ";
            }
        }
        else
        {
            saludo = "Estimado Sr. ";
        }
        String disclosure = "La diversificación de cartera es importante para la toma de decisiones de inversión, mantener una concentración alta en determinada posición representa un mayor riesgo para su cartera. Los factores que intervienen en la evaluación de si su cartera general de inversión está lo suficientemente diversificada, pueden no ser tan evidentes si sólo toma en consideración su cuenta en Citibank del Peru S.A.<BR>" +
            "Por lo tanto es importante que revise con el debido cuidado su cartera total con Citibank** y las diferentes instituciones con las que mantiene relaciones comerciales, entre las cuales se encuentra Fondos SURA SAF SAC., todo esto en relación a su patrimonio total y se asegure que ésta se encuentra acorde con sus objetivos de inversión, tolerancia al riesgo y objetivos de diversificación.<BR>" +
            "Contacte a su Asesor Financiero para  revisar su portafolio y estrategias potenciales para reducir el riesgo y/o volatilidad de la cartera. <BR>";
        disclosure += "- Los productos de inversión no están asegurados por ninguna entidad Gubernamental ni por Citibank del Peru S.A.<BR>";
        disclosure += "- Las inversiones no son depósitos bancarios por lo que no poseen ningún seguro de depósito.<BR>";
        disclosure += "- Los productos de inversión están sujetos a riesgos de inversión incluyendo posibles pérdidas.<BR>";
        disclosure += "- Los resultados pasados no representan resultados futuros.<BR>";
        disclosure += "(**): Cualquier referencia a Citibank debe entenderse como efectuada a Citibank del Peru S.A.";

        String nota = "";

        email =
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + nombreCompleto + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Lo saludamos y le informamos que el pago correspondiente a la Distribución de Resultados del Fondo de Inversión en Renta de Bienes Inmuebles SURA Asset Management está siendo procesado. </SPAN></P><BR><BR>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">A continuación, le indicamos el detalle del mismo: </SPAN></P><BR><BR>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P><BR>" +
            "			<table>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "				<tr><td colspan='2'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">DETALLE DEL PAGO" + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondo: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["DESCRIPCION_FONDO"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Tipo: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + "Distribución de resultados" + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de Proceso: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_PROCESO"]) + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Monto Bruto: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Impuesto a la Renta: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_RETENCION"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">ITF: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_ITF"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Monto Neto a Pagar: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_NETO"]) + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "			</table>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>" +


            "			<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + nota + "</SPAN></P><BR>"


            + ((codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK) ? "<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + disclosure + "</SPAN></P>" : "") +

            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";
        return email;
    }
    //OT11247 FIN

    //OT11436 INI
    private static String crearEmailPagoFlujoFondoPrivadoCapitalInteres(DataRow dr)
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        String saludo = null;
        String sexo = dr["SEXO"].ToString();
        String estadoCivil = dr["CODIGO_ESTADO_CIVIL"].ToString();
        String nombreCompleto = dr["NOMBRE_PARTICIPE"].ToString();
        String codigoAgenciaOrigen = dr["CODIGO_AGENCIA_ORIGEN"].ToString();

        if (sexo != null && sexo == ConstantesING.PERSONA_SEXO_FEMENINO)
        {
            if (estadoCivil == ConstantesING.ESTADO_CIVIL_SOL)
            {
                saludo = "Estimada Srta. ";
            }
            else
            {
                saludo = "Estimada Sra. ";
            }
        }
        else
        {
            saludo = "Estimado Sr. ";
        }
        String disclosure = "La diversificación de cartera es importante para la toma de decisiones de inversión, mantener una concentración alta en determinada posición representa un mayor riesgo para su cartera. Los factores que intervienen en la evaluación de si su cartera general de inversión está lo suficientemente diversificada, pueden no ser tan evidentes si sólo toma en consideración su cuenta en Citibank del Peru S.A.<BR>" +
            "Por lo tanto es importante que revise con el debido cuidado su cartera total con Citibank** y las diferentes instituciones con las que mantiene relaciones comerciales, entre las cuales se encuentra Fondos SURA SAF SAC., todo esto en relación a su patrimonio total y se asegure que ésta se encuentra acorde con sus objetivos de inversión, tolerancia al riesgo y objetivos de diversificación.<BR>" +
            "Contacte a su Asesor Financiero para  revisar su portafolio y estrategias potenciales para reducir el riesgo y/o volatilidad de la cartera. <BR>";
        disclosure += "- Los productos de inversión no están asegurados por ninguna entidad Gubernamental ni por Citibank del Peru S.A.<BR>";
        disclosure += "- Las inversiones no son depósitos bancarios por lo que no poseen ningún seguro de depósito.<BR>";
        disclosure += "- Los productos de inversión están sujetos a riesgos de inversión incluyendo posibles pérdidas.<BR>";
        disclosure += "- Los resultados pasados no representan resultados futuros.<BR>";
        disclosure += "(**): Cualquier referencia a Citibank debe entenderse como efectuada a Citibank del Peru S.A.";

        String nota = "";

        email =
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + nombreCompleto + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Lo saludamos y le informamos que su pago programado realizado ha sido procesado con éxito. A continuación le indicamos el detalle de la misma: </SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P><BR>" +
            "			<table>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "				<tr><td colspan='2'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">DETALLE DEL PAGO" + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondo: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["DESCRIPCION_FONDO"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Tipo: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + "Pago programado" + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de Proceso: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_PROCESO"]) + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Monto Interés: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_INTERES"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Monto Capital: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_CAPITAL"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Retención: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_RETENCION"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">ITF: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO_ITF"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Total a distribuir: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["TOTAL_DISTRIBUIR"]) + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "			</table>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>" +


            "			<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + nota + "</SPAN></P><BR>"


            + ((codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK) ? "<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + disclosure + "</SPAN></P>" : "") +

            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";
        return email;
    }
    //OT11436 FIN

    //OT10839 PSC001 INI
    private static String crearEmailLiberacionCuotas(DataRow dr)
    {
        String rutaAplicacion = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UrlImagenes"]);

        String email = "";
        String rutaLinea = rutaAplicacion + "/images/linea.png";
        String rutaLogo = rutaAplicacion + "/images/logo2.png";
        String rutaCierre = rutaAplicacion + "/images/cierre.jpg";

        String saludo = null;
        String sexo = dr["sexo"].ToString();
        String estadoCivil = dr["CODIGO_ESTADO_CIVIL"].ToString();
        String nombreCompleto = dr["NOMBRE_PARTICIPE"].ToString();
        String codigoAgenciaOrigen = dr["CODIGO_AGENCIA_ORIGEN"].ToString();

        if (sexo != null && sexo == ConstantesING.PERSONA_SEXO_FEMENINO)
        {
            if (estadoCivil == ConstantesING.ESTADO_CIVIL_SOL)
            {
                saludo = "Estimada Srta. ";
            }
            else
            {
                saludo = "Estimada Sra. ";
            }
        }
        else
        {
            saludo = "Estimado Sr. ";
        }
        String disclosure = "La diversificación de cartera es importante para la toma de decisiones de inversión, mantener una concentración alta en determinada posición representa un mayor riesgo para su cartera. Los factores que intervienen en la evaluación de si su cartera general de inversión está lo suficientemente diversificada, pueden no ser tan evidentes si sólo toma en consideración su cuenta en Citibank del Peru S.A.<BR>" +
            "Por lo tanto es importante que revise con el debido cuidado su cartera total con Citibank** y las diferentes instituciones con las que mantiene relaciones comerciales, entre las cuales se encuentra Fondos SURA SAF SAC., todo esto en relación a su patrimonio total y se asegure que ésta se encuentra acorde con sus objetivos de inversión, tolerancia al riesgo y objetivos de diversificación.<BR>" +
            "Contacte a su Asesor Financiero para  revisar su portafolio y estrategias potenciales para reducir el riesgo y/o volatilidad de la cartera. <BR>";
        disclosure += "- Los productos de inversión no están asegurados por ninguna entidad Gubernamental ni por Citibank del Peru S.A.<BR>";
        disclosure += "- Las inversiones no son depósitos bancarios por lo que no poseen ningún seguro de depósito.<BR>";
        disclosure += "- Los productos de inversión están sujetos a riesgos de inversión incluyendo posibles pérdidas.<BR>";
        disclosure += "- Los resultados pasados no representan resultados futuros.<BR>";
        disclosure += "(**): Cualquier referencia a Citibank debe entenderse como efectuada a Citibank del Peru S.A.";

        String nota = "";

        email =
            "<HTML><HEAD>" +
            "<META content='text/html; charset=iso-8859-1' http-equiv=Content-Type>" +
            "<META name=GENERATOR content='MSHTML 8.00.6001.18904'></HEAD>" +
            "<BODY>" +
            "<DIV >" +
            "<TABLE cellSpacing=0 cellPadding=0 width=800 align=center style='FONT-FAMILY: \"Trebuchet MS'; COLOR: rgb(0,0,102)\">" +
            "  <TBODY>" +
            "  <TR>" +
            "    <TD style='BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-TOP: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid'>" +
            "      <TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "        <TBODY>" +
            "        <TR bgColor=#0039a6>" +
            "          <TD bgColor=#ffffff height=74></TD>" +
            "          <TD bgColor=#ffffff height=74 width=617><FONT style='FONT-SIZE: 16px' color=#00aecb face='Trebuchet MS'><BR>&nbsp;</FONT></TD>" +
            "          <TD bgColor=#ffffff width=122><IMG border=0 hspace=0 alt='' src='" + rutaLogo + "'></TD>" +
            "          <TD bgColor=#ffffff height=74></TD></TR>" +
            "        <TR>" +
            "        <TD colSpan=4 align=middle><IMG border=0 hspace=0 alt='' src='" + rutaLinea + "'></TD></TR>" +
            "        <TR style='PADDING-TOP: 18px'>" +
            "          <TD width=30></TD>" +
            "          <TD colSpan=2><FONT color=#000099><FONT face='Trebuchet MS'><BR></FONT></FONT>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + saludo + "</SPAN><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + nombreCompleto + "</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Lo saludamos y le informamos que la operación realizada con nosotros ha sido procesada con éxito. A continuación le indicamos el detalle de la misma: </SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P><BR>" +

            "			<table>" +
            "				<tr><td colspan='2'><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">OPERACIÓN LIBERACIÓN DE CUOTAS" + (codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK ? ": UNSOLICITED" : "") + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Nro. Operación:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["NUMERO_OPERACION"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondo: </SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["DESCRIPCION"].ToString() + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fecha de proceso:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + String.Format("{0:dd-MM-yyyy}", dr["FECHA_PROCESO"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Monto:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + dr["MONEDA"].ToString() + " " + String.Format("{0:###,##0.00}", dr["MONTO"]) + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cuotas:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + Convert.ToDecimal(dr["NUMERO_CUOTAS"]).ToString("############0.00000000") + "</SPAN></td></tr>" +
            "				<tr><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Valor cuota del día:</SPAN></td><td><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">" + Convert.ToDecimal(dr["VALOR_CUOTA"]).ToString("###########0.00000000") + "</SPAN></td></tr>" +
            "				<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
            "			</table>" +

            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Cordialmente</SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><B><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\">Fondos SURA SAF</SPAN></B></P>" +
            "            <P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,0,102)\"></SPAN></P>" +
            "            <P></P><SPAN style='FONT-FAMILY: 'Trebuchet MS'; COLOR: rgb(0,57,166)'><BR></SPAN>" +


            "			<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + nota + "</SPAN></P><BR>"


            + ((codigoAgenciaOrigen == ConstantesING.CODIGO_AGENCIA_CITIBANK) ? "<P style=\"MARGIN-BOTTOM: 0pt\"><SPAN style=\"FONT-FAMILY: 'Trebuchet MS',sans-serif; COLOR: rgb(0,0,102);font-size: 12px\">" + disclosure + "</SPAN></P>" : "") +

            "</TR></TBODY></TABLE></TD></TR></TBODY></TABLE>" +
            "	<TABLE border=0 cellSpacing=0 cellPadding=0 width=800 align=center>" +
            "	<TBODY>" +
            "  	<TR>" +
            "    <TD style='BORDER-BOTTOM: rgb(187,189,191) 1px solid; BORDER-LEFT: rgb(187,189,191) 1px solid; BORDER-RIGHT: rgb(187,189,191) 1px solid' width=800 colSpan=4>" +
            "    <A href='http://www.fondossura.com.pe' target='_blank'><IMG border=0 hspace=0 alt='' src='" + rutaCierre + "'></A></TD>" +
            "    </TR></TBODY></TABLE></DIV></BODY>" +
            "</HTML>";
        return email;
    }
    //OT10839 PSC001 FIN

    #endregion

}
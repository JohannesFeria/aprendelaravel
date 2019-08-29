/*  -------------------------------------------------------------
 *  Fecha de modificación: 21/01/2015
 *  Modificado por: Juan Castro
 *  Nro. de Orden de Trabajo: 6986 
 *  Descripción del cambio: Creación
 *  -------------------------------------------------------------
 *  Fecha de modificación: 13/08/2015
 *  Modificado por: Alex Vega
 *  Nro. de Orden de Trabajo: 7349
 *  Descripción del cambio: Se ajusta actualización de comprobante automático
 *  -------------------------------------------------------------
 * */
/*
 * Fecha de Modificación: 26/08/2015
 * Modificado por: Juan Castro
 * Numero de OT: 7584
 * Descripción del cambio: Se modifica la generación de los comprobantes de los asientos contables para prepararlos para el envío electrónico.
 * */
/*
 * Fecha de Modificación: 24/11/2015
 * Modificado por: Robert Castillo
 * Numero de OT: 7999
 * Descripción del cambio: Se modifica el método GenerarAsientos para incluir el cálculo del monto de detracción.
 * */
using System;
using System.Collections.Generic;
using System.Text;
using Procesos.DA;
using INGFondos.Support.Interop;
using System.Globalization;
using System.Threading;
using System.Data;
using System.Collections;
using Procesos.TD;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Net;
using Procesos.Constants;
using System.Configuration;
using INGFondos.Constants;

namespace Procesos.BL
{
    public class AsientosBL
    {
        private AsientosDA da = new AsientosDA();
        //OT7999 INI
        private double dblPorcentajeDetraccion = 0;
        double tipoCambio = 0;
        //OT7999 FIN

        /// <summary>
        /// Obtiene los registros de asientos tipo PD de retenciones generadas.
        /// </summary>
        /// <param name="dtAsientos">Tabla en la que se volcará la información de los asientos a generar.</param>
        /// <param name="fechaInicio">Fecha de inicio de la consulta.</param>
        /// <param name="fechaFin">Fecha de fin de la consulta.</param>
        public void ObtenerAsientosPDRetenciones(Procesos.TD.Asiento.LISTA_ASIENTODataTable dtAsientos, DateTime fechaInicio, DateTime fechaFin)
        {
            da.ObtenerAsientosPDRetenciones(dtAsientos, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Obtiene los registros de asientos tipo VT de ingresos a partir de un archivo Excel.
        /// </summary>
        /// <param name="dtAsientos">Tabla en la que se volcará la información de los asientos a generar.</param>
        /// <param name="archivo">Ruta y nombre del archivo Excel que contiene la información del reporte de ingresos para obtener los asientos.</param>
        public void ObtenerAsientosVTIngresos(Procesos.TD.Asiento.LISTA_ASIENTODataTable dtAsientos, string archivo)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-PE");
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {

                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                ExcelApplication app = new ExcelApplication();
                ExcelWorkBook wb = app.OpenWorkBook(archivo, ExcelMode.Full);
                ExcelWorkSheet sheet = wb.GetSheet(1);

                String fechaAsiento = sheet.GetString("G3"); //convertir a fecha
                DataTable dt = sheet.LeerTabla("B", "I", 5, sheet.ObtenerUltimaFilaTabla("B", 6), true);
                /*String factura = sheet.GetString("G2");
                string[] partesFactura = ObtenerPartesFactura(factura);
                string numeros = partesFactura[0];
                string letras = partesFactura[1];
                */
                ParametrosDA parametrosDA = new ParametrosDA();
                DataTable dtParametroRUC = parametrosDA.ObtenerValoresParametros(ConstantesING.TABLA_TIPO_DOCUMENTO, ConstantesING.TIPO_DOCUMENTO_RUC);
                String tipoDocumentoSunat = dtParametroRUC.Rows[0]["CAMPO1"].ToString();

                //int correlativoFactura = Convert.ToInt32(numeros);

                wb.Close();
                app.Close();
                Hashtable ht = new Hashtable();
                foreach (DataRow dr in dt.Rows)
                {
                    string fondo = dr["Fondo"].ToString().Trim();
                    if (!ht.ContainsKey(fondo))
                    {
                        Asiento.LISTA_ASIENTORow row = dtAsientos.NewLISTA_ASIENTORow();

                        //string numeroFactura = letras + correlativoFactura.ToString().PadLeft(numeros.Length, '0');
                        //correlativoFactura++;
                        object total = dt.Compute("SUM(Ingresos)", "Fondo = '" + fondo + "'");
                        //object totalImpuesto = dt.Compute("SUM(IGV)", "Fondo = '" + fondo + "'");
                        object totalPN = dt.Compute("SUM(Ingresos)", "Fondo = '" + fondo + "' and TipoPersona = 'PN'");
                        //object impuestoPN = dt.Compute("SUM(IGV)", "Fondo = '" + fondo + "' and TipoPersona = 'PN'");
                        object totalPJ = dt.Compute("SUM(Ingresos)", "Fondo = '" + fondo + "' and TipoPersona = 'PJ'");
                        //object impuestoPJ = dt.Compute("SUM(IGV)", "Fondo = '" + fondo + "' and TipoPersona = 'PJ'");
                        //object subtotal = Convert.ToDecimal(total) - Convert.ToDecimal(totalImpuesto);
                        object igv = dt.Compute("SUM(IGV)", "Fondo = '" + fondo + "'");
                        row.TIPO_ASIENTO = "VT_ING";
                        row.UUID = "";
                        row.ESTADO = "";
                        row.FONDO = fondo;
                        row.FECHA_PROCESO = DateTime.ParseExact(fechaAsiento, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                        row.ID_OPERACION = "";
                        row.MONEDA = dr["Moneda"].ToString() == "SOL" ? "PEN" : "USD";
                        row.TOTAL = total == null || total == DBNull.Value ? 0 : Decimal.Round(Convert.ToDecimal(total), 2);
                        row.MONTO1 = total == null || total == DBNull.Value ? 0 : Decimal.Round(Convert.ToDecimal(total), 2);
                        
                        
                        //row.MONTO2 = totalImpuesto == null || totalImpuesto == DBNull.Value ? 0 : Convert.ToDecimal(totalImpuesto);
                        //row.MONTO3 = totalPN == null || totalPN == DBNull.Value ? 0 : Convert.ToDecimal(totalPN);
                        //row.MONTO4 = totalPJ == null || totalPJ == DBNull.Value ? 0 : Convert.ToDecimal(totalPJ);
                        row.MONTO2 = totalPN == null || totalPN == DBNull.Value ? 0 : Decimal.Round(Convert.ToDecimal(totalPN),2);
                        row.MONTO3 = totalPJ == null || totalPJ == DBNull.Value ? 0 : row.MONTO1-row.MONTO2;
                        row.MONTO4 = 0;
                        row.MONTO5 = 0;
                        //ABRIR EN CASO SE JUNTEN LOS MONTO 3 Y MONTO4
                        row.REFERENCIA = "";// numeroFactura;
                        row.REFERENCIA1 = tipoDocumentoSunat;
                        //row.MONTO3 = subtotal == null || subtotal == DBNull.Value ? 0 : Convert.ToDecimal(subtotal);
                        
                        row.TEXTO1 = row.FECHA_PROCESO.ToString("MM");
                        row.TEXTO2 = row.FECHA_PROCESO.ToString("yyyy");

                        DataTable dtFondo = parametrosDA.ObtenerValoresParametros(ConstantesING.FONDO, fondo);
                        row.TIPO_DOCUMENTO = ConstantesING.TIPO_DOCUMENTO_RUC;
                        row.CODIGO_CLIENTE = dtFondo.Rows[0]["CAMPO2"].ToString();
                        row.RAZON_SOCIAL = dtFondo.Rows[0]["CAMPO3"].ToString();

                        row.DIRECCION = ConfigurationSettings.AppSettings[ConstantesING.DIRECCION_FONDOS];
                        row.DISTRITO = ConfigurationSettings.AppSettings[ConstantesING.DISTRITO_FONDOS];
                        row.PROVINCIA = ConfigurationSettings.AppSettings[ConstantesING.PROVINCIA_FONDOS];
                        row.DEPARTAMENTO = ConfigurationSettings.AppSettings[ConstantesING.DEPARTAMENTO_FONDOS];
                        row.CODIGO_PAIS = ConstantesING.CODIGO_PERU;
                        row.CORREO = ConfigurationSettings.AppSettings[ConstantesING.CORREO_FONDOS];
                        row.ID_PAIS = ConfigurationSettings.AppSettings[ConstantesING.ID_PAIS_FONDOS];
                        row.ID_DEPARTAMENTO = ConfigurationSettings.AppSettings[ConstantesING.ID_DEPARTAMENTO_FONDOS];
                        row.ID_CIUDAD = ConfigurationSettings.AppSettings[ConstantesING.ID_CIUDAD_FONDOS];
                        row.ID_DISTRITO = ConfigurationSettings.AppSettings[ConstantesING.ID_DISTRITO_FONDOS];
                        row.UBIGEO = ConfigurationSettings.AppSettings[ConstantesING.UBIGEO_FONDOS];

                        row.IGVF = Decimal.ToDouble(Decimal.Round(Convert.ToDecimal(igv), 2));
                        row.TOTALF = Decimal.ToDouble(row.TOTAL);
                        row.SUBF = row.TOTALF - row.IGVF;

                        dtAsientos.AddLISTA_ASIENTORow(row);
                        ht.Add(fondo, fondo);
                    }
                }

                da.Server = INGFondos.Constants.Conexiones.ServidorSAP;
                da.Database = INGFondos.Constants.Conexiones.BaseDeDatosSAP;
                da.ListarControlAsiento(dtAsientos, "VT_ING", DateTime.ParseExact(fechaAsiento, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date);
                da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;
                da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }

        }

        private string[] ObtenerPartesFactura(string factura)
        {
            string[] partesFactura = new string[2];
            string numeros = string.Empty;
            string letras = string.Empty;

            for (int i = factura.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(factura[i]))
                {
                    numeros = factura.Substring(i + 1);
                    letras = factura.Substring(0, i + 1);
                    partesFactura[0] = numeros;
                    partesFactura[1] = letras;
                    return partesFactura;
                }
            }

            numeros = factura;
            partesFactura[0] = numeros;
            partesFactura[1] = letras;
            return partesFactura;
        }

        /// <summary>
        /// Obtiene los registros de asientos tipo VT y PD de comisiones anticipadas.
        /// </summary>
        /// <param name="dtAsientos">Tabla en la que se volcará la información de los asientos a generar.</param>
        /// <param name="fechaInicio">Fecha de inicio de la consulta.</param>
        /// <param name="fechaFin">Fecha de fin de la consulta.</param>
        public void ObtenerAsientosVTComisiones(Asiento.LISTA_ASIENTODataTable dtAsientos, DateTime fechaInicio, DateTime fechaFin, bool sapActivo)
        {
            //string[] partesBoleta = ObtenerPartesFactura(boleta);
            da.ObtenerAsientosVTComisiones(dtAsientos, fechaInicio, fechaFin, sapActivo);
        }

        /// <summary>
        /// Método que se encarga de generar los asientos contables a partir de la selección del usuario.
        /// </summary>
        /// <param name="lvAsientos">Objeto ListViewItem que contienen los asientos que posiblemente desea el usuario que se generem</param>
        /// <param name="usuario">Usuario responsable de la creación de los asientos.</param>
        public void GenerarAsientos(System.Windows.Forms.ListView lvAsientos, string usuario, bool sapActivo)
        {
            ParametrosDA parametroDA = new ParametrosDA();
            string estadoEjecutado = parametroDA.ObtenerValoresParametros("ESTEJE", "ESTEJE").Rows[0]["CAMPO1"].ToString();

            FacturacionDA facturacionDA = new FacturacionDA();
            facturacionDA.Server = Conexiones.ServidorPremBanco13;
            facturacionDA.Database = Conexiones.BaseDeDatosPremBanco13;
            facturacionDA.CrearConexion();
            facturacionDA.AbrirConexion();
            facturacionDA.AbrirTransaccion();

            da.Server = INGFondos.Constants.Conexiones.ServidorSAP;
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosSAP;
            SqlConnection cn = da.GetConnection2();
            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();
            Asiento asiento = new Asiento();
            string host = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(host);
            string direccionIP = ip.AddressList[0].ToString();

            string glosaVT_ING = string.Empty;
            string glosaVT_CRA = string.Empty;
            string descripcionVT_ING = string.Empty;
            string descripcionVT_CRA = string.Empty;

            Hashtable ht = new Hashtable();
            Comprobante comprobanteAsiento = null;

            try
            {
                

                DataTable dtParametroGlosas = parametroDA.ObtenerValoresParametros(ConstantesING.TABLA_COMPROBANTES, ConstantesING.GLOSA_ASIENTO_INGRESOS);
                glosaVT_ING = dtParametroGlosas.Rows[0]["CAMPO3"].ToString();

                dtParametroGlosas = parametroDA.ObtenerValoresParametros(ConstantesING.TABLA_COMPROBANTES, ConstantesING.GLOSA_ASIENTO_COMISIONES);
                glosaVT_CRA = dtParametroGlosas.Rows[0]["CAMPO3"].ToString();

                dtParametroGlosas = parametroDA.ObtenerValoresParametros(ConstantesING.TABLA_COMPROBANTES, ConstantesING.DESCRIPCION_ASIENTO_INGRESOS);
                descripcionVT_ING = dtParametroGlosas.Rows[0]["CAMPO3"].ToString();

                dtParametroGlosas = parametroDA.ObtenerValoresParametros(ConstantesING.TABLA_COMPROBANTES, ConstantesING.DESCRIPCION_ASIENTO_COMISIONES);
                descripcionVT_CRA = dtParametroGlosas.Rows[0]["CAMPO3"].ToString();

                DataTable dtCabecerasTipo = null;
                DataTable dtDetallesTipo = null;

                dtCabecerasTipo = da.ObtenerAsientoTipoCabecera(cn, trans);
                dtDetallesTipo = da.ObtenerAsientoTipoDetalle(cn, trans);

                DataRow drCabeceraTipo = null;
                DataRow[] drDetallesTipo = null;
                DataRow drDetalleTipo = null;

                int codigoAsiento = 0;

                StringBuilder filtro = null;
                //OT7999 INI
                //setear dblPorcentajeDetraccion, si es posible usar un metodo propio
                bool valorDetraccionHallado = ObtenerDetraccion();
                double montoSoles = 0;
                //OT7999 FIN

                foreach (ListViewItem item in lvAsientos.CheckedItems)
                {

                    Asiento.LISTA_ASIENTORow datosAsiento = (Asiento.LISTA_ASIENTORow)item.Tag;
                    decimal cantidadSubDetalle = 0;
                    for (int j = 1; j <= 5; j++)
                    {
                        if (Convert.ToDecimal(datosAsiento["MONTO" + j.ToString()]) != 0)
                        {
                            cantidadSubDetalle++;
                        }
                    }

                    filtro = new StringBuilder();
                    filtro.Append("DESCRIPCION = '" + datosAsiento.TIPO_ASIENTO + "'");
                    filtro.Append(" AND MONEDA_DOCUMENTO = '" + datosAsiento.MONEDA + "'");
                    if (datosAsiento.FONDO != "")
                    {
                        filtro.Append(" AND FONDO = '" + datosAsiento.FONDO + "'");
                    }

                    drCabeceraTipo = dtCabecerasTipo.Select(filtro.ToString(), "CODIGO_ASIENTO")[0];
                    codigoAsiento = Convert.ToInt32(drCabeceraTipo["CODIGO_ASIENTO"]);
                    drDetallesTipo = dtDetallesTipo.Select("CODIGO_ASIENTO = " + codigoAsiento.ToString());
                    
                    string generaAsiento = null;
                    string referencia = null;
                    generaAsiento = datosAsiento.IsID_OPERACIONNull() || datosAsiento.ID_OPERACION == "" ? (datosAsiento.IsFONDONull() || datosAsiento.FONDO == "" ? drCabeceraTipo["DESCRIPCION"].ToString().Trim() + datosAsiento.FECHA_PROCESO.ToString("yyyyMMdd") + datosAsiento.MONEDA : datosAsiento.FECHA_PROCESO.ToString("yyyyMMdd") + datosAsiento.FONDO) : drCabeceraTipo["DESCRIPCION"].ToString().Trim() + datosAsiento.ID_OPERACION;
                    referencia = generaAsiento; //datosAsiento.IsID_OPERACIONNull() || datosAsiento.ID_OPERACION == "" ? datosAsiento.FECHA_PROCESO.ToString("yyyyMMdd") + "|" + generaAsiento : generaAsiento;
                    if (!datosAsiento.IsUUIDNull() && datosAsiento.UUID.Trim() != "" && datosAsiento.ESTADO != estadoEjecutado)
                    {
                        da.EliminarAsientoAntiguo(datosAsiento.UUID, cn, trans);
                    }

                    int idComprobante = 0;
                    string referenciaComprobante = string.Empty;

                    //LOGICA DE GENERACION DE FACTURA
                    if (datosAsiento.TIPO_ASIENTO == ConstantesING.TIPO_ASIENTO_INGRESO || datosAsiento.TIPO_ASIENTO == ConstantesING.TIPO_ASIENTO_COMISION)
                    {
                        comprobanteAsiento = new Comprobante();
                        comprobanteAsiento.Cliente = new Cliente();
                        asiento.CLIENTE.ImportRow(datosAsiento);
                        comprobanteAsiento.Cliente.TipoDocumento = asiento.CLIENTE[0].TIPO_DOCUMENTO;
                        comprobanteAsiento.Cliente.CodigoCliente = asiento.CLIENTE[0].CODIGO_CLIENTE;
                        comprobanteAsiento.Cliente.RazonSocial = asiento.CLIENTE[0].RAZON_SOCIAL.Replace(",", "");
                        comprobanteAsiento.Cliente.Direccion = asiento.CLIENTE[0].DIRECCION;
                        comprobanteAsiento.Cliente.Distrito = asiento.CLIENTE[0].DISTRITO;
                        comprobanteAsiento.Cliente.Ciudad = asiento.CLIENTE[0].PROVINCIA;
                        comprobanteAsiento.Cliente.Departamento = asiento.CLIENTE[0].DEPARTAMENTO;
                        comprobanteAsiento.Cliente.IdPais = Convert.ToInt32(asiento.CLIENTE[0].ID_PAIS);
                        comprobanteAsiento.Cliente.Pais = asiento.CLIENTE[0].CODIGO_PAIS;
                        if (comprobanteAsiento.Cliente.IdPais.ToString().Equals(ConstantesING.CODIGO_PAIS_PERU))
                        {
                            comprobanteAsiento.Cliente.IdDepartamento = Convert.ToInt32(asiento.CLIENTE[0].ID_DEPARTAMENTO);
                            comprobanteAsiento.Cliente.IdCiudad = Convert.ToInt32(asiento.CLIENTE[0].ID_CIUDAD);
                            comprobanteAsiento.Cliente.IdDistrito = Convert.ToInt32(asiento.CLIENTE[0].ID_DISTRITO);
                            comprobanteAsiento.Cliente.Departamento = asiento.CLIENTE[0].DEPARTAMENTO;
                            comprobanteAsiento.Cliente.Ciudad = asiento.CLIENTE[0].PROVINCIA;
                            comprobanteAsiento.Cliente.Distrito = asiento.CLIENTE[0].DISTRITO;
                            comprobanteAsiento.Cliente.Ubigeo = asiento.CLIENTE[0].UBIGEO;
                            comprobanteAsiento.Cliente.CodigoPostal = "";
                        }
                        else
                        {
                            comprobanteAsiento.Cliente.IdDepartamento = 0;
                            comprobanteAsiento.Cliente.IdCiudad = 0;
                            comprobanteAsiento.Cliente.IdDistrito = 0;
                            comprobanteAsiento.Cliente.Departamento = "";
                            comprobanteAsiento.Cliente.Ciudad = "";
                            comprobanteAsiento.Cliente.Distrito = "";
                            comprobanteAsiento.Cliente.Ubigeo = "";
                            comprobanteAsiento.Cliente.CodigoPostal = asiento.CLIENTE[0].CODIGO_POSTAL;
                        }
                        comprobanteAsiento.Cliente.Correo = asiento.CLIENTE[0].CORREO;
                        comprobanteAsiento.Cliente.Usuario = usuario;

                        facturacionDA.RegistrarCliente(comprobanteAsiento.Cliente);
                        asiento.CLIENTE.Clear();
                        #region CODIGO COMENTADO
                        //DataTable dtInfoCliente = facturacionDA.ObtenerCliente(datosAsiento.CODIGO_CLIENTE);
                        //if (dtInfoCliente.Rows.Count == 0)
                        //{
                        //    asiento.CLIENTE.ImportRow(datosAsiento);
                        //    facturacionDA.InsertarCliente(asiento.CLIENTE[0].CODIGO_CLIENTE,
                        //        asiento.CLIENTE[0].RAZON_SOCIAL, asiento.CLIENTE[0].DIRECCION,
                        //        asiento.CLIENTE[0].DISTRITO, asiento.CLIENTE[0].PROVINCIA,
                        //        asiento.CLIENTE[0].DEPARTAMENTO, usuario);
                        //    asiento.CLIENTE.Clear();
                        //}
                        //else
                        //{
                        //    //asiento.CLIENTE.ImportRow(dtInfoCliente.Rows[0]);
                        //    //deberia actualizarse
                        //}
                        #endregion
                        //UNA VEZ OBTENIDO EL CLIENTE, PREPARAR LA DATA DEL COMPROBANTE Y DE SU DETALLE
                        Asiento.COMPROBANTERow comprobante = asiento.COMPROBANTE.NewCOMPROBANTERow();
                        comprobante.CODIGO_CLIENTE = datosAsiento.CODIGO_CLIENTE;
                        string descripcionComprobante = string.Empty;

                        switch (datosAsiento.TIPO_ASIENTO)
                        {
                            case ConstantesING.TIPO_ASIENTO_INGRESO:
                                comprobante.TIPO_DOCUMENTO = ConstantesING.TIPO_DOCUMENTO_FACTURA;
                                comprobante.COD_CONCEPTO = datosAsiento.MONEDA == "PEN" ? ConstantesING.CONCEPTO_INGRESO_SOLES : ConstantesING.CONCEPTO_INGRESO_DOLARES;
                                comprobante.GLOSA = glosaVT_ING;
                                descripcionComprobante = descripcionVT_ING;

                                comprobanteAsiento.TipoDocumento = ConstantesING.TIPO_DOCUMENTO_FACTURA;
                                comprobanteAsiento.CodigoConcepto = datosAsiento.MONEDA == "PEN" ? ConstantesING.CONCEPTO_INGRESO_SOLES : ConstantesING.CONCEPTO_INGRESO_DOLARES;
                                comprobanteAsiento.Glosa = glosaVT_ING;
                                descripcionComprobante = descripcionVT_ING;

                                break;
                            case ConstantesING.TIPO_ASIENTO_COMISION:
                                comprobante.TIPO_DOCUMENTO = ConstantesING.TIPO_DOCUMENTO_BOLETA;
                                comprobante.COD_CONCEPTO = datosAsiento.MONEDA == "PEN" ? ConstantesING.CONCEPTO_COMISION_SOLES : ConstantesING.CONCEPTO_COMISION_DOLARES;
                                comprobante.GLOSA = glosaVT_CRA;
                                descripcionComprobante = descripcionVT_CRA;

                                comprobanteAsiento.TipoDocumento = ConstantesING.TIPO_DOCUMENTO_BOLETA;
                                comprobanteAsiento.CodigoConcepto = datosAsiento.MONEDA == "PEN" ? ConstantesING.CONCEPTO_COMISION_SOLES : ConstantesING.CONCEPTO_COMISION_DOLARES;
                                comprobanteAsiento.Glosa = glosaVT_CRA;
                                descripcionComprobante = descripcionVT_CRA;

                                break;
                        }
                        if (!datosAsiento.IsUUIDNull() && datosAsiento.UUID.Equals(""))
                        {
                        //OT 7349 INI
                        }
                        else
                        {

                            DataTable dtComprobante2 = facturacionDA.ObtenerComprobantePorUUID(datosAsiento.UUID);
                            
                            asiento.COMPROBANTE.ImportRow(dtComprobante2.Rows[0]);

                            Comprobante comprobanteAntiguo = new Comprobante();
                            comprobanteAntiguo.IdComprobante = Convert.ToInt32(asiento.COMPROBANTE.Rows[0]["ID_COMPROBANTE"].ToString().Trim());
                            comprobanteAntiguo.Usuario = usuario;

                            facturacionDA.AnularComprobante(comprobanteAntiguo);
                            asiento.COMPROBANTE.Rows.Remove(asiento.COMPROBANTE[0]);
                            #region CODIGO_COMENTADO
                            //facturacionDA.EliminarComprobante(asiento.COMPROBANTE.Rows[0]["ID_COMPROBANTE"].ToString().Trim(), ConstantesING.ESTADO_INACTIVO, usuario);

                            //facturacionDA.ActualizarComprobante(ConstantesING.ACCION_ACTUALIZAR, asiento.COMPROBANTE[0].ID_COMPROBANTE,
                            //    asiento.COMPROBANTE[0].TIPO_DOCUMENTO, asiento.COMPROBANTE[0].MONEDA, asiento.COMPROBANTE[0].FECHA_EMISION,
                            //    asiento.COMPROBANTE[0].CODIGO_CLIENTE, "", "", asiento.COMPROBANTE[0].COD_CONCEPTO,
                            //    "", "", Convert.ToDateTime("01/01/1900"), ConstantesING.ESTADO_ACTIVO, ConstantesING.NO, ConstantesING.NO, ConstantesING.SI,
                            //    asiento.COMPROBANTE[0].GLOSA, 1, asiento.COMPROBANTE[0].PRECIO_UNITARIO,
                            //    asiento.COMPROBANTE[0].VALOR_VENTA, asiento.COMPROBANTE[0].IGV, asiento.COMPROBANTE[0].TOTAL, usuario, "");
                            //facturacionDA.ActualizarDetalle(asiento.COMPROBANTE[0].ID_COMPROBANTE, 1, string.Format(descripcionComprobante, datosAsiento.FONDO, datosAsiento.TEXTO1, datosAsiento.TEXTO2),
                            //    null, ConstantesING.SI, datosAsiento.SUBF, usuario);
                            //referenciaComprobante = dtComprobante.Rows[0]["SERIE"].ToString() + "-" + dtComprobante.Rows[0]["NUMERO"].ToString();
                            //if (!datosAsiento.IsID_OPERACIONNull() && datosAsiento.TIPO_ASIENTO == "VT_CRA")
                            //{
                            //    ht.Add(datosAsiento.ID_OPERACION, referenciaComprobante);
                            //}
                            #endregion

                        }
                        //OT 7349 FIN

                        //OT7999 INI
                        //comprobanteAsiento.Total = datosAsiento.TOTALF;
                        //datosAsiento.TOTALF -- de tipo double

                        if (comprobanteAsiento.TipoDocumento.Equals(ConstantesING.TIPO_DOCUMENTO_FACTURA))
                        {
                            //se calcula la detraccion sólo si el documento es Factura
                            bool tipoCambioHallado = setearTipoCambio(datosAsiento.FECHA_PROCESO);
                            montoSoles = 0;

                            if (datosAsiento.MONEDA != "PEN")
                            {
                                //dolares, convertir a soles
                                //bool tipoCambioHallado = setearTipoCambio();
                                if (!tipoCambioHallado)
                                {
                                    MessageBox.Show("No existe el valor del tipo de cambio para la fecha.", "Comprobantes");
                                    //salir del proceso...
                                }
                                else
                                {
                                    montoSoles = datosAsiento.TOTALF * tipoCambio;
                                }
                            }
                            else
                            {
                                montoSoles = datosAsiento.TOTALF;
                            }

                            if (montoSoles >= 700)
                            {
                                //calcular detraccion
                                //txtMontoDetrac.Text = (double.Parse(txtTotal.Text) * (dblPorcentajeDetraccion / 100)).ToString("0.00");
                                if (!valorDetraccionHallado)
                                {
                                    MessageBox.Show("No existe el valor del porcentaje de detracción.", "Comprobantes");
                                    //salir del proceso...
                                }
                                else
                                {
                                    comprobanteAsiento.PorcentajeDetraccion = dblPorcentajeDetraccion;
                                    //comprobanteAsiento.MontoDetraccion = montoSoles * (dblPorcentajeDetraccion / 100);
                                    comprobanteAsiento.MontoDetraccion = datosAsiento.TOTALF * (dblPorcentajeDetraccion / 100);
                                }
                            }
                        }
                        else
                        { 
                            //No es factura
                            comprobanteAsiento.PorcentajeDetraccion = 0;
                            comprobanteAsiento.MontoDetraccion = 0;
                        }
                        //OT7999 FIN

                        comprobanteAsiento.Moneda = datosAsiento.MONEDA == "PEN" ? "S" : "D";
                        comprobanteAsiento.FechaEmision = datosAsiento.FECHA_PROCESO;
                        comprobanteAsiento.Cliente.CodigoCliente = datosAsiento.CODIGO_CLIENTE;
                        comprobanteAsiento.Proceso = "";
                        comprobanteAsiento.Expediente = "";
                        comprobanteAsiento.TipoDocumentoRelacionado = "";
                        comprobanteAsiento.NumeroDocumentoRelacionado = "";
                        comprobanteAsiento.FechaDocumentoRelacionado = Convert.ToDateTime("01/01/1900");
                        comprobanteAsiento.Estado = ConstantesING.ESTADO_ACTIVO;
                        comprobanteAsiento.FlagImpresion = ConstantesING.NO;
                        comprobanteAsiento.FlagEnviado = ConstantesING.NO;
                        comprobanteAsiento.FlagAfectoIgv = ConstantesING.SI;
                        comprobanteAsiento.Glosa = comprobante.GLOSA;
                        comprobanteAsiento.Cantidad = 1;
                        comprobanteAsiento.ValorVenta = datosAsiento.SUBF;
                        comprobanteAsiento.PrecioUnitario = datosAsiento.SUBF;
                        comprobanteAsiento.Igv = datosAsiento.IGVF;
                        comprobanteAsiento.Total = datosAsiento.TOTALF;
                        comprobanteAsiento.Usuario = usuario;
                        comprobanteAsiento.TipoNotaCredito = "";

                        facturacionDA.RegistrarComprobante(comprobanteAsiento);

                        comprobanteAsiento.ListaDetalle = new List<DetalleFactura>();
                        DetalleFactura detalle = new DetalleFactura();
                        detalle.id_comprobante = comprobanteAsiento.IdComprobante;
                        detalle.descripcion = string.Format(descripcionComprobante, datosAsiento.FONDO, datosAsiento.TEXTO1, datosAsiento.TEXTO2);
                        detalle.cuenta = "";
                        detalle.afectoImpuesto = ConstantesING.SI;
                        detalle.importe = datosAsiento.SUBF;

                        facturacionDA.RegistrarDetalleComprobante(detalle, usuario);
                        referenciaComprobante = comprobanteAsiento.Serie + "-" + comprobanteAsiento.Numero;
                        if (!datosAsiento.IsID_OPERACIONNull() && datosAsiento.TIPO_ASIENTO == "VT_CRA")
                        {
                            ht.Add(datosAsiento.ID_OPERACION, referenciaComprobante);
                        }
                        #region CODIGO_COMENTADO
                        //string respuesta = facturacionDA.InsertarComprobante(comprobante.TIPO_DOCUMENTO,
                        //    datosAsiento.MONEDA == "PEN" ? "S" : "D", datosAsiento.FECHA_PROCESO,
                        //    datosAsiento.CODIGO_CLIENTE, "", "", comprobante.COD_CONCEPTO, "", "", Convert.ToDateTime("01/01/1900"),
                        //    //OT 7349 INI
                        //    ConstantesING.ESTADO_ACTIVO, ConstantesING.SI, ConstantesING.SI, ConstantesING.NO,
                        //    //OT 7349 FIN
                        //    comprobante.GLOSA, 1, datosAsiento.SUBF, datosAsiento.SUBF, datosAsiento.IGVF,
                        //    datosAsiento.TOTALF, usuario, "");
                        //idComprobante = 0;
                        //bool esNumero = Int32.TryParse(respuesta, out idComprobante);
                        //DataTable dtComprobante = facturacionDA.ObtenerComprobante(comprobanteAsiento.IdComprobante.ToString());
                        //referenciaComprobante = comprobanteAsiento.Serie + "-" + comprobanteAsiento.Numero;
                        //if (!datosAsiento.IsID_OPERACIONNull() && datosAsiento.TIPO_ASIENTO == "VT_CRA")
                        //{
                        //    ht.Add(datosAsiento.ID_OPERACION, referenciaComprobante);
                        //}
                        ////OT 7349 INI
                        //if (esNumero)
                        //{
                        //    facturacionDA.IngresarDetalle(idComprobante, string.Format(descripcionComprobante, datosAsiento.FONDO, datosAsiento.TEXTO1, datosAsiento.TEXTO2), "",
                        //        ConstantesING.SI, datosAsiento.SUBF, usuario);
                        //}
                        //OT 7349 FIN
                        //VERIFICAR SI datosAsiento TIENE VALOR EN UUID. SI TIENE, SE ACTUALIZA EL COMPROBANTE, SINO SE INSERTA
                        #endregion
                    }

                    string uuid = null;
                    if (sapActivo) //solo se generan los asientos si es que el flag de sap se encuentra activo.
                    {
                        //string uuid = da.RegistrarControl(usuario, direccionIP, referencia, cn, trans);
                        uuid = da.RegistrarControl(usuario, direccionIP, referencia, cantidadSubDetalle, datosAsiento.TIPO_ASIENTO, cn, trans);
                        //cantidadSubDetalle

                        if (datosAsiento.TIPO_ASIENTO == "PD_CRA")
                        {
                            referenciaComprobante = ht[datosAsiento.ID_OPERACION].ToString();
                        }


                        //continuar validando, probablemente este bloque tambien puede ir dentro
                        Asiento.SAPAR1Row sapar1Row = asiento.SAPAR1.NewSAPAR1Row();
                        sapar1Row.SAPUUI = uuid;
                        sapar1Row.SAPSOC = drCabeceraTipo["SOCIEDAD_FI"].ToString().Trim();
                        #region CODIGO_COMENTADO
                        //if (datosAsiento.TIPO_ASIENTO == "PD_CRA")
                        //{
                        //    sapar1Row.SAPNU5 = datosAsiento.REFERENCIA;
                        //    datosAsiento.REFERENCIA = "";
                        //}
                        //else
                        //{

                        //}
                        #endregion
                        sapar1Row.SAPNU5 = generaAsiento;
                        
                        sapar1Row.SAPCLA = drCabeceraTipo["CLASE_DOCUMENTO"].ToString().Trim();
                        sapar1Row.SAPMON = drCabeceraTipo["MONEDA_DOCUMENTO"].ToString().Trim();
                        sapar1Row.SAPFEC = datosAsiento.FECHA_PROCESO.ToString("yyyyMMdd");
                        sapar1Row.SAPFE1 = datosAsiento.FECHA_PROCESO.ToString("yyyyMMdd");
                        //sapar1Row.SAPFE2 = datosAsiento.FECHA_PROCESO.ToString("yyyyMMdd");
                        //colocando tipo de cambio para asientos con TC
                        if (datosAsiento.TIPO_CAMBIO > 0)
                        {
                            sapar1Row.SAPTAS = datosAsiento.TIPO_CAMBIO;
                        }
                        else
                        {
                            sapar1Row.SAPFE2 = datosAsiento.FECHA_PROCESO.ToString("yyyyMMdd");
                        }
                        sapar1Row.SAPREF = datosAsiento.IsREFERENCIANull() || datosAsiento.REFERENCIA == "" ? (drCabeceraTipo.IsNull("REFERENCIA") || drCabeceraTipo["REFERENCIA"].ToString().Trim() == "" ? referencia : drCabeceraTipo["REFERENCIA"].ToString().Trim()) : datosAsiento.REFERENCIA;
                        if (datosAsiento.TIPO_ASIENTO == "VT_ING" || datosAsiento.TIPO_ASIENTO == "VT_CRA" || datosAsiento.TIPO_ASIENTO == "PD_CRA")
                        {
                            sapar1Row.SAPREF = referenciaComprobante;
                            
                        }
                        string textoCabecera1 = string.Format(drCabeceraTipo["TEXTO_CABECERA"].ToString(), datosAsiento.TEXTO1, datosAsiento.TEXTO2);
                        if (textoCabecera1.Length > 25)
                        {
                            textoCabecera1 = textoCabecera1.Substring(0, 25);
                        }
                        sapar1Row.SAPTEX = textoCabecera1;
                        sapar1Row.SAPLIB = string.Empty;

                        asiento.SAPAR1.AddSAPAR1Row(sapar1Row);

                        da.RegistrarCabeceraAsiento(sapar1Row, cn, trans);

                        int i = 1;
                        int k = 1;
                        foreach (DataRow filaMovimiento in drDetallesTipo)
                        {
                            if (Convert.ToDecimal(datosAsiento["MONTO" + i.ToString()]) == 0)
                            {
                                i++;
                                continue;
                            }
                            Asiento.SAPAR2Row sapar2Row = asiento.SAPAR2.NewSAPAR2Row();
                            sapar2Row.SetParentRow(sapar1Row);
                            sapar2Row.SAPNU7 = k;
                            sapar2Row.SAPTI4 = filaMovimiento["CLAVE_CONTABILIZACION"].ToString().Trim(); //TIPO_MOVIMIENTO
                            sapar2Row.SAPCL2 = string.Empty; //CLASE MOVIMIENTO
                            sapar2Row.SAPIMP = Convert.ToDecimal(datosAsiento["MONTO" + i.ToString()]);
                            i++;
                            k++;
                            string glosa = string.Format(filaMovimiento["TEXTO_POSICION"].ToString(), datosAsiento.TEXTO1, datosAsiento.TEXTO2);
                            if (glosa.Length > 50)
                            {
                                glosa = glosa.Substring(0, 50);
                            }
                            sapar2Row.SAPTE1 = glosa;
                            sapar2Row.SAPASI = filaMovimiento["ASIGNACION"].ToString().Trim();
                            string referencia1 = string.Format(filaMovimiento["REFERENCIA1"].ToString(), datosAsiento.REFERENCIA1);
                            if (referencia1.Length > 12)
                            {
                                referencia1 = referencia1.Substring(0, 12);
                            }
                            sapar2Row.SAPRE3 = referencia1;
                            string referencia3 = string.Format(filaMovimiento["REFERENCIA3"].ToString(), datosAsiento.REFERENCIA3);
                            if (referencia3.Length > 20)
                            {
                                referencia3 = referencia3.Substring(0, 20);
                            }
                            sapar2Row.SAPR04 = referencia3;
                            sapar2Row.SAPCUE = filaMovimiento["CUENTA"].ToString().Trim().PadRight(10, '0');
                            if (filaMovimiento["TIPO_OBJETO_IMPUTACION"] != null)
                            {
                                sapar2Row.SAPTI5 = filaMovimiento["TIPO_OBJETO_IMPUTACION"].ToString().Trim();
                                sapar2Row.SAPOBJ = filaMovimiento["OBJETO_IMPUTACION"].ToString().Trim();
                            }
                            sapar2Row.SAPIND = filaMovimiento["INDICADOR_IGV"].ToString().Trim();
                            sapar2Row.SAPFE3 = datosAsiento.FECHA_PROCESO.ToString("yyyyMMdd");
                            sapar2Row.SAPCON = "";
                            sapar2Row.SAPVIA = "";

                            asiento.SAPAR2.AddSAPAR2Row(sapar2Row);
                            da.RegistrarDetalleAsiento(sapar2Row, cn, trans);
                        }
                        da.ActualizarControl(uuid, cn, trans);
                        da.RegistrarControlAsiento(uuid, datosAsiento, usuario, cn, trans);
                        if (datosAsiento.TIPO_ASIENTO == "VT_ING" || datosAsiento.TIPO_ASIENTO == "VT_CRA")
                        {
                            facturacionDA.ActualizarComprobanteIdentificador(comprobanteAsiento.IdComprobante, uuid);
                        }
                    }
                }
                
                trans.Commit();
                facturacionDA.CommitTransaccion();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                facturacionDA.RollbackTransaccion();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                cn.Close();
                cn.Dispose();
                facturacionDA.LiberarConexion();
            }
        }

        //OT7999 INI
        public bool ObtenerDetraccion()
        {
            bool valorDetraccionEncontrado = true;
            string strCodigo = ConstantesING.CODIGO_PORCENTAJE_DETRACCION;
            string strItem = ConstantesING.PORCENTAJE_DETRACCION;

            DataTable dt = ObtenerDatosParametro(strCodigo, strItem);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["CN1"] != DBNull.Value)
                {
                    DataRow row = dt.Rows[0];
                    dblPorcentajeDetraccion = Convert.ToDouble(row["CN1"]);
                }
                else{
                    valorDetraccionEncontrado = false;
                }

            }
            else
            {
                //MessageBox.Show("No existe el valor del porcentaje de detracción.", "Comprobantes");
                valorDetraccionEncontrado = false;
            }
            return valorDetraccionEncontrado;
        }

        public DataTable ObtenerDatosParametro(string strCodigo, string strItem)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ListaParametros(strCodigo, strItem));
        }

        public DataTable ObtenerTipoCambio(string strFecha)
        {
            AsientosDA da = new AsientosDA();
            DataTable ObtenerTipoCambio = da.ObtenerTipoCambio(strFecha);
            return ObtenerTipoCambio;
        }

        public bool setearTipoCambio(DateTime dtFecha){
            bool tipoCambioEncontrado = true;
            string sFecha = dtFecha.ToString("dd/MM/yyyy");
            DataTable dtTipoCambio = ObtenerTipoCambio(sFecha);

            if(dtTipoCambio.Rows.Count > 0){
                if (dtTipoCambio.Rows[0]["VEN_SUNAT"] != DBNull.Value)
                {
                    tipoCambio = Convert.ToDouble(dtTipoCambio.Rows[0]["VEN_SUNAT"]);
                }
                else{
                    tipoCambioEncontrado = false;
                }
            }
            else{
                tipoCambioEncontrado = false;
            }

            return tipoCambioEncontrado;
        }
        //OT7999 FIN
    }
}

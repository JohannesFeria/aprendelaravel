/*
 * Fecha de Modificación: 22/05/2015
 * Modificado por: Alex Vega
 * Numero de OT: 7349
 * Descripción del cambio: Creación
 * */
/*
 * Fecha de Modificación: 06/08/2015
 * Modificado por: Robert Castillo
 * Numero de OT: 7349
 * Descripción del cambio: Se agrega el método ObtenerTablaGeneral, que se encarga de llamar al 
 *                         método del mismo nombre en la clase FacturacionDA.cs.
 * */
/*
 * Fecha de Modificación: 26/08/2015
 * Modificado por: Juan Castro
 * Numero de OT: 7584
 * Descripción del cambio: Se implementa envío de comprobantes electrónicos.
 * */
/*
 * Fecha de Modificación: 20/11/2015
 * Modificado por: Robert Castillo
 * Numero de OT: 7999
 * Descripción del cambio: Se implementa obtención del tipo de cambio.
 * */
/*
* Fecha de Modificación:   02/02/2016
* Modificado por:          Irene Reyes
* Numero de OT:            8365
* Descripción del cambio:  Se modifica la clase para agregar el nuevo fondo SURA DEPOSITOS I DOLARES.
* */
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Procesos.TD;
using Procesos.DA;
using System.Data.SqlClient;
using Procesos.Constants;
using SistemaProcesosServices;
using INGFondos.Constants;
using System.IO;
using con_NumLetraC;
using System.Configuration;


namespace Procesos.BL
{
    public class FacturacionBO
    {
        FacturacionDA facturacionDA;
        public FacturacionBO()
        {
            this.facturacionDA = new FacturacionDA();
        }

        public DataTable DameEstadoSAP(string strCodigo, string strItem){          

            FacturacionDA da = new FacturacionDA();
            return (da.ListaParametros(strCodigo, strItem));            
        }

        public DataTable ListarTalonarios(string strTipo ,string strOrden)
        {
            FacturacionDA da = new FacturacionDA();
            //FacturacionDA da = new FacturacionDA();
            //da.Server = "ServidorPrem_Banco";
            //da.Database = "BDPrem_Banco";  
            return (da.ListarTalonarios(strTipo, strOrden));
        }
        #region CODIGO_COMENTADO
        //public DataTable ListarMonedas(string strCodigo, string strItem)
        //{
        //    FacturacionDA dt = new FacturacionDA();
        //    return (dt.ListaParametros(strCodigo, strItem));
        //}

        //public DataTable ListarTipoDocumento(string strCodigo, string strItem)
        //{
        //    FacturacionDA dt = new FacturacionDA();
        //    return (dt.ListaParametros(strCodigo, strItem));
        //}
        #endregion
        public DataTable ListarComprobantes(string strTipoComprobante, string strSerie, string strOrden)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ListarComprobantes(strTipoComprobante, strSerie, strOrden));
        }
        
        public DataSet ObtenerTalonario(string strCodigo)
        {
            DataSet ds = new DataSet();
            FacturacionDA da = new FacturacionDA();
            ds.Tables.Add(da.ObtenerTalonario(strCodigo));
            return ds;
        }

        public int CorrelativoTalonario()
        {
            FacturacionDA da = new FacturacionDA();
            return (da.CorrelativoTalonario());
        }

        public string EliminarTalonario(int intCodigoTalonario)
        {
            FacturacionDA da = new FacturacionDA();
            string mensaje = da.EliminarTalonario(intCodigoTalonario);
            return mensaje;
        }

        public string InsertarTalonario(string strTipo, string strSerie, int intInicio, int intFin, string strEstado, string strUsuario)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.InsertarTalonario(strTipo, strSerie, intInicio, intFin, strEstado, strUsuario));
        }

        public string ActualizarTalonario(int intCodigo, int intFin, string strEstado, string strUsuario)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ActualizarTalonario(intCodigo, intFin, strEstado, strUsuario));            
        }

        public DataSet ObtenerComprobante(string strCodigo) 
        {
            DataSet ds = new DataSet();
            FacturacionDA da = new FacturacionDA();
            ds.Tables.Add(da.ObtenerComprobante(strCodigo));
            return ds;
        }

        public DataTable ObtenerDatosParametro(string strCodigo, string strItem)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ListaParametros(strCodigo, strItem));
        }

        public DataTable ListarComprobanteDetalle()
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ListarComprobanteDetalle());
        }

        public DataTable ObtenerCliente(string ruc)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ObtenerCliente(ruc));
        }

        public string EliminarComprobante(string strCodigo, string strEstado, string strCodigoUsuario)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.EliminarComprobante(strCodigo, strEstado, strCodigoUsuario));
        }

        public string InsertarCliente(string strRuc, string txtRazonSocial, string txtDireccion,
                string strDistrito, string strProvincia, string strDepartamento, string codigoUsuario)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.InsertarCliente(strRuc, txtRazonSocial, txtDireccion,
                strDistrito, strProvincia, strDepartamento, codigoUsuario));
        }

        public string InsertarComprobante(string tipoComprobante, string moneda, DateTime fecha,
        string ruc, string proceso, string expediente, string concepto, string tipoDocRel, string NumDocRel,
        DateTime fechaDocRel, string estado, string impreso, string enviado, string afecto, string glosa,
        int cantidad, double precioUnitario, double subTotal, double igv, double total, string codigoUsuario,
        string uuid)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.InsertarComprobante(tipoComprobante, moneda, fecha, ruc, proceso, expediente,
            concepto, tipoDocRel, NumDocRel, fechaDocRel, estado, impreso, enviado, afecto, glosa, cantidad, precioUnitario,
            subTotal, igv, total, codigoUsuario, uuid));
        }

        public string ActualizarComprobante(string accion, int codigo, string tipoComprobante, string moneda, DateTime fecha,
        string ruc, string proceso, string expediente, string concepto, string tipoDocRel, string NumDocRel,
        DateTime fechaDocRel, string estado, string impreso, string enviado, string afecto, string glosa,
        int cantidad, double precioUnitario, double subTotal, double igv, double total, string codigoUsuario,
        string uuid)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ActualizarComprobante(accion, codigo, tipoComprobante, moneda, fecha, ruc, proceso, expediente,
            concepto, tipoDocRel, NumDocRel, fechaDocRel, estado, impreso, enviado, afecto, glosa, cantidad, precioUnitario,
            subTotal, igv, total, codigoUsuario, uuid));
        }

        public string ActualizarCorrelativoComprobante(string idComprobante, string numero)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ActualizarCorrelativoComprobante(idComprobante, numero));
        }

        public void ActualizarDetalle(int id_comprobante, int orden_detalle, string descripcion,
            string cuenta, string afectoImpuesto, double importe, string user)
        {
            FacturacionDA da = new FacturacionDA();
            da.ActualizarDetalle(id_comprobante, orden_detalle, descripcion,
            cuenta, afectoImpuesto, importe, user);
        }

        public void IngresarDetalle(int id_comprobante, string descripcion,
            string cuenta, string afectoImpuesto, double importe, string user)
        {
            FacturacionDA da = new FacturacionDA();
            da.IngresarDetalle(id_comprobante, descripcion,
            cuenta, afectoImpuesto, importe, user);
        }

        public DataSet ListarComprobanteImpresion(string strTipoComprobante, string strSerie, int intInicio, int intFinal)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ListarComprobanteImpresion(strTipoComprobante, strSerie, intInicio, intFinal));
        }

        public string ValidarRango(string strTipoComprobante, string strSerie, string strInicio, string strFinal)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ValidarRango(strTipoComprobante, strSerie, strInicio, strFinal));
        }

        public DataTable ObtenerComprobanteDetalle(string codigo)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ObtenerComprobanteDetalle(codigo));
        }
        public int EliminarComprobanteDetalle(int id_Comprobante,int cod_Detalle)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.EliminarComprobanteDetalle(id_Comprobante,cod_Detalle));
        }

        public string InsertarComprobanteDetalle(string strRuc, string strRazonSocial, string strDireccion,
        string strDistrito, string strProvincia, string strDepartamento, string tipoComprobante, string moneda,
        DateTime fecha, string ruc, string proceso, string expediente, string concepto, string tipoDocRel,
        string numDocRel, DateTime fechaDocRel, string estado, string impreso, string enviado, string afecto,
        string glosa, int cantidad, double precioUnitario, double subTotal, double igv, double total,
        string codigoUsuario, string uuid, List<DetalleFactura> listDetalle)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.InsertarComprobanteDetalle(strRuc, strRazonSocial, strDireccion, strDistrito, strProvincia,
            strDepartamento, tipoComprobante, moneda, fecha, ruc, proceso, expediente, concepto, tipoDocRel, numDocRel, fechaDocRel,
            estado, impreso, enviado, afecto, glosa, cantidad, precioUnitario, subTotal, igv, total, codigoUsuario, uuid, listDetalle));
        }

        public string ActualizarComprobanteDetalle(string strRuc, string strRazonSocial, string strDireccion, string strDistrito,
        string strProvincia, string strDepartamento, string accion, int codigo, string tipoComprobante, string moneda, DateTime fechaEmision,
        string ruc, string proceso, string expediente, string concepto, string tipoDocRel, string numDocRel, DateTime fechaDocRel,
        string estado, string impreso, string enviado, string afecto, string glosa, int cantidad, double precioUnitario, double subTotal,
        double igv, double total, string codigoUsuario, string uuid, List<DetalleFactura> listDetalle)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ActualizarComprobanteDetalle(strRuc, strRazonSocial, strDireccion, strDistrito, strProvincia, strDepartamento,
            accion, codigo, tipoComprobante, moneda, fechaEmision, ruc, proceso, expediente, concepto, tipoDocRel, numDocRel, fechaDocRel,
            estado, impreso, enviado, afecto, glosa, cantidad, precioUnitario, subTotal, igv, total, codigoUsuario, uuid, listDetalle));
        }

        //OT7349 INI
        public DataTable ObtenerTablaGeneral(string codigoTabla)
        {
            FacturacionDA da = new FacturacionDA();
            return (da.ObtenerTablaGeneral(codigoTabla));
        }
        //OT7349 FIN

        public string EnviarComprobante(string tipoComprobante,string serie, string numero, string codigoUsuario)
        {
            FacturacionDA da = new FacturacionDA();

            ParametrosDA parametroDA = new ParametrosDA();

            DataTable dt = null;
            string rutaApp = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string rutaModelo = rutaApp + ConfigurationSettings.AppSettings[ConstantesING.RUTA_MODELO];
            string rutaArchivo = rutaApp + ConfigurationSettings.AppSettings[ConstantesING.RUTA_ARCHIVOS];

            switch (tipoComprobante)
            {
                case "01":
                    dt = da.ObtenerFacturaParaEnvio(serie, numero);
                    break;
                case "03":
                    dt = da.ObtenerBoletaParaEnvio(serie, numero);
                    break;
                case "07":
                    dt = da.ObtenerNCParaEnvio(serie, numero);
                    break;
                
            }

            string destinoCsv = rutaArchivo + ConfigurationSettings.AppSettings[tipoComprobante+"csv"];
            string destinoRespuesta = rutaArchivo + ConfigurationSettings.AppSettings[tipoComprobante + "respuesta"];

            string flagAfectoIGV = dt.Rows[0]["FLAG_AFECTOIGV"].ToString();

            DataTable dtParametro = parametroDA.ObtenerValoresParametros(ConstantesING.TABLA_MODELO, tipoComprobante + "-" + flagAfectoIGV);
            string nombreArchivoModelo = dtParametro.Rows[0]["CAMPO1"].ToString();

            //FileStream modelo = File.Open(rutaModelo + nombreArchivoModelo, FileMode.Open);
            StreamReader modelo = new StreamReader(rutaModelo + nombreArchivoModelo);
            string contenidoModelo = modelo.ReadToEnd();

            //Reemplazando valores en el modelo
            string fechaEmision = dt.Rows[0]["FECHA_EMISION"].ToString();
            string numeroDocumento = dt.Rows[0]["NUMERO_DOCUMENTO"].ToString();
            string tipoDocumento = dt.Rows[0]["TIPO_DOCUMENTO"].ToString();
            string moneda = dt.Rows[0]["MONEDA"].ToString();
            string igv = Convert.ToDecimal(dt.Rows[0]["IGV"]).ToString("#0.00");
            string subTotal = Convert.ToDecimal(dt.Rows[0]["VALOR_VENTA"]).ToString("#0.00");
            string total = Convert.ToDecimal(dt.Rows[0]["TOTAL"]).ToString("#0.00");
            string docCliente = dt.Rows[0]["DOC_CLIENTE"].ToString();
            string tipoDocCliente = dt.Rows[0]["TIPO_DOC_CLIENTE"].ToString();
            string razonSocial = dt.Rows[0]["RAZON_SOCIAL"].ToString();
            string ubigeo = dt.Rows[0]["UBIGEO"].ToString();
            string codigoPostal = dt.Rows[0]["CODIGO_POSTAL"].ToString();
            string direccionCliente = dt.Rows[0]["DIRECCION_CLIENTE"].ToString();
            string departamentoCliente = dt.Rows[0]["DEP_CLIENTE"].ToString();
            string provinciaCliente = dt.Rows[0]["PRO_CLIENTE"].ToString();
            string distritoCliente = dt.Rows[0]["DIS_CLIENTE"].ToString();
            string pais = dt.Rows[0]["PAIS_CLIENTE"].ToString();
            string correo = dt.Rows[0]["CORREO_CLIENTE"].ToString();
            string cantidad = dt.Rows[0]["CANTIDAD"].ToString();
            string descripcion = dt.Rows[0]["DESCRIPCION"].ToString();
            string porcIgv = dt.Rows[0]["PORC_IGV"].ToString();
            string tipoDocRelacionado = dt.Rows[0]["TIPO_DOCREL"].ToString();
            //OT 8365 INI
            string numDocRelacionado = tipoDocRelacionado == "01" ? "F" : "B";
            //OT 8365 FIN
            numDocRelacionado = numDocRelacionado + dt.Rows[0]["NUMERO_DOCREL"].ToString();
            string tipoNC = dt.Rows[0]["TIPO_NC"].ToString();
            string glosa = dt.Rows[0]["GLOSA"].ToString();
            NumLetra nl = new NumLetra();
            string montoTexto = nl.Convertir(dt.Rows[0]["TOTAL"].ToString(), moneda=="PEN"? "Nuevos Soles": "Dólares americanos", true, tipoDocumento);
            string unidad = "UN";

            //OT7999 INI
            string montoDetraccion = string.Empty;
            string porcentajeDetraccion = string.Empty;
            string numeroBancoNancion = string.Empty;
            string detracMontoTotal = string.Empty;

            string codTabScotiabankMN = string.Empty;
            string codItemScotiabankMN = string.Empty;

            string codTabScotiabankCCI = string.Empty;
            string codItemScotiabankCCI = string.Empty;

            string cuentaScotiabankMN = string.Empty;
            string cuentaScotiabankCCI = string.Empty;

            string codTabScotiabankMNTexto = string.Empty;
            string codItemScotiabankMNTexto = string.Empty;

            string codTabScotiabankCCITexto = string.Empty;
            string codItemScotiabankCCITexto = string.Empty;

            string textoScotiabankMN = string.Empty;
            string textoScotiabankCCI = string.Empty;

            string textoCuentasScotiabank = string.Empty;

            /*Cargando los valores adicionales para las facturas*/
            if (tipoComprobante.Equals(ConstantesING.TIPO_DOCUMENTO_FACTURA))
            {
                double dTotal = Convert.ToDouble(dt.Rows[0]["TOTAL"]);
                double dDetraccion = Convert.ToDouble(dt.Rows[0]["MONTO_DETRACCION"]);

                if (dDetraccion != 0) //Se ha calculado la detraccion
                {
                    montoDetraccion = Convert.ToDecimal(dt.Rows[0]["MONTO_DETRACCION"]).ToString("#0.00");
                    porcentajeDetraccion = Convert.ToDecimal(dt.Rows[0]["PORCENTAJE_DETRACCION"]).ToString("#0.00");

                    //Cargando el numero de banco de la nacion.
                    string strCodigoBancoNacion = ConstantesING.CODIGO_NUMERO_BANCO_NACION;
                    string strNumeroBancoNacion = ConstantesING.NUMERO_BANCO_NACION;
                    DataTable dtNumeroBancoNacion = ObtenerDatosParametro(strCodigoBancoNacion, strNumeroBancoNacion);

                    if (dtNumeroBancoNacion.Rows.Count > 0)
                    {
                        if (dtNumeroBancoNacion.Rows[0]["Descripcion"] != DBNull.Value)
                        {
                            numeroBancoNancion = dtNumeroBancoNacion.Rows[0]["Descripcion"].ToString();
                        }
                        else
                        {
                            //se sigue procesando??
                        }
                    }
                    else
                    {
                        //se sigue procesando??
                    }

                    double dDetracMontoTotal = dTotal - dDetraccion;
                    detracMontoTotal = dDetracMontoTotal.ToString("#0.00");
                }
            }

            if (tipoComprobante.Equals(ConstantesING.TIPO_DOCUMENTO_FACTURA) || tipoComprobante.Equals(ConstantesING.TIPO_DOCUMENTO_BOLETA))
            {
                if (moneda == "PEN")
                {
                    //soles
                    codTabScotiabankMN = ConstantesING.CODIGO_SCOTIABANK_SOLES_MN;
                    codItemScotiabankMN = ConstantesING.SCOTIABANK_SOLES_MN;

                    codTabScotiabankCCI = ConstantesING.CODIGO_SCOTIABANK_SOLES_CCI;
                    codItemScotiabankCCI = ConstantesING.SCOTIABANK_SOLES_CCI;

                    codTabScotiabankMNTexto = ConstantesING.CODIGO_SCOTIABANK_SOLES_MN_TEXTO;
                    codItemScotiabankMNTexto = ConstantesING.SCOTIABANK_SOLES_MN_TEXTO;

                    codTabScotiabankCCITexto = ConstantesING.CODIGO_SCOTIABANK_SOLES_CCI_TEXTO;
                    codItemScotiabankCCITexto = ConstantesING.SCOTIABANK_SOLES_CCI_TEXTO;  

                }
                else { 
                    //dolares
                    codTabScotiabankMN = ConstantesING.CODIGO_SCOTIABANK_DOLARES_MN;
                    codItemScotiabankMN = ConstantesING.SCOTIABANK_DOLARES_MN;

                    codTabScotiabankCCI = ConstantesING.CODIGO_SCOTIABANK_DOLARES_CCI;
                    codItemScotiabankCCI = ConstantesING.SCOTIABANK_DOLARES_CCI;

                    codTabScotiabankMNTexto = ConstantesING.CODIGO_SCOTIABANK_DOLARES_MN_TEXTO;
                    codItemScotiabankMNTexto = ConstantesING.SCOTIABANK_DOLARES_MN_TEXTO;

                    codTabScotiabankCCITexto = ConstantesING.CODIGO_SCOTIABANK_DOLARES_CCI_TEXTO;
                    codItemScotiabankCCITexto = ConstantesING.SCOTIABANK_DOLARES_CCI_TEXTO;  
                }

                DataTable dtCuentaScotiabankMN = ObtenerDatosParametro(codTabScotiabankMN, codItemScotiabankMN);

                if (dtCuentaScotiabankMN.Rows.Count > 0)
                {
                    if (dtCuentaScotiabankMN.Rows[0]["Descripcion"] != DBNull.Value)
                    {
                        cuentaScotiabankMN = dtCuentaScotiabankMN.Rows[0]["Descripcion"].ToString();
                    }
                    else
                    {
                        //se sigue procesando??
                    }
                }
                else
                {
                    //se sigue procesando??
                }

                DataTable dtCuentaScotiabankCCI = ObtenerDatosParametro(codTabScotiabankCCI, codItemScotiabankCCI);

                if (dtCuentaScotiabankCCI.Rows.Count > 0)
                {
                    if (dtCuentaScotiabankCCI.Rows[0]["Descripcion"] != DBNull.Value)
                    {
                        cuentaScotiabankCCI = dtCuentaScotiabankCCI.Rows[0]["Descripcion"].ToString();
                    }
                    else
                    {
                        //se sigue procesando??
                    }
                }
                else
                {
                    //se sigue procesando??
                }

                DataTable dtScotiabankTextoMN = ObtenerDatosParametro(codTabScotiabankMNTexto, codItemScotiabankMNTexto);

                if (dtScotiabankTextoMN.Rows.Count > 0)
                {
                    if (dtScotiabankTextoMN.Rows[0]["Descripcion"] != DBNull.Value)
                    {
                        textoScotiabankMN = dtScotiabankTextoMN.Rows[0]["Descripcion"].ToString();
                    }
                    else
                    {
                        //se sigue procesando??
                    }
                }
                else
                {
                    //se sigue procesando??
                }

                DataTable dtScotiabankTextoCCI = ObtenerDatosParametro(codTabScotiabankCCITexto, codItemScotiabankCCITexto);

                if (dtScotiabankTextoCCI.Rows.Count > 0)
                {
                    if (dtScotiabankTextoCCI.Rows[0]["Descripcion"] != DBNull.Value)
                    {
                        textoScotiabankCCI = dtScotiabankTextoCCI.Rows[0]["Descripcion"].ToString();
                    }
                    else
                    {
                        //se sigue procesando??
                    }
                }
                else
                {
                    //se sigue procesando??
                }

                //cuentaScotiabankMN = "Scotiabank Cuenta Corriente MN " + cuentaScotiabankMN;
                cuentaScotiabankMN = textoScotiabankMN + " " + cuentaScotiabankMN;
                //cuentaScotiabankCCI = "CCI: " + cuentaScotiabankCCI;
                cuentaScotiabankCCI = textoScotiabankCCI + " " + cuentaScotiabankCCI;

                textoCuentasScotiabank = cuentaScotiabankMN + " " + cuentaScotiabankCCI;
            }
            //OT7999 FIN

            string usuarioSOL= ConfigurationSettings.AppSettings["UsuarioSol"];
            string passwordSOL = ConfigurationSettings.AppSettings["ClaveSol"];

            string contenidoArchivo = contenidoModelo.Replace("{FECHA_EMISION}", fechaEmision)
                .Replace("{NUMERO_DOCUMENTO}", numeroDocumento)
                .Replace("{TIPO_DOCUMENTO}", tipoDocumento)
                .Replace("{MONEDA}", moneda)
                .Replace("{IGV}", igv)
                .Replace("{TOTAL}", total)
                .Replace("{VALOR_VENTA}", subTotal)
                //OT 7999 INI
                .Replace("{DETRAC_MONTO}", montoDetraccion)
                .Replace("{DETRAC_PORCENTAJE}", porcentajeDetraccion)
                .Replace("{DETRAC_NUM_BANCO}", numeroBancoNancion)
                .Replace("{DETRAC_MONTO_TOTAL}", detracMontoTotal)
                //OT 7999 FIN
                .Replace("{DOC_CLIENTE}", docCliente)
                .Replace("{TIPO_DOC_CLIENTE}", tipoDocCliente)
                .Replace("{RAZON_SOCIAL}", razonSocial)
                .Replace("{UBIGEO_CLIENTE}", ubigeo)
                .Replace("{DIRECCION_CLIENTE}", direccionCliente)
                .Replace("{DEP_CLIENTE}", departamentoCliente)
                .Replace("{PRO_CLIENTE}", provinciaCliente)
                .Replace("{DIS_CLIENTE}", distritoCliente)
                .Replace("{PAIS_CLIENTE}", pais)
                .Replace("{CORREO_CLIENTE}", correo)
                .Replace("{MONTO_TEXTO}", montoTexto)
                //OT 7999 INI
                .Replace("{SCOTIABANK}", textoCuentasScotiabank)
                //OT 7999 FIN
                .Replace("{UNIDAD}", unidad)
                .Replace("{CANTIDAD}", cantidad)
                .Replace("{DESCRIPCION_DETALLE}", descripcion)
                .Replace("{PORC_IGV}", porcIgv)
                .Replace("{NUM_DOC_REL}", numDocRelacionado)
                .Replace("{TIP_DOC_REL}", tipoDocRelacionado)
                .Replace("{TIPO_AFECTACION}", tipoNC)
                .Replace("{GLOSA}", glosa)
                .Replace("{USUARIO}", usuarioSOL)
                .Replace("{CLAVE}", passwordSOL);

            string rutaArchivoCompleta = destinoCsv + numeroDocumento + ".csv";
            StreamWriter sw = new StreamWriter(rutaArchivoCompleta);
            sw.Write(contenidoArchivo);
            sw.Close();

            string rutaRespuesta = destinoRespuesta;

            //File.Move(rutaArchivoCompleta, rutaArchivoCompleta.Replace(".txt", ".csv"));

            string usuarioServicio = ConfigurationSettings.AppSettings["UsuarioServicio"];
            string passwordServicio = ConfigurationSettings.AppSettings["ClaveServicio"];

            string respuesta = string.Empty;

            switch (tipoComprobante)
            {
                case "01":
                    respuesta = EfactService.EnviarFactura(usuarioServicio, passwordServicio, rutaArchivoCompleta, rutaRespuesta, numeroDocumento, dt.Rows[0]);
                    break;
                case "03":
                    respuesta = EfactService.EnviarBoleta(usuarioServicio, passwordServicio, rutaArchivoCompleta, rutaRespuesta, numeroDocumento, dt.Rows[0]);
                    break;
                case "07":
                    respuesta = EfactService.EnviarNotaCredito(usuarioServicio, passwordServicio, rutaArchivoCompleta, rutaRespuesta, numeroDocumento, dt.Rows[0]);
                    break;
            }

            if (dt.Rows[0]["FLAG_ENVIADO"].ToString().Equals(ConstantesING.SI))
            {
                facturacionDA.Server = Conexiones.ServidorPremBanco13;
                facturacionDA.Database = Conexiones.BaseDeDatosPremBanco13;
                decimal idComprobante = Convert.ToDecimal(dt.Rows[0]["ID_COMPROBANTE"]);
                facturacionDA.CrearConexion();
                facturacionDA.AbrirConexion();
                facturacionDA.AbrirTransaccion();
                try
                {
                    facturacionDA.ActualizarEstadoEnvio(idComprobante, codigoUsuario);
                    facturacionDA.CommitTransaccion();
                }
                catch (Exception ex)
                {
                    facturacionDA.RollbackTransaccion();
                }
            }

            return respuesta;
        }

        public DataTable ListarPaises()
        {
            facturacionDA.Server = Conexiones.ServidorOperaciones;
            facturacionDA.Database = Conexiones.BaseDeDatosOperaciones;
            return facturacionDA.ListarPaises();
        }

        public DataTable ListarDepartamentos(int idPais)
        {
            facturacionDA.Server = Conexiones.ServidorOperaciones;
            facturacionDA.Database = Conexiones.BaseDeDatosOperaciones;
            return facturacionDA.ListarDepartamentos(idPais);
        }

        public DataTable ListarCiudades(int idDepartamento)
        {
            facturacionDA.Server = Conexiones.ServidorOperaciones;
            facturacionDA.Database = Conexiones.BaseDeDatosOperaciones;
            return facturacionDA.ListarCiudades(idDepartamento);
        }

        public DataTable ListarDistritos(int idCiudad)
        {
            facturacionDA.Server = Conexiones.ServidorOperaciones;
            facturacionDA.Database = Conexiones.BaseDeDatosOperaciones;
            return facturacionDA.ListarDistritos(idCiudad);
        }

        public void CrearComprobante(Comprobante comprobante)
        {
            facturacionDA.Server = Conexiones.ServidorPremBanco13;
            facturacionDA.Database = Conexiones.BaseDeDatosPremBanco13;

            facturacionDA.CrearConexion();
            facturacionDA.AbrirConexion();
            facturacionDA.AbrirTransaccion();

            try
            {
                facturacionDA.RegistrarCliente(comprobante.Cliente);
                int idComprobante = facturacionDA.RegistrarComprobante(comprobante);
                foreach (DetalleFactura dt in comprobante.ListaDetalle)
                {
                    dt.id_comprobante = comprobante.IdComprobante;
                    facturacionDA.RegistrarDetalleComprobante(dt, comprobante.Usuario);
                }

                facturacionDA.CommitTransaccion();

            }
            catch (Exception ex)
            {
                facturacionDA.RollbackTransaccion();
                throw ex;
            }
            finally
            {
                facturacionDA.LiberarConexion();
            }

        }

        public int ValidarTalonario(string tipoComprobante)
        {
            facturacionDA.Server = Conexiones.ServidorPremBanco13;
            facturacionDA.Database = Conexiones.BaseDeDatosPremBanco13;
            return facturacionDA.ValidarTalonario(tipoComprobante);
        }

        public void ActualizarComprobante(Comprobante comprobante)
        {
            facturacionDA.Server = Conexiones.ServidorPremBanco13;
            facturacionDA.Database = Conexiones.BaseDeDatosPremBanco13;

            facturacionDA.CrearConexion();
            facturacionDA.AbrirConexion();
            facturacionDA.AbrirTransaccion();

            try
            {
                facturacionDA.RegistrarCliente(comprobante.Cliente);
                facturacionDA.ActualizarComprobante(comprobante, ConstantesING.ACCION_ACTUALIZAR);
                foreach (DetalleFactura item in comprobante.ListaDetalle)
                {
                    if (item.orden_detalle != 0)
                    {
                        facturacionDA.ActualizarComprobanteDetalle(item, comprobante.Usuario);
                    }
                    else
                    {
                        facturacionDA.RegistrarDetalleComprobante(item, comprobante.Usuario);
                    }
                }

                facturacionDA.CommitTransaccion();

            }
            catch (Exception ex)
            {
                facturacionDA.RollbackTransaccion();
                throw ex;
            }
            finally
            {
                facturacionDA.LiberarConexion();
            }
        }
   
    /* Inicio OT7999*/
        /// <summary>
        /// Llama al método ObtenerTipoCambio de acceso a datos.
        /// </summary>
        /// <param name="strFecha">Fecha de proceso</param>
        /// <returns>Registro de tipo de cambio</returns>
        public DataTable ObtenerTipoCambio(string strFecha)
        {
            FacturacionDA da = new FacturacionDA();
            DataTable ObtenerTipoCambio = da.ObtenerTipoCambio(strFecha);
            return ObtenerTipoCambio;
        }
    /* Fin OT7999*/
    }
}

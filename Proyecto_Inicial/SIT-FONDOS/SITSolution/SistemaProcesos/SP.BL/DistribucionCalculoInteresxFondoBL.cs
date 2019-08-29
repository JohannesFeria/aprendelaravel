/*
 * Fecha Modificación: 16/06/2017
 * Modificado por: Anthony Joaquin
 * Nro de OT: 10748
 * Descripción del cambio: Creación.
 * */
/*
 * Fecha Modificación: 10/07/2017
 * Modificado por: Walter Rodriguez
 * Nro de OT: 10748 PSC001
 * Descripción del cambio:	Se agrega el método de registarRentabilidad.
 * */
/*
 * Fecha Modificación: 07/08/2017
 * Modificado por: Anthony Joaquin
 * Nro de OT: 10675
 * Descripción del cambio:	Se agrega la lógica del formulario al BL.
 * */
/*
 * Fecha Modificación: 09/08/2017
 * Modificado por: Anthony Joaquin
 * Nro de OT: 10604
 * Descripción del cambio:	Se agregan los siguientes métodos ObtenerDistribucionFondosFir, registarDistribucionFondosFIR, 
 *                          ValidarRentabilidad, EliminarRentabilidad y ObtenerFondoMidas.
 * */
/*
 * Fecha Modificación: 18/09/2017
 * Modificado por: Anthony Joaquin
 * Nro de OT: 10592
 * Descripción del cambio:	Se agregan los siguientes métodos listarPagoFlujosSobranteXPeriodo, registrarRentabilidadSobrantes, registrarRentabilidadTotal y eliminarPagoFlujoSobrantes.
 *                          Se agregan 2 Datatable para calcular los sobrantes y otro donde se donde se realiza la resta del sobrante para insertar en la tabla rentabilidad.
 * */
/*
 * 
 * Fecha Modificación: 27/11/2017
 * Modificado por: Anthony Joaquin
 * Nro de OT: 10944
 * Descripción del cambio:	Se comenta el datatable de distribiución inicial y  se agrega el parámetro de patrimonioContable. 
 *                          Añadir VC Calculado Después de la Distribución , que se calculará de la siguiente manera: (Patrimonio Contable + Valuación)/ Numero de Cuotas al Corte.
 *                          Añadir la fila rentabilidad que se calcula con la diferencia entre el VC Calculado – VC Corte
 *                          Registrar los datos calculados de la fila RENTABILIDAD en la tabla Rentabilidad de Tributación                         
 * */
/*
 * 
 * Fecha Modificación: 29/12/2017
 * Modificado por: Anthony Joaquin
 * Nro de OT: OT10987
 * Descripción del cambio:	Se modifica los valores de MONTO y RENTABILIDAD del datatable que almacena los montos a guardar en las tablas de Distribución Fir y Rentabilidad.
 * */

using System;
using System.Data;
using Procesos.TD;
using Procesos.DA;
using System.Data.SqlClient;
using INGFondos.Constants;
using INGFondos.Data;
using Procesos.Constants;
using System.IO;
using System.Configuration;
using INGFondos.Support.Interop;
using System.Text;
using System.Globalization;
using System.Threading;


namespace Procesos.BL
{
    /// <summary>
    /// Descripción breve de DistribucionCalculoInteresxFondoBO.
    /// </summary>
    public class DistribucionCalculoInteresxFondoBO
    {
        // OT10675 INI
        private double totalDis;
        private double diferencia;
        private DataTable dtDistribucionFIR = new DataTable();
        private DataTable dtDistribucionFlujo = new DataTable(); // OT10592 

        public double TotalDis
        {
            get
            {
                return totalDis;
            }
        }

        public double Diferencia
        {
            get
            {
                return diferencia;
            }
        }


        public DataTable DtDistribucionFir
        {
            get
            {
                return dtDistribucionFIR;
            }
        }
        // OT10675 FIN

    


        public DistribucionCalculoInteresxFondoBO()
        {
            //
            // TODO: agregar aquí la lógica del constructor
            //
        }

        public DataTable obtenerFondos()
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
            DataTable fondo = da.ObtenerFondosPrecierre();
            return fondo;
        }

        public DataTable buscarPrecierreXFecha(int idFondo, string fechaCorte)
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
            return da.buscarPrecierreXFecha(idFondo, fechaCorte);
        }

        public DataTable ObtenerTablaGeneral(string codigoTabla, string llaveTabla)
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();

            return da.ObtenerTablaGeneral(codigoTabla, llaveTabla);
        }

        public DataTable listarOrdenesPreOrdenesInversion(int fechaInicio, int fechaFin, int idFondo)
        {
            ConsultaInversionesDA da = new ConsultaInversionesDA();

            try
            {
                DataTable dtOrdenesPreOrdenesInversion = new DataTable();
                dtOrdenesPreOrdenesInversion = da.listarOrdenesPreOrdenesInversion(fechaInicio, fechaFin, idFondo);
                return dtOrdenesPreOrdenesInversion;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RegistarMontoDistribucion(int idFondo,double total, double montoSobrante, int idDistribucion, string codigoUsuario)
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            SqlConnection cn = da.GetConnection2();

            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();

            try
            {
                da.RegistarMontoDistribucion(idFondo, total, montoSobrante, idDistribucion, codigoUsuario, cn, trans);

                trans.Commit();

            }
            catch (Exception x2)
            {
                trans.Rollback();
                throw x2;
            }
            finally
            {
                trans.Dispose();
                cn.Close();

            }

        }

        // OT10478 PSC001 INI
        public void registarRentabilidad(int idFondo, string mnemonico, string moneda, string intermediario, string fechaPago ,double interes)
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosTributacion;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            SqlConnection cn = da.GetConnection2();

            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();

            try
            {
                da.registarRentabilidad(idFondo, mnemonico, moneda, intermediario, fechaPago, interes, cn, trans);
                trans.Commit();

            }
            catch (Exception x2)
            {
                trans.Rollback();
                throw x2;
            }
            finally
            {
                trans.Dispose();
                cn.Close();

            }

        }
        // OT10478 PSC001 FIN

        //OT10675 INI
        public DataSet GenerarExcel(DataTable dtOrdenesPreOrdenesInversion, double sobrante, double totalDistribucion, int periodo, int idFondo)
        {
            DataRow[] drOrdenesPreOrdenesInversion = dtOrdenesPreOrdenesInversion.Select("CodigoOperacion in ('3','35')");

            DataTable dtConstitucionTitulosUnicos = new DataTable();
            dtConstitucionTitulosUnicos.Columns.Add("FECHA");
            dtConstitucionTitulosUnicos.Columns.Add("ORDEN");
            dtConstitucionTitulosUnicos.Columns.Add("LIQUIDACION");
            dtConstitucionTitulosUnicos.Columns.Add("FIN_CONTRATO");
            dtConstitucionTitulosUnicos.Columns.Add("MONEDA");
            dtConstitucionTitulosUnicos.Columns.Add("MNEMONICO");
            dtConstitucionTitulosUnicos.Columns.Add("OPERACION");
            dtConstitucionTitulosUnicos.Columns.Add("TASA");
            dtConstitucionTitulosUnicos.Columns.Add("TIPO_CAMBIO");
            dtConstitucionTitulosUnicos.Columns.Add("PRECIO");
            dtConstitucionTitulosUnicos.Columns.Add("MONTO");
            dtConstitucionTitulosUnicos.Columns.Add("CANTIDAD");
            dtConstitucionTitulosUnicos.Columns.Add("INTERMEDIARIO");
            dtConstitucionTitulosUnicos.Columns.Add("ESTADO");
            dtConstitucionTitulosUnicos.Columns.Add("DIAS");
            dtConstitucionTitulosUnicos.Columns.Add("FACTOR");
            dtConstitucionTitulosUnicos.Columns.Add("MONTO_FINAL");
            dtConstitucionTitulosUnicos.Columns.Add("INTERES_DPZ");
            dtConstitucionTitulosUnicos.Columns.Add("INTERES_CUPONES");
            dtConstitucionTitulosUnicos.Columns.Add("DISTRIBUCION_FLUJOS");
            dtConstitucionTitulosUnicos.Columns.Add("SALDO_PENDIENTE");

            DataTable dtCobroVcmtoInteres = new DataTable();
            dtCobroVcmtoInteres = dtConstitucionTitulosUnicos.Clone();
            //OT10592 INI
            dtDistribucionFlujo = dtConstitucionTitulosUnicos.Clone();

            DataColumn Id = new DataColumn();
            Id.DataType = System.Type.GetType("System.Int32");
            Id.AutoIncrement = true;
            Id.AutoIncrementSeed = 1;
            Id.AutoIncrementStep = 1;
            Id.ColumnName = "ID";
            dtDistribucionFlujo.Columns.Add(Id);

            //Recupero la lista del periodo anterior

            int periodoAnterior;
            periodoAnterior = periodo - 1;

            DataTable dtSobrantePeriodoAnterior = new DataTable();
            dtSobrantePeriodoAnterior = listarPagoFlujosSobranteXPeriodo(periodoAnterior, idFondo);

            foreach (DataRow dr in dtSobrantePeriodoAnterior.Rows)
            {
                dtDistribucionFlujo.ImportRow(dr);
            }

            //OT10592 FIN

            try
            {
                for (int c = 0; c < drOrdenesPreOrdenesInversion.Length; c++)
                {
                    string idCodigoConstitucion = "35";
                    DateTime fechaInicial = Convert.ToDateTime(drOrdenesPreOrdenesInversion[c]["FechaOperacion"].ToString());
                    DateTime fechaFinal = Convert.ToDateTime(drOrdenesPreOrdenesInversion[c]["FechaContrato"].ToString());

                    double cantidad = 0;
                    if (drOrdenesPreOrdenesInversion[c]["CantidadOperacion"] != null && drOrdenesPreOrdenesInversion[c]["CantidadOperacion"] != DBNull.Value)
                    {
                        cantidad = Convert.ToDouble(drOrdenesPreOrdenesInversion[c]["CantidadOperacion"].ToString());
                    }

                    int dias = Convert.ToInt32((fechaFinal.Subtract(fechaInicial)).TotalDays);
                    double tasa = Convert.ToDouble(drOrdenesPreOrdenesInversion[c]["Tasa"].ToString());
                    decimal tipoCambio = Decimal.Parse(drOrdenesPreOrdenesInversion[c]["TipoCambio"].ToString());
                    double monto = Convert.ToDouble(drOrdenesPreOrdenesInversion[c]["MontoNetoOperacion"].ToString());
                    double factor = System.Math.Round(System.Math.Pow((1 + tasa / 100), (dias / 360.00)), 10);
                    double montoFinal = System.Math.Round((monto * factor), 2);
                    string descOperacion = drOrdenesPreOrdenesInversion[c]["DescOperacion"].ToString() + '/' + drOrdenesPreOrdenesInversion[c]["TipoInstrumento"].ToString();
                    double interesDPZ = System.Math.Round((montoFinal - monto), 2);

                    if (Convert.ToString(drOrdenesPreOrdenesInversion[c]["CodigoOperacion"]) == idCodigoConstitucion)
                    {
                        DataRow drCVI = dtCobroVcmtoInteres.NewRow();
                        drCVI["FECHA"] = drOrdenesPreOrdenesInversion[c]["FechaOperacion"].ToString();
                        drCVI["ORDEN"] = drOrdenesPreOrdenesInversion[c]["CodigoOrden"].ToString();
                        drCVI["LIQUIDACION"] = drOrdenesPreOrdenesInversion[c]["FechaLiquidacion"].ToString();
                        drCVI["FIN_CONTRATO"] = drOrdenesPreOrdenesInversion[c]["FechaContrato"].ToString();
                        drCVI["MONEDA"] = drOrdenesPreOrdenesInversion[c]["Moneda"].ToString();
                        //drCVI["MNEMONICO"] = drOrdenesPreOrdenesInversion[c]["CodigoMnemonico"].ToString();
                        drCVI["MNEMONICO"] = drOrdenesPreOrdenesInversion[c]["EquivalenciaNemonico"].ToString(); //OT10592
                        drCVI["OPERACION"] = descOperacion;
                        drCVI["TASA"] = tasa;
                        drCVI["TIPO_CAMBIO"] = tipoCambio.ToString("0.00");
                        drCVI["PRECIO"] = drOrdenesPreOrdenesInversion[c]["Precio"].ToString();
                        drCVI["MONTO"] = monto.ToString("###,###,##0.00");
                        if (cantidad != 0)
                        {
                            drCVI["CANTIDAD"] = cantidad.ToString("###,###,##0.00");
                        }
                        else
                        {
                            drCVI["CANTIDAD"] = "";
                        }
                        drCVI["INTERMEDIARIO"] = drOrdenesPreOrdenesInversion[c]["Intermediario"].ToString();
                        drCVI["ESTADO"] = drOrdenesPreOrdenesInversion[c]["Estado"].ToString();

                        drCVI["DIAS"] = (fechaFinal.Subtract(fechaInicial)).TotalDays;
                        drCVI["FACTOR"] = factor.ToString("0.0000000000");
                        drCVI["MONTO_FINAL"] = montoFinal.ToString("###,###,##0.00");
                        drCVI["INTERES_DPZ"] = 0;
                        drCVI["INTERES_CUPONES"] = montoFinal.ToString("###,###,##0.00");
                        drCVI["DISTRIBUCION_FLUJOS"] = montoFinal.ToString("###,###,##0.00");

                        if (dtCobroVcmtoInteres.Rows.Count == 0)
                        {
                            drCVI["SALDO_PENDIENTE"] = sobrante.ToString("0.00");
                        }
                        dtCobroVcmtoInteres.Rows.Add(drCVI);

                        dtDistribucionFlujo.ImportRow(drCVI);// OT10592
                        
                    }

                    else
                    {
                        DataRow drCT = dtConstitucionTitulosUnicos.NewRow();
                        drCT["FECHA"] = drOrdenesPreOrdenesInversion[c]["FechaOperacion"].ToString();
                        drCT["ORDEN"] = drOrdenesPreOrdenesInversion[c]["CodigoOrden"].ToString();
                        drCT["LIQUIDACION"] = drOrdenesPreOrdenesInversion[c]["FechaLiquidacion"].ToString();
                        drCT["FIN_CONTRATO"] = drOrdenesPreOrdenesInversion[c]["FechaContrato"].ToString();
                        drCT["MONEDA"] = drOrdenesPreOrdenesInversion[c]["Moneda"].ToString();
                        //drCT["MNEMONICO"] = drOrdenesPreOrdenesInversion[c]["CodigoMnemonico"].ToString();
                        drCT["MNEMONICO"] = drOrdenesPreOrdenesInversion[c]["EquivalenciaNemonico"].ToString(); //OT10592
                        drCT["OPERACION"] = descOperacion;
                        drCT["TASA"] = tasa;
                        drCT["TIPO_CAMBIO"] = tipoCambio.ToString("0.00");
                        drCT["PRECIO"] = drOrdenesPreOrdenesInversion[c]["Precio"].ToString();
                        drCT["MONTO"] = monto.ToString("###,###,##0.00");
                        if (cantidad != 0)
                        {
                            drCT["CANTIDAD"] = cantidad.ToString("###,###,##0.00");
                        }
                        else
                        {
                            drCT["CANTIDAD"] = "";
                        }
                        drCT["INTERMEDIARIO"] = drOrdenesPreOrdenesInversion[c]["Intermediario"].ToString();
                        drCT["ESTADO"] = drOrdenesPreOrdenesInversion[c]["Estado"].ToString();
                        drCT["DIAS"] = (fechaFinal.Subtract(fechaInicial)).TotalDays;
                        drCT["FACTOR"] = factor.ToString("0.0000000000");
                        drCT["MONTO_FINAL"] = montoFinal.ToString("###,###,##0.00");
                        drCT["INTERES_DPZ"] = interesDPZ;
                        drCT["INTERES_CUPONES"] = 0;
                        drCT["DISTRIBUCION_FLUJOS"] = interesDPZ;
                        //dr["SALDO_PENDIENTE"] = drOrdenesPreOrdenesInversion["FechaOperacion"].ToString();

                        dtConstitucionTitulosUnicos.Rows.Add(drCT);
                        dtDistribucionFlujo.ImportRow(drCT);// OT10592
                    }
                }

                DataSet dtDistribucion = new DataSet();
                dtDistribucion.Tables.Add(dtCobroVcmtoInteres);
                dtDistribucion.Tables.Add(dtConstitucionTitulosUnicos);
                dtDistribucion.Tables.Add(dtDistribucionFlujo);//OT10592

                dtDistribucion.Tables[0].TableName = "dtCobroVcmtoInteres";
                dtDistribucion.Tables[1].TableName = "dtConstitucionTitulosUnicos";
                dtDistribucion.Tables[2].TableName = "dtDistribucionFlujo";//OT10592

                return dtDistribucion;

            }
            catch (Exception x)
            {
                throw x;
            }
        }      

        public void GenerarExcelOrdenesPreOrdenes(DataTable dtCobroVcmtoInteres, DataTable dtConstitucionTitulosUnicos, double sobrante, double totalDistribucion, string ruta, DateTime fechaInicio, DateTime fechaFin, DateTime fechaCorte, string codigoUsuario, int idFondo,int periodo)
        {

            ////OT10592 INI
            DataTable dtSobranteCobroVcmtoInteres = new DataTable();
            dtSobranteCobroVcmtoInteres = dtCobroVcmtoInteres.Clone();

            DataTable dtSobranteConstitucionTitulosUnicos = new DataTable();
            dtSobranteConstitucionTitulosUnicos = dtConstitucionTitulosUnicos.Clone();

            int periodoAnterior;
            periodoAnterior = periodo - 1;

            DataTable dtSobrantePeriodoAnterior = new DataTable();
            dtSobrantePeriodoAnterior = listarPagoFlujosSobranteXPeriodo(periodoAnterior, idFondo);
            //OT10592 FIN
          
            double total;
            //double diferencia;
            string archivoDistribucion;
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path += @"\Procesos\Plantillas\";
            string plantilla = path + "RptDistribucionFlujos";

            archivoDistribucion = ruta + string.Format(@"\RptDistribucionFlujos_{0}{1}", (fechaCorte.ToString("MMyyyy")), System.DateTime.Now.ToString("HHmmss"));

            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-PE");

            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

            ExcelApplication excelApplication = new ExcelApplication();
            ExcelWorkBook wb = excelApplication.OpenWorkBook(plantilla, ExcelMode.Full);
            try
            {

                ExcelWorkSheet sheet = excelApplication.GetWorkBook(1).GetSheet(1);
                sheet.SetValue("J2", string.Format("FECHA: {0}", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                sheet.SetValue("J3", string.Format("USUARIO: {0}", codigoUsuario));
                sheet.SetValue("B8", string.Format(sheet.GetString("B8"), fechaInicio.ToString("dd/MM/yyyy"), fechaFin.ToString("dd/MM/yyyy"), fechaCorte.ToString("dd/MM/yyyy")));

                //DataTable dtPagoFlujo = rptPreliminarDistribucionPagoFlujoDA.ObtenerPagoFlujo(Convert.ToInt32(codigoFondo), fechaInicio);
                int fila;
                int filaConstitucion;//OT10592
                int filasCobros;
                int filasConstitucionTotal;
                int filasCobroTotal;
                int filasTotal;
                int filasTotalDistribucion;
                int filasDiferencia;
                string totalDPZ;
                string totalBONO;
                string textoTotal;
                string textoTotalDistribucion;
                string textoDiferencia;
                double totalConstitucion;
                double totalCobro;


                fila = 13;
                totalDPZ = "TOTAL INTERES DPZ";
                totalBONO = "TOTAL INTERES BONO";
                textoTotal = "TOTAL";
                textoTotalDistribucion = "TOTAL A DISTRIBUIR" + " " + fechaCorte.ToString("dd/MM/yyyy");
                textoDiferencia = "DIFERENCIA";

                filaConstitucion = fila + dtSobrantePeriodoAnterior.Rows.Count;//OT10592

                filasConstitucionTotal = filaConstitucion + dtConstitucionTitulosUnicos.Rows.Count;
                filasCobros = filasConstitucionTotal + 1;
                filasCobroTotal = filasCobros + dtCobroVcmtoInteres.Rows.Count;
                filasTotal = filasCobroTotal + 1;
                filasTotalDistribucion = filasTotal + 2;
                filasDiferencia = filasTotalDistribucion + 1;

                totalConstitucion = 0;
                totalCobro = 0;

                DataTable dtTotalConsitucion = new DataTable();
                dtTotalConsitucion.Columns.Add("DESCRIPCION");
                dtTotalConsitucion.Columns.Add("TOTAL");
                DataTable dtTotalCobro = dtTotalConsitucion.Clone();
                DataTable dtTotal = dtTotalConsitucion.Clone();
                DataTable dtTotalDistribucion = dtTotalConsitucion.Clone();
                DataTable dtDiferencia = dtTotalConsitucion.Clone();

                foreach (DataRow dr in dtConstitucionTitulosUnicos.Rows)
                {
                    totalConstitucion += Convert.ToDouble(dr["DISTRIBUCION_FLUJOS"]);
                }
                DataRow drTC = dtTotalConsitucion.NewRow();
                drTC["DESCRIPCION"] = totalDPZ;
                drTC["TOTAL"] = totalConstitucion.ToString("###,###,##0.00");
                dtTotalConsitucion.Rows.Add(drTC);


                foreach (DataRow dr in dtCobroVcmtoInteres.Rows)
                {
                    totalCobro += Convert.ToDouble(dr["DISTRIBUCION_FLUJOS"]);
                }
                DataRow drTCobro = dtTotalCobro.NewRow();
                drTCobro["DESCRIPCION"] = totalBONO;
                drTCobro["TOTAL"] = totalCobro.ToString("###,###,##0.00");
                dtTotalCobro.Rows.Add(drTCobro);

                totalDis = totalConstitucion + totalCobro + sobrante;
                total = Math.Round(totalDis, 2);

                DataRow drTotal = dtTotal.NewRow();
                drTotal["DESCRIPCION"] = textoTotal;
                drTotal["TOTAL"] = total.ToString("###,###,##0.00");
                dtTotal.Rows.Add(drTotal);

                DataRow drTotalDistribucion = dtTotalDistribucion.NewRow();
                drTotalDistribucion["DESCRIPCION"] = textoTotalDistribucion;
                drTotalDistribucion["TOTAL"] = totalDistribucion.ToString("###,###,##0.00");
                dtTotalDistribucion.Rows.Add(drTotalDistribucion);

                diferencia = Math.Round((totalDistribucion - total),2);
                DataRow drDiferencia = dtDiferencia.NewRow();
                drDiferencia["DESCRIPCION"] = textoDiferencia;
                drDiferencia["TOTAL"] = diferencia.ToString("###,###,##0.00");
                dtDiferencia.Rows.Add(drDiferencia);

                //OT10592 INI
                double diferenciaCalculada;
                diferenciaCalculada = diferencia * -1 ;
                double saldo;
                double saldoPendiente;
                double sobranteResta;
                bool sobranteDistribucion;

                    foreach (DataRow drCobroVcmtoInteres in dtCobroVcmtoInteres.Rows)
                    {
                        DataRow drSobranteCobro = dtSobranteCobroVcmtoInteres.NewRow();
                        drSobranteCobro["FECHA"] = drCobroVcmtoInteres["FECHA"].ToString();
                        drSobranteCobro["ORDEN"] = drCobroVcmtoInteres["ORDEN"].ToString();
                        drSobranteCobro["LIQUIDACION"] = drCobroVcmtoInteres["LIQUIDACION"].ToString();
                        drSobranteCobro["FIN_CONTRATO"] = drCobroVcmtoInteres["FIN_CONTRATO"].ToString();
                        drSobranteCobro["MONEDA"] = drCobroVcmtoInteres["MONEDA"].ToString();
                        drSobranteCobro["MNEMONICO"] = drCobroVcmtoInteres["MNEMONICO"].ToString();
                        drSobranteCobro["OPERACION"] = drCobroVcmtoInteres["OPERACION"].ToString();
                        drSobranteCobro["TASA"] = drCobroVcmtoInteres["TASA"].ToString();
                        drSobranteCobro["TIPO_CAMBIO"] = drCobroVcmtoInteres["TIPO_CAMBIO"].ToString();
                        drSobranteCobro["PRECIO"] = drCobroVcmtoInteres["PRECIO"].ToString();
                        drSobranteCobro["MONTO"] = drCobroVcmtoInteres["MONTO"].ToString();
                        drSobranteCobro["CANTIDAD"] = drCobroVcmtoInteres["CANTIDAD"].ToString();
                        drSobranteCobro["INTERMEDIARIO"] = drCobroVcmtoInteres["INTERMEDIARIO"].ToString();
                        drSobranteCobro["ESTADO"] = drCobroVcmtoInteres["ESTADO"].ToString();
                        drSobranteCobro["DIAS"] = drCobroVcmtoInteres["DIAS"].ToString();
                        drSobranteCobro["FACTOR"] = drCobroVcmtoInteres["FACTOR"].ToString();
                        drSobranteCobro["MONTO_FINAL"] = drCobroVcmtoInteres["MONTO_FINAL"].ToString();
                        drSobranteCobro["INTERES_DPZ"] = drCobroVcmtoInteres["INTERES_DPZ"].ToString();
                        drSobranteCobro["INTERES_CUPONES"] = drCobroVcmtoInteres["INTERES_CUPONES"].ToString();

                        //Se debe de restar el monto sobrante hasta que llegue a 0
                        sobranteDistribucion = false;

                        if (diferenciaCalculada > 0)
                        {
                            saldo = Convert.ToDouble(drCobroVcmtoInteres["DISTRIBUCION_FLUJOS"].ToString());
                            saldoPendiente = 0;
                            sobranteResta = saldo - diferenciaCalculada;
                           

                            if (sobranteResta > 0)
                            {
                                saldoPendiente = diferenciaCalculada;
                                sobranteDistribucion = true;
                            }
                            else
                            {
                                saldoPendiente = 0;
                                diferenciaCalculada = sobranteResta;
                            }
                            drSobranteCobro["DISTRIBUCION_FLUJOS"] = saldoPendiente;

                            if (sobranteDistribucion == true)
                            {
                                dtSobranteCobroVcmtoInteres.Rows.Add(drSobranteCobro);
                            }
                        } 
                    }

                    foreach (DataRow drConstitucionTitulosUnicos in dtConstitucionTitulosUnicos.Rows)
                    {
                        DataRow drConstitucion = dtSobranteConstitucionTitulosUnicos.NewRow();
                        drConstitucion["FECHA"] = drConstitucionTitulosUnicos["FECHA"].ToString();
                        drConstitucion["ORDEN"] = drConstitucionTitulosUnicos["ORDEN"].ToString();
                        drConstitucion["LIQUIDACION"] = drConstitucionTitulosUnicos["LIQUIDACION"].ToString();
                        drConstitucion["FIN_CONTRATO"] = drConstitucionTitulosUnicos["FIN_CONTRATO"].ToString();
                        drConstitucion["MONEDA"] = drConstitucionTitulosUnicos["MONEDA"].ToString();
                        drConstitucion["MNEMONICO"] = drConstitucionTitulosUnicos["MNEMONICO"].ToString(); 
                        drConstitucion["OPERACION"] = drConstitucionTitulosUnicos["OPERACION"].ToString();
                        drConstitucion["TASA"] = drConstitucionTitulosUnicos["TASA"].ToString();
                        drConstitucion["TIPO_CAMBIO"] = drConstitucionTitulosUnicos["TIPO_CAMBIO"].ToString();
                        drConstitucion["PRECIO"] = drConstitucionTitulosUnicos["PRECIO"].ToString();
                        drConstitucion["MONTO"] = drConstitucionTitulosUnicos["MONTO"].ToString();
                        drConstitucion["CANTIDAD"] = drConstitucionTitulosUnicos["CANTIDAD"].ToString();
                        drConstitucion["INTERMEDIARIO"] = drConstitucionTitulosUnicos["INTERMEDIARIO"].ToString();
                        drConstitucion["ESTADO"] = drConstitucionTitulosUnicos["ESTADO"].ToString();
                        drConstitucion["DIAS"] = drConstitucionTitulosUnicos["DIAS"].ToString();
                        drConstitucion["FACTOR"] = drConstitucionTitulosUnicos["FACTOR"].ToString();
                        drConstitucion["MONTO_FINAL"] = drConstitucionTitulosUnicos["MONTO_FINAL"].ToString();
                        drConstitucion["INTERES_DPZ"] = drConstitucionTitulosUnicos["INTERES_DPZ"].ToString();
                        drConstitucion["INTERES_CUPONES"] = drConstitucionTitulosUnicos["INTERES_CUPONES"].ToString();

                        //Se debe de restar el monto sobrante hasta que llegue a 0
                        sobranteDistribucion = false;
                        if (diferenciaCalculada > 0)
                        {
                            saldo = Convert.ToDouble(drConstitucionTitulosUnicos["DISTRIBUCION_FLUJOS"].ToString());
                            saldoPendiente = 0;
                            sobranteResta = saldo - diferenciaCalculada;

                            if (sobranteResta > 0)
                            {
                                saldoPendiente = diferenciaCalculada;
                                sobranteDistribucion = true;
                            }
                            else
                            {
                                saldoPendiente = 0;
                                diferenciaCalculada = sobranteResta;
                            }
                            drConstitucion["DISTRIBUCION_FLUJOS"] = saldoPendiente;
                        }
                        drConstitucion["SALDO_PENDIENTE"] = drConstitucionTitulosUnicos["SALDO_PENDIENTE"].ToString();
                        if (sobranteDistribucion == true)
                        {
                            dtSobranteConstitucionTitulosUnicos.Rows.Add(drConstitucion);
                        }

                    }


                sheet.EscribirDatatable("B", fila, dtSobrantePeriodoAnterior);
                //OT10592 FIN      
                sheet.EscribirDatatable("B", filaConstitucion, dtConstitucionTitulosUnicos);
                sheet.EscribirDatatable("T", filasConstitucionTotal, dtTotalConsitucion);
                sheet.EscribirDatatable("B", filasCobros, dtCobroVcmtoInteres);
                sheet.EscribirDatatable("T", filasCobroTotal, dtTotalCobro);
                sheet.EscribirDatatable("T", filasTotal, dtTotal);
                sheet.EscribirDatatable("T", filasTotalDistribucion, dtTotalDistribucion);
                sheet.EscribirDatatable("T", filasDiferencia, dtDiferencia);

                wb.SaveAs(archivoDistribucion);
            }


            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                wb.Close();
                excelApplication.Close();
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        //OT10675 FIN


        // OT10604 INI 
        //public DataSet ObtenerDistribucionFondosFir(DateTime fechaInicio, DateTime fechaFin, double valorRazonable)
        public DataSet ObtenerDistribucionFondosFir(DateTime fechaInicio, DateTime fechaFin, double valorRazonable, double patrimonioContable)//OT10944
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
            //OT10944 INI
            //DataSet dsDistribucionFondosFir = da.ObtenerDistribucionFondosFir(fechaInicio, fechaFin, valorRazonable);
            DataSet dsDistribucionFondosFir = da.ObtenerDistribucionFondosFir(fechaInicio, fechaFin, valorRazonable, patrimonioContable);
            //OT10944 FIN
            return dsDistribucionFondosFir;
        }

        public void registarDistribucionFondosFIR(int idFondo, decimal monto, decimal valuacion, decimal adicion, decimal deduccion, string codigoUsuario, string fechaCorte, string fechaPago)
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            SqlConnection cn = da.GetConnection2();

            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();

            try
            {
                da.registarDistribucionFondosFIR(idFondo, monto, valuacion, adicion, deduccion, codigoUsuario, cn, trans,fechaCorte,fechaPago);

                trans.Commit();

            }
            catch (Exception x2)
            {
                trans.Rollback();
                throw x2;
            }
            finally
            {
                trans.Dispose();
                cn.Close();

            }

        }


        public DataTable ValidarRentabilidad(DateTime fecha, string idFondo)
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosTributacion;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            SqlConnection cn = da.GetConnection2();

            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();

            try
            {
                return da.ValidarRentabilidad(fecha, idFondo, cn, trans);
                //trans.Commit();
            }
            catch (Exception xxa)
            {
                trans.Rollback();
                throw xxa;
            }
            finally
            {
                trans.Dispose();
                cn.Close();
            }

        }


        public void EliminarRentabilidad(DateTime fecha, string idFondo)
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosTributacion;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            SqlConnection cn = da.GetConnection2();

            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();

            DistribucionXPeriodo dxp = new DistribucionXPeriodo();

            try
            {

                da.EliminarRentabilidad(fecha, idFondo, cn, trans);
                trans.Commit();

            }
            catch (Exception x3)
            {
                trans.Rollback();
                throw x3;
            }
            finally
            {
                trans.Dispose();
                cn.Close();

            }

        }

        public DataTable ObtenerFondoMidas(string codigoTabla)
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosTributacion;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            SqlConnection cn = da.GetConnection2();

            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();

            try
            {
                return da.ObtenerFondoMidas(codigoTabla, cn, trans);
                trans.Commit();
            }
            catch (Exception xxa)
            {
                trans.Rollback();
                throw xxa;
            }
            finally
            {
                trans.Dispose();
                cn.Close();
            }

        }
        //OT10944 INI
        //public void GenerarExcelDistribucionFondosFIR(DataTable dtFondosFirInicio, DataTable dtFondosFirFin, decimal valuacion, decimal adicion, decimal deduccion, string ruta, string codigoUsuario, DateTime fechaInicio, DateTime fechaFin)
        public void GenerarExcelDistribucionFondosFIR(DataTable dtFondosFirFin, decimal valuacion, decimal adicion, decimal deduccion, string ruta, string codigoUsuario, DateTime fechaInicio, DateTime fechaFin, DateTime fechaCorte)
        //OT10944 FIN
       {
            string archivoDistribucion;
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path += @"\Procesos\Plantillas\";
            string plantilla = path + "RptDistribucionFlujosFIR";

            archivoDistribucion = ruta + string.Format(@"\RptDistribucionFlujosFIR_{0}{1}", System.DateTime.Now.ToString("ddMMyyyy"), System.DateTime.Now.ToString("HHmmss"));

            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-PE");

            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");


            ExcelApplication excelApplication = new ExcelApplication();
            ExcelWorkBook wb = excelApplication.OpenWorkBook(plantilla, ExcelMode.Full);
            try
            {

                ExcelWorkSheet sheet = excelApplication.GetWorkBook(1).GetSheet(1);
                sheet.SetValue("J2", string.Format("FECHA: {0}", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                sheet.SetValue("J3", string.Format("USUARIO: {0}", codigoUsuario));
                sheet.SetValue("B6", string.Format(sheet.GetString("B6"), fechaInicio.ToString("dd/MM/yyyy"), fechaFin.ToString("dd/MM/yyyy")));
                sheet.SetValue("C8", string.Format(sheet.GetString("C8"), fechaCorte.ToString("dd/MM/yyyy")));//OT10944


                //int filaFondosFirInicio;OT10944
                int filaFondosFirFin;
                int filaResultado;
                //int filaPorcentajeDistribucion;OT10944
                int filasAdicion;
                int filasDeduccion;
                int filasTotal;
                int filasDatosDistribucion;
                int filaRentabilidad;//OT10944

                //filaFondosFirInicio = 11;OT10944
                filaFondosFirFin = 11;
                filaRentabilidad = 17;//OT10944
                filaResultado = 19;
                //filaPorcentajeDistribucion = 21;//OT10944
                filasAdicion = 21;
                filasDeduccion = 23;
                filasTotal = 25;
                filasDatosDistribucion = 30;
                //OT10944 INI
                //decimal capitalInicialSerieA = 0;
                //decimal capitalInicialSerieB = 0;
                //decimal capitalInicialSerieC = 0;
                //decimal capitalFinalSerieA = 0;
                //decimal capitalFinalSerieB = 0;
                //decimal capitalFinalSerieC = 0;
                decimal resultadoSerieA = 0;
                decimal resultadoSerieB = 0;
                decimal resultadoSerieC = 0;
                decimal resultadoTotal = 0;
                //OT10944 INI
                //decimal porcentajeInicialSerieA = 0;
                //decimal porcentajeInicialSerieB = 0;
                //decimal porcentajeInicialSerieC = 0;              
                decimal rentabilidadSerieA = 0;
                decimal rentabilidadSerieB = 0;
                decimal rentabilidadSerieC = 0;
                decimal rentabilidadTotal = 0;
                decimal valorCuotaCorteSerieA = 0;
                decimal valorCuotaCorteSerieB = 0;
                decimal valorCuotaCorteSerieC = 0;
                decimal valorCuotaCalculadoSerieA = 0;
                decimal valorCuotaCalculadoSerieB = 0;
                decimal valorCuotaCalculadoSerieC = 0;
                decimal cuotasSerieA = 0;
                decimal cuotasSerieB = 0;
                decimal cuotasSerieC = 0;
                decimal porcentajeFinalSerieA = 0;
                decimal porcentajeFinalSerieB = 0;
                decimal porcentajeFinalSerieC = 0;   


                /*if (dtFondosFirInicio.Rows.Count > 0)
                {

                    porcentajeInicialSerieA = Math.Round(Convert.ToDecimal(dtFondosFirInicio.Rows[0]["SERIE_A"].ToString()), 4);
                    porcentajeInicialSerieB = Math.Round(Convert.ToDecimal(dtFondosFirInicio.Rows[0]["SERIE_B"].ToString()), 4);
                    porcentajeInicialSerieC = Math.Round(Convert.ToDecimal(dtFondosFirInicio.Rows[0]["SERIE_C"].ToString()), 4);
                    capitalInicialSerieA = Convert.ToDecimal(dtFondosFirInicio.Rows[3]["SERIE_A"].ToString());
                    capitalInicialSerieB = Convert.ToDecimal(dtFondosFirInicio.Rows[3]["SERIE_B"].ToString());
                    capitalInicialSerieC = Convert.ToDecimal(dtFondosFirInicio.Rows[3]["SERIE_C"].ToString());
                }*/
                //OT10944 FIN

                if (dtFondosFirFin.Rows.Count > 0)
                {
                    //OT10944 INI
                    //capitalFinalSerieA = Convert.ToDecimal(dtFondosFirFin.Rows[3]["SERIE_A"].ToString());
                    //capitalFinalSerieB = Convert.ToDecimal(dtFondosFirFin.Rows[3]["SERIE_B"].ToString());
                    //capitalFinalSerieC = Convert.ToDecimal(dtFondosFirFin.Rows[3]["SERIE_C"].ToString());
                    porcentajeFinalSerieA = Convert.ToDecimal(dtFondosFirFin.Rows[0]["SERIE_A"].ToString());
                    porcentajeFinalSerieB = Convert.ToDecimal(dtFondosFirFin.Rows[0]["SERIE_B"].ToString());
                    porcentajeFinalSerieC = Convert.ToDecimal(dtFondosFirFin.Rows[0]["SERIE_C"].ToString());
                    cuotasSerieA = Convert.ToDecimal(dtFondosFirFin.Rows[1]["SERIE_A"].ToString());
                    cuotasSerieB = Convert.ToDecimal(dtFondosFirFin.Rows[1]["SERIE_B"].ToString());
                    cuotasSerieC = Convert.ToDecimal(dtFondosFirFin.Rows[1]["SERIE_C"].ToString());
                    valorCuotaCorteSerieA = Convert.ToDecimal(dtFondosFirFin.Rows[2]["SERIE_A"].ToString());
                    valorCuotaCorteSerieB = Convert.ToDecimal(dtFondosFirFin.Rows[2]["SERIE_B"].ToString());
                    valorCuotaCorteSerieC = Convert.ToDecimal(dtFondosFirFin.Rows[2]["SERIE_C"].ToString());
                    valorCuotaCalculadoSerieA = Convert.ToDecimal(dtFondosFirFin.Rows[3]["SERIE_A"].ToString());
                    valorCuotaCalculadoSerieB = Convert.ToDecimal(dtFondosFirFin.Rows[3]["SERIE_B"].ToString());
                    valorCuotaCalculadoSerieC = Convert.ToDecimal(dtFondosFirFin.Rows[3]["SERIE_C"].ToString());
                    //OT10944 FIN
                }

                DataTable dtResultado = new DataTable();
                dtResultado.Columns.Add("SERIE_A");
                dtResultado.Columns.Add("SERIE_B");
                dtResultado.Columns.Add("SERIE_C");
                dtResultado.Columns.Add("TOTAL");

                //OT10944 INI
                rentabilidadSerieA = valorCuotaCorteSerieA - valorCuotaCalculadoSerieA;
                rentabilidadSerieB = valorCuotaCorteSerieB - valorCuotaCalculadoSerieB;
                rentabilidadSerieC = valorCuotaCorteSerieC - valorCuotaCalculadoSerieC;
                rentabilidadTotal = rentabilidadSerieA + rentabilidadSerieB + rentabilidadSerieC;

                DataTable dtRentabilidad = new DataTable();
                dtRentabilidad.Columns.Add("SERIE_A");
                dtRentabilidad.Columns.Add("SERIE_B");
                dtRentabilidad.Columns.Add("SERIE_C");
                dtRentabilidad.Columns.Add("TOTAL");

                DataRow drRentabilidad = dtRentabilidad.NewRow();
                drRentabilidad["SERIE_A"] = rentabilidadSerieA;
                drRentabilidad["SERIE_B"] = rentabilidadSerieB;
                drRentabilidad["SERIE_C"] = rentabilidadSerieC;
                drRentabilidad["TOTAL"] = rentabilidadTotal;
                dtRentabilidad.Rows.Add(drRentabilidad);
                //OT10944 FIN

                DataTable dtTotal = new DataTable();
                dtTotal = dtResultado.Clone();

                //OT10944 INI
                /*resultadoSerieA = capitalFinalSerieA - capitalInicialSerieA;
                resultadoSerieB = capitalFinalSerieB - capitalInicialSerieB;
                resultadoSerieC = capitalFinalSerieC - capitalInicialSerieC;
                resultadoTotal = resultadoSerieA + resultadoSerieB + resultadoSerieC;*/

                resultadoSerieA = cuotasSerieA * rentabilidadSerieA;
                resultadoSerieB = cuotasSerieB * rentabilidadSerieB;
                resultadoSerieC = cuotasSerieC * rentabilidadSerieC;
                resultadoTotal = resultadoSerieA + resultadoSerieB + resultadoSerieC;
                //OT10944 FIN

                DataRow drResultado = dtResultado.NewRow();
                drResultado["SERIE_A"] = resultadoSerieA;
                drResultado["SERIE_B"] = resultadoSerieB;
                drResultado["SERIE_C"] = resultadoSerieC;
                drResultado["TOTAL"] = resultadoTotal;
                dtResultado.Rows.Add(drResultado);

                //OT10944 INI
                //Recuperando los valores para el porcentaje de distribución
                //DataTable dtPorcentajeDistribucion = new DataTable();
                //dtPorcentajeDistribucion.Columns.Add("PORCENTAJE_SERIE_A");
                //dtPorcentajeDistribucion.Columns.Add("PORCENTAJE_SERIE_B");
                //dtPorcentajeDistribucion.Columns.Add("PORCENTAJE_SERIE_C");

                //DataRow dRPorcentajeDistribucion = dtPorcentajeDistribucion.NewRow();
                //dRPorcentajeDistribucion["PORCENTAJE_SERIE_A"] = resultadoSerieA / resultadoTotal;
                //dRPorcentajeDistribucion["PORCENTAJE_SERIE_B"] = resultadoSerieB / resultadoTotal;
                //dRPorcentajeDistribucion["PORCENTAJE_SERIE_C"] = resultadoSerieC / resultadoTotal;
                //dtPorcentajeDistribucion.Rows.Add(dRPorcentajeDistribucion);
                //OT10944 FIN

                //Recuperando los valores de las adiciones
                decimal adicionSerieA = 0;
                decimal adicionSerieB = 0;
                decimal adicionSerieC = 0;

                //OT10944 INI
                //adicionSerieA = porcentajeInicialSerieA * adicion;
                //adicionSerieB = porcentajeInicialSerieB * adicion;
                //adicionSerieC = porcentajeInicialSerieC * adicion;

                adicionSerieA = porcentajeFinalSerieA * adicion;
                adicionSerieB = porcentajeFinalSerieB * adicion;
                adicionSerieC = porcentajeFinalSerieC * adicion;
                //OT10944 FIN

                DataTable dtAdicion = new DataTable();
                dtAdicion.Columns.Add("SERIE_A");
                dtAdicion.Columns.Add("SERIE_B");
                dtAdicion.Columns.Add("SERIE_C");
                dtAdicion.Columns.Add("MONTO_INGRESANTE");

                DataTable dtDeduccion = new DataTable();
                dtDeduccion = dtAdicion.Clone();

                DataRow drAdicion = dtAdicion.NewRow();
                drAdicion["SERIE_A"] = adicionSerieA;
                drAdicion["SERIE_B"] = adicionSerieB;
                drAdicion["SERIE_C"] = adicionSerieC;
                drAdicion["MONTO_INGRESANTE"] = adicion;
                dtAdicion.Rows.Add(drAdicion);

                //Recuperando los valores de las deducciones

                decimal deduccionSerieA = 0;
                decimal deduccionSerieB = 0;
                decimal deduccionSerieC = 0;

                //OT10944 INI
                //deduccionSerieA = porcentajeInicialSerieA * deduccion;
                //deduccionSerieB = porcentajeInicialSerieB * deduccion;
                //deduccionSerieC = porcentajeInicialSerieC * deduccion;

                deduccionSerieA = porcentajeFinalSerieA * deduccion;
                deduccionSerieB = porcentajeFinalSerieB * deduccion;
                deduccionSerieC = porcentajeFinalSerieC * deduccion;
                //OT10944 FIN

                DataRow drDeduccion = dtDeduccion.NewRow();
                drDeduccion["SERIE_A"] = deduccionSerieA;
                drDeduccion["SERIE_B"] = deduccionSerieB;
                drDeduccion["SERIE_C"] = deduccionSerieC;
                drDeduccion["MONTO_INGRESANTE"] = deduccion;
                dtDeduccion.Rows.Add(drDeduccion);


                //Calculando el total

                decimal totalSerieA = 0;
                decimal totalSerieB = 0;
                decimal totalSerieC = 0;


                totalSerieA = resultadoSerieA + adicionSerieA - deduccionSerieA;
                totalSerieB = resultadoSerieB + adicionSerieB - deduccionSerieB;
                totalSerieC = resultadoSerieC + adicionSerieC - deduccionSerieC;

                DataRow drTotal = dtTotal.NewRow();
                drTotal["SERIE_A"] = totalSerieA;
                drTotal["SERIE_B"] = totalSerieB;
                drTotal["SERIE_C"] = totalSerieC;
                dtTotal.Rows.Add(drTotal);


                //Calculando los datos a distribuir

                DataTable dtDatosDistribucion = new DataTable();
                dtDatosDistribucion.Columns.Add("RESULTADOS");
                dtDatosDistribucion.Columns.Add("RENTAS");


                DataRow drSerieA = dtDatosDistribucion.NewRow();
                drSerieA["RESULTADOS"] = resultadoSerieA;
                drSerieA["RENTAS"] = totalSerieA;
                dtDatosDistribucion.Rows.Add(drSerieA);

                DataRow drSerieB = dtDatosDistribucion.NewRow();
                drSerieB["RESULTADOS"] = resultadoSerieB;
                drSerieB["RENTAS"] = totalSerieB;
                dtDatosDistribucion.Rows.Add(drSerieB);

                DataRow drSerieC = dtDatosDistribucion.NewRow();
                drSerieC["RESULTADOS"] = resultadoSerieC;
                drSerieC["RENTAS"] = totalSerieC;
                dtDatosDistribucion.Rows.Add(drSerieC);

                DataRow drDatosDistribucionTotal = dtDatosDistribucion.NewRow();
                drDatosDistribucionTotal["RESULTADOS"] = resultadoSerieA + resultadoSerieB + resultadoSerieC;
                drDatosDistribucionTotal["RENTAS"] = totalSerieA + totalSerieB + totalSerieC;
                dtDatosDistribucion.Rows.Add(drDatosDistribucionTotal);


                //Se crea un dataTable que contiene los registros a guardar 
                dtDistribucionFIR.Clear();

                if (dtDistribucionFIR.Columns.Count == 0)
                {
                    dtDistribucionFIR.Columns.Add("ID_FONDO");
                    dtDistribucionFIR.Columns.Add("MONTO");
                    dtDistribucionFIR.Columns.Add("VALUACION");
                    dtDistribucionFIR.Columns.Add("ADICIONES");
                    dtDistribucionFIR.Columns.Add("DEDUCCIONES");
                    dtDistribucionFIR.Columns.Add("RENTABILIDAD");//OT10944
                }


                DataRow drDistribucionSerieA = dtDistribucionFIR.NewRow();
                drDistribucionSerieA["ID_FONDO"] = 39;
                //OT10987 2018 INI
                //drDistribucionSerieA["MONTO"] = totalSerieA;
                drDistribucionSerieA["MONTO"] = resultadoSerieA;
                //OT10987 2018 FIN
                drDistribucionSerieA["VALUACION"] = valuacion;
                drDistribucionSerieA["ADICIONES"] = adicion;
                drDistribucionSerieA["DEDUCCIONES"] = deduccion;
                //OT10987 2018 INI
                //drDistribucionSerieA["RENTABILIDAD"] = rentabilidadSerieA;//OT10944
                drDistribucionSerieA["RENTABILIDAD"] = totalSerieA;
                //OT10987 2018 FIN
                dtDistribucionFIR.Rows.Add(drDistribucionSerieA);

                DataRow drDistribucionSerieB = dtDistribucionFIR.NewRow();
                drDistribucionSerieB["ID_FONDO"] = 40;
                //OT10987 2018 INI
                //drDistribucionSerieB["MONTO"] = totalSerieB;
                drDistribucionSerieB["MONTO"] = resultadoSerieB;
                //OT10987 2018 FIN
                drDistribucionSerieB["VALUACION"] = valuacion;
                drDistribucionSerieB["ADICIONES"] = adicion;
                drDistribucionSerieB["DEDUCCIONES"] = deduccion;
                //OT10987 2018 INI
                //drDistribucionSerieB["RENTABILIDAD"] = rentabilidadSerieB;//OT10944
                drDistribucionSerieB["RENTABILIDAD"] = totalSerieB;
                //OT10987 2018 FIN
                dtDistribucionFIR.Rows.Add(drDistribucionSerieB);

                DataRow drDistribucionSerieC = dtDistribucionFIR.NewRow();
                drDistribucionSerieC["ID_FONDO"] = 41;
                //OT10987 2018 INI
                //drDistribucionSerieC["MONTO"] = totalSerieC;
                drDistribucionSerieC["MONTO"] = resultadoSerieC;
                //OT10987 2018 FIN
                drDistribucionSerieC["VALUACION"] = valuacion;
                drDistribucionSerieC["ADICIONES"] = adicion;
                drDistribucionSerieC["DEDUCCIONES"] = deduccion;
                //OT10987 2018 INI
                //drDistribucionSerieC["RENTABILIDAD"] = rentabilidadSerieC;//OT10944
                drDistribucionSerieC["RENTABILIDAD"] = totalSerieC;
                //OT10987 2018 FIN
                dtDistribucionFIR.Rows.Add(drDistribucionSerieC);


                //sheet.EscribirDatatable("C", filaFondosFirInicio, dtFondosFirInicio);
                sheet.EscribirDatatable("C", filaFondosFirFin, dtFondosFirFin);
                sheet.EscribirDatatable("C", filaResultado, dtResultado);
               //sheet.EscribirDatatable("C", filaPorcentajeDistribucion, dtPorcentajeDistribucion);//OT10944
                sheet.EscribirDatatable("C", filasAdicion, dtAdicion);
                sheet.EscribirDatatable("C", filasDeduccion, dtDeduccion);
                sheet.EscribirDatatable("C", filasTotal, dtTotal);
                sheet.EscribirDatatable("D", filasDatosDistribucion, dtDatosDistribucion);
                sheet.EscribirDatatable("C", filaRentabilidad, dtRentabilidad);//OT10944

                wb.SaveAs(archivoDistribucion);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                wb.Close();
                excelApplication.Close();
                Thread.CurrentThread.CurrentCulture = originalCulture;

            }

        }
        // OT10604 FIN

        // OT10592 INI
        private DataTable listarPagoFlujosSobranteXPeriodo(int periodoAnterior, int idFondo)
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
            return da.listarPagoFlujosSobranteXPeriodo(periodoAnterior, idFondo);
        }

        public void registrarPagoFlujosSobrantes(DataTable dtDistribucionFlujoOrdenado, double diferencia, int idFondo, int periodo, string codigoUsuario)
        {
            DataTable dtSobranteLimitada = new DataTable();
            dtSobranteLimitada = dtDistribucionFlujoOrdenado.Clone();

            DataTable dtSobrantes = new DataTable();
            dtSobrantes = dtDistribucionFlujoOrdenado.Clone();
            bool sobranteDistribucion;
            double montoDistribucion;
            double restaDistribucion;
            double diferenciaDistribucion;
            restaDistribucion = 0;
            diferenciaDistribucion = diferencia * -1;

            try
            {
                foreach (DataRow dr in dtDistribucionFlujoOrdenado.Rows)
                {
                    sobranteDistribucion = false;
                    DataRow drSobranteLimitada = dtSobranteLimitada.NewRow();
                    drSobranteLimitada["FECHA"] = dr["FECHA"].ToString();
                    drSobranteLimitada["ORDEN"] = dr["ORDEN"].ToString();
                    drSobranteLimitada["LIQUIDACION"] = dr["LIQUIDACION"].ToString();
                    drSobranteLimitada["FIN_CONTRATO"] = dr["FIN_CONTRATO"].ToString();
                    drSobranteLimitada["MONEDA"] = dr["MONEDA"].ToString();
                    drSobranteLimitada["MNEMONICO"] = dr["MNEMONICO"].ToString(); 
                    drSobranteLimitada["OPERACION"] = dr["OPERACION"].ToString();
                    drSobranteLimitada["TASA"] = dr["TASA"] != DBNull.Value ? dr["TASA"] : 0;
                    drSobranteLimitada["TIPO_CAMBIO"] = dr["TIPO_CAMBIO"].ToString();
                    drSobranteLimitada["PRECIO"] = dr["PRECIO"].ToString() != "" ? dr["PRECIO"] : 0;
                    drSobranteLimitada["MONTO"] = dr["MONTO"].ToString();
                    drSobranteLimitada["CANTIDAD"] = dr["CANTIDAD"].ToString() != "" ? dr["CANTIDAD"] : 0; 
                    drSobranteLimitada["INTERMEDIARIO"] = dr["INTERMEDIARIO"].ToString();
                    drSobranteLimitada["ESTADO"] = dr["ESTADO"].ToString();
                    drSobranteLimitada["DIAS"] = dr["DIAS"].ToString();
                    drSobranteLimitada["FACTOR"] = dr["FACTOR"].ToString();
                    drSobranteLimitada["MONTO_FINAL"] = dr["MONTO_FINAL"].ToString();
                    drSobranteLimitada["INTERES_DPZ"] = dr["INTERES_DPZ"].ToString();
                    drSobranteLimitada["INTERES_CUPONES"] = dr["INTERES_CUPONES"].ToString();

                    //Se debe de restar el monto sobrante hasta que llegue a 0
   
                    montoDistribucion = Convert.ToDouble(dr["DISTRIBUCION_FLUJOS"].ToString());
                    if (diferenciaDistribucion > 0)
                    {
                        sobranteDistribucion = true;
                        restaDistribucion = Math.Round((montoDistribucion - diferenciaDistribucion),2);

                        if (restaDistribucion < 0)
                        {
                            drSobranteLimitada["DISTRIBUCION_FLUJOS"] = montoDistribucion;
                            diferenciaDistribucion = restaDistribucion * -1;

                        }
                        else
                        {
                            drSobranteLimitada["DISTRIBUCION_FLUJOS"] = diferenciaDistribucion;
                            diferenciaDistribucion = 0;
                        }
                    }

                    drSobranteLimitada["SALDO_PENDIENTE"] = 0;

                    if (sobranteDistribucion == true)
                    {
                        dtSobranteLimitada.Rows.Add(drSobranteLimitada);
                    }
                }


                if (dtSobranteLimitada.Rows.Count > 0)
                {
          
                    DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
                    da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
                    da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

                    SqlConnection cn = da.GetConnection2();
                    cn.Open();

                    SqlTransaction trans = cn.BeginTransaction();
                    

                    try
                    {
                        foreach (DataRow drSobranteLimitada in dtSobranteLimitada.Rows)
                        {
                            PagoFlujoSobranteTD sobrante = new PagoFlujoSobranteTD();
                            sobrante.IdFondo = idFondo;
                            sobrante.Periodo = periodo;
                            sobrante.Fecha = Convert.ToDateTime(drSobranteLimitada["FECHA"]);
                            sobrante.Orden = drSobranteLimitada["ORDEN"].ToString();
                            sobrante.Liquidacion = Convert.ToDateTime(drSobranteLimitada["LIQUIDACION"]);
                            sobrante.FinContrato = Convert.ToDateTime(drSobranteLimitada["FIN_CONTRATO"]);
                            sobrante.Moneda = drSobranteLimitada["MONEDA"].ToString();
                            sobrante.Mnemonico = drSobranteLimitada["MNEMONICO"].ToString();
                            sobrante.Operacion = drSobranteLimitada["OPERACION"].ToString();
                            sobrante.Tasa = Convert.ToDecimal(drSobranteLimitada["TASA"]);
                            sobrante.TipoCambio = Convert.ToDecimal(drSobranteLimitada["TIPO_CAMBIO"]);
                            sobrante.Precio = Convert.ToDecimal(drSobranteLimitada["PRECIO"]);
                            sobrante.Monto = Convert.ToDecimal(drSobranteLimitada["MONTO"]);
                            sobrante.Cantidad = Convert.ToDecimal(drSobranteLimitada["CANTIDAD"]);
                            sobrante.Intermediario = drSobranteLimitada["INTERMEDIARIO"].ToString();
                            sobrante.Estado = drSobranteLimitada["ESTADO"].ToString();
                            sobrante.Dias = Convert.ToInt32(drSobranteLimitada["DIAS"]);
                            sobrante.Factor = Convert.ToDecimal(drSobranteLimitada["FACTOR"]);
                            sobrante.MontoFinal = Convert.ToDecimal(drSobranteLimitada["MONTO_FINAL"]);
                            sobrante.InteresDPZ = Convert.ToDecimal(drSobranteLimitada["INTERES_DPZ"]);
                            sobrante.InteresCupones = Convert.ToDecimal(drSobranteLimitada["INTERES_CUPONES"]);
                            sobrante.DistribucionFlujo = Convert.ToDecimal(drSobranteLimitada["DISTRIBUCION_FLUJOS"]);
                            sobrante.SaldoPendiente = Convert.ToDecimal(drSobranteLimitada["SALDO_PENDIENTE"]);
                            sobrante.Usuario = codigoUsuario;
                            sobrante.Area = ConstantesING.AREA_PAGO_FLUJO_SOBRANTE;

                            da.registrarPagoFlujosSobrante(sobrante, cn, trans);
                            
                        }
                        trans.Commit();
                    }
                    catch (Exception xx3)
                    {
                        trans.Rollback();
                        throw xx3;
                    }
                    finally
                    {
                        trans.Dispose();
                        cn.Close();
                    }
                }              
            }
            catch(Exception xx)
            {
                throw xx;
            } 
        }
   

        public void registrarRentabilidadSobrantes(DataTable dtDistribucionFlujos, double diferencia, int idFondo, String fechaPago)
        {

            try
            {
                string mnemonico;
                string moneda;
                string intermediario;
                double interes;
                double diferenciaDistribucion;
                double restaDistribucion;
                bool sobranteDistribucion;
                double montoDistribucion;

                restaDistribucion = 0;
                interes = 0;
                diferenciaDistribucion = diferencia * -1;


                foreach(DataRow drDF in dtDistribucionFlujos.Rows)
                {
                    sobranteDistribucion = true;
                    mnemonico = drDF["MNEMONICO"].ToString();
                    moneda = drDF["MONEDA"].ToString();
                    intermediario = drDF["INTERMEDIARIO"].ToString();

                    montoDistribucion = Convert.ToDouble(drDF["DISTRIBUCION_FLUJOS"].ToString());
                    if (diferenciaDistribucion > 0)
                    {
                        restaDistribucion = montoDistribucion - diferenciaDistribucion;

                        if (restaDistribucion < 0)
                        {
                            sobranteDistribucion = false;
                            diferenciaDistribucion = restaDistribucion * -1;
                        }
                        else
                        {
                            interes = restaDistribucion;
                            diferenciaDistribucion = 0;
                        }
                    }
                    else
                    {
                        interes = montoDistribucion;
                    }

                    if (sobranteDistribucion == true)
                    {
                        registarRentabilidad(idFondo, mnemonico, moneda, intermediario, fechaPago, interes);
                    }
                }

                
            }
            catch (Exception x4)
            {
                throw x4;
            }
                
        }   

        public void registrarRentabilidadTotal(DataTable dtDistribucionFlujoOrdenado, int idFondo, string fechaPago)
        {
            try
            {
                string mnemonico;
                string moneda;
                string intermediario;
                double interes;

                foreach (DataRow drDF in dtDistribucionFlujoOrdenado.Rows)
                {
                    mnemonico = drDF["MNEMONICO"].ToString();
                    moneda = drDF["MONEDA"].ToString();
                    intermediario = drDF["INTERMEDIARIO"].ToString();
                    interes = Convert.ToDouble(drDF["DISTRIBUCION_FLUJOS"].ToString());   
                    registarRentabilidad(idFondo, mnemonico, moneda, intermediario, fechaPago, interes);
                }
            }
            catch (Exception x5)
            {
                throw x5;
            }
        }

        public void eliminarPagoFlujoSobrantes(int idFondo, int periodo)
        {
            DistribucionCalculoInteresxFondoDA da = new DistribucionCalculoInteresxFondoDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            SqlConnection cn = da.GetConnection2();
            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();


            try
            {
                da.eliminarPagoFlujoSobrantes(periodo,idFondo, cn, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                cn.Close();
            }
        }
        // OT10592 FIN
    }
}
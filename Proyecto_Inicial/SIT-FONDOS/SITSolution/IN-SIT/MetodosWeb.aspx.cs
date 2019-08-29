using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Sit.BusinessLayer;
using Sit.BusinessEntities;
//
using System.Data;
using System.Text;
//OT 10090 - 26/07/2017 - Carlos Espejo
//Descripcion: Clase para metodos WEB
public partial class MetodosWeb : System.Web.UI.Page
{
    public class GraficaDatos
    {
        public string portafolio;
        public string fechaOperacion;
        public string leyendaVariacion;
        public string imagen;
        public List<string> rangoX;
        public List<decimal> rangoY;
        public VariacionDatos variacionDatos;
    }

    public class VariacionDatos
    {
        public Decimal carteraPrecio;
        public Decimal carteraTipoCambio;
        public Decimal derivados;
        public Decimal cuentasPorCobrarTipoCambio;
        public Decimal cuentasPorPagarTipoCambio;
        public Decimal cuentasPorCobrarPrecio;
        public Decimal cuentasPorPagarPrecio;
        public Decimal cajaTipoCambio;
        public Decimal porcentageVariacionEstimado;
        public Decimal totalRentabilidadInversiones;
    }

    [WebMethod]
    public static GraficaDatos getRangos(string codigoPortafolio, string fechaOperacion, string serie, string tipoPeriodo)
    {
        GraficaDatos oResultado = new GraficaDatos();
        string imagen = "";
        string leyendaVariacion = "DIFERENCIA ({0}) = VARIACION ESTIMADA ({1}) - VARIACION SIT({2})";
        List<string> rangoX = new List<string>();
        List<decimal> rangoY = new List<decimal>();
        string portafolioNombre = "";
        ValorCuotaBM valorCuotaBM = new ValorCuotaBM();
        DataTable dtDatosGrafica = valorCuotaBM.Variacion_DatosGrafica(codigoPortafolio, serie, fechaOperacion, tipoPeriodo);
        DataTable dtVariacion = valorCuotaBM.Variacion_Obtener(codigoPortafolio, serie, UIUtility.ConvertirFechaaDecimal(fechaOperacion));
        if (dtDatosGrafica != null)
        {

            var listX = dtDatosGrafica.Rows.OfType<DataRow>().Select(dr => dr.Field<string>("RangoX")).ToList();
            var listY = dtDatosGrafica.Rows.OfType<DataRow>().Select(dr => dr.Field<decimal>("RangoY")).ToList();

            if (dtDatosGrafica.Rows.Count > 0)
            {
                portafolioNombre = dtDatosGrafica.Rows[0]["Portafolio"].ToString();
                rangoX = listX;
                rangoY = listY;
            }

        }

        if (dtVariacion.Rows.Count > 0)
        {
            ValorCuotaVariacionBE oValorCuotaVariacionBE = new ValorCuotaVariacionBE(dtVariacion.Rows[0]);
            VariacionDatos variacionDatos = new VariacionDatos();
            variacionDatos.carteraPrecio = oValorCuotaVariacionBE.CarteraPrecio;
            variacionDatos.carteraTipoCambio = oValorCuotaVariacionBE.CarteraTipoCambio;
            variacionDatos.derivados = oValorCuotaVariacionBE.Derivados;
            variacionDatos.cuentasPorCobrarTipoCambio = oValorCuotaVariacionBE.CuentasPorCobrarTipoCambio;
            variacionDatos.cuentasPorPagarTipoCambio = oValorCuotaVariacionBE.CuentasPorPagarTipoCambio;
            variacionDatos.cuentasPorCobrarPrecio = oValorCuotaVariacionBE.CuentasPorCobrarPrecio;
            variacionDatos.cuentasPorPagarPrecio = oValorCuotaVariacionBE.CuentasPorPagarPrecio;
            variacionDatos.cajaTipoCambio  = oValorCuotaVariacionBE.CajaTipoCambio;
            variacionDatos.porcentageVariacionEstimado = oValorCuotaVariacionBE.PorcentageVariacionEstimado;
            variacionDatos.totalRentabilidadInversiones = oValorCuotaVariacionBE.TotalRentabilidadInversiones;
            leyendaVariacion = String.Format(leyendaVariacion, oValorCuotaVariacionBE.DiferenciaEstimadoSIT, oValorCuotaVariacionBE.PorcentageVariacionEstimado, oValorCuotaVariacionBE.PorcentageVariacionSIT);
            oResultado.variacionDatos = variacionDatos;
            imagen = oValorCuotaVariacionBE.EstadoSemaforo == "R" ? "circulo_rojo" : "circulo_verde";
        }
        else {
            //leyendaVariacion = "VARIACION SIT({0})";
            if (rangoY.Count > 0) {
                leyendaVariacion = String.Format(leyendaVariacion, 0 - rangoY[rangoY.Count - 1], 0, rangoY[rangoY.Count - 1]);           
            }
            
            imagen = "circulo_verde";

        }

        oResultado.fechaOperacion = fechaOperacion;
        oResultado.rangoX = rangoX;
        oResultado.rangoY = rangoY;
        oResultado.portafolio = portafolioNombre;
        oResultado.leyendaVariacion = leyendaVariacion;
        oResultado.imagen = imagen;
        return oResultado;

    }

    [WebMethod]
    public static ValoresEList GetNemonico(string Nemonico, string FechaOperacion, string TipoRenta)
    {
        ValoresBM oValoresBM = new ValoresBM();
        return oValoresBM.ListarValores(Nemonico, UIUtility.ConvertirFechaaDecimal(FechaOperacion), TipoRenta);
    }
    [WebMethod]
    public static TercerosEList GetIntermediario(string Descripcion)
    {
        TercerosBM oTercerosBM = new TercerosBM();
        return oTercerosBM.ListarIntermediarios(Descripcion);
    }

    //INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - DESCRIPCION | 18/05/2018
    [WebMethod]
    public static string GetPortafolioSelectById(string Idportafolio)
    {
        PortafolioBM portafolioBM = new PortafolioBM();
        DataTable dt = portafolioBM.PortafolioSelectById(Idportafolio);
        decimal fecha = 0;
        string moneda = string.Empty;
        if (dt != null && dt.Rows.Count > 0)
        {
            fecha = Convert.ToDecimal(dt.Rows[0]["FechaNegocio"].ToString());
            moneda = dt.Rows[0]["CodigoMoneda"].ToString();
        }

        return UIUtility.ConvertirFechaaString(fecha); //+ "_" + moneda;
    }

    [WebMethod]
    public static List<listado> GetPortafolioCodigoListarByNemonico(string CodigoNemonico)
    {
        PortafolioBM portafolioBM = new PortafolioBM();
        List<listado> lista = new List<listado>();
        var listado = portafolioBM.PortafolioCodigoListarByNemonico("", CodigoNemonico.Trim(), Constantes.M_STR_CONDICIONAL_NO);
        if (listado != null && listado.Rows.Count > 0)
        {
            for (int i = 0; i < listado.Rows.Count; i++)
            {
                lista.Add(new listado() { id = listado.Rows[i][0].ToString(), descripcion = listado.Rows[i][1].ToString() });
            }
        }
        return lista;
    }

    [WebMethod]
    public static string VectorPrecioPIP(string IdPortafolio, string FechaOperacion)
    {
        string msg = string.Empty;
        PortafolioBM portafolio = new PortafolioBM();

        IdPortafolio = IdPortafolio.Trim();
        FechaOperacion = FechaOperacion.Trim();

        decimal fechaOperacion = UIUtility.ConvertirFechaaDecimal(FechaOperacion);

        DataTable dt = portafolio.ListarNemonicoXValorizar(IdPortafolio, fechaOperacion);
        if (dt.Rows.Count > 0)
        {
            if (!dt.Rows[0][0].ToString().Trim().Equals("0"))
            {
                msg = IdPortafolio + "-" + fechaOperacion;
            }

            //msg = "Ocurrio un error en el Proceso de Valoración";
        }

        return msg;
    }


    //INICIO | PROYECTO FONDOS II - ZOLUXIONES | RCE | Se cambia Fecha de Liquidación de acuerdo a fecha de operación por cada row | 10/09/2018
    [WebMethod]
    public static string GetFechaLiquidacionbyFechaOp(string fechaOperacion)
    {
        bool esDiaHabil = false;
        string fechaLiquidacion = string.Empty;
        int DiaFeriado = 2;
        FeriadoBM oFeriado = new FeriadoBM();

        if (!(fechaOperacion == ""))
        {
            while ((esDiaHabil == false))
            {
                fechaLiquidacion = DateTime.Parse(fechaOperacion).AddDays(DiaFeriado).ToShortDateString();
                esDiaHabil = oFeriado.VerificaDia(UIUtility.ConvertirFechaaDecimal(fechaLiquidacion), "");
                DiaFeriado++;
            }

            return fechaLiquidacion;
        }

        return fechaOperacion;
    }
    //FIN | PROYECTO FONDOS II - ZOLUXIONES | RCE | Se cambia Fecha de Liquidación de acuerdo a fecha de operación por cada row | 10/09/2018


    [WebMethod]
    public static string CargarTipoValorizacionPortafolio(string idPortafolio)
    {
        string tipoValorizacion = string.Empty;
        PortafolioBM objPortafolioBM = new PortafolioBM();
        idPortafolio = idPortafolio.Trim();
        DataTable dt = objPortafolioBM.PortafolioSelectById(idPortafolio);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                string tipoFondo = string.Empty;
                tipoFondo = dt.Rows[0]["TipoNegocio"].ToString();
                if (tipoFondo == "MANDA")
                {
                    tipoValorizacion = "DIS_VENTA";
                }
                else
                {
                    tipoValorizacion = "VAL_RAZO";
                }
            }
        }
        return tipoValorizacion;
    }

    [WebMethod]
    public static List<listado> CargarTipoValorizacion(string clasificacion)
    {
        ParametrosGeneralesBM objParamGeneral = new ParametrosGeneralesBM();
        List<listado> lista = new List<listado>();
        DataTable dt;
        dt = objParamGeneral.Listar("TipoValorizacion", null);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lista.Add(new listado() { id = dt.Rows[i]["Valor"].ToString(), descripcion = dt.Rows[i]["Nombre"].ToString() });
                }
            }
        }
        return lista;
    }

    [WebMethod]
    public static List<listado> CargarNumeroDeCuentas(string codigoFondo, string codigoMoneda, string codigoBanco)
    {
        PagoFechaComisionBM pagoFechaComisionBM = new PagoFechaComisionBM();
        List<listado> lista = new List<listado>();
        DataTable dt;
        dt = pagoFechaComisionBM.ListarNumeroDeCuentas(codigoFondo, codigoMoneda, codigoBanco);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lista.Add(new listado() { id = dt.Rows[i]["CuentaContable"].ToString() + " - " + dt.Rows[i]["NumeroCuenta"].ToString(), descripcion ="Nro. Cuenta: " +dt.Rows[i]["NumeroCuenta"].ToString() + ". Saldo:" + dt.Rows[i]["Saldo"].ToString() });
                }
            }
        }
        return lista;
    }

    //FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - DESCRIPCION | 18/05/2018
    // INICIO | Zoluxiones | Cronograma de Pagos | rcolonia | 13/02/2019
    [WebMethod]
    public static List<CronogramaPagosBE> GetDetalleInstrumentoCronogramaPagos(string fondo, string fechaPago)
    {
        CronogramaPagosBM oCronogramaPagosBM = new CronogramaPagosBM();
        DataTable dt = oCronogramaPagosBM.CronogramaPagos_ListarbyDetalleInstrumento(fondo, UIUtility.ConvertirFechaaDecimal(fechaPago));
        List<CronogramaPagosBE> listaCronogramaPagos = new List<CronogramaPagosBE>();
        CronogramaPagosBE oCronogramaPagosBE;
        foreach (DataRow dr in dt.Rows)
        {
            oCronogramaPagosBE = new CronogramaPagosBE(dr["codigoMnemonico"].ToString(), dr["tipoPago"].ToString(), dr["fechaLiquidacion"].ToString());
            listaCronogramaPagos.Add(oCronogramaPagosBE);
        }

        return listaCronogramaPagos;
    }

    [WebMethod]
    public static string GetFechaMaximaValorizacion(string idPortafolio)
    {
        ValoresBM oValoresBM = new ValoresBM();
        idPortafolio = idPortafolio.Trim();
        return oValoresBM.ObtenerUltimaFechaValorizacion(idPortafolio);
    }

    [WebMethod]
    public static List<ListaDetalleFondoParticipacion> Get_CuponeraNormal_ObtenerPorcentajeParticipacion(
                                                                           string codigoMnemonico,
                                                                           decimal fechaIni,
                                                                           decimal fechaFin,
                                                                           int consecutivo,
                                                                           int estado,
                                                                           decimal montoNominalTotal,
                                                                           decimal tasaCupon,
                                                                           decimal difDias,
                                                                           int baseCupon,
                                                                           decimal amortizac,
                                                                           decimal sumaMontoAmortizacion
                                                                           )
    {
        CuponeraBM oCuponeraBM = new CuponeraBM();
        List<ListaDetalleFondoParticipacion> lista = new List<ListaDetalleFondoParticipacion>();
        DataTable dt = oCuponeraBM.CuponeraNormal_ObtenerPorcentajeParticipacion(codigoMnemonico,
                                                                         fechaIni,
                                                                         fechaFin,
                                                                         consecutivo,
                                                                         estado,
                                                                         montoNominalTotal,
                                                                         tasaCupon,
                                                                         difDias,
                                                                         baseCupon,
                                                                         amortizac,
                                                                         sumaMontoAmortizacion);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lista.Add(new ListaDetalleFondoParticipacion()
                    {
                        Descripcion = dt.Rows[i]["Descripcion"].ToString(),
                        Principal = string.Format("{0:###,##0.00}", dt.Rows[i]["Principal"]),
                        Amortizacion = string.Format("{0:###,##0.00}", dt.Rows[i]["Amortizacion"]),
                        Interes = string.Format("{0:###,##0.00}", dt.Rows[i]["Interes"]),
                        Participacion = string.Format("{0:###,##0.0000000}", dt.Rows[i]["Participacion"])
                    });
                }
            }
        }
        return lista;
    }

    // FIN | Zoluxiones | Cronograma de Pagos | rcolonia | 13/02/2019
    //INICIO | OT12028 | Zoluxiones | Rcolonia | Validación de cierre de cajas para valorización de portafolio
    [WebMethod]
    public static string GetValidacionFechaCierreCajas(string idPortafolio, string FechaOperacion)
    {
        idPortafolio = idPortafolio.Trim();
        FechaOperacion = FechaOperacion.Trim();

        decimal fechaOperacion = UIUtility.ConvertirFechaaDecimal(FechaOperacion);
        return UIUtility.ValidarCajas(idPortafolio, string.Empty, fechaOperacion);
    }
    //FIN | OT12028 | Zoluxiones | Rcolonia | Validación de cierre de cajas para valorización de portafolio
}

public class listado
{
    public string id { get; set; }
    public string descripcion { get; set; }
}
public class ListaDetalleFondoParticipacion
{
    public string Descripcion { get; set; }
    public string Principal { get; set; }
    public string Amortizacion { get; set; }
    public string Interes { get; set; }
    public string Participacion { get; set; }
}
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Inversiones_Reportes_frmVisorInventarioForward
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strfondo, strFecini, strFecFin, strMon, strMond, strEst, strFecs, strEstd, strFecsd, Nomfondo As String
        Dim strNombre As String, strUsuario As String, strMercado As String
        Nomfondo = Request.QueryString("cFondo")
        strfondo = Request.QueryString("vFondo")
        strFecini = Request.QueryString("vfecini")
        strFecFin = Request.QueryString("vfecfin")
        strMon = Request.QueryString("vmon")
        strMond = Request.QueryString("vmond")
        strEst = Request.QueryString("vest")
        strFecs = Request.QueryString("vfecs")
        strEstd = Request.QueryString("vestd")
        strFecsd = Request.QueryString("vfecsd")
        strMercado = Request.QueryString("vMerca")
        strNombre = "Usuario"
        Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
        strUsuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & strNombre & "'")(0)(1), String)
        Dim drfila As DataRow
        oReport.Load(Server.MapPath("InventarioForwards.rpt"))
        Dim dsReporte As New DsInventarioForward
        Dim oReporte As DataSet
        oReporte = New OrdenPreOrdenInversionBM().InventarioForward(strfondo, Now.ToString("yyyyMMdd"), strFecini, strFecFin, strMon, strMond, strEst, strFecs, strMercado, DatosRequest)
        For Each dr As DataRow In oReporte.Tables(0).Rows
            drfila = dsReporte.Tables(0).NewRow
            drfila("FechaProceso") = dr("FechaProceso")
            drfila("Valor") = dr("Valor")
            drfila("CodEmisor") = dr("CodEmisor")
            drfila("Emisor") = dr("Emisor")
            drfila("CodigoMoneda") = dr("CodigoMoneda")
            drfila("Moneda") = dr("Moneda")
            drfila("CodigoRef") = dr("CodigoRef")
            drfila("HoraNegaciacion") = dr("HoraNegaciacion")
            drfila("CodigoMonedaVenta") = dr("CodigoMonedaVenta")
            drfila("MonedaVenta") = dr("MonedaVenta")
            drfila("IndicadorMovimiento") = dr("IndicadorMovimiento")
            drfila("TipoForward") = dr("TipoForward")
            drfila("MontoForward") = dr("MontoForward")
            drfila("PrecioTransaccion") = dr("PrecioTransaccion")
            drfila("FechaEmision") = dr("FechaEmision")
            drfila("FechaLiquidacion") = dr("FechaLiquidacion")
            drfila("FechaVencimiento") = dr("FechaVencimiento")
            drfila("PlazoVencimiento") = dr("PlazoVencimiento")
            drfila("Modalidad") = dr("Modalidad")
            drfila("TipoCambioForwardPactado") = dr("TipoCambioForwardPactado")
            drfila("IndicadorCaja") = dr("IndicadorCaja")
            drfila("PlazaNegociacion") = dr("PlazaNegociacion")
            drfila("Fondo") = dr("Descripcion")
            drfila("MontoCalculado") = dr("MontoCalculado") 'HDG 20110913
            drfila("Fixing") = dr("Fixing") 'HDG 20110913
            dsReporte.Tables(0).Rows.Add(drfila)
        Next
        oReport.SetDataSource(dsReporte)
        oReport.SetParameterValue("@Usuario", strUsuario)
        oReport.SetParameterValue("@Fondo", IIf(Nomfondo.Equals("Todos"), "Todos los Fondos", Nomfondo))
        oReport.SetParameterValue("@FechaOperacionIni", strFecsd & " del " & UIUtility.ConvertirFechaaString(strFecini) & " al " & UIUtility.ConvertirFechaaString(strFecFin))
        oReport.SetParameterValue("@Estado", strEstd)
        oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
        Me.CrystalReportViewer1.ReportSource = oReport
    End Sub
    Protected Sub Modulos_Inversiones_Reportes_frmVisorInventarioForward_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class
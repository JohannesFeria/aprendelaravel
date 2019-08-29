Imports System.Data
Imports Sit.BusinessLayer
Imports CrystalDecisions.CrystalReports.Engine
Partial Class Modulos_Inversiones_Reportes_frmVisorVerificacionFirmasOI
    Inherits System.Web.UI.Page
    Dim oReport As New ReportDocument
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim codCargoFirmante As Decimal = Convert.ToDecimal(IIf(Request.QueryString("vCodCargoFirmante") Is Nothing, -1, Request.QueryString("vCodCargoFirmante")))
        Dim codPortafolioSBS As String = IIf(Request.QueryString("vCodPortafolioSBS") Is Nothing, "", Request.QueryString("vCodPortafolioSBS"))
        Dim codigoOrden As String = IIf(Request.QueryString("vCodigoOrden") Is Nothing, "", Request.QueryString("vCodigoOrden"))
        Dim codigoMercado As String = IIf(Request.QueryString("vCodigoMercado") Is Nothing, "", Request.QueryString("vCodigoMercado"))
        Dim codigoOperacion As String = IIf(Request.QueryString("vCodigoOperacion") Is Nothing, "", Request.QueryString("vCodigoOperacion"))
        Dim estadoFirma As String = IIf(Request.QueryString("vEstadoFirma") Is Nothing, "", Request.QueryString("vEstadoFirma"))
        Dim fechaOperacion As Decimal = IIf(Request.QueryString("vFechaOperacion") Is Nothing, "", Request.QueryString("vFechaOperacion"))
        Dim codigoUsuario As String = IIf(Request.QueryString("vCodigoUsuario") Is Nothing, "", Request.QueryString("vCodigoUsuario"))
        Dim codCategReporte As String = IIf(Request.QueryString("vCodCategReporte") Is Nothing, "", Request.QueryString("vCodCategReporte"))
        Dim oOrdenInversion As New OrdenPreOrdenInversionBM()
        Dim dsReporteFirmas As DataSet = oOrdenInversion.ReporteVerificacionFirmas(fechaOperacion, codCargoFirmante, codigoUsuario, codigoOrden, estadoFirma, codPortafolioSBS, codigoOperacion, codigoMercado, codCategReporte)
        oReport.Load(Server.MapPath("rptVerificacionFirmasOI.rpt"))
        crVerificacionFirmas.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
        crVerificacionFirmas.RenderingDPI = 120
        oReport.SetDataSource(dsReporteFirmas.Tables(0))
        oReport.SetParameterValue("@Usuario", codigoUsuario)
        oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
        crVerificacionFirmas.ReportSource = oReport
    End Sub
    Protected Sub Modulos_Inversiones_Reportes_frmVisorVerificacionFirmasOI_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class
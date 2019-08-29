Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Inversiones_Reportes_Cuponera_frmVisorConsultaCuponeras
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strCodigoMnemonico As String = Request.QueryString("vCodigoMnemonico")
        Dim strGuid As String = Request.QueryString("vGuid")
        Dim strCodigoOrden As String = Request.QueryString("vCodigoOrden")
        Dim strCodigoPortafolioSBS As String = Request.QueryString("vCodigoPortafolioSBS")
        Dim strFechaOperacion As String = Request.QueryString("vFechaOperacion")
        Dim strInteresCorrido As String = Request.QueryString("vInteresCorrido")
        Dim strMontoOperacion As String = Request.QueryString("vMontoOperacion")
        Dim strPrecioCalculado As String = Request.QueryString("vPrecioCalculado")
        Dim oCuponeraBM As New CuponeraBM
        'Ejecutar la consulta
        Dim dtConsulta As New DataTable
        dtConsulta = oCuponeraBM.SeleccionarPorOrdenInversionVPN(strCodigoMnemonico, strGuid, strCodigoOrden, strCodigoPortafolioSBS, strFechaOperacion, DatosRequest)
        Dim usuario, nombre As String
        nombre = "Usuario"
        Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
        usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
        Dim dsconsulta As New DsConsultaCuponeras
        Dim drconsulta As DataRow        
        oReport.Load(Server.MapPath("RptConsultaCuponeras.rpt"))
        If Not dtConsulta Is Nothing Then
            For Each drv As DataRow In dtConsulta.Rows
                drconsulta = dsconsulta.Tables(0).NewRow()
                drconsulta("Indice") = drv("Indice")
                drconsulta("Secuencia") = drv("Secuencia")
                drconsulta("FechaInicio") = drv("FechaInicio")
                drconsulta("FechaTermino") = drv("FechaTermino")
                drconsulta("FechaInicio1") = drv("FechaInicio1")
                drconsulta("FechaTermino1") = drv("FechaTermino1")
                drconsulta("Amortizacion") = drv("Amortizacion")
                drconsulta("DiferenciaDias") = drv("DiferenciaDias")
                drconsulta("TasaCupon") = drv("TasaCupon")
                drconsulta("DiasAcumulados") = drv("DiasAcumulados")
                drconsulta("TotalVP") = drv("TotalVP")
                drconsulta("Flujo") = drv("Flujo")
                dsconsulta.Tables(0).Rows.Add(drconsulta)
            Next
        End If
        dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
        oReport.SetDataSource(dsconsulta)
        oReport.SetParameterValue("@Usuario", usuario)
        oReport.SetParameterValue("@InteresCorrido", strInteresCorrido)
        oReport.SetParameterValue("@MontoOperacion", strMontoOperacion)
        oReport.SetParameterValue("@PrecioCalculado", strPrecioCalculado)
        oReport.SetParameterValue("RutaLogo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
        Me.crPreOrden.ReportSource = oReport
    End Sub
    Protected Sub Modulos_Inversiones_Reportes_Cuponera_frmVisorConsultaCuponeras_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class
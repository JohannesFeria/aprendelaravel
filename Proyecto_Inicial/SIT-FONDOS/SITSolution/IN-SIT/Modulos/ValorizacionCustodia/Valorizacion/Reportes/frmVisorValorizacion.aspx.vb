Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Valorizacion_y_Custodia_Valorizacion_Reportes_frmVisorValorizacion
    Inherits BasePage
    Dim rep As New ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not IsPostBack Then
                Session("DsDatosValorizacion") = Nothing
            End If
            Dim reporte As String
            reporte = Request.QueryString("ClaseReporte")
            rep.Load(Server.MapPath(reporte & ".rpt"))
            Select Case (reporte)
                Case "ResultadosValorizacion" : ReporteResultadosValorizacion()
            End Select
            Me.CrystalReportViewer1.ReportSource = rep
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub
    Private Sub ReporteResultadosValorizacion()
        Dim codigoPortafolio As String = Request.QueryString("codPortafolio")
        Dim portafolio As String = Request.QueryString("Portafolio")
        Dim fecha As String = Decimal.Parse(Request.QueryString("Fecha"))
        Dim tipoValorizacion As String = Request.QueryString("tipoValorizacion")
        Dim dtConsulta As New DataTable
        Dim oValores As New ValoresBM
        Dim dsConsulta As New Valorizacion
        Dim drConsulta As DataRow
        If Session("DsDatosValorizacion") Is Nothing Then 'RGF 20110318
            dtConsulta = oValores.ReporteSeleccionarValorizacion(codigoPortafolio, fecha, tipoValorizacion, DatosRequest).Tables(0)
            For Each drv As DataRow In dtConsulta.Rows
                drConsulta = dsConsulta.Tables(0).NewRow()
                drConsulta("CodigoMnemonico") = drv("CodigoMnemonico")
                drConsulta("CodigoSBS") = drv("CodigoSBS")
                drConsulta("CodigoTercero") = drv("CodigoTercero")
                drConsulta("Entidad") = drv("Entidad")
                drConsulta("NumeroInversion") = drv("NumeroInversion")
                drConsulta("ValorNominalMonedaLocal") = Format(Convert.ToDecimal(drv("ValorNominalMonedaLocal")), "###,##0.0000000")
                drConsulta("SumaValorNominalMonedaLocal") = Format(Convert.ToDecimal(drv("SumaValorNominalMonedaLocal")), "###,##0.0000000")
                drConsulta("GlobalValorNominalMonedaLocal") = Format(Convert.ToDecimal(drv("GlobalValorNominalMonedaLocal")), "###,##0.0000000")
                drConsulta("ValorNominalMonedaOrigen") = Format(Convert.ToDecimal(drv("ValorNominalMonedaOrigen")), "###,##0.0000000")
                drConsulta("SumaValorNominalMonedaOrigen") = Format(Convert.ToDecimal(drv("SumaValorNominalMonedaOrigen")), "###,##0.0000000")
                drConsulta("GlobalValorNominalMonedaOrigen") = Format(Convert.ToDecimal(drv("GlobalValorNominalMonedaOrigen")), "###,##0.0000000")
                drConsulta("VPNLocalAnt") = Format(Convert.ToDecimal(drv("VPNLocalAnt")), "###,##0.0000000")
                drConsulta("SumaVPNLocalAnt") = Format(Convert.ToDecimal(drv("SumaVPNLocalAnt")), "###,##0.0000000")
                drConsulta("GlobalVPNLocalAnt") = Format(Convert.ToDecimal(drv("GlobalVPNLocalAnt")), "###,##0.0000000")
                drConsulta("VPNLocal") = Format(Convert.ToDecimal(drv("VPNLocal")), "###,##0.0000000")
                drConsulta("SumaVPNLocal") = Format(Convert.ToDecimal(drv("SumaVPNLocal")), "###,##0.0000000")
                drConsulta("GlobalVPNLocal") = Format(Convert.ToDecimal(drv("GlobalVPNLocal")), "###,##0.0000000")
                drConsulta("Diferencia") = Format(Convert.ToDecimal(drv("Diferencia")), "###,##0.0000000")
                drConsulta("SumaDiferencia") = Format(Convert.ToDecimal(drv("SumaDiferencia")), "###,##0.0000000")
                drConsulta("GlobalDiferencia") = Format(Convert.ToDecimal(drv("GlobalDiferencia")), "###,##0.0000000")
                drConsulta("ValorTIR360") = Format(Convert.ToDecimal(drv("ValorTIR360")), "###,##0.0000")
                drConsulta("SumaValorTIR360") = Format(Convert.ToDecimal(drv("SumaValorTIR360")), "###,##0.0000")
                drConsulta("GlobalValorTIR360") = Format(Convert.ToDecimal(drv("GlobalValorTIR360")), "###,##0.0000")
                drConsulta("ValorTIROriginal") = Format(Convert.ToDecimal(drv("ValorTIROriginal")), "###,##0.0000")
                drConsulta("SumaValorTIROriginal") = Format(Convert.ToDecimal(drv("SumaValorTIROriginal")), "###,##0.0000")
                drConsulta("GlobalValorTIROriginal") = Format(Convert.ToDecimal(drv("GlobalValorTIROriginal")), "###,##0.0000")
                drConsulta("ValorTIRLocal") = Format(Convert.ToDecimal(drv("ValorTIRLocal")), "###,##0.0000")
                drConsulta("SumaValorTIRLocal") = Format(Convert.ToDecimal(drv("SumaValorTIRLocal")), "###,##0.0000")
                drConsulta("GlobalValorTIRLocal") = Format(Convert.ToDecimal(drv("GlobalValorTIRLocal")), "###,##0.0000")
                drConsulta("Duracion") = Format(Convert.ToDecimal(drv("Duracion")), "###,##0.0000")
                drConsulta("SumaDuracion") = Format(Convert.ToDecimal(drv("SumaDuracion")), "###,##0.0000")
                drConsulta("GlobalDuracion") = Format(Convert.ToDecimal(drv("GlobalDuracion")), "###,##0.0000")
                drConsulta("FechaVencimiento") = drv("FechaVencimiento")
                drConsulta("Convexidad") = Format(Convert.ToDecimal(drv("Convexidad")), "###,##0.0000000")
                drConsulta("SumaConvexidad") = Format(Convert.ToDecimal(drv("SumaConvexidad")), "###,##0.0000000")
                drConsulta("GlobalConvexidad") = Format(Convert.ToDecimal(drv("GlobalConvexidad")), "###,##0.0000000")
                drConsulta("CodigoTipoTitulo") = drv("CodigoTipoTitulo")
                drConsulta("TipoTitulo") = drv("TipoTitulo")
                dsConsulta.Tables(0).Rows.Add(drConsulta)
            Next
            Session("DsDatosValorizacion") = dsConsulta
        Else
            dsConsulta = Session("DsDatosValorizacion") 'RGF 20110318
        End If
        rep.SetDataSource(dsConsulta)
        rep.SetParameterValue("Usuario", MyBase.Usuario)
        rep.SetParameterValue("Fondo", portafolio)
        rep.SetParameterValue("FechaValorizacion", UIUtility.ConvertirFechaaString(fecha))
        Dim strVal As String = ""
        Select Case tipoValorizacion
            Case "N"
                strVal = "Real"
            Case "E"
                strVal = "Estimada"
            Case "C"
                strVal = "Curva Cero"
        End Select
        rep.SetParameterValue("TipoValorizacion", "Resultados de Valorización - " & strVal)
    End Sub
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Protected Sub Modulos_Valorizacion_y_Custodia_Valorizacion_Reportes_frmVisorValorizacion_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        rep.Close()
        rep.Dispose()
    End Sub
End Class
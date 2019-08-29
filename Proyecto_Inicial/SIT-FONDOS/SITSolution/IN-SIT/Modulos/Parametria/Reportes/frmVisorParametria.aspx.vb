Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Parametria_Reportes_frmVisorParametria
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim StrOpcion, StrUsuario, StrNombre, Strindicador, StrEstadoPortafolio As String
        Dim DatFechaOperacionInicio, DatFechaOperacionFinal As Date
        Dim DecFechaOperacionInicio, DecFechaOperacionFinal As Decimal
        StrNombre = "Usuario"
        StrOpcion = Request.QueryString("vopcion")
        Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
        StrUsuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)
        Dim drfila As DataRow

        Select Case StrOpcion
            Case "Indicadores"
                DatFechaOperacionInicio = Request.QueryString("vfechainicio")
                DatFechaOperacionFinal = Request.QueryString("vfechafinal")
                Strindicador = Request.QueryString("vindicador")
                DecFechaOperacionInicio = CType(DatFechaOperacionInicio.ToString("yyyyMMdd"), Decimal)
                DecFechaOperacionFinal = CType(DatFechaOperacionFinal.ToString("yyyyMMdd"), Decimal)
                Dim oIndicadorBM As New IndicadorBM
                Dim manejaPeriodo As String
                Dim dsReporte As New ValorIndicador
                Dim oreporte As DataSet
                manejaPeriodo = oIndicadorBM.SeleccionarPorFiltro(Strindicador, "", "", 0, 0, 0, "", "", "", DatosRequest).Indicador.Rows(0).Item("ManejaPeriodo").ToString
                If manejaPeriodo = "S" Then
                    oReport.Load(Server.MapPath("RptIndicadores.rpt"))
                    oreporte = New ParametriaReportesBM().ReporteCotizacionVAC(Strindicador, DecFechaOperacionInicio, DecFechaOperacionFinal, DatosRequest)
                    For Each dr As DataRow In oreporte.Tables(0).Rows
                        drfila = dsReporte.Tables(0).NewRow
                        drfila("CodigoIndicador") = dr("CodigoIndicador")
                        drfila("Fecha") = dr("Fecha")
                        drfila("DiasPeriodo") = dr("DiasPeriodo")
                        drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                        drfila("Valor") = dr("Valor")
                        dsReporte.Tables(0).Rows.Add(drfila)
                    Next
                ElseIf manejaPeriodo = "N" Then
                    oReport.Load(Server.MapPath("ReporteIndicadoresNoPeriodicos.rpt"))
                    oreporte = New ParametriaReportesBM().ReporteCotizacionVAC(Strindicador, DecFechaOperacionInicio, DecFechaOperacionFinal, DatosRequest)
                    For Each dr As DataRow In oreporte.Tables(0).Rows
                        drfila = dsReporte.Tables(0).NewRow
                        drfila("CodigoIndicador") = dr("CodigoIndicador")
                        drfila("Fecha") = dr("Fecha")
                        drfila("DiasPeriodo") = dr("DiasPeriodo")
                        drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                        drfila("Valor") = dr("Valor")
                        dsReporte.Tables(0).Rows.Add(drfila)
                    Next
                End If
                oReport.SetDataSource(dsReporte)
                oReport.SetParameterValue("@FechaOperacionInicio", "De " & DatFechaOperacionInicio.ToShortDateString() & " Al " & DatFechaOperacionFinal.ToShortDateString())
                oReport.SetParameterValue("@Usuario", StrUsuario)
                oReport.SetParameterValue("@Indicador", Strindicador)
                oReport.SetParameterValue("@RutaImagen", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                Me.CrystalReportViewer1.ReportSource = oReport
            Case "Cuponera"
                Dim strmnemonico As String
                strmnemonico = Request.QueryString("vcodigomnemonico")
                oReport.Load(Server.MapPath("ReporteCuponera.rpt"))
                Dim dsReporte As New dsCuponera
                Dim oreporte As DataSet
                oreporte = New ParametriaReportesBM().ReporteCuponera(strmnemonico, "0", DatosRequest)
                For Each dr As DataRow In oreporte.Tables(0).Rows
                    drfila = dsReporte.Tables(0).NewRow
                    drfila("CodigoNemonico") = dr("CodigoNemonico")
                    drfila("Secuencia") = dr("Secuencia")
                    drfila("FechaInicio") = dr("FechaInicio")
                    drfila("FechaTermino") = dr("FechaTermino")
                    drfila("Diferencia") = dr("Diferenciadias")
                    drfila("Amortizacion") = dr("Amortizacion")
                    drfila("Estado") = dr("Estado")
                    drfila("FechaIDI") = dr("FechaIDI")
                    drfila("FechaPago") = dr("FechaPago")
                    drfila("Tasacupon") = dr("Tasacupon")
                    drfila("Base") = dr("Base")
                    drfila("DiasPago") = dr("DiasPago")
                    dsReporte.Tables(0).Rows.Add(drfila)
                Next
                oReport.SetDataSource(dsReporte)
                oReport.SetParameterValue("@Usuario", StrUsuario)
                oReport.SetParameterValue("@CodigoMnemonico", strmnemonico)
                oReport.SetParameterValue("@Titulo", "Reporte de Cuponera")
                Me.CrystalReportViewer1.ReportSource = oReport
            Case "CuponeraEspecial"
                Dim strmnemonico As String
                strmnemonico = Request.QueryString("vcodigomnemonico")
                oReport.Load(Server.MapPath("ReporteCuponera.rpt"))
                Dim dsReporte As New dsCuponera
                Dim oreporte As DataSet
                oreporte = New ParametriaReportesBM().ReporteCuponera(strmnemonico, "1", DatosRequest)
                For Each dr As DataRow In oreporte.Tables(0).Rows
                    drfila = dsReporte.Tables(0).NewRow
                    drfila("CodigoNemonico") = dr("CodigoNemonico")
                    drfila("Secuencia") = dr("Secuencia")
                    drfila("FechaInicio") = dr("FechaInicio")
                    drfila("FechaTermino") = dr("FechaTermino")
                    drfila("Diferencia") = dr("Diferenciadias")
                    drfila("Amortizacion") = dr("Amortizacion")
                    drfila("Tasacupon") = dr("Tasacupon")
                    drfila("Base") = dr("Base")
                    drfila("DiasPago") = dr("DiasPago")
                    dsReporte.Tables(0).Rows.Add(drfila)
                Next
                oReport.SetDataSource(dsReporte)
                oReport.SetParameterValue("@Usuario", StrUsuario)
                oReport.SetParameterValue("@CodigoMnemonico", strmnemonico)
                oReport.SetParameterValue("@Titulo", "Reporte de Cuponera Especial")
                Me.CrystalReportViewer1.ReportSource = oReport
        End Select
    End Sub
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Page.RegisterStartupScript("JScript", "<script language=javascript>window.close();</script>")
    End Sub
    Protected Sub Modulos_Parametria_Reportes_frmVisorParametria_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class
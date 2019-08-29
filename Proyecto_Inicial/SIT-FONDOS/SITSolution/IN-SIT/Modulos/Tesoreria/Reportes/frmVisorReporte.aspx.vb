Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Tesoreria_Reportes_frmVisorReporte
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load        
        Dim dsOrigen As DataSet
        If Not Request.QueryString("tipo") Is Nothing Then
            If Request.QueryString("tipo") = "OCE" Then
                Dim dsRepOperacionesCajaExt As New DsRepOperacionesCajaExt
                Dim dtRepOperacionesCajaExt As New DataTable
                Dim fechaInicio As Decimal = CType(Request.QueryString("fechaInicio"), Decimal)
                Dim fechaFin As Decimal = CType(Request.QueryString("fechaFin"), Decimal)
                Dim motivo As String = Request.QueryString("motivo")
                Dim estado As String = Request.QueryString("estado")
                dsOrigen = New OperacionesCajaBM().ReporteExtornos(fechaInicio, fechaFin, motivo, estado, DatosRequest)
                dtRepOperacionesCajaExt = dsOrigen.Tables(0)
                If dtRepOperacionesCajaExt.Rows.Count > 0 Then
                    CopiarTabla(dtRepOperacionesCajaExt, dsRepOperacionesCajaExt.Tables("RepOperacionesCajaExt"))
                    oReport.Load(Server.MapPath("rptOperacionesCajaExt.rpt"))
                    oReport.SetDataSource(dsRepOperacionesCajaExt)
                    oReport.SetParameterValue("@Usuario", Usuario.ToString())
                    oReport.SetParameterValue("@FechaInicio", UIUtility.ConvertirFechaaString(fechaInicio))
                    oReport.SetParameterValue("@FechaFin", UIUtility.ConvertirFechaaString(fechaFin))
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                Else
                    AlertaJS("No hay datos para mostrar!")
                End If
            End If
            If Request.QueryString("tipo") = "IC" Then
                Dim dtStockInicial As New DataTable
                Dim dtStockMovimiento As New DataTable
                Dim dtStockFinal As New DataTable
                Dim fechaInicio As Decimal = CType(Request.QueryString("fechaInicio"), Decimal)
                Dim fechaFinal As Decimal = CType(Request.QueryString("FechaFin"), Decimal)
                dsOrigen = New OperacionesCajaBM().ReporteInventarioCartas(fechaInicio, fechaFinal, DatosRequest)
                dtStockInicial = dsOrigen.Tables(0)
                If dtStockInicial.Rows.Count > 0 Then
                    dtStockMovimiento = dsOrigen.Tables(1)
                    dtStockFinal = dsOrigen.Tables(2)
                    Dim oDsInventarioCartas As New DsInventarioCartas
                    CopiarTabla(dtStockInicial, oDsInventarioCartas.StockInicial)
                    CopiarTabla(dtStockMovimiento, oDsInventarioCartas.InventarioCartas)
                    oReport.Load(Server.MapPath("rptInventarioCartas.rpt"))
                    oReport.SetDataSource(oDsInventarioCartas)
                    oReport.SetParameterValue("@Usuario", Usuario)
                    oReport.SetParameterValue("@TotalDisponible", CType(dtStockFinal.Rows(0)("NoImpresas"), Decimal))
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                Else
                    AlertaJS("No hay datos en el inventario de cartas")
                End If
            End If
            If Not oReport.IsLoaded Then
                Call Retornar()
            Else
                Me.CrystalReportViewer1.ReportSource = oReport
            End If
        End If
    End Sub
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        If (dtOrigen Is Nothing) Then
            Return
        End If
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Private Sub Retornar()
        EjecutarJS("window.close")
    End Sub
    Protected Sub Modulos_Tesoreria_Reportes_frmVisorReporte_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class
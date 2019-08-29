Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports System.Data

Partial Class Modulos_Tesoreria_Reportes_frmReporteNeg
    Inherits BasePage
    Dim rep As New ReportDocument

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim reporte As String

        reporte = Request.QueryString("ClaseReporte")
        rep.Load(Server.MapPath(reporte & ".rpt"))

        Select Case (reporte)
            Case "MovNegociacion" : ReporteMovNegociacion()
        End Select
        Me.CrystalReportViewer1.ReportSource = rep
    End Sub

    Private Sub ReporteMovNegociacion()
        Dim ds As DataSet = Session("ReporteMovimientosNeg")
        Dim dsAux As New RepMovNegociacion
        CopiarTabla(ds.Tables(0), dsAux.MovimientosNeg1)
        rep.SetDataSource(dsAux)
        rep.SetParameterValue("Usuario", MyBase.Usuario)
        rep.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
    End Sub

    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub

End Class

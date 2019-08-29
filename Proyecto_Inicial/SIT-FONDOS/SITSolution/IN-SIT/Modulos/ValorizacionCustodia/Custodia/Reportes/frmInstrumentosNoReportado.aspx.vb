Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_ValorizacionCustodia_Custodia_Reportes_frmInstrumentosNoReportado
    Inherits BasePage
    Dim rep As New ReportDocument
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try            
            Dim oInstNoreg As New InstNoreg
            Dim nFechaOperacion As Long = Request.QueryString("nFechaOperacion")
            Dim sPortafolioCodigo As String = Request.QueryString("sPortafolioCodigo")
            Dim sCodigoCustodio As String = Request.QueryString("sCodigoCustodio")
            Dim sNombreCustodio As String = Request.QueryString("sNombreCustodio")
            Dim sFechaOperacion As String = nFechaOperacion
            sFechaOperacion = sFechaOperacion.Substring(6, 2) & "/" & sFechaOperacion.Substring(4, 2) & "/" & sFechaOperacion.Substring(0, 4)
            rep.Load(Server.MapPath("InstrumentosNoReportados.rpt"))
            Dim oInstNoregTMP As DataSet = New CustodioArchivoBM().InstrumentosNoReportados(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
            CopiarTabla(oInstNoregTMP.Tables(0), oInstNoreg.InstrumentosNoRegistrados)
            rep.SetDataSource(oInstNoreg)
            crNoRegistrados.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
            crNoRegistrados.ReportSource = rep
            rep.SetParameterValue("Usuario", MyBase.Usuario)
            rep.SetParameterValue("FechaOperacion", sFechaOperacion)
            rep.SetParameterValue("Portafolio", sPortafolioCodigo)
            rep.SetParameterValue("Custodio", sCodigoCustodio & " - " & sNombreCustodio)
            rep.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al momento de cargar el reporte")
        End Try
    End Sub
    Protected Sub Modulos_ValorizacionCustodia_Custodia_Reportes_frmInstrumentosNoReportado_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        rep.Close()
        rep.Dispose()
    End Sub
End Class
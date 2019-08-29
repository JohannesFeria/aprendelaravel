Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_ValorizacionCustodia_Custodia_Reportes_frmInstrumentosConciliados
    Inherits BasePage
    Dim rep As New ReportDocument
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try           
            Dim oInstCons As New InstCons
            Dim nFechaOperacion As Long = Request.QueryString("nFechaOperacion")
            Dim sPortafolioCodigo As String = Request.QueryString("sPortafolioCodigo")
            Dim sCodigoCustodio As String = Request.QueryString("sCodigoCustodio")
            Dim sNombreCustodio As String = Request.QueryString("sNombreCustodio") 'RGF 20101103 INC 61546
            Dim sFechaOperacion As String = nFechaOperacion
            sFechaOperacion = sFechaOperacion.Substring(6, 2) & "/" & sFechaOperacion.Substring(4, 2) & "/" & sFechaOperacion.Substring(0, 4)
            rep.Load(Server.MapPath("InstrumentosConciliados.rpt"))
            Dim oInstConsTMP As DataSet = New CustodioArchivoBM().InstrumentosConciliados(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
            CopiarTabla(oInstConsTMP.Tables(0), oInstCons.InstrumentosConciliados)
            rep.SetDataSource(oInstCons)
            crConciliados.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
            crConciliados.ReportSource = rep
            rep.SetParameterValue("Usuario", MyBase.Usuario)
            rep.SetParameterValue("FechaOperacion", sFechaOperacion)
            rep.SetParameterValue("Portafolio", sPortafolioCodigo) 'RGF 20101102 INC 61546
            rep.SetParameterValue("Custodio", sCodigoCustodio & " - " & sNombreCustodio) 'RGF 20101103 INC 61546
            rep.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página de impresión")
        End Try
    End Sub
    Protected Sub Modulos_ValorizacionCustodia_Custodia_Reportes_frmInstrumentosConciliados_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        rep.Close()
        rep.Dispose()
    End Sub
End Class
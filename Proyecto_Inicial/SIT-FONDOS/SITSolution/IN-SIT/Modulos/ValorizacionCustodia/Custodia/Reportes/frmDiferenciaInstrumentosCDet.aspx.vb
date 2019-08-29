Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_ValorizacionCustodia_Custodia_Reportes_frmDiferenciaInstrumentosCDet
    Inherits BasePage
    Dim rep As New ReportDocument
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim nFechaOperacion As Long = Request.QueryString("nFechaOperacion")
            Dim sPortafolioCodigo As String = Request.QueryString("sPortafolioCodigo")
            Dim sPortafolioDescripcion As String = Request.QueryString("sPortafolioDescripcion")
            Dim sCodigoCustodio As String = Request.QueryString("sCodigoCustodio")
            Dim sNombreCustodio As String = Request.QueryString("sNombreCustodio") 'RGF 20101103 INC 61546
            Dim sFechaOperacion As String = nFechaOperacion
            sFechaOperacion = sFechaOperacion.Substring(6, 2) & "/" & sFechaOperacion.Substring(4, 2) & "/" & sFechaOperacion.Substring(0, 4)            
            Dim oInstDife As New InstDifeCDet
            'rep.Load(Server.MapPath("DiferenciasCDet.rpt"))
            rep.Load(Server.MapPath("DiferenciasCDetNew.rpt"))
            Dim oInstDifeTMP As DataSet = New CustodioArchivoBM().InstrumentosDiferenciasCDet(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
            CopiarTabla(oInstDifeTMP.Tables(0), oInstDife.Custodios)
            CopiarTabla(oInstDifeTMP.Tables(1), oInstDife.CabeceraDiferencias)
            CopiarTabla(oInstDifeTMP.Tables(2), oInstDife.DetalleDiferencias)
            rep.SetDataSource(oInstDife)
            rep.SetParameterValue("Usuario", MyBase.Usuario)
            rep.SetParameterValue("FechaOperacion", sFechaOperacion)
            rep.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
            crDiferencias.ReportSource = rep
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página de Impresión")
        End Try
    End Sub
    Protected Sub Modulos_ValorizacionCustodia_Custodia_Reportes_frmDiferenciaInstrumentosCDet_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        rep.Close()
        rep.Dispose()
    End Sub
End Class

Partial Class Modulos_Inversiones_Reportes_Consolidado_de_Posiciones_frmVisorReporteLimitePDF
    Inherits BasePage

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                VisualizaLimitesPDF()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub VisualizaLimitesPDF()
        Dim sRutaFilePDF As String
        Dim sArchivoPDF As String

        sRutaFilePDF = Session("RutaFilePDF")
        sArchivoPDF = Session("ArchivoPDF")
        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("Content-Disposition", "inline; filename=" + sArchivoPDF)
        Response.WriteFile(sRutaFilePDF)
        Response.End()
    End Sub

End Class

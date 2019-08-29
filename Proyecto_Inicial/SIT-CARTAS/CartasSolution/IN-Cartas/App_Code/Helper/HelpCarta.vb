Imports Microsoft.VisualBasic
Imports System.Data
Imports System.IO
Imports System.Diagnostics
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Public Class HelpCarta
    Public Shared Function CrearMultiCartaPDF(ByVal strCartas As String) As String
        Dim strDirecctorio As String = ""
        Dim strNombreArchivo As String
        Dim arrCartas As String()
        Dim oPdfWriter As PdfWriter
        Dim oDocument As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4)
        Try
            arrCartas = strCartas.Split(New Char() {"&"})
            If arrCartas.Length > 0 Then strDirecctorio = System.IO.Path.GetDirectoryName(arrCartas(0))
            strNombreArchivo = strDirecctorio & "\" & System.Guid.NewGuid().ToString() & ".pdf"
            oPdfWriter = PdfWriter.GetInstance(oDocument, New FileStream(strNombreArchivo, FileMode.Create))
            oDocument.Open()
            For Each strCartaPDF As String In arrCartas
                If File.Exists(strCartaPDF) Then
                    oPdfWriter.DirectContent.AddTemplate(oPdfWriter.GetImportedPage(New PdfReader(strCartaPDF), 1), 0, -10.0F)
                    oDocument.NewPage()
                End If
            Next
            oDocument.Close()
            oPdfWriter.Close()
            Return strNombreArchivo
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
Imports System.IO

Partial Class Modulos_Tesoreria_OperacionesCaja_frmVisorAprobOperacionesCaja
    Inherits System.Web.UI.Page


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strNC As String
        strNC = Request.QueryString("archivo")

        Dim strFl As New System.IO.FileStream(strNC, FileMode.Open)
        Dim bytes() As Byte = New Byte(strFl.Length) {}

        strFl.Read(bytes, 0, strFl.Length)
        strFl.Flush()
        strFl.Close()
        Response.Clear()
        Response.Buffer = True

        If strNC.IndexOf(".pdf") > -1 Then
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-Disposition", "inline;filename=" + "Carta.pdf")
        End If

        Response.BinaryWrite(bytes)
        Response.Flush()
        Response.End()
    End Sub
End Class

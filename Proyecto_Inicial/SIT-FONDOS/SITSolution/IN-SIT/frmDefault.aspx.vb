
Partial Class frmDefault
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If (Session("UInfo_CodUsuario") Is Nothing) Then
                If (Session("AppError_Msj") IsNot Nothing) Then
                    hiddenMsje.Value = Session("AppError_Msj").ToString()
                    Session.Remove("AppError_Msj")
                    hiddenUrlLogin.Value = ConfigurationManager.AppSettings("APP_URL_LOGIN")
                    hiddenMsje.Value = Replace(hiddenMsje.Value, "&oacute;", "ó")
                    Dim fineshedSession As New StringBuilder
                    fineshedSession.Append("<table style=""width: 100%; text-align: center; font-weight: bold;"">")
                    fineshedSession.Append("<tr><td><img src=""App_Themes/img/logo.jpg"" /></td></tr>")
                    fineshedSession.Append("<tr><td>" & hiddenMsje.Value & "</td></tr>")
                    fineshedSession.Append("</table><br/>")
                    EjecutarJS("ShowMessage('" & fineshedSession.ToString() & "');")
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btnEjecutar_Click(sender As Object, e As System.EventArgs) Handles btnEjecutar.Click
        Response.Write("<script>top.location='" & hiddenUrlLogin.Value & "';parent.location='" & hiddenUrlLogin.Value & "';</script>")
    End Sub

    Protected Sub EjecutarJS(ByVal strJS As String, Optional ByVal addScriptTags As Boolean = True)
        Dim sGUID As String = System.Guid.NewGuid.ToString()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), sGUID, strJS, addScriptTags)
    End Sub

End Class

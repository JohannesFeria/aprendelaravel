Partial Class frmTokenTarget
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("tokses") IsNot Nothing Then
            If Request.QueryString("tokses").ToString.Length > 0 Then
                Session("AppSecurity_Token") = Request.QueryString("tokses").ToString
                Session.Remove("UInfo_CodUsuario")
                Response.Redirect("frmPrincipal.aspx")
            End If
        Else
            HttpContext.Current.Session("AppError_Msj") = "Ha ocurrido un error inesperado. Se recomienda autenticarse nuevamente mediante el 'Sistema de Seguridad'"
            Response.Redirect("frmError.aspx")
        End If
    End Sub

End Class

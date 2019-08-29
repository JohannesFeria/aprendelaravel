Partial Class frmError
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Session("AppError_Msj") IsNot Nothing Then
        '    Dim errMsj As String = Session("AppError_Msj").ToString
        '    If TypeOf Session("AppError_Obj") Is Exception Then
        '        Dim exOri As Exception = Session("AppError_Obj")
        '        If exOri.InnerException IsNot Nothing Then errMsj += ". InnerException:: " & exOri.InnerException.Message
        '    End If
        '    lblMensaje.Text = errMsj
        '    hiddenUrlLogin.Value = ConfigurationManager.AppSettings("APP_URL_LOGIN")
        '    'Dim nuevoTR As String = "<tr><td></td><td align='left' width='91%'><P>{0}</P></td></tr></tbody>"
        '    'nuevoTR = String.Format(nuevoTR, errMsj)

        '    'Dim script As String = "var tabla = document.getElementById('tblError');"
        '    'script += "tabla.innerHTML = tabla.innerHTML.replace(/<\/tbody>/g, """ & nuevoTR & """);"
        '    ''txt += "alert(tabla.innerHTML.replace(/<\/tbody>/g, """ & scriptMsj & """));"

        '    'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertify", script, True)
        '    Session.Remove("AppError_Msj")
        '    Session.Remove("AppError_Obj")
        'End If

        ' Crear error mensaje constante.
        Dim generalErrorMsg As String = "Se ha producido un problema en este sitio web. Inténtalo de nuevo. " +
            "Si este error continúa, póngase en contacto con el soporte técnico."
        Dim httpErrorMsg As String = "Se produjo un error HTTP. Página no encontrada. Inténtalo de nuevo."
        Dim unhandledErrorMsg As String = "El error no fue manejado por el código de la aplicación."
        hiddenUrlLogin.Value = ConfigurationManager.AppSettings("APP_URL_LOGIN")
        ' Display safe error message.
        FriendlyErrorMsg.Text = generalErrorMsg

        ' Determine where error was handled.
        Dim errorHandler As String = Request.QueryString("handler")
        If errorHandler Is Nothing Then errorHandler = "Error Page"

        ' Get the last error from the server.
        Dim ex As Exception = Server.GetLastError()

        ' Get the error number passed as a querystring value.
        Dim errorMsg As String = Request.QueryString("msg")
        If (errorMsg = "404") Then
            ex = New HttpException(404, httpErrorMsg, ex)
            FriendlyErrorMsg.Text = ex.Message
        End If
        ' If the exception no longer exists, create a generic exception.
        If (ex Is Nothing) Then ex = New Exception(unhandledErrorMsg)

        ' Show error details to only you (developer). LOCAL ACCESS ONLY.
        'If Request.IsLocal Then

        ' Detalle del mensaje Error.
        ErrorDetailedMsg.Text = ex.Message

        ' Muestra donde fue manejado el error.
        lblErrorHandler.Text = errorHandler

        'Muestra local acceso detalle.
        DetailedErrorPanel.Attributes.Add("style", "display:block")

        If (Not ex.InnerException Is Nothing) Then
            InnerMessage.Text = ex.GetType().ToString() + "<br/>" +
                ex.InnerException.Message
            InnerTrace.Text = ex.InnerException.StackTrace

        Else

            InnerMessage.Text = ex.GetType().ToString()
            If (Not ex.StackTrace Is Nothing) Then

                InnerTrace.Text = ex.StackTrace.ToString().TrimStart()
            End If
        End If
        'End If

        ' Limpiar error del server.
        Server.ClearError()

    End Sub
End Class

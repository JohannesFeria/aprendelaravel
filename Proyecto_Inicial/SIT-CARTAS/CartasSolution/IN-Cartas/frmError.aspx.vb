Partial Class frmError
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AppError_Msj") IsNot Nothing Then
            Dim errMsj As String = Session("AppError_Msj").ToString
            If TypeOf Session("AppError_Obj") Is Exception Then
                Dim exOri As Exception = Session("AppError_Obj")
                If exOri.InnerException IsNot Nothing Then errMsj += ". InnerException:: " & exOri.InnerException.Message
            End If

            Dim nuevoTR As String = "<tr><td></td><td align='left' width='91%'><P>{0}</P></td></tr></tbody>"
            nuevoTR = String.Format(nuevoTR, errMsj)

            Dim script As String = "var tabla = document.getElementById('tblError');"
            script += "tabla.innerHTML = tabla.innerHTML.replace(/<\/tbody>/g, """ & nuevoTR & """);"
            'txt += "alert(tabla.innerHTML.replace(/<\/tbody>/g, """ & scriptMsj & """));"

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertify", script, True)
        End If
    End Sub
End Class

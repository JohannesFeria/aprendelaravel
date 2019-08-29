Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Web.Configuration
Imports ws_seguridad
Partial Class frmPrincipal
    Inherits System.Web.UI.Page
    Private Shared _Msj_ErrorSesion As String = "La sesión no ha cumplido con el proceso de autenticación. Se recomienda autenticarse nuevamente mediante el 'Sistema de Seguridad'"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Generamos una excepcion si no se encuentran los datos generados por la autenticación
            If Session("AppSecurity_Token") Is Nothing Then Throw New System.Exception(_Msj_ErrorSesion)
            'Obtenemos los datos de autenticación solo si aun no hemos realizado ninguna carga previa
            If HttpContext.Current.Session("UInfo_CodUsuario") Is Nothing Then
                'Dim ws As New ws_seguridad.SeguridadImplService
                Dim ws As New ws_seguridad.SeguridadImpl 'Codigo correcto para seguridad INTEGRA
                Dim info As ws_seguridad.ResultObtenerInfoSesionBean
                info = ws.ObtenerInfoSesion(Session("AppSecurity_Token").ToString)
                If info Is Nothing Then Throw New Exception("No es posible obtener datos válidos para esta sesión.")
                HttpContext.Current.Session("UInfo_CodUsuario") = info.codUsuario
                HttpContext.Current.Session("UInfo_FechaInicioSesion") = info.fechaInicioSesionTexto
                HttpContext.Current.Session("UInfo_NombreUsuario") = info.nombreUsuario
                HttpContext.Current.Session("UInfo_NombreRol") = info.nombreRol
            End If
        Catch ex As System.Exception 'Manejar el Error
            HttpContext.Current.Session("AppError_Msj") = ex.Message
            HttpContext.Current.Session("AppError_Obj") = ex
            Response.Redirect("frmError.aspx")
        End Try
    End Sub
End Class
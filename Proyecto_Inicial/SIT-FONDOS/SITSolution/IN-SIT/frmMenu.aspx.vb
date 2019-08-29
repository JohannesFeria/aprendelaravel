Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports System.Collections.Generic
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Web.Configuration
Imports ws_seguridad

Partial Class frmMenu
    Inherits System.Web.UI.Page

    Private Shared _Msj_ErrorApp As String = "El sistema ha encontrado una situación inesperada. Se recomienda autenticarse nuevamente mediante el 'Sistema de Seguridad'"
    Private Shared _Msj_ErrorSesion As String = "La sesión no ha cumplido con el proceso de autenticación. Se recomienda autenticarse nuevamente mediante el 'Sistema de Seguridad'"

    Private Shared _userCodigo As String

    <WebMethod()>
    Public Shared Function ListarMenu() As Object
        Dim msjErrParaPag As String = _Msj_ErrorApp
        Try
            Dim token As Object = HttpContext.Current.Session("AppSecurity_Token")

            'Generamos una excepcion si no se encuentran los datos generados por la autenticación
            If token Is Nothing Then
                msjErrParaPag = _Msj_ErrorSesion
                Throw New System.Exception(_Msj_ErrorSesion)
            End If

            'Obtenemos los datos para el menu (desde el Web Service)
            'Dim ws As New ws_seguridad.SeguridadImplService
            Dim ws As New ws_seguridad.SeguridadImpl 'Codigo correcto para seguridad INTEGRA

            Dim listaResult() As ws_seguridad.ResultListarMenuAplicativoBean
            listaResult = ws.ListarMenuAplicativo(token.ToString)
            If listaResult Is Nothing Then Throw New Exception("No es posible obtener datos válidos para esta sesión.")

            Dim listaMenu As New List(Of MnMenuAccesoBE)
            Dim entMenu As MnMenuAccesoBE

            'Ahora realizamos un mapeo con las entidades que reconocerá el menú dinámico
            For Each entMenuWS As ws_seguridad.ResultListarMenuAplicativoBean In listaResult
                entMenu = New MnMenuAccesoBE

                entMenu.id = entMenuWS.codOpcionMenu
                entMenu.pId = entMenuWS.codOpcionMenuPadre
                entMenu.name = entMenuWS.tituloOpcionMenu
                entMenu.file = entMenuWS.url
                entMenu.open = entMenuWS.open

                listaMenu.Add(entMenu)
            Next

            Return listaMenu
        Catch ex As System.Exception 'Manejar el Error
            HttpContext.Current.Session("AppError_Msj") = ex.Message
            HttpContext.Current.Session("AppError_Obj") = ex

            Return msjErrParaPag
        End Try
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Configuramos el URL_Login para cuando desee salir de la sessión
            Me.hiddenUrlLogin.Value = "emptyPage.html"
            Dim urlLogin As Object = ConfigurationManager.AppSettings("APP_URL_LOGIN")
            If urlLogin IsNot Nothing Then Me.hiddenUrlLogin.Value = urlLogin.ToString

            'Mostramos los datos del usuario de la sesión actual (si existe)
            If Session("UInfo_CodUsuario") IsNot Nothing Then
                lblUsuario.Text = Session("UInfo_CodUsuario") & " - " & Session("UInfo_NombreUsuario")
            End If
        End If
    End Sub

    'Public Shared ReadOnly Property userCodigo() As String
    '    Get
    '        _userCodigo = HttpContext.Current.Session("UInfo_CodUsuario")

    '        '_userCodigo = HttpContext.Current.User.Identity.Name
    '        'If HttpContext.Current.Session("userCod") = "" Or HttpContext.Current.Session("userCod") Is Nothing Then
    '        '    'HttpContext.Current.Session("userCod") = Request.ServerVariables("Remote_User").Substring(0)
    '        '    HttpContext.Current.Session("userCod") = HttpContext.Current.User.Identity.Name
    '        '    _userCodigo = HttpContext.Current.Session("userCod")
    '        '    'HttpContext.Current.Session("userCod") = User.Identity.Name
    '        '    'HttpContext.Current.Session("userCod") = Environment.GetEnvironmentVariable("username")
    '        'Else
    '        '    _userCodigo = HttpContext.Current.Session("userCod")
    '        'End If
    '        Return _userCodigo
    '    End Get
    'End Property

End Class

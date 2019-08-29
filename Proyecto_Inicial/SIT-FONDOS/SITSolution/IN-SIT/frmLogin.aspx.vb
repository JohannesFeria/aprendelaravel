Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports System.Collections.Generic
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Web.Configuration
Imports System.Data

Partial Class frmLogin
    Inherits BasePage
    Private Shared _userCodigo As String
    Private Shared _userPassword As String

    Public Shared Property userCodigo() As String
        Get
            Return _userCodigo
        End Get
        Set(ByVal value As String)
            _userCodigo = value
        End Set
    End Property

    Public Property userPassword() As String
        Get
            Return _userPassword
        End Get
        Set(ByVal value As String)
            _userPassword = value
        End Set
    End Property

    Protected Sub btnIngresar_Click(sender As Object, e As System.EventArgs) Handles btnIngresar.Click
        Try

            If validarLogin() = False Then
                Exit Sub
            End If
            Response.Redirect("frmPrincipal.aspx")

        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Private Function validarLogin() As Boolean

        'If Len(Me.txtPassword.Text.Trim) < 6 Then
        '    AlertaJS("La Contraseña tiene que ser mayor que 6 caracteres")
        '    Return False
        'End If

        Dim usuario As New MnMenuAccesoBM
        userCodigo = Me.txtUsuario.Text.Trim
        userPassword = Me.txtPassword.Text.Trim
        Dim tUsuario = usuario.ValidarUsuario(userCodigo, userPassword)

        If tUsuario.Rows.Count > 0 Then
            Session("usuario") = userCodigo
            Session("nombre") = tUsuario.Rows(0)(1).ToString()
        Else
            AlertaJS("Usuario o Clave incorrectos")
            Return False
        End If

        Return True
    End Function
End Class

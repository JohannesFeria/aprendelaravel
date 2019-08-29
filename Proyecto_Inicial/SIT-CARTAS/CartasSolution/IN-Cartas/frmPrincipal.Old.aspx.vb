Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Web.Configuration
Partial Class frmPrincipalOld
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Aprobador
            Session("UInfo_CodUsuario") = "P500108"
            Session("UInfo_NombreUsuario") = "P500108"
            'Firmante 1
            'Session("UInfo_CodUsuario") = "P500611"
            'Session("UInfo_NombreUsuario") = "P500611"
            'Firmante 2
            'Session("UInfo_CodUsuario") = "P500629"
            'Session("UInfo_NombreUsuario") = "P500629"
            If Session("UInfo_CodUsuario") Is Nothing Then
                Session("UInfo_CodUsuario") = "ADMIN_TEST"
                Session("UInfo_NombreUsuario") = "ADMIN_TEST"
            End If
            If Session("UInfo_CodUsuario") Is Nothing Then
                Response.Redirect("frmError.aspx")
                Exit Sub
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class
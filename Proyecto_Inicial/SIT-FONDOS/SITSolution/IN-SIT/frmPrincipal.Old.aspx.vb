Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports System.Collections.Generic
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Web.Configuration
Partial Class frmPrincipalOld
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Session("UInfo_CodUsuario") = "P500625" ' Probando con el usuario LISETH
            Session("UInfo_NombreUsuario") = "P500625" ' Probando con el usuario LISETH
            'Session("UInfo_CodUsuario") = "P500678" ' Probando con el usuario LISETH
            'Session("UInfo_NombreUsuario") = "P500678" ' Probando con el usuario LISETH
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
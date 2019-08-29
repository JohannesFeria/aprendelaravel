Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Inversiones_frmCuponesPorVencer
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not IsPostBack Then
                CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub CargarGrilla()
        Dim dtCuponesPorVencer As DataTable = Session("CuponesPorVencer")
        dgCupones.DataSource = dtCuponesPorVencer
        dgCupones.DataBind()
    End Sub

End Class

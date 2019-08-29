Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.IO
Partial Class Modulos_Parametria_Tablas_Valores_frm_ModificacionIsinNemonico
    Inherits BasePage
    Dim oValoresBM As New ValoresBM
    Sub ActualizaCaja()
        If ddlTipoAct.SelectedValue = "1" Then
            txtMnemonico.Enabled = True
            txtISIN.Enabled = False
        Else
            txtMnemonico.Enabled = False
            txtISIN.Enabled = True
        End If
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Session("SS_DatosModal") Is Nothing Then
            Dim datos As String() = CType(Session("SS_DatosModal"), String())
            txtISIN.Text = datos(0)
            txtMnemonico.Text = datos(1)
            btnGenera.Enabled = True
            ActualizaCaja()
            Session.Remove("SS_DatosModal")
        End If
    End Sub
    Protected Sub btnGenera_Click(sender As Object, e As System.EventArgs) Handles btnGenera.Click
        Try
            oValoresBM.ActualizaNemomicoIsin(ddlTipoAct.SelectedValue, txtMnemonico.Text, txtISIN.Text)
            AlertaJS("Se realizaron todas las actualizacion correspondientes, correctamente.")
        Catch ex As Exception
            AlertaJS (Replace (ex.Message,"'",""))
        End Try
    End Sub
    Protected Sub ddlTipoAct_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTipoAct.SelectedIndexChanged
        ActualizaCaja()
    End Sub
End Class
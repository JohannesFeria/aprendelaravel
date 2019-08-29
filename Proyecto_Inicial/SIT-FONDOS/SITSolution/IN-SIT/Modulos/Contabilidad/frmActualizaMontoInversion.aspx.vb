Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Partial Class Modulos_Contabilidad_frmActualizaMontoInversion
    Inherits BasePage
    Protected Sub btnCalculo_Click(sender As Object, e As System.EventArgs) Handles btnCalculo.Click
        Try
            Dim o As New OrdenPreOrdenInversionBM
            o.RecalculaMontoInversion("US38500P2083", "7", 20160627)
        Catch ex As Exception
            lblMensaje.Text = ex.Message
        End Try
    End Sub
End Class

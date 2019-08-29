Imports Sit.BusinessLayer
Imports System.Data
'OT 10090 - 31/07/2017 - Carlos Espejo
'Descripcion: Se ordeno el formulario
Partial Class Modulos_Inversiones_frmAprobarExcesosTrader
    Inherits BasePage
    Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
    Dim ds As New DataSet
    Dim codigoPrevOrden As Decimal
    Dim estado As String
    Dim listaCodigoPrevOrden As Array
    Dim bolValidaAprob As Boolean = False
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                If Not Request.QueryString("CodigoPrevOrden") Is Nothing And _
                    Not Request.QueryString("Usuario") Is Nothing And _
                    Not Request.QueryString("TipoRenta") Is Nothing Then
                    listaCodigoPrevOrden = Request.QueryString("CodigoPrevOrden").Split(",")
                    For i As Integer = 0 To listaCodigoPrevOrden.Length - 1
                        codigoPrevOrden = CType(listaCodigoPrevOrden.GetValue(i).ToString(), Decimal)
                        bolValidaAprob = oPrevOrdenInversionBM.ValidarAprobacion(codigoPrevOrden, ds)
                        If bolValidaAprob = True Then
                            oPrevOrdenInversionBM.AprobarNegociacionExcesosTrader(ds, Request.QueryString("Usuario"), codigoPrevOrden, Request.QueryString("TipoRenta"))
                        End If
                    Next
                    If bolValidaAprob Then
                        lbEstado.Text = "La aprobación de excesos por trader se ha realizado satisfactoriamente! "
                    Else
                        AlertaJS("El proceso de aprobación de excesos por trader ya se realizo anteriormente, verifíque el estado del registro previo!", "window.close();")
                    End If

                End If
            End If
        Catch ex As Exception
            AlertaJS("Error en el proceso de aprobación de la negociación, intentelo mas tarde!")
        End Try
    End Sub
End Class

Imports System.Data
Imports Sit.BusinessLayer

Partial Class Modulos_ValorizacionCustodia_Valorizacion_frmDiferenciaValorCuota
    Inherits BasePage
    '======= Origen : Creación                                     ======
    '======= Fecha Creación: 04/10/2017                            ======
    '======= Autor: Jorge Luis Benites Del Aguila - GMD            ======
    '======= Orden de trabajo: OT10902                             ======
    '======= Descripción: Pantalla de diferencia del valor cuota   ======
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Me.dgListaDif.DataSource = Nothing
                Me.dgListaDif.DataBind()
                Dim Fecha As Decimal
                Fecha = UIUtility.ConvertirFechaaDecimal(Request.QueryString("Fecha"))
                Dim dtblDatos As DataTable = New CarteraTituloValoracionBM().ObtenerDiferenciaReporteVL(Fecha, 0)
                If Not (dtblDatos Is Nothing) Then
                    Me.lblCantidadDif.Text = String.Format("({0})", dtblDatos.Rows.Count)
                Else
                    Me.lblCantidadDif.Text = String.Format("({0})", 0)
                End If
                Me.dgListaDif.DataSource = dtblDatos
                Me.dgListaDif.DataBind()
                ViewState("OIEjecutadas") = dtblDatos
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
End Class

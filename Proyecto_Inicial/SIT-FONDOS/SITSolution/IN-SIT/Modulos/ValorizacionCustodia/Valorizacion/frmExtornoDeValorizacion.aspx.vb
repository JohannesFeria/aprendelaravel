Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Valorizacion_y_Custodia_Valorizacion_frmExtornoDeValorizacion
    Inherits BasePage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            Dim ovalores As New ValoresBM
            If Not Page.IsPostBack Then
                CargarPortafolio()
                Me.tbFechaOperacion.Text = ovalores.ObtenerUltimaFechaValorizacion(ddlPortafolio.SelectedValue)
                ViewState("fechaOperacion") = tbFechaOperacion.Text
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la carga de la página")
        End Try
    End Sub
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim dsPortafolio = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolio.Items.Clear()
            ddlPortafolio.DataSource = dsPortafolio
            ddlPortafolio.DataValueField = "CodigoPortafolio"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            Dim oValores As New ValoresBM
            Dim iCantidad As Integer
            Dim fechaAExtornar As String = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) 'CRumiche: Simplificación

            iCantidad = oValores.VerificarExtornoValorizacionCartera(ddlPortafolio.SelectedValue, fechaAExtornar, DatosRequest)
            If iCantidad > 0 Then
                oValores.ExtornarValorizacionCartera(ddlPortafolio.SelectedValue, fechaAExtornar, DatosRequest)
                oValores.BorraMontoInversion(fechaAExtornar, ddlPortafolio.SelectedValue)
                Me.tbFechaOperacion.Text = oValores.ObtenerUltimaFechaValorizacion(ddlPortafolio.SelectedValue)
                ViewState("fechaOperacion") = tbFechaOperacion.Text
                AlertaJS(ObtenerMensaje("ALERT103"))
            Else
                AlertaJS(ObtenerMensaje("ALERT142"))
            End If

            ' INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-09-11 | Eliminar la Valorización Amortizada
            Dim oValorizacionBM As New ValorizacionAmortizadaBM
            oValorizacionBM.EliminarValorizacion(ddlPortafolio.SelectedValue, fechaAExtornar)
            ' FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-09-11 | Eliminar la Valorización Amortizada

        Catch ex As Exception
            AlertaJS("Ocurrió un error al exportar")
        End Try
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
            Dim strFecha As String
            strFecha = oCarteraTituloValoracion.ObtenerFechaValoracion(ddlPortafolio.SelectedValue.ToString, "N", True)
            If strFecha Is DBNull.Value Or strFecha = "" Then
                Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue.ToString))
            Else
                Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(strFecha))
            End If
            ViewState("fechaOperacion") = tbFechaOperacion.Text
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el portafolio")
        End Try
    End Sub
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de página")
        End Try
    End Sub
    Protected Sub tbFechaOperacion_TextChanged(sender As Object, e As System.EventArgs) Handles tbFechaOperacion.TextChanged
        Try
            If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) <> UIUtility.ConvertirFechaaDecimal(ViewState("fechaOperacion")) Then
                AlertaJS("La fecha de operación de reversión no coincide con la fecha de la última valorización: " + ViewState("fechaOperacion").ToString)
                Me.tbFechaOperacion.Text = ViewState("fechaOperacion")
            End If
        Catch ex As Exception
            AlertaJS(Replace("Ocurrió un error en el cambio de fecha de operación / " + ex.Message.ToString, "'", String.Empty))
        End Try
    End Sub
End Class
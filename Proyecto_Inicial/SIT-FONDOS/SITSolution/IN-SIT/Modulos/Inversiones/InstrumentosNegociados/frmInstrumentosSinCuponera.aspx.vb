Imports System.Data
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer

Partial Class Modulos_Inversiones_InstrumentosNegociados_frmInstrumentosSinCuponera
    Inherits BasePage

#Region "Eventos de la página"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarInstrumento()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim codigoOrden As String
            Dim codigoPortafolio As String
            codigoOrden = Request.QueryString("codigoOrden")
            codigoPortafolio = Request.QueryString("codigoPortafolio")
            Dim ordenInversionBM As New OrdenPreOrdenInversionBM
            If ordenInversionBM.confirmarInstrumentosSinCuponera(codigoOrden, codigoPortafolio, DatosRequest) Then
                AlertaJS("Se Confirmó la orden correctamente")
            Else
                AlertaJS("Ocurrió unos problemas en la confirmación. Por favor inténtelo nuevamente")
            End If
            EjecutarJS("window.close();")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

#End Region

#Region "Métodos y funciones de la página"

    Private Sub cargarInstrumento()
        Dim codigoOrden As String
        Dim codigoPortafolio As String
        codigoOrden = Request.QueryString("codigoOrden")
        codigoPortafolio = Request.QueryString("codigoPortafolio")

        Dim dt As DataTable
        Dim OrdenInversionBM As New OrdenPreOrdenInversionBM

        dt = OrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, codigoPortafolio)

        If dt.Rows.Count > 0 Then
            Me.txtCodigoNemonico.Text = dt.Rows(0)("CodigoMnemonico")
            Me.txtCodigoSBS.Text = dt.Rows(0)("CodigoSBS")
            Me.txtCodigoIsin.Text = dt.Rows(0)("CodigoISIN")
            Me.txtFechaOperacion.Text = UIUtility.ConvertirFechaaString(dt.Rows(0)("FechaOperacion").ToString)
            Me.txtFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(dt.Rows(0)("FechaLiquidacion").ToString)
            Me.txtFechaVencimiento.Text = UIUtility.ConvertirFechaaString(dt.Rows(0)("FechaContrato").ToString)
            Me.txtNumeroUnidades.Text = dt.Rows(0)("CantidadOperacion").ToString
            Me.txtMontoOperacion.Text = dt.Rows(0)("MontoOperacion").ToString
        End If
        inactivarControles(False)
    End Sub

    Private Sub inactivarControles(bool As Boolean)
        Me.txtCodigoNemonico.Enabled = bool
        Me.txtCodigoSBS.Enabled = bool
        Me.txtCodigoIsin.Enabled = bool
        Me.txtFechaOperacion.Enabled = bool
        Me.txtFechaLiquidacion.Enabled = bool
        Me.txtFechaVencimiento.Enabled = bool
        Me.txtNumeroUnidades.Enabled = bool
        Me.txtMontoOperacion.Enabled = bool
    End Sub

#End Region

End Class

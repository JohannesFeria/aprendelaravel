Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_Gestion_Reportes_frmDetalleForward
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If (Page.IsPostBack = False) Then
                If Not (Request.QueryString.Item("cp") Is Nothing) Then
                    txtPortafolio.Value = Request.QueryString.Item("cp")
                End If
                If Not (Request.QueryString.Item("pd") Is Nothing) Then
                    txtFechaOperacion.Value = Request.QueryString.Item("pd")
                End If
                If Not (Request.QueryString.Item("id") Is Nothing) Then
                    txtConsecutivo.Value = Request.QueryString.Item("id")
                    LoadForward(txtPortafolio.Value, Convert.ToInt32(txtFechaOperacion.Value), Convert.ToInt32(txtConsecutivo.Value))
                Else
                    txtConsecutivo.Value = "0"
                End If
                LoadMoneda()
                cargarIndicadorMovimiento()
                cargarTipoForward()
                cargarModalidad()
                indicadorCaja()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                txtCodigoSBS.Text = CType(Session("SS_DatosModal"), String())(0)
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Sub LoadMoneda()
        UIUtility.CargarMonedaOI(ddlMoneda)
    End Sub

    Public Sub cargarIndicadorMovimiento()
        Dim dtIndicadorMovimiento As DataTable = New ParametrosGeneralesBM().Listar("IndMov", Nothing)
        HelpCombo.LlenarComboBox(ddlIndicador, dtIndicadorMovimiento, "Nombre", "Valor", False)
    End Sub

    Public Sub cargarTipoForward()
        Dim dtTipoForward As DataTable = New ParametrosGeneralesBM().Listar("TipoFwd", Nothing)
        HelpCombo.LlenarComboBox(ddlTipo, dtTipoForward, "Nombre", "Valor", False)
    End Sub

    Public Sub cargarModalidad()
        Dim dtModalidad As DataTable = New ParametrosGeneralesBM().Listar("Modalid", Nothing)
        HelpCombo.LlenarComboBox(ddlModalidad, dtModalidad, "Nombre", "Valor", False)
    End Sub

    Public Sub indicadorCaja()
        Dim dtIndicadorCaja As DataTable = New ParametrosGeneralesBM().Listar("IndCaja", Nothing)
        HelpCombo.LlenarComboBox(ddlIndicadorCaja, dtIndicadorCaja, "Nombre", "Valor", False)
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        InsertOrUpdate()
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("frmSeguimientoForwards.aspx")
    End Sub
    Private Sub InsertOrUpdate()
        Dim reporteGestion As New ReporteGestionBM
        Dim result As Boolean
        Try

            Dim dt As New DataTable

            If (txtConsecutivo.Value = "0") Then
                result = reporteGestion.InsertForward(txtPortafolio.Value, txtFechaOperacion.Value, txtCodigoSBS.Text, ddlMoneda.SelectedValue, _
                ddlIndicador.SelectedValue, ddlTipo.SelectedValue, Convert.ToDecimal(txtMonto.Text), _
                Convert.ToDecimal(txtPrecio.Text), UIUtility.ConvertirFechaaDecimal(txtFechaVencimiento.Text), _
                Convert.ToInt32(txtPlazoVencimiento.Text), ddlModalidad.SelectedValue, Convert.ToDecimal(txtTipoCambio.Text), _
                 ddlIndicadorCaja.SelectedValue, txtPlaza.Text, DatosRequest)
            Else
                result = reporteGestion.UpdateForward(txtPortafolio.Value, txtFechaOperacion.Value, Convert.ToInt32(txtConsecutivo.Value), txtCodigoSBS.Text, ddlMoneda.SelectedValue, _
                                ddlIndicador.SelectedValue, ddlTipo.SelectedValue, Convert.ToDecimal(txtMonto.Text), _
                                Convert.ToDecimal(txtPrecio.Text), UIUtility.ConvertirFechaaDecimal(txtFechaVencimiento.Text), _
                                Convert.ToInt32(txtPlazoVencimiento.Text), ddlModalidad.SelectedValue, Convert.ToDecimal(txtTipoCambio.Text), _
                                 ddlIndicadorCaja.SelectedValue, txtPlaza.Text, DatosRequest)
            End If
            If (result) Then
                AlertaJS("Los datos se registraron correctamente.")
                btnAceptar.Enabled = False
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub LoadForward(ByVal codigoPortafolio As String, ByVal fechaOperacion As Integer, ByVal consecutivo As Integer)
        Dim reporteGestion As New ReporteGestionBM
        Try
            Dim dt As New DataTable

            dt = reporteGestion.GetForward(codigoPortafolio, fechaOperacion, consecutivo, DatosRequest)
            If (dt Is Nothing) Then
                Return
            End If

            If (dt.Rows.Count = 0) Then
                Return
            End If

            txtCodigoSBS.Text = dt.Rows(0)("CodigoSBS")
            ddlMoneda.SelectedValue = dt.Rows(0)("CodigoMonedaVenta")
            ddlIndicador.SelectedValue = dt.Rows(0)("Movimiento")
            ddlTipo.SelectedValue = dt.Rows(0)("IndicadorForward")
            txtMonto.Text = dt.Rows(0)("MontoForward")
            txtPrecio.Text = dt.Rows(0)("PrecioTransaccion")
            txtFechaVencimiento.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dt.Rows(0)("FechaVencimiento")))
            txtPlazoVencimiento.Text = dt.Rows(0)("PlazoVencimiento")
            ddlModalidad.SelectedValue = dt.Rows(0)("Modalidad")
            txtTipoCambio.Text = dt.Rows(0)("TipoCambio")
            ddlIndicadorCaja.SelectedValue = dt.Rows(0)("IndicadorCaja")
            txtPlaza.Text = dt.Rows(0)("Plaza")

        Catch ex As Exception

        End Try
    End Sub

End Class

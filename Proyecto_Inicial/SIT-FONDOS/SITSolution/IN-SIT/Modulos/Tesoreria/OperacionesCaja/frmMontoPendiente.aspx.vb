Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Partial Class Modulos_Tesoreria_OperacionesCaja_frmMontoPendiente
    Inherits BasePage
    Dim oPortafolio As New PortafolioBM
    Dim oOperacionCaja As New OperacionesCajaBM
    Dim oCuentaEconomica As New CuentaEconomicaBM
    Sub CargaMonedaBanco()
        If Not ddlTercero.SelectedValue = "" Then
            HelpCombo.LlenarComboBox(ddlMoneda, oOperacionCaja.Listar_MonedaBanco_Clase(ddlPortafolio.SelectedValue, ddlTercero.SelectedValue, "10"), "CodigoMoneda", "Descripcion", True)
        End If
    End Sub
    Sub CargaBanco()
        HelpCombo.LlenarComboBox(ddlTercero, oOperacionCaja.SeleccionBancos("10", ddlPortafolio.SelectedValue), "CodigoEntidad", "Tercero", True)
    End Sub
    Private Sub CargaNumeroCuenta(ByVal CodigoEntidad As String, ByVal codMercado As String, ByVal codClaseCuenta As String, ByVal codMoneda As String, ByVal codPortafolio As String)
        Dim oCuentaEconomica As New CuentaEconomicaBM
        Dim dsCuentaEconomica As DataTable = oOperacionCaja.CuentaEconomica_SeleccionarPorFiltro(codPortafolio, codClaseCuenta, CodigoEntidad, codMoneda)
        HelpCombo.LlenarComboBox(ddlNumeroCuenta, dsCuentaEconomica, "NumeroCuenta", "NumeroCuenta", True)
    End Sub
    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        Try
            If txtFechaOperacion.Text = "" Then
                AlertaJS("Debe ingresar la fecha de Inicio.", "$('#txtFechaOperacion').focus();")
                Exit Sub
            ElseIf ddlNumeroCuenta.SelectedValue = "" Then
                AlertaJS("Debe seleccionar un numero de cuenta.", "$('#ddlNumeroCuenta').focus();")
                Exit Sub
            End If
            If ViewState("Estado") = "Ingresar" Then
                oOperacionCaja.InsertarCajaRecaudoPendiente(ddlNumeroCuenta.SelectedValue, CDec(TXTImporte.Text), _
                UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text), Nothing, DatosRequest)
                Response.Redirect("frmMontoPendienteListar.aspx")
            Else
                oOperacionCaja.ActualizarCajaRecaudoPendiente(CInt(Request.QueryString("cod")), ddlNumeroCuenta.SelectedValue, CDec(TXTImporte.Text), _
                UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text), Nothing, DatosRequest)
                AlertaJS("Registro modificado correctamente.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Sub CargarPendiente(Correlativo As Integer)
        Dim dt As DataTable = oOperacionCaja.CajaRecaudoPendiente(Correlativo)
        If dt.Rows.Count > 0 Then
            ddlPortafolio.SelectedValue = dt.Rows(0)("CodigoPortafolioSBS")
            CargaBanco()
            ddlTercero.SelectedValue = dt.Rows(0)("CodigoEntidad")
            CargaMonedaBanco()
            ddlMoneda.SelectedValue = dt.Rows(0)("CodigoMoneda")
            CargaNumeroCuenta(ddlTercero.SelectedValue, "", "10", ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
            ddlNumeroCuenta.SelectedValue = dt.Rows(0)("NumeroCuenta")
            TXTImporte.Text = dt.Rows(0)("MontoPendiente")
            txtFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        End If
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("cod") Is Nothing Then
            ViewState("Estado") = "Modificar"
            ddlPortafolio.Enabled = False
            ddlTercero.Enabled = False
            ddlMoneda.Enabled = False
            ddlNumeroCuenta.Enabled = False
        Else
            ViewState("Estado") = "Ingresar"
        End If
        If Not IsPostBack Then
            HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolio.PortafolioCodigoListar(""), "CodigoPortafolio", "Descripcion", False)
            txtFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
            CargaBanco()
            If ViewState("Estado") = "Modificar" Then
                CargarPendiente(CInt(Request.QueryString("cod")))
            End If
        End If
    End Sub
    Protected Sub ddlTercero_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTercero.SelectedIndexChanged
        CargaMonedaBanco()
        CargaNumeroCuenta(ddlTercero.SelectedValue, "", "10", ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
    End Sub
    Protected Sub ddlMoneda_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        CargaNumeroCuenta(ddlTercero.SelectedValue, "", "10", ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
    End Sub
    Protected Sub ddlPortafolio_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        txtFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        CargaBanco()
        CargaMonedaBanco()
        CargaNumeroCuenta(ddlTercero.SelectedValue, "", "10", ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
    End Sub
    Protected Sub btnSalir_Click(sender As Object, e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("frmMontoPendienteListar.aspx")
    End Sub
    Protected Sub txtFechaOperacion_TextChanged(sender As Object, e As System.EventArgs) Handles txtFechaOperacion.TextChanged
        Try
            If txtFechaOperacion.Text = "" Then
                Exit Sub
            End If
            Dim FechaPortafolio As Date = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
            Dim FechaPago As Date = Date.Parse(txtFechaOperacion.Text)
            Dim fechaHabilSiguiente As Date = UIUtility.fnFechaNueva(FechaPortafolio.ToShortDateString)
            If FechaPago > FechaPortafolio Then
                If FechaPago <> FechaPortafolio And FechaPago <> fechaHabilSiguiente Then
                    txtFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
                    AlertaJS("La fecha ingresada no puede ser mayor a la fecha de apertura del portafolio o feriados.")
                End If
            Else
                Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text))
                If cantidadreg > 0 Then
                    AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
                    txtFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
End Class
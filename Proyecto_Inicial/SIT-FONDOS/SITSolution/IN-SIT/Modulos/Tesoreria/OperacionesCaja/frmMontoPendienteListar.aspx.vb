Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Partial Class Modulos_Tesoreria_OperacionesCaja_frmMontoPendienteListar
    Inherits BasePage
    Dim oPortafolio As New PortafolioBM
    Dim oOperacionCaja As New OperacionesCajaBM
    Dim oCuentaEconomica As New CuentaEconomicaBM
    Sub CargaMonedaBanco()
        HelpCombo.LlenarComboBox(ddlMoneda, oOperacionCaja.Listar_MonedaBanco_Clase(ddlPortafolio.SelectedValue, ddlTercero.SelectedValue, "10"), "CodigoMoneda", "Descripcion", True)
    End Sub
    Sub CargaBanco()
        HelpCombo.LlenarComboBox(ddlTercero, oOperacionCaja.SeleccionBancos("10", ddlPortafolio.SelectedValue), "CodigoEntidad", "Tercero", True)
    End Sub
    Private Sub CargaNumeroCuenta(ByVal CodigoEntidad As String, ByVal codMercado As String, ByVal codClaseCuenta As String, ByVal codMoneda As String, ByVal codPortafolio As String)
        Dim oCuentaEconomica As New CuentaEconomicaBM
        Dim dsCuentaEconomica As DataTable = oOperacionCaja.CuentaEconomica_SeleccionarPorFiltro(codPortafolio, codClaseCuenta, CodigoEntidad, codMoneda)
        HelpCombo.LlenarComboBox(ddlNumeroCuenta, dsCuentaEconomica, "NumeroCuenta", "NumeroCuenta", True)
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolio.PortafolioCodigoListar(""), "CodigoPortafolio", "Descripcion", True)
            txtFechaOperacion.Text = UIUtility.ConvertirDecimalAStringFormatoFecha(UIUtility.ObtenerFechaMaximaNegocio())
            CargaBanco()
            CargaMonedaBanco()
            CargaNumeroCuenta(ddlTercero.SelectedValue, "", "10", ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
            CargarGrilla()
        End If
    End Sub
    Protected Sub ddlPortafolio_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        If ddlPortafolio.SelectedValue = "" Then
            txtFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
        Else
            txtFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        End If
        CargaBanco()
        CargaMonedaBanco()
        CargaNumeroCuenta(ddlTercero.SelectedValue, "", "10", ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
    End Sub
    Protected Sub ddlTercero_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTercero.SelectedIndexChanged
        CargaMonedaBanco()
        CargaNumeroCuenta(ddlTercero.SelectedValue, "", "10", ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
    End Sub
    Protected Sub ddlMoneda_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        CargaNumeroCuenta(ddlTercero.SelectedValue, "", "10", ddlMoneda.SelectedValue, ddlPortafolio.SelectedValue)
    End Sub
    Sub CargarGrilla()
        dgLista.DataSource = oOperacionCaja.SeleccionarCajaRecaudoPendiente(ddlPortafolio.SelectedValue, ddlTercero.SelectedValue, ddlMoneda.SelectedValue, ddlNumeroCuenta.SelectedValue)
        dgLista.DataBind()
    End Sub
    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        If e.CommandName = "Modificar" Then
            Response.Redirect("frmMontoPendiente.aspx?cod=" & e.CommandArgument.ToString())
        ElseIf e.CommandName = "Eliminar" Then
            oOperacionCaja.EliminarPendiente(e.CommandArgument, UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text), DatosRequest)
            CargarGrilla()
        End If
    End Sub
    Protected Sub btnIngresar_Click(sender As Object, e As System.EventArgs) Handles btnIngresar.Click
        Response.Redirect("frmMontoPendiente.aspx")
    End Sub
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
    Protected Sub txtFechaOperacion_TextChanged(sender As Object, e As System.EventArgs) Handles txtFechaOperacion.TextChanged
        Try
            If txtFechaOperacion.Text = "" Then
                Exit Sub
            End If
            Dim FechaPortafolio As Date
            If ddlPortafolio.SelectedValue = "" Then
                FechaPortafolio = ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            Else
                FechaPortafolio = ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
            End If
            Dim FechaPago As Date = Date.Parse(txtFechaOperacion.Text)
            Dim fechaHabilSiguiente As Date = fnFechaNueva(FechaPortafolio.ToShortDateString)
            If FechaPago > FechaPortafolio Then
                If FechaPago <> FechaPortafolio And FechaPago <> fechaHabilSiguiente Then
                    If ddlPortafolio.SelectedValue = "" Then
                        txtFechaOperacion.Text = ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
                    Else
                        txtFechaOperacion.Text = ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
                    End If
                    AlertaJS("La fecha ingresada no puede ser mayor a la fecha de apertura del portafolio o feriados.")
                End If
            Else
                Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text))
                If cantidadreg > 0 Then
                    AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
                    txtFechaOperacion.Text = ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
End Class
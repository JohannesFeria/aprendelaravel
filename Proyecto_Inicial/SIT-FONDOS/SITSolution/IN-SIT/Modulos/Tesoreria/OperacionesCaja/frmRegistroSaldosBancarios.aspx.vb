Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility
Partial Class Modulos_Tesoreria_OperacionesCaja_frmRegistroSaldosBancarios
    Inherits BasePage
    Private FechaOperacion As Date
#Region "CargarDatos"
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            ddlPortafolio.DataSource = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolio.DataValueField = "CodigoPortafolio"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub CargarBanco(ByVal CodigoMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dt As DataTable = oBanco.SeleccionarBancoPorCodigoMercadoYPortafolio("", Me.ddlPortafolio.SelectedValue).Tables(0)
            HelpCombo.LlenarComboBox(ddlBanco, dt, "CodigoTercero", "Descripcion", True, "SELECCIONE")
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
    End Sub
    Private Sub CargarClaseCuenta(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oClaseCuenta As New ClaseCuentaBM
            Dim dsClaseCuenta As ClaseCuentaBE = oClaseCuenta.Listar()
            ddlClaseCuenta.Items.Clear()
            ddlClaseCuenta.DataSource = dsClaseCuenta
            ddlClaseCuenta.DataValueField = "CodigoClaseCuenta"
            ddlClaseCuenta.DataTextField = "Descripcion"
            ddlClaseCuenta.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
            ddlClaseCuenta.SelectedValue = "20"
        Else
            ddlClaseCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
        End If
        ddlClaseCuenta.Enabled = enabled
    End Sub
    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dt As DataTable = oMoneda.Listar(ParametrosSIT.ESTADO_ACTIVO).Tables(0)
            HelpCombo.LlenarComboBox(ddlMoneda, dt, "CodigoMoneda", "Descripcion", True, "SELECCIONE")
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        End If
        ddlMoneda.Enabled = enabled
    End Sub
    Private Sub CargarDetPortafolio(ByVal codTercero As String, ByVal codClaseCuenta As String, ByVal codPortafolio As String, ByVal codMoneda As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oCuentaEconomica As New CuentaEconomicaBM
            Dim dt As DataTable = oCuentaEconomica.SeleccionarPorFiltro(codPortafolio, codClaseCuenta, codTercero, codMoneda, "", DatosRequest).Tables(1)
            HelpCombo.LlenarComboBox(ddlNroCuenta, dt, "NumeroCuenta", "NumeroCuenta", True, "SELECCIONE")
        Else
            ddlNroCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
        End If
        ddlNroCuenta.Enabled = enabled
    End Sub
#End Region
#Region "Eventos de la Página"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarPortafolio(True)
            CargarBanco("")
            CargarClaseCuenta()
            CargarMoneda()
            CargarDetPortafolio(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
            EstablecerFecha()
            BuscarValor(Request.QueryString("codigoPortafolio"), Request.QueryString("numeroCuenta"))
            ViewState("Modo") = "Consulta"
        End If
    End Sub
    Private Sub BuscarValor(ByVal codigoPortafolio As String, ByVal numeroCuenta As String)
        If Not codigoPortafolio Is Nothing And Not numeroCuenta Is Nothing Then
            If Not ddlPortafolio.Items.FindByValue(codigoPortafolio) Is Nothing Then
                ddlPortafolio.SelectedValue = codigoPortafolio
            Else
                Exit Sub
            End If
            If Not ddlNroCuenta.Items.FindByValue(numeroCuenta) Is Nothing Then
                ddlNroCuenta.SelectedValue = numeroCuenta
            Else
                Exit Sub
            End If
            CargarGrilla()
        End If
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Private Function ObtenerFechaApertura(ByVal codigoPortafolio As String) As Decimal
        Return UIUtility.ConvertirFechaaDecimal(Now.ToString("dd/MM/yyyy"))
    End Function
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        CargarBanco("")
        CargarDetPortafolio(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
        EstablecerFecha()
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        GrabarSaldos()
    End Sub
    Private Function ValidarSaldos() As Boolean
        For Each row As GridViewRow In dgLista.Rows
            Dim chk As CheckBox = row.Cells(1).Controls(1)
            If chk.Checked Then
                Dim txtSaldoDisponible As TextBox = row.Cells(7).Controls(3)
                Dim dsSaldoCuenta As DataSet = New SaldosBancariosBM().Seleccionar(row.Cells(5).Text, row.Cells(6).Text, UIUtility.ConvertirFechaaDecimal(lblFechaOper.Text))
                If dsSaldoCuenta.Tables(0).Rows.Count > 0 Then
                    AlertaJS(ObtenerMensaje("ALERT10", " " & row.Cells(6).Text))
                    AlertaJS(New UtilDM().RetornarMensajeConfirmacion("ALERT10"))
                    Return False
                End If
            End If
        Next
        Return True
    End Function
    Protected Sub ddlBanco_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlBanco.SelectedIndexChanged
        CargarDetPortafolio(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
    End Sub
    Private Sub ddlMoneda_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        CargarDetPortafolio(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
    End Sub
    Private Sub ddlClaseCuenta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlClaseCuenta.SelectedIndexChanged
        CargarDetPortafolio(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
    End Sub
#End Region
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub
    Private Sub GrabarSaldos()
        Try
            Dim txtSaldoDisponible As New TextBox
            Dim dsSaldos As New SaldosBancariosBE
            Dim oSaldos As New SaldosBancariosBM
            For Each row As GridViewRow In dgLista.Rows
                If Not (row.FindControl("lblSaldoDisponible") Is Nothing) Then
                    txtSaldoDisponible = CType(row.FindControl("txtSaldoDisponible"), TextBox)
                End If
                Dim saldo As SaldosBancariosBE.SaldosBancariosRow = dsSaldos.SaldosBancarios.NewSaldosBancariosRow
                saldo.CodigoPortafolioSBS = row.Cells(5).Text
                saldo.NumeroCuenta = row.Cells(6).Text
                saldo.SaldoDisponibleInicial = Convert.ToDecimal(IIf(txtSaldoDisponible.Text = "", "0.00", txtSaldoDisponible.Text).Replace(".", UIUtility.DecimalSeparator()))
                saldo.SaldoContableInicial = Convert.ToDecimal(IIf(txtSaldoDisponible.Text = "", "0.00", txtSaldoDisponible.Text).Replace(".", UIUtility.DecimalSeparator()))
                saldo.TipoIngreso = "M"
                saldo.FechaOperacion = UIUtility.ConvertirFechaaDecimal(lblFechaOper.Text)
                dsSaldos.SaldosBancarios.AddSaldosBancariosRow(saldo)
                oSaldos.Insertar(dsSaldos, DatosRequest)
                dsSaldos.SaldosBancarios.Rows.Clear()
            Next
            CargarGrilla()
            AlertaJS("Se ha registrado correctamente")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub CargarGrilla()
        Dim dsCuentaEconomica As New CuentaEconomicaBE
        Dim roCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.NewCuentaEconomicaRow
        Dim dt As New DataTable
        roCuentaEconomica.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        roCuentaEconomica.NumeroCuenta = ddlNroCuenta.SelectedValue
        roCuentaEconomica.CodigoMoneda = ddlMoneda.SelectedValue
        roCuentaEconomica.CodigoClaseCuenta = ddlClaseCuenta.SelectedValue
        roCuentaEconomica.CodigoTercero = ddlBanco.SelectedValue
        roCuentaEconomica.FechaCreacion = UIUtility.ConvertirFechaaDecimal(lblFechaOper.Text)
        roCuentaEconomica.CodigoMercado = ""
        dsCuentaEconomica.CuentaEconomica.AddCuentaEconomicaRow(roCuentaEconomica)
        dt = SeleccionarSaldosBancarios(dsCuentaEconomica).Tables(0)
        dgLista.SelectedIndex = -1
        dgLista.DataSource = dt
        dgLista.DataBind()
        lbContador.Text = MostrarResultadoBusqueda(dt.Rows.Count)
    End Sub
    Private Function SeleccionarSaldosBancarios(ByVal dsCuentaEconomica As CuentaEconomicaBE) As DataSet
        Dim oSaldoBancario As New SaldosBancariosBM
        Return oSaldoBancario.SeleccionarPorFiltro(dsCuentaEconomica, DatosRequest)
    End Function
    Private Sub EstablecerFecha()
        If ddlPortafolio.SelectedValue = "" Then
            lblFechaOper.Text = UIUtility.ConvertirFechaaString(ObtenerFechaMaximaNegocio())
        Else
            lblFechaOper.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        End If
    End Sub
    Public Sub GrabarSaldo(ByVal codigoPortalio As String, ByVal numeroCuenta As String, ByVal saldoDisponible As String, ByVal saldoContable As String)
        Dim oSaldos As New SaldosBancariosBM
        Dim dsSaldos As New SaldosBancariosBE
        Dim saldo As SaldosBancariosBE.SaldosBancariosRow = dsSaldos.SaldosBancarios.NewSaldosBancariosRow
        saldo.CodigoPortafolioSBS = codigoPortalio
        saldo.NumeroCuenta = numeroCuenta
        saldo.SaldoDisponibleInicial = saldoDisponible
        saldo.SaldoContableInicial = saldoContable
        saldo.TipoIngreso = "M"
        saldo.FechaOperacion = UIUtility.ConvertirFechaaDecimal(lblFechaOper.Text)
        dsSaldos.SaldosBancarios.AddSaldosBancariosRow(saldo)
        oSaldos.Insertar(dsSaldos, DatosRequest)
        dsSaldos.SaldosBancarios.Rows.Clear()
    End Sub
    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub
    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Dim txtSaldoDisponible As TextBox
        Dim importe As Decimal
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = CType(e.Row.DataItem, DataRowView)
            If Not (e.Row.FindControl("txtSaldoDisponible") Is Nothing) Then
                txtSaldoDisponible = CType(e.Row.FindControl("txtSaldoDisponible"), TextBox)
                importe = Convert.ToDecimal(dr("SaldoDisponibleInicial"))
                txtSaldoDisponible.Text = importe.ToString("N2")
            End If
        End If
    End Sub
End Class
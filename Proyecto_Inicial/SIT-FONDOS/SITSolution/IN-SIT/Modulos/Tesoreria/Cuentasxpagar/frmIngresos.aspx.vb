Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Partial Class Modulos_Tesoreria_Cuentasxpagar_frmIngresos
    Inherits BasePage
#Region "CargarDatos"
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            'Dim dsMercado As Data.DataSet = oMercado.Listar(DatosRequest)
            Dim dsMercado As Data.DataSet = oMercado.ListarActivos(DatosRequest, ParametrosSIT.ESTADO_ACTIVO)
            ddlMercado.Items.Clear()
            ddlMercado.DataSource = dsMercado
            ddlMercado.DataValueField = "CodigoMercado"
            ddlMercado.DataTextField = "Descripcion"
            ddlMercado.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
        If (ddlMercado.Items.Count > 1) Then
            ddlMercado.SelectedIndex = 2
        End If
    End Sub
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            HelpCombo.PortafolioCodigoListar(ddlPortafolio, PORTAFOLIO_MULTIFONDOS)
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub CargarOperacion(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oOperacion As New OperacionBM
            Dim dsOperacion As Data.DataSet = oOperacion.SeleccionarPorCodigoTipoOperacion("", "S", "20")
            ddlOperacion.Items.Clear()
            ddlOperacion.DataSource = dsOperacion
            ddlOperacion.DataValueField = "CodigoOperacion"
            ddlOperacion.DataTextField = "Descripcion"
            ddlOperacion.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlOperacion)
        Else
            ddlOperacion.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlOperacion)
        End If
        ddlOperacion.Enabled = enabled
    End Sub
    Private Sub CargarIntermediario(Optional ByVal sMercado As String = "", Optional ByVal enabled As Boolean = True)
        Dim sCodigoPais As String
        If sMercado.Trim = "" Then
            sCodigoPais = ""
        Else
            sCodigoPais = IIf(sMercado = "1", "604", "XXX")
        End If

        If enabled Then
            Dim oIntermediario As New TercerosBM
            Dim dsIntermediario As TercerosBE = oIntermediario.SeleccionarPorFiltroMercado(ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO, "", sCodigoPais)
            ddlIntermediario.Items.Clear()
            ddlIntermediario.DataSource = dsIntermediario
            ddlIntermediario.DataValueField = "CodigoTercero"
            ddlIntermediario.DataTextField = "Descripcion"
            ddlIntermediario.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlIntermediario)
        Else
            ddlIntermediario.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlIntermediario)
        End If
        ddlIntermediario.Enabled = enabled
    End Sub
    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As Data.DataSet = oMoneda.Listar(ParametrosSIT.ESTADO_ACTIVO)
            ddlMoneda.Items.Clear()
            ddlMoneda.DataSource = dsMoneda
            ddlMoneda.DataValueField = "CodigoMoneda"
            ddlMoneda.DataTextField = "Descripcion"
            ddlMoneda.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        End If
        ddlMoneda.Enabled = enabled
    End Sub
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarPortafolio()
            CargarMercado()
            CargarOperacion()
            CargarIntermediario()
            CargarMoneda()
            EstablecerFecha()
            txtFechaCobro.Text = Now.ToString("dd/MM/yyyy")
        End If
        btnAceptar.Attributes.Add("onclick", "return ValidarDatos();")
    End Sub
    Private Sub ddlMercado_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        Dim sMercado As String = ddlMercado.SelectedValue.Trim
        Call CargarIntermediario(sMercado, True)
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        EstablecerFecha()
    End Sub
    Private Sub LimpiarFormulario()
        ddlPortafolio.SelectedIndex = 0
        ddlMercado.SelectedIndex = 0
        ddlOperacion.SelectedIndex = 0
        ddlIntermediario.SelectedIndex = 0
        ddlMoneda.SelectedIndex = 0
        txtImporte.Text = ""
        txtReferencia.Text = ""
        txtFechaCobro.Text = Now.ToString("dd/MM/yyyy")
        EstablecerFecha()
    End Sub
    Private Sub EstablecerFecha()
        lblFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim dsCuentasPorCobrar As New CuentasPorCobrarPagarBE
            Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.NewCuentasPorCobrarPagarRow
            cuentaPorCobrar.CodigoMercado = ddlMercado.SelectedValue
            cuentaPorCobrar.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
            cuentaPorCobrar.CodigoMoneda = ddlMoneda.SelectedValue
            cuentaPorCobrar.CodigoTercero = ddlIntermediario.SelectedValue
            cuentaPorCobrar.CodigoOperacion = ddlOperacion.SelectedValue
            cuentaPorCobrar.Referencia = txtReferencia.Text
            cuentaPorCobrar.Importe = txtImporte.Text.Replace(".", UIUtility.DecimalSeparator())
            cuentaPorCobrar.FechaOperacion = UIUtility.ConvertirFechaaDecimal(lblFechaOperacion.Text)
            cuentaPorCobrar.FechaIngreso = UIUtility.ConvertirFechaaDecimal(txtFechaCobro.Text)
            dsCuentasPorCobrar.CuentasPorCobrarPagar.AddCuentasPorCobrarPagarRow(cuentaPorCobrar)
            Dim oCuentasPorCobrar As New CuentasPorCobrarBM
            oCuentasPorCobrar.Insertar(dsCuentasPorCobrar, "S", DatosRequest)
            AlertaJS(ObtenerMensaje("ALERT12"))
            LimpiarFormulario()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
End Class
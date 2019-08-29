Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Tesoreria_OperacionesCaja_frmConsultaVencimientos
    Inherits BasePage
#Region "Eventos de la Pagina"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarMercado(True)
            CargarPortafolio(True)
            CargarMoneda(True)
            CargarIntermediario(ddlMercado.SelectedValue, True)
            CargarOperacion(True)
            CargarClaseInstrumento()
            Dim i As Integer = ddlPortafolio.SelectedIndex
            If i <> 0 Then
                tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
            End If
            tbFechaFin.Text = Now.ToString("dd/MM/yyyy")
        End If
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Private Sub CargarGrilla()
        Dim dsCuentasPorCobrar As New CuentasPorCobrarPagarBE
        Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.NewCuentasPorCobrarPagarRow
        cuentaPorCobrar.CodigoMercado = ddlMercado.SelectedValue
        cuentaPorCobrar.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        cuentaPorCobrar.CodigoMoneda = ddlMoneda.SelectedValue
        cuentaPorCobrar.CodigoTercero = ddlIntermediario.SelectedValue
        cuentaPorCobrar.CodigoOperacion = ddlOperacion.SelectedValue
        Dim fechaIni As Decimal = IIf(tbFechaInicio.Text = "", 0, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
        Dim fechaFin As Decimal = IIf(tbFechaFin.Text = "", 0, UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text))
        If fechaIni = 0 Or fechaFin = 0 Then
            AlertaJS("Debe seleccionar una fecha de inicio y de fin")
            Exit Sub
        End If
        cuentaPorCobrar.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
        cuentaPorCobrar.FechaIngreso = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
        dsCuentasPorCobrar.CuentasPorCobrarPagar.AddCuentasPorCobrarPagarRow(cuentaPorCobrar)
        Dim dsCuentas As DataSet = New CuentasPorCobrarBM().SeleccionarVencimientos(dsCuentasPorCobrar, ddlClaseInstrumento.SelectedValue, fechaIni, fechaFin, DatosRequest)
        Dim dv As DataView = dsCuentas.Tables(0).DefaultView
        dgLista.DataSource = dv
        dgLista.DataBind()
        EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dsCuentas.Tables(0).Rows.Count) + "')")
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            If UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text) Then
                AlertaJS(ObtenerMensaje("ALERT48"))
                Exit Sub
            End If
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim tablaParametros As New Hashtable

        tablaParametros("codPortafolio") = ddlPortafolio.SelectedValue
        tablaParametros("codMercado") = ddlMercado.SelectedValue
        tablaParametros("codMoneda") = ddlMoneda.SelectedValue
        tablaParametros("codOperacion") = ddlOperacion.SelectedValue
        tablaParametros("codIntermediario") = ddlIntermediario.SelectedValue
        tablaParametros("codClaseInstrumento") = ddlClaseInstrumento.SelectedValue
        tablaParametros("Portafolio") = IIf(ddlPortafolio.SelectedIndex = 0, "", ddlPortafolio.SelectedItem.Text)
        tablaParametros("Mercado") = IIf(ddlMercado.SelectedIndex = 0, "", ddlMercado.SelectedItem.Text)
        tablaParametros("Moneda") = IIf(ddlMoneda.SelectedIndex = 0, "", ddlMoneda.SelectedItem.Text)
        tablaParametros("Operacion") = IIf(ddlOperacion.SelectedIndex = 0, "", ddlOperacion.SelectedItem.Text)
        tablaParametros("Intermediario") = IIf(ddlIntermediario.SelectedIndex = 0, "", ddlIntermediario.SelectedItem.Text)
        tablaParametros("ClaseInstrumento") = IIf(ddlClaseInstrumento.SelectedIndex = 0, "", ddlClaseInstrumento.SelectedItem.Text)
        Session("ParametrosReporteVencimientos") = tablaParametros
        EjecutarJS(UIUtility.MostrarPopUp("../Reportes/frmReporte.aspx?ClaseReporte=ReporteVencimientos&FechaInicio=" & UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text) & "&FechaFin=" & UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text), "", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Dim i As Integer = ddlPortafolio.SelectedIndex
        If i <> 0 Then
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        End If
        If Not (tbFechaInicio.Text = "") Then
            tbFechaFin.Text = DateTime.Parse(tbFechaInicio.Text).AddDays(3).ToShortDateString()
        End If
    End Sub

    Private Sub ddlMercado_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        CargarIntermediario(ddlMercado.SelectedValue, True)
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub

#End Region
#Region "Cargar Datos"
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dt As DataTable = oMercado.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO).Tables(0)
            HelpCombo.LlenarComboBox(ddlMercado, dt, "CodigoMercado", "Descripcion", True)
            ddlMercado.SelectedIndex = 0
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
    End Sub

    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            ddlPortafolio.DataSource = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolio.DataValueField = "CodigoPortafolio"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
            ddlPortafolio.Items.Insert(0, "--SELECCIONE--")
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub

    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataSet = oMoneda.Listar(ParametrosSIT.ESTADO_ACTIVO)
            HelpCombo.LlenarComboBox(ddlMoneda, dsMoneda.Tables(0), "CodigoMoneda", "Descripcion", True, "SELECCIONE")
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        End If
        ddlMoneda.Enabled = enabled
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
    Private Sub CargarOperacion(ByVal enabled As Boolean)
        If enabled Then
            Dim oOperacion As New OperacionBM
            Dim dsOperacion As DataSet = oOperacion.SeleccionarPorCodigoTipoOperacion("", "", "20")
            HelpCombo.LlenarComboBox(ddlOperacion, dsOperacion.Tables(0), "CodigoOperacion", "Descripcion", True, "SELECCIONE")
        Else
            ddlOperacion.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlOperacion)
        End If
        ddlOperacion.Enabled = enabled
    End Sub
    Private Sub CargarClaseInstrumento()
        Dim oClaseInstrumento As New ClaseInstrumentoBM
        Dim dsClaseInstrumento As DataSet = oClaseInstrumento.Seleccionar(DatosRequest)
        HelpCombo.LlenarComboBox(ddlClaseInstrumento, dsClaseInstrumento.Tables(0), "Categoria", "Descripcion", True, "SELECCIONE")
    End Sub
#End Region
#Region "Metodos de Control"

    Private Sub ValidarLista(ByVal ddl As DropDownList, ByVal lista As String)
        Dim i As Integer = ddl.Items.Count - 1
        While (i >= 0)
            If lista.IndexOf(ddl.Items(i).Value) = -1 Then
                ddl.Items.RemoveAt(i)
            End If
            i = i - 1
        End While
    End Sub

#End Region
End Class
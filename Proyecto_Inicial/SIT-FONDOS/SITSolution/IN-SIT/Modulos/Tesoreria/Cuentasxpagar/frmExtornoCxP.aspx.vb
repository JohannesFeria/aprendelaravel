Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Tesoreria_Cuentasxpagar_frmExtornoCxP
    Inherits BasePage
#Region "CargarDatos"
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dsMercado As DataSet = oMercado.ListarActivos(DatosRequest, ParametrosSIT.ESTADO_ACTIVO)
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
            Dim dsOperacion As DataSet = oOperacion.SeleccionarPorCodigoTipoOperacion("", "S", "20")
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
            If sMercado.Length > 3 Then
                sCodigoPais = ""
            Else
                sCodigoPais = IIf(sMercado = "1", "604", "XXX")
            End If
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
            Dim dsMoneda As DataSet = oMoneda.Listar(ParametrosSIT.ESTADO_ACTIVO)
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
#Region "Eventos de la Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarMercado()
            CargarPortafolio()
            CargarOperacion()
            CargarIntermediario()
            CargarMoneda()
            EstablecerFecha()
        End If
        UIUtility.CrearConfirmDialogBox(btnExtornar, "CONF19")
    End Sub
    Private Sub CargarGrilla()
        Dim dsCuentasPorCobrar As New CuentasPorCobrarPagarBE
        Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.NewCuentasPorCobrarPagarRow
        cuentaPorCobrar.CodigoMercado = ddlMercado.SelectedValue
        cuentaPorCobrar.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        cuentaPorCobrar.CodigoMoneda = ddlMoneda.SelectedValue
        cuentaPorCobrar.CodigoTercero = ddlIntermediario.SelectedValue
        cuentaPorCobrar.CodigoOperacion = ddlOperacion.SelectedValue
        cuentaPorCobrar.FechaOperacion = UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text)
        'OT 10238 - 07/04/2017 - Carlos Espejo
        'Descripcion: Solo se presenta la fecha de liquidacion.
        cuentaPorCobrar.FechaIngreso = UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text)
        'OT 10238 Fin
        cuentaPorCobrar.Egreso = "S"
        Dim fechaIni As Decimal = IIf(txtFechaInicio.Text = "", 0, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text))
        Dim fechaFin As Decimal = IIf(txtFechaInicio.Text = "", 0, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text))
        dsCuentasPorCobrar.CuentasPorCobrarPagar.AddCuentasPorCobrarPagarRow(cuentaPorCobrar)
        Dim dsCuentas As DataSet = New CuentasPorCobrarBM().SeleccionarAnularPorFiltro(dsCuentasPorCobrar, fechaIni, fechaFin, DatosRequest)
        Dim view As DataView = dsCuentas.Tables(0).DefaultView
        view.RowFilter = "NroOperacion not like 'C%'"
        dgLista.DataSource = view
        dgLista.DataBind()
        Me.lbContador.Text = UIUtility.MostrarResultadoBusqueda(dsCuentas.Tables(0))
    End Sub
    Private Sub btnExtornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtornar.Click
        Dim CodigoCuenta As String
        Dim CodigoPortafolio As String
        Dim oCuentaPorCobrar As New CuentasPorCobrarBM
        Try
            For Each row As GridViewRow In dgLista.Rows
                If CType(row.Cells(0).Controls(1), CheckBox).Checked Then
                    CodigoCuenta = row.Cells(1).Text
                    CodigoPortafolio = row.Cells(13).Text
                    If UIUtility.ConvertirFechaaDecimal(row.Cells(3).Text) < UIUtility.ObtenerFechaNegocio(CodigoPortafolio) Then
                        AlertaJS("No se pueden anular Operaciones con fecha de negociación anteriores.")
                        Exit Sub
                    End If
                    oCuentaPorCobrar.Anular(CodigoCuenta, CodigoPortafolio, DatosRequest)
                    AlertaJS(ObtenerMensaje("ALERT63"))
                End If
            Next
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        'OT 10238 - 07/04/2017 - Carlos Espejo
        'Descripcion: Se agrega mensaje de seleccione portafolio.
        If ddlPortafolio.SelectedValue = "" Then
            AlertaJS("Seleccione un portafolio para continuar.", "$('#ddlPortafolio').focus()")
        Else
            lbContador.Text = ""
            If UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text) > UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text) Then
                AlertaJS(ObtenerMensaje("ALERT48"))
                Exit Sub
            End If
            CargarGrilla()
        End If
        'OT 10238 Fin
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Private Sub EstablecerFecha()
        If ddlPortafolio.SelectedIndex > 0 Then
            txtFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        Else
            txtFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
        End If
    End Sub
#End Region
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        EstablecerFecha()
    End Sub
    Private Sub ddlMercado_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        Dim sMercado As String = ddlMercado.SelectedValue.Trim
        Call CargarIntermediario(sMercado, True)
    End Sub
    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Dim chbConfirmar As CheckBox
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Not (e.Row.FindControl("chbConfirmar") Is Nothing) Then
                chbConfirmar = CType(e.Row.FindControl("chbConfirmar"), CheckBox)
                chbConfirmar.Attributes.Add("onclick", "__javascript:SelectedRow(" + chbConfirmar.ClientID + ")")
            End If
        End If
    End Sub
End Class
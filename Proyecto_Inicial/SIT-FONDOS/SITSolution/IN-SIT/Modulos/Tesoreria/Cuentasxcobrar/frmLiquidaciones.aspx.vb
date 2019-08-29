Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Partial Class Modulos_Tesoreria_Cuentasxcobrar_frmLiquidaciones
    Inherits BasePage
    Dim oRow As MonedaBE.MonedaRow
    Private operacionesSeleccionandas As New ArrayList
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim sCodigoMnemonio As String
    Dim sCategoria As String
    Private Sub CargarRenovacion()
        Dim dt As DataTable

        dt = oOrdenInversionBM.ConstitucionesdelDiaDPZ(ddlPortafolio.SelectedValue, hdCodigoTrecero.Value, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text))
          
        If dt.Rows.Count > 0 Then
            Dim dtListaSeleccionada As DataTable = dt.Select("CodigoMoneda = '" + hidCodigoMonedaOrigen.Value + "'").CopyToDataTable
            gvRenovacion.DataSource = dtListaSeleccionada
        Else
            gvRenovacion.DataSource = Nothing
        End If
        gvRenovacion.DataBind()
    End Sub
    Sub LimpiarRenovacion()
        gvRenovacion.DataSource = Nothing
        pnRenovacion.Visible = False
        ddlModeloCarta.SelectedIndex = 0
        txtObservacionCarta.Text = ""
    End Sub

    Private Sub LimpiarProcesos()
        pnRenovacion.Visible = False
        ddlModeloCarta.Items.Clear()
        lblMoneda.Text = ""
    End Sub
    Function ValidaTerceroOI() As Boolean
        Dim Indice As Integer
        For Each fila As GridViewRow In dgLista.Rows
            If fila.RowType = DataControlRowType.DataRow Then
                Dim chkSelect As System.Web.UI.WebControls.CheckBox = CType(fila.FindControl("chbConfirmar"), System.Web.UI.WebControls.CheckBox)
                If Not chkSelect Is Nothing Then
                    If chkSelect.Checked Then
                        Indice += 1
                        If Indice > 1 Then
                            Return False
                        End If
                    End If
                End If
            End If
        Next
        Return True
    End Function
    Function ValidaRenovacion() As Boolean
        Dim correlativo As Integer = 0
        For Each fila As GridViewRow In gvRenovacion.Rows
            If fila.RowType = DataControlRowType.DataRow Then
                Dim chkSelect As System.Web.UI.WebControls.CheckBox = CType(fila.FindControl("chkSelect"), System.Web.UI.WebControls.CheckBox)
                If Not chkSelect Is Nothing Then
                    correlativo += 1
                End If
            End If
        Next
        If correlativo = 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Sub InsertarRenovacion(CodigoOrden As String)
        Dim CodRelacion As Integer
        oOrdenInversionBM.Libera_Renovacion(CodigoOrden)
        CodRelacion = oOrdenInversionBM.Insertar_DPZRenovacionCabecera(CodigoOrden, "4", "", UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text))
        For Each fila As GridViewRow In gvRenovacion.Rows
            If fila.RowType = DataControlRowType.DataRow Then
                Dim chkSelect As System.Web.UI.WebControls.CheckBox = CType(fila.FindControl("chkSelect"), System.Web.UI.WebControls.CheckBox)
                Dim lbCodigo As System.Web.UI.WebControls.Label = CType(fila.FindControl("lbCodigo"), System.Web.UI.WebControls.Label)
                If Not chkSelect Is Nothing Then
                    If chkSelect.Checked Then
                        oOrdenInversionBM.Insertar_DPZRenovacionDetalle(CodRelacion, lbCodigo.Text, "3", UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text))
                    End If
                End If
            End If
        Next
    End Sub

#Region "CargarDatos"
    Sub CargaBancoDifRenovacion()
        UIUtility.CargarIntermediariosOISoloBancos(ddlBancoRenovacion)
        UIUtility.CargarIntermediariosOISoloBancos(ddlBancoGlosaEgreso)
    End Sub
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", False)
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub CargarOperacion(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oOperacion As New OperacionBM
            Dim dsOperacion As DataSet = oOperacion.SeleccionarPorCodigoTipoOperacion("", "", "20")
            HelpCombo.LlenarComboBox(ddlOperacion, dsOperacion.Tables(0), "CodigoOperacion", "Descripcion", False)
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
            ddlIntermediario.Items.Clear()
            Dim oIntermediario As New TercerosBM
            Dim dsIntermediario As TercerosBE = oIntermediario.SeleccionarPorFiltroMercado(ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO, "", sCodigoPais)
            HelpCombo.LlenarComboBox(ddlIntermediario, dsIntermediario.Tables(0), "CodigoTercero", "Descripcion", False)
            UIUtility.InsertarElementoSeleccion(ddlIntermediario)
        Else
            ddlIntermediario.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlIntermediario)
        End If
        ddlIntermediario.Enabled = enabled
    End Sub
    Private Function Distinct(ByVal dt As DataTable, ByVal columName As String) As DataTable
        Dim dr As DataRow
        Dim value As String
        Dim dtResult As DataTable
        dtResult = dt.Clone()
        If (dt.Rows.Count > 0) Then
            value = Convert.ToString(dt.Rows(0)(columName))
            dtResult.LoadDataRow(dt.Rows(0).ItemArray(), True)
            For Each dr In dt.Rows
                If String.Equals(value, Convert.ToString(dr(columName))) = False Then
                    value = Convert.ToString(dr(columName))
                    dtResult.LoadDataRow(dr.ItemArray(), True)
                End If
            Next
        End If
        Return dtResult
    End Function
    Private Sub CargarBanco(Optional ByVal sCodigoMercado As String = "", Optional ByVal sCodigoPortafolioSBS As String = "", Optional ByVal sCodigoMoneda As String = "", Optional ByVal enabled As Boolean = True)
        Dim dtBancos As DataTable
        If enabled Then
            Dim oBanco As New TercerosBM
            'Ahora filtra por codigo de mercado, portafolio, moneda
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone(sCodigoMercado, sCodigoPortafolioSBS, sCodigoMoneda)
            dtBancos = Distinct(dsBanco.Tables(0), "Descripcion")
            Me.Session().Add("Bancos", dtBancos)
            HelpCombo.LlenarComboBox(ddlBanco, dtBancos, "CodigoTercero", "Descripcion", False)
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
    End Sub
    Private Sub CargarBancoDestino(Optional ByVal sCodigoMercado As String = "", Optional ByVal sCodigoPortafolioSBS As String = "", Optional ByVal sCodigoMoneda As String = "", Optional ByVal enabled As Boolean = True)
        Dim dtBancos As DataTable
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone(String.Empty, sCodigoPortafolioSBS, sCodigoMoneda)
            dtBancos = Distinct(dsBanco.Tables(0), "Descripcion")
            Me.Session().Add("Bancos", dtBancos)
            HelpCombo.LlenarComboBox(ddlBancoDestino, dtBancos, "CodigoTercero", "Descripcion", False)
            UIUtility.InsertarElementoSeleccion(ddlBancoDestino)
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
    End Sub

    Private Sub CargarBancoMatrizDestino()

        UIUtility.CargarIntermediariosOISoloBancos(ddlBancoMatrizDestino)

    End Sub
    Private Sub CargarBancoMatrizOrigen()

        UIUtility.CargarIntermediariosOISoloBancos(ddlBancoMatrizOrigen)

    End Sub

    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            'OT 10238 - 07/04/2017 - Carlos Espejo
            'Descripcion: Se cargan las monedas sin el mercado
            Dim dsMoneda As DataSet = oMoneda.GetMonedaMercadoPortafolio("", ddlPortafolio.SelectedValue)
            'OT 10238 Fin
            HelpCombo.LlenarComboBox(ddlMoneda, dsMoneda.Tables(0), "CodigoMoneda", "Descripcion", False)
            UIUtility.InsertarElementoSeleccion(ddlMoneda, "", "Todos")
            ddlMoneda.SelectedIndex = 0
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda, "", "Todos")
        End If
        ddlMoneda.Enabled = enabled
    End Sub
    Private Sub CargarClaseCuenta(Optional ByVal enabled As Boolean = True, Optional ByVal sCodigoTercero As String = "")
        Dim dtBancos As DataTable
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim codigMoneda As String
            If pnlDestinoDivisas.Attributes("class") Is Nothing Then
                codigMoneda = ddlMoneda.SelectedValue
            Else
                codigMoneda = Me.hidCodigoMonedaOrigen.Value
            End If
            'OT 10238 - 07/04/2017 - Carlos Espejo
            'Descripcion: Se cargan los bancos sin el mercado
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone("", Me.ddlPortafolio.SelectedValue, codigMoneda)
            'OT 10238 Fin
            dtBancos = dsBanco.Tables(0)
            ddlClase.Items.Clear()
            dtBancos.DefaultView.RowFilter = "CodigoTercero='" + sCodigoTercero + "'"
            dtBancos.DefaultView.RowStateFilter = DataViewRowState.CurrentRows
            Dim r As DataRowView
            For Each r In dtBancos.DefaultView
                If ddlClase.Items.FindByValue(CType(r("CodigoClaseCuenta"), String)) Is Nothing Then
                    ddlClase.Items.Add(New ListItem(CType(r("NombreClaseCuenta"), String), CType(r("CodigoClaseCuenta"), String)))
                End If
            Next
            If ddlClase.Items.Count = 1 Then
                UIUtility.InsertarElementoSeleccion(ddlClase)
                ddlClase.SelectedIndex = 1
            Else
                UIUtility.InsertarElementoSeleccion(ddlClase)
                If ddlClase.Items.Count > 1 Then
                    ddlClase.SelectedValue = "20"
                End If
            End If
        Else
            ddlClase.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClase)
        End If
        ddlClase.Enabled = enabled
    End Sub
    Private Sub CargarDetallePortafolio(ByVal codTercero As String, ByVal CodigoClaseCuenta As String, ByVal CodigoPortafolio As String, Optional ByVal enabled As Boolean = True)
        Dim dtBancos As DataTable
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim codigMoneda As String
            If pnlDestinoDivisas.Attributes("class") Is Nothing Then
                codigMoneda = ddlMoneda.SelectedValue
            Else
                codigMoneda = Me.hidCodigoMonedaOrigen.Value
            End If
            'OT 10238 - 07/04/2017 - Carlos Espejo
            'Descripcion: Se cargan las bancos sin el mercado
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone("", CodigoPortafolio, codigMoneda)
            'OT 10238 Fin
            dtBancos = dsBanco.Tables(0)
            If ddlClase.SelectedValue = "" Then
                dtBancos.DefaultView.RowFilter = "CodigoTercero='" + ddlBanco.SelectedValue + "'"
            Else
                dtBancos.DefaultView.RowFilter = "CodigoClaseCuenta='" + ddlClase.SelectedValue + "' AND CodigoTercero='" + ddlBanco.SelectedValue + "'"
            End If
            dtBancos.DefaultView.RowStateFilter = DataViewRowState.CurrentRows
            ddlNroCuenta.Items.Clear()
            ddlNroCuenta.DataValueField = "CodigoCuenta"
            ddlNroCuenta.DataTextField = "NumeroCuenta"
            ddlNroCuenta.DataSource = dtBancos.DefaultView
            ddlNroCuenta.SelectedIndex = -1
            ddlNroCuenta.DataBind()
            If ddlNroCuenta.Items.Count = 1 Then
                UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
                ddlNroCuenta.SelectedIndex = 1
            Else
                UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
            End If
        Else
            ddlNroCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
        End If
        ddlNroCuenta.Enabled = enabled
    End Sub
    Private Sub CargarTipoPago(Optional ByVal enabled As Boolean = True)
        Dim oModalidadPago As New ModalidadPagoBM
        Dim dsModalidadPago As DataSet = oModalidadPago.Listar(DatosRequest, ParametrosSIT.ESTADO_ACTIVO)
        HelpCombo.LlenarComboBox(ddlTipoPago, dsModalidadPago.Tables(0), "codigoModalidadPago", "Descripcion", False)
        UIUtility.InsertarElementoSeleccion(ddlTipoPago)
        ddlTipoPago.SelectedValue = "CCOB"
        ddlTipoPago.Enabled = False
    End Sub
#End Region
#Region "Eventos de la Página"
    Private Sub CargarGrilla()
        btnAceptarFecha.Visible = False
        Dim dsCuentasPorCobrar As New CuentasPorCobrarPagarBE
        Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.NewCuentasPorCobrarPagarRow
        cuentaPorCobrar.CodigoMercado = ""
        cuentaPorCobrar.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        cuentaPorCobrar.CodigoMoneda = IIf(ddlMoneda.SelectedValue.ToString = "Todos", "", ddlMoneda.SelectedValue.ToString)
        cuentaPorCobrar.CodigoTercero = ddlIntermediario.SelectedValue
        cuentaPorCobrar.CodigoOperacion = ddlOperacion.SelectedValue
        cuentaPorCobrar.FechaOperacion = UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text)
        cuentaPorCobrar.FechaIngreso = UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text)
        cuentaPorCobrar.Egreso = "N"
        Dim fechaIni As Decimal = IIf(txtFechaInicio.Text = "", 0, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text))
        Dim fechaFin As Decimal = IIf(txtFechaFin.Text = "", 0, UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text))
        dsCuentasPorCobrar.CuentasPorCobrarPagar.AddCuentasPorCobrarPagarRow(cuentaPorCobrar)
        Dim dsCuentas As DataSet = New CuentasPorCobrarBM().SeleccionarPorFiltro(dsCuentasPorCobrar, fechaIni, fechaFin, DatosRequest, String.Empty)
        Dim view As DataView = dsCuentas.Tables(0).DefaultView
        view.RowFilter = "Estado <> 'E'"
        dgLista.DataSource = view
        dgLista.DataBind()
        Session("grillaCXC") = dsCuentas.Tables(0)
        Session("GrillaCuentasCobrar") = Nothing
        Dim dtTablaCobrar As New DataTable
        dtTablaCobrar = dsCuentas.Copy.Tables(0)
        Session("GrillaCuentasCobrar") = dtTablaCobrar
        Me.ddlContacto.SelectedIndex = 0
        '  If ddlModeloCarta. = Nothing Then
        'Me.ddlModeloCarta.SelectedIndex = -1
        'Else
        'If ddlModeloCarta.SelectedIndex <= 0 Then
        'Me.ddlModeloCarta.SelectedIndex = 0
        'End If
        'End If
        txtDescripcion.Text = ""
        txtImporte.Text = ""
        Me.ddlNroCuenta.SelectedIndex = 0
        Me.ddlBanco.SelectedIndex = 0
        Me.ddlClase.SelectedIndex = 0
        Me.pnlDestinoDivisas.Attributes.Add("class", "hidden")
        ddlContactoIntermediario.Items.Clear()
        ddlNroCuenta.Items.Clear()
        UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
        Me.lbContador.Text = UIUtility.MostrarResultadoBusqueda(dsCuentas.Tables(0).Rows.Count)
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        If Not Page.IsPostBack Then
            CargaBancoDifRenovacion()
            CargarPortafolio()
            txtPago.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
            CargarOperacion()
            CargarIntermediario("")
            CargarMoneda()
            CargarBanco("", "", "")
            CargarBancoDestino("", ddlPortafolio.SelectedValue, "")

            CargarBancoMatrizOrigen()
            CargarBancoMatrizDestino()

            CargarDetallePortafolio("", "", ddlPortafolio.SelectedValue)
            CargarClaseCuenta()
            CargarTipoPago()
            pnlDestinoDivisas.Attributes.Add("class", "hidden")
            If Not Request.QueryString("NumeroOperacion") Is Nothing Then
                BuscarCuenta(Request.QueryString("NumeroOperacion"))
            End If
            EstablecerFecha()
            btnAceptarFecha.Visible = False
        End If
        Dim _objUtilitario As New UtilDM
        Dim Mensaje As String = _objUtilitario.RetornarMensajeConfirmacion("CONF40")
        Dim mensaje2 As String = _objUtilitario.RetornarMensajeConfirmacion("CONF44")
        btnLiquidar.Attributes.Add("onclick", "return ValidarDatos('" & Mensaje & "');")
        btnAceptarFecha.Attributes.Add("onclick", "return confirm('" & mensaje2 & "');")
        btnSaldos.Attributes.Add("onclick", "return ValidarCuenta();")
    End Sub
    Private Sub BuscarCuenta(ByVal numeroOperacion As String)
        CargarGrilla()
        For Each row As GridViewRow In dgLista.Rows
            If row.Cells(1).Text = numeroOperacion Then
                CType(row.FindControl("chbConfirmar"), CheckBox).Checked = True
                txtImporte.Text = row.Cells(7).Text
                txtDescripcion.Text = HttpUtility.HtmlDecode(row.Cells(6).Text).Trim
                txtFechaVcto.Text = HttpUtility.HtmlDecode(row.Cells(4).Text).Trim
            End If
        Next
    End Sub
    Private Sub Imagebutton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect("../OperacionesCaja/frmConsultaSaldosBancarios.aspx")
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If hdIndLiqAntFondo.Value <> "1" Then
            If UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text) < UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue) Then
                AlertaJS(ObtenerMensaje("ALERT155"))
                Exit Sub
            End If
            If UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text) > UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text) Then
                AlertaJS(ObtenerMensaje("ALERT48"))
                Exit Sub
            End If
        End If
        CargarGrilla()
        LimpiarProcesos()
    End Sub
    Private Sub ddlClase_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlClase.SelectedIndexChanged
        CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, ddlPortafolio.SelectedValue)
    End Sub
    Private Sub ddlBanco_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBanco.SelectedIndexChanged
        CargarDatosBanco()
    End Sub
    Private Sub CargarDatosBanco()
        Call CargarClaseCuenta(True, ddlBanco.SelectedValue)
        CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, ddlPortafolio.SelectedValue)
        CargarContactos()
    End Sub
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub
    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Dim chbConfirmar As CheckBox
        Dim dr As DataRowView
        Dim codigoOperacion As String
        If e.Row.RowType = DataControlRowType.DataRow Then
            dr = CType(e.Row.DataItem, DataRowView)
            If Not (e.Row.FindControl("chbConfirmar") Is Nothing) Then
                chbConfirmar = CType(e.Row.FindControl("chbConfirmar"), CheckBox)
                If (chbConfirmar.Checked) Then
                    codigoOperacion = Convert.ToString(dr("CodigoOperacion"))
                    If (operacionesSeleccionandas.Count = 0) Then
                        operacionesSeleccionandas.Add(codigoOperacion)
                        Me.BindingModeloCarta(codigoOperacion, dr("NroOperacion"))
                    Else
                        If (Not operacionesSeleccionandas.Contains(codigoOperacion)) Then
                            chbConfirmar.Checked = False
                            AlertaJS(ObtenerMensaje("ALERT148"))
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub btnOperaciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOperaciones.Click
        Dim selectedRow As GridViewRow = Nothing
        If Not validarAgrupacion(selectedRow) Then
            If selectedRow Is Nothing Then
                Exit Sub
            End If
        Else
            AlertaJS(ObtenerMensaje("ALERT18"))
            Exit Sub
        End If
        If selectedRow.Cells(13).Text.Trim = "Manual" Then
            AlertaJS(ObtenerMensaje("ALERT129"))
            Exit Sub
        End If
        Dim sPortafolio As String = selectedRow.Cells(14).Text.Trim
        Dim sCategoria As String = selectedRow.Cells(16).Text.Trim
        Dim sCodigoOrden As String = selectedRow.Cells(17).Text.Trim
        Dim sCodigoMnemonico As String = selectedRow.Cells(18).Text.Trim
        Dim sCodigoOperacion As String = selectedRow.Cells(19).Text.Trim
        If sCodigoOrden = "" Or sCodigoOrden = "&nbsp;" Then
            AlertaJS(ObtenerMensaje("ALERT127"))
            Exit Sub
        End If
        If sCategoria = "" Or sCategoria = "&nbsp;" Then
            AlertaJS(ObtenerMensaje("ALERT130"))
            Exit Sub
        End If
        If sCodigoMnemonico = "" Or sCodigoMnemonico = "&nbsp;" Then
            AlertaJS(ObtenerMensaje("ALERT132"))
            Exit Sub
        End If
        If sCodigoOperacion = "" Or sCodigoOperacion = "&nbsp;" Then
            AlertaJS(ObtenerMensaje("ALERT131"))
            Exit Sub
        End If
        Call CargaOrdenInversion(sCategoria, sCodigoOrden, sPortafolio, sCodigoMnemonico, sCodigoOperacion)
    End Sub
    Private Sub CargaOrdenInversion(ByVal sCategoriaInstrumento As String, ByVal sCodigoOrden As String, ByVal sCodigoPortafolioSBS As String, ByVal sCodigoMnemonico As String, ByVal sCodigoOperacion As String)
        Select Case sCategoriaInstrumento
            Case "IE"   'Instrumentos estructurados
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmInstrumentosEstructurados.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
            Case "BO"   'Bonos
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmBonos.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
                'ALGUNOS REGISTROS SE CAEN POR CODIGO DE TERCERO QUE NO ESTA ESTA EN LA RELACION CUSTODIO VALORES
            Case "CS"   'Certificado de suscripción
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmCertificadosSuscripcion.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
                'ALGUNOS REGISTROS SE CAEN POR CODIGO DE TERCERO QUE NO ESTA ESTA EN LA RELACION CUSTODIO VALORES
            Case "DP"   'Depósito a plazos
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmDepositoPlazos.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
            Case "CD"   'Certificado de depósito
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmCertificadoDeposito.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
                'ALGUNOS REGISTROS SE CAEN POR CODIGO DE TERCERO QUE NO ESTA ESTA EN LA RELACION CUSTODIO VALORES
            Case "LH"   'Letras hipotecarias
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmLetrasHipotecarias.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
            Case "OR"   'Operaciones Reporte
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmOperacionesReporte.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
            Case "PA"   'Pagarés
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmPagares.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
            Case "AC"   'Acciones
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmAcciones.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
            Case "PC"   'Papeles comerciales
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmPapelesComerciales.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
            Case "CV"   'Compra y venta de moneda extranjera
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmCompraVentaMonedaExtranjera.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
            Case "FD"   'Opciones derivadas - forward de divisas
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmOpcionesDerivadasForwardDivisas.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
            Case "FI"   'Ordenes Fondo de Inversion
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmOrdenesFondo.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
        End Select
    End Sub
    Private Function VerificaAgrupacion() As Boolean
        Dim i As Integer, j As Integer
        Dim oCheck As CheckBox
        Dim bValor As Boolean
        bValor = False
        j = 0
        For i = 0 To dgLista.Rows.Count - 1
            oCheck = New CheckBox
            oCheck = CType(dgLista.Rows(i).FindControl("chbConfirmar"), CheckBox)
            If oCheck.Checked = True Then
                j = j + 1
            End If
        Next
        If j > 1 Then
            bValor = True
        End If
        Return bValor
    End Function
    Private Function VerificaDatoAgrupado(ByVal nIndice As Integer) As Boolean
        Dim i As Integer, j As Integer
        Dim oCheck As CheckBox
        Dim bValor As Boolean
        Dim sDatoAnt As String = "", sDato As String
        bValor = True
        j = 0
        For i = 0 To dgLista.Rows.Count - 1
            oCheck = New CheckBox
            oCheck = CType(dgLista.Rows(i).FindControl("chbConfirmar"), CheckBox)
            If oCheck.Checked = True Then
                j = j + 1
                If j = 1 Then
                    sDatoAnt = dgLista.Rows(i).Cells(nIndice).Text
                End If

                sDato = dgLista.Rows(i).Cells(nIndice).Text
                If sDato <> sDatoAnt Then
                    bValor = False
                    Return bValor
                    Exit Function
                End If
            End If
        Next
        Return bValor
    End Function
    Private Sub btnLiquidar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLiquidar.Click
        Dim ordenInverscionBM As New OrdenInversionWorkFlowBM
        Dim dt As DataTable
        Dim oCuentasPorCobrar As New CuentasPorCobrarBM
        Dim sNroCartaPortafolio As String
        Dim Agrupado As String = ""
        Try
            sNroCartaPortafolio = ""
            If (VerificaAgrupacion()) Then 'Verifica si esta agrupado
                If Not VerificaDatoAgrupado(9) Then    'para Portafolio
                    AlertaJS(ObtenerMensaje("ALERT146"))
                    Exit Sub
                End If
                If Not VerificaDatoAgrupado(10) Then    'para Moneda
                    AlertaJS(ObtenerMensaje("ALERT147"))
                    Exit Sub
                End If
                If Not VerificaDatoAgrupado(12) Then    'para Codigo Operacion
                    AlertaJS(ObtenerMensaje("ALERT148"))
                    Exit Sub
                End If
                If Not VerificaDatoAgrupado(20) Then    'para Codigo Renta
                    AlertaJS(ObtenerMensaje("ALERT149"))
                    Exit Sub
                End If
                If ddlModeloCarta.SelectedValue = "CO02" Then
                    If ValidaRenovacion() = False Then
                        AlertaJS("La carta de renovación requiere que se seleccione algun vencimiento de la lista.")
                        Exit Sub
                    End If
                End If
                Dim oDSNroCartaPortafolio As DataSet = oCuentasPorCobrar.ObtenerNumeroCartaPortafolio(ddlNroCuenta.SelectedValue.Split(",")(0), DatosRequest)
                If oDSNroCartaPortafolio.Tables.Count > 0 Then
                    sNroCartaPortafolio = oDSNroCartaPortafolio.Tables(0).Rows(0).Item(0)
                End If
                oDSNroCartaPortafolio = Nothing
                Agrupado = "A"
            End If
            If pnlDestinoDivisas.Attributes("class") = "hidden" Then
                If Not ValidarMoneda() Then
                    AlertaJS(ObtenerMensaje("ALERT71"))
                    Exit Sub
                End If
            Else
                If (Me.ddlBancoDestino.SelectedValue = "") Then
                    AlertaJS("Debe especificar el banco destino.")
                    Exit Sub
                End If
                If (Me.ddlClaseCuentaDestino.SelectedValue = "") Then
                    AlertaJS("Debe especificar la clase de cuenta destino.")
                    Exit Sub
                End If
                If (Me.ddlCuentaDestino.SelectedValue = "") Then
                    AlertaJS("Debe especificar la cuenta destino.")
                    Exit Sub
                End If
            End If
            For Each row As GridViewRow In dgLista.Rows
                If CType(row.FindControl("chbConfirmar"), CheckBox).Checked Then
                    If "" = "1" Then 'LOCAL
                        If Not ValidarLiquidacion_Matriz(row) Then Exit Sub
                    End If
                    Dim dsOpCaja As New OperacionCajaBE
                    Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()
                    Dim CodigoContacto As String

                    opCaja.NumeroOperacion = row.Cells(1).Text
                    opCaja.NumeroCuenta = ddlNroCuenta.SelectedValue.Split(",")(1)
                    opCaja.CodigoPortafolioSBS = ddlNroCuenta.SelectedValue.Split(",")(0)
                    opCaja.FechaPago = UIUtility.ConvertirFechaaDecimal(txtPago.Text)
                    opCaja.CodigoOperacionCaja = "1"
                    opCaja.CodigoModalidadPago = ddlTipoPago.SelectedValue
                    opCaja.CodigoModelo = ddlModeloCarta.SelectedValue
                    opCaja.CodigoMercado = ""
                    opCaja.CodigoClaseCuenta = ""
                    opCaja.CodigoTerceroDestino = ddlBancoRenovacion.SelectedValue
                    opCaja.Importe = Decimal.Parse(row.Cells(7).Text.Replace(".", UIUtility.DecimalSeparator()))
                    opCaja.TasaImpuesto = Nothing
                    opCaja.CodigoContactoIntermediario = ddlContactoIntermediario.SelectedValue
                    'dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)
                    opCaja.BancoMatrizOrigen = ddlBancoMatrizOrigen.SelectedValue
                    opCaja.BancoMatrizDestino = ddlBancoMatrizDestino.SelectedValue
                    opCaja.ObservacionCartaDestino = txtObservacionDestino.Text
                    opCaja.BancoGlosaDestino = ddlBancoGlosaEgreso.SelectedValue
                    dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)

                    If ddlContacto.Visible Then
                        CodigoContacto = Me.ddlContacto.SelectedValue
                    Else
                        CodigoContacto = Me.ddlContactoDivisa.SelectedValue
                    End If

                    If pnlDestinoDivisas.Attributes("class") Is Nothing Then


                        If Me.hdOperacion.Value = "93" Or txtDescripcion.Text.Trim.Substring(0, 4) = "FORW" Then

                            CodigoContacto = Me.ddlContactoDivisa.SelectedValue
                        End If

                        oCuentasPorCobrar.Liquidar(dsOpCaja, sNroCartaPortafolio, CodigoContacto, DatosRequest, ddlBanco.SelectedValue, ddlBancoDestino.SelectedValue, ddlCuentaDestino.SelectedValue.Split(",")(1), Agrupado, txtObservacionCarta.Text, String.Empty)
                    Else
                        oCuentasPorCobrar.Liquidar(dsOpCaja, sNroCartaPortafolio, CodigoContacto, DatosRequest, String.Empty, String.Empty, String.Empty, Agrupado, txtObservacionCarta.Text, String.Empty)
                    End If
                    If ddlModeloCarta.SelectedValue = "CO02" Then
                        InsertarRenovacion(row.Cells(1).Text)
                    End If
                End If
            Next
            'Solo en el caso de Divisas
            If pnlDestinoDivisas.Attributes("class") Is Nothing Then
                For Each row As GridViewRow In dgLista.Rows
                    If CType(row.FindControl("chbConfirmar"), CheckBox).Checked Then
                        Dim dsOpCaja As New OperacionCajaBE
                        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()
                        Dim CodigoContacto As String

                        opCaja.BancoMatrizOrigen = ""
                        opCaja.BancoMatrizDestino = ""
                        opCaja.BancoGlosaDestino = ""
                        opCaja.NumeroOperacion = row.Cells(1).Text
                        opCaja.NumeroCuenta = ddlCuentaDestino.SelectedValue.Split(",")(1)
                        opCaja.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                        opCaja.CodigoMercado = ""
                        opCaja.CodigoClaseCuenta = ddlClaseCuentaDestino.SelectedValue
                        opCaja.FechaPago = UIUtility.ConvertirFechaaDecimal(txtPago.Text)
                        opCaja.CodigoOperacionCaja = "1"
                        opCaja.CodigoModelo = ddlModeloCarta.SelectedValue
                        opCaja.CodigoModalidadPago = ddlTipoPago.SelectedValue
                        opCaja.CodigoMoneda = Me.hidCodigoMonedaDestino.Value
                        opCaja.CodigoContactoIntermediario = ddlContactoIntermediario.SelectedValue
 
                        opCaja.BancoMatrizOrigen = ddlBancoMatrizOrigen.SelectedValue
                        opCaja.BancoMatrizDestino = ddlBancoMatrizDestino.SelectedValue
                        opCaja.ObservacionCartaDestino = txtObservacionDestino.Text
                        opCaja.BancoGlosaDestino = ddlBancoGlosaEgreso.SelectedValue

                        If ddlContacto.Visible Then
                            CodigoContacto = Me.ddlContacto.SelectedValue
                        Else
                            CodigoContacto = Me.ddlContactoDivisa.SelectedValue
                        End If
                        dt = ordenInverscionBM.GetOrdenInversionDivisas(ddlPortafolio.SelectedValue, opCaja.NumeroOperacion)
                        If Not (dt Is Nothing) Then
                            If row.Cells(19).Text = "93" Or row.Cells(19).Text = "94" Then
                                opCaja.Importe = Convert.ToDecimal(dt.Rows(0)("MontoOperacion"))
                            Else
                                opCaja.Importe = Convert.ToDecimal(dt.Rows(0)("MontoDestino"))
                            End If
                            dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)
                            oCuentasPorCobrar.LiquidarDivisas(dsOpCaja, sNroCartaPortafolio, CodigoContacto, Agrupado, DatosRequest)
                        End If
                    End If
                Next
            End If
            AlertaJS(ObtenerMensaje("ALERT154"))
            LimpiarRenovacion()
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Function ValidarLiquidacion_Matriz(ByVal dataRow As GridViewRow) As Boolean
        Dim strLiqBanco As String = dataRow.Cells(23).Text
        Dim strLiqOperacion As String = dataRow.Cells(19).Text
        Dim strLiqCategoria As String = dataRow.Cells(16).Text
        Dim strOtroBancoCuenta As String = ObtieneCC_SC()
        Dim blnResultado As Boolean = False

        If strLiqCategoria = "BO" Then                          'BONOS
            If strLiqOperacion = "2" Then                           'VENTAS
                If ddlBanco.SelectedValue = strLiqBanco Then            'MB
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Bonos - Venta - C/C - MB")
                Else
                    If strOtroBancoCuenta = "C/C" Then                      'C/C
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Bonos - Venta - C/C - OtroBanco")
                    ElseIf strOtroBancoCuenta = "S/C" Then                  'S/C
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Bonos - Venta - S/C - OtroBanco")
                    Else
                        blnResultado = True
                    End If
                End If
            ElseIf strLiqOperacion = "1" Then                       'COMPRAS
                If ddlBanco.SelectedValue = strLiqBanco Then            'MB
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Bonos - Compras - C/C - MB")
                Else
                    If strOtroBancoCuenta = "S/C" Then                      'S/C
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Bonos - Compras - S/C - OtroBanco")
                    Else
                        blnResultado = True
                    End If
                End If
            Else
                blnResultado = True
            End If
        ElseIf strLiqCategoria = "AC" Then                      'ACCIONES
            If strLiqOperacion = "2" Then                           'VENTAS
                If ddlBanco.SelectedValue = strLiqBanco Then            'MB
                    blnResultado = ValidaCampos_Matriz(True, True, True, False, False, False, "Acciones - Ventas - C/C - MB")
                Else
                    blnResultado = True
                End If
            ElseIf strLiqOperacion = "1" Then                       'COMPRAS
                If ddlBanco.SelectedValue = strLiqBanco Then            'MB
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Acciones - Compras - C/C - MB")
                Else
                    blnResultado = True
                End If
            Else
                blnResultado = True
            End If
        Else
            If ddlBanco.SelectedValue = strLiqBanco Then            'MB
                If strLiqOperacion = "4" Then       'DPZ - Cancelacion
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "DPZ - Cancelación - C/C - MB")
                ElseIf strLiqOperacion = "3" Then   'DPZ - Constitucion
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "DPZ - Constitución - C/C - MB")
                ElseIf strLiqOperacion = "35" Then  'Cobro Vcmtos Interes
                    blnResultado = ValidaCampos_Matriz(True, True, True, False, False, False, "Cobro Vcmtos Interes - C/C - MB")
                ElseIf strLiqOperacion = "38" Then  'Cobro Amortiz Capital
                    blnResultado = ValidaCampos_Matriz(True, True, True, False, False, False, "Cobro Amortiz Capital - C/C - MB")
                ElseIf strLiqOperacion = "66" Then  'Operación de Cambios - Venta
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Operación de Cambios - Venta - C/C - MB")
                ElseIf strLiqOperacion = "65" Then  'Operación de Cambios - Compra
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Operación de Cambios - Compra - C/C - MB")
                Else
                    blnResultado = True
                End If
            Else
                If strOtroBancoCuenta = "C/C" Then 'C/C
                    If strLiqOperacion = "4" Then 'DPZ - Cancelacion
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "DPZ - Cancelacion - C/C - OtroBanco")
                    ElseIf strLiqOperacion = "66" Then  'Operación de Cambios - Venta
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Operación de Cambios - Venta - C/C - OtroBanco")
                    ElseIf strLiqOperacion = "65" Then  'Operación de Cambios - Compra
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Operación de Cambios - Compra - C/C - OtroBanco")
                    Else
                        blnResultado = True
                    End If
                ElseIf strOtroBancoCuenta = "S/C" Then                  'S/C
                    If strLiqOperacion = "4" Then       'DPZ - Cancelacion
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "DPZ - Cancelación - S/C - OtroBanco")
                    ElseIf strLiqOperacion = "3" Then   'DPZ - Constitucion
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "DPZ - Constitución - S/C - OtroBanco")
                    ElseIf strLiqOperacion = "66" Then  'Operación de Cambios - Venta
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Operación de Cambios - Venta - S/C - OtroBanco")
                    ElseIf strLiqOperacion = "65" Then  'Operación de Cambios - Compra
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, False, "Operación de Cambios - Compra - S/C - OtroBanco")
                    Else
                        blnResultado = True
                    End If
                Else
                    blnResultado = True
                End If
            End If
        End If
        Return blnResultado
    End Function
    Private Function ValidaCampos_Matriz(ByVal blnBanco As Boolean, ByVal blnTipoPago As Boolean, ByVal blnClaseCuenta As Boolean, ByVal blnModeloCarta As Boolean, ByVal blnContacto As Boolean, ByVal blnContactoIntermediario As Boolean, ByVal strMensaje As String) As Boolean
        Dim blnResul As Boolean = True
        If blnBanco = True And ddlBanco.SelectedValue = "" Then blnResul = False
        If blnTipoPago = True And ddlTipoPago.SelectedValue = "" Then blnResul = False
        If blnClaseCuenta = True And ddlClase.SelectedValue = "" Then blnResul = False
        If blnModeloCarta = True And ddlModeloCarta.SelectedValue = "" Then blnResul = False
        If ddlContacto.Visible Then '954
            If blnContacto = True And ddlContacto.SelectedValue = "" Then blnResul = False
        Else
            If blnContacto = True And Me.ddlContactoDivisa.SelectedValue = "" Then blnResul = False
        End If
        If blnContactoIntermediario = True And ddlContactoIntermediario.SelectedValue = "" Then blnResul = False
        If blnResul = False Then
            AlertaJS("Ingrese correctamente los campos para: " + strMensaje)
        End If
        Return blnResul
    End Function
    Private Function ObtieneCC_SC() As String
        Dim objAux As New CuentaEconomicaBM
        Dim dsAux As DataSet = objAux.EncontrarCuentasExistentes(ddlBanco.SelectedValue, DatosRequest)
        Return dsAux.Tables(0).Rows(0)(0)
    End Function
    Public Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim selectedRow As GridViewRow = Nothing
        Dim codigoOperacion As String = String.Empty
        Dim codigoMercado As String = String.Empty
        Dim codigoOrden As String = String.Empty
        Dim totalDestino As Decimal
        Dim ordenInverscionBM As New OrdenInversionWorkFlowBM
        Dim dt As New DataTable
        Dim dtDiv As New DataTable
        Dim preLiquidacionBM As New PreLiquidacionDivisasBM
        Dim importeTotal As Decimal = 0
        Dim concuenta As String
        Dim i As Integer
        Dim rowDatos As GridViewRow = CType(CType(sender, CheckBox).NamingContainer, GridViewRow)
        txtImporte.Text = ""
        txtDescripcion.Text = ""
        txtFechaVcto.Text = ""
        btnAceptarFecha.Visible = False
        lblMonedaDestino.Text = ""
        txtImporteDestino.Text = ""
        txtPago.Text = txtFechaInicio.Text
        LimpiarProcesos()
        Try
            Me.ddlContacto.DataSource = Nothing
            Me.ddlContacto.DataBind()
            Me.ddlContactoDivisa.DataSource = Nothing
            Me.ddlContactoDivisa.DataBind()
            If Not validarAgrupacion(selectedRow, importeTotal) Then
                If Not selectedRow Is Nothing Then
                    If ddlMoneda.SelectedValue = "" Then
                        oRow = New MonedaBM().SeleccionarPorFiltro(selectedRow.Cells(10).Text, "", "", "", "", DatosRequest).Tables(0).Rows(0)
                        lblMoneda.Text = "[" & oRow.CodigoMoneda & "] " & oRow.Descripcion
                        hidCodigoMonedaOrigen.Value = oRow.CodigoMoneda
                    Else
                        lblMoneda.Text = "[" & ddlMoneda.SelectedValue & "] " & ddlMoneda.SelectedItem.Text
                        hidCodigoMonedaOrigen.Value = ddlMoneda.SelectedValue
                    End If
                    txtImporte.Text = selectedRow.Cells(7).Text
                    txtDescripcion.Text = HttpUtility.HtmlDecode(selectedRow.Cells(6).Text).Trim
                    txtFechaVcto.Text = HttpUtility.HtmlDecode(selectedRow.Cells(4).Text).Trim
                    btnAceptarFecha.Visible = True
                    Me.hdOperacion.Value = HttpUtility.HtmlDecode(selectedRow.Cells(19).Text).Trim
                    If (Me.ddlContacto.Items.Count < 1) Then
                        Me.ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
                    End If
                    Me.hdIntermediario.Value = HttpUtility.HtmlDecode(rowDatos.Cells(11).Text).Trim
                    hdCodigoTrecero.Value = HttpUtility.HtmlDecode(selectedRow.Cells(22).Text)
                    Me.hdMercado.Value = selectedRow.Cells(21).Text
                    Me.hdSAB.Value = selectedRow.Cells(23).Text
                    For i = 1 To Me.ddlBanco.Items.Count - 1
                        concuenta = ddlBanco.Items(i).Text()
                        If (concuenta = Me.hdIntermediario.Value) Then
                            Me.hdnSinCuenta.Value = "NO"
                            Exit For
                        Else
                            Me.hdnSinCuenta.Value = "YES"
                        End If
                    Next

                    sCodigoMnemonio = HttpUtility.HtmlDecode(selectedRow.Cells(18).Text)
                    Dim objContacto As New ContactoBM
                    Dim dsContactoIntermediario As New DataSet
                    dsContactoIntermediario = objContacto.ListarContactoPorTercerosTesoreria(selectedRow.Cells(22).Text)
                    HelpCombo.LlenarComboBox(ddlContactoIntermediario, dsContactoIntermediario.Tables(0), "CodigoContacto", "DescripcionContacto", False)
                    Me.ddlContactoIntermediario.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
                    hdCantIntContacto.Value = Me.ddlContactoIntermediario.Items.Count
                End If
            Else
                txtImporte.Text = importeTotal.ToString
            End If
            If (Not selectedRow Is Nothing) Then
                codigoOperacion = selectedRow.Cells(19).Text
                codigoMercado = selectedRow.Cells(21).Text
                'If (codigoMercado = "2" And (codigoOperacion = "36" Or codigoOperacion = "37" Or _
                'codigoOperacion = "35" Or codigoOperacion = "38" Or codigoOperacion = "39")) Then
                'ActivarControlParaDividendoOCupon(True)
                'Else
                '   ActivarControlParaDividendoOCupon(False)
                'chkAfectoAlDescuento.Checked = False
                'End If
                'Else
                'ActivarControlParaDividendoOCupon(False)
                'chkAfectoAlDescuento.Checked = False
            End If
            If Not selectedRow Is Nothing Then
                codigoOperacion = selectedRow.Cells(19).Text
                If (codigoOperacion = "65" Or codigoOperacion = "66") Then
                    Me.pnlDestinoDivisas.Attributes.Remove("class")
                    Me.ddlContacto.Visible = False
                    Me.lblContacto.Visible = False
                    For Each row As GridViewRow In dgLista.Rows
                        If CType(row.FindControl("chbConfirmar"), CheckBox).Checked Then
                            codigoOrden = row.Cells(17).Text
                            dt = ordenInverscionBM.GetOrdenInversionDivisas(ddlPortafolio.SelectedValue, codigoOrden)
                            dtDiv = preLiquidacionBM.SeleccionarPorCodigoOrden(codigoOrden, DatosRequest).DatosOrdenInversion
                            If Not (dt Is Nothing) Then
                                If Not dt.Rows(0)("MontoDestino") Is DBNull.Value Then
                                    totalDestino = totalDestino + Convert.ToDecimal(dt.Rows(0)("MontoDestino"))
                                End If
                                Me.lblMonedaDestino.Text = Convert.ToString(dt.Rows(0)("Moneda"))
                                Me.hidCodigoMonedaOrigen.Value = Convert.ToString(dt.Rows(0)("CodigoMonedaOrigen"))
                                Me.hidCodigoMonedaDestino.Value = Convert.ToString(dt.Rows(0)("CodigoMonedaDestino"))
                                CargarBancoDestino(String.Empty, ddlPortafolio.SelectedValue, Me.hidCodigoMonedaDestino.Value)
                                Me.CargarBanco("", ddlPortafolio.SelectedValue, hidCodigoMonedaOrigen.Value, True)
                                Me.CargarClaseCuenta(True, ddlBanco.SelectedValue)
                                Me.CargarDetallePortafolio(ddlBanco.SelectedValue, String.Empty, ddlPortafolio.SelectedValue, True)
                            End If
                            If Not (dtDiv Is Nothing) Then
                                If dtDiv.Rows.Count > 0 Then
                                    Dim drBE As DatosOrdenInversionBE.DatosOrdenInversionRow = dtDiv.Rows(0)
                                    ddlBanco.SelectedValue = drBE.BancoOrigen
                                    CargarDatosBanco()
                                    ddlClase.SelectedValue = drBE.CodigoClaseCuenta
                                    CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, ddlPortafolio.SelectedValue)
                                    ddlNroCuenta.SelectedValue = ddlPortafolio.SelectedValue & "," & drBE.NumeroCuenta
                                    ddlBancoDestino.SelectedValue = drBE.BancoDestino
                                    CargarDatosBancoDestino()
                                    ddlContactoDivisa.SelectedValue = drBE.CodigoContacto
                                    ddlClaseCuentaDestino.SelectedValue = drBE.CodigoClaseCuentaDestino
                                    CargarCuentasDestino(String.Empty, String.Empty, ddlPortafolio.SelectedValue, True)
                                    ddlCuentaDestino.SelectedValue = ddlPortafolio.SelectedValue & "," & drBE.NumeroCuentaDestino
                                    ddlContactoIntermediario.SelectedValue = drBE.CodigoContactoIntermediario
                                    BindingModeloCarta("65", "")
                                    ddlTipoPago.SelectedValue = drBE.CodigoModalidadPago
                                    ddlModeloCarta.SelectedValue = drBE.CodigoModelo
                                End If
                            End If
                            ddlBanco_SelectedIndexChanged(Nothing, Nothing)
                            ddlBancoDestino_SelectedIndexChanged(Nothing, Nothing)
                        End If
                    Next
                    Me.txtImporteDestino.Text = totalDestino.ToString()
                    If (Me.ddlContacto.Items.Count < 1) Then
                        Me.ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
                    End If
                ElseIf (codigoOperacion = "93" Or codigoOperacion = "94") Then
                    Me.pnlDestinoDivisas.Attributes.Remove("class")
                    Me.ddlContacto.Visible = True
                    Me.lblContacto.Visible = True
                    For Each row As GridViewRow In dgLista.Rows
                        If CType(row.FindControl("chbConfirmar"), CheckBox).Checked Then
                            codigoOrden = row.Cells(17).Text
                            dt = ordenInverscionBM.GetOrdenInversionDivisas(ddlPortafolio.SelectedValue, codigoOrden)
                            If dt.Rows(0)("Delibery") = "N" Then Exit For
                            If Not (dt Is Nothing) Then
                                If Not dt.Rows(0)("MontoOperacion") Is DBNull.Value Then
                                    totalDestino = totalDestino + Convert.ToDecimal(dt.Rows(0)("MontoOperacion"))
                                End If
                                Me.lblMonedaDestino.Text = Convert.ToString(dt.Rows(0)("Moneda"))
                                Me.hidCodigoMonedaOrigen.Value = Convert.ToString(dt.Rows(0)("CodigoMonedaOrigen"))
                                Me.hidCodigoMonedaDestino.Value = Convert.ToString(dt.Rows(0)("CodigoMonedaDestino"))
                                CargarBancoDestino(String.Empty, ddlPortafolio.SelectedValue, Me.hidCodigoMonedaDestino.Value)
                                CargarDatosBancoDestino()
                                Me.CargarBanco("", ddlPortafolio.SelectedValue, hidCodigoMonedaOrigen.Value, True)
                                Me.CargarClaseCuenta(True, ddlBanco.SelectedValue)
                                Me.CargarDetallePortafolio(ddlBanco.SelectedValue, String.Empty, ddlPortafolio.SelectedValue, True)
                            End If
                            If row.Cells(21).Text = "1" Then
                                Me.BindingModeloCarta(codigoOperacion, "")
                            End If
                        End If
                    Next
                    If dt.Rows(0)("Delibery") = "N" Then
                        Me.pnlDestinoDivisas.Attributes.Add("class", "hidden")
                        Me.ddlContacto.Visible = True
                        Me.lblContacto.Visible = True
                    Else
                        Me.ddlContacto.Visible = False
                        Me.lblContacto.Visible = False
                        Me.txtImporteDestino.Text = totalDestino.ToString()
                        If (Me.ddlContacto.Items.Count < 1) Then
                            Me.ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
                        End If
                    End If
                Else
                    Me.pnlDestinoDivisas.Attributes.Add("class", "hidden")
                    Me.ddlContacto.Visible = True
                    Me.lblContacto.Visible = True
                End If
            Else
                Me.pnlDestinoDivisas.Attributes.Add("class", "hidden")
                Me.ddlContacto.Visible = True
                Me.lblContacto.Visible = True
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
#End Region
#Region "Metodos de control"
    Private Function validarAgrupacion(ByRef diCuenta As GridViewRow, Optional ByRef total As Decimal = 0) As Boolean
        Dim count As Integer = 0
        Dim codigoOperacion As String
        Dim codigoOperacionID As String

        For Each row As GridViewRow In dgLista.Rows
            If CType(row.FindControl("chbConfirmar"), CheckBox).Checked Then
                count = count + 1
                diCuenta = row
                total = total + Decimal.Parse(row.Cells(7).Text)
                codigoOperacion = row.Cells(12).Text
                codigoOperacionID = row.Cells(19).Text
                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#e3d829")
                'Verifica la fecha de Operacion que nose mayo ala fecha del portafolio
                If (UIUtility.ConvertirFechaaDecimal(Me.txtFechaInicio.Text) <> UIUtility.ConvertirFechaaDecimal(row.Cells(4).Text)) Then
                    If (codigoOperacionID <> "3") Then
                        btnLiquidar.Visible = False
                        btnAceptarFecha.Visible = True
                    End If
                End If
                If (operacionesSeleccionandas.Count = 0) Then
                    operacionesSeleccionandas.Add(codigoOperacion)
                    'Me.BindingModeloCarta(row.Cells(19).Text, IIf(row.Cells(6).Text.Substring(0, 11) = "FORWARD-NOM", "N",  IIf(row.Cells(6).Text.Substring(0, 11) = "FORWARD-DEL", "S", "")), "N")  'HDG OT 63063 R09 20110721
                    Me.BindingModeloCarta(row.Cells(19).Text, row.Cells(5).Text) 'OT12012
                Else
                    If (Not operacionesSeleccionandas.Contains(codigoOperacion)) Then
                        CType(row.FindControl("chbConfirmar"), CheckBox).Checked = False
                        row.BackColor = System.Drawing.Color.White
                        diCuenta = Nothing
                        AlertaJS(ObtenerMensaje("ALERT148"))
                    End If
                End If
            Else
                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff")
            End If
        Next
        If count > 1 Then
            txtImporte.Text = total
            Return True
        End If
        Return False
    End Function
    Private Function ValidarMoneda() As Boolean
        Dim oCuentaEconomica As New CuentaEconomicaBM
        Dim dr As DataRow = oCuentaEconomica.SeleccionarPorFiltro("", "", "", "", "", DatosRequest).Tables(1).Select("NumeroCuenta='" & ddlNroCuenta.SelectedValue.Split(",")(1) & "'")(0)
        Dim codMoneda As String = dr("CodigoMoneda")
        For Each row As GridViewRow In dgLista.Rows
            If CType(row.FindControl("chbConfirmar"), CheckBox).Checked Then
                Dim cuentaMoneda As String = row.Cells(15).Text
                If codMoneda <> cuentaMoneda Then
                    Return False
                End If
            End If
        Next
        Return True
    End Function
    Private Sub CargarContactos()
        Me.ddlContacto.DataSource = Nothing
        Me.ddlContacto.DataBind()
        Me.ddlContacto.DataTextField = "DescripcionContacto"
        Me.ddlContacto.DataValueField = "CodigoContacto"
        ddlContacto.DataSource = New ContactoBM().ListarPorCodigoEntidad(ddlBanco.SelectedValue)
        Me.ddlContacto.DataBind()
        Me.ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub
    Private Sub CargarContactosDivisas()
        ddlContactoDivisa.Items.Clear()
        Dim dtContactos As DataTable = New ContactoBM().ListarPorCodigoEntidad(ddlBancoDestino.SelectedValue).Tables(0)
        If dtContactos.Rows.Count > 0 Then
            Me.ddlContactoDivisa.DataTextField = "DescripcionContacto"
            Me.ddlContactoDivisa.DataValueField = "CodigoContacto"
            Me.ddlContactoDivisa.DataSource = dtContactos
            Me.ddlContactoDivisa.DataBind()
            Me.ddlContactoDivisa.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        End If
    End Sub
    'Private Sub ActivarControlParaDividendoOCupon(ByVal valor As Boolean)
    '    lblTasaImpuestos.Visible = valor
    '    txtTasaImpuestos.Visible = valor
    '    chkAfectoAlDescuento.Visible = valor
    '    chkAfectoAlDescuento.Checked = False
    '    ViewState("ImporteOriginal") = ""
    '    txtImporte.ReadOnly = Not valor
    'End Sub
#End Region
    Private Sub ddlMoneda_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        lblMoneda.Text = ""
        If ddlMoneda.SelectedIndex > 0 Then
            oRow = New MonedaBM().SeleccionarPorFiltro(ddlMoneda.SelectedValue, "", "", "", "", DatosRequest).Tables(0).Rows(0)
            lblMoneda.Text = "[" & oRow.SinonimoISO & "] " & ddlMoneda.SelectedItem.Text
        End If
        dgLista.DataSource = Nothing
        dgLista.DataBind()
        Call CargarBanco("", ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
        Call CargarDetallePortafolio("", "", "", False)
        Call CargarClaseCuenta(False, "")
        Call CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, ddlPortafolio.SelectedValue, True)
    End Sub
    Private Sub EstablecerFecha()
        txtFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        txtFechaFin.Text = txtFechaInicio.Text
        txtPago.Text = txtFechaInicio.Text
        hidFechaVcto.Value = UIUtility.ConvertirFechaaDecimal(Me.txtFechaInicio.Text)
    End Sub
    Private Function MostrarSaldos() As Boolean
        Dim sCodigoPortafolio As String = ddlPortafolio.SelectedValue
        Dim sNombrePortafolio As String = ddlPortafolio.SelectedItem.Text
        Dim sNumeroCuenta As String = ddlNroCuenta.SelectedItem.Text.Trim
        Dim sBanco As String = ddlBanco.SelectedValue
        Dim sBancoNombre As String = ddlBanco.SelectedItem.Text.Trim
        Dim sFecha As String = Me.txtPago.Text.Trim
        Dim strMensaje As New StringBuilder
        If sCodigoPortafolio.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT134"))
            Return False
            Exit Function
        ElseIf sNumeroCuenta.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT135"))
            Return False
            Exit Function
        ElseIf sBanco.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT136"))
            Return False
            Exit Function
        End If
        Dim oSaldoBancario As New SaldosBancariosBM
        Dim dsCuentaEconomica As New CuentaEconomicaBE
        Dim roCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.NewCuentaEconomicaRow
        roCuentaEconomica.CodigoPortafolioSBS = sCodigoPortafolio
        roCuentaEconomica.NumeroCuenta = sNumeroCuenta
        roCuentaEconomica.CodigoMoneda = ""
        roCuentaEconomica.CodigoClaseCuenta = ""
        roCuentaEconomica.CodigoTercero = ddlBanco.SelectedValue
        roCuentaEconomica.FechaCreacion = 0
        dsCuentaEconomica.CuentaEconomica.AddCuentaEconomicaRow(roCuentaEconomica)
        Dim oSBDS As DataSet = oSaldoBancario.SeleccionarPorFiltro2(dsCuentaEconomica, DatosRequest)
        If oSBDS.Tables(0).Rows.Count <= 0 Then
            Return False
            Exit Function
        End If
        If oSBDS.Tables(0).Rows.Count > 0 Then
            Dim sSaldoContable As Decimal = oSBDS.Tables(0).Rows(0).Item("SaldoContable")
            Dim sSaldoDisponible As Decimal = oSBDS.Tables(0).Rows(0).Item("SaldoDisponible")
            Dim sClaseCuenta As String = oSBDS.Tables(0).Rows(0).Item("ClaseDescripcion")
            Dim sMoneda As String = oSBDS.Tables(0).Rows(0).Item("DescripcionMoneda")
            strMensaje.Append("<table style=""width: 100%; text-align: left; font-weight: normal;"">")
            strMensaje.Append("<tr><td style=""width: 40%;"">Portafolio :</td><td>" & sNombrePortafolio & "</td></tr>")
            strMensaje.Append("<tr><td style=""width: 40%;"">Cuenta :</td><td>" & sNumeroCuenta & "</td></tr>")
            strMensaje.Append("<tr><td style=""width: 40%;"">Banco :</td><td>" & sBancoNombre & "</td></tr>")
            strMensaje.Append("<tr><td style=""width: 40%;"">Fecha :</td><td>" & sFecha & "</td></tr>")
            strMensaje.Append("</table>")
            strMensaje.Append("<hr>")
            strMensaje.Append("<table style=""width: 100%; text-align: left; font-weight: normal;"">")
            strMensaje.Append("<tr><td style=""width: 40%;"">Saldo Contable :</td><td>" & String.Format("{0:#,##0.0000000}", sSaldoContable) & "</td></tr>")
            strMensaje.Append("<tr><td style=""width: 40%;"">Saldo Disponible :</td><td>" & String.Format("{0:#,##0.0000000}", sSaldoDisponible) & "</td></tr>")
            strMensaje.Append("<tr><td style=""width: 40%;"">Clase Cuenta :</td><td>" & sClaseCuenta & "</td></tr>")
            strMensaje.Append("<tr><td style=""width: 40%;"">Moneda :</td><td>" & sMoneda & "</td></tr>")
            strMensaje.Append("</table>")
            AlertaJS(strMensaje.ToString())
        End If
        Return True
    End Function
    Private Sub btnSaldos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaldos.Click
        If Not MostrarSaldos() Then
            AlertaJS(ObtenerMensaje("ALERT137"))
            Exit Sub
        End If
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        dgLista.DataSource = Nothing
        dgLista.DataBind()
        Call CargarBanco("", ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
        Call CargarBancoDestino(String.Empty, ddlPortafolio.SelectedValue, String.Empty)
        Call CargarDetallePortafolio("", "", "", False)
        Call CargarClaseCuenta(False, "")
        Call CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, ddlPortafolio.SelectedValue, True)
        EstablecerFecha()
    End Sub
    Private Sub ibImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibImprimir.Click
        Dim Liquidaciones As String
        Liquidaciones = Me.txtFechaInicio.Text & "," & Me.txtFechaFin.Text & "," & ddlPortafolio.SelectedItem.Text & "," & "Todos" & "," & ddlMoneda.SelectedItem.Text & "," & ddlIntermediario.SelectedItem.Text & "," & ddlOperacion.SelectedItem.Text
        EjecutarJS(UIUtility.MostrarPopUp("../Reportes/frmVisorTesoreria.aspx?LiquidacionesCobrar=" & Liquidaciones, "no", 1100, 850, 40, 150, "no", "yes", "yes", "yes"), False)
    End Sub  
    Private Sub btnAceptarFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptarFecha.Click
        Dim selectedRow As GridViewRow = Nothing
        If Not validarAgrupacion(selectedRow) Then
            If selectedRow Is Nothing Then
                AlertaJS(ObtenerMensaje("ALERT18"))
                Exit Sub
            End If
        Else
            AlertaJS(ObtenerMensaje("ALERT18"))
            Exit Sub
        End If
        If Convert.ToDateTime(txtFechaVcto.Text) < Convert.ToDateTime(txtFechaInicio.Text) And hdIndLiqAntFondo.Value <> "1" Then
            AlertaJS("La fecha de Vencimiento debe ser mayor o igual a " + txtFechaInicio.Text)
            Exit Sub
        End If
        Try
            If txtFechaVcto.Text <> "" Then
                Dim codPortafolio As String = selectedRow.Cells(14).Text
                Dim nroOperacion As String = selectedRow.Cells(5).Text
                Dim oCuentasPorCobrar As New CuentasPorCobrarBM
                oCuentasPorCobrar.ModificarFechaVencimiento(nroOperacion, codPortafolio, UIUtility.ConvertirFechaaDecimal(txtFechaVcto.Text), DatosRequest)
            End If
            If (UIUtility.ConvertirFechaaDecimal(Me.txtFechaInicio.Text) = UIUtility.ConvertirFechaaDecimal(txtFechaVcto.Text)) Or hdIndLiqAntFondo.Value = "1" Then
                btnLiquidar.Visible = True
                btnAceptarFecha.Visible = False
            End If
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub ddlBancoDestino_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBancoDestino.SelectedIndexChanged
        CargarDatosBancoDestino()
    End Sub
    Private Sub CargarDatosBancoDestino()
        CargarClaseCuentaDestino(True, String.Empty)
        CargarCuentasDestino(String.Empty, String.Empty, ddlPortafolio.SelectedValue, True)
        CargarContactosDivisas()
    End Sub
    Private Sub ddlClaseCuentaDestino_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlClaseCuentaDestino.SelectedIndexChanged
        CargarCuentasDestino(String.Empty, String.Empty, ddlPortafolio.SelectedValue, True)
    End Sub
    Private Sub CargarClaseCuentaDestino(Optional ByVal enabled As Boolean = True, Optional ByVal sCodigoTercero As String = "")
        Dim dtBancos As DataTable
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone(String.Empty, Me.ddlPortafolio.SelectedValue, Me.hidCodigoMonedaDestino.Value)
            dtBancos = dsBanco.Tables(0)
            ddlClaseCuentaDestino.Items.Clear()
            dtBancos.DefaultView.RowFilter = "CodigoTercero='" + ddlBancoDestino.SelectedValue + "'"
            dtBancos.DefaultView.RowStateFilter = DataViewRowState.CurrentRows
            Dim r As DataRowView
            For Each r In dtBancos.DefaultView
                If ddlClaseCuentaDestino.Items.FindByValue(CType(r("CodigoClaseCuenta"), String)) Is Nothing Then
                    ddlClaseCuentaDestino.Items.Add(New ListItem(CType(r("NombreClaseCuenta"), String), CType(r("CodigoClaseCuenta"), String)))
                End If
            Next
            If ddlClaseCuentaDestino.Items.Count = 1 Then
                UIUtility.InsertarElementoSeleccion(ddlClaseCuentaDestino)
                ddlClaseCuentaDestino.SelectedIndex = 1
            Else
                UIUtility.InsertarElementoSeleccion(ddlClaseCuentaDestino)
            End If
        Else
            ddlClaseCuentaDestino.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClaseCuentaDestino)
        End If
        ddlClaseCuentaDestino.Enabled = enabled
    End Sub
    Private Sub CargarCuentasDestino(ByVal codTercero As String, ByVal CodigoClaseCuenta As String, ByVal CodigoPortafolio As String, Optional ByVal enabled As Boolean = True)
        Dim dtBancos As DataTable
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone(String.Empty, CodigoPortafolio, Me.hidCodigoMonedaDestino.Value)
            dtBancos = dsBanco.Tables(0)
            If ddlClaseCuentaDestino.SelectedValue = "" Then
                dtBancos.DefaultView.RowFilter = "CodigoTercero='" + ddlBancoDestino.SelectedValue + "'"
            Else
                dtBancos.DefaultView.RowFilter = "CodigoClaseCuenta='" + ddlClaseCuentaDestino.SelectedValue + "' AND CodigoTercero='" + ddlBancoDestino.SelectedValue + "'"
            End If
            dtBancos.DefaultView.RowStateFilter = DataViewRowState.CurrentRows
            ddlCuentaDestino.Items.Clear()
            ddlCuentaDestino.DataValueField = "CodigoCuenta"
            ddlCuentaDestino.DataTextField = "NumeroCuenta"
            ddlCuentaDestino.DataSource = dtBancos.DefaultView
            ddlCuentaDestino.SelectedIndex = -1
            ddlCuentaDestino.DataBind()
            If ddlCuentaDestino.Items.Count = 1 Then
                UIUtility.InsertarElementoSeleccion(ddlCuentaDestino)
                ddlCuentaDestino.SelectedIndex = 1
            Else
                UIUtility.InsertarElementoSeleccion(ddlCuentaDestino)
            End If
        Else
            ddlCuentaDestino.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlCuentaDestino)
        End If
        ddlCuentaDestino.Enabled = enabled
    End Sub
    Private Sub BindingModeloCarta(ByVal codigoOperacion As String, CodigoOrden As String) 'OT12012
        Dim operacion As New OperacionBM
        Try
            ddlModeloCarta.DataTextField = "Descripcion"
            ddlModeloCarta.DataValueField = "CodigoModelo"
            ddlModeloCarta.DataSource = operacion.ListaModeloCartaOperacion(codigoOperacion, CodigoOrden)
            ddlModeloCarta.DataBind()
            ddlModeloCarta.Items.Insert(0, New ListItem("SIN CARTA", "SC01"))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub ddlTipoPago_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoPago.SelectedIndexChanged
        Dim cadena As String
        Dim indBCR As Integer
        Dim indInterna As Integer
        Dim indExterna As Integer
        Dim indBusqueda As Integer
        Dim cadBusqueda As String
        Dim i As Integer
        cadena = Me.ddlTipoPago.SelectedItem.Text
        indBCR = cadena.IndexOf("BCR")
        indInterna = cadena.IndexOf("Intern")
        indExterna = cadena.IndexOf("Exter")
        If (indBCR <> -1) Then
            For i = 0 To Me.ddlModeloCarta.Items.Count - 1
                cadBusqueda = ddlModeloCarta.Items(i).Text()
                indBusqueda = cadBusqueda.IndexOf("BCR")
                If indBusqueda > 0 Then
                    ddlModeloCarta.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
        If (indInterna <> -1) Then
            For i = 0 To Me.ddlModeloCarta.Items.Count - 1
                cadBusqueda = ddlModeloCarta.Items(i).Text()
                indBusqueda = cadBusqueda.IndexOf("Intern")
                If indBusqueda > 0 Then
                    ddlModeloCarta.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
        If (indExterna <> -1) Then
            For i = 0 To Me.ddlModeloCarta.Items.Count - 1
                cadBusqueda = ddlModeloCarta.Items(i).Text()
                indBusqueda = cadBusqueda.IndexOf("Exter")
                If indBusqueda > 0 Then
                    ddlModeloCarta.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
    End Sub
    Private Sub ddlModeloCarta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModeloCarta.SelectedIndexChanged
        Me.lblContactoDivisa.Visible = True
        Me.ddlContactoDivisa.Visible = True
        If ddlModeloCarta.SelectedValue = "CO02" Then
            If ValidaTerceroOI() Then
                pnRenovacion.Visible = True
                CargarRenovacion()
            Else
                AlertaJS("Para realizar una renovacion debe seleccionar solo una orden.")
                ddlModeloCarta.SelectedIndex = 0
            End If
        Else
            pnRenovacion.Visible = False
        End If
    End Sub

    Sub ValidaVistaRelacion()
        'El registro relación aplica para los portafolios de Operaciones de Reporte
        'If (ddlPortafolio.SelectedValue.ToString.Equals("6") Or ddlPortafolio.SelectedValue.ToString.Equals("8")) Then
        '    lkbRelacion.Enabled = True
        'Else
        '    lkbRelacion.Enabled = False
        'End If
    End Sub

End Class
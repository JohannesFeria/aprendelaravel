Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Imports System.IO

Partial Class Modulos_Tesoreria_OperacionesCaja_frmPreLiquidacionDivisas
    Inherits BasePage
#Region "CargarDatos"
    Dim oPortafolioBM As New PortafolioBM
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            'Dim dsMercado As DataSet = oMercado.Listar(DatosRequest)
            Dim dsMercado As DataSet = oMercado.ListarActivos(DatosRequest, ParametrosSIT.ESTADO_ACTIVO)
            HelpCombo.LlenarComboBox(ddlMercado, dsMercado.Tables(0), "CodigoMercado", "Descripcion", False)
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
        If (ddlMercado.Items.Count > 1) Then
            ddlMercado.SelectedIndex = 1
        End If
    End Sub
    Private Sub CargarPortafolio()
        ddlPortafolio.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
        ddlPortafolio.Enabled = True
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

    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataSet = oMoneda.GetMonedaMercadoPortafolio(ddlMercado.SelectedValue, ddlPortafolio.SelectedValue)
            HelpCombo.LlenarComboBox(ddlMoneda, dsMoneda.Tables(0), "CodigoMoneda", "Descripcion", False)
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        End If
        ddlMoneda.Enabled = enabled
    End Sub

    Private Sub CargarClaseCuenta(Optional ByVal enabled As Boolean = True, Optional ByVal sCodigoTercero As String = "")
        Dim dtBancos As DataTable
        If enabled Then

            Dim oBanco As New TercerosBM
            Dim codigMoneda As String

            If (Me.pnlDestinoDivisas.Visible) Then
                codigMoneda = Me.hidCodigoMonedaOrigen.Value
            Else
                codigMoneda = ddlMoneda.SelectedValue
            End If

            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone(Me.ddlMercado.SelectedValue, Me.ddlPortafolio.SelectedValue, codigMoneda)

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

            If (Me.pnlDestinoDivisas.Visible) Then
                codigMoneda = Me.hidCodigoMonedaOrigen.Value
            Else
                codigMoneda = ddlMoneda.SelectedValue
            End If

            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone(Me.ddlMercado.SelectedValue, CodigoPortafolio, codigMoneda)
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
            ddlNroCuenta.SelectedIndex = -1

            ddlNroCuenta.DataSource = dtBancos.DefaultView
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
        If enabled Then
            Dim oModalidadPago As New ModalidadPagoBM
            Dim dsModalidadPago As DataSet = oModalidadPago.Listar(DatosRequest, ParametrosSIT.ESTADO_ACTIVO)
            ddlTipoPago.Items.Clear()
            ddlTipoPago.DataSource = dsModalidadPago
            ddlTipoPago.DataValueField = "codigoModalidadPago"
            ddlTipoPago.DataTextField = "Descripcion"
            ddlTipoPago.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlTipoPago)
        Else
            ddlTipoPago.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlTipoPago)
        End If
        ddlTipoPago.Enabled = enabled
    End Sub

#End Region

#Region "Eventos de la Página"

    Private Sub CargarGrilla()
        Dim dsCuentasPorCobrar As New CuentasPorCobrarPagarBE
        Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.NewCuentasPorCobrarPagarRow
        cuentaPorCobrar.CodigoMercado = ddlMercado.SelectedValue
        cuentaPorCobrar.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        cuentaPorCobrar.CodigoMoneda = ddlMoneda.SelectedValue
        cuentaPorCobrar.CodigoTercero = ddlIntermediario.SelectedValue
        cuentaPorCobrar.CodigoOperacion = IIf(ddlOperacion.SelectedValue.ToUpper.Equals("--SELECCIONE--"), "", ddlOperacion.SelectedValue) 'RGF 20090707
        cuentaPorCobrar.FechaOperacion = UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text)
        cuentaPorCobrar.FechaIngreso = UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text)
        cuentaPorCobrar.Egreso = ""
        Dim fechaIni As Decimal = IIf(txtFechaInicio.Text = "", 0, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text))
        Dim fechaFin As Decimal = IIf(txtFechaFin.Text = "", 0, UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text))
        dsCuentasPorCobrar.CuentasPorCobrarPagar.AddCuentasPorCobrarPagarRow(cuentaPorCobrar)
        Dim dsCuentas As DataSet = New PreLiquidacionDivisasBM().SeleccionarPorFiltro(dsCuentasPorCobrar, fechaIni, fechaFin, DatosRequest)
        Session("dtprueba") = dsCuentas.Tables(0)
        Dim view As DataView = dsCuentas.Tables(0).DefaultView
        view.RowFilter = "Estado <> 'E'"
        dgLista.DataSource = view
        dgLista.DataBind()
        dgLista.SelectedIndex = -1
        Session("GrillaCuentasCobrar") = Nothing
        Dim dtTablaCobrar As New DataTable
        dtTablaCobrar = dsCuentas.Copy.Tables(0)
        Session("GrillaCuentasCobrar") = dtTablaCobrar
        Me.ddlContacto.SelectedIndex = -1
        Me.ddlModeloCarta.SelectedIndex = -1
        txtDescripcion.Text = ""
        txtImporte.Text = ""
        Me.ddlNroCuenta.SelectedIndex = -1
        Me.ddlTipoPago.SelectedIndex = -1
        Me.ddlBanco.SelectedIndex = -1
        Me.ddlClase.SelectedIndex = -1
        Me.pnlDestinoDivisas.Visible = True
        ddlContactoIntermediario.Items.Clear()
        EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dsCuentas.Tables(0).Rows.Count) + "')")
        ddlBancoDestino.SelectedIndex = -1
        ddlClaseCuentaDestino.SelectedIndex = -1
        ddlCuentaDestino.SelectedIndex = -1
        lblMonedaDestino.Text = ""
        txtImporteDestino.Text = ""
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            ViewState("tieneDato") = False
            CargarMercado()
            CargarPortafolio()
            txtPago.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
            CargarIntermediario(ddlMercado.SelectedValue, True)
            CargarMoneda()
            CargarBanco("", "", "")
            CargarBancoDestino("", ddlPortafolio.SelectedValue, "")
            CargarDetallePortafolio("", "", ddlPortafolio.SelectedValue)
            CargarClaseCuenta()
            CargarTipoPago()
            EstablecerFecha()
            btnAceptar.Visible = False
            ddlContactoDivisa.Visible = False
            lblContactoEgreso.Visible = False

            'Creamos la session para que al volver a cargar la pagina no la vuelva a crear
            'Developer by Carlos Hernández
            'Session("dtprueba") = Nothing
        End If
        btnBuscar.Attributes.Add("onclick", "return ValidarBuscar();")
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        'Agregado por Carlos Hernández, --> Elimina la sessión
        Session.Remove("dtprueba")
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            If UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text) < UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue) Then
                AlertaJS(ObtenerMensaje("ALERT155"))
                Exit Sub
            End If
            If UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text) > UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text) Then
                AlertaJS(ObtenerMensaje("ALERT48"))
                Exit Sub
            End If
            ViewState("tieneDato") = False
            'Agregado por Carlos Hernández, --> Elimina la sessión
            Session.Remove("dtprueba")
            CargarGrilla()
            btnAceptar.Visible = False
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try

    End Sub

    Private Sub ddlClase_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlClase.SelectedIndexChanged
        CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, ddlPortafolio.SelectedValue)
    End Sub

    Private Sub ddlBanco_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBanco.SelectedIndexChanged
        CargarDatosBanco()
    End Sub

    Private Sub CargarDatosBanco()
        CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, ddlPortafolio.SelectedValue)
        Call CargarClaseCuenta(True, ddlBanco.SelectedValue)
        CargarContactos()
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
                Response.Redirect("../../Inversiones/InstrumentosNegociados/frmOpcionesDerivadas-ForwardDivisas.aspx?PTNeg=CP" & "&CodigoOrden=" & sCodigoOrden & "&PTFondo=" & sCodigoPortafolioSBS & "&Mnemonico=" & sCodigoMnemonico & "&PTOperacion=" & sCodigoOperacion)
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

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If Not hiddenCodigoOrden.Value Is Nothing Then
            If Not ValidarLiquidacion_Matriz() Then Exit Sub 'Modificado DB - REQ 48 - 20090331
            Dim objPreLiq As New PreLiquidacionDivisasBM
            If objPreLiq.SeleccionarPorCodigoOrden(hiddenCodigoOrden.Value, DatosRequest).DatosOrdenInversion.Rows.Count > 0 Then
                ModificarDOI(CrearObjetoDOI(hiddenCodigoOrden.Value))
            Else
                InsertarDOI(CrearObjetoDOI(hiddenCodigoOrden.Value))
            End If
        Else
            AlertaJS("Debe seleccionar un registro")
        End If
    End Sub

    Private Function ValidarLiquidacion_Matriz() As Boolean
        Dim strLiqBanco As String = Me.hdLiqBanco.Value
        Dim strLiqOperacion As String = Me.hdLiqOperacion.Value
        Dim strLiqCategoria As String = Me.hdLiqCategoria.Value
        Dim strOtroBancoCuenta As String = ObtieneCC_SC()
        Dim blnResultado As Boolean = False

        If strLiqCategoria = "BO" Then                          'BONOS
            If strLiqOperacion = "2" Then                           'VENTAS
                If ddlBanco.SelectedValue = strLiqBanco Then            'MB
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, True, False, "Bonos - Venta - C/C - MB")
                Else
                    If strOtroBancoCuenta = "C/C" Then                      'C/C
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, True, "Bonos - Venta - C/C - OtroBanco")
                    ElseIf strOtroBancoCuenta = "S/C" Then                  'S/C
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, True, "Bonos - Venta - S/C - OtroBanco")
                    Else
                        blnResultado = True
                    End If
                End If
            ElseIf strLiqOperacion = "1" Then                       'COMPRAS
                If ddlBanco.SelectedValue = strLiqBanco Then            'MB
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, True, False, "Bonos - Compras - C/C - MB")
                Else
                    If strOtroBancoCuenta = "S/C" Then                  'S/C
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, True, True, "Bonos - Compras - S/C - OtroBanco")
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
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, True, False, "Acciones - Compras - C/C - MB")
                Else
                    blnResultado = True
                End If
            Else
                blnResultado = True
            End If
        Else
            If ddlBanco.SelectedValue = strLiqBanco Then            'MB
                If strLiqOperacion = "4" Then       'DPZ - Cancelacion
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, True, False, "DPZ - Cancelación - C/C - MB")
                ElseIf strLiqOperacion = "3" Then   'DPZ - Constitucion
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, True, False, "DPZ - Constitución - C/C - MB")
                ElseIf strLiqOperacion = "35" Then  'Cobro Vcmtos Interes
                    blnResultado = ValidaCampos_Matriz(True, True, True, False, False, False, "Cobro Vcmtos Interes - C/C - MB")
                ElseIf strLiqOperacion = "38" Then  'Cobro Amortiz Capital
                    blnResultado = ValidaCampos_Matriz(True, True, True, False, False, False, "Cobro Amortiz Capital - C/C - MB")
                ElseIf strLiqOperacion = "66" Then  'Operación de Cambios - Venta
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, True, False, "Operación de Cambios - Venta - C/C - MB")
                ElseIf strLiqOperacion = "65" Then  'Operación de Cambios - Compra
                    blnResultado = ValidaCampos_Matriz(True, True, True, True, True, False, "Operación de Cambios - Compra - C/C - MB")
                Else
                    blnResultado = True
                End If
            Else
                If strOtroBancoCuenta = "C/C" Then                      'C/C
                    If strLiqOperacion = "4" Then       'DPZ - Cancelacion
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, True, "DPZ - Cancelacion - C/C - OtroBanco")
                    ElseIf strLiqOperacion = "66" Then  'Operación de Cambios - Venta
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, True, "Operación de Cambios - Venta - C/C - OtroBanco")
                    ElseIf strLiqOperacion = "65" Then  'Operación de Cambios - Compra
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, True, "Operación de Cambios - Compra - C/C - OtroBanco")
                    Else
                        blnResultado = True
                    End If
                ElseIf strOtroBancoCuenta = "S/C" Then                  'S/C
                    If strLiqOperacion = "4" Then       'DPZ - Cancelacion
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, False, True, "DPZ - Cancelación - S/C - OtroBanco")
                    ElseIf strLiqOperacion = "3" Then   'DPZ - Constitucion
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, True, True, "DPZ - Constitución - S/C - OtroBanco")
                    ElseIf strLiqOperacion = "66" Then  'Operación de Cambios - Venta
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, True, True, "Operación de Cambios - Venta - S/C - OtroBanco")
                    ElseIf strLiqOperacion = "65" Then  'Operación de Cambios - Compra
                        blnResultado = ValidaCampos_Matriz(True, True, True, True, True, True, "Operación de Cambios - Compra - S/C - OtroBanco")
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
        If ddlContacto.Visible Then
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

    Private Sub InsertarDOI(ByVal objDatos As DatosOrdenInversionBE)
        Dim objPreLiq As New PreLiquidacionDivisasBM
        If objPreLiq.Insertar(objDatos, DatosRequest) Then
            AlertaJS("Se ingresaron los datos exitosamente")
        End If
    End Sub

    Private Sub ModificarDOI(ByVal objDatos As DatosOrdenInversionBE)
        Dim objPreLiq As New PreLiquidacionDivisasBM
        If objPreLiq.Modificar(objDatos, DatosRequest) Then
            AlertaJS("Se modificaron los datos exitosamente")
        End If
    End Sub

    Private Function CrearObjetoDOI(ByVal strCodigoOrden As String) As DatosOrdenInversionBE
        Dim obj As New DatosOrdenInversionBE
        Dim objR As DatosOrdenInversionBE.DatosOrdenInversionRow
        objR = CType(obj.DatosOrdenInversion.NewRow, DatosOrdenInversionBE.DatosOrdenInversionRow)
        objR.CodigoOrden = strCodigoOrden
        objR.CodigoClaseCuenta = ddlClase.SelectedValue
        objR.CodigoClaseCuentaDestino = ddlClaseCuentaDestino.SelectedValue
        objR.NumeroCuenta = ddlNroCuenta.Items(ddlNroCuenta.SelectedIndex).Text
        objR.NumeroCuentaDestino = ddlCuentaDestino.Items(ddlCuentaDestino.SelectedIndex).Text
        objR.CodigoModelo = ddlModeloCarta.SelectedValue
        objR.BancoOrigen = ddlBanco.SelectedValue
        objR.BancoDestino = ddlBancoDestino.SelectedValue
        If ddlContacto.Visible Then
            objR.CodigoContacto = Me.ddlContacto.SelectedValue
        Else
            objR.CodigoContacto = Me.ddlContactoDivisa.SelectedValue
        End If
        objR.CodigoContactoIntermediario = ddlContactoIntermediario.SelectedValue
        objR.CodigoModalidadPago = ddlTipoPago.SelectedValue
        obj.DatosOrdenInversion.Rows.Add(objR)
        Return obj
    End Function
#End Region

#Region "Metodos de control"

    Private Function ValidarMoneda() As Boolean
        Dim oCuentaEconomica As New CuentaEconomicaBM
        Dim dr As DataRow = oCuentaEconomica.SeleccionarPorFiltro("", "", "", "", "", DatosRequest).Tables(1).Select("NumeroCuenta='" & ddlNroCuenta.SelectedValue.Split(",")(1) & "'")(0)
        Dim codMoneda As String = dr("CodigoMoneda")
        For Each row As DataGridItem In dgLista.Rows
            If CType(row.Cells(2).Controls(1), CheckBox).Checked Then
                Dim cuentaMoneda As String = row.Cells(15).Text
                If codMoneda <> cuentaMoneda Then
                    Return False
                End If
            End If
        Next
        Return True
    End Function


    Private Sub CargarContactos()

        Dim objContacto As New ContactoBM
        Dim oBanco As New TercerosBM
        Dim dsContactos As DataSet

        If ddlContacto.Visible Then
            dsContactos = objContacto.ListarPorCodigoEntidad(ddlBanco.SelectedValue)
            ddlContacto.DataSource = Nothing
            ddlContacto.DataBind()

            ddlContacto.DataTextField = "DescripcionContacto"
            ddlContacto.DataValueField = "CodigoContacto"
            ddlContacto.DataSource = dsContactos
            ddlContacto.DataBind()
            ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        Else
            dsContactos = objContacto.ListarPorCodigoEntidad(ddlBancoDestino.SelectedValue)
            ddlContactoDivisa.DataSource = Nothing
            ddlContactoDivisa.DataBind()

            ddlContactoDivisa.DataTextField = "DescripcionContacto"
            ddlContactoDivisa.DataValueField = "CodigoContacto"
            ddlContactoDivisa.DataSource = dsContactos
            ddlContactoDivisa.DataBind()
            ddlContactoDivisa.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        End If

    End Sub

#End Region

    Private Sub ddlMoneda_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        lblMoneda.Text = ""
        If ddlMoneda.SelectedIndex > 0 Then
            lblMoneda.Text = ddlMoneda.SelectedItem.Text
        End If
        dgLista.DataSource = Nothing
        dgLista.DataBind()
        Call CargarBanco(ddlMercado.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
        Call CargarDetallePortafolio("", "", "", False)
        Call CargarClaseCuenta(False, "")
        Call CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, ddlPortafolio.SelectedValue, True)
    End Sub

    Private Sub EstablecerFecha()
        txtFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        txtFechaFin.Text = txtFechaInicio.Text
        txtPago.Text = txtFechaInicio.Text
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        dgLista.DataSource = Nothing
        dgLista.DataBind()
        Me.CargarMoneda(True)
        Call CargarBanco(ddlMercado.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
        Call CargarBancoDestino(String.Empty, ddlPortafolio.SelectedValue, String.Empty)
        Call CargarDetallePortafolio("", "", "", False)
        Call CargarClaseCuenta(False, "")
        Call CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, ddlPortafolio.SelectedValue, True)
        EstablecerFecha()
    End Sub
    Private Sub ibImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibImprimir.Click
        Try
            If ViewState("tieneDato") = False Then
                AlertaJS("Debe de seleccionar un registro para poder imprimir")
                Exit Sub
            End If
            Dim dsImprimir As DataSet = New PreLiquidacionDivisasBM().ListarPreLiquidacionDivisasImprimir(hiddenCodigoOrden.Value, ddlModeloCarta.SelectedValue, DatosRequest)
            Dim dr As DataRow
            Dim windowsCartas As New System.Text.StringBuilder
            Dim ruta As String = New ParametrosGeneralesBM().ListarRutaGeneracionCartas(DatosRequest) + "\"

            HelpCarta.GenerarCartas(dsImprimir, DatosRequest)
            For Each dr In dsImprimir.Tables(0).Rows
                windowsCartas.Append(ruta & Convert.ToString(dr("NumeroCarta")) & ".doc" & "&")
            Next
            If windowsCartas.ToString() <> "" Then
                Session("RutaCarta") = HelpCarta.CrearMultiCarta(windowsCartas.ToString())
                EjecutarJS("window.open('frmVisorCarta.aspx')")
                'Dim strNC = Convert.ToString(Session("RutaCarta"))

                ''Dim strFl As New System.IO.FileStream(strNC, FileMode.Open)
                ''Dim bytes() As Byte = New Byte(strFl.Length) {}

                ''strFl.Read(bytes, 0, strFl.Length)
                ''strFl.Flush()
                ''strFl.Close()
                'Response.Clear()
                ''Response.Buffer = True

                'If strNC.IndexOf(".pdf") > -1 Then

                '    Response.ContentType = "application/pdf"
                '    Response.AddHeader("Content-Disposition", "inline;filename=" + "Carta.pdf")

                'ElseIf strNC.IndexOf(".doc") > -1 Then

                '    Response.ContentType = "application/msword"
                '    Response.AddHeader("Content-Disposition", "inline;filename=Carta.doc")

                'End If

                'Response.WriteFile(strNC)
                ''Response.Flush()
                'Response.End()
            Else
                AlertaJS("No se encontró la carta")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al enviar los datos a imprimir")
        End Try
    End Sub

    Private Sub ddlMercado_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        Dim sMercado As String = ddlMercado.SelectedValue.Trim
        Me.CargarMoneda(True)
        Call CargarBanco(ddlMercado.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
        Call CargarBancoDestino(String.Empty, ddlPortafolio.SelectedValue, String.Empty)
        Call CargarDetallePortafolio("", "", "", False)
        Call CargarClaseCuenta(False, "")
        Call CargarIntermediario(sMercado, True)
    End Sub

    Private Sub ddlBancoDestino_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBancoDestino.SelectedIndexChanged
        CargarDatosBancoDestino()
    End Sub

    Private Sub CargarDatosBancoDestino()
        CargarClaseCuentaDestino(True, String.Empty)
        CargarCuentasDestino(String.Empty, String.Empty, ddlPortafolio.SelectedValue, True)
        CargarContactos()
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
    Private Sub BindingModeloCarta(ByVal codigoOperacion As String)
        Dim operacion As New OperacionBM
        Try
            ddlModeloCarta.DataTextField = "Descripcion"
            ddlModeloCarta.DataValueField = "CodigoModelo"
            ddlModeloCarta.DataSource = operacion.ListaModeloCartaOperacion(codigoOperacion, "")
            ddlModeloCarta.DataBind()
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

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        If e.CommandName = "Select" Then
            Dim selectedRow As DataGridItem = Nothing
            Dim codigoOperacion As String = String.Empty
            Dim codigoOrden As String = String.Empty
            Dim totalDestino As Decimal
            Dim ordenInverscionBM As New OrdenInversionWorkFlowBM
            Dim preLiquidacionBM As New PreLiquidacionDivisasBM
            Dim dt As DatosOrdenInversionBE.DatosOrdenInversionDataTable
            Dim dt2 As DataTable
            Dim importeTotal As Decimal = 0

            Dim referencia As String = ""
            Dim dtLista As DataTable = DirectCast(Session("dtprueba"), System.Data.DataTable)

            referencia = dtLista.Rows(ToNullInt32(e.CommandArgument))("Referencia").ToString().Trim()

            txtImporte.Text = ""
            txtDescripcion.Text = ""
            txtFechaVcto.Text = ""
            lblMonedaDestino.Text = ""
            txtImporteDestino.Text = ""

            ddlContacto.DataSource = Nothing
            ddlContacto.DataBind()
            ddlContactoDivisa.DataSource = Nothing
            ddlContactoDivisa.DataBind()

            Try
                Dim fila As Integer = ToNullInt32(e.CommandArgument)

                'For Each row As DataGridItem In dgLista.Rows
                '    If row.ItemIndex = e.Item.ItemIndex Then
                With dgLista.Rows(fila)

                    codigoOrden = .Cells(20).Text
                    Me.hiddenCodigoOrden.Value = codigoOrden
                    Me.hdLiqCategoria.Value = .Cells(19).Text
                    Me.hdLiqOperacion.Value = .Cells(21).Text
                    Me.hdLiqBanco.Value = .Cells(25).Text
                    dt = preLiquidacionBM.SeleccionarPorCodigoOrden(codigoOrden, DatosRequest).DatosOrdenInversion
                    dt2 = ordenInverscionBM.GetOrdenInversionDivisas(ddlPortafolio.SelectedValue, codigoOrden)

                    If hdLiqOperacion.Value.Equals("65") Then
                        ddlContacto.Visible = False
                        lblContacto.Visible = False
                        ddlContactoDivisa.Visible = True
                        lblContactoEgreso.Visible = True
                    Else
                        ddlContacto.Visible = True
                        lblContacto.Visible = True
                        ddlContactoDivisa.Visible = False
                        lblContactoEgreso.Visible = False
                    End If

                    txtImporte.Text = .Cells(7).Text
                    txtImporteDestino.Text = .Cells(7).Text
                    'txtDescripcion.Text = .Cells(6).Text.Replace("&nbsp;", "")
                    txtDescripcion.Text = referencia
                    txtFechaVcto.Text = .Cells(4).Text.Replace("&nbsp;", "")
                    Dim objContacto As New ContactoBM
                    Me.ddlContactoIntermediario.DataTextField = "DescripcionContacto"
                    Me.ddlContactoIntermediario.DataValueField = "CodigoContacto"
                    Me.ddlContactoIntermediario.DataSource = objContacto.ListarContactoPorTercerosTesoreria(.Cells(24).Text)
                    Me.ddlContactoIntermediario.DataBind()
                    Me.ddlContactoIntermediario.Items.Insert(0, New ListItem("--SELECCIONE--", ""))

                    BindingModeloCarta(hdLiqOperacion.Value) 'OT12012

                    If Not dt Is Nothing Then
                        If dt.Rows.Count > 0 Then
                            If CType(dt.Rows(0), DatosOrdenInversionBE.DatosOrdenInversionRow).BancoOrigen.Equals("BBH") Then
                                ddlMercado.SelectedValue = 2
                            Else
                                ddlMercado.SelectedValue = 1
                            End If
                        End If
                    End If

                    If Not (dt2 Is Nothing) Then
                        If dt2.Rows.Count > 0 Then
                            If Not dt2.Rows(0)("MontoDestino") Is DBNull.Value Then
                                totalDestino = totalDestino + Convert.ToDecimal(dt2.Rows(0)("MontoDestino"))
                            End If
                            Me.lblMonedaDestino.Text = Convert.ToString(dt2.Rows(0)("Moneda"))
                            Me.hidCodigoMonedaOrigen.Value = Convert.ToString(dt2.Rows(0)("CodigoMonedaOrigen"))
                            Me.hidCodigoMonedaDestino.Value = Convert.ToString(dt2.Rows(0)("CodigoMonedaDestino"))
                            CargarBancoDestino(String.Empty, ddlPortafolio.SelectedValue, Me.hidCodigoMonedaDestino.Value)
                            Me.CargarBanco(ddlMercado.SelectedValue, ddlPortafolio.SelectedValue, hidCodigoMonedaOrigen.Value, True)
                            Me.CargarClaseCuenta(True, ddlBanco.SelectedValue)
                            Me.CargarDetallePortafolio(ddlBanco.SelectedValue, String.Empty, ddlPortafolio.SelectedValue, True)
                        End If
                    End If
                    If Not (dt Is Nothing) Then
                        If dt.Rows.Count > 0 Then
                            Dim drBE As DatosOrdenInversionBE.DatosOrdenInversionRow = dt.Rows(0)
                            ddlBanco.SelectedValue = drBE.BancoOrigen
                            CargarDatosBanco()
                            ddlClase.SelectedValue = drBE.CodigoClaseCuenta
                            CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, ddlPortafolio.SelectedValue)
                            ddlNroCuenta.SelectedValue = ddlPortafolio.SelectedValue & "," & drBE.NumeroCuenta

                            ddlBancoDestino.SelectedValue = drBE.BancoDestino
                            CargarDatosBancoDestino()

                            If ddlContacto.Visible Then
                                ddlContacto.SelectedValue = drBE.CodigoContacto
                            Else
                                ddlContactoDivisa.SelectedValue = drBE.CodigoContacto
                            End If

                            ddlClaseCuentaDestino.SelectedValue = drBE.CodigoClaseCuentaDestino
                            CargarCuentasDestino(String.Empty, String.Empty, ddlPortafolio.SelectedValue, True)
                            ddlCuentaDestino.SelectedValue = ddlPortafolio.SelectedValue & "," & drBE.NumeroCuentaDestino
                            ddlContactoIntermediario.SelectedValue = drBE.CodigoContactoIntermediario
                            ddlTipoPago.SelectedValue = drBE.CodigoModalidadPago

                            ddlModeloCarta.SelectedValue = drBE.CodigoModelo
                        End If
                    End If
                End With
                '    End If
                'Next
                Me.txtImporteDestino.Text = totalDestino.ToString()
                ViewState("tieneDato") = True
                btnAceptar.Visible = True
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        End If
    End Sub

End Class

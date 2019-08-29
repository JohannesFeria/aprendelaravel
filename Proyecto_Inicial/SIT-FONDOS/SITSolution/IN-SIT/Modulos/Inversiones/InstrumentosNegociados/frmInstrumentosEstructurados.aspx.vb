Imports SIT.BusinessLayer
Imports SIT.BusinessEntities
Imports System.Data
Imports ParametrosSIT
Imports UIUtility
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmInstrumentosEstructurados
    Inherits BasePage
#Region "Variables"
    Dim objutil As New UtilDM
    'Dim oImpComOP As New ImpuestosComisionesOrdenPreOrdenBM
    Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim oPortafolioBM As New PortafolioBM
    Dim oValoresBM As New ValoresBM
    Dim objcomisiones As New ImpuestosComisionesBM
    Dim strTipoRenta As String
    Dim objImpuestosComisionesOPBM As New ImpuestosComisionesOrdenPreOrdenBM
    Dim objferiadoBM As New FeriadoBM
#End Region
#Region "Metodos Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Me.hdSaldo.Value = 0
        Me.btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
        If Not Page.IsPostBack Then
            Me.btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
            Me.btnProcesar.Attributes.Add("onclick", "javascript:return Validar();")
            btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
            LimpiarSesiones()
            If Not Request.QueryString("PTNeg") Is Nothing Then
                Me.hdPagina.Value = Request.QueryString("PTNeg")
            End If
            If (Me.hdPagina.Value = "TI") Then
                UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
            Else
                UIUtility.CargarOperacionOI(ddlOperacion, "Estructurados")
            End If
            UIUtility.CargarGrupoIntermediarioOI(ddlGrupoInt)
            CargarPaginaInicio()
            Me.hdPagina.Value = ""
            HelpCombo.PortafolioCodigoListar(ddlFondo, PORTAFOLIO_MULTIFONDOS)
            If Not Request.QueryString("PTNeg") Is Nothing Then
                Me.hdPagina.Value = Request.QueryString("PTNeg")
                If Me.hdPagina.Value = "TI" Then  'Viene de la Pagina Traspaso de Instrumentos
                    Me.txtMnemonico.Text = Request.QueryString("PTCMnemo")
                    Me.ddlFondo.SelectedValue = Request.QueryString("PTFondoO")
                    Me.ddlFondoDestino.SelectedValue = Request.QueryString("PTFondoD")
                    Me.txtISIN.Text = Request.QueryString("PTISIN")
                    Me.txtSBS.Text = Request.QueryString("PTSBS")
                    Me.lblMoneda.Text = Request.QueryString("PTMon")
                    Me.ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                    Me.hdCustodio.Value = Request.QueryString("PTCustodio")
                    Me.hdSaldo.Value = Request.QueryString("PTSaldo")
                    CargarCaracteristicasValor()
                    ControlarCamposTI()
                Else
                    Me.txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                    Me.ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                    If (Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Then 'Viene de la Pagina Confirmacion , Ejecucion  o Extorno de Ordenes de Inversion
                        CargarDatosOrdenInversion()
                        CargarCaracteristicasValor()
                        tbNPoliza.Text = Right(txtCodigoOrden.Value, 4)
                        tbNPoliza.ReadOnly = True
                        Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                        If (Me.hdPagina.Value <> "XO") Then
                            Me.lNPoliza.Visible = True
                            Me.tbNPoliza.Visible = True
                        End If
                        ControlarCamposEO_CO_XO()
                        CargarPaginaModificarEO_CO_XO(Me.hdPagina.Value)
                    Else
                        If (Me.hdPagina.Value = "OE") Then 'Viene de la Pagina Ordenes Excedidas
                            ControlarCamposOE()
                        Else
                            If (Me.hdPagina.Value = "DA") Then 'Viene de la Pagina Negociacion Dias Anteriores
                                ViewState("ORDEN") = "OI-DA"
                                Me.tbFechaOperacion.Text = Request.QueryString("Fecha")
                                Me.tbFechaOperacion.ReadOnly = True
                                Me.imgFechaOperacion.Attributes.Add("class", "input-append")
                                ControlarCamposDA()
                            Else
                                If (Me.hdPagina.Value = "CP") Then 'Viene de la Pagina Liquidaciones Cuentas Por Pagar
                                    Call ConfiguraModoConsulta()
                                    ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                    txtMnemonico.Text = Request.QueryString("Mnemonico")
                                    txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                    ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                    Call CargarDatosOrdenInversion()
                                    Call CargarCaracteristicasValor()

                                    Call HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, True, False, False)
                                Else
                                    If (Me.hdPagina.Value = "CONSULTA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                        ConfiguraModoConsulta()
                                        ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                        txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                        Me.tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                        CargarDatosOrdenInversion()
                                        CargarCaracteristicasValor()
                                        HabilitaBotones(False, False, False, False, False, False, False, False, False, False, True, False, False, False)
                                        HabilitaDeshabilitaCabecera(False)
                                    Else
                                        If (Me.hdPagina.Value = "MODIFICA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                            ConfiguraModoConsulta()
                                            ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                            txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                            Me.tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                            CargarDatosOrdenInversion()
                                            CargarCaracteristicasValor()
                                            HabilitaBotones(False, False, False, False, False, False, True, False, True, False, True, False, False, False)
                                            HabilitaDeshabilitaCabecera(False)
                                            HabilitaDeshabilitaDatosOperacionComision(True)
                                            Session("EstadoPantalla") = "Modificar"
                                            lblAccion.Text = "Modificar"
                                            hdMensaje.Value = "la Modificación"

                                            EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
                                            HelpCombo.CargarMotivosCambio(Me)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                Me.btnSalir.Attributes.Remove("onClick")
                Me.btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                Me.btnAceptar.Attributes.Add("onClick", "javascript:return Confirmacion();")
                'Else
                '    HelpCombo.PortafolioCodigoListar(ddlFondo, PORTAFOLIO_MULTIFONDOS)
                '    'HelpCombo.CargaPortafolioSegunUsuario(ddlFondo, Session("Login"))
            End If
        Else
            'Valores del Pop Up
            If Session("SS_DatosModal") IsNot Nothing Then ObtenerValoresDesdePopup()
            If Not Session("EstadoPantalla") Is Nothing Then
                If Session("EstadoPantalla").ToString() = "Modificar" Or Session("EstadoPantalla").ToString() = "Eliminar" Then
                    EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
                End If
            End If
        End If
    End Sub

    Sub ObtenerValoresDesdePopup()
        Dim datosModal As String() = CType(Session("SS_DatosModal"), String())
        If hfModal.Value = "1" Then
            txtISIN.Text = datosModal(0)
            txtMnemonico.Text = datosModal(1)
            txtSBS.Text = datosModal(2)
            hdCustodio.Value = datosModal(3)
            hdSaldo.Value = datosModal(4)
        ElseIf hfModal.Value = 2 Then
            txtISIN.Text = datosModal(0)
            txtSBS.Text = datosModal(1)
            txtMnemonico.Text = datosModal(2)
            ddlFondo.SelectedValue = datosModal(3)
            ddlOperacion.SelectedValue = datosModal(4)
            lblMoneda.Text = datosModal(5)
            txtCodigoOrden.Value = datosModal(6)
        End If
        Session.Remove("SS_DatosModal")
    End Sub

    Private Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos()
        CargarFechaVencimiento()
    End Sub

    Private Sub btnCaracteristicas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCaracteristicas.Click
        If Me.txtMnemonico.Text <> "" Then
            Session("accionValor") = "MODIFICAR"
            EjecutarJS(UIUtility.MostrarPopUp("../../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" + Me.txtMnemonico.Text + "&vOI=T", "10", 1000, 600, 0, 0, "No", "No", "Yes", "Yes"), False)
        Else
            AlertaJS(ObtenerMensaje("CONF23"))
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Call HabilitaBotones(True, True, True, True, True, True, True, True, True, True, True, False, True, True)
    End Sub

    'Private Sub btnLimites_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimites.Click
    '    Session("prevPag") = "frmInstrumentosEstructurados.aspx"
    '    Session("Instrumento") = "INSTRUMENTOS ESTRUCTURADOS"
    '    Dim Guid As String = ViewState("GUID_Limites")
    '    EjecutarJS(UIUtility.MostrarPopUp("frmConsultaLimitesInstrumento.aspx?GUID=" + Guid, "no", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
    'End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Dim feriado As New FeriadoBM
        Try
            If ValidarFechas() = True Then
                If UIUtility.ValidarHora(Me.tbHoraOperacion.Text) = False Then
                    AlertaJS(ObtenerMensaje("CONF22"))
                    Exit Sub
                End If

                If (feriado.VerificaDia(UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text), Session("Mercado")) = False) Then
                    Me.AlertaJS("Fecha de Vencimiento no es valida.")
                    Exit Sub
                End If
                Session("Procesar") = 1

                ViewState("estadoOI") = ""

                'LIMITES
                If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then OrdenInversion.CalculaLimitesOnLine(Me.Page, DatosRequest, ViewState("estadoOI"), ViewState("GUID_Limites"))

                CargarPaginaProcesar()
            End If
        Catch ex As Exception
            Me.AlertaJS(ex.Message.ToString())
        End Try

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If Session("EstadoPantalla") = "Ingresar" Then
            If Session("Busqueda") = 0 Then
                If Me.ddlFondo.SelectedValue = "" Then
                    If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Or _
                        Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("CONS") Then
                        AlertaJS(ObtenerMensaje("CONF42"))
                    ElseIf Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
                        AlertaJS(ObtenerMensaje("CONF43"))
                        Exit Sub
                    End If
                End If
                ShowDialogPopupValores(txtISIN.Text.Trim.ToUpper, txtSBS.Text.Trim.ToUpper, txtMnemonico.Text.Trim.ToUpper, ddlOperacion.SelectedValue, "IE", "1")
                Session("Busqueda") = 2
            Else
                If Session("Busqueda") = 1 Then
                    CargarFechaVencimiento()
                    If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                        AsignarMensajeBoton(btnAceptar, "CONF1")
                    Else
                        AsignarMensajeBoton(btnAceptar, "CONF15")
                    End If
                    CargarPaginaIngresar()
                    If Me.ddlFondo.SelectedValue <> "" Then
                        CargarCaracteristicasValor()
                    End If
                    CargarIntermediario()
                    If UIUtility.ObtenerCodigoTipoOperacion("COMPRA") = Me.ddlOperacion.SelectedValue Or _
                         UIUtility.ObtenerCodigoTipoOperacion("CONS") = Me.ddlOperacion.SelectedValue Then
                        Me.ddlFondo.Enabled = True
                    End If
                Else
                    Session("Busqueda") = 0
                End If
            End If
        Else
            If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Or Session("EstadoPantalla") = "Consultar" Then
                If Session("Busqueda") = 0 Then
                    txtCodigoOrden.Value = ""
                    Dim strAux As String = String.Empty
                    If (Me.hdPagina.Value = "DA") Then
                        tbFechaOperacion.Text = Request.QueryString("Fecha")
                        strAux = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString()
                    End If
                    Dim strAccion As String
                    If Session("EstadoPantalla") = "Modificar" Then
                        strAccion = "M"
                    ElseIf Session("EstadoPantalla") = "Eliminar" Then
                        strAccion = "E"
                    ElseIf Session("EstadoPantalla") = "Consultar" Then
                        strAccion = "C"
                    End If
                    ShowDialogPopupInversionesRealizadas(Me.txtISIN.Text.Trim, txtSBS.Text.Trim, txtMnemonico.Text.Trim, ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, ddlOperacion.SelectedValue, String.Empty, strAux, strAccion, "2")
                    Session("Busqueda") = 2
                Else
                    If Session("Busqueda") = 1 Then
                        CargarCaracteristicasValor()
                        CargarDatosOrdenInversion()
                        btnAceptar.Enabled = True

                        Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                        If Session("EstadoPantalla") = "Modificar" Then
                            If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                AsignarMensajeBoton(btnAceptar, "CONF2", "Nro " + txtSBS.Text + "?")
                            Else
                                AsignarMensajeBoton(btnAceptar, "CONF16", "Nro " + txtSBS.Text + "?")
                            End If
                            CargarPaginaModificar()
                        ElseIf Session("EstadoPantalla") = "Eliminar" Then
                            If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                AsignarMensajeBoton(btnAceptar, "CONF3", "Nro " + txtSBS.Text + "?")
                            Else
                                AsignarMensajeBoton(btnAceptar, "CONF17", "Nro " + txtSBS.Text + "?")
                            End If
                            CargarPaginaEliminar()
                        ElseIf Session("EstadoPantalla") = "Consultar" Then
                            CargarPaginaConsultar()
                        End If
                    Else
                        Session("Busqueda") = 0
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../frmDefault.aspx", False)
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        LimpiarSesiones()
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlOperacion)
        CargarPaginaAccion()
        Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        If (Me.hdPagina.Value <> "DA") Then
            tbFechaOperacion.Text = objutil.RetornarFechaNegocio
        Else
            tbFechaOperacion.Text = Request.QueryString("Fecha")
        End If
        Me.tbHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Ingresar"
        SincronizarBotones("I")
        hdMensaje.Value = "el Ingreso"
        hdNumUnidades.Value = 0

        If Not ddlFondo.Items.FindByValue(PORTAFOLIO_MULTIFONDOS) Is Nothing Then
            Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS
        End If

        lblTitulo.Text = "PreOrden de Inversión - INSTRUMENTOS ESTRUCTURADOS"

    End Sub

    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        LimpiarSesiones()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Modificar"
        Session("Busqueda") = 0
        Session("Procesar") = 0
        lblAccion.Text = "Modificar"
        CargarPaginaAccion()
        SincronizarBotones("M")
        hdMensaje.Value = "la Modificación"

        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
        HelpCombo.CargarMotivosCambio(Me)
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        LimpiarSesiones()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Eliminar"
        lblAccion.Text = "Eliminar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        SincronizarBotones("E")
        hdMensaje.Value = "la Eliminación"

        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
        HelpCombo.CargarMotivosCambio(Me)
    End Sub

    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        LimpiarSesiones()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        SincronizarBotones("C")
        hdMensaje.Value = "la Consulta"
    End Sub

#End Region

    Private Sub HabilitaBotones(ByVal bLimites As Boolean, ByVal bIngresar As Boolean, _
                                ByVal bModificar As Boolean, ByVal bEliminar As Boolean, ByVal bConsultar As Boolean, _
                                ByVal bAsignar As Boolean, ByVal bProcesar As Boolean, ByVal bImprimir As Boolean, _
                                ByVal bAceptar As Boolean, ByVal bBuscar As Boolean, ByVal bSalir As Boolean, _
                                ByVal bRetornar As Boolean, ByVal bCaracteristicas As Boolean, ByVal bLimitesParametrizados As Boolean)

        'btnLimites.Visible = bLimites
        btnIngresar.Visible = bIngresar
        btnModificar.Visible = bModificar
        btnEliminar.Visible = bEliminar
        btnConsultar.Visible = bConsultar
        'btnAsignar.Visible = bAsignar
        btnProcesar.Visible = bProcesar
        btnImprimir.Visible = bImprimir
        btnAceptar.Visible = bAceptar
        btnBuscar.Visible = bBuscar
        btnSalir.Visible = bSalir
        btnRetornar.Visible = bRetornar
        btnCaracteristicas.Visible = bCaracteristicas
        'btnLimitesParametrizados.Visible = bLimitesParametrizados 'CMB OT 61566 20101122

    End Sub

    Private Sub LimpiarSesiones()
        Session("Procesar") = Nothing
        Session("ValorCustodio") = Nothing
        Session("Mercado") = Nothing
        Session("TipoRenta") = Nothing
        Session("Instrumento") = Nothing
        Session("EstadoPantalla") = Nothing
        Session("Busqueda") = Nothing
        Session("CodigoMoneda") = Nothing
        Session("datosEntidad") = Nothing
        Session("ReporteLimitesEvaluados") = Nothing
        Session("dtdatosoperacion") = Nothing
        Session("accionValor") = Nothing
        Session("dtCuponera") = Nothing
    End Sub

    Public Sub CargarIntermediario()
        If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Or _
            Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("CONS") Then
            UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        ElseIf Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
            UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue) 'modificado DB 20090325. req 20
        End If
        Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
    End Sub

    Public Sub CargarFechaVencimiento()
        If (Me.hdPagina.Value <> "CO") Then 'HDG 20130708
            If (Me.hdPagina.Value <> "DA") Then
                Dim dtAux As DataTable = oPortafolioBM.SeleccionarPortafolioPorFiltro(Me.ddlFondo.SelectedValue, DatosRequest).Tables(0)
                If Not dtAux Is Nothing Then
                    If dtAux.Rows.Count > 0 Then
                        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
                    End If
                End If
            Else
                tbFechaOperacion.Text = Request.QueryString("Fecha")
            End If

            If (txtMnemonico.Text.Trim <> "") Then
                tbFechaLiquidacion.Text = oOrdenInversionBM.RetornarFechaVencimiento(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), Me.txtMnemonico.Text, ddlFondo.SelectedValue, ddlIntermediario.SelectedValue)
            End If
        End If
    End Sub

    Private Function ObtieneCustodiosSaldos() As Boolean

        Dim strCodigoOperacion As String = String.Empty

        If Session("EstadoPantalla") = "Ingresar" Or Session("EstadoPantalla") = "Modificar" Then
            If VerificarSaldosCustodios() = False Then

                'Interpretar Codigos de Operacion (Según Tipo de Negociacion)
                If Me.hdPagina.Value = "TI" Then
                    Select Case ddlOperacion.SelectedValue
                        Case UIUtility.ObtenerCodigoOperacionTIngreso() : strCodigoOperacion = UIUtility.ObtenerCodigoOperacionCompra()
                        Case UIUtility.ObtenerCodigoOperacionTEgreso() : strCodigoOperacion = UIUtility.ObtenerCodigoOperacionVenta()
                    End Select
                Else
                    strCodigoOperacion = ddlOperacion.SelectedValue.ToString()
                End If

                'If strCodigoOperacion = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
                '    AlertaJS(ObtenerMensaje("CONF28"))
                '    Dim strSaldoFaltante As String
                '    strSaldoFaltante = txtUnidadesOp.Text
                '    MostrarOtrosCustodios(Me.tbFechaOperacion.Text, Me.txtMnemonico.Text, Me.ddlFondo.SelectedValue, strSaldoFaltante, Me.hdCustodio.Value)
                '    Return False
                'Else
                '    Return False
                'End If

                Return False
            End If
        End If
        Return True
    End Function

    Private Function VerificarSaldosCustodios() As Boolean
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        Dim decMontoAux As Decimal = 0.0
        Dim cantCustodios As Integer = 0
        Try
            If hdNumUnidades.Value = Me.txtUnidadesOp.Text Then
                Return True
            End If
            '********COMPRA*********
            If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Or _
                Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("CONS") Then
                Dim oValores As New ValoresBM
                Dim blnResul As Boolean = oValores.BuscarCustodioValor(Me.txtMnemonico.Text, Me.ddlFondo.SelectedValue, Me.hdCustodio.Value, DatosRequest)
                If blnResul = False Then
                    AlertaJS(ObtenerMensaje("CONF37"))
                    Return False
                End If
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.txtUnidadesOp.Text
                hdNumUnidades.Value = Me.txtUnidadesOp.Text
                Return True
            End If

            '********VENTA*********
            If Session("ValorCustodio") Is Nothing Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
            ElseIf Session("ValorCustodio") = "" Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
            End If
            decMontoAux = UIUtility.ObtenerSumatoriaSaldosSeleccionados(CType(Session("ValorCustodio"), String), cantCustodios)
            If decMontoAux = Convert.ToDecimal(Me.txtUnidadesOp.Text.Replace(".", UIUtility.DecimalSeparator)) Then
                hdNumUnidades.Value = Me.txtUnidadesOp.Text
                Return True
            ElseIf decMontoAux > Convert.ToDecimal(Me.txtUnidadesOp.Text.Replace(".", UIUtility.DecimalSeparator)) Then
                'redefinir calculos porq excede
                If cantCustodios = 1 Then
                    'porque solamente es el primer custodio
                    Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.txtUnidadesOp.Text
                    hdNumUnidades.Value = Me.txtUnidadesOp.Text
                    Return True
                Else
                    'porq hay mas de un custodio. ajustar montos
                    Session("ValorCustodio") = UIUtility.AjustarMontosCustodios(CType(Session("ValorCustodio"), String), Me.txtUnidadesOp.Text)
                    hdNumUnidades.Value = Me.txtUnidadesOp.Text
                    Return True
                End If
                Return False
            ElseIf decMontoAux < Convert.ToDecimal(Me.txtUnidadesOp.Text.Replace(".", UIUtility.DecimalSeparator)) Then
                'redefinir calculos porq falta
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub ConfiguraModoConsulta()
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        SincronizarBotones("C")
        hdMensaje.Value = "la Consulta"
    End Sub

    Private Sub CargarContactos()
        Dim objContacto As New ContactoBM
        Me.ddlContacto.DataTextField = "DescripcionContacto"
        Me.ddlContacto.DataValueField = "CodigoContacto"
        Me.ddlContacto.DataSource = objContacto.ListarContactoPorTerceros(Me.ddlIntermediario.SelectedValue)
        Me.ddlContacto.DataBind()
        Me.ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        Dim i As Integer
        Dim dtAux As DataTable
        dtAux = CType(Session("datosEntidad"), DataTable)
        If Not dtAux Is Nothing Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("CodigoTercero") = ddlIntermediario.SelectedValue Then
                    If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Or _
                        ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("CONS") Then
                        Me.hdCustodio.Value = dtAux.Rows(i)("codigoCustodio")
                    End If
                    Session("Mercado") = dtAux.Rows(i)("mercado")

                    Exit For
                End If
            Next
        End If
    End Sub

    Private Function ValidarFechas() As Boolean
        Dim dsFechas As PortafolioBE
        Dim drFechas As DataRow
        Dim blnResultado As Boolean = True
        If UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text) > UIUtility.ConvertirFechaaDecimal(Me.tbFechaLiquidacion.Text) Then
            blnResultado = False
            AlertaJS(ObtenerMensaje("CONF7"))
        End If
        If (Me.hdPagina.Value = "DA") Then
            Return True
        End If
        dsFechas = oPortafolioBM.Seleccionar(Me.ddlFondo.SelectedValue, DatosRequest)
        If dsFechas.Tables(0).Rows.Count > 0 Then
            drFechas = dsFechas.Tables(0).NewRow
            drFechas = dsFechas.Tables(0).Rows(0)
            Dim dblFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text)
            Dim dblFechaConstitucion As Decimal = CType(drFechas("FechaConstitucion"), Decimal)
            Dim dblFechaTermino As Decimal = CType(drFechas("FechaTermino"), Decimal)
            If dblFechaConstitucion > dblFechaOperacion Then
                blnResultado = False
                AlertaJS(ObtenerMensaje("CONF4"))
            End If
        End If
        If (objferiadoBM.BuscarPorFecha(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))) = True Then
            blnResultado = False
            AlertaJS(ObtenerMensaje("CONF8"))
        End If
        Return blnResultado
    End Function
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        Try
            If (Me.ddlOperacion.SelectedValue = 3) And hdPagina.Value = "" Then
                If (Me.VerificaSaldoIE() = False) Then
                    AlertaJS("Los elemntos del documento no tiene saldo suficiente para esta operación.")
                    Exit Sub
                End If
            End If
            If Me.hdPagina.Value <> "" And Me.hdPagina.Value <> "DA" And Me.hdPagina.Value <> "TI" And Me.hdPagina.Value <> "MODIFICA" Then
                If Me.hdPagina.Value = "EO" Or Me.hdPagina.Value = "CO" Then
                    ModificarOrdenInversion()
                    CargarPaginaAceptar()
                End If
                If Me.hdPagina.Value = "XO" Then
                    oOrdenInversionWorkFlowBM.ExtornarOIEjecutadas(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, Me.DatosRequest)
                    ReturnArgumentShowDialogPopup()
                Else
                    If Me.hdPagina.Value = "EO" Then
                        oOrdenInversionWorkFlowBM.EjecutarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, Me.tbNPoliza.Text.Trim, Me.DatosRequest)
                        ReturnArgumentShowDialogPopup()
                    Else
                        If Me.hdPagina.Value = "CO" Then
                            oOrdenInversionWorkFlowBM.ConfirmarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, Me.tbNPoliza.Text.Trim, Me.DatosRequest)
                            ReturnArgumentShowDialogPopup()
                        End If
                    End If
                End If
            Else
                If Session("EstadoPantalla") = "Ingresar" Then
                    If Session("Procesar") = 1 Then
                        Dim strcodigoOrden As String
                        strcodigoOrden = InsertarOrdenInversion()
                        If strcodigoOrden <> "" Then
                            If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then 'CMB OT 61566 20101228
                                If ViewState("estadoOI") = "E-EXC" Then

                                    Dim toUser As String = ""
                                    Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                                    Dim dt As DataTable
                                    dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                                    For Each fila As DataRow In dt.Rows
                                        toUser = toUser + fila("Valor").ToString() & ";"
                                    Next
                                    Try
                                        UIUtility.EnviarMail(toUser, "", "Pendiente de aprobación - Orden excedido por Limites de Inversión", MensajeExcesodeLimite(strcodigoOrden), DatosRequest)
                                    Catch ex As Exception
                                        Me.AlertaJS("Se ha generado un error en el proceso de envio de notificación! ") 'CMB OT 61566 20110107 Nro 7 Se agrego la alerta para controlar el error de envio de correo, cuando el servicio este inoperativa
                                    End Try
                                End If
                            End If
                        End If
                        oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                        Me.txtCodigoOrden.Value = strcodigoOrden
                        If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                            AlertaJS(ObtenerMensaje("CONF6"))
                        Else
                            AlertaJS(ObtenerMensaje("CONF18"))
                        End If
                        Session("dtdatosoperacion") = ObtenerDatosOperacion()
                        GenerarLlamado(strcodigoOrden + ViewState("CodigoOrden_T"), Me.ddlFondo.SelectedValue, "INSTRUMENTOS ESTRUCTURADOS", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text)
                        Session("CodigoOI") = strcodigoOrden
                        CargarPaginaAceptar()
                    End If
                Else
                    'Se debe obligar a ingresar el motivo por el cual esta modificando o eliminando
                    Dim strAlerta As String = ""
                    If ddlMotivoCambio.SelectedIndex <= 0 Then
                        strAlerta = "-Elija el motivo por el cual desea " & Session("EstadoPantalla") & " esta operación.\n"
                    End If
                    If txtComentarios.Text.Trim.Length <= 0 Then
                        strAlerta += "-Ingrese los comentarios por el cual desea " & Session("EstadoPantalla") & " esta operación."
                    End If
                    If strAlerta.Length > 0 Then
                        AlertaJS(strAlerta)
                        Exit Sub
                    End If
                    If Session("EstadoPantalla") = "Modificar" Then
                        ModificarOrdenInversion()
                        FechaEliminarModificarOI("M")

                        CargarPaginaAceptar()
                        Session("dtdatosoperacion") = ObtenerDatosOperacion()
                        If Me.hdPagina.Value <> "MODIFICA" Then
                            GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "INSTRUMENTOS ESTRUCTURADOS", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text)
                        Else
                            ReturnArgumentShowDialogPopup()
                        End If
                    ElseIf Session("EstadoPantalla") = "Eliminar" Then
                        EliminarOrdenInversion()
                        FechaEliminarModificarOI("E")
                        CargarPaginaAceptar()
                        CargarPaginaAccion()
                        HabilitaDeshabilitaCabecera(False)
                        Me.lblAccion.Text = ""
                    End If
                End If
                If Session("Procesar") = 0 And (Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Ingresar") Then
                    If CType(ViewState("MontoNeto"), String) = "" Then
                        AlertaJS(ObtenerMensaje("CONF9"))
                    End If
                End If                
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub

    Private Function MensajeExcesodeLimite(ByVal numeroOrden As String) As String
        Dim mensaje As New StringBuilder
        Dim nroOrden As String = numeroOrden
        Dim fondo As String = ddlFondo.SelectedValue
        Dim operacion As String = ddlOperacion.SelectedItem.Text
        Dim orden As String = "Instrumentos Estructurados"
        Dim codISIN As String = txtISIN.Text
        Dim codNem As String = txtMnemonico.Text
        Dim fecha As String = DateTime.Today.ToString("dd/MM/yyyy")

        With mensaje
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='550' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
            .Append("<tr><td colspan='3'>La siguiente orden emitido el " & fecha & ", se encuentra pendiente para su aprobación:</td></tr>")
            .Append("<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td width='35%'>Numero de Orden:</td>")
            .Append("<td colspan='2' width='65%'>" & nroOrden & "</td></tr>")
            .Append("<tr><td width='35%'>Fondo:</td><td colspan='2' width='65%'>" & fondo & "</td></tr>")
            .Append("<tr><td width='35%'>Tipo de Operación:</td><td colspan='2' width='65%'>" & operacion & "</td></tr>")
            .Append("<tr><td width='35%'>Tipo de Orden: </td><td colspan='2' width='65%'>" & orden & "</td></tr>")
            .Append("<tr><td width='35%'>C&oacute;digo ISIN:</td><td colspan='2' width='65%'>" & codISIN & "</td></tr>")
            .Append("<tr><td width='35%'>C&oacute;digo Nem&oacute;nico:</td><td colspan='2' width='65%'>" & codNem & "</td></tr>")
            .Append("<tr height='8'><td colspan='3'></td></tr>")
            .Append("<tr><td colspan='3'><strong>AFP Integra</strong></td></tr>")
            .Append("<tr><td colspan='3'><strong>Grupo SURA</strong></td></tr></table>")
        End With

        Return mensaje.ToString 'CMB OT 61566 20101209
    End Function

    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&vportafolio=" + portafolio + "&vdescripcionPortafolio=" + ddlFondo.SelectedItem.Text + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
    End Sub

    Public Function ObtenerDatosOperacion() As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = Me.tbFechaOperacion.Text
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = Me.tbFechaLiquidacion.Text
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = Me.tbHoraOperacion.Text
        drGrilla("c4") = "Unidades Ordenadas"
        drGrilla("v4") = Me.txtUnidadesOrd.Text
        drGrilla("c5") = "Unidades Operación"
        drGrilla("v5") = Me.txtUnidadesOp.Text
        drGrilla("c6") = "Precio"
        drGrilla("v6") = Me.txtPrecio.Text
        drGrilla("c7") = "Monto Operación"
        drGrilla("v7") = Me.txtMontoNominal.Text
        drGrilla("c8") = "Intermediario"
        drGrilla("v8") = Me.ddlIntermediario.SelectedItem.Text
        If Me.ddlContacto.SelectedIndex <> 0 Then
            drGrilla("c9") = "Contacto"
            drGrilla("v9") = Me.ddlContacto.SelectedItem.Text
        Else
            drGrilla("c9") = ""
            drGrilla("v9") = ""
        End If
        drGrilla("c10") = "Observación"
        drGrilla("v10") = Me.txtObservacion.Text.ToUpper
        If Me.tbNPoliza.Visible = True Then
            drGrilla("c11") = "Número Poliza"
            drGrilla("v11") = Me.tbNPoliza.Text
        Else
            drGrilla("c11") = ""
            drGrilla("v11") = ""
        End If
        drGrilla("c12") = ""
        drGrilla("v12") = ""
        drGrilla("c13") = ""
        drGrilla("v13") = ""
        drGrilla("c14") = ""
        drGrilla("v14") = ""
        drGrilla("c15") = ""
        drGrilla("v15") = ""
        drGrilla("c16") = ""
        drGrilla("v16") = ""
        drGrilla("c17") = ""
        drGrilla("v17") = ""
        drGrilla("c18") = ""
        drGrilla("v18") = ""
        drGrilla("c19") = "Total Comisiones"

        drGrilla("c20") = "Monto Neto Operación"

        drGrilla("c21") = "Precio Promedio"

        'RGF 20080627 se quito todo el bloque de comisiones

        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla

    End Function
    Private Function ReturnArgumentShowDialogPopup() As Boolean

        Dim script As New StringBuilder

        If Me.hdPagina.Value = "CO" Then
            With script
                .Append("<script>")
                .Append("   alert('Se Confirmó la orden correctamente');")
                .Append("   var setArgument = window.dialogArguments;")
                .Append("   window.close()")
                .Append("</script>")
            End With
        Else
            If Me.hdPagina.Value = "EO" Then
                With script
                    .Append("<script>")
                    .Append("   alert('Se Ejecutó la orden correctamente');")
                    .Append("   var setArgument = window.dialogArguments;")
                    .Append("   window.close()")
                    .Append("</script>")
                End With
            Else
                If Me.hdPagina.Value = "XO" Then
                    With script
                        .Append("<script>")
                        .Append("   alert('Se Extornó la orden correctamente');")
                        .Append("   var setArgument = window.dialogArguments;")
                        .Append("   window.close()")
                        .Append("</script>")
                    End With
                Else
                    If Me.hdPagina.Value = "OE" Then
                        With script
                            .Append("<script>")
                            .Append("   var setArgument = window.dialogArguments;")
                            .Append("   window.close()")
                            .Append("</script>")
                        End With
                    Else
                        If Me.hdPagina.Value = "MODIFICA" Then
                            With script
                                .Append("<script>")
                                .Append("   alert('Se Modificó la orden correctamente');")
                                .Append("   var setArgument = window.dialogArguments;")
                                .Append("   window.close()")
                                .Append("</script>")
                            End With
                        End If
                    End If
                End If
            End If
        End If
        Page.RegisterStartupScript(New Guid().ToString(), script.ToString())
    End Function
    Public Function VerificaSaldoIE() As Boolean

        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim ordenIE As New OrdenPreOrdenInversionBM
        Dim ok As Boolean = False
        Dim custodio As String
        Dim codigoCustodio As String
        oOrdenInversionBE = crearObjetoOI()
        custodio = CType(Session("ValorCustodio"), String)

        Dim dr As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow

        Dim items As String() = custodio.Split("&")
        codigoCustodio = items(0)
        dr = CType(oOrdenInversionBE.OrdenPreOrdenInversion.Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        ok = ordenIE.ValidaCantidadIE(dr.CodigoPortafolioSBS, codigoCustodio, dr.CodigoMnemonico, dr.FechaOperacion, dr.CantidadOperacion)
        Return ok
    End Function
    Public Function InsertarOrdenInversion()
        Dim strCodigoOI, strCodigoOI_T As String
        oOrdenInversionBE = crearObjetoOI()
        strCodigoOI = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
        If Me.hdPagina.Value = "TI" Then
            oOrdenInversionBE.OrdenPreOrdenInversion.Rows(0)("CodigoPortafolioSBS") = Me.ddlFondoDestino.SelectedValue
            oOrdenInversionBE.OrdenPreOrdenInversion.Rows(0)("CodigoOperacion") = UIUtility.ObtenerCodigoOperacionTIngreso().ToString()
            Session("ValorCustodio") = UIUtility.ObtieneUnCustodio(Session("ValorCustodio"))
            strCodigoOI_T = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
            ViewState("CodigoOrden_T") = "-" + strCodigoOI_T
        Else
            ViewState("CodigoOrden_T") = ""
        End If
        Return strCodigoOI
    End Function
    Public Sub ModificarOrdenInversion()
        oOrdenInversionBE = crearObjetoOI()
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, Me.hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
    End Sub

    Public Sub EliminarOrdenInversion()
        oOrdenInversionBM.EliminarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, ddlMotivoCambio.SelectedValue, DatosRequest)
    End Sub

    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(Me.ddlFondo.SelectedValue, Me.txtCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc, txtComentarios.Text, DatosRequest)
        txtComentarios.Text = ""
    End Sub

    Public Function crearObjetoOI() As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow

        oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        oOrdenInversionBM.InicializarOrdenInversion(oRow)
        oRow.CodigoOrden = Me.txtCodigoOrden.Value
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.CodigoMoneda = Session("CodigoMoneda")
        oRow.CodigoISIN = txtISIN.Text
        oRow.CodigoMnemonico = txtMnemonico.Text
        oRow.CodigoSBS = Me.txtSBS.Text
        oRow.GrupoIntermediario = ddlGrupoInt.SelectedValue 'DB 20090305
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.CantidadOperacion = Me.txtUnidadesOp.Text.Replace(".", UIUtility.DecimalSeparator)
        oRow.CantidadOrdenado = Me.txtUnidadesOrd.Text.Replace(".", UIUtility.DecimalSeparator)
        oRow.MontoPrima = tbMontoPrima.Text.Replace(".", UIUtility.DecimalSeparator)
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
        oRow.Precio = Convert.ToDecimal(Me.txtPrecio.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.MontoNominalOperacion = Convert.ToDecimal(Me.txtMontoNominal.Text.Replace(".", UIUtility.DecimalSeparator))

        oRow.MontoOperacion = oRow.MontoNominalOperacion

        oRow.HoraOperacion = Me.tbHoraOperacion.Text
        oRow.Observacion = Me.txtObservacion.Text.ToUpper
        oRow.Situacion = "A"

        oRow.CategoriaInstrumento = "IE"

        If Not ViewState("estadoOI") Is Nothing Then
            If ViewState("estadoOI").Equals("E-EXC") Then
                oRow.Estado = ViewState("estadoOI")
            End If
        End If

        If (Me.hdPagina.Value <> "XO") Then
            oRow.NumeroPoliza = Me.tbNPoliza.Text.ToString().Trim
        End If

        If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
            If ddlMotivoCambio.SelectedIndex > 0 Then
                oRow.CodigoMotivoCambio = ddlMotivoCambio.SelectedValue
            End If
            If Session("EstadoPantalla") = "Modificar" Then
                oRow.IndicaCambio = "1"
            End If
        End If

        If (chkFicticia.Checked) Then
            oRow.Ficticia = "S"
        Else
            oRow.Ficticia = "N"
        End If

        If (chkRegulaSBS.Checked) Then
            oRow.RegulaSBS = "S"
        Else
            oRow.RegulaSBS = "N"
        End If
        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()

        Return oOrdenInversionBE
    End Function

    Private Sub CargarCaracteristicasValor()
        Dim dsValor As New DataSet
        Dim drValor As DataRow
        Dim oOIFormulas As New OrdenInversionFormulasBM
        imgFechaOperacion.Attributes.Add("class", "input-append")
        Try
            dsValor = oOIFormulas.SeleccionarCaracValor_InstEstructurados(Me.txtMnemonico.Text, Me.ddlFondo.SelectedValue, DatosRequest)
            If dsValor.Tables(0).Rows.Count > 0 Then
                drValor = dsValor.Tables(0).NewRow
                drValor = dsValor.Tables(0).Rows(0)

                drValor = dsValor.Tables(0).NewRow
                drValor = dsValor.Tables(0).Rows(0)
                Session("TipoRenta") = CType(drValor("val_TipoRenta"), String)
                Session("CodigoMoneda") = CType(drValor("val_CodigoMoneda"), String)
                Session("Mercado") = CType(drValor("val_Mercado"), String)
                Me.lblMoneda.Text = CType(drValor("val_CodigoMoneda"), String)
                Me.txtISIN.Text = CType(drValor("val_CodigoISIN"), String)
                Me.txtSBS.Text = CType(drValor("val_CodigoSBS"), String)

                Me.lblnemo1.Text = CType(drValor("val_nemo1"), String)
                Me.lblporc1.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_porc1")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.00")
                Me.lblprecio1.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_precio1")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblnemo2.Text = CType(drValor("val_nemo2"), String)
                Me.lblporc2.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_porc2")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.00")
                Me.lblprecio2.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_precio2")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblnemo3.Text = CType(drValor("val_nemo3"), String)
                Me.lblporc3.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_porc3")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.00")
                Me.lblprecio3.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_precio3")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblparticipacion.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_porcParticip")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.00")

                'RGF 20090408 En la compra se deben jalar las unidades ingresadas en la parametria
                If UIUtility.ObtenerCodigoTipoOperacion("COMPRA") = ddlOperacion.SelectedValue Or _
                         UIUtility.ObtenerCodigoTipoOperacion("CONS") = ddlOperacion.SelectedValue Then
                    txtUnidadesOrd.Enabled = False
                    txtUnidadesOp.Enabled = False
                    txtUnidadesOrd.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("unidades")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                    txtUnidadesOp.Text = txtUnidadesOrd.Text
                End If
            End If
        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF21"))
        End Try

    End Sub

    Private Sub CargarDatosOrdenInversion()
        Try
            oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, DatosRequest, PORTAFOLIO_MULTIFONDOS)
            Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
            oRow = oOrdenInversionBE.Tables(0).Rows(0)
            Session("CodigoMoneda") = oRow.CodigoMoneda
            txtISIN.Text = oRow.CodigoISIN
            txtMnemonico.Text = oRow.CodigoMnemonico
            txtCodigoOrden.Value = oRow.CodigoOrden
            If oRow.CodigoOperacion.ToString <> "" Then
                ddlOperacion.SelectedIndex = ddlOperacion.Items.IndexOf(ddlOperacion.Items.FindByValue(oRow.CodigoOperacion.ToString()))
            Else
                ddlOperacion.SelectedIndex = 0
            End If
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)

            Me.txtUnidadesOrd.Text = Format(oRow.CantidadOrdenado, "##,##0.0000000")
            Me.txtUnidadesOp.Text = Format(oRow.CantidadOperacion, "##,##0.0000000")
            tbMontoPrima.Text = Format(oRow.MontoPrima, "##,##0.0000000")
            Me.txtPrecio.Text = Format(oRow.Precio, "##,##0.0000000")
            txtMontoNominal.Text = Format(oRow.MontoOperacion, "##,##0.0000000") 'RGF 20090424

            Me.tbHoraOperacion.Text = oRow.HoraOperacion
            txtObservacion.Text = oRow.Observacion
            hdNumUnidades.Value = Me.txtUnidadesOp.Text
            tbNPoliza.Text = oRow.NumeroPoliza.ToString()

            Me.ddlGrupoInt.SelectedValue = oRow.GrupoIntermediario  'modificado DB 20090325
            ViewState("MontoNeto") = txtMontoNominal.Text
            Dim dtAux As DataTable
            dtAux = (New TercerosBM().Seleccionar(oRow.CodigoTercero, DatosRequest)).Tables(0)
            If dtAux.Rows.Count > 0 Then
                Me.hdCustodio.Value = dtAux.Rows(0)("CodigoCustodio")
                CargarIntermediario()
                If ddlIntermediario.Items.Count > 1 Then
                    ddlIntermediario.SelectedValue = oRow.CodigoTercero
                    CargarContactos()
                    ddlContacto.SelectedValue = oRow.CodigoContacto
                Else
                    AlertaJS(ObtenerMensaje("CONF29"))
                End If
            Else
                AlertaJS(ObtenerMensaje("CONF29"))
            End If
            If oRow.Ficticia = "S" Then
                chkFicticia.Checked = True
            Else
                chkFicticia.Checked = False
            End If
        Catch ex As Exception
            If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                AlertaJS(ObtenerMensaje("CONF31"))
            Else
                AlertaJS(ObtenerMensaje("CONF32"))
            End If
        End Try
    End Sub


#Region " /* Métodos Personalizados (Popups Dialogs) */ "

    'Private Sub MostrarOtrosCustodios(ByVal fechaOper As String, ByVal mnemonico As String, ByVal fondo As String, ByVal saldo As String, ByVal codigoCustodio As String)
    '    Dim script As New StringBuilder
    '    With script
    '        '.Append("<script>")
    '        '.Append("function PopupBuscador(fecha, mnemonico, fondo, saldo, codigoCustodio)")
    '        '.Append("{")
    '        '.Append("   var argument = new Object();")
    '        .Append("window.showModalDialog('frmBuscarValorCustodios.aspx?vMnemonico=" + mnemonico + "&vFecha=" + fechaOper + "&vFondo=" + fondo + "&vSaldo=" + saldo + "&vCustodio=" + codigoCustodio + "', '', 'dialogHeight:530px; dialogWidth:800px; dialogLeft:150px;');")
    '        '.Append("   return false;")
    '        '.Append("}")
    '        '.Append("PopupBuscador('" + fechaOper + "','" + mnemonico + "','" + fondo + "','" + saldo + "','" + codigoCustodio + "');")
    '        '.Append("</script>")
    '    End With
    '    EjecutarJS(script.ToString())
    '    'Page.RegisterStartupScript(New Guid().ToString(), script.ToString())
    'End Sub

    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal operacion As String, ByVal categoria As String, ByVal valor As String)

        Dim strURL As String = "frmBuscarValor.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&cFondo=" & ddlFondo.SelectedValue & "&vFondo=" & ddlFondo.SelectedItem.Text & "&vOperacion=" & operacion & "&vCategoria=" & categoria
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='" & valor & "'; ")

    End Sub

    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal cfondo As String, ByVal fondo As String, ByVal operacion As String, ByVal moneda As String, ByVal fecha As String, ByVal accion As String, ByVal valor As String)

        Dim strURL As String = "../frmInversionesRealizadas.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&cFondo=" & cfondo & "&vFondo=" & fondo & "&vOperacion=" & operacion & "&vFechaOperacion=" & fecha & "&vAccion=" & accion & "&vCategoria=IE"
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='" & valor & "'; ")

    End Sub

#End Region

#Region " /* Métodos Controla Habilitar/Deshabilitar Campos */ "

    Private Sub ControlarCamposTI()
        UIUtility.CargarPortafoliosOI(Me.ddlFondoDestino)
        Me.lblFondo.Text = "Fondo Origen"
        Me.lblFondoDestino.Visible = True
        Me.ddlFondoDestino.Visible = True
        MostrarOcultarBotonesAcciones(False)
        Session("ValorCustodio") = ""
        Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        CargarFechaVencimiento()
        tbHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Traspaso"
        hdMensaje.Value = "el Ingreso"
        If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
            AsignarMensajeBoton(btnAceptar, "CONF1")
        Else
            AsignarMensajeBoton(btnAceptar, "CONF15")
        End If
        CargarPaginaIngresar()
        CargarIntermediario()
    End Sub

    Private Sub ControlarCamposEO_CO_XO()
        MostrarOcultarBotonesAcciones(False)
        Me.btnAceptar.Enabled = True
    End Sub

    Private Sub CargarPaginaModificarEO_CO_XO(ByVal acceso As String)
        If acceso = "EO" Or acceso = "CO" Then
            CargarPaginaBuscar()
            HabilitaDeshabilitaCabecera(False)
            Me.btnBuscar.Visible = False
            HabilitaDeshabilitaDatosOperacionComision(True)
            Me.btnCaracteristicas.Visible = True
            Me.btnCaracteristicas.Enabled = True
            Me.btnAceptar.Enabled = True
            Session("EstadoPantalla") = "Modificar"
        End If
    End Sub

    Private Sub ControlarCamposOE()
        MostrarOcultarBotonesAcciones(False)
        Me.btnAceptar.Enabled = True
    End Sub

    Private Sub ControlarCamposDA()

    End Sub

    Private Sub CargarPaginaInicio()
        HabilitaDeshabilitaCabecera(False)
        HabilitaDeshabilitaDatosOperacionComision(False)
        OcultarBotonesInicio()
        Me.btnAceptar.Enabled = False
    End Sub

    Private Sub CargarPaginaBuscar()
        'If Me.ddlFondo.SelectedValue = "MULTIFONDO" Then
        '    Me.btnAsignar.Visible = True
        '    Me.btnAsignar.Enabled = True
        'End If
        Me.btnProcesar.Visible = True
        Me.btnProcesar.Enabled = True
    End Sub

    Private Sub CargarPaginaIngresar()
        CargarPaginaBuscar()
        'Me.btnAsignar.Visible = False
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        Me.btnCaracteristicas.Visible = True
        Me.btnCaracteristicas.Enabled = True
    End Sub

    Private Sub CargarPaginaModificar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        Me.btnCaracteristicas.Visible = True
        Me.btnCaracteristicas.Enabled = True
        'Me.lblComentarios.Text = "Comentarios modificación:"
        Me.txtComentarios.Text = ""
    End Sub

    Private Sub CargarPaginaEliminar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        Me.btnCaracteristicas.Visible = True
        Me.btnCaracteristicas.Enabled = True
        CargarPaginaProcesar()
        'Me.lblComentarios.Text = "Comentarios eliminación:"
        Me.txtComentarios.Text = ""
    End Sub

    Private Sub CargarPaginaConsultar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        Me.btnCaracteristicas.Visible = True
        Me.btnCaracteristicas.Enabled = True
        CargarPaginaProcesar()
        Me.btnAceptar.Enabled = False
    End Sub

    Private Sub CargarPaginaProcesar()
        Dim strJS As New StringBuilder
        strJS.AppendLine("$('#btnAceptar').removeAttr('disabled');")
        If Session("EstadoPantalla") <> "Ingresar" Then
            strJS.AppendLine("$('#btnImprimir').show();")
            strJS.AppendLine("$('#btnImprimir').removeAttr('disabled');")
        End If
        EjecutarJS(strJS.ToString())
    End Sub

    Private Sub CargarPaginaAceptar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        If Session("EstadoPantalla") = "Ingresar" Then
            Me.btnImprimir.Visible = True
            Me.btnImprimir.Enabled = True
            'If Me.ddlFondo.SelectedValue = "MULTIFONDO" Then
            '    Me.btnAsignar.Visible = True
            'End If
        End If
        Me.btnAceptar.Enabled = False
    End Sub

    Private Sub CargarPaginaAccion()
        CargarPaginaInicio()
        LimpiarCaracteristicasValor()
        LimpiarDatosOperacion()
        HabilitaDeshabilitaCabecera(True)
        Me.btnBuscar.Visible = True
        Me.btnBuscar.Enabled = True
        Session("ValorCustodio") = ""
    End Sub

    Private Sub HabilitaDeshabilitaCabecera(ByVal estado As Boolean)
        ddlFondo.Enabled = estado
        ddlOperacion.Enabled = estado
        btnBuscar.Enabled = estado
        txtSBS.ReadOnly = Not estado
        txtISIN.ReadOnly = Not estado
        txtMnemonico.ReadOnly = Not estado
    End Sub

    Private Sub HabilitaDeshabilitaDatosOperacionComision(ByVal estado As Boolean)
        Me.ddlGrupoInt.Enabled = estado
        Me.ddlIntermediario.Enabled = estado
        Me.ddlContacto.Enabled = estado
        If estado Then
            imgFechaOperacion.Attributes.Add("class", "input-append date")
            imgFechaVcto.Attributes.Add("class", "input-append date")
        Else
            imgFechaOperacion.Attributes.Add("class", "input-append")
            imgFechaVcto.Attributes.Add("class", "input-append")
        End If
        ddlIntermediario.Enabled = estado
        ddlContacto.Enabled = estado
        txtUnidadesOrd.ReadOnly = Not estado
        tbMontoPrima.ReadOnly = Not estado
        tbFechaOperacion.ReadOnly = Not estado
        tbFechaLiquidacion.ReadOnly = Not estado
        txtUnidadesOp.ReadOnly = Not estado
        txtPrecio.ReadOnly = Not estado
        txtMontoNominal.ReadOnly = Not estado
        tbHoraOperacion.ReadOnly = Not estado
        txtObservacion.ReadOnly = Not estado
        If (hdPagina.Value = "DA") Then
            tbFechaOperacion.ReadOnly = True
            imgFechaOperacion.Attributes.Add("class", "input-append")
        End If
        chkFicticia.Enabled = False
        If (Not Session("EstadoPantalla") Is Nothing And Not Session("Procesar") Is Nothing) Then
            If Session("EstadoPantalla") = "Ingresar" And Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS And Session("Procesar") = "0" Then
                chkFicticia.Enabled = True
            End If
        End If

        If Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
            Me.chkRegulaSBS.Enabled = False
        Else
            Me.chkRegulaSBS.Enabled = estado
        End If

    End Sub

    Private Sub OcultarBotonesInicio()
        Me.btnBuscar.Visible = False
        Me.btnCaracteristicas.Visible = False
        'Me.btnLimites.Visible = False
        'Me.btnAsignar.Visible = False
        Me.btnProcesar.Visible = False
        Me.btnImprimir.Visible = False
    End Sub

    Private Sub MostrarOcultarBotonesAcciones(ByVal estado As Boolean)
        Me.btnIngresar.Visible = estado
        Me.btnModificar.Visible = estado
        Me.btnEliminar.Visible = estado
        Me.btnConsultar.Visible = estado
    End Sub

    Private Sub LimpiarCaracteristicasValor()
        Me.lblnemo1.Text = ""
        Me.lblporc1.Text = ""
        Me.lblprecio1.Text = ""
        Me.lblnemo2.Text = ""
        Me.lblporc2.Text = ""
        Me.lblprecio2.Text = ""
        Me.lblnemo3.Text = ""
        Me.lblporc3.Text = ""
        Me.lblprecio3.Text = ""
        Me.lblparticipacion.Text = ""

        Me.lblMoneda.Text = ""
        Me.ddlFondo.SelectedIndex = 0
        Me.ddlOperacion.SelectedIndex = 0
        Me.txtISIN.Text = ""
        Me.txtSBS.Text = ""
        Me.txtMnemonico.Text = ""
    End Sub

    Private Sub LimpiarDatosOperacion()
        Me.tbFechaOperacion.Text = ""
        Me.tbFechaLiquidacion.Text = ""
        Me.txtPrecio.Text = ""
        Me.txtMontoNominal.Text = ""
        Me.ddlGrupoInt.SelectedIndex = 0
        Me.ddlIntermediario.SelectedIndex = -1
        CargarContactos()
        Me.ddlContacto.SelectedIndex = 0
        Me.tbHoraOperacion.Text = ""

        Me.txtObservacion.Text = ""

        Me.txtUnidadesOp.Text = ""
        Me.txtUnidadesOrd.Text = ""
        tbMontoPrima.Text = ""
    End Sub

    Private Sub SincronizarBotones(ByVal boton As String)
        'If boton = "I" Then
        '    Me.btnIngresar.ImageUrl = "../../../Common/Imagenes/btc_Ingresar.gif"
        '    Me.btnModificar.ImageUrl = "../../../Common/Imagenes/btn_Modificar.gif"
        '    Me.btnEliminar.ImageUrl = "../../../Common/Imagenes/bt_eliminar.gif"
        '    Me.btnConsultar.ImageUrl = "../../../Common/Imagenes/btn_Consultar.gif"
        'ElseIf boton = "M" Then
        '    Me.btnIngresar.ImageUrl = "../../../Common/Imagenes/bt_ingresar.gif"
        '    Me.btnModificar.ImageUrl = "../../../Common/Imagenes/btc_Modificar.gif"
        '    Me.btnEliminar.ImageUrl = "../../../Common/Imagenes/bt_eliminar.gif"
        '    Me.btnConsultar.ImageUrl = "../../../Common/Imagenes/btn_Consultar.gif"
        'ElseIf boton = "E" Then
        '    Me.btnIngresar.ImageUrl = "../../../Common/Imagenes/bt_ingresar.gif"
        '    Me.btnModificar.ImageUrl = "../../../Common/Imagenes/btn_Modificar.gif"
        '    Me.btnEliminar.ImageUrl = "../../../Common/Imagenes/btc_Eliminar.gif"
        '    Me.btnConsultar.ImageUrl = "../../../Common/Imagenes/btn_Consultar.gif"
        'ElseIf boton = "C" Then
        '    Me.btnIngresar.ImageUrl = "../../../Common/Imagenes/bt_ingresar.gif"
        '    Me.btnModificar.ImageUrl = "../../../Common/Imagenes/btn_Modificar.gif"
        '    Me.btnEliminar.ImageUrl = "../../../Common/Imagenes/bt_eliminar.gif"
        '    Me.btnConsultar.ImageUrl = "../../../Common/Imagenes/btc_Consulta.gif"
        'End If
    End Sub
#End Region

    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "INSTRUMENTOS ESTRUCTURADOS", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text)
    End Sub

    'Private Sub btnAsignar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsignar.Click
    '    Session("URL_Anterior") = Page.Request.Url.AbsolutePath.ToString
    '    If ddlFondo.SelectedValue.ToString = "MULTIFONDO" Then
    '        Response.Redirect("../AsignacionFondos/frmIngresoCriteriosAsignacion.aspx?vISIN=" & Me.txtISIN.Text.ToString & "&vCantidad=" & Me.txtMontoNominal.Text.ToString & "&vMnemonico=" & Me.txtMnemonico.Text.ToString & "&vFondo=" & ddlFondo.SelectedValue.ToString & "&vOperacion=" & ddlOperacion.SelectedItem.Text & "&vMoneda=" & Me.lblMoneda.Text & "&vImpuestosComisiones=0" & "&vCodigoOrden=" & Me.txtCodigoOrden.Value & "&vCategoria=IE", False)
    '    End If
    'End Sub

    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If Session("EstadoPantalla") = "Ingresar" Then
            CargarFechaVencimiento()
        End If
        If Me.ddlFondo.SelectedValue <> "" And Me.btnBuscar.Visible = False Then
            CargarCaracteristicasValor()
        End If
    End Sub

    Private Sub ddlGrupoInt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrupoInt.SelectedIndexChanged
        UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        CargarContactos()
    End Sub
    'Private Sub btnLimitesParametrizados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimitesParametrizados.Click
    '    Try
    '        ConsultaLimitesPorInstrumento() 'CMB OT 61566 20101130
    '    Catch ex As Exception
    '        Me.AlertaJS(ex.Message.ToString())
    '    End Try
    'End Sub
    'Private Sub ConsultaLimitesPorInstrumento()
    '    Dim strFondo As String = ddlFondo.SelectedValue
    '    Dim strEscenario As String = "REAL"
    '    Dim strFecha As String = tbFechaOperacion.Text
    '    Dim strValorNivel As String = txtMnemonico.Text
    '    AlertaJS(UIUtility.MostrarPopUp("../Reportes/Orden de Inversion/frmVisorReporteLimitesPorInstrumento.aspx?Portafolio=" & strFondo & "&ValorNivel=" & strValorNivel & "&Escenario=" & strEscenario & "&Fecha=" & strFecha, "10", 800, 600, 10, 10, "No", "Yes", "Yes", "Yes"), False)

    'End Sub


End Class

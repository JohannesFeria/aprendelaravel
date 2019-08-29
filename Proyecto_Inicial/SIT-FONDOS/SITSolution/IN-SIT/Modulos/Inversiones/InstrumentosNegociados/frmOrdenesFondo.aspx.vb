Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports ParametrosSIT
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmOrdenesFondo
    Inherits BasePage
#Region "Variables"
    Dim oPortafolioBM As New PortafolioBM
    Dim oImpComOP As New ImpuestosComisionesOrdenPreOrdenBM
    Dim objcomisiones As New ImpuestosComisionesBM
    Dim objimpuestoscomisionesOPBM As New ImpuestosComisionesOrdenPreOrdenBM
    Dim objPortafolio As New PortafolioBM
    Dim objTipoOperacion As New TipoOperacionBM
    Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim objutil As New UtilDM
    Dim objtercero As New TercerosBM
    Dim objMoneda As New MonedaBM
    Dim objferiadoBM As New FeriadoBM
    Dim strTipoRenta As String
    Dim oValoresBM As New ValoresBM
    Dim oParamGeneralesBM As New ParametrosGeneralesBM
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Me.hdSaldo.Value = 0
        Me.btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
        If Not Page.IsPostBack Then
            Me.btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
            Me.btnProcesar.Attributes.Add("onclick", "javascript:return Validar();")
            '  Me.btnAceptar.Attributes.Add("onclick", "javascript:return Validar();")
            btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
            LimpiarSesiones()
            If Not Request.QueryString("PTNeg") Is Nothing Then
                Me.hdPagina.Value = Request.QueryString("PTNeg")
            End If
            If (Me.hdPagina.Value = "TI") Then
                UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
            Else
                UIUtility.CargarOperacionOI(ddlOperacion, "OperacionOI")
            End If
            cargaOrdenFondo()
            UIUtility.CargarGrupoIntermediarioOI(ddlGrupoInt)
            CargarTipo()
            CargarPaginaInicio()
            CargarPlaza()
            Me.hdPagina.Value = ""
            HelpCombo.PortafolioCodigoListar(ddlFondo, PORTAFOLIO_MULTIFONDOS)
            If Not Request.QueryString("PTNeg") Is Nothing Then
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                hdRptaConfirmar.Value = "NO"
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                UIUtility.InsertarOtroElementoSeleccion(Me.ddlFondo)
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
                        CargarDatosOrdenInversion(txtCodigoOrden.Value)
                        CargarCaracteristicasValor()
                        ObtieneImpuestosComisiones()
                        tbNPoliza.Text = Right(txtCodigoOrden.Value, 4)
                        tbNPoliza.ReadOnly = True
                        UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                        Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                        If (Me.hdPagina.Value <> "XO") Then
                            Me.lNPoliza.Visible = True
                            Me.tbNPoliza.Visible = True
                        End If
                        ControlarCamposEO_CO_XO()
                        CargarPaginaModificarEO_CO_XO(Me.hdPagina.Value)
                        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                        If hdPagina.Value = "CO" Then
                            btnAceptar.Text = "Grabar y Confirmar"
                            If Session("ValidarFecha").ToString = "FECHADIFERENTE" Then
                                CargarPaginaInicio()
                                btnCaracteristicas.Visible = True
                            End If
                            If UIUtility.ObtenerCodigoTipoOperacion("VENTA") = Me.ddlOperacion.SelectedValue And ddlOrdenFondo.SelectedValue = "M" Then
                                divSaldoDispo.Visible = True
                                txtSaldoDispo.Text = Me.lblSaldoValor.Text
                            End If
                        End If
                        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18            Else
                        If (Me.hdPagina.Value = "OE") Then 'Viene de la Pagina Ordenes Excedidas
                            ControlarCamposOE()
                        Else
                            If (Me.hdPagina.Value = "DA") Then 'Viene de la Pagina Negociacion Dias Anteriores
                                ViewState("ORDEN") = "OI-DA"
                                Me.tbFechaOperacion.Text = Request.QueryString("Fecha")
                                tbFechaOperacion.Enabled = False
                            Else
                                If (Me.hdPagina.Value = "CP") Then 'Viene de la Pagina Liquidaciones Cuentas Por Pagar
                                    Call ConfiguraModoConsulta()
                                    ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                    txtMnemonico.Text = Request.QueryString("Mnemonico")
                                    txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                    ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                    CargarDatosOrdenInversion(txtCodigoOrden.Value)
                                    CargarCaracteristicasValor()
                                    UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                                    Call HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, True, False, False)
                                Else
                                    If (Me.hdPagina.Value = "CONSULTA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                        ConfiguraModoConsulta()
                                        ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                        txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                        Me.tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                        CargarDatosOrdenInversion(txtCodigoOrden.Value)
                                        CargarCaracteristicasValor()
                                        UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                                        HabilitaBotones(False, False, False, False, False, False, False, False, False, False, True, False, False, False)
                                        HabilitaDeshabilitaCabecera(False)
                                    Else
                                        If (Me.hdPagina.Value = "MODIFICA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                            ConfiguraModoConsulta()
                                            ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                            txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                            Me.tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                            CargarDatosOrdenInversion(txtCodigoOrden.Value)
                                            CargarCaracteristicasValor()
                                            UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                                            HabilitaBotones(False, False, False, False, False, False, True, False, True, False, True, False, False, False)
                                            HabilitaDeshabilitaCabecera(False)
                                            HabilitaDeshabilitaDatosOperacionComision(True)
                                            Session("EstadoPantalla") = "Modificar"
                                            lblAccion.Text = "Modificar"
                                            hdMensaje.Value = "la Modificación"
                                            HelpCombo.CargarMotivosCambio(Me)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                End If
                Me.btnSalir.Attributes.Remove("onClick")
                '   Me.btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF49", txtCodigoOrden.Value + "?", "SI")
            End If
        End If
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Call HabilitaBotones(True, True, True, True, True, True, True, True, True, True, True, False, True, False)
        'OT 10031 27/02/2017 - Carlos Espejo
        'Descripcion: Se remuven las sesiones
        Session.Remove("Nemonico")
        Session.Remove("Orden")
        'OT 10031 Fin
    End Sub
    Private Sub HabilitaBotones(ByVal bLimites As Boolean, ByVal bIngresar As Boolean, ByVal bModificar As Boolean, ByVal bEliminar As Boolean, ByVal bConsultar As Boolean,
    ByVal bAsignar As Boolean, ByVal bProcesar As Boolean, ByVal bImprimir As Boolean, ByVal bAceptar As Boolean, ByVal bBuscar As Boolean, ByVal bSalir As Boolean,
    ByVal bRetornar As Boolean, ByVal bCaracteristicas As Boolean, ByVal bLimitesParametrizados As Boolean)
        btnIngresar.Visible = bIngresar
        btnModificar.Visible = bModificar
        btnEliminar.Visible = bEliminar
        btnConsultar.Visible = bConsultar
        btnProcesar.Visible = bProcesar
        btnImprimir.Visible = bImprimir
        btnAceptar.Visible = bAceptar
        btnBuscar.Visible = bBuscar
        btnSalir.Visible = bSalir
        btnRetornar.Visible = bRetornar
        btnCaracteristicas.Visible = bCaracteristicas
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
        drGrilla("c4") = "Fecha Trato"
        drGrilla("v4") = Me.tbFechaTrato.Text
        drGrilla("c5") = "Número Cuotas Ordenado"
        drGrilla("v5") = Me.txtNroFondoOrd.Text
        drGrilla("c6") = "Número Cuotas Operación"
        drGrilla("v6") = Me.txtNroFondoOp.Text
        drGrilla("c7") = "Monto Operación"
        drGrilla("v7") = Me.txtMontoNominal.Text
        drGrilla("c8") = "Precio"
        drGrilla("v8") = Me.txtPrecio.Text
        drGrilla("c9") = "Intermediario"
        drGrilla("v9") = Me.ddlintermediario.SelectedItem.Text
        If Me.ddlContacto.SelectedIndex <> 0 Then
            drGrilla("c10") = "Contacto"
            drGrilla("v10") = Me.ddlContacto.SelectedItem.Text
        Else
            drGrilla("c10") = ""
            drGrilla("v10") = ""
        End If
        drGrilla("c11") = "Observación"
        drGrilla("v11") = Me.txtObservacion.Text
        If Me.tbNPoliza.Visible = True Then
            drGrilla("c12") = "Número Poliza"
            drGrilla("v12") = Me.tbNPoliza.Text
        Else
            drGrilla("c12") = ""
            drGrilla("v12") = ""
        End If
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
        drGrilla("v19") = Me.txttotalComisionesC.Text
        drGrilla("c20") = "Monto Neto Operación"
        drGrilla("v20") = Me.txtMontoNetoOpe.Text
        drGrilla("c21") = "Precio Promedio"
        drGrilla("v21") = Me.txtPrecPromedio.Text
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Sub CargarTipo(Optional ByVal ordenFondo As String = "")
        Dim dtAux As DataTable
        dtAux = New ParametrosGeneralesBM().ListarFondosInversion(DatosRequest, ordenFondo)
        Session("datosTipoTitulo") = dtAux
        ddlTipoFondo.DataSource = dtAux
        HelpCombo.LlenarComboBox(Me.ddlTipoFondo, dtAux, "Valor", "Nombre", True)
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se fija valor por default "Normal" a Tipo Fondo | 25/05/18 
        ddlTipoFondo.SelectedValue = ddlTipoFondo.Items.FindByText("Normal").Value
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se fija valor por default "Normal" a Tipo Fondo | 25/05/18 
    End Sub
    Private Sub cargaOrdenFondo()
        ddlOrdenFondo.DataSource = oParamGeneralesBM.ListarOrdenFondo(DatosRequest)
        ddlOrdenFondo.DataTextField = "Nombre"
        ddlOrdenFondo.DataValueField = "Valor"
        ddlOrdenFondo.DataBind()
    End Sub
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Se quita validacion de trader version antigua
    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            hdPagina.Value = Request.QueryString("PTNeg")
            If ValidarFechas() = True Then
                If UIUtility.ValidarHora(Me.tbHoraOperacion.Text) = False Then
                    AlertaJS(ObtenerMensaje("CONF22"))
                    Exit Sub
                End If
                If (New FeriadoBM().VerificaDia(UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text), Session("Mercado")) = False) Then
                    AlertaJS("Fecha de Vencimiento no es valida.")
                    Exit Sub
                End If
                Session("Procesar") = 1
                If Session("Mercado") = "2" And Me.ddlOrdenFondo.SelectedValue = "M" And ddlGrupoInt.SelectedValue = "BRO" Then
                    If CDec(txtNroFondoOrd.Text) <= 0 Then
                        AlertaJS("Ingrese una cantidad de cuotas mayor a 0")
                        Exit Sub
                    End If
                    'OT 9968 09/02/2017 - Carlos Espejo
                    'Descripcion: Se utiliza la plaza
                    OrdenInversion.ObtieneImpuestosComisiones(dgLista, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlintermediario.SelectedValue)
                    'OT 9968 Fin
                End If
                'LIMITES
                Dim bolValidaLimites As Boolean = True
                If chkEmisionPrimaria.Checked = True Or _
                    (hdCashCall.Value = "1" And ddlTipoFondo.SelectedValue = "CC_SNC") Or _
                    (hdCashCall.Value = "1" And ddlTipoFondo.SelectedValue = "CC_CNC" And UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) >
                     UIUtility.ConvertirFechaaDecimal(objutil.RetornarFechaNegocio())) Then
                    bolValidaLimites = False
                End If
                CalcularComisiones()
                'If Session("TipoRenta") = 2 And Me.ddlOrdenFondo.SelectedValue = "M" Then RestriccionExcesosBroker()
                If ObtieneCustodiosSaldos() = False Then
                    AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                    Exit Sub
                End If
                CargarPaginaProcesar()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub RestriccionExcesosBroker()
        Dim oFormulasOI As New OrdenInversionFormulasBM
        Dim indExceso As String
        Me.hdPagina.Value = Request.QueryString("PTNeg")
        If hdPagina.Value <> "TI" Then
            indExceso = oFormulasOI.RestriccionExcesosBroker(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), ddlintermediario.SelectedValue.ToString(), Convert.ToDecimal(txtMontoNominal.Text), txtMnemonico.Text)

            If indExceso = "E" Then
                If ViewState("estadoOI") = "E-EXC" Then
                    ViewState("estadoOI") = "E-EBL"
                    AlertaJS("Se ha producido Exceso de Límites y Exceso por Broker.\n\nEl monto total de negociaciones realizadas por el Broker en los tres fondos excede el monto maximo a negociar.")
                Else
                    ViewState("estadoOI") = "E-ENV"
                    AlertaJS("El monto total de negociaciones realizadas por el Broker en los tres fondos excede el monto maximo a negociar.")
                End If
            Else
                If ViewState("estadoOI") <> "E-EXC" Then
                    ViewState("estadoOI") = ""
                End If
            End If
        End If
    End Sub
    Private Function ObtieneCustodiosSaldos() As Boolean
        Me.hdPagina.Value = Request.QueryString("PTNeg")
        Dim strCodigoOperacion As String = String.Empty
        If Session("EstadoPantalla") = "Ingresar" Or Session("EstadoPantalla") = "Modificar" Then
            If VerificarSaldosCustodios() = False And (ddlTipoFondo.SelectedValue <> "CC_CNC" Or ddlTipoFondo.SelectedValue <> "CC_SNC") Then
                'Interpretar Codigos de Operacion (Según Tipo de Negociacion)
                If Me.hdPagina.Value = "TI" Then
                    Select Case ddlOperacion.SelectedValue
                        Case UIUtility.ObtenerCodigoOperacionTIngreso() : strCodigoOperacion = UIUtility.ObtenerCodigoOperacionCompra()
                        Case UIUtility.ObtenerCodigoOperacionTEgreso() : strCodigoOperacion = UIUtility.ObtenerCodigoOperacionVenta()
                    End Select
                Else
                    strCodigoOperacion = ddlOperacion.SelectedValue.ToString()
                End If
                Return False
            End If
        End If
        Return True
    End Function
    Private Function VerificarSaldosCustodios() As Boolean
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        Dim decMontoAux As Decimal = 0.0,
            cantNegociada As Decimal = Convert.ToDecimal(Me.txtNroFondoOp.Text.Replace(".", UIUtility.DecimalSeparator))
        Dim cantCustodios As Integer = 0
        Dim oPrevOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim dtSumaUnidades As DataTable
        Try
            decMontoAux = Convert.ToDecimal(Me.lblSaldoValor.Text)
            If hdNumUnidades.Value = Me.txtNroFondoOp.Text Then
                If cantNegociada = decMontoAux Then Return True
            End If
            'COMPRA
            If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.txtNroFondoOp.Text
                hdNumUnidades.Value = Me.txtNroFondoOp.Text
                Return True
            End If
            'VENTA
            If Session("ValorCustodio") Is Nothing Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
            ElseIf Session("ValorCustodio") = "" Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
            End If

            dtSumaUnidades = oPrevOrdenInversionBM.ObtenerUnidadesNegociadasDiaT(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text).Tables(0)
            If dtSumaUnidades.Rows.Count > 0 Then
                cantNegociada += Decimal.Parse(dtSumaUnidades.Compute("Sum(UNIDADES)", String.Empty))
                If Me.hdPagina.Value = "CO" Then
                    cantNegociada -= CDec(hdNumUnidades.Value)
                End If
            End If

            If decMontoAux = cantNegociada Then
                hdNumUnidades.Value = Me.txtNroFondoOp.Text
                Return True
            ElseIf cantNegociada - decMontoAux < 0 Then
                If (cantNegociada - decMontoAux) > -1 Then Return False
                hdNumUnidades.Value = Me.txtNroFondoOp.Text
                Return True
            ElseIf decMontoAux > cantNegociada Then
                'redefinir calculos porq excede
                If cantCustodios = 1 Then
                    'porque solamente es el primer custodio
                    Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.txtNroFondoOp.Text
                    hdNumUnidades.Value = Me.txtNroFondoOp.Text
                Else
                    'por que hay mas de un custodio. ajustar montos
                    Session("ValorCustodio") = UIUtility.AjustarMontosCustodios(CType(Session("ValorCustodio"), String), Me.txtNroFondoOp.Text)
                    hdNumUnidades.Value = Me.txtNroFondoOp.Text
                End If
                Return True
            ElseIf decMontoAux < cantNegociada Then
                'redefinir calculos porq falta
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Me.hdPagina.Value = Request.QueryString("PTNeg")
        If Not Session("SS_DatosModal") Is Nothing Then
            Select Case hdPopUp.Value
                Case "V"
                    txtISIN.Text = CType(Session("SS_DatosModal"), String())(0)
                    txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1)
                    txtSBS.Text = CType(Session("SS_DatosModal"), String())(2)
                    hdCustodio.Value = CType(Session("SS_DatosModal"), String())(3)
                    hdSaldo.Value = CType(Session("SS_DatosModal"), String())(4)
                    'OT 9968 09/02/2017 - Carlos Espejo
                    'Descripcion: Sesiones con los datos del nemonico
                    Session("Nemonico") = CType(Session("SS_DatosModal"), String())(1)
                    'OT 9968 Fin
                Case "IR"
                    txtISIN.Text = CType(Session("SS_DatosModal"), String())(0)
                    txtSBS.Text = CType(Session("SS_DatosModal"), String())(1)
                    txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(2)
                    ddlFondo.SelectedValue = CType(Session("SS_DatosModal"), String())(3)
                    ddlOperacion.SelectedValue = CType(Session("SS_DatosModal"), String())(4)
                    lblMoneda.Text = CType(Session("SS_DatosModal"), String())(5)
                    txtCodigoOrden.Value = CType(Session("SS_DatosModal"), String())(6)
                    'OT 9968 09/02/2017 - Carlos Espejo
                    'Descripcion: Sesiones con los datos de la Orden y el nemonico
                    Session("Nemonico") = CType(Session("SS_DatosModal"), String())(2)
                    Session("Orden") = CType(Session("SS_DatosModal"), String())(6)
                    'OT 9968 Fin
            End Select
            Session.Remove("SS_DatosModal")
        End If
        Dim strAuxOF As String = ""
        If Me.ddlOrdenFondo.SelectedValue = "I" Then
            strAuxOF = "FI"
        ElseIf Me.ddlOrdenFondo.SelectedValue = "M" Then
            strAuxOF = "FM"
        End If
        If Session("EstadoPantalla") = "Ingresar" Then
            If Session("Busqueda") = 0 Then
                If Me.ddlFondo.SelectedValue = "" Then
                    If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                        AlertaJS(ObtenerMensaje("CONF42"))
                    ElseIf Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
                        AlertaJS(ObtenerMensaje("CONF43"))
                        Exit Sub
                    End If
                End If
                ShowDialogPopupValores(txtISIN.Text.Trim.ToUpper, txtSBS.Text.Trim.ToUpper, txtMnemonico.Text.Trim.ToUpper, ddlOperacion.SelectedValue, strAuxOF)
                Session("Busqueda") = 2
            Else
                If Session("Busqueda") = 1 Then
                    CargarFechaVencimiento()
                    If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                        UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
                    Else
                        UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
                    End If
                    CargarPaginaIngresar()
                    If Me.ddlFondo.SelectedValue <> "" Then
                        CargarCaracteristicasValor()
                    End If
                    CargarTipoTramo()
                    CargarIntermediario()
                    CargarMedioTrans()
                    If UIUtility.ObtenerCodigoTipoOperacion("COMPRA") = Me.ddlOperacion.SelectedValue Then
                        Me.ddlFondo.Enabled = True
                    ElseIf UIUtility.ObtenerCodigoTipoOperacion("VENTA") = Me.ddlOperacion.SelectedValue And ddlOrdenFondo.SelectedValue = "M" Then
                        divSaldoDispo.Visible = True
                        txtSaldoDispo.Text = Me.lblSaldoValor.Text
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
                    ShowDialogPopupInversionesRealizadas(Me.txtISIN.Text.ToString.Trim, Me.txtSBS.Text.ToString.Trim, Me.txtMnemonico.Text.ToString.Trim,
                    ddlFondo.SelectedValue, ddlOperacion.SelectedValue, lblMoneda.Text.ToString, strAuxOF, strAux, strAccion, ddlFondo.SelectedItem.Text)
                    Session("Busqueda") = 2
                Else
                    If Session("Busqueda") = 1 Then
                        CargarCaracteristicasValor()
                        CargarDatosOrdenInversion(Session("Orden"))
                        btnAceptar.Enabled = True
                        UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                        Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                        If Session("EstadoPantalla") = "Modificar" Then
                            If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF2", "Nro " + txtCodigoOrden.Value + "?", "SI")
                            Else
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF16", "Nro " + txtCodigoOrden.Value + "?", "SI")
                            End If
                            CargarPaginaModificar()
                        ElseIf Session("EstadoPantalla") = "Eliminar" Then
                            If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF3", "Nro " + txtCodigoOrden.Value + "?", "SI")
                            Else
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF17", "Nro " + txtCodigoOrden.Value + "?", "SI")
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
        MostrarTipoTramo()
    End Sub
    Public Sub CargarIntermediario()
        UIUtility.CargarIntermediariosXGrupoOI(ddlintermediario, ddlGrupoInt.SelectedValue)
        Session("datosEntidad") = CType(ddlintermediario.DataSource, DataSet).Tables(0)
    End Sub
    Public Sub CargarFechaVencimiento()
        Me.hdPagina.Value = Request.QueryString("PTNeg")
        If Session("EstadoPantalla") = "Ingresar" _
            And (chkEmisionPrimaria.Checked = True Or hdCashCall.Value = "1") Then
            tbFechaLiquidacion.Text = tbFechaOperacion.Text
        Else
            If (Me.hdPagina.Value <> "CO") Then
                If (Me.hdPagina.Value <> "DA") Then
                    Dim dtAux As DataTable = oPortafolioBM.SeleccionarPortafolioPorFiltro(Me.ddlFondo.SelectedValue, DatosRequest).Tables(0)
                    If Not dtAux Is Nothing Then
                        If dtAux.Rows.Count > 0 Then
                            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
                            tbFechaTrato.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
                        End If
                    End If
                Else
                    tbFechaOperacion.Text = Request.QueryString("Fecha")
                End If
                If (txtMnemonico.Text.Trim <> "") Then
                    tbFechaLiquidacion.Text = oOrdenInversionBM.RetornarFechaVencimiento(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), Me.txtMnemonico.Text,
                    ddlFondo.SelectedValue, ddlintermediario.SelectedValue)
                End If
            End If
        End If
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se implementa evento Click de botón Salir para mostrar Confirmación de acuerdo a Ventana de llamado  | 07/06/18 
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Dim strMensajeConfirmacion As String = ""
        Dim Pagina As String = hdPagina.Value
        Dim NroOrden As String = txtCodigoOrden.Value
        Dim strAccion As String = hdMensaje.Value

        Select Case Pagina
            Case "TI"
                strMensajeConfirmacion = "¿Desea cancelar el Traspaso de Instrumento?"
            Case "EO"
                strMensajeConfirmacion = "¿Desea cancelar la Ejecución de la orden de inversión Nro. " + NroOrden + "?"
            Case "CO"
                strMensajeConfirmacion = "¿Está seguro de salir de la Confirmación de la orden de inversión Nro. " + NroOrden + "?"
            Case "XO"
                strMensajeConfirmacion = "¿Desea cancelar el Extorno de la orden de inversión Nro. " + NroOrden + "?"
            Case "OE"
                strMensajeConfirmacion = "¿Desea cancelar la Aprobacion de la orden de inversión Excedida Nro. " + NroOrden + "?"
            Case "DA"
                strMensajeConfirmacion = "¿Desea cancelar la Negociación de la orden de inversión Nro. " + NroOrden + "?"
            Case Else
                If (strAccion <> String.Empty) Then
                    If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de la orden de inversión de Acciones?"
                    Else
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de pre-orden de inversión de Acciones?"
                    End If
                End If
        End Select

        If (strMensajeConfirmacion <> String.Empty) Then
            If hdRptaConfirmar.Value.ToUpper = "NO" Then
                ConfirmarJS(strMensajeConfirmacion, "document.getElementById('hdRptaConfirmar').value = 'SI'; document.getElementById('btnSalir').click(); ")
            Else
                hdRptaConfirmar.Value = "NO"
                If strAccion <> String.Empty Then
                    Response.Redirect("~/frmDefault.aspx")
                Else
                    EjecutarJS("window.close()")
                End If
            End If
        Else
            If (Pagina = "CONSULTA" Or Pagina = "MODIFICA") Then EjecutarJS("window.close()")
        End If
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se implementa evento Click de botón Salir para mostrar Confirmación de acuerdo a Ventana de llamado  | 07/06/18 
    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Me.hdPagina.Value = Request.QueryString("PTNeg")
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
        hdMensaje.Value = "el Ingreso"
        ddlTipoFondo.Enabled = True
        hdNumUnidades.Value = 0
        If Not ddlFondo.Items.FindByValue(PORTAFOLIO_MULTIFONDOS) Is Nothing Then
            Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS
        End If
        lblTitulo.Text = "PreOrden de Inversión - FONDOS MUTUOS"
        CargarFechaVencimiento()
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row")
        LimpiarSesiones()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Modificar"
        Session("Busqueda") = 0
        Session("Procesar") = 0
        lblAccion.Text = "Modificar"
        CargarPaginaAccion()
        hdMensaje.Value = "la Modificación"
        ddlTipoFondo.Enabled = True
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row")
        LimpiarSesiones()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Eliminar"
        lblAccion.Text = "Eliminar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Eliminación"
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    Private Sub ConfiguraModoConsulta()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Consulta"
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        LimpiarSesiones()
        Call ConfiguraModoConsulta()
    End Sub
    Private Sub actualizaMontos()
        Dim dblTotalComisiones As Decimal = 0.0
        dblTotalComisiones = UIUtility.ActualizaMontosFinales(dgLista)
        txttotalComisionesC.Text = dblTotalComisiones.ToString.Replace(".", UIUtility.DecimalSeparator)
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - De acuerdo al tipo de operación se restará o sumará al valor total | 27/06/18 
        If (ddlOperacion.SelectedValue = "2") Then dblTotalComisiones = dblTotalComisiones * -1
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - De acuerdo al tipo de operación se restará o sumará al valor total | 27/06/18 

        If Session("Mercado") = "1" Then
            txtMontoNetoOpe.Text = (Me.txtMontoNominal.Text.Replace(".", UIUtility.DecimalSeparator) + dblTotalComisiones).ToString.Replace(UIUtility.DecimalSeparator, ".")
        ElseIf Session("Mercado") = "2" Then
            txtMontoNetoOpe.Text = (Me.txtNroFondoOp.Text.Replace(".", UIUtility.DecimalSeparator) + dblTotalComisiones).ToString.Replace(UIUtility.DecimalSeparator, ".")
        End If
        Dim strMontoAux As String = txtMontoNetoOpe.Text.Replace(",", "")
        strMontoAux = strMontoAux.Replace(".", UIUtility.DecimalSeparator)
        Dim strAccAux As String = txtNroFondoOp.Text.Replace(",", "")
        strAccAux = strAccAux.Replace(".", UIUtility.DecimalSeparator)
        txtPrecPromedio.Text = Format(Convert.ToDecimal(strMontoAux) / Convert.ToDecimal(strAccAux), "##,##0.0000000")
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        Dim accionRpta As String = String.Empty
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        Try
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
            If hdRptaConfirmar.Value.ToUpper = "SI" Then
                EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
                hdPagina.Value = Request.QueryString("PTNeg")
                Dim dtFechaTrato As DateTime
                Dim dtFechaOperacion As DateTime
                Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
                If ObtieneCustodiosSaldos() = False Then
                    AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                    Exit Sub
                End If
                If Me.hdPagina.Value <> "" And Me.hdPagina.Value <> "DA" And Me.hdPagina.Value <> "TI" And Me.hdPagina.Value <> "MODIFICA" Then
                    If Me.hdPagina.Value = "EO" Or Me.hdPagina.Value = "CO" Then
                        ModificarOrdenInversion()
                        UIUtility.InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"),
                        ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                        Session("Modificar") = 0
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
                    If hdPagina.Value = "" Or hdPagina.Value = "TI" Or hdPagina.Value = "DA" Or hdPagina.Value = "MODIFICA" Then
                        If tbFechaTrato.Text.ToString() = String.Empty Then
                            AlertaJS(ObtenerMensaje("ALERT140"))
                            Exit Sub
                        Else
                            dtFechaTrato = Convert.ToDateTime(tbFechaTrato.Text)
                            dtFechaOperacion = Convert.ToDateTime(tbFechaOperacion.Text)
                        End If
                        If Session("Procesar") = 1 Then
                            actualizaMontos()
                        End If
                        If ObtieneCustodiosSaldos() = False Then
                            AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                            Exit Sub
                        End If
                        If Session("EstadoPantalla") = "Ingresar" Then
                            If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) <= UIUtility.ConvertirFechaaDecimal(objutil.RetornarFechaNegocio) _
                                And (chkEmisionPrimaria.Checked = True Or hdCashCall.Value = "1") Then
                                AlertaJS("La fecha de operacion debe ser mayor que la fecha de apertura IDI")
                                Exit Sub
                            End If


                            If Session("Procesar") = 1 Then
                                GuardarPreOrden()
                                accionRpta = "Ingresó"
                                CargarPaginaAceptar()
                            Else
                                AlertaJS("Se debe procesar antes de crear la negociacion")
                                Exit Sub
                            End If

                            '--------------------------------------------------------------------------------------------------------------------
                            'Dim strcodigoOrden As String
                            'strcodigoOrden = InsertarOrdenInversion()
                            'If strcodigoOrden <> "" Then
                            '    If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                            '        Dim toUser As String = ""
                            '        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                            '        Dim dt As DataTable
                            '        If ViewState("estadoOI") = "E-EXC" Or ViewState("estadoOI") = "E-EBL" Then
                            '            dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                            '            For Each fila As DataRow In dt.Rows
                            '                toUser = toUser + fila("Valor").ToString() & ";"
                            '            Next
                            '            Try
                            '                UIUtility.EnviarMail(toUser, "", "Pendiente de aprobación - Orden excedido por Limites de Inversión",
                            '                OrdenInversion.MensajeExcesosOI(strcodigoOrden, ddlFondo.SelectedValue, DatosRequest), DatosRequest)
                            '            Catch ex As Exception
                            '                AlertaJS("Se ha generado un error en el proceso de envio de notificación! ")
                            '            End Try
                            '        End If
                            '    End If
                            'End If
                            'oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                            'Me.txtCodigoOrden.Value = strcodigoOrden
                            'If Me.hdPagina.Value <> "TI" And chkFicticia.Checked = False Then
                            '    If Session("Procesar") = 1 Or Me.ddlOrdenFondo.SelectedValue = "M" Then
                            '        UIUtility.InsertarModificarImpuestosComisiones("I", dgLista, strcodigoOrden, ddlPlaza.SelectedValue, Session("TipoRenta"),
                            '        ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                            '    End If
                            'End If
                            'If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                            '    AlertaJS(ObtenerMensaje("CONF6"))
                            'Else
                            '    AlertaJS(ObtenerMensaje("CONF18"))
                            'End If
                            'Session("dtdatosoperacion") = ObtenerDatosOperacion()
                            'GenerarLlamado(strcodigoOrden + ViewState("CodigoOrden_T"), Me.ddlFondo.SelectedValue, "ORDENES DE FONDO", Me.ddlOperacion.SelectedItem.Text,
                            'Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text, ddlFondo.SelectedItem.Text)
                            'Session("CodigoOI") = strcodigoOrden
                            '--------------------------------------------------------------------------------------------------------------------


                     
                        Else
                            'Se debe obligar a ingresar el motivo por el cual esta modificando o eliminando
                            Dim strAlerta As String = ""
                            If ddlMotivoCambio.SelectedIndex <= 0 Then
                                strAlerta = "-Elija el motivo por el cual desea " & Session("EstadoPantalla") & " esta operación.<br>"
                            End If
                            If strAlerta.Length > 0 Then
                                AlertaJS(strAlerta)
                                Exit Sub
                            End If
                            If Session("EstadoPantalla") = "Modificar" Then
                                actualizaMontos()
                                ModificarOrdenInversion()

                                FechaEliminarModificarOI("M")
                                UIUtility.InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"),
                                ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                                Session("Modificar") = 0
                                CargarPaginaAceptar()
                                Session("dtdatosoperacion") = ObtenerDatosOperacion()
                                If Me.hdPagina.Value <> "MODIFICA" Then
                                    GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "ORDENES DE FONDO", Me.ddlOperacion.SelectedItem.Text,
                                    Session("CodigoMoneda"), Me.txtISIN.Text, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text, ddlFondo.SelectedItem.Text)
                                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
                                    accionRpta = "Modificó"
                                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
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
                                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Eliminar | 11/06/18 
                                accionRpta = "Eliminó"
                                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Eliminar | 11/06/18 
                            End If
                        End If
                        If Session("Procesar") = 0 And (Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Ingresar") Then
                            If CType(ViewState("MontoNeto"), String) = "" Then
                                AlertaJS(ObtenerMensaje("CONF9"))
                            Else
                                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                                retornarMensajeAccionPreOrden(accionRpta)
                                'EjecutarJS("window.location.href = window.location.href")
                                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                            End If
                        Else
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                            If (Session("EstadoPantalla") = "Eliminar" Or (Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Ingresar")) Then
                                retornarMensajeAccionPreOrden(accionRpta)
                                'EjecutarJS("window.location.href = window.location.href")
                            End If

                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

#Region "Registro En la Preorden"
    Sub GuardarPreOrden()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII | 2018-08-17 | Guardado en Pre Orden Inversion

        Dim entPreOrden As New PrevOrdenInversionBE
        Dim negPreOrden As New PrevOrdenInversionBM
        Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow = CType(entPreOrden.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)

        negPreOrden.InicializarPrevOrdenInversion(oRow)

        oRow.CodigoPrevOrden = 0
        oRow.CodigoOperacion = ddlOperacion.SelectedValue 'Compra/Venta/Etc.
        oRow.CodigoNemonico = txtMnemonico.Text
        '  oRow.IndPrecioTasa = ddlModoNegociacion.SelectedValue 'T: Tasa YTM % , P: Precio
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)

        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
        oRow.Cantidad = txtNroFondoOrd.Text.ToUpper.Replace(",", "")
        oRow.CantidadOperacion = txtNroFondoOrd.Text.ToUpper.Replace(",", "")
        'oRow.TipoTasa = ddlTipoTasa.SelectedValue
        'oRow.Tasa = neg.YTM * 100 'Es un Porcentaje
        oRow.Precio = txtPrecio.Text.Replace(",", "")
        oRow.PrecioOperacion = txtPrecio.Text.Replace(",", "")
        oRow.MontoNominal = txtMontoNominal.Text.Replace(",", "")
        oRow.MontoOperacion = txtMontoNominal.Text.Replace(",", "")
        '  oRow.InteresCorrido = neg.InteresCorrido

        oRow.CodigoPlaza = ddlPlaza.SelectedValue
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.HoraOperacion = Now.ToLongTimeString()
        oRow.HoraEjecucion = Replace(Now.ToLongTimeString(), " a.m.", "")
        oRow.IntervaloPrecio = 0

        'Valores por Defecto          
        oRow.MedioNegociacion = "S" 'Por defecto 'ELECTRONICO'
        oRow.TipoFondo = "Normal" 'Por defecto 'NORMAL'
        oRow.TipoTramo = "AGENCIA" 'Por defecto 'AGENCIA'
        oRow.TipoCondicion = "PL" 'Por defecto 'PRECIO LÍMITE'
        oRow.Porcentaje = "N" 'N: No Porcentaje, solo Monto directo
        oRow.Fixing = 0 'Por defecto 
        oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
        oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO

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

        entPreOrden.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
        entPreOrden.PrevOrdenInversion.AcceptChanges()

        Dim dtAsignacion As New DataTable ' Asignacion Por Fondo
        dtAsignacion.Columns.Add("CodigoPortafolio")
        dtAsignacion.Columns.Add("Asignacion")
        'Solo necesitamos una Fila donde se indicará el 100% de unidades para el Fondo
        dtAsignacion.Rows.Add(ddlFondo.SelectedValue, oRow.Cantidad)

        'Guardamos la Pre-Orden
        negPreOrden.Insertar(entPreOrden, ParametrosSIT.TR_RENTA_VARIABLE.ToString, DatosRequest, dtAsignacion)

        HabilitarBotonesAccion()
        'retornarMensajeAccion("Ingresó") 'Notificamos

        'FIN | ZOLUXIONES | RCE | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
    End Sub

    Sub HabilitarBotonesAccion()
        Me.btnIngresar.Visible = True
        Me.btnModificar.Visible = True
        Me.btnEliminar.Visible = True
        Me.btnConsultar.Visible = True

        Me.btnAceptar.Visible = False
    End Sub
#End Region
    Private Sub ReturnArgumentShowDialogPopup()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica el retorno de mensaje de confirmación | 06/06/18
        Select Case hdPagina.Value
            Case "CO"
                AlertaJS("Se Confirmó la orden correctamente", "window.close()")
            Case "EO"
                AlertaJS("Se Ejecutó la orden correctamente", "window.close()")
            Case "XO"
                AlertaJS("Se Extornó la orden correctamente", "window.close()")
            Case "OE"
                EjecutarJS("window.close()")
            Case "MODIFICA"
                AlertaJS("Se Modificó la orden correctamente", "window.close()")
        End Select
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica el retorno de mensaje de confirmación | 06/06/18
    End Sub
    Public Function InsertarOrdenInversion() As String
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
        Dim strTipo As String
        If ddlOrdenFondo.SelectedValue = "M" Then
            strTipo = "FM"    'UNICO POR TIPO
        ElseIf ddlOrdenFondo.SelectedValue = "I" Then
            strTipo = "FI"    'UNICO POR TIPO
        End If
        oOrdenInversionBM.EliminarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, ddlMotivoCambio.SelectedValue, DatosRequest)
        oImpComOP.Eliminar(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, DatosRequest)
    End Sub
    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(Me.ddlFondo.SelectedValue, Me.txtCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc,
        txtComentarios.Text, DatosRequest)
        txtComentarios.Text = ""
    End Sub
    Public Function crearObjetoOI() As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        oOrdenInversionBM.InicializarOrdenInversion(oRow)
        oRow.CodigoOrden = txtCodigoOrden.Value.Trim
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.CodigoMoneda = Session("CodigoMoneda")
        oRow.CodigoISIN = txtISIN.Text
        oRow.CodigoMnemonico = txtMnemonico.Text
        oRow.CodigoSBS = txtSBS.Text
        oRow.GrupoIntermediario = ddlGrupoInt.SelectedValue
        oRow.CodigoTercero = ddlintermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
        oRow.FechaPago = UIUtility.ConvertirFechaaDecimal(tbFechaTrato.Text)
        oRow.FechaTrato = UIUtility.ConvertirFechaaDecimal(tbFechaTrato.Text)
        oRow.Plaza = ddlPlaza.SelectedValue
        If (Me.hdPagina.Value <> "XO") Then
            oRow.NumeroPoliza = Me.tbNPoliza.Text.ToString().Trim
        End If
        If Me.txtNroFondoOp.Text.ToString() <> String.Empty Then oRow.CantidadOperacion = Convert.ToDecimal(Me.txtNroFondoOp.Text.Replace(".", UIUtility.DecimalSeparator))
        If txtNroFondoOrd.Text.ToString() <> String.Empty Then oRow.CantidadOrdenado = Convert.ToDecimal(Me.txtNroFondoOrd.Text.Replace(".", UIUtility.DecimalSeparator))
        If txtMontoNominal.Text.ToString() <> String.Empty Then oRow.MontoOperacion = Convert.ToDecimal(txtMontoNominal.Text.Replace(".", UIUtility.DecimalSeparator))
        If txtPrecio.Text.ToString() <> String.Empty Then oRow.Precio = Convert.ToDecimal(txtPrecio.Text.Replace(".", UIUtility.DecimalSeparator))
        If txttotalComisionesC.Text.ToString() <> String.Empty Then oRow.TotalComisiones = Convert.ToDecimal(txttotalComisionesC.Text.Replace(".", UIUtility.DecimalSeparator))
        If txtPrecPromedio.Text.ToString() <> String.Empty Then oRow.PrecioPromedio = Convert.ToDecimal(txtPrecPromedio.Text.Replace(".", UIUtility.DecimalSeparator))
        If txtMontoNetoOpe.Text.ToString() <> String.Empty Then oRow.MontoNetoOperacion = Convert.ToDecimal(txtMontoNetoOpe.Text.Replace(".", UIUtility.DecimalSeparator))
        If Session("Mercado") = "2" And ddlOrdenFondo.SelectedValue = "M" And ddlTipoFondo.SelectedValue = "ETF" Then 'RGF 20101216
            oRow.TipoTramo = ddlTipoTramo.SelectedValue
        End If
        oRow.Situacion = "A"
        oRow.Observacion = txtObservacion.Text
        oRow.HoraOperacion = Me.tbHoraOperacion.Text
        oRow.TipoFondo = Me.ddlTipoFondo.SelectedValue
        If ddlOrdenFondo.SelectedValue = "M" Then
            oRow.CategoriaInstrumento = "FM"    'UNICO POR TIPO
        ElseIf ddlOrdenFondo.SelectedValue = "I" Then
            oRow.CategoriaInstrumento = "FI"    'UNICO POR TIPO
        End If
        'Validacion (Orden Temporal o Real)
        Dim dtFechaTrato As DateTime
        Dim dtFechaOperacion As DateTime
        dtFechaTrato = Convert.ToDateTime(tbFechaTrato.Text)
        dtFechaOperacion = Convert.ToDateTime(tbFechaOperacion.Text)
        If dtFechaTrato = dtFechaOperacion Or dtFechaOperacion > dtFechaTrato Then
            oRow.IsTemporal = 0  'Orden Real
        ElseIf dtFechaOperacion < dtFechaTrato Then
            oRow.IsTemporal = 1  'Orden Temporal
        End If
        If Not ViewState("estadoOI") Is Nothing Then
            If ViewState("estadoOI").Equals("E-EXC") Or ViewState("estadoOI").Equals("E-ENV") Or ViewState("estadoOI").Equals("E-EBL") Then
                oRow.Estado = ViewState("estadoOI")
            End If
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
        oRow.MedioNegociacion = ddlMedioTrans.SelectedValue
        oRow.HoraEjecucion = tbHoraOperacion.Text
        If Session("EstadoPantalla") = "Ingresar" Then
            If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) > UIUtility.ConvertirFechaaDecimal(objutil.RetornarFechaNegocio) And
            chkEmisionPrimaria.Checked = True Then
                oRow.EventoFuturo = 1
            End If
        Else
            If chkEmisionPrimaria.Checked = True Then
                oRow.EventoFuturo = 1
            End If
        End If
        If ddlMercado.Enabled And (ddlTipoFondo.SelectedValue = "CC_CNC" Or ddlTipoFondo.SelectedValue = "CC_SNC") Then
            oRow.Plaza = ddlMercado.SelectedValue
        End If
        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
    Private Sub CargarContactos()
        Dim strcodigosectorempresarial As String
        Dim objContacto As New ContactoBM
        Me.ddlContacto.DataTextField = "DescripcionContacto"
        Me.ddlContacto.DataValueField = "CodigoContacto"
        Me.ddlContacto.DataSource = objContacto.ListarContactoPorTerceros(Me.ddlintermediario.SelectedValue)
        Me.ddlContacto.DataBind()
        Me.ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        Dim i As Integer
        Dim dtAux As DataTable
        dtAux = IIf(ddlTipoFondo.SelectedValue = "CC_SNC", Nothing, CType(Session("datosEntidad"), DataTable))
        If Not dtAux Is Nothing Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("CodigoTercero") = ddlintermediario.SelectedValue Then
                    If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                        Me.hdCustodio.Value = dtAux.Rows(i)("codigoCustodio")
                    End If
                    strcodigosectorempresarial = IIf(IsDBNull(dtAux.Rows(i)("codigosectorempresarial")), "", dtAux.Rows(i)("codigosectorempresarial"))
                    ViewState("CodigoEntidad") = dtAux.Rows(i)("codigoentidad")
                    Label1.Attributes.Remove("class")
                    Label2.Attributes.Remove("class")
                    Label3.Attributes.Remove("class")
                    If strcodigosectorempresarial = "BROX" Or Me.ddlOrdenFondo.SelectedValue = "M" Then
                        Me.lNPoliza.Visible = True
                        Me.tbNPoliza.Visible = True
                        txttotalComisionesC.Visible = True
                        txtPrecPromedio.Visible = True
                        txtMontoNetoOpe.Visible = True
                    End If
                    Exit For
                End If
            Next
        End If
    End Sub
    Private Sub CargarCaracteristicasValor()
        Dim dsValor As New DataSet
        Dim drValor As DataRow
        Dim oOIFormulas As New OrdenInversionFormulasBM
        If Session("EstadoPantalla") = "Ingresar" _
            And hdCashCall.Value = "0" Then
            chkEmisionPrimaria.Enabled = True
        Else
            chkEmisionPrimaria.Enabled = False
        End If
        Try
            'OT 9968 09/02/2017 - Carlos Espejo
            'Descripcion: Se usa la sesion Nemonico
            dsValor = oOIFormulas.SeleccionarCaracValor_OrdenesFondo(Session("Nemonico"), Me.ddlFondo.SelectedValue, DatosRequest)
            'OT 9968 Fin
            If dsValor.Tables(0).Rows.Count > 0 Then
                drValor = dsValor.Tables(0).NewRow
                drValor = dsValor.Tables(0).Rows(0)
                drValor = dsValor.Tables(0).NewRow
                drValor = dsValor.Tables(0).Rows(0)
                Session("TipoRenta") = CType(drValor("val_TipoRenta"), String)
                Session("CodigoMoneda") = CType(drValor("val_CodigoMoneda"), String)
                If Not ((Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Or (Me.hdPagina.Value = "MODIFICA")) Then
                    Session("Mercado") = CType(drValor("val_Mercado"), String)
                End If
                Me.lblMoneda.Text = CType(drValor("val_CodigoMoneda"), String)
                Me.txtISIN.Text = CType(drValor("val_CodigoISIN"), String)
                Me.txtSBS.Text = CType(drValor("val_CodigoSBS"), String)
                Me.lblSaldoValor.Text = CType(drValor("SaldoValor"), String)
                Me.lbldescripcion.Text = CType(drValor("val_Descripcion"), String)
                Me.lblduracion.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_Duracion")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblemisor.Text = CType(drValor("val_Emisor"), String)
                Me.lblnominales.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_NominalesEmitidos")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblUnidades.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_NominalesUnitarias")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblpreciovector.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_VectorPrecio")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblparticipacion.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_PorParticipacion")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
            End If
        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF21"))
        End Try
    End Sub
    Public Sub CargarDatosOrdenInversion(ByVal CodigoOrden As String)
        Try
            oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(CodigoOrden, Me.ddlFondo.SelectedValue, DatosRequest, PORTAFOLIO_MULTIFONDOS)
            Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
            oRow = oOrdenInversionBE.Tables(0).Rows(0)
            Session("CodigoMoneda") = oRow.CodigoMoneda
            txtISIN.Text = oRow.CodigoISIN
            txtMnemonico.Text = oRow.CodigoMnemonico
            'OT 10031 27/02/2017 - Carlos Espejo
            'Descripcion: Se agrego el nemonico a las sesiones
            Session("Nemonico") = oRow.CodigoMnemonico
            'OT 10031 Fin
            txtCodigoOrden.Value = oRow.CodigoOrden
            If oRow.CodigoOperacion.ToString <> "" Then
                ddlOperacion.SelectedIndex = ddlOperacion.Items.IndexOf(ddlOperacion.Items.FindByValue(oRow.CodigoOperacion.ToString()))
            Else
                ddlOperacion.SelectedIndex = 0
            End If
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
            Me.tbFechaTrato.Text = UIUtility.ConvertirFechaaString(oRow.FechaTrato)
            txtMontoNominal.Text = Format(oRow.MontoOperacion, "##,##0.0000000")
            txtPrecio.Text = Format(oRow.Precio, "##,##0.0000000")
            txttotalComisionesC.Text = Format(oRow.TotalComisiones, "##,##0.0000000")
            txtPrecPromedio.Text = Format(oRow.PrecioPromedio, "##,##0.0000000")
            txtMontoNetoOpe.Text = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
            txtObservacion.Text = oRow.Observacion
            txtNroFondoOp.Text = Format(oRow.CantidadOperacion, "##,##0.0000000")
            txtNroFondoOrd.Text = Format(oRow.CantidadOrdenado, "##,##0.0000000")
            If oRow.TipoFondo.Trim = "" Then
                ddlTipoFondo.SelectedIndex = 0
            Else
                If (ddlTipoFondo.Items.FindByValue(oRow.TipoFondo) Is Nothing) = False Then
                    Me.ddlTipoFondo.SelectedValue = oRow.TipoFondo
                End If
            End If
            If oRow.TipoFondo.Trim = "" Then
                hdCashCall.Value = "0"
            Else
                If oRow.TipoFondo.Trim = "CC_SNC" Or oRow.TipoFondo.Trim = "CC_CNC" Then
                    hdCashCall.Value = "1"
                End If
            End If
            If oRow.CategoriaInstrumento.Trim = "" Then
                ddlOrdenFondo.SelectedIndex = 0
            Else
                If oRow.CategoriaInstrumento = "FM" Then
                    Me.ddlOrdenFondo.SelectedValue = "M"
                Else
                    Me.ddlOrdenFondo.SelectedValue = "I"
                End If
            End If
            hdNumUnidades.Value = Me.txtNroFondoOp.Text
            Me.tbHoraOperacion.Text = oRow.HoraOperacion
            tbNPoliza.Text = oRow.NumeroPoliza.ToString()
            ViewState("MontoNeto") = oRow.MontoOperacion
            CargarPlaza()
            ddlPlaza.SelectedValue = oRow.Plaza
            Me.ddlGrupoInt.SelectedValue = oRow.GrupoIntermediario
            Dim dtAux As DataTable
            dtAux = (New TercerosBM().Seleccionar(oRow.CodigoTercero, DatosRequest)).Tables(0)
            If dtAux.Rows.Count > 0 Then
                hdCustodio.Value = dtAux.Rows(0)("CodigoCustodio")
                CargarIntermediario()
                If ddlintermediario.Items.Count > 1 Then
                    ddlintermediario.SelectedValue = oRow.CodigoTercero
                    CargarContactos()
                    ddlContacto.SelectedValue = oRow.CodigoContacto
                Else
                    If Not (oRow.TipoFondo.Trim = "CC_SNC" Or oRow.TipoFondo.Trim = "CC_CNC") Then
                        AlertaJS(ObtenerMensaje("CONF29"))
                    End If
                End If
            Else
                If Not (oRow.TipoFondo.Trim = "CC_SNC" Or oRow.TipoFondo.Trim = "CC_CNC") Then
                    AlertaJS(ObtenerMensaje("CONF29"))
                End If
            End If
            Me.CargarTipoTramo()
            Me.MostrarTipoTramo()
            If (oRow.TipoTramo <> "") Then
                Me.ddlTipoTramo.SelectedValue = oRow.TipoTramo
            End If
            If oRow.Ficticia = "S" Then
                chkFicticia.Checked = True
            Else
                chkFicticia.Checked = False
            End If

            If oRow.RegulaSBS = "S" Then
                chkRegulaSBS.Checked = True
            Else
                chkRegulaSBS.Checked = False
            End If
            CargarMedioTrans()
            If oRow.MedioNegociacion <> "" Then
                ddlMedioTrans.SelectedValue = oRow.MedioNegociacion
            End If
            tbHoraOperacion.Text = oRow.HoraEjecucion
            If oRow.EventoFuturo = 1 Then
                chkEmisionPrimaria.Checked = True
            Else
                chkEmisionPrimaria.Checked = False
            End If
            If ddlTipoFondo.SelectedValue = "CC_CNC" Or ddlTipoFondo.SelectedValue = "CC_SNC" Then
                CargarMercado()
                ddlMercado.SelectedValue = oRow.Plaza
            Else
                CargarMercado(False)
            End If
        Catch ex As Exception
            If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                AlertaJS(ObtenerMensaje("CONF31"))
            Else
                AlertaJS(ObtenerMensaje("CONF32"))
            End If
        End Try
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
        dsFechas = objPortafolio.Seleccionar(Me.ddlFondo.SelectedValue, DatosRequest)
        If dsFechas.Tables(0).Rows.Count > 0 Then
            drFechas = dsFechas.Tables(0).NewRow
            drFechas = dsFechas.Tables(0).Rows(0)
            Dim dblFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text)
            Dim dblFechaConstitucion As Decimal = CType(drFechas("FechaConstitucion"), Decimal)
            Dim dblFechaTermino As Decimal = CType(drFechas("FechaTermino"), Decimal)
            Dim dblFechaValoracion As Decimal = CType(drFechas("FechaValoracion"), Decimal)
            If (dblFechaConstitucion > dblFechaOperacion) And (dblFechaValoracion > dblFechaOperacion) Then
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
    Public Sub CalcularComisiones()
        Dim dblTotalComisiones As Decimal = 0.0
        If chkreprocesar.Checked Then
            If Me.hdPagina.Value = "TI" Or Me.ddlTipoFondo.SelectedItem.Text.ToUpper = "NORMAL" Then
                dblTotalComisiones = UIUtility.CalculaImpuestosComisionesOF(dgLista, Session("Mercado"), Me.txtMontoNominal.Text.Replace(",", ""),
                txtNroFondoOp.Text.Replace(",", ""), ddlOrdenFondo.SelectedValue, ddlTipoFondo.SelectedItem.Text.ToUpper)
            Else
                If hdCashCall.Value <> "1" Then
                    dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, ddlPlaza.SelectedValue, Me.txtMontoNominal.Text.Replace(",", ""),
                    txtNroFondoOp.Text.Replace(",", ""), ddlGrupoInt.SelectedValue, ddlOperacion.SelectedValue, CLASE_INSTRUMENTO_FONDOMUTUO,
                    ddlTipoTramo.SelectedValue, ddlTipoFondo.SelectedValue)
                End If
            End If
            If Session("Mercado") = "2" And Me.ddlOrdenFondo.SelectedValue = "M" And ddlGrupoInt.SelectedValue = "BRO" Then
                dblTotalComisiones = CalculaImpuestosComisionesIntermediarioExtranjero()
            End If
        Else
            dblTotalComisiones = UIUtility.CalculaImpuestosComisionesNoRecalculo(dgLista, Session("Mercado"), txtMontoNominal.Text.Replace(",", ""),
            txtNroFondoOp.Text.Replace(",", ""), ddlGrupoInt.SelectedValue, ddlOperacion.SelectedValue, CLASE_INSTRUMENTO_ACCION)
        End If
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        If (ddlOperacion.SelectedValue = "2") Then
            txtMontoNetoOpe.Text = Format(txtMontoNominal.Text.Replace(",", "") - dblTotalComisiones, "##,##0.0000000")
        Else
            txtMontoNetoOpe.Text = Format(txtMontoNominal.Text.Replace(",", "") + dblTotalComisiones, "##,##0.0000000")
        End If
        Dim strMontoAux As String = txtMontoNetoOpe.Text.Replace(",", "")
        strMontoAux = strMontoAux.Replace(".", UIUtility.DecimalSeparator)
        Dim strAccAux As String = ""
        If hdCashCall.Value <> "1" Then
            strAccAux = txtNroFondoOp.Text.Replace(",", "")
        End If
        strAccAux = strAccAux.Replace(".", UIUtility.DecimalSeparator)
        If strAccAux <> "" Then
            txtPrecPromedio.Text = Format(Convert.ToDecimal(strMontoAux) / Convert.ToDecimal(strAccAux), "##,##0.0000000")
        Else
            txtPrecPromedio.Text = Format(0, "##,##0.0000000")
        End If
    End Sub
    Private Function CalculaImpuestosComisionesIntermediarioExtranjero() As Decimal
        Dim dbltotalcomisiones As Decimal = 0.0
        Dim oBrokerBM As New EntidadBM
        Dim dt As DataTable
        If (Session("Mercado") = "2") Then 'EXTRANJERO
            dt = oBrokerBM.SeleccionarCostoBroker(Me.txtNroFondoOp.Text, ViewState("CodigoEntidad"), ddlTipoTramo.SelectedValue)
            If dt.Rows.Count > 0 Then
                If (Me.ddlTipoTramo.SelectedValue = "PRINCIPAL") Then
                    Dim index As Integer
                    For index = 0 To dgLista.Rows.Count - 1
                        Dim txtValorComision As TextBox = CType(dgLista.Rows(index).FindControl("txtValorComision1"), TextBox)
                        Dim txtValorComision2 As TextBox = CType(dgLista.Rows(index).FindControl("txtValorComision2"), TextBox)
                        dgLista.Rows(index).Cells(2).Text = "(0)"
                        txtValorComision.Text = "0"
                        dbltotalcomisiones = dbltotalcomisiones + CDec(txtValorComision2.Text)
                    Next
                ElseIf ddlTipoTramo.SelectedValue = "AGENCIA" Then
                    Dim index As Integer
                    For index = 0 To dgLista.Rows.Count - 1
                        Dim txtValorComision As TextBox = CType(dgLista.Rows(index).FindControl("txtValorComision1"), TextBox)
                        Dim txtValorComision2 As TextBox = CType(dgLista.Rows(index).FindControl("txtValorComision2"), TextBox)
                        If (dt.Rows(0)("TipoCosto") = "V") Then 'VALOR
                            dgLista.Rows(index).Cells(2).Text = String.Format("( {0} )", dt.Rows(0)("Costo"))
                            txtValorComision.Text = Me.txtNroFondoOrd.Text * dt.Rows(0)("Costo")
                        ElseIf dt.Rows(0)("TipoCosto") = "P" Then 'PORCENTAJE
                            dgLista.Rows(index).Cells(2).Text = String.Format("( {0}% )", dt.Rows(0)("Costo"))
                            txtValorComision.Text = CDec(Me.txtMontoNominal.Text) * (dt.Rows(0)("Costo") / 100)
                        End If
                        If ddlGrupoInt.SelectedValue = GRUPO_BROKER And dgLista.Rows(index).Cells(0).Text = IMP_COMIS_COM_EXT Then
                            If ddlOperacion.SelectedValue = OPERACION_COMPRA And ddlTipoFondo.SelectedValue = TIPOFONDO_ETF Then
                                txtValorComision.Text = "0.00"
                            End If
                        End If
                        dbltotalcomisiones = dbltotalcomisiones + CDec(txtValorComision.Text)
                        dbltotalcomisiones = dbltotalcomisiones + CDec(txtValorComision2.Text)
                    Next
                End If
            End If
        End If
        Return dbltotalcomisiones
    End Function
    Private Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlintermediario.SelectedIndexChanged
        CargarContactos()
        If (chkEmisionPrimaria.Checked = True Or hdCashCall.Value = "1") And Session("EstadoPantalla") = "Ingresar" Then
            tbFechaOperacion.Enabled = True
        Else
            tbFechaOperacion.Enabled = False
        End If
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlintermediario.SelectedValue)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
    End Sub
    Private Sub btnCaracteristicas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCaracteristicas.Click
        If Me.txtMnemonico.Text <> "" Then
            Session("accionValor") = "MODIFICAR"
            EjecutarJS(UIUtility.MostrarPopUp("../../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" + Me.txtMnemonico.Text +
            "&vOI=T&vOF=1", "10", 1045, 600, 0, 0, "No", "No", "Yes", "Yes"), False)
        Else
            AlertaJS(ObtenerMensaje("CONF23"))
        End If
    End Sub
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String,
    ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal nomPortafolio As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&cportafolio=" + portafolio + "&vportafolio=" + nomPortafolio +
        "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10",
        1000, 600, 0, 0, "No", "Yes", "Yes", "Yes"), False)
    End Sub
#Region " /* Métodos Personalizados (Popups Dialogs) */ "
    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal operacion As String, ByVal categoria As String)
        Dim strURL As String = "frmBuscarValor.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&cFondo=" + ddlFondo.SelectedValue + "&vFondo=" +
        ddlFondo.SelectedItem.Text + "&vOperacion=" + operacion + "&vCategoria=" + categoria
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hdPopUp').value='V';")
    End Sub
    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String,
    ByVal operacion As String, ByVal moneda As String, ByVal categoria As String, ByVal fecha As String, ByVal accion As String, ByVal nomFondo As String)
        Dim strURL As String = "../frmInversionesRealizadas.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&vFondo=" + nomFondo +
        "&cFondo=" + fondo + "&vOperacion=" + operacion + "&vFechaOperacion=" + fecha + "&vAccion=" + accion + "&vCategoria=" + categoria
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hdPopUp').value='IR';")
    End Sub
#End Region
#Region " /* Métodos Controla Habilitar/Deshabilitar Campos */ "
    Private Sub ControlarCamposTI()
        UIUtility.CargarPortafoliosOI(Me.ddlFondoDestino)
        Me.lblFondo.InnerText = "Fondo Origen"
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
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
        Else
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
        End If
        CargarPaginaIngresar()
        CargarIntermediario()
        ObtieneImpuestosComisiones()
        HabilitaDeshabilitaValoresGrilla(False)
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
    Private Sub CargarPaginaInicio()
        HabilitaControlesCashCallSNCuotas(True)
        If Not ddlTipoFondo.SelectedValue Is Nothing Then
            If ddlTipoFondo.SelectedValue = "CC_SNC" Then
                HabilitaControlesCashCallSNCuotas(False)
            End If
        End If
        HabilitaDeshabilitaCabecera(False)
        HabilitaDeshabilitaDatosOperacionComision(False)
        OcultarBotonesInicio()
        Me.btnAceptar.Enabled = False
    End Sub
    Private Sub CargarPaginaBuscar()
        If ddlTipoFondo.SelectedValue = "CC_SNC" Then
            HabilitaControlesCashCallSNCuotas(False)
        Else
            HabilitaControlesCashCallSNCuotas(True)
        End If
        Me.btnProcesar.Visible = True
        Me.btnProcesar.Enabled = True
        Me.btnAceptar.Enabled = False
        Me.btnImprimir.Visible = True
        Me.btnImprimir.Enabled = True
    End Sub
    Private Sub CargarPaginaIngresar()
        CargarPaginaBuscar()
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
        Me.lblComentarios.InnerText = "Comentarios modificación:"
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
        Me.lblComentarios.InnerText = "Comentarios eliminación:"
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
        btnAceptar.Enabled = True
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
        Me.ddlOrdenFondo.Enabled = estado
        ddlFondo.Enabled = estado
        ddlOperacion.Enabled = estado
        btnBuscar.Enabled = estado
        txtSBS.ReadOnly = Not estado
        txtISIN.ReadOnly = Not estado
        txtMnemonico.ReadOnly = Not estado
    End Sub
    Private Sub HabilitaDeshabilitaDatosOperacionComision(ByVal estado As Boolean)
        Me.dgLista.Enabled = estado
        HabilitaDeshabilitaValoresGrilla(estado)
        Me.tbFechaLiquidacion.ReadOnly = Not estado
        Me.ddlGrupoInt.Enabled = estado
        Me.ddlintermediario.Enabled = estado
        Me.ddlContacto.Enabled = estado
        Me.ddlTipoFondo.Enabled = estado
        Me.txtObservacion.ReadOnly = Not estado
        Me.txtNroFondoOp.ReadOnly = Not estado
        Me.txtNroFondoOrd.ReadOnly = Not estado
        Me.tbFechaTrato.ReadOnly = Not estado
        Me.tbHoraOperacion.ReadOnly = Not estado
        Me.txtPrecio.ReadOnly = Not estado
        Me.txtMontoNominal.ReadOnly = Not estado
        Me.txtMontoNetoOpe.ReadOnly = Not estado
        Me.tbHoraOperacion.ReadOnly = Not estado
        If estado Then
            imgFechaVcto.Attributes.Add("class", "input-append date")
            imgFechaTrato.Attributes.Add("class", "input-append date")
        Else
            imgFechaVcto.Attributes.Add("class", "input-append")
            imgFechaTrato.Attributes.Add("class", "input-append")
        End If
        chkFicticia.Enabled = False
        Me.ddlPlaza.Enabled = estado
        chkEmisionPrimaria.Enabled = False
        If (Not Session("EstadoPantalla") Is Nothing And Not Session("Procesar") Is Nothing) Then
            If Session("EstadoPantalla") = "Ingresar" And Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS And Session("Procesar") = "0" Then
                chkFicticia.Enabled = True
                chkEmisionPrimaria.Enabled = True
            End If
        End If
        Me.ddlContacto.Enabled = estado
        If Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
            Me.chkRegulaSBS.Enabled = False
        Else
            Me.chkRegulaSBS.Enabled = estado
        End If
        Me.ddlMedioTrans.Enabled = estado
    End Sub
    Private Sub HabilitaDeshabilitaValoresGrilla(ByVal estado As Boolean)
        Dim i As Integer
        For i = 0 To dgLista.Rows.Count - 1
            CType(dgLista.Rows(i).FindControl("txtValorComision1"), TextBox).Enabled = estado
            CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox).Enabled = estado
        Next
    End Sub
    Private Sub OcultarBotonesInicio()
        Me.btnBuscar.Visible = False
        Me.btnCaracteristicas.Visible = False
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
        Me.lbldescripcion.Text = ""
        Me.lblnominales.Text = ""
        Me.lblemisor.Text = ""
        Me.lblparticipacion.Text = ""
        Me.lblpreciovector.Text = ""
        Me.lblUnidades.Text = ""
        Me.lblduracion.Text = ""
        Me.lblMoneda.Text = ""
        Me.ddlOrdenFondo.SelectedIndex = 0
        Me.ddlFondo.SelectedIndex = 0
        Me.ddlOperacion.SelectedIndex = 0
        Me.ddlTipoFondo.SelectedIndex = 0
        Me.txtISIN.Text = ""
        Me.txtSBS.Text = ""
        Me.txtMnemonico.Text = ""
    End Sub
    Private Sub LimpiarDatosOperacion()
        Me.tbFechaOperacion.Text = ""
        Me.tbFechaLiquidacion.Text = ""
        Me.ddlGrupoInt.SelectedIndex = 0
        If Me.ddlintermediario.Items.Count > 0 Then Me.ddlintermediario.SelectedIndex = 0
        CargarContactos()
        Me.ddlContacto.SelectedIndex = 0
        Me.txtNroFondoOp.Text = ""
        Me.txtNroFondoOrd.Text = ""
        Me.tbFechaTrato.Text = ""
        Me.tbHoraOperacion.Text = ""
        Me.txtPrecio.Text = ""
        Me.txtMontoNominal.Text = ""
        Me.txtObservacion.Text = ""
        Me.txttotalComisionesC.Text = ""
        Me.txtMontoNetoOpe.Text = ""
        Me.txtPrecPromedio.Text = ""
        Me.tbHoraOperacion.Text = ""
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
    End Sub
#End Region
    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "ORDENES DE FONDO", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"),
        Me.txtISIN.Text, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text, ddlFondo.SelectedItem.Text)
    End Sub
    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If ddlFondo.SelectedValue <> "" Then
            CargarFechaVencimiento()
        End If
        Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        If cantidadreg > 0 Then
            AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
            ddlFondo.SelectedIndex = 0
            Exit Sub
        End If
    End Sub
    Private Sub ObtieneImpuestosComisiones()
        If Not Session("TipoRenta") Is Nothing Then
            OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlintermediario.SelectedValue)
        End If
    End Sub
    Private Sub ddlGrupoInt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrupoInt.SelectedIndexChanged
        UIUtility.CargarIntermediariosXGrupoOI(ddlintermediario, ddlGrupoInt.SelectedValue)
        CargarContactos()
        If Session("EstadoPantalla") = "Ingresar" _
        And (chkEmisionPrimaria.Checked = True Or hdCashCall.Value = "1") Then
            tbFechaOperacion.Enabled = True
        Else
            tbFechaOperacion.Enabled = False
        End If
    End Sub
    Private Sub MostrarTipoTramo()
        If Me.ddlTipoFondo.SelectedValue = "ETF" And ddlOrdenFondo.SelectedValue = "M" Then
            lblTipoTramo.Visible = True
            ddlTipoTramo.Visible = True
            If Me.btnBuscar.Visible = False Then
                ddlTipoTramo.Enabled = True
            Else
                ddlTipoTramo.Enabled = False
            End If
        Else
            lblTipoTramo.Visible = False
            ddlTipoTramo.Visible = False
        End If
    End Sub
    Public Sub CargarTipoTramo()
        Dim obm As New ParametrosGeneralesBM
        Dim dt As DataTable = obm.Listar("TIPOTRAMO", Me.DatosRequest)
        HelpCombo.LlenarComboBox(ddlTipoTramo, dt, "Valor", "Nombre", False)
    End Sub
    Public Sub CargarMedioTrans()
        Dim obm As New ParametrosGeneralesBM
        Dim dtMedioTrans As New DataTable
        dtMedioTrans = obm.ListarMedioNegociacionPrevOI(ParametrosSIT.TR_RENTA_VARIABLE).Tables(0)
        HelpCombo.LlenarComboBox(ddlMedioTrans, dtMedioTrans, "Valor", "Nombre", False)
        ddlMedioTrans.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
        ddlMedioTrans.SelectedValue = ParametrosSIT.MEDIO_TRANS_TELF
    End Sub
    Private Sub HabilitaControlesCashCallSNCuotas(ByVal enabled As Boolean)
        lbNroCuotasOp.Visible = enabled
        txtNroFondoOp.Visible = enabled
        lbNroCuotasOrdenado.Visible = enabled
        txtNroFondoOrd.Visible = enabled
        lbPrecio.Visible = enabled
        txtPrecio.Visible = enabled
        tr5.Visible = enabled
        lblTipoTramo.Visible = enabled
        ddlTipoTramo.Visible = enabled
        lbMedioTrans.Visible = enabled
        ddlMedioTrans.Visible = enabled
        tr7.Visible = enabled
        tr8.Attributes.Remove("class")
        If enabled Then
            tr8.Attributes.Add("class", "row")
        Else
            tr8.Attributes.Add("class", "row hidden")
        End If
    End Sub
    Private Sub ddlTipoFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoFondo.SelectedIndexChanged
        MostrarTipoTramo()
        If Session("EstadoPantalla") = "Ingresar" Then
            If chkEmisionPrimaria.Checked = False Then
                tbFechaOperacion.Enabled = False
            End If
            HabilitaControlesCashCallSNCuotas(True)
            If ddlTipoFondo.SelectedValue = "CC_CNC" Then
                chkEmisionPrimaria.Checked = False
                chkEmisionPrimaria.Enabled = False
                hdCashCall.Value = "1"
                tbFechaOperacion.Enabled = True
            Else
                chkEmisionPrimaria.Enabled = True
                hdCashCall.Value = "0"
                If ddlTipoFondo.SelectedValue = "CC_SNC" Then
                    chkEmisionPrimaria.Checked = False
                    chkEmisionPrimaria.Enabled = False
                    hdCashCall.Value = "1"
                    tbFechaOperacion.Enabled = True
                    HabilitaControlesCashCallSNCuotas(False)
                End If
            End If
        End If
        If ddlTipoFondo.SelectedValue = "CC_CNC" Or ddlTipoFondo.SelectedValue = "CC_SNC" Then
            CargarMercado()
        Else
            CargarMercado(False)
        End If
    End Sub
    Private Sub ddlOrdenFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlOrdenFondo.SelectedIndexChanged
        CargarTipo(Convert.ToString(ddlOrdenFondo.SelectedValue))
    End Sub
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dt As DataTable = oMercado.Listar(DatosRequest).Tables(0)
            HelpCombo.LlenarComboBox(ddlMercado, dt, "CodigoMercado", "Descripcion", False)
        Else
            ddlMercado.Items.Clear()
        End If
        ddlMercado.Enabled = enabled
    End Sub
    Protected Sub ddlPlaza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPlaza.SelectedIndexChanged
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        If Not Left(txtSBS.Text, 2) = "50" Then
            ObtieneImpuestosComisiones()
        End If
    End Sub
    Public Sub CargarPlaza()
        Dim oPlazaBM As New PlazaBM
        ddlPlaza.DataSource = oPlazaBM.Listar(Nothing)
        ddlPlaza.DataTextField = "Descripcion"
        ddlPlaza.DataValueField = "CodigoPlaza"
        ddlPlaza.DataBind()
        ddlPlaza.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub
End Class
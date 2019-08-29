Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports ParametrosSIT
Imports UIUtility

Imports System.Globalization
Imports Sit.BusinessLayer.MotorInversiones

Partial Class Modulos_Inversiones_InstrumentosNegociados_frmBonos
    Inherits BasePage
#Region "Variables"
    Dim objutil As New UtilDM
    Dim oImpComOP As New ImpuestosComisionesOrdenPreOrdenBM
    Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim oPortafolioBM As New PortafolioBM
    Dim oValoresBM As New ValoresBM
    Dim objcomisiones As New ImpuestosComisionesBM
    Dim strTipoRenta As String
    Dim objImpuestosComisionesOPBM As New ImpuestosComisionesOrdenPreOrdenBM
    Dim objferiadoBM As New FeriadoBM
    Dim oTempMontoDestino As Decimal
    Dim oTempPrecioPromedio As Decimal
    Dim codigoMultifondo As String = New ParametrosGeneralesBM().SeleccionarPorFiltro(FECHA_FONDO, FECHA_NEGOCIO, "", "", Nothing).Rows(0)("Valor").ToString.Trim
    Dim txtComisionSAB As TextBox
    Dim txtComisionIGV As TextBox
    Dim txtComisionConasev As TextBox
    Dim txtCuotaCavali As TextBox
    Dim txtCuotaBVL As TextBox
#End Region
#Region "Metodos Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Me.hdSaldo.Value = 0
        '     Me.btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
        Me.btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
        Me.btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
        Me.btnModal.Style.Add("display", "none")
        Try
            If Not Page.IsPostBack Then
                Dim oParametrosGenerales As New ParametrosGeneralesBM
                Dim dtMotivos As DataTable = oParametrosGenerales.Listar("MOTCAM", DatosRequest)
                HelpCombo.LlenarComboBox(ddlMotivoCambio, dtMotivos, "Valor", "Nombre", True)

                ' INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Tipo Valorizacion
                Dim dtParams As DataTable = oParametrosGenerales.Listar(ParametrosSIT.TIPO_VALORIZACION, DatosRequest)
                HelpCombo.LlenarComboBox(ddlTipoValorizacion, dtParams, "Valor", "Nombre", True)
                ' FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Tipo Valorizacion

                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                hdRptaConfirmar.Value = "NO"
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                LimpiarSesiones()
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    Me.hdPagina.Value = Request.QueryString("PTNeg")
                End If
                If (Me.hdPagina.Value = "TI") Then
                    CargarOperacionOIParaTraspaso(ddlOperacion)
                Else
                    CargarOperacionOI(ddlOperacion, "OperacionOI")
                End If
                CargarTipoCuponOI(ddlTipoTasa)
                CargarGrupoIntermediarioOI(ddlGrupoInt)
                CargarComboModoNegociacion()
                CargarPaginaInicio()
                Me.hdPagina.Value = ""                
                DivDatosCarta.Visible = False
                DivObservacion.Visible = False
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    ddlFondo.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
                    ddlFondo.DataValueField = "CodigoPortafolio"
                    ddlFondo.DataTextField = "Descripcion"
                    ddlFondo.DataBind()
                    Me.hdPagina.Value = Request.QueryString("PTNeg")
                    If Me.hdPagina.Value = "TI" Then  'Viene de la Pagina Traspaso de Instrumentos
                        Me.txtMnemonico.Text = Request.QueryString("PTCMnemo")
                        Me.ddlFondo.SelectedValue = Request.QueryString("PTFondoO")
                        Me.ddlFondoDestino.SelectedValue = Request.QueryString("PTFondoD")
                        Me.ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                        Me.txtISIN.Text = Request.QueryString("PTISIN")
                        Me.txtSBS.Text = Request.QueryString("PTSBS")
                        Me.lblMoneda.Text = Request.QueryString("PTMon")
                        Me.hdCustodio.Value = Request.QueryString("PTCustodio")
                        Me.hdSaldo.Value = Request.QueryString("PTSaldo")
                        CargarCaracteristicasValor()
                        ObtieneImpuestosComisiones()
                        ControlarCamposTI()
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        txtPrecioNegSucio.Text = "0.0000000"
                        lblInteresCorrido.Text = "0.0000000"
                        txtNroPapeles.Text = "0.0000000"
                        txtInteresCorNeg.Text = "0.0000000"
                        Me.lblPrecioCal.Text = "0.0000000"
                        Me.txtPrecioNegoc.Text = "0.0000000"
                        txtMontoOperacional.Text = "0.00"
                        Me.txtYTM.Text = "0.0000000"
                        ''''''''''''''''''''''''''''''''''''
                        Me.tbFechaOperacion.Text = ConvertirFechaaString(Convert.ToDecimal(Request.QueryString("fechaOperacion")))
                        tbFechaOperacion.Enabled = False
                    Else
                        Me.txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                        Me.ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                        Session("CodOrden") = txtCodigoOrden.Value
                        'ValidaOrigen()
                        'Aqui

                        If (Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Then 'Viene de la Pagina Confirmacion , Ejecucion  o Extorno de Ordenes de Inversion
                            CargarDatosOrdenInversion()
                            CargarCaracteristicasValor()
                            tbNPoliza.Text = Right(txtCodigoOrden.Value, 4)
                            tbNPoliza.ReadOnly = True
                            ObtieneMercadoIntermediario(ddlIntermediario.SelectedValue)
                            ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                            If Session("Plaza") = "7" Then validarComisionSAB(Decimal.Parse(txttotalComisionesC.Text))
                            Session("ValorCustodio") = ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                            If (Me.hdPagina.Value <> "XO") Then
                                Me.lNPoliza.Visible = True
                                Me.tbNPoliza.Visible = True
                            End If
                            ControlarCamposEO_CO_XO()
                            CargarPaginaModificarEO_CO_XO(Me.hdPagina.Value)
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se comprueba si grilla de comisiones está vacía y mercado es local debería mostrarse | 13/06/18 
                            If dgLista.Rows.Count = 0 Then ddlPlaza_SelectedIndexChanged(Nothing, Nothing)
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se comprueba si grilla de comisiones está vacía y mercado es local debería mostrarse | 13/06/18 
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                            If hdPagina.Value = "CO" Then
                                btnAceptar.Text = "Grabar y Confirmar"
                                DivDatosCarta.Visible = True
                                DivObservacion.Visible = True
                                If Session("ValidarFecha").ToString = "FECHADIFERENTE" Then
                                    CargarPaginaInicio()
                                    btnCaracteristicas.Visible = True
                                End If
                            End If
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                        Else
                            If (Me.hdPagina.Value = "OE") Then 'Viene de la Pagina Ordenes Excedidas
                                ControlarCamposOE()
                            Else
                                If (Me.hdPagina.Value = "DA") Then 'Viene de la Pagina Negociacion Dias Anteriores
                                    ViewState("ORDEN") = "OI-DA"
                                    Me.tbFechaOperacion.Text = Request.QueryString("Fecha")
                                    Me.tbFechaOperacion.ReadOnly = True
                                Else
                                    If (Me.hdPagina.Value = "CP") Then 'Viene de la Pagina Liquidaciones Cuentas Por Pagar
                                        Call ConfiguraModoConsulta()
                                        ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                        txtMnemonico.Text = Request.QueryString("Mnemonico")
                                        txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                        Call CargarDatosOrdenInversion()
                                        Call CargarCaracteristicasValor()
                                        ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                                        If Session("Plaza") = "7" Then validarComisionSAB(Decimal.Parse(txttotalComisionesC.Text))
                                        Call HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, False, True, False, False)
                                    Else
                                        If (Me.hdPagina.Value = "CONSULTA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                            ConfiguraModoConsulta()
                                            ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                            txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                            Me.tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                            CargarDatosOrdenInversion()
                                            CargarCaracteristicasValor()
                                            ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                                            If Session("Plaza") = "7" Then validarComisionSAB(Decimal.Parse(txttotalComisionesC.Text))
                                            HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, True, False, False, False)
                                            HabilitaDeshabilitaCabecera(False)
                                        Else
                                            If (Me.hdPagina.Value = "MODIFICA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN 
                                                ConfiguraModoConsulta()
                                                ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                                txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                                Me.tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                                CargarDatosOrdenInversion()
                                                CargarCaracteristicasValor()
                                                ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                                                If Session("Plaza") = "7" Then validarComisionSAB(Decimal.Parse(txttotalComisionesC.Text))
                                                HabilitaBotones(False, False, False, False, False, False, False, True, False, True, False, True, False, False, False)
                                                HabilitaDeshabilitaCabecera(False)
                                                HabilitaDeshabilitaDatosOperacionComision(True)
                                                Session("EstadoPantalla") = "Modificar"
                                                lblAccion.Text = "Modificar"
                                                hdMensaje.Value = "la Modificación"
                                                EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    '==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-21 | Calculo del TIR Neto
                    Me.pnlTirNeto.Visible = (Me.hdPagina.Value <> "") 'Solo será visible si hdPagina.Value tiene un valor
                    '==== FIN | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-21 | Calculo del TIR Neto

                    Me.btnSalir.Attributes.Remove("onClick")
                    '   Me.btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF49", txtCodigoOrden.Value + "?", "SI")
                Else
                    ddlFondo.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
                    ddlFondo.DataValueField = "CodigoPortafolio"
                    ddlFondo.DataTextField = "Descripcion"
                    ddlFondo.DataBind()
                    ddlFondo.Items.Insert(0, New ListItem("--Seleccione--", ""))
                End If
            Else
                'Valores del Pop Up
                If Session("SS_DatosModal") IsNot Nothing Then ObtenerValoresDesdePopup()
                If Not Session("EstadoPantalla") Is Nothing Then
                    If Session("EstadoPantalla").ToString() = "Modificar" Or Session("EstadoPantalla").ToString() = "Eliminar" Then
                        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
                        If ddlMotivoCambio.Items.Count = 0 Then HelpCombo.CargarMotivosCambio(Me)
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Sub ObtenerValoresDesdePopup()
        Dim datosModal As String() = CType(Session("SS_DatosModal"), String())
        If hfModal.Value = "1" Then
            txtISIN.Text = datosModal(0)
            txtMnemonico.Text = datosModal(1)
            txtSBS.Text = datosModal(2)
            hdCustodio.Value = datosModal(3)
            hdSaldo.Value = datosModal(4)
            Session("CodigoNemonico") = datosModal(1)
        ElseIf hfModal.Value = "2" Then
            txtISIN.Text = datosModal(0)
            txtSBS.Text = datosModal(1)
            txtMnemonico.Text = datosModal(2)
            ddlFondo.SelectedValue = datosModal(3)
            ddlOperacion.SelectedValue = datosModal(4)
            lblMoneda.Text = datosModal(5)
            Session("CodigoOrden") = datosModal(6)
            Session("CodigoNemonico") = datosModal(2)
        End If
        Session.Remove("SS_DatosModal")
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Call HabilitaBotones(True, True, True, True, True, True, True, True, True, False, True, True, False, True, True)
    End Sub
#End Region
    Private Sub HabilitaBotones(ByVal bCuponera As Boolean, ByVal bLimites As Boolean, ByVal bIngresar As Boolean, _
                                ByVal bModificar As Boolean, ByVal bEliminar As Boolean, ByVal bConsultar As Boolean, _
                                ByVal bAsignar As Boolean, ByVal bProcesar As Boolean, ByVal bImprimir As Boolean, _
                                ByVal bAceptar As Boolean, ByVal bBuscar As Boolean, ByVal bSalir As Boolean, _
                                ByVal bRetornar As Boolean, ByVal bCaracteristicas As Boolean, ByVal bLimitesParametrizados As Boolean)
        btnIngresar.Visible = bIngresar
        btnModificar.Visible = bModificar
        btnEliminar.Visible = bEliminar
        btnConsultar.Visible = bConsultar
        btnProcesar.Visible = bProcesar
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
        Session("ValorCustodio") = Nothing
        Session("dtdatosoperacion") = Nothing
        Session("accionValor") = Nothing
        Session("dtCuponera") = Nothing
        Session("Plaza") = Nothing
    End Sub
    Private Sub btnVista_Click(ByVal sender As System.Object, ByVal e As EventArgs)
        Session("prevPag") = "../InstrumentosNegociados/frmBonos.aspx"
        Response.Redirect("../Reportes/frmVisorOrdenesDeInversion.aspx?titulo=Ordenes de Inversión - Bonos")
    End Sub
    Private Sub btnLimites_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnLimites.Click
        Try
            Dim Guid As String = ViewState("GUID_Limites")
            'Solo se debe mostrar la ventana de Consulta de Limites, cuando hay limites excedidos
            Dim dstLimites As DataSet = CType(Session(Guid), DataSet)
            If dstLimites Is Nothing Then
                OrdenInversion.CalculaLimitesOnLine(Me, DatosRequest, ViewState("estadoOI"), ViewState("GUID_Limites"))
                Guid = ViewState("GUID_Limites")
                dstLimites = CType(Session(Guid), DataSet)
            End If
            If (dstLimites.Tables.Count > 0) Then
                If (dstLimites.Tables(0).Rows.Count > 0) Then
                    Session("Instrumento") = "BONOS"
                    EjecutarJS(UIUtility.MostrarPopUp("frmConsultaLimitesInstrumento.aspx?GUID=" + Guid, "no", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                Else
                    AlertaJS("¡No hay exceso de limites!")
                End If
            End If
        Catch ex As Exception
            EjecutarJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnCuponera_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnCuponera.Click
        ShowDialogCuponera(txtMnemonico.Text.ToString)
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
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de la orden de inversión de Bonos?"
                    Else
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de pre-orden de inversión de Bonos?"
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
            If (Pagina = "CONSULTA" Or Pagina = "MODIFICA") Then
                EjecutarJS("window.close()")
            ElseIf Pagina = "CP" Then
                EjecutarJS("javascript:history.back();") 'Si viene de la Pagina Liquidaciones Cuentas Por Pagar
            End If
        End If
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se implementa evento Click de botón Salir para mostrar Confirmación de acuerdo a Ventana de llamado  | 07/06/18 
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnBuscar.Click
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se centraliza el Try ... Catch  en uno solo por evento | 11/06/18 
        Try
            txtPrecioNegSucio.Text = "0.0000000"
            lblInteresCorrido.Text = "0.0000000"
            txtNroPapeles.Text = "0.0000000"
            txtInteresCorNeg.Text = "0.0000000"
            lblPrecioCal.Text = "0.0000000"
            txtPrecioNegoc.Text = "0.0000000"
            txtMontoOperacional.Text = "0.00"
            txtYTM.Text = "0.0000000"
            If Session("EstadoPantalla") = "Ingresar" Then
                If Session("Busqueda") = 0 Then
                    If Me.ddlFondo.SelectedValue = "" Then
                        If Me.ddlOperacion.SelectedValue = ObtenerCodigoTipoOperacion("COMPRA") Then
                            AlertaJS(ObtenerMensaje("CONF42"))
                        ElseIf Me.ddlOperacion.SelectedValue = ObtenerCodigoTipoOperacion("VENTA") Then
                            AlertaJS(ObtenerMensaje("CONF43"))
                            Exit Sub
                        End If
                    End If
                    ShowDialogPopupValores(txtISIN.Text.Trim.ToUpper, txtSBS.Text.Trim.ToUpper, txtMnemonico.Text.Trim.ToUpper, ddlFondo.SelectedItem.Text,
                    ddlFondo.SelectedValue, ddlOperacion.SelectedValue, "BO", 1)
                    Session("Busqueda") = 2
                Else
                    If Session("Busqueda") = 1 Then
                        If CargarCaracteristicasValor() Then

                            CargarFechaVencimiento()
                            If Me.ddlFondo.SelectedValue <> codigoMultifondo Then
                                '    btnAceptar.Attributes.Add("onClick", "javascript:return Salida();")
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
                                chkFicticia.Enabled = True
                            Else
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
                            End If
                            CargarPaginaIngresar()
                            CargarPlaza()
                            HabilitarModoNegociacion()

                            UIUtility.ResaltaCajaTexto(tbFechaLiquidacion, True)
                            UIUtility.ResaltaCajaTexto(txtYTM, True)
                            UIUtility.ResaltaCajaTexto(txtNroPapeles, True)
                            'UIUtility.ResaltaCajaTexto(txtMontoOperacional, True)
                            UIUtility.ResaltaCombo(ddlTipoTasa, True)
                            UIUtility.ResaltaCombo(ddlPlaza, True)
                            UIUtility.ResaltaCombo(ddlGrupoInt, True)
                            UIUtility.ResaltaCombo(ddlIntermediario, True)

                            CargarIntermediario()
                            If ObtenerCodigoTipoOperacion("COMPRA") = Me.ddlOperacion.SelectedValue Then
                                Me.ddlFondo.Enabled = True
                            End If
                        Else
                            Session("Busqueda") = 0
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
                            strAux = ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString()
                        End If
                        Dim strAccion As String = String.Empty
                        If Session("EstadoPantalla") = "Modificar" Then
                            strAccion = "M"
                        ElseIf Session("EstadoPantalla") = "Eliminar" Then
                            strAccion = "E"
                        ElseIf Session("EstadoPantalla") = "Consultar" Then
                            strAccion = "C"
                        End If
                        ShowDialogPopupInversionesRealizadas(Me.txtISIN.Text.ToString.Trim, Me.txtSBS.Text.ToString.Trim, Me.txtMnemonico.Text.ToString.Trim,
                        ddlFondo.SelectedItem.Text, ddlFondo.SelectedValue, ddlOperacion.SelectedValue, lblMoneda.Text.ToString, strAux, strAccion, "2")
                        Session("Busqueda") = 2
                    Else
                        If Session("Busqueda") = 1 Then
                            txtCodigoOrden.Value = Session("CodigoOrden")
                            CargarPlaza()
                            CargarCaracteristicasValor()
                            CargarDatosOrdenInversion()
                            Me.btnAceptar.Visible = True
                            ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                            Session("ValorCustodio") = ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                            lblSaldoValor.Text = CargarSaldoXTercero(lblSaldoValor.Text, ddlOperacion.SelectedValue, ddlIntermediario.SelectedValue, ddlFondo.SelectedValue,
                            txtMnemonico.Text, tbFechaOperacion.Text, DatosRequest)
                            SaldoUnidades_EnModificacion(lblSaldoValor, CDec(hdNumUnidades.Value), ddlOperacion.SelectedValue)
                            If Session("EstadoPantalla") = "Modificar" Then
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF2", "Nro " + txtCodigoOrden.Value + "?", "SI")
                                CargarPaginaModificar()
                            ElseIf Session("EstadoPantalla") = "Eliminar" Then
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF3", "Nro " + txtCodigoOrden.Value + "?", "SI")
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
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se centraliza el Try ... Catch  en uno solo por evento | 11/06/18 
    End Sub
    Public Sub CargarPlaza()
        Dim oPlazaBM As New PlazaBM
        ddlPlaza.DataSource = oPlazaBM.Listar(Nothing)
        ddlPlaza.DataTextField = "Descripcion"
        ddlPlaza.DataValueField = "CodigoPlaza"
        ddlPlaza.DataBind()
        ddlPlaza.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub
    Public Sub CargarIntermediario()
        CargarIntermediariosCustodioXGrupoInterOI(ddlIntermediario, Me.hdCustodio.Value, ddlGrupoInt.SelectedValue)
        Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
    End Sub
    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnIngresar.Click
        LimpiarSesiones()
        '------------------------------------------------------------------
        'GUID: Identificador único para cuponetas temporales
        '------------------------------------------------------------------
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        '------------------------------------------------------------------
        InsertarOtroElementoSeleccion(Me.ddlFondo)
        ExcluirOtroElementoSeleccion(Me.ddlOperacion)
        CargarPaginaAccion()
        Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        If (Me.hdPagina.Value <> "DA") Then
            tbFechaOperacion.Text = objutil.RetornarFechaNegocio
        Else
            tbFechaOperacion.Text = Request.QueryString("Fecha")
        End If
        Me.txtHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Ingresar"
        hdMensaje.Value = "el Ingreso"
        ViewState("IsIndica") = True
        hdNumUnidades.Value = 0
        ddlFondo.SelectedIndex = 0
        chkRecalcular.Checked = True

        HabilitarBotonGuardarPreOrden()
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnModificar.Click
        LimpiarSesiones()
        '------------------------------------------------------------------
        'GUID: Identificador único para cuponetas temporales
        '------------------------------------------------------------------
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        '------------------------------------------------------------------
        ExcluirOtroElementoSeleccion(Me.ddlFondo)
        InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Modificar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        lblAccion.Text = "Modificar"
        CargarPaginaAccion()
        hdMensaje.Value = "la Modificación"
        ViewState("IsIndica") = False
        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnEliminar.Click
        LimpiarSesiones()
        ExcluirOtroElementoSeleccion(Me.ddlFondo)
        InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Eliminar"
        lblAccion.Text = "Eliminar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Eliminación"
        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
    End Sub
    Public Sub CargarFechaVencimiento()
        If Session("EstadoPantalla") = "Ingresar" _
            And chkEmisionPrimaria.Checked = True Then
            tbFechaLiquidacion.Text = tbFechaOperacion.Text
        Else
            If (Me.hdPagina.Value <> "DA") Then
                Dim dtAux As DataTable = oPortafolioBM.SeleccionarPortafolioPorFiltro(Me.ddlFondo.SelectedValue, DatosRequest).Tables(0)
                If Not dtAux Is Nothing Then
                    If dtAux.Rows.Count > 0 Then
                        tbFechaOperacion.Text = ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
                    End If
                End If
            Else
                tbFechaOperacion.Text = Request.QueryString("Fecha")
            End If
            If (txtMnemonico.Text.Trim <> "") Then
                tbFechaLiquidacion.Text = oOrdenInversionBM.RetornarFechaVencimiento(ConvertirFechaaDecimal(tbFechaOperacion.Text), Me.txtMnemonico.Text,
                ddlFondo.SelectedValue, ddlIntermediario.SelectedValue)
            End If
        End If
    End Sub
    Private Sub CargarDatosOrdenInversion()
        Try
            oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, DatosRequest,
            PORTAFOLIO_MULTIFONDOS)
            Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
            oRow = oOrdenInversionBE.Tables(0).Rows(0)
            Session("CodigoMoneda") = oRow.CodigoMoneda
            txtISIN.Text = oRow.CodigoISIN
            Session("CodigoNemonico") = oRow.CodigoMnemonico
            txtMnemonico.Text = oRow.CodigoMnemonico
            txtCodigoOrden.Value = oRow.CodigoOrden
            If oRow.CodigoOperacion.ToString <> "" Then
                ddlOperacion.SelectedIndex = ddlOperacion.Items.IndexOf(ddlOperacion.Items.FindByValue(oRow.CodigoOperacion.ToString()))
            Else
                ddlOperacion.SelectedIndex = 0
            End If
            tbFechaOperacion.Text = ConvertirFechaaString(oRow.FechaOperacion)
            tbFechaLiquidacion.Text = ConvertirFechaaString(oRow.FechaLiquidacion)
            txtMnomOrd.Text = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
            txtMnomOp.Text = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
            ddlTipoTasa.SelectedValue = IIf(oRow.CodigoTipoCupon = "3", "", oRow.CodigoTipoCupon)
            txtYTM.Text = Format(oRow.YTM, "##,##0.0000000")
            lblInteresCorrido.Text = Format(oRow.InteresCorrido, "##,##0.0000000")
            txtHoraOperacion.Text = oRow.HoraOperacion
            txtInteresCorNeg.Text = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
            txtMontoOperacional.Text = Format(oRow.MontoOperacion, "##,##0.00")
            ViewState("MontoNeto") = Convert.ToDecimal(txtMontoOperacional.Text)


            txtPrecioLimpio.Text = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
            txtPrecioNegSucio.Text = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
            lblPrecioCal.Text = Format(oRow.PrecioCalculado, "##,##0.0000000")
            txtPrecioNegoc.Text = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")

            txtNroPapeles.Text = Format(oRow.CantidadOperacion, "##,##0.0000000")
            txtObservacion.Text = oRow.Observacion


            hdNumUnidades.Value = txtNroPapeles.Text
            lblPrecioVector.Text = Format(oRow.Precio, "##,##0.0000000")
            txttotalComisionesC.Text = Format(oRow.TotalComisiones, "##,##0.0000000")
            txtMontoNetoOpe.Text = Format(oRow.MontoNetoOperacion, "##,##0.00")
            tbNPoliza.Text = oRow.NumeroPoliza.ToString()
            CargarPlaza()
            ddlPlaza.SelectedValue = oRow.Plaza
            Session("Plaza") = oRow.Plaza
            oTempMontoDestino = oRow.MontoDestino
            oTempPrecioPromedio = oRow.PrecioPromedio
            'Agregado Para Tipo Moneda Destino Diferente
            If oRow.TipoCambio > 0 Or (lblMonedaDestino.Text <> lblMoneda.Text And lblMonedaDestino.Text = "DOL") Then
                Me.tblDestino.Attributes.Add("Style", "Visibility:visible")
                Me.txtMontoOperacionDestino.Text = Format(oRow.MontoDestino, "##,##0.00")
                Me.txtComisionesDestino.Text = Format((oRow.TipoCambio) * (oRow.TotalComisiones), "##,##0.0000000")
                Me.lblMDest.Text = Trim(oRow.CodigoMonedaDestino)
                Me.lblMDest2.Text = Trim(oRow.CodigoMonedaDestino)
            End If
            Me.tbFixing.Text = Format(oRow.Fixing, "##,##0.0000")
            Me.ddlGrupoInt.SelectedValue = oRow.GrupoIntermediario
            Dim dtAux As DataTable
            dtAux = (New TercerosBM().Seleccionar(oRow.CodigoTercero, DatosRequest)).Tables(0)
            If dtAux.Rows.Count > 0 Then
                Me.hdCustodio.Value = dtAux.Rows(0)("CodigoCustodio")
                CargarIntermediario()
                If ddlIntermediario.Items.Count > 1 Then
                    ddlIntermediario.SelectedValue = IIf(oRow.CodigoTercero = "4000", "", oRow.CodigoTercero)
                    CargarContactos()
                    ddlContacto.SelectedValue = oRow.CodigoContacto
                Else
                    AlertaJS(ObtenerMensaje("CONF29"))
                End If
            Else
                AlertaJS(ObtenerMensaje("CONF29"))
            End If
            If oRow.CodigoMotivoCambio <> "" Then
                ddlMotivoCambio.SelectedValue = oRow.CodigoMotivoCambio
            End If
            If oRow.Ficticia = "S" Then
                chkFicticia.Checked = True
            Else
                chkFicticia.Checked = False
            End If
            If (Me.hdPagina.Value = "CO") And oRow.CodigoOperacion.ToString = "39" Then
                ViewState("CodigoTercero") = oRow.CodigoTercero
                ViewState("CodigoTipoCupon") = oRow.CodigoTipoCupon
                ViewState("FechaContrato") = oRow.FechaContrato
                ViewState("CodigoMonedaDestino") = oRow.CodigoMonedaDestino
            End If
            If oRow.EventoFuturo = 1 Then
                chkEmisionPrimaria.Checked = True
            Else
                chkEmisionPrimaria.Checked = False
            End If

            ' INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Tipo Valorizacion
            If oRow.TipoValorizacion.ToString <> "" Then
                ddlTipoValorizacion.SelectedIndex = ddlTipoValorizacion.Items.IndexOf(ddlTipoValorizacion.Items.FindByValue(oRow.TipoValorizacion.ToString()))
            Else
                ddlTipoValorizacion.SelectedIndex = 0
            End If
            ddlTipoValorizacion.Enabled = IIf(ObtenerTipoFondo() = "MANDA", True, False)

            txtTIRNeto.Text = Format(oRow.TirNeta, "##,##0.0000000")
            If oRow.TirNeta = 0 Then txtTIRNeto.Text = Format(oRow.YTM, "##,##0.0000000")
            ' FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Tipo Valorizacion

            txtObservacionCarta.Text = oRow.ObservacionCarta

        Catch ex As Exception
            If Me.ddlFondo.SelectedValue <> codigoMultifondo Then
                AlertaJS(ObtenerMensaje("CONF31"))
            Else
                AlertaJS(ObtenerMensaje("CONF32"))
            End If
        End Try
    End Sub
    Private Function CargarCaracteristicasValor() As Boolean
        Try
            If Me.ddlFondo.SelectedValue = "" Then Return False

            Dim oOIFormulas As New OrdenInversionFormulasBM
            Dim dsValor As DataSet = oOIFormulas.SeleccionarCaracValor_Bonos(Session("CodigoNemonico"), Me.ddlFondo.SelectedValue, DatosRequest)
            Dim drValor As DataRow

            If dsValor.Tables(0).Rows.Count > 0 Then
                If Session("EstadoPantalla") = "Ingresar" Then
                    chkEmisionPrimaria.Enabled = True
                End If

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
                Me.lblBaseCupon.Text = CType(Math.Round(Convert.ToDecimal(drValor("val_BaseCupon")), Constantes.M_INT_NRO_DECIMALES), Integer)
                Me.lblBaseTir.Text = CType(Math.Round(Convert.ToDecimal(drValor("val_BaseTir")), Constantes.M_INT_NRO_DECIMALES), Integer)
                Me.lbldescripcion.Text = CType(drValor("val_Descripcion"), String)
                If (Me.lbldescripcion.Text.Length > 22) Then
                    Me.lbldescripcion.ToolTip = Me.lbldescripcion.Text
                    Me.lbldescripcion.Text = Me.lbldescripcion.Text.Substring(0, 22)
                End If
                Me.lblDuracion.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_Duracion")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblEmisor.Text = CType(drValor("val_Emisor"), String)
                Me.lblnominales.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_NominalesEmitidos")), Constantes.M_INT_NRO_DECIMALES), Decimal),
                "##,##0.0000000")
                Me.lblSaldoValor.Text = CType(drValor("SaldoValor"), String)
                Me.hdSaldo.Value = Me.lblSaldoValor.Text
                Me.lblUnidades.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_NominalesUnitarias")), Constantes.M_INT_NRO_DECIMALES), Decimal),
                "##,##0.0000000")
                Me.lblfecfinbono.Text = CType(drValor("val_FechaFinBono"), String)
                Me.lblFecProxCupon.Text = CType(drValor("val_FechaProxCupon"), String)
                Me.lblFecUltCupon.Text = CType(drValor("val_FechaUltCupon"), String)
                Me.lblPrecioVector.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_VectorPrecio")), Constantes.M_INT_NRO_DECIMALES), Decimal),
                "##,##0.0000000")
                Me.LBLBase.Text = drValor("BaseCalculoDiferenciaDias").ToString()
                If drValor("val_Rescate") = "N" Then
                    Me.lblRescate.Text = "NO"
                ElseIf drValor("val_Rescate") = "S" Then
                    Me.lblRescate.Text = "SI"
                Else
                    Me.lblRescate.Text = ""
                End If

                Me.lblMonedaDestino.Text = CType(drValor("MonedaPago"), String)
                OcultaVisibleFixing()

                '==== INICIO | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 11/07/2018 
                'If Session("EstadoPantalla") = "Ingresar" Or Me.hdPagina.Value = "TI" Then
                '    ValorTasa = CType(drValor("val_CodigoTipoCupon"), String)

                '    If ValorTasa = "1" Or ValorTasa = "2" Then
                '        Me.ddlTipoTasa.SelectedValue = "1"
                '    ElseIf ValorTasa = "4" Or ValorTasa = "5" Then
                '        Me.ddlTipoTasa.SelectedValue = "2"
                '    End If
                'End If

                If Session("EstadoPantalla") = "Ingresar" Or Me.hdPagina.Value = "TI" Then
                    ddlTipoTasa.SelectedValue = "2"
                    ''Si MERCADO (Del Valor) = Extranjero
                    'If drValor("val_Mercado").ToString.Equals("2") Then ddlTipoTasa.SelectedValue = "2"
                End If
                '==== FIN | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 11/07/2018  

                '==== INICIO | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 29/05/2018  
                If dsValor.Tables.Count < 2 Then Throw New Exception("Error: No se ha podido obtener el detalle de cupones del Bono")
                ViewState("DatosValor_DetalleCupones") = dsValor.Tables(1)

                ViewState("DatosValor_TasaCupon") = drValor("TasaCupon")
                ViewState("DatosValor_ValorNominalUnitario") = drValor("val_NominalesUnitarias")
                ViewState("DatosValor_EsCuponADescuento") = drValor("val_CodigoTipoCupon").ToString.Equals("3") ' Es cupón a descuento solo si CodigoTipoCupon = 3 
                ViewState("DatosValor_DiasPeriodicidad") = drValor("DiasPeriodicidad")
                ViewState("DatosValor_EsMercadoExtrangero") = drValor("val_Mercado").ToString.Equals("2") 'Mercado = 2 : Es equivalente a MERCADO EXTRANJERO
                'ViewState("DatosValor_CodigoMoneda") = drValor("val_CodigoMoneda")
                'ViewState("DatosValor_FechaEmision") = drValor("FechaEmision")

                ViewState("DatosValor_BaseCuponMensual") = drValor("BaseCuponMensual")
                ViewState("DatosValor_BaseCuponAnual") = drValor("BaseCuponAnual")

                Dim vacEmision As Decimal = 1, vacEvaluacion As Decimal = 1

                If drValor("val_CodigoMoneda").ToString.Equals("VAC") Then
                    ' Obtencion de valores VAC para los Bonos que aplique
                    UIUtility.ObtenerValoresVAC(drValor("FechaEmision"),
                                      ConvertirFechaaDecimal(Me.tbFechaOperacion.Text),
                                      vacEmision,
                                      vacEvaluacion,
                                      DatosRequest)

                    Me.pnlValoresVAC.Visible = True
                    Me.pnlInteresAjustado.Visible = True
                End If

                Me.txtVACEmision.Text = Format(vacEmision, "##,##0.0000000")
                Me.txtVACActual.Text = Format(vacEvaluacion, "##,##0.0000000")
                '==== FIN | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 29/05/2018

                If Me.hdPagina.Value <> "CO" Then
                    ddlTipoValorizacion.SelectedValue = CType(IIf(ObtenerTipoFondo() = "MANDA", "DIS_VENTA", "VAL_RAZO"), String)
                    ddlTipoValorizacion.Enabled = IIf(ObtenerTipoFondo() = "MANDA", True, False)
                End If
                Return True
            End If
        Catch exInfo As UserInfoException
            AlertaJS(exInfo.Message)
        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF21"))
        End Try

        Return False
    End Function

    Private Sub ObtieneMercadoIntermediario(ByVal pIntermediario As String)
        Dim i As Integer
        Dim dtAux As DataTable
        dtAux = CType(Session("datosEntidad"), DataTable)
        If Not dtAux Is Nothing Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("CodigoTercero") = pIntermediario Then
                    If ddlOperacion.SelectedValue = ObtenerCodigoTipoOperacion("COMPRA") Then
                        Me.hdCustodio.Value = dtAux.Rows(i)("codigoCustodio")
                    End If
                    Session("Mercado") = dtAux.Rows(i)("mercado")
                    Me.dgLista.Dispose()
                    Me.dgLista.DataBind()
                    ObtieneImpuestosComisiones()
                    Exit For
                End If
            Next
        End If
    End Sub
    Private Sub ddlGrupoInt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrupoInt.SelectedIndexChanged
        CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        CargarContactos()
    End Sub
    Private Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos()
        lblSaldoValor.Text = CargarSaldoXTercero(lblSaldoValor.Text, ddlOperacion.SelectedValue, ddlIntermediario.SelectedValue, ddlFondo.SelectedValue,
        txtMnemonico.Text, tbFechaOperacion.Text, DatosRequest)
        SaldoUnidades_EnModificacion(lblSaldoValor, CDec(hdNumUnidades.Value), ddlOperacion.SelectedValue)
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        Session("Plaza") = ddlPlaza.SelectedValue
        ObtieneImpuestosComisiones()

        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
    End Sub
    Private Sub CargarContactos()
        Dim objContacto As New ContactoBM
        Me.ddlContacto.DataSource = Nothing
        ddlContacto.DataBind()
        Me.ddlContacto.DataTextField = "DescripcionContacto"
        Me.ddlContacto.DataValueField = "CodigoContacto"
        Me.ddlContacto.DataSource = objContacto.ListarContactoPorTerceros(Me.ddlIntermediario.SelectedValue)
        Me.ddlContacto.DataBind()
        Me.ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        If txtMnemonico.Text <> "" And ddlIntermediario.SelectedValue <> "" And ddlContacto.Items.Count > 1 Then
            ddlContacto.SelectedValue = objContacto.SeleccionarUltimoContactoEnUnaNegociacion(txtMnemonico.Text, ddlIntermediario.SelectedValue)
        End If
        Dim i As Integer
        Dim dtAux As DataTable
        dtAux = CType(Session("datosEntidad"), DataTable)
        If Not dtAux Is Nothing Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("CodigoTercero") = ddlIntermediario.SelectedValue Then
                    If ddlOperacion.SelectedValue = ObtenerCodigoTipoOperacion("COMPRA") Then
                        Me.hdCustodio.Value = dtAux.Rows(i)("codigoCustodio")
                    End If
                    Exit For
                End If
            Next
        End If
    End Sub
    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnProcesar.Click
        Try
            If ddlModoNegociacion.SelectedValue = "" Then
                AlertaJS("Seleccione un modo de negociación.")
                Exit Sub
            End If
            If ddlTipoTasa.SelectedValue = "" Then
                AlertaJS("Seleccione un tipo de tasa por favor.")
                Exit Sub
            End If
       
            txtInteresCorNeg.Text = IIf(txtInteresCorNeg.Text.Trim.Length = 0, 0, txtInteresCorNeg.Text)
            txtYTM.Text = IIf(txtYTM.Text.Trim.Length = 0, 0, txtYTM.Text)
            Dim GUID As String = System.Guid.NewGuid.ToString()
            Dim feriado As New FeriadoBM
            If ValidarHora(Me.txtHoraOperacion.Text) = False Then
                AlertaJS(ObtenerMensaje("CONF22"))
                Exit Sub
            End If
            'If txtMnomOp.Text = "" Or CInt(txtMnomOp.Text) = 0 Then
            '    AlertaJS("El Monto Nominal Operación no puede ser 0 o estar vacio.")
            '    Exit Sub
            'End If
            If (feriado.VerificaDia(ConvertirFechaaDecimal(tbFechaLiquidacion.Text), Session("Mercado")) = False) Then
                AlertaJS("Fecha de Vencimiento no es valida.")
                Exit Sub
            End If
            Session("Procesar") = 1
            If hdPagina.Value = "TI" Or hdPagina.Value = "MODIFICA" Then
                ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
            End If

            txtMnomOrd.Text = Format(Convert.ToDecimal(txtNroPapeles.Text) * CDec(ViewState("DatosValor_ValorNominalUnitario")), "##,##0.0000000")
            txtMnomOp.Text = txtMnomOrd.Text
            If ObtieneCustodiosSaldos() = False Then
                AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                Exit Sub
            End If
            'OT12127 | 2019-07-16 | rcolonia | Zoluxiones | Agregar Negociación TBILL
            Dim dsValor As DataSet = New PrevOrdenInversionBM().SeleccionarCaracValor(txtMnemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
            If dsValor.Tables.Count < 2 Then Throw New Exception("Error: No se ha podido obtener el detalle de cupones del Bono")

            Dim dtCaracValor As DataTable = dsValor.Tables(0)
            Dim rowValor As DataRow = dtCaracValor.Rows(0)

            Dim esCalculoTBill As Boolean = rowValor("CodigoTipoInstrumentoSBS").ToString.Equals("100")

            '==== INICIO | PROYECTO FONDOS-II: RF007 | ZOLUXIONES | CRumiche | 29/05/2018
            Dim vacEmision As Decimal = CDec(Me.txtVACEmision.Text)
            Dim vacEvaluacion As Decimal = CDec(Me.txtVACActual.Text)

            Dim baseAnual As BaseAnualCupon = UIUtility.ObtenerBaseAnualDesdeTexto(ViewState("DatosValor_BaseCuponAnual"))
            Dim baseMensual As BaseMensualCupon = UIUtility.ObtenerBaseMensualDesdeTexto(ViewState("DatosValor_BaseCuponMensual"))
            Dim aplicacionTasa As TipoAplicacionTasa = UIUtility.ObtenerTipoAplicacionTasaDesdeCodTipoTasa(ddlTipoTasa.SelectedValue)

            'Pasamos a calcular la negociacion
            Dim neg As NegociacionRentaFija = UIUtility.CalcularNegociacionRentaFija(ViewState("DatosValor_DetalleCupones"),
                                                                CDec(ViewState("DatosValor_TasaCupon")),
                                                                CInt(ViewState("DatosValor_DiasPeriodicidad")),
                                                                CDec(ViewState("DatosValor_ValorNominalUnitario")),
                                                                Convert.ToDecimal(txtNroPapeles.Text),
                                                                Convert.ToDateTime(tbFechaLiquidacion.Text),
                                                                Convert.ToDecimal(txtYTM.Text),
                                                                baseMensual,
                                                                baseAnual,
                                                                CBool(ViewState("DatosValor_EsCuponADescuento")),
                                                                CBool(ViewState("DatosValor_EsMercadoExtrangero")),
                                                                aplicacionTasa,
                                                                vacEmision,
                                                                vacEvaluacion,
                                                                esCalculoTBill)
            'Mostramos los resultados
            txtMontoOperacional.Text = Format(neg.ValorActual, "##,##0.00")

            txtPrecioLimpio.Text = Format(neg.PrecioLimpio * 100, "##,##0.0000000") '%: Es un Porcentaje
            txtPrecioNegSucio.Text = Format(neg.PrecioSucio * 100, "##,##0.0000000") '%: Es un Porcentaje
            lblPrecioCal.Text = Format(neg.PrecioSucio * 100, "##,##0.0000000") '%: Es un Porcentaje
            txtPrecioNegoc.Text = Format(neg.PrecioSucio * 100, "##,##0.0000000") '%: Es un Porcentaje

            lblInteresCorrido.Text = Format(neg.InteresCorrido, "##,##0.0000")
            txtInteresCorNeg.Text = Format(neg.InteresCorrido, "##,##0.0000")
            txtInteresAjustado.Text = Format(neg.InteresCorridoAjustado, "##,##0.0000")

            txtMontoNominalVigente.Text = "0.0000"
            If neg.CuponVigente IsNot Nothing Then txtMontoNominalVigente.Text = Format(neg.CuponVigente.SaldoNominalInicial, "##,##0.0000")

            Session("NegociacionRentaFija") = neg

            '==== FIN | PROYECTO FONDOS-II: RF007 | ZOLUXIONES | CRumiche | 29/05/2018

            'LIMITES
            Dim bolValidaLimites As Boolean = True
            If chkEmisionPrimaria.Checked = True Then
                bolValidaLimites = False
            End If
            'If bolValidaLimites = True Then OrdenInversion.CalculaLimitesOnLine(Me, DatosRequest, ViewState("estadoOI"), ViewState("GUID_Limites"))
            Dim ds As DataSet
            Dim Mensaje As String
            ds = New OrdenPreOrdenInversionBM().ValidacionPuntual_LimitesTrading(txtMnemonico.Text, ConvertirFechaaDecimal(tbFechaOperacion.Text),
            ddlFondo.SelectedValue, Convert.ToDecimal(txtNroPapeles.Text.Replace(",", "").Replace(".", DecimalSeparator)), Session("CodigoMoneda"),
            Usuario.ToString.Trim, String.Empty)
            If ds.Tables(0).Rows.Count > 0 Then
                Mensaje = "El usuario no esta permitido grabar la operación, el monto de negociado excede el límite de autonomía por Trader:\n\n"
                For Each fila As DataRow In ds.Tables(0).Rows
                    Mensaje = Mensaje & "- Usuario (" & fila("TipoCargoExc") & ") excedió límite de autonomía \""" & fila("GrupoLimTrd") &
                    "\"", debe ser autorizado por un usuario " & fila("TipoCargoAut") & " (" & fila("TraderAut") & "). \n\n"
                Next
                Mensaje = Mensaje & "La operación debe ser grabada por el usuario autorizado haciendo clic en el botón Aceptar de la orden de inversión."
                AlertaJS(Mensaje)
                Session("dtValTrading") = ds.Tables(0)
            End If
            If dgLista.Rows.Count > 0 Then
                txtComisionSAB = CType(dgLista.Rows(0).FindControl("txtValorComision2"), TextBox)
                If Session("Plaza").ToString = "7" And Session("TipoRenta").ToString = "1" And dgLista.Rows.Count > 2 And txtComisionSAB.Text.Trim <> String.Empty And chkRecalcular.Checked Then
                    recalcularComision(Decimal.Parse(txtComisionSAB.Text), dgLista)
                Else
                    CalcularComisiones()
                End If
            Else
                CalcularComisiones()
            End If

            '==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-21 | Calculo del TIR Neto
            Me.txtTIRNeto.Text = Me.txtYTM.Text
            If Me.pnlTirNeto.Visible Then
                Dim negCopia As NegociacionRentaFija = neg.Clone() 'CRumiche: Copia de la Negociación para que no afecte la original
                If IsNumeric(Me.txtMontoNetoOpe.Text.Replace(",", "")) Then
                    If Math.Round(Convert.ToDecimal(Me.txtMontoNetoOpe.Text.Replace(",", "")), 2) <> Math.Round(negCopia.ValorActual, 2) Then
                        negCopia.PrecioSucio = Convert.ToDecimal(Me.txtMontoNetoOpe.Text.Replace(",", "")) / negCopia.CuponVigente.SaldoNominalInicial ' Se determina el nuevo Precio Sucio
                        negCopia.CalcularDatosDelFlujoDeCuponesBasadoEnPrecioSucio()
                    End If
                End If
                Me.txtTIRNeto.Text = Format(negCopia.YTM * 100, "##,##0.0000000") '%: Es un Porcentaje 
            End If
            '==== FIN | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-21 | Calculo del TIR Neto

            CargarPaginaProcesar()
            ObtenerValorFixing()
            'DeshabilitarModoNegociacion()
  
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub



    Private Sub ObtenerValorFixing()
        Dim decFixingAux As Decimal
        If tbFixing.Text = "" Then
            decFixingAux = 1
        Else
            If Convert.ToDecimal(tbFixing.Text) = 0 Then
                decFixingAux = 1
            Else
                decFixingAux = Convert.ToDecimal(tbFixing.Text)
            End If
        End If
        Me.tblDestino.Attributes.Add("Style", "Visibility:visible")
        Me.txtMontoOperacionDestino.Text = Format(Convert.ToDecimal(IIf(txtMontoOperacional.Text = "", 0, txtMontoOperacional.Text)) / decFixingAux, "###,###,##0.00")
        Me.txtComisionesDestino.Text = "0.0000000"
        Me.lblMDest.Text = "DOL"
        Me.lblMDest2.Text = "DOL"
    End Sub
    Private Function ObtieneCustodiosSaldos() As Boolean
        Dim decAux As Decimal
        Dim strCodigoOperacion As String = String.Empty
        If Session("EstadoPantalla") = "Ingresar" Or Session("EstadoPantalla") = "Modificar" Then
            If VerificarSaldosCustodios(decAux) = False Then
                'Interpretar Codigos de Operacion (Según Tipo de Negociacion)
                '------------------------------------------------------------------------------
                If Me.hdPagina.Value = "TI" Then
                    Select Case ddlOperacion.SelectedValue
                        Case ObtenerCodigoOperacionTIngreso() : strCodigoOperacion = ObtenerCodigoOperacionCompra()
                        Case ObtenerCodigoOperacionTEgreso() : strCodigoOperacion = ObtenerCodigoOperacionVenta()
                    End Select
                Else
                    strCodigoOperacion = ddlOperacion.SelectedValue.ToString()
                End If
                If strCodigoOperacion = ObtenerCodigoTipoOperacion("VENTA") Then
                    AlertaJS(ObtenerMensaje("CONF28"))
                    Return False
                Else
                    Return False
                End If
            End If
        End If
        Return True
    End Function
    Private Function VerificarSaldosCustodios(ByRef decAux As Decimal) As Boolean
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        Dim decMontoAux As Decimal = 0.0
        Dim cantCustodios As Integer = 0
        Dim oPrevOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim dtSumaUnidades As DataTable

        Try
            Dim decUnidades As Decimal
            If Me.lblUnidades.Text = "0" Then
                decUnidades = 1
            Else
                decUnidades = Convert.ToDecimal(Me.lblUnidades.Text.Replace(".", DecimalSeparator))
            End If
            Dim decValorLocal As Decimal = Convert.ToDecimal(Me.txtMnomOp.Text.Replace(".", DecimalSeparator)) / decUnidades
            decAux = Math.Round(decValorLocal, Constantes.M_INT_NRO_DECIMALES)
           
            If lblSaldoValor.Text = Convert.ToString(decAux).Replace(DecimalSeparator, ".") Then
                Return True
            End If
            '********COMPRA*********
            If Me.ddlOperacion.SelectedValue = ObtenerCodigoTipoOperacion("COMPRA") Then
                'Se comento para poder negociar con cualquier intermediario
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Convert.ToString(decAux).Replace(DecimalSeparator, ".")
                hdNumUnidades.Value = Convert.ToString(decAux).Replace(DecimalSeparator, ".")
                Return True
            End If
            '********VENTA*********
            If Session("ValorCustodio") Is Nothing Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
            ElseIf Session("ValorCustodio") = "" Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
            End If
            dtSumaUnidades = oPrevOrdenInversionBM.ObtenerUnidadesNegociadasDiaT(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text).Tables(0)
            If dtSumaUnidades.Rows.Count > 0 Then
                decAux += Decimal.Parse(dtSumaUnidades.Compute("Sum(UNIDADES)", String.Empty))
                If Me.hdPagina.Value = "CO" Then
                    decAux -= CDec(hdNumUnidades.Value)
                End If
            End If
            decMontoAux = Convert.ToDecimal(Me.lblSaldoValor.Text)
            If decMontoAux = decAux Then
                hdNumUnidades.Value = Convert.ToString(decAux).Replace(DecimalSeparator, ".")
                Return True
            ElseIf decMontoAux > decAux Then
                'redefinir calculos porq excede
                If cantCustodios = 1 Then
                    'porque solamente es el primer custodio
                    Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Convert.ToString(decAux).Replace(DecimalSeparator, ".")
                    hdNumUnidades.Value = Convert.ToString(decAux).Replace(DecimalSeparator, ".")
                    Return True
                Else
                    'porq hay mas de un custodio. ajustar montos
                    Session("ValorCustodio") = AjustarMontosCustodios(CType(Session("ValorCustodio"), String), Convert.ToString(decAux).Replace(DecimalSeparator, "."))
                    hdNumUnidades.Value = Convert.ToString(decAux).Replace(DecimalSeparator, ".")
                    Return True
                End If
                Return False
            ElseIf decMontoAux < decAux Then
                'redefinir calculos porq falta
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub CalcularComisiones()
        Dim dblTotalComisiones As Decimal = 0.0
        If Not lbldescripcion.Text.ToUpper = "BONOS GOBIERNO CENTRAL" Then
            If chkRecalcular.Checked Then
                'OT11008 - 22/01/2018 - Carlos Rumiche.
                'Descripción: Corrección cálculo de comisiones
                'dblTotalComisiones = CalculaImpuestosComisiones(dgLista, Session("Mercado"), Me.txtMontoOperacional.Text.Replace(",", ""),
                'Me.txtNroPapeles.Text.Replace(",", ""), ddlGrupoInt.SelectedValue, ddlOperacion.SelectedValue, CLASE_INSTRUMENTO_BONO)
                '     If txtCodigoOrden.Value.Trim = String.Empty Then
                'txtCodigoOrden.Value = "0"
                dblTotalComisiones = CalcularComisionesYLlenarGridView(dgLista, String.Empty, txtMontoOperacional.Text.Replace(",", ""), ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text, ddlIntermediario.SelectedValue)
                'Else
                '    dblTotalComisiones = CalcularComisionesYLlenarGridView(dgLista, txtCodigoOrden.Value, txtMontoOperacional.Text.Replace(",", ""))
                ' End If
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF011 - Se resalta Comisión SAB a Cero para que pueda ser editado | 07/08/18 
                If dgLista.Rows.Count > 0 Then validarComisionSAB(dblTotalComisiones)
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF011 - Se resalta  Comisión SAB a Cero para que pueda ser editado | 07/08/18 
            Else
                dblTotalComisiones = UIUtility.CalculaImpuestosComisionesNoRecalculo(dgLista, Session("Mercado"), txtMontoOperacional.Text.Replace(",", ""),
                txtNroPapeles.Text.Replace(",", ""), ddlGrupoInt.SelectedValue, ddlOperacion.SelectedValue, CLASE_INSTRUMENTO_ACCION)

            End If
        End If
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        If (ddlOperacion.SelectedValue = "2") Then
            txtMontoNetoOpe.Text = Format(txtMontoOperacional.Text.Replace(",", "") - dblTotalComisiones, "##,##0.00")
        Else
            txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoOperacional.Text.Replace(",", ""), "##,##0.00")
        End If
    End Sub
    Private Sub actualizaMontos()
        Dim dblTotalComisiones As Decimal = 0.0
        If txttotalComisionesC.Text = "" Then txttotalComisionesC.Text = "0.0000000"
        If txtMontoNetoOpe.Text = "" Then txtMontoNetoOpe.Text = "0.00"
        'se realiza esta validacion, asi como se realiza en el Procesar, en caso se Calcule o NO las comisiones!!!
        If oValoresBM.VerificarCalculoComisiones(Me.txtMnemonico.Text(), Me.ddlIntermediario.SelectedValue, ConvertirFechaaDecimal(tbFechaOperacion.Text),
        Me.ddlOperacion.SelectedValue) <> 0 Then Exit Sub
        dblTotalComisiones = ActualizaMontosFinales(dgLista)
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - De acuerdo al tipo de operación se restará o sumará al valor total | 27/06/18 
        If (ddlOperacion.SelectedValue = "2") Then dblTotalComisiones = dblTotalComisiones * -1
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - De acuerdo al tipo de operación se restará o sumará al valor total | 27/06/18 

        If Session("Mercado") = 1 Then  'local
            txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoOperacional.Text.Replace(",", ""), "##,##0.00")
        ElseIf Session("Mercado") = 2 Then  'extranjero
            txtMontoNetoOpe.Text = Format(dblTotalComisiones + Me.txtNroPapeles.Text.Replace(",", ""), "##,##0.00")
        End If
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        Dim oFormulasOI As New OrdenInversionFormulasBM
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        Dim accionRpta As String = String.Empty
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
        Try
            If hdRptaConfirmar.Value.ToUpper = "SI" Then
                EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
                ViewState("Treasury") = oFormulasOI.ValidarTresuryADescuento(txtMnemonico.Text)
                If ObtieneCustodiosSaldos() = False Then
                    AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                    Exit Sub
                End If
                If Me.hdPagina.Value <> "" And Me.hdPagina.Value <> "DA" And Me.hdPagina.Value <> "TI" And Me.hdPagina.Value <> "MODIFICA" Then
                    If Me.hdPagina.Value = "EO" Or Me.hdPagina.Value = "CO" Then
                        ModificarOrdenInversion()
                        InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlFondo.SelectedValue.Trim,
                        DatosRequest, ddlPlaza.SelectedValue)
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
                                ActualizaDatosCarta()
                                ReturnArgumentShowDialogPopup()
                            End If
                        End If
                    End If
                Else
                    If hdPagina.Value = "" Or hdPagina.Value = "TI" Or hdPagina.Value = "DA" Or hdPagina.Value = "MODIFICA" Then
                        If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
                            Dim strAlerta As String = ""
                            If ddlMotivoCambio.SelectedIndex <= 0 Then
                                strAlerta = "-Elija el motivo por el cual desea " & Session("EstadoPantalla") & " esta operación.<br>"
                            End If
                            If txtComentarios.Text.Trim.Length <= 0 Then
                                strAlerta += "-Ingrese los comentarios por el cual desea " & Session("EstadoPantalla") & " esta operación."
                            End If
                            If strAlerta.Length > 0 Then
                                AlertaJS(strAlerta)
                                Exit Sub
                            End If
                        End If
                        If ObtieneCustodiosSaldos() = False Then
                            AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                            Exit Sub
                        End If
                        If Session("EstadoPantalla") = "Ingresar" Then
                            If ConvertirFechaaDecimal(tbFechaOperacion.Text) <= ConvertirFechaaDecimal(objutil.RetornarFechaNegocio) _
                                And chkEmisionPrimaria.Checked = True Then
                                AlertaJS("La fecha de operacion debe ser mayor que la fecha de apertura IDI")
                                Exit Sub
                            End If
                            actualizaMontos()

                            If Session("NegociacionRentaFija") IsNot Nothing Then
                                If Session("Procesar") = 1 Then
                                    'Dim strcodigoOrden As String
                                    'strcodigoOrden = InsertarOrdenInversion()
                                    'If strcodigoOrden <> "" Then
                                    '    If ddlFondo.SelectedValue <> codigoMultifondo Then
                                    '        Dim toUser As String = ""
                                    '        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                                    '        Dim dt As DataTable

                                    '        If ViewState("estadoOI") = "E-EXC" Then
                                    '            dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                                    '            For Each fila As DataRow In dt.Rows
                                    '                toUser = toUser + fila("Valor").ToString() & ";"
                                    '            Next
                                    '            Try
                                    '                EnviarMail(toUser, "", "Pendiente de aprobación - Orden excedido por Limites de Inversión",
                                    '                OrdenInversion.MensajeExcesosOI(strcodigoOrden, ddlFondo.SelectedValue.ToString(), DatosRequest), DatosRequest)
                                    '            Catch ex As Exception
                                    '                AlertaJS("Se ha generado un error en el proceso de envio de notificación!")
                                    '            End Try

                                    '        End If
                                    '        Session("dtValTrading") = ""
                                    '    End If
                                    'End If
                                    'oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                                    'If ViewState("Treasury") = "T" And ObtenerFechaNegocio(Me.ddlFondo.SelectedValue) = ConvertirFechaaDecimal(Me.tbFechaOperacion.Text) Then
                                    '    oOrdenInversionWorkFlowBM.EnviarCXPCDPH(strcodigoOrden, Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                                    'End If
                                    'If Me.hdPagina.Value <> "TI" And chkFicticia.Checked = False Then
                                    '    InsertarModificarImpuestosComisiones("I", dgLista, strcodigoOrden, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlFondo.SelectedValue.Trim,
                                    '    DatosRequest, ddlPlaza.SelectedValue)
                                    'End If
                                    'Session("dtdatosoperacion") = ObtenerDatosOperacion()
                                    'GenerarLlamado(strcodigoOrden + ViewState("CodigoOrden_T"), Me.ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, "BONOS",
                                    'ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text.Trim)
                                    'Me.txtCodigoOrden.Value = strcodigoOrden

                                    'INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
                                    GuardarPreOrden()
                                    accionRpta = "Ingresó" 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18                                     
                                    CargarPaginaInicio()
                                    'FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
                                End If
                            Else
                                AlertaJS("De click en el botón Procesar para generar los datos de la negociación")
                            End If
                        Else
                            If Session("EstadoPantalla") = "Modificar" Then
                                actualizaMontos()
                                InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlFondo.SelectedValue.Trim,
                                DatosRequest, ddlPlaza.SelectedValue)
                                ModificarOrdenInversion()
                                FechaEliminarModificarOI("M")
                                CargarPaginaAceptar()
                                Session("dtdatosoperacion") = ObtenerDatosOperacion()
                                If Me.hdPagina.Value <> "MODIFICA" Then
                                    GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, "BONOS", Me.ddlOperacion.SelectedItem.Text,
                                    Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text)
                                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 12/06/18 
                                    accionRpta = "Modificó"
                                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 12/06/18 
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
                                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Eliminar | 12/06/18 
                                accionRpta = "Eliminó"
                                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Eliminar | 12/06/18 
                            End If
                        End If
                        If Session("Procesar") = 0 And (Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Ingresar") Then
                            If CType(ViewState("MontoNeto"), String) = "" Then
                                AlertaJS(ObtenerMensaje("CONF9"))
                            Else
                                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                                retornarMensajeAccion(accionRpta)
                                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                            End If
                        Else
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                            If (Session("EstadoPantalla") = "Eliminar" Or (Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Ingresar")) Then retornarMensajeAccion(accionRpta)
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                        End If
                        EliminarCuponerasOITemporales()
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub


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
    Public Function ObtenerDatosOperacion() As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11",
        "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = Me.tbFechaOperacion.Text
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = Me.tbFechaLiquidacion.Text
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = Me.txtHoraOperacion.Text
        drGrilla("c4") = "Monto Nominal Ordenado"
        drGrilla("v4") = Me.txtMnomOrd.Text
        drGrilla("c5") = "Monto Nominal Operación"
        drGrilla("v5") = Me.txtMnomOp.Text
        drGrilla("c6") = "Tipo Tasa"
        drGrilla("v6") = Me.ddlTipoTasa.SelectedItem.Text
        drGrilla("c7") = "YTM%"
        drGrilla("v7") = Me.txtYTM.Text
        drGrilla("c8") = "Precio Negociación %"
        drGrilla("v8") = Me.txtPrecioNegoc.Text
        drGrilla("c9") = "Precio Calculado %"
        drGrilla("v9") = Me.lblPrecioCal.Text
        drGrilla("c10") = "Precio Negociación Sucio"
        drGrilla("v10") = Me.txtPrecioNegSucio.Text
        drGrilla("c11") = "Interés Corrido Negociado"
        drGrilla("v11") = Me.txtInteresCorNeg.Text
        drGrilla("c12") = "Interés Corrido"
        drGrilla("v12") = Me.lblInteresCorrido.Text
        drGrilla("c13") = "Monto Operación"
        drGrilla("v13") = Me.txtMontoOperacional.Text
        drGrilla("c14") = "Número Papeles"
        drGrilla("v14") = Me.txtNroPapeles.Text
        drGrilla("c15") = "Intermediario"
        drGrilla("v15") = Me.ddlIntermediario.SelectedItem.Text
        If Me.ddlContacto.SelectedIndex <> 0 Then
            drGrilla("c16") = "Contacto"
            drGrilla("v16") = Me.ddlContacto.SelectedItem.Text
        Else
            drGrilla("c16") = ""
            drGrilla("v16") = ""
        End If
        If Me.tbNPoliza.Visible = True Then
            drGrilla("c17") = "Número Poliza"
            drGrilla("v17") = Me.tbNPoliza.Text
        Else
            drGrilla("c17") = ""
            drGrilla("v17") = ""
        End If
        drGrilla("c18") = "Observación"
        drGrilla("v18") = Me.txtObservacion.Text.ToUpper
        drGrilla("c19") = "Total Comisiones"
        drGrilla("v19") = Me.txttotalComisionesC.Text
        drGrilla("c20") = "Monto Neto Operación"
        drGrilla("v20") = Me.txtMontoNetoOpe.Text
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Function InsertarOrdenInversion() As String
        Dim custodios As String()
        Dim strCodigoOI, strCodigoOI_T As String
        Dim importe As String
        Dim cantidad As Decimal
        cantidad = Convert.ToDecimal(txtMnomOp.Text.Replace(",", "")) / Convert.ToDecimal(lblUnidades.Text.Replace(",", ""))
        custodios = CType(Session("ValorCustodio"), String).Split("&")
        importe = Me.hdCustodio.Value + "&" + CType(cantidad, String)
        oOrdenInversionBE = crearObjetoOI()
        strCodigoOI = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, importe, DatosRequest)
        If Me.hdPagina.Value = "TI" Then
            oOrdenInversionBE.OrdenPreOrdenInversion.Rows(0)("CodigoPortafolioSBS") = Me.ddlFondoDestino.SelectedValue
            oOrdenInversionBE.OrdenPreOrdenInversion.Rows(0)("CodigoOperacion") = ObtenerCodigoOperacionTIngreso().ToString()
            oOrdenInversionBE.AcceptChanges()
            Session("ValorCustodio") = ObtieneUnCustodio(Session("ValorCustodio"))
            strCodigoOI_T = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
            ViewState("CodigoOrden_T") = "-" + strCodigoOI_T
        Else
            ViewState("CodigoOrden_T") = ""
        End If
        Return strCodigoOI
    End Function
    Public Sub ModificarOrdenInversion()
        Dim custodios As String()
        Dim importe As String
        Dim cantidad As Decimal
        cantidad = Convert.ToDecimal(txtMnomOp.Text.Replace(",", "")) / Convert.ToDecimal(lblUnidades.Text.Replace(",", ""))
        custodios = CType(Session("ValorCustodio"), String).Split("&")
        importe = custodios(0) + "&" + CType(cantidad, String)
        oOrdenInversionBE = crearObjetoOI()
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, Me.hdPagina.Value, importe, DatosRequest)
    End Sub
    Public Sub EliminarOrdenInversion()
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

        oRow.CodigoOrden = Me.txtCodigoOrden.Value
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.CodigoMoneda = Session("CodigoMoneda")
        oRow.CodigoISIN = txtISIN.Text
        oRow.CodigoMnemonico = txtMnemonico.Text
        oRow.CodigoSBS = Me.txtSBS.Text
        oRow.FechaOperacion = ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaIDI = ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaLiquidacion = ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
        oRow.MontoNominalOrdenado = Math.Round(Convert.ToDecimal(Me.txtMnomOrd.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.MontoNominalOperacion = Math.Round(Convert.ToDecimal(Me.txtMnomOp.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.CodigoTipoCupon = Me.ddlTipoTasa.SelectedValue
        oRow.YTM = Math.Round(Convert.ToDecimal(Me.txtYTM.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.InteresCorrido = Math.Round(Convert.ToDecimal(Me.lblInteresCorrido.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)

        oRow.PrecioNegociacionLimpio = Math.Round(Convert.ToDecimal(Me.txtPrecioLimpio.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.PrecioNegociacionSucio = Math.Round(Convert.ToDecimal(Me.txtPrecioNegSucio.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.PrecioCalculado = Math.Round(Convert.ToDecimal(Me.lblPrecioCal.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)

        oRow.Situacion = "A"
        oRow.InteresCorridoNegociacion = Math.Round(Convert.ToDecimal(Me.txtInteresCorNeg.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.MontoOperacion = Math.Round(Convert.ToDecimal(Me.txtMontoOperacional.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.CantidadOperacion = Math.Round(Convert.ToDecimal(Me.txtNroPapeles.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.Observacion = Me.txtObservacion.Text.ToUpper


        oRow.HoraOperacion = txtHoraOperacion.Text
        oRow.Precio = lblPrecioVector.Text.Replace(",", "")
        oRow.TotalComisiones = txttotalComisionesC.Text.Replace(",", "")
        oRow.MontoNetoOperacion = txtMontoNetoOpe.Text.Replace(",", "")
        oRow.CategoriaInstrumento = "BO"    'UNICO POR TIPO
        oRow.Plaza = ddlPlaza.SelectedValue
        If Not ViewState("estadoOI") Is Nothing Then
            If ViewState("estadoOI").Equals("E-EXC") Then
                oRow.Estado = ViewState("estadoOI")
            End If
        End If
        If (Me.hdPagina.Value <> "XO") Then
            oRow.NumeroPoliza = Me.tbNPoliza.Text.ToString().Trim
        End If
        If txtMontoOperacionDestino.Text.Trim.Equals("") Then
            oRow.MontoDestino = 0
        Else
            oRow.MontoDestino = txtMontoOperacionDestino.Text.Replace(",", "")
        End If
        oRow.Fixing = IIf(tbFixing.Text = "", 0, tbFixing.Text.Replace(",", ""))
        oRow.GrupoIntermediario = ddlGrupoInt.SelectedValue
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
        If (ViewState("Treasury") = "T") And ViewState("CodigoTercero") <> "" Then
            oRow.CodigoTercero = ViewState("CodigoTercero")
            oRow.CodigoTipoCupon = ViewState("CodigoTipoCupon")
            oRow.FechaContrato = ViewState("FechaContrato")
            oRow.CodigoMonedaDestino = ViewState("CodigoMonedaDestino")
        End If
        If (chkRegulaSBS.Checked) Then
            oRow.RegulaSBS = "S"
        Else
            oRow.RegulaSBS = "N"
        End If
        If Session("EstadoPantalla") = "Ingresar" Then
            If ConvertirFechaaDecimal(tbFechaOperacion.Text) > ConvertirFechaaDecimal(objutil.RetornarFechaNegocio) _
            And chkEmisionPrimaria.Checked = True Then
                oRow.EventoFuturo = 1
            End If
        Else
            If chkEmisionPrimaria.Checked = True Then
                oRow.EventoFuturo = 1
            End If
        End If

        ' INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Tipo Valorizacion
        oRow.TipoValorizacion = ddlTipoValorizacion.SelectedValue
        oRow.TirNeta = Math.Round(Convert.ToDecimal(Me.txtTIRNeto.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        ' FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Tipo Valorizacion

        oRow.ObservacionCarta = Me.txtObservacionCarta.Text.ToUpper

        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
    Private Function ValidarFechas() As Boolean
        Dim dsFechas As PortafolioBE
        Dim drFechas As DataRow
        Dim blnResultado As Boolean = True
        If ConvertirFechaaDecimal(Me.tbFechaOperacion.Text) > ConvertirFechaaDecimal(Me.tbFechaLiquidacion.Text) Then
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
            Dim dblFechaOperacion As Decimal = ConvertirFechaaDecimal(Me.tbFechaOperacion.Text)
            Dim dblFechaConstitucion As Decimal = CType(drFechas("FechaConstitucion"), Decimal)
            Dim dblFechaTermino As Decimal = CType(drFechas("FechaTermino"), Decimal)
            If dblFechaConstitucion > dblFechaOperacion Then
                blnResultado = False
                AlertaJS(ObtenerMensaje("CONF4"))
            End If
        End If
        If (objferiadoBM.BuscarPorFecha(ConvertirFechaaDecimal(tbFechaOperacion.Text))) = True Then
            blnResultado = False
            AlertaJS(ObtenerMensaje("CONF8"))
        End If
        Return blnResultado
    End Function
    Private Sub ConfiguraModoConsulta()
        ExcluirOtroElementoSeleccion(Me.ddlFondo)
        InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Consulta"
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnConsultar.Click
        LimpiarSesiones()
        Call ConfiguraModoConsulta()
    End Sub
    Private Sub btnCaracteristicas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCaracteristicas.Click
        If Me.txtMnemonico.Text <> "" Then
            Session("accionValor") = "MODIFICAR"
            EjecutarJS(UIUtility.MostrarPopUp("../../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" + Me.txtMnemonico.Text + "&vOI=T", "10",
            1024, 600, 0, 0, "No", "No", "Yes", "Yes"), False)
        Else
            AlertaJS(ObtenerMensaje("CONF23"))
        End If
    End Sub
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal codPortafolio As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String,
    ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&cportafolio=" + codPortafolio + "&vportafolio=" + portafolio +
        "&vdescripcionPortafolio=" + portafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs +
        "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "No", "Yes", "Yes"), False)
    End Sub
#Region " /* Métodos Personalizados (Popups Dialogs) */ "
    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal cfondo As String,
    ByVal operacion As String, ByVal categoria As String, ByVal valor As String)
        Dim strURL As String = "frmBuscarValor.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&cFondo=" & cfondo & "&vFondo=" & fondo &
        "&vOperacion=" & operacion & "&vCategoria=" & categoria
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='" & valor & "';")
    End Sub
    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal cfondo As String,
    ByVal operacion As String, ByVal moneda As String, ByVal fecha As String, ByVal accion As String, ByVal valor As String)
        Dim strURL As String = "../frmInversionesRealizadas.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&cFondo=" & cfondo & "&vFondo=" & fondo &
        "&vOperacion=" & operacion & "&vFechaOperacion=" & fecha & "&vAccion=" & accion & "&vCategoria=BO"
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='" & valor & "';")
    End Sub
    Private Sub ShowDialogCuponera(ByVal mnemonico As String, Optional ByVal IsProcesar As Boolean = True)
        Dim strGuid As String = String.Empty
        Dim strCodigoOrden As String = String.Empty
        Dim strCodigoPortafolioSBS As String = String.Empty
        IsProcesar = CType(ViewState("IsIndica"), Boolean)
        If IsProcesar Then
            strGuid = ViewState("CuponeraTemporalGUID").ToString()
            strCodigoOrden = "0"
            strCodigoPortafolioSBS = "0"
        Else
            strGuid = "0"
            strCodigoOrden = Me.txtCodigoOrden.Value
            strCodigoPortafolioSBS = ddlFondo.SelectedValue
        End If
        Dim strCodigoMnemonico As String = mnemonico
        Dim strInteresCorrido As String = Convert.ToDecimal(Me.txtInteresCorNeg.Text.Replace(",", "")).ToString()
        Dim strMontoOperacion As String = Convert.ToDecimal(Me.txtMontoOperacional.Text.Replace(",", "")).ToString()
        Dim strPrecioCalculado As String = Convert.ToDecimal(Me.lblPrecioCal.Text.Replace(",", "")).ToString()
        Dim strFechaOperacion As String = ConvertirFechaaDecimal(Me.tbFechaOperacion.Text)
        Dim script As New StringBuilder
        With script
            .Append("window.showModalDialog('frmConsultaCuponeras.aspx?CodigoMnemonico=" + mnemonico + "&guid=" + strGuid + "&codigoOrden=" + strCodigoOrden +
            "&CodigoPortafolioSBS=" + strCodigoPortafolioSBS + "&InteresCorrido=" + strInteresCorrido + "&MontoOperacion=" + strMontoOperacion +
            "&PrecioCalculado=" + strPrecioCalculado + "&FechaOperacion=" + strFechaOperacion + "', '', 'dialogHeight:530px; dialogWidth:1180px; dialogLeft:150px;');")
        End With
        EjecutarJS(script.ToString())
    End Sub
#End Region
#Region " /* Métodos Controla Habilitar/Deshabilitar Campos */ "
    Private Sub ControlarCamposTI()
        CargarPortafoliosOI(Me.ddlFondoDestino)
        Me.lblFondo.Text = "Fondo Origen"
        Me.lblFondoDestino.Visible = True
        Me.ddlFondoDestino.Visible = True
        MostrarOcultarBotonesAcciones(False)
        Session("ValorCustodio") = ""
        Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        CargarFechaVencimiento()
        txtHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Traspaso"
        hdMensaje.Value = "el Ingreso"
        If Me.ddlFondo.SelectedValue <> codigoMultifondo Then
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
        Me.btnAceptar.Visible = True
    End Sub
    Private Sub CargarPaginaModificarEO_CO_XO(ByVal acceso As String)
        If acceso = "EO" Or acceso = "CO" Then
            CargarPaginaBuscar()
            HabilitaDeshabilitaCabecera(False)
            Me.btnBuscar.Visible = False
            HabilitaDeshabilitaDatosOperacionComision(True)
            Me.btnCaracteristicas.Visible = True
            Me.btnCaracteristicas.Enabled = True
            Me.btnAceptar.Visible = True
            Session("EstadoPantalla") = "Modificar"
            '------------------------------------------------------------------
            'GUID: Identificador único para cuponetas temporales
            '------------------------------------------------------------------
            Dim GUID As Guid = System.Guid.NewGuid()
            ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
            '------------------------------------------------------------------
            If (Me.hdPagina.Value = "CO") And ViewState("CodigoTercero") <> "" Then
                Me.ddlTipoTasa.Enabled = False
                Me.ddlGrupoInt.Enabled = False
                Me.ddlIntermediario.Enabled = False
                Me.ddlContacto.Enabled = False
            End If
        End If
    End Sub
    Private Sub ControlarCamposOE()
        MostrarOcultarBotonesAcciones(False)
        Me.btnAceptar.Visible = True
    End Sub
    Private Sub CargarPaginaInicio()
        HabilitaDeshabilitaCabecera(False)
        HabilitaDeshabilitaDatosOperacionComision(False)
        OcultarBotonesInicio()

        Me.btnAceptar.Visible = False
    End Sub
    Private Sub CargarPaginaBuscar()
        Me.btnProcesar.Visible = True
        Me.btnProcesar.Enabled = True
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
        Me.btnAceptar.Visible = False
    End Sub
    Private Sub CargarPaginaProcesar()
        Dim strJS As New StringBuilder
        btnAceptar.Visible = True
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
        Me.btnAceptar.Visible = False
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
        Me.dgLista.Enabled = estado
        HabilitaDeshabilitaValoresGrilla(estado)
        Me.tbFechaLiquidacion.ReadOnly = Not estado
        'Me.txtMnomOp.ReadOnly = Not estado
        'Me.txtMnomOrd.ReadOnly = Not estado
        Me.ddlTipoTasa.Enabled = estado
        Me.txtYTM.ReadOnly = Not estado
        Me.txtPrecioNegoc.ReadOnly = Not estado
        Me.txtInteresCorNeg.ReadOnly = Not estado
        Me.ddlGrupoInt.Enabled = estado
        Me.ddlIntermediario.Enabled = estado
        Me.ddlPlaza.Enabled = estado
        Me.ddlContacto.Enabled = estado
        Me.txtMontoOperacional.ReadOnly = Not estado
        Me.txtPrecioNegSucio.ReadOnly = Not estado
        Me.txtHoraOperacion.ReadOnly = Not estado
        Me.txtNroPapeles.ReadOnly = Not estado
        Me.txtObservacion.ReadOnly = Not estado
        Me.txtMontoNetoOpe.ReadOnly = Not estado
        If estado Then
            imgFechaVcto.Attributes.Add("class", "input-append date")
        Else
            imgFechaVcto.Attributes.Add("class", "input-append")
        End If
        Me.imbEjecucion.Enabled = estado
        Me.tbFixing.ReadOnly = Not estado
        Me.txtMontoOperacionDestino.ReadOnly = Not estado
        chkFicticia.Enabled = False
        chkEmisionPrimaria.Enabled = False
        If (Not Session("EstadoPantalla") Is Nothing And Not Session("Procesar") Is Nothing) Then
            If Session("EstadoPantalla") = "Ingresar" And Me.ddlFondo.SelectedValue <> codigoMultifondo And Session("Procesar") = "0" Then
                chkFicticia.Enabled = True
                chkEmisionPrimaria.Enabled = True
            End If
        End If
        If Me.ddlFondo.SelectedValue = codigoMultifondo Then
            Me.chkRegulaSBS.Enabled = False
        Else
            Me.chkRegulaSBS.Enabled = estado
        End If

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inhabilita valor default: YTM | 07/06/18 
        ddlModoNegociacion.Enabled = False
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inhabilita valor default: YTM | 07/06/18 
    End Sub
    Private Sub HabilitaDeshabilitaValoresGrilla(ByVal estado As Boolean)
        Dim i As Integer
        For i = 0 To dgLista.Rows.Count - 1
            CType(dgLista.Rows(i).FindControl("txtValorComision1"), TextBox).Enabled = estado
            CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox).Enabled = estado
        Next
    End Sub
    Private Sub OcultarBotonesInicio()
        Me.btnCuponera.Visible = False
        Me.btnBuscar.Visible = False
        Me.btnCaracteristicas.Visible = False
        Me.btnLimites.Visible = False
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
        Me.lblfecfinbono.Text = ""
        Me.lblnominales.Text = ""
        Me.lblSaldoValor.Text = ""
        Me.lblEmisor.Text = ""
        Me.LBLBase.Text = ""
        Me.lblBaseCupon.Text = ""
        Me.lblFecUltCupon.Text = ""
        Me.lblPrecioVector.Text = ""
        Me.lblBaseTir.Text = ""
        Me.lblFecProxCupon.Text = ""
        Me.lblUnidades.Text = ""
        Me.lblDuracion.Text = ""
        Me.lblRescate.Text = ""
        Me.lblMonedaDestino.Text = ""
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
        Me.txtMnomOp.Text = ""
        Me.txtMnomOrd.Text = ""
        Me.ddlTipoTasa.SelectedIndex = 0
        Me.txtYTM.Text = ""
        Me.lblPrecioCal.Text = ""
        Me.txtPrecioNegoc.Text = ""
        Me.lblInteresCorrido.Text = ""
        Me.txtInteresCorNeg.Text = ""
        Me.ddlGrupoInt.SelectedIndex = 0
        If Me.ddlIntermediario.Items.Count > 0 Then Me.ddlIntermediario.SelectedIndex = 0
        CargarContactos()
        Me.ddlContacto.SelectedIndex = 0
        Me.txtMontoOperacional.Text = ""
        Me.txtPrecioNegSucio.Text = ""
        Me.txtHoraOperacion.Text = ""
        Me.txtNroPapeles.Text = ""
        Me.txtObservacion.Text = ""
        Me.txttotalComisionesC.Text = ""
        Me.tbFixing.Text = ""
        Me.txtMontoOperacionDestino.Text = ""
        Me.txtMontoNetoOpe.Text = ""
        Me.txtHoraOperacion.Text = ""
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
    End Sub
    Private Sub HabilitarModoNegociacion()
        If ddlModoNegociacion.SelectedValue = "T" Then
            HabilitarNegociacionYTM()
        ElseIf ddlModoNegociacion.SelectedValue = "P" Then
            HabilitarNegociacionPrecio()
        End If
    End Sub
    'Private Sub DeshabilitarModoNegociacion()
    '    'txtMnomOrd.ReadOnly = False
    '    'txtMnomOp.ReadOnly = False
    '    txtPrecioNegoc.ReadOnly = False
    '    txtPrecioNegSucio.ReadOnly = False
    '    txtInteresCorNeg.ReadOnly = False
    '    txtMontoOperacional.ReadOnly = False
    '    txtYTM.ReadOnly = False
    'End Sub
    Private Sub HabilitarNegociacionYTM()
        'txtMnomOrd.ReadOnly = True
        'txtMnomOp.ReadOnly = True
        txtPrecioNegoc.ReadOnly = True
        txtPrecioNegSucio.ReadOnly = True
        txtInteresCorNeg.ReadOnly = True
        txtYTM.ReadOnly = False
    End Sub
    Private Sub HabilitarNegociacionPrecio()
        'txtMnomOrd.ReadOnly = True
        'txtMnomOp.ReadOnly = True
        txtPrecioNegoc.ReadOnly = False
        txtPrecioNegSucio.ReadOnly = True
        txtInteresCorNeg.ReadOnly = True
        txtYTM.ReadOnly = True
    End Sub
#End Region
    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, "BONOS", Me.ddlOperacion.SelectedItem.Text,
        Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text)
    End Sub
    Private Sub CargarCuponera(ByVal strCodigoMnemonico As String)
        Dim oCuponeraBM As New CuponeraBM
        Dim dtCuponera As DataTable = oCuponeraBM.SeleccionarPorOrdenInversion(strCodigoMnemonico, DatosRequest)
        Session("dtCuponera") = dtCuponera
    End Sub
    Private Sub GrabarCuponeraOI(ByVal IsTemporal As Boolean, Optional ByVal GUID As String = "")
        CargarCuponera(txtMnemonico.Text.Trim)
        Dim oCuponera As New CuponeraBM
        Dim dtCuponeraOI As DataTable = CType(Session("dtCuponera"), DataTable)
        Dim i As Integer
        Dim montoNominal As Decimal = Math.Round(Convert.ToDecimal(Me.txtMnomOp.Text.Replace(".", DecimalSeparator)), 2)
        Dim YTM As String = Math.Round(Convert.ToDecimal(Me.txtYTM.Text.Replace(".", DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        If Not dtCuponeraOI Is Nothing Then
            If dtCuponeraOI.Rows.Count > 0 Then
                oCuponera.EliminarCuponeraOI(GUID, IsTemporal, txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest)
                For i = 0 To dtCuponeraOI.Rows.Count - 1
                    If i < dtCuponeraOI.Rows.Count - 1 Then
                        oCuponera.InsertarCuponeraOI(0, GUID, IsTemporal, txtCodigoOrden.Value, ddlFondo.SelectedValue.ToString,
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaInicio")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaTermino")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("Amortizacion")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("ValorNominal")),
                        String.Empty, String.Empty, "N", dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TasaCupon")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TotalVP")), montoNominal, Me.ddlTipoTasa.SelectedValue,
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("DiferenciaDias")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("DiasPago")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("Base")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("codigoTipoAmortizacion")),
                        txtMnemonico.Text.ToString(), DatosRequest)
                    Else
                        oCuponera.InsertarCuponeraOI(1, GUID, IsTemporal, txtCodigoOrden.Value, ddlFondo.SelectedValue.ToString,
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaInicio")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaTermino")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("Amortizacion")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("ValorNominal")),
                        String.Empty, String.Empty, "N", dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TasaCupon")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TotalVP")), montoNominal, Me.ddlTipoTasa.SelectedValue,
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("DiferenciaDias")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("DiasPago")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("Base")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("codigoTipoAmortizacion")),
                        txtMnemonico.Text.ToString(), DatosRequest)
                    End If
                Next
            End If
        End If
    End Sub
    Private Sub EliminarCuponerasOITemporales()
        Dim oCuponera As New CuponeraBM
        Dim dtCuponeraOI As DataTable = CType(Session("dtCuponera"), DataTable)
        If Not dtCuponeraOI Is Nothing Then
            If dtCuponeraOI.Rows.Count > 0 Then
                oCuponera.EliminarCuponeraOI(ViewState("CuponeraTemporalGUID").ToString(), True, txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest)
            End If
        End If
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        Try
            If ddlFondo.SelectedValue <> "" Then
                CargarFechaVencimiento()
            End If
            Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
            If cantidadreg > 0 Then
                AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
                ddlFondo.SelectedIndex = 0
                Exit Sub
            End If

            Dim portafolioBM As New PortafolioBM()
            Dim dt As DataTable = portafolioBM.PortafolioSelectById(ddlFondo.SelectedValue)
            Dim fecha As Decimal = 0

            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    fecha = CDec(dt.Rows(0)("FechaNegocio").ToString())
                    tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(fecha)
                    tbFechaLiquidacion.Text = tbFechaOperacion.Text
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub ObtieneImpuestosComisiones()
        If Not Session("Plaza") Is Nothing And Not Session("TipoRenta") Is Nothing Then
            OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Plaza"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
        End If
    End Sub
    Private Sub imbEjecucion_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imbEjecucion.Click
        Dim oDatosHiper As New ValoresBM
        Dim dsDatosHiper As DataSet
        dsDatosHiper = oDatosHiper.ObtenerDatosHipervalorizador(txtMnemonico.Text, DatosRequest)
        If dsDatosHiper.Tables.Count > 0 Then
            If dsDatosHiper.Tables(0).Rows.Count > 0 Then
                With dsDatosHiper.Tables(0)
                    EjecutarJS(UIUtility.MostrarPopUp("../../Parametria/AdministracionValores/frmHipervalorizador.aspx?nemonico=" + txtMnemonico.Text +
                    "&valorUnitario=" + .Rows(0)("valorUnitario").ToString + "&tasaCupon=" + .Rows(0)("tasaCupon").ToString + "&baseTIR=" + .Rows(0)("baseTir").ToString +
                    "&baseDias=" + .Rows(0)("baseCupon").ToString + "&fechaEmision=" + ConvertirFechaaString(.Rows(0)("fechaEmision")) +
                    "&tipoCuponera=" + .Rows(0)("tipoCuponera").ToString + "&codigoSBS=" + Me.txtSBS.Text + "&fondo=" + ddlFondo.SelectedValue, "no", 800,
                    650, 30, 50, "No", "No", "Yes", "Yes"), False)
                End With
            End If
        End If
    End Sub
    Private Sub OcultaVisibleFixing()
        If lblMonedaDestino.Text = "DOL" And lblMonedaDestino.Text <> lblMoneda.Text Then
            Me.lbFixing.Visible = True
            Me.tbFixing.Visible = True
        Else
            Me.lbFixing.Visible = False
            Me.tbFixing.Visible = False
        End If
    End Sub
    Private Sub btnLimitesParametrizados_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnLimitesParametrizados.Click
        Try
            ConsultaLimitesPorInstrumento()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub ConsultaLimitesPorInstrumento()
        Dim strFondo As String = ddlFondo.SelectedValue
        Dim strEscenario As String = "REAL"
        Dim strFecha As String = tbFechaOperacion.Text
        Dim strValorNivel As String = txtMnemonico.Text
        EjecutarJS(UIUtility.MostrarPopUp("../Reportes/Orden de Inversion/frmVisorReporteLimitesPorInstrumento.aspx?Portafolio=" & strFondo & "&ValorNivel=" &
        strValorNivel & "&Escenario=" & strEscenario & "&Fecha=" & strFecha, "10", 800, 600, 10, 10, "No", "Yes", "Yes", "Yes"), False)
    End Sub
    Public Sub SaldoUnidades_EnModificacion(ByVal lblSaldoValor As TextBox, ByVal UnidadesNegociadas As Decimal, ByVal CodigoOperacion As String)
        If Session("EstadoPantalla") = "Modificar" And CodigoOperacion = "2" Then
            lblSaldoValor.Text = CDec(lblSaldoValor.Text) + UnidadesNegociadas
        End If
    End Sub
    Protected Sub CargarComboModoNegociacion()
        Dim dt As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dt = oParametrosGeneralesBM.Listar(ParametrosSIT.INDICE_PRECIO_TASA, DatosRequest)
        HelpCombo.LlenarComboBox(ddlModoNegociacion, dt, "Valor", "Nombre", True)
        ddlModoNegociacion.SelectedValue = "T"
    End Sub
    Protected Sub ddlPlaza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPlaza.SelectedIndexChanged
        Session("Plaza") = ddlPlaza.SelectedValue
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        ObtieneImpuestosComisiones()
        dgLista.Visible = True
    End Sub
    Protected Sub ddlModoNegociacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModoNegociacion.SelectedIndexChanged
        If Request.QueryString("PTNeg") <> "CO" And Request.QueryString("PTNeg") <> "EO" Then
            HabilitarModoNegociacion()
        End If
    End Sub
    Private Sub recalcularComision(ByVal comisionSAB As Decimal, ByVal grid As GridView)
        Dim txtIGV As TextBox = CType(grid.Rows(0).FindControl("txtValorComision1"), TextBox)
        Dim valorIGV As Decimal = Decimal.Parse(grid.Rows.Item(0).Cells(2).Text.Substring(1, Len(grid.Rows.Item(0).Cells(2).Text.Trim) - 3))
        Dim dblTotalComisiones As Decimal = 0.0
        txtIGV.Text = (CType(ViewState("igvTemp"), Decimal) + (comisionSAB * (valorIGV / 100))).ToString
        txtIGV.Text = IIf(txtIGV.Text.Trim = String.Empty, 0, txtIGV.Text)
        dblTotalComisiones = Decimal.Parse(txtIGV.Text) + CType(ViewState("totalComision"), Decimal) + comisionSAB
        txtIGV.Text = Format(Decimal.Parse(txtIGV.Text), "##,##0.00")
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        If chkRecalcular.Checked Then
            txtComisionConasev = CType(dgLista.Rows(1).FindControl("txtValorComision1"), TextBox)
            txtCuotaCavali = CType(dgLista.Rows(1).FindControl("txtValorComision2"), TextBox)
            If dgLista.Rows.Count > 2 Then txtCuotaBVL = CType(dgLista.Rows(2).FindControl("txtValorComision1"), TextBox)
            txtComisionConasev.Text = ViewState("comisionConasevTemp")
            txtCuotaCavali.Text = ViewState("comisionCavaliTemp")
            If dgLista.Rows.Count > 2 Then txtCuotaBVL.Text = ViewState("txtCuotaBVLTemp")
        End If
        If (ddlOperacion.SelectedValue = "2") Then
            txtMontoNetoOpe.Text = Format(txtMontoOperacional.Text.Replace(",", "") - dblTotalComisiones, "##,##0.00")
        Else
            txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoOperacional.Text.Replace(",", ""), "##,##0.00")
        End If
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF011 - Se resalta Comisión SAB a Cero para que pueda ser editado | 07/08/18 
    Private Sub validarComisionSAB(ByVal dblTotalComisiones As Decimal)
        txtComisionSAB = CType(dgLista.Rows(0).FindControl("txtValorComision2"), TextBox)
        txtComisionIGV = CType(dgLista.Rows(0).FindControl("txtValorComision1"), TextBox)
        txtComisionConasev = CType(dgLista.Rows(1).FindControl("txtValorComision1"), TextBox)
        txtCuotaCavali = CType(dgLista.Rows(1).FindControl("txtValorComision2"), TextBox)
        If dgLista.Rows.Count > 2 Then txtCuotaBVL = CType(dgLista.Rows(2).FindControl("txtValorComision1"), TextBox)
        txtComisionIGV.Text = IIf(txtComisionIGV.Text.Trim = String.Empty, "0", txtComisionIGV.Text)
        txtComisionSAB.Text = IIf(txtComisionSAB.Text.Trim = String.Empty, "0", txtComisionSAB.Text)
        txtComisionConasev.Text = IIf(txtComisionConasev.Text.Trim = String.Empty, "0", txtComisionConasev.Text)
        txtCuotaCavali.Text = IIf(txtCuotaCavali.Text.Trim = String.Empty, "0", txtCuotaCavali.Text)
        If dgLista.Rows.Count > 2 Then txtCuotaBVL.Text = IIf(txtCuotaBVL.Text.Trim = String.Empty, "0", txtCuotaBVL.Text)
        ViewState("igvTemp") = txtComisionIGV.Text
        ViewState("comisionConasevTemp") = txtComisionConasev.Text
        ViewState("comisionCavaliTemp") = txtCuotaCavali.Text
        If dgLista.Rows.Count > 2 Then ViewState("txtCuotaBVLTemp") = txtCuotaBVL.Text
        ViewState("totalComision") = dblTotalComisiones - Decimal.Parse(txtComisionIGV.Text)
        txtComisionSAB.BorderColor = Drawing.Color.Red
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF011 - Se resalta  Comisión SAB a Cero para que pueda ser editado | 07/08/18 

    Private Function ObtenerTipoFondo() As String
        ObtenerTipoFondo = String.Empty
        Dim portafolioBM As New PortafolioBM()
        Dim dt As DataTable = portafolioBM.PortafolioSelectById(ddlFondo.SelectedValue)
        If dt IsNot Nothing Then
            If dt.Rows.Count > 0 Then
                ObtenerTipoFondo = dt.Rows(0)("TipoNegocio").ToString()
            End If
        End If
    End Function

#Region "Registro En la Preorden"

    Sub GuardarPreOrden()
        'INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion

        Dim neg As NegociacionRentaFija = Session("NegociacionRentaFija")

        Dim entPreOrden As New PrevOrdenInversionBE
        Dim negPreOrden As New PrevOrdenInversionBM
        Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow = CType(entPreOrden.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)

        negPreOrden.InicializarPrevOrdenInversion(oRow)

        oRow.CodigoPrevOrden = 0
        oRow.CodigoOperacion = ddlOperacion.SelectedValue 'Compra/Venta/Etc.
        oRow.CodigoNemonico = txtMnemonico.Text
        oRow.IndPrecioTasa = ddlModoNegociacion.SelectedValue 'T: Tasa YTM % , P: Precio
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)

        oRow.FechaLiquidacion = neg.FechaEvaluacion.ToString("yyyyMMdd")
        oRow.Cantidad = neg.CantidadUnidadesNegociadas
        oRow.CantidadOperacion = neg.CantidadUnidadesNegociadas
        oRow.TipoTasa = ddlTipoTasa.SelectedValue
        oRow.Tasa = neg.YTM * 100 'Es un Porcentaje
        oRow.Precio = neg.PrecioSucio * 100 'Es un Porcentaje
        oRow.PrecioOperacion = neg.PrecioSucio * 100 'Es un Porcentaje
        oRow.MontoNominal = neg.ValorNominal
        oRow.MontoOperacion = neg.ValorActual
        oRow.InteresCorrido = neg.InteresCorrido

        oRow.CodigoPlaza = ddlPlaza.SelectedValue
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.HoraOperacion = Now.ToLongTimeString()

        'Valores por Defecto          
        oRow.MedioNegociacion = "E" 'Por defecto 'ELECTRONICO'
        oRow.TipoFondo = "Normal" 'Por defecto 'NORMAL'
        oRow.TipoTramo = "AGENCIA" 'Por defecto 'AGENCIA'
        oRow.TipoCondicion = "AM" 'Por defecto 'A MERCADO'
        oRow.Porcentaje = "N" 'N: No Porcentaje, solo Monto directo
        oRow.Fixing = 0 'Por defecto 
        oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
        oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO

        ' INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Tipo Valorizacion
        oRow.TipoValorizacion = ddlTipoValorizacion.SelectedValue
        ' FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Tipo Valorizacion

        entPreOrden.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
        entPreOrden.PrevOrdenInversion.AcceptChanges()

        Dim dtAsignacion As New DataTable ' Asignacion Por Fondo
        dtAsignacion.Columns.Add("CodigoPortafolio")
        dtAsignacion.Columns.Add("Asignacion")
        'Solo necesitamos una Fila donde se indicará el 100% de unidades para el Fondo
        dtAsignacion.Rows.Add(ddlFondo.SelectedValue, neg.CantidadUnidadesNegociadas)

        'Guardamos la Pre-Orden
        negPreOrden.Insertar(entPreOrden, ParametrosSIT.TR_RENTA_FIJA.ToString(), DatosRequest, dtAsignacion)

        HabilitarBotonesAccion()
        'retornarMensajeAccion("Ingresó") 'Notificamos

        'FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
    End Sub

    Sub HabilitarBotonGuardarPreOrden()
        Me.btnIngresar.Visible = False
        Me.btnModificar.Visible = False
        Me.btnEliminar.Visible = False
        Me.btnConsultar.Visible = False
        Me.btnAceptar.Visible = True
    End Sub

    Sub HabilitarBotonesAccion()
        Me.btnIngresar.Visible = True
        Me.btnModificar.Visible = True
        Me.btnEliminar.Visible = True
        Me.btnConsultar.Visible = True

        Me.btnAceptar.Visible = False
    End Sub

    Sub ValidaOrigen()
        Dim objNeg As New OrdenInversionWorkFlowBM()
        If (objNeg.OrdenInversion_ValidaExterior(txtCodigoOrden.Value.ToString()).Equals("EXT")) Then
            lkbMuestraModalDatos.Enabled = True
        Else
            lkbMuestraModalDatos.Enabled = False
        End If
    End Sub

    Sub ActualizaDatosCarta()
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM()
        If (lkbMuestraModalDatos.Enabled = True) Then

            If Not IsDBNull(Session("ObjDatosCarta")) Then
                Dim objEnt As New DatosCartasBE()
                objEnt = CType(Session("ObjDatosCarta"), DatosCartasBE)
                oOrdenInversionWorkFlowBM.ActualizaDatosCarta(txtCodigoOrden.Value, objEnt.NumeroCuenta, objEnt.ValorTipo)
            End If

        End If
    End Sub
#End Region

End Class
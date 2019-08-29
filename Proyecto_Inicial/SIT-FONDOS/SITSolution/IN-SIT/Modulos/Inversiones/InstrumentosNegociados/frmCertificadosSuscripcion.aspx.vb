Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmCertificadosSuscripcion
    Inherits BasePage
#Region "Variables"

    Dim objintermediarioContacto As New IntermediarioContactoBM
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

#End Region
#Region "Metodos de Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Me.hdSaldo.Value = 0
            '   Me.btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
            Me.btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
            Me.btnProcesar.Attributes.Add("onclick", "javascript:return Validar();")
            Me.btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
            Me.btnModal.Style.Add("display", "none")
            'Valores del Pop Up
            If Not Session("SS_DatosModal") Is Nothing Then
                Dim datosModal As String() = CType(Session("SS_DatosModal"), String())
                If hfModal.Value = "1" Then
                    txtISIN.Text = CType(Session("SS_DatosModal"), String())(0)
                    txtSBS.Text = CType(Session("SS_DatosModal"), String())(2)
                    txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1)
                    ddlFondo.SelectedValue = CType(Session("SS_DatosModal"), String())(6)
                    lblMoneda.Text = CType(Session("SS_DatosModal"), String())(5)

                ElseIf hfModal.Value = "2" Then 'Esta session viene de la busqueda del formulario frmInversionesRealizadas
                    txtISIN.Text = CType(Session("SS_DatosModal"), String())(0).ToString.Trim()
                    txtSBS.Text = CType(Session("SS_DatosModal"), String())(1).ToString.Trim()
                    txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(2).ToString.Trim()
                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se cambia control ddloperacion por ddlfondo | 07/06/18 
                    ddlFondo.SelectedValue = CType(Session("SS_DatosModal"), String())(3).ToString.Trim()
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se cambia control ddloperacion por ddlfondo | 07/06/18 
                    ddlOperacion.SelectedValue = CType(Session("SS_DatosModal"), String())(4).ToString.Trim()
                    lblMoneda.Text = CType(Session("SS_DatosModal"), String())(5).ToString.Trim()
                    txtCodigoOrden.Value = CType(Session("SS_DatosModal"), String())(6).ToString.Trim()
                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se traslada valor de carga de código de orden para búsqueda | 07/06/18 
                    Session("Orden") = txtCodigoOrden.Value
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se traslada valor de carga de código de orden para búsqueda | 07/06/18 
                    EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
                End If
                Session.Remove("SS_DatosModal")
                hfModal.Value = "0"
            End If
            If Not Page.IsPostBack Then
                LimpiarSesiones()
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                hdRptaConfirmar.Value = "NO"
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    Me.hdPagina.Value = Request.QueryString("PTNeg")
                End If
                If (Me.hdPagina.Value = "TI") Then
                    UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
                Else
                    UIUtility.CargarOperacionOI(ddlOperacion, "OperacionOI")
                End If
                UIUtility.CargarGrupoIntermediarioOI(ddlGrupoInt)
                CargarPlaza()
                CargarPaginaInicio()
                Me.hdPagina.Value = ""
                If (Request.QueryString("PTNeg") Is Nothing) Then 'Viene de la Pagina Liquidaciones Cuentas Por Pagar
                    MostrarConsultaCertificados("0")
                End If
                DivDatosCarta.Visible = False
                DivObservacion.Visible = False
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    UIUtility.CargarPortafoliosOI(ddlFondo)
                    Me.hdPagina.Value = Request.QueryString("PTNeg")
                    EjecutarJS("$('#divMoneda').removeAttr('style');")
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
                        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
                        ControlarCamposTI()
                        Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(Request.QueryString("fechaOperacion")))
                    Else
                        Me.txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                        Me.ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                        Session("CodOrden") = txtCodigoOrden.Value
                        'ValidaOrigen()
                        If (Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Then 'Viene de la Pagina Confirmacion , Ejecucion  o Extorno de Ordenes de Inversion
                            CargarDatosOrdenInversion(txtCodigoOrden.Value)
                            CargarCaracteristicasValor()
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
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar | 13/07/18 
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

                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar | 13/07/18 
                        Else
                            If (Me.hdPagina.Value = "OE") Then 'Viene de la Pagina Ordenes Excedidas
                                ControlarCamposOE()
                            Else
                                If (Me.hdPagina.Value = "DA") Then 'Viene de la Pagina Negociacion Dias Anteriores
                                    ViewState("ORDEN") = "OI-DA"
                                    Me.tbFechaOperacion.Text = Request.QueryString("Fecha")
                                    Me.tbFechaOperacion.ReadOnly = True
                                    Me.imgFechaOperacion.Attributes.Add("class", "input-append")
                                Else
                                    If (Me.hdPagina.Value = "CP") Then 'Viene de la Pagina Liquidaciones Cuentas Por Pagar
                                        Call ConfiguraModoConsulta()
                                        ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                        txtMnemonico.Text = Request.QueryString("Mnemonico")
                                        txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                        Call CargarDatosOrdenInversion(txtCodigoOrden.Value.Trim)
                                        Call CargarCaracteristicasValor()
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
                    '     Me.btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF49", txtCodigoOrden.Value + "?", "SI")
                    '  Me.btnAceptar.Attributes.Add("onClick", "if (Confirmacion()){this.disabled = true; this.value = 'en proceso...'; __doPostBack('btnAceptar','');}")
                Else
                    'HelpCombo.CargaPortafolioSegunUsuario(ddlFondo, Session("Login"))
                    Dim dtportafolio As DataTable = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
                    HelpCombo.LlenarComboBox(ddlFondo, dtportafolio, "CodigoPortafolio", "Descripcion", True)
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub


#End Region

#Region "Metodos privados"

#End Region

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Call HabilitaBotones(True, True, True, True, True, True, True, True, True, True, True, False, True, True)
    End Sub

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
        btnAsignar.Visible = bAsignar
        btnProcesar.Visible = bProcesar
        btnImprimir.Visible = bImprimir
        btnAceptar.Visible = bAceptar
        btnBuscar.Visible = bBuscar
        btnSalir.Visible = bSalir
        btnRetornar.Visible = bRetornar
        btnCaracteristicas.Visible = bCaracteristicas
        'btnLimitesParametrizados.Visible = bLimitesParametrizados 'CMB OT 61566 20101130

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

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
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
                    ShowDialogPopupValores(txtISIN.Text.Trim.ToUpper, txtSBS.Text.Trim.ToUpper, txtMnemonico.Text.Trim.ToUpper, ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text.Trim, ddlOperacion.SelectedValue, "CS", "1")
                    Session("Busqueda") = 2
                Else
                    If Session("Busqueda") = 1 Then
                        CargarFechaVencimiento()
                        If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
                        Else
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
                        End If
                        CargarPaginaIngresar()
                        CargarCaracteristicasValor()
                        CargarIntermediario()
                        If UIUtility.ObtenerCodigoTipoOperacion("COMPRA") = Me.ddlOperacion.SelectedValue Then
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
                        ShowDialogPopupInversionesRealizadas(Me.txtISIN.Text.ToString.Trim, Me.txtSBS.Text.ToString.Trim, Me.txtMnemonico.Text.ToString.Trim, ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, ddlOperacion.SelectedValue, lblMoneda.Text.ToString, strAux, strAccion, "2")
                        Session("Busqueda") = 2
                    Else
                        If Session("Busqueda") = 1 Then
                            CargarCaracteristicasValor()
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se busca Orden de Inversión en base a Session Cargada| 07/06/18 
                            CargarDatosOrdenInversion(CType(Session("Orden"), String))
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se busca Orden de Inversión en base a Session Cargada| 07/06/18 
                            UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                            Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                            btnAceptar.Enabled = True
                            If Session("EstadoPantalla") = "Modificar" Then
                                If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF2", "Nro " + txtSBS.Text + "?", "SI")
                                Else
                                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF16", "Nro " + txtSBS.Text + "?", "SI")
                                End If
                                CargarPaginaModificar()
                            ElseIf Session("EstadoPantalla") = "Eliminar" Then
                                If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF3", "Nro " + txtSBS.Text + "?", "SI")
                                Else
                                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF17", "Nro " + txtSBS.Text + "?", "SI")
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
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub
    Public Sub CargarIntermediario()
        If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
            UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        ElseIf Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
            UIUtility.CargarIntermediariosCustodioXGrupoInterOI(ddlIntermediario, Me.hdCustodio.Value, ddlGrupoInt.SelectedValue)
        End If

        Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
    End Sub
    Public Sub CargarDatosOrdenInversion(ByVal CodigoOrden As String)
        'Try
        oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(CodigoOrden, Me.ddlFondo.SelectedValue, DatosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = oOrdenInversionBE.Tables(0).Rows(0)
        Session("CodigoMoneda") = oRow.CodigoMoneda
        txtISIN.Text = oRow.CodigoISIN
        txtMnemonico.Text = oRow.CodigoMnemonico
        txtCodigoOrden.Value = oRow.CodigoOrden
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se recupera valor de Código SBS | 07/06/18 
        txtSBS.Text = oRow.CodigoSBS
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se recupera valor de Código SBS | 07/06/18 
        If oRow.CodigoOperacion.ToString <> "" Then
            ddlOperacion.SelectedIndex = ddlOperacion.Items.IndexOf(ddlOperacion.Items.FindByValue(oRow.CodigoOperacion.ToString()))
        Else
            ddlOperacion.SelectedIndex = 0
        End If
        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        txtNroAccOrde.Text = Format(oRow.CantidadOrdenado, "##,##0.0000000")
        txtNroAccOper.Text = Format(oRow.CantidadOperacion, "##,##0.0000000")
        txtMontoNominal.Text = Format(oRow.MontoOperacion, "##,##0.0000000")
        txtPrecio.Text = Format(oRow.Precio, "##,##0.0000000")
        txttotalComisionesC.Text = Format(oRow.TotalComisiones, "##,##0.0000000")
        txtPrecPromedio.Text = Format(oRow.PrecioPromedio, "##,##0.0000000")
        txtMontoNetoOpe.Text = Format(oRow.MontoNetoOperacion, "##,##0.0000000")

        ViewState("MontoNeto") = Convert.ToDecimal(txtMontoNominal.Text)

        txtObservacion.Text = oRow.Observacion
        txtObservacionCarta.Text = oRow.ObservacionCarta

        tbHoraOperacion.Text = oRow.HoraOperacion
        hdNumUnidades.Value = txtNroAccOper.Text
        Me.tbNPoliza.Text = oRow.NumeroPoliza.ToString()

        CargarPlaza()   'HDG INC 63774	20110815
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se recupera (si fuera el caso) el tipo mercado en combo ddlPlaza | 05/06/18
        If ddlPlaza.Items.Count > 0 And oRow.Plaza <> String.Empty Then ddlPlaza.SelectedValue = oRow.Plaza
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se recupera (si fuera el caso) el tipo mercado en combo ddlPlaza | 05/06/18
        Me.ddlGrupoInt.SelectedValue = oRow.GrupoIntermediario  'modificado DB 20090325
        Dim dtAux As DataTable
        dtAux = (New TercerosBM().Seleccionar(oRow.CodigoTercero, DatosRequest)).Tables(0)
        If dtAux.Rows.Count > 0 Then
            Me.hdCustodio.Value = dtAux.Rows(0)("CodigoCustodio")
            CargarIntermediario()
            If ddlIntermediario.Items.Count > 1 Then
                ddlIntermediario.SelectedValue = oRow.CodigoTercero
                CargarContactos()
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se valida campo vacío "Contacto" debido a que ahora está oculto | 05/06/18
                If oRow.CodigoContacto <> String.Empty Then ddlContacto.SelectedValue = oRow.CodigoContacto
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se valida campo vacío "Contacto" debido a que ahora está oculto | 05/06/18
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

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se recupera (si fuera el caso) el motivo de cambio en combo dllMotivoCambio| 05/06/18
        If ddlMotivoCambio.Items.Count > 0 And oRow.CodigoMotivoCambio <> String.Empty Then ddlMotivoCambio.SelectedValue = oRow.CodigoMotivoCambio
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se recupera (si fuera el caso) el motivo de cambio en combo dllMotivoCambio| 05/06/18

        'Catch ex As Exception
        '    If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
        '        AlertaJS(ObtenerMensaje("CONF31"))
        '    Else
        '        AlertaJS(ObtenerMensaje("CONF32"))
        '    End If
        'End Try
    End Sub
    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Dim feriado As New FeriadoBM
        Try
            If CDec(txtNroAccOper.Text) <= 0 Then
                AlertaJS("Ingrese una cantidad de acciones mayor a 0")
                Exit Sub
            End If
            If ValidarFechas() = True Then
                If UIUtility.ValidarHora(Me.tbHoraOperacion.Text) = False Then
                    AlertaJS(ObtenerMensaje("CONF22"))
                    Exit Sub
                End If
                If (feriado.VerificaDia(UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text), Session("Mercado")) = False) Then
                    Me.AlertaJS("Fecha de Vencimiento no es valida.")
                    Exit Sub
                End If
                CalcularComisiones()
                Session("Procesar") = 1
                ViewState("estadoOI") = ""
                'LIMITES
                'If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then OrdenInversion.CalculaLimitesOnLine(Me.Page, DatosRequest, ViewState("estadoOI"), ViewState("GUID_Limites"))
                CargarPaginaProcesar()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    '#Error No se utiliza este metodo
    Private Function ObtieneCustodiosSaldos() As Boolean
        Dim strCodigoOperacion As String = String.Empty
        If Session("EstadoPantalla") = "Ingresar" Or Session("EstadoPantalla") = "Modificar" Then
            If VerificarSaldosCustodios() = False Then
                'Interpretar Codigos de Operacion (Según Tipo de Negociacion)
                '------------------------------------------------------------------------------
                If Me.hdPagina.Value = "TI" Then
                    Select Case ddlOperacion.SelectedValue
                        Case UIUtility.ObtenerCodigoOperacionTIngreso() : strCodigoOperacion = UIUtility.ObtenerCodigoOperacionCompra()
                        Case UIUtility.ObtenerCodigoOperacionTEgreso() : strCodigoOperacion = UIUtility.ObtenerCodigoOperacionVenta()
                    End Select
                Else
                    strCodigoOperacion = ddlOperacion.SelectedValue.ToString()
                End If
                '------------------------------------------------------------------------------

                If strCodigoOperacion = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
                    AlertaJS(ObtenerMensaje("CONF28"))
                    Dim strSaldoFaltante As String
                    strSaldoFaltante = txtNroAccOper.Text
                    MostrarOtrosCustodios(Me.tbFechaOperacion.Text, Me.txtMnemonico.Text, Me.ddlFondo.SelectedValue, strSaldoFaltante, Me.hdCustodio.Value)
                    Return False
                Else
                    Return False
                End If
            End If
        End If
        Return True
    End Function
    Private Function VerificarSaldosCustodios() As Boolean
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        Dim decMontoAux As Decimal = 0.0
        Dim cantCustodios As Integer = 0
        Try
            If hdNumUnidades.Value = txtNroAccOper.Text Then
                Return True
            End If
            '********COMPRA*********
            If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                Dim oValores As New ValoresBM
                Dim blnResul As Boolean = oValores.BuscarCustodioValor(Me.txtMnemonico.Text, Me.ddlFondo.SelectedValue, Me.hdCustodio.Value, DatosRequest)
                If blnResul = False Then
                    AlertaJS(ObtenerMensaje("CONF37"))
                    Return False
                End If
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.txtNroAccOper.Text
                hdNumUnidades.Value = txtNroAccOper.Text
                Return True
            End If
            '********VENTA*********
            If Session("ValorCustodio") Is Nothing Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
            ElseIf Session("ValorCustodio") = "" Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
            End If
            decMontoAux = UIUtility.ObtenerSumatoriaSaldosSeleccionados(CType(Session("ValorCustodio"), String), cantCustodios)
            If decMontoAux = Convert.ToDecimal(Me.txtNroAccOper.Text.Replace(".", UIUtility.DecimalSeparator)) Then
                hdNumUnidades.Value = txtNroAccOper.Text
                Return True
            ElseIf decMontoAux > Convert.ToDecimal(Me.txtNroAccOper.Text.Replace(".", UIUtility.DecimalSeparator)) Then
                'redefinir calculos porq excede
                If cantCustodios = 1 Then
                    'porque solamente es el primer custodio
                    Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.txtNroAccOper.Text
                    hdNumUnidades.Value = txtNroAccOper.Text
                    Return True
                Else
                    'porq hay mas de un custodio. ajustar montos
                    Session("ValorCustodio") = UIUtility.AjustarMontosCustodios(CType(Session("ValorCustodio"), String), Me.txtNroAccOper.Text)
                    hdNumUnidades.Value = txtNroAccOper.Text
                    Return True
                End If
                Return False
            ElseIf decMontoAux < Convert.ToDecimal(Me.txtNroAccOper.Text.Replace(".", UIUtility.DecimalSeparator)) Then
                'redefinir calculos porq falta
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
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
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Certificado de Suscripción?"
                    Else
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de Pre-Orden de Inversión de Certificado de Suscripción?"
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
        LimpiarSesiones()
        'GUID: Identificador único para cuponetas temporales
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlOperacion)
        CargarPaginaAccion()
        Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        tbFechaOperacion.Text = objutil.RetornarFechaNegocio
        Me.tbHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Ingresar"
        SincronizarBotones("I")
        hdMensaje.Value = "el Ingreso"
        hdNumUnidades.Value = 0
        If Not ddlFondo.Items.FindByValue(PORTAFOLIO_MULTIFONDOS) Is Nothing Then 'RGF 20081001
            Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS
        End If
        lblTitulo.Text = "PreOrden de Inversión - CERTIFICADO DE SUSCRIPCIÓN"
    End Sub
    Private Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos()
        CargarFechaVencimiento() 'RGF 20081124
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
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
                    If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                        Me.hdCustodio.Value = dtAux.Rows(i)("codigoCustodio")
                    End If
                    Exit For
                End If
            Next
        End If
    End Sub
    Public Sub CalcularComisiones()
        Dim dblTotalComisiones As Decimal = 0.0
        If Me.hdPagina.Value = "TI" Then
            dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), Me.txtMontoNominal.Text.Replace(",", ""), Me.txtNroAccOper.Text.Replace(",", ""))
        Else
            dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), Me.txtMontoNominal.Text.Replace(",", ""), Me.txtNroAccOper.Text.Replace(",", ""))
        End If
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        If (ddlOperacion.SelectedValue = "2") Then
            txtMontoNetoOpe.Text = Format(txtMontoNominal.Text.Replace(",", "") - dblTotalComisiones, "##,##0.0000000")
        Else
            txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoNominal.Text.Replace(",", ""), "##,##0.0000000")
        End If
        Dim strMontoAux As String = txtMontoNetoOpe.Text.Replace(",", "")
        strMontoAux = strMontoAux.Replace(".", UIUtility.DecimalSeparator)
        Dim strAccAux As String = txtNroAccOper.Text.Replace(",", "")
        strAccAux = strAccAux.Replace(".", UIUtility.DecimalSeparator)
        txtPrecPromedio.Text = Format(Convert.ToDecimal(strMontoAux) / Convert.ToDecimal(strAccAux), "##,##0.0000000")
    End Sub
    Private Sub actualizaMontos()
        Dim dblTotalComisiones As Decimal = 0.0
        dblTotalComisiones = UIUtility.ActualizaMontosFinales(dgLista)

        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        If Session("Mercado") = 1 Then  'local
            txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoNominal.Text.Replace(",", ""), "##,##0.0000000")
        ElseIf Session("Mercado") = 2 Then  'extranjero
            txtMontoNetoOpe.Text = Format(Me.txtNroAccOper.Text.Replace(",", "") + dblTotalComisiones, "##,##0.0000000")
        End If
        Dim strMontoAux As String = txtMontoNetoOpe.Text.Replace(",", "")
        strMontoAux = strMontoAux.Replace(".", UIUtility.DecimalSeparator)
        Dim strAccAux As String = txtNroAccOper.Text.Replace(",", "")
        strAccAux = strAccAux.Replace(".", UIUtility.DecimalSeparator)
        txtPrecPromedio.Text = Format(Convert.ToDecimal(strMontoAux) / Convert.ToDecimal(strAccAux), "##,##0.0000000")
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        Dim accionRpta As String = String.Empty
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
        Try
            If hdRptaConfirmar.Value.ToUpper = "SI" Then
                EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
                If Me.hdPagina.Value <> "" And Me.hdPagina.Value <> "DA" And Me.hdPagina.Value <> "TI" And Me.hdPagina.Value <> "MODIFICA" Then
                    If Me.hdPagina.Value = "EO" Or Me.hdPagina.Value = "CO" Then
                        ModificarOrdenInversion()
                        UIUtility.InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
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
                                ActualizaDatosCarta()
                                ReturnArgumentShowDialogPopup()
                            End If
                        End If
                    End If
                Else
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
                    actualizaMontos()
                    If Session("EstadoPantalla") = "Ingresar" Then
                        If Session("Procesar") = 1 Then
                            Dim strcodigoOrden As String
                            strcodigoOrden = InsertarOrdenInversion()

                            If strcodigoOrden <> "" Then
                                If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                    If ViewState("estadoOI") = "E-EXC" Then
                                        Dim toUser As String = ""
                                        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                                        Dim dt As DataTable
                                        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                                        For Each fila As DataRow In dt.Rows
                                            toUser = toUser + fila("Valor").ToString() & ";"
                                        Next
                                        UIUtility.EnviarMail(toUser, "", "Pendiente de aprobación - Orden excedido por Limites de Inversión", OrdenInversion.MensajeExcesosOI(strcodigoOrden, ddlFondo.SelectedValue, DatosRequest), DatosRequest)
                                    End If
                                End If
                            End If
                            oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                            Me.txtCodigoOrden.Value = strcodigoOrden
                            If Me.hdPagina.Value <> "TI" And chkFicticia.Checked = False Then
                                UIUtility.InsertarModificarImpuestosComisiones("I", dgLista, strcodigoOrden, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                            End If
                            Session("dtdatosoperacion") = ObtenerDatosOperacion()
                            GenerarLlamado(strcodigoOrden + ViewState("CodigoOrden_T"), Me.ddlFondo.SelectedValue, "CERTIFICADO DE SUSCRIPCION", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text)
                            Session("CodigoOI") = strcodigoOrden
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                            accionRpta = "Ingresó"
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                            CargarPaginaAceptar()
                        End If
                    Else
                        If Session("EstadoPantalla") = "Modificar" Then
                            actualizaMontos()
                            ModificarOrdenInversion()
                            FechaEliminarModificarOI("M")
                            UIUtility.InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                            Session("Modificar") = 0
                            CargarPaginaAceptar()
                            Session("dtdatosoperacion") = ObtenerDatosOperacion()
                            If Me.hdPagina.Value <> "MODIFICA" Then
                                GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "CERTIFICADO DE SUSCRIPCION", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text)
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
                            retornarMensajeAccion(accionRpta)
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                        End If
                    Else
                        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                        If (Session("EstadoPantalla") = "Eliminar" Or (Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Ingresar")) Then retornarMensajeAccion(accionRpta)
                        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
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
        Dim orden As String = "Certificado Suscripción"
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
        Return mensaje.ToString
    End Function
    Public Function ObtenerDatosOperacion() As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operacion"
        drGrilla("v1") = Me.tbFechaOperacion.Text
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = Me.tbFechaLiquidacion.Text
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = Me.tbHoraOperacion.Text
        drGrilla("c4") = "Número Papeles Ordenadas"
        drGrilla("v4") = Me.txtNroAccOrde.Text
        drGrilla("c5") = "Número Papeles Operación"
        drGrilla("v5") = Me.txtNroAccOper.Text
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
        drGrilla("v10") = Me.txtObservacion.Text
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
        drGrilla("v19") = Me.txttotalComisionesC.Text
        drGrilla("c20") = "Monto Neto Operación"
        drGrilla("v20") = Me.txtMontoNetoOpe.Text
        drGrilla("c21") = "Precio Promedio"
        drGrilla("v21") = Me.txtPrecPromedio.Text
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Private Sub ReturnArgumentShowDialogPopup()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica el retorno de mensaje de confirmación | 12/06/18
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
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica el retorno de mensaje de confirmación | 12/06/18
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
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
        oRow.CantidadOrdenado = Convert.ToDecimal(txtNroAccOrde.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.CantidadOperacion = Convert.ToDecimal(txtNroAccOper.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.MontoOperacion = Convert.ToDecimal(txtMontoNominal.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.Precio = Convert.ToDecimal(txtPrecio.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.TotalComisiones = Convert.ToDecimal(txttotalComisionesC.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.PrecioPromedio = Convert.ToDecimal(txtPrecPromedio.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.MontoNetoOperacion = Convert.ToDecimal(txtMontoNetoOpe.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.Situacion = "A"

        oRow.Observacion = txtObservacion.Text
        oRow.ObservacionCarta = txtObservacionCarta.Text

        oRow.HoraOperacion = Me.tbHoraOperacion.Text
        oRow.CategoriaInstrumento = "CS"    'UNICO POR TIPO
        oRow.NumeroPoliza = Me.tbNPoliza.Text.Trim
        oRow.GrupoIntermediario = ddlGrupoInt.SelectedValue 'DB 20090305
        oRow.Plaza = ddlPlaza.SelectedValue
        If Not ViewState("estadoOI") Is Nothing Then
            If ViewState("estadoOI").Equals("E-EXC") Then
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
        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
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
        oOrdenInversionBM.EliminarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, ddlMotivoCambio.SelectedValue, DatosRequest)
        oImpComOP.Eliminar(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, DatosRequest)
    End Sub
    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(Me.ddlFondo.SelectedValue, Me.txtCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc, txtComentarios.Text, DatosRequest)
        txtComentarios.Text = ""
    End Sub
    Public Sub CargarFechaVencimiento()
        If (Me.hdPagina.Value <> "CO") Then 'HDG 20130708
            If (Me.hdPagina.Value <> "DA") Then
                Dim dtAux As DataTable = objPortafolio.SeleccionarPortafolioPorFiltro(Me.ddlFondo.SelectedValue, DatosRequest).Tables(0)
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
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        LimpiarSesiones()
        'GUID: Identificador único para cuponetas temporales
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        '------------------------------------------------------------------
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Modificar"
        Session("Modificar") = 1
        Session("Procesar") = 0
        Session("Busqueda") = 0
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
    Private Sub CargarCaracteristicasValor()
        Dim dsValor As New DataSet
        Dim drValor As DataRow
        Dim oOIFormulas As New OrdenInversionFormulasBM
        Dim sPortafolio As String = ""
        Dim i As Integer = 0
        imgFechaOperacion.Attributes.Add("class", "input-append")
        Try
            dsValor = oOIFormulas.SeleccionarCaracValor_CertificadoSuscripcion(ddlFondo.SelectedValue, txtMnemonico.Text, DatosRequest)
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
                Me.lblMarketCap.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_MarketCap")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                txtporcentaje.Text = CType(drValor("Porcentaje"), String)
                Me.lblSigDivFecha.Text = CType(drValor("val_sigDivFecha"), String)
                Me.lblSigDivFactor.Text = CType(drValor("val_sigDivFactor"), String)
                Me.lblMonNegDiaProm.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_MontoNegDiarProm")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblNroOperDiaProm.Text = CType(drValor("val_NroOperDiarProm"), String)
                Me.lblPriceEarnings.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_PriceEarnings")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblValorDFC.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_ValorDFC")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
            End If
        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF21"))
        End Try
    End Sub
    Private Sub ConfiguraModoConsulta()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        SincronizarBotones("C")
        hdMensaje.Value = "la Consulta"
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        LimpiarSesiones()
        Call ConfiguraModoConsulta()
    End Sub
    Private Sub btnCaracteristicas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCaracteristicas.Click
        If Me.txtMnemonico.Text <> "" Then
            Session("accionValor") = "MODIFICAR"
            EjecutarJS(UIUtility.MostrarPopUp("../../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" + Me.txtMnemonico.Text + "&vOI=T", "10", 1024, 600, 0, 0, "No", "No", "Yes", "Yes"), False)
            ' EjecutarJS(MostrarPopUp("../../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" + Me.txtMnemonico.Text + "&vOI=T", "10", 1025, 600, 0, 0, "No", "No", "Yes", "Yes"), False)
        Else
            AlertaJS(ObtenerMensaje("CONF23"))
        End If
    End Sub
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        EjecutarJS(MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&vportafolio=" + portafolio + "&vdescripcionPortafolio=" + ddlFondo.SelectedItem.Text.Trim + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
    End Sub
#Region " /* Métodos Personalizados (Popups Dialogs) */ "
    Private Sub MostrarOtrosCustodios(ByVal fechaOper As String, ByVal mnemonico As String, ByVal fondo As String, ByVal saldo As String, ByVal codigoCustodio As String)
        Dim strURL As String = "frmBuscarValorCustodios.aspx?vMnemonico=" & mnemonico & "&vFecha=" & fechaOper & "&vFondo=" & fondo & "&vSaldo=" & saldo & "&vCustodio=" & codigoCustodio
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '');")
    End Sub
    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal descripcionFondo As String, ByVal operacion As String, ByVal categoria As String, ByVal valor As String)
        Dim strURL As String = "frmBuscarValor.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&vFondo=" & descripcionFondo & "&cFondo=" & fondo & "&vOperacion=" & operacion & "&vCategoria=" & categoria
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica Procedimiento para cambiar en ejecucción valor de campo oculto hfModal de acuerdo a campo "valor" de entrada   | 11/06/18 
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='" & valor & "'; ")
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica Procedimiento para cambiar en ejecucción valor de campo oculto hfModal de acuerdo a campo "valor" de entrada   | 11/06/18 
    End Sub
    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, descripcionFondo As String, ByVal operacion As String, ByVal moneda As String, ByVal fecha As String, ByVal accion As String, valor As String)
        Dim strURL As String = "../frmInversionesRealizadas.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&vFondo=" & descripcionFondo & "&cFondo=" & fondo & "&vOperacion=" & operacion & "&vFechaOperacion=" & fecha & "&vAccion=" & accion & "&vCategoria=CS"
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica Procedimiento para cambiar en ejecucción valor de campo oculto hfModal de acuerdo a campo "valor" de entrada   | 11/06/18 
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='" & valor & "'; ")
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica Procedimiento para cambiar en ejecucción valor de campo oculto hfModal de acuerdo a campo "valor" de entrada   | 11/06/18 
    End Sub
    Private Sub MostrarConsultaCertificados(ByVal indicador As String)
        'Dim strURL As String = "frmConsultaCertificado.aspx?vIndica=" + indicador
        'EjecutarJS("showModalDialog('" & strURL & "', '950', '600', ''); ")
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
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
        Else
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
        End If
        CargarPaginaIngresar()
        CargarIntermediario()
        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
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
        HabilitaDeshabilitaCabecera(False)
        HabilitaDeshabilitaDatosOperacionComision(False)
        OcultarBotonesInicio()
        Me.btnAceptar.Enabled = False
    End Sub
    Private Sub CargarPaginaBuscar()
        If Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
            Me.btnAsignar.Visible = True
            Me.btnAsignar.Enabled = True
        End If
        Me.btnProcesar.Visible = True
        Me.btnProcesar.Enabled = True
    End Sub
    Private Sub CargarPaginaIngresar()
        CargarPaginaBuscar()
        Me.btnAsignar.Visible = False
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
        Me.btnAceptar.Enabled = False
    End Sub
    Private Sub CargarPaginaProcesar()
        Dim strJS As New StringBuilder
        strJS.AppendLine("$('#btnAceptar').removeAttr('disabled');")
        If Session("EstadoPantalla") <> "Ingresar" Then
            strJS.AppendLine("$('#btnImprimir').show();")
            strJS.AppendLine("$('#btnImprimir').removeAttr('disabled');")
            If ddlFondo.SelectedValue = "MULTIFONDO" Then
                btnAsignar.Visible = True
                strJS.AppendLine("$('#btnAsignar').show();")
            End If
            If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
                EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
                HelpCombo.CargarMotivosCambio(Me)
            End If
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
            If Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
                Me.btnAsignar.Visible = True
            End If
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
    Public Sub CargarPlaza()
        Dim oPlazaBM As New PlazaBM
        ddlPlaza.DataSource = oPlazaBM.Listar(Nothing)
        ddlPlaza.DataTextField = "Descripcion"
        ddlPlaza.DataValueField = "CodigoPlaza"
        ddlPlaza.DataBind()
        ddlPlaza.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub
    Private Sub HabilitaDeshabilitaDatosOperacionComision(ByVal estado As Boolean)
        Me.ddlGrupoInt.Enabled = estado
        Me.ddlIntermediario.Enabled = estado
        Me.ddlContacto.Enabled = estado
        Me.dgLista.Enabled = estado
        HabilitaDeshabilitaValoresGrilla(estado)
        Me.tbFechaOperacion.ReadOnly = Not estado
        Me.tbFechaLiquidacion.ReadOnly = Not estado
        Me.txtNroAccOrde.ReadOnly = Not estado
        Me.txtNroAccOper.ReadOnly = Not estado
        Me.txtPrecio.ReadOnly = Not estado
        Me.tbHoraOperacion.ReadOnly = Not estado
        Me.txtObservacion.ReadOnly = Not estado
        Me.txtMontoNetoOpe.ReadOnly = Not estado
        ' INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se agrega campo "Monto Operación" para la habilitación de edición| 30/05/18 
        Me.txtMontoNominal.ReadOnly = Not estado
        ' FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se agrega campo "Monto Operación" para la habilitación de edición| 30/05/18 
        Me.ddlPlaza.Enabled = estado
        If estado Then
            imgFechaOperacion.Attributes.Add("class", "input-append date")
            imgFechaVcto.Attributes.Add("class", "input-append date")
        Else
            imgFechaOperacion.Attributes.Add("class", "input-append")
            imgFechaVcto.Attributes.Add("class", "input-append")
        End If
        If (Me.hdPagina.Value = "DA") Then
            Me.tbFechaOperacion.ReadOnly = True
            Me.imgFechaOperacion.Attributes.Add("class", "input-append")
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
        Me.btnAsignar.Visible = False
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
        Me.lblMarketCap.Text = ""
        Me.lblSigDivFactor.Text = ""
        Me.lblSigDivFecha.Text = ""
        Me.lblMonNegDiaProm.Text = ""
        Me.lblNroOperDiaProm.Text = ""
        Me.lblPriceEarnings.Text = ""
        Me.lblValorDFC.Text = ""
    End Sub
    Private Sub LimpiarDatosOperacion()
        Me.lblMoneda.Text = ""
        Me.ddlFondo.SelectedIndex = 0
        Me.ddlOperacion.SelectedIndex = 0
        Me.txtISIN.Text = ""
        Me.txtSBS.Text = ""
        Me.txtMnemonico.Text = ""
        Me.tbFechaOperacion.Text = ""
        Me.tbFechaLiquidacion.Text = ""
        Me.txtNroAccOrde.Text = ""
        Me.txtNroAccOper.Text = ""
        Me.txtPrecio.Text = ""
        Me.txtMontoNominal.Text = ""
        Me.ddlGrupoInt.SelectedIndex = 0
        If ddlIntermediario.Items.Count > 0 Then
            Me.ddlIntermediario.SelectedIndex = 0
        End If
        CargarContactos()
        Me.ddlContacto.SelectedIndex = 0
        Me.tbHoraOperacion.Text = ""
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        Me.txtObservacion.Text = ""
        Me.txttotalComisionesC.Text = ""
        Me.txtMontoNetoOpe.Text = ""
        Me.txtPrecPromedio.Text = ""
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
        GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "CERTIFICADO DE SUSCRIPCION", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text)
    End Sub
    Private Sub btnAsignar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsignar.Click
        Session("URL_Anterior") = Page.Request.Url.AbsolutePath.ToString
        If ddlFondo.SelectedValue.ToString = PORTAFOLIO_MULTIFONDOS Then
            Response.Redirect("../AsignacionFondos/frmIngresoCriteriosAsignacion.aspx?vISIN=" & Me.txtISIN.Text.ToString & "&vCantidad=" & Me.txtMontoNetoOpe.Text.ToString & "&vMnemonico=" & Me.txtMnemonico.Text.ToString & "&vFondo=" & ddlFondo.SelectedValue.ToString & "&vOperacion=" & ddlOperacion.SelectedItem.Text & "&vMoneda=" & Me.lblMoneda.Text & "&vImpuestosComisiones=" & Me.txttotalComisionesC.Text & "&vCodigoOrden=" & Me.txtCodigoOrden.Value & "&vCategoria=CS", False)
        End If
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
    Private Sub ddlGrupoInt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrupoInt.SelectedIndexChanged
        UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        CargarContactos()
    End Sub
    Private Sub ConsultaLimitesPorInstrumento()
        Dim strFondo As String = ddlFondo.SelectedValue
        Dim strEscenario As String = "REAL"
        Dim strFecha As String = tbFechaOperacion.Text
        Dim strValorNivel As String = txtMnemonico.Text
        EjecutarJS(MostrarPopUp("../Reportes/Orden de Inversion/frmVisorReporteLimitesPorInstrumento.aspx?Portafolio=" & strFondo & "&ValorNivel=" & strValorNivel & "&Escenario=" & strEscenario & "&Fecha=" & strFecha, "10", 800, 600, 10, 10, "No", "Yes", "Yes", "Yes"), False)
    End Sub
    Protected Sub ddlPlaza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPlaza.SelectedIndexChanged
        Session("Mercado") = ddlPlaza.SelectedValue
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
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
End Class
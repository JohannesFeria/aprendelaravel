Imports SIT.BusinessLayer
Imports SIT.BusinessEntities
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
'--------Orden de trabajo: en OT-10795  -----------
'--------Nombre Método con cambios: crearObjetoOI()-----------
'--------Fecha de Modificación: 15/09/2017 -----------
'--------Tipo: Modificación -----------
'--------Descripción: Se válido Set al atritubo FechaLiquidación  con el valor del control tbFechaContrato cuando el control ddlOperacion tiene valor 6 -----------

Partial Class Modulos_Inversiones_InstrumentosNegociados_frmDepositoPlazos
    Inherits BasePage
    Dim objutil As New UtilDM
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
    Dim oOrdenInversionWF As New OrdenInversionWorkFlowBM
    Dim oPortafolioBM As New PortafolioBM
    Dim oImpComOP As New ImpuestosComisionesOrdenPreOrdenBM
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Me.hdSaldo.Value = 0
        agregarJavaScript()
        If Not Page.IsPostBack Then
            Me.btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
            btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 13/06/18 
            hdRptaConfirmar.Value = "NO"
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 13/06/18 
            LimpiarSesiones()
            UIUtility.CargarOperacionOI(ddlOperacion, "DepositoPlazos")
            UIUtility.CargarTipoCuponOI(ddlTipoTasa)
            CargarIntermediario()
            CargarTipoTitulo()
            CargarPaginaInicio()
            Me.hdPagina.Value = ""
            If Not Request.QueryString("PTNeg") Is Nothing Then
                HelpCombo.LlenarComboBox(ddlFondo, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "SELECCIONE")
                Me.hdPagina.Value = Request.QueryString("PTNeg")
                If Me.hdPagina.Value = "TI" Then  'Viene de la Pagina Traspaso de Instrumentos
                    Me.ddlFondo.SelectedValue = Request.QueryString("PTFondoO")
                    Me.ddlFondoDestino.SelectedValue = Request.QueryString("PTFondoD")
                    Me.ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                    Me.hdCustodio.Value = Request.QueryString("PTCustodio")
                    Me.hdSaldo.Value = Request.QueryString("PTSaldo")
                    ControlarCamposTI()
                    Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(Request.QueryString("fechaOperacion")))
                Else
                    Me.txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                    Me.ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                    If (Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Then 'Viene de la Pagina Confirmacion , Ejecucion  o Extorno de Ordenes de Inversion
                        'OT 10019 - 22/02/2017 - Carlos Espejo
                        'Descripcion: Se agrega el parametro CodigoOrden
                        CargarDatosOrdenInversion(Request.QueryString("PTNOrden"))
                        'OT 10019 Fin
                        tbNPoliza.Text = Right(txtCodigoOrden.Value, 5)
                        tbNPoliza.ReadOnly = True
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
                            If Session("ValidarFecha").ToString = "FECHADIFERENTE" Then CargarPaginaInicio()
                        End If
                        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                    Else
                        If (Me.hdPagina.Value = "OE") Then 'Viene de la Pagina Ordenes Excedidas
                            ControlarCamposOE()
                        Else
                            If (Me.hdPagina.Value = "DA") Then 'Viene de la Pagina Negociacion Dias Anteriores
                                ViewState("ORDEN") = "OI-DA"
                                Me.tbFechaOperacion.Text = Request.QueryString("Fecha")
                            Else
                                If (Me.hdPagina.Value = "CP") Then 'Viene de la Pagina Liquidaciones Cuentas Por Pagar
                                    ConfiguraModoConsulta()
                                    ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                    txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                    ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                    'OT 10019 - 22/02/2017 - Carlos Espejo
                                    'Descripcion: Se agrega el parametro CodigoOrden
                                    CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
                                    'OT 10019 Fin
                                    HabilitaBotones(False, False, False, False, False, False, False, False, False, False, True, False)
                                Else
                                    If hdPagina.Value = "CDP" Then 'Viene de la pagina "CorrecionDepositoPlazos"
                                        HabilitaBotones(False, False, False, False, False, True, False, True, False, True, False, False)
                                        HabilitaDeshabilitaCabecera(False)
                                        HabilitaDeshabilitaDatosOperacionComision(False)
                                        ddlIntermediario.Enabled = True
                                        txtPlazo.ReadOnly = True
                                        txtTasa.ReadOnly = False
                                        lblCodigoSBS.Visible = True
                                        tbCodigoSBS.Visible = True
                                        btnProcesar.Visible = True
                                        tbFechaContrato.ReadOnly = False
                                        ddlTipoTasa.Enabled = True
                                        imgFechaContrato.Attributes.Clear()
                                        imgFechaContrato.Attributes.Add("class", "input-append date")
                                        ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                        If Not Request.QueryString("PTOperacion") Is Nothing Then
                                            ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                        End If
                                        txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                        ddlTipoTitulo.SelectedValue = Request.QueryString("CodigoTipoTitulo")
                                        'OT 10019 - 22/02/2017 - Carlos Espejo
                                        'Descripcion: Se agrega el parametro CodigoOrden
                                        CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
                                        'OT 10019 Fin
                                        hdMensaje.Value = "la Correccion"
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                Me.btnSalir.Attributes.Remove("onClick")
                '   Me.btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                '   Me.btnAceptar.Attributes.Add("onClick", "if (Confirmacion()){this.disabled = true; this.value = 'en proceso...'; __doPostBack('btnAceptar','');}")
                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF49", txtCodigoOrden.Value + "?", "SI")
            Else
                HelpCombo.LlenarComboBox(ddlFondo, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "SELECCIONE")
            End If
        Else
            'Valores del Pop Up
            If Session("SS_DatosModal") IsNot Nothing Then ObtenerValoresDesdePopup()
            If Not Session("EstadoPantalla") Is Nothing Then
                If (Session("EstadoPantalla").ToString() = "Modificar" Or Session("EstadoPantalla").ToString() = "Eliminar") And (Request.QueryString("PTNeg") Is Nothing) Then
                    EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
                    If ddlMotivoCambio.Items.Count = 0 Then HelpCombo.CargarMotivosCambio(Me)
                End If
            End If
        End If
    End Sub
    Sub ObtenerValoresDesdePopup()
        Dim datosModal As String() = CType(Session("SS_DatosModal"), String())
        If hfModal.Value = "2" Then
            ddlFondo.SelectedValue = datosModal(3)
            txtCodigoOrden.Value = datosModal(6)
            'OT 9968 14/02/2017 - Carlos Espejo
            'Descripcion: Se asgina la orden para tenerla en memoria
            Session("Orden") = datosModal(6)
            'OT 9968 Fin 
        End If
        Session.Remove("SS_DatosModal")
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Call HabilitaBotones(False, False, False, False, False, False, False, False, False, False, True, False)
    End Sub
    Private Sub HabilitaBotones(ByVal bIngresar As Boolean, _
                                ByVal bModificar As Boolean, ByVal bEliminar As Boolean, ByVal bConsultar As Boolean, _
                                ByVal bAsignar As Boolean, ByVal bProcesar As Boolean, ByVal bImprimir As Boolean, _
                                ByVal bAceptar As Boolean, ByVal bBuscar As Boolean, ByVal bSalir As Boolean, _
                                ByVal bRetornar As Boolean, ByVal bLimitesParametrizados As Boolean)
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
        Session("ReporteLimitesEvaluados") = Nothing
        Session("dtdatosoperacion") = Nothing
        Session("accionValor") = Nothing
    End Sub
    Public Sub CargarIntermediario()
        ddlIntermediario.Items.Clear()
        UIUtility.CargarIntermediariosOISoloBancos(ddlIntermediario)
        Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
    End Sub
    Public Sub CargarTipoTitulo()
        Dim dtAux As DataTable
        dtAux = New TipoTituloBM().ListarPorDepositoPlazos(DatosRequest).Tables(0).Select("CodigoTipoInstrumentoSBS = 60").CopyToDataTable
        Session("datosTipoTitulo") = dtAux
        If dtAux.Rows.Count <> 0 Then
            ddlTipoTitulo.Items.Clear()
            ddlTipoTitulo.DataSource = dtAux
            ddlTipoTitulo.DataTextField = "Descripcion" : ddlTipoTitulo.DataValueField = "CodigoTipoTitulo" : ddlTipoTitulo.DataBind() : ddlTipoTitulo.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        Else
            ddlTipoTitulo.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlTipoTitulo)
        End If
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If ViewState("EstadoOperacion") = "Ingresar-Cancelacion" Or _
            ViewState("EstadoOperacion") = "Ingresar-Renovacion" Or _
            ViewState("EstadoOperacion") = "Ingresar-PreCancelacion" Or _
            Session("EstadoPantalla") = "Modificar" Or _
            Session("EstadoPantalla") = "Eliminar" Or _
            Session("EstadoPantalla") = "Consultar" Then
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
                If ViewState("EstadoOperacion") = "Ingresar-Cancelacion" And ddlOperacion.SelectedValue = "4" Then
                    'Si esta ingresando una cancelacion se debe listar las constituciones pero que esten solo confirmadas
                    ShowDialogPopupInversionesRealizadas(String.Empty, String.Empty, String.Empty, ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, "3", String.Empty, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString(), "X", "2")
                Else
                    ShowDialogPopupInversionesRealizadas(String.Empty, String.Empty, Me.ddlTipoTitulo.SelectedValue, ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, ddlOperacion.SelectedValue, String.Empty, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString(), strAccion, "2")
                End If
                Session("Busqueda") = 2
            Else
                If Session("Busqueda") = 1 Or ViewState("EstadoOperacion") = "Ingresar-PreCancelacion" Then
                    'OT 10019 - 22/02/2017 - Carlos Espejo
                    'Descripcion: Se agrega el parametro CodigoOrden
                    CargarDatosOrdenInversion(Session("Orden"))
                    'OT 10019 Fin
                    If Session("EstadoPantalla") = "Modificar" Or _
                       ViewState("EstadoOperacion") = "Ingresar-Cancelacion" Or _
                       ViewState("EstadoOperacion") = "Ingresar-Renovacion" Or _
                       ViewState("EstadoOperacion") = "Ingresar-PreCancelacion" Then
                        If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF2", "", "SI")
                        Else
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF16", "", "SI")
                        End If
                        If Session("EstadoPantalla") = "Modificar" Then
                            CargarPaginaModificar1()
                        Else : Session("EstadoPantalla") = "Ingresar"
                            CargarPaginaModificar2()
                        End If
                    ElseIf Session("EstadoPantalla") = "Eliminar" Then
                        If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF3", "", "SI")
                        Else
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF17", "", "SI")
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
            Case "CDP"
                strMensajeConfirmacion = "¿Desea cancelar la Corrección de la orden de inversión Nro. " + NroOrden + "?"
            Case Else
                If (strAccion <> String.Empty) Then
                    If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de la orden de inversión de Depósito a Plazos?"
                    Else
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de pre-orden de inversión de Depósito a Plazos?"
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
        txtMontoOperacion.Text = "0.00"
        ddlOperacion.SelectedIndex = 0
        'GUID: Identificador único para cuponetas temporales
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        Session("EstadoPantalla") = "Ingresar"
        CargarPaginaAccion()
        Session("Procesar") = 0
        Session("Busqueda") = 0
        If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
        Else
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
        End If
        lblAccion.Text = "Ingresar"
        CargarPaginaIngresar()
        hdMensaje.Value = "el Ingreso"
        hdNumUnidades.Value = 0
        btnBuscar.Visible = False
        If Not ddlFondo.Items.FindByValue(PORTAFOLIO_MULTIFONDOS) Is Nothing Then
            Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS
        End If
        lblTitulo.Text = "PreOrden de Inversión - DEPÓSITO A PLAZO"
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
        tbFechaPago.Text = tbFechaOperacion.Text
        Me.tbHoraOperacion.Text = objutil.RetornarHoraSistema

        'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 25/05/2018
        ddlOperacion.SelectedValue = "3"
        ddlOperacion.Enabled = False
        'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 25/05/2018
        HabilitarBotonGuardarPreOrden()

    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        LimpiarSesiones()
        ddlOperacion.SelectedIndex = 0
        'GUID: Identificador único para cuponetas temporales
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlOperacion)
        Session("EstadoPantalla") = "Modificar"
        Session("Busqueda") = 0
        Session("Procesar") = 0
        lblAccion.Text = "Modificar"
        CargarPaginaAccion()
        hdMensaje.Value = "la Modificación"
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
        Me.tbHoraOperacion.Text = objutil.RetornarHoraSistema
        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        LimpiarSesiones()
        ddlOperacion.SelectedIndex = 0
        Session("EstadoPantalla") = "Eliminar"
        lblAccion.Text = "Eliminar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Eliminación"
        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    Private Sub ConfiguraModoConsulta()
        Session("EstadoPantalla") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Consulta"
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        LimpiarSesiones()
        ddlOperacion.SelectedIndex = 0
        Call ConfiguraModoConsulta()
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
                    Session("Mercado") = dtAux.Rows(i)("mercado")
                    Exit For
                End If
            Next
        End If
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        Try
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
            Dim accionRpta As String = String.Empty
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
            If hdRptaConfirmar.Value.ToUpper = "SI" Then
                EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
                If validaFechas() Then
                    If Me.hdPagina.Value <> "" And Me.hdPagina.Value <> "DA" And Me.hdPagina.Value <> "TI" Then
                        If Me.hdPagina.Value = "XO" Then
                            oOrdenInversionWorkFlowBM.ExtornarOIEjecutadas(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, Me.DatosRequest)
                            ReturnArgumentShowDialogPopup()
                        Else
                            If Me.hdPagina.Value = "EO" Then
                                If Me.tbNPoliza.Text.Trim <> "" Then
                                    If Not validarPolizaExistencia(Me.txtCodigoOrden.Value, Me.tbNPoliza.Text, Me.ddlOperacion.SelectedValue) Then Exit Sub
                                End If
                                ModificarOrdenInversion()
                                oOrdenInversionWorkFlowBM.EjecutarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, Me.tbNPoliza.Text.Trim, Me.DatosRequest)
                                ReturnArgumentShowDialogPopup()
                            Else
                                If Me.hdPagina.Value = "CO" Then
                                    If Not validarPolizaExistencia(Me.txtCodigoOrden.Value, Me.tbNPoliza.Text, Me.ddlOperacion.SelectedValue) Then Exit Sub
                                    If (Trim(Me.tbFechaPago.Text) = "") Then
                                        AlertaJS("Ingresar la fecha de liquidación.")
                                        Exit Sub
                                    End If
                                    tbNPoliza.Text = Right(tbNPoliza.Text, 5)
                                    If (Trim(Me.tbNPoliza.Text.Length) > 5) Then
                                        AlertaJS("La Poliza no debe exceder los 5 dígitos.")
                                        Exit Sub
                                    End If
                                    ModificarOrdenInversion()
                                    oOrdenInversionWorkFlowBM.ConfirmarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, Me.tbNPoliza.Text.Trim, Me.DatosRequest)
                                    ReturnArgumentShowDialogPopup()
                                Else
                                    If Me.hdPagina.Value = "CDP" Then
                                        CorreccionDepositoPlazo()
                                        ReturnArgumentShowDialogPopup()
                                    End If
                                End If
                            End If
                        End If
                        If Me.hdPagina.Value = "EO" Or Me.hdPagina.Value = "CO" Then
                            'No se valida saldo disponible al confirmar, el problema detectado fue que el saldo se puso en cero en la ejecucion
                            If hdPagina.Value <> "CO" Then
                                ObtieneCustodiosSaldos()
                            End If
                            If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacionDP("CONS") Then 'CONSTITUCION, genera OI
                                ModificarOrdenInversion()
                            ElseIf ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacionDP("RENO") Then 'RENOVACION - actualizara campos nada mas
                                ModificarOrdenInversion()
                            End If
                            Session("Modificar") = 0
                            CargarPaginaAceptar()
                        End If
                    Else
                        If validacampos() Or Session("EstadoPantalla") = "Eliminar" Then
                            If Session("EstadoPantalla") = "Ingresar" Then
                                'Dim strcodigoOrden As String
                                ''insertar la orden de inversion
                                'strcodigoOrden = InsertarOrdenInversion()
                                'If strcodigoOrden <> "" Then
                                '    Dim toUser As String = ""
                                '    Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                                '    Dim dt As DataTable
                                '    If ViewState("EstadoOI") = "E-EXC" Then
                                '        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                                '        For Each fila As DataRow In dt.Rows
                                '            toUser = toUser + fila("Valor").ToString() & ";"
                                '        Next
                                '        UIUtility.EnviarMail(toUser, "", "Pendiente de aprobación - Orden excedido por Limites de Inversión", OrdenInversion.MensajeExcesosOI(strcodigoOrden, ddlFondo.SelectedValue.ToString(), DatosRequest), DatosRequest) 'CMB OT 62254 20110418
                                '    End If
                                '    'OT10795 - No se validan los límites en línea para el negocio Fondos Sura
                                '    'If PORTAFOLIO_ADMINISTRA <> ddlFondo.SelectedValue Then
                                '    '    Dim bResultado As Boolean
                                '    '    bResultado = New OrdenPreOrdenInversionBM().ValidarLineaNegociacionDPZ(Me.ddlTipoTitulo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), ddlFondo.SelectedValue, Me.ddlIntermediario.SelectedValue, Convert.ToDecimal(Me.txtMnomOp.Text.Replace(".", UIUtility.DecimalSeparator)), strcodigoOrden)   'HDG OT 62087 Nro14-R23 20110511
                                '    '    If bResultado Then
                                '    '        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                                '    '        For Each fila As DataRow In dt.Rows
                                '    '            toUser = toUser + fila("Valor").ToString() + ";"
                                '    '        Next
                                '    '        UIUtility.EnviarMail(toUser, "", "Exceso de Línea de negociación genérica", MensajeExcesoLineaNegociacion(strcodigoOrden), DatosRequest)
                                '    '    End If
                                '    'End If
                                'End If
                                'oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                                'If Session("EstadoPantalla") = "Ingresar" And (ViewState("EstadoOperacion") = "Ingresar-Cancelacion" Or ViewState("EstadoOperacion") = "Ingresar-PreCancelacion") Then
                                '    If Not ViewState("CodigoOrdenSeleccionado") Is Nothing Then
                                '        cancelarOrdenPreOrdenInvesion(ViewState("CodigoOrdenSeleccionado").ToString, strcodigoOrden)
                                '    End If
                                'End If
                                'Me.txtCodigoOrden.Value = strcodigoOrden
                                'Session("dtdatosoperacion") = ObtenerDatosOperacion()
                                'GenerarLlamado(strcodigoOrden, Me.ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, "DEPOSITOS A PLAZO", Me.ddlOperacion.SelectedItem.Text, txtMoneda.Text, Me.hdMnemonico.Value)
                                ''INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                                'accionRpta = "Ingresó"
                                ''FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                                'CargarPaginaAceptar()
                                'INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
                                GuardarPreOrden()
                                accionRpta = "Ingresó" 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18                                     
                                CargarPaginaInicio()
                                'FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
                            Else
                                Dim strAlerta As String = ""
                                If ddlMotivoCambio.SelectedIndex <= 0 Then
                                    strAlerta = "-Elija el motivo por el cual desea " & Session("EstadoPantalla") & " esta operación.\n"
                                End If
                                If strAlerta.Length > 0 Then
                                    AlertaJS(strAlerta)
                                    Exit Sub
                                End If
                                If Session("EstadoPantalla") = "Modificar" Or _
                                    ViewState("EstadoOperacion") = "Ingresar-Cancelacion" Or _
                                    ViewState("EstadoOperacion") = "Ingresar-Renovacion" Or _
                                    ViewState("EstadoOperacion") = "Ingresar-PreCancelacion" Then
                                    If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacionDP("CONS") Then        'CONSTITUCION, genera OI
                                        ModificarOrdenInversion()
                                        FechaEliminarModificarOI("M")
                                    ElseIf ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacionDP("CANC") Then    'CANCELACION - habilita solo Aceptar, generar otra OI
                                        Dim strcodigoOrden As String
                                        EliminarOrdenInversion()
                                        strcodigoOrden = InsertarOrdenInversion()
                                        Me.txtCodigoOrden.Value = strcodigoOrden
                                        FechaEliminarModificarOI("M")
                                        GenerarLlamado(strcodigoOrden, Me.ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, "DEPOSITOS A PLAZO", Me.ddlOperacion.SelectedItem.Text, txtMoneda.Text, Me.hdMnemonico.Value)
                                    ElseIf ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacionDP("RENO") Then   'RENOVACION - actualizara campos nada mas
                                        ModificarOrdenInversion()
                                        FechaEliminarModificarOI("M")
                                    ElseIf ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacionDP("PREC") Then    'PRECANCELACION - visualiza panel PreCancelacion, genera otra OI
                                        Dim strcodigoOrden As String
                                        EliminarOrdenInversion()
                                        strcodigoOrden = InsertarOrdenInversion()
                                        Me.txtCodigoOrden.Value = strcodigoOrden
                                        FechaEliminarModificarOI("M")
                                        GenerarLlamado(strcodigoOrden, Me.ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, "DEPOSITOS A PLAZO", Me.ddlOperacion.SelectedItem.Text, txtMoneda.Text, Me.hdMnemonico.Value)
                                    End If
                                    Session("Modificar") = 0
                                    CargarPaginaAceptar()
                                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
                                    accionRpta = "Modificó"
                                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
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
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                            Select Case Session("EstadoPantalla")
                                Case "Eliminar", "Modificar"
                                    retornarMensajeAccion(accionRpta)
                                Case "Ingresar"
                                    retornarMensajeAccionPreOrden(accionRpta)
                            End Select
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                        Else
                            AlertaJS(ObtenerMensaje("CONF5"))
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Function MensajeExcesoLineaNegociacion(ByVal numeroOrden As String) As String
        Dim mensaje As New StringBuilder
        Dim nroOrden As String = numeroOrden
        Dim fondo As String = ddlFondo.SelectedValue
        Dim operacion As String = ddlOperacion.SelectedItem.Text
        Dim orden As String = "Deposito a Plazos"
        Dim fecha As String = DateTime.Today.ToString("dd/MM/yyyy")
        Dim tipoTitulo As String = Me.ddlTipoTitulo.SelectedItem.Text
        Dim codigoNemonico As String = Me.ddlTipoTitulo.SelectedValue
        Dim intermediario As String = Me.ddlIntermediario.SelectedItem.Text
        With mensaje
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='550' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
            .Append("<tr><td colspan='3'>La siguiente orden emitido el " & fecha & ", ha superado la línea de negociación otorgada:</td></tr>")
            .Append("<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td width='35%'>Numero de Orden:</td>")
            .Append("<td colspan='2' width='65%'>" & nroOrden & "</td></tr>")
            .Append("<tr><td width='35%'>Fondo:</td><td colspan='2' width='65%'>" & fondo & "</td></tr>")
            .Append("<tr><td width='35%'>Tipo de Operación:</td><td colspan='2' width='65%'>" & operacion & "</td></tr>")
            .Append("<tr><td width='35%'>Tipo de Orden: </td><td colspan='2' width='65%'>" & orden & "</td></tr>")
            .Append("<tr><td width='35%'>Tipo T&iacute;tulo:</td><td colspan='2' width='65%'>" & tipoTitulo & "</td></tr>")
            .Append("<tr><td width='35%'>C&oacute;digo Nem&oacute;nico:</td><td colspan='2' width='65%'>" & codigoNemonico & "</td></tr>")
            .Append("<tr><td width='35%'>Intermediario:</td><td colspan='2' width='65%'>" & intermediario & "</td></tr>")
            .Append("<tr height='8'><td colspan='3'></td></tr>")
            .Append("<tr><td colspan='3'><strong>AFP Integra</strong></td></tr>")
            .Append("<tr><td colspan='3'><strong>Grupo Sura</strong></td></tr></table>")
        End With
        Return mensaje.ToString
    End Function
    Private Function validarPolizaExistencia(ByVal varCodigoOrden As String, ByVal vartbNPoliza As String, ByVal varddlOperacion As String) As Boolean
        Dim intCantPoliza As Integer
        Dim dsResulAux As DataSet = oOrdenInversionBM.validarPolizaExistencia(varCodigoOrden, vartbNPoliza, varddlOperacion, DatosRequest)
        If dsResulAux.Tables.Count > 0 Then
            If dsResulAux.Tables(0).Rows.Count > 0 Then
                intCantPoliza = Convert.ToInt32(dsResulAux.Tables(0).Rows(0)("CANT"))
                If intCantPoliza > 0 Then
                    AlertaJS(ObtenerMensaje("CONF46"))
                    Return False
                Else
                    Return True
                End If
            End If
        End If
        Return False
    End Function
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal cportafolio As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal mnemonico As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&cportafolio=" + cportafolio + "&vportafolio=" + portafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
    End Sub
    Public Function ObtenerDatosOperacion(Optional ByVal drOrden As DataRow = Nothing) As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        If drOrden Is Nothing Then
            drGrilla("v1") = Me.tbFechaOperacion.Text
            drGrilla("v2") = Me.tbFechaPago.Text
            drGrilla("v3") = Me.tbFechaContrato.Text
            drGrilla("v4") = Me.tbHoraOperacion.Text
            drGrilla("v5") = Me.txtPlazo.Text
            drGrilla("v6") = Me.txtMoneda.Text
            drGrilla("v7") = Me.txtMnomOp.Text
            drGrilla("v8") = Me.txtMnomOp.Text
            drGrilla("v9") = Me.ddlTipoTasa.SelectedItem.Text
            drGrilla("v10") = Me.txtTasa.Text
            drGrilla("v11") = Me.txtMontoOperacion.Text
            drGrilla("v12") = Me.ddlIntermediario.SelectedItem.Text
            drGrilla("v13") = txtObservacion.Text.ToUpper
            If Me.tbNPoliza.Visible = True Then
                drGrilla("c14") = "Número Poliza"
                drGrilla("v14") = Me.tbNPoliza.Text
            Else
                drGrilla("c14") = ""
                drGrilla("v14") = ""
            End If
            drGrilla("c15") = ""
            drGrilla("v15") = ""
            drGrilla("c16") = ""
            drGrilla("v16") = ""
            drGrilla("c17") = ""
            drGrilla("v17") = ""
            drGrilla("c18") = ""
            drGrilla("v18") = ""
            drGrilla("v19") = "0"
            drGrilla("v20") = "0"
        Else
            drGrilla("v1") = UIUtility.ConvertirFechaaString(drOrden("FechaOperacion"))
            drGrilla("v2") = UIUtility.ConvertirFechaaString(drOrden("FechaLiquidacion"))
            drGrilla("v3") = UIUtility.ConvertirFechaaString(drOrden("FechaContrato"))
            drGrilla("v4") = drOrden("HoraOperacion")
            drGrilla("v5") = drOrden("Plazo")
            drGrilla("v6") = drOrden("CodigoMoneda")
            drGrilla("v7") = Format(drOrden("MontoNominalOrdenado"), "#,##0.0000000")
            drGrilla("v8") = Format(drOrden("MontoNominalOperacion"), "#,##0.0000000")
            drGrilla("v9") = New TipoCuponBM().Seleccionar(drOrden("CodigoTipoCupon"), DatosRequest).Tables(0).Rows(0)("Descripcion")
            drGrilla("v10") = Format(drOrden("TasaPorcentaje"), "#,##0.0000000")
            drGrilla("v11") = Format(drOrden("MontoOperacion"), "#,##0.0000000")
            drGrilla("v12") = New TercerosBM().Seleccionar(drOrden("CodigoTercero"), DatosRequest).Tables(0).Rows(0)("Descripcion")
            drGrilla("v13") = drOrden("Observacion")
            drGrilla("v20") = Format(drOrden("MontoNetoOperacion"), "#,##0.0000000")
        End If
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("c3") = "Fecha Fin de Contrato"
        drGrilla("c4") = "Hora Operación"
        drGrilla("c5") = "Plazo"
        drGrilla("c6") = "Moneda"
        drGrilla("c7") = "Monto Nominal Ordenado"
        drGrilla("c8") = "Monto Nominal Operación"
        drGrilla("c9") = "Tipo Tasa"
        drGrilla("c10") = "Tasa %"
        drGrilla("c11") = "Monto Operación"
        drGrilla("c12") = "Intermediario"
        drGrilla("c13") = "Observación"
        drGrilla("c19") = "Total Comisiones"
        drGrilla("c20") = "Monto Neto Operación"
        drGrilla("c21") = ""
        drGrilla("v21") = ""
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
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
            Case "CDP"
                AlertaJS("Se Actualizó la orden correctamente", "window.close()")
            Case "MODIFICA"
                AlertaJS("Se Modificó la orden correctamente", "window.close()")
        End Select
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica el retorno de mensaje de confirmación | 06/06/18
    End Sub
    Private Sub CargarDatosOrdenInversion(CodigoOrden As String)
        Try
            UPcuerpo.Update()
            'OT 10019 - 22/02/2017 - Carlos Espejo
            'Descripcion: Se agrega el parametro CodigoOrden
            oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(CodigoOrden, Me.ddlFondo.SelectedValue, DatosRequest, PORTAFOLIO_MULTIFONDOS)
            'OT 10019 Fin
            Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
            oRow = oOrdenInversionBE.Tables(0).Rows(0)
            txtCodigoOrden.Value = oRow.CodigoOrden
            If (Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Then
                If oRow.CodigoOperacion.ToString <> "" Then
                    ddlOperacion.SelectedIndex = ddlOperacion.Items.IndexOf(ddlOperacion.Items.FindByValue(oRow.CodigoOperacion.ToString()))
                End If
            End If

            Session("Fechaop") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            txtPlazo.Text = oRow.Plazo.ToString.Replace(UIUtility.DecimalSeparator, ".")
            tbFechaPago.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
            ddlTipoTasa.SelectedValue = oRow.CodigoTipoCupon
            txtMnomOp.Text = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
            txtMontoOperacion.Text = Format(oRow.MontoOperacion, "##,##0.00")
            Try
                ddlIntermediario.SelectedValue = oRow.CodigoTercero
            Catch ex As Exception
                ddlIntermediario.SelectedIndex = 0
            End Try
            Me.hdMnemonico.Value = oRow.CodigoMnemonico
            CargarContactos()
            Me.txtMoneda.Text = oRow.CodigoMoneda
            ddlContacto.SelectedValue = oRow.CodigoContacto
            Me.txtTasa.Text = Format(oRow.TasaPorcentaje, "##,##0.0000")
            Me.tbHoraOperacion.Text = oRow.HoraOperacion
            txtObservacion.Text = oRow.Observacion
            'Inicio OT-10795
            If ddlOperacion.SelectedValue = 6 Then
                Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaContrato)
                Me.tbFechaContrato.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                Me.tbFechaOperacion.Enabled = False
                Me.tbFechaPago.Enabled = False
            Else
                Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                Me.tbFechaContrato.Text = UIUtility.ConvertirFechaaString(oRow.FechaContrato)
                Me.tbFechaOperacion.Enabled = True
                Me.tbFechaPago.Enabled = True
            End If
            'Fin OT-10795

            Me.tbCodigoSBS.Text = oRow.CodigoSBS
            ViewState("OrdenSeleccionadaOperacion") = oRow.CodigoOperacion.ToString
            ViewState("CodigoOrdenSeleccionado") = oRow.CodigoOrden.ToString()
            tbNPoliza.Text = oRow.NumeroPoliza.ToString()
            If IsDBNull(oRow.CodigoTipoTitulo) = False And oRow.CodigoTipoTitulo <> "" Then
                ddlTipoTitulo.SelectedValue = oRow.CodigoTipoTitulo
            Else
                ddlTipoTitulo.SelectedIndex = -1
            End If
            If oRow.Ficticia = "S" Then
                chkFicticia.Checked = True
            Else
                chkFicticia.Checked = False
            End If
            If oRow.Renovacion = "S" Then
                pnRenovacion.Visible = True
                pnBCR.Visible = True
            Else
                pnRenovacion.Visible = False
                pnBCR.Visible = True
            End If

            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga Motivo de Cambio si fuera el caso | 13/06/18 
            If oRow.CodigoMotivoCambio <> "" Then
                ddlMotivoCambio.SelectedValue = oRow.CodigoMotivoCambio
            End If
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga Motivo de Cambio si fuera el caso | 13/06/18 
        Catch ex As Exception
            If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                AlertaJS(ObtenerMensaje("CONF31"))
            Else
                AlertaJS(ObtenerMensaje("CONF32"))
            End If
        End Try
    End Sub
    Public Function cancelarOrdenPreOrdenInvesion(ByVal codigoOrdenCancelado As String, ByVal codigoOrdenGenerado As String) As Boolean
        oOrdenInversionWF.CancelarOI_POI(codigoOrdenCancelado, codigoOrdenGenerado, Me.DatosRequest)
        Return True
    End Function
    Public Function InsertarOrdenInversion() As String
        Dim strCodigoOI As String
        Dim oValoresBM As New ValoresBM
        Dim strCodigoReporte As String = oValoresBM.SeleccionarCorrelativoMnemonicoDP(Me.ddlTipoTitulo.SelectedValue, Me.ddlIntermediario.SelectedValue, DatosRequest).Tables(0).Rows(0)(0)
        oOrdenInversionBE = crearObjetoOI(strCodigoReporte)
        strCodigoOI = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
        Return strCodigoOI
    End Function
    Public Sub ModificarOrdenInversion()
        oOrdenInversionBE = crearObjetoOI(hdMnemonico.Value)
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, Me.hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
    End Sub
    Public Sub EliminarOrdenInversion()
        oOrdenInversionBM.EliminarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, ddlMotivoCambio.SelectedValue, DatosRequest)
    End Sub
    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(Me.ddlFondo.SelectedValue, Me.txtCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc, txtComentarios.Text, DatosRequest)
        txtComentarios.Text = ""
    End Sub
    Private Sub CorreccionDepositoPlazo()
        oOrdenInversionBM.CorreccionDepositoPlazo(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, Me.txtPlazo.Text, _
         Me.txtTasa.Text.Replace(".", UIUtility.DecimalSeparator), Me.tbCodigoSBS.Text, _
         Convert.ToDecimal(Me.txtMontoOperacion.Text.Replace(".", UIUtility.DecimalSeparator)), ddlTipoTasa.SelectedValue, _
         UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text), DatosRequest, ddlIntermediario.SelectedValue, ddlTipoTitulo.SelectedValue)
        oOrdenInversionBM.FechaModificarEliminarOI(Me.ddlFondo.SelectedValue, Me.txtCodigoOrden.Value, _
        Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), "M", "", DatosRequest)
    End Sub
    Private Sub GenerarCustodioAfectado()
        Dim dtAux As DataTable
        Dim i As Integer
        dtAux = CType(Session("datosEntidad"), DataTable)
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        For i = 0 To dtAux.Rows.Count - 1
            If dtAux.Rows(i)("CodigoCustodio") = ddlIntermediario.SelectedValue Then
                Session("ValorCustodio") = dtAux.Rows(i)("CodigoCustodio") + strSeparador + "1"
            End If
        Next
    End Sub
    Public Function crearObjetoOI(ByVal strCodigoNemonico As String) As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        oOrdenInversionBM.InicializarOrdenInversion(oRow)
        oRow.Fixing = Nothing
        oRow.MontoOrigen = Nothing
        oRow.CodigoOrden = Me.txtCodigoOrden.Value
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.CodigoTipoTitulo = Me.ddlTipoTitulo.SelectedValue
        oRow.CodigoMnemonico = strCodigoNemonico

        oRow.Plazo = Convert.ToDecimal(Me.txtPlazo.Text.Replace(".", UIUtility.DecimalSeparator))


        ' Inicio de Cambio OT-10795
        If ddlOperacion.SelectedValue = 6 Then 'Valor 6 hace referencia a la PRECANCELACION DE TITULOS UNICOS
            oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaPago.Text)
            oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text)
            oRow.FechaPago = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        Else
            oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaPago.Text)
            oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            oRow.FechaPago = UIUtility.ConvertirFechaaDecimal(tbFechaPago.Text)
            oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text)
        End If
        ' Fin de Cambio OT-10795

        oRow.CodigoTipoCupon = ddlTipoTasa.SelectedValue
        oRow.MontoNominalOrdenado = Convert.ToDecimal(Me.txtMnomOp.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.MontoNominalOperacion = Convert.ToDecimal(Me.txtMnomOp.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.MontoOperacion = Convert.ToDecimal(Me.txtMontoOperacion.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.Observacion = txtObservacion.Text.ToUpper
        oRow.CodigoMoneda = Me.txtMoneda.Text
        oRow.TasaPorcentaje = Me.txtTasa.Text.Replace(".", UIUtility.DecimalSeparator)
        oRow.Situacion = "A"
        oRow.MontoNetoOperacion = Convert.ToDecimal(Me.txtMontoOperacion.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.TotalComisiones = 0
        oRow.HoraOperacion = Me.tbHoraOperacion.Text
        If (Me.hdPagina.Value <> "XO") Then
            oRow.NumeroPoliza = Me.tbNPoliza.Text.ToString().Trim
        End If
        oRow.CategoriaInstrumento = "DP"
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
        If ViewState("EstadoOperacion") = "Ingresar-Renovacion" Then
            oRow.OrdenGenera = Me.txtCodigoOrden.Value
        Else
            oRow.OrdenGenera = Nothing
        End If
        If (chkFicticia.Checked) Then
            oRow.Ficticia = "S"
        Else
            oRow.Ficticia = "N"
        End If
        oRow.Renovacion = "N"
        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
    Public Function validacampos() As Boolean
        Dim intvalida As Integer = 1
        Dim strError As String
        strError = "Los siguientes campos son obligatorios:\n"
        If (intvalida = 0) Then
            AlertaJS(strError)
            Return False
        ElseIf Not ValidarFechaVencimiento() Then
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos()
    End Sub
    Private Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            If validaFechas() = False Then
                Exit Sub
            End If
            'OT 10238 - 07/04/2017 - Carlos Espejo
            'Descripcion: Se agrega mensaje de seleccione portafolio.
            If oOrdenInversionBM.ValidarCodigoValor(ddlTipoTitulo.SelectedValue, ddlIntermediario.SelectedValue, ddlTipoTasa.SelectedValue) = 0 Then
                AlertaJS("No existe relación con algún Código Valor para estos datos. Se debe corregir la información o informar a custodia para crear la relación.")
            End If
            'OT 10238 Fin
            Session("Procesar") = 1
            Dim GUID As String = System.Guid.NewGuid.ToString()
            If Me.hdPagina.Value <> "TI" Then
                Dim oLimiteEvaluacion As New LimiteEvaluacionBM
                Dim dsAux As New DataSet
                Dim codigoOperacion As String = ddlOperacion.SelectedValue.ToString()
                Dim codigoNemonico As String = ""
                Dim cantidadValor As Decimal = 0
                Dim montoNominal As Decimal = Convert.ToDecimal(Me.txtMnomOp.Text.ToString())
                Dim codigoPortafolio As String = ddlFondo.SelectedValue.ToString()
                Dim fechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.ToString())
                dsAux = oLimiteEvaluacion.Evaluar(codigoOperacion, codigoNemonico, cantidadValor, montoNominal, codigoPortafolio, fechaOperacion, Me.DatosRequest)
                Session(GUID) = dsAux
                ViewState("GUID_Limites") = GUID
                If Not dsAux Is Nothing And dsAux.Tables.Count > 0 Then
                    If (dsAux.Tables(0).Rows.Count > 0) Then
                        Session("Instrumento") = "DEPÓSITO PLAZOS"
                        ViewState("EstadoOI") = "L-EXC"
                        EjecutarJS(UIUtility.MostrarPopUp("frmConsultaLimitesInstrumento.aspx?GUID=" + GUID, "no", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                    Else
                        ViewState("EstadoOI") = ""
                    End If
                End If
            End If
            Dim dtAux As DataTable
            Dim i As Integer
            dtAux = CType(Session("datosTipoTitulo"), DataTable)
            Dim BaseTir As Integer = 360
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("CodigoTipoTitulo") = Me.ddlTipoTitulo.SelectedValue Then
                    Session("TipoRenta") = dtAux.Rows(i)("CodigoTipoRenta")
                    BaseTir = dtAux.Rows(i)("BaseTir")
                    Exit For
                End If
            Next
            Dim fecha As String
            Dim intdiferenciadias As Integer
            Dim dbMontoOperacion As Decimal
            Dim diff As TimeSpan
            diff = Convert.ToDateTime(tbFechaContrato.Text).Subtract(Convert.ToDateTime(tbFechaOperacion.Text))
            txtPlazo.Text = diff.Days
            If ddlTipoTitulo.SelectedValue = "DPZNSOL365B" Or ddlTipoTitulo.SelectedValue = "DPZDOL365B" Then
                'Años bisiesto - Se usa la misma formula que se usara en el vencimiento del deposito
                Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
                txtMontoOperacion.Text = oOrdenInversionWorkFlowBM.CalculoDPZBisiesto(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
                UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text), CDec(txtMnomOp.Text), CDec(txtTasa.Text), ddlTipoTasa.SelectedValue)
            ElseIf (Me.ddlTipoTasa.SelectedValue = "1") Then
                txtMontoOperacion.Text = ((Convert.ToDecimal(txtMnomOp.Text) * ((Convert.ToDecimal(txtTasa.Text) / 100) / BaseTir) * Convert.ToDecimal(txtPlazo.Text)) + Convert.ToDecimal(txtMnomOp.Text))
            Else
                txtMontoOperacion.Text = Convert.ToDecimal(txtMnomOp.Text) * Math.Pow((1 + (Convert.ToDecimal(txtTasa.Text) / 100)), (Convert.ToDecimal(txtPlazo.Text) / BaseTir))
            End If
            If ViewState("EstadoOperacion") = "Ingresar-PreCancelacion" Then
                fecha = Date.Now.ToString("dd/MM/yyyy")
                intdiferenciadias = DateDiff(DateInterval.Day, CDate(Session("Fechaop")), CDate(fecha))
                If (Me.ddlTipoTasa.SelectedValue = "1") Then
                    dbMontoOperacion = Format(((Convert.ToDecimal(txtMnomOp.Text) * ((Convert.ToDecimal(txtTasa.Text) / 100) / BaseTir) * intdiferenciadias) + Convert.ToDecimal(txtMnomOp.Text)), "##,##0.0000000")
                Else
                    dbMontoOperacion = Format(Convert.ToDecimal(txtMnomOp.Text) * Math.Pow((1 + (Convert.ToDecimal(txtTasa.Text) / 100)), (intdiferenciadias / BaseTir)), "##,##0.0000000")
                End If
            End If
            Dim ds As DataSet
            Dim Mensaje As String
            ds = New OrdenPreOrdenInversionBM().ValidacionPuntual_LimitesTrading(ddlTipoTitulo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), ddlFondo.SelectedValue, Convert.ToDecimal(txtMnomOp.Text.Replace(",", "").Replace(".", UIUtility.DecimalSeparator)), Me.txtMoneda.Text, Usuario.ToString.Trim, CLASE_INSTRUMENTO_DEPOSITOPLAZO)
            If ds.Tables(0).Rows.Count > 0 Then
                Mensaje = "El usuario no esta permitido grabar la operación, el monto de negociado excede el límite de autonomía por Trader:\n\n"
                For Each fila As DataRow In ds.Tables(0).Rows
                    Mensaje = Mensaje & "- Usuario (" & fila("TipoCargoExc") & ") excedió límite de autonomía \""" & fila("GrupoLimTrd") & "\"", debe ser autorizado por un usuario " & fila("TipoCargoAut") & " (" & fila("TraderAut") & "). \n\n"
                Next
                Mensaje = Mensaje & "La operación debe ser grabada por el usuario autorizado haciendo clic en el botón Aceptar de la orden de inversión."
                AlertaJS(Mensaje)
                Session("dtValTrading") = ds.Tables(0)
            End If
            If hdPagina.Value <> "CO" Then
                ObtieneCustodiosSaldos()
            End If
            CargarPaginaProcesar()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub ObtieneCustodiosSaldos()
        'CUSTODIOS, CARTERA y KARDEX
        If Session("EstadoPantalla") = "Ingresar" Then
            GenerarCustodioAfectado()
        End If
    End Sub
    Private Function validaFechas() As Boolean
        If UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text) > UIUtility.ConvertirFechaaDecimal(Me.tbFechaPago.Text) Then
            AlertaJS(ObtenerMensaje("CONF7"))
            Return False
        ElseIf Not ValidarFechaVencimiento() Then
            Return False
        ElseIf ddlOperacion.SelectedValue = "3" And UIUtility.ConvertirFechaaDecimal(Me.tbFechaContrato.Text) <= UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text) Then
            AlertaJS(ObtenerMensaje("CONF7"))
            Return False
        Else
            Return True
        End If
    End Function
#Region " /* Métodos Personalizados (Popups Dialogs) */ "
    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal cfondo As String, ByVal fondo As String, ByVal operacion As String, ByVal moneda As String, ByVal fecha As String, ByVal accion As String, ByVal valor As String)
        Dim strURL As String = "../frmInversionesRealizadas.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&cFondo=" & cfondo & "&vFondo=" & fondo & "&vOperacion=" & operacion & "&vFechaOperacion=" & fecha & "&vAccion=" & accion & "&vCategoria=DP"
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
        Dim dtAux As DataTable = oPortafolioBM.SeleccionarPortafolioPorFiltro(Me.ddlFondo.SelectedValue, DatosRequest).Tables(0)
        If Not dtAux Is Nothing Then
            If dtAux.Rows.Count > 0 Then
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaConstitucion")))
            End If
        End If
        Me.tbFechaPago.Text = Me.tbFechaOperacion.Text
        tbHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Ingresar"
        hdMensaje.Value = "el Ingreso"
        If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
        Else
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
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
            Me.btnAceptar.Enabled = True
            Session("EstadoPantalla") = "Modificar"
            Dim GUID As Guid = System.Guid.NewGuid()
            ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
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
        Me.btnProcesar.Visible = True
        Me.btnProcesar.Enabled = True
    End Sub
    Private Sub CargarPaginaIngresar()
        CargarPaginaBuscar()
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
    End Sub
    Private Sub CargarPaginaModificar1()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        Me.txtComentarios.Text = ""
    End Sub
    Private Sub CargarPaginaModificar2()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
    End Sub
    Private Sub CargarPaginaEliminar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        CargarPaginaProcesar()
        Me.txtComentarios.Text = ""
    End Sub
    Private Sub CargarPaginaConsultar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
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
        LimpiarDatosOperacion()
        HabilitaDeshabilitaCabecera(True)
        Me.btnBuscar.Visible = True
        Me.btnBuscar.Enabled = True
        Session("ValorCustodio") = ""
    End Sub
    Private Sub HabilitaDeshabilitaCabecera(ByVal estado As Boolean)
        ddlFondo.Enabled = estado
        ddlOperacion.Enabled = estado
        ddlTipoTitulo.Enabled = estado
    End Sub
    Private Sub HabilitaDeshabilitaDatosOperacionComision(ByVal estado As Boolean)
        If estado Then
            imgFechaContrato.Attributes.Add("class", "input-append date")
        Else
            imgFechaContrato.Attributes.Add("class", "input-append")
        End If
        txtPlazo.ReadOnly = Not estado
        tbFechaContrato.ReadOnly = Not estado
        ddlTipoTasa.Enabled = estado
        txtTasa.ReadOnly = Not estado
        txtMnomOp.ReadOnly = Not estado
        tbFechaOperacion.ReadOnly = Not estado
        txtMontoOperacion.ReadOnly = Not estado
        ddlIntermediario.Enabled = estado
        ddlContacto.Enabled = estado
        txtObservacion.ReadOnly = Not estado
        tbHoraOperacion.ReadOnly = Not estado
        gvDatos.DataSource = Nothing
        gvDatos.DataBind()
        pnRenovacion.Visible = False
        pnBCR.Visible = False
        ' chkRenovacion.Checked = False
    End Sub
    Private Sub OcultarBotonesInicio()
        Me.btnBuscar.Visible = False
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
    Private Sub LimpiarDatosOperacion()
        Me.ddlFondo.SelectedIndex = 0
        Me.ddlOperacion.SelectedIndex = 0
        Me.ddlTipoTitulo.SelectedIndex = 0
        Me.tbFechaOperacion.Text = ""
        Me.txtPlazo.Text = ""
        Me.tbFechaPago.Text = ""
        Me.tbFechaContrato.Text = ""
        Me.ddlTipoTasa.SelectedIndex = 0
        Me.txtTasa.Text = ""
        Me.txtMnomOp.Text = ""
        Me.txtMoneda.Text = ""
        Me.txtMontoOperacion.Text = ""
        Me.ddlIntermediario.SelectedIndex = 0
        CargarContactos()
        Me.ddlContacto.SelectedIndex = 0
        Me.txtCodigoOrden.Value = ""
        Me.txtObservacion.Text = ""
        Me.tbHoraOperacion.Text = ""
    End Sub
#End Region
    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, "DEPOSITOS A PLAZO", Me.ddlOperacion.SelectedItem.Text, txtMoneda.Text, "")
    End Sub
    Private Sub ddlTipoTitulo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoTitulo.SelectedIndexChanged
        Dim i As Integer
        Dim dtAux As DataTable
        dtAux = Session("datosTipoTitulo")
        If ddlTipoTitulo.SelectedIndex <> 0 Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("CodigoTipoTitulo") = Me.ddlTipoTitulo.SelectedValue Then
                    Me.txtMoneda.Text = dtAux.Rows(i)("CodigoMoneda")
                    'Me.ddlTipoTasa.SelectedValue = IIf(dtAux.Rows(i)("CodigoTipoCupon") Is DBNull.Value, "", dtAux.Rows(i)("CodigoTipoCupon"))
                    Me.ddlTipoTasa.SelectedValue = "2"
                    Session("TipoRenta") = dtAux.Rows(i)("CodigoTipoRenta")
                    Me.tbFechaPago.Text = Me.tbFechaOperacion.Text
                End If
            Next
        Else
            Me.txtMoneda.Text = ""
            Me.ddlTipoTasa.SelectedIndex = 0
        End If
    End Sub
    Private Function obtenerCodigoEmisor(ByVal codigoIntermediario As String) As String
        Dim oEntidadBM As New EntidadBM
        Dim oDT As DataTable
        Dim strAux As String = ""
        oDT = oEntidadBM.SeleccionarPorCodigoTercero(codigoIntermediario, DatosRequest).Tables(0)
        If Not oDT Is Nothing Then
            If oDT.Rows.Count > 0 Then
                strAux = oDT.Rows(0)(0)
            End If
        End If
        Return strAux
    End Function
    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
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
        Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        If cantidadreg > 0 And Session("EstadoPantalla") <> "Consultar" Then
            AlertaJS("Ya existe una valorización para esta este fondo en esta fecha, debe extornarla.")
            ddlFondo.SelectedIndex = 0
            Exit Sub
        End If
        tbFechaPago.Text = tbFechaOperacion.Text
        chkFicticia.Enabled = False
        If (Not Session("EstadoPantalla") Is Nothing) Then
            If Session("EstadoPantalla") = "Ingresar" And Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                chkFicticia.Enabled = True
            End If
        End If
    End Sub
    Private Sub tbFechaOperacion_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFechaOperacion.TextChanged
        If (tbFechaOperacion.Text <> "") And (txtPlazo.Text <> "") Then
            Dim Fecha As Date
            Fecha = tbFechaOperacion.Text.Trim()
            Fecha = Fecha.AddDays(Me.txtPlazo.Text.Trim())
            Me.tbFechaContrato.Text = Fecha.ToShortDateString
        Else
            Me.tbFechaContrato.Text = ""
        End If
    End Sub
    Private Sub txtPlazo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPlazo.TextChanged
        If (tbFechaOperacion.Text <> "") And (txtPlazo.Text <> "") Then
            Dim Fecha As Date
            Fecha = tbFechaOperacion.Text.Trim()
            Fecha = Fecha.AddDays(Me.txtPlazo.Text.Trim())
            Me.tbFechaContrato.Text = Fecha.ToShortDateString
            ValidarFechaVencimiento()
        Else
            Me.tbFechaContrato.Text = ""
        End If
    End Sub
    Private Function ValidarFechaVencimiento() As Boolean
        Dim dia As String = ""
        Dim Fecha As Date = tbFechaContrato.Text
        If Fecha.DayOfWeek = DayOfWeek.Saturday Then
            dia = "Sábado"
        ElseIf Fecha.DayOfWeek = DayOfWeek.Sunday Then
            dia = "Domingo"
        ElseIf ddlIntermediario.SelectedIndex > 0 Then
            dia = New UtilDM().ValidarFechaHabil(Fecha.ToString("yyyyMMdd"), ddlIntermediario.SelectedValue)
        End If
        If dia.Equals("") Then
            Return True
        Else
            AlertaJS("La fecha fin de contrato (Vencimiento) es " & dia & ". Debe cambiar los días de plazo.")
            Return False
        End If
    End Function

    Private Sub agregarJavaScript()
        Me.btnSalir.Attributes.Add("onclick", "return Salida();")
    End Sub
    Private Function btnLimites() As Object
        Throw New NotImplementedException
    End Function
    Protected Sub tbFechaContrato_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFechaContrato.TextChanged
        If (tbFechaOperacion.Text <> "") And (txtPlazo.Text <> "") And (tbFechaContrato.Text.Trim() <> "") Then
            Dim fechaOperacion, fechaContrato As Date
            Dim difDias As Int32
            fechaOperacion = tbFechaOperacion.Text.Trim()
            fechaContrato = tbFechaContrato.Text.Trim()
            difDias = DateDiff(DateInterval.Day, fechaOperacion, fechaContrato)
            txtPlazo.Text = difDias
            txtPlazo_TextChanged(Nothing, Nothing)
        End If
    End Sub
    Protected Sub gvDatos_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDatos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim LBCodigoConstitucion As Label = DirectCast(e.Row.FindControl("lbCodigoConstitucion"), Label)
            Dim chkSelect As CheckBox = DirectCast(e.Row.FindControl("chkSelect"), CheckBox)
            If LBCodigoConstitucion.Text = txtCodigoOrden.Value Then
                chkSelect.Checked = True
            End If
        End If
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF008 - Se implementa las funciones para el guardado en PreOrden  | 17/08/18 
#Region "Registro En la Preorden"
    Sub GuardarPreOrden()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII | 2018-08-17 | Guardado en Pre Orden Inversion

        Dim entPreOrden As New PrevOrdenInversionBE
        Dim negPreOrden As New PrevOrdenInversionBM
        Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow = CType(entPreOrden.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
        Dim oValoresBM As New ValoresBM
        ' Dim strCodigoReporte As String = oValoresBM.SeleccionarCorrelativoMnemonicoDP(Me.ddlTipoTitulo.SelectedValue, Me.ddlIntermediario.SelectedValue, DatosRequest).Tables(0).Rows(0)(0)

        negPreOrden.InicializarPrevOrdenInversion(oRow)

        oRow.CodigoPrevOrden = 0
        oRow.CodigoOperacion = ddlOperacion.SelectedValue 'Compra/Venta/Etc.
        oRow.CodigoNemonico = ddlTipoTitulo.SelectedValue
        '  oRow.IndPrecioTasa = ddlModoNegociacion.SelectedValue 'T: Tasa YTM % , P: Precio
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.HoraOperacion = Now.ToLongTimeString()
        oRow.ClaseInstrumentoFx = "DP"
        oRow.CodigoTipoTitulo = Me.ddlTipoTitulo.SelectedValue

        oRow.MontoNominal = CType(txtMnomOp.Text, Decimal)
        oRow.Tasa = CType(txtTasa.Text, Decimal)
        oRow.TipoTasa = ddlTipoTasa.SelectedValue
        oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.MontoOperacion = Convert.ToDecimal(Me.txtMontoOperacion.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.Cantidad = 0
        oRow.CantidadOperacion = 0
        oRow.CodigoPlaza = "4"

        'Valores por Defecto          
        oRow.MedioNegociacion = "E" 'Por defecto 'ELECTRONICO'
        'oRow.TipoFondo = "Normal" 'Por defecto 'NORMAL'
        '  oRow.TipoTramo = "AGENCIA" 'Por defecto 'AGENCIA'
        ' oRow.TipoCondicion = "PL" 'Por defecto 'PRECIO LÍMITE'
        oRow.Porcentaje = "N" 'N: No Porcentaje, solo Monto directo
        oRow.Fixing = 0 'Por defecto 
        oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
        oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO

        entPreOrden.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
        entPreOrden.PrevOrdenInversion.AcceptChanges()

        Dim dtAsignacion As New DataTable ' Asignacion Por Fondo
        dtAsignacion.Columns.Add("CodigoPortafolio")
        dtAsignacion.Columns.Add("Asignacion")
        'Solo necesitamos una Fila donde se indicará el 100% de unidades para el Fondo
        dtAsignacion.Rows.Add(ddlFondo.SelectedValue, oRow.MontoNominal)

        'Guardamos la Pre-Orden
        negPreOrden.Insertar(entPreOrden, ParametrosSIT.TR_DERIVADOS.ToString, DatosRequest, dtAsignacion)

        HabilitarBotonesAccion()

        'FIN | ZOLUXIONES | RCE | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
    End Sub
    Sub HabilitarBotonGuardarPreOrden()
        Me.btnIngresar.Visible = False
        Me.btnModificar.Visible = False
        Me.btnEliminar.Visible = False
        Me.btnConsultar.Visible = False
        Me.btnAceptar.Visible = True
        UIUtility.ResaltaCajaTexto(txtPlazo, True)
        UIUtility.ResaltaCajaTexto(txtMnomOp, True)
        UIUtility.ResaltaCajaTexto(txtTasa, True)
        UIUtility.ResaltaCajaTexto(tbFechaContrato, True)
        UIUtility.ResaltaCombo(ddlTipoTitulo, True)
        UIUtility.ResaltaCombo(ddlFondo, True)
        UIUtility.ResaltaCombo(ddlTipoTasa, True)
        UIUtility.ResaltaCombo(ddlIntermediario, True)
    End Sub
    Sub HabilitarBotonesAccion()
        Me.btnIngresar.Visible = True
        Me.btnModificar.Visible = True
        Me.btnEliminar.Visible = True
        Me.btnConsultar.Visible = True

        Me.btnAceptar.Visible = False
    End Sub
#End Region
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF008 - Se implementa las funciones para el guardado en PreOrden  | 17/08/18 
End Class
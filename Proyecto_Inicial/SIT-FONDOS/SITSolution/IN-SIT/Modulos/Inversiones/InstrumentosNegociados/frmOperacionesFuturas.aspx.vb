Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmOperacionesFuturas
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
    Dim oDSLimites As New DataSet
#End Region
#Region "Inicializacion"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Me.hdSaldo.Value = 0
        Me.btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
        If (Not Session("SS_DatosModal") Is Nothing) Then
            Dim datosModal As String() = CType(Session("SS_DatosModal"), String())
            If (ToNullInt32(hfModal.Value) = 1) Then
                txtISIN.Text = datosModal(0)
                txtMnemonico.Text = datosModal(1)
                txtSBS.Text = datosModal(2)
                hdCustodio.Value = datosModal(3)
                hdSaldo.Value = datosModal(4)
            ElseIf (ToNullInt32(hfModal.Value) = 2) Then
                txtISIN.Text = datosModal(0)
                txtSBS.Text = datosModal(1)
                txtMnemonico.Text = datosModal(2)
                ddlFondo.SelectedValue = datosModal(3)
                ddlOperacion.SelectedValue = datosModal(4)
                lblMoneda.Text = datosModal(5)
                txtCodigoOrden.Value = datosModal(6)
            End If
            Session.Remove("SS_DatosModal")
        End If
        If Not Page.IsPostBack Then
            Try
                Me.btnProcesar.Attributes.Add("onclick", "javascript:return Validar();")
                Me.btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
                btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
                LimpiarSesiones()
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    Me.hdPagina.Value = Request.QueryString("PTNeg")
                End If

                If (Me.hdPagina.Value = "TI") Then
                    UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
                Else
                    UIUtility.CargarOperacionOI(ddlOperacion, "Futuro")
                End If
                CargarPaginaInicio()
                Me.hdPagina.Value = ""
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    UIUtility.CargarPortafoliosOI(ddlFondo)
                    Me.hdPagina.Value = Request.QueryString("PTNeg")
                    If Me.hdPagina.Value = "TI" Then  'Viene de la Pagina Traspaso de Instrumentos
                        Me.txtMnemonico.Text = Request.QueryString("PTCMnemo")
                        Me.ddlFondo.SelectedValue = Request.QueryString("PTFondoO")
                        Me.ddlFondoDestino.SelectedValue = Request.QueryString("PTFondoD")
                        Me.ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                        Me.txtISIN.Text = Request.QueryString("PTISIN")
                        Me.txtSBS.Text = Request.QueryString("PTSBS")
                        Me.lblMoneda.Text = Request.QueryString("PTMon")
                        Me.ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                        Me.hdCustodio.Value = Request.QueryString("PTCustodio")
                        Me.hdSaldo.Value = Request.QueryString("PTSaldo")
                        CargarCaracteristicasValor()
                        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
                        ControlarCamposTI()
                        Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(Request.QueryString("fechaOperacion")))
                    Else
                        Me.txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                        Me.ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                        If (Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Then 'Viene de la Pagina Confirmacion , Ejecucion  o Extorno de Ordenes de Inversion
                            CargarDatosOrdenInversion(txtCodigoOrden.Value)
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

                            UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue) 'Modificado por LC 20081023
                        Else
                            If (Me.hdPagina.Value = "OE") Then 'Viene de la Pagina Ordenes Excedidas
                                ControlarCamposOE()
                            Else
                                If (Me.hdPagina.Value = "DA") Then 'Viene de la Pagina Negociacion Dias Anteriores
                                    ViewState("ORDEN") = "OI-DA"
                                    Me.tbFechaOperacion.Text = Request.QueryString("Fecha")
                                    Me.tbFechaOperacion.ReadOnly = True

                                    imgFechaOperacion.Attributes.Remove("class")
                                    imgFechaOperacion.Attributes.Add("class", "input-append")

                                    ControlarCamposDA()
                                Else
                                    If (Me.hdPagina.Value = "CP") Then 'Viene de la Pagina Liquidaciones Cuentas Por Pagar
                                        Call ConfiguraModoConsulta()
                                        ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                        txtMnemonico.Text = Request.QueryString("Mnemonico")
                                        txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                        Call CargarDatosOrdenInversion(txtCodigoOrden.Value)
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
                                                HelpCombo.CargarMotivosCambio(Me)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Else
                    'HelpCombo.CargaPortafolioSegunUsuario(ddlFondo, Usuario)
                    '*********************** Developer by Carlos Hernández ******************************
                    Dim oPortafolio = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
                    HelpCombo.LlenarComboBox(ddlFondo, oPortafolio, "CodigoPortafolio", "Descripcion", True)

                    '************************************************************************************
                End If
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try

        End If

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Call HabilitaBotones(True, True, True, True, True, True, True, True, True, True, True, False, True, False)
    End Sub

#End Region

#Region "Eventos Objetos"

    Private Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click

        LimpiarSesiones()
        Me.txtPrecio.Text = "0.0000000"
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
        hdNumUnidades.Value = 0

        If Not ddlFondo.Items.FindByValue("MULTIFONDO") Is Nothing Then 'RGF 20081001
            Me.ddlFondo.SelectedValue = "MULTIFONDO"
        End If

        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row hidden")
    End Sub

    Sub LlenarSesionContextInfo()
        Dim tablaParametros As New Hashtable

        If (ddlFondo.SelectedIndex >= 0) Then
            tablaParametros("codPortafolio") = ddlFondo.SelectedValue
            tablaParametros("Portafolio") = ddlFondo.SelectedItem.Text
        End If

        ' context_info ==> Información del Contexto Actual (Hashtable de preferencia)
        Session("context_info") = tablaParametros
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If (Convert.ToString(Session("EstadoPantalla")).Equals("Ingresar")) Then
            If (ToNullInt32(Session("Busqueda")) = 0) Then
                If Me.ddlFondo.SelectedValue = "" Then
                    If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                        AlertaJS(ObtenerMensaje("CONF42"))
                    ElseIf Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
                        AlertaJS(ObtenerMensaje("CONF43"))
                        Exit Sub
                    End If
                End If
                ShowDialogPopupValores(txtISIN.Text.Trim().ToUpper(), txtSBS.Text.Trim().ToUpper(), txtMnemonico.Text.Trim().ToUpper(), ddlFondo.SelectedValue, ddlOperacion.SelectedValue, "FT", (1).ToString())
                Session("Busqueda") = 2
            Else
                If (ToNullInt32(Session("Busqueda")) = 1) Then
                    CargarFechaVencimiento()
                    If (Not Me.ddlFondo.SelectedItem.Text.ToUpper().Equals("MULTIFONDO")) Then
                        UIUtility.AsignarMensajeBoton(btnAceptar, "CONF1")
                    Else
                        UIUtility.AsignarMensajeBoton(btnAceptar, "CONF15")
                    End If
                    CargarPaginaIngresar()
                    CargarCaracteristicasValor()
                    CargarIntermediario()
                    CargarPlaza()
                    CargarVencimientoMes()
                    CargarCondicion()

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
                    Dim strAux As String = String.Empty
                    If (Me.hdPagina.Value = "DA") Then
                        tbFechaOperacion.Text = Request.QueryString("Fecha")
                        strAux = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString()
                    End If
                    Dim strAccion As String = ""
                    If Session("EstadoPantalla") = "Modificar" Then
                        strAccion = "M"
                    ElseIf Session("EstadoPantalla") = "Eliminar" Then
                        strAccion = "E"
                    ElseIf Session("EstadoPantalla") = "Consultar" Then
                        strAccion = "C"
                    End If
                    Me.LlenarSesionContextInfo()
                    ShowDialogPopupInversionesRealizadas(GetISIN, GetSBS, GetMNEMONICO, GetFONDO, GetOPERACION, GetMONEDA, strAux, strAccion, (2).ToString())
                    Session("Busqueda") = 2
                Else
                    Dim m As Hashtable
                    If Session("Busqueda") = 1 Then
                        CargarCaracteristicasValor()
                        CargarDatosOrdenInversion(txtCodigoOrden.Value)
                        UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                        Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                        Me.btnAceptar.Enabled = True
                        If Session("EstadoPantalla") = "Modificar" Then
                            If Me.ddlFondo.SelectedValue <> "MULTIFONDO" Then
                                UIUtility.AsignarMensajeBoton(btnAceptar, "CONF2", "Nro " + txtSBS.Text + "?")
                            Else
                                UIUtility.AsignarMensajeBoton(btnAceptar, "CONF16", "Nro " + txtSBS.Text + "?")
                            End If
                            CargarPaginaModificar()
                        ElseIf Session("EstadoPantalla") = "Eliminar" Then
                            If Me.ddlFondo.SelectedValue <> "MULTIFONDO" Then
                                UIUtility.AsignarMensajeBoton(btnAceptar, "CONF3", "Nro " + txtSBS.Text + "?")
                            Else
                                UIUtility.AsignarMensajeBoton(btnAceptar, "CONF17", "Nro " + txtSBS.Text + "?")
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
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        Try
            If CDec(txtNumConOperacion.Text) <= 0 Then
                AlertaJS("Ingrese una cantidad de acciones mayor a 0")
                Exit Sub
            End If
            If Me.hdPagina.Value <> "" And Me.hdPagina.Value <> "DA" And Me.hdPagina.Value <> "TI" And Me.hdPagina.Value <> "MODIFICA" Then
                If Me.hdPagina.Value = "EO" Or Me.hdPagina.Value = "CO" Then
                    If hdPagina.Value <> "CO" Then
                        ObtieneCustodiosSaldos()
                    End If
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
                If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
                    Dim strAlerta As String = ""
                    If ddlMotivoCambio.SelectedIndex <= 0 Then
                        strAlerta = "-Elija el motivo por el cual desea " & Session("EstadoPantalla") & " esta operación.\n"
                    End If
                    If txtComentarios.Text.Trim().Length <= 0 Then
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
                            If ddlFondo.SelectedValue <> "MULTIFONDO" Then
                                Dim toUser As String = ""
                                Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                                Dim dt As DataTable
                                If ViewState("estadoOI") = "E-EXC" Or ViewState("estadoOI") = "E-EBL" Then
                                    dt = oParametrosGeneralesBM.SeleccionarPorFiltro(ParametrosSIT.USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                                    For Each fila As DataRow In dt.Rows
                                        toUser = toUser + fila("Valor") + ";"
                                    Next
                                    Try
                                        UIUtility.EnviarMail(toUser, "", "Pendiente de aprobación - Orden excedido por Limites de Inversión", OrdenInversion.MensajeExcesosOI(strcodigoOrden, ddlFondo.SelectedValue.ToString(), DatosRequest), DatosRequest) 'CMB OT 62254 20110415
                                    Catch ex As Exception
                                        AlertaJS("Se ha generado un error en el proceso de envio de notificación! ") 'CMB OT 61566 20110107 Nro 7 Se agrego la alerta para controlar el error de envio de correo, cuando el servicio este inoperativa
                                    End Try
                                End If
                            End If
                        End If
                        oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                        Me.txtCodigoOrden.Value = strcodigoOrden
                        Session("dtdatosoperacion") = ObtenerDatosOperacion()
                        GenerarLlamado(strcodigoOrden + ViewState("CodigoOrden_T"), Me.ddlFondo.SelectedValue, "ACCIONES", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text)
                        CargarPaginaAceptar()
                    End If
                Else
                    If Session("EstadoPantalla") = "Modificar" Then
                        actualizaMontos()
                        ModificarOrdenInversion()
                        FechaEliminarModificarOI("M")
                        CargarPaginaAceptar()
                        Session("dtdatosoperacion") = ObtenerDatosOperacion()
                        If Me.hdPagina.Value <> "MODIFICA" Then
                            GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "ACCIONES", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text)
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
    Private Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos(True)
        CargarFechaVencimiento()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
    End Sub
    Private Sub btnLimites_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimites.Click
        Dim Guid As String = ViewState("GUID_Limites")

        Dim dstLimites As DataSet = CType(Session(Guid), DataSet)
        If (Not dstLimites Is Nothing) Then
            If (dstLimites.Tables.Count > 0) Then
                If (dstLimites.Tables(0).Rows.Count > 0) Then
                    Session("Instrumento") = "ACCIONES"
                    EjecutarJS(UIUtility.MostrarPopUp("frmConsultaLimitesInstrumento.aspx?GUID=" + Guid, "yes", 1000, 500, 50, 5, "no", "yes", "yes", "yes"))
                Else
                    AlertaJS("¡No hay exceso de limites!")
                End If
            End If
        End If
    End Sub

    Private Sub btnLimitesParametrizados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimitesParametrizados.Click
        Try
            ConsultaLimitesPorInstrumento() 'CMB OT 61566 20101130
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        LimpiarSesiones()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Eliminar"
        lblAccion.Text = "Eliminar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Eliminación"

        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row")

        HelpCombo.CargarMotivosCambio(Me)
    End Sub

    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        LimpiarSesiones()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Modificar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        lblAccion.Text = "Modificar"
        CargarPaginaAccion()
        hdMensaje.Value = "la Modificación"
        Me.hdSaldo.Value = 0

        HelpCombo.CargarMotivosCambio(Me)
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row")
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        'Dim feriado As New FeriadoBM

        Try
            If CDec(txtNumConOperacion.Text) <= 0 Then
                AlertaJS("Ingrese una cantidad de acciones mayor a 0")
                Exit Sub
            End If
            If ValidarFechas() = True Then
                If UIUtility.ValidarHora(Me.tbHoraOperacion.Text) = False Then
                    AlertaJS(ObtenerMensaje("CONF22"))
                    Exit Sub
                End If

                txtMontoNominal.Text = Format((Convert.ToDecimal(Me.txtNumConOperacion.Text) * Convert.ToDecimal(Me.txtPrecio.Text)), "##,##0.00")

                CalcularComisiones()
                Session("Procesar") = 1

                ViewState("estadoOI") = ""

                If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then OrdenInversion.CalculaLimitesOnLine(Me, DatosRequest, ViewState("estadoOI"), ViewState("GUID_Limites")) 'RGF 20090401   'HDG INC 62817	20110419
                'FIN CMB OT 64769 20120320
                '------------------------------------------------------------------
                If Session("TipoRenta") = 2 Then RestriccionExcesosBroker() 'HDG OT 60022 20100713

                Dim ds As DataSet
                Dim Mensaje As String
                ds = New OrdenPreOrdenInversionBM().ValidacionPuntual_LimitesTrading(txtMnemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), ddlFondo.SelectedValue, Convert.ToDecimal(txtNumConOperacion.Text.Replace(",", "").Replace(".", UIUtility.DecimalSeparator)), Session("CodigoMoneda"), Usuario.ToString.Trim, String.Empty)
                If ds.Tables(0).Rows.Count > 0 Then
                    Mensaje = "El usuario no esta permitido grabar la operación, el monto de negociado excede el límite de autonomía por Trader:\n\n"
                    For Each fila As DataRow In ds.Tables(0).Rows
                        Mensaje = Mensaje & "- Usuario (" & fila("TipoCargoExc") & ") excedió límite de autonomía \""" & fila("GrupoLimTrd") & "\"", debe ser autorizado por un usuario " & fila("TipoCargoAut") & " (" & fila("TraderAut") & "). \n\n"
                    Next
                    Mensaje = Mensaje & "La operación debe ser grabada por el usuario autorizado haciendo clic en el botón Aceptar de la orden de inversión."
                    AlertaJS(Mensaje)
                    Session("dtValTrading") = ds.Tables(0)
                End If

                If ObtieneCustodiosSaldos() = False Then
                    Exit Sub
                End If

                CargarPaginaProcesar()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try

    End Sub

    Private Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        LimpiarSesiones()
        Call ConfiguraModoConsulta()
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "ACCIONES", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text)
    End Sub

    Private Sub btnAsignar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsignar.Click
        Session("URL_Anterior") = Page.Request.Url.AbsolutePath.ToString
        If ddlFondo.SelectedValue.ToString = "MULTIFONDO" Then
            Response.Redirect("../AsignacionFondos/frmIngresoCriteriosAsignacion.aspx?vISIN=" & Me.txtISIN.Text.ToString & "&vCantidad=" & txtNumConOperacion.Text.ToString & "&vMnemonico=" & Me.txtMnemonico.Text.ToString & "&vFondo=" & ddlFondo.SelectedValue.ToString & "&vOperacion=" & Me.ddlOperacion.SelectedItem.Text & "&vImpuestosComisiones=" & Me.txttotalComisionesC.Text & "&vMoneda=" & Me.lblMoneda.Text & "&vCodigoOrden=" & Me.txtCodigoOrden.Value & "&vCategoria=AC", False)
        End If
    End Sub

    Private Sub btnVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Session("prevPag") = "../InstrumentosNegociados/frmAcciones.aspx"
        Response.Redirect("../Reportes/frmVisorOrdenesDeInversion.aspx?titulo=Orden de Inversión - ACCIONES")
    End Sub

    Private Sub btnCaracteristicas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCaracteristicas.Click
        If Me.txtMnemonico.Text <> "" Then
            Session("accionValor") = "MODIFICAR"
            EjecutarJS(UIUtility.MostrarPopUp("../../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" + Me.txtMnemonico.Text + "&vOI=T", "11", 1000, 600, 0, 0, "No", "No", "Yes", "Yes"), False)
        Else
            AlertaJS(ObtenerMensaje("CONF23"))
        End If
    End Sub

#End Region

#Region "Private Methods"

#Region "Usuados en el Load"

    Private Sub CargarPaginaInicio()
        HabilitaDeshabilitaCabecera(False)
        HabilitaDeshabilitaDatosOperacionComision(False)
        OcultarBotonesInicio()
        Me.btnAceptar.Enabled = False
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
        Me.ddlIntermediario.Enabled = estado
        Me.ddlPlaza.Enabled = estado
        Me.ddlCondicion.Enabled = estado
        Me.ddlContacto.Enabled = estado
        Me.dgLista.Enabled = estado
        HabilitaDeshabilitaValoresGrilla(estado)
        Me.tbFechaOperacion.ReadOnly = Not estado
        Me.tbFechaLiquidacion.ReadOnly = Not estado
        Me.ddlVencimientoMes.Enabled = estado
        Me.txtVencimientoAno.Enabled = estado
        Me.txtPrecio.ReadOnly = Not estado
        Me.tbHoraOperacion.ReadOnly = Not estado

        If estado Then
            imgFechaOperacion.Attributes.Remove("class")
            imgFechaOperacion.Attributes.Add("class", "input-append date")
        Else
            imgFechaOperacion.Attributes.Remove("class")
            imgFechaOperacion.Attributes.Add("class", "input-append")
        End If

        If estado Then
            imgFechaVcto.Attributes.Remove("class")
            imgFechaVcto.Attributes.Add("class", "input-append date")
        Else
            imgFechaVcto.Attributes.Remove("class")
            imgFechaVcto.Attributes.Add("class", "input-append")
        End If

        If (Me.hdPagina.Value = "DA") Then
            Me.tbFechaOperacion.ReadOnly = True

            imgFechaOperacion.Attributes.Remove("class")
            imgFechaOperacion.Attributes.Add("class", "input-append")
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
        Me.btnLimites.Visible = False
        Me.btnAsignar.Visible = False
        Me.btnProcesar.Visible = False
        Me.btnImprimir.Visible = False
    End Sub

    Private Sub CargarCaracteristicasValor()
        Dim dsValor As New DataSet
        Dim drValor As DataRow
        Dim oOIFormulas As New OrdenInversionFormulasBM

        If Session("EstadoPantalla") = "Ingresar" Then
            imgFechaOperacion.Attributes.Remove("class")
            imgFechaOperacion.Attributes.Add("class", "input-append date")
        End If

        Try
            dsValor = oOIFormulas.SeleccionarCaracValor_Futuros(Me.txtMnemonico.Text, DatosRequest)
            If dsValor.Tables(0).Rows.Count > 0 Then
                drValor = dsValor.Tables(0).NewRow
                drValor = dsValor.Tables(0).Rows(0)
                Session("TipoRenta") = CType(drValor("val_TipoRenta"), String)
                Session("CodigoMoneda") = CType(drValor("val_CodigoMoneda"), String)
                Me.lblMoneda.Text = CType(drValor("val_CodigoMoneda"), String)
                Me.txtISIN.Text = CType(drValor("val_CodigoISIN"), String)
                Me.txtSBS.Text = CType(drValor("val_CodigoSBS"), String)

                'Registrando los datos de Caracteristicas Valor
                Me.lblNumUnidades.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_NumUnidades")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblFechaEmision.Text = UIUtility.ConvertirFechaaString(CType(drValor("val_FechaEmision"), String))
                Me.lblFechaVencimiento.Text = UIUtility.ConvertirFechaaString(CType(drValor("val_FechaVenc"), String))
                Me.lblMargenInicial.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_MargenInicial")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblMargenMantenimiento.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_MargenMantenimiento")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.00")
                Me.lblContractZise.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_ContractSize")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.00")

            End If
        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF21"))
        End Try
    End Sub

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
        If Me.ddlFondo.SelectedValue <> "MULTIFONDO" Then
            UIUtility.AsignarMensajeBoton(btnAceptar, "CONF1")
        Else
            UIUtility.AsignarMensajeBoton(btnAceptar, "CONF15")
        End If
        CargarPaginaIngresar()
        CargarIntermediario()
        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
        HabilitaDeshabilitaValoresGrilla(False)
    End Sub

    Private Sub MostrarOcultarBotonesAcciones(ByVal estado As Boolean)
        Me.btnIngresar.Visible = estado
        Me.btnModificar.Visible = estado
        Me.btnEliminar.Visible = estado
        Me.btnConsultar.Visible = estado
    End Sub

    Public Sub CargarFechaVencimiento()
        If txtMnemonico.Text.Trim().Equals("") Then
            Exit Sub
        End If
        If Session("EstadoPantalla") = "Ingresar" Then
            tbFechaLiquidacion.Text = tbFechaOperacion.Text
        Else
            If (Me.hdPagina.Value <> "CO") Then
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
                tbFechaLiquidacion.Text = oOrdenInversionBM.RetornarFechaVencimiento(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), Me.txtMnemonico.Text, ddlFondo.SelectedValue, ddlIntermediario.SelectedValue)
            End If
        End If
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

    Private Sub CargarPaginaBuscar()
        If Me.ddlFondo.SelectedValue = "MULTIFONDO" Then
            Me.btnAsignar.Visible = True
            Me.btnAsignar.Enabled = True
        End If
        Me.btnProcesar.Visible = True
        Me.btnProcesar.Enabled = True
    End Sub

    Private Sub CargarIntermediario()
        ddlIntermediario.Items.Clear()
        UIUtility.CargarIntermediariosCustodioXGrupoInterOI(ddlIntermediario, "", "")
        Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
    End Sub

    Public Sub CargarDatosOrdenInversion(ByVal CodigoOrden As String)
        Try
            oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(CodigoOrden, Me.ddlFondo.SelectedValue, DatosRequest, PORTAFOLIO_MULTIFONDOS)
            Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
            oRow = oOrdenInversionBE.Tables(0).Rows(0)
            Session("CodigoMoneda") = oRow.CodigoMoneda

            If oRow.CodigoOperacion.ToString <> "" Then
                ddlOperacion.SelectedIndex = ddlOperacion.Items.IndexOf(ddlOperacion.Items.FindByValue(oRow.CodigoOperacion.ToString()))
            Else
                ddlOperacion.SelectedIndex = 0
            End If

            txtISIN.Text = oRow.CodigoISIN.ToString()
            txtMnemonico.Text = oRow.CodigoMnemonico.ToString()
            txtCodigoOrden.Value = oRow.CodigoOrden.ToString()

            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
            txtNumConOrdenados.Text = Format(oRow.CantidadOrdenado, "##,##0.0000000")
            txtNumConOperacion.Text = Format(oRow.CantidadOperacion, "##,##0.0000000")
            txtMontoNominal.Text = Format(oRow.MontoOperacion, "##,##0.0000000")
            txtPrecio.Text = Format(oRow.Precio, "##,##0.0000000")
            txttotalComisionesC.Text = Format(oRow.TotalComisiones, "##,##0.0000000")
            txtPrecPromedio.Text = Format(oRow.PrecioPromedio, "##,##0.0000000")
            txtMontoNetoOpe.Text = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
            tbHoraOperacion.Text = oRow.HoraOperacion
            hdNumUnidades.Value = Format(oRow.CantidadOrdenado, "##,##0.0000000")
            txtVencimientoAno.Text = oRow.VencimientoAno


            If (Me.hdPagina.Value = "CO") And oRow.TipoCambio > 0 Then
                Me.tblDestino.Attributes.Add("Style", "Visibility:visible")
                Me.txtMontoOperacionDestino.Text = Format(oRow.MontoDestino, "##,##0.0000000")
                Me.txtComisionesDestino.Text = Format((oRow.TipoCambio) * (oRow.TotalComisiones), "##,##0.0000000")
                If oRow.CodigoMnemonico = "MPLE LN" Then
                    Me.lblMDest.Text = Trim(oRow.CodigoMoneda)
                    Me.lblMDest2.Text = Trim(oRow.CodigoMoneda)
                Else
                    Me.lblMDest.Text = Trim(oRow.CodigoMonedaDestino)
                    Me.lblMDest2.Text = Trim(oRow.CodigoMonedaDestino)
                End If

            End If

            CargarPlaza()
            ddlPlaza.SelectedValue = oRow.Plaza

            CargarCondicion()
            ddlCondicion.SelectedValue = oRow.TipoCondicion

            CargarVencimientoMes()
            ddlVencimientoMes.SelectedValue = oRow.VencimientoMes

            tbHoraEjecucion.Text = oRow.HoraEjecucion

            ViewState("MontoNeto") = Convert.ToDecimal(txtMontoNominal.Text)
            Me.tbNPoliza.Text = oRow.NumeroPoliza.ToString()

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
            If oRow.CodigoMotivoCambio <> "" Then
                ddlMotivoCambio.SelectedValue = oRow.CodigoMotivoCambio
            End If
        Catch ex As Exception
            If Me.ddlFondo.SelectedValue <> "MULTIFONDO" Then
                AlertaJS(ObtenerMensaje("CONF31"))
            Else
                AlertaJS(ObtenerMensaje("CONF32"))
            End If
        End Try
    End Sub

    Public Sub CargarPlaza()
        Dim oPlazaBM As New PlazaBM
        ddlPlaza.DataSource = oPlazaBM.Listar(Nothing)
        ddlPlaza.DataTextField = "Descripcion"
        ddlPlaza.DataValueField = "CodigoPlaza"
        ddlPlaza.DataBind()
        ddlPlaza.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub

    Public Sub CargarVencimientoMes()
        Dim dtVencimientoMes As DataTable
        dtVencimientoMes = New ParametrosGeneralesBM().ListarVencimiento(DatosRequest)
        HelpCombo.LlenarComboBox(ddlVencimientoMes, dtVencimientoMes, "Valor", "Nombre", True)
    End Sub

    Public Sub CargarCondicion()
        Dim oCondicionBM As New ParametrosGeneralesBM
        Dim dtCondicion As DataTable
        dtCondicion = oCondicionBM.ListarCondicionPrevOI().Tables(0)
        HelpCombo.LlenarComboBox(ddlCondicion, dtCondicion, "Valor", "Nombre", True)
    End Sub

    Private Sub CargarContactos(Optional ByVal cambiaIntermediario As Boolean = False)
        If ddlContacto.SelectedIndex > 0 And Session("EstadoPantalla") <> "Ingresar" And Not cambiaIntermediario Then
            Exit Sub
        End If

        Dim objContacto As New ContactoBM
        Dim dtContacto As DataTable
        dtContacto = objContacto.ListarContactoPorTerceros(Me.ddlIntermediario.SelectedValue).Tables(0)
        Me.ddlContacto.Items.Clear()
        If dtContacto.Rows.Count > 0 Then

            HelpCombo.LlenarComboBox(ddlContacto, dtContacto, "CodigoContacto", "DescripcionContacto", True)

            Me.ddlContacto.DataSource = dtContacto
            Me.ddlContacto.DataTextField = "DescripcionContacto"
            Me.ddlContacto.DataValueField = "CodigoContacto"
            Me.ddlContacto.DataBind()

            Me.ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))

            Me.ddlContacto.SelectedValue = dtContacto.Rows(0)(0).ToString 'HDG 20120918

            'LETV 20090320
            If txtMnemonico.Text <> "" And ddlIntermediario.SelectedValue <> "" Then
                ddlContacto.SelectedValue = objContacto.SeleccionarUltimoContactoEnUnaNegociacion(txtMnemonico.Text, ddlIntermediario.SelectedValue)
            End If
        Else
            Me.ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        End If

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
                    ViewState("CodigoEntidad") = dtAux.Rows(i)("CodigoTercero")
                    Me.dgLista.Dispose()
                    Me.dgLista.DataBind()
                    OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
                    Exit For
                End If
            Next
        End If

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

    Private Sub ConfiguraModoConsulta()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Consulta"
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

    Private Sub LimpiarCaracteristicasValor()
        Me.lblMargenInicial.Text = ""
        Me.lblMargenMantenimiento.Text = ""
        Me.lblContractZise.Text = ""
        Me.lblFechaEmision.Text = ""
        Me.lblFechaVencimiento.Text = ""
        Me.lblNumUnidades.Text = ""
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
        Me.txtPrecio.Text = ""
        Me.txtNumConOrdenados.Text = ""
        Me.txtNumConOperacion.Text = ""
        Me.txtMontoNominal.Text = ""
        If Me.ddlIntermediario.Items.Count > 0 Then Me.ddlIntermediario.SelectedIndex = 0
        CargarContactos()
        Me.ddlContacto.SelectedIndex = 0
        Me.tbHoraOperacion.Text = ""
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        Me.txttotalComisionesC.Text = ""
        Me.txtMontoNetoOpe.Text = ""
        Me.txtPrecPromedio.Text = ""
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
        btnLimitesParametrizados.Visible = bLimitesParametrizados

    End Sub

    Private Sub LimpiarSesiones()
        Session("Procesar") = Nothing
        Session("ValorCustodio") = Nothing
        Session("Mercado") = Nothing
        Session("TipoRenta") = Nothing
        Session("EstadoPantalla") = Nothing
        Session("Busqueda") = Nothing
        Session("datosEntidad") = Nothing
        Session("CodigoMoneda") = Nothing
        Session("Instrumento") = Nothing
        Session("ReporteLimitesEvaluados") = Nothing
        Session("ValorCustodio") = Nothing
        Session("dtdatosoperacion") = Nothing
        Session("accionValor") = Nothing
    End Sub

#End Region

#Region "Usuados en busqueda"

    Private Sub CargarPaginaModificar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        Me.btnCaracteristicas.Visible = True
        Me.btnCaracteristicas.Enabled = True
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
        End If
        EjecutarJS(strJS.ToString())
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

#End Region

#Region "Usuados en Aceptar"

    Public Sub ModificarOrdenInversion()
        oOrdenInversionBE = crearObjetoOI()
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, Me.hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
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
        oRow.CantidadOrdenado = txtNumConOrdenados.Text.Replace(",", "")
        oRow.CantidadOperacion = txtNumConOperacion.Text.Replace(",", "")
        oRow.MontoOperacion = txtMontoNominal.Text.Replace(",", "")
        oRow.Precio = txtPrecio.Text.Replace(",", "")
        oRow.TotalComisiones = txttotalComisionesC.Text.Replace(",", "")
        oRow.PrecioPromedio = txtPrecPromedio.Text.Replace(",", "")
        oRow.MontoNetoOperacion = txtMontoNetoOpe.Text.Replace(",", "")
        oRow.Situacion = "A"
        oRow.Observacion = txtObservacion.Text
        oRow.HoraOperacion = tbHoraOperacion.Text
        oRow.CategoriaInstrumento = ParametrosSIT.FUTURO
        oRow.Plaza = ddlPlaza.SelectedValue
        oRow.VencimientoAno = txtVencimientoAno.Text
        oRow.VencimientoMes = ddlVencimientoMes.SelectedValue
        oRow.TipoCondicion = ddlCondicion.SelectedValue

        If (Me.hdPagina.Value <> "XO") Then
            oRow.NumeroPoliza = Me.tbNPoliza.Text.ToString().Trim
        End If

        If Not ViewState("estadoOI") Is Nothing Then
            If ViewState("estadoOI").Equals("E-EXC") Or ViewState("estadoOI").Equals("E-ENV") Or ViewState("estadoOI").Equals("E-EBL") Then 'HDG OT 61166 20100920
                oRow.Estado = ViewState("estadoOI")
            End If
        End If

        If (Me.hdPagina.Value <> "XO") Then
            oRow.NumeroPoliza = tbNPoliza.Text.ToString()
        End If

        If (Me.hdPagina.Value = "CO") Then
            If txtMontoOperacionDestino.Text.Trim.Equals("") Then
                oRow.MontoDestino = 0
            Else
                oRow.MontoDestino = txtMontoOperacionDestino.Text.Replace(",", "")
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

        oRow.HoraEjecucion = tbHoraEjecucion.Text 'HDG OT 64291 20111128

        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()

        Return oOrdenInversionBE
    End Function

    Private Sub CargarPaginaAceptar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        If Session("EstadoPantalla") = "Ingresar" Then
            'Me.btnImprimir.Visible = True
            'Me.btnImprimir.Enabled = True
            If Me.ddlFondo.SelectedValue = "MULTIFONDO" Then
                Me.btnAsignar.Visible = True
            End If
        End If
        Me.btnAceptar.Enabled = False
    End Sub

    Private Sub ReturnArgumentShowDialogPopup()

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

        EjecutarJS(script.ToString())
    End Sub

    Private Sub actualizaMontos()
        Dim dblTotalComisiones As Decimal = 0.0
        dblTotalComisiones = UIUtility.ActualizaMontosFinales(dgLista)
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoNominal.Text.Replace(",", ""), "##,##0.0000000")
        Dim strMontoAux As String = txtMontoNetoOpe.Text.Replace(",", "")
        strMontoAux = strMontoAux.Replace(".", UIUtility.DecimalSeparator)
        Dim strAccAux As String = txtNumConOperacion.Text.Replace(",", "")
        strAccAux = strAccAux.Replace(".", UIUtility.DecimalSeparator)
        txtPrecPromedio.Text = Format(Convert.ToDecimal(strMontoAux) / Convert.ToDecimal(strAccAux), "##,##0.0000000")
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
        drGrilla("v3") = "Hora Operación"
        drGrilla("c3") = Me.tbHoraOperacion.Text
        drGrilla("c4") = "Numero Acciones Ordenadas"
        drGrilla("v4") = Me.txtNumConOrdenados.Text
        drGrilla("c5") = "Numero Acciones Operación"
        drGrilla("v5") = Me.txtNumConOperacion.Text
        drGrilla("c6") = "Precio"
        drGrilla("v6") = Me.txtPrecio.Text
        drGrilla("c7") = "Monto Operacion"
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
            drGrilla("c11") = "Poliza"
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
        drGrilla("c20") = "Precio Promedio"
        drGrilla("v20") = Me.txtPrecPromedio.Text
        drGrilla("c21") = "Monto Neto Operacion"
        drGrilla("v21") = Me.txtMontoNetoOpe.Text
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla

    End Function

    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        Me.LlenarSesionContextInfo()
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&cportafolio=" + portafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
    End Sub

    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(Me.ddlFondo.SelectedValue, Me.txtCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc, txtComentarios.Text, DatosRequest)
    End Sub

    Public Sub EliminarOrdenInversion()
        oOrdenInversionBM.EliminarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, ddlMotivoCambio.SelectedValue, DatosRequest)
        oImpComOP.Eliminar(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, DatosRequest)
    End Sub

#End Region

#Region "Usuados en Procesar"

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

    Public Sub CalcularComisiones()
        Dim dblTotalComisiones As Decimal = 0.0
        If Me.hdPagina.Value = "TI" Then
            dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), Me.txtMontoNominal.Text.Replace(",", ""), Me.txtNumConOperacion.Text.Replace(",", ""), "", ddlOperacion.SelectedValue, ParametrosSIT.FUTURO)  'HDG 20120224
        Else
            dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), Me.txtMontoNominal.Text.Replace(",", ""), Me.txtNumConOperacion.Text.Replace(",", ""), "", ddlOperacion.SelectedValue, ParametrosSIT.FUTURO)  'HDG 20120224
        End If

        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")

        If (ddlOperacion.SelectedValue = "2") Then
            txtMontoNetoOpe.Text = Format(txtMontoNominal.Text.Replace(",", "") - dblTotalComisiones, "##,##0.0000000")
        Else
            txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoNominal.Text.Replace(",", ""), "##,##0.0000000")
        End If

        Dim strMontoAux As String = txtMontoNetoOpe.Text.Replace(",", "")
        strMontoAux = strMontoAux.Replace(".", UIUtility.DecimalSeparator)
        Dim strAccAux As String = txtNumConOperacion.Text.Replace(",", "")
        strAccAux = strAccAux.Replace(".", UIUtility.DecimalSeparator)
        txtPrecPromedio.Text = Format(Convert.ToDecimal(strMontoAux) / Convert.ToDecimal(strAccAux), "##,##0.0000000")
    End Sub

    Protected Sub RestriccionExcesosBroker()
        Dim oFormulasOI As New OrdenInversionFormulasBM
        Dim indExceso As String

        If hdPagina.Value <> "TI" Then
            indExceso = oFormulasOI.RestriccionExcesosBroker(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), ddlIntermediario.SelectedValue.ToString(), Convert.ToDecimal(txtMontoNominal.Text), txtMnemonico.Text)

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
                    strCodigoOperacion = UIUtility.ObtenerCodigoTipoOperacion("VENTA")
                End If
                '------------------------------------------------------------------------------

                If strCodigoOperacion = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
                    AlertaJS(ObtenerMensaje("CONF28"))
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
            If hdNumUnidades.Value = txtNumConOperacion.Text Then
                Return True
            End If
            '********COMPRA*********
            If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                'RGF 20090313 se comento para poder negociar con cualquier intermediario
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.txtNumConOperacion.Text
                hdNumUnidades.Value = txtNumConOperacion.Text
                Return True
            End If

            '********VENTA*********
            If Session("ValorCustodio") Is Nothing Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
            ElseIf Session("ValorCustodio") = "" Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
            End If
            decMontoAux = Convert.ToDecimal(Me.lblSaldoValor.Text)
            If decMontoAux = Convert.ToDecimal(Me.txtNumConOperacion.Text.Replace(".", UIUtility.DecimalSeparator)) Then
                hdNumUnidades.Value = txtNumConOperacion.Text
                Return True
            ElseIf decMontoAux > Convert.ToDecimal(Me.txtNumConOperacion.Text.Replace(".", UIUtility.DecimalSeparator)) Then
                'redefinir calculos porq excede
                If cantCustodios = 1 Then
                    'porque solamente es el primer custodio
                    Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.txtNumConOperacion.Text
                    hdNumUnidades.Value = txtNumConOperacion.Text
                    Return True
                Else
                    'porq hay mas de un custodio. ajustar montos
                    Session("ValorCustodio") = UIUtility.AjustarMontosCustodios(CType(Session("ValorCustodio"), String), Me.txtNumConOperacion.Text)
                    hdNumUnidades.Value = txtNumConOperacion.Text
                    Return True
                End If
                Return False
            ElseIf decMontoAux < Convert.ToDecimal(Me.txtNumConOperacion.Text.Replace(".", UIUtility.DecimalSeparator)) Then
                'redefinir calculos porq falta
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "Usados en Limites Parametrizados"

    Private Sub ConsultaLimitesPorInstrumento()
        Dim strFondo As String = ddlFondo.SelectedValue
        Dim strEscenario As String = "REAL"
        Dim strFecha As String = tbFechaOperacion.Text
        Dim strValorNivel As String = txtMnemonico.Text
        EjecutarJS(UIUtility.MostrarPopUp("../Reportes/Orden de Inversion/frmVisorReporteLimitesPorInstrumento.aspx?Portafolio=" & strFondo & "&ValorNivel=" & strValorNivel & "&Escenario=" & strEscenario & "&Fecha=" & strFecha, "10", 800, 600, 10, 10, "No", "Yes", "Yes", "Yes"), False)
    End Sub

#End Region

#End Region

#Region "Metodos Get y Set"

    Private Function GetISIN() As String
        Return txtISIN.Text
    End Function
    Private Function GetSBS() As String
        Return txtSBS.Text
    End Function
    Private Function GetMNEMONICO() As String
        Return txtMnemonico.Text
    End Function
    Private Function GetFONDO() As String
        Return Me.ddlFondo.SelectedValue
    End Function
    Private Function GetFondo_2() As String
        Return Me.ddlFondo.SelectedItem.Text
    End Function
    Private Function GetOPERACION() As String
        Return IIf(ddlOperacion.SelectedIndex = 0, "", ddlOperacion.SelectedValue)
    End Function
    Private Function GetMONEDA() As String
        Return lblMoneda.Text
    End Function

#End Region

#Region "Métodos Personalizados (Popups Dialogs)"

    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal operacion As String, ByVal categoria As String, ByVal valor As String)
        Dim script As New StringBuilder
        With script
            .Append("<script>")
            .Append("$('#" & hfModal.ClientID & "').val('" & valor & "'); ")
            '.Append("function PopupBuscador(isin, mnemonico, sbs, fondo, operacion, categoria)")
            '.Append("{")
            '.Append("   window.showModalDialog(', '', 'dialogHeight:530px; dialogWidth:1180px; dialogLeft:150px;');")
            '.Append("   return false;")
            '.Append("}")
            '.Append("PopupBuscador('" + isin + "','" + mnemonico + "','" + sbs + "','" + fondo + "','" + operacion + "','" + categoria + "');")
            '.Append("   document.getElementById('btnBuscar').click();")
            .Append("</script>")
        End With

        EjecutarJS(script.ToString(), False)
        EjecutarJS("showModalDialog('frmBuscarValor.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&vFondo=" + fondo + "&vOperacion=" + operacion + "&vCategoria=" + categoria + "', '1180', '530', 'btnBuscar')")

    End Sub

    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal operacion As String, ByVal moneda As String, ByVal fecha As String, ByVal accion As String, ByVal valor As String)
        Dim script As New StringBuilder
        With script
            .Append("<script>")
            .Append("$('#" & hfModal.ClientID & "').val('" & valor & "'); ")
            '.Append("function PopupBuscador(isin, sbs, mnemonico, fondo, operacion, moneda, fecha, accion)")
            '.Append("{")
            '.Append("   window.showModalDialog(', '', 'dialogHeight:700px; dialogWidth:950px; dialogLeft:150px;');")
            '.Append("   return false;")
            '.Append("}")
            '.Append("PopupBuscador('" + isin + "','" + sbs + "','" + mnemonico + "','" + fondo + "','" + operacion + "','" + moneda + "','" + fecha + "','" + accion + "');")
            '.Append("   document.getElementById('btnBuscar').click();")
            .Append("</script>")
        End With
        EjecutarJS(script.ToString(), False)

        EjecutarJS("showModalDialog('../frmInversionesRealizadas.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&vFondo=" + fondo + "&cFondo=" + fondo + "&vOperacion=" + operacion + "&vFechaOperacion=" + fecha + "&vAccion=" + accion + "&vCategoria=FT', '950', '700', 'btnBuscar')")
    End Sub

#End Region

    Private Function lblSaldoValor() As Object
        Throw New NotImplementedException
    End Function

    Protected Sub ibConsultaCertificados_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibConsultaCertificados.Click

    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
End Class

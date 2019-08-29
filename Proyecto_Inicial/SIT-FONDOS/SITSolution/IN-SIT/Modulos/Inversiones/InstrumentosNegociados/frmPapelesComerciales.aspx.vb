Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports ParametrosSIT

Imports Sit.BusinessLayer.MotorInversiones

Partial Class Modulos_Inversiones_InstrumentosNegociados_frmPapelesComerciales
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


    Dim txtComisionSAB As TextBox
    Dim txtComisionConasev As TextBox
    Dim txtCuotaCavali As TextBox
    Dim txtCuotaBVL As TextBox
    Dim txtComisionIGV As TextBox
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Session("SS_DatosModal") Is Nothing Then
                Select Case hdPopUp.Value
                    Case "V"
                        txtISIN.Text = CType(Session("SS_DatosModal"), String())(0)
                        txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1)
                        txtSBS.Text = CType(Session("SS_DatosModal"), String())(2)
                        hdCustodio.Value = CType(Session("SS_DatosModal"), String())(3)
                        hdSaldo.Value = CType(Session("SS_DatosModal"), String())(4)
                        'OT 9968 - 14/02/2017 - Carlos Espejo
                        'Descripcion: Se crea las sesiones
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
                        'OT 9968 - 14/02/2017 - Carlos Espejo
                        'Descripcion: Se crea las sesiones
                        Session("Orden") = CType(Session("SS_DatosModal"), String())(6)
                        Session("Nemonico") = CType(Session("SS_DatosModal"), String())(2)
                        'OT 9968 Fin
                End Select
                Session.Remove("SS_DatosModal")
            End If
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            hdSaldo.Value = 0
            btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
            If Not Page.IsPostBack Then
                btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
                btnProcesar.Attributes.Add("onclick", "javascript:return Validar();")
                btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                hdRptaConfirmar.Value = "NO"
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                LimpiarSesiones()
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    hdPagina.Value = Request.QueryString("PTNeg")
                End If
                'If (hdPagina.Value = "TI") Then
                '    UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
                'Else
                '    UIUtility.CargarOperacionOI(ddlOperacion, "OperacionOI")
                'End If
                'CargarPlaza()
                'UIUtility.CargarTipoCuponOI(ddlTipoTasa)
                'UIUtility.CargarGrupoIntermediarioOI(ddlGrupoInt)
                CargarCombos()
                CargarPaginaInicio()
                hdPagina.Value = ""
                'HelpCombo.PortafolioCodigoListar(ddlFondo, PORTAFOLIO_MULTIFONDOS)
                DivDatosCarta.Visible = False
                DivObservacion.Visible = False
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    hdPagina.Value = Request.QueryString("PTNeg")
                    If hdPagina.Value = "TI" Then
                        txtMnemonico.Text = Request.QueryString("PTCMnemo")
                        ddlFondo.SelectedValue = Request.QueryString("PTFondoO")
                        ddlFondoDestino.SelectedValue = Request.QueryString("PTFondoD")
                        txtISIN.Text = Request.QueryString("PTISIN")
                        txtSBS.Text = Request.QueryString("PTSBS")
                        lblMoneda.Text = Request.QueryString("PTMon")
                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                        hdCustodio.Value = Request.QueryString("PTCustodio")
                        hdSaldo.Value = Request.QueryString("PTSaldo")
                        CargarCaracteristicasValor()
                        ObtieneImpuestosComisiones()
                        ControlarCamposTI()
                        txtPrecioNegSucio.Text = "0.0000000"
                        lblInteresCorrido.Text = "0.0000000"
                        txtNroPapeles.Text = "0.0000000"
                        txtInteresCorNeg.Text = "0.0000000"
                        lblPrecioCal.Text = "0.0000000"
                        txtPrecioNegoc.Text = "0.0000000"
                        txtMontoOperacional.Text = "0.0000000"
                        txtYTM.Text = "0.0000000"
                        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(Request.QueryString("fechaOperacion")))
                    Else
                        txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                        Session("Orden") = Request.QueryString("PTNOrden")
                        ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                        Session("CodOrden") = txtCodigoOrden.Value
                        'ValidaOrigen()

                        If (hdPagina.Value = "EO") Or (hdPagina.Value = "CO") Or (hdPagina.Value = "XO") Then 'Viene de la Pagina Confirmacion , Ejecucion  o Extorno de Ordenes de Inversion
                            CargarDatosOrdenInversion(Request.QueryString("PTNOrden"))
                            CargarCaracteristicasValor()
                            tbNPoliza.Text = Right(txtCodigoOrden.Value, 4)
                            tbNPoliza.ReadOnly = True
                            UIUtility.ObtieneImpuestosComisionesGuardado(dgLista, txtCodigoOrden.Value, ddlFondo.SelectedValue)
                            Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                            If (hdPagina.Value <> "XO") Then
                                lNPoliza.Attributes.Remove("class")
                                lNPoliza.Attributes.Add("class", "col-sm-6 control-label")
                                tbNPoliza.Visible = True
                            End If
                            ControlarCamposEO_CO_XO()
                            CargarPaginaModificarEO_CO_XO(hdPagina.Value)
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
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18   Else
                            If (hdPagina.Value = "OE") Then 'Viene de la Pagina Ordenes Excedidas
                                ControlarCamposOE()
                            Else
                                If (hdPagina.Value = "DA") Then 'Viene de la Pagina Negociacion Dias Anteriores
                                    ViewState("ORDEN") = "OI-DA"
                                    tbFechaOperacion.Text = Request.QueryString("Fecha")
                                Else
                                    If (hdPagina.Value = "CP") Then 'Viene de la Pagina Liquidaciones Cuentas Por Pagar
                                        Call ConfiguraModoConsulta()
                                        ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                        txtMnemonico.Text = Request.QueryString("Mnemonico")
                                        txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                        Call CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
                                        Call CargarCaracteristicasValor()
                                        UIUtility.ObtieneImpuestosComisionesGuardado(dgLista, txtCodigoOrden.Value, ddlFondo.SelectedValue)
                                        Call HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, False, True, False, False)
                                    Else
                                        If (hdPagina.Value = "CONSULTA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                            ConfiguraModoConsulta()
                                            ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                            txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                            tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                            CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
                                            CargarCaracteristicasValor()
                                            UIUtility.ObtieneImpuestosComisionesGuardado(dgLista, txtCodigoOrden.Value, ddlFondo.SelectedValue)
                                            HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, True, False, False, False)
                                            HabilitaDeshabilitaCabecera(False)
                                        Else
                                            If (hdPagina.Value = "MODIFICA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                                ConfiguraModoConsulta()
                                                ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                                txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                                tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                                CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
                                                CargarCaracteristicasValor()
                                                UIUtility.ObtieneImpuestosComisionesGuardado(dgLista, txtCodigoOrden.Value, ddlFondo.SelectedValue)
                                                HabilitaBotones(False, False, False, False, False, False, False, True, False, True, False, True, False, False, False)
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

                    '==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-21 | Calculo del TIR Neto
                    Me.pnlTirNeto.Visible = (Me.hdPagina.Value <> "") 'Solo será visible si hdPagina.Value tiene un valor
                    '==== FIN | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-21 | Calculo del TIR Neto

                    btnSalir.Attributes.Remove("onClick")
                    '       btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF49", txtCodigoOrden.Value + "?", "SI")
                    '     Me.btnAceptar.Attributes.Add("onClick", "if (Confirmacion()){this.disabled = true; this.value = 'en proceso...'; __doPostBack('btnAceptar','');}")
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Call HabilitaBotones(True, True, True, True, True, True, True, True, True, True, True, True, False, True, True)
    End Sub
    Private Sub HabilitaBotones(ByVal bCuponera As Boolean, ByVal bLimites As Boolean, ByVal bIngresar As Boolean, ByVal bModificar As Boolean, ByVal bEliminar As Boolean,
    ByVal bConsultar As Boolean, ByVal bAsignar As Boolean, ByVal bProcesar As Boolean, ByVal bImprimir As Boolean, ByVal bAceptar As Boolean, ByVal bBuscar As Boolean,
    ByVal bSalir As Boolean, ByVal bRetornar As Boolean, ByVal bCaracteristicas As Boolean, ByVal bLimitesParametrizados As Boolean)
        btnCuponera.Visible = bCuponera
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
        Session("dtCuponera") = Nothing
    End Sub
    Public Function ObtenerDatosOperacion() As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11",
        "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = tbFechaOperacion.Text
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = tbFechaLiquidacion.Text
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = txtHoraOperacion.Text
        drGrilla("c4") = "Monto Nominal Ordenado"
        drGrilla("v4") = txtMnomOrd.Text
        drGrilla("c5") = "Monto Nominal Operación"
        drGrilla("v5") = txtMnomOp.Text
        drGrilla("c6") = "Tipo Tasa"
        drGrilla("v6") = ddlTipoTasa.SelectedItem.Text
        drGrilla("c7") = "YTM%"
        drGrilla("v7") = txtYTM.Text
        drGrilla("c8") = "Precio Negociación %"
        drGrilla("v8") = txtPrecioNegoc.Text
        drGrilla("c9") = "Precio Calculado %"
        drGrilla("v9") = lblPrecioCal.Text
        drGrilla("c10") = "Precio Negociación Sucio"
        drGrilla("v10") = txtPrecioNegSucio.Text
        drGrilla("c11") = "Interés Corrido Negociado"
        drGrilla("v11") = txtInteresCorNeg.Text
        drGrilla("c12") = "Interés Corrido"
        drGrilla("v12") = lblInteresCorrido.Text
        drGrilla("c13") = "Monto Operación"
        drGrilla("v13") = txtMontoOperacional.Text
        drGrilla("c14") = "Número Papeles"
        drGrilla("v14") = txtNroPapeles.Text
        drGrilla("c15") = "Intermediario"
        drGrilla("v15") = ddlIntermediario.SelectedItem.Text
        If ddlContacto.SelectedIndex <> 0 Then
            drGrilla("c16") = "Contacto"
            drGrilla("v16") = ddlContacto.SelectedItem.Text
        Else
            drGrilla("c16") = ""
            drGrilla("v16") = ""
        End If
        If tbNPoliza.Visible = True Then
            drGrilla("c17") = "Número Poliza"
            drGrilla("v17") = tbNPoliza.Text
        Else
            drGrilla("c17") = ""
            drGrilla("v17") = ""
        End If
        drGrilla("c18") = "Observación"
        drGrilla("v18") = txtObservacion.Text
        drGrilla("c19") = "Total Comisiones"
        drGrilla("v19") = txttotalComisionesC.Text
        drGrilla("c20") = "Monto Neto Operación"
        drGrilla("v20") = txtMontoNetoOpe.Text
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Private Sub btnCuponera_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCuponera.Click
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
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de la orden de inversión de Papeles Comerciales?"
                    Else
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de pre-orden de inversión de Papeles Comerciales?"
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

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        txtPrecioNegSucio.Text = "0.0000000"
        lblInteresCorrido.Text = "0.0000000"
        txtNroPapeles.Text = "0.0000000"
        txtInteresCorNeg.Text = "0.0000000"
        lblPrecioCal.Text = "0.0000000"
        txtPrecioNegoc.Text = "0.0000000"
        txtMontoOperacional.Text = "0.0000000"
        txtYTM.Text = "0.0000000"
        If Session("EstadoPantalla") = "Ingresar" Then
            If Session("Busqueda") = 0 Then
                If ddlFondo.SelectedValue = "" Then
                    If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                        AlertaJS(ObtenerMensaje("CONF42"))
                    ElseIf ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
                        AlertaJS(ObtenerMensaje("CONF43"))
                        Exit Sub
                    End If
                End If
                ShowDialogPopupValores(txtISIN.Text.Trim.ToUpper, txtSBS.Text.Trim.ToUpper, txtMnemonico.Text.Trim.ToUpper, ddlFondo.SelectedValue,
                ddlOperacion.SelectedValue, "PC")
                Session("Busqueda") = 2
            Else
                If Session("Busqueda") = 1 Then
                    CargarFechaVencimiento()
                    If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                        UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
                    Else
                        UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
                    End If
                    CargarPaginaIngresar()
                    If ddlFondo.SelectedValue <> "" Then
                        CargarCaracteristicasValor()

                        UIUtility.ResaltaCajaTexto(tbFechaLiquidacion, True)
                        UIUtility.ResaltaCajaTexto(txtYTM, True)
                        UIUtility.ResaltaCajaTexto(txtNroPapeles, True)
                        UIUtility.ResaltaCombo(ddlTipoTasa, True)

                        UIUtility.ResaltaCombo(ddlGrupoInt, True)
                        UIUtility.ResaltaCombo(ddlIntermediario, True)
                        UIUtility.ResaltaCombo(ddlPlaza, True)
                    End If
                    CargarIntermediario()
                    If UIUtility.ObtenerCodigoTipoOperacion("COMPRA") = ddlOperacion.SelectedValue Then
                        ddlFondo.Enabled = True
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
                    If (hdPagina.Value = "DA") Then
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
                    ShowDialogPopupInversionesRealizadas(txtISIN.Text.ToString.Trim, txtSBS.Text.ToString.Trim, txtMnemonico.Text.ToString.Trim,
                    ddlFondo.SelectedValue, ddlOperacion.SelectedValue, lblMoneda.Text.ToString, strAux, strAccion, ddlFondo.SelectedItem.Text)
                    Session("Busqueda") = 2
                Else
                    If Session("Busqueda") = 1 Then
                        CargarCaracteristicasValor()
                        CargarDatosOrdenInversion(Session("Orden"))
                        btnAceptar.Visible = True
                        'OT 9968 - 14/02/2017 - Carlos Espejo
                        'Descripcion: Se usa la sesion Orden
                        UIUtility.ObtieneImpuestosComisionesGuardado(dgLista, Session("Orden"), ddlFondo.SelectedValue)
                        'OT 9968 Fin
                        Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(Session("Orden"), ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                        If Session("EstadoPantalla") = "Modificar" Then
                            If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF2", "Nro " + txtCodigoOrden.Value + "?", "SI")
                            Else
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF16", "Nro " + txtCodigoOrden.Value + "?", "SI")
                            End If
                            CargarPaginaModificar()
                        ElseIf Session("EstadoPantalla") = "Eliminar" Then
                            If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
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
    End Sub
    Public Sub CargarIntermediario()
        If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
            UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        ElseIf ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
            UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        End If
        Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
    End Sub
    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row hidden")
        LimpiarSesiones()
        'GUID: Identificador único para cuponetas temporales
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        UIUtility.InsertarOtroElementoSeleccion(ddlFondo)
        UIUtility.ExcluirOtroElementoSeleccion(ddlOperacion)
        CargarPaginaAccion()
        Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        If (hdPagina.Value <> "DA") Then
            tbFechaOperacion.Text = objutil.RetornarFechaNegocio
        Else
            tbFechaOperacion.Text = Request.QueryString("Fecha")
        End If
        txtHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Ingresar"
        hdMensaje.Value = "el Ingreso"
        ViewState("IsIndica") = True
        hdNumUnidades.Value = 0
        If Not ddlFondo.Items.FindByValue(PORTAFOLIO_MULTIFONDOS) Is Nothing Then
            ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS
        End If
        lblTitulo.Text = "PreOrden de Inversión - PAPELES COMERCIALES"

        HabilitarBotonGuardarPreOrden()
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row")
        LimpiarSesiones()
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        UIUtility.ExcluirOtroElementoSeleccion(ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion, "")
        Session("EstadoPantalla") = "Modificar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        lblAccion.Text = "Modificar"
        CargarPaginaAccion()
        hdMensaje.Value = "la Modificación"
        ViewState("IsIndica") = False
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row")
        LimpiarSesiones()
        UIUtility.ExcluirOtroElementoSeleccion(ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion, "")
        Session("EstadoPantalla") = "Eliminar"
        lblAccion.Text = "Eliminar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Eliminación"
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    Public Sub CargarFechaVencimiento()
        If (hdPagina.Value <> "CO") Then
            If (hdPagina.Value <> "DA") Then
                Dim dtAux As DataTable = oPortafolioBM.SeleccionarPortafolioPorFiltro(ddlFondo.SelectedValue, DatosRequest).Tables(0)
                If Not dtAux Is Nothing Then
                    If dtAux.Rows.Count > 0 Then
                        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
                    End If
                End If
            Else
                tbFechaOperacion.Text = Request.QueryString("Fecha")
            End If
            If (txtMnemonico.Text.Trim <> "") Then
                tbFechaLiquidacion.Text = oOrdenInversionBM.RetornarFechaVencimiento(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text,
                ddlFondo.SelectedValue, ddlIntermediario.SelectedValue)
            End If
        End If
    End Sub
    Private Sub CargarDatosOrdenInversion(ByVal Orden As String)
        Try
            'OT 9968 - 14/02/2017 - Carlos Espejo
            'Descripcion: Se usa la sesion Orden
            'OT 10019 - 22/02/2017 - Carlos Espejo
            'Descripcion: Se agrega el parametro CodigoOrden
            oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(Orden, ddlFondo.SelectedValue, DatosRequest, PORTAFOLIO_MULTIFONDOS)
            'OT 10019 Fin
            'OT 9968 Fin
            Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
            oRow = oOrdenInversionBE.Tables(0).Rows(0)
            Session("CodigoMoneda") = oRow.CodigoMoneda
            txtISIN.Text = oRow.CodigoISIN
            txtMnemonico.Text = oRow.CodigoMnemonico
            Session("Nemonico") = oRow.CodigoMnemonico
            txtCodigoOrden.Value = oRow.CodigoOrden
            If oRow.CodigoOperacion.ToString <> "" Then
                ddlOperacion.SelectedIndex = ddlOperacion.Items.IndexOf(ddlOperacion.Items.FindByValue(oRow.CodigoOperacion.ToString()))
            Else
                ddlOperacion.SelectedIndex = 0
            End If
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
            txtMnomOrd.Text = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
            txtMnomOp.Text = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
            ddlTipoTasa.SelectedValue = oRow.CodigoTipoCupon
            txtYTM.Text = Format(oRow.YTM, "##,##0.0000000")
            lblInteresCorrido.Text = Format(oRow.InteresCorrido, "##,##0.0000000")

            txtInteresCorNeg.Text = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
            txtHoraOperacion.Text = oRow.HoraOperacion
            txtMontoOperacional.Text = Format(oRow.MontoOperacion, "##,##0.0000000")

            txtPrecioLimpio.Text = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
            txtPrecioNegSucio.Text = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
            lblPrecioCal.Text = Format(oRow.PrecioCalculado, "##,##0.0000000")
            txtPrecioNegoc.Text = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")

            txtNroPapeles.Text = Format(oRow.CantidadOperacion, "##,##0.0000000")

            txtObservacion.Text = oRow.Observacion
            txtObservacionCarta.Text = oRow.ObservacionCarta

            hdNumUnidades.Value = txtMontoOperacional.Text
            lblpreciovector.Text = Format(oRow.Precio, "##,##0.0000000")
            txttotalComisionesC.Text = Format(oRow.TotalComisiones, "##,##0.0000000")
            txtMontoNetoOpe.Text = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
            tbNPoliza.Text = oRow.NumeroPoliza.ToString()
            ViewState("MontoNeto") = Convert.ToDecimal(txtMontoOperacional.Text)
            CargarPlaza()
            ddlPlaza.SelectedValue = oRow.Plaza
            ddlGrupoInt.SelectedValue = oRow.GrupoIntermediario
            Session("CodigoOI") = txtCodigoOrden.Value
            Dim dtAux As DataTable
            dtAux = (New TercerosBM().Seleccionar(oRow.CodigoTercero, DatosRequest)).Tables(0)
            If dtAux.Rows.Count > 0 Then
                hdCustodio.Value = dtAux.Rows(0)("CodigoCustodio")
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

            ' INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Tipo Valorizacion
            If oRow.TipoValorizacion.ToString <> "" Then
                ddlCategoriaContable.SelectedIndex = ddlCategoriaContable.Items.IndexOf(ddlCategoriaContable.Items.FindByValue(oRow.TipoValorizacion.ToString()))
            Else
                ddlCategoriaContable.SelectedIndex = 0
            End If
            ddlCategoriaContable.Enabled = IIf(ObtenerTipoFondo() = "MANDA", True, False)

            txtTIRNeto.Text = Format(oRow.TirNeta, "##,##0.0000000")
            If oRow.TirNeta = 0 Then txtTIRNeto.Text = Format(oRow.YTM, "##,##0.0000000")
            ' FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | 

        Catch ex As Exception
            If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                AlertaJS(ObtenerMensaje("CONF31"))
            Else
                AlertaJS(ObtenerMensaje("CONF32"))
            End If
        End Try
    End Sub
    Private Sub CargarCaracteristicasValor()
        Dim dsValor As New DataSet
        Dim drValor As DataRow
        Dim oOIFormulas As New OrdenInversionFormulasBM
        Try
            'OT 9968 - 14/02/2017 - Carlos Espejo
            'Descripcion: Se usa la sesion Nemonico
            dsValor = oOIFormulas.SeleccionarCaracValor_PapelesComerciales(Session("Nemonico"), ddlFondo.SelectedValue, DatosRequest)
            'OT 9968 Fin
            If dsValor.Tables(0).Rows.Count > 0 Then
                drValor = dsValor.Tables(0).NewRow
                drValor = dsValor.Tables(0).Rows(0)
                drValor = dsValor.Tables(0).NewRow
                drValor = dsValor.Tables(0).Rows(0)
                Session("TipoRenta") = CType(drValor("val_TipoRenta"), String)
                Session("CodigoMoneda") = CType(drValor("val_CodigoMoneda"), String)
                If Not ((hdPagina.Value = "EO") Or (hdPagina.Value = "CO") Or (hdPagina.Value = "XO") Or (hdPagina.Value = "MODIFICA")) Then
                    Session("Mercado") = CType(drValor("val_Mercado"), String)
                End If
                lblMoneda.Text = CType(drValor("val_CodigoMoneda"), String)
                txtISIN.Text = CType(drValor("val_CodigoISIN"), String)
                txtSBS.Text = CType(drValor("val_CodigoSBS"), String)
                lblbasecupon.Text = CType(Math.Round(Convert.ToDecimal(drValor("val_BaseCupon")), Constantes.M_INT_NRO_DECIMALES), Integer)
                lblbasetir.Text = CType(Math.Round(Convert.ToDecimal(drValor("val_BaseTir")), Constantes.M_INT_NRO_DECIMALES), Integer)
                lbldescripcion.Text = CType(drValor("val_Descripcion"), String)
                lblduracion.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_Duracion")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                lblemisor.Text = CType(drValor("val_Emisor"), String)
                lblnominales.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_NominalesEmitidos")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                lblUnidades.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_NominalesUnitarias")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                lblfecfinbono.Text = CType(drValor("val_FechaFinBono"), String)
                lblFecProxCupon.Text = CType(drValor("val_FechaProxCupon"), String)
                lblFecUltCupon.Text = CType(drValor("val_FechaUltCupon"), String)
                lblpreciovector.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_VectorPrecio")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                lblparticipacion.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_PorParticipacion")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                lblSaldoValor.Text = CType(drValor("SaldoValor"), String)
                If drValor("val_Rescate") = "N" Then
                    lblRescate.Text = "NO"
                ElseIf drValor("val_Rescate") = "S" Then
                    lblRescate.Text = "SI"
                Else
                    lblRescate.Text = ""
                End If

                '==== INICIO | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 11/07/2018 
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
                ViewState("DatosValor_EsMercadoExtrangero") = drValor("val_Mercado").ToString.Equals("2") 'Mercado = 2 : Es equivalente a MERCADO EXTRANJERO

                ViewState("DatosValor_BaseCuponMensual") = drValor("BaseCuponMensual")
                ViewState("DatosValor_BaseCuponAnual") = drValor("BaseCuponAnual")
                '==== FIN | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 29/05/2018
                If Me.hdPagina.Value <> "CO" Then
                    ddlCategoriaContable.SelectedValue = CType(IIf(ObtenerTipoFondo() = "MANDA", "DIS_VENTA", "VAL_RAZO"), String)
                    ddlCategoriaContable.Enabled = IIf(ObtenerTipoFondo() = "MANDA", True, False)
                End If
            End If
        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF21"))
        End Try
    End Sub
    Private Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos()
        CargarFechaVencimiento()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        Session("Mercado") = ddlPlaza.SelectedValue
        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
    End Sub
    Private Sub CargarContactos()
        Dim objContacto As New ContactoBM
        ddlContacto.DataTextField = "DescripcionContacto"
        ddlContacto.DataValueField = "CodigoContacto"
        ddlContacto.DataSource = objContacto.ListarContactoPorTerceros(ddlIntermediario.SelectedValue)
        ddlContacto.DataBind()
        ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        Dim i As Integer
        Dim dtAux As DataTable
        dtAux = CType(Session("datosEntidad"), DataTable)
        If Not dtAux Is Nothing Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("CodigoTercero") = ddlIntermediario.SelectedValue Then
                    If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                        hdCustodio.Value = dtAux.Rows(i)("codigoCustodio")
                    End If
                    Session("Mercado") = dtAux.Rows(i)("mercado")
                    dgLista.Dispose()
                    dgLista.DataBind()
                    ObtieneImpuestosComisiones()
                    Exit For
                End If
            Next
        End If
    End Sub
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Se quita validacion de trader version antigua
    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Dim feriado As New FeriadoBM
        Try
            If ValidarFechas() = True Then
                If UIUtility.ValidarHora(txtHoraOperacion.Text) = False Then
                    AlertaJS(ObtenerMensaje("CONF22"))
                    Exit Sub
                End If
                If (feriado.VerificaDia(UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text), Session("Mercado")) = False) Then
                    AlertaJS("Fecha de Vencimiento no es valida.")
                    Exit Sub
                End If
                ViewState("estadoOI") = ""
                Session("Procesar") = 1

                txtMnomOrd.Text = Format(Convert.ToDecimal(txtNroPapeles.Text) * CDec(ViewState("DatosValor_ValorNominalUnitario")), "##,##0.0000000")
                txtMnomOp.Text = txtMnomOrd.Text

                '==== INICIO | PROYECTO FONDOS-II: RF007 | ZOLUXIONES | CRumiche | 29/05/2018
                Dim baseAnual As BaseAnualCupon = UIUtility.ObtenerBaseAnualDesdeTexto(ViewState("DatosValor_BaseCuponAnual"))
                Dim baseMensual As BaseMensualCupon = UIUtility.ObtenerBaseMensualDesdeTexto(ViewState("DatosValor_BaseCuponMensual"))
                Dim aplicacionTasa As TipoAplicacionTasa = UIUtility.ObtenerTipoAplicacionTasaDesdeCodTipoTasa(ddlTipoTasa.SelectedValue)
                Dim diasPeriodicidad As Integer = 360 'Periodicidad Anual (Un CUPON x Año)  


                'Pasamos a calcular la negociacion (El motor hará todo el trabajo en tiempo real y en la capa de Negocio)
                Dim neg As NegociacionRentaFija = UIUtility.CalcularNegociacionRentaFija(ViewState("DatosValor_DetalleCupones"),
                                                                    CDec(ViewState("DatosValor_TasaCupon")),
                                                                    diasPeriodicidad,
                                                                    CDec(ViewState("DatosValor_ValorNominalUnitario")),
                                                                    Convert.ToDecimal(txtNroPapeles.Text),
                                                                    Convert.ToDateTime(tbFechaLiquidacion.Text),
                                                                    Convert.ToDecimal(txtYTM.Text),
                                                                    baseMensual,
                                                                    baseAnual,
                                                                    CBool(ViewState("DatosValor_EsCuponADescuento")),
                                                                    CBool(ViewState("DatosValor_EsMercadoExtrangero")),
                                                                    aplicacionTasa)

                'Mostramos los resultados -
                txtMontoOperacional.Text = Format(neg.ValorActual, "##,##0.00")

                txtPrecioLimpio.Text = Format(neg.PrecioLimpio * 100, "##,##0.0000000") '%: Es un Porcentaje
                txtPrecioNegSucio.Text = Format(neg.PrecioSucio * 100, "##,##0.0000000") '%: Es un Porcentaje
                lblPrecioCal.Text = Format(neg.PrecioSucio * 100, "##,##0.0000000") '%: Es un Porcentaje
                txtPrecioNegoc.Text = Format(neg.PrecioSucio * 100, "##,##0.0000000") '%: Es un Porcentaje

                lblInteresCorrido.Text = Format(neg.InteresCorrido, "##,##0.0000") 'Deberia ser CERO
                txtInteresCorNeg.Text = Format(neg.InteresCorrido, "##,##0.0000")
                Session("NegociacionRentaFija") = neg
                '==== FIN | PROYECTO FONDOS-II: RF007 | ZOLUXIONES | CRumiche | 29/05/2018

                If ObtieneCustodiosSaldos() = False Then
                    AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                    Exit Sub
                End If
                If (chkRecalcular.Checked) Then
                    txtComisionSAB = CType(dgLista.Rows(0).FindControl("txtValorComision2"), TextBox)
                    If ddlPlaza.SelectedValue = "7" And Session("TipoRenta").ToString = "1" And dgLista.Rows.Count > 2 And txtComisionSAB.Text.Trim <> String.Empty And chkRecalcular.Checked Then
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
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Function ObtieneCustodiosSaldos() As Boolean
        Dim decAux As Decimal
        Dim strCodigoOperacion As String = String.Empty
        If Session("EstadoPantalla") = "Ingresar" Or Session("EstadoPantalla") = "Modificar" Then
            If VerificarSaldosCustodios(decAux) = False Then
                'Interpretar Codigos de Operacio.00 "n (Según Tipo de Negociacion)
                If hdPagina.Value = "TI" Then
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
    Private Function VerificarSaldosCustodios(ByRef decAux As Decimal) As Boolean
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        Dim decMontoAux As Decimal = 0.0
        Dim cantCustodios As Integer = 0
        Dim oPrevOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim dtSumaUnidades As DataTable
        Try
            Dim decUnidades As Decimal
            If lblUnidades.Text = "0" Then
                decUnidades = 1
            Else
                decUnidades = Convert.ToDecimal(lblUnidades.Text.Replace(".", UIUtility.DecimalSeparator))
            End If
            Dim decValorLocal As Decimal = Convert.ToDecimal(txtMnomOp.Text.Replace(".", UIUtility.DecimalSeparator)) / decUnidades
            decAux = Math.Round(decValorLocal, Constantes.M_INT_NRO_DECIMALES)

            dtSumaUnidades = oPrevOrdenInversionBM.ObtenerUnidadesNegociadasDiaT(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text).Tables(0)
            If dtSumaUnidades.Rows.Count > 0 Then
                decAux += Decimal.Parse(dtSumaUnidades.Compute("Sum(UNIDADES)", String.Empty))
                If Me.hdPagina.Value = "CO" Then
                    decAux -= CDec(hdNumUnidades.Value)
                End If
            End If

            If hdNumUnidades.Value = Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".") Then
                Return True
            End If
            '********COMPRA*********
            If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                Return True
            End If
            '********VENTA*********
            Session.Remove("ValorCustodio")
            If Session("ValorCustodio") Is Nothing Then
                Session("ValorCustodio") = hdCustodio.Value + strSeparador + hdSaldo.Value
            ElseIf Session("ValorCustodio") = "" Then
                Session("ValorCustodio") = hdCustodio.Value + strSeparador + hdSaldo.Value
            End If
            decMontoAux = Convert.ToDecimal(lblSaldoValor.Text)
            If decMontoAux = decAux Then
                hdNumUnidades.Value = Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".")
                Return True
            ElseIf decMontoAux > decAux Then
                'redefinir calculos porq excede
                If cantCustodios = 1 Then
                    'porque solamente es el primer custodio
                    Session("ValorCustodio") = hdCustodio.Value + strSeparador + Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".")
                    hdNumUnidades.Value = Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".")
                    Return True
                Else
                    'porq hay mas de un custodio. ajustar montos
                    Session("ValorCustodio") = UIUtility.AjustarMontosCustodios(CType(Session("ValorCustodio"), String), Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, "."))
                    hdNumUnidades.Value = Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".")
                    Return True
                End If
                Return False
            ElseIf decMontoAux < decAux Then
                Session("ValorCustodio") = hdCustodio.Value + strSeparador + hdSaldo.Value
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function
    Private Sub CalcularComisiones()
        Dim dblTotalComisiones As Decimal = 0.0
        If chkRecalcular.Checked Then
            dblTotalComisiones = UIUtility.CalcularComisionesYLlenarGridView(dgLista, String.Empty, txtMontoOperacional.Text.Replace(",", ""), UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text, ddlIntermediario.SelectedValue)
            If dgLista.Rows.Count > 0 Then validarComisionSAB(dblTotalComisiones)
        Else
            'If hdPagina.Value = "TI" Then
            '    dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), txtMontoOperacional.Text.Replace(",", ""),
            '    txtNroPapeles.Text.Replace(",", ""))
            'Else
            '    dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), txtMontoOperacional.Text.Replace(",", ""),
            '       txtNroPapeles.Text.Replace(",", ""))
            '    '    dblTotalComisiones = UIUtility.CalcularComisionesYLlenarGridView(dgLista, String.Empty, txtMontoOperacional.Text.Replace(",", ""), UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text, ddlIntermediario.SelectedValue)
            'End If

            dblTotalComisiones = UIUtility.CalculaImpuestosComisionesNoRecalculo(dgLista, Session("Mercado"), txtMontoOperacional.Text.Replace(",", ""),
                txtNroPapeles.Text.Replace(",", ""), ddlGrupoInt.SelectedValue, ddlOperacion.SelectedValue, CLASE_INSTRUMENTO_ACCION)
        End If
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        If (ddlOperacion.SelectedValue = "2") Then
            txtMontoNetoOpe.Text = Format(txtMontoOperacional.Text.Replace(",", "") - dblTotalComisiones, "##,##0.00")
        Else
            txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoOperacional.Text.Replace(",", ""), "##,##0.00")
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


    Private Sub actualizaMontos()
        Dim dblTotalComisiones As Decimal = 0.0
        dblTotalComisiones = UIUtility.ActualizaMontosFinales(dgLista)
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - De acuerdo al tipo de operación se restará o sumará al valor total | 27/06/18 
        If (ddlOperacion.SelectedValue = "2") Then dblTotalComisiones = dblTotalComisiones * -1
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - De acuerdo al tipo de operación se restará o sumará al valor total | 27/06/18 
        If Session("Mercado") = 1 Then  'local
            txtMontoNetoOpe.Text = Format(txtMontoOperacional.Text.Replace(".", UIUtility.DecimalSeparator) + dblTotalComisiones, "##,##0.0000000")
        ElseIf Session("Mercado") = 2 Then  'extranjero
            txtMontoNetoOpe.Text = Format(txtNroPapeles.Text.Replace(".", UIUtility.DecimalSeparator) + dblTotalComisiones, "##,##0.0000000")
        End If
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        Dim accionRpta As String = String.Empty
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        Try
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
            If hdRptaConfirmar.Value.ToUpper = "SI" Then
                EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
                If ObtieneCustodiosSaldos() = False Then
                    AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                    Exit Sub
                End If
                If hdPagina.Value <> "" And hdPagina.Value <> "DA" And hdPagina.Value <> "TI" And hdPagina.Value <> "MODIFICA" Then
                    If hdPagina.Value = "EO" Or hdPagina.Value = "CO" Then
                        ModificarOrdenInversion()
                        UIUtility.InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"),
                        ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                        CargarPaginaAceptar()
                    End If
                    If hdPagina.Value = "XO" Then
                        oOrdenInversionWorkFlowBM.ExtornarOIEjecutadas(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest)
                        ReturnArgumentShowDialogPopup()
                    Else
                        If hdPagina.Value = "EO" Then
                            oOrdenInversionWorkFlowBM.EjecutarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, tbNPoliza.Text.Trim, DatosRequest)
                            ReturnArgumentShowDialogPopup()
                        Else
                            If hdPagina.Value = "CO" Then
                                oOrdenInversionWorkFlowBM.ConfirmarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, tbNPoliza.Text.Trim, DatosRequest)
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
                            'OT 9968 Se quito validacion de observacion de cambio obligatoria
                            If strAlerta.Length > 0 Then
                                AlertaJS(strAlerta)
                                Exit Sub
                            End If
                        End If
                        If ObtieneCustodiosSaldos() = False Then
                            AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                            Exit Sub
                        End If
                        actualizaMontos()

                        If Session("EstadoPantalla") = "Ingresar" Then
                            If Session("NegociacionRentaFija") IsNot Nothing Then
                                If Session("Procesar") = 1 Then
                                    'Dim strcodigoOrden As String
                                    'strcodigoOrden = InsertarOrdenInversion()
                                    'If strcodigoOrden <> "" Then

                                    '    Dim toUser As String = ""
                                    '    Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                                    '    Dim dt As DataTable
                                    '    If ViewState("estadoOI") = "E-EXC" Then
                                    '        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                                    '        For Each fila As DataRow In dt.Rows
                                    '            toUser = toUser + fila("Valor").ToString() & ";"
                                    '        Next
                                    '        Try
                                    '            UIUtility.EnviarMail(toUser, "", "Pendiente de aprobación - Orden excedido por Limites de Inversión",
                                    '            MensajeExcesodeLimite(strcodigoOrden), DatosRequest)
                                    '        Catch ex As Exception
                                    '            AlertaJS("Se ha generado un error en el proceso de envio de notificación! ")
                                    '        End Try
                                    '    End If
                                    '    Session("dtValTrading") = ""
                                    'End If
                                    'oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, ddlFondo.SelectedValue, "", DatosRequest)
                                    'txtCodigoOrden.Value = strcodigoOrden
                                    'If hdPagina.Value <> "TI" Then
                                    '    UIUtility.InsertarModificarImpuestosComisiones("I", dgLista, strcodigoOrden, ddlPlaza.SelectedValue, Session("TipoRenta"),
                                    '    ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                                    'End If
                                    'Session("dtdatosoperacion") = ObtenerDatosOperacion()
                                    'GenerarLlamado(strcodigoOrden + ViewState("CodigoOrden_T"), ddlFondo.SelectedValue, "PAPELES COMERCIALES", ddlOperacion.SelectedItem.Text,
                                    'Session("CodigoMoneda"), txtISIN.Text.Trim, txtSBS.Text.Trim, txtMnemonico.Text, ddlFondo.SelectedItem.Text)
                                    'Session("CodigoOI") = strcodigoOrden

                                    'INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
                                    GuardarPreOrden()
                                    accionRpta = "Ingresó"
                                    CargarPaginaAceptar()
                                    'FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
                                End If
                            Else
                                AlertaJS("De click en el botón Procesar para generar los datos de la negociación")
                            End If
                        Else
                            If Session("EstadoPantalla") = "Modificar" Then
                                actualizaMontos()
                                UIUtility.InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"),
                                ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                                ModificarOrdenInversion()
                                FechaEliminarModificarOI("M")
                                CargarPaginaAceptar()
                                Session("dtdatosoperacion") = ObtenerDatosOperacion()
                                If hdPagina.Value <> "MODIFICA" Then
                                    GenerarLlamado(txtCodigoOrden.Value, ddlFondo.SelectedValue, "PAPELES COMERCIALES", ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"),
                                    txtISIN.Text.Trim, txtSBS.Text.Trim, txtMnemonico.Text, ddlFondo.SelectedItem.Text)
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
                                lblAccion.Text = ""
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
                        EliminarCuponerasOITemporales()
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
        Dim orden As String = "Papeles Comerciales"
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
        If hdPagina.Value = "TI" Then
            oOrdenInversionBE.OrdenPreOrdenInversion.Rows(0)("CodigoPortafolioSBS") = ddlFondoDestino.SelectedValue
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
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
    End Sub
    Public Sub EliminarOrdenInversion()
        oOrdenInversionBM.EliminarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, ddlMotivoCambio.SelectedValue, DatosRequest)
        oImpComOP.Eliminar(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest)
    End Sub
    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(ddlFondo.SelectedValue, txtCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")),
        tProc, txtComentarios.Text, DatosRequest)
        txtComentarios.Text = ""
    End Sub
    Public Function crearObjetoOI() As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        oOrdenInversionBM.InicializarOrdenInversion(oRow)
        oRow.CodigoOrden = txtCodigoOrden.Value
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.CodigoMoneda = Session("CodigoMoneda")
        oRow.CodigoISIN = txtISIN.Text
        oRow.CodigoMnemonico = txtMnemonico.Text
        oRow.CodigoSBS = txtSBS.Text
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
        oRow.MontoNominalOrdenado = Math.Round(Convert.ToDecimal(txtMnomOrd.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.MontoNominalOperacion = Math.Round(Convert.ToDecimal(txtMnomOp.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.CodigoTipoCupon = ddlTipoTasa.SelectedValue
        oRow.YTM = Math.Round(Convert.ToDecimal(txtYTM.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.InteresCorrido = Math.Round(Convert.ToDecimal(lblInteresCorrido.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)

        oRow.PrecioNegociacionLimpio = Math.Round(Convert.ToDecimal(txtPrecioLimpio.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.PrecioNegociacionSucio = Math.Round(Convert.ToDecimal(txtPrecioNegSucio.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.PrecioCalculado = Math.Round(Convert.ToDecimal(lblPrecioCal.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)

        oRow.Situacion = "A"
        oRow.InteresCorridoNegociacion = Math.Round(Convert.ToDecimal(txtInteresCorNeg.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.GrupoIntermediario = ddlGrupoInt.SelectedValue
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.MontoOperacion = Math.Round(Convert.ToDecimal(txtMontoOperacional.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.CantidadOperacion = Math.Round(Convert.ToDecimal(txtNroPapeles.Text), 0)

        oRow.Observacion = txtObservacion.Text
        oRow.ObservacionCarta = txtObservacionCarta.Text

        oRow.HoraOperacion = txtHoraOperacion.Text
        oRow.Precio = Math.Round(Convert.ToDecimal(lblpreciovector.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.TotalComisiones = Convert.ToDecimal(txttotalComisionesC.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.MontoNetoOperacion = Convert.ToDecimal(txtMontoNetoOpe.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.CategoriaInstrumento = "PC"    'UNICO POR TIPO
        oRow.Plaza = ddlPlaza.SelectedValue
        If Not ViewState("estadoOI") Is Nothing Then
            If ViewState("estadoOI").Equals("E-EXC") Then
                oRow.Estado = ViewState("estadoOI")
            End If
        End If
        If (hdPagina.Value <> "XO") Then
            oRow.NumeroPoliza = tbNPoliza.Text.ToString().Trim
        End If
        If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
            If ddlMotivoCambio.SelectedIndex > 0 Then
                oRow.CodigoMotivoCambio = ddlMotivoCambio.SelectedValue
            End If
            If Session("EstadoPantalla") = "Modificar" Then
                oRow.IndicaCambio = "1"
            End If
        End If
        oRow.Ficticia = "N"
        If (chkRegulaSBS.Checked) Then
            oRow.RegulaSBS = "S"
        Else
            oRow.RegulaSBS = "N"
        End If
        ' INICIO | Proyecto SIT Fondos - Mandato | Ian Pastor M. | 2018-09-21 | Categoría Contable
        oRow.TipoValorizacion = ddlCategoriaContable.SelectedValue
        ' FIN | Proyecto SIT Fondos - Mandato | Ian Pastor M. | 2018-09-21 | Categoría Contable

        ' INICIO | Proyecto SIT Fondos - Mandato | CRumiche | 2018-09-21 | Tir Neta
        oRow.TirNeta = Math.Round(Convert.ToDecimal(Me.txtTIRNeto.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        ' FIN | Proyecto SIT Fondos - Mandato | CRumiche | 2018-09-21 | Tir Neta

        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
    Private Function ValidarFechas() As Boolean
        Dim dsFechas As PortafolioBE
        Dim drFechas As DataRow
        Dim blnResultado As Boolean = True
        If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text) Then
            blnResultado = False
            AlertaJS(ObtenerMensaje("CONF7"))
        End If
        If (hdPagina.Value = "DA") Then
            Return True
        End If
        dsFechas = oPortafolioBM.Seleccionar(ddlFondo.SelectedValue, DatosRequest)
        If dsFechas.Tables(0).Rows.Count > 0 Then
            drFechas = dsFechas.Tables(0).NewRow
            drFechas = dsFechas.Tables(0).Rows(0)
            Dim dblFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            Dim dblFechaConstitucion As Decimal = CType(drFechas("FechaConstitucion"), Decimal)
            Dim dblFechaTermino As Decimal = CType(drFechas("FechaTermino"), Decimal)
            If dblFechaConstitucion > dblFechaOperacion Then
                blnResultado = False
                AlertaJS(ObtenerMensaje("CONF4"))
            End If
        End If
        If (objferiadoBM.BuscarPorFecha(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))) = True Then
            blnResultado = False
        End If
        Return blnResultado
    End Function
    Private Sub ConfiguraModoConsulta()
        UIUtility.ExcluirOtroElementoSeleccion(ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion, "")
        Session("EstadoPantalla") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Consulta"
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row hidden")
        LimpiarSesiones()
        Call ConfiguraModoConsulta()
    End Sub
    Private Sub btnCaracteristicas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCaracteristicas.Click
        If txtMnemonico.Text <> "" Then
            Session("accionValor") = "MODIFICAR"
            EjecutarJS(UIUtility.MostrarPopUp("../../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" + txtMnemonico.Text + "&vOI=T", "10",
            1045, 600, 0, 0, "No", "No", "Yes", "Yes"), False)
        Else
            AlertaJS(ObtenerMensaje("CONF23"))
        End If
    End Sub
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String,
    ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal nomPortafolio As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&vportafolio=" + nomPortafolio + "&cportafolio=" + portafolio +
        "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0,
        "No", "Yes", "Yes", "Yes"), False)
    End Sub
#Region " /* Métodos Personalizados (Popups Dialogs) */ "
    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal operacion As String,
    ByVal categoria As String)
        Dim strURL As String = "frmBuscarValor.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&cFondo=" + fondo + "&vOperacion=" + operacion +
        "&vCategoria=" + categoria
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hdPopUp').value='V'; ")
    End Sub
    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal operacion As String,
    ByVal moneda As String, ByVal fecha As String, ByVal accion As String, ByVal nomFondo As String)
        Dim strURL As String = "../frmInversionesRealizadas.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&vFondo=" + nomFondo + "&cFondo=" +
        fondo + "&vOperacion=" + operacion + "&vFechaOperacion=" + fecha + "&vAccion=" + accion + "&vCategoria=PC"
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hdPopUp').value='IR'; ")
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
            strCodigoOrden = txtCodigoOrden.Value
            strCodigoPortafolioSBS = ddlFondo.SelectedValue
        End If
        Dim strCodigoMnemonico As String = mnemonico
        Dim strInteresCorrido As String = Convert.ToDecimal(txtInteresCorNeg.Text.Replace(",", "")).ToString()
        Dim strMontoOperacion As String = Convert.ToDecimal(txtMontoOperacional.Text.Replace(",", "")).ToString()
        Dim strPrecioCalculado As String = Convert.ToDecimal(lblPrecioCal.Text.Replace(",", "")).ToString()
        Dim strFechaOperacion As String = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        Dim strURL As String = "frmConsultaCuponeras.aspx?CodigoMnemonico=" + strCodigoMnemonico + "&guid=" + strGuid + "&codigoOrden=" + strCodigoOrden +
        "&CodigoPortafolioSBS=" + strCodigoPortafolioSBS + "&InteresCorrido=" + strInteresCorrido + "&MontoOperacion=" + strMontoOperacion + "&PrecioCalculado=" +
        strPrecioCalculado + "&FechaOperacion=" + strFechaOperacion
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', ''); ")
    End Sub
#End Region
#Region " /* Métodos Controla Habilitar/Deshabilitar Campos */ "
    Private Sub ControlarCamposTI()
        UIUtility.CargarPortafoliosOI(ddlFondoDestino)
        lblFondo.InnerText = "Fondo Origen"
        lblFondoDestino.Visible = True
        ddlFondoDestino.Visible = True
        MostrarOcultarBotonesAcciones(False)
        Session("ValorCustodio") = ""
        Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        CargarFechaVencimiento()
        txtHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Traspaso"
        hdMensaje.Value = "el Ingreso"
        If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
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
        btnAceptar.Visible = True
    End Sub
    Private Sub CargarPaginaModificarEO_CO_XO(ByVal acceso As String)
        If acceso = "EO" Or acceso = "CO" Then
            CargarPaginaBuscar()
            HabilitaDeshabilitaCabecera(False)
            btnBuscar.Visible = False
            HabilitaDeshabilitaDatosOperacionComision(True)
            btnCaracteristicas.Visible = True
            btnCaracteristicas.Enabled = True
            btnAceptar.Visible = True
            Session("EstadoPantalla") = "Modificar"
            'GUID: Identificador único para cuponetas temporales
            Dim GUID As Guid = System.Guid.NewGuid()
            ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        End If
    End Sub
    Private Sub ControlarCamposOE()
        MostrarOcultarBotonesAcciones(False)
        btnAceptar.Visible = True
    End Sub
    Private Sub CargarPaginaInicio()
        HabilitaDeshabilitaCabecera(False)
        HabilitaDeshabilitaDatosOperacionComision(False)
        OcultarBotonesInicio()
        btnAceptar.Visible = False
    End Sub
    Private Sub CargarPaginaBuscar()
        btnProcesar.Visible = True
        btnProcesar.Enabled = True
    End Sub
    Private Sub CargarPaginaIngresar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        btnCaracteristicas.Visible = True
        btnCaracteristicas.Enabled = True
    End Sub
    Private Sub CargarPaginaModificar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        btnCaracteristicas.Visible = True
        btnCaracteristicas.Enabled = True
    End Sub
    Private Sub CargarPaginaEliminar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        btnCaracteristicas.Visible = True
        btnCaracteristicas.Enabled = True
        CargarPaginaProcesar()
    End Sub
    Private Sub CargarPaginaConsultar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        btnCaracteristicas.Visible = True
        btnCaracteristicas.Enabled = True
        CargarPaginaProcesar()
        btnAceptar.Visible = False
    End Sub
    Private Sub CargarPaginaProcesar()
        Dim strJS As New StringBuilder
        strJS.AppendLine("$('#btnCuponera').removeAttr('disabled');")
        strJS.AppendLine("$('#btnCuponera').show();")
        strJS.AppendLine("$('#btnAceptar').removeAttr('disabled');")
        If Session("EstadoPantalla") <> "Ingresar" Then
            strJS.AppendLine("$('#btnImprimir').show();")
            strJS.AppendLine("$('#btnImprimir').removeAttr('disabled');")
        End If
        EjecutarJS(strJS.ToString())
    End Sub
    Private Sub CargarPaginaAceptar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        If Session("EstadoPantalla") = "Ingresar" Then
            btnImprimir.Visible = True
            btnImprimir.Enabled = True
        End If
        btnAceptar.Visible = False
    End Sub
    Private Sub CargarPaginaAccion()
        CargarPaginaInicio()
        LimpiarCaracteristicasValor()
        LimpiarDatosOperacion()
        HabilitaDeshabilitaCabecera(True)
        btnBuscar.Visible = True
        btnBuscar.Enabled = True
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
        dgLista.Enabled = estado
        HabilitaDeshabilitaValoresGrilla(estado)
        tbFechaLiquidacion.ReadOnly = Not estado
        'txtMnomOp.ReadOnly = Not estado
        'txtMnomOrd.ReadOnly = Not estado
        ddlTipoTasa.Enabled = estado
        txtYTM.ReadOnly = Not estado
        'txtPrecioNegoc.ReadOnly = Not estado
        'txtInteresCorNeg.ReadOnly = Not estado
        ddlGrupoInt.Enabled = estado
        ddlIntermediario.Enabled = estado
        ddlContacto.Enabled = estado
        txtMontoOperacional.ReadOnly = Not estado
        txtPrecioNegSucio.ReadOnly = Not estado
        txtHoraOperacion.ReadOnly = Not estado
        txtNroPapeles.ReadOnly = Not estado
        txtObservacion.ReadOnly = Not estado
        txtMontoNetoOpe.ReadOnly = Not estado
        Me.ddlPlaza.Enabled = estado
        If estado Then
            imgFechaVcto.Attributes.Add("class", "input-append date")
        Else
            imgFechaVcto.Attributes.Add("class", "input-append")
        End If
        If (hdPagina.Value = "DA") Then
            tbFechaOperacion.ReadOnly = True
        End If
        If ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
            chkRegulaSBS.Enabled = False
        Else
            chkRegulaSBS.Enabled = estado
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
        btnBuscar.Visible = False
        btnCaracteristicas.Visible = False
        btnCuponera.Visible = False
        btnProcesar.Visible = False
        btnImprimir.Visible = False
    End Sub
    Private Sub MostrarOcultarBotonesAcciones(ByVal estado As Boolean)
        btnIngresar.Visible = estado
        btnModificar.Visible = estado
        btnEliminar.Visible = estado
        btnConsultar.Visible = estado
    End Sub
    Private Sub LimpiarCaracteristicasValor()
        lbldescripcion.Text = ""
        lblfecfinbono.Text = ""
        lblnominales.Text = ""
        lblemisor.Text = ""
        lblparticipacion.Text = ""
        lblbasecupon.Text = ""
        lblFecUltCupon.Text = ""
        lblpreciovector.Text = ""
        lblbasetir.Text = ""
        lblFecProxCupon.Text = ""
        lblUnidades.Text = ""
        lblduracion.Text = ""
        lblMoneda.Text = ""
        ddlFondo.SelectedIndex = 0
        ddlOperacion.SelectedIndex = 0
        txtISIN.Text = ""
        txtSBS.Text = ""
        txtMnemonico.Text = ""
    End Sub
    Private Sub LimpiarDatosOperacion()
        tbFechaOperacion.Text = ""
        tbFechaLiquidacion.Text = ""
        txtMnomOp.Text = ""
        txtMnomOrd.Text = ""
        ddlTipoTasa.SelectedIndex = 0
        txtYTM.Text = ""
        lblPrecioCal.Text = ""
        txtPrecioNegoc.Text = ""
        lblInteresCorrido.Text = ""
        txtInteresCorNeg.Text = ""
        ddlGrupoInt.SelectedIndex = 0
        If ddlIntermediario.Items.Count > 0 Then ddlIntermediario.SelectedIndex = 0
        CargarContactos()
        ddlContacto.SelectedIndex = 0
        txtMontoOperacional.Text = ""
        txtPrecioNegSucio.Text = ""
        txtHoraOperacion.Text = ""
        txtNroPapeles.Text = ""
        txtObservacion.Text = ""
        txttotalComisionesC.Text = ""
        txtMontoNetoOpe.Text = ""
        txtHoraOperacion.Text = ""
        dgLista.Dispose()
        dgLista.DataBind()
    End Sub
#End Region
    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(txtCodigoOrden.Value, ddlFondo.SelectedValue, "PAPELES COMERCIALES", ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"),
        txtISIN.Text.Trim, txtSBS.Text.Trim, txtMnemonico.Text, ddlFondo.SelectedItem.Text)
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
        Dim montoNominal As Decimal = Math.Round(Convert.ToDecimal(txtMnomOp.Text.Replace(".", UIUtility.DecimalSeparator)), 2)
        Dim YTM As String = Math.Round(Convert.ToDecimal(txtYTM.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        If Not dtCuponeraOI Is Nothing Then
            If dtCuponeraOI.Rows.Count > 0 Then
                oCuponera.EliminarCuponeraOI(GUID, IsTemporal, txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest)
                For i = 0 To dtCuponeraOI.Rows.Count - 1
                    If i < dtCuponeraOI.Rows.Count - 1 Then
                        oCuponera.InsertarCuponeraOI(0, GUID, IsTemporal, txtCodigoOrden.Value, ddlFondo.SelectedValue.ToString,
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaInicio")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaTermino")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("Amortizacion")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("ValorNominal")),
                        String.Empty, String.Empty, "N", dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TasaCupon")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TotalVP")), montoNominal, ddlTipoTasa.SelectedValue,
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("DiferenciaDias")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("DiasPago")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("Base")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("codigoTipoAmortizacion")),
                        txtMnemonico.Text.ToString(), DatosRequest)
                    Else
                        oCuponera.InsertarCuponeraOI(1, GUID, IsTemporal, txtCodigoOrden.Value, ddlFondo.SelectedValue.ToString,
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaInicio")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaTermino")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("Amortizacion")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("ValorNominal")),
                        String.Empty, String.Empty, "N", dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TasaCupon")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TotalVP")), montoNominal, ddlTipoTasa.SelectedValue,
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
        If Not Session("Mercado") Is Nothing And Not Session("TipoRenta") Is Nothing Then
            OrdenInversion.ObtieneImpuestosComisiones(dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
        End If
    End Sub
    Private Sub ddlGrupoInt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrupoInt.SelectedIndexChanged
        UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        CargarContactos()
    End Sub
    Protected Sub ddlPlaza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPlaza.SelectedIndexChanged
        Session("Mercado") = ddlPlaza.SelectedValue
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        ObtieneImpuestosComisiones()
    End Sub
    Private Sub CargarPlaza()
        Dim oPlazaBM As New PlazaBM
        ddlPlaza.DataSource = oPlazaBM.Listar(Nothing)
        ddlPlaza.DataTextField = "Descripcion"
        ddlPlaza.DataValueField = "CodigoPlaza"
        ddlPlaza.DataBind()
        ddlPlaza.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub

    Private Sub CargarCombos()
        CargarComboCategoriaContable()
        CargarComboOperacionOIParaTraspaso()
        CargarPlaza()
        UIUtility.CargarTipoCuponOI(ddlTipoTasa)
        UIUtility.CargarGrupoIntermediarioOI(ddlGrupoInt)
        HelpCombo.PortafolioCodigoListar(ddlFondo, PORTAFOLIO_MULTIFONDOS)
    End Sub

    Private Sub CargarComboCategoriaContable()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dtParams As DataTable = oParametrosGenerales.Listar(ParametrosSIT.TIPO_VALORIZACION, DatosRequest)
        HelpCombo.LlenarComboBox(ddlCategoriaContable, dtParams, "Valor", "Nombre", True)
    End Sub

    Private Sub CargarComboOperacionOIParaTraspaso()
        If (Me.hdPagina.Value = "TI") Then
            UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
        Else
            UIUtility.CargarOperacionOI(ddlOperacion, "OperacionOI")
        End If
    End Sub

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
        oRow.IndPrecioTasa = "T" 'ddlModoNegociacion.SelectedValue 'T: Tasa YTM % , P: Precio
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

        oRow.CodigoPlaza = "7" 'Por defecto 7:'LIMA' ---- ddlPlaza.SelectedValue
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

        ' INICIO | Proyecto SIT Fondos - Mandato | Ian Pastor M. | 2018-09-21 | Categoría Contable
        oRow.TipoValorizacion = ddlCategoriaContable.SelectedValue
        ' FIN | Proyecto SIT Fondos - Mandato | Ian Pastor M. | 2018-09-21 | Categoría Contable

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
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports ParametrosSIT
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmAcciones
    Inherits BasePage
#Region "Variables"
    Dim oPortafolioBM As New PortafolioBM
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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        If Not Session("SS_DatosModal") Is Nothing And hdPopUp.Value = "V" Then
            txtISIN.Text = CType(Session("SS_DatosModal"), String())(0)
            txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1)
            Session("Nemonico") = txtMnemonico.Text
            txtSBS.Text = CType(Session("SS_DatosModal"), String())(2)
            hdCustodio.Value = CType(Session("SS_DatosModal"), String())(3)
            hdSaldo.Value = CType(Session("SS_DatosModal"), String())(4)
            Session.Remove("SS_DatosModal")
        End If
        If Not Session("SS_DatosModal") Is Nothing And hdPopUp.Value = "IR" Then
            txtISIN.Text = CType(Session("SS_DatosModal"), String())(0)
            txtSBS.Text = CType(Session("SS_DatosModal"), String())(1)
            txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(2)
            Session("Nemonico") = txtMnemonico.Text
            ddlFondo.SelectedValue = CType(Session("SS_DatosModal"), String())(3)
            ddlOperacion.SelectedValue = CType(Session("SS_DatosModal"), String())(4)
            lblMoneda.Text = CType(Session("SS_DatosModal"), String())(5)
            txtCodigoOrden.Value = CType(Session("SS_DatosModal"), String())(6)
            Session("CodOrden") = txtCodigoOrden.Value
            Session.Remove("SS_DatosModal")
        End If
        If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
            EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
        End If
        hdSaldo.Value = 0
        'btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
        If Not Page.IsPostBack Then
            Try
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                hdRptaConfirmar.Value = "NO"
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
                btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
                LimpiarSesiones()
                Dim oParametrosGenerales As New ParametrosGeneralesBM
                Dim dtMotivos As DataTable = oParametrosGenerales.Listar("MOTCAM", DatosRequest)
                HelpCombo.LlenarComboBox(ddlMotivoCambio, dtMotivos, "Valor", "Nombre", True)
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    hdPagina.Value = Request.QueryString("PTNeg")
                End If
                If (hdPagina.Value = "TI") Then
                    UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
                Else
                    UIUtility.CargarOperacionOI(ddlOperacion, "OperacionOI")
                End If
                UIUtility.CargarGrupoIntermediarioOI(ddlGrupoInt)
                CargarPaginaInicio()
                hdPagina.Value = ""
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    ddlFondo.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
                    ddlFondo.DataValueField = "CodigoPortafolio"
                    ddlFondo.DataTextField = "Descripcion"
                    ddlFondo.DataBind()
                    ddlFondo.Items.Insert(0, "--SELECCIONE--")
                    hdPagina.Value = Request.QueryString("PTNeg")
                    If hdPagina.Value = "TI" Then
                        txtMnemonico.Text = Request.QueryString("PTCMnemo")
                        ddlFondo.SelectedValue = Request.QueryString("PTFondoO")
                        ddlFondoDestino.SelectedValue = Request.QueryString("PTFondoD")
                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                        txtISIN.Text = Request.QueryString("PTISIN")
                        txtSBS.Text = Request.QueryString("PTSBS")
                        lblMoneda.Text = Request.QueryString("PTMon")
                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                        hdCustodio.Value = Request.QueryString("PTCustodio")
                        hdSaldo.Value = Request.QueryString("PTSaldo")
                        CargarCaracteristicasValor()
                        OrdenInversion.ObtieneImpuestosComisiones(dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
                        ControlarCamposTI()
                        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(Request.QueryString("fechaOperacion")))
                    Else
                        txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                        Session("CodOrden") = txtCodigoOrden.Value
                        ddlFondo.SelectedValue = Request.QueryString("PTFondo")

                        'Validamos si la Orden de Inversion, es Exterior
                        'ValidaOrigen()

                        If (hdPagina.Value = "EO") Or (hdPagina.Value = "CO") Or (hdPagina.Value = "XO") Then 'Viene de la Pagina Confirmacion , Ejecucion  o Extorno de Ordenes de Inversion
                            CargarDatosOrdenInversion(txtCodigoOrden.Value)
                            CargarCaracteristicasValor()
                            tbNPoliza.Text = Right(txtCodigoOrden.Value, 4)
                            tbNPoliza.ReadOnly = True
                            Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                            If (hdPagina.Value <> "XO") Then
                                lNPoliza.Visible = True
                                tbNPoliza.Visible = True
                            End If
                            ControlarCamposEO_CO_XO()
                            CargarPaginaModificarEO_CO_XO(hdPagina.Value)
                            UIUtility.ObtieneImpuestosComisionesGuardado(dgLista, txtCodigoOrden.Value, ddlFondo.SelectedValue) 'Modificado por LC 20081023
                            If dgLista.Rows.Count = 0 Then
                                OrdenInversion.ObtieneImpuestosComisiones(dgLista, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlIntermediario.SelectedValue)
                            End If
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                            If hdPagina.Value = "CO" Then
                                btnAceptar.Text = "Grabar y Confirmar"
                                If Session("ValidarFecha").ToString = "FECHADIFERENTE" Then
                                    CargarPaginaInicio()
                                    btnCaracteristicas.Visible = True
                                End If
                            End If
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                        Else
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
                                        Session("CodOrden") = txtCodigoOrden.Value
                                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                        Call CargarDatosOrdenInversion(txtCodigoOrden.Value)
                                        Call CargarCaracteristicasValor()
                                        UIUtility.ObtieneImpuestosComisionesGuardado(dgLista, txtCodigoOrden.Value, ddlFondo.SelectedValue)
                                        Call HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, True, False, False)
                                    Else
                                        If (hdPagina.Value = "CONSULTA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                            ConfiguraModoConsulta()
                                            ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                            txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                            Session("CodOrden") = txtCodigoOrden.Value
                                            tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                            CargarDatosOrdenInversion(txtCodigoOrden.Value)
                                            CargarCaracteristicasValor()
                                            UIUtility.ObtieneImpuestosComisionesGuardado(dgLista, txtCodigoOrden.Value, ddlFondo.SelectedValue)
                                            HabilitaBotones(False, False, False, False, False, False, False, False, False, False, True, False, False, False)
                                            HabilitaDeshabilitaCabecera(False)
                                        Else
                                            If (hdPagina.Value = "MODIFICA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                                ConfiguraModoConsulta()
                                                ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                                txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                                Session("CodOrden") = txtCodigoOrden.Value
                                                tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                                CargarDatosOrdenInversion(txtCodigoOrden.Value)
                                                CargarCaracteristicasValor()
                                                UIUtility.ObtieneImpuestosComisionesGuardado(dgLista, txtCodigoOrden.Value, ddlFondo.SelectedValue)
                                                HabilitaBotones(False, False, False, False, False, False, True, False, True, False, True, False, False, False)
                                                HabilitaDeshabilitaCabecera(False)
                                                HabilitaDeshabilitaDatosOperacionComision(True)
                                                Session("EstadoPantalla") = "Modificar"
                                                lblAccion.Text = "Modificar"
                                                hdMensaje.Value = "la Modificación"
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                    btnSalir.Attributes.Remove("onClick")
                    '  btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF49", txtCodigoOrden.Value + "?", "SI")
                Else
                    ddlFondo.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
                    ddlFondo.DataValueField = "CodigoPortafolio"
                    ddlFondo.DataTextField = "Descripcion"
                    ddlFondo.DataBind()
                    ddlFondo.Items.Insert(0, "--SELECCIONE--")
                End If
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        End If
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Call HabilitaBotones(True, True, True, True, True, True, True, True, True, True, True, False, True, False)
    End Sub
    Private Sub HabilitaBotones(ByVal bLimites As Boolean, ByVal bIngresar As Boolean, _
                                ByVal bModificar As Boolean, ByVal bEliminar As Boolean, ByVal bConsultar As Boolean, _
                                ByVal bAsignar As Boolean, ByVal bProcesar As Boolean, ByVal bImprimir As Boolean, _
                                ByVal bAceptar As Boolean, ByVal bBuscar As Boolean, ByVal bSalir As Boolean, _
                                ByVal bRetornar As Boolean, ByVal bCaracteristicas As Boolean, ByVal bLimitesParametrizados As Boolean)
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
    Public Function ObtenerDatosOperacion() As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = tbFechaOperacion.Text
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = tbFechaLiquidacion.Text
        drGrilla("v3") = "Hora Operación"
        drGrilla("c3") = tbHoraOperacion.Text
        drGrilla("c4") = "Numero Acciones Ordenadas"
        drGrilla("v4") = txtNroAccOrde.Text.ToUpper
        drGrilla("c5") = "Numero Acciones Operación"
        drGrilla("v5") = txtNroAccOper.Text
        drGrilla("c6") = "Precio"
        drGrilla("v6") = txtPrecio.Text
        drGrilla("c7") = "Monto Operacion"
        drGrilla("v7") = txtMontoNominal.Text
        drGrilla("c8") = "Intermediario"
        drGrilla("v8") = ddlIntermediario.SelectedItem.Text
        If ddlContacto.SelectedIndex <> 0 Then
            drGrilla("c9") = "Contacto"
            drGrilla("v9") = ddlContacto.SelectedItem.Text
        Else
            drGrilla("c9") = ""
            drGrilla("v9") = ""
        End If
        drGrilla("c10") = "Observación"
        drGrilla("v10") = txtObservacion.Text
        If tbNPoliza.Visible = True Then
            drGrilla("c11") = "Poliza"
            drGrilla("v11") = tbNPoliza.Text
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
        drGrilla("v19") = txttotalComisionesC.Text
        drGrilla("c20") = "Precio Promedio"
        drGrilla("v20") = txtPrecPromedio.Text
        drGrilla("c21") = "Monto Neto Operacion"
        drGrilla("v21") = txtMontoNetoOpe.Text
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If Not ddlFondo.SelectedValue = "--SELECCIONE--" Then
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
                    ShowDialogPopupValores(txtISIN.Text.Trim.ToUpper, txtSBS.Text.Trim.ToUpper, txtMnemonico.Text.Trim.ToUpper, ddlOperacion.SelectedValue, "AC")
                    Session("Busqueda") = 2
                Else
                    If Session("Busqueda") = 1 Then
                        If CargarCaracteristicasValor() Then
                            CargarFechaVencimiento()
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
                            CargarPaginaIngresar()

                            CargarIntermediario()
                            CargarPlaza()
                            CargarTipoTramo()
                            CargarMedioTrans()
                            If UIUtility.ObtenerCodigoTipoOperacion("COMPRA") = ddlOperacion.SelectedValue Then
                                ddlFondo.Enabled = True
                            End If
                            UIUtility.ResaltaCombo(ddlIntermediario, True)
                            UIUtility.ResaltaCajaTexto(txtNroAccOrde, True)
                            UIUtility.ResaltaCajaTexto(txtPrecio, True)
                            UIUtility.ResaltaCombo(ddlPlaza, True)
                            UIUtility.ResaltaCombo(ddlGrupoInt, True)
                            UIUtility.ResaltaCajaTexto(tbFechaLiquidacion, True)
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
                        Dim strAux As String = String.Empty
                        If (hdPagina.Value = "DA") Then
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
                        ShowDialogPopupInversionesRealizadas(GetISIN, GetSBS, GetMNEMONICO, ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, GetOPERACION, GetMONEDA, strAux, strAccion)
                        Session("Busqueda") = 2
                    Else
                        If Session("Busqueda") = 1 Then
                            CargarCaracteristicasValor()
                            txtCodigoOrden.Value = Session("CodOrden")
                            CargarDatosOrdenInversion(txtCodigoOrden.Value)
                            UIUtility.ObtieneImpuestosComisionesGuardado(dgLista, txtCodigoOrden.Value, ddlFondo.SelectedValue)
                            Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                            btnAceptar.Enabled = True
                            lblSaldoValor.Text = UIUtility.CargarSaldoXTercero(lblSaldoValor.Text, ddlOperacion.SelectedValue, ddlIntermediario.SelectedValue, ddlFondo.SelectedValue, txtMnemonico.Text, tbFechaOperacion.Text, DatosRequest)
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
        Else
            AlertaJS("Debe seleccionar un portafolio antes de realizar la busqueda.")
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
    Public Sub CargarTipoTramo()
        Dim obm As New ParametrosGeneralesBM
        ddlTipoTramo.DataSource = obm.Listar("TIPOTRAMO", DatosRequest)
        ddlTipoTramo.DataValueField = "Valor"
        ddlTipoTramo.DataTextField = "Nombre"
        ddlTipoTramo.DataBind()
    End Sub
    Public Sub CargarMedioTrans()
        Dim obm As New ParametrosGeneralesBM
        Dim dtMedioTrans As New DataTable

        dtMedioTrans = obm.ListarMedioNegociacionPrevOI(ParametrosSIT.TR_RENTA_VARIABLE).Tables(0)
        HelpCombo.LlenarComboBox(ddlMedioTrans, dtMedioTrans, "Valor", "Nombre", False)
        ddlMedioTrans.Items.Insert(0, New ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
        ddlMedioTrans.SelectedValue = ParametrosSIT.MEDIO_TRANS_TELF
    End Sub
    Public Sub CargarIntermediario()
        UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
    End Sub
    Public Sub CargarDatosOrdenInversion(ByVal CodigoOrden As String)
        Try
            oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(CodigoOrden, ddlFondo.SelectedValue, DatosRequest, PORTAFOLIO_MULTIFONDOS)
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
            Session("Nemonico") = txtMnemonico.Text
            txtCodigoOrden.Value = oRow.CodigoOrden.ToString()
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
            txtNroAccOrde.Text = Format(oRow.CantidadOrdenado, "##,##0.0000000")
            txtNroAccOper.Text = Format(oRow.CantidadOperacion, "##,##0.0000000")
            txtMontoNominal.Text = Format(oRow.MontoOperacion, "##,##0.0000000")
            txtPrecio.Text = Format(oRow.Precio, "##,##0.0000000")
            txttotalComisionesC.Text = Format(oRow.TotalComisiones, "##,##0.0000000")
            txtPrecPromedio.Text = Format(oRow.PrecioPromedio, "##,##0.0000000")
            txtMontoNetoOpe.Text = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
            txtObservacion.Text = oRow.Observacion
            tbHoraOperacion.Text = oRow.HoraOperacion
            hdNumUnidades.Value = txtNroAccOper.Text
            If (hdPagina.Value = "CO") And oRow.TipoCambio > 0 Then
                tblDestino.Attributes.Add("Style", "Visibility:visible")
                txtMontoOperacionDestino.Text = Format(oRow.MontoDestino, "##,##0.0000000")
                txtComisionesDestino.Text = Format((oRow.TipoCambio) * (oRow.TotalComisiones), "##,##0.0000000")
                lblMDest.InnerText = Trim(oRow.CodigoMonedaDestino)
                lblMDest2.InnerText = Trim(oRow.CodigoMonedaDestino)
            End If
            CargarPlaza()
            ddlPlaza.SelectedValue = oRow.Plaza
            CargarMedioTrans()
            If oRow.MedioNegociacion <> "" Then
                ddlMedioTrans.SelectedValue = oRow.MedioNegociacion
            Else
                ddlMedioTrans.SelectedIndex = 0
            End If
            tbHoraEjecucion.Text = oRow.HoraEjecucion
            CargarTipoTramo()
            If oRow.TipoTramo <> "" Then
                ddlTipoTramo.SelectedValue = oRow.TipoTramo
            End If
            ViewState("MontoNeto") = Convert.ToDecimal(txtMontoNominal.Text)
            tbNPoliza.Text = oRow.NumeroPoliza.ToString()
            ddlGrupoInt.SelectedValue = oRow.GrupoIntermediario
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
            If oRow.CodigoMotivoCambio <> "" Then
                ddlMotivoCambio.SelectedValue = oRow.CodigoMotivoCambio
            End If
            If oRow.Ficticia = "S" Then
                chkFicticia.Checked = True
            Else
                chkFicticia.Checked = False
            End If
            If oRow.EventoFuturo = 1 Then
                chkEmisionPrimaria.Checked = True
            Else
                chkEmisionPrimaria.Checked = False
            End If
        Catch ex As Exception
            If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                AlertaJS(ObtenerMensaje("CONF31"))
            Else
                AlertaJS(ObtenerMensaje("CONF32"))
            End If
        End Try
    End Sub
    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Dim feriado As New FeriadoBM
        Try
            If CDec(txtNroAccOper.Text) <= 0 Then
                AlertaJS("Ingrese una cantidad de acciones mayor a 0")
                Exit Sub
            End If
            If hdPagina.Value = "DA" Then
                hdPagina.Value = "DA"
            End If
            If ValidarFechas() = True Then
                If UIUtility.ValidarHora(tbHoraOperacion.Text) = False Then
                    AlertaJS(ObtenerMensaje("CONF22"))
                    Exit Sub
                End If
                If (feriado.VerificaDia(UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text), Session("Mercado")) = False) Then
                    AlertaJS("Fecha de Vencimiento no es valida.")
                    Exit Sub
                End If
                txtMontoNominal.Text = Format((Convert.ToDecimal(txtNroAccOper.Text) * Convert.ToDecimal(txtPrecio.Text)), "##,##0.00")
                CalcularComisiones()
                Session("Procesar") = 1
                ViewState("estadoOI") = ""
                ViewState("GUID_Limites") = System.Guid.NewGuid.ToString()
                'LIMITES
                Dim bolValidaLimites As Boolean = True
                If chkEmisionPrimaria.Checked = True Then
                    bolValidaLimites = False
                End If
                'If bolValidaLimites = True Then OrdenInversion.CalculaLimitesOnLine(Me, DatosRequest, ViewState("estadoOI"), ViewState("GUID_Limites").ToString())
                If Session("TipoRenta") = 2 Then RestriccionExcesosBroker()
                If bolValidaLimites = True Then
                    Dim ds As DataSet
                    Dim Mensaje As String
                    ds = New OrdenPreOrdenInversionBM().ValidacionPuntual_LimitesTrading(txtMnemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), ddlFondo.SelectedValue, Convert.ToDecimal(txtNroAccOper.Text.Replace(",", "").Replace(".", UIUtility.DecimalSeparator)), Session("CodigoMoneda"), Usuario.ToString.Trim, String.Empty)
                    If ds.Tables(0).Rows.Count > 0 Then
                        Mensaje = "El usuario no esta permitido grabar la operación, el monto de negociado excede el límite de autonomía por Trader:\n\n"
                        For Each fila As DataRow In ds.Tables(0).Rows
                            Mensaje = Mensaje & "- Usuario (" & fila("TipoCargoExc") & ") excedió límite de autonomía \""" & fila("GrupoLimTrd") & "\"", debe ser autorizado por un usuario " & fila("TipoCargoAut") & " (" & fila("TraderAut") & "). \n\n"
                        Next
                        Mensaje = Mensaje & "La operación debe ser grabada por el usuario autorizado haciendo clic en el botón Aceptar de la orden de inversión."
                        AlertaJS(Mensaje)
                        Session("dtValTrading") = ds.Tables(0)
                    End If
                End If
                If ObtieneCustodiosSaldos() = False Then
                    AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                    Exit Sub
                End If
                CargarPaginaProcesar()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub RestriccionExcesosBroker()
        Dim oFormulasOI As New OrdenInversionFormulasBM
        Dim indExceso As String
        If hdPagina.Value <> "TI" Then
            indExceso = oFormulasOI.RestriccionExcesosBroker(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), ddlIntermediario.SelectedValue.ToString(), Convert.ToDecimal(txtMontoNominal.Text), txtMnemonico.Text)
            If indExceso = "E" Then
                If ViewState("estadoOI") = "E-EXC" Then
                    ViewState("estadoOI") = "E-EBL"
                    Page.RegisterStartupScript("ExcesoBroker", "<script language=""JavaScript"">alert(""Se ha producido Exceso de Límites y Exceso por Broker.\n\nEl monto total de negociaciones realizadas por el Broker en los tres fondos excede el monto maximo a negociar."")</script>")
                Else
                    ViewState("estadoOI") = "E-ENV"
                    Page.RegisterStartupScript("ExcesoBroker", "<script language=""JavaScript"">alert(""El monto total de negociaciones realizadas por el Broker en los tres fondos excede el monto maximo a negociar."")</script>")
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
                If hdPagina.Value = "TI" Then
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
        Dim decMontoAux As Decimal = 0.0,
             cantNegociada As Decimal = Convert.ToDecimal(Me.txtNroAccOper.Text.Replace(".", UIUtility.DecimalSeparator))
        Dim cantCustodios As Integer = 0
        Dim oPrevOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim dtSumaUnidades As DataTable
        Try
            If hdNumUnidades.Value = txtNroAccOper.Text Then
                Return True
            End If
            '********COMPRA*********
            If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                Session("ValorCustodio") = hdCustodio.Value + strSeparador + txtNroAccOper.Text
                hdNumUnidades.Value = txtNroAccOper.Text
                Return True
            End If
            '********VENTA*********
            If Session("ValorCustodio") Is Nothing Then
                Session("ValorCustodio") = hdCustodio.Value + strSeparador + hdSaldo.Value
            ElseIf Session("ValorCustodio") = "" Then
                Session("ValorCustodio") = hdCustodio.Value + strSeparador + hdSaldo.Value
            End If

            dtSumaUnidades = oPrevOrdenInversionBM.ObtenerUnidadesNegociadasDiaT(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text).Tables(0)
            If dtSumaUnidades.Rows.Count > 0 Then
                cantNegociada += Decimal.Parse(dtSumaUnidades.Compute("Sum(UNIDADES)", String.Empty))
                If Me.hdPagina.Value = "CO" Then
                    cantNegociada -= CDec(hdNumUnidades.Value)
                End If
            End If

            decMontoAux = Convert.ToDecimal(lblSaldoValor.Text)
            If decMontoAux = cantNegociada Then
                hdNumUnidades.Value = txtNroAccOper.Text
                Return True
            ElseIf decMontoAux > cantNegociada Then
                If cantCustodios = 1 Then
                    Session("ValorCustodio") = hdCustodio.Value + strSeparador + txtNroAccOper.Text
                    hdNumUnidades.Value = txtNroAccOper.Text
                    Return True
                Else
                    Session("ValorCustodio") = UIUtility.AjustarMontosCustodios(CType(Session("ValorCustodio"), String), txtNroAccOper.Text)
                    hdNumUnidades.Value = txtNroAccOper.Text
                    Return True
                End If
                Return False
            ElseIf decMontoAux < cantNegociada Then
                Session("ValorCustodio") = hdCustodio.Value + strSeparador + hdSaldo.Value
                Return False
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
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
        dsFechas = objPortafolio.Seleccionar(ddlFondo.SelectedValue, DatosRequest)
        If dsFechas.Tables(0).Rows.Count > 0 Then
            drFechas = dsFechas.Tables(0).NewRow
            drFechas = dsFechas.Tables(0).Rows(0)
            Dim dblFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
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
    Private Sub btnVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Session("prevPag") = "../InstrumentosNegociados/frmAcciones.aspx"
        Response.Redirect("../Reportes/frmVisorOrdenesDeInversion.aspx?titulo=Orden de Inversión - ACCIONES")
    End Sub
    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        LimpiarSesiones()
        txtMontoNominal.Text = "0.0000000"
        txtPrecio.Text = "0.0000000"
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
        tbHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Ingresar"
        hdMensaje.Value = "el Ingreso"
        hdNumUnidades.Value = 0
        If Not ddlFondo.Items.FindByValue("MULTIFONDO") Is Nothing Then 'RGF 20081001
            ddlFondo.SelectedValue = "MULTIFONDO"
        End If
        lblTitulo.Text = "PreOrden de Inversión - ACCIONES"

        HabilitarBotonGuardarPreOrden()
    End Sub
    Private Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos(True)
        CargarFechaVencimiento()
        lblSaldoValor.Text = UIUtility.CargarSaldoXTercero(lblSaldoValor.Text, ddlOperacion.SelectedValue, ddlIntermediario.SelectedValue, ddlFondo.SelectedValue, txtMnemonico.Text, tbFechaOperacion.Text, DatosRequest) 'DB 20090428
        SaldoUnidades_EnModificacion(lblSaldoValor, CDec(hdNumUnidades.Value), ddlOperacion.SelectedValue)
        dgLista.Dispose()
        dgLista.DataBind()
        Session("Mercado") = ddlPlaza.SelectedValue
        OrdenInversion.ObtieneImpuestosComisiones(dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
    End Sub
    Private Sub CargarContactos(Optional ByVal cambiaIntermediario As Boolean = False)
        If ddlContacto.SelectedIndex > 0 And Session("EstadoPantalla") <> "Ingresar" And Not cambiaIntermediario Then
            Exit Sub
        End If
        Dim objContacto As New ContactoBM
        Dim dtContacto As DataSet
        dtContacto = objContacto.ListarContactoPorTerceros(ddlIntermediario.SelectedValue)
        ddlContacto.Items.Clear()
        If dtContacto.Tables(0).Rows.Count > 0 Then
            ddlContacto.DataSource = dtContacto
            ddlContacto.DataTextField = "DescripcionContacto"
            ddlContacto.DataValueField = "CodigoContacto"
            ddlContacto.SelectedValue = dtContacto.Tables(0).Rows(0)(0).ToString 'HDG 20120918
            ddlContacto.DataBind()
            ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
            If txtMnemonico.Text <> "" And ddlIntermediario.SelectedValue <> "" Then
                ddlContacto.SelectedValue = objContacto.SeleccionarUltimoContactoEnUnaNegociacion(txtMnemonico.Text, ddlIntermediario.SelectedValue)
            End If
        Else
            ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        End If
        Dim i As Integer
        Dim dtAux As DataTable
        dtAux = CType(Session("datosEntidad"), DataTable)
        If Not dtAux Is Nothing Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("CodigoTercero") = ddlIntermediario.SelectedValue Then
                    If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                        hdCustodio.Value = dtAux.Rows(i)("codigoCustodio")
                    End If
                    Exit For
                End If
            Next
        End If
        If Not Session("Mercado") Is Nothing Then
            If Session("Mercado") = "1" Then
                ddlTipoTramo.SelectedIndex = 0
                ddlTipoTramo.Enabled = False
            ElseIf Session("Mercado") = "2" Then
                ddlTipoTramo.Enabled = True
            End If
        End If
    End Sub
    Public Sub CalcularComisiones()
        Dim dblTotalComisiones As Decimal = 0.0
        If chkRecalcular.Checked = True Then
            If hdPagina.Value = "TI" Then
                dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), txtMontoNominal.Text.Replace(",", ""), txtNroAccOper.Text.Replace(",", ""), ddlGrupoInt.SelectedValue, ddlOperacion.SelectedValue, CLASE_INSTRUMENTO_ACCION)  'HDG 20120224
            Else
                '       dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), txtMontoNominal.Text.Replace(",", ""), txtNroAccOper.Text.Replace(",", ""), ddlGrupoInt.SelectedValue, ddlOperacion.SelectedValue, CLASE_INSTRUMENTO_ACCION)  'HDG 20120224
                dblTotalComisiones = UIUtility.CalcularComisionesYLlenarGridView(dgLista, String.Empty, txtMontoNominal.Text.Replace(",", ""), UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text, ddlIntermediario.SelectedValue)

            End If
            If Session("Mercado") = 2 And ddlGrupoInt.SelectedValue = "BRO" Then
                dblTotalComisiones = CalculaImpuestosComisionesIntermediarioExtranjero()
            End If
        Else
            dblTotalComisiones = UIUtility.CalculaImpuestosComisionesNoRecalculo(dgLista, Session("Mercado"), txtMontoNominal.Text.Replace(",", ""), txtNroAccOper.Text.Replace(",", ""), ddlGrupoInt.SelectedValue, ddlOperacion.SelectedValue, CLASE_INSTRUMENTO_ACCION)  'HDG 20120224
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
    Private Function CalculaImpuestosComisionesIntermediarioExtranjero() As Decimal
        Dim dbltotalcomisiones As Decimal = 0.0
        If (Session("Mercado") = "2") Then 'EXTRANJERO
            If (ddlTipoTramo.SelectedValue = "PRINCIPAL") Then
                Dim index As Integer
                For index = 0 To dgLista.Rows.Count - 1
                    Dim txtValorComision As TextBox = CType(dgLista.Rows(index).FindControl("txtValorComision1"), TextBox)
                    Dim txtValorComision2 As TextBox = CType(dgLista.Rows(index).FindControl("txtValorComision2"), TextBox)
                    dgLista.Rows(index).Cells(2).Text = "(0)"
                    txtValorComision.Text = "0"
                    dbltotalcomisiones = dbltotalcomisiones + CDec(txtValorComision2.Text)
                Next
            ElseIf ddlTipoTramo.SelectedValue = "AGENCIA" Then
                Dim oBrokerBM As New EntidadBM
                Dim dt As DataTable
                dt = oBrokerBM.SeleccionarCostoBroker(txtNroAccOper.Text, ViewState("CodigoEntidad"), ddlTipoTramo.SelectedValue)
                If dt.Rows.Count > 0 Then
                    Dim index As Integer
                    For index = 0 To dgLista.Rows.Count - 1
                        Dim txtValorComision As TextBox = CType(dgLista.Rows(index).FindControl("txtValorComision1"), TextBox)
                        Dim txtValorComision2 As TextBox = CType(dgLista.Rows(index).FindControl("txtValorComision2"), TextBox)
                        If (dt.Rows(0)("TipoCosto") = "V") Then 'VALOR
                            dgLista.Rows(index).Cells(2).Text = String.Format("( {0} )", dt.Rows(0)("Costo"))
                            txtValorComision.Text = txtNroAccOper.Text * dt.Rows(0)("Costo")
                        ElseIf dt.Rows(0)("TipoCosto") = "P" Then 'PORCENTAJE
                            dgLista.Rows(index).Cells(2).Text = String.Format("( {0}% )", dt.Rows(0)("Costo"))
                            txtValorComision.Text = CDec(txtMontoNominal.Text) * (dt.Rows(0)("Costo") / 100)
                        End If
                        dbltotalcomisiones = dbltotalcomisiones + CDec(txtValorComision.Text)
                        dbltotalcomisiones = dbltotalcomisiones + CDec(txtValorComision2.Text)
                    Next
                End If
            End If
        End If
        Return dbltotalcomisiones
    End Function
    Private Sub actualizaMontos()
        Dim dblTotalComisiones As Decimal = 0.0
        dblTotalComisiones = UIUtility.ActualizaMontosFinales(dgLista)
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - De acuerdo al tipo de operación se restará o sumará al valor nominal | 27/06/18 
        If (ddlOperacion.SelectedValue = "2") Then
            txtMontoNetoOpe.Text = Format(txtMontoNominal.Text.Replace(",", "") - dblTotalComisiones, "##,##0.0000000")
        Else
            txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoNominal.Text.Replace(",", ""), "##,##0.0000000")
        End If
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - De acuerdo al tipo de operación se restará o sumará al valor nominal | 27/06/18 
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

        Try
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
            If hdRptaConfirmar.Value.ToUpper = "SI" Then
                EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
                If CDec(txtNroAccOper.Text) <= 0 Then
                    AlertaJS("Ingrese una cantidad de acciones mayor a 0")
                    Exit Sub
                End If
                If ObtieneCustodiosSaldos() = False Then
                    AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                    Exit Sub
                End If
                If txtMnemonico.Text.Trim = "BAP" Or txtMnemonico.Text.Trim = "SCCO" Or txtMnemonico.Text.Trim = "BVN" Or txtMnemonico.Text.Trim = "MPLE LN" Then
                    Dim oTercerosBE As New TercerosBE
                    oTercerosBE = objtercero.Seleccionar(ddlIntermediario.SelectedValue, DatosRequest)
                    If oTercerosBE.Tables(0).Rows.Count > 0 Then
                        If CType(oTercerosBE.Tables(0).Rows(0)("CodigoPais"), String) = "604" Then
                            Session("Mercado") = "1"
                        Else
                            Session("Mercado") = "2"
                        End If
                    End If
                End If
                If hdPagina.Value <> "" And hdPagina.Value <> "DA" And hdPagina.Value <> "TI" And hdPagina.Value <> "MODIFICA" Then
                    If hdPagina.Value = "EO" Or hdPagina.Value = "CO" Then
                        ModificarOrdenInversion()
                        UIUtility.InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                        CargarPaginaAceptar()
                    End If
                    If hdPagina.Value = "XO" Then
                        oOrdenInversionWorkFlowBM.ExtornarOIEjecutadas(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest)
                        ReturnArgumentShowDialogPopup()
                        Session("Extornado") = 1
                    Else
                        If hdPagina.Value = "EO" Then
                            oOrdenInversionWorkFlowBM.EjecutarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, tbNPoliza.Text.Trim, DatosRequest)
                            ReturnArgumentShowDialogPopup()
                            Session("Ejecutado") = 1
                        Else
                            If hdPagina.Value = "CO" Then
                                oOrdenInversionWorkFlowBM.ConfirmarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, tbNPoliza.Text.Trim, DatosRequest)
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
                        If strAlerta.Length > 0 Then
                            AlertaJS(strAlerta)
                            Exit Sub
                        End If
                    End If
                    actualizaMontos()
                    If ObtieneCustodiosSaldos() = False Then
                        AlertaJS("El saldo ingresado no coincide o sobrepasa el saldo actual.")
                        Exit Sub
                    End If
                    If Session("EstadoPantalla") = "Ingresar" Then
                        If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) <= UIUtility.ConvertirFechaaDecimal(objutil.RetornarFechaNegocio) _
                            And chkEmisionPrimaria.Checked = True Then
                            AlertaJS("La fecha de operacion debe ser mayor que la fecha de apertura IDI")
                            Exit Sub
                        End If

                        If Session("Procesar") = 1 Then
                            'Dim strcodigoOrden As String
                            'strcodigoOrden = InsertarOrdenInversion()
                            'If strcodigoOrden <> "" Then
                            '    If ddlFondo.SelectedValue <> "MULTIFONDO" Then
                            '        Dim toUser As String = ""
                            '        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                            '        Dim dt As DataTable
                            '        If ViewState("estadoOI") = "E-EXC" Or ViewState("estadoOI") = "E-EBL" Then
                            '            dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                            '            For Each fila As DataRow In dt.Rows
                            '                toUser = toUser + fila("Valor") + ";"
                            '            Next
                            '            Try
                            '                UIUtility.EnviarMail(toUser, "", "Pendiente de aprobación - Orden excedido por Limites de Inversión", OrdenInversion.MensajeExcesosOI(strcodigoOrden, ddlFondo.SelectedValue.ToString(), DatosRequest), DatosRequest)
                            '            Catch ex As Exception
                            '                AlertaJS("Se ha generado un error en el proceso de envio de notificación! ")
                            '            End Try
                            '        End If
                            '        Session("dtValTrading") = ""
                            '    End If
                            'End If
                            'oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, ddlFondo.SelectedValue, "", DatosRequest)
                            'txtCodigoOrden.Value = strcodigoOrden
                            'If hdPagina.Value <> "TI" And chkFicticia.Checked = False Then
                            '    UIUtility.InsertarModificarImpuestosComisiones("I", dgLista, strcodigoOrden, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                            'End If
                            'Session("dtdatosoperacion") = ObtenerDatosOperacion()
                            'GenerarLlamado(strcodigoOrden + ViewState("CodigoOrden_T"), ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, "ACCIONES", ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), txtISIN.Text.Trim, txtSBS.Text.Trim, txtMnemonico.Text)
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                            'accionRpta = "Ingresó"
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                            'CargarPaginaInicio()

                            'INICIO | ZOLUXIONES | RCE | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
                            GuardarPreOrden()
                            accionRpta = "Ingresó" 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18                                     
                            CargarPaginaInicio()
                            'FIN | ZOLUXIONES | RCE | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
                        End If
                    Else
                        If Session("EstadoPantalla") = "Modificar" Then
                            actualizaMontos()
                            ModificarOrdenInversion()
                            FechaEliminarModificarOI("M")
                            UIUtility.InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                            CargarPaginaAceptar()
                            Session("dtdatosoperacion") = ObtenerDatosOperacion()
                            If hdPagina.Value <> "MODIFICA" Then
                                GenerarLlamado(txtCodigoOrden.Value, ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, "ACCIONES", ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), txtISIN.Text.Trim, txtSBS.Text.Trim, txtMnemonico.Text)
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
                            Select Case Session("EstadoPantalla")
                                Case "Eliminar", "Modificar"
                                    retornarMensajeAccion(accionRpta)
                                Case "Ingresar"
                                    retornarMensajeAccionPreOrden(accionRpta)
                            End Select
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                        End If
                    Else
                        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                        Select Case Session("EstadoPantalla")
                            Case "Eliminar", "Modificar"
                                retornarMensajeAccion(accionRpta)
                            Case "Ingresar"
                                retornarMensajeAccionPreOrden(accionRpta)
                        End Select
                        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
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
    Public Function crearObjetoOI() As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        oOrdenInversionBM.InicializarOrdenInversion(oRow)
        oRow.CodigoOrden = txtCodigoOrden.Value.Trim
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.CodigoISIN = txtISIN.Text
        oRow.CodigoMnemonico = txtMnemonico.Text
        oRow.CodigoSBS = txtSBS.Text
        oRow.GrupoIntermediario = ddlGrupoInt.SelectedValue 'DB 20090305
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
        oRow.CantidadOrdenado = txtNroAccOrde.Text.ToUpper.Replace(",", "")
        oRow.CantidadOperacion = txtNroAccOper.Text.Replace(",", "")
        oRow.MontoOperacion = txtMontoNominal.Text.Replace(",", "")
        oRow.Precio = txtPrecio.Text.Replace(",", "")
        oRow.TotalComisiones = txttotalComisionesC.Text.Replace(",", "")
        oRow.PrecioPromedio = txtPrecPromedio.Text.Replace(",", "")
        oRow.MontoNetoOperacion = txtMontoNetoOpe.Text.Replace(",", "")
        oRow.Situacion = "A"
        oRow.Observacion = txtObservacion.Text
        oRow.HoraOperacion = tbHoraOperacion.Text
        oRow.CategoriaInstrumento = "AC"
        oRow.Plaza = ddlPlaza.SelectedValue
        oRow.CodigoMoneda = lblMoneda.Text
        If Not ViewState("estadoOI") Is Nothing Then
            If ViewState("estadoOI").Equals("E-EXC") Or ViewState("estadoOI").Equals("E-ENV") Or ViewState("estadoOI").Equals("E-EBL") Then 'HDG OT 61166 20100920
                oRow.Estado = ViewState("estadoOI")
            End If
        End If
        If (hdPagina.Value <> "XO") Then
            oRow.NumeroPoliza = tbNPoliza.Text.ToString()
        End If
        If (hdPagina.Value = "CO") Then
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
        If Session("EstadoPantalla") = "Ingresar" Then
            If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) > UIUtility.ConvertirFechaaDecimal(objutil.RetornarFechaNegocio) _
            And chkEmisionPrimaria.Checked = True Then 'CMB OT 64769 20120328Then
                oRow.EventoFuturo = 1
            End If
        Else
            If chkEmisionPrimaria.Checked = True Then
                oRow.EventoFuturo = 1
            End If
        End If
        If Session("Mercado") = 2 Then
            oRow.TipoTramo = ddlTipoTramo.SelectedValue
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
        oRow.HoraEjecucion = tbHoraEjecucion.Text
        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
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
        oOrdenInversionBM.FechaModificarEliminarOI(ddlFondo.SelectedValue, txtCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc, txtComentarios.Text, DatosRequest)
        txtComentarios.Text = ""
    End Sub
    Public Sub CargarFechaVencimiento()
        If Session("EstadoPantalla") = "Ingresar" _
            And chkEmisionPrimaria.Checked = True Then
            tbFechaLiquidacion.Text = tbFechaOperacion.Text
        Else
            If (hdPagina.Value <> "CO") Then
                If (hdPagina.Value <> "DA") Then
                    Dim dtAux As DataTable = objPortafolio.SeleccionarPortafolioPorFiltro(ddlFondo.SelectedValue, DatosRequest).Tables(0)
                    If Not dtAux Is Nothing Then
                        If dtAux.Rows.Count > 0 Then
                            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
                        End If
                    End If
                Else
                    tbFechaOperacion.Text = Request.QueryString("Fecha")
                End If
                tbFechaLiquidacion.Text = oOrdenInversionBM.RetornarFechaVencimiento(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text, ddlFondo.SelectedValue, ddlIntermediario.SelectedValue)
            End If
        End If
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
        LimpiarSesiones()
        UIUtility.ExcluirOtroElementoSeleccion(ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion, "")
        Session("EstadoPantalla") = "Modificar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        lblAccion.Text = "Modificar"
        CargarPaginaAccion()
        hdMensaje.Value = "la Modificación"
        hdSaldo.Value = 0
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
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
    Private Function CargarCaracteristicasValor() As Boolean
        Try
            If Me.ddlFondo.SelectedValue = String.Empty Then Return False
            Dim oOIFormulas As New OrdenInversionFormulasBM
            Dim dsValor As DataSet = oOIFormulas.SeleccionarCaracValor_Acciones(ddlFondo.SelectedValue, Session("Nemonico"), DatosRequest)
            Dim drValor As DataRow

            Dim dtPortafolio As DataTable
            Dim htPortafolio As Hashtable
            Dim sPortafolio As String = String.Empty

            If dsValor.Tables(0).Rows.Count > 0 Then
                If Session("EstadoPantalla") = "Ingresar" Then
                    chkEmisionPrimaria.Enabled = True
                End If
                dtPortafolio = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO)
                htPortafolio = New Hashtable
                drValor = dsValor.Tables(0).NewRow
                drValor = dsValor.Tables(0).Rows(0)
                Session("TipoRenta") = CType(drValor("val_TipoRenta"), String)
                Session("CodigoMoneda") = CType(drValor("val_CodigoMoneda"), String)
                If Not ((hdPagina.Value = "EO") Or (hdPagina.Value = "XO") Or (hdPagina.Value = "MODIFICA")) Then
                    Session("Mercado") = CType(drValor("val_Mercado"), String)
                End If
                lblMoneda.Text = CType(drValor("val_CodigoMoneda"), String)
                txtISIN.Text = CType(drValor("val_CodigoISIN"), String)
                txtSBS.Text = CType(drValor("val_CodigoSBS"), String)
                lblSaldoValor.Text = CType(drValor("SaldoValor"), String)
                lblMarketCap.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_MarketCap")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                txtporcentaje.Text = CType(drValor("Porcentaje"), String)
                lblSigDivFecha.Text = UIUtility.ConvertirFechaaString(CType(drValor("val_sigDivFecha"), String))
                lblSigDivFactor.Text = CType(drValor("val_sigDivFactor"), Decimal)
                lblMonNegDiaProm.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_MontoNegDiarProm")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                lblNroOperDiaProm.Text = CType(drValor("val_NroOperDiarProm"), String)
                lblPriceEarnings.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_PriceEarnings")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                lblValorDFC.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_ValorDFC")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Return True
            End If

        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF21"))
        End Try
        Return False
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
    Private Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        LimpiarSesiones()
        Call ConfiguraModoConsulta()
    End Sub
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
        Return ddlFondo.SelectedValue
    End Function
    Private Function GetOPERACION() As String
        Return IIf(ddlOperacion.SelectedIndex = 0, "", ddlOperacion.SelectedValue)
    End Function
    Private Function GetMONEDA() As String
        Return lblMoneda.Text
    End Function
    Private Sub btnCaracteristicas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCaracteristicas.Click
        If txtMnemonico.Text <> "" Then
            Session("accionValor") = "MODIFICAR"
            Dim strURL As String = "../../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" + txtMnemonico.Text + "&vOI=T"
            '    EjecutarJS("showModalDialog('" & strURL & "', '1030', '600', '');")
            EjecutarJS(UIUtility.MostrarPopUp(strURL, "no", 1030, 600, 0, 0, "No", "No", "Yes", "Yes"), False)
        Else
            AlertaJS(ObtenerMensaje("CONF23"))
        End If
    End Sub
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal cportafolio As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&cportafolio=" + cportafolio + "&vportafolio=" + portafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
    End Sub
#Region " /* Métodos Personalizados (Popups Dialogs) */ "
    Private Sub MostrarOtrosCustodios(ByVal fechaOper As String, ByVal mnemonico As String, ByVal fondo As String, ByVal saldo As String, ByVal codigoCustodio As String)
        Dim strURL As String = "frmBuscarValorCustodios.aspx?vMnemonico='" + mnemonico + " '&vFecha='" + fechaOper + "'&vFondo='" + fondo + "'&vSaldo='" + saldo + "'&vCustodio='" + codigoCustodio
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '');")
    End Sub
    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal operacion As String, ByVal categoria As String)
        Dim strURL As String = "frmBuscarValor.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&vFondo=" + ddlFondo.SelectedItem.Text + "&cFondo=" + ddlFondo.SelectedValue + "&vOperacion=" + operacion + "&vCategoria=" + categoria
        EjecutarJS("showModalDialog('" & strURL & "', '950', '500', '" & btnBuscar.ClientID & "'); document.getElementById('hdPopUp').value='V'; ")
    End Sub
    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal cfondo As String, ByVal fondo As String, ByVal operacion As String, ByVal moneda As String, ByVal fecha As String, ByVal accion As String)
        Dim strURL As String = "../frmInversionesRealizadas.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&cFondo=" + cfondo + "&vFondo=" + fondo + "&vOperacion=" + operacion + "&vFechaOperacion=" + fecha + "&vAccion=" + accion + "&vCategoria=AC"
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hdPopUp').value='IR'; ")
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
        tbHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Traspaso"
        hdMensaje.Value = "el Ingreso"
        If ddlFondo.SelectedValue <> "MULTIFONDO" Then
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
        Else
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
        End If
        CargarPaginaIngresar()
        CargarIntermediario()
        OrdenInversion.ObtieneImpuestosComisiones(dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
        HabilitaDeshabilitaValoresGrilla(False)
    End Sub
    Private Sub ControlarCamposEO_CO_XO()
        MostrarOcultarBotonesAcciones(False)
        btnAceptar.Enabled = True
    End Sub
    Private Sub ControlarCamposOE()
        MostrarOcultarBotonesAcciones(False)
        btnAceptar.Enabled = True
    End Sub
    Private Sub CargarPaginaInicio()
        HabilitaDeshabilitaCabecera(False)
        HabilitaDeshabilitaDatosOperacionComision(False)
        OcultarBotonesInicio()
        btnAceptar.Enabled = False
    End Sub
    Private Sub CargarPaginaBuscar()
        If ddlFondo.SelectedValue = "MULTIFONDO" Then
            btnAsignar.Visible = True
            btnAsignar.Enabled = True
        End If
        btnProcesar.Visible = True
        btnProcesar.Enabled = True
    End Sub
    Private Sub CargarPaginaIngresar()
        CargarPaginaBuscar()
        btnAsignar.Visible = False
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
        lblComentarios.InnerText = "Comentarios modificación:"
        txtComentarios.Text = ""
    End Sub
    Private Sub CargarPaginaModificarEO_CO_XO(ByVal acceso As String)
        If acceso = "EO" Or acceso = "CO" Then
            CargarPaginaBuscar()
            HabilitaDeshabilitaCabecera(False)
            btnBuscar.Visible = False
            HabilitaDeshabilitaDatosOperacionComision(True)
            btnCaracteristicas.Visible = True
            btnCaracteristicas.Enabled = True
            btnAceptar.Enabled = True
            Session("EstadoPantalla") = "Modificar"
            CargarContactos()
        End If
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
        lblComentarios.InnerText = "Comentarios eliminación:"
        txtComentarios.Text = ""
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
        btnAceptar.Enabled = False
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
    Private Sub CargarPaginaAceptar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        If Session("EstadoPantalla") = "Ingresar" Then
            btnImprimir.Visible = True
            btnImprimir.Enabled = True
            If ddlFondo.SelectedValue = "MULTIFONDO" Then
                btnAsignar.Visible = True
            End If
        End If
        btnAceptar.Enabled = False
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
        ddlGrupoInt.Enabled = estado
        ddlIntermediario.Enabled = estado
        ddlPlaza.Enabled = estado
        ddlTipoTramo.Enabled = estado
        ddlContacto.Enabled = estado
        dgLista.Enabled = estado
        HabilitaDeshabilitaValoresGrilla(estado)
        'tbFechaOperacion.ReadOnly = Not estado
        tbFechaLiquidacion.ReadOnly = Not estado
        txtNroAccOrde.ReadOnly = Not estado
        txtNroAccOper.ReadOnly = Not estado
        txtPrecio.ReadOnly = Not estado
        tbHoraOperacion.ReadOnly = Not estado
        txtObservacion.ReadOnly = Not estado
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se habilita/deshabilita campo "Monto Operación" en carga de formulario | 06/06/18
        txtMontoNominal.ReadOnly = Not estado
        txtMontoNetoOpe.ReadOnly = Not estado
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se habilita/deshabilita campo "Monto Operación" en carga de formulario | 06/06/18
        If estado Then
            'imgFechaOperacion.Attributes.Add("class", "input-append date")
            imgFechaVcto.Attributes.Add("class", "input-append date")
        Else
            'imgFechaOperacion.Attributes.Add("class", "input-append")
            imgFechaVcto.Attributes.Add("class", "input-append")
        End If
        chkFicticia.Enabled = False
        chkEmisionPrimaria.Enabled = False
        If (Not Session("EstadoPantalla") Is Nothing And Not Session("Procesar") Is Nothing) Then
            If Session("EstadoPantalla") = "Ingresar" And ddlFondo.SelectedValue <> "MULTIFONDO" And Session("Procesar") = "0" Then
                chkFicticia.Enabled = True
                chkEmisionPrimaria.Enabled = True
            End If
        End If
        If ddlFondo.SelectedValue = "MULTIFONDO" Then
            chkRegulaSBS.Enabled = False
        Else
            chkRegulaSBS.Enabled = estado
        End If
        ddlMedioTrans.Enabled = estado
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
        btnAsignar.Visible = False
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
        lblMarketCap.Text = ""
        lblSigDivFactor.Text = ""
        lblSigDivFecha.Text = ""
        lblMonNegDiaProm.Text = ""
        lblNroOperDiaProm.Text = ""
        lblPriceEarnings.Text = ""
        lblValorDFC.Text = ""
        lblSaldoValor.Text = ""
    End Sub
    Private Sub LimpiarDatosOperacion()
        lblMoneda.Text = ""
        ddlFondo.SelectedIndex = 0
        ddlOperacion.SelectedIndex = 0
        txtISIN.Text = ""
        txtSBS.Text = ""
        txtMnemonico.Text = ""
        tbFechaOperacion.Text = ""
        tbFechaLiquidacion.Text = ""
        txtNroAccOrde.Text = ""
        txtNroAccOper.Text = ""
        txtPrecio.Text = ""
        txtMontoNominal.Text = ""
        ddlGrupoInt.SelectedIndex = 0
        If ddlIntermediario.Items.Count > 0 Then ddlIntermediario.SelectedIndex = 0
        CargarContactos()
        ddlContacto.SelectedIndex = 0
        tbHoraOperacion.Text = ""
        dgLista.Dispose()
        dgLista.DataBind()
        txtObservacion.Text = ""
        txttotalComisionesC.Text = ""
        txtMontoNetoOpe.Text = ""
        txtPrecPromedio.Text = ""
        chkEmisionPrimaria.Checked = False
    End Sub
#End Region
    Private Sub btnAsignar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsignar.Click
        Session("URL_Anterior") = Page.Request.Url.AbsolutePath.ToString
        If ddlFondo.SelectedValue.ToString = PORTAFOLIO_MULTIFONDOS Then
            Response.Redirect("../AsignacionFondos/frmIngresoCriteriosAsignacion.aspx?vISIN=" & txtISIN.Text.ToString & "&vCantidad=" & txtNroAccOper.Text.ToString & "&vMnemonico=" & txtMnemonico.Text.ToString & "&vFondo=" & ddlFondo.SelectedValue.ToString & "&vOperacion=" & ddlOperacion.SelectedItem.Text & "&vImpuestosComisiones=" & txttotalComisionesC.Text & "&vMoneda=" & lblMoneda.Text & "&vCodigoOrden=" & txtCodigoOrden.Value & "&vCategoria=AC", False)
        End If
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(txtCodigoOrden.Value, ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, "ACCIONES", ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), txtISIN.Text.Trim, txtSBS.Text.Trim, txtMnemonico.Text)
    End Sub
    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If ddlFondo.SelectedValue <> "" Then
            CargarFechaVencimiento()
        End If
        Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        If cantidadreg > 0 Then
            AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
            'OT 9968 - 14/02/2017 - Carlos Espejo
            'Descripcion: Se cambio el modo de indicar el indice
            ddlFondo.SelectedIndex = 0
            'OT 9968 Fin
            Exit Sub
        End If
    End Sub
    Private Sub ddlGrupoInt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrupoInt.SelectedIndexChanged
        UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        CargarContactos()
        If Session("EstadoPantalla") = "Ingresar" _
        And chkEmisionPrimaria.Checked = True Then
            tbFechaOperacion.Enabled = True
        Else
            tbFechaOperacion.Enabled = False
        End If
    End Sub
    Private Sub ConsultaLimitesPorInstrumento()
        Dim strFondo As String = ddlFondo.SelectedValue
        Dim strEscenario As String = "REAL"
        Dim strFecha As String = tbFechaOperacion.Text
        Dim strValorNivel As String = txtMnemonico.Text
        Dim strURL As String = "../Reportes/Orden de Inversion/frmVisorReporteLimitesPorInstrumento.aspx?Portafolio=" & strFondo & "&ValorNivel=" & strValorNivel & "&Escenario=" & strEscenario & "&Fecha=" & strFecha
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '');")
    End Sub
    Public Sub SaldoUnidades_EnModificacion(ByVal lblSaldoValor As TextBox, ByVal UnidadesNegociadas As Decimal, ByVal CodigoOperacion As String)
        If Session("EstadoPantalla") = "Modificar" And CodigoOperacion = "2" Then
            lblSaldoValor.Text = CDec(lblSaldoValor.Text) + UnidadesNegociadas
        End If
    End Sub
    Protected Sub ddlPlaza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPlaza.SelectedIndexChanged
        Session("Mercado") = ddlPlaza.SelectedValue
        dgLista.Dispose()
        dgLista.DataBind()
        OrdenInversion.ObtieneImpuestosComisiones(dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
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
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF008 - Se implementa las funciones para el guardado en PreOrden  | 17/08/18 
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
        oRow.Cantidad = txtNroAccOrde.Text.ToUpper.Replace(",", "")
        oRow.CantidadOperacion = txtNroAccOrde.Text.ToUpper.Replace(",", "")
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

    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF008 - Se implementa las funciones para el guardado en PreOrden  | 17/08/18 

   
End Class
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports System.IO
Imports iTextSharp.text.pdf
Imports ParametrosSIT
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmOpcionesDerivadasForwardDivisas
    Inherits BasePage
    Dim objPortafolio As New PortafolioBM
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
    Protected WithEvents hdFondoD As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtMontoPlazo As System.Web.UI.WebControls.TextBox
    Dim oMotivoBM As New MotivoBM
    Dim sOISwap As String
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Request.QueryString("PTNegSim") = "M" Then
                trMotivoCambio.Attributes.Remove("class")
                trMotivoCambio.Attributes.Add("class", "row")
            End If
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            hdSaldo.Value = 0
            '    btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
            btnProcesar.Attributes.Add("onclick", "javascript:return Validar();")
            btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
            btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
            If Not Page.IsPostBack Then
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                hdRptaConfirmar.Value = "NO"
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                LimpiarSesiones()
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    hdPagina.Value = Request.QueryString("PTNeg")
                    If hdPagina.Value = "CO" Then
                        pnISIN.Visible = True
                    End If
                End If
                If (hdPagina.Value = "TI") Then
                    UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
                Else
                    UIUtility.CargarOperacionOI(ddlOperacion, "Forward")
                End If
                If Not Request.QueryString("PTNegSim") Is Nothing Then
                    hdPagina.Value = "SW"
                End If
                UIUtility.CargarMonedaOI(ddlMonedaOrigen)
                UIUtility.CargarMonedaOI(ddlMonedaDestino)
                CargarIntermediario()
                CargarMotivo()
                CargarPaginaInicio()
                CargarTipoMoneda()
                hdPagina.Value = ""
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    UIUtility.CargarPortafoliosOI(ddlFondo)
                    hdPagina.Value = Request.QueryString("PTNeg")
                    If hdPagina.Value = "TI" Then  'Viene de la Pagina Traspaso de Instrumentos
                        ddlFondo.SelectedValue = Request.QueryString("PTFondoO")
                        ddlFondoDestino.SelectedValue = Request.QueryString("PTFondoD")
                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                        hdCustodio.Value = Request.QueryString("PTCustodio")
                        hdSaldo.Value = Request.QueryString("PTSaldo")
                        ControlarCamposTI()
                    Else
                        txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                        ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                        If (hdPagina.Value = "EO") Or (hdPagina.Value = "CO") Or (hdPagina.Value = "XO") Then 'Viene de la Pagina Confirmacion , Ejecucion  o Extorno de Ordenes de Inversion
                            'OT 10019 - 22/02/2017 - Carlos Espejo
                            'Descripcion: Se agrega el parametro CodigoOrden
                            CargarDatosOrdenInversion(Request.QueryString("PTNOrden"))
                            'OT 10019 Fin
                            tbNPoliza.Text = Right(txtCodigoOrden.Value, 5)
                            tbNPoliza.ReadOnly = True
                            If (hdPagina.Value <> "XO") Then
                                If (hdPagina.Value = "EO") Then
                                    tbNPoliza.Text = ""
                                    lNPoliza.Visible = False
                                    tbNPoliza.Visible = False
                                Else
                                    lNPoliza.Visible = True
                                    tbNPoliza.Visible = True
                                End If
                            End If
                            ControlarCamposEO_CO_XO()
                            CargarPaginaModificarEO_CO_XO(hdPagina.Value)
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                            If hdPagina.Value = "CO" Then
                                btnAceptar.Text = "Grabar y Confirmar"
                                If Session("ValidarFecha").ToString = "FECHADIFERENTE" Then CargarPaginaInicio()
                            End If
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18     Else
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
                                        txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                        'OT 10019 - 22/02/2017 - Carlos Espejo
                                        'Descripcion: Se agrega el parametro CodigoOrden
                                        CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
                                        'OT 10019 Fin
                                        Call HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, True, False)
                                    End If
                                End If
                            End If
                        End If
                    End If
                    btnSalir.Attributes.Remove("onClick")
                    '    btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                    '  btnAceptar.UseSubmitBehavior = True
                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF49", txtCodigoOrden.Value + "?", "SI")
                Else
                    Dim portafolio As DataTable = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
                    HelpCombo.LlenarComboBox(ddlFondo, portafolio, "CodigoPortafolio", "Descripcion", True)
                    If Not Request.QueryString("PTNegSim") Is Nothing Then
                        Dim sAcc As String
                        hdPagina.Value = "SW"
                        sAcc = Request.QueryString("PTNegSim")
                        If (sAcc = "I") Then
                            ConfiguraModoInserta()
                            '    btnAceptar.UseSubmitBehavior = False
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
                        ElseIf (sAcc = "M") Then
                            ConfiguraModoModifica()
                            CargarPaginaModificar()
                            btnAceptar.UseSubmitBehavior = False
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
                        ElseIf (sAcc = "E") Then
                            ConfiguraModoElimina()
                            If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF3", "", "SI")
                            Else
                                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF17", "SI")
                            End If
                            Dim usu As String = Usuario
                            CargarPaginaEliminar()
                        ElseIf (sAcc = "C") Then
                            ConfiguraModoConsulta()
                            CargarPaginaConsultar()
                        End If
                        If hdPagina.Value = "SW" Then OcultarDeshabilitaInicioSwap()
                        lblRenovacion.Visible = False
                        rbnRenovacion.Visible = False
                        btnSalir.Attributes.Remove("onClick")
                        '     btnSalir.Attributes.Add("onClick", "javascript:return Salida2();")
                    End If
                End If
            Else
                If Request.QueryString("PTNegSim") = "M" Then
                    EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
                End If
                If (hdPagina.Value = "CO") Then
                    If Not Request.Form("hddLoad") Is Nothing Then
                        If Request.Form("hddLoad").ToString.Equals("1") Then
                            CargarDatosOrdenInversion(Session("Orden"))
                            hddLoad.Value = "0"
                        End If
                    End If
                Else
                    '   btnAceptar.UseSubmitBehavior = False
                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
                    If (hdPagina.Value = "EO") Then
                        tbNPoliza.Text = ""
                        lNPoliza.Visible = False
                        tbNPoliza.Visible = False
                    End If
                End If
            End If
            ValTipoFor()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Call HabilitaBotones(True, True, True, True, True, True, True, True, True, True, True, False, True)
    End Sub
    Private Sub HabilitaBotones(ByVal bLimites As Boolean, ByVal bIngresar As Boolean, _
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
        Session("ReporteLimitesEvaluados") = Nothing
        Session("dtdatosoperacion") = Nothing
        Session("accionValor") = Nothing
    End Sub
    Public Sub CargarIntermediario()
        ddlIntermediario.Items.Clear()
        UIUtility.CargarIntermediariosOISoloBancos(ddlIntermediario)
        Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
    End Sub
    Private Sub CargarMotivo()
        ddlMotivo.DataSource = oMotivoBM.Listar(DatosRequest)
        ddlMotivo.DataTextField = "Descripcion"
        ddlMotivo.DataValueField = "CodigoMotivo"
        ddlMotivo.DataBind()
        ddlMotivo.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If ddlFondo.SelectedValue = "" Then
            AlertaJS("Seleccione un portafolio")
        Else
            If Not Session("SS_DatosModal") Is Nothing Then
                ddlFondo.SelectedValue = CType(Session("SS_DatosModal"), String())(3).ToString.Trim()
                ddlOperacion.SelectedValue = CType(Session("SS_DatosModal"), String())(4).ToString.Trim()
                txtCodigoOrden.Value = CType(Session("SS_DatosModal"), String())(6).ToString.Trim()
                Session("Orden") = txtCodigoOrden.Value
                Session.Remove("SS_DatosModal")
            End If
            If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Or Session("EstadoPantalla") = "Consultar" Then
                If Session("Busqueda") = 0 Then
                    txtCodigoOrden.Value = ""
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
                    ShowDialogPopupInversionesRealizadas(String.Empty, String.Empty, String.Empty, ddlFondo.SelectedItem.Text, ddlFondo.SelectedValue, ddlOperacion.SelectedValue, ddlMonedaOrigen.SelectedValue.ToString, strAux, strAccion)
                    Session("Busqueda") = 2
                Else
                    If Session("Busqueda") = 1 Then
                        CargarDatosOrdenInversion(Session("Orden"))
                        If Session("EstadoPantalla") = "Modificar" Then
                            CargarPaginaModificar()
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF2", "Nro " + txtCodigoOrden.Value + "?", "SI")
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
                strMensajeConfirmacion = "¿Desea cancelar la Ejecución de la órden de inversión Nro. " + NroOrden + "?"
            Case "CO"
                strMensajeConfirmacion = "¿Está seguro de salir de la Confirmación de la órden de inversión Nro. " + NroOrden + "?"
            Case "XO"
                strMensajeConfirmacion = "¿Desea cancelar el Extorno de la órden de inversión Nro. " + NroOrden + "?"
            Case "OE"
                strMensajeConfirmacion = "¿Desea cancelar la Aprobacion de la órden de inversión Excedida Nro. " + NroOrden + "?"
            Case "DA"
                strMensajeConfirmacion = "¿Desea cancelar la Negociación de la órden de inversión Nro. " + NroOrden + "?"
            Case Else
                If (strAccion <> String.Empty) Then
                    If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de la órden de inversión de Forward de Divisas?"
                    Else
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de pre-orden de inversión de Forward de Divisas?"
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
        Call ConfiguraModoInserta()
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        Call ConfiguraModoModifica()
    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Call ConfiguraModoElimina()
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        Call ConfiguraModoConsulta()
    End Sub
    Private Sub ConfiguraModoInserta()
        UIUtility.ExcluirOtroElementoSeleccion(ddlOperacion)
        CargarPaginaAccion()
        Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        lblAccion.Text = "Ingresar"
        CargarPaginaIngresar()
        hdMensaje.Value = "el Ingreso"
        hdNumUnidades.Value = 0
        If Not ddlFondo.Items.FindByValue(PORTAFOLIO_MULTIFONDOS) Is Nothing Then
            ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS
        End If
        lblTitulo.Text = "PreOrden de Inversión - OPCIONES DERIVADAS - FORWARD DIVISAS"
        If (hdPagina.Value <> "SW") Then
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
        Else
            If (hdPagina.Value = "SW") Then
                ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                ddlOperacion.SelectedValue = Request.QueryString("Operacion")
                ddlMonedaOrigen.SelectedValue = "DOL"
                ddlMonedaDestino.SelectedValue = "NSOL"
                ddlMonedaOrigen.Enabled = False
                ddlMonedaDestino.Enabled = False
                sOISwap = Request.QueryString("OI")
                If sOISwap = "2" Then
                    tbMontoOrigen.Text = Session("MontoOperacion")
                    ddlIntermediario.SelectedValue = (Session("Intermediario"))
                    tbMontoOrigen.Enabled = False
                    ddlIntermediario.Enabled = False
                    CargarContactos()
                End If
            End If
        End If
        tbFechaLiquidacion.Text = tbFechaOperacion.Text
        tbHoraOperacion.Text = objutil.RetornarHoraSistema

        'INICIO | ZOLUXIONES | DACV | ProyFondosII 
        ddlMotivo.SelectedValue = "11"
        'FIN | ZOLUXIONES | DACV | ProyFondosII 
    End Sub
    Private Sub ConfiguraModoModifica()
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row")
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion, "")
        Session("EstadoPantalla") = "Modificar"
        Session("Busqueda") = 0
        Session("Procesar") = 0
        lblAccion.Text = "Modificar"
        CargarPaginaAccion()
        hdMensaje.Value = "la Modificación"
        HelpCombo.CargarMotivosCambio(Me)
        If hdPagina.Value = "SW" Then
            ddlFondo.SelectedValue = Request.QueryString("Portafolio")
            txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
            tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
            CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
            CargarPaginaModificar()
        End If
    End Sub
    Private Sub ConfiguraModoElimina()
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row")
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion, "")
        Session("EstadoPantalla") = "Eliminar"
        lblAccion.Text = "Eliminar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Eliminación"
        HelpCombo.CargarMotivosCambio(Me)
        If hdPagina.Value = "SW" Then
            ddlFondo.SelectedValue = Request.QueryString("Portafolio")
            txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
            tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
            CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
            CargarPaginaEliminar()
        End If
    End Sub
    Private Sub ConfiguraModoConsulta()
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion, "")
        Session("EstadoPantalla") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Consulta"
        If hdPagina.Value = "SW" Then
            ddlFondo.SelectedValue = Request.QueryString("Portafolio")
            txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
            tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
            CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
            CargarPaginaConsultar()
        End If
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
                    'OT10855 11/10/2017 - La comparación debe de ser entre tipo de caracteres String
                    If ddlOperacion.SelectedValue = "95" Then
                        hdCustodio.Value = dtAux.Rows(i)("codigoCustodio")
                    End If
                    Session("Mercado") = dtAux.Rows(i)("mercado")
                    Exit For
                End If
            Next
        End If
    End Sub
    Private Sub CargarDatosOrdenInversion(Orden As String)
        Try
            oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(Orden, ddlFondo.SelectedValue, DatosRequest, PORTAFOLIO_MULTIFONDOS)
            Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
            oRow = oOrdenInversionBE.Tables(0).Rows(0)
            txtCodigoOrden.Value = oRow.CodigoOrden
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
            hdNemonico.Value = oRow.CodigoMnemonico
            ddlFondo.SelectedValue = oRow.CodigoPortafolioSBS
            ddlOperacion.SelectedValue = oRow.CodigoOperacion
            tbHoraOperacion.Text = oRow.HoraOperacion
            tbMontoOrigen.Text = oRow.MontoCancelar.ToString.Replace(UIUtility.DecimalSeparator, ".")
            ddlIntermediario.SelectedValue = oRow.CodigoTercero
            CargarContactos()
            ddlContacto.SelectedValue = oRow.CodigoContacto
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
            tbFechaFinContrato.Text = UIUtility.ConvertirFechaaString(oRow.FechaContrato)
            If oRow.Delibery <> "" Then
                rbnDelivery.SelectedValue = oRow.Delibery
            End If
            txtTcFuturo.Text = oRow.TipoCambioFuturo.ToString.Replace(UIUtility.DecimalSeparator, ".")
            txtPlazo.Text = oRow.Plazo.ToString.Replace(UIUtility.DecimalSeparator, ".")
            txtMontoFuturo.Text = oRow.MontoOperacion.ToString.Replace(UIUtility.DecimalSeparator, ".")
            txtObservacion.Text = oRow.Observacion
            tbHoraOperacion.Text = oRow.HoraOperacion
            ddlMonedaOrigen.SelectedValue = oRow.CodigoMonedaOrigen
            ddlMonedaDestino.SelectedValue = oRow.CodigoMonedaDestino
            tbTcSpot.Text = oRow.TipoCambioSpot.ToString.Replace(UIUtility.DecimalSeparator, ".")
            TXTisin.Text = oRow.CodigoISIN
            Try
                ddlMotivo.SelectedValue = oRow.CodigoMotivo
            Catch ex As Exception
            End Try

            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga data de motivo de cambio si fuera el caso | 07/06/18 
            If oRow.CodigoMotivoCambio <> String.Empty Then ddlMotivoCambio.SelectedValue = oRow.CodigoMotivoCambio
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga data de motivo de cambio si fuera el caso | 07/06/18 

            If (hdPagina.Value = "EO") Then
                tbNPoliza.Text = ""
                lNPoliza.Visible = False
                tbNPoliza.Visible = False
            Else
                If hdPagina.Value = "CO" Then
                    tbNPoliza.Text = oRow.NumeroPoliza
                End If
            End If
            If oRow.Ficticia = "S" Then
                chkFicticia.Checked = True
            Else
                chkFicticia.Checked = False
            End If
            If oRow.Renovacion = "" Then
                rbnRenovacion.SelectedValue = "N"
            Else
                rbnRenovacion.SelectedValue = oRow.Renovacion
            End If
            CargarTipoMoneda()
            ddlTipoMoneda.SelectedValue = oRow.TipoMonedaForw


        Catch ex As Exception
            If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                AlertaJS(ObtenerMensaje("CONF31"))
            Else
                AlertaJS(ObtenerMensaje("CONF32"))
            End If
        End Try
    End Sub
    Private Sub CargarTipoMoneda()
        ddlTipoMoneda.DataSource = oOrdenInversionBM.SeleccionarTipoMonedaxMotivoForw(ddlMotivo.SelectedValue, "")
        ddlTipoMoneda.DataTextField = "Descripcion"
        ddlTipoMoneda.DataValueField = "CodigoMoneda"
        ddlTipoMoneda.DataBind()
        ddlTipoMoneda.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        Dim accionRpta As String = String.Empty
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        Try   'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
            If hdRptaConfirmar.Value.ToUpper = "SI" Then
                EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
                If hdPagina.Value <> "" And hdPagina.Value <> "DA" And hdPagina.Value <> "TI" And hdPagina.Value <> "SW" Then
                    If hdPagina.Value = "XO" Then
                        oOrdenInversionWorkFlowBM.ExtornarOIEjecutadas(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest)
                        ReturnArgumentShowDialogPopup()
                    Else
                        If hdPagina.Value = "EO" Then
                            oOrdenInversionWorkFlowBM.EjecutarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, tbNPoliza.Text.Trim, DatosRequest)
                            ReturnArgumentShowDialogPopup()
                        Else
                            If hdPagina.Value = "CO" Then
                                'Cuando se confirma la orden se debe ingresar el codigoISIN para actualizar la orden
                                If TXTisin.Text.Length < 12 Then
                                    AlertaJS("El codigo ISIN debe tener 12 caracteres.")
                                    TXTisin.Focus()
                                    Exit Sub
                                End If
                                oOrdenInversionWorkFlowBM.ActualizaISINOrden(txtCodigoOrden.Value, ddlFondo.SelectedValue, TXTisin.Text)
                                If oOrdenInversionWorkFlowBM.ConfirmarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, tbNPoliza.Text.Trim, DatosRequest) Then
                                    EjecutarJS("<script language='JavaScript'>" & _
                                            "document.getElementById('hddLoad').value = '0';" & _
                                            "if(confirm('El Nro. de Poliza ya se asignó.<br>¿Desea cargar los datos nuevamente?.')){" & _
                                                "document.getElementById('hddLoad').value = '1';" & _
                                                "document.getElementById('frmInvocador').submit();" & _
                                            "}" & _
                                        "</script>", False)
                                    Exit Sub
                                End If
                                ReturnArgumentShowDialogPopup()
                            End If
                        End If
                    End If
                    If hdPagina.Value = "EO" Or hdPagina.Value = "CO" Then
                        If hdPagina.Value <> "CO" Then
                            ObtieneCustodiosSaldos()
                        End If
                        'OT 9968 14/02/2017 - Carlos Espejo
                        'Descripcion: Se cambio el mensaje de la validacion
                        If Not ModificarOrdenInversion() Then
                            AlertaJS("Error en la información ingresada para crear la negociacion.")
                            Exit Sub
                        End If
                        'OT 9968 Fin
                        Session("Modificar") = 0
                        CargarPaginaAceptar()
                    End If
                Else
                    If hdPagina.Value = "" Or hdPagina.Value = "TI" Or hdPagina.Value = "DA" Then
                        If Session("EstadoPantalla") = "Ingresar" Then
                            Dim strcodigoOrden As String
                            strcodigoOrden = InsertarOrdenInversion()
                            If strcodigoOrden = "VFWD" Then
                                AlertaJS("Registro no ingresado. El intermediario es extranjero y su fecha de vencimiento es mayor al 20/08/2013 ") '#ERROR#
                                Exit Sub
                            End If
                            If strcodigoOrden <> "" Then
                                If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then '#ERROR#
                                    If ViewState("EstadoOI") = "E-EXC" Then '#ERROR#

                                        Dim toUser As String = ""
                                        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                                        Dim dt As DataTable
                                        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(USUARIOS_ENVIO_FASEI, "", "", "", DatosRequest)
                                        For Each fila As DataRow In dt.Rows
                                            toUser = toUser + fila("Valor").ToString() & ";" '#ERROR#
                                        Next
                                        Try
                                            UIUtility.EnviarMail(toUser, "", "Pendiente de aprobación - Orden excedido por Limites de Inversión", OrdenInversion.MensajeExcesosOI(strcodigoOrden, ddlFondo.SelectedValue, DatosRequest), DatosRequest) 'CMB OT 62254 20110418
                                        Catch ex As Exception
                                            AlertaJS("Se ha generado un error en el proceso de envio de notificación! ")
                                        End Try
                                    End If
                                End If
                            End If
                            oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, ddlFondo.SelectedValue, "", DatosRequest)
                            txtCodigoOrden.Value = strcodigoOrden
                            Session("dtdatosoperacion") = ObtenerDatosOperacion()
                            GenerarLlamado(strcodigoOrden + ViewState("CodigoOrden_T"), ddlFondo.SelectedValue, "OPERACIONES DERIVADAS - FORWARD DIVISAS", ddlOperacion.SelectedItem.Text, ddlMonedaOrigen.SelectedValue, hdNemonico.Value)
                            CargarPaginaAceptar()
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                            accionRpta = "Ingresó"
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                        Else
                            Dim strAlerta As String = ""
                            If ddlMotivoCambio.SelectedIndex <= 0 Then
                                strAlerta = "-Elija el motivo por el cual desea " & Session("EstadoPantalla") & " esta operación.<br>"
                            End If
                            If strAlerta.Length > 0 Then
                                AlertaJS(strAlerta)
                                Exit Sub
                            End If
                            If Session("EstadoPantalla") = "Modificar" Then
                                If Not ModificarOrdenInversion() Then
                                    AlertaJS("- Registro no modificado. El intermediario es extranjero y su fecha de vencimiento es mayor al 20/08/2013 ")
                                    Exit Sub
                                End If
                                FechaEliminarModificarOI("M")
                                Session("Modificar") = 0
                                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
                                accionRpta = "Modificó"
                                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
                                CargarPaginaAceptar()
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
                        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                        If (Session("EstadoPantalla") = "Eliminar" Or (Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Ingresar")) Then
                            retornarMensajeAccion(accionRpta)
                        End If
                        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                    ElseIf hdPagina.Value = "SW" Then
                        sOISwap = Request.QueryString("OI")
                        If Session("EstadoPantalla") = "Ingresar" Then
                            oOrdenInversionBE = crearObjetoOI()
                            Session("OrdenInversionBE" & sOISwap) = oOrdenInversionBE.OrdenPreOrdenInversion
                        Else
                            Dim strAlerta As String = ""
                            If ddlMotivoCambio.SelectedIndex <= 0 Then
                                strAlerta = "-Elija el motivo por el cual desea " & Session("EstadoPantalla") & " esta operación.<br>"
                            End If
                            If strAlerta.Length > 0 Then
                                AlertaJS(strAlerta)
                                Exit Sub
                            End If
                            If Session("EstadoPantalla") = "Modificar" Then
                                If validaFechas() = True Then
                                    oOrdenInversionBE = crearObjetoOI()
                                    Session("OrdenInversionBE" & sOISwap) = oOrdenInversionBE.OrdenPreOrdenInversion
                                    Session("Comentarios" & sOISwap) = txtComentarios.Text
                                End If
                            ElseIf Session("EstadoPantalla") = "Eliminar" Then
                                oOrdenInversionBE = crearObjetoOI()
                                Session("OrdenInversionBE" & sOISwap) = oOrdenInversionBE.OrdenPreOrdenInversion
                                Session("Comentarios" & sOISwap) = txtComentarios.Text
                            End If
                        End If
                        EjecutarJS("window.close();")
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Function MensajeExcesodeLimite(ByVal numeroOrden As String) As String
        Dim mensaje As New StringBuilder
        Dim nroOrden As String = numeroOrden
        Dim fondo As String = ddlFondo.SelectedValue
        Dim operacion As String = ddlOperacion.SelectedItem.Text
        Dim orden As String = "Forward y divisas"
        Dim fecha As String = DateTime.Now.ToString("dd/MM/yyyy")
        With mensaje
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='550' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
            .Append("<tr><td colspan='3'>La siguiente orden emitido el " & fecha & ", se encuentra pendiente para su aprobación:</td></tr>")
            .Append("<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td width='35%'>Numero de Orden:</td>")
            .Append("<td colspan='2' width='65%'>" & nroOrden & "</td></tr>")
            .Append("<tr><td width='35%'>Fondo:</td><td colspan='2' width='65%'>" & fondo & "</td></tr>")
            .Append("<tr><td width='35%'>Tipo de Operación:</td><td colspan='2' width='65%'>" & operacion & "</td></tr>")
            .Append("<tr><td width='35%'>Tipo de Orden: </td><td colspan='2' width='65%'>" & orden & "</td></tr>")
            .Append("<tr><td width='35%'>C&oacute;digo Nem&oacute;nico:</td><td colspan='2' width='65%'>FORWARD</td></tr>")
            .Append("<tr height='8'><td colspan='3'></td></tr>")
            .Append("<tr><td colspan='3'><strong>AFP Integra</strong></td></tr>")
            .Append("<tr><td colspan='3'><strong>Grupo Integra</strong></td></tr></table>")
        End With
        Return mensaje.ToString
    End Function
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal mnemonico As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&vportafolio=" + ddlFondo.SelectedItem.Text + "&cportafolio=" + portafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
    End Sub
    Public Function ObtenerDatosOperacion() As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21", "c22", "v22"} 'HDG OT 62325 20110323
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = tbFechaOperacion.Text
        drGrilla("c2") = "Fecha Contrato"
        drGrilla("v2") = tbFechaFinContrato.Text
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = tbHoraOperacion.Text
        drGrilla("c4") = "Fecha Liquidación"
        drGrilla("v4") = tbFechaLiquidacion.Text
        drGrilla("c5") = "Tipo Cambio Spot"
        drGrilla("v5") = tbTcSpot.Text
        drGrilla("c6") = "Tipo Cambio Futuro"
        drGrilla("v6") = txtTcFuturo.Text
        drGrilla("c7") = "De"
        drGrilla("v7") = ddlMonedaOrigen.SelectedItem.Text
        drGrilla("c8") = "Monto Origen"
        drGrilla("v8") = tbMontoOrigen.Text
        drGrilla("c9") = "A"
        drGrilla("v9") = ddlMonedaDestino.SelectedItem.Text
        drGrilla("c10") = "Monto Futuro"
        drGrilla("v10") = txtMontoFuturo.Text
        drGrilla("c11") = "Plazo"
        drGrilla("v11") = txtPlazo.Text
        drGrilla("c12") = "Diferencial"
        drGrilla("v12") = 0 'txtDiferencial.Text
        If rbnDelivery.SelectedValue = "S" Then
            drGrilla("c13") = "Modalidad Compra"
            drGrilla("v13") = "Delivery"
        Else
            drGrilla("c13") = "Modalidad Compra"
            drGrilla("v13") = "Non-Delivery"
        End If
        drGrilla("c14") = "Intermediario"
        drGrilla("v14") = ddlIntermediario.SelectedItem.Text
        If ddlContacto.SelectedIndex <> 0 Then
            drGrilla("c15") = "Contacto"
            drGrilla("v15") = ddlContacto.SelectedItem.Text
        Else
            drGrilla("c15") = ""
            drGrilla("v15") = ""
        End If
        drGrilla("c16") = "Observación"
        drGrilla("v16") = txtObservacion.Text.ToUpper
        If tbNPoliza.Visible = True Then
            drGrilla("c17") = "Número Poliza"
            drGrilla("v17") = tbNPoliza.Text
        Else
            drGrilla("c17") = ""
            drGrilla("v17") = ""
        End If
        drGrilla("c18") = "Motivo"
        drGrilla("v18") = ddlMotivo.SelectedItem.Text
        drGrilla("c19") = ""
        drGrilla("v19") = ""
        drGrilla("c20") = ""
        drGrilla("v20") = ""
        drGrilla("c21") = ""
        drGrilla("v21") = ""
        drGrilla("c22") = "Cobertura"
        drGrilla("v22") = oOrdenInversionBM.SeleccionarTipoMonedaxMotivoForw(ddlMotivo.SelectedValue, ddlTipoMoneda.SelectedValue).Rows(0)("Descripcion")   'HDG OT 62325 20110323
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
            Case "MODIFICA"
                AlertaJS("Se Modificó la orden correctamente", "window.close()")
        End Select
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica el retorno de mensaje de confirmación | 06/06/18

    End Sub
    Public Function InsertarOrdenInversion() As String
        Dim strCodigoOI, strCodigoOI_T As String
        Dim oValoresBM As New ValoresBM
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
    Public Function ModificarOrdenInversion() As Boolean
        Dim bResult As Boolean = True
        oOrdenInversionBE = crearObjetoOI()
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
        Return bResult
    End Function
    Public Sub EliminarOrdenInversion()
        oOrdenInversionBM.EliminarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, ddlMotivoCambio.SelectedValue, DatosRequest)
        oImpComOP.Eliminar(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest)
    End Sub
    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(ddlFondo.SelectedValue, txtCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc, txtComentarios.Text, DatosRequest)
        txtComentarios.Text = ""
    End Sub
    Private Sub GenerarCustodioAfectado()
        Dim dtAux As DataTable
        Dim i As Integer
        dtAux = CType(Session("datosEntidad"), DataTable)
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        For i = 0 To dtAux.Rows.Count - 1
            If dtAux.Rows(i)("CodigoCustodio") = ddlIntermediario.SelectedValue Then
                Session("ValorCustodio") = dtAux.Rows(i)("CodigoCustodio") + strSeparador + "1"
                Exit For
            End If
        Next
    End Sub
    Public Function crearObjetoOI() As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        oOrdenInversionBM.InicializarOrdenInversion(oRow)
        oRow.CodigoOrden = txtCodigoOrden.Value
        oRow.MontoNominalOperacion = tbMontoOrigen.Text
        oRow.MontoNominalOrdenado = tbMontoOrigen.Text
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.CodigoMoneda = ddlMonedaOrigen.SelectedValue
        oRow.HoraOperacion = tbHoraOperacion.Text
        oRow.MontoCancelar = tbMontoOrigen.Text.Replace(".", UIUtility.DecimalSeparator)
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
        oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaFinContrato.Text)
        oRow.TipoCambioFuturo = txtTcFuturo.Text.Replace(".", UIUtility.DecimalSeparator)
        oRow.Plazo = txtPlazo.Text.Replace(".", UIUtility.DecimalSeparator)
        oRow.Situacion = "A"
        oRow.MontoPlazo = 0
        oRow.CodigoMonedaOrigen = ddlMonedaOrigen.SelectedValue
        oRow.CodigoMonedaDestino = ddlMonedaDestino.SelectedValue
        oRow.HoraOperacion = tbHoraOperacion.Text
        oRow.Delibery = rbnDelivery.SelectedValue
        oRow.MontoOperacion = txtMontoFuturo.Text.Replace(".", UIUtility.DecimalSeparator)
        oRow.Observacion = txtObservacion.Text.ToUpper
        oRow.TipoCambioSpot = tbTcSpot.Text.Replace(".", UIUtility.DecimalSeparator)
        oRow.Diferencial = 0
        oRow.CodigoMotivo = ddlMotivo.SelectedValue
        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        oRow.CategoriaInstrumento = "FD"    'UNICO POR TIPO
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
        If (chkFicticia.Checked) Then
            oRow.Ficticia = "S"
        Else
            oRow.Ficticia = "N"
        End If
        oRow.Renovacion = rbnRenovacion.SelectedValue
        oRow.TipoMonedaForw = ddlTipoMoneda.SelectedValue
        If (chkRegulaSBS.Checked) Then
            oRow.RegulaSBS = "S"
        Else
            oRow.RegulaSBS = "N"
        End If
        Return oOrdenInversionBE
    End Function
    Private Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos()
    End Sub
    Private Function obtenerTipoCambio(ByVal decTipoCambioOrigen As Decimal, ByVal strTipoCalculoOrigen As String, ByVal decTipoCambioDestino As Decimal, ByVal strTipoCalculoDestino As String) As Decimal
        If strTipoCalculoOrigen = "I" Then
            decTipoCambioOrigen = 1 / decTipoCambioOrigen
        End If
        If strTipoCalculoDestino = "I" Then
            decTipoCambioDestino = 1 / decTipoCambioDestino
        End If
        Return Format(Math.Round(decTipoCambioDestino / decTipoCambioOrigen, 4), "##,##0.0000000")
    End Function
    Private Sub obtieneTipoCambio()
        Dim oTipoCambioBM As New TipoCambioBM
        Dim dtAux As New DataTable
        Dim decTipoCambio As Decimal = 0.0
        If ddlMonedaOrigen.SelectedValue <> "" And ddlMonedaDestino.SelectedValue <> "" Then
            If ddlMonedaOrigen.SelectedValue <> ddlMonedaDestino.SelectedValue Then
                dtAux = oTipoCambioBM.SeleccionarTCOrigen_TCDestino(ddlMonedaOrigen.SelectedValue, ddlMonedaDestino.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString(), DatosRequest).Tables(0)
                If dtAux.Rows.Count = 2 Then
                    If CType(dtAux.Rows(0)(0), String) = ddlMonedaOrigen.SelectedValue Then
                        decTipoCambio = obtenerTipoCambio(dtAux.Rows(0)(2), dtAux.Rows(0)(3), dtAux.Rows(1)(2), dtAux.Rows(1)(3))
                    Else
                        decTipoCambio = obtenerTipoCambio(dtAux.Rows(1)(2), dtAux.Rows(1)(3), dtAux.Rows(0)(2), dtAux.Rows(0)(3))
                    End If
                ElseIf dtAux.Rows.Count = 1 Then
                    If CType(dtAux.Rows(0)(0), String) = ddlMonedaOrigen.SelectedValue Then
                        AlertaJS(ObtenerMensaje("CONF13"))
                    ElseIf CType(dtAux.Rows(0)(0), String) = ddlMonedaDestino.SelectedValue Then
                        AlertaJS(ObtenerMensaje("CONF12"))
                    End If
                Else
                    AlertaJS(ObtenerMensaje("CONF14"))
                End If
            Else
                decTipoCambio = "1.0000000"
            End If
        End If
        If decTipoCambio <> "" Then
            If tbMontoOrigen.Text <> "" Then
                txtMontoFuturo.Text = Format(Math.Round(tbMontoOrigen.Text.Replace(".", UIUtility.DecimalSeparator) * decTipoCambio.ToString.Replace(".", UIUtility.DecimalSeparator), 7), "##,##0.0000000")
            ElseIf txtMontoFuturo.Text <> "" Then
                tbMontoOrigen.Text = Format(Math.Round(txtMontoFuturo.Text.Replace(".", UIUtility.DecimalSeparator) / decTipoCambio.ToString.Replace(".", UIUtility.DecimalSeparator), 7), "##,##0.0000000")
            End If
        End If
    End Sub
    Private Sub CalculaImportes()
        If tbTcSpot.Text.Trim <> "" Then
            If ddlMonedaOrigen.SelectedValue = "NSOL" Then
                tbMontoOrigen.Text = Format(txtMontoFuturo.Text * tbTcSpot.Text, "##,##0.0000000")
                If (ObtenerTipoCalculo(ddlMonedaDestino.SelectedValue.ToString, "") = "I" Or ObtenerTipoCalculo("", ddlMonedaOrigen.SelectedValue.ToString) = "I") Then
                    tbMontoOrigen.Text = Format(txtMontoFuturo.Text / tbTcSpot.Text, "##,##0.0000000")
                End If
            ElseIf ddlMonedaDestino.SelectedValue = "NSOL" Then
                tbMontoOrigen.Text = Format(txtMontoFuturo.Text / tbTcSpot.Text, "##,##0.0000000")
                If (ObtenerTipoCalculo(ddlMonedaDestino.SelectedValue.ToString, "") = "I" Or ObtenerTipoCalculo("", ddlMonedaOrigen.SelectedValue.ToString) = "I") Then
                    tbMontoOrigen.Text = Format(txtMontoFuturo.Text * tbTcSpot.Text, "##,##0.0000000")
                End If
            End If
            tbTcSpot.Text = Format(tbTcSpot.Text, "##,##0.0000000")
        End If
        If txtTcFuturo.Text.Trim <> "" Then
            txtTcFuturo.Text = Format(txtTcFuturo.Text, "##,##0.0000000")
        End If
    End Sub
    Private Sub obtieneTipoCambio2()
        If txtMontoFuturo.Text.Trim = "" Then
            txtMontoFuturo.Text = "0.0000000"
        End If
        If tbMontoOrigen.Text.Trim = "" Then
            tbMontoOrigen.Text = "0.0000000"
        End If
        If txtTcFuturo.Text.Trim = "" Then
            txtTcFuturo.Text = "0.0000000"
        End If
        Dim dtAux As New DataTable
        Dim decTipoCambio As Decimal = 0.0
        Dim montoDestino As Decimal = 0.0
        Dim montoOrigen As Decimal = 0.0
        Dim tipoCalculo As String = ""
        If ddlMonedaOrigen.SelectedValue <> "" And ddlMonedaDestino.SelectedValue <> "" Then
            If ddlMonedaOrigen.SelectedValue <> ddlMonedaDestino.SelectedValue Then
                Dim tipoDI As String = ObtenerTipoCambioDI()
                If tipoDI = "D" Then
                    decTipoCambio = Decimal.Parse(txtTcFuturo.Text.Replace(",", ""))
                    montoOrigen = Decimal.Parse(tbMontoOrigen.Text.Replace(",", ""))
                    montoDestino = montoOrigen * decTipoCambio
                    txtMontoFuturo.Text = Format(Math.Round(montoDestino, 7), "##,##0.0000000")
                ElseIf tipoDI = "I" Then
                    decTipoCambio = Decimal.Parse(txtTcFuturo.Text.Replace(",", ""))
                    montoOrigen = Decimal.Parse(tbMontoOrigen.Text.Replace(",", ""))
                    montoDestino = montoOrigen / decTipoCambio
                    txtMontoFuturo.Text = Format(Math.Round(montoDestino, 7), "##,##0.0000000")
                End If
            Else
                AlertaJS("Las monedas negociadas deben ser diferentes")
            End If
        Else
            txtTcFuturo.Text = "1.0000000"
        End If
    End Sub
    Private Function ObtenerTipoCambioDI() As String
        Dim strResul As String = ""
        Dim objTipoCambioDI As New TipoCambioDI_BM
        Dim objDsAux As DataSet = objTipoCambioDI.SeleccionarPorFiltros("", ddlMonedaOrigen.SelectedValue, ddlMonedaDestino.SelectedValue, "", "A", DatosRequest)
        If objDsAux.Tables(0).Rows.Count > 0 Then
            If objDsAux.Tables(0).Rows.Count = 1 Then
                strResul = objDsAux.Tables(0).Rows(0)("Tipo").Substring(0, 1)
            Else
                AlertaJS("Existe error en la matriz de monedas (Directo e Indirecto)")
            End If
        Else
            AlertaJS("No se encuentra definido el tipo de cambio Directo o Indirecto para las dos monedas")
        End If
        Return strResul
    End Function
    Private Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        If validaFechas() = False Then
            Exit Sub
        End If
        obtieneTipoCambio2()
        Dim GUID As String = New Guid().ToString()
        'LIMITES
        If hdPagina.Value <> "TI" Then
            Dim oLimiteEvaluacion As New LimiteEvaluacionBM
            Dim dsAux As New DataSet
            Dim codigoOperacion As String = ddlOperacion.SelectedValue.ToString()
            Dim codigoNemonico As String = ""
            Dim cantidadValor As Decimal = 0
            Dim montoNominal As Decimal = IIf(tbMontoOrigen.Text = "", "0", tbMontoOrigen.Text)
            Dim codigoPortafolio As String = ddlFondo.SelectedValue.ToString()
            Dim fechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.ToString())
            dsAux = oLimiteEvaluacion.Evaluar(codigoOperacion, codigoNemonico, cantidadValor, montoNominal, codigoPortafolio, fechaOperacion, DatosRequest)
            Session(GUID) = dsAux
            ViewState("GUID_Limites") = GUID
            If (dsAux.Tables(0).Rows.Count > 0) Then
                Session("Instrumento") = "FORWARD DIVISAS"
                ViewState("EstadoOI") = "L-EXC"
                EjecutarJS(UIUtility.MostrarPopUp("frmConsultaLimitesInstrumento.aspx?GUID=" + GUID, "no", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
            Else
                ViewState("EstadoOI") = ""
            End If
        End If
        ''LIMITES
        If hdPagina.Value <> "CO" Then
            ObtieneCustodiosSaldos()
        End If
        CargarPaginaProcesar()
    End Sub
    Private Sub ObtieneCustodiosSaldos()
        'CUSTODIOS, CARTERA y KARDEX
        If Session("EstadoPantalla") = "Ingresar" Then
            GenerarCustodioAfectado()
        End If
    End Sub
    Private Function validaFechas() As Boolean
        If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text) Then
            AlertaJS(ObtenerMensaje("CONF7"))
            Return False
        Else
            Return True
        End If
    End Function
#Region " /* Métodos Personalizados (Popups Dialogs) */ "
    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal cfondo As String, ByVal operacion As String, ByVal moneda As String, ByVal fecha As String, ByVal accion As String)
        EjecutarJS("showModalDialog('../frmInversionesRealizadas.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&vFondo=" + fondo + "&cFondo=" + cfondo + "&vOperacion=" + operacion + "&vFechaOperacion=" + fecha + "&vAccion=" + accion + "&vCategoria=FD', '950','700','btnBuscar');")
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
        Dim dtAux As DataTable = oPortafolioBM.SeleccionarPortafolioPorFiltro(ddlFondo.SelectedValue, DatosRequest).Tables(0)
        If Not dtAux Is Nothing Then
            If dtAux.Rows.Count > 0 Then
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaConstitucion")))
            End If
        End If
        tbHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Traspaso"
        hdMensaje.Value = "el Ingreso"
        CargarPaginaIngresar()
        CargarIntermediario()
    End Sub
    Private Sub ControlarCamposEO_CO_XO()
        MostrarOcultarBotonesAcciones(False)
        btnAceptar.Enabled = True
    End Sub
    Private Sub CargarPaginaModificarEO_CO_XO(ByVal acceso As String)
        If acceso = "EO" Or acceso = "CO" Then
            CargarPaginaBuscar()
            HabilitaDeshabilitaCabecera(False)
            btnBuscar.Visible = False
            HabilitaDeshabilitaDatosOperacionComision(True)
            btnAceptar.Enabled = True
            Session("EstadoPantalla") = "Modificar"
        End If
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
        btnProcesar.Visible = True
        btnProcesar.Enabled = True
    End Sub
    Private Sub CargarPaginaIngresar()
        CargarPaginaBuscar()
        btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
    End Sub
    Private Sub CargarPaginaModificar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        lblComentarios.InnerText = "Comentarios modificación:"
        txtComentarios.Text = ""
    End Sub
    Private Sub CargarPaginaEliminar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
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
        CargarPaginaProcesar()
        btnAceptar.Enabled = False
    End Sub
    Private Sub CargarPaginaProcesar()
        Dim strJS As New StringBuilder
        btnAceptar.Enabled = True
        strJS.AppendLine("$('#btnAceptar').removeAttr('disabled');")
        If Session("EstadoPantalla") <> "Ingresar" Then
            btnImprimir.Visible = True
            btnImprimir.Enabled = True
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
        btnAceptar.Enabled = False
    End Sub
    Private Sub CargarPaginaAccion()
        CargarPaginaInicio()
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
        ddlMonedaOrigen.Enabled = estado
    End Sub
    Private Sub HabilitaDeshabilitaDatosOperacionComision(ByVal estado As Boolean)
        If estado Then
            imgFechaVcto.Attributes.Add("class", "input-append date")
            imgFechaContrato.Attributes.Add("class", "input-append date")
        Else
            imgFechaVcto.Attributes.Add("class", "input-append")
            imgFechaContrato.Attributes.Add("class", "input-append")
        End If
        tbHoraOperacion.ReadOnly = Not estado
        tbMontoOrigen.ReadOnly = Not estado
        ddlIntermediario.Enabled = estado
        ddlContacto.Enabled = estado
        tbFechaLiquidacion.ReadOnly = Not estado
        tbFechaFinContrato.ReadOnly = Not estado
        txtTcFuturo.ReadOnly = Not estado
        txtPlazo.ReadOnly = Not estado
        rbnDelivery.Enabled = estado
        txtMontoFuturo.ReadOnly = Not estado
        ddlMonedaDestino.Enabled = estado
        txtObservacion.ReadOnly = Not estado
        tbTcSpot.ReadOnly = Not estado
        ddlMotivo.Enabled = estado
        ddlTipoMoneda.Enabled = estado
        If ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
            chkRegulaSBS.Enabled = False
        Else
            chkRegulaSBS.Enabled = estado
        End If
    End Sub
    Private Sub OcultarBotonesInicio()
        btnBuscar.Visible = False
        btnProcesar.Visible = False
        btnImprimir.Visible = False
    End Sub
    Private Sub OcultarDeshabilitaInicioSwap()
        ddlFondo.Enabled = False
        ddlOperacion.Enabled = False
        btnIngresar.Visible = False
        btnModificar.Visible = False
        btnEliminar.Visible = False
        btnIngresar.Visible = False
        btnConsultar.Visible = False
    End Sub
    Private Sub MostrarOcultarBotonesAcciones(ByVal estado As Boolean)
        btnIngresar.Visible = estado
        btnModificar.Visible = estado
        btnEliminar.Visible = estado
        btnConsultar.Visible = estado
    End Sub
    Private Sub LimpiarDatosOperacion()
        ddlFondo.SelectedIndex = 0
        ddlMonedaOrigen.SelectedIndex = 0
        ddlOperacion.SelectedIndex = 0
        ddlMonedaDestino.SelectedIndex = 0
        tbHoraOperacion.Text = ""
        tbMontoOrigen.Text = ""
        ddlIntermediario.SelectedIndex = 0
        CargarContactos()
        ddlContacto.SelectedIndex = 0
        tbFechaOperacion.Text = ""
        tbFechaLiquidacion.Text = ""
        tbFechaFinContrato.Text = ""
        txtTcFuturo.Text = ""
        txtPlazo.Text = ""
        rbnDelivery.Items(0).Selected = True
        txtMontoFuturo.Text = ""
        txtObservacion.Text = ""
        tbHoraOperacion.Text = ""
        tbTcSpot.Text = ""
        ddlMotivo.SelectedIndex = 0
        txtCodigoOrden.Value = ""
        CargarTipoMoneda()
        ddlTipoMoneda.SelectedIndex = 0
    End Sub
#End Region
    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(txtCodigoOrden.Value, ddlFondo.SelectedValue, "OPERACIONES DERIVADAS - FORWARD DIVISAS", ddlOperacion.SelectedItem.Text, ddlMonedaOrigen.SelectedValue, hdNemonico.Value)
    End Sub
    Private moneda As String = String.Empty
    Private Sub CargarFechaVencimiento()
        Dim dtAux As DataTable = objPortafolio.SeleccionarPortafolioPorFiltro(Me.ddlFondo.SelectedValue, DatosRequest).Tables(0)
        If Not dtAux Is Nothing Then
            If dtAux.Rows.Count > 0 Then
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))

                'INICIO | ZOLUXIONES | DACV | SPINT - III'
                moneda = dtAux(0).Item("CodigoMoneda").ToString()
                If ddlOperacion.Items.Count > 0 Then
                    Select Case moneda.Trim
                        Case "DOL"
                            ddlOperacion.SelectedValue = "93"
                        Case "NSOL"
                            ddlOperacion.SelectedValue = "94"
                        Case Else
                    End Select

                End If
                'FIN | ZOLUXIONES | DACV | SPINT - III'

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
        Else
            If Session("EstadoPantalla") = "Ingresar" Then
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
            End If
            chkFicticia.Enabled = False
            If ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
                chkFicticia.Checked = False
            End If
            If (Not Session("EstadoPantalla") Is Nothing And Not Session("Procesar") Is Nothing) Then
                If Session("EstadoPantalla") = "Ingresar" And ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS And Session("Procesar") = "0" Then
                    chkFicticia.Enabled = True
                End If
            End If
        End If
    End Sub
    Private Sub txtPlazo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPlazo.TextChanged
        If (tbFechaOperacion.Text <> "") And (txtPlazo.Text <> "") Then
            Try
                Dim Fecha As Date
                Fecha = tbFechaOperacion.Text.Trim()
                Fecha = Fecha.AddDays(ToNullDecimal(txtPlazo.Text.Trim()))
                tbFechaFinContrato.Text = Fecha.ToShortDateString
                tbFechaLiquidacion.Text = Fecha.ToShortDateString 'Agregado LC 20080902
            Catch ex As Exception
                AlertaJS("La fecha de la Operacion a Plazo cae. Debe cambiar los días de plazo Reporte.")
            End Try

        Else
            tbFechaFinContrato.Text = ""
        End If
    End Sub
    Private Sub ddlMonedaDestino_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMonedaDestino.SelectedIndexChanged
        ObtenerTipoCalculo(ddlMonedaDestino.SelectedValue.ToString, "")
    End Sub
    Private Sub ddlMonedaOrigen_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMonedaOrigen.SelectedIndexChanged
        ObtenerTipoCalculo("", ddlMonedaOrigen.SelectedValue.ToString)
    End Sub
    Private Sub ddlMotivo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMotivo.SelectedIndexChanged
        CargarTipoMoneda()
    End Sub
    Private Function ObtenerTipoCalculo(ByVal strCodigoMonedaDestino As String, ByVal strCodigoMonedaOrigen As String) As String
        Dim oMonedaBM As New MonedaBM
        Dim strTipoCalculo As String
        If strCodigoMonedaDestino <> "" Then
            strTipoCalculo = oMonedaBM.SeleccionarPorFiltro(strCodigoMonedaDestino, "", "", "", "", DatosRequest).Moneda.Item(0).TipoCalculo.ToString
        Else
            strTipoCalculo = oMonedaBM.SeleccionarPorFiltro(strCodigoMonedaOrigen, "", "", "", "", DatosRequest).Moneda.Item(0).TipoCalculo.ToString
        End If
        Return strTipoCalculo
    End Function
    Public Sub LimitesParametrizadosPDF(ByVal ruta As String)
        Try
            Dim destinoPdf As String, nombreNuevoArchivo As String
            Dim PrefijoFolder As String = "LimitesParametrizadosPDF_"
            Dim fechaActual As String = System.DateTime.Now.ToString("yyyyMMdd")
            Dim sRutaTemp As String = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")    'HDG OT 67554 duplicado
            Dim foldersAsesoria() As String = Directory.GetDirectories(sRutaTemp, PrefijoFolder & "*")  'HDG OT 67554 duplicado
            Dim folderActual As String = sRutaTemp & PrefijoFolder & fechaActual  'HDG OT 67554 duplicado
            Dim cont As Integer
            Try
                For cont = 0 To foldersAsesoria.Length - 1
                    If Not foldersAsesoria(cont).Equals(folderActual) Then
                        Try
                            Directory.Delete(foldersAsesoria(cont), True)
                        Catch ex As Exception
                        End Try
                    End If
                Next
                If Not Directory.Exists(folderActual) Then
                    Directory.CreateDirectory(folderActual)
                End If
                nombreNuevoArchivo = System.Guid.NewGuid().ToString() & ".pdf"
                destinoPdf = folderActual & "\" & nombreNuevoArchivo
                Dim sourceFiles() As String = ruta.Substring(1).Split("&")
                Dim f As Integer = 0
                Dim reader As PdfReader = New PdfReader(sourceFiles(f))
                Dim n As Integer = reader.NumberOfPages
                Dim document As iTextSharp.text.Document = New iTextSharp.text.Document(reader.GetPageSizeWithRotation(1))
                Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(destinoPdf, FileMode.Create))
                document.Open()
                Dim cb As PdfContentByte = writer.DirectContent
                Dim page As PdfImportedPage
                Dim rotation As Integer
                While (f < sourceFiles.Length)
                    Dim i As Integer = 0
                    While (i < n)
                        i += 1
                        document.SetPageSize(reader.GetPageSizeWithRotation(i))
                        document.NewPage()
                        page = writer.GetImportedPage(reader, i)
                        rotation = reader.GetPageRotation(i)
                        If rotation = 90 Or rotation = 270 Then
                            cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                        Else
                            cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
                        End If
                    End While
                    f += 1
                    If f < sourceFiles.Length Then
                        reader = New PdfReader(sourceFiles(f))
                        n = reader.NumberOfPages
                    End If
                End While
                document.Close()
                Dim sfile As String
                sfile = folderActual & "\" & nombreNuevoArchivo
                EjecutarJS("<script>window.open('" & sfile.Replace("\", "\\") & "')</script>", False)
                For Each savedDoc As String In ruta.Split(New Char() {"&"})
                    If File.Exists(savedDoc) Then
                        File.Delete(savedDoc)
                    End If
                Next
            Catch ex As Exception
                UIUtility.PublicarEvento("LimitesParametrizadosPDF - ex.Message = " & ex.Message & " ++ ex.StackTrace = " & ex.StackTrace)
                For Each savedDoc As String In ruta.Split(New Char() {"&"})
                    If File.Exists(savedDoc) Then
                        File.Delete(savedDoc)
                    End If
                Next
            Finally
            End Try
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Public Function ValidarForward(ByVal oOrdenPreOrdenInversion As OrdenPreOrdenInversionBE) As Boolean
        Dim validaForward As Boolean = False
        Dim oEntidad As New OrdenPreOrdenInversionBE
        Dim codigoTercero, codigoPais As String
        Dim fechaLiquidacion As Decimal = 0
        Dim oTerceros As New TercerosBM
        Dim oTercerosBE As New TercerosBE
        oEntidad = oOrdenPreOrdenInversion
        codigoPais = ""
        codigoTercero = ""
        fechaLiquidacion = CType(oEntidad.OrdenPreOrdenInversion.Rows(0)("FechaLiquidacion"), Decimal)
        If oEntidad.OrdenPreOrdenInversion.Rows(0)("CategoriaInstrumento") = "FD" Then
            codigoTercero = oEntidad.OrdenPreOrdenInversion.Rows(0)("CodigoTercero")
            oTercerosBE = oTerceros.SeleccionarPorFiltroActivo(codigoTercero, "", "", "", "", "A", DatosRequest)
            If oTercerosBE.Terceros.Rows.Count > 0 Then
                codigoPais = oTercerosBE.Terceros.Rows(0)("CodigoPais")
            End If
            If codigoPais <> "604" And fechaLiquidacion > 20130810 Then
                validaForward = True
            End If
        End If
        Return validaForward
    End Function
    Sub ValTipoFor()
        If rbnDelivery.SelectedValue = "N" Then
            tbTcSpot.Enabled = False
        Else
            tbTcSpot.Enabled = True
        End If
    End Sub
    Protected Sub rbnDelivery_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles rbnDelivery.SelectedIndexChanged
        ValTipoFor()
    End Sub
End Class
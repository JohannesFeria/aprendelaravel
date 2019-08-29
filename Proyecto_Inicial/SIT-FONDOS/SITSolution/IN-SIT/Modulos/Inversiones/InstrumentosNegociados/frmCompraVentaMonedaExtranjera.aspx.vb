Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports System.IO
Imports iTextSharp.text.pdf
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmCompraVentaMonedaExtranjera
    Inherits BasePage
    Dim objutil As New UtilDM
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
    Dim oPortafolioBM As New PortafolioBM
    Dim oValoresBM As New ValoresBM
    Dim oTipoCambioBM As New TipoCambioBM
    Dim sOISwap As String
    Dim report As CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Session("SS_DatosModal") Is Nothing Then
            ddlFondo.SelectedValue = CType(Session("SS_DatosModal"), String())(3).ToString.Trim()
            ddlOperacion.SelectedValue = CType(Session("SS_DatosModal"), String())(4).ToString.Trim()
            txtCodigoOrden.Value = CType(Session("SS_DatosModal"), String())(6).ToString.Trim()
            Session.Remove("SS_DatosModal")
        End If
        If Request.QueryString("PTNegSim") = "M" Then
            trMotivoCambio.Attributes.Remove("class")
            trMotivoCambio.Attributes.Add("class", "row")
        End If
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        hdSaldo.Value = 0
        '  btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
        btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
        If Not Page.IsPostBack Then
            Dim par As New ParametrosGeneralesBM
            ddlAfectaFlujoCaja.DataSource = par.SeleccionarPorFiltro("Desicion", "", "", "", Nothing)
            ddlAfectaFlujoCaja.DataTextField = "Nombre"
            ddlAfectaFlujoCaja.DataValueField = "Valor"
            ddlAfectaFlujoCaja.DataBind()
            ddlAfectaFlujoCaja.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
            btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
            btnProcesar.Attributes.Add("onclick", "javascript:return Validar();")
            Try
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                hdRptaConfirmar.Value = "NO"
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
                LimpiarSesiones()
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    hdPagina.Value = Request.QueryString("PTNeg")
                End If
                If (hdPagina.Value = "TI") Then
                    UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
                Else
                    UIUtility.CargarOperacionOI(ddlOperacion, "CVME")
                End If
                If Not Request.QueryString("PTNegSim") Is Nothing Then
                    hdPagina.Value = "SW"
                End If
                UIUtility.CargarMonedaOI(ddlMoneda)
                UIUtility.CargarMonedaOI(ddlMonedaDestino)
                UIUtility.CargarIntermediariosOISoloBancos(ddlIntermediario)
                CargarPaginaInicio()
                hdPagina.Value = ""
                If Not Request.QueryString("PTNeg") Is Nothing Then
                    UIUtility.CargarPortafoliosOI(ddlFondo)
                    hdPagina.Value = Request.QueryString("PTNeg")
                    If hdPagina.Value = "TI" Then
                        ddlFondo.SelectedValue = Request.QueryString("PTFondoO")
                        ddlFondoDestino.SelectedValue = Request.QueryString("PTFondoD")
                        ddlMoneda.SelectedValue = CType(Session("MonedaOrigen"), String)

                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                        hdCustodio.Value = Request.QueryString("PTCustodio")
                        hdSaldo.Value = Request.QueryString("PTSaldo")
                        ControlarCamposTI()
                    Else
                        txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                        ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                        If (hdPagina.Value = "EO") Or (hdPagina.Value = "CO") Or (hdPagina.Value = "XO") Then 'Viene de la Pagina Confirmacion , Ejecucion  o Extorno de Ordenes de Inversion
                            CargarDatosOrdenInversion()
                            ControlarCamposEO_CO_XO()
                            tbNPoliza.Text = Right(txtCodigoOrden.Value, 4)
                            CargarPaginaModificarEO_CO_XO(hdPagina.Value)
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                            If hdPagina.Value = "CO" Then
                                btnAceptar.Text = "Grabar y Confirmar"
                                If Session("ValidarFecha").ToString = "FECHADIFERENTE" Then CargarPaginaInicio()
                            End If
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                        Else
                            If (hdPagina.Value = "OE") Then 'Viene de la Pagina Ordenes Excedidas
                                ControlarCamposOE()
                            Else
                                If (hdPagina.Value = "DA") Then 'Viene de la Pagina Negociacion Dias Anteriores
                                    ViewState("ORDEN") = "OI-DA"
                                    tbFechaOperacion.Text = Request.QueryString("Fecha")
                                    tbFechaOperacion.ReadOnly = True
                                    imgFechaOperacion.Attributes.Add("class", "input-append")
                                Else
                                    If (hdPagina.Value = "CP") Then 'Viene de la Pagina Liquidaciones Cuentas Por Pagar
                                        Call ConfiguraModoConsulta()
                                        ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                        txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                        ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                        Call CargarDatosOrdenInversion()
                                        Call HabilitaBotones(False, False, False, False, False, False, False, False, False, True, False)
                                    End If
                                    '-----------------------------------
                                End If
                            End If
                        End If
                    End If
                    btnSalir.Attributes.Remove("onClick")
                    '   btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF49", txtCodigoOrden.Value + "?", "SI")
                Else
                    HelpCombo.PortafolioCodigoListar(ddlFondo, PORTAFOLIO_MULTIFONDOS)
                    If Not Request.QueryString("PTNegSim") Is Nothing Then
                        Dim sAcc As String
                        hdPagina.Value = "SW"
                        sAcc = Request.QueryString("PTNegSim")
                        If (sAcc = "I") Then
                            ConfiguraModoInserta()
                        ElseIf (sAcc = "M") Then
                            ConfiguraModoModifica()
                            CargarPaginaModificar()
                        ElseIf (sAcc = "E") Then
                            ConfiguraModoElimina()
                            CargarPaginaEliminar()
                        ElseIf (sAcc = "C") Then
                            ConfiguraModoConsulta()
                            CargarPaginaConsultar()
                        End If
                        btnSalir.Attributes.Remove("onClick")
                    End If
                End If
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        End If
        imgFechaOperacion.Attributes.Add("class", "input-append")
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Call HabilitaBotones(True, True, True, True, True, True, True, True, True, False, True)
    End Sub
    Private Sub HabilitaBotones(ByVal bIngresar As Boolean, _
                                ByVal bModificar As Boolean, ByVal bEliminar As Boolean, ByVal bConsultar As Boolean, _
                                ByVal bAsignar As Boolean, ByVal bImprimir As Boolean, _
                                ByVal bAceptar As Boolean, ByVal bBuscar As Boolean, ByVal bSalir As Boolean, _
                                ByVal bRetornar As Boolean, ByVal bLimitesParametrizados As Boolean)
        btnIngresar.Visible = bIngresar
        btnModificar.Visible = bModificar
        btnEliminar.Visible = bEliminar
        btnConsultar.Visible = bConsultar
        btnImprimir.Visible = bImprimir
        btnAceptar.Visible = bAceptar
        btnBuscar.Visible = bBuscar
        btnSalir.Visible = bSalir
        btnRetornar.Visible = bRetornar
    End Sub
    Private Sub LimpiarSesiones()
        Session("Procesar") = Nothing
        Session("ValorCustodio") = Nothing
        Session("EstadoPantalla") = Nothing
        Session("Busqueda") = Nothing
        Session("CodigoMoneda") = Nothing
        Session("datosEntidad") = Nothing
        Session("dtdatosoperacion") = Nothing
    End Sub
    Public Function ObtenerDatosOperacion() As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operacion"
        drGrilla("v1") = tbFechaOperacion.Text
        drGrilla("c2") = "Fecha Liquidacion"
        drGrilla("v2") = tbFechaLiquidacion.Text
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = tbHoraOperacion.Text
        drGrilla("c4") = "De :"
        drGrilla("v4") = ddlMoneda.SelectedItem.Text
        drGrilla("c5") = "Monto Divisa Negociada"
        drGrilla("v5") = txtMontoOrigen.Text
        drGrilla("c6") = "A :"
        drGrilla("v6") = ddlMonedaDestino.SelectedItem.Text
        drGrilla("c7") = "Monto"
        drGrilla("v7") = txtMontoDestino.Text
        drGrilla("c8") = "Tipo Cambio"
        drGrilla("v8") = txtTipoCambio.Text
        drGrilla("c9") = "Intermediario"
        drGrilla("v9") = ddlIntermediario.SelectedItem.Text
        If ddlContacto.SelectedIndex <> 0 Then
            drGrilla("c10") = "Contacto"
            drGrilla("v10") = ddlContacto.SelectedItem.Text
        Else
            drGrilla("c10") = ""
            drGrilla("v10") = ""
        End If
        If tbNPoliza.Visible = True Then
            drGrilla("c11") = "Número Poliza"
            drGrilla("v11") = tbNPoliza.Text
        Else
            drGrilla("c11") = ""
            drGrilla("v11") = ""
        End If
        drGrilla("c12") = "Observación"
        drGrilla("v12") = txtObservacion.Text.ToUpper
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
        drGrilla("c19") = ""
        drGrilla("v19") = ""
        drGrilla("c20") = ""
        drGrilla("v20") = ""
        drGrilla("c21") = ""
        drGrilla("v21") = ""
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If Session("EstadoPantalla") = "Ingresar" Then
            'NO EXISTE BOTON BUSCAR PARA CUANDO ES "INGRESAR"
        Else
            If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Or Session("EstadoPantalla") = "Consultar" Then
                If Session("Busqueda") = 0 Then
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
                    ShowDialogPopupInversionesRealizadas(ddlFondo.SelectedValue, ddlOperacion.SelectedValue, ddlMoneda.SelectedValue.ToString, strAux, strAccion, ddlFondo.SelectedItem.Text)
                    txtCodigoOrden.Value = ""
                    Session("Busqueda") = 2
                Else
                    If Session("Busqueda") = 1 Then
                        CargarDatosOrdenInversion()
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
    End Sub
    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        If Not validaFechas() Then
            Exit Sub
        End If
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Para añadir validación en botón Aceptar cuando la acción es Ingresar | 07/06/18 
        If Session("EstadoPantalla") = "Ingresar" Then UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Para añadir validación en botón Aceptar cuando la acción es Ingresar | 07/06/18 
        obtieneTipoCambio2()
        CargarPaginaProcesar()
    End Sub
    Private Sub btnVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Session("prevPag") = "../frmInstrumentosNegociados/CompraVentaMonedaExtranjera.aspx"
        Response.Redirect("../Reportes/frmVisorOrdenesDeInversion.aspx?titulo=Orden de Inversión - COMPRA / VENTA DE MONEDA EXTRANJERA")
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
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de la orden de inversión de Compra / Venta de moneda extranjera?"
                    Else
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de pre-orden de inversión de Compra / Venta de moneda extranjera?"
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
        Call ConfiguraModoInserta()
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        LimpiarSesiones()
        Call ConfiguraModoModifica()
    End Sub
    Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        LimpiarSesiones()
        Call ConfiguraModoElimina()
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        LimpiarSesiones()
        Call ConfiguraModoConsulta()
    End Sub
    Private Sub ConfiguraModoInserta()
        UIUtility.ExcluirOtroElementoSeleccion(ddlFondo)
        UIUtility.ExcluirOtroElementoSeleccion(ddlOperacion)
        CargarPaginaAccion()
        Session("EstadoPantalla") = "Ingresar"
        lblAccion.Text = "Ingresar"
        CargarPaginaIngresar()
        hdMensaje.Value = "el Ingreso"
        If Not ddlFondo.Items.FindByValue(PORTAFOLIO_MULTIFONDOS) Is Nothing Then
            ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS
        End If
        lblTitulo.Text = "PreOrden de Inversión - COMPRA / VENTA DE MONEDA EXTRANJERA"
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
                ddlMoneda.SelectedValue = "DOL"
                ddlMonedaDestino.SelectedValue = "NSOL"
                ddlMoneda.Enabled = False
                ddlMonedaDestino.Enabled = False
                sOISwap = Request.QueryString("OI")
                If sOISwap = "2" Then
                    txtMontoOrigen.Text = Session("MontoOperacion")
                    ddlIntermediario.SelectedValue = Session("Intermediario")
                    txtMontoOrigen.Enabled = False
                    ddlIntermediario.Enabled = False
                    CargarContactos()
                    If ddlContacto.Items.Count > 0 Then ddlContacto.SelectedValue = Session("Contacto")
                End If
            End If
        End If
        tbFechaLiquidacion.Text = tbFechaOperacion.Text
        tbHoraOperacion.Text = objutil.RetornarHoraSistema
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
            CargarDatosOrdenInversion()
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
            CargarDatosOrdenInversion()
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
            CargarDatosOrdenInversion()
            CargarPaginaConsultar()
        End If
    End Sub
    Private Sub obtieneTipoCambio()
        If txtMontoDestino.Text.Trim = "" Then
            txtMontoDestino.Text = "0.0000000"
        End If
        If txtMontoOrigen.Text.Trim = "" Then
            txtMontoOrigen.Text = "0.0000000"
        End If
        If txtTipoCambio.Text.Trim = "" Then
            txtTipoCambio.Text = "0.0000000"
        End If
        Dim dtAux As New DataTable
        Dim decTipoCambio As Decimal = 0.0
        Dim montoDestino As Decimal = 0.0
        Dim montoOrigen As Decimal = 0.0
        Dim tipoCalculo As String = ""
        If ddlMoneda.SelectedValue <> "" And ddlMonedaDestino.SelectedValue <> "" Then
            If ddlMoneda.SelectedValue <> ddlMonedaDestino.SelectedValue Then
                dtAux = oTipoCambioBM.SeleccionarTCOrigen_TCDestino(ddlMoneda.SelectedValue, ddlMonedaDestino.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString(), DatosRequest).Tables(0)
                If dtAux.Rows.Count = 2 Then
                    If (txtMontoDestino.Text <> "0.0000000" And txtMontoOrigen.Text = "0.0000000") Then    'primer sentido
                        decTipoCambio = Decimal.Parse(txtTipoCambio.Text.Replace(",", ""))
                        montoDestino = Decimal.Parse(txtMontoDestino.Text.Replace(",", ""))
                        Dim dr As DataRow
                        For Each dr In dtAux.Rows
                            If (CType(dr("codigoMoneda"), String) = ddlMoneda.SelectedValue) Then
                                tipoCalculo = CType(dr("TipoCalculo"), String)
                                If (tipoCalculo.Equals("I")) Then
                                    montoOrigen = montoDestino * decTipoCambio
                                Else
                                    montoOrigen = montoDestino / decTipoCambio
                                End If
                            End If
                        Next
                        txtMontoOrigen.Text = Format(Math.Round(montoOrigen, 7), "##,##0.0000000")
                    Else
                        If (txtMontoDestino.Text = "0.0000000" And txtMontoOrigen.Text <> "0.0000000") Then  ' segudo sentido
                            decTipoCambio = Decimal.Parse(txtTipoCambio.Text.Replace(",", ""))
                            montoOrigen = Decimal.Parse(txtMontoOrigen.Text.Replace(",", ""))
                            Dim dr As DataRow
                            For Each dr In dtAux.Rows
                                If (CType(dr("codigoMoneda"), String) = ddlMoneda.SelectedValue) Then
                                    tipoCalculo = CType(dr("TipoCalculo"), String)
                                    If (tipoCalculo.Equals("I")) Then
                                        montoDestino = montoOrigen / decTipoCambio
                                    Else
                                        montoDestino = montoOrigen * decTipoCambio
                                    End If
                                End If
                            Next
                            txtMontoDestino.Text = Format(Math.Round(montoDestino, 7), "##,##0.0000000")
                        End If
                    End If
                ElseIf dtAux.Rows.Count = 1 Then
                    If CType(dtAux.Rows(0)(0), String) = ddlMoneda.SelectedValue Then
                        AlertaJS(ObtenerMensaje("CONF13"))
                    ElseIf CType(dtAux.Rows(0)(0), String) = ddlMonedaDestino.SelectedValue Then
                        AlertaJS(ObtenerMensaje("CONF12"))
                    End If
                Else
                    AlertaJS(ObtenerMensaje("CONF14"))
                End If
            Else
                txtTipoCambio.Text = "1.0000000"
            End If
        Else
            txtTipoCambio.Text = ""
        End If
    End Sub
    Private Sub obtieneTipoCambio2()
        If txtMontoDestino.Text.Trim = "" Then
            txtMontoDestino.Text = "0.0000000"
        End If
        If txtMontoOrigen.Text.Trim = "" Then
            txtMontoOrigen.Text = "0.0000000"
        End If
        If txtTipoCambio.Text.Trim = "" Then
            txtTipoCambio.Text = "0.0000000"
        End If
        Dim dtAux As New DataTable
        Dim decTipoCambio As Decimal = 0.0
        Dim montoDestino As Decimal = 0.0
        Dim montoOrigen As Decimal = 0.0
        Dim tipoCalculo As String = ""
        If ddlMoneda.SelectedValue <> "" And ddlMonedaDestino.SelectedValue <> "" Then
            If ddlMoneda.SelectedValue <> ddlMonedaDestino.SelectedValue Then
                Dim tipoDI As String = ObtenerTipoCambioDI()
                If tipoDI = "D" Then
                    decTipoCambio = Decimal.Parse(txtTipoCambio.Text.Replace(",", ""))
                    montoOrigen = Decimal.Parse(txtMontoOrigen.Text.Replace(",", ""))
                    montoDestino = montoOrigen * decTipoCambio
                    txtMontoDestino.Text = Format(Math.Round(montoDestino, 7), "##,##0.0000000")
                ElseIf tipoDI = "I" Then
                    decTipoCambio = Decimal.Parse(txtTipoCambio.Text.Replace(",", ""))
                    montoOrigen = Decimal.Parse(txtMontoOrigen.Text.Replace(",", ""))
                    montoDestino = montoOrigen / decTipoCambio
                    txtMontoDestino.Text = Format(Math.Round(montoDestino, 7), "##,##0.0000000")
                End If
            Else
                AlertaJS(ObtenerMensaje("CONF14"))
            End If
        Else
            txtTipoCambio.Text = "1.0000000"
        End If
    End Sub
    Private Function ObtenerTipoCambioDI() As String
        Dim strResul As String = ""
        Dim objTipoCambioDI As New TipoCambioDI_BM
        Dim objDsAux As DataSet = objTipoCambioDI.SeleccionarPorFiltros("", ddlMoneda.SelectedValue, ddlMonedaDestino.SelectedValue, "", "A", DatosRequest)
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
    Private Function obtenerTipoCambio(ByVal decTipoCambioOrigen As Decimal, ByVal strTipoCalculoOrigen As String, ByVal decTipoCambioDestino As Decimal, ByVal strTipoCalculoDestino As String) As Decimal
        If strTipoCalculoOrigen = "I" Then
            decTipoCambioOrigen = 1 / decTipoCambioOrigen
        End If
        If strTipoCalculoDestino = "I" Then
            decTipoCambioDestino = 1 / decTipoCambioDestino
        End If
        Return Format(Math.Round(decTipoCambioDestino / decTipoCambioOrigen, 4), "##,##0.0000000")
    End Function
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        Dim accionRpta As String = String.Empty
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        Try
            If (ddlIntermediario.SelectedIndex <= 0) Then
                AlertaJS("Debe seleccionar el intermediario")
                Exit Sub
            End If
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
            If hdRptaConfirmar.Value.ToUpper = "SI" Then
                EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
                If hdPagina.Value <> "" And hdPagina.Value <> "DA" And hdPagina.Value <> "TI" And hdPagina.Value <> "SW" Then
                    If hdPagina.Value = "EO" Or hdPagina.Value = "CO" Then
                        If validaFechas() = True Then
                            ModificarOrdenInversion()
                            Session("Modificar") = 0
                            CargarPaginaAceptar()
                        End If
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
                                ReturnArgumentShowDialogPopup()
                            End If
                        End If
                    End If
                Else
                    If hdPagina.Value = "" Or hdPagina.Value = "TI" Or hdPagina.Value = "DA" Then
                        If Session("EstadoPantalla") = "Ingresar" Then
                            If validaFechas() = True Then
                                Dim strcodigoOrden As String
                                strcodigoOrden = InsertarOrdenInversion()
                                oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, ddlFondo.SelectedValue, "", DatosRequest)
                                txtCodigoOrden.Value = strcodigoOrden
                                Session("dtdatosoperacion") = ObtenerDatosOperacion()
                                GenerarLlamado(strcodigoOrden, ddlFondo.SelectedValue, "COMPRA/VENTA MONEDA EXTRANJERA", ddlOperacion.SelectedItem.Text, ddlMoneda.SelectedItem.Value, "", ddlFondo.SelectedItem.Text)
                                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                                accionRpta = "Ingresó"
                                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                                CargarPaginaAceptar()
                            End If
                        Else
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
                            If Session("EstadoPantalla") = "Modificar" Then
                                If validaFechas() = True Then
                                    ModificarOrdenInversion()
                                    FechaEliminarModificarOI("M")
                                    Session("Modificar") = 0
                                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
                                    accionRpta = "Modificó"
                                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
                                    CargarPaginaAceptar()
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
                            If txtComentarios.Text.Trim.Length <= 0 Then
                                strAlerta += "-Ingrese los comentarios por el cual desea " & Session("EstadoPantalla") & " esta operación."
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
                        Dim script As New StringBuilder
                        With script
                            .Append("<script>")
                            .Append("   window.close();")
                            .Append("</script>")
                        End With
                        EjecutarJS(script.ToString(), False)
                    End If
                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                    If (Session("EstadoPantalla") = "Eliminar" Or (Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Ingresar")) Then retornarMensajeAccion(accionRpta)
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Function validaFechas() As Boolean
        If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text) Then
            AlertaJS("La Fecha de Liquidación no puede ser menor a fecha de operación")
            Return False
        Else
            Return True
        End If
    End Function
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal mnemonico As String, ByVal Descripcion As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&cportafolio=" + portafolio + "&vportafolio=" + Descripcion + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
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
    Public Function InsertarOrdenInversion() As String
        Dim strCodigoOI As String
        oOrdenInversionBE = crearObjetoOI()
        strCodigoOI = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, "", DatosRequest)
        Return strCodigoOI
    End Function
    Public Sub ModificarOrdenInversion()
        oOrdenInversionBE = crearObjetoOI()
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, hdPagina.Value, "", DatosRequest)
    End Sub
    Public Sub EliminarOrdenInversion()
        oOrdenInversionBM.EliminarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, ddlMotivoCambio.SelectedValue, DatosRequest)
    End Sub
    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(ddlFondo.SelectedValue, txtCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc, txtComentarios.Text, DatosRequest)
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
        oRow.CodigoMoneda = ddlMoneda.SelectedValue
        oRow.Delibery = rblspot.SelectedValue
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
        oRow.HoraOperacion = tbHoraOperacion.Text
        oRow.Situacion = "A"
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.TipoCambio = Convert.ToDecimal(txtTipoCambio.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.CodigoMonedaDestino = ddlMonedaDestino.SelectedValue
        oRow.Observacion = txtObservacion.Text.ToUpper
        oRow.MontoOrigen = Convert.ToDecimal(txtMontoOrigen.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.MontoDestino = Convert.ToDecimal(txtMontoDestino.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.MontoOperacion = Convert.ToDecimal(txtMontoOrigen.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.MontoNetoOperacion = Convert.ToDecimal(txtMontoOrigen.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.CategoriaInstrumento = "CV"
        oRow.AfectaFlujoCaja = ddlAfectaFlujoCaja.SelectedValue
        Dim oParamGen As New ParametrosGeneralesBM
        oRow.CodigoMnemonico = oParamGen.ListarCompraVentaME_Mnemonico(DatosRequest).Rows(0)("Valor")
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
        If (chkRegulaSBS.Checked) Then
            oRow.RegulaSBS = "S"
        Else
            oRow.RegulaSBS = "N"
        End If
        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
    Private Sub CargarDatosOrdenInversion()
        Try
            oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, PORTAFOLIO_MULTIFONDOS)
            Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
            oRow = oOrdenInversionBE.Tables(0).Rows(0)
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
            txtCodigoOrden.Value = oRow.CodigoOrden
            If oRow.CodigoOperacion.ToString <> "" Then
                ddlOperacion.SelectedIndex = ddlOperacion.Items.IndexOf(ddlOperacion.Items.FindByValue(oRow.CodigoOperacion.ToString()))
            Else
                ddlOperacion.SelectedIndex = 0
            End If
            ddlFondo.SelectedValue = oRow.CodigoPortafolioSBS
            ddlOperacion.SelectedValue = oRow.CodigoOperacion
            ddlMoneda.SelectedValue = oRow.CodigoMoneda
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
            tbHoraOperacion.Text = oRow.HoraOperacion
            Try
                ddlIntermediario.SelectedValue = oRow.CodigoTercero
            Catch ex As Exception
                ddlIntermediario.SelectedValue = ""
            End Try

            If Not oRow.AfectaFlujoCaja.Equals("") Then 'RGF 20090119
                ddlAfectaFlujoCaja.SelectedValue = oRow.AfectaFlujoCaja 'RGF 20090116
            End If

            CargarContactos()
            ddlContacto.SelectedValue = oRow.CodigoContacto

            txtTipoCambio.Text = Format(oRow.TipoCambio, "##,##0.0000000")
            Try
                ddlMonedaDestino.SelectedValue = oRow.CodigoMonedaDestino
            Catch ex As Exception
                ddlMonedaDestino.SelectedValue = ""
            End Try
            txtObservacion.Text = oRow.Observacion
            tbNPoliza.Text = oRow.NumeroPoliza.ToString()
            rblspot.SelectedValue = IIf(oRow.Delibery = "", "N", oRow.Delibery)
            txtMontoOrigen.Text = Format(oRow.MontoOrigen, "##,##0.0000000")
            txtMontoDestino.Text = Format(oRow.MontoDestino, "##,##0.0000000")
            If oRow.Ficticia = "S" Then
                chkFicticia.Checked = True
            Else
                chkFicticia.Checked = False
            End If
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga data de motivo de cambio si fuera el caso | 07/06/18 
            If oRow.CodigoMotivoCambio <> String.Empty Then ddlMotivoCambio.SelectedValue = oRow.CodigoMotivoCambio
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga data de motivo de cambio si fuera el caso | 07/06/18 
        Catch ex As Exception
            If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                AlertaJS(ObtenerMensaje("CONF31"))
            Else
                AlertaJS(ObtenerMensaje("CONF32"))
            End If
        End Try
    End Sub
    Private Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos()
    End Sub
    Private Sub CargarContactos()
        Dim objContacto As New ContactoBM
        ddlContacto.DataTextField = "DescripcionContacto"
        ddlContacto.DataValueField = "CodigoContacto"
        ddlContacto.DataSource = objContacto.ListarContactoPorTerceros(ddlIntermediario.SelectedValue)
        ddlContacto.DataBind()
        ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub
#Region " /* Métodos Personalizados (Popups Dialogs) */ "
    Private Sub ShowDialogPopupInversionesRealizadas(ByVal fondo As String, ByVal operacion As String, ByVal moneda As String, ByVal fecha As String, ByVal accion As String, ByVal nomFondo As String)
        EjecutarJS("showModalDialog('../frmInversionesRealizadas.aspx?vFondo=" + nomFondo + "&cFondo=" + fondo + "&vOperacion=" + operacion + "&vFechaOperacion=" + fecha + "&vAccion=" + accion + "&vCategoria=CV', '950', '600', 'btnBuscar') ")
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
        tbFechaLiquidacion.Text = tbFechaOperacion.Text
        tbHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Traspaso"
        hdMensaje.Value = "el Ingreso"
        CargarPaginaIngresar()
    End Sub
    Private Sub ControlarCamposEO_CO_XO()
        MostrarOcultarBotonesAcciones(False)
        btnAceptar.Enabled = True
    End Sub
    Private Sub CargarPaginaModificarEO_CO_XO(ByVal acceso As String)
        If acceso = "EO" Or acceso = "CO" Then
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
    Private Sub CargarPaginaIngresar()
        btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        If hdPagina.Value = "SW" Then OcultarDeshabilitaInicioSwap()
    End Sub
    Private Sub CargarPaginaModificar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        CargarPaginaProcesar()
        lblComentarios.InnerText = "Comentarios modificación:"
        txtComentarios.Text = ""
        If hdPagina.Value = "SW" Then OcultarDeshabilitaInicioSwap()
    End Sub
    Private Sub CargarPaginaEliminar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        CargarPaginaProcesar()
        lblComentarios.InnerText = "Comentarios eliminación:"
        txtComentarios.Text = ""
        If hdPagina.Value = "SW" Then OcultarDeshabilitaInicioSwap()
    End Sub
    Private Sub CargarPaginaConsultar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        CargarPaginaProcesar()
        btnAceptar.Enabled = False
        If hdPagina.Value = "SW" Then OcultarDeshabilitaInicioSwap()
    End Sub
    Private Sub CargarPaginaProcesar()
        Dim strJS As New StringBuilder
        btnAceptar.Enabled = True
        strJS.AppendLine("$('#btnAceptar').removeAttr('disabled');")
        If Session("EstadoPantalla") <> "Ingresar" Then
            strJS.AppendLine("$('#btnImprimir').show();")
            strJS.AppendLine("$('#btnImprimir').removeAttr('disabled');")
            btnImprimir.Visible = True
            btnImprimir.Enabled = True
        End If
        EjecutarJS(strJS.ToString())
    End Sub
    Private Sub CargarPaginaAceptar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
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
    End Sub
    Private Sub HabilitaDeshabilitaCabecera(ByVal estado As Boolean)
        ddlFondo.Enabled = estado
        ddlOperacion.Enabled = estado
        ddlMoneda.Enabled = estado
        btnBuscar.Enabled = estado
    End Sub
    Private Sub HabilitaDeshabilitaDatosOperacionComision(ByVal estado As Boolean)
        ddlIntermediario.Enabled = estado
        ddlAfectaFlujoCaja.Enabled = estado
        ddlContacto.Enabled = estado
        tbFechaOperacion.ReadOnly = Not estado
        tbFechaLiquidacion.ReadOnly = Not estado
        tbHoraOperacion.ReadOnly = Not estado
        txtObservacion.ReadOnly = Not estado
        imgFechaOperacion.Attributes.Add("class", "input-append")
        If estado Then
            imgFechaVcto.Attributes.Add("class", "input-append date")
        Else
            imgFechaVcto.Attributes.Add("class", "input-append")
        End If
        ddlMonedaDestino.Enabled = estado
        txtMontoOrigen.ReadOnly = Not estado
        txtObservacion.ReadOnly = Not estado
        txtMontoDestino.ReadOnly = Not estado

        If (hdPagina.Value = "DA") Then
            tbFechaOperacion.ReadOnly = True
            imgFechaOperacion.Attributes.Add("class", "input-append")
        End If

        If ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
            chkRegulaSBS.Enabled = False
        Else
            chkRegulaSBS.Enabled = estado
        End If
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se valida estado de campo Tipo de Cambio | 07/06/18 
        txtTipoCambio.ReadOnly = Not estado
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se valida estado de campo Tipo de Cambio | 07/06/18 
    End Sub
    Private Sub OcultarBotonesInicio()
        btnBuscar.Visible = False
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
        ddlMoneda.SelectedIndex = 0
        ddlOperacion.SelectedIndex = 0
        tbFechaOperacion.Text = ""
        tbFechaLiquidacion.Text = ""
        tbHoraOperacion.Text = ""
        txtTipoCambio.Text = "0.00000000"
        ddlMonedaDestino.SelectedIndex = 0
        txtMontoOrigen.Text = "0.0000000"
        txtObservacion.Text = ""
        txtMontoDestino.Text = "0.0000000"
        txtCodigoOrden.Value = ""
        tbHoraOperacion.Text = ""
    End Sub
#End Region
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(txtCodigoOrden.Value, ddlFondo.SelectedValue, "COMPRA/VENTA MONEDA EXTRANJERA", ddlOperacion.SelectedItem.Text, ddlMoneda.SelectedItem.Value, "", ddlFondo.SelectedItem.Text)
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
    Private Sub ConsolidaLimitesPDF()
        Session("PortafolioLimite") = ddlFondo.SelectedValue
        '1=Vuelve a generar los limites
        '0=No genera, solo lee de la tabla ReporteLimites
        Session("ProcesarLimite") = 0
        Dim rutaArchivo As String
        Dim dtLimites As DataTable
        Dim dr As DataRow
        Dim rutas As New System.Text.StringBuilder
        Dim detalladoPorFondo As String
        Dim ds As DataSet
        UIUtility.PublicarEvento("Inicio Proceso")
        ds = New ReporteLimitesBM().SeleccionarLimitesPorForward(ddlFondo.SelectedValue, DatosRequest)
        dtLimites = ds.Tables(0)
        If dtLimites.Rows.Count > 0 Then
            For Each dr In dtLimites.Rows
                detalladoPorFondo = "N"
                If ddlFondo.SelectedValue.Equals(PORTAFOLIO_MULTIFONDOS) And Not dr("codigoLimite").Equals("16") Then
                    detalladoPorFondo = "S"
                End If
                Dim visor As New ReporteLimites
                Dim dsObtenerFecha As New DataSet
                Dim fecha As Date
                dsObtenerFecha = New ReporteLimitesBM().ObtenerUltimaFecha_ReporteLimite(CType(dr("codigoLimite"), String), CType(dr("codigoLimiteCaracteristica"), String), UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), "REAL", "FORWARD", DatosRequest)
                If Not dsObtenerFecha Is Nothing Then
                    fecha = CType(dsObtenerFecha.Tables(0).Rows(0)(0), Date)
                Else
                    fecha = CType(tbFechaOperacion.Text, Date)
                End If
                visor._CodLimite = dr("codigoLimite")
                visor._CodLimiteCaracteristica = dr("codigoLimiteCaracteristica")
                visor._FechaOperacion = fecha
                visor._DetalladoPorFondo = detalladoPorFondo
                visor._Escenario = "REAL"
                visor._ProcesarLimite = 0
                visor._FolderReportes = "../Reportes/Limites/"
                visor._Portafolio = ddlFondo.SelectedValue
                If dr("codigoLimite") = "42" Then
                    If (Limites.LimiteMaximoNegociacion_Validar(UIUtility.ConvertirFechaaDecimal(fecha)).Count <> 0) Then
                        GoTo [continue]
                    End If
                End If
                If dr("codigoLimite") = "36" Or dr("codigoLimite") = "44" Then
                    Dim oTipoCambio As New TipoCambioBM
                    If (Not oTipoCambio.ExisteTipoCambio("Spot", "DOL", UIUtility.ConvertirFechaaDecimal(fecha))) Then
                        GoTo [continue]
                    End If
                End If
                report = visor.GeneraLimite(DatosRequest, Me.Usuario)
                If Not (report Is Nothing) Then
                    If Not Directory.Exists("c:\temp\") Then
                        Directory.CreateDirectory("c:\temp\")
                    End If
                    rutaArchivo = "c:\temp\" & visor._CodLimite & " - " & UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) & Now.ToString(" - yyyyMMdd hh.mm.ss") & ".pdf"
                    report.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
                    rutas.Append("&" & rutaArchivo)
                End If
[continue]:
            Next
            LimitesParametrizadosPDF(rutas.ToString())
        Else
            AlertaJS("No hay limites para el codigo mnemonico seleccionado!")
        End If
    End Sub
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
                EjecutarJS("window.open('" & sfile.Replace("\", "\\") & "');")
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
    Protected Sub btnRetornar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        If Session("PTNegSim") = "M" Or Session("PTNegSim") = "C" Then
            Response.Redirect("~/frmDefault.aspx")
        Else
            EjecutarJS("windows.close()")
        End If
    End Sub
End Class
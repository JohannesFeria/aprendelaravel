Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Text
Imports ParametrosSIT
Partial Class Modulos_Inversiones_frmConfirmarOrdenesDeInversion
    Inherits BasePage
    Dim StrNumeroOrden As String
    Dim OOrdenInversionDatosOperacionBM As New OrdenInversionDatosOperacionBM
    Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se centraliza las variables que servirá para el filtro de búsqueda | 02/07/18 
    Dim fondo As String = String.Empty
    Dim nroOrden As String = String.Empty
    Dim fecha As String = String.Empty
    Dim tipoInstrumento As String = String.Empty
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se centraliza las variables que servirá para el filtro de búsqueda | 02/07/18 
    Dim listaErrores As New Hashtable

#Region " /* Eventos de Página */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            If (Session("Accion") IsNot Nothing) Then
                If (Session("Accion").ToString().Equals("1")) Then
                    GoTo Load
                End If
            End If
            If Not Page.IsPostBack Then

                Dim FechaOperacion As Decimal
                Dim NroOrden As String
                Dim Portafolio As String
                Session("ValidarFecha") = "FECHAOPERACION"
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de desconfirmación y confirmación | 07/06/18 
                hdRptaDesConfirmar.Value = "NO"
                hdRptaConfirmar.Value = "NO"
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de desconfirmación y confirmación | 07/06/18 
                displayBotones(divBotonConfirmar, "none")
                displayBotones(divBotonEliminar, "none")
                If Not Request.QueryString("vFechaOperacion") Is Nothing Then
                    If Request.QueryString("vFechaOperacion").Length = 8 Then
                        FechaOperacion = Convert.ToDecimal(Request.QueryString("vFechaOperacion"))
                        Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(FechaOperacion))
                    Else
                        FechaOperacion = UIUtility.ConvertirFechaaDecimal(Convert.ToString(Request.QueryString("vFechaOperacion")))
                        Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(FechaOperacion))
                    End If
                Else
                    FechaOperacion = 0
                End If
                If Not Request.QueryString("vNroOrden") Is Nothing Then
                    NroOrden = Request.QueryString("vNroOrden")
                    Me.txtNroOrdenOE.Text = NroOrden
                Else
                    NroOrden = ""
                End If
                If Not Request.QueryString("vPortafolio") Is Nothing Then
                    Portafolio = Request.QueryString("vPortafolio")
                    Me.ddlFondoOE.SelectedValue = Portafolio
                Else
                    Portafolio = ""
                End If
                CargarCombos()
                '    CargarGrillaOIEjecutadas(Portafolio, NroOrden, FechaOperacion, tipoInstrumento)
                '  CargarGrillaOIConfirmadas(Portafolio, NroOrden, FechaOperacion, tipoInstrumento)
                ConsultarPaginasPorOI()
                ViewState("Indica") = 0
                PanelDetalleAmortizacionesVencidas(False)
                PanelDetalleCuponesVencidos(False)
                PanelDetalleDividendosRebatesLiberadas(False)
                If Session("CuponVence") <> "1" Then
                    Dim dtCuponesPorVencer As DataTable = New CuponeraBM().ListarCuponesPorVencer()
                    If dtCuponesPorVencer.Rows.Count > 0 Then
                        Session("CuponesPorVencer") = dtCuponesPorVencer

                        EjecutarJS("showModalDialog('frmCuponesPorVencer.aspx', '650', '700', '');")
                    End If
                Else
                    Session("CuponVence") = ""
                End If
Load:
                If (Session("Accion") IsNot Nothing) Then
                    If (Session("Accion").ToString().Equals("1")) Then
                        Session.Remove("Accion")
                        AlertaJS3("Cargando Listas Actualizadas", "$('#btnBuscar').click();", 500)
                    End If
                End If
            Else
                validarCambioOpcion()
            End If

        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub
    Private Sub btnConfirmar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmar.Click
        Dim menValidacion As String
        Dim contConforme As Integer = 0
        Dim RptaConfirmacion As Integer = 0
        Try
            Dim tipoConfirmacion As String = String.Empty
            Dim nSeleccionadas As Int64 = 0, iCont As Int64 = dgListaOE.Rows.Count - 1
            Dim chk As CheckBox
            nSeleccionadas = retornarNumSeleccionados(dgListaOE, "chkSelectPE")
            If nSeleccionadas > 0 Then
                If hdRptaConfirmar.Value.ToUpper = "NO" Then
                    ConfirmarJS("¿Está seguro de realizar la confirmación masiva?", "document.getElementById('hdRptaConfirmar').value = 'SI'; document.getElementById('btnConfirmar').click(); ")
                Else
                    hdRptaConfirmar.Value = "NO"
                    reiniciarColObservacion()
                    Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM

                    iCont = dgListaOE.Rows.Count - 1
                    listaErrores.Clear()
                    While iCont >= 0
                        If dgListaOE.Rows(iCont).FindControl("chkSelectPE").GetType Is GetType(CheckBox) Then
                            chk = CType(dgListaOE.Rows(iCont).FindControl("chkSelectPE"), CheckBox)
                            If chk.Checked = True Then
                                'oOrdenInversionWorkFlowBM.ConfirmarOI(dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOE)).Text, dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolio", dgListaOE)).Text, "CM", Me.DatosRequest)
                                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se implementa para la confirmación masiva de acuerdo al tipo de instrumento | 07/06/18 
                                menValidacion = validarCampos_Grilla(dgListaOE.Rows.Item(iCont), dgListaOE)
                                If menValidacion <> String.Empty Then
                                    If menValidacion.Substring(0, 3).Trim = "-" Then menValidacion = "Falta los siguientes campos Obligatorios:" + Environment.NewLine + menValidacion
                                    listaErrores.Add(dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOE)).Text, menValidacion)
                                Else
                                    If (dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("TipoOperacion", dgListaOE)).Text.Trim = "Dividendo" Or dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("TipoOperacion", dgListaOE)).Text.Trim = "Rebate" Or dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("TipoOperacion", dgListaOE)).Text.Trim = "Liberada") Then
                                        Dim dobj As New DividendosRebatesLiberadasBM()
                                        Dim strPortafolio As String = dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolio", dgListaOE)).Text.Trim
                                        Dim strnemonico As String = dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("CodigoMnemonico", dgListaOE)).Text.Trim
                                        Dim strfecha As String = dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("FechaVencimiento", dgListaOE)).Text.Trim
                                        Dim strOperador As String = dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOE)).Text.Trim
                                        Dim strIdentificador As String = strOperador.Substring(4, strOperador.Length - 4)
                                        Dim Monto As Decimal = Convert.ToDecimal(dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("MontoOperacion", dgListaOE)).Text.Trim)

                                        dobj.ConfirmarDividendoRebateLiberada(strPortafolio, strnemonico, strfecha, strIdentificador, Monto, DatosRequest)
    
                                    Else
                                        RptaConfirmacion = oOrdenInversionWorkFlowBM.ConfirmarOI(dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOE)).Text, _
                                                                        dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolio", dgListaOE)).Text, _
                                                                        obteneNroPoliza(dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOE)).Text, dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("Categoria", dgListaOE)).Text), _
                                                                        Me.DatosRequest)
                                    End If

                                    If RptaConfirmacion = 0 Then
                                        contConforme += 1
                                    Else
                                        If RptaConfirmacion = 1 Then
                                            menValidacion = "Esta orden de inversión ya fue confirmada, por favor revisar."
                                        Else
                                            menValidacion = "Ocurrió un error en el proceso de Confirmación:" + Environment.NewLine + "Número de Error: " + RptaConfirmacion.ToString + Environment.NewLine + "Por Favor Confirmar Individualmente."
                                        End If
                                        listaErrores.Add(dgListaOE.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOE)).Text, menValidacion)
                                    End If
                                End If
                            End If
                        End If
                        iCont = iCont - 1
                    End While
                    dgListaOE.PageIndex = 0
                    ViewState("Indica") = 1

                    If listaErrores.Count > 0 Then
                        dgListaOE.Columns(3).HeaderStyle.CssClass = ""
                        dgListaOE.Columns(3).ItemStyle.CssClass = ""
                    End If

                    GetDatos(fondo, nroOrden, fecha, tipoInstrumento)

                    CargarGrillaOIEjecutadas(fondo, nroOrden, fecha, tipoInstrumento)
                    CargarGrillaOIConfirmadas(fondo, nroOrden, fecha, tipoInstrumento)
                    Me.lblNroTransaccion.Text = ""
                    Me.lblTotalComision.Text = ""
                    Me.dgrComisiones.DataSource = Nothing
                    Me.dgrComisiones.DataBind()
                    If dgListaOE.Columns(3).HeaderStyle.CssClass = String.Empty Then
                        AlertaJS("<p align=left>- Se confirmaron " + contConforme.ToString + " ordenes de inversiones correctamente. <br>" + _
                                 "- Hubieron " + listaErrores.Count.ToString + " ordenes de inversiones que no se pudieron confirmar masivamente, por favor revisar columna de [Observaciones]</p>")
                    Else
                        AlertaJS("Se Confirmaron todas (" + contConforme.ToString + ") las ordenes de inversiones correctamente.")
                    End If
                    displayBotones(divBotonConfirmar, "none")
                End If
            Else
                'OT 10090 - 21/03/2017 - Carlos Espejo
                'Descripcion: Se validacion para cuando se de un texto html invalido
                reiniciarColObservacion()
                If HttpUtility.HtmlDecode(lCategoria.Text) <> "" Then
                    tipoConfirmacion = lTipoConfirmacion.Text.ToString()
                    'CUPONERA ORDEN INVERSION
                    If tipoConfirmacion = "COI" Then
                        IrCOI(lFondo.Text, lNemonico.Text, lblMontoCuponera.Text, UIUtility.ConvertirFechaaDecimal(lfechaOperacion.Text).ToString(), _
                        lSecuencial.Text.ToString(), lblNroTransaccion.Text)
                        'DIVIDENDOS REBATES LIBERADAS
                    ElseIf tipoConfirmacion = "DRL" Then
                        Ir(lNombreFondo.Text, lFondo.Text, lblMnemonicoDRL.Text.ToString(), lblMontoDRL.Text, _
                        UIUtility.ConvertirFechaaDecimal(lblFechaVencimientoDRL.Text.ToString()).ToString(), lIdentificador.Text.ToString(), _
                        lEstado.Text.ToString(), lblMonedaDRL.Text.ToString())
                        'CUPONERAS ORDEN AMORTIZACION
                    ElseIf tipoConfirmacion = "COA" Then
                        IrCOA(Me.lblNroTransaccion.Text, lFondo.Text, lNemonico.Text, lblValorNominalLocalAmort.Text, _
                            UIUtility.ConvertirFechaaDecimal(lfechaOperacion.Text).ToString(), lSecuencial.Text.ToString(), lblNroTransaccion.Text)
                    Else
                        If hndCodigoOperacion.Value = "40" Then
                            Dim StrURL As String
                            StrURL = "InstrumentosNegociados/frmInstrumentosSinCuponera.aspx?codigoOrden=" & Me.lblNroTransaccion.Text.Trim & _
                            "&codigoPortafolio=" & lFondo.Text
                            ShowDialogPopup(StrURL)
                        Else
                            Ir(HttpUtility.HtmlDecode(lCategoria.Text), Me.lblNroTransaccion.Text, Me.lFondo.Text)
                        End If
                    End If
                    Session("Accion") = 1
                Else
                    AlertaJS("Debe seleccionar un Registro")
                    displayBotones(divBotonConfirmar, "none")
                End If
            End If
            'OT 10090 Fin
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Confirmar")
        End Try
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Try

            Dim nSeleccionadas As Int64 = 0, iCont As Int64 = dgListaOE.Rows.Count - 1
            Dim chk As CheckBox
            Dim RptaConfirmacion As Integer = 0
            Dim contConforme As Integer = 0
            Dim menValidacion As String
            Dim codPortafolio As String
            Dim fechaOperacion As Decimal

            nSeleccionadas = retornarNumSeleccionados(dgListaOC, "chkSelectOC")
            If nSeleccionadas > 0 And hdRptaDesConfirmar.Value.ToUpper = "NO" Then 'RCE | Opción cuando la eliminación de confirmación es masiva | 11/07/2018
                If hdRptaConfirmar.Value.ToUpper = "NO" Then
                    ConfirmarJS("¿Está seguro de realizar la eliminación en confirmación masiva?", "document.getElementById('hdRptaConfirmar').value = 'SI'; document.getElementById('btnEliminar').click(); ")
                Else
                    hdRptaConfirmar.Value = "NO"
                    reiniciarColObservacion()
                    iCont = dgListaOC.Rows.Count - 1
                    listaErrores.Clear()
                    While iCont >= 0
                        If dgListaOC.Rows(iCont).FindControl("chkSelectOC").GetType Is GetType(CheckBox) Then
                            chk = CType(dgListaOC.Rows(iCont).FindControl("chkSelectOC"), CheckBox)
                            If chk.Checked = True Then
                                codPortafolio = dgListaOC.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolio", dgListaOC)).Text
                                fechaOperacion = UIUtility.ConvertirFechaaDecimal(dgListaOC.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("FechaOperacion", dgListaOC)).Text)
                                StrNumeroOrden = dgListaOC.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOC)).Text
                                menValidacion = verValorizadoExistencia(codPortafolio, fechaOperacion, StrNumeroOrden)
                                If Not menValidacion = String.Empty Then
                                    listaErrores.Add(dgListaOC.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOC)).Text, menValidacion)
                                Else
                                    RptaConfirmacion = oOrdenPreOrdenInversionBM.ExtornaOrdenInversionConfirmada(codPortafolio, StrNumeroOrden, Me.DatosRequest)
                                    If RptaConfirmacion = 0 Then
                                        contConforme += 1
                                    Else
                                        menValidacion = "Ocurrió un error en el proceso de eliminación de confirmación:" + Environment.NewLine + "Número de Error: " + RptaConfirmacion.ToString + Environment.NewLine + "Por Favor eliminar la confirmación Individualmente."
                                        listaErrores.Add(dgListaOC.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOC)).Text, menValidacion)
                                    End If
                                End If
                            End If
                        End If
                        iCont = iCont - 1
                    End While

                    If listaErrores.Count > 0 Then
                        dgListaOC.Columns(2).HeaderStyle.CssClass = ""
                        dgListaOC.Columns(2).ItemStyle.CssClass = ""
                    End If

                    GetDatos(fondo, nroOrden, fecha, tipoInstrumento)
                    CargarGrillaOIEjecutadas(fondo, nroOrden, fecha, tipoInstrumento)
                    CargarGrillaOIConfirmadas(fondo, nroOrden, fecha, tipoInstrumento)
                    Me.lblNroTransaccion.Text = ""
                    Me.lblTotalComision.Text = ""
                    Me.dgrComisiones.DataSource = Nothing
                    Me.dgrComisiones.DataBind()
                    If dgListaOC.Columns(2).HeaderStyle.CssClass = String.Empty Then
                        AlertaJS("<p align=left>- Se eliminaron " + contConforme.ToString + " confirmaciones de las ordenes de inversiones correctamente. <br>" + _
                                 "- Hubieron " + listaErrores.Count.ToString + " ordenes de inversiones que no se pudieron eliminar su confirmación masivamente, por favor revisar columna de [Observaciones]</p>")
                    Else
                        AlertaJS("Se eliminaron todas (" + contConforme.ToString + ") las confirmaciones de las ordenes de inversiones correctamente.")
                    End If
                    displayBotones(divBotonEliminar, "none")
                End If
            ElseIf hdRptaDesConfirmar.Value.ToUpper = "SI" Then  'RCE | Opción cuando la eliminación de confirmación es individual | 11/07/2018
                displayBotones(divBotonEliminar, "none")
                If lCategoria.Text = "" And lFondo.Text = "" Then
                    AlertaJS("Debe seleccionar un Registro")
                    Return
                End If
                reiniciarColObservacion()
                StrNumeroOrden = Me.lblNroTransaccion.Text
                fechaOperacion = UIUtility.ConvertirFechaaDecimal(lfechaOperacion.Text)
                hdRptaDesConfirmar.Value = "NO"
                menValidacion = verValorizadoExistencia(Me.lFondo.Text, fechaOperacion, StrNumeroOrden)
                If Not menValidacion = String.Empty Then
                    AlertaJS(menValidacion)
                Else
                    GetDatos(fondo, nroOrden, fecha, tipoInstrumento)
                    If (Me.hdnTipoSeleccion.Value.Equals("OE")) Then
                        oOrdenPreOrdenInversionBM.EliminarOI(StrNumeroOrden, Me.lFondo.Text, "", DatosRequest)
                        AlertaJS("La Orden de Inversión ha sido eliminado satisfactoriamente.")
                    ElseIf (Me.hdnTipoSeleccion.Value.Equals("OC")) Then
                        oOrdenPreOrdenInversionBM.ExtornaOrdenInversionConfirmada(Me.lFondo.Text, StrNumeroOrden, Me.DatosRequest)
                        CargarGrillaOIConfirmadas(fondo, nroOrden, fecha, tipoInstrumento)
                        AlertaJS("Se ha eliminado la confirmación de la Orden de Inversión satisfactoriamente.")
                    End If
                    CargarGrillaOIEjecutadas(fondo, nroOrden, fecha, tipoInstrumento)
                    Me.lblNroTransaccion.Text = ""
                    Me.lblTotalComision.Text = ""
                    Me.dgrComisiones.DataSource = Nothing
                    Me.dgrComisiones.DataBind()
                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Reiniciar columna observación | 04/07/18 
                    reiniciarColObservacion()
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Reiniciar columna observación | 04/07/18 
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim nombreDia As String = String.Empty
            Dim diferenciaDias As Integer = 0

            dgListaOE.PageIndex = 0
            dgListaOC.PageIndex = 0
            ViewState("Indica") = 1
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Ocultar botones de confirmar y eliminar | 04/07/18 
            displayBotones(divBotonEliminar, "none")
            displayBotones(divBotonConfirmar, "none")
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Ocultar botones de confirmar y eliminar | 04/07/18 
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Reiniciar columna observación | 04/07/18 
            reiniciarColObservacion()
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Reiniciar columna observación | 04/07/18 
            GetDatos(fondo, nroOrden, fecha, tipoInstrumento)
            CargarGrillaOIEjecutadas(fondo, nroOrden, fecha, tipoInstrumento)
            CargarGrillaOIConfirmadas(fondo, nroOrden, fecha, tipoInstrumento)
            Me.lblNroTransaccion.Text = ""
            Me.lblTotalComision.Text = ""
            Me.dgrComisiones.DataSource = Nothing
            Me.dgrComisiones.DataBind()
            ViewState("Fondo") = fondo
            ViewState("NroOrden") = nroOrden
            'ViewState("Fecha") = fecha
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se oculta las dos primeras columnas cuando el filtro de búsqueda tiene fecha de operación diferente al generado por portafolio| 04/07/18 
            MostrarColumna_Grilla(dgListaOE, 0, String.Empty, String.Empty)
            MostrarColumna_Grilla(dgListaOE, 1, String.Empty, String.Empty)
            Session("ValidarFecha") = "FECHAOPERACION"
            If Not ddlFondoOE.SelectedItem.Text.ToUpper.Contains("TODOS") And CType(ViewState("Fecha"), String) <> tbFechaOperacion.Text Then
                nombreDia = UIUtility.ObtenerNombreDia(tbFechaOperacion.Text).ToUpper
                diferenciaDias = UIUtility.ObtenerDiferenciaDias(tbFechaOperacion.Text, CType(ViewState("Fecha"), String))
                If nombreDia.Contains("BADO") Or nombreDia.Contains("DOMINGO") Then
                    If diferenciaDias > 2 Or diferenciaDias < 0 Then
                        MostrarColumna_Grilla(dgListaOE, 0, String.Empty, "ocultarCol")
                        MostrarColumna_Grilla(dgListaOE, 1, String.Empty, "ocultarCol")
                        Session("ValidarFecha") = "FECHADIFERENTE"
                    End If
                Else
                    MostrarColumna_Grilla(dgListaOE, 0, String.Empty, "ocultarCol")
                    MostrarColumna_Grilla(dgListaOE, 1, String.Empty, "ocultarCol")
                    Session("ValidarFecha") = "FECHADIFERENTE"
                End If
            End If
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se oculta las dos primeras columnas cuando el filtro de búsqueda tiene fecha de operación diferente al generado por portafolio| 04/07/18 
            Session.Remove("Accion")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub
    Protected Sub dgListaOE_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgListaOE.RowDataBound
        Try
            Dim ibSeleccionarPE As ImageButton
            Dim chkSelectPE As CheckBox
            Dim ibObservacion As ImageButton
            Dim xError As DictionaryEntry
            Dim ibDetallePE As ImageButton
            If e.Row.RowType = DataControlRowType.DataRow Then
                ibSeleccionarPE = CType(e.Row.FindControl("ibSeleccionarPE"), ImageButton)
                ibDetallePE = CType(e.Row.FindControl("ibVer"), ImageButton)
                chkSelectPE = CType(e.Row.FindControl("chkSelectPE"), CheckBox)
                e.Row.BackColor = Drawing.Color.White

                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se mostrará para los tipos de instrumento "DP", "BO", "AC", "OR", "FI", "FM", "CV" Y "PC" un checkbox para selección masiva, para FD se considera confirmación individual porque se ingresa el ISIN manualmente| 04/07/18 
                If (e.Row.Cells(obtenerIndiceColumna_Grilla("TipoOperacion", dgListaOE)).Text.Trim = "Dividendo" Or e.Row.Cells(obtenerIndiceColumna_Grilla("TipoOperacion", dgListaOE)).Text.Trim = "Rebate" Or e.Row.Cells(obtenerIndiceColumna_Grilla("TipoOperacion", dgListaOE)).Text.Trim = "Liberada") Then
                    ibSeleccionarPE.Visible = False
                    chkSelectPE.Visible = True
                Else
                    Select Case e.Row.Cells(obtenerIndiceColumna_Grilla("Categoria", dgListaOE)).Text
                        Case CLASE_INSTRUMENTO_DEPOSITOPLAZO, CLASE_INSTRUMENTO_BONO, CLASE_INSTRUMENTO_ACCION, CLASE_INSTRUMENTO_OPERACIONES_REPORTE, _
                            CLASE_INSTRUMENTO_FONDOINVERSION, CLASE_INSTRUMENTO_FONDOMUTUO, CLASE_INSTRUMENTO_CVME, CLASE_INSTRUMENTO_PAPELES_COMERCIALES
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se valida que tipo de OI sea de DividendosRebatesLiberadas para confirmar individual | 04/07/18 
                            Select Case e.Row.Cells(obtenerIndiceColumna_Grilla("TipoConfirmacion", dgListaOE)).Text.Trim
                                Case "COA", "COI", "DRL"
                                    ibSeleccionarPE.Visible = True
                                    chkSelectPE.Visible = False
                                Case Else
                                    If e.Row.Cells(obtenerIndiceColumna_Grilla("CodigoOperacion", dgListaOE)).Text.Trim = "40" Then
                                        ibSeleccionarPE.Visible = True
                                        chkSelectPE.Visible = False
                                    Else
                                        ibSeleccionarPE.Visible = False
                                        chkSelectPE.Visible = True
                                    End If
                            End Select
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se valida que tipo de OI sea de DividendosRebatesLiberadas para confirmar individual | 04/07/18 
                        Case "FA"
                            ibDetallePE.Visible = False
                            ibSeleccionarPE.Visible = False
                            chkSelectPE.Visible = True

                        Case Else
                            ibSeleccionarPE.Visible = True
                            chkSelectPE.Visible = False
                    End Select
                End If



                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se mostrará para los tipos de instrumento "DP", "BO", "AC", "OR", "FI", "FM", "CV" Y "PC" un checkbox para selección masiva, para FD se considera confirmación individual porque se ingresa el ISIN manualmente| 04/07/18 
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se valida si se tiene errores en la confirmación masiva | 04/07/18 
                If listaErrores.Count > 0 Then
                    For Each xError In listaErrores
                        If xError.Key = e.Row.Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOE)).Text Then
                            ibObservacion = CType(e.Row.FindControl("ibObservacion"), ImageButton)
                            ibObservacion.Visible = True
                            ibObservacion.ToolTip = xError.Value
                        End If
                    Next
                End If
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se valida si se tiene errores en la confirmación masiva | 04/07/18 
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub
    Protected Sub dgListaOE_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaOE.PageIndexChanging
        Try
            dgListaOE.PageIndex = e.NewPageIndex
            reiniciarColObservacion()
            GetDatos(fondo, nroOrden, fecha, tipoInstrumento)
            CargarGrillaOIEjecutadas(fondo, nroOrden, fecha, tipoInstrumento)
            Me.lblCodigoISIN.Text = ""
            Me.lblNroTransaccion.Text = ""
            Me.lCategoria.Text = ""
            Me.lFondo.Text = ""
            Me.lblTipoOperacion.Text = ""
            Me.lblTipoOrden.Text = ""
            PanelDetalleCuponesVencidos(False)
            PanelDetalleDividendosRebatesLiberadas(False)
            PanelDetalleOrdenes(True)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub
    Protected Sub dgListaOE_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaOE.RowCommand
        Try
            Dim i As Int32 = 0
            Dim valor As String = String.Empty
            Dim iCont As Int64 = dgListaOE.Rows.Count - 1
            Dim Row As GridViewRow
            Dim tipoConfirmacion As String = String.Empty

            displayBotones(divBotonConfirmar, "none")
            If e.CommandName = "Seleccionar" Then
                i = dgListaOE.SelectedIndex
                If i >= 0 And i <= iCont Then
                    If i Mod 2 = 0 Then
                        dgListaOE.Rows.Item(i).BackColor = System.Drawing.Color.White
                    Else
                        dgListaOE.Rows.Item(i).BackColor = System.Drawing.Color.White
                    End If
                End If
                seleccionarMasivoRow_Grilla(dgListaOE, "chkSelectPE")
                Dim inpChk As HtmlInputCheckBox = CType(dgListaOE.Controls(0).Controls(0).Controls(0).FindControl("SelectAllCheckBoxOC"), HtmlInputCheckBox)
                If Not (inpChk Is Nothing) Then inpChk.Checked = False
                iCont = dgListaOE.Rows.Count - 1
                Row = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                i = Row.RowIndex
                'OT 10059 02/03/2017 - Carlos Espejo
                'Descripcion: Validar el codigo de Mercado del Emisor
                Dim TerceroVal As String = OOrdenInversionDatosOperacionBM.ValidarDatosConfirmacion(dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoISIN", dgListaOE)).Text)
                If TerceroVal <> String.Empty Then
                    displayBotones(divBotonConfirmar, "none")
                    displayBotones(divBotonEliminar, "none")
                    AlertaJS("El campo Mercado del tercero: " + TerceroVal + " esta incompleto. <br/> Favor de actualizarlo antes de continuar.")
                    Exit Sub
                End If
                'OT 10059 Fin
                dgListaOE.SelectedIndex = Row.RowIndex
                If i >= 0 And i <= iCont Then
                    dgListaOE.Rows.Item(i).BackColor = System.Drawing.Color.LemonChiffon
                    lTipoConfirmacion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("TipoConfirmacion", dgListaOE)).Text
                    If lTipoConfirmacion.Text = "COA" Then
                        PanelDetalleCuponesVencidos(False)
                        PanelDetalleOrdenes(False)
                        PanelDetalleAmortizacionesVencidas(True)
                        If dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaVencimiento", dgListaOE)).Text.ToString <> Nothing Or HttpUtility.HtmlDecode(dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaVencimiento", dgListaOE)).Text.ToString).Trim <> "" Then
                            lblFechaVencimientoAmort.Text = UIUtility.ConvertirFechaaString(dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaVencimiento", dgListaOE)).Text)
                        End If
                        lblValorNominalLocalAmort.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("ValorNominaLocalCupon", dgListaOE)).Text
                        lSecuencial.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Secuencial", dgListaOE)).Text
                        valor = "COA"
                        lfechaOperacion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaOperacion", dgListaOE)).Text
                    ElseIf lTipoConfirmacion.Text = "COI" Then
                        PanelDetalleCuponesVencidos(True)
                        PanelDetalleOrdenes(False)
                        If dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaVencimiento", dgListaOE)).Text.ToString <> Nothing Or HttpUtility.HtmlDecode(dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaVencimiento", dgListaOE)).Text.ToString).Trim <> "" Then
                            lblFechaVencimiento.Text = UIUtility.ConvertirFechaaString(dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaVencimiento", dgListaOE)).Text)
                        End If
                        lblMontoCuponera.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("ValorNominaLocalCupon", dgListaOE)).Text
                        lSecuencial.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Secuencial", dgListaOE)).Text
                        valor = "COI"
                        lfechaOperacion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaOperacion", dgListaOE)).Text
                    ElseIf lTipoConfirmacion.Text = "DRL" Then
                        PanelDetalleDividendosRebatesLiberadas(True)
                        PanelDetalleOrdenes(False)
                        If dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaVencimiento", dgListaOE)).Text.ToString <> Nothing Or HttpUtility.HtmlDecode(dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaVencimiento", dgListaOE)).Text.ToString).Trim <> "" Then
                            lblFechaVencimientoDRL.Text = UIUtility.ConvertirFechaaString(dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaVencimiento", dgListaOE)).Text)
                        End If
                        lblTipoOperacionDRL.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("TipoOperacion", dgListaOE)).Text
                        lblMontoDRL.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("MontoOperacion", dgListaOE)).Text
                        lblMnemonicoDRL.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoMnemonico", dgListaOE)).Text
                        lblMonedaDRL.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoMoneda", dgListaOE)).Text
                        lIdentificador.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Secuencial", dgListaOE)).Text

                        Session("CodigoSBS_DRL") = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoSBS", dgListaOE)).Text
                        lEstado.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Estado", dgListaOE)).Text
                        valor = "DRL"
                        lfechaOperacion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaOperacion", dgListaOE)).Text
                    Else
                        PanelDetalleAmortizacionesVencidas(False)
                        PanelDetalleCuponesVencidos(False)
                        PanelDetalleDividendosRebatesLiberadas(False)
                        PanelDetalleOrdenes(True)
                        valor = "COI"
                    End If
                    Me.hdnTipoSeleccion.Value = "OE"
                    lblCodigoISIN.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoISIN", dgListaOE)).Text
                    lblTipoOrden.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("TipoOrden", dgListaOE)).Text
                    lblNroTransaccion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOE)).Text
                    lblTipoOperacion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("TipoOperacion", dgListaOE)).Text
                    lCategoria.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Categoria", dgListaOE)).Text
                    lNemonico.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoMnemonico", dgListaOE)).Text
                    lFondo.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolio", dgListaOE)).Text
                    lNombreFondo.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Fondo", dgListaOE)).Text
                    Session("codigoPortafolioSBS_CO") = lFondo.Text
                    Session("TipoInstrumento_OI") = lCategoria.Text
                    hndCodigoOperacion.Value = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoOperacion", dgListaOE)).Text
                    If lblNroTransaccion.Text.IndexOf("Tmp") = 0 Then
                        Dim obj As New Hashtable
                        obj.Add("CodigoPortafolioSBS", lFondo.Text)
                        obj.Add("MontoOperacion", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("MontoOperacion", dgListaOE)).Text)
                        obj.Add("CodigoTipoCupon", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoTipoCupon", dgListaOE)).Text)
                        obj.Add("CodigoMoneda", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoMoneda", dgListaOE)).Text)
                        obj.Add("CodigoISIN", lblCodigoISIN.Text)
                        obj.Add("CodigoSBS", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoSBS", dgListaOE)).Text)
                        obj.Add("CodigoTercero", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoTercero", dgListaOE)).Text)
                        obj.Add("CodigoNemonico", lNemonico.Text)
                        obj.Add("CodigoTipoTitulo", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoTipoTitulo", dgListaOE)).Text)
                        obj.Add("Categoria", lCategoria.Text)
                        obj.Add("TasaCupon", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("TasaCupon", dgListaOE)).Text)
                        obj.Add("CodigoOperacion", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoOperacion", dgListaOE)).Text)
                        obj.Add("FechaOperacion", 0)
                        Session("DatosOrdenTemporal") = obj
                    End If
                End If
                displayBotones(divBotonConfirmar, "block")
                Me.lblTotalComision.Text = ""
                Me.dgrComisiones.DataSource = Nothing
                Me.dgrComisiones.DataBind()
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se implementa columna para ver y eliminar la OI | 04/07/18 
            ElseIf e.CommandName = "Modificar" Then
                Row = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                i = Row.RowIndex
                lCategoria.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Categoria", dgListaOE)).Text
                lblNroTransaccion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOE)).Text
                lFondo.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolio", dgListaOE)).Text
                tipoConfirmacion = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("TipoConfirmacion", dgListaOE)).Text
                lNemonico.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoMnemonico", dgListaOE)).Text
                lfechaOperacion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaOperacion", dgListaOE)).Text
                lSecuencial.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Secuencial", dgListaOE)).Text
                lIdentificador.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Secuencial", dgListaOE)).Text
                lNombreFondo.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Fondo", dgListaOE)).Text
                Session("codigoPortafolioSBS_CO") = lFondo.Text
                Session("CodigoSBS_DRL") = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoSBS", dgListaOE)).Text
                Session("TipoInstrumento_OI") = lCategoria.Text
                Select Case tipoConfirmacion
                    Case "COI"
                        lblMontoCuponera.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("ValorNominaLocalCupon", dgListaOE)).Text
                        IrCOI(lFondo.Text, lNemonico.Text, lblMontoCuponera.Text, UIUtility.ConvertirFechaaDecimal(lfechaOperacion.Text).ToString(), _
                        lSecuencial.Text.ToString(), lblNroTransaccion.Text)
                    Case "DRL"
                        lblMnemonicoDRL.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoMnemonico", dgListaOE)).Text
                        lblMontoDRL.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("MontoOperacion", dgListaOE)).Text
                        lblMonedaDRL.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoMoneda", dgListaOE)).Text
                        lEstado.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Estado", dgListaOE)).Text
                        lblFechaVencimientoDRL.Text = UIUtility.ConvertirFechaaString(dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaVencimiento", dgListaOE)).Text)
                        Ir(lNombreFondo.Text, lFondo.Text, lblMnemonicoDRL.Text.ToString(), lblMontoDRL.Text, _
                        UIUtility.ConvertirFechaaDecimal(lblFechaVencimientoDRL.Text.ToString()).ToString(), lIdentificador.Text.ToString(), _
                        lEstado.Text.ToString(), lblMonedaDRL.Text.ToString())
                    Case "COA"
                        lblValorNominalLocalAmort.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("ValorNominaLocalCupon", dgListaOE)).Text
                        IrCOA(Me.lblNroTransaccion.Text, lFondo.Text, lNemonico.Text, lblValorNominalLocalAmort.Text, _
                        UIUtility.ConvertirFechaaDecimal(lfechaOperacion.Text).ToString(), lSecuencial.Text.ToString(), lblNroTransaccion.Text)
                    Case Else
                        Ir(HttpUtility.HtmlDecode(lCategoria.Text), Me.lblNroTransaccion.Text, Me.lFondo.Text)
                End Select
                Session("Accion") = 1
            ElseIf e.CommandName = "Eliminar" Then
                Row = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                i = Row.RowIndex
                lblNroTransaccion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOE)).Text
                lFondo.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolio", dgListaOE)).Text
                Me.hdnTipoSeleccion.Value = "OE"
                ConfirmarJS("¿Está seguro de eliminar la orden de inversión Nro. " + lblNroTransaccion.Text + "?", "document.getElementById('hdRptaDesConfirmar').value = 'SI'; document.getElementById('btnEliminar').click(); ")
            End If
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se implementa columna para ver y eliminar la OI | 04/07/18 
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try
    End Sub
    Public Sub dgListaOE_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim chkTemp As CheckBox = CType(sender, CheckBox)
            Dim dgi As GridViewRow
            Dim i As Int32 = 0
            Dim valor As String = String.Empty
            Dim nSeleccionadas As Int64 = 0
            If hdnSelect.Value = String.Empty Then
                Dim chk As CheckBox
                Dim iCont As Int64 = dgListaOE.Rows.Count - 1
                i = dgListaOE.SelectedIndex
                displayBotones(divBotonConfirmar, "none")
                If i >= 0 And i <= iCont Then
                    If dgListaOE.Rows(i).FindControl("chkSelectPE").GetType Is GetType(CheckBox) Then
                        chk = CType(dgListaOE.Rows(i).FindControl("chkSelectPE"), CheckBox)
                        If chk.Visible = False Then
                            If i Mod 2 = 0 Then
                                dgListaOE.Rows(i).BackColor = System.Drawing.Color.White ' Smoke
                            Else
                                dgListaOE.Rows(i).BackColor = System.Drawing.Color.White
                            End If
                        End If
                    End If
                End If
                dgi = CType(chkTemp.Parent.Parent, GridViewRow)
                i = dgi.RowIndex
                If i >= 0 And i <= iCont Then
                    If chkTemp.Checked = True Then
                        displayBotones(divBotonConfirmar, "block")
                        nSeleccionadas = retornarNumSeleccionados(dgListaOE, "chkSelectPE")
                        If nSeleccionadas > 1 Then
                            dgListaOE.SelectedIndex = -1
                            lblNroTransaccion.Text = ""
                            dgListaOE.Rows(i).BackColor = System.Drawing.Color.LemonChiffon
                        Else
                            dgListaOE.SelectedIndex = dgi.RowIndex
                            dgListaOE.Rows(i).BackColor = System.Drawing.Color.LemonChiffon
                            lTipoConfirmacion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("TipoConfirmacion", dgListaOE)).Text
                            PanelDetalleAmortizacionesVencidas(False)
                            PanelDetalleCuponesVencidos(False)
                            PanelDetalleDividendosRebatesLiberadas(False)
                            PanelDetalleOrdenes(True)
                            valor = "COI"
                            Me.hdnTipoSeleccion.Value = "OE"
                            lblCodigoISIN.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoISIN", dgListaOE)).Text
                            lblTipoOrden.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("TipoOrden", dgListaOE)).Text
                            lblNroTransaccion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOE)).Text
                            lblTipoOperacion.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("TipoOperacion", dgListaOE)).Text
                            lCategoria.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("Categoria", dgListaOE)).Text
                            lNemonico.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoMnemonico", dgListaOE)).Text
                            lFondo.Text = dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolio", dgListaOE)).Text

                            Me.lblTotalComision.Text = ""
                            Me.dgrComisiones.DataSource = Nothing
                            Me.dgrComisiones.DataBind()
                            If lblNroTransaccion.Text.IndexOf("Tmp") = 0 Then
                                Dim obj As New Hashtable
                                obj.Add("CodigoPortafolioSBS", lFondo.Text)
                                obj.Add("MontoOperacion", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("MontoOperacion", dgListaOE)).Text)
                                obj.Add("CodigoTipoCupon", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoTipoCupon", dgListaOE)).Text)
                                obj.Add("CodigoMoneda", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoMoneda", dgListaOE)).Text)
                                obj.Add("CodigoISIN", lblCodigoISIN.Text)
                                obj.Add("CodigoSBS", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoSBS", dgListaOE)).Text)
                                obj.Add("CodigoTercero", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoTercero", dgListaOE)).Text)
                                obj.Add("CodigoNemonico", lNemonico.Text)
                                obj.Add("CodigoTipoTitulo", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoTipoTitulo", dgListaOE)).Text)
                                obj.Add("Categoria", lCategoria.Text)
                                obj.Add("TasaCupon", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("TasaCupon", dgListaOE)).Text)
                                obj.Add("CodigoOperacion", dgListaOE.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoOperacion", dgListaOE)).Text)
                                obj.Add("FechaOperacion", 0)
                                Session("DatosOrdenTemporal") = obj
                            End If
                        End If
                    Else
                        dgListaOE.SelectedIndex = -1
                        lblNroTransaccion.Text = ""
                        If dgListaOE.Rows(i).RowType = DataControlRowType.DataRow Then
                            dgListaOE.Rows(i).BackColor = System.Drawing.Color.White
                        ElseIf dgListaOE.Rows(i).RowType = DataControlRowType.DataRow Then
                            dgListaOE.Rows(i).BackColor = System.Drawing.Color.White
                        End If
                        nSeleccionadas = retornarNumSeleccionados(dgListaOE, "chkSelectPE")
                        If nSeleccionadas > 0 Then displayBotones(divBotonConfirmar, "block")
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub
    Protected Sub dgListaOC_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaOC.PageIndexChanging
        Try
            dgListaOC.PageIndex = e.NewPageIndex
            reiniciarColObservacion()
            GetDatos(fondo, nroOrden, fecha, tipoInstrumento)
            CargarGrillaOIConfirmadas(fondo, nroOrden, fecha, tipoInstrumento)
            Me.lblCodigoISIN.Text = ""
            Me.lblNroTransaccion.Text = ""
            Me.lCategoria.Text = ""
            Me.lFondo.Text = ""
            Me.lblTipoOperacion.Text = ""
            Me.lblTipoOrden.Text = ""
            PanelDetalleCuponesVencidos(False)
            PanelDetalleDividendosRebatesLiberadas(False)
            PanelDetalleOrdenes(True)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub
    Protected Sub dgListaOC_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaOC.RowCommand
        Dim Row As GridViewRow
        Dim i As Integer
        Try
            displayBotones(divBotonEliminar, "none")
            If e.CommandName = "Seleccionar" Then
                Me.hndCodigoOperacion.Value = String.Empty
                Row = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                i = Row.RowIndex
                Dim poliza As String
                Dim codigoOperacion As String = String.Empty
                Dim iCont As Int64 = dgListaOC.Rows.Count - 1
                dgListaOC.SelectedIndex = Row.RowIndex
                seleccionarMasivoRow_Grilla(dgListaOC, "Seleccionar")
                If i >= 0 And i <= iCont Then
                    dgListaOC.Rows.Item(i).BackColor = System.Drawing.Color.LemonChiffon
                    lblNroTransaccion.Text = dgListaOC.Rows(i).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOC)).Text
                    lFondo.Text = dgListaOC.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolio", dgListaOC)).Text
                    Me.hdnTipoSeleccion.Value = "OC"
                    poliza = dgListaOC.Rows(i).Cells(obtenerIndiceColumna_Grilla("NumeroPoliza", dgListaOC)).Text
                    BindginComisionesOrdenInversion(poliza)
                End If
                displayBotones(divBotonEliminar, "block")
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se implementa columna para desconfirmar la OI | 04/07/18 
            ElseIf e.CommandName = "Eliminar" Then
                Row = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                i = Row.RowIndex
                lblNroTransaccion.Text = dgListaOC.Rows(i).Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOC)).Text
                lFondo.Text = dgListaOC.Rows(i).Cells(obtenerIndiceColumna_Grilla("CodigoPortafolio", dgListaOC)).Text
                lfechaOperacion.Text = dgListaOC.Rows(i).Cells(obtenerIndiceColumna_Grilla("FechaOperacion", dgListaOC)).Text
                Me.hdnTipoSeleccion.Value = "OC"
                ConfirmarJS("¿Está seguro de eliminar la confirmación de la orden de inversión Nro. " + lblNroTransaccion.Text + "?", "document.getElementById('hdRptaDesConfirmar').value = 'SI'; document.getElementById('btnEliminar').click(); ")
            End If
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se implementa columna para desconfirmar la OI | 04/07/18 

        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try
    End Sub
    Public Sub dgListaOC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim chkTemp As CheckBox = CType(sender, CheckBox)
            Dim dgi As GridViewRow
            Dim i As Int32 = 0
            Dim nSeleccionadas As Int64 = 0
            Dim valor As String = String.Empty
            displayBotones(divBotonEliminar, "none")
            If hdnSelect.Value = String.Empty Then
                dgi = CType(chkTemp.Parent.Parent, GridViewRow)
                i = dgi.RowIndex
                If chkTemp.Checked = True Then
                    displayBotones(divBotonEliminar, "block")
                    dgListaOC.SelectedIndex = dgi.RowIndex
                    dgListaOC.Rows(i).BackColor = System.Drawing.Color.LemonChiffon
                Else
                    dgListaOC.SelectedIndex = -1
                    lblNroTransaccion.Text = ""
                    If dgListaOC.Rows(i).RowType = DataControlRowType.DataRow Then
                        dgListaOC.Rows(i).BackColor = System.Drawing.Color.White
                    ElseIf dgListaOC.Rows(i).RowType = DataControlRowType.DataRow Then
                        dgListaOC.Rows(i).BackColor = System.Drawing.Color.White
                    End If
                    nSeleccionadas = (retornarNumSeleccionados(dgListaOC, "chkSelectOC"))
                    If nSeleccionadas > 0 Then displayBotones(divBotonEliminar, "block")
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea evento para modificar grilla de ordenes confirmadas y muestre la columna observaciones cuando suceda un error en la eliminación de confirmaciones masiva | 04/07/18 
    Protected Sub dgListaOC_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgListaOC.RowDataBound
        Dim ibObservacion As ImageButton
        Dim xError As DictionaryEntry
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.BackColor = Drawing.Color.White
                If listaErrores.Count > 0 Then
                    For Each xError In listaErrores
                        If xError.Key = e.Row.Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", dgListaOC)).Text Then
                            ibObservacion = CType(e.Row.FindControl("ibObservacionOC"), ImageButton)
                            ibObservacion.Visible = True
                            ibObservacion.ToolTip = xError.Value
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea evento para modificar grilla de ordenes confirmadas y muestre la columna observaciones cuando suceda un error en la eliminación de confirmaciones masiva | 04/07/18 
    Private Sub ddlFondoOE_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFondoOE.SelectedIndexChanged
        Try
            If Me.ddlFondoOE.SelectedIndex > 0 Then
                Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlFondoOE.SelectedValue.ToString))
                ViewState("Fecha") = Me.tbFechaOperacion.Text.Trim
            Else
                Me.tbFechaOperacion.Text = ""
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try
    End Sub

    Protected Sub dgrComisiones_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgrComisiones.RowDataBound
        Try
            Dim txtComision As TextBox
            Dim hdnCodigoComision As HtmlInputHidden
            Dim hdnOldComision As HtmlInputHidden
            Dim dr As DataRowView
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                dr = CType(e.Row.DataItem, DataRowView)
                If Not (e.Row.FindControl("txtComision") Is Nothing) Then
                    txtComision = CType(e.Row.FindControl("txtComision"), TextBox)
                    txtComision.Text = Decimal.Round(Convert.ToDecimal(dr("ValorCalculado")), 2).ToString()
                    txtComision.Style.Add("text-align", "right")
                End If
                If Not (e.Row.FindControl("hdnCodigoComision") Is Nothing) Then
                    hdnCodigoComision = CType(e.Row.FindControl("hdnCodigoComision"), HtmlInputHidden)
                    hdnCodigoComision.Value = Convert.ToString(dr("CodigoComision"))
                End If
                If Not (e.Row.FindControl("hdnOldComision") Is Nothing) Then
                    hdnOldComision = CType(e.Row.FindControl("hdnOldComision"), HtmlInputHidden)
                    hdnOldComision.Value = Decimal.Round(Convert.ToDecimal(dr("ValorCalculado")), 2).ToString()
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub
    Private Sub btnActualizar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActualizar.Click
        Try
            PanelDetalleSession()
            Dim txtComision As TextBox
            Dim hdnCodigoComision As HtmlInputHidden, hdnOldComision As HtmlInputHidden, newImporteComision As Decimal, oldImporteComision As Decimal,
            codigoComision As String, difComision As Decimal, poliza As String, fondo As String, nroOrden As String, fecha As String
            If hdnTipoSeleccion.Value.Equals("OC") Then
                Dim estado As String = New CuentasPorCobrarBM().ObtenerEstadoOperacion(lblNroTransaccion.Text)
                If estado.Equals("L") Then
                    AlertaJS("La operación ya está liquidada. No puede modificar las comisiones.")
                    Exit Sub
                End If
            End If
            GetDatos(fondo, nroOrden, fecha, tipoInstrumento)
            CargarGrillaOIEjecutadas(fondo, nroOrden, fecha, tipoInstrumento)
            CargarGrillaOIConfirmadas(fondo, nroOrden, fecha, tipoInstrumento)
            If (Me.dgListaOC.Rows.Count = 0) Then
                Return
            End If
            If (Me.dgListaOC.SelectedRow Is Nothing) Then
                Return
            End If
            poliza = Me.dgListaOC.SelectedRow.Cells(obtenerIndiceColumna_Grilla("TipoOrden", dgListaOC)).Text
            If (poliza = "" Or poliza = "&nbsp;") Then
                Return
            End If
            If (dgrComisiones.Rows.Count <= 0) Then
                Return
            End If
            If (lblNroTransaccion.Text = "") Then
                Return
            End If
            For Each item As GridViewRow In dgrComisiones.Rows
                If (item.RowType = ListItemType.Item Or item.RowType = ListItemType.AlternatingItem) Then
                    If Not (item.FindControl("txtComision") Is Nothing) Then
                        txtComision = CType(item.FindControl("txtComision"), TextBox)
                        newImporteComision = Convert.ToDecimal(txtComision.Text)
                    End If
                    If Not (item.FindControl("hdnCodigoComision") Is Nothing) Then
                        hdnCodigoComision = CType(item.FindControl("hdnCodigoComision"), HtmlInputHidden)
                        codigoComision = hdnCodigoComision.Value
                    End If
                    If Not (item.FindControl("hdnOldComision") Is Nothing) Then
                        hdnOldComision = CType(item.FindControl("hdnOldComision"), HtmlInputHidden)
                        oldImporteComision = Convert.ToDecimal(hdnOldComision.Value)
                    End If
                    difComision = oldImporteComision - newImporteComision
                    oOrdenPreOrdenInversionBM.UpdateImpuestosComisionesOrdenPreOrden(ddlFondoOE.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString(), codigoComision, difComision, poliza, Trim(lblNroTransaccion.Text)) 'Modificado por LC 25082008
                End If
            Next
            BindginComisionesOrdenInversion(poliza)
            AlertaJS("El registro ha sido actualizado correctamente")
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub
    Private Sub btnSeleccionar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Try
            Dim grilla As GridView
            Dim chkControl As String = String.Empty
            Dim bBotones As String = String.Empty

            If hdnGridViewSelect.Value = "dgListaOE" Then
                grilla = dgListaOE
                chkControl = "chkSelectPE"
            Else
                grilla = dgListaOC
                chkControl = "chkSelectOC"
            End If
            grilla.SelectedIndex = -1
            lblNroTransaccion.Text = ""
            Dim iCont As Int64 = grilla.Rows.Count - 1
            Dim chk As CheckBox
            If hdnSelect.Value = "2" Then
                If chkControl = "chkSelectPE" Then
                    displayBotones(divBotonConfirmar, "none")
                ElseIf chkControl = "chkSelectOC" Then
                    displayBotones(divBotonEliminar, "none")
                End If
                While iCont >= 0
                    If grilla.Rows(iCont).FindControl(chkControl).GetType Is GetType(CheckBox) Then
                        chk = CType(grilla.Rows(iCont).FindControl(chkControl), CheckBox)
                        If chk.Checked = False And chk.Visible = True Then
                            If grilla.Rows(iCont).RowType = DataControlRowType.DataRow Then
                                grilla.Rows(iCont).BackColor = System.Drawing.Color.White 'Smoke
                            ElseIf grilla.Rows(iCont).RowType = DataControlRowType.DataRow Then
                                grilla.Rows(iCont).BackColor = System.Drawing.Color.White
                            End If
                        End If
                    End If
                    iCont = iCont - 1
                End While
            Else
                While iCont >= 0
                    If grilla.Rows(iCont).FindControl(chkControl).GetType Is GetType(CheckBox) Then
                        chk = CType(grilla.Rows(iCont).FindControl(chkControl), CheckBox)
                        If chk.Checked = True Then
                            grilla.Rows(iCont).BackColor = System.Drawing.Color.LemonChiffon
                            bBotones = "SI"
                        Else
                            grilla.Rows(iCont).BackColor = System.Drawing.Color.White
                        End If
                    End If
                    iCont = iCont - 1
                End While
                If bBotones = "SI" Then
                    If chkControl = "chkSelectPE" Then
                        displayBotones(divBotonConfirmar, "block")
                    ElseIf chkControl = "chkSelectOC" Then
                        displayBotones(divBotonEliminar, "block")
                    End If
                End If
            End If

            hdnSelect.Value = String.Empty
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try
    End Sub

    Private Sub btnAgruparAcciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgruparAcciones.Click
        Response.Redirect("frmAgruparAcciones.aspx")
    End Sub

#End Region
#Region " /* Métodos Personalizados */ "
    Private Sub BindginComisionesOrdenInversion(ByVal poliza As String)
        If (Me.ddlFondoOE.SelectedIndex = -1) Then
            Return
        End If
        Dim dtComisiones As DataTable
        dtComisiones = oOrdenPreOrdenInversionBM.GetComisionesOrdenInversionByPoliza(DatosRequest, ddlFondoOE.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString(), poliza)
        lblTotalComision.Text = Convert.ToString(dtComisiones.Compute("SUM(ValorCalculado)", Nothing))
        If (lblTotalComision.Text <> "") Then
            lblTotalComision.Text = String.Format("{0:N2}", Convert.ToDecimal(lblTotalComision.Text))
        End If
        Me.dgrComisiones.DataSource = dtComisiones
        Me.dgrComisiones.DataBind()
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se modifica el retorno de la función de boolean a string para reutilización de desconfirmación masiva | 11/07/18 
    Private Function verValorizadoExistencia(ByVal varCodigoPortafolio As String, ByVal varFechaOperacion As Decimal, ByVal varNumeroOrden As String) As String
        Dim intCantPoliza As Integer
        Dim liquidado As Boolean
        Dim dsResulAux As DataSet = oOrdenPreOrdenInversionBM.verValorizadoExistencia(varCodigoPortafolio, varFechaOperacion, varNumeroOrden, liquidado, DatosRequest)
        Dim Valida As Integer = 0
        Valida = ValidaOIAccionAgrupada(varNumeroOrden)

        If (Valida > 0) Then
            Return "La Orden N° " + varNumeroOrden + ", pertenece a una acción agrupada, para proceder con la eliminación, debe desagruparla en la opción Carta Acciones."
        End If

        If dsResulAux.Tables.Count > 0 Then
            If dsResulAux.Tables(0).Rows.Count > 0 Then
                intCantPoliza = Convert.ToInt32(dsResulAux.Tables(0).Rows(0)("CANT"))
                If intCantPoliza > 0 Then
                    Return ObtenerMensaje("CONF47")
                ElseIf liquidado Then
                    Return ObtenerMensaje("ALERT176")
                Else
                    Return String.Empty
                End If
            End If
        End If

        Return String.Empty
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se modifica el retorno de la función de boolean a string para reutilización de desconfirmación masiva | 11/07/18 
    Private Sub PanelDetalleAmortizacionesVencidas(ByVal visible As Boolean)
        lblFechaVencimientoAmort.Text = String.Empty
        lblValorNominalLocalAmort.Text = String.Empty
        tbDetalleCA.Visible = visible
    End Sub
    Private Sub PanelDetalleCuponesVencidos(ByVal visible As Boolean)
        lblFechaVencimiento.Text = String.Empty
        lblMontoCuponera.Text = String.Empty
        tblDetalleCV.Visible = False
    End Sub
    Private Sub PanelDetalleDividendosRebatesLiberadas(ByVal visible As Boolean)
        lblFechaVencimientoDRL.Text = String.Empty
        lblTipoOperacionDRL.Text = String.Empty
        lblMontoDRL.Text = String.Empty
        lblMnemonicoDRL.Text = String.Empty
        lblMonedaDRL.Text = String.Empty
        lEstado.Text = String.Empty
        lIdentificador.Text = String.Empty
        tblDetalleDRL.Visible = visible
    End Sub
    Private Sub PanelDetalleOrdenes(ByVal visible As Boolean)
        lblCodigoISIN.Text = String.Empty
        lblTipoOperacion.Text = String.Empty
        lblTipoOrden.Text = String.Empty
        lblNroTransaccion.Text = String.Empty
        tblDetalleOI.Visible = visible
    End Sub

    Private Sub PanelDetalleSession()
        lblCodigoISIN.Text = String.Empty
        lblTipoOrden.Text = String.Empty
        lblNroTransaccion.Text = String.Empty
        lblTipoOperacion.Text = String.Empty
        lCategoria.Text = String.Empty
        lNemonico.Text = String.Empty
        lFondo.Text = String.Empty
        lNombreFondo.Text = String.Empty
    End Sub
    Private Sub CargarGrillaOIEjecutadas(ByVal fondo As String, ByVal nroOrden As String, ByVal fecha As Decimal, Optional ByVal tipoInstrumento As String = "")
        Me.dgListaOE.DataSource = Nothing
        Me.dgListaOE.DataBind()
        Dim dtblDatos As DataTable = oOrdenPreOrdenInversionBM.ListarOIEjecutadasConfirmacion(DatosRequest, fondo, nroOrden, fecha, tipoInstrumento)
        If Not (dtblDatos Is Nothing) Then
            Me.lblCantidadOE.Text = String.Format("({0})", dtblDatos.Rows.Count) + " - " + ddlFondoOE.SelectedItem.Text
        Else
            Me.lblCantidadOE.Text = String.Format("({0})", 0)
        End If
        Me.dgListaOE.DataSource = dtblDatos
        Me.dgListaOE.DataBind()
        MostrarColumna_TipoInstrumento(dgListaOE, tipoInstrumento)

        ViewState("OIEjecutadas") = dtblDatos
    End Sub
    Private Sub CargarGrillaOIConfirmadas(ByVal fondo As String, ByVal nroOrden As String, ByVal fecha As Decimal, ByVal tipoInstrumento As String)
        Me.dgListaOC.DataSource = Nothing
        Me.dgListaOC.DataBind()
        Dim dtblDatos As DataTable = oOrdenPreOrdenInversionBM.ListarOIConfirmadas(DatosRequest, fondo, nroOrden, fecha, tipoInstrumento)
        If Not (dtblDatos Is Nothing) Then
            Me.lblCantidadOIC.Text = String.Format("({0})", dtblDatos.Rows.Count) + " - " + ddlFondoOE.SelectedItem.Text
        Else
            Me.lblCantidadOIC.Text = String.Format("({0})", 0)
        End If
        dgListaOC.DataSource = dtblDatos
        dgListaOC.DataBind()
        MostrarColumna_TipoInstrumento(dgListaOC, tipoInstrumento)
        ViewState("OIConfirmadas") = dtblDatos
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se implementa procedimiento para mostrar las columnas correspondientes por tipo de instrumento | 04/07/18 
    Private Sub MostrarColumna_TipoInstrumento(ByVal grilla As GridView, ByVal tipoInstrumento As String)
        ReiniciarColumnas_Grilla(grilla)
        Select Case tipoInstrumento
            Case "DP"
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoNominalOrdenado", grilla), "M. Nominal Operación", String.Empty) 'Monto Constitución
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Nombre", grilla), "Tipo Tasa", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Plazo", grilla), "Plazo", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("CodigoMoneda", grilla), "Moneda", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TasaPorcentaje", grilla), "Tasa", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla), "Monto Operación", String.Empty) 'Monto al Vencimiento
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Contraparte", grilla), "Intermediario", String.Empty) 'Contraparte 
            Case "BO"
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TipoOperacion", grilla), "Tipo de Operación", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoNominalOrdenado", grilla), "Monto Nominal Ordenado", String.Empty) 'Monto Nominal
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla), "Monto Neto Operación", String.Empty) ' Monto Final
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("InteresCorridoNegociacion", grilla), "Interés Corrido", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("CantidadOperacion", grilla), "Cantidad", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("FechaLiquidacion", grilla), "Fecha Vencimiento", String.Empty) 'Fecha de Liquidación
            Case "AC"
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TipoOperacion", grilla), "Tipo de Operación", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("CantidadOperacion", grilla), "Nro. Acciones Ordenadas", String.Empty) 'Cantidad
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Precio", grilla), "Precio", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoOperacion", grilla), "Monto Operación", String.Empty)  'Monto Nominal
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Contraparte", grilla), "Intermediario", String.Empty) 'Contraparte 
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("FechaLiquidacion", grilla), "Fecha Vencimiento", String.Empty) 'Fecha de Liquidación
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TotalComisiones", grilla), "Comisiones", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla), "Monto Neto Operación", String.Empty) 'Monto Total
            Case "FD"
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TipoOperacion", grilla), "Tipo de Operación", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoNominalOrdenado", grilla), "Monto Negociado", String.Empty) 'Monto
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TipoCambioFuturo", grilla), "Tipo Cambio Futuro", String.Empty) 'Tipo Cambio Forward
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TipoCambioSpot", grilla), "Tipo Cambio Spot", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("CodigoISIN", grilla), "Código ISIN", String.Empty) 'Ingreso Código ISIN
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Contraparte", grilla), "Intermediario", String.Empty) 'Contraparte 
            Case "OR"
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoNominalOrdenado", grilla), "Monto Neto", String.Empty) ' Monto al Contado(Compra)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("CodigoMnemonico", grilla), "Código Mnemónico", String.Empty) 'Acción en Garantía
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("CantidadOperacion", grilla), "Cantidad", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Plazo", grilla), "Plazo Vencimiento", String.Empty) 'Plazo
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla), "Monto Neto Vcto.", String.Empty) 'Monto a plazo (Venta)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Contraparte", grilla), "Intermediario", String.Empty) 'Contraparte 
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TotalComisiones", grilla), "Comisiones (Venta+Compra)", String.Empty)
            Case "CV"
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoOrigen", grilla), "Monto Divisa Negociada", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoDestino", grilla), "Monto", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TipoCambio", grilla), "Tipo Cambio", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Plazo", grilla), "Plazo", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Contraparte", grilla), "Intermediario", String.Empty) 'Contraparte 
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("FechaLiquidacion", grilla), "Fecha de Liquidación", String.Empty)
            Case "FI", "FM"
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TipoOperacion", grilla), "Tipo de Operación", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Precio", grilla), "Precio", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Bolsa", grilla), "Bolsa", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("CantidadOrdenado", grilla), "Cuotas", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Contraparte", grilla), "Intermediario", String.Empty) 'Contraparte 
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("FechaLiquidacion", grilla), "Fecha Vencimiento", String.Empty) 'Fecha de Liquidación
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla), "Monto Operación", String.Empty)
            Case "PC"
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TipoOperacion", grilla), "Tipo de Operación", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("CantidadOperacion", grilla), "Nro Papeles", String.Empty) 'Cantidad
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Precio", grilla), "Precio Negociación %", String.Empty) 'Precio
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("InteresCorridoNegociacion", grilla), "Interés Corrido Negociación", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Contraparte", grilla), "Intermediario", String.Empty) 'Contraparte 
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Bolsa", grilla), "Bolsa", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("FechaLiquidacion", grilla), "Fecha Vencimiento", String.Empty) 'Fecha de Liquidación
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TotalComisiones", grilla), "Comisiones", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla), "Monto Neto Operación", String.Empty) 'Monto Total           
            Case Else
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("TipoOperacion", grilla), "Tipo de Operación", String.Empty)
                If ddlTipoInstrumento.SelectedItem.Text.ToUpper.Contains("SELECCIONE") Then MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Categoria", grilla), "Tipo Instrumento", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Descripcion", grilla), "Descripción", String.Empty)
                If obtenerIndiceColumna_Grilla("NumeroPoliza", grilla) < 0 Then MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Estado", grilla), "Estado", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("CodigoMoneda", grilla), "Moneda", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoOperacion", grilla), "Monto Operación", String.Empty)
                MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla), "Monto Neto", String.Empty)
        End Select

        If ddlFondoOE.SelectedItem.Text.ToUpper.Contains("TODOS") Then
            MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("FechaOperacion", grilla), "Fecha Operación", String.Empty)
            MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Fondo", grilla), "Portafolio", String.Empty)
            MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("NumeroTransaccion", grilla), "Nro. orden", String.Empty)
        End If

        If obtenerIndiceColumna_Grilla("NumeroPoliza", grilla) >= 0 Then
            MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("NumeroPoliza", grilla), "Número Poliza", String.Empty)
            MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("CantidadOperacion", grilla), "Cantidad Unidades", String.Empty)
            MostrarColumna_Grilla(grilla, obtenerIndiceColumna_Grilla("Precio", grilla), "Precio", String.Empty)

        End If

    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se implementa procedimiento para mostrar las columnas correspondientes por tipo de instrumento | 04/07/18 
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Mostrar Columnas de acuerdo a requerimiento | 04/07/18 
    Private Sub MostrarColumna_Grilla(ByVal grilla As GridView, ByVal ColActual As Integer, ByVal nombreCol As String, ByVal opcionMostrar As String)
        grilla.Columns(ColActual).ItemStyle.CssClass = opcionMostrar
        grilla.Columns(ColActual).HeaderStyle.CssClass = opcionMostrar
        If nombreCol <> String.Empty Then grilla.HeaderRow.Cells(ColActual).Text = nombreCol
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Mostrar Columnas de acuerdo a requerimiento | 04/07/18 
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Obtener índice de columna de acuerdo al nombre de la columna en grilla | 04/07/18 
    Private Function obtenerIndiceColumna_Grilla(ByVal nomCol As String, ByVal grilla As GridView) As Integer
        Dim columna As DataControlFieldCollection = grilla.Columns
        Dim indiceCol As Integer = -1
        For Each celda As DataControlField In columna
            If TypeOf celda Is System.Web.UI.WebControls.BoundField Then
                If CType(celda, BoundField).DataField = nomCol Then
                    indiceCol = columna.IndexOf(celda)
                    Exit For
                End If
            End If
        Next
        'For Each celda As TableCell In grilla.HeaderRow.Cells
        '    If celda.Text.Equals(nomCol) Then
        '        indiceCol = grilla.HeaderRow.Cells.GetCellIndex(celda)
        '        Exit For
        '    End If
        'Next
        Return indiceCol
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Obtener índice de columna de acuerdo al nombre de la columna en grilla | 04/07/18 
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Reinciar el hidden de las columnas de la grilla a estado inicial | 04/07/18 
    Private Sub ReiniciarColumnas_Grilla(ByVal grilla As GridView)
        Dim i, x As Integer
        If grilla.Columns(3).HeaderText.ToString.ToUpper = "OBSERVACIONES" Then
            If grilla.Columns(3).HeaderStyle.CssClass = String.Empty Then x = 4 Else x = 3
        ElseIf grilla.Columns(2).HeaderText.ToString.ToUpper = "OBSERVACIONES" Then
            If grilla.Columns(2).HeaderStyle.CssClass = String.Empty Then x = 3 Else x = 2
        End If
        For i = x To grilla.Columns.Count - 1
            grilla.Columns(i).HeaderStyle.CssClass = "ocultarCol"
            grilla.Columns(i).ItemStyle.CssClass = "ocultarCol"
        Next
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Reinciar el hidden de las columnas de la grilla a estado inicial | 04/07/18 
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Detecta el cambio de ddlFondoOE| 04/07/18 
    Private Sub validarCambioOpcion()
        If (Me.ddlFondoOE.SelectedIndex > 0) Then
            EjecutarJS("$('#divtbFechaOperacion').removeAttr('style');")
            EjecutarJS("$('#divlblFechaOperacion').removeAttr('style');")
        Else
            Me.tbFechaOperacion.Text = ""
            EjecutarJS("$('#divtbFechaOperacion').css('display', 'none');")
            EjecutarJS("$('#divlblFechaOperacion').css('display', 'none');")
        End If
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Detecta el cambio de ddlFondoOE| 04/07/18 
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Seleccionar/Deseleccionar todos los registros de la grilla | 04/07/18 
    Private Sub seleccionarMasivoRow_Grilla(ByVal grilla As GridView, ByVal chkBuscar As String)
        Dim iCont As Integer = grilla.Rows.Count - 1
        Dim chk As CheckBox
        While iCont >= 0
            If grilla.Rows(iCont).FindControl(chkBuscar).GetType Is GetType(CheckBox) Then
                chk = CType(grilla.Rows(iCont).FindControl(chkBuscar), CheckBox)
                If chk.Checked = True Then
                    chk.Checked = False
                    If grilla.Rows(iCont).RowType = DataControlRowType.DataRow Then
                        grilla.Rows(iCont).BackColor = System.Drawing.Color.White
                    ElseIf grilla.Rows(iCont).RowType = DataControlRowType.DataRow Then
                        grilla.Rows(iCont).BackColor = System.Drawing.Color.White
                    End If
                End If
            End If
            iCont = iCont - 1
        End While
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Seleccionar/Deseleccionar todos los registros de la grilla | 04/07/18 
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Obtener Código de Poliza de acuerdo al tipo de Instrumento | 04/07/18 
    Private Function obteneNroPoliza(ByVal codigoOrden As String, ByVal tipoInstrumento As String) As String
        obteneNroPoliza = String.Empty
        Select Case tipoInstrumento.ToUpper
            Case "BO", "AC", "CV", "FM", "FI", "PC"
                obteneNroPoliza = Right(codigoOrden, 4)
            Case "DP", "OR"
                obteneNroPoliza = Right(codigoOrden, 5)
            Case Else

        End Select
        Return obteneNroPoliza
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Obtener Código de Poliza de acuerdo al tipo de Instrumento | 04/07/18 
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Obtener Validar Campos por tipo Instrumento antes de Confirmar| 04/07/18 
    Public Function validarCampos_Grilla(ByVal grillaFila As GridViewRow, ByVal grilla As GridView) As String
        Dim strMensaje As String = String.Empty
        If (grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoOperacion", grilla)).Text.Trim = "Dividendo" Or grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoOperacion", grilla)).Text.Trim = "Rebate" Or grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoOperacion", grilla)).Text.Trim = "Liberada") Then
            strMensaje = ""
        Else
            Select Case grillaFila.Cells(obtenerIndiceColumna_Grilla("Categoria", grilla)).Text
                Case "DP"
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoNominalOrdenado", grilla)).Text = "0.00" Then
                        strMensaje = " - Monto Nominal Operación" + Environment.NewLine
                    End If
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("Nombre", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje += " - Tipo Tasa" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("Plazo", grilla)).Text.Trim() = "0" Then
                        strMensaje += " - Días Plazo" + Environment.NewLine
                    End If
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("CodigoMoneda", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje += " - Moneda" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla)).Text = "0.00" Then
                        strMensaje += " - Monto Operación" + Environment.NewLine
                    End If
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("Contraparte", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje += " - Intermediario" + Environment.NewLine
                    End If
                    If strMensaje = String.Empty Then strMensaje = validarPolizaExistencia(grillaFila.Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", grilla)).Text, _
                                                                                           obteneNroPoliza(grillaFila.Cells(obtenerIndiceColumna_Grilla("NumeroTransaccion", grilla)).Text, "DP"))
                Case "BO"
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoOperacion", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje = " - Tipo Operación" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoNominalOrdenado", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Monto Nominal Ordenado" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Monto Operación" + Environment.NewLine
                    End If
                    'If grillaFila.Cells(obtenerIndiceColumna_Grilla("InteresCorridoNegociacion", grilla)).Text.Trim() = "0.00" Then
                    '    strMensaje += " - Interés Corrido" + Environment.NewLine
                    'End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("CantidadOperacion", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Nro. Papeles" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("FechaLiquidacion", grilla)).Text.Trim() = "00/00/0000" Then
                        strMensaje += " - Fecha Vencimiento" + Environment.NewLine
                    End If
                Case "AC"
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoOperacion", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje = " - Tipo Operación" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("CantidadOperacion", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Nro. Acciones Ordenadas" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("Precio", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Precio" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoOperacion", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Monto Operación" + Environment.NewLine
                    End If
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("Contraparte", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje += " - Intermediario" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("FechaLiquidacion", grilla)).Text.Trim() = "00/00/0000" Then
                        strMensaje += " - Fecha Vencimiento" + Environment.NewLine
                    End If
                    'If grillaFila.Cells(obtenerIndiceColumna_Grilla("TotalComisiones", grilla)).Text.Trim() = string.empty Then
                    '    strMensaje = " - Comisiones" + Environment.NewLine
                    'End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Monto Neto Operación" + Environment.NewLine
                    End If
                Case "FD"
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoOperacion", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje = " - Tipo Operación" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoNominalOrdenado", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Monto Negociado" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoCambioFuturo", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Tipo Cambio Futuro" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoCambioSpot", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Tipo Cambio Spot" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoCambioSpot", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Tipo Cambio Spot" + Environment.NewLine
                    End If
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("Contraparte", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje += " - Intermediario" + Environment.NewLine
                    End If
                Case "OR"
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoNominalOrdenado", grilla)).Text.Trim() = "0.00" Then
                        strMensaje = " - Monto Neto" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("CodigoMnemonico", grilla)).Text.Trim() = "-" Then
                        strMensaje += " - Código Mnemónico" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("CantidadOperacion", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Cantidad" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("Plazo", grilla)).Text.Trim() = "0" Then
                        strMensaje += " - Plazo Vencimiento" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Monto Neto Vcto." + Environment.NewLine
                    End If
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("Contraparte", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje += " - Intermediario" + Environment.NewLine
                    End If
                    'If grillaFila.Cells(obtenerIndiceColumna_Grilla("TotalComisiones", grilla)).Text.Trim() = string.empty Then
                    '    strMensaje = " - Comisiones" + Environment.NewLine
                    'End If
                Case "CV"
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoOrigen", grilla)).Text.Trim() = "0" Then
                        strMensaje = " - Monto Divisa Negociada" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoDestino", grilla)).Text.Trim() = "0" Then
                        strMensaje += " - Monto" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoCambio", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Tipo Cambio" + Environment.NewLine
                    End If
                    'If grillaFila.Cells(obtenerIndiceColumna_Grilla("Plazo", grilla)).Text.Trim() = "0" Then
                    '    strMensaje += " - Plazo" + Environment.NewLine
                    'End If
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("Contraparte", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje += " - Intermediario" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("FechaLiquidacion", grilla)).Text.Trim() = "00/00/0000" Then
                        strMensaje += " - Fecha Liquidación" + Environment.NewLine
                    End If
                Case "FI", "FM"
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoOperacion", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje = " - Tipo Operación" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("Precio", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Precio" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("Bolsa", grilla)).Text.Trim() = "-" Then
                        strMensaje += " - Mercado" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("CantidadOrdenado", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Nro. Cuotas Ordenado" + Environment.NewLine
                    End If
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("Contraparte", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje += " - Intermediario" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("FechaLiquidacion", grilla)).Text.Trim() = "00/00/0000" Then
                        strMensaje += " - Fecha Vencimiento" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Monto Neto Operacion" + Environment.NewLine
                    End If
                Case "PC"
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("TipoOperacion", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje = " - Tipo Operación" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("CantidadOperacion", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Nro Papeles" + Environment.NewLine
                    End If
                    'If grillaFila.Cells(obtenerIndiceColumna_Grilla("Precio", grilla)).Text.Trim() = "0.00" Then
                    '    strMensaje += " - Precio Negociación %" + Environment.NewLine
                    'End If
                    'If grillaFila.Cells(obtenerIndiceColumna_Grilla("InteresCorridoNegociacion", grilla)).Text.Trim() = "0.00" Then
                    '    strMensaje += " - Interés Corrido Negociación" + Environment.NewLine
                    'End If
                    If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("Contraparte", grilla)).Text.Trim()) = String.Empty Then
                        strMensaje += " - Intermediario" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("Bolsa", grilla)).Text.Trim() = "-" Then
                        strMensaje += " - Mercado" + Environment.NewLine
                    End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("FechaLiquidacion", grilla)).Text.Trim() = "00/00/0000" Then
                        strMensaje += " - Fecha Vencimiento" + Environment.NewLine
                    End If
                    'If HttpUtility.HtmlDecode(grillaFila.Cells(obtenerIndiceColumna_Grilla("TotalComisiones", grilla)).Text.Trim()) = string.empty Then
                    '    strMensaje = " - Comisiones" + Environment.NewLine
                    'End If
                    If grillaFila.Cells(obtenerIndiceColumna_Grilla("MontoNetoOperacion", grilla)).Text.Trim() = "0.00" Then
                        strMensaje += " - Monto Neto Operacion" + Environment.NewLine
                    End If
                Case "FA"
                    strMensaje = ""
                Case Else
                    strMensaje = "El tipo de Instrumento no está definido para confirmar masivamente."
            End Select

        End If
        Return strMensaje
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Obtener Validar Campos por tipo Instrumento antes de Confirmar| 04/07/18 
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Reiniciar Columna de Observaciones para una nueva búsqueda | 04/07/18 
    Public Sub reiniciarColObservacion()
        dgListaOE.Columns(3).HeaderStyle.CssClass = "ocultarCol"
        dgListaOE.Columns(3).ItemStyle.CssClass = "ocultarCol"
        dgListaOC.Columns(2).HeaderStyle.CssClass = "ocultarCol"
        dgListaOC.Columns(2).ItemStyle.CssClass = "ocultarCol"
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Reiniciar Columna de Observaciones para una nueva búsqueda | 04/07/18 
    Public Sub CargarCombos()
        Try
            Dim tablaClaseInstrumento As New DataTable
            Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
            Dim oPortafolioBM As New PortafolioBM
            tablaClaseInstrumento = oClaseInstrumentoBM.Listar(Me.DatosRequest).Tables(0)
            HelpCombo.LlenarComboBox(ddlFondoOE, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "TODOS")
            HelpCombo.LlenarComboBox(ddlTipoInstrumento, tablaClaseInstrumento, "Categoria", "categoriaDescripcion", True)
        Catch ex As Exception
            AlertaJS("Error al cargar combos.")
        End Try
    End Sub
    Private Sub Ir(ByVal nportaolio As String, ByVal portafolio As String, ByVal codigoNemonico As String, ByVal montoNominalLocal As String, ByVal fecha As String, ByVal identificador As String, ByVal estado As String, ByVal moneda As String)
        Dim StrURL As String
        StrURL = "frmDividendosRebatesLiberadasVencimiento.aspx?Portafolio=" + nportaolio + "&codigoPortafolio=" + portafolio + "&identificador=" + identificador + "&montoNominalLocal=" + montoNominalLocal + "&codigoNemonico=" + codigoNemonico + "&fechaVencimiento=" + fecha.ToString() + "&estado=" + estado.ToString() + "&moneda=" + lblMonedaDRL.Text.ToString()
        ShowDialogPopup(StrURL, "950", "600", "200")
    End Sub
    Private Sub IrCOI(ByVal portafolio As String, ByVal codigoNemonico As String, ByVal montoNominalLocal As String, ByVal fecha As Decimal, ByVal secuencial As String, ByVal ordenInversion As String)
        Dim StrURL As String
        StrURL = "frmCuponesVencimiento.aspx?codigoPortafolio=" + portafolio + "&secuencial=" + secuencial + "&montoNominalLocal=" + montoNominalLocal + "&codigoNemonico=" + codigoNemonico + "&fechaVencimiento=" + fecha.ToString() + "&ordenInversion=" + ordenInversion
        ShowDialogPopup(StrURL)
    End Sub
    Private Sub Ir(ByVal clasificacion As String, ByVal StrNOrden As String, ByVal StrFondo As String)
        Dim StrURL As String
        StrURL = Pagina(clasificacion).Replace("#", StrFondo).Replace("%", "&").Replace("@", StrNOrden)
        ShowDialogPopup(StrURL)
    End Sub
    Private Sub IrCOA(ByVal codigoOrden As String, ByVal portafolio As String, ByVal codigoNemonico As String, ByVal montoNominalLocal As String, ByVal fecha As Decimal, ByVal secuencial As String, ByVal ordenInversion As String)
        Dim StrURL As String
        StrURL = "frmAmortizacionesVencimiento.aspx?codigoOrden=" + codigoOrden + "&codigoPortafolio=" + portafolio + "&secuencial=" + secuencial + "&montoNominalLocal=" + montoNominalLocal + "&codigoNemonico=" + codigoNemonico + "&fechaVencimiento=" + fecha.ToString() + "&ordenInversion=" + ordenInversion
        ShowDialogPopup(StrURL)
    End Sub
    Private Sub ShowDialogPopup(ByVal StrURL As String, Optional ByVal width As String = "950", Optional ByVal heigth As String = "600", Optional ByVal left As String = "125")
        Dim FechaOperacion As String
        Dim NroOrden As String
        Dim Portafolio As String
        FechaOperacion = UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text).ToString
        NroOrden = Me.txtNroOrdenOE.Text
        If (Me.ddlFondoOE.SelectedIndex = 0) Then
            Portafolio = ""
        Else
            Portafolio = Me.ddlFondoOE.SelectedValue.ToString
        End If
        ViewState("Fondo") = Portafolio
        ViewState("NroOrden") = NroOrden
        ' ViewState("Fecha") = FechaOperacion
        Session("CuponVence") = "1"
        EjecutarJS("showModalDialog('" & StrURL & "', '1081', '550','');")
    End Sub
    Private Function Pagina(ByVal clasificacion As String) As String
        Dim dvwPaginas As DataView = DirectCast(ViewState("PaginasOI"), DataTable).DefaultView
        dvwPaginas.RowFilter = "Clasificacion = '" + clasificacion + "'"
        Return IIf(dvwPaginas.Count > 0, dvwPaginas(0)(1).ToString, String.Empty)
    End Function
    Private Function ConsultarPaginasPorOI() As Boolean
        Try
            Dim dsPaginas As New DataSet
            dsPaginas.ReadXml(MapPath("") + "\Configuracion\TConfirmacionOI.xml")
            ViewState("PaginasOI") = dsPaginas.Tables(0)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetDatos(ByRef fondo As String, ByRef nroOrden As String, ByRef fecha As String, ByRef tipoInstrumento As String) As Boolean
        Dim arrFecha() As String
        Try
            Dim dato As DateTime
            fondo = IIf(ddlFondoOE.SelectedIndex = 0, "", ddlFondoOE.SelectedValue)
            nroOrden = txtNroOrdenOE.Text.Trim
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se valida que cuando tipo de Portafolio es opción "Todos" la fecha debe ser vacía o valor "0"| 02/07/18 
            If fondo.Trim <> String.Empty Then
                dato = Convert.ToDateTime(Me.tbFechaOperacion.Text.Trim())
                arrFecha = Me.tbFechaOperacion.Text.Trim().Split("/")
                fecha = arrFecha(2) + arrFecha(1) + arrFecha(0)
            Else
                fecha = "000000"
            End If
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se valida que cuando tipo de Portafolio es opción "Todos" la fecha debe ser vacía o valor "0"| 02/07/18 
        Catch ex As Exception
            AlertaJS("El Formato de la fecha es incorrecto")
            Return False
        End Try

        ViewState("Fondo") = fondo
        ViewState("NroOrden") = nroOrden
        '   ViewState("Fecha") = fecha
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega el nuevo parámetro de búsqueda| 02/07/18 
        tipoInstrumento = IIf(ddlTipoInstrumento.SelectedIndex = 0, "", ddlTipoInstrumento.SelectedValue)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega el nuevo parámetro de búsqueda | 02/07/18 
        Return True
    End Function
    Private Sub displayBotones(ByVal divNombre As System.Web.UI.HtmlControls.HtmlGenericControl, ByVal opcion As String)
        divNombre.Attributes.Add("style", "text-align:right;display:" + opcion)
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Retorna el número de seleccionados por checkbox en grilla | 04/07/18 
    Private Function retornarNumSeleccionados(ByVal grilla As GridView, ByVal chkNombre As String) As Integer
        Dim iCont As Int64 = grilla.Rows.Count - 1
        Dim chk As CheckBox
        retornarNumSeleccionados = 0
        While iCont >= 0 And retornarNumSeleccionados < 1
            If grilla.Rows(iCont).FindControl(chkNombre).GetType Is GetType(CheckBox) Then
                chk = CType(grilla.Rows(iCont).FindControl(chkNombre), CheckBox)
                If chk.Checked = True Then
                    retornarNumSeleccionados += 1
                End If
            End If
            iCont = iCont - 1
        End While
        Return retornarNumSeleccionados
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Retorna el número de seleccionados por checkbox en grilla | 04/07/18 
    Private Function validarPolizaExistencia(ByVal varCodigoOrden As String, ByVal vartbNPoliza As String) As String
        Dim intCantPoliza As Integer
        Dim dsResulAux As DataSet = oOrdenPreOrdenInversionBM.validarPolizaExistencia(varCodigoOrden, vartbNPoliza, String.Empty, DatosRequest)
        If dsResulAux.Tables.Count > 0 Then
            If dsResulAux.Tables(0).Rows.Count > 0 Then
                intCantPoliza = Convert.ToInt32(dsResulAux.Tables(0).Rows(0)("CANT"))
                If intCantPoliza > 0 Then
                    Return (ObtenerMensaje("CONF46"))
                Else
                    Return String.Empty
                End If
            End If
        End If
        Return "Ocurrió un error en el retorno de validarPolizaExistencia."
    End Function

    Function ValidaOIAccionAgrupada(ByVal CodigoOrden As String) As Integer
        Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim Valida As Integer
        Valida = oOrdenPreOrdenInversionBM.ValidaOI_Agrupada(CodigoOrden)
        Return Valida
    End Function

#End Region

End Class
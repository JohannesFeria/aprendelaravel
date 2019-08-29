Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices.Marshal

Imports iTextSharp.text
Imports iTextSharp.text.pdf

Imports Microsoft.Office.Core
Imports System.Collections.Generic
Imports CrystalDecisions.Shared
Imports ParametrosSIT

Partial Class Modulos_Inversiones_frmIngresoMasivoOperacionFT
    Inherits BasePage


#Region "Inicializacion"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarPagina()
        End If
    End Sub

#End Region

#Region "Eventos Pagina"
    Private Sub ibBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim decFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            Dim decFechaActual As Decimal = Convert.ToDecimal(DateTime.Now.ToString("yyyyMMdd"))

            If decFechaOperacion = hdFechaNegocio.Value And hdPuedeNegociar.Value = "1" Then
                HabilitaControles(True)
            ElseIf (decFechaOperacion > hdFechaNegocio.Value And decFechaOperacion = decFechaActual) And hdPuedeNegociar.Value = "1" Then
                HabilitaControles(False, True)
            Else
                HabilitaControles(False)
            End If

            ViewState("decFechaOperacion") = decFechaOperacion
            Dim strCodigoClaseInstrumento As String = ddlClaseInstrumento.SelectedValue
            Dim strOperador As String = ddlOperador.SelectedValue
            Dim strCodigoNemonico As String = tbCodigoMnemonico.Text
            Dim strEstado As String = ddlEstado.SelectedValue
            ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
            ViewState("strOperador") = ddlOperador.SelectedValue
            ViewState("strCodigoNemonico") = tbCodigoMnemonico.Text
            ViewState("strEstado") = ddlEstado.SelectedValue
            CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), decFechaOperacion, strCodigoClaseInstrumento, "", strCodigoNemonico, strOperador, strEstado)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ibGrabar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGrabar.Click
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
            Dim bolValidaCampos As Boolean = False
            Dim NroOper As String = ""
            Dim lbCodigoPrevOrden As Label
            Dim tbNemonico As TextBox
            Dim ddlVencimientoMes As DropDownList
            Dim tbVencimientoAno As TextBox
            Dim ddlOperacion As DropDownList
            Dim tbHora As TextBox
            Dim tbCantidad As TextBox
            Dim tbPrecio As TextBox
            Dim tbTotalOrden As TextBox
            Dim ddlCondicion As DropDownList
            Dim tbIntermediario As TextBox
            Dim hdIntermediario As HtmlControls.HtmlInputHidden
            Dim ddlContacto As DropDownList
            Dim ddlPlazaN As DropDownList
            Dim tbCantidadOperacion As TextBox
            Dim tbPrecioOperacion As TextBox
            Dim tbMontoOperacion As TextBox
            Dim tbFondo1 As TextBox
            Dim tbFondo2 As TextBox
            Dim tbFondo3 As TextBox
            Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
            Dim strMensaje As String = ""
            Dim tbHoraEje As TextBox
            Dim tbFechaLiquidacion As TextBox
            Dim hdCambio As HtmlControls.HtmlInputHidden
            'ini HDG OT 67627 20130531
            Dim oTrazabilidadOperacionBE As New TrazabilidadOperacionBE
            Dim oRowT As TrazabilidadOperacionBE.TrazabilidadOperacionRow
            Dim hdCambioTraza As HtmlControls.HtmlInputHidden
            Dim hdCambioTrazaFondo As HtmlControls.HtmlInputHidden
            Dim hdNemonicoTrz As HtmlControls.HtmlInputHidden
            Dim hdOperacionTrz As HtmlControls.HtmlInputHidden
            Dim hdCantidadTrz As HtmlControls.HtmlInputHidden
            Dim hdPrecioTrz As HtmlControls.HtmlInputHidden
            Dim hdIntermediarioTrz As HtmlControls.HtmlInputHidden
            Dim hdCantidadOperacionTrz As HtmlControls.HtmlInputHidden
            Dim hdPrecioOperacionTrz As HtmlControls.HtmlInputHidden
            Dim hdFondo1Trz As HtmlControls.HtmlInputHidden
            Dim hdFondo2Trz As HtmlControls.HtmlInputHidden
            Dim hdFondo3Trz As HtmlControls.HtmlInputHidden
            Dim strCambioTrazaFondo As String = ""
            Dim dt As New DataTable
            Dim dr As DataRow

            Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                    hdCambio = CType(fila.FindControl("hdCambio"), HtmlControls.HtmlInputHidden)
                    hdCambioTraza = CType(fila.FindControl("hdCambioTraza"), HtmlControls.HtmlInputHidden)
                    hdCambioTrazaFondo = CType(fila.FindControl("hdCambioTrazaFondo"), HtmlControls.HtmlInputHidden)
                    If fila.Cells(2).Text = PREV_OI_INGRESADO Then
                        If Not lbCodigoPrevOrden Is Nothing And hdCambio.Value = "1" Then
                            hdCambio.Value = ""
                            oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                            tbNemonico = CType(fila.FindControl("tbNemonico"), TextBox)
                            ddlVencimientoMes = CType(fila.FindControl("ddlVencimientoMes"), DropDownList)
                            tbVencimientoAno = CType(fila.FindControl("tbVencimientoAno"), TextBox)
                            ddlOperacion = CType(fila.FindControl("ddlOperacion"), DropDownList)
                            tbHora = CType(fila.FindControl("tbHora"), TextBox)
                            tbCantidad = CType(fila.FindControl("tbCantidad"), TextBox)
                            tbPrecio = CType(fila.FindControl("tbPrecio"), TextBox)
                            tbTotalOrden = CType(fila.FindControl("tbTotal"), TextBox)
                            ddlCondicion = CType(fila.FindControl("ddlCondicion"), DropDownList)
                            tbIntermediario = CType(fila.FindControl("tbIntermediario"), TextBox)
                            hdIntermediario = CType(fila.FindControl("hdIntermediario"), HtmlControls.HtmlInputHidden)
                            ddlContacto = CType(fila.FindControl("ddlContacto"), DropDownList)
                            ddlPlazaN = CType(fila.FindControl("ddlPlazaN"), DropDownList)
                            tbCantidadOperacion = CType(fila.FindControl("tbCantidadOperacion"), TextBox)
                            tbPrecioOperacion = CType(fila.FindControl("tbPrecioOperacion"), TextBox)
                            tbMontoOperacion = CType(fila.FindControl("tbTotalOperacion"), TextBox)
                            tbFondo1 = CType(fila.FindControl("tbFondo1"), TextBox)
                            tbFondo2 = CType(fila.FindControl("tbFondo2"), TextBox)
                            tbFondo3 = CType(fila.FindControl("tbFondo3"), TextBox)
                            hdClaseInstrumento = CType(fila.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                            tbHoraEje = CType(fila.FindControl("tbHoraEje"), TextBox)
                            tbFechaLiquidacion = CType(fila.FindControl("tbFechaLiquidacion"), TextBox)
                            NroOper = fila.Cells(1).Text

                            bolValidaCampos = False
                            If tbCantidad.Text <> "" And _
                               tbPrecio.Text <> "" And _
                               tbPrecioOperacion.Text <> "" And _
                               tbCantidadOperacion.Text <> "" And _
                               tbNemonico.Text <> "" And _
                               hdIntermediario.Value.ToString <> "" Then
                                If ValidarOperacionPorClaseInstrumento(ParametrosSIT.FUTURO, ddlOperacion.SelectedValue) Then
                                    If IsNumeric(tbTotalOrden.Text) Then
                                        If ValidarAsignaciones(CType(IIf(tbFondo1.Text = "", 0, tbFondo1.Text), Decimal), CType(IIf(tbFondo2.Text = "", 0, tbFondo2.Text), Decimal), CType(IIf(tbFondo3.Text = "", 0, tbFondo3.Text), Decimal), CType(tbTotalOrden.Text, Decimal)) Then
                                            bolValidaCampos = True
                                        Else
                                            strMensaje = "Las asignaciones por fondo es incorrecto! "
                                        End If
                                    Else
                                        strMensaje = "Ingrese correctamente el registro"
                                    End If
                                Else
                                    strMensaje = "La operacion ingresada no aplica para el tipo de instrumento a negociar! "
                                End If
                            Else
                                If tbNemonico.Text = "" Then
                                    strMensaje = strMensaje + "- Ingrese Nemónico. \n"
                                End If
                                If tbCantidad.Text = "" Then
                                    strMensaje = strMensaje + "- Ingrese Cant. Instrumento. \n"
                                End If
                                If tbVencimientoAno.Text = "" Then
                                    strMensaje = strMensaje + "- Ingrese Año. \n"
                                End If
                                If tbPrecio.Text = "" Then
                                    strMensaje = strMensaje + "- Ingrese Precio. \n"
                                End If
                                If hdIntermediario.Value.ToString() = "" Then
                                    strMensaje = strMensaje + "- Ingrese Intermediario. \n"
                                End If
                                If tbCantidadOperacion.Text = "" Then
                                    strMensaje = strMensaje + "- Ingrese Cant. Instrumento Ejecución. \n"
                                End If
                                If tbPrecioOperacion.Text = "" Then
                                    strMensaje = strMensaje + "- Ingrese Precio Ejecución. \n"
                                End If
                                If (hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) Then
                                    strMensaje = strMensaje + "- Seleccione Tipo. \n"
                                End If
                                If tbVencimientoAno.Text = "" Then
                                    strMensaje = strMensaje + "- Ingrese Año. \n"
                                End If
                            End If
                            If bolValidaCampos = True Then

                                oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                                oRow.CodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                                oRow.FechaOperacion = ViewState("decFechaOperacion")
                                oRow.HoraOperacion = tbHora.Text
                                oRow.CodigoContacto = ddlContacto.SelectedValue
                                oRow.CodigoNemonico = tbNemonico.Text
                                oRow.VencimientoMes = ddlVencimientoMes.SelectedValue
                                oRow.VencimientoAno = tbVencimientoAno.Text
                                oRow.CodigoOperacion = ddlOperacion.SelectedValue
                                oRow.ClaseInstrumentoFx = ParametrosSIT.FUTURO
                                oRow.Cantidad = CType(tbCantidad.Text, Decimal)
                                oRow.Precio = CType(tbPrecio.Text, Decimal)
                                oRow.MontoNominal = CType(tbPrecio.Text, Decimal)
                                oRow.TipoCondicion = ddlCondicion.SelectedValue
                                oRow.CodigoTercero = hdIntermediario.Value
                                oRow.CodigoContacto = ddlContacto.SelectedValue
                                oRow.HoraEjecucion = tbHoraEje.Text
                                oRow.CodigoPlaza = ddlPlazaN.SelectedValue
                                oRow.CantidadOperacion = CType(tbCantidadOperacion.Text, Decimal)
                                oRow.PrecioOperacion = CType(tbPrecioOperacion.Text, Decimal)
                                oRow.MontoOperacion = CType(tbMontoOperacion.Text, Decimal)
                                oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
                                oRow.FechaContrato = oRow.FechaLiquidacion

                                If ValidarAsignaciones(CType(IIf(tbFondo1.Text = "", 0, tbFondo1.Text), Decimal), CType(IIf(tbFondo2.Text = "", 0, tbFondo2.Text), Decimal), CType(IIf(tbFondo3.Text = "", 0, tbFondo3.Text), Decimal), CType(IIf(tbCantidadOperacion.Text = "", 0, tbCantidadOperacion.Text), Decimal)) Then
                                    oRow.AsignacionF1 = CType(IIf(tbFondo1.Text = "", 0, tbFondo1.Text), Decimal)
                                    oRow.AsignacionF2 = CType(IIf(tbFondo2.Text = "", 0, tbFondo2.Text), Decimal)
                                    oRow.AsignacionF3 = CType(IIf(tbFondo3.Text = "", 0, tbFondo3.Text), Decimal)
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                                    'ini HDG OT 67627 20130531
                                    If hdCambioTraza.Value = "1" Then
                                        hdCambioTraza.Value = ""
                                        hdNemonicoTrz = CType(fila.FindControl("hdNemonicoTrz"), HtmlControls.HtmlInputHidden)
                                        hdOperacionTrz = CType(fila.FindControl("hdOperacionTrz"), HtmlControls.HtmlInputHidden)
                                        hdCantidadTrz = CType(fila.FindControl("hdCantidadTrz"), HtmlControls.HtmlInputHidden)
                                        hdPrecioTrz = CType(fila.FindControl("hdPrecioTrz"), HtmlControls.HtmlInputHidden)
                                        hdIntermediarioTrz = CType(fila.FindControl("hdIntermediarioTrz"), HtmlControls.HtmlInputHidden)
                                        hdCantidadOperacionTrz = CType(fila.FindControl("hdCantidadOperacionTrz"), HtmlControls.HtmlInputHidden)
                                        hdPrecioOperacionTrz = CType(fila.FindControl("hdPrecioOperacionTrz"), HtmlControls.HtmlInputHidden)
                                        hdFondo1Trz = CType(fila.FindControl("hdFondo1Trz"), HtmlControls.HtmlInputHidden)
                                        hdFondo2Trz = CType(fila.FindControl("hdFondo2Trz"), HtmlControls.HtmlInputHidden)
                                        hdFondo3Trz = CType(fila.FindControl("hdFondo3Trz"), HtmlControls.HtmlInputHidden)
                                        tbIntermediario = CType(fila.FindControl("tbIntermediario"), TextBox)
              
                                        If hdCambioTrazaFondo.Value = "1" Then
                                            strCambioTrazaFondo = "1"
                                        End If
                                        oRowT = oTrazabilidadOperacionBE.TrazabilidadOperacion.NewTrazabilidadOperacionRow()
                                        oRowT.FechaOperacion = ViewState("decFechaOperacion").ToString()
                                        oRowT.Correlativo = fila.Cells(1).Text
                                        oRowT.Estado = fila.Cells(2).Text
                                        oRowT.TipoOperacion = TIPO_OPER_PREVORDEN
                                        oRowT.CodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                                        oRowT.CodigoOrden = String.Empty
                                        oRowT.CodigoNemonico = tbNemonico.Text
                                        oRowT.CodigoOperacion = ddlOperacion.SelectedValue
                                        oRowT.Cantidad = CType(tbCantidad.Text, Decimal)
                                        oRowT.Precio = CType(tbPrecio.Text, Decimal)
                                        oRowT.CodigoTercero = hdIntermediario.Value
                                        oRowT.CantidadEjecucion = CType(tbCantidadOperacion.Text, Decimal)
                                        oRowT.PrecioEjecucion = CType(tbPrecioOperacion.Text, Decimal)
                                        oRowT.ModoIngreso = MODO_ING_MASIVO
                                        oRowT.Proceso = _PROCESO_TRAZA.Grabar
                                        oRowT.AsignacionF1 = CType(IIf(tbFondo1.Text = "", 0, tbFondo1.Text), Decimal)
                                        oRowT.AsignacionF2 = CType(IIf(tbFondo2.Text = "", 0, tbFondo2.Text), Decimal)
                                        oRowT.AsignacionF3 = CType(IIf(tbFondo3.Text = "", 0, tbFondo3.Text), Decimal)
                                        oRowT.MotivoCambio = MOTIVO_MODIFICAR_TRAZA
                                        oRowT.Comentarios = COMENTARIO_MODIFICA_TRAZA
                                        oTrazabilidadOperacionBE.TrazabilidadOperacion.AddTrazabilidadOperacionRow(oRowT)
                                        oTrazabilidadOperacionBE.TrazabilidadOperacion.AcceptChanges()
                                    End If

                                Else
                                    If strMensaje <> "" Then
                                        EjecutarJS("alert('Nro. Operación " + NroOper + ":\n" + strMensaje + "')")
                                        Exit For
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next

            If oPrevOrdenInversionBE.PrevOrdenInversion.Rows.Count > 0 Then
                oPrevOrdenInversionBM.Modificar(oPrevOrdenInversionBE, DatosRequest)
                oRow = Nothing
                If oTrazabilidadOperacionBE.TrazabilidadOperacion.Rows.Count > 0 Then
                    oPrevOrdenInversionBM.InsertarTrazabilidad(oTrazabilidadOperacionBE, PROCESO_TRAZA1, DatosRequest)
                    oRowT = Nothing
                End If
                CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
            End If

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ibValidar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnValidar.Click
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
            Dim count As Integer = 0
            Dim lbCodigoPrevOrden As Label

            Dim decNProceso As Decimal = 0
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Session("Login"))

            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                    If Not lbCodigoPrevOrden Is Nothing Then
                        'PROCESAR VALIDACION
                        '-------------------------------------------
                        Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                        Dim decCodigoPrevOrden As Decimal

                        If chkSelect.Checked = True Then
                            decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                            oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest)
                            count = count + 1
                        End If
                    End If
                End If
            Next

            'SE PROCEDE A VALIDAR
            If count > 0 Then
                Limites.VerificaExcesoLimites(Me.Usuario, ParametrosSIT.TR_DERIVADOS.ToString(), decNProceso)

                Dim dt As New DataTable
                If dt.Rows.Count > 0 Then
                    Session("dtListaExcesos") = dt
                    EjecutarJS(UIUtility.MostrarPopUp("frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" + ParametrosSIT.TR_DERIVADOS.ToString(), "ValidacionExcesos", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                End If
            Else
                AlertaJS("Seleccione los registros a validar!")
            End If

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ibValidarTrader_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnValidarTrader.Click
        Try
            'VALIDACION DE EXCESOS POR TRADER
            Dim dtValidaTrader As New DataTable
            Dim oLimiteTradingBM As New LimiteTradingBM
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM

            Dim count As Decimal = 0
            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                    Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                    Dim decCodigoPrevOrden As Decimal
                    If chkSelect.Checked = True Then
                        If fila.Cells(2).Text = "ING" Then
                            decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                            oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest)
                            count = count + 1
                        End If
                    End If
                End If
            Next
            If count > 0 Then
                dtValidaTrader = oLimiteTradingBM.SeleccionarValidacionExcesosTrader(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), Session("Login"), DatosRequest, ParametrosSIT.FUTURO).Tables(0)  'HDG 20111228
                If dtValidaTrader.Rows.Count > 0 Then
                    Session("dtValidaTrader") = dtValidaTrader
                    EjecutarJS(UIUtility.MostrarPopUp("frmValidacionExcesosTrader.aspx?TipoRenta=" & ParametrosSIT.TR_DERIVADOS.ToString() & "&Instrumento=" & ParametrosSIT.FUTURO, "ValidacionExcesosTrader", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                End If
            Else
                AlertaJS("alert('Seleccione los registros a validar!")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ibAprobar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        Try
            'EJECUCION DE ORDENES DE INVERSION
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim strCodPrevOrden As String = ""

            Dim count As Decimal = 0
            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                    Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                    Dim decCodigoPrevOrden As Decimal
                    If chkSelect.Checked = True Then
                        If fila.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                            decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                            oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest)
                            count = count + 1
                            strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text & "|"
                        End If
                    End If
                End If
            Next

            If count > 0 Then
                strCodPrevOrden = strCodPrevOrden.Substring(0, strCodPrevOrden.Length - 1)
                EjecutarOrdenInversion(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), strCodPrevOrden, False, ParametrosSIT.FUTURO)
            Else
                AlertaJS("Seleccione el registro a ejecutar!")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
#End Region

#Region "Private Methods"

#Region "Usuados en el Load"

    Private Sub CargarPagina()
        'Me.ibBuscarNemonico.Attributes.Add("onclick", "javascript:showPopupMnemonico();") Miguel
        CargarCombos()

        hdFechaNegocio.Value = UIUtility.ObtenerFechaNegocio(ParametrosSIT.PORTAFOLIO_MULTIFONDOS)
        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(hdFechaNegocio.Value)

        ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
        ViewState("strOperador") = ddlOperador.SelectedValue
        ViewState("strCodigoNemonico") = tbCodigoMnemonico.Text
        ViewState("strEstado") = ddlEstado.SelectedValue

        Dim dtCondicion As New DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtCondicion = oParametrosGeneralesBM.ListarCondicionPrevOI().Tables(0)
        Session("dtCondicion") = dtCondicion

        Dim dtOperacion As New DataTable
        Dim oOperacionBM As New OperacionBM
        dtOperacion = oOperacionBM.SeleccionarporClaseinstrumento("Futuro", ParametrosSIT.ESTADO_ACTIVO).Tables(0)
        Session("dtOperacion") = dtOperacion

        Dim dtMoneda As New DataTable
        Dim oMonedaBM As New MonedaBM
        dtMoneda = oMonedaBM.Listar(ESTADO_ACTIVO).Tables(0)
        Session("dtMoneda") = dtMoneda

        Dim dtPlazaN As New DataTable
        Dim oPlazaBM As New PlazaBM
        dtPlazaN = oPlazaBM.ListarxOrden(DatosRequest).Tables(0)
        Session("dtPlazaN") = dtPlazaN

        Dim dtTipoFondo As DataTable
        dtTipoFondo = New ParametrosGeneralesBM().ListarFondosInversion(DatosRequest, "M")
        Session("dtTipoFondo") = dtTipoFondo

        Dim dtVencimientoMes As DataTable
        dtVencimientoMes = New ParametrosGeneralesBM().ListarVencimiento(DatosRequest)
        Session("dtVencimientoMes") = dtVencimientoMes

        CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"))

        hdPuedeNegociar.Value = New PersonalBM().VerificaPermisoNegociacion(Session("Login"))
        If hdPuedeNegociar.Value = "0" Then
            HabilitaControles(False)
        End If
    End Sub

    Private Sub CargarCombos()
        CargarOperadores()
        CargaClaseInstrumento()
        CargarEstados()
    End Sub

    Private Sub CargarOperadores()
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim dt As New DataTable
        dt = oPrevOrdenInversionBM.SeleccionarOperadores(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlOperador, dt, "UsuarioCreacion", "UsuarioCreacion", True)
        ddlOperador.SelectedIndex = 0
    End Sub

    Private Sub CargaClaseInstrumento()
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
        Dim dsClaseInstrumento As DataSet
        dsClaseInstrumento = oClaseInstrumentoBM.SeleccionarClaseInstrumentoPorTipoRenta(ParametrosSIT.TR_DERIVADOS, DatosRequest)
        HelpCombo.LlenarComboBox(ddlClaseInstrumento, dsClaseInstrumento.Tables(0), "Codigo", "Descripcion", True)
        ddlClaseInstrumento.SelectedIndex = 0
    End Sub

    Private Sub CargarEstados()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As New DataTable
        dt = oParametrosGeneralesBM.Listar(ParametrosSIT.ESTADO_PREV_OI, DatosRequest)
        HelpCombo.LlenarComboBox(ddlEstado, dt, "Valor", "Nombre", True)
        ddlEstado.SelectedIndex = 0
    End Sub

    Private Sub CargarGrilla(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, _
        Optional ByVal strCodigoClaseInstrumento As String = "", _
        Optional ByVal strCodigoTipoInstrumentoSBS As String = "", _
        Optional ByVal strCodigoNemonico As String = "", _
        Optional ByVal strOperador As String = "", _
        Optional ByVal strEstado As String = "")

        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim ds As DataSet
        ds = oPrevOrdenInversionBM.SeleccionarPorFiltroFuturo(strTipoRenta, decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, strOperador, strEstado, DatosRequest)

        Datagrid1.DataSource = ds
        Datagrid1.DataBind()

    End Sub

    Private Sub HabilitaControles(ByVal habilita As Boolean, Optional ByVal AntesApertura As Boolean = False)   'HDG OT 64480 20120120
        btnGrabar.Enabled = habilita
        btnValidar.Enabled = habilita
        btnValidarTrader.Enabled = habilita
        btnAprobar.Enabled = habilita
        Datagrid1.Enabled = habilita
        If AntesApertura Then
            btnGrabar.Enabled = True
            Datagrid1.Enabled = True
        End If
    End Sub
#End Region

#Region "Usados en DataGrid"
    Private Function ValidarNemonico(ByVal strNemonico As String) As Boolean
        Dim oValoresBM As New ValoresBM
        Dim bolResult As Boolean = False
        Dim strTipoRenta As String = ""

        strTipoRenta = oValoresBM.SeleccionarTipoRentaPorCodigoNemonico(strNemonico)
        If strTipoRenta <> "" Then
            If ParametrosSIT.TIPO_RENTA_VARIABLE.ToString.Replace("_", " ") = strTipoRenta Then
                bolResult = True
            End If
        End If
        Return bolResult
    End Function

    Private Function ValidarIntermediario(ByVal strCodigoTercero As String) As Boolean
        Dim oTercerosBM As New TercerosBM
        Dim oTercerosBE As New TercerosBE
        Dim bolResult As Boolean = False
        oTercerosBE = oTercerosBM.Seleccionar(strCodigoTercero, DatosRequest)
        If oTercerosBE.Tables(0).Rows.Count > 0 Then
            If ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO = CType(oTercerosBE.Tables(0).Rows(0)("ClasificacionTercero"), String) Then
                bolResult = True
            End If
        End If
        Return bolResult
    End Function

    Private Function ValidarAsignaciones(ByVal decAsignacionF1 As Decimal, ByVal decAsignacionF2 As Decimal, ByVal decAsignacionF3 As Decimal, ByVal decCantidadOperacion As Decimal) As Boolean
        Dim bolResult As Boolean = False

        Dim decSumAsignaciones As Decimal = 0
        decSumAsignaciones = decAsignacionF1 + decAsignacionF2 + decAsignacionF3

        If decSumAsignaciones <> 0 Then
            If decSumAsignaciones = 100 Then
                bolResult = True
            Else
                If decSumAsignaciones = decCantidadOperacion Then
                    bolResult = True
                End If
            End If
        End If

        Return bolResult
    End Function

    Private Function ValidarOperacionPorClaseInstrumento(ByVal strClaseInstrumento As String, ByVal strCodigoOperacion As String) As Boolean
        Dim bolResult As Boolean = False
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dtAux As New DataTable
        Dim dtOperacion As New DataTable
        Dim oOperacionBM As New OperacionBM
        dtAux = oParametrosGeneralesBM.SeleccionarPorFiltro(ParametrosSIT.CLASE_INSTRUMENTO_FTO, "", strClaseInstrumento, "", DatosRequest)
        If dtAux.Rows.Count > 0 Then
            dtOperacion = oOperacionBM.SeleccionarporClaseinstrumento(dtAux.Rows(0)("Comentario").ToString(), ParametrosSIT.ESTADO_ACTIVO).Tables(0)
            If dtOperacion.Rows.Count > 0 Then
                For Each fila As DataRow In dtOperacion.Rows
                    If strCodigoOperacion = fila("codigoOperacion").ToString() Then
                        bolResult = True
                    End If
                Next
            End If
        End If
        Return bolResult
    End Function

    Private Function validaFechas(ByVal fechaOperacion As Decimal, ByVal fechaLiquidacion As Decimal) As Boolean
        If fechaOperacion > fechaLiquidacion Then
            Return False
        Else
            Return True
        End If
    End Function
#End Region

#End Region

#Region "66056 - Modificacion: JZAVALA"

    ''' <summary>66056 - JZAVALA.
    ''' EVENTO EXPORTAR.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ibExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            GenerarReporteConsolidadoFuturo()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    ''' <summary>66056 - JZAVALA.
    ''' GENERAR EXCEL REPORTE: Consolidado de Operaciónes de Compra y Venta de Futuros efectuada por AFP HORIZONTE
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GenerarReporteConsolidadoFuturo()
        Try
            Dim sFile As String, sTemplate As String

            Dim dtConsolidadoFuturos As New DataTable
            Dim dtResumen As New DataTable
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oDs As DataSet
            oDs = oPrevOrdenInversionBM.GenerarReporteConsolidado(ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"), DatosRequest)
            dtConsolidadoFuturos = oDs.Tables(0)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "FT_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            Dim n As Integer
            Dim n2 As Long
            Dim dr As DataRow
            Dim oExcel As New Excel.Application
            Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
            Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
            Dim oCells As Excel.Range
            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaPrevOrdenInversionCON.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells
            oSheet.SaveAs(sFile)
            n = 14
            For Each dr In dtConsolidadoFuturos.Rows
                n2 = n + 1
                oSheet.Rows(n & ":" & n).Copy()
                oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                oSheet.Application.CutCopyMode = False
                oCells(n, 1) = dr("CONTA")
                oCells(n, 2) = dr("CODIGO_INSTRUMENTO")
                oCells(n, 3) = dr("VENCIMIENTO")
                oCells(n, 4) = dr("ANIO")
                oCells(n, 5) = dr("INTERMEDIARIO")
                oCells(n, 6) = dr("TIPO_OPERACION")
                oCells(n, 7) = dr("MONEDA")
                oCells(n, 8) = dr("ASIGNACIONF1")
                oCells(n, 9) = dr("ASIGNACIONF2")
                oCells(n, 10) = dr("ASIGNACIONF3")
                oCells(n, 11) = dr("MARGENINICIAL")
                oCells(n, 12) = dr("MARGENMANTENIMIENTO")
                oCells(n, 13) = dr("SALDO_INICIAL")
                oCells(n, 14) = dr("DEPOSITO_T_HORIZONTE")
                oCells(n, 15) = dr("DEPOSITO_T_INTERMEDIARIO")
                oCells(n, 16) = dr("SALDO_FINAL")
                oCells(n, 17) = dr("CODIGOISIN")
                oCells(10, 3) = tbFechaOperacion.Text
                n = n + 1
            Next
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp) 'HDG 2012011
            oBook.Save()
            oBook.Close()
            oExcel.Quit()
            ReleaseComObject(oCells) : ReleaseComObject(oSheet)
            ReleaseComObject(oSheets) : ReleaseComObject(oBook)
            ReleaseComObject(oBooks) : ReleaseComObject(oExcel)
            oExcel = Nothing : oBooks = Nothing : oBook = Nothing
            oSheets = Nothing : oSheet = Nothing : oCells = Nothing
            System.GC.Collect()
            EjecutarJS("window.open('" & sFile.Replace("\", "\\") & "');")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Dim pRutas As String
    Dim rutas As New System.Text.StringBuilder

    ''' <summary>66056 - JZAVALA.
    ''' EVENTO IMPRIMIR. CARGAR EL REPORTE CONSOLIDADO EN CRYSTAL REPORT.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ibImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM
        Dim ds As DataSet
        Dim dtOI As DataTable
        Dim lbCodigoPrevOrden As Label
        Dim lbClase As Label
        Dim tbNemonico As TextBox
        Dim strCodigoOrden As String = ""
        Dim strPortafolioSBS As String = ""
        Dim strMoneda As String = ""
        Dim strOperacion As String = ""
        Dim strCodigoISIN As String = ""
        Dim strCodigoSBS As String = ""
        Dim chkSelect As CheckBox
        Try
            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)  'HDG 20120402
                    If fila.Cells(2).Text = PREV_OI_EJECUTADO And chkSelect.Checked = True Then
                        lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                        Dim strCodigoPrevOrden = lbCodigoPrevOrden.Text
                        lbClase = CType(fila.FindControl("lbClase"), Label)
                        tbNemonico = CType(fila.FindControl("tbNemonico"), TextBox)
                        ds = oPrevOrdenInversion.SeleccionarImprimir_PrevOrdenInversion(lbCodigoPrevOrden.Text, DatosRequest)
                        dtOI = ds.Tables(0)

                        For Each fila2 As DataRow In dtOI.Rows
                            strCodigoOrden = fila2("CodigoOrden")
                            strPortafolioSBS = fila2("CodigoPortafolioSBS")
                            strMoneda = fila2("Moneda")
                            strOperacion = fila2("Operacion")
                            strCodigoISIN = fila2("CodigoISIN")
                            strCodigoSBS = fila2("CodigoSBS")
                            Session("dtdatosoperacion") = Nothing
                            GenerarLlamado(strCodigoOrden, strPortafolioSBS, lbClase.Text.ToUpper, strOperacion, strMoneda, strCodigoISIN, strCodigoSBS, tbNemonico.Text, strCodigoPrevOrden)
                        Next
                    End If
                End If
            Next
            CrearMultiCartaPDF(rutas.ToString())
        Catch ex As Exception
            UIUtility.PublicarEvento("Llamado masivo renta fija - ex.Message = " & ex.Message & " ++ ex.StackTrace = " & ex.StackTrace)
            AlertaJS(ex.Message.ToString)
            For Each savedDoc As String In pRutas.Split(New Char() {"&"})
                If File.Exists(savedDoc) Then
                    File.Delete(savedDoc)
                End If
            Next
        End Try
    End Sub

    ''' <summary>66056 - JZAVALA.
    ''' METODO PARA CARGAR EL REPORTE CONSOLIDADO EN CRYSTAL REPORT.
    ''' </summary>
    ''' <param name="codigo"></param>
    ''' <param name="portafolio"></param>
    ''' <param name="clase"></param>
    ''' <param name="operacion"></param>
    ''' <param name="moneda"></param>
    ''' <param name="isin"></param>
    ''' <param name="sbs"></param>
    ''' <param name="mnemonico"></param>
    ''' <remarks></remarks>
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, _
                              ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, _
                              ByVal strCodigoPrevOrden As String)
        Dim strtitulo, strnemonico, strisin, strsbs, strsubtitulo, strcodigo1, strcodigo, strportafolio, strclase, strmoneda, stroperacion, strOperacion_, strmnemonicotemp As String
        Dim dscaract As New dscaracteristicas
        Dim dttemp As DataTable

        Dim drcar As DataRow
        strclase = clase
        strnemonico = mnemonico
        strisin = isin
        strsbs = sbs
        strmnemonicotemp = mnemonico
        strportafolio = portafolio
        strcodigo1 = codigo

        Select Case clase
            Case "Fondos mutuos en el exterior"
                strclase = "ORDENES DE FONDO"
            Case "Fondos de inversión"
                strclase = "ORDENES DE FONDO"
        End Select

        Dim oOIFormulas As New OrdenInversionFormulasBM
        If strclase = "Opciones derivadas - futuros".ToUpper() Then
            '******CARGAR LOS DATOS DE LA OPERACION***************
            Dim dsDatosOperacionPrevOrden As New DataSet
            Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
            dsDatosOperacionPrevOrden = oOrdenInversionBM.ListarDatosOperacionPorCodigoPrevOrdenInversion(codigo, DatosRequest)
            '*****************************************************
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = dsDatosOperacionPrevOrden.Tables(0)
            Dim dsValorFuturo As New DataSet
            Dim drValor As DataRow
            dsValorFuturo = oOIFormulas.SeleccionarCaracValor_Futuros(mnemonico, DatosRequest)
            If dsValorFuturo.Tables(0).Rows.Count > 0 Then
                drValor = dsValorFuturo.Tables(0).NewRow
                drValor = dsValorFuturo.Tables(0).Rows(0)
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_MargenInicial"), String)
                drcar("Val2") = CType(drValor("val_MargenInicial"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car3") = CType(drValor("lbl_FechaEmision"), String)
                drcar("Val3") = CType(drValor("val_FechaEmision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_MargenInicial"), String)
                drcar("Val4") = CType(drValor("val_MargenInicial"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car5") = CType(drValor("lbl_FechaVenc"), String)
                drcar("Val5") = CType(drValor("val_FechaVenc"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car6") = CType(drValor("lbl_FechaVenc"), String)
                drcar("Val6") = CType(drValor("val_FechaVenc"), String).Replace(UIUtility.DecimalSeparator, ".")
                dscaract.Tables(0).Rows.Add(drcar)
            End If
        End If
        Dim sNNeg As String = ""
        If strclase.Substring(1) = "OPERACIONES DERIVADAS - FORWARD DIVISAS" Or strclase.Substring(1) = "COMPRA/VENTA MONEDA EXTRANJERA" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = Session("dtdatosoperacionSW" & strclase.Substring(0, 1))
            sNNeg = strclase.Substring(0, 1)
            strclase = strclase.Substring(1)
        End If

        'ASIGNAR LOS DATOS DE LA OPERACION.
        Dim dttempoperacion As New DataTable
        If Not Session("dtdatosoperacion") Is Nothing Then dttempoperacion = Session("dtdatosoperacion")
        stroperacion = operacion
        Dim dsoper As New dsDatosOperacion
        Dim droper As DataRow
        For Each dr As DataRow In dttempoperacion.Rows
            droper = dsoper.Tables(0).NewRow
            droper("c1") = dr("c1")
            If (dr("FECHA_OPERACION").ToString() = "-") Then
                droper("v1") = "-"
            Else
                droper("v1") = UIUtility.ConvertirDecimalAStringFormatoFecha(dr("FECHA_OPERACION"))
            End If
            droper("c2") = dr("c2")
            If (dr("FECHA_VENCIMIENTO").ToString() = "-") Then
                droper("v2") = "-"
            Else
                droper("v2") = UIUtility.ConvertirDecimalAStringFormatoFecha(dr("FECHA_OPERACION"))
            End If
            droper("c3") = dr("c3")
            droper("v3") = dr("HORA_OPERACION")
            droper("c4") = dr("c4")
            droper("v4") = dr("HORA_EJECUCION")
            droper("c5") = dr("c5")
            droper("v5") = dr("NUMERO_CONTRATOS_ORDENADOS")
            droper("c6") = dr("c6")
            droper("v6") = dr("NUMERO_CONTRATOS_OPERACION")
            droper("c7") = dr("c7")
            droper("v7") = dr("PRECIO")
            droper("c8") = dr("c8")
            droper("v8") = dr("MONTO_OPERACION")
            droper("c9") = dr("c9")
            droper("v9") = dr("MERCADO")
            droper("c10") = dr("c10")
            droper("v10") = dr("TIPO_CODICION")
            droper("c11") = dr("c11")
            droper("v11") = dr("INTERMEDIARIO")
            droper("c12") = dr("c12")
            droper("v12") = dr("CONTACTO")
            droper("c13") = dr("c13")
            droper("v13") = dr("ANIO_VENCIMIENTO")
            droper("c14") = dr("c14")
            droper("v14") = dr("MES_VENCIMIENTO")
            dsoper.Tables(0).Rows.Add(droper)
        Next
        '*****************************************************
        Dim oStream As New System.IO.MemoryStream
        Dim Cro As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim Archivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Llamado/RptLlamadoConsolidado.rpt"
        strcodigo = strcodigo1
        strmoneda = moneda
        '-----------------------------------------------------------------------------------------------
        'Validar si se trata de una Traspaso de Instrumento entre Fondos
        '-----------------------------------------------------------------------------------------------
        Dim ordenes As String() = strcodigo.Split("-")
        Dim ordenOrigen As String = String.Empty
        Dim ordenDestino As String = String.Empty
        Dim esTraspaso As Boolean = False
        If ordenes.Length > 1 Then
            esTraspaso = True
            ordenOrigen = ordenes(0).ToString()
            ordenDestino = ordenes(1).ToString()
        End If
        '-----------------------------------------------------------------------------------------------
        Try
            Dim StrNombre As String = "Usuario"
            Dim strusuario, strcorrelativo As String
            If strportafolio <> "MULTIFONDO" Then
                If esTraspaso Then
                    strtitulo = "Orden de Inversion: (Origen) Nro - " + ordenOrigen + " , (Destino) Nro - " + ordenDestino
                Else
                    strtitulo = "Orden de Inversion Nro - " + strcodigo
                End If
            Else
                If esTraspaso Then
                    strtitulo = "PreOrden de Inversion: Origen Nro - " + (ordenOrigen) + " , (Destino) Nro - " + ordenDestino
                Else
                    strtitulo = "PreOrden de Inversion Nro - " + strcodigo
                End If
            End If

            strsubtitulo = strclase + " - " + stroperacion

            Dim dscomisiones As New DataSet
            Dim dscomi As New dsdatoscomisiones
            Dim drcomi As DataRow
            dscomisiones = UIUtility.ObtenerTablaimpuestosComisionesGuardado(strcodigo, strportafolio) 'IMPORTES Y COMISIONES.
            If (dscomisiones.Tables(0).Rows.Count <= 0) Then
                dscomisiones.Tables(0).Rows.Add("", "", "", "", "", "", "", "")
                For Each drcomisiones As DataRow In dscomisiones.Tables(0).Rows
                    drcomi = dscomi.Tables(0).NewRow
                    drcomi("Descripcion1") = drcomisiones("Descripcion1")
                    drcomi("ValorOcultoComision1") = drcomisiones("ValorOcultoComision1")
                    drcomi("PorcentajeComision1") = drcomisiones("PorcentajeComision1")
                    drcomi("Descripcion2") = drcomisiones("Descripcion2")
                    drcomi("ValorOcultoComision2") = drcomisiones("ValorOcultoComision2")
                    drcomi("PorcentajeComision2") = drcomisiones("PorcentajeComision2")
                    dscomi.Tables(0).Rows.Add(drcomi)
                Next
            Else
                For Each drcomisiones As DataRow In dscomisiones.Tables(0).Rows
                    drcomi = dscomi.Tables(0).NewRow
                    drcomi("Descripcion1") = drcomisiones("Descripcion1").ToString().Substring(0, 1).ToUpper() + drcomisiones("Descripcion1").ToString().Substring(1, drcomisiones("Descripcion1").ToString().Length - 1).ToLower() 'drcomisiones("Descripcion1")
                    drcomi("ValorOcultoComision1") = drcomisiones("ValorOcultoComision1") 'IIf(drcomisiones("ValorOcultoComision1") Is DBNull.Value, "", Format(CType(drcomisiones("ValorOcultoComision1"), Decimal), "##,##0.0000000"))
                    drcomi("PorcentajeComision1") = drcomisiones("PorcentajeComision1").ToString().Substring(0, 1).ToUpper() + drcomisiones("PorcentajeComision1").ToString().Substring(1, drcomisiones("PorcentajeComision1").ToString().Length - 1).ToLower() 'drcomisiones("PorcentajeComision1") + " :"
                    drcomi("Descripcion2") = drcomisiones("Descripcion2").ToString().Substring(0, 1).ToUpper() + drcomisiones("Descripcion2").ToString().Substring(1, drcomisiones("Descripcion2").ToString().Length - 1).ToLower() 'drcomisiones("Descripcion2")
                    drcomi("ValorOcultoComision2") = drcomisiones("ValorOcultoComision2") 'IIf(drcomisiones("ValorOcultoComision2") Is DBNull.Value, "", Format(CType(drcomisiones("ValorOcultoComision2"), Decimal), "##,##0.0000000"))
                    drcomi("PorcentajeComision2") = drcomisiones("PorcentajeComision2") + " :"
                    dscomi.Tables(0).Rows.Add(drcomi)
                Next
            End If

            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            strusuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)
            Cro.Load(Archivo)
            Dim oOrdenInverion As New OrdenPreOrdenInversionBM()
            Dim dsFirma As New DsFirma
            Dim drFirma As DsFirma.FirmaRow
            Dim dr2 As DataRow
            Dim dtFirmas As New DataTable
            dtFirmas = oOrdenInverion.ObtenerFirmasLlamadoOI(ordenes(0).ToString(), UIUtility.ObtenerFechaNegocio("MULTIFONDO"), DatosRequest).Tables(0)
            dr2 = dtFirmas.Rows(0)

            drFirma = CType(dsFirma.Firma.NewFirmaRow(), DsFirma.FirmaRow)
            drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(0), String)))
            drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(1), String)))
            drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(2), String)))
            drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(3), String)))
            drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(4), String)))
            drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(5), String)))
            drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(6), String)))
            drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(7), String)))
            drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(8), String)))
            drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(9), String)))
            drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(10), String)))
            drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(11), String)))
            drFirma.Firma13 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(12), String)))
            drFirma.Firma14 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(13), String)))

            dsFirma.Firma.AddFirmaRow(drFirma)
            dsFirma.Firma.AcceptChanges()
            Cro.SetDataSource(dsFirma)
            Cro.OpenSubreport("RptCaracteristicas").SetDataSource(dscaract)
            Cro.OpenSubreport("RptDatosOperacion").SetDataSource(dsoper)
            Cro.OpenSubreport("RptDatosComisiones").SetDataSource(dscomi)

            dsoper = New dsDatosOperacion
            Dim dttemptotal As DataTable = CType(Session("dtdatosoperacion"), DataTable)
            For Each dr As DataRow In dttemptotal.Rows
                If CType(dr("c19"), String) = "" Then
                    Exit For
                End If
                droper = dsoper.Tables(0).NewRow
                droper("c19") = dr("c19")
                droper("v19") = dr("TOTAL_COMISIONES")
                droper("c20") = dr("c20")
                droper("v20") = dr("MONTO_NETO_OPERACION")
                droper("c21") = dr("c21")
                droper("v21") = dr("PRECIO_PROMEDIO")
                dsoper.Tables(0).Rows.Add(droper)
            Next

            Cro.OpenSubreport("RptTotalOperacion").SetDataSource(dsoper)
            Cro.SetParameterValue("@Titulo", strtitulo)
            Cro.SetParameterValue("@Subtitulo", strsubtitulo)
            Cro.SetParameterValue("@Fondo", "Portafolio: " & strportafolio)
            Cro.SetParameterValue("@Moneda", "Moneda: " & strmoneda)

            If esTraspaso Then
                Cro.SetParameterValue("@Operacion", "Operación: " & stroperacion & "-" & Constantes.M_TRASPASO_INGRESO_)
            Else
                Cro.SetParameterValue("@Operacion", "Operación: " & stroperacion)
            End If

            If strmnemonicotemp <> "" Or strmnemonicotemp <> Nothing Then
                Cro.SetParameterValue("@MnemonicoReporte", "Mnemónico Temporal: " & strmnemonicotemp)
            Else
                Cro.SetParameterValue("@MnemonicoReporte", "")
            End If
            'ISIN
            If strisin <> "" Or strisin <> Nothing Then
                Cro.SetParameterValue("@CodigoIsin", "Codigo Isin: " & strisin)
            Else
                Cro.SetParameterValue("@CodigoIsin", "")
            End If
            'SBS
            If strsbs <> "" Or strsbs <> Nothing Then
                Cro.SetParameterValue("@CodigoSBS", "Codigo SBS: " & strsbs)
            Else
                Cro.SetParameterValue("@CodigoSBS", "")
            End If
            'NEMONICO
            If strnemonico <> "" Or strnemonico <> Nothing Then
                Cro.SetParameterValue("@CodigoNemonico", "Codigo Mnemónico: " & strnemonico)
            Else
                Cro.SetParameterValue("@CodigoNemonico", "")
            End If

            Dim rutaArchivo As String

            If Not (Cro Is Nothing) Then
                rutaArchivo = "c:\temp\" & strcodigo1 & " - " & Now.ToString(" - yyyyMMdd hhmmss") & ".pdf"
                Cro.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
                rutas.Append("&" & rutaArchivo)
                pRutas = rutas.ToString()
            End If

        Catch ex As Exception
        End Try

    End Sub

    ''' <summary>66056 - JZAVALA.
    ''' METODO PARA TRABAJAR CON EL PDF.
    ''' </summary>
    ''' <param name="cartas"></param>
    ''' <remarks></remarks>
    Public Sub CrearMultiCartaPDF(ByVal cartas As String)
        Dim destinoPdf As String, nombreNuevoArchivo As String
        Dim PrefijoFolder As String = "Llamado_"
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

            Dim sourceFiles() As String = cartas.Substring(1).Split("&")
            Dim f As Integer = 0
            Dim reader As PdfReader = New PdfReader(sourceFiles(f))
            Dim n As Integer = reader.NumberOfPages
            Dim document As Document = New Document(reader.GetPageSizeWithRotation(1))
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
            'ini HDG OT 67554 duplicado
            Dim sfile As String
            sfile = folderActual & "\" & nombreNuevoArchivo
            RegisterStartupScript("abre", "<script>window.open('" & sfile.Replace("\", "\\") & "')</script>")
            'fin HDG OT 67554 duplicado

            For Each savedDoc As String In cartas.Split(New Char() {"&"})
                If File.Exists(savedDoc) Then
                    File.Delete(savedDoc)
                End If
            Next
        Catch ex As Exception
            UIUtility.PublicarEvento("CrearMultiCartaPDF - ex.Message = " & ex.Message & " ++ ex.StackTrace = " & ex.StackTrace)
            For Each savedDoc As String In cartas.Split(New Char() {"&"})
                If File.Exists(savedDoc) Then
                    File.Delete(savedDoc)
                End If
            Next
        Finally

        End Try

    End Sub

    Private Sub GenerarReporteRentaFijaPDF()

        'OBTENER Y CARGAR EL CRYSTAL REPORT.
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM()
        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim fileReport As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Reportes/rptReporteConsolidadoFT.rpt"
        oReport.Load(fileReport)
        'OBTENER LA DATA DE LA CONSULTA CONSOLIDADO.
        Dim oDs As DataSet
        Dim dtConsolidadoFuturos As New DataTable
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        oDs = oPrevOrdenInversionBM.GenerarReporteConsolidado(ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"), DatosRequest)
        dtConsolidadoFuturos = oDs.Tables(0)

        '*******************FIRMAS*********************
        Dim oPreOrdenInversion As New PrevOrdenInversionBM()
        Dim dr2 As DataRow
        Dim dtFirmas As New DataTable
        dtFirmas = oPreOrdenInversion.GenerarReporteConFirmas(ParametrosSIT.TR_DERIVADOS, ViewState("decFechaOperacion"), DatosRequest, "FT").Tables(2)
        dr2 = dtFirmas.Rows(0)

        '**********************************************
        '******** ASIGNAR LOS VALORES *****************
        Dim dsConsolidado As New DataTable
        dsConsolidado = dtConsolidadoFuturos

        Dim Firmas As New DataTable
        Firmas.Columns.Add("Firma1", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma2", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma3", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma4", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma5", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma6", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma7", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma8", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma9", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma10", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma11", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma12", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma13", Type.GetType("System.Byte[]"))
        Firmas.Columns.Add("Firma14", Type.GetType("System.Byte[]"))
        Firmas.Rows.Add(HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(0), String))), HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(1), String))), _
                        HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(2), String))), HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(3), String))), _
                        HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(4), String))), HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(5), String))), _
                        HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(6), String))), HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(7), String))), _
                        HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(8), String))), HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(9), String))), _
                        HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(10), String))), HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(11), String))), _
                        HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(12), String))), HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(13), String))))
        'Firmas = dtFirmas

        Dim dsInventarioForward As DataSet = New DataSet("dsInventarioForward")

        dsInventarioForward.Tables.Add(dsConsolidado.Copy)

        dsInventarioForward.Tables(0).TableName = "dsConsolidado"

        Dim dblSumaMargenInicial As Double = 0
        Dim dblSumaMargenFinal As Double = 0
        Dim dblSumaDepositoTHorizonte As Double = 0
        Dim dblSumaDepositoTIntermediario As Double = 0

        For Each objRegistros As DataRow In dtConsolidadoFuturos.Rows
            dblSumaMargenInicial += Convert.ToDouble(objRegistros("MARGENINICIAL").ToString())
            dblSumaMargenFinal += Convert.ToDouble(objRegistros("SALDO_FINAL").ToString())
            dblSumaDepositoTHorizonte += Convert.ToDouble(objRegistros("DEPOSITO_T_HORIZONTE").ToString())
            dblSumaDepositoTIntermediario += Convert.ToDouble(objRegistros("DEPOSITO_T_INTERMEDIARIO").ToString())
        Next

        'oReport.SetDataSource(dtConsolidadoFuturos)
        oReport.SetDataSource(dsInventarioForward)
        oReport.SetParameterValue("@FechaOperacion", tbFechaOperacion.Text)
        oReport.SetParameterValue("@SaldoInicialCtaMargen", dblSumaMargenInicial.ToString())
        oReport.SetParameterValue("@SaldoFinalCtaMargen", dblSumaMargenFinal.ToString())
        oReport.SetParameterValue("@TotalDepositoHorizonte", dblSumaDepositoTHorizonte.ToString())
        oReport.SetParameterValue("@TotalDepositoIntermediario", dblSumaDepositoTIntermediario.ToString())
        '**********************************************
        Dim rutaArchivo As String = ""
        If Not (oReport Is Nothing) Then
            'rutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RF_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
            rutaArchivo = "c:\temp\RF_" & Now.ToString(" - yyyyMMdd hhmmss") & ".pdf"
            oReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
        End If
        ViewState("RutaReporte") = rutaArchivo

    End Sub

    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            Try
                dtDestino.LoadDataRow(dr.ItemArray, False)
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        Next
    End Sub

#End Region


    Public Sub EjecutarOrdenInversion(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, Optional ByVal strCodPrevOrden As String = "", Optional ByRef bolUpdGrilla As Boolean = False, Optional ByVal claseInstrumento As String = "", Optional ByVal decNProceso As Decimal = 0)
        Dim objBM As New LimiteBM
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim ds As New DataSet
        Dim dtOrdenInversion As New DataTable
        Dim bolGeneraOrden As Boolean = False
        Dim i As Long
        Dim arrCodPrevOrden As Array

        objBM.RegistrarOrdenesPreviasSeleccionadas(strTipoRenta, decNProceso)
        ds = oPrevOrdenInversionBM.GenerarOrdenInversion_Sura(strTipoRenta, decFechaOperacion, DatosRequest, claseInstrumento, decNProceso)

        If ds.Tables(0).Rows.Count <= 0 Then
            bolUpdGrilla = True
        Else
            If ds.Tables(0).Rows(0)(0) = LIQUIDADO Then
                AlertaJS("Alguna operación ejecutada similar para una agrupación esta liquidada.\nDebe extornar la operación liquidada antes de ejecutar una agrupación similar.")
            Else
                Dim script As New StringBuilder
                dtOrdenInversion = ds.Tables(0)
                bolGeneraOrden = CType(ds.Tables(1).Rows(0)("GeneraOrden"), Boolean)

                arrCodPrevOrden = strCodPrevOrden.Split("|")
                For i = 0 To arrCodPrevOrden.Length - 1
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "1")
                Next

                If bolGeneraOrden Then
                    If dtOrdenInversion.Rows.Count > 0 Then
                        Dim Variable As String = "TmpCodigoUsuario,TmpEscenario,TmpNProceso"
                        Dim Parametros As String = Usuario + "," + ParametrosSIT.EJECUCION_PREVOI + "," + decNProceso.ToString
                        Dim obj As New JobBM
                        Session("dtOrdenInversion") = dtOrdenInversion

                        With script
                            .Append("window.showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=OI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + "','','dialogHeight:550px;dialogWidth:1000px;status:no;unadorned:yes;help:No');")
                            .Append("document.getElementById('" & btnBuscar.ClientID & "').click();")
                        End With
                        EjecutarJS(script.ToString())
                        'EjecutarJS(UIUtility.MostrarPopUp("frmValidacionExcesosPrevOI.aspx?Tipo=OI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento, "ConsultaOIGeneradas", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                    End If
                Else
                    If dtOrdenInversion.Rows.Count > 0 Then
                        Session("dtListaExcesos") = dtOrdenInversion

                        With script
                            .Append("window.showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + "','','dialogHeight:550px;dialogWidth:1000px;status:no;unadorned:yes;help:No');")
                            .Append("document.getElementById('" & btnBuscar.ClientID & "').click();")
                        End With
                        EjecutarJS(script.ToString())

                        'EjecutarJS(UIUtility.MostrarPopUp("frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento, "ConsultaOIGeneradas", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                    End If
                End If
            End If
        End If
        If bolGeneraOrden = False Then
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
        End If

    End Sub

    Protected Sub Datagrid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Datagrid1.RowCommand
        Dim row As GridViewRow = Nothing
        Dim gvr As GridViewRow = Nothing
        Select Case e.CommandName
            Case "Footer", "Item", "_Add"
                row = Datagrid1.FooterRow
            Case Else
                Try
                    gvr = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Catch ex As Exception
                End Try
        End Select

        If e.CommandName = "_Add" Then
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
            Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
            Dim ddlOperacionF As DropDownList
            Dim tbCantidadF As TextBox
            Dim tbTotalExposicionF As TextBox
            Dim tbVencimientoAnoF As TextBox
            Dim tbPrecioF As TextBox
            Dim tbTotalOrdenF As TextBox
            Dim ddlCondicionF As DropDownList
            Dim ddlPlazaNF As DropDownList
            Dim ddlVencimientoMesF As DropDownList
            Dim tbFechaLiquidacionF As TextBox
            Dim tbCantidadOperacionF As TextBox
            Dim tbPrecioOperacionF As TextBox
            Dim tbMontoOperacionF As TextBox
            Dim tbFondo1F As TextBox
            Dim tbFondo2F As TextBox
            Dim tbFondo3F As TextBox
            Dim strMensaje As String = ""
            Dim tbNemonicoF As TextBox
            Dim tbIntermediarioF As TextBox
            Dim hdIntermediarioF As HtmlControls.HtmlInputHidden
            Dim bolValidaCampos As Boolean = False
            Dim hdClaseInstrumentoF As HtmlControls.HtmlInputHidden
            Dim ddlContactoF As DropDownList
            Dim tbHoraF As TextBox
            Dim tbHoraEjeF As TextBox

            oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
            oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
            tbNemonicoF = CType(row.FindControl("tbNemonicoF"), TextBox)
            ddlVencimientoMesF = CType(row.FindControl("ddlVencimientoMesF"), DropDownList)
            tbVencimientoAnoF = CType(row.FindControl("tbVencimientoAnoF"), TextBox)
            tbTotalExposicionF = CType(row.FindControl("tbTotalExposicionF"), TextBox)
            ddlOperacionF = CType(row.FindControl("ddlOperacionF"), DropDownList)

            tbFechaLiquidacionF = CType(row.FindControl("tbFechaLiquidacionF"), TextBox)
            tbCantidadF = CType(row.FindControl("tbCantidadF"), TextBox)
            tbPrecioF = CType(row.FindControl("tbPrecioF"), TextBox)
            tbTotalOrdenF = CType(row.FindControl("tbTotalF"), TextBox)
            ddlCondicionF = CType(row.FindControl("ddlCondicionF"), DropDownList)
            tbIntermediarioF = CType(row.FindControl("tbIntermediarioF"), TextBox)
            hdIntermediarioF = CType(row.FindControl("hdIntermediarioF"), HtmlControls.HtmlInputHidden)
            ddlContactoF = CType(row.FindControl("ddlContactoF"), DropDownList)
            ddlPlazaNF = CType(row.FindControl("ddlPlazaNF"), DropDownList)
            tbHoraF = CType(row.FindControl("tbHoraF"), TextBox)
            tbCantidadOperacionF = CType(row.FindControl("tbCantidadOperacionF"), TextBox)
            tbPrecioOperacionF = CType(row.FindControl("tbPrecioOperacionF"), TextBox)
            tbMontoOperacionF = CType(row.FindControl("tbTotalOperacionF"), TextBox)
            tbFondo1F = CType(row.FindControl("tbFondo1F"), TextBox)
            tbFondo2F = CType(row.FindControl("tbFondo2F"), TextBox)
            tbFondo3F = CType(row.FindControl("tbFondo3F"), TextBox)
            hdClaseInstrumentoF = CType(row.FindControl("hdClaseInstrumentoF"), HtmlControls.HtmlInputHidden)
            tbHoraEjeF = CType(row.FindControl("tbHoraEjeF"), TextBox)

            If row.RowType = DataControlRowType.Footer Then
                If tbCantidadF.Text <> "" And _
                    tbPrecioF.Text <> "" And _
                    tbPrecioOperacionF.Text <> "" And _
                    tbCantidadOperacionF.Text <> "" And _
                    tbNemonicoF.Text <> "" And _
                    tbVencimientoAnoF.Text <> "" And _
                    ddlVencimientoMesF.SelectedValue <> "" And _
                    tbTotalExposicionF.Text <> "" And _
                    hdIntermediarioF.Value.ToString() <> "" And _
                    tbFechaLiquidacionF.Text <> "" Then
                    If ValidarOperacionPorClaseInstrumento(ParametrosSIT.FUTURO, ddlOperacionF.SelectedValue) Then
                        If IsNumeric(tbTotalOrdenF.Text) Then
                            If ValidarAsignaciones(CType(IIf(tbFondo1F.Text = "", 0, tbFondo1F.Text), Decimal), CType(IIf(tbFondo2F.Text = "", 0, tbFondo2F.Text), Decimal), CType(IIf(tbFondo3F.Text = "", 0, tbFondo3F.Text), Decimal), CType(tbTotalOrdenF.Text, Decimal)) Then
                                bolValidaCampos = True
                            Else
                                strMensaje = "Las asignaciones por fondo es incorrecto! "
                            End If
                        Else
                            strMensaje = "Ingrese correctamente el registro"
                        End If
                    Else
                        strMensaje = "La operacion ingresada no aplica para el tipo de instrumento a negociar! "
                    End If
                Else
                    If tbFechaLiquidacionF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Fecha Liquidación. \n"
                    End If
                    If tbNemonicoF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Nemónico. \n"
                    End If
                    If tbCantidadF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Cant. Instrumento. \n"
                    End If
                    If tbVencimientoAnoF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Año. \n"
                    End If
                    If ddlVencimientoMesF.SelectedValue = "" Then
                        strMensaje = strMensaje + "- Ingrese Mes. \n"
                    End If
                    If tbPrecioF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Precio. \n"
                    End If
                    If hdIntermediarioF.Value.ToString() = "" Then
                        strMensaje = strMensaje + "- Ingrese Intermediario. \n"
                    End If
                    If tbCantidadOperacionF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Cant. Instrumento Ejecución. \n"
                    End If
                    If tbPrecioOperacionF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Precio Ejecución. \n"
                    End If
                    If (hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) Then
                        strMensaje = strMensaje + "- Seleccione Tipo. \n"
                    End If
                    If tbTotalExposicionF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Total Exposición. \n"
                    End If
                End If


                If bolValidaCampos = True Then
                    Dim fechaMes As String
                    oRow.FechaOperacion = ViewState("decFechaOperacion")
                    oRow.HoraOperacion = tbHoraF.Text
                    oRow.CodigoContacto = ddlContactoF.SelectedValue
                    oRow.CodigoNemonico = tbNemonicoF.Text
                    oRow.VencimientoMes = ddlVencimientoMesF.SelectedValue
                    oRow.VencimientoAno = tbVencimientoAnoF.Text
                    oRow.CodigoOperacion = ddlOperacionF.SelectedValue
                    oRow.ClaseInstrumentoFx = ParametrosSIT.FUTURO
                    oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacionF.Text)
                    oRow.Cantidad = CType(tbCantidadF.Text, Decimal)
                    oRow.Precio = CType(tbPrecioF.Text, Decimal)
                    oRow.MontoNominal = CType(tbPrecioF.Text, Decimal)
                    oRow.TipoCondicion = ddlCondicionF.SelectedValue
                    oRow.CodigoTercero = hdIntermediarioF.Value
                    oRow.CodigoContacto = ddlContactoF.SelectedValue
                    oRow.HoraEjecucion = tbHoraEjeF.Text
                    oRow.CodigoPlaza = ddlPlazaNF.SelectedValue
                    oRow.CantidadOperacion = CType(tbCantidadOperacionF.Text, Decimal)
                    oRow.PrecioOperacion = CType(tbPrecioOperacionF.Text, Decimal)
                    oRow.MontoOperacion = CType(tbMontoOperacionF.Text, Decimal)
                    oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                    oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO
                    oRow.TotalExposicion = tbTotalExposicionF.Text.Replace(",", "")
                    oRow.FechaContrato = oRow.FechaLiquidacion

                    If ValidarAsignaciones(CType(IIf(tbFondo1F.Text = "", 0, tbFondo1F.Text), Decimal), CType(IIf(tbFondo2F.Text = "", 0, tbFondo2F.Text), Decimal), CType(IIf(tbFondo3F.Text = "", 0, tbFondo3F.Text), Decimal), CType(tbCantidadOperacionF.Text, Decimal)) Then
                        oRow.AsignacionF1 = CType(IIf(tbFondo1F.Text = "", 0, tbFondo1F.Text), Decimal)
                        oRow.AsignacionF2 = CType(IIf(tbFondo2F.Text = "", 0, tbFondo2F.Text), Decimal)
                        oRow.AsignacionF3 = CType(IIf(tbFondo3F.Text = "", 0, tbFondo3F.Text), Decimal)

                        oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                        oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()

                        oPrevOrdenInversionBM.Insertar(oPrevOrdenInversionBE, ParametrosSIT.TR_DERIVADOS.ToString(), DatosRequest)
                        CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                    Else
                        AlertaJS("La asignacion por fondo es incorrecta!")
                    End If
                Else
                    If strMensaje <> "" Then
                        AlertaJS(strMensaje)
                    Else
                        AlertaJS("Ingrese correctamente el registro!")
                    End If
                End If
            End If

            EjecutarJS("JavaScript:BorrarHiddens();")

        End If

        If e.CommandName = "_Delete" Then
            Dim decCodigoPrevOrden As Decimal = CType(e.CommandArgument.ToString(), Decimal)
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim script As String = ""

            If gvr.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Then
                Dim dtValidaExtorno As New DataTable
                dtValidaExtorno = oPrevOrdenInversionBM.ValidaExtorno(decCodigoPrevOrden, DatosRequest).Tables(0)
                If dtValidaExtorno.Rows.Count > 0 Then
                    script = "Para realizar el extorno, debe revertir las siguientes operaciones: \n"
                    For Each fila As DataRow In dtValidaExtorno.Rows
                        script = script & " - " & fila("CodigoOrden").ToString() & ", " & fila("CodigoPortafolioSBS") & ", " & fila("Estado").ToString() & "\n"
                    Next
                    AlertaJS(script)
                Else
                    script = UIUtility.MostrarPopUp("frmExtornoIngresoMasivoOperacion.aspx?tipoRenta=" & _
                    ParametrosSIT.TR_DERIVADOS.ToString() & "&codigo=" & _
                    decCodigoPrevOrden.ToString() & "&claseIntrumento=FT", "DetalleRegistroRV", 770, 480, 110, 55, "no", "no", "yes", "yes")
                    EjecutarJS(script, False)
                End If

            ElseIf gvr.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
            Else
                oPrevOrdenInversionBM.Eliminar(decCodigoPrevOrden, DatosRequest)
                CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
            End If
        End If

        If e.CommandName = "Item" Then
            Dim hdIntermediario As HtmlControls.HtmlInputHidden
            Dim tbNemonico As TextBox

            hdIntermediario = CType(gvr.FindControl("hdIntermediario"), HtmlControls.HtmlInputHidden)

            If hdIntermediario.Value.Trim <> "" Then
                Dim ddlContacto As DropDownList
                Dim objContacto As New ContactoBM
                Dim dtContacto As DataSet

                ddlContacto = CType(gvr.FindControl("ddlContacto"), DropDownList)
                dtContacto = objContacto.ListarContactoPorTerceros(hdIntermediario.Value)
                If dtContacto.Tables.Count > 0 Then
                    ddlContacto = CType(gvr.FindControl("ddlContacto"), DropDownList)
                    HelpCombo.LlenarComboBox(ddlContacto, dtContacto.Tables(0), "CodigoContacto", "DescripcionContacto", True)
                    ddlContacto.SelectedValue = ""
                Else
                    ddlContacto.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ""))
                End If
            End If

            tbNemonico = CType(gvr.FindControl("tbNemonico"), TextBox)
            Dim hdNemonico As HtmlControls.HtmlInputHidden
            hdNemonico = CType(gvr.FindControl("hdNemonico"), HtmlControls.HtmlInputHidden)
            If hdNemonico.Value <> "" Then
                tbNemonico.Text = hdNemonico.Value
            End If

            Dim hdDescTercero As HtmlControls.HtmlInputHidden
            Dim tbIntermediario As TextBox
            hdDescTercero = CType(gvr.FindControl("hdDescTercero"), HtmlControls.HtmlInputHidden)
            tbIntermediario = CType(gvr.FindControl("tbIntermediario"), TextBox)
            If hdDescTercero.Value <> "" Then
                tbIntermediario.Text = hdDescTercero.Value
            End If
            If tbNemonico.Text.Trim <> "" Then
                Dim tbHora As TextBox
                Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden

                hdClaseInstrumento = CType(gvr.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                tbHora = CType(gvr.FindControl("tbHora"), TextBox)
                tbHora.Text = Now.ToString("hh:mm:ss")
            End If

        End If

        If e.CommandName = "TipoFondo" Then
            Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden

            hdClaseInstrumento = CType(gvr.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
        End If

        If e.CommandName = "Footer" Then
            Dim hdIntermediarioF As HtmlControls.HtmlInputHidden
            Dim tbNemonicoF As TextBox
            Dim tbIntermediarioF As TextBox
            Dim tbTotalF As TextBox

            hdIntermediarioF = CType(row.FindControl("hdIntermediarioF"), HtmlControls.HtmlInputHidden)
            tbIntermediarioF = CType(row.FindControl("tbIntermediarioF"), TextBox)
            tbTotalF = CType(row.FindControl("tbTotalF"), TextBox)
            tbNemonicoF = CType(row.FindControl("tbNemonicoF"), TextBox)

            tbTotalF.Text = Me.hdnTotalF.Value
            tbNemonicoF.Text = Me.hdnOperador.Value

            If hdIntermediarioF.Value.Trim <> "" Then
                tbIntermediarioF.Text = hdnIntermediario.Value

                Dim ddlContactoF As DropDownList
                Dim objContacto As New ContactoBM
                Dim dtContacto As DataSet

                ddlContactoF = CType(row.FindControl("ddlContactoF"), DropDownList)
                dtContacto = objContacto.ListarContactoPorTerceros(hdIntermediarioF.Value)
                If dtContacto.Tables.Count > 0 Then
                    ddlContactoF = CType(row.FindControl("ddlContactoF"), DropDownList)
                    HelpCombo.LlenarComboBox(ddlContactoF, dtContacto.Tables(0), "CodigoContacto", "DescripcionContacto", True)
                    ddlContactoF.SelectedValue = ""
                Else
                    ddlContactoF.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ""))
                End If
            End If

            If tbNemonicoF.Text.Trim <> "" Then
                Dim tbHoraF As TextBox
                Dim tbHoraEjeF As TextBox
                Dim hdClaseInstrumentoF As HtmlControls.HtmlInputHidden

                hdClaseInstrumentoF = CType(row.FindControl("hdClaseInstrumentoF"), HtmlControls.HtmlInputHidden)
                tbHoraF = CType(row.FindControl("tbHoraF"), TextBox)
                tbHoraF.Text = New UtilDM().RetornarHoraSistema
                tbHoraEjeF = CType(row.FindControl("tbHoraEjeF"), TextBox)
                tbHoraEjeF.Text = New UtilDM().RetornarHoraSistema
            End If

        End If

        If e.CommandName = "Condicion" Then
            Dim ddlCondicion As DropDownList
            Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
            Dim hdTotal As HtmlControls.HtmlInputHidden
            Dim tbTotal As TextBox
            tbTotal = CType(gvr.FindControl("tbTotal"), TextBox)
            hdTotal = CType(gvr.FindControl("hdTotal"), HtmlControls.HtmlInputHidden)
            If hdTotal.Value <> "" Then
                tbTotal.Text = hdTotal.Value
            End If

            ddlCondicion = CType(gvr.FindControl("ddlCondicion"), DropDownList)
            hdClaseInstrumento = CType(gvr.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
        End If

        If e.CommandName = "CondicionF" Then
            Dim ddlCondicionF As DropDownList
            Dim hdClaseInstrumentoF As HtmlControls.HtmlInputHidden
            Dim hdTotalF As HtmlControls.HtmlInputHidden
            Dim tbTotalF As TextBox
            tbTotalF = CType(row.FindControl("tbTotalF"), TextBox)
            hdTotalF = CType(row.FindControl("hdTotalF"), HtmlControls.HtmlInputHidden)
            If hdTotalF.Value <> "" Then
                tbTotalF.Text = hdTotalF.Value
            End If

            ddlCondicionF = CType(row.FindControl("ddlCondicionF"), DropDownList)
            hdClaseInstrumentoF = CType(row.FindControl("hdClaseInstrumentoF"), HtmlControls.HtmlInputHidden)
        End If
    End Sub

    Protected Sub Datagrid1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Datagrid1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ddlCondicion As DropDownList
            Dim ddlOperacion As DropDownList
            Dim ddlPlazaN As DropDownList
            Dim ibBNemonico As ImageButton
            Dim ibBIntermediario As ImageButton
            Dim lbTipoCondicion2 As Label
            Dim lbOperacion As Label
            Dim lbVencimientoMes As Label
            Dim lbPlazaN As Label
            Dim ddlContacto As DropDownList
            Dim ddlVencimientoMes As DropDownList
            Dim hdIntermediario As HtmlControls.HtmlInputHidden
            Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
            Dim lbContacto As Label
            Dim lbTipoFondo As Label
            Dim hdOperacionTrz As HtmlControls.HtmlInputHidden  'HDG OT 67627 20130531

            ibBNemonico = CType(e.Row.FindControl("ibBNemonico"), ImageButton)
            ibBNemonico.Attributes.Add("onclick", "javascript:showPopupMnemonicoGrilla(this);")

            lbOperacion = CType(e.Row.FindControl("lbOperacion"), Label)
            ddlOperacion = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
            HelpCombo.LlenarComboBox(ddlOperacion, CType(Session("dtOperacion"), DataTable), "codigoOperacion", "Descripcion", False)
            ddlOperacion.SelectedValue = lbOperacion.Text
            hdOperacionTrz = CType(e.Row.FindControl("hdOperacionTrz"), HtmlControls.HtmlInputHidden)    'HDG OT 67627 20130531
            hdOperacionTrz.Value = ddlOperacion.SelectedItem.Text   'HDG OT 67627 20130531

            lbVencimientoMes = CType(e.Row.FindControl("lbVencimientoMes"), Label)
            ddlVencimientoMes = CType(e.Row.FindControl("ddlVencimientoMes"), DropDownList)
            HelpCombo.LlenarComboBox(ddlVencimientoMes, CType(Session("dtVencimientoMes"), DataTable), "Valor", "Nombre", False)
            ddlVencimientoMes.SelectedValue = lbVencimientoMes.Text

            ibBIntermediario = CType(e.Row.FindControl("ibBIntermediario"), ImageButton)
            ibBIntermediario.Attributes.Add("onclick", "javascript:ShowPopupTercerosGrilla(this);")

            lbTipoCondicion2 = CType(e.Row.FindControl("lbTipoCondicion2"), Label)
            ddlCondicion = CType(e.Row.FindControl("ddlCondicion"), DropDownList)
            HelpCombo.LlenarComboBox(ddlCondicion, CType(Session("dtCondicion"), DataTable), "Valor", "Nombre", False)
            ddlCondicion.SelectedValue = lbTipoCondicion2.Text
            ddlCondicion.Attributes.Add("onchange", "javascript:HabilitaCondicion(this);")

            lbPlazaN = CType(e.Row.FindControl("lbPlazaN"), Label)
            ddlPlazaN = CType(e.Row.FindControl("ddlPlazaN"), DropDownList)
            HelpCombo.LlenarComboBox(ddlPlazaN, CType(Session("dtPlazaN"), DataTable), "CodigoPlaza", "Descripcion", False)
            ddlPlazaN.SelectedValue = lbPlazaN.Text

            hdIntermediario = CType(e.Row.FindControl("hdIntermediario"), HtmlControls.HtmlInputHidden)
            ddlContacto = CType(e.Row.FindControl("ddlContacto"), DropDownList)
            If hdIntermediario.Value.Trim <> "" Then
                Dim objContacto As New ContactoBM
                Dim dtContacto As DataSet

                dtContacto = objContacto.ListarContactoPorTerceros(hdIntermediario.Value)
                If dtContacto.Tables.Count > 0 Then
                    lbContacto = CType(e.Row.FindControl("lbContacto"), Label)
                    ddlContacto = CType(e.Row.FindControl("ddlContacto"), DropDownList)
                    HelpCombo.LlenarComboBox(ddlContacto, dtContacto.Tables(0), "CodigoContacto", "DescripcionContacto", True)
                    ddlContacto.SelectedValue = lbContacto.Text
                Else
                    ddlContacto.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ""))
                End If
            Else
                ddlContacto.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ""))
            End If

            lbTipoFondo = CType(e.Row.FindControl("lbTipoFondo"), Label)

            hdClaseInstrumento = CType(e.Row.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)

            If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or _
                e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or _
                e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Or _
                e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then
                ddlCondicion.Enabled = False
                ddlOperacion.Enabled = False
                ddlVencimientoMes.Enabled = False
                ddlPlazaN.Enabled = False
                ddlContacto.Enabled = False
                ibBNemonico.Enabled = False
                ibBIntermediario.Enabled = False
                Dim tbCantidad As TextBox
                Dim tbVencimientoAno As TextBox
                Dim tbPrecio As TextBox
                Dim tbCantidadOperacion As TextBox
                Dim tbPrecioOperacion As TextBox
                Dim tbFondo1 As TextBox
                Dim tbFondo2 As TextBox
                Dim tbFondo3 As TextBox
                Dim tbTotalOperacion As TextBox
                Dim tbIntermediario As TextBox
                Dim tbTotalExposicion As TextBox
                Dim tbNemonico As TextBox
                Dim chkSelect As CheckBox
                Dim Imagebutton1 As ImageButton
                Dim tbHora As TextBox
                Dim tbHoraEje As TextBox
                Dim tbTotal As TextBox

                Imagebutton1 = CType(e.Row.FindControl("Imagebutton1"), ImageButton)
                tbCantidad = CType(e.Row.FindControl("tbCantidad"), TextBox)
                tbVencimientoAno = CType(e.Row.FindControl("tbVencimientoAno"), TextBox)
                tbPrecio = CType(e.Row.FindControl("tbPrecio"), TextBox)
                tbCantidadOperacion = CType(e.Row.FindControl("tbCantidadOperacion"), TextBox)
                tbPrecioOperacion = CType(e.Row.FindControl("tbPrecioOperacion"), TextBox)
                tbTotalExposicion = CType(e.Row.FindControl("tbTotalExposicion"), TextBox)
                tbFondo1 = CType(e.Row.FindControl("tbFondo1"), TextBox)
                tbFondo2 = CType(e.Row.FindControl("tbFondo2"), TextBox)
                tbFondo3 = CType(e.Row.FindControl("tbFondo3"), TextBox)
                tbTotalOperacion = CType(e.Row.FindControl("tbTotalOperacion"), TextBox)
                tbTotalOperacion = CType(e.Row.FindControl("tbTotalOperacion"), TextBox)
                tbIntermediario = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                tbNemonico = CType(e.Row.FindControl("tbNemonico"), TextBox)
                chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)
                tbHora = CType(e.Row.FindControl("tbHora"), TextBox)
                tbHoraEje = CType(e.Row.FindControl("tbHoraEje"), TextBox)
                tbTotal = CType(e.Row.FindControl("tbTotal"), TextBox)

                tbCantidad.Enabled = False
                tbPrecio.Enabled = False
                tbVencimientoAno.Enabled = False
                tbCantidadOperacion.Enabled = False
                tbPrecioOperacion.Enabled = False
                tbTotalExposicion.Enabled = False
                tbFondo1.Enabled = False
                tbFondo2.Enabled = False
                tbFondo3.Enabled = False
                tbTotalOperacion.Enabled = False
                tbIntermediario.Enabled = False
                tbNemonico.Enabled = False
                Imagebutton1.Enabled = False
                tbHora.Enabled = False
                tbHoraEje.Enabled = False
                tbTotal.Enabled = False

                If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then    'HDG OT 64291 20111201
                    Imagebutton1.Enabled = True
                End If
            End If
            Dim tbHora2 As TextBox
            Dim tbNemonico2 As TextBox
            Dim ddlOperacion2 As DropDownList
            Dim ddlVencimientoMes2 As DropDownList
            Dim tbTotalExposicion2 As TextBox
            Dim tbCantidad2 As TextBox
            Dim tbPrecio2 As TextBox
            Dim tbTotal2 As TextBox
            Dim tbIntermediario2 As TextBox
            Dim tbVencimientoAno2 As TextBox
            Dim ddlContacto2 As DropDownList
            Dim ddlPlazaN2 As DropDownList
            Dim tbHoraEje2 As TextBox
            Dim tbCantidadOperacion2 As TextBox
            Dim tbPrecioOperacion2 As TextBox
            Dim tbTotalOperacion2 As TextBox
            Dim tbFondo12 As TextBox
            Dim tbFondo22 As TextBox
            Dim tbFondo32 As TextBox
            tbHora2 = CType(e.Row.FindControl("tbHora"), TextBox)
            tbNemonico2 = CType(e.Row.FindControl("tbNemonico"), TextBox)
            ddlOperacion2 = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
            ddlVencimientoMes2 = CType(e.Row.FindControl("ddlVencimientoMes"), DropDownList)
            tbCantidad2 = CType(e.Row.FindControl("tbCantidad"), TextBox)
            tbPrecio2 = CType(e.Row.FindControl("tbPrecio"), TextBox)
            tbTotal2 = CType(e.Row.FindControl("tbTotal"), TextBox)
            tbIntermediario2 = CType(e.Row.FindControl("tbIntermediario"), TextBox)
            tbVencimientoAno2 = CType(e.Row.FindControl("tbVencimientoAno"), TextBox)
            tbTotalExposicion2 = CType(e.Row.FindControl("tbTotalExposicion"), TextBox)
            ddlContacto2 = CType(e.Row.FindControl("ddlContacto"), DropDownList)
            ddlPlazaN2 = CType(e.Row.FindControl("ddlPlazaN"), DropDownList)
            tbHoraEje2 = CType(e.Row.FindControl("tbHoraEje"), TextBox)
            tbCantidadOperacion2 = CType(e.Row.FindControl("tbCantidadOperacion"), TextBox)
            tbPrecioOperacion2 = CType(e.Row.FindControl("tbPrecioOperacion"), TextBox)
            tbTotalOperacion2 = CType(e.Row.FindControl("tbTotalOperacion"), TextBox)
            tbFondo12 = CType(e.Row.FindControl("tbFondo1"), TextBox)
            tbFondo22 = CType(e.Row.FindControl("tbFondo2"), TextBox)
            tbFondo32 = CType(e.Row.FindControl("tbFondo3"), TextBox)
            tbHora2.Attributes.Add("onchange", "javascript:cambio(this);")
            tbNemonico2.Attributes.Add("onpropertychange", "javascript:cambio(this);")
            ddlOperacion2.Attributes.Add("onchange", "javascript:cambio(this);")
            ddlVencimientoMes2.Attributes.Add("onchange", "javascript:cambio(this);")
            tbTotalExposicion2.Attributes.Add("onchange", "javascript:cambio(this);")
            tbCantidad2.Attributes.Add("onchange", "javascript:cambio(this);")
            tbPrecio2.Attributes.Add("onchange", "javascript:cambio(this);")
            tbTotal2.Attributes.Add("onchange", "javascript:cambio(this);")
            tbIntermediario2.Attributes.Add("onpropertychange", "javascript:cambio(this);")
            tbVencimientoAno2.Attributes.Add("onchange", "javascript:cambio(this);")
            ddlContacto2.Attributes.Add("onchange", "javascript:cambio(this);")
            ddlPlazaN2.Attributes.Add("onchange", "javascript:cambio(this);")
            tbHoraEje2.Attributes.Add("onchange", "javascript:cambio(this);")
            tbCantidadOperacion2.Attributes.Add("onchange", "javascript:cambio(this);")
            tbPrecioOperacion2.Attributes.Add("onchange", "javascript:cambio(this);")
            tbTotalOperacion2.Attributes.Add("onchange", "javascript:cambio(this);")
            tbFondo12.Attributes.Add("onchange", "javascript:cambio(this);")
            tbFondo22.Attributes.Add("onchange", "javascript:cambio(this);")
            tbFondo32.Attributes.Add("onchange", "javascript:cambio(this);")
        End If
        If e.Row.RowType = DataControlRowType.Footer Then
            Dim ddlCondicionF As DropDownList
            Dim ddlOperacionF As DropDownList
            Dim ddlPlazaNF As DropDownList
            Dim tbOperadorF As TextBox
            Dim ibBNemonicoF As ImageButton
            Dim ibBIntermediarioF As ImageButton
            Dim ddlContactoF As DropDownList
            Dim ddlVencimientoMesF As DropDownList

            tbOperadorF = CType(e.Row.FindControl("tbOperadorF"), TextBox)
            tbOperadorF.Text = Usuario.ToString.Trim

            ibBNemonicoF = CType(e.Row.FindControl("ibBNemonicoF"), ImageButton)
            ibBNemonicoF.Attributes.Add("onclick", "javascript:showPopupMnemonicoGrillaF(this);")

            ddlOperacionF = CType(e.Row.FindControl("ddlOperacionF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlOperacionF, CType(Session("dtOperacion"), DataTable), "codigoOperacion", "Descripcion", False)
            ddlOperacionF.SelectedIndex = 0

            ddlVencimientoMesF = CType(e.Row.FindControl("ddlVencimientoMesF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlVencimientoMesF, CType(Session("dtVencimientoMes"), DataTable), "Valor", "Nombre", False)
            ddlVencimientoMesF.SelectedIndex = 0

            ibBIntermediarioF = CType(e.Row.FindControl("ibBIntermediarioF"), ImageButton)
            ibBIntermediarioF.Attributes.Add("onclick", "javascript:ShowPopupTercerosGrillaF(this);")

            ddlCondicionF = CType(e.Row.FindControl("ddlCondicionF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlCondicionF, CType(Session("dtCondicion"), DataTable), "Valor", "Nombre", False)
            ddlCondicionF.SelectedIndex = 0
            ddlCondicionF.Attributes.Add("onchange", "javascript:HabilitaCondicionF(this);")

            ddlPlazaNF = CType(e.Row.FindControl("ddlPlazaNF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlPlazaNF, CType(Session("dtPlazaN"), DataTable), "CodigoPlaza", "Descripcion", False)
            ddlPlazaNF.SelectedIndex = 0

            ddlContactoF = CType(e.Row.FindControl("ddlContactoF"), DropDownList)
            ddlContactoF.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ""))
            ddlContactoF.SelectedIndex = 0
        End If
    End Sub
End Class

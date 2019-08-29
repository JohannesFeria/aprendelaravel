Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports CrystalDecisions.Shared
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports Microsoft.Office
Imports System.Collections.Generic
Imports ParametrosSIT
Imports System.Globalization
Imports System.Threading

Partial Class Modulos_Inversiones_frmIngresoMasivoOperacionFX__
    Inherits BasePage
    Dim pRutas As String
    Dim BMPrevOrden As New PrevOrdenInversionBE
    Dim rutas As New System.Text.StringBuilder
    Private Sub AgregarFilaTrazabilidad(ByRef _fila As TrazabilidadOperacionBE.TrazabilidadOperacionRow, ByVal fila As GridViewRow, ByVal CodigoPrevOrden As String, ByVal Nemonico As String, ByVal Operacion As String, ByVal Cantidad As String, ByVal Precio As String, ByVal Intermediario As String, ByVal CantidadOperacion As String, ByVal precioEjecutado As String, ByVal CodigoPortafolio As String, ByVal Asignacion As String)
        _fila.FechaOperacion = ViewState("decFechaOperacion").ToString()
        _fila.Correlativo = fila.Cells(1).Text
        _fila.Estado = fila.Cells(2).Text
        _fila.TipoOperacion = TIPO_OPER_PREVORDEN
        _fila.CodigoPrevOrden = CType(CodigoPrevOrden, Decimal)
        _fila.CodigoOrden = String.Empty
        _fila.CodigoNemonico = Nemonico
        _fila.CodigoOperacion = Operacion
        _fila.Cantidad = CType(Cantidad, Decimal)
        _fila.Precio = CType(Precio, Decimal)
        _fila.CodigoTercero = Intermediario
        _fila.CantidadEjecucion = CType(CantidadOperacion, Decimal)
        _fila.PrecioEjecucion = precioEjecutado
        _fila.ModoIngreso = MODO_ING_MASIVO
        _fila.Proceso = _PROCESO_TRAZA.Grabar
        _fila.MotivoCambio = MOTIVO_MODIFICAR_TRAZA
        _fila.Comentarios = COMENTARIO_MODIFICA_TRAZA
        _fila.CodigoPortafolio = CodigoPortafolio
        _fila.Asignacion = CType(Asignacion, Decimal)

    End Sub
    Public Function instanciarTabla(ByVal tabla As DataTable) As DataTable
        tabla = New DataTable
        tabla.Columns.Add("CodigoPrevOrden")
        tabla.Columns.Add("CodigoPortafolio")
        tabla.Columns.Add("Asignacion")
        tabla.Columns.Add("Cambio")
        Return tabla
    End Function
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Select Case _PopUp.Value
                    Case "GM"
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("tbNemonico"), TextBox).Text = CType(Session("SS_DatosModal"), String())(0)
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("tbInstrumento"), TextBox).Text = CType(Session("SS_DatosModal"), String())(2)
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("hdClaseInstrumento"), HtmlInputHidden).Value = String.Empty
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("hdNemonico"), HtmlInputHidden).Value = CType(Session("SS_DatosModal"), String())(0)
                    Case "FM"
                        CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox).Text = CType(Session("SS_DatosModal"), String())(0)
                        CType(Datagrid1.FooterRow.FindControl("tbInstrumentoF"), TextBox).Text = CType(Session("SS_DatosModal"), String())(2)
                        CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlInputHidden).Value = String.Empty
                        hdnOperador.Value = CType(Session("SS_DatosModal"), String())(0)
                    Case "GT"
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("tbIntermediario"), TextBox).Text = CType(Session("SS_DatosModal"), String())(1)
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("hdIntermediario"), HtmlInputHidden).Value = CType(Session("SS_DatosModal"), String())(0)
                        CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("hdDescTercero"), HtmlInputHidden).Value = CType(Session("SS_DatosModal"), String())(1)
                        EjecutarJS("cambio(" + _ControlID.Value + ");")
                    Case "FT"
                        CType(Datagrid1.FooterRow.FindControl("tbIntermediarioF"), TextBox).Text = CType(Session("SS_DatosModal"), String())(1)
                        CType(Datagrid1.FooterRow.FindControl("hdIntermediarioF"), HtmlInputHidden).Value = CType(Session("SS_DatosModal"), String())(0)
                        hdnIntermediario.Value = CType(Session("SS_DatosModal"), String())(0)
                End Select
                _RowIndex.Value = String.Empty
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub CargarPagina()
        CargarCombos()
        hdFechaNegocio.Value = UIUtility.ObtenerFechaMaximaNegocio()
        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(hdFechaNegocio.Value)
        ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
        ViewState("strOperador") = ddlOperador.SelectedValue
        ViewState("strEstado") = ddlEstado.SelectedValue
        Dim dtOperacion As New DataTable
        Dim oOperacionBM As New OperacionBM
        dtOperacion = oOperacionBM.SeleccionarOperacionesFx().Tables(0)
        Session("dtOperacion") = dtOperacion
        Dim dtMoneda As New DataTable
        Dim oMonedaBM As New MonedaBM
        dtMoneda = oMonedaBM.Listar(ESTADO_ACTIVO).Tables(0)
        Session("dtMoneda") = dtMoneda
        Dim dtTipoTitulo As New DataTable
        Dim oTipoTituloBM As New TipoTituloBM
        dtTipoTitulo = oTipoTituloBM.ListarPorDepositoPlazos(DatosRequest).Tables(0)
        Session("dtTipoTitulo") = dtTipoTitulo
        Dim dtMedioNeg As New DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtMedioNeg = oParametrosGeneralesBM.ListarMedioNegociacionPrevOI(ParametrosSIT.TR_RENTA_FIJA).Tables(0)   'HDG OT 64291 20111021
        Session("dtMedioNeg") = dtMedioNeg
        Dim dtMotivo As New DataTable
        Dim oMotivoBM As New MotivoBM
        dtMotivo = oMotivoBM.Listar(DatosRequest).Tables(0)
        Session("dtMotivo") = dtMotivo
        Dim dtTipoTasa As New DataTable
        dtTipoTasa = oParametrosGeneralesBM.Listar("TipoTasaI", DatosRequest)
        Session("dtTipoTasa") = dtTipoTasa
        Dim dtModforw As New DataTable
        dtModforw = oParametrosGeneralesBM.Listar(ParametrosSIT.MODALIDAD_FORW, DatosRequest)
        Session("dtModforw") = dtModforw
        Dim dtClaseifx As New DataTable
        dtClaseifx = oParametrosGeneralesBM.Listar(ParametrosSIT.CLASE_INSTRUMENTO_FX, DatosRequest)
        Session("dtClaseifx") = dtClaseifx
        CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"))
        'RGF 20110505 OT 63063 REQ 01
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
    Private Sub CargarEstados()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As New DataTable
        dt = oParametrosGeneralesBM.Listar(ParametrosSIT.ESTADO_PREV_OI, DatosRequest)
        HelpCombo.LlenarComboBox(ddlEstado, dt, "Valor", "Nombre", True)
        ddlEstado.SelectedIndex = 0
    End Sub
    Private Sub CargarOperadores()
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim dt As New DataTable
        dt = oPrevOrdenInversionBM.SeleccionarOperadores(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlOperador, dt, "UsuarioCreacion", "UsuarioCreacion", True)
        ddlOperador.SelectedIndex = 0
    End Sub
    Private Sub CargaClaseInstrumento()
        'CLASE INSTRUMENTO
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
        Dim dsClaseInstrumento As DataSet
        dsClaseInstrumento = oClaseInstrumentoBM.SeleccionarClaseInstrumentoPorTipoRenta(ParametrosSIT.TR_DERIVADOS, DatosRequest)
        HelpCombo.LlenarComboBox(ddlClaseInstrumento, dsClaseInstrumento.Tables(0), "Codigo", "Descripcion", True)
        ddlClaseInstrumento.SelectedIndex = 0
    End Sub
    Private Sub llenarFilaVacia(ByRef table As DataSet)
        Dim row As DataRow = table.Tables(0).NewRow()
        For Each item As DataColumn In table.Tables(0).Columns
            Select Case item.DataType
                Case GetType(String)
                    row(item.ColumnName) = String.Empty
                Case GetType(Decimal)
                    row(item.ColumnName) = 0.0
            End Select
        Next
        table.Tables(0).Rows.Add(row)
    End Sub
    Private Sub CargarGrilla(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, _
        Optional ByVal strCodigoClaseInstrumento As String = "", _
        Optional ByVal strCodigoTipoInstrumentoSBS As String = "", _
        Optional ByVal strCodigoNemonico As String = "", _
        Optional ByVal strOperador As String = "", _
        Optional ByVal strEstado As String = "")
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim ds As DataSet
        ds = oPrevOrdenInversionBM.SeleccionarPorFiltro(strTipoRenta, decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, strOperador, strEstado, DatosRequest)
        If ds.Tables(0).Rows.Count = 0 Then
            llenarFilaVacia(ds)
            Datagrid1.DataSource = ds
            Datagrid1.DataBind()
            Datagrid1.Rows(0).Visible = False
        Else
            Datagrid1.DataSource = ds
            Datagrid1.DataBind()
        End If
    End Sub
    Private Function ObtenerTipoCambioDI(ByVal monedaOrigen As String, ByVal monedaDestino As String) As String
        Dim strResul As String = ""
        Dim objTipoCambioDI As New TipoCambioDI_BM
        Dim objDsAux As DataSet = objTipoCambioDI.SeleccionarPorFiltros("", monedaOrigen, monedaDestino, "", "A", DatosRequest)
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
    Private Function validaFechas(ByVal fechaOperacion As Decimal, ByVal fechaLiquidacion As Decimal) As Boolean
        If fechaOperacion > fechaLiquidacion Then
            Return False
        Else
            Return True
        End If
    End Function
    Private Function obtenerMontoOperacion(ByVal monedaOrigen As String, ByVal monedaDestino As String, ByVal tipoCambio As Decimal, ByVal montoNominal As Decimal) As Decimal
        Dim dtAux As New DataTable
        Dim decTipoCambio As Decimal = 0.0
        Dim montoDestino As Decimal = 0.0
        Dim montoOrigen As Decimal = 0.0
        Dim montoOperacion As Decimal = 0.0
        Dim tipoCalculo As String = ""
        Dim tipoDI As String = ObtenerTipoCambioDI(monedaOrigen, monedaDestino)
        If tipoDI = "D" Then
            decTipoCambio = tipoCambio
            montoOrigen = montoNominal
            montoDestino = montoOrigen * decTipoCambio
            montoOperacion = Math.Round(montoDestino, 7) 'HDG 20120723
        ElseIf tipoDI = "I" Then
            decTipoCambio = tipoCambio
            montoOrigen = montoNominal
            montoDestino = montoOrigen / decTipoCambio
            montoOperacion = Math.Round(montoDestino, 7) 'HDG 20120723
        End If
        Return montoOperacion
    End Function
    Private Function obtenerMontoOperacionDPZ(ByVal TipoTasa As String, ByVal montoNominal As Decimal, ByVal tasa As Decimal, ByVal fechaOperacion As Decimal, ByVal fechaLiquidacion As Decimal) As Decimal
        Dim montoOperacion As Decimal = 0.0
        Dim plazo As Integer
        plazo = DateDiff(DateInterval.Day, CType(UIUtility.ConvertirFechaaString(fechaOperacion), Date), CType(UIUtility.ConvertirFechaaString(fechaLiquidacion), Date))
        If (TipoTasa = "1") Then
            montoOperacion = ((((montoNominal * tasa) / 100) / 360) * plazo) + montoNominal
        Else
            montoOperacion = montoNominal * Math.Pow((1 + (tasa / 100)), (plazo / 360))
        End If
        Return Math.Round(montoOperacion, 2) 'RGF 20110316
    End Function
    Private Function ValidarOperacionPorClaseInstrumento(ByVal strClaseInstrumento As String, ByVal strCodigoOperacion As String) As Boolean
        Dim bolResult As Boolean = False
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dtAux As New DataTable
        Dim dtOperacion As New DataTable
        Dim oOperacionBM As New OperacionBM
        dtAux = oParametrosGeneralesBM.SeleccionarPorFiltro(ParametrosSIT.CLASE_INSTRUMENTO_FX, "", strClaseInstrumento, "", DatosRequest)
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
    Private Function ValidarAsignaciones(ByVal decAsignacionF1 As Decimal, ByVal decAsignacionF2 As Decimal, ByVal decAsignacionF3 As Decimal, ByVal decCantidadOperacion As Decimal) As Boolean
        Dim bolResult As Boolean = False
        Dim decSumAsignaciones As Decimal = 0
        decSumAsignaciones = decAsignacionF1 + decAsignacionF2 + decAsignacionF3
        If decSumAsignaciones <> 0 Then
            If decSumAsignaciones = 100 Then
                'ASIGNACION DE PORCENTAJE
                bolResult = True
            Else
                'ASIGNACION DE UNIDADES
                If decSumAsignaciones = decCantidadOperacion Then
                    bolResult = True
                End If
            End If
        End If
        Return bolResult
    End Function
    'RGF 20110317
    Private Sub HabilitaControles(ByVal habilita As Boolean)
        btnGrabar.Enabled = habilita
        btnValidar.Enabled = habilita
        btnSwapDivisa.Enabled = habilita
        btnAprobar.Enabled = habilita
        Datagrid1.Enabled = habilita
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim decFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            'RGF 20110317
            If decFechaOperacion = hdFechaNegocio.Value And hdPuedeNegociar.Value = "1" Then 'RGF 20110505 OT 63063 REQ 01
                HabilitaControles(True)
            Else
                HabilitaControles(False)
            End If
            ViewState("decFechaOperacion") = decFechaOperacion
            Dim strCodigoClaseInstrumento As String = ddlClaseInstrumento.SelectedValue
            Dim strOperador As String = ddlOperador.SelectedValue
            Dim strEstado As String = ddlEstado.SelectedValue
            ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
            ViewState("strOperador") = ddlOperador.SelectedValue
            ViewState("strEstado") = ddlEstado.SelectedValue
            CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), decFechaOperacion, strCodigoClaseInstrumento, "", "", strOperador, strEstado)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
            Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
            Dim ddlOperacion As DropDownList
            Dim ddlMedioNeg As DropDownList
            Dim ddlTipoTasa As DropDownList
            Dim ddlClaseInstrumentofx As DropDownList
            Dim ddlMonedaNeg As DropDownList
            Dim ddlMoneda As DropDownList
            Dim ddlTipoTitulo As DropDownList
            Dim ddlModalidad As DropDownList
            Dim ddlMotivo As DropDownList
            Dim tbFechaLiquidacion As TextBox
            Dim hdIntermediario As HtmlControls.HtmlInputHidden
            Dim tbPrecio As TextBox
            Dim tbTasa As TextBox
            Dim tbTotalOrden As TextBox
            Dim tbPrecioFuturo As TextBox
            Dim tbFechaContrato As TextBox
            Dim tbHora As TextBox
            Dim lbCodigoPrevOrden As Label
            Dim _filaTraz As TrazabilidadOperacionBE.TrazabilidadOperacionRow
            Dim hdCodigoPais As HtmlControls.HtmlInputHidden 'CMB OT 66768 20130530
            Dim oTrazabilidadOperacionBE As New TrazabilidadOperacionBE
            Dim hdCambio As HtmlControls.HtmlInputHidden
            Dim hdCambioTraza As HtmlControls.HtmlInputHidden
            Dim hdCambioTrazaFondo As HtmlControls.HtmlInputHidden
            Dim hdClaseInstrumentofxTrz As HtmlControls.HtmlInputHidden
            Dim hdTipoTituloTrz As HtmlControls.HtmlInputHidden
            Dim hdOperacionTrz As HtmlControls.HtmlInputHidden
            Dim hdTotalOrdenTrz As HtmlControls.HtmlInputHidden
            Dim hdPrecioTrz As HtmlControls.HtmlInputHidden
            Dim hdIntermediarioTrz As HtmlControls.HtmlInputHidden
            Dim hdTotalOperacionTrz As HtmlControls.HtmlInputHidden
            Dim hdPrecioFuturoTrz As HtmlControls.HtmlInputHidden
            Dim chkPorcentaje As CheckBox
            Dim tbIntermediario As TextBox
            Dim porcentaje As String
            Dim ddlPortafolio As DropDownList
            Dim sNemonico As String
            Dim strCambioTrazaFondo As String = ""
            Dim dt As New DataTable
            For Each fila As GridViewRow In Datagrid1.Rows
                lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                hdCambio = CType(fila.FindControl("hdCambio"), HtmlControls.HtmlInputHidden)
                hdCambioTraza = CType(fila.FindControl("hdCambioTraza"), HtmlControls.HtmlInputHidden)  'HDG OT 67627 20130531
                hdCambioTrazaFondo = CType(fila.FindControl("hdCambioTrazaFondo"), HtmlControls.HtmlInputHidden)  'HDG OT 67627 20130531
                chkPorcentaje = CType(fila.FindControl("chkPorcentaje"), CheckBox)
                porcentaje = "N"
                If fila.Cells(2).Text = PREV_OI_INGRESADO Then
                    If (Not lbCodigoPrevOrden Is Nothing And hdCambio.Value = "1") Then
                        ddlOperacion = CType(fila.FindControl("ddlOperacion"), DropDownList)
                        ddlMedioNeg = CType(fila.FindControl("ddlMedioNeg"), DropDownList)
                        ddlTipoTasa = CType(fila.FindControl("ddlTipoTasa"), DropDownList)
                        ddlClaseInstrumentofx = CType(fila.FindControl("ddlClaseInstrumentofx"), DropDownList)
                        ddlMonedaNeg = CType(fila.FindControl("ddlMonedaNeg"), DropDownList)
                        ddlMoneda = CType(fila.FindControl("ddlMoneda"), DropDownList)
                        ddlTipoTitulo = CType(fila.FindControl("ddlTipoTitulo"), DropDownList)
                        ddlModalidad = CType(fila.FindControl("ddlModalidad"), DropDownList)
                        ddlMotivo = CType(fila.FindControl("ddlMotivo"), DropDownList)
                        tbFechaLiquidacion = CType(fila.FindControl("tbFechaLiquidacion"), TextBox)
                        hdIntermediario = CType(fila.FindControl("hdIntermediario"), HtmlControls.HtmlInputHidden)
                        tbPrecio = CType(fila.FindControl("tbPrecio"), TextBox)
                        tbTasa = CType(fila.FindControl("tbTasa"), TextBox)
                        tbTotalOrden = CType(fila.FindControl("tbTotalOrden"), TextBox)
                        tbPrecioFuturo = CType(fila.FindControl("tbPrecioFuturo"), TextBox)
                        tbFechaContrato = CType(fila.FindControl("tbFechaContrato"), TextBox)
                        tbHora = CType(fila.FindControl("tbHora"), TextBox)
                        hdCodigoPais = CType(fila.FindControl("hdCodigoPais"), HtmlControls.HtmlInputHidden) 'CMB OT 66768 20130530
                        ddlPortafolio = CType(fila.FindControl("ddlfondos"), DropDownList)
                        Dim bolValidar As Boolean = False
                        Select Case ddlClaseInstrumentofx.SelectedValue
                            Case ParametrosSIT.CLASE_INSTRUMENTO_DEPOSITOPLAZO
                                If ddlTipoTitulo.SelectedValue <> "" And _
                                    tbTotalOrden.Text <> "" And _
                                    tbTasa.Text <> "" And _
                                    ddlTipoTasa.SelectedValue <> "" And _
                                    tbFechaContrato.Text <> "" And _
                                    tbFechaLiquidacion.Text <> "" And _
                                    hdIntermediario.Value <> "" Then
                                    If ValidarOperacionPorClaseInstrumento(ParametrosSIT.CLASE_INSTRUMENTO_DEPOSITOPLAZO, ddlOperacion.SelectedValue) Then
                                        If IsNumeric(tbTotalOrden.Text) And _
                                            IsNumeric(tbTasa.Text) And _
                                            IsDate(tbFechaContrato.Text) And _
                                            IsDate(tbFechaLiquidacion.Text) Then
                                            If validaFechas(ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text)) Then
                                                If fila.Cells(2).Text <> ParametrosSIT.PREV_OI_EJECUTADO And _
                                                    fila.Cells(2).Text <> ParametrosSIT.PREV_OI_APROBADO And _
                                                    fila.Cells(2).Text <> ParametrosSIT.PREV_OI_PENDIENTE Then
                                                    bolValidar = True
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                                If bolValidar Then
                                    oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                                    oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                                    oRow.CodigoPrevOrden = lbCodigoPrevOrden.Text
                                    oRow.HoraOperacion = tbHora.Text
                                    oRow.ClaseInstrumentoFx = ddlClaseInstrumentofx.SelectedValue
                                    oRow.CodigoTipoTitulo = ddlTipoTitulo.SelectedValue
                                    oRow.MedioNegociacion = ddlMedioNeg.SelectedValue
                                    oRow.MontoNominal = CType(tbTotalOrden.Text, Decimal)
                                    oRow.Tasa = CType(tbTasa.Text, Decimal)
                                    oRow.TipoTasa = ddlTipoTasa.SelectedValue
                                    oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text)
                                    oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
                                    oRow.MontoOperacion = obtenerMontoOperacionDPZ(ddlTipoTasa.SelectedValue, CType(tbTotalOrden.Text, Decimal), CType(tbTasa.Text, Decimal), ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text))
                                    oRow.CodigoTercero = hdIntermediario.Value
                                    oRow.CodigoOperacion = ddlOperacion.SelectedValue
                                    oRow.CodigoNemonico = ddlTipoTitulo.SelectedValue
                                    oRow.Porcentaje = porcentaje
                                    oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                                    sNemonico = ddlTipoTitulo.SelectedValue
                                End If
                            Case ParametrosSIT.CLASE_INSTRUMENTO_FORWARD
                                If ddlMonedaNeg.SelectedValue <> "" And _
                                    ddlMoneda.SelectedValue <> "" And _
                                    tbTotalOrden.Text <> "" And _
                                    tbPrecio.Text <> "" And _
                                    ddlModalidad.SelectedValue <> "" And _
                                    tbPrecioFuturo.Text <> "" And _
                                    tbFechaContrato.Text <> "" And _
                                    tbFechaLiquidacion.Text <> "" And _
                                    hdIntermediario.Value <> "" And _
                                    ddlMotivo.SelectedValue <> "" Then
                                    If ValidarOperacionPorClaseInstrumento(ParametrosSIT.CLASE_INSTRUMENTO_FORWARD, ddlOperacion.SelectedValue) Then
                                        If IsNumeric(tbTotalOrden.Text) And _
                                        IsNumeric(tbPrecio.Text) And _
                                        IsNumeric(tbPrecioFuturo.Text) And _
                                        IsDate(tbFechaContrato.Text) And _
                                        IsDate(tbFechaLiquidacion.Text) Then
                                            If ddlMoneda.SelectedValue <> ddlMonedaNeg.SelectedValue Then
                                                If validaFechas(ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)) Then
                                                    If fila.Cells(2).Text <> ParametrosSIT.PREV_OI_EJECUTADO And _
                                                        fila.Cells(2).Text <> ParametrosSIT.PREV_OI_APROBADO And _
                                                        fila.Cells(2).Text <> ParametrosSIT.PREV_OI_PENDIENTE Then
                                                        bolValidar = True
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                                If bolValidar Then
                                    oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                                    oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                                    oRow.CodigoPrevOrden = lbCodigoPrevOrden.Text
                                    oRow.HoraOperacion = tbHora.Text
                                    oRow.ClaseInstrumentoFx = ddlClaseInstrumentofx.SelectedValue
                                    oRow.CodigoNemonico = ddlClaseInstrumentofx.SelectedItem.Text
                                    oRow.CodigoTercero = hdIntermediario.Value
                                    oRow.CodigoOperacion = ddlOperacion.SelectedValue
                                    oRow.MedioNegociacion = ddlMedioNeg.SelectedValue
                                    oRow.MonedaNegociada = ddlMonedaNeg.SelectedValue
                                    oRow.Moneda = ddlMoneda.SelectedValue
                                    oRow.MontoNominal = CType(tbTotalOrden.Text, Decimal)
                                    oRow.Precio = CType(tbPrecio.Text, Decimal)
                                    oRow.ModalidadForward = ddlModalidad.SelectedValue
                                    oRow.PrecioFuturo = CType(tbPrecioFuturo.Text, Decimal)
                                    oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text)
                                    oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
                                    oRow.CodigoMotivo = ddlMotivo.SelectedValue
                                    oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                                    oRow.Estado = PREV_OI_INGRESADO
                                    oRow.MontoOperacion = obtenerMontoOperacion(ddlMonedaNeg.SelectedValue, ddlMoneda.SelectedValue, CType(tbPrecioFuturo.Text, Decimal), CType(tbTotalOrden.Text, Decimal))
                                    oRow.Porcentaje = porcentaje
                                    oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                                    sNemonico = ddlClaseInstrumentofx.SelectedItem.Text
                                End If
                            Case ParametrosSIT.CLASE_INSTRUMENTO_CVME
                                If ddlMonedaNeg.SelectedValue <> "" And _
                                    ddlMoneda.SelectedValue <> "" And _
                                    tbTotalOrden.Text <> "" And _
                                    tbPrecio.Text <> "" And _
                                    tbFechaLiquidacion.Text <> "" And _
                                    hdIntermediario.Value <> "" Then
                                    If ValidarOperacionPorClaseInstrumento(ParametrosSIT.CLASE_INSTRUMENTO_CVME, ddlOperacion.SelectedValue) Then
                                        If IsNumeric(tbTotalOrden.Text) And _
                                           IsNumeric(tbPrecio.Text) And _
                                           IsDate(tbFechaLiquidacion.Text) Then
                                            If ddlMoneda.SelectedValue <> ddlMonedaNeg.SelectedValue Then
                                                If validaFechas(ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)) Then
                                                    If fila.Cells(2).Text <> ParametrosSIT.PREV_OI_EJECUTADO And _
                                                        fila.Cells(2).Text <> ParametrosSIT.PREV_OI_APROBADO And _
                                                        fila.Cells(2).Text <> ParametrosSIT.PREV_OI_PENDIENTE Then
                                                        bolValidar = True
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                                If bolValidar Then
                                    oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                                    oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                                    oRow.CodigoPrevOrden = lbCodigoPrevOrden.Text
                                    oRow.HoraOperacion = tbHora.Text
                                    oRow.ClaseInstrumentoFx = ddlClaseInstrumentofx.SelectedValue
                                    oRow.MonedaNegociada = ddlMonedaNeg.SelectedValue
                                    oRow.Moneda = ddlMoneda.SelectedValue
                                    oRow.MontoNominal = CType(tbTotalOrden.Text, Decimal)
                                    oRow.Precio = CType(tbPrecio.Text, Decimal)
                                    oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
                                    oRow.CodigoNemonico = ddlClaseInstrumentofx.SelectedItem.Text
                                    oRow.CodigoOperacion = ddlOperacion.SelectedValue
                                    oRow.CodigoTercero = hdIntermediario.Value
                                    oRow.MedioNegociacion = ddlMedioNeg.SelectedValue
                                    oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                                    oRow.Estado = PREV_OI_INGRESADO
                                    oRow.MontoOperacion = obtenerMontoOperacion(ddlMonedaNeg.SelectedValue, ddlMoneda.SelectedValue, CType(tbPrecio.Text, Decimal), CType(tbTotalOrden.Text, Decimal))
                                    oRow.Porcentaje = porcentaje
                                    oRow.CodigoMotivo = ddlMotivo.SelectedValue
                                    oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                                    sNemonico = ddlClaseInstrumentofx.SelectedItem.Text 'HDG OT 67627 20130531
                                End If
                        End Select
                        If bolValidar Then
                            If hdCambioTraza.Value = "1" Then
                                hdCambioTraza.Value = ""
                                hdClaseInstrumentofxTrz = CType(fila.FindControl("hdClaseInstrumentofxTrz"), HtmlControls.HtmlInputHidden)
                                hdTipoTituloTrz = CType(fila.FindControl("hdTipoTituloTrz"), HtmlControls.HtmlInputHidden)
                                hdOperacionTrz = CType(fila.FindControl("hdOperacionTrz"), HtmlControls.HtmlInputHidden)
                                hdTotalOrdenTrz = CType(fila.FindControl("hdTotalOrdenTrz"), HtmlControls.HtmlInputHidden)
                                hdPrecioTrz = CType(fila.FindControl("hdPrecioTrz"), HtmlControls.HtmlInputHidden)
                                hdIntermediarioTrz = CType(fila.FindControl("hdIntermediarioTrz"), HtmlControls.HtmlInputHidden)
                                hdTotalOperacionTrz = CType(fila.FindControl("hdTotalOperacionTrz"), HtmlControls.HtmlInputHidden)
                                hdPrecioFuturoTrz = CType(fila.FindControl("hdPrecioFuturoTrz"), HtmlControls.HtmlInputHidden)
                                tbIntermediario = CType(fila.FindControl("tbIntermediario"), TextBox)
                                If hdCambioTrazaFondo.Value = "1" Then
                                    strCambioTrazaFondo = "1"
                                End If
                                Dim CantOpe As Decimal = 0
                                Dim codnemo As String = String.Empty
                                If ddlClaseInstrumentofx.SelectedValue = CLASE_INSTRUMENTO_DEPOSITOPLAZO Then
                                    codnemo = hdTipoTituloTrz.Value
                                Else
                                    codnemo = hdClaseInstrumentofxTrz.Value
                                End If
                                If ddlClaseInstrumentofx.SelectedValue = CLASE_INSTRUMENTO_DEPOSITOPLAZO Then
                                    CantOpe = obtenerMontoOperacionDPZ(ddlTipoTasa.SelectedValue, CType(tbTotalOrden.Text, Decimal), CType(tbTasa.Text, Decimal), ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text))
                                ElseIf ddlClaseInstrumentofx.SelectedValue = CLASE_INSTRUMENTO_FORWARD Then
                                    CantOpe = obtenerMontoOperacion(ddlMonedaNeg.SelectedValue, ddlMoneda.SelectedValue, CType(tbPrecioFuturo.Text, Decimal), CType(tbTotalOrden.Text, Decimal))
                                Else
                                    CantOpe = obtenerMontoOperacion(ddlMonedaNeg.SelectedValue, ddlMoneda.SelectedValue, CType(tbPrecio.Text, Decimal), CType(tbTotalOrden.Text, Decimal))
                                End If
                                'If dtDetalleInversiones.Rows.Count > 0 Then
                                '    For Each dr As DataRow In dtDetalleInversiones.Rows
                                '        _filaTraz = oTrazabilidadOperacionBE.TrazabilidadOperacion.NewTrazabilidadOperacionRow()
                                '        AgregarFilaTrazabilidad(_filaTraz, fila, lbCodigoPrevOrden.Text, hdTipoTituloTrz.Value, ddlOperacion.SelectedValue, CType(hdTotalOrdenTrz.Value, Decimal), tbPrecio.Text, hdIntermediario.Value, CantOpe, CType(Val(tbPrecioFuturo.Text), Decimal), dr("CodigoPortafolio"), dr("Asignacion").ToString.Trim)
                                '        oTrazabilidadOperacionBE.TrazabilidadOperacion.AddTrazabilidadOperacionRow(_filaTraz)
                                '    Next
                                'Else
                                _filaTraz = oTrazabilidadOperacionBE.TrazabilidadOperacion.NewTrazabilidadOperacionRow()
                                AgregarFilaTrazabilidad(_filaTraz, fila, lbCodigoPrevOrden.Text, hdTipoTituloTrz.Value, ddlOperacion.SelectedValue, CType(hdTotalOrdenTrz.Value, Decimal), tbPrecio.Text, hdIntermediario.Value, CantOpe, CType(Val(tbPrecioFuturo.Text), Decimal), "", 0)
                                oTrazabilidadOperacionBE.TrazabilidadOperacion.AddTrazabilidadOperacionRow(_filaTraz)
                                'End If
                                oTrazabilidadOperacionBE.TrazabilidadOperacion.AcceptChanges()
                                oPrevOrdenInversionBM.InsertarTrazabilidad_sura(oTrazabilidadOperacionBE, PROCESO_TRAZA1, DatosRequest)
                            End If
                        End If
                        If bolValidar = False Then
                            AlertaJS("Complete los datos faltantes para cumplir con la validacion previa.")
                        End If
                    End If
                End If
            Next
            If oPrevOrdenInversionBE.PrevOrdenInversion.Rows.Count > 0 Then
                oPrevOrdenInversionBM.Modificar(oPrevOrdenInversionBE, DatosRequest)
                oRow = Nothing
                CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), "", "", ViewState("strOperador"), ViewState("strEstado"))
                AlertaJS("Grabación exitosa")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            GenerarReporteOperacionesFx()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub GenerarReporteOperacionesFx()
        'Dim oldCulture As CultureInfo
        Try
            Dim sFile As String, sTemplate As String
            Dim dtOperacionFx As New DataTable
            Dim dtResumen As New DataTable
            Dim dtResumenSBS As New DataTable
            Dim oDs As New DataSet
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim decNProceso As Decimal = 0
            'oldCulture = Thread.CurrentThread.CurrentCulture
            'Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            oDs = oPrevOrdenInversionBM.GenerarReporte(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), DatosRequest, decNProceso)
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            dtOperacionFx = oDs.Tables(0)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "FX_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls" 'CMB OT 62087 20110427 REQ 6
            Dim n As Integer
            Dim n2 As Long
            Dim dr As DataRow
            Dim oExcel As New Excel.Application
            Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
            Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
            Dim oCells As Excel.Range
            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaPrevOrdenInversionFX.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(2, 2) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheet.SaveAs(sFile)
            n = 8
            For Each dr In dtOperacionFx.Rows
                'ini HDG 20120314
                n2 = n + 1
                oSheet.Rows(n & ":" & n).Copy()
                oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                oSheet.Application.CutCopyMode = False
                oCells(n, 1) = dr("Correlativo")
                oCells(n, 2) = dr("HoraOperacion")
                oCells(n, 3) = dr("UsuarioCreacion")
                oCells(n, 4) = dr("Operacion")
                oCells(n, 5) = dr("MonedaNegociada")
                oCells(n, 6) = dr("Moneda")
                oCells(n, 7) = dr("MontoNominal")
                oCells(n, 7).NumberFormat = "###,###,##0.00"
                oCells(n, 8) = dr("Precio")
                oCells(n, 8).NumberFormat = "###,###,##0.0000"
                Try
                    oCells(n, 9) = UIUtility.ConvertirFechaaString(CType(dr("FechaContrato"), Decimal))
                Catch ex As Exception
                    oCells(n, 9) = ""
                End Try
                Try
                    oCells(n, 10) = UIUtility.ConvertirFechaaString(CType(dr("FechaLiquidacion"), Decimal))
                Catch ex As Exception
                    oCells(n, 10) = ""
                End Try
                oCells(n, 11) = dr("ModalidadForward")
                oCells(n, 12) = dr("Intermediario")
                oCells(n, 13) = dr("MedioNegociacion")
                oCells(n, 14) = dr("Descripcion")
                oCells(n, 15) = dr("Motivo")
                n = n + 1
            Next
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp) 'HDG 20120314
            oBook.Save()
            oBook.Close()
            oExcel.Quit()
            ReleaseComObject(oCells)
            ReleaseComObject(oSheet)
            ReleaseComObject(oSheets) : ReleaseComObject(oBook)
            ReleaseComObject(oBooks) : ReleaseComObject(oExcel)
            oExcel = Nothing : oBooks = Nothing : oBook = Nothing
            oSheets = Nothing : oSheet = Nothing : oCells = Nothing
            System.GC.Collect()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "FX_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
            Response.WriteFile(sFile)
            Response.End()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
        'Thread.CurrentThread.CurrentCulture = oldCulture
    End Sub
    Private Sub btnAprobar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim strCodPrevOrden As String = ""
            Dim decNProceso As Decimal = 0
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            Dim count As Decimal = 0
            For Each fila As GridViewRow In Datagrid1.Rows
                Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                Dim decCodigoPrevOrden As Decimal
                If chkSelect.Checked = True Then
                    If fila.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                        decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                        oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                        count = count + 1
                        strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text & "|"
                    End If
                End If
            Next
            If Not strCodPrevOrden = "" Then
                strCodPrevOrden = strCodPrevOrden.Substring(0, strCodPrevOrden.Length - 1)
                If count > 0 Then
                    EjecutarOrdenInversion(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), strCodPrevOrden, , , decNProceso)
                Else
                    AlertaJS("Seleccione el registro a ejecutar!")
                    oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
                End If
            Else
                AlertaJS("Ningun registro cumple el requerimiento de estado.")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnValidar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnValidar.Click
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
            Dim lbCodigoPrevOrden As Label
            Dim count As Integer = 0
            Dim decNProceso As Decimal = 0  'HDG OT 67554 duplicado
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Session("Login")) 'HDG OT 67554 duplicado
            For Each fila As GridViewRow In Datagrid1.Rows
                'If fila.ItemType = ListItemType.AlternatingItem Or fila.ItemType = ListItemType.Item Then
                lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                If fila.Cells(2).Text <> PREV_OI_EJECUTADO Then 'HDG 20120402
                    If Not lbCodigoPrevOrden Is Nothing Then
                        'PROCESAR VALIDACION
                        '-------------------------------------------
                        Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                        Dim decCodigoPrevOrden As Decimal
                        If chkSelect.Checked = True Then
                            decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                            oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)  'HDG OT 67554 duplicado
                            count = count + 1
                        End If
                    End If
                End If
                'End If
            Next
            If count > 0 Then
                Limites.VerificaExcesoLimites(Me.Usuario, ParametrosSIT.TR_DERIVADOS.ToString(), decNProceso)
                Dim dt As New DataTable
                dt = oPrevOrdenInversionBM.SeleccionarValidacionExcesos(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), DatosRequest, decNProceso).Tables(0)   'HDG OT 67554 duplicado
                If dt.Rows.Count > 0 Then
                    Session("dtListaExcesos") = dt
                    EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" + ParametrosSIT.TR_RENTA_VARIABLE.ToString() + "', '800', '550','" & btnBuscar.ClientID & "');")
                End If
            Else
                AlertaJS("Seleccione los registros a validar!")
                oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)  'HDG OT 67554 duplicado
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnValidarTrader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidarTrader.Click
        Try
            'VALIDACION DE EXCESOS POR TRADER
            Dim dtValidaTrader As New DataTable
            Dim oLimiteTradingBM As New LimiteTradingBM
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim decNProceso As Decimal = 0  'HDG OT 67554 duplicado
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Session("Login")) 'HDG OT 67554 duplicado
            Dim count As Decimal = 0
            For Each fila As GridViewRow In Datagrid1.Rows
                Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                Dim decCodigoPrevOrden As Decimal
                If chkSelect.Checked = True Then
                    If fila.Cells(2).Text = ParametrosSIT.PREV_OI_INGRESADO Then
                        decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                        oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso) 'HDG OT 67554 duplicado
                        count = count + 1
                    End If
                End If
            Next
            If count > 0 Then
                dtValidaTrader = oLimiteTradingBM.SeleccionarValidacionExcesosTrader_Sura(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), Usuario, DatosRequest, , decNProceso).Tables(0)  'HDG OT 67554 duplicado
                If dtValidaTrader.Rows.Count > 0 Then
                    Session("dtValidaTrader") = dtValidaTrader
                    EjecutarJS("showModalDialog('frmValidacionExcesosTrader.aspx?TipoRenta=" & ParametrosSIT.TR_DERIVADOS.ToString() & "&nProc=" & decNProceso.ToString() + "', '800', '550','" & btnBuscar.ClientID & "');")
                Else
                    oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)  'HDG OT 67554 duplicado
                End If
            Else
                AlertaJS("Seleccione los registros a validar!")
                oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)  'HDG OT 67554 duplicado
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnSwapDivisa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSwapDivisa.Click
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim bolResult As Boolean = False
            Dim decNProceso As Decimal = 0  'HDG OT 67554 duplicado
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Session("Login"))
            For Each fila As GridViewRow In Datagrid1.Rows
                Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                Dim decCodigoPrevOrden As Decimal
                If chkSelect.Checked = True Then
                    If fila.Cells(2).Text <> PREV_OI_EJECUTADO Then 'HDG 20120402
                        decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                        oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                    End If
                End If
            Next
            bolResult = oPrevOrdenInversionBM.ProcesarSwapDivisa(ViewState("decFechaOperacion"), DatosRequest, decNProceso)
            If bolResult = True Then
                AlertaJS("Proceso realizado satisfactoriamente!")
            Else
                AlertaJS("Procesa no valido, verifíque si las condiciones son correctas!")
            End If
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"))
        Catch ex As Exception

            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM
        Dim ds As DataSet
        Dim dtOI As DataTable
        Dim lbCodigoPrevOrden As Label
        Dim lbClase As Label
        Dim ddlClaseInstrumentofx As DropDownList
        Dim strCodigoOrden As String = ""
        Dim strPortafolioSBS As String = ""
        Dim PortafolioSBS As String = ""
        Dim strMoneda As String = ""
        Dim strOperacion As String = ""
        Dim strCodigoISIN As String = ""
        Dim strCodigoSBS As String = ""
        Dim ddlTipoTitulo As DropDownList
        Dim strCodigoMnemonico As String = ""
        Dim strClase As String = ""
        Dim strCatClase As String = ""
        Dim chkSelect As CheckBox
        Try
            For Each fila As GridViewRow In Datagrid1.Rows
                chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)
                If fila.Cells(2).Text = PREV_OI_EJECUTADO And chkSelect.Checked = True Then
                    lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                    lbClase = CType(fila.FindControl("lbClase"), Label)
                    ddlClaseInstrumentofx = CType(fila.FindControl("ddlClaseInstrumentofx"), DropDownList)
                    ddlTipoTitulo = CType(fila.FindControl("ddlTipoTitulo"), DropDownList)   'HDG 20120307
                    ds = oPrevOrdenInversion.SeleccionarImprimir_PrevOrdenInversion(lbCodigoPrevOrden.Text, DatosRequest)
                    dtOI = ds.Tables(0)
                    For Each fila2 As DataRow In dtOI.Rows
                        strCodigoOrden = fila2("CodigoOrden")
                        strPortafolioSBS = fila2("CodigoPortafolioSBS")
                        PortafolioSBS = fila2("PortafolioSBS")
                        Try
                            strMoneda = New MonedaBM().SeleccionarPorFiltro(fila2("Moneda"), String.Empty, String.Empty, String.Empty, String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion") 'HDG 20120307
                        Catch ex As Exception
                            strMoneda = ""
                        End Try
                        strOperacion = fila2("Operacion")
                        Session("dtdatosoperacion") = Nothing
                        strCodigoSBS = ""
                        'ini HDG 20120307
                        strCatClase = ddlClaseInstrumentofx.SelectedValue
                        If strCatClase = CLASE_INSTRUMENTO_DEPOSITOPLAZO Then
                            strClase = CLASE_LLAMADO_DEPOSITOPLAZO
                            strCodigoMnemonico = ddlTipoTitulo.SelectedValue
                            strCodigoSBS = IIf(fila2("CodigoSBS").ToString.Length < 12, String.Empty, fila2("CodigoSBS").ToString)
                        ElseIf strCatClase = CLASE_INSTRUMENTO_FORWARD Then
                            strClase = CLASE_LLAMADO_FORWARD
                            strCodigoMnemonico = ddlClaseInstrumentofx.SelectedItem.Text
                        ElseIf strCatClase = CLASE_INSTRUMENTO_CVME Then
                            strClase = CLASE_LLAMADO_CVME
                            strCodigoMnemonico = ddlClaseInstrumentofx.SelectedItem.Text
                        End If
                        'fin HDG 20120307
                        GenerarLlamado(strCodigoOrden, strPortafolioSBS, PortafolioSBS, strClase, strOperacion, strMoneda, strCodigoISIN, strCodigoSBS, strCodigoMnemonico)  'HDG 20120223
                    Next
                End If
            Next
            CrearMultiCartaPDF(rutas.ToString())
        Catch ex As Exception
            UIUtility.PublicarEvento("Llamado masivo renta fija - ex.Message = " & ex.Message & " ++ ex.StackTrace = " & ex.StackTrace)
            AlertaJS(ex.Message.ToString())
            For Each savedDoc As String In pRutas.Split(New Char() {"&"})
                If File.Exists(savedDoc) Then
                    File.Delete(savedDoc)
                End If
            Next
        End Try
    End Sub
    'HDG 20120301
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal codportafolio As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        'Page.RegisterStartupScript(Guid.NewGuid().ToString(), UIUtility.MostrarPopUp_New("Llamado/VisorLlamado.aspx?vcodigo=" + codigo + "&vportafolio=" + portafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "No", "Yes", "Yes"))
        Dim strtitulo, strnemonico, strisin, strsbs, strsubtitulo, strcodigo1, strcodigo, strportafolio, strclase, strmoneda, stroperacion, strmnemonicotemp As String
        Dim dscaract As New dscaracteristicas
        Dim dttempoperacion As DataTable
        strclase = clase
        strnemonico = mnemonico
        strisin = isin
        strsbs = sbs
        strmnemonicotemp = ""    'Request.QueryString("vnemonicotemp")
        strportafolio = codportafolio
        strcodigo1 = codigo
        Dim dsValor As New DataSet
        Dim oOIFormulas As New OrdenInversionFormulasBM
        If strclase = CLASE_LLAMADO_DEPOSITOPLAZO Then
            If Session("dtdatosoperacion") Is Nothing Then
                Dim dtOrden As DataTable = New OrdenPreOrdenInversionBM().ListarOrdenesInversionPorCodigoOrden(strcodigo1, strportafolio, DatosRequest, PORTAFOLIO_MULTIFONDOS).Tables(0)
                Session("dtdatosoperacion") = New DepositoPlazos().ObtenerDatosOperacion(DatosRequest, dtOrden.Rows(0))
            End If
        End If
        If strclase = CLASE_LLAMADO_FORWARD Then
            If Session("dtdatosoperacion") Is Nothing Then
                Dim dtOrden As DataTable = New OrdenPreOrdenInversionBM().ListarOrdenesInversionPorCodigoOrden(strcodigo1, strportafolio, DatosRequest, PORTAFOLIO_MULTIFONDOS).Tables(0)
                Session("dtdatosoperacion") = ObtenerDatosOperacionFDCV(strclase, dtOrden.Rows(0))
            End If
        End If
        If strclase = CLASE_LLAMADO_CVME Then
            If Session("dtdatosoperacion") Is Nothing Then
                Dim dtOrden As DataTable = New OrdenPreOrdenInversionBM().ListarOrdenesInversionPorCodigoOrden(strcodigo1, strportafolio, DatosRequest, PORTAFOLIO_MULTIFONDOS).Tables(0)
                Session("dtdatosoperacion") = ObtenerDatosOperacionFDCV(strclase, dtOrden.Rows(0))
            End If
        End If
        stroperacion = operacion 'RGF 20081212
        Dim dsoper As New dsDatosOperacion
        Dim droper As DataRow
        dttempoperacion = CType(Session("dtdatosoperacion"), DataTable)
        For Each dr As DataRow In dttempoperacion.Rows
            droper = dsoper.Tables(0).NewRow
            droper("c1") = dr("c1")
            droper("v1") = dr("v1")
            droper("c2") = dr("c2")
            droper("v2") = dr("v2")
            droper("c3") = dr("c3")
            droper("v3") = dr("v3")
            droper("c4") = dr("c4")
            'RGF 20081114 Joao Guell pidio q las monedas salgan al revés
            'RGF 20081212 Joao Guell envio ejemplo:
            'si se realiza una compra de divisas (EUROS), debe aparecer: de Dólares a EUROS. Si se realiza una venta de EUROS (Venta de Divisas), debe aparecer “de: EUROS a: Dólares”
            If strclase.Equals(CLASE_LLAMADO_CVME) And stroperacion.ToLower.IndexOf("compra") >= 0 Then
                droper("v4") = dr("v6")
                droper("v6") = dr("v4")
            Else
                droper("v4") = dr("v4")
                droper("v6") = dr("v6")
            End If
            droper("c5") = dr("c5")
            droper("v5") = dr("v5")
            droper("c6") = dr("c6")
            droper("c7") = dr("c7")
            droper("v7") = dr("v7")
            droper("c8") = dr("c8")
            droper("v8") = dr("v8")
            droper("c9") = dr("c9")
            droper("v9") = dr("v9")
            droper("c10") = dr("c10")
            droper("v10") = dr("v10")
            droper("c11") = dr("c11")
            droper("v11") = dr("v11")
            droper("c12") = dr("c12")
            droper("v12") = dr("v12")
            droper("c13") = dr("c13")
            droper("v13") = dr("v13")
            droper("c14") = dr("c14")
            droper("v14") = dr("v14")
            droper("c15") = dr("c15")
            droper("v15") = dr("v15")
            droper("c16") = dr("c16")
            droper("v16") = dr("v16")
            droper("c17") = dr("c17")
            droper("v17") = dr("v17")
            droper("c18") = dr("c18")
            droper("v18") = dr("v18")
            droper("c19") = dr("c19")
            droper("v19") = dr("v19")
            droper("c20") = dr("c20")
            droper("v20") = dr("v20")
            droper("c21") = dr("c21")
            droper("v21") = dr("v21")
            If strclase = CLASE_LLAMADO_FORWARD Or strclase.Substring(1) = CLASE_LLAMADO_FORWARD Then
                droper("c22") = dr("c22")
                droper("v22") = dr("v22")
                droper("v18") = dr("v18").ToString.Substring(0, IIf(dr("v18").ToString.Length >= 42, 42, dr("v18").ToString.Length))
            Else
                droper("c22") = ""
                droper("v22") = ""
            End If
            dsoper.Tables(0).Rows.Add(droper)
        Next

        Dim oStream As New System.IO.MemoryStream
        Dim Cro As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim Archivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Llamado/RptLlamado.rpt"
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
            Dim strusuario As String
            ' If strportafolio <> "MULTIFONDO" Then

            If esTraspaso Then
                strtitulo = "Orden de Inversion: (Origen) Nro - " + ordenOrigen + " , (Destino) Nro - " + ordenDestino
            Else
                strtitulo = "Orden de Inversion Nro - " + strcodigo
            End If

            'Else

            '    If esTraspaso Then
            '        strtitulo = "PreOrden de Inversion: Origen Nro - " + (ordenOrigen) + " , (Destino) Nro - " + ordenDestino
            '    Else
            '        strtitulo = "PreOrden de Inversion Nro - " + strcodigo
            '    End If

            'End If

            strsubtitulo = strclase + " - " + stroperacion

            Dim dscomisiones As New DataSet
            Dim dscomi As New dsdatoscomisiones
            Dim drcomi As DataRow
            dscomisiones = UIUtility.ObtenerTablaimpuestosComisionesGuardado(strcodigo, strportafolio)
            For Each drcomisiones As DataRow In dscomisiones.Tables(0).Rows
                drcomi = dscomi.Tables(0).NewRow
                drcomi("Descripcion1") = drcomisiones("Descripcion1")
                drcomi("ValorOcultoComision1") = drcomisiones("ValorOcultoComision1") 'IIf(drcomisiones("ValorOcultoComision1") Is DBNull.Value, "", Format(CType(drcomisiones("ValorOcultoComision1"), Decimal), "##,##0.0000000"))
                drcomi("PorcentajeComision1") = drcomisiones("PorcentajeComision1") + " :"
                drcomi("Descripcion2") = drcomisiones("Descripcion2")
                drcomi("ValorOcultoComision2") = drcomisiones("ValorOcultoComision2") 'IIf(drcomisiones("ValorOcultoComision2") Is DBNull.Value, "", Format(CType(drcomisiones("ValorOcultoComision2"), Decimal), "##,##0.0000000"))
                drcomi("PorcentajeComision2") = drcomisiones("PorcentajeComision2") + " :"
                dscomi.Tables(0).Rows.Add(drcomi)
            Next
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            strusuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)

            Cro.Load(Archivo)
            'ini CMB OT 65473 20120921
            Dim oOrdenInverion As New OrdenPreOrdenInversionBM()
            Dim dsFirma As New DsFirma
            Dim drFirma As DsFirma.FirmaRow
            Dim dr2 As DataRow
            Dim dtFirmas As New DataTable
            dtFirmas = oOrdenInverion.ObtenerFirmasLlamadoOI(ordenes(0).ToString(), UIUtility.ObtenerFechaNegocio(PORTAFOLIO_MULTIFONDOS), DatosRequest).Tables(0)
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
            'fin CMB OT 65473 20120921
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
                droper("v19") = dr("v19")
                droper("c20") = dr("c20")
                droper("v20") = dr("v20")
                droper("c21") = dr("c21")
                droper("v21") = dr("v21")
                dsoper.Tables(0).Rows.Add(droper)
            Next

            Cro.OpenSubreport("RptTotalOperacion").SetDataSource(dsoper)

            Cro.SetParameterValue("@Titulo", strtitulo)
            Cro.SetParameterValue("@Subtitulo", strsubtitulo)
            Cro.SetParameterValue("@Fondo", "Portafolio: " & portafolio)
            Cro.SetParameterValue("@Moneda", "Moneda: " & strmoneda)
            Cro.SetParameterValue("@Ruta_Logo", ConfigurationManager.AppSettings("RUTA_LOGO"))
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
            'isin
            If strisin <> "" Or strisin <> Nothing Then
                Cro.SetParameterValue("@CodigoIsin", "Codigo Isin: " & strisin)
                'Cro.SetParameterValue("@CodigoIsin", strisin)
            Else
                Cro.SetParameterValue("@CodigoIsin", "")
            End If
            'sbs
            If strsbs <> "" Or strsbs <> Nothing Then
                Cro.SetParameterValue("@CodigoSBS", "Codigo SBS: " & strsbs)
                'Cro.SetParameterValue("@CodigoSBS", strsbs)
            Else
                Cro.SetParameterValue("@CodigoSBS", "")
            End If
            'nemonico
            If strnemonico <> "" Or strnemonico <> Nothing Then
                Cro.SetParameterValue("@CodigoNemonico", "Codigo Mnemónico: " & strnemonico)
                'Cro.SetParameterValue("@CodigoNemonico", strnemonico)
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
            AlertaJS("Ha ocurrido un error al generar el reporte.")
        End Try
    End Sub
    'HDG 20120301
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
            'ini HDG 20130208
            Dim sfile As String
            sfile = folderActual & "\" & nombreNuevoArchivo
            Response.Clear()
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nombreNuevoArchivo)
            Response.WriteFile(sfile)
            Response.End()
            'RegisterStartupScript("abre", "<script>window.open('" & sfile.Replace("\", "\\") & "')</script>")
            'fin HDG 20130208
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
    'HDG 20120301
    Public Function ObtenerDatosOperacionFDCV(ByVal pClase As String, ByVal drOrden As DataRow) As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21", "c22", "v22"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        If pClase = CLASE_LLAMADO_CVME Then
            drGrilla("c1") = "Fecha Operacion"
            drGrilla("v1") = UIUtility.ConvertirFechaaString(drOrden("FechaOperacion"))
            drGrilla("c2") = "Fecha Liquidacion"
            drGrilla("v2") = UIUtility.ConvertirFechaaString(drOrden("FechaLiquidacion"))
            drGrilla("c3") = "Hora Operación"
            drGrilla("v3") = drOrden("HoraOperacion")
            drGrilla("c4") = "De :"
            Try
                drGrilla("v4") = New MonedaBM().SeleccionarPorFiltro(drOrden("CodigoMoneda"), String.Empty, String.Empty, String.Empty, String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
            Catch ex As Exception
                drGrilla("v4") = ""
            End Try
            drGrilla("c5") = "Monto Divisa Negociada"
            drGrilla("v5") = Format(drOrden("MontoOrigen"), "#,##0.0000000")
            drGrilla("c6") = "A :"
            Try
                drGrilla("v6") = New MonedaBM().SeleccionarPorFiltro(drOrden("CodigoMonedaDestino"), String.Empty, String.Empty, String.Empty, String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
            Catch ex As Exception
                drGrilla("v6") = ""
            End Try
            drGrilla("c7") = "Monto"
            drGrilla("v7") = Format(drOrden("MontoDestino"), "#,##0.0000000")
            drGrilla("c8") = "Tipo Cambio"
            drGrilla("v8") = Format(drOrden("TipoCambio"), "#,##0.0000000")
            drGrilla("c9") = "Intermediario"
            drGrilla("v9") = New TercerosBM().SeleccionarPorFiltro(drOrden("CodigoTercero"), String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
            If drOrden("CodigoContacto").ToString <> String.Empty Then
                drGrilla("c10") = "Contacto"
                drGrilla("v10") = New ContactoBM().Seleccionar(drOrden("CodigoContacto"), DatosRequest).Tables(0).Rows(0)("Descripcion")
            Else
                drGrilla("c10") = ""
                drGrilla("v10") = ""
            End If
            drGrilla("c11") = ""
            drGrilla("v11") = ""
            drGrilla("c12") = "Observación"
            drGrilla("v12") = drOrden("Observacion")
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
            drGrilla("c22") = ""
            drGrilla("v22") = ""
        End If
        If pClase = CLASE_LLAMADO_FORWARD Then
            drGrilla("c1") = "Fecha Operación"
            drGrilla("v1") = UIUtility.ConvertirFechaaString(drOrden("FechaOperacion"))
            drGrilla("c2") = "Fecha Contrato"
            drGrilla("v2") = UIUtility.ConvertirFechaaString(drOrden("FechaContrato"))
            drGrilla("c3") = "Hora Operación"
            drGrilla("v3") = drOrden("HoraOperacion")
            drGrilla("c4") = "Fecha Liquidación"
            drGrilla("v4") = UIUtility.ConvertirFechaaString(drOrden("FechaLiquidacion"))
            drGrilla("c5") = "Tipo Cambio Spot"
            drGrilla("v5") = Format(drOrden("TipoCambioSpot"), "#,##0.0000000")
            drGrilla("c6") = "Tipo Cambio Futuro"
            drGrilla("v6") = Format(drOrden("TipoCambioFuturo"), "#,##0.0000000")
            drGrilla("c7") = "De"
            Try
                drGrilla("v7") = New MonedaBM().SeleccionarPorFiltro(drOrden("CodigoMonedaOrigen"), String.Empty, String.Empty, String.Empty, String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
            Catch ex As Exception
                drGrilla("v7") = ""
            End Try
            drGrilla("c8") = "Monto Origen"
            drGrilla("v8") = Format(drOrden("MontoCancelar"), "#,##0.0000000")
            drGrilla("c9") = "A"
            Try
                drGrilla("v9") = New MonedaBM().SeleccionarPorFiltro(drOrden("CodigoMonedaDestino"), String.Empty, String.Empty, String.Empty, String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
            Catch ex As Exception
                drGrilla("v9") = ""
            End Try
            drGrilla("c10") = "Monto Futuro"
            drGrilla("v10") = Format(drOrden("MontoOperacion"), "#,##0.0000000")
            'drGrilla("c10") = ""
            'drGrilla("v10") = ""
            drGrilla("c11") = "Plazo"
            drGrilla("v11") = drOrden("Plazo")
            drGrilla("c12") = "Diferencial"
            drGrilla("v12") = drOrden("Diferencial")
            If drOrden("Delibery") = "S" Then
                drGrilla("c13") = "Modalidad Compra"
                drGrilla("v13") = "Delivery"
            Else
                drGrilla("c13") = "Modalidad Compra"
                drGrilla("v13") = "Non-Delivery"
            End If
            drGrilla("c14") = "Intermediario"
            drGrilla("v14") = New TercerosBM().SeleccionarPorFiltro(drOrden("CodigoTercero"), String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
            If drOrden("CodigoContacto").ToString <> String.Empty Then
                drGrilla("c15") = "Contacto"
                drGrilla("v15") = New ContactoBM().Seleccionar(drOrden("CodigoContacto"), DatosRequest).Tables(0).Rows(0)("Descripcion")
            Else
                drGrilla("c15") = ""
                drGrilla("v15") = ""
            End If
            drGrilla("c16") = "Observación"
            drGrilla("v16") = drOrden("Observacion")
            drGrilla("c17") = ""
            drGrilla("v17") = ""
            drGrilla("c18") = "Motivo"
            drGrilla("v18") = New MotivoBM().Seleccionar(drOrden("CodigoMotivo"), DatosRequest).Tables(0).Rows(0)("Descripcion")
            drGrilla("c19") = ""
            drGrilla("v19") = ""
            drGrilla("c20") = ""
            drGrilla("v20") = ""
            drGrilla("c21") = ""
            drGrilla("v21") = ""
            drGrilla("c22") = "Cobertura"
            drGrilla("v22") = New OrdenPreOrdenInversionBM().SeleccionarTipoMonedaxMotivoForw(drOrden("CodigoMotivo"), drOrden("TipoMonedaForw")).Rows(0)("Descripcion")
        End If
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    'CMB OT 65473 20120917
    Private Sub GenerarReporteFXPDF()
        Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio("MULTIFONDO")
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM()
        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim fileReport As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Reportes/rptReporteOperacionesMasivasFX.rpt"
        oReport.Load(fileReport)
        Dim dsAux As DataSet
        Dim dsReporteOperacionesMasivasFX As New DsReporteOperacionesMasivasFX
        dsAux = oPrevOrdenInversion.GenerarReporteConFirmas(ParametrosSIT.TR_DERIVADOS, fechaNegocio, DatosRequest)
        CopiarTabla(dsAux.Tables(0), dsReporteOperacionesMasivasFX.RegistroPrevio)
        'Firmas
        Dim drFirma As DsReporteOperacionesMasivasFX.FirmaRow
        Dim drRutaFirma As DataRow
        drRutaFirma = dsAux.Tables(1).Rows(0)
        drFirma = CType(dsReporteOperacionesMasivasFX.Firma.NewFirmaRow(), DsReporteOperacionesMasivasFX.FirmaRow)
        drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(0), String)))
        drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(1), String)))
        drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(2), String)))
        drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(3), String)))
        drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(4), String)))
        drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(5), String)))
        drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(6), String)))
        drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(7), String)))
        drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(8), String)))
        drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(9), String)))
        drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(10), String)))
        drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(11), String)))
        drFirma.Firma13 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(12), String)))
        drFirma.Firma14 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(13), String)))
        drFirma.Firma15 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(14), String)))
        drFirma.Firma16 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(15), String)))
        drFirma.Firma17 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(16), String)))
        drFirma.Firma18 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(17), String)))
        drFirma.Firma19 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(18), String)))
        drFirma.Firma20 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(19), String)))
        dsReporteOperacionesMasivasFX.Firma.AddFirmaRow(drFirma)
        dsReporteOperacionesMasivasFX.Firma.AcceptChanges()
        dsReporteOperacionesMasivasFX.Merge(dsReporteOperacionesMasivasFX, False, System.Data.MissingSchemaAction.Ignore)
        oReport.SetDataSource(dsReporteOperacionesMasivasFX)
        oReport.SetParameterValue("@Usuario", Usuario)
        oReport.SetParameterValue("@FechaOperacion", UIUtility.ConvertirFechaaString(fechaNegocio))
        Dim rutaArchivo As String = ""
        If Not (oReport Is Nothing) Then
            rutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "FX_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
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
                AlertaJS("Alguna operación ejecutada similar para una agrupación esta liquidada.\nDebe extornar la operación liquidada antes de ejecutar una agrupación similar.')")
            Else
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
                        Dim mensaje As String = obj.EjecutarJob("DTS_SIT_VerificaExcesoLimitesEnLinea" & System.DateTime.Now.ToString("_hhmmss"), "Verifica exceso de limites en linea, considerando el neteo de operaciones", Variable, Parametros, "", "", ConfigurationManager.AppSettings("SERVIDORETL"))
                        AlertaJS(mensaje)
                        Session("dtOrdenInversion") = dtOrdenInversion
                        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=OI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + "', '1000', '500','" & btnBuscar.ClientID & "');")
                    End If
                Else
                    If dtOrdenInversion.Rows.Count > 0 Then
                        Session("dtListaExcesos") = dtOrdenInversion
                        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + "', '1000', '500','" & btnBuscar.ClientID & "');")
                    End If
                End If
            End If
        End If
        If bolGeneraOrden = False Then
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
        End If
    End Sub
    Private Sub showDialogoPopupExtorno(ByVal tipoRenta As String, ByVal codigoPrevOrden As String)

        EjecutarJS("showModalDialog('frmExtornoIngresoMasivoOperacion.aspx?tipoRenta=" & tipoRenta & "&codigo=" & codigoPrevOrden + "', '900', '500','" & btnBuscar.ClientID & "');")
    End Sub
    Protected Sub Datagrid1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Datagrid1.PageIndexChanging
        Datagrid1.PageIndex = e.NewPageIndex
        CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"))
    End Sub
    Protected Sub Datagrid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Datagrid1.RowCommand
        Try
            Dim row As GridViewRow = Nothing
            Dim gvr As GridViewRow = Nothing
            Select Case e.CommandName
                Case "Footer", "Item", "Add"
                    row = Datagrid1.FooterRow
                Case Else
                    Try
                        gvr = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                    Catch ex As Exception
                    End Try
            End Select
            If e.CommandName = "asignarfondo" Then
                Dim codigoPrevOrden As String = e.CommandArgument.trim
                Dim chkPorcentaje As CheckBox
                Dim porcentaje As String
                chkPorcentaje = CType(gvr.FindControl("chkPorcentaje"), CheckBox)
                If chkPorcentaje.Checked Then
                    porcentaje = "S"
                Else
                    porcentaje = "N"
                End If

                If gvr.Cells(2).Text = PREV_OI_INGRESADO Then
                    EjecutarJS("showModalDialog('frmAsignacionFondo.aspx?codigoprevorden=" & codigoPrevOrden & "&porcentaje=" & porcentaje + "', '650', '450','');")
                End If
            ElseIf e.CommandName = "asignarfondoF" Then
                Dim codigoPrevOrden As String = e.CommandArgument.trim
                Dim chkPorcentajeF As CheckBox
                Dim porcentajeF As String
                chkPorcentajeF = CType(gvr.FindControl("chkPorcentajeF"), CheckBox)
                If chkPorcentajeF.Checked Then
                    porcentajeF = "S"
                Else
                    porcentajeF = "N"
                End If
                EjecutarJS("showModalDialog('frmAsignacionFondo.aspx?codigoprevorden=" & codigoPrevOrden & "&porcentaje=" & porcentajeF + "', '650', '450','');")
            ElseIf e.CommandName = "Add" Then
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
                Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
                Dim ddlOperacionF As DropDownList
                Dim ddlMedioNegF As DropDownList
                Dim ddlTipoTasaF As DropDownList
                Dim ddlClaseInstrumentofxF As DropDownList
                Dim ddlMonedaNegF As DropDownList
                Dim ddlMonedaF As DropDownList
                Dim ddlTipoTituloF As DropDownList
                Dim ddlModalidadF As DropDownList
                Dim ddlMotivoF As DropDownList
                Dim tbFechaLiquidacionF As TextBox
                Dim hdIntermediarioF As HtmlControls.HtmlInputHidden
                Dim tbPrecioF As TextBox
                Dim tbTasaF As TextBox
                Dim tbTotalOrdenF As TextBox
                Dim tbPrecioFuturoF As TextBox
                Dim chkPorcentajeF As CheckBox
                Dim porcentajeF As String
                Dim dtDetalleInversiones As DataTable
                Dim ddlPortafolio As DropDownList

                Dim tbFechaContratoF As TextBox
                Dim hdCodigoPaisF As HtmlControls.HtmlInputHidden
                ddlOperacionF = CType(row.FindControl("ddlOperacionF"), DropDownList)
                ddlMedioNegF = CType(row.FindControl("ddlMedioNegF"), DropDownList)
                ddlTipoTasaF = CType(row.FindControl("ddlTipoTasaF"), DropDownList)
                ddlClaseInstrumentofxF = CType(row.FindControl("ddlClaseInstrumentofxF"), DropDownList)
                ddlMonedaNegF = CType(row.FindControl("ddlMonedaNegF"), DropDownList)
                ddlMonedaF = CType(row.FindControl("ddlMonedaF"), DropDownList)
                ddlTipoTituloF = CType(row.FindControl("ddlTipoTituloF"), DropDownList)
                ddlModalidadF = CType(row.FindControl("ddlModalidadF"), DropDownList)
                ddlMotivoF = CType(row.FindControl("ddlMotivoF"), DropDownList)
                tbFechaLiquidacionF = CType(row.FindControl("tbFechaLiquidacionF"), TextBox)
                hdIntermediarioF = CType(row.FindControl("hdIntermediarioF"), HtmlControls.HtmlInputHidden)
                tbPrecioF = CType(row.FindControl("tbPrecioF"), TextBox)
                tbTasaF = CType(row.FindControl("tbTasaF"), TextBox)
                tbTotalOrdenF = CType(row.FindControl("tbTotalOrdenF"), TextBox)
                tbPrecioFuturoF = CType(row.FindControl("tbPrecioFuturoF"), TextBox)
                tbFechaContratoF = CType(row.FindControl("tbFechaContratoF"), TextBox)
                hdCodigoPaisF = CType(row.FindControl("hdCodigoPaisF"), HtmlControls.HtmlInputHidden) 'CMB OT 66768 20130530
                chkPorcentajeF = CType(Datagrid1.FooterRow.FindControl("chkPorcentajeF"), CheckBox)
                ddlPortafolio = CType(Datagrid1.FooterRow.FindControl("ddlfondosF"), DropDownList)
                porcentajeF = "N"
                Dim bolValidar As Boolean = False
                Dim mensajeValida As String = ""
                Select Case ddlClaseInstrumentofxF.SelectedValue
                    Case ParametrosSIT.CLASE_INSTRUMENTO_DEPOSITOPLAZO
                        If ddlTipoTituloF.SelectedValue <> "" And _
                            tbTotalOrdenF.Text <> "" And _
                            tbTasaF.Text <> "" And _
                            ddlTipoTasaF.SelectedValue <> "" And _
                            tbFechaContratoF.Text <> "" And _
                            tbFechaLiquidacionF.Text <> "" And _
                            hdIntermediarioF.Value <> "" Then
                            If ValidarOperacionPorClaseInstrumento(ParametrosSIT.CLASE_INSTRUMENTO_DEPOSITOPLAZO, ddlOperacionF.SelectedValue) Then
                                If IsNumeric(tbTotalOrdenF.Text) And _
                                    IsNumeric(tbTasaF.Text) And _
                                    IsDate(tbFechaContratoF.Text) And _
                                    IsDate(tbFechaLiquidacionF.Text) Then
                                    If validaFechas(ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaContratoF.Text)) Then
                                        bolValidar = True
                                    Else
                                        mensajeValida = "La fecha de vencimiento debe ser mayor a la fecha de operación! "
                                    End If
                                Else
                                    mensajeValida = "Ingrese correctamente el registro"
                                End If
                            Else
                                mensajeValida = "La operacion ingresada no aplica para el tipo de instrumento a negociar! "
                            End If
                        Else
                            If ddlOperacionF.SelectedValue = "" Then
                                mensajeValida = mensajeValida + "- Seleccione Operación\n"
                            End If
                            If ddlTipoTituloF.SelectedValue = "" Then
                                mensajeValida = mensajeValida + "- Seleccione Tipo Titulo\n"
                            End If
                            If tbTotalOrdenF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Monto Orden\n"
                            End If
                            If ddlTipoTasaF.SelectedValue = "" Then
                                mensajeValida = mensajeValida + "- Seleccione Tipo Tasa\n"
                            End If
                            If tbTasaF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese YTM% \n"
                            End If
                            If tbFechaContratoF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Fecha Fin Contrato\n"
                            End If
                            If tbFechaLiquidacionF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Fecha Vencimiento\n"
                            End If
                            If hdIntermediarioF.Value = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Intermediario\n"
                            End If
                            If tbPrecioF.Text = "" Or tbPrecioF.Text = "0" Then
                                mensajeValida = mensajeValida + "- Ingrese el Precio T.C.\n"
                            End If
                        End If
                        If bolValidar Then
                            oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                            oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                            oRow.FechaOperacion = ViewState("decFechaOperacion")
                            oRow.HoraOperacion = String.Format("{0:HH:mm}", Date.Now)
                            oRow.ClaseInstrumentoFx = ddlClaseInstrumentofxF.SelectedValue
                            oRow.CodigoTipoTitulo = ddlTipoTituloF.SelectedValue
                            oRow.MedioNegociacion = ddlMedioNegF.SelectedValue
                            oRow.MontoNominal = CType(tbTotalOrdenF.Text, Decimal)
                            oRow.Tasa = CType(tbTasaF.Text, Decimal)
                            oRow.TipoTasa = ddlTipoTasaF.SelectedValue
                            oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaContratoF.Text)
                            oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacionF.Text)
                            oRow.MontoOperacion = obtenerMontoOperacionDPZ(ddlTipoTasaF.SelectedValue, CType(tbTotalOrdenF.Text, Decimal), CType(tbTasaF.Text, Decimal), ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaContratoF.Text))
                            oRow.CodigoTercero = hdIntermediarioF.Value
                            oRow.CodigoOperacion = ddlOperacionF.SelectedValue
                            oRow.CodigoNemonico = ddlTipoTituloF.SelectedValue
                            oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                            oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO
                            oRow.Porcentaje = porcentajeF
                            oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                            oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                            oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                            oPrevOrdenInversionBM.Insertar(oPrevOrdenInversionBE, ParametrosSIT.TR_DERIVADOS.ToString(), DatosRequest, dtDetalleInversiones)
                            CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), "", "", ViewState("strOperador"), ViewState("strEstado"))
                        Else
                            AlertaJS(mensajeValida)
                        End If
                    Case ParametrosSIT.CLASE_INSTRUMENTO_FORWARD
                        If ddlMonedaNegF.SelectedValue <> "" And _
                            ddlMonedaF.SelectedValue <> "" And _
                            tbTotalOrdenF.Text <> "" And _
                            (tbPrecioF.Text <> "" And CInt(tbPrecioF.Text) <> 0) And _
                            ddlModalidadF.SelectedValue <> "" And _
                            tbPrecioFuturoF.Text <> "" And _
                            tbFechaContratoF.Text <> "" And _
                            tbFechaLiquidacionF.Text <> "" And _
                            hdIntermediarioF.Value <> "" And _
                            ddlMotivoF.SelectedValue <> "" Then
                            If ValidarOperacionPorClaseInstrumento(ParametrosSIT.CLASE_INSTRUMENTO_FORWARD, ddlOperacionF.SelectedValue) Then
                                If IsNumeric(tbTotalOrdenF.Text) And _
                                IsNumeric(tbPrecioF.Text) And _
                                IsNumeric(tbPrecioFuturoF.Text) And _
                                IsDate(tbFechaContratoF.Text) And _
                                IsDate(tbFechaLiquidacionF.Text) Then
                                    If ddlMonedaF.SelectedValue <> ddlMonedaNegF.SelectedValue Then
                                        If validaFechas(ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaContratoF.Text)) Then
                                            bolValidar = True
                                        Else
                                            mensajeValida = "La fecha de vencimiento debe ser mayor a la fecha de operación! "
                                        End If
                                    Else
                                        mensajeValida = "Las monedas negociadas deben ser diferentes"
                                    End If
                                Else
                                    mensajeValida = "Ingrese correctamente el registro"
                                End If
                            Else
                                mensajeValida = "La operacion ingresada no aplica para el tipo de instrumento a negociar! "
                            End If
                        Else
                            If ddlMonedaNegF.SelectedValue = "" Then
                                mensajeValida = mensajeValida + "- Seleccione Moneda Negociada\n"
                            End If
                            If ddlMonedaF.SelectedValue = "" Then
                                mensajeValida = mensajeValida + "- Seleccione Moneda\n"
                            End If
                            If ddlOperacionF.SelectedValue = "" Then
                                mensajeValida = mensajeValida + "- Seleccione Operación\n"
                            End If
                            If tbTotalOrdenF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Monto Orden\n"
                            End If
                            If tbPrecioF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Precio / T.C\n"
                            End If
                            If ddlModalidadF.SelectedValue = "" Then
                                mensajeValida = mensajeValida + "- Seleccione Modalidad Forward\n"
                            End If
                            If tbPrecioFuturoF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese T.C. Futuro\n"
                            End If
                            If tbFechaContratoF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Fecha Fin Contrato\n"
                            End If
                            If tbFechaLiquidacionF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Fecha Vencimiento\n"
                            End If
                            If hdIntermediarioF.Value = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Intermediario\n"
                            End If
                            If ddlMotivoF.SelectedValue = "" Then
                                mensajeValida = mensajeValida + "- Seleccione Motivo\n"
                            End If
                        End If

                        If bolValidar Then
                            oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                            oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                            oRow.FechaOperacion = ViewState("decFechaOperacion")
                            oRow.HoraOperacion = String.Format("{0:HH:mm}", Date.Now)
                            oRow.ClaseInstrumentoFx = ddlClaseInstrumentofxF.SelectedValue
                            oRow.CodigoNemonico = ddlClaseInstrumentofxF.SelectedItem.Text
                            oRow.CodigoTercero = hdIntermediarioF.Value
                            oRow.CodigoOperacion = ddlOperacionF.SelectedValue
                            oRow.MedioNegociacion = ddlMedioNegF.SelectedValue
                            oRow.MonedaNegociada = ddlMonedaNegF.SelectedValue
                            oRow.Moneda = ddlMonedaF.SelectedValue
                            oRow.MontoNominal = CType(tbTotalOrdenF.Text, Decimal)
                            oRow.Precio = CType(tbPrecioF.Text, Decimal)
                            oRow.ModalidadForward = ddlModalidadF.SelectedValue
                            oRow.PrecioFuturo = CType(tbPrecioFuturoF.Text, Decimal)
                            oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaContratoF.Text)
                            oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacionF.Text)
                            oRow.CodigoMotivo = ddlMotivoF.SelectedValue
                            oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                            oRow.Estado = PREV_OI_INGRESADO
                            oRow.Porcentaje = porcentajeF
                            oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                            oRow.MontoOperacion = obtenerMontoOperacion(ddlMonedaNegF.SelectedValue, ddlMonedaF.SelectedValue, CType(tbPrecioFuturoF.Text, Decimal), CType(tbTotalOrdenF.Text, Decimal)) 'CMB 20110429
                            oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                            oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                            oPrevOrdenInversionBM.Insertar(oPrevOrdenInversionBE, ParametrosSIT.TR_DERIVADOS.ToString(), DatosRequest, dtDetalleInversiones)
                            CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), "", "", ViewState("strOperador"), ViewState("strEstado"))
                        Else
                            AlertaJS(mensajeValida)
                        End If
                    Case ParametrosSIT.CLASE_INSTRUMENTO_CVME
                        If ddlMonedaNegF.SelectedValue <> "" And _
                            ddlMonedaF.SelectedValue <> "" And _
                            tbTotalOrdenF.Text <> "" And _
                            tbPrecioF.Text <> "" And _
                            tbFechaLiquidacionF.Text <> "" And _
                            hdIntermediarioF.Value <> "" Then
                            If ValidarOperacionPorClaseInstrumento(ParametrosSIT.CLASE_INSTRUMENTO_CVME, ddlOperacionF.SelectedValue) Then
                                If IsNumeric(tbTotalOrdenF.Text) And _
                                   IsNumeric(tbPrecioF.Text) And _
                                   IsDate(tbFechaLiquidacionF.Text) Then
                                    If ddlMonedaF.SelectedValue <> ddlMonedaNegF.SelectedValue Then
                                        If validaFechas(ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacionF.Text)) Then
                                            bolValidar = True
                                        Else
                                            mensajeValida = "La fecha de vencimiento debe ser mayor a la fecha de operación! "
                                        End If
                                    Else
                                        mensajeValida = "Las monedas negociadas deben ser diferentes"
                                    End If
                                Else
                                    mensajeValida = "Ingrese correctamente el registro"
                                End If
                            Else
                                mensajeValida = "La operacion ingresada no aplica para el tipo de instrumento a negociar! "
                            End If
                        Else
                            If ddlMonedaNegF.SelectedValue = "" Then
                                mensajeValida = mensajeValida + "- Seleccione Moneda Negociada\n"
                            End If
                            If ddlMonedaF.SelectedValue = "" Then
                                mensajeValida = mensajeValida + "- Seleccione Moneda\n"
                            End If
                            If tbTotalOrdenF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Monto Orden\n"
                            End If
                            If tbPrecioF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Precio / T.C \n"
                            End If
                            If tbFechaLiquidacionF.Text = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Fecha Vencimiento\n"
                            End If
                            If hdIntermediarioF.Value = "" Then
                                mensajeValida = mensajeValida + "- Ingrese Intermediario\n"
                            End If
                        End If
                        If bolValidar Then
                            oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                            oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                            oRow.FechaOperacion = ViewState("decFechaOperacion")
                            oRow.HoraOperacion = String.Format("{0:HH:mm}", Date.Now)
                            oRow.ClaseInstrumentoFx = ddlClaseInstrumentofxF.SelectedValue
                            oRow.MonedaNegociada = ddlMonedaNegF.SelectedValue
                            oRow.Moneda = ddlMonedaF.SelectedValue
                            oRow.MontoNominal = CType(tbTotalOrdenF.Text, Decimal)
                            oRow.Precio = CType(tbPrecioF.Text, Decimal)
                            oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacionF.Text)
                            oRow.CodigoNemonico = ddlClaseInstrumentofxF.SelectedItem.Text
                            oRow.CodigoOperacion = ddlOperacionF.SelectedValue
                            oRow.CodigoTercero = hdIntermediarioF.Value
                            oRow.MedioNegociacion = ddlMedioNegF.SelectedValue
                            oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                            oRow.Estado = PREV_OI_INGRESADO
                            oRow.Porcentaje = porcentajeF
                            oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                            oRow.MontoOperacion = obtenerMontoOperacion(ddlMonedaNegF.SelectedValue, ddlMonedaF.SelectedValue, CType(tbPrecioF.Text, Decimal), CType(tbTotalOrdenF.Text, Decimal)) 'CMB 20110429
                            oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                            oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                            oPrevOrdenInversionBM.Insertar(oPrevOrdenInversionBE, ParametrosSIT.TR_DERIVADOS.ToString(), DatosRequest, dtDetalleInversiones)
                            CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), "", "", ViewState("strOperador"), ViewState("strEstado"))
                        Else
                            AlertaJS(mensajeValida)
                        End If
                End Select
                EjecutarJS("BorrarHiddens();")
            End If
            If e.CommandName = "_Delete" Then
                Dim decCodigoPrevOrden As Decimal = CType(e.CommandArgument.ToString(), Decimal)
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                Dim script As String = ""
                gvr = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
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
                        showDialogoPopupExtorno(ParametrosSIT.TR_DERIVADOS.ToString(), decCodigoPrevOrden.ToString())
                        'script = UIUtility.MostrarPopUp("frmExtornoIngresoMasivoOperacion.aspx?tipoRenta=" & _
                        'ParametrosSIT.TR_DERIVADOS.ToString() & "&codigo=" & _
                        'decCodigoPrevOrden.ToString(), "DetalleRegistroFX", 770, 480, 110, 55, "no", "no", "yes", "yes")
                        'EjecutarJS(script, False)
                    End If
                ElseIf gvr.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                    oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                    CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                Else
                    oPrevOrdenInversionBM.Eliminar(decCodigoPrevOrden, DatosRequest)
                    CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try

    End Sub
    Protected Sub Datagrid1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Datagrid1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ddlMonedaNeg As DropDownList
                Dim ddlMoneda As DropDownList
                Dim ddlOperacion As DropDownList
                Dim ddlTipoTitulo As DropDownList
                Dim ddlModalidad As DropDownList
                Dim ddlTipoTasa As DropDownList
                Dim ddlMedioNeg As DropDownList
                Dim ddlMotivo As DropDownList
                Dim ddlFondos As DropDownList
                Dim ddlClaseInstrumentofx As DropDownList
                Dim ibBIntermediario As ImageButton
                Dim lbMonedaNeg As Label
                Dim lbMoneda As Label
                Dim lbOperacion As Label
                Dim lbTipoTitulo As Label
                Dim lbModalidad As Label
                Dim lbTipoTasa As Label
                Dim lbMedioNeg As Label
                Dim lbMotivo As Label
                Dim lbClaseInstrumentofx As Label
                Dim hdOperacionTrz As HtmlControls.HtmlInputHidden  'HDG OT 67627 20130531
                Dim hdClaseInstrumentofxTrz As HtmlControls.HtmlInputHidden  'HDG OT 67627 20130531
                Dim hdTipoTituloTrz As HtmlControls.HtmlInputHidden  'HDG OT 67627 20130531
                lbMonedaNeg = CType(e.Row.FindControl("lbMonedaNeg"), Label)
                lbMoneda = CType(e.Row.FindControl("lbMoneda"), Label)
                lbOperacion = CType(e.Row.FindControl("lbOperacion"), Label)
                lbTipoTitulo = CType(e.Row.FindControl("lbTipoTitulo"), Label)
                lbModalidad = CType(e.Row.FindControl("lbModalidad"), Label)
                lbTipoTasa = CType(e.Row.FindControl("lbTipoTasa"), Label)
                lbMedioNeg = CType(e.Row.FindControl("lbMedioNeg"), Label)
                lbMotivo = CType(e.Row.FindControl("lbMotivo"), Label)
                lbClaseInstrumentofx = CType(e.Row.FindControl("lbClaseInstrumentofx"), Label)
                ddlMonedaNeg = CType(e.Row.FindControl("ddlMonedaNeg"), DropDownList)
                ddlMoneda = CType(e.Row.FindControl("ddlMoneda"), DropDownList)
                ddlOperacion = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
                ddlTipoTitulo = CType(e.Row.FindControl("ddlTipoTitulo"), DropDownList)
                ddlModalidad = CType(e.Row.FindControl("ddlModalidad"), DropDownList)
                ddlTipoTasa = CType(e.Row.FindControl("ddlTipoTasa"), DropDownList)
                ddlMedioNeg = CType(e.Row.FindControl("ddlMedioNeg"), DropDownList)
                ddlMotivo = CType(e.Row.FindControl("ddlMotivo"), DropDownList)
                ddlFondos = CType(e.Row.FindControl("ddlfondos"), DropDownList)
                ddlClaseInstrumentofx = CType(e.Row.FindControl("ddlClaseInstrumentofx"), DropDownList)
                ibBIntermediario = CType(e.Row.FindControl("ibBIntermediario"), ImageButton)
                ibBIntermediario.Attributes.Add("onclick", "javascript:return ShowPopupTercerosGrilla(this);")
                HelpCombo.LlenarComboBox(ddlMonedaNeg, CType(Session("dtMoneda"), DataTable), "CodigoMoneda", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlMoneda, CType(Session("dtMoneda"), DataTable), "CodigoMoneda", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlOperacion, CType(Session("dtOperacion"), DataTable), "codigoOperacion", "Descripcion", False)
                HelpCombo.LlenarComboBox(ddlTipoTitulo, CType(Session("dtTipoTitulo"), DataTable), "CodigoTipoTitulo", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlModalidad, CType(Session("dtModforw"), DataTable), "Valor", "Nombre", True)
                HelpCombo.LlenarComboBox(ddlTipoTasa, CType(Session("dtTipoTasa"), DataTable), "Valor", "Nombre", True)
                HelpCombo.LlenarComboBox(ddlMedioNeg, CType(Session("dtMedioNeg"), DataTable), "Valor", "Nombre", False)
                HelpCombo.LlenarComboBox(ddlMotivo, CType(Session("dtMotivo"), DataTable), "CodigoMotivo", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlClaseInstrumentofx, CType(Session("dtClaseifx"), DataTable), "Valor", "Nombre", False)
                Dim fondos As String() = CType(e.Row.FindControl("hdFondo1Trz"), HtmlControls.HtmlInputHidden).Value.Split("/")
                For Each fondo As String In fondos
                    ddlFondos.Items.Add(fondo)
                Next
                ddlMonedaNeg.SelectedValue = lbMonedaNeg.Text
                ddlMoneda.SelectedValue = lbMoneda.Text
                ddlOperacion.SelectedValue = lbOperacion.Text
                ddlTipoTitulo.SelectedValue = lbTipoTitulo.Text
                ddlModalidad.SelectedValue = lbModalidad.Text
                ddlTipoTasa.SelectedValue = lbTipoTasa.Text
                If lbMedioNeg.Text <> String.Empty Then
                    ddlMedioNeg.SelectedValue = lbMedioNeg.Text
                End If
                ddlMotivo.SelectedValue = lbMotivo.Text
                ddlClaseInstrumentofx.SelectedValue = lbClaseInstrumentofx.Text
                'ini HDG OT 67627 20130531
                hdClaseInstrumentofxTrz = CType(e.Row.FindControl("hdClaseInstrumentofxTrz"), HtmlControls.HtmlInputHidden)
                hdClaseInstrumentofxTrz.Value = ddlClaseInstrumentofx.SelectedItem.Text
                hdTipoTituloTrz = CType(e.Row.FindControl("hdTipoTituloTrz"), HtmlControls.HtmlInputHidden)
                hdTipoTituloTrz.Value = ddlTipoTitulo.SelectedItem.Text
                hdOperacionTrz = CType(e.Row.FindControl("hdOperacionTrz"), HtmlControls.HtmlInputHidden)
                hdOperacionTrz.Value = ddlOperacion.SelectedItem.Text

                ddlFondos = CType(e.Row.FindControl("ddlfondos"), DropDownList)
                CargaPortafolio(ddlFondos)
                Dim hdPortafolioSel As HtmlControls.HtmlInputHidden = CType(e.Row.FindControl("hdPortafolioSel"), HtmlControls.HtmlInputHidden)
                ddlFondos.SelectedValue = hdPortafolioSel.Value

                'fin HDG OT 67627 20130531
                If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or _
                    e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or _
                    e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Or _
                    e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then
                    Dim tbHora As TextBox
                    Dim tbTotalOrden As TextBox
                    Dim tbPrecio As TextBox
                    Dim tbPrecioFuturo As TextBox
                    Dim tbTasa As TextBox
                    Dim tbFechaContrato As TextBox
                    Dim tbFechaLiquidacion As TextBox
                    Dim tbTotalOperacion As TextBox
                    Dim tbIntermediario As TextBox
                    '  Dim tbFondo1 As TextBox
                    '  Dim tbFondo2 As TextBox
                    '  Dim tbFondo3 As TextBox
                    Dim chkSelect As CheckBox
                    Dim Imagebutton1 As ImageButton
                    tbHora = CType(e.Row.FindControl("tbHora"), TextBox)
                    tbTotalOrden = CType(e.Row.FindControl("tbTotalOrden"), TextBox)
                    tbPrecio = CType(e.Row.FindControl("tbPrecio"), TextBox)
                    tbPrecioFuturo = CType(e.Row.FindControl("tbPrecioFuturo"), TextBox)
                    tbTasa = CType(e.Row.FindControl("tbTasa"), TextBox)
                    tbFechaContrato = CType(e.Row.FindControl("tbFechaContrato"), TextBox)
                    tbFechaLiquidacion = CType(e.Row.FindControl("tbFechaLiquidacion"), TextBox)
                    tbTotalOperacion = CType(e.Row.FindControl("tbTotalOperacion"), TextBox)
                    tbIntermediario = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                    ' tbFondo1 = CType(e.Row.FindControl("tbFondo1"), TextBox)
                    ' tbFondo2 = CType(e.Row.FindControl("tbFondo2"), TextBox)
                    ' tbFondo3 = CType(e.Row.FindControl("tbFondo3"), TextBox)
                    chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)
                    Imagebutton1 = CType(e.Row.FindControl("Imagebutton1"), ImageButton)
                    tbHora.Enabled = False
                    tbTotalOrden.Enabled = False
                    tbPrecio.Enabled = False
                    tbPrecioFuturo.Enabled = False
                    tbTasa.Enabled = False
                    tbFechaContrato.Enabled = False
                    tbFechaLiquidacion.Enabled = False
                    tbTotalOperacion.Enabled = False
                    tbIntermediario.Enabled = False
                    ' tbFondo1.Enabled = False
                    ' tbFondo2.Enabled = False
                    ' tbFondo3.Enabled = False
                    ddlMonedaNeg.Enabled = False
                    ddlMoneda.Enabled = False
                    ddlOperacion.Enabled = False
                    ddlTipoTitulo.Enabled = False
                    ddlModalidad.Enabled = False
                    ddlTipoTasa.Enabled = False
                    ddlMedioNeg.Enabled = False
                    ddlMotivo.Enabled = False
                    ddlClaseInstrumentofx.Enabled = False
                    ibBIntermediario.Enabled = False
                    Imagebutton1.Enabled = False
                    chkSelect.Enabled = False
                    'HDG 20120402
                    If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or _
                        e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or _
                        e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Then
                        'CType(e.Item.FindControl("lbSwapDivisa"), Label).Text <> "" Then
                        chkSelect.Enabled = True
                    End If
                    'ini HDG 20120327
                    If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                        Imagebutton1.Enabled = True
                    End If
                    'fin HDG 20120327

                    Dim FechaVal As HtmlControls.HtmlGenericControl
                    FechaVal = CType(e.Row.FindControl("FechaContrato"), HtmlControls.HtmlGenericControl)
                    FechaVal.Attributes.Add("class", "input-append")

                    Dim FechaLiqui As HtmlControls.HtmlGenericControl
                    FechaLiqui = CType(e.Row.FindControl("FechaLiqui"), HtmlControls.HtmlGenericControl)
                    FechaLiqui.Attributes.Add("class", "input-append")
                End If
                Dim tbHora2 As TextBox
                Dim ddlClaseInstrumentofx2 As DropDownList
                Dim ddlMonedaNeg2 As DropDownList
                Dim ddlMoneda2 As DropDownList
                Dim ddlOperacion2 As DropDownList
                Dim ddlTipoTitulo2 As DropDownList
                Dim tbTotalOrden2 As TextBox
                Dim tbPrecio2 As TextBox
                Dim ddlModalidad2 As DropDownList
                Dim tbPrecioFuturo2 As TextBox
                Dim ddlTipoTasa2 As DropDownList
                Dim tbTasa2 As TextBox
                Dim tbFechaContrato2 As TextBox
                Dim tbFechaLiquidacion2 As TextBox
                Dim tbIntermediario2 As TextBox
                Dim ddlMedioNeg2 As DropDownList
                Dim ddlMotivo2 As DropDownList
                Dim chkPorcentaje2 As CheckBox
                Dim hdPorcentaje2 As HtmlControls.HtmlInputHidden
                tbHora2 = CType(e.Row.FindControl("tbHora"), TextBox)
                ddlClaseInstrumentofx2 = CType(e.Row.FindControl("ddlClaseInstrumentofx"), DropDownList)
                ddlMonedaNeg2 = CType(e.Row.FindControl("ddlMonedaNeg"), DropDownList)
                ddlMoneda2 = CType(e.Row.FindControl("ddlMoneda"), DropDownList)
                ddlOperacion2 = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
                ddlTipoTitulo2 = CType(e.Row.FindControl("ddlTipoTitulo"), DropDownList)
                tbTotalOrden2 = CType(e.Row.FindControl("tbTotalOrden"), TextBox)
                tbPrecio2 = CType(e.Row.FindControl("tbPrecio"), TextBox)
                ddlModalidad2 = CType(e.Row.FindControl("ddlModalidad"), DropDownList)
                tbPrecioFuturo2 = CType(e.Row.FindControl("tbPrecioFuturo"), TextBox)
                ddlTipoTasa2 = CType(e.Row.FindControl("ddlTipoTasa"), DropDownList)
                tbTasa2 = CType(e.Row.FindControl("tbTasa"), TextBox)
                tbFechaContrato2 = CType(e.Row.FindControl("tbFechaContrato"), TextBox)
                tbFechaLiquidacion2 = CType(e.Row.FindControl("tbFechaLiquidacion"), TextBox)
                tbIntermediario2 = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                ddlMedioNeg2 = CType(e.Row.FindControl("ddlMedioNeg"), DropDownList)
                ddlMotivo2 = CType(e.Row.FindControl("ddlMotivo"), DropDownList)
                hdPorcentaje2 = CType(e.Row.FindControl("hdPorcentaje"), HtmlControls.HtmlInputHidden)
                chkPorcentaje2 = CType(e.Row.FindControl("chkPorcentaje"), CheckBox)
                tbHora2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlClaseInstrumentofx2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlMonedaNeg2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlMoneda2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlOperacion2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlTipoTitulo2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbTotalOrden2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbPrecio2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlModalidad2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbPrecioFuturo2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlTipoTasa2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbTasa2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbFechaContrato2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbFechaLiquidacion2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbIntermediario2.Attributes.Add("onpropertychange", "javascript:cambio(this);")
                ddlMedioNeg2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlMotivo2.Attributes.Add("onchange", "javascript:cambio(this);")
                chkPorcentaje2.Attributes.Add("onchange", "javascript:cambio(this);")
                If hdPorcentaje2.Value = "S" Then
                    chkPorcentaje2.Checked = True
                Else
                    chkPorcentaje2.Checked = False
                End If
            End If
            If e.Row.RowType = DataControlRowType.Footer Then
                Dim ddlMonedaNegF As DropDownList
                Dim ddlMonedaF As DropDownList
                Dim ddlOperacionF As DropDownList
                Dim ddlTipoTituloF As DropDownList
                Dim ddlModalidadF As DropDownList
                Dim ddlTipoTasaF As DropDownList
                Dim ddlMedioNegF As DropDownList
                Dim ddlMotivoF As DropDownList
                Dim ddlfondos As DropDownList
                Dim ddlClaseInstrumentofxF As DropDownList
                Dim ibBIntermediarioF As ImageButton
                ddlMonedaNegF = CType(e.Row.FindControl("ddlMonedaNegF"), DropDownList)
                ddlMonedaF = CType(e.Row.FindControl("ddlMonedaF"), DropDownList)
                ddlOperacionF = CType(e.Row.FindControl("ddlOperacionF"), DropDownList)
                ddlTipoTituloF = CType(e.Row.FindControl("ddlTipoTituloF"), DropDownList)
                ddlModalidadF = CType(e.Row.FindControl("ddlModalidadF"), DropDownList)
                ddlTipoTasaF = CType(e.Row.FindControl("ddlTipoTasaF"), DropDownList)
                ddlMedioNegF = CType(e.Row.FindControl("ddlMedioNegF"), DropDownList)
                ddlMotivoF = CType(e.Row.FindControl("ddlMotivoF"), DropDownList)
                ddlClaseInstrumentofxF = CType(e.Row.FindControl("ddlClaseInstrumentofxF"), DropDownList)
                ibBIntermediarioF = CType(e.Row.FindControl("ibBIntermediarioF"), ImageButton)
                ibBIntermediarioF.Attributes.Add("onclick", "javascript:return ShowPopupTercerosGrillaF(this);")
                HelpCombo.LlenarComboBox(ddlMonedaNegF, CType(Session("dtMoneda"), DataTable), "CodigoMoneda", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlMonedaF, CType(Session("dtMoneda"), DataTable), "CodigoMoneda", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlOperacionF, CType(Session("dtOperacion"), DataTable), "codigoOperacion", "Descripcion", False)
                HelpCombo.LlenarComboBox(ddlTipoTituloF, CType(Session("dtTipoTitulo"), DataTable), "CodigoTipoTitulo", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlModalidadF, CType(Session("dtModforw"), DataTable), "Valor", "Nombre", True)
                HelpCombo.LlenarComboBox(ddlTipoTasaF, CType(Session("dtTipoTasa"), DataTable), "Valor", "Nombre", True)
                HelpCombo.LlenarComboBox(ddlMedioNegF, CType(Session("dtMedioNeg"), DataTable), "Valor", "Nombre", False)
                HelpCombo.LlenarComboBox(ddlMotivoF, CType(Session("dtMotivo"), DataTable), "CodigoMotivo", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlClaseInstrumentofxF, CType(Session("dtClaseifx"), DataTable), "Valor", "Nombre", False)
                ddlTipoTasaF.SelectedValue = TASA_NOMINAL

                ddlfondos = CType(e.Row.FindControl("ddlfondosF"), DropDownList)
                CargaPortafolio(ddlfondos)
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargaPortafolio(ByVal drlista As DropDownList)
        Dim objportafolio As New PortafolioBM
        drlista.DataSource = objportafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        drlista.DataTextField = "Descripcion"
        drlista.DataValueField = "CodigoPortafolio"
        drlista.DataBind()
        ' UIUtility.InsertarElementoSeleccion(drlista)
        objportafolio = Nothing
    End Sub
End Class

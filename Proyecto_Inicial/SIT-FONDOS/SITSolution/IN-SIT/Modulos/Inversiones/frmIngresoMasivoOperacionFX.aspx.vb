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
Partial Class Modulos_Inversiones_frmIngresoMasivoOperacionFX
    Inherits BasePage
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se ordenes los controles
    Dim ddlMonedaNeg As DropDownList, ddlMoneda As DropDownList, ddlOperacion As DropDownList, ddlModalidad As DropDownList, ddlMedioNeg As DropDownList,
    ddlMotivo As DropDownList, ddlFondos As DropDownList, ddlClaseInstrumentofx As DropDownList, lbMonedaNeg As Label, lbMoneda As Label, lbOperacion As Label,
    lbModalidad As Label, lbMedioNeg As Label, lbMotivo As Label, lbClaseInstrumentofx As Label, hdOperacionTrz As HtmlInputHidden,
    hdClaseInstrumentofxTrz As HtmlInputHidden, tbFechaLiquidacion As TextBox, hdIntermediario As HtmlInputHidden, tbPrecio As TextBox,
    tbTotalOrden As TextBox, tbHora As TextBox, lbCodigoPrevOrden As Label, hdCodigoPais As HtmlInputHidden, hdCambio As HtmlInputHidden, hdCambioTraza As HtmlInputHidden,
    hdCambioTrazaFondo As HtmlInputHidden, hdTotalOrdenTrz As HtmlInputHidden, hdPrecioTrz As HtmlInputHidden, hdPrecioFuturoTrz As HtmlInputHidden,
    chkPorcentaje As CheckBox, tbIntermediario As TextBox, lbClase As Label, chkSelect As CheckBox, ddlMonedaNegF As DropDownList, ddlMonedaF As DropDownList,
    ddlOperacionF As DropDownList, ddlModalidadF As DropDownList, ddlMedioNegF As DropDownList, ddlMotivoF As DropDownList, ddlClaseInstrumentofxF As DropDownList,
    tbHora2 As TextBox, ddlClaseInstrumentofx2 As DropDownList, ddlMonedaNeg2 As DropDownList, ddlMoneda2 As DropDownList, ddlOperacion2 As DropDownList,
    tbTotalOrden2 As TextBox, tbPrecio2 As TextBox, ddlModalidad2 As DropDownList, tbFechaLiquidacion2 As TextBox, tbIntermediario2 As TextBox, ddlMedioNeg2 As DropDownList,
    ddlMotivo2 As DropDownList, chkPorcentaje2 As CheckBox, hdPorcentaje2 As HtmlInputHidden, tbTotalOperacion As TextBox, tbFechaLiquidacionF As TextBox,
    hdIntermediarioF As HtmlInputHidden, tbPrecioF As TextBox, tbTotalOrdenF As TextBox, chkPorcentajeF As CheckBox, porcentajeF As String, dtDetalleInversiones As DataTable
    'OT10855 - Agregar tipos de campos TextBox para obtener el tipo de cambio futuro
    Dim tbPrecioFuturo As TextBox, tbPrecioFuturoF As TextBox, tbPrecioFuturo2 As TextBox, HdFechaOperacion As HiddenField, HdCodigoPortafolioSBS As HiddenField
    'OT 10090 Fin
    Dim _filaTraz As TrazabilidadOperacionBE.TrazabilidadOperacionRow
    Dim oTrazabilidadOperacionBE As New TrazabilidadOperacionBE
    Dim pRutas As String
    Dim BMPrevOrden As New PrevOrdenInversionBE
    Dim rutas As New System.Text.StringBuilder
    Dim Cro As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Dim oOperacionBM As New OperacionBM, objPortafolioBM As New PortafolioBM
    Private Sub AgregarFilaTrazabilidad(ByRef _fila As TrazabilidadOperacionBE.TrazabilidadOperacionRow, ByVal fila As GridViewRow, ByVal CodigoPrevOrden As String,
    ByVal Nemonico As String, ByVal Operacion As String, ByVal Cantidad As String, ByVal Precio As String, ByVal Intermediario As String, ByVal CantidadOperacion As String,
    ByVal precioEjecutado As String, ByVal CodigoPortafolio As String, ByVal Asignacion As String)
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
            'CargarPortafolio()
            If Not Page.IsPostBack Then
                CargarLoading("btnBuscar")
                'InicializarBotones()
                CargarPagina()
            Else
                Dim _hdFechaOperacionF As HiddenField
                Dim _tFechaOperacionF As TextBox
                _hdFechaOperacionF = CType(Datagrid1.FooterRow.FindControl("hdFechaOperacionF"), HiddenField)
                _tFechaOperacionF = CType(Datagrid1.FooterRow.FindControl("tFechaOperacionF"), TextBox)
                _tFechaOperacionF.Text = _hdFechaOperacionF.Value
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página: " & Replace(ex.Message, "'", ""))
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
        dtOperacion = oOperacionBM.SeleccionarOperacionesFx().Tables(0)
        Session("dtOperacion") = dtOperacion
        Dim dtMoneda As New DataTable
        Dim oMonedaBM As New MonedaBM
        dtMoneda = oMonedaBM.Listar(ESTADO_ACTIVO).Tables(0)
        Session("dtMoneda") = dtMoneda
        Dim dtMedioNeg As New DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtMedioNeg = oParametrosGeneralesBM.ListarMedioNegociacionPrevOI(ParametrosSIT.TR_RENTA_FIJA).Tables(0)
        Session("dtMedioNeg") = dtMedioNeg
        Dim dtMotivo As New DataTable
        Dim oMotivoBM As New MotivoBM
        dtMotivo = oMotivoBM.Listar(DatosRequest).Tables(0)
        Session("dtMotivo") = dtMotivo
        Dim dtModforw As New DataTable
        dtModforw = oParametrosGeneralesBM.Listar(ParametrosSIT.MODALIDAD_FORW, DatosRequest)
        Session("dtModforw") = dtModforw
        Dim dtClaseifx As New DataTable
        dtClaseifx = oParametrosGeneralesBM.Listar(ParametrosSIT.CLASE_INSTRUMENTO_FX, DatosRequest)
        Session("dtClaseifx") = dtClaseifx
        CargarGrilla("FX", ViewState("decFechaOperacion"))
        Session.Remove("dtDetalleInversiones")
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
    Private Sub CargarGrilla(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, Optional ByVal strCodigoClaseInstrumento As String = "", _
    Optional ByVal strOperador As String = "", Optional ByVal strEstado As String = "")
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim ds As DataSet = oPrevOrdenInversionBM.SeleccionarPorFiltro(strTipoRenta, decFechaOperacion, strCodigoClaseInstrumento, "", "", strOperador, _
        strEstado, DatosRequest)
        hdGrillaRegistros.Value = ds.Tables(0).Rows.Count
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
            montoOperacion = Math.Round(montoDestino, 7)
        ElseIf tipoDI = "I" Then
            decTipoCambio = tipoCambio
            montoOrigen = montoNominal
            montoDestino = montoOrigen / decTipoCambio
            montoOperacion = Math.Round(montoDestino, 7)
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
        Return Math.Round(montoOperacion, 2)
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
    Private Sub HabilitaControles(ByVal habilita As Boolean)
        'INICIO | RCE | Zoluxiones | Se deshabilita por desuso | 13/08/2018
        'btnGrabar.Enabled = habilita
        'btnValidar.Enabled = habilita
        'btnSwapDivisa.Enabled = habilita
        'btnAprobar.Enabled = habilita
        'Datagrid1.Enabled = habilita
        'FIN | RCE | Zoluxiones | Se deshabilita por desuso | 13/08/2018
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim decFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            If decFechaOperacion = hdFechaNegocio.Value And hdPuedeNegociar.Value = "1" Then
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
            CargarGrilla("FX", decFechaOperacion, strCodigoClaseInstrumento, strOperador, strEstado)
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            btnBuscar.Text = "Buscar"
            btnBuscar.Enabled = True
        End Try
    End Sub
    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
            Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
            Dim dtDetalleInversiones As DataTable = Nothing
            Dim porcentaje As String
            Dim sNemonico As String
            Dim strCambioTrazaFondo As String = ""
            Dim dt As New DataTable
            For Each fila As GridViewRow In Datagrid1.Rows
                lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                hdCambio = CType(fila.FindControl("hdCambio"), HtmlInputHidden)
                hdCambioTraza = CType(fila.FindControl("hdCambioTraza"), HtmlInputHidden)
                hdCambioTrazaFondo = CType(fila.FindControl("hdCambioTrazaFondo"), HtmlInputHidden)
                chkPorcentaje = CType(fila.FindControl("chkPorcentaje"), CheckBox)

                'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 17/05/2018
                porcentaje = "N"
                'If porcentaje.Equals("N") Then
                '    If Not Session("dtDetalleInversiones" & lbCodigoPrevOrden.Text.Trim) Is Nothing Then
                '        dtDetalleInversiones = instanciarTabla(dtDetalleInversiones)
                '        dtDetalleInversiones = Session("dtDetalleInversiones" & lbCodigoPrevOrden.Text.Trim)
                '        Session.Remove("dtDetalleInversiones" & lbCodigoPrevOrden.Text.Trim)
                '    End If
                'End If
                'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 17/05/2018

                If fila.Cells(2).Text = PREV_OI_INGRESADO Or Not dtDetalleInversiones Is Nothing Then
                    If (Not lbCodigoPrevOrden Is Nothing And hdCambio.Value = "1") Or Not dtDetalleInversiones Is Nothing Then
                        ddlOperacion = CType(fila.FindControl("ddlOperacion"), DropDownList)
                        ddlMedioNeg = CType(fila.FindControl("ddlMedioNeg"), DropDownList)
                        ddlClaseInstrumentofx = CType(fila.FindControl("ddlClaseInstrumentofx"), DropDownList)
                        ddlMonedaNeg = CType(fila.FindControl("ddlMonedaNeg"), DropDownList)
                        ddlMoneda = CType(fila.FindControl("ddlMoneda"), DropDownList)
                        ddlModalidad = CType(fila.FindControl("ddlModalidad"), DropDownList)
                        ddlMotivo = CType(fila.FindControl("ddlMotivo"), DropDownList)
                        tbFechaLiquidacion = CType(fila.FindControl("tbFechaLiquidacion"), TextBox)
                        hdIntermediario = CType(fila.FindControl("hdIntermediario"), HtmlInputHidden)
                        tbPrecio = CType(fila.FindControl("tbPrecio"), TextBox)
                        tbPrecioFuturo = CType(fila.FindControl("tbPrecioFuturo"), TextBox) 'OT10855 10/10/2017
                        tbTotalOrden = CType(fila.FindControl("tbTotalOrden"), TextBox)
                        tbHora = CType(fila.FindControl("tbHora"), TextBox)
                        hdCodigoPais = CType(fila.FindControl("hdCodigoPais"), HtmlInputHidden)
                        Dim bolValidar As Boolean = False
                        Select Case ddlClaseInstrumentofx.SelectedValue
                            Case ParametrosSIT.CLASE_INSTRUMENTO_FORWARD
                                If ddlMonedaNeg.SelectedValue <> "" And ddlMoneda.SelectedValue <> "" And tbTotalOrden.Text <> "" And tbPrecio.Text <> "" And _
                                    ddlModalidad.SelectedValue <> "" And tbFechaLiquidacion.Text <> "" And _
                                    hdIntermediario.Value <> "" And ddlMotivo.SelectedValue <> "" Then
                                    If IsNumeric(tbTotalOrden.Text) And IsNumeric(tbPrecio.Text) And IsDate(tbFechaLiquidacion.Text) Then
                                        If ddlMoneda.SelectedValue <> ddlMonedaNeg.SelectedValue Then
                                            If validaFechas(ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)) Then
                                                If fila.Cells(2).Text <> ParametrosSIT.PREV_OI_EJECUTADO And fila.Cells(2).Text <> ParametrosSIT.PREV_OI_APROBADO And
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
                                    oRow.HoraOperacion = Now.ToLongTimeString()
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
                                    oRow.PrecioFuturo = CType(tbPrecioFuturo.Text, Decimal) 'OT10855 10/10/2017
                                    oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
                                    oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
                                    oRow.CodigoMotivo = ddlMotivo.SelectedValue
                                    oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                                    oRow.Estado = PREV_OI_INGRESADO
                                    'OT10855 10/10/2017
                                    oRow.MontoOperacion = obtenerMontoOperacion(ddlMonedaNeg.SelectedValue, ddlMoneda.SelectedValue, CType(tbPrecioFuturo.Text, Decimal), _
                                    CType(tbTotalOrden.Text, Decimal))
                                    oRow.Porcentaje = porcentaje
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                                    sNemonico = ddlClaseInstrumentofx.SelectedItem.Text

                                    dtDetalleInversiones = instanciarTabla(dtDetalleInversiones)
                                    Dim dtDetallePortafolio As DataTable = oPrevOrdenInversionBM.SeleccionarDetallePreOrdenInversion(CType(lbCodigoPrevOrden.Text.Trim, Decimal))
                                    dtDetalleInversiones.Rows.Add(CType(lbCodigoPrevOrden.Text.Trim, Decimal), dtDetallePortafolio.Rows(0)("CodigoPortafolio"), CType(tbTotalOrden.Text, Decimal), "N")

                                End If
                            Case ParametrosSIT.CLASE_INSTRUMENTO_CVME
                                If ddlMonedaNeg.SelectedValue <> "" And ddlMoneda.SelectedValue <> "" And tbTotalOrden.Text <> "" And _
                                    tbPrecio.Text <> "" And tbFechaLiquidacion.Text <> "" And hdIntermediario.Value <> "" Then
                                    If IsNumeric(tbTotalOrden.Text) And IsNumeric(tbPrecio.Text) And IsDate(tbFechaLiquidacion.Text) Then
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
                                If bolValidar Then
                                    oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                                    oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                                    oRow.CodigoPrevOrden = lbCodigoPrevOrden.Text
                                    oRow.HoraOperacion = Now.ToLongTimeString()
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
                                    oRow.MontoOperacion = obtenerMontoOperacion(ddlMonedaNeg.SelectedValue, ddlMoneda.SelectedValue, CType(tbPrecio.Text, Decimal), _
                                    CType(tbTotalOrden.Text, Decimal))
                                    oRow.Porcentaje = porcentaje
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                                    sNemonico = ddlClaseInstrumentofx.SelectedItem.Text

                                    dtDetalleInversiones = instanciarTabla(dtDetalleInversiones)
                                    Dim dtDetallePortafolio As DataTable = oPrevOrdenInversionBM.SeleccionarDetallePreOrdenInversion(CType(lbCodigoPrevOrden.Text.Trim, Decimal))
                                    dtDetalleInversiones.Rows.Add(CType(lbCodigoPrevOrden.Text.Trim, Decimal), dtDetallePortafolio.Rows(0)("CodigoPortafolio"), CType(tbTotalOrden.Text, Decimal), "N")

                                End If
                        End Select
                        If bolValidar Then
                            If hdCambioTraza.Value = "1" Then
                                hdCambioTraza.Value = ""
                                hdClaseInstrumentofxTrz = CType(fila.FindControl("hdClaseInstrumentofxTrz"), HtmlInputHidden)
                                hdOperacionTrz = CType(fila.FindControl("hdOperacionTrz"), HtmlInputHidden)
                                hdTotalOrdenTrz = CType(fila.FindControl("hdTotalOrdenTrz"), HtmlInputHidden)
                                hdPrecioTrz = CType(fila.FindControl("hdPrecioTrz"), HtmlInputHidden)
                                hdPrecioFuturoTrz = CType(fila.FindControl("hdPrecioFuturoTrz"), HtmlInputHidden)
                                tbIntermediario = CType(fila.FindControl("tbIntermediario"), TextBox)
                                If hdCambioTrazaFondo.Value = "1" Then
                                    strCambioTrazaFondo = "1"
                                End If
                                Dim CantOpe As Decimal = 0
                                Dim codnemo As String = String.Empty
                                codnemo = hdClaseInstrumentofxTrz.Value
                                CantOpe = obtenerMontoOperacion(ddlMonedaNeg.SelectedValue, ddlMoneda.SelectedValue, CType(tbPrecio.Text, Decimal),
                                CType(tbTotalOrden.Text, Decimal))
                                If Not dtDetalleInversiones Is Nothing Then
                                    For Each dr As DataRow In dtDetalleInversiones.Rows
                                        _filaTraz = oTrazabilidadOperacionBE.TrazabilidadOperacion.NewTrazabilidadOperacionRow()
                                        AgregarFilaTrazabilidad(_filaTraz, fila, lbCodigoPrevOrden.Text, hdClaseInstrumentofxTrz.Value, ddlOperacion.SelectedValue,
                                        CType(hdTotalOrdenTrz.Value, Decimal), tbPrecio.Text, hdIntermediario.Value, CantOpe, CType(Val(tbPrecio.Text), Decimal),
                                        dr("CodigoPortafolio"), dr("Asignacion").ToString.Trim)
                                        oTrazabilidadOperacionBE.TrazabilidadOperacion.AddTrazabilidadOperacionRow(_filaTraz)
                                    Next
                                Else
                                    _filaTraz = oTrazabilidadOperacionBE.TrazabilidadOperacion.NewTrazabilidadOperacionRow()
                                    AgregarFilaTrazabilidad(_filaTraz, fila, lbCodigoPrevOrden.Text, hdClaseInstrumentofxTrz.Value, ddlOperacion.SelectedValue,
                                    CType(hdTotalOrdenTrz.Value, Decimal), tbPrecio.Text, hdIntermediario.Value, CantOpe, CType(Val(tbPrecio.Text), Decimal), "", 0)
                                    oTrazabilidadOperacionBE.TrazabilidadOperacion.AddTrazabilidadOperacionRow(_filaTraz)
                                End If
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
                If Not dtDetalleInversiones Is Nothing Then
                    If dtDetalleInversiones.Rows.Count > 0 Then
                        For i = 0 To dtDetalleInversiones.Rows.Count - 1
                            oPrevOrdenInversionBM.eliminarDetalle(Integer.Parse(dtDetalleInversiones.Rows(i)("CodigoPrevOrden").ToString.Trim), _
                            dtDetalleInversiones.Rows(i)("CodigoPortafolio").ToString.Trim)
                        Next
                        For i = 0 To dtDetalleInversiones.Rows.Count - 1
                            If Decimal.Parse(dtDetalleInversiones.Rows(i)("Asignacion").ToString.Trim) <> 0 Then
                                oPrevOrdenInversionBM.insertarDetalle(Integer.Parse(dtDetalleInversiones.Rows(i)("CodigoPrevOrden").ToString.Trim), _
                                dtDetalleInversiones.Rows(i)("CodigoPortafolio").ToString.Trim, Decimal.Parse(dtDetalleInversiones.Rows(i)("Asignacion").ToString.Trim))
                            End If
                        Next
                    End If
                End If
                oRow = Nothing
                CargarGrilla("FX", ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strOperador"), _
                ViewState("strEstado"))
                AlertaJS("Grabación exitosa")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        Finally
            btnGrabar.Text = "Grabar"
            btnGrabar.Enabled = True
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
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range

        Dim sFile As String, sTemplate As String
        Dim dtOperacionFx As New DataTable
        Dim dtResumen As New DataTable
        Dim dtResumenSBS As New DataTable
        Dim oDs As New DataSet
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim decNProceso As Decimal = 0
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            oDs = oPrevOrdenInversionBM.GenerarReporte("FX", ViewState("decFechaOperacion"), DatosRequest, decNProceso)
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            dtOperacionFx = oDs.Tables(0)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "FX_" & Usuario.ToString() & _
            String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            Dim n As Integer
            Dim n2 As Long
            Dim dr As DataRow

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
            Dim codpreorden As String = ""
            For Each dr In dtOperacionFx.Rows
                n2 = n + 1
                oSheet.Rows(n & ":" & n).Copy()
                oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                oSheet.Application.CutCopyMode = False
                If Not codpreorden = dr("CodigoPrevOrden").ToString() Then
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
                    oCells(n, 16) = dr("Motivo")
                    oSheet.Range("A" + n.ToString() + ":P" + n.ToString()).Interior.Color = RGB(215, 211, 211)
                    n = n + 1
                    n2 = n + 1
                    oSheet.Rows(n & ":" & n).Copy()
                    oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                    oSheet.Application.CutCopyMode = False
                    oCells(n, 14) = dr("Descripcion")
                    oCells(n, 15) = dr("Asignacion")
                Else
                    oCells(n, 14) = dr("Descripcion")
                    oCells(n, 15) = dr("Asignacion")
                End If
                codpreorden = dr("CodigoPrevOrden")
                n = n + 1
            Next
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "FX_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
            Response.WriteFile(sFile)
            Response.End()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            oExcel.Quit()
            ReleaseComObject(oExcel)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub
    Private Sub btnAprobar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim strCodPrevOrden As String = ""
        Dim ds As New DataSet
        Dim strEstadoPrevOrden As String = ""
        Dim decNProceso As Decimal = 0
        Dim arrCodPrevOrden As Array
        Try
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            Dim count As Decimal = 0, contFechaDiferenteNegocio As Integer = 0, strMensajeDiferenteFechaNegocio As String = "<p align=left>"
            For Each fila As GridViewRow In Datagrid1.Rows
                Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                Dim fondos As String = CType(fila.FindControl("hdFondo1Trz"), HtmlInputHidden).Value
                HdFechaOperacion = CType(fila.FindControl("HdFechaOperacion"), HiddenField)
                HdCodigoPortafolioSBS = CType(fila.FindControl("HdCodigoPortafolioSBS"), HiddenField)
                Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio(HdCodigoPortafolioSBS.Value.Trim)
                Dim decCodigoPrevOrden As Decimal
                strEstadoPrevOrden = oPrevOrdenInversionBM.ObtenerEstadoPrevOrdenInversion(CType(lbCodigoPrevOrden.Text, Decimal), ds) 'agregado por JH , para no generar duplicados
                If chkSelect.Checked = True Then
                    If fila.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                        If HdFechaOperacion.Value.Trim <> fechaNegocio.ToString Then
                            contFechaDiferenteNegocio += 1
                            strMensajeDiferenteFechaNegocio += "-> N°: " + fila.Cells(1).Text + " / Portafolio: " + fondos + _
                                                               "<br> F. Operación: " + UIUtility.ConvertirFechaaString(HdFechaOperacion.Value.Trim) + _
                                                               " / F. Portafolio: " + UIUtility.ConvertirFechaaString(fechaNegocio) + "<br><br>"
                        Else
                            decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                            oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                            count = count + 1
                            strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text & "|"
                        End If
                    End If
                End If
            Next
            If contFechaDiferenteNegocio > 0 Then
                AlertaJS("Verificar las siguiente negociaciones, las fecha de operación no coincide con la fecha negocio del portafolio: " + _
                         strMensajeDiferenteFechaNegocio + "</p>")
            ElseIf Not strCodPrevOrden = "" Then
                strCodPrevOrden = strCodPrevOrden.Substring(0, strCodPrevOrden.Length - 1)

                If count > 0 Then
                    EjecutarOrdenInversion("FX", ViewState("decFechaOperacion"), strCodPrevOrden, , , decNProceso)
                Else
                    AlertaJS("Seleccione el registro a ejecutar ó el registro ya se encontraba ejecutado!")
                    CargarGrilla("FX", ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                            ViewState("strOperador"), ViewState("strEstado"))
                    'OT-10784 Inicio
                    ''Se eliminan todos los procesos
                    'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
                    ''Se setea el flag a "0" de las órdenes afectadas
                    'arrCodPrevOrden = strCodPrevOrden.Split("|")
                    'For i = 0 To arrCodPrevOrden.Length - 1
                    '    If arrCodPrevOrden(i) <> "" Then
                    '        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                    '    End If
                    'Next
                    'OT-10784 Fin
                End If
            Else
                AlertaJS("Ningun registro cumple el requerimiento de estado.")
                CargarGrilla("FX", ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                           ViewState("strOperador"), ViewState("strEstado"))
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            'OT-10784 Inicio
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            arrCodPrevOrden = strCodPrevOrden.Split("|")
            For i = 0 To arrCodPrevOrden.Length - 1
                If arrCodPrevOrden(i) <> "" Then
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                End If
            Next
            'OT-10784 Fin
            btnAprobar.Text = "Ejecutar"
            btnAprobar.Enabled = True
        End Try
    End Sub
    Private Sub btnValidar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnValidar.Click
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
        Dim count As Integer = 0
        Dim decNProceso As Decimal = 0
        Dim strCodPrevOrden As String = ""
        Dim arrCodPrevOrden As Array
        Try
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Session("Login"))
            For Each fila As GridViewRow In Datagrid1.Rows
                lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                If fila.Cells(2).Text <> PREV_OI_EJECUTADO Then
                    If Not lbCodigoPrevOrden Is Nothing Then
                        'PROCESAR VALIDACION
                        Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                        Dim decCodigoPrevOrden As Decimal
                        If chkSelect.Checked = True Then
                            decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                            oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                            count = count + 1
                            strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text.Trim & "|"
                        End If
                    End If
                End If
            Next
            If count > 0 Then
                Limites.VerificaExcesoLimites(Me.Usuario, "FX", decNProceso)
                Dim dt As New DataTable
                dt = oPrevOrdenInversionBM.SeleccionarValidacionExcesos("FX", _
                ViewState("decFechaOperacion"), DatosRequest, decNProceso).Tables(0)
                If dt.Rows.Count > 0 Then
                    Session("dtListaExcesos") = dt
                    EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" + ParametrosSIT.TR_RENTA_VARIABLE.ToString() + _
                    "', '800', '550','" & btnBuscar.ClientID & "');")
                End If
            Else
                AlertaJS("Seleccione los registros a validar!")
                'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            End If
            'OT-10784 Inicio
            'arrCodPrevOrden = strCodPrevOrden.Split("|")
            'For i = 0 To arrCodPrevOrden.Length - 1
            '    If arrCodPrevOrden(i) <> "" Then
            '        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
            '    End If
            'Next
            'OT-10784 Fin
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            'OT-10784 Inicio
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            arrCodPrevOrden = strCodPrevOrden.Split("|")
            For i = 0 To arrCodPrevOrden.Length - 1
                If arrCodPrevOrden(i) <> "" Then
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                End If
            Next
            'OT-10784 Fin
        End Try
    End Sub
    Private Sub btnValidarTrader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidarTrader.Click
        Dim dtValidaTrader As New DataTable
        Dim oLimiteTradingBM As New LimiteTradingBM
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim ds As New DataSet
        Dim decNProceso As Decimal = 0
        Dim strCodPrevOrden As String = ""
        Dim arrCodPrevOrden As Array
        Dim strEstadoPrevOrden As String = ""
        Try
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            Dim count As Decimal = 0, contFechaDiferenteNegocio As Integer = 0, strMensajeDiferenteFechaNegocio As String = "<p align=left>"
            For Each fila As GridViewRow In Datagrid1.Rows
                Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                Dim fondos As String = CType(fila.FindControl("hdFondo1Trz"), HtmlInputHidden).Value
                HdFechaOperacion = CType(fila.FindControl("HdFechaOperacion"), HiddenField)
                HdCodigoPortafolioSBS = CType(fila.FindControl("HdCodigoPortafolioSBS"), HiddenField)
                Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio(HdCodigoPortafolioSBS.Value.Trim)
                Dim decCodigoPrevOrden As Decimal
                strEstadoPrevOrden = oPrevOrdenInversionBM.ObtenerEstadoPrevOrdenInversion(CType(lbCodigoPrevOrden.Text, Decimal), ds) 'agregado por JH , para no generar duplicados
                If chkSelect.Checked = True Then
                    If fila.Cells(2).Text = ParametrosSIT.PREV_OI_INGRESADO Then
                        If HdFechaOperacion.Value.Trim <> fechaNegocio Then
                            contFechaDiferenteNegocio += 1
                            strMensajeDiferenteFechaNegocio += "-> N°: " + fila.Cells(1).Text + " / Portafolio: " + fondos + _
                                                              "<br> F. Operación: " + UIUtility.ConvertirFechaaString(HdFechaOperacion.Value.Trim) + _
                                                              " / F. Portafolio: " + UIUtility.ConvertirFechaaString(fechaNegocio) + "<br><br>"
                        Else
                            decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                            oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                            count = count + 1
                            strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text & "|"
                        End If
                    End If
                End If
            Next
            If contFechaDiferenteNegocio > 0 Then
                AlertaJS("Verificar las siguiente negociaciones, las fecha de operación no coincide con la fecha negocio del portafolio: " + _
                         strMensajeDiferenteFechaNegocio + "</p>")
            ElseIf count > 0 Then
                dtValidaTrader = oLimiteTradingBM.SeleccionarValidacionExcesosTrader_Sura("FX", ViewState("decFechaOperacion"), _
                Usuario, DatosRequest, , decNProceso).Tables(0)
                If dtValidaTrader.Rows.Count > 0 Then
                    Session("dtValidaTrader") = dtValidaTrader
                    EjecutarJS("showModalDialog('frmValidacionExcesosTrader.aspx?TipoRenta=" & "FX" & _
                    "&nProc=" & decNProceso.ToString() + "', '800', '550','" & btnBuscar.ClientID & "');")
                Else
                    AlertaJS("No se han podido evaluar los limites trader.")
                    'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)'OT-10784
                End If
            Else
                AlertaJS("Seleccione el registro a validar ó el registro ya se encontraba aprobado!")
                CargarGrilla("FX", ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                            ViewState("strOperador"), ViewState("strEstado"))
                'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso) 'OT-10784
            End If
            'OT-10784 Inicio
            'arrCodPrevOrden = strCodPrevOrden.Split("|")
            'For i = 0 To arrCodPrevOrden.Length - 1
            '    If arrCodPrevOrden(i) <> "" Then
            '        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
            '    End If
            'Next
            'OT-10784 Fin
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            'OT-10784 Inicio
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            arrCodPrevOrden = strCodPrevOrden.Split("|")
            For i = 0 To arrCodPrevOrden.Length - 1
                If arrCodPrevOrden(i) <> "" Then
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                End If
            Next
            'OT-10784 Fin
            btnValidarTrader.Text = "Validar Exc. Trader"
            btnValidarTrader.Enabled = True
        End Try
    End Sub
    Private Sub btnSwapDivisa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSwapDivisa.Click
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim bolResult As Boolean = False
        Dim decNProceso As Decimal = 0
        Dim strCodPrevOrden As String = ""
        Dim arrCodPrevOrden As Array
        Try
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Session("Login"))
            For Each fila As GridViewRow In Datagrid1.Rows
                Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                Dim decCodigoPrevOrden As Decimal
                If chkSelect.Checked = True Then
                    If fila.Cells(2).Text <> PREV_OI_EJECUTADO Then
                        decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                        oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                        strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text & "|"
                    End If
                End If
            Next
            bolResult = oPrevOrdenInversionBM.ProcesarSwapDivisa(ViewState("decFechaOperacion"), DatosRequest, decNProceso)
            If bolResult = True Then
                AlertaJS("Proceso realizado satisfactoriamente!")
            Else
                AlertaJS("Procesa no valido, verifíque si las condiciones son correctas!")
            End If
            'OT-10784 Inicio
            'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            'arrCodPrevOrden = strCodPrevOrden.Split("|")
            'For i = 0 To arrCodPrevOrden.Length - 1
            '    If arrCodPrevOrden(i) <> "" Then
            '        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
            '    End If
            'Next
            'OT-10784 Fin
            CargarGrilla("FX", ViewState("decFechaOperacion"))
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            'OT-10784 Inicio
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            arrCodPrevOrden = strCodPrevOrden.Split("|")
            For i = 0 To arrCodPrevOrden.Length - 1
                If arrCodPrevOrden(i) <> "" Then
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                End If
            Next
            'OT-10784 Fin
        End Try
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM
        Dim ds As DataSet
        Dim dtOI As DataTable
        Dim strCodigoOrden As String = ""
        Dim strPortafolioSBS As String = ""
        Dim PortafolioSBS As String = ""
        Dim strMoneda As String = ""
        Dim strOperacion As String = ""
        Dim strCodigoISIN As String = ""
        Dim strCodigoSBS As String = ""
        Dim strCodigoMnemonico As String = ""
        Dim strClase As String = ""
        Dim strCatClase As String = ""
        Try
            For Each fila As GridViewRow In Datagrid1.Rows
                chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)
                If fila.Cells(2).Text = PREV_OI_EJECUTADO And chkSelect.Checked = True Then
                    lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                    lbClase = CType(fila.FindControl("lbClase"), Label)
                    ddlClaseInstrumentofx = CType(fila.FindControl("ddlClaseInstrumentofx"), DropDownList)
                    ds = oPrevOrdenInversion.SeleccionarImprimir_PrevOrdenInversion(lbCodigoPrevOrden.Text, DatosRequest)
                    dtOI = ds.Tables(0)
                    For Each fila2 As DataRow In dtOI.Rows
                        strCodigoOrden = fila2("CodigoOrden")
                        strPortafolioSBS = fila2("CodigoPortafolioSBS")
                        PortafolioSBS = fila2("PortafolioSBS")
                        Try
                            strMoneda = New MonedaBM().SeleccionarPorFiltro(fila2("Moneda"), String.Empty, String.Empty, String.Empty, _
                            String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
                        Catch ex As Exception
                            strMoneda = ""
                        End Try
                        strOperacion = fila2("Operacion")
                        Session("dtdatosoperacion") = Nothing
                        strCodigoSBS = ""
                        strCatClase = ddlClaseInstrumentofx.SelectedValue
                        If strCatClase = CLASE_INSTRUMENTO_FORWARD Then
                            strClase = CLASE_LLAMADO_FORWARD
                            strCodigoMnemonico = ddlClaseInstrumentofx.SelectedItem.Text
                        ElseIf strCatClase = CLASE_INSTRUMENTO_CVME Then
                            strClase = CLASE_LLAMADO_CVME
                            strCodigoMnemonico = ddlClaseInstrumentofx.SelectedItem.Text
                        End If
                        GenerarLlamado(strCodigoOrden, strPortafolioSBS, PortafolioSBS, strClase, strOperacion, strMoneda, strCodigoISIN, strCodigoSBS, strCodigoMnemonico)
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
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal codportafolio As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, _
    ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        Dim strtitulo, strnemonico, strisin, strsbs, strsubtitulo, strcodigo1, strcodigo, strportafolio, strclase, strmoneda, stroperacion, strmnemonicotemp As String
        Dim dscaract As New dscaracteristicas
        Dim dttempoperacion As DataTable
        strclase = clase
        strnemonico = mnemonico
        strisin = isin
        strsbs = sbs
        strmnemonicotemp = ""
        strportafolio = codportafolio
        strcodigo1 = codigo
        Dim dsValor As New DataSet
        Dim oOIFormulas As New OrdenInversionFormulasBM
        If strclase = CLASE_LLAMADO_DEPOSITOPLAZO Then
            If Session("dtdatosoperacion") Is Nothing Then
                Dim dtOrden As DataTable = New OrdenPreOrdenInversionBM().ListarOrdenesInversionPorCodigoOrden(strcodigo1, strportafolio, _
                DatosRequest, PORTAFOLIO_MULTIFONDOS).Tables(0)
                Session("dtdatosoperacion") = New DepositoPlazos().ObtenerDatosOperacion(DatosRequest, dtOrden.Rows(0))
            End If
        End If
        If strclase = CLASE_LLAMADO_FORWARD Then
            If Session("dtdatosoperacion") Is Nothing Then
                Dim dtOrden As DataTable = New OrdenPreOrdenInversionBM().ListarOrdenesInversionPorCodigoOrden(strcodigo1, strportafolio, DatosRequest, _
                PORTAFOLIO_MULTIFONDOS).Tables(0)
                Session("dtdatosoperacion") = ObtenerDatosOperacionFDCV(strclase, dtOrden.Rows(0))
            End If
        End If
        If strclase = CLASE_LLAMADO_CVME Then
            If Session("dtdatosoperacion") Is Nothing Then
                Dim dtOrden As DataTable = New OrdenPreOrdenInversionBM().ListarOrdenesInversionPorCodigoOrden(strcodigo1, strportafolio, DatosRequest, _
                PORTAFOLIO_MULTIFONDOS).Tables(0)
                Session("dtdatosoperacion") = ObtenerDatosOperacionFDCV(strclase, dtOrden.Rows(0))
            End If
        End If
        stroperacion = operacion
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
        Dim Archivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Llamado/RptLlamado.rpt"
        strcodigo = strcodigo1
        strmoneda = moneda
        'Validar si se trata de una Traspaso de Instrumento entre Fondos
        Dim ordenes As String() = strcodigo.Split("-")
        Dim ordenOrigen As String = String.Empty
        Dim ordenDestino As String = String.Empty
        Dim esTraspaso As Boolean = False
        If ordenes.Length > 1 Then
            esTraspaso = True
            ordenOrigen = ordenes(0).ToString()
            ordenDestino = ordenes(1).ToString()
        End If
        Try
            Dim StrNombre As String = "Usuario"
            Dim strusuario As String
            If esTraspaso Then
                strtitulo = "Orden de Inversion: (Origen) Nro - " + ordenOrigen + " , (Destino) Nro - " + ordenDestino
            Else
                strtitulo = "Orden de Inversion Nro - " + strcodigo
            End If
            strsubtitulo = strclase + " - " + stroperacion
            Dim dscomisiones As New DataSet
            Dim dscomi As New dsdatoscomisiones
            Dim drcomi As DataRow
            dscomisiones = UIUtility.ObtenerTablaimpuestosComisionesGuardado(strcodigo, strportafolio)
            For Each drcomisiones As DataRow In dscomisiones.Tables(0).Rows
                drcomi = dscomi.Tables(0).NewRow
                drcomi("Descripcion1") = drcomisiones("Descripcion1")
                drcomi("ValorOcultoComision1") = drcomisiones("ValorOcultoComision1")
                drcomi("PorcentajeComision1") = drcomisiones("PorcentajeComision1") + " :"
                drcomi("Descripcion2") = drcomisiones("Descripcion2")
                drcomi("ValorOcultoComision2") = drcomisiones("ValorOcultoComision2")
                drcomi("PorcentajeComision2") = drcomisiones("PorcentajeComision2") + " :"
                dscomi.Tables(0).Rows.Add(drcomi)
            Next
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            strusuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)
            Cro.Load(Archivo)
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
            If strisin <> "" Or strisin <> Nothing Then
                Cro.SetParameterValue("@CodigoIsin", "Codigo Isin: " & strisin)
            Else
                Cro.SetParameterValue("@CodigoIsin", "")
            End If
            If strsbs <> "" Or strsbs <> Nothing Then
                Cro.SetParameterValue("@CodigoSBS", "Codigo SBS: " & strsbs)
            Else
                Cro.SetParameterValue("@CodigoSBS", "")
            End If
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
            Dim sfile As String
            sfile = folderActual & "\" & nombreNuevoArchivo
            Response.Clear()
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nombreNuevoArchivo)
            Response.WriteFile(sfile)
            Response.End()
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
    Public Function ObtenerDatosOperacionFDCV(ByVal pClase As String, ByVal drOrden As DataRow) As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", _
        "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", _
        "v20", "c21", "v21", "c22", "v22"}
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
                drGrilla("v4") = New MonedaBM().SeleccionarPorFiltro(drOrden("CodigoMoneda"), String.Empty, String.Empty, String.Empty, _
                String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
            Catch ex As Exception
                drGrilla("v4") = ""
            End Try
            drGrilla("c5") = "Monto Divisa Negociada"
            drGrilla("v5") = Format(drOrden("MontoOrigen"), "#,##0.0000000")
            drGrilla("c6") = "A :"
            Try
                drGrilla("v6") = New MonedaBM().SeleccionarPorFiltro(drOrden("CodigoMonedaDestino"), String.Empty, String.Empty, String.Empty, _
                String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
            Catch ex As Exception
                drGrilla("v6") = ""
            End Try
            drGrilla("c7") = "Monto"
            drGrilla("v7") = Format(drOrden("MontoDestino"), "#,##0.0000000")
            drGrilla("c8") = "Tipo Cambio"
            drGrilla("v8") = Format(drOrden("TipoCambio"), "#,##0.0000000")
            drGrilla("c9") = "Intermediario"
            drGrilla("v9") = New TercerosBM().SeleccionarPorFiltro(drOrden("CodigoTercero"), String.Empty, String.Empty, String.Empty, String.Empty, _
            String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
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
                drGrilla("v7") = New MonedaBM().SeleccionarPorFiltro(drOrden("CodigoMonedaOrigen"), String.Empty, String.Empty, String.Empty, _
                String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
            Catch ex As Exception
                drGrilla("v7") = ""
            End Try
            drGrilla("c8") = "Monto Origen"
            drGrilla("v8") = Format(drOrden("MontoCancelar"), "#,##0.0000000")
            drGrilla("c9") = "A"
            Try
                drGrilla("v9") = New MonedaBM().SeleccionarPorFiltro(drOrden("CodigoMonedaDestino"), String.Empty, String.Empty, String.Empty, _
                String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
            Catch ex As Exception
                drGrilla("v9") = ""
            End Try
            drGrilla("c10") = "Monto Futuro"
            drGrilla("v10") = Format(drOrden("MontoOperacion"), "#,##0.0000000")
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
            drGrilla("v14") = New TercerosBM().SeleccionarPorFiltro(drOrden("CodigoTercero"), String.Empty, String.Empty, String.Empty, String.Empty, _
            String.Empty, DatosRequest).Tables(0).Rows(0)("Descripcion")
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
    Private Sub GenerarReporteFXPDF()
        Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio("MULTIFONDO")
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM()
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
                    If arrCodPrevOrden(i) <> "" Then
                        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "1")
                    End If
                Next
                If bolGeneraOrden Then
                    'OT-10784 Inicio
                    If dtOrdenInversion.Rows.Count > 0 Then
                        Session("dtListaExcesos") = dtOrdenInversion
                        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + _
                        "', '1000', '500','" & btnBuscar.ClientID & "');")
                    End If
                    '    If dtOrdenInversion.Rows.Count > 0 Then
                    '        Dim Variable As String = "TmpCodigoUsuario,TmpEscenario,TmpNProceso"
                    '        Dim Parametros As String = Usuario + "," + ParametrosSIT.EJECUCION_PREVOI + "," + decNProceso.ToString
                    '        Dim obj As New JobBM
                    '        Dim mensaje As String = obj.EjecutarJob("DTS_SIT_VerificaExcesoLimitesEnLinea" & DateTime.Today.ToString("_yyyyMMdd") & _
                    '        System.DateTime.Now.ToString("_hhmmss"), "Verifica exceso de limites en linea, considerando el neteo de operaciones", Variable, _
                    '        Parametros, "", "", ConfigurationManager.AppSettings("SERVIDORETL"))
                    '        AlertaJS(mensaje)
                    '        Session("dtOrdenInversion") = dtOrdenInversion
                    '        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=OI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + _
                    '        "', '1000', '500','" & btnBuscar.ClientID & "');")
                    '    End If
                    'Else
                    '    If dtOrdenInversion.Rows.Count > 0 Then
                    '        Session("dtListaExcesos") = dtOrdenInversion
                    '        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + _
                    '        "', '1000', '500','" & btnBuscar.ClientID & "');")
                    '    End If
                End If
            End If
        End If
        'If bolGeneraOrden = False Then
        '    'Se eliminan todos los procesos
        '    oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
        '    'Se setea a "0" el flag para las órdenes afectadas
        '    arrCodPrevOrden = strCodPrevOrden.Split("|")
        '    For i = 0 To arrCodPrevOrden.Length - 1
        '        If arrCodPrevOrden(i) <> "" Then
        '            oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "1")
        '        End If
        '    Next
        'End If
        'OT-10784 Inicio
    End Sub
    Private Sub showDialogoPopupExtorno(ByVal tipoRenta As String, ByVal codigoPrevOrden As String)
        EjecutarJS("showModalDialog('frmExtornoIngresoMasivoOperacion.aspx?tipoRenta=" & tipoRenta & "&codigo=" & codigoPrevOrden + _
        "', '900', '500','" & btnBuscar.ClientID & "');")
    End Sub
    Protected Sub Datagrid1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Datagrid1.PageIndexChanging
        Datagrid1.PageIndex = e.NewPageIndex
        CargarGrilla("FX", ViewState("decFechaOperacion"))
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
                'Dim codigoPrevOrden As String = e.CommandArgument.trim
                'Dim porcentaje As String
                'chkPorcentaje = CType(gvr.FindControl("chkPorcentaje"), CheckBox)
                'If chkPorcentaje.Checked Then
                '    porcentaje = "S"
                'Else
                '    porcentaje = "N"
                'End If
                'If gvr.Cells(2).Text = PREV_OI_INGRESADO Then
                '    EjecutarJS("showModalDialog('frmAsignacionFondo.aspx?codigoprevorden=" & codigoPrevOrden & "&porcentaje=" & porcentaje + "', '650', '450','');")
                'End If
            ElseIf e.CommandName = "asignarfondoF" Then
                '    Dim codigoPrevOrden As String = e.CommandArgument.trim

                '    Dim porcentajeF As String
                '    chkPorcentajeF = CType(gvr.FindControl("chkPorcentajeF"), CheckBox)
                '    If chkPorcentajeF.Checked Then
                '        porcentajeF = "S"
                '    Else
                '        porcentajeF = "N"
                '    End If
                '    EjecutarJS("showModalDialog('frmAsignacionFondo.aspx?codigoprevorden=" & codigoPrevOrden & "&porcentaje=" & porcentajeF + "', '650', '450','');")
            ElseIf e.CommandName = "Add" Then
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
                Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
                ddlOperacionF = CType(row.FindControl("ddlOperacionF"), DropDownList)
                ddlMedioNegF = CType(row.FindControl("ddlMedioNegF"), DropDownList)
                ddlClaseInstrumentofxF = CType(row.FindControl("ddlClaseInstrumentofxF"), DropDownList)
                ddlMonedaNegF = CType(row.FindControl("ddlMonedaNegF"), DropDownList)
                ddlMonedaF = CType(row.FindControl("ddlMonedaF"), DropDownList)
                ddlModalidadF = CType(row.FindControl("ddlModalidadF"), DropDownList)
                ddlMotivoF = CType(row.FindControl("ddlMotivoF"), DropDownList)
                tbFechaLiquidacionF = CType(row.FindControl("tbFechaLiquidacionF"), TextBox)
                hdIntermediarioF = CType(row.FindControl("hdIntermediarioF"), HtmlInputHidden)
                tbPrecioFuturoF = CType(row.FindControl("tbPrecioFuturoF"), TextBox) 'OT10855 10/10/2017
                tbPrecioF = CType(row.FindControl("tbPrecioF"), TextBox)
                tbTotalOrdenF = CType(row.FindControl("tbTotalOrdenF"), TextBox)
                chkPorcentajeF = CType(Datagrid1.FooterRow.FindControl("chkPorcentajeF"), CheckBox)

                'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 16/05/2018
                Dim porcentajeF As String = "N"
                Dim sumaAsignacion As Decimal = 0
                If porcentajeF.Equals("N") Then
                    sumaAsignacion = Decimal.Parse(tbTotalOrdenF.Text.Trim)
                End If
                Dim dtDetalleInversiones As New DataTable
                dtDetalleInversiones = instanciarTabla(dtDetalleInversiones)
                Dim portafolioF As DropDownList
                portafolioF = CType(Datagrid1.FooterRow.FindControl("ddlPortafolioF"), DropDownList)
                Dim codigoPrevOrden As HiddenField
                codigoPrevOrden = CType(Datagrid1.FooterRow.FindControl("HdCodigoOrdenF"), HiddenField)
                If codigoPrevOrden.Value.Trim.Length <= 0 Then
                    codigoPrevOrden.Value = "0"
                End If
                dtDetalleInversiones.Rows.Add(CType(codigoPrevOrden.Value, Decimal), portafolioF.SelectedValue.Trim(), CType(tbTotalOrdenF.Text, Decimal), "N")
                Session("dtDetalleInversiones") = dtDetalleInversiones
                'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 16/05/2018


                If dtDetalleInversiones Is Nothing Then
                    AlertaJS("No ha ingresado el detalle de los fondos.")
                    Exit Sub
                End If
                Dim bolValidar As Boolean = False
                Dim mensajeValida As String = ""
                Select Case ddlClaseInstrumentofxF.SelectedValue
                    Case ParametrosSIT.CLASE_INSTRUMENTO_FORWARD
                        If ddlMonedaNegF.SelectedValue <> "" And ddlMonedaF.SelectedValue <> "" And _
                        (tbTotalOrdenF.Text <> "" And Decimal.Parse(tbTotalOrdenF.Text) <> 0) And (tbPrecioF.Text <> "" And Decimal.Parse(tbPrecioF.Text) <> 0) And _
                        ddlModalidadF.SelectedValue <> "" And
                        tbFechaLiquidacionF.Text <> "" And hdIntermediarioF.Value <> "" And ddlMotivoF.SelectedValue <> "" Then
                            If IsNumeric(tbTotalOrdenF.Text) And IsNumeric(tbPrecioF.Text) And IsDate(tbFechaLiquidacionF.Text) Then
                                If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) >= UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacionF.Text) Then
                                    bolValidar = False
                                    mensajeValida = "La fecha de vencimiento debe ser mayor a la fecha de operación."
                                Else
                                    If ddlMonedaF.SelectedValue <> ddlMonedaNegF.SelectedValue Then
                                        bolValidar = True
                                    Else
                                        mensajeValida = "Las monedas negociadas deben ser diferentes"
                                    End If
                                End If
                            Else
                                mensajeValida = "Ingrese correctamente el registro"
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
                            If tbTotalOrdenF.Text = "" Or Decimal.Parse(tbTotalOrdenF.Text) = 0 Then
                                mensajeValida = mensajeValida + "- Ingrese Monto Orden\n"
                            End If
                            If tbPrecioF.Text = "" Or Decimal.Parse(tbPrecioF.Text) = 0 Then
                                mensajeValida = mensajeValida + "- Ingrese Precio / T.C\n"
                            End If
                            If ddlModalidadF.SelectedValue = "" Then
                                mensajeValida = mensajeValida + "- Seleccione Modalidad Forward\n"
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

                            'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - COMENTADO | 22/05/2018
                            'oRow.FechaOperacion = ViewState("decFechaOperacion")
                            'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - COMENTADO | 22/05/2018

                            'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 22/05/2018
                            'Dim tFechaOperacionF As TextBox
                            'tFechaOperacionF = CType(Datagrid1.FooterRow.FindControl("tFechaOperacionF"), TextBox)
                            'ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Text)
                            'oRow.FechaOperacion = ViewState("decFechaOperacion")
                            Dim tFechaOperacionF As HiddenField
                            tFechaOperacionF = CType(Datagrid1.FooterRow.FindControl("hdFechaOperacionF"), HiddenField)
                            ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Value)
                            oRow.FechaOperacion = ViewState("decFechaOperacion")
                            'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 22/05/2018

                            oRow.HoraOperacion = Now.ToLongTimeString()
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
                            oRow.PrecioFuturo = CType(tbPrecioFuturoF.Text, Decimal) 'OT10855 10/10/2017
                            oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacionF.Text)
                            oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacionF.Text)
                            oRow.CodigoMotivo = ddlMotivoF.SelectedValue
                            oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                            oRow.Estado = PREV_OI_INGRESADO
                            oRow.Porcentaje = porcentajeF
                            'OT10855 10/10/2017 Se cambia el precio futuro para obtener el monto de operación
                            oRow.MontoOperacion = obtenerMontoOperacion(ddlMonedaNegF.SelectedValue, ddlMonedaF.SelectedValue, CType(tbPrecioFuturoF.Text, Decimal), _
                            CType(tbTotalOrdenF.Text, Decimal))
                            oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                            oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                            oPrevOrdenInversionBM.Insertar(oPrevOrdenInversionBE, "FX", DatosRequest, dtDetalleInversiones)
                            CargarGrilla("FX", ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                            ViewState("strOperador"), ViewState("strEstado"))
                            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(CType(ViewState("decFechaOperacion"), String))
                            Session.Remove("dtDetalleInversiones")
                        Else
                            AlertaJS(mensajeValida)
                        End If
                    Case ParametrosSIT.CLASE_INSTRUMENTO_CVME
                        If ddlMonedaNegF.SelectedValue <> "" And ddlMonedaF.SelectedValue <> "" And tbTotalOrdenF.Text <> "" And tbPrecioF.Text <> "" And _
                        tbFechaLiquidacionF.Text <> "" And hdIntermediarioF.Value <> "" Then
                            If IsNumeric(tbTotalOrdenF.Text) And IsNumeric(tbPrecioF.Text) And IsDate(tbFechaLiquidacionF.Text) Then
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
                            'INICIO | RCE | Zoluxiones | Se guarda con fecha de apertura de  portafolio | 13/08/2018
                            Dim ddlportafoliof As New DropDownList
                            ddlportafoliof = CType(row.FindControl("ddlPortafolioF"), DropDownList)

                            oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                            oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                            ViewState("decFechaOperacion") = UIUtility.ObtenerFechaApertura(ddlportafoliof.SelectedValue.ToString)
                            oRow.FechaOperacion = UIUtility.ObtenerFechaApertura(ddlportafoliof.SelectedValue.ToString)
                            'FIN | RCE | Zoluxiones | Se guarda con fecha de apertura de  portafolio | 13/08/2018
                            oRow.HoraOperacion = Now.ToLongTimeString()
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
                            oRow.MontoOperacion = obtenerMontoOperacion(ddlMonedaNegF.SelectedValue, ddlMonedaF.SelectedValue, CType(tbPrecioF.Text, Decimal), _
                            CType(tbTotalOrdenF.Text, Decimal))
                            oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                            oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                            oPrevOrdenInversionBM.Insertar(oPrevOrdenInversionBE, "FX", DatosRequest, dtDetalleInversiones)
                            CargarGrilla("FX", ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                            ViewState("strOperador"), ViewState("strEstado"))
                            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(CType(ViewState("decFechaOperacion"), String))
                            Session.Remove("dtDetalleInversiones")
                        Else
                            AlertaJS(mensajeValida)
                        End If
                End Select
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
                        showDialogoPopupExtorno("FX", decCodigoPrevOrden.ToString())
                    End If
                ElseIf gvr.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                    oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                ElseIf gvr.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then
                    oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                Else
                    oPrevOrdenInversionBM.Eliminar(decCodigoPrevOrden, DatosRequest)
                End If
                CargarGrilla("FX", ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strOperador"), ViewState("strEstado"))
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub Datagrid1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Datagrid1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                lbMonedaNeg = CType(e.Row.FindControl("lbMonedaNeg"), Label)
                lbMoneda = CType(e.Row.FindControl("lbMoneda"), Label)
                lbOperacion = CType(e.Row.FindControl("lbOperacion"), Label)
                lbModalidad = CType(e.Row.FindControl("lbModalidad"), Label)
                lbMedioNeg = CType(e.Row.FindControl("lbMedioNeg"), Label)
                lbMotivo = CType(e.Row.FindControl("lbMotivo"), Label)
                lbClaseInstrumentofx = CType(e.Row.FindControl("lbClaseInstrumentofx"), Label)
                ddlMonedaNeg = CType(e.Row.FindControl("ddlMonedaNeg"), DropDownList)
                ddlMoneda = CType(e.Row.FindControl("ddlMoneda"), DropDownList)
                ddlOperacion = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
                ddlModalidad = CType(e.Row.FindControl("ddlModalidad"), DropDownList)
                ddlMedioNeg = CType(e.Row.FindControl("ddlMedioNeg"), DropDownList)
                ddlMotivo = CType(e.Row.FindControl("ddlMotivo"), DropDownList)
                ddlFondos = CType(e.Row.FindControl("ddlfondos"), DropDownList)
                ddlClaseInstrumentofx = CType(e.Row.FindControl("ddlClaseInstrumentofx"), DropDownList)
                HelpCombo.LlenarComboBox(ddlMonedaNeg, CType(Session("dtMoneda"), DataTable), "CodigoMoneda", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlMoneda, CType(Session("dtMoneda"), DataTable), "CodigoMoneda", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlClaseInstrumentofx, CType(Session("dtClaseifx"), DataTable), "Valor", "Nombre", False)
                ddlClaseInstrumentofx.SelectedValue = lbClaseInstrumentofx.Text
                'OT 10090 - 26/07/2017 - Carlos Espejo
                'Descripcion: Se llena la categoria en base a la clase
                HelpCombo.LlenarComboBox(ddlOperacion, oOperacionBM.ListarOperacion_Categoria(ddlClaseInstrumentofx.SelectedValue), "codigoOperacion", "Descripcion", False)
                'OT 10090 Fin
                HelpCombo.LlenarComboBox(ddlModalidad, CType(Session("dtModforw"), DataTable), "Valor", "Nombre", True)
                HelpCombo.LlenarComboBox(ddlMedioNeg, CType(Session("dtMedioNeg"), DataTable), "Valor", "Nombre", False)
                HelpCombo.LlenarComboBox(ddlMotivo, CType(Session("dtMotivo"), DataTable), "CodigoMotivo", "Descripcion", True)
                Dim fondos As String() = CType(e.Row.FindControl("hdFondo1Trz"), HtmlInputHidden).Value.Split("/")
                For Each fondo As String In fondos
                    ddlFondos.Items.Add(fondo)
                Next
                ddlMonedaNeg.SelectedValue = lbMonedaNeg.Text
                ddlMoneda.SelectedValue = lbMoneda.Text
                ddlOperacion.SelectedValue = lbOperacion.Text
                ddlModalidad.SelectedValue = lbModalidad.Text
                If lbMedioNeg.Text <> String.Empty Then
                    ddlMedioNeg.SelectedValue = lbMedioNeg.Text
                End If
                ddlMotivo.SelectedValue = lbMotivo.Text
                hdClaseInstrumentofxTrz = CType(e.Row.FindControl("hdClaseInstrumentofxTrz"), HtmlInputHidden)
                hdClaseInstrumentofxTrz.Value = ddlClaseInstrumentofx.SelectedItem.Text
                hdOperacionTrz = CType(e.Row.FindControl("hdOperacionTrz"), HtmlInputHidden)
                hdOperacionTrz.Value = ddlOperacion.SelectedItem.Text
                If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or _
                e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Or e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then
                    tbHora = CType(e.Row.FindControl("tbHora"), TextBox)
                    tbTotalOrden = CType(e.Row.FindControl("tbTotalOrden"), TextBox)
                    tbPrecio = CType(e.Row.FindControl("tbPrecio"), TextBox)
                    tbPrecioFuturo = CType(e.Row.FindControl("tbPrecioFuturo"), TextBox) 'OT10855 10/10/2017
                    tbFechaLiquidacion = CType(e.Row.FindControl("tbFechaLiquidacion"), TextBox)
                    tbTotalOperacion = CType(e.Row.FindControl("tbTotalOperacion"), TextBox)
                    tbIntermediario = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                    chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)
                    tbHora.Enabled = False
                    tbTotalOrden.Enabled = False
                    tbPrecio.Enabled = False
                    tbPrecioFuturo.Enabled = False
                    tbFechaLiquidacion.Enabled = False
                    tbTotalOperacion.Enabled = False
                    tbIntermediario.Enabled = False
                    ddlMonedaNeg.Enabled = False
                    ddlMoneda.Enabled = False
                    ddlOperacion.Enabled = False
                    ddlModalidad.Enabled = False
                    ddlMedioNeg.Enabled = False
                    ddlMotivo.Enabled = False
                    ddlClaseInstrumentofx.Enabled = False
                    chkSelect.Enabled = False
                    If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or _
                    e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Then
                        chkSelect.Enabled = True
                    End If
                    Dim FechaLiqui As HtmlGenericControl
                    FechaLiqui = CType(e.Row.FindControl("FechaLiqui"), HtmlGenericControl)
                    FechaLiqui.Attributes.Add("class", "input-append")
                End If
                tbHora2 = CType(e.Row.FindControl("tbHora"), TextBox)
                ddlClaseInstrumentofx2 = CType(e.Row.FindControl("ddlClaseInstrumentofx"), DropDownList)
                ddlMonedaNeg2 = CType(e.Row.FindControl("ddlMonedaNeg"), DropDownList)
                ddlMoneda2 = CType(e.Row.FindControl("ddlMoneda"), DropDownList)
                ddlOperacion2 = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
                tbTotalOrden2 = CType(e.Row.FindControl("tbTotalOrden"), TextBox)
                tbPrecio2 = CType(e.Row.FindControl("tbPrecio"), TextBox)
                tbPrecioFuturo2 = CType(e.Row.FindControl("tbPrecioFuturo"), TextBox)
                ddlModalidad2 = CType(e.Row.FindControl("ddlModalidad"), DropDownList)
                tbFechaLiquidacion2 = CType(e.Row.FindControl("tbFechaLiquidacion"), TextBox)
                tbIntermediario2 = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                ddlMedioNeg2 = CType(e.Row.FindControl("ddlMedioNeg"), DropDownList)
                ddlMotivo2 = CType(e.Row.FindControl("ddlMotivo"), DropDownList)
                hdPorcentaje2 = CType(e.Row.FindControl("hdPorcentaje"), HtmlInputHidden)
                chkPorcentaje2 = CType(e.Row.FindControl("chkPorcentaje"), CheckBox)
                tbHora2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlClaseInstrumentofx2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlMonedaNeg2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlMoneda2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlOperacion2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbTotalOrden2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbPrecio2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbPrecioFuturo2.Attributes.Add("onchange", "javascript:cambio(this);") 'OT10855 10/10/2017
                ddlModalidad2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbFechaLiquidacion2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbIntermediario2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlMedioNeg2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlMotivo2.Attributes.Add("onchange", "javascript:cambio(this);")
                chkPorcentaje2.Attributes.Add("onchange", "javascript:cambio(this);")
                If hdPorcentaje2.Value = "S" Then
                    chkPorcentaje2.Checked = True
                Else
                    chkPorcentaje2.Checked = False
                End If
                'INICIO | RCE | Zoluxiones | Se habilita row de acuerdo a estado de la Preorden de inversión  "Eliminado" de acuerdo a fecha de búsqueda | 13/08/2018
                If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then
                    e.Row.Enabled = False
                End If
                'FIN | RCE | Zoluxiones | Se habilita row de acuerdo a estado de la Preorden de inversión "Eliminado" de acuerdo a fecha de búsqueda | 13/08/2018
            End If
            If e.Row.RowType = DataControlRowType.Footer Then
                ddlMonedaNegF = CType(e.Row.FindControl("ddlMonedaNegF"), DropDownList)
                ddlMonedaF = CType(e.Row.FindControl("ddlMonedaF"), DropDownList)
                ddlOperacionF = CType(e.Row.FindControl("ddlOperacionF"), DropDownList)
                ddlModalidadF = CType(e.Row.FindControl("ddlModalidadF"), DropDownList)
                ddlMedioNegF = CType(e.Row.FindControl("ddlMedioNegF"), DropDownList)
                ddlMotivoF = CType(e.Row.FindControl("ddlMotivoF"), DropDownList)
                ddlClaseInstrumentofxF = CType(e.Row.FindControl("ddlClaseInstrumentofxF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlMonedaNegF, CType(Session("dtMoneda"), DataTable), "CodigoMoneda", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlMonedaF, CType(Session("dtMoneda"), DataTable), "CodigoMoneda", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlOperacionF, CType(Session("dtOperacion"), DataTable), "codigoOperacion", "Descripcion", False)
                HelpCombo.LlenarComboBox(ddlClaseInstrumentofxF, CType(Session("dtClaseifx"), DataTable), "Valor", "Nombre", False)
                'OT 10090 - 26/07/2017 - Carlos Espejo
                'Descripcion: Se llena la categoria en base a la clase
                HelpCombo.LlenarComboBox(ddlOperacionF, oOperacionBM.ListarOperacion_Categoria(ddlClaseInstrumentofxF.SelectedValue), "codigoOperacion", "Descripcion", False)
                'OT 10090 Fin
                HelpCombo.LlenarComboBox(ddlModalidadF, CType(Session("dtModforw"), DataTable), "Valor", "Nombre", True)
                HelpCombo.LlenarComboBox(ddlMedioNegF, CType(Session("dtMedioNeg"), DataTable), "Valor", "Nombre", False)
                HelpCombo.LlenarComboBox(ddlMotivoF, CType(Session("dtMotivo"), DataTable), "CodigoMotivo", "Descripcion", True)

                'INICIO | ZOLUXIONES | DACV | SPRINT III '
                ddlModalidadF.SelectedValue = "S"
                ddlMotivoF.SelectedValue = "11"
                'FIN | ZOLUXIONES | DACV | SPRINT III '

                'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - DESCRIPCION | 16/05/2018
                Dim ddlportafoliof As New DropDownList
                ddlportafoliof = CType(e.Row.FindControl("ddlPortafolioF"), DropDownList)
                'HelpCombo.LlenarComboBox(ddlportafoliof, CType(Session("portafolio"), DataTable), "CodigoPortafolio", "Descripcion", False)
                HelpCombo.LlenarComboBox(ddlportafoliof, objPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO), "CodigoPortafolio", "Descripcion", False)
                ddlportafoliof.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, "0"))
                ddlportafoliof.SelectedValue = "0"
                'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - DESCRIPCION | 16/05/2018
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    'Sub CargarPortafolio()
    '    Dim dtPortafolio As New DataTable
    '    dtPortafolio = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO)
    '    Session("portafolio") = dtPortafolio
    'End Sub

    Protected Sub Modulos_Inversiones_frmIngresoMasivoOperacionFX_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Cro.Close()
        Cro.Dispose()
        oReport.Close()
        oReport.Dispose()
    End Sub
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: En base a la clase se activan o desactivan controles
    Protected Sub ddlClaseInstrumentofx_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        Try
            For Each row As GridViewRow In Datagrid1.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    ddlClaseInstrumentofx = CType(row.Cells(5).FindControl("ddlClaseInstrumentofx"), DropDownList)
                    If ddlClaseInstrumentofx.ClientID = CType(sender, DropDownList).ClientID Then
                        ddlOperacion = TryCast(row.Cells(6).FindControl("ddlOperacion"), DropDownList)
                        ddlModalidad = CType(row.Cells(10).FindControl("ddlModalidad"), DropDownList)
                        ddlClaseInstrumentofx = CType(row.Cells(5).FindControl("ddlClaseInstrumentofx"), DropDownList)
                        ddlMotivo = CType(row.Cells(14).FindControl("ddlMotivo"), DropDownList)
                        HelpCombo.LlenarComboBox(ddlOperacion, oOperacionBM.ListarOperacion_Categoria(ddlClaseInstrumentofx.SelectedValue), "codigoOperacion",
                        "Descripcion", False)
                        If ddlClaseInstrumentofx.SelectedValue = "FD" Then
                            ddlModalidad.Enabled = True
                            ddlMotivo.Enabled = True
                        Else
                            ddlModalidad.Enabled = False
                            ddlMotivo.Enabled = False
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: En base a la clase se activan o desactivan controles
    Protected Sub ddlClaseInstrumentofxF_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        Try
            ddlOperacionF = CType(Datagrid1.FooterRow.FindControl("ddlOperacionF"), DropDownList)
            ddlModalidadF = CType(Datagrid1.FooterRow.FindControl("ddlModalidadF"), DropDownList)
            ddlClaseInstrumentofxF = CType(Datagrid1.FooterRow.FindControl("ddlClaseInstrumentofxF"), DropDownList)
            ddlMotivoF = CType(Datagrid1.FooterRow.FindControl("ddlMotivoF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlOperacionF, oOperacionBM.ListarOperacion_Categoria(ddlClaseInstrumentofxF.SelectedValue), "codigoOperacion", "Descripcion", False)
            If ddlClaseInstrumentofxF.SelectedValue = "FD" Then
                ddlModalidadF.Enabled = True
                ddlMotivoF.Enabled = True
            Else
                ddlModalidadF.Enabled = False
                ddlMotivoF.Enabled = False
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub InicializarBotones()
        btnBuscar.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnGrabar.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnValidarTrader.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnAprobar.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
    End Sub
End Class
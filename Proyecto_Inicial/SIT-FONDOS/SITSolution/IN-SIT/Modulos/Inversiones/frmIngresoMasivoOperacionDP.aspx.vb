'OT 10090 - 26/07/2017 - Carlos Espejo
'Descripcion: Nuevo formulario para masivo de DPZ Y OR
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
    'Controles
    Dim ddlMedioNeg As DropDownList, ddlTipoTasa As DropDownList, ddlClaseInstrumentofx As DropDownList, ddlTipoTitulo As DropDownList,
    hdIntermediario As HtmlInputHidden, tbTasa As TextBox, tbTotalOrden As TextBox, tbFechaContrato As TextBox, tbHora As TextBox, lbCodigoPrevOrden As Label,
    _filaTraz As TrazabilidadOperacionBE.TrazabilidadOperacionRow, oTrazabilidadOperacionBE As New TrazabilidadOperacionBE, hdCambio As HtmlInputHidden,
    hdCambioTraza As HtmlInputHidden, hdCambioTrazaFondo As HtmlInputHidden, hdTipoTituloTrz As HtmlInputHidden, hdTotalOrdenTrz As HtmlInputHidden,
    hdPrecioTrz As HtmlInputHidden, hdTotalOperacionTrz As HtmlInputHidden, hdPrecioFuturoTrz As HtmlInputHidden, chkPorcentaje As CheckBox, tbIntermediario As TextBox,
    ddlMedioNegF As DropDownList, ddlTipoTasaF As DropDownList, ddlClaseInstrumentofxF As DropDownList, hdIntermediarioF As HtmlInputHidden,
    tbTasaF As TextBox, tbTotalOrdenF As TextBox, chkPorcentajeF As CheckBox, porcentajeF As String, dtDetalleInversiones As DataTable, tbFechaContratoF As TextBox,
    tbTotalOperacion As TextBox, chkSelect As CheckBox, Imagebutton1 As ImageButton, lbClase As Label, ddlFondos As DropDownList, lbMonedaNeg As Label, lbMoneda As Label,
    lbTipoTitulo As Label, lbModalidad As Label, lbTipoTasa As Label, lbMedioNeg As Label, lbMotivo As Label, lbClaseInstrumentofx As Label, tbHora2 As TextBox,
    ddlClaseInstrumentofx2 As DropDownList, ddlOperacion2 As DropDownList, ddlTipoTitulo2 As DropDownList, tbTotalOrden2 As TextBox, ddlTipoTasa2 As DropDownList,
    tbTasa2 As TextBox, tbFechaContrato2 As TextBox, tbIntermediario2 As TextBox, ddlMedioNeg2 As DropDownList, chkPorcentaje2 As CheckBox,
    hdPorcentaje2 As HtmlInputHidden, lbOperacion As Label, ddlOperacion As DropDownList, ddlOperacionF As DropDownList,
    ddlTipoTituloF As DropDownList, tbNemonico As TextBox, tbCantidad As TextBox, tbNemonicoF As TextBox, tbCantidadF As TextBox, ddlPlazaN As DropDownList,
    ddlPlazaNF As DropDownList, lbPlazaN As Label, hdFondo As HtmlInputHidden, HdFechaOperacion As HiddenField, HdCodigoPortafolioSBS As HiddenField
    '
    Dim pRutas As String
    Dim ValidarDia As String = String.Empty
    Dim BMPrevOrden As New PrevOrdenInversionBE
    Dim rutas As New System.Text.StringBuilder
    Dim Cro As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Dim oOperacionBM As New OperacionBM, oTipoTituloBM As New TipoTituloBM, oPlazaBM As New PlazaBM, objPortafolio As New PortafolioBM
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
            If Not Page.IsPostBack Then
                CargarLoading("btnBuscar")
                'InicializarBotones()
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
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
        Dim dtMedioNeg As New DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtMedioNeg = oParametrosGeneralesBM.ListarMedioNegociacionPrevOI(ParametrosSIT.TR_RENTA_FIJA).Tables(0)
        Session("dtMedioNeg") = dtMedioNeg
        Dim dtTipoTasa As New DataTable
        dtTipoTasa = oParametrosGeneralesBM.Listar("TipoTasaI", DatosRequest)
        Session("dtTipoTasa") = dtTipoTasa
        Dim dtClaseifx As New DataTable
        dtClaseifx = oParametrosGeneralesBM.Listar("CLASE_TU", DatosRequest)
        Session("dtClaseifx") = dtClaseifx
        CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"))
        Dim dtPlazaN As New DataTable
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
    Private Sub CargarGrilla(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, Optional ByVal strCodigoClaseInstrumento As String = "",
    Optional ByVal strCodigoTipoInstrumentoSBS As String = "", Optional ByVal strCodigoNemonico As String = "", Optional ByVal strOperador As String = "",
    Optional ByVal strEstado As String = "")
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim ds As DataSet
        ds = oPrevOrdenInversionBM.SeleccionarPorFiltro(strTipoRenta, decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, _
        strCodigoNemonico, strOperador, strEstado, DatosRequest)
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
    'OT11112 - 07/02/2018 - Ian Pastor M.
    'Descripción: Corregir el cálculo de los depósitos a plazo teniendo en cuenta su BaseTIR
    Private Function obtenerMontoOperacionDPZ(ByVal TipoTasa As String, ByVal montoNominal As Decimal, ByVal tasa As Decimal, ByVal fechaOperacion As Decimal,
    ByVal fechaLiquidacion As Decimal, ByVal codigoTipoTitulo As String) As Decimal
        Dim montoOperacion As Decimal = 0.0
        Dim plazo As Integer
        Dim objTipoTituloBM As New TipoTituloBM
        Dim baseTir As Decimal = 0.0

        Using objTipoTituloBE As TipoTituloBE = objTipoTituloBM.Seleccionar(codigoTipoTitulo, DatosRequest)
            baseTir = Decimal.Parse(objTipoTituloBE.Tables(0).Rows(0)("BaseTir"))
        End Using

        If baseTir = 0 Then Throw New System.Exception("La base TIR del título: " & codigoTipoTitulo & " No se encuentra difinido. " _
            & "Por favor definirlo en la opción Tipo de Título")

        plazo = DateDiff(DateInterval.Day, CType(UIUtility.ConvertirFechaaString(fechaOperacion), Date), CType(UIUtility.ConvertirFechaaString(fechaLiquidacion), Date))

        'Cálculo depósitos a plazo en un año Bisiesto
        If codigoTipoTitulo = "DPZNSOL365B" Or codigoTipoTitulo = "DPZDOL365B" Then
            Dim objOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
            montoOperacion = objOrdenInversionWorkFlowBM.CalculoDPZBisiesto(fechaOperacion, fechaLiquidacion, montoNominal, tasa, TipoTasa)
        Else
            If (TipoTasa = "1") Then
                montoOperacion = ((((montoNominal * tasa) / 100) / baseTir) * plazo) + montoNominal
            Else
                montoOperacion = montoNominal * Math.Pow((1 + (tasa / 100)), (plazo / baseTir))
            End If
        End If

        Return Math.Round(montoOperacion, 2)
    End Function
    'OT11112 - Fin
    Private Function ValidarOperacionPorClaseInstrumento(ByVal strClaseInstrumento As String, ByVal strCodigoOperacion As String) As Boolean
        Dim bolResult As Boolean = False
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dtAux As New DataTable
        Dim dtOperacion As New DataTable
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
    Private Function ValidarAsignaciones(ByVal decAsignacionF1 As Decimal, ByVal decAsignacionF2 As Decimal, ByVal decAsignacionF3 As Decimal,
    ByVal decCantidadOperacion As Decimal) As Boolean
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
        'btnAprobar.Enabled = habilita
        '        Datagrid1.Enabled = habilita
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
            CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), decFechaOperacion, strCodigoClaseInstrumento, "", "", strOperador, strEstado)
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
            Dim porcentaje As String, strCambioTrazaFondo As String = ""
            Dim dt As New DataTable
            For Each fila As GridViewRow In Datagrid1.Rows
                lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                hdCambio = CType(fila.FindControl("hdCambio"), HtmlInputHidden)
                hdCambioTraza = CType(fila.FindControl("hdCambioTraza"), HtmlInputHidden)
                hdCambioTrazaFondo = CType(fila.FindControl("hdCambioTrazaFondo"), HtmlInputHidden)
                chkPorcentaje = CType(fila.FindControl("chkPorcentaje"), CheckBox)
                porcentaje = "N"

                If fila.Cells(2).Text = PREV_OI_INGRESADO Then
                    'OT10709. Se quito el flag hdCambio.Value. Cualquier registro con campo editable puede modificarse.
                    If Not lbCodigoPrevOrden Is Nothing Then
                        ddlOperacion = CType(fila.FindControl("ddlOperacion"), DropDownList)
                        ddlMedioNeg = CType(fila.FindControl("ddlMedioNeg"), DropDownList)
                        ddlTipoTasa = CType(fila.FindControl("ddlTipoTasa"), DropDownList)
                        ddlClaseInstrumentofx = CType(fila.FindControl("ddlClaseInstrumentofx"), DropDownList)
                        ddlTipoTitulo = CType(fila.FindControl("ddlTipoTitulo"), DropDownList)
                        hdIntermediario = CType(fila.FindControl("hdIntermediario"), HtmlInputHidden)
                        tbTasa = CType(fila.FindControl("tbTasa"), TextBox)
                        tbTotalOrden = CType(fila.FindControl("tbTotalOrden"), TextBox)
                        tbFechaContrato = CType(fila.FindControl("tbFechaContrato"), TextBox)
                        tbHora = CType(fila.FindControl("tbHora"), TextBox)
                        tbNemonico = CType(fila.FindControl("tbNemonico"), TextBox)
                        tbCantidad = CType(fila.FindControl("tbCantidad"), TextBox)
                        ddlPlazaN = CType(fila.FindControl("ddlPlazaN"), DropDownList)
                        Dim bolValidar As Boolean = False
                        If ddlTipoTitulo.SelectedValue <> "" And tbTotalOrden.Text <> "" And tbTasa.Text <> "" And ddlTipoTasa.SelectedValue <> "" And
                            tbFechaContrato.Text <> "" And hdIntermediario.Value <> "" And tbNemonico.Text <> "" Then
                            If IsNumeric(tbTotalOrden.Text) And IsNumeric(tbTasa.Text) And IsDate(tbFechaContrato.Text) Then
                                If validaFechas(ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text)) Then
                                    If fila.Cells(2).Text <> ParametrosSIT.PREV_OI_EJECUTADO And fila.Cells(2).Text <> ParametrosSIT.PREV_OI_APROBADO And _
                                    fila.Cells(2).Text <> ParametrosSIT.PREV_OI_PENDIENTE Then
                                        bolValidar = True
                                    End If
                                End If
                            End If
                        End If

                        If bolValidar Then
                            If ddlClaseInstrumentofx.SelectedValue = "DP" Then
                                ValidarDia = ValidarFechaVencimiento(tbFechaContrato.Text, hdIntermediario.Value)
                                If ValidarDia <> String.Empty Then
                                    AlertaJS("- La fecha fin de contrato (Vencimiento) no puede ser: " & ValidarDia & ".")
                                    Exit Sub
                                End If
                            End If

                            oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                            oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                            oRow.CodigoPrevOrden = lbCodigoPrevOrden.Text
                            oRow.HoraOperacion = Now.ToLongTimeString()
                            oRow.ClaseInstrumentoFx = ddlClaseInstrumentofx.SelectedValue
                            oRow.CodigoTipoTitulo = ddlTipoTitulo.SelectedValue
                            oRow.MedioNegociacion = ddlMedioNeg.SelectedValue
                            oRow.MontoNominal = CType(tbTotalOrden.Text, Decimal)
                            oRow.Tasa = CType(tbTasa.Text, Decimal)
                            oRow.TipoTasa = ddlTipoTasa.SelectedValue
                            oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text)
                            oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
                            oRow.MontoOperacion = obtenerMontoOperacionDPZ(ddlTipoTasa.SelectedValue, CType(tbTotalOrden.Text, Decimal), _
                            CType(tbTasa.Text, Decimal), ViewState("decFechaOperacion"), UIUtility.ConvertirFechaaDecimal(tbFechaContrato.Text), ddlTipoTitulo.SelectedValue)
                            oRow.CodigoTercero = hdIntermediario.Value
                            oRow.CodigoOperacion = ddlOperacion.SelectedValue
                            oRow.Porcentaje = porcentaje
                            oRow.CodigoNemonico = IIf(ddlClaseInstrumentofx.SelectedValue = "DP", ddlTipoTitulo.SelectedValue, tbNemonico.Text)
                            oRow.Cantidad = CDec(tbCantidad.Text)
                            oRow.CantidadOperacion = CDec(tbCantidad.Text)
                            oRow.CodigoPlaza = ddlPlazaN.SelectedValue
                            oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                            oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                            If dtDetalleInversiones Is Nothing Then dtDetalleInversiones = instanciarTabla(dtDetalleInversiones)
                            Dim dtDetallePortafolio As DataTable = oPrevOrdenInversionBM.SeleccionarDetallePreOrdenInversion(CType(lbCodigoPrevOrden.Text.Trim, Decimal))
                            dtDetalleInversiones.Rows.Add(CType(lbCodigoPrevOrden.Text.Trim, Decimal), dtDetallePortafolio.Rows(0)("CodigoPortafolio"), CType(tbTotalOrden.Text, Decimal), "N")

                        Else
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
                CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), "", "", ViewState("strOperador"), _
                ViewState("strEstado"))
                AlertaJS("Grabación exitosa")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            btnGrabar.Text = "Grabar"
            btnGrabar.Enabled = True
        End Try
    End Sub
    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            GenerarReporteOperacionesFx()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
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
            oDs = oPrevOrdenInversionBM.GenerarReporte(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), DatosRequest, decNProceso)
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
    'OT-10784 Inicio
    Private Sub btnAprobar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim strCodPrevOrden As String = ""
        Dim decNProceso As Decimal = 0
        Dim arrCodPrevOrden As Array
        Dim ds As New DataSet
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
                    EjecutarOrdenInversion(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), strCodPrevOrden, , , decNProceso)
                Else
                    AlertaJS("Seleccione el registro a ejecutar ó el registro ya se encontraba ejecutado!")
                    CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), "", "", _
                    ViewState("strOperador"), ViewState("strEstado"))
                    ''Se eliminan todos los procesos
                    'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
                    ''Se setea el flag a "0" de las órdenes afectadas
                    'arrCodPrevOrden = strCodPrevOrden.Split("|")
                    'For i = 0 To arrCodPrevOrden.Length - 1
                    '    If arrCodPrevOrden(i) <> "" Then
                    '        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                    '    End If
                    'Next
                End If
            Else
                AlertaJS("Ningún registro cumple el requerimiento de estado.")
                CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), "", "", _
                    ViewState("strOperador"), ViewState("strEstado"))
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            arrCodPrevOrden = strCodPrevOrden.Split("|")
            For i = 0 To arrCodPrevOrden.Length - 1
                If arrCodPrevOrden(i) <> "" Then
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                End If
            Next
            btnAprobar.Text = "Ejecutar"
            btnAprobar.Enabled = True
        End Try
    End Sub
    'OT-10784 Fin
    'OT-10784 Inicio
    Private Sub btnValidar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnValidar.Click
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
        Dim lbCodigoPrevOrden As Label
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
                Limites.VerificaExcesoLimites(Me.Usuario, ParametrosSIT.TR_DERIVADOS.ToString(), decNProceso)
                Dim dt As New DataTable
                dt = oPrevOrdenInversionBM.SeleccionarValidacionExcesos(ParametrosSIT.TR_DERIVADOS.ToString(), _
                ViewState("decFechaOperacion"), DatosRequest, decNProceso).Tables(0)
                If dt.Rows.Count > 0 Then
                    Session("dtListaExcesos") = dt
                    EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" + ParametrosSIT.TR_RENTA_VARIABLE.ToString() + _
                    "', '800', '550','" & btnBuscar.ClientID & "');")
                End If
            Else
                AlertaJS("Seleccione los registros a validar!")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            arrCodPrevOrden = strCodPrevOrden.Split("|")
            For i = 0 To arrCodPrevOrden.Length - 1
                If arrCodPrevOrden(i) <> "" Then
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                End If
            Next
        End Try
    End Sub
    'OT-10784 Fin
    'OT-10784 Inicio
    Private Sub btnValidarTrader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidarTrader.Click
        Dim dtValidaTrader As New DataTable
        Dim oLimiteTradingBM As New LimiteTradingBM
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim decNProceso As Decimal = 0
        Dim ds As New DataSet
        Dim strEstadoPrevOrden As String = ""
        Dim strCodPrevOrden As String = ""
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
                    If fila.Cells(2).Text = ParametrosSIT.PREV_OI_INGRESADO Then
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
            ElseIf count > 0 Then
                dtValidaTrader = oLimiteTradingBM.SeleccionarValidacionExcesosTrader_Sura(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), _
                Usuario, DatosRequest, , decNProceso).Tables(0)
                If dtValidaTrader.Rows.Count > 0 Then
                    Session("dtValidaTrader") = dtValidaTrader
                    EjecutarJS("showModalDialog('frmValidacionExcesosTrader.aspx?TipoRenta=" & ParametrosSIT.TR_DERIVADOS.ToString() & _
                    "&nProc=" & decNProceso.ToString() + "', '800', '550','" & btnBuscar.ClientID & "');")
                Else
                    AlertaJS("No se han podido evaluar los limites trader.")
                    'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
                End If
            Else
                AlertaJS("Seleccione el registro a validar ó el registro ya se encontraba aprobado!")
                CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), "", "", _
                    ViewState("strOperador"), ViewState("strEstado"))
                'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            End If
            'arrCodPrevOrden = strCodPrevOrden.Split("|")
            'For i = 0 To arrCodPrevOrden.Length - 1
            '    If arrCodPrevOrden(i) <> "" Then
            '        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
            '    End If
            'Next
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            arrCodPrevOrden = strCodPrevOrden.Split("|")
            For i = 0 To arrCodPrevOrden.Length - 1
                If arrCodPrevOrden(i) <> "" Then
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                End If
            Next
            btnValidarTrader.Text = "Validar Exc. Trader"
            btnValidarTrader.Enabled = True
        End Try
    End Sub
    'OT-10784 Fin
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
                    ddlTipoTitulo = CType(fila.FindControl("ddlTipoTitulo"), DropDownList)
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
                        If strCatClase = CLASE_INSTRUMENTO_DEPOSITOPLAZO Then
                            strClase = CLASE_LLAMADO_DEPOSITOPLAZO
                            strCodigoMnemonico = ddlTipoTitulo.SelectedValue
                            strCodigoSBS = IIf(fila2("CodigoSBS").ToString.Length < 12, String.Empty, fila2("CodigoSBS").ToString)
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
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
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
            droper("c22") = ""
            droper("v22") = ""
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
        Dim sRutaTemp As String = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
        Dim foldersAsesoria() As String = Directory.GetDirectories(sRutaTemp, PrefijoFolder & "*")
        Dim folderActual As String = sRutaTemp & PrefijoFolder & fechaActual
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
            rutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "FX_" & Usuario.ToString() &
            String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
            oReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
        End If
        ViewState("RutaReporte") = rutaArchivo
    End Sub
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            Try
                dtDestino.LoadDataRow(dr.ItemArray, False)
            Catch ex As Exception
                AlertaJS(Replace(ex.Message.ToString(), "'", ""))
            End Try
        Next
    End Sub
    Public Sub EjecutarOrdenInversion(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, Optional ByVal strCodPrevOrden As String = "",
    Optional ByRef bolUpdGrilla As Boolean = False, Optional ByVal claseInstrumento As String = "", Optional ByVal decNProceso As Decimal = 0)
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
                ''OT-10784 Inicio
                If bolGeneraOrden Then
                    If dtOrdenInversion.Rows.Count > 0 Then
                        Session("dtListaExcesos") = dtOrdenInversion
                        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + _
                        "', '1000', '500','" & btnBuscar.ClientID & "');")
                    End If
                End If
                'If bolGeneraOrden Then
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
                'End If
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
        ''OT-10784 Fin
    End Sub
    'Private Sub showDialogoPopupExtorno(ByVal tipoRenta As String, ByVal codigoPrevOrden As String)
    Private Sub showDialogoPopupExtorno(ByVal tipoRenta As String, ByVal codigoPrevOrden As String, ByVal codigoClaseInstrumento As String)
        'codigoClaseInstrumento
        'EjecutarJS("showModalDialog('frmExtornoIngresoMasivoOperacion.aspx?tipoRenta=" & tipoRenta & "&codigo=" & codigoPrevOrden + _
        '"', '900', '500','" & btnBuscar.ClientID & "');")
        EjecutarJS("showModalDialog('frmExtornoIngresoMasivoOperacion.aspx?tipoRenta=" & tipoRenta & "&codigo=" & codigoPrevOrden + _
                "&codigoClaseInstrumento=" & codigoClaseInstrumento + _
        "', '900', '500','" & btnBuscar.ClientID & "');")

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
                'Case "Footer", "Item", "Add"
                Case "Footer", "Item", "Add", "_Delete"
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
                ddlOperacionF = CType(row.FindControl("ddlOperacionF"), DropDownList)
                ddlMedioNegF = CType(row.FindControl("ddlMedioNegF"), DropDownList)
                ddlTipoTasaF = CType(row.FindControl("ddlTipoTasaF"), DropDownList)
                ddlClaseInstrumentofxF = CType(row.FindControl("ddlClaseInstrumentofxF"), DropDownList)
                ddlTipoTituloF = CType(row.FindControl("ddlTipoTituloF"), DropDownList)
                hdIntermediarioF = CType(row.FindControl("hdIntermediarioF"), HtmlInputHidden)
                tbTasaF = CType(row.FindControl("tbTasaF"), TextBox)
                tbTotalOrdenF = CType(row.FindControl("tbTotalOrdenF"), TextBox)
                tbFechaContratoF = CType(row.FindControl("tbFechaContratoF"), TextBox)
                chkPorcentajeF = CType(Datagrid1.FooterRow.FindControl("chkPorcentajeF"), CheckBox)
                tbNemonicoF = CType(row.FindControl("tbNemonicoF"), TextBox)
                tbCantidadF = CType(row.FindControl("tbCantidadF"), TextBox)
                ddlPlazaNF = CType(row.FindControl("ddlPlazaNF"), DropDownList)

                porcentajeF = "N"
                Dim sumaAsignacion As Decimal = 0
                If porcentajeF.Equals("N") Then
                    sumaAsignacion = Decimal.Parse(tbTotalOrdenF.Text.Trim)
                End If
                Dim portafolioF As DropDownList
                portafolioF = CType(Datagrid1.FooterRow.FindControl("ddlPortafolioF"), DropDownList)
                Dim codigoPrevOrden As HiddenField
                codigoPrevOrden = CType(Datagrid1.FooterRow.FindControl("HdCodigoOrdenF"), HiddenField)
                If codigoPrevOrden.Value.Trim.Length <= 0 Then
                    codigoPrevOrden.Value = "0"
                End If
                dtDetalleInversiones = instanciarTabla(dtDetalleInversiones)
                dtDetalleInversiones.Rows.Add(CType(codigoPrevOrden.Value, Decimal), portafolioF.SelectedValue.Trim(), CType(tbTotalOrdenF.Text, Decimal), "N")
                Session("dtDetalleInversiones") = dtDetalleInversiones

                'If chkPorcentajeF.Checked Then
                '    porcentajeF = "S"
                'Else
                '    porcentajeF = "N"
                'End If

                'If chkPorcentajeF.Checked Then
                '    porcentajeF = "S"
                '    If Not Session("dtDetalleInversiones") Is Nothing Then
                '        dtDetalleInversiones = instanciarTabla(dtDetalleInversiones)
                '        dtDetalleInversiones = Session("dtDetalleInversiones")
                '        Dim sumaAsignacion As Decimal = 0
                '        Dim i As Integer
                '        For i = 0 To dtDetalleInversiones.Rows.Count - 1
                '            sumaAsignacion = sumaAsignacion + Decimal.Parse(dtDetalleInversiones.Rows(i)("Asignacion").ToString.Trim)
                '        Next
                '        If sumaAsignacion <> 100 Then
                '            AlertaJS("Ocurrió un error: Asignación incorrecta de fondos")
                '            Exit Sub
                '        End If
                '    End If
                'Else
                '    porcentajeF = "N"
                '    If Not Session("dtDetalleInversiones") Is Nothing Then
                '        dtDetalleInversiones = instanciarTabla(dtDetalleInversiones)
                '        dtDetalleInversiones = Session("dtDetalleInversiones")
                '        Dim sumaAsignacion As Decimal = 0
                '        Dim i As Integer
                '        For i = 0 To dtDetalleInversiones.Rows.Count - 1
                '            sumaAsignacion = sumaAsignacion + Decimal.Parse(dtDetalleInversiones.Rows(i)("Asignacion").ToString.Trim)
                '        Next
                '        If CDec(sumaAsignacion) <> CDec(tbTotalOrdenF.Text) Then
                '            AlertaJS("Ocurrió un error: Asignación incorrecta de fondos")
                '            Exit Sub
                '        End If
                '    End If
                'End If


                If dtDetalleInversiones Is Nothing Then
                    AlertaJS("No ha ingresado el detalle de los fondos.")
                    Exit Sub
                End If
                Dim bolValidar As Boolean = False
                Dim mensajeValida As String = ""
                If IsNumeric(tbTotalOrdenF.Text) And IsNumeric(tbTasaF.Text) And IsDate(tbFechaContratoF.Text) Then
                    If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) >= UIUtility.ConvertirFechaaDecimal(tbFechaContratoF.Text) Then
                        mensajeValida = "- La fecha de fin de contrato debe ser mayor a la fecha de operación.<br>"
                    End If
                Else
                    mensajeValida = "- Ingrese correctamente las fechas<br> "
                End If

                If ddlOperacionF.SelectedValue = "" Then
                    mensajeValida = mensajeValida + "- Seleccione Operación<br>"
                End If
                If ddlTipoTituloF.SelectedValue = "" Then
                    mensajeValida = mensajeValida + "- Seleccione Tipo Titulo<br>"
                End If
                If tbTotalOrdenF.Text = "" Then
                    mensajeValida = mensajeValida + "- Ingrese Monto Orden<br>"
                End If
                If ddlTipoTasaF.SelectedValue = "" Then
                    mensajeValida = mensajeValida + "- Seleccione Tipo Tasa<br>"
                End If
                If tbTasaF.Text = "" Then
                    mensajeValida = mensajeValida + "- Ingrese YTM% <br>"
                End If
                If tbFechaContratoF.Text = "" Then
                    mensajeValida = mensajeValida + "- Ingrese Fecha Fin Contrato<br>"
                End If
                If hdIntermediarioF.Value = "" Then
                    mensajeValida = mensajeValida + "- Ingrese Intermediario<br>"
                End If
                If ddlClaseInstrumentofxF.SelectedValue = "OR" And tbNemonicoF.Text = "" Then
                    mensajeValida = mensajeValida + "- Ingrese el nemonico asociado<br>"
                End If

                If ddlClaseInstrumentofxF.SelectedValue = "DP" Then
                    ValidarDia = ValidarFechaVencimiento(tbFechaContratoF.Text, hdIntermediarioF.Value)
                    If ValidarDia <> String.Empty Then
                        mensajeValida = mensajeValida + "- La fecha fin de contrato (Vencimiento) no puede ser: " & ValidarDia & ".<br>"
                    End If
                End If

                If mensajeValida = "" Then
                    bolValidar = True
                End If
                If bolValidar Then
                    'INICIO | RCE | Zoluxiones | Se guarda con fecha de apertura de  portafolio | 13/08/2018
                    Dim ddlportafoliof As New DropDownList
                    ddlportafoliof = CType(row.FindControl("ddlPortafolioF"), DropDownList)

                    oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                    oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                    '    oRow.FechaOperacion = ViewState("decFechaOperacion")
                    ViewState("decFechaOperacion") = UIUtility.ObtenerFechaApertura(ddlportafoliof.SelectedValue.ToString)
                    oRow.FechaOperacion = UIUtility.ObtenerFechaApertura(ddlportafoliof.SelectedValue.ToString)
                    'FIN | RCE | Zoluxiones | Se guarda con fecha de apertura de  portafolio | 13/08/2018
                    oRow.HoraOperacion = Now.ToLongTimeString()
                    oRow.ClaseInstrumentoFx = ddlClaseInstrumentofxF.SelectedValue
                    oRow.CodigoTipoTitulo = ddlTipoTituloF.SelectedValue
                    oRow.MedioNegociacion = ddlMedioNegF.SelectedValue
                    oRow.MontoNominal = CType(tbTotalOrdenF.Text, Decimal)
                    oRow.Tasa = CType(tbTasaF.Text, Decimal)
                    oRow.TipoTasa = ddlTipoTasaF.SelectedValue
                    oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaContratoF.Text)
                    oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
                    oRow.MontoOperacion = obtenerMontoOperacionDPZ(ddlTipoTasaF.SelectedValue, CType(tbTotalOrdenF.Text, Decimal), CType(tbTasaF.Text, Decimal), _
                      oRow.FechaOperacion, UIUtility.ConvertirFechaaDecimal(tbFechaContratoF.Text), ddlTipoTituloF.SelectedValue)
                    oRow.CodigoTercero = hdIntermediarioF.Value
                    oRow.CodigoOperacion = ddlOperacionF.SelectedValue
                    oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                    oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO
                    oRow.Porcentaje = porcentajeF
                    oRow.CodigoNemonico = IIf(ddlClaseInstrumentofxF.SelectedValue = "DP", ddlTipoTituloF.SelectedValue, tbNemonicoF.Text)
                    If tbCantidadF.Text = "" Then
                        oRow.Cantidad = 0
                    Else
                        oRow.Cantidad = CDec(tbCantidadF.Text)
                    End If
                    oRow.CantidadOperacion = CDec(tbCantidadF.Text)
                    '  INICIO | RCE | Zoluxiones | Cuando valor es - por default enviamos valor 4 (NEW YORK) | 13/08/2018
                    oRow.CodigoPlaza = IIf(ddlPlazaNF.SelectedValue = "-", "4", ddlPlazaNF.SelectedValue)
                    '  FIN | RCE | Zoluxiones | Cuando valor es - por default enviamos valor 4 (NEW YORK) | 13/08/2018
                    oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                    oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                    oPrevOrdenInversionBM.Insertar(oPrevOrdenInversionBE, ParametrosSIT.TR_DERIVADOS.ToString(), DatosRequest, dtDetalleInversiones)
                    CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), "", "", _
                    ViewState("strOperador"), ViewState("strEstado"))
                    tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(CType(ViewState("decFechaOperacion"), String))
                    Session.Remove("dtDetalleInversiones")
                Else
                    AlertaJS(mensajeValida)
                End If
            End If
            If e.CommandName = "_Delete" Then
                Dim decCodigoPrevOrden As Decimal = CType(e.CommandArgument.ToString(), Decimal)
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                Dim script As String = ""
                Dim codigoClaseInstrumento As String = ""

                ddlClaseInstrumentofxF = CType(row.FindControl("ddlClaseInstrumentofxF"), DropDownList)
                codigoClaseInstrumento = ddlClaseInstrumentofxF.SelectedValue

                gvr = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                If gvr.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Then
                    Dim dtValidaExtorno As New DataTable
                    dtValidaExtorno = oPrevOrdenInversionBM.ValidaExtorno(decCodigoPrevOrden, DatosRequest).Tables(0)
                    If dtValidaExtorno.Rows.Count > 0 Then
                        script = "Para realizar el extorno, debe revertir las siguientes operaciones: <br>"
                        For Each fila As DataRow In dtValidaExtorno.Rows
                            script = script & " - " & fila("CodigoOrden").ToString() & ", " & fila("CodigoPortafolioSBS") & ", " & fila("Estado").ToString() & "<br>"
                        Next
                        AlertaJS(script)
                    Else
                        'showDialogoPopupExtorno(ParametrosSIT.TR_DERIVADOS.ToString(), decCodigoPrevOrden.ToString())
                        showDialogoPopupExtorno(ParametrosSIT.TR_DERIVADOS.ToString(), decCodigoPrevOrden.ToString(), codigoClaseInstrumento)
                    End If
                ElseIf gvr.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                    oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                ElseIf gvr.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then
                    oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                Else
                    oPrevOrdenInversionBM.Eliminar(decCodigoPrevOrden, DatosRequest)
                End If
                CargarGrilla(ParametrosSIT.TR_DERIVADOS.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), _
                ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))

            End If

        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub Datagrid1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Datagrid1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                lbMonedaNeg = CType(e.Row.FindControl("lbMonedaNeg"), Label)
                lbMoneda = CType(e.Row.FindControl("lbMoneda"), Label)
                lbOperacion = CType(e.Row.FindControl("lbOperacion"), Label)
                lbTipoTitulo = CType(e.Row.FindControl("lbTipoTitulo"), Label)
                lbModalidad = CType(e.Row.FindControl("lbModalidad"), Label)
                lbTipoTasa = CType(e.Row.FindControl("lbTipoTasa"), Label)
                lbMedioNeg = CType(e.Row.FindControl("lbMedioNeg"), Label)
                lbMotivo = CType(e.Row.FindControl("lbMotivo"), Label)
                lbClaseInstrumentofx = CType(e.Row.FindControl("lbClaseInstrumentofx"), Label)
                ddlOperacion = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
                ddlTipoTitulo = CType(e.Row.FindControl("ddlTipoTitulo"), DropDownList)
                ddlTipoTasa = CType(e.Row.FindControl("ddlTipoTasa"), DropDownList)
                ddlMedioNeg = CType(e.Row.FindControl("ddlMedioNeg"), DropDownList)
                ddlFondos = CType(e.Row.FindControl("ddlfondos"), DropDownList)
                ddlClaseInstrumentofx = CType(e.Row.FindControl("ddlClaseInstrumentofx"), DropDownList)
                ddlPlazaN = CType(e.Row.FindControl("ddlPlazaN"), DropDownList)
                lbPlazaN = CType(e.Row.FindControl("lbPlazaN"), Label)
                Imagebutton1 = CType(e.Row.FindControl("Imagebutton1"), ImageButton)
                'xxxx
                HelpCombo.LlenarComboBox(ddlClaseInstrumentofx, CType(Session("dtClaseifx"), DataTable), "Valor", "Nombre", False)
                HelpCombo.LlenarComboBox(ddlOperacion, oOperacionBM.ListarOperacion_Categoria(lbClaseInstrumentofx.Text), "codigoOperacion", "Descripcion", False)
                HelpCombo.LlenarComboBox(ddlTipoTitulo, oTipoTituloBM.ListarTipoTitulo_CCI(lbClaseInstrumentofx.Text), "CodigoTipoTitulo", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlTipoTasa, CType(Session("dtTipoTasa"), DataTable), "Valor", "Nombre", True)
                HelpCombo.LlenarComboBox(ddlMedioNeg, CType(Session("dtMedioNeg"), DataTable), "Valor", "Nombre", False)
                HelpCombo.LlenarComboBox(ddlPlazaN, oPlazaBM.ListarxOrden(DatosRequest).Tables(0), "CodigoPlaza", "Descripcion", False)
                ddlPlazaN.Items.Insert(0, "-")
                Dim fondos As String() = CType(e.Row.FindControl("hdFondo1Trz"), HtmlInputHidden).Value.Split("/")
                For Each fondo As String In fondos
                    ddlFondos.Items.Add(fondo)
                Next
                If Not lbPlazaN.Text = "" Then
                    If lbClaseInstrumentofx.Text.ToUpper = "DP" Then
                        ddlPlazaN.SelectedValue = "-"
                    Else
                        ddlPlazaN.SelectedValue = lbPlazaN.Text
                    End If

                End If
                If Not lbOperacion.Text = "" Then
                    ddlOperacion.SelectedValue = lbOperacion.Text
                End If
                ddlTipoTitulo.SelectedValue = lbTipoTitulo.Text
                ddlTipoTasa.SelectedValue = lbTipoTasa.Text
                If lbMedioNeg.Text <> String.Empty Then
                    ddlMedioNeg.SelectedValue = lbMedioNeg.Text
                End If
                ddlClaseInstrumentofx.SelectedValue = lbClaseInstrumentofx.Text
                If lbClaseInstrumentofx.Text = "OR" Then
                    ddlPlazaN.Enabled = True
                End If
                hdTipoTituloTrz = CType(e.Row.FindControl("hdTipoTituloTrz"), HtmlInputHidden)
                hdTipoTituloTrz.Value = ddlTipoTitulo.SelectedItem.Text
                If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or
                    e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Or e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then
                    tbHora = CType(e.Row.FindControl("tbHora"), TextBox)
                    tbTotalOrden = CType(e.Row.FindControl("tbTotalOrden"), TextBox)
                    tbTasa = CType(e.Row.FindControl("tbTasa"), TextBox)
                    tbFechaContrato = CType(e.Row.FindControl("tbFechaContrato"), TextBox)
                    tbTotalOperacion = CType(e.Row.FindControl("tbTotalOperacion"), TextBox)
                    tbIntermediario = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                    chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)
                    tbHora.Enabled = False
                    tbTotalOrden.Enabled = False
                    ddlPlazaN.Enabled = False
                    tbTasa.Enabled = False
                    tbFechaContrato.Enabled = False
                    tbTotalOperacion.Enabled = False
                    tbIntermediario.Enabled = False
                    ddlOperacion.Enabled = False
                    ddlTipoTitulo.Enabled = False
                    ddlTipoTasa.Enabled = False
                    ddlMedioNeg.Enabled = False
                    ddlClaseInstrumentofx.Enabled = False
                    chkSelect.Enabled = False
                    If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or _
                    e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Then
                        chkSelect.Enabled = True
                    End If
                    Dim FechaVal As HtmlGenericControl
                    FechaVal = CType(e.Row.FindControl("FechaContrato"), HtmlGenericControl)
                    FechaVal.Attributes.Add("class", "input-append")
                End If
                tbHora2 = CType(e.Row.FindControl("tbHora"), TextBox)
                ddlClaseInstrumentofx2 = CType(e.Row.FindControl("ddlClaseInstrumentofx"), DropDownList)
                ddlOperacion2 = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
                ddlTipoTitulo2 = CType(e.Row.FindControl("ddlTipoTitulo"), DropDownList)
                tbTotalOrden2 = CType(e.Row.FindControl("tbTotalOrden"), TextBox)
                ddlTipoTasa2 = CType(e.Row.FindControl("ddlTipoTasa"), DropDownList)
                tbTasa2 = CType(e.Row.FindControl("tbTasa"), TextBox)
                tbFechaContrato2 = CType(e.Row.FindControl("tbFechaContrato"), TextBox)
                tbIntermediario2 = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                ddlMedioNeg2 = CType(e.Row.FindControl("ddlMedioNeg"), DropDownList)
                hdPorcentaje2 = CType(e.Row.FindControl("hdPorcentaje"), HtmlInputHidden)
                chkPorcentaje2 = CType(e.Row.FindControl("chkPorcentaje"), CheckBox)
                tbHora2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlClaseInstrumentofx2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlOperacion2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlTipoTitulo2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbTotalOrden2.Attributes.Add("onchange", "javascript:cambio(this);")
                ddlTipoTasa2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbTasa2.Attributes.Add("onchange", "javascript:cambio(this);")
                'tbFechaContrato2.Attributes.Add("onchange", "javascript:cambio(this);")
                tbIntermediario2.Attributes.Add("onpropertychange", "javascript:cambio(this);")
                ddlMedioNeg2.Attributes.Add("onchange", "javascript:cambio(this);")
                chkPorcentaje2.Attributes.Add("onchange", "javascript:cambio(this);")
                If hdPorcentaje2.Value = "S" Then
                    chkPorcentaje2.Checked = True
                Else
                    chkPorcentaje2.Checked = False
                End If
                '  INICIO | RCE | Zoluxiones | Se habilita row de acuerdo a estado de la Preorden de inversión "Eliminado" de acuerdo a fecha de búsqueda | 13/08/2018
                If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then
                    e.Row.Enabled = False
                End If
                '  FIN | RCE | Zoluxiones | Se habilita row de acuerdo a estado de la Preorden de inversión "Eliminado" de acuerdo a fecha de búsqueda | 13/08/2018
            End If
            If e.Row.RowType = DataControlRowType.Footer Then
                ddlOperacionF = CType(e.Row.FindControl("ddlOperacionF"), DropDownList)
                ddlTipoTituloF = CType(e.Row.FindControl("ddlTipoTituloF"), DropDownList)
                ddlTipoTasaF = CType(e.Row.FindControl("ddlTipoTasaF"), DropDownList)
                ddlMedioNegF = CType(e.Row.FindControl("ddlMedioNegF"), DropDownList)
                ddlPlazaNF = CType(e.Row.FindControl("ddlPlazaNF"), DropDownList)
                ddlClaseInstrumentofxF = CType(e.Row.FindControl("ddlClaseInstrumentofxF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlClaseInstrumentofxF, CType(Session("dtClaseifx"), DataTable), "Valor", "Nombre", False)
                HelpCombo.LlenarComboBox(ddlOperacionF, oOperacionBM.ListarOperacion_Categoria(ddlClaseInstrumentofxF.SelectedValue), "codigoOperacion", "Descripcion", False)
                HelpCombo.LlenarComboBox(ddlTipoTituloF, oTipoTituloBM.ListarTipoTitulo_CCI(ddlClaseInstrumentofxF.SelectedValue), "CodigoTipoTitulo", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlTipoTasaF, CType(Session("dtTipoTasa"), DataTable), "Valor", "Nombre", True)
                HelpCombo.LlenarComboBox(ddlMedioNegF, CType(Session("dtMedioNeg"), DataTable), "Valor", "Nombre", False)
                HelpCombo.LlenarComboBox(ddlPlazaNF, oPlazaBM.ListarxOrden(DatosRequest).Tables(0), "CodigoPlaza", "Descripcion", False)
                ddlPlazaNF.Items.Insert(0, "-")
                ddlTipoTasaF.SelectedValue = "2"

                Dim ddlportafoliof As New DropDownList
                ddlportafoliof = CType(e.Row.FindControl("ddlPortafolioF"), DropDownList)

                HelpCombo.LlenarComboBox(ddlportafoliof, objPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO), "CodigoPortafolio", "Descripcion", True)
                'ddlportafoliof.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, "0"))
                'ddlportafoliof.SelectedValue = "0"
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub Modulos_Inversiones_frmIngresoMasivoOperacionFX_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Cro.Close()
        Cro.Dispose()
        oReport.Close()
        oReport.Dispose()
    End Sub
    Protected Sub ddlClaseInstrumentofxF_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ddlOperacionF = CType(Datagrid1.FooterRow.FindControl("ddlOperacionF"), DropDownList)
            ddlClaseInstrumentofxF = CType(Datagrid1.FooterRow.FindControl("ddlClaseInstrumentofxF"), DropDownList)
            ddlTipoTituloF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTituloF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlOperacionF, oOperacionBM.ListarOperacion_Categoria(ddlClaseInstrumentofxF.SelectedValue), "codigoOperacion", "Descripcion", False)
            HelpCombo.LlenarComboBox(ddlTipoTituloF, oTipoTituloBM.ListarTipoTitulo_CCI(ddlClaseInstrumentofxF.SelectedValue), "CodigoTipoTitulo", "Descripcion", True)
            tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox)
            tbCantidadF = CType(Datagrid1.FooterRow.FindControl("tbCantidadF"), TextBox)
            ddlPlazaNF = CType(Datagrid1.FooterRow.FindControl("ddlPlazaNf"), DropDownList)
            If ddlClaseInstrumentofxF.SelectedValue = "DP" Then
                tbNemonicoF.ReadOnly = True
                tbCantidadF.ReadOnly = True
                ddlPlazaNF.Enabled = False
                'INICIO | RCE | Zoluxiones | Cuando es DP no tiene definido una plaza | 13/08/2018
                ddlPlazaNF.Items.Insert(0, "-")
                ddlPlazaNF.SelectedValue = "-"
                'INICIO | RCE | Zoluxiones | Cuando es DP no tiene definido una plaza | 13/08/2018
            Else
                tbNemonicoF.ReadOnly = False
                tbCantidadF.ReadOnly = False
                ddlPlazaNF.Enabled = True
                ddlPlazaNF.SelectedValue = "7"
                ddlPlazaNF.Items.Remove("-")
                ddlPlazaNF.Items.Remove("-")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub tbFechaContrato_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            For Each row As GridViewRow In Datagrid1.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    tbFechaContrato = CType(row.FindControl("tbFechaContrato"), TextBox)
                    ddlClaseInstrumentofx = CType(row.FindControl("ddlClaseInstrumentofx"), DropDownList)
                    hdIntermediario = CType(row.FindControl("hdIntermediario"), HtmlInputHidden)

                    If ddlClaseInstrumentofx.SelectedValue = "DP" Then
                        ValidarDia = ValidarFechaVencimiento(tbFechaContrato.Text, hdIntermediario.Value)
                        If ValidarDia <> String.Empty Then
                            AlertaJS("- La fecha fin de contrato (Vencimiento) no puede ser: " & ValidarDia & ".")
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub tbFechaContratoF_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try     
            tbFechaContratoF = CType(Datagrid1.FooterRow.FindControl("tbFechaContratoF"), TextBox)
            ddlClaseInstrumentofxF = CType(Datagrid1.FooterRow.FindControl("ddlClaseInstrumentofxF"), DropDownList)
            hdIntermediarioF = CType(Datagrid1.FooterRow.FindControl("hdIntermediarioF"), HtmlInputHidden)

            If ddlClaseInstrumentofxF.SelectedValue = "DP" Then
                ValidarDia = ValidarFechaVencimiento(tbFechaContratoF.Text, hdIntermediarioF.Value)
                If ValidarDia <> String.Empty Then
                    AlertaJS("- La fecha fin de contrato (Vencimiento) no puede ser: " & ValidarDia & ".")
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub ddlClaseInstrumentofx_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            For Each row As GridViewRow In Datagrid1.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    ddlClaseInstrumentofx = CType(row.Cells(5).FindControl("ddlClaseInstrumentofx"), DropDownList)
                    If ddlClaseInstrumentofx.ClientID = CType(sender, DropDownList).ClientID Then
                        ddlOperacion = TryCast(row.Cells(6).FindControl("ddlOperacion"), DropDownList)
                        ddlTipoTitulo = CType(row.Cells(7).FindControl("ddlTipoTitulo"), DropDownList)
                        tbNemonico = CType(row.Cells(8).FindControl("tbNemonico"), TextBox)
                        tbCantidad = CType(row.Cells(9).FindControl("tbCantidad"), TextBox)
                        ddlPlazaN = CType(row.Cells(15).FindControl("ddlPlazaN"), DropDownList)
                        HelpCombo.LlenarComboBox(ddlOperacion, oOperacionBM.ListarOperacion_Categoria(ddlClaseInstrumentofx.SelectedValue), "codigoOperacion", "Descripcion", False)
                        HelpCombo.LlenarComboBox(ddlTipoTitulo, oTipoTituloBM.ListarTipoTitulo_CCI(ddlClaseInstrumentofx.SelectedValue), "CodigoTipoTitulo", "Descripcion", True)
                        If ddlClaseInstrumentofx.SelectedValue = "DP" Then
                            tbNemonico.ReadOnly = True
                            tbCantidad.ReadOnly = True
                            ddlPlazaN.Enabled = False
                            'INICIO | RCE | Zoluxiones | Cuando es DP no tiene definido una plaza | 13/08/2018
                            ddlPlazaN.Items.Insert(0, "-")
                            ddlPlazaN.SelectedValue = "-"
                            'INICIO | RCE | Zoluxiones | Cuando es DP no tiene definido una plaza | 13/08/2018
                        Else
                            tbNemonico.ReadOnly = False
                            tbCantidad.ReadOnly = False
                            ddlPlazaN.Enabled = True
                            ddlPlazaN.SelectedValue = "7"
                            ddlPlazaN.Items.Remove("-")
                        End If
                    End If
                End If
            Next
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
    Private Function ValidarFechaVencimiento(ByVal fechaContrato As String, ByVal intermediario As String) As String
        Dim dia As String = String.Empty
        Dim Fecha As Date = fechaContrato
        If Fecha.DayOfWeek = DayOfWeek.Saturday Then
            dia = "Sábado"
        ElseIf Fecha.DayOfWeek = DayOfWeek.Sunday Then
            dia = "Domingo"
        ElseIf intermediario.Trim.Length > 0 Then
            dia = New UtilDM().ValidarFechaHabil(Fecha.ToString("yyyyMMdd"), intermediario.Trim)
        End If
        If dia.Equals("") Then
            Return String.Empty
        Else
            Return dia.Trim
        End If
    End Function
End Class
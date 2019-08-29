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
Imports System.Collections.Generic 'CMB OT 65473 20120918   
Imports ParametrosSIT
Imports System.Globalization
Imports System.Threading

Partial Class Modulos_Inversiones_frmIngresoMasivoOperacionRF__
    Inherits BasePage
    Dim pRutas As String    'HDG 20120228
    Dim rutas As New System.Text.StringBuilder  'HDG 20120228

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    Public Function instanciarTabla(ByVal tabla As DataTable) As DataTable
        tabla = New DataTable
        tabla.Columns.Add("CodigoPrevOrden")
        tabla.Columns.Add("CodigoPortafolio")
        tabla.Columns.Add("Asignacion")
        tabla.Columns.Add("Cambio")
        Return tabla
    End Function
    Private Sub btnAprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
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
                    EjecutarOrdenInversion(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), strCodPrevOrden, , , decNProceso)
                    CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                Else
                    AlertaJS("Seleccione el registro a ejecutar")
                    oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
                End If
            Else
                AlertaJS("Ningun registro cumple el requerimiento de estado.")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim decFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            ViewState("decFechaOperacion") = decFechaOperacion
            Dim strCodigoClaseInstrumento As String = ddlClaseInstrumento.SelectedValue
            Dim strCodigoTipoInstrumentoSBS As String = ddlTipoInstrumento.SelectedValue
            Dim strOperador As String = ddlOperador.SelectedValue
            Dim strCodigoNemonico As String = tbCodigoMnemonico.Text
            Dim strEstado As String = ddlEstado.SelectedValue
            ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
            ViewState("strTipoInstrumento") = ddlTipoInstrumento.SelectedValue
            ViewState("strOperador") = ddlOperador.SelectedValue
            ViewState("strCodigoNemonico") = tbCodigoMnemonico.Text
            ViewState("strEstado") = ddlEstado.SelectedValue
            CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, strOperador, strEstado)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnGrabar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGrabar.Click
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
            Dim lbCodigoPrevOrden As Label
            Dim tbHora As TextBox
            Dim ddlOperacion As DropDownList
            Dim ddlMedioNeg As DropDownList
            Dim tbNemonico As TextBox
            Dim ddlCondicion As DropDownList
            Dim ddlTipoTasa As DropDownList
            Dim tbFechaLiquidacion As TextBox
            Dim tbCantidad As TextBox
            Dim tbPrecio As TextBox
            Dim tbCantidadOperacion As TextBox
            Dim tbPrecioOperacion As TextBox
            Dim hdIntermediario As HtmlControls.HtmlInputHidden
            Dim tbTasa As TextBox
            Dim ddlPlazaN As DropDownList
            Dim ddlIndice As DropDownList
            Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
            Dim ddlTipoFondo As DropDownList
            Dim ddlTipoTramo As DropDownList
            Dim strMensaje As String = ""
            Dim tbFixing As TextBox
            Dim objFormulasOI As New OrdenInversionFormulasBM
            Dim precioOperacion As Decimal
            Dim tasa As Decimal
            Dim totalOperacionRF As Decimal
            Dim precioEjecutado As Decimal
            Dim totalEjecutadoRF As Decimal
            Dim bolValidaCampos As Boolean = False
            Dim fixing As Decimal = 0
            Dim validaFixing As Boolean = True
            Dim hdCambio As HtmlControls.HtmlInputHidden
            Dim oTrazabilidadOperacionBE As New TrazabilidadOperacionBE
            Dim hdCambioTraza As HtmlControls.HtmlInputHidden
            Dim hdCambioTrazaFondo As HtmlControls.HtmlInputHidden
            Dim ddlPortafolio As DropDownList
            Dim porcentaje As String
            Dim dtDetalleInversiones As New DataTable
            Dim dtDetalleInversionesTotal As New DataTable
            Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                    hdCambio = CType(fila.FindControl("hdCambio"), HtmlControls.HtmlInputHidden)
                    hdCambioTraza = CType(fila.FindControl("hdCambioTraza"), HtmlControls.HtmlInputHidden)
                    hdCambioTrazaFondo = CType(fila.FindControl("hdCambioTrazaFondo"), HtmlControls.HtmlInputHidden)
                    porcentaje = "N"
                    If fila.Cells(2).Text = PREV_OI_INGRESADO Then
                        If (Not lbCodigoPrevOrden Is Nothing And hdCambio.Value = "1") Then
                            hdCambio.Value = ""
                            tbHora = CType(fila.FindControl("tbHora"), TextBox)
                            tbNemonico = CType(fila.FindControl("tbNemonico"), TextBox)
                            hdIntermediario = CType(fila.FindControl("hdIntermediario"), HtmlControls.HtmlInputHidden)
                            ddlOperacion = CType(fila.FindControl("ddlOperacion"), DropDownList)
                            ddlMedioNeg = CType(fila.FindControl("ddlMedioNeg"), DropDownList)
                            ddlCondicion = CType(fila.FindControl("ddlCondicion"), DropDownList)
                            ddlTipoTasa = CType(fila.FindControl("ddlTipoTasa"), DropDownList)
                            tbFechaLiquidacion = CType(fila.FindControl("tbFechaLiquidacion"), TextBox)
                            tbCantidad = CType(fila.FindControl("tbCantidad"), TextBox)
                            tbPrecio = CType(fila.FindControl("tbPrecio"), TextBox)
                            tbCantidadOperacion = CType(fila.FindControl("tbCantidadOperacion"), TextBox)
                            tbPrecioOperacion = CType(fila.FindControl("tbPrecioOperacion"), TextBox)
                            tbTasa = CType(fila.FindControl("tbTasa"), TextBox)
                            ddlPlazaN = CType(fila.FindControl("ddlPlazaN"), DropDownList)
                            ddlIndice = CType(fila.FindControl("ddlIndice"), DropDownList)

                            hdClaseInstrumento = CType(fila.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                            ddlTipoFondo = CType(fila.FindControl("ddlTipoFondo"), DropDownList)
                            ddlTipoTramo = CType(fila.FindControl("ddlTipoTramo"), DropDownList)
                            ddlPortafolio = CType(fila.FindControl("ddlfondos"), DropDownList)

                            tbFixing = CType(fila.FindControl("tbFixing"), TextBox)
                            If tbFixing.Text <> "" Then
                                fixing = CType(tbFixing.Text, Decimal)
                                If fixing < 0 Then
                                    fixing = -1
                                End If
                            Else
                                fixing = -1
                            End If
                            bolValidaCampos = False
                            If tbCantidad.Text <> "" And _
                                tbNemonico.Text <> "" And _
                                hdIntermediario.Value <> "" And _
                                Not ((hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And ddlTipoFondo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) And _
                                Not (hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And ddlTipoTramo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) Then   'HDG OT 20120517
                                validaFixing = ValidarFixing(hdClaseInstrumento.Value, tbNemonico.Text, fixing)
                                If ValidarNemonico(tbNemonico.Text) And _
                                    ValidarIntermediario(hdIntermediario.Value.ToString) And _
                                    ValidarFechaVencimiento(tbFechaLiquidacion.Text, tbNemonico.Text) And _
                                    validaFixing = True Then
                                    If fila.Cells(2).Text <> ParametrosSIT.PREV_OI_EJECUTADO And _
                                        fila.Cells(2).Text <> ParametrosSIT.PREV_OI_APROBADO And _
                                        fila.Cells(2).Text <> ParametrosSIT.PREV_OI_PENDIENTE Then
                                        bolValidaCampos = True
                                    End If
                                End If
                            Else
                                If (hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And ddlTipoFondo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                                    strMensaje = strMensaje + "- Seleccione Tipo. <br />"
                                End If
                                If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And ddlTipoTramo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                                    strMensaje = strMensaje + "- Seleccione Tipo Tramo. <br />"
                                End If
                            End If
                            If bolValidaCampos = True Then
                                Dim Mensaje As String = ""
                                If (hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) Then   'HDG 20120517
                                    precioOperacion = Val(tbPrecio.Text)
                                    totalOperacionRF = precioOperacion * CType(tbCantidad.Text, Decimal)
                                    precioEjecutado = Val(tbPrecioOperacion.Text)
                                    totalEjecutadoRF = precioEjecutado * CType(tbCantidadOperacion.Text, Decimal)
                                Else
                                    tasa = Val(tbTasa.Text)
                                    precioOperacion = Val(tbPrecio.Text)
                                    totalOperacionRF = OrdenInversion.CalculaMontoOperacion(tbNemonico.Text, tbCantidad.Text, ddlIndice.SelectedValue, tbFechaLiquidacion.Text, ddlTipoTasa.SelectedValue, tbFechaOperacion.Text, precioOperacion, tasa, Mensaje, DatosRequest).ToString()
                                    precioEjecutado = Val(tbPrecioOperacion.Text)
                                    totalEjecutadoRF = OrdenInversion.CalculaMontoOperacion(tbNemonico.Text, tbCantidadOperacion.Text, ddlIndice.SelectedValue, tbFechaLiquidacion.Text, ddlTipoTasa.SelectedValue, tbFechaOperacion.Text, precioEjecutado, tasa, Mensaje, DatosRequest).ToString()
                                    If Mensaje.Length > 0 Then
                                        AlertaJS(Mensaje)
                                        Exit Sub
                                    End If
                                End If
                                oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                                oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                                oRow.CodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                                oRow.HoraOperacion = tbHora.Text
                                oRow.CodigoNemonico = tbNemonico.Text
                                oRow.CodigoOperacion = ddlOperacion.SelectedValue
                                oRow.CodigoTercero = hdIntermediario.Value
                                oRow.CodigoPlaza = ddlPlazaN.SelectedValue
                                oRow.MedioNegociacion = ddlMedioNeg.SelectedValue
                                oRow.TipoTasa = ddlTipoTasa.SelectedValue
                                oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
                                oRow.Cantidad = CType(tbCantidad.Text, Decimal)
                                oRow.Precio = CType(IIf(tbPrecio.Text = "", -1, tbPrecio.Text), Decimal)
                                oRow.MontoNominal = totalOperacionRF
                                oRow.CantidadOperacion = CType(tbCantidadOperacion.Text, Decimal)
                                oRow.PrecioOperacion = precioEjecutado
                                oRow.MontoOperacion = totalEjecutadoRF
                                oRow.Tasa = tasa
                                oRow.IndPrecioTasa = ddlIndice.SelectedValue
                                oRow.TipoFondo = IIf(ddlTipoFondo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoFondo.SelectedValue)
                                oRow.TipoTramo = IIf(ddlTipoTramo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoTramo.SelectedValue)
                                oRow.Porcentaje = porcentaje
                                oRow.TipoCondicion = ddlCondicion.SelectedValue
                                oRow.Fixing = fixing
                                oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                                oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                                oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                            End If
                        End If
                    End If
                End If
            Next
            If oPrevOrdenInversionBE.PrevOrdenInversion.Rows.Count > 0 Then
                oPrevOrdenInversionBM.Modificar(oPrevOrdenInversionBE, DatosRequest)
            End If
            CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
            AlertaJS("Operaciones modificadas correctamente.")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub TotalizarDetalleInversion(ByRef dtTotal As DataTable, ByVal dt As DataTable)
        If dtTotal.Rows.Count = 0 Then
            dtTotal = dt.Copy
        Else
            For Each dr As DataRow In dt.Rows
                dtTotal.ImportRow(dr)
            Next
        End If

    End Sub

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

    Private Sub btnValidar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidar.Click
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
            Dim lbCodigoPrevOrden As Label
            Dim count As Integer = 0
            Dim decNProceso As Decimal = 0

            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)

            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
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
                            End If
                        End If
                    End If
                End If
            Next

            'SE PROCEDE A VALIDAR
            If count > 0 Then
                Dim mensaje As String = Limites.VerificaExcesoLimites(Usuario, ParametrosSIT.TR_RENTA_FIJA.ToString(), decNProceso)
                Dim dt As New DataTable
                dt = oPrevOrdenInversionBM.SeleccionarValidacionExcesos(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), DatosRequest, decNProceso).Tables(0)
                If dt.Rows.Count > 0 Then
                    Session("dtListaExcesos") = dt
                    AlertaJS(mensaje, "window.showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" + ParametrosSIT.TR_RENTA_FIJA.ToString() + "','','dialogHeight:550px;dialogWidth:789px;status:no;unadorned:yes;help:No');document.getElementById('btnBuscar').click();")
                Else
                    AlertaJS(mensaje)
                End If
            Else
                AlertaJS("Seleccione los registros a validar")
                oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            GenerarReporteRentaFija()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub GenerarReporteRentaFija()
        Dim oldCulture As CultureInfo
        Try
            Dim sFile As String, sTemplate As String
            Dim dtRentaFija As New DataTable
            Dim dtResumen As New DataTable
            Dim dtResumenSBS As New DataTable
            Dim oDs As New DataSet
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim decNProceso As Decimal = 0
            oldCulture = Thread.CurrentThread.CurrentCulture
            Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            oDs = oPrevOrdenInversionBM.GenerarReporte(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), DatosRequest, decNProceso)
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            dtRentaFija = oDs.Tables(0)
            dtResumenSBS = oDs.Tables(4)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RF_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls" 'CMB OT 62087 20110427 REQ 6
            Dim n As Integer
            Dim dr As DataRow
            Dim oExcel As New Excel.Application
            Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
            Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet, oSheetSBS As Excel.Worksheet
            Dim oCells As Excel.Range
            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaPrevOrdenInversionRF.xls"
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
            For Each dr In dtRentaFija.Rows
                oSheet.Rows(n & ":" & n).Copy()
                oSheet.Application.CutCopyMode = False
                oCells(n, 1) = dr("Correlativo")
                oCells(n, 2) = dr("HoraOperacion")
                oCells(n, 3) = dr("UsuarioCreacion")
                oCells(n, 4) = dr("CodigoNemonico")
                oCells(n, 5) = dr("Instrumento")
                oCells(n, 6) = dr("Operacion")
                oCells(n, 7) = dr("Cantidad")
                oCells(n, 8) = dr("Precio")
                oCells(n, 9) = CType(dr("Tasa"), String)
                oCells(n, 10) = dr("Condicion")
                oCells(n, 11) = dr("Intermediario")
                oCells(n, 12) = dr("MedioNegociacion")
                oCells(n, 14) = dr("IntervaloPrecio")
                oCells(n, 15) = dr("CantidadOperacion")
                oCells(n, 16) = dr("PrecioOperacion")
                oCells(n, 17) = CType(dr("Tasa"), String)
                oCells(n, 18) = dr("Portafolico")
                oCells(n, 20) = dr("Ejecutado")
                n = n + 1
            Next
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp) 'HDG 2012011
            'REPORTE DE LA SBS
            oSheetSBS = CType(oSheets.Item(2), Excel.Worksheet)
            oCells = oSheetSBS.Cells
            oCells(2, 12) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheetSBS.SaveAs(sFile)
            n = 6
            For Each dr In dtResumenSBS.Rows
                oCells(n, 2) = dr("Afp")
                oCells(n, 3) = dr("Fondo")
                oCells(n, 4) = dr("Nemonico")
                oCells(n, 5) = dr("HoraOrden")
                oCells(n, 6) = dr("HoraOperacion")
                oCells(n, 7) = dr("Movimiento")
                oCells(n, 8) = dr("Cantidad")
                oCells(n, 8).NumberFormat = "###,###,##0"
                oCells(n, 9) = dr("Precio")
                oCells(n, 9).NumberFormat = "###,###,##0.0000"
                oCells(n, 10) = dr("Intermediario")
                oCells(n, 11) = dr("Resultado")
                oCells(n, 12) = dr("NumeroOperacion")
                oCells(n, 13) = dr("MedioTransmision")
                n = n + 1
            Next
            oBook.Save()
            oBook.Close()
            oExcel.Quit()
            ReleaseComObject(oCells) : ReleaseComObject(oSheet) : ReleaseComObject(oSheetSBS)
            ReleaseComObject(oSheets) : ReleaseComObject(oBook)
            ReleaseComObject(oBooks) : ReleaseComObject(oExcel)
            oExcel = Nothing : oBooks = Nothing : oBook = Nothing
            oSheets = Nothing : oSheet = Nothing : oCells = Nothing : oSheetSBS = Nothing
            System.GC.Collect()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "FX_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
            Response.WriteFile(sFile)
            Response.End()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
        Thread.CurrentThread.CurrentCulture = oldCulture
    End Sub

    Private Sub CargarPagina()
        'btnBuscarNemonico.Attributes.Add("onclick", "javascript:showPopupMnemonico();")
        CargarCombos()

        hdFechaNegocio.Value = UIUtility.ObtenerFechaMaximaNegocio()
        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(hdFechaNegocio.Value)

        ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
        ViewState("strTipoInstrumento") = ddlTipoInstrumento.SelectedValue
        ViewState("strOperador") = ddlOperador.SelectedValue
        ViewState("strCodigoNemonico") = tbCodigoMnemonico.Text
        ViewState("strEstado") = ddlEstado.SelectedValue

        Dim dtOperacion As New DataTable
        Dim oOperacionBM As New OperacionBM
        dtOperacion = oOperacionBM.SeleccionarporClaseinstrumento("OperacionOI", ParametrosSIT.ESTADO_ACTIVO).Tables(0)
        Session("dtOperacion") = dtOperacion

        Dim dtIntermediario As New DataTable
        Dim oTercerosBM As New TercerosBM
        dtIntermediario = oTercerosBM.ListarTerceroPorGrupoIntermediario(ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO, "").Tables(0)
        Session("dtIntermediario") = dtIntermediario

        Dim dtMedioNeg As New DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtMedioNeg = oParametrosGeneralesBM.ListarMedioNegociacionPrevOI(ParametrosSIT.TR_RENTA_FIJA).Tables(0)   'HDG OT 64291 20111021
        Session("dtMedioNeg") = dtMedioNeg

        Dim dtCondicion As New DataTable
        dtCondicion = oParametrosGeneralesBM.ListarCondicionPrevOI().Tables(0)
        Session("dtCondicion") = dtCondicion

        Dim dtPlazaN As New DataTable
        Dim oPlazaBM As New PlazaBM
        dtPlazaN = oPlazaBM.Listar(DatosRequest).Tables(0)
        Session("dtPlazaN") = dtPlazaN

        Dim dtTipoTasa As New DataTable
        dtTipoTasa = oParametrosGeneralesBM.Listar("TipoTasaI", DatosRequest)
        Session("dtTipoTasa") = dtTipoTasa

        Dim dtIndice As New DataTable
        dtIndice = oParametrosGeneralesBM.Listar(ParametrosSIT.INDICE_PRECIO_TASA, DatosRequest)
        Session("dtIndice") = dtIndice

        'ini HDG 2012
        Dim dtTipoFondo As DataTable
        dtTipoFondo = New ParametrosGeneralesBM().ListarFondosInversion(DatosRequest, "M")
        Session("dtTipoFondo") = dtTipoFondo

        Dim dtTipoTramo As DataTable
        dtTipoTramo = New ParametrosGeneralesBM().Listar("TIPOTRAMO", DatosRequest)
        Session("dtTipoTramo") = dtTipoTramo
        'fin HDG OT 64034 04 20111111

        CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"))

        'RGF 20110505 OT 63063 REQ 01
        hdPuedeNegociar.Value = New PersonalBM().VerificaPermisoNegociacion(Usuario)
        If hdPuedeNegociar.Value = "0" Then
            HabilitaControles(False)
        End If
    End Sub

    Private Sub CargarCombos()
        CargarOperadores()
        CargaClaseInstrumento()
        CargarTipoInstrumento("")
        CargarEstados()
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
        dsClaseInstrumento = oClaseInstrumentoBM.SeleccionarClaseInstrumentoPorTipoRenta(ParametrosSIT.TR_RENTA_FIJA, DatosRequest)
        HelpCombo.LlenarComboBox(ddlClaseInstrumento, dsClaseInstrumento.Tables(0), "Codigo", "Descripcion", True)
        ddlClaseInstrumento.SelectedIndex = 0
    End Sub

    Private Sub CargarTipoInstrumento(ByVal codigoClaseInstrumento As String)
        'TIPO INSTRUMENTO
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim dt As New DataTable
        dt = oTipoInstrumentoBM.SeleccionarPorFiltro(codigoClaseInstrumento, "", ParametrosSIT.TR_RENTA_FIJA, "A", DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoInstrumento, dt, "CodigoTipoInstrumentoSBS", "Descripcion", True)
        ddlTipoInstrumento.SelectedIndex = 0
    End Sub

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

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    'RGF 20110317
    Private Sub HabilitaControles(ByVal habilita As Boolean, Optional ByVal AntesApertura As Boolean = False)   'HDG OT 64480 20120120
        btnGrabar.Enabled = habilita
        btnValidar.Enabled = habilita
        btnValidarTrader.Enabled = habilita
        btnAprobar.Enabled = habilita
        Datagrid1.Enabled = habilita
        'ini HDG OT 64480 20120120
        If AntesApertura Then
            btnGrabar.Enabled = True
            Datagrid1.Enabled = True
        End If
        'fin HDG OT 64480 20120120
    End Sub

    Private Sub btnBuscar_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim decFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            Dim decFechaActual As Decimal = Convert.ToDecimal(DateTime.Now.ToString("yyyyMMdd"))    'HDG OT 64480 20120120

            'RGF 20110317
            If decFechaOperacion = hdFechaNegocio.Value And hdPuedeNegociar.Value = "1" Then 'RGF 20110505 OT 63063 REQ 01
                HabilitaControles(True)
            ElseIf decFechaOperacion > hdFechaNegocio.Value And hdPuedeNegociar.Value = "1" Then    'HDG OT 64480 20120120
                HabilitaControles(False)
            Else
                HabilitaControles(False)
            End If

            ViewState("decFechaOperacion") = decFechaOperacion
            Dim strCodigoClaseInstrumento As String = ddlClaseInstrumento.SelectedValue
            Dim strCodigoTipoInstrumentoSBS As String = ddlTipoInstrumento.SelectedValue
            Dim strOperador As String = ddlOperador.SelectedValue
            Dim strCodigoNemonico As String = tbCodigoMnemonico.Text
            Dim strEstado As String = ddlEstado.SelectedValue
            ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
            ViewState("strTipoInstrumento") = ddlTipoInstrumento.SelectedValue
            ViewState("strOperador") = ddlOperador.SelectedValue
            ViewState("strCodigoNemonico") = tbCodigoMnemonico.Text
            ViewState("strEstado") = ddlEstado.SelectedValue
            CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, strOperador, strEstado)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Function ValidarNemonico(ByVal strNemonico As String) As Boolean
        Dim oValoresBM As New ValoresBM
        Dim bolResult As Boolean = False
        Dim strTipoRenta As String = ""

        strTipoRenta = oValoresBM.SeleccionarTipoRentaPorCodigoNemonico(strNemonico)
        If strTipoRenta <> "" Then
            If ParametrosSIT.TIPO_RENTA_FIJA.ToString.Replace("_", " ") = strTipoRenta Then
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

    Private Function ValidarFechaVencimiento(ByVal strFechaVencimiento As String, ByVal strNemonico As String) As Boolean
        Dim feriado As New FeriadoBM
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim bolResult As Boolean = False
        Dim decFechaVenc As Decimal
        Dim decFechaOpe As Decimal
        Dim dtValor As New DataTable

        dtValor = oPrevOrdenInversionBM.SeleccionarCaracValor(strNemonico, DatosRequest).Tables(0)
        If dtValor.Rows.Count > 0 Then
            If (feriado.VerificaDia(UIUtility.ConvertirFechaaDecimal(strFechaVencimiento), dtValor.Rows(0)("CodigoMercado").ToString()) = True) Then
                decFechaVenc = UIUtility.ConvertirFechaaDecimal(strFechaVencimiento)
                decFechaOpe = ViewState("decFechaOperacion")
                If Not decFechaVenc < decFechaOpe Then
                    bolResult = True
                End If
            End If
        End If
        Return bolResult
    End Function
    'CMB REQ 67089 20130319
    Private Function ValidarFixing(ByVal claseInstrumento As String, ByVal nemonico As String, ByVal fixing As Decimal) As Boolean
        Dim bolValidaFixing As Boolean = True
        Dim oOrdenInversionFormulas As New OrdenInversionFormulasBM()
        Dim dtCaracValorBono As DataTable
        Dim monedaPago As String = ""
        Dim valCodigoMoneda As String = ""
        If claseInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_BONO Then
            'Masivo
            'dtCaracValorBono = oOrdenInversionFormulas.SeleccionarCaracValor_Bonos(nemonico, ParametrosSIT.PORTAFOLIO_FONDO1, DatosRequest).Tables(0)
            valCodigoMoneda = CType(dtCaracValorBono.Rows(0)("val_CodigoMoneda"), String)
            monedaPago = CType(dtCaracValorBono.Rows(0)("MonedaPago"), String)
            If valCodigoMoneda <> monedaPago Then
                If fixing <> -1 Then
                    bolValidaFixing = True
                Else
                    bolValidaFixing = False
                End If
            End If
        End If
        Return bolValidaFixing
    End Function

    Private Sub btnValidarTrader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidarTrader.Click
        Try
            'VALIDACION DE EXCESOS POR TRADER
            Dim dtValidaTrader As New DataTable
            Dim oLimiteTradingBM As New LimiteTradingBM
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim decNProceso As Decimal = 0
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            Dim count As Decimal = 0
            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                    Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                    Dim decCodigoPrevOrden As Decimal
                    If chkSelect.Checked = True Then
                        If fila.Cells(2).Text = ParametrosSIT.PREV_OI_INGRESADO Then
                            decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                            oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                            count = count + 1
                        End If
                    End If
                End If
            Next
            If count > 0 Then
                dtValidaTrader = oLimiteTradingBM.SeleccionarValidacionExcesosTrader_Sura(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), Usuario, DatosRequest, , decNProceso).Tables(0)
                If dtValidaTrader.Rows.Count > 0 Then
                    Session("dtValidaTrader") = dtValidaTrader

                    Dim strURL As String = "frmValidacionExcesosTrader.aspx?TipoRenta=" & ParametrosSIT.TR_RENTA_FIJA.ToString() & "&nProc=" & decNProceso.ToString()
                    EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "');")

                Else
                    oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
                End If
            Else
                AlertaJS("Seleccione el registro a ejecutar")
                oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            End If
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
        Dim tbNemonico As TextBox
        Dim strCodigoOrden As String = ""
        Dim strPortafolioSBS As String = ""
        Dim PortafolioSBS As String = ""
        Dim strMoneda As String = ""    'HDG 20120130
        Dim strOperacion As String = "" 'HDG 20120130
        Dim strCodigoISIN As String = "" 'HDG 20120223
        Dim strCodigoSBS As String = "" 'HDG 20120223
        Dim chkSelect As CheckBox

        Try
            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)
                    If fila.Cells(2).Text = PREV_OI_EJECUTADO And chkSelect.Checked = True Then
                        lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                        lbClase = CType(fila.FindControl("lbClase"), Label)
                        tbNemonico = CType(fila.FindControl("tbNemonico"), TextBox)
                        ds = oPrevOrdenInversion.SeleccionarImprimir_PrevOrdenInversion(lbCodigoPrevOrden.Text, DatosRequest)
                        dtOI = ds.Tables(0)
                        For Each fila2 As DataRow In dtOI.Rows
                            strCodigoOrden = fila2("CodigoOrden")
                            strPortafolioSBS = fila2("PortafolioSBS")
                            PortafolioSBS = fila2("CodigoPortafolioSBS")
                            strMoneda = fila2("Moneda") 'HDG 20120130
                            strOperacion = fila2("Operacion")   'HDG 20120130
                            strCodigoISIN = fila2("CodigoISIN")   'HDG 20120223
                            strCodigoSBS = fila2("CodigoSBS")   'HDG 20120223
                            Session("dtdatosoperacion") = Nothing  'HDG 20120223
                            GenerarLlamado(strCodigoOrden, PortafolioSBS, strPortafolioSBS, lbClase.Text.ToUpper, strOperacion, strMoneda, strCodigoISIN, strCodigoSBS, tbNemonico.Text)  'HDG 20120223
                        Next
                    End If
                End If
            Next
            CrearMultiCartaPDF(rutas.ToString())    'HDG 20120228
        Catch ex As Exception   'HDG 20120228
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    'HDG 20120228
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal dscportafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        'Page.RegisterStartupScript(Guid.NewGuid().ToString(), UIUtility.MostrarPopUp_New("Llamado/VisorLlamado.aspx?vcodigo=" + codigo + "&vportafolio=" + portafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "No", "Yes", "Yes"))
        Dim strtitulo, strnemonico, strisin, strsbs, strsubtitulo, strcodigo1, strcodigo, strportafolio, strclase, strmoneda, stroperacion, strOperacion_, strmnemonicotemp As String
        Dim dscaract As New dscaracteristicas
        Dim dttempoperacion As DataTable
        Dim drcar As DataRow
        strclase = clase
        strnemonico = mnemonico
        strisin = isin
        strsbs = sbs
        strmnemonicotemp = mnemonico    'Request.QueryString("vnemonicotemp")
        strportafolio = portafolio
        strcodigo1 = codigo
        Select Case clase.ToUpper
            Case "FONDOS MUTUOS EN EL EXTERIOR"
                strclase = "ORDENES DE FONDO"
            Case "FONDOS DE INVERSIÓN"
                strclase = "ORDENES DE FONDO"
        End Select

        Dim dsValor As New DataSet
        Dim oOIFormulas As New OrdenInversionFormulasBM
        If strclase = "BONOS" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_Bonos(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_Bonos(strnemonico, strportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val13") = CType(drValor("val_CodigoISIN"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val14") = CType(drValor("val_CodigoSBS"), String)
                drcar("Car15") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val15") = CType(drValor("val_TipoRenta"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "ACCIONES" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_Acciones(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_Acciones(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val2") = CType(drValor("val_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car3") = CType(drValor("lbl_sigDivFecha"), String)
                drcar("Val3") = CType(drValor("val_sigDivFecha"), String)
                drcar("Car4") = CType(drValor("lbl_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val4") = CType(drValor("val_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car5") = CType(drValor("lbl_sigDivFactor"), String)
                drcar("Val5") = CType(drValor("val_sigDivFactor"), String)
                drcar("Car6") = CType(drValor("lbl_NroOperDiarProm"), String)
                drcar("Val6") = CType(drValor("val_NroOperDiarProm"), String)
                drcar("Car7") = CType(drValor("lbl_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("val_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val10") = CType(drValor("val_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")

                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "CERTIFICADO DE DEPÓSITO" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_CertificadoDeposito(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_CertificadoDeposito(strnemonico, strportafolio, strcodigo1)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = "" 'CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = "" 'CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = "" ' CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = "" 'CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")

                drcar("Car13") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val13") = CType(drValor("val_TipoRenta"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val14") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car15") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val15") = CType(drValor("val_CodigoSBS"), String)

                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "CERTIFICADO DE SUSCRIPCION" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_CertificadoSuscripcion(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_CertificadoSuscripcion(portafolio, strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val2") = CType(drValor("val_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car3") = CType(drValor("lbl_sigdivfecha"), String)
                drcar("Val3") = CType(drValor("val_sigdivfecha"), String)
                drcar("Car4") = CType(drValor("lbl_valorDFC"), String)
                drcar("Val4") = CType(drValor("val_valorDFC"), String)
                drcar("Car5") = CType(drValor("lbl_sigdivfactor"), String)
                drcar("Val5") = CType(drValor("val_sigdivfactor"), String)
                drcar("Car6") = CType(drValor("lbl_NroOperDiarProm"), String)
                drcar("Val6") = CType(drValor("val_NroOperDiarProm"), String)
                drcar("Car7") = CType(drValor("lbl_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_Priceearnings"), String)
                drcar("Val8") = CType(drValor("val_Priceearnings"), String)
                drcar("Car9") = CType(drValor("lbl_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val10") = CType(drValor("val_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                drcar("Car13") = ""
                drcar("Val13") = ""
                drcar("Car14") = ""
                drcar("Val14") = ""
                drcar("Car15") = ""
                drcar("Val15") = ""
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "PAPELES COMERCIALES" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_PapelesComerciales(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_PapelesComerciales(strnemonico, strportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val13") = CType(drValor("val_TipoRenta"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val14") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car15") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val15") = CType(drValor("val_CodigoISIN"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "PAGARES" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_Pagares(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_Pagares(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Emisor"), String)
                drcar("Val1") = CType(drValor("val_Emisor"), String)
                drcar("Car2") = CType(drValor("lbl_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val2") = CType(drValor("val_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car3") = CType(drValor("lbl_Serie"), String)
                drcar("Val3") = CType(drValor("val_Serie"), String)
                drcar("Car4") = CType(drValor("lbl_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val4") = CType(drValor("val_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car5") = CType(drValor("lbl_TasaCupon"), String)
                drcar("Val5") = CType(drValor("val_TasaCupon"), String)
                drcar("Car6") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val6") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car7") = CType(drValor("lbl_TipoCupon"), String)
                drcar("Val7") = CType(drValor("val_TipoCupon"), String)
                drcar("Car8") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val8") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car9") = CType(drValor("lbl_PeriodoPago"), String)
                drcar("Val9") = CType(drValor("val_PeriodoPago"), String)
                drcar("Car10") = CType(drValor("lbl_FechaVcto"), String)
                drcar("Val10") = CType(drValor("val_FechaVcto"), String)
                drcar("Car11") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")

                drcar("Car13") = CType(drValor("lbl_fc"), String)
                drcar("Val13") = CType(drValor("val_fc"), String)
                drcar("Car14") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val14") = CType(drValor("val_TipoRenta"), String)
                drcar("Car15") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val15") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "ORDENES DE FONDO" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_OrdenesFondo(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_OrdenesFondo(strnemonico, strportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")

                drcar("Car13") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val13") = CType(drValor("val_TipoRenta"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val14") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car15") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val15") = CType(drValor("val_CodigoISIN"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "DEPOSITOS A PLAZO" Then
            'RGF 20090703
            If Session("dtdatosoperacion") Is Nothing Then
                Dim dtOrden As DataTable = New OrdenPreOrdenInversionBM().ListarOrdenesInversionPorCodigoOrden(strcodigo1, strportafolio, DatosRequest, PORTAFOLIO_MULTIFONDOS).Tables(0)
                Session("dtdatosoperacion") = New DepositoPlazos().ObtenerDatosOperacion(DatosRequest, dtOrden.Rows(0))
            End If

        End If
        If strclase = "OPERACIONES DE REPORTE" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_OperacionesReporte(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_OperacionesReporte(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val2") = CType(drValor("val_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car3") = CType(drValor("lbl_sigDivFecha"), String)
                drcar("Val3") = CType(drValor("val_sigDivFecha"), String)
                drcar("Car4") = CType(drValor("lbl_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val4") = CType(drValor("val_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car5") = CType(drValor("lbl_sigDivFactor"), String)
                drcar("Val5") = CType(drValor("val_sigDivFactor"), String)
                drcar("Car6") = CType(drValor("lbl_NroOperDiarProm"), String)
                drcar("Val6") = CType(drValor("val_NroOperDiarProm"), String)
                drcar("Car7") = CType(drValor("lbl_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val10") = CType(drValor("val_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")

                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                drcar("Car13") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val13") = CType(drValor("val_CodigoISIN"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val14") = CType(drValor("val_CodigoSBS"), String)
                drcar("Car15") = CType(drValor("lbl_Mercado"), String)
                drcar("Val15") = CType(drValor("val_Mercado"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "INSTRUMENTOS COBERTURADOS" Then
            dsValor = oOIFormulas.SeleccionarCaracValor_InstCoberturados(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val1") = CType(drValor("val_TipoRenta"), String)
                drcar("Car2") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val2") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car3") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val3") = CType(drValor("val_CodigoISIN"), String)
                drcar("Car4") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val4") = CType(drValor("val_CodigoSBS"), String)
                drcar("Car5") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val5") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car6") = CType(drValor("lbl_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_Emisor"), String)
                drcar("Val10") = CType(drValor("val_Emisor"), String)
                drcar("Car11") = CType(drValor("lbl_fc"), String)
                drcar("Val11") = CType(drValor("val_fc"), String)
                drcar("Car12") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val12") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car13") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val13") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car14") = CType(drValor("lbl_FechaVcto"), String)
                drcar("Val14") = CType(drValor("val_FechaVcto"), String)
                drcar("Car15") = CType(drValor("lbl_observacion"), String)
                drcar("Val15") = CType(drValor("val_observacion"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                'drcar("Car16") = CType(drValor("lbl_Serie"), String)
                'drcar("Val16") = CType(drValor("val_Serie"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If

        If strclase = "LETRAS HIPOTECARIAS" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_LetrasHipotecarias(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_LetrasHipotecarias(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinLetra"), String)
                drcar("Val2") = CType(drValor("val_FechaFinLetra"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String)
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String)
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String)
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String)
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String)
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String)
                drcar("Car12") = CType(drValor("lbl_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val13") = CType(drValor("val_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car14") = CType(drValor("lbl_fc"), String)
                drcar("Val14") = CType(drValor("val_fc"), String)
                drcar("Car15") = CType(drValor("lbl_FechaVcto"), String)
                drcar("Val15") = CType(drValor("val_FechaVcto"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If

        If strclase = "INSTRUMENTOS ESTRUCTURADOS" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_InstrumentosEstructurados(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_InstEstructurados(strnemonico, strportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_nemo1"), String)
                drcar("Val1") = CType(drValor("val_nemo1"), String)
                drcar("Car2") = CType(drValor("lbl_nemo2"), String)
                drcar("Val2") = CType(drValor("val_nemo2"), String)
                drcar("Car3") = CType(drValor("lbl_nemo3"), String)
                drcar("Val3") = CType(drValor("val_nemo3"), String)
                drcar("Car4") = CType(drValor("lbl_porc1"), String)
                drcar("Val4") = CType(drValor("val_porc1"), String)
                drcar("Car5") = CType(drValor("lbl_porc2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val5") = CType(drValor("val_porc2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car6") = CType(drValor("lbl_porc3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_porc3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_precio1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_precio1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_precio2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_precio2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_precio3"), String)
                drcar("Val9") = CType(drValor("val_precio3"), String)
                drcar("Car10") = CType(drValor("lbl_porcParticip"), String)
                drcar("Val10") = CType(drValor("val_porcParticip"), String)
                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                drcar("Car13") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val13") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car14") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val14") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car15") = ""
                drcar("Val15") = ""
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If

        Dim sNNeg As String = ""
        If strclase.Substring(1) = "OPERACIONES DERIVADAS - FORWARD DIVISAS" Or strclase.Substring(1) = "COMPRA/VENTA MONEDA EXTRANJERA" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = Session("dtdatosoperacionSW" & strclase.Substring(0, 1))
            sNNeg = strclase.Substring(0, 1)
            strclase = strclase.Substring(1)
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
            If strclase.Equals("COMPRA/VENTA MONEDA EXTRANJERA") And stroperacion.ToLower.IndexOf("compra") >= 0 Then
                droper("v4") = dr("v6")
                droper("v6") = dr("v4")
            Else
                droper("v4") = dr("v4")
                droper("v6") = dr("v6")
            End If

            droper("c5") = dr("c5")
            droper("v5") = dr("v5")
            droper("c6") = dr("c6")
            'droper("v6") = dr("v6")
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
            If strclase = "OPERACIONES DERIVADAS - FORWARD DIVISAS" Or strclase.Substring(1) = "OPERACIONES DERIVADAS - FORWARD DIVISAS" Then
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
        Dim Archivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos\Inversiones\Llamado\RptLlamado.rpt"
        'Dim Archivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Llamado/RptLlamado.rpt"

        'Dim Archivo As String = "D:\Cespejo\Sourcesafe Integra\Integra\02. Fuentes\FuentesSIT_Integra - Ultimo 30-07-2014\SITSolution\IN-SIT\Modulos\Inversiones\Llamado\RptLlamado.rpt"
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
            '  If strportafolio <> "MULTIFONDO" Then

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

            Cro.SetParameterValue("@Ruta_Logo", System.Configuration.ConfigurationManager.AppSettings("RUTA_LOGO"))

            Cro.SetParameterValue("@Titulo", strtitulo)
            Cro.SetParameterValue("@Subtitulo", strsubtitulo)
            Cro.SetParameterValue("@Fondo", "Portafolio: " & dscportafolio)
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
        End Try

    End Sub

    'HDG 20120228
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

            'ini HDG 20130208
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
    'CMB OT 65473 20120917
    Private Sub GenerarReporteRentaFijaPDF()
        Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio("MULTIFONDO")
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM()
        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim fileReport As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Reportes/rptReporteOperacionesMasivasRF.rpt"
        oReport.Load(fileReport)
        Dim dsAux As DataSet
        Dim dsReporteOperacionesMasivasRF As New DsReporteOperacionesMasivasRF
        dsAux = oPrevOrdenInversion.GenerarReporteConFirmas(ParametrosSIT.TR_RENTA_FIJA, fechaNegocio, DatosRequest)
        CopiarTabla(dsAux.Tables(0), dsReporteOperacionesMasivasRF.RegistroPrevio)
        'Firmas
        Dim drFirma As DsReporteOperacionesMasivasRF.FirmaRow
        Dim drRutaFirma As DataRow
        drRutaFirma = dsAux.Tables(1).Rows(0)
        drFirma = CType(dsReporteOperacionesMasivasRF.Firma.NewFirmaRow(), DsReporteOperacionesMasivasRF.FirmaRow)
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
        dsReporteOperacionesMasivasRF.Firma.AddFirmaRow(drFirma)
        dsReporteOperacionesMasivasRF.Firma.AcceptChanges()
        dsReporteOperacionesMasivasRF.Merge(dsReporteOperacionesMasivasRF, False, System.Data.MissingSchemaAction.Ignore)
        oReport.SetDataSource(dsReporteOperacionesMasivasRF)
        oReport.SetParameterValue("@Usuario", Usuario)
        oReport.SetParameterValue("@FechaOperacion", UIUtility.ConvertirFechaaString(fechaNegocio))
        Dim rutaArchivo As String = ""
        If Not (oReport Is Nothing) Then
            rutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RF_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
            oReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
        End If
        ViewState("RutaReporte") = rutaArchivo
    End Sub

    'CMB OT 65473 20120924
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            Try
                dtDestino.LoadDataRow(dr.ItemArray, False)
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        Next
    End Sub
    Protected Sub Datagrid1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Datagrid1.PageIndexChanging
        Datagrid1.PageIndex = e.NewPageIndex
        CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
    End Sub
    Protected Sub Datagrid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Datagrid1.RowCommand
        Try
            Dim gvr As GridViewRow = Nothing
            Select Case e.CommandName
                Case "Footer", "Item", "Add", "Footer"
                    gvr = Datagrid1.FooterRow
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
            ElseIf e.CommandName = "Add" Then
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
                Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
                Dim tbNemonicoF As TextBox
                Dim ddlOperacionF As DropDownList
                Dim ddlIndiceF As DropDownList
                Dim ddlPlazaNF As DropDownList
                Dim ddlMedioNegF As DropDownList
                Dim ddlCondicionF As DropDownList
                Dim ddlTipoTasaF As DropDownList
                Dim tbFechaLiquidacionF As TextBox
                Dim hdIntermediarioF As HtmlControls.HtmlInputHidden
                Dim tbFixingF As TextBox 'CMB REQ 67089 20130319
                Dim ddlPortafolio As DropDownList
                Dim tbCantidadF As TextBox
                Dim tbPrecioF As TextBox
                Dim tbCantidadOperacionF As TextBox
                Dim tbPrecioOperacionF As TextBox
                Dim tbTasaF As TextBox
                Dim chkPorcentajeF As CheckBox
                Dim dtDetalleInversiones As DataTable
                Dim tbInstrumentoF As TextBox
                Dim porcentajeF As String = String.Empty
                Dim bolValidaCampos As Boolean = False
                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox)
                ddlOperacionF = CType(Datagrid1.FooterRow.FindControl("ddlOperacionF"), DropDownList)
                ddlIndiceF = CType(Datagrid1.FooterRow.FindControl("ddlIndiceF"), DropDownList)
                ddlPlazaNF = CType(Datagrid1.FooterRow.FindControl("ddlPlazaNF"), DropDownList)
                ddlMedioNegF = CType(Datagrid1.FooterRow.FindControl("ddlMedioNegF"), DropDownList)
                ddlCondicionF = CType(Datagrid1.FooterRow.FindControl("ddlCondicionF"), DropDownList)
                ddlTipoTasaF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTasaF"), DropDownList)
                tbFechaLiquidacionF = CType(Datagrid1.FooterRow.FindControl("tbFechaLiquidacionF"), TextBox)
                hdIntermediarioF = CType(Datagrid1.FooterRow.FindControl("hdIntermediarioF"), HtmlControls.HtmlInputHidden)
                tbCantidadF = CType(Datagrid1.FooterRow.FindControl("tbCantidadF"), TextBox)
                tbPrecioF = CType(Datagrid1.FooterRow.FindControl("tbPrecioF"), TextBox)
                tbCantidadOperacionF = CType(Datagrid1.FooterRow.FindControl("tbCantidadOperacionF"), TextBox)
                tbPrecioOperacionF = CType(Datagrid1.FooterRow.FindControl("tbPrecioOperacionF"), TextBox)
                tbTasaF = CType(Datagrid1.FooterRow.FindControl("tbTasaF"), TextBox)
                tbFixingF = CType(Datagrid1.FooterRow.FindControl("tbFixingF"), TextBox)
                tbInstrumentoF = CType(Datagrid1.FooterRow.FindControl("tbInstrumentoF"), TextBox)
                chkPorcentajeF = CType(Datagrid1.FooterRow.FindControl("chkPorcentajeF"), CheckBox)
                ddlPortafolio = CType(Datagrid1.FooterRow.FindControl("ddlfondosF"), DropDownList)
                porcentajeF = "N"
                Dim hdClaseInstrumentoF As HtmlControls.HtmlInputHidden
                Dim ddlTipoFondoF As DropDownList
                Dim ddlTipoTramoF As DropDownList
                hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlControls.HtmlInputHidden)
                ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                Dim objFormulasOI As New OrdenInversionFormulasBM
                Dim precioOperacion As Decimal
                Dim tasa As Decimal
                Dim totalOperacionRF As Decimal
                Dim precioEjecutado As Decimal
                Dim totalEjecutadoRF As Decimal
                Dim strMensaje As String = ""
                Dim fixing As Decimal = 0
                Dim validaFixing As Boolean = True
                Dim validaFechaVenc As Boolean
                Dim validaIntermediario As Boolean
                Dim validaNemonico As Boolean
                If tbFixingF.Text <> "" Then
                    fixing = CType(tbFixingF.Text, Decimal)
                    If fixing < 0 Then
                        fixing = -1
                    End If
                Else
                    fixing = -1
                End If
                If tbCantidadF.Text <> "" And _
                    tbNemonicoF.Text <> "" And _
                    hdIntermediarioF.Value <> "" And _
                    tbFechaLiquidacionF.Text <> "" And _
                    Not ((hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And ddlTipoFondoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) And _
                    Not (hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondoF.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And ddlTipoTramoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) Then  'HDG 20120517
                    If IsNumeric(tbCantidadF.Text) And _
                       IsDate(tbFechaLiquidacionF.Text) Then
                        validaNemonico = ValidarNemonico(tbNemonicoF.Text)
                        validaIntermediario = ValidarIntermediario(hdIntermediarioF.Value.ToString)
                        validaFechaVenc = ValidarFechaVencimiento(tbFechaLiquidacionF.Text, tbNemonicoF.Text)
                        validaFixing = ValidarFixing(hdClaseInstrumentoF.Value, tbNemonicoF.Text, fixing)
                        If validaNemonico = True And _
                            validaIntermediario = True And _
                            validaFechaVenc = True And _
                            validaFixing = True Then
                            bolValidaCampos = True
                        Else
                            If validaNemonico = False Then
                                strMensaje = strMensaje + "- El nemonico no pertenece a renta fija. \n"
                            End If
                            If validaIntermediario = False Then
                                strMensaje = strMensaje + "- El intermediario es incorrecto"
                            End If
                            If validaFechaVenc = False Then
                                strMensaje = strMensaje + "- La Fecha de liquidación es incorrecta."
                            End If
                            If validaFixing = False Then
                                strMensaje = strMensaje + "- Debe ingresar el Fixing. \n"
                            End If
                        End If
                    End If
                Else
                    If tbNemonicoF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Nemónico. \n"
                    End If
                    If tbCantidadF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Cant. Instrumento. \n"
                    End If
                    If hdIntermediarioF.Value.ToString() = "" Then
                        strMensaje = strMensaje + "- Ingrese Intermediario. \n"
                    End If
                    If tbFechaLiquidacionF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Fecha de liquidación. \n"
                    End If
                    If (hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And ddlTipoFondoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                        strMensaje = strMensaje + "- Seleccione Tipo. \n"
                    End If
                    If hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondoF.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And ddlTipoTramoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                        strMensaje = strMensaje + "- Seleccione Tipo Tramo. \n"
                    End If
                End If
                If bolValidaCampos = True Then
                    Dim Mensaje As String = ""
                    If (hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) Then  'HDG 20120517
                        precioOperacion = Val(tbPrecioF.Text)
                        totalOperacionRF = precioOperacion * CType(tbCantidadF.Text, Decimal)
                        precioEjecutado = Val(tbPrecioOperacionF.Text)
                        totalEjecutadoRF = precioEjecutado * CType(tbCantidadOperacionF.Text, Decimal)
                    Else
                        tasa = Val(tbTasaF.Text)
                        precioOperacion = Val(tbPrecioF.Text)
                        totalOperacionRF = OrdenInversion.CalculaMontoOperacion(tbNemonicoF.Text, tbCantidadF.Text, ddlIndiceF.SelectedValue, tbFechaLiquidacionF.Text, ddlTipoTasaF.SelectedValue, tbFechaOperacion.Text, precioOperacion, tasa, Mensaje, DatosRequest).ToString()
                        tasa = Val(tbTasaF.Text)
                        precioEjecutado = Val(tbPrecioOperacionF.Text)
                        totalEjecutadoRF = OrdenInversion.CalculaMontoOperacion(tbNemonicoF.Text, tbCantidadOperacionF.Text, ddlIndiceF.SelectedValue, tbFechaLiquidacionF.Text, ddlTipoTasaF.SelectedValue, tbFechaOperacion.Text, precioEjecutado, tasa, Mensaje, DatosRequest).ToString()
                        If Mensaje.Length > 0 Then
                            AlertaJS(Mensaje)
                            Exit Sub
                        End If
                    End If
                    oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                    oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                    oRow.FechaOperacion = ViewState("decFechaOperacion")
                    oRow.HoraOperacion = String.Format("{0:HH:mm}", Date.Now)
                    oRow.CodigoNemonico = tbNemonicoF.Text
                    oRow.CodigoOperacion = ddlOperacionF.SelectedValue
                    oRow.CodigoTercero = hdIntermediarioF.Value
                    oRow.CodigoPlaza = ddlPlazaNF.SelectedValue
                    oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                    oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO
                    oRow.MedioNegociacion = ddlMedioNegF.SelectedValue
                    oRow.TipoCondicion = ddlCondicionF.SelectedValue
                    oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacionF.Text)
                    oRow.TipoTasa = ddlTipoTasaF.SelectedValue
                    oRow.Tasa = tasa
                    oRow.Cantidad = CType(tbCantidadF.Text, Decimal)
                    oRow.Precio = CType(IIf(tbPrecioF.Text = "", -1, tbPrecioF.Text), Decimal)
                    oRow.MontoNominal = totalOperacionRF
                    oRow.CantidadOperacion = CType(tbCantidadOperacionF.Text, Decimal)
                    oRow.PrecioOperacion = precioEjecutado
                    oRow.MontoOperacion = totalEjecutadoRF
                    oRow.IndPrecioTasa = ddlIndiceF.SelectedValue
                    oRow.TipoFondo = IIf(ddlTipoFondoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoFondoF.SelectedValue)
                    oRow.TipoTramo = IIf(ddlTipoTramoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoTramoF.SelectedValue)
                    oRow.Porcentaje = porcentajeF
                    oRow.Fixing = fixing 'CMB INC 67089 20130319
                    oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                    oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                    oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                    oPrevOrdenInversionBM.Insertar(oPrevOrdenInversionBE, ParametrosSIT.TR_RENTA_FIJA.ToString(), DatosRequest, dtDetalleInversiones)
                    CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                Else
                    If strMensaje <> "" Then
                        AlertaJS(strMensaje)
                    Else
                        AlertaJS("Ingrese correctamente el registro")
                    End If
                    Exit Sub
                End If
                EjecutarJS("BorrarHiddens();")
            End If
            If e.CommandName = "TipoFondo" Then
                Dim ddlTipoFondo As DropDownList
                Dim ddlTipoTramo As DropDownList
                Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
                ddlTipoFondo = CType(gvr.FindControl("ddlTipoFondo"), DropDownList)
                ddlTipoTramo = CType(gvr.FindControl("ddlTipoTramo"), DropDownList)
                hdClaseInstrumento = CType(gvr.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                    If ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF Then
                        ddlTipoTramo.Enabled = True
                    Else
                        ddlTipoTramo.Enabled = False
                        ddlTipoTramo.SelectedIndex = 0
                    End If
                End If
            End If
            If e.CommandName = "_Delete" Then
                'ini HDG 20120117
                gvr = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim decCodigoPrevOrden As Decimal = CType(e.CommandArgument.ToString().Split("|")(0), Decimal)
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
                        ParametrosSIT.TR_RENTA_FIJA.ToString() & "&codigo=" & _
                        decCodigoPrevOrden.ToString(), "DetalleRegistroRV", 770, 480, 110, 55, "no", "no", "yes", "yes")
                        EjecutarJS(script, False)
                    End If
                ElseIf gvr.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                    'ini HDG OT 64291 20111201
                    oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                    CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                    'fin HDG OT 64291 20111201
                Else
                    oPrevOrdenInversionBM.Eliminar(decCodigoPrevOrden, DatosRequest)
                    CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                End If
                'fin HDG 20120117
            End If
            'ini HDG 20120517
            If e.CommandName = "Item" Then
                Dim tbNemonico As TextBox
                Dim hdNemonico As HtmlControls.HtmlInputHidden
                hdNemonico = CType(row.FindControl("hdNemonico"), HtmlControls.HtmlInputHidden)
                tbNemonico = CType(row.FindControl("tbNemonico"), TextBox)
                If hdNemonico.Value <> "" Then
                    tbNemonico.Text = hdNemonico.Value
                End If
                Dim hdDescTercero As HtmlControls.HtmlInputHidden
                Dim tbIntermediario As TextBox
                hdDescTercero = CType(row.FindControl("hdDescTercero"), HtmlControls.HtmlInputHidden)
                tbIntermediario = CType(row.FindControl("tbIntermediario"), TextBox)
                If hdDescTercero.Value <> "" Then
                    tbIntermediario.Text = hdDescTercero.Value
                End If
                'fin Migracion CMB 20120809
                tbNemonico = CType(row.FindControl("tbNemonico"), TextBox)
                If tbNemonico.Text.Trim <> "" Then
                    Dim ddlTipoFondo As DropDownList
                    Dim ddlTipoTramo As DropDownList
                    Dim tbInstrumento As TextBox
                    Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
                    tbInstrumento = CType(row.FindControl("tbInstrumento"), TextBox)
                    hdClaseInstrumento = CType(row.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                    If hdClaseInstrumento.Value.Trim = "" Then
                        hdClaseInstrumento.Value = tbInstrumento.Text.Trim.Substring(0, 2)
                        tbInstrumento.Text = tbInstrumento.Text.Trim.Substring(2)
                    End If
                    ddlTipoFondo = CType(row.FindControl("ddlTipoFondo"), DropDownList)
                    ddlTipoTramo = CType(row.FindControl("ddlTipoTramo"), DropDownList)
                    If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION Then
                        ddlTipoFondo.Enabled = True
                        If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                            ddlTipoTramo.Enabled = True
                        Else
                            ddlTipoTramo.Enabled = False
                            ddlTipoTramo.SelectedIndex = 0
                        End If
                    Else
                        ddlTipoFondo.Enabled = False
                        ddlTipoTramo.Enabled = False
                        ddlTipoFondo.SelectedIndex = 0
                        ddlTipoTramo.SelectedIndex = 0
                    End If
                End If
            End If
            If e.CommandName = "TipoFondo" Then
                Dim ddlTipoFondo As DropDownList
                Dim ddlTipoTramo As DropDownList
                Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden

                ddlTipoFondo = CType(row.FindControl("ddlTipoFondo"), DropDownList)
                ddlTipoTramo = CType(row.FindControl("ddlTipoTramo"), DropDownList)
                hdClaseInstrumento = CType(row.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                    If ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF Then
                        ddlTipoTramo.Enabled = True
                    Else
                        ddlTipoTramo.Enabled = False
                        ddlTipoTramo.SelectedIndex = 0
                    End If
                End If
            End If
            If e.CommandName = "Footer" Then
                Dim oValoresBM As New ValoresBM
                Dim oValorBE As ValoresBE
                Dim ddlTipoTasaF As DropDownList
                Dim strTipoCupon As String

                Dim tbNemonicoF As TextBox
                Dim tbIntermediarioF As TextBox

                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbOperadorF"), TextBox)
                ddlTipoTasaF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTasaF"), DropDownList)
                oValorBE = oValoresBM.Seleccionar(tbNemonicoF.Text, DatosRequest)

                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox)
                tbIntermediarioF = CType(Datagrid1.FooterRow.FindControl("tbIntermediarioF"), TextBox)

                tbNemonicoF.Text = hdnOperador.Value
                '   tbIntermediarioF.Text = hdnIntermediario.Value

                If oValorBE.Tables(0).Rows.Count > 0 Then
                    strTipoCupon = oValorBE.Tables(0).Rows(0)("CodigoTipoCupon")
                    If strTipoCupon = "1" Or strTipoCupon = "2" Then
                        ddlTipoTasaF.SelectedValue = "1"
                    ElseIf strTipoCupon = "4" Or strTipoCupon = "5" Then
                        ddlTipoTasaF.SelectedValue = "2"
                    End If
                End If
                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox)
                If tbNemonicoF.Text.Trim <> "" Then
                    Dim ddlTipoFondoF As DropDownList
                    Dim ddlTipoTramoF As DropDownList
                    Dim tbInstrumentoF As TextBox
                    Dim hdClaseInstrumentoF As HtmlControls.HtmlInputHidden
                    tbInstrumentoF = CType(Datagrid1.FooterRow.FindControl("tbInstrumentoF"), TextBox)
                    hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlControls.HtmlInputHidden)
                    If hdClaseInstrumentoF.Value.Trim = "" Then
                        hdClaseInstrumentoF.Value = tbInstrumentoF.Text.Trim.Substring(0, 2)
                        tbInstrumentoF.Text = tbInstrumentoF.Text.Trim.Substring(2)
                    End If
                    ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                    ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                    If hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION Then
                        ddlTipoFondoF.Enabled = True
                        If hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                            ddlTipoTramoF.Enabled = True
                        Else
                            ddlTipoTramoF.Enabled = False
                            ddlTipoTramoF.SelectedIndex = 0
                        End If
                    Else
                        ddlTipoFondoF.Enabled = False
                        ddlTipoTramoF.Enabled = False
                        ddlTipoFondoF.SelectedIndex = 0
                        ddlTipoTramoF.SelectedIndex = 0
                    End If
                End If
            End If
            If e.CommandName = "TipoFondoF" Then
                Dim ddlTipoFondoF As DropDownList
                Dim ddlTipoTramoF As DropDownList
                Dim hdClaseInstrumentoF As HtmlControls.HtmlInputHidden

                ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlControls.HtmlInputHidden)
                If hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                    If ddlTipoFondoF.SelectedValue = ParametrosSIT.TIPOFONDO_ETF Then
                        ddlTipoTramoF.Enabled = True
                    Else
                        ddlTipoTramoF.Enabled = False
                        ddlTipoTramoF.SelectedIndex = 0
                    End If
                End If
            End If
            If e.CommandName = "Condicion" Then
                Dim ddlCondicion As DropDownList
                Dim ddlTipoFondo As DropDownList
                Dim ddlTipoTramo As DropDownList
                Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden

                ddlCondicion = CType(row.FindControl("ddlCondicion"), DropDownList)
                ddlTipoFondo = CType(row.FindControl("ddlTipoFondo"), DropDownList)
                ddlTipoTramo = CType(row.FindControl("ddlTipoTramo"), DropDownList)
                hdClaseInstrumento = CType(row.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
                If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                    ddlTipoTramo.Enabled = True
                    ddlTipoFondo.SelectedValue = TIPOFONDO_ETF
                    If ddlCondicion.SelectedValue = PRINCIPAL_TRADE Then
                        ddlTipoTramo.SelectedValue = TIPO_TRAMO_PRINCIPAL
                    Else
                        ddlTipoTramo.SelectedValue = TIPO_TRAMO_AGENCIA
                    End If
                End If
            End If
            If e.CommandName = "asignarfondoF" Then
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
            End If
            If e.CommandName = "CondicionF" Then
                Dim ddlCondicionF As DropDownList
                Dim ddlTipoFondoF As DropDownList
                Dim ddlTipoTramoF As DropDownList
                Dim hdClaseInstrumentoF As HtmlControls.HtmlInputHidden

                ddlCondicionF = CType(Datagrid1.FooterRow.FindControl("ddlCondicionF"), DropDownList)
                ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlControls.HtmlInputHidden)
                If hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                    ddlTipoTramoF.Enabled = True
                    ddlTipoFondoF.SelectedValue = TIPOFONDO_ETF
                    If ddlCondicionF.SelectedValue = PRINCIPAL_TRADE Then
                        ddlTipoTramoF.SelectedValue = TIPO_TRAMO_PRINCIPAL
                    Else
                        ddlTipoTramoF.SelectedValue = TIPO_TRAMO_AGENCIA
                    End If
                End If
            End If
            'fin HDG 20120517
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub Datagrid1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Datagrid1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ddlOperacion As DropDownList
            Dim ddlPlazaN As DropDownList
            Dim ddlMedioNeg As DropDownList
            Dim ddlCondicion As DropDownList
            Dim ddlTipoTasa As DropDownList
            Dim ddlIndice As DropDownList
            Dim ddlFondos As DropDownList
            Dim ibBNemonico As LinkButton
            Dim ibBIntermediario As LinkButton
            Dim lbOperacion As Label
            Dim lbPlazaN As Label
            Dim lbMedioNeg As Label
            Dim lbCondicion As Label
            Dim lbTipoTasa As Label
            Dim lbIndice As Label
            Dim tbPrecio As TextBox
            Dim tbTasa As TextBox
            Dim ddlTipoFondo As DropDownList
            Dim ddlTipoTramo As DropDownList
            Dim lbTipoFondo As Label
            Dim lbTipoTramo As Label
            Dim hdClaseInstrumento As HtmlControls.HtmlInputHidden
            Dim hdIntermediario As HtmlControls.HtmlInputHidden
            Dim hdOperacionTrz As HtmlControls.HtmlInputHidden  'HDG OT 67627 20130531

            hdIntermediario = CType(e.Row.FindControl("hdIntermediario"), HtmlControls.HtmlInputHidden)
            If hdIntermediario.Value.Trim = "55555" Then    'DATATEC
                e.Row.BackColor = System.Drawing.Color.Red
            End If

            ibBNemonico = CType(e.Row.FindControl("ibBNemonico"), LinkButton) '#Miguel
            'ibBNemonico.Attributes.Add("onclick", "javascript:showPopupMnemonicoGrilla(this);")

            lbOperacion = CType(e.Row.FindControl("lbOperacion"), Label)
            ddlOperacion = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
            HelpCombo.LlenarComboBox(ddlOperacion, CType(Session("dtOperacion"), DataTable), "codigoOperacion", "Descripcion", False)
            ddlOperacion.SelectedValue = lbOperacion.Text
            hdOperacionTrz = CType(e.Row.FindControl("hdOperacionTrz"), HtmlControls.HtmlInputHidden)    'HDG OT 67627 20130531
            hdOperacionTrz.Value = ddlOperacion.SelectedItem.Text   'HDG OT 67627 20130531

            ibBIntermediario = CType(e.Row.FindControl("ibBIntermediario"), LinkButton)
            'ibBIntermediario.Attributes.Add("onclick", "javascript:ShowPopupTercerosGrilla(this);")

            lbMedioNeg = CType(e.Row.FindControl("lbMedioNeg"), Label)
            ddlMedioNeg = CType(e.Row.FindControl("ddlMedioNeg"), DropDownList)
            HelpCombo.LlenarComboBox(ddlMedioNeg, CType(Session("dtMedioNeg"), DataTable), "Valor", "Nombre", False)
            ddlMedioNeg.SelectedValue = lbMedioNeg.Text

            lbCondicion = CType(e.Row.FindControl("lbCondicion"), Label)
            ddlCondicion = CType(e.Row.FindControl("ddlCondicion"), DropDownList)
            HelpCombo.LlenarComboBox(ddlCondicion, CType(Session("dtCondicion"), DataTable), "Valor", "Nombre", False)
            ddlCondicion.SelectedValue = lbCondicion.Text
            ddlCondicion.Attributes.Add("onchange", "javascript:HabilitaCondicion(this);")

            lbTipoTasa = CType(e.Row.FindControl("lbTipoTasa"), Label)
            ddlTipoTasa = CType(e.Row.FindControl("ddlTipoTasa"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoTasa, CType(Session("dtTipoTasa"), DataTable), "Valor", "Nombre", False)
            ddlTipoTasa.SelectedValue = IIf(lbTipoTasa.Text = "", "2", lbTipoTasa.Text)

            lbPlazaN = CType(e.Row.FindControl("lbPlazaN"), Label)
            ddlPlazaN = CType(e.Row.FindControl("ddlPlazaN"), DropDownList)
            HelpCombo.LlenarComboBox(ddlPlazaN, CType(Session("dtPlazaN"), DataTable), "CodigoPlaza", "Descripcion", False)
            ddlPlazaN.SelectedValue = lbPlazaN.Text

            tbPrecio = CType(e.Row.FindControl("tbPrecio"), TextBox)
            tbTasa = CType(e.Row.FindControl("tbTasa"), TextBox)

            lbIndice = CType(e.Row.FindControl("lbIndice"), Label)
            ddlIndice = CType(e.Row.FindControl("ddlIndice"), DropDownList)
            HelpCombo.LlenarComboBox(ddlIndice, CType(Session("dtIndice"), DataTable), "Valor", "Nombre", False)

            ddlIndice.SelectedValue = lbIndice.Text 'RGF 20110405

            lbTipoFondo = CType(e.Row.FindControl("lbTipoFondo"), Label)
            ddlTipoFondo = CType(e.Row.FindControl("ddlTipoFondo"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoFondo, CType(Session("dtTipoFondo"), DataTable), "Valor", "Nombre", False)
            ddlTipoFondo.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
            ddlTipoFondo.SelectedValue = IIf(lbTipoFondo.Text.Trim = "", ParametrosSIT.DDL_ITEM_SELECCIONE, lbTipoFondo.Text)

            lbTipoTramo = CType(e.Row.FindControl("lbTipoTramo"), Label)
            ddlTipoTramo = CType(e.Row.FindControl("ddlTipoTramo"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoTramo, CType(Session("dtTipoTramo"), DataTable), "Valor", "Nombre", False)
            ddlTipoTramo.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
            ddlTipoTramo.SelectedValue = IIf(lbTipoTramo.Text.Trim = "", ParametrosSIT.DDL_ITEM_SELECCIONE, lbTipoTramo.Text)

            ddlFondos = CType(e.Row.FindControl("ddlfondos"), DropDownList)
            Dim fondos As String() = CType(e.Row.FindControl("hdFondo1Trz"), HtmlControls.HtmlInputHidden).Value.Split("/")
            For Each fondo As String In fondos
                ddlFondos.Items.Add(fondo)
            Next
            ddlTipoFondo.Attributes.Add("onchange", "javascript:HabilitaTipoTramo(this);")
            hdClaseInstrumento = CType(e.Row.FindControl("hdClaseInstrumento"), HtmlControls.HtmlInputHidden)
            If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                ddlTipoFondo.Enabled = True
                If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF Then
                    ddlTipoTramo.Enabled = True
                End If
            End If

            ddlFondos = CType(e.Row.FindControl("ddlfondos"), DropDownList)
            CargaPortafolio(ddlFondos)
            Dim hdPortafolioSel As HtmlControls.HtmlInputHidden = CType(e.Row.FindControl("hdPortafolioSel"), HtmlControls.HtmlInputHidden)
            ddlFondos.SelectedValue = hdPortafolioSel.Value

            If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or _
                e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or _
                e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Or _
                e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then

                ddlCondicion.Enabled = False
                ddlOperacion.Enabled = False
                ddlPlazaN.Enabled = False
                ddlTipoTasa.Enabled = False
                ddlMedioNeg.Enabled = False
                ddlIndice.Enabled = False
                ibBNemonico.Enabled = False
                ibBIntermediario.Enabled = False
                ddlTipoFondo.Enabled = False
                ddlTipoTramo.Enabled = False

                Dim tbHora As TextBox
                Dim tbCantidad As TextBox
                Dim tbTotal As TextBox
                Dim tbFechaLiquidacion As TextBox
                Dim tbIntermediario As TextBox
                Dim tbNemonico As TextBox
                Dim chkSelect As CheckBox
                Dim Imagebutton1 As ImageButton

                Imagebutton1 = CType(e.Row.FindControl("Imagebutton1"), ImageButton)
                tbHora = CType(e.Row.FindControl("tbHora"), TextBox)
                tbCantidad = CType(e.Row.FindControl("tbCantidad"), TextBox)
                tbTotal = CType(e.Row.FindControl("tbTotal"), TextBox)
                tbFechaLiquidacion = CType(e.Row.FindControl("tbFechaLiquidacion"), TextBox)
                tbPrecio = CType(e.Row.FindControl("tbPrecio"), TextBox)
                tbIntermediario = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                tbNemonico = CType(e.Row.FindControl("tbNemonico"), TextBox)
                chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)

                tbCantidad.Enabled = False
                tbPrecio.Enabled = False
                tbTasa.Enabled = False
                tbTotal.Enabled = False
                tbFechaLiquidacion.Enabled = False
                tbIntermediario.Enabled = False
                tbNemonico.Enabled = False
                chkSelect.Enabled = False
                Imagebutton1.Enabled = False

                If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or _
                    e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or _
                    e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Then
                    chkSelect.Enabled = True
                End If
                'ini HDG 20120117
                If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                    Imagebutton1.Enabled = True
                End If

                Dim FechaVal As HtmlControls.HtmlGenericControl
                FechaVal = CType(e.Row.FindControl("FechaVal"), HtmlControls.HtmlGenericControl)
                FechaVal.Attributes.Add("class", "input-append")
                'fin HDG 20120117
            Else
                ibBIntermediario.Attributes.Add("onclick", "javascript:ShowPopupTercerosGrilla(this);")
                ibBNemonico.Attributes.Add("onclick", "javascript:showPopupMnemonicoGrilla(this);")


            End If
            Dim tbHora2 As TextBox
            Dim tbNemonico2 As TextBox
            Dim ddlOperacion2 As DropDownList
            Dim tbCantidad2 As TextBox
            Dim ddlIndice2 As DropDownList
            Dim tbPrecio2 As TextBox
            Dim ddlTipoTasa2 As DropDownList
            Dim tbTasa2 As TextBox
            Dim ddlPlazaN2 As DropDownList
            Dim ddlCondicion2 As DropDownList
            Dim tbIntermediario2 As TextBox
            Dim ddlMedioNeg2 As DropDownList
            Dim tbFechaLiquidacion2 As TextBox
            Dim tbCantidadOperacion2 As TextBox
            Dim tbPrecioOperacion2 As TextBox
            Dim tbTotalOperacion2 As TextBox
            'Dim tbFondo12 As TextBox
            '  Dim tbFondo22 As TextBox
            '  Dim tbFondo32 As TextBox
            Dim ddlTipoTramo2 As DropDownList    'HDG 20120517
            Dim tbFixing As TextBox 'CMB REQ 67089 20130319
            Dim chkPorcentaje2 As CheckBox
            Dim hdPorcentaje2 As HtmlControls.HtmlInputHidden

            tbHora2 = CType(e.Row.FindControl("tbHora"), TextBox)
            tbNemonico2 = CType(e.Row.FindControl("tbNemonico"), TextBox)
            ddlOperacion2 = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
            tbCantidad2 = CType(e.Row.FindControl("tbCantidad"), TextBox)
            ddlIndice2 = CType(e.Row.FindControl("ddlIndice"), DropDownList)
            tbPrecio2 = CType(e.Row.FindControl("tbPrecio"), TextBox)
            ddlTipoTasa2 = CType(e.Row.FindControl("ddlTipoTasa"), DropDownList)
            tbTasa2 = CType(e.Row.FindControl("tbTasa"), TextBox)
            ddlPlazaN2 = CType(e.Row.FindControl("ddlPlazaN"), DropDownList)
            ddlCondicion2 = CType(e.Row.FindControl("ddlCondicion"), DropDownList)
            tbIntermediario2 = CType(e.Row.FindControl("tbIntermediario"), TextBox)
            ddlMedioNeg2 = CType(e.Row.FindControl("ddlMedioNeg"), DropDownList)
            tbFechaLiquidacion2 = CType(e.Row.FindControl("tbFechaLiquidacion"), TextBox)
            tbCantidadOperacion2 = CType(e.Row.FindControl("tbCantidadOperacion"), TextBox)
            tbPrecioOperacion2 = CType(e.Row.FindControl("tbPrecioOperacion"), TextBox)
            tbTotalOperacion2 = CType(e.Row.FindControl("tbTotalOperacion"), TextBox)
            tbFixing = CType(e.Row.FindControl("tbFixing"), TextBox)
            ddlTipoTramo2 = CType(e.Row.FindControl("ddlTipoTramo"), DropDownList)
            hdPorcentaje2 = CType(e.Row.FindControl("hdPorcentaje"), HtmlControls.HtmlInputHidden)
            chkPorcentaje2 = CType(e.Row.FindControl("chkPorcentaje"), CheckBox)
            If hdPorcentaje2.Value = "S" Then
                chkPorcentaje2.Checked = True
            Else
                chkPorcentaje2.Checked = False
            End If
            tbHora2.Attributes.Add("onchange", "cambio(this);")
            tbNemonico2.Attributes.Add("onpropertychange", "cambio(this);")
            ddlOperacion2.Attributes.Add("onchange", "cambio(this);")
            tbCantidad2.Attributes.Add("onchange", "cambio(this);")
            ddlIndice2.Attributes.Add("onchange", "cambio(this);")
            tbPrecio2.Attributes.Add("onchange", "cambio(this);")
            ddlTipoTasa2.Attributes.Add("onchange", "cambio(this);")
            tbTasa2.Attributes.Add("onchange", "cambio(this);")
            ddlPlazaN2.Attributes.Add("onchange", "cambio(this);")
            ddlCondicion2.Attributes.Add("onchange", "cambio(this);")
            tbIntermediario2.Attributes.Add("onpropertychange", "cambio(this);")
            ddlMedioNeg2.Attributes.Add("onchange", "cambio(this);")
            tbFechaLiquidacion2.Attributes.Add("onchange", "cambio(this);")
            tbCantidadOperacion2.Attributes.Add("onchange", "cambio(this);")
            tbPrecioOperacion2.Attributes.Add("onchange", "cambio(this);")
            tbTotalOperacion2.Attributes.Add("onchange", "cambio(this);")
            ddlTipoTramo2.Attributes.Add("onchange", "cambio(this);")
            tbFixing.Attributes.Add("onchange", "cambio(this);")
            chkPorcentaje2.Attributes.Add("onchange", "cambio(this);")
        End If
        If e.Row.RowType = DataControlRowType.Footer Then
            Dim ddlCondicionF As DropDownList
            Dim ddlOperacionF As DropDownList
            Dim ddlMedioNegF As DropDownList
            Dim ddlPlazaNF As DropDownList
            Dim ddlTipoTasaF As DropDownList
            Dim ddlIndiceF As DropDownList
            'ini HDG 20120517
            Dim ddlTipoFondoF As DropDownList
            Dim ddlTipoTramoF As DropDownList
            'fin HDG 20120517
            Dim ddlfondos As DropDownList

            Dim tbOperadorF As TextBox
            Dim ibBNemonicoF As LinkButton
            Dim ibBIntermediarioF As LinkButton

            tbOperadorF = CType(e.Row.FindControl("tbOperadorF"), TextBox)
            tbOperadorF.Text = Usuario.ToString.Trim

            ibBNemonicoF = CType(e.Row.FindControl("ibBNemonicoF"), LinkButton)
            'ibBNemonicoF.Attributes.Add("onclick", "javascript:showPopupMnemonicoGrillaF(this);")

            ddlOperacionF = CType(e.Row.FindControl("ddlOperacionF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlOperacionF, CType(Session("dtOperacion"), DataTable), "codigoOperacion", "Descripcion", False)
            ddlOperacionF.SelectedIndex = 0

            ibBIntermediarioF = CType(e.Row.FindControl("ibBIntermediarioF"), LinkButton)
            'ibBIntermediarioF.Attributes.Add("onclick", "javascript:ShowPopupTercerosGrillaF(this);")

            ddlMedioNegF = CType(e.Row.FindControl("ddlMedioNegF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlMedioNegF, CType(Session("dtMedioNeg"), DataTable), "Valor", "Nombre", False)
            ddlMedioNegF.SelectedIndex = 0

            ddlCondicionF = CType(e.Row.FindControl("ddlCondicionF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlCondicionF, CType(Session("dtCondicion"), DataTable), "Valor", "Nombre", False)
            ddlCondicionF.SelectedIndex = 0
            ddlCondicionF.Attributes.Add("onchange", "javascript:HabilitaCondicionF(this);")

            ddlPlazaNF = CType(e.Row.FindControl("ddlPlazaNF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlPlazaNF, CType(Session("dtPlazaN"), DataTable), "CodigoPlaza", "Descripcion", False)
            ddlPlazaNF.SelectedIndex = 0

            ddlTipoTasaF = CType(e.Row.FindControl("ddlTipoTasaF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoTasaF, CType(Session("dtTipoTasa"), DataTable), "Valor", "Nombre", False)
            ddlTipoTasaF.SelectedIndex = 0

            ddlIndiceF = CType(e.Row.FindControl("ddlIndiceF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlIndiceF, CType(Session("dtIndice"), DataTable), "Valor", "Nombre", False)
            ddlIndiceF.SelectedIndex = 0

            'ini HDG 20120517
            ddlTipoFondoF = CType(e.Row.FindControl("ddlTipoFondoF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoFondoF, CType(Session("dtTipoFondo"), DataTable), "Valor", "Nombre", False)
            ddlTipoFondoF.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
            ddlTipoFondoF.SelectedValue = ParametrosSIT.TIPOFONDO_ETF

            ddlTipoTramoF = CType(e.Row.FindControl("ddlTipoTramoF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoTramoF, CType(Session("dtTipoTramo"), DataTable), "Valor", "Nombre", False)
            ddlTipoTramoF.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
            ddlTipoTramoF.SelectedValue = ParametrosSIT.TIPO_TRAMO_PRINCIPAL

            ddlTipoFondoF.Attributes.Add("onchange", "javascript:HabilitaTipoTramoF(this);")
            'fin HDG 20120517

            ddlfondos = CType(e.Row.FindControl("ddlfondosF"), DropDownList)
            CargaPortafolio(ddlfondos)

        End If
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
                Try
                    Try
                        bolGeneraOrden = CType(ds.Tables(1).Rows(0)("GeneraOrden"), Boolean)
                    Catch ex As Exception
                        bolGeneraOrden = CType(ds.Tables(2).Rows(0)("GeneraOrden"), Boolean)
                    End Try
                Catch ex As Exception
                    bolGeneraOrden = CType(ds.Tables(3).Rows(0)("GeneraOrden"), Boolean)
                End Try

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
                        'EjecutarJS(UIUtility.MostrarPopUp("frmValidacionExcesosPrevOI.aspx?Tipo=OI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento, "ConsultaOIGeneradas", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False) 'JHC REQ 66056: Implementacion Futuros
                    End If
                Else
                    If dtOrdenInversion.Rows.Count > 0 Then
                        Session("dtListaExcesos") = dtOrdenInversion
                        'EjecutarJS(UIUtility.MostrarPopUp("frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento, "ConsultaOIGeneradas", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + "', '1000', '500','" & btnBuscar.ClientID & "');")
                    End If
                End If
            End If
        End If
        If bolGeneraOrden = False Then
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
        End If
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

    Private Function row() As Object
        Throw New NotImplementedException
    End Function

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        If Not Page.IsPostBack Then
            CargarPagina()
        End If
        If Not Session("SS_DatosModal") Is Nothing Then
            Select Case _PopUp.Value
                Case "B"
                    tbCodigoMnemonico.Text = CType(Session("SS_DatosModal"), String())(0)
                Case "GM"
                    CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("tbNemonico"), TextBox).Text = CType(Session("SS_DatosModal"), String())(0)
                    CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("tbInstrumento"), TextBox).Text = CType(Session("SS_DatosModal"), String())(2)
                    CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("hdClaseInstrumento"), HtmlInputHidden).Value = String.Empty
                    CType(Datagrid1.Rows(CInt(_RowIndex.Value) - 2).FindControl("hdNemonico"), HtmlInputHidden).Value = CType(Session("SS_DatosModal"), String())(0)
                Case "FM"
                    CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox).Text = CType(Session("SS_DatosModal"), String())(0)
                    Dim CatInst As String = CType(Session("SS_DatosModal"), String())(2)
                    CType(Datagrid1.FooterRow.FindControl("tbInstrumentoF"), TextBox).Text = Right(CatInst, CatInst.Length - 2)
                    CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlInputHidden).Value = CatInst.Trim.Substring(0, 2)
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
    End Sub
End Class

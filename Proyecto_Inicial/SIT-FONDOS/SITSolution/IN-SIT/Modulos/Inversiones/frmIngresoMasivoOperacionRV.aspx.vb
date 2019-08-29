Imports System.Runtime.InteropServices.Marshal
Imports ParametrosSIT
Imports System.Data
Imports System.Drawing
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.IO
Imports System.Threading
Imports System.Globalization
Imports Application = Excel.Application
Imports System.Web.UI.WebControls
Partial Class Modulos_Inversiones_frmIngresoMasivoOperacionRV
    Inherits BasePage
#Region "Rutinas"
    Dim oTrazabilidadOperacionBE As New TrazabilidadOperacionBE
    Dim oRowT As TrazabilidadOperacionBE.TrazabilidadOperacionRow
    Dim strMensaje As String = "", NroOper As String = ""
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se ordeno los controles
    Dim ddlTipoFondo As DropDownList, ddlTipoTramo As DropDownList, tbHora As TextBox, hdClaseInstrumento As HtmlInputHidden, hdClaseInstrumentoF As HtmlInputHidden,
    ddlTipoFondoF As DropDownList, ddlTipoTramoF As DropDownList, ddlContactoF As DropDownList, tbHoraF As TextBox, ddlMedioTransF As DropDownList,
    hdIntermediarioF As HtmlInputHidden, tbNemonicoF As TextBox, tbIntermediarioF As TextBox, ddlCondicion As DropDownList, hdTotal As HtmlInputHidden, tbTotal As TextBox,
    ddlOperacionF As DropDownList, tbCantidadF As TextBox, tbPrecioF As TextBox, ddlCondicionF As DropDownList, ddlPlazaNF As DropDownList, tbCantidadOperacionF As TextBox,
    tbPrecioOperacionF As TextBox, chkPorcentajeF As CheckBox, porcentajeF As String, dtDetalleInversiones As Data.DataTable, tbOperadorF As TextBox,
    hdTotalF As HtmlInputHidden, tbTotalF As TextBox, hdIntermediario As HtmlInputHidden, tbNemonico As TextBox, hdNemonico As HtmlInputHidden,
    hdDescTercero As HtmlInputHidden, tbIntermediario As TextBox, tbCantidad As TextBox, tbPrecio As TextBox, tbCantidadOperacion As TextBox, tbPrecioOperacion As TextBox,
    tbTotalOperacion As TextBox, chkSelect As CheckBox, chkPorcentaje As CheckBox, hdPorcentaje As HtmlInputHidden, lbCodigoPrevOrden As Label,
    ddlPlazaN As DropDownList, porcentaje As String, ddlContacto As DropDownList, ddlMedioTrans As DropDownList, hdCambio As HtmlInputHidden, hdCambioTraza As HtmlInputHidden,
    hdCambioTrazaFondo As HtmlInputHidden, hdNemonicoTrz As HtmlInputHidden, hdOperacionTrz As HtmlInputHidden, hdCantidadTrz As HtmlInputHidden,
    hdPrecioTrz As HtmlInputHidden, hdIntermediarioTrz As HtmlInputHidden, hdCantidadOperacionTrz As HtmlInputHidden, hdPrecioOperacionTrz As HtmlInputHidden,
    hdFondo1Trz As HtmlInputHidden, hdFondo2Trz As HtmlInputHidden, hdFondo3Trz As HtmlInputHidden, ddlOperacion As DropDownList, lbTipoCondicion2 As Label,
    lbOperacion As Label, lbPlazaN As Label, ddlfondos As DropDownList, lbContacto As Label, lbTipoFondo As Label, lbTipoTramo As Label, lbMedioTrans As Label,
    tbHora2 As TextBox, tbNemonico2 As TextBox, ddlOperacion2 As DropDownList, tbCantidad2 As TextBox, tbPrecio2 As TextBox, tbTotal2 As TextBox, tbIntermediario2 As TextBox,
    ddlContacto2 As DropDownList, ddlMedioTrans2 As DropDownList, ddlTipoTramo2 As DropDownList, ddlPlazaN2 As DropDownList, tbCantidadOperacion2 As TextBox,
    tbPrecioOperacion2 As TextBox, tbTotalOperacion2 As TextBox, chkPorcentaje2 As CheckBox, hdPorcentaje2 As HtmlInputHidden, HdFondos As HiddenField, tFechaOperacion As TextBox
    'OT 10090  Fin
    Private Sub GenerarReporteRentaVariable()
        Dim oldCulture As CultureInfo
        oldCulture = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)
        Try
            Dim sFile As String, sTemplate As String
            Dim dtRentaVariable As New Data.DataTable, dtResumen As New Data.DataTable, dtResumen2 As New Data.DataTable, dtResumenSBS As New Data.DataTable,
            dtPatrimFondos As New Data.DataTable, oDs As New DataSet
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim decNProceso As Decimal = 0
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            oDs = oPrevOrdenInversionBM.GenerarReporte(TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), DatosRequest, decNProceso)
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            dtRentaVariable = oDs.Tables(0)
            dtResumen = oDs.Tables(2)
            dtResumen2 = oDs.Tables(3)
            dtResumenSBS = oDs.Tables(4)
            dtPatrimFondos = oDs.Tables(5)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RV_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", _
            DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            Dim n As Integer
            Dim n2 As Long
            Dim dr As DataRow
            Dim i As Integer = 0
            Dim m As Integer = 0
            Dim j As Integer = 0
            Dim k As Integer = 0
            Dim oExcel As New Application
            Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
            Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet, oSheetSBS As Excel.Worksheet
            Dim oCells As Excel.Range
            If File.Exists(sFile) Then
                File.Delete(sFile)
            End If
            sTemplate = RutaPlantillas() & "\" & "PlantillaPrevOrdenInversionRV.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Excel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(2, 17) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheet.Range(oCells(2, 17), oCells(2, 18)).Merge()
            oSheet.Range(oCells(2, 17), oCells(2, 18)).Font.Name = "Arial"
            oSheet.Range(oCells(2, 17), oCells(2, 18)).Font.Bold = True
            oSheet.Range(oCells(2, 17), oCells(2, 18)).Font.Size = 13
            oSheet.SaveAs(sFile)
            n = 8
            Dim nCorrela As Decimal = 0
            Dim nCorrela2 As Decimal = 0
            Dim correlativo As Integer = 0
            Dim cantidadOperacionHoja1 As Decimal = 0
            Dim totalOperacionHoja1 As Decimal = 0
            Dim codigoPrevOrdenHoja1 As Integer = 0
            For Each dr In dtRentaVariable.Rows
                If Integer.Parse(dr("CodigoPrevOrden").ToString.Trim) <> codigoPrevOrdenHoja1 Then
                    n2 = n + 1
                    oSheet.Rows(n & ":" & n).Copy()
                    oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                    oSheet.Application.CutCopyMode = False
                    oCells(n, 2) = dr("Correlativo")
                    codigoPrevOrdenHoja1 = Integer.Parse(dr("CodigoPrevOrden").ToString.Trim)
                    oCells(n, 3) = dr("HoraOperacion")
                    oCells(n, 4) = dr("UsuarioCreacion")
                    oCells(n, 5) = dr("CodigoNemonico")
                    oCells(n, 6) = dr("Instrumento")
                    oCells(n, 7) = dr("Operacion")
                    oCells(n, 8) = dr("Cantidad")
                    oCells(n, 8).NumberFormat = "###,###,##0"
                    oCells(n, 9) = dr("Precio")
                    oCells(n, 9).NumberFormat = "###,###,##0.0000"
                    oCells(n, 10) = dr("Total")
                    oCells(n, 10).NumberFormat = "###,###,##0.00"
                    oCells(n, 11) = dr("Condicion")
                    oCells(n, 12) = dr("Intermediario")
                    oCells(n, 13) = dr("Plaza")
                    oCells(n, 14) = "Fondo " & Replace(dr("TipoFondo").ToString.Trim, "IN-FONDO", "")
                    oCells(n, 15) = dr("IntervaloPrecio")
                    oCells(n, 15).NumberFormat = "###,###,##0.0000000"
                    cantidadOperacionHoja1 = 0
                    totalOperacionHoja1 = 0
                    If dr("CantidadOperacion").ToString.Trim <> "" And dr("CodigoNemonico") <> "" Then
                        cantidadOperacionHoja1 = cantidadOperacionHoja1 + Decimal.Parse(dr("CantidadOperacion"))
                    End If
                    If dr("TotalOperacionRV").ToString.Trim <> "" And dr("CodigoNemonico") <> "" Then
                        totalOperacionHoja1 = totalOperacionHoja1 + Decimal.Parse(dr("TotalOperacionRV"))
                    End If
                    oCells(n, 16) = cantidadOperacionHoja1
                    oCells(n, 16).NumberFormat = "###,###,##0"
                    oCells(n, 17) = dr("PrecioOperacion")
                    oCells(n, 17).NumberFormat = "###,###,##0.0000"
                    oCells(n, 18) = totalOperacionHoja1
                    oCells(n, 18).NumberFormat = "###,###,##0.00"
                    oSheet.Columns("E:E").EntireColumn.AutoFit()
                    oSheet.Columns("H:M").EntireColumn.AutoFit()
                    oSheet.Columns("O:R").EntireColumn.AutoFit()
                    n = n + 1
                End If
                If Integer.Parse(dr("CodigoPrevOrden").ToString.Trim) = codigoPrevOrdenHoja1 And dr("CodigoNemonico") = "" Then
                    n2 = n + 1
                    oSheet.Rows(n & ":" & n).Copy()
                    oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                    oSheet.Application.CutCopyMode = False
                    oCells(n, 16) = dr("CantidadOperacion")
                    oCells(n, 16).NumberFormat = "###,###,##0"
                    oCells(n, 17) = dr("PrecioOperacion")
                    oCells(n, 17).NumberFormat = "###,###,##0.0000"
                    oCells(n, 18) = dr("TotalOperacionRV")
                    oCells(n, 18).NumberFormat = "###,###,##0.00"
                    n = n + 1
                End If
            Next
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            oSheet = CType(oSheets.Item(2), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(6, 9) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheet.SaveAs(sFile)
            n = 10
            Dim codigoPrevOrdenHoja2 As Integer = 0
            Dim formato As String
            Dim correlativoHoja2 As Integer = 0
            Dim swFormato As Integer = 0
            m = 0
            For i = 0 To dtRentaVariable.Rows.Count - 1
                If n >= 29 Then
                    n2 = n + 1
                    oSheet.Rows(n & ":" & n).Copy()
                    oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                    oSheet.Application.CutCopyMode = False
                End If
                If dtRentaVariable.Rows(i)("Correlativo") <> correlativoHoja2 And dtRentaVariable.Rows(i)("CodigoPrevOrden") <> codigoPrevOrdenHoja2 Then
                    oCells(n, 2) = dtRentaVariable.Rows(i)("Operacion")
                    oCells(n, 3) = dtRentaVariable.Rows(i)("Instrumento")
                    oCells(n, 5) = dtRentaVariable.Rows(i)("CodigoNemonico")
                    oCells(n, 6) = dtRentaVariable.Rows(i)("HoraOperacion")
                    oCells(n, 7) = dtRentaVariable.Rows(i)("CantidadOperacion")
                    codigoPrevOrdenHoja2 = Integer.Parse(dtRentaVariable.Rows(i)("CodigoPrevOrden").ToString.Trim)
                    oCells(n, 2).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 3).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 5).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 6).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 7).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 8).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 9).Interior.Color = RGB(204, 255, 204)
                    swFormato = 1
                    n = n + 1
                    For k = 0 To dtRentaVariable.Rows.Count - 1
                        If dtRentaVariable.Rows(k)("CodigoPrevOrden") = dtRentaVariable.Rows(i)("CodigoPrevOrden") And _
                            dtRentaVariable.Rows(k)("CodigoPortafolio") <> "" Then
                            If dtRentaVariable.Rows(k)("CantidadOperacion") <> "" Then
                                If k > m Then
                                    m = k
                                    oCells(n - 1, 7) = dtRentaVariable.Rows(k)("CantidadOperacion")
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                    For j = 0 To dtRentaVariable.Rows.Count - 1
                        If dtRentaVariable.Rows(j)("CodigoPrevOrden") = dtRentaVariable.Rows(i)("CodigoPrevOrden") And _
                            dtRentaVariable.Rows(j)("CodigoPortafolio") <> "" Then
                            If dtRentaVariable.Rows(j)("CantidadOperacion") <> "" Then
                                If IsDBNull(dtRentaVariable.Rows(j)("Asignacion")) = True Then
                                    dtRentaVariable.Rows(j)("Asignacion") = "0"
                                End If
                                formato = IIf(CDec(dtRentaVariable.Rows(j)("Asignacion")) / 100 >= 1, "#,##0", "0%")
                                oCells(n, 8) = dtRentaVariable.Rows(j)("DescripcionFondo").ToString.Trim
                                If Decimal.Parse(dtRentaVariable.Rows(j)("Asignacion")) / 100 >= 1 Then
                                    oCells(n, 9) = dtRentaVariable.Rows(j)("Asignacion")
                                    oCells(n, 9).NumberFormat = formato
                                ElseIf Decimal.Parse(dtRentaVariable.Rows(i)("Asignacion")) / 100 = 1 Then
                                    oCells(n, 9) = dtRentaVariable.Rows(i)("Asignacion").ToString.Trim + "%"
                                Else
                                    oCells(n, 9) = Decimal.Parse(dtRentaVariable.Rows(j)("Asignacion")) / 100
                                    oCells(n, 9).NumberFormat = formato
                                End If
                                n = n + 1
                            End If
                        ElseIf dtRentaVariable.Rows(j)("CodigoPrevOrden") = dtRentaVariable.Rows(i)("CodigoPrevOrden") And _
                            dtRentaVariable.Rows(j)("CodigoPortafolio") = "" Then
                            If swFormato = 0 Then
                                oCells(n, 2) = dtRentaVariable.Rows(i)("Operacion")
                                oCells(n, 3) = dtRentaVariable.Rows(i)("Instrumento")
                                oCells(n, 5) = dtRentaVariable.Rows(i)("CodigoNemonico")
                                oCells(n, 6) = dtRentaVariable.Rows(i)("HoraOperacion")
                                oCells(n, 7) = dtRentaVariable.Rows(i)("CantidadOperacion")
                                oCells(n, 2).Interior.Color = RGB(204, 255, 204)
                                oCells(n, 3).Interior.Color = RGB(204, 255, 204)
                                oCells(n, 5).Interior.Color = RGB(204, 255, 204)
                                oCells(n, 6).Interior.Color = RGB(204, 255, 204)
                                oCells(n, 7).Interior.Color = RGB(204, 255, 204)
                                oCells(n, 8).Interior.Color = RGB(204, 255, 204)
                                oCells(n, 9).Interior.Color = RGB(204, 255, 204)
                                n = n + 1
                            End If
                            oCells(n - 1, 7) = dtRentaVariable.Rows(j)("CantidadOperacion")
                            If IsDBNull(dtRentaVariable.Rows(i)("Asignacion")) = True Then
                                dtRentaVariable.Rows(i)("Asignacion") = "0"
                            End If
                            formato = IIf(CDec(dtRentaVariable.Rows(i)("Asignacion")) / 100 >= 1, "#,##0", "0%")
                            oCells(n, 8) = dtRentaVariable.Rows(i)("DescripcionFondo").ToString.Trim
                            If Decimal.Parse(dtRentaVariable.Rows(i)("Asignacion")) / 100 > 1 Then
                                oCells(n, 9) = dtRentaVariable.Rows(i)("Asignacion")
                                oCells(n, 9).NumberFormat = formato
                            ElseIf Decimal.Parse(dtRentaVariable.Rows(i)("Asignacion")) / 100 = 1 Then
                                oCells(n, 9) = dtRentaVariable.Rows(i)("Asignacion").ToString.Trim + "%"
                            Else
                                oCells(n, 9) = Decimal.Parse(dtRentaVariable.Rows(i)("Asignacion")) / 100
                                oCells(n, 9).NumberFormat = formato
                            End If
                            swFormato = 0
                            n = n + 1
                        End If
                    Next
                    correlativoHoja2 = Integer.Parse(dtRentaVariable.Rows(i)("Correlativo").ToString.Trim)
                End If
            Next
            oSheet.Columns("D:E").EntireColumn.AutoFit()
            If n >= 30 Then
                oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            End If
            oSheet = CType(oSheets.Item(3), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(6, 9) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheet.SaveAs(sFile)
            n = 9
            Dim NemonicoIntermediarioHoja3 As String = ""
            For Each dr In dtResumen.Rows
                If dr("NemonicoOperacionIntermediario").ToString.Trim <> NemonicoIntermediarioHoja3 Then
                    n2 = n + 1
                    oSheet.Rows(n & ":" & n).Copy()
                    oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                    oSheet.Application.CutCopyMode = False
                    oCells(n, 2) = dr("CodigoNemonico")
                    oCells(n, 3) = dr("Operacion")
                    oCells(n, 4) = dr("Intermediario")
                    oCells(n, 5) = dr("HoraOperacion")
                    oCells(n, 6) = dr("PrecioPromedio")
                    oCells(n, 6).NumberFormat = "###,###,##0.0000"
                    'Agregando color
                    oCells(n, 2).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 3).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 4).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 5).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 6).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 7).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 8).Interior.Color = RGB(204, 255, 204)
                    NemonicoIntermediarioHoja3 = dr("NemonicoOperacionIntermediario").ToString.Trim
                    n = n + 1
                End If
                oCells(n, 7) = dr("DescripcionFondo").ToString().Trim()
                oCells(n, 8) = dr("Cantidad").ToString.Trim
                oCells(n, 8).NumberFormat = "###,###,##0.0000"
                n = n + 1
            Next
            oSheet.Columns("D:D").EntireColumn.AutoFit()
            oSheet.Columns("I:I").EntireColumn.AutoFit()
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            'REPORTE DE LA SBS Hoja 4
            oSheetSBS = CType(oSheets.Item(4), Excel.Worksheet)
            oCells = oSheetSBS.Cells
            oCells(6, 13) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheetSBS.SaveAs(sFile)
            n = 10
            For Each dr In dtResumenSBS.Rows
                n2 = n + 1
                oSheetSBS.Rows(n & ":" & n).Copy()
                oSheetSBS.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                oSheetSBS.Application.CutCopyMode = False
                oCells(n, 2) = dr("Afp")
                oCells(n, 3) = dr("Fondo").ToString.Trim
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
            oSheetSBS.Columns("D:D").EntireColumn.AutoFit()
            oSheetSBS.Columns("H:J").EntireColumn.AutoFit()
            oSheetSBS.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            oSheet = CType(oSheets.Item(5), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(5, 14) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheet.SaveAs(sFile)
            n = 10
            Dim MontoAsigna As Decimal
            Dim nemonicoHoja5 As String = ""
            Dim aux As String = ""
            Dim dtportafolioHoja5 As Data.DataTable = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            For Each dr In dtResumen2.Rows
                If dr("NemonicoIntermediario").ToString.Trim <> nemonicoHoja5 Then
                    n2 = n + 1
                    oSheet.Rows(n & ":" & n).Copy()
                    oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                    oSheet.Application.CutCopyMode = False
                    oCells(n, 2) = dr("CodigoNemonico")
                    oCells(n, 2).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 3).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 4).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 5).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 6).Interior.Color = RGB(204, 255, 204)
                    oCells(n, 7).Interior.Color = RGB(204, 255, 204)
                    nemonicoHoja5 = dr("NemonicoIntermediario").ToString.Trim
                    n = n + 1
                End If
                If dr("TotalOperacionRV") > 0 Then
                    oCells(n, 15) = dr("CantidadOperacion") 'O
                    oCells(n, 16) = dr("TotalOperacionRV") 'P
                    oCells(n, 17) = dr("Asignacion") 'Q
                    oCells(n, 17).font.color = ColorTranslator.ToOle(Color.White)
                    oCells(n, 20) = dr("CodigoOperacion") 'T
                    oCells(n, 20).font.color = ColorTranslator.ToOle(Color.White)
                    oCells(n, 21) = ObtenerMontoPatrimonio(dtPatrimFondos, dr("CodigoPortafolio").ToString)
                    oCells(n, 21).font.color = ColorTranslator.ToOle(Color.White)
                    oCells(n, 3) = dr("DescripcionFondo").ToString.Trim()
                    oCells(n, 4) = "=IF(" & dr("Fondo") & "=0,"""",IF($T" & n & "=""Venta"",-1,1) * " & dr("Fondo") & ")"
                    oCells(n, 5) = "=IF(" & Decimal.Parse(dr("Asignacion").ToString.Trim) & ">=1," & _
                    Decimal.Parse(dr("TotalOperacionRV").ToString.Trim) & "/" & Integer.Parse(dr("CantidadOperacion").ToString.Trim) & _
                    "*" & "$D" & n & ",IF(ISERROR(" & Decimal.Parse(dr("TotalOperacionRV").ToString.Trim) & "*" & _
                    Decimal.Parse(dr("Asignacion").ToString.Trim) & "),""""," & Decimal.Parse(dr("TotalOperacionRV").ToString.Trim) & "*" & _
                    Decimal.Parse(dr("Asignacion").ToString.Trim) & "))"
                    oCells(n, 6) = "=IF($T" & n & "=" & """Venta""" & ",IF(ISERROR($P" & n & "/$U" & n & "),"""",-$P" & n & "/$U" & n & "),IF(ISERROR($P" & _
                    n & "/$U" & n & "),"""",$P" & n & "/$U" & n & "))"
                    oCells(n, 6).NumberFormat = "0.00%"
                    oCells(n, 7) = "=IF(ISERROR(E" & n & "/" & "$U" & n & "),"""",E" & n & "/" & "$U" & n & ")"
                    oCells(n, 7).NumberFormat = "0.00%"
                    MontoAsigna = 0
                    For Each draux In dtResumen2.Rows
                        If draux("NemonicoIntermediario").ToString.Trim = nemonicoHoja5 Then
                            MontoAsigna = MontoAsigna + CDec(draux("MAsignaF").ToString.Trim)
                        End If
                    Next
                    If MontoAsigna > 0 Then
                        oCells(n, 5) = "=IF(" & Decimal.Parse(dr("MAsignaF").ToString.Trim) & "=0,0,IF($T" & n & "=""Venta"",-1,1) * " & _
                        Decimal.Parse(dr("MAsignaF").ToString.Trim) & ")"
                    End If
                End If
                n = n + 1
            Next
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            m = 10
            'Portafolios
            For i = 0 To dtportafolioHoja5.Rows.Count - 1
                oCells(m + i, 9) = dtportafolioHoja5.Rows(i)("Descripcion").ToString.Trim
                oCells(m + i, 9).Interior.Color = RGB(204, 255, 204)
            Next
            'Numero de acciones
            For i = 0 To dtportafolioHoja5.Rows.Count - 1
                oCells(m + i, 10) = "=IF($I" & m + i & "="""","""",SUMIF($C" & m & ":$C" & n - 1 & ",$I" & m + i & ",$D" & m & ":$D" & n - 1 & "))"
            Next
            'Monto Asignado(Nuevos soles)
            For i = 0 To dtportafolioHoja5.Rows.Count - 1
                oCells(m + i, 11) = "=SUMIF($C" & m & ":$C" & n - 1 & ",$I" & m + i & ",$E" & m & ":$E" & n - 1 & ")"
            Next
            'Puntos Básicos Equivalentes
            For i = 0 To dtportafolioHoja5.Rows.Count - 1
                oCells(m + i, 12) = "=SUMIF($C" & m & ":$C" & n - 1 & ",$I" & m + i & ",$F" & m & ":$F" & n - 1 & ")"
                oCells(m + i, 12).NumberFormat = "0.00%"
            Next
            'Puntos Básicos Asignados
            For i = 0 To dtportafolioHoja5.Rows.Count - 1
                oCells(m + i, 13) = "=SUMIF($C" & m & ":$C" & n - 1 & ",$I" & m + i & ",$G" & m & ":$G" & n - 1 & ")"
                oCells(m + i, 13).NumberFormat = "0.00%"
            Next
            oBook.Save()
            oBook.Close()
            oExcel.Quit()
            ReleaseComObject(oCells)
            ReleaseComObject(oSheet)
            ReleaseComObject(oSheets) : ReleaseComObject(oBook)
            ReleaseComObject(oBooks) : ReleaseComObject(oExcel)
            oExcel = Nothing : oBooks = Nothing : oBook = Nothing
            oSheets = Nothing : oSheet = Nothing : oCells = Nothing
            GC.Collect()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "FV_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & _
            String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
            Response.WriteFile(sFile)
            Response.End()
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
        Thread.CurrentThread.CurrentCulture = oldCulture
    End Sub
    Public Function ObtenerMontoPatrimonio(ByVal dtPatrimonio As Data.DataTable, ByVal codFondo As String) As String
        Dim monto As String = "0"
        For Each dr As DataRow In dtPatrimonio.Rows
            If dr("CodigoPortafolio").ToString = codFondo Then
                monto = dr("TotalPatrim").ToString
                Exit For
            End If
        Next
        Return monto
    End Function
    Public Sub EjecutarOrdenInversion(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, Optional ByVal strCodPrevOrden As String = "", _
    Optional ByRef bolUpdGrilla As Boolean = False, Optional ByVal claseInstrumento As String = "", Optional ByVal decNProceso As Decimal = 0)
        Dim objBM As New LimiteBM
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim ds As New DataSet
        Dim dtOrdenInversion As New Data.DataTable
        Dim bolGeneraOrden As Boolean = False
        Dim i As Long
        Dim arrCodPrevOrden As Array
        objBM.RegistrarOrdenesPreviasSeleccionadas(strTipoRenta, decNProceso)
        ds = oPrevOrdenInversionBM.GenerarOrdenInversion_Sura(strTipoRenta, decFechaOperacion, DatosRequest, claseInstrumento, decNProceso)
        If ds.Tables(0).Rows.Count <= 0 Then
            bolUpdGrilla = True
        Else
            If ds.Tables(0).Rows(0)(0).ToString.Trim = LIQUIDADO Then
                AlertaJS("Alguna operación ejecutada similar para una agrupación esta liquidada.\nDebe extornar " + _
                         "la operación liquidada antes de ejecutar una agrupación similar.")
            Else
                Dim script As New StringBuilder
                dtOrdenInversion = ds.Tables(0)
                bolGeneraOrden = CType(ds.Tables(1).Rows(0)("GeneraOrden"), Boolean)
                arrCodPrevOrden = strCodPrevOrden.Split("|")
                For i = 0 To arrCodPrevOrden.Length - 1
                    If arrCodPrevOrden(i) <> "" Then
                        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "1")
                    End If
                Next
                'OT-10784 Inicio
                If bolGeneraOrden Then
                    If dtOrdenInversion.Rows.Count > 0 Then
                        Session("dtListaExcesos") = dtOrdenInversion
                        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & _
                        claseInstrumento + "', '650', '450','" & btnBuscar.ClientID & "');")
                    End If
                    '    If dtOrdenInversion.Rows.Count > 0 Then
                    '        Dim Variable As String = "TmpCodigoUsuario,TmpEscenario,TmpNProceso"
                    '        Dim Parametros As String = Usuario + "," + EJECUCION_PREVOI + "," + decNProceso.ToString
                    '        Session("dtOrdenInversion") = dtOrdenInversion
                    '        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=OI&TipoRenta=" & strTipoRenta & "&Instrumento=" & _
                    '        claseInstrumento + "', '650', '450','" & btnBuscar.ClientID & "');")
                    '    End If
                    'Else
                    '    If dtOrdenInversion.Rows.Count > 0 Then
                    '        Session("dtListaExcesos") = dtOrdenInversion
                    '        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & _
                    '        claseInstrumento + "', '650', '450','" & btnBuscar.ClientID & "');")
                    '    End If
                End If
            End If
        End If
        'If bolGeneraOrden = False Then
        '    oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso) 'Se eliminan todos los procesos
        '    'Se setea a cero los flag de las órdenes afectadas
        '    arrCodPrevOrden = strCodPrevOrden.Split("|")
        '    For i = 0 To arrCodPrevOrden.Length - 1
        '        If arrCodPrevOrden(i) <> "" Then
        '            oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
        '        End If
        '    Next
        'End If
        'OT-10784 Fin
    End Sub
    Public Sub VerificaExcesoLimites(ByVal tipoRenta As String, Optional ByVal decNProceso As Decimal = 0)
        Dim Variable As String = "TmpCodigoUsuario,TmpEscenario,TmpNProceso"
        Dim parametros As String = Usuario + "," + VALIDACION_PREVOI + "," + decNProceso.ToString
        Dim objBM As New LimiteBM
        objBM.RegistrarOrdenesPreviasSeleccionadas(tipoRenta, decNProceso)
        Dim obj As New JobBM
        Dim mensaje As String = obj.EjecutarJob("DTS_SIT_VerificaExcesoLimitesEnLinea" & DateTime.Now.ToString("_hhmmss"), _
        "Verifica exceso de limites en linea, considerando el neteo de operaciones", Variable, parametros, "", "", ConfigurationManager.AppSettings(SERVIDORETL))
        AlertaJS(mensaje)
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
    Private Function ValidarIntermediario(ByVal strCodigoTercero As String) As Boolean
        Dim oTercerosBM As New TercerosBM
        Dim oTercerosBE As New TercerosBE
        Dim bolResult As Boolean = False
        oTercerosBE = oTercerosBM.Seleccionar(strCodigoTercero, DatosRequest)
        If oTercerosBE.Tables(0).Rows.Count > 0 Then
            If CLASIFICACIONTERCERO_INTERMEDIARIO = CType(oTercerosBE.Tables(0).Rows(0)("ClasificacionTercero"), String) Then
                bolResult = True
            End If
        End If
        Return bolResult
    End Function
    Private Function ValidarNemonico(ByVal strNemonico As String) As Boolean
        Dim oValoresBM As New ValoresBM
        Dim bolResult As Boolean = False
        Dim strTipoRenta As String = ""
        strTipoRenta = oValoresBM.SeleccionarTipoRentaPorCodigoNemonico(strNemonico)
        If strTipoRenta <> "" Then
            If TIPO_RENTA_VARIABLE.ToString.Replace("_", " ") = strTipoRenta Then
                bolResult = True
            End If
        End If
        Return bolResult
    End Function
    Private Function ValidarSaldo(ByVal FechaOperacion As Decimal, ByVal strNemonico As String, ByVal strCodigoPortafolio As String, ByVal strCategoriaInstrumento As String, ByVal strOperacion As String, ByVal cantidad As Decimal, ByVal tipoGuardado As Integer, ByVal codigoOrden As Decimal) As Boolean
        Dim bolResult As Boolean = True
        Dim objValores As New ValoresBM
        Dim oPrevOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim dtSumaUnidades As DataTable
        Dim DSresultado = objValores.ListarPorFiltro(DatosRequest, strCategoriaInstrumento, String.Empty, String.Empty, strNemonico, strCodigoPortafolio, String.Empty, strOperacion)
        If strOperacion = OPERACION_VENTA Then
            If DSresultado.Tables(0).Rows.Count > 0 Then
                dtSumaUnidades = oPrevOrdenInversionBM.ObtenerUnidadesNegociadasDiaT(strCodigoPortafolio, FechaOperacion, strNemonico).Tables(0)
                If dtSumaUnidades.Rows.Count > 0 Then
                    If tipoGuardado = 1 Then
                        cantidad += Decimal.Parse(dtSumaUnidades.Compute("Sum(UNIDADES)", String.Empty))
                    ElseIf tipoGuardado = 2 Then
                        cantidad += Decimal.Parse(dtSumaUnidades.Compute("Sum(UNIDADES)", "ID <> " & codigoOrden))
                    End If
                End If
                If CDec(DSresultado.Tables(0).Rows(0)(5)) < cantidad Then bolResult = False
            Else
                bolResult = False
            End If
        End If
        Return bolResult
    End Function
    Private Sub HabilitaControles(ByVal habilita As Boolean, Optional ByVal AntesApertura As Boolean = False)
        '==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-13 | El contenido de este método pasa a ser obsoleto pues cada negociación estará HABILITADA o NO según su ESTADO

        'ibGrabar.Enabled = habilita
        'ibValidar.Enabled = habilita
        'ibValidarTrader.Enabled = habilita
        'ibAprobar.Enabled = habilita
        'Datagrid1.Enabled = habilita
        'If AntesApertura Then
        '    ibGrabar.Enabled = True
        '    Datagrid1.Enabled = True
        'End If

        '==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-13 | El contenido de este método pasa a ser obsoleto pues cada negociación estará HABILITADA o NO según su ESTADO
    End Sub
    Private Sub CargarGrilla(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, Optional ByVal strCodigoClaseInstrumento As String = "", _
    Optional ByVal strCodigoTipoInstrumentoSBS As String = "", Optional ByVal strCodigoNemonico As String = "", Optional ByVal strOperador As String = "", _
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
    Private Sub CargarEstados()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As New Data.DataTable
        dt = oParametrosGeneralesBM.Listar(ESTADO_PREV_OI, DatosRequest)
        HelpCombo.LlenarComboBox(ddlEstado, dt, "Valor", "Nombre", True)
        ddlEstado.SelectedIndex = 0
    End Sub
    Private Sub CargarOperadores()
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim dt As New Data.DataTable
        dt = oPrevOrdenInversionBM.SeleccionarOperadores(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlOperador, dt, "UsuarioCreacion", "UsuarioCreacion", True)
        ddlOperador.SelectedIndex = 0
    End Sub
    Private Sub CargaClaseInstrumento()
        'CLASE INSTRUMENTO
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
        Dim dsClaseInstrumento As DataSet
        dsClaseInstrumento = oClaseInstrumentoBM.SeleccionarClaseInstrumentoPorTipoRenta(TR_RENTA_VARIABLE, DatosRequest)
        HelpCombo.LlenarComboBox(ddlClaseInstrumento, dsClaseInstrumento.Tables(0), "Codigo", "Descripcion", True)
        ddlClaseInstrumento.SelectedIndex = 0
    End Sub
    Private Sub CargarTipoInstrumento(ByVal codigoClaseInstrumento As String)
        'TIPO INSTRUMENTO
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim dt As New Data.DataTable
        dt = oTipoInstrumentoBM.SeleccionarPorFiltro(codigoClaseInstrumento, "", TR_RENTA_VARIABLE, "A", DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoInstrumento, dt, "CodigoTipoInstrumentoSBS", "Descripcion", True)
        ddlTipoInstrumento.SelectedIndex = 0
    End Sub
    Private Sub CargarCombos()
        CargarOperadores()
        CargaClaseInstrumento()
        CargarTipoInstrumento("")
        CargarEstados()
    End Sub
    Private Sub CargarPagina()
        CargarCombos()
        hdFechaNegocio.Value = UIUtility.ObtenerFechaMaximaNegocio()
        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(hdFechaNegocio.Value)
        ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
        ViewState("strTipoInstrumento") = ddlTipoInstrumento.SelectedValue
        ViewState("strOperador") = ddlOperador.SelectedValue
        ViewState("strCodigoNemonico") = tbCodigoMnemonico.Text
        ViewState("strEstado") = ddlEstado.SelectedValue
        Dim dtCondicion As New Data.DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtCondicion = oParametrosGeneralesBM.ListarCondicionPrevOI().Tables(0)
        Session("dtCondicion") = dtCondicion
        Dim dtOperacion As New Data.DataTable
        Dim oOperacionBM As New OperacionBM
        dtOperacion = oOperacionBM.SeleccionarporClaseinstrumento("OperacionOI", ESTADO_ACTIVO).Tables(0)
        Session("dtOperacion") = dtOperacion
        Dim dtPlazaN As New Data.DataTable
        Dim oPlazaBM As New PlazaBM
        dtPlazaN = oPlazaBM.ListarxOrden(DatosRequest).Tables(0)
        Session("dtPlazaN") = dtPlazaN
        Dim dtTipoFondo As Data.DataTable
        dtTipoFondo = New ParametrosGeneralesBM().ListarFondosInversion(DatosRequest, "M")
        Session("dtTipoFondo") = dtTipoFondo
        Dim dtTipoTramo As Data.DataTable
        dtTipoTramo = New ParametrosGeneralesBM().Listar("TIPOTRAMO", Me.DatosRequest)
        Session("dtTipoTramo") = dtTipoTramo
        Dim dtMedioTrans As New Data.DataTable
        dtMedioTrans = oParametrosGeneralesBM.ListarMedioNegociacionPrevOI(TR_RENTA_VARIABLE).Tables(0)
        Session("dtMedioTrans") = dtMedioTrans
        CargarGrilla(TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"))
        '  Session.Remove("dtDetalleInversiones")
        hdPuedeNegociar.Value = New PersonalBM().VerificaPermisoNegociacion(Session("Login"))
        If hdPuedeNegociar.Value = "0" Then
            HabilitaControles(False)
        End If
    End Sub
    Private Sub removerSessiones()
        Dim i As Integer
        For i = 0 To Datagrid1.Rows.Count - 1
            lbCodigoPrevOrden = CType(Datagrid1.Rows(i).FindControl("lbCodigoPrevOrden"), Label)
            Session.Remove("dtDetalleInversiones" & lbCodigoPrevOrden.Text.Trim)
        Next
    End Sub
#End Region
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles form1.Load
        Try
            If Not Page.IsPostBack Then
                CargarLoading("btnBuscar")
                'ibAprobar.Attributes.Add("onClick", "this.disabled = true; this.value = 'En proceso...';")
                'ibValidarTrader.Attributes.Add("onClick", "this.disabled = true; this.value = 'En proceso...';")
                'ibGrabar.Attributes.Add("onClick", "this.disabled = true; this.value = 'En proceso...';")
                CargarPagina()
                removerSessiones()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            Dim decFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            Dim decFechaActual As Decimal = Convert.ToDecimal(DateTime.Now.ToString("yyyyMMdd"))
            If decFechaOperacion = hdFechaNegocio.Value And hdPuedeNegociar.Value = "1" Then
                HabilitaControles(True)
            ElseIf decFechaOperacion > hdFechaNegocio.Value And hdPuedeNegociar.Value = "1" Then
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
            CargarGrilla(TR_RENTA_VARIABLE.ToString(), decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, strOperador, strEstado)
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub Datagrid1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles Datagrid1.RowCommand
        Try
            If e.CommandName = "Add" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow), oPrevOrdenInversionBM As New PrevOrdenInversionBM,
                oPrevOrdenInversionBE As New PrevOrdenInversionBE, oRow As PrevOrdenInversionBE.PrevOrdenInversionRow, bolValidaCampos As Boolean = False
                oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox)
                tbIntermediarioF = CType(Datagrid1.FooterRow.FindControl("tbIntermediarioF"), TextBox)
                hdIntermediarioF = CType(Datagrid1.FooterRow.FindControl("hdIntermediarioF"), HtmlInputHidden)
                ddlOperacionF = CType(Datagrid1.FooterRow.FindControl("ddlOperacionF"), DropDownList)
                tbCantidadF = CType(Datagrid1.FooterRow.FindControl("tbCantidadF"), TextBox)
                tbPrecioF = CType(Datagrid1.FooterRow.FindControl("tbPrecioF"), TextBox)
                ddlCondicionF = CType(Datagrid1.FooterRow.FindControl("ddlCondicionF"), DropDownList)
                ddlPlazaNF = CType(Datagrid1.FooterRow.FindControl("ddlPlazaNF"), DropDownList)
                tbCantidadOperacionF = CType(Datagrid1.FooterRow.FindControl("tbCantidadOperacionF"), TextBox)
                tbPrecioOperacionF = CType(Datagrid1.FooterRow.FindControl("tbPrecioOperacionF"), TextBox)
                chkPorcentajeF = CType(Datagrid1.FooterRow.FindControl("chkPorcentajeF"), CheckBox)
                Dim tFechaOperacionF As HiddenField
                tFechaOperacionF = CType(Datagrid1.FooterRow.FindControl("hdFechaOperacionF"), HiddenField)

                'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 16/05/2018
                '  Session.Remove("dtDetalleInversiones")
                porcentajeF = "N"
                Dim sumaAsignacion As Decimal = 0
                If porcentajeF.Equals("N") Then
                    sumaAsignacion = Decimal.Parse(tbCantidadOperacionF.Text.Trim)
                End If
                Dim dtDetalleInversiones As New DataTable
                dtDetalleInversiones = instanciarTabla(dtDetalleInversiones)

                Dim portafolioF As DropDownList
                portafolioF = CType(Datagrid1.FooterRow.FindControl("ddlPortafolioF"), DropDownList)
                Dim portafolioIdF As HiddenField
                portafolioIdF = CType(Datagrid1.FooterRow.FindControl("HdPortafolioF"), HiddenField)
                Dim codigoPrevOrden As HiddenField
                codigoPrevOrden = CType(Datagrid1.FooterRow.FindControl("HdCodigoOrdenF"), HiddenField)
                If codigoPrevOrden.Value.Trim.Length <= 0 Then
                    codigoPrevOrden.Value = "0"
                End If
                dtDetalleInversiones.Rows.Add(CType(codigoPrevOrden.Value, Decimal), portafolioIdF.Value, CType(tbCantidadF.Text, Decimal), "N")
                '  Session("dtDetalleInversiones") = dtDetalleInversiones
                'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 16/05/2018

                hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlInputHidden)
                ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                ddlContactoF = CType(Datagrid1.FooterRow.FindControl("ddlContactoF"), DropDownList)
                tbHoraF = CType(Datagrid1.FooterRow.FindControl("tbHoraF"), TextBox)
                ddlMedioTransF = CType(Datagrid1.FooterRow.FindControl("ddlMedioTransF"), DropDownList)
                If Datagrid1.FooterRow.RowType = ListItemType.Footer Then
                    If tbCantidadF.Text <> "" And tbPrecioF.Text <> "" And tbPrecioOperacionF.Text <> "" And tbCantidadOperacionF.Text <> "" _
                    And tbNemonicoF.Text <> "" And hdIntermediarioF.Value.ToString <> "" And _
                    Not ((hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_FONDOINVERSION) And _
                    ddlTipoFondoF.SelectedValue = DDL_ITEM_SELECCIONE) And _
                    Not (hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondoF.SelectedValue = TIPOFONDO_ETF And _
                    ddlTipoTramoF.SelectedValue = DDL_ITEM_SELECCIONE) Then
                        If IsNumeric(tbCantidadF.Text) And IsNumeric(tbPrecioF.Text) And IsNumeric(tbCantidadOperacionF.Text) And IsNumeric(tbPrecioOperacionF.Text) Then
                            If ValidarNemonico(tbNemonicoF.Text) And ValidarIntermediario(hdIntermediarioF.Value.ToString) _
                                And ValidarSaldo(UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Value.ToString), tbNemonicoF.Text, portafolioIdF.Value.ToString, hdClaseInstrumentoF.Value.ToString, ddlOperacionF.SelectedValue, IIf(IsNumeric(tbCantidadF.Text), CDec(tbCantidadF.Text), 0), 1, 0) Then
                                bolValidaCampos = True
                            End If
                        End If
                    Else
                        If tbNemonicoF.Text = "" Then
                            strMensaje = strMensaje + "- Ingrese Nemónico. \n"
                        End If
                        If tbCantidadF.Text = "" Then
                            strMensaje = strMensaje + "- Ingrese Cant. Instrumento. \n"
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
                        If (hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_FONDOINVERSION) And _
                            ddlTipoFondoF.SelectedValue = DDL_ITEM_SELECCIONE Then
                            strMensaje = strMensaje + "- Seleccione Tipo. \n"
                        End If
                        If hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondoF.SelectedValue = TIPOFONDO_ETF And _
                            ddlTipoTramoF.SelectedValue = DDL_ITEM_SELECCIONE Then
                            strMensaje = strMensaje + "- Seleccione Tipo Tramo. \n"
                        End If
                    End If
                    If bolValidaCampos = True Then
                        'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - COMENTADO | 18/05/2018
                        'oRow.FechaOperacion = ViewState("decFechaOperacion")
                        'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - COMENTADO | 18/05/2018

                        'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 18/05/2018
                        'Dim tFechaOperacionF As TextBox
                        'tFechaOperacionF = CType(Datagrid1.FooterRow.FindControl("tFechaOperacionF"), TextBox)
                        'ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Text)
                        'oRow.FechaOperacion = ViewState("decFechaOperacion")
               
                        ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Value)
                        oRow.FechaOperacion = ViewState("decFechaOperacion")

                        'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 18/05/2018

                        oRow.HoraOperacion = Replace(Now.ToLongTimeString(), " a.m.", "")
                        oRow.CodigoNemonico = tbNemonicoF.Text
                        oRow.CodigoOperacion = ddlOperacionF.SelectedValue
                        If hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_ACCION Then
                            oRow.Cantidad = CType(tbCantidadF.Text, Integer)
                            oRow.CantidadOperacion = CType(tbCantidadOperacionF.Text, Integer)
                        Else
                            oRow.Cantidad = CType(tbCantidadF.Text, Decimal)
                            oRow.CantidadOperacion = CType(tbCantidadOperacionF.Text, Decimal)
                        End If
                        oRow.Precio = CType(tbPrecioF.Text, Decimal)
                        oRow.TipoCondicion = ddlCondicionF.SelectedValue
                        oRow.CodigoTercero = hdIntermediarioF.Value
                        oRow.CodigoPlaza = ddlPlazaNF.SelectedValue
                        oRow.IntervaloPrecio = 0
                        oRow.PrecioOperacion = CType(tbPrecioOperacionF.Text, Decimal)
                        oRow.Situacion = ESTADO_ACTIVO
                        oRow.Estado = PREV_OI_INGRESADO
                        oRow.CodigoContacto = ddlContactoF.SelectedValue
                        oRow.TipoFondo = IIf(ddlTipoFondoF.SelectedValue = DDL_ITEM_SELECCIONE, "", ddlTipoFondoF.SelectedValue)
                        oRow.TipoTramo = IIf(ddlTipoTramoF.SelectedValue = DDL_ITEM_SELECCIONE, "", ddlTipoTramoF.SelectedValue)
                        oRow.MedioNegociacion = IIf(ddlMedioTransF.SelectedValue = DDL_ITEM_SELECCIONE, "", ddlMedioTransF.SelectedValue)
                        oRow.HoraEjecucion = Replace(Now.ToLongTimeString(), " a.m.", "")
                        oRow.Porcentaje = porcentajeF
                        oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                        oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                        oPrevOrdenInversionBM.Insertar(oPrevOrdenInversionBE, TR_RENTA_VARIABLE.ToString(), DatosRequest, dtDetalleInversiones)
                        CargarGrilla(TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                        ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(CType(ViewState("decFechaOperacion"), String))
                        '   Session.Remove("dtDetalleInversiones")
                    Else
                        If Not ValidarNemonico(tbNemonicoF.Text) Then
                            strMensaje = "- El nemonico ingresado es invalido! \n"
                        End If
                        If Not ValidarIntermediario(hdIntermediarioF.Value) Then
                            strMensaje = strMensaje + "- El intermediario ingresado es invalido! \n"
                        End If
                        If Not ValidarSaldo(UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Value.ToString), tbNemonicoF.Text, portafolioIdF.Value.ToString, hdClaseInstrumentoF.Value.ToString, ddlOperacionF.SelectedValue, IIf(IsNumeric(tbCantidadF.Text), CDec(tbCantidadF.Text), 0), 1, 0) Then
                            strMensaje += "No existe el saldo suficiente para generar la pre-orden de inversión. \n"
                        End If
                        If strMensaje <> "" Then
                            AlertaJS(strMensaje)
                        End If
                    End If
                Else
                    If strMensaje <> "" Then
                        AlertaJS(strMensaje)
                    Else
                        AlertaJS("Ingrese correctamente el registro!")
                    End If
                End If
            End If
            If e.CommandName = "_Delete" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim decCodigoPrevOrden As Decimal = CType(e.CommandArgument.ToString(), Decimal)
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                Dim script As String = ""
                If gvr.Cells(2).Text = PREV_OI_EJECUTADO Then
                    Dim dtValidaExtorno As New Data.DataTable
                    dtValidaExtorno = oPrevOrdenInversionBM.ValidaExtorno(decCodigoPrevOrden, DatosRequest).Tables(0)
                    If dtValidaExtorno.Rows.Count > 0 Then
                        script = "Para realizar el extorno, debe revertir las siguientes operaciones: \n"
                        For Each fila As DataRow In dtValidaExtorno.Rows
                            script = script & " - " & fila("CodigoOrden").ToString() & ", " & fila("CodigoPortafolioSBS") & ", " & fila("Estado").ToString() & "\n"
                        Next
                        AlertaJS(script)
                    Else
                        EjecutarJS("showModalDialog('frmExtornoIngresoMasivoOperacion.aspx?tipoRenta=" + TR_RENTA_VARIABLE.ToString() + "&codigo=" + _
                        decCodigoPrevOrden.ToString() + "', '650', '450','" & btnBuscar.ClientID & "');")
                    End If
                ElseIf gvr.Cells(2).Text = PREV_OI_APROBADO Then
                    oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                ElseIf gvr.Cells(2).Text = PREV_OI_ELIMINADO Then
                    oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                Else
                    oPrevOrdenInversionBM.Eliminar(decCodigoPrevOrden, DatosRequest)
                End If
                CargarGrilla(TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), _
                ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
            End If
            If e.CommandName = "Select" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim decCodigoPrevOrden As Decimal = CType(e.CommandArgument.ToString(), Decimal)
                EjecutarJS("showModalDialog('frmSubNivelesRentaVariable.aspx?Estado=" + gvr.Cells(2).Text + "&Codigo=" & decCodigoPrevOrden.ToString() + "', '800', '450','');")
            End If
            If e.CommandName = "Item" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)
                hdIntermediario = CType(gvr.FindControl("hdIntermediario"), HtmlInputHidden)
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
                        ddlContacto.Items.Insert(0, New ListItem(DDL_ITEM_SELECCIONE, ""))
                    End If
                End If
                tbNemonico = CType(gvr.FindControl("tbNemonico"), TextBox)
                hdNemonico = CType(gvr.FindControl("hdNemonico"), HtmlInputHidden)
                If hdNemonico.Value <> "" Then
                    tbNemonico.Text = hdNemonico.Value
                End If
                hdDescTercero = CType(gvr.FindControl("hdDescTercero"), HtmlInputHidden)
                tbIntermediario = CType(gvr.FindControl("tbIntermediario"), TextBox)
                If hdDescTercero.Value <> "" Then
                    tbIntermediario.Text = hdDescTercero.Value
                End If
                If tbNemonico.Text.Trim <> "" Then
                    ddlTipoFondo = CType(gvr.FindControl("ddlTipoFondo"), DropDownList)
                    ddlTipoTramo = CType(gvr.FindControl("ddlTipoTramo"), DropDownList)
                    If hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOINVERSION Then
                        ddlTipoFondo.Enabled = True
                        If hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOMUTUO Then
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
                    tbHora = CType(gvr.FindControl("tbHora"), TextBox)
                    tbHora.Text = Now.ToString("hh:mm:ss")
                End If
            End If
            If e.CommandName = "TipoFondo" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim ddlTipoFondo As DropDownList
                Dim ddlTipoTramo As DropDownList
                Dim hdClaseInstrumento As HtmlInputHidden
                ddlTipoFondo = CType(gvr.FindControl("ddlTipoFondo"), DropDownList)
                ddlTipoTramo = CType(gvr.FindControl("ddlTipoTramo"), DropDownList)
                hdClaseInstrumento = CType(gvr.FindControl("hdClaseInstrumento"), HtmlInputHidden)
                If hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOMUTUO Then
                    If ddlTipoFondo.SelectedValue = TIPOFONDO_ETF Then
                        ddlTipoTramo.Enabled = True
                    Else
                        ddlTipoTramo.Enabled = False
                        ddlTipoTramo.SelectedIndex = 0
                    End If
                End If
            End If
            If e.CommandName = "Footer" Then
                hdIntermediarioF = CType(Datagrid1.FooterRow.FindControl("hdIntermediarioF"), HtmlInputHidden)
                tbIntermediarioF = CType(Datagrid1.FooterRow.FindControl("tbIntermediarioF"), TextBox)
                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox)
                If tbNemonicoF.Text.Trim <> "" Then
                    hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlInputHidden)
                    ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                    ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                    If hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_FONDOINVERSION Then
                        ddlTipoFondoF.Enabled = True
                        If hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_FONDOMUTUO Then
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
                    tbHoraF = CType(Datagrid1.FooterRow.FindControl("tbHoraF"), TextBox)
                    tbHoraF.Text = New UtilDM().RetornarHoraSistema
                End If
            End If
            If e.CommandName = "TipoFondoF" Then
                ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlInputHidden)
                If hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_FONDOMUTUO Then
                    If ddlTipoFondoF.SelectedValue = TIPOFONDO_ETF Then
                        ddlTipoTramoF.Enabled = True
                    Else
                        ddlTipoTramoF.Enabled = False
                        ddlTipoTramoF.SelectedIndex = 0
                    End If
                End If
            End If
            If e.CommandName = "Condicion" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                tbTotal = CType(gvr.FindControl("tbTotal"), TextBox)
                hdTotal = CType(gvr.FindControl("hdTotal"), HtmlInputHidden)
                ddlCondicion = CType(gvr.FindControl("ddlCondicion"), DropDownList)
                ddlTipoFondo = CType(gvr.FindControl("ddlTipoFondo"), DropDownList)
                ddlTipoTramo = CType(gvr.FindControl("ddlTipoTramo"), DropDownList)
                hdClaseInstrumento = CType(gvr.FindControl("hdClaseInstrumento"), HtmlInputHidden)
                If hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOMUTUO Then
                    ddlTipoTramo.Enabled = True
                    ddlTipoFondo.SelectedValue = TIPOFONDO_ETF
                    If ddlCondicion.SelectedValue = PRINCIPAL_TRADE Then
                        ddlTipoTramo.SelectedValue = TIPO_TRAMO_PRINCIPAL
                    Else
                        ddlTipoTramo.SelectedValue = TIPO_TRAMO_AGENCIA
                    End If
                End If
            End If
            If e.CommandName = "CondicionF" Then
                tbTotalF = CType(Datagrid1.FooterRow.FindControl("tbTotalF"), TextBox)
                hdTotalF = CType(Datagrid1.FooterRow.FindControl("hdTotalF"), HtmlInputHidden)
                ddlCondicionF = CType(Datagrid1.FooterRow.FindControl("ddlCondicionF"), DropDownList)
                ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlInputHidden)
                If hdClaseInstrumentoF.Value = CLASE_INSTRUMENTO_FONDOMUTUO Then
                    ddlTipoTramoF.Enabled = True
                    ddlTipoFondoF.SelectedValue = TIPOFONDO_ETF
                    If ddlCondicionF.SelectedValue = PRINCIPAL_TRADE Then
                        ddlTipoTramoF.SelectedValue = TIPO_TRAMO_PRINCIPAL
                    Else
                        ddlTipoTramoF.SelectedValue = TIPO_TRAMO_AGENCIA
                    End If
                End If
            End If
            If e.CommandName = "asignarfondo" Then
                Dim codigoPrevOrden As String = e.CommandArgument.trim
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
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
            End If
            If e.CommandName = "asignarfondoF" Then
                Dim codigoPrevOrden As String = e.CommandArgument.trim
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
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
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la grilla")
        End Try
    End Sub
    Public Sub CargarDatosContactoF(ByVal Intermediario As String)
        Dim ddlContactoF As DropDownList
        Dim objContacto As New ContactoBM
        Dim dtContacto As DataSet
        ddlContactoF = CType(Datagrid1.FooterRow.FindControl("ddlContactoF"), DropDownList)
        dtContacto = objContacto.ListarContactoPorTerceros(Intermediario)
        If dtContacto.Tables.Count > 0 Then
            ddlContactoF = CType(Datagrid1.FooterRow.FindControl("ddlContactoF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlContactoF, dtContacto.Tables(0), "CodigoContacto", "DescripcionContacto", True)
            ddlContactoF.SelectedValue = ""
        Else
            ddlContactoF.Items.Insert(0, New ListItem(DDL_ITEM_SELECCIONE, ""))
        End If
    End Sub
    Protected Sub Datagrid1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles Datagrid1.RowDataBound
        Try
            If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
                ddlTipoFondo = CType(e.Row.FindControl("ddlTipoFondo"), DropDownList)
                lbOperacion = CType(e.Row.FindControl("lbOperacion"), Label)
                ddlOperacion = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
                HelpCombo.LlenarComboBox(ddlOperacion, CType(Session("dtOperacion"), Data.DataTable), "codigoOperacion", "Descripcion", False)
                ddlOperacion.SelectedValue = lbOperacion.Text
                hdOperacionTrz = CType(e.Row.FindControl("hdOperacionTrz"), HtmlInputHidden)
                hdOperacionTrz.Value = ddlOperacion.SelectedItem.Text
                lbTipoCondicion2 = CType(e.Row.FindControl("lbTipoCondicion2"), Label)
                ddlCondicion = CType(e.Row.FindControl("ddlCondicion"), DropDownList)
                HelpCombo.LlenarComboBox(ddlCondicion, CType(Session("dtCondicion"), Data.DataTable), "Valor", "Nombre", False)
                ddlCondicion.SelectedValue = lbTipoCondicion2.Text
                lbPlazaN = CType(e.Row.FindControl("lbPlazaN"), Label)
                ddlPlazaN = CType(e.Row.FindControl("ddlPlazaN"), DropDownList)
                HelpCombo.LlenarComboBox(ddlPlazaN, CType(Session("dtPlazaN"), Data.DataTable), "CodigoPlaza", "Descripcion", False)
                ddlPlazaN.SelectedValue = lbPlazaN.Text
                hdIntermediario = CType(e.Row.FindControl("hdIntermediario"), HtmlInputHidden)
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
                        ddlContacto.Items.Insert(0, New ListItem(DDL_ITEM_SELECCIONE, ""))
                    End If
                Else
                    ddlContacto.Items.Insert(0, New ListItem(DDL_ITEM_SELECCIONE, ""))
                End If
                lbTipoFondo = CType(e.Row.FindControl("lbTipoFondo"), Label)
                HelpCombo.LlenarComboBox(ddlTipoFondo, CType(Session("dtTipoFondo"), Data.DataTable), "Valor", "Nombre", False)
                ddlTipoFondo.Items.Insert(0, New ListItem(DDL_ITEM_SELECCIONE, DDL_ITEM_SELECCIONE))
                ddlTipoFondo.SelectedValue = IIf(lbTipoFondo.Text.Trim = "", DDL_ITEM_SELECCIONE, lbTipoFondo.Text)
                lbTipoTramo = CType(e.Row.FindControl("lbTipoTramo"), Label)
                ddlTipoTramo = CType(e.Row.FindControl("ddlTipoTramo"), DropDownList)
                HelpCombo.LlenarComboBox(ddlTipoTramo, CType(Session("dtTipoTramo"), Data.DataTable), "Valor", "Nombre", False)
                ddlTipoTramo.Items.Insert(0, New ListItem(DDL_ITEM_SELECCIONE, DDL_ITEM_SELECCIONE))
                ddlTipoTramo.SelectedValue = IIf(lbTipoTramo.Text.Trim = "", DDL_ITEM_SELECCIONE, lbTipoTramo.Text)
                '==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-07 | Mejor forma de utilizar los datos de la grilla con "DataItem", en vez de Hiddens por todos lados
                ddlfondos = CType(e.Row.FindControl("ddlfondos"), DropDownList)
                ddlfondos.Items.Add(New WebControls.ListItem(e.Row.DataItem("PortafolioSelec"), e.Row.DataItem("CodigoPortafolioSelec")))
                '==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-07 | Mejor forma de utilizar los datos de la grilla con "DataItem", en vez de Hiddens por todos lados

                hdClaseInstrumento = CType(e.Row.FindControl("hdClaseInstrumento"), HtmlInputHidden)
                If hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOINVERSION Or hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOMUTUO Then
                    ddlTipoFondo.Enabled = True
                    If hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondo.SelectedValue = TIPOFONDO_ETF Then
                        ddlTipoTramo.Enabled = True
                    End If
                End If
                lbMedioTrans = CType(e.Row.FindControl("lbMedioTrans"), Label)
                ddlMedioTrans = CType(e.Row.FindControl("ddlMedioTrans"), DropDownList)
                HelpCombo.LlenarComboBox(ddlMedioTrans, CType(Session("dtMedioTrans"), Data.DataTable), "Valor", "Nombre", False)
                ddlMedioTrans.Items.Insert(0, New ListItem(DDL_ITEM_SELECCIONE, DDL_ITEM_SELECCIONE))
                ddlMedioTrans.SelectedValue = IIf(lbMedioTrans.Text.Trim = "", DDL_ITEM_SELECCIONE, lbMedioTrans.Text)
                If e.Row.Cells(2).Text = PREV_OI_EJECUTADO Or e.Row.Cells(2).Text = PREV_OI_APROBADO Or e.Row.Cells(2).Text = PREV_OI_PENDIENTE Or
                e.Row.Cells(2).Text = PREV_OI_ELIMINADO Then
                    ddlCondicion.Enabled = False
                    ddlOperacion.Enabled = False
                    ddlPlazaN.Enabled = False
                    ddlContacto.Enabled = False
                    ddlTipoFondo.Enabled = False
                    ddlTipoTramo.Enabled = False
                    ddlMedioTrans.Enabled = False
                    tbCantidad = CType(e.Row.FindControl("tbCantidad"), TextBox)
                    tbPrecio = CType(e.Row.FindControl("tbPrecio"), TextBox)
                    tbPrecioOperacion = CType(e.Row.FindControl("tbPrecioOperacion"), TextBox)
                    tbTotalOperacion = CType(e.Row.FindControl("tbTotalOperacion"), TextBox)
                    tbIntermediario = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                    tbNemonico = CType(e.Row.FindControl("tbNemonico"), TextBox)
                    chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)
                    hdPorcentaje = CType(e.Row.FindControl("hdPorcentaje"), HtmlInputHidden)
                    chkPorcentaje = CType(e.Row.FindControl("chkPorcentaje"), CheckBox)
                    tbHora = CType(e.Row.FindControl("tbHora"), TextBox)
                    tbTotal = CType(e.Row.FindControl("tbTotal"), TextBox)
                    tbCantidadOperacion = CType(e.Row.FindControl("tbCantidadOperacion"), TextBox)
                    tbCantidad.Enabled = False
                    tbPrecio.Enabled = False
                    tbCantidadOperacion.Enabled = False
                    tbPrecioOperacion.Enabled = False
                    tbTotalOperacion.Enabled = False
                    tbIntermediario.Enabled = False
                    tbNemonico.Enabled = False
                    chkSelect.Enabled = False
                    If hdPorcentaje.Value = "S" Then
                        chkPorcentaje.Checked = True
                    Else
                        chkPorcentaje.Checked = False
                    End If
                    tbHora.Enabled = False
                    tbTotal.Enabled = False
                    If e.Row.Cells(2).Text = PREV_OI_APROBADO Or _
                        e.Row.Cells(2).Text = PREV_OI_PENDIENTE Then
                        chkSelect.Enabled = True
                    End If
                End If
                tbHora2 = CType(e.Row.FindControl("tbHora"), TextBox)
                tbNemonico2 = CType(e.Row.FindControl("tbNemonico"), TextBox)
                ddlOperacion2 = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
                tbCantidad2 = CType(e.Row.FindControl("tbCantidad"), TextBox)
                tbPrecio2 = CType(e.Row.FindControl("tbPrecio"), TextBox)
                tbTotal2 = CType(e.Row.FindControl("tbTotal"), TextBox)
                tbIntermediario2 = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                ddlContacto2 = CType(e.Row.FindControl("ddlContacto"), DropDownList)
                ddlMedioTrans2 = CType(e.Row.FindControl("ddlMedioTrans"), DropDownList)
                ddlTipoTramo2 = CType(e.Row.FindControl("ddlTipoTramo"), DropDownList)
                ddlPlazaN2 = CType(e.Row.FindControl("ddlPlazaN"), DropDownList)
                tbCantidadOperacion2 = CType(e.Row.FindControl("tbCantidadOperacion"), TextBox)
                tbPrecioOperacion2 = CType(e.Row.FindControl("tbPrecioOperacion"), TextBox)
                tbTotalOperacion2 = CType(e.Row.FindControl("tbTotalOperacion"), TextBox)
                hdPorcentaje2 = CType(e.Row.FindControl("hdPorcentaje"), HtmlInputHidden)
                chkPorcentaje2 = CType(e.Row.FindControl("chkPorcentaje"), CheckBox)
                If hdPorcentaje2.Value = "S" Then
                    chkPorcentaje2.Checked = True
                Else
                    chkPorcentaje2.Checked = False
                End If

                '==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-13 | Para cada fila que llena, solo estarán habilitados sus controles si el ESTADO es EJECUTADO o ELIMINADO
                e.Row.Enabled = Not (e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO)
                '==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-13 | Para cada fila que llena, solo estarán habilitados sus controles si el ESTADO es EJECUTADO o ELIMINADO
            End If
            If e.Row.RowType = ListItemType.Footer Then
                tbOperadorF = CType(e.Row.FindControl("tbOperadorF"), TextBox)
                tbOperadorF.Text = Usuario.ToString.Trim
                ddlOperacionF = CType(e.Row.FindControl("ddlOperacionF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlOperacionF, CType(Session("dtOperacion"), Data.DataTable), "codigoOperacion", "Descripcion", False)
                ddlOperacionF.SelectedIndex = 0
                ddlCondicionF = CType(e.Row.FindControl("ddlCondicionF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlCondicionF, CType(Session("dtCondicion"), Data.DataTable), "Valor", "Nombre", False)
                ddlCondicionF.SelectedValue = "PL"
                ddlPlazaNF = CType(e.Row.FindControl("ddlPlazaNF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlPlazaNF, CType(Session("dtPlazaN"), Data.DataTable), "CodigoPlaza", "Descripcion", False)
                ddlPlazaNF.SelectedValue = "4"
                ddlContactoF = CType(e.Row.FindControl("ddlContactoF"), DropDownList)
                ddlContactoF.Items.Insert(0, New ListItem(DDL_ITEM_SELECCIONE, ""))
                ddlContactoF.SelectedIndex = 0
                ddlTipoFondoF = CType(e.Row.FindControl("ddlTipoFondoF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlTipoFondoF, CType(Session("dtTipoFondo"), Data.DataTable), "Valor", "Nombre", True, DDL_ITEM_SELECCIONE)
                ddlTipoFondoF.SelectedValue = "Normal"
                ddlTipoTramoF = CType(e.Row.FindControl("ddlTipoTramoF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlTipoTramoF, CType(Session("dtTipoTramo"), Data.DataTable), "Valor", "Nombre", True, DDL_ITEM_SELECCIONE)
                ddlTipoTramoF.SelectedValue = "AGENCIA"
                ddlMedioTransF = CType(e.Row.FindControl("ddlMedioTransF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlMedioTransF, CType(Session("dtMedioTrans"), Data.DataTable), "Valor", "Nombre", True, DDL_ITEM_SELECCIONE)
                ddlMedioTransF.SelectedValue = "S"

                'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 24/05/2018
                Dim ddlPortafolioF As DropDownList
                ddlPortafolioF = CType(e.Row.FindControl("ddlPortafolioF"), DropDownList)
                ddlPortafolioF.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, "0"))
                ddlPortafolioF.SelectedValue = "0"
                'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 24/05/2018

            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la grilla")
        End Try
    End Sub
    Public Function instanciarTabla(ByVal tabla As Data.DataTable) As Data.DataTable
        tabla = New Data.DataTable
        tabla.Columns.Add("CodigoPrevOrden")
        tabla.Columns.Add("CodigoPortafolio")
        tabla.Columns.Add("Asignacion")
        tabla.Columns.Add("Cambio")
        Return tabla
    End Function
    Protected Sub ibGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibGrabar.Click
        Try
            Dim sw As Integer = 0
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
            Dim bolValidaCampos As Boolean = False
            Dim strCambioTrazaFondo As String = ""
            Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
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
                chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)
                If chkSelect.Checked Then
                    If fila.Cells(2).Text = PREV_OI_INGRESADO Or Not dtDetalleInversiones Is Nothing Then
                        If (Not lbCodigoPrevOrden Is Nothing) Or Not dtDetalleInversiones Is Nothing And (fila.Cells(2).Text = PREV_OI_INGRESADO) Then
                            hdCambio.Value = ""
                            oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                            tbHora = CType(fila.FindControl("tbHora"), TextBox)
                            tbNemonico = CType(fila.FindControl("tbNemonico"), TextBox)
                            hdIntermediario = CType(fila.FindControl("hdIntermediario"), HtmlInputHidden)
                            ddlOperacion = CType(fila.FindControl("ddlOperacion"), DropDownList)
                            tbCantidad = CType(fila.FindControl("tbCantidad"), TextBox)
                            tbPrecio = CType(fila.FindControl("tbPrecio"), TextBox)
                            ddlCondicion = CType(fila.FindControl("ddlCondicion"), DropDownList)
                            ddlPlazaN = CType(fila.FindControl("ddlPlazaN"), DropDownList)
                            tbCantidadOperacion = CType(fila.FindControl("tbCantidadOperacion"), TextBox)
                            tbPrecioOperacion = CType(fila.FindControl("tbPrecioOperacion"), TextBox)
                            hdClaseInstrumento = CType(fila.FindControl("hdClaseInstrumento"), HtmlInputHidden)
                            ddlTipoFondo = CType(fila.FindControl("ddlTipoFondo"), DropDownList)
                            ddlTipoTramo = CType(fila.FindControl("ddlTipoTramo"), DropDownList)
                            ddlContacto = CType(fila.FindControl("ddlContacto"), DropDownList)
                            NroOper = fila.Cells(1).Text
                            ddlMedioTrans = CType(fila.FindControl("ddlMedioTrans"), DropDownList)
                            HdFondos = CType(fila.FindControl("Hdfondos"), HiddenField)
                            hdFechaNegocio = CType(fila.FindControl("hdFechaOperacion"), HiddenField)
                            ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(hdFechaNegocio.Value)
                            bolValidaCampos = False
                            If tbCantidad.Text <> "" And _
                                tbPrecio.Text <> "" And _
                                tbPrecioOperacion.Text <> "" And _
                                tbCantidadOperacion.Text <> "" And _
                                tbNemonico.Text <> "" And _
                                hdIntermediario.Value.ToString <> "" And _
                                Not ((hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOINVERSION) And _
                                     ddlTipoFondo.SelectedValue = DDL_ITEM_SELECCIONE) And _
                                Not (hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondo.SelectedValue = TIPOFONDO_ETF And _
                                     ddlTipoTramo.SelectedValue = DDL_ITEM_SELECCIONE) Then
                                If ValidarNemonico(tbNemonico.Text) And ValidarIntermediario(hdIntermediario.Value.ToString) Then
                                    If fila.Cells(2).Text <> PREV_OI_EJECUTADO And fila.Cells(2).Text <> PREV_OI_APROBADO And fila.Cells(2).Text <> PREV_OI_PENDIENTE Then
                                        bolValidaCampos = True
                                    End If
                                Else
                                    strMensaje = strMensaje + "- Validar Nemonico o Intermediario. \n"
                                End If

                                If Not ValidarSaldo(UIUtility.ConvertirFechaaDecimal(hdFechaNegocio.Value.ToString), tbNemonico.Text, HdFondos.Value.ToString, hdClaseInstrumento.Value.ToString, ddlOperacion.SelectedValue, IIf(IsNumeric(tbCantidad.Text), CDec(tbCantidad.Text), 0), 2, CType(lbCodigoPrevOrden.Text, Decimal)) Then
                                    strMensaje += "No existe el saldo suficiente para generar la pre-orden de inversión. \n"
                                    bolValidaCampos = False
                                End If
                            Else
                                If (hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOINVERSION) And _
                                    ddlTipoFondo.SelectedValue = DDL_ITEM_SELECCIONE Then
                                    strMensaje = strMensaje + "- Seleccione Tipo. \n"
                                End If
                                If hdClaseInstrumento.Value = CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondo.SelectedValue = TIPOFONDO_ETF And _
                                    ddlTipoTramo.SelectedValue = DDL_ITEM_SELECCIONE Then
                                    strMensaje = strMensaje + "- Seleccione Tipo Tramo. \n"
                                End If
                            End If
                            If bolValidaCampos = True Then
                                oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                                oRow.CodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                                oRow.HoraOperacion = Replace(Now.ToLongTimeString(), " a.m.", "")
                                oRow.CodigoNemonico = tbNemonico.Text
                                oRow.CodigoOperacion = ddlOperacion.SelectedValue
                                If hdClaseInstrumento.Value = CLASE_INSTRUMENTO_ACCION Then
                                    oRow.Cantidad = CType(tbCantidad.Text, Integer)
                                    oRow.CantidadOperacion = CType(tbCantidadOperacion.Text, Integer)
                                Else
                                    oRow.Cantidad = CType(tbCantidad.Text, Decimal)
                                    oRow.CantidadOperacion = CType(tbCantidadOperacion.Text, Decimal)
                                End If
                                oRow.Precio = CType(tbPrecio.Text, Decimal)
                                oRow.TipoCondicion = ddlCondicion.SelectedValue
                                oRow.CodigoTercero = hdIntermediario.Value
                                oRow.CodigoPlaza = ddlPlazaN.SelectedValue
                                oRow.IntervaloPrecio = -1
                                oRow.PrecioOperacion = CType(tbPrecioOperacion.Text, Decimal)
                                oRow.CodigoContacto = ddlContacto.SelectedValue
                                oRow.TipoFondo = IIf(ddlTipoFondo.SelectedValue = DDL_ITEM_SELECCIONE, "", ddlTipoFondo.SelectedValue)
                                oRow.TipoTramo = IIf(ddlTipoTramo.SelectedValue = DDL_ITEM_SELECCIONE, "", ddlTipoTramo.SelectedValue)
                                oRow.MedioNegociacion = IIf(ddlMedioTrans.SelectedValue = DDL_ITEM_SELECCIONE, "", ddlMedioTrans.SelectedValue)
                                oRow.HoraEjecucion = Replace(Now.ToLongTimeString(), " a.m.", "")
                                oRow.Porcentaje = porcentaje
                                oRow.FechaOperacion = ViewState("decFechaOperacion").ToString()
                                'cambio
                                'Dim sumaValidacion As Decimal
                                'If Not dtDetalleInversiones Is Nothing Then
                                '    Dim validarCodigoPrevOrden As String = ""
                                '    For i = 0 To dtDetalleInversiones.Rows.Count - 1
                                '        If dtDetalleInversiones.Rows(i)("CodigoPrevOrden").ToString.Trim = lbCodigoPrevOrden.Text.Trim Then
                                '            sumaValidacion = sumaValidacion + Decimal.Parse(dtDetalleInversiones.Rows(i)("Asignacion").ToString.Trim)
                                '            validarCodigoPrevOrden = lbCodigoPrevOrden.Text.Trim
                                '        End If
                                '    Next
                                '    If validarCodigoPrevOrden = lbCodigoPrevOrden.Text.Trim Then
                                '        If (CType(tbCantidadOperacion.Text, Decimal) <> sumaValidacion) And porcentaje = "N" Then
                                '            AlertaJS("La cantidad de Ejecución no es igual a la suma de las asignaciones de los fondos")
                                '            Exit Sub
                                '        End If
                                '    End If
                                'End If
                                oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                                oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                                If dtDetalleInversiones Is Nothing Then dtDetalleInversiones = instanciarTabla(dtDetalleInversiones)
                                '   Dim dtDetallePortafolio As DataTable = oPrevOrdenInversionBM.SeleccionarDetallePreOrdenInversion(CType(lbCodigoPrevOrden.Text.Trim, Decimal))
                                dtDetalleInversiones.Rows.Add(CType(lbCodigoPrevOrden.Text.Trim, Decimal), HdFondos.Value.ToString, CType(tbCantidad.Text, Decimal), "N")

                                If hdCambioTraza.Value = "1" Then
                                    hdCambioTraza.Value = ""
                                    hdNemonicoTrz = CType(fila.FindControl("hdNemonicoTrz"), HtmlInputHidden)
                                    hdOperacionTrz = CType(fila.FindControl("hdOperacionTrz"), HtmlInputHidden)
                                    hdCantidadTrz = CType(fila.FindControl("hdCantidadTrz"), HtmlInputHidden)
                                    hdPrecioTrz = CType(fila.FindControl("hdPrecioTrz"), HtmlInputHidden)
                                    hdIntermediarioTrz = CType(fila.FindControl("hdIntermediarioTrz"), HtmlInputHidden)
                                    hdCantidadOperacionTrz = CType(fila.FindControl("hdCantidadOperacionTrz"), HtmlInputHidden)
                                    hdPrecioOperacionTrz = CType(fila.FindControl("hdPrecioOperacionTrz"), HtmlInputHidden)

                                    hdFondo2Trz = CType(fila.FindControl("hdFondo2Trz"), HtmlInputHidden)
                                    hdFondo3Trz = CType(fila.FindControl("hdFondo3Trz"), HtmlInputHidden)
                                    tbIntermediario = CType(fila.FindControl("tbIntermediario"), TextBox)
                                    If hdCambioTrazaFondo.Value = "1" Then
                                        strCambioTrazaFondo = "1"
                                    End If
                                    If dtDetalleInversiones Is Nothing Then
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
                                        oRowT.CodigoPortafolio = ""
                                        oRowT.Asignacion = 0
                                        oRowT.ModoIngreso = MODO_ING_MASIVO
                                        oRowT.Proceso = _PROCESO_TRAZA.Grabar
                                        oRow.Porcentaje = porcentaje
                                        oRowT.MotivoCambio = MOTIVO_MODIFICAR_TRAZA
                                        oRowT.Comentarios = IIf(strCambioTrazaFondo = "1", COMENTARIO_MODIFICA_FONDO, COMENTARIO_MODIFICA_TRAZA)
                                        oTrazabilidadOperacionBE.TrazabilidadOperacion.AddTrazabilidadOperacionRow(oRowT)
                                        oTrazabilidadOperacionBE.TrazabilidadOperacion.AcceptChanges()
                                    Else
                                        For i = 0 To dtDetalleInversiones.Rows.Count - 1
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
                                            oRowT.CodigoPortafolio = dtDetalleInversiones.Rows(i)("CodigoPortafolio").ToString.Trim
                                            oRowT.Asignacion = Decimal.Parse(dtDetalleInversiones.Rows(i)("Asignacion").ToString.Trim)
                                            oRowT.ModoIngreso = MODO_ING_MASIVO
                                            oRowT.Proceso = _PROCESO_TRAZA.Grabar
                                            oRow.Porcentaje = porcentaje
                                            oRowT.MotivoCambio = MOTIVO_MODIFICAR_TRAZA
                                            oRowT.Comentarios = IIf(strCambioTrazaFondo = "1", COMENTARIO_MODIFICA_FONDO, COMENTARIO_MODIFICA_TRAZA)
                                            oTrazabilidadOperacionBE.TrazabilidadOperacion.AddTrazabilidadOperacionRow(oRowT)
                                            oTrazabilidadOperacionBE.TrazabilidadOperacion.AcceptChanges()
                                        Next
                                    End If
                                End If
                            Else
                                If strMensaje <> "" Then
                                    AlertaJS("Nro. Operación " + NroOper + ":\n" + strMensaje)
                                    Exit For
                                End If
                            End If
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
                            oPrevOrdenInversionBM.insertarDetalle(Integer.Parse(dtDetalleInversiones.Rows(i)("CodigoPrevOrden").ToString.Trim), _
                            dtDetalleInversiones.Rows(i)("CodigoPortafolio").ToString.Trim, Decimal.Parse(dtDetalleInversiones.Rows(i)("Asignacion").ToString.Trim))
                        Next
                    End If
                End If
                oRow = Nothing
                If oTrazabilidadOperacionBE.TrazabilidadOperacion.Rows.Count > 0 Then
                    oPrevOrdenInversionBM.InsertarTrazabilidad_sura(oTrazabilidadOperacionBE, PROCESO_TRAZA1, DatosRequest)
                    oRowT = Nothing
                End If
                CargarGrilla(TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), _
                ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(CType(ViewState("decFechaOperacion"), String))
                AlertaJS("Grabación exitosa")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        Finally
            ibGrabar.Text = "Grabar"
            ibGrabar.Enabled = True
        End Try
    End Sub
    Protected Sub ibValidar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibValidar.Click
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
                If Not lbCodigoPrevOrden Is Nothing Then
                    'PROCESAR VALIDACION
                    chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)
                    Dim decCodigoPrevOrden As Decimal
                    If chkSelect.Checked = True Then
                        decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                        oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                        count = count + 1
                        strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text & "|"
                    End If
                End If
            Next
            'SE PROCEDE A VALIDAR
            If count > 0 Then
                VerificaExcesoLimites(TR_RENTA_VARIABLE.ToString(), decNProceso)
                Dim dt As New Data.DataTable
                dt = oPrevOrdenInversionBM.SeleccionarValidacionExcesos(TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), DatosRequest, decNProceso).Tables(0)
                If dt.Rows.Count > 0 Then
                    Session("dtListaExcesos") = dt
                    EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" + TR_RENTA_VARIABLE.ToString() + "', '650', '450','');")
                End If
            Else
                AlertaJS("Seleccione los registros a validar!")
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
            arrCodPrevOrden = strCodPrevOrden.Split("|")
            For i = 0 To arrCodPrevOrden.Length - 1
                If arrCodPrevOrden(i) <> "" Then
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                End If
            Next
            'OT-10784 Fin
        End Try
    End Sub
    Protected Sub ibAprobar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibAprobar.Click
        'EJECUCION DE ORDENES DE INVERSION
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
                chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)
                lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                Dim fondos As String = CType(fila.FindControl("ddlfondos"), DropDownList).SelectedItem.ToString
                tFechaOperacion = CType(fila.FindControl("tFechaOperacion"), TextBox)
                HdFondos = CType(fila.FindControl("Hdfondos"), HiddenField)
                Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio(HdFondos.Value.Trim)
                Dim decCodigoPrevOrden As Decimal
                If chkSelect.Checked = True Then
                    If fila.Cells(2).Text = "APR" Then
                        If UIUtility.ConvertirFechaaDecimal(tFechaOperacion.Text.Trim) <> fechaNegocio Then
                            contFechaDiferenteNegocio += 1
                            strMensajeDiferenteFechaNegocio += "-> N°: " + fila.Cells(1).Text + " / Portafolio: " + fondos + _
                                                              "<br> F. Operación: " + tFechaOperacion.Text.Trim + _
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
                Dim bolUpdGrilla As Boolean = False
                If (strCodPrevOrden.Length > 0) Then
                    strCodPrevOrden = strCodPrevOrden.Substring(0, strCodPrevOrden.Length - 1)
                End If
                EjecutarOrdenInversion(TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), strCodPrevOrden, bolUpdGrilla, , decNProceso)
                If bolUpdGrilla Then
                    CargarGrilla(TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), _
                    ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                End If
            Else
                AlertaJS("Seleccione el registro a ejecutar! ó el registro ya se encontraba ejecutado!")
                CargarGrilla(TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), _
                    ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso) 'Se eliminan todos los procesos 'OT-10784
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
            ibAprobar.Text = "Ejecutar"
            ibAprobar.Enabled = True
        End Try
    End Sub
    Protected Sub ibExportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibExportar.Click
        Try
            GenerarReporteRentaVariable()
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub ibValidarTrader_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibValidarTrader.Click
        'VALIDACION DE EXCESOS POR TRADER
        'crear variables globales
        Dim dtValidaTrader As New Data.DataTable
        Dim ds As New DataSet
        Dim oLimiteTradingBM As New LimiteTradingBM
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim decNProceso As Decimal = 0
        Dim strCodPrevOrden As String = ""
        Dim arrCodPrevOrden As Array
        Dim strEstadoPrevOrden As String = ""
        Try
            'inserto el proceso masivo
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            'Se declara un contador
            Dim count As Decimal = 0, contFechaDiferenteNegocio As Integer = 0, strMensajeDiferenteFechaNegocio As String = "<p align=left>"
            'Se recorre un bucle en la grilla
            For Each fila As GridViewRow In Datagrid1.Rows
                chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)
                lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                Dim fondos As String = CType(fila.FindControl("ddlfondos"), DropDownList).SelectedItem.ToString
                tFechaOperacion = CType(fila.FindControl("tFechaOperacion"), TextBox)
                HdFondos = CType(fila.FindControl("Hdfondos"), HiddenField)
                Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio(HdFondos.Value.Trim)
                Dim decCodigoPrevOrden As Decimal
                strEstadoPrevOrden = oPrevOrdenInversionBM.ObtenerEstadoPrevOrdenInversion(CType(lbCodigoPrevOrden.Text, Decimal), ds) 'agregado por JH , para no generar duplicados
                If chkSelect.Checked = True Then

                    If fila.Cells(2).Text = "ING" Then
                        If UIUtility.ConvertirFechaaDecimal(tFechaOperacion.Text.Trim) <> fechaNegocio Then
                            contFechaDiferenteNegocio += 1
                            strMensajeDiferenteFechaNegocio += "-> N°: " + fila.Cells(1).Text + " / Portafolio: " + fondos + _
                                                              "<br> F. Operación: " + tFechaOperacion.Text.Trim + _
                                                              " / F. Portafolio: " + UIUtility.ConvertirFechaaString(fechaNegocio) + "<br><br>"
                        Else
                            decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                            'Ejecutar limites trader
                            oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                            If count = 0 Then
                                strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text
                            Else
                                strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text & "|"
                            End If
                            count = count + 1
                        End If
                    End If
                End If
            Next
            'Si la cantidad es mayor que cero
            If contFechaDiferenteNegocio > 0 Then
                AlertaJS("Verificar las siguiente negociaciones, las fecha de operación no coincide con la fecha negocio del portafolio: " + _
                         strMensajeDiferenteFechaNegocio + "</p>")
            ElseIf count > 0 Then
                'Valida excesos límite trader
                dtValidaTrader = oLimiteTradingBM.SeleccionarValidacionExcesosTrader_Sura(TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), _
                Usuario, DatosRequest, , decNProceso).Tables(0)
                If dtValidaTrader.Rows.Count > 0 Then
                    Session("dtValidaTrader") = dtValidaTrader
                    EjecutarJS("showModalDialog('frmValidacionExcesosTrader.aspx?TipoRenta=" & TR_RENTA_VARIABLE.ToString() & "&nProc=" & _
                    decNProceso.ToString() + "', '650', '450','" & btnBuscar.ClientID & "');")
                Else
                    AlertaJS("No se han podido evaluar los limites trader.")
                    'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso) 'OT-10784
                End If
            Else
                AlertaJS("Seleccione el registro a validar ó el registro ya se encontraba aprobado!")
                CargarGrilla(ParametrosSIT.TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                'AlertaJS("Seleccione los registros a validar!")
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
            ibValidarTrader.Text = "Validar Exec.Trader"
            ibValidarTrader.Enabled = True
        End Try
    End Sub
    Protected Sub Datagrid1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Datagrid1.PageIndexChanging
        Datagrid1.PageIndex = e.NewPageIndex
        CargarGrilla(TR_RENTA_VARIABLE.ToString(), ViewState("decFechaOperacion"))
    End Sub
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
End Class
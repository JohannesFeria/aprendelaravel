Option Explicit On
Option Strict On
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office

Partial Class Modulos_Inversiones_frmCarteraForward
    Inherits BasePage

#Region "Eventos"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        rblEscenario.Visible = True
        lblFechaDsc1.InnerText = "Fecha"
        divFechaDsc1.Attributes.Add("class", "col-md-3")
        divTipoMercado.Attributes.Add("class", "col-md-3")

        If Not Page.IsPostBack Then
            Try
                Dim oUtil As New UtilDM
                CargarCombos()
                tbFechaValoracion.Text = oUtil.RetornarFechaNegocio

            Catch ex As Exception
                AlertaJS(Replace(ex.Message.ToString(), "'", ""))
            End Try

        End If

    End Sub

    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Dim decFechaDesde As Decimal = 0
        Dim strPortafolio As String = String.Empty
        Dim strEscenario As String = String.Empty
        Dim ds As DataSet
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Dim objOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM()
        Dim objReporteGestionDAM As New ReporteGestionBM()

        Try
            If Trim(tbFechaValoracion.Text) = "" Then
                AlertaJS(Constantes.M_STR_MENSAJE_FECHA_VACIA)
                Exit Sub
            End If

            decFechaDesde = UIUtility.ConvertirFechaaDecimal(Me.tbFechaValoracion.Text)
            strPortafolio = CStr(IIf(ddlportafolio.SelectedValue.ToString = "Todos", "", ddlportafolio.SelectedValue.ToString))
            strEscenario = rblEscenario.SelectedValue

            ds = objOrdenPreOrdenInversionBM.Reporte_InventarioForward_Fecha_Rango(strPortafolio, decFechaDesde, decFechaDesde, DatosRequest)
            If ds.Tables.Count > 0 Then
                dt1 = ds.Tables(0)
            End If

            'ds = objOrdenPreOrdenInversionBM.Reporte_Gestion_CompCartera_Fecha_Rango(strPortafolio, strEscenario, decFechaDesde, decFechaHasta, DatosRequest)
            'dt2 = ds.Tables(0)
            ds = objReporteGestionDAM.ComposicionCartera(decFechaDesde, strEscenario, strPortafolio, DatosRequest)
            If ds.Tables.Count > 0 Then
                dt2 = ds.Tables(0)
            End If

            ds = objOrdenPreOrdenInversionBM.Reporte_VectorTipoCambio_Fecha_Rango(decFechaDesde, decFechaDesde, DatosRequest)
            If ds.Tables.Count > 0 Then
                dt3 = ds.Tables(0)
            End If

            ds = objOrdenPreOrdenInversionBM.Reporte_Gestion_MarkToMarkedFW_Fecha_Rango(decFechaDesde, strEscenario, strPortafolio, DatosRequest)
            If ds.Tables.Count > 0 Then
                dt4 = ds.Tables(0)
            End If

            ExportarExcel(dt1, dt2, dt3, dt4)

        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))

        Finally
            GC.Collect()

        End Try

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click

        Try
            Response.Redirect("~/frmDefault.aspx")

        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))

        End Try

    End Sub

#End Region

#Region "Metodos"

    Private Sub LimpiarControles()

    End Sub

    Private Sub CargarGrilla()

    End Sub

    Public Sub CargarCombos()
        Dim oPortafolioBM As New PortafolioBM

        ddlportafolio.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlportafolio.DataValueField = "CodigoPortafolio"
        ddlportafolio.DataTextField = "Descripcion"
        ddlportafolio.DataBind()
        ddlportafolio.Items.Insert(0, New ListItem("Todos"))

    End Sub

    ''' <summary>
    ''' ExportarExcel
    ''' </summary>
    ''' <param name="dt1"></param>
    ''' <param name="dt2"></param>
    ''' <remarks>
    ''' 2015-11-19        Herbert Mendoza              Creacion
    ''' </remarks>
    Private Sub ExportarExcel(ByVal dt1 As DataTable, _
                              ByVal dt2 As DataTable, _
                              ByVal dt3 As DataTable, _
                              ByVal dt4 As DataTable)
        'OT10689 - Inicio. Kill process excel
        Dim strRutaSIT As String = String.Empty
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oCells As Excel.Range = Nothing
        Dim strRutaPlantilla As String = String.Empty
        Dim strNombreArchivo As String = String.Empty
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            strRutaSIT = CStr(New ParametrosGeneralesBM().Listar(Constantes.ParametrosGenerales.RUTA_TEMP, DatosRequest).Rows(0)("Valor"))
            strRutaPlantilla = RutaPlantillas() & "\PlantillaCarteraForward.xls"
            strNombreArchivo = "ReporteCarteraForward_" & Usuario.ToString() & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & System.DateTime.Now.ToString("_HHmmss") & ".xls"

            strRutaSIT = strRutaSIT + strNombreArchivo

            If File.Exists(strRutaPlantilla) = True Then
                'Exportación a Excel
                oExcel.DisplayAlerts = False
                oExcel.Visible = False
                'Start a new workbook
                oBooks = oExcel.Workbooks
                oBooks.Open(strRutaPlantilla) 'Load colorful template with chart
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                'oSheet.Name = "Inventario Forward"
                oCells = oSheet.Cells

                DumpData(dt1, dt2, dt3, dt4, oBook)

                oSheet.SaveAs(strRutaSIT)
                oBook.Save()
                oBook.Close()
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strNombreArchivo)
                Response.WriteFile(strRutaSIT)
                Response.End()
            Else
                AlertaJS(Constantes.M_STR_MENSAJE_NO_SE_ENCONTRO_ARCHIVO_RUTA + strRutaPlantilla)
            End If

        Catch ex As Exception
            Throw ex
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

    ''' <summary>
    ''' DumpData
    ''' </summary>
    ''' <param name="dt1"></param>
    ''' <param name="dt2"></param>
    ''' <param name="oBook"></param>
    ''' <remarks>
    ''' 2015-11-19        Herbert Mendoza              Creacion
    ''' </remarks>
    Private Sub DumpData(ByVal dt1 As DataTable, _
                         ByVal dt2 As DataTable, _
                         ByVal dt3 As DataTable, _
                         ByVal dt4 As DataTable, _
                         ByVal oBook As Excel.Workbook)
        Dim iRow As Integer = 0, iCol As Integer = 0, nFilIni As Long = 0, nFilFin As Long = 0, nPFil As Long = 0
        Dim dr As DataRow
        Dim ary() As Object

        For Each sheet As Excel.Worksheet In oBook.Sheets()
            Select Case sheet.Name
                Case Constantes.HojaExcel.InventarioForward
                    iRow = 0
                    iCol = 0
                    nPFil = 0
                    nFilIni = 8
                    nFilFin = 0

                    sheet.Cells.Range("C1").Value = DateTime.Now.ToString(Constantes.Pantalla.FormatoFecha.ddMMyyyy_01)
                    sheet.Cells.Range("C2").Value = ddlportafolio.SelectedItem.Text
                    sheet.Cells.Range("C3").Value = tbFechaValoracion.Text

                    For iRow = 0 To dt1.Rows.Count - 1

                        dr = dt1.Rows.Item(iRow)
                        nPFil = iRow + nFilIni

                        sheet.Cells.Range("A" & nPFil).Value = dt1.Rows(iRow)("Fila")
                        sheet.Cells.Range("B" & nPFil).Value = dt1.Rows(iRow)("Valor")
                        sheet.Cells.Range("C" & nPFil).Value = dt1.Rows(iRow)("CodEmisor")
                        sheet.Cells.Range("D" & nPFil).Value = dt1.Rows(iRow)("Emisor")
                        sheet.Cells.Range("E" & nPFil).Value = dt1.Rows(iRow)("CodigoMoneda")
                        sheet.Cells.Range("F" & nPFil).Value = dt1.Rows(iRow)("Moneda")
                        sheet.Cells.Range("G" & nPFil).Value = dt1.Rows(iRow)("CodigoRef")
                        sheet.Cells.Range("H" & nPFil).Value = dt1.Rows(iRow)("HoraNegociacion")
                        sheet.Cells.Range("I" & nPFil).Value = dt1.Rows(iRow)("CodigoMonedaVenta")
                        sheet.Cells.Range("J" & nPFil).Value = dt1.Rows(iRow)("MonedaVenta")
                        sheet.Cells.Range("K" & nPFil).Value = dt1.Rows(iRow)("IndicadorMovimiento")
                        sheet.Cells.Range("L" & nPFil).Value = dt1.Rows(iRow)("TipoForward")
                        sheet.Cells.Range("N" & nPFil).Value = dt1.Rows(iRow)("MontoForward")
                        sheet.Cells.Range("O" & nPFil).Value = dt1.Rows(iRow)("PrecioTransaccion")
                        sheet.Cells.Range("P" & nPFil).Value = dt1.Rows(iRow)("FechaEmision")
                        sheet.Cells.Range("Q" & nPFil).Value = dt1.Rows(iRow)("FechaVencimiento")
                        sheet.Cells.Range("R" & nPFil).Value = dt1.Rows(iRow)("FechaLiquidacion")
                        sheet.Cells.Range("T" & nPFil).Value = dt1.Rows(iRow)("Modalidad")
                        sheet.Cells.Range("V" & nPFil).Value = dt1.Rows(iRow)("TipoCambioForwardPactado")
                        sheet.Cells.Range("W" & nPFil).Value = dt1.Rows(iRow)("IndicadorCaja")
                        sheet.Cells.Range("X" & nPFil).Value = dt1.Rows(iRow)("PlazaNegociacion")
                        sheet.Cells.Range("Y" & nPFil).Value = dt1.Rows(iRow)("Descripcion")
                        sheet.Cells.Range("Z" & nPFil).Value = dt1.Rows(iRow)("Fixing")
                        sheet.Cells.Range("AA" & nPFil).Value = dt1.Rows(iRow)("MonedaLiquida")
                        sheet.Cells.Range("AB" & nPFil).Value = dt1.Rows(iRow)("MontoCalculado")
                        sheet.Cells.Range("AC" & nPFil).Value = dt1.Rows(iRow)("NumeroContrato")
                        sheet.Cells.Range("AD" & nPFil).Value = dt1.Rows(iRow)("Estado")
                        sheet.Cells.Range("AE" & nPFil).Value = dt1.Rows(iRow)("Mercado")
                        nFilFin = nPFil
                    Next iRow

                    If dt1.Rows.Count > 1 Then
                        sheet.Cells.Range("M8").Copy()
                        sheet.Cells.Range("M9:M" & CStr(nFilFin)).Select()
                        sheet.Paste()
                        sheet.Cells.Range("S8").Copy()
                        sheet.Cells.Range("S9:S" & CStr(nFilFin)).Select()
                        sheet.Paste()
                        sheet.Cells.Range("U8").Copy()
                        sheet.Cells.Range("U9:U" & CStr(nFilFin)).Select()
                        sheet.Paste()
                        sheet.Application.CutCopyMode = CType(False, Excel.XlCutCopyMode)

                        sheet.Cells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
                        If nFilIni < nFilFin Then
                            sheet.Cells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous
                            sheet.Cells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous
                        End If

                        sheet.Cells.Range("A7").Select()
                    End If

                Case Constantes.HojaExcel.ComposicionCartera
                    iRow = 0
                    iCol = 0
                    nPFil = 0
                    nFilIni = 2
                    nFilFin = 0

                    For iCol = 0 To dt2.Columns.Count - 1
                        sheet.Cells(1, iCol + 1) = dt2.Columns(iCol).ToString
                    Next
                    For iRow = 0 To dt2.Rows.Count - 1
                        dr = dt2.Rows.Item(iRow)
                        nPFil = iRow + nFilIni
                        ary = dr.ItemArray
                        For iCol = 0 To UBound(ary)
                            sheet.Cells(iRow + 2, iCol + 1) = ary(iCol).ToString
                        Next
                        nFilFin = iRow + nFilIni
                    Next

                    If dt2.Rows.Count > 1 Then
                        sheet.Cells.Range("A" & nFilIni.ToString & ":Y" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":Y" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":Y" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":Y" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous

                        If nFilIni < nFilFin Then
                            sheet.Cells.Range("A" & nFilIni.ToString & ":Y" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous
                            sheet.Cells.Range("A" & nFilIni.ToString & ":Y" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous
                        End If

                    End If

                    sheet.Name = Constantes.HojaExcel.ComposicionCartera & Space(1) & rblEscenario.SelectedValue

                Case Constantes.HojaExcel.TipoCambio
                    iRow = 0
                    iCol = 0
                    nPFil = 0
                    nFilIni = 2

                    For iRow = 0 To dt3.Rows.Count - 1
                        nPFil = iRow + nFilIni

                        sheet.Cells.Range("A" & nPFil).Value = dt3.Rows(iRow)("Moneda")
                        sheet.Cells.Range("B" & nPFil).Value = dt3.Rows(iRow)("TipoCalculo")
                        sheet.Cells.Range("C" & nPFil).Value = dt3.Rows(iRow)("Fecha")
                        sheet.Cells.Range("D" & nPFil).Value = dt3.Rows(iRow)("ValorPrimario")

                        nFilFin = nPFil
                    Next

                    If dt2.Rows.Count > 1 Then
                        sheet.Cells.Range("A" & nFilIni.ToString & ":D" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":D" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":D" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":D" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous

                        If nFilIni < nFilFin Then
                            sheet.Cells.Range("A" & nFilIni.ToString & ":D" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous
                            sheet.Cells.Range("A" & nFilIni.ToString & ":D" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous
                        End If

                    End If

                Case Constantes.HojaExcel.MarkToMarketFW
                    iRow = 0
                    iCol = 0
                    nPFil = 0
                    nFilIni = 2
                    nFilFin = 0

                    For iCol = 0 To dt4.Columns.Count - 1
                        sheet.Cells(1, iCol + 1) = dt4.Columns(iCol).ToString
                    Next
                    For iRow = 0 To dt4.Rows.Count - 1
                        dr = dt4.Rows.Item(iRow)
                        nPFil = iRow + nFilIni
                        ary = dr.ItemArray
                        For iCol = 0 To UBound(ary)
                            sheet.Cells(iRow + 2, iCol + 1) = ary(iCol).ToString
                        Next
                        nFilFin = iRow + nFilIni
                    Next

                    If dt4.Rows.Count > 1 Then
                        sheet.Cells.Range("A" & nFilIni.ToString & ":M" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":M" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":M" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":M" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous

                        If nFilIni < nFilFin Then
                            sheet.Cells.Range("A" & nFilIni.ToString & ":M" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous
                            sheet.Cells.Range("A" & nFilIni.ToString & ":M" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous
                        End If

                    End If

                    sheet.Name = Constantes.HojaExcel.MarkToMarketFW & Space(1) & rblEscenario.SelectedValue

            End Select

        Next

    End Sub

#End Region

End Class

Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports System.Globalization
Imports System.Threading
Imports System.IO

Partial Class Modulos_Gestion_Reportes_frmReportesCalculoInteres
    Inherits BasePage

    'Dim fecha As String
    'Dim ds As DataTable
    Dim dtUltimoDiaMes As DateTime

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnImprimir.Attributes.Add("onclick", "javascript:return Validar();")

        If Not Page.IsPostBack Then
            dllAnio.Items.Insert(0, New ListItem(Date.Now.Year, Date.Now.Year))
            dllAnio.Items.Insert(1, New ListItem(Date.Now.Year - 1, Date.Now.Year - 1))
            dllAnio.Items.Insert(2, New ListItem(Date.Now.Year - 2, Date.Now.Year - 2))
            dllAnio.Items.Insert(3, New ListItem(Date.Now.Year - 3, Date.Now.Year - 3))
            dllAnio.Items.Insert(4, New ListItem(Date.Now.Year - 4, Date.Now.Year - 4))
            dllAnio.Items.Insert(5, New ListItem(Date.Now.Year - 5, Date.Now.Year - 5))
        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim ds, dsCD, dsFM As DataSet
        Try
            Dim _ReporteCalculoInteresBM As New ReporteCalculoInteresBM
            dtUltimoDiaMes = New DateTime(dllAnio.SelectedValue, ddlMes.SelectedValue, 1).AddMonths(1).AddDays(-1)

            ds = _ReporteCalculoInteresBM.CalculoInteres(dllAnio.SelectedValue + ddlMes.SelectedValue + "01", dtUltimoDiaMes.ToString("yyyyMMdd"), DatosRequest)
            dsCD = _ReporteCalculoInteresBM.CertificadoDeposito(dllAnio.SelectedValue + ddlMes.SelectedValue + "01", dtUltimoDiaMes.ToString("yyyyMMdd"), DatosRequest)
            dsFM = _ReporteCalculoInteresBM.FondosMutuos(dllAnio.SelectedValue + ddlMes.SelectedValue + "01", dtUltimoDiaMes.ToString("yyyyMMdd"), DatosRequest)
            Copia("Calculo Interes DPZ", ds, CType(dsCD.Tables(0), DataTable), CType(dsFM.Tables(0), DataTable))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Public Sub Copia(ByVal Archivo As String, ByVal ds As DataSet, ByVal dsCD As DataTable, ByVal dtFM As DataTable)
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets
        Dim oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Dim sFile As String, sTemplate As String
        Dim sufijo As String = String.Empty
        Dim oldCulture As CultureInfo
        oldCulture = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Archivo & "_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            sTemplate = RutaPlantillas() & "\PlantillaCalInt.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False

            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Name = Archivo
            oCells = oSheet.Cells
            DumpData(ds, oCells, dsCD)
            oSheet.SaveAs(sFile)

            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(2), Excel.Worksheet)
            oSheet.Name = "Fondos Mutuos"
            oCells = oSheet.Cells
            DumpDataFondosMutuos(dtFM, oCells)
            oSheet.SaveAs(sFile)
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(sFile))
            Response.WriteFile(sFile)
            Response.End()

        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
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

    Private Sub DumpDataFondosMutuos(ByVal dtFM As DataTable, ByVal oCells As Excel.Range)
        Dim dr As DataRow
        Dim iRow, x, iExcel As Integer
        Dim rango As String

        x = 0
        hidFM.Value = ""
        hidFMncuota.Value = ""
        hidInteres1.Value = ""
        hidInteres2.Value = ""
        hidSumaD.Value = ""
        hidSumaF.Value = ""
        hidSumaH.Value = ""
        hidSumaJ.Value = ""

        If dtFM.Rows.Count > 0 Then
            For iRow = 0 To dtFM.Rows.Count - 1
                dr = dtFM.Rows.Item(iRow)

                oCells(iExcel + 3, 2) = dr("Descripcion")
                oCells(iExcel + 4, 2) = "AL " & dtUltimoDiaMes.ToString("dd.MM.yyyy")
                oCells(iExcel + 6, 2) = "Valor Nominal"
                oCells(iExcel + 6, 3) = "N Cuotas"
                oCells(iExcel + 6, 4) = "Intereses"

                oCells(iExcel + 7, 4) = dr("anterior").ToString().Replace("/", ".")
                oCells(iExcel + 7, 6) = dtUltimoDiaMes.ToString("dd.MM.yyyy")
                oCells(iExcel + 8, 4) = "V.C"
                oCells(iExcel + 8, 5) = dr("ValorCuotaAnterior")
                oCells(iExcel + 8, 6) = "V.C"
                oCells(iExcel + 8, 7) = dr("ValorCuotaActual")

                oCells(iExcel + 9, 2) = dr("Valor Nominal")
                oCells(iExcel + 9, 3) = dr("No cuotas")
                oCells(iExcel + 9, 4) = dr("Interes Anterior")
                oCells(iExcel + 9, 6) = dr("Interes Actual")
                oCells(iExcel + 9, 2) = dr("Valor Nominal")

                If dtFM.Rows.Count > 1 And dtFM.Rows.Count <> iRow + 1 Then
                    oCells.Range("B3:G9").Copy()
                    rango = String.Concat("B", iExcel + 16, ":B", iExcel + 16)
                    oCells.Range(rango).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)
                End If
                iExcel = iExcel + 13
            Next
        End If
    End Sub

    Private Sub DumpData(ByVal ds As DataSet, ByVal oCells As Excel.Range, ByVal dtCD As DataTable)

        Dim dr As DataRow, ary() As Object
        Dim iRow, iRowCD, iCol, iCol2, x As Integer
        Dim fecha As String
        Dim dt As DataTable = ds.Tables(0) 'RGF 20090807
        x = 0

        hiRow.Value = ""
        hiRowDolares.Value = "" 'RGF 20090807
        If dt.Rows.Count > 0 Then
            oCells(1, 1) = "CALCULO DE INTERESES DE DPZ AL " & dt.Rows(0).Item(6) 'RGF 20090807

            For iRow = 0 To dt.Rows.Count - 1
                dr = dt.Rows.Item(iRow)
                ary = dr.ItemArray

                For iCol = 0 To UBound(ary)
                    If iCol = 0 Then
                        If fecha <> dt.Rows(iRow).Item(0) Then
                            For iCol2 = 0 To dt.Columns.Count - 1
                                If iCol2 <> 0 Then
                                    If iCol2 = 1 Then
                                        oCells.Cells(iRow + 6 + x - 1, iCol2).Font.Bold = True
                                        oCells(iRow + 6 + x - 1, iCol2) = UIUtility.ConvertirFechaaString(dt.Rows(iRow).Item(0))
                                        oCells(iRow + 6 + x, 1).Borders.LineStyle = 1
                                        oCells(iRow + 6 + x, 2).Borders.LineStyle = 1
                                        oCells(iRow + 6 + x, 3).Borders.LineStyle = 1
                                        oCells(iRow + 6 + x, 4).Borders.LineStyle = 1
                                        oCells(iRow + 6 + x, 5).Borders.LineStyle = 1
                                        oCells(iRow + 6 + x, 6).Borders.LineStyle = 1
                                        oCells(iRow + 6 + x, 7).Borders.LineStyle = 1
                                        oCells(iRow + 6 + x, 8).Borders.LineStyle = 1
                                        oCells(iRow + 6 + x, 9).Borders.LineStyle = 1
                                    End If
                                    oCells.Cells(iRow + 6 + x, iCol2).Font.Bold = True
                                    oCells(iRow + 6 + x, iCol2) = dt.Columns(iCol2).ToString
                                    oCells(iRow + 6 + x, 5).Interior.ColorIndex = 34
                                End If
                            Next
                            fecha = dt.Rows(iRow).Item(0)
                        Else
                            x = x - 3
                        End If
                    End If
                    If iCol <> 0 Then
                        Select Case iCol
                            Case 7
                                oCells(iRow + 7 + x, iCol) = "=((1+E" & (iRow + 7 + x) & ")^(D" & (iRow + 7 + x) & "/360))-1"
                            Case 8
                                oCells(iRow + 7 + x, iCol) = "=B" & (iRow + 7 + x) & "*G" & (iRow + 7 + x)
                            Case 9
                                oCells(iRow + 7 + x, iCol) = "=B" & (iRow + 7 + x) & "+H" & (iRow + 7 + x)
                            Case Else
                                oCells(iRow + 7 + x, iCol) = ary(iCol).ToString
                        End Select
                        oCells(iRow + 7 + x, 5).Interior.ColorIndex = 34
                    End If
                Next
                If dt.Rows(iRow)("DPZ").ToString().ToLower().Equals("soles") Then
                    hiRow.Value = hiRow.Value & "+H" & iRow + 6 + x + 1
                Else
                    hiRowDolares.Value = hiRowDolares.Value & "+H" & iRow + 6 + x + 1
                End If
                x = x + 3
            Next
        End If

        x = x + iRow

        If dtCD.Rows.Count > 0 Then
            For iRowCD = 0 To dtCD.Rows.Count - 1
                dr = dtCD.Rows.Item(iRowCD)
                ary = dr.ItemArray
                For iCol = 0 To UBound(ary)
                    If iCol = 0 Then
                        If fecha <> dtCD.Rows(iRowCD).Item(0) Then
                            For iCol2 = 0 To dtCD.Columns.Count - 1
                                If iCol2 <> 0 Then
                                    If iCol2 = 1 Then
                                        oCells.Cells(iRowCD + 6 + x - 1, iCol2).Font.Bold = True
                                        oCells(iRowCD + 6 + x - 1, iCol2) = UIUtility.ConvertirFechaaString(dtCD.Rows(iRowCD).Item(0))
                                        oCells(iRowCD + 6 + x, 1).Borders.LineStyle = 1
                                        oCells(iRowCD + 6 + x, 2).Borders.LineStyle = 1
                                        oCells(iRowCD + 6 + x, 3).Borders.LineStyle = 1
                                        oCells(iRowCD + 6 + x, 4).Borders.LineStyle = 1
                                        oCells(iRowCD + 6 + x, 5).Borders.LineStyle = 1
                                        oCells(iRowCD + 6 + x, 6).Borders.LineStyle = 1
                                        oCells(iRowCD + 6 + x, 7).Borders.LineStyle = 1
                                        oCells(iRowCD + 6 + x, 8).Borders.LineStyle = 1
                                        oCells(iRowCD + 6 + x, 9).Borders.LineStyle = 1
                                    End If
                                    oCells.Cells(iRowCD + 6 + x, iCol2).Font.Bold = True
                                    oCells(iRowCD + 6 + x, iCol2) = dtCD.Columns(iCol2).ToString
                                    oCells(iRowCD + 6 + x, 5).Interior.ColorIndex = 34
                                End If
                            Next
                            fecha = dtCD.Rows(iRowCD).Item(0)
                        Else
                            x = x - 3
                        End If
                    End If
                    If iCol <> 0 Then
                        Select Case iCol
                            Case 7
                                oCells(iRowCD + 7 + x, iCol) = "=((1+E" & (iRowCD + 7 + x) & ")^(D" & (iRowCD + 7 + x) & "/360))-1"
                            Case 8
                                oCells(iRowCD + 7 + x, iCol) = "=B" & (iRowCD + 7 + x) & "*G" & (iRowCD + 7 + x)
                            Case 9
                                oCells(iRowCD + 7 + x, iCol) = "=B" & (iRowCD + 7 + x) & "+H" & (iRowCD + 7 + x)
                            Case Else
                                oCells(iRowCD + 7 + x, iCol) = ary(iCol).ToString
                        End Select
                        oCells(iRowCD + 7 + x, 5).Interior.ColorIndex = 34
                    End If
                Next
                If dtCD.Rows(iRowCD)("CD").ToString().ToLower().Equals("soles") Then
                    hiRow.Value = hiRow.Value & "+H" & iRowCD + 6 + x + 1
                Else
                    hiRowDolares.Value = hiRowDolares.Value & "+H" & iRowCD + 6 + x + 1
                End If
                x = x + 3
            Next
        End If

        If dt.Rows.Count > 0 Then
            oCells(iRowCD + 8 + x + dt.Rows.Count, iCol2 - 3) = "provisión mes SOLES"
            oCells(iRowCD + 8 + x + dt.Rows.Count, iCol2 - 2) = IIf(hiRow.Value = "", 0, "=" & hiRow.Value)
            oCells(iRowCD + 8 + x + dt.Rows.Count, iCol2 - 1) = "=H" & iRowCD + 8 + x + dt.Rows.Count
            oCells(iRowCD + 9 + x + dt.Rows.Count, iCol2 - 3) = "provisión mes DOLARES"
            oCells(iRowCD + 9 + x + dt.Rows.Count, iCol2 - 2) = IIf(hiRowDolares.Value = "", 0, "=" & hiRowDolares.Value)
            oCells(iRowCD + 9 + x + dt.Rows.Count, iCol2 - 1) = "=H" & iRowCD + 9 + x + dt.Rows.Count & "*" & ds.Tables(1).Rows(0)("TipoCambio")
            oCells(iRowCD + 10 + x + dt.Rows.Count, iCol2 - 3) = "provisión total del Mes"
            oCells(iRowCD + 10 + x + dt.Rows.Count, iCol2 - 1) = "=I" & iRowCD + 8 + x + dt.Rows.Count & "+I" & iRowCD + 9 + x + dt.Rows.Count
        End If

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

End Class

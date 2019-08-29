Imports Sit.BusinessEntities
Imports SIT.BusinessLayer
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml
Imports X14 = DocumentFormat.OpenXml.Office2010.Excel
Imports System.Data

Public Class Modulos_PrevisionPagos_frmReportes
    Inherits BasePage
    'Dim IdUsuario As String = Usuario.ToString

    Private Shared Function CreateStylesheet() As Spreadsheet.Stylesheet
        Dim ss As New Spreadsheet.Stylesheet()

        Dim fts As New Spreadsheet.Fonts()
        Dim ft As New DocumentFormat.OpenXml.Spreadsheet.Font()
        Dim FontFamilyNumbering As New Spreadsheet.FontFamilyNumbering()
        Dim FontScheme As New Spreadsheet.FontScheme()
        Dim ftn As New Spreadsheet.FontName()
        Dim bold As New Spreadsheet.Bold()
        ftn.Val = StringValue.FromString("Calibri")
        Dim ftsz As New Spreadsheet.FontSize()
        ftsz.Val = DoubleValue.FromDouble(11)
        ft.FontName = ftn
        ft.FontSize = ftsz
        fts.Append(ft)

        '0
        ft = New DocumentFormat.OpenXml.Spreadsheet.Font()
        ftn = New Spreadsheet.FontName()
        ftn.Val = StringValue.FromString("Palatino Linotype")
        ftsz = New Spreadsheet.FontSize()
        ftsz.Val = DoubleValue.FromDouble(11)
        ft.FontName = ftn
        ft.FontSize = ftsz
        fts.Append(ft)

        '1
        ft = New DocumentFormat.OpenXml.Spreadsheet.Font()
        ftsz = New Spreadsheet.FontSize()
        ftsz.Val = DoubleValue.FromDouble(11)
        FontFamilyNumbering = New Spreadsheet.FontFamilyNumbering()
        FontFamilyNumbering.Val = 2
        FontScheme = New Spreadsheet.FontScheme()
        FontScheme.Val = Spreadsheet.FontSchemeValues.Minor
        bold = New Spreadsheet.Bold()
        ft.FontSize = ftsz
        ft.FontFamilyNumbering = FontFamilyNumbering
        ft.FontScheme = FontScheme
        ft.Bold = bold
        fts.Append(ft)

        fts.Count = UInt32Value.FromUInt32(CUInt(fts.ChildElements.Count))

        Dim fills As New Spreadsheet.Fills()
        Dim fill As Spreadsheet.Fill
        Dim patternFill As Spreadsheet.PatternFill
        fill = New Spreadsheet.Fill()
        patternFill = New Spreadsheet.PatternFill()
        patternFill.PatternType = Spreadsheet.PatternValues.None
        fill.PatternFill = patternFill
        fills.Append(fill)

        fill = New Spreadsheet.Fill()
        patternFill = New Spreadsheet.PatternFill()
        patternFill.PatternType = Spreadsheet.PatternValues.Gray125
        fill.PatternFill = patternFill
        fills.Append(fill)

        fill = New Spreadsheet.Fill()
        patternFill = New Spreadsheet.PatternFill()
        patternFill.PatternType = Spreadsheet.PatternValues.Solid
        patternFill.ForegroundColor = New Spreadsheet.ForegroundColor()
        patternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("00ff9728")
        patternFill.BackgroundColor = New Spreadsheet.BackgroundColor()
        patternFill.BackgroundColor.Rgb = patternFill.ForegroundColor.Rgb
        fill.PatternFill = patternFill
        fills.Append(fill)

        '4
        fill = New Spreadsheet.Fill()
        patternFill = New Spreadsheet.PatternFill()
        patternFill.PatternType = Spreadsheet.PatternValues.Solid
        patternFill.ForegroundColor = New Spreadsheet.ForegroundColor()
        patternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("FFFF00")
        patternFill.BackgroundColor = New Spreadsheet.BackgroundColor()
        patternFill.BackgroundColor.Rgb = patternFill.ForegroundColor.Rgb
        fill.PatternFill = patternFill
        fills.Append(fill)

        '5
        fill = New Spreadsheet.Fill()
        patternFill = New Spreadsheet.PatternFill()
        patternFill.PatternType = Spreadsheet.PatternValues.Solid
        patternFill.ForegroundColor = New Spreadsheet.ForegroundColor()
        patternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("E0E0E0")
        patternFill.BackgroundColor = New Spreadsheet.BackgroundColor()
        patternFill.BackgroundColor.Rgb = patternFill.ForegroundColor.Rgb
        fill.PatternFill = patternFill
        fills.Append(fill)

        fills.Count = UInt32Value.FromUInt32(CUInt(fills.ChildElements.Count))

        Dim borders As New Spreadsheet.Borders()
        Dim border As New Spreadsheet.Border()
        border.LeftBorder = New Spreadsheet.LeftBorder()
        border.RightBorder = New Spreadsheet.RightBorder()
        border.TopBorder = New Spreadsheet.TopBorder()
        border.BottomBorder = New Spreadsheet.BottomBorder()
        border.DiagonalBorder = New Spreadsheet.DiagonalBorder()
        borders.Append(border)

        border = New Spreadsheet.Border()
        border.LeftBorder = New Spreadsheet.LeftBorder()
        border.LeftBorder.Style = Spreadsheet.BorderStyleValues.Thin
        border.RightBorder = New Spreadsheet.RightBorder()
        border.RightBorder.Style = Spreadsheet.BorderStyleValues.Thin
        border.TopBorder = New Spreadsheet.TopBorder()
        border.TopBorder.Style = Spreadsheet.BorderStyleValues.Thin
        border.BottomBorder = New Spreadsheet.BottomBorder()
        border.BottomBorder.Style = Spreadsheet.BorderStyleValues.Thin
        border.DiagonalBorder = New Spreadsheet.DiagonalBorder()
        borders.Append(border)
        borders.Count = UInt32Value.FromUInt32(CUInt(borders.ChildElements.Count))



        Dim csfs As New Spreadsheet.CellStyleFormats()
        Dim cf As New Spreadsheet.CellFormat()
        cf.NumberFormatId = 0
        cf.FontId = 0
        cf.FillId = 0
        cf.BorderId = 1
        csfs.Append(cf)
        csfs.Count = UInt32Value.FromUInt32(CUInt(csfs.ChildElements.Count))

        'Dim iExcelIndex As UInteger = 164
        Dim iExcelIndex As Integer = 164
        Dim nfs As New Spreadsheet.NumberingFormats()
        Dim cfs As New Spreadsheet.CellFormats()

        cf = New Spreadsheet.CellFormat()
        cf.NumberFormatId = 0
        cf.FontId = 0
        cf.FillId = 0
        cf.BorderId = 0
        cf.FormatId = 0
        cfs.Append(cf)

        Dim nfDateTime As New Spreadsheet.NumberingFormat()
        nfDateTime.NumberFormatId = UInt32Value.FromUInt32(System.Math.Max(System.Threading.Interlocked.Increment(iExcelIndex), iExcelIndex - 1))
        nfDateTime.FormatCode = StringValue.FromString("dd/mm/yyyy hh:mm:ss")
        nfs.Append(nfDateTime)

        Dim nf4decimal As New Spreadsheet.NumberingFormat()
        nf4decimal.NumberFormatId = UInt32Value.FromUInt32(System.Math.Max(System.Threading.Interlocked.Increment(iExcelIndex), iExcelIndex - 1))
        nf4decimal.FormatCode = StringValue.FromString("#,##0.0000")
        nfs.Append(nf4decimal)

        ' #,##0.00 is also Excel style index 4
        Dim nf2decimal As New Spreadsheet.NumberingFormat()
        nf2decimal.NumberFormatId = UInt32Value.FromUInt32(System.Math.Max(System.Threading.Interlocked.Increment(iExcelIndex), iExcelIndex - 1))
        nf2decimal.FormatCode = StringValue.FromString("#,##0.00")
        nfs.Append(nf2decimal)

        ' @ is also Excel style index 49
        Dim nfForcedText As New Spreadsheet.NumberingFormat()
        nfForcedText.NumberFormatId = UInt32Value.FromUInt32(System.Math.Max(System.Threading.Interlocked.Increment(iExcelIndex), iExcelIndex - 1))
        nfForcedText.FormatCode = StringValue.FromString("@")
        nfs.Append(nfForcedText)
        'Alignment
        Dim align As New Spreadsheet.Alignment() With { _
          .Horizontal = Spreadsheet.HorizontalAlignmentValues.General, _
          .Vertical = Spreadsheet.VerticalAlignmentValues.Center _
        }
        'wraptext
        ' Alignment align1 = new Alignment(){Horizontal=HorizontalAlignmentValues.CenterContinuous,Vertical=VerticalAlignmentValues.Center};


        ' index 1
        cf = New Spreadsheet.CellFormat()
        cf.NumberFormatId = nfDateTime.NumberFormatId
        cf.FontId = 0
        cf.FillId = 0
        cf.BorderId = 1
        cf.FormatId = 0

        cf.ApplyNumberFormat = BooleanValue.FromBoolean(True)
        cfs.Append(cf)

        ' index 2
        cf = New Spreadsheet.CellFormat()
        cf.NumberFormatId = nf4decimal.NumberFormatId
        cf.FontId = 0
        cf.FillId = 0
        cf.BorderId = 1
        cf.FormatId = 0
        cf.ApplyNumberFormat = BooleanValue.FromBoolean(True)
        cfs.Append(cf)

        ' index 3
        cf = New Spreadsheet.CellFormat()
        cf.NumberFormatId = nf2decimal.NumberFormatId
        cf.FontId = 0
        cf.FillId = 0
        cf.BorderId = 1
        cf.FormatId = 0
        cf.ApplyNumberFormat = BooleanValue.FromBoolean(True)
        cfs.Append(cf)

        ' index 4
        cf = New Spreadsheet.CellFormat()
        cf.NumberFormatId = nfForcedText.NumberFormatId
        cf.FontId = 2
        cf.FillId = 3
        cf.BorderId = 1
        cf.FormatId = 0
        cf.ApplyNumberFormat = BooleanValue.FromBoolean(True)
        cfs.Append(cf)

        ' index 5
        ' Header text
        cf = New Spreadsheet.CellFormat()
        cf.NumberFormatId = nfForcedText.NumberFormatId
        cf.FontId = 2
        cf.FillId = 4
        cf.BorderId = 1
        cf.FormatId = 0
        cf.ApplyNumberFormat = BooleanValue.FromBoolean(True)
        cfs.Append(cf)

        ' index 6
        ' column text
        cf = New Spreadsheet.CellFormat()
        cf.NumberFormatId = nfForcedText.NumberFormatId
        cf.FontId = 0
        cf.FillId = 0
        cf.BorderId = 1
        cf.FormatId = 0
        cf.ApplyNumberFormat = BooleanValue.FromBoolean(True)
        cfs.Append(cf)

        ' index 7
        ' coloured 2 decimal text
        cf = New Spreadsheet.CellFormat()
        cf.NumberFormatId = nf2decimal.NumberFormatId
        cf.FontId = 0
        'cf.FillId = 2;
        cf.FillId = 0
        cf.BorderId = 1
        cf.FormatId = 0
        cf.ApplyNumberFormat = BooleanValue.FromBoolean(True)
        cfs.Append(cf)
        'cf.Append(align);
        cf.Append(align)

        ' index 8
        ' coloured column text
        cf = New Spreadsheet.CellFormat()
        cf.NumberFormatId = nfForcedText.NumberFormatId
        cf.FontId = 0
        'cf.FillId = 2;
        cf.FillId = 0
        cf.BorderId = 1
        cf.FormatId = 0
        cf.ApplyNumberFormat = BooleanValue.FromBoolean(True)
        cfs.Append(cf)

        nfs.Count = UInt32Value.FromUInt32(CUInt(nfs.ChildElements.Count))
        cfs.Count = UInt32Value.FromUInt32(CUInt(cfs.ChildElements.Count))

        ss.Append(nfs)
        ss.Append(fts)
        ss.Append(fills)
        ss.Append(borders)
        ss.Append(csfs)
        ss.Append(cfs)

        Dim css As New Spreadsheet.CellStyles()
        Dim cs As New Spreadsheet.CellStyle()
        cs.Name = StringValue.FromString("Normal")
        cs.FormatId = 0
        cs.BuiltinId = 0
        css.Append(cs)
        css.Count = UInt32Value.FromUInt32(CUInt(css.ChildElements.Count))
        ss.Append(css)

        Dim dfs As New Spreadsheet.DifferentialFormats()
        dfs.Count = 0
        ss.Append(dfs)

        Dim tss As New Spreadsheet.TableStyles()
        tss.Count = 0
        tss.DefaultTableStyle = StringValue.FromString("TableStyleMedium9")
        tss.DefaultPivotStyle = StringValue.FromString("PivotStyleLight16")
        ss.Append(tss)

        Return ss
    End Function

    Private Shared Sub AppendNumericCell(ByVal cellReference As String, ByVal cellStringValue As String, ByVal excelRow As Spreadsheet.Row)
        '  Add a new Excel Cell to our Row 
        Dim cell As New Spreadsheet.Cell() With { _
         .CellReference = cellReference _
        }
        Dim cellValue As New Spreadsheet.CellValue()
        cellValue.Text = cellStringValue
        cell.Append(cellValue)
        excelRow.Append(cell)
    End Sub

    Private Shared Sub AppendTextCell(ByVal paso As Integer, ByVal cellReference As String, ByVal cellStringValue As String, ByVal excelRow As Spreadsheet.Row)
        '  Add a new Excel Cell to our Row 
        If paso <= 2 Then
            Dim cell As New Spreadsheet.Cell() With { _
                .StyleIndex = 4,
                .CellReference = cellReference, _
                .DataType = Spreadsheet.CellValues.[String] _
            }
            Dim cellValue As New Spreadsheet.CellValue()
            cellValue.Text = cellStringValue
            cell.Append(cellValue)
            excelRow.Append(cell)
        ElseIf paso <= 4 Then
            Dim cell As New Spreadsheet.Cell() With { _
                .StyleIndex = 5,
                .CellReference = cellReference, _
                .DataType = Spreadsheet.CellValues.[String] _
            }
            Dim cellValue As New Spreadsheet.CellValue()
            cellValue.Text = cellStringValue
            cell.Append(cellValue)
            excelRow.Append(cell)
        Else
            Dim cell As New Spreadsheet.Cell() With { _
                .StyleIndex = 6,
                .CellReference = cellReference, _
                .DataType = Spreadsheet.CellValues.[String] _
            }
            Dim cellValue As New Spreadsheet.CellValue()
            cellValue.Text = cellStringValue
            cell.Append(cellValue)
            excelRow.Append(cell)
        End If

    End Sub

    Private Shared Function GetExcelColumnName(ByVal columnIndex As Integer) As String
        If columnIndex < 26 Then
            Return Chr(Asc("A") + columnIndex).ToString()
        End If

        Dim firstChar As Char = Chr(Asc("A") + (columnIndex \ 26) - 1)
        Dim secondChar As Char = Chr(Asc("A") + (columnIndex Mod 26))

        Return String.Format("{0}{1}", firstChar, secondChar)
    End Function

    Private Shared Sub WriteDataTableToExcelWorksheet(ByVal rowIndex As UInteger, ByVal dt As DataTable, ByVal worksheetPart As WorksheetPart)
        Dim worksheet = worksheetPart.Worksheet
        Dim sheetData = worksheet.GetFirstChild(Of Spreadsheet.SheetData)()
        Dim cellValue As String = ""
        Dim paso As Integer = 1

        '  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        '
        '  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        '  cells of data, we'll know if to write Text values or Numeric cell values.
        Dim numberOfColumns As Integer = dt.Columns.Count
        Dim IsNumericColumn As Boolean() = New Boolean(numberOfColumns - 1) {}

        Dim excelColumnNames As String() = New String(numberOfColumns - 1) {}
        For n As Integer = 0 To numberOfColumns - 1
            excelColumnNames(n) = GetExcelColumnName(n)
        Next

        '
        '  Create the Header row in our Excel Worksheet
        '

        'Dim rowIndex As UInteger = 1

        Dim headerRow = New Spreadsheet.Row() With { _
         .RowIndex = rowIndex _
        }
        ' add a row at the top of spreadsheet
        sheetData.Append(headerRow)

        Dim cellNumericValue As Double = 0
        For Each dr As DataRow In dt.Rows
            ' ...create a new row, and append a set of this row's data to it.
            rowIndex += 1
            Dim newExcelRow = New Spreadsheet.Row() With { _
             .RowIndex = rowIndex _
            }
            ' add a row at the top of spreadsheet
            sheetData.Append(newExcelRow)

            For colInx As Integer = 0 To numberOfColumns - 1
                cellValue = dr.ItemArray(colInx).ToString()

                ' Create cell with data
                If IsNumericColumn(colInx) Then
                    '  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                    '  If this numeric value is NULL, then don't write anything to the Excel file.
                    cellNumericValue = 0
                    If Double.TryParse(cellValue, cellNumericValue) Then
                        cellValue = cellNumericValue.ToString()
                        AppendNumericCell(excelColumnNames(colInx) & rowIndex.ToString(), cellValue, newExcelRow)
                    End If
                Else
                    '  For text cells, just write the input data straight out to the Excel file.
                    AppendTextCell(paso, excelColumnNames(colInx) & rowIndex.ToString(), cellValue, newExcelRow)
                    paso = paso + 1
                End If
            Next
        Next
    End Sub

    Private Function DumpData(ByVal rowStar As Integer, ByVal dt As System.Data.DataTable, ByVal oCells As Excel.Range)
        For i As Integer = 0 To dt.Rows.Count - 1
            For j As Integer = 0 To dt.Columns.Count - 1
                oCells(i + rowStar, j + 2) = dt.Rows(i)(j).ToString()
                oCells.Range("B" & (rowStar + i).ToString & ":C" & (rowStar + j).ToString).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
                oCells.Range("B" & (rowStar + i).ToString & ":C" & (rowStar + j).ToString).Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
                oCells.Range("B" & (rowStar + i).ToString & ":C" & (rowStar + j).ToString).Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
                oCells.Range("B" & (rowStar + i).ToString & ":C" & (rowStar + j).ToString).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
                oCells.Range("B" & (rowStar + i).ToString & ":C" & (rowStar + j).ToString).Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous
                oCells.Range("B" & (rowStar + i).ToString & ":C" & (rowStar + j).ToString).Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous
                oCells.Range("B" & (rowStar + i).ToString).EntireColumn.ColumnWidth = 30
                oCells.Range("C" & (rowStar + i).ToString).EntireColumn.ColumnWidth = 50
                If i > 0 Then
                    oCells.Range("B" & (rowStar + i).ToString).NumberFormat = "0.0000000"
                End If
                If i <= 1 Then
                    If i = 0 Then
                        oCells(i + rowStar, j + 2).Interior.Color = RGB(250, 250, 25)
                    Else
                        oCells(i + rowStar, j + 2).Interior.Color = RGB(234, 234, 231)
                    End If
                    oCells(i + rowStar, j + 2).Font.Bold = True
                End If
            Next
        Next
    End Function

    Function BuildDataTable(ByVal oDataTable As System.Data.DataTable) As System.Data.DataTable
        Try
            Dim oDataTable2 As New System.Data.DataTable
            Dim oRow As DataRow = oDataTable2.NewRow
            Dim Total As Decimal
            Dim Banco, CuentaCte, IdTipoMoneda As String
            Dim contador As Integer = 0

            'oDataTable2 = oDataTable.Clone
            oDataTable2.Columns.Add("Importe")
            oDataTable2.Columns.Add("Fuente")

            Banco = oDataTable.Rows(0)(0)
            CuentaCte = oDataTable.Rows(0)(1)
            IdTipoMoneda = oDataTable.Rows(0)(4)

            For Each row As DataRow In oDataTable.Rows
                Total = Total + Convert.ToDecimal(row("Importe"))
            Next

            oRow("Importe") = Banco.ToString
            oRow("Fuente") = CuentaCte.ToString + " - " + IdTipoMoneda.ToString()
            oDataTable2.Rows.Add(oRow)

            oRow = oDataTable2.NewRow
            oRow("Importe") = String.Format("{0:0.00}", Total)
            oRow("Fuente") = Convert.ToString("Sumatoria / Movimientos")
            oDataTable2.Rows.Add(oRow)

            For i As Integer = 0 To oDataTable.Rows.Count - 1
                If oDataTable.Rows(i)(2).ToString().StartsWith("-") = True Then
                    Dim cadena As String = oDataTable.Rows(i)(3).ToString()
                    Dim ncadena As Integer = cadena.Length - 5
                    Dim partRight As String = cadena.Substring(5, ncadena)
                    Dim cadenaNueva As String = "ACRED" + partRight
                    oRow = oDataTable2.NewRow
                    oRow("Importe") = oDataTable.Rows(i)(2).ToString()
                    'oRow("Fuente") = oDataTable.Rows(i)(3).ToString()
                    oRow("Fuente") = cadenaNueva
                    oDataTable2.Rows.Add(oRow)
                Else
                    Dim cadena As String = oDataTable.Rows(i)(3).ToString()
                    Dim ncadena As Integer = cadena.Length - 5
                    Dim partRight As String = cadena.Substring(5, ncadena)
                    Dim cadenaNueva As String = "DEB" + partRight
                    oRow = oDataTable2.NewRow
                    oRow("Importe") = oDataTable.Rows(i)(2).ToString()
                    'oRow("Fuente") = oDataTable.Rows(i)(3).ToString()
                    oRow("Fuente") = cadenaNueva
                    oDataTable2.Rows.Add(oRow)
                End If
            Next

            Return oDataTable2

        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Function

    Private Shared Function CreateColumnData(ByVal StartColumnIndex As UInt32, ByVal EndColumnIndex As UInt32, ByVal ColumnWidth As Double) As Spreadsheet.Column
        Dim column As Spreadsheet.Column
        column = New Spreadsheet.Column()
        column.Min = StartColumnIndex
        column.Max = EndColumnIndex
        column.Width = ColumnWidth
        column.CustomWidth = True
        Return column
    End Function

    Private Sub GenerarExcelPorFondo()
        Try
            Dim sufijo As String
            Dim rowStar As Integer
            Dim star As Integer
            Dim nFilas As Integer
            Dim nPasos As Integer = 0

            Dim path As System.IO.FileInfo
            path = New System.IO.FileInfo(Server.MapPath("../../ExcelOut/ReportePorFondo.xls"))
            'path = New System.IO.FileInfo(Server.MapPath("E:\ipastor\SITSolution Nuevo Diseño\SIT Nuevo Diseño\SITSolution Nuevo\IN-SIT\ExcelOut"))
            If path.Exists Then
                path.Delete()
            End If

            Using document As SpreadsheetDocument = SpreadsheetDocument.Create(path.ToString(), SpreadsheetDocumentType.Workbook)
                Dim spreadsheet As SpreadsheetDocument = document

                '  Create the Excel file contents.  This function is used when creating an Excel file either writing 
                '  to a file, or writing to a MemoryStream.
                spreadsheet.AddWorkbookPart()
                spreadsheet.WorkbookPart.Workbook = New DocumentFormat.OpenXml.Spreadsheet.Workbook()

                '  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
                spreadsheet.WorkbookPart.Workbook.Append(New DocumentFormat.OpenXml.Spreadsheet.BookViews(New DocumentFormat.OpenXml.Spreadsheet.WorkbookView()))

                '  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
                Dim workbookStylesPart As WorkbookStylesPart = spreadsheet.WorkbookPart.AddNewPart(Of WorkbookStylesPart)("rIdStyles")
                Dim stylesheet As New DocumentFormat.OpenXml.Spreadsheet.Stylesheet()
                'workbookStylesPart.Stylesheet = stylesheet
                workbookStylesPart.Stylesheet = CreateStylesheet()
                workbookStylesPart.Stylesheet.Save()

                Dim cadena As New List(Of String)
                cadena.Add(5)
                cadena.Add(1)
                cadena.Add(2)
                cadena.Add(3)
                cadena.Add(4)

                Dim worksheetNumber As UInteger = 1

                For i As Integer = 1 To 5
                    rowStar = 2
                    star = 0
                    nFilas = 0



                    Dim oDataSet As DataSet = PrevisionPagoDetalleBM.ListarCuentaCtePorIdFondo(cadena(i - 1), UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text.Trim))
                    If oDataSet.Tables(0).Rows.Count > 0 Then
                        nPasos = nPasos + 1
                    End If

                    Dim newWorksheetPart As WorksheetPart = spreadsheet.WorkbookPart.AddNewPart(Of WorksheetPart)()
                    newWorksheetPart.Worksheet = New DocumentFormat.OpenXml.Spreadsheet.Worksheet()

                    Dim columns As New DocumentFormat.OpenXml.Spreadsheet.Columns()
                    columns.Append(CreateColumnData(1, 1, 35))
                    columns.Append(CreateColumnData(2, 2, 60))
                    newWorksheetPart.Worksheet.Append(columns)

                    ' create sheet data
                    newWorksheetPart.Worksheet.AppendChild(New DocumentFormat.OpenXml.Spreadsheet.SheetData())

                    Dim workSheetID As String
                    Dim worksheetName As String

                    Select Case i
                        Case "1"
                            worksheetName = "Fondo 0"
                        Case "2"
                            worksheetName = "Fondo 1"
                        Case "3"
                            worksheetName = "Fondo 2"
                        Case "4"
                            worksheetName = "Fondo 3"
                        Case "5"
                            worksheetName = "Administrador"
                    End Select

                    For Each row As DataRow In oDataSet.Tables(0).Rows

                        Dim CuentaCte As String = row("IdCuentaCorriente").ToString.Trim
                        Dim dt As System.Data.DataTable = BuildDataTable(PrevisionPagoDetalleBM.PagoDetallePorCuentaCte(UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text.Trim), CuentaCte).Tables(0))
                        star = star + 1
                        If star > 1 Then
                            rowStar = rowStar + nFilas
                            'nFilas = nFilas + (dt.Rows.Count + 1)
                            nFilas = 2 + (dt.Rows.Count)
                        Else
                            nFilas = 2 + (dt.Rows.Count)
                        End If

                        WriteDataTableToExcelWorksheet(rowStar, dt, newWorksheetPart)

                    Next


                    If worksheetNumber = 1 Then
                        spreadsheet.WorkbookPart.Workbook.AppendChild(New DocumentFormat.OpenXml.Spreadsheet.Sheets())
                    End If

                    spreadsheet.WorkbookPart.Workbook.GetFirstChild(Of DocumentFormat.OpenXml.Spreadsheet.Sheets)().AppendChild(New DocumentFormat.OpenXml.Spreadsheet.Sheet() With { _
                     .Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart), _
                     .SheetId = CUInt(worksheetNumber), _
                     .Name = worksheetName _
                    })

                    worksheetNumber += 1

                    newWorksheetPart.Worksheet.Save()
                Next

                spreadsheet.WorkbookPart.Workbook.Save()
                spreadsheet.Close()

                If nPasos > 0 Then
                    System.GC.Collect()

                    Dim response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
                    response.ClearContent()
                    response.Clear()
                    response.ContentType = "text/plain"
                    response.AddHeader("Content-Disposition", "attachment; filename=ReportePorFondo.xls;")
                    response.TransmitFile(Server.MapPath("../../ExcelOut/ReportePorFondo.xls"))
                    response.Flush()
                    response.[End]()
                Else
                    AlertaJS("No hay datos para descargar.")
                End If
            End Using
            
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Private Sub GenerarExcelDetallado()

        Dim sFechaInicio, sFechaFin As String
        sFechaInicio = UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text.Trim)
        sFechaFin = UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text.Trim)

        Dim pathImagen As String
        Dim _dt As New System.Data.DataTable()
        _dt = PrevisionParametriaBM.ListarParametria(14).Tables(0)
        pathImagen = _dt.Rows(0)("Descripcion").ToString()

        Dim oDataSet As DataSet = PrevisionPagoDetalleBM.ReportePrevisionPagoPorDetalle(sFechaInicio, sFechaFin)
        If oDataSet.Tables(0).Rows.Count > 0 Then

            Dim oRow As DataRow = oDataSet.Tables(0).NewRow
            Dim Ingreso0, Egreso0, Ingreso1, Egreso1, Ingreso2, Egreso2, Ingreso3, Egreso3, IngresoAdm, EgresoAdm, Total As Double

            For Each row As DataRow In oDataSet.Tables(0).Rows
                Ingreso0 = Ingreso0 + Convert.ToDouble(row("Ingreso0").ToString)
                Egreso0 = Egreso0 + Convert.ToDouble(row("Egreso0").ToString)
                Ingreso1 = Ingreso1 + Convert.ToDouble(row("Ingreso1").ToString)
                Egreso1 = Egreso1 + Convert.ToDouble(row("Egreso1").ToString)
                Ingreso2 = Ingreso2 + Convert.ToDouble(row("Ingreso2").ToString)
                Egreso2 = Egreso2 + Convert.ToDouble(row("Egreso2").ToString)
                Ingreso3 = Ingreso3 + Convert.ToDouble(row("Ingreso3").ToString)
                Egreso3 = Egreso3 + Convert.ToDouble(row("Egreso3").ToString)
                IngresoAdm = IngresoAdm + Convert.ToDouble(row("IngresoADM").ToString)
                EgresoAdm = EgresoAdm + Convert.ToDouble(row("EgresoADM").ToString)
                Total = Total + Convert.ToDouble(row("Total").ToString)
            Next

            oRow("IdTipoMoneda") = "Totales:"
            oRow("Ingreso0") = String.Format("{0:0.00}", Ingreso0)
            oRow("Egreso0") = String.Format("{0:0.00}", Egreso0)
            oRow("Ingreso1") = String.Format("{0:0.00}", Ingreso1)
            oRow("Egreso1") = String.Format("{0:0.00}", Egreso1)
            oRow("Ingreso2") = String.Format("{0:0.00}", Ingreso2)
            oRow("Egreso2") = String.Format("{0:0.00}", Egreso2)
            oRow("Ingreso3") = String.Format("{0:0.00}", Ingreso3)
            oRow("Egreso3") = String.Format("{0:0.00}", Egreso3)
            oRow("IngresoADM") = String.Format("{0:0.00}", IngresoAdm)
            oRow("EgresoADM") = String.Format("{0:0.00}", EgresoAdm)
            oRow("Total") = String.Format("{0:0.00}", Total)
            oDataSet.Tables(0).Rows.Add(oRow)

            'Agregado

            Dim myDS As New Data.DataSet("Provision")
            Dim myCustomers As Data.DataTable = myDS.Tables.Add("ProvisionDetalle")

            With myCustomers
                .Columns.Add("CodigoPago", Type.GetType("System.String"))
                .Columns.Add("TipoOperacion", Type.GetType("System.String"))
                .Columns.Add("IdTipoMoneda", Type.GetType("System.String"))
                .Columns.Add("Banco", Type.GetType("System.String"))
                .Columns.Add("IdBanco", Type.GetType("System.String"))
                .Columns.Add("IdEntidad0", Type.GetType("System.String"))
                .Columns.Add("Ingreso0", Type.GetType("System.String"))
                .Columns.Add("Egreso0", Type.GetType("System.String"))
                .Columns.Add("IdEntidad1", Type.GetType("System.String"))
                .Columns.Add("Ingreso1", Type.GetType("System.String"))
                .Columns.Add("Egreso1", Type.GetType("System.String"))
                .Columns.Add("IdEntidad2", Type.GetType("System.String"))
                .Columns.Add("Ingreso2", Type.GetType("System.String"))
                .Columns.Add("Egreso2", Type.GetType("System.String"))
                .Columns.Add("IdEntidad3", Type.GetType("System.String"))
                .Columns.Add("Ingreso3", Type.GetType("System.String"))
                .Columns.Add("Egreso3", Type.GetType("System.String"))
                .Columns.Add("IdEntidadADM", Type.GetType("System.String"))
                .Columns.Add("IngresoADM", Type.GetType("System.String"))
                .Columns.Add("EgresoADM", Type.GetType("System.String"))
                .Columns.Add("Total", Type.GetType("System.String"))
                .Columns.Add("FechaPago", Type.GetType("System.String"))
                .Columns.Add("Estado", Type.GetType("System.String"))
            End With

            Dim myDr As Data.DataRow

            For Each row As DataRow In oDataSet.Tables(0).Rows
                myDr = myCustomers.NewRow()
                myDr("CodigoPago") = row("CodigoPago").ToString
                myDr("TipoOperacion") = row("TipoOperacion").ToString
                myDr("IdTipoMoneda") = row("IdTipoMoneda").ToString
                myDr("Banco") = row("Banco").ToString
                myDr("IdBanco") = row("IdBanco").ToString
                myDr("IdEntidad0") = row("IdEntidad0").ToString
                myDr("Ingreso0") = IIf(row("Ingreso0").ToString = "0.00", "", row("Ingreso0").ToString)
                myDr("Egreso0") = IIf(row("Egreso0").ToString = "0.00", "", row("Egreso0").ToString)
                myDr("IdEntidad1") = row("IdEntidad1").ToString
                myDr("Ingreso1") = IIf(row("Ingreso1").ToString = "0.00", "", row("Ingreso1").ToString)
                myDr("Egreso1") = IIf(row("Egreso1").ToString = "0.00", "", row("Egreso1").ToString)
                myDr("IdEntidad2") = row("IdEntidad2").ToString
                myDr("Ingreso2") = IIf(row("Ingreso2").ToString = "0.00", "", row("Ingreso2").ToString)
                myDr("Egreso2") = IIf(row("Egreso2").ToString = "0.00", "", row("Egreso2").ToString)
                myDr("IdEntidad3") = row("IdEntidad3").ToString
                myDr("Ingreso3") = IIf(row("Ingreso3").ToString = "0.00", "", row("Ingreso3").ToString)
                myDr("Egreso3") = IIf(row("Egreso3").ToString = "0.00", "", row("Egreso3").ToString)
                myDr("IdEntidadADM") = row("IdEntidadADM").ToString
                myDr("IngresoADM") = IIf(row("IngresoADM").ToString = "0.00", "", row("IngresoADM").ToString)
                myDr("EgresoADM") = IIf(row("EgresoADM").ToString = "0.00", "", row("EgresoADM").ToString)
                myDr("Total") = IIf(row("Total").ToString = "0.00", "", row("Total").ToString)
                myDr("FechaPago") = row("FechaPago").ToString
                myDr("Estado") = row("Estado").ToString

                myCustomers.Rows.Add(myDr)
            Next
            '
            gvDetallado.DataSource = myCustomers 'oDataSet
            gvDetallado.DataBind()
            'gvDetallado.Visible = True


            Dim sb As New StringBuilder()
            Dim sw As New StringWriter(sb)
            Dim htw As New HtmlTextWriter(sw)

            Dim page As New Page()
            Dim form As New HtmlForm()
            Dim HtmlBody As String

            HtmlBody = "<div><table>" &
                        "<tr>" &
                        "<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>" &
                        "<td>" &
                        "<br>Fecha Emision:</br>" &
                        "<br>Hora Emision:</br>" &
                        "<br>Usuario:</br>" &
                        "</td>" &
                        "<td>" &
                        "<br style=" + "float: right;" + ">&nbsp;&nbsp;" + System.DateTime.Now.ToString("dd/MM/yyyy") + "</br>" &
                        "<br style=" + "float: right;" + ">&nbsp;&nbsp;" + System.DateTime.Now.ToString("hh:mm:ss tt") + "</br>" &
                        "<br style=" + "float: right;" + ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Usuario.ToString + "</br>" &
                        "</td>" &
                        "</tr>" &
                        "</table></div>"

            sb.Append(HtmlBody)

            'sb.Append("<div>")
            'sb.Append("<tr>")
            'sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td>")
            'sb.Append("<b>Fecha Emision: " + System.DateTime.Now.ToString("dd/MM/yyyy") + "</b>")
            'sb.Append("<b>Hora Emision: " + System.DateTime.Now.ToString("hh:mm:ss tt") + "</b>")
            'sb.Append("<b>Usuario: " + IdUsuario.ToString + "</b>")
            'sb.Append("</td>")
            'sb.Append("</tr>")
            'sb.Append("</div>")


            ' Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = False

            ' Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize()

            page.Controls.Add(form)
            form.Controls.Add(gvDetallado)

            page.RenderControl(htw)

            response.Clear()
            response.Buffer = True
            response.ContentType = "application/vnd.ms-excel"
            response.AddHeader("Content-Disposition", "attachment;filename=Export.xls")
            response.Charset = "UTF-8"
            response.ContentEncoding = Encoding.[Default]

            'Dim fi As System.IO.FileInfo
            Dim style As String = "<style> td { mso-number-format:\@; } </style>"
            Dim tag As String
            'fi = New System.IO.FileInfo(Server.MapPath("../../Common/Imagenes/logo.gif"))
            'tag = "<img src='" & fi.ToString & "' width='220' eight='100'/>"
            tag = "<img src='" & pathImagen.ToString & "' width='220' eight='100'/>"
            Response.Write(tag)
            response.Write(style.ToString())
            response.Write(sb.ToString())
            response.[End]()

            '"<style> td { mso-number-format:" + "0\.0000000" + "; } </style> "
            '"<style> td { mso-number-format:" + "\@" + "; } </style> "
        Else
            AlertaJS("No hay datos para descargar.")
        End If
    End Sub

    Private Sub CargarCombos(ByVal ddl As DropDownList, ByVal Parametro As Int32)
        Dim tablaListaParametria As New Data.DataTable
        Dim oTipoDocumento As New TipoDocumentoBM
        tablaListaParametria = PrevisionParametriaBM.ListarParametria(Parametro).Tables(0)
        HelpCombo.LlenarComboBox(ddl, tablaListaParametria, "Valor", "Descripcion", True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CargarCombos(ddlTipoOperacion, 12)
        End If
    End Sub

    Protected Sub btnGenerar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenerar.Click
        Try
            If ddlTipoOperacion.SelectedIndex > 0 Then
                If ddlTipoOperacion.SelectedValue = "REP2" Then
                    If txtFechaInicio.Text = String.Empty Or txtFechaFin.Text = String.Empty Then
                        AlertaJS("Debe ingresar ambas fechas.")
                    Else
                        If UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text.Trim) < UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text.Trim) Then
                            AlertaJS("La fecha final no debe ser menor a la inicial.")
                        Else
                            GenerarExcelDetallado()
                        End If
                    End If
                Else
                    If txtFechaInicio.Text = String.Empty Then
                        AlertaJS("Debe ingresar la fecha de inicio.")
                    Else
                        GenerarExcelPorFondo()
                    End If
                End If
            Else
                AlertaJS("Debe ingresar y/o seleccionar para generar.")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Protected Sub gvDetallado_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDetallado.RowDataBound
        Select Case e.Row.RowType
            Case DataControlRowType.Header

                'Agrupando las dos primeras columnas  (col=4, col=5)
                e.Row.Cells(4).ColumnSpan = 2
                'e.Row.Cells(4).Text = "Fondo 0"
                e.Row.Cells(4).Text = "<table><tr><td colspan=" + "2" + " align=" + "center" + "><b>Fondo 0</b></td></tr><tr><td align=" + "center" + "><b>Ingreso</b></td><td align=" + "center" + "><b>Egreso</b></td></tr></table>"
                e.Row.Cells(5).Visible = False

                'Agrupando las dos ultimas columnas (col=6 y col=7)
                e.Row.Cells(6).ColumnSpan = 2
                'e.Row.Cells(6).Text = "Fondo 1"
                e.Row.Cells(6).Text = "<table><tr><td colspan=" + "2" + " align=" + "center" + "><b>Fondo 1</b></td></tr><tr><td align=" + "center" + "><b>Ingreso</b></td><td align=" + "center" + "><b>Egreso</b></td></tr></table>"
                e.Row.Cells(7).Visible = False

                'Agrupando las dos primeras columnas  (col=8, col=9)
                e.Row.Cells(8).ColumnSpan = 2
                'e.Row.Cells(8).Text = "Fondo 2"
                e.Row.Cells(8).Text = "<table><tr><td colspan=" + "2" + " align=" + "center" + "><b>Fondo 2</b></td></tr><tr><td align=" + "center" + "><b>Ingreso</b></td><td align=" + "center" + "><b>Egreso</b></td></tr></table>"
                e.Row.Cells(9).Visible = False

                'Agrupando las dos primeras columnas  (col=10, col=11)
                e.Row.Cells(10).ColumnSpan = 2
                'e.Row.Cells(10).Text = "Fondo 3"
                e.Row.Cells(10).Text = "<table><tr><td colspan=" + "2" + " align=" + "center" + "><b>Fondo 3</b></td></tr><tr><td align=" + "center" + "><b>Ingreso</b></td><td align=" + "center" + "><b>Egreso</b></td></tr></table>"
                e.Row.Cells(11).Visible = False

                'Agrupando las dos primeras columnas  (col=12, col=13)
                e.Row.Cells(12).ColumnSpan = 2
                'e.Row.Cells(12).Text = "Fondo Adm"
                e.Row.Cells(12).Text = "<table><tr><td colspan=" + "2" + " align=" + "center" + "><b>Fondo Adm</b></td></tr><tr><td align=" + "center" + "><b>Ingreso</b></td><td align=" + "center" + "><b>Egreso</b></td></tr></table>"
                e.Row.Cells(13).Visible = False

        End Select
    End Sub

End Class

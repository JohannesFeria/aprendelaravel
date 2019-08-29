Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports System.Data.OleDb
Imports Microsoft.Office

Partial Class Modulos_Gestion_Archivos_Planos_frmInterfaseCompararVL
    Inherits BasePage

    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click

        Dim decFecha As Decimal = 0
        Dim strPortafolio As String = String.Empty
        Dim strEscenario As String = String.Empty
        Dim ds As DataSet
        Dim dt1 As New DataTable

        Dim objReporteVLMidasBM As New ReporteVLMidasBM()
        Dim objReporteVLMidasBE As New ReporteVLMidasBE()

        Try
            If Trim(txtFecha.Text) = "" Then
                AlertaJS(Constantes.M_STR_MENSAJE_FECHA_VACIA)
                Exit Sub
            End If

            decFecha = UIUtility.ConvertirFechaaDecimal(Me.txtFecha.Text)
            objReporteVLMidasBE.Fecha = decFecha

            Dim Resultado As String = ""
            Resultado = objReporteVLMidasBM.ValidarVL(objReporteVLMidasBE, DatosRequest)
            If Resultado.Length > 1 Then
                AlertaJS(Resultado)
                Return
            End If

            ds = objReporteVLMidasBM.CompararVL(objReporteVLMidasBE, DatosRequest)
            If ds.Tables.Count > 0 Then
                dt1 = ds.Tables(0)
            End If

            ExportarExcel(dt1)

        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Sub ExportarExcel(ByVal dt1 As DataTable)
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
            strRutaPlantilla = RutaPlantillas() & "\PlantillaCompararVL.xls"
            strNombreArchivo = "CompararVL_" & Usuario.ToString() & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & System.DateTime.Now.ToString("_HHmmss") & ".xls"

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

                DumpData(dt1, oBook)

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

    Private Sub DumpData(ByVal dt1 As DataTable, ByVal oBook As Excel.Workbook)
        Dim iRow As Integer = 0, iCol As Integer = 0, nFilIni As Long = 0, nFilFin As Long = 0, nPFil As Long = 0
        Dim dr As DataRow
        Dim ary() As Object

        For Each sheet As Excel.Worksheet In oBook.Sheets()
            Select Case sheet.Name

                Case "VL"
                    iRow = 0
                    iCol = 0
                    nPFil = 0
                    nFilIni = 7
                    nFilFin = 0

                    'For iCol = 0 To dt1.Columns.Count - 1
                    '    sheet.Cells(1, iCol + 1) = dt1.Columns(iCol).ToString
                    'Next

                    sheet.Cells(4, 2) = txtFecha.Text

                    For iRow = 0 To dt1.Rows.Count - 1
                        dr = dt1.Rows.Item(iRow)
                        nPFil = iRow + nFilIni
                        ary = dr.ItemArray
                        For iCol = 0 To UBound(ary)
                            sheet.Cells(iRow + 7, iCol + 1) = ary(iCol).ToString
                        Next
                        nFilFin = iRow + nFilIni
                    Next

                    If dt1.Rows.Count > 1 Then
                        sheet.Cells.Range("A" & nFilIni.ToString & ":Z" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":Z" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":Z" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
                        sheet.Cells.Range("A" & nFilIni.ToString & ":Z" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous

                        If nFilIni < nFilFin Then
                            sheet.Cells.Range("A" & nFilIni.ToString & ":Z" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous
                            sheet.Cells.Range("A" & nFilIni.ToString & ":Z" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous
                        End If

                    End If

            End Select

        Next

    End Sub

End Class

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports System.IO
Partial Class Modulos_Gestion_Reportes_frmReporteValorCuotaVariacion
    Inherits BasePage
  
    Private Sub GenerarReporteFormula()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim sFile As String, sTemplate As String
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oSheetTemplate As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Dim Indice As Integer = 1
        Dim i As Int32 = 1
        Dim exitDo As Boolean = False
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RC_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaValorCuotaVariacion20.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            Dim dteFechaHoy As Date = tbFechaIni.Text
            'Tabla 1 RESUMEN
            oSheet = oBook.Sheets(1)
            oCells = oSheet.Cells
            Dim dsvVecVar As DataSet = New ReporteGestionBM().Reporte_ValorCuotaVariacionFormula(ddlVariacion.SelectedValue, ddlAlerta.SelectedValue, UIUtility.ConvertirFechaaDecimal(dteFechaHoy))
            If dsvVecVar.Tables(0).Rows.Count = 0 Then
                AlertaJS("No existen registros que mostrar.")
                Exit Sub
            End If
            oCells(5, 2) = "FECHA: " + tbFechaIni.Text
            oCells(2, 6) = Now.ToLongDateString
            oCells(3, 6) = Usuario
            oCells(7, 4) = "Estadísticos (a " & ddlVariacion.SelectedValue & " meses)"

            Dim dtResumen As DataTable = dsvVecVar.Tables(0)
            Dim FilaInicialResumen As Integer = 9
            Dim intAcumularTotalInicio As Integer = 0
            Dim intAcumularTotalFin As Integer = 0
            For Each dr In dtResumen.Rows
                oCells(FilaInicialResumen, 2) = dr("Portafolio")
                oCells(FilaInicialResumen, 3) = dr("VariacionPorcentual")

                oSheet.Range("D" & FilaInicialResumen).FormulaR1C1 = "='2_Detalle_Calculo'!RC[3]"
                oSheet.Range("E" & FilaInicialResumen).FormulaR1C1 = "='2_Detalle_Calculo'!RC[3]"

                oSheet.Range("F" & FilaInicialResumen).FormulaR1C1 = "=(RC[-3]-RC[-2])/RC[-1]"
                oSheet.Range("G" & FilaInicialResumen).FormulaR1C1 = "=IF(ABS(RC[-1])>" & dr("ParametroCantidadVariacionOK") & ", ""Revisar"",""OK"")"

                FilaInicialResumen += 1
            Next
            oSheet.Range(oSheet.Cells(9, 2), oSheet.Cells(FilaInicialResumen - 1, 7)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous

            ''Tabla 2 DETALLE CALCULO
            oSheet = oBook.Sheets(2)
            oCells = oSheet.Cells
            ''Tabla 3 DETALLE HISTORIAL
            Dim dtDetalleHistorial As DataTable = dsvVecVar.Tables(2)
            Dim FilaInicialHistorial As Integer = 9
            Dim strID As String = ""
            intAcumularTotalInicio = 1
            intAcumularTotalFin = 0
            For Each dr In dtDetalleHistorial.Rows
                oCells(FilaInicialHistorial, 10) = dr("Portafolio")
                oCells(FilaInicialHistorial, 11) = dr("FechaProceso")

                oCells(FilaInicialHistorial, 12) = dr("ValCuotaValoresCierre")

                oCells(FilaInicialHistorial, 13) = dr("VariacionPorcentual")
                oSheet.Range("N" & FilaInicialHistorial).FormulaR1C1 = "=RC[-1]+1"

                If FilaInicialHistorial = 9 Then
                    oSheet.Range("O" & FilaInicialHistorial).FormulaR1C1 = "=RC[-2]-RC[-8]"
                Else
                    If strID = dr("CodigoPortafolioSBS") & dr("CodigoSerie") Then
                        intAcumularTotalInicio = intAcumularTotalInicio + 1
                    Else
                        strID = dr("CodigoPortafolioSBS") & dr("CodigoSerie")
                    End If
                    oSheet.Range("O" & FilaInicialHistorial).FormulaR1C1 = "=RC[-2]-R[-" & intAcumularTotalInicio & "]C[-8]"
                End If


                FilaInicialHistorial += 1
            Next
            ''Tabla 2 DETALLE CALCULO
            Dim dtDetalleCalculo As DataTable = dsvVecVar.Tables(1)
            Dim FilaInicialCalculo As Integer = 9
            intAcumularTotalInicio = 0
            intAcumularTotalFin = 0
            For Each dr In dtDetalleCalculo.Rows
                oCells(FilaInicialCalculo, 1) = dr("Portafolio")
                oCells(FilaInicialCalculo, 2) = dr("FechaProceso")
                oCells(FilaInicialCalculo, 3) = dr("FechaProcesoFin")
                oCells(FilaInicialCalculo, 4) = dr("VariacionPorcentual")

                intAcumularTotalFin = intAcumularTotalInicio + dr("TotalRegistro") - 1
                If FilaInicialCalculo = 9 Then
                    oSheet.Range("E" & FilaInicialCalculo).FormulaR1C1 = "=PRODUCT(RC[9]:R[" & intAcumularTotalFin & "]C[9])"
                    oSheet.Range("H" & FilaInicialCalculo).FormulaR1C1 = "=(SUM(RC[7]:R[" & intAcumularTotalFin & "]C[7])/RC[-2])^0.5"
                Else
                    oSheet.Range("E" & FilaInicialCalculo).FormulaR1C1 = "=PRODUCT(R[" & intAcumularTotalInicio & "]C[9]:R[" & intAcumularTotalFin & "]C[9])"
                    oSheet.Range("H" & FilaInicialCalculo).FormulaR1C1 = "=(SUM(R[" & intAcumularTotalInicio & "]C[7]:R[" & intAcumularTotalFin & "]C[7])/RC[-2])^0.5"
                End If
                intAcumularTotalInicio = intAcumularTotalInicio + dr("TotalRegistro") - 1

                oCells(FilaInicialCalculo, 6) = dr("TotalRegistro")
                oSheet.Range("G" & FilaInicialCalculo).FormulaR1C1 = "=RC[-2]^(1/RC[-1])-1"

                FilaInicialCalculo += 1
            Next
            oSheet.SaveAs(sFile)
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "RC_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
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
    Protected Sub Modulos_Gestion_Reportes_frmReporteValorCuotaVariacion_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            tbFechaIni.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
        End If
    End Sub
    Protected Sub btnImprimirFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimirFormula.Click
        GenerarReporteFormula()
    End Sub
End Class
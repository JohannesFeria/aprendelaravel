Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports System.Data
Imports System.IO
Partial Class Modulos_Contabilidad_Reportes_frmEvidenciaLoteClaseInstrumento
    Inherits BasePage
    Private Sub CargarFiltros()
        Dim oPortafolioBE As DataTable
        Dim oPortafolioBM As New PortafolioBM
        oPortafolioBE = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlFondo, oPortafolioBE, "CodigoPortafolio", "Descripcion", False)
    End Sub
    Sub Reporte()
        'OT10689 - Inicio. Kill process excel
        Dim can As Integer = 0
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim sTemplate As String, sNomFile As String
            Dim sFecha As String, sHora As String, sRutaTemp As String
            Dim sFile As String = ""
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            sFecha = String.Format("{0:yyyyMMdd}", DateTime.Today)
            sHora = String.Format("{0:HHMMss}", DateTime.Now)
            sRutaTemp = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
            sNomFile = "ON_" & Usuario.ToString() & sFecha & sHora & ".xls"
            sFile = sRutaTemp & sNomFile
            sTemplate = RutaPlantillas() & "\" & "PlantillaLotesResumenClaseInstrumento.xls"
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells
            Dim DT As DataTable = New ReporteContabilidadBM().LotesResumenClaseInstrumento(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
            If Not (DT Is Nothing) Then
                Dim ncol As Integer = 5
                For Each dr As DataRow In DT.Rows
                    oCells(3, 3) = UIUtility.ConvertirFechaaString(CDec(dr("Fecha1")))
                    oCells(3, 5) = UIUtility.ConvertirFechaaString(CDec(dr("Fecha2")))
                    oSheet.Range(ncol.ToString() + ":" + ncol.ToString()).Copy()
                    oSheet.Range((ncol + 1).ToString() + ":" + (ncol + 1).ToString()).PasteSpecial(Excel.XlPasteType.xlPasteAll)
                    oCells(ncol, 2) = IIf(dr("Clase").Equals(DBNull.Value), "", dr("Clase"))
                    oCells(ncol, 3) = IIf(dr("InteresGanadoT").Equals(DBNull.Value), "", dr("InteresGanadoT"))
                    oCells(ncol, 4) = IIf(dr("GananciaPerdidaT").Equals(DBNull.Value), "", dr("GananciaPerdidaT"))
                    oCells(ncol, 5) = IIf(dr("InteresGanadoT_1").Equals(DBNull.Value), "", dr("InteresGanadoT_1"))
                    oCells(ncol, 6) = IIf(dr("GananciaPerdidaT").Equals(DBNull.Value), "", dr("GananciaPerdidaT"))
                    oCells(ncol, 7) = IIf(dr("Rentabilidad").Equals(DBNull.Value), "", dr("Rentabilidad"))
                    oCells(ncol, 8) = IIf(dr("Fluctuacion").Equals(DBNull.Value), "", dr("Fluctuacion"))
                    oCells(ncol, 9) = IIf(dr("RentabilidadSOL").Equals(DBNull.Value), "", dr("RentabilidadSOL"))
                    oCells(ncol, 10) = IIf(dr("FluctuacionSOL").Equals(DBNull.Value), "", dr("FluctuacionSOL"))
                    ncol += 1
                    can = ncol
                Next
                oSheet.SaveAs(sFile)
                oBook.Save()
                oBook.Close()
                If sFile <> "" Then
                    Response.Clear()
                    Response.ContentType = "application/xls"
                    Response.AddHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(sFile))
                    Response.WriteFile(sFile)
                    Response.End()
                End If
            Else
                AlertaJS("No se encontraron registros para la fecha indicada.")
            End If
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
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarFiltros()
        End If
    End Sub
    Protected Sub btnVista_Click(sender As Object, e As System.EventArgs) Handles btnVista.Click
        Try
            Reporte()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
End Class
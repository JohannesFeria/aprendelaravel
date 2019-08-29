Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Runtime.InteropServices.Marshal
Imports System.IO

Partial Class Modulos_Gestion_Reportes_ReportesMandatos_frmReporteOperacionesMandatos
    Inherits BasePage

#Region "Eventos de la página"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                InicializarFechas()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            ExcelGenerate()
        Catch ex As Exception
            AlertaJS("Ocurrió un error: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
#End Region

#Region "Métodos de la página"
    Private Sub CargarCombos()
        CargarPortafolio()
    End Sub
    Private Sub InicializarFechas()
        txtFechaInicio.Text = FechaActual
        txtFechaFin.Text = FechaActual
    End Sub
    Private Sub CargarPortafolio()
        Dim objPortafolioBM As New PortafolioBM
        Dim dt As DataTable = objPortafolioBM.PortafolioListar("", "MANDA", "A")
        HelpCombo.LlenarListBox(lbPortafolio, dt, "CodigoPortafolio", "Descripcion", False)
    End Sub
    Private Function ObtenerPortafolios() As String
        Dim strPortafolio As String = String.Empty
        For Each item As ListItem In lbPortafolio.Items
            If item.Selected Then
                strPortafolio += item.Value & ","
            End If
        Next
        ObtenerPortafolios = strPortafolio
    End Function
    Private Sub ExcelGenerate()
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets
        Dim oSheet As Excel.Worksheet, oSheet2 As Excel.Worksheet
        Dim oCells As Excel.Range
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtOperaciones As DataTable = Nothing
            Dim dtVencimientos As DataTable = Nothing
            Dim objReporte As New ReporteGestionBM
            dtOperaciones = objReporte.ReporteOperacionesVencimientosOTC_Mandatos(ObtenerPortafolios(), UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text))
            dtVencimientos = objReporte.ReporteVencimientoOperaciones(ObtenerPortafolios, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text))
            If dtOperaciones IsNot Nothing Or dtVencimientos IsNot Nothing Then
                If dtOperaciones.Rows.Count > 0 Or dtVencimientos.Rows.Count > 0 Then
                    Dim sFile As String, sTemplate As String
                    sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RptOpeVen_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                    If File.Exists(sFile) Then File.Delete(sFile)
                    sTemplate = RutaPlantillas() & "\" & "PlantillaReporteOperacionesMandatos.xlsx"
                    oExcel.Visible = False : oExcel.DisplayAlerts = False
                    oBooks = oExcel.Workbooks
                    oBooks.Open(sTemplate)
                    oBook = oBooks.Item(1)
                    oSheets = oBook.Worksheets

                    FillOperationDetail(dtOperaciones, oSheets, oSheet, oCells, sFile)
                    FillMaturity(dtVencimientos, oSheets, oSheet2, oCells, sFile)

                    oBook.Save()
                    oBook.Close()
                    Response.Clear()
                    Response.ContentType = "application/xls"
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + "RptOpeVen__" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xlsx")
                    Response.WriteFile(sFile)
                    Response.End()
                Else
                    Throw New Exception("No existen datos a la fecha para la generación del reporte")
                End If
            Else
                Throw New Exception("No existen datos a la fecha para la generación del reporte")
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
    End Sub
    Private Function Rango(ByVal Numero As Integer) As String
        Dim array() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                 "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ",
                                 "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ",
                                 "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ",
                                 "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ",
                                 "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ",
                                 "FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ"}
        Return array(Numero - 1)
    End Function
    Private Sub FillOperationDetail(ByVal dtOperation As DataTable, ByRef oSheets As Excel.Sheets, ByRef oSheet As Excel.Worksheet, ByVal oCells As Excel.Range, ByVal sFile As String)
        oSheet = CType(oSheets.Item(1), Excel.Worksheet)
        oCells = oSheet.Cells
        oSheet.SaveAs(sFile)

        Dim index As Integer = 2
        Dim consecutive As Integer = 1
        For Each drOperacion As DataRow In dtOperation.Rows
            oCells(index, 1) = consecutive
            If IsDBNull(drOperacion("FechaOperacion")) Then
                oCells(index, 2) = ""
                oCells(index, 3) = ""
            Else
                oCells(index, 2) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(drOperacion("FechaOperacion"))))
                oCells(index, 3) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(drOperacion("FechaLiquidacion"))))
            End If
            oCells(index, 4) = drOperacion("TipoInstrumento")
            oCells(index, 5) = drOperacion("CodigoMnemonico")
            oCells(index, 6) = drOperacion("CodigoISIN")
            oCells(index, 7) = drOperacion("NroDeposito")
            oCells(index, 8) = drOperacion("CodigoMoneda")
            oCells(index, 9) = drOperacion("BaseCuponAnual")
            If IsDBNull(drOperacion("YTM")) Then
                oCells(index, 10) = "0"
            Else
                oCells(index, 10) = Decimal.Parse(drOperacion("YTM")) / 100
            End If
            oCells(index, 11) = drOperacion("MontoNominalOrdenado")
            oCells(index, 12) = drOperacion("ValorNominalResidual")
            oCells(index, 13) = drOperacion("CantidadOperacion")
            oCells(index, 14) = drOperacion("ValorPagado")
            oCells(index, 15) = drOperacion("TotalComisiones")
            oCells(index, 16) = drOperacion("IGV")
            oCells(index, 17) = drOperacion("MontoNetoOperacion")
            If IsDBNull(drOperacion("PrecioNegociacionSucio")) Then
                oCells(index, 18) = "0"
            Else
                oCells(index, 18) = Decimal.Parse(drOperacion("PrecioNegociacionSucio")) / 100
            End If
            oCells(index, 19) = drOperacion("DescripcionOperacion")
            If IsDBNull(drOperacion("FechaVencimiento")) Then
                oCells(index, 20) = ""
            Else
                oCells(index, 20) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(drOperacion("FechaVencimiento"))))
            End If
            oCells(index, 21) = drOperacion("NombrePortafolio")
            oCells(index, 22) = drOperacion("Observacion")
            index += 1
            consecutive += 1
            'oSheet.Rows(index).Insert()
            'oSheet.Range(Rango(1) + index.ToString(), Rango(22) + index.ToString()).Borders.LineStyle = _
            '    oSheet.Range(Rango(1) + (index - 1).ToString(), Rango(22) + (index - 1).ToString()).Borders.LineStyle
        Next
    End Sub
    Private Sub FillMaturity(ByVal dtVencimientos As DataTable, ByRef oSheets As Excel.Sheets, ByRef oSheet2 As Excel.Worksheet, ByVal oCells As Excel.Range, ByVal sFile As String)
        oSheet2 = CType(oSheets.Item(2), Excel.Worksheet)
        oCells = oSheet2.Cells
        oSheet2.SaveAs(sFile)
        Dim index As Integer = 2
        For Each drVencimiento As DataRow In dtVencimientos.Rows
            oCells(index, 1) = index - 1
            If IsDBNull(drVencimiento("FechaInicioCupon")) Then
                oCells(index, 2) = ""
            Else
                oCells(index, 2) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(drVencimiento("FechaInicioCupon"))))
            End If
            oCells(index, 3) = drVencimiento("CodigoMoneda")
            oCells(index, 4) = drVencimiento("MontoInicial")
            oCells(index, 5) = Decimal.Parse(drVencimiento("TasaCupon")) / 100
            If IsDBNull(drVencimiento("FechaVencimientoCupon")) Then
                oCells(index, 6) = ""
            Else
                oCells(index, 6) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(drVencimiento("FechaVencimientoCupon"))))
            End If
            oCells(index, 7) = drVencimiento("MontoVencimiento")
            oCells(index, 8) = drVencimiento("Institucion_Instrumento")
            If IsDBNull(drVencimiento("FechaEmision")) Then
                oCells(index, 9) = ""
            Else
                oCells(index, 9) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(drVencimiento("FechaEmision"))))
            End If
            oCells(index, 10) = drVencimiento("TipoInstrumento")
            oCells(index, 11) = drVencimiento("DescripcionPortafolio")
            oCells(index, 12) = drVencimiento("Comentario")
            index += 1
            'oSheet2.Rows(index).Insert()
            'oSheet2.Range(Rango(1) + index.ToString(), Rango(12) + index.ToString()).Borders.LineStyle = _
            '    oSheet2.Range(Rango(1) + (index - 1).ToString(), Rango(12) + (index - 1).ToString()).Borders.LineStyle
        Next
    End Sub
#End Region

End Class

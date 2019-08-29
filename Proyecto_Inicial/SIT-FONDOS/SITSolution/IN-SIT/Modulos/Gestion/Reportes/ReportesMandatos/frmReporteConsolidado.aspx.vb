Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Runtime.InteropServices.Marshal
Imports System.IO

Partial Class Modulos_Gestion_Reportes_ReportesMandatos_frmReporteConsolidado
    Inherits BasePage

#Region "Eventos de la página"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                txtFechaProceso.Text = FechaActual
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            GenerarExcel()
        Catch ex As Exception
            AlertaJS("Ocurrió un error: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
#End Region

#Region "Clases - Objetos"
    Public Class ConsolidadoExcelUtilBE
        Private _codigoTipoInstrumento As String
        Private _nombreTipoInstrumento As String
        Private _celda As String
        Private _hoja As String
        Private _formulaSuma As String

        Public Property CodigoTipoInstrumento() As String
            Get
                Return _codigoTipoInstrumento
            End Get
            Set(ByVal value As String)
                _codigoTipoInstrumento = value
            End Set
        End Property

        Public Property NombreTipoInstrumento() As String
            Get
                Return _nombreTipoInstrumento
            End Get
            Set(ByVal value As String)
                _nombreTipoInstrumento = value
            End Set
        End Property

        Public Property FormulaSuma() As String
            Get
                Return _formulaSuma
            End Get
            Set(ByVal value As String)
                _formulaSuma = value
            End Set
        End Property

        Public Property Hoja() As String
            Get
                Return _hoja
            End Get
            Set(ByVal value As String)
                _hoja = value
            End Set
        End Property

        Public Property Celda() As String
            Get
                Return _celda
            End Get
            Set(ByVal value As String)
                _celda = value
            End Set
        End Property

    End Class
#End Region
    
#Region "Métodos de la página"
    Private Sub CargarCombos()
        CargarPortafolio()
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
    Private Sub GenerarExcel()
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets
        Dim oSheet As Excel.Worksheet
        Dim oSheetResume As Excel.Worksheet
        Dim oCells As Excel.Range
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtConsolidado As DataSet = Nothing
            Dim objReporte As New ReporteGestionBM
            dtConsolidado = objReporte.ReporteConsolidado(ObtenerPortafolios(), UIUtility.ConvertirFechaaDecimal(txtFechaProceso.Text))
            If dtConsolidado IsNot Nothing Then
                If dtConsolidado.Tables.Count > 0 Then
                    If dtConsolidado.Tables(0).Rows.Count > 0 Then
                        Dim sFile As String, sTemplate As String
                        sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RptVal_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                        If File.Exists(sFile) Then File.Delete(sFile)
                        sTemplate = RutaPlantillas() & "\" & "PlantillaReporteConsolidadoMandatos.xls"
                        oExcel.Visible = False : oExcel.DisplayAlerts = False
                        oBooks = oExcel.Workbooks
                        oBooks.Open(sTemplate)
                        oBook = oBooks.Item(1)

                        oSheets = oBook.Worksheets

                        oSheet = CType(oSheets.Item(2), Excel.Worksheet)
                        oCells = oSheet.Cells
                        oSheet.SaveAs(sFile)
                        Dim listaExcelConsolidado As New List(Of ConsolidadoExcelUtilBE)

                        'Crear hojas de excel disponibles para su llenado de datos
                        CreateWorksheets(dtConsolidado.Tables(0), oBook)
                        Dim indice As Integer = 11
                        FillConsolidado(dtConsolidado, oSheet, oCells, indice, listaExcelConsolidado)
                        FillConsolidadoDetallado(dtConsolidado, oBook, oSheets, oSheet, oCells, indice, sFile)

                        oSheetResume = CType(oSheets.Item(1), Excel.Worksheet)


                        oSheets("Reporte Consolidado").Move(Before:=oSheets(dtConsolidado.Tables(1).Rows(0)("DescripcionPortafolio")))


                        oCells = oSheetResume.Cells


                        FillResumen(dtConsolidado, oSheetResume, oCells, 5, sFile, listaExcelConsolidado)
                        'oSheets(1).Delete()
                        oBook.Save()
                        'oCells(1, 2) = ddlPortafolio.SelectedItem.Text
                        'oCells(2, 2) = tbFechaOperacion.Text

                        'oExcel.Cells.EntireColumn.AutoFit()
                        oBook.Save()
                        oBook.Close()
                        Response.Clear()
                        Response.ContentType = "application/xls"
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + "RptVal_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
                        Response.WriteFile(sFile)
                        Response.End()
                    Else
                        Throw New Exception("No existen datos a la fecha para la generación del reporte")
                    End If
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
    Private Sub CreateWorksheets(ByVal dt As DataTable, ByRef oBook As Excel.Workbook)
        Dim dtWorkSheet As DataTable = dt.DefaultView.ToTable(True, "CodigoPortafolioSBS")
        For Each dr As DataRow In dtWorkSheet.Rows
            'oBook.Worksheets.Copy(oBook.Sheets(oBook.Sheets.Count))
            oBook.Worksheets.Copy(oBook.Sheets.Item(oBook.Sheets.Count))
        Next
        Dim numberSheetNeed As Integer = dtWorkSheet.Rows.Count + 2
        Dim numberSheetDontNeed As Integer = oBook.Sheets.Count - numberSheetNeed
        For i = 1 To numberSheetDontNeed
            oBook.Sheets(1).Delete()
        Next
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
    Private Sub FillResumen(dtConsolidado As DataSet, oSheet As Excel.Worksheet, oCells As Excel.Range, indice As Integer, ByVal sFile As String, ByRef listaExcelConsolidado As List(Of ConsolidadoExcelUtilBE))
        oSheet.Name = "Resumen"
        Dim formulaSuma As String = ""
        oSheet.SaveAs(sFile)
        indice += 1
        Dim indiceInferior As Integer = indice
        For Each obj As ConsolidadoExcelUtilBE In listaExcelConsolidado
            oSheet.Rows(indice).Insert()
            oCells(indice, 2) = obj.NombreTipoInstrumento
            oCells(indice, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

            Dim referenciaCelda = "='" & obj.Hoja & "'!" & obj.Celda
            oCells(indice, 3) = referenciaCelda
            oCells(indice, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            indice += 1
        Next
        formulaSuma = String.Format("=SUM(${0}$" & (indiceInferior).ToString() & ":${0}$" & (indice - 1).ToString & ")", Rango(3))
        oCells(indice, 3).Formula = formulaSuma

        If listaExcelConsolidado.Count > 0 Then
            oSheet.Rows(indiceInferior - 1).Delete()
        End If

    End Sub
    Private Sub FillConsolidado(ByVal ds As DataSet, ByRef oSheet As Excel.Worksheet, ByVal oCells As Excel.Range, ByVal indice As Integer, ByRef listaExcelConsolidado As List(Of ConsolidadoExcelUtilBE))
        'Nombrar a la hoja de cálculo
        oSheet.Name = "Reporte Consolidado"
        oCells(5, 2) = "ESTRUCTURA DEL PORTAFOLIO AL: " & UIUtility.ConvertirStringaFecha(txtFechaProceso.Text)
        oCells(5, 4) = UIUtility.ConvertirStringaFecha(txtFechaProceso.Text)
        oCells(6, 3) = ds.Tables(0).Rows(0)("TipoCambio").ToString

        'LLenar los campos de Caja, CxC, Patrimonio
        Dim dtValorCuota As DataTable
        Dim objReporte As New ReporteGestionBM
        Dim vectorPatrimonio(,) As String = {{"0", "0"}, {"0", "0"}, {"0", "0"}, {"0", "0"}, {"0", "0"}, {"0", "0"}}
        'For i = 0 To 5
        '    For j = 0 To 2
        '        vectorPatrimonio(i, j) = "0"
        '    Next
        'Next
        For Each StrPortafolio As String In ObtenerPortafolios().Split(",")
            If StrPortafolio <> String.Empty Then
                dtValorCuota = objReporte.ListarReporteValorCuotaLimiteMandatos(StrPortafolio, UIUtility.ConvertirFechaaDecimal(Me.txtFechaProceso.Text.Trim))
                If dtValorCuota IsNot Nothing Then
                    If dtValorCuota.Rows.Count > 0 Then
                        'Soles
                        Dim rowSoles As DataRow() = dtValorCuota.Select("CodigoMonedaCta='NSOL'")
                        Dim rowDolares As DataRow() = dtValorCuota.Select("CodigoMonedaCta='DOL'")


                        vectorPatrimonio(0, 0) = Decimal.Parse(vectorPatrimonio(0, 0)) + Decimal.Parse(rowSoles(0)("totalInversiones"))
                        vectorPatrimonio(1, 0) = Decimal.Parse(vectorPatrimonio(1, 0)) + Decimal.Parse(rowSoles(0)("cxc"))
                        vectorPatrimonio(2, 0) = Decimal.Parse(vectorPatrimonio(2, 0)) + Decimal.Parse(rowSoles(0)("cxpcierre"))
                        vectorPatrimonio(3, 0) = Decimal.Parse(vectorPatrimonio(3, 0)) + Decimal.Parse(rowSoles(0)("Caja"))
                        'vectorPatrimonio(4, 0) = Decimal.Parse(vectorPatrimonio(4, 0)) + Decimal.Parse(dtValorCuota(0)("patrimonioCierre"))
                        vectorPatrimonio(5, 0) = dtValorCuota(0)("CodigoMonedaCta")

                        vectorPatrimonio(0, 1) = Decimal.Parse(vectorPatrimonio(0, 1)) + Decimal.Parse(rowDolares(0)("totalInversiones"))
                        vectorPatrimonio(1, 1) = Decimal.Parse(vectorPatrimonio(1, 1)) + Decimal.Parse(rowDolares(0)("cxc"))
                        vectorPatrimonio(2, 1) = Decimal.Parse(vectorPatrimonio(2, 1)) + Decimal.Parse(rowDolares(0)("cxpcierre"))
                        vectorPatrimonio(3, 1) = Decimal.Parse(vectorPatrimonio(3, 1)) + Decimal.Parse(rowDolares(0)("Caja"))
                        'vectorPatrimonio(4, 1) = Decimal.Parse(vectorPatrimonio(4, 1)) + Decimal.Parse(dtValorCuota(0)("patrimonioCierre"))
                        vectorPatrimonio(5, 1) = dtValorCuota(0)("CodigoMonedaCta")

                    End If
                End If
            End If
        Next

        oCells(15, 19) = vectorPatrimonio(3, 0) 'Caja Soles
        oCells(15, 19).NumberFormat = "#,##0.00"
        oCells(15, 21) = vectorPatrimonio(3, 0) 'Caja Soles
        oCells(15, 21).NumberFormat = "#,##0.00"

        oCells(15, 30) = "=" & Rango(21) & "15/$AD$1"
        oCells(15, 30).NumberFormat = "#,##0.00%"

        oCells(16, 19) = (Decimal.Parse(vectorPatrimonio(3, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'Caja Dólares
        oCells(16, 19).NumberFormat = "#,##0.00"
        oCells(16, 21) = (Decimal.Parse(vectorPatrimonio(3, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'Caja Dólares
        oCells(16, 21).NumberFormat = "#,##0.00"


        oCells(16, 30) = "=" & Rango(21) & "16/$AD$1"
        oCells(16, 30).NumberFormat = "#,##0.00%"

        oCells(17, 19) = Decimal.Parse(vectorPatrimonio(3, 0)) + (Decimal.Parse(vectorPatrimonio(3, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'Caja Total Soles
        oCells(17, 19).NumberFormat = "#,##0.00"
        oCells(17, 21) = Decimal.Parse(vectorPatrimonio(3, 0)) + (Decimal.Parse(vectorPatrimonio(3, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'Caja Total Soles
        oCells(17, 21).NumberFormat = "#,##0.00"

        oCells(17, 30) = "=" & Rango(21) & "17/$AD$1"
        oCells(17, 30).NumberFormat = "#,##0.00%"


        oCells(18, 19) = vectorPatrimonio(1, 0) 'CxC Soles
        oCells(18, 19).NumberFormat = "#,##0.00"
        oCells(18, 21) = vectorPatrimonio(1, 0) 'CxC Soles
        oCells(18, 21).NumberFormat = "#,##0.00"

        oCells(18, 30) = "=" & Rango(21) & "18/$AD$1"
        oCells(18, 30).NumberFormat = "#,##0.00%"
        oCells(19, 19) = (Decimal.Parse(vectorPatrimonio(1, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxC Dólares
        oCells(19, 19).NumberFormat = "#,##0.00"
        oCells(19, 21) = (Decimal.Parse(vectorPatrimonio(1, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxC Dólares
        oCells(19, 21).NumberFormat = "#,##0.00"

        oCells(19, 30) = "=" & Rango(21) & "19/$AD$1"
        oCells(19, 30).NumberFormat = "#,##0.00%"

        oCells(20, 19) = Decimal.Parse(vectorPatrimonio(1, 0)) + (Decimal.Parse(vectorPatrimonio(1, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxC Total Soles
        oCells(20, 19).NumberFormat = "#,##0.00"
        oCells(20, 21) = Decimal.Parse(vectorPatrimonio(1, 0)) + (Decimal.Parse(vectorPatrimonio(1, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxC Total Soles
        oCells(20, 21).NumberFormat = "#,##0.00"

        oCells(20, 30) = "=" & Rango(21) & "20/$AD$1"
        oCells(20, 30).NumberFormat = "#,##0.00%"

        oCells(21, 19) = vectorPatrimonio(2, 0) 'CxP Soles
        oCells(21, 19).NumberFormat = "#,##0.00"
        oCells(21, 21) = vectorPatrimonio(2, 0) 'CxP Soles
        oCells(21, 21).NumberFormat = "#,##0.00"


        oCells(21, 30) = "=" & Rango(21) & "21/$AD$1"
        oCells(21, 30).NumberFormat = "#,##0.00%"

        oCells(22, 19) = Decimal.Parse(vectorPatrimonio(2, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))  'CxP Dólares
        oCells(22, 19).NumberFormat = "#,##0.00"
        oCells(22, 21) = Decimal.Parse(vectorPatrimonio(2, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio")) 'CxP Dólares
        oCells(22, 21).NumberFormat = "#,##0.00"


        oCells(22, 30) = "=" & Rango(21) & "22/$AD$1"
        oCells(22, 30).NumberFormat = "#,##0.00%"

        oCells(23, 19) = Decimal.Parse(vectorPatrimonio(2, 0)) + (Decimal.Parse(vectorPatrimonio(2, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxP Total Soles
        oCells(23, 19).NumberFormat = "#,##0.00"
        oCells(23, 21) = Decimal.Parse(vectorPatrimonio(2, 0)) + (Decimal.Parse(vectorPatrimonio(2, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxP Total Soles
        oCells(23, 21).NumberFormat = "#,##0.00"

        'oCells(24, 6) = Decimal.Parse(vectorPatrimonio(4, 0)) + (Decimal.Parse(vectorPatrimonio(4, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'Patrimonio
        'oCells(24, 6).NumberFormat = "#,##0.00"
        oCells(23, 30) = "=" & Rango(21) & "20/$AD$1"
        oCells(23, 30).NumberFormat = "#,##0.00%"
        oCells(25, 21) = "0"
        oCells(25, 21).NumberFormat = "#,##0.00"

        'oCells(26, 6) = Decimal.Parse(vectorPatrimonio(4, 0)) + (Decimal.Parse(vectorPatrimonio(4, 1)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'Patrimonio
        'oCells(26, 6).NumberFormat = "#,##0.00"

        'Llenado de las operaciones de valorización
        Dim correlativo As Integer = 1
        Dim codigoTipoInstrumento As String = String.Empty
        Dim dtTipoInstrumento As DataTable = Nothing
        'Dim sumaPatFon As Decimal = 0
        'sumaPatFon = ObtenerSumaPatrimonioFondos(ds.Tables(1))
        dtTipoInstrumento = ds.Tables(0).DefaultView.ToTable(True, "CodigoTipoInstrumentoSBS")
        Dim formulaSumaProductoTotal As String = "=("
        If dtTipoInstrumento IsNot Nothing Then
            For Each dtRow As DataRow In dtTipoInstrumento.Rows
                Dim drAux() As DataRow = ds.Tables(1).Select("CodigoTipoInstrumentoSBS in ('" & dtRow("CodigoTipoInstrumentoSBS") & "')")
                If drAux IsNot Nothing Then
                    If drAux.Length > 0 Then
                        indice += 1
                        oSheet.Rows(indice).Insert()
                        oCells(indice, 2) = "N°"
                        oCells(indice, 2).Font.Bold = True
                        oCells(indice, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                        oCells(indice, 3) = drAux(0)("DescripcionTipoInstrumento").ToString
                        oCells(indice, 3).Font.Bold = True
                        oCells(indice, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                        Dim oUtil As New ConsolidadoExcelUtilBE
                        oUtil.NombreTipoInstrumento = drAux(0)("DescripcionTipoInstrumento").ToString
                        oUtil.CodigoTipoInstrumento = drAux(0)("CodigoTipoInstrumentoSBS").ToString
                        oUtil.Hoja = oSheet.Name


                        Dim topeIndiceSuma As Integer = indice + drAux.Count()
                        Dim formulaSuma As String = "=SUM(${0}$" & (indice + 1).ToString() & ":${0}$" & topeIndiceSuma.ToString() + ")"
                        Dim formulaProductoSuma As String = "=SUMPRODUCT(${0}$" & (indice + 1).ToString() & ":${0}$" & topeIndiceSuma.ToString() + ",${1}$" & (indice + 1).ToString() & ":${1}$" & topeIndiceSuma.ToString() & ")/SUM(${0}$" & (indice + 1).ToString() & ":${0}$" & topeIndiceSuma.ToString() + ")"
                        formulaSumaProductoTotal = formulaSumaProductoTotal & "+SUMPRODUCT(${0}$" & (indice + 1).ToString() & ":${0}$" & topeIndiceSuma.ToString() + ",${1}$" & (indice + 1).ToString() & ":${1}$" & topeIndiceSuma.ToString() & ")"
                        Dim indiceInicio As Integer = indice
                        indice += 1
                        oSheet.Rows(indice).Insert()
                        correlativo = 1
                        For Each dr As DataRow In drAux
                            oCells(indice, 2) = correlativo
                            oCells(indice, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                            'oCells(indice, 3) = IIf(dr("CodigoTipoInstrumentoSBS") = "60", dr("Tercero"), dr("CodigoNemonico"))
                            oCells(indice, 3) = dr("CodigoNemonico")
                            'oCells(indice, 4) = IIf(dr("CodigoTipoInstrumentoSBS") = "60", dr("CodigoNemonico"), dr("CodigoISIN"))
                            oCells(indice, 4) = "'" & dr("CodigoISIN")
                            oCells(indice, 5) = dr("Tercero")
                            oCells(indice, 6) = dr("CodigoMoneda")
                            oCells(indice, 7) = dr("Rating")
                            oCells(indice, 8) = dr("EmpresaClasificadora")
                            oCells(indice, 9) = dr("FechaClasificacion")
                            If IsDBNull(dr("FechaVencimiento")) Then
                                oCells(indice, 10) = ""
                            Else
                                oCells(indice, 10) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(dr("FechaVencimiento"))))
                            End If
                            If IsDBNull(dr("ProximoCuponFecha")) Then
                                oCells(indice, 11) = ""
                            Else
                                oCells(indice, 11) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(dr("ProximoCuponFecha"))))
                            End If
                            oCells(indice, 12) = dr("ProximoCuponMonto")
                            oCells(indice, 12).NumberFormat = "#,##0.00"

                            oCells(indice, 13) = dr("CantidadOperacion")
                            oCells(indice, 13).NumberFormat = "0"

                            oCells(indice, 14) = dr("MontoNominalInicial")
                            oCells(indice, 14).NumberFormat = "#,##0.00"

                            oCells(indice, 15) = dr("MontoNominalVigente")
                            oCells(indice, 15).NumberFormat = "#,##0.00"

                            If IsDBNull(dr("PrecioOperacion")) Then
                                oCells(indice, 16) = "0"
                                oCells(indice, 16).NumberFormat = "#,##0.00%"
                            Else
                                oCells(indice, 16) = Decimal.Parse(dr("PrecioOperacion")) / 100
                                oCells(indice, 16).NumberFormat = "#,##0.00%"
                            End If
                            'oCells(indice, 16) = dr("PrecioOperacion") / 100
                            'oCells(indice, 16).NumberFormat = "#,##0.00%"

                            oCells(indice, 17) = dr("ValorCompra")
                            oCells(indice, 17).NumberFormat = "#,##0.00"

                            If IsDBNull(dr("TIRCOM_PrecioLimpio")) Then
                                oCells(indice, 18) = "0"
                                oCells(indice, 18).NumberFormat = "#,##0.00%"
                            Else
                                oCells(indice, 18) = Decimal.Parse(dr("TIRCOM_PrecioLimpio")) / 100
                                oCells(indice, 18).NumberFormat = "#,##0.00%"
                            End If

                            oCells(indice, 19) = dr("TIRCOM_ValorActual")
                            oCells(indice, 19).NumberFormat = "#,##0.00"

                            If IsDBNull(dr("TIRRAZ_PrecioLimpio")) Then
                                oCells(indice, 20) = "0"
                                oCells(indice, 20).NumberFormat = "#,##0.00%"
                            Else
                                oCells(indice, 20) = Decimal.Parse(dr("TIRRAZ_PrecioLimpio")) / 100
                                oCells(indice, 20).NumberFormat = "#,##0.00%"
                            End If

                            oCells(indice, 21).NumberFormat = "#,##0.00"
                            If IsDBNull(dr("PRELIM_ValorActual")) Then
                                oCells(indice, 21) = "0"
                            Else
                                oCells(indice, 21) = dr("PRELIM_ValorActual")
                            End If


                            If IsDBNull(dr("PRELIM_ValorActual_PrecioLimpio")) Then
                                oCells(indice, 22) = "0"
                                oCells(indice, 22).NumberFormat = "#,##0.00"
                            Else
                                If dr("CodigoTipoInstrumentoSBS") = "60" Then
                                    oCells(indice, 22) = Decimal.Parse(dr("PRELIM_ValorActual_PrecioLimpio"))
                                Else
                                    oCells(indice, 22) = Decimal.Parse(dr("PRELIM_ValorActual_PrecioLimpio")) / 100
                                End If
                                oCells(indice, 22).NumberFormat = "#,##0.00"
                            End If

                            If IsDBNull(dr("TasaDescuentoSBS")) Then
                                oCells(indice, 23) = "0"
                                oCells(indice, 23).NumberFormat = "#,##0.00%"
                            Else
                                oCells(indice, 23) = Decimal.Parse(dr("TasaDescuentoSBS")) / 100
                                oCells(indice, 23).NumberFormat = "#,##0.00%"
                            End If

                            If IsDBNull(dr("TasaCupon")) Then
                                oCells(indice, 24) = "0"
                                oCells(indice, 24).NumberFormat = "#,##0.00%"
                            Else
                                oCells(indice, 24) = Decimal.Parse(dr("TasaCupon")) / 100
                                oCells(indice, 24).NumberFormat = "#,##0.00%"
                            End If

                            If IsDBNull(dr("TasaDescuentoCompra")) Then
                                oCells(indice, 25) = "0"
                                oCells(indice, 25).NumberFormat = "#,##0.00%"
                            Else
                                oCells(indice, 25) = Decimal.Parse(dr("TasaDescuentoCompra")) / 100
                                oCells(indice, 25).NumberFormat = "#,##0.00%"
                            End If

                            oCells(indice, 26) = dr("InteresesCorridos")
                            oCells(indice, 26).NumberFormat = "#,##0.00"

                            oCells(indice, 27) = dr("Sobre_BajoPrecio")
                            oCells(indice, 27).NumberFormat = "#,##0.00"

                            oCells(indice, 28) = dr("FluctuacionValor")
                            oCells(indice, 28).NumberFormat = "#,##0.00"
                            oCells(indice, 28).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

                            'oCells(indice, 29) = dr("DuracionSBS")
                            'oCells(indice, 29) = IIf(dr("CodigoTipoInstrumentoSBS") = "60", DateDiff(DateInterval.Day, _
                            '                              UIUtility.ConvertirStringaFecha(txtFechaProceso.Text), _
                            '                              UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(dr("FechaVencimiento")))) _
                            '                              ) / 365, dr("DuracionSBS"))
                            'oCells(indice, 29).NumberFormat = "#,##0.00"
                            If dr("CodigoTipoInstrumentoSBS") = "60" Then
                                oCells(indice, 29) = DateDiff(DateInterval.Day, _
                                                          UIUtility.ConvertirStringaFecha(txtFechaProceso.Text), _
                                                          UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(dr("FechaVencimiento")))) _
                                                          ) / 365
                                oCells(indice, 29).NumberFormat = "#,##0.00"
                            Else
                                If IsDBNull(dr("DuracionSBS")) Then
                                    oCells(indice, 29) = "0"
                                Else
                                    oCells(indice, 29) = Decimal.Parse(dr("DuracionSBS"))
                                End If
                                oCells(indice, 29).NumberFormat = "#,##0.00"
                            End If


                            'oCells(indice, 30) = dr("ParticipacionPatrimonio")
                            'oCells(indice, 30) = dr("VPNFondo") / sumaPatFon
                            'oCells(indice, 30) = dr("TIRRAZ_ValorActual") / dr("PatrimonioFondo")
                            'oCells(indice, 30) = oCells(11, 27)
                            'oCells(indice, 30).Formula = "=SI.ERROR(" & Rango(18) & indice.ToString & "/$AD$1,"""")"
                            oCells(indice, 30) = "=" & Rango(21) & indice.ToString & "/$AD$1"
                            oCells(indice, 30).NumberFormat = "#,##0.00%"

                            oCells(indice, 31).Formula = "=IF(" & Rango(10) & indice.ToString & "-$" & Rango(4) & "$5>365,""Largo Plazo"",""Corto Plazo"")"

                            oSheet.Range(Rango(2) + indice.ToString, Rango(31) + indice.ToString).Font.Bold = False
                            'oSheet.Range(Rango(2) + indice.ToString, Rango(27) + indice.ToString).Font.Name = "Arial"
                            'oSheet.Range(Rango(2) + indice.ToString, Rango(27) + indice.ToString).Font.Size = 8

                            oCells(indice, 32) = dr("ActividadEconomica")

                            '---
                            If IsDBNull(dr("FechaOperacion")) Then
                                oCells(indice, 33) = ""
                            Else
                                oCells(indice, 33) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(dr("FechaOperacion"))))
                            End If
                            '--

                            correlativo += 1
                            indice += 1
                            oSheet.Rows(indice).Insert()
                        Next
                        oUtil.FormulaSuma = formulaSuma
                        oUtil.Celda = "$" & Rango(21) & "$" & indiceInicio.ToString
                        listaExcelConsolidado.Add(oUtil)
                        FillCeldaSumatoria(oCells, formulaSuma, 17, indiceInicio)
                        FillCeldaSumatoria(oCells, formulaSuma, 19, indiceInicio)
                        FillCeldaSumatoria(oCells, formulaSuma, 21, indiceInicio)
                        FillCeldaSumatoria(oCells, formulaSuma, 22, indiceInicio)
                        FillCeldaSumaProducto(oCells, formulaProductoSuma, 21, 23, indiceInicio, False)
                        FillCeldaSumaProducto(oCells, formulaProductoSuma, 21, 24, indiceInicio, False)
                        FillCeldaSumaProducto(oCells, formulaProductoSuma, 21, 25, indiceInicio, False)
                        FillCeldaSumatoria(oCells, formulaSuma, 26, indiceInicio)
                        FillCeldaSumatoria(oCells, formulaSuma, 27, indiceInicio)
                        FillCeldaSumatoria(oCells, formulaSuma, 28, indiceInicio)
                        FillCeldaSumaProducto(oCells, formulaProductoSuma, 21, 29, indiceInicio, False)
                        FillCeldaSumatoria(oCells, formulaSuma, 30, indiceInicio)

                    End If
                End If
            Next
            FillDuracionSBSTotal(oCells, 29, indice)
            formulaSumaProductoTotal = formulaSumaProductoTotal + ")/${0}$" & (indice + 13).ToString()
            FillCeldaSumaProducto(oCells, formulaSumaProductoTotal, 21, 23, indice + 13, True)
            FillCeldaSumaProducto(oCells, formulaSumaProductoTotal, 19, 25, indice + 13, True)
            FillCeldaSumaProducto(oCells, formulaSumaProductoTotal, 21, 29, indice + 13, True)

            Dim oUtilCajaTotal As New ConsolidadoExcelUtilBE
            oUtilCajaTotal.NombreTipoInstrumento = "CAJA"
            oUtilCajaTotal.CodigoTipoInstrumento = "CAJA"
            oUtilCajaTotal.FormulaSuma = ""
            oUtilCajaTotal.Celda = "$" & Rango(21) & "$" & (indice + 6).ToString
            oUtilCajaTotal.Hoja = oSheet.Name

            Dim oUtilCxCTotal As New ConsolidadoExcelUtilBE
            oUtilCxCTotal.NombreTipoInstrumento = "CTAS. POR COBRAR"
            oUtilCxCTotal.CodigoTipoInstrumento = "CxC"
            oUtilCxCTotal.FormulaSuma = ""
            oUtilCxCTotal.Celda = "$" & Rango(21) & "$" & (indice + 9).ToString
            oUtilCxCTotal.Hoja = oSheet.Name

            Dim oUtilCxPTotal As New ConsolidadoExcelUtilBE
            oUtilCxPTotal.NombreTipoInstrumento = "CTAS. POR PAGAR"
            oUtilCxPTotal.CodigoTipoInstrumento = "CxP"
            oUtilCxPTotal.FormulaSuma = ""
            oUtilCxPTotal.Celda = "$" & Rango(21) & "$" & (indice + 12).ToString
            oUtilCxPTotal.Hoja = oSheet.Name


            listaExcelConsolidado.Add(oUtilCajaTotal)
            listaExcelConsolidado.Add(oUtilCxCTotal)
            listaExcelConsolidado.Add(oUtilCxPTotal)

        End If
    End Sub
    Private Sub FillCeldaSumaProducto(ByVal oCells As Excel.Range, ByVal formula As String, ByVal columnaNumerica As Integer, ByVal columnaPorcentual As Integer, ByVal fila As Integer, ByVal total As Boolean)
        oCells(fila, columnaPorcentual).Font.Bold = True

        If total = False Then
            oCells(fila, columnaPorcentual).Interior.Color = System.Drawing.Color.FromArgb(217, 217, 217)
        End If



        oCells(fila, columnaPorcentual).Formula = String.Format(formula, Rango(columnaNumerica), Rango(columnaPorcentual))
        oCells(fila, columnaPorcentual).NumberFormat = "#,##0.00"

        If (columnaPorcentual = 23 Or columnaPorcentual = 24 Or columnaPorcentual = 25 Or columnaPorcentual = 30) Then
            oCells(fila, columnaPorcentual).NumberFormat = "#,##0.00%"
        End If

        If (columnaPorcentual = 29) Then
            oCells(fila, columnaPorcentual).NumberFormat = "#,##0.00"
        End If

        'oCells(fila, columna).NumberFormat = IIf(columna = 30, "#,##0.00%", "#,##0.00")
        oCells(fila, columnaPorcentual).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    End Sub
    Private Sub FillDuracionSBSTotal(ByVal oCells As Excel.Range, ByVal columna As Integer, ByVal fila As Integer)

        If (oCells(fila + 4, 21).Value IsNot Nothing) Then
            If (Decimal.Equals(Decimal.Parse(oCells(fila + 4, 21).Value), 0D)) = False Then
                oCells(fila + 4, columna) = "=1/360"
                oCells(fila + 4, columna).NumberFormat = "#,##0.00"
            End If
        End If

        If (oCells(fila + 5, 21).Value IsNot Nothing) Then
            If (Decimal.Equals(Decimal.Parse(oCells(fila + 5, 21).Value), 0D)) = False Then
                oCells(fila + 5, columna) = "=1/360"
                oCells(fila + 5, columna).NumberFormat = "#,##0.00"
            End If
        End If

        'Total
        If (oCells(fila + 6, 21).Value IsNot Nothing) Then
            If (Decimal.Equals(Decimal.Parse(oCells(fila + 6, 21).Value), 0D)) = False Then
                oCells(fila + 6, columna) = "=1/360"
                oCells(fila + 6, columna).NumberFormat = "#,##0.00"
            End If
        End If

        If (oCells(fila + 7, 21).Value IsNot Nothing) Then
            If (Decimal.Equals(Decimal.Parse(oCells(fila + 7, 21).Value), 0D)) = False Then
                oCells(fila + 7, columna) = "=1/360"
                oCells(fila + 7, columna).NumberFormat = "#,##0.00"
            End If
        End If

        If (oCells(fila + 8, 21).Value IsNot Nothing) Then
            If (Decimal.Equals(Decimal.Parse(oCells(fila + 8, 21).Value), 0D)) = False Then
                oCells(fila + 8, columna) = "=1/360"
                oCells(fila + 8, columna).NumberFormat = "#,##0.00"
            End If
        End If

        'Total
        If (oCells(fila + 9, 21).Value IsNot Nothing) Then
            If (Decimal.Equals(Decimal.Parse(oCells(fila + 9, 21).Value), 0D)) = False Then
                oCells(fila + 9, columna) = "=1/360"
                oCells(fila + 9, columna).NumberFormat = "#,##0.00"
            End If
        End If


        If (oCells(fila + 10, 21).Value IsNot Nothing) Then
            If (Decimal.Equals(Decimal.Parse(oCells(fila + 10, 21).Value), 0D)) = False Then
                oCells(fila + 10, columna) = "=1/360"
                oCells(fila + 10, columna).NumberFormat = "#,##0.00"
            End If
        End If

        If (oCells(fila + 11, 21).Value IsNot Nothing) Then
            If (Decimal.Equals(Decimal.Parse(oCells(fila + 11, 21).Value), 0D)) = False Then
                oCells(fila + 11, columna) = "=1/360"
                oCells(fila + 11, columna).NumberFormat = "#,##0.00"
            End If
        End If

        'Total
        If (oCells(fila + 12, 21).Value IsNot Nothing) Then
            If (Decimal.Equals(Decimal.Parse(oCells(fila + 12, 21).Value), 0D)) = False Then
                oCells(fila + 12, columna) = "=1/360"
                oCells(fila + 12, columna).NumberFormat = "#,##0.00"
            End If
        End If
    End Sub
    Private Sub FillCeldaSumatoria(ByVal oCells As Excel.Range, ByVal formula As String, ByVal columna As Integer, ByVal fila As Integer)
        oCells(fila, columna).Font.Bold = True
        oCells(fila, columna).Interior.Color = System.Drawing.Color.FromArgb(217, 217, 217)
        oCells(fila, columna).Formula = String.Format(formula, Rango(columna))
        oCells(fila, columna).NumberFormat = "#,##0.00"

        If (columna = 23 Or columna = 24 Or columna = 25 Or columna = 30) Then
            oCells(fila, columna).NumberFormat = "#,##0.00%"
        End If

        If (columna = 29) Then
            oCells(fila, columna).NumberFormat = "#,##0.00"
        End If

        'oCells(fila, columna).NumberFormat = IIf(columna = 30, "#,##0.00%", "#,##0.00")
        oCells(fila, columna).Interior.Color = System.Drawing.Color.FromArgb(217, 217, 217)
        oCells(fila, columna).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    End Sub
    Private Sub FillConsolidadoDetallado(ByVal ds As DataSet, ByRef oBook As Excel.Workbook, ByRef oSheets As Excel.Sheets, ByRef oSheet As Excel.Worksheet, ByVal oCells As Excel.Range, ByVal indice As Integer, ByVal sFile As String)
        Dim dtPortafolio As DataTable = ds.Tables(0).DefaultView.ToTable(True, "CodigoPortafolioSBS")
        Dim indexSheet As Integer = 2
        Dim correlativo As Integer = 1
        For Each drPortafolio As DataRow In dtPortafolio.Rows
            'oBook.Worksheets.Copy(oBookAux.Sheets(oBookAux.Sheets.Count))
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(indexSheet), Excel.Worksheet)
            oCells = oSheet.Cells
            oSheet.SaveAs(sFile)

            Dim drTipoInstrumentos() As DataRow = ds.Tables(0).Select("CodigoPortafolioSBS = '" & drPortafolio("CodigoPortafolioSBS").ToString() & "'")
            If drTipoInstrumentos IsNot Nothing Then
                If drTipoInstrumentos.Length > 0 Then

                    Dim dtValorCuota As DataTable
                    Dim objReporte As New ReporteGestionBM
                    Dim vectorPatrimonio(7) As String
                    For i = 0 To vectorPatrimonio.Length - 1
                        vectorPatrimonio(i) = "0"
                    Next

                    dtValorCuota = objReporte.ListarReporteValorCuotaLimiteMandatos(drPortafolio("CodigoPortafolioSBS").ToString(), UIUtility.ConvertirFechaaDecimal(Me.txtFechaProceso.Text.Trim))
                    If dtValorCuota IsNot Nothing Then
                        If dtValorCuota.Rows.Count > 0 Then
                            Dim rowSoles As DataRow() = dtValorCuota.Select("CodigoMonedaCta='NSOL'")
                            Dim rowDolares As DataRow() = dtValorCuota.Select("CodigoMonedaCta='DOL'")

                            vectorPatrimonio(0) = Decimal.Parse(vectorPatrimonio(0)) + Decimal.Parse(rowSoles(0)("totalInversiones"))
                            vectorPatrimonio(1) = Decimal.Parse(vectorPatrimonio(1)) + Decimal.Parse(rowSoles(0)("cxc"))
                            vectorPatrimonio(2) = Decimal.Parse(vectorPatrimonio(2)) + Decimal.Parse(rowSoles(0)("cxpcierre"))
                            vectorPatrimonio(3) = Decimal.Parse(vectorPatrimonio(3)) + Decimal.Parse(rowSoles(0)("Caja"))
                            vectorPatrimonio(4) = Decimal.Parse(vectorPatrimonio(4)) + Decimal.Parse(rowDolares(0)("Caja")) 'Caja Dolares
                            vectorPatrimonio(5) = Decimal.Parse(vectorPatrimonio(5)) + Decimal.Parse(rowDolares(0)("cxpcierre")) 'CxP Dolares
                            vectorPatrimonio(6) = Decimal.Parse(vectorPatrimonio(6)) + Decimal.Parse(rowDolares(0)("cxc")) 'CxC Dolares
                            'vectorPatrimonio(4) = Decimal.Parse(vectorPatrimonio(4)) + Decimal.Parse(dtValorCuota(0)("patrimonioCierre"))
                            'vectorPatrimonio(5) = rowSoles(0)("CodigoMonedaFondo")
                        End If
                    End If

                    '  If vectorPatrimonio(5) = "NSOL" Then
                    oCells(15, 19) = vectorPatrimonio(3) 'Caja Soles
                    oCells(15, 19).NumberFormat = "#,##0.00"
                    oCells(15, 21) = vectorPatrimonio(3) 'Caja Soles
                    oCells(15, 21).NumberFormat = "#,##0.00"

                    oCells(15, 30) = "=" & Rango(21) & "15/$AD$1"
                    oCells(15, 30).NumberFormat = "#,##0.00%"
                    oCells(16, 30) = "=" & Rango(21) & "16/$AD$1"
                    oCells(16, 30).NumberFormat = "#,##0.00%"
                    oCells(17, 30) = "=" & Rango(21) & "17/$AD$1"
                    oCells(17, 30).NumberFormat = "#,##0.00%"

                    oCells(16, 19) = (Decimal.Parse(vectorPatrimonio(4)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'Caja Dólares
                    oCells(16, 19).NumberFormat = "#,##0.00"
                    oCells(16, 21) = (Decimal.Parse(vectorPatrimonio(4)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'Caja Dólares
                    oCells(16, 21).NumberFormat = "#,##0.00"

                    oCells(17, 19) = (Decimal.Parse(vectorPatrimonio(3)) + (Decimal.Parse(vectorPatrimonio(4)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio")))) 'Caja Total Soles
                    oCells(17, 19).NumberFormat = "#,##0.00"
                    oCells(17, 21) = (Decimal.Parse(vectorPatrimonio(3)) + (Decimal.Parse(vectorPatrimonio(4)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio")))) 'Caja Total Soles
                    oCells(17, 21).NumberFormat = "#,##0.00"

                    oCells(18, 19) = vectorPatrimonio(1) 'CxC Soles
                    oCells(18, 19).NumberFormat = "#,##0.00"
                    oCells(18, 21) = vectorPatrimonio(1) 'CxC Soles
                    oCells(18, 21).NumberFormat = "#,##0.00"

                    oCells(18, 30) = "=" & Rango(21) & "18/$AD$1"
                    oCells(18, 30).NumberFormat = "#,##0.00%"
                    oCells(19, 30) = "=" & Rango(21) & "19/$AD$1"
                    oCells(19, 30).NumberFormat = "#,##0.00%"
                    oCells(20, 30) = "=" & Rango(21) & "20/$AD$1"
                    oCells(20, 30).NumberFormat = "#,##0.00%"

                    oCells(19, 19) = (Decimal.Parse(vectorPatrimonio(6)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio")))  'CxC Dólares
                    oCells(19, 19).NumberFormat = "#,##0.00"
                    oCells(19, 21) = (Decimal.Parse(vectorPatrimonio(6)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxC Dólares
                    oCells(19, 21).NumberFormat = "#,##0.00"

                    oCells(20, 19) = Decimal.Parse(vectorPatrimonio(1)) + (Decimal.Parse(vectorPatrimonio(6)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxC Total Soles
                    oCells(20, 19).NumberFormat = "#,##0.00"
                    oCells(20, 21) = Decimal.Parse(vectorPatrimonio(1)) + (Decimal.Parse(vectorPatrimonio(6)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxC Total Soles
                    oCells(20, 21).NumberFormat = "#,##0.00"

                    oCells(21, 19) = vectorPatrimonio(2) 'CxP Soles
                    oCells(21, 19).NumberFormat = "#,##0.00"
                    oCells(21, 21) = vectorPatrimonio(2) 'CxP Soles
                    oCells(21, 21).NumberFormat = "#,##0.00"

                    oCells(21, 30) = "=" & Rango(21) & "21/$AD$1"
                    oCells(21, 30).NumberFormat = "#,##0.00%"
                    oCells(22, 30) = "=" & Rango(21) & "22/$AD$1"
                    oCells(22, 30).NumberFormat = "#,##0.00%"
                    oCells(23, 30) = "=" & Rango(21) & "23/$AD$1"
                    oCells(23, 30).NumberFormat = "#,##0.00%"

                    oCells(22, 19) = (Decimal.Parse(vectorPatrimonio(5)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxP Dólares
                    oCells(22, 19).NumberFormat = "#,##0.00"
                    oCells(23, 19) = Decimal.Parse(vectorPatrimonio(2)) + (Decimal.Parse(vectorPatrimonio(5)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxP Total Soles
                    oCells(23, 19).NumberFormat = "#,##0.00"

                    oCells(22, 21) = (Decimal.Parse(vectorPatrimonio(5)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxP Dólares
                    oCells(22, 21).NumberFormat = "#,##0.00"
                    oCells(23, 21) = Decimal.Parse(vectorPatrimonio(2)) + (Decimal.Parse(vectorPatrimonio(5)) * Decimal.Parse(ds.Tables(0).Rows(0)("TipoCambio"))) 'CxP Total Soles
                    oCells(23, 21).NumberFormat = "#,##0.00"

                    'oCells(24, 6) = Decimal.Parse(vectorPatrimonio(4)) 'Patrimonio
                    'oCells(24, 6).NumberFormat = "#,##0.00"

                    oCells(25, 21) = "0"
                    oCells(25, 21).NumberFormat = "#,##0.00"

                    Dim formulaSumaProductoTotal As String = "=("
                    For Each drTipIns As DataRow In drTipoInstrumentos
                        Dim drOperaciones() As DataRow = ds.Tables(1).Select("CodigoPortafolioSBS = '" & drTipIns("CodigoPortafolioSBS") & "' AND CodigoTipoInstrumentoSBS = '" & drTipIns("CodigoTipoInstrumentoSBS") & "'")
                        If drOperaciones IsNot Nothing Then
                            If drOperaciones.Length > 0 Then
                                indice += 1
                                oSheet.Rows(indice).Insert()
                                oCells(indice, 2) = "N°"
                                oCells(indice, 2).Font.Bold = True
                                oCells(indice, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                oCells(indice, 3) = drTipIns("DescripcionTipoInstrumento")
                                oCells(indice, 3).Font.Bold = True
                                Dim topeIndiceSuma As Integer = indice + drOperaciones.Count()
                                Dim formulaSuma As String = "=SUM(${0}$" & (indice + 1).ToString() & ":${0}$" & topeIndiceSuma.ToString() + ")"
                                Dim formulaProductoSuma As String = "=SUMPRODUCT(${0}$" & (indice + 1).ToString() & ":${0}$" & topeIndiceSuma.ToString() + ",${1}$" & (indice + 1).ToString() & ":${1}$" & topeIndiceSuma.ToString() & ")/SUM(${0}$" & (indice + 1).ToString() & ":${0}$" & topeIndiceSuma.ToString() + ")"
                                formulaSumaProductoTotal = formulaSumaProductoTotal & "+SUMPRODUCT(${0}$" & (indice + 1).ToString() & ":${0}$" & topeIndiceSuma.ToString() + ",${1}$" & (indice + 1).ToString() & ":${1}$" & topeIndiceSuma.ToString() & ")"
                                Dim indiceInicio As Integer = indice
                                indice += 1
                                oSheet.Rows(indice).Insert()
                                correlativo = 1
                                For Each dr As DataRow In drOperaciones
                                    oCells(indice, 2) = correlativo
                                    oCells(indice, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                    'oCells(indice, 3) = IIf(dr("CodigoTipoInstrumentoSBS") = "60", dr("Tercero"), dr("CodigoNemonico"))
                                    'oCells(indice, 4) = IIf(dr("CodigoTipoInstrumentoSBS") = "60", dr("CodigoNemonico"), dr("CodigoISIN"))
                                    oCells(indice, 3) = dr("CodigoNemonico")
                                    oCells(indice, 4) = "'" & dr("CodigoISIN")
                                    oCells(indice, 5) = dr("Tercero")
                                    oCells(indice, 6) = dr("CodigoMoneda")
                                    oCells(indice, 7) = dr("Rating")
                                    oCells(indice, 8) = dr("EmpresaClasificadora")
                                    oCells(indice, 9) = dr("FechaClasificacion")
                                    If IsDBNull(dr("FechaVencimiento")) Then
                                        oCells(indice, 10) = ""
                                    Else
                                        oCells(indice, 10) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(dr("FechaVencimiento"))))
                                    End If
                                    If IsDBNull(dr("ProximoCuponFecha")) Then
                                        oCells(indice, 11) = ""
                                    Else
                                        oCells(indice, 11) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(dr("ProximoCuponFecha"))))
                                    End If
                                    oCells(indice, 12) = dr("ProximoCuponMonto")
                                    oCells(indice, 12).NumberFormat = "#,##0.00"

                                    oCells(indice, 13) = dr("CantidadOperacion")
                                    oCells(indice, 13).NumberFormat = "0"

                                    oCells(indice, 14) = dr("MontoNominalInicial")
                                    oCells(indice, 14).NumberFormat = "#,##0.00"

                                    oCells(indice, 15) = dr("MontoNominalVigente")
                                    oCells(indice, 15).NumberFormat = "#,##0.00"

                                    If IsDBNull(dr("PrecioOperacion")) Then
                                        oCells(indice, 16) = "0"
                                        oCells(indice, 16).NumberFormat = "#,##0.00%"
                                    Else
                                        oCells(indice, 16) = Decimal.Parse(dr("PrecioOperacion")) / 100
                                        oCells(indice, 16).NumberFormat = "#,##0.00%"
                                    End If

                                    'oCells(indice, 16) = dr("PrecioOperacion") / 100
                                    'oCells(indice, 16).NumberFormat = "#,##0.00%"

                                    oCells(indice, 17) = dr("ValorCompra")
                                    oCells(indice, 17).NumberFormat = "#,##0.00"

                                    If IsDBNull(dr("TIRCOM_PrecioLimpio")) Then
                                        oCells(indice, 18) = "0"
                                        oCells(indice, 18).NumberFormat = "#,##0.00%"
                                    Else
                                        oCells(indice, 18) = Decimal.Parse(dr("TIRCOM_PrecioLimpio")) / 100
                                        oCells(indice, 18).NumberFormat = "#,##0.00%"
                                    End If

                                    oCells(indice, 19) = dr("TIRCOM_ValorActual")
                                    oCells(indice, 19).NumberFormat = "#,##0.00"

                                    If IsDBNull(dr("TIRRAZ_PrecioLimpio")) Then
                                        oCells(indice, 20) = "0"
                                        oCells(indice, 20).NumberFormat = "#,##0.00%"
                                    Else
                                        oCells(indice, 20) = Decimal.Parse(dr("TIRRAZ_PrecioLimpio")) / 100
                                        oCells(indice, 20).NumberFormat = "#,##0.00%"
                                    End If

                                    oCells(indice, 21).NumberFormat = "#,##0.00"
                                    If IsDBNull(dr("PRELIM_ValorActual")) Then
                                        oCells(indice, 21) = "0"
                                    Else
                                        oCells(indice, 21) = dr("PRELIM_ValorActual")
                                    End If

                                    If IsDBNull(dr("PRELIM_ValorActual_PrecioLimpio")) Then
                                        oCells(indice, 22) = "0"
                                        oCells(indice, 22).NumberFormat = "#,##0.00"
                                    Else
                                        If drTipIns("CodigoTipoInstrumentoSBS") = "60" Then
                                            oCells(indice, 22) = Decimal.Parse(dr("PRELIM_ValorActual_PrecioLimpio"))
                                        Else
                                            oCells(indice, 22) = Decimal.Parse(dr("PRELIM_ValorActual_PrecioLimpio")) / 100
                                        End If
                                        oCells(indice, 22).NumberFormat = "#,##0.00"
                                    End If
                                    If IsDBNull(dr("TasaDescuentoSBS")) Then
                                        oCells(indice, 23) = "0"
                                        oCells(indice, 23).NumberFormat = "#,##0.00%"
                                    Else
                                        oCells(indice, 23) = Decimal.Parse(dr("TasaDescuentoSBS")) / 100
                                        oCells(indice, 23).NumberFormat = "#,##0.00%"
                                    End If
                                    If IsDBNull(dr("TasaCupon")) Then
                                        oCells(indice, 24) = "0"
                                        oCells(indice, 24).NumberFormat = "#,##0.00%"
                                    Else
                                        oCells(indice, 24) = Decimal.Parse(dr("TasaCupon")) / 100
                                        oCells(indice, 24).NumberFormat = "#,##0.00%"
                                    End If
                                    If IsDBNull(dr("TasaDescuentoCompra")) Then
                                        oCells(indice, 25) = "0"
                                        oCells(indice, 25).NumberFormat = "#,##0.00%"
                                    Else
                                        oCells(indice, 25) = Decimal.Parse(dr("TasaDescuentoCompra")) / 100
                                        oCells(indice, 25).NumberFormat = "#,##0.00%"
                                    End If
                                    oCells(indice, 26) = dr("InteresesCorridos")
                                    oCells(indice, 26).NumberFormat = "#,##0.00"

                                    oCells(indice, 27) = dr("Sobre_BajoPrecio")
                                    oCells(indice, 27).NumberFormat = "#,##0.00"

                                    oCells(indice, 28) = dr("FluctuacionValor")
                                    oCells(indice, 28).NumberFormat = "#,##0.00"

                                    'oCells(indice, 29) = dr("DuracionSBS")
                                    'oCells(indice, 29) = IIf(dr("CodigoTipoInstrumentoSBS") = "60", DateDiff(DateInterval.Day, _
                                    '                      UIUtility.ConvertirStringaFecha(txtFechaProceso.Text), _
                                    '                      UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(dr("FechaVencimiento")))) _
                                    '                      ) / 365, dr("DuracionSBS"))
                                    'oCells(indice, 29).NumberFormat = "#,##0.00"

                                    If dr("CodigoTipoInstrumentoSBS") = "60" Then
                                        oCells(indice, 29) = DateDiff(DateInterval.Day, _
                                                                  UIUtility.ConvertirStringaFecha(txtFechaProceso.Text), _
                                                                  UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(dr("FechaVencimiento")))) _
                                                                  ) / 365
                                        oCells(indice, 29).NumberFormat = "#,##0.00"
                                    Else
                                        If IsDBNull(dr("DuracionSBS")) Then
                                            oCells(indice, 29) = "0"
                                        Else
                                            oCells(indice, 29) = Decimal.Parse(dr("DuracionSBS"))
                                        End If
                                        oCells(indice, 29).NumberFormat = "#,##0.00"
                                    End If


                                    'oCells(indice, 30) = dr("ParticipacionPatrimonio")
                                    'oCells(indice, 30) = dr("TIRRAZ_ValorActual") / dr("PatrimonioFondo")
                                    'oCells(indice, 30) = oCells(11, 27)
                                    'oCells(indice, 30).Formula = "=SI.ERROR(" & Rango(18) & indice.ToString & "/$AD$1,"""")"
                                    oCells(indice, 30) = "=" & Rango(21) & indice.ToString & "/$AD$1"
                                    oCells(indice, 30).NumberFormat = "#,##0.00%"

                                    oCells(indice, 31).Formula = "=IF(" & Rango(10) & indice.ToString & "-$" & Rango(4) & "$5>365,""Largo Plazo"",""Corto Plazo"")"

                                    oSheet.Range(Rango(2) + indice.ToString, Rango(31) + indice.ToString).Font.Bold = False
                                    'oSheet.Range(Rango(2) + indice.ToString, Rango(27) + indice.ToString).Font.Name = "Arial"
                                    'oSheet.Range(Rango(2) + indice.ToString, Rango(27) + indice.ToString).Font.Size = 8
                                    oCells(indice, 32) = dr("ActividadEconomica")

                                    '--Agregar Fecha Operacion
                                    If IsDBNull(dr("FechaOperacion")) Then
                                        oCells(indice, 33) = ""
                                    Else
                                        oCells(indice, 33) = UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(Decimal.Parse(dr("FechaOperacion"))))
                                    End If
                                    '---
                                    correlativo += 1
                                    indice += 1
                                    oSheet.Rows(indice).Insert()
                                Next

                                FillCeldaSumatoria(oCells, formulaSuma, 17, indiceInicio)
                                FillCeldaSumatoria(oCells, formulaSuma, 19, indiceInicio)
                                FillCeldaSumatoria(oCells, formulaSuma, 21, indiceInicio)
                                FillCeldaSumatoria(oCells, formulaSuma, 22, indiceInicio)
                                FillCeldaSumaProducto(oCells, formulaProductoSuma, 21, 23, indiceInicio, False)
                                FillCeldaSumaProducto(oCells, formulaProductoSuma, 21, 24, indiceInicio, False)
                                FillCeldaSumaProducto(oCells, formulaProductoSuma, 21, 25, indiceInicio, False)
                                FillCeldaSumatoria(oCells, formulaSuma, 26, indiceInicio)
                                FillCeldaSumatoria(oCells, formulaSuma, 27, indiceInicio)
                                FillCeldaSumatoria(oCells, formulaSuma, 28, indiceInicio)
                                FillCeldaSumaProducto(oCells, formulaProductoSuma, 21, 29, indiceInicio, False)
                                FillCeldaSumatoria(oCells, formulaSuma, 30, indiceInicio)
                            End If
                        End If
                    Next
                    FillDuracionSBSTotal(oCells, 29, indice)
                    formulaSumaProductoTotal = formulaSumaProductoTotal + ")/${0}$" & (indice + 13).ToString()
                    FillCeldaSumaProducto(oCells, formulaSumaProductoTotal, 21, 23, indice + 13, True)
                    FillCeldaSumaProducto(oCells, formulaSumaProductoTotal, 19, 25, indice + 13, True)
                    FillCeldaSumaProducto(oCells, formulaSumaProductoTotal, 21, 29, indice + 13, True)
                End If
            End If
            oCells(4, 2) = drTipoInstrumentos(0)("DescripcionPortafolio").ToString
            'oCells(5, 3) = "ESTRUCTURA DEL PORTAFOLIO AL: " & UIUtility.ConvertirStringaFecha(txtFechaProceso.Text)
            'oCells(6, 2) = "Tipo de Cambio: " & ds.Tables(0).Rows(0)("TipoCambio").ToString
            oCells(5, 2) = "ESTRUCTURA DEL PORTAFOLIO AL: " & UIUtility.ConvertirStringaFecha(txtFechaProceso.Text)
            oCells(5, 4) = UIUtility.ConvertirStringaFecha(txtFechaProceso.Text)
            oCells(6, 3) = ds.Tables(0).Rows(0)("TipoCambio")
            oSheet.Name = drTipoInstrumentos(0)("DescripcionPortafolio").ToString
            oSheet.SaveAs(sFile)
            indexSheet += 1
            indice = 11
        Next
    End Sub
    Private Function ObtenerSumaPatrimonioFondos(ByVal dt As DataTable) As Decimal
        ObtenerSumaPatrimonioFondos = 0
        Dim dtPortafolio As DataTable = dt.DefaultView.ToTable(True, "CodigoPortafolioSBS")
        For Each dtRowPor As DataRow In dtPortafolio.Rows
            For Each dtPatPor As DataRow In dt.Rows
                If dtRowPor("CodigoPortafolioSBS") = dtPatPor("CodigoPortafolioSBS") Then
                    ObtenerSumaPatrimonioFondos += Decimal.Parse(dtPatPor("PatrimonioFondo"))
                    Exit For
                End If
            Next
        Next
    End Function
#End Region

End Class

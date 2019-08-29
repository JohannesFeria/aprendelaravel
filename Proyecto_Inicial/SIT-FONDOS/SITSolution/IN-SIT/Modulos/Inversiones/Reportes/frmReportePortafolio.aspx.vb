Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Runtime.InteropServices.Marshal
Imports System.Text
Imports System.Data
Imports Microsoft.Office.Core
Imports System.Globalization
Imports System.Threading
Imports UIUtility
Imports System.IO
Partial Class Modulos_Inversiones_Reportes_frmReportePortafolio
    Inherits BasePage
    Dim oOrdenPrevOrden As New OrdenPreOrdenInversionBM
    Private Sub CargarPortafolio()
        Dim oPortafolio As New PortafolioBM
        Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True, "--Todos--")
    End Sub
    Private Sub GenerarReporte()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing, oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing, oSheet As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Dim oSheetTemplate As Excel.Worksheet = Nothing
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim ds As DataSet = oOrdenPrevOrden.ResumenPortafolio(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
            Dim PrimeraFila As Integer = 3
            If ds.Tables.Count > 0 Then
                Dim dt_Caja As DataTable = ds.Tables(0)
                Dim dt As DataTable = ds.Tables(1)
                Dim sFile As String = String.Empty, sTemplate As String = String.Empty, Nombre As String = String.Empty
                Nombre = "ResumenPortafolio" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Nombre
                Dim Indice As Integer = 1
                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutaPlantillas() & "\" & "Plantilla_ResumenPortafolio.xls"
                oExcel.Visible = False : oExcel.DisplayAlerts = False
                oExcel.DisplayAlerts = False
                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheetTemplate = CType(oSheets.Item(1), Excel.Worksheet)
                Dim i As Integer = 1
                If ddlPortafolio.SelectedValue = "" Then
                    For Each lt As ListItem In ddlPortafolio.Items
                        If lt.Value <> "" Then
                            oSheetTemplate.Copy(After:=oBook.Sheets(i))
                            i += 1
                            oSheet = oBook.Sheets(i)
                            oSheet.Name = lt.Text
                            oCells = oSheet.Cells
                            DumpData_Fondos(dt, dt_Caja, 3, oSheet, oCells, lt.Text)
                            oCells.EntireColumn.AutoFit()
                        End If
                    Next
                    oBook.Worksheets(1).Delete()
                Else
                    oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                    oCells = oSheet.Cells
                    oSheet.Name = ddlPortafolio.SelectedItem.Text
                    DumpData_Fondos(dt, dt_Caja, 3, oSheet, oCells, ddlPortafolio.SelectedItem.Text)
                End If
                oSheet.SaveAs(sFile)
                oExcel.Cells.EntireColumn.AutoFit()
                oBook.Save()
                oBook.Close()
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Nombre)
                Response.WriteFile(sFile)
                Response.End()
            Else
                AlertaJS("No existen registros que mostrar para esta fecha y portafolio.")
            End If
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
    Sub DumpData_Fondos(ByVal dt As DataTable, dtCajas As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range, Portafolio As String)
        Dim rows, rowsCajas As DataRow()
        rows = dt.Select("Portafolio = '" + Portafolio + "'")
        rowsCajas = dtCajas.Select("Portafolio = '" + Portafolio + "'")
        Dim iCajas As Integer = 0
        For Each dr As DataRow In rowsCajas
            oSheet.Rows(FilaInicial.ToString & ":" & FilaInicial.ToString).Copy()
            oSheet.Rows((FilaInicial + 1).ToString & ":" & (FilaInicial + 1).ToString).Insert(Excel.XlDirection.xlDown)
            Dim i As Integer = 0
            Do While i <= dtCajas.Columns.Count - 1
                oCells(FilaInicial, i + 1) = dr(i)
                i = i + 1
            Loop
            FilaInicial += 1
            iCajas += 1
        Next
        FormatoFila(oSheet, FilaInicial)
        If iCajas > 0 Then
            FormulaFila(oCells, FilaInicial, rowsCajas.Length)
        End If
        oCells(FilaInicial, 1) = "CAJA"
        FilaInicial += 3
        Dim Clase As String = "", Mercado As String = "", Moneda As String = "", CantGru As Integer = 0,
        ClaseAct As String = "", MercadoAct As String = "", MonedaAct As String = ""
        Dim Agergar As Boolean = False
        For Each dr As DataRow In rows
            Dim i As Integer = 0
            Do While i <= dt.Columns.Count - 1
                oCells(FilaInicial, i + 1) = dr(i)
                If i = 2 Then
                    ClaseAct = Clase
                    If Clase = "" Then
                        Clase = dr(i)
                    Else
                        If Not Clase = dr(i) Then
                            Agergar = True
                            Clase = dr(i)
                        End If
                    End If
                End If
                If i = 9 Then
                    MercadoAct = Mercado
                    If Mercado = "" Then
                        Mercado = dr(i)
                    Else
                        If Not Mercado = dr(i) Then
                            Agergar = True
                            Mercado = dr(i)
                        End If
                    End If
                End If
                If i = 10 Then
                    MonedaAct = Moneda
                    If Moneda = "" Then
                        Moneda = dr(i)
                    Else
                        If Not Moneda = dr(i) Then
                            Agergar = True
                            Moneda = dr(i)
                        End If
                    End If
                End If
                i = i + 1
            Loop
            CantGru += 1
            If Agergar Then
                oSheet.Rows((FilaInicial + 1).ToString & ":" & (FilaInicial + 1).ToString).Copy()
                oSheet.Rows((FilaInicial).ToString & ":" & (FilaInicial).ToString).Insert(Excel.XlDirection.xlUp)
                oSheet.Rows((FilaInicial).ToString & ":" & (FilaInicial).ToString).Insert(Excel.XlDirection.xlUp)
                FormatoFila(oSheet, FilaInicial)
                FormulaFila(oCells, FilaInicial, CantGru)
                oCells(FilaInicial, 1) = (ClaseAct + "/" + MercadoAct + "/" + MonedaAct).ToUpper
                Agergar = False
                FilaInicial += 3
                CantGru = 1
            Else
                FilaInicial += 1
            End If
        Next
        FormatoFila(oSheet, FilaInicial)
        If CantGru > 0 Then
            FormulaFila(oCells, FilaInicial, CantGru)
        End If
        If CantGru = 1 Then
            oCells(FilaInicial, 1) = (Clase + "/" + Mercado + "/" + Moneda).ToUpper
        End If
        If ClaseAct <> "" And MercadoAct <> "" And MonedaAct <> "" Then
            oCells(FilaInicial, 1) = (ClaseAct + "/" + MercadoAct + "/" + MonedaAct).ToUpper
        End If
    End Sub
    Sub FormatoFila(ByVal oSheet As Excel.Worksheet, FilaInicial As Integer)
        Dim objRango As Object
        objRango = oSheet.Range("A" + FilaInicial.ToString + ":K" + FilaInicial.ToString)
        objRango.Interior.Color = RGB(149, 179, 215)
        objRango.Cells.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
        objRango.Cells.Font.Bold = True
    End Sub
    Sub FormulaFila(ByVal oCells As Excel.Range, FilaInicial As Integer, CantGru As Integer)
        oCells(FilaInicial, 5).Formula = "=SUM(E" + (FilaInicial - CantGru).ToString + ":E" + (FilaInicial - 1).ToString + ")"
        oCells(FilaInicial, 9).Formula = "=SUM(I" + (FilaInicial - CantGru).ToString + ":I" + (FilaInicial - 1).ToString + ")"
        oCells(FilaInicial, 7).Formula = "=SUMPRODUCT(G" + (FilaInicial - CantGru).ToString + ":G" + (FilaInicial - 1).ToString + "," +
        "E" + (FilaInicial - CantGru).ToString + ":E" + (FilaInicial - 1).ToString + ")/E" + FilaInicial.ToString
        oCells(FilaInicial, 8).Formula = "=SUMPRODUCT(H" + (FilaInicial - CantGru).ToString + ":H" + (FilaInicial - 1).ToString + "," +
        "E" + (FilaInicial - CantGru).ToString + ":E" + (FilaInicial - 1).ToString + ")/E" + FilaInicial.ToString
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarPortafolio()
        End If
    End Sub
    Protected Sub btnGenera_Click(sender As Object, e As System.EventArgs) Handles btnGenera.Click
        Try
            GenerarReporte()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
End Class
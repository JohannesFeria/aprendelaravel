Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports System.Globalization
Imports System.Threading
Imports System.IO

Partial Class Modulos_Gestion_Reportes_frmReportePromedioTasas
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            ddlAnio.Items.Insert(0, New ListItem(Date.Now.Year, Date.Now.Year))
            ddlAnio.Items.Insert(1, New ListItem(Date.Now.Year - 1, Date.Now.Year - 1))
            ddlAnio.Items.Insert(2, New ListItem(Date.Now.Year - 2, Date.Now.Year - 2))
            ddlAnio.Items.Insert(3, New ListItem(Date.Now.Year - 3, Date.Now.Year - 3))
            ddlAnio.Items.Insert(4, New ListItem(Date.Now.Year - 4, Date.Now.Year - 4))
            ddlAnio.Items.Insert(5, New ListItem(Date.Now.Year - 5, Date.Now.Year - 5))
        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim ds As DataSet
        Try
            ds = New ReportePromedioTasasBM().PromedioTasas(ddlAnio.SelectedValue + "0101", ddlAnio.SelectedValue + "1231", DatosRequest)
            Copia("Promedio Tasas", CType(ds.Tables(0), DataTable))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Public Sub Copia(ByVal Archivo As String, ByVal dt As DataTable)
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Dim sFile As String, sTemplate As String
        Dim oldCulture As CultureInfo
        oldCulture = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Archivo & "_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            sTemplate = RutaPlantillas() & "\PlantillaPromedioTasa.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Name = Archivo
            oCells = oSheet.Cells
            DumpData(dt, oCells)
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

    Private Sub DumpData(ByVal dt As DataTable, ByVal oCells As Excel.Range)
        Dim dr As DataRow, ary() As Object
        Dim iRow, mes, cont, contD, contS As Integer

        oCells(1, 10) = Me.ddlAnio.SelectedValue

        For iRow = 0 To dt.Rows.Count - 1
            dr = dt.Rows.Item(iRow)
            ary = dr.ItemArray

            If mes <> dt.Rows(iRow).Item(1) Then
                For cont = 1 To 12
                    If dt.Rows(iRow).Item(1) = cont Then
                        Select Case dt.Rows(iRow).Item(4)
                            Case "DOL"
                                If iRow <> 0 Then
                                    oCells(hiContD.Value, 33) = "=SUM(B" & hiContD.Value & ":AF" & hiContD.Value & ")/" & contD
                                    If contS = 0 Then
                                        oCells(hiContS.Value, 33) = "=SUM(B" & hiContS.Value & ":AF" & hiContS.Value & ")/" & contD
                                    Else
                                        oCells(hiContS.Value, 33) = "=SUM(B" & hiContS.Value & ":AF" & hiContS.Value & ")/" & contS
                                    End If
                                    oCells(hiContS.Value, 33).Font.ColorIndex = 41
                                    oCells(hiContD.Value, 33).Font.ColorIndex = 41
                                    oCells(hiContS.Value, 33).Font.Bold = True
                                    oCells(hiContD.Value, 33).Font.Bold = True
                                    contS = 0
                                End If
                                contD = 0
                                contD = contD + 1
                                oCells(((cont * 2) - 1) + 4, dt.Rows(iRow).Item(3) + 1) = dt.Rows(iRow).Item(2)
                                hiContD.Value = ((cont * 2) - 1) + 4
                                hiContS.Value = ((cont * 2) - 1) + 3
                            Case Else
                                If iRow <> 0 Then
                                    oCells(hiContS.Value, 33) = "=SUM(B" & hiContS.Value & ":AF" & hiContS.Value & ")/" & contS
                                    If contD = 0 Then
                                        oCells(hiContD.Value, 33) = "=SUM(B" & hiContD.Value & ":AF" & hiContD.Value & ")/" & contS
                                    Else
                                        oCells(hiContD.Value, 33) = "=SUM(B" & hiContD.Value & ":AF" & hiContD.Value & ")/" & contD
                                    End If
                                    oCells(hiContS.Value, 33).Font.ColorIndex = 41
                                    oCells(hiContD.Value, 33).Font.ColorIndex = 41
                                    oCells(hiContS.Value, 33).Font.Bold = True
                                    oCells(hiContD.Value, 33).Font.Bold = True
                                    contD = 0
                                End If
                                contS = 0
                                contS = contS + 1
                                oCells(((cont * 2) - 1) + 3, dt.Rows(iRow).Item(3) + 1) = dt.Rows(iRow).Item(2)
                                oCells(((cont * 2) - 1) + 4, dt.Rows(iRow).Item(3) + 1) = "0.00%"
                                oCells(((cont * 2) - 1) + 3, dt.Rows(iRow).Item(3) + 1).Interior.ColorIndex = 2
                                oCells(((cont * 2) - 1) + 4, dt.Rows(iRow).Item(3) + 1).Interior.ColorIndex = 2
                                hiContS.Value = ((cont * 2) - 1) + 3
                                hiContD.Value = ((cont * 2) - 1) + 4
                        End Select
                        Exit For
                    End If
                Next
                mes = dt.Rows(iRow).Item(1)
            Else
                Select Case dt.Rows(iRow).Item(4)
                    Case "DOL"
                        oCells(((cont * 2) - 1) + 4, dt.Rows(iRow).Item(3) + 1) = dt.Rows(iRow).Item(2)
                        oCells(((cont * 2) - 1) + 4, dt.Rows(iRow).Item(3) + 1).Interior.ColorIndex = 2
                        contD = contD + 1
                    Case Else
                        oCells(((cont * 2) - 1) + 3, dt.Rows(iRow).Item(3) + 1) = dt.Rows(iRow).Item(2)
                        oCells(((cont * 2) - 1) + 4, dt.Rows(iRow).Item(3) + 1) = "0.00%"
                        oCells(((cont * 2) - 1) + 3, dt.Rows(iRow).Item(3) + 1).Interior.ColorIndex = 2
                        oCells(((cont * 2) - 1) + 4, dt.Rows(iRow).Item(3) + 1).Interior.ColorIndex = 2
                        contS = contS + 1
                End Select
                mes = dt.Rows(iRow).Item(1)
            End If
        Next
        If contS = 0 Then
            oCells(((cont * 2) - 1) + 3, 33) = "=SUM(B26:AF26)/" & contD
        Else
            oCells(((cont * 2) - 1) + 3, 33) = "=SUM(B26:AF26)/" & contS
        End If
        If contD = 0 Then
            oCells(((cont * 2) - 1) + 4, 33) = "=SUM(B27:AF27)/" & contS
        Else
            oCells(((cont * 2) - 1) + 4, 33) = "=SUM(B27:AF27)/" & contD
        End If
        oCells(((cont * 2) - 1) + 3, 33).Font.ColorIndex = 41
        oCells(((cont * 2) - 1) + 4, 33).Font.ColorIndex = 41
        oCells(((cont * 2) - 1) + 3, 33).Font.Bold = True
        oCells(((cont * 2) - 1) + 4, 33).Font.Bold = True
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

End Class

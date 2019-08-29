Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports System.Globalization
Imports System.Threading
Imports System.IO

Partial Class Modulos_Gestion_Reportes_frmReporteControlForward
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            CargarPortafolio(True)
        End If
    End Sub

    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", False)
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim ds As DataSet
        Try
            ds = New ControlForwardBM().ControlForward(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), ddlPortafolio.SelectedValue, DatosRequest)
            Copia("Control Forward", CType(ds.Tables(0), DataTable))
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
            sTemplate = RutaPlantillas() & "\PlantillaForward.xls"
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
        Dim dr As DataRow
        Dim iRow, iCol As Integer

        hidB.Value = ""
        hidF.Value = ""
        hidFN.Value = ""
        oCells(4, 1) = Me.ddlPortafolio.SelectedItem.Text

        For iRow = 0 To dt.Rows.Count - 1
            dr = dt.Rows.Item(iRow)
            For iCol = 0 To dt.Columns.Count - 1
                oCells(iRow + 6, iCol + 1) = dr(iCol).ToString()
                oCells(iRow + 6, iCol + 1).Borders.LineStyle = 1
                If iCol = 0 Then
                    hidB.Value = hidB.Value & "+C" & iRow + 6
                    If dr(12) > 0 Then
                        hidFN.Value = hidFN.Value & "+I" & iRow + 6
                    ElseIf dr(12) < 0 Then
                        hidF.Value = hidF.Value & "+I" & iRow + 6
                    End If
                End If
            Next
        Next

        If hidB.Value <> "" Then
            oCells(iRow + 7, 1) = "TOTAL"
            oCells(iRow + 7, 3) = "=" & hidB.Value
        End If

        If hidFN.Value <> "" Then
            oCells(iRow + 8, 9) = "=" & hidFN.Value
        End If

        If hidF.Value <> "" Then
            oCells(iRow + 7, 9) = "=" & hidF.Value
        End If

        oCells(iRow + 8, 10) = "Ganancia"
        oCells(iRow + 7, 10) = "Perdida"
        '   End If

        oCells.Cells(iRow + 7, 1).Font.Bold = True
        oCells.Cells(iRow + 7, 2).Font.Bold = True
        oCells.Cells(iRow + 8, 6).Font.Bold = True
        oCells.Cells(iRow + 7, 6).Font.Bold = True
        oCells.Cells(iRow + 8, 7).Font.Bold = True
        oCells.Cells(iRow + 7, 7).Font.Bold = True

        oCells(iRow + 7, 1).Borders.LineStyle = 1
        oCells(iRow + 7, 2).Borders.LineStyle = 1
        oCells(iRow + 7, 3).Borders.LineStyle = 1
        oCells(iRow + 7, 4).Borders.LineStyle = 1
        oCells(iRow + 7, 5).Borders.LineStyle = 1
        oCells(iRow + 7, 6).Borders.LineStyle = 1
        oCells(iRow + 7, 7).Borders.LineStyle = 1
        oCells(iRow + 7, 8).Borders.LineStyle = 1
        oCells(iRow + 7, 9).Borders.LineStyle = 1
        oCells(iRow + 7, 10).Borders.LineStyle = 1
        oCells(iRow + 7, 11).Borders.LineStyle = 1

        oCells(iRow + 8, 1).Borders.LineStyle = 1
        oCells(iRow + 8, 2).Borders.LineStyle = 1
        oCells(iRow + 8, 3).Borders.LineStyle = 1
        oCells(iRow + 8, 4).Borders.LineStyle = 1
        oCells(iRow + 8, 5).Borders.LineStyle = 1
        oCells(iRow + 8, 6).Borders.LineStyle = 1
        oCells(iRow + 8, 7).Borders.LineStyle = 1
        oCells(iRow + 8, 8).Borders.LineStyle = 1
        oCells(iRow + 8, 9).Borders.LineStyle = 1
        oCells(iRow + 8, 10).Borders.LineStyle = 1
        oCells(iRow + 8, 11).Borders.LineStyle = 1

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

End Class

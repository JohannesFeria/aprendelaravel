Imports System.Data
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.IO
Imports System.Runtime.InteropServices.Marshal

Partial Class Modulos_Gestion_Reportes_frmReporteCustodia
    Inherits BasePage

#Region "Eventos de la página"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarCombos()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Protected Sub btnGenerarExcel_Click(sender As Object, e As System.EventArgs) Handles btnGenerarExcel.Click
        Try
            If ddlReporte.SelectedValue.Equals("") Then
                AlertaJS("Seleccione un reporte")
                Exit Sub
            ElseIf txtFechaOperacion.Text.Equals("") Then
                AlertaJS("Seleccione una fecha")
                Exit Sub
            End If
            generarReporteCustodia()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

#End Region

#Region "Métodos y Funciones personalizados"

    Private Sub cargarCombos()
        cargarReporte()
    End Sub

    Private Sub cargarReporte()
        ddlReporte.Items.Add(New ListItem("-- Seleccionar --", ""))
        ddlReporte.Items.Add(New ListItem("DPZ", "1"))
        ddlReporte.Items.Add(New ListItem("Forward", "2"))
        ddlReporte.Items.Add(New ListItem("Tenencias", "3"))
        ddlReporte.Items.Add(New ListItem("Ope. Reporte", "4"))
    End Sub

    Private Sub generarReporteCustodia()
        If ddlReporte.SelectedValue = "1" Then
            generarReporteDPZ(ddlReporte.SelectedValue)
        ElseIf ddlReporte.SelectedValue = "2" Then
            generarReporteForward(ddlReporte.SelectedValue)
        ElseIf ddlReporte.SelectedValue = "3" Then
            generarReporteTenencias(ddlReporte.SelectedValue)
        Else
            generarOperacionesReporte(ddlReporte.SelectedValue)
        End If
    End Sub

    Private Sub generarReporteDPZ(tipoReporte As String)
        Dim dt As DataTable
        Dim rGestionBM As New ReporteGestionBM
        dt = rGestionBM.listarReporteCustodiaDPZ("", UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text))
        generarExcel(tipoReporte, dt)
    End Sub

    Private Sub generarReporteForward(tipoReporte As String)
        Dim dt As DataTable
        Dim rGestionBM As New ReporteGestionBM
        dt = rGestionBM.listarReporteCustodiaForward("", UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text.Trim))
        generarExcel(tipoReporte, dt)
    End Sub

    Private Sub generarReporteTenencias(tipoReporte As String)
        Dim dt As DataTable
        Dim rGestionBM As New ReporteGestionBM
        dt = rGestionBM.listarReporteCustodiaTenencia("", UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text.Trim))
        generarExcel(tipoReporte, dt)
    End Sub

    Private Sub generarOperacionesReporte(tipoReporte As String)
        Dim dt As DataTable
        Dim rGestionBM As New ReporteGestionBM
        dt = rGestionBM.listarReporteCustodiaOpReporte("", UIUtility.ConvertirFechaaDecimal(Me.txtFechaOperacion.Text))
        generarExcel(tipoReporte, dt)
    End Sub

    Private Sub generarExcel(tipoReporte As String, dt As DataTable)
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim aplicacionExcel As Excel.Application
        Dim librosExceles As Excel.Workbooks
        Dim libroExcel As Excel.Workbook
        Dim hojas As Excel.Sheets
        Dim hojaTrabajo As Excel.Worksheet
        Dim celda As Excel.Range
        Dim nombreArchivo As String
        Dim oFile As String
        Dim plantillaReporteCustodia As String
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            aplicacionExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            nombreArchivo = "ReporteCustodia_" & Usuario.ToString.Trim & "_" & System.DateTime.Now.ToString("dd-MM-yy") & "_" & System.DateTime.Now.ToString("hhmmss") & ".xlsx"
            oFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & nombreArchivo
            If tipoReporte = "1" Then
                plantillaReporteCustodia = RutaPlantillas() & "\plantillaReporteCustodiaDPZ.xlsx"
            ElseIf tipoReporte = "2" Then
                plantillaReporteCustodia = RutaPlantillas() & "\plantillaReporteCustodiaForward.xlsx"
            ElseIf tipoReporte = "3" Then
                plantillaReporteCustodia = RutaPlantillas() & "\plantillaReporteCustodiaTenencias.xlsx"
            Else
                plantillaReporteCustodia = RutaPlantillas() & "\plantillaReporteOpReporte.xlsx"
            End If
            If File.Exists(oFile) Then
                File.Delete(oFile)
            End If
            aplicacionExcel.Visible = False : aplicacionExcel.DisplayAlerts = False
            librosExceles = aplicacionExcel.Workbooks
            librosExceles.Open(plantillaReporteCustodia)
            libroExcel = librosExceles.Item(1)
            hojas = libroExcel.Worksheets
            hojaTrabajo = CType(hojas.Item(1), Excel.Worksheet)
            celda = hojaTrabajo.Cells
            If (dt.Rows.Count > 0) Then
                llenarReporteCustodia(dt, celda)
            End If
            hojaTrabajo.SaveAs(oFile)
            libroExcel.Save()
            libroExcel.Close()
            Response.Clear()
            Response.ContentType = "application/xlsx"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nombreArchivo)
            Response.WriteFile(oFile)
            Response.End()
        Catch ex As Exception
            Throw ex
        Finally
            aplicacionExcel.Quit()
            ReleaseComObject(aplicacionExcel)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub

    Public Sub llenarReporteCustodia(dt As DataTable, celda As Excel.Range)
        Dim nCelda As Integer = 3
        For i = 0 To dt.Rows.Count - 1
            For j = 0 To dt.Columns.Count - 1
                celda(nCelda + i, j + 1) = dt.Rows(i)(j).ToString
            Next
        Next
    End Sub

#End Region

End Class

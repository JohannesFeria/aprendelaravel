Imports System.Data
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Partial Class Modulos_Gestion_Reportes_frmReporteValorCuota
    Inherits BasePage
#Region "Eventos de la Página"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarPortafolio()
                txtFechaInicial.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
                txtFechaFinal.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub chkSeriado_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSeriado.CheckedChanged
        If chkSeriado.Checked Then
            cargarPortafolioSeriados()
        Else
            cargarPortafolio()
        End If
    End Sub
    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim dt1 As DataTable
            Dim dt2 As DataTable
            Dim dt3 As DataTable
            Dim dt4 As DataTable
            Dim objReporte As New ReporteGestionBM
            dt1 = objReporte.listarReporteValorCuota(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.txtFechaInicial.Text.Trim), UIUtility.ConvertirFechaaDecimal(Me.txtFechaFinal.Text.Trim))
            dt2 = objReporte.listarReporteValorCuotaTotalPorFondo(UIUtility.ConvertirFechaaDecimal(Me.txtFechaInicial.Text.Trim), UIUtility.ConvertirFechaaDecimal(Me.txtFechaFinal.Text.Trim))
            dt3 = objReporte.listarReporteValorCuotaTotalPorFondoSeriado(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.txtFechaInicial.Text.Trim), UIUtility.ConvertirFechaaDecimal(Me.txtFechaFinal.Text.Trim)).Tables(0)
            dt4 = objReporte.listarReporteValorCuotaTotalPorFondoSeriado(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.txtFechaInicial.Text.Trim), UIUtility.ConvertirFechaaDecimal(Me.txtFechaFinal.Text.Trim)).Tables(1)
            generarExcel(dt1, dt2, dt3, dt4)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub
#End Region
#Region "Métodos y funciones de la página"
    Private Sub cargarPortafolio()
        ddlPortafolio.DataSource = Nothing
        ddlPortafolio.DataBind()
        Dim portafolioBM As New PortafolioBM
        Dim dt As DataTable = portafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True, "Todos")
    End Sub
    Private Sub cargarPortafolioSeriados()
        ddlPortafolio.DataSource = Nothing
        ddlPortafolio.DataBind()
        Dim portafolioBM As New PortafolioBM
        Dim dt As DataTable = portafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, "S", "S", "A")
        HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True, "Todos")
    End Sub
    Public Sub generarExcel(ByVal dt1 As DataTable, ByVal dt2 As DataTable, ByVal dt3 As DataTable, ByVal dt4 As DataTable)
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
        Dim plantillaValorCuota As String
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            aplicacionExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            nombreArchivo = "ReporteValorCuota_" & Usuario.ToString.Trim & "_" & System.DateTime.Now.ToString("dd-MM-yy") & "_" & System.DateTime.Now.ToString("hhmmss") & ".xls"
            oFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & nombreArchivo
            plantillaValorCuota = RutaPlantillas() & "\plantillaValorCuota.xlsx"
            If File.Exists(oFile) Then
                File.Delete(oFile)
            End If
            aplicacionExcel.Visible = False : aplicacionExcel.DisplayAlerts = False
            librosExceles = aplicacionExcel.Workbooks
            librosExceles.Open(plantillaValorCuota)
            libroExcel = librosExceles.Item(1)
            hojas = libroExcel.Worksheets
            hojaTrabajo = CType(hojas.Item(1), Excel.Worksheet)
            celda = hojaTrabajo.Cells
            If dt1.Rows.Count > 0 Then
                llenarReporteValorCuota(dt1, celda)
            End If
            hojaTrabajo.SaveAs(oFile)
            hojaTrabajo = CType(hojas.Item(2), Excel.Worksheet)
            celda = hojaTrabajo.Cells
            If dt2.Rows.Count > 0 Then
                llenarReporteValorCuotaTotalPorFond(dt2, celda)
            End If
            hojaTrabajo.SaveAs(oFile)
            hojaTrabajo = CType(hojas.Item(3), Excel.Worksheet)
            celda = hojaTrabajo.Cells
            If dt3.Rows.Count > 0 And dt4.Rows.Count > 0 Then
                llenarReporteValorCuotaTotalPorFondoSeriado(dt3, dt4, celda)
            End If
            hojaTrabajo.SaveAs(oFile)
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
    Private Sub llenarReporteValorCuota(ByVal dt As DataTable, ByVal celda As Excel.Range)
        Dim nCelda As Integer = 3
        For i = 0 To dt.Rows.Count - 1
            For j = 0 To dt.Columns.Count - 1
                celda(nCelda + i, j + 1) = dt.Rows(i)(j).ToString
            Next
        Next
    End Sub
    Private Sub llenarReporteValorCuotaTotalPorFond(ByVal dt As DataTable, ByVal celda As Excel.Range)
        Dim nCelda As Integer = 3
        For i = 0 To dt.Rows.Count - 1
            For j = 0 To dt.Columns.Count - 1
                celda(nCelda + i, j + 1) = dt.Rows(i)(j).ToString
            Next
        Next
    End Sub
    Private Sub llenarReporteValorCuotaTotalPorFondoSeriado(ByVal dt1 As DataTable, ByVal dt2 As DataTable, ByVal celda As Excel.Range)
        Dim nCelda As Integer = 3
        Dim nCeldaTotales As Integer = 0
        Dim condicion As String = ""
        For i = 0 To dt1.Rows.Count - 1
            For j = 0 To dt1.Columns.Count - 1
                celda(nCelda + i, j + 1) = dt1.Rows(i)(j).ToString
            Next
            nCeldaTotales = nCelda + i
            If i <> dt1.Rows.Count - 1 Then
                If condicion <> dt1.Rows(i + 1)("portafolio").ToString & dt1.Rows(i + 1)("fechaProceso").ToString Or condicion = "" Then
                    If condicion = "" Then
                        condicion = dt1.Rows(i)("portafolio").ToString & dt1.Rows(i)("fechaProceso").ToString
                    Else
                        For k = 0 To dt2.Rows.Count - 1
                            If condicion = dt2.Rows(k)("portafolio").ToString & dt2.Rows(k)("fechaProceso").ToString Then
                                celda(nCeldaTotales + 1, 1) = "Total " & dt2.Rows(k)("portafolio").ToString & " " & dt2.Rows(k)("fechaProceso").ToString & ":"
                                formatoCeldaNegrita(celda(nCeldaTotales + 1, 1))
                                For l = 2 To dt2.Columns.Count - 1
                                    celda(nCeldaTotales + 1, 6 + l) = dt2.Rows(k)(l).ToString
                                Next
                                nCelda = nCelda + 1
                            End If
                        Next
                        condicion = dt1.Rows(i + 1)("portafolio").ToString & dt1.Rows(i + 1)("fechaProceso").ToString
                    End If
                End If
            Else
                For k = 0 To dt2.Rows.Count - 1
                    If condicion = dt2.Rows(k)("portafolio").ToString & dt2.Rows(k)("fechaProceso").ToString Then
                        celda(nCeldaTotales + 1, 1) = "Total " & dt2.Rows(k)("portafolio").ToString & " " & dt2.Rows(k)("fechaProceso").ToString & ":"
                        formatoCeldaNegrita(celda(nCeldaTotales + 1, 1))
                        For l = 2 To dt2.Columns.Count - 1
                            celda(nCeldaTotales + 1, 6 + l) = dt2.Rows(k)(l).ToString
                        Next
                        nCelda = nCelda + 1
                    End If
                Next
            End If
        Next
    End Sub
    Private Sub formatoCeldaNegrita(celda As Excel.Range)
        celda.Cells.Font.Bold = True
    End Sub
#End Region
End Class
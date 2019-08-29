Imports System.Data
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports System.Threading
Partial Class Modulos_Gestion_Reportes_frmReporteLimite
    Inherits BasePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarPortafolio()
                txtFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
                txtFechaFinal.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub cargarPortafolio()
        ddlPortafolio.DataSource = Nothing
        ddlPortafolio.DataBind()
        Dim portafolioBM As New PortafolioBM
        Dim dt As DataTable = portafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True, "Todos")
    End Sub
    Private Sub LlenarExcel(ByVal dt As DataTable, ByVal celda As Excel.Range)
        Dim nCelda As Integer = 3
        For i = 0 To dt.Rows.Count - 1
            For j = 0 To dt.Columns.Count - 1
                celda(nCelda + i, j + 1) = dt.Rows(i)(j).ToString
            Next
        Next
    End Sub
    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            Dim dtCarteraFondo As DataTable
            Dim dtValorCuota As DataTable
            Dim dtCarteraFondoForward As DataTable
            Dim dtTipoCambio As DataTable
            Dim dtOption As DataTable
            Dim dtSwap As DataTable
            Dim objReporte As New ReporteGestionBM
            dtCarteraFondo = objReporte.ListarCarteraFondo(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.txtFechaFinal.Text.Trim), "REAL")
            dtValorCuota = objReporte.ListarReporteValorCuotaLimite(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.txtFecha.Text.Trim), UIUtility.ConvertirFechaaDecimal(Me.txtFechaFinal.Text.Trim))
            dtCarteraFondoForward = objReporte.ListarCarteraFondoForward(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.txtFechaFinal.Text.Trim))
            dtTipoCambio = objReporte.ListarTipoCambio(UIUtility.ConvertirFechaaDecimal(Me.txtFechaFinal.Text.Trim))
            dtOption = objReporte.ListarOption(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.txtFechaFinal.Text.Trim))
            dtSwap = objReporte.ListarSwap(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.txtFechaFinal.Text.Trim))
            generarExcel(dtCarteraFondo, dtValorCuota, dtCarteraFondoForward, dtTipoCambio, dtOption, dtSwap)
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString, "'", String.Empty))
        End Try
    End Sub
    Public Sub generarExcel(ByVal dtCarteraFondo As DataTable, ByVal dtValorCuota As DataTable, ByVal dtCarteraFondoForward As DataTable, _
                            ByVal dtTipoCambio As DataTable, ByVal dtOption As DataTable, ByVal dtSwap As DataTable)
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim aplicacionExcel As Excel.Application
        Dim librosExceles As Excel.Workbooks = Nothing
        Dim libroExcel As Excel.Workbook = Nothing
        Dim hojas As Excel.Sheets = Nothing
        Dim hojaTrabajo As Excel.Worksheet = Nothing
        Dim celda As Excel.Range = Nothing
        Dim nombreArchivo As String
        Dim oFile As String = String.Empty
        Dim PlantillaLimite As String
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            aplicacionExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            nombreArchivo = "ReporteLimite_" & Usuario.ToString.Trim & "_" & System.DateTime.Now.ToString("dd-MM-yy") & "_" & System.DateTime.Now.ToString("hhmmss") & ".xlsx"
            oFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & nombreArchivo
            PlantillaLimite = RutaPlantillas() & "\PlantillaLimite.xlsx"
            If File.Exists(oFile) Then
                File.Delete(oFile)
            End If
            aplicacionExcel.Visible = False : aplicacionExcel.DisplayAlerts = False
            librosExceles = aplicacionExcel.Workbooks
            librosExceles.Open(PlantillaLimite)
            libroExcel = librosExceles.Item(1)
            hojas = libroExcel.Worksheets
            hojaTrabajo = CType(hojas.Item(1), Excel.Worksheet)
            celda = hojaTrabajo.Cells
            If dtCarteraFondo.Rows.Count > 0 Then
                LlenarExcel(dtCarteraFondo, celda)
            End If

            hojaTrabajo.SaveAs(oFile)
            hojaTrabajo = CType(hojas.Item(2), Excel.Worksheet)
            celda = hojaTrabajo.Cells
            If dtValorCuota.Rows.Count > 0 Then
                LlenarExcel(dtValorCuota, celda)
            End If

            hojaTrabajo.SaveAs(oFile)
            hojaTrabajo = CType(hojas.Item(3), Excel.Worksheet)
            celda = hojaTrabajo.Cells
            If dtCarteraFondoForward.Rows.Count > 0 Then
                LlenarExcel(dtCarteraFondoForward, celda)
            End If

            hojaTrabajo.SaveAs(oFile)
            hojaTrabajo = CType(hojas.Item(4), Excel.Worksheet)
            celda = hojaTrabajo.Cells
            If dtTipoCambio.Rows.Count > 0 Then
                LlenarExcel(dtTipoCambio, celda)
            End If

            hojaTrabajo.SaveAs(oFile)
            hojaTrabajo = CType(hojas.Item(5), Excel.Worksheet)
            celda = hojaTrabajo.Cells
            If dtOption.Rows.Count > 0 Then
                LlenarExcel(dtOption, celda)
            End If

            hojaTrabajo.SaveAs(oFile)
            hojaTrabajo = CType(hojas.Item(6), Excel.Worksheet)
            celda = hojaTrabajo.Cells
            If dtSwap.Rows.Count > 0 Then
                LlenarExcel(dtSwap, celda)
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
End Class
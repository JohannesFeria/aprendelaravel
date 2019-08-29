Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports ParametrosSIT
Imports System.IO
Partial Class Modulos_Tesoreria_Reportes_frmReporteDiarioOperaciones
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
    Dim oOrdenPrevOrden As New OrdenPreOrdenInversionBM
    Dim contador As Integer
    Private Sub CargarPortafolio()
        HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "Todos")
    End Sub
    Sub DatosExcel(ByVal dt As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        oCells(2, 2) = tbFechaInicio.Text
        For Each dr In dt.Rows
            Dim i As Integer = 0
            Do While i <= dt.Columns.Count - 1
                oCells(FilaInicial, i + 1) = dr(i)
                i = i + 1
            Loop
            FilaInicial += 1
        Next
    End Sub
    Private Sub GenerarReporte()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtRegInv As DataTable = oOrdenPrevOrden.Reporte_ReportesDiariosOperaciones(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), ddlreporte.SelectedValue)
            If dtRegInv.Rows.Count > 0 Then
                Dim sFile As String, sTemplate As String, Nombre As String
                If ddlreporte.SelectedValue = "1" Then
                    Nombre = "RDO_DPZ_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                    sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Nombre
                ElseIf ddlreporte.SelectedValue = "2" Then
                    Nombre = "RDO_FD_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                    sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Nombre
                ElseIf ddlreporte.SelectedValue = "3" Then
                    Nombre = "RDO_OR_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                    sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Nombre
                ElseIf ddlreporte.SelectedValue = "4" Then
                    Nombre = "RDO_Tenencia_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                    sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Nombre
                Else
                    Nombre = "RDO_Dividendo_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                    sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Nombre

                End If

                Dim Indice As Integer = 1
                If File.Exists(sFile) Then File.Delete(sFile)
                If ddlreporte.SelectedValue = "1" Then
                    sTemplate = RutaPlantillas() & "\" & "ReportesDiariosOperaciones-DPZ.xls"
                ElseIf ddlreporte.SelectedValue = "2" Then
                    sTemplate = RutaPlantillas() & "\" & "ReportesDiariosOperaciones-FD.xls"
                ElseIf ddlreporte.SelectedValue = "3" Then
                    sTemplate = RutaPlantillas() & "\" & "ReportesDiariosOperaciones-OR.xls"
                ElseIf ddlreporte.SelectedValue = "4" Then
                    sTemplate = RutaPlantillas() & "\" & "ReportesDiariosOperaciones-Tenencias.xls"
                Else
                    sTemplate = RutaPlantillas() & "\" & "ReportesDiariosOperaciones-Dividendos.xls"
                End If
                oExcel.Visible = False : oExcel.DisplayAlerts = False
                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                oCells = oSheet.Cells
                oSheet.SaveAs(sFile)
                DatosExcel(dtRegInv, 4, oSheet, oCells)
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
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If (Page.IsPostBack = False) Then
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            CargarPortafolio()
        End If
    End Sub
    Protected Sub btnGenera_Click(sender As Object, e As System.EventArgs) Handles btnGenera.Click
        GenerarReporte()
    End Sub
End Class
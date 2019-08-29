Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports System.IO
Partial Class Modulos_Gestion_Reportes_frmReporteRegistroInversiones
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
    Dim oOrdenPrevOrden As New OrdenPreOrdenInversionBM
    Sub DatosExcel(ByVal dt As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        oCells(2, 2) = tbFechaIni.Text
        oCells(2, 4) = tbFechaFin.Text
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
            Dim dtRegInv As DataTable = oOrdenPrevOrden.ReporteRegistroInversiones(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaIni.Text), _
            UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text))
            If dtRegInv.Rows.Count > 0 Then
                Dim sFile As String, sTemplate As String, Nombre As String
                Nombre = "RRO_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Nombre
                Dim Indice As Integer = 1
                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutaPlantillas() & "\" & "ReporteRegistroInversiones.xls"
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
    Protected Sub btnImprimir_Click(sender As Object, e As System.EventArgs) Handles btnImprimir.Click
        GenerarReporte()
    End Sub
    Private Sub CargarPortafolio()
        HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "Todos")
    End Sub
    Protected Sub Modulos_Gestion_Reportes_frmReporteRegistroInversiones_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            tbFechaIni.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            tbFechaFin.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            CargarPortafolio()
        End If
    End Sub
End Class
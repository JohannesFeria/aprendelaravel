Imports System.Runtime.InteropServices.Marshal
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports Microsoft.Office
Imports System.Data
Imports System.Globalization
Imports System.Threading
Imports System.IO

Partial Class Modulos_Gestion_Reportes_frmReporteComisionesAgente
    Inherits BasePage

#Region "Variables"
    Dim oUtil As New UtilDM
#End Region
#Region "/* Metodos Personalizados */"
    Private Sub VisualizarIntermediario(ByVal situacion As Boolean)
        ddlIntermediarios.Visible = situacion
    End Sub

    Public Sub CargarCombos()
        Dim DtablaEntidad As New DataTable
        Dim oEntidadBM As New EntidadBM
        Try
            DtablaEntidad = oEntidadBM.Entidad_Listar(DatosRequest, "", ParametrosSIT.ESTADO_ACTIVO, "S").Tables(0)
            HelpCombo.LlenarComboBox(ddlIntermediarios, DtablaEntidad, "CODIGOTERCERO", "NOMBRETERCERO", True)
            ddlIntermediarios.SelectedIndex = 0
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ExportarExcel(ByVal Archivo As String, ByVal dt As DataTable)
        'OT10689 - Inicio. Kill process excel
        Dim oldCulture As CultureInfo
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim sFile As String = "", sTemplate As String = ""
        Dim iRow As Integer = 0, iCol As Integer = 0
        Dim oExcel As Excel.Application
        Dim dr As DataRow, ary() As Object
        Dim oCells As Excel.Range
        oldCulture = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Archivo & "_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            sTemplate = RutaPlantillas() & "\Plantilla" & Archivo & ".xls"
            oExcel.DisplayAlerts = False
            oExcel.Visible = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Name = Archivo
            oCells = oSheet.Cells
            oCells(2, 1) = "LIQUIDACION DE COMISION - AGENTES DE BOLSA (CxP) de la Administradora"
            oSheet.Range("A2:C2").Font.Bold = True
            oSheet.Range("A2:C2").Select()
            oSheet.Range("A2:C2").Merge()
            For iCol = 0 To dt.Columns.Count - 1
                oCells(4, iCol + 1) = dt.Columns(iCol).ToString
            Next
            oSheet.Range("A4:N4").Font.Bold = True
            For iRow = 0 To dt.Rows.Count - 1
                dr = dt.Rows.Item(iRow)
                ary = dr.ItemArray
                For iCol = 0 To UBound(ary)
                    oCells(iRow + 5, iCol + 1) = ary(iCol).ToString
                Next iCol
            Next iRow
            oSheet.SaveAs(sFile)
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(sFile))
            Response.WriteFile(sFile)
            Response.End()
            Thread.CurrentThread.CurrentCulture = oldCulture
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
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
#End Region
#Region "/* Eventos */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            Try
                CargarCombos()
                tbFechaInicio.Text = oUtil.RetornarFechaSistema
                tbFechaFin.Text = oUtil.RetornarFechaSistema
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        End If
    End Sub

    Private Sub btnVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVista.Click
        Dim strCodigoIntermediario As String = ""
        Dim decFechaIni As Decimal = 0
        Dim decFechaFin As Decimal = 0
        Dim ds As DataSet
        Try
            If ddlIntermediarios.SelectedIndex > 0 Then
                strCodigoIntermediario = ddlIntermediarios.SelectedValue
            End If
            If Trim(tbFechaInicio.Text) = "" Then
                AlertaJS("Debe ingresar la fecha de inicio...!")
                Exit Sub
            End If
            If Trim(tbFechaFin.Text) = "" Then
                AlertaJS("Debe ingresar la fecha de fin...!")
                Exit Sub
            End If
            decFechaIni = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
            decFechaFin = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
            If decFechaIni <= decFechaFin Then
                ds = New ReporteGestionBM().LiquidacionComisionesAgentes(strCodigoIntermediario, decFechaIni, decFechaFin)
                If ds.Tables.Count > 0 Then
                    ExportarExcel("Comisiones", CType(ds.Tables(0), DataTable))
                Else
                    AlertaJS("No existen registros para mostrar el reporte")
                End If
            Else
                AlertaJS("La fecha de inicio no puede ser mayor que la fecha de fin...!")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
#End Region

End Class

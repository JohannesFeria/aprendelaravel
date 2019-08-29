'OT 9908 31/01/2017 - Carlos Espejo
'Descripcion: Formulario para la generacion de reporte de Intereses Cobrados
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports ParametrosSIT
Imports System.IO
Partial Class Modulos_Inversiones_Reportes_frmReporteInteresCobrado
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
    Dim oOIOperacion As New OrdenInversionDatosOperacionBM
    Sub CargaGrilla()
        dgReporte.DataSource = oOIOperacion.InteresesCobrados(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text), _
            UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text))
        dgReporte.DataBind()
    End Sub
    Private Sub CargarPortafolio()
        HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "--Todos--")
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarPortafolio()
        End If
    End Sub
    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargaGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub dgReporte_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgReporte.PageIndexChanging
        Try
            dgReporte.PageIndex = e.NewPageIndex
            CargaGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub
    Sub DatosExcel(ByVal dt As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        oCells(2, 2) = txtFechaInicio.Text
        oCells(2, 4) = txtFechaFin.Text
        For Each dr In dt.Rows
            Dim i As Integer = 0
            Do While i <= dt.Columns.Count - 1
                oCells(FilaInicial, i + 1) = dr(i)
                i = i + 1
            Loop
            FilaInicial += 1
        Next
    End Sub
    Protected Sub btnGenera_Click(sender As Object, e As System.EventArgs) Handles btnGenera.Click
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtInteres As DataTable = oOIOperacion.InteresesCobrados(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text), _
            UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text))
            If dtInteres.Rows.Count > 0 Then
                Dim sFile As String = String.Empty, sTemplate As String = String.Empty, Nombre As String = String.Empty
                Nombre = "InteresCobrado" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Nombre
                Dim Indice As Integer = 1
                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutaPlantillas() & "\" & "Plantilla_IntGanCob.xls"
                oExcel.Visible = False : oExcel.DisplayAlerts = False
                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                oCells = oSheet.Cells
                oSheet.SaveAs(sFile)
                DatosExcel(dtInteres, 4, oSheet, oCells)
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
End Class
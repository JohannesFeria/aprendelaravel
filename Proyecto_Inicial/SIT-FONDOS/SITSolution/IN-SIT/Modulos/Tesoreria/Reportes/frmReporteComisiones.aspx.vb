﻿'OT 10328 - 28/04/2017 - Carlos Espejo
'Descripcion: Creacion del reporte
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports ParametrosSIT
Imports System.IO
Partial Class Modulos_Tesoreria_Reportes_frmReporteComisiones
    Inherits BasePage
    Dim oCuentaEconomica As New CuentaEconomicaBM
    Dim oPortafolioBM As New PortafolioBM
    Sub DatosExcel(ByVal dt As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        oCells(2, 2) = tbFechaInicio.Text
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
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtCE As DataTable = oCuentaEconomica.ReporteComisionesRecaudo(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), _
            UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text))
            If dtCE.Rows.Count > 0 Then
                Dim sFile As String, sTemplate As String
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "COM_" & Usuario.ToString() & _
                String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                Dim Indice As Integer = 3
                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutaPlantillas() & "\" & "PlantillaComisionesRecaudo.xls"
                oExcel.Visible = False : oExcel.DisplayAlerts = False
                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                oCells = oSheet.Cells
                oSheet.SaveAs(sFile)
                DatosExcel(dtCE, 4, oSheet, oCells)
                oExcel.Cells.EntireColumn.AutoFit()
                oBook.Save()
                oBook.Close()
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "COM_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & _
                String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
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
    Protected Sub btnGenera_Click(sender As Object, e As System.EventArgs) Handles btnGenera.Click
        GenerarReporte()
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True)
        End If
    End Sub
    Sub CargarVista()
        Try
            Dim dt As DataTable = oCuentaEconomica.ReporteComisionesRecaudo(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), _
            UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text))
            dgLista.DataSource = dt
            dgLista.DataBind()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnVista_Click(sender As Object, e As System.EventArgs) Handles btnVista.Click
        CargarVista()
    End Sub
    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarVista()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
End Class
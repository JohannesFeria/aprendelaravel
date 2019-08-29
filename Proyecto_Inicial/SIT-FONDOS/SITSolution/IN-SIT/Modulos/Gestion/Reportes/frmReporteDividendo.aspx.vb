Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports ParametrosSIT
Imports System.IO

Partial Class Modulos_Gestion_Reportes_frmReporteDividendo
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
    Dim oOrdenPrevOrden As New OrdenPreOrdenInversionBM
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
            Dim dsvVecVar As DataSet = New ReporteGestionBM().Reporte_Dividendo(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaIni.Text), UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text))
            If dsvVecVar.Tables.Count > 0 Then
                Dim sFile As String, sTemplate As String, Nombre As String
                Nombre = "RRO_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Nombre

                Dim Indice As Integer = 1
                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutaPlantillas() & "\" & "PlantillaReporteDividendos.xls"
                oExcel.Visible = False : oExcel.DisplayAlerts = False

                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets

                'Tabla 1 RESUMEN DE PRECIOS
                oSheet = oBook.Sheets(1)
                oCells = oSheet.Cells
                oCells(2, 2) = "DEL " + tbFechaIni.Text + " AL " + tbFechaFin.Text
                oCells(2, 9) = Usuario
                oCells(2, 11) = Now.ToLongDateString
                Dim dtResumen As DataTable = dsvVecVar.Tables(0)
                Dim FilaInicial As Integer = 4
                For Each dr In dtResumen.Rows
                    oCells(FilaInicial, 1) = dr("CODIGOORDEN")
                    oCells(FilaInicial, 2) = dr("TipoOperacion")
                    oCells(FilaInicial, 3) = dr("CodigoNemonico")
                    oCells(FilaInicial, 4) = dr("TipoInstrumento")
                    oCells(FilaInicial, 5) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaOperacion")))
                    oCells(FilaInicial, 6) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaLiquidacion")))
                    oCells(FilaInicial, 7) = dr("CantidadOrdenado")
                    oCells(FilaInicial, 8) = dr("Precio")
                    oCells(FilaInicial, 9) = dr("MontoOperacion")
                    oCells(FilaInicial, 10) = dr("TOTALCOMISIONES")
                    oCells(FilaInicial, 11) = dr("MontoNetoOperaciones")
                    oCells(FilaInicial, 12) = dr("Moneda")
                    oCells(FilaInicial, 13) = dr("ValorPrimario")
                    oCells(FilaInicial, 14) = dr("MontoSoles")
                    FilaInicial += 1
                Next
                oExcel.Cells.EntireColumn.AutoFit()
                oSheet.SaveAs(sFile)
                oBook.Save()
                oBook.Close()
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "RV_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
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
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        GenerarReporte()
    End Sub
    Private Sub CargarPortafolio()
        'HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "Todos")
        HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True)
    End Sub
    Protected Sub Modulos_Gestion_Reportes_frmReporteRegistroInversiones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            tbFechaIni.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            tbFechaFin.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            CargarPortafolio()
        End If
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        CargarGrillaReporte()
    End Sub
    Protected Sub dgReporte_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgReporte.PageIndexChanging
        dgReporte.PageIndex = e.NewPageIndex
        CargarGrillaReporte()
    End Sub

    Private Sub CargarGrillaReporte()
        Try
            Dim dsvVecVar As DataSet = New ReporteGestionBM().Reporte_Dividendo(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaIni.Text), UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text))
            Dim dtResumen As DataTable = dsvVecVar.Tables(0)
            dgReporte.DataSource = dtResumen
            dgReporte.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurri&oacute; un error al buscar")
        End Try
    End Sub
End Class

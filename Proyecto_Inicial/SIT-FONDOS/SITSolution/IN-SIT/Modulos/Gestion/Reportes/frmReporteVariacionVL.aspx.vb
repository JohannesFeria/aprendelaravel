Imports System.Data
Imports System.Runtime.InteropServices.Marshal
Imports Sit.BusinessLayer
Imports System.IO

Partial Class Modulos_Gestion_Reportes_frmReporteVariacionVL
    Inherits BasePage

    Sub GenerarReporte()
        Dim can As Integer = 0
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim sTemplate As String, sNomFile As String
        Dim sFecha As String, sHora As String, sRutaTemp As String
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Dim sFile As String = ""
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            sFecha = String.Format("{0:yyyyMMdd}", DateTime.Today)
            sHora = String.Format("{0:HHMMss}", DateTime.Now)
            sRutaTemp = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
            sNomFile = "ON_" & Usuario.ToString() & sFecha & sHora & ".xls"
            sFile = sRutaTemp & sNomFile
            sTemplate = RutaPlantillas() & "\" & "PlantillaDiferenciaVL.xls"
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells

            Dim dteFechaHoy As Date = tbFechaIni.Text
            Dim dteFechaAyer As Date = dteFechaHoy.AddDays(-1)

            Dim DT As DataTable = New CarteraTituloValoracionBM().ReporteVLDiferencia(UIUtility.ConvertirFechaaDecimal(dteFechaHoy), UIUtility.ConvertirFechaaDecimal(dteFechaAyer))
            If Not (DT Is Nothing) Then
                Dim ncol As Integer = 7
                oCells(3, 4) = "Fecha : " + tbFechaIni.Text
                oCells(3, 6) = "Número Registros : " + DT.Rows.Count.ToString
                For Each dr As DataRow In DT.Rows
                    oCells(ncol, 1) = IIf(dr("FondoIndador").Equals(DBNull.Value), "", dr("FondoIndador"))
                    oCells(ncol, 2) = IIf(dr("Fondo").Equals(DBNull.Value), "", dr("Fondo"))
                    oCells(ncol, 3) = IIf(dr("Fecha").Equals(DBNull.Value), "", UIUtility.ConvertirDecimalAStringFormatoFecha(dr("Fecha")))
                    oCells(ncol, 4) = IIf(dr("CodigoValor").Equals(DBNull.Value), "", dr("CodigoValor"))
                    oCells(ncol, 5) = IIf(dr("MontoNominal").Equals(DBNull.Value), "", dr("MontoNominal"))
                    oCells(ncol, 6) = IIf(dr("DescripcionTipoInstrumento").Equals(DBNull.Value), "", dr("DescripcionTipoInstrumento"))
                    oCells(ncol, 7) = IIf(dr("Moneda").Equals(DBNull.Value), "", dr("Moneda"))

                    oCells(ncol, 9) = IIf(dr("Signo").Equals(DBNull.Value), "", dr("Signo"))
                    oCells(ncol, 10) = IIf(dr("Diferencia").Equals(DBNull.Value), "", dr("Diferencia"))

                    ncol += 1
                    can = ncol
                Next
                oSheet.SaveAs(sFile)
                oBook.Save()
                oBook.Close()

                If sFile <> "" Then
                    Response.Clear()
                    Response.ContentType = "application/xls"
                    Response.AddHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(sFile))
                    Response.WriteFile(sFile)
                    Response.End()
                End If
            Else
                AlertaJS("No se encontraron registros para la fecha indicada.")
            End If
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

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Call GenerarReporte()
        Catch ex As Exception
            AlertaJS("Ha ocurrido un error: " & Replace(ex.Message, "'", ""))
        End Try
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
            Dim dteFechaHoy As Date = tbFechaIni.Text
            Dim dteFechaAyer As Date = dteFechaHoy.AddDays(-1)
            Dim dtResumen As DataTable = New CarteraTituloValoracionBM().ReporteVLDiferencia(UIUtility.ConvertirFechaaDecimal(dteFechaHoy), UIUtility.ConvertirFechaaDecimal(dteFechaAyer))
            dgReporte.DataSource = dtResumen
            dgReporte.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurri&oacute; un error al buscar")
        End Try
    End Sub
End Class

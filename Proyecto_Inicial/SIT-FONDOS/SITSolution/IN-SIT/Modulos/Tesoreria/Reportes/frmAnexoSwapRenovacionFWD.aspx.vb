Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Tesoreria_Reportes_frmAnexoSwapRenovacionFWD
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oUtil As New UtilDM
        If (Page.IsPostBack = False) Then
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            tbFechaFin.Text = tbFechaInicio.Text
            Me.CargarPortafolio(True)
        End If
    End Sub

    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", False)
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Private Sub btnVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVista.Click
        Try
            Dim tablaParametros As New Hashtable

            If (ddlPortafolio.SelectedIndex <> 0) Then
                tablaParametros("codPortafolio") = ddlPortafolio.SelectedValue
                tablaParametros("Portafolio") = ddlPortafolio.SelectedItem.Text
            End If

            GeneraReporteAnexos()

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try

    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        EstablecerFecha()
    End Sub

    Private Sub EstablecerFecha()
        tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        tbFechaFin.Text = tbFechaInicio.Text
    End Sub

    Private Sub GeneraReporteAnexos()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Dim sFile As String, sTemplate As String
        Dim ds As DataSet, dts As DataTable, dtr As DataTable
        Dim Archivo As String
        Dim oReporteGestionBM As New ReporteGestionBM
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Archivo = "Anexos_Swaps_Renovacion_FWD_" & Usuario.ToString() & "_" & DateTime.Parse(tbFechaInicio.Text).ToString("dd-MM-yy") & System.DateTime.Now.ToString("_hhmmss") & ".xls"
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Archivo
            sTemplate = RutaPlantillas() & "\PlantillaAnexoI_II_FW_SBS.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False

            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells

            ds = oReporteGestionBM.Anexo_Swaps_RenovacionFWD(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text), ddlPortafolio.SelectedValue)

            dts = ds.Tables(0)
            dtr = ds.Tables(1)
            DumpData(dts, oSheet, oCells, ANEXO_I_SWAPS)
            oSheet = CType(oSheets.Item(2), Excel.Worksheet)
            oCells = oSheet.Cells
            DumpData(dtr, oSheet, oCells, ANEXO_II_RENOVA_FWD)
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.SaveAs(sFile)
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Archivo)
            Response.WriteFile(sFile)
            Response.End()
        Catch ex As Exception
            Throw ex
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

    Private Sub DumpData(ByVal dt As DataTable, ByRef oSheet As Excel.Worksheet, ByRef oCells As Excel.Range, Optional ByVal TipoHoja As String = "")
        Dim dr As DataRow
        Dim iRow As Integer

        iRow = 8

        If TipoHoja = ANEXO_I_SWAPS Then
            For Each dr In dt.Rows
                oSheet.Rows(iRow.ToString & ":" & iRow.ToString).Copy()
                oSheet.Rows((iRow + 1).ToString & ":" & (iRow + 1).ToString).Insert(Excel.XlDirection.xlDown)
                oSheet.Application.CutCopyMode = False

                oCells(iRow, 1) = dr("Fecha")
                oCells(iRow, 2) = dr("TipoFondo")
                oCells(iRow, 3) = dr("CodigoSBSForward")
                oCells(iRow, 4) = dr("CodigoMonedaVenta")
                oCells(iRow, 5) = dr("MontoNocional")
                oCells(iRow, 6) = dr("IndicadorForward")
                oCells(iRow, 7) = dr("HoraEjecucionF")
                oCells(iRow, 8) = dr("CodContraparteS")
                oCells(iRow, 9) = dr("IndcadorMovimientoS")
                oCells(iRow, 10) = dr("CodMonedaCompraS")
                oCells(iRow, 11) = dr("CodMonedaVentaS")
                oCells(iRow, 12) = dr("HoraEjecucionS")
                oCells(iRow, 13) = dr("MontoCompra")
                oCells(iRow, 14) = dr("MontoVenta")
                oCells(iRow, 15) = dr("PlazaNegociacion")

                iRow = iRow + 1
            Next
        End If
        If TipoHoja = ANEXO_II_RENOVA_FWD Then
            For Each dr In dt.Rows
                oSheet.Rows(iRow.ToString & ":" & iRow.ToString).Copy()
                oSheet.Rows((iRow + 1).ToString & ":" & (iRow + 1).ToString).Insert(Excel.XlDirection.xlDown)
                oSheet.Application.CutCopyMode = False

                oCells(iRow, 1) = dr("Fecha")
                oCells(iRow, 2) = dr("TipoFondo")
                oCells(iRow, 3) = dr("CodigoSBSForward")
                oCells(iRow, 4) = dr("CodigoMonedaVenta")
                oCells(iRow, 5) = dr("MontoNocional")
                oCells(iRow, 6) = dr("IndicadorForward")
                oCells(iRow, 7) = dr("CodigoSBSForwardR")
                oCells(iRow, 8) = dr("CodigoMonedaVentaR")
                oCells(iRow, 9) = dr("MontoNocionalR")
                oCells(iRow, 10) = dr("IndicadorForwardR")
                oCells(iRow, 11) = dr("PlazaNegociacion")

                iRow = iRow + 1
            Next
        End If
        oSheet.Rows(iRow.ToString & ":" & iRow.ToString).Delete(Excel.XlDirection.xlUp)
    End Sub

End Class

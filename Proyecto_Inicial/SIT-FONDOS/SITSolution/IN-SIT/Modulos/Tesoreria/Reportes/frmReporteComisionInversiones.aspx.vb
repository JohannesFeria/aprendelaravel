Imports System.Data
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.IO
Imports System.Runtime.InteropServices.Marshal

Partial Class Modulos_Tesoreria_Reportes_frmReporteComisionInversiones
    Inherits BasePage

    Dim objPortafolioBM As PortafolioBM
    Dim objReporteGestionBM As ReporteGestionBM

#Region "Eventos de la página"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnGenera_Click(sender As Object, e As System.EventArgs) Handles btnGenera.Click
        Try
            GenerarReporte()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
#End Region

#Region "Métodos de la página"

    Private Sub CargarPagina()
        CargarPortatafolios()
        CargarFechas()
    End Sub

    Private Sub CargarPortatafolios()
        Dim dt As DataTable
        objPortafolioBM = New PortafolioBM
        dt = objPortafolioBM.Listar(DatosRequest, "A").Tables(0)
        HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True, " Todos ")
    End Sub

    Private Sub CargarFechas()
        txtFechaInicio.Text = Convert.ToDateTime(UIUtility.ObtenerValorRequest(DatosRequest, "Fecha")).ToString("dd/MM/yyyy")
        txtFechaFin.Text = Convert.ToDateTime(UIUtility.ObtenerValorRequest(DatosRequest, "Fecha")).ToString("dd/MM/yyyy")
    End Sub

    Private Sub GenerarReporte()
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
            objReporteGestionBM = New ReporteGestionBM
            Dim dt As DataTable = objReporteGestionBM.ReporteComisionInvesiones(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text))
            If dt.Rows.Count > 0 Then
                Dim sFile As String, sTemplate As String
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "CM_" & Usuario.ToString() & _
                String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"

                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutaPlantillas() & "\" & "PlantillaComisionInversiones.xls"
                oExcel.Visible = False : oExcel.DisplayAlerts = False
                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                oCells = oSheet.Cells
                oCells(2, 14) = Now.ToLongDateString
                oCells(3, 14) = "Usuario: " & Usuario
                DatosExcel(dt, 12, oSheet, oCells)
                oSheet.SaveAs(sFile)
                oBook.Save()
                oBook.Close()
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "RCI_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
                Response.WriteFile(sFile)
                Response.End()
            Else
                Throw New System.Exception("No existen operaciones de inversión para generar el reporte")
            End If
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
    End Sub

    Private Sub DatosExcel(ByVal dt As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        For Each dr In dt.Rows
            oSheet.Rows(FilaInicial.ToString & ":" & FilaInicial.ToString).Copy()
            oSheet.Rows((FilaInicial + 1).ToString & ":" & (FilaInicial + 1).ToString).Insert(Excel.XlDirection.xlDown)
            oCells(FilaInicial, 2) = dr("Portafolio")
            oCells(FilaInicial, 3) = dr("CodigoOrden")
            oCells(FilaInicial, 3) = dr("CodigoOrden")
            oCells(FilaInicial, 4) = dr("Operacion")
            oCells(FilaInicial, 5) = dr("CodigoEntidad")
            oCells(FilaInicial, 6) = dr("CodigoISIN")
            oCells(FilaInicial, 7) = dr("CodigoTipoTitulo")
            oCells(FilaInicial, 8) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaOperacion")))
            oCells(FilaInicial, 9) = dr("CantidadOperacion")
            oCells(FilaInicial, 10) = dr("Precio")
            oCells(FilaInicial, 11) = dr("CodigoMoneda")
            oCells(FilaInicial, 12) = dr("MontoNominalOperacion")
            oCells(FilaInicial, 13) = dr("MontoOperacion")
            oCells(FilaInicial, 14) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaVencimiento")))
            oCells(FilaInicial, 15) = dr("Tercero")
            oCells(FilaInicial, 16) = dr("CodigoPlaza")
            oCells(FilaInicial, 17) = dr("Mecanismo")
            oCells(FilaInicial, 18) = dr("TasaPorcentaje")
            oCells(FilaInicial, 19) = dr("TasaEfectiva")
            oCells(FilaInicial, 20) = dr("TotalComisiones")
            oCells(FilaInicial, 21) = dr("RestoComisiones")
            oCells(FilaInicial, 22) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaLiquidacion")))
            oCells(FilaInicial, 23) = dr("SUBYACENTE")
            oCells(FilaInicial, 24) = dr("ValorEjecucion")
            oCells(FilaInicial, 25) = dr("Prima")
            oCells(FilaInicial, 26) = dr("CodigoSBS")
            oCells(FilaInicial, 27) = dr("TipoCambio")
            FilaInicial += 1
        Next
    End Sub

#End Region

End Class

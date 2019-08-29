Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports ParametrosSIT
Partial Class Modulos_Tesoreria_Reportes_frmGenerarReporteConstForward
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oUtil As New UtilDM
        If (Page.IsPostBack = False) Then
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            tbFechaFin.Text = tbFechaInicio.Text
            Me.CargarPortafolio()
            Me.CargarMoneda(True)
            Me.CargarNroCuenta(True)
        End If
    End Sub
    Private Sub CargarPortafolio()
        ddlPortafolio.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
        ddlPortafolio.Enabled = True
    End Sub
    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataSet = oMoneda.GetMonedaMercadoPortafolio(String.Empty, ddlPortafolio.SelectedValue)
            HelpCombo.LlenarComboBox(ddlMoneda, dsMoneda.Tables(0), "CodigoMoneda", "Descripcion", True)
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        End If
        ddlMoneda.Enabled = enabled
    End Sub
    Private Sub CargarNroCuenta(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oCuentaEconomica As New CuentaEconomicaBM
            Dim dtCuentaEconomica As DataTable = oCuentaEconomica.ListarCuentaEconomica(DatosRequest)
            HelpCombo.LlenarComboBoxBusquedas(ddlNroCuenta, dtCuentaEconomica, "numerocuenta", "numerocuenta", True)
        End If
        ddlMoneda.Enabled = enabled
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
            If (ddlMoneda.SelectedIndex <> 0) Then
                tablaParametros("codMoneda") = ddlMoneda.SelectedValue
                tablaParametros("Moneda") = ddlMoneda.SelectedItem.Text
            End If
            If (ddlMercado.SelectedIndex <> 0) Then
                tablaParametros("codMercado") = ddlMercado.SelectedValue
                tablaParametros("Mercado") = ddlMercado.SelectedItem.Text
            End If
            If (ddlTipo.SelectedIndex <> 0) Then
                tablaParametros("codOperacion") = ddlTipo.SelectedValue
                tablaParametros("Operacion") = ddlTipo.SelectedItem.Text
            End If
            If (ddlNroCuenta.SelectedIndex <> 0) Then
                tablaParametros("numerocuenta") = ddlNroCuenta.SelectedValue
            Else
                tablaParametros("numerocuenta") = ""
            End If
            GeneraReporteConstitucionForwards()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub ActualizarCuenta()
        Dim portafolio As String
        Dim mercado As String
        Dim moneda As String
        Dim clasecuenta As String

        portafolio = IIf(Me.ddlPortafolio.SelectedIndex = 0, "", Me.ddlPortafolio.SelectedValue)
        mercado = IIf(Me.ddlMercado.SelectedIndex = 0, "", Me.ddlMercado.SelectedValue)
        moneda = IIf(Me.ddlMoneda.SelectedIndex = 0, "", Me.ddlMoneda.SelectedValue)
        clasecuenta = ""

        Dim oCuentaEconomica As New CuentaEconomicaBM
        Dim dtCuentaEconomica As DataTable = oCuentaEconomica.SeleccionarPorFiltro2(portafolio, "", clasecuenta, moneda, mercado, DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(ddlNroCuenta, dtCuentaEconomica, "numerocuenta", "numerocuenta", True)

    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged

        EstablecerFecha()
        ActualizarCuenta()

    End Sub

    Private Sub ddlMercado_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        ActualizarCuenta()
    End Sub

    Private Sub ddlMoneda_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        ActualizarCuenta()
    End Sub

    Private Sub ddlTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipo.SelectedIndexChanged
        ActualizarCuenta()
    End Sub

    Private Sub EstablecerFecha()
        tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        tbFechaFin.Text = tbFechaInicio.Text
    End Sub

    Private Sub GeneraReporteConstitucionForwards()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Dim sFile As String, sTemplate As String
        Dim dt As DataTable
        Dim Archivo As String
        Dim oReporteGestionBM As New ReporteGestionBM
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Archivo = "Anexo XVIII FW SBS_" & Usuario.ToString() & "_" & DateTime.Parse(tbFechaInicio.Text).ToString("dd-MM-yy") & System.DateTime.Now.ToString("_hhmmss") & ".xls"    'JHC REQ 66056 20130208
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Archivo    'JHC REQ 66056 20130208
            sTemplate = RutaPlantillas() & "\PlantillaAnexoXVIII_FW_SBS.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            'Start a new workbook
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate) 'Load colorful template with chart
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Name = "INVERSIONES"
            oCells = oSheet.Cells
            dt = oReporteGestionBM.ListarConstitucionForwards(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), _
                                                                UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text), _
                                                                ddlPortafolio.SelectedValue, DatosRequest).Tables(0)
            DumpData(dt, oCells, CONSTITUCION_FWD_INVERSION) 'Fill in the data   
            oSheet = CType(oSheets.Item(2), Excel.Worksheet)
            oSheet.Name = "DVI"
            oCells = oSheet.Cells
            DumpData(dt, oCells, CONSTITUCION_FWD_DVI) 'Fill in the data
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Cells(1, 1).Activate()
            oSheet.SaveAs(sFile) 'Save in a temporary file
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
            'Quit Excel and thoroughly deallocate everything
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

    Private Sub DumpData(ByVal dt As DataTable, ByVal oCells As Excel.Range, Optional ByVal TipoHoja As String = CONSTITUCION_FWD_INVERSION)
        Dim dr As DataRow
        Dim iRow As Integer

        iRow = 8

        'Output Data
        For Each dr In dt.Rows
            oCells(iRow, 1) = dr("Operacion")
            oCells(iRow, 2) = dr("FechaInicio")
            oCells(iRow, 3) = dr("FechaTermino")

            oCells(iRow, 4) = dr("NumeroPoliza")
            'ini HDG OT 66471 20130121
            If TipoHoja = CONSTITUCION_FWD_INVERSION Then
                oCells(iRow, 5) = dr("MontoRecibir")
                oCells(iRow, 6) = dr("MontoEntregar")
            Else
                oCells(iRow, 5) = 0
                oCells(iRow, 6) = 0
            End If
            'fin HDG OT 66471 20130121
            oCells(iRow, 7) = dr("MonedaRecibir")
            oCells(iRow, 8) = dr("MonedaEntregar")
            oCells(iRow, 9) = dr("Comprador")
            oCells(iRow, 10) = dr("Vendedor")
            oCells(iRow, 11) = dr("TipoCambioFuturo")
            oCells(iRow, 15) = dr("ModalidadPago")

            iRow = iRow + 1
        Next

    End Sub

End Class

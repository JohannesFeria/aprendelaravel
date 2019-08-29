Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Partial Class Modulos_Tesoreria_Reportes_frmGenerarReportes
    Inherits BasePage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oUtil As New UtilDM
        If (Page.IsPostBack = False) Then
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            tbFechaFin.Text = tbFechaInicio.Text
            CargarPortafolio(True)
            CargarMercado()
            CargarMoneda(True)
            CargarNroCuenta(True)
        End If
    End Sub
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim dtPortafolios As DataTable = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dtPortafolios, "CodigoPortafolio", "Descripcion", True, "TODOS")
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dsMercado As DataSet = oMercado.Listar(DatosRequest, "A")
            HelpCombo.LlenarComboBox(ddlMercado, dsMercado.Tables(0), "CodigoMercado", "Descripcion", True, "TODOS")
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
    End Sub
    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataTable = oMoneda.GetMonedaMercadoPortafolio(String.Empty, ddlPortafolio.SelectedValue).Tables(0)
            HelpCombo.LlenarComboBox(ddlMoneda, dsMoneda, "CodigoMoneda", "Descripcion", True, "SELECCIONE")
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
            HelpCombo.LlenarComboBox(ddlNroCuenta, dtCuentaEconomica, "numerocuenta", "numerocuenta", True, "TODOS")
        End If
        ddlMoneda.Enabled = enabled
    End Sub
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Sub ValidarAntesDeImprimir()
        If Me.RbtnFiltro.SelectedValue.ToString.Length = 0 Then Throw New Exception("Debe seleccionar un Reporte")
    End Sub
    Private Sub btnVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVista.Click
        Dim opcion As String
        Try
            ValidarAntesDeImprimir()

            opcion = Me.RbtnFiltro.SelectedValue.ToString
            Dim tablaParametros As New Hashtable

            If (ddlPortafolio.SelectedIndex >= 0) Then
                tablaParametros("codPortafolio") = ddlPortafolio.SelectedValue
                tablaParametros("Portafolio") = ddlPortafolio.SelectedItem.Text.ToString().Replace("--", "")
            End If
            If (ddlMoneda.SelectedIndex >= 0) Then
                tablaParametros("codMoneda") = ddlMoneda.SelectedValue
                tablaParametros("Moneda") = IIf(ddlMoneda.SelectedItem.Text = "--SELECCIONE--", "TODOS", ddlMoneda.SelectedItem.Text)
            End If
            If (ddlMercado.SelectedIndex >= 0) Then
                tablaParametros("codMercado") = ddlMercado.SelectedValue
                tablaParametros("Mercado") = ddlMercado.SelectedItem.Text.ToString().Replace("--", "")
            End If
            If (ddlTipo.SelectedIndex >= 0) Then
                tablaParametros("codOperacion") = ddlTipo.SelectedValue
                tablaParametros("Operacion") = ddlTipo.SelectedItem.Text.ToString().Replace("--", "")
            End If
            If (ddlNroCuenta.SelectedIndex >= 0) Then
                tablaParametros("numerocuenta") = ddlNroCuenta.SelectedValue.ToString().Replace("--", "")
            End If

            Select Case (RbtnFiltro.SelectedValue)
                Case "ControlDeForwards"
                    ' context_info ==> Información del Contexto Actual (Hashtable de preferencia)
                    Session("context_info") = tablaParametros
                    Session("ParametrosReporteVencimientos") = tablaParametros 'MC #ERROR#
                Case "ReporteVencimientos"
                    Session("ParametrosReporteVencimientos") = tablaParametros
                Case "MovimientosTotales"
                    Session("ParametrosReporteMovimientos") = tablaParametros
                Case "SaldosNetos"
                    Session("ParametrosReporteSaldos") = tablaParametros
                Case "DetalleSaldosBancarios"
                    Session("ParametrosReporteDetalleSaldos") = tablaParametros

                Case "ReporteEnvioCartas" 'NO IMPLEMENTADO
                Case "ReporteMovimientosNeg" 'NO IMPLEMENTADO
                Case "ReporteFlujoEstimado" 'NO IMPLEMENTADO
            End Select

            Dim oSaldoBancario As New SaldosBancariosBM
            Dim rpt As Boolean = oSaldoBancario.ActualizaSaldosBancarios(Me.ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text))

            Dim script As String = ""
            If opcion = "ControlDeForwards" Then
                script = "../../../Modulos/Gestion/Reportes/frmVisorGestion.aspx?pportafolio=" & ddlPortafolio.SelectedValue & "&moneda=" & ddlMoneda.SelectedValue & "&pFechaIni=" & UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text.Trim) & "&pFechaFin=" & UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text.Trim) & "&pReporte=RDSDF"
            Else
                script = "frmReporte.aspx?portafolio=" & ddlPortafolio.SelectedValue & "&moneda=" & ddlMoneda.SelectedValue & "&ClaseReporte=" & Me.RbtnFiltro.SelectedValue.ToString & "&FechaInicio=" & UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text) & "&FechaFin=" & UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text)
            End If
            EjecutarJS(UIUtility.MostrarPopUp(script, "no", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False) '"CargarPopUpReportes()
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
        HelpCombo.LlenarComboBox(ddlNroCuenta, dtCuentaEconomica, "numerocuenta", "numerocuenta", True, "TODOS")
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
            Archivo = "Anexo XVIII FW SBS_" & DateTime.Parse(tbFechaInicio.Text).ToString("dd-MM-yy")
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Archivo & "_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            sTemplate = RutaPlantillas() & "\PlantillaAnexoXVIII_FW_SBS.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Name = "FWD"
            oCells = oSheet.Cells
            dt = oReporteGestionBM.ListarConstitucionForwards(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), _
                                                                UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text), _
                                                                ddlPortafolio.SelectedValue, DatosRequest).Tables(0)
            DumpData(dt, oCells)
            oSheet.SaveAs(sFile)
            oBook.Save()
            oBook.Close()
            EjecutarJS("window.open('" & sFile.Replace("\", "\\") & "');")
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
    Private Function DumpData(ByVal dt As DataTable, ByVal oCells As Excel.Range) As String
        Dim dr As DataRow, ary() As Object
        Dim iRow As Integer, cont As Integer
        iRow = 8
        For Each dr In dt.Rows
            oCells(iRow, 1) = dr("Operacion")
            oCells(iRow, 2) = dr("FechaInicio")
            oCells(iRow, 3) = dr("FechaTermino")
            oCells(iRow, 4) = dr("NumeroPoliza")
            oCells(iRow, 5) = dr("MontoRecibir")
            oCells(iRow, 6) = dr("MontoEntregar")
            oCells(iRow, 7) = dr("MonedaRecibir")
            oCells(iRow, 8) = dr("MonedaEntregar")
            oCells(iRow, 9) = dr("Comprador")
            oCells(iRow, 10) = dr("Vendedor")
            oCells(iRow, 11) = dr("TipoCambioFuturo")
            oCells(iRow, 15) = dr("ModalidadPago")
            iRow = iRow + 1
        Next
    End Function
    Private Sub GeneraCartasConstitucionForwards()
        Dim dsForwards As DataSet = New OrdenPreOrdenInversionBM().ListarForwardsCartas(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), ddlPortafolio.SelectedValue, DatosRequest)
        Dim dr As DataRow
        Dim windowsCartas As New System.Text.StringBuilder
        Dim ruta As String = New ParametrosGeneralesBM().ListarRutaGeneracionCartas(DatosRequest) + "\"
        HelpCarta.GenerarCartas(dsForwards, DatosRequest)
        For Each dr In dsForwards.Tables(0).Rows
            windowsCartas.Append(ruta & dr("NumeroCarta") & ".doc" & "&")
        Next
        Session("RutaCarta") = HelpCarta.CrearMultiCartaPDF(windowsCartas.ToString())
        EjecutarJS("window.open('../OperacionesCaja/frmVisorCarta.aspx');")
    End Sub
End Class
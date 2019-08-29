Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office.Core
Imports System.Data

Partial Class Modulos_Tesoreria_Reportes_frmGenerarReporteFuturo
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oUtil As New UtilDM
        If (Page.IsPostBack = False) Then

            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(PORTAFOLIO_MULTIFONDOS))
            tbFechaFin.Text = tbFechaInicio.Text
            CargarPortafolio(True)
            CargarMoneda(True)
            CargarNroCuenta(True)
        End If
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
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

            GeneraReporteFuturo()

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dsPortafolio = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolio.Items.Clear()
            ddlPortafolio.DataSource = dsPortafolio
            ddlPortafolio.DataValueField = "CodigoPortafolio"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio, "", "-- SELECCIONE --")
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub

    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataSet = oMoneda.GetMonedaMercadoPortafolio(String.Empty, ddlPortafolio.SelectedValue)
            ddlMoneda.Items.Clear()
            ddlMoneda.DataSource = dsMoneda
            ddlMoneda.DataValueField = "CodigoMoneda"
            ddlMoneda.DataTextField = "Descripcion"
            ddlMoneda.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMoneda, "", "-- SELECCIONE --")
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
            HelpCombo.LlenarComboBoxBusquedas(ddlNroCuenta, dtCuentaEconomica, "numerocuenta", "numerocuenta", False)
            ddlNroCuenta.Items.Insert(0, New ListItem("--Todos--"))
        End If
        ddlMoneda.Enabled = enabled
    End Sub

    Private Sub GeneraReporteFuturo()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Dim sFile As String, sTemplate As String
        Dim dt As DataTable
        Dim Archivo As String
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Archivo = "Anexo_Futuros_SBS_" & DateTime.Parse(tbFechaInicio.Text).ToString("dd-MM-yy") & ".xls"
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Archivo
            sTemplate = RutaPlantillas() & "\PlantillaPrevOrdenInversionFUT.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Name = "Futuros"
            oCells = oSheet.Cells
            Dim strMoneda As String = String.Empty
            Dim strMercado As String = String.Empty
            Dim strOperacion As String = "0"
            Dim strNroCuenta As String = String.Empty
            If (ddlMoneda.SelectedIndex <> 0) Then strMoneda = ddlMoneda.SelectedValue
            If (ddlMercado.SelectedIndex <> 0) Then strMercado = ddlMercado.SelectedValue
            If (ddlTipo.SelectedIndex <> 0) Then strOperacion = ddlTipo.SelectedValue
            If (ddlNroCuenta.SelectedIndex <> 0) Then strNroCuenta = ddlNroCuenta.SelectedValue
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            dt = oPrevOrdenInversionBM.ListarOperacionesCajaFuturos(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), _
                                                                UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text), _
                                                                ddlPortafolio.SelectedValue, strOperacion, strMoneda, strMercado, strNroCuenta, DatosRequest).Tables(0)
            DumpData(dt, oCells)
            oSheet.SaveAs(sFile)
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & Archivo)
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

    Private Sub DumpData(ByVal dt As DataTable, ByVal oCells As Excel.Range)
        Dim dr As DataRow
        Dim iRow As Integer
        iRow = 8
        For Each dr In dt.Rows
            oCells(iRow, 1) = dr("OPERACION")
            oCells(iRow, 2) = dr("BBGID")
            oCells(iRow, 3) = dr("UNDERLYING")
            oCells(iRow, 4) = dr("FECHA_TERMINADO")
            oCells(iRow, 5) = dr("CODIGO_INSTRUMENTO")
            oCells(iRow, 6) = dr("NUMERO_CONTRATOS")
            oCells(iRow, 7) = dr("PRECIO")
            oCells(iRow, 8) = dr("VALOR_MERCADO")
            oCells(iRow, 9) = dr("MONTO_NOMINAL")
            oCells(iRow, 10) = dr("MONEDA")
            oCells(iRow, 11) = dr("INTERMEDIARIO")
            oCells(iRow, 12) = dr("VENDEDOR")
            oCells(iRow, 13) = dr("MODALIDAD_DELIVERY")
            oCells(iRow, 14) = dr("MARGEN_INICIAL")
            oCells(iRow, 15) = dr("MARGEN_MANTENIMIENTO")
            iRow = iRow + 1
        Next
    End Sub

End Class

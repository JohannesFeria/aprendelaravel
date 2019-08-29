Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.IO
Imports System.Text
Imports UIUtility
Imports System.Data
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Threading
Partial Class Modulos_Tesoreria_OperacionesCaja_frmConsultaSaldosBancarios
    Inherits BasePage
    Private codigoPortafolio As String = ""
    Dim oCuentaEconomica As New CuentaEconomicaBM
    Function Rango(ByVal Numero As Integer) As String
        Dim array() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                 "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ",
                                 "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ",
                                 "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ",
                                 "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ",
                                 "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ",
                                 "FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ"}
        Return array(Numero - 1)
    End Function
    Private Sub GenerarReporte()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets
        Dim oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtCE As DataTable = oCuentaEconomica.SeleccionarCuentaEconomica(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text),
            ddlMoneda.SelectedValue, ddlBanco.SelectedValue, ddlMercado.SelectedValue, ddlClaseCuenta.SelectedValue)
            Dim dtCEM As DataTable

            Dim rescatePreliminar As Decimal = 0

            If dtCE.Rows.Count > 0 Then
                Dim sFile As String, sTemplate As String
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "CM_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                Dim Indice As Integer = 1
                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutaPlantillas() & "\" & "PlantillaCajaMovimientos.xls"
                oExcel.Visible = False : oExcel.DisplayAlerts = False
                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                oCells = oSheet.Cells
                oCells(1, 2) = ddlPortafolio.SelectedItem.Text
                oCells(2, 2) = tbFechaOperacion.Text
                Dim i As Integer = 0
                oSheet.SaveAs(sFile)
                Dim contador As Integer = 0
                Dim Mercado As String = ""
                For Each dr In dtCE.Rows
                    contador += 1
                    If Not contador = dtCE.Rows.Count Then
                        oSheet.Range(Rango(Indice) + "4", Rango(Indice + 2) + "9").Copy()
                        oSheet.Range(Rango(Indice) + "4", Rango(Indice + 2) + "9").Insert(Excel.XlDirection.xlToRight)
                    End If
                    If Not Mercado = dr("Mercado") Then
                        oCells(3, Indice) = dr("Mercado")
                        oCells(3, Indice).Interior.Color = RGB(217, 217, 217)
                    End If
                    Mercado = dr("Mercado")
                    oCells(4, Indice) = dr("Tercero") + " ( " + dr("Moneda") + ")"
                    oCells(5, Indice) = dr("NumeroCuenta")
                    oCells(6, Indice) = dr("NumeroCuentaInterBancario")
                    oCells(8, Indice + 1) = dr("SaldoDisponibleFinal")
                    oCells(9, Indice + 1) = dr("SaldoDisponibleInicial")
                    dtCEM = oCuentaEconomica.SeleccionarCuentaEconomica_Movimientos(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), dr("NumeroCuenta"))
                    Dim Fil As Integer = 10
                    For Each drM In dtCEM.Rows
                        oCells(Fil, Indice) = drM("Referencia")
                        oCells(Fil, Indice + 1) = drM("Importe")
                        oCells(Fil, Indice + 1).NumberFormat = "#,##0.00"
                        oSheet.Range(Rango(Indice + 1) + Fil.ToString).Interior.Color = RGB(218, 238, 243)
                        If CDec(drM("Importe")) < 1 Then
                            oSheet.Range(Rango(Indice + 1) + Fil.ToString).Font.Color = RGB(255, 0, 0)
                        End If
                        Fil += 1
                    Next

                    If ddlClaseCuenta.SelectedValue = "20" Then
                        oCells(Fil + 12, Indice) = "Presaldo Final"
                        oCells(Fil + 12, Indice + 1) = "=SUM(" + Rango(Indice + 1) + "9:" + Rango(Indice + 1) + (Fil + 11).ToString + ")"
                        oSheet.Range(Rango(Indice) + (Fil + 12).ToString).Interior.Color = RGB(0, 32, 96)
                        oSheet.Range(Rango(Indice + 1) + (Fil + 12).ToString).Interior.Color = RGB(217, 217, 217)
                        oSheet.Range(Rango(Indice) + (Fil + 12).ToString).Font.Color = RGB(255, 255, 255)
                        oSheet.Range(Rango(Indice + 1) + (Fil + 12).ToString).Font.Color = RGB(0, 0, 0)
                        oSheet.Range(Rango(Indice) + (Fil + 12).ToString).Font.Bold = True
                        oSheet.Range(Rango(Indice + 1) + (Fil + 12).ToString).Font.Bold = True

                        'OT11237 - 26/02/2018 - Ian Pastor M.
                        'PROGRAMACIÓN OBTENER RESCATES PRE-LIMINARES
                        Dim fila As Integer = Fil + 12
                        Fil += 13
                        rescatePreliminar = oCuentaEconomica.ObtenerRescatesPreliminaresSisOpe(ddlPortafolio.SelectedValue, _
                                                                                               tbFechaOperacion.Text, dr("CodigoEntidad"), _
                                                                                               dr("CodigoMoneda"), DatosRequest)
                        oCells(Fil, Indice) = "Rescate pre-liminar"
                        oCells(Fil, Indice + 1) = rescatePreliminar
                        oCells(Fil, Indice + 1).NumberFormat = "#,##0.00"
                        If rescatePreliminar < 1 Then oCells(Fil, Indice + 1).Font.Color = RGB(255, 0, 0)

                        Fil += 2
                        oCells(Fil, Indice) = "Pre-saldo final (Inc. rescates pre-liminares)"
                        oCells(Fil, Indice).Interior.Color = RGB(70, 130, 180)
                        oCells(Fil, Indice).Font.Color = RGB(255, 255, 255)
                        oCells(Fil, Indice).Font.Name = "Arial"
                        oCells(Fil, Indice).Font.Size = 10
                        'oCells(Fil, indice + 1) = saldoDisponibleFinal + rescatePreliminar
                        oCells(Fil, Indice + 1) = "=SUM(" + Rango(Indice + 1) + (fila).ToString() + ":" + Rango(Indice + 1) + (Fil - 1).ToString + ")"
                        oCells(Fil, Indice + 1).Interior.Color = RGB(217, 217, 217)
                        oCells(Fil, Indice + 1).NumberFormat = "#,##0.00"
                        If Decimal.Parse(dr("SaldoDisponibleFinal")) + rescatePreliminar < 1 Then oCells(Fil, Indice + 1).Font.Color = RGB(255, 0, 0)
                        'OT11237 - Fin
                    End If

                    ''OT11237 - 26/02/2018 - Ian Pastor M.
                    ''Formato Celta "Pre-saldo Final"
                    'oCells(Fil, Indice) = "Pre-saldo Final"
                    'oCells(Fil, Indice).Interior.Color = RGB(4, 36, 100)
                    'oCells(Fil, Indice).Font.color = RGB(255, 255, 255)
                    'oCells(Fil, Indice).Font.Name = "Arial"
                    'oCells(Fil, Indice).Font.Size = 10
                    'oCells(Fil, Indice + 1) = dr("SaldoDisponibleFinal")
                    'oCells(Fil, Indice + 1).NumberFormat = "#,##0.00"
                    'oCells(Fil, Indice + 1).Interior.Color = RGB(217, 217, 217)

                    ''PROGRAMACIÓN OBTENER RESCATES PRE-LIMINARES
                    'Fil += 1
                    'rescatePreliminar = oCuentaEconomica.ObtenerRescatesPreliminaresSisOpe(ddlPortafolio.SelectedValue, tbFechaOperacion.Text, dr("CodigoEntidad"), dr("CodigoMoneda"), DatosRequest)
                    'oCells(Fil, Indice) = "Rescate pre-liminar"
                    'oCells(Fil, Indice + 1) = rescatePreliminar
                    'oCells(Fil, Indice + 1).NumberFormat = "#,##0.00"
                    'If rescatePreliminar < 1 Then oCells(Fil, Indice + 1).Font.Color = RGB(255, 0, 0)

                    'Fil += 2
                    'oCells(Fil, Indice) = "Pre-saldo final (Inc. rescates pre-liminares)"
                    'oCells(Fil, Indice).Interior.Color = RGB(70, 130, 180)
                    'oCells(Fil, Indice).Font.Color = RGB(255, 255, 255)
                    'oCells(Fil, Indice).Font.Name = "Arial"
                    'oCells(Fil, Indice).Font.Size = 10
                    'oCells(Fil, Indice + 1) = Decimal.Parse(dr("SaldoDisponibleFinal")) + rescatePreliminar
                    'oCells(Fil, Indice + 1).Interior.Color = RGB(217, 217, 217)
                    'oCells(Fil, Indice + 1).NumberFormat = "#,##0.00"
                    'If Decimal.Parse(dr("SaldoDisponibleFinal")) + rescatePreliminar < 1 Then oCells(Fil, Indice + 1).Font.Color = RGB(255, 0, 0)
                    ''OT11237 - Fin

                    oSheet.Columns(Rango(Indice + 2) + ":" + Rango(Indice + 2)).ColumnWidth = 3
                    Indice = Indice + 3
                Next
                oExcel.Cells.EntireColumn.AutoFit()
                oBook.Save()
                oBook.Close()
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "CM_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
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
#Region "Eventos de la Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarMercado(True)
            CargarMoneda(True)
            CargarClaseCuenta(True)
            CargarBanco(ddlMercado.SelectedValue, True)
            CargarPortafolio(True)
            EstablecerFecha()
        End If
        If ddlPortafolio.SelectedIndex > 0 Then
            codigoPortafolio = ddlPortafolio.SelectedValue
        End If
        btnBuscar.Attributes.Add("onclick", "return ValidarDatos();")
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub btnMovimientos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovimientos.Click
        If hdNroCuenta.Value <> "" Then
            Dim sCodigoBanco As String = ddlBanco.SelectedValue.Trim
            Dim sClaseCuenta As String = dgLista.SelectedRow.Cells(14).Text
            Dim sCodigoMoneda As String = ddlMoneda.SelectedValue.Trim
            Dim codPortafolio As String = hdNroCuenta.Value.Split(",")(0)
            Dim codMercado As String = hdNroCuenta.Value.Split(",")(2)
            Dim nroCuenta As String = hdNroCuenta.Value.Split(",")(1)
            Dim fecha As String = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString

            Response.Redirect("frmConsultaMovimientosBancarios.aspx?codMercado=" & codMercado & "&codPortafolio=" & codPortafolio & "&nroCuenta=" & nroCuenta & "&fecha=" & fecha & "&CodigoBanco=" & sCodigoBanco & "&ClaseCuenta=" & sClaseCuenta & "&CodigoMoneda=" & sCodigoMoneda)
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            dgLista.PageIndex = 0
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Function SeleccionarSaldosBancarios(ByVal dsCuentaEconomica As CuentaEconomicaBE) As DataSet
        Dim oSaldoBancario As New SaldosBancariosBM
        Return oSaldoBancario.SeleccionarPorFiltro(dsCuentaEconomica, DatosRequest)
    End Function

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ddlPortafolio.SelectedIndex <> 0 Then
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
        End If
    End Sub
#End Region
#Region "Metodos de Control"
    Private Function ObtenerFechaApertura() As Decimal
        If tbFechaOperacion.Text.Trim = "" Then
            Return Decimal.Parse(DateTime.Now.ToString("yyyyMMdd"))
        Else
            Dim sFecha As String() = tbFechaOperacion.Text.Split("/")
            Return Decimal.Parse(sFecha(2) & sFecha(1) & sFecha(0))
        End If
    End Function
#End Region
#Region "CargarDatos"
    Sub CargarGrilla()
        Dim dsCuentaEconomica As New CuentaEconomicaBE
        Dim roCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.NewCuentaEconomicaRow
        Dim oSaldoBancario As New SaldosBancariosBM
        'OT 10238 - 25/04/2017 - Carlos Espejo
        'Descripcion: Se comenta por que se actualiza en tiempo real
        'rpt = oSaldoBancario.ActualizaSaldosBancarios(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        'OT 10238 Fin
        roCuentaEconomica.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        roCuentaEconomica.CodigoMoneda = ddlMoneda.SelectedValue
        roCuentaEconomica.NumeroCuenta = ""
        roCuentaEconomica.CodigoClaseCuenta = ddlClaseCuenta.SelectedValue
        roCuentaEconomica.CodigoMercado = ddlMercado.SelectedValue
        roCuentaEconomica.CodigoTercero = ddlBanco.SelectedValue
        roCuentaEconomica.FechaCreacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        dsCuentaEconomica.CuentaEconomica.AddCuentaEconomicaRow(roCuentaEconomica)
        Dim dt As DataTable = SeleccionarSaldosBancarios(dsCuentaEconomica).Tables(0)
        dgLista.DataSource = dt
        dgLista.DataBind()
        hdGrilla.Value = dt.Rows.Count
        lbContador.Text = "Registros Encontrados: " + dt.Rows.Count.ToString
    End Sub
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dt As New DataTable
            dt = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True, "SELECCIONE")
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
        ddlPortafolio.SelectedIndex = 1
    End Sub
    Private Sub CargarClaseCuenta(Optional ByVal enabled As Boolean = True)
        Dim dtBancos As DataTable
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone(ddlMercado.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
            dtBancos = dsBanco.Tables(0)

            ddlClaseCuenta.Items.Clear()

            Dim r As DataRowView
            For Each r In dtBancos.DefaultView
                If ddlClaseCuenta.Items.FindByValue(CType(r("CodigoClaseCuenta"), String)) Is Nothing Then
                    ddlClaseCuenta.Items.Add(New ListItem(CType(r("NombreClaseCuenta"), String), CType(r("CodigoClaseCuenta"), String)))
                End If
            Next

            If ddlClaseCuenta.Items.Count = 1 Then
                UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
                ddlClaseCuenta.SelectedIndex = 1
            Else
                UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
                ddlClaseCuenta.SelectedValue = "20"
            End If
        Else
            ddlClaseCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
        End If
        ddlClaseCuenta.Enabled = enabled
    End Sub

    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataSet = oMoneda.GetMonedaMercadoPortafolio(ddlMercado.SelectedValue, ddlPortafolio.SelectedValue)
            Dim cm As String = ddlMoneda.SelectedValue

            ddlMoneda.Items.Clear()
            ddlMoneda.DataSource = dsMoneda
            ddlMoneda.DataValueField = "CodigoMoneda"
            ddlMoneda.DataTextField = "Descripcion"
            ddlMoneda.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
            If cm <> "" Then
                Try
                    ddlMoneda.SelectedValue = cm
                Catch ex As Exception
                End Try
            End If
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        End If
        ddlMoneda.Enabled = enabled
    End Sub

    Private Sub CargarBanco(ByVal codigoMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dt As DataTable = oBanco.SeleccionarBancoPorCodigoMercadoYPortafolio(ddlMercado.SelectedValue, ddlPortafolio.SelectedValue).Tables(0)
            HelpCombo.LlenarComboBox(ddlBanco, dt, "CodigoTercero", "Descripcion", True, "SELECCIONE")
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
    End Sub
#End Region
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim tablaParametros As New Hashtable
        tablaParametros("codPortafolio") = ddlPortafolio.SelectedValue
        tablaParametros("codMoneda") = ddlMoneda.SelectedValue
        tablaParametros("codBanco") = ddlBanco.SelectedValue
        tablaParametros("codClaseCuenta") = ddlClaseCuenta.SelectedValue
        tablaParametros("Portafolio") = IIf(ddlPortafolio.SelectedIndex = 0, "", ddlPortafolio.SelectedItem.Text)
        tablaParametros("Moneda") = IIf(ddlMoneda.SelectedIndex = 0, "", ddlMoneda.SelectedItem.Text)
        tablaParametros("Banco") = IIf(ddlBanco.SelectedIndex = 0, "", ddlBanco.SelectedItem.Text)
        tablaParametros("Mercado") = IIf(ddlMercado.SelectedIndex = 0, "", ddlMercado.SelectedValue)
        tablaParametros("codMercado") = IIf(ddlMercado.SelectedIndex = 0, "", ddlMercado.SelectedValue)
        tablaParametros("ClaseCuenta") = IIf(ddlClaseCuenta.SelectedIndex = 0, "", ddlClaseCuenta.SelectedItem.Text)
        Session("ParametrosReporteSaldos") = tablaParametros
        EjecutarJS(UIUtility.MostrarPopUp("../Reportes/frmReporte.aspx?ClaseReporte=SaldosNetos&FechaInicio=" & UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) & "&FechaFin=0", "", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        If ddlPortafolio.SelectedIndex <> 0 Then
            EstablecerFecha()
        End If
        CargarBanco(String.Empty, True)
        CargarMoneda(True)
    End Sub
    Private Sub EstablecerFecha()
        If ddlPortafolio.SelectedValue = "" Then
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(ObtenerFechaMaximaNegocio())
        Else
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        End If
    End Sub
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dt As DataTable = oMercado.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO).Tables(0)
            HelpCombo.LlenarComboBox(ddlMercado, dt, "CodigoMercado", "Descripcion", True, "SELECCIONE")
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
    End Sub
    Private Sub ddlMercado_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        CargarMoneda(True)
        CargarBanco(ddlMercado.SelectedValue, True)
    End Sub
    Private Sub ddlBanco_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBanco.SelectedIndexChanged
        CargarClaseCuenta(True)
    End Sub
    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            Dim dsCuentaEconomica As New CuentaEconomicaBE
            Dim roCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.NewCuentaEconomicaRow
            roCuentaEconomica.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
            roCuentaEconomica.CodigoMoneda = ddlMoneda.SelectedValue
            roCuentaEconomica.NumeroCuenta = ""
            roCuentaEconomica.CodigoClaseCuenta = ddlClaseCuenta.SelectedValue
            roCuentaEconomica.CodigoMercado = ddlMercado.SelectedValue
            roCuentaEconomica.CodigoTercero = ddlBanco.SelectedValue
            roCuentaEconomica.FechaCreacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            dsCuentaEconomica.CuentaEconomica.AddCuentaEconomicaRow(roCuentaEconomica)
            Dim dt As DataTable = SeleccionarMovimientosBancarios(dsCuentaEconomica).Tables(0)
            If dt.Rows.Count > 0 Then
                GenerarArchivoMovimientos(dt)
            Else
                AlertaJS("No existe datos de movimientos.")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub GenerarArchivoMovimientos(ByVal pDt As DataTable)
        Dim sFile As String
        Dim cabecera As String = ""
        Dim detalle As String = ""
        Dim sArchivo As String = ""
        Dim i As Integer
        Dim tw As TextWriter
        sArchivo = "Movimientos"
        sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & sArchivo & "_" & Usuario.ToString() & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & System.DateTime.Now.ToString("_hhmmss") & ".txt"
        Dim totalRegistros As Integer = 0
        Dim existeFile As Boolean = False
        tw = New StreamWriter(sFile)
        cabecera = cabecera + pDt.Columns(0).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(1).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(2).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(3).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(4).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(5).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(6).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(7).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(8).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(9).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(10).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(11).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(12).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(13).ColumnName & "|"
        cabecera = cabecera + pDt.Columns(14).ColumnName

        tw.WriteLine(cabecera)
        i = 0
        While (i < pDt.Rows.Count())
            detalle = detalle + pDt.Rows(i).Item(0) & "|"
            detalle = detalle + pDt.Rows(i).Item(1) & "|"
            detalle = detalle + pDt.Rows(i).Item(2) & "|"
            detalle = detalle + pDt.Rows(i).Item(3) & "|"
            detalle = detalle + pDt.Rows(i).Item(4) & "|"
            detalle = detalle + pDt.Rows(i).Item(5) & "|"
            detalle = detalle + pDt.Rows(i).Item(6) & "|"
            detalle = detalle + pDt.Rows(i).Item(7) & "|"
            detalle = detalle + pDt.Rows(i).Item(8).ToString() & "|"
            detalle = detalle + pDt.Rows(i).Item(9) & "|"
            detalle = detalle + pDt.Rows(i).Item(10) & "|"
            detalle = detalle + pDt.Rows(i).Item(11) & "|"
            detalle = detalle + pDt.Rows(i).Item(12) & "|"
            detalle = detalle + pDt.Rows(i).Item(13) & "|"
            detalle = detalle + pDt.Rows(i).Item(14)
            tw.WriteLine(detalle)
            i = i + 1
            detalle = ""
        End While
        tw.Close()
        Response.Clear()
        Response.ContentType = " text/plain"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & System.IO.Path.GetFileName(sFile))
        Response.WriteFile(sFile)
        Response.End()
    End Sub
    Private Function SeleccionarMovimientosBancarios(ByVal dsCuentaEconomica As CuentaEconomicaBE) As DataSet
        Dim oSaldoBancario As New SaldosBancariosBM
        Return oSaldoBancario.SeleccionarMovimientosBancarios(dsCuentaEconomica, DatosRequest)
    End Function
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub
    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        If e.CommandName = "Seleccionar" Then
            Dim datos As String() = e.CommandArgument.ToString.Split(",")
            dgLista.SelectedIndex = datos(3)
            hdNroCuenta.Value = e.CommandArgument.ToString
        End If
    End Sub
    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseover", "this.style.cursor='hand'")
            Dim cnt As Integer
            For cnt = 6 To 11
                If e.Row.Cells(cnt).Text < 0 Then
                    e.Row.Cells(cnt).ForeColor = Drawing.Color.Red
                End If
            Next
        End If
    End Sub
    Protected Sub btnexcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexcel.Click
        GenerarReporte()
    End Sub
End Class
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports ParametrosSIT
Imports System.IO
Partial Class Modulos_Tesoreria_Reportes_frmReporteCajaMovimientos
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
    Dim oCuentaEconomica As New CuentaEconomicaBM
    Dim oMercado As New MercadoBM
    Dim oClaseCuenta As New ClaseCuentaBM
    Dim oSaldoBancario As New SaldosBancariosBM
    Sub CargaClaseCuenta()
        Try
            HelpCombo.LlenarComboBox(ddlClaseCuenta, oClaseCuenta.Listar().Tables(0), "CodigoClaseCuenta", "Descripcion", True)
            ddlClaseCuenta.SelectedValue = "20"
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        HelpCombo.LlenarComboBox(ddlmercado, oMercado.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO).Tables(0), "CodigoMercado", "Descripcion", True)
    End Sub
    Function Rango(ByVal Numero As Integer) As String
        Dim array() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                 "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV",
                                 "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR",
                                 "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN",
                                 "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ",
                                 "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED", "EE", "EF",
                                 "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ", "FA", "FB",
                                 "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX",
                                 "FY", "FZ"}
        Return array(Numero - 1)
    End Function
    'OT 10362 - 18/05/2017 - Carlos Espejo
    'Descripcion: Rutina para imprimir el la seccion de autosuma.
    Private Sub ImprimePresaldo(oCells As Excel.Range, oSheet As Excel.Worksheet, Fil As Integer, indice As Integer, _
                        codigoEntidad As String, saldoDisponibleFinal As Decimal, codigoPortafolio As String, _
                        codigoMonedaCuenta As String)
        If ddlClaseCuenta.SelectedValue = "20" And chkPresaldo.Checked Then
            Dim rescatePreliminar As Decimal = 0.0
            oCells(Fil + 12, indice) = "Presaldo Final"
            oCells(Fil + 12, indice + 1) = "=SUM(" + Rango(indice + 1) + "9:" + Rango(indice + 1) + (Fil + 11).ToString + ")"
            oSheet.Range(Rango(indice) + (Fil + 12).ToString).Interior.Color = RGB(0, 32, 96)
            oSheet.Range(Rango(indice + 1) + (Fil + 12).ToString).Interior.Color = RGB(217, 217, 217)
            oSheet.Range(Rango(indice) + (Fil + 12).ToString).Font.Color = RGB(255, 255, 255)
            oSheet.Range(Rango(indice + 1) + (Fil + 12).ToString).Font.Color = RGB(0, 0, 0)
            oSheet.Range(Rango(indice) + (Fil + 12).ToString).Font.Bold = True
            oSheet.Range(Rango(indice + 1) + (Fil + 12).ToString).Font.Bold = True

            'OT11237 - 26/02/2018 - Ian Pastor M.
            'PROGRAMACIÓN OBTENER RESCATES PRE-LIMINARES
            Dim fila As Integer = Fil + 12
            Fil += 13
            rescatePreliminar = oCuentaEconomica.ObtenerRescatesPreliminaresSisOpe(codigoPortafolio, tbFechaInicio.Text, _
                                                                                   codigoEntidad, codigoMonedaCuenta, DatosRequest)
            oCells(Fil, indice) = "Rescate pre-liminar"
            oCells(Fil, indice + 1) = rescatePreliminar
            oCells(Fil, indice + 1).NumberFormat = "#,##0.00"
            If rescatePreliminar < 1 Then oCells(Fil, indice + 1).Font.Color = RGB(255, 0, 0)

            Fil += 2
            oCells(Fil, indice) = "Pre-saldo final (Inc. rescates pre-liminares)"
            oCells(Fil, indice).Interior.Color = RGB(70, 130, 180)
            oCells(Fil, indice).Font.Color = RGB(255, 255, 255)
            oCells(Fil, indice).Font.Name = "Arial"
            oCells(Fil, indice).Font.Size = 10
            'oCells(Fil, indice + 1) = saldoDisponibleFinal + rescatePreliminar
            oCells(Fil, indice + 1) = "=SUM(" + Rango(indice + 1) + (fila).ToString() + ":" + Rango(indice + 1) + (Fil - 1).ToString + ")"
            oCells(Fil, indice + 1).Interior.Color = RGB(217, 217, 217)
            oCells(Fil, indice + 1).NumberFormat = "#,##0.00"
            If saldoDisponibleFinal + rescatePreliminar < 1 Then oCells(Fil, indice + 1).Font.Color = RGB(255, 0, 0)
            'OT11237 - Fin
        End If
    End Sub
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
            'OT 10238 - 25/04/2017 - Carlos Espejo
            'Descripcion: Se comenta por que se actualiza en tiempo real
            'oSaldoBancario.ActualizaSaldosBancarios(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
            'OT 10238 Fin
            Dim dtCE As DataTable = oCuentaEconomica.SeleccionarCuentaEconomica(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), "", "", _
            ddlmercado.SelectedValue, ddlClaseCuenta.SelectedValue)
            Dim dtCEM As DataTable
            Dim rescatePreliminar As Decimal = 0
            If dtCE.Rows.Count > 0 Then
                Dim sFile As String, sTemplate As String
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "CM_" & Usuario.ToString() & _
                String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
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
                oCells(2, 2) = tbFechaInicio.Text
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
                    If ddlClaseCuenta.SelectedValue = "10" Then
                        oCells(8, Indice) = "Saldo Libro Banco"
                    End If
                    oCells(8, Indice + 1) = dr("SaldoDisponibleFinal")
                    oCells(9, Indice + 1) = dr("SaldoDisponibleInicial")
                    dtCEM = oCuentaEconomica.SeleccionarCuentaEconomica_Movimientos(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), _
                    dr("NumeroCuenta"))
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
                    If ddlClaseCuenta.SelectedValue = "10" Then
                        rescatePreliminar = 0.0
                        oCells(Fil, Indice) = "Saldo Estado de CTA"
                        oCells(Fil, Indice + 1) = dr("SaldoEstadoCTA")
                        oSheet.Range(Rango(Indice) + Fil.ToString).Interior.Color = RGB(0, 32, 96)
                        oSheet.Range(Rango(Indice + 1) + Fil.ToString).Interior.Color = RGB(0, 32, 96)
                        oSheet.Range(Rango(Indice) + Fil.ToString).Font.Color = RGB(255, 255, 255)
                        oSheet.Range(Rango(Indice + 1) + Fil.ToString).Font.Color = RGB(255, 255, 255)
                        oSheet.Range(Rango(Indice) + Fil.ToString).Font.Bold = True
                        oSheet.Range(Rango(Indice + 1) + Fil.ToString).Font.Bold = True


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
                        'rescatePreliminar = oCuentaEconomica.ObtenerRescatesPreliminaresSisOpe(ddlPortafolio.SelectedValue, tbFechaInicio.Text, dr("CodigoEntidad"), dr("CodigoMonada"), DatosRequest)
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


                    End If
                    'OT 10362 - 11/05/2017 - Carlos Espejo
                    'Descripcion: Se imprime una fila de pre saldo
                    ImprimePresaldo(oCells, oSheet, Fil, Indice, dr("CodigoEntidad").ToString(), Decimal.Parse(dr("SaldoDisponibleFinal")), ddlPortafolio.SelectedValue, dr("CodigoMoneda"))
                    'OT 10362 Fin
                    oSheet.Columns(Rango(Indice + 2) + ":" + Rango(Indice + 2)).ColumnWidth = 3
                    Indice = Indice + 3
                Next
                oExcel.Cells.EntireColumn.AutoFit()
                oBook.Save()
                oBook.Close()
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "CM_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & _
                String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
                Response.WriteFile(sFile)
                Response.End()
            Else
                AlertaJS("No existen registros que mostrar para esta fecha y portafolio.")
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
        'OT10689 - Fin.
    End Sub
    Private Sub CargarPortafolio()
        HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "Todos")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Page.IsPostBack = False) Then
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            CargarPortafolio()
            CargarMercado()
            CargaClaseCuenta()
        End If
    End Sub
    Protected Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        If ddlPortafolio.SelectedValue = "" Then
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(Date.Now.ToString("yyyyMMdd"))
        Else
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        End If
    End Sub
    Protected Sub btnGenera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenera.Click
        Try
            If ddlPortafolio.SelectedValue = "" Then
                GenerarReporteTodos()
            Else
                GenerarReporte()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub GenerarReporteTodos()
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oSheetCopia As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim strPortafolioCodigo As String
            Dim strPortafolioNombre As String
            Dim tblPortafolio As DataTable
            Dim intHojaNumero As Integer
            Dim sFile As String, sTemplate As String
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "CM_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", _
            DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaCajaMovimientosTodos.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            intHojaNumero = oBook.Worksheets.Count
            Dim dteFechaHoy As Date = tbFechaInicio.Text
            Dim rescatePreliminar As Decimal = 0.0
            tblPortafolio = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            For Each drPortafolio In tblPortafolio.Rows
                strPortafolioCodigo = drPortafolio("CodigoPortafolio")
                strPortafolioNombre = drPortafolio("Descripcion")
                'OT 10238 - 25/04/2017 - Carlos Espejo
                'Descripcion: Se comenta por que se actualiza en tiempo real
                'oSaldoBancario.ActualizaSaldosBancarios(strPortafolioCodigo, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
                'OT 10238 Fin
                Dim dtCE As DataTable = oCuentaEconomica.SeleccionarCuentaEconomica(strPortafolioCodigo, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), _
                "", "", ddlmercado.SelectedValue, ddlClaseCuenta.SelectedValue)
                Dim dtCEM As DataTable
                If dtCE.Rows.Count > 0 Then
                    Dim Indice As Integer = 1
                    intHojaNumero = intHojaNumero + 1
                    oBook.Sheets(1).Copy(after:=oBook.Sheets(intHojaNumero - 1))
                    oSheet = oBook.Sheets(intHojaNumero)
                    oSheet.Name = strPortafolioNombre + "-" + strPortafolioCodigo
                    oSheet.Visible = Excel.XlSheetVisibility.xlSheetVisible
                    oCells = oSheet.Cells
                    oCells(1, 2) = strPortafolioNombre
                    oCells(2, 2) = tbFechaInicio.Text
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
                        If ddlClaseCuenta.SelectedValue = "10" Then
                            oCells(8, Indice) = "Saldo Libro Banco"
                        End If
                        oCells(8, Indice + 1) = dr("SaldoDisponibleFinal")
                        oCells(9, Indice + 1) = dr("SaldoDisponibleInicial")
                        dtCEM = oCuentaEconomica.SeleccionarCuentaEconomica_Movimientos(strPortafolioCodigo, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), _
                        dr("NumeroCuenta"))
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
                        If ddlClaseCuenta.SelectedValue = "10" Then
                            oCells(Fil, Indice) = "Saldo Estado de CTA"
                            oCells(Fil, Indice + 1) = dr("SaldoEstadoCTA")
                            oSheet.Range(Rango(Indice) + Fil.ToString).Interior.Color = RGB(0, 32, 96)
                            oSheet.Range(Rango(Indice + 1) + Fil.ToString).Interior.Color = RGB(0, 32, 96)
                            oSheet.Range(Rango(Indice) + Fil.ToString).Font.Color = RGB(255, 255, 255)
                            oSheet.Range(Rango(Indice + 1) + Fil.ToString).Font.Color = RGB(255, 255, 255)
                            oSheet.Range(Rango(Indice) + Fil.ToString).Font.Bold = True
                            oSheet.Range(Rango(Indice + 1) + Fil.ToString).Font.Bold = True

                        End If
                        'OT 10362 - 11/05/2017 - Carlos Espejo
                        'Descripcion: Se imprime una fila de pre saldo
                        ImprimePresaldo(oCells, oSheet, Fil, Indice, dr("CodigoEntidad").ToString(), Decimal.Parse(dr("SaldoDisponibleFinal")), strPortafolioCodigo, dr("CodigoMoneda"))
                        'OT 10362 Fin
                        oSheet.Columns(Rango(Indice + 2) + ":" + Rango(Indice + 2)).ColumnWidth = 3
                        Indice = Indice + 3
                    Next
                    oSheet.SaveAs(sFile)
                    oExcel.Cells.EntireColumn.AutoFit()
                End If
            Next
            oBook.Worksheets(1).Delete()
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "CM_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & _
            String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
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
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
End Class
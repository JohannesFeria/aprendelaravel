Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office

Partial Class Modulos_Inversiones_frmInventarioForward
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
    Private Const VWS_CODIGOORDEN As String = "CodigoOrden"
    Private Const INDICE_DGLISTA_CODIGOORDEN As Integer = 18
    Private Const INDICE_DGLISTA_FONDO As Integer = 2
    Private Const INDICE_DGLISTA_PVECTOR As Integer = 17
    Private Const INDICE_DGLISTA_MTMPEN As Integer = 16
    Private Const INDICE_DGLISTA_MTMUSD As Integer = 15
    Private Const INDICE_DGLISTA_REF As Integer = 1

    Private Sub LimpiarControles()
        TxtPrecioVector.Text = ""
        TxtMtm.Text = ""
        TxtMtmDestino.Text = ""
        TxtRef.Text = ""
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            Try
                Dim oUtil As New UtilDM
                CargarCombos()
                tbFechaVencimientoDesde.Text = oUtil.RetornarFechaNegocio
                tbFechaVencimientoHasta.Text = oUtil.RetornarFechaNegocio
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        End If
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Public Sub CargarCombos()
        ddlFondo.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlFondo.DataValueField = "CodigoPortafolio"
        ddlFondo.DataTextField = "Descripcion"
        ddlFondo.DataBind()
        ddlFondo.Items.Insert(0, New ListItem("Todos"))
        UIUtility.CargarMonedaOI(ddlMonedaNegociada)
        ddlMonedaNegociada.Items.RemoveAt(0)
        ddlMonedaNegociada.Items.Insert(0, New ListItem("Todos"))   'HDG OT 63375 20110715
        UIUtility.CargarMonedaOI(ddlMonedaDestino)
        ddlMonedaDestino.Items.RemoveAt(0)
        ddlMonedaDestino.Items.Insert(0, New ListItem("Todos"))   'HDG OT 63375 20110715

        Dim dt As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dt = oParametrosGeneralesBM.Listar("FECHRANGO", Me.DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlFechas, dt, "Valor", "Nombre", False)
        Me.ddlFechas.SelectedValue = 2
        dt = oParametrosGeneralesBM.Listar("ESTADOFWD", Me.DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlEstado, dt, "Valor", "Nombre", True)

        Dim DtablaMercado As New DataTable
        Dim oMercadoBM As New MercadoBM
        DtablaMercado = oMercadoBM.ListarActivos(DatosRequest).Tables(0)
        DtablaMercado.DefaultView.Sort = "CodigoMercado"
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlMercado, DtablaMercado, "CodigoMercado", "Descripcion", True)
    End Sub
    Private Sub CargarGrilla()

        Dim sFondo As String = IIf(ddlFondo.SelectedValue.ToString = "Todos", "", ddlFondo.SelectedValue.ToString)
        Dim sMonedaNeg As String = IIf(ddlMonedaNegociada.SelectedValue.ToString = "Todos", "", ddlMonedaNegociada.SelectedValue.ToString)
        Dim sMonedaDes As String = IIf(ddlMonedaDestino.SelectedValue.ToString = "Todos", "", ddlMonedaDestino.SelectedValue.ToString)
        Dim dEstado As Decimal = IIf(ddlEstado.SelectedValue.ToString = "Todos", 0, ddlEstado.SelectedValue.ToString)
        Dim sMercado As String = IIf(ddlMercado.SelectedValue.ToString = "Todos", "", ddlMercado.SelectedValue.ToString)

        Dim dsOperaciones As DataSet = New OrdenPreOrdenInversionBM().ListarVencimientoForward(sFondo, Now.ToString("yyyyMMdd"), UIUtility.ConvertirFechaaDecimal(tbFechaVencimientoDesde.Text), UIUtility.ConvertirFechaaDecimal(tbFechaVencimientoHasta.Text), sMonedaNeg, sMonedaDes, dEstado, ddlFechas.SelectedValue, sMercado, DatosRequest)

        lblCantidad.Text = String.Format("({0})", dsOperaciones.Tables(0).Rows.Count)
        Me.ibProcesar.Visible = IIf(dsOperaciones.Tables(0).Rows.Count > 0, True, False)
        dgLista.DataSource = dsOperaciones
        dgLista.DataBind()
        dgLista.SelectedIndex = -1
    End Sub

    Private Sub ibProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibProcesar.Click
        Dim objB As OrdenPreOrdenInversionBM
        Try
            objB = New OrdenPreOrdenInversionBM
            If Me.txtNumeroContrato.Text.Trim.Equals("") Then
                AlertaJS("Debe ingresar el Número de contrato.")
                Exit Sub
            End If
            If Viewstate(VWS_CODIGOORDEN) = Nothing Then
                AlertaJS("Debe seleccionar un Forward.")
                Exit Sub
            End If
            objB.ModificarNumeroContratoForward(ViewState("Fondo"), ViewState("CodigoOrden"), Me.txtNumeroContrato.Text, Convert.ToDecimal(IIf(TxtMtm.Text = "", 0, TxtMtm.Text)), Convert.ToDecimal(IIf(TxtMtmDestino.Text = "", 0, TxtMtmDestino.Text)), Convert.ToDecimal(IIf(TxtPrecioVector.Text = "", 0, TxtPrecioVector.Text)), DatosRequest)
            AlertaJS("Datos se han actualizado satisfactoriamente...!")

            Me.txtNumeroContrato.Text = ""
            Viewstate(VWS_CODIGOORDEN) = Nothing
            LimpiarControles()
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        Finally
            objB = Nothing
            GC.Collect()
        End Try
    End Sub
    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim index As Integer = 0
            If e.CommandName = "Select" Then
                Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                index = row.RowIndex
                ViewState("CodigoOrden") = HttpUtility.HtmlDecode(dgLista.Rows.Item(index).Cells(INDICE_DGLISTA_CODIGOORDEN).Text())
                ViewState("Fondo") = HttpUtility.HtmlDecode(dgLista.Rows.Item(index).Cells(INDICE_DGLISTA_FONDO).Text())
                TxtPrecioVector.Text = IIf(HttpUtility.HtmlDecode(dgLista.Rows.Item(index).Cells(INDICE_DGLISTA_PVECTOR).Text).Trim = "", "0", HttpUtility.HtmlDecode(dgLista.Rows.Item(index).Cells(INDICE_DGLISTA_PVECTOR).Text.Trim))
                TxtMtm.Text = HttpUtility.HtmlDecode(IIf(dgLista.Rows.Item(index).Cells(INDICE_DGLISTA_MTMUSD).Text.Trim = "&nbsp;", "0", dgLista.Rows.Item(index).Cells(INDICE_DGLISTA_MTMUSD).Text.Trim))
                TxtMtmDestino.Text = HttpUtility.HtmlDecode(dgLista.Rows.Item(index).Cells(INDICE_DGLISTA_MTMPEN).Text.Trim)
                TxtRef.Text = HttpUtility.HtmlDecode(IIf(dgLista.Rows.Item(index).Cells(INDICE_DGLISTA_REF).Text.Trim = "&nbsp;", "", dgLista.Rows.Item(index).Cells(INDICE_DGLISTA_REF).Text.Trim))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar un registro en la Grilla")
        End Try
    End Sub

    Private Sub ExportarExcelFwd(ByVal dt As DataTable)
        Dim strRutaSIT As String = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
        Dim iRow As Integer = 0, iCol As Integer = 0, nFilIni As Long = 8, nFilFin As Long = 0, nPFil As Long = 0
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim strRutaPlantilla As String = ""
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim Archivo As String = ""
        Dim oCells As Excel.Range = Nothing
        Dim dr As DataRow
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            strRutaPlantilla = RutaPlantillas() & "\PlantillaSucave.xls"
            With DateTime.Now
                Archivo = "PlantillaFWD_" + Usuario.ToString() + "_" + .ToString("yyyyMMdd") + "_" + .Hour.ToString + .Minute.ToString + .Second.ToString
            End With
            strRutaSIT = strRutaSIT + Archivo & ".xls"
            If File.Exists(strRutaPlantilla) = True Then
                'Exportación a Excel
                oExcel.DisplayAlerts = False
                oExcel.Visible = False
                'Start a new workbook
                oBooks = oExcel.Workbooks
                oBooks.Open(strRutaPlantilla) 'Load colorful template with chart
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                oSheet.Name = "Inventario Forward"
                oCells = oSheet.Cells
                'TITULO DE EXCEL
                'VARIABLES LOCALES
                Dim iFila As Int64 = 14

                oCells.Range("C1").Value = DateTime.Now.ToString("dd/MM/yyyy")
                oCells.Range("G2").Value = IIf(ddlFondo.SelectedValue.Equals("Todos"), "21", ddlFondo.SelectedValue.ToString)
                oCells.Range("C3").Value = ddlEstado.SelectedItem.Text
                oCells.Range("B4").Value = ddlFechas.SelectedItem.Text & " del " & tbFechaVencimientoDesde.Text & " al " & tbFechaVencimientoHasta.Text
                'VARIBLES
                Dim sClave As String = ""
                Dim sFondo As String = ""
                'LLENAR DATOS
                If dt.Rows.Count > 0 Then
                    For iRow = 0 To dt.Rows.Count - 1
                        dr = dt.Rows.Item(iRow)
                        nPFil = iRow + nFilIni
                        oCells.Range("B" & nPFil).Value = dt.Rows(iRow)("Valor")
                        oCells.Range("C" & nPFil).Value = dt.Rows(iRow)("CodEmisor")
                        oCells.Range("D" & nPFil).Value = dt.Rows(iRow)("Emisor")
                        oCells.Range("E" & nPFil).Value = dt.Rows(iRow)("CodigoMoneda")
                        oCells.Range("F" & nPFil).Value = dt.Rows(iRow)("Moneda")
                        oCells.Range("G" & nPFil).Value = dt.Rows(iRow)("CodigoRef")
                        oCells.Range("H" & nPFil).Value = dt.Rows(iRow)("HoraNegaciacion")
                        oCells.Range("I" & nPFil).Value = dt.Rows(iRow)("CodigoMonedaVenta")
                        oCells.Range("J" & nPFil).Value = dt.Rows(iRow)("MonedaVenta")
                        oCells.Range("K" & nPFil).Value = dt.Rows(iRow)("IndicadorMovimiento")
                        oCells.Range("L" & nPFil).Value = dt.Rows(iRow)("TipoForward")
                        oCells.Range("N" & nPFil).Value = dt.Rows(iRow)("MontoForward")
                        oCells.Range("O" & nPFil).Value = dt.Rows(iRow)("PrecioTransaccion")
                        oCells.Range("P" & nPFil).Value = dt.Rows(iRow)("FechaEmision")
                        oCells.Range("Q" & nPFil).Value = dt.Rows(iRow)("FechaVencimiento")
                        oCells.Range("R" & nPFil).Value = dt.Rows(iRow)("FechaLiquidacion")
                        oCells.Range("T" & nPFil).Value = dt.Rows(iRow)("Modalidad")
                        oCells.Range("V" & nPFil).Value = dt.Rows(iRow)("TipoCambioForwardPactado")
                        oCells.Range("W" & nPFil).Value = dt.Rows(iRow)("IndicadorCaja")
                        oCells.Range("X" & nPFil).Value = dt.Rows(iRow)("PlazaNegociacion")
                        oCells.Range("Y" & nPFil).Value = dt.Rows(iRow)("Descripcion")
                        oCells.Range("Z" & nPFil).Value = dt.Rows(iRow)("Fixing")
                        oCells.Range("AA" & nPFil).Value = dt.Rows(iRow)("MonedaLiquida")
                        oCells.Range("AB" & nPFil).Value = dt.Rows(iRow)("MontoCalculado")
                        oCells.Range("AC" & nPFil).Value = dt.Rows(iRow)("NumeroContrato")
                        oCells.Range("AD" & nPFil).Value = dt.Rows(iRow)("Estado")
                        oCells.Range("AE" & nPFil).Value = dt.Rows(iRow)("Mercado")
                        oCells.Range("AF" & nPFil).Value = iRow + 1
                        nFilFin = nPFil
                    Next iRow

                    If dt.Rows.Count > 1 Then
                        oCells.Range("A8").Copy()
                        oCells.Range("A9:A" + CStr(nFilFin)).Select()
                        oSheet.Paste()
                        oCells.Range("M8").Copy()
                        oCells.Range("M9:M" + CStr(nFilFin)).Select()
                        oSheet.Paste()
                        oCells.Range("S8").Copy()
                        oCells.Range("S9:S" + CStr(nFilFin)).Select()
                        oSheet.Paste()
                        oCells.Range("U8").Copy()
                        oCells.Range("U9:U" + CStr(nFilFin)).Select()
                        oSheet.Paste()
                        oSheet.Application.CutCopyMode = False

                        oCells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
                        oCells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
                        oCells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
                        oCells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
                        If nFilIni < nFilFin Then
                            oCells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous
                            oCells.Range("A" & nFilIni.ToString & ":AE" & nFilFin.ToString).Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous
                        End If
                    End If
                    'Save in a temporary file
                    oSheet.SaveAs(strRutaSIT)
                    oBook.Close()
                    Response.Clear()
                    Response.ContentType = "application/xls"
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Archivo & ".xls")
                    Response.WriteFile(strRutaSIT)
                    Response.End()
                Else
                    AlertaJS("No se encontraron datos con los filtros ingresados.")
                End If
            Else
                AlertaJS("No se encontro el archivo en la ruta: " + strRutaPlantilla)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
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

    Private Sub ibImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibImprimir.Click
        Dim strCodigoFondos As String = ""
        Dim strMonedaNeg As String = ""
        Dim strMonedaDes As String = ""
        Dim decFechaIni As Decimal = 0
        Dim decFechaFin As Decimal = 0
        Try
            If Trim(tbFechaVencimientoDesde.Text) = "" Then
                AlertaJS("Debe ingresar la fecha de inicio...!")
                Exit Sub
            End If
            If Trim(tbFechaVencimientoHasta.Text) = "" Then
                AlertaJS("Debe ingresar la fecha de fin...!")
                Exit Sub
            End If

            decFechaIni = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVencimientoDesde.Text)
            decFechaFin = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVencimientoHasta.Text)
            If decFechaIni <= decFechaFin Then
                Dim sFondo As String = IIf(ddlFondo.SelectedValue.ToString = "Todos", "", ddlFondo.SelectedValue.ToString)
                Dim sMonedaNeg As String = IIf(ddlMonedaNegociada.SelectedValue.ToString = "Todos", "", ddlMonedaNegociada.SelectedValue.ToString)
                Dim sMonedaDes As String = IIf(ddlMonedaDestino.SelectedValue.ToString = "Todos", "", ddlMonedaDestino.SelectedValue.ToString)
                Dim dEstado As Decimal = IIf(ddlEstado.SelectedValue.ToString = "Todos", 0, ddlEstado.SelectedValue.ToString)
                Dim sMercado As String = IIf(ddlMercado.SelectedValue.ToString = "Todos", "", ddlMercado.SelectedValue.ToString)

                Dim strurl As String = "Reportes/frmVisorInventarioForward.aspx?cFondo=" & ddlFondo.SelectedItem.Text & "&vFondo=" & sFondo & "&vfecini=" & UIUtility.ConvertirFechaaDecimal(tbFechaVencimientoDesde.Text) & "&vfecfin=" & UIUtility.ConvertirFechaaDecimal(tbFechaVencimientoHasta.Text) & "&vmon=" & sMonedaNeg & "&vmond=" & sMonedaDes & "&vest=" & dEstado.ToString & "&vfecs=" & ddlFechas.SelectedValue & "&vestd=" & ddlEstado.SelectedItem.Text & "&vfecsd=" & ddlFechas.SelectedItem.Text & "&vMerca=" & sMercado
                EjecutarJS("showWindow('" & strurl & "', '800', '600');")

                'EjecutarJS(UIUtility.MostrarPopUp("Reportes/frmVisorInventarioForward.aspx?cFondo=" & ddlFondo.SelectedItem.Text & "&vFondo=" & sFondo & "&vfecini=" & UIUtility.ConvertirFechaaDecimal(tbFechaVencimientoDesde.Text) & "&vfecfin=" & UIUtility.ConvertirFechaaDecimal(tbFechaVencimientoHasta.Text) & "&vmon=" & sMonedaNeg & "&vmond=" & sMonedaDes & "&vest=" & dEstado.ToString & "&vfecs=" & ddlFechas.SelectedValue & "&vestd=" & ddlEstado.SelectedItem.Text & "&vfecsd=" & ddlFechas.SelectedItem.Text & "&vMerca=" & sMercado, "no", 1010, 670, 0, 0, "no", "yes", "yes", "yes"), False)
            Else
                AlertaJS("La fecha de inicio no puede ser mayor que la fecha de fin...!")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        Finally
            GC.Collect()
        End Try
    End Sub
    Private Sub ibSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibSalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub
    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        Try
            dgLista.DataSource = Nothing
            dgLista.DataBind()
            LimpiarControles()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim decFechaIni As Decimal = 0
        Dim decFechaFin As Decimal = 0
        Dim ds As DataSet
        Dim dt As DataTable
        Try
            If Trim(tbFechaVencimientoDesde.Text) = "" Then
                AlertaJS("Debe ingresar la fecha de inicio...!")
                Exit Sub
            End If
            If Trim(tbFechaVencimientoHasta.Text) = "" Then
                AlertaJS("Debe ingresar la fecha de fin...!")
                Exit Sub
            End If

            decFechaIni = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVencimientoDesde.Text)
            decFechaFin = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVencimientoHasta.Text)
            If decFechaIni <= decFechaFin Then
                Dim sFondo As String = IIf(ddlFondo.SelectedValue.ToString = "Todos", "", ddlFondo.SelectedValue.ToString)
                Dim sMonedaNeg As String = IIf(ddlMonedaNegociada.SelectedValue.ToString = "Todos", "", ddlMonedaNegociada.SelectedValue.ToString)
                Dim sMonedaDes As String = IIf(ddlMonedaDestino.SelectedValue.ToString = "Todos", "", ddlMonedaDestino.SelectedValue.ToString)
                Dim dEstado As Decimal = IIf(ddlEstado.SelectedValue.ToString = "Todos", 0, ddlEstado.SelectedValue.ToString)
                Dim sMercado As String = IIf(ddlMercado.SelectedValue.ToString = "Todos", "", ddlMercado.SelectedValue.ToString)
                ds = New OrdenPreOrdenInversionBM().InventarioForward(sFondo, Now.ToString("yyyyMMdd"), decFechaIni, UIUtility.ConvertirFechaaDecimal(tbFechaVencimientoHasta.Text), sMonedaNeg, sMonedaDes, dEstado, ddlFechas.SelectedValue, sMercado, DatosRequest)
                dt = ds.Tables(0)
                ExportarExcelFwd(dt)
            Else
                AlertaJS("La fecha de inicio no puede ser mayor que la fecha de fin...!")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        Finally
            GC.Collect()
        End Try
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            If tbFechaVencimientoDesde.Text.Trim.Equals("") Then
                AlertaJS("Debe ingresar una fecha inicial.")
                Exit Sub
            End If
            If tbFechaVencimientoHasta.Text.Trim.Equals("") Then
                AlertaJS("Debe ingresar una fecha final.")
                Exit Sub
            End If
            dgLista.PageIndex = 0
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
End Class

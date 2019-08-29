Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data
Imports System.Runtime.InteropServices.Marshal
Imports System.IO

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaPatrimonioValor
    Inherits BasePage

    Private CodigoTipoInstrumento As String
    Private CodigoMnemonico As String
    Private FechaVigencia As String
    Private FechaInicio As Integer
    Private FechaFin As Integer

#Region " /* Metodos de Pagina */ "

    'Public Overrides Sub VerifyRenderingInServerForm(control As Control)
    '    ' Confirms that an HtmlForm control is rendered for the specified ASP.NET
    '    '     server control at run time. 
    'End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarFecha()
                CargarCombos()
                CargarTipoFiltroCarga(rbTipoBusqueda.SelectedValue)
                CargarGrilla()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Select Case hdTipoModal.Value
                    Case "INS"
                        txtTipoInstrumento.Text = CType(Session("SS_DatosModal"), String())(0).ToString()

                    Case "MNE"
                        txtCodigoNemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()

                    Case "EMI"
                        tbEmisor.Text = CType(Session("SS_DatosModal"), String())(0)
                        tbEmisorDesc.Text = CType(Session("SS_DatosModal"), String())(1)
                End Select
                'Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página: " & Replace(ex.Message, "'", ""))
        Finally
            If Session("SS_DatosModal") IsNot Nothing Then
                Session.Remove("SS_DatosModal")
            End If
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            If rbTipoBusqueda.SelectedValue = "Emision" Then
                Response.Redirect("frmPatrimonioValor.aspx?ope=reg")
            Else
                Response.Redirect("frmPatrimonioEmisor.aspx?Operacion=Ingresar")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub

    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            ExportarExcel()
            'Dim strNombreReporte As String
            'strNombreReporte = "PatrimonioValor_" & Date.Now.ToShortDateString().Replace("/", "_") & ".xls"
            'Response.AddHeader("Content-Disposition", "attachment; filename= " & strNombreReporte)
            'Response.ContentType = "application/vnd.ms-excel"
            'Response.Charset = ""
            'Me.EnableViewState = False
            'Dim tw As New System.IO.StringWriter
            'Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            'dgExportar.Visible = True
            'Me.dgExportar.RenderControl(hw)
            'Response.Write(tw.ToString())
            'Response.End()
            'dgExportar.Visible = False
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Exportar: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Sub btnImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Response.Redirect("frmPatrimonioValorImportar.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Importar")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
                If (e.Row.Cells(6).Text = "A") Then
                    e.Row.Cells(6).Text = "Activo"
                ElseIf (e.Row.Cells(6).Text = "I") Then
                    e.Row.Cells(6).Text = "Inactivo"
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim index As Integer = 0
            If e.CommandName = "Modificar" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                CodigoTipoInstrumento = DirectCast(gvr.FindControl("hdCodigoTipoInstrumento"), HiddenField).Value
                CodigoMnemonico = DirectCast(gvr.FindControl("hdCodigoMnemonico"), HiddenField).Value
                FechaVigencia = IIf(gvr.Cells(5).Text = "&nbsp;", "", UIUtility.ConvertirFechaaDecimal(gvr.Cells(5).Text))
                Response.Redirect("frmPatrimonioValor.aspx?ope=mod&codTI=" + CodigoTipoInstrumento + "&codMne=" + CodigoMnemonico + "&fecVig=" + FechaVigencia)
            End If
            If e.CommandName = "Eliminar" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                CodigoTipoInstrumento = DirectCast(gvr.FindControl("hdCodigoTipoInstrumento"), HiddenField).Value
                CodigoMnemonico = DirectCast(gvr.FindControl("hdCodigoMnemonico"), HiddenField).Value
                FechaVigencia = IIf(gvr.Cells(5).Text = "&nbsp;", "", UIUtility.ConvertirFechaaDecimal(gvr.Cells(5).Text))
                Dim oPatrimonioValorBM As New PatrimonioValorBM
                oPatrimonioValorBM.Eliminar(CodigoTipoInstrumento, CodigoMnemonico, FechaVigencia, Me.DatosRequest)
                CargarGrilla()
            End If
            'If (e.CommandName = "Eliminar" Or e.CommandName = "Modificar") Then
            '    Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            '    Dim Index As Integer = Row.RowIndex
            '    CodigoTipoInstrumento = DirectCast(Row.FindControl("hdCodigoTipoInstrumento"), HiddenField).Value
            '    'CodigoTipoInstrumento = IIf(Row.Cells(7).Text = "&nbsp;", "", Row.Cells(7).Text)
            '    CodigoMnemonico = DirectCast(Row.FindControl("hdCodigoMnemonico"), HiddenField).Value
            '    'CodigoMnemonico = IIf(Row.Cells(8).Text = "&nbsp;", "", Row.Cells(8).Text)
            '    FechaVigencia = IIf(Row.Cells(5).Text = "&nbsp;", "", UIUtility.ConvertirFechaaDecimal(Row.Cells(5).Text))  'HDG OT 61045 20101021
            'End If

            'Select Case e.CommandName
            '    Case "Eliminar"
            '        Try
            '            Dim oPatrimonioValorBM As New PatrimonioValorBM
            '            Dim codigo As String = e.CommandArgument.ToString()
            '            'oPatrimonioValorBM.Eliminar(CodigoTipoInstrumento, CodigoMnemonico, Me.DatosRequest)    'HDG OT 61045 20101021
            '            oPatrimonioValorBM.Eliminar(CodigoTipoInstrumento, CodigoMnemonico, FechaVigencia, Me.DatosRequest)    'HDG OT 61045 20101021
            '            CargarGrilla()
            '        Catch ex As Exception
            '            'Las excepciones deben ser enviadas a la clase base con el método AlertaJS,esta clase se encarga de mostrar los mensajes correspondientes
            '            AlertaJS(ex.ToString)
            '        End Try
            '    Case "Modificar"
            '        Response.Redirect("frmPatrimonioValor.aspx?ope=mod&codTI=" + CodigoTipoInstrumento + "&codMne=" + CodigoMnemonico + "&fecVig=" + FechaVigencia)    'HDG OT 61045 20101021
            'End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub gvListaEmisor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvListaEmisor.RowCommand
        Try
            Dim index As Integer = 0
            If e.CommandName = "Modificar" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim id As Integer = 0
                Dim fecha As String = String.Empty
                id = Integer.Parse(gvr.Cells(2).Text.Trim.Replace("&nbsp;", ""))
                fecha = gvr.Cells(10).Text.Trim.Replace("&nbsp;", "")
                Response.Redirect("frmPatrimonioEmisor.aspx?Operacion=Modificar&Id=" & ID.ToString() & "&Fecha=" & fecha)
            End If
            If e.CommandName = "Eliminar" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim id As Integer = 0
                Dim fecha As String = String.Empty
                id = Integer.Parse(gvr.Cells(2).Text.Trim.Replace("&nbsp;", ""))
                fecha = gvr.Cells(10).Text.Trim.Replace("&nbsp;", "")
                Dim oPatrimonioValorBM As New PatrimonioValorBM
                oPatrimonioValorBM.EliminarPatrimonioEmisor(id)
                CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
            'Me.dgLista.DataSource = CType(Session("dtPatrimonioValor"), DataTable)
            'Me.dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub gvListaEmisor_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvListaEmisor.PageIndexChanging
        Try
            gvListaEmisor.PageIndex = e.NewPageIndex
            CargarGrilla()
            'Me.dgLista.DataSource = CType(Session("dtPatrimonioValor"), DataTable)
            'Me.dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub rbTipoBusqueda_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbTipoBusqueda.SelectedIndexChanged
        Try
            CargarTipoFiltroCarga(rbTipoBusqueda.SelectedValue)
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub Buscar()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
        'EjecutarJS("$('#" + lbContador.ClientID + "').text('Registros Encontrados: " + Me.dgLista.Rows.Count.ToString + "')")
        If rbTipoBusqueda.SelectedValue = "Emision" Then
            If Me.dgLista.Rows.Count = 0 Then
                AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
            End If
        ElseIf rbTipoBusqueda.SelectedValue = "Emisor" Then
            If Me.gvListaEmisor.Rows.Count = 0 Then
                AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
            End If
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oPatrimonioValorBM As New PatrimonioValorBM
        Dim dt As DataTable
        Dim situacion As String

        If rbTipoBusqueda.SelectedValue = "Emision" Then
            gvListaEmisor.Visible = False
            dgLista.Visible = True

            CodigoTipoInstrumento = txtTipoInstrumento.Text
            CodigoMnemonico = txtCodigoNemonico.Text
            situacion = ""

            dt = New PatrimonioValorBM().SeleccionarPorFiltro(CodigoTipoInstrumento, CodigoMnemonico, "A", UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text)).Tables(0)
            EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
            Me.dgLista.DataSource = dt
            Me.dgLista.DataBind()
            'dgExportar.DataSource = dt
            'dgExportar.DataBind()
        Else
            dgLista.Visible = False
            gvListaEmisor.Visible = True
            dt = New PatrimonioValorBM().SeleccionarPatrimonioEmisor(0, "", tbEmisor.Text, ddlTipo.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text))
            EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
            gvListaEmisor.DataSource = dt
            gvListaEmisor.DataBind()
        End If
    End Sub

    Private Sub CargarCombos()
        CargarComboTipoValor()
        'Dim tablaSituacion As New DataTable
        'Dim oParametrosGenerales As New ParametrosGeneralesBM
        'tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        ''HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    'Private Sub LimpiarConsulta()
    '    'Me.tbCodigoTipoInstrumento.Text = ""
    '    'Me.tbCodigoMnemonico.Text = ""
    '    'Me.ddlSituacion.SelectedIndex = 0
    'End Sub

    Private Sub CargarTipoFiltroCarga(ByVal p_TipoBusqueda As String)
        Select Case p_TipoBusqueda
            Case "Emision"
                dgLista.Visible = True
                gvListaEmisor.Visible = False
                divTipoInstrumento.Attributes.Add("style", "display:block")
                divNemonico.Attributes.Add("style", "display:block")

                divEmisor.Attributes.Add("style", "display:none")
                divTipo.Attributes.Add("style", "display:none")
                CargarGrilla()
            Case "Emisor"
                dgLista.Visible = False
                gvListaEmisor.Visible = True
                divTipoInstrumento.Attributes.Add("style", "display:none")
                divNemonico.Attributes.Add("style", "display:none")

                divEmisor.Attributes.Add("style", "display:block")
                divTipo.Attributes.Add("style", "display:block")
                CargarGrilla()
        End Select
    End Sub

    Private Sub CargarFecha()
        Me.txtFechaInicio.Text = FechaActual
        Me.txtFechaFin.Text = FechaActual
    End Sub

    Private Sub CargarComboTipoValor()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dt As DataTable = Nothing
        dt = oParametrosGenerales.SeleccionarPorFiltro("PATEMI_TIPVAL", "", "", "", DatosRequest)
        HelpCombo.LlenarComboBox(ddlTipo, dt, "Valor", "Nombre", True)
    End Sub

    Private Sub ExportarExcel()
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
            Dim dtPatVal As DataTable = Nothing
            Dim dtPatEmi As DataTable = Nothing
            Dim objReporte As New PatrimonioValorBM
            Dim swImprimir As Boolean = False

            Dim sFile As String, sTemplate As String
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RptPatVal_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaExportarValorYEmisor.xlsx"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells
            oSheet.SaveAs(sFile)

            dtPatVal = objReporte.SeleccionarPorFiltro(txtTipoInstrumento.Text, txtCodigoNemonico.Text, "A", UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text)).Tables(0)
            dtPatEmi = objReporte.SeleccionarPatrimonioEmisor(0, "", tbEmisor.Text, ddlTipo.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text))

            If dtPatVal IsNot Nothing Then
                If dtPatVal.Rows.Count > 0 Then
                    FillPatrimonioValor(dtPatVal, oSheet, oCells)
                    oBook.Save()
                    swImprimir = True
                End If
            End If

            oSheet = CType(oSheets.Item(2), Excel.Worksheet)
            oCells = oSheet.Cells

            If dtPatEmi IsNot Nothing Then
                If dtPatEmi.Rows.Count > 0 Then
                    FillPatrimonioEmisor(dtPatEmi, oSheet, oCells)
                    oBook.Save()
                    swImprimir = True
                End If
            End If

            If swImprimir = False Then
                Throw New System.Exception("No existe información para generar el reporte")
            End If

            'oExcel.Cells.EntireColumn.AutoFit()
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "RptPatVal_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
            Response.WriteFile(sFile)
            Response.End()
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
    End Sub

    Private Sub FillPatrimonioValor(ByVal dt As DataTable, ByRef oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        Dim indice As Integer = 2
        For Each dr As DataRow In dt.Rows
            oCells(indice, 1) = dr("DescripcionTipoInstrumento")
            oCells(indice, 2) = dr("CodigoMnemonico")
            oCells(indice, 3) = Decimal.Parse(dr("Patrimonio"))
            oCells(indice, 4) = UIUtility.ConvertirStringaFecha(dr("strFechaVigencia"))
            indice += 1
        Next
    End Sub

    Private Sub FillPatrimonioEmisor(ByVal dt As DataTable, ByRef oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        Dim indice As Integer = 2
        For Each dr As DataRow In dt.Rows
            oCells(indice, 1) = dr("CodigoEntidad")
            oCells(indice, 2) = dr("DescripcionEmisor")
            oCells(indice, 3) = dr("TipoValor")
            oCells(indice, 4) = Decimal.Parse(dr("Valor"))
            oCells(indice, 5) = UIUtility.ConvertirStringaFecha(dr("FechaString"))
            indice += 1
        Next
    End Sub

#End Region

End Class

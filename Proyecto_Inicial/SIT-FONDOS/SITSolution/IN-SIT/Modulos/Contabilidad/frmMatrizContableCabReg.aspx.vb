Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Text
Partial Class Modulos_Contabilidad_frmMatrizContableCabReg
    Inherits BasePage
    'Private Const DET_MATRIZCON As String = "Detalle_MatrizContable"
    Dim oCabeceraMatrizContableBM As New CabeceraMatrizContableBM
    Dim oDetalleMatrizContableBM As New DetalleMatrizContableBM
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
            Response.Redirect("frmMatrizContable.aspx?retornar=" + If(hd.Value <> String.Empty, "1", "0"))
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmMatrizContable.aspx?retornar=" + If(hd.Value <> String.Empty, "1", "0"))
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    'OT10783 - Refactorización de código
    'Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
    '    Try
    '        Dim dt As DataTable
    '        Dim oRow As DataRow
    '        Dim sNumerocuenta = e.CommandArgument
    '        dt = CType(Session(DET_MATRIZCON), DataTable)
    '        For Each oRow In dt.Rows
    '            If (oRow("NumeroCuentaContable") = sNumerocuenta) Then
    '                tbNumeroCuentaContable.Text = oRow("NumeroCuentaContable")
    '                SeleccionarOpcionCombo(ddlDebeHaber, oRow("DebeHaber"))
    '                Session("NumeroCuentaMod") = sNumerocuenta

    '            End If
    '        Next
    '    Catch ex As Exception
    '        AlertaJS("Ocurri&oacute; un error al realizar una operaci&oacute;n en la Grilla")
    '    End Try
    'End Sub
    'Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
    '    Try
    '        Dim sNumeroCuenta = e.CommandArgument
    '        Dim dt, dtAux As DataTable
    '        Dim oRow As DataRow
    '        dt = CType(Session(DET_MATRIZCON), DataTable)
    '        dtAux = dt.Clone()
    '        For Each oRow In dt.Rows
    '            If Not (oRow("NumeroCuentaContable") = sNumeroCuenta) Then
    '                dtAux.Rows.Add(-1, oRow("NumeroCuentaContable"), oRow("DebeHaber"), "", _
    '                                oRow("Glosa"), "", "", oRow("Aplicar"))
    '            End If
    '        Next
    '        Session(DET_MATRIZCON) = dtAux
    '        dgLista.DataSource = dtAux
    '        dgLista.DataBind()
    '    Catch ex As Exception
    '        AlertaJS("Ocurri&oacute; un error al realizar una operaci&oacute;n en la Grilla")
    '    End Try
    'End Sub
    'OT10783 - Fin
    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        'OT10783 - Refactorización de código
        Try
            If btnAgregar.Text = "Modificar" Then
                If hfIndexRow.Value <> String.Empty Then
                    Dim index As Integer = Integer.Parse(hfIndexRow.Value)
                    Dim oDetalleMatrizContableBE As New DetalleMatrizContableBE
                    dgLista.Rows(index).Cells(3).Text = tbNumeroCuentaContable.Text.Trim
                    dgLista.Rows(index).Cells(4).Text = ddlDebeHaber.SelectedValue
                    dgLista.Rows(index).Cells(6).Text = ddlTipoCuenta.SelectedValue
                    LimpiarControlesDetalle()
                End If
                btnAgregar.Text = "Agregar"
            Else
                Dim dt As DataTable
                Dim sGlosa As String = ""
                dt = ViewState("DET_MATRIZCON")
                Select Case ddlMatriz.SelectedValue
                    Case "1"
                        sGlosa = "RENTABILIDAD"
                    Case "2"
                        sGlosa = "COMPRA VENT INVERSIONES"
                End Select
                dt.Rows.Add(IIf(tbCodigo.Text.Trim <> String.Empty, tbCodigo.Text, "0"), tbNumeroCuentaContable.Text, _
                            ddlDebeHaber.SelectedValue, String.Empty, sGlosa, String.Empty, String.Empty, _
                            ddlTipoCuenta.SelectedValue)
                ViewState("DET_MATRIZCON") = dt
                dgLista.DataSource = dt
                dgLista.DataBind()
                LimpiarControlesDetalle()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
        'Dim dt As DataTable
        'Dim oRow As DataRow
        'Try
        '    dt = CType(Session(DET_MATRIZCON), DataTable)
        '    If dt Is Nothing Then
        '        InicializarCabecera()
        '        dt = New DataTable()
        '        dt.Columns.Add(New DataColumn("CodigoCabeceraMatriz", GetType(String)))
        '        dt.Columns.Add(New DataColumn("NumeroCuentaContable", GetType(String)))
        '        dt.Columns.Add(New DataColumn("DebeHaber", GetType(String)))
        '        dt.Columns.Add(New DataColumn("TipoContabilidad", GetType(String)))
        '        dt.Columns.Add(New DataColumn("Glosa", GetType(String)))
        '        dt.Columns.Add(New DataColumn("CodigoTercero", GetType(String)))
        '        dt.Columns.Add(New DataColumn("Tercero", GetType(String)))
        '        dt.Columns.Add(New DataColumn("Aplicar", GetType(String)))
        '        dt.Columns.Add(New DataColumn("Secuencia", GetType(String)))
        '    End If
        '    Dim sGlosa, sAplicar As String
        '    Select Case ddlMatriz.SelectedValue
        '        Case "1"
        '            sGlosa = "RENTABILIDAD"
        '            sAplicar = "ValorCausado Local"
        '        Case "2"
        '            sGlosa = "COMPRA VENT INVERSIONES"
        '            sAplicar = "Importe"
        '    End Select
        '    If Not Session("NumeroCuentaMod") Is Nothing Then
        '        For Each oRow In dt.Rows
        '            If (oRow("NumeroCuentaContable") = Session("NumeroCuentaMod")) Then
        '                oRow("NumeroCuentaContable") = tbNumeroCuentaContable.Text
        '                oRow("Aplicar") = sAplicar
        '                oRow("Glosa") = sGlosa
        '                oRow("DebeHaber") = ddlDebeHaber.SelectedValue
        '                Session("NumeroCuentaMod") = Nothing
        '            End If
        '        Next
        '    Else
        '        dt.Rows.Add(-1, tbNumeroCuentaContable.Text, ddlDebeHaber.SelectedValue, _
        '                "", sGlosa, "", "", sAplicar, "")
        '    End If
        '    Session(DET_MATRIZCON) = dt
        '    dgLista.DataSource = dt
        '    dgLista.DataBind()
        '    LimpiarControlesDetalle()
        'Catch ex As Exception
        '    AlertaJS("Ocurrió un error al agregar el registro")
        'End Try
        'OT10783 - Fin
    End Sub
    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim index As Integer = 0
            If e.CommandName = "Modificar" Then
                btnAgregar.Text = "Modificar"
                index = Integer.Parse(e.CommandArgument.ToString)
                tbNumeroCuentaContable.Text = Replace(dgLista.Rows(index).Cells(3).Text.Trim, "&nbsp;", "")
                ddlDebeHaber.SelectedValue = Replace(dgLista.Rows(index).Cells(4).Text.Trim, "&nbsp;", "")
                ddlTipoCuenta.SelectedValue = Replace(dgLista.Rows(index).Cells(6).Text.Trim, "&nbsp;", "")
                hfIndexRow.Value = index.ToString
            ElseIf e.CommandName = "Eliminar" Then
                index = Integer.Parse(e.CommandArgument.ToString)
                Dim dt As DataTable
                dt = ViewState("DET_MATRIZCON")
                dt.Rows(index).Delete()
                ViewState("DET_MATRIZCON") = dt
                dgLista.DataSource = dt
                dgLista.DataBind()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
#End Region

#Region "Metodos Privados"
    Private Sub CargarCombos()
        'Portafolio
        Dim tblPortafolio As New Data.DataTable
        Dim oPortafolio As New PortafolioBM
        tblPortafolio = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlFondo, tblPortafolio, "CodigoPortafolio", "Descripcion", True)
        'Operacion
        Dim tblOperacion As New Data.DataTable
        Dim oOperacion As New OperacionBM
        tblOperacion = oOperacion.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlOperacion, tblOperacion, "CodigoOperacion", "Descripcion", True)
        'TipoInstrumento
        Dim tblTipoInstrumento As New Data.DataTable
        Dim oTipoInstrumento As New TipoInstrumentoBM
        tblTipoInstrumento = oTipoInstrumento.Listar(DatosRequest).Tables(0)
        tblTipoInstrumento.Rows.Add("DIVISA", "DIVISA", "DIVISAS", "3", "", 0, "", "", "", "", "", 0, "", "", "", "", "DIVISA - DIVISA")
        HelpCombo.LlenarComboBox(Me.ddlTipoInstrumento, tblTipoInstrumento, "CodigoTipoInstrumento", "CodigoMasDescripcion", True)
        'Moneda
        Dim tblMoneda As New Data.DataTable
        Dim oMoneda As New MonedaBM
        tblMoneda = oMoneda.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlMoneda, tblMoneda, "CodigoMoneda", "Descripcion", True)
        'Matriz
        Dim oMatrizContableBM As New MatrizContableBM
        Dim dtblDatos As DataTable = oMatrizContableBM.SeleccionarPorFiltros("", "A", "0").Tables(0)
        HelpCombo.LlenarComboBox(ddlMatriz, dtblDatos, "CodigoMatrizContable", "Descripcion", True)
    End Sub
    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            'OT10783 - Se eliminaron las sesiones para no recargar la memoria del servidor
            'Session(DET_MATRIZCON) = Nothing
            If Not Request.QueryString("cod") Is Nothing Then
                tbCodigo.Enabled = False
                Me.hd.Value = Request.QueryString("cod")
                CargarRegistro(hd.Value)
                CargarGrilla(hd.Value)
            Else
                CargarGrilla("-1")
                tbCodigo.Enabled = True
                Me.hd.Value = String.Empty
                'Session("NumeroCuentaMod") = Nothing OT10783
            End If
        End If
    End Sub
    Private Sub CargarRegistro(ByVal codigoCabecera As String)
        Dim oRow As CabeceraMatrizContableBE.CabeceraMatrizContableRow
        Dim oCabeceraMatrizContableBE As New CabeceraMatrizContableBE
        oCabeceraMatrizContableBE = oCabeceraMatrizContableBM.Seleccionar(codigoCabecera, DatosRequest)
        oRow = DirectCast(oCabeceraMatrizContableBE.CabeceraMatrizContable.Rows(0), CabeceraMatrizContableBE.CabeceraMatrizContableRow)
        SeleccionarOpcionCombo(Me.ddlFondo, oRow.CodigoPortafolioSBS)
        SeleccionarOpcionCombo(Me.ddlMatriz, oRow.CodigoMatrizContable)
        SeleccionarOpcionCombo(Me.ddlTipoInstrumento, oRow.CodigoTipoInstrumento)
        SeleccionarOpcionCombo(Me.ddlOperacion, oRow.CodigoOperacion)
        SeleccionarOpcionCombo(Me.ddlMoneda, oRow.CodigoMoneda)
        SeleccionarOpcionCombo(Me.ddlSituacion, oRow.Situacion)
        tbCodigo.Text = oRow.CodigoCabeceraMatriz
    End Sub
    Private Sub CargarGrilla(ByVal sCodigoCabeceraContable As String)
        'OT10783 - Se eliminaron las sesiones para no recargar la memoria del servidor
        'Session(DET_MATRIZCON) = oDetalleMatrizContableBM.Seleccionar(sCodigoCabeceraContable, DatosRequest).DetalleMatrizContable
        'dgLista.DataSource = CType(Session(DET_MATRIZCON), DataTable)
        'dgLista.DataBind()
        'OT10783 - Fin
        Dim dt As DataTable = oDetalleMatrizContableBM.Seleccionar(sCodigoCabeceraContable, DatosRequest).DetalleMatrizContable
        ViewState("DET_MATRIZCON") = dt
        dgLista.DataSource = dt
        dgLista.DataBind()
    End Sub
    Private Function CrearObjetoCabecera() As CabeceraMatrizContableBE
        Dim oCabeceraMatrizContableBE As New CabeceraMatrizContableBE
        Dim oRow As CabeceraMatrizContableBE.CabeceraMatrizContableRow
        oRow = DirectCast(oCabeceraMatrizContableBE.CabeceraMatrizContable.NewRow(), CabeceraMatrizContableBE.CabeceraMatrizContableRow)
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue()
        oRow.CodigoOperacion = ddlOperacion.SelectedValue()
        oRow.CodigoClaseInstrumento = String.Empty
        oRow.CodigoModalidadPago = String.Empty
        oRow.CodigoTipoInstrumento = ddlTipoInstrumento.SelectedValue()
        oRow.CodigoSectorEmpresarial = String.Empty
        oRow.CodigoMoneda = ddlMoneda.SelectedValue
        oRow.CodigoMatrizContable = ddlMatriz.SelectedValue()
        oRow.Situacion = ddlSituacion.SelectedValue()
        oRow.NumeroCuentaIngreso = String.Empty
        oRow.CodigoSBSBanco = String.Empty
        If Not Me.hd.Value.Equals(String.Empty) Then
            oRow.CodigoCabeceraMatriz = hd.Value
        Else
            oRow.CodigoCabeceraMatriz = "0"
        End If
        oRow.CodigoNegocio = ""
        oRow.CodigoSerie = ""
        oCabeceraMatrizContableBE.CabeceraMatrizContable.AddCabeceraMatrizContableRow(oRow)
        oCabeceraMatrizContableBE.CabeceraMatrizContable.AcceptChanges()
        Return oCabeceraMatrizContableBE
    End Function
    Private Function CrearObjetoDetalle(ByVal sCodigoCabeceraMatriz As String) As DetalleMatrizContableBE
        Dim oDetalleMatrizContableBE As New DetalleMatrizContableBE
        If dgLista.Rows.Count > 0 Then
            Dim oDetalleMatrizContableRow As DetalleMatrizContableBE.DetalleMatrizContableRow
            Dim oRow As GridViewRow
            For Each oRow In dgLista.Rows
                oDetalleMatrizContableRow = DirectCast(oDetalleMatrizContableBE.DetalleMatrizContable.NewDetalleMatrizContableRow, DetalleMatrizContableBE.DetalleMatrizContableRow)
                oDetalleMatrizContableRow.CodigoCabeceraMatriz = sCodigoCabeceraMatriz
                oDetalleMatrizContableRow.Aplicar = oRow.Cells(6).Text.Trim.Replace("&nbsp;", "")
                oDetalleMatrizContableRow.DebeHaber = oRow.Cells(4).Text.Trim.Replace("&nbsp;", "")
                oDetalleMatrizContableRow.Glosa = oRow.Cells(5).Text.Trim.Replace("&nbsp;", "")
                oDetalleMatrizContableRow.NumeroCuentaContable = oRow.Cells(3).Text.Trim.Replace("&nbsp;", "")
                oDetalleMatrizContableRow.Secuencia = 0
                oDetalleMatrizContableRow.TipoContabilidad = String.Empty
                oDetalleMatrizContableRow.CodigoTercero = String.Empty
                oDetalleMatrizContableRow.IndicaNroDocumento = String.Empty
                oDetalleMatrizContableRow.CodigoCentroCosto = String.Empty
                oDetalleMatrizContableBE.DetalleMatrizContable.AddDetalleMatrizContableRow(oDetalleMatrizContableRow)
                oDetalleMatrizContableBE.AcceptChanges()
            Next
        End If
        Return oDetalleMatrizContableBE
    End Function
    Private Sub Aceptar()
        If Me.hd.Value.Equals(String.Empty) Then
            Insertar()
        Else
            Modificar()
        End If
    End Sub
    Private Sub Insertar()
        Dim oCabeceraMatrizContableBE As New CabeceraMatrizContableBE
        Dim oDetalleMatrizContableBE As New DetalleMatrizContableBE
        Dim sCodigoCabeceraMatriz As String = "0"
        oCabeceraMatrizContableBE = CrearObjetoCabecera()
        sCodigoCabeceraMatriz = oCabeceraMatrizContableBM.Insertar_1(oCabeceraMatrizContableBE, DatosRequest)
        oDetalleMatrizContableBE = CrearObjetoDetalle(sCodigoCabeceraMatriz)
        oDetalleMatrizContableBM.Insertar(oDetalleMatrizContableBE, ddlFondo.SelectedValue, DatosRequest)
    End Sub
    Private Sub Modificar()
        Dim oCabeceraMatrizContableBE As New CabeceraMatrizContableBE
        Dim oDetalleMatrizContableBE As New DetalleMatrizContableBE
        Dim esEliminado As Boolean = False
        oCabeceraMatrizContableBE = CrearObjetoCabecera()
        oCabeceraMatrizContableBM.Modificar(oCabeceraMatrizContableBE, DatosRequest)
        esEliminado = oDetalleMatrizContableBM.Eliminar(Me.hd.Value.Trim())
        If (esEliminado) Then
            oDetalleMatrizContableBE = CrearObjetoDetalle(Me.hd.Value.Trim())
            oDetalleMatrizContableBM.Insertar(oDetalleMatrizContableBE, ddlFondo.SelectedValue, DatosRequest)
        End If
    End Sub
    'OT10783 - Refactorización de código
    'Private Sub InicializarCabecera()
    '    Dim dt As DataTable
    '    dt = New DataTable
    '    dt.Columns.Add(New DataColumn("CodigoCabeceraMatriz", GetType(String)))
    '    dt.Columns.Add(New DataColumn("NumeroCuentaContable", GetType(String)))
    '    dt.Columns.Add(New DataColumn("DebeHaber", GetType(String)))
    '    dt.Columns.Add(New DataColumn("Glosa", GetType(String)))
    '    dt.Columns.Add(New DataColumn("Aplicar", GetType(String)))
    '    dt.Columns.Add(New DataColumn("Secuencia", GetType(String)))
    '    dt.GetChanges()
    '    Me.dgLista.DataSource = dt
    '    Me.dgLista.DataBind()
    'End Sub
    Private Sub LimpiarControlesDetalle()
        tbNumeroCuentaContable.Text = String.Empty
        ddlDebeHaber.SelectedValue = String.Empty
        ddlTipoCuenta.SelectedValue = String.Empty
        hfIndexRow.Value = String.Empty
    End Sub
    Private Sub SeleccionarOpcionCombo(ByVal combo As Control, ByVal valorSeleccionar As Object)
        Select Case combo.GetType().ToString()
            Case "System.Web.UI.WebControls.DropDownList"
                Dim ddlCombo As DropDownList = (CType(combo, DropDownList))
                If Not ddlCombo.Items.FindByValue(Convert.ToString(valorSeleccionar)) Is Nothing Then
                    Dim indice As Integer = ddlCombo.Items.IndexOf(ddlCombo.Items.FindByValue(Convert.ToString(valorSeleccionar)))
                    ddlCombo.SelectedIndex = indice
                End If
                Exit Sub
            Case "System.Web.UI.HtmlControls.HtmlSelect"
                Dim htmlCombo As HtmlSelect = (CType(combo, HtmlSelect))
                If Not htmlCombo.Items.FindByValue(Convert.ToString(valorSeleccionar)) Is Nothing Then
                    Dim indice As Integer = htmlCombo.Items.IndexOf(htmlCombo.Items.FindByValue(Convert.ToString(valorSeleccionar)))
                    htmlCombo.SelectedIndex = indice
                End If
                Exit Sub
            Case Else
                Return
        End Select
    End Sub
#End Region
End Class
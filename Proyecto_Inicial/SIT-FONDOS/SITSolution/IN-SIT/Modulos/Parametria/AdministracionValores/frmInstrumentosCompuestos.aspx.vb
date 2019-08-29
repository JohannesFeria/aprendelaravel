Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_AdministracionValores_frmInstrumentosCompuestos
    Inherits BasePage
    Protected dtDetalle As New DataTable
    Protected dtDetalleEliminado As New DataTable

    '#Region " /* Metodos de Pagina */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarCombos()
            lCodigoIsin.Text = Request.QueryString("vISIN")
            lCodigoNemo.Text = Request.QueryString("vMnemonico")
            tbRF.Text = Request.QueryString("vRF")
            tbRV.Text = Request.QueryString("vRV")
            Session("FilaSeleccionada") = -1
            If CType(Session("accionValor"), String) = "INGRESAR" Then
                If Not Session("DetalleAgrupacionC") Is Nothing Then
                    If CType(Session("DetalleAgrupacionC"), DataTable).Rows.Count > 0 Then
                        CargarDetalle()
                        dgLista.DataSource = CType(Session("DetalleAgrupacionC"), DataTable)
                        dgLista.DataBind()
                        ActualizaIdentificador()
                        ActualizaMontos_RF_RV()
                    End If
                Else
                    Session("Identificador") = 0
                    CargarDetalle()
                End If
            End If
            If CType(Session("accionValor"), String) = "MODIFICAR" Then
                If Session("DetalleAgrupacionC") Is Nothing Then
                    CargarDetalle()
                    CargarRegistro(lCodigoNemo.Text)
                    ActualizaMontos_RF_RV()
                Else
                    If CType(Session("DetalleAgrupacionC"), DataTable).Rows.Count > 0 Then
                        dgLista.DataSource = CType(Session("DetalleAgrupacionC"), DataTable)
                        dgLista.DataBind()
                        ActualizaIdentificador()
                        ActualizaMontos_RF_RV()
                    End If
                End If
            End If
            If Request.QueryString("vReadOnly") = "YES" Then
                bloqueatodo()
            End If
            Dim dtTemporal As New DataTable
            If Not Session("DetalleAgrupacionC") Is Nothing Then
                dtTemporal = CType(Session("DetalleAgrupacionC"), DataTable).Copy
                Session("Temporal") = dtTemporal
            Else
                Session("Temporal") = Nothing
            End If
        End If
    End Sub

    Private Sub bloqueatodo()
        ddlEmision.Enabled = False
        ddlTipoInstrumento.Enabled = False
        ddlSituacion.Enabled = False
        tbCantidad.ReadOnly = True
        tbMonto.ReadOnly = True
        btnAgregar.Visible = False
        btnAceptar.Visible = False
        dgLista.Columns(0).Visible = False
        dgLista.Columns(1).Visible = False
    End Sub

    Public Sub CargarCombos()
        Dim DtTablaTipoInstrumento As New DataTable
        Dim DtTablaEmision As New DataTable
        Dim DtTablaSituacion As New DataTable
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        DtTablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        DtTablaTipoInstrumento = oTipoInstrumentoBM.Listar(DatosRequest).Tables(0)

        Session("TablaTipoInstrumento") = DtTablaTipoInstrumento
        HelpCombo.LlenarComboBox(ddlTipoInstrumento, DtTablaTipoInstrumento, "CodigoTipoInstrumento", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlSituacion, DtTablaSituacion, "Valor", "Nombre", False)
    End Sub

    Public Sub CargarDetalle()
        If Not Session("DetalleAgrupacionC") Is Nothing Then
            If CType(Session("DetalleAgrupacionC"), DataTable).Rows.Count > 0 Then
                Exit Sub
            End If
        End If
        dtDetalle = New DataTable
        dtDetalle.Columns.Add("CodigoTipoInstrumento")
        dtDetalle.Columns.Add("DescripcionTipoInstrumento")
        dtDetalle.Columns.Add("CodigoNemonicoAsociado")
        dtDetalle.Columns.Add("Emision")
        dtDetalle.Columns.Add("Monto")
        dtDetalle.Columns.Add("Cantidad")
        dtDetalle.Columns.Add("Situacion")
        dtDetalle.Columns.Add("Identificador")
        Session("DetalleAgrupacionC") = dtDetalle
        dgLista.DataSource = dtDetalle
        dgLista.DataBind()
    End Sub

    Public Sub CargarRegistro(ByVal CodigoNemo As String)
        Dim oInsComBM As New InstrumentosCompuestosBM
        Dim i As Integer
        dtDetalle = oInsComBM.SeleccionarInstrumentosCompuestos(CodigoNemo, DatosRequest).Tables(0)
        dtDetalle.Columns.Add("Identificador")
        If dtDetalle.Rows.Count > 0 Then
            For i = 0 To dtDetalle.Rows.Count - 1
                dtDetalle.Rows(i)("Identificador") = i
            Next
            Session("Identificador") = dtDetalle.Rows.Count
            Session("DetalleAgrupacionC") = dtDetalle
            dgLista.DataSource = dtDetalle
            dgLista.DataBind()
        End If
    End Sub

    Private Sub ActualizaIdentificador()
        dtDetalle = New DataTable
        dtDetalle = CType(Session("DetalleAgrupacionC"), DataTable)
        Dim i As Integer
        For i = 0 To dtDetalle.Rows.Count - 1
            dtDetalle.Rows(i)("Identificador") = i
        Next
        Session("Identificador") = dtDetalle.Rows.Count
        Session("DetalleAgrupacionC") = dtDetalle
        dgLista.DataSource = dtDetalle
        dgLista.DataBind()
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        If CType(Session("FilaSeleccionada"), Integer) <> -1 Then
            ModificarFila()
            btnAgregar.Text = "Agregar"
        Else
            InsertarFila()
        End If
        ActualizaMontos_RF_RV()
        LimpiarDetalle()
    End Sub

    Private Sub ActualizaMontos_RF_RV()
        'CodigoRenta
        Dim dtDetalle As DataTable = CType(Session("DetalleAgrupacionC"), DataTable)
        Dim dtTipoInstrumento As DataTable = CType(Session("TablaTipoInstrumento"), DataTable)
        Dim i As Integer
        Dim j As Integer
        Dim decRF As Decimal = 0.0
        Dim decRV As Decimal = 0.0
        Dim fecha As Decimal = Convert.ToDecimal(Request.QueryString("vFecha"))
        Dim OvpBM As New VectorPrecioBM
        Dim dtAux As DataTable
        Dim decVectorPrecio As Decimal = 0.0
        For i = 0 To dtDetalle.Rows.Count - 1
            For j = 0 To dtTipoInstrumento.Rows.Count - 1
                If dtDetalle.Rows(i)("CodigoTipoInstrumento") = dtTipoInstrumento.Rows(j)("CodigoTipoInstrumento") Then
                    dtAux = OvpBM.SeleccionarPorMnemonicoFecha(fecha, dtDetalle.Rows(i)("CodigoNemonicoAsociado"), DatosRequest).Tables(0)
                    If Not dtAux Is Nothing Then
                        If dtAux.Rows.Count > 0 Then
                            decVectorPrecio = Convert.ToDecimal(dtAux.Rows(0)("ValorPrecio"))
                        Else
                            decVectorPrecio = 0
                        End If
                    Else
                        decVectorPrecio = 0
                    End If
                    If dtTipoInstrumento.Rows(j)("CodigoRenta") = UIUtility.ObtenerCodigoTipoRenta("RFIJ") Then
                        decRF = decRF + decVectorPrecio * Convert.ToDecimal(CType(dtDetalle.Rows(i)("Cantidad"), String).Replace(".", UIUtility.DecimalSeparator))
                    ElseIf dtTipoInstrumento.Rows(j)("CodigoRenta") = UIUtility.ObtenerCodigoTipoRenta("RVAR") Then
                        decRV = decRV + decVectorPrecio * Convert.ToDecimal(CType(dtDetalle.Rows(i)("Cantidad"), String).Replace(".", UIUtility.DecimalSeparator))
                    End If
                    Exit For
                End If
            Next
        Next
        If decRF + decRV > 0 Then
            tbRFteorico.Text = Math.Round(100 * decRF / (decRF + decRV), Constantes.M_INT_NRO_DECIMALES).ToString.Replace(UIUtility.DecimalSeparator, ".")
            tbRVteorico.Text = Math.Round(100 * decRV / (decRF + decRV), Constantes.M_INT_NRO_DECIMALES).ToString.Replace(UIUtility.DecimalSeparator, ".")
        Else
            tbRFteorico.Text = "0.0000000"
            tbRVteorico.Text = "0.0000000"
        End If
    End Sub

    Private Sub LimpiarDetalle()
        ddlEmision.Items.Clear()
        ddlTipoInstrumento.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0
        tbMonto.Text = ""
        tbCantidad.Text = ""
        ddlEmision.Enabled = True
        ddlTipoInstrumento.Enabled = True
        tbCantidad.Enabled = True
        tbMonto.Enabled = True
    End Sub

    Public Sub InsertarFila()
        Dim drFila As DataRow
        dtDetalle = New DataTable
        dtDetalle = CType(Session("DetalleAgrupacionC"), DataTable)
        drFila = dtDetalle.NewRow
        drFila("CodigoTipoInstrumento") = ddlTipoInstrumento.SelectedValue
        drFila("DescripcionTipoInstrumento") = ddlTipoInstrumento.Items(ddlTipoInstrumento.SelectedIndex).Text
        drFila("Emision") = ddlEmision.Items(ddlEmision.SelectedIndex).Text
        drFila("CodigoNemonicoAsociado") = ddlEmision.SelectedValue
        drFila("Monto") = tbMonto.Text
        drFila("Cantidad") = tbCantidad.Text
        If ddlSituacion.SelectedValue = "A" Then
            drFila("Situacion") = "ACTIVO"
        Else
            drFila("Situacion") = "INACTIVO"
        End If
        drFila("Identificador") = CType(Session("Identificador"), Integer)

        If VerificarFila(ddlTipoInstrumento.SelectedValue, ddlEmision.SelectedValue) Then
            AlertaJS("Ya ingresó una misma característica")
        Else
            dtDetalle.Rows.Add(drFila)
            Session("DetalleAgrupacionC") = dtDetalle
            Session("Identificador") = CType(Session("Identificador"), Integer) + 1
            dgLista.DataSource = dtDetalle
            dgLista.DataBind()
        End If
    End Sub

    Public Function VerificarFilaModificar(ByVal strCodigoTipoInstrumento As String, ByVal StrCodigoEmision As String, ByVal intNroFila As Integer) As Boolean
        Dim BlnEsta As Boolean = False
        Dim i As Integer
        dtDetalle = CType(Session("DetalleAgrupacionC"), DataTable)
        For i = 0 To dtDetalle.Rows.Count - 1
            If CType(dtDetalle.Rows(i)("CodigoTipoInstrumento"), String) = strCodigoTipoInstrumento And CType(dtDetalle.Rows(i)("CodigoNemonicoAsociado"), String) = StrCodigoEmision And i <> intNroFila Then
                BlnEsta = True
                Exit For
            End If
        Next
        Return BlnEsta
    End Function

    Public Sub ModificarFila()
        Dim numSeleccionado As Integer
        Dim i As Integer
        dtDetalle = CType(Session("DetalleAgrupacionC"), DataTable)
        numSeleccionado = CType(Session("FilaSeleccionada"), Integer)
        If (numSeleccionado <> -1) Then
            For i = 0 To dtDetalle.Rows.Count - 1
                If CType(dtDetalle.Rows(i)("Identificador"), Integer) = numSeleccionado Then
                    If VerificarFilaModificar(ddlTipoInstrumento.SelectedValue, ddlEmision.SelectedValue, i) Then
                        AlertaJS("Ya ingresó el Custodio")
                    Else
                        dtDetalle.Rows(i)("CodigoTipoInstrumento") = ddlTipoInstrumento.SelectedValue
                        dtDetalle.Rows(i)("DescripcionTipoInstrumento") = ddlTipoInstrumento.Items(ddlTipoInstrumento.SelectedIndex).Text
                        dtDetalle.Rows(i)("CodigoNemonicoAsociado") = ddlEmision.SelectedValue
                        dtDetalle.Rows(i)("Emision") = ddlEmision.Items(ddlEmision.SelectedIndex).Text
                        dtDetalle.Rows(i)("Monto") = tbMonto.Text
                        dtDetalle.Rows(i)("Cantidad") = tbCantidad.Text
                        If ddlSituacion.SelectedValue = "A" Then
                            dtDetalle.Rows(i)("Situacion") = "ACTIVO"
                        Else
                            dtDetalle.Rows(i)("Situacion") = "INACTIVO"
                        End If
                        dgLista.DataSource = dtDetalle
                        dgLista.DataBind()
                        Session("DetalleAgrupacionC") = dtDetalle
                        Session("FilaSeleccionada") = -1
                    End If
                End If
            Next
        End If
    End Sub

    Public Function VerificarFila(ByVal strCodigoTipoInstrumento As String, ByVal strCodigoEmision As String) As Boolean
        Dim BlnEsta As Boolean = False
        Dim i As Integer
        dtDetalle = CType(Session("DetalleAgrupacionC"), DataTable)
        For i = 0 To dtDetalle.Rows.Count - 1
            If dtDetalle.Rows(i)("CodigoTipoInstrumento") = strCodigoTipoInstrumento And dtDetalle.Rows(i)("CodigoNemonicoAsociado") = strCodigoEmision Then
                BlnEsta = True
                Exit For
            End If
        Next
        Return BlnEsta
    End Function

    Private Sub ddlTipoInstrumento_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoInstrumento.SelectedIndexChanged
        cambiaTipoInstrumento()
    End Sub

    Private Sub cambiaTipoInstrumento()
        Dim i As Integer
        Dim dtTipoInst As DataTable
        dtTipoInst = CType(Session("TablaTipoInstrumento"), DataTable)
        For i = 0 To dtTipoInst.Rows.Count - 1
            If CType(dtTipoInst.Rows(i)("CodigoTipoInstrumento"), String) = ddlTipoInstrumento.SelectedValue Then
                cargarEmisor(CType(dtTipoInst.Rows(i)("CodigoTipoInstrumentoSBS"), String))
                tbMonto.Text = ""
                If dtTipoInst.Rows(i)("CodigoRenta") = UIUtility.ObtenerCodigoTipoRenta("RVAR") Then
                    tbMonto.ReadOnly = True
                    tbMonto.Text = "0"
                Else
                    tbMonto.ReadOnly = False
                End If
                Exit For
            End If
        Next
    End Sub

    Private Sub cargarEmisor(ByVal strCodigoSBS As String)
        Dim dtEmisor As DataTable
        Dim oValores As New ValoresBM
        dtEmisor = oValores.ListarPorTipoInstrumentoSBS(DatosRequest, strCodigoSBS).Tables(0)
        Session("dtEmisor") = dtEmisor
        HelpCombo.LlenarComboBox(ddlEmision, dtEmisor, "CodigoNemonico", "Descripcion", True)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Session("DetalleAgrupacionC") = Session("Temporal")
        EjecutarJS("window.close();")
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        dtDetalle = CType(Session("DetalleAgrupacionC"), DataTable)
        If (dtDetalle.Rows.Count > 0) Then
            Try
                Session("agrupacionIC") = tbRF.Text + ParametrosSIT.SEPARADOR_OI + tbRV.Text
                EjecutarJS("window.close();")
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        Else
            AlertaJS("Debe Ingresar mínimo dos detalles")
        End If
    End Sub

    Private Sub tbMonto_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMonto.TextChanged
        If tbMonto.Text <> "" And hdValorUnitario.Value <> "" Then
            If Convert.ToDecimal(tbMonto.Text.Replace(".", UIUtility.DecimalSeparator)) <> 0 Then
                tbCantidad.Text = (Convert.ToDecimal(tbMonto.Text.Replace(".", UIUtility.DecimalSeparator)) / Convert.ToDecimal(hdValorUnitario.Value)).ToString.Replace(UIUtility.DecimalSeparator, ".")
            End If
        End If
    End Sub

    Private Sub ddlEmision_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEmision.SelectedIndexChanged
        cambiaEmision()
    End Sub

    Private Sub cambiaEmision()
        Dim dtAux As DataTable = CType(Session("dtEmisor"), DataTable)
        Dim i As Integer
        For i = 0 To dtAux.Rows.Count - 1
            If dtAux.Rows(i)("CodigoNemonico") = ddlEmision.SelectedValue Then
                hdValorUnitario.Value = dtAux.Rows(i)("ValorUnitario")
                If hdValorUnitario.Value = "" Or hdValorUnitario.Value = "0" Or hdValorUnitario.Value = 0 Then
                    hdValorUnitario.Value = "1"
                End If
                Exit For
            End If
        Next
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Dim intNroFila = -1
        Dim i As Integer
        Dim oInstEstBM As New InstrumentosCompuestosBM 'InstrumentosEstructuradosBM
        dtDetalle = CType(Session("DetalleAgrupacionC"), DataTable)
        Session("FilaSeleccionada") = CType(e.CommandArgument, Integer)
        intNroFila = CType(e.CommandArgument, Integer)
        For i = 0 To dtDetalle.Rows.Count - 1
            If dtDetalle.Rows(i)("Identificador") = intNroFila Then
                If CType(e.CommandSource, ImageButton).CommandName = "Modificar" Then
                    ddlTipoInstrumento.SelectedValue = CType(dtDetalle.Rows(i)("CodigoTipoInstrumento"), String)
                    cambiaTipoInstrumento()
                    ddlEmision.SelectedValue = CType(dtDetalle.Rows(i)("CodigoNemonicoAsociado"), String)
                    cambiaEmision()
                    If CType(dtDetalle.Rows(i)("Situacion"), String) = "ACTIVO" Then
                        ddlSituacion.SelectedValue = "A"
                    Else
                        ddlSituacion.SelectedValue = "I"
                    End If
                    tbCantidad.Text = dtDetalle.Rows(i)("Cantidad")
                    tbMonto.Text = dtDetalle.Rows(i)("Monto")
                    ddlEmision.Enabled = False
                    ddlTipoInstrumento.Enabled = False
                    btnAgregar.Text = "Modificar"
                ElseIf CType(e.CommandSource, ImageButton).CommandName = "Eliminar" Then
                    dtDetalle.Rows(i)("Situacion") = "INACTIVO"
                    Dim dtOtro As New DataTable
                    If lCodigoNemo.Text <> "" Then
                        dtOtro.ImportRow(dtDetalle.Rows(i))
                        oInstEstBM.IngresarModificarInstrumentosCompuestos(lCodigoNemo.Text, dtOtro, DatosRequest)
                    End If
                    dtDetalle.Rows.RemoveAt(i)
                    Session("FilaSeleccionada") = -1
                    LimpiarDetalle()
                    btnAgregar.Text = "Agregar"
                    ActualizaMontos_RF_RV()
                End If
                Exit For
            End If
        Next
        dgLista.DataSource = dtDetalle
        dgLista.DataBind()
        Session("DetalleAgrupacionC") = dtDetalle

    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
        End If
    End Sub
End Class

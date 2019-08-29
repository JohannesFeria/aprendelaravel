Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.IO
Imports Word
Imports iTextSharp.text.pdf
Imports Microsoft.Office
Imports System.Data
Partial Class Modulos_Tesoreria_OperacionesCaja_frmGeneracionCartasEmitidas
    Inherits BasePage
#Region "Declaracion de Variables"
    Private _strcartas As String
#End Region
#Region "Eventos de la Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarMercado(True)
                CargarPortafolio(True)
                CargarBanco(ddlMercado.SelectedValue, True)
                CargarIntermediario(True)
                tbFecha.Text = UIUtility.ConvertirDecimalAStringFormatoFecha(UIUtility.ObtenerFechaMaximaNegocio())
                btnProcesar.Attributes.Add("onclick", "return ValidarDatos(true);")
            End If
            btnExtornar.Attributes.Add("onclick", "return ValidarSeleccion(false);")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            dgLista.PageIndex = 0
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al procesar los datos")
        End Try
    End Sub
    Public Sub dgListaItemCommand(ByVal sender As Object, ByVal e As CommandEventArgs)
        If e.CommandName = "Seleccionar" Then
            hdNumeroCarta.Value = e.CommandArgument
        End If
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oOperacionCaja As New OperacionesCajaBM
            If rbtlEmision.SelectedValue = 0 Then
                EjecutarJS("window.open('CartasTesoreria/" & hdNumeroCarta.Value & ".pdf');")
                oOperacionCaja.ModificarEstadoCarta(hdNumeroCarta.Value, ParametrosSIT.ESTADOCARTA_ENVIADA, ParametrosSIT.TIPOEMISIONCARTA_MANUAL, False, DatosRequest)
            Else
                oOperacionCaja.ModificarEstadoCarta(hdNumeroCarta.Value, ParametrosSIT.ESTADOCARTA_PORAPROBAR, ParametrosSIT.TIPOEMISIONCARTA_AUTOMATICA, False, DatosRequest)
            End If
            CargarGrilla(False)
            dgLista.SelectedIndex = -1
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVista.Click
        Dim objparametrosgenerales As New ParametrosGeneralesBM
        Dim oOperacionCaja As New OperacionesCajaBM
        Dim strNumeroCarta As String
        Dim ruta As String = objparametrosgenerales.ListarRutaGeneracionCartas(DatosRequest) + "\"
        Dim path As String
        Dim chk As System.Web.UI.WebControls.CheckBox
        Dim windowsCartas As New System.Text.StringBuilder
        Dim rutaCartas As String
        Dim veces As Integer
        veces = 0
        Try
            Session("RutaCarta") = ""
            For Each item As GridViewRow In Me.dgLista.Rows
                If (item.RowType = DataControlRowType.DataRow) Then
                    If (item.FindControl("chkPrint") Is Nothing) = False Then
                        chk = CType(item.FindControl("chkPrint"), WebControls.CheckBox)
                        If (chk.Checked = True) Then
                            strNumeroCarta = item.Cells(2).Text
                            path = ruta & strNumeroCarta & ".pdf"
                            If IO.File.Exists(path) Then
                                oOperacionCaja.ModificarEstadoCarta(strNumeroCarta, "I", "", False, DatosRequest)
                                windowsCartas.Append(path & "&")
                                veces = veces + 1
                            End If
                        End If
                    End If
                End If
            Next
            rutaCartas = windowsCartas.ToString()
            If (rutaCartas.Length > 0) Then
                rutaCartas = rutaCartas.Remove(rutaCartas.Length - 1, 1)
                If (veces = 1) Then
                    Session("RutaCarta") = rutaCartas
                Else
                    Session("RutaCarta") = HelpCarta.CrearMultiCartaPDF(rutaCartas)
                End If
                If IO.File.Exists(Session("RutaCarta")) Then
                    EjecutarJS("window.open('frmVisorCarta.aspx?');")
                Else
                    AlertaJS(ObtenerMensaje("ALERT126"))
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnExtornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtornar.Click
        Dim objparametrosgenerales As New ParametrosGeneralesBM
        Dim strcartas As String = objparametrosgenerales.ListarRutaGeneracionCartas(DatosRequest)
        Try
            Dim oOperacionCaja As New OperacionesCajaBM
            oOperacionCaja.ModificarEstadoCarta(hdNumeroCarta.Value, "", "", False, DatosRequest)
            If File.Exists(strcartas & "\" & hdNumeroCarta.Value & ".doc") Then
                File.Delete(strcartas & "\" & hdNumeroCarta.Value & ".doc")
            End If
            AlertaJS(ObtenerMensaje("ALERT52"))
            dgLista.SelectedIndex = -1
            CargarDatosGrilla()
            CargarGrilla(False)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
#End Region
#Region "Cargar Datos"
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dsMercado As DataSet = oMercado.ListarActivos(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            ddlMercado.Items.Clear()
            ddlMercado.DataSource = dsMercado
            ddlMercado.DataValueField = "CodigoMercado"
            ddlMercado.DataTextField = "Descripcion"
            ddlMercado.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
    End Sub
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            'Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolio.Items.Clear()
            HelpCombo.LlenarComboBox(ddlPortafolio, dsPortafolio, "CodigoPortafolio", "Descripcion", True)
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub

    Private Sub CargarBanco(ByVal CodigoMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorCodigoMercadoYPortafolio("", Me.ddlPortafolio.SelectedValue)
            ddlBanco.Items.Clear()
            ddlBanco.DataSource = dsBanco
            ddlBanco.DataValueField = "CodigoTercero"
            ddlBanco.DataTextField = "Descripcion"
            ddlBanco.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
    End Sub
    Private Sub CargarIntermediario(ByVal enabled As Boolean)
        If enabled Then
            Dim oIntermediario As New TercerosBM
            Dim dsIntermediario As TercerosBE = oIntermediario.SeleccionarPorFiltro(ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO, "")
            ddlIntermediario.Items.Clear()
            ddlIntermediario.DataSource = dsIntermediario
            ddlIntermediario.DataValueField = "CodigoTercero"
            ddlIntermediario.DataTextField = "Descripcion"
            ddlIntermediario.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlIntermediario)
        Else
            ddlIntermediario.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlIntermediario)
        End If
        ddlIntermediario.Enabled = enabled
    End Sub
#End Region
#Region "Metodos de Control"
    Private Sub CargarGrilla(Optional ByVal generar As Boolean = True)
        Dim fecha As Decimal = 0
        Dim Impreso As String
        If tbFecha.Text <> "" Then
            fecha = UIUtility.ConvertirFechaaDecimal(tbFecha.Text)
        End If
        If rdImpreso.SelectedValue = "T" Then
            Impreso = ""
        Else
            Impreso = rdImpreso.SelectedValue
        End If
        Dim dsOperaciones As DataSet = New OperacionesCajaBM().SeleccionarPorFiltro(ddlMercado.SelectedValue, _
         ddlPortafolio.SelectedValue, ddlIntermediario.SelectedValue, ddlBanco.SelectedValue, fecha, generar, False, Impreso, DatosRequest)
        If generar Then
            HelpCarta.GenerarCartas(dsOperaciones, DatosRequest)
        End If
        dgLista.DataSource = dsOperaciones
        dgLista.DataBind()
        NroCarta.Text = UIUtility.MostrarResultadoBusqueda(dsOperaciones.Tables(0))
    End Sub
    Sub CargarDatosGrilla()
        Dim i As Integer = 0
        Dim fecha As Decimal = 0
        Dim Impreso As String = String.Empty
        Dim dsOperaciones As DataSet
        If tbFecha.Text <> "" Then fecha = UIUtility.ConvertirFechaaDecimal(tbFecha.Text)
        If rdImpreso.SelectedValue <> "T" Then Impreso = rdImpreso.SelectedValue
        dsOperaciones = New OperacionesCajaBM().SeleccionarPorFiltro(ddlMercado.SelectedValue, _
        ddlPortafolio.SelectedValue, ddlIntermediario.SelectedValue, ddlBanco.SelectedValue, fecha, True, False, Impreso, DatosRequest)
        _strcartas = New ParametrosGeneralesBM().ListarRutaGeneracionCartas(DatosRequest)
        Dim dtOperaciones As DataTable = dsOperaciones.Tables(0)
        Dim drOperacion As DataRow
        Dim contador As Integer = 0
        For i = 0 To dtOperaciones.Rows.Count - 1
            drOperacion = dtOperaciones.Rows(i)
            If dtOperaciones.Rows(i)("NumeroCarta").ToString.Trim = "" Then
                dtOperaciones.Rows(i).Delete()
                contador = contador + 1
            Else
                If Not HelpCarta.CrearCartaPDF(drOperacion, DatosRequest) Then
                    dtOperaciones.Rows(i).Delete()
                    contador = contador + 1
                End If
            End If
        Next
        dgLista.DataSource = dtOperaciones
        dgLista.DataBind()
        NroCarta.Text = "Registros Encontrados: " & dtOperaciones.Rows.Count - contador
    End Sub
    Private Sub EiminaReferencias(ByRef Referencias As Object)
        Try
            Do Until _
                 System.Runtime.InteropServices.Marshal.ReleaseComObject(Referencias) <= 0
            Loop
        Catch
        Finally
            Referencias = Nothing
        End Try
    End Sub
    Private Sub EstablecerFecha()
        tbFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
    End Sub
#End Region
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        EstablecerFecha()
        CargarBanco(ddlMercado.SelectedValue, True)
    End Sub
    Private Sub ddlMercado_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        CargarBanco(ddlMercado.SelectedValue, True)
    End Sub
#Region "Crear PDF"
    Public Sub cbSeleccionarTodo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        For Each oDataGridItem As GridViewRow In Me.dgLista.Rows
            CType(oDataGridItem.FindControl("chkPrint"), WebControls.CheckBox).Checked = sender.Checked
        Next
    End Sub
#End Region
    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarDatosGrilla()
    End Sub
    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Dim oCheck As System.Web.UI.WebControls.CheckBox
        Dim valor As String
        Static ind = 1
        If e.Row.RowType = ListItemType.Item Then
            e.Row.Attributes.Add("onmouseover", "this.style.cursor='hand'")
            e.Row.Attributes.Add("onclick", "document.getElementById('hdNumeroCarta').value='" & e.Row.Cells(2).Text & "';__doPostBack" & _
                   "('_ctl0$dgLista$_ctl" & _
                   ((Convert.ToInt32(e.Row.RowIndex.ToString())) + 2) & _
                   "$_ctl0','')")
            Dim dr As DataRowView = CType(e.Row.DataItem, DataRowView)

            If Not (e.Row.FindControl("ChkImpreso") Is Nothing) Then
                oCheck = CType(e.Row.FindControl("ChkImpreso"), System.Web.UI.WebControls.CheckBox)
                'valor = IIf(IsDBNull(dr.DataItem("Impreso")), "N", dr.DataItem("Impreso"))
                valor = IIf(IsDBNull(dr("Impreso")), "N", dr("Impreso"))
                oCheck.Checked = IIf(valor = "I", True, False)
            End If

        End If
        ind = ind + 1
    End Sub
End Class
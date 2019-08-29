Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports UIUtility
Imports System.Data
Partial Class Modulos_Inversiones_frmInversionesRealizadas
    Inherits BasePage
    Dim oOrdenesInversionBM As New OrdenPreOrdenInversionBM
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Dim oUtilBM As New UtilDM
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If Not Page.IsPostBack Then
            dgLista.PageIndex = 0
            CargarOrdenes()
            HelpCombo.CargarMotivosCambio(Me)
        End If
    End Sub
    Public Sub CargarOrdenes()
        Dim strISIN As String = IIf(Request.QueryString("vISIN") Is Nothing, "", Request.QueryString("vISIN"))
        Dim strSBS As String = IIf(Request.QueryString("vSBS") Is Nothing, "", Request.QueryString("vSBS"))
        Dim strMnemonico As String = IIf(Request.QueryString("vMnemonico") Is Nothing, "", Request.QueryString("vMnemonico"))
        Dim strFondo As String = IIf(Request.QueryString("vFondo") Is Nothing, "", Request.QueryString("vFondo"))
        Dim codFondo As String = IIf(Request.QueryString("cFondo") Is Nothing, "", Request.QueryString("cFondo"))
        Dim strMoneda As String = IIf(Request.QueryString("vMoneda") Is Nothing, "", Request.QueryString("vMoneda"))
        Dim strOperacion As String = IIf(Request.QueryString("vOperacion") Is Nothing, "", Request.QueryString("vOperacion"))
        Dim strCategoriaInstrumento As String = IIf(Request.QueryString("vCategoria") Is Nothing, "", Request.QueryString("vCategoria"))
        hdnCategoriaInstrumento.Value = strCategoriaInstrumento
        Dim strAccion As String = IIf(Request.QueryString("vAccion") Is Nothing, "", Request.QueryString("vAccion"))
        Dim dcFechaOperacion As String = String.Empty
        '****************************** Developer by Carlos hernández *************************************
        '
        If (Not Session("context_info") Is Nothing) Then
            If TypeOf Session("context_info") Is Hashtable Then 'Si se desea pasar otro tipo de objeto se debe validar de esta manera - CRumiche
                Dim htInfo As Hashtable = Session("context_info")

                If htInfo.Contains("Portafolio") Then strFondo = htInfo("Portafolio").ToString
                '// Obtener más valores aquí de la manera mostrada
            End If
        End If
        '
        '**************************************************************************************************
        Try
            If Request.QueryString("vFechaOperacion") = "" Then
                If strFondo <> "" Then
                    Dim oPortBM As New PortafolioBM
                    Dim dtAux As DataTable = oPortBM.SeleccionarPortafolioPorFiltro(codFondo, DatosRequest).Tables(0)
                    If Not dtAux Is Nothing Then
                        If dtAux.Rows.Count > 0 Then
                            dcFechaOperacion = Convert.ToDecimal(dtAux.Rows(0)("FechaConstitucion"))
                        End If
                    End If
                Else
                    dcFechaOperacion = UIUtility.ConvertirFechaaDecimal(oUtilBM.RetornarFechaSistema).ToString
                End If
            Else
                dcFechaOperacion = Request.QueryString("vFechaOperacion")
            End If

            Dim dcFechaO As Decimal = ToNullDecimal(dcFechaOperacion)
            lblMoneda.Text = IIf(strMoneda.ToString <> "", strMoneda, "Todos")
            lblFondo.Text = strFondo
            hdCodFondo.Value = codFondo
            Dim dtblDatos As New DataTable

            dtblDatos.Rows.Clear()
            dtblDatos = oOrdenesInversionBM.ListarOrdenesCabecera(strCategoriaInstrumento, dcFechaO, strSBS, strISIN, strMnemonico, codFondo, strOperacion, strMoneda, strAccion, DatosRequest).Tables(0)
            lblTitulo.Text = "Ordenes de Inversión Vigentes a la Fecha de " & UIUtility.ConvertirFechaaString(dcFechaO) & " en adelante"

            If dtblDatos.Rows.Count > 0 Then
                dgLista.DataSource = dtblDatos
                dgLista.DataBind()
            Else
                AlertaJS("No se encontraron Registros", "window.close();")
            End If
            If (strAccion <> "E" Or hdnCategoriaInstrumento.Value = "SW") Then
                btnEliminar.Visible = False
                dgLista.Columns(22).Visible = False
                RowEliminacion.Visible = False
            End If

            lbContador.Text = MostrarResultadoBusqueda(dtblDatos)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ReturnArgumentShowDialogPopup(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal operacion As String, ByVal moneda As String, ByVal codigoorden As String)
        If Not Session("SS_DatosModal") Is Nothing Then
            Session.Remove("SS_DatosModal")
        End If

        Dim arraySesiones As String() = New String(6) {}
        arraySesiones(0) = isin
        arraySesiones(1) = sbs
        arraySesiones(2) = mnemonico
        arraySesiones(3) = fondo
        arraySesiones(4) = operacion
        arraySesiones(5) = moneda
        arraySesiones(6) = codigoorden

        Session("SS_DatosModal") = arraySesiones
        Session("Busqueda") = 1
        EjecutarJS("Close(); return false;")
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If lblParametros.Value.Trim = "" Then
            AlertaJS("Debe seleccionar una Orden de Inversión")
        Else
            Dim parametros() As String = lblParametros.Value.Split(",")
            ReturnArgumentShowDialogPopup(HttpUtility.HtmlDecode(parametros(0)).Trim, HttpUtility.HtmlDecode(parametros(1)).Trim, HttpUtility.HtmlDecode(parametros(2)).Trim, HttpUtility.HtmlDecode(parametros(3)).Trim, HttpUtility.HtmlDecode(parametros(4)).Trim, HttpUtility.HtmlDecode(parametros(5)).Trim, HttpUtility.HtmlDecode(parametros(6)).Trim)
        End If
    End Sub

    'Private Sub dgLista_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgLista.PageIndexChanged
    '    dgLista.SelectedIndex = -1
    '    dgLista.PageIndex = e.NewPageIndex
    '    CargarOrdenes()
    'End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Session("Busqueda") = 2
        EjecutarJS("window.close();")
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        EliminarOrdenInversion()
        dgLista.SelectedIndex = -1
        CargarOrdenes()
    End Sub
    Public Sub EliminarOrdenInversion()
        Dim chkSelect As CheckBox
        Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim oImpComOP As New ImpuestosComisionesOrdenPreOrdenBM
        For Each item As GridViewRow In dgLista.Rows
            If Not (item.FindControl("chkSelect") Is Nothing) Then
                chkSelect = CType(item.FindControl("chkSelect"), CheckBox)
                If (chkSelect.Checked) Then
                    oOrdenInversionBM.EliminarOI(item.Cells(1).Text, hdCodFondo.Value, ddlMotivoCambio.SelectedValue, DatosRequest)
                    oOrdenInversionBM.FechaModificarEliminarOI(hdCodFondo.Value, item.Cells(1).Text, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), "E", txtComentarios.Text, DatosRequest)    'HDG OT 60882 20100915
                    oImpComOP.Eliminar(item.Cells(1).Text, hdCodFondo.Value, DatosRequest)
                End If
            End If
        Next
        AlertaJS("Se ha eliminado las ordenes satisfactoriamente.")
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Dim chkSelect As CheckBox
        If e.CommandName = "Seleccionar" Then
            Dim intA As Integer = CInt(e.CommandArgument.ToString())
            dgLista.SelectedIndex = CInt(e.CommandArgument.ToString())
            lbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dgLista.Rows.Item(intA).Cells(12).Text))
            lbHoraNegociacion.Text = dgLista.Rows.Item(intA).Cells(13).Text
            lbTipoValor.Text = IIf(dgLista.Rows.Item(intA).Cells(14).Text.Equals("&nbsp;"), String.Empty, dgLista.Rows.Item(intA).Cells(14).Text)
            lbPrecio.Text = Format(Convert.ToDecimal(dgLista.Rows.Item(intA).Cells(15).Text), "##,##0.0000000")
            lbTasa.Text = Format(Convert.ToDecimal(dgLista.Rows.Item(intA).Cells(16).Text), "##,##0.0000000")
            lbMontoOperacion.Text = Format(Convert.ToDecimal(dgLista.Rows.Item(intA).Cells(17).Text), "##,##0.0000000")
            lbMontoNominal.Text = Format(Convert.ToDecimal(dgLista.Rows.Item(intA).Cells(18).Text), "##,##0.0000000")
            lbFechaUltimo.Text = IIf(dgLista.Rows.Item(intA).Cells(19).Text.Equals("&nbsp;"), String.Empty, dgLista.Rows.Item(intA).Cells(19).Text)
            lbFechaProximo.Text = IIf(dgLista.Rows.Item(intA).Cells(20).Text.Equals("&nbsp;"), String.Empty, dgLista.Rows.Item(intA).Cells(20).Text)
            lbFechaVencimiento.Text = IIf(dgLista.Rows.Item(intA).Cells(21).Text.Equals("&nbsp;"), String.Empty, dgLista.Rows.Item(intA).Cells(21).Text)
            chkSelect = CType(dgLista.Rows.Item(intA).Cells(22).Controls(1), CheckBox)
            If chkSelect.Checked = True Then
                chkSelect.Checked = False
            Else
                chkSelect.Checked = True
            End If
            With dgLista.Rows.Item(intA)
                '
                lblParametros.Value = HttpUtility.HtmlDecode(.Cells(2).Text).Trim & "," & HttpUtility.HtmlDecode(.Cells(4).Text).Trim & "," & HttpUtility.HtmlDecode(.Cells(3).Text).Trim & "," & HttpUtility.HtmlDecode(.Cells(23).Text).Trim & "," & HttpUtility.HtmlDecode(.Cells(11).Text).Trim & "," & HttpUtility.HtmlDecode(.Cells(8).Text).Trim & "," & HttpUtility.HtmlDecode(.Cells(1).Text).Trim
            End With

        End If
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Dim chkSelect As CheckBox
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            If Not (e.Row.FindControl("chkSelect") Is Nothing) Then
                chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)
                chkSelect.Attributes.Add("onclick", "javaScript:OnSelectOrden(" + chkSelect.ClientID + ");")
            End If
        End If
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarOrdenes()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el paginado")
        End Try
    End Sub
End Class
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports ParametrosSIT
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaLimiteTrading
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            Me.AlertaJS("Ocurrió un error en al cargar la Página")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Response.Redirect("frmLimiteTrading.aspx?ope=reg&CodigoRenta=" + ViewState("CodigoRenta") + "&CodigoGrupLimTrader=" + ViewState("CodigoGrupLimTrader") + "&TipoCargo=" + ViewState("TipoCargo") + "&Portafolio=" + ViewState("Portafolio"))
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            ViewState("CodigoRenta") = ddlTipoRenta.SelectedValue
            ViewState("CodigoGrupLimTrader") = ddlGrupoLimite.SelectedValue
            ViewState("TipoCargo") = ddlTipoCargo.SelectedValue
            ViewState("Portafolio") = ddlPortafolio.SelectedValue
            CargarGrilla(IIf(ViewState("CodigoRenta") = DDL_ITEM_SELECCIONE, "", ViewState("CodigoRenta")), Val(ViewState("CodigoGrupLimTrader")), ddlTipoCargo.SelectedValue, ddlPortafolio.SelectedValue)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ibModificar As ImageButton
                ibModificar = CType(e.Row.FindControl("ImageButton2"), ImageButton)
                ibModificar.Attributes.Add("onclick", "javascript:return confirm('Desea eliminar el registro? ')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName = "Editar" Then
                Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim lbTipoRenta As Label
                lbTipoRenta = CType(Row.FindControl("lbTipoRenta"), Label)
                Response.Redirect("frmLimiteTrading.aspx?ope=mod&cod=" + e.CommandArgument + "&tipo_renta=" + lbTipoRenta.Text + "&CodigoRenta=" + ViewState("CodigoRenta") + "&CodigoGrupLimTrader=" + ViewState("CodigoGrupLimTrader") + "&TipoCargo=" + ViewState("TipoCargo") + "&Portafolio=" + ViewState("Portafolio"))
            End If
            If e.CommandName = "Eliminar" Then
                Dim codigoTrading As Decimal
                codigoTrading = CType(e.CommandArgument.ToString(), Decimal)
                Dim oLimiteTradingBM As New LimiteTradingBM
                oLimiteTradingBM.Eliminar(codigoTrading, DatosRequest)
                CargarGrilla(IIf(ViewState("CodigoRenta") = DDL_ITEM_SELECCIONE, "", ViewState("CodigoRenta")), Val(ViewState("CodigoGrupLimTrader")), ViewState("TipoCargo"), ViewState("Portafolio"))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla(IIf(ViewState("CodigoRenta") = DDL_ITEM_SELECCIONE, "", ViewState("CodigoRenta")), Val(ViewState("CodigoGrupLimTrader")), ViewState("TipoCargo"), ViewState("Portafolio"))   'HDG OT 64291 20111202
    End Sub

    Private Sub ddlTipoRenta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoRenta.SelectedIndexChanged
        Try
            CargarGrupoLimite()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try        
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../../frmDefault.aspx")
    End Sub

    Private Sub CargarPagina()
        ViewState("CodigoRenta") = IIf(Request.QueryString("CodigoRenta") Is Nothing, DDL_ITEM_SELECCIONE, Request.QueryString("CodigoRenta"))    'HDG OT 64291 20111202
        ViewState("CodigoGrupLimTrader") = IIf(Request.QueryString("CodigoGrupLimTrader") Is Nothing, "", Request.QueryString("CodigoGrupLimTrader"))
        ViewState("TipoCargo") = IIf(Request.QueryString("TipoCargo") Is Nothing, "", Request.QueryString("TipoCargo"))
        ViewState("Portafolio") = IIf(Request.QueryString("Portafolio") Is Nothing, "", Request.QueryString("Portafolio"))
        CargarCombos()
        CargarGrilla(IIf(ViewState("CodigoRenta") = DDL_ITEM_SELECCIONE, "", ViewState("CodigoRenta")), Val(ViewState("CodigoGrupLimTrader")), ViewState("TipoCargo"), ViewState("Portafolio"))   'HDG OT 64291 20111202
    End Sub

    Private Sub CargarCombos()
        CargarPortafolio()
        CargarTipoRenta()
        CargarGrupoLimite()
        CargarTipoCargo()
    End Sub

    Private Sub CargarPortafolio()
        Dim oPortafolio As New PortafolioBM
        Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO)
        HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True)
        ddlPortafolio.SelectedValue = ViewState("Portafolio")
    End Sub

    Private Sub CargarGrupoLimite()
        Dim oGrupoLimiteTraderBM As New GrupoLimiteTraderBM
        Dim dtGrupoLimite As New DataTable

        dtGrupoLimite = oGrupoLimiteTraderBM.ListarGrupoLimite(ddlTipoRenta.SelectedValue).Tables(0)
        HelpCombo.LlenarComboBox(ddlGrupoLimite, dtGrupoLimite, "CodigoGrupLimTrader", "Nombre", True)
        Try
            ddlGrupoLimite.SelectedValue = ViewState("CodigoGrupLimTrader")
        Catch ex As Exception
        End Try
        ddlGrupoLimite.AutoPostBack = True
    End Sub

    Private Sub CargarTipoRenta()
        Dim oTipoRentaBM As New TipoRentaBM
        Dim dtTipoRenta As New DataTable
        dtTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoRenta, dtTipoRenta, "CodigoRenta", "Descripcion", False)
        ddlTipoRenta.Items.Insert(0, New ListItem(DDL_ITEM_SELECCIONE, DDL_ITEM_SELECCIONE))
        ddlTipoRenta.SelectedValue = TR_DERIVADOS
        ddlTipoRenta.Items.RemoveAt(ddlTipoRenta.SelectedIndex)
        ddlTipoRenta.SelectedValue = ViewState("CodigoRenta")
        ddlTipoRenta.AutoPostBack = True
    End Sub

    Private Sub CargarTipoCargo()
        Dim oRolAprobadoresTraderBM As New RolAprobadoresTraderBM
        Dim dtTipoCargo As New DataTable
        dtTipoCargo = oRolAprobadoresTraderBM.SeleccionarPorFiltro("", ESTADO_ACTIVO).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoCargo, dtTipoCargo, "TipoCargo", "Descripcion", True)
        ddlTipoCargo.SelectedValue = ViewState("TipoCargo")
    End Sub

    Private Sub CargarGrilla(ByVal CodigoRenta As String, ByVal CodigoGrupLimTrader As Decimal, ByVal TipoCargo As String, ByVal CodigoPortafolioSBS As String) 'HDG OT 64291 20111202
        Dim dtLista As New DataTable
        Dim oLimiteTradingBM As New LimiteTradingBM
        dtLista = oLimiteTradingBM.SeleccionarPorFiltro(CodigoRenta, CodigoGrupLimTrader, TipoCargo, CodigoPortafolioSBS, DatosRequest).Tables(0)
        dgLista.DataSource = dtLista
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtLista.Rows.Count) + "');")
    End Sub

End Class

Imports System.Data
Imports Sit.BusinessLayer
Partial Class Modulos_Gestion_Reportes_frmCodigoValor
    Inherits BasePage
    Dim oCodigoValoBM As New CodigoValorBM
#Region "Métodos de la página"
    Sub Modificar(ByVal id As Integer)
        'Session("CV") = codigoValor
        'Session("EstadoCV") = "Modificar"
        Response.Redirect("frmCodigoValorMantenimiento.aspx?id=" & id.ToString() & "&estadoCV=Modificar")
    End Sub
    Sub Eliminar(ByVal id As integer)
        oCodigoValoBM.EliminarCodigoValor(id, DatosRequest)
        CargarGrilla()
    End Sub
    Private Sub CargarGrilla()
        Dim dt As DataTable = Nothing
        dt = oCodigoValoBM.ListarCodigoValor(0, txtcodigovalor.Text, ddlnemonico.SelectedValue, hdemisor.Value, _
                                                             ddlmoneda.SelectedValue, "", ddlsituacion.SelectedValue)._CodigoValorBE()
        dgLista.DataSource = dt
        dgLista.DataBind()
    End Sub
#End Region
#Region "Eventos de la página"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Dim DtTablaMoneda As DataTable
                Dim DtNemonico As DataTable
                Dim oMonedaBM As New MonedaBM
                DtTablaMoneda = oMonedaBM.Listar(DatosRequest).Tables(0)
                DtNemonico = oCodigoValoBM.NemonicoCodigoValor()
                HelpCombo.LlenarComboBox(ddlmoneda, DtTablaMoneda, "CodigoMoneda", "Descripcion", True, "Todos")
                HelpCombo.LlenarComboBox(ddlnemonico, DtNemonico, "CodigoNemonico", "CodigoNemonico", True, "Todos")
                CargarGrilla()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                txtEmisor.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                hdemisor.Value = CType(Session("SS_DatosModal"), String())(0).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim index As Integer = 0
            Dim id As Integer = 0
            If e.CommandName = "Eliminar" Then
                index = Integer.Parse(e.CommandArgument.ToString())
                id = Integer.Parse(dgLista.Rows(index).Cells(7).Text.Trim.Replace("&nbsp;", ""))
                'Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Eliminar(id)
            ElseIf e.CommandName = "Modificar" Then
                index = Integer.Parse(e.CommandArgument.ToString())
                id = Integer.Parse(dgLista.Rows(index).Cells(7).Text.Trim.Replace("&nbsp;", ""))
                Modificar(id)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        'Session("EstadoCV") = "Nuevo"
        'Session("CV") = ""
        Response.Redirect("frmCodigoValorMantenimiento.aspx?estadoCV=Nuevo")
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            'dgLista.DataSource = oCodigoValoBM.ListarCodigoValor(txtcodigovalor.Text, ddlsituacion.SelectedValue, ddlnemonico.SelectedValue, hdemisor.Value, ddlmoneda.SelectedValue)
            'dgLista.DataSource = oCodigoValoBM.ListarCodigoValor(txtcodigovalor.Text, ddlsituacion.SelectedValue, ddlnemonico.SelectedValue, hdemisor.Value, ddlmoneda.SelectedValue)
            'dgLista.DataBind()
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
#End Region
End Class

Imports Sit.BusinessLayer
Imports System.Data
Imports UIUtility

Public Class Modulos_PrevisionPagos_frmBancoCuenta
    Inherits BasePage

    Private Sub CargarCombos(ByVal ddl As DropDownList, ByVal Parametro As Int32)
        Dim tablaListaParametria As New Data.DataTable
        Dim oTipoDocumento As New TipoDocumentoBM
        tablaListaParametria = PrevisionParametriaBM.ListarParametria(Parametro).Tables(0)
        HelpCombo.LlenarComboBox(ddl, tablaListaParametria, "Valor", "Descripcion", False)
    End Sub

    Private Sub CargarGrilla(ByVal strBanco As String, ByVal strEstado As String)
        'gvPagos.DataSource = Nothing
        'gvPagos.DataBind()
        Dim dtblDatos As New DataSet
        dtblDatos = PrevisionParametriaBM.ListarDetalleCuentasCorrientes(strBanco, strEstado)
        gvPagos.DataSource = dtblDatos
        gvPagos.DataBind()
        If ddlEstado.SelectedValue.Equals("I") Then
            gvPagos.Columns(1).Visible = False
        Else
            gvPagos.Columns(1).Visible = True
        End If
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CargarCombos(ddlEstado, 1)
            gvPagos.Columns(2).Visible = False
            CargarGrilla(tbcuotadmF.Text.Trim, ddlEstado.SelectedValue.ToString.Trim)
        End If
    End Sub

    Protected Sub ibIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibIngresar.Click
        Response.Redirect("frmBancoCuentaDetalle.aspx")
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        CargarGrilla(tbcuotadmF.Text.Trim, ddlEstado.SelectedValue.ToString.Trim)
    End Sub

    Protected Sub gvPagos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPagos.PageIndexChanging
        Try
            gvPagos.PageIndex = e.NewPageIndex
            CargarGrilla(tbcuotadmF.Text.Trim, ddlEstado.SelectedValue.ToString.Trim)
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Protected Sub gvPagos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPagos.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ibt1 As ImageButton = e.Row.FindControl("ibModificar")
                Dim ibt2 As ImageButton = e.Row.FindControl("ibEliminar")
                ibt1.CommandArgument = e.Row.RowIndex
                ibt2.CommandArgument = e.Row.RowIndex
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Protected Sub gvPagos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPagos.RowCommand
        Try
            Dim index As String = ""
            If e.CommandArgument = "First" Then
                index = "1"
            ElseIf e.CommandArgument = "Last" Then
                Dim auxInteger As Integer = gvPagos.Rows.Count / 10
                index = auxInteger
            Else
                index = e.CommandArgument
            End If
            Dim row As GridViewRow = gvPagos.Rows(index)
            If e.CommandName = "Modificar" Then
                Session("sCodigo") = gvPagos.DataKeys(index)("Codigo")
                Session("sEntidad") = row.Cells(3).Text.ToString.Trim
                Session("sBanco") = row.Cells(4).Text.ToString.Trim
                Session("sCta") = row.Cells(5).Text.ToString.Trim
                Session("sTipoCuenta") = row.Cells(6).Text.ToString.Trim
                Session("sTipoMoneda") = row.Cells(7).Text.ToString.Trim
                Session("sEstado") = row.Cells(8).Text.ToString.Trim
                Response.Redirect("frmBancoCuentaDetalle.aspx")
            End If
            If e.CommandName = "Eliminar" Then
                If PrevisionParametriaBM.EliminarCuentaCorriente(Convert.ToInt32(gvPagos.DataKeys(index)("Codigo").ToString.Trim), Usuario) = True Then
                    CargarGrilla(tbcuotadmF.Text.Trim, ddlEstado.SelectedValue.ToString.Trim)
                    AlertaJS("Registro eliminado")
                Else
                    AlertaJS("Error al eliminar")
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Protected Sub ibCancelar_Click(sender As Object, e As System.EventArgs) Handles ibCancelar.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub
End Class

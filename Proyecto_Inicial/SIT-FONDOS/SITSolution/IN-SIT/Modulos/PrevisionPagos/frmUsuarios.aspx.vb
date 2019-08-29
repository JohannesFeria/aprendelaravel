Imports Sit.BusinessLayer
Imports System.Data

Public Class Modulos_PrevisionPagos_frmUsuarios
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ListarUsuarios()
        End If
    End Sub

    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnIngresar.Click
        Response.Redirect("frmUsuariosDetalle.aspx?user=" + Usuario)
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        ListarUsuarios()
    End Sub

    Private Sub ListarUsuarios()
        Dim objBM As PrevisionUsuarioBM = New PrevisionUsuarioBM()
        Dim ds As DataTable

        Try
            ds = objBM.ListarUsuarios(tbUsuario.Text.ToString(), ddlEstado.SelectedValue.ToString()).Tables(0)
            
            gvPagos.DataSource = ds
            gvPagos.DataBind()
            EjecutarJS(String.Format("$('#lbContador').text('{0}');", "Registros Encontrados : " + ds.Rows.Count.ToString()))
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Private Sub gvPagos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPagos.RowCommand
        Dim objBM As PrevisionUsuarioBM = New PrevisionUsuarioBM()
        Dim result As Integer

        Select Case e.CommandName
            Case "Modificar"
                Response.Redirect("frmUsuariosDetalle.aspx?cod=" + e.CommandArgument.ToString() + "&user=" + Usuario)
            Case "Eliminar"
                Try
                    result = objBM.EliminarUsuario(e.CommandArgument.ToString())
                    If (result = 1) Then AlertaJS("Registro Inactivado correctamente.") Else AlertaJS("Ocurrió un error en el sistema al intentar eliminar el registro.")
                    ListarUsuarios()
                Catch ex As Exception
                    AlertaJS("Ocurrió un error en el sistema.")
                End Try
        End Select
    End Sub

    Private Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../Bienvenida.aspx", False)
    End Sub

    Protected Sub gvPagos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPagos.PageIndexChanging
        gvPagos.PageIndex = e.NewPageIndex
        ListarUsuarios()
    End Sub

End Class

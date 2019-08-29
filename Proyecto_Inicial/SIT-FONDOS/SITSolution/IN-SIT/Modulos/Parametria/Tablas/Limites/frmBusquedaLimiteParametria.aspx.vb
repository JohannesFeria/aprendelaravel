Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Limites_frmBusquedaLimiteParametria
    Inherits BasePage
    Sub Buscar(ByVal TipoBusqueda As String)
        Dim oLimParametria As New LimiteParametriaBM
        If TipoBusqueda = "" Then
            AlertaJS("Seleccione un tipo de Grupo.", "ddltipogrupo.focus();")
        ElseIf TipoBusqueda = "0" Then
            ''Grupo por tipo de moneda
            Dim dt As DataTable = oLimParametria.GrupoPorTipoMoneda_Buscar(txtCodigo.Text, txtdescripcion.Text)
            dgLista.DataSource = dt
            dgLista.DataBind()
        ElseIf TipoBusqueda = "1" Then
            'Grupo por clase de Instrumento
            Dim dt As DataTable = oLimParametria.GrupoClaseInstrumento_Buscar(txtCodigo.Text, txtdescripcion.Text)
            dgLista.DataSource = dt
            dgLista.DataBind()
        ElseIf TipoBusqueda = "2" Then
            'Grupo por Entidad
            Dim dt As DataTable = oLimParametria.GrupoPorEntidad_Buscar(txtCodigo.Text, txtdescripcion.Text)
            dgLista.DataSource = dt
            dgLista.DataBind()
        ElseIf TipoBusqueda = "3" Then
            'Grupo por Derivados
            Dim dt As DataTable = oLimParametria.GrupoPorDerivados_Buscar(txtCodigo.Text, txtdescripcion.Text)
            dgLista.DataSource = dt
            dgLista.DataBind()
        ElseIf TipoBusqueda = "4" Then
            'Grupo por Nemonico
            Dim dt As DataTable = oLimParametria.GrupoPorNemonico_Buscar(txtCodigo.Text, txtdescripcion.Text)
            dgLista.DataSource = dt
            dgLista.DataBind()
        ElseIf TipoBusqueda = "5" Then
            'Grupo por Calificacion
            Dim dt As DataTable = oLimParametria.GrupoPorCalificacion_Buscar(txtCodigo.Text, txtdescripcion.Text)
            dgLista.DataSource = dt
            dgLista.DataBind()
        End If
    End Sub
    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Dim index As Integer
        If e.CommandName = "Modificar" Then
            index = CInt(e.CommandArgument)
            Session("Codigo") = dgLista.Rows(index).Cells(1).Text
            Session("TipoGrupo") = ddltipogrupo.SelectedValue
            If ddltipogrupo.SelectedValue = "0" Then
                Response.Redirect("frmGrupoPorTipoMoneda.aspx")
            ElseIf ddltipogrupo.SelectedValue = "1" Then
                Response.Redirect("frmGrupoPorClaseInstrumento.aspx")
            ElseIf ddltipogrupo.SelectedValue = "2" Then
                Response.Redirect("frmGrupoPorEntidad.aspx")
            ElseIf ddltipogrupo.SelectedValue = "3" Then
                Response.Redirect("frmGrupoPorDerivados.aspx")
            ElseIf ddltipogrupo.SelectedValue = "4" Then
                Response.Redirect("frmGrupoPorNemonico.aspx")
            ElseIf ddltipogrupo.SelectedValue = "5" Then
                Response.Redirect("frmGrupoPorCalificacion.aspx")
            End If
        End If
    End Sub
    Protected Sub btnbuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnbuscar.Click
        Buscar(ddltipogrupo.SelectedValue)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("TipoGrupo") Is Nothing Then
            ddltipogrupo.SelectedValue = Session("TipoGrupo")
            Buscar(ddltipogrupo.SelectedValue)
            Session("TipoGrupo") = Nothing
        End If
    End Sub
    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Session("Codigo") = Nothing
        Session("TipoGrupo") = ddltipogrupo.SelectedValue
        If ddltipogrupo.SelectedValue = "" Then
            AlertaJS("Seleccione un tipo de grupo.", "ddltipogrupo.focus();")
        ElseIf ddltipogrupo.SelectedValue = "0" Then
            Response.Redirect("frmGrupoPorTipoMoneda.aspx")
        ElseIf ddltipogrupo.SelectedValue = "1" Then
            Response.Redirect("frmGrupoPorClaseInstrumento.aspx")
        ElseIf ddltipogrupo.SelectedValue = "2" Then
            Response.Redirect("frmGrupoPorEntidad.aspx")
        ElseIf ddltipogrupo.SelectedValue = "3" Then
            Response.Redirect("frmGrupoPorDerivados.aspx")
        ElseIf ddltipogrupo.SelectedValue = "4" Then
            Response.Redirect("frmGrupoPorNemonico.aspx")
        ElseIf ddltipogrupo.SelectedValue = "5" Then
            Response.Redirect("frmGrupoPorCalificacion.aspx")
        End If
    End Sub
End Class

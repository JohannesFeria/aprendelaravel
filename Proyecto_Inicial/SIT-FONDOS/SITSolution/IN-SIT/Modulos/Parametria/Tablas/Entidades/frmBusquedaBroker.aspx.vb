Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Entidades_frmBusquedaBroker
    Inherits BasePage

#Region "/* Métodos de la página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarSituacion()
                CargarTipoTramo()
                CargarTramoBroker(txtCodigoEntidad.Text.Trim(), txtDescripcion.Text.Trim(), ddlSituacion.SelectedValue, ddlTipoTramo.SelectedValue)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmBroker.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try        
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarTramoBroker(txtCodigoEntidad.Text.Trim(), txtDescripcion.Text.Trim(), ddlSituacion.SelectedValue, ddlTipoTramo.SelectedValue)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            Dim btn As ImageButton
            If e.Row.RowType = DataControlRowType.DataRow Then
                btn = DirectCast(e.Row.FindControl("ibEliminar"), ImageButton)
                btn.Attributes.Add("onclick", ConfirmJS(Constantes.M_STR_MENSAJE_PREGUNTA_ELIMINAR_ENTIDAD))
                If (e.Row.Cells(9).Text.Trim = "A") Then
                    e.Row.Cells(9).Text = "Activo"
                Else
                    e.Row.Cells(9).Text = "Inactivo"
                End If
                If (e.Row.Cells(4).Text = "PRINCIPAL") Then
                    e.Row.Cells(6).Text = "-"
                    e.Row.Cells(7).Text = "-"
                ElseIf (e.Row.Cells(4).Text = "AGENCIA") Then
                    If e.Row.Cells(10).Text = "P" Then
                        e.Row.Cells(6).Text = "-"
                        e.Row.Cells(7).Text = "-"
                    ElseIf e.Row.Cells(10).Text = "V" Then
                        If e.Row.Cells(7).Text = "0" Then
                            e.Row.Cells(7).Text = ""
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Response.Redirect(String.Format("frmBroker.aspx?Tramo={0}", e.CommandArgument))
                Case "Eliminar"
                    eliminar(e.CommandArgument)
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarTramoBroker(txtCodigoEntidad.Text.Trim(), txtDescripcion.Text.Trim(), ddlSituacion.SelectedValue, ddlTipoTramo.SelectedValue)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try        
    End Sub

#End Region

#Region "/* Métodos personalizados */"

    Private Sub CargarSituacion()
        Dim obm As New ParametrosGeneralesBM
        ddlSituacion.DataSource = obm.Listar("Situación", Me.DatosRequest)
        ddlSituacion.DataValueField = "Valor"
        ddlSituacion.DataTextField = "Nombre"
        ddlSituacion.DataBind()
        ddlSituacion.Items.Insert(0, New ListItem("--TODOS--", ""))
    End Sub

    Private Sub CargarTipoTramo()
        Dim obm As New ParametrosGeneralesBM
        ddlTipoTramo.DataSource = obm.Listar("TIPOTRAMO", Me.DatosRequest)
        ddlTipoTramo.DataValueField = "Valor"
        ddlTipoTramo.DataTextField = "Nombre"
        ddlTipoTramo.DataBind()
        ddlTipoTramo.Items.Insert(0, New ListItem("--TODOS--", ""))
    End Sub

    Private Sub CargarTramoBroker(ByVal CodigoEntidad As String, ByVal descripcion As String, ByVal situacion As String, ByVal tipotramo As String)
        Dim obm As New EntidadBM
        Dim dt As DataTable = obm.ListarTramoBroker(CodigoEntidad, descripcion, situacion, tipotramo)
        dgLista.DataSource = dt
        dgLista.DataBind()
        EjecutarJS("$('#" + Me.lblContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt.Rows.Count) + "')")
    End Sub

    Private Sub eliminar(ByVal tramo As String)
        Dim oEntidadBM As New EntidadBM
        oEntidadBM.EliminarTramoBroker(tramo, DatosRequest)
        AlertaJS("Dato Eliminado Correctamente")
        CargarTramoBroker(txtCodigoEntidad.Text.Trim(), txtDescripcion.Text.Trim(), ddlSituacion.SelectedValue, ddlTipoTramo.SelectedValue)
    End Sub

#End Region

End Class

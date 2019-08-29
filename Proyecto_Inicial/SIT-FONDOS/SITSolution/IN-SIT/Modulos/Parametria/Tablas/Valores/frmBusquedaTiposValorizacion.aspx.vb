Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaTiposValorizacion
    Inherits BasePage


#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarDatosGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Dim TipoBCR As String = ddlSituacion.SelectedValue
            Response.Redirect("frmTiposValorizacion.aspx?TipoBCR=" & TipoBCR)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgBCRUnico_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgBCRUnico.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminarUnicos"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgBCRUnico_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgBCRUnico.PageIndexChanging
        Try
            dgBCRUnico.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

#End Region

#Region " /* Funciones Modificar */"

    Public Sub ModificarBCRSeriado(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim codigo As String = e.CommandArgument
            Dim TipoBCR As String = ddlSituacion.SelectedValue
            Response.Redirect(String.Format("frmTiposValorizacion.aspx?TipoBCR={0}&cod={1}", TipoBCR, codigo))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub

    Public Sub ModificarBCRUnico(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim codigo As String = e.CommandArgument
            Dim TipoBCR As String = ddlSituacion.SelectedValue
            Response.Redirect(String.Format("frmTiposValorizacion.aspx?TipoBCR={0}&codUnico={1}", TipoBCR, codigo))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Eliminar */"

    Public Sub EliminarBCRSeriado(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oTipoValorizacionBM As New TipoValorizacionBM
            Dim codigo As String = e.CommandArgument
            oTipoValorizacionBM.EliminarBCRSeriado(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub

    Public Sub EliminarBCRUnico(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oTipoValorizacionBM As New TipoValorizacionBM
            Dim codigo As String = e.CommandArgument
            oTipoValorizacionBM.EliminarBCRUnico(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarGrilla()
        Dim oTipoValorizacionBM As New TipoValorizacionBM
        Dim dtblDatos As DataTable

        If ddlSituacion.SelectedValue = "Seriado" Then
            dtblDatos = oTipoValorizacionBM.SeleccionarPorFiltroBcrSeriado(Me.tbDescripcion.Text, Me.DatosRequest)
            Me.dgLista.DataSource = dtblDatos
            Me.dgLista.DataBind()
        Else
            dtblDatos = oTipoValorizacionBM.SeleccionarPorFiltroBcrUnico(Me.tbDescripcion.Text, Me.DatosRequest).BCRUnico
            Me.dgBCRUnico.DataSource = dtblDatos
            dgBCRUnico.DataBind()
        End If

        EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")

    End Sub

    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub CargarDatosGrilla()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
    End Sub

#End Region

    Private Sub ddlSituacion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSituacion.SelectedIndexChanged
        Try
            Dim dt As New DataTable
            If (ddlSituacion.SelectedValue = "Seriado") Then
                dgLista.Visible = True
                dgBCRUnico.Visible = False
                dgBCRUnico.DataSource = Nothing
                dgBCRUnico.DataBind()
            Else
                dgBCRUnico.Visible = True
                dgLista.Visible = False
                dgLista.DataSource = Nothing
                dgLista.DataBind()
            End If
            EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar la situación")
        End Try
    End Sub

End Class

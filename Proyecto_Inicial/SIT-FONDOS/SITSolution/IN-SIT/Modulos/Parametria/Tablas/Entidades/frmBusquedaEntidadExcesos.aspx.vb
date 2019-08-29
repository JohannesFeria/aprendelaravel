Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data


Partial Class Modulos_Parametria_Tablas_Entidades_frmBusquedaEntidadExcesos
    Inherits BasePage

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                Buscar()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.Cells(4).Text.Trim = "A" Then
                    e.Row.Cells(4).Text = "Activo"
                Else
                    e.Row.Cells(4).Text = "Inactivo"
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ibIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibIngresar.Click
        Try
            Response.Redirect("frmEntidadExcesos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try
    End Sub

    Protected Sub ibBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Protected Sub ibCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub

#End Region

#Region "/* Metodos Personalizados */"

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmEntidadExcesos.aspx?codigo=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oEntidadBM As New EntidadBM
            Dim strCodigo As String

        strCodigo = e.CommandArgument.ToString()

        oEntidadBM.EliminarExcesosBroker(strCodigo, DatosRequest)

            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Private Sub Buscar()
        dgLista.PageIndex = 0
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oEntidadBM As New EntidadBM
        Dim strSituacion As String
        strSituacion = IIf(Me.ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlSituacion.SelectedValue).ToString()
        Dim dtblDatos As DataTable = oEntidadBM.SeleccionarExcesosBroker(tbCodigoEntidad.Text.Trim(), tbDescripcion.Text.TrimStart.TrimEnd, strSituacion)
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos.Rows.Count) + "')")
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)

        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)

    End Sub

    Private Sub LimpiarConsulta()
        Me.tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlSituacion.SelectedIndex = 0
    End Sub

    Private Sub MostrarMensajeConfirmacion(ByVal oDataGridItem As DataGridItem)
        Dim imgEliminar As ImageButton
        If oDataGridItem.ItemType = ListItemType.Item Or oDataGridItem.ItemType = ListItemType.AlternatingItem Then
            imgEliminar = DirectCast(oDataGridItem.FindControl("ibEliminar"), ImageButton)
            imgEliminar.Attributes.Add("onclick", ConfirmJS(Constantes.M_STR_MENSAJE_PREGUNTA_ELIMINAR_ENTIDAD))
        End If
    End Sub

#End Region

End Class

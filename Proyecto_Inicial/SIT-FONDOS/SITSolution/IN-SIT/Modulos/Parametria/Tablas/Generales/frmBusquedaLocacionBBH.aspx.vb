Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaLocacionBBH
    Inherits BasePage

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Ingresar()
        Catch ex As Exception
            AlertaJS("Ocurrio un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Cancelar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            ActualizarIndice(e.NewPageIndex)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            MostrarMensajeConfirmacion(e.Row)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

#End Region

#Region "/* Metodos Personalizados */"

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmLocacionBBH.aspx?codigo=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oLocacionBBHBM As New LocacionBBHBM
            Dim strCodigoLocation As String
            strCodigoLocation = e.CommandArgument.ToString().Split(","c)(0)
            oLocacionBBHBM.Eliminar(strCodigoLocation, Me.DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            Buscar()
        End If
    End Sub

    Private Sub Ingresar()
        Response.Redirect("frmLocacionBBH.aspx")
    End Sub

    Private Sub Cancelar()
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub Buscar()
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub

    Private Sub ActualizarIndice(ByVal nuevoIndice As Integer)
        dgLista.PageIndex = nuevoIndice
        CargarGrilla()
    End Sub

    Private Sub CargarGrilla()
        Dim oLocacionBBHBM As New LocacionBBHBM
        Dim strSituacion As String, strLocation As String
        strSituacion = IIf(Me.ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, Me.ddlSituacion.SelectedValue).ToString()
        strLocation = tbDescripcion.Text
        Dim dtblDatos As DataTable = oLocacionBBHBM.SeleccionarPorFiltro(strLocation, strSituacion, Me.DatosRequest).LocacionBBH
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        Me.dgLista.PageIndex = 0
        EjecutarJS("$('#" + lblContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")        
    End Sub

    Private Sub CargarCombos()

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub MostrarMensajeConfirmacion(ByVal oDataGridItem As GridViewRow)
        Dim imgEliminar As ImageButton
        If oDataGridItem.RowType = ListItemType.Item Or oDataGridItem.RowType = ListItemType.AlternatingItem Then
            imgEliminar = DirectCast(oDataGridItem.FindControl("ibEliminar"), ImageButton)
            imgEliminar.Attributes.Add("onclick", ConfirmJS(Constantes.M_STR_MENSAJE_PREGUNTA_ELIMINAR_ENTIDAD))
        End If
    End Sub

#End Region

End Class

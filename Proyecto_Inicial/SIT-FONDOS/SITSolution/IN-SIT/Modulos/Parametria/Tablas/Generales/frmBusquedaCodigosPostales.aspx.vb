Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports UIUtility

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaCodigosPostales
    Inherits BasePage

#Region "/* Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarCombos()
            CargarGrilla()
        End If
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Response.Redirect("frmCodigoPostal.aspx")
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            dgLista.PageIndex = 0
            CargarGrilla()
            If dgLista.Rows.Count = 0 Then
                AlertaJS("No se encontraron Registros")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub dgLista_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub

#End Region

#Region " /* Funciones Modificar */"
    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmCodigoPostal.aspx?cod=" & e.CommandArgument)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub
#End Region

#Region " /* Funciones Eliminar */"
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oCodigoPostalBM As New CodigoPostalBM
            Dim codigo As String = e.CommandArgument
            oCodigoPostalBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub
#End Region

#Region " /* Funciones Personalizadas*/"
    Private Sub CargarGrilla()
        Dim oCodigoPostalBM As New CodigoPostalBM
        Dim dtblDatos As DataTable = oCodigoPostalBM.SeleccionarPorFiltro(tbCodigo.Text.Trim, tbDescripcion.Text.TrimStart.TrimEnd, IIf(Me.ddlSituacion.SelectedValue = "Todos", "", ddlSituacion.SelectedValue), DatosRequest).Tables(0)
        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub

    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Public Sub LimpiarConsulta()
        tbCodigo.Text = ""
        tbDescripcion.Text = ""
        ddlSituacion.SelectedIndex = 0
    End Sub
#End Region

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la carga de datos de la grilla")
        End Try
    End Sub
End Class

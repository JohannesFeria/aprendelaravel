Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaRating
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not IsPostBack Then
                'Se agrego faltaba tipo rating
                Dim oParametrosGenerales As New ParametrosGeneralesBM
                Dim tablaRating As New DataTable

                tablaRating = oParametrosGenerales.Listar("RatingPar", DatosRequest)
                HelpCombo.LlenarComboBox(Me.ddlTipoRating, tablaRating, "Valor", "Nombre", True)

                CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub CargarGrilla()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dtblDatos As DataTable

        Dim strTipoRating As String = IIf(ddlTipoRating.SelectedValue.ToString.Equals("--SELECCIONE--"), "", ddlTipoRating.SelectedValue.ToString)
        dtblDatos = oParametrosGenerales.ListarRating(tbDescripcion.Text.Trim, _
                                        "", strTipoRating, "", DatosRequest).Tables(0)

        dgLista.DataSource = dtblDatos
        dgLista.DataBind()

        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub

    Public Sub ModificarRating(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim valor As String = e.CommandArgument
            Response.Redirect(String.Format("frmRating.aspx?accion={0}&valor={1}", "upd", valor))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub

    Public Sub EliminarRating(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oParametrosGenerales As New ParametrosGeneralesBM
            Dim valor As String = e.CommandArgument
            oParametrosGenerales.EliminarRating(valor, DatosRequest)
            CargarGrilla()
            AlertaJS("El registro se eliminó correctamente")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Dim oParametrosGenerales As New ParametrosGeneralesBM
            Response.Redirect(String.Format("frmRating.aspx?accion={0}", "new"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Response.Redirect("frmRatingImportar.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Actualización Masiva")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la carga de datos en la Grilla")
        End Try
    End Sub

    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub
End Class

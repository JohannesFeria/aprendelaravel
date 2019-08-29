Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Web
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaCalificacionInstrumentos
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Session("CodigoCalificacion") = Nothing
                CargarCombos()
                CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            dgLista.PageIndex = 0
            CargarGrilla()
            If dgLista.Rows.Count = 0 Then
                AlertaJS("No se encontraron Registros")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmCalificacionInstrumentos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar a la página")
        End Try        
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de la página")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarGrilla()
        Dim oCalificacionInstrumentoBM As New CalificacionInstrumentoBM
        Dim dtblDatos As DataTable = oCalificacionInstrumentoBM.SeleccionarPorFiltro(tbDescripcion.Text, IIf(ddlPlazo.SelectedValue = "Todos", "", ddlPlazo.SelectedValue), IIf(ddlSituacion.SelectedValue = "Todos", "", ddlSituacion.SelectedValue), DatosRequest).Tables(0)
        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub

    Public Sub CargarCombos()
        Dim tablaPlazo As New Data.DataTable
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaPlazo = oParametrosGenerales.ListarCalifInstr(DatosRequest)
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)

        HelpCombo.LlenarComboBoxBusquedas(ddlPlazo, tablaPlazo, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBoxBusquedas(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub
#End Region

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim codigo As String = e.CommandArgument
            Session("CodigoCalificacion") = codigo
            Response.Redirect("frmCalificacionInstrumentos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar el registro")
        End Try
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oCalificacionInstrumentoBM As New CalificacionInstrumentoBM
            Dim codigo As String = e.CommandArgument
            oCalificacionInstrumentoBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception            
            AlertaJS("Ocurrió un error al Elminar el registro")
        End Try
    End Sub

End Class

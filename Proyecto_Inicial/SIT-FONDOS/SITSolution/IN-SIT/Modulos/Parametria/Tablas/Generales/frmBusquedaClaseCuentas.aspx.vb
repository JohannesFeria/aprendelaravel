Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_General_frmBusquedaClaseCuentas
    Inherits BasePage

#Region " /* Funciones Modificar */"

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmClaseCuenta.aspx?codigo=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub
#End Region

#Region " /* Funciones Eliminar */"
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim oClaseCuentaBM As New ClaseCuentaBM
        Dim codigo As String

        Try
            codigo = e.CommandArgument.ToString()
            oClaseCuentaBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub
#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarPagina()
        CargarCombos()
    End Sub

    Private Sub CargarGrilla()
        Dim oClaseCuentaBM As New ClaseCuentaBM
        Dim strSituacion As String

        If ddlSituacion.SelectedItem.Text.Equals(Constantes.M_STR_TEXTO_TODOS) Then
            strSituacion = String.Empty
        Else
            strSituacion = Me.ddlSituacion.SelectedValue
        End If

        Dim dtblDatos As DataTable = oClaseCuentaBM.SeleccionarPorFiltro(Me.txtCodigo.Text.Trim, Me.txtDescripcion.Text.TrimStart.TrimEnd, strSituacion, DatosRequest).Tables(0)
        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "')")
    End Sub

    Private Sub CargarCombos()
        Dim dtSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        dtSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, dtSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub LimpiarConsulta()
        txtCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
        txtDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlSituacion.SelectedIndex = 0
    End Sub

#End Region

#Region "/* Metodos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
                CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarGrilla()
            If dgLista.Rows.Count = 0 Then
                AlertaJS("No se encontraron Registros")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmClaseCuenta.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al intentar cancelar el proceso")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                DirectCast(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
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

#End Region

End Class

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Contabilidad_frmBusquedaMatriz
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Dim tablaSituacion As New DataTable
                Dim oParametrosGenerales As New ParametrosGeneralesBM
                tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
            HelpCombo.LlenarComboBoxBusquedas(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
                CargarDatosGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

#Region " /* Metodos de Pagina */ "

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmMatriz.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

#End Region

#Region " /* Funciones Modificar */"

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim codigo As String = e.CommandArgument
            Response.Redirect("frmMatriz.aspx?cod=" & codigo)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Eliminar */"
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oMatrizContableBM As New MatrizContableBM
            Dim codigo As String = e.CommandArgument
            oMatrizContableBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarDatosGrilla()
        CargarGrilla()
        If dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oMatrizContableBM As New MatrizContableBM
        Dim situacion As String
        Dim descripcion As String

        situacion = ddlSituacion.SelectedValue.Substring(0, 1)
        If situacion = "T" Then
            situacion = ""
        End If
        descripcion = tbDescripcion.Text.Trim.ToUpper
        Dim dtblDatos As DataTable = oMatrizContableBM.SeleccionarPorFiltros(descripcion, situacion, "").Tables(0)
        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub
#End Region

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("btnEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar los datos en la Grilla")
        End Try
    End Sub
End Class

Option Strict On
Option Explicit On

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaGrupoLimiteTrader
    Inherits BasePage

#Region " /* Metodos de Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Ingresar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Cancelar()
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

#End Region

#Region " /* Funciones Modificar */"

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmGrupoLimiteTrader.aspx?cod=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Eliminar */"

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oGrupoLimiteTraderBM As New GrupoLimiteTraderBM
            Dim decCodigo As Decimal
            decCodigo = CDec(e.CommandArgument.ToString())
            oGrupoLimiteTraderBM.Eliminar(decCodigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarPagina()
        CargarCombos()
        Buscar()
    End Sub

    Private Sub Ingresar()
        Response.Redirect("frmGrupoLimiteTrader.aspx")
    End Sub

    Private Sub Cancelar()
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub Buscar()
        dgLista.PageIndex = 0
        CargarGrilla()
        If dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub

    Private Sub ActualizarIndice(ByVal nuevoIndice As Integer)
        dgLista.PageIndex = nuevoIndice
        CargarGrilla()
    End Sub

    Private Sub CargarGrilla()
        Dim oGrupoLimiteTraderBM As New GrupoLimiteTraderBM
        Dim strSituacion As String

        strSituacion = IIf(ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlSituacion.SelectedValue).ToString()
        Dim dtblDatos As DataTable = oGrupoLimiteTraderBM.SeleccionarPorFiltro(tbDescripcion.Text.TrimStart.TrimEnd, strSituacion).Tables(0)

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
        tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlSituacion.SelectedIndex = 0
    End Sub

    Private Sub MostrarMensajeConfirmacion(ByVal oDataGridItem As GridViewRow)
        Dim imgEliminar As ImageButton

        If oDataGridItem.RowType = DataControlRowType.DataRow Then
            imgEliminar = DirectCast(oDataGridItem.FindControl("ibEliminar"), ImageButton)
            imgEliminar.Attributes.Add("onclick", ConfirmJS("¿Confirmar la eliminación del registro?"))
        End If
    End Sub

#End Region

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            ActualizarIndice(e.NewPageIndex)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            MostrarMensajeConfirmacion(e.Row)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la carga de datos de la Grilla")
        End Try
    End Sub
End Class

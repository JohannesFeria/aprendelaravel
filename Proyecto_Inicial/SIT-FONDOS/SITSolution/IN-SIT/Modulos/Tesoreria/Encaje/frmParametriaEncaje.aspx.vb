Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports UIUtility
Imports System.Data

Partial Class Modulos_Valorizacion_y_Custodia_Encaje_frmParametriaEncaje
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                cargargrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmValorParametroEncaje.aspx?cod=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar el registro")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de la Página")
        End Try
    End Sub

    Public Sub cargargrilla()
        Dim oencajeBM As New EncajeBM
        Dim dtblDatos As DataTable = oencajeBM.ListarParametros(DatosRequest).Tables(0)
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        Me.lbContador.Text = MostrarResultadoBusqueda(dtblDatos)
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            cargargrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la páginación")
        End Try
    End Sub

End Class

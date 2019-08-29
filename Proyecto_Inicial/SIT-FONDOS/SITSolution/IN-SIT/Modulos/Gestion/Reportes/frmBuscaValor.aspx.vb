Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Text
Imports System.Data

Partial Class Modulos_Gestion_Reportes_frmBuscaValor
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            ObtenerDatosBusqueda()
            btnCancelar.Attributes.Add("onclick", "window.close();")
        End If
    End Sub


#Region " /* Métodos de Búsqueda */ "

    Private Function BuscarValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String) As DataTable

        Return New ValoresBM().InstrumentosBuscarPorFiltro(sbs, isin, mnemonico, DatosRequest).Tables(0)

    End Function

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Function ObtenerDatosBusqueda() As Boolean
        Dim sbs As String = IIf(Request.QueryString("vSBS") Is Nothing, "", Request.QueryString("vSBS"))
        Dim isin As String = IIf(Request.QueryString("vISIN") Is Nothing, "", Request.QueryString("vISIN"))
        Dim mnemonico As String = IIf(Request.QueryString("vMnemonico") Is Nothing, "", Request.QueryString("vMnemonico"))

        Dim dtblDatos As DataTable = BuscarValores(isin, sbs, mnemonico)
        dgLista.DataSource = dtblDatos : Me.dgLista.DataBind()
        lbContador.Text = UIUtility.MostrarResultadoBusqueda(dtblDatos)

        Return True
    End Function

#End Region

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        If e.CommandName = "_seleccionar" Then
            Dim datos As String() = New String(1) {}
            datos(0) = e.CommandArgument.ToString()

            If Session("SS_DatosModal") Is Nothing Then
                Session.Remove("SS_DatosModal")
            End If

            Session("SS_DatosModal") = datos
            EjecutarJS("window.close();")
        End If
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Dim lkbSeleccionar As LinkButton
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim dr As DataRowView = CType(e.Row.DataItem, DataRowView)

            lkbSeleccionar = DirectCast(e.Row.FindControl("lkbSeleccionar"), LinkButton)
            lkbSeleccionar.CommandArgument = dr("Código SBS")
        End If
    End Sub
End Class

Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Text
Imports System.Data

Partial Class Modulos_Valorizacion_y_Custodia_Valorizacion_frmBusquedaInstrumentos
    Inherits BasePage
    Private TipoInstrumento As String
    Private isin As String
    Private mnemonico As String

#Region " /* Eventos de Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            ObtenerDatosBusqueda()
            'CargarGrilla(isin, TipoInstrumento, mnemonico)
        End If
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff';")
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5f5f5';this.style.cursor='hand'")
        End If
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        ObtenerDatosBusqueda()
    End Sub

#End Region

#Region " /* Métodos de Búsqueda */ "

    Private Function BuscarValores(ByVal isin As String, ByVal TipoInstrumento As String, ByVal mnemonico As String) As DataTable
        Return New ValoresBM().InstrumentosBuscarPorFiltroKardex(TipoInstrumento, isin, mnemonico, DatosRequest).Tables(0)
    End Function

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub ObtenerDatosBusqueda()
        TipoInstrumento = IIf(Request.QueryString("vSBS") = "Todos", "", Request.QueryString("vSBS"))
        isin = IIf(Request.QueryString("vISIN") Is Nothing, "", Request.QueryString("vISIN"))
        mnemonico = IIf(Request.QueryString("vMnemonico") Is Nothing, "", Request.QueryString("vMnemonico"))
        Dim dtblDatos As DataTable = BuscarValores(isin, TipoInstrumento, mnemonico)
        dgLista.DataSource = dtblDatos : Me.dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")
    End Sub

    Private Sub ReturnArgumentShowDialogPopup(ByVal isin As String, ByVal mnemonico As String, ByVal sbs As String, ByVal descripcion As String)
        Dim arraySesiones As String() = New String(2) {}
        arraySesiones(0) = isin.ToString.Trim
        arraySesiones(1) = mnemonico.ToString.Trim
        arraySesiones(2) = sbs.ToString.Trim
        Session("SS_DatosModal") = arraySesiones
    End Sub

    Public Sub SeleccionarISIN(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String = e.CommandArgument
        ReturnArgumentShowDialogPopup(strcadena.Split(",").GetValue(0), strcadena.Split(",").GetValue(1), strcadena.Split(",").GetValue(2), strcadena.Split(",").GetValue(3))
        EjecutarJS("Cerrar();")
    End Sub

#End Region

End Class

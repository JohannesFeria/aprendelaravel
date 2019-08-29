Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data

Partial Class Modulos_Parametria_AdministracionValores_frmBusquedaTipoInstrumento
    Inherits BasePage

#Region " /* Eventos de Página */ "
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        dgLista.PageIndex = 0
        ObtenerDatosBusqueda()
    End Sub

    Private Sub ActualizarIndice(ByVal nuevoIndice As Integer)
        dgLista.PageIndex = nuevoIndice
        ObtenerDatosBusqueda()
    End Sub

#End Region

#Region " /* Métodos de Búsqueda */ "

    Private Function BuscarValores(ByVal Codigo As String, ByVal Descripcion As String, ByVal codigoTipoRenta As String) As DataTable
        Dim dtblDatos As DataTable
        Dim dvw As DataView

        'Traer Todos los datos (Solo la primera vez)
        If ViewState("Datos") Is Nothing Then
            dtblDatos = New TipoInstrumentoBM().SeleccionarPorFiltroValores(codigoTipoRenta, DatosRequest).Tables(0)
            ViewState("Datos") = dtblDatos
        Else
            dtblDatos = CType(ViewState("Datos"), DataTable)
        End If
        'Filtrar Segun criterio
        dvw = dtblDatos.DefaultView

        If Codigo.ToString() <> String.Empty And Descripcion.ToString <> String.Empty Then
            dvw.RowFilter = "CodigoTipoInstrumentoSBS like '" + Codigo + "%' and Descripcion like '" + Descripcion + "%'"
        ElseIf Codigo.ToString() <> String.Empty And Descripcion.ToString = String.Empty Then
            dvw.RowFilter = "CodigoTipoInstrumentoSBS like '" + Codigo + "%'"
        ElseIf Codigo.ToString() = String.Empty And Descripcion.ToString <> String.Empty Then
            dvw.RowFilter = "Descripcion like '" + Descripcion + "%'"
        ElseIf Codigo.ToString() = String.Empty And Descripcion.ToString = String.Empty Then
            dvw = dtblDatos.DefaultView
        End If
        'Ordenar por codigo
        dvw.Sort = "CodigoTipoInstrumentoSBS ASC"
        Return dtblDatos
    End Function

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Function ObtenerDatosBusqueda() As Boolean
        Dim codigoTipoRenta As String = ""
        If Not Request.QueryString("tipoRenta") Is Nothing Then
            codigoTipoRenta = Request.QueryString("tipoRenta")
        End If
        Dim dtblDatos As DataTable = BuscarValores(txtCodigo.Text.Trim, txtDescripcion.Text.Trim, codigoTipoRenta)
        dgLista.DataSource = dtblDatos : dgLista.DataBind()

        Return True
    End Function

    Private Function ReturnArgumentShowDialogPopup(ByVal codigoSBS As String, ByVal codigo As String, ByVal CodigoClaseInstrumento As String) As Boolean
        Dim arraySesiones As String() = New String(3) {}

        arraySesiones(0) = codigoSBS
        arraySesiones(1) = codigo
        arraySesiones(2) = CodigoClaseInstrumento

        If (Not Session("SS_DatosModal") Is Nothing) Then Session.Remove("SS_DatosModal")

        Session("SS_DatosModal") = arraySesiones

        EjecutarJS("window.close();")
        Return Nothing
    End Function

    Public Sub Seleccionar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String = e.CommandArgument
        ReturnArgumentShowDialogPopup(strcadena.Split(",").GetValue(0), strcadena.Split(",").GetValue(1), strcadena.Split(",").GetValue(2))
    End Sub
#End Region

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        ActualizarIndice(e.NewPageIndex)
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff';")
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5f5f5';this.style.cursor='hand'")
        End If
    End Sub
End Class

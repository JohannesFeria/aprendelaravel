Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Text
Imports System.Data

Partial Class Modulos_Valorizacion_y_Custodia_Custodia_BuscarInstrumento
    Inherits BasePage


#Region " /* Eventos de Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                ObtenerDatosBusqueda()
                Session("SS_DatosModal") = Nothing
                'ibCancelar.Attributes.Add("onclick", "CloseWindow();")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub

#End Region

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
        lblContador.Text = UIUtility.MostrarResultadoBusqueda(dtblDatos)

        Return True

    End Function

    'Private Function ReturnArgumentShowDialogPopup(ByVal isin As String, ByVal mnemonico As String, ByVal sbs As String, ByVal descripcion As String, ByVal Moneda As String) As Boolean

    '    Dim script As New StringBuilder
    '    With script
    '        .Append("<script>")
    '        .Append("   var setArgument = window.dialogArguments;")
    '        .Append("   setArgument.ISIN = '" + isin + "';")
    '        .Append("   setArgument.SBS = '" + sbs + "';")
    '        .Append("   setArgument.MNEMONICO = '" + mnemonico + "';")
    '        .Append("   setArgument.DESCRIPCION = '" + descripcion + "';")
    '        .Append("   setArgument.MONEDA = '" + Moneda + "';")
    '        .Append("   window.close()")
    '        .Append("</script>")
    '    End With

    '    Page.RegisterStartupScript(New Guid().ToString(), script.ToString())
    'End Function

    'Public Sub SeleccionarISIN(ByVal sender As Object, ByVal e As CommandEventArgs)

    '    Dim strcadena As String = e.CommandArgument
    '    ReturnArgumentShowDialogPopup(strcadena.Split(",").GetValue(0), strcadena.Split(",").GetValue(1), strcadena.Split(",").GetValue(2), strcadena.Split(",").GetValue(3), strcadena.Split(",").GetValue(4))

    'End Sub


#End Region

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            ObtenerDatosBusqueda()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName = "Seleccionar" Then
                Dim Index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim Row As GridViewRow = dgLista.Rows(Index)
                Dim arraySesiones As String() = New String(4) {}
                arraySesiones(0) = Row.Cells(1).Text
                arraySesiones(1) = Row.Cells(2).Text
                arraySesiones(2) = Row.Cells(3).Text
                arraySesiones(3) = Row.Cells(4).Text
                arraySesiones(4) = Row.Cells(5).Text
                'ReturnArgumentShowDialogPopup(arraySesiones(0), arraySesiones(1), arraySesiones(2), arraySesiones(3), arraySesiones(4))
                Session("SS_DatosModal") = arraySesiones
                EjecutarJS("window.close();")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

End Class

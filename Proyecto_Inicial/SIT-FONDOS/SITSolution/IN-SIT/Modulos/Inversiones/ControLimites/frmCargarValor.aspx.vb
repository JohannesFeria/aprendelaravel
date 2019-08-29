Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Text
Imports System.Data
Partial Class Modulos_Inversiones_ControLimites_frmCargarValor
    Inherits BasePage
#Region "Rutinas"
    Private Function BuscarValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String) As DataTable
        Return New ValoresBM().InstrumentosBuscarPorFiltro(sbs, isin, mnemonico, DatosRequest).Tables(0)
    End Function
    Private Function ObtenerDatosBusqueda() As Boolean
        Dim sbs As String = IIf(Request.QueryString("vSBS") Is Nothing, "", Request.QueryString("vSBS"))
        Dim isin As String = IIf(Request.QueryString("vISIN") Is Nothing, "", Request.QueryString("vISIN"))
        Dim mnemonico As String = IIf(Request.QueryString("vMnemonico") Is Nothing, "", Request.QueryString("vMnemonico"))
        Dim dtblDatos As DataTable = BuscarValores(isin, sbs, mnemonico)
        dgLista.DataSource = dtblDatos : Me.dgLista.DataBind()
        lbContador.Text = UIUtility.MostrarResultadoBusqueda(dtblDatos)
        Return True
    End Function
    Private Sub ReturnArgumentShowDialogPopup(ByVal mnemonico As String)
        Dim script As New StringBuilder
        With script
            .Append("<script>")
            .Append("   var setArgument = window.dialogArguments;")
            .Append("   setArgument.MNEMONICO = '" + mnemonico + "';")
            .Append("   window.close()")
            .Append("</script>")
        End With
        EjecutarJS(script.ToString())
    End Sub
    Public Sub SeleccionarISIN(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String = e.CommandArgument
        'ReturnArgumentShowDialogPopup(strcadena.Split(",").GetValue(1))
        Dim arraySesiones As String() = New String(1) {}
        arraySesiones(0) = strcadena.Split(",").GetValue(1)
        Session("arraynemonico") = arraySesiones
        EjecutarJS("<script>window.close();</script>", False)
    End Sub
#End Region
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ObtenerDatosBusqueda()
            ibCancelar.Attributes.Add("onclick", "CloseWindow();")
        End If
    End Sub
End Class

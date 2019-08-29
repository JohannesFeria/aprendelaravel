'Creado por: HDG OT 61566 Nro3 20101027
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Text
Imports System.Data
Imports UIUtility

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaUsuariosNotifica
    Inherits BasePage
    Protected oLimiteBM As New LimiteBM

#Region " /* Eventos de Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarCombo()
            CargarGrilla()
            btnCancelar.Attributes.Add("onclick", "window.close();")
        End If
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Function ReturnArgumentShowDialogPopup(ByVal codusu As String, ByVal nombre As String, ByVal unidad As String, ByVal coduni As String, ByVal codint As String) As Boolean
        Dim script As New StringBuilder
        With script
            .Append("<script>")
            .Append("   window.close()")
            .Append("</script>")
        End With
        EjecutarJS(script.ToString(), False)
    End Function

    Public Sub SeleccionarUsuario(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String = e.CommandArgument
        Dim arraySesiones As String() = New String(5) {}

        arraySesiones(0) = strcadena.Split(",").GetValue(0).ToString()
        arraySesiones(1) = strcadena.Split(",").GetValue(1).ToString()
        arraySesiones(2) = strcadena.Split(",").GetValue(2).ToString()
        arraySesiones(3) = strcadena.Split(",").GetValue(3).ToString()
        arraySesiones(4) = strcadena.Split(",").GetValue(4).ToString()

        Session("SS_DatosModal") = arraySesiones
        EjecutarJS("window.close();")
    End Sub

    Private Sub CargarGrilla()
        Dim dt As DataTable
        Dim oUsuario As New UsuariosNotificaBE
        Dim oRow As UsuariosNotificaBE.UsuariosNotificaRow

        oRow = CType(oUsuario.UsuariosNotifica.NewRow(), UsuariosNotificaBE.UsuariosNotificaRow)
        oRow.CodigoUsuario = tbCodUsu.Text.Trim
        oRow.Nombre = tbPriNom.Text.Trim
        oRow.Apellido = tbPriApe.Text.Trim
        oRow.CodigoCentroCosto = ddlUnidad.SelectedValue

        oUsuario.UsuariosNotifica.AddUsuariosNotificaRow(oRow)
        oUsuario.UsuariosNotifica.AcceptChanges()

        dt = oLimiteBM.SeleccionarPersonal(oUsuario)
        dgLista.DataSource = dt
        dgLista.DataBind()
        'EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dt) + "');")
    End Sub

    Public Sub CargarCombo()
        Dim DtTablaUnidad As New DataTable
        DtTablaUnidad = oLimiteBM.ListarUnidadesPuestos()
        HelpCombo.LlenarComboBox(ddlUnidad, DtTablaUnidad, "CodigoCentroCosto", "NombreCentroCosto", True)
    End Sub

#End Region

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        CargarGrilla()
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff';")
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5f5f5';this.style.cursor='hand'")
        End If
    End Sub
End Class

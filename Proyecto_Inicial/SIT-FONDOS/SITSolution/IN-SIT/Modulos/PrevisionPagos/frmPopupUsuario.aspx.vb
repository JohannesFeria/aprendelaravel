Imports SIT.BusinessLayer
Imports SIT.BusinessEntities
Imports System.Text
Imports System.Data

Public Class Modulos_PrevisionPagos_frmPopupUsuario
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            btnCancelarM.Attributes.Add("onclick", "javascript:window.close();")
        End If
    End Sub

    Protected Sub btnBuscarPersonal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscarPersonal.Click
        Try
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Sub CargarGrilla()
        Dim Nombres As String = Me.tbNombre_popup.Text.Trim()
        Dim ApellidoPat As String = Me.tbApePat_popup.Text.Trim()
        Dim Codigo As String = Me.tbCodUsuario_popup.Text.Trim()
        Dim dtblDatos As DataTable = New PrevisionPersonalBM().ListarPersonal(Nombres, ApellidoPat, Codigo).Tables(0)
        gvUsuarios_popup.DataSource = dtblDatos
        gvUsuarios_popup.DataBind()
        EjecutarJS(String.Format("$('#lbContador_popup').text('{0}');", "Registros Encontrados : " + dtblDatos.Rows.Count.ToString()))
    End Sub

    Protected Sub gvUsuarios_popup_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUsuarios_popup.RowCommand
        Dim strcadena As String = e.CommandArgument
        If e.CommandName = "Seleccionar" Then
            ReturnArgumentShowDialogPopup(strcadena.Split(",").GetValue(0), strcadena.Split(",").GetValue(1), strcadena.Split(",").GetValue(2), strcadena.Split(",").GetValue(3))
        End If

    End Sub

    Private Function ReturnArgumentShowDialogPopup(ByVal CodigoInterno As String, ByVal CodigoUsuario As String, ByVal NumDocumento As String, ByVal NombreCompleto As String) As Boolean
        Dim script As New StringBuilder
        With script
            .Append("<script>")
            .Append("GetRowValue('" + CodigoUsuario + "','" + NombreCompleto + "');")
            .Append("</script>")
        End With
        ClientScript.RegisterStartupScript(GetType(String), New Guid().ToString, script.ToString())
    End Function

End Class

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_PrevisionPagos_frmParametriaGeneral
    Inherits BasePage

#Region "/* Eventos de la Pagina */"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../frmDefault.aspx", False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim strcadena As String = e.CommandArgument
            Response.Redirect("frmParametriaDetalle.aspx?cod=" & strcadena & "&desc=" & e.CommandName)
        Catch ex As Exception
            'AlertaJS("Ocurrió un error en el sistema.")
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
#End Region

#Region "/* Funciones Personalizadas */"

    Private Sub CargarGrilla()
        Dim dtTabla As Data.DataTable
        dtTabla = New PrevisionParametroBM().Listar(tbParametro.Text.Trim.ToString).Tables(0)
        If dtTabla.Rows.Count > 0 Then
            gvPagos.DataSource = dtTabla
        End If
        gvPagos.DataBind()
        EjecutarJS(String.Format("$('#lbContador').text('{0}');", "Registros Encontrados : " + dtTabla.Rows.Count.ToString()))
    End Sub

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarGrilla()
        End If
    End Sub

#End Region

    Protected Sub gvPagos_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPagos.PageIndexChanging
        gvPagos.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub

    'Protected Sub Alerta(ByVal mensaje As String)
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), New Guid().ToString(), "<script language='javascript'>alert('" & mensaje & "');</script>", False)
    '    'Me.RegisterStartupScript("Mensaje", "<script language='javascript'>alert('" & mensaje & "');</script>")
    'End Sub

    Private Sub Alerta(mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), New Guid().ToString(), "<script language='javascript'>alert('" & mensaje & "');</script>", False)
    End Sub

End Class

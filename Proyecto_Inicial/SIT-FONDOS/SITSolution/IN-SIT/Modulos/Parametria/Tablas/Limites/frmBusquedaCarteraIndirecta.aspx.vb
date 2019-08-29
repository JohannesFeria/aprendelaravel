Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Limites_frmBusquedaCarteraIndirecta
    Inherits BasePage

    Private FechaCarteraI As Decimal
    Private GrupoEconomico As String
    Private Fondo As String
    Private Emisor As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarGrilla()
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oCarteraIndirectaBM As New CarteraIndirectaBM
        Dim dt As DataTable
        Me.FechaCarteraI = UIUtility.ConvertirFechaaDecimal(Me.txtFecha.Text)
        Me.GrupoEconomico = Me.txtGrupoEconomico.Text
        Me.Fondo = Me.txtFondo.Text
        Me.Emisor = Me.txtEmisor.Text

        dt = oCarteraIndirectaBM.SeleccionarPorFiltro("", Me.FechaCarteraI, Me.GrupoEconomico, Me.Fondo, Me.Emisor, "", "", "", DatosRequest).Tables(0)
        Me.dgLista.DataSource = dt
        Me.dgLista.DataBind()

        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error la realizar la Búsqueda")
        End Try
    End Sub

    Private Sub Buscar()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Protected Sub btnIngresar_Click(sender As Object, e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmCarteraIndirecta.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As System.EventArgs) Handles btnSalir.Click
        Try
            'Session("dtMontoNegociadoBVL") = Nothing
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmCarteraIndirecta.aspx?cod=" & e.CommandArgument)
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oCarteraIndirectaBM As New CarteraIndirectaBM
            Dim codigo As String = e.CommandArgument
            oCarteraIndirectaBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
        End If
    End Sub

    Protected Sub btnImportar_Click(sender As Object, e As System.EventArgs) Handles btnImportar.Click
        Try
            Response.Redirect("frmCarteraIndirectaImportar.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Importar")
        End Try
    End Sub
End Class

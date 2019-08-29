Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_Inversiones_InstrumentosNegociados_frmConsultaCertificado
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        dgLista.PageIndex = 0

        Dim intIndica As Integer = CInt(Request.QueryString("vIndica"))
        ViewState("Indicador") = intIndica

        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If Not Me.IsPostBack Then
            CargarGrilla(intIndica)
        End If
        If intIndica = 1 Then
            lbTitulo.Text = "Relación de Certificados de Suscripción Vigentes"
        Else
            lbTitulo.Text = "Relación de Certificados de Suscripción a Vencidas"
        End If

    End Sub

    Private Sub CargarGrilla(ByVal intIndica As Integer)

        Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim dtbDatos As DataTable = oOrdenPreOrdenInversionBM.ConsultaCertificados(intIndica, DatosRequest)

        Me.dgLista.DataSource = dtbDatos
        Me.dgLista.DataBind()

        lbContador.Text = UIUtility.MostrarResultadoBusqueda(dtbDatos)

        If dtbDatos.Rows.Count = 0 And intIndica = 1 Then
            AlertaJS("No existen registros para mostrar", "window.close();")
        Else
            If dtbDatos.Rows.Count = 0 And intIndica = 0 Then
                CloseWindow()
            End If
        End If

    End Sub
    Private Sub CloseWindow()
        EjecutarJS("window.close();")
    End Sub

    Private Sub ibCancelar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibCancelar.Click
        CloseWindow()
    End Sub

    Private Sub dgLista_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgLista.PageIndexChanged
        Dim intOtroIndicador As Integer = ViewState("Indicador")
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla(intOtroIndicador)

    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Dim intOtroIndicador As Integer = ViewState("Indicador")
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla(intOtroIndicador)
    End Sub

End Class

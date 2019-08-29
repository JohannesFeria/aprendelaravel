Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Text
Partial Class Modulos_Inversiones_frmCorreccionDepositoPlazo
    Inherits BasePage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(Date.Now.ToString("yyyyMMdd")))
            CargarCombos()
        End If
        If Not Session("correccionDP") Is Nothing Then
            CargarGrilla()
            Session.Remove("correccionDP")
        End If
    End Sub
    Private Sub btnBuscarOPE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarOPE.Click
        CargarGrilla()
    End Sub
    Public Sub CargarCombos()
        Dim dt As New DataTable
        Dim oPortafolioBM As New PortafolioBM
        dt = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(ddlFondo, dt, "CodigoPortafolio", "Descripcion", True)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub ShowDialogPopup(ByVal StrURL As String)
        EjecutarJS("window.showModalDialog('" + StrURL + "','','dialogHeight:550px;dialogWidth:1200px;status:no;unadorned:yes;help:No'); document.getElementById('" & btnBuscarOPE.ClientID & "').click;")
    End Sub
    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If (ddlFondo.SelectedIndex <> 0) Then
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlFondo.SelectedValue.ToString))
        ElseIf (ddlFondo.SelectedIndex <> 0) Then
            tbFechaOperacion.Text = ""
        End If
    End Sub
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        If e.CommandName = "Select" Then
            Dim i As Integer = CInt(e.CommandArgument.ToString())
            hdNroOrden.Value = dgLista.Rows(i).Cells(1).Text
            hdTipoTitulo.Value = dgLista.Rows(i).Cells(7).Text
            hdCodigoOperacion.Value = dgLista.Rows(i).Cells(8).Text
        End If
    End Sub
    Sub CargarGrilla()
        Dim dt As New DataTable
        Dim oCorreccionBM As New CorreccionDepositoPlazoBM
        dt = oCorreccionBM.ListarCorreccionDepositoPlazo(ddlFondo.SelectedValue.ToString, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), DatosRequest)
        ViewState("CorreccionDP") = dt
        dgLista.DataSource = dt
        dgLista.DataBind()
    End Sub
End Class
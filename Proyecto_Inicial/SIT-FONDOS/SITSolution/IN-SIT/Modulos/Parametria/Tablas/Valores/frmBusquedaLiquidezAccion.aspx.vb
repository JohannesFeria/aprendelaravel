Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaLiquidezAccion
    Inherits BasePage

    Private CodigoMnemonico As String

#Region "/* Funciones de la Página*/"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Session("SS_DatosModal") Is Nothing Then
                If hdTipoModal.Value = "MNE" Then
                    tbCodigoMnemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                End If
                Session.Remove("SS_DatosModal")
            End If
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            dgLista.PageIndex = 0
            CargarGrilla()
            If dgLista.Rows.Count = 0 Then
                AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
            End If
        Catch ex As Exception
            AlertaJS(ex.ToString())
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Response.Redirect("frmLiquidezAccion.aspx?ope=reg")
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        dgLista.DataSource = CType(Session("dtLiquidezAccion"), DataTable)
        dgLista.DataBind()
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Select Case e.CommandName
            Case "Eliminar"
                Try
                    Dim oLiquidezAccionBM As New LiquidezAccionBM
                    Dim codigo As String = e.CommandArgument.ToString()
                    oLiquidezAccionBM.Eliminar(codigo, DatosRequest)
                    CargarGrilla()
                Catch ex As Exception                    
                    AlertaJS(ex.ToString())
                End Try
            Case "Modificar"
                Response.Redirect("frmLiquidezAccion.aspx?ope=mod&codMne=" + e.CommandArgument.ToString())
        End Select
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            If (e.Row.Cells(5).Text = "A") Then
                e.Row.Cells(5).Text = "Activo"
            ElseIf (e.Row.Cells(5).Text = "I") Then
                e.Row.Cells(5).Text = "Inactivo"
            End If
        End If
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarGrilla()
        Dim oLiquidezAccionBM As New LiquidezAccionBM
        Dim dt As DataTable
        Dim situacion As String
        situacion = IIf(ddlSituacion.SelectedIndex = 0, "", ddlSituacion.SelectedValue)
        CodigoMnemonico = tbCodigoMnemonico.Text
        dt = New LiquidezAccionBM().SeleccionarPorFiltro(CodigoMnemonico, situacion, DatosRequest).Tables(0)
        Session("dtLiquidezAccion") = dt
        dgLista.DataSource = dt
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub LimpiarConsulta()
        tbCodigoMnemonico.Text = ""
        ddlSituacion.SelectedIndex = 0
    End Sub

#End Region
   
End Class

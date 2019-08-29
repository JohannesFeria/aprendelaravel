Imports SIT.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaTipoCambio
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarCombos()
            CargarGrilla()
        End If
    End Sub

    Private Sub CargarCombos()
        UIUtility.CargarMonedaSituacionOI(ddlMonedaOrigen, "")
        UIUtility.CargarMonedaSituacionOI(ddlMonedaDestino, "")
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)

        ddlTipo.Items.Add("Todos")
        ddlTipo.Items.Add("Directo")
        ddlTipo.Items.Add("Indirecto")
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Me.dgLista.PageIndex = 0
            CargarGrilla()
            If Me.dgLista.Rows.Count = 0 Then
                AlertaJS("No se encontraron Registros")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Private Sub CargarGrilla()
        Dim oTipoCambioDI_BM As New TipoCambioDI_BM
        Dim dtblDatos As DataTable = oTipoCambioDI_BM.SeleccionarPorFiltros("", ddlMonedaOrigen.SelectedValue, ddlMonedaDestino.SelectedValue, IIf(ddlTipo.SelectedValue = "Todos", "", ddlTipo.SelectedValue.Substring(0, 1)), IIf(Me.ddlSituacion.SelectedValue = "Todos", "", ddlSituacion.SelectedValue), DatosRequest).Tables(0)
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()

        'Me.lblContador.Text = UIUtility.MostrarResultadoBusqueda(dtblDatos)

        EjecutarJS("$('#" + lblContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")
    End Sub

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmTipoCambio.aspx?cod=" & e.CommandArgument)
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oTipoCambioDI_BM As New TipoCambioDI_BM
            Dim codigo As String = e.CommandArgument
            oTipoCambioDI_BM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Public Sub LimpiarConsulta()
        Me.ddlMonedaOrigen.SelectedIndex = 0
        Me.ddlMonedaDestino.SelectedIndex = 0
        Me.ddlTipo.SelectedIndex = 0
        Me.ddlSituacion.SelectedIndex = 0
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Response.Redirect("frmTipoCambio.aspx")
    End Sub

    Private Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
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
End Class

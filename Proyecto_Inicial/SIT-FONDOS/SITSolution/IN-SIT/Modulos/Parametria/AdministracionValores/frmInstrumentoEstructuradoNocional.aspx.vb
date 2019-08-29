Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data

Partial Class Modulos_Parametria_AdministracionValores_frmInstrumentoEstructuradoNocional
    Inherits BasePage

    Private Const IE_NOCIONAL As String = "Nocional"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ViewState("CodigoNemonico") = Request.QueryString("Nemonico")
        If Not Page.IsPostBack Then
            CargarPagina(ViewState("CodigoNemonico"))
        End If
    End Sub

    Private Sub CargarPagina(ByVal codigoNemonico As String)
        Dim oMonedaBM As New MonedaBM

        If Session(IE_NOCIONAL) Is Nothing Then
            Dim oIENocionalBM As New InstrumentosEstructuradosBM
            Session(IE_NOCIONAL) = oIENocionalBM.ListarInstrumentoEstructuradoNocional(codigoNemonico)

            dgIENocional.DataSource = CType(Session(IE_NOCIONAL), DataTable)
            dgIENocional.DataBind()
        Else
            dgIENocional.DataSource = CType(Session(IE_NOCIONAL), DataTable)
            dgIENocional.DataBind()
        End If

        HelpCombo.LlenarComboBox(ddlMoneda, oMonedaBM.Listar(DatosRequest).Tables(0), "CodigoMoneda", "CodigoMoneda", True)
        UIUtility.CargarPortafoliosOI(ddlPortafolio)
        ddlPortafolio.Items.Remove(ParametrosSIT.PORTAFOLIO_ADMINISTRA)
        ddlPortafolio.Items.Remove(ParametrosSIT.PORTAFOLIO_MULTIFONDOS)

    End Sub

    Private Sub EliminarNocional(ByVal index As Integer)
        Dim dt As DataTable
        dt = CType(Session(IE_NOCIONAL), DataTable)
        dt.Rows.RemoveAt(index)
        Session(IE_NOCIONAL) = dt
        dgIENocional.DataSource = dt
        dgIENocional.DataBind()
    End Sub

    Private Sub ModificarNocional(ByVal index As Integer)
        Dim dt As DataTable
        dt = CType(Session(IE_NOCIONAL), DataTable)
        txtNocional.Text = dt.Rows(index)("Nocional")
        ddlPortafolio.SelectedValue = dt.Rows(index)("Codigoportafoliosbs")
        ddlMoneda.SelectedValue = dt.Rows(index)("CodigoMoneda")
        btnAgregar.Visible = False
        btnModificar.Visible = True
        btnCancelar.Visible = True
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        If Validar() = False Then
            AlertaJS("Ya existe un Nocional para este Fondo")
            Exit Sub
        End If
        Dim dt As DataTable
        Dim oRow As DataRow
        dt = CType(Session(IE_NOCIONAL), DataTable)
        oRow = dt.NewRow()
        oRow.Item("CodigoInstrumentoEstructuradoNocional") = -1
        oRow.Item("Nocional") = txtNocional.Text
        oRow.Item("CodigoNemonico") = ViewState("CodigoNemonico")
        oRow.Item("CodigoPortafolioSBS") = ddlPortafolio.SelectedValue
        oRow.Item("CodigoMoneda") = ddlMoneda.SelectedValue
        dt.Rows.Add(oRow)
        Session(IE_NOCIONAL) = dt
        dgIENocional.DataSource = dt
        dgIENocional.DataBind()
        limpiarCampos()
    End Sub

    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        Dim dt As DataTable
        dt = CType(Session(IE_NOCIONAL), DataTable)
        dt.Rows(dgIENocional.SelectedIndex)("Nocional") = txtNocional.Text
        dt.Rows(dgIENocional.SelectedIndex)("CodigoPortafolioSBS") = ddlPortafolio.SelectedValue
        dt.Rows(dgIENocional.SelectedIndex)("CodigoMoneda") = ddlMoneda.SelectedValue

        Session(IE_NOCIONAL) = dt
        dgIENocional.DataSource = dt
        dgIENocional.DataBind()
        dgIENocional.SelectedIndex = -1
        btnAgregar.Visible = True
        btnModificar.Visible = False
        btnCancelar.Visible = False
        limpiarCampos()
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        dgIENocional.SelectedIndex = -1
        btnAgregar.Visible = True
        btnModificar.Visible = False
        btnCancelar.Visible = False
        limpiarCampos()
    End Sub

    Private Sub limpiarCampos()
        txtNocional.Text = ""
        ddlPortafolio.SelectedIndex = -1
        ddlMoneda.SelectedIndex = -1
    End Sub

    Private Function Validar() As Boolean
        Dim resultado As Boolean = True
        For Each row As GridViewRow In dgIENocional.Rows
            If row.Cells(2).Text = ddlPortafolio.SelectedValue Then
                resultado = False
                Exit For
            End If
        Next
        Return resultado
    End Function

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        EjecutarJS("window.close();")
    End Sub

    Protected Sub dgIENocional_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIENocional.RowCommand
        Select Case e.CommandName
            Case "Eliminar" : EliminarNocional(CInt(e.CommandArgument))
            Case "Select" : ModificarNocional(CInt(e.CommandArgument)) 'es modificar'
        End Select
    End Sub

    Protected Sub dgIENocional_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgIENocional.RowDataBound
        Dim btn As ImageButton
        If e.Row.RowType = DataControlRowType.DataRow Then
            btn = CType(e.Row.FindControl("ibEliminar"), ImageButton)
            btn.Attributes.Add("onclick", "return confirm_delete();")
        End If
    End Sub
End Class

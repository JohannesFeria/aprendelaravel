Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports UIUtility

Partial Class Modulos_Contabilidad_frmPlanDeCuentas
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarCombos()
            'BindingPeriodos()
            dgLista.DataSource = CreateDataSource()
            dgLista.DataBind()
        End If
    End Sub

    Function CreateDataSource() As ICollection
        Dim dt As DataTable
        Dim dr As DataRow
        dt = New DataTable
        dt.Columns.Add(New DataColumn("CuentaContable", GetType(String)))
        dt.Columns.Add(New DataColumn("DescripcionCuenta", GetType(String)))

        dr = dt.NewRow()
        dr(0) = ""
        dr(1) = ""
        dr = dt.NewRow()
        CreateDataSource = New DataView(dt)
    End Function
    Sub CargarCombos()
        Dim tablaPortafolio As New Data.DataTable
        Dim oPortafolio As New PortafolioBM
        tablaPortafolio = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlFondo, tablaPortafolio, "CodigoPortafolio", "Descripcion", True)
    End Sub

    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If ddlFondo.SelectedIndex <> 0 Then
            Dim oPortafolio As New PortafolioBM
            Me.lblNombreFondo.Text = oPortafolio.Seleccionar(ddlFondo.SelectedValue.ToString, DatosRequest).Portafolio.Item(0).Descripcion.ToString

            CargarGrilla()
            If dgLista.Rows.Count = 0 Then
                AlertaJS("No se encontraron Registros")
            End If
        End If
    End Sub

    Private Sub ibListar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CargarGrilla()
        If dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub

    Private Sub ibSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibSalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub

    Private Sub dgLista_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Try
            tbCuentaContable.Value = e.Item.Cells(1).Text
            tbDescripcionCuentaContable.Value = e.Item.Cells(2).Text
        Catch ex As Exception
            tbCuentaContable.Value = ""
            tbDescripcionCuentaContable.Value = ""
        End Try

    End Sub


    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
        Me.tbCuentaContable.Value = ""
        Me.tbDescripcionCuentaContable.Value = ""
    End Sub

    Private Sub CargarGrilla()
        Dim dtPlanCuentas As New DataTable
        Try
            Dim oPlanDeCuentas As New PlanDeCuentasBM
            dtPlanCuentas = oPlanDeCuentas.SeleccionarPorFiltro(ddlFondo.SelectedValue.ToString, 0, DatosRequest).Tables(0)
            dgLista.DataSource = dtPlanCuentas.DefaultView
            dgLista.DataBind()
        Catch ex As Exception
            dgLista.DataSource = Nothing
            dgLista.DataBind()
            Me.tbCuentaContable.Value = ""
            Me.tbDescripcionCuentaContable.Value = ""
            dgLista.PageIndex = 0
        End Try
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtPlanCuentas.Rows.Count) + "');")
    End Sub

    'Private Sub BindingPeriodos()
    '    Dim aux As Integer = DateTime.Now.Year
    '    Dim i As Integer
    '    For i = aux - 4 To aux
    '        ddlPeriodo.Items.Add(New ListItem(i.ToString(), i.ToString()))
    '    Next
    '    ddlPeriodo.SelectedValue = DateTime.Now.Year.ToString()
    'End Sub

    Private Sub ibImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibImprimir.Click
        If dgLista.Rows.Count = 0 Then
            AlertaJS("No existen registros a mostrar")
        Else
            'Session("Ano") = Me.ddlPeriodo.SelectedValue.ToString
            Session("CodigoFondo") = Me.ddlFondo.SelectedValue.ToString
            Session("descripcionFondo") = Me.ddlFondo.SelectedItem.Text.Trim
            EjecutarJS(MostrarPopUp("frmVisorPlanCuentas.aspx", "no", 1100, 850, 40, 150, "no", "yes", "yes", "yes"), False)
        End If
    End Sub

End Class

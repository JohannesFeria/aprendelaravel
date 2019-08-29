Imports System.Data
Imports Sit.BusinessLayer
Imports ParametrosSIT
Partial Class Modulos_Inversiones_ConsultasPreOrden_frmConsultaHistoricaPreOrden
    Inherits BasePage
#Region "Rutinas"
    Dim oUtil As New UtilDM
    Dim oPortafolioBM As New PortafolioBM
    Public Sub CargarCombos()
        Dim tablaTipoFondo As New DataTable
        Dim tablaOperacion As New DataTable
        Dim tablaEstado As New DataTable
        Dim oOperacionBM As New OperacionBM
        tablaOperacion = oOperacionBM.Listar_ClaseInstrumento(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlTipoOperacion, tablaOperacion, "CodigoOperacion", "Descripcion", True)
        Dim oPortafolio As New PortafolioBM
        tablaTipoFondo = oPortafolio.Listar(DatosRequest).Tables(0)
        ddlPortafolio.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
        ddlPortafolio.Items.Insert(0, New ListItem("--Seleccione--", ""))
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        tablaEstado = oParametrosGeneralesBM.ListarEstadoOI(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlEstado, tablaEstado, "Valor", "Nombre", True)
    End Sub
    Private Sub ShowDialogPopup(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        Dim script As New StringBuilder
        With script
            .Append("<script>")
            .Append("function PopupBuscador(isin, sbs, mnemonico)")
            .Append("{")
            .Append("   window.showModalDialog('frmBusquedaValoresInstrumento.aspx?vIsin='+ isin + '&vSbs=' + sbs + '&vMnemonico=' + mnemonico, '', 'dialogHeight:600px; dialogWidth:700px; dialogLeft:150px;');document.getElementById('" + btnpopup.ClientID + "').click();")
            .Append("}")
            .Append("PopupBuscador('" + isin + "','" + sbs + "','" + mnemonico + "');")
            .Append("</script>")
        End With
        EjecutarJS(script.ToString(), False)
    End Sub
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarCombos()
            Dim dstemp As New DataTable
            dgordenpreorden.DataSource = dstemp
            dgordenpreorden.DataBind()
            Me.lbContador.Text = UIUtility.MostrarResultadoBusqueda(dstemp)
            Me.tbFechaInicio.Text = oUtil.RetornarFechaSistema
        End If
        If Not Session("SS_DatosModal") Is Nothing Then
            'Private Sub ReturnArgumentShowDialogPopup(ByVal isin As String, ByVal mnemonico As String, ByVal sbs As String, ByVal descripcion As String)
            txtISIN.Text = CType(Session("SS_DatosModal"), String())(0)
            txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1)
            txtsbs.Text = CType(Session("SS_DatosModal"), String())(2)
            Session.Remove("SS_DatosModal")
        End If
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        ShowDialogPopup(txtISIN.Text.Trim, txtsbs.Text.Trim, txtMnemonico.Text.Trim)
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click
        Dim objOrdeninversionBM As New OrdenPreOrdenInversionBM
        Dim decfechainicio As Decimal
        decfechainicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text)
        Dim dsconsulta As New DataTable
        dsconsulta = objOrdeninversionBM.ConsultaHistoricaPreOrdenes(Me.ddlPortafolio.SelectedValue, decfechainicio, Me.ddlTipoOperacion.SelectedValue, Me.txtMnemonico.Text, Me.txtISIN.Text, Me.txtsbs.Text, ddlEstado.SelectedValue.ToString, DatosRequest)
        If dsconsulta.Rows.Count <> 0 Then
            dgordenpreorden.DataSource = dsconsulta
            dgordenpreorden.DataBind()
            Me.lbContador.Text = UIUtility.MostrarResultadoBusqueda(dsconsulta)
            Session("TablaConsulta") = dsconsulta
        Else
            Dim dstemp As New DataTable
            dgordenpreorden.DataSource = dstemp
            dgordenpreorden.DataBind()
            Me.lbContador.Text = UIUtility.MostrarResultadoBusqueda(dstemp)
        End If
    End Sub
    Protected Sub ibSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibSalir.Click
        Response.Redirect("../../../frmDefault.aspx", False)
    End Sub
    Protected Sub btnexportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexportar.Click
        Dim decfechainicio As Decimal
        decfechainicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text)
        EjecutarJS(UIUtility.MostrarPopUp("frmVisorConsultaPreorden.aspx?pIndica=1&pestado=" + Me.ddlEstado.SelectedValue() + "&pportafolio=" + Me.ddlPortafolio.SelectedValue() + "&pcodigooperacion=" + Me.ddlTipoOperacion.SelectedValue + "&pmnemonico=" + Me.txtMnemonico.Text + "&pisin=" + Me.txtISIN.Text + "&psbs=" + Me.txtsbs.Text + "&pfechaasignacion=" + Convert.ToString(decfechainicio), "no", 800, 600, 10, 10, "no", "yes", "yes", "yes"), False)
    End Sub
End Class

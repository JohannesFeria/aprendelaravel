Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Text
Partial Class Modulos_Inversiones_AsignacionFondos_frmExtornoAsignacion
    Inherits BasePage
#Region "Variables"
    Private campos() As String = {"CodigoMnemonico", "HO-FONDO1", "HO-FONDO2", "HO-FONDO3", "Monto"}
    Private tipos() As String = {"System.String", "System.String", "System.String", "System.String", "System.String"}
#End Region
#Region "Rutinas"
    Private Function ExisteEntidad() As Boolean
        Dim oValoresBM As New ValoresBM
        Dim oValoresBE As New DataTable
        oValoresBE = oValoresBM.SeleccionarInstrumento(String.Empty, Me.tbMnemonico.Text.Trim(), Me.DatosRequest).Tables(0)
        Return oValoresBE.Rows.Count > 0
    End Function
    Private Sub ShowDialogPopup(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        Dim script As New StringBuilder
        With script
            .Append("<script>")
            .Append("function PopupBuscador(isin, sbs, mnemonico)")
            .Append("{")
            .Append("   window.showModalDialog('../ControLimites/frmCargarValor.aspx?vIsin='+ isin + '&vSbs=' + sbs + '&vMnemonico=' + mnemonico, '', 'dialogHeight:450px; dialogWidth:700px; dialogLeft:150px;');")
            .Append("   return false;")
            .Append("}")
            .Append("PopupBuscador('" + isin + "','" + sbs + "','" + mnemonico + "'); $('#btnpopup').trigger('click') ")
            .Append("</script>")
        End With
        EjecutarJS(script.ToString(), False)
    End Sub
    Protected Sub CrearMensajeStartupScript(ByVal mensaje As String)
        AlertaJS(mensaje)
    End Sub
    Private Sub CargarGrilla()
        Dim tablaOIAsignadas As Data.DataTable
        Dim StrCodigoMnemo, StrISIN As String
        Dim decfecha As Decimal
        StrCodigoMnemo = Me.tbMnemonico.Text.ToString.ToUpper.Trim
        StrISIN = Me.tbISIN.Text.ToString.ToUpper.Trim
        decfecha = UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text.Trim())
        Dim oOrdenesInversionBM As New OrdenPreOrdenInversionBM
        tablaOIAsignadas = oOrdenesInversionBM.ListarOIAsignadas(StrCodigoMnemo, StrISIN, decfecha, DatosRequest)
        dgLista.DataSource = tablaOIAsignadas
        dgLista.DataBind()
        Me.lbContador.Text = UIUtility.MostrarResultadoBusqueda(tablaOIAsignadas)
    End Sub
    Private Sub Buscar()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            CrearMensajeStartupScript(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub
#End Region
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.ibBuscar.Attributes.Add("onclick", "javascript:return ValidaCamposObligatorios();")
        Me.ibExtornar.Attributes.Add("onClick", "javascript:return confirm('¿Confirmar el extorno de la Orden de Inversión?');")
        If Not Page.IsPostBack Then
            Dim dtblGenerico As DataTable = UIUtility.GetStructureTablebase(campos, tipos)
            lbContador.Text = UIUtility.MostrarResultadoBusqueda(dtblGenerico)
            dgLista.DataSource = dtblGenerico : dgLista.DataBind()
            'RGF 20080625
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(PORTAFOLIO_MULTIFONDOS)
        End If
     
    End Sub
    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.SelectedIndex = -1
        dgLista.PageIndex = e.NewPageIndex
        Buscar()
    End Sub
    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        If e.CommandName = "Seleccionar" Then
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            dgLista.SelectedIndex = gvr.RowIndex
            Me.lblCodigoMnemonico.Text = gvr.Cells(1).Text
            Me.lblFondo1.Text = gvr.Cells(2).Text
            Me.lblFondo2.Text = gvr.Cells(3).Text
            Me.lbFondo3.Text = gvr.Cells(4).Text
            Me.lblCodigoPreOrden.Text = gvr.Cells(5).Text
        End If
    End Sub
    Protected Sub ibAyuda_Click(sender As Object, e As System.EventArgs) Handles ibAyuda.Click
        ShowDialogPopup(Me.tbISIN.Text.Trim(), "", Me.tbMnemonico.Text.Trim())
    End Sub
    Protected Sub ibBuscar_Click(sender As Object, e As System.EventArgs) Handles ibBuscar.Click
        Dim blnExisteItem As Boolean
        Try
            If Me.tbMnemonico.Text.Trim <> "" Then
                Dim StrId As String = String.Empty
                blnExisteItem = ExisteEntidad()
                If Not blnExisteItem Then
                    AlertaJS("No existe el Instrumento")
                Else
                    Buscar()
                End If
            Else
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub ibConsultar_Click(sender As Object, e As System.EventArgs) Handles ibConsultar.Click
        Me.tbISIN.Text = ""
        Me.tbMnemonico.Text = ""
        Me.tbFechaOperacion.Text = ""
    End Sub
    Protected Sub ibExtornar_Click(sender As Object, e As System.EventArgs) Handles ibExtornar.Click
        Dim oOrdenesInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        Try
            If Me.lblCodigoMnemonico.Text.ToString <> "" Then
                oOrdenesInversionWorkFlowBM.ExtornarOIAsignada(UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text.Trim()), Me.lblCodigoPreOrden.Text, Me.lblFondo1.Text.Trim(), Me.lblFondo2.Text.Trim(), Me.lbFondo3.Text.Trim(), Me.lblCodigoMnemonico.Text.Trim(), DatosRequest)
                AlertaJS("Se realizó el extorno satisfactoriamente")
                CargarGrilla()
            Else
                AlertaJS("Debe seleccionar un Registro")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub ibSalir_Click(sender As Object, e As System.EventArgs) Handles ibSalir.Click
        Response.Redirect("~/frmDefault.aspx", False)
    End Sub
    Protected Sub btnpopup_Click(sender As Object, e As System.EventArgs) Handles btnpopup.Click
        If Not Session("arraynemonico") Is Nothing Then
            tbMnemonico.Text = CType(Session("arraynemonico"), String())(0)
            Session.Remove("arraynemonico")
        End If
    End Sub
End Class

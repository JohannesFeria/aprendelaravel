Imports System.Text
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

'Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.ReportSource
'Imports CrystalDecisions.Shared

Partial Class Modulos_Tesoreria_OperacionesCaja_frmExtornoOperacionesCaja
    Inherits BasePage

    Dim oOperacionesCaja As New OperacionesCajaBM

#Region "Eventos de la Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try

    End Sub

    Private Sub btnAprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        Try
            Dim chkSelect As System.Web.UI.WebControls.CheckBox
            Dim hdCodigoExtorno As System.Web.UI.HtmlControls.HtmlInputHidden
            Dim cont As Integer = 0
            For Each fila As GridViewRow In dgLista.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    chkSelect = CType(fila.FindControl("chkSelect"), System.Web.UI.WebControls.CheckBox)
                    hdCodigoExtorno = CType(fila.FindControl("hdCodigoExtorno"), System.Web.UI.HtmlControls.HtmlInputHidden)
                    If chkSelect.Checked Then
                        oOperacionesCaja.ExtornarOperacionesCaja(CType(hdCodigoExtorno.Value, Decimal), True, DatosRequest)
                        cont = cont + 1
                    End If
                End If
            Next
            If cont > 0 Then
                AlertaJS("Los cambios se realizaron satisfactoriamente! ")
            Else
                AlertaJS("Debe seleccionar algún registro! ")
            End If
            CargarGrilla(ViewState("fechaInicio"), ViewState("fechaFin"), ViewState("motivo"), ViewState("estado"))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            ViewState("fechaInicio") = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
            ViewState("fechaFin") = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
            ViewState("motivo") = ddlMotivo.SelectedValue
            ViewState("estado") = ddlEstado.SelectedValue
            CargarGrilla(ViewState("fechaInicio"), ViewState("fechaFin"), ViewState("motivo"), ViewState("estado"))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnDesaprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDesaprobar.Click
        Try
            Dim chkSelect As System.Web.UI.WebControls.CheckBox
            Dim hdCodigoExtorno As System.Web.UI.HtmlControls.HtmlInputHidden
            Dim cont As Integer = 0
            For Each fila As GridViewRow In dgLista.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    chkSelect = CType(fila.FindControl("chkSelect"), System.Web.UI.WebControls.CheckBox)
                    hdCodigoExtorno = CType(fila.FindControl("hdCodigoExtorno"), System.Web.UI.HtmlControls.HtmlInputHidden)
                    If chkSelect.Checked Then
                        oOperacionesCaja.ExtornarOperacionesCaja(CType(hdCodigoExtorno.Value, Decimal), False, DatosRequest)
                        cont = cont + 1
                    End If
                End If
            Next
            If cont > 0 Then
                AlertaJS("Los cambios se realizaron satisfactoriamente! ")
            Else
                AlertaJS("Debe seleccionar algún registro! ")
            End If
            CargarGrilla(ViewState("fechaInicio"), ViewState("fechaFin"), ViewState("motivo"), ViewState("estado"))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim fechaInicio As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
            Dim fechaFin As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
            Dim motivo As String = ddlMotivo.SelectedValue
            Dim estado As String = ddlEstado.SelectedValue
            EjecutarJS(UIUtility.MostrarPopUp("../Reportes/frmVisorReporte.aspx?tipo=OCE&fechaInicio=" & fechaInicio & "&fechaFin=" & fechaFin & "&motivo=" & motivo & "&estado=" & estado, "no", 1100, 750, 40, 150, "no", "yes", "yes", "yes"), False)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

#End Region

#Region "Funciones Personalizadas"
    Private Sub CargarPagina()
        btnAprobar.Attributes.Add("onclick", "javascript:return SeleccionarOperaciones();")
        btnDesaprobar.Attributes.Add("onclick", "javascript:return SeleccionarOperaciones();")
        CargarMotivo()
        CargarEstado()
        tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
        tbFechaFin.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Today)
        ViewState("fechaInicio") = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
        ViewState("fechaFin") = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
        ViewState("motivo") = ddlMotivo.SelectedValue
        ViewState("estado") = ddlEstado.SelectedValue
    End Sub

    Private Sub CargarMotivo()
        Dim dtMotivo As DataTable = New ParametrosGeneralesBM().Listar(ParametrosSIT.MOTIVO_EXTORNO, DatosRequest)
        HelpCombo.LlenarComboBox(ddlMotivo, dtMotivo, "Valor", "Nombre", True)
    End Sub

    Private Sub CargarEstado()
        Dim dtEstado As DataTable = New ParametrosGeneralesBM().Listar(ParametrosSIT.ESTADO_EXTORNO, DatosRequest)
        HelpCombo.LlenarComboBox(ddlEstado, dtEstado, "Valor", "Nombre", True)
        ddlEstado.SelectedValue = "1"
    End Sub

    Private Sub CargarGrilla(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal motivo As String, ByVal estado As String)
        dgLista.DataSource = oOperacionesCaja.SeleccionarExtornos(fechaInicio, fechaFin, motivo, estado, DatosRequest)
        dgLista.DataBind()
    End Sub

#End Region
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla(ViewState("fechaInicio"), ViewState("fechaFin"), ViewState("motivo"), ViewState("estado"))
    End Sub
End Class

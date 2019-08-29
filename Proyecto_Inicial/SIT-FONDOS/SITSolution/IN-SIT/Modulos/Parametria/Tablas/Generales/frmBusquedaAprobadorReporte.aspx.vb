Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaAprobadorReporte
    Inherits BasePage

#Region "Eventos de la Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoUsuario.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmAprobadorReporte.aspx?p_codInterno=" & ViewState("p_codInterno") & "&p_situacion=" & ViewState("p_situacion"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar a la página")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            ViewState("p_codInterno") = tbCodigoUsuario.Text.Trim()
            ViewState("p_situacion") = ddlSituacion.SelectedValue.ToString()
            CargarGrilla(ViewState("p_codInterno"), ViewState("p_situacion"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

#End Region

#Region "Metodos Personalizados"
    Private Sub CargarPagina()
        If Request.QueryString("p_codInterno") Is Nothing Or _
         Request.QueryString("p_situacion") Is Nothing Then
            ViewState("p_codInterno") = ""
            ViewState("p_situacion") = "A"
        Else
            ViewState("p_codInterno") = Request.QueryString("p_codInterno")
            ViewState("p_situacion") = Request.QueryString("p_situacion")
        End If
        CargarSituacion()
        tbCodigoUsuario.Text = ViewState("p_codInterno")
        ddlSituacion.SelectedValue = ViewState("p_situacion")
        CargarGrilla(ViewState("p_codInterno"), ViewState("p_situacion"))
    End Sub

    Private Sub CargarSituacion()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dt As New DataTable
        dt = oParametrosGenerales.SeleccionarPorFiltro(ParametrosSIT.SITUACION, String.Empty, String.Empty, String.Empty, DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, dt, "Valor", "Nombre", True, "TODOS")
        ddlSituacion.SelectedIndex = 0
    End Sub

    Private Sub CargarGrilla(Optional ByVal codigoInterno As String = "", Optional ByVal situacion As String = "")
        Dim oAprobadorDocumento As New AprobadorDocumentoBM
        Dim oAprobadorDocumentoBE As New AprobadorDocumentoBE
        Dim oAprobadorDocumentoRow As AprobadorDocumentoBE.AprobadorDocumentoRow
        oAprobadorDocumentoRow = CType(oAprobadorDocumentoBE.AprobadorDocumento.NewRow(), AprobadorDocumentoBE.AprobadorDocumentoRow)
        oAprobadorDocumento.InicializarAprobadorDocumento(oAprobadorDocumentoRow)
        oAprobadorDocumentoRow.CodigoInterno = codigoInterno
        oAprobadorDocumentoRow.Situacion = situacion
        oAprobadorDocumentoBE.AprobadorDocumento.AddAprobadorDocumentoRow(oAprobadorDocumentoRow)
        oAprobadorDocumentoBE.AcceptChanges()

        Dim dt As New DataTable
        dt = oAprobadorDocumento.SeleccionarPorFiltro(oAprobadorDocumentoBE, DatosRequest).Tables(0)
        dgLista.DataSource = dt
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dt) + "');")
    End Sub
#End Region

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Select Case e.CommandName
                Case "Editar"
                    Response.Redirect("frmAprobadorReporte.aspx?codInterno=" & e.CommandArgument & "&p_codInterno=" & ViewState("p_codInterno") & "&p_situacion=" & ViewState("p_situacion"))
                Case "Eliminar"
                    Dim oAprobadorDocumento As New AprobadorDocumentoBM
                    oAprobadorDocumento.Eliminar(e.CommandArgument, DatosRequest)
                    CargarGrilla(tbCodigoUsuario.Text, ddlSituacion.SelectedValue)
                    AlertaJS("Cambios actualizados satisfactoriamente!")
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim chkAdmin As CheckBox = CType(e.Row.FindControl("chkAdmin"), CheckBox)
                Dim chkFirma As CheckBox = CType(e.Row.FindControl("chkFirma"), CheckBox)
                Dim chkOperador As CheckBox = CType(e.Row.FindControl("chkOperador"), CheckBox)
                chkAdmin.Attributes.Add("onclick", "return false")
                chkFirma.Attributes.Add("onclick", "return false")
                chkOperador.Attributes.Add("onclick", "return false")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar datos en la Grilla")
        End Try
    End Sub
End Class

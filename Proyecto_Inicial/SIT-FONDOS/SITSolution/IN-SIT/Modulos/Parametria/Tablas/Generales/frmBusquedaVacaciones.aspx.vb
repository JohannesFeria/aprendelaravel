Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaVacaciones
    Inherits BasePage
    Dim oMovimientoPersonalBM As New MovimientoPersonalBM
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoUsuario.Text = CType(Session("SS_DatosModal")(0), String)
                lbNombreUsuario.Text = CType(Session("SS_DatosModal")(1), String)
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmVacaciones.aspx?codInterno=" & ViewState("codigoInterno") & "&situacion=" & ViewState("situacion"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            ViewState("codigoInterno") = tbCodigoUsuario.Text.Trim()
            'ViewState("rol") = ddlRol.SelectedValue.ToString()
            ViewState("situacion") = ddlSituacion.SelectedValue.ToString()
            CargarGrilla(ViewState("situacion"), ViewState("codigoInterno"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub CargarPagina()
        If Request.QueryString("codInterno") Is Nothing Or _
            Request.QueryString("rol") Is Nothing Or _
            Request.QueryString("situacion") Is Nothing Then
            ViewState("codInterno") = ""
            ViewState("rol") = ""
            ViewState("situacion") = "A"
        Else
            ViewState("codInterno") = Request.QueryString("codInterno")
            ViewState("rol") = Request.QueryString("rol")
            ViewState("situacion") = Request.QueryString("situacion")
        End If
        CargarCombos()
        tbCodigoUsuario.Text = ViewState("codInterno")
        CargarGrilla(ViewState("situacion"), ViewState("codInterno"))
        lkbBuscarUsuario.Attributes.Add("onclick", "javascript:return showPopupUsuarios();")
    End Sub

    Private Sub CargarCombos()
        CargarRol()
        CargarSituacion()
    End Sub

    Private Sub CargarRol()
        Dim dt As New DataTable
        dt = New ParametrosGeneralesBM().Listar(ParametrosSIT.ROL_CARTA, DatosRequest)
        'HelpCombo.LlenarComboBox(ddlRol, dt, "Valor", "Nombre", True)
        ' ddlRol.SelectedValue = ViewState("rol")
    End Sub

    Private Sub CargarSituacion()
        Dim dt As New DataTable
        dt = New ParametrosGeneralesBM().Listar(ParametrosSIT.SITUACION, DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, dt, "Valor", "Nombre", True)
        ddlSituacion.SelectedValue = ViewState("situacion")
    End Sub
    Private Sub CargarGrilla(Optional ByVal situacion As String = "", Optional ByVal codigoInterno As String = "")
        Dim dt As DataTable = oMovimientoPersonalBM.SeleccionarPorFiltro(codigoInterno,situacion, DatosRequest).Tables(0)
        dgLista.DataSource = dt
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName = "Edit" Then
                Response.Redirect("frmVacaciones.aspx?cod=" & e.CommandArgument & "&codInterno=" & ViewState("codigoInterno") & "&situacion=" & ViewState("situacion"))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la grilla")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla(ViewState("situacion"), ViewState("codInterno"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
End Class

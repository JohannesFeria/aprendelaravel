Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data
Imports System.IO
Imports System.Drawing

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaBalanceContable
    Inherits BasePage
    Private CodigoEmisor As String

#Region " /* Metodos de Pagina */ "

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Confirms that an HtmlForm control is rendered for the specified ASP.NET
        '     server control at run time. 

    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Me.CargarCombos()
                Me.CargarGrilla()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoEmisor.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmBalanceContable.aspx?ope=reg")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            Dim strNombreReporte As String
            strNombreReporte = "BalanceContable_" & Date.Now.ToShortDateString().Replace("/", "_") & ".xls"
            Response.AddHeader("Content-Disposition", "attachment; filename= " & strNombreReporte)
            Response.ContentType = "application/vnd.ms-excel"
            Response.Charset = ""
            Me.EnableViewState = False
            Dim tw As New System.IO.StringWriter
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            dgExportar.Visible = True
            Me.dgExportar.RenderControl(hw)
            Response.Write(tw.ToString())
            Response.End()
            dgExportar.Visible = False
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Exportar")
        End Try        
    End Sub

    Private Sub btnImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Response.Redirect("frmBalanceContableImportar.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Importar")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
                Dim a As String = e.Row.Cells(6).Text = "A"
                Dim b As String = e.Row.Cells(7).Text = "A"
                Dim c As String = e.Row.Cells(8).Text = "A"

                If (e.Row.Cells(7).Text = "A") Then
                    e.Row.Cells(7).Text = "Activo"
                ElseIf (e.Row.Cells(7).Text = "I") Then
                    e.Row.Cells(7).Text = "Inactivo"
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If (e.CommandName = "Eliminar" Or e.CommandName = "Modificar") Then
                Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim Index As Integer = Row.RowIndex
                CodigoEmisor = IIf(Row.Cells(8).Text = "&nbsp;", "", Row.Cells(8).Text)
            End If

            Select Case e.CommandName
                Case "Eliminar"
                    Try
                        Dim oBalanceContableBM As New BalanceContableBM
                        Dim codigo As String = e.CommandArgument.ToString()
                        oBalanceContableBM.Eliminar(CodigoEmisor, Me.DatosRequest)
                        CargarGrilla()
                    Catch ex As Exception
                        AlertaJS(ex.ToString)
                    End Try
                Case "Modificar"
                    Response.Redirect("frmBalanceContable.aspx?ope=mod&codEm=" + CodigoEmisor)
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            Me.dgLista.DataSource = CType(Session("dtBalanceContable"), DataTable)
            Me.dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub Buscar()        
        CargarGrilla()
        Me.dgLista.PageIndex = 0
        If Me.dgLista.Rows.Count = 0 Then
            Me.AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
        Me.LimpiarConsulta()
    End Sub

    Private Sub CargarGrilla()
        Dim oBalanceContableBM As New BalanceContableBM
        Dim dt As DataTable
        Dim Tipo As String
        Dim situacion As String
        situacion = IIf(Me.ddlSituacion.SelectedIndex = 0, "", Me.ddlSituacion.SelectedValue)
        Me.CodigoEmisor = Me.tbCodigoEmisor.Text
        dt = New BalanceContableBM().SeleccionarPorFiltro(CodigoEmisor, situacion, DatosRequest).Tables(0)
        Session("dtBalanceContable") = dt
        Me.dgLista.DataSource = dt
        Me.dgLista.DataBind()
        dgExportar.DataSource = dt
        dgExportar.DataBind()

        EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub LimpiarConsulta()
        Me.tbCodigoEmisor.Text = ""
        Me.ddlSituacion.SelectedIndex = 0
    End Sub

#End Region

End Class

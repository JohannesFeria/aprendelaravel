Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Parametria_AdministracionPortafolios_frmBusquedaPortafolio
    Inherits BasePage
#Region " /* Metodos de Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Page.IsPostBack = False Then
                CargarFiltros()
                Buscar()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmAdministracionPortafolios.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            dgLista.PageIndex = 0
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("btnEliminar"), ImageButton).Attributes.Add("OnClick", "return confirm('¿Confirmar la eliminación del registro?')")
                Dim sFecha As String = e.Row.Cells(4).Text.Trim.Replace("&nbsp;", "")
                If sFecha = "" Then
                    e.Row.Cells(4).Text = ""
                Else
                    e.Row.Cells(4).Text = UIUtility.ConvertirDecimalAStringFormatoFecha(sFecha)
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName = "Modificar" Then
                Dim codigo As String = e.CommandArgument
                Response.Redirect("frmAdministracionPortafolios.aspx?codigo=" & codigo)
            ElseIf e.CommandName = "Eliminar" Then
                Try
                    Dim oPortafolioBM As New PortafolioBM
                    'oPortafolioBM.EliminarDetalle(e.CommandArgument, DatosRequest)
                    oPortafolioBM.Eliminar(e.CommandArgument, DatosRequest)
                    CargarGrilla()
                Catch ex As Exception
                    AlertaJS(ex.Message.ToString)
                End Try
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarGrilla()
        Dim oPortafolioBM As New PortafolioBM
        Dim strSituacion As String
        strSituacion = IIf(Me.ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, Me.ddlSituacion.SelectedValue).ToString()
        Dim dtblDatos As DataTable = oPortafolioBM.SeleccionarPorFiltros(Me.txtDescripcion.Text, strSituacion, Me.DatosRequest).Portafolio
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")
    End Sub

    Private Sub Buscar()
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Private Sub CargarFiltros()
        Dim dtSituacion As DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        dtSituacion = oParametrosGenerales.ListarSituacion(Me.DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, dtSituacion, "Valor", "Nombre", True)
    End Sub

#End Region

End Class

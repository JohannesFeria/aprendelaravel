Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaGrupoTipoRenta
    Inherits BasePage

    Private GrupoInstrumento As String
    Private Descripcion As String

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Me.CargarCombos()
                Me.CargarGrilla()
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
            Response.Redirect("frmGrupoTipoRenta.aspx?ope=reg")
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

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
                If (e.Row.Cells(4).Text = "A") Then
                    e.Row.Cells(4).Text = "Activo"
                ElseIf (e.Row.Cells(4).Text = "I") Then
                    e.Row.Cells(4).Text = "Inactivo"
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
                GrupoInstrumento = IIf(Row.Cells(2).Text = "&nbsp;", "", Row.Cells(2).Text)
            End If

            Select Case e.CommandName
                Case "Eliminar"
                    Try
                        Dim oGrupoTipoRentaBM As New GrupoTipoRentaBM
                        Dim codigo As String = e.CommandArgument.ToString()
                        oGrupoTipoRentaBM.Eliminar(GrupoInstrumento, Me.DatosRequest)
                        CargarGrilla()
                    Catch ex As Exception
                        AlertaJS(ex.ToString)
                    End Try
                Case "Modificar"
                    Response.Redirect("frmGrupoTipoRenta.aspx?ope=mod&codGru=" + GrupoInstrumento)
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            Me.dgLista.DataSource = CType(Session("dtGrupoTipoRenta"), DataTable)
            Me.dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub Buscar()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oGrupoTipoRentaBM As New GrupoTipoRentaBM
        Dim dt As DataTable
        Dim Tipo As String
        Dim situacion As String
        situacion = IIf(Me.ddlSituacion.SelectedIndex = 0, "", Me.ddlSituacion.SelectedValue)

        dt = New GrupoTipoRentaBM().SeleccionarPorFiltro(Me.tbGrupoInstrumento.Text, Me.tbDescripcion.Text, situacion, "Cabecera", DatosRequest).Tables(0)
        Session("dtGrupoTipoRenta") = dt
        Me.dgLista.DataSource = dt
        Me.dgLista.DataBind()
        EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub LimpiarConsulta()
        Me.ddlSituacion.SelectedIndex = 0
    End Sub

#End Region

End Class

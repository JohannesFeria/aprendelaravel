Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaGrupoTipoInstrumento
    Inherits BasePage
    Private GrupoInstrumento As String
    Private Descripcion As String

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarGrilla()
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
            Response.Redirect("frmGrupoTipoInstrumento.aspx?ope=reg")
        Catch ex As Exception
            AlertaJS("Ocurrió un error a l Ingresar")
        End Try        
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            dgLista.DataSource = CType(Session("dtGrupoTipoInstrumento"), DataTable)
            dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Select Case e.CommandName
                Case "Eliminar"
                    Try
                        Dim oGrupoTipoInstrumentoBM As New GrupoTipoInstrumentoBM
                        Dim codigo As String = e.CommandArgument.ToString()
                        oGrupoTipoInstrumentoBM.Eliminar(codigo, DatosRequest)
                        CargarGrilla()
                    Catch ex As Exception
                        AlertaJS(ex.ToString())
                    End Try
                Case "Modificar"
                    Response.Redirect("frmGrupoTipoInstrumento.aspx?ope=mod&codGru=" + e.CommandArgument.ToString())
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
                If (e.Row.Cells(4).Text = "A") Then
                    e.Row.Cells(4).Text = "Activo"
                ElseIf (e.Row.Cells(4).Text = "I") Then
                    e.Row.Cells(4).Text = "Inactivo"
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub Buscar()        
        CargarGrilla()
        If dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oGrupoTipoInstrumentoBM As New GrupoTipoInstrumentoBM
        Dim dt As DataTable
        Dim situacion As String
        situacion = IIf(ddlSituacion.SelectedIndex = 0, "", ddlSituacion.SelectedValue)

        dt = New GrupoTipoInstrumentoBM().SeleccionarPorFiltro(tbGrupoInstrumento.Text, tbDescripcion.Text, situacion, "Cabecera", DatosRequest).Tables(0)
        Session("dtGrupoTipoInstrumento") = dt
        dgLista.DataSource = dt
        dgLista.DataBind()
        dgLista.PageIndex = 0
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

#End Region

End Class

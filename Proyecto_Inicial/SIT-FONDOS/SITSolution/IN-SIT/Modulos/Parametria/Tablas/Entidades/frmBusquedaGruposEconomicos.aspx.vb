Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Entidades_frmBusquedaGruposEconomicos
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()            
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmGruposEconomicos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        Try
            LimpiarConsulta()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Consultar")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try        
    End Sub

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmGruposEconomicos.aspx?cod=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar Registro")
        End Try
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oGrupoEconomicoBM As New GrupoEconomicoBM
            Dim codigo As String = e.CommandArgument.ToString()
            oGrupoEconomicoBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Eliminar Registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub Buscar()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oGrupoEconomicoBM As New GrupoEconomicoBM
        Dim dtblDatos As DataTable = oGrupoEconomicoBM.SeleccionarPorFiltro(Me.tbCodigo.Text.Trim, tbDescripcion.Text.TrimStart.TrimEnd, IIf(ddlSituacion.SelectedValue = Constantes.M_STR_TEXTO_TODOS, String.Empty, ddlSituacion.SelectedValue).ToString(), DatosRequest).Tables(0)

        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()        
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)

    End Sub

    Private Sub LimpiarConsulta()
        Me.tbCodigo.Text = ""
        Me.tbDescripcion.Text = ""
        Me.ddlSituacion.SelectedIndex = 0

    End Sub

#End Region

End Class

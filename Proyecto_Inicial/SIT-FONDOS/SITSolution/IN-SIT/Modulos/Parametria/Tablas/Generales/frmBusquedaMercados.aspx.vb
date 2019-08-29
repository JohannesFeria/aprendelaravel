Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaMercados
    Inherits BasePage

#Region " */ Metodos de Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Me.dgLista.PageIndex = 0
            CargarGrilla()
            If Me.dgLista.Rows.Count = 0 Then                
                AlertaJS("No se encontraron Registros")
            End If
        Catch ex As Exception            
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmMercado.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

#End Region

#Region " /* Funciones Modificar */"
    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmMercado.aspx?cod=" & e.CommandArgument)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub
#End Region

#Region " /* Funciones Eliminar */"
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oMercadoBM As New MercadoBM
            Dim codigo As String = e.CommandArgument
            oMercadoBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub
#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            CargarGrilla()
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oMercadoBM As New MercadoBM
        Dim strSituacion As String

        If ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS) Then
            strSituacion = String.Empty
        Else
            strSituacion = Me.ddlSituacion.SelectedValue
        End If

        Me.dgLista.DataSource = oMercadoBM.SeleccionarPorFiltro(Me.txtCodigo.Text.Trim, txtDescripcion.Text.TrimStart.TrimEnd, strSituacion, DatosRequest).Tables(0)
        Me.dgLista.DataBind()        
        Dim cantidad As Int32 = oMercadoBM.SeleccionarPorFiltro(Me.txtCodigo.Text.Trim, txtDescripcion.Text.TrimStart.TrimEnd, strSituacion, DatosRequest).Tables(0).Rows.Count
        EjecutarJS("$('#" + lblContador.ClientID + "').text('" + "Registro(s) Encontrado(s): " + cantidad.ToString() + "');")
    End Sub

    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Public Sub LimpiarConsulta()

        Me.txtCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlSituacion.SelectedIndex = 0

    End Sub
#End Region

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar los datos en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
End Class

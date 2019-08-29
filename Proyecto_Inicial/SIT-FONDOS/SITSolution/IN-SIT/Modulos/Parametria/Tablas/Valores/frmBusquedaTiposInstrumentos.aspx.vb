Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaTiposInstrumentos
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarDatosGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Carga de la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            dgLista.PageIndex = 0
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try        
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmTiposInstrumentos.aspx", False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar a la página")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de la página")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("OnClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim codigo As String = e.CommandArgument
            Response.Redirect("frmTiposInstrumentos.aspx?cod=" & codigo)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar el registro")
        End Try        
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oTipoInstrumentoBM As New TipoInstrumentoBM
            Dim codigo As String = e.CommandArgument
            oTipoInstrumentoBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Eliminar el Registro")
        End Try
    End Sub

    Private Sub CargarDatosGrilla()
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim strSinonimo, strClaseInstrumento, strSituacion, strTipoRenta As String
        strClaseInstrumento = IIf(Me.ddlClaseInstrumento.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, Me.ddlClaseInstrumento.SelectedValue).ToString()
        strSituacion = IIf(Me.ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, Me.ddlSituacion.SelectedValue).ToString()
        strTipoRenta = IIf(Me.ddlTipoRenta.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, Me.ddlTipoRenta.SelectedValue).ToString()
        Dim dtblDatos As DataTable = oTipoInstrumentoBM.SeleccionarPorFiltro(strClaseInstrumento, String.Empty, strTipoRenta, strSituacion, DatosRequest).Tables(0)
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()        
        EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")
    End Sub

    Public Sub CargarCombos()
        Dim tablaClaseInstrumento As New Data.DataTable
        Dim tablaTipoRenta As New Data.DataTable
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
        tablaClaseInstrumento = oClaseInstrumentoBM.Listar(DatosRequest).Tables(0)
        Dim oTipoRentaBM As New TipoRentaBM
        tablaTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(ddlClaseInstrumento, tablaClaseInstrumento, "CodigoClaseInstrumento", "Descripcion", True)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoRenta, tablaTipoRenta, "CodigoRenta", "Descripcion", True)
    End Sub

#End Region

End Class

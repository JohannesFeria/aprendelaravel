Option Explicit On
Option Strict On

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaTiposTitulos
    Inherits BasePage


#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
                CargarDatosGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
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
            Response.Redirect("frmTiposTitulos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar la página")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub

#End Region

#Region " /* Funciones Modificar */"
    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim codigo As String = e.CommandArgument.ToString()
            Response.Redirect("frmTiposTitulos.aspx?cod=" & codigo)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Eliminar */"

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oTipoTituloBM As New TipoTituloBM
            Dim codigo As String = e.CommandArgument.ToString()
            oTipoTituloBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Eliminar el registro")
        End Try
    End Sub
#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oTipoTituloBM As New TipoTituloBM
        Dim strSituacion, strDescripcion, strCodigoMoneda, strCodigoTipoInstrumento, strCodigo As String

        strSituacion = ddlSituacion.SelectedValue
        strCodigoMoneda = ddlMoneda.SelectedValue
        strCodigoTipoInstrumento = ddlTipoInstrumento.SelectedValue

        If strSituacion.Equals(Constantes.M_STR_TEXTO_TODOS) Then
            strSituacion = String.Empty
        End If

        If strCodigoMoneda.Equals(Constantes.M_STR_TEXTO_TODOS) Then
            strCodigoMoneda = String.Empty
        End If

        If strCodigoTipoInstrumento.Equals(Constantes.M_STR_TEXTO_TODOS) Then
            strCodigoTipoInstrumento = String.Empty
        End If

        strCodigo = tbCodigo.Text.ToUpper.Trim
        strDescripcion = tbDescripcion.Text.ToUpper.Trim
        Dim dtblDatos As DataTable = oTipoTituloBM.SeleccionarPorFiltro(String.Empty, strCodigoMoneda, strCodigoTipoInstrumento, strDescripcion, strCodigo, strSituacion, DatosRequest).Tables(0)

        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")
    End Sub

    Private Sub CargarCombos()

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        Dim dtMoneda As DataTable
        Dim oMonedaBM As New MonedaBM

        Dim dtTipoInstrumento As DataTable
        Dim oTipoInstrumento As New TipoInstrumentoBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)

        dtMoneda = oMonedaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(ddlMoneda, dtMoneda, "CodigoMoneda", "Descripcion", True)

        dtTipoInstrumento = oTipoInstrumento.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(ddlTipoInstrumento, dtTipoInstrumento, "CodigoTipoInstrumentoSBS", "Descripcion", True)
    End Sub

    Private Sub CargarDatosGrilla()
        CargarGrilla()
        If dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub
#End Region

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try        
    End Sub
End Class

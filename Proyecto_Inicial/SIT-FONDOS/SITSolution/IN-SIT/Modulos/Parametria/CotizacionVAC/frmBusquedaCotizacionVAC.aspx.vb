Option Explicit On
Option Strict Off
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility

Partial Class Modulos_Parametria_CotizacionVAC_frmBusquedaCotizacionVAC
    Inherits BasePage

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarFiltros()
                Buscar()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un eror al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmCotizacionVAC.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try        
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Private Sub btnImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Response.Redirect("frmCotizacionVACImportar.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Importar")
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

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim imgEliminar As ImageButton
                imgEliminar = DirectCast(e.Row.FindControl("ibEliminar"), ImageButton)
                imgEliminar.Attributes.Add("onclick", ConfirmJS(Constantes.M_STR_MENSAJE_PREGUNTA_ELIMINAR_ENTIDAD))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

#End Region

#Region "/* Metodos Personalizados */"

    Private Sub Buscar()
        dgLista.PageIndex = 0
        CargarGrilla()
        If dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmCotizacionVAC.aspx?codigo=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try        
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oIndicadorBM As New IndicadorBM
            Dim strCodigoIndicador As String

            strCodigoIndicador = e.CommandArgument.ToString().Split(","c)(1)
            oIndicadorBM.Eliminar(strCodigoIndicador, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try       
    End Sub

    Private Sub CargarGrilla()
        Dim oIndicadorBM As New IndicadorBM

        Dim CodigoIndicador, NombreIndicador, Situacion, ManejaPeriodo As String
        Dim sTipoIndicador As String
        CodigoIndicador = txtCodigoIndicador.Text
        NombreIndicador = txtNombreIndicador.Text
        If (ddlSituacion.SelectedValue.ToString = "--Seleccione--") Then
            Situacion = ""
        Else
            Situacion = ddlSituacion.SelectedValue.ToString
        End If
        If (ddlManejaPeriodo.SelectedValue.ToString = "--Seleccione--") Then
            ManejaPeriodo = ""
        Else
            ManejaPeriodo = ddlManejaPeriodo.SelectedValue.ToString
        End If


        If (dllTipoIndicador.SelectedValue.ToString = "--Seleccione--") Then
            sTipoIndicador = ""
        Else
            sTipoIndicador = dllTipoIndicador.SelectedValue.ToString
        End If

        Dim dtblDatos As DataTable = oIndicadorBM.SeleccionarPorFiltro(CodigoIndicador, NombreIndicador, Situacion, 0, 0, 0, ManejaPeriodo, "", "", DatosRequest, sTipoIndicador).Tables(0)
        dgLista.DataSource = dtblDatos
        dgLista.DataBind()

        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub

    Private Sub CargarFiltros()
        Dim dtSituacion, dtManejaPeriodo As DataTable
        Dim dtTipoIndicador As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtSituacion = oParametrosGeneralesBM.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, dtSituacion, "Valor", "Nombre", True)

        dtManejaPeriodo = oParametrosGeneralesBM.Listar("Desicion", DatosRequest)
        HelpCombo.LlenarComboBox(ddlManejaPeriodo, dtManejaPeriodo, "Valor", "Nombre", True)

        dtTipoIndicador = oParametrosGeneralesBM.Listar("TipoIndica", DatosRequest)
        HelpCombo.LlenarComboBox(dllTipoIndicador, dtTipoIndicador, "Valor", "Nombre", True)
    End Sub

#End Region

End Class

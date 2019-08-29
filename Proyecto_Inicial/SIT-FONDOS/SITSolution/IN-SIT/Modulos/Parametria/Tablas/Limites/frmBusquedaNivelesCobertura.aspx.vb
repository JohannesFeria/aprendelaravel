Option Explicit On
Option Strict On

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Limites_frmBusquedaNivelesCobertura
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
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Ingresar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try        
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Cancelar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            EditarCelda(e.Row)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
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

#End Region

#Region "/* Metodos Personalizados */"

    Private Sub CargarGrilla()
        Dim oNivelesCoberturaBM As New NivelesCoberturaBM
        Dim strSituacion, strCodigoTercero As String

        strSituacion = IIf(Me.ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlSituacion.SelectedValue).ToString()
        strCodigoTercero = IIf(Me.ddlTercero.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlTercero.SelectedValue).ToString()
        Dim dtblDatos As DataTable = oNivelesCoberturaBM.SeleccionarPorFiltro_sura(strCodigoTercero, strSituacion)

        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub

    Private Sub CargarFiltros()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dtTerceros As DataTable
        Dim oTerceroBM As New TercerosBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
        dtTerceros = oTerceroBM.Listar(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTercero, dtTerceros, "CodigoTercero", "Descripcion", True)
    End Sub

    Private Sub Buscar()
        dgLista.PageIndex = 0
        CargarGrilla()
        If dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Response.Redirect("frmNivelesCobertura.aspx?codigo=" & e.CommandArgument.ToString())
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oNivelesCoberturaBM As New NivelesCoberturaBM
            Dim strCodigoTercero As String

            strCodigoTercero = e.CommandArgument.ToString().Split(","c)(0)
            oNivelesCoberturaBM.Eliminar(strCodigoTercero, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub Ingresar()
        Response.Redirect("frmNivelesCobertura.aspx")
    End Sub

    Private Sub Cancelar()
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub EditarCelda(ByVal oDataGridItem As GridViewRow)        
        If oDataGridItem.RowType = DataControlRowType.DataRow Then
            Dim imgEliminar As ImageButton
            imgEliminar = DirectCast(oDataGridItem.FindControl("ibEliminar"), ImageButton)
            imgEliminar.Attributes.Add("onclick", ConfirmJS(Constantes.M_STR_MENSAJE_PREGUNTA_ELIMINAR_ENTIDAD))
        End If
    End Sub

#End Region

End Class

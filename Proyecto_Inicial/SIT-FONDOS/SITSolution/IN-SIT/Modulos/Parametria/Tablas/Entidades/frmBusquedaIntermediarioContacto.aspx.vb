Imports SIT.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Entidades_frmBusquedaIntermediarioContacto
    Inherits BasePage

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarFiltros()
                Buscar()
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
            Response.Redirect("frmIntermediarioContacto.aspx")
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
            Response.Redirect("frmIntermediarioContacto.aspx?codigo=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar Registro")
        End Try
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oIntermediarioContactoBM As New IntermediarioContactoBM
            Dim strCodigoContacto, strCodigoTercero As String
            strCodigoTercero = e.CommandArgument.ToString().Split(","c)(0)
            strCodigoContacto = e.CommandArgument.ToString().Split(","c)(1)
            oIntermediarioContactoBM.Eliminar(strCodigoTercero, strCodigoContacto, Me.DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Eliminar Registro")
        End Try
    End Sub

#End Region

#Region "/* Metodos Personalizados */"

    Private Sub CargarGrilla()
        Dim oIntermediarioContactoBM As New IntermediarioContactoBM
        Dim strSituacion, strCodigoTercero, strCodigoContacto As String
        strSituacion = IIf(Me.ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, Me.ddlSituacion.SelectedValue).ToString()
        strCodigoTercero = IIf(Me.ddlTercero.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, Me.ddlTercero.SelectedValue).ToString()
        strCodigoContacto = IIf(Me.ddlContacto.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, Me.ddlContacto.SelectedValue).ToString()
        Dim dtblDatos As DataTable = oIntermediarioContactoBM.SeleccionarPorFiltro(strCodigoTercero, strCodigoContacto, strSituacion, Me.DatosRequest).IntermediarioContacto
        Dim i As Integer = dtblDatos.Rows.Count
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        Me.lbContador.Text = UIUtility.MostrarResultadoBusqueda(dtblDatos)
    End Sub

    Private Sub CargarFiltros()

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)

        Dim dtContacto As DataTable
        Dim oContactoBM As New ContactoBM

        dtContacto = oContactoBM.Listar(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlContacto, dtContacto, "CodigoContacto", "Descripcion", True)

        Dim dtTerceros As DataTable
        Dim oTerceroBM As New TercerosBM

        dtTerceros = oTerceroBM.Listar(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTercero, dtTerceros, "CodigoTercero", "Descripcion", True)

    End Sub

    Private Sub Buscar()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

#End Region

End Class

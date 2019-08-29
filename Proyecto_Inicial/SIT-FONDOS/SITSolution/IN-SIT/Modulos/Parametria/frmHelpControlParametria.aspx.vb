Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_frmHelpControlParametria
    Inherits BasePage

#Region "/* Metodos Personalizados */"

    Private Sub ConstruirGrilla()
        Dim strNombreTabla As String
        Dim oColumn As BoundField
        strNombreTabla = ViewState("NombreTabla").ToString()
        If strNombreTabla.Equals("Terceros") Then
            DirectCast(Me.dgLista.Columns(1), BoundField).DataField = "CodigoTercero"
            DirectCast(Me.dgLista.Columns(2), BoundField).DataField = "Descripcion"
            DirectCast(Me.dgLista.Columns(3), BoundField).DataField = "CodigoPais" 'CMB OT 66768 20130530
            DirectCast(Me.dgLista.Columns(3), BoundField).Visible = False 'CMB OT 66768 20130530 
        ElseIf strNombreTabla.Equals("Entidad") Then
            DirectCast(Me.dgLista.Columns(1), BoundField).DataField = "CodigoEntidad"
            DirectCast(Me.dgLista.Columns(2), BoundField).DataField = "NombreCompleto"
        ElseIf strNombreTabla.Equals("Contacto") Then
            DirectCast(Me.dgLista.Columns(1), BoundField).DataField = "CodigoContacto"
        ElseIf strNombreTabla.Equals("TipoInstrumento") Then
            DirectCast(Me.dgLista.Columns(1), BoundField).DataField = "CodigoTipoInstrumento"
            DirectCast(Me.dgLista.Columns(2), BoundField).DataField = "Descripcion"
        ElseIf strNombreTabla.Equals("Personal") Then
            DirectCast(Me.dgLista.Columns(1), BoundField).DataField = "CodigoInterno"
            DirectCast(Me.dgLista.Columns(2), BoundField).DataField = "NombreCompleto"
            DirectCast(Me.dgLista.Columns(3), BoundField).DataField = String.Empty
            DirectCast(Me.dgLista.Columns(3), BoundField).Visible = False
        ElseIf strNombreTabla.Equals("Broker") Then
            DirectCast(Me.dgLista.Columns(1), BoundField).DataField = "CodigoEntidad"
            DirectCast(Me.dgLista.Columns(2), BoundField).DataField = "Descripcion"
            DirectCast(Me.dgLista.Columns(3), BoundField).DataField = "Situacion"
        ElseIf strNombreTabla.Equals("Rating") Then
            DirectCast(Me.dgLista.Columns(1), BoundField).DataField = "Valor"
            DirectCast(Me.dgLista.Columns(2), BoundField).DataField = "Nombre"
            DirectCast(Me.dgLista.Columns(3), BoundField).DataField = String.Empty
            DirectCast(Me.dgLista.Columns(3), BoundField).Visible = False
            DirectCast(Me.dgLista.Columns(4), BoundField).DataField = String.Empty
            DirectCast(Me.dgLista.Columns(4), BoundField).Visible = False
        ElseIf strNombreTabla.Equals("ValoresNemonicoFuturo") Then
            DirectCast(Me.dgLista.Columns(1), BoundField).DataField = "Codigointerno"
            DirectCast(Me.dgLista.Columns(2), BoundField).DataField = "Descripcion"
            DirectCast(Me.dgLista.Columns(3), BoundField).DataField = "ContractSize"
            DirectCast(Me.dgLista.Columns(3), BoundField).Visible = False
            DirectCast(Me.dgLista.Columns(4), BoundField).DataField = "CodigoSBS"
        End If
    End Sub

    Private Sub CargarPagina()
        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If Not Page.IsPostBack Then
            ViewState("NombreTabla") = Request.QueryString("tlbBusqueda")
            Me.lblTitulo.Text = Request.QueryString("tlbBusqueda")

            ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-24 
            If Request.QueryString("codSelec") IsNot Nothing Then 'Si nos proporcionan el código seleccionado inicializamos la búsqueda
                txtCodigo.Text = Request.QueryString("codSelec")
                btnBuscar_Click(Nothing, Nothing)
            End If
            ' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-24 
        End If
    End Sub
    Private Sub Buscar()
        Dim strNombreTabla As String
        Dim dtResultado As DataTable
        strNombreTabla = ViewState("NombreTabla").ToString()
        ConstruirGrilla()
        If strNombreTabla.Equals("Terceros") Then
            Dim oTercerosBM As New TercerosBM
            dtResultado = oTercerosBM.SeleccionarPorFiltroActivo(Me.txtCodigo.Text, String.Empty, Me.txtDescripcion.Text, String.Empty, String.Empty, String.Empty, Me.DatosRequest).Terceros
        ElseIf strNombreTabla.Equals("Entidad") Then
            Dim oEntidadBM As New EntidadBM
            dtResultado = oEntidadBM.SeleccionarPorFiltro(Me.txtCodigo.Text, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, Me.txtDescripcion.Text, Me.DatosRequest).Entidad
        ElseIf strNombreTabla.Equals("Contacto") Then
            Dim oContactoBM As New ContactoBM
            dtResultado = oContactoBM.SeleccionarPorFiltro(Me.txtCodigo.Text, Me.txtDescripcion.Text, String.Empty, Me.DatosRequest).Contacto
        ElseIf strNombreTabla.Equals("Personal") Then
            Dim oPersonalBM As New PersonalBM
            dtResultado = oPersonalBM.SeleccionarPorFiltro(Me.txtCodigo.Text, Me.txtDescripcion.Text, Me.DatosRequest).Personal
        ElseIf strNombreTabla.Equals("Valores") Then
            Dim oValoresBM As New ValoresBM
            dtResultado = oValoresBM.SeleccionarPorFiltroValores(Me.txtCodigo.Text, Me.txtDescripcion.Text, Me.DatosRequest).Valor
        ElseIf strNombreTabla.Equals("ValoresNemonico") Then
            Dim oValoresBM As New ValoresBM
            dtResultado = oValoresBM.SeleccionarPorFiltroNemonico(Me.txtCodigo.Text, Me.txtDescripcion.Text, Me.DatosRequest).Valor
        ElseIf strNombreTabla.Equals("ValoresNemonicoAprob") Then
            Dim oValoresBM As New ValoresBM
            dtResultado = oValoresBM.SeleccionarPorFiltroValoresAprob(Me.txtCodigo.Text, Me.txtDescripcion.Text, Me.DatosRequest).Valor
        ElseIf strNombreTabla.Equals("TipoInstrumento") Then
            Dim oTipoInstrumentoBM As New TipoInstrumentoBM
            dtResultado = oTipoInstrumentoBM.SeleccionarPorCodigoyDescripcion(Me.txtCodigo.Text, Me.txtDescripcion.Text, Me.DatosRequest).TipoInstrumento
        ElseIf strNombreTabla.Equals("Broker") Then
            Dim oBroker As New EntidadBM
            dtResultado = oBroker.ListarBroker(Me.txtCodigo.Text.Trim, Me.txtDescripcion.Text.Trim)
        ElseIf strNombreTabla.Equals("Rating") Then
            Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
            dtResultado = oParametrosGeneralesBM.SeleccionarPorFiltro(ParametrosSIT.RATING, Me.txtDescripcion.Text, Me.txtCodigo.Text, "", DatosRequest)
        ElseIf strNombreTabla.Equals("ValoresNemonicoFuturo") Then
            Dim oValoresBM As New ValoresBM

            dtResultado = oValoresBM.SeleccionarPorFiltroValoresFuturo(Me.txtCodigo.Text, Me.txtDescripcion.Text, Me.DatosRequest).Valor
        End If
        If Not dtResultado.Columns.Contains("codigoSBS") Then
            dtResultado.Columns.Add("codigoSBS")
        End If
        Me.dgLista.DataSource = dtResultado
        Me.dgLista.DataBind()
    End Sub

    Private Sub ActualizarIndice(ByVal nuevoIndice As Integer)
        Me.dgLista.PageIndex = nuevoIndice
        Buscar()
    End Sub

    Private Sub EditarCelda(ByVal oDataGridItem As GridViewRow)
        Dim lkbSeleccionar As LinkButton
        Dim strNombreTabla As String
        If oDataGridItem.RowType = DataControlRowType.DataRow Then
            strNombreTabla = ViewState("NombreTabla").ToString()
            lkbSeleccionar = DirectCast(oDataGridItem.FindControl("lkbSeleccionar"), LinkButton)
            Session("sCodigo") = oDataGridItem.Cells(1).Text
            Session("Descripcion") = oDataGridItem.Cells(2).Text
            Session("3") = oDataGridItem.Cells(3).Text
            Session("4") = oDataGridItem.Cells(4).Text
        End If

    End Sub

#End Region

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CargarPagina()
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        dgLista.PageIndex = 0
        Buscar()
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            EditarCelda(e.Row)
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            Buscar()
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName = "Seleccionar" Then
                Dim Index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim Row As GridViewRow = dgLista.Rows(Index)
                Dim arraySesiones As String() = New String(2) {}
                arraySesiones(0) = HttpUtility.HtmlDecode(Row.Cells(1).Text)
                arraySesiones(1) = HttpUtility.HtmlDecode(Row.Cells(2).Text)
                arraySesiones(2) = HttpUtility.HtmlDecode(Row.Cells(4).Text)
                Session("SS_DatosModal") = arraySesiones
                EjecutarJS("window.close();")
            End If
        Catch ex As Exception
            AlertaJS(ex.ToString)
        End Try
    End Sub

#End Region


End Class

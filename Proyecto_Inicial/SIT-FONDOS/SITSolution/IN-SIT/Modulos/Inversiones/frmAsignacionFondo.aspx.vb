Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports ParametrosSIT
Partial Class Modulos_Inversiones_frmAsignacionFondo
    Inherits BasePage
    Dim codigoprevordeninversion As String
    Dim porcentaje As String
#Region " /* Metodos de Pagina */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            codigoprevordeninversion = Request.QueryString("codigoprevorden").Trim
            porcentaje = Request.QueryString("porcentaje").Trim
            If Not Page.IsPostBack Then
                CargarCombos()
                cargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Sub cargarGrilla()
        Dim dtportafolio As DataTable
        dtportafolio = Session("dtDetalleInversiones" & codigoprevordeninversion)
        If Not dtportafolio Is Nothing Then
            dgLista.DataSource = dtportafolio
            dgLista.DataBind()
            Exit Sub
        End If
        Dim codigopreorden As Integer
        If porcentaje <> "" Then
            If codigoprevordeninversion <> "" Then
                codigopreorden = Integer.Parse(codigoprevordeninversion)
            End If
        Else
            codigopreorden = 0
        End If
        dtportafolio = New PrevOrdenInversionBM().SeleccionarDetallePreOrdenInversion(codigopreorden)
        If dtportafolio.Rows.Count = 0 Then
            llenarFilaVacia(dtportafolio)
            dgLista.DataSource = dtportafolio
            dgLista.DataBind()
            dgLista.Rows(0).Visible = False
        Else
            dgLista.DataSource = dtportafolio
            dgLista.DataBind()
        End If
    End Sub
    Private Sub llenarFilaVacia(ByRef table As DataTable)
        Dim row As DataRow = table.NewRow()
        For Each item As DataColumn In table.Columns
            Select Case item.DataType
                Case GetType(String)
                    row(item.ColumnName) = String.Empty
                Case GetType(Decimal)
                    row(item.ColumnName) = 0.0
            End Select
        Next
        table.Rows.Add(row)
    End Sub
#End Region
#Region " /* Funciones Personalizadas */"
    Public Sub CargarCombos()
        Dim dtPortafolio As DataTable = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO)
        Session("portafolio") = dtPortafolio
    End Sub
    Public Function instanciarTabla(tabla As DataTable) As DataTable
        tabla = New DataTable
        tabla.Columns.Add("CodigoPrevOrden")
        tabla.Columns.Add("CodigoPortafolio")
        tabla.Columns.Add("Asignacion")
        tabla.Columns.Add("Cambio")
        Return tabla
    End Function
#End Region
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se agregan validaciones de integra
    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim codprevorden As String = e.CommandArgument
            Dim i As Integer
            Dim dtDetalle As DataTable
            If e.CommandName = "Modificar" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim preordeninversion As New PrevOrdenInversionBM
                Dim lblportafolio As Label
                Dim txtMontoAsignado As TextBox
                Dim auxMontoAsignado As TextBox
                auxMontoAsignado = CType(gvr.FindControl("txtMontoAsignado"), TextBox)
                If auxMontoAsignado.Text = "" Then
                    AlertaJS("Debe asignar un monto al portafolio")
                    Exit Sub
                End If
                If Double.Parse(auxMontoAsignado.Text.Trim) <= 0 Then
                    AlertaJS("Debe asignar un monto mayor a cero")
                    Exit Sub
                End If
                dtDetalle = instanciarTabla(dtDetalle)
                For i = 0 To dgLista.Rows.Count - 1
                    lblportafolio = CType(dgLista.Rows(i).FindControl("lblportafolio"), Label)
                    txtMontoAsignado = CType(dgLista.Rows(i).FindControl("txtMontoAsignado"), TextBox)
                    dtDetalle.Rows.Add(codigoprevordeninversion, lblportafolio.Text.Trim, txtMontoAsignado.Text.Trim, "S")
                Next
                dgLista.DataSource = dtDetalle
                dgLista.DataBind()
                Session("dtDetalleInversiones" & codigoprevordeninversion) = dtDetalle
            End If
            If e.CommandName = "Add" Then
                Dim codigoprevorden As Integer
                If codigoprevordeninversion <> "" Then
                    codigoprevorden = Integer.Parse(codigoprevordeninversion)
                Else
                    codigoprevorden = 0
                End If
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim preordeninversion As New PrevOrdenInversionBM
                Dim lblportafolio As Label
                Dim txtMontoAsignado As TextBox
                Dim ddlportafoliof As DropDownList
                Dim txtMontoAsignadof As TextBox
                ddlportafoliof = CType(gvr.FindControl("ddlPortafolioF"), DropDownList)
                txtMontoAsignadof = CType(gvr.FindControl("txtMontoAsignadoF"), TextBox)
                If txtMontoAsignadof.Text = "" Then
                    AlertaJS("Debe asignar un monto al portafolio")
                    Exit Sub
                End If
                If Double.Parse(txtMontoAsignadof.Text.Trim) <= 0 Then
                    AlertaJS("Debe asignar un monto mayor a cero")
                    Exit Sub
                End If
                dtDetalle = instanciarTabla(dtDetalle)
                For i = 0 To dgLista.Rows.Count - 1
                    lblportafolio = CType(dgLista.Rows(i).FindControl("lblportafolio"), Label)
                    txtMontoAsignado = CType(dgLista.Rows(i).FindControl("txtMontoAsignado"), TextBox)
                    If txtMontoAsignado.Text.Trim <> "0" Then
                        dtDetalle.Rows.Add(codigoprevordeninversion, lblportafolio.Text.Trim, txtMontoAsignado.Text.Trim, "S")
                    End If
                Next
                For i = 0 To dtDetalle.Rows.Count - 1
                    If dtDetalle.Rows(i)(1) = ddlportafoliof.SelectedValue.Trim Then
                        AlertaJS("Ya existe un portafolio con un monto asignado")
                        Exit Sub
                    End If
                Next
                dtDetalle.Rows.Add(codigoprevordeninversion, ddlportafoliof.SelectedValue.Trim, txtMontoAsignadof.Text.Trim, "S")
                dgLista.DataSource = dtDetalle
                dgLista.DataBind()
                Session("dtDetalleInversiones" & codigoprevordeninversion) = dtDetalle
            End If
            If e.CommandName = "Delete" Then
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim preordeninversion As New PrevOrdenInversionBM
                Dim lblportafolio As Label
                Dim txtMontoAsignado As TextBox
                dtDetalle = instanciarTabla(dtDetalle)
                For i = 0 To dgLista.Rows.Count - 1
                    lblportafolio = CType(dgLista.Rows(i).FindControl("lblportafolio"), Label)
                    txtMontoAsignado = CType(dgLista.Rows(i).FindControl("txtMontoAsignado"), TextBox)
                    dtDetalle.Rows.Add(codigoprevordeninversion, lblportafolio.Text.Trim, txtMontoAsignado.Text.Trim, "S")
                Next
                dtDetalle.Rows(gvr.RowIndex).Delete()
                If dtDetalle.Rows.Count = 0 Then
                    llenarFilaVacia(dtDetalle)
                    dgLista.DataSource = dtDetalle
                    dgLista.DataBind()
                    dgLista.Rows(0).Visible = False
                Else
                    dgLista.DataSource = dtDetalle
                    dgLista.DataBind()
                End If
                Session("dtDetalleInversiones" & codigoprevordeninversion) = dtDetalle
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la grilla")
        End Try
    End Sub
    Private Function verificaExistencia(codigopreorden As Integer, codigoportafolio As String) As Boolean
        Dim dtPreOrdenInversion As DataTable = New PrevOrdenInversionBM().SeleccionarDetallePreOrdenInversion(codigopreorden, codigoportafolio)
        If dtPreOrdenInversion.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
                Dim ddlportafolio As DropDownList
                Dim lblportafolio As Label
                Dim txtMontoAsignacion As TextBox
                lblportafolio = CType(e.Row.FindControl("lblportafolio"), Label)
                ddlportafolio = CType(e.Row.FindControl("ddlPortafolio"), DropDownList)
                HelpCombo.LlenarComboBox(ddlportafolio, CType(Session("portafolio"), DataTable), "CodigoPortafolio", "Descripcion", False)
                ddlportafolio.SelectedValue = lblportafolio.Text.Trim
                txtMontoAsignacion = CType(e.Row.FindControl("txtMontoAsignado"), TextBox)
            End If
            If e.Row.RowType = ListItemType.Footer Then
                Dim ddlportafoliof As New DropDownList
                Dim txtMontoAsiganacion As TextBox
                ddlportafoliof = CType(e.Row.FindControl("ddlPortafolioF"), DropDownList)
                HelpCombo.LlenarComboBox(ddlportafoliof, CType(Session("portafolio"), DataTable), "CodigoPortafolio", "Descripcion", False)
                txtMontoAsiganacion = CType(e.Row.FindControl("txtMontoAsignadoF"), TextBox)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub
    Protected Sub dgLista_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles dgLista.RowDeleting
    End Sub
End Class

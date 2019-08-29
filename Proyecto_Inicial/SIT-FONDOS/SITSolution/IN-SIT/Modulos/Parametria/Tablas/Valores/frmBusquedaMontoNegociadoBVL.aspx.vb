
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaMontoNegociadoBVL
    Inherits BasePage

    Private FechaOperacion As Decimal
    Private NumeroOperacion As Decimal
    Private CodigoMnemonico As String

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Me.CargarCombos()
                Me.CargarGrilla()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoMnemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error la realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmMontoNegociadoBVL.aspx?ope=reg")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Session("dtMontoNegociadoBVL") = Nothing
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?');")
                If (e.Row.Cells(5).Text = "A") Then
                    e.Row.Cells(5).Text = "Activo"
                ElseIf (e.Row.Cells(5).Text = "I") Then
                    e.Row.Cells(5).Text = "Inactivo"
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If (e.CommandName = "Eliminar" Or e.CommandName = "Modificar") Then
                Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim Index As Integer = Row.RowIndex
                NumeroOperacion = IIf(Row.Cells(3).Text = "&nbsp;", 0, CType(Row.Cells(3).Text, Decimal))
                FechaOperacion = DirectCast(Row.FindControl("hdFechaOperacion"), HiddenField).Value
            End If

            Select Case e.CommandName
                Case "Eliminar"
                    Try
                        Dim oMontoNegociadoBVLBM As New MontoNegociadoBVLBM
                        Dim codigo As String = e.CommandArgument.ToString()
                        oMontoNegociadoBVLBM.Eliminar(Me.FechaOperacion, Me.NumeroOperacion, Me.DatosRequest)
                        CargarGrilla()
                    Catch ex As Exception
                        AlertaJS(ex.ToString)
                    End Try
                Case "Modificar"
                    Response.Redirect("frmMontoNegociadoBVL.aspx?ope=mod&fecOP=" + Me.FechaOperacion.ToString + "&numOP=" + Me.NumeroOperacion.ToString)
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            Me.dgLista.DataSource = CType(Session("dtMontoNegociadoBVL"), DataTable)
            Me.dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub Buscar()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oMontoNegociadoBVLBM As New MontoNegociadoBVLBM
        Dim dt As DataTable
        Dim Tipo As String
        Dim situacion As String

        situacion = IIf(Me.ddlSituacion.SelectedIndex = 0, "", Me.ddlSituacion.SelectedValue)
        Me.FechaOperacion = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVigencia.Text)
        If Me.tbNumeroOperacion.Text.Length = 0 Then
            Me.NumeroOperacion = 0
        Else
            Me.NumeroOperacion = CType(Me.tbNumeroOperacion.Text, Decimal)
        End If
        Me.CodigoMnemonico = Me.tbCodigoMnemonico.Text
        dt = New MontoNegociadoBVLBM().SeleccionarPorFiltro(Me.FechaOperacion, Me.NumeroOperacion, Me.tbCodigoMnemonico.Text, situacion, DatosRequest).Tables(0)
        Session("dtMontoNegociadoBVL") = dt
        Me.dgLista.DataSource = dt
        Me.dgLista.DataBind()    
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub LimpiarConsulta()
        Me.tbFechaVigencia.Text = ""
        Me.tbNumeroOperacion.Text = ""
        Me.tbCodigoMnemonico.Text = ""
        Me.ddlSituacion.SelectedIndex = 0
    End Sub

#End Region

End Class

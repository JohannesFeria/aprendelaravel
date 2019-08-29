Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmLiborFecha
    Inherits BasePage

    Private Const INDEXGRILLA As String = "Index"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                txtFechaInicio.Text = DateTime.Today
                txtFechaTermino.Text = DateTime.Today
                CargaGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If (Convert.ToString(ViewState(INDEXGRILLA)) <> "") Then
                If Me.txtFechaLibor.Text.Trim.Length > 0 Then
                    Dim CodigoNemonico As String
                    Dim Secuencia As String
                    Dim FechaInicio As Decimal
                    CodigoNemonico = dgLista.Rows(CInt(ViewState(INDEXGRILLA))).Cells(1).Text
                    Secuencia = dgLista.Rows(CInt(ViewState(INDEXGRILLA))).Cells(2).Text
                    FechaInicio = UIUtility.ConvertirFechaaDecimal(dgLista.Rows(CInt(ViewState(INDEXGRILLA))).Cells(3).Text)
                    Dim oIndicadorBM As New IndicadorBM
                    oIndicadorBM.ModificarCuponesLibor(CodigoNemonico, Secuencia, FechaInicio, UIUtility.ConvertirFechaaDecimal(txtFechaLibor.Text))
                    CargaGrilla()
                    Me.txtFechaLibor.Text = ""
                Else
                    AlertaJS("-Fecha Libor")
                End If
            Else
                AlertaJS("Debe de Seleccionar un Cupon")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al grabar los datos")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try        
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargaGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try        
    End Sub

    Private Sub CargaGrilla()
        Dim oIndicadorBM As New IndicadorBM
        Dim dtblDatos As DataTable = oIndicadorBM.SeleccionarCuponesLibor(txtCodigoNemonico.Text, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(txtFechaTermino.Text))
        dgLista.DataSource = dtblDatos
        dgLista.DataBind()    
        EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")
    End Sub

    Protected Sub dgLisa_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(3).Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(e.Row.Cells(3).Text)) 'F. Inicio
                e.Row.Cells(4).Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(e.Row.Cells(4).Text)) 'F Termino
                If e.Row.Cells(5).Text <> "&nbsp;" Then
                    e.Row.Cells(5).Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(e.Row.Cells(5).Text)) 'F Termino
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName = "Select" Then
                Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim Index As Integer = Row.RowIndex
                ViewState(INDEXGRILLA) = Row.RowIndex
                dgLista.SelectedIndex = Row.RowIndex
                dgLista.SelectedIndex = -1
                Dim i As Integer = Row.RowIndex
                If Row.Cells(5).Text <> "" And Row.Cells(5).Text <> "&nbsp;" Then
                    EjecutarJS("$('#" + txtFechaLibor.ClientID + "').val('" + Row.Cells(5).Text + "')")
                Else
                    EjecutarJS("$('#" + txtFechaLibor.ClientID + "').val('')")
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargaGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

End Class

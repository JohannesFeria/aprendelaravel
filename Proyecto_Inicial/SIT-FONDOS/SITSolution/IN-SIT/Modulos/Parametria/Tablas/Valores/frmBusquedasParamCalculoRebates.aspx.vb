Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedasParamCalculoRebates
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarGrilla()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbNemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmParamCalculoRebates.aspx?CodigoNemonico=" + "0")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try        
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim CodigoNemonico As String = ""
            If e.CommandName = "Modificar" Then
                Dim strDatos() As String = e.CommandArgument.ToString.Split(",")
                CodigoNemonico = strDatos(0)
                Response.Redirect("frmParamCalculoRebates.aspx?CodigoNemonico=" + CodigoNemonico)
            End If
            If e.CommandName = "Eliminar" Then
                Dim strDatos() As String = e.CommandArgument.ToString.Split(",")
                CodigoNemonico = strDatos(0)
                Dim oRebatesBM As New DividendosRebatesLiberadasBM
                Dim Usuario As String = "mayarzat"
                Dim dataRequest As DataSet = Nothing

                oRebatesBM.EliminarRebateCabecera(CodigoNemonico, dataRequest)
                CargarGrilla()
                Me.AlertaJS("Se Elimino el registro correctamente")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
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

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibnEliminar"), ImageButton).Attributes.Add("OnClick", "return confirm('¿Desea eliminar registro?')")
                If e.Row.Cells(3).Text.Trim = "A" Then
                    e.Row.Cells(3).Text = "Activo"
                Else
                    e.Row.Cells(3).Text = "Inactivo"
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try        
    End Sub

    Private Sub CargarGrilla()
        Dim dtTabla As Data.DataTable
        Dim oRebatesBM As New DividendosRebatesLiberadasBM
        dtTabla = oRebatesBM.ObtenerCabeceraCodigoRebateDetalle(Me.tbNemonico.Text)
        dgLista.DataSource = dtTabla
        dgLista.DataBind()
        EjecutarJS("$('#" + lblContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtTabla) + "');")
    End Sub

End Class

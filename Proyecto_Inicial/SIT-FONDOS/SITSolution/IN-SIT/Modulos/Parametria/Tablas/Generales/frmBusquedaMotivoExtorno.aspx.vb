Imports Sit.BusinessLayer
Imports Sit.BusinessEntities

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaMotivoExtorno
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            Me.AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            ViewState("nombre") = tbNombre.Text
            CargarGrilla(ViewState("nombre"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmMotivoExtorno.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub CargarPagina()
        ViewState("nombre") = ""
        CargarGrilla()
    End Sub

    Private Sub CargarGrilla(Optional ByVal nombre As String = "")
        dgLista.DataSource = New ParametrosGeneralesBM().SeleccionarMotivoExtorno(nombre, "", DatosRequest)
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + Constantes.M_STR_TEXTO_RESULTADOS + dgLista.Rows.Count.ToString() + "');")
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName = "Editar" Then
                Response.Redirect("frmMotivoExtorno.aspx?cod=" & e.CommandArgument)
            End If
            If e.CommandName = "Borrar" Then
                Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                oParametrosGeneralesBM.Eliminar(ParametrosSIT.MOTIVO_EXTORNO, e.CommandArgument, DatosRequest)
                CargarGrilla(ViewState("nombre"))
                AlertaJS("Los cambios se han realizado satisfactoriamente! ")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub
End Class

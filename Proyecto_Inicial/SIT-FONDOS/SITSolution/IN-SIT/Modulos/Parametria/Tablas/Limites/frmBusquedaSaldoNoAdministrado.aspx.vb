Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Limites_frmBusquedaSaldoNoAdministrado
    Inherits BasePage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            LlenarComboMandato()
            CargarGrilla()
        End If
    End Sub

    Private Fecha As Decimal
    Private Mandato As String

    Private Sub LlenarComboMandato()
        Dim dtTerceros As DataTable
        Dim oTerceroBM As New TercerosBM
        dtTerceros = oTerceroBM.ListarMandatos().Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlMandato, dtTerceros, "CodigoTercero", "Descripcion", True)

    End Sub

    Private Sub CargarGrilla()
        Dim oSaldoNoAdministradoBM As New SaldoNoAdministradoBM
        Dim dt As DataTable
        Me.Fecha = UIUtility.ConvertirFechaaDecimal(Me.txtFecha.Text)
        Me.Mandato = ddlMandato.SelectedValue


        dt = oSaldoNoAdministradoBM.SeleccionarPorFiltro("", Me.Mandato, Me.Fecha, "", "", "", "").Tables(0)
        Me.dgLista.DataSource = dt
        Me.dgLista.DataBind()

        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
    End Sub

    Protected Sub btnImportar_Click(sender As Object, e As System.EventArgs) Handles btnImportar.Click
        Try
            Response.Redirect("frmSaldoNoAdministradoImportar.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Importar")
        End Try
    End Sub


    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmSaldoNoAdministrado.aspx?cod=" & e.CommandArgument)
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oSaldoNoAdministradoBM As New SaldoNoAdministradoBM
            Dim codigo As String = e.CommandArgument
            oSaldoNoAdministradoBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error la realizar la Búsqueda")
        End Try
    End Sub

    Private Sub Buscar()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Protected Sub btnIngresar_Click(sender As Object, e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmSaldoNoAdministrado.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As System.EventArgs) Handles btnSalir.Click
        Try
            'Session("dtMontoNegociadoBVL") = Nothing
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
        End If
    End Sub
End Class

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Valorizacion_y_Custodia_Valorizacion_frmConsultaValoracion
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPortafolio()
                cargarFecha()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    'Private Sub btnCalculoPromedio_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Response.Redirect("frmCalculoPromedioDiario.aspx", False)
    'End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim strurl As String = "Reportes/frmVisorValorizacion.aspx?ClaseReporte=ResultadosValorizacion&codPortafolio=" & ddlPortafolio.SelectedValue & "&Portafolio=" & ddlPortafolio.SelectedItem.Text & "&Fecha=" & UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString() & "&tipoValorizacion=" & rbtlNormativa.SelectedValue
            EjecutarJS("showWindow('" & strurl & "', '800', '600');")
            'EjecutarJS(UIUtility.MostrarPopUp("Reportes/frmVisorValorizacion.aspx?ClaseReporte=ResultadosValorizacion&codPortafolio=" & ddlPortafolio.SelectedValue & "&Portafolio=" & ddlPortafolio.SelectedItem.Text & "&Fecha=" & UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString() & "&tipoValorizacion=" & rbtlNormativa.SelectedValue, "10", 1500, 900, 0, 0, "No", "No", "Yes", "Yes"), False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Impresión")
        End Try
    End Sub

    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim dsPortafolio = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolio.Items.Clear()
            ddlPortafolio.DataSource = dsPortafolio
            ddlPortafolio.DataValueField = "CodigoPortafolio"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            cargarFecha()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Portafolio")
        End Try
    End Sub

    Private Sub rbtlNormativa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtlNormativa.SelectedIndexChanged
        Try
            cargarFecha()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el tipo de Operación")
        End Try
    End Sub

    Private Sub cargarFecha()
        ' If ddlPortafolio.SelectedIndex <> 0 Then
        Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
        Dim strFecha As String
        strFecha = oCarteraTituloValoracion.ObtenerFechaValoracion(ddlPortafolio.SelectedValue.ToString, rbtlNormativa.SelectedValue.ToString, True)
        If strFecha Is DBNull.Value Or strFecha = "" Then
            Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue.ToString))
        Else
            Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(strFecha))
        End If
        ' End If
    End Sub

    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de página")
        End Try
    End Sub

End Class

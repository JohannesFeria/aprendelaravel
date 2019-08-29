
Partial Class Modulos_Contabilidad_frmOperacionesNoContabilizadas
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            UIUtility.CargarPortafoliosOI(ddlFondo)
            tbFechaProceso.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(ddlFondo.SelectedValue))
        End If
    End Sub

    Private Sub btnTerminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTerminar.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub btnVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVista.Click
        Try
            Dim fondo As String
            Dim egreso As String
            fondo = ddlFondo.SelectedValue()
            Session("ReporteContabilidad_Fondo") = ddlFondo.SelectedValue
            If fondo = "" Then
                AlertaJS("Debe seleccionar el campo Egreso")
            Else
                EjecutarJS(UIUtility.MostrarPopUp("frmVisorOperacionesNoContabilizadas.aspx?vfechaproceso=" + tbFechaProceso.Text.Trim() + "&vfondo=" + fondo + "&vdescripcionFondo=" + ddlFondo.SelectedItem.Text.Trim + "&vegreso=" + egreso, "no", 800, 600, 40, 150, "no", "yes", "yes", "yes"), False)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al enviar los datos a imprimir")
        End Try
        
    End Sub

    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        tbFechaProceso.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(ddlFondo.SelectedValue))
    End Sub

End Class

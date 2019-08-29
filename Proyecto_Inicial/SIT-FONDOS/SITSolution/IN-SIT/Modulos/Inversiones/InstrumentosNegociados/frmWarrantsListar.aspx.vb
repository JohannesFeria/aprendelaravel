Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmWarrantsListar
    Inherits BasePage
    Dim oPortafolio As New PortafolioBM
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Sub CargaGrilla()
        dgLista.DataSource = oOrdenInversionBM.ConsultaOrden(ddlPortafolio.SelectedValue, ConvertirFechaaDecimal(txtFechaOperacion.Text), "", "WA")
        dgLista.DataBind()
    End Sub
    Sub FechaPortafolio()
        If ddlPortafolio.SelectedValue = "" Then
            txtFechaOperacion.Text = UIUtility.ConvertirDecimalAStringFormatoFecha(UIUtility.ObtenerFechaMaximaNegocio())
        Else
            txtFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        End If
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolio.PortafolioCodigoListar(""), "CodigoPortafolio", "Descripcion", True)
            FechaPortafolio()
            CargaGrilla()
        End If
    End Sub
    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargaGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            Dim estado As String = ""
            If e.CommandName = "Modificar" Then
                If gvr.Cells(11).Text <> UIUtility.ObtenerFechaNegocio(gvr.Cells(4).Text) Then
                    Response.Redirect("frmWarrants.aspx?cod=" & e.CommandArgument.ToString() & "&estado=E-ELI")
                Else
                    Response.Redirect("frmWarrants.aspx?cod=" & e.CommandArgument.ToString() & "&estado=" & gvr.Cells(2).Text)
                End If
            Else
                If gvr.Cells(10).Text <> UIUtility.ObtenerFechaNegocio(gvr.Cells(4).Text) Then
                    AlertaJS("La fecha de negocio del portafolio es distinta a la de la orden.")
                ElseIf gvr.Cells(2).Text = "E-CON" Then
                    AlertaJS("No se puede anular ordenes confirmadas.")
                ElseIf gvr.Cells(2).Text = "E-ELI" Then
                    AlertaJS("Esta orden ya ha sido anulada.")
                Else
                    oOrdenInversionBM.AnulaOrdenInversion(e.CommandArgument, gvr.Cells(4).Text, DatosRequest)
                    CargaGrilla()
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnIngresar_Click(sender As Object, e As System.EventArgs) Handles btnIngresar.Click
        Response.Redirect("frmWarrants.aspx?cod=")
    End Sub
End Class
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Inversiones_InstrumentosNegociados_frmConsultaCuponeras
    Inherits BasePage

#Region "Metodos pagina"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Not Page.IsPostBack Then

            Dim strCodigoMnemonico As String = Request.QueryString("CodigoMnemonico")
            Dim strGuid As String = Request.QueryString("Guid")
            Dim strCodigoOrden As String = Request.QueryString("CodigoOrden")
            Dim strCodigoPortafolioSBS As String = Request.QueryString("CodigoPortafolioSBS")
            Dim strInteresCorrido As String = Request.QueryString("InteresCorrido")
            Dim strMontoOperacion As String = Request.QueryString("MontoOperacion")
            Dim strPrecioCalculado As String = Request.QueryString("PrecioCalculado")
            Dim strFechaOperacion As String = Request.QueryString("FechaOperacion")

            ViewState("CodigoMnemonico") = strCodigoMnemonico
            ViewState("GUID") = strGuid
            ViewState("CodigoOrden") = strCodigoOrden
            ViewState("codigoPortafolioSBS") = strCodigoPortafolioSBS

            ViewState("InteresCorrido") = strInteresCorrido
            ViewState("MontoOperacion") = strMontoOperacion
            ViewState("PrecioCalculado") = strPrecioCalculado
            ViewState("FechaOperacion") = strFechaOperacion

            CargarCuponera(strCodigoMnemonico, strGuid, strCodigoOrden, strCodigoPortafolioSBS, strFechaOperacion)

            lblInteresCorrido.Text = Format(Convert.ToDecimal(strInteresCorrido), "##,##0.0000000")
            lblMontoOperación.Text = Format(Convert.ToDecimal(strMontoOperacion), "##,##0.0000000")
            lblPrecioCalculado.Text = Format(Convert.ToDecimal(strPrecioCalculado), "##,##0.0000000")

        End If

    End Sub

    Private Sub btnRetornar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Retornar()
    End Sub

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Imprimir()
    End Sub

#End Region

#Region "Metodos privados"

    Private Sub CargarCuponera(ByVal strCodigoMnemonico As String, ByVal Guid As String, ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal fechaOperacion As String)

        Dim oCuponeraBM As New CuponeraBM
        Dim dtCuponera As DataTable = oCuponeraBM.SeleccionarPorOrdenInversionVPN(strCodigoMnemonico, Guid, codigoOrden, codigoPortafolioSBS, fechaOperacion, DatosRequest)

        dgLista.DataSource = dtCuponera
        dgLista.DataBind()
        Session("dtCuponera") = dtCuponera

    End Sub

    Private Sub Imprimir()

        Dim strCodigoMnemonico As String = Request.QueryString("CodigoMnemonico")
        Dim strGuid As String = Request.QueryString("Guid")
        Dim strCodigoOrden As String = Request.QueryString("CodigoOrden")
        Dim strCodigoPortafolioSBS As String = Request.QueryString("CodigoPortafolioSBS")
        Dim strFechaOperacion As String = Request.QueryString("FechaOperacion")
        Dim strInteresCorrido As String = Request.QueryString("InteresCorrido")
        Dim strMontoOperacion As String = Request.QueryString("MontoOperacion")
        Dim strPrecioCalculado As String = Request.QueryString("PrecioCalculado")

        'Page.RegisterStartupScript(Guid.NewGuid().ToString(), UIUtility.MostrarPopUp("../Reportes/Cuponera/VisorConsultaCuponeras.aspx?vCodigoMnemonico=" + strCodigoMnemonico + "&vGuid=" + strGuid + "&vCodigoOrden=" + strCodigoOrden + "&vCodigoPortafolioSBS=" + strCodigoPortafolioSBS + "&vFechaOperacion=" + strFechaOperacion + "&vInteresCorrido=" + strInteresCorrido + "&vMontoOperacion=" + strMontoOperacion + "&vPrecioCalculado=" + strPrecioCalculado, "10", 1100, 650, 0, 0, "No", "Yes", "Yes", "Yes"))
        EjecutarJS(UIUtility.MostrarPopUp("../Reportes/Cuponera/frmVisorConsultaCuponeras.aspx?vCodigoMnemonico=" + strCodigoMnemonico + "&vGuid=" + strGuid + "&vCodigoOrden=" + strCodigoOrden + "&vCodigoPortafolioSBS=" + strCodigoPortafolioSBS + "&vFechaOperacion=" + strFechaOperacion + "&vInteresCorrido=" + strInteresCorrido + "&vMontoOperacion=" + strMontoOperacion + "&vPrecioCalculado=" + strPrecioCalculado, "10", 1100, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
        'Page.RegisterStartupScript("JScript", "<script language=javascript>window.print();</script>")

    End Sub

    Private Sub Retornar()

        EjecutarJS("window.close();")
        'Page.RegisterStartupScript("JScript", "<script language=javascript>window.close();</script>")

    End Sub

#End Region

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            dgLista.DataSource = Session("dtCuponera")
            dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el paginado")
        End Try
    End Sub
End Class

Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Inversiones_frmValidacionExcesosPrevOI
    Inherits BasePage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarPagina()
        End If
    End Sub
    Private Sub Imprimir()
        EjecutarJS("window.print();")
    End Sub
    Private Sub Retornar()
        EjecutarJS("window.close();")
    End Sub
    Private Sub CargarPagina()
        If Request.QueryString("Tipo") = "PREVOI" Then
            Dim dt As New DataTable
            dt = CType(Session("dtListaExcesos"), DataTable)
            dgLista.DataSource = dt
            dgLista.DataBind()
            lblTipo.Text = "Registro Previo de Ordenes de Inversión"
        Else
            If Request.QueryString("Tipo") = "OI" Then
                Dim dt As New DataTable
                dt = CType(Session("dtOrdenInversion"), DataTable)
                dgLista.DataSource = dt
                dgLista.DataBind()
                lblTipo.Text = "Ordenes de Inversión"
            End If
        End If
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Call Imprimir()
    End Sub
    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Call Retornar()
    End Sub
    Protected Sub CalculaLimites(ByVal strCodigoOperacion As String, ByVal strCodigoNemonico As String, ByVal decCantidadOperacion As Decimal,
    ByVal strPortafolio As String, ByVal strInstrumento As String)
        Dim oLimiteEvaluacion As New LimiteEvaluacionBM
        Dim dsAux As New DataSet
        Dim codigoOperacion As String = strCodigoOperacion
        Dim codigoNemonico As String = strCodigoNemonico
        Dim cantidadOperacion As Decimal = decCantidadOperacion
        Dim portafolio As String = strPortafolio
        Dim Guid As String = System.Guid.NewGuid.ToString()
        dsAux = oLimiteEvaluacion.ListarExcesosLimitesOnLine(codigoNemonico, cantidadOperacion, portafolio, codigoOperacion, DatosRequest)
        If Not (dsAux Is Nothing) Then
            If (dsAux.Tables.Count > 0) Then
                If (dsAux.Tables(0).Rows.Count > 0) Then
                    Session(Guid) = dsAux
                    Session("Instrumento") = strInstrumento
                    EjecutarJS(UIUtility.MostrarPopUp("InstrumentosNegociados/frmConsultaLimitesInstrumento.aspx?GUID=" + Guid, "LimitesExcedidos",
                    1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                End If
            End If
        End If
    End Sub
    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            If Request.QueryString("Tipo") = "OI" Then
                Dim lbHeader1 As Label
                lbHeader1 = CType(e.Row.FindControl("lbHeader1"), Label)
                lbHeader1.Visible = True
            End If
            If Request.QueryString("TipoRenta") <> ParametrosSIT.TR_RENTA_VARIABLE.ToString Then
                Dim lbHeader5 As Label
                lbHeader5 = CType(e.Row.FindControl("lbHeader5"), Label)
                lbHeader5.Visible = True
            End If
            'OT 10090 - 31/07/2017 - Carlos Espejo
            'Descripcion: Se agrega tipo renta FX
            If Request.QueryString("TipoRenta") = ParametrosSIT.TR_DERIVADOS.ToString Or Request.QueryString("TipoRenta") = "FX" Then
                Dim lbHeader4 As Label
                lbHeader4 = CType(e.Row.FindControl("lbHeader4"), Label)
                lbHeader4.Visible = True
            Else
                Dim lbHeader2 As Label
                Dim lbHeader3 As Label
                lbHeader2 = CType(e.Row.FindControl("lbHeader2"), Label)
                lbHeader3 = CType(e.Row.FindControl("lbHeader3"), Label)
                lbHeader2.Visible = True
                lbHeader3.Visible = True
            End If
            'OT 10090 Fin
        End If
    End Sub
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en al Paginación")
        End Try
    End Sub
End Class

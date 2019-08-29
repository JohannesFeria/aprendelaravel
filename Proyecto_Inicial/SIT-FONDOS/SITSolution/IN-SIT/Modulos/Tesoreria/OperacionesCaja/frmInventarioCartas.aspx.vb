Imports System.IO
Imports System.Text
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports System.Data

Partial Class Modulos_Tesoreria_OperacionesCaja_frmInventarioCartas
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarPagina()
        End If
    End Sub

    Private Sub CargarPagina()
        tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio("MULTIFONDO"))
        tbFechaFin.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio("MULTIFONDO"))
        CargarGrilla()
    End Sub

    Private Sub CargarGrilla()
        dgStockInicial.DataSource = New InventarioCartasBM().Seleccionar(DatosRequest)
        dgStockInicial.DataBind()
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim fechaInicio As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
            Dim fechaFin As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
            EjecutarJS(UIUtility.MostrarPopUp("../Reportes/frmVisorReporte.aspx?tipo=IC&fechaInicio=" & fechaInicio & "&fechaFin=" & fechaFin, "no", 1100, 750, 40, 150, "no", "yes", "yes", "yes"), False)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            Try
                dtDestino.LoadDataRow(dr.ItemArray, False)
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        Next
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Protected Sub dgStockInicial_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgStockInicial.PageIndexChanging
        dgStockInicial.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub

    Protected Sub dgStockInicial_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgStockInicial.RowCommand
        If e.CommandName = "_Add" Then
            Dim oInventarioCartasBE As New InventarioCartasBE
            Dim oRow As InventarioCartasBE.InventarioCartasRow
            Dim tbFechaF As System.Web.UI.WebControls.TextBox
            Dim tbRangoInicialF As System.Web.UI.WebControls.TextBox
            Dim tbRangoFinalF As System.Web.UI.WebControls.TextBox
            Dim bolValida As Boolean = False

            'If e.Item.ItemType = ListItemType.Footer Then
            tbFechaF = CType(dgStockInicial.FooterRow.FindControl("tbFechaF"), System.Web.UI.WebControls.TextBox)
            tbRangoInicialF = CType(dgStockInicial.FooterRow.FindControl("tbRangoInicialF"), System.Web.UI.WebControls.TextBox)
            tbRangoFinalF = CType(dgStockInicial.FooterRow.FindControl("tbRangoFinalF"), System.Web.UI.WebControls.TextBox)

            If tbFechaF.Text <> "" And _
                tbRangoInicialF.Text <> "" And _
                tbRangoFinalF.Text <> "" Then
                If IsDate(tbFechaF.Text) And _
                    IsNumeric(tbRangoInicialF.Text) And _
                    IsNumeric(tbRangoFinalF.Text) Then
                    If Val(tbRangoInicialF.Text) < Val(tbRangoFinalF.Text) Then
                        bolValida = True
                    End If
                End If
            End If

            If bolValida Then
                Dim oInventarioCartas As New InventarioCartasBM
                oRow = CType(oInventarioCartasBE.InventarioCartas.NewRow(), InventarioCartasBE.InventarioCartasRow)
                oInventarioCartas.InicializarInventarioCartas(oRow, DatosRequest)
                oRow.Fecha = UIUtility.ConvertirFechaaDecimal(tbFechaF.Text)
                oRow.RangoInicial = CType(tbRangoInicialF.Text, Decimal)
                oRow.RangoFinal = CType(tbRangoFinalF.Text, Decimal)
                oInventarioCartasBE.InventarioCartas.Rows.Add(oRow)
                oInventarioCartasBE.AcceptChanges()
                oInventarioCartas.Insertar(oInventarioCartasBE, DatosRequest)
                CargarGrilla()
            Else
                AlertaJS("Ingrese correctamente el registro!")
            End If
            'End If
        End If
        If e.CommandName = "_Delete" Then
            Dim oInventarioCartas As New InventarioCartasBM
            oInventarioCartas.Eliminar(CType(e.CommandArgument, Decimal), DatosRequest)
            CargarGrilla()
        End If

    End Sub
End Class

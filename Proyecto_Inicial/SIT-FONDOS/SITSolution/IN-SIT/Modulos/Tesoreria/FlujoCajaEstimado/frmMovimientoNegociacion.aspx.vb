Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Tesoreria_FlujoCajaEstimado_frmMovimientoNegociacion
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        If Not Page.IsPostBack Then
            CargarPortafolio(True)
            CargarMercado(True)
            tbFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
            ViewState("DESPLEGADO") = "NO"
        End If
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarGrilla()
        Dim ds1 As New DataSet
        Dim oSaldoBancario As New SaldosBancariosBM
        ds1 = oSaldoBancario.SeleccionarMovimientosNegociacionOnLine(ddlPortafolio.SelectedValue, ddlMercado.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFecha.Text), "N", DatosRequest)
        dgLista.DataSource = ds1
        dgLista.DataBind()
        Session("ReporteMovimientosNeg") = ds1
        ManipularItemsGrilla(dgLista, "TODO")
    End Sub

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        ManipularItemsGrilla(Me.dgLista, e.CommandArgument.ToString())
    End Sub

    Private Sub ManipularItemsGrilla(ByVal oGrilla As GridView, ByVal clave As String)
        Dim oItem As GridViewRow
        Dim menuPlegado As String = "../../../App_Themes/img/icons/bullet_menu_1.GIF"
        Dim menuDesplegado As String = "../../../App_Themes/img/icons/bullet_menu_2.GIF"
        Dim menuActual As String = String.Empty
        Dim oImagen As New ImageButton
        Dim oItemvisible As Boolean = False

        If clave = "TODO" Then
            For Each oItem In oGrilla.Rows
                Dim categoria As String = oItem.Cells(2).Text.ToString.ToUpper()
                If categoria = "I" Then
                    oItem.Visible = False
                End If
            Next
        End If

        For Each oItem In oGrilla.Rows
            Dim categoria As String
            Dim formula As String
            categoria = oItem.Cells(2).Text.ToString.ToUpper()
            formula = oItem.Cells(1).Text.ToString.ToUpper()

            If formula = clave And categoria = "C" Then

                oImagen = CType(oItem.Cells(0).FindControl("ibMenu"), ImageButton)
                menuActual = oImagen.ImageUrl.ToString()

                If menuActual = menuPlegado Then

                    oImagen.ImageUrl = menuDesplegado
                    oItemvisible = True

                End If

                If menuActual = menuDesplegado Then

                    oImagen.ImageUrl = menuPlegado
                    oItemvisible = False

                End If

                Dim oSubItem As GridViewRow
                Dim comodinBusqueda As String = String.Empty

                For Each oSubItem In oGrilla.Rows
                    Dim formulaSubItem As String = oSubItem.Cells(1).Text.ToString.ToUpper()
                    Dim categoriaSubItem As String = oSubItem.Cells(2).Text.ToString.ToUpper()

                    If formulaSubItem = clave + "-I" And categoriaSubItem = "I" Then
                        oSubItem.Visible = oItemvisible
                    End If
                Next
                Exit For
            End If
        Next

    End Sub

    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)

        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", False)
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub

    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dt As DataTable = oMercado.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO).Tables(0)
            HelpCombo.LlenarComboBox(ddlMercado, dt, "CodigoMercado", "Descripcion", False)
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
        ddlMercado.SelectedValue = "1"
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim ds1 As New DataSet
        Dim oSaldoBancario As New SaldosBancariosBM
        ds1 = oSaldoBancario.SeleccionarMovimientosNegociacionOnLine(ddlPortafolio.SelectedValue, ddlMercado.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFecha.Text), "S", DatosRequest)
        Session("ReporteMovimientosNeg") = ds1

        EjecutarJS(UIUtility.MostrarPopUp("../Reportes/frmReporteNeg.aspx?ClaseReporte=MovNegociacion", "no", 800, 600, 5, 5, "no", "yes", "yes", "yes"), False)
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim clasificacion As String = e.Row.Cells(2).Text.ToString.ToUpper()
            Dim menuPlegado As String = "../../../App_Themes/img/icons/bullet_menu_1.GIF"
            Dim menuDesplegado As String = "../../../App_Themes/img/icons/bullet_menu_2.GIF"
            Dim menuActual As String = String.Empty

            Dim oCheck As System.Web.UI.WebControls.CheckBox
            Dim valor As String

            Dim oImagen As New ImageButton
            oImagen = CType(e.Row.Cells(0).FindControl("ibMenu"), ImageButton)

            If ViewState("DESPLEGADO") = "SI" Then
                menuActual = menuDesplegado
            Else
                menuActual = menuPlegado
            End If

            If clasificacion = "C" Then

                e.Row.BackColor = Drawing.Color.Gainsboro
                e.Row.Font.Bold = True
                oImagen.ImageUrl = menuActual

            End If

            If clasificacion = "I" Then
                oImagen.Visible = False
            End If

            Dim dr As DataRowView = CType(e.Row.DataItem, DataRowView)

            If Not (e.Row.FindControl("ChkLiquidado") Is Nothing) Then
                oCheck = CType(e.Row.FindControl("ChkLiquidado"), System.Web.UI.WebControls.CheckBox)
                valor = IIf(IsDBNull(dr("Liquidado")), "N", dr("Liquidado"))
                oCheck.Checked = IIf(valor = "L", True, False)
                If clasificacion = "C" Then
                    oCheck.Visible = False
                End If
            End If


            If Not (e.Row.FindControl("ChkImpreso") Is Nothing) Then
                oCheck = CType(e.Row.FindControl("ChkImpreso"), System.Web.UI.WebControls.CheckBox)
                valor = IIf(IsDBNull(dr("Impreso")), "N", dr("Impreso"))
                oCheck.Checked = IIf(valor = "I", True, False)
                If clasificacion = "C" Then
                    oCheck.Visible = False
                End If
            End If

            Dim cnt As Integer
            For cnt = 5 To 6
                If IIf(e.Row.Cells(cnt).Text = "&nbsp;", 0, e.Row.Cells(cnt).Text) < 0 Then
                    e.Row.Cells(cnt).ForeColor = Drawing.Color.Red
                End If
            Next
        End If
    End Sub


End Class

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Tesoreria_FlujoCajaEstimado_frmFlujoCajaLocal
    Inherits BasePage

#Region "CargarDatos"
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dsPortafolio As DataSet = oPortafolio.Listar(DatosRequest, ParametrosSIT.ESTADO_ACTIVO)
            ddlPortafolio.Items.Clear()
            ddlPortafolio.DataSource = dsPortafolio
            ddlPortafolio.DataValueField = "CodigoPortafolioSBS"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub

    Private Sub CargarClaseCuenta(Optional ByVal enabled As Boolean = True)
        Dim dtBancos As DataTable
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone("1", Me.ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue)
            dtBancos = dsBanco.Tables(0)

            ddlClaseCuenta.Items.Clear()

            If (ddlBanco.SelectedValue <> "") Then
                dtBancos.DefaultView.RowFilter = "CodigoTerceroRuc='" + ddlBanco.SelectedValue + "'"
                dtBancos.DefaultView.RowStateFilter = DataViewRowState.CurrentRows
            End If

            Dim r As DataRowView
            For Each r In dtBancos.DefaultView
                If ddlClaseCuenta.Items.FindByValue(CType(r("CodigoClaseCuenta"), String)) Is Nothing Then
                    ddlClaseCuenta.Items.Add(New ListItem(CType(r("NombreClaseCuenta"), String), CType(r("CodigoClaseCuenta"), String)))
                End If
            Next

            If ddlClaseCuenta.Items.Count = 1 Then
                UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
                ddlClaseCuenta.SelectedIndex = 1
            Else
                UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
            End If
        Else
            ddlClaseCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
        End If
        ddlClaseCuenta.Enabled = enabled
    End Sub

    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataSet = oMoneda.GetMonedaMercadoPortafolio("1", ddlPortafolio.SelectedValue)
            ddlMoneda.Items.Clear()
            ddlMoneda.DataSource = dsMoneda
            ddlMoneda.DataValueField = "CodigoMoneda"
            ddlMoneda.DataTextField = "Descripcion"
            ddlMoneda.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        End If
        ddlMoneda.Enabled = enabled
    End Sub

    Private Sub CargarBanco(ByVal codigoMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorCodigoMercadoYPortafolio("1", Me.ddlPortafolio.SelectedValue)
            ddlBanco.Items.Clear()
            ddlBanco.DataSource = dsBanco
            ddlBanco.DataValueField = "CodigoTercero"
            ddlBanco.DataTextField = "Descripcion"
            ddlBanco.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
    End Sub
#End Region

#Region "Metodos de Control"
    Private Function ObtenerFechaApertura() As Decimal
        Return Decimal.Parse(DateTime.Now.ToString("yyyyMMdd"))
    End Function
#End Region
#Region "Metodo Personalizados"
    Private Sub EditarCelda(ByVal oDataGridItem As GridViewRow)
        Dim egreso As String
        egreso = oDataGridItem.Cells(5).Text
        If (egreso = "S") Then 'Es un egreso
            oDataGridItem.Cells(4).Text = "0.00"
            oDataGridItem.Cells(5).Text = Decimal.Parse(oDataGridItem.Cells(0).Text).ToString("#,##0.00")
        ElseIf egreso = "N" Then ' Es un ingreso
            oDataGridItem.Cells(4).Text = Decimal.Parse(oDataGridItem.Cells(0).Text).ToString("#,##0.00")
            oDataGridItem.Cells(5).Text = "0.00"
        End If

    End Sub
#End Region
#Region "Eventos de la Pagina"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            'CargarPortafolio(True)
            'UIUtility.SeleccionarDefaultValue(ddlPortafolio, 1)

            Dim dtPortafolio As DataTable = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dtPortafolio, "CodigoPortafolio", "Descripcion", True)

            CargarBanco(String.Empty, True)
            CargarMoneda(True)
            CargarClaseCuenta(True)
            ViewState("DESPLEGADO") = "NO"

            tbFechaVctoIni.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            tbFechaVctoFin.Text = Date.Now.ToString("dd/MM/yyyy")
            Me.rbtDias.Checked = True
        End If
        btnBuscar.Attributes.Add("onclick", "return ValidarFormulario();")
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

    Private Function SeleccionarSaldosBancarios(ByVal dsCuentaEconomica As CuentaEconomicaBE) As DataSet
        Dim oSaldoBancario As New SaldosBancariosBM
        Dim fechaIni As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaVctoIni.Text)
        Dim fechaFin As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaVctoFin.Text)
        Dim Periodo As String
        Periodo = IIf(Me.rbtMeses.Checked = True, "M", "D")
        'RGF 20090119 se agregó parámetro PorDivisas
        Return oSaldoBancario.SeleccionarFlujoEstimadoPorFiltro(dsCuentaEconomica, fechaIni, fechaFin, Periodo, ddlTipoFlujo.SelectedValue, DatosRequest)
    End Function
#End Region

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim dsCuentaEconomica As New CuentaEconomicaBE
        Dim roCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.NewCuentaEconomicaRow
        roCuentaEconomica.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        roCuentaEconomica.CodigoMoneda = ddlMoneda.SelectedValue
        roCuentaEconomica.CodigoClaseCuenta = ddlClaseCuenta.SelectedValue
        roCuentaEconomica.CodigoTercero = ddlBanco.SelectedValue
        roCuentaEconomica.CodigoMercado = ParametrosSIT.MERCADO_LOCAL
        dsCuentaEconomica.CuentaEconomica.AddCuentaEconomicaRow(roCuentaEconomica)

        Session("Rep_CuentaEconomica") = dsCuentaEconomica
        Dim Periodo As String
        Periodo = IIf(Me.rbtMeses.Checked = True, "M", "D")

        EjecutarJS("window.open('../Reportes/frmReporte.aspx?ClaseReporte=ReporteFlujoEstimado&Mercado=Local&FechaInicio=" + tbFechaVctoIni.Text + "&FechaFin=" + tbFechaVctoFin.Text + "&Moneda=" + ddlMoneda.SelectedValue + "&TipoFlujo=" + ddlTipoFlujo.SelectedItem.Text + "&Periodo=" + Periodo + "&PorDivisas=" + ddlTipoFlujo.SelectedValue + "', '', 'width=800, height=600, top=50, left=50, menubar=no, resizable=yes');")
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Dim i As Integer = ddlPortafolio.SelectedIndex
        If i <> 0 Then
            tbFechaVctoIni.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        End If
        CargarMoneda(True)
        CargarBanco(String.Empty, True)
        CargarClaseCuenta(True)
    End Sub

    Private Sub ddlBanco_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBanco.SelectedIndexChanged

        CargarClaseCuenta(True)
    End Sub

    Private Sub ddlMoneda_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged

        CargarClaseCuenta(True)
        CargarBanco(String.Empty, True)
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

        'Pliega o Despliega (Todas las cabeceras)
        If clave = "TODO" Then

            For Each oItem In oGrilla.Rows
                Dim categoria As String = oItem.Cells(2).Text.ToString.ToUpper()
                If categoria = "I" Then
                    oItem.Visible = False
                End If
            Next
        End If
        'Pliega o Despliega (Solo la cabecera clave)
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

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        'dgLista.PageIndex = e.NewPageIndex
        'CargarGrilla()
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim clasificacion As String = e.Row.Cells(2).Text.ToString.ToUpper()
            Dim formula As String = e.Row.Cells(1).Text.ToString.ToUpper()
            Dim menuPlegado As String = "../../../App_Themes/img/icons/bullet_menu_1.GIF"
            Dim menuDesplegado As String = "../../../App_Themes/img/icons/bullet_menu_2.GIF"
            Dim menuActual As String = String.Empty
            Dim oImagen As New ImageButton


            'EditarCelda(e.Item)
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

            If formula = "SALFIN" Then
                e.Row.Cells(4).ForeColor = Drawing.Color.Blue
                e.Row.Cells(7).ForeColor = Drawing.Color.Blue
            End If
        End If
    End Sub

    Sub CargarGrilla()
        If UIUtility.ConvertirFechaaDecimal(tbFechaVctoIni.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaVctoFin.Text) Then
            AlertaJS(ObtenerMensaje("ALERT48"))
            Exit Sub
        End If
        Dim dsCuentaEconomica As New CuentaEconomicaBE
        Dim roCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.NewCuentaEconomicaRow
        roCuentaEconomica.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        roCuentaEconomica.CodigoMoneda = ddlMoneda.SelectedValue
        roCuentaEconomica.CodigoClaseCuenta = ddlClaseCuenta.SelectedValue
        roCuentaEconomica.CodigoTercero = ddlBanco.SelectedValue
        roCuentaEconomica.CodigoMercado = ParametrosSIT.MERCADO_LOCAL

        dsCuentaEconomica.CuentaEconomica.AddCuentaEconomicaRow(roCuentaEconomica)
        Dim ds As DataSet = SeleccionarSaldosBancarios(dsCuentaEconomica)
        If ds.Tables.Count > 0 Then
            dgLista.DataSource = ds.Tables(0)
            dgLista.DataBind()
            ManipularItemsGrilla(dgLista, "TODO")
        Else
            dgLista.DataSource = Nothing
            dgLista.DataBind()
        End If
        Session("ReporteFlujoEstimado") = ds
    End Sub

End Class

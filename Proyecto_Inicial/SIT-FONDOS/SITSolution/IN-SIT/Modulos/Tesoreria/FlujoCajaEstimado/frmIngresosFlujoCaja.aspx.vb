Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Tesoreria_FlujoCajaEstimado_frmIngresosFlujoCaja
    Inherits BasePage
    Private FechaOperacion As Date
#Region "CargarDatos"
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolio.Items.Clear()
            ddlPortafolio.DataSource = dsPortafolio
            ddlPortafolio.DataValueField = "CodigoPortafolio"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dsMercado As DataSet = oMercado.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            ddlMercado.Items.Clear()
            ddlMercado.DataSource = dsMercado
            ddlMercado.DataValueField = "CodigoMercado"
            ddlMercado.DataTextField = "Descripcion"
            ddlMercado.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
    End Sub
    Private Sub CargarBanco(ByVal codigoMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorCodigoMercadoYPortafolio(codigoMercado, Me.ddlPortafolio.SelectedValue)
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
    Private Sub CargarTipoOperacion(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oTipoOperacion As New TipoOperacionBM
            Dim dsTipoOperacion As DataSet = oTipoOperacion.Listar(ParametrosSIT.ESTADO_ACTIVO)
            ddlTipoOperacion.Items.Clear()
            ddlTipoOperacion.DataSource = dsTipoOperacion
            ddlTipoOperacion.DataValueField = "CodigoTipoOperacion"
            ddlTipoOperacion.DataTextField = "Descripcion"
            ddlTipoOperacion.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlTipoOperacion)
        Else
            ddlTipoOperacion.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlTipoOperacion)
        End If
        ddlTipoOperacion.Enabled = enabled
    End Sub
    Private Sub CargarClaseCuenta(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oClaseCuenta As New ClaseCuentaBM
            Dim dsClaseCuenta As ClaseCuentaBE = oClaseCuenta.Listar()
            ddlClaseCuenta.Items.Clear()
            ddlClaseCuenta.DataSource = dsClaseCuenta
            ddlClaseCuenta.DataValueField = "CodigoClaseCuenta"
            ddlClaseCuenta.DataTextField = "Descripcion"
            ddlClaseCuenta.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
        Else
            ddlClaseCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
        End If
        ddlClaseCuenta.Enabled = enabled
    End Sub
    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataSet = oMoneda.Listar(ParametrosSIT.ESTADO_ACTIVO)
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
    Private Sub CargarDetPortafolio(ByVal codTercero As String, ByVal codigoClaseCuenta As String, ByVal codigoPortafolio As String, ByVal codigoMoneda As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oCuentaEconomica As New CuentaEconomicaBM
            Dim dsCuentaEconomica As DataSet = oCuentaEconomica.SeleccionarPorFiltro(codigoPortafolio, codigoClaseCuenta, codTercero, codigoMoneda, "", DatosRequest)
            ddlNroCuenta.Items.Clear()
            ddlNroCuenta.DataSource = dsCuentaEconomica.Tables(1)
            ddlNroCuenta.DataValueField = "NumeroCuenta"
            ddlNroCuenta.DataTextField = "NumeroCuenta"
            ddlNroCuenta.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
        Else
            ddlNroCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
        End If
        ddlNroCuenta.Enabled = enabled
    End Sub
    Private Sub CargarPortafolioi(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolioi.Items.Clear()
            ddlPortafolioi.DataSource = dsPortafolio
            ddlPortafolioi.DataValueField = "CodigoPortafolio"
            ddlPortafolioi.DataTextField = "Descripcion"
            ddlPortafolioi.DataBind()
        Else
            ddlPortafolioi.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolioi)
        End If
        ddlPortafolioi.Enabled = enabled
    End Sub
    Private Sub CargarMercadoi(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dsMercado As DataSet = oMercado.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            ddlMercadoi.Items.Clear()
            ddlMercadoi.DataSource = dsMercado
            ddlMercadoi.DataValueField = "CodigoMercado"
            ddlMercadoi.DataTextField = "Descripcion"
            ddlMercadoi.DataBind()
        Else
            ddlMercadoi.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercadoi)
        End If
        ddlMercadoi.Enabled = enabled
    End Sub
    Private Sub CargarTipoOperacioni(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oTipoOperacion As New TipoOperacionBM
            Dim dsTipoOperacion As DataSet = oTipoOperacion.Listar(ParametrosSIT.ESTADO_ACTIVO)
            ddlTipoOperacioni.Items.Clear()
            ddlTipoOperacioni.DataSource = dsTipoOperacion
            ddlTipoOperacioni.DataValueField = "CodigoTipoOperacion"
            ddlTipoOperacioni.DataTextField = "Descripcion"
            ddlTipoOperacioni.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlTipoOperacioni)
        Else
            ddlTipoOperacioni.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlTipoOperacioni)
        End If
        ddlTipoOperacioni.Enabled = enabled
    End Sub

    Private Sub CargarMonedai(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataSet = oMoneda.Listar(ParametrosSIT.ESTADO_ACTIVO)
            ddlMonedai.Items.Clear()
            ddlMonedai.DataSource = dsMoneda
            ddlMonedai.DataValueField = "CodigoMoneda"
            ddlMonedai.DataTextField = "Descripcion"
            ddlMonedai.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMonedai)
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMonedai)
        End If
        ddlMonedai.Enabled = enabled
    End Sub
    Private Sub CargarOperacion(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oOperacion As New OperacionBM
            Dim dsTipoOperacion As DataSet = oOperacion.SeleccionarPorCodigoTipoOperacion(ddlTipoOperacion.SelectedValue, "", ddlClaseCuenta.SelectedValue)
            ddlOperacion.Items.Clear()
            ddlOperacion.DataSource = dsTipoOperacion
            ddlOperacion.DataValueField = "CodigoOperacion"
            ddlOperacion.DataTextField = "Descripcion"
            ddlOperacion.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlOperacion)
        Else
            ddlOperacion.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlOperacion)
        End If
        ddlOperacion.Enabled = enabled
    End Sub
    Private Sub CargarOperacioni(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oOperacion As New OperacionBM
            Dim dsTipoOperacion As DataSet = oOperacion.SeleccionarPorCodigoTipoOperacion(ddlTipoOperacioni.SelectedValue, "", ddlClaseCuenta.SelectedValue)
            ddlOperacioni.Items.Clear()
            ddlOperacioni.DataSource = dsTipoOperacion
            ddlOperacioni.DataValueField = "CodigoOperacion"
            ddlOperacioni.DataTextField = "Descripcion"
            ddlOperacioni.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlOperacioni)
        Else
            ddlOperacioni.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlOperacioni)
        End If
        ddlOperacioni.Enabled = enabled
        hdCantidadOperaciones.Value = Me.ddlOperacioni.Items.Count
    End Sub
    Private Function listarPortafolios() As String
        Dim i As Integer = 0
        Dim listaPortafolio As String = ""
        Dim dtPortafolio As DataTable = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        For i = 0 To dtPortafolio.Rows.Count - 1
            If listaPortafolio = "" Then
                listaPortafolio = dtPortafolio.Rows(i)("CodigoPortafolio").ToString.Trim
            Else
                listaPortafolio = listaPortafolio & "," & dtPortafolio.Rows(i)("CodigoPortafolio").ToString.Trim
            End If
        Next
        Return listaPortafolio
    End Function
    Private Function listarMercados() As String
        Dim i As Integer = 0
        Dim listaMercado As String = ""
        Dim oMercado As New MercadoBM
        Dim dtMercado As DataTable = oMercado.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO).Tables(0)
        For i = 0 To dtMercado.Rows.Count - 1
            If listaMercado = "" Then
                listaMercado = dtMercado.Rows(i)("CodigoMercado").ToString.Trim
            Else
                listaMercado = listaMercado & "," & dtMercado.Rows(i)("CodigoMercado").ToString.Trim
            End If
        Next
        Return listaMercado
    End Function
#End Region
#Region "Eventos de la Página"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        If Not Page.IsPostBack Then
            CargarPortafolio(True)
            CargarOperacion()
            CargarOperacioni()
            CargarMercado(True)
            CargarBanco(ddlMercado.SelectedValue, True)
            CargarClaseCuenta(True)
            CargarDetPortafolio("", "", ddlPortafolio.SelectedValue, "", True)
            CargarMoneda(True)
            CargarTipoOperacion(True)
            CargarPortafolioi(True)
            CargarOperacioni()
            CargarMercadoi(True)
            CargarMonedai(True)
            CargarTipoOperacioni(True)
        End If
        btnAceptar.Attributes.Add("onclick", "return ValidarDatos();")
        btnEliminar.Attributes.Add("onClick", "javascript:return Confirmar();")  'HDG INC 64511	20120110
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Private Function ObtenerFechaApertura(ByVal codigoPortafolio As String) As Decimal
        Return UIUtility.ConvertirFechaaDecimal(Now.ToString("dd/MM/yyyy"))
    End Function
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        CargarDetPortafolio(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue, True)
        CargarBanco(ddlMercado.SelectedValue, True)
    End Sub
    Private Sub ddlBanco_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBanco.SelectedIndexChanged
        CargarDetPortafolio(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue, True)
    End Sub
    Private Sub ibModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ViewState("Modo") = "Modificar"
        txtDescripcion.ReadOnly = False
        txtImporte.ReadOnly = False
        btnAceptar.Enabled = True
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oSaldos As New SaldosBancariosBM
            Dim dsFlujo As New CuentaEconomicaBE
            Dim drFlujo As CuentaEconomicaBE.CuentaEconomicaRow = dsFlujo.CuentaEconomica.NewCuentaEconomicaRow
            drFlujo.CodigoPortafolioSBS = ddlPortafolioi.SelectedValue
            drFlujo.CodigoMercado = ddlMercadoi.SelectedValue
            drFlujo.CodigoMoneda = ddlMonedai.SelectedValue

            dsFlujo.CuentaEconomica.AddCuentaEconomicaRow(drFlujo)
            oSaldos.InsertarFlujoEstimado(dsFlujo, UIUtility.ConvertirFechaaDecimal(txtFechaInst.Text), ddlOperacioni.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaDesc.Text), Decimal.Parse(txtImporte.Text.Replace(".", UIUtility.DecimalSeparator)), txtDescripcion.Text.Trim.ToString, UIUtility.ConvertirFechaaDecimal(Me.txtFechaDesc.Text), Me.ddlTipoOperacioni.SelectedValue)  'HDG INC 64511	20120110
            CargarGrilla()
            LimpiarFormulario()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub LimpiarFormulario()
        ddlMercadoi.SelectedIndex = 0
        ddlPortafolioi.SelectedIndex = 0
        ddlOperacioni.SelectedIndex = 0
        ddlTipoOperacioni.SelectedIndex = 0
        ddlMonedai.SelectedIndex = 0
        txtFechaDesc.Text = ""
        txtImporte.Text = ""
        txtDescripcion.Text = ""
    End Sub
    Private Sub ddlMoneda_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        CargarDetPortafolio(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue, True)
    End Sub
    Private Sub ddlClaseCuenta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlClaseCuenta.SelectedIndexChanged
        CargarDetPortafolio(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue, True)
    End Sub
    Private Sub ibEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Try
            Dim oSaldos As New SaldosBancariosBM
            For Each dgRow As GridViewRow In dgLista.Rows
                Dim chk As CheckBox = dgRow.Cells(0).Controls(1)
                If chk.Checked Then
                    Dim dsFlujo As New CuentaEconomicaBE
                    Dim drFlujo As CuentaEconomicaBE.CuentaEconomicaRow = dsFlujo.CuentaEconomica.NewCuentaEconomicaRow
                    drFlujo.CodigoPortafolioSBS = dgRow.Cells(1).Text
                    drFlujo.CodigoMercado = dgRow.Cells(2).Text
                    drFlujo.CodigoMoneda = dgRow.Cells(3).Text
                    dsFlujo.CuentaEconomica.AddCuentaEconomicaRow(drFlujo)
                    oSaldos.EliminarFlujoEstimado(dsFlujo, Decimal.Parse(dgRow.Cells(4).Text), dgRow.Cells(5).Text, "M")
                End If
            Next
            LimpiarFormulario()
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub ddlMercado_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        CargarDetPortafolio(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue, True)
        CargarBanco(ddlMercado.SelectedValue, True)
    End Sub
    Private Sub ddlTipoOperacion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoOperacion.SelectedIndexChanged
        CargarOperacion()
    End Sub
    Private Sub ddlTipoOperacioni_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoOperacioni.SelectedIndexChanged
        If Not Page.IsPostBack Then
            CargarOperacioni()
        End If
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub CargarGrilla()
        Dim dsFlujo As New CuentaEconomicaBE
        Dim drFlujo As CuentaEconomicaBE.CuentaEconomicaRow = dsFlujo.CuentaEconomica.NewCuentaEconomicaRow
        If ddlPortafolio.SelectedValue = "" Then
            drFlujo.CodigoPortafolioSBS = listarPortafolios()
        Else
            drFlujo.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        End If
        If ddlMercado.SelectedValue = "" Then
            drFlujo.CodigoMercado = "LOC," & listarMercados()
        Else
            drFlujo.CodigoMercado = ddlMercado.SelectedValue
        End If
        drFlujo.CodigoMoneda = ddlMoneda.SelectedValue
        dsFlujo.CuentaEconomica.AddCuentaEconomicaRow(drFlujo)
        Dim saldos As New SaldosBancariosBM
        Dim FechaInicio As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)   'HDG INC 64511	20120110
        Dim FechaFin As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text) 'HDG INC 64511	20120110

        dgLista.DataSource = saldos.ListarFlujoEstimado(dsFlujo, ddlOperacion.SelectedValue, ddlTipoOperacion.SelectedValue, "M", FechaInicio, FechaFin)    'HDG INC 64511	20120110
        dgLista.DataBind()
    End Sub
#End Region
End Class
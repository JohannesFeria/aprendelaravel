Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
'OT10795 - Se eliminó comentarios innecesarios que se heredó de Horizonte.
'Se corrigieron los try cash que sólo deben de estar en los métodos de eventos
'ipastor 25/09/2017
Partial Class Modulos_Parametria_AdministracionValores_frmGeneracionCuponeraNormal
    Inherits BasePage
    'INICIO | ZOLUXIONES | RCE | RF002 - Se crea variable global para controlar si se utiliza la opción de %Amortización | 24/05/18
    Public bAmortizar As Boolean

    Private oPeriodicidadBM As New PeriodicidadBM
    Private oTipoAmortizacionBM As New TipoAmortizacionBM
    Private oParametrosGenerales As New ParametrosGeneralesBM
    Private oValoresBM As New ValoresBM
    Private oIndicador As New IndicadorBM
    Private oIndicadorBM As New IndicadorBM
    Private hdEstado As HiddenField
    Private oCodigoNemonico As String
    Private oFechaInicio As Decimal
    Private oCodigoiIsan As String
    Private oMontoNominal As Decimal
    Private oCuponeraBM As New CuponeraBM
    Private dsCuponActual As DataSet
    Private dtAnterior As DataTable
    'FIN | ZOLUXIONES | RCE | RF002 - Se crea variable global para controlar si se utiliza la opción de %Amortización | 24/05/18

    Public Property dtCuponeraNormalSesion() As DataTable
        Get
            If Session("cuponeraNormal") Is Nothing Then
                Return New DataTable
            Else
                Return CType(Session("cuponeraNormal"), DataTable)
            End If
        End Get
        Set(ByVal value As DataTable)
            Session("cuponeraNormal") = value
        End Set
    End Property

    Public Property CuponeraValoresSesion() As CuponeraNormalValoresBE

        Get
            If Session("sCuponeraNormalValoresSesion") Is Nothing Then
                Return New CuponeraNormalValoresBE
            Else
                Return CType(Session("sCuponeraNormalValoresSesion"), CuponeraNormalValoresBE)
            End If
        End Get
        Set(ByVal value As CuponeraNormalValoresBE)
            Session("sCuponeraNormalValoresSesion") = value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Dim strFlagAmortizacion As String
            Dim decTasaCupon As String
            Dim decBaseCupon As String
            Dim decTotal As Decimal
            Dim periodicidad As String
            Dim numerodias As String

            'Dim i As Integer
            'Dim oDT As DataTable
            If Not (Page.IsPostBack) Then
                'Se agrego faltaba tipo rating
                ViewState("FlagSinCalcular") = "0"
  
                pnlCupon.Visible = False
                btnAgregar.Visible = False
                'INICIO | ZOLUXIONES | RCE | RF002 - Botón recursivo para validar el porcentaje de amortización | 24/05/18
                btnValidar.Attributes.Add("style", "display:none")
                'FIN | ZOLUXIONES | RCE | RF002 - Botón recursivo para validar el porcentaje de amortización | 24/05/18
                Dim strTemp As String = Request.QueryString("vEstado")
                tbCodigoNemonico.Text = Request.QueryString("vMnemo")
                tbCodigoIsin.Text = Request.QueryString("vISIN")

                tbFechaEmision.Text = Request.QueryString("vFechaE")
                tbFechaVencimiento.Text = Request.QueryString("vFechaV")
                tbFechaPrimer.Text = Request.QueryString("vFechaP")
                strFlagAmortizacion = Request.QueryString("vFlag")
                decTasaCupon = Request.QueryString("vTasaC").Replace(".", UIUtility.DecimalSeparator)
                decBaseCupon = Request.QueryString("vBaseC").Replace(".", UIUtility.DecimalSeparator)
                periodicidad = Request.QueryString("vPeriod")
                oCodigoNemonico = Request.QueryString("vMnemo")
                decTotal = Convert.ToDecimal(IIf(Request.QueryString("vTasaSpread") = "", "0", Request.QueryString("vTasaSpread").Replace(".", UIUtility.DecimalSeparator))) + Convert.ToDecimal(IIf(Request.QueryString("vIndicador") = "", "0", Request.QueryString("vIndicador").Replace(".", UIUtility.DecimalSeparator)))
                numerodias = Request.QueryString("vNumeroDias")
                ' CargarCombos()

                Session("cuponNormal_Eliminados") = DirectCast(Me.Lllenar_Eliminado, DataTable)
                Dim ApInst As String = ""
                If Not (Request.QueryString("vApInst") = Nothing) Then
                    ApInst = Request.QueryString("vApInst")
                    Session("cuponeraNormal") = Nothing
                End If
                If Session("accionValor") = "INGRESAR" Then
                    ViewState("FlagSinCalcular") = "1"
                    CargarControlesPanelPeriodico()
                    'If Session("cuponeraNormal") Is Nothing Then
                    '    oDT = oCuponeraBM.CalcularCuponeraNormal(oCodigoNemonico, _
                    '                                             False, _
                    '                                             ddlPeriodicidad.SelectedValue, _
                    '                                             oMontoNominal, _
                    '                                             ddlTipoAmortizacion.SelectedValue, _
                    '                                             UIUtility.ConvertirFechaaDecimal(txtFechaEmision.Text), _
                    '                                             UIUtility.ConvertirFechaaDecimal(txtFechaVencimiento.Text), _
                    '                                             UIUtility.ConvertirFechaaDecimal(txtFechaPrimerCupon.Text), _
                    '                                             decTasaCupon, _
                    '                                             decBaseCupon, _
                    '                                             decTotal, _
                    '                                             numerodias, _
                    '                                             ViewState("FlagSinCalcular"), _
                    '                                             DatosRequest).Tables(0)
                    '    gvCupones.DataSource = oDT
                    '    gvCupones.DataBind()
                    '    Session("cuponeraNormal") = oDT
                    'Else
                    '    gvCupones.DataSource = CType(Session("cuponeraNormal"), DataTable)
                    '    gvCupones.DataBind()
                    'End If
                ElseIf Session("accionValor") = "MODIFICAR" Or ApInst = "SI" Then
                    ViewState("FlagSinCalcular") = "0"
                    CargarControlesPanelPeriodico()
                    'If Session("cuponeraNormal") Is Nothing Then
                    '    oDT = oCuponeraBM.CalcularCuponeraNormal(oCodigoNemonico, _
                    '                                             False, _
                    '                                             ddlPeriodicidad.SelectedValue, _
                    '                                             oMontoNominal, _
                    '                                             ddlTipoAmortizacion.SelectedValue, _
                    '                                             UIUtility.ConvertirFechaaDecimal(txtFechaEmision.Text), _
                    '                                             UIUtility.ConvertirFechaaDecimal(txtFechaVencimiento.Text), _
                    '                                             UIUtility.ConvertirFechaaDecimal(txtFechaPrimerCupon.Text), _
                    '                                             decTasaCupon, _
                    '                                             decBaseCupon, _
                    '                                             decTotal, _
                    '                                             numerodias, _
                    '                                             ViewState("FlagSinCalcular"), _
                    '                                             DatosRequest).Tables(0)
                    'Else
                    'gvCupones.DataSource = CType(Session("cuponeraNormal"), DataTable)
                    'gvCupones.DataBind()
                    'ActualizaCuponeraNormalNuevo()
                    ' End If
                End If
                Dim cuponValores As CuponeraNormalValoresBE = CuponeraValoresSesion
                cuponValores.CambioValores = False

                gvCupones.DataSource = CType(Session("cuponeraNormal"), DataTable)
                gvCupones.DataBind()
                ActualizaCuponeraNormalNuevo()
                habilitarTasaVariable()
                If Request.QueryString("vReadOnly") = "YES" Then
                    bloqueatodo()
                End If
                'ActualizaCuponeraNormal()

                'INICIO | ZOLUXIONES | RCE | RF002 - Valida que si tipo de amortización es "Al Vencimiento" no debe aparecer los nuevos campos "A partir de" y "%Amortización" | 24/05/18
                If strFlagAmortizacion = 1 Then fsAmortizacion.Attributes.Add("style", "display:none")
                'FIN | ZOLUXIONES | RCE | RF002 - Valida que si tipo de amortización es "Al Vencimiento" no debe aparecer los nuevos campos "A partir de" y "%Amortización" | 24/05/18

                Dim dtTemporal As New DataTable
                If Not Session("cuponeraNormal") Is Nothing Then
                    dtTemporal = CType(Session("cuponeraNormal"), DataTable).Copy
                    Session("Temporal") = dtTemporal

                    'INICIO | ZOLUXIONES | RCE | RF002 - Se llama nueva función para cargar combo "A partir de" y "% Amortización" | 23/05/18
                    '   Actualizar_ComboApartirDe()
                    'FIN | ZOLUXIONES | RCE | RF002 - Se llama nueva función para cargar combo "A partir de" y "% Amortización" | 23/05/18
                Else
                    Session("Temporal") = Nothing
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Function Lllenar_Eliminado() As DataTable
        Dim Param_1 As New DataColumn("Param_1")
        Param_1.DataType = GetType(String)
        Dim Param_2 As New DataColumn("Param_2")
        Param_2.DataType = GetType(String)
        Dim Eliminados As New DataTable("Eliminados")
        Eliminados.Columns.Add(Param_1)
        Eliminados.Columns.Add(Param_2)
        Return Eliminados
    End Function
    Private Sub CargarRegistro()
        Dim oDT As DataTable
        Dim oCuponera As New CuponeraBM
        oDT = oCuponera.LeerCuponeraNormal(tbCodigoNemonico.Text, DatosRequest).Tables(0)
        dgLista.DataSource = oDT
        dgLista.DataBind()
        Session("cuponeraNormal") = oDT
    End Sub

    Private Sub CargarCombos(Optional ByVal refresh As Boolean = False)
        '  Dim DtTablaPeriodicidad As DataTable
        'Dim DtTablaTipoAmortizacion As DataTable

        Dim tablaRating As New DataTable

        If ViewState("CuponeraNormalPeriodicidad") Is Nothing Or refresh Then
            ViewState("CuponeraNormalPeriodicidad") = oPeriodicidadBM.Listar(DatosRequest).Tables(0)
        End If

        If ViewState("CuponeraNormalTipoAmortizacion") Is Nothing Or refresh Then
            ViewState("CuponeraNormalTipoAmortizacion") = oTipoAmortizacionBM.Listar(DatosRequest).Tables(0)
        End If

        'DtTablaPeriodicidad = oPeriodicidadBM.Listar(DatosRequest).Tables(0)
        'DtTablaTipoAmortizacion = oTipoAmortizacionBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlPeriodicidad, ViewState("CuponeraNormalPeriodicidad"), "CodigoPeriodicidad", "Descripcion", False)
        HelpCombo.LlenarComboBox(ddlTipoAmortizacion, ViewState("CuponeraNormalTipoAmortizacion"), "CodigoTipoAmortizacion", "Descripcion", False)
        HelpCombo.LlenarComboBox(ddlTipoActCuponera, oParametrosGenerales.Listar("ParTipCup", DatosRequest), "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlBaseDias, oParametrosGenerales.Listar("CupBaseMes", DatosRequest), "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlBaseMes, oParametrosGenerales.Listar("CupBaseAño", DatosRequest), "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlbaseDiasModificar, oParametrosGenerales.Listar("CupBaseMes", DatosRequest), "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlbaseMesModificar, oParametrosGenerales.Listar("CupBaseAño", DatosRequest), "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlTasaVariable, oIndicadorBM.Indicador_SeleccionarSWAP("SW-"), "CodigoIndicador", "NombreIndicador", True)
    End Sub

    Private Sub CalcularCuponerPeriodica()
        Dim decTasaCupon As String
        Dim decTasaSpread As Decimal = CDec(IIf(txtTasaVariable.Text.Trim = String.Empty, "0", txtTasaVariable.Text.Trim))
        Dim mesTipoAmortizacion As Integer = UIUtility.ObtenerCantidadMes(ddlTipoAmortizacion.SelectedItem.ToString)
        Dim mesPeriodicidad As Integer = UIUtility.ObtenerCantidadMes(ddlPeriodicidad.SelectedItem.ToString)

        If mesTipoAmortizacion < mesPeriodicidad And mesPeriodicidad <> 999 Then
            AlertaJS("El Tipo de Amortización no puede ser menor a la Periodicidad.")
        ElseIf (mesTipoAmortizacion Mod mesPeriodicidad <> 0) And mesPeriodicidad <> 999 Then
            AlertaJS("El Tipo de Amortización debe ser múltiplo de la Periodicidad.")
        ElseIf ddlTasaVariable.SelectedValue <> String.Empty And decTasaSpread = 0 Then
            AlertaJS("Debe ingresar la tasa variable para el tipo de tasa de tasa variable seleccionado.")
        Else
            oMontoNominal = Request.QueryString("vMontoNominal")
            oCodigoNemonico = Request.QueryString("vMnemo")
            decTasaCupon = txtTasaCupon.Text.Replace(".", UIUtility.DecimalSeparator)
            Dim cuponValores As CuponeraNormalValoresBE = CuponeraValoresSesion
            cuponValores.CodigoPeriodo = ddlPeriodicidad.SelectedValue
            cuponValores.CodigoTipoAmortizacion = ddlTipoAmortizacion.SelectedValue
            cuponValores.CambioValores = False
            cuponValores.RealizoBusqueda = True
            cuponValores.TasaCupon = decTasaCupon
            cuponValores.baseDias = ddlBaseDias.SelectedValue
            cuponValores.baseAnios = ddlBaseMes.SelectedValue
            CuponeraValoresSesion = cuponValores

            If Session("accionValor") = "INGRESAR" Then
                cuponValores.FechaEmision = UIUtility.ConvertirFechaaDecimal(txtFechaEmision.Text)
                cuponValores.FechaFinProximoCupon = UIUtility.ConvertirFechaaDecimal(txtFechaPrimerCupon.Text)
                cuponValores.FechaVencimiento = UIUtility.ConvertirFechaaDecimal(txtFechaVencimiento.Text)
            End If

            dtCuponeraNormalSesion = oCuponeraBM.CalcularCuponeraNormal(oCodigoNemonico, _
                                                                        False, _
                                                                        ddlPeriodicidad.SelectedValue, _
                                                                        oMontoNominal, _
                                                                        ddlTipoAmortizacion.SelectedValue, _
                                                                        UIUtility.ConvertirFechaaDecimal(txtFechaEmision.Text), _
                                                                        UIUtility.ConvertirFechaaDecimal(txtFechaVencimiento.Text), _
                                                                        cuponValores.FechaFinProximoCupon, _
                                                                        decTasaCupon, _
                                                                        cuponValores.baseAnios, _
                                                                        decTasaSpread, _
                                                                        cuponValores.baseDias, _
                                                                        ViewState("FlagSinCalcular"), _
                                                                        DatosRequest).Tables(0)
            gvCupones.DataSource = dtCuponeraNormalSesion
            gvCupones.DataBind()
            ActualizaCuponeraNormalNuevo()
        End If

    End Sub

    Private Sub CalcularCuponerPeriodicaBuscar(ByVal fechaPrimerCupon As Decimal, ByVal fechaVencimiento As Decimal, ByVal codigoPeriocidad As String, ByVal codigoTipoAmortizacion As String)
        Dim decTasaCupon As String
        Dim decBaseCupon As String
        Dim decTasaSpread As Decimal = 0D
        Dim numerodias As String = Request.QueryString("vNumeroDias")

        oMontoNominal = Request.QueryString("vMontoNominal")
        oCodigoNemonico = Request.QueryString("vMnemo")

        decTasaCupon = Request.QueryString("vTasaC").Replace(".", UIUtility.DecimalSeparator)
        decBaseCupon = Request.QueryString("vBaseC").Replace(".", UIUtility.DecimalSeparator)
        dtCuponeraNormalSesion = oCuponeraBM.CalcularCuponeraNormal(oCodigoNemonico, _
                                                                    False, _
                                                                    codigoPeriocidad, _
                                                                    oMontoNominal, _
                                                                    ddlTipoAmortizacion.SelectedValue, _
                                                                    UIUtility.ConvertirFechaaDecimal(txtFechaEmision.Text), _
                                                                    UIUtility.ConvertirFechaaDecimal(txtFechaVencimiento.Text), _
                                                                    UIUtility.ConvertirFechaaDecimal(txtFechaPrimerCupon.Text), _
                                                                    decTasaCupon, _
                                                                    decBaseCupon, _
                                                                    decTasaSpread, _
                                                                    numerodias, _
                                                                    ViewState("FlagSinCalcular"), _
                                                                    DatosRequest).Tables(0)
        gvCupones.DataSource = dtCuponeraNormalSesion
        gvCupones.DataBind()
    End Sub

    Private Sub CargarCuponActual()
        dsCuponActual = oCuponeraBM.Obtener_CuponActual(Request.QueryString("vMnemo"), DatosRequest)

        If dsCuponActual.Tables.Count > 0 Then
            Dim ExisteCupon As String = dsCuponActual.Tables(0).Rows(0)("ExisteCupon")
            Dim cuponValores As CuponeraNormalValoresBE = CuponeraValoresSesion
            If ExisteCupon = "1" Or ddlPeriodicidad.SelectedValue = "7" Then
                txtFechaEmision.Enabled = False
                txtFechaPrimerCupon.Enabled = False
                txtFechaVencimiento.Enabled = False
                divFechaEmision.Attributes.Add("class", "input-append")
                divFechaPrimerCupon.Attributes.Add("class", "input-append")
                divFechaVencimiento.Attributes.Add("class", "input-append")
                Dim dtCuponActual As DataTable = dsCuponActual.Tables(1)

                If dtCuponActual.Rows.Count > 0 Then
                    cuponValores.FechaFinProximoCupon = dtCuponActual.Rows(0)("FechaTermino")
                    CuponeraValoresSesion = cuponValores
                End If
            Else
                txtFechaEmision.Enabled = True
            End If
        End If
    End Sub

    Private Sub CargarControlesPanelPeriodico()
        CargarCombos()
        Dim oValoresBE As New ValoresBE, oRow As ValoresBE.ValorRow

        txtCodigoIsin.Text = Request.QueryString("vISIN")
        txtCodigoNemonico.Text = Request.QueryString("vMnemo")
        txtMontoNominal.Text = Request.QueryString("vMontoNominal")
        oCodigoNemonico = txtCodigoNemonico.Text
        oMontoNominal = txtMontoNominal.Text

        txtFechaEmision.Text = Request.QueryString("vFechaE")
        txtFechaVencimiento.Text = Request.QueryString("vFechaV")
        txtFechaPrimerCupon.Text = Request.QueryString("vFechaP")

        ddlPeriodicidad.SelectedValue = Request.QueryString("vPeriodicidad")
        ddlTipoAmortizacion.SelectedValue = Request.QueryString("vFlag")
        txtTasaCupon.Text = Request.QueryString("vTasaC")
        ddlBaseDias.SelectedValue = Request.QueryString("vNumeroDias")
        ddlBaseMes.SelectedValue = Request.QueryString("vBaseC")
        oValoresBE = oValoresBM.Seleccionar(txtCodigoNemonico.Text, DatosRequest)
        ViewState("DiasTTasaVariable") = "0"
        If oValoresBE.Tables(0).Rows.Count > 0 Then
            oRow = oValoresBE.Valor(0)
            If oRow.TipoTasaVariable <> String.Empty Then
                ddlTasaVariable.SelectedValue = oRow.TipoTasaVariable
                txtTasaVariable.Enabled = True
                ddlPeriodicidadVariable.Enabled = True
                txtTasaVariable.Text = oRow.TasaVariable
                ddlPeriodicidadVariable.SelectedValue = oRow.DiasTTasaVariable
                ViewState("DiasTTasaVariable") = oRow.DiasTTasaVariable
            End If
        End If
        Dim cuponValores As CuponeraNormalValoresBE = CuponeraValoresSesion
        cuponValores.CambioValores = False

        cuponValores.FechaEmision = UIUtility.ConvertirFechaaDecimal(txtFechaEmision.Text)
        cuponValores.FechaFinProximoCupon = UIUtility.ConvertirFechaaDecimal(txtFechaPrimerCupon.Text)
        cuponValores.FechaVencimiento = UIUtility.ConvertirFechaaDecimal(txtFechaVencimiento.Text)

        cuponValores.CodigoPeriodo = ddlPeriodicidad.SelectedValue
        cuponValores.CodigoTipoAmortizacion = ddlTipoAmortizacion.SelectedValue
        CuponeraValoresSesion = cuponValores


        CargarCuponActual()
        txtCodigoIsin.Enabled = False
        txtCodigoNemonico.Enabled = False
        txtMontoNominal.Enabled = False


        CalcularCuponerPeriodica()

    End Sub
    Private Sub ActualizaCuponeraNormal()
        Dim i As Integer
        For i = 0 To dgLista.Rows.Count - 1
            dgLista.Rows(i).Cells(3).Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dgLista.Rows(i).Cells(3).Text))
            dgLista.Rows(i).Cells(4).Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dgLista.Rows(i).Cells(4).Text))
            dgLista.Rows(i).Cells(5).Text = CType(dgLista.Rows(i).Cells(5).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            dgLista.Rows(i).Cells(7).Text = CType(dgLista.Rows(i).Cells(7).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            dgLista.Rows(i).Cells(8).Text = CType(dgLista.Rows(i).Cells(8).Text, String).Replace(UIUtility.DecimalSeparator, ".")
        Next
    End Sub
    Private Sub ActualizaCuponeraNormalNuevo()
        Dim i As Integer, bFlagCuponBloqueado As Boolean = False

        For i = 0 To gvCupones.Rows.Count - 1

            gvCupones.Rows(i).Cells(1).Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(gvCupones.Rows(i).Cells(1).Text))
            gvCupones.Rows(i).Cells(2).Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(gvCupones.Rows(i).Cells(2).Text))
            '    gvCupones.Rows(i).Cells(4).Text = CType(gvCupones.Rows(i).Cells(4).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            gvCupones.Rows(i).Cells(5).Text = CType(gvCupones.Rows(i).Cells(5).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            gvCupones.Rows(i).Cells(6).Text = CType(gvCupones.Rows(i).Cells(6).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            gvCupones.Rows(i).Cells(7).Text = CType(gvCupones.Rows(i).Cells(7).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            gvCupones.Rows(i).Cells(8).Text = CType(gvCupones.Rows(i).Cells(8).Text, String).Replace(UIUtility.DecimalSeparator, ".")

            '   gvCupones.Rows(i).Cells(9).Text = CType(gvCupones.Rows(i).Cells(9).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            gvCupones.Rows(i).Cells(10).Text = CType(gvCupones.Rows(i).Cells(10).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            gvCupones.Rows(i).Cells(11).Text = CType(gvCupones.Rows(i).Cells(11).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            gvCupones.Rows(i).Cells(12).Text = CType(gvCupones.Rows(i).Cells(12).Text, String).Replace(UIUtility.DecimalSeparator, ".")
            If CType(gvCupones.Rows(i).FindControl("hdEstado"), HiddenField).Value = "1" Then bFlagCuponBloqueado = True
        Next
        bloquearControlesConfigurables(bFlagCuponBloqueado)
        reiniciarColAccion()
    End Sub
    Private Sub bloquearControlesConfigurables(ByVal Flag As Boolean)
        Dim i As Integer
        If Session("accionValor") <> "INGRESAR" Then
            ddlTipoAmortizacion.Enabled = Flag
            ddlPeriodicidad.Enabled = Flag
            txtTasaCupon.Enabled = Flag
            btnCalcularPeriodica.Enabled = Flag
            ddlBaseDias.Enabled = Flag
            ddlBaseMes.Enabled = Flag
            btnAceptar.Enabled = Flag
            If dtCuponeraNormalSesion.Rows.Count > 1 Then
                If dtCuponeraNormalSesion.Rows(dtCuponeraNormalSesion.Rows.Count - 1)("Estado").ToString = "1" And _
                   dtCuponeraNormalSesion.Rows(dtCuponeraNormalSesion.Rows.Count - 2)("Estado").ToString = "0" Then
                    ddlTipoAmortizacion.Enabled = False
                End If
            End If
        End If
    End Sub
    Private Sub bloqueatodo()
        tbCodigoNemonico.ReadOnly = True
        tbCodigoIsin.ReadOnly = True
        tbFechaEmision.ReadOnly = True
        tbFechaVencimiento.ReadOnly = True
        tbFechaPrimer.ReadOnly = True
        'INICIO | ZOLUXIONES | RCE | RF002 - Se bloquea nuevos campos "A partir de", "% Amortización" cuando es llamado de formulario "Administración de Valores" en popup | 23/05/18
        ddlApartirDe.Enabled = False
        tbPorcentajeAmortizacion.Enabled = False
        btnAmortizar.Enabled = False
        'FIN | ZOLUXIONES | RCE | RF002 - Se bloquea nuevos campos "A partir de", "% Amortización" cuando es llamado de formulario "Administración de Valores" en popup | 23/05/18
        btnAceptar.Visible = False
        btnAgregar.Enabled = False
        btnProcesar.Enabled = False
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If Session("accionValor") = "INGRESAR" And Request.QueryString("vFlag").ToString <> 1 Then
                'If CType(Session("UtilizaAmortizacion"), Boolean) = False Then
                '    AlertaJS("Debe Ingresar el Porcentaje de Amortización")
                '    UIUtility.ResaltaCajaTexto(tbPorcentajeAmortizacion, True)
                '    Exit Sub
                'End If
            End If
            Session("cuponeraNormal") = dtCuponeraNormalSesion
            Dim dtAux As DataTable = CType(Session("cuponeraNormal"), DataTable)
            Dim i As Integer
            Dim decContador As Decimal = 0.0
            For i = 0 To dtAux.Rows.Count - 1
                decContador = decContador + Convert.ToDecimal(dtAux.Rows(i)("Amortizac"))
            Next
            'INICIO | ZOLUXIONES | RCE | RF002 - Se agrega validación para amortización menores al 100% del total | 24/05/18
            If Math.Round(decContador, 0) > 100 Then
                AlertaJS(ObtenerMensaje("CONF33"))
                Exit Sub
            ElseIf Math.Round(decContador, 0) < 100 Then
                AlertaJS(ObtenerMensaje("ALERT174"))
                Exit Sub
            End If
            'FIN | ZOLUXIONES | RCE | RF002 - Se agrega validación para amortización menores al 100% del total | 24/05/18
            ActualizarAmortizacionAceptar()
            Dim cuponValores As CuponeraNormalValoresBE = CuponeraValoresSesion
            If cuponValores.RealizoBusqueda = True Then
                cuponValores.CambioValores = True
                cuponValores.tipoTasaVariable = ddlTasaVariable.SelectedValue
                cuponValores.tasaVariable = CDec(IIf(txtTasaVariable.Text.Trim = String.Empty, "0", txtTasaVariable.Text.Trim))
                cuponValores.periodicidadTasaVariable = CDec(IIf(ddlPeriodicidadVariable.SelectedValue.Trim = String.Empty, "0", ddlPeriodicidadVariable.SelectedValue.Trim))
                If ddlPeriodicidad.SelectedValue = "7" Then
                    cuponValores.CodigoPeriodo = ddlPeriodicidad.SelectedValue
                    cuponValores.FechaVencimiento = UIUtility.ConvertirFechaaDecimal(txtFechaVencimiento.Text)
                End If
            Else
                cuponValores.CambioValores = False
            End If
            CuponeraValoresSesion = cuponValores
            EjecutarJS(" window.opener.location.reload(true); window.close();")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Session("cuponeraNormal") = Session("Temporal")
        EjecutarJS("window.close();")
    End Sub
    Private Sub btnAgregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            If Not validarCamposVacios() Then
                AlertaJS("Por favor verificar los campos no se encuentren vacíos para ingresar en el cupón.")
                Exit Sub
            End If
            Dim dtAux As DataTable = CType(Session("cuponeraNormal"), DataTable)
            dtAnterior = CType(Session("cuponeraNormal"), DataTable).Copy
            Dim i As Integer
            Dim bAgregar As Boolean = False
            btnAgregar.Visible = True
            btnAceptar.Visible = False
            If btnAgregar.Text.ToUpper.Trim = "AGREGAR" Then
                Dim nuevaRow As DataRow = dtAux.NewRow()
                dtAux.Rows.InsertAt(nuevaRow, hdConsecutivo.Value - 1)
                dtAux.AcceptChanges()
                dtAux.Rows(hdConsecutivo.Value - 1)("consecutivo") = hdConsecutivo.Value
            End If
            'ACTUALIZACION DE UN REGISTRO DE gvCupones
            'For i = 0 To gvCupones.Rows.Count - 1
            '    If gvCupones.Rows(i).Cells(0).Text = hdConsecutivo.Value Then
            '        gvCupones.Rows(i).Cells(1).Text = tbFechaInicio.Text
            '        gvCupones.Rows(i).Cells(2).Text = tbFechaTermino.Text
            '        gvCupones.Rows(i).Cells(3).Text = tbDifDias.Text
            '        gvCupones.Rows(i).Cells(4).Text = Format(Convert.ToDecimal(tbTasaCupon.Text.Replace(".", UIUtility.DecimalSeparator)), "##0.0000000")
            '        gvCupones.Rows(i).Cells(5).Text = Format(Convert.ToDecimal(tbAmortizacion.Text.Replace(".", UIUtility.DecimalSeparator)), "##0.0000000")
            '        gvCupones.Rows(i).Cells(14).Text = IIf(ddlbaseMesModificar.SelectedValue = "ACT", "366", ddlbaseMesModificar.SelectedValue)
            '        Exit For
            '    End If
            'Next
            'ACTUALIZA UN REGISTRO  DE DTAUX RECUPERADO DE LA SESSION Y LO GUARDA EN LA SESSION
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("consecutivo") = hdConsecutivo.Value And Not bAgregar Then
                    dtAux.Rows(i)("FechaIni") = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
                    dtAux.Rows(i)("FechaFin") = UIUtility.ConvertirFechaaDecimal(tbFechaTermino.Text)
                    dtAux.Rows(i)("Amortizac") = tbAmortizacion.Text.Trim 'tbAmortizacion.Text.Replace(".", UIUtility.DecimalSeparator)
                    dtAux.Rows(i)("DifDias") = tbDifDias.Text.Trim
                    dtAux.Rows(i)("TasaCupon") = tbTasaCupon.Text.Trim 'tbTasaCupon.Text.Replace(".", UIUtility.DecimalSeparator)
                    dtAux.Rows(i)("BaseCupon") = IIf(ddlbaseMesModificar.SelectedValue = "ACT", "366", ddlbaseMesModificar.SelectedValue)
                    If btnAgregar.Text.ToUpper.Trim <> "AGREGAR" Then
                        Exit For
                    Else
                        bAgregar = True
                        dtAux.Rows(i)("DiasPago") = tbDifDias.Text.Trim
                    End If
                ElseIf btnAgregar.Text.ToUpper.Trim = "AGREGAR" And bAgregar Then
                    dtAux.Rows(i)("consecutivo") += 1
                End If
            Next
            Session("cuponeraNormal") = dtAux
            pnlCupon.Visible = False
            'VERIFICA LA VARIACION DE LA FECHA INICIO MEDIANTE UN HIDDEN
            'If hdFechaIni.Value <> tbFechaInicio.Text Then
            '    ActualizarFechasCuponeras("I", tbFechaInicio.Text) 'ACTUALIZA FECHA
            'End If
            'VERIFICA LA VARIACION DE LA FECHA FIN MEDIANTE UN HIDDEN
            '     If hdFechaFin.Value <> tbFechaTermino.Text Then
            ActualizarFechasCuponeras(tbFechaTermino.Text) 'ACTUZALIZA FECHA
            '   End If

            'OTXXXX - Está mal el recalculo de fechas
            'If (ddlTipoActCuponera.SelectedValue = "Todos" Or ddlTipoActCuponera.SelectedValue = "EnAdelante") Then
            '    ActualizarFechasCuponerasDiferencia(ddlTipoActCuponera.SelectedValue)
            'End If

            ActualizarAmortizacionTasaCupon() 'ACTUALIZACION DE AMORTIZACION

            LimpiarCampos()
            btnAgregar.Visible = False
            btnAceptar.Visible = True

            If ddlPeriodicidad.SelectedValue = "7" Then txtFechaVencimiento.Text = UIUtility.ConvertirFechaaString(CDec(dtCuponeraNormalSesion.Rows(dtCuponeraNormalSesion.Rows.Count - 1)("FechaFin")))

        Catch ex As Exception
            btnAgregar.Visible = True
            btnAceptar.Visible = False
            Session("cuponeraNormal") = dtAnterior
            gvCupones.DataSource = dtAnterior
            gvCupones.DataBind()
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            ActualizaCuponeraNormalNuevo() 'AMORTIZACION DE CUPONERA NORMAL(DA FORMATO A GVCUPONES)
        End Try
    End Sub
    Private Sub ActualizarAmortizacion2()
        Dim i As Integer
        Dim dtAux As DataTable = CType(Session("cuponeraNormal"), DataTable)
        Dim amortSuma As Decimal = 0
        Dim amortActual As Decimal = 0
        Dim indiceActual As Integer = -1
        For i = 0 To dtAux.Rows.Count - 1
            If dtAux.Rows(i)("consecutivo") < hdConsecutivo.Value Then
                amortSuma = amortSuma + Convert.ToDecimal(dtAux.Rows(i)("Amortizac"))
            ElseIf dtAux.Rows(i)("consecutivo") = hdConsecutivo.Value Then
                amortActual = Convert.ToDecimal(dtAux.Rows(i)("Amortizac"))
                amortSuma = amortSuma + amortActual
                indiceActual = i
            End If
        Next
        If amortSuma < 100 Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("consecutivo") > hdConsecutivo.Value Then
                    If amortSuma + amortActual <= 100 Then
                        dtAux.Rows(i)("Amortizac") = Format(amortActual, "##0.0000000")
                        amortSuma = amortSuma + amortActual
                    Else
                        dtAux.Rows(i)("Amortizac") = Format(100 - amortSuma, "##0.0000000")
                    End If
                End If
            Next
        ElseIf amortSuma > 100 Then
            dtAux.Rows(indiceActual)("Amortizac") = Format(amortActual - amortSuma + 100, "##0.0000000")
        ElseIf amortSuma = 100 Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("consecutivo") > hdConsecutivo.Value Then
                    dtAux.Rows(i)("Amortizac") = Format(0, "##0.0000000")
                End If
            Next
        End If
        amortSuma = 0
        dtAux = ActualizarAmortizacion_100(dtAux)
        Session("cuponeraNormal") = dtAux
        dgLista.DataSource = dtAux
        dgLista.DataBind()
    End Sub
    Private Sub ActualizarAmortizacionTasaCupon()
        Dim i As Integer
        'RECUPERA DE LA SESSION LA CUPONERA EL EL DATATABLE DTAUX
        Dim dtAux As DataTable = CType(Session("cuponeraNormal"), DataTable)
        Dim amortSuma As Decimal = 0
        Dim amortActual As Decimal = 0
        Dim indiceActual As Integer = -1, ultimo As Integer = 0
        Dim strFlagAmortizacion As String
        Dim amortAcumulada As Decimal = 0

        ultimo = dtAux.Rows.Count - 1

        'REALIZA UN RECORRIDO A LA CUPONERA
        For i = 0 To ultimo
            'REPERA LA AMORTIZACION DE CADA REGISTRO
            amortActual = dtAux.Rows(i)("Amortizac")
            If Integer.Parse(dtAux.Rows(i)("consecutivo")) >= Integer.Parse(hdConsecutivo.Value) Then
                'Si el acumulado de amortizaciones es >100 el resto queda en 0
                If ddlTipoActCuponera.SelectedValue = "enAdelante" Or _
                    (Integer.Parse(dtAux.Rows(i)("consecutivo")) = Integer.Parse(hdConsecutivo.Value) And ddlTipoActCuponera.SelectedValue = "cuponActual") Then
                    If (amortAcumulada + Decimal.Parse(tbAmortizacion.Text) >= 100) Then
                        amortActual = 100 - (amortAcumulada)
                    Else
                        amortActual = Decimal.Parse(tbAmortizacion.Text)
                    End If
                    dtAux.Rows(i)("TasaCupon") = Format(Decimal.Parse(tbTasaCupon.Text), "##0.0000000")
                End If
                dtAux.Rows(i)("Amortizac") = Format(amortActual, "##0.0000000")
            End If
            dtAux.Rows(i)("AmortizacConsolidado") = Format(amortAcumulada, "##0.0000000") 'Cambio por LC
            amortAcumulada = amortAcumulada + amortActual
        Next

        'verificamos monto menor a 100
        strFlagAmortizacion = Request.QueryString("vFlag")

        If (strFlagAmortizacion <> "10") Then
            If (amortAcumulada < 100 Or ddlPeriodicidad.SelectedValue = "7") Then
                If (amortAcumulada - dtAux.Rows(ultimo)("Amortizac")) <= 100 Then
                    dtAux.Rows(ultimo)("Amortizac") = Format((100 - (amortAcumulada - dtAux.Rows(ultimo)("Amortizac"))), "##0.0000000")
                Else
                    Throw New Exception("La suma de los porcentajes de las amortizaciones de los cupones es mayor 100%.")
                End If
            ElseIf amortAcumulada > 100 Then
                Throw New Exception("La suma de los porcentajes de las amortizaciones de los cupones es mayor 100%.")
            End If
        End If
        Session("cuponeraNormal") = dtAux
        gvCupones.DataSource = dtAux
        gvCupones.DataBind()
    End Sub
    Private Sub ActualizarAmortizacionAceptar()
        Dim i As Integer
        Dim dtAux As DataTable = CType(Session("cuponeraNormal"), DataTable)
        Dim amortSuma As Decimal = 0
        Dim amortActual As Decimal = 0
        Dim indiceActual As Integer = -1
        Dim strFlagAmortizacion As String

        Dim amortAcumulada As Decimal = 0
        For i = 0 To dtAux.Rows.Count - 1
            amortActual = dtAux.Rows(i)("Amortizac")
            dtAux.Rows(i)("AmortizacConsolidado") = Format(amortAcumulada, "##0.0000000") 'Cambio por LC
            amortAcumulada = amortAcumulada + amortActual
        Next
        'verificamos monto menor a 100
        strFlagAmortizacion = Request.QueryString("vFlag")
        If (strFlagAmortizacion <> "10") Then
            If (amortAcumulada < 100) Then
                dtAux.Rows(dtAux.Rows.Count - 1)("Amortizac") = Format((100 - (amortAcumulada - dtAux.Rows(dtAux.Rows.Count - 1)("Amortizac"))), "##0.0000000")
            End If
        End If
        Session("cuponeraNormal") = dtAux
        ' dgLista.DataSource = dtAux
        ' dgLista.DataBind()
    End Sub
    Private Function ActualizarAmortizacion_100(ByVal dtAux100 As DataTable) As DataTable
        Dim i As Integer
        Dim amortSuma As Decimal = 0
        For i = 0 To dtAux100.Rows.Count - 1
            amortSuma = amortSuma + dtAux100.Rows(i)("Amortizac")
        Next
        If amortSuma <> 100 Then
            dtAux100.Rows(dtAux100.Rows.Count - 1)("Amortizac") = Format(Convert.ToDecimal(dtAux100.Rows(dtAux100.Rows.Count - 1)("Amortizac")) + 100 - amortSuma, "##0.0000000")
        End If
        Return dtAux100
    End Function
    Private Function ActualizarFechasEliminarCupon(ByVal dtAuxFechas As DataTable, ByVal correlativo As Integer, ByVal Nuevafecha As Decimal) As DataTable
        Dim numerodias As String
        dtAuxFechas.Rows(correlativo)("FechaIni") = Nuevafecha
        numerodias = IIf(CInt(dtAuxFechas.Rows(correlativo)("DifDias").ToString.Trim) Mod 30 = 0, "30", "ACT")
        dtAuxFechas.Rows(correlativo)("DifDias") = DiferenciaDias(UIUtility.ConvertirFechaaString(Nuevafecha), UIUtility.ConvertirFechaaString(CDec(dtAuxFechas.Rows(correlativo)("FechaFin"))), numerodias)
        Return dtAuxFechas
    End Function
    Private Sub ActualizarFechasCuponeras(ByVal fecha As String)
        Dim i, ultimo, diferenciaDiasRes As Integer
        Dim difDias As Integer = 0
        Dim numerodias As String
        numerodias = IIf(ddlbaseDiasModificar.SelectedValue = String.Empty, "ACT", ddlbaseDiasModificar.SelectedValue)
        Dim dtAux As DataTable = CType(Session("cuponeraNormal"), DataTable)
        i = hdConsecutivo.Value - 1

        If ddlTipoActCuponera.SelectedValue = "enAdelante" Then
            ultimo = dtAux.Rows.Count - 1
        Else
            ultimo = IIf(hdConsecutivo.Value = dtAux.Rows.Count, i, hdConsecutivo.Value)
        End If

        While i < ultimo
            i = i + 1
            difDias = CType(dtAux.Rows(i)("DifDias"), Integer)
            dtAux.Rows(i)("FechaIni") = UIUtility.ConvertirFechaaDecimal(fecha)
            If i < ultimo And ddlTipoActCuponera.SelectedValue = "enAdelante" Then
                dtAux.Rows(i)("FechaFin") = UIUtility.ConvertirFechaaDecimal(ObtieneFecha(fecha, difDias, "+"))
            ElseIf i = ultimo Then
                diferenciaDiasRes = CInt(DiferenciaDias(fecha, UIUtility.ConvertirFechaaString(dtAux.Rows(i)("FechaFin")), numerodias))   'HDG INC 60479	20100719
                If diferenciaDiasRes <= 0 Then
                    Throw New Exception("La nueva fecha de Inicio (" + fecha.ToString + ") no puede ser mayor a la fecha Término (" _
                                        + UIUtility.ConvertirFechaaString(dtAux.Rows(i)("FechaFin")).ToString + ") del Cupón Nro. " + (i + 1).ToString)
                Else
                    dtAux.Rows(i)("DifDias") = diferenciaDiasRes.ToString
                End If

            End If
            fecha = UIUtility.ConvertirFechaaString(dtAux.Rows(i)("FechaFin"))
        End While

        Session("cuponeraNormal") = dtAux
        gvCupones.DataSource = dtAux
        gvCupones.DataBind()
    End Sub
    Private Sub ActualizarFechasCuponerasDiferencia(ByVal Tip As String)
        Dim fecha As String = ""
        Dim i As Integer
        Dim difDias As Integer = 0
        Dim numerodias As String
        numerodias = Request.QueryString("vNumeroDias")
        Dim dtAux As DataTable = CType(Session("cuponeraNormal"), DataTable)
        If (Tip = "Todos") Then
            i = 0
        ElseIf (Tip = "EnAdelante") Then
            i = hdConsecutivo.Value - 1
        End If
        '   difDias = tbNuevaDifDias.Text
        fecha = UIUtility.ConvertirFechaaString(dtAux.Rows(i)("FechaIni"))
        While i < dtAux.Rows.Count
            dtAux.Rows(i)("FechaIni") = UIUtility.ConvertirFechaaDecimal(fecha)
            dtAux.Rows(i)("FechaFin") = UIUtility.ConvertirFechaaDecimal(ObtieneFecha(fecha, difDias, "+"))
            dtAux.Rows(i)("DifDias") = DiferenciaDias(fecha, UIUtility.ConvertirFechaaString(dtAux.Rows(i)("FechaFin")), numerodias).ToString
            fecha = UIUtility.ConvertirFechaaString(dtAux.Rows(i)("FechaFin"))
            i = i + 1
        End While
        Session("cuponeraNormal") = dtAux
        dgLista.DataSource = dtAux
        dgLista.DataBind()
        '   tbNuevaDifDias.Text = 0
    End Sub
    Private Function ObtieneFecha(ByVal fecha As String, ByVal difDias As Integer, ByVal accion As String) As String
        Dim dtFecha As Date = CType(fecha, Date)
        Dim fechaNueva As String = ""
        If accion = "-" Then    'DISMINUYE FECHA
            fechaNueva = DateAdd(DateInterval.Day, -difDias, dtFecha).ToShortDateString
        ElseIf accion = "+" Then    'AUMENTA FECHA
            fechaNueva = DateAdd(DateInterval.Day, difDias, dtFecha).ToShortDateString
        End If
        Return fechaNueva
    End Function
    Private Function DiferenciaDias(ByVal FechaInicial As String, ByVal FechaFinal As String, ByVal NumeroDias As String) As String
        Dim DifDias As String = ""
        Dim nAnios As Decimal
        Dim nMeses As Decimal
        Dim nDias As Decimal
        Dim nDiaIni As Decimal
        Dim nDiaFin As Decimal
        Dim nAnio1 As Decimal
        Dim nAnio2 As Decimal
        Dim nDias29 As Decimal
        Dim nDia As Decimal
        If NumeroDias = "360" Or NumeroDias = "30" Then
            nAnios = Convert.ToDecimal(FechaFinal.Substring(6, 4)) - Convert.ToDecimal(FechaInicial.Substring(6, 4))
            nMeses = Convert.ToDecimal(FechaFinal.Substring(3, 2)) - Convert.ToDecimal(FechaInicial.Substring(3, 2))
            nDiaIni = Convert.ToDecimal(FechaInicial.Substring(0, 2))
            nDiaFin = Convert.ToDecimal(FechaFinal.Substring(0, 2))
            If nDiaIni > 30 Then nDiaIni = 30
            If nDiaFin > 30 Then nDiaFin = 30
            nDias = 360 * nAnios + 30 * nMeses + (nDiaFin - nDiaIni)
        Else
            nDias = DateDiff(DateInterval.Day, CType(FechaInicial, Date), CType(FechaFinal, Date))
            nAnio1 = Convert.ToDecimal(FechaInicial.Substring(6, 4))
            nAnio2 = Convert.ToDecimal(FechaFinal.Substring(6, 4))
            nMeses = Convert.ToDecimal(FechaInicial.Substring(3, 2))
            If nMeses > 2 Then nAnio1 = nAnio1 + 1
            nDia = Convert.ToDecimal(FechaFinal.Substring(0, 2))
            nMeses = Convert.ToDecimal(FechaFinal.Substring(3, 2))
            If nMeses <= 2 Then nAnio2 = nAnio2 - 1
            nDias29 = 0
            While nAnio1 <= nAnio2
                If nAnio1 Mod 4 = 0 Then nDias29 = nDias29 + 1
                nAnio1 = nAnio1 + 1
            End While
            nDias = nDias - nDias29
        End If
        DifDias = nDias.ToString
        Return DifDias
    End Function
    Private Sub LimpiarCampos()
        tbFechaInicio.Text = String.Empty
        tbFechaTermino.Text = String.Empty
        tbAmortizacion.Text = String.Empty
        tbDifDias.Text = String.Empty
        tbTasaCupon.Text = String.Empty
        ddlbaseDiasModificar.SelectedValue = "30"
        ddlbaseMesModificar.SelectedValue = "360"
        hdConsecutivo.Value = String.Empty
    End Sub
    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Dim strFlagAmortizacion As String
        Dim decTasaCupon As String
        Dim decBaseCupon As String
        Dim decTasaSpread As String
        Dim periodicidad As String
        Dim numerodias As String
        Dim oCuponera As New CuponeraBM
        Dim oDT As DataTable
        strFlagAmortizacion = Request.QueryString("vFlag")
        decTasaCupon = Request.QueryString("vTasaC").Replace(".", UIUtility.DecimalSeparator)
        decBaseCupon = Request.QueryString("vBaseC").Replace(".", UIUtility.DecimalSeparator)
        periodicidad = Request.QueryString("vPeriod")
        decTasaSpread = IIf(Request.QueryString("vTasaSpread") = "", "0", Request.QueryString("vTasaSpread").Replace(".", UIUtility.DecimalSeparator))
        numerodias = Request.QueryString("vNumeroDias")
        oDT = oCuponera.GenerarCuponeraNormal(strFlagAmortizacion, UIUtility.ConvertirFechaaDecimal(tbFechaEmision.Text), UIUtility.ConvertirFechaaDecimal(tbFechaVencimiento.Text), UIUtility.ConvertirFechaaDecimal(tbFechaPrimer.Text), decTasaCupon, decBaseCupon, periodicidad, decTasaSpread, numerodias, DatosRequest).Tables(0)
        dgLista.DataSource = oDT
        dgLista.DataBind()
        Session("cuponeraNormal") = oDT
        ActualizaCuponeraNormal()
        Actualizar_ComboApartirDe()
    End Sub
    Private Sub tbFechaTermino_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFechaTermino.TextChanged
        CalcularDiferenciaDiasModificarCupon()
    End Sub
    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Dim consecutivo As String = e.CommandArgument
        Dim i, j As Integer
        If e.CommandName = "Modificar" Then
            For i = 0 To dgLista.Rows.Count - 1
                If dgLista.Rows(i).Cells(2).Text = e.CommandArgument Then
                    pnlCupon.Visible = True
                    tbFechaInicio.Text = dgLista.Rows(i).Cells(3).Text
                    tbFechaTermino.Text = dgLista.Rows(i).Cells(4).Text
                    tbAmortizacion.Text = dgLista.Rows(i).Cells(5).Text
                    tbDifDias.Text = dgLista.Rows(i).Cells(6).Text
                    tbTasaCupon.Text = dgLista.Rows(i).Cells(7).Text
                    '  tbBase.Text = dgLista.Rows(i).Cells(8).Text
                    '   tbDiasPago.Text = dgLista.Rows(i).Cells(9).Text
                    hdConsecutivo.Value = e.CommandArgument
                    hdFechaIni.Value = tbFechaInicio.Text
                    hdFechaFin.Value = tbFechaTermino.Text
                    btnAgregar.Visible = True
                    btnAceptar.Visible = False
                    '  tbNuevaDifDias.Text = "0"
                    dgLista.SelectedIndex = i

                    Exit For
                End If
            Next
        ElseIf e.CommandName = "Eliminar" Then

            If Session("context_eliminar") IsNot Nothing Then
                'Si se desea pasar otro tipo de objeto se debe validar de esta manera
                If TypeOf Session("context_eliminar") Is Hashtable Then
                    Dim htInfo As Hashtable = Session("context_eliminar")
                    If htInfo.Contains("Eliminar") Then
                        Return
                    End If
                End If
            End If
            Dim dtAuxCuponEliminado As DataTable = CType(Session("cuponNormal_Eliminados"), DataTable)
            Dim drAuxCuponEli As DataRow = dtAuxCuponEliminado.NewRow
            drAuxCuponEli(0) = tbCodigoNemonico.Text
            drAuxCuponEli(1) = e.CommandArgument
            dtAuxCuponEliminado.Rows.Add(drAuxCuponEli)
            Session("cuponNormal_Eliminados") = dtAuxCuponEliminado
            Dim dtAux As DataTable = CType(Session("cuponeraNormal"), DataTable)
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("consecutivo") = e.CommandArgument Then
                    dtAux.Rows.RemoveAt(i)
                    Exit For
                End If
            Next
            dtAux = ActualizarAmortizacion_100(dtAux)
            dgLista.DataSource = dtAux
            dgLista.DataBind()
            Session("cuponeraNormal") = dtAux
            ActualizaCuponeraNormal()
        End If
    End Sub
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            If Session("cuponeraNormal") IsNot Nothing Then
                dgLista.PageIndex = e.NewPageIndex
                dgLista.DataSource = CType(Session("cuponeraNormal"), DataTable)
                dgLista.DataBind()
                ActualizaCuponeraNormal()
                dgLista.SelectedIndex = -1
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    'INICIO | ZOLUXIONES | RCE | RF002 - Se crea evento click para boton "Aplicar" DT y asigne el porcentaje de amortización  | 23/05/18
    Protected Sub btnAmortizar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAmortizar.Click
        Dim dtAux As DataTable = CType(Session("cuponeraNormal"), DataTable).Copy
        Dim i, ApartirCambiarAmortizacion As Integer
        Dim sumaAmortizacionTotal As Decimal = 0D

        For i = 0 To dtAux.Rows.Count - 1

            If dtAux.Rows(i)("FechaIni").ToString = ddlApartirDe.SelectedValue.ToString Then
                ApartirCambiarAmortizacion = i
                Exit For
            Else
                sumaAmortizacionTotal += Convert.ToDecimal(dtAux.Rows(i)("Amortizac").ToString)
            End If
        Next
        For i = ApartirCambiarAmortizacion To dtAux.Rows.Count - 2
            dtAux.Rows(i)("Amortizac") = Format(Convert.ToDecimal(tbPorcentajeAmortizacion.Text.Replace(".", UIUtility.DecimalSeparator)), "##0.0000000")
            sumaAmortizacionTotal += Convert.ToDecimal(dtAux.Rows(i)("Amortizac").ToString)
        Next
        'INICIO | ZOLUXIONES | RCE | RF002 - Debido a la presición de redondeo se fija el porcentaje del úlitmo cupon el resto de la sumatoria de amortización | 24/05/18
        If sumaAmortizacionTotal <= 100D Then dtAux.Rows(dtAux.Rows.Count - 1)("Amortizac") = 100D - sumaAmortizacionTotal
        'FIN | ZOLUXIONES | RCE | RF002 - Debido a la presición de redondeo se fija el porcentaje del úlitmo cupon el resto de la sumatoria de amortización | 24/05/18

        If ValidaAmortizacion_Mayor_100(dtAux) Then
            Session("CuponeraNormalAux") = dtAux.Copy
            ConfirmarJS(ObtenerMensaje("CONF48") & Right(ddlApartirDe.SelectedItem.Text, 10) & "?", "document.getElementById('btnValidar').click();")
        Else
            AlertaJS(ObtenerMensaje("CONF33"))
        End If

    End Sub
    'FIN | ZOLUXIONES | RCE | RF002 - Se crea evento click para boton "Aplicar" DT y asigne el porcentaje de amortización  | 23/05/18
    'INICIO | ZOLUXIONES | RCE | RF002 - Se crea el evento de cambio de selección de item en nuevo Combo "A partir de" para calcular el porcentaje de amortización" | 24/05/18
    Protected Sub ddlApartirDe_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlApartirDe.SelectedIndexChanged
        Dim AmortizacionAnterior As Decimal = 0
        Dim i As Integer = 0
        Dim indice As Integer = 0
        Dim obternerConsecutivo(2) As String
        Dim calculoIndice As Integer = 0

        Try
            While (CType(Session("cuponeraNormal"), DataTable).Rows(i)("FechaIni").ToString <> ddlApartirDe.SelectedValue.ToString)
                AmortizacionAnterior += Convert.ToDecimal(CType(Session("cuponeraNormal"), DataTable).Rows(i)("Amortizac"))
                i += 1
            End While
            tbPorcentajeAmortizacion.Text = ((100 - AmortizacionAnterior) / (ddlApartirDe.Items.Count - ddlApartirDe.SelectedIndex)).ToString
            'INICIO | ZOLUXIONES | RCE | RF002 - Se implementa condicionales para cuando se cambie el valor del combo se fije en la grilla la fila correspondiente y pintado para facilitadad de ubicación para usuario| 24/05/18
            obternerConsecutivo = ddlApartirDe.SelectedItem.Text.Split("-")
            If Session("cuponeraNormal") IsNot Nothing Then
                calculoIndice = Convert.ToInt32(obternerConsecutivo(0)) Mod 10
                If calculoIndice > 0 Then
                    indice = Convert.ToInt32(obternerConsecutivo(0)) \ 10
                    calculoIndice = calculoIndice - 1
                Else
                    indice = (Convert.ToInt32(obternerConsecutivo(0)) / 10) - 1
                    calculoIndice = 9
                End If
                dgLista.PageIndex = indice
                dgLista.DataSource = CType(Session("cuponeraNormal"), DataTable)
                dgLista.DataBind()
                ActualizaCuponeraNormal()
                dgLista.SelectedIndex = calculoIndice
            End If
            'FIN | ZOLUXIONES | RCE | RF002 - Se implementa condicionales para cuando se cambie el valor del combo se fije en la grilla la fila correspondiente y pintado para facilitadad de ubicación para usuario| 24/05/18
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    'FIN | ZOLUXIONES | RCE | RF002 - Se crea el evento de cambio de selección de item en nuevo Combo "A partir de" para calcular el porcentaje de amortización" | 24/05/18

    'INICIO | ZOLUXIONES | RCE | RF002 - Se implementa función para validar la sumatoria del porcentaje ingresado en la grilla | 24/05/18
    Private Function ValidaAmortizacion_Mayor_100(ByVal dtAux100 As DataTable) As Boolean
        Dim i As Integer
        Dim amortSuma As Decimal = 0

        For i = 0 To dtAux100.Rows.Count - 1
            amortSuma = amortSuma + dtAux100.Rows(i)("Amortizac")
        Next
        If amortSuma > 100 Then
            Return False
        End If
        Return True
    End Function
    'FIN | ZOLUXIONES | RCE | RF002 - Se implementa función para validar la sumatoria del porcentaje ingresado en la grilla | 24/05/18
    'INICIO | ZOLUXIONES | RCE | RF002 - Procedimiento recursivo para confirmación de cambio de porcentaje de amortizacón | 24/05/18
    Protected Sub btnValidar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidar.Click
        Session("cuponeraNormal") = Session("CuponeraNormalAux")
        dgLista.DataSource = CType(Session("cuponeraNormal"), DataTable)
        dgLista.DataBind()
        ActualizaCuponeraNormal()
        Session("UtilizaAmortizacion") = True
    End Sub
    Private Sub Actualizar_ComboApartirDe()
        'INICIO | ZOLUXIONES | RCE | RF002 - Se crea nuevo DT para cargar nuevo combo "A partir de" | 23/05/18
        Dim DtTablaApartirDeTemp As DataTable
        Dim DtTablaApartirDeFiltrado As DataTable = New DataTable()
        Dim dtTemporal As New DataTable
        Dim IdFecha As String
        Dim FechaFormateada As String
        Dim AmortizacionAnterior As Decimal
        Dim i As Integer
        'FIN | ZOLUXIONES | RCE | RF002 - Se crea nuevo DT para cargar nuevo combo "A partir de" | 23/05/18
        If Not Session("cuponeraNormal") Is Nothing Then

            dtTemporal = CType(Session("cuponeraNormal"), DataTable).Copy
            If dtTemporal.Select("FechaIni >" & DateTime.Now.ToString("yyyyMMdd")).Length > 0 Then
                'INICIO | ZOLUXIONES | RCE | RF002 - Se reutiliza dtTemporal filtrado por Fechas mayores a la actual para cargar nuevo combo "A partir de" | 23/05/18
                DtTablaApartirDeTemp = dtTemporal.Select("FechaIni >" & DateTime.Now.ToString("yyyyMMdd")).CopyToDataTable()
                DtTablaApartirDeFiltrado.Columns.Add("IdFecha", GetType(Decimal))
                DtTablaApartirDeFiltrado.Columns.Add("FechaFormateada", GetType(String))

                'INICIO | ZOLUXIONES | RCE | RF002 - Se implementa rutina para formatear valores mostrados en combo "A partir de": [Número de Cupon - Fecha Inicio]  | 24/05/18
                For i = 0 To DtTablaApartirDeTemp.Rows.Count - 1
                    Dim j As Integer = DtTablaApartirDeFiltrado.Rows.Count
                    IdFecha = DtTablaApartirDeTemp.Rows(i)("FechaIni")
                    FechaFormateada = DtTablaApartirDeTemp.Rows(i)("Consecutivo") & " - " & UIUtility.ConvertirFechaaString(Convert.ToDecimal(DtTablaApartirDeTemp.Rows(i)("FechaIni")))
                    DtTablaApartirDeFiltrado.Rows.Add(j)("IdFecha") = IdFecha
                    DtTablaApartirDeFiltrado.Rows(j)("FechaFormateada") = FechaFormateada
                Next
                'FIN | ZOLUXIONES | RCE | RF002 - Se implementa rutina para formatear valores mostrados en combo "A partir de": [Número de Cupon - Fecha Inicio]  | 24/05/18
                HelpCombo.LlenarComboBox(ddlApartirDe, DtTablaApartirDeFiltrado, "IdFecha", "FechaFormateada", False)

                i = 0
                AmortizacionAnterior = 0

                'INICIO | ZOLUXIONES | RCE | RF002 - Se implementa rutina para obtener primer valor de porcentaje de amortización en base a primera fecha disponible y amortizaciones previas  | 24/05/18
                While (CType(Session("cuponeraNormal"), DataTable).Rows(i)("FechaIni").ToString <> DtTablaApartirDeFiltrado.Rows(0)("IdFecha").ToString)
                    AmortizacionAnterior += Convert.ToDecimal(CType(Session("cuponeraNormal"), DataTable).Rows(i)("Amortizac"))
                    i += 1
                End While
                'FIN | ZOLUXIONES | RCE | RF002 - Se implementa rutina para obtener primer valor de porcentaje de amortización en base a primera fecha disponible y amortizaciones previas  | 24/05/18
                tbPorcentajeAmortizacion.Text = ((100 - AmortizacionAnterior) / DtTablaApartirDeTemp.Rows.Count).ToString
                dgLista.SelectedIndex = -1

                'FIN | ZOLUXIONES | RCE | RF002 - Se reutiliza dtTemporal filtrado por Fechas mayores a la actual para cargar nuevo combo "A partir de" | 23/05/18
            End If
        End If
    End Sub
    'FIN | ZOLUXIONES | RCE | RF002 - Procedimiento recursivo para confirmación de cambio de porcentaje de amortizacón | 24/05/18

    Protected Sub btnCalcularPeriodica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalcularPeriodica.Click
        ViewState("FlagSinCalcular") = "1"
        CalcularCuponerPeriodica()
        btnAceptar.Enabled = True
    End Sub
    Protected Sub gvCupones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCupones.PageIndexChanging
        Try
            'If Session("cuponeraNormal") IsNot Nothing Then
            '    dgLista.PageIndex = e.NewPageIndex
            '    dgLista.DataSource = CType(Session("cuponeraNormal"), DataTable)
            '    dgLista.DataBind()
            '    ActualizaCuponeraNormal()
            '    dgLista.SelectedIndex = -1
            'End If
            gvCupones.PageIndex = e.NewPageIndex
            If dtCuponeraNormalSesion Is Nothing Then
                CalcularCuponerPeriodica()
            Else
                gvCupones.DataSource = dtCuponeraNormalSesion
                gvCupones.DataBind()
                ActualizaCuponeraNormalNuevo()

                gvCupones.SelectedIndex = -1
            End If


            'gvCupones.SelectedIndex = -1
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub gvCupones_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCupones.RowCommand
        Dim consecutivo As String = e.CommandArgument
        btnAgregar.Text = e.CommandName.ToString
        If e.CommandName = "Modificar" Then
            For i = 0 To gvCupones.Rows.Count - 1
                If gvCupones.Rows(i).Cells(0).Text = e.CommandArgument Then
                    pnlCupon.Visible = True
                    tbFechaInicio.Text = gvCupones.Rows(i).Cells(1).Text
                    tbFechaTermino.Text = gvCupones.Rows(i).Cells(2).Text
                    tbDifDias.Text = gvCupones.Rows(i).Cells(3).Text
                    tbTasaCupon.Text = gvCupones.Rows(i).Cells(17).Text
                    tbAmortizacion.Text = gvCupones.Rows(i).Cells(5).Text
                    hdConsecutivo.Value = e.CommandArgument
                    hdFechaIni.Value = tbFechaInicio.Text
                    hdFechaFin.Value = tbFechaTermino.Text
                    ddlbaseDiasModificar.SelectedValue = IIf(CInt(tbDifDias.Text.Trim) Mod 30 = 0, "30", "ACT")
                    ddlbaseMesModificar.SelectedValue = IIf(CInt(gvCupones.Rows(i).Cells(14).Text.Trim) = 366, "ACT", gvCupones.Rows(i).Cells(14).Text.Trim)
                    btnAgregar.Visible = True
                    btnAceptar.Visible = False
                    gvCupones.SelectedIndex = i
                    ddlTipoActCuponera.Enabled = True
                    lblAccionCupon.Text = "Modificar"
                    Exit For
                End If
            Next
        ElseIf e.CommandName = "Agregar" Then
            For i = 0 To gvCupones.Rows.Count - 1
                If gvCupones.Rows(i).Cells(0).Text = e.CommandArgument Then
                    pnlCupon.Visible = True
                    tbFechaInicio.Text = gvCupones.Rows(i).Cells(2).Text
                    tbFechaTermino.Text = UIUtility.ConvertirFechaaString(CType(Convert.ToDateTime(tbFechaInicio.Text).AddDays(30).ToString("yyyyMMdd"), Decimal))
                    tbAmortizacion.Text = "0"
                    tbTasaCupon.Text = "0"
                    hdConsecutivo.Value = CInt(e.CommandArgument) + 1
                    hdFechaIni.Value = tbFechaInicio.Text
                    hdFechaFin.Value = tbFechaTermino.Text

                    ddlbaseMesModificar.SelectedValue = IIf(CInt(gvCupones.Rows(i).Cells(14).Text.Trim) = 366, "ACT", gvCupones.Rows(i).Cells(14).Text.Trim)
                    CalcularDiferenciaDiasModificarCupon()
                    ddlbaseDiasModificar.SelectedValue = IIf(CInt(tbDifDias.Text.Trim) Mod 30 = 0, "30", "ACT")
                    btnAgregar.Visible = True
                    btnAceptar.Visible = False
                    gvCupones.SelectedIndex = i
                    ddlTipoActCuponera.Enabled = False
                    lblAccionCupon.Text = "Agregar"
                    Exit For
                End If
            Next
        ElseIf e.CommandName = "Eliminar" Then
            Dim dtAux As DataTable = CType(Session("cuponeraNormal"), DataTable),
                fechafinProximo As Decimal = 0, correlativo As Integer = 0
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("consecutivo") = e.CommandArgument Then
                    fechafinProximo = dtAux.Rows(i)("FechaIni")
                    correlativo = i
                    dtAux.Rows.RemoveAt(i)
                    Exit For
                End If
            Next
            If correlativo < dtAux.Rows.Count - 1 Then dtAux = ActualizarFechasEliminarCupon(dtAux, correlativo, fechafinProximo)
            dtAux = ActualizarAmortizacion_100(dtAux)
            gvCupones.DataSource = dtAux
            gvCupones.DataBind()
            Session("cuponeraNormal") = dtAux
            ActualizaCuponeraNormalNuevo()
        End If
    End Sub
    Protected Sub gvCupones_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCupones.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim HeaderGrid As GridView = sender
            Dim HeaderGridRow As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
            Dim HeaderCell As New TableCell

            HeaderCell.Text = "DATOS CUPÓN"
            HeaderCell.ColumnSpan = 6
            HeaderCell.BorderColor = Drawing.Color.Gray
            HeaderCell.VerticalAlign = VerticalAlign.Middle
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow.Cells.Add(HeaderCell)

            HeaderCell = New TableCell()
            HeaderCell.Text = "EMISOR"
            HeaderCell.ColumnSpan = 3
            HeaderCell.BorderColor = Drawing.Color.Gray
            HeaderCell.VerticalAlign = VerticalAlign.Middle
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow.Cells.Add(HeaderCell)

            HeaderCell = New TableCell()
            HeaderCell.Text = "ADMINISTRADORA"
            HeaderCell.ColumnSpan = 4
            HeaderCell.BorderColor = Drawing.Color.Gray
            HeaderCell.VerticalAlign = VerticalAlign.Middle
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow.Cells.Add(HeaderCell)


            gvCupones.Controls(0).Controls.AddAt(0, HeaderGridRow)

        End If
    End Sub
    Protected Sub gvCupones_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCupones.RowDataBound
        Dim ibSeleccionar As ImageButton, ibEliminar As ImageButton, ibAgregar As ImageButton, lblTasaCupon As Label, lblParticipacion As Label

        If e.Row.RowType = DataControlRowType.DataRow Then
            hdEstado = CType(e.Row.FindControl("hdEstado"), HiddenField)
            ibSeleccionar = CType(e.Row.FindControl("ibModificar"), ImageButton)
            ibEliminar = CType(e.Row.FindControl("ibEliminar"), ImageButton)
            ibAgregar = CType(e.Row.FindControl("ibAgregar"), ImageButton)
            lblTasaCupon = CType(e.Row.FindControl("lblTasaCupon"), Label)
            lblParticipacion = CType(e.Row.FindControl("lblParticipacion"), Label)

            If hdEstado.Value.Trim().Equals("0") Then
                e.Row.Attributes.Add("style", "background-color: #dad9ce")
                ibSeleccionar.Visible = False
                ibEliminar.Visible = False
                ibAgregar.Visible = False
            End If
            ' Tasa Variable
            If CDec(IIf(System.Web.HttpUtility.HtmlDecode(e.Row.Cells(16).Text).Trim = String.Empty, 0, e.Row.Cells(16).Text)) > 0 Then
                lblTasaCupon.CssClass = "selector"
                lblTasaCupon.Attributes.Add("onclick", "javascript:AgregarFilaTasaFijaVariable('" & String.Format("{0:###,##0.0000000}", CDec(IIf(System.Web.HttpUtility.HtmlDecode(e.Row.Cells(17).Text).Trim = String.Empty, 0, e.Row.Cells(17).Text)) - CDec(IIf(e.Row.Cells(16).Text.Trim = String.Empty, 0, e.Row.Cells(16).Text))) & "','" _
                                                                                                  & String.Format("{0:###,##0.0000000}", e.Row.Cells(16).Text) & "');")
            End If
            If CDec(IIf(System.Web.HttpUtility.HtmlDecode(e.Row.Cells(10).Text).Trim = String.Empty, 0, e.Row.Cells(10).Text)) > 0 Then   ' Participacion de Fondo
                lblParticipacion.CssClass = "selector"
                lblParticipacion.Attributes.Add("onclick", "javascript:MostrarDetalleParticipacion(" & e.Row.Cells(1).Text & "," & _
                                                                                                        e.Row.Cells(2).Text & "," & _
                                                                                                        e.Row.Cells(0).Text & "," & _
                                                                                                        hdEstado.Value & "," & _
                                                                                                        CDec(e.Row.Cells(6).Text) & "," & _
                                                                                                        e.Row.Cells(17).Text & "," & _
                                                                                                        e.Row.Cells(3).Text & "," & _
                                                                                                        e.Row.Cells(14).Text & "," & _
                                                                                                        e.Row.Cells(5).Text & "," & _
                                                                                                        e.Row.Cells(18).Text & ");")
            End If
        End If

    End Sub
    Protected Sub ddlPeriodicidad_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriodicidad.SelectedIndexChanged
        reiniciarColAccion()
    End Sub

    Public Sub reiniciarColAccion()
        If ddlPeriodicidad.SelectedValue <> 7 Then
            gvCupones.Columns(15).HeaderStyle.CssClass = "ocultarCol"
            gvCupones.Columns(15).ItemStyle.CssClass = "ocultarCol"
            ddlTipoAmortizacion.Enabled = ddlPeriodicidad.Enabled
            txtTasaCupon.Enabled = ddlPeriodicidad.Enabled
            ddlBaseDias.Enabled = ddlPeriodicidad.Enabled
            ddlBaseMes.Enabled = ddlPeriodicidad.Enabled
            btnCalcularPeriodica.Enabled = ddlPeriodicidad.Enabled
            ddlTasaVariable.Enabled = ddlPeriodicidad.Enabled
            txtTasaVariable.Enabled = Not (ddlTasaVariable.SelectedValue = String.Empty)
            ddlPeriodicidadVariable.Enabled = Not (ddlTasaVariable.SelectedValue = String.Empty)
            btnAceptar.Enabled = False
            pnlCupon.Visible = False
            If Session("accionValor") <> "INGRESAR" Then btnAceptar.Enabled = False
        Else
            gvCupones.Columns(15).HeaderStyle.CssClass = String.Empty
            gvCupones.Columns(15).ItemStyle.CssClass = String.Empty
            ddlTipoAmortizacion.Enabled = False
            btnAceptar.Enabled = True
            txtTasaCupon.Enabled = False
            ddlBaseDias.Enabled = False
            ddlBaseMes.Enabled = False
            btnCalcularPeriodica.Enabled = False
            ddlTasaVariable.Enabled = False
            txtTasaVariable.Enabled = False
            ddlPeriodicidadVariable.Enabled = False
        End If
    End Sub
    Public Sub CalcularDiferenciaDiasModificarCupon()
        Dim numerodias As String = IIf(ddlbaseDiasModificar.SelectedValue = String.Empty, "ACT", ddlbaseDiasModificar.SelectedValue)
        tbDifDias.Text = DiferenciaDias(tbFechaInicio.Text, tbFechaTermino.Text, numerodias)
    End Sub
    Public Sub habilitarTasaVariable()
        txtTasaVariable.Enabled = Not (ddlTasaVariable.SelectedValue = String.Empty Or ddlPeriodicidad.SelectedValue = "7")
        ddlPeriodicidadVariable.Enabled = Not (ddlTasaVariable.SelectedValue = String.Empty Or ddlPeriodicidad.SelectedValue = "7")
        If ddlTasaVariable.SelectedValue <> String.Empty Then
            HelpCombo.LlenarComboBox(ddlPeriodicidadVariable, oParametrosGenerales.Listar("PeriodoTasaLibor", DatosRequest), "Valor", "Nombre", False)
            ddlPeriodicidadVariable.SelectedValue = ViewState("DiasTTasaVariable")
        Else
            ddlPeriodicidadVariable.Items.Clear()
            ddlPeriodicidadVariable.SelectedValue = String.Empty
        End If
    End Sub
    Public Function validarCamposVacios() As Boolean
        If tbFechaTermino.Text.Trim = String.Empty Then Return False
        If tbDifDias.Text.Trim = String.Empty Then Return False
        If tbTasaCupon.Text.Trim = String.Empty Then Return False
        If tbAmortizacion.Text.Trim = String.Empty Then Return False
        Return True
    End Function
    Public Sub CalcularTasaLibor()
        Dim fechaInicioCupon As String
        Dim fechaReferencia As Decimal
        For index = 0 To dtCuponeraNormalSesion().Rows.Count - 1
            If dtCuponeraNormalSesion().Rows(index)("Estado").ToString = "1" Then
                fechaInicioCupon = UIUtility.ConvertirFechaaString(CType(dtCuponeraNormalSesion().Rows(index)("FechaIni").ToString, Decimal))
                fechaReferencia = UIUtility.ConvertirFechaaDecimal(ObtieneFecha(fechaInicioCupon, CInt(ddlPeriodicidadVariable.SelectedValue), "+"))
                Exit For
            End If
        Next
        txtTasaVariable.Text = CStr(oIndicador.Indicador_SeleccionarValorLibor(ddlTasaVariable.SelectedValue, fechaReferencia))
    End Sub

    Protected Sub ddlbaseDiasModificar_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlbaseDiasModificar.SelectedIndexChanged
        CalcularDiferenciaDiasModificarCupon()
    End Sub

    Protected Sub ddlTasaVariable_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTasaVariable.SelectedIndexChanged
        ViewState("DiasTTasaVariable") = "0"
        habilitarTasaVariable()
        If ddlPeriodicidadVariable.SelectedValue.Trim <> String.Empty Then CalcularTasaLibor() Else txtTasaVariable.Text = "0"
        btnAceptar.Enabled = False
    End Sub

    Protected Sub ddlPeriodicidadVariable_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriodicidadVariable.SelectedIndexChanged
        CalcularTasaLibor()
        btnAceptar.Enabled = False
    End Sub
End Class

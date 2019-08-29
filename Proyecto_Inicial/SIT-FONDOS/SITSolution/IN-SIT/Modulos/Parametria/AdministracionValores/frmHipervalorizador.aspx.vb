Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Partial Class Modulos_Parametria_AdministracionValores_frmHipervalorizador
    Inherits BasePage
    Dim dtValorizador As DataTable
#Region " /* Metodos de Pagina */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            If Not Page.IsPostBack Then
                lblCodigoNemonico.Text = Request.QueryString("nemonico")
                tbPrincipal.Text = Request.QueryString("valorUnitario")
                tbRendimiento.Text = Request.QueryString("tasaCupon")
                tbBaseTIR.Text = Request.QueryString("baseTIR")
                tbBaseDias.Text = Request.QueryString("baseDias")
                lblFechaEmision.Text = Request.QueryString("fechaEmision")
                Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                ddlParaTIR.DataSource = oParametrosGeneralesBM.Listar("Para TIR", DatosRequest)
                ddlParaTIR.DataTextField = "Nombre"
                ddlParaTIR.DataValueField = "Valor"
                ddlParaTIR.DataBind()
                ddlBaseCupon.DataSource = oParametrosGeneralesBM.Listar("Base Cupón", DatosRequest)
                ddlBaseCupon.DataTextField = "Nombre"
                ddlBaseCupon.DataValueField = "Valor"
                ddlBaseCupon.DataBind()
                ddlTreasuries.DataSource = oParametrosGeneralesBM.Listar("Treasuries", DatosRequest)
                ddlTreasuries.DataTextField = "Nombre"
                ddlTreasuries.DataValueField = "Valor"
                ddlTreasuries.DataBind()
                tbFechaCompra.Text = Request.QueryString("fechaEmision")
                tbPrecio.Text = Request.QueryString("valorUnitario")
                tbRendimientoNegociacion.Text = Request.QueryString("tasaCupon")
                hdnTipoCuponera.Value = Request.QueryString("tipoCuponera")
                obtenerValorIndicador(tbVACEmision, lblFechaEmision.Text)
                Dim codigoSBS As String = Request.QueryString("codigoSBS")
                If Not Left(codigoSBS, 2).Equals("08") Then
                    ddlTreasuries.SelectedIndex = 1 'NORMAL
                Else
                    ddlTreasuries.SelectedIndex = 0
                End If
                If (codigoSBS.Substring(7, 1).Equals("3") Or codigoSBS.Substring(7, 1).Equals("1")) And Left(codigoSBS, 2).Equals("01") Then
                    ddlTreasuries.SelectedIndex = 0
                Else
                    ddlTreasuries.SelectedIndex = 1
                End If

                If (codigoSBS.Substring(6, 1).Equals("3")) Then
                    ddlValoresAjustados.SelectedValue = "1"
                Else
                    ddlValoresAjustados.SelectedValue = "2"
                End If
                Procesar()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Sistema")
        End Try
    End Sub
    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        EjecutarJS("window.close();")
    End Sub
#End Region
#Region " /* Metodos personalizados */ "
    Private Sub Procesar()
        obtenerValorIndicador(tbVACActual, tbFechaCompra.Text)
        calcularPrincipalAjustado()
        crearGrilla()
    End Sub
    Private Sub calcularPrincipalAjustado()
        Dim dcValor As Decimal
        If ddlValoresAjustados.SelectedValue() = 1 Then
            If (tbVACActual.Text = "1.0000000") Then
                dcValor = Decimal.Parse(tbPrincipal.Text)
            Else
                dcValor = Decimal.Parse(tbPrincipal.Text) * Decimal.Parse(tbVACActual.Text) / Decimal.Parse(tbVACEmision.Text)
            End If
        Else
            dcValor = Decimal.Parse(tbPrincipal.Text)
        End If
        tbPrincipalAjustado.Text = Format(dcValor, "##,##0.0000000")
    End Sub
    Private Sub obtenerValorIndicador(ByVal controlVAC As TextBox, ByVal strFecha As String)
        Dim oBMindicador As New IndicadorBM
        Dim oDT As DataTable
        If strFecha <> "" Then
            oDT = oBMindicador.SeleccionarValorIndicador("FACVAC", UIUtility.ConvertirFechaaDecimal(strFecha), DatosRequest).Tables(0)
            If oDT.Rows.Count > 0 Then
                controlVAC.Text = oDT.Rows(0)("Valor")
            Else
                controlVAC.Text = "1.0000000"
            End If
        End If
    End Sub
    Private Sub cargarCombos()
        Dim DtTablaCuponBase As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        DtTablaCuponBase = oParametrosGeneralesBM.ListarBaseCupon(DatosRequest)
        HelpCombo.LlenarComboBox(ddlBaseCupon, DtTablaCuponBase, "Valor", "Nombre", False)
    End Sub
    Private Sub crearGrilla()
        Dim oCuponeraBM As New CuponeraBM
        Dim dtCuponera As DataTable
        Dim drValorizador As DataRow
        Dim i, intValorDias As Int32
        Dim dcValorCupon, dcValorTIR, dcValorVA As Decimal
        Dim valor As Decimal
        Dim diasEntreFechaCuponyCompra As Int32
        Dim diasEntreFechaCuponyCompra360 As Int32
        If hdnTipoCuponera.Value.Equals("N") Then
            dtCuponera = oCuponeraBM.LeerCuponeraNormal(lblCodigoNemonico.Text, DatosRequest).Tables(0)
            If dtCuponera.Rows.Count = 0 Then
                AlertaJS("No existe el " + lblCodigoNemonico.Text + " en la Cuponera Normal")
                Exit Sub
            End If
        Else
            dtCuponera = oCuponeraBM.LeerCuponeraEspecial(lblCodigoNemonico.Text, DatosRequest).Tables(0)
        End If
        dtValorizador = crearTablaInicialHiperValorizador()
        For i = 0 To dtCuponera.Rows.Count - 1
            drValorizador = dtValorizador.NewRow()
            drValorizador.BeginEdit()
            drValorizador("Evento") = "Cupón " & Int32.Parse(i).ToString()
            drValorizador("Fecha") = UIUtility.ConvertirFechaaString(dtCuponera.Rows(i)("FechaIni"))
            If i > 0 Then
                intValorDias = dtCuponera.Rows(i - 1)("DifDias")
                drValorizador("Dias") = intValorDias
                drValorizador("AmortAcum") = dtCuponera.Rows(i - 1)("AmortizacConsolidado")
                valor = Convert.ToDouble(Convert.ToDouble(tbPrincipalAjustado.Text) * dtCuponera.Rows(i - 1)("Amortizac") / 100)
                drValorizador("Amortiza") = Math.Round(valor, 2)
                If ddlTasaCupon.SelectedValue() = 1 Then
                    dcValorCupon = ((1 - (dtCuponera.Rows(i - 1)("AmortizacConsolidado")) / 100)) * Decimal.Parse(tbPrincipalAjustado.Text) *
                    ((dtCuponera.Rows(i - 1)("TasaCupon") / 100) * dtCuponera.Rows(i - 1)("DifDias")) / dtCuponera.Rows(i - 1)("BaseCupon")
                Else
                    dcValorCupon = Decimal.Parse(tbPrincipalAjustado.Text) * ((Math.Pow(1 + dtCuponera.Rows(i - 1)("TasaCupon") / 100,
                    dtCuponera.Rows(i - 1)("DifDias") / dtCuponera.Rows(i - 1)("BaseCupon")) - 1))
                End If
                diasEntreFechaCuponyCompra = DateDiff("d", UIUtility.ConvertirStringaFecha(tbFechaCompra.Text),
                UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(dtCuponera.Rows(i - 1)("FechaFin"))))
                diasEntreFechaCuponyCompra360 = Dias360(UIUtility.ConvertirStringaFecha(tbFechaCompra.Text),
                UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(dtCuponera.Rows(i - 1)("FechaFin"))))
                If ddlTreasuries.SelectedValue = "2" Then
                    dcValorTIR = Math.Pow((1 + (Decimal.Parse(IIf(tbRendimientoNegociacion.Text = "", 0, tbRendimientoNegociacion.Text)) / 100)),
                    (diasEntreFechaCuponyCompra / dtCuponera.Rows(i - 1)("BaseCupon"))) - 1
                    If dcValorTIR >= 0 Then
                        drValorizador("TIR") = dcValorTIR * 100
                    Else
                        drValorizador("TIR") = 0
                    End If
                Else
                    dcValorTIR = Math.Pow((1 + (Decimal.Parse(IIf(tbRendimientoNegociacion.Text = "", 0, tbRendimientoNegociacion.Text)) / 100)),
                    (diasEntreFechaCuponyCompra360 / dtCuponera.Rows(i - 1)("DiasPago"))) - 1
                    If dcValorTIR >= 0 Then
                        drValorizador("TIR") = dcValorTIR * 100
                    Else
                        drValorizador("TIR") = 0
                    End If
                End If
                If (dcValorTIR > 0) Then
                    dcValorVA = (dcValorCupon + valor) / (1 + dcValorTIR)
                Else
                    dcValorVA = 0
                End If
                drValorizador("VA") = dcValorVA
                drValorizador("Fecha") = UIUtility.ConvertirFechaaString(dtCuponera.Rows(i - 1)("FechaFin"))
            Else 'para el primer registro
                drValorizador("Dias") = "0"
                drValorizador("Evento") = "Emisión"
                drValorizador("AmortAcum") = "0"
                drValorizador("Amortiza") = "0"
                drValorizador("Fecha") = UIUtility.ConvertirFechaaString(dtCuponera.Rows(i)("FechaIni"))
                dcValorCupon = 0
                dcValorTIR = 0
                drValorizador("TIR") = 0
                drValorizador("VA") = 0
            End If
            drValorizador("Cupon") = dcValorCupon
            drValorizador.EndEdit()
            dtValorizador.Rows.Add(drValorizador)
        Next
        drValorizador = dtValorizador.NewRow()
        drValorizador.BeginEdit()
        drValorizador("Evento") = "Cupón " & (dtCuponera.Rows.Count).ToString()
        drValorizador("Fecha") = UIUtility.ConvertirFechaaString(dtCuponera.Rows(dtCuponera.Rows.Count - 1)("FechaFin"))
        Dim fechaanterior As String = UIUtility.ConvertirFechaaString(dtCuponera.Rows(dtCuponera.Rows.Count - 1)("FechaIni"))
        diasEntreFechaCuponyCompra = DateDiff("d", UIUtility.ConvertirStringaFecha(tbFechaCompra.Text),
        UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(dtCuponera.Rows(dtCuponera.Rows.Count - 1)("FechaFin"))))
        diasEntreFechaCuponyCompra360 = Dias360(UIUtility.ConvertirStringaFecha(tbFechaCompra.Text),
        UIUtility.ConvertirStringaFecha(UIUtility.ConvertirFechaaString(dtCuponera.Rows(dtCuponera.Rows.Count - 1)("FechaFin"))))
        If ddlTreasuries.SelectedValue = "2" Then
            dcValorTIR = Math.Pow((1 + (Decimal.Parse(IIf(tbRendimientoNegociacion.Text = "", 0, tbRendimientoNegociacion.Text)) / 100)),
            (diasEntreFechaCuponyCompra / dtCuponera.Rows(dtCuponera.Rows.Count - 1)("BaseCupon"))) - 1
            If dcValorTIR >= 0 Then
                drValorizador("TIR") = dcValorTIR * 100
            Else
                drValorizador("TIR") = 0
            End If
        Else
            dcValorTIR = Math.Pow((1 + (Decimal.Parse(IIf(tbRendimientoNegociacion.Text = "", 0, tbRendimientoNegociacion.Text)) / 100)),
            (diasEntreFechaCuponyCompra360 / dtCuponera.Rows(dtCuponera.Rows.Count - 1)("DiasPago"))) - 1
            If dcValorTIR >= 0 Then
                drValorizador("TIR") = dcValorTIR * 100
            Else
                drValorizador("TIR") = 0
            End If
        End If

        If ddlTasaCupon.SelectedValue() = 1 Then
            dcValorCupon = ((1 - (dtCuponera.Rows(dtCuponera.Rows.Count - 1)("AmortizacConsolidado")) / 100)) * Decimal.Parse(tbPrincipalAjustado.Text) *
            ((dtCuponera.Rows(dtCuponera.Rows.Count - 1)("TasaCupon") / 100) * dtCuponera.Rows(dtCuponera.Rows.Count - 1)("DifDias")) /
            dtCuponera.Rows(dtCuponera.Rows.Count - 1)("BaseCupon")
        Else
            dcValorCupon = Decimal.Parse(tbPrincipalAjustado.Text) * ((Math.Pow(1 + dtCuponera.Rows(dtCuponera.Rows.Count - 1)("TasaCupon") / 100,
            dtCuponera.Rows(dtCuponera.Rows.Count - 1)("DifDias") / dtCuponera.Rows(dtCuponera.Rows.Count - 1)("BaseCupon")) - 1))
        End If
        drValorizador("Cupon") = dcValorCupon
        intValorDias = dtCuponera.Rows(dtCuponera.Rows.Count - 1)("DifDias")
        drValorizador("Dias") = intValorDias
        drValorizador("AmortAcum") = dtCuponera.Rows(dtCuponera.Rows.Count - 1)("AmortizacConsolidado")
        valor = Convert.ToDouble(dtCuponera.Rows(dtCuponera.Rows.Count - 1)("Amortizac") * Convert.ToDouble(tbPrincipalAjustado.Text) / 100)
        drValorizador("Amortiza") = Math.Round(valor, 2)
        If (dcValorTIR > 0) Then
            dcValorVA = (dcValorCupon + valor) / (1 + dcValorTIR)
        Else
            dcValorVA = 0
        End If
        drValorizador("VA") = dcValorVA
        drValorizador.EndEdit()
        dtValorizador.Rows.Add(drValorizador)
        Session("HValorizador") = dtValorizador
        dgLista.DataSource = dtValorizador
        dgLista.DataBind()
        obtenerFechaUltimoCupon()
        obtenerFechaSiguienteCupon()
        Dim objFormulasOI As New OrdenInversionFormulasBM
        Dim InteresCorrido As Decimal
        'OT 10090 - 26/07/2017 - Carlos Espejo
        'Se quita el parametro GUID y DataRequest
        InteresCorrido = objFormulasOI.CalcularInteresesCorridos2(lblCodigoNemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaCompra.Text), _
        Convert.ToDecimal(tbPrincipalAjustado.Text), Convert.ToDecimal(IIf(tbRendimiento.Text = "", 0, tbRendimiento.Text)), ddlTasaCupon.SelectedValue)
        'OT 10090 Fin
        If InteresCorrido = -1 Then
            AlertaJS("Los Datos ingresados no son consistente para el calculo del interes corrido")
            Exit Sub
        End If
        tbMasIntereses.Text = Format(InteresCorrido, "##,##0.00")
        calcularVATotal()
        tbPrincipal.Text = Format(Convert.ToDecimal(tbPrincipal.Text), "##,##0.0000000")
    End Sub
    Private Function crearTablaInicialHiperValorizador() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("Evento", Type.GetType("System.String"))
        dt.Columns.Add("Fecha", Type.GetType("System.String"))
        dt.Columns.Add("Cupon", Type.GetType("System.Decimal"))
        dt.Columns.Add("TIR", Type.GetType("System.Decimal"))
        dt.Columns.Add("VA", Type.GetType("System.Decimal"))
        dt.Columns.Add("Dias", Type.GetType("System.Int32"))
        dt.Columns.Add("Amortiza", Type.GetType("System.Decimal"))
        dt.Columns.Add("AmortAcum", Type.GetType("System.Decimal"))
        Return dt
    End Function
    Private Function calcularValorCupon(ByVal indice As Int32, ByVal fechaCupon As String) As Decimal
        Dim dcValorCupon As Decimal
        Dim diasEntreFechas As Int32
        diasEntreFechas = obtenerdiferenciaFechaEmisionCupon(indice, fechaCupon)
        If ddlTasaCupon.SelectedValue() = 1 Then
            dcValorCupon = ((Decimal.Parse(tbRendimiento.Text) / 100) * Decimal.Parse(tbPrincipalAjustado.Text)) / (IIf(ddlTasaCupon.SelectedValue = 2,
            Decimal.Parse(tbBaseDias.Text) / 180, Decimal.Parse(tbBaseDias.Text) / diasEntreFechas))
        Else
            dcValorCupon = Math.Pow((1 + Decimal.Parse(tbRendimiento.Text)), diasEntreFechas / Decimal.Parse(tbBaseDias.Text))
        End If
        Return dcValorCupon
    End Function
    Private Function calcularValorCuponInicial(ByVal fechaCupon As String) As Decimal
        Dim dcValorCupon As Decimal
        Dim diasEntreFechas As Int32
        diasEntreFechas = DateDiff("d", UIUtility.ConvertirStringaFecha(lblFechaEmision.Text), UIUtility.ConvertirStringaFecha(fechaCupon))
        If ddlTasaCupon.SelectedValue() = 1 Then
            If (diasEntreFechas = 0) Then
                dcValorCupon = 0
            Else
                dcValorCupon = (Decimal.Parse(tbRendimiento.Text) * diasEntreFechas) / (Decimal.Parse(tbBaseDias.Text) * Decimal.Parse(tbPrincipalAjustado.Text))
            End If
        Else
            dcValorCupon = ((Math.Pow((1 + Decimal.Parse(tbRendimiento.Text)), diasEntreFechas / Decimal.Parse(tbBaseDias.Text))) - 1) *
            Decimal.Parse(tbPrincipalAjustado.Text)
        End If
        Return dcValorCupon
    End Function
    Private Function calcularValorTIR(ByVal indice As Int32, ByVal fechaCupon As String) As Decimal
        Dim diasEntreFechaCuponyCompra As Int32
        Dim diasEntreFechaEmision As Int32
        Dim dcValorTIR As Decimal
        diasEntreFechaCuponyCompra = DateDiff("d", UIUtility.ConvertirStringaFecha(tbFechaCompra.Text), UIUtility.ConvertirStringaFecha(fechaCupon))
        diasEntreFechaEmision = obtenerdiferenciaFechaEmisionCupon(indice, fechaCupon)
        If diasEntreFechaCuponyCompra < 0 Then
            dcValorTIR = Decimal.Parse(0)
        ElseIf ddlParaTIR.SelectedValue = 1 Then
            If ddlTreasuries.SelectedValue = 1 Then
                dcValorTIR = (Math.Pow((1 + (Decimal.Parse(tbRendimientoNegociacion.Text) / 100)), (Dias360(UIUtility.ConvertirStringaFecha(tbFechaCompra.Text),
                UIUtility.ConvertirStringaFecha(fechaCupon)) / Decimal.Parse(tbBaseTIR.Text))) - 1) * 100
            Else
                dcValorTIR = (1 + ((Decimal.Parse(tbRendimientoNegociacion.Text) / 100) / 2) * (diasEntreFechaCuponyCompra / diasEntreFechaEmision))
            End If
        Else
            dcValorTIR = Math.Pow((1 + (Decimal.Parse(tbRendimientoNegociacion.Text) / 100)), (Dias360(UIUtility.ConvertirStringaFecha(tbFechaCompra.Text),
            UIUtility.ConvertirStringaFecha(fechaCupon)) / Decimal.Parse(tbBaseTIR.Text))) - 1
        End If
        Return dcValorTIR
    End Function
    Private Function calcularValorVA(ByVal dcValorCupon As Decimal, ByVal dcValorTIR As Decimal) As Decimal
        Dim dcValorVA As Decimal
        If dcValorTIR = Decimal.Parse(0) Then
            dcValorVA = Decimal.Parse(0)
        ElseIf ddlTreasuries.SelectedValue = 1 Then
            dcValorVA = dcValorCupon / (1 + (dcValorTIR / 100))
        Else
            dcValorVA = dcValorCupon / (dcValorTIR / 100)
        End If
        Return dcValorVA
    End Function
    Private Function calcularDias(ByVal fechaCupon As String) As Int32
        Dim fechaActual As Date = UIUtility.ConvertirStringaFecha(fechaCupon)
        Dim fechaCompra As Date = UIUtility.ConvertirStringaFecha(tbFechaCompra.Text)
        Return DateDiff("d", fechaCompra, fechaActual)
    End Function
    Private Function calcularDiasxVA(ByVal dias As Integer, ByVal VA As Decimal) As Decimal
        Dim dcValor As Decimal
        If dias < 0 Then
            dcValor = Decimal.Parse(0)
        Else
            dcValor = dias * VA
        End If
        Return dcValor
    End Function
    Private Function obtenerdiferenciaFechaEmisionCupon(ByVal indice As Int32, ByVal fechaCupon As String) As Int32
        Dim fechaActual As Date = UIUtility.ConvertirStringaFecha(fechaCupon)
        Dim fechaAnterior As Date
        If indice = 0 Then
            fechaAnterior = UIUtility.ConvertirStringaFecha(lblFechaEmision.Text)
        Else
            fechaAnterior = UIUtility.ConvertirStringaFecha(dtValorizador.Rows(indice - 1)("Fecha"))
        End If
        Return (DateDiff("m", fechaAnterior, fechaActual) * 30)
    End Function
    Public Function Dias360(ByVal FechaDesde As Date, ByVal FechaHasta As Date) As Long
        Dim lngMeses As Long
        Dim lngDias As Long
        lngDias = Day(FechaHasta) - Day(FechaDesde)
        lngMeses = DateDiff("m", FechaDesde, FechaHasta)
        Dias360 = lngMeses * 30 + lngDias
    End Function
    Public Sub obtenerFechaUltimoCupon()
        Dim fechaCompra As Date = UIUtility.ConvertirStringaFecha(tbFechaCompra.Text)
        Dim fechaCuponActual As Date
        Dim fechaUltimoCupon As String = ""
        Dim valorUltimoCupon As String = ""
        Dim i As Int32
        For i = 0 To dtValorizador.Rows.Count - 1
            fechaCuponActual = UIUtility.ConvertirStringaFecha(dtValorizador.Rows(i)("Fecha"))
            If DateDiff("d", fechaCuponActual, fechaCompra) < 0 Then
                If i > 0 Then
                    fechaUltimoCupon = dtValorizador.Rows(i - 1)("Fecha")
                    valorUltimoCupon = dtValorizador.Rows(i - 1)("Cupon")
                End If
                Exit For
            End If
        Next
        lblUltimoCupon.Text = fechaUltimoCupon
        hdnUltimoCuponValor.Value = valorUltimoCupon
    End Sub
    Public Sub obtenerFechaSiguienteCupon()
        Dim fechaCompra As Date = UIUtility.ConvertirStringaFecha(tbFechaCompra.Text)
        Dim fechaCuponActual As Date
        Dim fechaSiguienteCupon As String = ""
        Dim valorSiguienteCupon As String = ""
        Dim i As Int32
        For i = dtValorizador.Rows.Count - 1 To 0 Step -1
            fechaCuponActual = UIUtility.ConvertirStringaFecha(dtValorizador.Rows(i)("Fecha"))
            If DateDiff("d", fechaCuponActual, fechaCompra) > 0 Then
                If i < dtValorizador.Rows.Count - 1 Then
                    fechaSiguienteCupon = dtValorizador.Rows(i + 1)("Fecha")
                    valorSiguienteCupon = dtValorizador.Rows(i + 1)("Cupon")
                End If

                Exit For
            End If
        Next
        lblSiguienteCupon.Text = fechaSiguienteCupon
        hdnSiguienteCuponValor.Value = valorSiguienteCupon
    End Sub
    Public Sub calcularInteresesTotales()
        Dim dtFechaCompra, dtFechaUltimoCupon, dtFechaSiguienteCupon As Date
        Dim diasEntreFechaCompraYUltimoCupon, diasEntreSiguienteYUltimoCupon As Integer
        Dim dcInteresesTotales As Decimal
        dtFechaCompra = UIUtility.ConvertirStringaFecha(tbFechaCompra.Text)
        dtFechaUltimoCupon = UIUtility.ConvertirStringaFecha(lblUltimoCupon.Text)
        dtFechaSiguienteCupon = UIUtility.ConvertirStringaFecha(lblSiguienteCupon.Text)
        diasEntreFechaCompraYUltimoCupon = Dias360(dtFechaUltimoCupon, dtFechaCompra)
        diasEntreSiguienteYUltimoCupon = Dias360(dtFechaUltimoCupon, dtFechaSiguienteCupon)
        If ddlBaseCupon.SelectedValue = 1 Then
            dcInteresesTotales = Decimal.Parse(hdnSiguienteCuponValor.Value) * (IIf(ddlParaTIR.SelectedValue = 1, (diasEntreFechaCompraYUltimoCupon /
            diasEntreSiguienteYUltimoCupon), (Dias360(dtFechaUltimoCupon, dtFechaCompra) / Dias360(dtFechaUltimoCupon, dtFechaSiguienteCupon))))
        Else
            dcInteresesTotales = (Math.Pow(1 + Decimal.Parse(tbRendimiento.Text), diasEntreFechaCompraYUltimoCupon / Decimal.Parse(tbBaseDias.Text)) - 1) *
            Decimal.Parse(tbPrincipalAjustado.Text)
        End If
        tbMasIntereses.Text = Format(dcInteresesTotales, "##,##0.00")
    End Sub
    Public Sub calcularVATotal()
        Dim i As Int32
        Dim dcVATotal As Decimal
        For i = 0 To dtValorizador.Rows.Count - 1
            dcVATotal += Decimal.Parse(dtValorizador.Rows(i)("VA"))
        Next
        tbValorActual.Text = Format(dcVATotal - Decimal.Parse(tbMasIntereses.Text), "##,##0.00")
        tbValorTotal.Text = Format(Decimal.Parse(tbValorActual.Text) + Decimal.Parse(tbMasIntereses.Text), "##,##0.00")
    End Sub
#End Region
    Private Sub btnexportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        CreaDatos()
        EjecutarJS(UIUtility.MostrarPopUp("frmHiperValorizador_rep.aspx", "no", 1000, 1000, 10, 10, "no", "no", "yes", "yes"), False)
    End Sub
    Private Sub CreaDatos()
        Dim dtHiperValorizar As New DataTable
        Dim dtHiperValorizar_Rep As New DataTable
        Dim drValorizador As DataRow
        Dim i As Integer
        dtHiperValorizar = CType(Session("HValorizador"), DataTable)
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Nemonico", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Principal", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("PrincipalAjustado", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Rendimiento", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("BaseDias", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("BaseTIR", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("VacEmision", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("VacActual", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("UltimoCupon", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("SiguienteCupon", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("FechaCompra", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Precio", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("RendimientoNeg", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Treasuries", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("BaseCupon", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("TasaCupon", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("ParaTir", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("ValorAjustado", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("FechaEmisionNeg", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Evento_d", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Fecha_d", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Cupon_d", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Tir_d", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Va_d", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Dias_d", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Dias_Acum_d", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("AmortAcum_d", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("ValorActual_t", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Interes_t", GetType(String)))
        dtHiperValorizar_Rep.Columns.Add(New DataColumn("Valor_Total_t", GetType(String)))
        For i = 0 To dtHiperValorizar.Rows.Count - 1
            drValorizador = dtHiperValorizar_Rep.NewRow()
            drValorizador("Nemonico") = lblCodigoNemonico.Text
            drValorizador("Principal") = tbPrincipal.Text
            drValorizador("PrincipalAjustado") = tbPrincipalAjustado.Text
            drValorizador("Rendimiento") = tbRendimiento.Text
            drValorizador("BaseDias") = tbBaseDias.Text
            drValorizador("BaseTIR") = tbBaseTIR.Text
            drValorizador("VacEmision") = tbVACEmision.Text
            drValorizador("VacActual") = tbVACActual.Text
            drValorizador("UltimoCupon") = lblUltimoCupon.Text
            drValorizador("SiguienteCupon") = lblSiguienteCupon.Text
            drValorizador("FechaCompra") = tbFechaCompra.Text
            drValorizador("Precio") = tbPrecio.Text
            drValorizador("RendimientoNeg") = tbRendimientoNegociacion.Text
            drValorizador("Treasuries") = ddlTreasuries.SelectedItem.Text
            drValorizador("BaseCupon") = ddlBaseCupon.SelectedItem.Text
            drValorizador("TasaCupon") = ddlTasaCupon.SelectedItem.Text
            drValorizador("ParaTir") = ddlParaTIR.SelectedItem.Text
            drValorizador("ValorAjustado") = ddlValoresAjustados.SelectedItem.Text
            drValorizador("FechaEmisionNeg") = lblFechaEmision.Text
            drValorizador("Evento_d") = dtHiperValorizar.Rows(i)("Evento")
            drValorizador("Fecha_d") = dtHiperValorizar.Rows(i)("Fecha")
            drValorizador("Cupon_d") = Format(dtHiperValorizar.Rows(i)("Cupon"), "##,##0.00")
            drValorizador("Tir_d") = Format(dtHiperValorizar.Rows(i)("TIR"), "##,##0.00")
            drValorizador("Va_d") = Format(dtHiperValorizar.Rows(i)("VA"), "##,##0.00")
            drValorizador("Dias_d") = dtHiperValorizar.Rows(i)("Dias")
            drValorizador("Dias_Acum_d") = ""
            drValorizador("AmortAcum_d") = Format(dtHiperValorizar.Rows(i)("AmortAcum"), "##,##0.00")
            drValorizador("ValorActual_t") = Format(Convert.ToDouble(tbValorActual.Text.ToString), "##,##0.00")
            drValorizador("Interes_t") = Format(Convert.ToDouble(tbMasIntereses.Text.ToString), "##,##0.00")
            drValorizador("Valor_Total_t") = Format(Convert.ToDouble(tbValorTotal.Text.ToString), "##,##0.00")
            dtHiperValorizar_Rep.Rows.Add(drValorizador)
        Next
        Session("tabla") = dtHiperValorizar_Rep
    End Sub
    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            Procesar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Sistema")
        End Try
    End Sub
    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            crearGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Sistema")
        End Try
    End Sub
End Class

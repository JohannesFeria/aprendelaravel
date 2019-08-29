Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports Sit.BusinessLayer.MotorInversiones.Utiles

Partial Class Modulos_Inversiones_InstrumentosNegociados_frmSwapPorcentaje
    Inherits BasePage
    Dim oCuponera As New CuponeraBM
    Dim oPortafolioBM As New PortafolioBM
    Dim oPeriodicidadBM As New PeriodicidadBM
    Dim oTipoAmortizacionBM As New TipoAmortizacionBM
    Dim oValoresBM As New ValoresBM
    Dim objutil As New UtilDM
    Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
    Dim oOrdenInversion_DetalleSWAPBE As New OrdenInversion_DetalleSWAPBE
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
    Dim oIndicadorBM As New IndicadorBM
    Dim oCotizacionVACBM As New CotizacionVACBM
    Private Sub ReturnArgumentShowDialogPopup()
        Select Case hdPagina.Value
            Case "CO"
                AlertaJS("Se confirmó la orden correctamente", "window.close()")
                'Case "EO"
                '    AlertaJS("Se Ejecutó la orden correctamente", "window.close()")
                'Case "XO"
                '    AlertaJS("Se Extornó la orden correctamente", "window.close()")
                'Case "OE"
                '    EjecutarJS("window.close()")
                'Case "MODIFICA"
                '    AlertaJS("Se Modificó la orden correctamente", "window.close()")
        End Select
    End Sub
    Function ValidadAmoritacion() As Boolean
        Dim Amortizacion As Decimal = 0
        Dim DT As DataTable = DirectCast(ViewState("dtCuponera"), DataTable)
        For Each DTR As DataRow In DT.Rows
            Amortizacion += CDec(DTR("Amortizac"))
        Next
        If Amortizacion > 100 Or Amortizacion < 100 Then
            Return False
        Else
            Return True
        End If
    End Function
    Sub Ingresar()
        txtMontoNominal.Text = "0.0000000"
        CargarPaginaAccion(True)
        ViewState("EstadoPantalla") = "Ingresar"
        If (hdPagina.Value <> "DA") Then
            tbFechaOperacion.Text = objutil.RetornarFechaNegocio
        Else
            tbFechaOperacion.Text = Request.QueryString("Fecha")
        End If
        lblAccion.Text = "Ingresar"
        Botones()
        TipoCuponera(ddlTipoCuponera.SelectedValue, True)
    End Sub
    Sub Modificar(ByVal CodigoOrden As String, ByVal CodigoPortafolioSBS As String)

        Try
            Dim dsOISWAP As DataSet = oOrdenInversionBM.ConsultaOrdenSwapBono(CodigoPortafolioSBS, 0, CodigoOrden)
            If dsOISWAP.Tables.Count > 0 Then
                ViewState("EstadoPantalla") = "Modificar"
                CargarPaginaAccion(True)
                Dim dtOICab As DataTable = dsOISWAP.Tables(0)
                If dtOICab.Rows.Count > 0 Then
                    hdCodigoOrden.Value = CodigoOrden
                    ddlFondo.SelectedValue = dtOICab(0)("CodigoPortafolioSBS")
                    ddlTipoCuponera.SelectedValue = dtOICab(0)("CodigoTipoCupon")
                    TipoCuponera(ddlTipoCuponera.SelectedValue, False)
                    tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(CDec(dtOICab(0)("FechaOperacion")))
                    txtISINOperacion.Text = dtOICab(0)("CodigoISIN")
                    ddlFormaCambio.SelectedValue = dtOICab(0)("TipoCobertura")
                    txtMontoNominalOriginal.Text = dtOICab(0)("MontoNominalOperacion")
                    txtMontoNominal.Text = dtOICab(0)("MontoNetoOperacion")
                    hdMoneda.Value = dtOICab(0)("CodigoMoneda")
                    ddlTipoOperacion.SelectedValue = dtOICab(0)("TipoFondo")
                    ddlGrupoInt.SelectedValue = dtOICab(0)("GrupoIntermediario")
                    ddlIntermediario.SelectedValue = dtOICab(0)("CodigoTercero")
                Else
                    AlertaJS("No existe información de la cabecera de la órden seleccionada.")
                    Exit Sub
                End If
                Dim dtOIDet As DataTable = dsOISWAP.Tables(1)
                If dtOIDet.Rows.Count > 0 Then
                    txtfecIniCuponOrigen.Text = UIUtility.ConvertirFechaaString(dtOIDet(0)("FechaIniLeg1"))
                    txtfecFinCuponOrigen.Text = UIUtility.ConvertirFechaaString(dtOIDet(0)("FechaFinLeg1"))
                    txtfecIniCupon.Text = UIUtility.ConvertirFechaaString(dtOIDet(0)("FechaIniLeg2"))
                    txtfecFinCupon.Text = UIUtility.ConvertirFechaaString(dtOIDet(0)("FechaFinLeg2"))
                    ddlMonedaLeg1.SelectedValue = dtOIDet(0)("CodigoMonedaLeg1")
                    ddlMonedaLeg2.SelectedValue = dtOIDet(0)("CodigoMonedaLeg2")
                    txtTipoCambio.Text = dtOIDet(0)("TipoCambioSpot").ToString
                    txtTasaOriginal.Text = dtOIDet(0)("TasaInteresLeg1").ToString
                    txtTasa.Text = dtOIDet(0)("TasaInteresLeg2").ToString
                    ddlTasaInteresVariableOrigen.SelectedValue = dtOIDet(0)("TasaFlotanteLeg1")
                    ddlTasaInteresVariable.SelectedValue = dtOIDet(0)("TasaFlotanteLeg2")
                    ddlPeriodicidadOriginal.SelectedValue = dtOIDet(0)("PeriodicidadLeg1")
                    ddlPeriodicidad.SelectedValue = dtOIDet(0)("PeriodicidadLeg2")
                    ddlAmortizacionOriginal.SelectedValue = dtOIDet(0)("AmortizacionLeg1")
                    ddlAmortizacion.SelectedValue = dtOIDet(0)("AmortizacionLeg2")
                    ddlBaseDiasOrigen.SelectedValue = dtOIDet(0)("BaseDiasLeg1")
                    ddlBaseMesOrigen.SelectedValue = dtOIDet(0)("BaseAniosLeg1")
                    ddlBaseDias.SelectedValue = dtOIDet(0)("BaseDiasLeg2")
                    ddlBaseMes.SelectedValue = dtOIDet(0)("BaseAniosLeg2")

                    pnlTasaLiborOrigen.Visible = If(ddlTasaInteresVariableOrigen.SelectedValue <> String.Empty, True, False)
                    pnlTasaLibor.Visible = If(ddlTasaInteresVariable.SelectedValue <> String.Empty, True, False)


                    ddlTasaLiborOrigen.SelectedValue = If(ddlTasaInteresVariableOrigen.SelectedValue <> String.Empty, Convert.ToString(dtOIDet(0)("DiaTLeg1")), String.Empty)
                    ddlTasaLibor.SelectedValue = If(ddlTasaInteresVariable.SelectedValue <> String.Empty, Convert.ToString(dtOIDet(0)("DiaTLeg2")), String.Empty)
                    txtTasaLiborOrigen.Text = dtOIDet(0)("TasaLiborLeg1").ToString
                    txtTasaLibor.Text = dtOIDet(0)("TasaLiborLeg2").ToString

                  

            Else
                AlertaJS("No existe información del detalle de la órden seleccionada.")
                Exit Sub
            End If
            If ddlTipoCuponera.SelectedValue = "1" Then
                Dim dtBonoSwap As DataTable = oOrdenInversionBM.ConsultaBono_Swap(CodigoOrden)
                If dtBonoSwap.Rows.Count > 0 Then
                    ViewState("dtBonos") = dtBonoSwap
                    gvBonos.DataSource = dtBonoSwap
                    gvBonos.DataBind()
                Else
                    AlertaJS("No existe información de los bonos negociados en la órden seleccionada.")
                    Exit Sub
                End If
            End If
            Dim dtCuponeraBonoSwap As DataTable = oOrdenInversionBM.ConsultaCuponera_Bono_Swap(CodigoOrden, String.Empty)
                If dtCuponeraBonoSwap.Rows.Count > 0 Then
                    habilitarColumnaFechaLiborOrigen(ddlTasaInteresVariableOrigen.SelectedValue)
                    habilitarColumnaFechaLibor(ddlTasaInteresVariable.SelectedValue)
                    ViewState("dtCuponera") = dtCuponeraBonoSwap
                    dgLista.DataSource = dtCuponeraBonoSwap
                    dgLista.DataBind()

                Else
                    AlertaJS("No existe información de la cuponera generada en la órden seleccionada.")
                    Exit Sub
                End If
            Else
            AlertaJS("No existe información de la órden seleccionada.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Sub Limpiar()
        CargarPaginaAccion(False)
        ViewState("EstadoPantalla") = ""
        BotonesCancelar()
        dgLista.DataSource = Nothing
        dgLista.DataBind()
        gvBonos.DataSource = Nothing
        gvBonos.DataBind()
        ViewState("dtCuponera") = Nothing
        ViewState("dtBonos") = Nothing
    End Sub
    Public Function crearObjetoOI_detalleSWAP(ByVal CodigoOrden As String) As OrdenInversion_DetalleSWAPBE
        oOrdenInversionBM.InicializarOrdenInversion_DetalleSWAP(oOrdenInversion_DetalleSWAPBE)
        oOrdenInversion_DetalleSWAPBE.CodigoOrden = CodigoOrden
        oOrdenInversion_DetalleSWAPBE.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oOrdenInversion_DetalleSWAPBE.FechaIniLeg1 = UIUtility.ConvertirFechaaDecimal(txtfecIniCuponOrigen.Text)
        oOrdenInversion_DetalleSWAPBE.FechaFinLeg1 = UIUtility.ConvertirFechaaDecimal(txtfecFinCuponOrigen.Text)
        oOrdenInversion_DetalleSWAPBE.FechaIniLeg2 = UIUtility.ConvertirFechaaDecimal(txtfecIniCupon.Text)
        oOrdenInversion_DetalleSWAPBE.FechaFinLeg2 = UIUtility.ConvertirFechaaDecimal(txtfecFinCupon.Text)
        oOrdenInversion_DetalleSWAPBE.CodigoMonedaLeg1 = ddlMonedaLeg1.SelectedValue
        oOrdenInversion_DetalleSWAPBE.CodigoMonedaLeg2 = ddlMonedaLeg2.SelectedValue
        oOrdenInversion_DetalleSWAPBE.TipoCambioSpot = CDec(IIf(txtTipoCambio.Text.Trim = String.Empty, "0", txtTipoCambio.Text.Trim))
        oOrdenInversion_DetalleSWAPBE.TasaInteresLeg1 = CDec(IIf(txtTasaOriginal.Text.Trim = String.Empty, "0", txtTasaOriginal.Text.Trim))
        oOrdenInversion_DetalleSWAPBE.TasaFlotanteLeg1 = ddlTasaInteresVariableOrigen.SelectedValue
        oOrdenInversion_DetalleSWAPBE.TasaInteresLeg2 = CDec(IIf(txtTasa.Text.Trim = String.Empty, "0", txtTasa.Text.Trim))
        oOrdenInversion_DetalleSWAPBE.TasaFlotanteLeg2 = ddlTasaInteresVariable.SelectedValue
        oOrdenInversion_DetalleSWAPBE.PeriodicidadLeg1 = ddlPeriodicidadOriginal.SelectedValue
        oOrdenInversion_DetalleSWAPBE.PeriodicidadLeg2 = ddlPeriodicidad.SelectedValue
        oOrdenInversion_DetalleSWAPBE.AmortizacionLeg1 = ddlAmortizacionOriginal.SelectedValue
        oOrdenInversion_DetalleSWAPBE.AmortizacionLeg2 = ddlAmortizacion.SelectedValue
        oOrdenInversion_DetalleSWAPBE.BaseDiasLeg1 = ddlBaseDiasOrigen.SelectedValue
        oOrdenInversion_DetalleSWAPBE.BaseAniosLeg1 = ddlBaseMesOrigen.SelectedValue
        oOrdenInversion_DetalleSWAPBE.BaseDiasLeg2 = ddlBaseDias.SelectedValue
        oOrdenInversion_DetalleSWAPBE.BaseAniosLeg2 = ddlBaseMes.SelectedValue


        oOrdenInversion_DetalleSWAPBE.DiaTLeg1 = If(ddlTasaInteresVariableOrigen.SelectedValue = String.Empty, 1, CInt(ddlTasaLiborOrigen.SelectedValue))
        oOrdenInversion_DetalleSWAPBE.DiaTLeg2 = If(ddlTasaInteresVariable.SelectedValue = String.Empty, 1, CInt(ddlTasaLibor.SelectedValue))
        oOrdenInversion_DetalleSWAPBE.TasaLiborLeg1 = CDec(IIf(txtTasaLiborOrigen.Text.Trim = String.Empty, "0", txtTasaLiborOrigen.Text.Trim))
        oOrdenInversion_DetalleSWAPBE.TasaLiborLeg2 = CDec(IIf(txtTasaLibor.Text.Trim = String.Empty, "0", txtTasaLibor.Text.Trim))

        Return oOrdenInversion_DetalleSWAPBE
    End Function
    Public Function crearObjetoOI() As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        oOrdenInversionBM.InicializarOrdenInversion(oRow)
        oRow.CodigoOrden = hdCodigoOrden.Value.Trim
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoOperacion = "21"
        oRow.CodigoISIN = txtISINOperacion.Text
        oRow.CodigoMnemonico = "SWAP"
        oRow.CodigoSBS = String.Empty
        oRow.GrupoIntermediario = ddlGrupoInt.SelectedValue
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = String.Empty
        oRow.TipoCambio = txtTipoCambio.Text
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.CantidadOrdenado = 0
        oRow.CantidadOperacion = 0
        oRow.TipoCobertura = ddlFormaCambio.SelectedValue
        oRow.CodigoTipoCupon = ddlTipoCuponera.SelectedValue
        oRow.MontoOperacion = CDec(txtMontoNominalOriginal.Text)
        oRow.Precio = 0
        oRow.TotalComisiones = 0
        oRow.PrecioPromedio = 0
        oRow.MontoNetoOperacion = CDec(txtMontoNominal.Text)
        oRow.Situacion = "A"
        oRow.Observacion = String.Empty
        oRow.HoraOperacion = objutil.RetornarHoraSistema
        oRow.CategoriaInstrumento = "BS"
        oRow.Plazo = IIf(ddlBaseDiasOrigen.SelectedValue = "30", Dias360(UIUtility.ConvertirStringaFecha(txtfecIniCuponOrigen.Text), UIUtility.ConvertirStringaFecha(txtfecFinCuponOrigen.Text)), _
                                                                 DiasACT(UIUtility.ConvertirStringaFecha(txtfecIniCuponOrigen.Text), UIUtility.ConvertirStringaFecha(txtfecFinCuponOrigen.Text)))
        oRow.CodigoMoneda = ddlMonedaLeg1.SelectedValue
        oRow.CodigoMonedaOrigen = ddlMonedaLeg1.SelectedValue
        oRow.CodigoMonedaDestino = ddlMonedaLeg2.SelectedValue
        oRow.Estado = "E-EJE"
        oRow.NumeroPoliza = String.Empty
        oRow.MontoDestino = 0
        oRow.CodigoMotivo = ddlPeriodicidad.SelectedValue
        oRow.Plaza = ddlAmortizacion.SelectedValue
        oRow.IndicaCambio = "1"
        oRow.EventoFuturo = 1
        oRow.TipoTramo = String.Empty
        oRow.Ficticia = "N"
        oRow.MontoNominalOperacion = CDec(txtMontoNominalOriginal.Text)
        oRow.RegulaSBS = "N"
        oRow.MedioNegociacion = String.Empty
        oRow.HoraEjecucion = objutil.RetornarHoraSistema
        oRow.TasaPorcentaje = txtTasa.Text
        oRow.TasaCastigo = txtTasaOriginal.Text
        oRow.TipoFondo = ddlTipoOperacion.SelectedValue
        If txtfecIniCupon.Text = String.Empty Then
            oRow.FechaTrato = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        Else
            oRow.FechaTrato = UIUtility.ConvertirFechaaDecimal(txtfecIniCupon.Text)
        End If
        If txtfecFinCupon.Text = String.Empty Then
            oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        Else
            oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(txtfecFinCupon.Text)
        End If
        oRow.TipoCambioSpot = txtTipoCambio.Text

        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
    Sub LimpiaIsin()
        txtISIN.Text = String.Empty
        txtMnemonico.Text = String.Empty
        txtUnidades.Text = String.Empty
    End Sub
    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal cfondo As String, ByVal operacion As String, ByVal categoria As String, ByVal valor As String)
        Dim strURL As String = "frmBuscarValor.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&cFondo=" & cfondo & "&vFondo=" & fondo & "&vOperacion=" & operacion & "&vCategoria=" & categoria
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "');")
    End Sub
    Sub ObtenerValoresDesdePopup()
        Dim datosModal As String() = CType(Session("SS_DatosModal"), String())
        txtISIN.Text = datosModal(0)
        txtMnemonico.Text = datosModal(1)
        '  txtSBS.Text = datosModal(2)
        txtUnidades.Text = datosModal(4)
        ViewState("totalUnidades") = datosModal(4)
        Session.Remove("SS_DatosModal")
        HabilitarControlesBusqueda(False)
    End Sub
    Public Sub CargarIntermediario()
        UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Session("SS_DatosModal") IsNot Nothing Then ObtenerValoresDesdePopup()
        If Not Page.IsPostBack Then
            ViewState("Load") = "0"
            hdRptaConfirmar.Value = "NO"
            CargarDataInicial()
            CargarIntermediario()
            btnProcesar.Enabled = True
            btnRetornar.Enabled = True
            btnAgregar.Enabled = False
            hdPagina.Value = Request.QueryString("PTNeg")
            If Request.QueryString("cod") = String.Empty Then
                Ingresar()
            Else
                If hdPagina.Value <> "CO" Then
                    Modificar(Request.QueryString("cod"), Request.QueryString("portafolio"))
                Else
                    Modificar(Request.QueryString("cod"), CType(Session("codigoPortafolioSBS_CO"), String))
                End If
                ddlFondo.Enabled = False
                ddlTipoCuponera.Enabled = False
                ddlTipoOperacion.Enabled = False
                If hdPagina.Value = "CO" Then
                    btnAceptar.Text = "Grabar y Confirmar"
                    If Session("ValidarFecha").ToString = "FECHADIFERENTE" Then
                        CargarPaginaAccion(False)
                        btnProcesar.Enabled = False
                        btnAceptar.Enabled = False
                    End If
                End If
                If Request.QueryString("estado") = "E-ELI" Or Request.QueryString("estado") = "E-CON" Then
                    btnProcesar.Enabled = False
                    btnAceptar.Enabled = False
                    If (UIUtility.ConvertirFechaaDecimal(txtfecFinCuponOrigen.Text) >= UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) Or _
                       UIUtility.ConvertirFechaaDecimal(txtfecFinCupon.Text) >= UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)) _
                       And hdPagina.Value = "CO" Then
                        btnAceptar.Enabled = True
                    End If
                    CargarPaginaAccion(False)
                Else
                    btnAceptar.Enabled = True
                    '      btnAceptar.Attributes.Add("onclick", "this.disabled = true; this.value = 'en proceso...'; __doPostBack('btnAceptar','');")
            End If
                If hdPagina.Value <> "CO" Then
                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF2", "Nro " + Request.QueryString("cod").ToString + "?", "SI")
                Else
                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF49", "Nro " + Request.QueryString("cod").ToString + "?", "SI")
                End If

            End If
        End If
    End Sub
    Private Sub CargarPaginaAccion(Estado As Boolean)
        ddlTipoCuponera.Enabled = Estado
        ddlFondo.Enabled = Estado
        btnBuscar.Enabled = Estado
        txtISIN.Enabled = Estado
        txtMnemonico.Enabled = Estado
        txtUnidades.Enabled = Estado
        txtTasa.Enabled = Estado
        txtTipoCambio.Enabled = Estado
        ddlFormaCambio.Enabled = Estado
        ddlPeriodicidad.Enabled = Estado
        ddlPeriodicidadOriginal.Enabled = Estado
        ddlAmortizacion.Enabled = Estado
        ddlBaseDias.Enabled = Estado
        ddlBaseMes.Enabled = Estado
        ddlBaseDiasOrigen.Enabled = Estado
        ddlBaseMesOrigen.Enabled = Estado
        txtISINOperacion.Enabled = Estado
        ddlTipoOperacion.Enabled = Estado
        txtMontoNominalOriginal.Enabled = Estado
        ddlMonedaLeg1.Enabled = Estado
        ddlMonedaLeg2.Enabled = Estado
        txtfecIniCuponOrigen.Enabled = Estado
        txtfecIniCupon.Enabled = Estado
        txtfecFinCuponOrigen.Enabled = Estado
        txtfecFinCupon.Enabled = Estado
        txtTasaOriginal.Enabled = Estado
        ddlTasaInteresVariableOrigen.Enabled = Estado
        ddlTasaInteresVariable.Enabled = Estado
        ddlAmortizacionOriginal.Enabled = Estado
        ddlGrupoInt.Enabled = Estado
        ddlIntermediario.Enabled = Estado
        btnBuscar.Enabled = Estado

        ddlTasaLibor.Enabled = Estado
        ddlTasaLiborOrigen.Enabled = Estado
        txtTasaLibor.Enabled = Estado
        txtTasaLiborOrigen.Enabled = Estado



        If Estado Then
            Div1.Attributes.Add("class", "input-append date")
            Div2.Attributes.Add("class", "input-append date")
            Div3.Attributes.Add("class", "input-append date")
            Div4.Attributes.Add("class", "input-append date")
        Else
            Div1.Attributes.Add("class", "input-append")
            Div2.Attributes.Add("class", "input-append")
            Div3.Attributes.Add("class", "input-append")
            Div4.Attributes.Add("class", "input-append")
        End If
    End Sub
    Sub Botones()
        btnRetornar.Enabled = True
    End Sub
    Sub BotonesCancelar()
        btnRetornar.Enabled = False
        btnAceptar.Enabled = False
    End Sub
    Private Sub ReiniciarColoresCamposObligatorios()
        EjecutarJS("ResaltarColor('ddlFondo', false);")
        EjecutarJS("ResaltarColor('ddlGrupoInt', false);")
        EjecutarJS("ResaltarColor('ddlIntermediario',false);")
        EjecutarJS("ResaltarColor('ddlPeriodicidad', false);")
        EjecutarJS("ResaltarColor('ddlPeriodicidadOriginal', false);")
        EjecutarJS("ResaltarColor('ddlMonedaLeg1', false);")
        EjecutarJS("ResaltarColor('ddlMonedaLeg2', false);")
        EjecutarJS("ResaltarColor('txtfecIniCuponOrigen', false);")
        EjecutarJS("ResaltarColor('txtfecIniCupon', false);")
        EjecutarJS("ResaltarColor('txtfecFinCuponOrigen', false);")
        EjecutarJS("ResaltarColor('txtfecFinCupon', false);")
        EjecutarJS("ResaltarColor('txtTipoCambio', false);")
        EjecutarJS("ResaltarColor('txtTasaOriginal', false);")
        EjecutarJS("ResaltarColor('txtTasa', false);")
        EjecutarJS("ResaltarColor('ddlAmortizacionOriginal', false);")
        EjecutarJS("ResaltarColor('ddlAmortizacion', false);")
        EjecutarJS("ResaltarColor('ddlBaseMesOrigen', false);")
        EjecutarJS("ResaltarColor('ddlBaseDiasOrigen', false);")
        EjecutarJS("ResaltarColor('ddlBaseMes', false);")
        EjecutarJS("ResaltarColor('ddlBaseDias', false);")
        EjecutarJS("ResaltarColor('txtMontoNominalOriginal', false);")
        EjecutarJS("ResaltarColor('txtISINOperacion', false);")
        EjecutarJS("ResaltarColor('txtTasaLiborOrigen', false);")
        EjecutarJS("ResaltarColor('txtTasaLibor', false);")
    End Sub
    Private Function ValidarCamposObligatorios() As String
        Dim mensaje As String = String.Empty
        ReiniciarColoresCamposObligatorios()
        If ddlFondo.SelectedValue = String.Empty Then EjecutarJS("ResaltarColor('ddlFondo', true);") : mensaje = "- Portafolio. <br />"
        If gvBonos.Rows.Count = 0 And ddlTipoCuponera.SelectedValue = "1" Then mensaje += "- Seleccione al menos un Bono para continuar. <br />"
        If ddlGrupoInt.SelectedValue = String.Empty Then EjecutarJS("ResaltarColor('ddlGrupoInt', true);") : mensaje += "- Grupo Intermediario.<br />"
        If ddlIntermediario.SelectedValue = String.Empty Then EjecutarJS("ResaltarColor('ddlIntermediario',true);") : mensaje += "- Intermediario.<br />"
        If ddlPeriodicidadOriginal.SelectedValue = String.Empty Then EjecutarJS("ResaltarColor('ddlPeriodicidadOriginal', true);") : mensaje += "- Periodicidad LEG1.<br />"
        If ddlPeriodicidad.SelectedValue = String.Empty Then EjecutarJS("ResaltarColor('ddlPeriodicidad', true);") : mensaje += "- Periodicidad LEG2.<br />"
        If ddlMonedaLeg1.SelectedValue = String.Empty Then EjecutarJS("ResaltarColor('ddlMonedaLeg1', true);") : mensaje += "- Moneda LEG1.<br />"
        If ddlMonedaLeg2.SelectedValue = String.Empty Then EjecutarJS("ResaltarColor('ddlMonedaLeg2', true);") : mensaje += "- Moneda LEG2.<br />"
        If txtfecIniCuponOrigen.Text = String.Empty Then EjecutarJS("ResaltarColor('txtfecIniCuponOrigen', true);") : mensaje += "- Fecha Inicio LEG1.<br />"
        If txtfecIniCupon.Text = String.Empty Then EjecutarJS("ResaltarColor('txtfecIniCupon', true);") : mensaje += "- Fecha Inicio LEG2.<br />"
        If txtfecFinCuponOrigen.Text = String.Empty Then EjecutarJS("ResaltarColor('txtfecFinCuponOrigen', true);") : mensaje += "- Fecha Fin LEG1.<br />"
        If txtfecFinCupon.Text = String.Empty Then EjecutarJS("ResaltarColor('txtfecFinCupon', true);") : mensaje += "- Fecha Fin LEG2.<br />"
        If (txtTipoCambio.Text = String.Empty Or CDec(IIf(IsNumeric(txtTipoCambio.Text), txtTipoCambio.Text, "0")) = 0) _
            And (ddlMonedaLeg1.SelectedValue <> ddlMonedaLeg2.SelectedValue) Then EjecutarJS("ResaltarColor('txtTipoCambio', true);") : mensaje += "- Tipo Cambio SPOT.<br />"
        If (txtTasaOriginal.Text = String.Empty Or CDec(IIf(IsNumeric(txtTasaOriginal.Text), txtTasaOriginal.Text, "0")) = 0) And ddlTipoCuponera.SelectedValue = "0" Then EjecutarJS("ResaltarColor('txtTasaOriginal', true);") : mensaje += "- Interes % LEG1.<br />"
        If txtTasa.Text = String.Empty Or CDec(IIf(IsNumeric(txtTasa.Text), txtTasa.Text, "0")) = 0 Then EjecutarJS("ResaltarColor('txtTasa', true);") : mensaje += "- Interes % LEG2.<br />"
        If ddlAmortizacionOriginal.SelectedValue = String.Empty And ddlTipoCuponera.SelectedValue = "0" Then EjecutarJS("ResaltarColor('ddlAmortizacionOriginal', true);") : mensaje += "- Amortización LEG1.<br />"
        If ddlAmortizacion.SelectedValue = String.Empty Then EjecutarJS("ResaltarColor('ddlAmortizacion', true);") : mensaje += "- Amortización LEG2.<br />"
        If ddlBaseDiasOrigen.SelectedValue = String.Empty Or ddlBaseMesOrigen.SelectedValue = String.Empty Then EjecutarJS("ResaltarColor('ddlBaseMesOrigen', true);") : EjecutarJS("ResaltarColor('ddlBaseDiasOrigen', true);") : mensaje += "- Base LEG1.<br />"
        If ddlBaseDias.SelectedValue = String.Empty Or ddlBaseMes.SelectedValue = String.Empty Then EjecutarJS("ResaltarColor('ddlBaseMes', true);") : EjecutarJS("ResaltarColor('ddlBaseDias', true);") : mensaje += "- Base LEG2.<br />"
        If txtMontoNominalOriginal.Text = String.Empty And ddlTipoCuponera.SelectedValue = "0" Then EjecutarJS("ResaltarColor('txtMontoNominalOriginal', true);") : mensaje += "- Nominal Inicial LEG1.<br />"

        'verificando Tasa Libor
        If ddlTasaInteresVariableOrigen.SelectedValue <> String.Empty Then
            If txtTasaLiborOrigen.Text = String.Empty Or CDec(IIf(IsNumeric(txtTasaLiborOrigen.Text), txtTasaLiborOrigen.Text, "0")) = 0 Then EjecutarJS("ResaltarColor('txtTasaLiborOrigen', true);") : mensaje += "- Tasa Libor % LEG1.<br />"
        End If


        If ddlTasaInteresVariable.SelectedValue <> String.Empty Then
            If txtTasaLibor.Text = String.Empty Or CDec(IIf(IsNumeric(txtTasaLibor.Text), txtTasaLibor.Text, "0")) = 0 Then EjecutarJS("ResaltarColor('txtTasaLibor', true);") : mensaje += "- Tasa Libor % LEG2.<br />"
        End If

       
        If mensaje.Length > 0 Then
            mensaje = "Los siguientes campos son obligatorios: <br /><p align=left>" + mensaje + "</p><br />"
        ElseIf UIUtility.ConvertirFechaaDecimal(txtfecIniCupon.Text) >= UIUtility.ConvertirFechaaDecimal(txtfecFinCupon.Text) Then
            EjecutarJS("ResaltarColor('txtfecIniCupon', true);")
            EjecutarJS("ResaltarColor('txtfecFinCupon', true);")
            mensaje += "- La Fecha de Inicio no puede ser mayor o igual a la fecha Fecha Fin de LEG2<br />"
        ElseIf UIUtility.ConvertirFechaaDecimal(txtfecIniCuponOrigen.Text) >= UIUtility.ConvertirFechaaDecimal(txtfecFinCuponOrigen.Text) Then
            EjecutarJS("ResaltarColor('txtfecIniCuponOrigen', true);")
            EjecutarJS("ResaltarColor('txtfecFinCuponOrigen', true);")
            mensaje += "- La Fecha de Inicio no puede ser mayor o igual a la fecha Fecha Fin de LEG1<br />"
        End If

        Return mensaje
    End Function
    Sub CargarFechaVencimiento()
        If Not ddlFondo.SelectedValue = String.Empty Then
            Dim dtAux As DataTable = oPortafolioBM.SeleccionarPortafolioPorFiltro(ddlFondo.SelectedValue, DatosRequest).Tables(0)
            hdMoneda.Value = dtAux.Rows(0)("CodigoMoneda")
            If dtAux.Rows.Count > 0 Then
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
            End If
        End If
    End Sub
    Protected Sub ddlFondo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If ddlFondo.SelectedValue <> String.Empty Then
            CargarFechaVencimiento()
        End If
        Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        If cantidadreg > 0 Then
            AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
            ddlFondo.SelectedIndex = 0
        End If
    End Sub
    Protected Sub btnRetornar_Click(sender As Object, e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("frmListarSwapPorcentaje.aspx")
    End Sub
    Protected Sub btnProcesar_Click(sender As Object, e As System.EventArgs) Handles btnProcesar.Click
        Try
            Dim CadenaNemonico As String = String.Empty, Cantidad As Integer = gvBonos.Rows.Count, i As Integer = 0, j As Integer = 0, DT As DataTable = Nothing
            Dim txtPrecioVentaTEMP As TextBox, importeVentaTotal As Decimal = 0, tasaFloatingOriginal As Decimal = 0, tasaFloating As Decimal = 0
            Dim mensajeCampoObligatorio = ValidarCamposObligatorios()
            If mensajeCampoObligatorio = String.Empty Then
                txtMontoNominal.Text = Math.Round((CDec(txtMontoNominalOriginal.Text) * CDec(txtTipoCambio.Text)), 7).ToString
                'If ddlTipoCuponera.SelectedValue = "1" Then
                For Each row As GridViewRow In gvBonos.Rows
                    i += 1
                    txtPrecioVentaTEMP = CType(row.FindControl("txtPrecioVenta"), TextBox)
                    CadenaNemonico = CadenaNemonico + row.Cells(3).Text + ","
                    txtPrecioVentaTEMP.BorderColor = Drawing.Color.Gray
                    importeVentaTotal += IIf(txtPrecioVentaTEMP.Text = String.Empty, "0", CStr(CDec(txtPrecioVentaTEMP.Text)))
                    If UIUtility.ConvertirFechaaDecimal(row.Cells(6).Text) > UIUtility.ConvertirFechaaDecimal(txtfecFinCuponOrigen.Text) And CDec(txtPrecioVentaTEMP.Text) = 0 Then
                        txtPrecioVentaTEMP.BorderColor = Drawing.Color.Red
                        j += 1
                    End If
                Next
                If j > 0 Then
                    AlertaJS("La fecha de vencimiento de uno de los bonos es mayor a la fecha fin del cupón SWAP, se necesita especificar el Importe Venta.")
                Else
                    'If ddlTasaInteresVariableOrigen.SelectedValue <> String.Empty Then tasaFloatingOriginal = ddlTasaInteresVariableOrigen.SelectedItem.ToString.Split("|")(1).Trim
                    ' If ddlTasaInteresVariable.SelectedValue <> String.Empty Then tasaFloating = ddlTasaInteresVariable.SelectedItem.ToString.Split("|")(1).Trim
                    If ddlTasaInteresVariableOrigen.SelectedValue <> String.Empty Then tasaFloatingOriginal = If(txtTasaLiborOrigen.Text = String.Empty, 0, CDec(txtTasaLiborOrigen.Text))
                    If ddlTasaInteresVariable.SelectedValue <> String.Empty Then tasaFloating = If(txtTasaLibor.Text = String.Empty, 0, CDec(txtTasaLibor.Text))


                    'Habilitar Columna FechaLibo
                    Dim valorDiaTOriginal As Decimal = 0
                    Dim valorDiaT As Decimal = 0
                    If ddlTasaInteresVariableOrigen.SelectedValue <> String.Empty And ddlTasaLiborOrigen.Enabled Then

                        valorDiaTOriginal = CDec(ddlTasaLiborOrigen.SelectedValue)
                    End If

                    If ddlTasaInteresVariable.SelectedValue <> String.Empty Then

                        valorDiaT = CDec(ddlTasaLibor.SelectedValue)
                    End If

                    DT = oCuponera.ObtenerCuponera(ddlAmortizacionOriginal.SelectedValue, _
                                                   UIUtility.ConvertirFechaaDecimal(txtfecIniCuponOrigen.Text), _
                                                   UIUtility.ConvertirFechaaDecimal(txtfecFinCuponOrigen.Text), _
                                                   UIUtility.ConvertirFechaaDecimal(txtfecIniCuponOrigen.Text), _
                                                   ddlPeriodicidadOriginal.Text, _
                                                   CDec(txtTasaOriginal.Text), _
                                                   ddlBaseMesOrigen.SelectedValue, _
                                                   tasaFloatingOriginal, _
                                                   ddlBaseDiasOrigen.SelectedValue, _
                                                   CDec(txtMontoNominalOriginal.Text), _
                                                   CadenaNemonico, _
                                                   importeVentaTotal, _
                                                   ddlTipoCuponera.SelectedValue, _
                                                   ddlAmortizacion.SelectedValue, _
                                                   UIUtility.ConvertirFechaaDecimal(txtfecIniCupon.Text), _
                                                   UIUtility.ConvertirFechaaDecimal(txtfecFinCupon.Text), _
                                                   UIUtility.ConvertirFechaaDecimal(txtfecIniCupon.Text), _
                                                   ddlPeriodicidad.SelectedValue, _
                                                   CDec(txtTasa.Text), _
                                                   ddlBaseMes.SelectedValue, _
                                                   tasaFloating, _
                                                   ddlBaseDias.SelectedValue, _
                                                   CDec(txtMontoNominal.Text),
                                                   UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text),
                                                   valorDiaTOriginal,
                                                   valorDiaT,
                                                   ddlFondo.SelectedValue)
                End If
                'Else
                '    DT = oValoresBM.CuponeraSWAP(CadenaNemonico, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), CDec(txtTasa.Text), _
                '        CDec(txtMontoNominal.Text), CDec(txtTasaOriginal.Text), CDec(txtMontoNominalOriginal.Text))
                'End If
                DT.Columns.Add("")
                dgLista.DataSource = DT
                dgLista.DataBind()



                habilitarColumnaFechaLiborOrigen(ddlTasaInteresVariableOrigen.SelectedValue)
                habilitarColumnaFechaLibor(ddlTasaInteresVariable.SelectedValue)


                    ViewState("dtCuponera") = DT
                    btnAceptar.Enabled = True
                    '  btnAceptar.Attributes.Add("onclick", "this.disabled = true; this.value = 'en proceso...'; __doPostBack('btnAceptar','');")
                    If ViewState("EstadoPantalla") <> "Modificar" Then UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
                Else
                    AlertaJS(mensajeCampoObligatorio)
                End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
            btnAceptar.Enabled = False
        Finally
            TipoCuponera(ddlTipoCuponera.SelectedValue, False)
        End Try

    End Sub
    Public Function InsertarOrdenInversion() As String
        Dim strCodigoOI As String
        oOrdenInversionBE = crearObjetoOI()
        strCodigoOI = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
        ViewState("CodigoOrden") = strCodigoOI
        ViewState("CodigoPortafolio") = ddlFondo.SelectedValue
        oOrdenInversion_DetalleSWAPBE = crearObjetoOI_detalleSWAP(strCodigoOI)
        oOrdenInversionBM.InsertarOI_DetalleSwap(oOrdenInversion_DetalleSWAPBE)
        Return strCodigoOI
    End Function
    Public Sub InsertarBono_Swap(ByVal CodigoOrden As String)
        Dim dt As DataTable = DirectCast(ViewState("dtBonos"), DataTable)
        oOrdenInversionBM.BorrarBono_Swap(CodigoOrden)
        For Each dr As DataRow In dt.Rows
            oOrdenInversionBM.InsertaBono_Swap(CodigoOrden, dr("CodigoISIN"), dr("CodigoNemonico"), dr("Nominal"), dr("Unidades"), UIUtility.ConvertirFechaaDecimal(dr("FechaVencimiento")), dr("PrecioVenta"))
        Next
    End Sub
    Public Sub InsertarCuponeraBono_Swap(ByVal CodigoOrden As String)
        Dim dt As DataTable = DirectCast(ViewState("dtCuponera"), DataTable)
        ActualizarColumnaFechaLibor(dt)

        oOrdenInversionBM.BorrarCuponera_Bono_Swap(CodigoOrden)
        For Each dr As DataRow In dt.Rows
            oOrdenInversionBM.InsertaCuponera_Bono_Swap(CInt(dr("Consecutivo")), CodigoOrden, UIUtility.ConvertirFechaaDecimal(dr("FechaIniOriginal")), UIUtility.ConvertirFechaaDecimal(dr("FechaFinOriginal")), _
                                                        CDec(dr("DifDiasOriginal")), CDec(dr("AmortizacOriginal")), CDec(dr("TasaCuponOriginal")), CStr(dr("BaseCuponOriginal")), CStr(dr("DiasPagoOriginal")), _
                                                        UIUtility.ConvertirFechaaDecimal(dr("fechaRealInicialOriginal")), UIUtility.ConvertirFechaaDecimal(dr("fechaRealFinalOriginal")), CDec(dr("AmortizacConsolidadoOriginal")), _
                                                        CDec(dr("MontoInteresOriginal")), CDec(dr("MontoAmortizacionOriginal")), CDec(dr("NominalRestanteOriginal")), CDec(dr("TasaSpreadOriginal")), UIUtility.ConvertirFechaaDecimal(dr("FechaIni")), _
                                                        UIUtility.ConvertirFechaaDecimal(dr("FechaFin")), CDec(dr("DifDias")), CDec(dr("Amortizac")), CDec(dr("TasaCupon")), CStr(dr("BaseCupon")), CStr(dr("DiasPago")), _
                                                        UIUtility.ConvertirFechaaDecimal(dr("fechaRealInicial")), UIUtility.ConvertirFechaaDecimal(dr("fechaRealFinal")), CDec(dr("AmortizacConsolidado")), CDec(dr("MontoInteres")), _
                                                        CDec(dr("MontoAmortizacion")), CDec(dr("NominalRestante")), CDec(dr("TasaSpread")), CDec(dr("FechaLiborOriginal")), CDec(dr("FechaLibor")))
        Next
    End Sub
    Public Sub ModificarOrdenInversion()
        oOrdenInversionBE = crearObjetoOI()
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
        oOrdenInversion_DetalleSWAPBE = crearObjetoOI_detalleSWAP(hdCodigoOrden.Value.Trim)
        oOrdenInversionBM.ModificarOI_DetalleSwap(oOrdenInversion_DetalleSWAPBE)
    End Sub
    Public Sub CargarDataInicial()
        Dim dtTemp As DataTable
        Dim rowTemp As DataRow()

        dtTemp = oPeriodicidadBM.Listar(DatosRequest).Tables(0)
        rowTemp = dtTemp.Select("CodigoPeriodicidad = '7'") ' Se retira Aperiodico
        dtTemp.Rows.Remove(rowTemp(0))

        HelpCombo.LlenarComboBox(ddlPeriodicidad, dtTemp, "CodigoPeriodicidad", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlPeriodicidadOriginal, dtTemp, "CodigoPeriodicidad", "Descripcion", True)

        dtTemp = oTipoAmortizacionBM.Listar(DatosRequest).Tables(0)
        rowTemp = dtTemp.Select("CodigoTipoAmortizacion = '10'") ' Se retira HIPOTECARIA
        dtTemp.Rows.Remove(rowTemp(0))

        HelpCombo.LlenarComboBox(ddlAmortizacion, dtTemp, "CodigoTipoAmortizacion", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlAmortizacionOriginal, dtTemp, "CodigoTipoAmortizacion", "Descripcion", True)

        HelpCombo.LlenarComboBox(ddlFondo, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlBaseDias, oParametrosGeneralesBM.Listar("CupBaseMes", DatosRequest), "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(ddlBaseMes, oParametrosGeneralesBM.Listar("CupBaseAño", DatosRequest), "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(ddlBaseDiasOrigen, oParametrosGeneralesBM.Listar("CupBaseMes", DatosRequest), "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(ddlBaseMesOrigen, oParametrosGeneralesBM.Listar("CupBaseAño", DatosRequest), "Valor", "Nombre", True)
        UIUtility.CargarGrupoIntermediarioOI(ddlGrupoInt)
        UIUtility.CargarMonedaOI(ddlMonedaLeg1)
        UIUtility.CargarMonedaOI(ddlMonedaLeg2)
        HelpCombo.LlenarComboBox(ddlTasaInteresVariableOrigen, oIndicadorBM.Indicador_SeleccionarSWAP("SW-"), "CodigoIndicador", "NombreIndicador", True)
        HelpCombo.LlenarComboBox(ddlTasaInteresVariable, oIndicadorBM.Indicador_SeleccionarSWAP("SW-"), "CodigoIndicador", "NombreIndicador", True)
        Dim dtTasaLibor As DataTable
        dtTasaLibor = oParametrosGeneralesBM.Listar("PeriodoTasaLibor", DatosRequest)
        HelpCombo.LlenarComboBox(ddlTasaLiborOrigen, dtTasaLibor, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlTasaLibor, dtTasaLibor, "Valor", "Nombre", False)
        pnlTasaLibor.Visible = False
        pnlTasaLiborOrigen.Visible = False
    End Sub
    Public Sub HabilitarControlesBusqueda(ByVal estado As Boolean)
        txtISIN.Enabled = estado
        txtMnemonico.Enabled = estado
        btnAgregar.Enabled = Not estado
    End Sub
    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim accionRpta As String = String.Empty
            Dim CodigoOrden As String
            If hdRptaConfirmar.Value.ToUpper = "SI" Then
                EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
                Dim mensajeCampoObligatorio = ValidarCamposObligatorios()
                If mensajeCampoObligatorio = String.Empty Then

                    ViewState("CodigoPortafolio") = ViewState("CodigoOrden") = ViewState("GrabarOrden") = Nothing

                    If oOrdenInversionBM.ValidaISIN(txtISINOperacion.Text, hdCodigoOrden.Value) > 0 Then
                        AlertaJS("Codigo ISIN ya en uso.")
                    ElseIf ValidadAmoritacion() = False Then
                        AlertaJS("La suma de la amortización no puede ser mayor a 100%")
                    Else
                        If Request.QueryString("PTNeg") = String.Empty Then

                            If ViewState("EstadoPantalla") = "Ingresar" Then
                                CodigoOrden = InsertarOrdenInversion()

                            Else
                                CodigoOrden = hdCodigoOrden.Value
                                ModificarOrdenInversion()
                            End If

                            If ddlTipoCuponera.SelectedValue = "1" Then InsertarBono_Swap(CodigoOrden)
                            InsertarCuponeraBono_Swap(CodigoOrden)
                            Limpiar()
                            ViewState("GrabarOrden") = "OK"
                            Response.Redirect("frmListarSwapPorcentaje.aspx")
                        Else
                            If Trim(txtISINOperacion.Text) = String.Empty Then
                                AlertaJS("Debe ingresar el codigo ISIN de la operación.", "$('#txtISINOperacion').css('border', '1px solid red');")
                            Else
                                If txtISINOperacion.Text.Length < 12 Then
                                    AlertaJS("El codigo ISIN debe tener 12 caracteres.", "$('#txtISINOperacion').css('border', '1px solid red');")
                                Else
                                    CodigoOrden = hdCodigoOrden.Value

                                    ModificarOrdenInversion()
                                    If ddlTipoCuponera.SelectedValue = "1" Then InsertarBono_Swap(CodigoOrden)
                                    InsertarCuponeraBono_Swap(CodigoOrden)

                                    oOrdenInversionBM.ConfirmaOrdenInversion(hdCodigoOrden.Value, ddlFondo.SelectedValue, txtISINOperacion.Text, DatosRequest)
                                    ViewState("GrabarOrden") = "OK"
                                    ReturnArgumentShowDialogPopup()
                                End If
                            End If
                        End If
                    End If
                Else
                    AlertaJS(mensajeCampoObligatorio)
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
            If Not ViewState("CodigoOrden") Is Nothing And ViewState("GrabarOrden") Is Nothing Then oOrdenInversionBM.OrdenInversion_BorrarOI_Error_SWAP(ViewState("CodigoPortafolio"), ViewState("CodigoOrden"))
        End Try
    End Sub
    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        If Not ViewState("Load") = "1" Then
            If Me.ddlFondo.SelectedValue = "" Then
                AlertaJS("Seleccione un portafolio para continuar.")
                Exit Sub
            End If
            ShowDialogPopupValores(txtISIN.Text.Trim.ToUpper, String.Empty, txtMnemonico.Text.Trim.ToUpper, ddlFondo.SelectedItem.Text, ddlFondo.SelectedValue, "2", "BO", 1)
            ViewState("Load") = "1"
        Else
            ViewState("Load") = "0"
        End If
    End Sub
    Protected Sub btnAgregar_Click(sender As Object, e As System.EventArgs) Handles btnAgregar.Click
        Try
            Dim dt As New DataTable
            Dim oValoresBE As New ValoresBE
            If txtMnemonico.Text.Trim = String.Empty Then
                AlertaJS("Falta ingresar el código Mnemónico.")
            ElseIf txtISIN.Text.Trim = String.Empty Then
                AlertaJS("Falta ingresar el código ISIN.")       
            ElseIf txtUnidades.Text.Trim = String.Empty Or CDec(txtUnidades.Text) = 0 Then
                AlertaJS("Falta ingresar las Unidades.")
            ElseIf CType(ViewState("totalUnidades"), Decimal) >= CDec(txtUnidades.Text) Then
                If ViewState("dtBonos") Is Nothing Then
                    dt = New DataTable
                    dt.Columns.Add("Correlativo")
                    dt.Columns.Add("CodigoISIN")
                    dt.Columns.Add("CodigoNemonico")
                    dt.Columns.Add("Unidades", GetType(Decimal))
                    dt.Columns.Add("Nominal", GetType(Decimal))
                    dt.Columns.Add("FechaVencimiento")
                    dt.Columns.Add("PrecioVenta", GetType(Decimal))
                    Dim DR As DataRow = dt.NewRow
                    DR(0) = "0"
                    DR(1) = txtISIN.Text
                    DR(2) = txtMnemonico.Text
                    DR(3) = txtUnidades.Text
                    DR(4) = CDec(oValoresBM.NominalBono(ddlFondo.SelectedValue, txtISIN.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), CDec(txtUnidades.Text)))
                    txtMontoNominalOriginal.Text = CDec(txtMontoNominalOriginal.Text) + CDec(DR(4))
                    oValoresBE = oValoresBM.Seleccionar(txtMnemonico.Text.Trim, DatosRequest)
                    If oValoresBE.Tables(0).Rows.Count > 0 Then
                        DR(5) = UIUtility.ConvertirFechaaString(oValoresBE.Valor.Item(0).FechaVencimiento.ToString)
                        DR(6) = 0
                        dt.Rows.Add(DR)
                        HabilitarControlesBusqueda(True)
                    Else
                        AlertaJS("No existe el código Mnemónico para agregar a la negociación.")
                    End If
                Else

                    For Each row As GridViewRow In gvBonos.Rows
                        If row.Cells(2).Text = txtISIN.Text Then
                            AlertaJS("Este Bono ya se agregó anteriormente.")
                            LimpiaIsin()
                            Exit Sub
                        End If
                    Next
                    dt = DirectCast(ViewState("dtBonos"), DataTable)
                    Dim DR As DataRow = dt.NewRow
                    DR(0) = "0"
                    DR(1) = txtISIN.Text
                    DR(2) = txtMnemonico.Text
                    DR(3) = txtUnidades.Text
                    DR(4) = oValoresBM.NominalBono(ddlFondo.SelectedValue, txtISIN.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), CDec(txtUnidades.Text))
                    txtMontoNominalOriginal.Text = CDec(txtMontoNominalOriginal.Text) + CDec(DR(4))
                    oValoresBE = oValoresBM.Seleccionar(txtMnemonico.Text.Trim, DatosRequest)
                    If oValoresBE.Tables(0).Rows.Count > 0 Then
                        DR(5) = UIUtility.ConvertirFechaaString(oValoresBE.Valor.Item(0).FechaVencimiento.ToString)
                        DR(6) = 0
                        dt.Rows.Add(DR)
                        HabilitarControlesBusqueda(True)
                        Dim I As Integer = 0
                    Else
                        AlertaJS("No existe el código Mnemónico para agregar a la negociación.")
                    End If
                End If
                'If ddlTipoCuponera.SelectedValue = "1" Then
                '    txtTasaOriginal.Text = oValoresBM.TasaBono(txtISIN.Text)
                'End If
                LimpiaIsin()
                gvBonos.DataSource = dt
                gvBonos.DataBind()
                ViewState("dtBonos") = dt
              
            Else
                AlertaJS("No se puede agregar, las unidades sobrepasan el total disponible del instrumento.")
            End If

        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub gvBonos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBonos.RowCommand
        Try
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            If e.CommandName = "Eliminar" Then
                Dim dt As DataTable = DirectCast(ViewState("dtBonos"), DataTable)
                txtMontoNominalOriginal.Text = CDec(txtMontoNominalOriginal.Text) - CDec(gvr.Cells(5).Text)
                dt.Rows.RemoveAt(gvr.RowIndex)
                gvBonos.DataSource = dt
                gvBonos.DataBind()
                ViewState("dtBonos") = dt
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            Dim dt As DataTable = DirectCast(ViewState("dtCuponera"), DataTable)
            If e.CommandName = "Eliminar" Then
                dt.Rows.RemoveAt(gvr.RowIndex)
                dgLista.DataSource = dt
                dgLista.DataBind()
                ViewState("dtCuponera") = dt
            ElseIf e.CommandName = "Modificar" Then
                Dim txtFechaTerminoOriginal As TextBox, txtFechaTermino As TextBox, txtAmortizacion As TextBox,
                PagoAmortizacion As Decimal, NominalActual As Decimal, PagoAmortizacionOriginal As Decimal, NominalActualOriginal As Decimal,
                PeriodicidadDiasOriginal As Decimal, PeriodicidadDias As Decimal, tasaSpreadOriginal As Decimal = 0, tasaSpread As Decimal = 0, baseAnual As Decimal,
                cantRegistros As Integer = 0
                txtFechaTerminoOriginal = CType(gvr.FindControl("tbFechaTerminoOriginal"), TextBox)
                txtFechaTermino = CType(gvr.FindControl("tbFechaTermino"), TextBox)
                txtAmortizacion = CType(gvr.FindControl("txtAmortizacion"), TextBox)

                Select Case ddlPeriodicidadOriginal.SelectedValue
                    Case 1, 2, 3, 4
                        PeriodicidadDiasOriginal = ddlPeriodicidadOriginal.SelectedValue * 30
                    Case 5
                        PeriodicidadDiasOriginal = 180
                    Case 6
                        PeriodicidadDiasOriginal = 360
                End Select

                Select Case ddlPeriodicidad.SelectedValue
                    Case 1, 2, 3, 4
                        PeriodicidadDias = ddlPeriodicidad.SelectedValue * 30
                    Case 5
                        PeriodicidadDias = 180
                    Case 6
                        PeriodicidadDias = 360
                End Select

                If ddlTasaInteresVariableOrigen.SelectedValue <> String.Empty Then tasaSpreadOriginal = CDec(ddlTasaInteresVariableOrigen.SelectedItem.Text.Split("|")(1).Trim)
                If ddlTasaInteresVariable.SelectedValue <> String.Empty Then tasaSpread = CDec(ddlTasaInteresVariable.SelectedItem.Text.Split("|")(1).Trim)

                If txtFechaTerminoOriginal.Text.Trim <> String.Empty And txtFechaTerminoOriginal.Text <> "00/00/0000" Then
                    If UIUtility.ConvertirFechaaDecimal(dt.Rows(gvr.RowIndex)("FechaIniOriginal")) < UIUtility.ConvertirFechaaDecimal(txtFechaTerminoOriginal.Text) Then
                        If UIUtility.ConvertirFechaaDecimal(txtFechaTerminoOriginal.Text) >= UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) Then
                            dt.Rows(gvr.RowIndex)("FechaFinOriginal") = txtFechaTerminoOriginal.Text
                            dt.Rows(gvr.RowIndex)("DifDiasOriginal") = IIf(ddlBaseDiasOrigen.SelectedValue = "30", Dias360(CType(dt.Rows(gvr.RowIndex)("FechaIniOriginal"), Date), CType(dt.Rows(gvr.RowIndex)("FechaFinOriginal"), Date)), _
                                                                                                                  DiasACT(CType(dt.Rows(gvr.RowIndex)("FechaIniOriginal"), Date), CType(dt.Rows(gvr.RowIndex)("FechaFinOriginal"), Date)))
                            If ddlBaseMesOrigen.SelectedValue = "ACT" Then
                                baseAnual = IIf(DateTime.IsLeapYear(Right(dt.Rows(gvr.RowIndex)("FechaFinOriginal"), 4)), 366, 365)
                            Else
                                baseAnual = 360
                            End If

                            dt.Rows(gvr.RowIndex)("MontoInteresOriginal") = (CDec(txtTasaOriginal.Text + tasaSpreadOriginal) / 100) * _
                                                                             CDec(dt.Rows(gvr.RowIndex)("NominalRestanteOriginal")) * _
                                                                             (CDec(dt.Rows(gvr.RowIndex)("DifDiasOriginal")) / baseAnual)

                            dt.Rows(gvr.RowIndex)("TotalFlujoOriginal") = CDec(dt.Rows(gvr.RowIndex)("MontoAmortizacionOriginal")) + CDec(dt.Rows(gvr.RowIndex)("MontoInteresOriginal"))
                            cantRegistros = gvr.RowIndex
                            For i As Integer = gvr.RowIndex + 1 To dt.Rows.Count - 1
                                If dt.Rows(i)("FechaIniOriginal") <> String.Empty And dt.Rows(i)("FechaIniOriginal") <> "00/00/0000" Then
                                    dt.Rows(i)("FechaIniOriginal") = dt.Rows(i - 1)("FechaFinOriginal")
                                    dt.Rows(i)("FechaFinOriginal") = RetornarFechaProxima360(CType(dt.Rows(i)("FechaIniOriginal"), String), PeriodicidadDiasOriginal)
                                    dt.Rows(i)("DifDiasOriginal") = IIf(ddlBaseDiasOrigen.SelectedValue = "30", Dias360(CType(dt.Rows(i)("FechaIniOriginal"), Date), CType(dt.Rows(i)("FechaFinOriginal"), Date)), _
                                                                                                             DiasACT(CType(dt.Rows(i)("FechaIniOriginal"), Date), CType(dt.Rows(i)("FechaFinOriginal"), Date)))

                                    If ddlBaseMesOrigen.SelectedValue = "ACT" Then
                                        baseAnual = IIf(DateTime.IsLeapYear(Right(dt.Rows(i)("FechaFinOriginal"), 4)), 366, 365)
                                    Else
                                        baseAnual = 360
                                    End If

                                    dt.Rows(i)("MontoInteresOriginal") = (CDec(txtTasaOriginal.Text + tasaSpreadOriginal) / 100) * _
                                                                          CDec(dt.Rows(i)("NominalRestanteOriginal")) * _
                                                                         (CDec(dt.Rows(i)("DifDiasOriginal")) / baseAnual)

                                    dt.Rows(i)("TotalFlujoOriginal") = CDec(dt.Rows(i)("MontoAmortizacionOriginal")) + CDec(dt.Rows(i)("MontoInteresOriginal"))
                                    cantRegistros += 1
                                Else
                                    Exit For
                                End If
                            Next
                            If cantRegistros > 1 Then txtfecFinCuponOrigen.Text = CType(dt.Rows(cantRegistros)("FechaFinOriginal"), String)
                        Else
                            AlertaJS("LEG 1: La fecha final no puede ser menor a la fecha de operación del portafolio seleccionado.")
                            GoTo actualizarCuponera
                        End If
                    Else
                        AlertaJS("LEG 1: La fecha final no puede ser menor a la fecha inicial.")
                        GoTo actualizarCuponera
                    End If

                    End If

                If txtFechaTermino.Text.Trim <> String.Empty And txtFechaTermino.Text <> "00/00/0000" Then
                    If UIUtility.ConvertirFechaaDecimal(dt.Rows(gvr.RowIndex)("FechaIni")) < UIUtility.ConvertirFechaaDecimal(txtFechaTermino.Text) Then
                        If UIUtility.ConvertirFechaaDecimal(txtFechaTermino.Text) >= UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) Then
                            dt.Rows(gvr.RowIndex)("FechaFin") = txtFechaTermino.Text
                            dt.Rows(gvr.RowIndex)("DifDias") = IIf(ddlBaseDias.SelectedValue = "30", Dias360(CType(dt.Rows(gvr.RowIndex)("FechaIni"), Date), CType(dt.Rows(gvr.RowIndex)("FechaFin"), Date)), _
                                                                                                     DiasACT(CType(dt.Rows(gvr.RowIndex)("FechaIni"), Date), CType(dt.Rows(gvr.RowIndex)("FechaFin"), Date)))
                            If ddlBaseMes.SelectedValue = "ACT" Then
                                baseAnual = IIf(DateTime.IsLeapYear(Right(dt.Rows(gvr.RowIndex)("FechaFin"), 4)), 366, 365)
                            Else
                                baseAnual = 360
                            End If

                            dt.Rows(gvr.RowIndex)("MontoInteres") = (CDec(txtTasa.Text + tasaSpread) / 100) * _
                                                                     CDec(dt.Rows(gvr.RowIndex)("NominalRestante")) * _
                                                                     (CDec(dt.Rows(gvr.RowIndex)("DifDias")) / baseAnual)

                            dt.Rows(gvr.RowIndex)("TotalFlujo") = CDec(dt.Rows(gvr.RowIndex)("MontoAmortizacion")) + CDec(dt.Rows(gvr.RowIndex)("MontoInteres"))
                            dt.Rows(gvr.RowIndex)("TCFlujo") = CDec(dt.Rows(gvr.RowIndex)("TotalFlujo")) / IIf(CDec(dt.Rows(gvr.RowIndex)("TotalFlujoOriginal")) = 0, 1, CDec(dt.Rows(gvr.RowIndex)("TotalFlujoOriginal")))
                            cantRegistros = gvr.RowIndex
                            For i As Integer = gvr.RowIndex + 1 To dt.Rows.Count - 1
                                If dt.Rows(i)("FechaIni") <> String.Empty And dt.Rows(i)("FechaIni") <> "00/00/0000" Then
                                    dt.Rows(i)("FechaIni") = dt.Rows(i - 1)("FechaFin")
                                    dt.Rows(i)("FechaFin") = RetornarFechaProxima360(CType(dt.Rows(i)("FechaIni"), String), PeriodicidadDias)
                                    dt.Rows(i)("DifDias") = IIf(ddlBaseDias.SelectedValue = "30", Dias360(CType(dt.Rows(i)("FechaIni"), Date), CType(dt.Rows(i)("FechaFin"), Date)), _
                                                                                                             DiasACT(CType(dt.Rows(i)("FechaIni"), Date), CType(dt.Rows(i)("FechaFin"), Date)))

                                    If ddlBaseMes.SelectedValue = "ACT" Then
                                        baseAnual = IIf(DateTime.IsLeapYear(Right(dt.Rows(i)("FechaFin"), 4)), 366, 365)
                                    Else
                                        baseAnual = 360
                                    End If

                                    dt.Rows(i)("MontoInteres") = (CDec(txtTasa.Text + tasaSpread) / 100) * _
                                                                          CDec(dt.Rows(i)("NominalRestante")) * _
                                                                         (CDec(dt.Rows(i)("DifDias")) / baseAnual)

                                    dt.Rows(i)("TotalFlujo") = CDec(dt.Rows(i)("MontoAmortizacion")) + CDec(dt.Rows(i)("MontoInteres"))
                                    dt.Rows(i)("TCFlujo") = CDec(dt.Rows(i)("TotalFlujo")) / IIf(CDec(dt.Rows(i)("TotalFlujoOriginal")) = 0, 1, CDec(dt.Rows(i)("TotalFlujoOriginal")))
                                    cantRegistros += 1
                                Else
                                    Exit For
                                End If
                            Next
                            If cantRegistros > 1 Then txtfecFinCupon.Text = CType(dt.Rows(cantRegistros)("FechaFin"), String)
                        Else
                            AlertaJS("LEG 2: La fecha final no puede ser menor a la fecha de operación del portafolio seleccionado.")
                        End If
                    Else
                        AlertaJS("LEG 2: La fecha final no puede ser menor a la fecha inicial.")
                    End If
                End If


                    'PagoAmortizacion = Math.Round((CDec(txtAmortizacion.Text) / 100) * CDec(txtMontoNominal.Text), 7)
                    'PagoAmortizacionOriginal = Math.Round((CDec(txtAmortizacion.Text) / 100) * CDec(txtMontoNominalOriginal.Text), 7)
                    'dt.Rows(gvr.RowIndex)("MontoAmortizacion") = PagoAmortizacion
                    'dt.Rows(gvr.RowIndex)("MontoAmortizacionOriginal") = PagoAmortizacionOriginal
                    'dt.Rows(gvr.RowIndex)("Amortizac") = txtAmortizacion.Text
                    'NominalActual = CDec(dt.Rows(gvr.RowIndex)("NominalRestante"))
                    'NominalActualOriginal = CDec(dt.Rows(gvr.RowIndex)("NominalRestanteOriginal"))
                    'For i As Integer = gvr.RowIndex + 1 To dt.Rows.Count - 1
                    '    NominalActual = NominalActual - CDec(dt.Rows(i - 1)("MontoAmortizacion"))
                    '    NominalActualOriginal = NominalActualOriginal - CDec(dt.Rows(i - 1)("MontoAmortizacionOriginal"))
                    '    dt.Rows(i)("NominalRestante") = NominalActual
                    '    dt.Rows(i)("NominalRestanteOriginal") = NominalActualOriginal
                    '    dt.Rows(i)("MontoInteres") = (CDec(txtTasa.Text) / 100) * CDec(dt.Rows(i)("NominalRestante")) *
                    '    (CDec(dt.Rows(i)("DiasPago")) / CInt(ddlBaseDias.SelectedValue))
                    '    '  (CDec(dt.Rows(i)("DiasPago")) / CInt(ddlBase.SelectedValue))
                    '    dt.Rows(i)("MontoInteresOriginal") = (CDec(txtTasaOriginal.Text) / 100) * CDec(dt.Rows(i)("NominalRestanteOriginal")) *
                    '     (CDec(dt.Rows(i)("DiasPago")) / CInt(ddlBaseDias.SelectedValue))
                    '    ' (CDec(dt.Rows(i)("DiasPago")) / CInt(ddlBase.SelectedValue))
                'Next
actualizarCuponera:
                    dgLista.DataSource = dt
                    dgLista.DataBind()
                    ViewState("dtCuponera") = dt
                End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub ddlTipoCuponera_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTipoCuponera.SelectedIndexChanged
        TipoCuponera(ddlTipoCuponera.SelectedValue, True)
    End Sub
    Sub TipoCuponera(ByVal Tipo As String, ByVal Opcion As Boolean)

        If Tipo = "1" Then
            '   ddlAmortizacion.Enabled = False
            txtMontoNominalOriginal.Enabled = False
            txtTasaOriginal.Enabled = False
            ddlTasaInteresVariableOrigen.Enabled = False
            fsEmisionesSWAP.Visible = True
            ddlAmortizacionOriginal.Enabled = False
        Else
            '  ddlAmortizacion.Enabled = True
            txtMontoNominalOriginal.Enabled = True
            txtTasaOriginal.Enabled = True
            ddlTasaInteresVariableOrigen.Enabled = True
            fsEmisionesSWAP.Visible = False
            ddlAmortizacionOriginal.Enabled = True
            gvBonos.DataSource = Nothing
            gvBonos.DataBind()
        End If
        If Opcion Then
            ddlPeriodicidad.SelectedValue = String.Empty
            ddlPeriodicidadOriginal.SelectedValue = String.Empty
            ddlAmortizacion.SelectedValue = String.Empty
            ddlAmortizacionOriginal.SelectedValue = String.Empty
            txtTasaOriginal.Text = String.Empty
            txtMontoNominal.Text = String.Empty
            txtMontoNominalOriginal.Text = "0"
        End If
    End Sub
    Protected Sub ddlTipoOperacion_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTipoOperacion.SelectedIndexChanged
        If ddlTipoOperacion.SelectedValue = "1" Then
            ddlFormaCambio.SelectedValue = "1"
        Else
            ddlFormaCambio.SelectedValue = "2"
        End If
    End Sub
    Protected Sub ddlGrupoInt_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlGrupoInt.SelectedIndexChanged
        CargarIntermediario()
    End Sub
    Protected Sub dgLista_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim HeaderGrid As GridView = sender
            Dim HeaderGridRow As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
            Dim HeaderCell As New TableCell
            HeaderCell.Text = ""
            HeaderCell.ColumnSpan = 2
            HeaderCell.BorderColor = Drawing.Color.Black
            HeaderCell.VerticalAlign = VerticalAlign.Middle
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow.Cells.Add(HeaderCell)

            HeaderCell = New TableCell()
            HeaderCell.Text = "LEG 1"
            HeaderCell.ColumnSpan = If(ddlTasaInteresVariableOrigen.SelectedValue <> String.Empty, 9, 8)
            HeaderCell.BorderColor = Drawing.Color.Black
            HeaderCell.VerticalAlign = VerticalAlign.Middle
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow.Cells.Add(HeaderCell)

            HeaderCell = New TableCell()
            HeaderCell.Text = "LEG 2"
            HeaderCell.ColumnSpan = If(ddlTasaInteresVariable.SelectedValue <> String.Empty, 9, 8)
            HeaderCell.BorderColor = Drawing.Color.Black
            HeaderCell.VerticalAlign = VerticalAlign.Middle
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow.Cells.Add(HeaderCell)


            dgLista.Controls(0).Controls.AddAt(0, HeaderGridRow)

        End If
    End Sub
    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Dim ibSeleccionar As ImageButton, txtFechaFin As TextBox, txtFechaFinOriginal As TextBox, divFechaOriginal As HtmlControl, divFecha As HtmlControl, divFechaLiborOriginal As HtmlControl, divFechaLibor As HtmlControl, txtFechaLiborOriginal As TextBox, txtFechaLibor As TextBox
        Dim fechaOperacionPortafolio As Decimal = UIUtility.ObtenerFechaNegocio(ddlFondo.SelectedValue.Trim)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                ibSeleccionar = CType(e.Row.FindControl("btnModificar"), ImageButton)
                txtFechaFinOriginal = CType(e.Row.FindControl("tbFechaTerminoOriginal"), TextBox)
                txtFechaFin = CType(e.Row.FindControl("tbFechaTermino"), TextBox)
                divFechaOriginal = CType(e.Row.FindControl("divFechaTerminoOriginal"), HtmlControl)
                divFecha = CType(e.Row.FindControl("divFechaTermino"), HtmlControl)

                txtFechaLiborOriginal = CType(e.Row.FindControl("tbFechaLiborOriginal"), TextBox)
                txtFechaLibor = CType(e.Row.FindControl("tbFechaLibor"), TextBox)
                divFechaLiborOriginal = CType(e.Row.FindControl("divFechaLiborOriginal"), HtmlControl)
                divFechaLibor = CType(e.Row.FindControl("divFechaLibor"), HtmlControl)

                txtFechaFinOriginal.Enabled = Not (fechaOperacionPortafolio > UIUtility.ConvertirFechaaDecimal(txtFechaFinOriginal.Text))
                txtFechaFin.Enabled = Not (fechaOperacionPortafolio > UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text))
                ibSeleccionar.Visible = (txtFechaFin.Enabled Or txtFechaFinOriginal.Enabled)
                If Not txtFechaFinOriginal.Enabled Then divFechaOriginal.Attributes.Add("class", "input-append")
                If Not txtFechaFin.Enabled Then divFecha.Attributes.Add("class", "input-append")

                txtFechaLibor.Enabled = txtFechaFin.Enabled
                txtFechaLiborOriginal.Enabled = txtFechaFinOriginal.Enabled

                If Not txtFechaLiborOriginal.Enabled Then divFechaLiborOriginal.Attributes.Add("class", "input-append")
                If Not txtFechaLibor.Enabled Then divFechaLibor.Attributes.Add("class", "input-append")


                'If CType(Session("ValidarFecha"), String) = "FECHADIFERENTE" And hdPagina.Value = "CO" Then
                '    txtFechaFin.Enabled = False
                '    txtFechaFinOriginal.Enabled = False
                '    divFechaOriginal.Attributes.Add("class", "input-append")
                '    divFecha.Attributes.Add("class", "input-append")
                '    ibSeleccionar.Visible = False
                'End If

                If ViewState("EstadoPantalla") = "Modificar" And hdPagina.Value <> "CO" And Request.QueryString("estado") <> "E-EJE" Then
                    txtFechaFin.Enabled = False
                    txtFechaFinOriginal.Enabled = False
                    divFechaOriginal.Attributes.Add("class", "input-append")
                    divFecha.Attributes.Add("class", "input-append")

                    txtFechaLibor.Enabled = False
                    txtFechaLiborOriginal.Enabled = False
                    divFechaLiborOriginal.Attributes.Add("class", "input-append")
                    divFechaLibor.Attributes.Add("class", "input-append")


                    ibSeleccionar.Visible = False
                End If
            End If

        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla de Cuponera SWAP / " + ex.Message)
        End Try
    End Sub
    Private Function RetornarFechaProxima360(ByVal fecha As String, ByVal nperiodo As Integer) As String
        Dim fechaD As Integer, fechaM As Integer, fechaA As Integer
        Dim contFecha As Integer = 30

        fechaD = fecha.Substring(0, 2)
        fechaM = fecha.Substring(3, 2)
        fechaA = fecha.Substring(6, 4)
        fechaD += nperiodo

        While (fechaD > contFecha)
            fechaD -= contFecha
            fechaM += 1
            If fechaM > 12 Then
                fechaA += 1
                fechaM = 1
            End If
        End While

        RetornarFechaProxima360 = String.Concat(fechaA, _
                                  IIf(Len(fechaM.ToString) = 1, String.Concat("0", fechaM.ToString), fechaM.ToString), _
                                  IIf(Len(fechaD.ToString) = 1, String.Concat("0", fechaD.ToString), fechaD.ToString))

        Return UIUtility.ConvertirFechaaString(RetornarFechaProxima360)
    End Function

    Private Function RetornarFechaDiferencia(ByVal fecha As Decimal, ByVal nperiodo As Integer) As Decimal
        Dim fechaResultante As Date
        Dim fechaOperacion As Date = Convert.ToDateTime(UIUtility.ConvertirDecimalAStringFormatoFecha(fecha))
        fechaResultante = fechaOperacion.AddDays(nperiodo)
        Return (UIUtility.ConvertirFechaaDecimal(fechaResultante.ToString("dd/MM/yyyy")))
    End Function

    Protected Sub ddlTasaInteresVariableOrigen_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTasaInteresVariableOrigen.SelectedIndexChanged
        habilitarTasaVariableOrigen()
    End Sub

    Protected Sub ddlTasaInteresVariable_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTasaInteresVariable.SelectedIndexChanged
        habilitarTasaVariable()
    End Sub

    Public Sub habilitarTasaVariableOrigen()
        ddlTasaLiborOrigen.Enabled = Not (ddlTasaInteresVariableOrigen.SelectedValue = String.Empty)
        If ddlTasaInteresVariableOrigen.SelectedValue <> String.Empty Then
            pnlTasaLiborOrigen.Visible = True
            ddlTasaLiborOrigen.SelectedValue = "0"
            ObtenerTasaLiborOrigenDiaT()

        Else
            pnlTasaLiborOrigen.Visible = False
            txtTasaLiborOrigen.Text = String.Empty
        End If
        btnAceptar.Enabled = False
    End Sub

    Public Sub habilitarTasaVariable()

        If ddlTasaInteresVariable.SelectedValue <> String.Empty Then
            pnlTasaLibor.Visible = True
            ddlTasaLibor.SelectedValue = "0"
            ObtenerTasaLiborDiaT()

        Else
            pnlTasaLibor.Visible = False
            txtTasaLibor.Text = String.Empty
        End If
        btnAceptar.Enabled = False
    End Sub

    Protected Sub ddlTasaLiborOrigen_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTasaLiborOrigen.SelectedIndexChanged
        Dim tiempoLibo As String = ddlTasaLiborOrigen.SelectedValue
        Dim oCotizacionVACBE As New CotizacionVACBE

        If tiempoLibo <> "" Then
            ObtenerTasaLiborOrigenDiaT()
        End If
        btnAceptar.Enabled = False
    End Sub

    Private Sub ObtenerTasaLiborOrigenDiaT()
        Dim tiempoLibo As String = ddlTasaLiborOrigen.SelectedValue
        Dim oCotizacionVACBE As New CotizacionVACBE
        Dim fechaBuscar As Decimal = RetornarFechaDiferencia(UIUtility.ObtenerFechaNegocio(ddlFondo.SelectedValue.Trim), Convert.ToInt32(tiempoLibo))
        oCotizacionVACBE = oCotizacionVACBM.Seleccionar(fechaBuscar, ddlTasaInteresVariableOrigen.SelectedValue, DatosRequest)
        If oCotizacionVACBE.Tables(0).Rows.Count > 0 Then
            txtTasaLiborOrigen.Text = oCotizacionVACBE.Tables(0).Rows(0)("Valor")
        Else
            txtTasaLiborOrigen.Text = "0"
        End If
    End Sub

    Private Sub ObtenerTasaLiborDiaT()
        Dim tiempoLibo As String = ddlTasaLibor.SelectedValue
        Dim oCotizacionVACBE As New CotizacionVACBE
        Dim fechaBuscar As Decimal = RetornarFechaDiferencia(UIUtility.ObtenerFechaNegocio(ddlFondo.SelectedValue.Trim), Convert.ToInt32(tiempoLibo))
        oCotizacionVACBE = oCotizacionVACBM.Seleccionar(fechaBuscar, ddlTasaInteresVariable.SelectedValue, DatosRequest)
        If oCotizacionVACBE.Tables(0).Rows.Count > 0 Then
            txtTasaLibor.Text = oCotizacionVACBE.Tables(0).Rows(0)("Valor")
        Else
            txtTasaLibor.Text = "0"
        End If
    End Sub

    Public Sub habilitarColumnaFechaLiborOrigen(ByVal diaT As String)
        If diaT <> "" Then
            Me.dgLista.Columns(6).Visible = True
        Else
            Me.dgLista.Columns(6).Visible = False
        End If

    End Sub

    Public Sub habilitarColumnaFechaLibor(ByVal diaT As String)
        If diaT <> "" Then
            Me.dgLista.Columns(15).Visible = True
        Else
            Me.dgLista.Columns(15).Visible = False
        End If

    End Sub


    Public Sub ActualizarColumnaFechaLiborOrigen(ByVal diaT As String)
        Dim i As Integer
        For i = 0 To dgLista.Rows.Count - 1
            Dim casillaFechaLibo As TextBox = CType(dgLista.Rows(i).FindControl("tbFechaLiboOriginal"), TextBox)
            Dim casillaFechaFinOriginal As TextBox = CType(dgLista.Rows(i).FindControl("tbFechaTerminoOriginal"), TextBox)

            If casillaFechaFinOriginal.Enabled = False Then

            End If
            casillaFechaLibo.Text = RetornarFechaProxima360(dgLista.Rows(i).Cells(3).Text.Trim(), Convert.ToInt32(diaT))

        Next
    End Sub

    Public Sub ActualizarColumnaFechaLibor(ByRef dtCupones As DataTable)
        Dim i As Integer
        '15
        For i = 0 To dgLista.Rows.Count - 1
            Dim casillaFechaLiborOriginal As TextBox = CType(dgLista.Rows(i).FindControl("tbFechaLiborOriginal"), TextBox)
            Dim casillaFechaLibor As TextBox = CType(dgLista.Rows(i).FindControl("tbFechaLibor"), TextBox)
            Dim obj = (From dr In dtCupones Where dr("Consecutivo") = dgLista.Rows(i).Cells(2).Text Select dr).FirstOrDefault()
            If obj IsNot Nothing Then
                obj("FechaLiborOriginal") = UIUtility.ConvertirFechaaDecimal(casillaFechaLiborOriginal.Text)
                obj("FechaLibor") = UIUtility.ConvertirFechaaDecimal(casillaFechaLibor.Text)
            Else
                obj("FechaLiborOriginal") = 0
                obj("FechaLibor") = 0
            End If

        Next
    End Sub

    Protected Sub ddlTasaLibor_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTasaLibor.SelectedIndexChanged
        Dim tiempoLibo As String = ddlTasaLibor.SelectedValue
        Dim oCotizacionVACBE As New CotizacionVACBE

        If tiempoLibo <> "" Then
            ObtenerTasaLiborDiaT()
        End If
        btnAceptar.Enabled = False
    End Sub

    Protected Sub txtTasaLiborOrigen_TextChanged(sender As Object, e As System.EventArgs) Handles txtTasaLiborOrigen.TextChanged
        btnAceptar.Enabled = False
    End Sub

    Protected Sub txtTasaLibor_TextChanged(sender As Object, e As System.EventArgs) Handles txtTasaLibor.TextChanged
        btnAceptar.Enabled = False
    End Sub
End Class

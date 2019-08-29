Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Imports ParametrosSIT
Imports Sit.BusinessLayer.MotorInversiones

Partial Class Modulos_Parametria_AdministracionValores_frmAdministracionValores
    Inherits BasePage
    Public Property dtCuponeraNormalSesion() As DataTable
        Get
            If Session("dtCuponeraNormal") Is Nothing Then
                Return New DataTable
            Else
                Return CType(Session("dtCuponeraNormal"), DataTable)
            End If
        End Get
        Set(ByVal value As DataTable)
            Session("dtCuponeraNormal") = value
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

#Region " /* Metodos de Pagina */ "
    Sub LlenaRating(ByVal Local As String)
        ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-23 | Carga Rating
        Dim oRating As New ParametrosGeneralesBM
        'Obtenemos la tabla completa Rating 
        Dim dtRating As DataTable = oRating.Listar("RATING", DatosRequest)

        Dim rowRating() As DataRow = dtRating.Select("Comentario in ('DLP_EXT','DLP_LOC','DCP_LOC','DCP_EXT','TODOS')")
        HelpCombo.LlenarComboBox(Me.ddlCalificacion, rowRating.CopyToDataTable(), "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlRating, rowRating.CopyToDataTable(), "Valor", "Nombre", True)
        ' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-23 | Carga Rating
    End Sub
    Sub LlenaRatingMandato(ByVal Local As String)
        ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-23 | Carga Rating
        Dim oRating As New ParametrosGeneralesBM
        'Obtenemos la tabla completa Rating 
        Dim dtRating As DataTable = oRating.Listar("RATING", DatosRequest)

        Dim rowRating() As DataRow = dtRating.Select("Comentario in ('DLP_EXT','DLP_LOC','DCP_LOC','DCP_EXT','TODOS')")
        'HelpCombo.LlenarComboBox(Me.ddlCalificacion, rowRating.CopyToDataTable(), "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(Me.ddlRatingM, rowRating.CopyToDataTable(), "Valor", "Nombre", True)
        ' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-23 | Carga Rating
    End Sub
    Sub RemoverCombo(ByVal Combo As DropDownList)
        Combo.BorderColor = Drawing.Color.Black
    End Sub
    Sub HabilitaControlesWarrants(ByVal Clase As String)
        If Clase = "18" Then
            'OT 9856 - 24/01/2017 - Carlos Espejo
            'Descripcion: Se movio la funcion de resaltar al archivo UIUtility.vb
            UIUtility.ResaltaCajaTexto(txtPiso, True)
            UIUtility.ResaltaCajaTexto(tbMnemonico, True)
            UIUtility.ResaltaCajaTexto(txtTecho, True)
            UIUtility.ResaltaCajaTexto(txtGarante, True)
            UIUtility.ResaltaCajaTexto(txtSubyacente, True)
            UIUtility.ResaltaCajaTexto(txtPrecioEjercicio, True)
            UIUtility.ResaltaCajaTexto(txtTamanoEmision, True)
            UIUtility.ResaltaCajaTexto(tbDescripcion, True)
            UIUtility.ResaltaCajaTexto(tbFechaEmision, True)
            UIUtility.ResaltaCajaTexto(tbFechaVencimiento, True)
            UIUtility.ResaltaCajaTexto(tbCodigoISIN, True)
            UIUtility.ResaltaCajaTexto(tbValorNominal, True)
            UIUtility.ResaltaCombo(ddlSituacion, True)
            UIUtility.ResaltaCombo(ddlMercado, True)
            UIUtility.ResaltaCombo(ddlTipoCodigoValor, True)
            UIUtility.ResaltaCombo(ddlCalificacion, True)
            UIUtility.ResaltaCombo(ddlRating, True)

            UIUtility.ResaltaCombo(ddlRatingM, True)

            UIUtility.ResaltaCombo(ddlMoneda, True)
            UIUtility.ResaltaBotones(btnCustodios, True)
            ' Fin OT 9856
            tbFechaLiberada.Enabled = False
            ddlTipoLiquidez.Enabled = False
            ddlTipoRentaFija.Enabled = False
            ddlGrupoContable.Enabled = False
            tbTasaSpread.Enabled = False
            ddlCotizacionVAC.Enabled = False
            ddlPeriodicidad.Enabled = False
            ddlTipoAmortizacion.Enabled = False
            tbTasaCupon.Enabled = False
            tbFechaPrimerCupon.Enabled = False
            ddlTipoCupon.Enabled = False
            tbPorcPosicion.Enabled = False
            tbPosicionAct.Enabled = False
            tbTasaEncaje.Enabled = False
        ElseIf Clase = "1" Or Clase = "3" Or Clase = "7" Then
            'OT 10090 - 21/03/2017 - Carlos Espejo
            'Descripcion: Se resaltan los controles minimos necesarios
            UIUtility.ResaltaCajaTexto(tbValorUnitario, True)
            UIUtility.ResaltaCajaTexto(tbMnemonico, True)
            UIUtility.ResaltaCajaTexto(tbDescripcion, True)
            UIUtility.ResaltaCajaTexto(tbCodigoSBS, True)
            UIUtility.ResaltaCajaTexto(txtCodigoSBSCorrelativo, True)
            UIUtility.ResaltaCajaTexto(tbCodigoISIN, True)
            UIUtility.ResaltaCajaTexto(tbFechaEmision, True)
            UIUtility.ResaltaCajaTexto(tbFechaVencimiento, True)
            UIUtility.ResaltaCajaTexto(tbFechaPrimerCupon, True)
            UIUtility.ResaltaCajaTexto(tbValorNominal, True)
            UIUtility.ResaltaCajaTexto(tbTasaCupon, True)
            UIUtility.ResaltaCombo(ddlMoneda, True)
            UIUtility.ResaltaCombo(ddlMercado, True)
            UIUtility.ResaltaCombo(ddlTipoCodigoValor, True)
            UIUtility.ResaltaCombo(ddlCalificacion, True)
            UIUtility.ResaltaCombo(ddlRating, True)

            UIUtility.ResaltaCombo(ddlRatingM, True)


            UIUtility.ResaltaCombo(ddlSituacion, True)
            UIUtility.ResaltaCombo(ddlPeriodicidad, True)
            If hdnCodigoClaseInstrumento.Value <> "11" And (Trim(tbCodigoSBSinst.Text) <> "100" And Trim(tbCodigoSBSinst.Text) <> "101") Then
                UIUtility.ResaltaCombo(ddlTipoAmortizacion, True)
                UIUtility.ResaltaCombo(ddlTipoCupon, True)
            End If
            UIUtility.ResaltaCombo(ddlCuponNDias, True)
            UIUtility.ResaltaCombo(ddlCuponBase, True)
            UIUtility.ResaltaBotones(btnCustodios, True)
            UIUtility.ResaltaBotones(btnCuponNormal, True)
            'OT 10090 Fin
            tbFechaLiberada.Enabled = False
            ddlGrupoContable.Enabled = False
            tbMargenInicial.Enabled = False
            tbMargenInicial.Enabled = False
            ddlAgrupacion.Enabled = False
            ddlTipoTitulo.Enabled = False
            tbContractSize.Enabled = False
            tbMargenMnto.Enabled = False
            tbValorEfecColocado.Enabled = False
            tbTasaSpread.Enabled = False
            ddlTipoRentaFija.Enabled = False
        End If
    End Sub
    Private Sub CargaPortafolio()
        HelpCombo.LlenarComboBox(ddlPortafolio, New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True)
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            Dim cuponValores As CuponeraNormalValoresBE = CuponeraValoresSesion
            If hdEmisorVal.Value <> "" Then
                tbEmisor.Text = hdEmisorVal.Value
            End If
            If hdEmisorDesc.Value <> "" Then
                tbEmisorDesc.Text = hdEmisorDesc.Value
            End If
            If hdCodigoSBSinst.Value <> "" Then
                tbCodigoSBSinst.Text = hdCodigoSBSinst.Value
                If ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RVAR") Then
                    HabilitaRentaVariable(False)
                End If
            End If
            If hdSinTipoInst.Value <> "" Then
                tbSinTipoInst.Text = hdSinTipoInst.Value
            End If
            Dim pagina As String = "Valores"
            If ddlTipoRenta.SelectedValue = TR_DERIVADOS.ToString Then
                If tbCodigoSBSinst.Text = CODIGOSBS_FUTUROS Then
                    HabilitaDividendosFuturos(True)
                Else
                    HabilitaDividendosFuturos(False)
                End If
            End If
            If Request.QueryString("vOI") Is Nothing Then
                btnAceptar.Attributes.Add("onclick", "javascript:return Validar();")
                btnCuponNormal.Attributes.Add("onclick", "javascript:return ValidarCuponNormal();")
                HabilitaFondos()
                If tbCodigoSBSinst.Text <> "" And ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RVAR") Then
                    Dim oTipoInstrumentoBM As New TipoInstrumentoBM
                    Dim oTipoInstrumentoBE As New TipoInstrumentoBE
                    Dim oRowTI As TipoInstrumentoBE.TipoInstrumentoRow
                    oTipoInstrumentoBE = oTipoInstrumentoBM.SeleccionarPorFiltroValores("", tbCodigoSBSinst.Text, "", DatosRequest)
                    oRowTI = CType(oTipoInstrumentoBE.Tables(0).Rows(0), TipoInstrumentoBE.TipoInstrumentoRow)
                    If oRowTI.CodigoClaseInstrumento = "6" Then
                        ddlTipoLiquidez.Visible = True
                        lblTipoLiquidez.Visible = True
                    Else
                        lblTipoLiquidez.Visible = False
                        ddlTipoLiquidez.Visible = False
                    End If
                Else
                    lblTipoLiquidez.Visible = False
                    ddlTipoLiquidez.Visible = False
                End If
            End If
            If Not Page.IsPostBack Then
                Try
                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se implementa en procedimiento la habilitación (Visibilidad) de campos para renta fija | 17/05/18
                    If Session("accionValor") = "INGRESAR" Then
                        fdSeccionCentral.Attributes.Add("style", "display:none")
                        fdSeccionInferior.Attributes.Add("style", "display:none")
                        divClasificacionRating.Attributes.Add("style", "display:none")
                    End If
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se implementa en procedimiento la habilitación (Visibilidad) de campos para renta fija | 17/05/18

                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se crea objeto sesión para controlar la utlización del nuevo combo de porcentaje de amortización en cuponera | 24/05/18
                    Session("UtilizaAmortizacion") = False
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se crea objeto sesión para controlar la utlización del nuevo combo de porcentaje de amortización en cuponera | 24/05/18

                    hdnTipoRenta.Value = Request.QueryString("tipoRenta")
                    hdnNemonico.Value = Request.QueryString("nemonico")
                    CargarCombos()
                    BloqueaTodo()
                    CargarParametrosGenerales()
                    ddlTipoRenta.Enabled = True
                    ddlPortafolio.Enabled = True
                    Session("cuponeraEspecial") = Nothing
                    Session("cuponEspecial_Eliminados") = Nothing
                    Session("cuponNormal_Eliminados") = Nothing
                    Dim dtAuxCuponera As New DataTable
                    dtAuxCuponera.Columns.Add()
                    dtAuxCuponera.Columns.Add()
                    Session("cuponEspecial_Eliminados") = dtAuxCuponera
                    Session("cuponNormal_Eliminados") = dtAuxCuponera
                    Session("agrupacionIE") = Nothing
                    Session("agrupacionIC") = Nothing
                    Session("TablaDetalle") = Nothing
                    Session("TablaDetalleCapComp") = Nothing
                    Session("DetalleAgrupacionE") = Nothing
                    Session("Nocional") = Nothing
                    ViewState("CUPONERA_REQUERIDO") = ""
                    If Not cuponValores.CambioValores Then Session("cuponeraNormal") = Nothing
                    'Request.QueryString("accionValor") => Cuando este control tiene el valor "Consulta_Aprobacion_Instrumento", quiere decir
                    'que la pantalla de administración de valores se mostrará en modo consulta. Esta consulta se realiza desde la opción "Aprobación de
                    'Instrumentos.
                    If (Session("accionValor") = "MODIFICAR" And Not (Request.QueryString("cod") = Nothing)) Or _
                        (Not (Request.QueryString("cod") = Nothing) And (Request.QueryString("accionValor") = "Consulta_Aprobacion_Instrumento")) Then
                        If (Session("accionValor") = "MODIFICAR") Then
                            divClasificacionRating.Attributes.Add("style", "display:none")
                        End If
                        tbMnemonico.Text = Server.UrlDecode(Request.QueryString("cod"))
                        tbMnemonico.Enabled = False

                        Dim Mnemonico As String
                        If tbMnemonico.Text.Length >= 4 Then
                            Mnemonico = tbMnemonico.Text.Substring(0, 4)
                        Else
                            Mnemonico = tbMnemonico.Text
                        End If
                        tbMnemonico.Enabled = False
                        imbTipoInstrumento.Visible = False
                        CargarGrupoRegAux()
                        CargarRegistro(tbMnemonico.Text)
                        HabilitaFondos()
                        If Not Request.QueryString("vOI") Is Nothing Or Request.QueryString("accionValor") IsNot Nothing Then
                            BloqueaTodo()
                            BloqueaOI()
                        End If
                        If (Not Request.QueryString("vOF") Is Nothing) Then
                            btnCuponNormal.Visible = False
                        End If
                        Dim objInstComp As New InstrumentosCompuestos()
                        objInstComp.CargarRegistro(tbMnemonico.Text)
                        Session("Identificador") = objInstComp.Count
                        Session("DetalleAgrupacionE") = objInstComp.Datos
                        Dim oIENocionalBM As New InstrumentosEstructuradosBM
                        Session("NOCIONAL") = oIENocionalBM.ListarInstrumentoEstructuradoNocional(tbMnemonico.Text)
                        If tbCodigoSBSinst.Text <> "" And ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RVAR") Then
                            Dim oTipoInstrumentoBM As New TipoInstrumentoBM
                            Dim oTipoInstrumentoBE As New TipoInstrumentoBE
                            Dim oRowTI As TipoInstrumentoBE.TipoInstrumentoRow
                            oTipoInstrumentoBE = oTipoInstrumentoBM.SeleccionarPorFiltroValores("", tbCodigoSBSinst.Text, "", DatosRequest)
                            oRowTI = CType(oTipoInstrumentoBE.Tables(0).Rows(0), TipoInstrumentoBE.TipoInstrumentoRow)
                            If oRowTI.CodigoClaseInstrumento = "6" Then
                                ddlTipoLiquidez.Visible = True
                                lblTipoLiquidez.Visible = True
                                Dim oLiquidezAccionBM As New LiquidezAccionBM
                                Dim oLiquidezAccionBE As New LiquidezAccionBE
                                Dim oRowLA As LiquidezAccionBE.LiquidezAccionRow
                                oLiquidezAccionBE = oLiquidezAccionBM.SeleccionarPorFiltro(tbMnemonico.Text, ParametrosSIT.ESTADO_ACTIVO, DatosRequest)
                                If oLiquidezAccionBE.Tables(0).Rows.Count > 0 Then  'HDG 20111005
                                    oRowLA = CType(oLiquidezAccionBE.Tables(0).Rows(0), LiquidezAccionBE.LiquidezAccionRow)
                                    'ddlTipoLiquidez.SelectedValue = oRowLA.CriterioLiquidez
                                End If
                            Else
                                lblTipoLiquidez.Visible = False
                                ddlTipoLiquidez.Visible = False
                            End If
                        Else
                            lblTipoLiquidez.Visible = False
                            ddlTipoLiquidez.Visible = False
                        End If
                        'Se ocultan los botones cuando se consulta desde la opción de Aprobación de Instrumentos
                        If (Request.QueryString("accionValor") = "Consulta_Aprobacion_Instrumento") Then
                            'btnCustodios.Visible = False
                            'btnCuponNormal.Visible = False
                            btnAceptar.Visible = False
                            btnSalir.Visible = False
                            CargarClasificacionRating()
                            divClasificacionRating.Attributes.Add("style", "display:block")
                        End If
                    ElseIf Session("accionValor") = "INGRESAR" Then
                        InicializarCampos()
                        tbMnemonico.Enabled = True
                        Session("Custodios") = False
                    Else
                        Session("accionValor") = "INGRESAR"
                        Session("Custodios") = False
                        If tbCodigoSBSinst.Text <> "" And ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RVAR") Then
                            Dim oTipoInstrumentoBM As New TipoInstrumentoBM
                            Dim oTipoInstrumentoBE As New TipoInstrumentoBE
                            Dim oRowTI As TipoInstrumentoBE.TipoInstrumentoRow
                            oTipoInstrumentoBE = oTipoInstrumentoBM.SeleccionarPorFiltroValores("", tbCodigoSBSinst.Text, "", DatosRequest)
                            oRowTI = CType(oTipoInstrumentoBE.Tables(0).Rows(0), TipoInstrumentoBE.TipoInstrumentoRow)
                            If oRowTI.CodigoClaseInstrumento = "6" Then
                                ddlTipoLiquidez.Visible = True
                                lblTipoLiquidez.Visible = True
                            Else
                                lblTipoLiquidez.Visible = False
                                ddlTipoLiquidez.Visible = False
                            End If
                        Else
                            lblTipoLiquidez.Visible = False
                            ddlTipoLiquidez.Visible = False
                        End If
                    End If
                Catch ex As Exception
                    AlertaJS(Replace(ex.Message.ToString(), "'", "").Replace(vbCrLf, " "), "CerrarLoading();")
                End Try
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Select Case _Modal.Value
                    Case "INS"
                        tbCodigoSBSinst.Text = CType(Session("SS_DatosModal"), String())(0)
                        tbSinTipoInst.Text = CType(Session("SS_DatosModal"), String())(1)
                        hdnCodigoClaseInstrumento.Value = CType(Session("SS_DatosModal"), String())(2)
                        hdCodigoSBSinst.Value = CType(Session("SS_DatosModal"), String())(0)
                        hdSinTipoInst.Value = CType(Session("SS_DatosModal"), String())(1)
                        CargarGrupoRegAux()
                        HabilitaControlesWarrants(hdnCodigoClaseInstrumento.Value)
                        If hdnCodigoClaseInstrumento.Value = "11" Or (hdCodigoSBSinst.Value = "100" Or hdCodigoSBSinst.Value = "101") Then
                            HabilitaRentaFija(False)
                            ddlTipoCupon.SelectedValue = "3"
                            imgInfoTipoCupon.Visible = (ddlTipoCupon.SelectedValue = ViewState(ParametrosSIT.TIPO_CUPON_A_DESCUENTO))
                            ddlTipoAmortizacion.SelectedValue = "1"
                            tbTasaCupon.Text = "0.0000000"
                            btnCuponNormal.Visible = False
                            ViewState("CUPONERA_REQUERIDO") = "N"
                        ElseIf ddlTipoRenta.SelectedValue.ToUpper = UIUtility.ObtenerCodigoTipoRenta("RFIJ") And hdnCodigoClaseInstrumento.Value <> "11" Then
                            lblTipoRentaFija.Visible = True
                            btnCuponNormal.Visible = True
                            tbFechaPrimerCupon.Text = ""
                            tbFechaVencimiento.Text = ""
                            ddlTipoRentaFija.Visible = True
                            'OTXXXX - 23/09/2018 - Ian Pastor M.
                            'hdnCodigoClaseInstrumento.Value = "11" => Representa a la clase de instrumento "Letras Hipotecarias"
                            If (Trim(tbCodigoSBSinst.Text) = "52") Or (Trim(tbCodigoSBSinst.Text) = "08" And Trim(hdEmisor.Value) = "4000" And ddlTipoCupon.SelectedValue = "3") Or (hdnCodigoClaseInstrumento.Value = "11") Then
                                ViewState("CUPONERA_REQUERIDO") = "N"
                                btnCuponNormal.Visible = False
                                btnCuponNormal.Enabled = False
                            End If
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se modifica procedimiento agregando input flag para detectar el origen del llamado de procedimiento HabilitaRentaFija(Bool)| 17/05/18
                            HabilitaRentaFija(False)
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se modifica procedimiento agregando input flag para detectar el origen del llamado de procedimiento HabilitaRentaFija(Bool)| 17/05/18
                        End If
                    Case "EMI"
                        tbEmisor.Text = CType(Session("SS_DatosModal"), String())(0)
                        tbEmisorDesc.Text = CType(Session("SS_DatosModal"), String())(1)
                        hdEmisor.Value = CType(Session("SS_DatosModal"), String())(2)
                        hdEmisorVal.Value = CType(Session("SS_DatosModal"), String())(0)
                        hdEmisorDesc.Value = CType(Session("SS_DatosModal"), String())(1)
                        If CType(Session("SS_DatosModal"), String())(2) = "" Then
                            AlertaJS("El emisor seleccionado no tiene código SBS", "CerrarLoading();")
                        End If
                End Select
                Session.Remove("SS_DatosModal")
            End If


        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página: " & Replace(ex.Message, "'", ""), "CerrarLoading();")
        End Try
    End Sub
    Private Sub BloqueaOI()
        ddlTipoRenta.Enabled = False
        ddlPortafolio.Enabled = False
        imbEmisor.Visible = False
        btnCustodios.Enabled = True
        btnCustodios.Visible = True
        If (ddlTipoRenta.SelectedValue.ToString = UIUtility.ObtenerCodigoTipoRenta("RFIJ")) Then
            btnCuponNormal.Visible = True
        End If
        dvDiv121.Attributes.Add("class", "input-append")
        dvDiv221.Attributes.Add("class", "input-append")
        dvDiv321.Attributes.Add("class", "input-append")
        btnAceptar.Visible = False
    End Sub
    Private Sub btnCuponNormal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCuponNormal.Click
        Try
            If validaFechas() = False Then
                Exit Sub
            End If
            Dim strTemp As String = ""
            If ViewState("FechaEmision") <> tbFechaEmision.Text Or ViewState("FechaVencimiento") <> tbFechaVencimiento.Text Or ViewState("FechaPrimerCupon") <> tbFechaPrimerCupon.Text Or ViewState("TasaSpread") <> tbTasaSpread.Text Or ViewState("TasaCupon") <> tbTasaCupon.Text Then
                strTemp = "SI"
            End If
            Dim strAux As String = "NO"
            If Not Request.QueryString("vOI") Is Nothing Then
                strAux = "YES"
            End If
            'OT 10090 - 21/03/2017 - Carlos Espejo
            'Descripcion: Se toma el valor del control base de cupon
            Dim valor As String = ddlCuponBase.SelectedValue
            'OT 10090 Fin
            'Indica que el mantenimiento de Custodio Valores se abrirá en modo consulta cuando se abre desde la opción de Aprobación de Instrumentos Financieros
            If Request.QueryString("accionValor") = "Consulta_Aprobacion_Instrumento" Then
                strAux = "YES"
            End If
            EjecutarJS(UIUtility.MostrarPopUp("frmGeneracionCuponeraNormal.aspx?vISIN=" + tbCodigoISIN.Text.Trim.ToUpper + _
                                              "&vPeriod=" + obtenerNroDiasPeriodicidad(ddlPeriodicidad.SelectedValue).ToString() + _
                                              "&vBaseC=" + ddlIntCorrBase.SelectedValue + _
                                              "&vMontoNominal=" + tbValorNominal.Text + _
                                              "&vTasaC=" + tbTasaCupon.Text + _
                                              "&vPeriodicidad=" + ddlPeriodicidad.SelectedValue + _
                                              "&vTasaSpread=" + tbTasaSpread.Text + _
                                              "&vIndicador=0&vFlag=" + ddlTipoAmortizacion.SelectedValue + _
                                              "&vFechaP=" + tbFechaPrimerCupon.Text + _
                                              "&vFechaV=" + tbFechaVencimiento.Text + _
                                              "&vMnemo=" + tbMnemonico.Text.Trim.ToUpper + _
                                              "&vFechaE=" + tbFechaEmision.Text + _
                                              "&vReadOnly=" + strAux + _
                                              "&vEstado=" + strTemp + _
                                              "&vNumeroDias=" + ddlIntCorrNDias.SelectedValue.Trim, _
                                              "no", 1140, 655, 10, 90, "No", "No", "Yes", "Yes"), False)

        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación", "CerrarLoading();")
        End Try
    End Sub
    Private Sub EnviarAlertaPorNuevoIntrumento()
        Dim destinatarios As String = String.Empty
        Dim dt As DataTable
        dt = New ParametrosGeneralesBM().Listar(CORREOS_ALTA_INSTRUMENTOS, DatosRequest)
        For Each fila As DataRow In dt.Rows
            destinatarios = destinatarios + fila("Valor") + ";"
        Next
        Dim asunto As String = "SIT - NUEVO INSTRUMENTO PENDIENTE DE APROBACIÓN: " & tbMnemonico.Text
        Dim mensaje As String
        mensaje = "Se ha registrado un nuevo instrumento en el SIT de tipo <b>" & ddlTipoRenta.SelectedItem.Text.ToUpper & "</b>, el cual requiere de su aprobación para poder ser negociado.<br/>"
        If ddlTipoRenta.SelectedValue.ToUpper = UIUtility.ObtenerCodigoTipoRenta("RFIJ") Then
            mensaje = mensaje & "<br/>Código Nemónico: <b>" & tbMnemonico.Text & "</b>"
            mensaje = mensaje & "<br/>Descripción: <b>" & tbDescripcion.Text & "</b>"
            mensaje = mensaje & "<br/>Valor Nomimal: <b>" & tbValorNominal.Text & "</b>"
            mensaje = mensaje & "<br/>Fecha Emisión: <b>" & tbFechaEmision.Text & "</b>"
            mensaje = mensaje & "<br/>Fecha Vencimiento: <b>" & tbFechaVencimiento.Text & "</b>"
            mensaje = mensaje & "<br/>Fecha Primer Cupón: <b>" & tbFechaPrimerCupon.Text & "</b>"
            mensaje = mensaje & "<br/>Tipo Amortización: <b>" & ddlTipoAmortizacion.SelectedItem.Text & "</b>"
            mensaje = mensaje & "<br/>Rating Fondo: <b>" & ddlRating.SelectedItem.Text & "</b>"

            mensaje = mensaje & "<br/>Rating Aseguradora: <b>" & ddlRatingM.SelectedItem.Text & "</b>"

            mensaje = mensaje & "<br/>Calificacion: <b>" & ddlCalificacion.SelectedItem.Text & "</b>"
            mensaje = mensaje & "<br/>Nombre emisor: <b>" & tbEmisorDesc.Text & "</b>"
            mensaje = mensaje & "<br/>Tipo de Instrumento: <b>" & tbCodigoSBSinst.Text & " - " & tbSinTipoInst.Text & "</b>"
            mensaje = mensaje & "<br/>Nombre del Instrumento: <b>" & tbDescripcion.Text & "</b>"
            mensaje = mensaje & "<br/>Tipo Cupón: <b>" & ddlTipoCupon.SelectedItem.Text & "</b>"
            mensaje = mensaje & "<br/>Tasa Cupón: <b>" & tbTasaCupon.Text & "</b>"
            mensaje = mensaje & "<br/>Periodicidad: <b>" & ddlPeriodicidad.SelectedItem.Text & "</b>"
            mensaje = mensaje & "<br/>Moneda Cupón: <b>" & ddlMoneda.SelectedItem.Text & "</b>"
            mensaje = mensaje & "<br/>Mercado: <b>" & ddlMercado.SelectedItem.Text & "</b><br/>"

            Dim dtAux As DataTable = Session("cuponeraNormal")
            If Not IsNothing(dtAux) Then
                If dtAux.Rows.Count > 0 Then
                    mensaje = mensaje & "<br/>Detalle:<br/>"
                    mensaje = mensaje & "<Table cellspacing='0' cellpadding='4' rules='all' style='border-color:#706F6F;border-width:1px;border-style:solid;width:100%;border-collapse:collapse;'>"
                    mensaje = mensaje & "<tr align='center' style='color:#0039A6;background-color:#EFF4FA;font-family:Trebuchet MS;font-size:11px;font-weight:bold;height:23px;'>"
                    mensaje = mensaje & "<td> Nro </td>"
                    mensaje = mensaje & "<td> Fecha Inicio </td>"
                    mensaje = mensaje & "<td> Fecha Termino </td>"
                    mensaje = mensaje & "<td> Diferencia Dias </td>"
                    mensaje = mensaje & "<td> Amortizacion </td>"
                    mensaje = mensaje & "<td> Tasa Cupon </td>"
                    mensaje = mensaje & "<td> Base </td>"
                    mensaje = mensaje & "<td> Dias Pago </td>"
                    mensaje = mensaje & "</tr>"
                    For i = 0 To dtAux.Rows.Count - 1
                        mensaje = mensaje & "<tr class='nowrap' style='font-family:Trebuchet MS;font-size:11px;height:18px;'>"
                        mensaje = mensaje & "<td align='center'> " & CStr(i + 1) & " </td>"
                        mensaje = mensaje & "<td align='center'> " & dtAux.Rows(i)("FechaIni") & " </td>"
                        mensaje = mensaje & "<td align='center'> " & dtAux.Rows(i)("FechaFin") & " </td>"
                        mensaje = mensaje & "<td align='center'> " & dtAux.Rows(i)("DifDias") & " </td>"
                        mensaje = mensaje & "<td align='right'> " & dtAux.Rows(i)("Amortizac") & " </td>"
                        mensaje = mensaje & "<td align='right'> " & dtAux.Rows(i)("TasaCupon") & " </td>"
                        mensaje = mensaje & "<td align='right'> " & dtAux.Rows(i)("BaseCupon") & " </td>"
                        mensaje = mensaje & "<td align='center'> " & dtAux.Rows(i)("DiasPago") & " </td>"
                        mensaje = mensaje & "</tr>"
                    Next
                    mensaje = mensaje & "<Table><br/>"
                End If
            End If
        ElseIf ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RVAR") Then 'renta variable
            mensaje = mensaje & "<br/>Código Nemónico: <b>" & tbMnemonico.Text & "</b>"
            mensaje = mensaje & "<br/>Descripción: <b>" & tbDescripcion.Text & "</b>"
            mensaje = mensaje & "<br/>Tipo Título: <b>" & ddlTipoTitulo.SelectedItem.Text & "</b>"
            mensaje = mensaje & "<br/>Mercado: <b>" & ddlMercado.SelectedItem.Text & "</b>"
            mensaje = mensaje & "<br/>Tipo de Instrumento: <b>" & tbCodigoSBSinst.Text & " - " & tbSinTipoInst.Text & "</b>"
            mensaje = mensaje & "<br/>Nombre del Instrumento: <b>" & tbDescripcion.Text & "</b>"
            mensaje = mensaje & "<br/>Nombre emisor: <b>" & tbEmisorDesc.Text & "</b>"
            mensaje = mensaje & "<br/>Moneda: <b>" & ddlMoneda.SelectedItem.Text & "</b>"
            mensaje = mensaje & "<br/>Código ISIN: <b>" & tbCodigoISIN.Text & "</b><br/>"
        ElseIf ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RDER") Then
            mensaje = mensaje & "<br/>Código Nemónico: <b>" & tbMnemonico.Text & "</b>"
            mensaje = mensaje & "<br/>Descripción: <b>" & tbDescripcion.Text & "</b>"
            mensaje = mensaje & "<br/>Piso: <b>" & txtPiso.Text & "</b>"
            mensaje = mensaje & "<br/>Techo: <b>" & txtTecho.Text & "</b>"
            mensaje = mensaje & "<br/>Subyacente: <b>" & txtSubyacente.Text & "</b>"
            mensaje = mensaje & "<br/>Precio Ejercicio: <b>" & txtPrecioEjercicio.Text & "</b>"
            mensaje = mensaje & "<br/>Nombre emisor: <b>" & tbEmisorDesc.Text & "</b>"
            mensaje = mensaje & "<br/>Código ISIN: <b>" & tbCodigoISIN.Text & "</b>"
            mensaje = mensaje & "<br/>Moneda Cupón: <b>" & ddlMoneda.SelectedItem.Text & "</b>"
            mensaje = mensaje & "<br/>Mercado: <b>" & ddlMercado.SelectedItem.Text & "</b>"
            mensaje = mensaje & "<br/>Tipo de Instrumento: <b>" & tbCodigoSBSinst.Text & " - " & tbSinTipoInst.Text & "</b>"
            mensaje = mensaje & "<br/>Nombre del Instrumento: <b>" & tbDescripcion.Text & "</b>"
            mensaje = mensaje & "<br/>Tipo Título: <b>" & ddlTipoTitulo.SelectedItem.Text & "</b><br/>"
        End If
        mensaje = mensaje & "<br/>"
        Dim ret As Boolean = UIUtility.EnviarMail(destinatarios, "", asunto, mensaje, DatosRequest)
    End Sub
    Private Sub EnviarAlertaPorActivarIntrumento()
        Dim destinatarios As String = String.Empty
        Dim dt As DataTable
        dt = New ParametrosGeneralesBM().Listar(CORREOS_ALTA_INSTRUMENTOS, DatosRequest)
        For Each fila As DataRow In dt.Rows
            destinatarios = destinatarios + fila("Valor") + ";"
        Next
        Dim asunto As String = "SIT - CAMBIO SITUACION A ACTIVO DE INSTRUMENTO : " & tbMnemonico.Text
        Dim mensaje As String = "Se ha cambiado la situación a activo de un instrumento en el SIT.<br/>" & _
            "<br/>Código Nemónico: " & tbMnemonico.Text & _
            "<br/>Tipo de Instrumento: " & tbCodigoSBSinst.Text & " - " & tbSinTipoInst.Text & _
            "<br/>Nombre del Instrumento: " & tbDescripcion.Text & _
            "<br/>Fecha de alta del producto: " & Date.Now.ToString("dd/MM/yyyy") & _
            "<br/>Nombre emisor: " & tbEmisorDesc.Text & _
            "<br/>Código ISIN: " & tbCodigoISIN.Text & _
            "<br/>Código SBS: " & tbCodigoSBS.Text & _
            "<br/>Cantidad en circulación: " & tbNumUnidades.Text & _
            "<br/>Monto en circulación: " & tbValorEfecColocado.Text
        Dim ret As Boolean = UIUtility.EnviarMail(destinatarios, "", asunto, mensaje, DatosRequest)
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oValoresBM As New ValoresBM
            Dim oValoresBE As New ValoresBE
            Dim NumerosValidos As String
            Dim oLiquidezAccionBM As New LiquidezAccionBM
            Dim oLiquidezAccionBE As New LiquidezAccionBE
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se agrega variable para controlar el envio de correos | 30/05/18
            Dim rootPath As String = Request.Url.Host
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se agrega variable para controlar el envio de correos | 30/05/18
            Dim codigoSBS As String = tbCodigoSBS.Text.Trim() + txtCodigoSBSCorrelativo.Text.Trim
            If Session("accionValor") = "INGRESAR" Then
                If New ValoresBM().ValidarExistenciaValor(tbMnemonico.Text.Trim(), DatosRequest) = True Or tbMnemonico.Text.Trim() = "" Then
                    AlertaJS("El Código Nemónico ingresado ya existe.", "CerrarLoading();")
                    Exit Sub
                End If
            Else
                If New ValoresBM().ValidarExistenciaValorModificar(tbMnemonico.Text.Trim(), DatosRequest) = True Or tbMnemonico.Text.Trim() = "" Then
                    AlertaJS("El Código Nemónico ingresado ya existe.", "CerrarLoading();")
                    Exit Sub
                End If
            End If
            If Session("accionValor") = "INGRESAR" Then
                If New ValoresBM().ValidarExistenciaValorISIN(tbCodigoISIN.Text.Trim(), DatosRequest) = True Or tbCodigoISIN.Text.Trim() = "" Then
                    AlertaJS("El Código ISIN ingresado ya existe.", "CerrarLoading();")
                    Exit Sub
                End If
            Else
                If New ValoresBM().ValidarExistenciaValorISINModificar(tbCodigoISIN.Text.Trim(), DatosRequest) = True Or tbCodigoISIN.Text.Trim() = "" Then
                    AlertaJS("El Código ISIN ingresado ya existe.", "CerrarLoading();")
                    Exit Sub
                End If
            End If
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se agrega validación de tasa cupon > 0 tanto para modificación y nuevo registro en Tipo Renta Fija| 17/05/18
            If ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RFIJ") Then
                'If hdnCodigoClaseInstrumento.Value <> "11" And hdnCodigoClaseInstrumento.Value <> "7" Then
                '    If IsNumeric(tbTasaCupon.Text) Then
                '        If CDec(tbTasaCupon.Text) <= 0 Then
                '            AlertaJS("El Tasa Cupón debe ser mayor a cero.", "CerrarLoading();")
                '            Exit Sub
                '        End If
                '    Else
                '        AlertaJS("El Tasa Cupón debe ser un número válido.", "CerrarLoading();")
                '        Exit Sub
                '    End If
                'End If

                'CRumiche (2019-06-21):Si es un TipoCupon A DECUENTOS la tasa no es obligatoria, de lo contrario sí se validará que sea mayor a CERO
                If IsNumeric(tbTasaCupon.Text) Then
                    If ddlTipoCupon.SelectedValue <> ViewState(ParametrosSIT.TIPO_CUPON_A_DESCUENTO) And CDec(tbTasaCupon.Text) <= 0 Then
                        AlertaJS("El Tasa Cupón debe ser mayor a cero.", "CerrarLoading();")
                        Exit Sub
                    End If
                Else
                    AlertaJS("El Tasa Cupón debe ser un número válido.", "CerrarLoading();")
                    Exit Sub
                End If
                'FIN -- CRumiche (2019-05-21):
            End If
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se agrega validación de tasa cupon > 0 tanto para modificación y nuevo registro en Tipo Renta Fija| 17/05/18
            If Not (hdnCodigoClaseInstrumento.Value = "18" And codigoSBS = "") Then
                If Session("accionValor") = "INGRESAR" Then
                    If New ValoresBM().ValidarExistenciaValorSBS(codigoSBS, DatosRequest) = True Or codigoSBS = "" Then
                        AlertaJS("El Código SBS ingresado ya existe.", "CerrarLoading();")
                        Exit Sub
                    End If
                Else
                    If New ValoresBM().ValidarExistenciaValorSBSModificar(codigoSBS, DatosRequest) = True Or codigoSBS = "" Then
                        AlertaJS("El Código SBS ingresado ya existe.", "CerrarLoading();")
                        Exit Sub
                    End If
                End If
            End If
            If validaFechas() = False Then
                Exit Sub
            End If
            If ddlAgrupacion.SelectedValue = "E" Then
                If Session("Nocional") Is Nothing Then
                    AlertaJS("Debe de Ingresar Nocionales para el Instrumento", "CerrarLoading();")
                    Exit Sub
                Else
                    If (CType(Session("Nocional"), DataTable).Rows.Count = 0) Then
                        AlertaJS("Debe de Ingresar Nocionales para el Instrumento", "CerrarLoading();")
                        Exit Sub
                    End If
                End If
            End If
            Dim intPeriodicidad As Integer = obtenerNroDiasPeriodicidad(ddlPeriodicidad.SelectedValue)
            If (hdnCodigoClaseInstrumento.Value = "3" Or _
                hdnCodigoClaseInstrumento.Value = "7" Or _
                (hdnCodigoClaseInstrumento.Value = "10" And tbCodigoSBSinst.Text.Trim.Equals("19"))) And _
                (intPeriodicidad <> 0) Then
                AlertaJS("La Periodicidad debe ser: APERIODICA.", "CerrarLoading();")
                Exit Sub
            End If
            Dim tipoInsturmento As String = tbCodigoSBSinst.Text.ToString().Trim()
            Dim flag As Boolean = False
            Dim flagCuponera As Boolean = False
            If tbCodigoSBSinst.Text = ParametrosSIT.CODIGOSBS_FUTUROS Then
                If ddlTipoRenta.SelectedValue <> UIUtility.ObtenerCodigoTipoRenta("RDER") And ddlTipoRenta.SelectedValue <> UIUtility.ObtenerCodigoTipoRenta("RVAR") Then
                    If codigoSBS < 12 Then
                        AlertaJS("Codigo SBS Incompleto.Ingrese 12 caracteres", "CerrarLoading();")
                        Exit Sub
                    End If
                End If
                Dim msjValidaRFuturos As String = ""
                msjValidaRFuturos = ValidarRegistroFuturos()
                If msjValidaRFuturos <> "" Then
                    AlertaJS(msjValidaRFuturos, "CerrarLoading();")
                    Exit Sub
                End If
                If (Session("accionValor") = "INGRESAR") Then
                    If ddlTipoRenta.SelectedValue = "" Then
                        AlertaJS("Seleccione un tipo de renta, e ingrese los datos adecuadamente", "CerrarLoading();")
                        Exit Sub
                    End If
                    Try
                        oValoresBE = crearObjeto()
                        oValoresBM.Insertar(oValoresBE, DatosRequest)
                        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se agrega condicional para el envío de correo en ambiente de desarrollo | 30/05/18
                        If rootPath.ToUpper.Trim <> "LOCALHOST" Then EnviarAlertaPorNuevoIntrumento()
                        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se agrega condicional para el envío de correo en ambiente de desarrollo | 30/05/18
                        AlertaJS("Los Datos fueron grabados correctamente", "CerrarLoading();")
                        BloqueaTodo()
                        Salir("Grabacion")
                    Catch ex As Exception
                        AlertaJS(Replace(ex.Message.ToString(), "'", ""), "CerrarLoading();")
                    End Try
                ElseIf Session("accionValor") = "MODIFICAR" Then
                    Try
                        oValoresBE = crearObjeto()
                        oValoresBM.Modificar(oValoresBE, DatosRequest)
                        If ddlSituacion.SelectedValue = "A" And Session("vSituacion") = "I" Then EnviarAlertaPorActivarIntrumento() 'HDG 20110411 OT 62087 REQ 23
                        AlertaJS("Los Datos fueron modificados correctamente", "CerrarLoading();")
                        BloqueaTodo()
                        Salir("Grabacion")
                    Catch ex As Exception
                        AlertaJS(Replace(ex.Message.ToString(), "'", ""), "CerrarLoading();")
                    End Try
                End If
                Exit Sub
            End If
            NumerosValidos = ValidarNumeros()
            If NumerosValidos = "" Then
                If ddlTipoRenta.SelectedValue <> UIUtility.ObtenerCodigoTipoRenta("RDER") And ddlTipoRenta.SelectedValue <> UIUtility.ObtenerCodigoTipoRenta("RVAR") Then
                    If codigoSBS.Length < 12 Then
                        AlertaJS("Codigo SBS Incompleto.Ingrese 12 caracteres", "CerrarLoading();")
                        Exit Sub
                    End If
                End If
                If (Session("accionValor") = "INGRESAR") Then
                    If ddlTipoRenta.SelectedValue = "" Then
                        AlertaJS("Seleccione un tipo de renta, e ingrese los datos adecuadamente", "CerrarLoading();")
                        Exit Sub
                    End If
                    Try
                        If ddlTipoRenta.SelectedValue <> UIUtility.ObtenerCodigoTipoRenta("RDER") Then
                            If (Session("Custodios") = False) Then
                                AlertaJS("Ingrese por lo menos un custodio", "CerrarLoading();")
                                Exit Sub
                            End If
                            If ddlAgrupacion.SelectedValue <> "S" Then
                                If verificarAgrupacion() = False Then
                                    AlertaJS("Ingrese adecuadamente los valores en la Agrupación", "CerrarLoading();")
                                    Exit Sub
                                End If
                                If verificarCantidadAgrupacion() = False Then
                                    AlertaJS("Ingrese por lo menos un valor en la Agrupación", "CerrarLoading();")
                                    Exit Sub
                                End If
                            End If
                        End If
                        If hdnCodigoClaseInstrumento.Value = "3" Then 'Los Certificados de Deposito no necesitan cuponera
                            ViewState("CUPONERA_REQUERIDO") = "N"
                        End If
                        If Not ViewState("CUPONERA_REQUERIDO") Is Nothing Then
                            If ViewState("CUPONERA_REQUERIDO").ToString() = "S" Then
                                If ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RFIJ") Then
                                    If Session("cuponeraNormal") Is Nothing Then
                                        AlertaJS("Requiere generar la cuponera normal", "CerrarLoading();")
                                        Exit Sub
                                    End If
                                End If
                            End If
                        End If
                        If (Session("Custodios") = False) Then
                            AlertaJS("Ingrese por lo menos un custodio", "CerrarLoading();")
                            Exit Sub
                        End If
                        oValoresBE = crearObjeto()
                        oValoresBM.Insertar(oValoresBE, DatosRequest)
                        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se agrega condicional para el envío de correo en ambiente de desarrollo | 30/05/18
                        If rootPath.ToUpper.Trim <> "LOCALHOST" Then EnviarAlertaPorNuevoIntrumento()
                        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se agrega condicional para el envío de correo en ambiente de desarrollo | 30/05/18
                        If ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RFIJ") Then
                            If ViewState("CUPONERA_REQUERIDO").ToString() = "S" Then
                                GrabarCuponeraNormal()
                            ElseIf ViewState("CUPONERA_REQUERIDO").ToString() = "N" Then
                                If (tbCodigoSBSinst.Text <> "52") And Not (Trim(tbCodigoSBSinst.Text) = "08" And Trim(hdEmisor.Value) = "4000" And ddlTipoCupon.SelectedValue = "3") Then
                                    Session("cuponeraNormal") = crearObjeto_CuponDscto()
                                    Session("accionValor") = "INGRESAR"
                                    ValoresParaGuardarCuponDescuento()
                                    GrabarCuponeraNormal()
                                End If
                            End If
                        End If
                        If Not Session("TablaDetalle") Is Nothing Then
                            oValoresBM.ModificarDetalleCustodios(tbMnemonico.Text, CType(Session("TablaDetalle"), DataTable), DatosRequest)
                        End If
                        If Not Session("TablaDetalleCapComp") Is Nothing And (tbCodigoSBSinst.Text = "51" Or tbCodigoSBSinst.Text = "53") Then
                            oValoresBM.ModificarDetalleCapitalCompro(tbMnemonico.Text, CType(Session("TablaDetalleCapComp"), DataTable), DatosRequest)
                        End If
                        If ddlAgrupacion.SelectedValue = "E" Then
                            Dim oInstrumentoEstructuradoBM As New InstrumentosEstructuradosBM
                            oInstrumentoEstructuradoBM.InsertarInstrumentoEstructuradosNocional(CType(Session("Nocional"), DataTable))
                        End If
                        oLiquidezAccionBE = CrearObjetoLA()
                        oLiquidezAccionBM.Insertar(oLiquidezAccionBE, DatosRequest)
                        AlertaJS("Los Datos fueron grabados correctamente", "CerrarLoading();")
                        GrabarAgrupacion()
                        BloqueaTodo()
                        Salir("Grabacion")
                    Catch ex As Exception
                        AlertaJS(Replace(ex.Message.ToString(), "'", ""), "CerrarLoading();")
                    End Try
                ElseIf Session("accionValor") = "MODIFICAR" Then
                    Try
                        If ddlTipoRenta.SelectedValue <> UIUtility.ObtenerCodigoTipoRenta("RDER") Then
                            If ddlAgrupacion.SelectedValue <> "S" Then
                                If verificarAgrupacion() = False And verificarAgrupacionGrabada(tbMnemonico.Text) = False Then
                                    AlertaJS("Ingrese adecuadamente los valores en la Agrupación", "CerrarLoading();")
                                    Exit Sub
                                End If
                            End If
                            Dim fueTemporal As Boolean = False
                            fueTemporal = New ValoresBM().ValidarFueNemonicoTemporal(tbMnemonico.Text, DatosRequest)
                            If fueTemporal = False Then
                                If Not VerificarCustodia(tbMnemonico.Text) Then
                                    AlertaJS("Ingrese por lo menos un custodio", "CerrarLoading();")
                                    Exit Sub
                                End If
                            End If
                        End If
                        If Not (ViewState("CUPONERA_REQUERIDO") Is Nothing) Then
                            If ViewState("CUPONERA_REQUERIDO").ToString() = "S" Then
                                If ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RFIJ") Then
                                    If Not Session("cuponeraNormal") Is Nothing Then
                                        GrabarCuponeraNormal()
                                    End If
                                    Dim fueTemporal As Boolean = False
                                    fueTemporal = New ValoresBM().ValidarFueNemonicoTemporal(tbMnemonico.Text, DatosRequest)
                                    If fueTemporal = False Then

                                        If Not VerificarCuponeraNormal(tbMnemonico.Text) Then
                                            AlertaJS("Requiere generar la cuponera normal", "CerrarLoading();")
                                            Exit Sub
                                        End If
                                    End If
                                End If
                            ElseIf ViewState("CUPONERA_REQUERIDO").ToString() = "N" Then
                                If (tbCodigoSBSinst.Text <> "52") And Not (Trim(tbCodigoSBSinst.Text) = "08" And Trim(hdEmisor.Value) = "4000" And ddlTipoCupon.SelectedValue = "3") Then
                                    Session("cuponeraNormal") = crearObjeto_CuponDscto()
                                    Session("accionValor") = "MODIFICAR"
                                    ValoresParaGuardarCuponDescuento()
                                    GrabarCuponeraNormal()
                                End If
                            End If
                        End If
                        If Not Session("TablaDetalle") Is Nothing Then
                            If CType(Session("TablaDetalle"), DataTable).Rows.Count = 0 Then
                                AlertaJS("Ingrese por lo menos un custodio", "CerrarLoading();")
                                Exit Sub
                            End If
                        End If
                        oValoresBE = crearObjeto()
                        oValoresBM.Modificar(oValoresBE, DatosRequest)
                        GrabarAgrupacion()
                        If ddlSituacion.SelectedValue = "A" And Session("vSituacion") = "I" Then EnviarAlertaPorActivarIntrumento() 'HDG 20110411 OT 62087 REQ 23
                        If Not Session("TablaDetalle") Is Nothing Then
                            If CType(Session("TablaDetalle"), DataTable).Rows.Count > 0 Then
                                oValoresBM.ModificarDetalleCustodios(tbMnemonico.Text, CType(Session("TablaDetalle"), DataTable), DatosRequest)
                            End If
                        End If
                        If Not Session("TablaDetalleCapComp") Is Nothing And (tbCodigoSBSinst.Text = "51" Or tbCodigoSBSinst.Text = "53") Then
                            If CType(Session("TablaDetalleCapComp"), DataTable).Rows.Count > 0 Then
                                oValoresBM.ModificarDetalleCapitalCompro(tbMnemonico.Text, CType(Session("TablaDetalleCapComp"), DataTable), DatosRequest)
                            End If
                        End If
                        If ddlAgrupacion.SelectedValue = "E" Then
                            Dim oInstrumentoEstructuradoBM As New InstrumentosEstructuradosBM
                            oInstrumentoEstructuradoBM.ModificarInstrumentoEstructuradosNocional(CType(Session("Nocional"), DataTable))
                        End If
                        oLiquidezAccionBE = CrearObjetoLA()
                        oLiquidezAccionBM.Modificar(oLiquidezAccionBE, DatosRequest)
                        AlertaJS("Los Datos fueron modificados correctamente", "CerrarLoading();")
                        BloqueaTodo()
                        Salir("Grabacion")
                    Catch ex As Exception
                        AlertaJS(Replace(ex.Message.ToString(), "'", ""), "CerrarLoading();")
                    End Try
                End If
            Else
                AlertaJS(NumerosValidos, "CerrarLoading();")
                Exit Sub
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar", "CerrarLoading();")
        End Try
    End Sub
    Private Function ValidarRegistroFuturos() As String
        Dim strMensaje As String = ""
        If Not IsNumeric(tbMargenInicial.Text) Then
            strMensaje = strMensaje + "\t-Margen Inicial\n"
        End If
        If Not IsNumeric(tbMargenMnto.Text) Then
            strMensaje = strMensaje + "\t-Margen Mantenimiento\n"
        End If
        If Not IsNumeric(tbContractSize.Text) Then
            strMensaje = strMensaje + "\t-Contract Size"
        End If
        If (strMensaje <> "") Then

            strMensaje = "Formato Numerico Incorrecto \n" + strMensaje + "\n"
            Return strMensaje
        Else
            Return ""
        End If
    End Function
    Private Function CrearObjetoLA() As LiquidezAccionBE
        Dim oLiquidezAccionBE As New LiquidezAccionBE
        Dim oRowLA As LiquidezAccionBE.LiquidezAccionRow
        oRowLA = CType(oLiquidezAccionBE.LiquidezAccion.NewRow(), LiquidezAccionBE.LiquidezAccionRow)
        oRowLA.CodigoMnemonico = tbMnemonico.Text
        oRowLA.CriterioLiquidez = ddlTipoLiquidez.SelectedValue
        oRowLA.Situacion = ParametrosSIT.ESTADO_ACTIVO
        oRowLA.DescripcionMnemonico = tbDescripcion.Text
        oLiquidezAccionBE.LiquidezAccion.AddLiquidezAccionRow(oRowLA)
        oLiquidezAccionBE.AcceptChanges()
        Return oLiquidezAccionBE
    End Function
    Private Sub GrabarCuponeraNormal()
        Dim oCuponera As New CuponeraBM
        If Session("accionValor") = "INGRESAR" And CType(Session("cambioCuponera"), Boolean) Then
            oCuponera.RegistrarCuponeraNormal(tbMnemonico.Text, Session("cuponeraNormal"), DatosRequest, _
                                              hdTipoTasaVariable.Value, hdTasaVariable.Value, hdDiasTTasaVariable.Value)
        ElseIf Session("accionValor") = "MODIFICAR" And CType(Session("cambioCuponera"), Boolean) Then
            oCuponera.EliminarCuponeraNormal(tbMnemonico.Text, DatosRequest)
            oCuponera.RegistrarCuponeraNormal(tbMnemonico.Text, Session("cuponeraNormal"), DatosRequest, _
                                              hdTipoTasaVariable.Value, hdTasaVariable.Value, hdDiasTTasaVariable.Value)
        End If
        If Not Session("cuponNormal_Eliminados") Is Nothing Then
            oCuponera.EliminarCuponeraNormal_Cupon(Session("cuponNormal_Eliminados"), DatosRequest)
        End If
    End Sub
    Private Sub ValoresParaGuardarCuponDescuento()
        hdTipoTasaVariable.Value = String.Empty
        hdTasaVariable.Value = "0"
        hdDiasTTasaVariable.Value = "0"
        Session("cambioCuponera") = True
    End Sub
    Private Sub GrabarCuponeraEspecial()
        Dim oCuponera As New CuponeraBM
        If Session("accionValor") = "INGRESAR" Then
            oCuponera.RegistrarCuponeraEspecial(tbMnemonico.Text, Session("cuponeraEspecial"), DatosRequest)
        ElseIf Session("accionValor") = "MODIFICAR" Then
            oCuponera.EliminarCuponeraEspecial(tbMnemonico.Text, DatosRequest)
            oCuponera.RegistrarCuponeraEspecial(tbMnemonico.Text, Session("cuponeraEspecial"), DatosRequest)
        End If
        If Not Session("cuponEspecial_Eliminados") Is Nothing Then
            oCuponera.EliminarCuponeraEspecial_Cupon(Session("cuponEspecial_Eliminados"), DatosRequest)
        End If
    End Sub
    Private Function verificarAgrupacion() As Boolean
        If ddlAgrupacion.SelectedValue = "E" Then
            If Session("DetalleAgrupacionE") Is Nothing Then
                Return False
            End If
        ElseIf ddlAgrupacion.SelectedValue = "C" Then
            If Session("DetalleAgrupacionC") Is Nothing Then
                Return False
            End If
        End If
        Return True
    End Function
    Private Function verificarCantidadAgrupacion() As Boolean
        If ddlAgrupacion.SelectedValue = "E" Then
            If CType(Session("DetalleAgrupacionE"), DataTable).Rows.Count >= 1 Then
                Return True
            End If
        ElseIf ddlAgrupacion.SelectedValue = "C" Then
            If CType(Session("DetalleAgrupacionC"), DataTable).Rows.Count >= 1 Then
                Return True
            End If
        End If
        Return False
    End Function
    Private Function verificarAgrupacionGrabada(ByVal CodigoMnemonico As String) As Boolean
        Dim dtAuxAgrupacion As DataTable = New DataTable
        If ddlAgrupacion.SelectedValue = "E" Then
            Dim oInsEstBM As New InstrumentosEstructuradosBM
            dtAuxAgrupacion = oInsEstBM.SeleccionarInstrumentosEstructurados(CodigoMnemonico, DatosRequest).Tables(0)
        ElseIf ddlAgrupacion.SelectedValue = "C" Then
            Dim oInsComBM As New InstrumentosCompuestosBM
            dtAuxAgrupacion = oInsComBM.SeleccionarInstrumentosCompuestos(CodigoMnemonico, DatosRequest).Tables(0)
        End If
        If dtAuxAgrupacion.Rows.Count >= 1 Then
            Return False
        End If
        Return True
    End Function
    Private Function VerificarCustodia(ByVal CodigoMnemonico As String) As Boolean
        Dim oValoresBM As New ValoresBM
        Dim LngContarCustodia As Long
        If Session("Custodios") = True Then
            Return True
        End If
        LngContarCustodia = oValoresBM.VerificarCustodia(CodigoMnemonico)
        If LngContarCustodia > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function VerificarCuponeraNormal(ByVal CodigoMnemonico As String) As Boolean
        Dim oValoresBM As New ValoresBM
        Dim LngContarCustodia As Long
        LngContarCustodia = oValoresBM.VerificarCuponeraNormal(CodigoMnemonico)
        If LngContarCustodia > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function VerificarCuponeraEspecial(ByVal CodigoMnemonico As String) As Boolean
        Dim oValoresBM As New ValoresBM
        Dim LngContarCustodia As Long
        LngContarCustodia = oValoresBM.VerificarCuponeraEspecial(CodigoMnemonico)
        If LngContarCustodia > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub GrabarAgrupacion()
        If ddlAgrupacion.SelectedValue = "E" Then
            Dim oInsEst As New InstrumentosEstructuradosBM
            If Not Session("DetalleAgrupacionE") Is Nothing Then
                oInsEst.IngresarModificarInstrumentosEstructurados(tbMnemonico.Text, CType(Session("DetalleAgrupacionE"), DataTable), DatosRequest)
            End If
        ElseIf ddlAgrupacion.SelectedValue = "C" Then
            Dim oInsCom As New InstrumentosCompuestosBM
            If Not Session("DetalleAgrupacionC") Is Nothing Then
                oInsCom.IngresarModificarInstrumentosCompuestos(tbMnemonico.Text, CType(Session("DetalleAgrupacionC"), DataTable), DatosRequest)
            End If
        End If
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Salir("Retorno")
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación", "CerrarLoading();")
        End Try
    End Sub
    Private Sub Salir(ByVal TipSalida As String)
        Dim retorno As String = String.Empty
        If TipSalida = "Grabacion" Then
            retorno = "yes"
        End If
        Session("DetalleAgrupacionE") = Nothing
        Session("DetalleAgrupacionC") = Nothing
        Session("Custodios") = Nothing
        Session("cuponeraNormal") = Nothing
        Session("cuponeraEspecial") = Nothing
        Session("sCuponeraNormalValoresSesion") = Nothing
        If Not Request.QueryString("vOI") Is Nothing Then
            EjecutarJS("window.close();")
        Else
            Response.Redirect("frmBusquedaValores.aspx?nemonico=" + hdnNemonico.Value + "&tipoRenta=" + hdnTipoRenta.Value + "&retorno=" + retorno, False)
        End If
    End Sub
    Private Sub btnCustodios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustodios.Click
        Try
            Dim strAux As String = "NO"
            If Not Request.QueryString("vOI") Is Nothing Then
                strAux = "YES"
            End If
            'Indica que el mantenimiento de Custodio Valores se abrirá en modo consulta cuando se abre desde la opción de Aprobación de Instrumentos Financieros
            If Request.QueryString("accionValor") = "Consulta_Aprobacion_Instrumento" Then
                strAux = "YES"
            End If
            EjecutarJS(UIUtility.MostrarPopUp("frmIngresoCustodios.aspx?vISIN=" + tbCodigoISIN.Text.Trim.ToUpper + "&vMnemonico=" + tbMnemonico.Text.Trim.ToUpper + "&vSBS=" + tbCodigoSBS.Text.Trim.ToUpper + "&vReadOnly=" + strAux, "yes", 769, 500, 150, 50, "no", "no", "yes", "yes"), False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación", "CerrarLoading();")
        End Try
    End Sub
    Private Sub ddlTipoRenta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoRenta.SelectedIndexChanged
        Try
            If ddlTipoRenta.SelectedIndex <> 0 Then
                CargaTipoTituloDeTipoRenta()
                If (ddlTipoRenta.SelectedValue.ToString = UIUtility.ObtenerCodigoTipoRenta("RFIJ")) Then
                    lblTipoRentaFija.Visible = True
                    btnCuponNormal.Visible = True
                    tbFechaPrimerCupon.Text = ""
                    tbFechaVencimiento.Text = ""
                    ddlTipoRentaFija.Visible = True
                    'OTXXXX - 23/09/2018 - Ian Pastor M.
                    'hdnCodigoClaseInstrumento.Value = "11" => Representa a la clase de instrumento "Letras Hipotecarias"
                    If (Trim(tbCodigoSBSinst.Text) = "52") Or (Trim(tbCodigoSBSinst.Text) = "08" And Trim(hdEmisor.Value) = "4000" And ddlTipoCupon.SelectedValue = "3") Or (hdnCodigoClaseInstrumento.Value = "11") _
                       Or (Trim(tbCodigoSBSinst.Text) = "100" Or Trim(tbCodigoSBSinst.Text) = "101") Then
                        ViewState("CUPONERA_REQUERIDO") = "N"
                        btnCuponNormal.Visible = False
                        btnCuponNormal.Enabled = False
                    End If
                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se modifica procedimiento agregando input flag para detectar el origen del llamado de procedimiento HabilitaRentaFija(Bool)| 17/05/18
                    HabilitaRentaFija(False)
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se modifica procedimiento agregando input flag para detectar el origen del llamado de procedimiento HabilitaRentaFija(Bool)| 17/05/18
                Else
                    lblTipoRentaFija.Visible = False
                    btnCuponNormal.Visible = False
                    ddlTipoRentaFija.Visible = False
                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se modifica procedimiento agregando input flag para detectar el origen del llamado de procedimiento HabilitaRentaVariable(Bool)| 17/05/18
                    If (ddlTipoRenta.SelectedValue.ToString = UIUtility.ObtenerCodigoTipoRenta("RVAR")) Then HabilitaRentaVariable(False)
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se modifica procedimiento agregando input flag para detectar el origen del llamado de procedimiento HabilitaRentaVariable(Bool)| 17/05/18
                End If


                ddlAgrupacion.SelectedValue = "S"
                Session("cuponeraEspecial") = Nothing
                Session("cuponeraNormal") = Nothing
                If ddlTipoRenta.SelectedValue = "2" Or ddlTipoRenta.SelectedValue = "3" Then
                    ddlRating.SelectedValue = ParametrosSIT.RATING_NR

                    ddlRatingM.SelectedValue = ParametrosSIT.RATING_NR
                Else
                    ddlRating.SelectedValue = Constantes.M_STR_TEXTO_INICIAL

                    ddlRatingM.SelectedValue = Constantes.M_STR_TEXTO_INICIAL
                End If
                ObtenerTipoFactor(ddlTipoRenta.SelectedValue.ToString)
            Else
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se implementa en procedimiento la habilitación (Visibilidad) de campos para renta fija | 17/05/18
                fdSeccionCentral.Attributes.Add("style", "display:none")
                fdSeccionInferior.Attributes.Add("style", "display:none")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se implementa en procedimiento la habilitación (Visibilidad) de campos para renta fija | 17/05/18

                LimpiarCampos()
                BloqueaTodo()
                ddlTipoRenta.Enabled = True
                ddlPortafolio.Enabled = True
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar", "CerrarLoading();")
        End Try
    End Sub
    Private Sub CargaTipoTituloDeTipoRenta()
        Dim dtTipoTitulo As DataTable
        Dim dtAux As New DataTable
        Dim i As Integer
        dtTipoTitulo = Session("tipoTitulo")
        dtAux = dtTipoTitulo.Clone
        For i = 0 To dtTipoTitulo.Rows.Count - 1
            If CType(dtTipoTitulo.Rows(i)("CodigoTipoRenta"), String) = ddlTipoRenta.SelectedValue Then
                dtAux.ImportRow(dtTipoTitulo.Rows(i))
            End If
        Next
        HelpCombo.LlenarComboBox(ddlTipoTitulo, dtAux, "CodigoTipoTitulo", "CodigoTipoTitulo", False)
        ddlTipoTitulo.Items.Insert(0, New ListItem("Especial", ""))
        BloqueaTodo()
        ddlTipoRenta.Enabled = True
        ddlPortafolio.Enabled = True
        If ddlTipoRenta.SelectedValue.ToUpper = UIUtility.ObtenerCodigoTipoRenta("RFIJ") Then
            HabilitaRentaFija(True)
            DatosPorDefectoRF()
        ElseIf ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RVAR") Then 'renta variable
            HabilitaRentaVariable(True)
            DatosPorDefectoRV()
        ElseIf ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RDER") Then
            HabilitaRentaDerivados()
        End If
    End Sub
    Private Sub DatosPorDefectoRF()
        Dim oUtil As New UtilDM
        ddlAgrupacion.SelectedValue = ""
        tbValorUnitario.Text = ""
        tbFechaEmision.Text = oUtil.RetornarFechaSistema()
        tbFechaPrimerCupon.Text = tbFechaEmision.Text
    End Sub
    Private Sub DatosPorDefectoRV()
        ddlAgrupacion.SelectedValue = "S"
        tbValorUnitario.Text = "1.0000000"
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se modifica procedimiento agregando input flag para detectar el origen del llamado de procedimiento HabilitaRentaFija(Bool)| 17/05/18
    Private Sub HabilitaRentaFija(ByVal ConCarga As Boolean)
        If ConCarga Then
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se modifica procedimiento agregando input flag para detectar el origen del llamado de procedimiento HabilitaRentaFija(Bool)| 17/05/18
            tbMnemonico.ReadOnly = False
            tbDescripcion.ReadOnly = False
            ddlAgrupacion.Enabled = True
            ddlTipoTitulo.Enabled = True
            imbEmisor.Visible = True
            ddlMoneda.Enabled = True
            tbNumUnidades.ReadOnly = False
            tbValorUnitario.ReadOnly = False
            tbValorNominal.ReadOnly = False
            tbValorEfecColocado.ReadOnly = False
            tbCodigoISIN.ReadOnly = False
            tbCodigoSBS.ReadOnly = False
            ddlCalificacion.Enabled = True
            tbFechaEmision.ReadOnly = False
            tbFechaVencimiento.ReadOnly = False
            tbFechaPrimerCupon.ReadOnly = False
            'tbTasaCupon.ReadOnly = False
            ddlPeriodicidad.Enabled = True
            ddlCotizacionVAC.Enabled = True
            tbTasaSpread.ReadOnly = False
            ddlTipoAmortizacion.Enabled = True
            ddlSituacion.Enabled = True
            ddlCuponBase.Enabled = True
            ddlCuponNDias.Enabled = True
            tbObservaciones.ReadOnly = False
            btnCustodios.Enabled = True
            btnCustodios.Visible = True
            btnAceptar.Enabled = True
            dvDiv121.Attributes.Add("class", "input-append date")
            dvDiv221.Attributes.Add("class", "input-append date")
            dvDiv321.Attributes.Add("class", "input-append date")

            If hdnCodigoClaseInstrumento.Value = "11" Or (Trim(tbCodigoSBSinst.Text) = "100" Or Trim(tbCodigoSBSinst.Text) = "101") Then 'Letras Hipotecarias
                ddlTipoCupon.Enabled = False
                tbTasaCupon.ReadOnly = True
                ddlTipoAmortizacion.Enabled = False
            Else
                ddlTipoCupon.Enabled = True
                tbTasaCupon.ReadOnly = False
                ddlTipoAmortizacion.Enabled = True
            End If

            ddlGrupoContable.Enabled = True
            ddlRating.Enabled = True

            ddlRatingM.Enabled = True
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se habilitan los campos cuando se cambia de tipo de renta | 25/05/18
            chkPagaDividendo.Checked = False
            ddlMercado.Enabled = True
            ddlTipoCodigoValor.Enabled = True
            txtCodigoSBSCorrelativo.Enabled = True
            ddlTipoRentaRiesgo.Enabled = True
            chkPagaDividendo.Enabled = True
            ddlFrecuenciaPago.Enabled = True
            chkBaseIC.Enabled = True
            ddlIntCorrBase.Enabled = True
            ddlIntCorrNDias.Enabled = True
            ddlPeriodicidad.Enabled = True
            chkGeneraIntereses.Enabled = True
            imbTipoInstrumento.Visible = True
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se habilitan los campos cuando se cambia de tipo de renta | 25/05/18

            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se implementa en procedimiento para cargar por default con check el nuevo flag de Base interes corrido| 15/06/18
            chkBaseIC.Checked = True
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se implementa en procedimiento para cargar por default con check el nuevo flag de Base interes corrido| 15/06/18
            chkSubordinario.Enabled = True
            chkPrecioDevengado.Enabled = True
        End If
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se implementa en procedimiento la habilitación (Visibilidad) de campos para renta fija | 17/05/18
        fdSeccionCentral.Attributes.Add("style", "display:block")
        fdSeccionInferior.Attributes.Add("style", "display:block")
        divCodigoMnemonico.Attributes.Add("style", "display:block")
        divMercado.Attributes.Add("style", "display:block")
        divDescripcion.Attributes.Add("style", "display:block")
        divRating.Attributes.Add("style", "display:block")
        divNumeroUnidades.Attributes.Add("style", "display:block")
        divTipoInstrumento.Attributes.Add("style", "display:block")
        divValorUnitario.Attributes.Add("style", "display:block")
        divEmisor.Attributes.Add("style", "display:block")
        divValorNominal.Attributes.Add("style", "display:block")
        divMoneda.Attributes.Add("style", "display:block")
        divValorEfectivoColocado.Attributes.Add("style", "display:block")
        divCodigoIsin.Attributes.Add("style", "display:block")
        divFechaEmision.Attributes.Add("style", "display:block")
        divCodigoSBS.Attributes.Add("style", "display:block")
        divFechaVencimiento.Attributes.Add("style", "display:block")
        divTipoCupon.Attributes.Add("style", "display:block")
        divSubordinario.Attributes.Add("style", "display:block")
        divPrecioDevengado.Attributes.Add("style", "display:block")

        If hdnCodigoClaseInstrumento.Value = "11" Or (Trim(tbCodigoSBSinst.Text) = "100" Or Trim(tbCodigoSBSinst.Text) = "101") Then 'Letras Hipotecarias y TBills
            divPrimerVctoCupon.Attributes.Add("style", "display:none")
            divVacio.Attributes.Add("style", "display:block")
            divPeriocidad.Attributes.Add("style", "display:none")
            divIndicadores.Attributes.Add("style", "display:none")
            ddlTipoCupon.Enabled = False
            tbTasaCupon.ReadOnly = True
            ddlTipoAmortizacion.Enabled = False
        Else
            divPrimerVctoCupon.Attributes.Add("style", "display:block")
            divVacio.Attributes.Add("style", "display:none")
            divPeriocidad.Attributes.Add("style", "display:block")
            divIndicadores.Attributes.Add("style", "display:block")
            ddlTipoAmortizacion.Enabled = True
        End If

        divTasaCupon.Attributes.Add("style", "display:block")
        divTipoAmortizacion.Attributes.Add("style", "display:block")
        'divPeriocidad.Attributes.Add("style", "display:block")
        divTipoCodigoValor.Attributes.Add("style", "display:block")
        'divIndicadores.Attributes.Add("style", "display:block")
        divGeneraInteres.Attributes.Add("style", "display:block")
        divTituloBaseCupon.Attributes.Add("style", "display:block")
        divDiasAño.Attributes.Add("style", "display:block")
        divBaseCupon.Attributes.Add("style", "display:block")
        divRowFechaEmisionTipoCupon.Attributes.Add("style", "display:block")
        divRowFechaVencimientoTasaCupon.Attributes.Add("style", "display:block")
        divRowPrimerVctoCuponPeriocidad.Attributes.Add("style", "display:block")
        divRowTipoAmortizacionIndicadores.Attributes.Add("style", "display:block")
        divRowGeneraInteres.Attributes.Add("style", "display:block")
        divPagaDividendo.Attributes.Add("style", "display:none")
        divRowFrecuenciaPago.Attributes.Add("style", "display:none")
        divBaseNDiasIC.Attributes.Add("style", "display:block")
        divBaseIC.Attributes.Add("style", "display:block")
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se implementa en procedimiento la habilitación (Visibilidad) de campos para renta fija | 17/05/18
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se modifica procedimiento agregando input flag para detectar el origen del llamado de procedimiento HabilitaRentaVariable(Bool)| 17/05/18
    Private Sub HabilitaRentaVariable(ByVal conCarga As Boolean)
        If conCarga Then
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se modifica procedimiento agregando input flag para detectar el origen del llamado de procedimiento HabilitaRentaVariable(Bool)| 17/05/18
            tbMnemonico.ReadOnly = False
            tbDescripcion.ReadOnly = False
            ddlAgrupacion.Enabled = True
            ddlTipoTitulo.Enabled = True
            imbEmisor.Visible = True
            ddlMoneda.Enabled = True
            tbNumUnidades.ReadOnly = False
            tbValorUnitario.ReadOnly = False
            tbValorNominal.ReadOnly = False
            tbValorEfecColocado.ReadOnly = False
            tbCodigoISIN.ReadOnly = False
            tbCodigoSBS.ReadOnly = False
            ddlCalificacion.Enabled = True
            tbFechaEmision.ReadOnly = False
            tbFechaVencimiento.ReadOnly = False
            tbFechaPrimerCupon.ReadOnly = False
            ddlBursatilidad.Enabled = True
            ddlSituacion.Enabled = True
            tbObservaciones.ReadOnly = False
            btnCustodios.Enabled = True
            btnCustodios.Visible = True
            btnAceptar.Enabled = True
            btnCuponNormal.Visible = False
            lblTipoRentaFija.Visible = False
            dvDiv121.Attributes.Add("class", "input-append date")
            dvDiv221.Attributes.Add("class", "input-append date")
            dvDiv321.Attributes.Add("class", "input-append date")
            ddlCuponBase.Enabled = False
            ddlCuponNDias.Enabled = False
            ddlGrupoContable.Enabled = True
            ddlRating.Enabled = True

            ddlRatingM.Enabled = True


            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se habilitan los campos cuando se cambia de tipo de renta | 25/05/18
            chkPagaDividendo.Checked = False
            ddlMercado.Enabled = True
            ddlTipoCodigoValor.Enabled = True
            txtCodigoSBSCorrelativo.Enabled = True
            ddlTipoRentaRiesgo.Enabled = True
            chkPagaDividendo.Enabled = True
            ddlFrecuenciaPago.Enabled = True
            chkBaseIC.Enabled = True
            ddlIntCorrBase.Enabled = True
            ddlIntCorrNDias.Enabled = True
            ddlPeriodicidad.Enabled = True
            chkGeneraIntereses.Enabled = True
            imbTipoInstrumento.Visible = True
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se habilitan los campos cuando se cambia de tipo de renta | 25/05/18

            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se implementa en procedimiento para cargar por default sin check el nuevo flag de Base interes corrido| 22/05/18
            chkBaseIC.Checked = False
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se implementa en procedimiento para cargar por default sin check el nuevo flag de Base interes corrido| 22/05/18
            chkSubordinario.Enabled = True
            chkPrecioDevengado.Enabled = True
        End If
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se implementa en procedimiento la habilitación (Visibilidad) de campos para renta variable| 17/05/18
        fdSeccionCentral.Attributes.Add("style", "display:block")
        fdSeccionInferior.Attributes.Add("style", "display:none")
        divCodigoMnemonico.Attributes.Add("style", "display:block")
        divMercado.Attributes.Add("style", "display:block")
        divDescripcion.Attributes.Add("style", "display:block")
        divRating.Attributes.Add("style", "display:none")
        divNumeroUnidades.Attributes.Add("style", "display:block")
        divTipoInstrumento.Attributes.Add("style", "display:block")
        divValorUnitario.Attributes.Add("style", "display:block")
        divEmisor.Attributes.Add("style", "display:block")
        divValorNominal.Attributes.Add("style", "display:block")
        divMoneda.Attributes.Add("style", "display:block")
        divValorEfectivoColocado.Attributes.Add("style", "display:block")
        divSubordinario.Attributes.Add("style", "display:none")
        divPrecioDevengado.Attributes.Add("style", "display:none")
        divCodigoIsin.Attributes.Add("style", "display:block")
        If tbCodigoSBSinst.Text = "49" Then
            divFechaEmision.Attributes.Add("style", "display:block")
            divFechaVencimiento.Attributes.Add("style", "display:block")
        Else
            divFechaEmision.Attributes.Add("style", "display:none")
            divFechaVencimiento.Attributes.Add("style", "display:none")
        End If
        divCodigoSBS.Attributes.Add("style", "display:block")
        divTipoCupon.Attributes.Add("style", "display:none")
        divPrimerVctoCupon.Attributes.Add("style", "display:none")
        divVacio.Attributes.Add("style", "display:none")
        divTasaCupon.Attributes.Add("style", "display:none")
        divTipoAmortizacion.Attributes.Add("style", "display:none")
        divPeriocidad.Attributes.Add("style", "display:none")
        divTipoCodigoValor.Attributes.Add("style", "display:block")
        divIndicadores.Attributes.Add("style", "display:none")
        divGeneraInteres.Attributes.Add("style", "display:none")
        divTituloBaseCupon.Attributes.Add("style", "display:none")
        divDiasAño.Attributes.Add("style", "display:none")
        divBaseCupon.Attributes.Add("style", "display:none")
        If tbCodigoSBSinst.Text = "49" Then
            divRowFechaEmisionTipoCupon.Attributes.Add("style", "display:block")
            divRowFechaVencimientoTasaCupon.Attributes.Add("style", "display:block")
        Else
            divRowFechaEmisionTipoCupon.Attributes.Add("style", "display:none")
            divRowFechaVencimientoTasaCupon.Attributes.Add("style", "display:none")
        End If
        divRowPrimerVctoCuponPeriocidad.Attributes.Add("style", "display:none")
        divRowTipoAmortizacionIndicadores.Attributes.Add("style", "display:none")
        divRowGeneraInteres.Attributes.Add("style", "display:none")
        divPagaDividendo.Attributes.Add("style", "display:block")
        divRowFrecuenciaPago.Attributes.Add("style", "display:none")
        divBaseNDiasIC.Attributes.Add("style", "display:none")
        divBaseIC.Attributes.Add("style", "display:none")
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se implementa en procedimiento la habilitación (Visibilidad) de campos para renta variable| 17/05/18
    End Sub
    Private Sub HabilitaRentaDerivados()
        tbMnemonico.ReadOnly = False
        tbDescripcion.ReadOnly = False
        ddlAgrupacion.Enabled = True
        ddlTipoTitulo.Enabled = True
        imbEmisor.Visible = True
        ddlMoneda.Enabled = True
        tbNumUnidades.ReadOnly = False
        tbValorUnitario.ReadOnly = False
        tbValorNominal.ReadOnly = False
        tbValorEfecColocado.ReadOnly = False
        tbCodigoISIN.ReadOnly = False
        tbCodigoSBS.ReadOnly = False
        ddlCalificacion.Enabled = True
        tbFechaEmision.ReadOnly = False
        tbFechaVencimiento.ReadOnly = False
        tbFechaPrimerCupon.ReadOnly = False
        tbTasaCupon.ReadOnly = False
        ddlPeriodicidad.Enabled = True
        ddlCotizacionVAC.Enabled = True
        tbTasaSpread.ReadOnly = False
        ddlTipoAmortizacion.Enabled = True
        ddlSituacion.Enabled = True
        ddlCuponBase.Enabled = True
        ddlCuponNDias.Enabled = True
        tbObservaciones.ReadOnly = False
        btnCustodios.Enabled = True
        btnCustodios.Visible = True
        btnAceptar.Enabled = True
        dvDiv121.Attributes.Add("class", "input-append date")
        dvDiv221.Attributes.Add("class", "input-append date")
        dvDiv321.Attributes.Add("class", "input-append date")
        ddlTipoCupon.Enabled = True
        ddlBursatilidad.Enabled = True
        btnCuponNormal.Visible = False
        lblTipoRentaFija.Visible = False
        ddlGrupoContable.Enabled = True
        ddlRating.Enabled = True

        ddlRatingM.Enabled = True
    End Sub
    Private Sub HabilitaDividendosFuturos(ByVal habilita As Boolean)
        If habilita Then
            trMargenes.Visible = True
            trContractSize.Visible = True
            tbNumUnidades.ReadOnly = True
            tbValorUnitario.ReadOnly = True
            tbValorNominal.ReadOnly = True
            tbValorEfecColocado.ReadOnly = True
            tbTasaCupon.ReadOnly = True
            tbTasaSpread.ReadOnly = True
            tbObservaciones.ReadOnly = True
            ddlAgrupacion.Enabled = False
            ddlTipoTitulo.Enabled = False
            imbEmisor.Visible = False
            ddlCalificacion.Enabled = False
            ddlPeriodicidad.Enabled = False
            ddlCotizacionVAC.Enabled = False
            ddlTipoAmortizacion.Enabled = False
            ddlCuponBase.Enabled = False
            ddlCuponNDias.Enabled = False
            btnCustodios.Enabled = False
            btnCustodios.Visible = False
            dvDiv121.Attributes.Add("class", "input-append")
            dvDiv221.Attributes.Add("class", "input-append")
            dvDiv321.Attributes.Add("class", "input-append")
            btnAceptar.Enabled = True
            btnSalir.Enabled = True
            ddlTipoCupon.Enabled = False
            ddlBursatilidad.Enabled = False
            ddlRating.Enabled = False

            ddlRatingM.Enabled = False

            tbFechaEmision.Enabled = False
            tbFechaVencimiento.Enabled = False
            tbFechaPrimerCupon.Enabled = False
            tbFechaLiberada.Enabled = False
            tbFactor.Enabled = False
        Else
            trMargenes.Visible = False
            trContractSize.Visible = False
            tbNumUnidades.ReadOnly = False
            tbValorUnitario.ReadOnly = False
            tbValorNominal.ReadOnly = False
            tbValorEfecColocado.ReadOnly = False
            tbTasaCupon.ReadOnly = False
            tbTasaSpread.ReadOnly = False
            tbObservaciones.ReadOnly = False
            ddlAgrupacion.Enabled = True
            ddlTipoTitulo.Enabled = True
            imbEmisor.Visible = True
            ddlCalificacion.Enabled = True
            ddlPeriodicidad.Enabled = True
            ddlCotizacionVAC.Enabled = True
            ddlTipoAmortizacion.Enabled = True
            ddlCuponBase.Enabled = True
            ddlCuponNDias.Enabled = True
            btnCustodios.Enabled = True
            btnCustodios.Visible = True
            dvDiv121.Attributes.Add("class", "input-append date")
            dvDiv221.Attributes.Add("class", "input-append date")
            dvDiv321.Attributes.Add("class", "input-append date")
            ddlTipoCupon.Enabled = True
            ddlBursatilidad.Enabled = True
            ddlRating.Enabled = True

            ddlRatingM.Enabled = True

            tbFechaEmision.Enabled = True
            tbFechaVencimiento.Enabled = True
            tbFechaPrimerCupon.Enabled = True
            tbFechaLiberada.Enabled = True
            tbFactor.Enabled = True
        End If
    End Sub
    Private Sub HabilitaFondos()
        If tbCodigoSBSinst.Text = "51" Or tbCodigoSBSinst.Text = "53" Then
            dvDiv111.Attributes.Add("class", "hidden")
            dvDiv121.Attributes.Add("class", "hidden")
            dvDiv211.Attributes.Add("class", "hidden")
            dvDiv221.Attributes.Add("class", "hidden")
            dvDiv112.Attributes.Add("class", "col-sm-5 control-label")
            dvDiv212.Attributes.Add("class", "col-sm-5 control-label")
            dvDiv222.Attributes.Remove("class")
            trCategoria.Attributes.Add("class", "row")
        Else
            dvDiv111.Attributes.Add("class", "col-sm-5 control-label")
            dvDiv121.Attributes.Add("class", "input-append date")
            dvDiv211.Attributes.Add("class", "col-sm-5 control-label")
            dvDiv221.Attributes.Add("class", "input-append date")
            dvDiv311.Attributes.Add("class", "col-sm-5 control-label")
            dvDiv321.Attributes.Add("class", "input-append date")
            dvDiv112.Attributes.Add("class", "hidden")
            dvDiv212.Attributes.Add("class", "hidden")
            dvDiv222.Attributes.Add("class", "hidden")
            trCategoria.Attributes.Add("class", "hidden")
        End If
    End Sub
    Private Sub BloqueaTodo()
        tbMnemonico.ReadOnly = True
        ddlTipoTitulo.Enabled = False
        tbDescripcion.ReadOnly = True
        tbCodigoSBSinst.ReadOnly = True
        ddlAgrupacion.Enabled = False
        tbEmisor.ReadOnly = True
        ddlBursatilidad.Enabled = False
        ddlMoneda.Enabled = False
        tbNumUnidades.ReadOnly = True
        tbCodigoISIN.ReadOnly = True
        tbValorUnitario.ReadOnly = True
        tbCodigoSBS.ReadOnly = True
        tbValorNominal.ReadOnly = True
        ddlCalificacion.Enabled = False
        tbValorEfecColocado.ReadOnly = True
        tbTasaEncaje.ReadOnly = True
        tbFechaEmision.ReadOnly = True
        ddlTipoCupon.Enabled = False
        tbFechaVencimiento.ReadOnly = True
        tbTasaCupon.ReadOnly = True
        tbFechaPrimerCupon.ReadOnly = True
        ddlTipoAmortizacion.Enabled = False
        tbTasaSpread.ReadOnly = True
        ddlCotizacionVAC.Enabled = False
        ddlSituacion.Enabled = False
        ddlCuponBase.Enabled = False
        ddlCuponNDias.Enabled = False
        tbObservaciones.ReadOnly = True
        imbEmisor.Visible = False
        btnCustodios.Enabled = False
        btnCustodios.Visible = False
        btnCuponNormal.Visible = False
        btnAceptar.Enabled = False
        dvDiv121.Attributes.Add("class", "input-append")
        dvDiv221.Attributes.Add("class", "input-append")
        dvDiv321.Attributes.Add("class", "input-append")
        ddlTipoRenta.Enabled = False
        ddlPortafolio.Enabled = False
        ddlGrupoContable.Enabled = False
        ddlRating.Enabled = False

        ddlRatingM.Enabled = False

        tbFactor.Enabled = False

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se deshabilita los campos cuando se llama desde otra ventana| 25/05/18
        ddlMercado.Enabled = False
        ddlTipoCodigoValor.Enabled = False
        txtCodigoSBSCorrelativo.Enabled = False
        ddlTipoRentaRiesgo.Enabled = False
        chkPagaDividendo.Enabled = False
        ddlFrecuenciaPago.Enabled = False
        chkBaseIC.Enabled = False
        ddlIntCorrBase.Enabled = False
        ddlIntCorrNDias.Enabled = False
        ddlPeriodicidad.Enabled = False
        chkGeneraIntereses.Enabled = False
        imbTipoInstrumento.Visible = False
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se deshabilita los campos cuando se llama desde otra ventana| 25/05/18
        chkSubordinario.Enabled = False
        chkPrecioDevengado.Enabled = False
    End Sub
#End Region
#Region " /* Funciones Personalizadas*/"
    Public Sub CargarRegistro(ByVal CodigoMnemonico As String)
        ddlTipoRenta.Enabled = False
        Dim oValoresBM As New ValoresBM, oValoresBE As New ValoresBE, oRow As ValoresBE.ValorRow
        Dim StrUnidades As String, blnExisteCuponera As Boolean
        oValoresBE = oValoresBM.Seleccionar(CodigoMnemonico, DatosRequest)
        oRow = oValoresBE.Valor(0)
        If oRow.SituacionEmi.Equals("I") Then
            lbAlerta.Visible = True
            lbAlerta.Text = "El Emisor esta Inactivo."
        Else
            lbAlerta.Visible = False
        End If

        'INICIO | ZOLUXIONES | RCE | RU022 - Se agrega nuevo atributo a ValoresBE para campo Subordinario y Precio Devengado | 23/10/18
        If oRow.Subordinado.Equals("1") Then
            chkSubordinario.Checked = True
        End If
        If oRow.PrecioDevengado.Equals("1") Then
            chkPrecioDevengado.Checked = True
        End If

        'FIN | ZOLUXIONES | RCE | RU022 - Se agrega nuevo atributo a ValoresBE para campo Subordinario y Precio Devengado | 23/10/18


        ddlTipoCodigoValor.SelectedValue = oRow.TipoCodigoValor
        chkGeneraIntereses.Checked = IIf(oRow.GeneraInteres = "1", True, False)
        tbMnemonico.Text = oRow.CodigoNemonico
        If (ddlTipoTitulo.Items.FindByValue(oRow.CodigoTipoTitulo) Is Nothing) Then
            ddlTipoTitulo.SelectedValue = ""
            ddlPeriodicidad.Enabled = True
            ddlTipoAmortizacion.Enabled = True
            ddlCotizacionVAC.Enabled = True
            ddlTipoCupon.Enabled = True
            imbTipoInstrumento.Visible = True
        Else
            If oRow.CodigoTipoTitulo <> "" Then
                ddlTipoTitulo.SelectedValue = oRow.CodigoTipoTitulo
            End If
            If (oRow.CodigoTipoTitulo = "") Then
                ddlPeriodicidad.Enabled = True
                ddlTipoAmortizacion.Enabled = True
                ddlCotizacionVAC.Enabled = True
                ddlTipoCupon.Enabled = True
                imbTipoInstrumento.Visible = True
            End If
        End If
        ddlTipoRenta.SelectedValue = oRow.CodigoRenta
        hdnCodigoClaseInstrumento.Value = oRow.CodigoClaseInstrumento
        tbCodigoSBSinst.Text = oRow.CodigoTipoInstrumentoSBS
        If ddlTipoRenta.SelectedValue.ToUpper = UIUtility.ObtenerCodigoTipoRenta("RFIJ") Then
            HabilitaRentaFija(True)
        ElseIf ddlTipoRenta.SelectedValue.ToUpper = UIUtility.ObtenerCodigoTipoRenta("RVAR") Then    'renta variable
            HabilitaRentaVariable(True)
        ElseIf ddlTipoRenta.SelectedValue.ToUpper = UIUtility.ObtenerCodigoTipoRenta("RDER") Then
            If oRow.CodigoTipoInstrumentoSBS = ParametrosSIT.CODIGOSBS_FUTUROS Then
                HabilitaDividendosFuturos(True)
            Else
                HabilitaDividendosFuturos(False)
                HabilitaRentaDerivados()
            End If
        End If
        imbTipoInstrumento.Visible = True

        tbSinTipoInst.Visible = True
        tbSinTipoInst.Text = oRow.DescripcionTipoInstrumento
        ddlTipoRenta.SelectedValue = oRow.CodigoRenta
        'hdnCodigoClaseInstrumento.Value = oRow.CodigoClaseInstrumento
        HabilitaControlesWarrants(hdnCodigoClaseInstrumento.Value)
        If (oRow.CodigoRenta = 1) Then
            lblTipoRentaFija.Visible = True
            ddlTipoRentaFija.Visible = True
            If oRow.CodigoClaseInstrumento.Equals("1") Then
                blnExisteCuponera = ExisteCuponera()
                If Not blnExisteCuponera And oRow.CodigoTipoInstrumentoSBS <> "52" And Not (oRow.CodigoTipoInstrumentoSBS = "08" And Trim(oRow.CodigoEmisor) = "US-T" And oRow.CodigoTipoCupon.ToString() = "3") Then
                    AlertaJS(Constantes.M_STR_MENSAJE_FALTA_CUPONERA, "CerrarLoading();")
                End If
            End If
        Else
            lblTipoRentaFija.Visible = False
            ddlTipoRentaFija.Visible = False
        End If
        tbDescripcion.Text = oRow.Descripcion
        ddlAgrupacion.SelectedValue = oRow.Agrupacion
        tbEmisor.Text = oRow.CodigoEmisor
        ObtenerEmisorSBS()
        If oRow.CodigoBursatilidad <> "" Then
            ddlBursatilidad.SelectedValue = oRow.CodigoBursatilidad
        End If
        ddlMoneda.SelectedValue = oRow.CodigoMoneda
        If oRow.NumeroUnidades.ToString <> "" Then
            StrUnidades = oRow.NumeroUnidades.ToString
            tbNumUnidades.Text = Format(CType(StrUnidades, Decimal), "##,##0.0000000")
        End If
        tbCodigoISIN.Text = oRow.CodigoISIN
        If oRow.ValorUnitario.ToString <> "" Then
            tbValorUnitario.Text = Format(CType(oRow.ValorUnitario.ToString, Decimal), "##,##0.0000000")
        End If
        tbCodigoSBS.Text = Left(oRow.CodigoSBS, 7)
        txtCodigoSBSCorrelativo.Text = Right(oRow.CodigoSBS, 5)
        ActualizarCodigoSBSint()
        If oRow.ValorNominal.ToString <> "" Then
            tbValorNominal.Text = Format(CType(oRow.ValorNominal.ToString, Decimal), "##,##0.0000000")
        End If
        If oRow.ValorEfectivoColocado.ToString <> "" Then
            tbValorEfecColocado.Text = Format(CType(oRow.ValorEfectivoColocado, Decimal), "##,##0.0000000")
        End If
        If oRow.TasaEncaje.ToString <> "" Then
            tbTasaEncaje.Text = oRow.TasaEncaje.ToString.Replace(UIUtility.DecimalSeparator, ".")
        End If
        If oRow.CodigoTipoCupon.ToString <> "" Then
            ddlTipoCupon.SelectedValue = oRow.CodigoTipoCupon.ToString()
            imgInfoTipoCupon.Visible = (ddlTipoCupon.SelectedValue = ViewState(ParametrosSIT.TIPO_CUPON_A_DESCUENTO))
        End If
        If oRow.TipoRentaFija.ToString <> "" Then
            ddlTipoRentaFija.SelectedValue = oRow.TipoRentaFija
        End If
        tbFechaEmision.Text = UIUtility.ConvertirFechaaString(oRow.FechaEmision)
        tbFechaVencimiento.Text = UIUtility.ConvertirFechaaString(oRow.FechaVencimiento)
        tbFechaPrimerCupon.Text = UIUtility.ConvertirFechaaString(oRow.FechaPrimerCupon)
        ViewState("FechaEmision") = tbFechaEmision.Text
        ViewState("FechaVencimiento") = tbFechaVencimiento.Text
        ViewState("FechaPrimerCupon") = tbFechaPrimerCupon.Text
        If oRow.TasaCupon.ToString <> "" Then
            tbTasaCupon.Text = oRow.TasaCupon.ToString.Replace(UIUtility.DecimalSeparator, ".")
        End If
        ViewState("TasaCupon") = tbTasaCupon.Text
        If oRow.CodigoPeriodicidad.ToString <> "" Then
            ddlPeriodicidad.SelectedValue = oRow.CodigoPeriodicidad
        End If
        If oRow.CodigoTipoAmortizacion.ToString <> "" Then
            ddlTipoAmortizacion.SelectedValue = oRow.CodigoTipoAmortizacion
        End If
        If oRow.TasaSpread.ToString <> "" Then
            tbTasaSpread.Text = oRow.TasaSpread.ToString.Replace(UIUtility.DecimalSeparator, ".")
        End If
        ViewState("TasaSpread") = tbTasaSpread.Text
        If oRow.CodigoIndicador <> "" Then
            ddlCotizacionVAC.SelectedValue = oRow.CodigoIndicador
        End If
        ddlSituacion.SelectedValue = oRow.Situacion
        Session("vSituacion") = oRow.Situacion
        If oRow.BaseCupon.ToString <> "0" And oRow.BaseCupon.ToString <> "" Then
            ddlCuponBase.SelectedValue = oRow.BaseCupon
        End If
        If oRow.BaseCuponDias.ToString <> "0" And oRow.BaseCuponDias.ToString <> "" Then
            ddlCuponNDias.SelectedValue = oRow.BaseCuponDias
        End If
        If oRow.GrupoContable <> "" Then
            ddlGrupoContable.SelectedValue = oRow.GrupoContable
        End If
        Dim oTipoTituloBM As New TipoTituloBM
        tbTasaEncaje.Text = Format(Convert.ToDecimal(oTipoTituloBM.ObtenerTasaEncaje(ddlTipoTitulo.SelectedValue.ToString, tbMnemonico.Text.Trim, DatosRequest).ToString), "##,##0.0000000")
        tbObservaciones.Text = oRow.Observacion
        If ddlAgrupacion.SelectedValue = "E" Then
            hdCantidadIE.Value = oRow.cantidadIE.ToString.Replace(UIUtility.DecimalSeparator, ".")
            hdRentaFijaIE.Value = oRow.rentaFijaIE.ToString.Replace(UIUtility.DecimalSeparator, ".")
            hdRentaVarIE.Value = oRow.rentaVarIE.ToString.Replace(UIUtility.DecimalSeparator, ".")
            hdCodigoTipoDerivado.Value = oRow.CodigoTipoDerivado
            hdEmisorIE.Value = oRow.CodigoEmisorIE
        ElseIf ddlAgrupacion.SelectedValue = "C" Then
            hdRentaFijaIC.Value = oRow.rentaFijaIE.ToString.Replace(UIUtility.DecimalSeparator, ".")
            hdRentaVarIC.Value = oRow.rentaVarIE.ToString.Replace(UIUtility.DecimalSeparator, ".")
        End If
        tbPosicionAct.Text = oRow.PosicionAct
        tbPorcPosicion.Text = oRow.PorcPosicion
        If oRow.Categoria <> "" Then
            ddlCategoria.SelectedValue = oRow.Categoria
        End If
        CargarComboEstilo("EstiloCat", ddlCategoria.SelectedValue)
        If oRow.Estilo <> "" Then
            ddlEstilo.SelectedValue = oRow.Estilo
        End If
        tbFechaLiberada.Text = ""
        If oRow.FechaLiberada <> "0" Then
            tbFechaLiberada.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiberada.ToString)
        End If
        ObtenerFactor(ddlRating.SelectedValue)
        ObtenerTipoFactor(ddlTipoRenta.SelectedValue.ToString)
        ValidarRequerimientoCuponera(True)
        If oRow.CodigoTipoInstrumentoSBS = ParametrosSIT.CODIGOSBS_FUTUROS Then
            tbMargenInicial.Text = String.Format("{0:###,##0.0000000}", oRow.MargenInicial)
            tbMargenMnto.Text = String.Format("{0:###,##0.0000000}", oRow.MargenMantenimiento)
            tbContractSize.Text = String.Format("{0:###,##0.0000000}", oRow.ContractSize)
        End If
        ddlMercado.SelectedValue = oRow.CodigoMercado
        LlenaRating(ddlMercado.SelectedValue)
        If oRow.CodigoCalificacion.ToString <> "" Then
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se deshabilita ddlCalificacion ya que no se mostrará en formulario | 25/05/18
            '  ddlCalificacion.SelectedValue = oRow.CodigoCalificacion
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se deshabilita ddlCalificacion ya que no se mostrará en formulario | 25/05/18
        End If
        If oRow.Rating.ToString <> "" Then
            ddlRating.SelectedValue = oRow.Rating
        End If

        LlenaRatingMandato(ddlMercado.SelectedValue)
        If oRow.RatingMandato <> "" Then
            ddlRatingM.SelectedValue = oRow.RatingMandato
        End If


        txtPiso.Text = oRow.MontoPiso
        txtTecho.Text = oRow.MontoTecho
        txtGarante.Text = oRow.Garante
        txtSubyacente.Text = oRow.Subyacente
        txtPrecioEjercicio.Text = oRow.PrecioEjercicio
        txtTamanoEmision.Text = oRow.TamanoEmision
        If oRow.CodigoPortafolioSBS.ToString <> "" Then
            ddlPortafolio.SelectedValue = oRow.CodigoPortafolioSBS
        End If
        'OT 10090 - 21/03/2017 - Carlos Espejo
        'Descripcion: Se guarda el valor de la moneda en el Hidden
        hdCodigoSBSMoneda.Value = oRow.CodigoMonedaSBS
        'OT 10090 Fin

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega nuevo campo tipoRentaRiesgo | 17/05/18
        If oRow.TipoRentaRiesgo.ToString <> "" Then ddlTipoRentaRiesgo.SelectedValue = oRow.TipoRentaRiesgo
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega nuevo campo tipoRentaRiesgo | 17/05/18

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se carga nuevo campo Frecuencia de Pagos (Dividendos) | 18/05/18
        If oRow.CodigoFrecuenciaDividendo.ToString <> "" Then
            divRowFrecuenciaPago.Attributes.Add("style", "display:block")
            chkPagaDividendo.Checked = True
            ddlFrecuenciaPago.SelectedValue = oRow.CodigoFrecuenciaDividendo
        End If

        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se carga nuevo campo Frecuencia de Pagos (Dividendos) | 18/05/18

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se carga nuevo campo de base interes corrido | 22/05/18
        If oRow.EstadoBaseIC.ToString <> "0" Then
            chkBaseIC.Checked = True
            If (oRow.BaseInteresCorridoDias.ToString <> "0" And oRow.BaseInteresCorridoDias.ToString <> "") Then
                ddlIntCorrNDias.SelectedValue = oRow.BaseInteresCorridoDias.ToString
                divBaseNDiasIC.Attributes.Add("style", "display:block")
            End If
            If (oRow.BaseInteresCorrido.ToString <> "0" And oRow.BaseInteresCorrido.ToString <> "") Then
                ddlIntCorrBase.SelectedValue = oRow.BaseInteresCorrido.ToString
                divBaseIC.Attributes.Add("style", "display:block")
            End If
        End If
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se carga nuevo campo de base interes corrido | 22/05/18


    End Sub
    Public Function crearObjeto() As ValoresBE
        Dim oValoresBE As New ValoresBE
        Dim oRow As ValoresBE.ValorRow
        Dim oValoresBM As New ValoresBM
        oRow = CType(oValoresBE.Valor.NewRow(), ValoresBE.ValorRow)
        oValoresBM.InicializarValores(oRow)
        oRow.CodigoNemonico = tbMnemonico.Text
        oRow.CodigoTipoTitulo = ddlTipoTitulo.SelectedValue
        oRow.CodigoTipoInstrumentoSBS = tbCodigoSBSinst.Text.Trim
        oRow.Descripcion = tbDescripcion.Text
        oRow.Agrupacion = ddlAgrupacion.SelectedValue
        oRow.CodigoEmisor = tbEmisor.Text

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se setea valor por default: 4 - Mínimo en Renta Variable en campo Bursatibilidad| 18/05/18 
        oRow.CodigoBursatilidad = IIf(ddlTipoRenta.SelectedValue = "2", "4", ddlBursatilidad.SelectedValue)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se setea valor por default: 4 - Mínimo en Renta Variable en campo Bursatibilidad| 18/05/18 

        oRow.CodigoRenta = ddlTipoRenta.SelectedValue
        oRow.CodigoMoneda = ddlMoneda.SelectedItem.Text
        oRow.MonedaPago = ddlMoneda.SelectedValue
        oRow.MonedaCupon = ddlMoneda.SelectedValue
        oRow.NumeroUnidades = tbNumUnidades.Text
        oRow.TipoCuponera = "N"
        oRow.CodigoISIN = tbCodigoISIN.Text
        oRow.ValorUnitario = tbValorUnitario.Text
        oRow.CodigoSBS = tbCodigoSBS.Text + txtCodigoSBSCorrelativo.Text.Trim
        oRow.ValorNominal = tbValorNominal.Text

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se setea valor por default: 84 - Sin Clasf (I) en Renta Variable en campo Rating Interno | 18/05/18 
        If ddlMercado.SelectedValue = 1 Then
            oRow.CodigoCalificacion = IIf(ddlTipoRenta.SelectedValue = "2", "84", ddlCalificacion.SelectedValue)
        ElseIf ddlMercado.SelectedValue = 2 Then
            oRow.CodigoCalificacion = IIf(ddlTipoRenta.SelectedValue = "2", "87", ddlCalificacion.SelectedValue)
        End If
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se setea valor por default por default: 84 - Sin Clasf (I) en Renta Variable en campo Rating Interno | 18/05/18         oRow.ValorEfectivoColocado = tbValorEfecColocado.Text

        oRow.TipoRentaFija = ddlTipoRentaFija.SelectedValue
        oRow.TasaEncaje = IIf(tbTasaEncaje.Text = "", 0, tbTasaEncaje.Text)
        oRow.FechaEmision = UIUtility.ConvertirFechaaDecimal(tbFechaEmision.Text)
        If hdnCodigoClaseInstrumento.Value = "11" Or (Trim(hdCodigoSBSinst.Value) = "100" Or Trim(hdCodigoSBSinst.Value) = "101") Then
            oRow.CodigoTipoCupon = "3"
            oRow.TasaCupon = "0"
        Else
            oRow.CodigoTipoCupon = ddlTipoCupon.SelectedValue
            oRow.TasaCupon = IIf(tbTasaCupon.Text = "", "0", tbTasaCupon.Text)
        End If
        oRow.FechaVencimiento = UIUtility.ConvertirFechaaDecimal(tbFechaVencimiento.Text)
        oRow.FechaPrimerCupon = UIUtility.ConvertirFechaaDecimal(tbFechaPrimerCupon.Text)
        oRow.CodigoPeriodicidad = ddlPeriodicidad.SelectedValue
        oRow.CodigoTipoAmortizacion = ddlTipoAmortizacion.SelectedValue
        oRow.TasaSpread = IIf(tbTasaSpread.Text = "", "0", tbTasaSpread.Text)
        oRow.CodigoIndicador = ddlCotizacionVAC.SelectedValue
        oRow.Situacion = ddlSituacion.SelectedValue
        oRow.BaseCupon = IIf(ddlCuponBase.SelectedValue = "", "0", ddlCuponBase.SelectedValue)
        oRow.BaseCuponDias = IIf(ddlCuponNDias.SelectedValue = "", "0", ddlCuponNDias.SelectedValue)
        oRow.NemonicoTemporal = String.Empty
        oRow.Observacion = tbObservaciones.Text

        If ddlAgrupacion.SelectedValue = "E" Then
            If Not Session("agrupacionIE") Is Nothing Then
                Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
                Dim strAgrupacion() As String = CType(Session("agrupacionIE"), String).Split(strSeparador)
                hdCantidadIE.Value = strAgrupacion(0)
                hdRentaFijaIE.Value = strAgrupacion(1)
                hdRentaVarIE.Value = strAgrupacion(2)
                hdCodigoTipoDerivado.Value = strAgrupacion(3)
                hdEmisorIE.Value = strAgrupacion(4)
                oRow.cantidadIE = Convert.ToDecimal(hdCantidadIE.Value)
                oRow.rentaFijaIE = Convert.ToDecimal(hdRentaFijaIE.Value)
                oRow.rentaVarIE = Convert.ToDecimal(hdRentaVarIE.Value)
                oRow.CodigoTipoDerivado = hdCodigoTipoDerivado.Value
                oRow.CodigoEmisorIE = hdEmisorIE.Value
            Else
                If hdCantidadIE.Value > 0 Then
                    oRow.cantidadIE = Convert.ToDecimal(hdCantidadIE.Value)
                End If
                If hdRentaFijaIE.Value > 0 Then
                    oRow.rentaFijaIE = Convert.ToDecimal(hdRentaFijaIE.Value)
                End If
                If hdRentaVarIE.Value > 0 Then
                    oRow.rentaVarIE = Convert.ToDecimal(hdRentaVarIE.Value)
                End If
                oRow.CodigoTipoDerivado = hdCodigoTipoDerivado.Value
                oRow.CodigoEmisorIE = hdEmisorIE.Value
            End If
        ElseIf ddlAgrupacion.SelectedValue = "C" Then
            If Not Session("agrupacionIC") Is Nothing Then
                Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
                Dim strAgrupacion() As String = CType(Session("agrupacionIC"), String).Split(strSeparador)
                hdRentaFijaIC.Value = strAgrupacion(0)
                hdRentaVarIC.Value = strAgrupacion(1)
                oRow.rentaFijaIE = Convert.ToDecimal(hdRentaFijaIC.Value)
                oRow.rentaVarIE = Convert.ToDecimal(hdRentaVarIC.Value)
            End If
        End If

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se setea valor por default: 84 - Sin Clasf (I) en Renta Variable en campo Rating | 18/05/18 
        If ddlMercado.SelectedValue = 1 Then
            oRow.Rating = IIf(ddlTipoRenta.SelectedValue = "2", "84", ddlRating.SelectedValue)
        ElseIf ddlMercado.SelectedValue = 2 Then
            oRow.Rating = IIf(ddlTipoRenta.SelectedValue = "2", "87", ddlRating.SelectedValue)
        End If
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se setea valor por default: 84 - Sin Clasf (I) en Renta Variable en campo Rating | 18/05/18 

        'If ddlMercado.SelectedValue = 1 Then
        'oRow.RatingMandato = IIf(ddlTipoRenta.SelectedValue = "2", "84", ddlRatingM.SelectedValue)
        'ElseIf ddlMercado.SelectedValue = 2 Then
        'oRow.RatingMandato = IIf(ddlTipoRenta.SelectedValue = "2", "87", ddlRatingM.SelectedValue)
        'End If
        oRow.RatingMandato = ddlRatingM.SelectedValue



        oRow.PosicionAct = IIf(tbPosicionAct.Text = "", 0, tbPosicionAct.Text)
        oRow.PorcPosicion = IIf(tbPorcPosicion.Text = "", 0, tbPorcPosicion.Text)
        oRow.Categoria = ddlCategoria.SelectedValue.ToString()
        oRow.Estilo = ddlEstilo.SelectedValue
        If tbCodigoSBSinst.Text = ParametrosSIT.CODIGOSBS_FUTUROS Then
            If IsNumeric(tbMargenInicial.Text) Then
                oRow.MargenInicial = Convert.ToDecimal(tbMargenInicial.Text)
            End If
            If IsNumeric(tbMargenMnto.Text) Then
                oRow.MargenMantenimiento = Convert.ToDecimal(tbMargenMnto.Text)
            End If
            If IsNumeric(tbContractSize.Text) Then
                oRow.ContractSize = Convert.ToDecimal(tbContractSize.Text)
            End If
        End If
        'OT 10090 - 21/03/2017 - Carlos Espejo
        'Descripcion: Se guadar el valor de los controles Base y Base dias
        oRow.BaseIC = IIf(ddlCuponBase.SelectedValue = "", "0", ddlCuponBase.SelectedValue)
        oRow.BaseICDias = IIf(ddlCuponNDias.SelectedValue = "", "0", ddlCuponNDias.SelectedValue)
        'OT 10090 Fin
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se guardan los valores de los nuevos campos de de base interes corrido | 22/05/18 
        If chkBaseIC.Checked Then
            oRow.BaseInteresCorrido = IIf(ddlIntCorrBase.SelectedValue = "", "0", ddlIntCorrBase.SelectedValue)
            oRow.BaseInteresCorridoDias = IIf(ddlIntCorrNDias.SelectedValue = "", "0", ddlIntCorrNDias.SelectedValue)
            oRow.EstadoBaseIC = "1"
        Else
            oRow.BaseInteresCorrido = "0"
            oRow.BaseInteresCorridoDias = "0"
            oRow.EstadoBaseIC = "0"
        End If
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se guardan los valores de los nuevos campos de de base interes corrido | 22/05/18 

        oRow.CodigoMercado = ddlMercado.SelectedValue
        oRow.TipoCodigoValor = ddlTipoCodigoValor.SelectedValue
        oRow.GeneraInteres = IIf(chkGeneraIntereses.Checked = True, "1", "0")
        If txtPiso.Text = "" Then
            oRow.MontoPiso = 0
        Else
            oRow.MontoPiso = CDec(txtPiso.Text)
        End If
        If txtTecho.Text = "" Then
            oRow.MontoTecho = 0
        Else
            oRow.MontoTecho = CDec(txtTecho.Text)
        End If
        If txtPrecioEjercicio.Text = "" Then
            oRow.PrecioEjercicio = 0
        Else
            oRow.PrecioEjercicio = CDec(txtPrecioEjercicio.Text)
        End If
        If txtTamanoEmision.Text = "" Then
            oRow.TamanoEmision = 0
        Else
            oRow.TamanoEmision = CDec(txtTamanoEmision.Text)
        End If
        oRow.Garante = txtGarante.Text
        oRow.Subyacente = txtSubyacente.Text
        oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        oRow.CondicionImpuesto = ""
        'OT 10090 - 21/03/2017 - Carlos Espejo
        'Descripcion: Se toma el valor del control base y base dias
        'oRow.BaseTir = ddlCuponBase.SelectedValue
        'oRow.BaseTirDias = ddlCuponNDias.SelectedValue
        oRow.BaseTir = IIf(ddlCuponBase.SelectedValue = "", "0", ddlCuponBase.SelectedValue)
        oRow.BaseTirDias = IIf(ddlCuponNDias.SelectedValue = "", "0", ddlCuponNDias.SelectedValue)
        'OT 10090 Fin

        'INICIO | ZOLUXIONES | RCE | RF002 - Se agrega nuevo atributo a ValoresBE para campo TipoRentaRiesgo | 17/05/18
        oRow.TipoRentaRiesgo = IIf(ddlTipoRentaRiesgo.SelectedValue = "", "", ddlTipoRentaRiesgo.SelectedValue)
        'FIN | ZOLUXIONES | RCE | RF002 - Se agrega nuevo atributo a ValoresBE para campo TipoRentaRiesgo | 17/05/18

        'INICIO | ZOLUXIONES | RCE | RF002 - Se agrega nuevo atributo a ValoresBE para campo FrecuenciaDividendo | 18/05/18
        oRow.CodigoFrecuenciaDividendo = IIf(chkPagaDividendo.Checked, ddlFrecuenciaPago.SelectedValue, "")
        'FIN | ZOLUXIONES | RCE | RF002 - Se agrega nuevo atributo a ValoresBE para campo FrecuenciaDividendo | 18/05/18

        'INICIO | ZOLUXIONES | RCE | RU022 - Se agrega nuevo atributo a ValoresBE para campo Subordinario y Precio Devengado | 23/10/18
        oRow.Subordinado = IIf(chkSubordinario.Checked, "1", "0")
        oRow.PrecioDevengado = IIf(chkPrecioDevengado.Checked, "1", "0")
        'FIN | ZOLUXIONES | RCE | RU022 - Se agrega nuevo atributo a ValoresBE para campo Subordinario y Precio Devengado  | 23/10/18

        oValoresBE.Valor.AddValorRow(oRow)
        oValoresBE.Valor.AcceptChanges()
        Return oValoresBE
    End Function
    Private Sub LimpiarCampos()
        tbMnemonico.Text = ""
        tbDescripcion.Text = ""
        tbCodigoSBSinst.Text = ""
        tbEmisor.Text = ""
        tbEmisorDesc.Text = ""
        tbNumUnidades.Text = ""
        tbCodigoISIN.Text = ""
        tbValorUnitario.Text = ""
        tbCodigoSBS.Text = ""
        tbValorNominal.Text = ""
        tbValorEfecColocado.Text = ""
        tbTasaEncaje.Text = ""
        tbFechaEmision.Text = ""
        tbFechaVencimiento.Text = ""
        tbTasaCupon.Text = ""
        tbFechaPrimerCupon.Text = ""
        tbTasaSpread.Text = ""
        tbObservaciones.Text = ""
        ddlTipoRenta.SelectedValue = ""
        ddlTipoTitulo.SelectedValue = ""
        ddlAgrupacion.SelectedValue = ""
        ddlBursatilidad.SelectedValue = ""
        ddlMoneda.SelectedValue = ""
        ddlCalificacion.SelectedValue = ""
        ddlTipoCupon.SelectedValue = ""
        ddlPeriodicidad.SelectedValue = ""
        ddlTipoAmortizacion.SelectedValue = ""
        ddlCotizacionVAC.SelectedValue = ""
        ddlSituacion.SelectedValue = "A"
        ddlCuponBase.SelectedValue = ""
        ddlCuponNDias.SelectedValue = ""
        ddlGrupoContable.SelectedValue = ""
        tbFactor.Text = ""
        tbTipoFactor.Text = ""
        tbMargenInicial.Text = ""
        tbMargenMnto.Text = ""
        tbContractSize.Text = ""
        ddlTipoCodigoValor.SelectedIndex = 0
        chkGeneraIntereses.Checked = False
        'OT 9856 - 24/01/2017 - Carlos Espejo
        'Descripcion: Se movio la funcion de resaltar al archivo UIUtility.vb
        UIUtility.ResaltaCajaTexto(txtPiso, False)
        UIUtility.ResaltaCajaTexto(tbValorNominal, False)
        UIUtility.ResaltaCajaTexto(tbMnemonico, False)
        UIUtility.ResaltaCajaTexto(txtTecho, False)
        UIUtility.ResaltaCajaTexto(txtGarante, False)
        UIUtility.ResaltaCajaTexto(txtSubyacente, False)
        UIUtility.ResaltaCajaTexto(txtPrecioEjercicio, False)
        UIUtility.ResaltaCajaTexto(txtTamanoEmision, False)
        UIUtility.ResaltaCajaTexto(tbDescripcion, False)
        UIUtility.ResaltaCajaTexto(tbFechaEmision, False)
        UIUtility.ResaltaCajaTexto(tbFechaVencimiento, False)
        UIUtility.ResaltaCajaTexto(tbCodigoISIN, False)
        UIUtility.ResaltaCombo(ddlSituacion, False)
        UIUtility.ResaltaCombo(ddlMercado, False)
        UIUtility.ResaltaCombo(ddlTipoCodigoValor, False)
        UIUtility.ResaltaCombo(ddlCalificacion, False)
        UIUtility.ResaltaCombo(ddlRating, False)

        UIUtility.ResaltaCombo(ddlRatingM, False)

        UIUtility.ResaltaCombo(ddlMoneda, False)
        UIUtility.ResaltaBotones(btnCustodios, False)
        'Fin OT 9856
    End Sub
    Private Sub InicializarCampos()
        btnCuponNormal.Visible = False
        btnCustodios.Enabled = False
        btnCustodios.Visible = False
    End Sub
    Public Sub CargarCombos()
        Dim DtTablaTipoRenta As DataTable
        Dim DtTablaTipoTitulo As DataTable
        Dim DtTablaAgrupacion As DataTable
        Dim DtTablaBursatilidad As DataTable
        Dim DtTablaMoneda As DataTable
        Dim DtTablaCalificacionSBS As DataTable
        Dim DtTablaTipoCupon As DataTable
        Dim DtTablaPeriodicidad As DataTable
        Dim DtTablaTipoAmortizacion As DataTable
        Dim DtTablaIndicador As DataTable
        Dim DtTablaSituacion As DataTable
        Dim DtTablaTirBase As DataTable
        Dim DtTablaTirNDias As DataTable
        Dim DtTablaCuponBase As DataTable
        Dim DtTablaCuponNDias As DataTable
        Dim DTTablaCategoria As DataTable
        Dim DtTablaTipoLiquidez As DataTable
        Dim DtTablaICBase As DataTable
        Dim DtTablaICNDias As DataTable
        Dim dtTipoRentaFija As DataTable
        Dim dtMercado As DataTable

        'INICIO | ZOLUXIONES | RCE | RF002 - Se crea nuevo DT para cargar nuevo combo Tipo Renta Riesgo | 17/05/18
        Dim dtTipoRentaRiesgo As DataTable
        'FIN | ZOLUXIONES | RCE | RF002 - Se crea nuevo DT para cargar nuevo combo Tipo Renta Riesgo | 17/05/18 

        'INICIO | ZOLUXIONES | RCE | RF002 - Se crea nuevo DT para cargar nuevo combo Frecuencia de Pago (Dividendo) | 18/05/18
        Dim dtFrecuenciaDividendo As DataTable
        'FIN | ZOLUXIONES | RCE | RF002 - Se crea nuevo DT para cargar nuevo combo Frecuencia de Pago (Dividendo) | 18/05/18

        Dim oTipoRentaBM As New TipoRentaBM
        Dim oTipoTituloBM As New TipoTituloBM
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim oBursatilidadBM As New TipoBursatilidadBM
        Dim oMonedaBM As New MonedaBM
        Dim oCalificacionSBS As New CalificacionInstrumentoBM
        Dim oTipoCuponBM As New TipoCuponBM
        Dim oPeriodicidadBM As New PeriodicidadBM
        Dim oTipoAmortizacionBM As New TipoAmortizacionBM
        Dim oIndicadorBM As New IndicadorBM
        Dim oCotizacionBM As New CotizacionVACBM
        Dim oValoresBM As New ValoresBM
        Dim oMercadoBM As New MercadoBM
        Dim oMercadoBE As New MercadoBE

        'INICIO | ZOLUXIONES | RCE | RF002 - Se crea nuevo Objeto BM para cargar nuevo combo Frecuencia de Pago (Dividendo) | 18/05/18
        Dim oFrecuenciaDividendoBM As New FrecuenciaDividendoBM
        'FIN | ZOLUXIONES | RCE | RF002 - Se crea nuevo Objeto BM para cargar nuevo combo Frecuencia de Pago (Dividendo) | 18/05/18

        dtMercado = oMercadoBM.ListarActivos(DatosRequest).Tables(0)
        DtTablaTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se filtra en dataTable Tipo Renta por RENTA FIJA y VARIABLE para mostrarse en Combo SOLO cuando es un registro nuevo (Botón Ingresar) | 16/05/18
        If Session("accionValor") = "INGRESAR" Then DtTablaTipoRenta = DtTablaTipoRenta.Select("Descripcion in ('RENTA FIJA','RENTA VARIABLE')").CopyToDataTable()
        'FIN | ZOLUXIONES | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se filtra en dataTable Tipo Renta por RENTA FIJA y VARIABLE para mostrarse en Combo SOLO cuando es un registro nuevo (Botón Ingresar) | 16/05/18

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se carga Data en DT de FrecuenciaDividendo | 18/05/18
        dtFrecuenciaDividendo = oFrecuenciaDividendoBM.Listar(DatosRequest).Tables(0)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se carga Data en DT de FrecuenciaDividendo | 18/05/18

        DtTablaTipoTitulo = oTipoTituloBM.Listar(DatosRequest).Tables(0)
        Session("tipoTitulo") = DtTablaTipoTitulo
        DtTablaAgrupacion = oParametrosGeneralesBM.ListarAgrupacion(DatosRequest)
        DtTablaBursatilidad = oBursatilidadBM.Listar(DatosRequest).Tables(0)
        DtTablaMoneda = oMonedaBM.Listar(DatosRequest).Tables(0)
        DtTablaCalificacionSBS = oCalificacionSBS.Listar(DatosRequest).Tables(0)
        Session("Calificacion") = DtTablaCalificacionSBS
        DtTablaTipoCupon = oTipoCuponBM.Listar(DatosRequest).Tables(0)
        DtTablaPeriodicidad = oPeriodicidadBM.Listar(DatosRequest).Tables(0)
        Session("periodicidad") = DtTablaPeriodicidad
        DtTablaTipoAmortizacion = oTipoAmortizacionBM.Listar(DatosRequest).Tables(0)
        DtTablaIndicador = oIndicadorBM.Listar(DatosRequest).Tables(0)
        DtTablaSituacion = oParametrosGeneralesBM.ListarSituacion(DatosRequest)
        DtTablaTirBase = oParametrosGeneralesBM.ListarBaseTir(DatosRequest)
        DtTablaTirNDias = oParametrosGeneralesBM.ListarBaseTirNDias(DatosRequest)
        DtTablaCuponBase = oParametrosGeneralesBM.ListarBaseCupon(DatosRequest)
        DtTablaCuponNDias = oParametrosGeneralesBM.ListarBaseCuponNDias(DatosRequest)
        DTTablaCategoria = oParametrosGeneralesBM.Listar("CategoInst", DatosRequest)
        DtTablaTipoLiquidez = oParametrosGeneralesBM.Listar("CriterioLi", DatosRequest)
        'INICIO | ZOLUXIONES | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se obtiene data para DT para cargar nuevo combo Base Interes Corrido | 22/05/18
        DtTablaICBase = oParametrosGeneralesBM.Listar("CupBaseAño", DatosRequest)
        DtTablaICNDias = oParametrosGeneralesBM.Listar("CupBaseMes", DatosRequest)
        'FIN | ZOLUXIONES | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se obtiene data para DT para cargar nuevo combo Base Interes Corrido | 22/05/18
        'INICIO | ZOLUXIONES | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se ordena por nombre para que muestre por default valor "ACT" | 15/06/18
        DtTablaICNDias.DefaultView.Sort = "Nombre DESC"
        'FIN | ZOLUXIONES | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se ordena por nombre para que muestre por default valor "ACT" | 15/06/18
        dtTipoRentaFija = oParametrosGeneralesBM.Listar("rentaFija", DatosRequest)

        'INICIO | ZOLUXIONES | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se obtiene data para DT para cargar nuevo combo Tipo Renta Riesgo | 17/05/18
        dtTipoRentaRiesgo = oParametrosGeneralesBM.ListarTipoRentaRiesgo(DatosRequest)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se obtiene data para DT para cargar nuevo combo Tipo Renta Riesgo | 17/05/18

        HelpCombo.LlenarComboBox(ddlMercado, dtMercado, "CodigoMercado", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlTipoRenta, DtTablaTipoRenta, "CodigoRenta", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlTipoTitulo, DtTablaTipoTitulo, "CodigoTipoTitulo", "CodigoTipoTitulo", False)
        ddlTipoTitulo.Items.Insert(0, New ListItem("Especial", ""))
        HelpCombo.LlenarComboBox(ddlAgrupacion, DtTablaAgrupacion, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(ddlBursatilidad, DtTablaBursatilidad, "CodigoBursatilidad", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlMoneda, DtTablaMoneda, "CodigoMoneda", "CodigoMoneda", True)
        HelpCombo.LlenarComboBox(ddlTipoCupon, DtTablaTipoCupon, "CodigoTipoCupon", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlPeriodicidad, DtTablaPeriodicidad, "CodigoPeriodicidad", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlTipoAmortizacion, DtTablaTipoAmortizacion, "CodigoTipoAmortizacion", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlCotizacionVAC, DtTablaIndicador, "CodigoIndicador", "NombreIndicador", True)
        HelpCombo.LlenarComboBox(ddlSituacion, DtTablaSituacion, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlCuponBase, DtTablaCuponBase, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlCuponNDias, DtTablaCuponNDias, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlCategoria, DTTablaCategoria, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(ddlTipoLiquidez, DtTablaTipoLiquidez, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBox(ddlTipoRentaFija, dtTipoRentaFija, "Nombre", "Valor", True)

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se ingresa data para cargar nuevo combo Tipo Renta Riesgo | 17/05/18
        HelpCombo.LlenarComboBox(ddlTipoRentaRiesgo, dtTipoRentaRiesgo, "Valor", "Nombre", True)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se ingresa data para cargar nuevo combo Tipo Renta Riesgo | 17/05/18

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se ingresa data para cargar nuevo combo Frecuencia Pago (Dividendo)| 17/05/18
        HelpCombo.LlenarComboBox(ddlFrecuenciaPago, dtFrecuenciaDividendo, "CodigoFrecuenciaDividendo", "Descripcion", False)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se ingresa data para cargar nuevo combo Frecuencia Pago (Dividendo)| 17/05/18

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se ingresa data para cargar nuevo combo Base de Interés Corrido| 22/05/18
        HelpCombo.LlenarComboBox(ddlIntCorrBase, DtTablaICBase, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlIntCorrNDias, DtTablaICNDias, "Valor", "Nombre", False)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se ingresa data para cargar nuevo combo Base de Interés Corrido| 22/05/18

        CargarGrupoRegAux()
        CargaPortafolio()
    End Sub
    Private Sub CargarGrupoRegAux()
        If Not tbCodigoSBSinst.Text.Equals("") Then
            Dim DtGrupoContable As DataTable
            Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
            DtGrupoContable = oParametrosGeneralesBM.ListarGrupoContable(tbCodigoSBSinst.Text, DatosRequest)
            HelpCombo.LlenarComboBox(ddlGrupoContable, DtGrupoContable, "Valor", "Nombre", True)
        End If
    End Sub
    Private Function CargarParametrosGenerales() As Boolean
        Dim dtblTipoCupon As DataTable
        Dim dtblTipoAmortizacion As DataTable
        dtblTipoCupon = New ParametrosGeneralesBM().SeleccionarPorFiltro(ParametrosSIT.TIPO_CUPON, ParametrosSIT.TIPO_CUPON_A_DESCUENTO, "", "", DatosRequest)
        dtblTipoAmortizacion = New ParametrosGeneralesBM().SeleccionarPorFiltro(ParametrosSIT.TIPO_AMORTIZACION, ParametrosSIT.TIPO_AMORTIZACION_A_VENCIMIENTO, "", "", DatosRequest)
        If dtblTipoCupon.Rows.Count > 0 Then
            ViewState(ParametrosSIT.TIPO_CUPON_A_DESCUENTO) = dtblTipoCupon.Rows(0)(2).ToString()
        End If
        If dtblTipoAmortizacion.Rows.Count > 0 Then
            ViewState(ParametrosSIT.TIPO_AMORTIZACION) = dtblTipoAmortizacion.Rows(0)(2).ToString()
        End If
        ViewState(ParametrosSIT.TIPO_RENTA_VARIABLE) = ConfigurationManager.AppSettings("TR_FIJA").ToString()
        Return Nothing
    End Function
    Protected Sub lbkModalEmisor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbkModalEmisor.Click
        Try
            If tbEmisor.Text = "" Then
                tbEmisor.Text = ""
                tbEmisorDesc.Text = ""
            End If
            If tbCodigoSBSinst.Text.Trim() = Constantes.M_CODIGO_TIPO_INSTRUMENTO_FONDO_MUTUO Then
                ddlBursatilidad.Enabled = True
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación", "CerrarLoading();")
        End Try
    End Sub
    Private Sub CargarComboEstilo(ByVal clasificacion As String, ByVal valor2 As String)
        Dim oParametro As New ParametrosGeneralesBM
        Dim dtEstilo As New DataTable
        dtEstilo = oParametro.ListarSubLista(clasificacion, valor2, DatosRequest)
        HelpCombo.LlenarComboBox(ddlEstilo, dtEstilo, "Valor", "Nombre", True)
    End Sub
#End Region
    Private Sub ObtenerEmisorSBS()
        Dim oEntidad As New EntidadBM
        Dim oDT As DataTable
        oDT = oEntidad.Seleccionar(tbEmisor.Text, DatosRequest).Tables(0)
        If oDT.Rows.Count > 0 Then
            hdEmisor.Value = oDT.Rows(0)("CodigoSBS")
            tbEmisorDesc.Text = oDT.Rows(0)("NombreCompleto")
        End If
    End Sub
    Private Sub ObtenerValorIndicador(ByVal strCodigoIndicadorVAC As String)
        Dim i As Integer
        Dim strCod As String
        If strCodigoIndicadorVAC <> "" Then
            For i = 0 To ddlCotizacionVAC.Items.Count - 1
                strCod = ddlCotizacionVAC.Items(i).Value
                If strCod = strCodigoIndicadorVAC Then
                    ddlCotizacionVAC.SelectedIndex = i
                    Exit For
                End If
            Next
        Else
            ddlCotizacionVAC.SelectedIndex = 0
        End If
    End Sub
    Private Sub ObtenerValorTipoCupon(ByVal strCodigoTipoCupon As String)
        Dim i As Integer
        Dim strCod As String
        If strCodigoTipoCupon <> "" Then
            For i = 0 To ddlTipoCupon.Items.Count - 1
                strCod = ddlTipoCupon.Items(i).Value
                If strCod = strCodigoTipoCupon Then
                    ddlTipoCupon.SelectedIndex = i
                    Exit For
                End If
            Next
        Else
            ddlTipoCupon.SelectedIndex = 0
        End If
    End Sub
    Private Sub ObtenerValorPeriodicidad(ByVal strCodigoPeriodicidad As String)
        Dim i As Integer
        Dim strCod As String
        If strCodigoPeriodicidad <> "" Then
            For i = 0 To ddlPeriodicidad.Items.Count - 1
                strCod = ddlPeriodicidad.Items(i).Value
                If strCod = strCodigoPeriodicidad Then
                    ddlPeriodicidad.SelectedIndex = i
                    Exit For
                End If
            Next
        Else
            ddlPeriodicidad.SelectedIndex = 0
        End If
    End Sub
    Private Sub ObtenerValorTipoAmortizacion(ByVal strTipoAmortizacion As String)
        Dim i As Integer
        If strTipoAmortizacion <> "" Then
            For i = 0 To ddlTipoAmortizacion.Items.Count - 1
                If ddlTipoAmortizacion.Items(i).Value = strTipoAmortizacion Then
                    ddlTipoAmortizacion.SelectedIndex = i
                    Exit For
                End If
            Next
        Else
            ddlTipoAmortizacion.SelectedIndex = 0
        End If
    End Sub
    Private Sub ObtenerValorMoneda(ByVal strCodigoMoneda As String)
        Dim i As Integer
        For i = 0 To ddlMoneda.Items.Count - 1
            If ddlMoneda.Items(i).Text = strCodigoMoneda Then
                ddlMoneda.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub
    Private Sub ObtenerValorMonedaAux(ByVal ddlAuxMoneda As System.Web.UI.WebControls.DropDownList, ByVal strCodigoMoneda As String)
        Dim i As Integer
        For i = 0 To ddlAuxMoneda.Items.Count - 1
            If ddlAuxMoneda.Items(i).Text = strCodigoMoneda Then
                ddlAuxMoneda.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub
    Private Sub ObtenerValorBases(ByVal strTirBase As String, ByVal strNDiasTir As String, ByVal strCuponBase As String, ByVal strNDiasCupon As String)
        Dim i As Integer
        For i = 0 To ddlCuponBase.Items.Count - 1
            If ddlCuponBase.Items(i).Text = strCuponBase Then
                ddlCuponBase.SelectedIndex = i
                Exit For
            End If
        Next
        For i = 0 To ddlCuponNDias.Items.Count - 1
            If ddlCuponNDias.Items(i).Text = strNDiasCupon Then
                ddlCuponNDias.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub
    Private Function obtenerNroDiasPeriodicidad(ByVal strValor As String) As Integer
        Dim dtPeriodicidad As DataTable
        dtPeriodicidad = Session("periodicidad")
        Dim i As Integer
        For i = 0 To dtPeriodicidad.Rows.Count - 1
            If (dtPeriodicidad.Rows(i)("CodigoPeriodicidad") = strValor) Then
                If dtPeriodicidad.Rows(i)("DiasPeriodo") Is DBNull.Value Then
                    Return 0
                Else
                    Return CType(dtPeriodicidad.Rows(i)("DiasPeriodo"), Integer)
                End If
            End If
        Next
        Return 0
    End Function
    Private Sub ObtenerFactor(ByVal strRating As String)
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dtblDatos As DataTable
        dtblDatos = oParametrosGenerales.ListarRating("", strRating, "", "", DatosRequest).Tables(0)
        tbFactor.Text = ""
        If dtblDatos.Rows.Count > 0 Then
            If dtblDatos.Rows(0)("factor").ToString <> "" Then tbFactor.Text = Format(Convert.ToDecimal(dtblDatos.Rows(0)("factor").ToString), "##,##0.00")
        End If
    End Sub
    Private Sub ObtenerTipoFactor(ByVal strTipoRenta As String)
        Dim oTipoRentaBM As New TipoRentaBM
        Dim dtblDatos As DataTable
        dtblDatos = oTipoRentaBM.SeleccionarTipoFactor(strTipoRenta, DatosRequest).Tables(0)
        tbTipoFactor.Text = ""
        If dtblDatos.Rows.Count > 0 Then
            If dtblDatos.Rows(0)("Nombre").ToString <> "" Then tbTipoFactor.Text = dtblDatos.Rows(0)("Nombre").ToString
        End If
    End Sub
    Private Sub ddlMoneda_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        Try
            ddlMoneda_CambiaIndice()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar", "CerrarLoading();")
        End Try
    End Sub
    Private Sub ddlMoneda_CambiaIndice()
        Dim oMoneda As New MonedaBM, oMonedaLista As New MonedaBE, oMonedaRow As MonedaBE.MonedaRow
        oMonedaLista = oMoneda.SeleccionarPorFiltro(ddlMoneda.SelectedValue, "", "A", "", "", DatosRequest)
        oMonedaRow = oMonedaLista.Moneda(0)
        hdCodigoSBSMoneda.Value = oMonedaRow.CodigoMonedaSBS
        If ddlMoneda.SelectedValue <> "" And tbCodigoSBSinst.Text <> "" And tbEmisor.Text <> "" Then
            If tbCodigoSBS.Text.Length > 7 Then
                tbCodigoSBS.Text = tbCodigoSBSinst.Text + hdEmisor.Value + hdCodigoSBSMoneda.Value + tbCodigoSBS.Text.Substring(7)
            Else
                tbCodigoSBS.Text = tbCodigoSBSinst.Text + hdEmisor.Value + hdCodigoSBSMoneda.Value
            End If
        End If
        If tbCodigoSBSinst.Text.Trim() = Constantes.M_CODIGO_TIPO_INSTRUMENTO_FONDO_MUTUO Then
            ddlBursatilidad.Enabled = True
        End If
    End Sub
    Private Sub ddlTipoTitulo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoTitulo.SelectedIndexChanged
        Try
            Dim dtTablaTitulo As DataTable
            Dim i As Integer
            dtTablaTitulo = CType(Session("tipoTitulo"), DataTable)
            For i = 0 To dtTablaTitulo.Rows.Count - 1
                If dtTablaTitulo.Rows(i)("CodigoTipoTitulo") = ddlTipoTitulo.SelectedValue Then
                    tbCodigoSBSinst.Text = dtTablaTitulo.Rows(i)("CodigoTipoInstrumentoSBS")
                    tbSinTipoInst.Text = dtTablaTitulo.Rows(i)("CodigoTipoTitulo")
                    ObtenerValorMoneda(dtTablaTitulo.Rows(i)("CodigoMoneda"))
                    ObtenerValorBases(dtTablaTitulo.Rows(i)("BaseTIR"), dtTablaTitulo.Rows(i)("BaseTirDias"), dtTablaTitulo.Rows(i)("BaseCupon"), dtTablaTitulo.Rows(i)("BaseCuponDias"))
                    ObtenerValorPeriodicidad(IIf(dtTablaTitulo.Rows(i)("CodigoPeriodicidad") Is DBNull.Value, "", dtTablaTitulo.Rows(i)("CodigoPeriodicidad")))
                    tbTasaSpread.Text = IIf(dtTablaTitulo.Rows(i)("TasaSpread") Is DBNull.Value, 0, dtTablaTitulo.Rows(i)("TasaSpread"))
                    ObtenerValorTipoAmortizacion(IIf(dtTablaTitulo.Rows(i)("CodigoTipoAmortizacion") Is DBNull.Value, "", dtTablaTitulo.Rows(i)("CodigoTipoAmortizacion")))
                    ObtenerValorIndicador(IIf(dtTablaTitulo.Rows(i)("CodigoIndicadorVAC") Is DBNull.Value, "", dtTablaTitulo.Rows(i)("CodigoIndicadorVAC")))
                    ObtenerValorTipoCupon(IIf(dtTablaTitulo.Rows(i)("CodigoTipoCupon") Is DBNull.Value, "", dtTablaTitulo.Rows(i)("CodigoTipoCupon")))
                    Dim oTipoTituloBM As New TipoTituloBM
                    tbTasaEncaje.Text = Format(Convert.ToDecimal(oTipoTituloBM.ObtenerTasaEncaje(ddlTipoTitulo.SelectedValue.ToString, tbMnemonico.Text.Trim, DatosRequest).ToString), "##,##0.0000000")
                    ddlPeriodicidad.Enabled = True
                    ddlTipoAmortizacion.Enabled = True
                    ddlCotizacionVAC.Enabled = True
                    ddlTipoCupon.Enabled = True
                    Exit For
                End If
            Next
            If ddlTipoTitulo.SelectedValue <> "" Then
                tbCodigoSBSinst.ReadOnly = True
                tbSinTipoInst.Visible = True
            Else
                imbTipoInstrumento.Visible = True
                ddlMoneda.SelectedValue = ""
                tbCodigoSBSinst.Text = ""
                ddlCuponBase.SelectedValue = ""
                ddlCuponNDias.SelectedValue = ""
            End If
            If ddlMoneda.SelectedValue <> "" And tbEmisor.Text <> "" And tbCodigoSBSinst.Text <> "" Then
                If tbCodigoSBS.Text.Length > 7 Then
                    tbCodigoSBS.Text = tbCodigoSBSinst.Text + hdEmisor.Value + ddlMoneda.SelectedValue + tbCodigoSBS.Text.Substring(7)
                Else
                    tbCodigoSBS.Text = tbCodigoSBSinst.Text + hdEmisor.Value + ddlMoneda.SelectedValue
                End If
            End If
            ValidarRequerimientoCuponera()
            CargarGrupoRegAux()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar", "CerrarLoading();")
        End Try
    End Sub
    Private Sub ActualizarCodigoSBSint()
        If ddlTipoTitulo.SelectedValue <> "" Then
            tbCodigoSBSinst.ReadOnly = True
        Else
            tbCodigoSBSinst.ReadOnly = False
        End If
    End Sub
    Private Sub lbkModalInstrumento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbkModalInstrumento.Click
        Try
            EjecutarJS("calcularSBS();")
            CargarGrupoRegAux()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación", "CerrarLoading();")
        End Try
    End Sub
    Public Function ValidarNumero(ByVal Cadena() As Char, ByVal Tipo As Byte) As Boolean
        Dim Estado As Boolean
        Dim CadBase As String = "0123456789,"
        Dim posic, Largo, Ind As Integer
        Dim Eval As Char
        If Tipo = 2 Then
            CadBase = CadBase + "."
        End If
        If Tipo = 3 Then
            CadBase = CadBase + "/"
        End If
        Estado = True
        Largo = Len(Cadena)
        Ind = 0
        Do While Ind < Largo
            Eval = Cadena(Ind)
            posic = InStr(CadBase, Eval)
            If posic = 0 Then
                Estado = False
                Ind = Largo
            End If
            Ind = Ind + 1
        Loop
        Return Estado
    End Function
    Public Function ValidarNumeros() As String
        Dim Resultado1, Resultado2, Resultado3, Resultado4 As Boolean
        Dim msg As String = ""
        Dim strMensajeError As String = ""
        Resultado1 = ValidarNumero(tbNumUnidades.Text.Trim(), 2)
        If Not Resultado1 Then
            msg += "\t-Numero de Unidades\n"
        End If
        Resultado2 = ValidarNumero(tbValorEfecColocado.Text.Trim(), 2)
        If Not Resultado2 Then
            msg += "\t-Valor Efectivo Colocado\n"
        End If
        Resultado3 = ValidarNumero(tbValorNominal.Text.Trim(), 2)
        If Not Resultado3 Then
            msg += "\t-Valor Nominal\n"
        End If
        Resultado4 = ValidarNumero(tbValorUnitario.Text.Trim(), 2)
        If Not Resultado4 Then
            msg += "\t-ValorUnitario\n"
        End If
        If (msg <> "") Then
            strMensajeError = "Formato Numerico Incorrecto \n" + msg + "\n"
            Return strMensajeError
        Else
            Return ""
        End If
    End Function
    Private Function validarTipoInstrumento() As Boolean
        validarTipoInstrumento = False
        Dim oBMTipoInstrumento As New TipoInstrumentoBM
        Dim sValor As String
        If tbCodigoSBSinst.Text <> "" Then
            sValor = oBMTipoInstrumento.Validar_TipoInstrumento(tbCodigoSBSinst.Text, DatosRequest)
            If sValor <> "0" Then
                validarTipoInstrumento = True
            Else
                validarTipoInstrumento = False
            End If
        End If
    End Function
    Public Function validaFechas() As Boolean
        If Not hdnCodigoClaseInstrumento.Value.Equals("1") Then
            Return True
        End If
        Dim FechaEmision As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaEmision.Text)
        Dim FechaVencimiento As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaVencimiento.Text)
        Dim FechaPrimerCupon As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaPrimerCupon.Text)
        ValidarRequerimientoCuponera(True)
        If ddlTipoRenta.SelectedValue <> UIUtility.ObtenerCodigoTipoRenta("RFIJ") Then
            If FechaVencimiento <> 0 And FechaEmision > FechaVencimiento Then
                AlertaJS("La Fecha de Emisión no debe ser mayor a la Fecha de Vencimiento", "CerrarLoading();")
                Return False
            End If
        Else
            If FechaEmision > FechaVencimiento Then
                AlertaJS("La Fecha de Emisión no debe ser mayor a la Fecha de Vencimiento", "CerrarLoading();")
                Return False
            End If
            If obtenerNroDiasPeriodicidad(ddlPeriodicidad.SelectedValue) <> 0 Then
                If FechaVencimiento - FechaEmision <= obtenerNroDiasPeriodicidad(ddlPeriodicidad.SelectedValue) Then
                    AlertaJS("La Periodicidad debe ser menor a la diferencia de la Fecha de Vencimiento y Fecha de Emisión", "CerrarLoading();")
                    Return False
                End If
            End If
            If tbCodigoSBSinst.Text <> Constantes.M_CODIGO_TIPO_INSTRUMENTO_FONDO_MUTUO And ddlTipoRenta.SelectedValue = UIUtility.ObtenerCodigoTipoRenta("RFIJ") Then
                If ViewState("CUPONERA_REQUERIDO").ToString() = "S" Then
                    If FechaEmision > FechaPrimerCupon Then
                        AlertaJS("La Fecha de Emisión no debe ser mayor a la Fecha del Primer Cupón", "CerrarLoading();")
                        Return False
                    End If
                End If
                If ViewState("CUPONERA_REQUERIDO").ToString() = "S" Then
                    If FechaPrimerCupon > FechaVencimiento Then
                        AlertaJS("La Fecha del Primer Cupón no debe ser mayor a la Fecha de Vencimiento", "CerrarLoading();")
                        Return False
                    End If
                End If
            End If
        End If
        Return True
    End Function
    Private Sub ddlTipoCupon_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoCupon.SelectedIndexChanged
        Try
            imgInfoTipoCupon.Visible = (ddlTipoCupon.SelectedValue = ViewState(ParametrosSIT.TIPO_CUPON_A_DESCUENTO))
            ValidarRequerimientoCuponera()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar", "CerrarLoading();")
        End Try
    End Sub
    Private Sub ValidarRequerimientoCuponera(Optional ByVal fbFlag As Boolean = False)
        If ddlTipoAmortizacion.Items.Count > 0 Then
            Dim codigoTipoCupon As String
            Dim tipoRenta As String
            codigoTipoCupon = ddlTipoCupon.SelectedValue.ToString()
            tipoRenta = ViewState(ParametrosSIT.TIPO_RENTA_VARIABLE)
            If ddlTipoRenta.SelectedValue = tipoRenta Then
                If codigoTipoCupon = ViewState(ParametrosSIT.TIPO_CUPON_A_DESCUENTO).ToString() Then
                    ddlTipoAmortizacion.SelectedValue = ViewState(ParametrosSIT.TIPO_AMORTIZACION).ToString()
                    ddlCotizacionVAC.Enabled = False
                Else
                    If fbFlag = False Then ddlTipoAmortizacion.SelectedIndex = 0
                    tbTasaCupon.Enabled = True
                    ddlCotizacionVAC.Enabled = True
                    ddlPeriodicidad.Enabled = True
                    ddlSituacion.Enabled = True
                End If
            End If
        End If
        If (hdnCodigoClaseInstrumento.Value = "3" Or _
            hdnCodigoClaseInstrumento.Value = "7" Or _
            hdnCodigoClaseInstrumento.Value = "11" Or _
            (hdnCodigoClaseInstrumento.Value = "10" And tbCodigoSBSinst.Text.Trim.Equals("19")) Or _
            (Trim(tbCodigoSBSinst.Text) = "08" And Trim(hdEmisor.Value) = "4000" And ddlTipoCupon.SelectedValue = "3")) Or _
            (Trim(tbCodigoSBSinst.Text) = "100" Or Trim(tbCodigoSBSinst.Text) = "101") Then
            ViewState("CUPONERA_REQUERIDO") = "N"
            btnCuponNormal.Visible = False
            btnCuponNormal.Enabled = False
        Else
            If (hdnCodigoClaseInstrumento.Value = "1" Or _
                 hdnCodigoClaseInstrumento.Value = "5" Or _
                (hdnCodigoClaseInstrumento.Value = "10" And tbCodigoSBSinst.Text.Trim.Equals("09")) Or _
                tbCodigoSBSinst.Text.Trim.Equals("57")) Then
                ViewState("CUPONERA_REQUERIDO") = "S"
                btnCuponNormal.Visible = True
                btnCuponNormal.Enabled = True
                ddlPeriodicidad.Enabled = True
                tbTasaCupon.Enabled = True
            End If
        End If
    End Sub
    Private Function crearObjeto_CuponDscto() As DataTable
        Dim dtCuponDscto As New DataTable
        Dim drCuponDscto As DataRow
        Dim difdias As Integer
        dtCuponDscto.Columns.Add("FechaIni", GetType(Decimal))
        dtCuponDscto.Columns.Add("FechaFin", GetType(Decimal))
        dtCuponDscto.Columns.Add("DifDias", GetType(Decimal))
        dtCuponDscto.Columns.Add("Amortizac", GetType(Decimal))
        dtCuponDscto.Columns.Add("TasaCupon", GetType(Decimal))
        dtCuponDscto.Columns.Add("BaseCupon", GetType(Decimal))
        dtCuponDscto.Columns.Add("DiasPago", GetType(Decimal))
        dtCuponDscto.Columns.Add("AmortizacConsolidado", GetType(Decimal))
        dtCuponDscto.Columns.Add("TasaVariable", GetType(Decimal))
        dtCuponDscto.Columns.Add("Estado", GetType(String))
        drCuponDscto = dtCuponDscto.NewRow

        'OT10965 - 28/11/2017 - Ian Pastor M.
        'Descripción: Para todos los instrumentos que generan cuponera ficticia, la fecha de inicio es su fecha de emisión.
        'drCuponDscto("FechaIni") = UIUtility.ConvertirFechaaDecimal(tbFechaPrimerCupon.Text)
        drCuponDscto("FechaIni") = UIUtility.ConvertirFechaaDecimal(tbFechaEmision.Text)
        'OT10965 Fin

        drCuponDscto("FechaFin") = UIUtility.ConvertirFechaaDecimal(tbFechaVencimiento.Text)

        'OT10965 - 28/11/2017 - Ian Pastor M.
        'Descripción: Para todos los instrumentos que generan cuponera ficticia, la fecha de inicio es su fecha de emisión.
        ''OT10689 - Inicio. Se toma en cuenta el número de días de la base del cupón
        'drCuponDscto("DifDias") = Convert.ToDecimal(IIf(ddlCuponBase.SelectedValue = "", 0, ddlCuponBase.SelectedValue))
        difdias = IIf(ddlIntCorrNDias.SelectedValue = "30", _
                      Utiles.Dias360(Date.Parse(tbFechaEmision.Text), Date.Parse(tbFechaVencimiento.Text)), _
                      Utiles.DiasACT(Date.Parse(tbFechaEmision.Text), Date.Parse(tbFechaVencimiento.Text)))

        'drCuponDscto("DifDias") = DateDiff(DateInterval.Day, Date.Parse(tbFechaEmision.Text), Date.Parse(tbFechaVencimiento.Text))
        ''OT10689 Fin.
        'OT10965 Fin
        drCuponDscto("DifDias") = difdias
        drCuponDscto("TasaVariable") = DBNull.Value
        drCuponDscto("Estado") = DBNull.Value
        drCuponDscto("Amortizac") = 100
        drCuponDscto("TasaCupon") = Convert.ToDecimal(IIf(tbTasaCupon.Text = "", 0, tbTasaCupon.Text))
        drCuponDscto("BaseCupon") = Convert.ToDecimal(IIf(ddlCuponBase.SelectedValue = "", 0, ddlCuponBase.SelectedValue))
        drCuponDscto("DiasPago") = difdias
        drCuponDscto("AmortizacConsolidado") = 0
        dtCuponDscto.Rows.Add(drCuponDscto)
        Return dtCuponDscto
    End Function
    Private Function ExisteCuponera() As Boolean
        Dim oValoresBM As New ValoresBM
        Dim dtblDatos As DataTable = oValoresBM.ExistenciaCuponera(tbMnemonico.Text, DatosRequest)
        If dtblDatos.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub ddlRating_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRating.SelectedIndexChanged
        Try
            ddlCalificacion.SelectedValue = ddlRating.SelectedValue
            ObtenerFactor(ddlRating.SelectedValue)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar", "CerrarLoading();")
        End Try
    End Sub
    Protected Sub ddlCategoria_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCategoria.SelectedIndexChanged
        Try
            CargarComboEstilo("EstiloCat", ddlCategoria.SelectedValue)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar", "CerrarLoading();")
        End Try
        CargarComboEstilo("EstiloCat", ddlCategoria.SelectedValue)
    End Sub
    Protected Sub btnCapComp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCapComp.Click
        Try
            Dim strAux As String = "NO"
            If Not Request.QueryString("vOI") Is Nothing Then
                strAux = "YES"
            End If
            EjecutarJS(UIUtility.MostrarPopUp("frmIngresoCapitalCompro.aspx?vISIN=" + tbCodigoISIN.Text.Trim.ToUpper + "&vMnemonico=" + tbMnemonico.Text.Trim.ToUpper + "&vSBS=" + tbCodigoSBS.Text.Trim.ToUpper + "&vReadOnly=" + strAux, "yes", 769, 500, 150, 50, "no", "no", "yes", "yes"), False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación", "CerrarLoading();")
        End Try
    End Sub
    Protected Sub ddlMercado_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        LlenaRating(ddlMercado.SelectedValue)
        LlenaRatingMandato(ddlMercado.SelectedValue)
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Activacion de Combo Frecuencia de pago | 18/05/18
    Protected Sub chkPagaDividendo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPagaDividendo.CheckedChanged
        If chkPagaDividendo.Checked Then
            divRowFrecuenciaPago.Attributes.Add("style", "display:block")
        Else
            ddlFrecuenciaPago.SelectedIndex = 0
            divRowFrecuenciaPago.Attributes.Add("style", "display:none")
        End If
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Activacion de Combo Frecuencia de pago | 18/05/18
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Activacion de Combos de Base Interes Corrido | 22/05/18
    Protected Sub chkBaseIC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkBaseIC.CheckedChanged
        If chkBaseIC.Checked Then
            divBaseNDiasIC.Attributes.Add("style", "display:block")
            divBaseIC.Attributes.Add("style", "display:block")
        Else
            divBaseNDiasIC.Attributes.Add("style", "display:none")
            divBaseIC.Attributes.Add("style", "display:none")
            ddlIntCorrBase.SelectedIndex = 0
            ddlIntCorrNDias.SelectedIndex = 0
        End If
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Activacion de Combos de Base Interes Corrido | 22/05/18
    Private Sub CargarClasificacionRating()
        Dim dt As DataTable = Nothing
        Dim objTerceroBM As New TercerosBM
        dt = objTerceroBM.SeleccionarRatingTercero_Historia("", tbEmisor.Text, 0)
        txtRatingInterno.Text = IIf(IsDBNull(dt.Rows(0)("DescripcionRatingInterno")), "SIN DEFINIR", dt.Rows(0)("DescripcionRatingInterno"))
        txtRatingExterno.Text = IIf(IsDBNull(dt.Rows(0)("DescripcionRating")), "SIN DEFINIR", dt.Rows(0)("DescripcionRating"))
        txtFortalezaFinanciera.Text = IIf(IsDBNull(dt.Rows(0)("DescripcionFortFina")), "SIN DEFINIR", dt.Rows(0)("DescripcionFortFina"))
    End Sub

    Protected Sub ddlPortafolio_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Dim oValoresBM As New ValoresBM
        Dim codigoMnemonico As String = oValoresBM.VerificarCodigoPortafolioSBS(ddlPortafolio.SelectedValue)
        If codigoMnemonico <> String.Empty Then
            AlertaJS("El portafolio " + ddlPortafolio.SelectedItem.ToString + " ya se encuentra asignado al instrumento " + codigoMnemonico, "CerrarLoading();")
            ddlPortafolio.SelectedValue = String.Empty
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        'INICIO - OT11851 - Se deshabilita campos, sólo se podrá cambiar por ventana de CuponeraNormal.
        tbMnemonico.ReadOnly = (Session("accionValor") = "MODIFICAR")
        Dim cuponValores As CuponeraNormalValoresBE = CuponeraValoresSesion
        Session("cambioCuponera") = False
        tbTasaCupon.Enabled = True
        If cuponValores.CambioValores Then
            ddlPeriodicidad.SelectedValue = cuponValores.CodigoPeriodo
            ddlTipoAmortizacion.SelectedValue = cuponValores.CodigoTipoAmortizacion
            tbTasaCupon.Text = cuponValores.TasaCupon
            ddlIntCorrNDias.SelectedValue = cuponValores.baseDias
            ddlIntCorrBase.SelectedValue = cuponValores.baseAnios
            tbFechaEmision.Text = UIUtility.ConvertirFechaaString(cuponValores.FechaEmision)
            tbFechaVencimiento.Text = UIUtility.ConvertirFechaaString(cuponValores.FechaVencimiento)
            tbFechaPrimerCupon.Text = UIUtility.ConvertirFechaaString(cuponValores.FechaFinProximoCupon)
            hdTipoTasaVariable.Value = cuponValores.tipoTasaVariable
            hdDiasTTasaVariable.Value = cuponValores.periodicidadTasaVariable
            hdTasaVariable.Value = cuponValores.tasaVariable
            tbTasaCupon.Enabled = False
            Session("sCuponeraNormalValoresSesion") = Nothing
            Session("cambioCuponera") = True
        End If


        If (btnCuponNormal.Enabled And ((Session("accionValor") = "MODIFICAR") Or _
            ((cuponValores.CambioValores Or Not Session("cuponeraNormal") Is Nothing) And Session("accionValor") = "INGRESAR"))) And _
            ddlTipoRenta.SelectedValue.ToUpper = UIUtility.ObtenerCodigoTipoRenta("RFIJ") Then
            tbFechaEmision.ReadOnly = Not (tbFechaEmision.Text.Trim = String.Empty)
            tbFechaVencimiento.ReadOnly = Not (tbFechaVencimiento.Text.Trim = String.Empty)
            tbFechaPrimerCupon.ReadOnly = Not (tbFechaPrimerCupon.Text.Trim = String.Empty)
            ddlPeriodicidad.Enabled = (ddlPeriodicidad.SelectedValue = String.Empty)
            ddlTipoAmortizacion.Enabled = (ddlTipoAmortizacion.SelectedValue = String.Empty)
            tbTasaCupon.Enabled = (tbTasaCupon.Text.Trim = String.Empty)
            If tbFechaEmision.ReadOnly Then dvDiv121.Attributes.Add("class", "input-append")
            If tbFechaVencimiento.ReadOnly Then dvDiv221.Attributes.Add("class", "input-append")
            If tbFechaPrimerCupon.ReadOnly Then dvDiv321.Attributes.Add("class", "input-append")
            ddlIntCorrNDias.Enabled = False
            ddlIntCorrBase.Enabled = False

        End If


        'FIN - OT11851 - Se deshabilita campos, sólo se podrá cambiar por ventana de CuponeraNormal.
    End Sub
End Class

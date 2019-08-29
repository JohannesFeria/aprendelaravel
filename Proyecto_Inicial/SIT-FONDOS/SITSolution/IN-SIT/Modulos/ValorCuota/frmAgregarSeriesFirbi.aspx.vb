Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Imports CrystalDecisions.CrystalReports.Engine
Imports System.IO
Imports SistemaProcesosBL

Partial Class Modulos_ValorCuota_frmAgregarSeriesFirbi
    Inherits BasePage
    Dim ObjValorCuota As New ValorCuotaBM
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not Page.IsPostBack Then
                InicializarBotones()
                ViewState("NroReg") = Request.QueryString("NroReg")
                ViewState("Porcentaje") = Request.QueryString("Porcentaje")
                ViewState("Portafolio") = Request.QueryString("Portafolio")
                tbPortafolio.Text = Request.QueryString("DetPorta")
                tbFechaInforme.Text = Request.QueryString("Fecha")
                tbSerie.Text = Request.QueryString("Serie")
                CargarControlesCuotasLiberadas() 'OT10916 - 07/11/2017 - Ian Pastor M. Obtener Porcentaje del día anterior
                ViewState("CxCMontosDividendos") = ObtenerCxCMontosDividendos()
                Dim oPortafolioBE As PortafolioBE
                Dim oPortafolioBM As New PortafolioBM
                Dim oRow As PortafolioBE.PortafolioRow
                oPortafolioBE = oPortafolioBM.Seleccionar(ViewState("Portafolio"), Me.DatosRequest)
                oRow = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
                Dim ExisteValoracion As Decimal, ExistePrecierre As Integer
                Dim Switch As Boolean = False
                Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
                Dim mensaje As String = String.Empty 'Variable para obtener los mensajes de las validaciones
                ExisteValoracion = oCarteraTituloValoracion.ExisteValorizacionFechasPosteriores(ViewState("Portafolio"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text))
                ExistePrecierre = oCarteraTituloValoracion.Valida_PRECIERRE_CONSOLIDADO_OPERACIONES(CDate(tbFechaInforme.Text), CDate(tbFechaInforme.Text), IIf(oRow.CodigoPortafolioSisOpe = "", 0, oRow.CodigoPortafolioSisOpe))
                'OT12126 | rcolonia  | Zoluxiones | Se valida T - 1 VC cerrado en operaciones
                Dim fechaAyer As Date = Convert.ToDateTime(Request.QueryString("Fecha"))
                fechaAyer = fechaAyer.AddDays(-1)
                Dim ExisteVCT_1 As String = BloquearFondoPorCerrarEnOperacion(fechaAyer.ToShortDateString, True)

                ' Fin OT12126
                If oRow.CodigoPortafolioSisOpe = "" Then
                    btnProcesar.Visible = False
                    mensaje = "El portafolio debe tener asociado un codigo para el Sistema de Operaciones, se debe completar la informacion."
                ElseIf ExistePrecierre > 0 Then
                    btnProcesar.Visible = False
                    Switch = True
                    mensaje = "Existe un precierre generado desde el sistema de Operaciones, no se puede modificar la informacion presentada."
                ElseIf ExisteVCT_1 <> String.Empty Then
                    btnProcesar.Visible = False
                    Switch = True
                    mensaje = ExisteVCT_1
                ElseIf ExisteValoracion > 0 Then
                    btnProcesar.Visible = False
                    Switch = True
                    mensaje = "La fecha no coincide con la ultima valorización, no se puede modificar la informacion presentada."
              
                ElseIf ExisteValorCuota() > 0 Then
                    'btnProcesar.Visible = False
                    Switch = True
                Else
                    btnProcesar.Visible = True
                End If
                CargarValoresCuota()
                If mensaje.Length > 0 Then
                    AlertaJS(mensaje)
                    FormularioModoLectura(True)
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub CargarValoresCuota()

        If Not CargarValoresIngresados() Then
            tbInversionesT1.Text = 0
            tbComprasT.Text = 0
            tbVentasVenciT.Text = 0
            tbRentabilidadT.Text = 0
            tbValoriForwardT.Text = 0
            txtVCA.Text = "0"
            txtigv.Text = ParametrosSIT.IGV
            txtporcom.Text = "0"
            txtMontoDividendosPrecierre.Text = "0"
            tbCXCPreCierre.Text = "0"
            tbCXPPreCierre.Text = "0"
            tbValPatriPrecierre2.Text = "0"
            tbComiSAFM.Text = "0"
            tbValPatriPrecierre.Text = "0"
            tbInversionesSubTotal.Text = 0
            tbCXCVentaTitulo.Text = 0
            tbCXPtitulo.Text = 0
            tbCXPCompraTitulo.Text = 0
            tbCajaPrecierre.Text = 0
            tbOtrosGastos.Text = 0
            tbOtrosIngresos.Text = 0
            'Se comenta porque la primera vez se carga los valores con cero
            'CargarValoresCalculados()
            txtVCDiferencia.Text = "0"
            tbValCuotaPrecierre.Text = 0
        End If
        BloquearFondoPorCerrarEnOperacion(Request.QueryString("Fecha"), False)
    End Sub
    Private Sub ObtenerMontosContablesFirbi(ByVal CodigoPortafolio As String, ByVal FechaOperacionCadena As String, ByVal Porcentaje As Decimal)
        Dim DTValorCuota As DataTable
        DTValorCuota = ObjValorCuota.ObtenerDatosFirbiPrecierre(CodigoPortafolio, UIUtility.ConvertirFechaaDecimal(FechaOperacionCadena))
        tbInversionesSubTotal.Text = Math.Round((DTValorCuota.Rows(0)("InversionesSubTotal") * Decimal.Parse(ViewState("Porcentaje"))) / 100, 7)
        tbCXCVentaTitulo.Text = Math.Round((DTValorCuota.Rows(0)("CXCVentaTitulo") * Decimal.Parse(ViewState("Porcentaje"))) / 100, 7)
        tbCXPtitulo.Text = Math.Round((DTValorCuota.Rows(0)("CXPCompraTitulo") * Decimal.Parse(ViewState("Porcentaje"))) / 100, 7)
        tbCajaPrecierre.Text = Math.Round((DTValorCuota.Rows(0)("CajaPrecierre") * Decimal.Parse(ViewState("Porcentaje"))) / 100, 7)

        Dim idFondoFirbiOperacion As Integer = Convert.ToInt32(Request.QueryString("CodigoPortafolioSeriadoSO"))
        Dim totalCuotas As Decimal = 0
        Dim dtSeriesFondo As DataTable = UIUtility.ObtenerSeriesFirbi(ObjValorCuota, Request.QueryString("Fecha"), Request.QueryString("Portafolio"), totalCuotas)
        Dim filaSeleccionada As DataRow = (From dr In dtSeriesFondo
                           Where (dr("CodigoPortafolioSO") = idFondoFirbiOperacion) Select dr
                  ).FirstOrDefault()
        tbValCuotaPrecierre.Text = Decimal.Parse(filaSeleccionada("CuotasCierreOnline"))
        tbValValoresPrecierre.Text = Decimal.Parse(filaSeleccionada("ValoresCierreOnline"))

    End Sub
    Private Function BloquearFondoPorCerrarEnOperacion(ByVal fechaProceso As String, ByVal bFechaT_1 As Boolean) As String
        Dim objOperaciones As New PrecierreBO

        Dim idFondoFirbiOperacion As Integer = Convert.ToInt32(Request.QueryString("CodigoPortafolioSeriadoSO"))
        Dim rowVCuotaHistorico As DataRow = Nothing
        rowVCuotaHistorico = objOperaciones.ObtenerDetalleValorCuotaCerrado(Convert.ToDecimal(idFondoFirbiOperacion), Convert.ToDateTime(fechaProceso))

        If rowVCuotaHistorico Is Nothing Then
            'no existe cierre en operacion, por lo tanto el formulario es editable
            FormularioModoLectura(False)
            Return IIf(bFechaT_1, "Se necesita cerrar el valor cuota <br> (T-1: " + fechaProceso + ") en operaciones para continuar con el proceso de cierre.", String.Empty)
        Else
            FormularioModoLectura(True)
            Return String.Empty
        End If

    End Function

    Private Sub FormularioModoLectura(ByVal Lectura As Boolean)
        If Lectura Then
            tbInversionesSubTotal.ReadOnly = True
            tbCXCVentaTitulo.ReadOnly = True
            tbCXPtitulo.ReadOnly = True
            tbCajaPrecierre.ReadOnly = True
            tbCXPCompraTitulo.ReadOnly = True
            tbValCuotaPrecierre.ReadOnly = True
            tbValValoresPrecierre.ReadOnly = True
            tbOtrosGastos.ReadOnly = True
            tbOtrosIngresos.ReadOnly = True

            btnAceptar.Visible = False
            btnProcesar.Visible = False
        Else
            tbInversionesSubTotal.ReadOnly = False
            tbCXCVentaTitulo.ReadOnly = False
            tbCXPtitulo.ReadOnly = False
            tbCajaPrecierre.ReadOnly = False
            tbCXPCompraTitulo.ReadOnly = False
            tbValCuotaPrecierre.ReadOnly = False
            tbValValoresPrecierre.ReadOnly = False
            tbOtrosGastos.ReadOnly = False
            tbOtrosIngresos.ReadOnly = False
        End If
    End Sub

    Private Function CargarValoresIngresados() As Boolean
        CargarValoresIngresados = False
        Dim oValorCuotaBESerie As New ValorCuotaBE
        Dim dtValorCuotaSerie As DataTable
        oValorCuotaBESerie = ObjValorCuota.SeleccionarValorCuota(ViewState("Portafolio"), Request.QueryString("NroReg"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim))
        dtValorCuotaSerie = oValorCuotaBESerie.Tables(0)
        tbOtrosGastos.Text = "0"
        tbOtrosIngresos.Text = "0"


        '-----------
        If dtValorCuotaSerie.Rows.Count > 0 Then
            tbInversionesSubTotal.Text = dtValorCuotaSerie.Rows(0)("InversionesSubTotal")
            tbCXCVentaTitulo.Text = dtValorCuotaSerie.Rows(0)("CXCVentaTitulo")
            tbCXPtitulo.Text = dtValorCuotaSerie.Rows(0)("CXPCompraTitulo")
            tbCXPCompraTitulo.Text = dtValorCuotaSerie.Rows(0)("CXPCompraTitulo")
            tbCajaPrecierre.Text = dtValorCuotaSerie.Rows(0)("CajaPrecierre")

            tbValValoresPrecierre.Text = dtValorCuotaSerie.Rows(0)("ValCuotaPreCierreVal")
            tbValCuotaCierreCuota.Text = dtValorCuotaSerie.Rows(0)("ValCuotaPreCierreVal")
            tbValCuotaPrecierre.Text = dtValorCuotaSerie.Rows(0)("ValCuotaPreCierre")

            tbOtrosGastos.Text = dtValorCuotaSerie.Rows(0)("OtrosGastos")
            tbOtrosIngresos.Text = dtValorCuotaSerie.Rows(0)("OtrosIngresos")

            tbValPatriPrecierre.Text = dtValorCuotaSerie.Rows(0)("ValPatriPreCierre1")
            tbComiSAFM.Text = dtValorCuotaSerie.Rows(0)("ComisionSAFM")
            tbValPatriPrecierre2.Text = dtValorCuotaSerie.Rows(0)("ValPatriPreCierre2")

            CargarValoresIngresados = True
        Else
            tbComiSAFM.Text = "0"
            tbValPatriPrecierre2.Text = "0"
            tbValPatriPrecierre.Text = "0"
            tbValValoresPrecierre.Text = "0"
            tbAporteCuota.Text = "0"
            tbAporteValores.Text = "0"
            tbRescatesCuota.Text = "0"
            tbRescatesValores.Text = "0"
            tbCXCVentaTituloCierre.Text = "0"
            tbCXCCierre.Text = "0"
            tbCXPtituloCierre.Text = "0"
            tbCXPotrasCierre.Text = "0"
            tbOtrasCXPCierre.Text = "0"
            tbOtrosGastosCierre.Text = "0"
            tbOtrosIngresosCierre.Text = "0"
            tbOtrosGastosExlusivos.Text = "0"
            tbOtrosIngresosExlusivos.Text = "0"
            tbCXCExclusivosCierre.Text = "0"
            tbCXPExclusivosCierre.Text = "0"
            tbOtrosGastosExlusivosCierre.Text = "0"
            tbOtrosIngresosExlusivosCierre.Text = "0"
            tbValPatCierreCuota.Text = "0"
            tbValPatCierreValores.Text = "0"
            tbValCuotaCierreCuota.Text = "0"
            tbValCuotaCierreValores.Text = "0"
            tbCXPCompraTituloCierre.Text = "0"
            txtVCA.Text = "0"
            txtVCDiferencia.Text = "0"
            tbOtrosCXCCierre.Text = "0"
            tbOtrosGastos.Text = "0"
            tbOtrosIngresos.Text = "0"
            tbInversionesSubTotal.Text = "0"
            tbCXCVentaTitulo.Text = "0"
            tbCXPtitulo.Text = "0"
            tbCXPCompraTitulo.Text = "0"
            tbCajaPrecierre.Text = "0"
            tbCXCPreCierre.Text = "0"
            tbCXPPreCierre.Text = "0"
            tbValPatriPrecierre2.Text = "0"
            tbComiSAFM.Text = "0"
            tbValPatriPrecierre.Text = "0"
        End If


    End Function

    Private Function CargarValoresCalculados() As Boolean
        CargarValoresCalculados = False
        Dim oValorCuotaBESerie As New ValorCuotaBE
        Dim dtValorCuotaSerie As DataTable
        oValorCuotaBESerie = ObjValorCuota.SeleccionarValorCuota(ViewState("Portafolio"), "", UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim))
        dtValorCuotaSerie = oValorCuotaBESerie.Tables(0)
        '-----------
        If dtValorCuotaSerie.Rows.Count > 0 Then
            tbInversionesSubTotal.Text = Math.Round((dtValorCuotaSerie.Rows(0)("InversionesSubTotal") * Decimal.Parse(ViewState("Porcentaje"))) / 100, 7)
            tbCXCVentaTitulo.Text = Math.Round((dtValorCuotaSerie.Rows(0)("CXCVentaTitulo") * Decimal.Parse(ViewState("Porcentaje"))) / 100, 7)
            tbCXPtitulo.Text = Math.Round((dtValorCuotaSerie.Rows(0)("CXPCompraTitulo") * Decimal.Parse(ViewState("Porcentaje"))) / 100, 7)
            tbCXPCompraTitulo.Text = Math.Round((dtValorCuotaSerie.Rows(0)("CXPCompraTitulo") * Decimal.Parse(ViewState("Porcentaje"))) / 100, 7)
            tbCajaPrecierre.Text = Math.Round((dtValorCuotaSerie.Rows(0)("CajaPrecierre") * Decimal.Parse(ViewState("Porcentaje"))) / 100, 7)


            tbCXPPreCierre.Text = tbCXPtitulo.Text
            tbCXCPreCierre.Text = tbCXCVentaTitulo.Text
            tbCajaCierre.Text = tbCajaPrecierre.Text

            'tbOtrosGastos.Text = Math.Round((dtValorCuotaSerie.Rows(0)("OtrosGastos") * Decimal.Parse(ViewState("Porcentaje"))) / 100, 7)
            'tbOtrosIngresos.Text = Math.Round((dtValorCuotaSerie.Rows(0)("OtrosIngresos") * Decimal.Parse(ViewState("Porcentaje"))) / 100, 7)
            'tbOtrosGastos.Text = 0
            'tbOtrosIngresos.Text = 0

            'tbValValoresPrecierre.Text = Request.QueryString("ValorCierre")
            tbValCuotaPrecierre.Text = Request.QueryString("CuotaCierre")
            tbCXPotrasCierre.Text = "0"
            CargarValoresCalculados = True
        Else
            tbComiSAFM.Text = "0"
            tbValPatriPrecierre2.Text = "0"
            tbValPatriPrecierre.Text = "0"
            tbValValoresPrecierre.Text = "0"
            tbAporteCuota.Text = "0"
            tbAporteValores.Text = "0"
            tbRescatesCuota.Text = "0"
            tbRescatesValores.Text = "0"
            tbCXCVentaTituloCierre.Text = "0"
            tbCXCCierre.Text = "0"
            tbCXPtituloCierre.Text = "0"
            tbCXPotrasCierre.Text = "0"
            tbOtrasCXPCierre.Text = "0"
            tbOtrosGastosCierre.Text = "0"
            tbOtrosIngresosCierre.Text = "0"
            tbOtrosGastosExlusivos.Text = "0"
            tbOtrosIngresosExlusivos.Text = "0"
            tbCXCExclusivosCierre.Text = "0"
            tbCXPExclusivosCierre.Text = "0"
            tbOtrosGastosExlusivosCierre.Text = "0"
            tbOtrosIngresosExlusivosCierre.Text = "0"
            tbValPatCierreCuota.Text = "0"
            tbValPatCierreValores.Text = "0"
            tbValCuotaCierreCuota.Text = "0"
            tbValCuotaCierreValores.Text = "0"
            tbCXPCompraTituloCierre.Text = "0"
            txtVCA.Text = "0"
            txtVCDiferencia.Text = "0"
            tbOtrosCXCCierre.Text = "0"
            tbOtrosGastos.Text = "0"
            tbOtrosIngresos.Text = "0"
            tbInversionesSubTotal.Text = "0"
            tbCXCVentaTitulo.Text = "0"
            tbCXPtitulo.Text = "0"
            tbCXPCompraTitulo.Text = "0"
            tbCajaPrecierre.Text = "0"

        End If


    End Function


    Private Function ExisteValorCuota() As Integer
        Dim oValorCuotaBESerie As New ValorCuotaBE
        oValorCuotaBESerie = ObjValorCuota.SeleccionarValorCuota(ViewState("Portafolio"), ViewState("NroReg"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim))
        Return oValorCuotaBESerie.Tables(0).Rows.Count
    End Function

    Private Function CargarValoresIngresados2() As Boolean
        CargarValoresIngresados2 = False
        Dim oValorCuotaBESerie As New ValorCuotaBE
        Dim dtValorCuotaSerie As DataTable
        oValorCuotaBESerie = ObjValorCuota.SeleccionarValorCuota(ViewState("Portafolio"), ViewState("NroReg"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim))
        dtValorCuotaSerie = oValorCuotaBESerie.Tables(1)
        If dtValorCuotaSerie.Rows.Count > 0 Then
            'tbOtrasCXC.Text = dtValorCuotaSerie.Rows(0)("OtrasCXC")
            'tbCXPCompraTitulo.Text = dtValorCuotaSerie.Rows(0)("CXPCompraTitulo")
            tbOtrasCXP.Text = dtValorCuotaSerie.Rows(0)("OtrasCXP")
            hdAjustesCxPPrecierre.Value = dtValorCuotaSerie.Rows(0)("AjustesCXP") 'Obtener los ajustes CxP
            tbOtrosGastos.Text = dtValorCuotaSerie.Rows(0)("OtrosGastos")
            tbOtrosIngresos.Text = dtValorCuotaSerie.Rows(0)("OtrosIngresos")
            tbCXCPreCierre.Text = dtValorCuotaSerie.Rows(0)("CXCPreCierre")
            tbCXPPreCierre.Text = dtValorCuotaSerie.Rows(0)("CXPPreCierre")
            '--------------------
            'tbCXCPreCierre.Text = oRowSerie("CXCPreCierre")
            'tbCXPPreCierre.Text = oRowSerie("CXPPreCierre")
            tbComiSAFM.Text = dtValorCuotaSerie.Rows(0)("ComisionSAFM")
            tbValPatriPrecierre2.Text = dtValorCuotaSerie.Rows(0)("ValPatriPreCierre2")
            tbValPatriPrecierre.Text = dtValorCuotaSerie.Rows(0)("ValPatriPreCierre1")
            tbValValoresPrecierre.Text = dtValorCuotaSerie.Rows(0)("ValCuotaPreCierreVal")
            tbAporteCuota.Text = dtValorCuotaSerie.Rows(0)("AportesCuotas")
            tbAporteValores.Text = dtValorCuotaSerie.Rows(0)("AportesValores")
            tbRescatesCuota.Text = dtValorCuotaSerie.Rows(0)("RescateCuotas")
            tbRescatesValores.Text = dtValorCuotaSerie.Rows(0)("RescateValores")
            tbCXCVentaTituloCierre.Text = dtValorCuotaSerie.Rows(0)("CXCVentaTitulo")
            tbCXCCierre.Text = dtValorCuotaSerie.Rows(0)("CXCCierre")
            tbCXPtituloCierre.Text = dtValorCuotaSerie.Rows(0)("CXPCompraTituloCierre")
            tbCXPotrasCierre.Text = dtValorCuotaSerie.Rows(0)("OtrasCXPCierre")
            tbOtrasCXPCierre.Text = dtValorCuotaSerie.Rows(0)("CXPCierre")
            tbOtrosGastosCierre.Text = dtValorCuotaSerie.Rows(0)("OtrosGastosCierre")
            tbOtrosIngresosCierre.Text = dtValorCuotaSerie.Rows(0)("OtrosIngresosCierre")
            tbOtrosGastosExlusivos.Text = dtValorCuotaSerie.Rows(0)("OtrosGastosExclusivos")
            tbOtrosIngresosExlusivos.Text = dtValorCuotaSerie.Rows(0)("OtrosIngresosExclusivos")
            tbCXCExclusivosCierre.Text = dtValorCuotaSerie.Rows(0)("OtrosCXCExclusivoCierre")
            tbCXPExclusivosCierre.Text = dtValorCuotaSerie.Rows(0)("OtrasCXPExclusivoCierre")
            tbOtrosGastosExlusivosCierre.Text = dtValorCuotaSerie.Rows(0)("OtrosGastosExclusivosCierre")
            tbOtrosIngresosExlusivosCierre.Text = dtValorCuotaSerie.Rows(0)("OtrosIngresosExclusivosCierre")
            tbValPatCierreCuota.Text = dtValorCuotaSerie.Rows(0)("ValPatriCierreCuota")
            tbValPatCierreValores.Text = dtValorCuotaSerie.Rows(0)("ValPatriCierreValores")
            tbValCuotaCierreCuota.Text = dtValorCuotaSerie.Rows(0)("ValCuotaCierre")
            tbValCuotaCierreValores.Text = dtValorCuotaSerie.Rows(0)("ValCuotaValoresCierre")
            tbCXPCompraTituloCierre.Text = dtValorCuotaSerie.Rows(0)("CXPCompraTituloCierre")
            txtVCA.Text = IIf(txtVCA.Text = String.Empty, "0", txtVCA.Text).ToString()
            txtVCDiferencia.Text = Decimal.Parse(tbValCuotaCierreValores.Text) - Decimal.Parse(txtVCA.Text)
            tbOtrosCXCCierre.Text = dtValorCuotaSerie.Rows(0)("OtrosCXCCierre")
            'Nuevos campos agregados
            txtAporteLiberadas.Text = dtValorCuotaSerie.Rows(0)("AportesLiberadas")
            txtRetencionPendiente.Text = dtValorCuotaSerie.Rows(0)("RetencionPendiente")
            '*********************************************************************************************************************
            'Asignar venta de títulos para los portafolios con atributo Valor cuota liberadas
            '*********************************************************************************************************************
            If Request.QueryString("CuotasLiberadas") = "1" Then
                If tbSerie.Text = "SERA" Then
                    txtDividendosDetalle.Text = ViewState("CxCMontosDividendos")
                    txtVentaTitulosDetalle.Text = tbCXCVentaTitulo.Text
                    tbCXCVentaTitulo.Text = Math.Round(Decimal.Parse(tbCXCVentaTitulo.Text) + Decimal.Parse(txtDividendosDetalle.Text), 7)
                Else
                    'Obtiene el monto total de dividendos que aún no liquidan
                    'txtMontoDividendosCierre.Text = dtValorCuotaSerie.Rows(0)("CXCVentaTituloDividendos")
                    txtMontoDividendosCierre.Text = ViewState("CxCMontosDividendos")
                    txtMontoDividendosPrecierre.Text = txtMontoDividendosCierre.Text
                End If
            End If
            '*********************************************************************************************************************
            tbValCuotaPrecierre.Text = dtValorCuotaSerie.Rows(0)("ValCuotaPreCierre")
            'oValorCuotaBESerie = ObjValorCuota.SeleccionarValorCuota(ViewState("Portafolio"), "", UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim))
            '  dtValorCuotaSerie = oValorCuotaBESerie.Tables(1)
            'tbCajaPrecierre.Text = Math.Round((dtValorCuotaSerie.Rows(0)("CajaPreCierre") * ViewState("Porcentaje")) / 100, 7)
            ' tbOtrasCXC.Text = Math.Round((dtValorCuotaSerie.Rows(0)("OtrasCXC") * ViewState("Porcentaje")) / 100, 7)
            tbCajaCierre.Text = tbCajaPrecierre.Text
            tbCajaPrecierre.ReadOnly = True
            'Agregar valor inicial de rescate de valores
            tbRescatesValores.Text = ObtenerRescateValores()
            CargarValoresIngresados2 = True
        End If
    End Function
    Public Sub Modificar_Porcentaje_Serie()
        Dim objPortafolioBM As New PortafolioBM
        objPortafolioBM.Modificar_PorcentajeSeries(Request.QueryString("Portafolio"), Request.QueryString("NroReg"), UIUtility.ConvertirFechaaDecimal(Request.QueryString("Fecha")), Request.QueryString("Porcentaje"), Me.DatosRequest)
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oValorCuotaBE As New ValorCuotaBE
            Dim rowValorCuota As ValorCuotaBE.ValorCuotaRow
            Dim rowDetalleCuota As ValorCuotaBE.ValorCuotaSerieRow
            rowValorCuota = CType(oValorCuotaBE.ValorCuota.NewRow(), ValorCuotaBE.ValorCuotaRow)
            rowDetalleCuota = CType(oValorCuotaBE.ValorCuotaSerie.NewRow(), ValorCuotaBE.ValorCuotaSerieRow)
            CargarObjetoDetalle(rowDetalleCuota)
            CargarObjetoCabecera(rowValorCuota, ViewState("NroReg"))
            oValorCuotaBE.ValorCuota.AddValorCuotaRow(rowValorCuota)
            oValorCuotaBE.ValorCuotaSerie.AddValorCuotaSerieRow(rowDetalleCuota)
            oValorCuotaBE.AcceptChanges()
            ObjValorCuota.Insertar_ValorCuota(oValorCuotaBE, DatosRequest)
            AlertaJS("Registro guardado correctamente.")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        Finally
            btnAceptar.Text = "Aceptar"
            btnAceptar.Enabled = True
        End Try
    End Sub
   

    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            CargarValoresCalculados()
            Dim ValCuotaPrecierre As String = Replace(IIf(tbValCuotaPrecierre.Text.Trim = "", "0", tbValCuotaPrecierre.Text.Trim), ",", "")
            Dim ValValoresPrecierre As String = Replace(IIf(tbValValoresPrecierre.Text.Trim = "", "0", tbValValoresPrecierre.Text.Trim), ",", "")
            If Decimal.Parse(ValCuotaPrecierre) = 0 And Decimal.Parse(ValValoresPrecierre) = 0 Then
                AlertaJS("Debe Ingresar V. Cuota Precierre (Cuota) o V. Cuota Precierre (Valor)")
                Exit Sub
            End If
            Dim InversionesSubtotal As String = Replace(IIf(tbInversionesSubTotal.Text.Trim = "", "0", tbInversionesSubTotal.Text.Trim), ",", "")
            Dim CajaPrecierre As String = Replace(IIf(tbCajaPrecierre.Text.Trim = "", "0", tbCajaPrecierre.Text.Trim), ",", "")
            Dim CXCTitulo As String = Replace(IIf(tbCXCVentaTitulo.Text.Trim = "", "0", tbCXCVentaTitulo.Text.Trim), ",", "")
            Dim CXCOtras As String = Replace(IIf(tbOtrasCXC.Text.Trim = "", "0", tbOtrasCXC.Text.Trim), ",", "")
            Dim CXPTitulo As String = Replace(IIf(tbCXPtitulo.Text.Trim = "", "0", tbCXPtitulo.Text.Trim), ",", "")
            Dim CXPOtras As String = Replace(IIf(tbOtrasCXP.Text.Trim = "", "0", tbOtrasCXP.Text.Trim), ",", "")
            Dim OtrosGastos As String = Replace(IIf(tbOtrosGastos.Text.Trim = "", "0", tbOtrosGastos.Text.Trim), ",", "")
            Dim OtrosIngresos As String = Replace(IIf(tbOtrosIngresos.Text.Trim = "", "0", tbOtrosIngresos.Text.Trim), ",", "")
            Dim AporteValores As String = Replace(IIf(tbAporteValores.Text.Trim = "", "0", tbAporteValores.Text.Trim), ",", "")
            Dim RescatesValores As String = Replace(IIf(tbRescatesValores.Text.Trim = "", "0", tbRescatesValores.Text.Trim), ",", "")
            Dim otrasCXCExclusivos As String = Replace(IIf(tbotrasCXCExclusivos.Text.Trim = "", "0", tbotrasCXCExclusivos.Text.Trim), ",", "")
            Dim OtrosGastosExlusivos As String = Replace(IIf(tbOtrosGastosExlusivos.Text.Trim = "", "0", tbOtrosGastosExlusivos.Text.Trim), ",", "")
            Dim CXPExclusivos As String = Replace(IIf(tbCXPExclusivos.Text.Trim = "", "0", tbCXPExclusivos.Text.Trim), ",", "")
            Dim OtrosIngresosExlusivos As String = Replace(IIf(tbOtrosIngresosExlusivos.Text.Trim = "", "0", tbOtrosIngresosExlusivos.Text.Trim), ",", "")
            Dim CXPCompraTitulo As String = Replace(IIf(tbCXPCompraTitulo.Text.Trim = "", "0", tbCXPCompraTitulo.Text.Trim), ",", "")
            Dim OtrosGastosCierre As String = Replace(IIf(tbOtrosGastosCierre.Text.Trim = "", "0", tbOtrosGastosCierre.Text.Trim), ",", "")
            Dim OtrosGastosExlusivosCierre As String = Replace(IIf(tbOtrosGastosExlusivosCierre.Text.Trim = "", "0", tbOtrosGastosExlusivosCierre.Text.Trim), ",", "")
            Dim OtrosIngresosCierre As String = Replace(IIf(tbOtrosIngresosCierre.Text.Trim = "", "0", tbOtrosIngresosCierre.Text.Trim), ",", "")
            Dim OtrosIngresosExlusivosCierre As String = Replace(IIf(tbOtrosIngresosExlusivosCierre.Text.Trim = "", "0", tbOtrosIngresosExlusivosCierre.Text.Trim), ",", "")
            Dim VCAnterior As String = Replace(IIf(txtVCA.Text.Trim = "", "0", txtVCA.Text.Trim), ",", "")
            Dim MontoDividendosPrecierre As String = Replace(IIf(txtMontoDividendosPrecierre.Text.Trim = "", "0", txtMontoDividendosPrecierre.Text.Trim), ",", "")
            Dim MontoDividendosCierre As String = Replace(IIf(txtMontoDividendosCierre.Text.Trim = "", "0", txtMontoDividendosCierre.Text.Trim), ",", "")
            Dim AporteLiberadas As String = Replace(IIf(txtAporteLiberadas.Text.Trim = "", "0", txtAporteLiberadas.Text.Trim), ",", "")
            Dim RetencionPendiente As String = Replace(IIf(txtRetencionPendiente.Text.Trim = "", "0", txtRetencionPendiente.Text.Trim), ",", "")
            Dim CxPLiberadaPreCierre As String = Replace(IIf(txtCxPLiberadaPreCierre.Text.Trim = "", "0", txtCxPLiberadaPreCierre.Text.Trim), ",", "")
            Dim diferenciaAporteLiberada As Decimal = 0.0
            'Calculo Valor Patrimonio precierre1
            ' tbValPatriPrecierre.Text = Math.Round(Decimal.Parse(InversionesSubtotal) + Decimal.Parse(CajaPrecierre) + _
            '                           Decimal.Parse(CXCTitulo) + Decimal.Parse(CXCOtras) + Decimal.Parse(otrasCXCExclusivos) - _
            '                           Decimal.Parse(CXPCompraTitulo) - Decimal.Parse(CXPOtras) - Decimal.Parse(CXPExclusivos) - _
            '                           Decimal.Parse(OtrosGastos) - Decimal.Parse(OtrosGastosExlusivos) + Decimal.Parse(OtrosIngresos) + _
            '                          Decimal.Parse(OtrosIngresosExlusivos) - Decimal.Parse(hdAjustesCxPPrecierre.Value), 7)

            tbValPatriPrecierre.Text = Math.Round(Decimal.Parse(InversionesSubtotal) + Decimal.Parse(CXCTitulo) - Decimal.Parse(CXPTitulo) + Decimal.Parse(CajaPrecierre) - Decimal.Parse(OtrosGastos) + Decimal.Parse(OtrosIngresos), 7)
            'Calculando la Comisión SAFM
            Dim Porcomi As Decimal = ObjValorCuota.PorcentajeComisionSerie(ViewState("Portafolio"), tbSerie.Text)

            'Para FIRBI el cálculo se considera Mensual por lo tanto el porcentaje utilizado es "PorcComisionMensual"
            Dim PorcComisionDiario As Decimal = (Porcomi / 100) / 360 ' DIARIO
            Dim PorcComisionMensual As Decimal = (Porcomi / 100) / 12 ' MENSUAL

            Dim comiSAFM As Decimal = PorcComisionMensual * Decimal.Parse(tbValPatriPrecierre.Text.Trim)
            'tbComiSAFM.Text = Math.Round(comiSAFM * (1 + ParametrosSIT.IGV), 7)
            tbComiSAFM.Text = Math.Round(comiSAFM, 7)

            'Calculo Valor Patrimonio precierre2
            tbValPatriPrecierre2.Text = Math.Round(Decimal.Parse(tbValPatriPrecierre.Text.Trim) - Decimal.Parse(tbComiSAFM.Text.Trim), 7)
            'tbValPatriPrecierre2.Text = Math.Round(Decimal.Parse(tbValPatriPrecierre.Text.Trim) - Decimal.Parse(tbComiSAFM.Text.Trim) _
            '                                       + Decimal.Parse(MontoDividendosPrecierre), 7) 'OT10981 - 05/12/2017 - Ian Pastor M. Agregar suma de dividendos liberados

            'Agregar suma CxP liberadas al patrimonio pre-cierre 2
            ' tbValPatriPrecierre2.Text = Math.Round(Decimal.Parse(tbValPatriPrecierre.Text.Trim) - comiSAFM _
            '    + Decimal.Parse(MontoDividendosPrecierre) - Decimal.Parse(CxPLiberadaPreCierre), 7)

            'Monto Valor Cuota Precierre
            'If Decimal.Parse(ValCuotaPrecierre) <> 0 Then
            '    tbValValoresPrecierre.Text = Math.Round(Decimal.Parse(tbValPatriPrecierre2.Text.Trim) / Decimal.Parse(ValCuotaPrecierre), 7)
            '    RescatesValores = ObtenerRescateValores()
            '    tbRescatesValores.Text = RescatesValores
            'End If
            If Decimal.Parse(tbValValoresPrecierre.Text) <> 0 Then
                tbAporteCuota.Text = Math.Round((Decimal.Parse(AporteValores) + Decimal.Parse(AporteLiberadas)) / Decimal.Parse(tbValValoresPrecierre.Text.Trim), 7)
                'Calcular Rescate Cuos
                tbRescatesCuota.Text = Math.Round(Decimal.Parse(RescatesValores) / Decimal.Parse(tbValValoresPrecierre.Text.Trim), 7)
                'CALCULAR EL VALOR PATRIMONIO CIERRE
                tbValPatCierreCuota.Text = Math.Round(Decimal.Parse(ValCuotaPrecierre) + Decimal.Parse(tbAporteCuota.Text.Trim) - Decimal.Parse(tbRescatesCuota.Text.Trim), 7)
                tbValPatCierreValores.Text = Math.Round(Decimal.Parse(tbValPatriPrecierre2.Text.Trim) + Decimal.Parse(AporteValores) - Decimal.Parse(RescatesValores), 7)
                tbValPatCierreValores.Text = CDec(tbValPatCierreValores.Text) + (CDec(OtrosIngresosExlusivosCierre) + CDec(OtrosIngresosCierre)) -
                (CDec(OtrosGastosCierre) + CDec(OtrosGastosExlusivosCierre)) + Decimal.Parse(AporteLiberadas)
                'EL VALOR CUOTA CIERRE
                tbValCuotaCierreCuota.Text = Math.Round(Decimal.Parse(tbValPatCierreCuota.Text.Trim), 7)
                If Decimal.Parse(tbValCuotaCierreCuota.Text) <> 0 Then
                    tbValCuotaCierreValores.Text = Math.Round(Decimal.Parse(tbValPatCierreValores.Text.Trim) / Decimal.Parse(tbValCuotaCierreCuota.Text.Trim), 7)
                Else
                    tbValCuotaCierreValores.Text = 0
                End If
                If tbSerie.Text = "SERB" Then
                    If Decimal.Parse(RetencionPendiente) > 0 Then
                        diferenciaAporteLiberada = Decimal.Parse(RetencionPendiente)
                    End If
                End If

                'Agregar suma aporte liberadas
                tbCXPotrasCierre.Text = Math.Round(Decimal.Parse(CXPOtras) + Decimal.Parse(hdAjustesCxPPrecierre.Value) + Decimal.Parse(tbComiSAFM.Text.Trim) - Decimal.Parse(AporteValores) + Decimal.Parse(RescatesValores) _
                                                   + diferenciaAporteLiberada - Decimal.Parse(AporteLiberadas) + Decimal.Parse(CxPLiberadaPreCierre), 7)
                tbCXPotrasCierre.Text = "0"
            End If
            'Titulo
            tbCXCVentaTituloCierre.Text = Decimal.Parse(CXCTitulo)
            'El monto dividendo del Precierre tiene que ser igual a del Cierre.
            MontoDividendosCierre = MontoDividendosPrecierre
            tbCXPtituloCierre.Text = tbCXPtitulo.Text
            'Totales
            'Se agrega el monto de dividendos al total de CxC Precierre.
            'tbCXCPreCierre.Text = Math.Round(Decimal.Parse(CXCTitulo) + Decimal.Parse(tbOtrasCXC.Text) + Decimal.Parse(MontoDividendosPrecierre), 7)
            'tbCXPPreCierre.Text = Math.Round(Decimal.Parse(tbCXPtitulo.Text) + Decimal.Parse(tbOtrasCXP.Text) + Decimal.Parse(hdAjustesCxPPrecierre.Value), 7)
            tbOtrosCXCCierre.Text = tbOtrasCXC.Text 'Desc: Estos dos valores siempre son iguales.
            'Se agrega el monto de dividendos al total de CxC Cierre.
            tbCXCCierre.Text = Math.Round(Decimal.Parse(tbCXCVentaTituloCierre.Text) + Decimal.Parse(tbOtrosCXCCierre.Text) + Decimal.Parse(MontoDividendosCierre), 7)
            tbOtrasCXPCierre.Text = Math.Round(Decimal.Parse(tbCXPtituloCierre.Text) + Decimal.Parse(tbCXPotrasCierre.Text), 7)
            btnAceptar.Visible = True
            'Diferencia entre el valor cuota del dia anterior y el calculado Hoy
            'txtVCDiferencia.Text = Decimal.Parse(tbValCuotaCierreValores.Text) - Decimal.Parse(VCAnterior)
            txtVCDiferencia.Text = "0"
            'ObtenerMontosContablesFirbi(Request.QueryString("Portafolio"), Request.QueryString("Fecha"), Decimal.Parse(Request.QueryString("Porcentaje")))
            Dim precierre2 As String = Replace(IIf(tbValPatriPrecierre2.Text.Trim = "", "0", tbValPatriPrecierre2.Text.Trim), ",", "")
            Dim cuotaVal As String = Replace(IIf(tbValCuotaPrecierre.Text.Trim = "", "0", tbValCuotaPrecierre.Text.Trim), ",", "")

            tbValValoresPrecierre.Text = Math.Round((Decimal.Parse(precierre2) / Decimal.Parse(cuotaVal)), 7)
            tbCXPotrasCierre.Text = tbComiSAFM.Text
            CargarValoresCalculados()
            tbValPatCierreCuota.Text = tbValCuotaPrecierre.Text
            tbValCuotaCierreCuota.Text = tbValCuotaPrecierre.Text
            tbValPatCierreValores.Text = tbValPatriPrecierre2.Text

            tbCXPotrasCierre.Text = tbComiSAFM.Text
            tbValCuotaCierreValores.Text = tbValValoresPrecierre.Text
            tbCXPotrasCierre.Text = Math.Round(Decimal.Parse(tbCXPtitulo.Text) + Decimal.Parse(tbComiSAFM.Text), 7)
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        Finally
            btnProcesar.Text = "Procesar"
            btnProcesar.Enabled = True

        End Try
    End Sub
    Private Sub CargarObjetoCabecera(ByVal rowValorCuota As ValorCuotaBE.ValorCuotaRow, Optional ByVal CodigoSerie As String = "")
        rowValorCuota.CodigoPortafolioSBS = ViewState("Portafolio")
        rowValorCuota.CodigoSerie = CodigoSerie
        rowValorCuota.FechaProceso = UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim)
        rowValorCuota.InversionesT1 = IIf(tbInversionesT1.Text = "", 0, tbInversionesT1.Text)
        rowValorCuota.VentasVencimientos = IIf(tbVentasVenciT.Text = "", 0, tbVentasVenciT.Text)
        rowValorCuota.Rentabilidad = IIf(tbRentabilidadT.Text = "", 0, tbRentabilidadT.Text)
        rowValorCuota.ValForwards = IIf(tbValoriForwardT.Text = "", 0, tbValoriForwardT.Text)
        rowValorCuota.ValSwaps = 0
        rowValorCuota.InversionesSubTotal = IIf(tbInversionesSubTotal.Text = "", 0, tbInversionesSubTotal.Text)
    End Sub
    Private Sub CargarObjetoDetalle(ByVal rowDetalleCuota As ValorCuotaBE.ValorCuotaSerieRow)
        rowDetalleCuota.CajaPreCierre = IIf(tbCajaPrecierre.Text = "", 0, tbCajaPrecierre.Text)
        rowDetalleCuota.CXCVentaTitulo = IIf(tbCXCVentaTitulo.Text = "", 0, tbCXCVentaTitulo.Text)
        rowDetalleCuota.OtrasCXC = IIf(tbOtrasCXC.Text = "", 0, tbOtrasCXC.Text)
        rowDetalleCuota.CXCPreCierre = IIf(tbCXCPreCierre.Text = "", 0, tbCXCPreCierre.Text)
        rowDetalleCuota.CXPCompraTitulo = IIf(tbCXPtitulo.Text = "", 0, tbCXPtitulo.Text)
        rowDetalleCuota.OtrasCXP = IIf(tbOtrasCXP.Text = "", 0, tbOtrasCXP.Text)
        rowDetalleCuota.CXPPreCierre = IIf(tbCXPPreCierre.Text = "", 0, tbCXPPreCierre.Text)
        rowDetalleCuota.OtrosGastos = IIf(tbOtrosGastos.Text = "", 0, tbOtrosGastos.Text)
        rowDetalleCuota.OtrosIngresos = IIf(tbOtrosIngresos.Text = "", 0, tbOtrosIngresos.Text)
        rowDetalleCuota.ValPatriPreCierre1 = IIf(tbValPatriPrecierre.Text = "", 0, tbValPatriPrecierre.Text)
        rowDetalleCuota.ComisionSAFM = IIf(tbComiSAFM.Text = "", 0, tbComiSAFM.Text)
        rowDetalleCuota.ValPatriPreCierre2 = IIf(tbValPatriPrecierre2.Text = "", 0, tbValPatriPrecierre2.Text)
        rowDetalleCuota.ValCuotaPreCierre = IIf(tbValCuotaPrecierre.Text = "", 0, tbValCuotaPrecierre.Text)
        rowDetalleCuota.ValCuotaPreCierreVal = IIf(tbValValoresPrecierre.Text = "", 0, tbValValoresPrecierre.Text)
        rowDetalleCuota.AportesCuotas = IIf(tbAporteCuota.Text = "", 0, tbAporteCuota.Text)
        rowDetalleCuota.AportesValores = IIf(tbAporteValores.Text = "", 0, tbAporteValores.Text)
        rowDetalleCuota.RescateCuotas = IIf(tbRescatesCuota.Text = "", 0, tbRescatesCuota.Text)
        rowDetalleCuota.RescateValores = IIf(tbRescatesValores.Text = "", 0, tbRescatesValores.Text)
        rowDetalleCuota.Caja = IIf(tbCajaCierre.Text = "", 0, tbCajaCierre.Text)
        rowDetalleCuota.CXCVentaTituloCierre = IIf(tbCXCVentaTituloCierre.Text = "", 0, tbCXCVentaTituloCierre.Text)
        rowDetalleCuota.OtrosCXCCierre = IIf(tbOtrosCXCCierre.Text = "", 0, tbOtrosCXCCierre.Text)
        rowDetalleCuota.CXCCierre = IIf(tbCXCCierre.Text = "", 0, tbCXCCierre.Text)
        rowDetalleCuota.CXPCompraTituloCierre = IIf(tbCXPtituloCierre.Text = "", 0, tbCXPtituloCierre.Text)
        rowDetalleCuota.OtrasCXPCierre = IIf(tbCXPotrasCierre.Text = "", 0, tbCXPotrasCierre.Text)
        rowDetalleCuota.CXPCierre = IIf(tbOtrasCXPCierre.Text = "", 0, tbOtrasCXPCierre.Text)
        rowDetalleCuota.OtrosGastosCierre = IIf(tbOtrosGastosCierre.Text = "", 0, tbOtrosGastosCierre.Text)
        rowDetalleCuota.OtrosIngresosCierre = IIf(tbOtrosIngresosCierre.Text = "", 0, tbOtrosIngresosCierre.Text)
        rowDetalleCuota.ValPatriCierreCuota = IIf(tbValPatCierreCuota.Text = "", 0, tbValPatCierreCuota.Text)
        rowDetalleCuota.ValPatriCierreValores = IIf(tbValPatCierreValores.Text = "", 0, tbValPatCierreValores.Text)
        rowDetalleCuota.ValCuotaCierre = IIf(tbValCuotaCierreCuota.Text = "", 0, tbValCuotaCierreCuota.Text)
        rowDetalleCuota.ValCuotaValoresCierre = IIf(tbValCuotaCierreValores.Text = "", 0, tbValCuotaCierreValores.Text)
        rowDetalleCuota.OtrasCXCExclusivos = IIf(tbotrasCXCExclusivos.Text = "", 0, tbotrasCXCExclusivos.Text)
        rowDetalleCuota.OtrasCXPExclusivos = IIf(tbCXPExclusivos.Text = "", 0, tbCXPExclusivos.Text)
        rowDetalleCuota.OtrosGastosExclusivos = IIf(tbOtrosGastosExlusivos.Text = "", 0, tbOtrosGastosExlusivos.Text)
        rowDetalleCuota.OtrosIngresosExclusivos = IIf(tbOtrosIngresosExlusivos.Text = "", 0, tbOtrosIngresosExlusivos.Text)
        rowDetalleCuota.OtrosCXCExclusivoCierre = IIf(tbCXCExclusivosCierre.Text = "", 0, tbCXCExclusivosCierre.Text)
        rowDetalleCuota.OtrasCXPExclusivoCierre = IIf(tbCXPExclusivosCierre.Text = "", 0, tbCXPExclusivosCierre.Text)
        rowDetalleCuota.OtrosGastosExclusivosCierre = IIf(tbOtrosGastosExlusivosCierre.Text = "", 0, tbOtrosGastosExlusivosCierre.Text)
        rowDetalleCuota.OtrosIngresosExclusivosCierre = IIf(tbOtrosIngresosExlusivosCierre.Text = "", 0, tbOtrosIngresosExlusivosCierre.Text)
        rowDetalleCuota.InversionesSubTotalSerie = IIf(tbValCuotaCierreValores.Text = "", 0, tbValCuotaCierreValores.Text)
        rowDetalleCuota.ValCuotaPreCierreAnt = 0
        rowDetalleCuota.ComisionSAFMAnterior = "0"
        'Descripcion: Campos nuevos para el calculo de Otras CXP
        rowDetalleCuota.RescatePendiente = 0
        rowDetalleCuota.ChequePendiente = 0
        'Descripcion: Se cambia el valor del campo ComisionSAFMAnterior
        rowDetalleCuota.ComisionSAFMAnterior = 0
        rowDetalleCuota.AjustesCXP = IIf(hdAjustesCxPPrecierre.Value = "", 0, hdAjustesCxPPrecierre.Value)
        '*********************************************************************************************************************
        'Asignar venta de títulos para los portafolios con atributo Valor cuota liberadas
        '*********************************************************************************************************************
        If Request.QueryString("CuotasLiberadas") = "1" Then
            If tbSerie.Text = "SERA" Then
                rowDetalleCuota.CXCVentaTituloDividendos = IIf(txtDividendosDetalle.Text = "", 0, txtDividendosDetalle.Text)
            Else
                rowDetalleCuota.CXCVentaTituloDividendos = IIf(txtMontoDividendosCierre.Text = "", 0, txtMontoDividendosCierre.Text)
            End If
        Else
            rowDetalleCuota.CXCVentaTituloDividendos = 0
        End If
        '*********************************************************************************************************************
        'Nuevos campos agregados
        rowDetalleCuota.AportesLiberadas = IIf(txtAporteLiberadas.Text = "", 0, txtAporteLiberadas.Text)
        rowDetalleCuota.RetencionPendiente = IIf(txtRetencionPendiente.Text = "", 0, txtRetencionPendiente.Text)
        rowDetalleCuota.AjustesCXC = 0
        rowDetalleCuota.DevolucionComisionUnificada = 0
        rowDetalleCuota.OtrasCxCPreCierre = 0
    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        EjecutarJS("window.close();")
    End Sub
    'Obtener Porcentaje del día anterior
    Private Sub ObtenerPorcentajeSerieDiaAnterior()
        Dim oPortafolioBM As New PortafolioBM
        Dim dtPorcentaje As DataTable
        Dim fechaAnterior As String
        fechaAnterior = fnFechaAnteriorSinFeriado(tbFechaInforme.Text)
        dtPorcentaje = oPortafolioBM.Portafolio_Series_Cuotas(Request.QueryString("Portafolio"), UIUtility.ConvertirFechaaDecimal(fechaAnterior))
        ViewState("PorcentajeDiaAnterior") = dtPorcentaje.Select("CodigoSerie='" & Request.QueryString("Serie") & "'")(0)("Porcentaje")
    End Sub
    Private Sub CargarControlesCuotasLiberadas()
        txtMontoDividendosCierre.ReadOnly = True
        txtMontoDividendosPrecierre.ReadOnly = True
        If Request.QueryString("CuotasLiberadas") = "1" Then
            ObtenerPorcentajeSerieDiaAnterior()
            txtAporteLiberadas.ReadOnly = False
            txtCxPLiberadaPreCierre.ReadOnly = False
        Else
            ViewState("PorcentajeDiaAnterior") = Request.QueryString("Porcentaje")
            txtAporteLiberadas.ReadOnly = True
            txtCxPLiberadaPreCierre.ReadOnly = True
        End If
    End Sub
    'Asignar venta de títulos para los portafolios con atributo Valor cuota liberadas
    Private Function ObtenerCxCMontosDividendos() As Decimal
        ObtenerCxCMontosDividendos = 0
        Dim objPortafolioBM As New PortafolioBM
        Dim dtPorcentaje As DataTable
        Dim fechaAnterior As String = String.Empty
        Dim porcentajeAnterior As Decimal = 0
        Dim montoDividendos As Decimal = 0
        Dim objDividendosBM As New DividendosRebatesLiberadasBM
        Dim dt As DataTable = objDividendosBM.DividendosRebatesLiberadas_ObtenerMontosDividendosDetallado(Request.QueryString("Portafolio"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text))
        For Each dr As DataRow In dt.Rows
            fechaAnterior = fnFechaAnteriorSinFeriado(UIUtility.ConvertirFechaaString(Decimal.Parse(dr("FechaOperacion"))))
            dtPorcentaje = objPortafolioBM.Portafolio_Series_Cuotas(Request.QueryString("Portafolio"), UIUtility.ConvertirFechaaDecimal(fechaAnterior))
            porcentajeAnterior = dtPorcentaje.Select("CodigoSerie='" & Request.QueryString("Serie") & "'")(0)("Porcentaje")
            montoDividendos = montoDividendos + ((dr("MontoOperacion") * porcentajeAnterior) / 100)
        Next
        montoDividendos = Math.Round(montoDividendos, 7)
        ObtenerCxCMontosDividendos = montoDividendos
    End Function
    Protected Sub imgDetalleVT_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgDetalleVT.Click
        EjecutarJS("MostrarDetalleVentaTitulo();")
    End Sub

    Private Function ObtenerRescateValores() As Decimal
        ObtenerRescateValores = 0
        Dim dtRescateValores As DataTable
        Try
            dtRescateValores = ObjValorCuota.ValorCuota_ObtenerRescateValores(Request.QueryString("CodigoPortafolioSeriadoSO").ToString(), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text), "RES")
        Catch ex As Exception
            ObtenerRescateValores = Decimal.Parse(Replace(IIf(tbRescatesValores.Text.Trim = "", "0", tbRescatesValores.Text.Trim), ",", "")) 'Devuelve el valor que esté en la caja de texto.
            Exit Function
        End Try
        Dim valorCuotaDia As String = Replace(IIf(tbValValoresPrecierre.Text.Trim = "", "0", tbValValoresPrecierre.Text.Trim), ",", "")
        If Decimal.Parse(valorCuotaDia) = 0 Then
            ObtenerRescateValores = 0
            Exit Function
        End If
        Dim rescateCuotas As Decimal = 0
        Dim rescateValores As Decimal = 0
        If dtRescateValores.Rows.Count > 0 Then
            If Not IsDBNull(dtRescateValores.Rows(0)("VALOR1")) And Not IsDBNull(dtRescateValores.Rows(0)("VALOR2")) Then
                rescateCuotas = dtRescateValores.Rows(0)("VALOR2") * Decimal.Parse(valorCuotaDia)
                rescateValores = Math.Round(dtRescateValores.Rows(0)("VALOR1") + rescateCuotas, 7)
            End If
        End If
        If rescateValores > 0 Then 'Si el rescate valor es cero, se mantiene el valor de la caja de texto.
            ObtenerRescateValores = rescateValores
        Else
            ObtenerRescateValores = Decimal.Parse(Replace(IIf(tbRescatesValores.Text.Trim = "", "0", tbRescatesValores.Text.Trim), ",", "")) 'Devuelve el valor que esté en la caja de texto.
        End If
    End Function
    Private Sub InicializarBotones()
        btnAceptar.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnProcesar.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
    End Sub
End Class

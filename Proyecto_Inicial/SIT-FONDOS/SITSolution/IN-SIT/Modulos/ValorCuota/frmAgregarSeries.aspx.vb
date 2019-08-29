Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Imports CrystalDecisions.CrystalReports.Engine
Imports System.IO
Imports SistemaProcesosBL

Partial Class Modulos_ValorCuota_frmAgregarSeries
    Inherits BasePage
    Dim ValorCuota As New ValorCuotaBM
    Private bExisteAC As Boolean = False

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not Page.IsPostBack Then
                ViewState("ActualizacionAC") = False
                ViewState("NroReg") = Request.QueryString("NroReg")
                ViewState("Porcentaje") = Request.QueryString("Porcentaje")
                ViewState("Portafolio") = Request.QueryString("Portafolio")
                tbPortafolio.Text = Request.QueryString("DetPorta")
                tbFechaInforme.Text = Request.QueryString("Fecha")
                tbSerie.Text = Request.QueryString("Serie")
                hdCodigoPortafolioSisOpe.Value = Request.QueryString("CodigoPortafolioSeriadoSO")
                hdCheckOperaciones.Value = Request.QueryString("hidCheck")
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
                Dim mensaje As String = String.Empty 'OT10952 - 17/11/2017 - Ian Pastor M. Variable para obtener los mensajes de las validaciones
                ExisteValoracion = oCarteraTituloValoracion.ExisteValorizacionFechasPosteriores(ViewState("Portafolio"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text))
                ExistePrecierre = oCarteraTituloValoracion.Valida_PRECIERRE_CONSOLIDADO_OPERACIONES(CDate(tbFechaInforme.Text), CDate(tbFechaInforme.Text), hdCodigoPortafolioSisOpe.Value)
                If oRow.CodigoPortafolioSisOpe = "" Then
                    btnProcesar.Visible = False
                    mensaje = "El portafolio debe tener asociado un codigo para el Sistema de Operaciones, se debe completar la informacion."
                ElseIf ExistePrecierre > 0 Then
                    btnProcesar.Visible = False
                    Switch = True
                    mensaje = "Existe un precierre generado desde el sistema de Operaciones, no se puede modificar la informacion presentada."
                ElseIf ExisteValoracion > 0 Then
                    btnProcesar.Visible = False
                    Switch = True
                    mensaje = "La fecha no coincide con la ultima valorización, no se puede modificar la informacion presentada."
                ElseIf ExisteValorCuota() > 0 Then
                    'btnProcesar.Visible = False
                    'INICIO | rcolonia | Se comenta ya que ahora lo manejará de acuerdo al cierre de operaciones | 20181017
                    '    Switch = True  
                    'FIN | rcolonia | Se comenta ya que ahora lo manejará de acuerdo al cierre de operaciones | 20181017
                Else
                    btnProcesar.Visible = True
                End If

                'INICIO | PROYECTO SIT | ZOLUXIONES | CRumiche | 2018-08-17
                'Consultamos si ya existe un proceso de Calculo de Valor Cuota en Operaciones (Para la fecha y fondo determinado)

                Dim mostrarFrmSoloLectura As Boolean = False

                If oRow.CodigoPortafolioSisOpe.Trim.Length > 0 Then ' Si no tenemos el CODIGO DE FONDO OPERACIONES no podemos realizar la ejecucion en OPE
                    Dim objOperaciones As New PrecierreBO
                    Dim rowVCuotaHistorico As DataRow = objOperaciones.ObtenerDetalleValorCuotaCerrado(hdCodigoPortafolioSisOpe.Value, UIUtility.ConvertirStringaFecha(tbFechaInforme.Text))

                    mostrarFrmSoloLectura = (rowVCuotaHistorico IsNot Nothing)
                    btnEjecutarCierreOpe.Visible = Not mostrarFrmSoloLectura
                    btnSoloLecturaCierreOpe.Visible = mostrarFrmSoloLectura
                    If Not Switch Then Switch = mostrarFrmSoloLectura

                Else 'Metodo tradicional para determinar el formlario en solo lectura
                    btnEjecutarCierreOpe.Visible = False
                    btnSoloLecturaCierreOpe.Visible = False
                End If
                'FIN | PROYECTO SIT | ZOLUXIONES | CRumiche | 2018-08-17


                If Switch Then
                    CargarValoresCuota()
                    CargarValoresIngresados2()
                Else
                    CargarValoresCuota()
                    CargarValoresCalculados()
                    CargarValoresIngresados()
                End If


                'OT10927 - 17/11/2017 - Ian Pastor M. Verificar si existe mensaje para mostrar
                If mensaje.Length > 0 Then
                    AlertaJS(mensaje)
                End If
                'OT10927 - Fin
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub CargarValoresCuota()
        Dim DTValorCuota As DataTable
        DTValorCuota = ValorCuota.CalcularValoresCuota(ViewState("Portafolio"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim), ViewState("NroReg"))
        tbInversionesT1.Text = Math.Round((DTValorCuota.Rows(0)("InversionesT1") * ViewState("Porcentaje")) / 100, 7)
        tbComprasT.Text = Math.Round((DTValorCuota.Rows(0)("ComprasT") * ViewState("Porcentaje")) / 100, 7)
        tbVentasVenciT.Text = Math.Round((DTValorCuota.Rows(0)("VentasyVencimientosT") * ViewState("Porcentaje")) / 100, 7)
        tbRentabilidadT.Text = Math.Round((DTValorCuota.Rows(0)("RentabilidadT") * ViewState("Porcentaje")) / 100, 7)
        tbValoriForwardT.Text = Math.Round((DTValorCuota.Rows(0)("ValoracionForwards") * ViewState("Porcentaje")) / 100, 7)
        tbValoriSwapT.Text = Math.Round((DTValorCuota.Rows(0)("ValorizacionSwaps") * ViewState("Porcentaje")) / 100, 7)
        tbInversionesSubTotal.Text = Math.Round((DTValorCuota.Rows(0)("InversionesSubTotal") * ViewState("Porcentaje")) / 100, 7)
        tbCXCVentaTitulo.Text = Math.Round((DTValorCuota.Rows(0)("CXCVentaTitulo") * ViewState("Porcentaje")) / 100, 7)
        tbCXPtitulo.Text = Math.Round((DTValorCuota.Rows(0)("CXPCompraTitulo") * ViewState("Porcentaje")) / 100, 7)
        tbValCuotaPrecierre.Text = DTValorCuota.Rows(0)("CuotaPreCierreAnterior")
        txtVCA.Text = DTValorCuota.Rows(0)("VCAnterior")
        txtDevolucionComisionDiaria.Text = IIf(Decimal.Parse(Request.QueryString("DevolucionComisionDiaria")) = 0, 0, Math.Round((Decimal.Parse(Request.QueryString("DevolucionComisionDiaria")) * ViewState("Porcentaje")) / 100, 7))
        Dim ValCuotaPrecierre As String = Replace(IIf(tbValCuotaPrecierre.Text.Trim = "", "0", tbValCuotaPrecierre.Text.Trim), ",", "")
        If Decimal.Parse(ValCuotaPrecierre) = 0 Then
            tbValCuotaPrecierre.ReadOnly = False
            tbValValoresPrecierre.ReadOnly = False
        End If
        txtigv.Text = ParametrosSIT.IGV
        txtporcom.Text = Math.Round(Decimal.Parse(ValorCuota.PorcentajeComisionSerie(ViewState("Portafolio"), tbSerie.Text)), 2).ToString + " %"
        '==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-07-17 | Aplicación de Compra de Divisas con Liquidación Post Cierre
        'El requerimiento tomará efecto a partir de la fecha indicada por el usuario, Ejemplo: 2018-08-01
        Dim fechaInicioLiquidacionesPost As New DateTime(2018, 8, 15)
        If fechaInicioLiquidacionesPost <= Date.Now Then
            tbCXCVentaTitulo.Text = CDec(tbCXCVentaTitulo.Text) + DTValorCuota.Rows(0)("CxC_DivisasLiquidacionPost")
            tbCXPtitulo.Text = CDec(tbCXPtitulo.Text) + DTValorCuota.Rows(0)("CxP_DivisasLiquidacionPost")
        End If
        '==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-07-17 | Aplicación de Compra de Divisas con Liquidación Post Cierre

        If Request.QueryString("CuotasLiberadas") = "1" Then 'OT10927 - 21/11/2017 - Ian Pastor M. Validación para los portafolios con atributo Valor cuota liberadas
            If tbSerie.Text = "SERB" Then
                'OT10916 - 26/10/2017 - Ian Pastor M. Obtiene el monto total de dividendos que aún no liquidan
                'txtMontoDividendosPrecierre.Text = Math.Round((DTValorCuota.Rows(0)("CXCVentaTituloDividendos") * ViewState("PorcentajeDiaAnterior")) / 100, 7)
                txtMontoDividendosPrecierre.Text = ViewState("CxCMontosDividendos")
                'OT10916 - Fin
            End If
        Else
            tbCXCVentaTitulo.Text = Math.Round(((DTValorCuota.Rows(0)("CXCVentaTitulo") + DTValorCuota.Rows(0)("CXCVentaTituloDividendos") + DTValorCuota.Rows(0)("CxC_DivisasLiquidacionPost")) * ViewState("Porcentaje")) / 100, 7)
            txtDividendosDetalle.Text = Math.Round((DTValorCuota.Rows(0)("CXCVentaTituloDividendos") * ViewState("Porcentaje")) / 100, 7)
            txtVentaTitulosDetalle.Text = Math.Round((DTValorCuota.Rows(0)("CXCVentaTitulo") * ViewState("Porcentaje")) / 100, 7) 'OT10927
        End If
    End Sub
    Private Sub CargarValoresCalculados()
        Dim oValorCuotaBE As New ValorCuotaBE
        oValorCuotaBE = ValorCuota.SeleccionarValorCuota(ViewState("Portafolio"), "", UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim))
        If Not oValorCuotaBE.Tables(0).Rows.Count = 0 Then
            Dim oRowS As ValorCuotaBE.ValorCuotaSerieRow
            oRowS = DirectCast(oValorCuotaBE.ValorCuotaSerie.Rows(0), ValorCuotaBE.ValorCuotaSerieRow)
            tbCajaPrecierre.Text = Math.Round((oRowS("CajaPreCierre") * ViewState("Porcentaje")) / 100, 7)
            tbCajaCierre.Text = tbCajaPrecierre.Text
            tbOtrasCXC.Text = Math.Round((oRowS("OtrasCXC") * ViewState("Porcentaje")) / 100, 7)
            tbCXPCompraTitulo.Text = Math.Round((oRowS("CXPCompraTitulo") * ViewState("Porcentaje")) / 100, 7)
            tbOtrasCXP.Text = Math.Round((oRowS("OtrasCXP") * ViewState("Porcentaje")) / 100, 7)
            hdAjustesCxPPrecierre.Value = Math.Round((oRowS("AjustesCXP") * ViewState("Porcentaje")) / 100, 7) 'OT11103 - 01/02/2018 - Ian Pastor M. Obtener los ajustes CxP
            tbOtrosGastos.Text = Math.Round((oRowS("OtrosGastos") * ViewState("Porcentaje")) / 100, 7)
            tbOtrosIngresos.Text = Math.Round((oRowS("OtrosIngresos") * ViewState("Porcentaje")) / 100, 7)
            tbCXCPreCierre.Text = Math.Round((oRowS("CXCPreCierre") * ViewState("Porcentaje")) / 100, 7)
            tbCXPPreCierre.Text = Math.Round((oRowS("CXPPreCierre") * ViewState("Porcentaje")) / 100, 7)
            tbCXCVentaTituloCierre.Text = tbCXCVentaTitulo.Text
            'OT10916 - 26/10/2017 - Ian Pastor M. El Monto dividendos del precierre tiene que ser igual al monto del Cierre
            txtMontoDividendosCierre.Text = txtMontoDividendosPrecierre.Text

            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se recupera nuevo campo AjustesCXC | 16/07/18 
            hdAjustesCxCPrecierre.Value = Math.Round((oRowS("AjustesCXC") * ViewState("Porcentaje")) / 100, 7)
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se recupera nuevo campo AjustesCXC | 16/07/18 
            hdDevolucionComisionUnificada.Value = Math.Round((oRowS("DevolucionComisionUnificada") * ViewState("Porcentaje")) / 100, 7)
            hdOtrasCxCPreCierre.Value = Math.Round((oRowS("OtrasCxCPreCierre") * ViewState("Porcentaje")) / 100, 7)

            tbOtrosCXCCierre.ReadOnly = True
            tbOtrosCXCCierre.Text = tbOtrasCXC.Text
            tbCXCExclusivos.ReadOnly = False
            tbOtrosGastosExlusivos.ReadOnly = False
            tbOtrosIngresosExlusivos.ReadOnly = False
            tbValPatriPrecierre.ReadOnly = False
            tbComiSAFM.ReadOnly = True
            tbValPatriPrecierre2.ReadOnly = True
            tbCXPExclusivosCierre.ReadOnly = False
            tbCXPCompraTituloCierre.ReadOnly = True
            tbotrasCXCExclusivos.ReadOnly = False
            tbCXPExclusivos.ReadOnly = False
            tbotrasCXCExclusivos.ReadOnly = False
            tbCajaPrecierre.ReadOnly = True
            tbValPatriPrecierre.ReadOnly = True
            tbCajaCierre.ReadOnly = True
            tbOtrasCXC.ReadOnly = True
            tbOtrasCXP.ReadOnly = True
            tbOtrosGastos.ReadOnly = True
            tbOtrosIngresos.ReadOnly = True
            tbCXCExclusivosCierre.ReadOnly = False
        End If
    End Sub
    Private Sub CargarValoresIngresados()
        Dim oValorCuotaBESerie As New ValorCuotaBE
        oValorCuotaBESerie = ValorCuota.SeleccionarValorCuota(ViewState("Portafolio"), ViewState("NroReg"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim))
        If Not oValorCuotaBESerie.Tables(0).Rows.Count = 0 Then
            Dim oRowSerie As ValorCuotaBE.ValorCuotaSerieRow
            oRowSerie = DirectCast(oValorCuotaBESerie.ValorCuotaSerie.Rows(0), ValorCuotaBE.ValorCuotaSerieRow)
            'tbCXCPreCierre.Text = oRowSerie("CXCPreCierre")
            'tbCXPPreCierre.Text = oRowSerie("CXPPreCierre")
            tbComiSAFM.Text = oRowSerie("ComisionSAFM")
            tbValPatriPrecierre2.Text = oRowSerie("ValPatriPreCierre2")
            tbValPatriPrecierre.Text = oRowSerie("ValPatriPreCierre1")
            tbValValoresPrecierre.Text = oRowSerie("ValCuotaPreCierreVal")
            tbAporteCuota.Text = oRowSerie("AportesCuotas")
            tbAporteValores.Text = oRowSerie("AportesValores")
            tbRescatesCuota.Text = oRowSerie("RescateCuotas")
            tbRescatesValores.Text = oRowSerie("RescateValores")
            tbCXCVentaTituloCierre.Text = oRowSerie("CXCVentaTitulo")
            tbCXCCierre.Text = oRowSerie("CXCCierre")
            tbCXPtituloCierre.Text = oRowSerie("CXPCompraTituloCierre")
            tbCXPotrasCierre.Text = oRowSerie("OtrasCXPCierre")
            tbOtrasCXPCierre.Text = oRowSerie("CXPCierre")
            tbOtrosGastosCierre.Text = oRowSerie("OtrosGastosCierre")
            tbOtrosIngresosCierre.Text = oRowSerie("OtrosIngresosCierre")
            tbOtrosGastosExlusivos.Text = oRowSerie("OtrosGastosExclusivos")
            tbOtrosIngresosExlusivos.Text = oRowSerie("OtrosIngresosExclusivos")
            tbCXCExclusivosCierre.Text = oRowSerie("OtrosCXCExclusivoCierre")
            tbCXPExclusivosCierre.Text = oRowSerie("OtrasCXPExclusivoCierre")
            tbOtrosGastosExlusivosCierre.Text = oRowSerie("OtrosGastosExclusivosCierre")
            tbOtrosIngresosExlusivosCierre.Text = oRowSerie("OtrosIngresosExclusivosCierre")
            tbValCuotaCierreValores.Text = oRowSerie("ValCuotaValoresCierre")
            tbValCuotaCierreValores.Text = oRowSerie("InversionesSubTotalSerie")
            tbValPatCierreCuota.Text = oRowSerie("ValPatriCierreCuota")
            tbValPatCierreValores.Text = oRowSerie("ValPatriCierreValores")
            tbValCuotaCierreCuota.Text = oRowSerie("ValCuotaCierre")
            tbValCuotaCierreValores.Text = oRowSerie("ValCuotaValoresCierre")
            tbCXPCompraTituloCierre.Text = oRowSerie("CXPCompraTituloCierre")
            txtVCDiferencia.Text = Decimal.Parse(tbValCuotaCierreValores.Text) - Decimal.Parse(txtVCA.Text)
            'OT10883 - 08/11/2017 - Ian Pastor M. Nuevos campos agregados
            txtAporteLiberadas.Text = oRowSerie("AportesLiberadas")
            txtRetencionPendiente.Text = oRowSerie("RetencionPendiente")
            'OT10883 - Fin
            '*********************************************************************************************************************
            'OT10927 - 21/11/2017 - Ian Pastor M. Asignar venta de títulos para los portafolios con atributo Valor cuota liberadas
            '*********************************************************************************************************************
            If Request.QueryString("CuotasLiberadas") = "1" Then
                If tbSerie.Text = "SERA" Then
                    txtDividendosDetalle.Text = ViewState("CxCMontosDividendos")
                    txtVentaTitulosDetalle.Text = tbCXCVentaTitulo.Text
                    tbCXCVentaTitulo.Text = Math.Round(Decimal.Parse(tbCXCVentaTitulo.Text) + Decimal.Parse(txtDividendosDetalle.Text), 7)
                Else
                    ''OT10916 - 26/10/2017 - Ian Pastor M. Obtiene el monto total de dividendos que aún no liquidan
                    'txtMontoDividendosCierre.Text = oRowSerie("CXCVentaTituloDividendos")
                    ''OT10916 - Fin
                    txtMontoDividendosCierre.Text = ViewState("CxCMontosDividendos")
                End If
            End If
            'OT10927 - Fin
            '*********************************************************************************************************************
        End If
        'OT11033 - 02/01/2018 - Ian Pastor M. Agregar valor inicial de rescate de valores
        'tbRescatesValores.Text = ObtenerRescateValores()
        'tbAporteValores.Text = ObtenerAporteValores() 'OT11192 - 14/03/2018 - Ian Pastor M. Desc: Obtiene los aportes de valores del Sis. de Operaciones
    End Sub
    Private Function ExisteValorCuota() As Integer
        Dim oValorCuotaBESerie As New ValorCuotaBE
        oValorCuotaBESerie = ValorCuota.SeleccionarValorCuota(ViewState("Portafolio"), ViewState("NroReg"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim))
        Return oValorCuotaBESerie.Tables(0).Rows.Count
    End Function
    Private Sub CargarValoresIngresados2()
        Dim oValorCuotaBESerie As New ValorCuotaBE
        Dim dtValorCuotaSerie As DataTable
        oValorCuotaBESerie = ValorCuota.SeleccionarValorCuota(ViewState("Portafolio"), ViewState("NroReg"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim))
        If Not oValorCuotaBESerie.Tables(0).Rows.Count = 0 Then
            dtValorCuotaSerie = oValorCuotaBESerie.Tables(1)
            '---------
            'tbOtrasCXC.Text = dtValorCuotaSerie.Rows(0)("OtrasCXC")
            tbCXPCompraTitulo.Text = dtValorCuotaSerie.Rows(0)("CXPCompraTitulo")
            tbOtrasCXP.Text = dtValorCuotaSerie.Rows(0)("OtrasCXP")
            hdAjustesCxPPrecierre.Value = dtValorCuotaSerie.Rows(0)("AjustesCXP") 'OT11103 - 01/02/2018 - Ian Pastor M. Obtener los ajustes CxP
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
            'If Decimal.Parse(Replace(IIf(tbAporteValores.Text.Trim = "", "0", tbAporteValores.Text.Trim), ",", "")) = 0 Then
            '    tbAporteValores.Text = ObtenerAporteValores()
            'End If
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
            txtVCDiferencia.Text = Decimal.Parse(tbValCuotaCierreValores.Text) - Decimal.Parse(txtVCA.Text)
            tbOtrosCXCCierre.Text = dtValorCuotaSerie.Rows(0)("OtrosCXCCierre")
            'OT10883 - 08/11/2017 - Ian Pastor M. Nuevos campos agregados
            txtAporteLiberadas.Text = dtValorCuotaSerie.Rows(0)("AportesLiberadas")
            txtRetencionPendiente.Text = dtValorCuotaSerie.Rows(0)("RetencionPendiente")
            'OT10883 - Fin
            '*********************************************************************************************************************
            'OT10927 - 21/11/2017 - Ian Pastor M. Asignar venta de títulos para los portafolios con atributo Valor cuota liberadas
            '*********************************************************************************************************************
            If Request.QueryString("CuotasLiberadas") = "1" Then
                If tbSerie.Text = "SERA" Then
                    txtDividendosDetalle.Text = ViewState("CxCMontosDividendos")
                    txtVentaTitulosDetalle.Text = tbCXCVentaTitulo.Text
                    tbCXCVentaTitulo.Text = Math.Round(Decimal.Parse(tbCXCVentaTitulo.Text) + Decimal.Parse(txtDividendosDetalle.Text), 7)
                Else
                    ''OT10916 - 26/10/2017 - Ian Pastor M. Obtiene el monto total de dividendos que aún no liquidan
                    'txtMontoDividendosCierre.Text = dtValorCuotaSerie.Rows(0)("CXCVentaTituloDividendos")
                    ''OT10916 - Fin
                    txtMontoDividendosCierre.Text = ViewState("CxCMontosDividendos")
                    txtMontoDividendosPrecierre.Text = txtMontoDividendosCierre.Text
                End If
            End If
            'OT10927 - Fin
            '*********************************************************************************************************************
            'OT10965 - 27/11/2017 - Ian Pastor M.
            oValorCuotaBESerie = ValorCuota.SeleccionarValorCuota(ViewState("Portafolio"), "", UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text.Trim))
            dtValorCuotaSerie = oValorCuotaBESerie.Tables(1)
            tbCajaPrecierre.Text = Math.Round((dtValorCuotaSerie.Rows(0)("CajaPreCierre") * ViewState("Porcentaje")) / 100, 7)
            tbOtrasCXC.Text = Math.Round((dtValorCuotaSerie.Rows(0)("OtrasCXC") * ViewState("Porcentaje")) / 100, 7)
            tbCajaCierre.Text = tbCajaPrecierre.Text
            tbCajaPrecierre.ReadOnly = True
            'OT10965 - Fin
            'OT11033 - 02/01/2018 - Ian Pastor M. Agregar valor inicial de rescate de valores
            'tbRescatesValores.Text = ObtenerRescateValores()
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se recupera nuevo campo AjustesCXC | 17/07/18 
            hdAjustesCxCPrecierre.Value = Math.Round((dtValorCuotaSerie.Rows(0)("AjustesCXC") * ViewState("Porcentaje")) / 100, 7)
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se recupera nuevo campo AjustesCXC | 17/07/18 
            hdDevolucionComisionUnificada.Value = Math.Round((dtValorCuotaSerie.Rows(0)("DevolucionComisionUnificada") * ViewState("Porcentaje")) / 100, 7)
            hdOtrasCxCPreCierre.Value = Math.Round((dtValorCuotaSerie.Rows(0)("OtrasCxCPreCierre") * ViewState("Porcentaje")) / 100, 7)
        End If
    End Sub
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
            ValorCuota.Insertar_ValorCuota(oValorCuotaBE, DatosRequest)
            Session("llamadoAgregarSeries") = "SI"
            If CType(ViewState("ActualizacionAC"), Boolean) And (ViewState("DiferenciaComisiones") > 1 Or ViewState("DiferenciaComisiones") < -1) Then
                Dim bResult As Integer = New AumentoCapitalBM().AumentoCapital_ActualizarGastoComision(ViewState("Portafolio"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text), ViewState("DiferenciaComisiones"), DatosRequest)
                If bResult <> 0 Then
                    AlertaJS("Hubo un Error en el procesamiento del Aumento de Capital / Code Error: " + bResult.ToString)
                Else
                    AlertaJS("Registro guardado correctamente.<br>Se actualizó la distribución de intereses por Aumento de Capital.")
                    Session("ActualizacionAC_VC") = True
                End If
                ViewState("ActualizacionAC") = False
            Else
                AlertaJS("Registro guardado correctamente.")
            End If

        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            CargarValoresAutomaticos()
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

            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se setea en variable ajustesCXC | 16/07/18 
            Dim ajustesCXC As String = Replace(IIf(hdAjustesCxCPrecierre.Value.Trim = String.Empty, "0", hdAjustesCxCPrecierre.Value.Trim), ",", "")
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se setea en variable ajustesCXC | 16/07/18 
            Dim DevolucionComisionUnificada As String = Replace(IIf(hdDevolucionComisionUnificada.Value.Trim = String.Empty, "0", hdDevolucionComisionUnificada.Value.Trim), ",", "")
            Dim OtrasCxCPreCierre As String = Replace(IIf(hdOtrasCxCPreCierre.Value.Trim = String.Empty, "0", hdOtrasCxCPreCierre.Value.Trim), ",", "")

ControlAC:
            'Calculo Valor Patrimonio precierre1
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se agrega el ajustesCXC en cálculo de Patrimonio precierre | 16/07/18 
            hdAjustesCxPPrecierre.Value = IIf(hdAjustesCxPPrecierre.Value.Trim = String.Empty, 0, hdAjustesCxPPrecierre.Value)
            tbValPatriPrecierre.Text = Math.Round(Decimal.Parse(InversionesSubtotal) + Decimal.Parse(CajaPrecierre) + _
                                       Decimal.Parse(CXCTitulo) + Decimal.Parse(CXCOtras) + Decimal.Parse(otrasCXCExclusivos) - _
                                       Decimal.Parse(CXPCompraTitulo) - Decimal.Parse(CXPOtras) - Decimal.Parse(CXPExclusivos) - _
                                       Decimal.Parse(OtrosGastos) - Decimal.Parse(OtrosGastosExlusivos) + Decimal.Parse(OtrosIngresos) + _
                                       Decimal.Parse(OtrosIngresosExlusivos) - Decimal.Parse(hdAjustesCxPPrecierre.Value) + _
                                       Decimal.Parse(ajustesCXC), 7)
            '+ Decimal.Parse(DevolucionComisionUnificada), 7)
            ' + Decimal.Parse(OtrasCxCPreCierre), 7)
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se agrega el ajustesCXC en cálculo de Patrimonio precierre | 16/07/18 

            'Calculando la Comisión SAFM
            Dim Porcomi As Decimal = ValorCuota.PorcentajeComisionSerie(ViewState("Portafolio"), tbSerie.Text)
            tbComiSAFM.Text = (Math.Round((Porcomi / 100) / 360 * (1 + ParametrosSIT.IGV) * Decimal.Parse(tbValPatriPrecierre.Text.Trim), 7))
            '- Decimal.Parse(txtDevolucionComisionDiaria.Text)
            'Calculo Valor Patrimonio precierre2
            'tbValPatriPrecierre2.Text = Math.Round(Decimal.Parse(tbValPatriPrecierre.Text.Trim) - Decimal.Parse(tbComiSAFM.Text.Trim) _
            '                                       + Decimal.Parse(MontoDividendosPrecierre), 7) 'OT10981 - 05/12/2017 - Ian Pastor M. Agregar suma de dividendos liberados

            'OT10986 - 13/12/2017 - Ian Pastor M. Agregar suma CxP liberadas al patrimonio pre-cierre 2
            tbValPatriPrecierre2.Text = Math.Round(Decimal.Parse(tbValPatriPrecierre.Text.Trim) - Decimal.Parse(tbComiSAFM.Text.Trim) _
                                                   + Decimal.Parse(MontoDividendosPrecierre) - Decimal.Parse(CxPLiberadaPreCierre), 7)
            'OT10986 - Fin

            'Monto Valor Cuota Precierre
            If Decimal.Parse(ValCuotaPrecierre) <> 0 Then
                tbValValoresPrecierre.Text = Math.Round(Decimal.Parse(tbValPatriPrecierre2.Text.Trim) / Decimal.Parse(ValCuotaPrecierre), 7)
                RescatesValores = ObtenerRescateValores() 'OT11033 - 02/01/2018 - Ian Pastor M.
                tbRescatesValores.Text = RescatesValores

                'INICIO | rcolonia | Zoluxiones | OT11652 | Validar si existe un Aumento de Capital | 09/11/2018
                Dim cantidadreg As Integer = New AumentoCapitalBM().AumentoCapital_ExisteGeneradaOI(ViewState("Portafolio"), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text))
                Dim oPortafolioBE As PortafolioBE
                Dim oPortafolioBM As New PortafolioBM
                Dim oRow As PortafolioBE.PortafolioRow
                Dim DiferenciaComisiones As Decimal = 0D
                oPortafolioBE = oPortafolioBM.Seleccionar(ViewState("Portafolio"), Me.DatosRequest)
                oRow = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
                If cantidadreg > 0 And Not bExisteAC Then
                    DiferenciaComisiones = (Decimal.Parse(ValCuotaPrecierre) * oRow.ValorInicialFondo) - Decimal.Parse(tbValPatriPrecierre2.Text.Trim)
                    DiferenciaComisiones = Math.Round(DiferenciaComisiones * (1 + Math.Round((Porcomi / 100) / 360 * (1 + ParametrosSIT.IGV), 7)), 7) 'Se descuenta la comisión SAFM 
                    ViewState("DiferenciaComisiones") = DiferenciaComisiones
                    CXPOtras -= DiferenciaComisiones
                    tbOtrasCXP.Text = CXPOtras
                    bExisteAC = True
                    ViewState("ActualizacionAC") = True
                    GoTo ControlAC
                End If
                'FIN | rcolonia | Zoluxiones | OT11652 | Validar si existe un Aumento de Capital | 09/11/2018

            End If

            If Decimal.Parse(tbValValoresPrecierre.Text) <> 0 Then
                'OT10916 - 07/11/2017 - Ian Pastor M.
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
                'OT10916 - 07/11/2017 - Ian Pastor M.
                If tbSerie.Text = "SERB" Then
                    If Decimal.Parse(RetencionPendiente) > 0 Then
                        diferenciaAporteLiberada = Decimal.Parse(RetencionPendiente)
                    End If
                End If

                'OT10986 - 13/12/2017 - Ian Pastor M. Agregar suma aporte liberadas
                tbCXPotrasCierre.Text = Math.Round(Decimal.Parse(CXPOtras) + Decimal.Parse(hdAjustesCxPPrecierre.Value) + Decimal.Parse(tbComiSAFM.Text.Trim) - Decimal.Parse(AporteValores) + Decimal.Parse(RescatesValores) _
                                                   + diferenciaAporteLiberada - Decimal.Parse(AporteLiberadas) + Decimal.Parse(CxPLiberadaPreCierre), 7)
                'OT10986 - Fin
                'OT10916 - Fin
            End If
            'Titulo
            tbCXCVentaTituloCierre.Text = Decimal.Parse(CXCTitulo)
            'OT10916 - 06/11/2017 - Ian Pastor M. El monto dividendo del Precierre tiene que ser igual a del Cierre.
            MontoDividendosCierre = MontoDividendosPrecierre
            tbCXPtituloCierre.Text = tbCXPtitulo.Text
            'Totales
            'OT10916 - 06/11/2017 - Ian Pastor M. Se agrega el monto de dividendos al total de CxC Precierre.
            tbCXCPreCierre.Text = Math.Round(Decimal.Parse(CXCTitulo) + Decimal.Parse(tbOtrasCXC.Text) + Decimal.Parse(MontoDividendosPrecierre) + Decimal.Parse(ajustesCXC), 7)
            '+ Decimal.Parse(DevolucionComisionUnificada), 7)
            '    tbCXCPreCierre.Text = Math.Round(Decimal.Parse(CXCTitulo) + Decimal.Parse(tbOtrasCXC.Text) + Decimal.Parse(MontoDividendosPrecierre) + Decimal.Parse(ajustesCXC) + Decimal.Parse(OtrasCxCPreCierre), 7)
            tbCXPPreCierre.Text = Math.Round(Decimal.Parse(tbCXPtitulo.Text) + Decimal.Parse(tbOtrasCXP.Text) + Decimal.Parse(hdAjustesCxPPrecierre.Value), 7)
            tbOtrosCXCCierre.Text = tbOtrasCXC.Text 'OT10864 - 27/10/2017 - Ian Pastor. Desc: Estos dos valores siempre son iguales.
            'OT10916 - 06/11/2017 - Ian Pastor M. Se agrega el monto de dividendos al total de CxC Cierre.
            tbCXCCierre.Text = Math.Round(Decimal.Parse(tbCXCVentaTituloCierre.Text) + Decimal.Parse(tbOtrosCXCCierre.Text) + Decimal.Parse(MontoDividendosCierre) + Decimal.Parse(ajustesCXC), 7)
            '+ Decimal.Parse(DevolucionComisionUnificada), 7)
            'tbCXCCierre.Text = Math.Round(Decimal.Parse(tbCXCVentaTituloCierre.Text) + Decimal.Parse(tbOtrosCXCCierre.Text) + Decimal.Parse(MontoDividendosCierre) + Decimal.Parse(ajustesCXC) + Decimal.Parse(OtrasCxCPreCierre), 7)
            tbCXPotrasCierre.Text = IIf(tbCXPotrasCierre.Text.Trim = String.Empty, 0, tbCXPotrasCierre.Text)
            tbOtrasCXPCierre.Text = Math.Round(Decimal.Parse(tbCXPtituloCierre.Text) + Decimal.Parse(tbCXPotrasCierre.Text), 7)
            btnAceptar.Visible = True
            'Diferencia entre el valor cuota del dia anterior y el calculado Hoy
            tbValCuotaCierreValores.Text = IIf(tbValCuotaCierreValores.Text.Trim = String.Empty, 0, tbValCuotaCierreValores.Text)
            txtVCDiferencia.Text = Decimal.Parse(tbValCuotaCierreValores.Text) - Decimal.Parse(VCAnterior)
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
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
        rowValorCuota.ValSwaps = IIf(tbValoriSwapT.Text = "", 0, tbValoriSwapT.Text)
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
        'OT 9851 27/01/2017 - Carlos Espejo
        'Descripcion: Campos nuevos para el calculo de Otras CXP
        rowDetalleCuota.RescatePendiente = 0
        rowDetalleCuota.ChequePendiente = 0
        'OT 9981 15/02/2017 - Carlos Espejo
        'Descripcion: Se cambia el valor del campo ComisionSAFMAnterior
        rowDetalleCuota.ComisionSAFMAnterior = 0
        rowDetalleCuota.AjustesCXP = IIf(hdAjustesCxPPrecierre.Value = "", 0, hdAjustesCxPPrecierre.Value)
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se guarda nuevo campo AjustesCXC | 26/07/18 
        rowDetalleCuota.AjustesCXC = IIf(hdAjustesCxCPrecierre.Value = String.Empty, 0, hdAjustesCxCPrecierre.Value)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se guarda nuevo campo AjustesCXC | 26/07/18 
        rowDetalleCuota.DevolucionComisionUnificada = IIf(hdDevolucionComisionUnificada.Value = String.Empty, 0, hdDevolucionComisionUnificada.Value)
        rowDetalleCuota.OtrasCxCPreCierre = IIf(hdOtrasCxCPreCierre.Value = String.Empty, "0", hdOtrasCxCPreCierre.Value)
        'OT 9981 Fin
        'OT 9851 Fin
        '*********************************************************************************************************************
        'OT10927 - 21/11/2017 - Ian Pastor M. Asignar venta de títulos para los portafolios con atributo Valor cuota liberadas
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
        'OT10927 - Fin
        '*********************************************************************************************************************
        'OT10883 - 08/11/2017 - Ian Pastor M. Nuevos campos agregados
        rowDetalleCuota.AportesLiberadas = IIf(txtAporteLiberadas.Text = "", 0, txtAporteLiberadas.Text)
        rowDetalleCuota.RetencionPendiente = IIf(txtRetencionPendiente.Text = "", 0, txtRetencionPendiente.Text)
        rowDetalleCuota.DevolucionComisionDiaria = IIf(txtDevolucionComisionDiaria.Text = "", 0, txtDevolucionComisionDiaria.Text)
        'OT10883 - Fin
    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        EjecutarJS("window.close();")
    End Sub
    'OT10916 - 07/11/2017 - Ian Pastor M. Obtener Porcentaje del día anterior
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
    'OT10916 - Fin
    'OT10927 - 21/11/2017 - Ian Pastor M. Asignar venta de títulos para los portafolios con atributo Valor cuota liberadas
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
    'OT10927 - Fin

    Protected Sub imgDetalleVT_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgDetalleVT.Click
        EjecutarJS("MostrarDetalleVentaTitulo();")
    End Sub

    Private Function ObtenerRescateValores() As Decimal
        ObtenerRescateValores = 0
        Dim dtRescateValores As DataTable
        Try
            dtRescateValores = ValorCuota.ValorCuota_ObtenerRescateValores(Request.QueryString("CodigoPortafolioSeriadoSO").ToString(), UIUtility.ConvertirFechaaDecimal(tbFechaInforme.Text), "RES")
        Catch ex As Exception
            ObtenerRescateValores = Decimal.Parse(Replace(IIf(tbRescatesValores.Text.Trim = "", "0", tbRescatesValores.Text.Trim), ",", "")) 'OT10989 - 11/12/2017 - Ian Pastor M. Devuelve el valor que esté en la caja de texto.
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
        If rescateValores > 0 Then 'OT10986 - 15/12/2017 - Ian Pastor M. Si el rescate valor es cero, se mantiene el valor de la caja de texto.
            ObtenerRescateValores = rescateValores
        Else
            ObtenerRescateValores = Decimal.Parse(Replace(IIf(tbRescatesValores.Text.Trim = "", "0", tbRescatesValores.Text.Trim), ",", "")) 'OT10989 - 11/12/2017 - Ian Pastor M. Devuelve el valor que esté en la caja de texto.
        End If
    End Function

    'OT11192 - 12/03/2018 - Ian Pastor M.
    'Descripción: Obtiene el monto de aporte de valores del sistema de operaciones
    Private Function ObtenerAporteValores() As Decimal
        ObtenerAporteValores = 0
        Dim montoAporteValores As Decimal = 0
        Try
            montoAporteValores = ValorCuota.ObtenerAporteValoresSisOpe(Request.QueryString("CodigoPortafolioSeriadoSO").ToString, tbFechaInforme.Text)
        Catch ex As Exception
            ObtenerAporteValores = Decimal.Parse(Replace(IIf(tbAporteValores.Text.Trim = "", "0", tbAporteValores.Text.Trim), ",", ""))
            Exit Function
        End Try
        If montoAporteValores > 0 Then
            ObtenerAporteValores = montoAporteValores
        Else
            ObtenerAporteValores = Decimal.Parse(Replace(IIf(tbAporteValores.Text.Trim = "", "0", tbAporteValores.Text.Trim), ",", ""))
        End If
    End Function
    'OT11192 - Fin
    Private Sub CargarValoresAutomaticos()
        tbRescatesValores.Text = ObtenerRescateValores()
        tbAporteValores.Text = ObtenerAporteValores()
    End Sub
End Class

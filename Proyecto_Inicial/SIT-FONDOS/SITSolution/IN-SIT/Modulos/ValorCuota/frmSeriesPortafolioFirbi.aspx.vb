Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO

Partial Class Modulos_ValorCuota_frmSeriesPortafolioFirbi
    Inherits BasePage
    Dim oUtil As New UtilDM
    Private RutaDestino As String = String.Empty
    Dim objValorCuotaBE As New ValorCuotaBE
    Dim ObjValorCuota As New ValorCuotaBM
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Dim totalCuotas As Decimal = 0
    Dim hdDiferenciaMensaje, hdFlagDiferencia As HiddenField
    Const COD_PROTAFOLIO_FIRBI As String = "222"

    Function TruncateDecimal(value As Decimal, precision As Integer) As Decimal
        Dim stepper As Decimal = Math.Pow(10, precision)
        Dim tmp As Decimal = Math.Truncate(stepper * value)
        Return tmp / stepper
    End Function
    

    Sub CargaPantalla()
        If ddlPortafolio.SelectedValue = 0 Then
            AlertaJS("Debe seleccionar un Portafolio")
        Else
            Dim alerta As String = String.Empty
            Dim oPortafolioBE As PortafolioBE
            Dim oPortafolioBM As New PortafolioBM
            Dim oCarteraTituloValoracionBM As New CarteraTituloValoracionBM
            Dim oRow As PortafolioBE.PortafolioRow
            MostrarOcultarFormulario(True)
            oPortafolioBE = oPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, Me.DatosRequest)
            oRow = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
            tbCXPotras.ReadOnly = True
            txtChequePendiente.ReadOnly = False
            txtRescatePendiente.ReadOnly = False
            hdSeriado.Value = oRow.PorSerie
            hdCuotasLiberadas.Value = oRow.CuotasLiberadas
            hdCodigoPortafolioSisOpe.Value = oRow.CodigoPortafolioSisOpe 'OT10965 - 30/11/2017 - Ian Pastor M. (Obtener codigo de portafolio de operaciones)
            txtComisionPortafolio.Value = oRow.PorcentajeComision 'Obtener Porcentaje de comision del portafolio para realizar las operaciones
            hdCPPadreSisOpe.Value = oRow.CPPadreSisOpe
            HabilitarEscenario(True, oRow.PorSerie)
            CargarValoresCuota()
            btnProcesar.Visible = True
            btnCarga.Visible = True

            cbHabilitarCarga.Visible = True
            'Habilitar carga automática
            If cbHabilitarCarga.Checked Then
                btnCarga.Enabled = False
                iptRuta.Attributes.Add("disabled", "true")
            End If
            MostrarFormularioSoloLectura(False)
            Dim mensajeSeriesCerrados As String = ValidarFondoPorCerrarHijos()

            If mensajeSeriesCerrados <> "" Then
                MostrarFormularioSoloLectura(True)
                AlertaJS(mensajeSeriesCerrados)
                Exit Sub
            End If

            If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) < Decimal.Parse(ObjValorCuota.ObtenerFechaMaxima(ddlPortafolio.SelectedValue)) Then
                MostrarFormularioSoloLectura(True)
                Exit Sub
            Else
                MostrarFormularioSoloLectura(False)
            End If

            If alerta.Length > 0 Then
                AlertaJS(alerta)
            End If
        End If
    End Sub
    Private Function obtenerIndiceColumna_Grilla(ByVal nomCol As String, ByVal grilla As GridView) As Integer
        Dim columna As DataControlFieldCollection = grilla.Columns
        Dim indiceCol As Integer = -1
        For Each celda As DataControlField In columna
            If TypeOf celda Is System.Web.UI.WebControls.BoundField Then
                If CType(celda, BoundField).DataField = nomCol Then
                    indiceCol = columna.IndexOf(celda)
                    Exit For
                End If
            End If
        Next
        Return indiceCol
    End Function
    Private Function ValidarFondoPorCerrarHijos() As String
        Dim oValorCuotaBE As New ValorCuotaBE
        oValorCuotaBE = ObjValorCuota.SeleccionarValorCuota(ddlPortafolio.SelectedValue, "", UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim))
        Dim seriesCerrados As String = ""
        Dim cerroFondoHijos As Boolean = False
        For j = 0 To dgArchivo.Rows.Count - 1
            Dim serie As String = dgArchivo.Rows(j).Cells(obtenerIndiceColumna_Grilla("CodigoSerie", dgArchivo)).Text.Trim()
            oValorCuotaBE = ObjValorCuota.SeleccionarValorCuota(ddlPortafolio.SelectedValue, serie, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim))
            If oValorCuotaBE.Tables(0).Rows.Count > 0 Then
                cerroFondoHijos = True
                seriesCerrados = seriesCerrados + "</br>-" + dgArchivo.Rows(j).Cells(obtenerIndiceColumna_Grilla("Nombre", dgArchivo)).Text.Trim()
            End If
        Next

        If seriesCerrados <> "" Then
            seriesCerrados = "Se bloqueo el formulario porque existen series que han realizado el valor cuota: " + seriesCerrados
        End If
        Return seriesCerrados
    End Function

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not Page.IsPostBack Then
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
                Call CargaPortafolio(ddlPortafolio)
                InicializarBotones()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub CargaPortafolio(ByVal drlista As DropDownList)
        Dim objportafolio As New PortafolioBM
        drlista.DataSource = objportafolio.PortafolioCodigoListar(COD_PROTAFOLIO_FIRBI)
        drlista.DataTextField = "Descripcion"
        drlista.DataValueField = "CodigoPortafolio"
        drlista.DataBind()
        UIUtility.InsertarElementoSeleccion(drlista, 0)
        objportafolio = Nothing
    End Sub
    Private Sub MostrarOcultarFormulario(ByVal Estado As String)
        FormInversiones.Visible = Estado
        FormPrecierre.Visible = Estado
        FormCierre.Visible = Estado
        FormSeries.Visible = Estado
        btnProcesar.Visible = Estado
        btnImprimir.Visible = Estado
    End Sub
    Private Sub HabilitarEscenario(ByVal Estado As Boolean, ByVal TipoFondo As String)
        FormInversiones.Visible = Estado
        FormPrecierre.Visible = Estado
        If TipoFondo.Equals("S") Then
            FormSeries.Visible = Estado
            'FilaPrecierre.Visible = Not Estado
            tbValValoresPrecierre.Visible = Not Estado
            cuotapre.Visible = Not Estado
            FormCierre.Visible = Not Estado
            pncom.Visible = False
        Else
            FormCierre.Visible = Estado
            FormSeries.Visible = Not Estado
            tbValValoresPrecierre.Visible = Estado
            cuotapre.Visible = Estado
            pncom.Visible = True
        End If
    End Sub
    Protected Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            listButtons.Attributes.Add("style", "display:inline")
            CargaPantalla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub HabilitarControles()
        If UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) = Decimal.Parse(HdFechaCreacionFondo.Value) Then
            tbCXCtitulo.ReadOnly = False
            txtCXCPrecierre.ReadOnly = False
            tbCXPtitulo.ReadOnly = False
            txtCXPPrecierre.ReadOnly = False
            tbValPatriPrecierre.ReadOnly = False
            tbComiSAFM.ReadOnly = False
            tbValPatriPrecierre2.ReadOnly = False
            tbValValoresPrecierre.ReadOnly = False
            tbCajaCierre.ReadOnly = False
            tbAporteCuota.ReadOnly = False
            tbRescatesCuota.ReadOnly = False
            tbCXCtituloCierre.ReadOnly = False
            tbCXPtituloCierre.ReadOnly = False
            tbValPatCierreCuota.ReadOnly = False
            tbValPatCierreValores.ReadOnly = False
            tbValCuotaCierreCuota.ReadOnly = False
            tbValCuotaCierreValores.ReadOnly = False
            tbValValoresPrecierre.ReadOnly = False
            tbCXPotrasCierre.ReadOnly = False
            tbValCuotaCierreCuota.ReadOnly = False
            tbValCuotaCierreValores.ReadOnly = False
            btnAceptar.Visible = True
            btnProcesar.Visible = False
        Else
            tbCXCtitulo.ReadOnly = True
            txtCXCPrecierre.ReadOnly = True
            tbCXPtitulo.ReadOnly = True
            txtCXPPrecierre.ReadOnly = True
            tbValPatriPrecierre.ReadOnly = True
            tbComiSAFM.ReadOnly = True
            tbValPatriPrecierre2.ReadOnly = True
            tbValValoresPrecierre.ReadOnly = True
            tbCajaCierre.ReadOnly = True
            tbAporteCuota.ReadOnly = True
            tbRescatesCuota.ReadOnly = True
            tbCXCtituloCierre.ReadOnly = True
            tbCXPtituloCierre.ReadOnly = True
            tbValPatCierreCuota.ReadOnly = True
            tbValPatCierreValores.ReadOnly = True
            tbValCuotaCierreCuota.ReadOnly = True
            tbValCuotaCierreValores.ReadOnly = True
            tbValValoresPrecierre.ReadOnly = True
            tbCXPotrasCierre.ReadOnly = True
            tbValCuotaCierreCuota.ReadOnly = True
            tbValCuotaCierreValores.ReadOnly = True
            btnAceptar.Visible = False
            btnProcesar.Visible = True
        End If
        HabilitarBotonDistribucion()
    End Sub

    Private Sub CargarValoresCuota()
        If Not CargarValoresIngresados() Then
            ObtenerCuentasOnline()
        Else
            CargarGrillaSeries(False)
        End If
    End Sub
    Private Sub ObtenerCuentasOnline()
        Dim DTValorCuota As DataTable
        DTValorCuota = ObjValorCuota.ObtenerDatosFirbiPrecierre(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim))
        tbInversionesT1.Text = 0
        tbComprasT.Text = 0
        tbVentasVenciT.Text = 0
        tbRentabilidadT.Text = 0
        tbValoriForwardT.Text = 0
        tbInversionesSubTotal.Text = DTValorCuota.Rows(0)("InversionesSubTotal")
        tbCXCtitulo.Text = DTValorCuota.Rows(0)("CXCVentaTitulo")
        tbCXPtitulo.Text = DTValorCuota.Rows(0)("CXPCompraTitulo")
        HdFechaCreacionFondo.Value = 0
        tbCajaPrecierre.Text = DTValorCuota.Rows(0)("CajaPrecierre")
        tbValCuotaPrecierre.Text = DTValorCuota.Rows(0)("ValCuotaPreCierre")
        hdCajaPrecierre.Value = 0
        tbAporteValores.Text = 0
        'txtComisionSAFMAnterior.Text = ""
        txtVCA.Text = "0"
        txtigv.Text = ParametrosSIT.IGV
        txtMontoDividendosPrecierre.Text = "0"
        tbValPatriPrecierre.Text = Math.Round(Decimal.Parse(tbInversionesSubTotal.Text) + Decimal.Parse(tbCajaPrecierre.Text) + Decimal.Parse(tbCXCtitulo.Text) - Decimal.Parse(tbCXPtitulo.Text), 7)
        CargarGrillaSeries(True)
        tbValPatriPrecierre2.Text = Decimal.Parse(tbValPatriPrecierre.Text) - Decimal.Parse(tbComiSAFM.Text)
    End Sub
    Private Function CargarValoresIngresados() As Boolean
        CargarValoresIngresados = False
        txtVCDiferencia.Text = 0
        txtCXCPrecierre.Text = 0
        txtCXPPrecierre.Text = 0

        Dim oValorCuotaBE As New ValorCuotaBE
        oValorCuotaBE = ObjValorCuota.SeleccionarValorCuota(ddlPortafolio.SelectedValue, "", UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim))
        If Not oValorCuotaBE.Tables(0).Rows.Count = 0 Then
            Dim oRowS As ValorCuotaBE.ValorCuotaSerieRow
            oRowS = DirectCast(oValorCuotaBE.ValorCuotaSerie.Rows(0), ValorCuotaBE.ValorCuotaSerieRow)
            tbInversionesSubTotal.Text = oRowS("InversionesSubTotal")
            tbCajaPrecierre.Text = oRowS("CajaPrecierre")
            tbCXCtitulo.Text = oRowS("CXCVentaTitulo")
            tbCXPtitulo.Text = oRowS("CXPCompraTitulo")
            tbValCuotaPrecierre.Text = oRowS("ValCuotaPreCierre")
            tbCXCotras.Text = oRowS("OtrasCXC")
            tbCXPotras.Text = oRowS("OtrasCXP")
            tbOtrosGastos.Text = oRowS("OtrosGastos")
            tbOtrosIngresos.Text = oRowS("OtrosIngresos")
            tbValPatriPrecierre.Text = oRowS("ValPatriPreCierre1")
            tbValPatriPrecierre2.Text = oRowS("ValPatriPreCierre2")
            'tbComiSAFM.Text = oRowS("ComisionSAFM")
            tbComiSAFM.Text = Math.Round(Decimal.Parse(oRowS("ValPatriPreCierre1")) - Decimal.Parse(oRowS("ValPatriPreCierre2")), 7)

            tbValValoresPrecierre.Text = oRowS("ValCuotaPreCierreVal")
            tbAporteCuota.Text = oRowS("AportesCuotas")
            tbRescatesCuota.Text = oRowS("RescateCuotas")
            tbRescatesValores.Text = oRowS("RescateValores")
            tbCajaCierre.Text = oRowS("Caja")
            tbCXCtituloCierre.Text = oRowS("CXCVentaTituloCierre")
            tbCXCotrasCierre.Text = oRowS("OtrosCXCCierre")
            tbCXPtituloCierre.Text = oRowS("CXPCompraTituloCierre")
            tbCXPotrasCierre.Text = oRowS("OtrasCXPCierre")
            tbOtrosGastosCierre.Text = oRowS("OtrosGastosCierre")
            tbOtrosIngresosCierre.Text = oRowS("OtrosIngresosCierre")
            tbValPatCierreCuota.Text = oRowS("ValPatriCierreCuota")
            tbValPatCierreValores.Text = oRowS("ValPatriCierreValores")
            tbValCuotaCierreCuota.Text = oRowS("ValCuotaCierre")
            tbValCuotaCierreValores.Text = oRowS("ValCuotaValoresCierre")
            txtCXCPrecierre.Text = oRowS("CXCPreCierre")
            txtCXPPrecierre.Text = oRowS("CXPPreCierre")
            tbCXCCierre.Text = oRowS("CXCCierre")
            tbCXPCierre.Text = oRowS("CXPCierre")
            txtChequePendiente.Text = oRowS("ChequePendiente")
            txtRescatePendiente.Text = oRowS("RescatePendiente")
            'txtRescatePendiente.Text = ObtenerRescatesPendientes()
            If hdSeriado.Value = "S" Then
                txtVCDiferencia.Text = "0"
            Else
                txtVCDiferencia.Text = Decimal.Parse(tbValCuotaCierreValores.Text) - Decimal.Parse(txtVCA.Text)
            End If
            If Not CInt(oRowS("ComisionSAFMAnterior")) = 0 Then
                txtComisionSAFMAnterior.Text = UIUtility.ConvertirFechaaString(CDec(oRowS("ComisionSAFMAnterior")))
            End If
            txtAjustesCXP.Text = oRowS("AjustesCXP")
            If Decimal.Parse(IIf(tbValCuotaPrecierre.Text.Trim = "", 0, tbValCuotaPrecierre.Text.Trim)) = 0 Then
                tbValCuotaPrecierre.Text = oRowS("ValCuotaPreCierre")
            End If
            

            txtComisionSAFM.Text = 0
            txtCajaRecaudo.Text = 0
            txtSuscripcion.Text = 0
            txtRescateP.Text = 0
            txtChequeP.Text = 0
            CargarValoresIngresados = True
        Else
            tbCXCotras.Text = 0
            tbCXPotras.Text = 0
            tbOtrosGastos.Text = 0
            tbOtrosIngresos.Text = 0
            tbValPatriPrecierre.Text = 0
            tbComiSAFM.Text = 0
            tbValPatriPrecierre2.Text = 0
            tbValValoresPrecierre.Text = 0
            tbAporteCuota.Text = 0
            tbRescatesCuota.Text = 0
            'tbRescatesValores.Text = 0
            tbRescatesValores.Text = ObtenerRescateValores()
            tbCajaCierre.Text = 0
            tbCXCtituloCierre.Text = 0
            tbCXCotrasCierre.Text = 0
            tbCXPtituloCierre.Text = 0
            tbCXPotrasCierre.Text = 0
            tbOtrosGastosCierre.Text = 0
            tbOtrosIngresosCierre.Text = 0
            tbValPatCierreCuota.Text = 0
            tbValPatCierreValores.Text = 0
            tbValCuotaCierreCuota.Text = 0
            tbValCuotaCierreValores.Text = 0
            txtCXCPrecierre.Text = 0
            txtCXPPrecierre.Text = 0
            tbCXCCierre.Text = 0
            tbCXPCierre.Text = 0
            txtChequePendiente.Text = 0
            'txtRescatePendiente.Text = 0
            txtRescatePendiente.Text = ObtenerRescatesPendientes()
            txtAjustesCXP.Text = 0
        End If
    End Function
    Private Sub CargarObjetoCabecera(ByVal rowValorCuota As ValorCuotaBE.ValorCuotaRow, Optional ByVal CodigoSerie As String = "")
        rowValorCuota.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        rowValorCuota.CodigoSerie = CodigoSerie
        rowValorCuota.FechaProceso = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        rowValorCuota.InversionesT1 = IIf(tbInversionesT1.Text = "", 0, tbInversionesT1.Text)
        rowValorCuota.VentasVencimientos = IIf(tbVentasVenciT.Text = "", 0, tbVentasVenciT.Text)
        rowValorCuota.Rentabilidad = IIf(tbRentabilidadT.Text = "", 0, tbRentabilidadT.Text)
        rowValorCuota.ValForwards = IIf(tbValoriForwardT.Text = "", 0, tbValoriForwardT.Text)
        rowValorCuota.ValSwaps = 0
        rowValorCuota.InversionesSubTotal = IIf(tbInversionesSubTotal.Text = "", 0, tbInversionesSubTotal.Text)
    End Sub
    Private Sub CargarObjetoDetalle(ByVal rowDetalleCuota As ValorCuotaBE.ValorCuotaSerieRow)
        rowDetalleCuota.CajaPreCierre = IIf(tbCajaPrecierre.Text = "", 0, tbCajaPrecierre.Text)
        rowDetalleCuota.CXCVentaTitulo = IIf(tbCXCtitulo.Text = "", 0, tbCXCtitulo.Text)
        rowDetalleCuota.OtrasCXC = IIf(tbCXCotras.Text = "", 0, tbCXCotras.Text)
        rowDetalleCuota.CXCPreCierre = IIf(txtCXCPrecierre.Text = "", 0, txtCXCPrecierre.Text)
        rowDetalleCuota.CXPCompraTitulo = IIf(tbCXPtitulo.Text = "", 0, tbCXPtitulo.Text)
        rowDetalleCuota.OtrasCXP = IIf(tbCXPotras.Text = "", 0, tbCXPotras.Text)
        rowDetalleCuota.CXPPreCierre = IIf(txtCXPPrecierre.Text = "", 0, txtCXPPrecierre.Text)
        rowDetalleCuota.OtrosGastos = IIf(tbOtrosGastos.Text = "", 0, tbOtrosGastos.Text)
        rowDetalleCuota.OtrosIngresos = IIf(tbOtrosIngresos.Text = "", 0, tbOtrosIngresos.Text)
        rowDetalleCuota.ValPatriPreCierre1 = IIf(tbValPatriPrecierre.Text = "", 0, tbValPatriPrecierre.Text)
        'rowDetalleCuota.ComisionSAFM = IIf(tbComiSAFM.Text = "", 0, tbComiSAFM.Text)
        rowDetalleCuota.ComisionSAFM = 0
        rowDetalleCuota.ValPatriPreCierre2 = IIf(tbValPatriPrecierre2.Text = "", 0, tbValPatriPrecierre2.Text)
        rowDetalleCuota.ValCuotaPreCierre = IIf(tbValCuotaPrecierre.Text = "", 0, tbValCuotaPrecierre.Text)
        rowDetalleCuota.ValCuotaPreCierreVal = IIf(tbValValoresPrecierre.Text = "", 0, tbValValoresPrecierre.Text)
        rowDetalleCuota.AportesCuotas = IIf(tbAporteCuota.Text = "", 0, tbAporteCuota.Text)
        rowDetalleCuota.AportesValores = IIf(tbAporteValores.Text = "", 0, tbAporteValores.Text)
        rowDetalleCuota.RescateCuotas = IIf(tbRescatesCuota.Text = "", 0, tbRescatesCuota.Text)
        rowDetalleCuota.RescateValores = IIf(tbRescatesValores.Text = "", 0, tbRescatesValores.Text)
        rowDetalleCuota.Caja = IIf(tbCajaCierre.Text = "", 0, tbCajaCierre.Text)
        rowDetalleCuota.CXCVentaTituloCierre = IIf(tbCXCtituloCierre.Text = "", 0, tbCXCtituloCierre.Text)
        rowDetalleCuota.OtrosCXCCierre = IIf(tbCXCotrasCierre.Text = "", 0, tbCXCotrasCierre.Text)
        rowDetalleCuota.CXCCierre = IIf(tbCXCCierre.Text = "", 0, tbCXCCierre.Text)
        rowDetalleCuota.CXPCompraTituloCierre = IIf(tbCXPtituloCierre.Text = "", 0, tbCXPtituloCierre.Text)
        rowDetalleCuota.OtrasCXPCierre = IIf(tbCXPotrasCierre.Text = "", 0, tbCXPotrasCierre.Text)
        rowDetalleCuota.CXPCierre = IIf(tbCXPCierre.Text = "", 0, tbCXPCierre.Text)
        rowDetalleCuota.OtrosGastosCierre = IIf(tbOtrosGastosCierre.Text = "", 0, tbOtrosGastosCierre.Text)
        rowDetalleCuota.OtrosIngresosCierre = IIf(tbOtrosIngresosCierre.Text = "", 0, tbOtrosIngresosCierre.Text)
        rowDetalleCuota.ValPatriCierreCuota = IIf(tbValPatCierreCuota.Text = "", 0, tbValPatCierreCuota.Text)
        rowDetalleCuota.ValPatriCierreValores = IIf(tbValPatCierreValores.Text = "", 0, tbValPatCierreValores.Text)
        rowDetalleCuota.ValCuotaCierre = IIf(tbValCuotaCierreCuota.Text = "", 0, tbValCuotaCierreCuota.Text)
        rowDetalleCuota.ValCuotaValoresCierre = IIf(tbValCuotaCierreValores.Text = "", 0, tbValCuotaCierreValores.Text)
        rowDetalleCuota.OtrasCXCExclusivos = 0
        rowDetalleCuota.OtrasCXPExclusivos = 0
        rowDetalleCuota.OtrosGastosExclusivos = 0
        rowDetalleCuota.OtrosIngresosExclusivos = 0
        rowDetalleCuota.OtrosCXCExclusivoCierre = 0
        rowDetalleCuota.OtrasCXPExclusivoCierre = 0
        rowDetalleCuota.OtrosGastosExclusivosCierre = 0
        rowDetalleCuota.OtrosIngresosExclusivosCierre = 0
        rowDetalleCuota.InversionesSubTotalSerie = 0
        rowDetalleCuota.ValCuotaPreCierreAnt = 0
        rowDetalleCuota.RescatePendiente = IIf(txtRescatePendiente.Text = "", 0, txtRescatePendiente.Text)
        rowDetalleCuota.ChequePendiente = IIf(txtChequePendiente.Text = "", 0, txtChequePendiente.Text)
        rowDetalleCuota.ComisionSAFMAnterior = UIUtility.ConvertirFechaaDecimal(txtComisionSAFMAnterior.Text)
        rowDetalleCuota.AjustesCXP = IIf(txtAjustesCXP.Text = "", 0, txtAjustesCXP.Text)
        rowDetalleCuota.CXCVentaTituloDividendos = IIf(txtMontoDividendosPrecierre.Text = "", 0, txtMontoDividendosPrecierre.Text)
        rowDetalleCuota.AportesLiberadas = 0
        rowDetalleCuota.RetencionPendiente = 0
        rowDetalleCuota.AjustesCXC = 0
        rowDetalleCuota.DevolucionComisionUnificada = 0
        rowDetalleCuota.OtrasCxCPreCierre = 0
    End Sub
    Private Function ValidarCuotaPreCierre() As Boolean
        'En el ambiente de desarrollo no se consulta el precierre
        ValidarCuotaPreCierre = False
        Dim ExistePrecierre As Integer
        Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
        Dim oPortafolioBE As PortafolioBE
        Dim oPortafolioBM As New PortafolioBM
        Dim oRow As PortafolioBE.PortafolioRow
        oPortafolioBE = oPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, Me.DatosRequest)
        oRow = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
        ExistePrecierre = oCarteraTituloValoracion.Valida_PRECIERRE_CONSOLIDADO_OPERACIONES(CDate(tbFechaOperacion.Text), CDate(tbFechaOperacion.Text), _
            IIf(oRow.CodigoPortafolioSisOpe = "", 0, oRow.CodigoPortafolioSisOpe))
        If ExistePrecierre > 0 Then
            Dim dt As DataTable = oCarteraTituloValoracion.ObtenerValorCuotaPreCierreOperaciones(tbFechaOperacion.Text, IIf(oRow.CodigoPortafolioSisOpe = "", 0, oRow.CodigoPortafolioSisOpe))
            If dt.Rows.Count > 0 Then
                If CType(tbValValoresPrecierre.Text, Decimal) <> CType(dt.Rows(0)("VALOR_CUOTA"), Decimal) Then
                    ValidarCuotaPreCierre = True
                End If
            End If
        End If
    End Function
    Private Sub HabilitarControlesCalculoValorCuota(ByVal TipoCalculoValorCuota As String)
        Select Case TipoCalculoValorCuota
            Case "01"
                tbInversionesSubTotal.ReadOnly = False
                tbCajaPrecierre.ReadOnly = False
            Case "02"
                tbInversionesSubTotal.ReadOnly = True
                tbCajaPrecierre.ReadOnly = True
        End Select
    End Sub
    Private Sub MostrarFormularioSoloLectura(ByVal Lectura As Boolean)
        If Lectura Then
            txtVCA.ReadOnly = True
            txtVCDiferencia.ReadOnly = True
            txtComisionSAFMAnterior.ReadOnly = True
            tbInversionesT1.ReadOnly = True
            tbComprasT.ReadOnly = True
            tbVentasVenciT.ReadOnly = True
            tbRentabilidadT.ReadOnly = True
            tbValoriForwardT.ReadOnly = True
            tbInversionesSubTotal.ReadOnly = True
            txtigv.ReadOnly = True
            txtporcom.ReadOnly = True
            tbCajaPrecierre.ReadOnly = True
            tbCXCtitulo.ReadOnly = True
            tbCXCotras.ReadOnly = True
            txtCXCPrecierre.ReadOnly = True
            txtChequePendiente.ReadOnly = True
            txtRescatePendiente.ReadOnly = True
            txtAjustesCXP.ReadOnly = True
            tbCXPtitulo.ReadOnly = True
            tbCXPotras.ReadOnly = True
            txtCXPPrecierre.ReadOnly = True
            tbOtrosGastos.ReadOnly = True
            tbOtrosIngresos.ReadOnly = True
            tbValPatriPrecierre.ReadOnly = True
            tbComiSAFM.ReadOnly = True
            tbValPatriPrecierre2.ReadOnly = True
            tbValCuotaPrecierre.ReadOnly = True
            tbValValoresPrecierre.ReadOnly = True
            tbCajaCierre.ReadOnly = True
            tbAporteCuota.ReadOnly = True
            tbAporteValores.ReadOnly = True
            tbRescatesCuota.ReadOnly = True
            tbRescatesValores.ReadOnly = True
            tbCXCtituloCierre.ReadOnly = True
            tbCXCotrasCierre.ReadOnly = True
            tbCXCCierre.ReadOnly = True
            tbCXPtituloCierre.ReadOnly = True
            tbCXPotrasCierre.ReadOnly = True
            tbCXPCierre.ReadOnly = True
            tbOtrosGastosCierre.ReadOnly = True
            tbOtrosIngresosCierre.ReadOnly = True
            tbValPatCierreCuota.ReadOnly = True
            tbValPatCierreValores.ReadOnly = True
            tbValCuotaCierreCuota.ReadOnly = True
            tbValCuotaCierreValores.ReadOnly = True
            txtComisionSAFM.ReadOnly = True
            txtCajaRecaudo.ReadOnly = True
            txtSuscripcion.ReadOnly = True
            txtChequeP.ReadOnly = True
            txtRescateP.ReadOnly = True
            btnCarga.Visible = Not True
            btnAceptar.Visible = Not True
            btnProcesar.Visible = Not True
            ' btnInicializar.Visible = Not True
        Else
            txtComisionSAFMAnterior.ReadOnly = False
            tbCXCotras.ReadOnly = False
            txtChequePendiente.ReadOnly = False
            txtRescatePendiente.ReadOnly = False
            txtAjustesCXP.ReadOnly = False
            tbOtrosGastos.ReadOnly = False
            tbOtrosIngresos.ReadOnly = False
            tbValCuotaPrecierre.ReadOnly = False
            tbRescatesValores.ReadOnly = False
            tbOtrosGastosCierre.ReadOnly = False
            tbOtrosIngresosCierre.ReadOnly = False
            tbCajaPrecierre.ReadOnly = False
            tbCXCtitulo.ReadOnly = False
            tbCXPtitulo.ReadOnly = False
        End If
    End Sub
    Private Sub HabilitarBotonDistribucion()
        Dim objPortafolioBM As New PortafolioBM
        Dim objPortafolioBE As New PortafolioBE
        objPortafolioBE = objPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, DatosRequest)
        If objPortafolioBE.Tables(0).Rows(0)("CuotasLiberadas") = "1" Then
            btnVerDistribucion.Visible = True
            ConsultarDistribucionLib()
        Else
            btnVerDistribucion.Visible = False
        End If
        ViewState("CuotasLiberadas") = objPortafolioBE.Tables(0).Rows(0)("CuotasLiberadas")
    End Sub
    Protected Sub dgArchivo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgArchivo.RowCommand
        Dim Row As GridViewRow
        Dim i As Int32 = 0
        If e.CommandName.Equals("Modificar") Then
            Row = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            i = Row.RowIndex
            Dim cadena As String = e.CommandArgument.ToString()
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            Dim rowValorCuota As ValorCuotaBE.ValorCuotaRow
            Dim dblPorcentaje As Decimal
            Dim DtValoresSerie As DataTable
            Dim oPortafolio As New PortafolioBM
            Dim CodigoPortafolioSeriadoSO As String = String.Empty

            Dim oValorCuotaBESerie As New ValorCuotaBE
            Dim dtValorCuotaSerie As DataTable
            oValorCuotaBESerie = ObjValorCuota.SeleccionarValorCuota(ddlPortafolio.SelectedValue, "", UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim))
            dtValorCuotaSerie = oValorCuotaBESerie.Tables(0)
            '-----------
            If dtValorCuotaSerie.Rows.Count = 0 Then
                AlertaJS("Debe realizar primero el valor cuota en al fondo padre.")
                Exit Sub
            End If
            '-----------



            dblPorcentaje = Decimal.Parse(CType(gvr.FindControl("tbPorcentaje"), TextBox).Text)
            rowValorCuota = CType(objValorCuotaBE.ValorCuota.NewRow(), ValorCuotaBE.ValorCuotaRow)
            CargarObjetoCabecera(rowValorCuota, cadena(0))
            Session("DatosCabecera") = rowValorCuota
            DtValoresSerie = oPortafolio.PortafolioCodigoListar_ValoresSerie(ddlPortafolio.SelectedValue)
            If DtValoresSerie.Rows.Count > 0 Then
                CodigoPortafolioSeriadoSO = DtValoresSerie.Select("CodigoSerie='" & cadena & "'")(0)("CodigoPortafolioSO")
            End If


            Dim cuotaCierre As String = dgArchivo.Rows(i).Cells(obtenerIndiceColumna_Grilla("CuotasCierre", dgArchivo)).Text
            Dim valorCierre As String = dgArchivo.Rows(i).Cells(obtenerIndiceColumna_Grilla("ValoresCierre", dgArchivo)).Text

            Dim strURL As String = "frmAgregarSeriesFirbi.aspx?NroReg=" & cadena & "&Serie=" & cadena & "&Portafolio=" & ddlPortafolio.SelectedValue & _
                "&Fecha=" & tbFechaOperacion.Text & "&Porcentaje=" & CType(gvr.FindControl("tbPorcentaje"), TextBox).Text & _
                "&DetPorta=" & ddlPortafolio.SelectedItem.Text & "&CuotasLiberadas=" & ViewState("CuotasLiberadas") & "&CodigoPortafolioSeriadoSO=" & CodigoPortafolioSeriadoSO & _
                "&ValorCierre=" + valorCierre + "&CuotaCierre=" + cuotaCierre
            EjecutarJS("showModalDialog('" & strURL & "', '1200', '600', '');") 'OT10795 - 20/10/2017 - Ian Pastor M. Se muestra la ventana sin importar el valor del porcentaje
        End If
    End Sub
    Protected Sub btnCarga_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCarga.Click
        Try
            Dim ValidaPortafolio As Boolean = True
            Dim ValidaSeries As Boolean = False
            Dim ValidaFecha As Boolean = True
            If iptRuta.Value.Equals("") Then
                AlertaJS("Debe seleccionar un archivo.")
                Exit Sub
            End If
            Dim oArchivoPlanoBE As DataSet
            Dim oArchivoPlanoBM As New ArchivoPlanoBM
            oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("005", MyBase.DatosRequest())
            RutaDestino = oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
            If (Not Directory.Exists(RutaDestino)) Then
                AlertaJS("No existe la ruta destino.")
                Exit Sub
            End If
            Dim fInfo As New FileInfo(iptRuta.Value)
            iptRuta.PostedFile.SaveAs(RutaDestino & "\" & fInfo.Name)
            Dim strFile As String = File.ReadAllText(RutaDestino & "\" & fInfo.Name)
            strFile = Regex.Replace(strFile, "\n$", "") 'Elimina la ultima linea si es vacia
            strFile = Regex.Replace(strFile, "\n\r", "")  'Elimina las lineas vacias que no son ni la primera ni la ultima
            File.WriteAllText(RutaDestino & "\" & fInfo.Name, strFile)
            Dim objReader As New StreamReader(RutaDestino & "\" & fInfo.Name)
            Dim sLine As String = ""
            Dim arrText As New ArrayList()
            Do
                sLine = objReader.ReadLine()
                If Not sLine Is Nothing Then
                    arrText.Add(sLine)
                End If
            Loop Until sLine Is Nothing
            objReader.Close()
            'Validamos que el portafolio seleccionado sea el correcto.
            For Each sLine In arrText
                Dim Fila() As String = sLine.Split(";")

                If Not Fila(0).Equals(ddlPortafolio.SelectedItem.Text) Then
                    ValidaPortafolio = False
                    Exit For
                End If
            Next
            If Not ValidaPortafolio Then
                AlertaJS("El archivo a cargar no pertenece al Portafolio seleccionado.")
                Exit Sub
            End If
            'Validamos que la fecha del archivo se lgual a la fecha de Proceso
            For Each sLine In arrText
                Dim Fila() As String = sLine.Split(";")

                If Not Fila(1).Equals(tbFechaOperacion.Text) Then
                    ValidaFecha = False
                    Exit For
                End If
            Next
            If Not ValidaFecha Then
                AlertaJS("El archivo no corresponde a la Fecha de Proceso.")
                Exit Sub
            End If
            'validamos que la configuración de series este bien parametrizado
            For Each sLine In arrText
                Dim Fila() As String = sLine.Split(";")
                ValidaSeries = False
                For Each row As GridViewRow In dgArchivo.Rows
                    If row.Cells(1).Text.ToUpper.Equals(Fila(2).ToUpper) Then
                        ValidaSeries = True
                    End If
                Next
                If ValidaSeries = False Then
                    Exit For
                End If
            Next
            If Not ValidaSeries Then
                AlertaJS("El archivo a cargar no concuerda con la parametria de Series para este portafolio.")
                Exit Sub
            End If
            'Eliminamos los porcentajes antiguos antes de insertar los nuevos.
            Dim objportafolio As New PortafolioBM
            objportafolio.Eliminar_PorcentajeSeries(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
            'Insertamos los porcentajes de las series
            For Each sLine In arrText
                Dim Fila() As String = sLine.Split(";")
                objportafolio = New PortafolioBM
                'objportafolio.Insertar_PorcentajeSeries(Fila(0), Fila(2), UIUtility.ConvertirFechaaDecimal(Fila(1)), Fila(3), Me.DatosRequest)
                objportafolio.Insertar_PorcentajeSeries(ddlPortafolio.SelectedValue, Fila(2), UIUtility.ConvertirFechaaDecimal(Fila(1)), Fila(3), Me.DatosRequest)
            Next
            dgArchivo.DataSource = Nothing
            dgArchivo.DataBind()
            Dim oPortafolioBM As New PortafolioBM
            dgArchivo.DataSource = oPortafolioBM.Portafolio_Series_Cuotas(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
            dgArchivo.DataBind()
            For Each row As GridViewRow In dgArchivo.Rows
                CType(row.FindControl("btnModificar"), ImageButton).Enabled = True
            Next
            AlertaJS("La carga se realizó con éxito.")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        Finally
            btnCarga.Text = "Cargar Archivo"
            btnCarga.Enabled = True
        End Try
    End Sub
    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        ObtenerMontosContablesFirbi() 'Obtener nuevamente los montos contables del Portafolio FIRBI
        'If cbHabilitarCarga.Checked Then
        '    CargarPorcentajeSeries()
        'End If
        'Descripcion: Validacion para la fecha anterior
        'If txtComisionSAFMAnterior.Text = "" Then
        '    AlertaJS("Se debe ingresar una fecha la comision SAFM anterior", "('#txtComisionSAFMAnterior').focus();")
        '    Exit Sub
        'End If
        Dim objPortafolioPCBM As New PortafolioPorcentajeComisionBM
        Dim listPortafolioPCBE As List(Of PortafolioPorcentajeComisionBE)
        Dim valPatriPreCierre, saldoPatriPreCierre, comisionActual, comisionAcumulada, margenValorCuota, suscripcionIni As Decimal
        Dim cantComi, i As Int32
        Dim existeSaldo = True
        Dim oPortafolioBM As New PortafolioBM
        Dim oRowPortafolioBE As PortafolioBE.PortafolioRow
        Dim oPortafolioBE As PortafolioBE
        Try
            oPortafolioBE = oPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, Me.DatosRequest)
            oRowPortafolioBE = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
            listPortafolioPCBE = objPortafolioPCBM.Listar(ddlPortafolio.SelectedValue)
            'Calculo Valor Patrimonio precierre1
            Dim InversionesSubtotal As String = Replace(IIf(tbInversionesSubTotal.Text.Trim = "", "0", tbInversionesSubTotal.Text.Trim), ",", "")
            Dim CajaPrecierre As String = Replace(IIf(tbCajaPrecierre.Text.Trim = "", "0", tbCajaPrecierre.Text.Trim), ",", "")
            Dim CXCTitulo As String = Replace(IIf(tbCXCtitulo.Text.Trim = "", "0", tbCXCtitulo.Text.Trim), ",", "")
            Dim CXCOtras As String = Replace(IIf(tbCXCotras.Text.Trim = "", "0", tbCXCotras.Text.Trim), ",", "")
            Dim CXPTitulo As String = Replace(IIf(tbCXPtitulo.Text.Trim = "", "0", tbCXPtitulo.Text.Trim), ",", "")
            Dim OtrosGastos As String = Replace(IIf(tbOtrosGastos.Text.Trim = "", "0", tbOtrosGastos.Text.Trim), ",", "")
            Dim OtrosIngresos As String = Replace(IIf(tbOtrosIngresos.Text.Trim = "", "0", tbOtrosIngresos.Text.Trim), ",", "")
            Dim AporteValores As String = Replace(IIf(tbAporteValores.Text.Trim = "", "0", tbAporteValores.Text.Trim), ",", "")
            Dim ValCuotaPrecierre As String = Replace(IIf(tbValCuotaPrecierre.Text.Trim = "", "0", tbValCuotaPrecierre.Text.Trim), ",", "")
            Dim RescatesValores As String = Replace(IIf(tbRescatesValores.Text.Trim = "", "0", tbRescatesValores.Text.Trim), ",", "")
            Dim OtrosGastosCierre As String = Replace(IIf(tbOtrosGastosCierre.Text.Trim = "", "0", tbOtrosGastosCierre.Text.Trim), ",", "")
            Dim OtrosIngresosCierre As String = Replace(IIf(tbOtrosIngresosCierre.Text.Trim = "", "0", tbOtrosIngresosCierre.Text.Trim), ",", "")
            Dim ChequePendiente As String = Replace(IIf(txtChequePendiente.Text.Trim = "", "0", txtChequePendiente.Text.Trim), ",", "")
            Dim RescatePendiente As String = Replace(IIf(txtRescatePendiente.Text.Trim = "", "0", txtRescatePendiente.Text.Trim), ",", "")
            Dim VCAnterior As String = Replace(IIf(txtVCA.Text.Trim = "", "0", txtVCA.Text.Trim), ",", "")
            Dim AjustesCXP As String = Replace(IIf(txtAjustesCXP.Text = "", 0, txtAjustesCXP.Text), ",", "")
            Dim CXPOtras As Decimal = 0
            'Descripcion: Si es seriado el fondo se calcula Otras CXP.
            'La caja se ve afectada por los cheques pendietes
            If oRowPortafolioBE.TipoCalculoValorCuota = "02" Then
                CajaPrecierre = Decimal.Parse(hdCajaPrecierre.Value) + Decimal.Parse(ChequePendiente)
            Else
                CajaPrecierre = Decimal.Parse(tbCajaPrecierre.Text) + Decimal.Parse(ChequePendiente)
            End If

            tbCajaPrecierre.Text = CajaPrecierre
            'Descripcion: Se cambia el uso de ComisionSAFMAnterior
            '*********************************************************************************************************************************
            'Portafolio FIR no hace el calculo
            '*********************************************************************************************************************************
            'If oRowPortafolioBE.TipoCalculoValorCuota = "02" Then
            '    Dim dtOtrasCXP = ObjValorCuota.OtrasCXP(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
            '    Decimal.Parse(ChequePendiente), Decimal.Parse(RescatePendiente), UIUtility.ConvertirFechaaDecimal(txtComisionSAFMAnterior.Text), 0D)
            '    CXPOtras = dtOtrasCXP.Rows(0)("OtrasCXP")
            '    txtComisionSAFM.Text = dtOtrasCXP.Rows(0)("ComisionSAFM")
            '    txtCajaRecaudo.Text = dtOtrasCXP.Rows(0)("CajaRecaudo")
            '    txtSuscripcion.Text = dtOtrasCXP.Rows(0)("Suscripcion")
            '    txtRescateP.Text = dtOtrasCXP.Rows(0)("RescatePendiente")
            '    txtChequeP.Text = dtOtrasCXP.Rows(0)("ChequePendiente")
            'Else
            '    CXPOtras = Decimal.Parse(tbCXPotras.Text)
            'End If

            CXPOtras = 0
            txtComisionSAFM.Text = 0
            txtCajaRecaudo.Text = 0
            txtSuscripcion.Text = 0
            txtRescateP.Text = 0
            txtChequeP.Text = 0

            'Calculo
            tbValPatriPrecierre.Text =
            Decimal.Parse(InversionesSubtotal) + Decimal.Parse(CajaPrecierre) + Decimal.Parse(CXCTitulo) + Decimal.Parse(CXCOtras) - Decimal.Parse(CXPTitulo) -
            Decimal.Parse(CXPOtras) - Decimal.Parse(AjustesCXP) - Decimal.Parse(OtrosGastos) + Decimal.Parse(OtrosIngresos)
            'Comision Variable
            margenValorCuota = oRowPortafolioBE.TopeValorCuota
            suscripcionIni = oRowPortafolioBE.MontoSuscripcionInicial
            'Calculo Nuevo Comision SAFM
            valPatriPreCierre = Decimal.Parse(tbValPatriPrecierre.Text.Trim)
            If CType(oRowPortafolioBE.FlagComisionVariable, Boolean) And (valPatriPreCierre > margenValorCuota Or CType(oRowPortafolioBE.FlagComisionSuscripInicial, Boolean)) Then
                valPatriPreCierre = suscripcionIni
            End If
            saldoPatriPreCierre = valPatriPreCierre
            existeSaldo = If(saldoPatriPreCierre > 0, True, False)
            cantComi = listPortafolioPCBE.Count
            i = 0
            Do While cantComi > 0 And existeSaldo And i < cantComi
                Dim obj As PortafolioPorcentajeComisionBE = listPortafolioPCBE(i)
                If saldoPatriPreCierre > obj.ValorMargenMaximo Then
                    comisionActual = ((obj.ValorPorcentajeComision / 100) / 360) * (1 + ParametrosSIT.IGV) * obj.ValorMargenMaximo
                    saldoPatriPreCierre = valPatriPreCierre - obj.ValorMargenMaximo
                Else
                    comisionActual = ((obj.ValorPorcentajeComision / 100) / 360) * (1 + ParametrosSIT.IGV) * saldoPatriPreCierre
                    existeSaldo = False
                End If

                comisionAcumulada += comisionActual
                i += 1
            Loop
            'tbComiSAFM.Text = TruncateDecimal(comisionAcumulada, 7)
            'tbValPatriPrecierre2.Text = TruncateDecimal((Decimal.Parse(tbValPatriPrecierre.Text.Trim) - comisionAcumulada), 7)
            margenValorCuota = oRowPortafolioBE.TopeValorCuota
            suscripcionIni = oRowPortafolioBE.MontoSuscripcionInicial
            'Monto Valor Cuota Precierre
            If Decimal.Parse(ValCuotaPrecierre) <> 0 Then
                tbValValoresPrecierre.Text = TruncateDecimal(Decimal.Parse(tbValPatriPrecierre2.Text.Trim) / Decimal.Parse(ValCuotaPrecierre), 7)
                RescatesValores = ObtenerRescateValores()
                tbRescatesValores.Text = RescatesValores
            End If
            If CDec(Replace(IIf(tbValValoresPrecierre.Text.Trim = "", "0", tbValValoresPrecierre.Text.Trim), ",", "")) <> 0 Then
                'Calcular Aportes Cuotas
                tbAporteCuota.Text = TruncateDecimal(Decimal.Parse(AporteValores) / Decimal.Parse(tbValValoresPrecierre.Text.Trim), 7)
                'Calcular Rescate Cuos
                tbRescatesCuota.Text = TruncateDecimal(Decimal.Parse(RescatesValores) / Decimal.Parse(tbValValoresPrecierre.Text.Trim), 7)
            Else
                tbAporteCuota.Text = 0
                tbRescatesCuota.Text = 0
            End If
            'Asignando Caja Cierre de "Caja Precierre"
            tbCajaCierre.Text = TruncateDecimal(Decimal.Parse(CajaPrecierre), 7)
            'CALCULAR EL VALOR PATRIMONIO CIERRE
            tbValPatCierreCuota.Text = TruncateDecimal(Decimal.Parse(ValCuotaPrecierre) + Decimal.Parse(tbAporteCuota.Text.Trim) -
            Decimal.Parse(tbRescatesCuota.Text.Trim), 7)
            tbValPatCierreValores.Text = TruncateDecimal(Decimal.Parse(tbValPatriPrecierre2.Text.Trim) + Decimal.Parse(AporteValores) - Decimal.Parse(RescatesValores) _
            + Decimal.Parse(OtrosIngresosCierre) - Decimal.Parse(OtrosGastosCierre), 7)
            'EL VALOR CUOTA CIERRE
            tbValCuotaCierreCuota.Text = TruncateDecimal(Decimal.Parse(tbValPatCierreCuota.Text.Trim), 7)
            If CDec(Replace(IIf(tbValCuotaCierreCuota.Text.Trim = "", "0", tbValCuotaCierreCuota.Text.Trim), ",", "")) <> 0 Then
                tbValCuotaCierreValores.Text = TruncateDecimal(Decimal.Parse(tbValPatCierreValores.Text.Trim) / Decimal.Parse(tbValCuotaCierreCuota.Text.Trim), 7)
            Else
                tbValCuotaCierreValores.Text = 0
            End If
            'Páginas modificadas: ParametrosSIT.vb y tambien capas
            tbCXPotrasCierre.Text = TruncateDecimal(Decimal.Parse(CXPOtras) + Decimal.Parse(AjustesCXP) + Decimal.Parse(tbComiSAFM.Text) + Decimal.Parse(RescatesValores) - Decimal.Parse(AporteValores), 7)
            'Try
            '    txtCXCPrecierre.Text = Math.Round(Decimal.Parse(CXCTitulo) + Decimal.Parse(CXCOtras) + Decimal.Parse(txtMontoDividendosPrecierre.Text), 7)
            'Catch ex As Exception
            '    txtCXCPrecierre.Text = "0"
            'End Try
            'Try
            '    txtCXPPrecierre.Text = Math.Round(Decimal.Parse(CXPTitulo) + Decimal.Parse(CXPOtras) + Decimal.Parse(AjustesCXP), 7).ToString
            'Catch ex As Exception
            '    txtCXPPrecierre.Text = "0"
            'End Try
            'Cierre Titulo
            tbCXCtituloCierre.Text = tbCXCtitulo.Text
            tbCXPtituloCierre.Text = tbCXPtitulo.Text
            tbCXCotrasCierre.Text = CXCOtras
            'Cierre Totales CXC y CXP
            Try
                tbCXCCierre.Text = Math.Round(Decimal.Parse(tbCXCtituloCierre.Text) + Decimal.Parse(tbCXCotrasCierre.Text), 7).ToString
            Catch ex As Exception
                tbCXCCierre.Text = "0"
            End Try
            Try
                tbCXPCierre.Text = Math.Round(Decimal.Parse(tbCXPtituloCierre.Text) + Decimal.Parse(tbCXPotrasCierre.Text), 7).ToString
            Catch ex As Exception
                tbCXPCierre.Text = "0"
            End Try
            btnAceptar.Visible = True
            'Ajustes
            tbValCuotaCierreValores.Text = Decimal.Parse(tbValCuotaCierreValores.Text)
            tbCXPotras.Text = CXPOtras
            txtComisionSAFM.Text = 0

            tbValPatCierreCuota.Text = tbValCuotaPrecierre.Text
            tbValCuotaCierreCuota.Text = tbValCuotaPrecierre.Text
            tbCXPotrasCierre.Text = 0
            'Diferencia entre el valor cuota del dia anterior y el calculado Hoy
            'txtVCDiferencia.Text = Decimal.Parse(tbValCuotaCierreValores.Text) - Decimal.Parse(VCAnterior)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al procesar los datos: " & Replace(ex.Message, "'", ""))
        Finally
            btnProcesar.Text = "Procesar"
            btnProcesar.Enabled = True
        End Try
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oValorCuotaBE As New ValorCuotaBE
            Dim rowValorCuota As ValorCuotaBE.ValorCuotaRow
            Dim rowDetalleCuota As ValorCuotaBE.ValorCuotaSerieRow
            rowValorCuota = CType(oValorCuotaBE.ValorCuota.NewRow(), ValorCuotaBE.ValorCuotaRow)
            rowDetalleCuota = CType(oValorCuotaBE.ValorCuotaSerie.NewRow(), ValorCuotaBE.ValorCuotaSerieRow)
            CargarObjetoDetalle(rowDetalleCuota)
            CargarObjetoCabecera(rowValorCuota)
            If Not ValidarCuotaPreCierre() Then
                oValorCuotaBE.ValorCuota.AddValorCuotaRow(rowValorCuota)
                oValorCuotaBE.ValorCuotaSerie.AddValorCuotaSerieRow(rowDetalleCuota)
                oValorCuotaBE.AcceptChanges()
                ObjValorCuota.Insertar_ValorCuota(oValorCuotaBE, DatosRequest)
                ObjValorCuota.PrecioValorCuota(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), DatosRequest)
                InsertarSeries()
                AlertaJS("Registro guardado correctamente.")
            Else
                AlertaJS("Existe un precierre generado desde el sistema de Operaciones, no se puede modificar la informacion presentada.")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al guardar los datos: " & Replace(ex.Message, "'", ""))
        Finally
            btnAceptar.Text = "Aceptar"
            btnAceptar.Enabled = True
        End Try
    End Sub

    Private Sub InsertarSeries()
        Dim dtSeries As DataTable = UIUtility.ObtenerSeriesFirbi(ObjValorCuota, tbFechaOperacion.Text, ddlPortafolio.SelectedValue, totalCuotas)
        Dim oPortafolioBM As New PortafolioBM

        For Each dr As DataRow In dtSeries.Rows
            oPortafolioBM.Insertar_PorcentajeSeries(ddlPortafolio.SelectedValue, dr("CodigoSerie"), UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), dr("Porcentaje"), Me.DatosRequest, Decimal.Parse(dr("ValoresCierreOnline")), Decimal.Parse(dr("CuotasCierreOnline")))
        Next


     End Sub
    Protected Sub btnRefrescar_Click(sender As Object, e As System.EventArgs) Handles btnRefrescar.Click
        Try
            listButtons.Attributes.Add("style", "display:inline")
            CargaPantalla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar los datos: " & ex.Message.ToString().Replace("'", ""))
        Finally
            btnRefrescar.Enabled = True
        End Try
    End Sub
    Protected Sub btnImprimir_Click(sender As Object, e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim dtTabla As New DataTable
            dtTabla = ObjValorCuota.ReporteValorCuota(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), tbFechaOperacion.Text)
            dtTabla.TableName = "dsValorCuota"
            If dtTabla.Rows.Count > 0 Then
                oReport.Load(Server.MapPath("Reportes/rptValorCuota.rpt"))
                oReport.SetDataSource(dtTabla)
                oReport.SetParameterValue("@Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                oReport.SetParameterValue("SubtototalInv", tbInversionesSubTotal.Text)
                oReport.SetParameterValue("IGV", txtigv.Text)
                oReport.SetParameterValue("Comision", txtporcom.Text)
                oReport.SetParameterValue("Usuario", Usuario)
                oReport.SetParameterValue("ComprasT", IIf(tbComprasT.Text.Trim = String.Empty, 0, tbComprasT.Text))
                Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
                Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
                exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
                exportOpts.ExportFormatOptions = pdfOpts
                oReport.ExportToHttpResponse(exportOpts, Response, True, "ValorCuota")
            Else
                AlertaJS("No se ha registrado el calculo de Valor Cuota para este dia y portafolio.")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al imprimir: " & Replace(ex.Message, "'", ""))
        Finally
            btnImprimir.Text = "Imprimir"
            btnImprimir.Enabled = True
        End Try
    End Sub
    Protected Sub form1_Unload(sender As Object, e As System.EventArgs) Handles form1.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
    'Desc: Lógica para obtener los dividendos decretados por custodia
    Protected Sub btnVerDistribucion_Click(sender As Object, e As System.EventArgs) Handles btnVerDistribucion.Click
        Try
            ConsultarDistribucionLib() 'Consultar previamente si existe distribución
            Dim mensaje As String = String.Empty
            mensaje = VerDistribucionLiberadas()
            AlertaJS2(mensaje, "GrabarDistribucionLib();") 'Mensaje de distribución del portafolio
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la distribución de los dividendos decretados: " & Replace(ex.Message, "'", ""))
        Finally
            btnVerDistribucion.Text = "Ver Distribución"
            btnVerDistribucion.Enabled = True
        End Try
    End Sub
    Protected Sub btnInicializar_Click(sender As Object, e As System.EventArgs) Handles btnInicializar.Click
        Try
            listButtons.Attributes.Add("style", "display:inline")
            InicializarDatosAutomaticos()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al inicializar los datos: " & Replace(ex.Message, "'", ""))
        Finally
            btnInicializar.Text = "Inicializar"
            btnInicializar.Enabled = True
        End Try
    End Sub
    Protected Sub cbHabilitarCarga_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbHabilitarCarga.CheckedChanged
        btnCarga.Enabled = IIf(cbHabilitarCarga.Checked = True, False, True)
        If cbHabilitarCarga.Checked Then
            iptRuta.Attributes.Add("disabled", "true")
        Else
            iptRuta.Attributes.Remove("disabled")
        End If
    End Sub
    Private Function VerDistribucionLiberadas() As String
        VerDistribucionLiberadas = String.Empty
        Dim oPortafolioBM As New PortafolioBM
        Dim dtPorcentaje As DataTable
        Dim fechaAnterior As String
        fechaAnterior = fnFechaAnteriorSinFeriado(tbFechaOperacion.Text)
        dtPorcentaje = oPortafolioBM.Portafolio_Series_Cuotas(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(fechaAnterior))
        Dim sumPorcentaje As Decimal = ExistePorcentajeSeries(dtPorcentaje)
        If sumPorcentaje = 0 Then
            Throw New System.Exception("No existen porcentajes series cargados a la fecha.")
        End If
        Dim objDividendosBM As New DividendosRebatesLiberadasBM
        Dim dt As DataTable
        dt = objDividendosBM.DividendosRebatesLiberadas_ObtenerMontoTotalDividendos(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        'If dt.Rows(0)("MontoTotalOperacion") <> 0 Then
        If dt.Rows.Count > 0 Then
            VerDistribucionLiberadas = CreateMensajeDistribucion(dt, dtPorcentaje)
        Else
            Throw New System.Exception("No existen dividendos liberados a la fecha.")
        End If
    End Function
    Private Function CreateMensajeDistribucion(ByVal dt As DataTable, ByVal dtPorcentaje As DataTable) As String
        CreateMensajeDistribucion = String.Empty
        Dim mensaje As New StringBuilder
        Dim porcentaje As Decimal
        Dim MontoOperacion As Decimal
        Dim MontoTotalOperacion As Decimal
        mensaje.Append("<table border=""0"" style=""width: 100%; text-align: left; font-weight: normal;"">")
        For Each dr As DataRow In dt.Rows
            MontoOperacion = Decimal.Parse(dr("MontoOperacion"))
            mensaje.Append("<tr>")
            mensaje.Append("<td colspan=""2"">" & dr("CodigoNemonico") & "</td>")
            mensaje.Append("<td style=""text-align:right;"">" & Format(MontoOperacion, "##,##0.00") & "</td>")
            mensaje.Append("</tr>")
            MontoTotalOperacion = MontoTotalOperacion + MontoOperacion
        Next

        mensaje.Append("<tr><td colspan=""3"">-------------------------------------------------------------------------</td></tr>")
        mensaje.Append("<tr><td colspan=""2"">Total Dividendos:</td><td style=""text-align:right;"">" & Format(MontoTotalOperacion, "##,##0.00") & "</td></tr>")
        mensaje.Append("<tr><td colspan=""3"" style=""text-align:center;"">")

        If Convert.ToInt16(hdCantRegDistribLib.Value) > 0 Then
            mensaje.Append("<input type=""button"" value=""Exportar rentabilidad"" onClick=""ExportarArchivo();"" class=""btn btn-integra"" style=""font-size:10px; width:120px;"" /></td></tr>")
        Else
            mensaje.Append("<input type=""button"" value=""Exportar rentabilidad"" onClick=""ExportarArchivo();"" disabled=""disabled"" class=""btn btn-integra"" style=""font-size:10px; width:120px;"" /></td></tr>")
        End If

        mensaje.Append("<tr><td colspan=""3"">&nbsp;</td></tr>")
        mensaje.Append("<tr><td colspan=""3"">-------------------------------------------------------------------------</td></tr>")
        For Each drPorcentaje As DataRow In dtPorcentaje.Rows
            porcentaje = Decimal.Parse(drPorcentaje("Porcentaje"))
            mensaje.Append("<tr><td>" & drPorcentaje("Nombre") & "</td><td style=""text-align:right;"">" & Format(porcentaje, "##,##0.0000000") & "%" & "</td><td style=""text-align:right;"">" & Format((MontoTotalOperacion * (porcentaje / 100)), "##,##0.0000000") & "</td></tr>")
        Next
        mensaje.Append("</table>")

        CreateMensajeDistribucion = mensaje.ToString()
    End Function
    Private Function ExistePorcentajeSeries(ByVal p_dtPorcentaje As DataTable) As Decimal
        ExistePorcentajeSeries = 0
        Dim porcentaje As Decimal = 0
        For Each dr As DataRow In p_dtPorcentaje.Rows
            porcentaje = porcentaje + Decimal.Parse(dr("Porcentaje"))
        Next
        ExistePorcentajeSeries = porcentaje
    End Function
    'Exportar rentabilidad
    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Dim dt As DataTable
        dt = New ReporteGestionBM().reporteRentabilidad_Flujos(ddlPortafolio.SelectedValue, ConvertirFechaaDecimal(tbFechaOperacion.Text))
        copiaNotepad("Rentabilidad", dt)
    End Sub
    'Grabar distribución de dividendos
    Protected Sub btnGrabarDistribucionLib_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGrabarDistribucionLib.Click
        Dim fechaAnterior As String
        fechaAnterior = fnFechaAnteriorSinFeriado(tbFechaOperacion.Text)

        Dim oDistribucion As DistribucionLibBE
        Dim dt As DataTable = New DividendosRebatesLiberadasBM().DividendosRebatesLiberadas_ObtenerMontosDividendosDetallado_Flujos(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))

        'If dt.Rows.Count > 0 Then
        Dim rptaElim As Boolean = New DistribucionLibBM().Eliminar(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        'End If

        For Each fila As DataRow In dt.Rows
            If fila("CodigoSerie") = "SERB" Then
                oDistribucion = New DistribucionLibBE

                oDistribucion.CodigoPortafolioSBS = fila("CodigoPortafolioSBS")
                oDistribucion.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
                'oDistribucion.CodigoNemonico = dt.Rows(0)("CodigoMnemonico")
                oDistribucion.CodigoNemonico = fila("CodigoMnemonico") 'Grabar Nemónico
                oDistribucion.Serie = fila("CodigoSerie")
                oDistribucion.Monto = (fila("MontoOperacion") * (fila("Porcentaje") / 100))

                Dim rpta As Boolean = New DistribucionLibBM().Insertar(oDistribucion, Me.DatosRequest)
            End If
        Next
        ConsultarDistribucionLib()
    End Sub
    Private Sub ConsultarDistribucionLib()
        Dim dt As DataTable = New DividendosRebatesLiberadasBM().ConsultarDistribucionLib(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        If Not dt Is Nothing Then
            If dt.Rows.Count > 0 Then
                hdCantRegDistribLib.Value = dt.Rows(0)(0).ToString()
            Else
                hdCantRegDistribLib.Value = 0
            End If
        Else
            hdCantRegDistribLib.Value = 0
        End If
    End Sub
    'Creación del archivo txt para el reporte de rentabilidad
    Private Sub copiaNotepad(ByVal Archivo As String, ByVal dt As DataTable, Optional ByVal dtPortafolio As DataTable = Nothing)
        Dim sRutaArchivo As String
        Dim nombreArchivo As String
        Dim sufijo As String
        nombreArchivo = Archivo & "_" & ddlPortafolio.SelectedItem.Text & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & System.DateTime.Now.ToString("_hhmmss") & ".txt"
        sRutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & nombreArchivo
        sufijo = Request.QueryString("RPT")
        escribirArchivo(sRutaArchivo, dt)
        System.GC.Collect()
        Response.Clear()
        Response.ContentType = "application/txt"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & nombreArchivo)
        Response.WriteFile(sRutaArchivo)
        Response.End()
    End Sub
    Private Sub escribirArchivo(ByVal rutaArchivo As String, ByVal dt As DataTable)
        Dim lineaArchivo As String
        Dim escritura As New IO.StreamWriter(rutaArchivo)
        escritura.Flush()
        For i = 0 To dt.Rows.Count - 1
            lineaArchivo = ""
            For j = 0 To dt.Columns.Count - 1
                'Remueve Acentos
                lineaArchivo = UIUtility.RemoveDiacritics(lineaArchivo & dt.Rows(i)(j).ToString.Trim & ";")
            Next
            lineaArchivo = lineaArchivo.Remove(lineaArchivo.Length - 1, 1)
            escritura.WriteLine(lineaArchivo)
        Next
        escritura.Close()
    End Sub
    Private Function ObtenerRescateValores() As Decimal
        ObtenerRescateValores = 0
        Dim dtRescateValores As DataTable
        Try
            dtRescateValores = ObjValorCuota.ValorCuota_ObtenerRescateValores(hdCodigoPortafolioSisOpe.Value, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), "RES")
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
        If rescateValores > 0 Then 'Si el rescate valor es cero, se mantiene el valor de la caja de texto.
            ObtenerRescateValores = rescateValores
        Else
            ObtenerRescateValores = Decimal.Parse(Replace(IIf(tbRescatesValores.Text.Trim = "", "0", tbRescatesValores.Text.Trim), ",", "")) 'OT10989 - 11/12/2017 - Ian Pastor M. Devuelve el valor que esté en la caja de texto.
        End If
    End Function
    'Descripción: Obtiene los rescates pendientes del sistema de operaciones
    Private Function ObtenerRescatesPendientes() As Decimal
        ObtenerRescatesPendientes = 0
        Dim montoRescatePND As Decimal = 0
        Try
            montoRescatePND = ObjValorCuota.ValorCuota_ObtenerRescatesPendientes(ddlPortafolio.SelectedValue, hdCodigoPortafolioSisOpe.Value, _
                                                                                 UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), hdSeriado.Value)
        Catch ex As Exception
            ObtenerRescatesPendientes = Decimal.Parse(Replace(IIf(txtRescatePendiente.Text.Trim = "", "0", txtRescatePendiente.Text.Trim), ",", ""))
            Exit Function
        End Try
        If montoRescatePND > 0 Then
            ObtenerRescatesPendientes = montoRescatePND
        Else
            ObtenerRescatesPendientes = Decimal.Parse(Replace(IIf(txtRescatePendiente.Text.Trim = "", "0", txtRescatePendiente.Text.Trim), ",", ""))
        End If
    End Function
    Private Sub CargarPorcentajeSeries()
        Dim bol As Boolean = ObjValorCuota.ValorCuota_ObtenerPorcentajeSeries(hdCPPadreSisOpe.Value, tbFechaOperacion.Text, ddlPortafolio.SelectedValue, ddlPortafolio.SelectedItem.Text, DatosRequest)
        If bol Then
            dgArchivo.DataSource = Nothing
            dgArchivo.DataBind()
            Dim oPortafolioBM As New PortafolioBM
            dgArchivo.DataSource = oPortafolioBM.Portafolio_Series_CuotasFirbi(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
            dgArchivo.DataBind()
            For Each row As GridViewRow In dgArchivo.Rows
                CType(row.FindControl("btnModificar"), ImageButton).Enabled = True
            Next
            'AlertaJS("La carga se realizó con éxito.")
        End If
    End Sub
    Private Sub InicializarBotones()

        btnProcesar.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnAceptar.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnImprimir.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnVerDistribucion.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnCarga.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
        btnInicializar.Attributes.Add("onClick", "this.disabled = true; this.value = 'Procesando...';")
    End Sub
    Private Sub ObtenerMontosContablesFirbi()
        Dim DTValorCuota As DataTable
        DTValorCuota = ObjValorCuota.ObtenerDatosFirbiPrecierre(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim))
        tbInversionesSubTotal.Text = DTValorCuota.Rows(0)("InversionesSubTotal")
        tbCajaPrecierre.Text = DTValorCuota.Rows(0)("CajaPrecierre")
        tbCXCtitulo.Text = DTValorCuota.Rows(0)("CXCVentaTitulo")
        tbCXPtitulo.Text = DTValorCuota.Rows(0)("CXPCompraTitulo")
        'Otros
        Dim otrosGastos As Decimal = IIf(tbOtrosGastos.Text = "", 0, tbOtrosGastos.Text)
        Dim otrosIngresos As Decimal = IIf(tbOtrosIngresos.Text = "", 0, tbOtrosIngresos.Text)

        txtCXPPrecierre.Text = tbCXPtitulo.Text
        tbValPatriPrecierre.Text = Math.Round(Decimal.Parse(tbInversionesSubTotal.Text) + Decimal.Parse(tbCajaPrecierre.Text) + Decimal.Parse(tbCXCtitulo.Text) - Decimal.Parse(tbCXPtitulo.Text) - otrosGastos + otrosIngresos, 7)
        CargarGrillaSeries(True)
        tbValPatriPrecierre2.Text = Decimal.Parse(tbValPatriPrecierre.Text) - Decimal.Parse(tbComiSAFM.Text)
    End Sub
    Private Sub InicializarDatosAutomaticos()
        If ddlPortafolio.SelectedValue = 0 Then
            AlertaJS("Debe seleccionar un Portafolio")
        Else
            Dim alerta As String = String.Empty
            Dim oPortafolioBE As PortafolioBE
            Dim oPortafolioBM As New PortafolioBM
            Dim oCarteraTituloValoracionBM As New CarteraTituloValoracionBM
            Dim oRow As PortafolioBE.PortafolioRow
            MostrarOcultarFormulario(True)
            oPortafolioBE = oPortafolioBM.Seleccionar(ddlPortafolio.SelectedValue, Me.DatosRequest)
            oRow = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
            tbCXPotras.ReadOnly = True
            txtChequePendiente.ReadOnly = False
            txtRescatePendiente.ReadOnly = False
            hdSeriado.Value = oRow.PorSerie
            hdCuotasLiberadas.Value = oRow.CuotasLiberadas
            hdCodigoPortafolioSisOpe.Value = oRow.CodigoPortafolioSisOpe 'OT10965 - 30/11/2017 - Ian Pastor M. (Obtener codigo de portafolio de operaciones)
            txtComisionPortafolio.Value = oRow.PorcentajeComision 'Obtener Porcentaje de comision del portafolio para realizar las operaciones
            hdCPPadreSisOpe.Value = oRow.CPPadreSisOpe
            HabilitarEscenario(True, oRow.PorSerie)
            InicializarControles()
            CargarGrillaSeries(True)
            'If oRow.PorSerie.Equals("S") Then
            '    dgArchivo.DataSource = oPortafolioBM.Portafolio_Series_CuotasFirbi(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
            '    dgArchivo.DataBind()
            'End If
            btnProcesar.Visible = True
            btnCarga.Visible = True
            cbHabilitarCarga.Visible = True
            'Habilitar carga automática
            If cbHabilitarCarga.Checked Then
                btnCarga.Enabled = False
                iptRuta.Attributes.Add("disabled", "true")
            End If
            Dim FormSoloLectura As Boolean = False
            If alerta.Length > 0 Then
                AlertaJS(alerta)
            End If
        End If
    End Sub
    Private Sub InicializarControles()
        Dim DTValorCuota As DataTable
        DTValorCuota = ObjValorCuota.ObtenerDatosFirbiPrecierre(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim))
        tbInversionesSubTotal.Text = DTValorCuota.Rows(0)("InversionesSubTotal")
        tbCXCtitulo.Text = DTValorCuota.Rows(0)("CXCVentaTitulo")
        tbCXPtitulo.Text = DTValorCuota.Rows(0)("CXPCompraTitulo")
        tbCajaPrecierre.Text = DTValorCuota.Rows(0)("CajaPrecierre")
        tbValCuotaPrecierre.Text = DTValorCuota.Rows(0)("ValCuotaPreCierre")
        txtRescatePendiente.Text = ObtenerRescatesPendientes()
        tbRescatesValores.Text = ObtenerRescateValores()
    End Sub

    Private Sub CargarGrillaSeries(ByVal ModoOnline As Boolean)

        Dim dtSeries As DataTable = UIUtility.ObtenerSeriesFirbi(ObjValorCuota, tbFechaOperacion.Text, ddlPortafolio.SelectedValue, totalCuotas)

        If ModoOnline Then
            Dim ComisionTotal As Decimal = 0
            For Each dr As DataRow In dtSeries.Rows
                dr("ValoresCierre") = Math.Round(dr("ValoresCierreOnline"), 7)
                dr("ValoresCieree") = Math.Round(dr("ValoresCierreOnline"), 7)
                dr("CuotasCierre") = Math.Round(dr("CuotasCierreOnline"), 7)
                dr("CuotasPrecierre") = Math.Round(dr("CuotasCierreOnline"), 7)
                dr("Porcentaje") = Math.Round(dr("PorcentajeOnline"), 7)
                dr("FlagDiferencia") = "0"
                Dim PrecierreSerie As Decimal = (Decimal.Parse(dr("PorcentajeOnline")) / 100) * Decimal.Parse(tbValPatriPrecierre.Text)
                Dim PorcComisionMensual As Decimal = (Decimal.Parse(dr("PorcentajeSerie")) / 100) / 12
                ComisionTotal = ComisionTotal + PorcComisionMensual * Decimal.Parse(PrecierreSerie)
            Next
            tbValCuotaPrecierre.Text = totalCuotas
            tbComiSAFM.Text = Math.Round(ComisionTotal, 7)
        Else

            For Each dr As DataRow In dtSeries.Rows
                dr("DiferenciaValoresCierre") = Math.Round(dr("CuotasCierre"), 7) - Math.Round(dr("CuotasCierreOnline"), 7)
                dr("FlagDiferencia") = If(dr("DiferenciaValoresCierre") = 0D, "0", "1")
                Dim mensajeFila As String = " Diferencias  (SIT - OPE) =>" + dr("CuotasPrecierre").ToString() + " - " + Convert.ToString(Math.Round(dr("CuotasCierreOnline"), 7)) + " = " + dr("DiferenciaValoresCierre").ToString()
                dr("DiferenciaMensaje") = mensajeFila
            Next

        End If



        dgArchivo.DataSource = dtSeries
        dgArchivo.DataBind()
    End Sub

    Protected Sub dgArchivo_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgArchivo.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            hdDiferenciaMensaje = CType(e.Row.FindControl("hdDiferenciaMensaje"), HiddenField)
            hdFlagDiferencia = CType(e.Row.FindControl("hdFlagDiferencia"), HiddenField)
            
            If hdFlagDiferencia.Value = "1" Then
                e.Row.Cells(6).Attributes.Add("bgcolor", "FFB27E")
                e.Row.Cells(3).Attributes.Add("bgcolor", "FFB27E")
                e.Row.Cells(3).Attributes.Add("title", hdDiferenciaMensaje.Value)
                e.Row.Cells(6).Attributes.Add("title", hdDiferenciaMensaje.Value)
            Else
                e.Row.Cells(6).Attributes.Remove("bgcolor")
                e.Row.Cells(3).Attributes.Remove("Style")
                e.Row.Cells(3).Attributes.Remove("title")
                e.Row.Cells(6).Attributes.Remove("title")
            End If

        End If



    End Sub
End Class

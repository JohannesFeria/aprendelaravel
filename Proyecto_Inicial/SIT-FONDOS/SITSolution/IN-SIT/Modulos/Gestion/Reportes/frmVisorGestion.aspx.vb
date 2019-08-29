Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Gestion_Reportes_frmVisorGestion
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Function CargarRuta() As String
        Dim oArchivoPlanoBM As New ArchivoPlanoBM
        Dim oArchivoPlanoBE As New DataSet
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("012", MyBase.DatosRequest())
        Return (oArchivoPlanoBE.Tables(0).Rows(0).Item(4))
    End Function
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim vReporte, vFondo, vMercado, FechaFin As String
        Dim vfechainicio, vfechafin As Decimal
        Dim vInstrumento As String 'Agreado para el reporte SAFP 20080622 por yanina perez
        vReporte = Request.QueryString("pReporte")
        Dim ruta As String
        ruta = CargarRuta()
        Try
            vInstrumento = IIf(Request.QueryString("pInstrumento") = "--Seleccione--", "", Request.QueryString("pInstrumento"))
            vFondo = Request.QueryString("pPortafolio")
            vfechainicio = Convert.ToDecimal(Request.QueryString("pFechaIni"))
            FechaFin = IIf(Request.QueryString("pFechaFin") Is Nothing, "", Request.QueryString("pFechaFin"))
            If FechaFin <> "" Then
                vfechafin = Convert.ToDecimal(FechaFin)
            End If
            vMercado = IIf(Request.QueryString("pMercado") = "Todos", "", Request.QueryString("pMercado"))
            '=======================================================
            '-- Cargamos infirmación del Contexto (si existe) - CRumiche
            Dim nombrePortafolio As String = ""
            If Session("context_info") IsNot Nothing Then
                If TypeOf Session("context_info") Is Hashtable Then 'Si se desea pasar otro tipo de objeto se debe validar de esta manera - CRumiche
                    Dim htInfo As Hashtable = Session("context_info")

                    If htInfo.Contains("Portafolio") Then nombrePortafolio = htInfo("Portafolio").ToString
                    '// Obtener más valores aquí de la manera mostrada
                End If
            End If
            '=======================================================            
            Select Case vReporte
                '=====> CRUMICHE: 21 Reportes revisados, remodelados e identificados como validos (2014-09-26)
                '============================================================================================
                Case "CCM" 'COMP. CARTERA POR MONEDA ' Evaluado x CRumiche
                    Dim dsVal As DataSet = New ValoresBM().SeleccionarValorizacion(vFondo, vfechainicio, "N", DatosRequest)
                    If dsVal.Tables(0).Rows.Count = 0 Then
                        Session("dtConsulta") = Nothing
                        EjecutarJS("alert('No existe una valorización para esta fecha y Portafolio'); window.close();")
                        Exit Sub
                    End If
                    Dim ds As DataSet, dsTipado As New DsComposicionMonedaC
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.CarteraMoneda(vfechainicio, vFondo, vMercado, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.DsComposicionMonedaC.Merge(ds.Tables(0))
                    oReport.Load(Server.MapPath("RptComposicionMoneda.rpt"))
                    Dim strmercado As String = IIf(vMercado = "", "TODOS", IIf(vMercado = "1", "LOCAL", "EXTRANJERO"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaValoracion", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("@Portafolio", nombrePortafolio)
                    oReport.SetParameterValue("@Mercado", strmercado)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "CCE" 'COMP. CARTERA POR EMISOR ' Evaluado x CRumiche                                  
                    Dim ds As DataSet, dsTipado As New DsCompEmisor
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.CarteraEmisor(vFondo, vfechainicio, vfechafin, vMercado, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.Emisor.Merge(ds.Tables(0))
                    If ds.Tables.Count > 1 Then dsTipado.OperacionTransito.Merge(ds.Tables(1))
                    If ds.Tables.Count > 2 Then dsTipado.Utiles.Merge(ds.Tables(2))
                    oReport.Load(Server.MapPath("ComposicionCarteraPorEmisor.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaInicio", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    Me.crGestion.ReportSource = oReport
                Case "CCS" 'COMP. CARTERA POR SECTOR ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsCompSector
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.CarteraSector(vFondo, vfechainicio, vfechafin, vMercado, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.Sector.Merge(ds.Tables(0))
                    If ds.Tables.Count > 1 Then dsTipado.OperacionTransito.Merge(ds.Tables(1))
                    If ds.Tables.Count > 2 Then dsTipado.Utiles.Merge(ds.Tables(2))
                    oReport.Load(Server.MapPath("ComposicionCarteraPorSector.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaInicio", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    Me.crGestion.ReportSource = oReport
                Case "CCPD" 'COMP. CARTERA POR PLAZO DETALLE ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsCompPlazoDetalle
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.CarteraPlazoDetalle(vFondo, vfechainicio, vfechafin, vMercado, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.PlazoDetalle.Merge(ds.Tables(0))
                    oReport.Load(Server.MapPath("ComposicionCarteraPlazosDetalle.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaInicio", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("@Portafolio", nombrePortafolio)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "CCR" 'COMP. CARTERA POR CATEGORIA DE RIESGO DETALLE ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsCompRiesgoDetalle
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.CarteraRiesgoDetalle(vFondo, vfechainicio, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.RiesgoDetalle.Merge(ds.Tables(0))
                    oReport.Load(Server.MapPath("ComposicionCarteraRiesgoDetalle.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaValoracion", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("@Portafolio", nombrePortafolio)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    Me.crGestion.ReportSource = oReport
                Case "DCD" 'DURACION DE CARTERA DETALLE  ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New dsReporteDetalleDuraciones
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.DuracionCarteraDetalle(vFondo, vfechainicio, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.ReporteDetalleDuraciones.Merge(ds.Tables(0))
                    oReport.Load(Server.MapPath("ConsultaDetalleDuraciones.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaValoracion", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Fondo", nombrePortafolio)
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    Me.crGestion.ReportSource = oReport
                Case "SIPE" 'SALDOS DE INSTRUMENTOS POR EMPRESA ' Evaluado x CRumiche  
                    Dim ds As DataSet, dsTipado As New ReporteAuxiliarBCR
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.SaldosInstrumentosPorEmpresa(vfechainicio, vFondo, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then
                        Dim drTipado As DataRow
                        For Each drv As DataRow In ds.Tables(0).Rows
                            drTipado = dsTipado.Tables(0).NewRow()
                            drTipado.ItemArray = drv.ItemArray
                            'Forma más conveniente para pasar los datos q tengan el formato y anular diferencia de monedas a nivel de centimos
                            drTipado("InversionDolares") = Format(Convert.ToDecimal(drv("InversionDolares")), "###,##0.0000000")
                            drTipado("MontoSolesInversionDolares") = Format(Convert.ToDecimal(drv("MontoSolesInversionDolares")), "###,##0.0000000")
                            drTipado("InversionSoles") = Format(Convert.ToDecimal(drv("InversionSoles")), "###,##0.0000000")
                            drTipado("TotalCartera") = Format(Convert.ToDecimal(drv("TotalCartera")), "###,##0.0000000")
                            drTipado("TotalInversionDolares") = Format(Convert.ToDecimal(drv("TotalInversionDolares")), "###,##0.0000000")
                            drTipado("TotalMontoSolesInversionDolares") = Format(Convert.ToDecimal(drv("TotalMontoSolesInversionDolares")), "###,##0.0000000")
                            drTipado("TotalInversionSoles") = Format(Convert.ToDecimal(drv("TotalInversionSoles")), "###,##0.0000000")
                            drTipado("TotalTotalCartera") = Format(Convert.ToDecimal(drv("TotalTotalCartera")), "###,##0.0000000")
                            drTipado("GlobalInversionDolares") = Format(Convert.ToDecimal(drv("GlobalInversionDolares")), "###,##0.0000000")
                            drTipado("GlobalMontoSolesInversionDolares") = Format(Convert.ToDecimal(drv("GlobalMontoSolesInversionDolares")), "###,##0.0000000")
                            drTipado("GlobalInversionSoles") = Format(Convert.ToDecimal(drv("GlobalInversionSoles")), "###,##0.0000000")
                            drTipado("GlobalTotalCartera") = Format(Convert.ToDecimal(drv("GlobalTotalCartera")), "###,##0.0000000")
                            dsTipado.Tables(0).Rows.Add(drTipado)
                        Next
                    End If
                    oReport.Load(Server.MapPath("RptSaldosInstrumentos.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("FechaProceso", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Portafolio", nombrePortafolio)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "RDSDCDD" 'REPORTE DE STOCKS DE CERTIFICADOS DE DEPOSITO 'Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsReporteCertificadoDeposito
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.GenerarReporteCertificadosDeposito(vfechainicio, vFondo, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.ReporteCertificadoDeposito.Merge(ds.Tables(0))
                    oReport.Load(Server.MapPath("Reporte_CertificadosDeposito.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaFin", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("@Portafolio", nombrePortafolio)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "RDU" 'REPORTE DE UTILIDAD ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsReporteUtilidad

                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.GenerarReporteUtilidad(vfechainicio, vfechafin, vFondo, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.DsReporteUtilidad.Merge(ds.Tables(0))
                    oReport.Load(Server.MapPath("ReporteDeUtilidad.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@Portafolio", IIf(nombrePortafolio.Length = 0, "TODOS", nombrePortafolio))
                    oReport.SetParameterValue("@FechaInicio", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@FechaFin", UIUtility.ConvertirFechaaString(vfechafin))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "RFC" 'REPORTE DE FLUJO DE CAJA ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsReporteDeFlujoDeCaja
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.GenerarReporteFlujoCaja(vfechainicio, vFondo, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.ReporteDeFlujoDeCaja.Merge(ds.Tables(0))
                    oReport.Load(Server.MapPath("ReporteDeFlujoDeCaja.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@Portafolio", IIf(nombrePortafolio.Length = 0, "TODOS", nombrePortafolio))
                    oReport.SetParameterValue("@Fecha", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "SAFPM" 'LISTA DE PRECIOS DE MONEDA ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New TipoCamb
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.ConsultarVecTipoCambio_Por_Fecha(vfechainicio, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.TipoCambio.Merge(ds.Tables(0))
                    oReport.Load(Server.MapPath("VectorTipoCambioFecha.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@Fecha", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "CCIE" 'COMPOSICION DE CARTERA POR INSTRUMENTO-EMPRESA ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New ReporteComposicionCarteraPorInstrumento_Empresa
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.GenerarReporteComposicionCarteraInstrumentoEmpresa(vfechainicio, vFondo, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado._ReporteComposicionCarteraPorInstrumento_Empresa.Merge(ds.Tables(0))
                    oReport.Load(Server.MapPath("ComposicionCarteraPorInstrumento_Empresa.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@Portafolio", IIf(nombrePortafolio.Length = 0, "TODOS", nombrePortafolio))
                    oReport.SetParameterValue("@Fecha", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "RDSDF" 'REPORTE DE CONTROL (STOCKS) DE FORWARDS ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsReporteForwards2
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.GenerarReporteForwards(vfechainicio, vFondo, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.ReporteForwards.Merge(ds.Tables(0))
                    oReport.Load(Server.MapPath("Reporte_Forwards.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaFin", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("@Portafolio", nombrePortafolio)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "CCTR" 'COMP. CARTERA POR TIPO DE RENTA ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsCompTipoRenta
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.CarteraTipoRenta(vFondo, vfechainicio, vfechafin, vMercado, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.TipoRenta.Merge(ds.Tables(0))
                    If ds.Tables.Count > 1 Then dsTipado.OperacionTransito.Merge(ds.Tables(1))
                    If ds.Tables.Count > 2 Then dsTipado.Utiles.Merge(ds.Tables(2))
                    oReport.Load(Server.MapPath("ComposicionCarteraTipoRenta.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaInicio", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    Me.crGestion.ReportSource = oReport
                Case "CCCI" 'COMP. CARTERA POR CATEGORIA DE INSTRUMENTO ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsCompCategoriaInstrumento
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.CarteraCategoriaInstrumento(vFondo, vfechainicio, vfechafin, vMercado, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.CategoriaInstrumento.Merge(ds.Tables(0))
                    If ds.Tables.Count > 1 Then dsTipado.OperacionTransito.Merge(ds.Tables(1))
                    If ds.Tables.Count > 2 Then dsTipado.Utiles.Merge(ds.Tables(2))
                    oReport.Load(Server.MapPath("ComposicionCarteraPorCategoriaInstrumento.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaInicio", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "CCRR" 'COMP. CARTERA POR CATEGORIA DE RIESGO RESUMEN ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsCompRiesgoResumen
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.CarteraRiesgoResumen(vfechainicio, vMercado, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.RiesgoResumen.Merge(ds.Tables(0))
                    If ds.Tables.Count > 1 Then dsTipado.OperacionTransito.Merge(ds.Tables(1))
                    If ds.Tables.Count > 2 Then dsTipado.Utiles.Merge(ds.Tables(2))
                    oReport.Load(Server.MapPath("ComposicionCarteraRiesgoResumen.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaValoracion", UIUtility.ConvertirFechaaString(vfechafin))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "CCEX" 'COMP. CARTERA EXTERIOR ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsCarteraExterior
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.CarteraExterior(vfechainicio, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.CarteraExterior.Merge(ds.Tables(0))
                    If ds.Tables.Count > 1 Then dsTipado.OperacionTransito.Merge(ds.Tables(1))
                    If ds.Tables.Count > 2 Then dsTipado.Utiles.Merge(ds.Tables(2))
                    oReport.Load(Server.MapPath("ComposicionCarteraExterior.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaValoracion", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "DC" 'DURACION DE CARTERA RESUMEN ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New dsReporteResumenDuraciones
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.DuracionCarteraResumen(vfechainicio, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.ResumenDuraciones.Merge(ds.Tables(0))
                    oReport.Load(Server.MapPath("ReporteResumenDuraciones.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaValoracion", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "REPCOMPB" 'REPORTE DE COMPOSICION DE BENCHMARK SBS ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsCompBenchmarkSBS
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.GenerarReporteCompBenchmark(vfechainicio, vFondo, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.CompBenchmarkSBS.Merge(ds.Tables(0))
                    If ds.Tables.Count > 1 Then dsTipado.OperacionTransito.Merge(ds.Tables(1))
                    oReport.Load(Server.MapPath("RptCompBenchmarkSBS.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaValoracion", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "CCP1" 'COMP. CARTERA POR PLAZO ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsCompPlazoResumen
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.CarteraPlazoResumen(vFondo, vfechainicio, vfechafin, vMercado, DatosRequest)
                        Session("dsTipado") = ds
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.CompPlazoResumen.Merge(ds.Tables(0))
                    If ds.Tables.Count > 1 Then dsTipado.OperacionTransito.Merge(ds.Tables(1))
                    oReport.Load(Server.MapPath("ComposicionCarteraPlazosResumen.rpt"))
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaValoracion", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "SAFP" 'LISTA DE PRECIOS SAFP ' Evaluado x CRumiche
                    Dim ds As DataSet, dsTipado As New DsPreciosLista
                    Dim dsTC As DataSet, dsTipadoTC As New ListaTipoCambio
                    If Not Page.IsPostBack Then
                        Dim repBM As New ReporteGestionBM
                        ds = repBM.ListaPrecios(vfechainicio, vInstrumento, DatosRequest)
                        dsTC = repBM.ListaTipoCambio(vfechainicio, DatosRequest)
                        Session("dsTipado") = ds
                        Session("dsTipoCambio") = dsTC
                    Else
                        ds = CType(Session("dsTipado"), DataSet)
                        dsTC = CType(Session("dsTipoCambio"), DataSet)
                    End If
                    If ds.Tables.Count > 0 Then dsTipado.DsPreciosLista.Merge(ds.Tables(0))
                    If dsTC.Tables.Count > 0 Then dsTipadoTC.ListaTipoCambioHorizontal.Merge(dsTC.Tables(0))
                    oReport.Load(Server.MapPath("ListaPrecios.rpt"))
                    Dim strSocio As String = ""
                    If vInstrumento = "SBS" Then strSocio = "Instrumentos No Asociados"
                    If vInstrumento = "AFP" Then strSocio = "Instrumentos Asociados"
                    oReport.SetDataSource(dsTipado)
                    oReport.OpenSubreport("ListaTCambioSub").SetDataSource(dsTipadoTC)
                    oReport.SetParameterValue("@FechaInicio", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Socio", strSocio)
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    crGestion.ReportSource = oReport
                Case "Grupo1Det"
                    Dim dtConsulta As DataTable
                    Dim ds As New RegDetContable
                    Dim dr As RegDetContable.DetalladoRow
                    Dim regDet As New VaxRegedBM
                    dtConsulta = regDet.SeleccionarPorCartera(vFondo, vfechainicio, DatosRequest).Tables(0)
                    For Each drv As DataRow In dtConsulta.Rows
                        dr = ds.Tables(0).NewRow()
                        dr("Portafolio") = drv("Portafolio")
                        dr("CuentaContable") = drv("CuentaContable")
                        dr("SinonimoCodigoInstrumento") = drv("SinonCodigoInstrumento")
                        dr("SinonimoCodigoEmisor") = drv("SinonCodigoEmisor")
                        dr("SaldoAnteriorMonedaLocal") = drv("SaldoAnteriorMonedaLocal")
                        dr("TotalCompras") = drv("TotalCompras")
                        dr("TotalVentas") = drv("TotalVentas")
                        dr("TotalVencimientos") = drv("TotalVencimientos")
                        dr("TotalCupones") = drv("TotalCupones")
                        dr("IndicadorCustodio") = drv("IndicadorCustodio")
                        ds.Tables(0).Rows.Add(dr)
                    Next
                    oReport.Load(Server.MapPath("RegDet.rpt"))
                    oReport.SetDataSource(ds)
                    oReport.DataDefinition.FormulaFields("FechaProceso").Text = "'" & UIUtility.ConvertirFechaaString(vfechainicio) & "'"
                    oReport.DataDefinition.FormulaFields("Portafolio").Text = "'" & vFondo & "'"
                    oReport.DataDefinition.FormulaFields("Usuario").Text = "'" & MyBase.Usuario & "'"
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    Me.crGestion.ReportSource = oReport
                Case "CCP"
                    Dim dtConsulta As DataTable
                    Dim ds As New CXCP
                    Dim dr As CXCP.CcpRow
                    If Session("dtCCP") Is Nothing Then
                        Dim oVaxTitcusBE As New VaxTitcusBM
                        dtConsulta = oVaxTitcusBE.GetCuentasPorCobrarPagarToVAX(vFondo, Convert.ToDecimal(vfechainicio), DatosRequest).Tables(0)
                    Else
                        dtConsulta = Session("dtCCP")
                    End If
                    For Each drv As DataRow In dtConsulta.Rows
                        dr = ds.Tables(0).NewRow()
                        dr("TipoRegistro") = drv("TipoRegistro")
                        dr("FechaOperacion") = UIUtility.ConvertirFechaaString(Convert.ToDecimal(drv("FechaOperacion")))
                        dr("CodigoTercero") = drv("CodigoTercero")
                        dr("ImporteMonedaLocal") = drv("Importe")
                        If Not drv("FechaIngreso") Is DBNull.Value Then
                            dr("FechaIngreso") = UIUtility.ConvertirFechaaString(Convert.ToDecimal(drv("FechaIngreso")))
                        End If
                        dr("CodigoMonedaSBS") = drv("CodigoMonedaSBS")
                        dr("ImporteMonedaExtranjera") = drv("MontoOrigen")
                        dr("TipoCambio") = drv("TipoCambio")
                        dr("CodigoOperacion") = drv("CodigoOperacion")
                        dr("CodigoSBS") = drv("CodigoSBS")
                        dr("DescripcionOperacion") = drv("DescripcionOperacion")
                        dr("DescripcionTercero") = drv("DescripcionTercero")
                        dr("CodigoMoneda") = drv("CodigoMoneda")
                        ds.Tables(0).Rows.Add(dr)
                    Next
                    oReport.Load(Server.MapPath("Ccp.rpt"))
                    oReport.SetDataSource(ds)
                    oReport.DataDefinition.FormulaFields("FechaProceso").Text = "'" & UIUtility.ConvertirFechaaString(vfechainicio) & "'"
                    oReport.DataDefinition.FormulaFields("Fondo").Text = "'" & vFondo & "'"
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    Me.crGestion.ReportSource = oReport
                Case "BMIDAS" 'REPORTE DE CARGA DE INTERFAZ BMIDAS
                    Dim dtConsulta As DataTable
                    Dim dsTipado As New DsReporteBmidas
                    Dim drconsulta As DataRow
                    oReport.Load(Server.MapPath("ReporteBmidas.rpt"))
                    Dim repBM As New ReporteGestionBM
                    If Not Page.IsPostBack Then
                        dtConsulta = repBM.GenerarReporteBmidas(vfechainicio, vFondo, DatosRequest).Tables(0)
                        Session("dtConsulta") = dtConsulta
                    Else
                        dtConsulta = CType(Session("dtConsulta"), DataTable)
                    End If
                    For Each drv As DataRow In dtConsulta.Rows
                        drconsulta = dsTipado.Tables(0).NewRow()
                        drconsulta("CodigoPortafolioSBS") = drv("CodigoPortafolioSBS")
                        drconsulta("NombreBanco") = drv("NombreBanco")
                        drconsulta("Fecha") = drv("Fecha")
                        drconsulta("SaldoSoles") = drv("SaldoSoles")
                        drconsulta("SaldoDolares") = drv("SaldoDolares")
                        drconsulta("SaldoEuros") = drv("SaldoEuros")
                        drconsulta("SaldoYenes") = drv("SaldoYenes")
                        drconsulta("SaldoLibras") = drv("SaldoLibras")
                        drconsulta("SaldoDolCan") = drv("SaldoDolCan")
                        drconsulta("SaldoMxN") = drv("SaldoMxN")
                        dsTipado.Tables(0).Rows.Add(drconsulta)
                    Next
                    dsTipado.Merge(dsTipado, False, System.Data.MissingSchemaAction.Ignore)
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaProceso", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    Me.crGestion.ReportSource = oReport
                Case "BCOS" 'REPORTE DE CARGA DE INTERFAZ BCOS
                    Dim dtConsulta As DataTable
                    Dim dsTipado As New DsReporteBcos
                    Dim drconsulta As DataRow
                    oReport.Load(Server.MapPath("ReporteBcos.rpt"))
                    Dim repBM As New ReporteGestionBM
                    If Not Page.IsPostBack Then
                        dtConsulta = repBM.GenerarReporteBcos(vfechainicio, vFondo, DatosRequest).Tables(0)
                        Session("dtConsulta") = dtConsulta
                    Else
                        dtConsulta = CType(Session("dtConsulta"), DataTable)
                    End If
                    For Each drv As DataRow In dtConsulta.Rows
                        drconsulta = dsTipado.Tables(0).NewRow()

                        drconsulta("CodigoIndicador") = drv("CodigoIndicador")
                        drconsulta("PortafolioSBS") = nombrePortafolio
                        drconsulta("Valor") = drv("Valor")
                        drconsulta("Fecha") = drv("Fecha")

                        dsTipado.Tables(0).Rows.Add(drconsulta)
                    Next
                    dsTipado.Merge(dsTipado, False, System.Data.MissingSchemaAction.Ignore)
                    oReport.SetDataSource(dsTipado)
                    oReport.SetParameterValue("@FechaProceso", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", MyBase.Usuario)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    Me.crGestion.ReportSource = oReport
                Case "REGAUX"
                    Dim dtConsulta As New DataTable
                    Dim ds As New RegAuxContable
                    Dim dr As RegAuxContable.AuxiliarRow
                    Dim regAux As New VaxRegauxBM
                    dtConsulta = regAux.SeleccionarPorCartera(vFondo, vfechainicio, DatosRequest).Tables(0)
                    For Each drv As DataRow In dtConsulta.Rows
                        dr = ds.Tables(0).NewRow()
                        dr("Portafolio") = drv("portafolio")
                        dr("CuentaContable") = drv("cuentacontable")
                        dr("SinonimoCodigoInstrumento") = drv("sinoncodigoinstrumento")
                        dr("SaldoAnteriorMonedaLocal") = Format(Convert.ToDecimal(drv("saldoanteriormonedalocal")), "###,##0.0000000")
                        dr("SumaSaldoAnteriorMonedaLocal") = Format(Convert.ToDecimal(drv("SumaSaldoAnteriorMonedaLocal")), "###,##0.0000000")
                        dr("GlobalSaldoAnteriorMonedaLocal") = Format(Convert.ToDecimal(drv("GlobalSaldoAnteriorMonedaLocal")), "###,##0.0000000")
                        dr("TotalCompras") = Format(Convert.ToDecimal(drv("totalcompras")), "###,##0.0000000")
                        dr("SumaTotalCompras") = Format(Convert.ToDecimal(drv("SumaTotalCompras")), "###,##0.0000000")
                        dr("GlobalTotalCompras") = Format(Convert.ToDecimal(drv("GlobalTotalCompras")), "###,##0.0000000")
                        dr("TotalVentas") = Format(Convert.ToDecimal(drv("totalventas")), "###,##0.0000000")
                        dr("SumaTotalVentas") = Format(Convert.ToDecimal(drv("SumaTotalVentas")), "###,##0.0000000")
                        dr("GlobalTotalVentas") = Format(Convert.ToDecimal(drv("GlobalTotalVentas")), "###,##0.0000000")
                        dr("TotalVencimientos") = Format(Convert.ToDecimal(drv("totalvencimientos")), "###,##0.0000000")
                        dr("SumaTotalVencimientos") = Format(Convert.ToDecimal(drv("SumaTotalVencimientos")), "###,##0.0000000")
                        dr("GlobalTotalVencimientos") = Format(Convert.ToDecimal(drv("GlobalTotalVencimientos")), "###,##0.0000000")
                        dr("TotalCupones") = Format(Convert.ToDecimal(drv("totalcupones")), "###,##0.0000000")
                        dr("SumaTotalCupones") = Format(Convert.ToDecimal(drv("SumaTotalCupones")), "###,##0.0000000")
                        dr("GlobalTotalCupones") = Format(Convert.ToDecimal(drv("GlobalTotalCupones")), "###,##0.0000000")
                        dr("TotalRentabilidad") = Format(Convert.ToDecimal(drv("totalrentabilidad")), "###,##0.0000000")
                        dr("SumaTotalRentabilidad") = Format(Convert.ToDecimal(drv("SumaTotalRentabilidad")), "###,##0.0000000")
                        dr("GlobalTotalRentabilidad") = Format(Convert.ToDecimal(drv("GlobalTotalRentabilidad")), "###,##0.0000000")
                        dr("SaldoDelDia1") = Format(Convert.ToDecimal(drv("saldodeldia1")), "###,##0.0000000")
                        dr("SumaSaldoDelDia1") = Format(Convert.ToDecimal(drv("SumaSaldoDelDia1")), "###,##0.0000000")
                        dr("GlobalSumaSaldoDelDia1") = Format(Convert.ToDecimal(drv("GlobalSaldoDelDia1")), "###,##0.0000000")
                        dr("SaldoDelDia2") = Format(Convert.ToDecimal(drv("saldodeldia2")), "###,##0.0000000")
                        dr("SumaSaldoDelDia2") = Format(Convert.ToDecimal(drv("SumaSaldoDelDia2")), "###,##0.0000000")
                        dr("GlobalSaldoDelDia2") = Format(Convert.ToDecimal(drv("GlobalSaldoDelDia2")), "###,##0.0000000")
                        ds.Tables(0).Rows.Add(dr)
                    Next
                    oReport.Load(Server.MapPath("RegAux.rpt"))
                    oReport.SetDataSource(ds)
                    oReport.DataDefinition.FormulaFields("FechaProceso").Text = "'" & UIUtility.ConvertirFechaaString(vfechainicio) & "'"
                    oReport.DataDefinition.FormulaFields("Portafolio").Text = "'" & vFondo & "'"
                    oReport.DataDefinition.FormulaFields("Usuario").Text = "'" & MyBase.Usuario & "'"
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                    Me.crGestion.ReportSource = oReport
                Case "RDVP" 'REPORTE DE DIFERENCIA DEL VECTOR DE PRECIOS (SBS VS ELEX) 'Aparentemente ya no se usa
            End Select

        Catch ex As Exception
            EjecutarJS("alert('" + ex.Message.ToString() + "'); window.close();")
        End Try
    End Sub

    Private Sub MergePDF(ByVal archivosEntrada As String, ByVal archivoSalida As String)
        Dim archivoEntrada() = archivosEntrada.Split("!")
        Dim i As Byte
        Dim MemStream As New System.IO.MemoryStream
        Dim doc As New iTextSharp.text.Document(PageSize.A4, 33, 0, 5, 0)
        Dim reader As iTextSharp.text.pdf.PdfReader
        Dim numberOfPages As Integer
        Dim currentPageNumber As Integer

        Dim PdfPath As String = archivoSalida
        Dim writer As iTextSharp.text.pdf.PdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, New FileStream(PdfPath, FileMode.Create))
        doc.Open()
        Dim cb As iTextSharp.text.pdf.PdfContentByte = writer.DirectContent
        Dim page As iTextSharp.text.pdf.PdfImportedPage
        Dim rotation As Integer
        Dim filename As String
        For i = 0 To archivoEntrada.Length - 1
            filename = archivoEntrada(i)
            reader = New PdfReader(filename)
            numberOfPages = reader.NumberOfPages
            currentPageNumber = 0
            Do While (currentPageNumber < numberOfPages)
                currentPageNumber += 1
                doc.SetPageSize(reader.GetPageSizeWithRotation(currentPageNumber))
                doc.NewPage()
                page = writer.GetImportedPage(reader, currentPageNumber)
                rotation = reader.GetPageRotation(currentPageNumber)

                If (rotation = 90) Or (rotation = 270) Then
                    cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(currentPageNumber).Height)
                Else
                    cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
                End If
            Loop
        Next
        doc.Close()
    End Sub
    Protected Sub Modulos_Gestion_Reportes_frmVisorGestion_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class
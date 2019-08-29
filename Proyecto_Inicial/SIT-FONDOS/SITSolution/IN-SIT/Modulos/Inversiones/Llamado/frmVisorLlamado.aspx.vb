Imports ParametrosSIT
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Inversiones_InstrumentosNegociados_Llamado_frmVisorLlamado
    Inherits BasePage
    Dim codigoMultifondo As String = New ParametrosGeneralesBM().SeleccionarPorFiltro(FECHA_FONDO, FECHA_NEGOCIO, "", "", Nothing).Rows(0)("Valor").ToString.Trim
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strtitulo, strnemonico, strisin, strsbs, strsubtitulo, strcodigo, strportafolio, strdescripcionPortafolio, strclase, strmoneda, stroperacion,
        strmnemonicotemp, codportafolio As String
        Dim dscaract As New dscaracteristicas
        Dim dttempoperacion As DataTable
        Dim drcar As DataRow
        strclase = Request.QueryString("vclase")
        strnemonico = Request.QueryString("vnemonico")
        strisin = Request.QueryString("visin")
        strsbs = Request.QueryString("vsbs")
        strmnemonicotemp = Request.QueryString("vnemonicotemp")
        codportafolio = Request.QueryString("cportafolio")
        strportafolio = Request.QueryString("vportafolio")
        strdescripcionPortafolio = Request.QueryString("vdescripcionPortafolio")
        If Session("context_info") IsNot Nothing Then
            If TypeOf Session("context_info") Is Hashtable Then
                Dim htInfo As Hashtable = Session("context_info")
                If htInfo.Contains("Portafolio") Then strportafolio = htInfo("Portafolio").ToString
            End If
            Session.Remove("context_info")
        End If
        tbFondo.Value = strportafolio
        Dim dsValor As New DataSet
        Dim oOIFormulas As New OrdenInversionFormulasBM
        If strclase = "BONOS" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_Bonos(Request.QueryString("vcodigo"),
            codportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_Bonos(strnemonico, codportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val13") = CType(drValor("val_CodigoISIN"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val14") = CType(drValor("val_CodigoSBS"), String)
                drcar("Car15") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val15") = CType(drValor("val_TipoRenta"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "ACCIONES" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_Acciones(Request.QueryString("vcodigo"),
            codportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_Acciones(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = ""
                drcar("Val2") = ""
                drcar("Car3") = CType(drValor("lbl_sigDivFecha"), String)
                drcar("Val3") = CType(drValor("val_sigDivFecha"), String)
                drcar("Car4") = CType(drValor("lbl_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val4") = CType(drValor("val_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car5") = CType(drValor("lbl_sigDivFactor"), String)
                drcar("Val5") = CType(drValor("val_sigDivFactor"), String)
                drcar("Car6") = CType(drValor("lbl_NroOperDiarProm"), String)
                drcar("Val6") = CType(drValor("val_NroOperDiarProm"), String)
                drcar("Car7") = ""
                drcar("Val7") = ""
                drcar("Car8") = CType(drValor("lbl_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = ""
                drcar("Val9") = ""
                drcar("Car10") = CType(drValor("val_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val10") = CType(drValor("val_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "CERTIFICADO DE DEPOSITO" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_CertificadoDeposito(Request.QueryString("vcodigo"),
            codportafolio, DatosRequest)
            'OT 10090 - 21/03/2017 - Carlos Espejo
            'Descripcion: Se elimian el parametro obsoleto
            dsValor = oOIFormulas.SeleccionarCaracValor_CertificadoDeposito(strnemonico, codportafolio, Request.QueryString("vcodigo"))
            'OT 10090 Fin
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = ""
                drcar("Val7") = ""
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = ""
                drcar("Val10") = ""
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val13") = CType(drValor("val_TipoRenta"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val14") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car15") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val15") = CType(drValor("val_CodigoSBS"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "CERTIFICADO DE SUSCRIPCION" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_CertificadoSuscripcion(Request.QueryString("vcodigo"),
            codportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_CertificadoSuscripcion(Request.QueryString("cportafolio"), strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = ""
                drcar("Val2") = ""
                drcar("Car3") = CType(drValor("lbl_sigdivfecha"), String)
                drcar("Val3") = CType(drValor("val_sigdivfecha"), String)
                drcar("Car4") = CType(drValor("lbl_valorDFC"), String)
                drcar("Val4") = CType(drValor("val_valorDFC"), String)
                drcar("Car5") = CType(drValor("lbl_sigdivfactor"), String)
                drcar("Val5") = CType(drValor("val_sigdivfactor"), String)
                drcar("Car6") = CType(drValor("lbl_NroOperDiarProm"), String)
                drcar("Val6") = CType(drValor("val_NroOperDiarProm"), String)
                drcar("Car7") = ""
                drcar("Val7") = ""
                drcar("Car8") = CType(drValor("lbl_Priceearnings"), String)
                drcar("Val8") = CType(drValor("val_Priceearnings"), String)
                drcar("Car9") = ""
                drcar("Val9") = ""
                drcar("Car10") = CType(drValor("lbl_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val10") = CType(drValor("val_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                drcar("Car13") = ""
                drcar("Val13") = ""
                drcar("Car14") = ""
                drcar("Val14") = ""
                drcar("Car15") = ""
                drcar("Val15") = ""
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "PAPELES COMERCIALES" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_PapelesComerciales(Request.QueryString("vcodigo"),
            codportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_PapelesComerciales(strnemonico, codportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val13") = CType(drValor("val_TipoRenta"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val14") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car15") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val15") = CType(drValor("val_CodigoISIN"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "PAGARES" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_Pagares(Request.QueryString("vcodigo"),
            codportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_Pagares(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Emisor"), String)
                drcar("Val1") = CType(drValor("val_Emisor"), String)
                drcar("Car2") = CType(drValor("lbl_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val2") = CType(drValor("val_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car3") = CType(drValor("lbl_Serie"), String)
                drcar("Val3") = CType(drValor("val_Serie"), String)
                drcar("Car4") = CType(drValor("lbl_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val4") = CType(drValor("val_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car5") = CType(drValor("lbl_TasaCupon"), String)
                drcar("Val5") = CType(drValor("val_TasaCupon"), String)
                drcar("Car6") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val6") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car7") = CType(drValor("lbl_TipoCupon"), String)
                drcar("Val7") = CType(drValor("val_TipoCupon"), String)
                drcar("Car8") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val8") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car9") = CType(drValor("lbl_PeriodoPago"), String)
                drcar("Val9") = CType(drValor("val_PeriodoPago"), String)
                drcar("Car10") = CType(drValor("lbl_FechaVcto"), String)
                drcar("Val10") = CType(drValor("val_FechaVcto"), String)
                drcar("Car11") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_fc"), String)
                drcar("Val13") = CType(drValor("val_fc"), String)
                drcar("Car14") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val14") = CType(drValor("val_TipoRenta"), String)
                drcar("Car15") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val15") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "ORDENES DE FONDO" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_OrdenesFondo(Request.QueryString("vcodigo"),
            codportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_OrdenesFondo(strnemonico, codportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val13") = CType(drValor("val_TipoRenta"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val14") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car15") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val15") = CType(drValor("val_CodigoISIN"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "DEPOSITOS A PLAZO" Then
            If Session("dtdatosoperacion") Is Nothing Then
                Dim dtOrden As DataTable = New OrdenPreOrdenInversionBM().ListarOrdenesInversionPorCodigoOrden(Request.QueryString("vcodigo"), codportafolio, DatosRequest,
                PORTAFOLIO_MULTIFONDOS).Tables(0)
                Session("dtdatosoperacion") = New DepositoPlazos().ObtenerDatosOperacion(DatosRequest, dtOrden.Rows(0))
            End If
        End If
        If strclase = "OPERACIONES DE REPORTE" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_OperacionesReporte(Request.QueryString("vcodigo"),
            codportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_OperacionesReporte(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = ""
                drcar("Val2") = ""
                drcar("Car3") = CType(drValor("lbl_sigDivFecha"), String)
                drcar("Val3") = CType(drValor("val_sigDivFecha"), String)
                drcar("Car4") = CType(drValor("lbl_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val4") = CType(drValor("val_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car5") = CType(drValor("lbl_sigDivFactor"), String)
                drcar("Val5") = CType(drValor("val_sigDivFactor"), String)
                drcar("Car6") = CType(drValor("lbl_NroOperDiarProm"), String)
                drcar("Val6") = CType(drValor("val_NroOperDiarProm"), String)
                drcar("Car7") = ""
                drcar("Val7") = ""
                drcar("Car8") = CType(drValor("lbl_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = ""
                drcar("Val9") = ""
                drcar("Car10") = CType(drValor("lbl_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val10") = CType(drValor("val_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                drcar("Car13") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val13") = CType(drValor("val_CodigoISIN"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val14") = CType(drValor("val_CodigoSBS"), String)
                drcar("Car15") = CType(drValor("lbl_Mercado"), String)
                drcar("Val15") = CType(drValor("val_Mercado"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "INSTRUMENTOS COBERTURADOS" Then
            dsValor = oOIFormulas.SeleccionarCaracValor_InstCoberturados(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val1") = CType(drValor("val_TipoRenta"), String)
                drcar("Car2") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val2") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car3") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val3") = CType(drValor("val_CodigoISIN"), String)
                drcar("Car4") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val4") = CType(drValor("val_CodigoSBS"), String)
                drcar("Car5") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val5") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car6") = CType(drValor("lbl_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_Emisor"), String)
                drcar("Val10") = CType(drValor("val_Emisor"), String)
                drcar("Car11") = CType(drValor("lbl_fc"), String)
                drcar("Val11") = CType(drValor("val_fc"), String)
                drcar("Car12") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val12") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car13") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val13") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car14") = CType(drValor("lbl_FechaVcto"), String)
                drcar("Val14") = CType(drValor("val_FechaVcto"), String)
                drcar("Car15") = CType(drValor("lbl_observacion"), String)
                drcar("Val15") = CType(drValor("val_observacion"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "LETRAS HIPOTECARIAS" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_LetrasHipotecarias(Request.QueryString("vcodigo"),
            codportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_LetrasHipotecarias(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinLetra"), String)
                drcar("Val2") = CType(drValor("val_FechaFinLetra"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String)
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String)
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String)
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String)
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String)
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String)
                drcar("Car12") = CType(drValor("lbl_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val13") = CType(drValor("val_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car14") = CType(drValor("lbl_fc"), String)
                drcar("Val14") = CType(drValor("val_fc"), String)
                drcar("Car15") = CType(drValor("lbl_FechaVcto"), String)
                drcar("Val15") = CType(drValor("val_FechaVcto"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "INSTRUMENTOS ESTRUCTURADOS" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") =
            UIUtility.ObtenerDatosOperacion_InstrumentosEstructurados(Request.QueryString("vcodigo"), codportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_InstEstructurados(strnemonico, codportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_nemo1"), String)
                drcar("Val1") = CType(drValor("val_nemo1"), String)
                drcar("Car2") = CType(drValor("lbl_nemo2"), String)
                drcar("Val2") = CType(drValor("val_nemo2"), String)
                drcar("Car3") = CType(drValor("lbl_nemo3"), String)
                drcar("Val3") = CType(drValor("val_nemo3"), String)
                drcar("Car4") = CType(drValor("lbl_porc1"), String)
                drcar("Val4") = CType(drValor("val_porc1"), String)
                drcar("Car5") = CType(drValor("lbl_porc2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val5") = CType(drValor("val_porc2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car6") = CType(drValor("lbl_porc3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_porc3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_precio1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_precio1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_precio2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_precio2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_precio3"), String)
                drcar("Val9") = CType(drValor("val_precio3"), String)
                drcar("Car10") = CType(drValor("lbl_porcParticip"), String)
                drcar("Val10") = CType(drValor("val_porcParticip"), String)
                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                drcar("Car13") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val13") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car14") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val14") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car15") = ""
                drcar("Val15") = ""
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        Dim sNNeg As String = ""
        If strclase.Substring(1) = "OPERACIONES DERIVADAS - FORWARD DIVISAS" Or strclase.Substring(1) = "COMPRA/VENTA MONEDA EXTRANJERA" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = Session("dtdatosoperacionSW" & strclase.Substring(0, 1))
            sNNeg = strclase.Substring(0, 1)
            strclase = strclase.Substring(1)
        End If
        stroperacion = Request.QueryString("voperacion")
        Dim dsoper As New dsDatosOperacion
        Dim droper As DataRow
        dttempoperacion = CType(Session("dtdatosoperacion"), DataTable)
        For Each dr As DataRow In dttempoperacion.Rows
            droper = dsoper.Tables(0).NewRow
            droper("c1") = dr("c1")
            droper("v1") = dr("v1")
            droper("c2") = dr("c2")
            droper("v2") = dr("v2")
            droper("c3") = dr("c3")
            droper("v3") = dr("v3")
            droper("c4") = dr("c4")
            If strclase.Equals("COMPRA/VENTA MONEDA EXTRANJERA") And stroperacion.ToLower.IndexOf("compra") >= 0 Then
                droper("v4") = dr("v6")
                droper("v6") = dr("v4")
            Else
                droper("v4") = dr("v4")
                droper("v6") = dr("v6")
            End If
            droper("c5") = dr("c5")
            droper("v5") = dr("v5")
            droper("c6") = dr("c6")
            droper("c7") = dr("c7")
            droper("v7") = dr("v7")
            droper("c8") = dr("c8")
            droper("v8") = dr("v8")
            droper("c9") = dr("c9")
            droper("v9") = dr("v9")
            droper("c10") = dr("c10")
            droper("v10") = dr("v10")
            droper("c11") = dr("c11")
            droper("v11") = dr("v11")
            droper("c12") = dr("c12")
            droper("v12") = dr("v12")
            droper("c13") = dr("c13")
            droper("v13") = dr("v13")
            droper("c14") = dr("c14")
            droper("v14") = dr("v14")
            droper("c15") = dr("c15")
            droper("v15") = dr("v15")
            droper("c16") = dr("c16")
            droper("v16") = dr("v16")
            droper("c17") = dr("c17")
            droper("v17") = dr("v17")
            droper("c18") = dr("c18")
            droper("v18") = dr("v18")
            droper("c19") = dr("c19")
            droper("v19") = dr("v19")
            droper("c20") = dr("c20")
            droper("v20") = dr("v20")
            droper("c21") = dr("c21")
            droper("v21") = dr("v21")
            If strclase = "OPERACIONES DERIVADAS - FORWARD DIVISAS" Or strclase.Substring(1) = "OPERACIONES DERIVADAS - FORWARD DIVISAS" Then
                droper("c22") = dr("c22")
                droper("v22") = dr("v22")
                droper("v18") = dr("v18").ToString.Substring(0, IIf(dr("v18").ToString.Length >= 42, 42, dr("v18").ToString.Length))
            Else
                droper("c22") = ""
                droper("v22") = ""
            End If
            dsoper.Tables(0).Rows.Add(droper)
        Next
        Dim oStream As New System.IO.MemoryStream
        Dim Cro As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim Archivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Llamado/RptLlamado.rpt"
        strcodigo = Request.QueryString("vcodigo")
        strmoneda = Request.QueryString("vmoneda")
        Dim ordenes As String() = strcodigo.Split("-")
        Dim ordenOrigen As String = String.Empty
        Dim ordenDestino As String = String.Empty
        Dim esTraspaso As Boolean = False
        If ordenes.Length > 1 Then
            esTraspaso = True
            ordenOrigen = ordenes(0).ToString()
            ordenDestino = ordenes(1).ToString()
        End If
        Try
            Dim StrNombre As String = "Usuario"
            Dim strusuario As String
            Dim codigoPortafolio = New ParametrosGeneralesBM().SeleccionarPorFiltro(FECHA_FONDO, FECHA_NEGOCIO, "", "", Nothing).Rows(0)("Valor").ToString.Trim
            If codportafolio <> codigoPortafolio Then
                If esTraspaso Then
                    strtitulo = "Orden de Inversion: (Origen) Nro - " + ordenOrigen + " , (Destino) Nro - " + ordenDestino
                Else
                    strtitulo = "Orden de Inversion Nro - " + strcodigo
                End If
            Else
                If esTraspaso Then
                    strtitulo = "PreOrden de Inversion: Origen Nro - " + (ordenOrigen) + " , (Destino) Nro - " + ordenDestino
                Else
                    strtitulo = "PreOrden de Inversion Nro - " + strcodigo
                End If
            End If
            strsubtitulo = strclase + " - " + stroperacion
            Dim dscomisiones As New DataSet
            Dim dscomi As New dsdatoscomisiones
            Dim drcomi As DataRow
            dscomisiones = UIUtility.ObtenerTablaimpuestosComisionesGuardado(strcodigo, codportafolio)
            For Each drcomisiones As DataRow In dscomisiones.Tables(0).Rows
                drcomi = dscomi.Tables(0).NewRow
                drcomi("Descripcion1") = drcomisiones("Descripcion1")
                drcomi("ValorOcultoComision1") = drcomisiones("ValorOcultoComision1")
                drcomi("PorcentajeComision1") = IIf(Convert.ToString(drcomisiones("PorcentajeComision1")).Equals(""), "",
                Convert.ToString(drcomisiones("PorcentajeComision1")) + " : ")
                drcomi("Descripcion2") = drcomisiones("Descripcion2")
                drcomi("ValorOcultoComision2") = drcomisiones("ValorOcultoComision2")
                drcomi("PorcentajeComision2") = IIf(Convert.ToString(drcomisiones("PorcentajeComision2")).Equals(""), "",
            Convert.ToString(drcomisiones("PorcentajeComision2")) + " : ")
                dscomi.Tables(0).Rows.Add(drcomi)
            Next
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            strusuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)
            Cro.Load(Archivo)
            Dim oOrdenInverion As New OrdenPreOrdenInversionBM()
            Dim dsFirma As New DsFirma
            Dim drFirma As DsFirma.FirmaRow
            Dim dr2 As DataRow
            Dim dtFirmas As New DataTable
            dtFirmas = oOrdenInverion.ObtenerFirmasLlamadoOI(ordenes(0).ToString(), UIUtility.ObtenerFechaNegocio(codigoMultifondo), DatosRequest).Tables(0)
            dr2 = dtFirmas.Rows(0)
            drFirma = CType(dsFirma.Firma.NewFirmaRow(), DsFirma.FirmaRow)
            drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(0), String)))
            drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(1), String)))
            drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(2), String)))
            drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(3), String)))
            drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(4), String)))
            drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(5), String)))
            drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(6), String)))
            drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(7), String)))
            drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(8), String)))
            drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(9), String)))
            drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(10), String)))
            drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(11), String)))
            drFirma.Firma13 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(12), String)))
            drFirma.Firma14 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(13), String)))
            dsFirma.Firma.AddFirmaRow(drFirma)
            dsFirma.Firma.AcceptChanges()
            Cro.SetDataSource(dsFirma)
            Cro.OpenSubreport("RptCaracteristicas").SetDataSource(dscaract)
            Cro.OpenSubreport("RptDatosOperacion").SetDataSource(dsoper)
            Cro.OpenSubreport("RptDatosComisiones").SetDataSource(dscomi)
            dsoper = New dsDatosOperacion
            Dim dttemptotal As DataTable = CType(Session("dtdatosoperacion"), DataTable)
            For Each dr As DataRow In dttemptotal.Rows
                If CType(dr("c19"), String) = "" Then
                    Exit For
                End If
                droper = dsoper.Tables(0).NewRow
                droper("c19") = dr("c19")
                droper("v19") = dr("v19")
                droper("c20") = dr("c20")
                droper("v20") = dr("v20")
                droper("c21") = dr("c21")
                droper("v21") = dr("v21")
                dsoper.Tables(0).Rows.Add(droper)
            Next
            If Not strdescripcionPortafolio Is Nothing Then
                strportafolio = strdescripcionPortafolio
            End If
            Cro.OpenSubreport("RptTotalOperacion").SetDataSource(dsoper)
            Cro.SetParameterValue("@Titulo", strtitulo)
            Cro.SetParameterValue("@Subtitulo", strsubtitulo)
            Cro.SetParameterValue("@Fondo", "Portafolio: " & strportafolio)
            Cro.SetParameterValue("@Moneda", "Moneda: " & strmoneda)
            If esTraspaso Then
                Cro.SetParameterValue("@Operacion", "Operación: " & stroperacion & "-" & Constantes.M_TRASPASO_INGRESO_)
            Else
                Cro.SetParameterValue("@Operacion", "Operación: " & stroperacion)
            End If

            If strmnemonicotemp <> "" Or strmnemonicotemp <> Nothing Then
                Cro.SetParameterValue("@MnemonicoReporte", "Mnemónico Temporal: " & strmnemonicotemp)
            Else
                Cro.SetParameterValue("@MnemonicoReporte", "")
            End If
            If strisin <> "" Or strisin <> Nothing Then
                Cro.SetParameterValue("@CodigoIsin", "Codigo Isin: " & strisin)
            Else
                Cro.SetParameterValue("@CodigoIsin", "")
            End If
            If strsbs <> "" Or strsbs <> Nothing Then
                Cro.SetParameterValue("@CodigoSBS", "Codigo SBS: " & strsbs)
            Else
                Cro.SetParameterValue("@CodigoSBS", "")
            End If
            If strnemonico <> "" Or strnemonico <> Nothing Then
                Cro.SetParameterValue("@CodigoNemonico", "Codigo Mnemónico: " & strnemonico)
            Else
                Cro.SetParameterValue("@CodigoNemonico", "")
            End If
            Cro.SetParameterValue("@Ruta_Logo", ConfigurationManager.AppSettings("RUTA_LOGO"))
            Dim rptStream As New System.IO.MemoryStream
            rptStream = ExportToMemoryStream(Cro.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat))
            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-Disposition", "inline;filename=" + "Llamado" + sNNeg + ".pdf")
            Response.BinaryWrite(rptStream.ToArray())
            Response.End()
            Session("dtdatosoperacion") = Nothing
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
End Class
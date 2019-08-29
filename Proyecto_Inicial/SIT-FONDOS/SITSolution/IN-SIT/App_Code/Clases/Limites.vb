Imports Microsoft.VisualBasic
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine

Public Class Limites
    Inherits System.Web.UI.Page

    Dim oLimiteBM As New LimiteBM

    Public _CodLimite As String
    Public _CodLimiteCaracteristica As String
    Public _FechaOperacion As Date
    Public _DetalladoPorFondo As String
    Public _Escenario As String
    Public _ProcesarLimite As Integer
    Public _Portafolio As String = ""

    Public _FolderReportes As String = "" 'Esto debe setearse si se llama a la funcion Generar desde el consolidado
    Private _CodigoValorBase As String
    Public _PorcentajeCercaLimite As Decimal

    Public Shared Function LimiteMaximoNegociacion_Validar(ByVal Fecha As Decimal) As ArrayList
        Dim oLimiteBM As New LimiteBM
        Return oLimiteBM.LimiteMaximoNegociacion_Validar(Fecha)
    End Function

    Public Function GeneraLimite(ByVal Usuario As String, ByVal DatosRequest As DataSet) As CrystalDecisions.CrystalReports.Engine.ReportDocument

        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim DecFechaLimite As Decimal = CType(_FechaOperacion.ToString("yyyyMMdd"), Decimal)
        Dim strFechaOperacion As String = _FechaOperacion.ToShortDateString
        Dim PosicionTotalN1SUMA As Decimal = 0
        _PorcentajeCercaLimite = New LimiteBM().SeleccionarPorcentajeCercaLimite(_CodLimite, _CodLimiteCaracteristica)

        If Not Page.IsPostBack And (_CodLimite = "22" Or _CodLimite = "23" Or _CodLimite = "24") Then 'RGF 20080711 se agrego como caso particular el reporte de limites BVL (24)

            Dim dtDatos As DataTable, montoTotal3Fondos As Decimal 'Parametro de Salida
            Select Case _CodLimite
                Case "22"
                    dtDatos = New ReporteLimitesBM().Seleccionar_ReporteLimite_Trading_Diario(DecFechaLimite, "MULTIFONDO", montoTotal3Fondos, DatosRequest).Tables(0)
                    oReport.Load(Server.MapPath(_FolderReportes & "LimiteTradingDiario.rpt"))
                Case "23"
                    dtDatos = New ReporteLimitesBM().Seleccionar_ReporteLimite_Trading_Mensual(DecFechaLimite, "MULTIFONDO", montoTotal3Fondos, DatosRequest).Tables(0)
                    oReport.Load(Server.MapPath(_FolderReportes & "LimiteTradingMensual.rpt"))
                Case "24"
                    dtDatos = New ReporteLimitesBM().Seleccionar_ReporteLimite_BVL(DecFechaLimite, "MULTIFONDO", DatosRequest).Tables(0)
                    oReport.Load(Server.MapPath(_FolderReportes & "Limites_BVL.rpt"))
            End Select
            oReport.SetDataSource(dtDatos)
            If _CodLimite = "22" Or _CodLimite = "23" Then
                oReport.SetParameterValue("@FechaInicio", strFechaOperacion)
                oReport.SetParameterValue("@Portafolio", "MULTIFONDO")
                oReport.SetParameterValue("@TotalPatrimonio3Fondos", montoTotal3Fondos)
                oReport.SetParameterValue("@fechaT-1", strFechaOperacion)
            Else
                oReport.SetParameterValue("@FechaLimite", strFechaOperacion)
            End If

            oReport.SetParameterValue("@Usuario", Usuario)
        ElseIf (_CodLimite = "49" Or _CodLimite = "50") Then

            Dim dtDatos As DataTable
            dtDatos = New ReporteLimitesBM().Seleccionar_ReporteLimite_Moneda(_CodLimite, DecFechaLimite, _Portafolio, _Escenario, _ProcesarLimite).Tables(0)   'HDG INC 62882	20110406
            oReport.Load(Server.MapPath(_FolderReportes & "LimiteExteriorDetalladoDivisas.rpt"))
            oReport.SetDataSource(dtDatos)
            _CodigoValorBase = dtDatos.Rows(0)("ValorBase")
            Dim dtVB As DataTable = New ParametrosGeneralesBM().SeleccionarPorFiltro("ValBase", "", _CodigoValorBase, "", DatosRequest)
            Dim sValorBase As String = ""
            If dtVB.Rows.Count > 0 Then sValorBase = dtVB.Rows(0).Item("Nombre").ToString

            oReport.SetParameterValue("TituloReporte1", _CodLimite & " - " & oLimiteBM.SeleccionarPorFiltro(_CodLimite, "", "", DatosRequest).Tables(0).Rows(0).Item("NombreLimite").ToString)
            oReport.SetParameterValue("Fondo", _Portafolio)
            oReport.SetParameterValue("Escenario", _Escenario)
            oReport.SetParameterValue("ValorBase", sValorBase)
            oReport.SetParameterValue("FechaProceso", strFechaOperacion)
            oReport.SetParameterValue("Usuario", Usuario)
            oReport.SetParameterValue("CodigoValorBase", _CodigoValorBase)

        Else
            Dim DescValorBase As String = ""
            Dim drDatosLimite As LimiteBE.LimiteRow 'DataRow que contiene los datos del Limite seleccionado (pertenece a la entidad LimiteBE)

            Dim dtOrigen As New DataTable
            Dim NroNiveles As Integer

            If Not Page.IsPostBack Then

                dtOrigen = New ReporteLimitesBM().ProcesarLimite(_CodLimite, _CodLimiteCaracteristica, DecFechaLimite, _ProcesarLimite, _Escenario, DatosRequest)

                'RGF 20100303
                If dtOrigen Is Nothing Then
                    Return Nothing
                ElseIf dtOrigen.Rows.Count = 0 Then
                    Return Nothing
                End If

                If _CodLimite = "12" Then EliminarFilasDelBCRP(dtOrigen)

                If (_CodLimite = "36") Then
                    Dim dtOrdenPorFechaVencimiento As New DataTable
                    dtOrdenPorFechaVencimiento = OrdenarTabla(dtOrigen, "1=1", "FechaVencimiento,ValorNivel")
                    dtOrigen = Nothing
                    dtOrigen = dtOrdenPorFechaVencimiento
                    PosicionTotalN1SUMA = CalcularSumaParaMarkToMarket(dtOrigen)
                    FormatearPosicion(dtOrigen)
                End If
                'Buscamos la data del Limite(Siempre debería encontrarse data). Si no se encuantra la data del Límite no se puede continuar, generamos una excepcion
                Dim oLimiteBE As LimiteBE = New LimiteBM().Seleccionar(_CodLimite, DatosRequest)
                If oLimiteBE.Limite.Rows.Count = 0 Then Throw New Exception("No se encontraron los datos del Límite.")
                drDatosLimite = oLimiteBE.Limite.Rows(0)
                _CodigoValorBase = drDatosLimite("valorBase")
                Dim dt As DataTable = New ParametrosGeneralesBM().SeleccionarPorFiltro("ValBase", "", _CodigoValorBase, "", DatosRequest)
                If dt.Rows.Count > 0 Then DescValorBase = dt.Rows(0).Item("Nombre").ToString
                If (_CodLimite = "36" Or _CodLimite = "65") Then 'JHC REQ 66056: Implementacion Futuros
                    NroNiveles = 2
                Else
                    'NroNiveles = ObtenerNumeroNiveles(dtOrigen) 'HDG INC 61852	20101207
                    NroNiveles = New ReporteLimitesBM().Obtener_LimiteNivelMaximo(_CodLimiteCaracteristica) 'HDG INC 61852	20101207
                End If

                Session("NroNiveles") = NroNiveles
                Session("dtOrigen") = dtOrigen
                Session("dtDatosLimite") = drDatosLimite
                Session("PosicionTotalN1SUMA") = PosicionTotalN1SUMA
            Else
                NroNiveles = CType(Session("NroNiveles"), Integer)
                dtOrigen = CType(Session("dtOrigen"), DataTable)
                drDatosLimite = CType(Session("dtDatosLimite"), LimiteBE.LimiteRow)
                PosicionTotalN1SUMA = Session("PosicionTotalN1SUMA")
            End If

            _CodigoValorBase = drDatosLimite.ValorBase

            Dim dsDestino As DsReporteLimites = CargarDataSetReporte(NroNiveles, dtOrigen, _CodLimite)
            If dsDestino.Tables(0).Rows.Count = 0 Then Return Nothing

            Dim tituloLimite As String = _CodLimite & " - " & oLimiteBM.SeleccionarPorFiltro(_CodLimite, "", "", DatosRequest).Tables(0).Rows(0).Item("NombreLimite").ToString

            If _CodLimite = "36" Then
                oReport.Load(Server.MapPath(_FolderReportes & "LimiteExteriorDetallado2forward.rpt"))
            ElseIf _CodLimite = "65" Then
                oReport.Load(Server.MapPath(_FolderReportes & "LimiteExteriorDetallado2Futuro.rpt"))
            ElseIf _CodLimite = "51" Then
                oReport.Load(Server.MapPath(_FolderReportes & "LimiteExteriorDetallado2Swaps.rpt"))
            ElseIf _CodLimite = "54" Then
                oReport.Load(Server.MapPath(_FolderReportes & "LimiteExteriorDetallado3Bench.rpt"))
            ElseIf _CodLimite = "57" Then
                oReport.Load(Server.MapPath(_FolderReportes & "LimiteExteriorDetalladoCobertura.rpt"))
            ElseIf _CodLimite = "63" Then
                oReport.Load(Server.MapPath(_FolderReportes & "LimiteExteriorDetalladoBanda2.rpt"))
            ElseIf _CodLimite = "64" Then
                oReport.Load(Server.MapPath(_FolderReportes & "LimiteExteriorDetallado2Adm.rpt"))
            Else
                oReport.Load(Server.MapPath(_FolderReportes & "LimiteExteriorDetallado" & IIf(NroNiveles = 1, "", NroNiveles.ToString) & ".rpt"))
            End If

            oReport.SetDataSource(dsDestino)
            Dim patrimonioIncDec As Double = 0
            Dim totalAjustes As Double = 0
            If _CodLimite <> "64" Then 'HDG OT 65023-4 20120611
                If _CodigoValorBase = "PATRIM" And _Escenario = "ESTIMADO" Then
                    patrimonioIncDec = CrearSubReporte_PatrimonioIncrementoDecremento(oReport, _Escenario, _Portafolio, DecFechaLimite)
                End If
                totalAjustes = CrearSubReporte_AjusteLimitesEstimados(oReport, DecFechaLimite, _Portafolio, _CodLimite, DatosRequest)
            End If

            oReport.SetParameterValue("TituloReporte1", tituloLimite)
            oReport.SetParameterValue("Fondo", _Portafolio)
            oReport.SetParameterValue("Escenario", _Escenario)
            oReport.SetParameterValue("ValorBase", DescValorBase)
            oReport.SetParameterValue("SubtituloAgrupacion", "")
            oReport.SetParameterValue("FechaProceso", strFechaOperacion)
            oReport.SetParameterValue("Usuario", Usuario)
            oReport.SetParameterValue("PorcentajeCercaLimite", _PorcentajeCercaLimite)

            If NroNiveles = 1 Or NroNiveles = 2 Or NroNiveles = 3 Then
                oReport.SetParameterValue("TituloReporte2", "")
                oReport.SetParameterValue("CodigoValorBase", _CodigoValorBase)
            End If
            If NroNiveles = 4 Then
                oReport.SetParameterValue("CodigoValorBase", _CodigoValorBase)
            End If

            Select Case NroNiveles
                Case 1
                    If _CodLimite <> "57" Then
                        oReport.SetParameterValue("TipoFactor", drDatosLimite.TipoFactor)
                    End If
                Case 2
                    Dim PosicionTotalN1 As Decimal
                    If drDatosLimite.UnidadPosicion = "MTM" Then
                        PosicionTotalN1 = PosicionTotalN1SUMA
                    End If
                    If _CodLimite <> 36 And _CodLimite <> 65 And _CodLimite <> 51 Then
                        oReport.SetParameterValue("TipoFactor", drDatosLimite.TipoFactor)
                    End If
                    Dim TotalPatrimRF As Decimal = 0
                    If _CodLimite <> 36 And _CodLimite <> 51 And _CodLimite <> 64 And _CodLimite <> 65 And _CodLimite <> 63 Then 'JCH OT 66768 20130425
                        If _CodLimite = 62 Then
                            TotalPatrimRF = New ReporteLimitesBM().TotalPatrimonioRentaFija(DecFechaLimite, _Portafolio, DatosRequest)
                        End If
                        oReport.SetParameterValue("TotalPatrimonioRentaFija", TotalPatrimRF)
                    End If
                    If _CodLimite <> 63 Then
                        oReport.SetParameterValue("PosicionTotalN1", PosicionTotalN1) 'LETV 20090428
                        oReport.SetParameterValue("MarketShare", drDatosLimite.MarketShare)
                        oReport.SetParameterValue("DetalladoPorFondo", _DetalladoPorFondo) 'RGF 20081029
                        oReport.SetParameterValue("UnidadPosicion", drDatosLimite.UnidadPosicion) 'LETV 20090428
                    End If
                    'FIN JCH OT 66768 20130425
                Case 3 'ini OT 62839 20110408
                    If _CodLimite = 46 Then
                        Dim TotalSaldoBancos As Decimal
                        Dim Condicion As Integer
                        TotalSaldoBancos = New ReporteLimitesBM().TotalSaldoBancos(DecFechaLimite, _Portafolio, _Escenario, DatosRequest)
                        Dim oLimiteBE As New LimiteBE
                        Dim oRow As LimiteBE.LimiteRow
                        oLimiteBE = oLimiteBM.Seleccionar("46", DatosRequest)
                        oRow = DirectCast(oLimiteBE.Limite.Rows(0), LimiteBE.LimiteRow)
                        TotalSaldoBancos = IIf(oRow.SaldoBanco = "S", TotalSaldoBancos, 0)
                        Condicion = IIf(oRow.SaldoBanco = "S", 1, 0)
                        oReport.SetParameterValue("SaldoBancos", TotalSaldoBancos)
                        oReport.SetParameterValue("DescSaldoBancos", "Saldo Banco")
                        oReport.SetParameterValue("CondicionSaldoBanco", Condicion)
                    Else
                        oReport.SetParameterValue("SaldoBancos", 0)
                        oReport.SetParameterValue("DescSaldoBancos", "")
                        oReport.SetParameterValue("CondicionSaldoBanco", 0)
                    End If
                    oReport.SetParameterValue("TipoFactor", drDatosLimite.TipoFactor)   'HDG 20120216
                Case 4
                    Dim saldoBanco As Decimal = 0

                    saldoBanco = 0
                    If _CodLimite = "08" Then
                        Dim oLimiteBE As New LimiteBE
                        Dim oRow As LimiteBE.LimiteRow
                        oLimiteBE = oLimiteBM.Seleccionar("08", DatosRequest)
                        oRow = DirectCast(oLimiteBE.Limite.Rows(0), LimiteBE.LimiteRow)
                        saldoBanco = IIf(oRow.SaldoBanco = "S", 1, 0)
                    End If
                    oReport.SetParameterValue("SaldoBancosExterior", saldoBanco)

            End Select

            If _CodLimite <> "64" Then
                oReport.SetParameterValue("PatrimonioIncrementoDecremento", patrimonioIncDec)
                oReport.SetParameterValue("TotalAjustes", totalAjustes)
            End If
        End If

        Return oReport
    End Function

    Private Function OrdenarTabla(ByVal dt As DataTable, ByVal filtro As String, ByVal orderby As String) As DataTable
        Dim rows As DataRow()
        Dim dtNew As DataTable = dt.Clone()

        rows = dt.Select(filtro, orderby)

        For Each dr As DataRow In rows
            dtNew.ImportRow(dr)
        Next
        Return dtNew
    End Function

    Private Sub EliminarFilasDelBCRP(ByVal dt As DataTable)
        For Each dr As DataRow In dt.Select("ValorNivel = 'BCRP'")
            dt.Rows.Remove(dr)
        Next
    End Sub

    Private Sub FormatearPosicion(ByVal dt As DataTable)
        Dim FechaVencimientoTEMP As Decimal
        Dim PosicionTEMP As Decimal
        If dt.Rows.Count > 1 Then
            FechaVencimientoTEMP = CDec(dt.Rows(1)("FechaVencimiento"))
        End If
        Dim i As Integer

        For i = 1 To dt.Rows.Count - 1
            If dt.Rows(i)("FechaVencimiento") = FechaVencimientoTEMP Then
                PosicionTEMP = dt.Rows(i)("Posicion")
                dt.Rows(i - 1)("Posicion") = 0
            Else
                dt.Rows(i - 1)("Posicion") = PosicionTEMP
                PosicionTEMP = dt.Rows(i)("Posicion")
            End If
            FechaVencimientoTEMP = CDec(dt.Rows(i)("FechaVencimiento"))

            If i = dt.Rows.Count Then
                dt.Rows(i)("Posicion") = PosicionTEMP
            End If
        Next
    End Sub

    Private Function CargarDataSetReporte(ByVal ultimoNivel As Integer, ByVal dtOrigen As DataTable, ByVal CodigoLimite As String) As DsReporteLimites

        Dim dsDestino As New DsReporteLimites
        Dim dtDestino As DsReporteLimites.ReporteLimitesDataTable = dsDestino.Tables(0)
        Dim drDestino As DataRow

        Dim drNivel1, drNivel2, drNivel3, drNivel4 As DataRow  'Temporales de los DataRow de Nivel menor a Ultimo nivel 
        Dim nivelActual As Integer

        For Each drActual As DataRow In dtOrigen.Rows

            nivelActual = drActual("Secuencial") 'nivel del DataRow Actual

            If nivelActual = ultimoNivel Then

                drDestino = dtDestino.NewRow()
                Select Case nivelActual
                    Case 1 'Campos personalizados para este Nivel
                        drDestino("EntidadVinculada") = drActual("EntidadVinculada")
                        drDestino("Factor") = drActual("Factor")
                        drDestino("TotalPasivo") = drActual("TotalPasivo")
                        drDestino("TotalActivo") = drActual("TotalActivo")
                        drDestino("Patrimonio") = drActual("Patrimonio")
                        If CodigoLimite = "57" Then    'HDG OT 62087 Nro10-R19 20110310
                            drDestino("ValorEfectivoColocado") = drActual("ValorEfectivoColocado")
                            drDestino("FloatOficioMultiple") = drActual("FloatOficioMultiple")
                            drDestino("FechaVencimiento") = UIUtility.ConvertirFechaaString(drActual("FechaVencimiento"))
                            drDestino("FechaOperacion") = UIUtility.ConvertirFechaaString(drActual("FechaOperacion"))
                            drDestino("CodigoMoneda") = drActual("CodigoMoneda")
                        End If
                    Case 2 'Seteamos los valores previos al nivel 2
                        SetValoresDeNivel(1, drNivel1, drDestino) 'Insertando los valores del nivel 1
                        'Campos personalizados para este Nivel
                        drDestino("ValorEfectivoColocado") = drActual("ValorEfectivoColocado")
                        drDestino("FloatOficioMultiple") = drActual("FloatOficioMultiple")
                        drDestino("UnidadesEmitidas") = drActual("UnidadesEmitidas")
                        drDestino("Factor") = drActual("Factor")
                        If CodigoLimite = "36" Or _CodLimite = "65" Then 'JHC REQ 66056: Implementacion Futuros
                            drDestino("EntidadVinculada") = drActual("EntidadVinculada")
                            drDestino("FechaVencimiento") = UIUtility.ConvertirFechaaString(drActual("FechaVencimiento"))
                            drDestino("FechaOperacion") = UIUtility.ConvertirFechaaString(drActual("FechaOperacion"))
                            drDestino("CodigoMoneda") = drActual("CodigoMoneda")
                        End If
                        If CodigoLimite = "43" Then
                            drDestino("CodigoMoneda") = drActual("CodigoMoneda")
                        End If
                        If CodigoLimite = "37" Or CodigoLimite = "38" Or CodigoLimite = "39" Then
                            drDestino("TotalActivo") = drNivel1("TotalActivo")
                            drDestino("TotalPasivo") = drNivel1("TotalPasivo")
                            drDestino("Patrimonio") = drNivel1("Patrimonio")
                        Else
                            drDestino("TotalActivo") = drActual("TotalActivo")
                            drDestino("TotalPasivo") = drActual("TotalPasivo")
                            drDestino("Patrimonio") = drActual("Patrimonio")
                        End If

                    Case 3
                        SetValoresDeNivel(1, drNivel1, drDestino)
                        SetValoresDeNivel(2, drNivel2, drDestino)
                        drDestino("ValorEfectivoColocado") = drActual("ValorEfectivoColocado")
                        drDestino("Patrimonio") = drActual("Patrimonio")
                        drDestino("FloatOficioMultiple") = drActual("FloatOficioMultiple")
                        drDestino("Factor") = drActual("Factor")
                        If CodigoLimite = "54" Then
                            drDestino("Factor") = drActual("Factor")
                            drDestino("FloatOficioMultiple") = drActual("FloatOficioMultiple")
                        End If
                    Case 4
                        SetValoresDeNivel(1, drNivel1, drDestino)
                        SetValoresDeNivel(2, drNivel2, drDestino)
                        SetValoresDeNivel(3, drNivel3, drDestino)
                        drDestino("Patrimonio") = drActual("Patrimonio")
                End Select

                SetValoresComunesDeLosNiveles(drActual, drDestino)
                dtDestino.Rows.Add(drDestino)

            Else
                Select Case nivelActual
                    Case 1 : drNivel1 = drActual
                    Case 2 : drNivel2 = drActual
                    Case 3 : drNivel3 = drActual
                End Select
            End If
        Next

        Return dsDestino
    End Function

    Public Sub SetValoresDeNivel(ByVal nivel As Integer, ByVal drNivel As DataRow, ByVal drDestino As DataRow)
        drDestino("DescripcionN" & nivel.ToString) = drNivel("DescripcionNivel")
        drDestino("PosicionN" & nivel.ToString) = drNivel("Posicion")
        drDestino("ParticipacionN" & nivel.ToString) = drNivel("Participacion")
        drDestino("MargenN" & nivel.ToString) = drNivel("Margen")
        drDestino("AlertaN" & nivel.ToString) = drNivel("Alerta")
        drDestino("numerico" & nivel.ToString) = drNivel("ValorPorcentaje")
        drDestino("FactorN" & nivel.ToString) = drNivel("Factor")
        drDestino("NivelSaldoBancoN" & nivel.ToString) = drNivel("NivelSaldoBanco")
        If nivel = 1 Then
            drDestino("numeric1M") = IIf(drNivel("ValorPorcentajeM") Is DBNull.Value, 0, drNivel("ValorPorcentajeM"))
        End If
    End Sub

    Public Sub SetValoresComunesDeLosNiveles(ByVal drOrigen As DataRow, ByVal drDestino As DataRow)
        drDestino("PorcentajeLimite") = drOrigen("ValorPorcentaje")
        drDestino("PorcentajeLimiteM") = drOrigen("ValorPorcentajeM")
        drDestino("Descripcion") = drOrigen("DescripcionNivel").Replace("*", " (*) ")
        drDestino("Posicion") = drOrigen("Posicion")
        drDestino("Participacion") = drOrigen("Participacion")
        drDestino("Margen") = drOrigen("Margen")
        drDestino("Alerta") = drOrigen("Alerta")
        drDestino("Tope") = drOrigen("Tope")
        drDestino("SimboloLimite") = drOrigen("Simbolo")
        drDestino("TipoLimite") = drOrigen("Tipo")
        drDestino("TotalInversion") = drOrigen("TotalInversion")
        drDestino("Bancos") = drOrigen("Bancos")
        drDestino("CuentasPorPagar") = drOrigen("CuentasPorPagar")
        drDestino("CuentasPorCobrar") = drOrigen("CuentasPorCobrar")
        drDestino("PosicionF1") = drOrigen("PosicionF1")
        drDestino("PosicionF2") = drOrigen("PosicionF2")
        drDestino("PosicionF3") = drOrigen("PosicionF3")
    End Sub

    Private Function CrearSubReporte_PatrimonioIncrementoDecremento(ByVal oReport As CrystalDecisions.CrystalReports.Engine.ReportDocument, ByVal Escenario As String, ByVal Portafolio As String, ByVal FechaOperacion As Decimal) As Double
        Dim TotalIncrementoDecremento As Decimal = 0
        If Me._CodigoValorBase = "PATRIM" And Escenario = "ESTIMADO" Then
            Dim obm As New LimiteBM
            Dim dsPatrimonioIncDec As New DsPatrimonioIncDec
            Dim drPatrimonioInDec As DataRow

            'Dim dtPatrimonioIncrementoDecremento As DataTable = obm.SeleccionarPorFiltro("PATRI", "", "", "", Me.DatosRequest)
            Dim dtPatrimonioIncrementoDecremento As DataTable = obm.SeleccionarPorFondoIncDecPatrimonio(Portafolio, FechaOperacion)
            TotalIncrementoDecremento = CalcularDecrementoIncremento(dtPatrimonioIncrementoDecremento)
            For Each drv As DataRow In dtPatrimonioIncrementoDecremento.Rows
                drPatrimonioInDec = dsPatrimonioIncDec.Tables(0).NewRow
                drPatrimonioInDec("Clasificacion") = drv("Clasificacion")
                Select Case Convert.ToString(drv("TipoIngreso")).ToUpper
                    Case "INCREMENTO"
                        drPatrimonioInDec("Nombre") = String.Concat("+ ", drv("Nombre"))
                    Case "DISMINUCION"
                        drPatrimonioInDec("Nombre") = String.Concat("- ", drv("Nombre"))
                End Select
                drPatrimonioInDec("Valor") = Convert.ToDecimal(drv("Valor"))
                drPatrimonioInDec("Tipo") = drv("TipoIngreso")
                dsPatrimonioIncDec.Tables(0).Rows.Add(drPatrimonioInDec)
            Next
            oReport.OpenSubreport("PatrimonioIncDec").SetDataSource(dsPatrimonioIncDec)
        End If
        Return TotalIncrementoDecremento
    End Function
    'CMB OT 63540 20110714
    Private Function CrearSubReporte_AjusteLimitesEstimados(ByVal oReport As ReportDocument, ByVal fechaOperacion As Decimal, ByVal codigoPortafolio As String, ByVal codigoLimite As String, ByVal DatosRequest As DataSet) As Decimal
        Dim oLimiteBM As New LimiteBM
        Dim dsAjustesLE As New DsAjustesLE
        Dim drAjustesLE As DataRow
        Dim totalAjustes As Decimal = 0
        Dim dtAjustesLE As DataTable = oLimiteBM.SeleccionarAjusteLimitesEstimados(fechaOperacion, codigoPortafolio, codigoLimite, totalAjustes, DatosRequest).Tables(0)
        For Each dr As DataRow In dtAjustesLE.Rows
            drAjustesLE = dsAjustesLE.Tables(0).NewRow
            drAjustesLE("Nombre") = dr("Nombre")
            drAjustesLE("Valor") = dr("Valor")
            dsAjustesLE.Tables(0).Rows.Add(drAjustesLE)
        Next
        oReport.OpenSubreport("Ajustes").SetDataSource(dsAjustesLE)
        If Me._Escenario = "REAL" Then
            totalAjustes = 0
        End If
        Return totalAjustes
    End Function

    Private Function CalcularDecrementoIncremento(ByVal dt As DataTable) As Decimal
        Dim valor As Decimal = 0
        For Each row As DataRow In dt.Rows
            If row("TipoIngreso") = "INCREMENTO" Then valor = valor + CDec(row("Valor"))
            If row("TipoIngreso") = "DISMINUCION" Then valor = valor - CDec(row("Valor"))
        Next
        Return valor
    End Function

    Private Function CalcularSumaParaMarkToMarket(ByVal dt As DataTable) As Decimal
        Dim Suma As Decimal = 0
        Dim dtSumaMTM As DataTable
        Dim FechaVencimientoTEMP As Decimal
        Dim SumaTEMP As Decimal
        Dim ListaMTMSoles As New ArrayList
        dtSumaMTM = OrdenarTabla(dt, "Secuencial=2", "FechaVencimiento,ValorNivel")

        If dtSumaMTM.Rows.Count > 0 Then FechaVencimientoTEMP = CDec(dtSumaMTM.Rows(0)("FechaVencimiento"))


        Dim contador As Integer = 0
        For Each row As DataRow In dtSumaMTM.Rows
            If row("FechaVencimiento") = FechaVencimientoTEMP Then
                SumaTEMP = SumaTEMP + CDec(row("Posicion"))
            Else
                Suma = Suma + Math.Abs(SumaTEMP)
                SumaTEMP = 0
                SumaTEMP = SumaTEMP + CDec(row("Posicion"))
            End If
            SetPosicion(dt, CInt(row("CodigoReporte")), Math.Abs(SumaTEMP))
            FechaVencimientoTEMP = CDec(row("FechaVencimiento"))

            contador = contador + 1

            If dtSumaMTM.Rows.Count = contador Then
                SetPosicion(dt, CInt(row("CodigoReporte")), Math.Abs(SumaTEMP))
                Suma = Suma + Math.Abs(SumaTEMP)
            End If
        Next
        Return Suma
    End Function

    Private Sub SetPosicion(ByVal dt As DataTable, ByVal CodigoReporte As Integer, ByVal posicion As Decimal)
        For Each row As DataRow In dt.Rows
            If row("CodigoReporte") = CodigoReporte Then
                row("Posicion") = posicion
                Exit For
            End If
        Next
    End Sub

    Public Shared Function VerificaExcesoLimites(ByVal usuario As String, ByVal tipoRenta As String, Optional ByVal decNProceso As Decimal = 0) As String
        Dim Variable As String = "TmpCodigoUsuario,TmpEscenario,TmpNProceso"
        Dim parametros As String = usuario + "," + ParametrosSIT.VALIDACION_PREVOI + "," + decNProceso.ToString
        Dim objBM As New LimiteBM
        objBM.RegistrarOrdenesPreviasSeleccionadas(tipoRenta, decNProceso)
        Dim obj As New JobBM
        Dim mensaje As String = obj.EjecutarJob("DTS_SIT_VerificaExcesoLimitesEnLinea" & DateTime.Today.ToString("_yyyyMMdd") & System.DateTime.Now.ToString("_hhmmss"), "Verifica exceso de limites en linea, considerando el neteo de operaciones", Variable, parametros, "", "", ConfigurationManager.AppSettings("SERVIDORETL"))
        Return mensaje
    End Function
End Class

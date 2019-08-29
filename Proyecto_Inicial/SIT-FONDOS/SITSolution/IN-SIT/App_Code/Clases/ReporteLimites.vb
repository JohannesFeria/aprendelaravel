Imports Microsoft.VisualBasic
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports ParametrosSIT
Public Class ReporteLimites
    Inherits System.Web.UI.Page
#Region "Variables"
    Dim oLimiteBM As New LimiteBM
    Private Usuario As String
    Private DatosRequest As DataSet
    Public _CodLimite As String
    Public _CodLimiteCaracteristica As String
    Public _FechaOperacion As Date
    Public _DetalladoPorFondo As String
    Public _Escenario As String
    Public _ProcesarLimite As Integer
    Public _Portafolio As String = ""
    Public _DescPortafolio As String = ""
    Public _FolderReportes As String = ""
    Private _CodigoValorBase As String
    Public _PorcentajeCercaLimite As Decimal
#End Region
    Public Function GeneraLimite(ByVal request As DataSet, ByVal user As String) As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim DecFechaLimite As Decimal = CType(_FechaOperacion.ToString("yyyyMMdd"), Decimal)
        Dim strFechaOperacion As String = _FechaOperacion.ToShortDateString
        Dim PosicionTotalN1SUMA As Decimal = 0
        Usuario = user
        DatosRequest = request
        _PorcentajeCercaLimite = New LimiteBM().SeleccionarPorcentajeCercaLimite(_CodLimite, _CodLimiteCaracteristica)
        Dim DescValorBase As String = ""
        Dim drDatosLimite As LimiteBE.LimiteRow
        Dim dtOrigen As New DataTable
        Dim NroNiveles As Integer
        If Not Page.IsPostBack Then
            dtOrigen = New ReporteLimitesBM().ProcesarLimite(_CodLimite, _CodLimiteCaracteristica, DecFechaLimite, _ProcesarLimite, _Escenario, DatosRequest)
            If dtOrigen Is Nothing Then
                Return Nothing
            ElseIf dtOrigen.Rows.Count = 0 Then
                Return Nothing
            End If
            Dim oLimiteBE As LimiteBE = New LimiteBM().Seleccionar(_CodLimite, DatosRequest)
            If oLimiteBE.Limite.Rows.Count = 0 Then Throw New Exception("No se encontraron los datos del Límite.")
            drDatosLimite = oLimiteBE.Limite.Rows(0)
            _CodigoValorBase = drDatosLimite("valorBase")
            Dim dt As DataTable = New ParametrosGeneralesBM().SeleccionarPorFiltro("ValBase", "", _CodigoValorBase, "", DatosRequest)
            If dt.Rows.Count > 0 Then DescValorBase = dt.Rows(0).Item("Nombre").ToString
            NroNiveles = New ReporteLimitesBM().Obtener_LimiteNivelMaximo(_CodLimiteCaracteristica)
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
        If _CodLimite = "07" Then
            oReport.Load(Server.MapPath(_FolderReportes & "LimiteExteriorDetalladoDuracion.rpt"))
        Else
            If drDatosLimite.IsAgrupadoPorcentaje Then    '''' MPENAL - 15/09/16
                '''' INI MPENAL - 15/09/16
                oReport.Load(Server.MapPath(_FolderReportes & "LimiteDetallado" & IIf(NroNiveles = 1, "", NroNiveles.ToString) & ".rpt"))
                '''' FIN MPENAL - 15/09/16
            Else '''' MPENAL - 15/09/16
                oReport.Load(Server.MapPath(_FolderReportes & "LimiteExteriorDetallado" & IIf(NroNiveles = 1, "", NroNiveles.ToString) & ".rpt"))
            End If
        End If
        oReport.SetDataSource(dsDestino)
        Dim patrimonioIncDec As Double = 0
        Dim totalAjustes As Double = 0
        oReport.SetParameterValue("TituloReporte1", tituloLimite)
        oReport.SetParameterValue("Fondo", _DescPortafolio)
        oReport.SetParameterValue("Escenario", _Escenario)
        oReport.SetParameterValue("ValorBase", DescValorBase)
        oReport.SetParameterValue("SubtituloAgrupacion", "")
        oReport.SetParameterValue("FechaProceso", strFechaOperacion)
        oReport.SetParameterValue("Usuario", Usuario)
        oReport.SetParameterValue("PorcentajeCercaLimite", _PorcentajeCercaLimite)
        oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO").ToString()))
        oReport.SetParameterValue("DetalladoPorFondo", _DetalladoPorFondo)
        If NroNiveles = 1 Or NroNiveles = 2 Or NroNiveles = 3 Then
            oReport.SetParameterValue("TituloReporte2", "")
            oReport.SetParameterValue("CodigoValorBase", _CodigoValorBase)
        End If
        If NroNiveles = 4 Then
            oReport.SetParameterValue("CodigoValorBase", _CodigoValorBase)
        End If
        Select Case NroNiveles
            Case 1
                oReport.SetParameterValue("TipoFactor", drDatosLimite.TipoFactor)
            Case 2
                Dim PosicionTotalN1 As Decimal
                If drDatosLimite.UnidadPosicion = "MTM" Then
                    PosicionTotalN1 = PosicionTotalN1SUMA
                End If
                oReport.SetParameterValue("TipoFactor", drDatosLimite.TipoFactor)
                Dim TotalPatrimRF As Decimal = 0
                oReport.SetParameterValue("TotalPatrimonioRentaFija", TotalPatrimRF)
                oReport.SetParameterValue("PosicionTotalN1", PosicionTotalN1)
                oReport.SetParameterValue("MarketShare", drDatosLimite.MarketShare)
                oReport.SetParameterValue("DetalladoPorFondo", _DetalladoPorFondo)
                oReport.SetParameterValue("UnidadPosicion", drDatosLimite.UnidadPosicion)
            Case 3
                oReport.SetParameterValue("SaldoBancos", 0)
                oReport.SetParameterValue("DescSaldoBancos", "")
                oReport.SetParameterValue("CondicionSaldoBanco", 0)
                oReport.SetParameterValue("TipoFactor", drDatosLimite.TipoFactor)
            Case 4
                Dim saldoBanco As Decimal = 0
                saldoBanco = 0
                oReport.SetParameterValue("SaldoBancosExterior", saldoBanco)
        End Select
        oReport.SetParameterValue("PatrimonioIncrementoDecremento", patrimonioIncDec)
        oReport.SetParameterValue("TotalAjustes", totalAjustes)
        Return oReport
    End Function
#Region "Metodos y Funciones"
    Private Function CargarDataSetReporte(ByVal ultimoNivel As Integer, ByVal dtOrigen As DataTable, ByVal CodigoLimite As String) As DsReporteLimites
        Dim dsDestino As New DsReporteLimites
        Dim dtDestino As DsReporteLimites.ReporteLimitesDataTable = dsDestino.Tables(0)
        Dim drDestino As DataRow
        Dim drNivel1, drNivel2, drNivel3 As DataRow
        Dim nivelActual As Integer
        For Each drActual As DataRow In dtOrigen.Rows
            nivelActual = drActual("Secuencial") '
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
                    Case 2
                        SetValoresDeNivel(1, drNivel1, drDestino)
                        drDestino("ValorEfectivoColocado") = drActual("ValorEfectivoColocado")
                        drDestino("FloatOficioMultiple") = drActual("FloatOficioMultiple")
                        drDestino("UnidadesEmitidas") = drActual("UnidadesEmitidas")
                        drDestino("Factor") = drActual("Factor")
                        drDestino("TotalActivo") = drActual("TotalActivo")
                        drDestino("TotalPasivo") = drActual("TotalPasivo")
                        drDestino("Patrimonio") = drActual("Patrimonio")
                        drDestino("ValorBase") = drActual("ValorBase")
                    Case 3
                        SetValoresDeNivel(1, drNivel1, drDestino)
                        SetValoresDeNivel(2, drNivel2, drDestino)
                        drDestino("ValorEfectivoColocado") = drActual("ValorEfectivoColocado")
                        drDestino("Patrimonio") = drActual("Patrimonio")
                        drDestino("FloatOficioMultiple") = drActual("FloatOficioMultiple")
                        drDestino("Factor") = drActual("Factor")
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
        'drDestino("ParticipacionN" & nivel.ToString) = drNivel("Participacion")
        drDestino("MargenN" & nivel.ToString) = drNivel("Margen")
        drDestino("AlertaN" & nivel.ToString) = drNivel("Alerta")
        drDestino("numerico" & nivel.ToString) = drNivel("ValorPorcentaje")
        drDestino("FactorN" & nivel.ToString) = drNivel("Factor")
        drDestino("NivelSaldoBancoN" & nivel.ToString) = drNivel("NivelSaldoBanco")   'HDG OT 60574 20100913
        If nivel = 1 Then
            drDestino("numeric1M") = IIf(drNivel("ValorPorcentajeM") Is DBNull.Value, 0, drNivel("ValorPorcentajeM"))
        End If
    End Sub
    Public Sub SetValoresComunesDeLosNiveles(ByVal drOrigen As DataRow, ByVal drdestino As DataRow)
        drdestino("PorcentajeLimite") = drOrigen("ValorPorcentaje")
        drdestino("PorcentajeLimiteM") = drOrigen("ValorPorcentajeM")
        drdestino("Descripcion") = drOrigen("DescripcionNivel").Replace("*", " (*) ")
        drdestino("Posicion") = drOrigen("Posicion")
        drdestino("Participacion") = drOrigen("Participacion")
        drdestino("Margen") = drOrigen("Margen")
        drdestino("Alerta") = drOrigen("Alerta")
        drdestino("Tope") = drOrigen("Tope")
        drdestino("SimboloLimite") = drOrigen("Simbolo")
        drdestino("TipoLimite") = drOrigen("Tipo")
        drdestino("TotalInversion") = drOrigen("TotalInversion")
        drdestino("Bancos") = drOrigen("Bancos")
        drdestino("CuentasPorPagar") = drOrigen("CuentasPorPagar")
        drdestino("CuentasPorCobrar") = drOrigen("CuentasPorCobrar")
        drdestino("DescPortafolio") = drOrigen("DescPortafolio")
        drdestino("PosicionPortafolio") = drOrigen("PosicionPortafolio")
        drdestino("Valor1") = drOrigen("Valor1")
        drdestino("Valor2") = drOrigen("Valor2")
        drdestino("Valor3") = drOrigen("Valor3")
    End Sub
    Private Function CrearSubReporte_PatrimonioIncrementoDecremento(ByVal oReport As CrystalDecisions.CrystalReports.Engine.ReportDocument, ByVal Escenario As String, ByVal Portafolio As String, ByVal FechaOperacion As Decimal) As Double
        Dim TotalIncrementoDecremento As Decimal = 0
        If _CodigoValorBase = "PATRIM" And Escenario = "ESTIMADO" Then
            Dim obm As New LimiteBM
            Dim dsPatrimonioIncDec As New DsPatrimonioIncDec
            Dim drPatrimonioInDec As DataRow
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
    Private Function CrearSubReporte_AjusteLimitesEstimados(ByVal oReport As CrystalDecisions.CrystalReports.Engine.ReportDocument, ByVal fechaOperacion As Decimal, ByVal codigoPortafolio As String, ByVal codigoLimite As String) As Decimal
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
        If _Escenario = "REAL" Then
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
    Private Function OrdenarTabla(ByVal dt As DataTable, ByVal filtro As String, ByVal orderby As String) As DataTable
        Dim rows As DataRow()
        Dim dtNew As DataTable = dt.Clone() ' Copiamos la estructura del DataTable
        rows = dt.Select(filtro, orderby) 'Filtramos y ordenamos
        For Each dr As DataRow In rows ' fill dtNew with selected rows
            dtNew.ImportRow(dr)
        Next
        Return dtNew
    End Function
    Private Sub EliminarFilasDelBCRP(ByVal dt As DataTable)
        'Eliminamos todas las filas tales que ValorNivel = BCRP
        For Each dr As DataRow In dt.Select("ValorNivel = 'BCRP'")
            dt.Rows.Remove(dr)
        Next
    End Sub
    Private Sub SetPosicion(ByVal dt As DataTable, ByVal CodigoReporte As Integer, ByVal posicion As Decimal)
        For Each row As DataRow In dt.Rows
            If row("CodigoReporte") = CodigoReporte Then
                row("Posicion") = posicion
                Exit For
            End If
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
#End Region
End Class
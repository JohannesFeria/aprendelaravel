Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class ReporteLimitesBM
    'Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function SeleccionarPorEmisionAccionesFloat(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarPorEmisionAccionesFloat(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarEnElExterior(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarEnElExterior(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarPorEmisionAccionesFactorLiquidez(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarPorEmisionAccionesFactorLiquidez(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorEmisorDeudaFactorRiesgo(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarPorEmisorDeudaFactorRiesgo(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarValorContablePatrimonio(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarValorContablePatrimonio(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPasivoValorContable(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarPasivoValorContable(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarValorContableActivo(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarValorContableActivo(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarTrading(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarTrading(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarBVL(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarBVL(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorEmisionSerieAcciones(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarPorEmisionSerieAcciones(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorEmisorDeuda(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarPorEmisorDeuda(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorEmisionSerieRentaFija(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarPorEmisionSerieRentaFija(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorExtranjeroRespectoAlInstrumento(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarPorExtranjeroRespectoAlInstrumento(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarActivosTitulizados(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarActivosTitulizados(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarEstructurados(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarEstructurados(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarSociedadAdministradoraFondos(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarSociedadAdministradoraFondos(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarInversiones(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarInversiones(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarIndividuales(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarIndividuales(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarGlobales(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarGlobales(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarGrupoEconomico(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarGrupoEconomico(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorEmisorAcciones(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarPorEmisorAcciones(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorEmisor(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarPorEmisor(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorInstrumentoGestor(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Try
            Dim oReportesLimitesBE As ReportesLimitesBE
            oReportesLimitesBE = New ReportesLimitesDAM().SeleccionarPorInstrumentoGestor(param1, dataRequest)
            Return oReportesLimitesBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#Region "/* Reporte Limites*/"
    Public Function Nivel4(ByVal CodigoLimite As String, _
    ByVal CodigoLimiteCaracteristica As String, ByVal FechaLimite As Decimal, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oLimite As DataSet
            oLimite = New ReportesLimitesDAM().Nivel4(CodigoLimite, CodigoLimiteCaracteristica, FechaLimite, dataRequest)
            Return oLimite
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    '-------------------------- Reporte Limites ---------------------------
    'RGF 20081031 se agrego el parametro "escenario" (REAL o ESTIMADO)
    Public Function Seleccionar_ReporteLimites(ByVal codLimite As String, ByVal codLimiteCaracteristica As String, _
        ByVal fecha As Decimal, ByVal procesar As Decimal, ByVal escenario As String, ByVal dataRequest As DataSet) As DataSet
        Dim ds As DataSet
        Try
            ds = New ReportesLimitesDAM().Seleccionar_ReporteLimites(codLimite, codLimiteCaracteristica, fecha, procesar, escenario)
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function

    Public Function Seleccionar_ReporteLimite_Trading_Diario(ByVal fechaLimite As Decimal, ByVal fondo As String, ByRef montoTotal3Fondos As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim ds As DataSet
        Try
            ds = New ReportesLimitesDAM().Seleccionar_ReporteLimite_Trading_Diario(fechaLimite, fondo, montoTotal3Fondos)
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function

    Public Function Seleccionar_ReporteLimite_Trading_Mensual(ByVal fechaLimite As Decimal, ByVal fondo As String, ByRef montoTotal3Fondos As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim ds As DataSet
        Try
            ds = New ReportesLimitesDAM().Seleccionar_ReporteLimite_Trading_Mensual(fechaLimite, fondo, montoTotal3Fondos)
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function

    'RGF 20080711
    Public Function Seleccionar_ReporteLimite_BVL(ByVal fechaLimite As Decimal, ByVal fondo As String, ByVal dataRequest As DataSet) As DataSet
        Dim ds As DataSet
        Try
            ds = New ReportesLimitesDAM().Seleccionar_ReporteLimite_BVL(fechaLimite, fondo)
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function

    Public Function Obtener_LimiteExterior_SaldoBancos(ByVal fechaLimite As Decimal, ByVal escenario As String, ByVal dataRequest As DataSet) As Decimal
        Dim saldo As Decimal
        Try
            saldo = New ReportesLimitesDAM().Obtener_LimiteExterior_SaldoBancos(fechaLimite, escenario) 'HDG INC 59836	20100617
        Catch ex As Exception
            Throw ex
        End Try
        Return saldo
    End Function

    Public Function ValidarSaldoBancos(ByVal Fecha As Decimal, ByVal Portafolio As String, ByVal Banco As String, ByVal Escenario As String, ByVal dataRequest As DataSet) As Boolean
        Dim saldo As Decimal
        Try
            Return New ReportesLimitesDAM().ValidarSaldoBancos(Fecha, Portafolio, Banco, Escenario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidarGeneraTodoFondos(ByVal fecha As Decimal, ByVal Escenario As String, ByVal Portafolio As String) As Boolean
        Try
            Dim oReportesLimitesDAM As New ReportesLimitesDAM
            Return oReportesLimitesDAM.ValidarGeneraTodoFondos(fecha, Escenario, Portafolio) 'RGF 20101111
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListaEmailUsuarioNotifica() As DataTable
        Try
            Return New ReportesLimitesDAM().ListaEmailUsuarioNotifica()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Seleccionar_ReporteLimite_Moneda(ByVal codlimite As String, ByVal fechaLimite As Decimal, ByVal fondo As String, ByVal escenario As String, ByVal procesar As Integer) As DataSet   'HDG INC 62882	20110406
        Dim ds As DataSet
        Try
            ds = New ReportesLimitesDAM().Seleccionar_ReporteLimite_Moneda(codlimite, fechaLimite, fondo, escenario, procesar)   'HDG INC 62882	20110406
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function

    'HDG INC 61852	20101207
    Public Function Obtener_LimiteNivelMaximo(ByVal CodigoLimiteCaracteristica As String) As Decimal
        Dim nivelMaximo As Decimal
        Try
            nivelMaximo = New ReportesLimitesDAM().Obtener_LimiteNivelMaximo(CodigoLimiteCaracteristica)
        Catch ex As Exception
            Throw ex
        End Try
        Return nivelMaximo
    End Function

    'RGF 20080901
    Public Function Seleccionar_ConsolidadoLimitesExcedidos(ByVal fecha As Decimal, ByVal portafolio As String, ByVal escenario As String, ByVal dataRequest As DataSet) As DataSet
        Dim ds As DataSet
        Try
            ds = New ReportesLimitesDAM().Seleccionar_ConsolidadoLimitesExcedidos(fecha, portafolio, escenario)
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function

    'RGF 20080905
    Public Function SeleccionarLimitesPorPortafolio(ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim ds As DataSet
        Try
            ds = New ReportesLimitesDAM().SeleccionarLimitesPorPortafolio(portafolio)
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function

    Public Function ObtenerUltimaFecha_ReporteLimite(ByVal limite As String, ByVal portafolio As String, ByVal fecha As Decimal, ByVal escenario As String, ByVal codmnemonico As String, ByVal dataRequest As DataSet)
        Dim ds As DataSet
        Try
            ds = New ReportesLimitesDAM().ObtenerUltimaFecha_ReporteLimite(limite, portafolio, fecha, escenario, codmnemonico)
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function

    Public Function SeleccionarLimitesPorForward(ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim ds As DataSet
        Try
            ds = New ReportesLimitesDAM().SeleccionarLimitesPorForward(portafolio)
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function
    Public Function Seleccionar_ReporteLimitesPorInstrumento(ByVal portafolio As String, ByVal fecha As Decimal, ByVal escenario As String, ByVal valorNivel As String, ByVal dataRequest As DataSet) As DataSet
        Dim ds As DataSet
        Try
            ds = New ReportesLimitesDAM().Seleccionar_ReporteLimitesPorInstrumento(portafolio, fecha, escenario, valorNivel)
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function

    Public Function ValidarInstrumentosParametrizados(ByVal fecha As Decimal, ByVal escenario As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim ds As DataSet
        Try
            ds = New ReportesLimitesDAM().ValidarInstrumentosParametrizados(fecha, escenario, portafolio)
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function

    Public Function LimiteValoresInicializa(ByVal Portafolio As String) As Boolean
        Try
            Dim oReportesLimitesDAM As New ReportesLimitesDAM
            Return oReportesLimitesDAM.LimiteValoresInicializa(Portafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
    Public Function TieneCalculoExistente(ByVal codLimite As String, ByVal codLimiteCaracteristica As String, ByVal fecha As Decimal, ByVal escenario As String, ByVal dataRequest As DataSet) As Boolean
        Try
            Return New ReportesLimitesDAM().TieneCalculoExistente(codLimite, codLimiteCaracteristica, fecha, escenario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ProcesarLimite(ByVal codLimite As String, ByVal codLimiteCaracteristica As String, ByVal fecha As Decimal, ByVal procesar As Decimal, ByVal escenario As String, ByVal dataRequest As DataSet) As DataTable
        Try
            Return New ReportesLimitesDAM().ProcesarLimite(codLimite, codLimiteCaracteristica, fecha, escenario, procesar)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TotalSaldoBancos(ByVal fecha As Decimal, ByVal portafolio As String, ByVal escenario As String, ByVal dataRequest As DataSet) As Decimal
        Try
            Return New ReportesLimitesDAM().TotalSaldoBancos(fecha, portafolio, escenario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function ' fin OT 62839 20110408

    Public Function TotalPatrimonioRentaFija(ByVal fecha As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As Decimal
        Try
            Return New ReportesLimitesDAM().TotalPatrimonioRentaFija(fecha, portafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidarValorizacion(ByVal fecha As Decimal, ByVal Portafolio As String, ByVal Escenario As String) As Boolean
        Try
            Dim oReportesLimitesDAM As New ReportesLimitesDAM
            Return oReportesLimitesDAM.ValidarValorizacion(fecha, Portafolio, Escenario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerReporteLimite_PorPortafolio(ByVal p_CodigoPortafolio As String, ByVal p_FechaDecimal As Integer, ByVal p_FechaString As String, ByVal p_Mandato As String) As DataTable
        Try
            Return New ReportesLimitesDAM().ObtenerReporteLimite_PorPortafolio(p_CodigoPortafolio, p_FechaDecimal, p_FechaString, p_Mandato)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class

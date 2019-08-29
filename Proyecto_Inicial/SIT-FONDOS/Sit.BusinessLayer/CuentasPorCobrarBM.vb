Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class CuentasPorCobrarBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Function Insertar(ByVal dsCuentasPorPagar As CuentasPorCobrarPagarBE, ByVal egreso As String, ByVal dataRequest As DataSet)
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentasPorPagar, dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            daCuentasPorCobrar.Insertar(dsCuentasPorPagar, egreso, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Sub Anular(ByVal CodigoCuenta As String, ByVal CodigoPortafolio As String, ByVal dataRequest As DataSet)
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            daCuentasPorCobrar.Anular(CodigoCuenta, CodigoPortafolio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function Extornar(ByVal CodigoCuenta As String, ByVal CodigoPortafolio As String, ByVal dataRequest As DataSet)
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoCuenta, CodigoPortafolio, dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            daCuentasPorCobrar.Extornar(CodigoCuenta, CodigoPortafolio, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Sub ModificarFechaVencimiento(ByVal CodigoCuenta As String, ByVal CodigoPortafolio As String, ByVal fechaVencimiento As Decimal, ByVal dataRequest As DataSet)
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            daCuentasPorCobrar.ModificarFechaVencimiento(CodigoCuenta, CodigoPortafolio, fechaVencimiento, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ObtenerNumeroCartaPortafolio(ByVal sPortafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {sPortafolio, dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            Dim dsCuentas As DataSet = daCuentasPorCobrar.ObtenerNumeroCartaPortafolio(sPortafolio)
            Return dsCuentas
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function LiquidarNPH(ByVal dsOperacionCaja As OperacionCajaBE, ByVal sNumeroCarta As String, ByVal dataRequest As DataSet, ByVal bancoOrigen As String, ByVal bancoDestino As String, ByVal numeroCuentaDestino As String)
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsOperacionCaja, dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            daCuentasPorCobrar.LiquidarNPH(dsOperacionCaja, sNumeroCarta, dataRequest, bancoOrigen, bancoDestino, numeroCuentaDestino)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function Liquidar(ByVal dsOperacionCaja As OperacionCajaBE, ByVal sNumeroCarta As String, ByVal sCodigoContacto As String, ByVal dataRequest As DataSet, _
    ByVal bancoOrigen As String, ByVal bancoDestino As String, ByVal numeroCuentaDestino As String, ByVal Agrupado As String, ByVal ObservacionCarta As String, ByVal CodigoRelacion As String)
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            daCuentasPorCobrar.Liquidar(dsOperacionCaja, sNumeroCarta, sCodigoContacto, dataRequest, bancoOrigen, bancoDestino, numeroCuentaDestino, Agrupado, _
            ObservacionCarta, CodigoRelacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LiquidarDivisas(ByVal dsOperacionCaja As OperacionCajaBE, ByVal sNumeroCarta As String, ByVal sCodigoContacto As String, ByVal Agrupado As String, ByVal dataRequest As DataSet)
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            daCuentasPorCobrar.LiquidarDivisas(dsOperacionCaja, sNumeroCarta, sCodigoContacto, Agrupado, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LiquidarDivisasNPH(ByVal dsOperacionCaja As OperacionCajaBE, ByVal sNumeroCarta As String, ByVal dataRequest As DataSet)
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsOperacionCaja, dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            daCuentasPorCobrar.LiquidarDivisasNPH(dsOperacionCaja, sNumeroCarta, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function IngresarPagoParcial(ByVal codigoCuenta As String, ByVal numeroCuenta As String, ByVal codigoPortafolio As String, ByVal importe As Decimal, ByVal dataRequest As DataSet)
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCuenta, numeroCuenta, codigoPortafolio, importe, dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            daCuentasPorCobrar.IngresarPagoParcial(codigoCuenta, numeroCuenta, codigoPortafolio, importe, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarAnularPorFiltro(ByVal dsCuentasPorCobrar As CuentasPorCobrarPagarBE, ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal dataRequest As DataSet, Optional ByVal LiquidaFechaAnt As String = "") As DataSet 'HDG OT 64767 20120222
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            Dim dsCuentas As DataSet = daCuentasPorCobrar.SeleccionarAnularPorFiltro(dsCuentasPorCobrar, fechaIni, fechaFin, LiquidaFechaAnt)
            Return dsCuentas
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarPorFiltro(ByVal dsCuentasPorCobrar As CuentasPorCobrarPagarBE, ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal dataRequest As DataSet, Optional ByVal LiquidaFechaAnt As String = "") As DataSet 'HDG OT 64767 20120222
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            Dim dsCuentas As DataSet = daCuentasPorCobrar.SeleccionarPorFiltro(dsCuentasPorCobrar, fechaIni, fechaFin, LiquidaFechaAnt) 'HDG OT 64767 20120222
            Return dsCuentas
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarVencimientos(ByVal dsCuentasPorCobrar As CuentasPorCobrarPagarBE, ByVal codigoClaseInstrumento As String, ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentasPorCobrar, codigoClaseInstrumento, fechaIni, fechaFin, dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            Dim dsCuentas As DataSet = daCuentasPorCobrar.SeleccionarVencimientos(dsCuentasPorCobrar, codigoClaseInstrumento, fechaIni, fechaFin)
            RegistrarAuditora(parameters)
            Return dsCuentas
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarVencimientos2(ByVal dsCuentasPorCobrar As CuentasPorCobrarPagarBE, ByVal codigoClaseInstrumento As String, ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentasPorCobrar, codigoClaseInstrumento, fechaIni, fechaFin, dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            Dim dsCuentas As DataSet = daCuentasPorCobrar.SeleccionarVencimientos2(dsCuentasPorCobrar, codigoClaseInstrumento, fechaIni, fechaFin)
            RegistrarAuditora(parameters)
            Return dsCuentas
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function Limite1(ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim dsCuentas As DataSet
        Dim parameters As Object() = {dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            dsCuentas = daCuentasPorCobrar.Limite1()
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsCuentas
    End Function
    Public Function OperacionesNoContabilizadas(ByVal fechaProceso As Decimal, ByVal fondo As String, ByVal egreso As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim dsOperacionesNoContabilizadas As DataSet
        Dim parameters As Object() = {dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            dsOperacionesNoContabilizadas = daCuentasPorCobrar.OperacionesNoContabilizadas(fechaProceso, fondo, egreso)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsOperacionesNoContabilizadas
    End Function
    Public Function ValidaLotesCuadradosParaCierre(ByVal fechaProceso As Decimal, ByVal fondo As String, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim lote As String
        Dim parameters As Object() = {dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            lote = daCuentasPorCobrar.ValidaLotesCuadradosParaCierre(fechaProceso, fondo)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return lote
    End Function
    Public Function ObtenerEstadoOperacion(ByVal codigoOrden As String) As String
        Dim estado As String
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            estado = daCuentasPorCobrar.ObtenerEstadoOperacion(codigoOrden)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return estado
    End Function
    Public Sub ActualizaCuentaMatriz(CodigoOperacionCaja As String, ByVal CodigoPortafolioSBS As String)
        Dim estado As String
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            daCuentasPorCobrar.ActualizaCuentaMatriz(CodigoOperacionCaja, CodigoPortafolioSBS)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se implementa proceso de automatización para suscripción de fondos CAPESTRen T + 1 | 01/08/18 
    Public Function SuscripcionFondos_Automatico(ByVal dataRequest As DataSet, ByVal codigoPortafolioSBS As String, ByVal codigoMoneda As String, ByVal numeroCuenta As String, ByVal importe As Decimal, ByVal codigoTerceroOrigen As String, ByVal codigoClaseCuenta As String, ByVal fechaOperacion As Decimal) As Integer
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim rpta As Integer
        Dim parameters As Object() = {dataRequest}
        Try
            Dim daCuentasPorCobrar As New CuentasPorCobrarDAM
            rpta = daCuentasPorCobrar.SuscripcionFondos_Automatico(codigoPortafolioSBS, codigoMoneda, numeroCuenta, importe, codigoTerceroOrigen, codigoClaseCuenta, fechaOperacion)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return rpta
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se implementa proceso de automatización para suscripción de fondos CAPESTRen T + 1 | 01/08/18 
End Class
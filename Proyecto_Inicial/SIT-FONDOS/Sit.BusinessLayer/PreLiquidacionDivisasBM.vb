Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class PreLiquidacionDivisasBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function ListarPreLiquidacionDivisasImprimir(ByVal strCodigoOrden As String, ByVal strCodModeloCarta As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoOrden, dataRequest}
        Try
            Dim daPreLiquid As New PreLiquidacionDivisasDAM
            Dim dsCuentas As DataSet = daPreLiquid.ListarPreLiquidacionDivisasImprimir(strCodigoOrden, strCodModeloCarta)
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

    Public Function SeleccionarPorFiltro(ByVal dsCuentasPorCobrar As CuentasPorCobrarPagarBE, ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentasPorCobrar, fechaIni, fechaFin, dataRequest}
        Try
            Dim daPreLiquid As New PreLiquidacionDivisasDAM
            Dim dsCuentas As DataSet = daPreLiquid.SeleccionarPorFiltro(dsCuentasPorCobrar, fechaIni, fechaFin)
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

    Public Function SeleccionarPorCodigoOrden(ByVal strCodigoOrden As String, ByVal dataRequest As DataSet) As DatosOrdenInversionBE
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoOrden, dataRequest}
        Dim dsDatos As DatosOrdenInversionBE
        Try
            Dim daPreLiquid As New PreLiquidacionDivisasDAM
            dsDatos = daPreLiquid.SeleccionarPorCodigoOrden(strCodigoOrden)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsDatos
    End Function

    Public Function Insertar(ByVal dsDatos As DatosOrdenInversionBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsDatos, dataRequest}
        Dim blnRespuesta As Boolean = False
        Try
            Dim daPreLiquid As New PreLiquidacionDivisasDAM
            blnRespuesta = daPreLiquid.Insertar(dsDatos, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return blnRespuesta
    End Function

    Public Function Modificar(ByVal dsDatos As DatosOrdenInversionBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsDatos, dataRequest}
        Dim blnRespuesta As Boolean = False
        Try
            Dim daPreLiquid As New PreLiquidacionDivisasDAM
            blnRespuesta = daPreLiquid.Modificar(dsDatos, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return blnRespuesta
    End Function
End Class

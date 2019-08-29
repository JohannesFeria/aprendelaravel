Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class ReporteCalculoInteresBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function CalculoInteres(ByVal FechaInicial As Decimal, ByVal FechaFinal As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {dataRequest}

        Try
            Dim objReporteInteres As New ReporteCalculoInteresDAM
            oreporte = objReporteInteres.CalculoInteres(FechaInicial, FechaFinal)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function

    Public Function CertificadoDeposito(ByVal FechaInicial As Decimal, ByVal FechaFinal As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {dataRequest}

        Try
            Dim objReporteInteres As New ReporteCalculoInteresDAM
            oreporte = objReporteInteres.CertificadoDeposito(FechaInicial, FechaFinal)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function

    Public Function FondosMutuos(ByVal FechaInicial As Decimal, ByVal FechaFinal As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {dataRequest}

        Try
            Dim objReporteInteres As New ReporteCalculoInteresDAM
            oreporte = objReporteInteres.FondosMutuos(FechaInicial, FechaFinal)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function

    Public Function ValorCuota(ByVal Fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {dataRequest}

        Try
            Dim objReporteInteres As New ReporteCalculoInteresDAM
            oreporte = objReporteInteres.ValorCuota(Fecha)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function

End Class

Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class ReporteGipsaBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function ReporteGIPSA(ByVal Fecha As Decimal, ByVal Portafolios As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {dataRequest}

        Try
            Dim objReporteGipsa As New ReporteGipsaDAM
            oreporte = objReporteGipsa.ReporteGipsa(Fecha, Portafolios)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function

    Public Function ReporteGIPSAEmisor(ByVal Fecha As Decimal, ByVal Portafolios As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {dataRequest}

        Try
            Dim objReporteGipsa As New ReporteGipsaDAM
            oreporte = objReporteGipsa.ReporteGIPSAEmisor(Fecha, Portafolios)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function

    Public Function TablaReporteGipsa(ByVal Fecha As Decimal, ByVal Portafolios As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {dataRequest}

        Try
            Dim objReporteGipsa As New ReporteGipsaDAM
            oreporte = objReporteGipsa.TablaReporteGipsa(Fecha, Portafolios)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function


    Public Function EncajeReporteGipsa(ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {dataRequest}

        Try
            Dim objReporteGipsa As New ReporteGipsaDAM
            oreporte = objReporteGipsa.EncajeReporteGipsa()

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

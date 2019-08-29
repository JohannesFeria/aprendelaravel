Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class ReportePromedioTasasBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function PromedioTasas(ByVal FechaInicial As Decimal, ByVal FechaFinal As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {dataRequest}

        Try
            Dim objReportePromedioTasas As New ReportePromedioTasasDAM
            oreporte = objReportePromedioTasas.PromedioTasas(FechaInicial, FechaFinal)

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

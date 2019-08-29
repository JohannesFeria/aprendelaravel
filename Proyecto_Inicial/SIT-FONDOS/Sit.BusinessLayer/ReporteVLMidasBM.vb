Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports MotorTransaccionesProxy

Public Class ReporteVLMidasBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub

    Public Function Insertar(ByVal eReporteVLMidas As ReporteVLMidasBE, ByVal dataRequest As DataSet) As String
        Dim resultado As String = String.Empty
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {eReporteVLMidas, dataRequest}
        Try
            Dim ReporteVLMidasDAM As New ReporteVLMidasDAM
            resultado = ReporteVLMidasDAM.Insertar(eReporteVLMidas, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resultado
    End Function

    Public Function Eliminar(ByVal eReporteVLMidas As ReporteVLMidasBE, ByVal dataRequest As DataSet) As String
        Dim resultado As String = String.Empty
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {eReporteVLMidas, dataRequest}
        Try
            Dim ReporteVLMidasDAM As New ReporteVLMidasDAM
            resultado = ReporteVLMidasDAM.Elminar(eReporteVLMidas, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resultado
    End Function

    Public Function CompararVL(ByVal eReporteVLMidas As ReporteVLMidasBE, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim ReporteVLMidasDAM As New ReporteVLMidasDAM
            Return ReporteVLMidasDAM.CompararVL(eReporteVLMidas)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidarVL(ByVal eReporteVLMidas As ReporteVLMidasBE, ByVal dataRequest As DataSet) As String
        Try
            Dim ReporteVLMidasDAM As New ReporteVLMidasDAM
            Return ReporteVLMidasDAM.ValidarVL(eReporteVLMidas)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class

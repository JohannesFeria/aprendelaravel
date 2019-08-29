Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class ControlForwardBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function ControlForward(ByVal FechaInicial As Decimal, ByVal Portafolios As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {dataRequest}

        Try
            Dim objControlForward As New ControlForwardDAM
            oreporte = objControlForward.ControlForward(FechaInicial, Portafolios)

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

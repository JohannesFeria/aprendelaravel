Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class CustodioKardexBM
    Inherits InvokerCOM

    Public Function Insertar(ByVal oCustodioKardex As CustodioKardexBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCustodioKardex, dataRequest}
        Try
            Dim oCustodioKardexDAM As New CustodioKardexDAM
            oCustodioKardexDAM.Insertar(oCustodioKardex, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

End Class

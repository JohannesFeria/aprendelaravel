'JCH OT 66056 Clase para el acceso de los datos para OperacionFuturo tabla.
Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
Public Class OperacionFuturoBM
    Inherits InvokerCOM

    Public Function SeleccionarPorFechaAnterior(ByVal fechaOperacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fechaOperacion, dataRequest}
        Dim oOperacionCaja As New DataSet
        Dim operaciones As New OperacionFuturoDAM
        Try
            oOperacionCaja = operaciones.SeleccionarPorFechaAnterior(fechaOperacion, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oOperacionCaja
    End Function

End Class

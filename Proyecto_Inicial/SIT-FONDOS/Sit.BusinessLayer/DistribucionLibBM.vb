Imports System
Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

''' <summary>
''' Clase de negocio para tabla DistribucionLib.
''' 'OT10927 - 21/11/2017 - Hanz Cocchi. Insertar distribución de cuotas liberadas
''' </summary>

Public Class DistribucionLibBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oDis As DistribucionLibBE, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oDistribucionLibDAM As New DistribucionLibDAM
            Return oDistribucionLibDAM.Insertar(oDis, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'OT10989 - 12/12/2017 - Hanz Cocchi. Eliminar rentabilidad distribucción
    Public Function Eliminar(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal) As Boolean
        Try
            Dim oDistribucionLibDAM As New DistribucionLibDAM
            Return oDistribucionLibDAM.Eliminar(CodigoPortafolioSBS, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class

Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class VectorForwardSBSBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    'OT10709 Eliminar duplicidad de código
    Public Function Insertar(ByVal objVectorForwardSBSBE As VectorForwardSBSBE.VectorForwardSBSRow, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim daVectorForwardSBS As New VectorForwardSBSDAM
            daVectorForwardSBS.Insertar(objVectorForwardSBSBE, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CargarPrecioFWD(ByVal decFecha As Decimal) As DataSet
        Try
            Dim oVectorForwardSBS As New VectorForwardSBSDAM
            Return oVectorForwardSBS.CargarPrecioFWD(decFecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function EliminatmpVectorForwardSBS() As Boolean
        Try
            Dim oVectorForwardSBS As New VectorForwardSBSDAM
            oVectorForwardSBS.EliminatmpVectorForwardSBS()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class VectorPrecioSBSBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Sub delVectorPrecioPIP(ByVal Fecha As Decimal, ByVal Manual As String, ByVal Escenario As String)
        Try
            Dim daVectorPrecioSBS As New VectorPrecioSBSDAM
            daVectorPrecioSBS.delVectorPrecioPIP(Fecha, Manual, Escenario)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub VectorPrecioPIP(ByVal objVectorPrecioPIP As VectorPrecioPIP.VectorPrecioPIPRow, ByVal dataRequest As DataSet)
        Try
            Dim daVectorPrecioSBS As New VectorPrecioSBSDAM
            daVectorPrecioSBS.VectorPrecioPIP(objVectorPrecioPIP, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Update_RatingValores(p_Rating As String, p_CodigoIsin As String)
        Try
            Dim daVectorPrecioSBS As New VectorPrecioSBSDAM
            daVectorPrecioSBS.Update_RatingValores(p_Rating, p_CodigoIsin)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function Insertar(ByVal objVectorPrecioSBS As VectorPrecioSBSBE.VectorPrecioSBSRow, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        Try
            Dim daVectorPrecioSBS As New VectorPrecioSBSDAM
            strCodigo = daVectorPrecioSBS.Insertar(objVectorPrecioSBS, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return strCodigo
    End Function
    Public Function EliminarPreciosSBS(ByVal FechaCarga As Date, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaCarga, dataRequest}
        Try
            Dim daVectorPrecioSBS As New VectorPrecioSBSDAM

            eliminado = daVectorPrecioSBS.EliminarPrecioSBS(FechaCarga)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function
    Public Sub InsertarVectorWarrant(CodigoIsin As String, Subyacente As Decimal, FechaProceso As Decimal, MarkToModel As Decimal,
    StrikePrice As Decimal, SpotPrice As Decimal, ByVal dataRequest As DataSet)
        Try
            Dim daVectorPrecioSBS As New VectorPrecioSBSDAM
            daVectorPrecioSBS.InsertarVectorWarrant(CodigoIsin, Subyacente, FechaProceso, MarkToModel, StrikePrice, SpotPrice, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function BorrarVectorWarrant(ByVal FechaProceso As Decimal)
        Try
            Dim daVectorPrecioSBS As New VectorPrecioSBSDAM
            daVectorPrecioSBS.BorrarVectorWarrant(FechaProceso)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
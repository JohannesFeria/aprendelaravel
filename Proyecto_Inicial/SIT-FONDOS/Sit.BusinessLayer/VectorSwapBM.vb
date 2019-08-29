Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class VectorSwapBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub

    Public Sub Insertar(ByVal objVectorSwapBE As VectorSwapBE.VectorSwapRow, ByVal dataRequest As DataSet)
        'Se solicita un c�digo de ejecuci�n al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los par�metros enviados en el mismo orden
        Dim parameters As Object() = {objVectorSwapBE, dataRequest}

        Try
            Dim daVectorSwap As New VectorSwapDAM
            daVectorSwap.Insertar(objVectorSwapBE, dataRequest)
            'Luego de terminar la ejecuci�n de m�todos(sin errores)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo m�todo pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 l�neas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
End Class

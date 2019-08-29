'Creado por: HDG OT 62087 Nro5-R10 20110118
Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class LineasCreditoBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub

    Public Function ActualizarLineasCreditoPorExcel(ByVal dtData As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim Codigo = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtData, dataRequest}

        Try
            Dim oLineasCreditoDAM As New LineasCreditoDAM
            Codigo = oLineasCreditoDAM.ActualizarLineasCreditoPorExcel(dtData, dataRequest, strmensaje)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            Dim sms As String = ex.Message
            Dim src As String = ex.Source
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function

End Class

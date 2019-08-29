'Creado por: HDG OT 62087 Nro6-R14 20110201
Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class LineasContraparteFWDBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub

    Public Function ActualizarLineasContraparteFWDPorExcel(ByVal dtData As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String, ByVal tipo As Integer) As Boolean
        Dim Codigo = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtData, dataRequest}

        Try
            Dim oLineasContraparteFWDDAM As New LineasContraparteFWDDAM
            Codigo = oLineasContraparteFWDDAM.ActualizarLineasContraparteFWDPorExcel(dtData, dataRequest, strmensaje, tipo)
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

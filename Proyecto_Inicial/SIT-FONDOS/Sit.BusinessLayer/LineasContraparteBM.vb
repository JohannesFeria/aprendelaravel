'Creado por: CMB OT 67168 20130429
Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class LineasContraparteBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub

    Public Function ActualizarLineasContrapartePorExcel(ByVal dtData As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim Codigo = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtData, dataRequest}

        Try
            Dim oLineasContraparteDAM As New LineasContraparteDAM
            Codigo = oLineasContraparteDAM.ActualizarLineasContrapartePorExcel(dtData, dataRequest, strmensaje)
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

Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class AnexoIDI8BM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Function SeleccionarPortafolioFecha(ByVal portafolioSBS As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {portafolioSBS, fecha, dataRequest}
        Dim oDSAnexoIDI8 As New DataSet
        Try
            oDSAnexoIDI8 = New AnexoIDI8DAM().SeleccionarPorPortafolioFecha(portafolioSBS, fecha, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo m�todo pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 l�neas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDSAnexoIDI8
    End Function

End Class

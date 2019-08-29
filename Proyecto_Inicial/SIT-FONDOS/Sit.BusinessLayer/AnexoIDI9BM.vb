Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class AnexoIDI9BM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Function SeleccionarPortafolioFecha(ByVal portafolioSBS As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {portafolioSBS, fecha, dataRequest}
        Dim oDSAnexoIDI9 As New DataSet
        Try
            oDSAnexoIDI9 = New AnexoIDI9DAM().SeleccionarPorPortafolioFecha(portafolioSBS, fecha, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDSAnexoIDI9
    End Function

End Class

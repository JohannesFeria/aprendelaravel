Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
Imports Sit.BusinessEntities
Public Class CorreccionDepositoPlazoBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Function ListarCorreccionDepositoPlazo(ByVal strCodigoPortafolioSBS As String, ByVal decFecha As Decimal, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoPortafolioSBS, decFecha}
        Dim oCorreccionDP As New CorreccionDepositoPlazoDAM
        Dim oTabla As DataTable
        Try
            RegistrarAuditora(parameters)
            oTabla = oCorreccionDP.ListarCorreccionDepositoPlazo(strCodigoPortafolioSBS, decFecha).Tables(0)
        Catch ex As Exception
            RegistrarAuditora(parameters)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oTabla
    End Function
End Class
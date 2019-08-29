Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class PatrimonioTerceroBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub

    Public Sub Borrar_PatrimonioTercero(ByVal FechaProceso As Decimal, ByVal tipoArchivo As String)
        Try
            Dim oPatrimonioTerceroDAM As New PatrimonioTerceroDAM
            oPatrimonioTerceroDAM.Borrar_PatrimonioTercero(FechaProceso, tipoArchivo)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Function Insertar(ByVal orowPatrimonioTercero As PatrimonioTerceroBE.PatrimonioTerceroRow, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String = ""
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {orowPatrimonioTercero, dataRequest}
        Try
            Dim oPatrimonioTerceroDAM As New PatrimonioTerceroDAM
            strCodigo = oPatrimonioTerceroDAM.Insertar(orowPatrimonioTercero, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return strCodigo
    End Function

    Function SeleccionarPorFecha(ByVal fechaProceso As Decimal, ByVal tipoArchivo As String) As PatrimonioTerceroBE
        Dim strCodigo As String = ""
        Dim parameters As Object() = {fechaProceso}
        Dim oPatrimonioTerceroBE As PatrimonioTerceroBE = Nothing
        Try
            Dim oPatrimonioTerceroDAM As New PatrimonioTerceroDAM
            oPatrimonioTerceroBE = oPatrimonioTerceroDAM.SeleccionarPorFecha(fechaProceso, tipoArchivo)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oPatrimonioTerceroBE
    End Function
End Class

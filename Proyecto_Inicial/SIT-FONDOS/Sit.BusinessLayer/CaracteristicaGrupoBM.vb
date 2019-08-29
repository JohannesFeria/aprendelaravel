Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy

Public Class CaracteristicaGrupoBM
    Inherits InvokerCOM


    Public Function ListarVistaCodigoCaracteristica(ByVal CodigoCaracteristica As String) As DataTable
        Dim parameters As Object() = {CodigoCaracteristica}
        Dim table As DataTable
        Try
            Dim objCaracteristicaGrupo As New CaracteristicaGrupoDAM()
            RegistrarAuditora(parameters)
            table = objCaracteristicaGrupo.ListarVistaCodigoCaracteristica(CodigoCaracteristica)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
        End Try

        Return table
    End Function

End Class

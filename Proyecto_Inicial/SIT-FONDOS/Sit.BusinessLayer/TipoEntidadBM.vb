Option Explicit On 
Option Strict On

Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class TipoEntidadBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Function Listar(ByVal dataRequest As DataSet) As TipoEntidadBE

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oTipoEntidadBE As TipoEntidadBE
        Dim oTipoEntidadDAM As New TipoEntidadDAM

        Try

            RegistrarAuditora(parameters)
            oTipoEntidadBE = oTipoEntidadDAM.Listar()

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If

        End Try

        Return oTipoEntidadBE

    End Function

End Class

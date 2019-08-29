Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy

Public Class FondoBM
    Public Function ListarPrecierreFondos() As DataTable
        Try
            Return New FondoDAM().ListarPrecierreFondos().Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class

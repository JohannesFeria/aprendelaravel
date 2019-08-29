Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
Imports System.Transactions

Public Class PortafolioCajaBM

    Public Function ObtenerFechaCajaOperaciones(ByVal CodigoPortafolioSBS As String, ByVal CodigoClaseCuenta As String) As DataTable
        Try
            Dim daPortafolioCajaDAM As New PortafolioCajaDAM
            Return daPortafolioCajaDAM.ObtenerFechaCajaOperaciones(CodigoPortafolioSBS, CodigoClaseCuenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Insertar(ByVal CodigoPortafolioSBS As String, ByVal CodigoClaseCuenta As String, ByVal FechaOperacion As Decimal, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim daPortafolioCajaDAM As New PortafolioCajaDAM
            Return daPortafolioCajaDAM.Insertar(CodigoPortafolioSBS, CodigoClaseCuenta, FechaOperacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function




End Class

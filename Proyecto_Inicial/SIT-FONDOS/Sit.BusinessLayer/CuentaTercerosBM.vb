Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class CuentaTercerosBM
    Inherits InvokerCOM
    'Public Function SeleccionarDetallePorFiltro(ByVal codigoPortafolio As String, ByVal codigoClaseCuenta As String, ByVal codigoTercero As String, ByVal codigoMoneda As String, ByVal codigoMercado As String, ByVal dataRequest As DataSet) As CuentaTercerosBE
    '    Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '    Dim parameters As Object() = {codigoPortafolio, codigoClaseCuenta, codigoTercero, dataRequest}
    '    Try
    '        Return New CuentaTercerosDAM().SeleccionarDetallePorFiltro(codigoPortafolio, codigoClaseCuenta, codigoTercero, codigoMoneda, codigoMercado)
    '    Catch ex As Exception
    '        RegistrarAuditora(parameters, ex)
    '        Dim rethrow As Boolean = true
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Function
    Public Function SeleccionarCuentaTerceros(ByVal strCodigoTercero As String, ByVal strLiqAutomatica As String) As DataSet
        Try
            Dim oCuentaTercerosDAM As New CuentaTercerosDAM
            Return oCuentaTercerosDAM.SeleccionarCuentaTerceros(strCodigoTercero, strLiqAutomatica)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function VerificarCuentaTerceros(ByVal strNumeroCuenta As String, ByVal strCodigoTercero As String, ByVal strCodigoPortafolioSBS As String, ByVal strBanco As String, ByVal strLiqAut As String) As Integer
        Dim LngContar As Long
        Try
            LngContar = New CuentaTercerosDAM().VerificarCuentaTerceros(strNumeroCuenta, strCodigoTercero, strCodigoPortafolioSBS, strBanco, strLiqAut)
        Catch ex As Exception
            Throw ex
        End Try
        Return LngContar
    End Function
    Public Sub Eliminar(ByVal secuencial As Integer)
        Try
            Dim CuentaTercerosDAM As New CuentaTercerosDAM
            CuentaTercerosDAM.Eliminar(secuencial)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function Seleccionar(ByVal banco As String, ByVal codigoTercero As String, ByVal codigoMoneda As String, ByVal liqAutomatica As String) As DataTable
        Try
            Dim CuentaTercerosDAM As New CuentaTercerosDAM
            Return CuentaTercerosDAM.Seleccionar(banco, codigoTercero, codigoMoneda, liqAutomatica)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Seleccionar(ByVal secuencial As Integer) As DataTable
        Try
            Dim CuentaTercerosDAM As New CuentaTercerosDAM
            Return CuentaTercerosDAM.Seleccionar(secuencial)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub Modificar(ByVal oCuentaTerceros As CuentaTercerosBE, ByVal dataRequest As DataSet)
        Try
            Dim CuentaTercerosDAM As New CuentaTercerosDAM
            CuentaTercerosDAM.Modificar(oCuentaTerceros, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class

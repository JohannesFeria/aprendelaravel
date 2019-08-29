Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class SaldoNoAdministradoBM
    Public Function InsertarRegistrosExcel(ByVal oSaldoNoAdministrado As SaldoNoAdministradoBE, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oSaldoNoAdministradoDAM As New SaldoNoAdministradoDAM
            oSaldoNoAdministradoDAM.InsertarRegistrosExcel(oSaldoNoAdministrado, dataRequest)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function

    'Public Function SeleccionarPorFiltro(ByVal CodigoSaldoNoAdministrado As String, ByVal Tercero As String, ByVal Fecha As Decimal, ByVal TerceroFinanciero As String, ByVal TipoCuenta As String, ByVal Moneda As String, ByVal TipoConsulta As String, ByVal dataRequest As DataSet) As DataSet
    Public Function SeleccionarPorFiltro(ByVal CodigoSaldoNoAdministrado As String, ByVal Mandato As String, ByVal Fecha As Decimal, ByVal TerceroFinanciero As String, ByVal TipoCuenta As String, ByVal Moneda As String, ByVal TipoConsulta As String) As DataSet
        Dim objDS As DataSet
        Try
            Dim oSaldoNoAdministradoDAM As New SaldoNoAdministradoDAM
            'objDS = oSaldoNoAdministradoDAM.SeleccionarPorFiltro(CodigoSaldoNoAdministrado, Mandato, Fecha, TerceroFinanciero, TipoCuenta, Moneda, TipoConsulta, dataRequest)
            objDS = oSaldoNoAdministradoDAM.SeleccionarPorFiltro(CodigoSaldoNoAdministrado, Mandato, Fecha, TerceroFinanciero, TipoCuenta, Moneda, TipoConsulta)

        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return objDS
    End Function

    Public Function Eliminar(ByVal CodigoSaldoNoAdministrado As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oSaldoNoAdministradoDAM As New SaldoNoAdministradoDAM
            eliminado = oSaldoNoAdministradoDAM.Eliminar(CodigoSaldoNoAdministrado, dataRequest)
        Catch ex As Exception

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function

    Public Function Insertar(ByVal oSaldoNoAdministrado As SaldoNoAdministradoBE, ByVal dataRequest As DataSet) As Boolean

        Try
            Dim oSaldoNoAdministradoDAM As New SaldoNoAdministradoDAM
            oSaldoNoAdministradoDAM.Insertar(oSaldoNoAdministrado, dataRequest)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function

    Public Function Modificar(ByVal ob As SaldoNoAdministradoBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oSaldoNoAdministradoDAM As New SaldoNoAdministradoDAM
            actualizado = oSaldoNoAdministradoDAM.Modificar(ob, dataRequest)

        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado
    End Function

    Public Function DesactivarRegistrosExcel(ByVal dataRequest As DataSet, ByRef strMensaje As String, ByVal strMes As String, ByVal strAnhio As String)
        Try
            Dim oSaldoNoAdministradoDAM As New SaldoNoAdministradoDAM
            oSaldoNoAdministradoDAM.DesactivarRegistrosExcel(dataRequest, strMensaje, strMes, strAnhio)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
End Class

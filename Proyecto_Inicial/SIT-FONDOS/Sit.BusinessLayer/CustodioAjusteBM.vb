Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class CustodioAjusteBM
    Inherits InvokerCOM

    Public Function Listar(ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal CodigoSBS As String, ByVal TipoMovimiento As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioAjusteDAM().Listar(CodigoPortafolioSBS, CodigoCustodio, CodigoSBS, TipoMovimiento)
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
    End Function

    Public Function Insertar(ByVal oCustodioAjuste As CustodioAjusteBE, ByVal oCustodioKardex As CustodioKardexBE, ByVal nFechaPortafolio As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCustodioAjuste, dataRequest}
        Dim parametersII As Object() = {oCustodioKardex, dataRequest}

        Try
            Dim oCustodioAjusteDAM As New CustodioAjusteDAM
            oCustodioAjusteDAM.Insertar(oCustodioAjuste, oCustodioKardex, nFechaPortafolio, dataRequest)
            RegistrarAuditora(parameters)
            RegistrarAuditora(parametersII)
            Return True

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            RegistrarAuditora(parametersII, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function InsertarTransferencia(ByVal oCustodioAjuste As CustodioAjusteBE, ByVal oCustodioKardex As CustodioKardexBE, ByVal nFechaPortafolio As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCustodioAjuste, dataRequest}
        Dim parametersII As Object() = {oCustodioKardex, dataRequest}

        Try
            Dim oCustodioAjusteDAM As New CustodioAjusteDAM
            oCustodioAjusteDAM.InsertarTransferencia(oCustodioAjuste, oCustodioKardex, nFechaPortafolio, dataRequest)
            RegistrarAuditora(parameters)
            RegistrarAuditora(parametersII)
            Return True

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            RegistrarAuditora(parametersII, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

End Class

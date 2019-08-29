Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
Public Class OrdenInversionDatosOperacionBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Function ObtenerDatoOperacion_PrecioCalculadoPorcentaje(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionDatosOperacionDAM().ObtenerDatoOperacion_PrecioCalculadoPorcentaje(strCodigoValor)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oCaracValor
    End Function
    Public Function ObtenerDatoOperacion_InteresCorrido(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionDatosOperacionDAM().ObtenerDatoOperacion_InteresCorrido(strCodigoValor)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oCaracValor
    End Function
    Public Function ObtenerDatoOperacion_PrecioNegociacionLimpio(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionDatosOperacionDAM().ObtenerDatoOperacion_PrecioNegociacionLimpio(strCodigoValor)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oCaracValor
    End Function
    Public Function ObtenerDatoOperacion_PrecioNegociacionSucio(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionDatosOperacionDAM().ObtenerDatoOperacion_PrecioNegociacionSucio(strCodigoValor)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oCaracValor
    End Function
    Public Function ObtenerDatoOperacion_InteresAcumulado(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionDatosOperacionDAM().ObtenerDatoOperacion_InteresAcumulado(strCodigoValor)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oCaracValor
    End Function
    Public Function ObtenerDatoOperacion_InteresCastigado(ByVal strCodigoValor As String, ByVal dataRequest As DataSet) As DataSet
        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoValor, dataRequest}
        Dim oCaracValor As DataSet
        Try
            oCaracValor = New OrdenInversionDatosOperacionDAM().ObtenerDatoOperacion_InteresCastigado(strCodigoValor)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oCaracValor
    End Function
    'JVC 20090403 VECTOR PRECIO FORWARDS
    Public Function ObtenerDatoOperacion_PorPoliza(ByVal NumeroPoliza As String) As DataTable
        Dim parameters As Object() = {NumeroPoliza}
        Dim oCaracValor As DataTable
        Try
            oCaracValor = New OrdenInversionDatosOperacionDAM().ObtenerDatoOperacion_PorPoliza(NumeroPoliza)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oCaracValor
    End Function
    'JVC 20090403 VECTOR PRECIO FORWARDS
    Public Sub OrdenInversion_ActualizarPrecioFWD(ByVal NumeroPoliza As String, ByVal PrecioForwards As Decimal, ByVal MtmDolares As Decimal, _
    ByVal MtmSoles As Decimal, ByVal PrecioVector As Decimal)
        Dim OrdInvDatOpeDAM As OrdenInversionDatosOperacionDAM
        Dim parameters As Object() = {NumeroPoliza}
        Try
            OrdInvDatOpeDAM = New OrdenInversionDatosOperacionDAM
            OrdInvDatOpeDAM.OrdenInversion_ActualizarPrecioFWD(NumeroPoliza, PrecioForwards, MtmDolares, MtmSoles, PrecioVector)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        Finally
            OrdInvDatOpeDAM = Nothing
            GC.Collect()
        End Try
    End Sub
    'OT 9908 31/01/2017 - Carlos Espejo
    'Descripcion: Devuelve tabla de Interes cobrados en un rango de fechas
    Public Function InteresesCobrados(ByVal CodigoPortafolioSBS As String, ByVal FechaInicio As Decimal, FechaTermino As Decimal) As DataTable
        Try
            Dim OrdInvDatOpeDAM As New OrdenInversionDatosOperacionDAM
            Return OrdInvDatOpeDAM.InteresesCobrados(CodigoPortafolioSBS, FechaInicio, FechaTermino)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10059 02/03/2017 - Carlos Espejo
    'Descripcion: Validar el codigo de Mercado del Emisor
    Public Function ValidarDatosConfirmacion(ByVal CodigoIsin As String) As String
        Try
            Dim OrdInvDatOpeDAM As New OrdenInversionDatosOperacionDAM
            Return OrdInvDatOpeDAM.ValidarDatosConfirmacion(CodigoIsin)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarValorTipoCarta(ByVal CodigoOrden As String, ByVal CodigoTipoDato As String) As DataSet
        Try
            Dim OrdInvDatOpeDAM As New OrdenInversionDatosOperacionDAM
            Return OrdInvDatOpeDAM.ListarValorTipoCarta(CodigoOrden, CodigoTipoDato)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarBancosConfirmacion(ByVal CodigoOrden As String) As DataSet
        Try
            Dim OrdInvDatOpeDAM As New OrdenInversionDatosOperacionDAM
            Return OrdInvDatOpeDAM.ListarBancosConfirmacion(CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarCuentasPorBancoConfirmacion(ByVal CodigoTercero As String, ByVal CodigoOrden As String) As DataSet
        Try
            Dim OrdInvDatOpeDAM As New OrdenInversionDatosOperacionDAM
            Return OrdInvDatOpeDAM.ListarCuentasPorBancoConfirmacion(CodigoTercero, CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class EncajeDetalleBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function SeleccionarPorFiltro(ByVal obj As EncajeDetalleBE.EncajeDetalleRow, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {obj}
        Dim daEncajeDetalle As New DataSet
        Try
            RegistrarAuditora(parameters)
            daEncajeDetalle = New EncajeDetalleDAM().SeleccionarPorFiltro(obj)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return daEncajeDetalle
    End Function
    Public Function ResultadosEncaje(ByVal Portafolio As String, ByVal fecha As Decimal, ByVal datarequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {Portafolio, fecha, datarequest}
        Dim daEncajeDetalle As New DataSet
        Try
            RegistrarAuditora(parameters)
            daEncajeDetalle = New EncajeDetalleDAM().ResultadosEncaje(Portafolio, fecha)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return daEncajeDetalle
    End Function

    Public Function ReporteResultadosEncaje(ByVal Portafolio As String, ByVal fecha As Decimal, ByVal datarequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {Portafolio, fecha, datarequest}
        Dim daEncajeDetalle As New DataSet
        Try
            RegistrarAuditora(parameters)
            daEncajeDetalle = New EncajeDetalleDAM().ReporteResultadosEncaje(Portafolio, fecha)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return daEncajeDetalle
    End Function
    ' OT 61609 REQ 37 20101122 PLD
    Public Function ProvisionContableImpuesto(ByVal p_fechainicio As Decimal, ByVal p_fechafin As Decimal) As DataSet

        Dim dsgestor As DataSet
        Try
            dsgestor = New EncajeDetalleDAM().ProvisionContableImpuesto(p_fechainicio, p_fechafin)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsgestor

    End Function
    'RGF 20101123 OT 61609
    Public Function ReporteImpuestoRentaAnualPorNemonico(ByVal CodigoNemonico As String, ByVal Portafolio As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal datarequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {CodigoNemonico, Portafolio, datarequest}
        Dim daEncajeDetalle As New DataSet
        Try
            RegistrarAuditora(parameters)
            daEncajeDetalle = New EncajeDetalleDAM().ReporteImpuestoRentaAnualPorNemonico(CodigoNemonico, Portafolio, FechaInicio, FechaFin)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return daEncajeDetalle
    End Function

    'CMB 20101124 OT 61609
    Public Function ObtenerUtilidadPorNemonico(ByVal Portafolio As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal CodigoNemonico As String, ByVal datarequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {CodigoNemonico, Portafolio, datarequest}
        Dim daEncajeDetalle As New DataSet
        Try
            RegistrarAuditora(parameters)
            daEncajeDetalle = New EncajeDetalleDAM().ObtenerUtilidadPorNemonico(Portafolio, FechaInicio, FechaFin, CodigoNemonico)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return daEncajeDetalle
    End Function

    'HDG OT 64771 20120227
    Public Function EliminarTablaTmpNemonicosFondoRenta(ByVal dataRequest As DataSet) As Boolean
        Dim parameters As Object() = {dataRequest}
        Dim bolResult As Boolean = False
        Try
            Dim oEncajeDetalleDAM As New EncajeDetalleDAM
            bolResult = oEncajeDetalleDAM.EliminarTablaTmpNemonicosFondoRenta()
            RegistrarAuditora(parameters)
            Return bolResult
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'HDG OT 64771 20120227
    Public Function ActualizarInstrumentosPorExcel(ByVal data As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim Codigo = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {data, dataRequest}

        Try
            Dim oEncajeDetalleDAM As New EncajeDetalleDAM
            Codigo = oEncajeDetalleDAM.ActualizarInstrumentosPorExcel(data, dataRequest, strmensaje)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            Dim sms As String = ex.Message
            Dim src As String = ex.Source
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function
End Class

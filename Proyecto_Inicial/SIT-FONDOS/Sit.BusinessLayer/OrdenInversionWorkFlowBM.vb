Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class OrdenInversionWorkFlowBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub
    Public Function GetOrdenInversionDivisas(ByVal codigoPortafolio As String, ByVal codigoOrden As String) As DataTable
        Dim dt As New DataTable
        Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
        Try
            dt = oOrdenInversionWorkFlowDAM.GetOrdenInversionDivisas(codigoPortafolio, codigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
    Public Function ValidarNegociacionDiaAnterior(ByVal strCodigoPortafolioSBS As String, ByVal strCodigoMnemonico As String, ByVal strFechaOperacion As String, ByVal dataRequest As DataSet)

        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoPortafolioSBS, strCodigoMnemonico, strFechaOperacion, dataRequest}

        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.ConfirmarOI(strCodigoPortafolioSBS, strCodigoMnemonico, strFechaOperacion, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function
    Public Function ConfirmarOI(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal nroPoliza As String, ByVal dataRequest As DataSet) As Integer
        Dim result As Integer
        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            result = oOrdenInversionWorkFlowDAM.ConfirmarOI(codigoOrden, codigoPortafolioSBS, nroPoliza, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return IIf(CType(result, Integer) = 0, 0, result)
    End Function
    Public Function ActualizaISINOrden(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal ISIN As String) As Boolean
        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.ActualizaISINOrden(codigoOrden, codigoPortafolioSBS, ISIN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function EnviarCXPCDPH(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal nroPoliza As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoOrden, codigoPortafolioSBS, nroPoliza, dataRequest}

        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.EnviarCXPCDPH(codigoOrden, codigoPortafolioSBS, nroPoliza, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return True

    End Function
    Public Function EjecutarOI(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal nroPoliza As String, ByVal dataRequest As DataSet)
        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.EjecutarOI(codigoOrden, codigoPortafolioSBS, nroPoliza, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Public Function ListarOIEjecutadas(ByVal dataRequest As DataSet, ByVal fondo As String, ByVal nroOrden As String, ByVal fecha As String) As DataSet

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim dsDatos As DataSet

        Try
            dsDatos = New OrdenPreOrdenInversionDAM().ListarOIEjecutadas(dataRequest, "", "", "")
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return dsDatos

    End Function

    Public Function AprobarOI(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoOrden, codigoPortafolioSBS, dataRequest}

        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.AprobarOI(codigoOrden, codigoPortafolioSBS, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return True

    End Function

    'HDG OT 60022 20100813
    'Public Function AprobarExcesoBrokerOI(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal dataRequest As DataSet) As Boolean   'HDG OT 61166 20100920
    Public Function AprobarExcesoBrokerOI(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal Estado As String, ByVal dataRequest As DataSet) As Boolean    'HDG OT 61166 20100920

        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoOrden, codigoPortafolioSBS, dataRequest}

        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            'oOrdenInversionWorkFlowDAM.AprobarExcesoBrokerOI(codigoOrden, codigoPortafolioSBS, dataRequest) 'HDG OT 61166 20100920
            oOrdenInversionWorkFlowDAM.AprobarExcesoBrokerOI(codigoOrden, codigoPortafolioSBS, Estado, dataRequest) 'HDG OT 61166 20100920
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return True

    End Function

    Public Function AsignarPOI(ByVal strCodigoPreOrden As String, ByVal dataRequest As DataSet) As String

        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoPreOrden, dataRequest}

        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.AsignarPOI(strCodigoPreOrden, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function CancelarOI_POI(ByVal strCodigoOrdenCancelado As String, ByVal strCodigoOrdenGenerado As String, ByVal dataRequest As DataSet) As Boolean

        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoOrdenCancelado, strCodigoOrdenGenerado, dataRequest}

        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.Cancelar_OI_POI(strCodigoOrdenCancelado, strCodigoOrdenGenerado, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try


    End Function

    Public Function ExtornarOIEjecutadas(ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal dataRequest As DataSet)

        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPortafolioSBS, codigoOrden, dataRequest}
        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.ExtornarOIEjecutadas(codigoPortafolioSBS, codigoOrden, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return True

    End Function

    Public Function ExtornarOIAsignada(ByVal fechaOperacion As Int32, ByVal codigoPreOrden As String, ByVal codigoOrdenF1 As String, ByVal codigoOrdenF2 As String, ByVal codigoOrdenF3 As String, ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As Boolean

        Dim CodigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMnemonico, codigoOrdenF1, codigoOrdenF2, codigoOrdenF3, dataRequest}
        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.ExtornarOIAsignadas(fechaOperacion, codigoPreOrden, codigoMnemonico, codigoOrdenF1, codigoOrdenF2, codigoOrdenF3, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return True

    End Function


    Public Function AutorizarUsuarioAprobacionExcesoOI(ByVal usuario As String, ByVal password As String, ByVal dataRequest As DataSet) As DataSet

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {usuario, password, dataRequest}
        Dim dsDatos As DataSet

        Try
            dsDatos = New OrdenInversionWorkFlowDAM().AutorizarUsuarioAprobacionExcesoOI(usuario, password, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return dsDatos

    End Function

    Public Function EliminarAsignacion(ByVal codigoPreOrden As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPreOrden, dataRequest}
        Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
        Dim result As Boolean = False

        Try
            result = oOrdenInversionWorkFlowDAM.EliminarAsignacion(codigoPreOrden, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return result

    End Function




    Public Function IngresarAsignacion(ByVal strEsTemporal As String, ByVal strCodigoPortafolioSBS As String, ByVal strCodigoAsignacionPreOrden As String, ByVal dclUnidadesPropuesto As Decimal, ByVal dclUnidadesReal As Decimal, ByVal dclPorcentajePropuesto As Decimal, ByVal dclPorcentajeReal As Decimal, ByVal strTipoAsignacion As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strEsTemporal, strCodigoPortafolioSBS, strCodigoAsignacionPreOrden, dclUnidadesPropuesto, dclUnidadesReal, dclPorcentajePropuesto, dclPorcentajeReal, strTipoAsignacion, dataRequest}
        Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
        Dim result As Boolean = False

        Try
            result = oOrdenInversionWorkFlowDAM.IngresarAsignacion(strEsTemporal, strCodigoPortafolioSBS, strCodigoAsignacionPreOrden, dclUnidadesPropuesto, dclUnidadesReal, dclPorcentajePropuesto, dclPorcentajeReal, strTipoAsignacion, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return result

    End Function

    Public Function IngresarAsignacionAgrupacion(ByVal strEsTemporal As String, ByVal strCodigoPreOrden As String, ByVal strCodigoAsignacionPreOrden As String, ByRef strCodigoAsignacionPreOrdenNuevo As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strEsTemporal, strCodigoPreOrden, strCodigoAsignacionPreOrden, dataRequest}
        Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM

        Dim result As Boolean = False

        Try
            result = oOrdenInversionWorkFlowDAM.IngresarAsignacionAgrupacion(strEsTemporal, strCodigoAsignacionPreOrden, strCodigoPreOrden, strCodigoAsignacionPreOrdenNuevo, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return result

    End Function


    Public Function BuscarAsignacion(ByVal strCodigoPreOrden As String, ByVal dataRequest As DataSet) As DataTable

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoPreOrden, dataRequest}
        Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
        Dim datos As DataTable

        Try
            datos = oOrdenInversionWorkFlowDAM.BuscarAsignacion(strCodigoPreOrden, dataRequest).Tables(0)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return datos

    End Function



    Public Function ModificarEstadoAsignacionPreOrden(ByVal StrCodigoPreOrden As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {StrCodigoPreOrden, dataRequest}
        Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
        Dim result As Boolean

        Try
            result = oOrdenInversionWorkFlowDAM.ModificarEstadoAsignacionPreOrden(StrCodigoPreOrden)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return result

    End Function

    Public Function ModificarCodigoAsignacionOI(ByVal StrCodigoOrdenInversion As String, ByVal StrCodigoAsignacionPreOrden As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {StrCodigoOrdenInversion, dataRequest}
        Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
        Dim result As Boolean

        Try
            result = oOrdenInversionWorkFlowDAM.ModificarCodigoAsignacionOI(StrCodigoOrdenInversion, StrCodigoAsignacionPreOrden)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return result
    End Function
    Public Function VencimientoDPZ_OR(CodigoPortafolioSBS As String, CodigoOrden As String, FechaNueva As Decimal, CalculaNuevoVencimiento As String, _
    Usuario As String) As String
        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            Return oOrdenInversionWorkFlowDAM.VencimientoDPZ_OR(CodigoPortafolioSBS, CodigoOrden, FechaNueva, CalculaNuevoVencimiento, Usuario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GeneraOrdenParaVencimientoFuturo(ByVal obj As Hashtable, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
        Dim result As String
        Try
            result = oOrdenInversionWorkFlowDAM.GeneraOrdenParaVencimientoFuturo(obj)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return result
    End Function
    Public Function ExisteNumeroPoliza(ByVal NroPoliza As String) As Boolean
        Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
        Dim result As String
        Try
            result = oOrdenInversionWorkFlowDAM.ExisteNroPoliza(NroPoliza)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return IIf(CType(result, Integer) = 0, False, True)
    End Function
    Function CalculoDPZBisiesto(FechaInicial As Decimal, FechaFinal As Decimal, MontoNominal As Decimal, Tasa As Decimal, TipoTasa As String) As Decimal
        Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
        Try
            Return oOrdenInversionWorkFlowDAM.CalculoDPZBisiesto(FechaInicial, FechaFinal, MontoNominal, Tasa, TipoTasa)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10238 - 12/04/2017 - Carlos Espejo
    'Descripcion: Si el fondo valorizado no se puede realizar la reversion
    Public Sub GeneraCuponera(CodigoOperacion As String, Recalculo As String, CodigoPortafolio As String, CodigoNemonico As String, FechaVencimiento As Decimal, _
    FechaNueva As Decimal)
        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.GeneraCuponera(CodigoOperacion, Recalculo, CodigoPortafolio, CodigoNemonico, FechaVencimiento, FechaNueva)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'OT-11033 - 03/01/2017 - Ian Pastor M.
    'Descripcion: Verifica si existen terceros negociados (Ejecutados o confirmados)
    Public Function OrdenInversion_ExisteTercero(ByVal p_CodigoTercero As String) As DataTable
        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            Return oOrdenInversionWorkFlowDAM.OrdenInversion_ExisteTercero(p_CodigoTercero)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub GeneraVencimientosBono_Swap(ByVal CodigoPortafolio As String, ByVal FechaVencimiento As Decimal, ByVal FechaNueva As Decimal, ByVal codigoOperacion As String)
        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.GeneraVencimientosBono_Swap(CodigoPortafolio, FechaVencimiento, FechaNueva, codigoOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ActualizaDatosCarta(ByVal StrCodigoOrden As String, ByVal StrNumeroCuenta As String, ByVal StrDtc As String)
        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            oOrdenInversionWorkFlowDAM.ActualizaDatosCarta(StrCodigoOrden, StrNumeroCuenta, StrDtc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function OrdenInversion_ValidaExterior(ByVal p_CodigoOrden As String) As String
        Try
            Dim oOrdenInversionWorkFlowDAM As New OrdenInversionWorkFlowDAM
            Return oOrdenInversionWorkFlowDAM.OrdenInversion_ValidaExterior(p_CodigoOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports System.Transactions
Imports MotorTransaccionesProxy
Imports System.Collections.Generic

Public Class PrevOrdenInversionBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
#Region "Funciones no transaccionales"
    Public Function SeleccionarPorFiltro(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal strCodigoClaseInstrumento As String,
    ByVal strCodigoTipoInstrumentoSBS As String, ByVal strCodigoNemonico As String, ByVal strOperador As String, ByVal strEstado As String,
    ByVal dataRequest As DataSet) As DataSet
        Try
            Return New PrevOrdenInversionDAM().SeleccionarPorFiltro(strTipoRenta, decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS,
            strCodigoNemonico, strOperador, strEstado)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarDetallePreOrdenInversion(ByVal codigoPreOrden As Integer, ByVal codigoportafolio As String) As DataTable
        Try
            Return New PrevOrdenInversionDAM().SeleccionarDetallePreOrdenInversion(codigoPreOrden, codigoportafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarDetallePreOrdenInversion(ByVal codigoPreOrden As Integer) As DataTable
        Try
            Return New PrevOrdenInversionDAM().SeleccionarDetallePreOrdenInversion(codigoPreOrden)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorFiltroFuturo(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal strCodigoClaseInstrumento As String,
    ByVal strCodigoTipoInstrumentoSBS As String, ByVal strCodigoNemonico As String, ByVal strOperador As String, ByVal strEstado As String,
    ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strTipoRenta, decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, dataRequest}
        Try
            Return New PrevOrdenInversionDAM().SeleccionarPorFiltroFuturo(strTipoRenta, decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS,
            strCodigoNemonico, strOperador, strEstado)
            RegistrarAuditora(parameters)
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
    Public Function SeleccionarOperaciones(ByVal decCodigoPrevOrden As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decCodigoPrevOrden, dataRequest}
        Try
            Return New PrevOrdenInversionDAM().SeleccionarOperaciones(decCodigoPrevOrden)
            RegistrarAuditora(parameters)
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
    Public Function SeleccionarPorCodigoOrden(ByVal codigoOrden As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoOrden, dataRequest}
        Try
            Return New PrevOrdenInversionDAM().SeleccionarPorCodigoOrden(codigoOrden)
            RegistrarAuditora(parameters)
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

    Public Function SeleccionarOperadores(ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New PrevOrdenInversionDAM().SeleccionarOperadores()
            RegistrarAuditora(parameters)
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
    Public Function SeleccionarValidacionExcesos(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal dataRequest As DataSet,
    Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New PrevOrdenInversionDAM().SeleccionarValidacionExcesos(strTipoRenta, decFechaOperacion, decNProceso)
            RegistrarAuditora(parameters)
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
    Public Function GenerarReporte(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal dataRequest As DataSet,
    Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strTipoRenta, decFechaOperacion, dataRequest}
        Try
            Return New PrevOrdenInversionDAM().GenerarReporte(strTipoRenta, decFechaOperacion, decNProceso)
            RegistrarAuditora(parameters)
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
    Public Function GenerarReporteConFirmas(ByVal tipoRenta As String, ByVal fechaOperacion As Decimal, ByVal dataRequest As DataSet,
    Optional ByVal strClaseInstrumento As String = "", Optional ByVal strCategoriaReporte As String = "", Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {tipoRenta, fechaOperacion, dataRequest}
        Try
            Return New PrevOrdenInversionDAM().GenerarReporteConFirmas(tipoRenta, fechaOperacion, strClaseInstrumento, strCategoriaReporte, decNProceso)
            RegistrarAuditora(parameters)
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
    Public Function SeleccionarTemporal(ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New PrevOrdenInversionDAM().SeleccionarTemporal()
            RegistrarAuditora(parameters)
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
    Public Function GenerarOrdenInversion_Sura(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal dataRequest As DataSet,
    ByVal claseInstrumento As String, Optional ByVal decNProceso As Decimal = 0) As DataSet
        Try
            Return New PrevOrdenInversionDAM().GenerarOrdenInversion_Sura(strTipoRenta, decFechaOperacion, dataRequest, claseInstrumento, decNProceso)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GenerarOrdenInversion(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal dataRequest As DataSet,
    ByVal claseInstrumento As String, Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strTipoRenta, decFechaOperacion, dataRequest}
        Try
            Return New PrevOrdenInversionDAM().GenerarOrdenInversion(strTipoRenta, decFechaOperacion, dataRequest, claseInstrumento, decNProceso)
            RegistrarAuditora(parameters)
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
    Public Function SeleccionarCaracValor(ByVal strNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New PrevOrdenInversionDAM().SeleccionarCaracValor(strNemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarCaracValor(ByVal p_CodigoNemonico As String, ByVal p_FechaOperacion As Decimal) As DataSet
        Try
            Return New PrevOrdenInversionDAM().SeleccionarCaracValor(p_CodigoNemonico, p_FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se quito parametro innecesario
    Public Function ValidarOperaciones(ByVal decCodigoPrevOrden As Decimal, ByVal decCantidadOperacion As Decimal, ByVal bolValida As Boolean) As Boolean
        Try
            Return New PrevOrdenInversionDAM().ValidarOperaciones(decCodigoPrevOrden, decCantidadOperacion, bolValida)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ObtenerExcesosTrader(ByVal strTipoRenta As String, ByVal datarequest As DataSet, Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {strTipoRenta, datarequest}
        Try
            Return New PrevOrdenInversionDAM().ObtenerExcesosTrader(strTipoRenta, decNProceso)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExcesosTrader(ByVal strCodigoGrupLimTrader As String, ByVal strTipoRenta As String, ByVal datarequest As DataSet,
    Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {strCodigoGrupLimTrader, strTipoRenta, datarequest}
        Try
            Return New PrevOrdenInversionDAM().ExcesosTrader(strCodigoGrupLimTrader, strTipoRenta, decNProceso)
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
    Public Function ValidarAprobacion(ByVal decCodigoPrevOrden As Decimal, ByVal datarequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {decCodigoPrevOrden, datarequest}
        Try
            Return New PrevOrdenInversionDAM().ValidarAprobacion(decCodigoPrevOrden)
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

    Public Function ObtenerEstadoPrevOrdenInversion(ByVal decCodigoPrevOrden As Decimal, ByVal datarequest As DataSet) As String 'Agregado por JH 25042019
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {decCodigoPrevOrden, datarequest}
        Try
            Return New PrevOrdenInversionDAM().ObtenerEstadoPrevOrdenInversion(decCodigoPrevOrden)
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
    Public Function ValidaExtorno(ByVal decCodigoPrevOrden As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decCodigoPrevOrden, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            Return oPrevOrdenInversionDAM.ValidaExtorno(decCodigoPrevOrden)
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
    Public Function SeleccionarPreExtorno(ByVal decCodigoPrevOrden As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decCodigoPrevOrden, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            Return oPrevOrdenInversionDAM.SeleccionarPreExtorno(decCodigoPrevOrden)
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
    Public Function SeleccionarImprimir_PrevOrdenInversion(ByVal decCodigoPrevOrden As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decCodigoPrevOrden, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            Return oPrevOrdenInversionDAM.SeleccionarImprimir_PrevOrdenInversion(decCodigoPrevOrden)
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
#Region "66056 - Modificacion: JZAVALA"
    ''' <summary>66056 - JZAVALA.
    ''' REPORTE EXCEL CONSOLIDADO.
    ''' </summary>
    ''' <param name="strTipoRenta"></param>
    ''' <param name="decFechaOperacion"></param>
    ''' <param name="strCodigoClaseInstrumento"></param>
    ''' <param name="strCodigoTipoInstrumentoSBS"></param>
    ''' <param name="strCodigoNemonico"></param>
    ''' <param name="strOperador"></param>
    ''' <param name="strEstado"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerarReporteConsolidado(ByVal decFechaOperacion As Decimal, ByVal strCodigoClaseInstrumento As String, ByVal strCodigoTipoInstrumentoSBS As String,
    ByVal strCodigoNemonico As String, ByVal strOperador As String, ByVal strEstado As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, dataRequest}
        Try
            Return New PrevOrdenInversionDAM().GenerarReporteConsolidado(decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, strOperador, strEstado)
            RegistrarAuditora(parameters)
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
    ''' <summary>66056 - JZAVALA.
    ''' REPORTE EXCEL FUTURO.
    ''' </summary>
    ''' <param name="fechaIni"></param>
    ''' <param name="fechaFin"></param>
    ''' <param name="Portafolio"></param>
    ''' <param name="strCodigoOperacion"></param>
    ''' <param name="strMoneda"></param>
    ''' <param name="strCodigoMercado"></param>
    ''' <param name="strNumeroCuenta"></param>
    ''' <param name="datarequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ListarOperacionesCajaFuturos(ByVal fechaIni As Decimal, ByVal fechaFin As Decimal, ByVal Portafolio As String, ByVal strCodigoOperacion As String,
    ByVal strMoneda As String, ByVal strCodigoMercado As String, ByVal strNumeroCuenta As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Try
            oreporte = New PrevOrdenInversionDAM().ListarOperacionesCajaFuturos(fechaIni, fechaFin, Portafolio, strCodigoOperacion, strMoneda, strCodigoMercado,
            strNumeroCuenta)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function
#End Region
#End Region
#Region "Funciones transaccionales"
    Public Function Insertar_Sura(ByVal oPrevOrdenInversionBE As PrevOrdenInversionBE, ByVal strTipoRenta As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPrevOrdenInversionBE, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.Insertar(oPrevOrdenInversionBE, strTipoRenta, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return bolResult
    End Function
    Public Function Modificar_Sura(ByVal oPrevOrdenInversionBE As PrevOrdenInversionBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPrevOrdenInversionBE, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.Modificar_Sura(oPrevOrdenInversionBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return bolResult
    End Function
    Public Function Insertar(ByVal oPrevOrdenInversionBE As PrevOrdenInversionBE, ByVal strTipoRenta As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPrevOrdenInversionBE, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.Insertar(oPrevOrdenInversionBE, strTipoRenta, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return bolResult
    End Function
    'OT10795 - 06/10/2017 Refactorizar código
    Public Function Insertar(ByVal oPrevOrdenInversionBE As PrevOrdenInversionBE, ByVal strTipoRenta As String, ByVal dataRequest As DataSet,
    ByVal dtdetalle As DataTable) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.Insertar(oPrevOrdenInversionBE, strTipoRenta, dataRequest, dtdetalle)
        Catch ex As Exception
            Throw ex
        End Try
        Return bolResult
    End Function
    'OT10795 - Fin
    Public Function insertarDetalle(ByVal codprevorden As Integer, ByVal codigoportafolio As String, ByVal asignacion As Decimal) As Boolean
        Try
            Return New PrevOrdenInversionDAM().insertarDetalle(codprevorden, codigoportafolio, Decimal.Parse(asignacion))
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT10795 - 06/10/2017 Refactorizar código
    Public Function Modificar(ByVal oPrevOrdenInversionBE As PrevOrdenInversionBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.Modificar(oPrevOrdenInversionBE, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return bolResult
    End Function
    'OT10795 - Fin
    Public Function modificarDetalle(ByVal codigoprevorden As Integer, ByVal codigoportafolio As String, ByVal asignacion As Decimal) As Boolean
        Try
            Return New PrevOrdenInversionDAM().modificarDetalle(codigoprevorden, codigoportafolio, asignacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Eliminar(ByVal decCodigoPrevOrden As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decCodigoPrevOrden, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.Eliminar(decCodigoPrevOrden, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return bolResult
    End Function
    Public Function eliminarDetalle(ByVal codigoprevorden As Integer, ByVal codigoportafolio As String) As Boolean
        Try
            Return New PrevOrdenInversionDAM().eliminarDetalle(codigoprevorden, codigoportafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertarOperacion(ByVal oPrevOrdenInversionDetalleBE As PrevOrdenInversionDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPrevOrdenInversionDetalleBE, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.InsertarOperacion(oPrevOrdenInversionDetalleBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return bolResult
    End Function
    Public Function ModificarOperacion(ByVal oPrevOrdenInversionDetalleBE As PrevOrdenInversionDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPrevOrdenInversionDetalleBE, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.ModificarOperacion(oPrevOrdenInversionDetalleBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return bolResult
    End Function
    Public Function EliminarOperacion(ByVal decCodigoPrevOrdenDet As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decCodigoPrevOrdenDet, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.EliminarOperacion(decCodigoPrevOrdenDet)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return bolResult
    End Function
    'OT10795 06/10/2017 Refactorizar código
    Public Function ProcesarEjecucion(ByVal decCodigoPrevOrden As Decimal, ByVal dataRequest As DataSet, Optional ByVal decNProceso As Decimal = 0) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.ProcesarEjecucion(decCodigoPrevOrden, dataRequest, decNProceso)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT10795 - Fin
    Public Sub InicializarPrevOrdenInversion(ByRef oRow As PrevOrdenInversionBE.PrevOrdenInversionRow)
        Try
            Dim daPrevOrdenInversion As New PrevOrdenInversionDAM
            daPrevOrdenInversion.InicializarPrevOrdenInversion(oRow)
            'Luego de terminar la ejecución de métodos(sin errores)             
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Function AprobarNegociacion(ByVal dataRequest As DataSet, Optional ByVal decNProceso As Decimal = 0) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.AprobarNegociacion(dataRequest, , , , decNProceso)
            RegistrarAuditora(parameters)
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
    Public Function AprobarNegociacionTrader(ByVal dataRequest As DataSet, ByVal tipoRenta As String, ByVal decNProceso As Decimal) As Boolean
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            Return oPrevOrdenInversionDAM.AprobarNegociacionTrader(dataRequest, tipoRenta, decNProceso)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function AprobarNegociacionExcesosTrader(ByVal dataRequest As DataSet, ByVal usuarioAprob As String, ByVal codigoPrevOrden As Decimal,
    ByVal tipoRenta As String) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {usuarioAprob, codigoPrevOrden, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.AprobarNegociacion(dataRequest, usuarioAprob, codigoPrevOrden, tipoRenta)
            RegistrarAuditora(parameters)
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
    Public Function ProcesarSwapDivisa(ByVal decFechaOperacion As Decimal, ByVal dataRequest As DataSet, ByVal decProceso As Decimal) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.ProcesarSwapDivisa(decFechaOperacion, decProceso)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return bolResult
    End Function
    Public Function Extornar(ByVal decCodigoPrevOrden As Decimal, ByVal strCodigoMotivo As String, ByVal strComentario As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decCodigoPrevOrden, strCodigoMotivo, strComentario, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.Extornar(decCodigoPrevOrden, strCodigoMotivo, strComentario, dataRequest)
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
    'OT10795 - 06/10/2017 Refactorizar código
    Public Sub ActualizaSeleccionPrevOrden(ByVal decCodigoPrevOrden As Decimal, ByVal Flag As String)
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            oPrevOrdenInversionDAM.ActualizaSeleccionPrevOrden(decCodigoPrevOrden, Flag)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'OT10795 Fin
    Public Function DesAprobarNegociacion(ByVal decCodigoPrevOrden As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decCodigoPrevOrden, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.DesAprobarNegociacion(decCodigoPrevOrden, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return bolResult
    End Function
    'OT10795 - 06/10/2017 Refactorizar código
    Public Function InsertarProcesoMasivo(ByVal pUsuario As String) As Decimal
        Dim NProceso As Decimal = 0
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            NProceso = oPrevOrdenInversionDAM.InsertarProcesoMasivo(pUsuario)
        Catch ex As Exception
            Throw ex
        End Try
        Return NProceso
    End Function
    'OT10795 - Fin
    Public Function EliminarProcesoMasivo(ByVal pNProceso As Decimal) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.EliminarProcesoMasivo(pNProceso)
        Catch ex As Exception
            Throw ex
        End Try
        Return bolResult
    End Function
    'OT10795 - Fin
    Public Function TruncarProcesoMasivo() As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.TruncarProcesoMasivo()
        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return bolResult
    End Function
    Public Function InsertarTrazabilidad(ByVal oTrazabilidadOperacionBE As TrazabilidadOperacionBE, ByVal pProceso As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oTrazabilidadOperacionBE, dataRequest}
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.InsertarTrazabilidad(oTrazabilidadOperacionBE, pProceso, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return bolResult
    End Function
    'OT10795 - 06/10/2017 Refactorizar código
    Public Function InsertarTrazabilidad_sura(ByVal oTrazabilidadOperacionBE As TrazabilidadOperacionBE, ByVal pProceso As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            bolResult = oPrevOrdenInversionDAM.InsertarTrazabilidad_sura(oTrazabilidadOperacionBE, pProceso, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return bolResult
    End Function
    'OT10795 - Fin
    Public Function TrazabilidadOperaciones(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim bolResult As Boolean = False
        Dim oReporte As New DataSet
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            oReporte = oPrevOrdenInversionDAM.TrazabilidadOperaciones(FechaInicio, FechaFin)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Funcion para listar aprobadores segun grupo
    Public Function AprobarNegociacionTrader(ByVal CodigoGrupLimTrader As Integer, ByVal CodigoPortafolio As String, ByVal PorcentajeExcedido As Decimal) As DataTable
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            Return oPrevOrdenInversionDAM.AprobarNegociacionTrader(CodigoGrupLimTrader, CodigoPortafolio, PorcentajeExcedido)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    '==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-10-01 | Calculo del TIR Neto Post Ejecución
    Public Sub ActualizarTirNetaEnOrdenInversion(ByVal codigoOrden As String, ByVal codigoPortafolio As String, ByVal tirNeta As Decimal)
        Try
            Dim oPrevOrdenInversionDAM As New PrevOrdenInversionDAM
            oPrevOrdenInversionDAM.ActualizarTirNetaEnOrdenInversion(codigoOrden, codigoPortafolio, tirNeta)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    '==== FIN | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-10-01 | Calculo del TIR Neto Post Ejecución

    '==== INICIO | FACTURAS NEGOCIABLES | CDA | Ernesto Galarza | 2019-01-16 | Proceso de generacion de ordenes (Facturas negociables) realizadas por excel
    Public Function InsertarFacturasNegociable(ByVal pListaFacturasNegociables As List(Of ValidarFicheroBE), ByVal pTipoRentaFija As String, ByVal pDatosRequest As DataSet) As Boolean
        Try

            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Using transaction As New TransactionScope
                For Each item As ValidarFicheroBE In pListaFacturasNegociables
                    'guardar la negociacion2

                    oPrevOrdenInversionBM.Insertar(item.PreOrdenInversion, pTipoRentaFija, pDatosRequest, item.DtAsignacion)
                Next
                transaction.Complete()
            End Using
        Catch ex As Exception
            'Throw ex
            Return False
        End Try

        Return True
    End Function

    Public Function RegistrarValorFacturaNegociable(ByVal pValidarFicheroBE As ValidarFicheroBE, ByVal pDatosRequest As DataSet) As ValoresBE

        Try
            Dim oValoresBE As New ValoresBE
            Dim oValoresDAM As New ValoresDAM
            Dim oRow As ValoresBE.ValorRow
            Dim oValoresBM As New ValoresBM
            oRow = CType(oValoresBE.Valor.NewRow(), ValoresBE.ValorRow)
            oValoresBM.InicializarValores(oRow)
            oRow.CodigoFactura = pValidarFicheroBE.CodigoFacturacion
            oRow.TipoRentaFija = "1"
            oRow.CodigoRenta = "1"
            'opcional oRow.CodigoTipoTitulo = ddlTipoTitulo.SelectedValue
            oRow.CodigoTipoInstrumentoSBS = "98"
            oRow.Descripcion = pValidarFicheroBE.CodigoFacturacion
            oRow.NumeroUnidades = 1D
            oRow.FechaVencimiento = pValidarFicheroBE.FechaVencimiento
            oRow.FechaEmision = pValidarFicheroBE.FechaEmision
            oRow.CodigoTipoCupon = "3"
            oRow.Cantidad = pValidarFicheroBE.MontoNominal
            oRow.ValorNominal = pValidarFicheroBE.MontoNominal
            oRow.BaseInteresCorrido = "360"
            oRow.BaseInteresCorridoDias = "ACT"
            oRow.GeneraInteres = "1"
            oRow.TasaCupon = pValidarFicheroBE.Tasa
            oRow.Rating = "56"
            oRow.CodigoMoneda = pValidarFicheroBE.CodigoMoneda
            oRow.MonedaCupon = oRow.CodigoMoneda
            oRow.MonedaPago = oRow.CodigoMoneda
            oRow.TipoCuponera = "N"
            oRow.CodigoTipoAmortizacion = "1" ' Al vencimiento
            oRow.CodigoPeriodicidad = "7"
            oRow.TasaSpread = 0D
            oRow.CodigoMercado = "1"
            oRow.Estilo = String.Empty
            oRow.FechaPrimerCupon = pValidarFicheroBE.FechaEmision
            oRow.NumeroUnidades = 1D
            oRow.Situacion = "A"
            oRow.TipoCodigoValor = "1"
            oRow.Agrupacion = "S"
            oRow.ValorUnitario = 1
            oRow.PosicionAct = 0D
            oRow.PorcPosicion = 0D
            oRow.BaseAnual = oRow.BaseInteresCorrido
            oRow.BaseCupon = oRow.BaseInteresCorrido
            oRow.BaseCuponDias = oRow.BaseInteresCorrido
            oRow.BaseIC = oRow.BaseInteresCorrido
            oRow.BaseICDias = oRow.BaseInteresCorrido
            oRow.BaseTir = oRow.BaseInteresCorrido
            oRow.BaseTirDias = oRow.BaseInteresCorrido
            oRow.CodigoEmisor = pValidarFicheroBE.CodigoEntidad
            oRow.FechaLiberada = 0D
            oRow.TipoCodigoValor = 2
            oRow.GeneraInteres = 0D

            oValoresBE.Valor.AddValorRow(oRow)
            oValoresBE.Valor.AcceptChanges()
            ' por revisar
            'oRow.CodigoEmisor  
            oValoresBM.Insertar(oValoresBE, pDatosRequest)
            Dim oValorRespuesta As ValoresBE = oValoresBM.SeleccionarPorCodigoFactura(oRow.CodigoFactura, String.Empty, pDatosRequest)
            oValoresBE.Valor.Rows(0)("CodigoNemonico") = oValorRespuesta.Valor.Rows(0)("CodigoNemonico")
            oValoresBE.Valor.Rows(0)("CodigoISIN") = oValorRespuesta.Valor.Rows(0)("CodigoISIN")
            oValoresBE.Valor.Rows(0)("CodigoTipoTitulo") = oValorRespuesta.Valor.Rows(0)("CodigoNemonico")
            oValoresBE.Valor.AcceptChanges()
            ' falta codigo nemonico
            Return oValoresBE

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function RegistrarCuponeraNomal(ByVal pValor As ValoresBE, ByVal pPreOrden As PrevOrdenInversionBE, ByVal pDatosRequest As DataSet) As Boolean

        Try

            Dim oRowValor As ValoresBE.ValorRow = CType(pValor.Valor.Rows(0), ValoresBE.ValorRow)
            Dim oRowPreOrden As PrevOrdenInversionBE.PrevOrdenInversionRow = CType(pPreOrden.PrevOrdenInversion.Rows(0), PrevOrdenInversionBE.PrevOrdenInversionRow)
            Dim oCuponeraDAM = New CuponeraDAM

            Dim oCuponNormalBE As New CuponeraNormalBE
            Dim oRow As CuponeraNormalBE.CuponeraNormalRow
            oRow = CType(oCuponNormalBE.CuponeraNormal.NewRow(), CuponeraNormalBE.CuponeraNormalRow)
            oRow.CodigoNemonico = oRowValor.CodigoNemonico
            oRow.Secuencia = "1"
            oRow.FechaInicio = oRowValor.FechaEmision  'ConvertirFechaaDecimal(oDR("FechaIni"))
            oRow.FechaTermino = oRowValor.FechaVencimiento 'ConvertirFechaaDecimal(oDR("FechaFin"))

            Dim strFechaInicio As String = oRow.FechaInicio.ToString()
            Dim strFechaTermino As String = oRow.FechaTermino.ToString()
            Dim dateFechaInicio As Date = Convert.ToDateTime(strFechaInicio.Substring(6, 2) + "/" + strFechaInicio.Substring(4, 2) + "/" + strFechaInicio.Substring(0, 4))
            Dim dateFechaTermino As Date = Convert.ToDateTime(strFechaTermino.Substring(6, 2) + "/" + strFechaTermino.Substring(4, 2) + "/" + strFechaTermino.Substring(0, 4))
            Dim DiferenciaDias As Integer = DateDiff(DateInterval.Day, dateFechaInicio, dateFechaTermino) + 1

            oRow.DiferenciaDias = DiferenciaDias
            oRow.Amortizacion = 0D
            oRow.TasaCupon = oRowPreOrden.Tasa
            oRow.Base = oRowValor.BaseCuponDias
            oRow.DiasPago = DiferenciaDias
            oRow.Amortizacion = 100
            oRow.Situacion = "A"
            oCuponNormalBE.CuponeraNormal.AddCuponeraNormalRow(oRow)
            oCuponNormalBE.CuponeraNormal.AcceptChanges()


            oCuponeraDAM.RegistrarCuponeraNormal(oCuponNormalBE, 100D, pDatosRequest)

            Return True



        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function RegistrarCustodio(ByVal pValidarFicheroBE As ValidarFicheroBE, ByVal pDatosRequest As DataSet) As Boolean

        Try
            Dim oValoresDAM As New ValoresDAM
            oValoresDAM.InsertarDetalleAux(pValidarFicheroBE.CodigoNemonico, pValidarFicheroBE.CodigoPortafolio, "CAVALI", String.Empty, "A", pDatosRequest)

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function ProcesarFacturasNegociable(ByVal pListaFacturasNegociables As List(Of ValidarFicheroBE), ByVal pTipoRentaFija As String, ByVal pCodigoPortafolio As String, ByVal pDatosRequest As DataSet) As Boolean
        Try

            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
            Using transaction As New TransactionScope
                For Each item As ValidarFicheroBE In pListaFacturasNegociables
                    item.CodigoPortafolio = pCodigoPortafolio

                    'Guarda el instrumento
                    Dim oValorBE As ValoresBE = RegistrarValorFacturaNegociable(item, pDatosRequest)
                    item.PreOrdenInversion.PrevOrdenInversion.Rows(0)("CodigoNemonico") = oValorBE.Valor.Rows(0)("CodigoNemonico")
                    item.PreOrdenInversion.PrevOrdenInversion.Rows(0)("Estado") = "EJE"
                    item.PreOrdenInversion.PrevOrdenInversion.Rows(0)("HoraOperacion") = Date.Now.ToLongTimeString()
                    item.PreOrdenInversion.PrevOrdenInversion.AcceptChanges()
                    item.CodigoNemonico = oValorBE.Valor.Rows(0)("CodigoNemonico")

                    'guarda cuponera
                    RegistrarCuponeraNomal(oValorBE, item.PreOrdenInversion, pDatosRequest)

                    'guarda custodio
                    RegistrarCustodio(item, pDatosRequest)


                    'Creo la asignacion
                    Dim dtAsignacion As New DataTable ' Asignacion Por Fondo
                    dtAsignacion.Columns.Add("CodigoPortafolio")
                    dtAsignacion.Columns.Add("Asignacion")
                    dtAsignacion.Rows.Add(pCodigoPortafolio, 100D)
                    item.DtAsignacion = dtAsignacion

                    'guardar la preoOrden
                    Insertar(item.PreOrdenInversion, pTipoRentaFija, pDatosRequest, item.DtAsignacion)

                    'guarda la orden
                    RegistrarOrden(item.PreOrdenInversion, oValorBE, item, pDatosRequest)

                    'seleccionar codigo de orden y preorden
                    Dim ds As DataSet = oOrdenPreOrdenInversionDAM.SeleccionarOrden_OrdenPreOrden(item.CodigoNemonico)
                    Dim codigoOrden As String = ""
                    Dim codigoPreOrden As String = ""

                    If ds.Tables.Count > 0 Then
                        codigoOrden = ds.Tables(0).Rows(0)("CodigoOrden")
                        codigoPreOrden = ds.Tables(1).Rows(0)("CodigoPrevOrden")
                    End If

                    'Registro Relacion de CodigoOrden con Orden
                    RegistrarRelacionOrdenPreOrden(codigoOrden, codigoPreOrden)

                    'Registro de comisiones
                    RegistrarComisionesPreOrden(codigoPreOrden, item, oValorBE, item.PreOrdenInversion, pDatosRequest)

                    'Actualiza las ordenes a confirmadas (genera cuentas por cobrar)
                    GenerarCuentasPorCobrarPagar(codigoOrden, item.CodigoPortafolio, pDatosRequest)

                Next
                transaction.Complete()
            End Using
        Catch ex As Exception
            Throw ex
            ' Return False
        End Try

        Return True
    End Function
    Public Function RegistrarComisionesPreOrden(ByVal pCodigoPreOrden As String, ByVal pValidarFicheroBE As ValidarFicheroBE, ByVal pValor As ValoresBE, ByVal pPreOrden As PrevOrdenInversionBE, ByVal pDatosRequest As DataSet) As Boolean

        Dim oImpuestosComisionesOrdenPreOrdenDAM As New ImpuestosComisionesOrdenPreOrdenDAM
        Dim oRowPreOrden As PrevOrdenInversionBE.PrevOrdenInversionRow = CType(pPreOrden.PrevOrdenInversion.Rows(0), PrevOrdenInversionBE.PrevOrdenInversionRow)
        Dim oImpuestosComisionesOrdenPreOrdenBE As New ImpuestosComisionesOrdenPreOrdenBE

        Dim oRowValor As ValoresBE.ValorRow = CType(pValor.Valor.Rows(0), ValoresBE.ValorRow)
        Dim oRowComision As ImpuestosComisionesOrdenPreOrdenBE.ImpuestosComisionesOrdenPreOrdenRow = CType(oImpuestosComisionesOrdenPreOrdenBE.ImpuestosComisionesOrdenPreOrden.NewRow(), ImpuestosComisionesOrdenPreOrdenBE.ImpuestosComisionesOrdenPreOrdenRow)

        oRowComision.CodigoOrdenPreorden = pCodigoPreOrden
        oRowComision.CodigoPortafolioSBS = pValidarFicheroBE.CodigoPortafolio
        oRowComision.Situacion = "A"
        oRowComision.CodigoMercado = oRowValor.CodigoMercado
        oRowComision.CodigoRenta = oRowValor.CodigoRenta
        oRowComision.CodigoPlaza = oRowPreOrden.CodigoPlaza
        oRowComision.CodigoTarifa = "PCOMIS SAB"
        oRowComision.ValorCalculado = pValidarFicheroBE.ComisionValor.PCOMIS_SAB
        oRowComision.ValorReal = pValidarFicheroBE.ComisionValor.PCOMIS_SAB
        oImpuestosComisionesOrdenPreOrdenBE.ImpuestosComisionesOrdenPreOrden.AddImpuestosComisionesOrdenPreOrdenRow(oRowComision)
        oImpuestosComisionesOrdenPreOrdenBE.AcceptChanges()
        oImpuestosComisionesOrdenPreOrdenDAM.Insertar(oImpuestosComisionesOrdenPreOrdenBE, pDatosRequest)


        oRowComision.CodigoTarifa = "PCUOTA B.V.L"
        oRowComision.ValorCalculado = pValidarFicheroBE.ComisionValor.PCUOTA_BVL
        oRowComision.ValorReal = pValidarFicheroBE.ComisionValor.PCUOTA_BVL
        oImpuestosComisionesOrdenPreOrdenBE.AcceptChanges()
        oImpuestosComisionesOrdenPreOrdenDAM.Insertar(oImpuestosComisionesOrdenPreOrdenBE, pDatosRequest)


        oRowComision.CodigoTarifa = "PFND GARNTIA"
        oRowComision.ValorCalculado = pValidarFicheroBE.ComisionValor.PFND_GARNTIA
        oRowComision.ValorReal = pValidarFicheroBE.ComisionValor.PFND_GARNTIA
        oImpuestosComisionesOrdenPreOrdenBE.AcceptChanges()
        oImpuestosComisionesOrdenPreOrdenDAM.Insertar(oImpuestosComisionesOrdenPreOrdenBE, pDatosRequest)


        oRowComision.CodigoTarifa = "PCONT CONASE"
        oRowComision.ValorCalculado = pValidarFicheroBE.ComisionValor.PCONT_CONASE
        oRowComision.ValorReal = pValidarFicheroBE.ComisionValor.PCONT_CONASE
        oImpuestosComisionesOrdenPreOrdenBE.AcceptChanges()
        oImpuestosComisionesOrdenPreOrdenDAM.Insertar(oImpuestosComisionesOrdenPreOrdenBE, pDatosRequest)


        oRowComision.CodigoTarifa = "PCUOT CAVLI"
        oRowComision.ValorCalculado = pValidarFicheroBE.ComisionValor.PCUOT_CAVLI
        oRowComision.ValorReal = pValidarFicheroBE.ComisionValor.PCUOT_CAVLI
        oImpuestosComisionesOrdenPreOrdenBE.AcceptChanges()
        oImpuestosComisionesOrdenPreOrdenDAM.Insertar(oImpuestosComisionesOrdenPreOrdenBE, pDatosRequest)


        oRowComision.CodigoTarifa = "P I.G.V. TOT"
        oRowComision.ValorCalculado = pValidarFicheroBE.ComisionValor.PIGV_TOT
        oRowComision.ValorReal = pValidarFicheroBE.ComisionValor.PIGV_TOT
        oImpuestosComisionesOrdenPreOrdenBE.AcceptChanges()
        oImpuestosComisionesOrdenPreOrdenDAM.Insertar(oImpuestosComisionesOrdenPreOrdenBE, pDatosRequest)




    End Function
    Private Sub RegistrarRelacionOrdenPreOrden(ByVal pCodigoOrden As String, ByVal pCodigoPreorden As String)
        Dim sqlCadena As New System.Text.StringBuilder
        Dim objImportar As New ImportadorExcelDAM

        sqlCadena.Append("INSERT INTO PrevOrdenInversion_OI")
        sqlCadena.Append("(CodigoPrevOrden,CodigoOrden)")
        sqlCadena.Append(" VALUES(@PREORDEN,@ORDEN)")
        sqlCadena.Replace("@PREORDEN", pCodigoPreorden)
        sqlCadena.Replace("@ORDEN", pCodigoOrden)
        objImportar.ImportarInformacion(sqlCadena.ToString())


    End Sub
    Private Sub GenerarCuentasPorCobrarPagar(ByVal pCodigoOrden As String, ByVal pCodigoPortafolio As String, ByVal pDatosRequest As DataSet)
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        oOrdenInversionWorkFlowBM.ConfirmarOI(pCodigoOrden, pCodigoPortafolio, String.Empty, pDatosRequest)
    End Sub
    Private Sub RegistrarOrden(ByVal pPrevOrdenInversionBE As PrevOrdenInversionBE, ByVal pValor As ValoresBE, ByVal ValidarFicheroBE As ValidarFicheroBE, ByVal pDatosRequest As DataSet)
        Dim oOrdenPreOrdenInversionDAM As New OrdenPreOrdenInversionDAM
        Dim oOrdenPreOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRowPreOrden As PrevOrdenInversionBE.PrevOrdenInversionRow = CType(pPrevOrdenInversionBE.PrevOrdenInversion.Rows(0), PrevOrdenInversionBE.PrevOrdenInversionRow)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow = CType(oOrdenPreOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        Dim oRowValor As ValoresBE.ValorRow = CType(pValor.Valor.Rows(0), ValoresBE.ValorRow)
        oOrdenPreOrdenInversionDAM.InicializarOrdenInversion(oRow)

        oRow.CodigoPortafolioSBS = ValidarFicheroBE.CodigoPortafolio
        oRow.FechaOperacion = oRowPreOrden.FechaOperacion

        'Valor por confirmar
        oRow.FechaLiquidacion = oRowPreOrden.FechaLiquidacion
        oRow.FechaPago = oRowPreOrden.FechaOperacion
        oRow.MontoNominalOperacion = oRowPreOrden.MontoNominal
        oRow.CodigoTipoCupon = oRowValor.CodigoTipoCupon

        oRow.CodigoMoneda = oRowPreOrden.Moneda
        oRow.CodigoMonedaDestino = oRowPreOrden.Moneda
        oRow.CodigoMonedaOrigen = oRowPreOrden.Moneda
        oRow.MontoOperacion = oRowPreOrden.MontoOperacion
        oRow.CodigoISIN = oRowValor.CodigoISIN
        oRow.CodigoMnemonico = oRowValor.CodigoNemonico
        oRow.CodigoTercero = oRowPreOrden.CodigoTercero
        oRow.CodigoOperacion = oRowPreOrden.CodigoOperacion
        oRow.HoraOperacion = oRowPreOrden.HoraOperacion
        oRow.TasaPorcentaje = oRowPreOrden.Tasa
        oRow.CategoriaInstrumento = "FA"
        oRow.TipoTramo = oRowPreOrden.TipoTramo
        oRow.RegulaSBS = "N"
        oRow.Estado = "E-EJE"
        oRow.YTM = oRowPreOrden.Tasa
        oRow.Situacion = "A"
        oRow.CantidadOperacion = oRowPreOrden.CantidadOperacion
        oRow.CantidadOrdenado = oRowPreOrden.MontoNominal
        'oRow.MontoNetoOperacion = oRowPreOrden.MontoOperacion
        oRow.MontoNetoOperacion = ValidarFicheroBE.MontoNeto
        oRow.MontoOperacion = oRowPreOrden.MontoOperacion
        oRow.MontoNominalOrdenado = oRowPreOrden.MontoNominal
        oRow.MontoNominalOperacion = oRowPreOrden.MontoNominal
        oRow.PrecioNegociacionLimpio = oRowPreOrden.Precio
        oRow.PrecioNegociacionSucio = oRowPreOrden.Precio

        oOrdenPreOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenPreOrdenInversionDAM.InsertarOI(oOrdenPreOrdenInversionBE, "", pDatosRequest)
        'oOrdenPreOrdenInversionBE()
        'oOrdenPreOrdenInversionBE.OrdenPreOrdenInversion.AddValorRow(oRow)
        'oValoresBE.Valor.AcceptChanges()


    End Sub
    '==== FIN | FACTURAS NEGOCIABLES | CDA | Ernesto Galarza | 2019-01-16 | Proceso de generacion de ordenes (Facturas negociables) realizadas por excel

#End Region
End Class


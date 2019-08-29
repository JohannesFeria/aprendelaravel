Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy

Public Class LimiteTradingBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Sub InicializarLimiteTrading(ByRef oRow As LimiteTradingBE.LimiteTradingRow)
        Try
            Dim daLimiteTrading As New LimiteTradingDAM
            daLimiteTrading.InicializarLimiteTrading(oRow)
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
#Region "Funciones no transaccionales"
    Public Function Seleccionar(ByVal codigoTrading As Decimal, ByVal dataRequest As DataSet) As LimiteTradingBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTrading, dataRequest}

        Try
            Return New LimiteTradingDAM().Seleccionar(codigoTrading)
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
    Public Function SeleccionarPorFiltro(ByVal strCodigoRenta As String, ByVal decGrupoLimite As Decimal, ByVal strTipoCargo As String, ByVal strPortafolio As String, ByVal dataRequest As DataSet) As DataSet  'HDG OT 64291 20111129
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoRenta, decGrupoLimite, strTipoCargo, strPortafolio, dataRequest} 'HDG OT 64291 20111129
        Try
            Return New LimiteTradingDAM().SeleccionarPorFiltro(strCodigoRenta, decGrupoLimite, strTipoCargo, strPortafolio) 'HDG OT 64291 20111129
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

    Public Function ReporteLimitesTrading(ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New LimiteTradingDAM().ReporteLimitesTrading()
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

    'JHC REQ 66056: Implementacion Futuros
    Public Function SeleccionarValidacionExcesosTrader(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal strUsuario As String, ByVal dataRequest As DataSet, Optional ByVal claseInstrumento As String = "", Optional ByVal decNProceso As Decimal = 0) As DataSet 'HDG OT 67554 duplicado
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strTipoRenta, decFechaOperacion, dataRequest}
        Try
            Return New LimiteTradingDAM().SeleccionarValidacionExcesosTrader(strTipoRenta, decFechaOperacion, strUsuario, claseInstrumento, decNProceso)    'HDG OT 67554 duplicado
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
    ''' <summary>
    ''' SURA 
    ''' </summary>
    ''' OT10795 06/10/2017 Refactorizar código
    Public Function SeleccionarValidacionExcesosTrader_Sura(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal strUsuario As String, ByVal dataRequest As DataSet, Optional ByVal claseInstrumento As String = "", Optional ByVal decNProceso As Decimal = 0) As DataSet
        Try
            Return New LimiteTradingDAM().SeleccionarValidacionExcesosTrader_Sura(strTipoRenta, decFechaOperacion, strUsuario, claseInstrumento, decNProceso)    'HDG OT 67554 duplicado
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT10795 Fin
#End Region

#Region "Funciones transaccionales"
    Public Function Insertar(ByVal oLimiteTradingBE As LimiteTradingBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oLimiteTradingBE, dataRequest}
        Try
            Dim oLimiteTradingDAM As New LimiteTradingDAM

            bolResult = oLimiteTradingDAM.Insertar(oLimiteTradingBE, dataRequest)
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

    Public Function Modificar(ByVal oLimiteTradingBE As LimiteTradingBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oLimiteTradingBE, dataRequest}
        Try
            Dim oLimiteTradingDAM As New LimiteTradingDAM

            bolResult = oLimiteTradingDAM.Modificar(oLimiteTradingBE, dataRequest)
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

    Public Function Eliminar(ByVal decCodigoTrading As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decCodigoTrading, dataRequest}
        Try
            Dim oLimiteTradingDAM As New LimiteTradingDAM

            bolResult = oLimiteTradingDAM.Eliminar(decCodigoTrading, dataRequest)
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
#End Region
End Class

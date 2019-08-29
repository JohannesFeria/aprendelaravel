'Creado por: HDG OT 64480 20120119
Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy

Public Class RolAprobadoresTraderBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

#Region "Funciones no transaccionales"
    Public Function ListarGrupoLimite(ByVal CodigoRenta As String) As RolTraderBE
        Dim parameters As Object() = {CodigoRenta}

        Try
            Return New RolAprobadoresTraderDAM().ListarGrupoLimite(CodigoRenta)
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

    Public Function SeleccionarPorFiltro(ByVal strDescripcion As String, ByVal strSituacion As String) As DataSet
        Dim parameters As Object() = {strDescripcion, strSituacion}
        Try

            Return New RolAprobadoresTraderDAM().SeleccionarPorFiltro(strDescripcion, strSituacion)
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

    Public Function SeleccionarPorFiltroDetalle(ByVal CodigoRolTrader As Decimal) As AprobadorTraderBE
        Dim parameters As Object() = {CodigoRolTrader}
        Try

            Return New RolAprobadoresTraderDAM().SeleccionarPorFiltroDetalle(CodigoRolTrader)
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

    Public Function Seleccionar(ByVal CodigoRolTrader As Decimal) As RolTraderBE
        Dim parameters As Object() = {CodigoRolTrader}

        Try
            Return New RolAprobadoresTraderDAM().Seleccionar(CodigoRolTrader)
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

    Public Function SeleccionarDetalle(ByVal CodigoRolTrader As Decimal, ByVal strCodigoUsuario As String, ByVal strCodigoRenta As String) As AprobadorTraderBE
        Try
            Return New RolAprobadoresTraderDAM().SeleccionarDetalle(CodigoRolTrader, strCodigoUsuario, strCodigoRenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region

#Region "Funciones transaccionales"

    Public Function Eliminar(ByVal decCodigoRolTrader As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decCodigoRolTrader, dataRequest}
        Try
            Dim oRolAprobadoresTraderDAM As New RolAprobadoresTraderDAM

            bolResult = oRolAprobadoresTraderDAM.Eliminar(decCodigoRolTrader, dataRequest)
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

    Public Function Insertar(ByVal oRolTraderBE As RolTraderBE, ByVal oAprobadorTraderBE As AprobadorTraderBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oRolTraderBE, dataRequest}
        Try
            Dim oRolAprobadoresTraderDAM As New RolAprobadoresTraderDAM

            bolResult = oRolAprobadoresTraderDAM.Insertar(oRolTraderBE, oAprobadorTraderBE, dataRequest)
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

    Public Function Modificar(ByVal oRolTraderBE As RolTraderBE, ByVal oAprobadorTraderBE As AprobadorTraderBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oRolAprobadoresTraderDAM As New RolAprobadoresTraderDAM
            bolResult = oRolAprobadoresTraderDAM.Modificar(oRolTraderBE, oAprobadorTraderBE, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return bolResult
    End Function

#End Region

#Region " /* Otras Funciones */ "
    Public Function ObtieneFechaActualizacionMovPersonal() As Decimal
        Try
            Return New RolAprobadoresTraderDAM().ObtieneFechaActualizacionMovPersonal()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'HDG OT 64926 20120320
    Public Function SeleccionarOperadores(ByVal strCodigoRenta As String) As DataSet
        Dim parameters As Object() = {strCodigoRenta}
        Try

            Return New RolAprobadoresTraderDAM().SeleccionarOperadores(strCodigoRenta)
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
#End Region

End Class

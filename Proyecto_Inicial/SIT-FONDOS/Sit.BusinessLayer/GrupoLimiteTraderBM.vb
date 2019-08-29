'Creado por: HDG OT 64291 20111202
Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy

Public Class GrupoLimiteTraderBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Sub InicializarGrupoLimiteTrader(ByRef oRow As GrupoLimiteTraderBE.GrupoLimiteTraderRow)
        Try
            Dim daGrupoLimiteTrader As New GrupoLimiteTraderDAM
            daGrupoLimiteTrader.InicializarGrupoLimiteTrader(oRow)
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

    Public Sub InicializarGrupoLimiteTraderDetalle(ByRef oRowD As GrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalleRow)
        Try
            Dim daGrupoLimiteTraderDetalle As New GrupoLimiteTraderDAM
            daGrupoLimiteTraderDetalle.InicializarGrupoLimiteTraderDetalle(oRowD)
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
    Public Function ListarGrupoLimite(ByVal CodigoRenta As String) As GrupoLimiteTraderBE
        Dim parameters As Object() = {CodigoRenta}

        Try
            Return New GrupoLimiteTraderDAM().ListarGrupoLimite(CodigoRenta)
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

            Return New GrupoLimiteTraderDAM().SeleccionarPorFiltro(strDescripcion, strSituacion)
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

    Public Function SeleccionarPorFiltroDetalle(ByVal codigoGrupLimTrader As Decimal, ByVal strTipo As String) As GrupoLimiteTraderDetalleBE
        Dim parameters As Object() = {codigoGrupLimTrader, strTipo}
        Try

            Return New GrupoLimiteTraderDAM().SeleccionarPorFiltroDetalle(codigoGrupLimTrader, strTipo)
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

    Public Function Seleccionar(ByVal codigoGrupLimTrader As Decimal) As GrupoLimiteTraderBE
        Dim parameters As Object() = {codigoGrupLimTrader}

        Try
            Return New GrupoLimiteTraderDAM().Seleccionar(codigoGrupLimTrader)
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

    Public Function SeleccionarDetalle(ByVal codigoGrupLimTrader As Decimal, ByVal strValor As String) As GrupoLimiteTraderDetalleBE
        Dim parameters As Object() = {codigoGrupLimTrader, strValor}

        Try
            Return New GrupoLimiteTraderDAM().SeleccionarDetalle(codigoGrupLimTrader, strValor)
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

#Region "Funciones transaccionales"

    Public Function Eliminar(ByVal decCodigoGrupLimTrader As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {decCodigoGrupLimTrader, dataRequest}
        Try
            Dim oGrupoLimiteTraderDAM As New GrupoLimiteTraderDAM

            bolResult = oGrupoLimiteTraderDAM.Eliminar(decCodigoGrupLimTrader, dataRequest)
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

    Public Function Insertar(ByVal oGrupoLimiteTraderBE As GrupoLimiteTraderBE, ByVal oGrupoLimiteTraderDetalleBE As GrupoLimiteTraderDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oGrupoLimiteTraderBE, dataRequest}
        Try
            Dim oGrupoLimiteTraderDAM As New GrupoLimiteTraderDAM

            bolResult = oGrupoLimiteTraderDAM.Insertar(oGrupoLimiteTraderBE, oGrupoLimiteTraderDetalleBE, dataRequest)
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

    Public Function Modificar(ByVal oGrupoLimiteTraderBE As GrupoLimiteTraderBE, ByVal oGrupoLimiteTraderDetalleBE As GrupoLimiteTraderDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oGrupoLimiteTraderBE, dataRequest}
        Try
            Dim oGrupoLimiteTraderDAM As New GrupoLimiteTraderDAM

            bolResult = oGrupoLimiteTraderDAM.Modificar(oGrupoLimiteTraderBE, oGrupoLimiteTraderDetalleBE, dataRequest)
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

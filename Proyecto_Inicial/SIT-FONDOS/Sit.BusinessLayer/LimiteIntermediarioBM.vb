'Creado por: HDG OT 64926 20120320
Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class LimiteIntermediarioBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oLimiteIntermediarioBE As LimiteIntermediarioBE, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oLimiteIntermediarioBE, dataRequest}

        Try
            Dim oLimiteIntermediarioDAM As New LimiteIntermediarioDAM

            oLimiteIntermediarioDAM.Insertar(oLimiteIntermediarioBE, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoTercero As String, ByVal situacion As String, ByVal dataRequest As DataSet) As LimiteIntermediarioBE

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTercero, situacion, dataRequest}

        Try
            RegistrarAuditora(parameters)

            Return New LimiteIntermediarioDAM().SeleccionarPorFiltro(codigoTercero, situacion)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function
    Public Function Seleccionar(ByVal CodigoLimInter As String, ByVal dataRequest As DataSet) As LimiteIntermediarioBE

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoLimInter, dataRequest}

        Try
            RegistrarAuditora(parameters)

            Return New LimiteIntermediarioDAM().Seleccionar(CodigoLimInter)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function Modificar(ByVal oLimiteIntermediarioBE As LimiteIntermediarioBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oLimiteIntermediarioBE, dataRequest}

        Dim oLimiteIntermediarioDAM As New LimiteIntermediarioDAM

        Try

            actualizado = oLimiteIntermediarioDAM.Modificar(oLimiteIntermediarioBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado

    End Function

    Public Function Eliminar(ByVal CodigoLimInter As String, ByVal dataRequest As DataSet) As Boolean

        Dim eliminado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoLimInter, dataRequest}
        Dim oLimiteIntermediarioDAM As New LimiteIntermediarioDAM

        Try

            eliminado = oLimiteIntermediarioDAM.Eliminar(CodigoLimInter, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado

    End Function

    Public Function GenerarReporteOperacionesNegociadas(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal tipoRenta As String, ByVal codigoUsuario As String) As DataSet
        Try
            Return New LimiteIntermediarioDAM().GenerarReporteOperacionesNegociadas(fechaInicio, fechaFin, tipoRenta, codigoUsuario)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

End Class


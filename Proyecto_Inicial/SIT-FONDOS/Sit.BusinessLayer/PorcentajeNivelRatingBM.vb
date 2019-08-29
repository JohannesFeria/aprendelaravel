Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy

Public Class PorcentajeNivelRatingBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Sub InicializarPorcentajeNivelRating(ByRef oRow As PorcentajeNivelRatingBE.PorcentajeNivelRatingRow, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oRow, dataRequest}
        Try
            Dim oPorcentajeNivelRatingDAM As New PorcentajeNivelRatingDAM
            oPorcentajeNivelRatingDAM.InicializarPorcentajeNivelRating(oRow)
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
    End Sub

#Region "Funciones no transaccionales"
    Public Function SeleccionarPorFiltro(ByVal oPorcentajeNivelRatingBE As PorcentajeNivelRatingBE, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPorcentajeNivelRatingBE, dataRequest}
        Try
            Return New PorcentajeNivelRatingDAM().SeleccionarPorFiltro(oPorcentajeNivelRatingBE)
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

    Public Function SeleccionarCategoriaInversiones(ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New PorcentajeNivelRatingDAM().SeleccionarCategoriaInversiones()
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
    Public Function Insertar(ByVal oPorcentajeNivelRatingBE As PorcentajeNivelRatingBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPorcentajeNivelRatingBE, dataRequest}
        Try
            Dim oPorcentajeNivelRatingDAM As New PorcentajeNivelRatingDAM

            bolResult = oPorcentajeNivelRatingDAM.Insertar(oPorcentajeNivelRatingBE, dataRequest)
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

    Public Function Modificar(ByVal oPorcentajeNivelRatingBE As PorcentajeNivelRatingBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPorcentajeNivelRatingBE, dataRequest}
        Try
            Dim oPorcentajeNivelRatingDAM As New PorcentajeNivelRatingDAM

            bolResult = oPorcentajeNivelRatingDAM.Modificar(oPorcentajeNivelRatingBE, dataRequest)
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

    Public Function Eliminar(ByVal oPorcentajeNivelRatingBE As PorcentajeNivelRatingBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPorcentajeNivelRatingBE, dataRequest}
        Try
            Dim oPorcentajeNivelRatingDAM As New PorcentajeNivelRatingDAM

            bolResult = oPorcentajeNivelRatingDAM.Eliminar(oPorcentajeNivelRatingBE, dataRequest)
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

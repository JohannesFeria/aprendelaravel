Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class GrupoEconomicoBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltro(ByVal codigoGrupoEconomico As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As GrupoEconomicoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoGrupoEconomico, descripcion, situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New GrupoEconomicoDAM().SeleccionarPorFiltro(codigoGrupoEconomico, descripcion, situacion, dataRequest)
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
    Public Function Listar(ByVal dataRequest As DataSet) As GrupoEconomicoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New GrupoEconomicoDAM().Listar(dataRequest)
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
#End Region

    Public Function Insertar(ByVal oGrupoEconomico As GrupoEconomicoBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oGrupoEconomico, dataRequest}
        Try
            Dim oGrupoEconomicoDAM As New GrupoEconomicoDAM
            oGrupoEconomicoDAM.Insertar(oGrupoEconomico, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Modificar(ByVal oGrupoEconomico As GrupoEconomicoBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oGrupoEconomico, dataRequest}
        Try
            Dim oGrupoEconomicoDAM As New GrupoEconomicoDAM
            actualizado = oGrupoEconomicoDAM.Modificar(oGrupoEconomico, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado
    End Function

    Public Function Eliminar(ByVal codigoGrupoEconomico As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoGrupoEconomico, dataRequest}
        Try
            Dim oGrupoEconomicoDAM As New GrupoEconomicoDAM
            eliminado = oGrupoEconomicoDAM.Eliminar(codigoGrupoEconomico, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function

    Public Function EliminarPorCodigoEmisor(ByVal codigoEmisor As String, ByVal dataRequest As DataSet) As Boolean


    End Function
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
End Class


Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class MotivoBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal codigoMotivo As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As MotivoBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMotivo, descripcion, dataRequest}

        Try
            RegistrarAuditora(parameters)
            Return New MotivoDAM().SeleccionarPorFiltro(codigoMotivo, descripcion, situacion)
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

    Public Function Listar(ByVal dataRequest As DataSet) As MotivoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New MotivoDAM().Listar(dataRequest)
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

    Public Function Seleccionar(ByVal codigoMotivo As String, ByVal dataRequest As DataSet) As MotivoBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New MotivoDAM().Seleccionar(codigoMotivo)
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

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oMotivoBE As MotivoBE, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMotivoBE, dataRequest}
        Try
            Dim oMotivoDAM As New MotivoDAM
            oMotivoDAM.Insertar(oMotivoBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

#End Region

#Region " /* Funciones Modificar */ "

    Public Function Modificar(ByVal oMotivoBE As MotivoBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMotivoBE, dataRequest}
        Try
            Dim oMotivoDAM As New MotivoDAM
            actualizado = oMotivoDAM.Modificar(oMotivoBE, dataRequest)
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

#End Region

#Region " /* Funciones Eliminar */ "

    Public Function Eliminar(ByVal codigoMotivo As String, ByVal dataRequest As DataSet) As Boolean

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMotivo, dataRequest}
        Try
            Dim oMotivoDAM As New MotivoDAM
            eliminado = oMotivoDAM.Eliminar(codigoMotivo, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado

    End Function

#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub Ingresar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Salir(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
#End Region

End Class
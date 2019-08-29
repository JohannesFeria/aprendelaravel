Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class GrupoTipoInstrumentoBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal GrupoInstrumento As String, ByVal Descripcion As String, ByVal Situacion As String, ByVal Tipo As String, ByVal dataRequest As DataSet) As GrupoTipoInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {GrupoInstrumento, Descripcion, Situacion, Tipo, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New GrupoTipoInstrumentoDAM().SeleccionarPorFiltro(GrupoInstrumento, Descripcion, Situacion, Tipo, dataRequest)
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


    Public Function Listar(ByVal dataRequest As DataSet) As GrupoTipoInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New GrupoTipoInstrumentoDAM().Listar(dataRequest)
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

    Public Function Insertar(ByVal oGrupoTipoInstrumento As GrupoTipoInstrumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oGrupoTipoInstrumento, dataRequest}
        Try
            Dim oGrupoTipoInstrumentoDAM As New GrupoTipoInstrumentoDAM
            oGrupoTipoInstrumentoDAM.Insertar(oGrupoTipoInstrumento, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function


    Public Function Modificar(ByVal oGrupoTipoInstrumento As GrupoTipoInstrumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oGrupoTipoInstrumento, dataRequest}
        Try
            Dim oGrupoTipoInstrumentoDAM As New GrupoTipoInstrumentoDAM
            actualizado = oGrupoTipoInstrumentoDAM.Modificar(oGrupoTipoInstrumento, dataRequest)
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


    Public Function Eliminar(ByVal GrupoInstrumento As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {GrupoInstrumento, dataRequest}
        Try
            Dim oGrupoTipoInstrumentoDAM As New GrupoTipoInstrumentoDAM
            eliminado = oGrupoTipoInstrumentoDAM.Eliminar(GrupoInstrumento, dataRequest)
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


    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
End Class

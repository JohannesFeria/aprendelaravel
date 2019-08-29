Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

    Public Class TipoDocumentoBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function Seleccionar(ByVal codigoTipoDocumento As String, ByVal dataRequest As DataSet) As TipoDocumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoDocumento, dataRequest}
        Try

            Return New TipoDocumentoDAM().Seleccionar(codigoTipoDocumento, dataRequest)
            RegistrarAuditora(parameters)

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
    Public Function Listar(ByVal dataRequest As DataSet) As TipoDocumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New TipoDocumentoDAM().Listar(dataRequest)
            RegistrarAuditora(parameters)

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
    Public Function SeleccionarPorFiltro(ByVal codigoTipoDocumento As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoDocumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoDocumento, descripcion, situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New TipoDocumentoDAM().SeleccionarPorFiltro(codigoTipoDocumento, descripcion, situacion, dataRequest)
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
    Public Function Insertar(ByVal oTipoDocumento As TipoDocumentoBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oTipoDocumento, dataRequest}
        Try
            Dim oTipoDocumentoDAM As New TipoDocumentoDAM
            oTipoDocumentoDAM.Insertar(oTipoDocumento, dataRequest)
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
    Public Function Modificar(ByVal oTipoDocumento As TipoDocumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oTipoDocumento, dataRequest}
        Try
            Dim oTipoDocumentoDAM As New TipoDocumentoDAM
            actualizado = oTipoDocumentoDAM.Modificar(oTipoDocumento, dataRequest)
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
    Public Function Eliminar(ByVal codigoTipoDocumento As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoDocumento, dataRequest}
        Try
            Dim oTipoDocumentoDAM As New TipoDocumentoDAM
            eliminado = oTipoDocumentoDAM.Eliminar(codigoTipoDocumento, dataRequest)
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
#End Region
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
End Class


Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class FactorBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal TipoFactor As String, ByVal CodigoMnemonico As String, ByVal CodigoEntidad As String, ByVal Situacion As String, ByVal GrupoFactor As String, ByVal dataRequest As DataSet) As FactorBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {TipoFactor, CodigoMnemonico, CodigoEntidad, Situacion, GrupoFactor, dataRequest}
        Try
            RegistrarAuditora(parameters)
            'CMB 20101028 - REQ 30 - OT 61566
            Return New FactorDAM().SeleccionarPorFiltro(TipoFactor, CodigoMnemonico, CodigoEntidad, Situacion, GrupoFactor, dataRequest)
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

    Public Function Listar(ByVal dataRequest As DataSet) As FactorBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New FactorDAM().Listar(dataRequest)
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

    Public Function Insertar(ByVal oFactor As FactorBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oFactor, dataRequest}
        Try
            Dim oFactorDAM As New FactorDAM
            oFactorDAM.Insertar(oFactor, dataRequest)
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

    Public Function Modificar(ByVal oFactor As FactorBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oFactor, dataRequest}
        Try
            Dim oFactorDAM As New FactorDAM
            actualizado = oFactorDAM.Modificar(oFactor, dataRequest)
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

    Public Function Eliminar(ByVal TipoFactor As String, ByVal CodigoEntidad As String, ByVal CodigoMnemonico As String, ByVal GrupoFactor As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {TipoFactor, CodigoEntidad, CodigoMnemonico, dataRequest}
        Try
            Dim oFactorDAM As New FactorDAM
            'CMB 20101028 - REQ 30 - OT 61566
            eliminado = oFactorDAM.Eliminar(TipoFactor, CodigoEntidad, CodigoMnemonico, GrupoFactor, dataRequest)
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

    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub

    'INI CMB OT 61566 20101102
    Public Function ActualizarFactorPorExcel(ByVal dtFactor As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim Codigo = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtFactor, dataRequest}
        Try
            Dim oFactorDAM As New FactorDAM
            Codigo = oFactorDAM.ActualizarFactorPorExcel(dtFactor, dataRequest, strmensaje)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            Dim sms As String = ex.Message
            Dim src As String = ex.Source
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function
    'FIN CMB OT 61566 20101102
End Class

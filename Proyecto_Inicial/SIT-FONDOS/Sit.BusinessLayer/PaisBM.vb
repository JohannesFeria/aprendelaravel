Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

''' Clase para el acceso de los datos para Pais tabla.
Public Class PaisBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltro(ByVal codigoPais As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As PaisBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPais, descripcion, situacion, dataRequest}
        Try
            Dim oPaisDAM As New PaisDAM
            Dim ds As DataSet = oPaisDAM.SeleccionarPorFiltro(codigoPais, descripcion, situacion, dataRequest)
            RegistrarAuditora(parameters)
            Return ds
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As PaisBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New PaisDAM().Listar(dataRequest)
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
#End Region

#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal oPais As PaisBE, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPais, dataRequest}
        Try
            Dim oPaisDAM As New PaisDAM
            oPaisDAM.Insertar(oPais, dataRequest)
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
    Public Function Modificar(ByVal oPais As PaisBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPais, dataRequest}
        Try
            Dim oPaisDAM As New PaisDAM
            actualizado = oPaisDAM.Modificar(oPais, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'RegistrarAuditora(parameters, ex)
            'Dim rethrow As Boolean = true
            'If (rethrow) Then
            '    Throw
            'End If
        End Try
        Return actualizado

    End Function
#End Region

#Region " /* Funciones Eliminar */ "
    Public Function Eliminar(ByVal codigoPais As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPais, dataRequest}
        Try
            Dim oPaisDAM As New PaisDAM
            eliminado = oPaisDAM.Eliminar(codigoPais, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            'Dim rethrow As Boolean = true
            'If (rethrow) Then
            'Throw
            'End If
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


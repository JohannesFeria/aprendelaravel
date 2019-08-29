Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Clase para el acceso de los datos para CentroCostos tabla.
''' </summary>
Public Class CentroCostosBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "
    
    Public Function SeleccionarPorFiltros(ByVal codigo As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As CentroCostosBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigo, descripcion, dataRequest}
        Dim oCentroCostoBE As CentroCostosBE

        Try

            oCentroCostoBE = New CentroCostosDAM().SeleccionarPorFiltros(codigo, descripcion, dataRequest)
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

        Return oCentroCostoBE

    End Function

    Public Function Seleccionar(ByVal codigoCentroCostos As String, ByVal dataRequest As DataSet) As CentroCostosBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCentroCostos, dataRequest}
        Try

            Return New CentroCostosDAM().Seleccionar(codigoCentroCostos, dataRequest)
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

    Public Function Listar(ByVal dataRequest As DataSet) As CentroCostosBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New CentroCostosDAM().Listar(dataRequest)
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
    ''' <summary>
    ''' Inserta un expediente en CentroCostos tabla.
    ''' <summary>
    ''' <param name="ob">CentroCostosBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal oCentroCostosBE As CentroCostosBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCentroCostosBE, dataRequest}
        Try
            Dim oCentroCostosDAM As New CentroCostosDAM

            codigo = oCentroCostosDAM.Insertar(oCentroCostosBE, dataRequest)
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

        Return codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"

    ''' <summary>
    ''' Midifica un expediente en CentroCostos tabla.
    ''' <summary>
    '' <param name="ob">CentroCostosBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal oCentroCostosBE As CentroCostosBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCentroCostosBE, dataRequest}
        Try
            Dim oCentroCostosDAM As New CentroCostosDAM

            actualizado = oCentroCostosDAM.Modificar(oCentroCostosBE, dataRequest)
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

        Return actualizado

    End Function

#End Region

#Region " /* Funciones Eliminar */"
    ''' <summary>
    ''' Elimina un expediente de CentroCostos table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoCentroCostos">Decimal</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoCentroCostos As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCentroCostos, dataRequest}
        Try
            Dim oCentroCostosDAM As New CentroCostosDAM

            eliminado = oCentroCostosDAM.Eliminar(codigoCentroCostos, dataRequest)
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
        Return eliminado
    End Function
#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub Salir(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Ingresar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
#End Region

End Class


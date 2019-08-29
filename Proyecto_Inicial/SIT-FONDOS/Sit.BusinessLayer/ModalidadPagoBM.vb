Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities



''' Clase para el acceso de los datos para ModalidadPago tabla.
Public Class ModalidadPagoBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub

#Region " /* Funciones Seleccionar */ "
    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de ModalidadPago tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="situacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ModalidadPagoBE</returns>
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ModalidadPagoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, situacion, dataRequest}
        Try

            Return New ModalidadPagoDAM().SeleccionarPorFiltros(descripcion, situacion, dataRequest)
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
    ''' <summary>
    ''' Selecciona un solo expediente de ModalidadPago tabla.
    ''' <summary>
    ''' <param name="codigoModalidadPago">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ModalidadPagoBE</returns>
    Public Function Seleccionar(ByVal codigoModalidadPago As String, ByVal dataRequest As DataSet) As ModalidadPagoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoModalidadPago, dataRequest}
        Try

            Return New ModalidadPagoDAM().Seleccionar(codigoModalidadPago, dataRequest)
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

    ''' <summary>
    ''' Lista todos los expedientes de ModalidadPago tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ModalidadPagoBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet, Optional ByVal situacion As String = "") As ModalidadPagoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest, situacion}
        Try
            Return New ModalidadPagoDAM().Listar(dataRequest, situacion)
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
    ''' Inserta un expediente en ModalidadPago tabla.
    ''' <summary>
    ''' <param name="ob">ModalidadPagoBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal oModalidadPagoBE As ModalidadPagoBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oModalidadPagoBE, dataRequest}
        Try
            Dim oTipoCuponDAM As New ModalidadPagoDAM

            codigo = oTipoCuponDAM.Insertar(oModalidadPagoBE, dataRequest)
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
    ''' Midifica un expediente en ModalidadPago tabla.
    ''' <summary>
    ''' <param name="ob">ModalidadPagoBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal oModalidadPagoBE As ModalidadPagoBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oModalidadPagoBE, dataRequest}
        Try
            Dim oTipoCuponDAM As New ModalidadPagoDAM

            actualizado = oTipoCuponDAM.Modificar(oModalidadPagoBE, dataRequest)
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
    ''' Elimina un expediente de ModalidadPago table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoModalidadPago">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoTipoCupon As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoCupon, dataRequest}
        Try
            Dim oTipoCuponDAM As New ModalidadPagoDAM

            eliminado = oTipoCuponDAM.Eliminar(codigoTipoCupon, dataRequest)
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


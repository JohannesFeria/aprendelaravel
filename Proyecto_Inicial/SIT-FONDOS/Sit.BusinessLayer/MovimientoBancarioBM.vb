Imports System
Imports System.Data
Imports System.Data.Common
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports MotorTransaccionesProxy

''' Clase para el acceso de los datos para MovimientoBancario tabla.
Public Class MovimientoBancarioBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub


#Region " /* Funciones Seleccionar */ "

    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de MovimientoBancario tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="situacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>MovimientoBancarioBE</returns>
    Public Function SeleccionarPorFiltros(ByVal situacion As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As MovimientoBancarioBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, situacion, dataRequest}
        Try

            Return New MovimientoBancarioDAM().SeleccionarPorFiltros(descripcion, situacion, dataRequest)
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
    ''' Selecciona un solo expediente de MovimientoBancario tabla.
    ''' <summary>
    ''' <param name="codigoMovimiento">String</param>
    ''' <returns>MovimientoBancarioBE</returns>
    Public Function Seleccionar(ByVal codigoMovimiento As String, ByVal dataRequest As DataSet) As MovimientoBancarioBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMovimiento, dataRequest}
        Try

            Return New MovimientoBancarioDAM().Seleccionar(codigoMovimiento, dataRequest)
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
    ''' Lista todos los expedientes de MovimientoBancario tabla.
    ''' <summary>
    ''' <returns>MovimientoBancarioBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As MovimientoBancarioBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New MovimientoBancarioDAM().Listar(dataRequest)
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
    ''' Inserta un expediente en MovimientoBancario tabla.
    ''' <summary>
    ''' <param name="ob">MovimientoBancarioBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal oMovimientoBancarioBE As MovimientoBancarioBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMovimientoBancarioBE, dataRequest}
        Try
            Dim oMovimientoBancarioDAM As New MovimientoBancarioDAM

            codigo = oMovimientoBancarioDAM.Insertar(oMovimientoBancarioBE, dataRequest)
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
    ''' Midifica un expediente en MovimientoBancario tabla.
    ''' <summary>
    ''' <param name="oMovimientoBancarioBE">MovimientoBancarioBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal oMovimientoBancarioBE As MovimientoBancarioBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMovimientoBancarioBE, dataRequest}
        Try
            Dim oMovimientoBancarioDAM As New MovimientoBancarioDAM

            actualizado = oMovimientoBancarioDAM.Modificar(oMovimientoBancarioBE, dataRequest)
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
    Public Function Extornar(ByVal CodigoOperacionCaja As String, ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoOperacionCaja, dataRequest}
        Try
            Dim oMovimientoBancarioDAM As New MovimientoBancarioDAM
            actualizado = oMovimientoBancarioDAM.Extornar(CodigoOperacionCaja, codigoPortafolio, dataRequest)
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

#Region " /* Funciones Eliminar */"
        ''' <summary>
        ''' Elimina un expediente de MovimientoBancario table por una llave primaria compuesta.
        ''' <summary>
        ''' <param name="codigoMovimiento">String</param>
        ''' <param name="dataRequest">DataSet</param>
        ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoMovimiento As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMovimiento, dataRequest}
        Try
            Dim oMovimientoBancarioDAM As New MovimientoBancarioDAM

            eliminado = oMovimientoBancarioDAM.Eliminar(codigoMovimiento, dataRequest)
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


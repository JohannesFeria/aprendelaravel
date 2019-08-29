Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Clase para el acceso de los datos para TipoOperacion tabla.
''' </summary>
Public Class TipoOperacionBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub



#Region " /* Funciones Seleccionar */ "

    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de TipoOperacion tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="situacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>TipoOperacionBE</returns>
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoOperacionBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, Situacion, dataRequest}
        Try

            Return New TipoOperacionDAM().SeleccionarPorFiltros(descripcion, situacion, dataRequest)
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
    Public Function SeleccionarPorFiltros(ByVal codigo As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoOperacionBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, situacion, dataRequest}
        Try
            Dim oTipoOperacionBE As TipoOperacionBE
            oTipoOperacionBE = New TipoOperacionDAM().SeleccionarPorFiltros(codigo, descripcion, situacion, dataRequest)
            RegistrarAuditora(parameters)
            Return oTipoOperacionBE
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
    ''' <summary>
    ''' Selecciona un solo expediente de TipoOperacion tabla.
    ''' <summary>
    ''' <param name="codigoTipoOperacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>TipoOperacionBE</returns>
    Public Function Seleccionar(ByVal codigoTipoOperacion As String, ByVal dataRequest As DataSet) As TipoOperacionBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoOperacion, dataRequest}
        Try

            Return New TipoOperacionDAM().Seleccionar(codigoTipoOperacion, dataRequest)
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

    ''' <summary>
    ''' Lista todos los expedientes de TipoOperacion tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>TipoOperacionBE</returns>
    Public Function Listar(Optional ByVal situacion As String = "", Optional ByVal egreso As String = "") As TipoOperacionBE
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {dataRequest}
        Try

            'Return New TipoOperacionDAM().Listar(situacion)
            'RegistrarAuditora(parameters)
            Dim oTipoOperacionDAM As New TipoOperacionDAM
            Return oTipoOperacionDAM.Listar(situacion, egreso)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            ' RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#End Region

#Region " /* Funciones Insertar */ "
    ''' <summary>
    ''' Inserta un expediente en TipoOperacion tabla.
    ''' <summary>
    ''' <param name="ob">TipoOperacionBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal oTipoOperacionBE As TipoOperacionBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oTipoOperacionBE, dataRequest}
        Try
            Dim oTipoCuponDAM As New TipoOperacionDAM

            codigo = oTipoCuponDAM.Insertar(oTipoOperacionBE, dataRequest)
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

        Return codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"

    ''' <summary>
    ''' Midifica un expediente en TipoOperacion tabla.
    ''' <summary>
    ''' <param name="ob">TipoOperacionBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal oTipoOperacionBE As TipoOperacionBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oTipoOperacionBE, dataRequest}
        Try
            Dim oTipoCuponDAM As New TipoOperacionDAM

            actualizado = oTipoCuponDAM.Modificar(oTipoOperacionBE, dataRequest)
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

        Return actualizado

    End Function

#End Region

#Region " /* Funciones Eliminar */"
    ''' <summary>
    ''' Elimina un expediente de TipoOperacion table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoTipoOperacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoTipoCupon As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoCupon, dataRequest}
        Try
            Dim oTipoCuponDAM As New TipoOperacionDAM

            eliminado = oTipoCuponDAM.Eliminar(codigoTipoCupon, dataRequest)
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

    Public Function SeleccionarporClaseinstrumento(ByVal clase As String, ByVal situacion As String) As DataSet
        Dim objtipooperacion As New TipoOperacionDAM
        Dim dstipooperacion As New DataSet
        Try
            dstipooperacion = objtipooperacion.SeleccionarporClaseinstrumento(clase, situacion)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception

            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dstipooperacion
    End Function





End Class


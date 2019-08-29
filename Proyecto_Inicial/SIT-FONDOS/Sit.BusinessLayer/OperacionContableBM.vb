Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


    ''' Clase para el acceso de los datos para OperacionContable tabla.
Public Class OperacionContableBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "
    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de OperacionContable tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="codigoOperacionContable">String</param>
    ''' <param name="descripcion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>OperacionContableBE</returns>
    Public Function SeleccionarPorFiltros(ByVal codigoOperacionContable As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As OperacionContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoOperacionContable, descripcion, situacion, dataRequest}
        Try
            Dim oOperacionContableBE As OperacionContableBE
            oOperacionContableBE = New OperacionContableDAM().SeleccionarPorFiltros(codigoOperacionContable, descripcion, situacion, dataRequest)
            RegistrarAuditora(parameters)
            Return oOperacionContableBE
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
    ''' Selecciona un solo expediente de OperacionContable tabla.
    ''' <summary>
    ''' <param name="codigoOperacionContable">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>OperacionContableBE</returns>
    Public Function Seleccionar(ByVal codigoOperacionContable As String, ByVal dataRequest As DataSet) As OperacionContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoOperacionContable, dataRequest}
        Try

            Return New OperacionContableDAM().Seleccionar(codigoOperacionContable, dataRequest)
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
    ''' Lista todos los expedientes de OperacionContable tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>OperacionContableBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As OperacionContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New OperacionContableDAM().Listar(dataRequest)
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
    ''' Inserta un expediente en OperacionContable tabla.
    ''' <summary>
    ''' <param name="ob">OperacionContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal oOperacionContableBE As OperacionContableBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oOperacionContableBE, dataRequest}
        Try
            Dim oTipoCuponDAM As New OperacionContableDAM

            codigo = oTipoCuponDAM.Insertar(oOperacionContableBE, dataRequest)
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
    ''' Midifica un expediente en OperacionContable tabla.
    ''' <summary>
    ''' <param name="ob">OperacionContableBE</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal oOperacionContableBE As OperacionContableBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oOperacionContableBE, dataRequest}
        Try
            Dim oTipoCuponDAM As New OperacionContableDAM

            actualizado = oTipoCuponDAM.Modificar(oOperacionContableBE, dataRequest)
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
    ''' Elimina un expediente de OperacionContable table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoOperacionContable">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoOperacionContable As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoOperacionContable, dataRequest}
        Try
            Dim oTipoCuponDAM As New OperacionContableDAM

            eliminado = oTipoCuponDAM.Eliminar(codigoOperacionContable, dataRequest)
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


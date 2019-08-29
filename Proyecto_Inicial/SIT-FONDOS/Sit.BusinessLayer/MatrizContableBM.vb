Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
''' Clase para el acceso de los datos para MatrizContable tabla.
Public Class MatrizContableBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal MatrizFondo As String) As MatrizContableBE
        Try
            Return New MatrizContableDAM().SeleccionarPorFiltros(descripcion, situacion, MatrizFondo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' <summary>
    ''' Selecciona un solo expediente de MatrizContable tabla.
    ''' <summary>
    ''' <param name="codigoMatriz">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>MatrizContableBE</returns>
    Public Function Seleccionar(ByVal codigoMatrizContable As Decimal, ByVal dataRequest As DataSet) As MatrizContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMatrizContable, dataRequest}
        Try
            Return New MatrizContableDAM().Seleccionar(codigoMatrizContable, dataRequest)
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
    ''' Lista todos los expedientes de MatrizContable tabla.
    ''' <summary>
    ''' <param name="codigoMatriz">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>MatrizContableBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As MatrizContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim MatrizcontableBE As New MatrizcontableBE
        Dim MatrizcontableDAM As New MatrizcontableDAM
        Try

            MatrizcontableBE = MatrizcontableDAM.Listar(dataRequest)
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
        Return MatrizcontableBE
    End Function
    ''' <summary>
    ''' Lista todos los expedientes de MatrizContable tabla y devuelve todo incluso la tabla matriz.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>MatrizContableBE</returns>
    Public Function ListarTablaMatriz(ByVal dataRequest As DataSet) As MatrizContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim MatrizcontableBE As New MatrizcontableBE
        Dim MatrizcontableDAM As New MatrizcontableDAM
        Try
            MatrizcontableBE = MatrizcontableDAM.ListarTablaMatriz(dataRequest)
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
        Return MatrizcontableBE
    End Function
    ''' <summary>
    ''' Obtener el codigo segun la descripcion proporcionada.
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>MatrizContableBE</returns>
    Public Function SeleccionarCodigo(ByVal descripcion As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, dataRequest}
        Dim MatrizcontableBE As New DataSet
        Dim MatrizcontableDAM As New MatrizcontableDAM
        Try
            MatrizcontableBE = MatrizcontableDAM.SeleccionarCodigo(descripcion, dataRequest)
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
        Return MatrizcontableBE
    End Function
#End Region
#Region " /* Funciones Insertar */ "
    ''' <summary>
    ''' Inserta un expediente en MatrizContable tabla.
    ''' <summary>
    ''' <param name="oMatrizContableBE">MatrizContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal oMatrizContableBE As MatrizContableBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMatrizContableBE, dataRequest}
        Try
            Dim oMatrizContableDAM As New MatrizContableDAM

            codigo = oMatrizContableDAM.Insertar(oMatrizContableBE, dataRequest)
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
    ''' <summary>
    ''' Inserta un expediente en MatrizContable tabla sin ingresar el codigo de la matriz contable.
    ''' <summary>
    ''' <param name="oMatrizContableBE">MatrizContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar_1(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, situacion, dataRequest}
        Dim oMatrizContableDAM As New MatrizContableDAM
        Try
            codigo = oMatrizContableDAM.Insertar_1(descripcion, situacion, dataRequest)
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
    ''' Midifica un expediente en MatrizContable tabla.
    ''' <summary>
    ''' <param name="oMatrizContableBE">MatrizContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal oMatrizContableBE As MatrizContableBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMatrizContableBE, dataRequest}
        Try
            Dim oTipoCuponDAM As New MatrizContableDAM

            actualizado = oTipoCuponDAM.Modificar(oMatrizContableBE, dataRequest)
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
    ''' Elimina un expediente de MatrizContable table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoMatriz">Decimal</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoMatriz As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMatriz, dataRequest}
        Try
            Dim oTipoCuponDAM As New MatrizContableDAM

            eliminado = oTipoCuponDAM.Eliminar(codigoMatriz, dataRequest)
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
End Class
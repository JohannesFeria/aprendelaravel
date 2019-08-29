Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Clase para el acceso de los datos para ConceptoAsientoContable tabla.
''' </summary>
Public Class ConceptoAsientoContableBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub

#Region " /* Funciones Seleccionar */ "
    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de ConceptoAsientoContable tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="situacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoAsientoContableBE</returns>
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ConceptoAsientoContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, situacion, dataRequest}
        Try

            Return New ConceptoAsientoContableDAM().SeleccionarPorFiltros(descripcion, situacion, dataRequest)
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
    ''' Selecciona un solo expediente de ConceptoAsientoContable tabla.
    ''' <summary>
    ''' <param name="codigoAsientoContable">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoAsientoContableBE</returns>
    Public Function Seleccionar(ByVal codigoAsientoContable As String, ByVal dataRequest As DataSet) As ConceptoAsientoContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoAsientoContable, dataRequest}
        Try

            Return New ConceptoAsientoContableDAM().Seleccionar(codigoAsientoContable, dataRequest)
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
    ''' Lista todos los expedientes de ConceptoAsientoContable tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoAsientoContableBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As ConceptoAsientoContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New ConceptoAsientoContableDAM().Listar(dataRequest)
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
    ''' Inserta un expediente en ConceptoAsientoContable tabla.
    ''' <summary>
    ''' <param name="ob">ConceptoAsientoContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal oConceptoAsientoContableBE As ConceptoAsientoContableBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oConceptoAsientoContableBE, dataRequest}
        Try
            Dim oConceptoAsientoContableDAM As New ConceptoAsientoContableDAM

            codigo = oConceptoAsientoContableDAM.Insertar(oConceptoAsientoContableBE, dataRequest)
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
    ''' Midifica un expediente en ConceptoAsientoContable tabla.
    ''' <summary>
    ''' <param name="ob">ConceptoAsientoContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal oConceptoAsientoContableBE As ConceptoAsientoContableBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oConceptoAsientoContableBE, dataRequest}
        Try
            Dim oConceptoAsientoContableDAM As New ConceptoAsientoContableDAM

            actualizado = oConceptoAsientoContableDAM.Modificar(oConceptoAsientoContableBE, dataRequest)
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
    ''' Elimina un expediente de ConceptoAsientoContable table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoMovimiento">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoAsientoContable As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoAsientoContable, dataRequest}
        Try
            Dim oConceptoAsientoContableDAM As New ConceptoAsientoContableDAM

            eliminado = oConceptoAsientoContableDAM.Eliminar(codigoAsientoContable, dataRequest)
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


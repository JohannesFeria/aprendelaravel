Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


''' <summary>
''' Clase para el acceso de los datos para ConceptoOperacionContable tabla.
''' </summary>
Public Class ConceptoOperacionContableBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub

#Region " /* Funciones Seleccionar */ "
    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de ConceptoOperacionContable tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="situacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoOperacionContableBE</returns>
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ConceptoOperacionContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, situacion, dataRequest}
        Try

            Return New ConceptoOperacionContableDAM().SeleccionarPorFiltros(descripcion, situacion, dataRequest)
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
    ''' Selecciona un solo expediente de ConceptoOperacionContable tabla.
    ''' <summary>
    ''' <param name="codigoConceptoOperacionContable">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoOperacionContableBE</returns>
    Public Function Seleccionar(ByVal codigoConceptoOperacionContable As String, ByVal dataRequest As DataSet) As ConceptoOperacionContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoConceptoOperacionContable, dataRequest}
        Try

            Return New ConceptoOperacionContableDAM().Seleccionar(codigoConceptoOperacionContable, dataRequest)
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
    ''' Lista todos los expedientes de ConceptoOperacionContable tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoOperacionContableBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As ConceptoOperacionContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New ConceptoOperacionContableDAM().Listar(dataRequest)
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
    ''' Inserta un expediente en ConceptoOperacionContable tabla.
    ''' <summary>
    ''' <param name="ob">ConceptoOperacionContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal oConceptoOperacionContableBE As ConceptoOperacionContableBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oConceptoOperacionContableBE, dataRequest}
        Try
            Dim ConceptoOperacionContableDAM As New ConceptoOperacionContableDAM

            codigo = ConceptoOperacionContableDAM.Insertar(oConceptoOperacionContableBE, dataRequest)
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
    ''' Midifica un expediente en ConceptoOperacionContable tabla.
    ''' <summary>
    ''' <param name="ob">ConceptoOperacionContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal oConceptoOperacionContableBE As ConceptoOperacionContableBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oConceptoOperacionContableBE, dataRequest}
        Try
            Dim ConceptoOperacionContableDAM As New ConceptoOperacionContableDAM

            actualizado = ConceptoOperacionContableDAM.Modificar(oConceptoOperacionContableBE, dataRequest)
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
    ''' Elimina un expediente de MatrizContable table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoConceptoOperacionContable">Decimal</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoConceptoOperacionContable As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoConceptoOperacionContable, dataRequest}
        Try
            Dim oConceptoOperacionContableDAM As New ConceptoOperacionContableDAM

            eliminado = oConceptoOperacionContableDAM.Eliminar(codigoConceptoOperacionContable, dataRequest)
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


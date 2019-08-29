Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities


''' <summary>
''' Clase para el acceso de los datos para ClaseCuenta tabla.
''' </summary>
Public Class ClaseCuentaBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function Listar(ByVal dataRequest As DataSet) As ClaseCuentaBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New ClaseCuentaDAM().Listar(dataRequest)
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
    Public Function SeleccionarPorFiltro(ByVal codigoClaseCuenta As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ClaseCuentaBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoClaseCuenta, dataRequest}
        Dim oClaseCuentaDAM As New ClaseCuentaDAM

        Try

            RegistrarAuditora(parameters)

            Return oClaseCuentaDAM.SeleccionarPorFiltro(codigoClaseCuenta, descripcion, situacion)

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
    Public Function Insertar(ByVal oClaseCuentaBE As ClaseCuentaBE, ByVal dataRequest As DataSet) As String

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oClaseCuentaBE, dataRequest}
        Dim oClaseCuentaDAM As New ClaseCuentaDAM

        Try

            oClaseCuentaDAM.Insertar(oClaseCuentaBE, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de ClaseCuenta tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoClaseCuenta As String, ByVal dataRequest As DataSet) As ClaseCuentaBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoClaseCuenta, dataRequest}
        Dim oClaseCuentaDAM As New ClaseCuentaDAM

        Try

            RegistrarAuditora(parameters)

            Return oClaseCuentaDAM.Seleccionar(codigoClaseCuenta)

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
    ''' Lista todos los expedientes de ClaseCuenta tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As ClaseCuentaBE
        Try
            Return New ClaseCuentaDAM().Listar()
        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ClaseCuentaXBanco_Listar(ByVal sCodigoTercero As String) As ClaseCuentaBE
        Try
            Return New ClaseCuentaDAM().ClaseCuentaXBanco_Listar(sCodigoTercero)
        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Modificar(ByVal oClaseCuentaBE As ClaseCuentaBE, ByVal dataRequest As DataSet) As Boolean

        Dim blnActualizadoExito As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oClaseCuentaBE, dataRequest}
        Dim oClaseCuentaDAM As New ClaseCuentaDAM

        Try

            blnActualizadoExito = oClaseCuentaDAM.Modificar(oClaseCuentaBE, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return blnActualizadoExito

    End Function

    ''' <summary>
    ''' Elimina un expediente de ClaseCuenta table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoClaseCuenta As String, ByVal dataRequest As DataSet) As Boolean

        Dim blnEliminadoExito As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoClaseCuenta, dataRequest}
        Dim oClaseCuentaDAM As New ClaseCuentaDAM

        Try

            blnEliminadoExito = oClaseCuentaDAM.Eliminar(codigoClaseCuenta, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return blnEliminadoExito

    End Function
End Class


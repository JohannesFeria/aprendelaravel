Option Explicit On 
Option Strict On

Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class ContactoBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oContactoBE As ContactoBE, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oContactoBE, dataRequest}

        Try
            Dim oContactoDAM As New ContactoDAM

            oContactoDAM.Insertar(oContactoBE, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoContacto As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ContactoBE

        Dim oContactoDAM As New ContactoDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {codigoContacto, descripcion, situacion, dataRequest}

        Dim oContactoBE As ContactoBE

        Try

            intCodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)

            oContactoBE = oContactoDAM.SeleccionarPorFiltro(codigoContacto, descripcion, situacion)

            RegistrarAuditora(parameters)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If

        End Try

        Return oContactoBE

    End Function

    Public Function Seleccionar(ByVal codigoContacto As String, ByVal dataRequest As DataSet) As ContactoBE

        Dim oContactoDAM As New ContactoDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {codigoContacto, dataRequest}

        Dim oContactoBE As ContactoBE

        Try

            intCodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)

            oContactoBE = oContactoDAM.Seleccionar(codigoContacto)

            RegistrarAuditora(parameters)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If

        End Try

        Return oContactoBE

    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As ContactoBE

        Dim oContactoDAM As New ContactoDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {dataRequest}

        Dim oContactoBE As ContactoBE

        Try

            intCodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)

            oContactoBE = oContactoDAM.Listar()

            RegistrarAuditora(parameters)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If

        End Try

        Return oContactoBE

    End Function

    Public Function ListarActivos(ByVal dataRequest As DataSet) As ContactoBE

        Dim oContactoDAM As New ContactoDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {dataRequest}

        Dim oContactoBE As ContactoBE

        Try

            intCodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)

            oContactoBE = oContactoDAM.ListarActivos()

            RegistrarAuditora(parameters)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If

        End Try

        Return oContactoBE

    End Function

    Public Function ListarContactoPorTerceros(ByVal strCodigoTercero As String) As DataSet
        Try
            Dim oContactoDAM As New ContactoDAM
            Return oContactoDAM.ListarPorTercero(strCodigoTercero)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarContactoPorTercerosTesoreria(ByVal strCodigoTercero As String) As DataSet
        Try
            Dim oContactoDAM As New ContactoDAM
            Return oContactoDAM.ListarPorTerceroTesoreria(strCodigoTercero)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'RGF 20090708
    Public Function ListarPorCodigoEntidad(ByVal strCodigoEntidad As String) As DataSet
        Try
            Dim oContactoDAM As New ContactoDAM
            Return oContactoDAM.ListarPorCodigoEntidad(strCodigoEntidad)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Modificar(ByVal oContactoBE As ContactoBE, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oContactoBE, dataRequest}

        Try
            Dim oContactoDAM As New ContactoDAM

            oContactoDAM.Modificar(oContactoBE, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function Eliminar(ByVal codigoContacto As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoContacto, dataRequest}

        Try
            Dim oContactoDAM As New ContactoDAM

            oContactoDAM.Eliminar(codigoContacto, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarUltimoContactoEnUnaNegociacion(ByVal CodigoMNemonico As String, ByVal CodigoTercero As String) As String
        Dim oContactoDAM As New ContactoDAM
        Dim CodigoContacto As String
        Try
            CodigoContacto = oContactoDAM.SeleccionarUltimoContactoEnUnaNegociacion(CodigoMNemonico, CodigoTercero)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return CodigoContacto
    End Function

#Region " /* Funciones Personalizadas*/"
    Public Sub Ingresar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub

    Public Sub Salir(ByVal dataRequest As DataSet)
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
#End Region

End Class
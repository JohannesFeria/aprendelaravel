Option Explicit On 
Option Strict On

#Region "/* Imports */"

Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

#End Region

Public Class PersonalBM
    Inherits InvokerCOM

#Region "/* Constructor */"

    Public Sub New()

    End Sub

#End Region


#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal oPersonal As PersonalBE, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPersonal, dataRequest}
        Try
            Dim oPersonalDAM As New PersonalDAM
            oPersonalDAM.Insertar(oPersonal, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#End Region

#Region " /* Funciones Modificar */ "
    Public Function Modificar(ByVal oPersonal As PersonalBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPersonal, dataRequest}
        Try
            Dim oPersonalDAM As New PersonalDAM
            actualizado = oPersonalDAM.Modificar(oPersonal, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'RegistrarAuditora(parameters, ex)
            'Dim rethrow As Boolean = true
            'If (rethrow) Then
            '    Throw
            'End If
        End Try
        Return actualizado
    End Function
#End Region

#Region " /* Funciones Eliminar */ "
    Public Function Eliminar(ByVal codigoPersonal As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPersonal, dataRequest}
        Try
            Dim oPersonalDAM As New PersonalDAM
            eliminado = oPersonalDAM.Eliminar(codigoPersonal, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
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


    Public Function SeleccionarPorFiltro(ByVal codigoInterno As String, ByVal nombreCompleto As String, ByVal dataRequest As DataSet) As PersonalBE

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoInterno, nombreCompleto, dataRequest}
        Try

            Return New PersonalDAM().SeleccionarPorFiltro(codigoInterno, nombreCompleto)
            RegistrarAuditora(parameters)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarPorCodigoInterno(ByVal codigoInterno As String, ByVal dataRequest As DataSet) As String

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Try

            Return New PersonalDAM().SeleccionarPorCodigoInterno(codigoInterno)
        
        Catch ex As Exception
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    'RGF 20110505 OT 63063 REQ 01
    Public Function VerificaPermisoNegociacion(ByVal LoginUsuario As String) As Integer
        Try
            Return New PersonalDAM().VerificaPermisoNegociacion(LoginUsuario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'CMB OT 65473 20120625
    Public Function SeleccionarCodigoInterno(ByVal codigoUsuario As String, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Try

            Return New PersonalDAM().SeleccionarCodigoInterno(codigoUsuario)

        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function SeleccionarMail(ByVal codigoUsuario As String) As String

        Try

            Return New PersonalDAM().SeleccionarMail(codigoUsuario)

        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
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

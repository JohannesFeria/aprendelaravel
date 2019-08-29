Option Explicit On 
Option Strict On

#Region "/* Imports */"

Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

#End Region

Public Class TipoTerceroBM
    Inherits InvokerCOM

#Region "/* Constructor */"

    Public Sub New()

    End Sub

#End Region

#Region "/* Funciones Personalizadas */"

    Public Function SeleccionarPorFiltro(ByVal codigoTipoTercero As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoTerceroBE

        Dim oTipoTerceroDAM As New TipoTerceroDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {codigoTipoTercero, descripcion, situacion, dataRequest}

        Dim oTipoTerceroBE As TipoTerceroBE

        Try

            intCodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)

            oTipoTerceroBE = oTipoTerceroDAM.SeleccionarPorFiltro(codigoTipoTercero, descripcion, situacion)

            RegistrarAuditora(parameters)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If

        End Try

        Return oTipoTerceroBE

    End Function

    Public Function Seleccionar(ByVal codigoTipoTercero As String, ByVal dataRequest As DataSet) As TipoTerceroBE

        Dim oTipoTerceroDAM As New TipoTerceroDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {codigoTipoTercero, dataRequest}

        Dim oTipoTerceroBE As TipoTerceroBE

        Try

            intCodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)

            oTipoTerceroBE = oTipoTerceroDAM.Seleccionar(codigoTipoTercero)

            RegistrarAuditora(parameters)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If

        End Try

        Return oTipoTerceroBE

    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As TipoTerceroBE

        Dim oTipoTerceroDAM As New TipoTerceroDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {dataRequest}

        Dim oTipoTerceroBE As TipoTerceroBE

        Try

            intCodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)

            oTipoTerceroBE = oTipoTerceroDAM.Listar()

            RegistrarAuditora(parameters)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If

        End Try

        Return oTipoTerceroBE

    End Function

#End Region

End Class

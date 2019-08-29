Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class GenerarAccessBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function EjecutarDTSAccess(ByVal Paquete As String, ByVal Servidor As String, ByVal RutaArchivo As String, ByVal dataRequest As DataSet) As String
        Dim strGenerarAccess As String

        Dim parameters As Object() = {Paquete, Servidor, RutaArchivo}
        Try
            Dim oGenerarAccessDAM As New GenerarAccessDAM
            strGenerarAccess = oGenerarAccessDAM.EjecutarDTSAccess(Paquete, Servidor, RutaArchivo, dataRequest)

            Return strGenerarAccess
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function InsertarControl(ByVal RutaArchivo As String, ByVal dataRequest As DataSet) As String
        Dim strGenerarAccess As String

        Dim parameters As Object() = {RutaArchivo}
        Try
            Dim oGenerarAccessDAM As New GenerarAccessDAM
            strGenerarAccess = oGenerarAccessDAM.InsertarControl(RutaArchivo, dataRequest)

            Return strGenerarAccess
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ListarControl(ByVal dataRequest As DataSet) As DataSet
        Dim dts As New DataSet
        Dim parameters As Object() = {dataRequest}

        Try
            Dim oGenerarAccessDAM As New GenerarAccessDAM
            dts = oGenerarAccessDAM.ListarControl(dataRequest)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dts
    End Function
End Class

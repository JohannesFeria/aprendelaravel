Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class InventarioCartasBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oInventarioCartas As InventarioCartasBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oInventarioCartas, dataRequest}
        Try
            Dim oInventarioCartasDAM As New InventarioCartasDAM
            oInventarioCartasDAM.Insertar(oInventarioCartas, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Eliminar(ByVal codigoInventario As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoInventario, dataRequest}
        Try
            Dim oInventarioCartasDAM As New InventarioCartasDAM
            oInventarioCartasDAM.Eliminar(codigoInventario, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Seleccionar(ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim oInventarioCartasDAM As New InventarioCartasDAM
            Dim dsAux As DataSet
            dsAux = oInventarioCartasDAM.Seleccionar()
            RegistrarAuditora(parameters)
            Return dsAux
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Sub InicializarInventarioCartas(ByRef oRow As InventarioCartasBE.InventarioCartasRow, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim oInventarioCartasDAM As New InventarioCartasDAM
            oInventarioCartasDAM.InicializarInventarioCartas(oRow)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
End Class

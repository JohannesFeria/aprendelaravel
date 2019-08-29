Imports System
Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class PlazaBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Function Listar(ByVal dataRequest As DataSet) As PlazaBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim dsPlaza As PlazaBE = New PlazaDAM().Listar(dataRequest)
            If Not dataRequest Is Nothing Then RegistrarAuditora(parameters)
            Return dsPlaza
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarxOrden(ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim dsPlaza As DataSet = New PlazaDAM().ListarxOrden()
            If Not dataRequest Is Nothing Then RegistrarAuditora(parameters)
            Return dsPlaza
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarDataset(ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim dsPlaza As DataTable = New PlazaDAM().ListarDataset(dataRequest)
            If Not dataRequest Is Nothing Then RegistrarAuditora(parameters)
            Return dsPlaza
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
End Class
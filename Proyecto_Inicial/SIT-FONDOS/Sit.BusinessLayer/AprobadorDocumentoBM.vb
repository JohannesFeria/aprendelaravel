Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class AprobadorDocumentoBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Sub InicializarAprobadorDocumento(ByRef oRow As AprobadorDocumentoBE.AprobadorDocumentoRow)
        Try
            Dim oAprobadorDocumentoDAM As New AprobadorDocumentoDAM
            oAprobadorDocumentoDAM.InicializarAprobadorDocumento(oRow)
            'Luego de terminar la ejecución de métodos(sin errores)             
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub


#Region "Metodos Transaccionales"
    Public Function Insertar(ByVal oAprobadorDocumentoBE As AprobadorDocumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oAprobadorDocumentoBE, dataRequest}
        Try
            Dim oAproborDocumentoDAM As New AprobadorDocumentoDAM
            Return oAproborDocumentoDAM.Insertar(oAprobadorDocumentoBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Modificar(ByVal oAprobadorDocumentoBE As AprobadorDocumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oAprobadorDocumentoBE, dataRequest}
        Try
            Dim oAproborDocumentoDAM As New AprobadorDocumentoDAM
            Return oAproborDocumentoDAM.Modificar(oAprobadorDocumentoBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Eliminar(ByVal codigoInterno As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoInterno, dataRequest}
        Try
            Dim oAproborDocumentoDAM As New AprobadorDocumentoDAM
            Return oAproborDocumentoDAM.Eliminar(codigoInterno, dataRequest)
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

#Region "Metodos No Transaccionales"
    Public Function SeleccionarPorFiltro(ByVal oAprobadorDocumentoBE As AprobadorDocumentoBE, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oAprobadorDocumentoBE, dataRequest}
        Try
            Dim oAproborDocumentoDAM As New AprobadorDocumentoDAM
            Return oAproborDocumentoDAM.SeleccionarPorFiltro(oAprobadorDocumentoBE, dataRequest)
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

End Class

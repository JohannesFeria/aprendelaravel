Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class FirmaDocumentoDetBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Sub InicializarFirmaDocumentoDet(ByRef oRow As FirmaDocumentoDetBE.FirmaDocumentoDetRow)
        Try
            Dim oFirmaDocumentoDetDAM As New FirmaDocumentoDetDAM
            oFirmaDocumentoDetDAM.InicializarFirmaDocumentoDet(oRow)
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
    Public Function Insertar(ByVal codFirmaDocumento As Decimal, ByVal login As String, ByRef validaFirma As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codFirmaDocumento, login, validaFirma, dataRequest}
        Try
            Dim oFirmaDocumentoDetDAM As New FirmaDocumentoDetDAM
            Return oFirmaDocumentoDetDAM.Insertar(codFirmaDocumento, login, validaFirma, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function Modificar(ByVal codFirmaDocumento As Decimal, ByVal situacion As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codFirmaDocumento, situacion, dataRequest}
        Try
            Dim oFirmaDocumentoDetDAM As New FirmaDocumentoDetDAM
            Return oFirmaDocumentoDetDAM.Modificar(codFirmaDocumento, situacion, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Eliminar(ByVal codFirmaDocumento As Decimal, ByVal login As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codFirmaDocumento, login, dataRequest}
        Try
            Dim oFirmaDocumentoDetDAM As New FirmaDocumentoDetDAM
            Return oFirmaDocumentoDetDAM.Eliminar(codFirmaDocumento, login, dataRequest)
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

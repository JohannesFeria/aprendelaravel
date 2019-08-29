Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class FirmaDocumentoBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Sub InicializarFirmaDocumento(ByRef oRow As FirmaDocumentoBE.FirmaDocumentoRow)
        Try
            Dim oFirmaDocumentoDAM As New FirmaDocumentoDAM
            oFirmaDocumentoDAM.InicializarFirmaDocumento(oRow)
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
    Public Function Insertar(ByVal oFirmaDocumentoBE As FirmaDocumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oFirmaDocumentoBE, dataRequest}
        Try
            Dim oFirmaDocumentoDAM As New FirmaDocumentoDAM
            Return oFirmaDocumentoDAM.Insertar(oFirmaDocumentoBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Modificar(ByVal oFirmaDocumentoBE As FirmaDocumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oFirmaDocumentoBE, dataRequest}
        Try
            Dim oFirmaDocumentoDAM As New FirmaDocumentoDAM
            Return oFirmaDocumentoDAM.Modificar(oFirmaDocumentoBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Eliminar(ByVal codFirmaDocumento As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codFirmaDocumento, dataRequest}
        Try
            Dim oFirmaDocumentoDAM As New FirmaDocumentoDAM
            Return oFirmaDocumentoDAM.Eliminar(codFirmaDocumento, dataRequest)
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
    Public Function SeleccionarPorFiltro(ByVal fecha As Decimal, _
                                        ByVal codReporte As String, _
                                        ByVal codCategReporte As String, _
                                        ByVal codCargoFirmante As Decimal, _
                                        ByVal codigoOrden As String, _
                                        ByVal codigoPortafolioSBS As String, _
                                        ByVal estado As String, _
                                        ByVal codigoMercado As String, _
                                        ByVal codigoOperacion As String, _
                                        ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, codReporte, _
                                    codCategReporte, codCargoFirmante, _
                                    codigoOrden, dataRequest}
        Try
            Dim oFirmaDocumentoDAM As New FirmaDocumentoDAM
            Return oFirmaDocumentoDAM.SeleccionarPorFiltro(fecha, codReporte, _
                                                        codCategReporte, _
                                                        codCargoFirmante, _
                                                        codigoOrden, _
                                                        codigoPortafolioSBS, _
                                                        estado, _
                                                        codigoMercado, _
                                                        codigoOperacion, _
                                                        dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function SeleccionarPorCodigo(ByVal codFirmaDocumento As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codFirmaDocumento, dataRequest}

        Try
            Dim oFirmaDocumentoDAM As New FirmaDocumentoDAM
            Return oFirmaDocumentoDAM.SeleccionarPorCodigo(codFirmaDocumento, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function VerificaPermisoFirma(ByVal login As String, ByVal clave As String, ByVal dataRequest As DataSet) As Decimal
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {login, dataRequest}

        Try
            Dim oFirmaDocumentoDAM As New FirmaDocumentoDAM
            Return oFirmaDocumentoDAM.VerificaPermisoFirma(login, clave)
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

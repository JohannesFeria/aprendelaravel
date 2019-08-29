Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class ArchivoVAXBMIDASBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function Insertar(ByVal objArchivosVAXBMIDAS As ArchivosVAXBMIDASBE.ArchivosVAXBMIDASRow, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {objArchivosVAXBMIDAS, dataRequest}
        Try
            Dim daArchivosVAXBMIDAS As New ArchivosVAXBMIDASDAM
            strCodigo = daArchivosVAXBMIDAS.Insertar(objArchivosVAXBMIDAS, dataRequest)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return strCodigo
    End Function

    Public Function SeleccionarPorPortafolioFecha(ByVal portafolioSBS As String, ByVal fechaCarga As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {portafolioSBS, fechaCarga, dataRequest}
        Dim oDSArchivosVAXBMIDAS As New DataSet
        Try
            oDSArchivosVAXBMIDAS = New ArchivosVAXBMIDASDAM().SeleccionarPorPortafolioFecha(portafolioSBS, fechaCarga, dataRequest)
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
        Return oDSArchivosVAXBMIDAS
    End Function

End Class
